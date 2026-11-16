using BaseLib.Utils;
using Crimsonfang.CrimsonfangCode.Cards;
using Crimsonfang.CrimsonfangCode.Character;
using Crimsonfang.CrimsonfangCode.Relics;
using Crimsonfang.CrimsonfangCode.Utilities;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace Crimsonfang.CrimsonfangCode.Relics;

[Pool(typeof(CrimsonfangRelicPool))]
public class Amaryllis() : CrimsonfangRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Shop;

    public override bool IsAllowedInShops => !RelicModel.IsLateInAct3(Owner.RunState);
    
    public override bool ShowCounter => true;

    public override int DisplayAmount => !IsActivating ? CombatsSeen % 2 : 2;
    
    public bool _isActivating;
    
    public int _combatsSeen;
    
    public bool IsActivating
    {
        get => _isActivating;
        set
        {
            AssertMutable();
            _isActivating = value;
            InvokeDisplayAmountChanged();
        }
    }
    [SavedProperty]
    public int CombatsSeen
    {
        get => _combatsSeen;
        set
        { 
            AssertMutable();
            _combatsSeen = value;
        }
    }
    
    public override bool TryModifyCardRewardOptions(
        Player player,
        List<CardCreationResult> options,
        CardCreationOptions creationOptions)
    {
        if (this.Owner != player || creationOptions.Source != CardCreationSource.Encounter || !this.IsInTriggeringCombat)
            return false;
        IEnumerable<CardModel> cardModels = (new CrimsonfangCard.CardFilters {Owner=Owner}).FilterCards();
        if (!cardModels.Any())
            return false;
        
        CardModel card = CardFactory.CreateForReward(this.Owner, 1, new CardCreationOptions([DummyCardPool.CreateDummyPool(cardModels)], CardCreationSource.Other, creationOptions.RarityOdds).WithFlags(CardCreationFlags.NoModifyHooks | CardCreationFlags.NoCardPoolModifications)).FirstOrDefault()?.Card;
        if (card != null)
        {
            CardCreationResult cardCreationResult = new CardCreationResult(card);
            cardCreationResult.ModifyCard(card, this);
            options.Add(cardCreationResult);
        }
        return card != null;
    }

    public override Task AfterCombatEnd(CombatRoom room)
    {
        ++CombatsSeen;
        if (IsInTriggeringCombat)
            TaskHelper.RunSafely(this.DoActivateVisuals());
        InvokeDisplayAmountChanged();
        return Task.CompletedTask;
    }

    public async Task DoActivateVisuals()
    {
        IsActivating = true;
        Flash();
        await Cmd.Wait(1f);
        IsActivating = false;
    }

    public bool IsInTriggeringCombat => CombatsSeen > 0 && CombatsSeen % 2 == 0;
}