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

public class SecondHandsmoke() : CrimsonfangCard(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(2, ValueProp.Move),
        new DamageVar("SecondDamage", 8, ValueProp.Move),
        new DynamicVar("VulnerablePower", 1)
    ];
    
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        var damage = DynamicVars.Damage.BaseValue;
        var second_damage = DynamicVars["SecondDamage"].BaseValue;
        var copy = CombatState.HittableEnemies.ToList();
        
        await DamageCmd.Attack(damage).FromCard(this, cardPlay).Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash").Execute(context);
        await PowerCmd.Apply<VulnerablePower>(context, cardPlay.Target, DynamicVars["VulnerablePower"].BaseValue, Owner.Creature, this, false);
        
        foreach (var c in copy)
        {
            if (c != cardPlay.Target)
            {
                await DamageCmd.Attack(second_damage).FromCard(this, cardPlay).Targeting(c)
                    .WithHitFx("vfx/vfx_attack_slash").Execute(context);
            }
        }
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(1M);
        DynamicVars["SecondDamage"].UpgradeValueBy(2M);
        DynamicVars["VulnerablePower"].UpgradeValueBy(1M);
    }
}