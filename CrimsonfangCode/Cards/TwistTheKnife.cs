using Crimsonfang.CrimsonfangCode.Utilities;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Crimsonfang.CrimsonfangCode.Cards;

public class TwistTheKnife() : CrimsonfangCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>(){new DamageVar(0M,  ValueProp.Move)};
    public static decimal TwistDamage = 0;
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play)
    {
        await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard(this, play).Targeting(play.Target).WithHitFx("vfx/vfx_attack_slash").Execute(context);
    }
    protected override void OnUpgrade() => AddKeyword(CardKeyword.Retain);
    public override async Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
    {
        TwistDamage = command.Results.SelectMany(r => r).Sum(r => r.TotalDamage) / 2;
        DynamicVars.Damage.BaseValue = TwistDamage;
    }
    public override void AfterCreated()
    {
        DynamicVars.Damage.BaseValue = TwistDamage;
    }
}