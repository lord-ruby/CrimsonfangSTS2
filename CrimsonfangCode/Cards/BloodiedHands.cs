using Crimsonfang.CrimsonfangCode.Character;
using Crimsonfang.CrimsonfangCode.Powers;
using Crimsonfang.CrimsonfangCode.Utilities;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
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

public class BloodiedHands() : CrimsonfangCard(2, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<WeakPower>(2), new CardsVar(2)];

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        await PowerCmd.Apply<WeakPower>(context, cardPlay.Target, DynamicVars.Weak.PreviewValue, Owner.Creature, this);
        for (int i = 0; i < this.DynamicVars.Cards.IntValue; ++i)
        {
            CardModel card = PileType.Draw.GetPile(Owner).Cards.Where((c => !c.Keywords.Contains(CardKeyword.Unplayable) && c.Type == CardType.Attack)).ToList().StableShuffle(Owner.RunState.Rng.CombatCardSelection).FirstOrDefault() ?? PileType.Draw.GetPile(Owner).Cards.ToList().StableShuffle(Owner.RunState.Rng.CombatCardSelection).FirstOrDefault();
            if (card != null)
                await CardCmd.AutoPlay(context, card, null);
        }
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<WeakPower>()];
}