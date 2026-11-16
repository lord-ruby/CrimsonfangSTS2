using Crimsonfang.CrimsonfangCode.Character;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Crimsonfang.CrimsonfangCode.Powers;

public class LycanFormPower() : CrimsonfangPower()
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Type == CardType.Skill)
        {
            var cards = ModelDb.AllCharacterCardPools.Select(p => p).Where(p => p.GetType() == typeof(CrimsonfangCardPool)).SelectMany(e => e.AllCards).Where(e => e.Type == CardType.Attack && e.Rarity != CardRarity.Basic);
            for (int i = 0; i < Amount; i++)
            {
                await CardCmd.AutoPlay(choiceContext,
                    CombatState.CreateCard(Owner.Player.RunState.Rng.CombatCardSelection.NextItem(cards), Owner.Player),
                    null);
            }
        }
    }
    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];
}