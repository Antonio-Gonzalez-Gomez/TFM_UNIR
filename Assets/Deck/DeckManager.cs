using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    //Los puntos que vale cada carta (de base) es un parametro configurable desde el inspector
    //No es posible serializar diccionarios, por lo que se convierte una lista de entries
    //Se ha preferido esta implementacion a hardcodear el diccionario
    [SerializeField] List<PuntuacionBase> puntuacionesBase;
    private Dictionary<Valor, int> puntuacionesDict;

    private List<CardInstance> mazoRobar;
    private List<CardInstance> mazoDescartes;
    private List<CardInstance> cartasMano;
    private CardInstance cartaMuestra;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitDeck();
        DeckShuffler.Shuffle(mazoRobar);
        foreach (CardInstance carta in mazoRobar)
        {
            print(carta.cartaBase);
        }
    }

    private void InitDeck()
    {
        mazoRobar = new List<CardInstance>();
        mazoDescartes = new List<CardInstance>();
        cartasMano = new List<CardInstance>();

        puntuacionesDict = puntuacionesBase.ToDictionary(e => e.carta, e => e.puntos);

        //Se genera una baraja española (40 cartas, 10 de cada palo, 4 de cada valor)
        //Iterando sobre los enum definidos
        foreach (Palo palo in (Palo[]) Enum.GetValues(typeof(Palo)))
        {
            foreach (Valor valor in (Valor[])Enum.GetValues(typeof(Valor)))
            {
                if(puntuacionesDict.TryGetValue(valor, out int puntos))
                {
                    CardData data = new CardData(valor, palo, puntos);
                    CardInstance card = new CardInstance(data);
                    mazoRobar.Add(card);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
