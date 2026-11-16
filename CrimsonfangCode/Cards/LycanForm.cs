using Crimsonfang.CrimsonfangCode.Powers;
using Crimsonfang.CrimsonfangCode.Utilities;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace Crimsonfang.CrimsonfangCode.Cards;

public class LycanForm() : CrimsonfangCard(3, CardType.Power, CardRarity.Rare, TargetType.None)
{

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play)
    {
        await PowerCmd.Apply<LycanFormPower>(context, Owner.Creature, 1M, Owner.Creature, this, false);
    }

    protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);

    protected override IEnumerable<DynamicVar> CanonicalVars => [new EnergyVar(1)];
}