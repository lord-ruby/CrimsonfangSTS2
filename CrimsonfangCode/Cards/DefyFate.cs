using Crimsonfang.CrimsonfangCode.Utilities;
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

public class DefyFate() : CrimsonfangCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        List<CardPoolModel> cards = Owner.RunState.Players.Select(p => p).Where(p => p != Owner)
            .Select(p => p.Character.CardPool).ToList();
        List<CardModel> realCards = CardFactory.GetDistinctForCombat(Owner, new CardFilters().FilterFromPools(cards), 3, Owner.RunState.Rng.CombatCardGeneration).ToList();
        if (IsUpgraded)
        {
            foreach (CardModel c in realCards)
            {
                CardCmd.Upgrade(c);
            }
        }

        CardModel card1 = await CardSelectCmd.FromChooseACardScreen(context, realCards, Owner, true);
        
        if (card1 == null)
            return;
        CardPileAddResult combat = await CardPileCmd.AddGeneratedCardToCombat(card1, PileType.Hand, Owner);
    }
}