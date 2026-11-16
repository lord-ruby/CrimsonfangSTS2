using Crimsonfang.CrimsonfangCode.Character;
using Crimsonfang.CrimsonfangCode.Utilities;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.ValueProps;

namespace Crimsonfang.CrimsonfangCode.Cards;

public class BrokenPulse() : CrimsonfangCard(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>(){new DamageVar(10M, ValueProp.Move)};
    

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play)
    {
        await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard(this, play).Targeting(play.Target)
            .WithHitFx("vfx/vfx_attack_slash").Execute(context);
        CardPile pile = PileType.Hand.GetPile(Owner);
        CardModel card = Owner.RunState.Rng.CombatCardSelection.NextItem(pile.Cards);
        if (card == null)
            return;
        var cards = ModelDb.AllCharacterCardPools.Select(p => p).Where(p => p.GetType() != typeof(CrimsonfangCardPool)).SelectMany(e => e.AllCards).Where(e => e.Type == CardType.Attack && e.Rarity != CardRarity.Basic);
        await CardCmd.Transform(card, CombatState.CreateCard(Owner.RunState.Rng.CombatCardSelection.NextItem(cards), Owner));
    }
    

    protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(3);
}