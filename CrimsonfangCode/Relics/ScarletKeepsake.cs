using BaseLib.Utils;
using Crimsonfang.CrimsonfangCode.Cards;
using Crimsonfang.CrimsonfangCode.Character;
using Crimsonfang.CrimsonfangCode.Relics;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;

namespace Crimsonfang.CrimsonfangCode.Relics;

[Pool(typeof(CrimsonfangRelicPool))]
public class ScarletKeepsake() : CrimsonfangRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Starter;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player.Creature.CombatState.RoundNumber > 1 || player != Owner)
            return;
        Flash();
        CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(player.Creature.CombatState.CreateCard(ModelDb.Card<Ward>(), Owner), PileType.Hand, Owner, CardPilePosition.Random));
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>()
    {
        HoverTipFactory.FromCard<Ward>()
    };

    public override RelicModel? GetUpgradeReplacement() => ModelDb.Relic<CrimsonHeirloom>();
}