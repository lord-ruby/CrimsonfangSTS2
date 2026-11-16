using Crimsonfang.CrimsonfangCode.Utilities;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace Crimsonfang.CrimsonfangCode.Cards;

public class FlameVolley() : CrimsonfangCard(1, CardType.Attack, CardRarity.Common, TargetType.None)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>(){new DamageVar(3M, ValueProp.Move)};

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>()
    {
        HoverTipFactory.Static(CrimsonfangHoverTips.Conjure),
        HoverTipFactory.Static(CrimsonfangHoverTips.OnCast),
        HoverTipFactory.Static(CrimsonfangHoverTips.CastStatic),
    };

    public override async Task OnCast(PlayerChoiceContext context, CardPlay play)
    {
        await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard(this, play).TargetingAllOpponents(CombatState).WithHitCount(2).WithHitFx("vfx/vfx_attack_slash").Execute(context);
    }

    protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(3M);
    
    protected override HashSet<CardTag> CanonicalTags => new() { Conjure };
}