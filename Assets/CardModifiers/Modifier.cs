using UnityEngine;

public abstract class Modifier
{
    public string Name { get; set; }
    public string Description { get; set; }
    public ModifierType Type { get; set; }
    //Para que el modificador tenga acceso a la carta que lo contiene
    public CardInstance FatherCard { get; set; }

    //Variable general para el efecto sobre la puntuacion del aumento
    //Puede ser util declararla aqui en caso de que se quiera modificar de forma dinamica
    protected int points;

    //Para aquellos modificadores que solo funcionen con ciertos tipos de cartas
    public virtual bool ModificadorAplicable(CardInstance card)
    {
        return true;
    }

    public virtual void UpdateReplay(ScoreManager sm)
    {

    }

    public virtual int CompararPaloConCartaCante(CardInstance card)
    {
        //-1 indica que la función no está definida
        return -1;
    }

    public virtual int CompararPaloConMuestraCante(Palo palo)
    {
        //-1 indica que la función no está definida
        return -1;
    }

    public virtual int CompararValorContraCartaRival(Valor otroValor)
    {
        //-1 indica que la función no está definida
        return -1;
    }
    public virtual int CompararValorEnCartaCante(Valor otroValor)
    {
        //-1 indica que la función no está definida
        return -1;
    }

    public virtual void PuntuarCarta(ScoreManager sm, string posicion)
    {

    }

    public virtual void EfectoCartaMano(ScoreManager sm)
    {

    }

    public virtual void EfectoCartaDescartada(ScoreManager sm)
    {

    }
}
