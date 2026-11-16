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
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;

namespace Crimsonfang.CrimsonfangCode.Relics;

[Pool(typeof(CrimsonfangRelicPool))]
public class BloodOrangeDaquiri() : CrimsonfangRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Uncommon;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>()
    {
        HoverTipFactory.Static(StaticHoverTip.ReplayStatic)
    };

    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        cardPlay.Card.BaseReplayCount++;
        UsedThisCombat = true;
        Flash();
        return base.AfterCardPlayed(choiceContext, cardPlay);
    }

    private bool _usedThisCombat;
    public bool UsedThisCombat
    {
        get => this._usedThisCombat;
        set
        {
            this.AssertMutable();
            this._usedThisCombat = value;
        }
    }
    public override Task AfterCombatEnd(CombatRoom _)
    {
        this.UsedThisCombat = false;
        this.Status = RelicStatus.Normal;
        return Task.CompletedTask;
    }
    public override Task AfterRoomEntered(AbstractRoom room)
    {
        if (!(room is CombatRoom))
            return Task.CompletedTask;
        this.UsedThisCombat = false;
        this.Status = RelicStatus.Active;
        return Task.CompletedTask;
    }
}