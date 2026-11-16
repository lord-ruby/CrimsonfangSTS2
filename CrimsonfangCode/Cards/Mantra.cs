using Crimsonfang.CrimsonfangCode.Utilities;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Crimsonfang.CrimsonfangCode.Cards;

public class Mantra() : CrimsonfangCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CastVar(1), new CardsVar(1)];

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        await CardPileCmd.Draw(context, DynamicVars.Cards.BaseValue, Owner);
        await Spellcasting.CastX(DynamicVars["CastAmount"], this, context, cardPlay);
    }
    protected override void OnUpgrade() => DynamicVars.Cards.UpgradeValueBy(1);

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(CrimsonfangHoverTips.CastDynamic, DynamicVars["CastAmount"])];
}