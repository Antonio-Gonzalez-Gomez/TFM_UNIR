using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] public DeckManager deckManager;
    [SerializeField] public DragAugmentSpot augmentSpot;

    [Header("Card Spots")]
    [SerializeField] public DragCardSpot manoSpot;
    [SerializeField] public DragCardSpot mazoRobarSpot;
    [SerializeField] public DragCardSpot muestraSpot;
    [SerializeField] public DragCardSpot rivalSpot;
    [SerializeField] public DragCardSpot canteSpot;
    [SerializeField] public DragCardSpot bazaSpot;

    public event Func<DragController, int, bool, Task> PointScoreAction;
    public event Func<DragController, int, bool, Task> ValueScoreAction;
    public event Func<DragController, int, bool, Task> BonusScoreAction;
    public event Func<Task> ScoreReadyAction;

    public int targetScore = 500;
    public int totalScore = 0;
    public int remainingHands = 5;

    private void Start()
    {
        //TEMPORAL PARA PROBAR AUMENTOS
        augmentSpot.AddAugment(new A_CanteBonus(Cante.Infanteria));
        augmentSpot.AddAugment(new A_CanteBonus(Cante.LasVeinte));
        augmentSpot.AddAugment(new A_CanteBonus(Cante.LasCuarenta));
        augmentSpot.AddAugment(new A_CanteBonus(Cante.TutePartido));
        augmentSpot.AddAugment(new A_CanteBonus(Cante.Socare));
        augmentSpot.AddAugment(new A_CanteBonus(Cante.SocareReal));
        //este no deberia aparecer por falta de hueco
        augmentSpot.AddAugment(new A_CanteBonus(Cante.Tute));
    }

    //Función que comprueba si la carta del jugador gana la baza contra la del rival
    public bool EsBazaGanada()
    {
        //Se asume que las cartas correspondientes han sido correctamente jugadas
        CardInstance playerCard = bazaSpot.cardList[0];
        CardInstance rivalCard = rivalSpot.cardList[0];

        Palo paloMuestra = muestraSpot.cardList[0].cartaBase.Palo;
        Palo paloPlayer = playerCard.cartaBase.Palo;
        Palo paloRival = rivalCard.cartaBase.Palo;

        //El modificador gamma de la carta jugada como baza puede afectar al resultado de la baza
        Modifier gammaMod = playerCard.GetMod(ModifierType.Gamma);
        if (gammaMod != null)
        {
            int res = gammaMod.CompararContraCartaRival(rivalCard, paloMuestra);
            if (res != -1)
            {
                return res == 0 ? true : false;
            }
        }

        if (paloPlayer != paloRival)
        {
            //Si una de las cartas es la muestra, gana la baza
            if (paloPlayer == paloMuestra)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        else
        {
            //TODO: hacer algo en caso de empate? (== 0)
            return playerCard.CompararValor(rivalCard.cartaBase.Valor) == 1;
        }
    }

    public List<CardInstance> scoringCards;
    public List<CardInstance> discardedCards;

    //Función que comprueba cual es el cante realizado por el jugador
    //Le da prioridad a los cantes de mayor valor (Tute > Socare real > Socare > etc.)
    //Reinicia y almacena en scoringCards las cartas que forman el cante (y en discardCards las descartadas)
    public Cante EvaluarCante()
    {
        scoringCards = new List<CardInstance>();

        //Como inyectar la interaccion con aumentos aqui?

        foreach (CardInstance card in canteSpot.cardList)
        {
            Modifier gammaMod = card.GetMod(ModifierType.Gamma);
            if (gammaMod != null)
                gammaMod.AntesDeEvaluarCante(this);
        }

        //Se separan las cartas que pueden formar los cantes (sotas, caballos y reyes) para facilitar las comprobaciones
        List<CardInstance> sotas = canteSpot.cardList.FindAll(x => x.CompararValorEnCartaCante(Valor.Sota) == 0);
        List<CardInstance> caballos = canteSpot.cardList.FindAll(x => x.CompararValorEnCartaCante(Valor.Caballo) == 0);
        List<CardInstance> reyes = canteSpot.cardList.FindAll(x => x.CompararValorEnCartaCante(Valor.Rey) == 0);

        int numSotas = sotas.Count;
        int numCaballos = caballos.Count;
        int numReyes = reyes.Count;

        Palo paloMuestra = muestraSpot.cardList[0].cartaBase.Palo;

        //Tute
        if (numSotas == 4 || numCaballos == 4 || numReyes == 4)
        {
            scoringCards.AddRange(canteSpot.cardList);
            return Cante.Tute;
        }

        //Socare y Socare real
        if (numSotas >= 1 && numCaballos >= 1 && numReyes >= 1)
        {
            CardInstance sotaCante = null;
            CardInstance caballoCante = null;
            CardInstance reyCante = null;

            //Se revisa cada combinación de sota, caballo y rey
            foreach (CardInstance sota in sotas)
            {
                foreach (CardInstance caballo in caballos)
                {
                    foreach (CardInstance rey in reyes)
                    {
                        //Si hay una sota, caballo y rey del mismo palo, hay un Socare
                        //Hay que hacer 3 comparaciones debido a posibles efectos
                        //Por ejemplo, si solo la sota tiene M_Polivalence
                        //Entonces <sota = caballo> y <sota = rey> pero <caballo != rey>
                        if (sota.CompararPaloConCartaCante(caballo)
                            && sota.CompararPaloConCartaCante(rey)
                            && caballo.CompararPaloConCartaCante(rey))
                        {
                            sotaCante = sota;
                            caballoCante = caballo;
                            reyCante = rey;
                            //Si son de la muestra, se devuelve el Socare real
                            //Al igual que antes, es necesario comprobarlo de forma individual
                            if (sotaCante.CompararPaloConMuestraCante(paloMuestra)
                                && caballoCante.CompararPaloConMuestraCante(paloMuestra)
                                && reyCante.CompararPaloConMuestraCante(paloMuestra))
                            {
                                scoringCards.Add(sotaCante);
                                scoringCards.Add(caballoCante);
                                scoringCards.Add(reyCante);
                                return Cante.SocareReal;
                            }
                        }
                    }
                }
            }

            //Se comprueba que se hayan encontrado, por los menos, un Socare normal
            if (sotaCante != null)
            {
                scoringCards.Add(sotaCante);
                scoringCards.Add(caballoCante);
                scoringCards.Add(reyCante);
                return Cante.Socare;
            }
        }

        //Tute partido
        if (numSotas == 3)
        {
            scoringCards.AddRange(sotas);
            return Cante.TutePartido;
        }

        if (numCaballos == 3)
        {
            scoringCards.AddRange(caballos);
            return Cante.TutePartido;
        }

        if (numReyes == 3)
        {
            scoringCards.AddRange(reyes);
            return Cante.TutePartido;
        }

        //Las 20 y las 40
        if (numCaballos >= 1 || numReyes >= 1)
        {
            CardInstance caballoCante = null;
            CardInstance reyCante = null;
            //Se revisa cada combinación de rey y caballo (de forma similar a los Socare)
            foreach (CardInstance caballo in caballos)
            {
                foreach (CardInstance rey in reyes)
                {
                    //Si hay un rey y caballo del mismo palo, hay un cante de las 20
                    if (caballo.CompararPaloConCartaCante(rey))
                    {
                        caballoCante = caballo;
                        reyCante = rey;
                        //Si son de la muestra, se devuelven las 40
                        if (caballo.CompararPaloConMuestraCante(paloMuestra)
                            && rey.CompararPaloConMuestraCante(paloMuestra))
                        {
                            scoringCards.Add(caballoCante);
                            scoringCards.Add(reyCante);
                            return Cante.LasCuarenta;
                        }
                    }
                }
            }
            //Se comprueba que se hayan encontrado, por los menos, las 20
            if (caballoCante != null)
            {
                scoringCards.Add(caballoCante);
                scoringCards.Add(reyCante);
                return Cante.LasVeinte;
            }
        }

        //Infantería
        if (numSotas == 2)
        {
            scoringCards.AddRange(sotas);
            return Cante.Infanteria;
        }

        //Si no hay otro cante posible
        //(añadir la carta mas alta como scoringCard?)
        return Cante.Ninguno;
    }

    //Los eventos no se pueden invocar fuera de esta clase (aunque sean publicos)
    public Task InvokePointScore(DragController drag, int points, bool isAugment)
    {
        return PointScoreAction?.Invoke(drag, points, isAugment);
    }

    public Task InvokeValueScore(DragController drag, int value, bool isAugment)
    {
        return ValueScoreAction?.Invoke(drag, value, isAugment);
    }

    public Task InvokeBonusScore(DragController drag, int bonus, bool isAugment)
    {
        return BonusScoreAction?.Invoke(drag, bonus, isAugment);
    }

    public int puntosJugada;
    public int valorJugada;
    public int bonusJugada;

    //Funcion que puntua las cartas que forman el cante jugado
    //Tiene en cuenta los aumentos y modificadores presentes
    public async Task ScorePlayedHand()
    {
        Cante cante = EvaluarCante();
        puntosJugada = 0;
        CanteDicts.valores.TryGetValue(cante, out valorJugada);
        CanteDicts.bonus.TryGetValue(cante, out bonusJugada);

        discardedCards = new List<CardInstance>(canteSpot.cardList);
        discardedCards.RemoveAll(x => scoringCards.Contains(x));

        //La lista de cartas puntuadas no respeta el orden original de juego
        scoringCards.Sort((x, y) => canteSpot.cardList.IndexOf(x).
            CompareTo(canteSpot.cardList.IndexOf(y)));

        CardInstance bazaCard = bazaSpot.cardList[0];

        //Se puntua la carta de la baza
        await bazaCard.PuntuarCarta(this, "baza");

        //Luego las cartas del cante
        foreach (CardInstance card in scoringCards)
        {
            await card.PuntuarCarta(this, "cante");
        }

        //Finalmente la carta del oponente
        CardInstance rivalCard = rivalSpot.cardList[0];
        await rivalCard.PuntuarCarta(this, "rival");

        //Si alguna carta en mano tiene modificador, tambien se le puntua
        foreach (CardInstance card in manoSpot.cardList)
        {
            await card.EfectoCartaMano(this);
        }

        //De forma similar, las cartas no puntuadas en el cante pueden tener modificadores
        foreach (CardInstance card in discardedCards)
        {
            await card.EfectoCartaDescartada(this);
        }

        foreach (AugmentInstance aug in augmentSpot.augmentList)
        {
            await aug.data.PuntuarCante(cante, this);
        }

        //Y aqui todo lo referente a otros efectos (aumentos)
        Debug.Log(puntosJugada.ToString() + " x " + valorJugada.ToString() + " + " + bonusJugada.ToString());

        int score = puntosJugada * valorJugada + bonusJugada;
        totalScore += score;
        //Siguiente baza
        remainingHands -= 1;

        await ScoreReadyAction?.Invoke();
    }

    //Funcion que se ejecuta si el jugador no gana la baza
    public async void LosePlayedHand()
    {
        //Todas las cartas del cante son descartadas
        foreach (CardInstance card in canteSpot.cardList)
        {
            await card.EfectoCartaDescartada(this);
        }

        remainingHands -= 1;
        Debug.Log("Manos restantes: " + remainingHands.ToString());
        if (remainingHands == 0)
        {
            Debug.Log("Ronda perdida");
        }
        deckManager.PrepareNextHand();
    }
}
