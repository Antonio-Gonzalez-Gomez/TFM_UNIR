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

        //Para los aumentos/modificadores, aqui iria la logica, implementar a parte desde CardInstance?

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
            //Ya que tanto los caballos como los reyes valen 9 puntos, se comprueba a parte esta casuística
            if (playerCard.cartaBase.Valor == Valor.Rey && rivalCard.cartaBase.Valor == Valor.Caballo)
                return true;

            //Si ambas cartas son de la muestra, gana la de mayor valor (1 > 3 > Rey > etc.)
            PuntuacionesCartas.dict.TryGetValue(playerCard.cartaBase.Valor, out int valorPlayer);
            PuntuacionesCartas.dict.TryGetValue(rivalCard.cartaBase.Valor, out int valorRival);
            return valorPlayer > valorRival;
        }
    }

    public List<CardInstance> scoringCards;

    //Función que comprueba cual es el cante realizado por el jugador
    //Le da prioridad a los cantes de mayor valor (Tute > Socare real > Socare > etc.)
    //Reinicia y almacena en scoringCards las cartas que forman el cante
    public Cante EvaluarCante()
    {
        scoringCards = new List<CardInstance>();

        //Se separan las cartas que pueden formar los cantes (sotas, caballos y reyes) para facilitar las comprobaciones
        List<CardInstance> sotas = canteSpot.cardList.FindAll(x => x.cartaBase.Valor == Valor.Sota);
        List<CardInstance> caballos = canteSpot.cardList.FindAll(x => x.cartaBase.Valor == Valor.Caballo);
        List<CardInstance> reyes = canteSpot.cardList.FindAll(x => x.cartaBase.Valor == Valor.Rey);

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

        //TEMPORAL PARA PROBAR AUMENTOS
        CardInstance bazaCard = bazaSpot.cardList[0];
        bazaCard.alphaMod = new M_PlusBonus();
        //

        //Se puntua la carta de la baza
        bazaCard.ScoreCard(this);

        //Y luego las cartas del cante
        foreach (CardInstance card in scoringCards)
        {
            card.alphaMod = new M_PlusValue();
            card.ScoreCard(this);
        }

        //Y aqui todo lo referente a otros efectos (aumentos o modificadores de cartas en mano)
        Debug.Log(puntosJugada.ToString() + " x " + valorJugada.ToString() + " + " + bonusJugada.ToString());
        return puntosJugada * valorJugada + bonusJugada;
    }
}
