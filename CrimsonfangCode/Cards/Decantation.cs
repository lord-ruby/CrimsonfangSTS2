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

public class Decantation() : CrimsonfangCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips  {
        get
        {
            if (IsUpgraded)
            {
                return new List<IHoverTip>() { HoverTipFactory.FromKeyword(CardKeyword.Retain) };
            }

            return new List<IHoverTip>();
        }
    }
    public CardModel? _mockGeneratedCard;
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        CardModel card1;
        if (_mockGeneratedCard == null)
        {
            List<CardPoolModel> list1 = Owner.UnlockState.CharacterCardPools.ToList<CardPoolModel>();
            if (list1.Count > 1)
                list1.Remove(Owner.Character.CardPool);
            IEnumerable<CardModel> cards = list1.SelectMany(e => e.AllCards);
            List<CardModel> list2 = CardFactory.GetDistinctForCombat(Owner, cards, 2, Owner.RunState.Rng.CombatCardGeneration).ToList<CardModel>();
            if (IsUpgraded)
            {
                foreach (CardModel c in list2)
                {
                    c.AddKeyword(CardKeyword.Retain);
                }
            }

            card1 = await CardSelectCmd.FromChooseACardScreen(context, list2, Owner, true);
        }
        else
        {
            card1 = _mockGeneratedCard;
            if (IsUpgraded) card1.AddKeyword(CardKeyword.Retain);
        }
        if (card1 == null)
            return;
        CardPileAddResult combat = await CardPileCmd.AddGeneratedCardToCombat(card1, PileType.Hand, Owner);
    }
    public void MockGeneratedCard(CardModel card)
    {
        this.AssertMutable();
        this._mockGeneratedCard = card;
    }
}