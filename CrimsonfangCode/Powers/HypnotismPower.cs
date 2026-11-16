using Crimsonfang.CrimsonfangCode.Character;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;

namespace Crimsonfang.CrimsonfangCode.Powers;

public class HypnotismPower() : CrimsonfangPower()
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer,
        CardModel? cardSource, CardPlay? cardPlay) {
        var totalamount = 1M;
        if (target != this.Owner || !props.IsPoweredAttack())
            return 1M;
        if (target.GetPower<VulnerablePower>() != null && target.GetPower<VulnerablePower>().Amount > 1 && target == this.Owner)
        {
            for (int i = 0; i < target.GetPower<VulnerablePower>().Amount-1; i++)
            {
                decimal amount1 = target.GetPower<VulnerablePower>().DynamicVars["DamageIncrease"].BaseValue;
                if (dealer != null)
                {
                    PaperPhrog relic = dealer.Player?.GetRelic<PaperPhrog>();
                    if (relic != null)
                        amount1 = relic.ModifyVulnerableMultiplier(target, amount1, props, dealer, cardSource);
                }
                DebilitatePower power1 = target.GetPower<DebilitatePower>();
                if (power1 != null)
                    amount1 = power1.ModifyVulnerableMultiplier(target, amount1, props, dealer, cardSource);

                totalamount *= amount1;
            }
        }
        return totalamount;
    }
    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (side != CombatSide.Enemy)
            return;
        await PowerCmd.TickDownDuration(this);
    }
}