using UnityEngine;

public class M_Fortune : Modifier
{
    float prob = 1 / 7;
    int puntos = 77;
    int valor = 7;
    int bonus = 777;
    public M_Fortune()
    {
        this.points = 7;
        this.Name = "Fortuna";
        this.Description = "Al puntuar en baza o cante, probabilidad de 1/7 de recibir "
            + puntos.ToString() + " puntos, " + valor.ToString() + " al valor o " + bonus.ToString() + " puntos bonus";
        this.Type = ModifierType.Alpha;
    }

    public void Activate(ScoreManager sm)
    {
        //Son 3 llamadas independientes al RNG
        if (RNG.RandomRange(prob))
        {
            sm.puntosJugada += puntos;
            sm.InvokeValueScore(puntos);
        }

        if (RNG.RandomRange(prob))
        {
            sm.valorJugada += valor;
            sm.InvokeValueScore(valor);
        }

        if (RNG.RandomRange(prob))
        {
            sm.bonusJugada += bonus;
            sm.InvokeValueScore(bonus);
        }
    }

    //TODO: Refactorizar Modifier para que PuntuarCarta sea siempre esto y Activate sea un metodo abstracto??
    //Revisar al terminar de implementar el resto de modificadores
    public override void PuntuarCarta(ScoreManager sm, string posicion)
    {
        switch (posicion)
        {
            case "baza":
                Activate(sm);
                break;

            case "cante":
                Activate(sm);
                break;

            default:
                break;
        }
    }
}