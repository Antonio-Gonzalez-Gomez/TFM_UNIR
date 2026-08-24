using UnityEngine;

public abstract class Modifier
{
    public string Name { get; set; }
    public string Description { get; set; }
    public ModifierType Type { get; set; }

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

    public virtual void PuntuarCartaBaza(ScoreManager sm)
    {

    }

    public virtual void PuntuarCartaCante(ScoreManager sm)
    {

    }

    public virtual void PuntuarCartaRival(ScoreManager sm)
    {

    }

    public virtual void EfectoCartaMano(ScoreManager sm)
    {

    }

    public virtual void EfectoCartaDescartada(ScoreManager sm)
    {

    }
}
