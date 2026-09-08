using UnityEngine;

public abstract class Modifier
{
    public string Name { get; set; }
    public string Description { get; set; }
    public ModifierType Type { get; set; }

    //Para que el modificador tenga acceso a la carta que lo contiene
    public CardInstance FatherCard { get; set; }
    //El indice depende de la spritesheet
    public int Index { get; set; }

    //Variable general para el efecto sobre la puntuacion del aumento
    //Puede ser util declararla aqui en caso de que se quiera modificar de forma dinamica
    protected int power;
    protected ColorText ct = new ColorText();

    //Para aquellos modificadores que solo funcionen con ciertos tipos de cartas
    public virtual bool ModificadorAplicable(CardInstance card)
    {
        return true;
    }
    public virtual int CompararContraCartaRival(CardInstance rival, Palo paloMuestra)
    {
        //-1 indica que la función no está definida
        return -1;
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


    public virtual int CompararValorEnCartaCante(Valor otroValor)
    {
        //-1 indica que la función no está definida
        return -1;
    }

    public virtual void AntesDeEvaluarCante(ScoreManager sm)
    {

    }

    public virtual void AntesDePuntuarCarta(ScoreManager sm, string posicion)
    {

    }

    public virtual void DespuesDePuntuarCarta(ScoreManager sm)
    {

    }

    public virtual void Activate(ScoreManager sm)
    {

    }

    public virtual void PuntuarCarta(ScoreManager sm, string posicion)
    {
        //Por defecto, los modificadores puntuan en cualquier situacion donde la carta original puntue
        switch (posicion)
        {
            case "baza":
                Activate(sm);
                break;

            case "cante":
                Activate(sm);
                break;

            case "rival":
                Activate(sm);
                break;

            default:
                break;
        }
    }

    public virtual void EfectoCartaMano(ScoreManager sm)
    {

    }

    public virtual void EfectoCartaDescartada(ScoreManager sm)
    {
        //Si la baza no se gana, no se deberia de puntuar por defecto
        if (!sm.EsBazaGanada())
            return;
    }
}
