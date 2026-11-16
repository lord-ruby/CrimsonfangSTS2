using Crimsonfang.CrimsonfangCode.Utilities;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Crimsonfang.CrimsonfangCode.Cards;

public class Haemophobia() : CrimsonfangCard(0, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>(){new DynamicVar("VulnerableAmt", 1M)};
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>()
    {
        HoverTipFactory.FromPower<VulnerablePower>()
    };

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        bool extra = ShouldGlowGoldInternal;
        await PowerCmd.Apply<VulnerablePower>(context, cardPlay.Target, 1 + (extra ? DynamicVars["VulnerableAmt"].BaseValue : 0), Owner.Creature, this);
    }
    protected override void OnUpgrade() => this.DynamicVars["VulnerableAmt"].UpgradeValueBy(1M);
    protected override bool ShouldGlowGoldInternal => CombatManager.Instance.History.CardPlaysFinished.Any(e => e.HappenedThisTurn(CombatState) && e.CardPlay.Card.Type == CardType.Attack);
}