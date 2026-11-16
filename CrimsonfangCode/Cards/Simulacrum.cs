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

public class Simulacrum() : CrimsonfangCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override int UPGRADE_REPLAY_INC => 1;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CastVar(1)];

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay cardPlay)
    {
        await Spellcasting.CastX(DynamicVars["CastAmount"].PreviewValue * PileType.Discard.GetPile(Owner).Cards.Select(p => p).Count(p => p.Tags.Contains(Conjure)), this, context, cardPlay);
    }
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(CrimsonfangHoverTips.CastDynamic), HoverTipFactory.Static(CrimsonfangHoverTips.Conjure)];
}