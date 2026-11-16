using BaseLib.Utils;
using Crimsonfang.CrimsonfangCode.Utilities;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using MegaCrit.Sts2.Core.ValueProps;

namespace Crimsonfang.CrimsonfangCode.Cards;

[Pool(typeof(TokenCardPool))]
public class Ward() : CrimsonfangCard(1, CardType.Skill, CardRarity.Token, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword>(){CardKeyword.Retain, CardKeyword.Exhaust};

    protected override Task OnPlay(PlayerChoiceContext context, CardPlay play)
    {
        CreatureCmd.GainBlock(Owner.Creature, (decimal)Owner.Creature.Block, ValueProp.Move, play);
        return Task.CompletedTask;
    }

    protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
    
    public static async Task<Ward> CreateInHand(
        Player owner,
        ICombatState combatState)
    {
        Ward wards = Ward.Create(owner, combatState);
        CardPileAddResult combat = CardPileCmd.AddGeneratedCardsToCombat(new Ward[]{wards}, PileType.Hand, owner).Result.First();
        Ward inHand = wards;
        wards = null;
        return inHand;
    }
    
    public static Ward Create(Player owner, ICombatState combatState)
    {
        return combatState.CreateCard<Ward>(owner);
    }
}