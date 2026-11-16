using Crimsonfang.CrimsonfangCode.Character;
using Crimsonfang.CrimsonfangCode.Powers;
using Crimsonfang.CrimsonfangCode.Utilities;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace Crimsonfang.CrimsonfangCode.Cards;

public class FountainOfYouth() : CrimsonfangCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.None)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(2)];

    public override async Task OnCast(PlayerChoiceContext context, CardPlay cardPlay)
    {
        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
    }
    
    protected override void OnUpgrade() => DynamicVars.Energy.UpgradeValueBy(1);
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.ForEnergy(this),
        HoverTipFactory.Static(CrimsonfangHoverTips.Conjure),
        HoverTipFactory.Static(CrimsonfangHoverTips.OnCast),
        HoverTipFactory.Static(CrimsonfangHoverTips.CastStatic),
    ];
    protected override HashSet<CardTag> CanonicalTags => new() { Conjure };
}