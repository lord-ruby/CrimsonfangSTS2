using System.ComponentModel;
using BaseLib.Abstracts;
using BaseLib.Cards.Variables;
using BaseLib.Extensions;
using BaseLib.Patches.Content;
using Crimsonfang.CrimsonfangCode.Cards;
using Crimsonfang.CrimsonfangCode.Relics;
using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace Crimsonfang.CrimsonfangCode.Utilities;

public class Spellcasting
{

    public static bool IsBeingCast;
    public static bool IsArcaneBlast;
    public static ICombatState? CombatState;

    public static async Task CastX(DynamicVar x, CardModel c, PlayerChoiceContext context, CardPlay play) =>
        await CastX(x.PreviewValue, c, context, play, (_) => true);

    public static async Task CastX(decimal x, CardModel c, PlayerChoiceContext context, CardPlay play) =>
        await CastX(x, c, context, play, (_) => true);
    public static async Task CastX(decimal x, CardModel c, PlayerChoiceContext context, CardPlay play, Func<CardModel, bool> filter)
    {
        var cards = SpellPile.SpellPileType.GetPile(c.Owner).Cards.Select(x => x).Where(filter).ToList();
        if (x <= 0 || cards.Count <= 0) return;
        int i = 0;
        if (c.Owner.TryGetRelic<Starfruit>(out Starfruit relic))
        {
            relic.Flash();
        }

        CombatState = c.CombatState;
        foreach(CardModel card in cards)
        {
            if (i++ < x)
            {
                IsBeingCast = true;
                if (c is ArcaneBlast)
                {
                    IsArcaneBlast = true;
                }
                await CardCmd.AutoPlay(context, card, null);
                IsBeingCast = false;
                IsArcaneBlast = false;
            }
        }   
    }
    
    public static async Task Conjure(
        PlayerChoiceContext? choiceContext,
        CardModel card,
        bool skipVisuals = false)
    {
        ICombatState combatState;
        if (CombatManager.Instance.IsOverOrEnding)
        {

        }
        else
        {
            await CardPileCmd.Add(card, SpellPile.SpellPileType, CardPilePosition.Bottom);
        }
    }
}

public class SpellPile() : CustomPile(SpellPileType)
{
    [CustomEnum] public static PileType SpellPileType;
    
    public override bool CardShouldBeVisible(CardModel card)
    {
        return true;
    }

    public override Vector2 GetTargetPosition(CardModel model, Vector2 size)
    {
        return new Vector2(75, 765);
    }
}

public class CastVar(int amount) : DynamicVar("CastAmount", amount)
{
    public override void UpdateCardPreview(CardModel card, CardPreviewMode previewMode, Creature? target, bool runGlobalHooks)
    {
        PreviewValue = BaseValue ;
        if (card.Owner.TryGetRelic<Starfruit>(out _))
        {
            PreviewValue += 1;
        }
    }
}