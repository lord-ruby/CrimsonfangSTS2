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

public class BiteForce() : CrimsonfangCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(4, ValueProp.Move),
        new DynamicVar("DamageIncrease", 1),
        new DynamicVar("DamageIncreased", 0)
    ];
    
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_slash").Execute(context);
    }


    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card != this && Pile != null && Pile.Type == PileType.Hand)
        {
            DynamicVars["DamageIncreased"].UpgradeValueBy(DynamicVars["DamageIncrease"].BaseValue);
            DynamicVars.Damage.UpgradeValueBy(DynamicVars["DamageIncrease"].BaseValue);
        }
        return base.AfterCardPlayed(choiceContext, cardPlay);
    }

    public override Task AfterCombatEnd(CombatRoom room)
    {
        DynamicVars.Damage.UpgradeValueBy(-DynamicVars["DamageIncreased"].BaseValue);
        DynamicVars["DamageIncreased"].UpgradeValueBy(-DynamicVars["DamageIncreased"].BaseValue);
        return base.AfterCombatEnd(room);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["DamageIncrease"].UpgradeValueBy(1M);
    }
}