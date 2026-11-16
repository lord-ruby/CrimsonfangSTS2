using BaseLib.Abstracts;
using BaseLib.Utils;
using Crimsonfang.CrimsonfangCode.Character;
using Crimsonfang.CrimsonfangCode.Utilities;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace Crimsonfang.CrimsonfangCode.Potions;

public class CrystallineSolution : CrimsonfangPotion
{
    public override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.ReplayStatic)];
    public override PotionRarity Rarity =>  PotionRarity.Uncommon;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.Self;

    protected override async Task OnUse(PlayerChoiceContext context, Creature? target)
    {
        var cards = await CardSelectCmd.FromHand(context, Owner, new CardSelectorPrefs(new LocString("card_selection", "CRIMSONFANG-ADD_REPLAY"), 1, 1), c => !c.Keywords.Contains(CardKeyword.Unplayable), this);
        foreach (var card in cards)
        {
            card.BaseReplayCount += 1;
        }
    }
}