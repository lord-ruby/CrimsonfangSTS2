using Crimsonfang.CrimsonfangCode.Powers;
using Crimsonfang.CrimsonfangCode.Utilities;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace Crimsonfang.CrimsonfangCode.Cards;

public class Hedonism() : CrimsonfangCard(0, CardType.Skill, CardRarity.Rare, TargetType.None)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>(){new EnergyVar(1), new DynamicVar("Cards", 1)};

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play)
    {
        if (!Spellcasting.IsBeingCast)
        {
            await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
            await CardPileCmd.Draw(context, Owner);
        }

        await base.OnPlay(context, play);
    }

    public override async Task OnCast(PlayerChoiceContext context, CardPlay play)
    {
        await PowerCmd.Apply<HedonismPower>(context, Owner.Creature, 1M, Owner.Creature, this);
        await CardCmd.Exhaust(context, this);
    }

    protected override void OnUpgrade() => this.DynamicVars.Energy.UpgradeValueBy(1);
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>()
    {
        HoverTipFactory.Static(CrimsonfangHoverTips.Conjure),
        HoverTipFactory.Static(CrimsonfangHoverTips.OnCast),
        HoverTipFactory.Static(CrimsonfangHoverTips.CastStatic),
        HoverTipFactory.FromKeyword(CardKeyword.Exhaust),
    };
    protected override HashSet<CardTag> CanonicalTags => new() { Conjure };
}