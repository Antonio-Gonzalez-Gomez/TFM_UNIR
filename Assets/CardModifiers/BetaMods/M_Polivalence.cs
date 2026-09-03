using UnityEngine;

public class M_Polivalence : Modifier
{
    public M_Polivalence()
    {
        this.Name = "Polivalencia";
        this.Description = "Al jugar como cante, esta carta cuenta como una de todos los palos";
        this.Type = ModifierType.Beta;
        this.Index = 8;
    }

    public override int CompararPaloConCartaCante(CardInstance otraCarta)
    {
        //Siempre es igual al palo de la otra carta
        return 0;
    }

    public override int CompararPaloConMuestraCante(Palo muestra)
    {
        //Siempre es de la muestra al jugar como cante
        return 0;
    }

}
