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
public class EchoingHorn() : CrimsonfangRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Rare;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>()
    {
        HoverTipFactory.Static(StaticHoverTip.ReplayStatic)
    };

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(3M, ValueProp.Unpowered)];

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        
        if (cardPlay.Card.BaseReplayCount > 0)
        {
            await CreatureCmd.Damage(choiceContext, Owner.Creature.CombatState.HittableEnemies, DynamicVars.Damage, Owner.Creature);
        }
    }
}