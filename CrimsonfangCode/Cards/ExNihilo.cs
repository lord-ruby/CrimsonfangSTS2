using Crimsonfang.CrimsonfangCode.Character;
using Crimsonfang.CrimsonfangCode.Powers;
using Crimsonfang.CrimsonfangCode.Utilities;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace Crimsonfang.CrimsonfangCode.Cards;

public class ExNihilo() : CrimsonfangCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.None)
{

    protected override bool HasEnergyCostX => true;
    
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        int powerAmount = ResolveEnergyXValue();
        if (IsUpgraded)
            ++powerAmount;
        
        var cards = new CardFilters
        {
            Owner = Owner, Pools = CardPoolPresets.AllCharacterPools
        }.FilterCards();
        for (int i = 0; i < powerAmount; i++)
        {
            var card = CombatState.CreateCard(Owner.RunState.Rng.CombatCardSelection.NextItem(cards), Owner);
            card.SetToFreeThisTurn();
            await CardPileCmd.Add(card, PileType.Hand, CardPilePosition.Top);
        }

    }

    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
}