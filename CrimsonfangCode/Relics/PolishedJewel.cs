using BaseLib.Utils;
using Crimsonfang.CrimsonfangCode.Cards;
using Crimsonfang.CrimsonfangCode.Character;
using Crimsonfang.CrimsonfangCode.Relics;
using Crimsonfang.CrimsonfangCode.Utilities;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace Crimsonfang.CrimsonfangCode.Relics;

[Pool(typeof(CrimsonfangRelicPool))]
public class PolishedJewel() : CrimsonfangRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Rare;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>()
    {
        HoverTipFactory.Static(CrimsonfangHoverTips.Conjure)
    };

    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(7M, ValueProp.Unpowered)];

    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Pile.Type == SpellPile.SpellPileType)
        {
            UsedThisTurn = true;
            CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, null);
            Flash();
        }

        return base.AfterCardPlayed(choiceContext, cardPlay);
    }
    public override Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side == CombatSide.Player)
        {
            UsedThisTurn = false;
            
        }
        return base.AfterSideTurnEnd(choiceContext, side, participants);
    }

    private bool _usedThisTurn;
    public bool UsedThisTurn
    {
        get => this._usedThisTurn;
        set
        {
            this.AssertMutable();
            this._usedThisTurn = value;
        }
    }
    public override Task AfterCombatEnd(CombatRoom _)
    {
        this.UsedThisTurn = false;
        this.Status = RelicStatus.Normal;
        return Task.CompletedTask;
    }
    public override Task AfterRoomEntered(AbstractRoom room)
    {
        if (!(room is CombatRoom))
            return Task.CompletedTask;
        this.UsedThisTurn = false;
        this.Status = RelicStatus.Active;
        return Task.CompletedTask;
    }
}