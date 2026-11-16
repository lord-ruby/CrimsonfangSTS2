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

public class SuckItUp() : CrimsonfangCard(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        if (CombatManager.Instance.History.CardPlaysFinished.Select(e => e.HappenedThisTurn(CombatState)).Any())
        {
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        }
    }
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>(){new BlockVar(8M, ValueProp.Move)};
    protected override void OnUpgrade() => DynamicVars.Block.UpgradeValueBy(2);

    protected override bool ShouldGlowGoldInternal => CombatManager.Instance.History.CardPlaysFinished
        .Select(e => e.HappenedThisTurn(CombatState)).Any();
}