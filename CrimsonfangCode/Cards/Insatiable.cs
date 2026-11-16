using Crimsonfang.CrimsonfangCode.Character;
using Crimsonfang.CrimsonfangCode.Powers;
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
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace Crimsonfang.CrimsonfangCode.Cards;

public class Insatiable() : CrimsonfangCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6, ValueProp.Move)];
    
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_slash").Execute(context);
        await Spellcasting.CastX(1, this, context, cardPlay, (c) => c.Type == CardType.Attack);
    }

    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(3M);

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(CrimsonfangHoverTips.CastStatic)];
}