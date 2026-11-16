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
using MegaCrit.Sts2.Core.Factories;
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
public class BrassKnuckles() : CrimsonfangRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Rare;
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>()
    {
        HoverTipFactory.FromKeyword(CardKeyword.Ethereal),
        HoverTipFactory.Static(StaticHoverTip.ReplayStatic),
    };

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner || Owner.PlayerCombatState.TurnNumber != 1)
            return;
        Flash();
        for (int i = 0; i < 2; i++)
        {
            CardModel card = CardFactory.GetDistinctForCombat(Owner,
                (new CrimsonfangCard.CardFilters
                    { Pools = CrimsonfangCard.CardPoolPresets.AllPoolsMinusCrimsonfang, Owner = Owner }).FilterCards(), 1,
                Owner.RunState.Rng.CombatCardGeneration).First();
            card.AddKeyword(CardKeyword.Ethereal);
            CardPileAddResult combat = await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, player);
        }
    }
}