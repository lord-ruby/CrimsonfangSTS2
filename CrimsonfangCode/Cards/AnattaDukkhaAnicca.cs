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
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;

namespace Crimsonfang.CrimsonfangCode.Cards;

public class AnattaDukkhaAnnica() : CrimsonfangCard(2, CardType.Skill, CardRarity.Rare, TargetType.None)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>(){new EnergyVar(1), new DynamicVar("Cards", 1)};

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play)
    {
        while (PileType.Hand.GetPile(Owner).Cards.Count > 0)
        {
            CardModel c = PileType.Hand.GetPile(Owner).Cards.First();
            if (c != null)
            {
                c.AddKeyword(CardKeyword.Exhaust);
                await Spellcasting.Conjure(context, c);
            }
        }
    }

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override void OnUpgrade() => RemoveKeyword(CardKeyword.Exhaust);
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>()
    {
        HoverTipFactory.Static(CrimsonfangHoverTips.Conjure),
    };
}