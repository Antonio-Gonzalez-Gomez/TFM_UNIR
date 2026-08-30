using UnityEngine;

public class M_Clone : Modifier
{
    public M_Clone()
    {
        this.Name = "Clonación";
        this.Description = "Al puntuar como cante, la siguiente carta del cante puntúa otra vez";
        this.Type = ModifierType.Beta;
    }

    public override void PuntuarCarta(ScoreManager sm, string posicion)
    {
        if (posicion == "cante")
        {
            int currentCardIndex = sm.scoringCards.IndexOf(FatherCard);
            //Si no es la última carta del cante
            if (sm.scoringCards.Count > currentCardIndex + 1) 
            {
                CardInstance nextCard = sm.scoringCards[currentCardIndex + 1];
                nextCard.replay++;
            }
        }
    }

}
