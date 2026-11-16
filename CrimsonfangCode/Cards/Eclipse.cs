using Crimsonfang.CrimsonfangCode.Utilities;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace Crimsonfang.CrimsonfangCode.Cards;

public class Eclipse() : CrimsonfangCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>(){new BlockVar(10M, ValueProp.Move)};

    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>()
    {
        HoverTipFactory.Static(CrimsonfangHoverTips.Conjure),
        HoverTipFactory.Static(CrimsonfangHoverTips.OnCast),
        HoverTipFactory.Static(CrimsonfangHoverTips.CastStatic),
    };

    public override async Task OnCast(PlayerChoiceContext context, CardPlay play)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, play);
    }

    protected override void OnUpgrade() => this.DynamicVars.Block.UpgradeValueBy(6M);
    
    protected override HashSet<CardTag> CanonicalTags => new() { Conjure };
}