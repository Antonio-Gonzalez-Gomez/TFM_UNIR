using System;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("Card Spots")]
    [SerializeField] DragCardSpot manoSpot;
    [SerializeField] DragCardSpot muestraSpot;
    [SerializeField] DragCardSpot rivalSpot;
    [SerializeField] DragCardSpot canteSpot;
    [SerializeField] DragCardSpot bazaSpot;

    public event Action<int> pointScoreAction;
    public event Action<int> valueScoreAction;
    public event Action<int> bonusScoreAction;

    //Función que comprueba si la carta del jugador gana la baza contra la del rival
    public bool EsBazaGanada()
    {
        //Se asume que las cartas correspondientes han sido correctamente jugadas
        CardInstance playerCard = bazaSpot.cardList[0];
        CardInstance rivalCard = rivalSpot.cardList[0];

        Palo paloMuestra = muestraSpot.cardList[0].cartaBase.Palo;
        Palo paloPlayer = playerCard.cartaBase.Palo;
        Palo paloRival = rivalCard.cartaBase.Palo;

        //Aqui se inyectaría la lógica de los aumentos (sobre el palo de las cartas)

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


        //Aqui se inyectaría la lógica de los aumentos (sobre el valor de las cartas)

        else
        {
            //Se usa un método a parte para verificar qué carta gana
            //En caso de empate??
            return playerCard.CompararValor(rivalCard.cartaBase.Valor) == 1;
        }
    }

    public List<CardInstance> scoringCards;

    //Función que comprueba cual es el cante realizado por el jugador
    //Le da prioridad a los cantes de mayor valor (Tute > Socare real > Socare > etc.)
    //Reinicia y almacena en scoringCards las cartas que forman el cante
    public Cante EvaluarCante()
    {
        scoringCards = new List<CardInstance>();

        //Como inyectar la interaccion con aumentos aqui?

        //Modificador espejo: como??
        //Se separan las cartas que pueden formar los cantes (sotas, caballos y reyes) para facilitar las comprobaciones
        List<CardInstance> sotas = canteSpot.cardList.FindAll(x => x.CompararValor(Valor.Sota) == 0);
        List<CardInstance> caballos = canteSpot.cardList.FindAll(x => x.CompararValor(Valor.Caballo) == 0);
        List<CardInstance> reyes = canteSpot.cardList.FindAll(x => x.CompararValor(Valor.Rey) == 0);

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
                        if (sota.cartaBase.Palo == caballo.cartaBase.Palo && sota.cartaBase.Palo == rey.cartaBase.Palo)
                        {
                            sotaCante = sota;
                            caballoCante = caballo;
                            reyCante = rey;
                            //Si son de la muestra, se devuelve el Socare real
                            if (sotaCante.cartaBase.Palo == paloMuestra)
                            {
                                //mantener orden?
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
                    if (caballo.cartaBase.Palo == rey.cartaBase.Palo)
                    {
                        caballoCante = caballo;
                        reyCante = rey;
                        //Si son de la muestra, se devuelven las 40
                        if (caballoCante.cartaBase.Palo == paloMuestra)
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
            return Cante.Infantería;
        }

        //Si no hay otro cante posible
        //(añadir la carta mas alta como scoringCard?)
        return Cante.Ninguno;
    }

    //Los eventos no se pueden invocar fuera de esta clase (aunque sean publicos)
    public void InvokePointScore(int points)
    {
        pointScoreAction?.Invoke(points);
    }

    public void InvokeValueScore(int value)
    {
        valueScoreAction?.Invoke(value);
    }

    public void InvokeBonusScore(int bonus)
    {
        bonusScoreAction?.Invoke(bonus);
    }

    public int puntosJugada;
    public int valorJugada;
    public int bonusJugada;
    public int CalculateScore()
    {
        Cante cante = EvaluarCante();
        puntosJugada = 0;
        PuntuacionesCantes.valores.TryGetValue(cante, out valorJugada);
        PuntuacionesCantes.bonus.TryGetValue(cante, out bonusJugada);

        List<CardInstance> discardedCards = new List<CardInstance>(canteSpot.cardList);
        discardedCards.RemoveAll(x => scoringCards.Contains(x));

        CardInstance bazaCard = bazaSpot.cardList[0];
        //TEMPORAL PARA PROBAR AUMENTOS
        bazaCard.AddModifier(new M_PlusPoints(), ModifierType.Alpha);

        //Se puntua la carta de la baza
        bazaCard.PuntuarCartaBaza(this);

        //Luego las cartas del cante
        foreach (CardInstance card in scoringCards)
        {
            //TEMPORAL PARA PROBAR AUMENTOS
            card.AddModifier(new M_PlusValue(), ModifierType.Alpha);

            card.PuntuarCartaCante(this);
        }

        //Finalmente la carta del oponente
        CardInstance rivalCard = rivalSpot.cardList[0];
        rivalCard.PuntuarCartaRival(this);

        //Si alguna carta en mano tiene modificador, tambien se le puntua
        foreach (CardInstance card in manoSpot.cardList)
        {
            card.PuntuarCartaMano(this);
        }

        //De forma similar, las cartas no puntuadas en el cante pueden tener modificadores
        foreach (CardInstance card in discardedCards)
        {
            card.DescartarCarta(this);
        }

        //Y aqui todo lo referente a otros efectos (aumentos)
        Debug.Log(puntosJugada.ToString() + " x " + valorJugada.ToString() + " + " + bonusJugada.ToString());

        //Mover las scoring cards al spot de descartes tambien

        return puntosJugada * valorJugada + bonusJugada;
    }
}
