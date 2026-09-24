using System.Threading.Tasks;
using UnityEngine;

public class A_CanteBonus : AugmentData
{
    Cante cante;
    int bonus;

    public A_CanteBonus(Cante cante)
    {
        this.cante = cante;
        this.bonus = CanteDicts.bonus[cante] * 5;
        this.Name = "Bonus en " + CanteDicts.text[cante];
        this.Description = ct.BonusText(bonus) + "al jugar " + CanteDicts.text[cante];
    }

    public override async Task PuntuarCante(Cante cante, ScoreManager sm)
    {
        if (cante == this.cante)
        {
            sm.bonusJugada += bonus;
            await sm.InvokeValueScore(ParentReference.drag, bonus, true);
            await Task.CompletedTask;
        }
    }
}
