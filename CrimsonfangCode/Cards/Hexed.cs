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

public class Hexed() : CrimsonfangCard(2, CardType.Skill, CardRarity.Rare, TargetType.None)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>(){new DynamicVar("Cards", 1)};

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play)
    {
        var cards = await CardSelectCmd.FromHand(context, Owner, new CardSelectorPrefs(new LocString("card_selection", "CRIMSONFANG-TO_SPELL_PILE"), (int)DynamicVars["Cards"].BaseValue, (int)DynamicVars["Cards"].BaseValue), _ => true, this);
        foreach (var card in cards)
        {
            await CardPileCmd.AddGeneratedCardToCombat(card.CreateClone(), SpellPile.SpellPileType, Owner);
        }
    }

    protected override void OnUpgrade() => this.DynamicVars["Cards"].UpgradeValueBy(1M);
}