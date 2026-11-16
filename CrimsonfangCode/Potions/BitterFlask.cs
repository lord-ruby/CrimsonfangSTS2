using BaseLib.Abstracts;
using BaseLib.Utils;
using Crimsonfang.CrimsonfangCode.Character;
using Crimsonfang.CrimsonfangCode.Utilities;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Potions;

namespace Crimsonfang.CrimsonfangCode.Potions;

public class BitterFlask : CrimsonfangPotion
{
    public override PotionRarity Rarity =>  PotionRarity.Rare;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.Self;

    protected override async Task OnUse(PlayerChoiceContext context, Creature? target)
    {
        List<CardPoolModel> list1 = Owner.UnlockState.CharacterCardPools.ToList<CardPoolModel>();
        if (list1.Count > 1)
            list1.Remove(Owner.Character.CardPool);
        IEnumerable<CardModel> cards = list1.SelectMany(e => e.AllCards);
        List<CardModel> list2 = CardFactory.GetDistinctForCombat(Owner, cards, 3, Owner.RunState.Rng.CombatCardGeneration).ToList<CardModel>();
        foreach (CardModel c in list2)
        {
            CardCmd.Upgrade(c);
        }

        var card = await CardSelectCmd.FromChooseACardScreen(context, list2, Owner, true);
        CardPileCmd.Add(card, PileType.Hand);
    }
}