using BaseLib.Abstracts;
using BaseLib.Utils;
using Crimsonfang.CrimsonfangCode.Character;
using Crimsonfang.CrimsonfangCode.Utilities;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace Crimsonfang.CrimsonfangCode.Potions;

public class SpectralPotion : CrimsonfangPotion
{
    public override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(CrimsonfangHoverTips.Conjure)];
    public override PotionRarity Rarity =>  PotionRarity.Common;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.Self;

    protected override async Task OnUse(PlayerChoiceContext context, Creature? target)
    {
        IEnumerable<CardModel> cards = Owner.Character.CardPool.AllCards;
        List<CardModel> list2 = CardFactory.GetDistinctForCombat(Owner, cards, 2, Owner.RunState.Rng.CombatCardGeneration).ToList<CardModel>();
        foreach (CardModel c in list2)
        {
            await Spellcasting.Conjure(context, c);
        }
    }
}