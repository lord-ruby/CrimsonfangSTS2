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

public class Congealed() : CrimsonfangCard(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        await PowerCmd.Apply<VulnerablePower>(context, cardPlay.Target, 1, Owner.Creature, this);
        CardSelectorPrefs prefs = new CardSelectorPrefs(ModelDb.Card<Graveblast>().SelectionScreenPrompt, 1);
        CardModel card2 = (await CardSelectCmd.FromCombatPile(context, PileType.Discard.GetPile(Owner), Owner, prefs)).FirstOrDefault<CardModel>();
        if (card2 == null)
            return;
        CardPileAddResult cardPileAddResult = await CardPileCmd.Add(card2, PileType.Hand);
    }

    public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword>() { CardKeyword.Exhaust };
    protected override void OnUpgrade() => RemoveKeyword(CardKeyword.Exhaust);
}