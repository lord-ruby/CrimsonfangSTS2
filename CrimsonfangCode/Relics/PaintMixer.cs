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
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace Crimsonfang.CrimsonfangCode.Relics;

[Pool(typeof(CrimsonfangRelicPool))]
public class PaintMixer() : CrimsonfangRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Uncommon;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(3M, ValueProp.Unpowered)];
    public override Task BeforeSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side == CombatSide.Player)
        {
            int colors = CountColors(PileType.Hand.GetPile(Owner).Cards);
            if (colors > 0)
            {
                CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block.BaseValue * (decimal)colors, ValueProp.Move,
                    null);
                Flash();
            }
        }
        return base.BeforeSideTurnEnd(choiceContext, side, participants);
    }

    public static int CountColors(IEnumerable<CardModel> cards)
    {
        int total = 0;
        Dictionary<string, bool> seen  = new Dictionary<string, bool>();
        foreach (CardModel c in cards)
        {
            if (!seen.ContainsKey(c.EnergyIconPath))
            {
                total++;
                seen.Add(c.EnergyIconPath, true);
            }
        }

        return total;
    }
}