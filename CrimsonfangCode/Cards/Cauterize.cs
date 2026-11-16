using BaseLib.Utils;
using Crimsonfang.CrimsonfangCode.Utilities;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Crimsonfang.CrimsonfangCode.Cards;

[Pool(typeof(StatusCardPool))]
public class Cauterize() : CrimsonfangCard(1, CardType.Status, CardRarity.Status, TargetType.None)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, CardKeyword.Retain];
    
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        CardPile pile = PileType.Hand.GetPile(Owner);
        CardModel card2 = Owner.RunState.Rng.CombatCardSelection.NextItem(pile.Cards);
        if (card2 == null)
            return;
        await CardCmd.Exhaust(context, card2);
    }
    public override int MaxUpgradeLevel => 0;
}