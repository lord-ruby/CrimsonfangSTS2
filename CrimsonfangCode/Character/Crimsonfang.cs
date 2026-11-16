using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using Crimsonfang.CrimsonfangCode.Cards;
using Crimsonfang.CrimsonfangCode.Epochs;
using Crimsonfang.CrimsonfangCode.Extensions;
using Crimsonfang.CrimsonfangCode.Relics;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Unlocks;

namespace Crimsonfang.CrimsonfangCode.Character;
public class Crimsonfang : PlaceholderCharacterModel
{
    public const string CharacterId = "Crimsonfang";

    public static readonly Color Color = new("ff3bc3");

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Feminine;
    public override int StartingHp => 70;
    
    public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<StrikeCrimsonfang>(),
        ModelDb.Card<StrikeCrimsonfang>(),
        ModelDb.Card<StrikeCrimsonfang>(),
        ModelDb.Card<StrikeCrimsonfang>(),
        ModelDb.Card<DefendCrimsonfang>(),
        ModelDb.Card<DefendCrimsonfang>(),
        ModelDb.Card<DefendCrimsonfang>(),
        ModelDb.Card<DefendCrimsonfang>(),
        ModelDb.Card<Providence>(),
        ModelDb.Card<Spellweaver>()
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<ScarletKeepsake>()
    ];

    public override CardPoolModel CardPool => ModelDb.CardPool<CrimsonfangCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<CrimsonfangRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<CrimsonfangPotionPool>();

    /*  PlaceholderCharacterModel will utilize placeholder basegame assets for most of your character assets until you
        override all the other methods that define those assets.
        These are just some of the simplest assets, given some placeholders to differentiate your character with.
        You don't have to, but you're suggested to rename these images. */
    public override Control CustomIcon
    {
        get
        {
            var icon = NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
            icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            return icon;
        }
    }
    
    protected override CharacterModel? UnlocksAfterRunAs => ModelDb.Character<Fool>();

    public override string CustomIconTexturePath => "character_icon_crimsonfang.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_crimsonfang.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_crimsonfang_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_crimsonfang.png".CharacterUiPath();
    public override string? CustomEnergyCounterPath =>
        "res://Crimsonfang/scenes/morningstar_energy_counter.tscn";

    public override string CustomArmPointingTexturePath => "hand_1.png".CharacterUiPath();
    public override string CustomArmRockTexturePath => "hand_0.png".CharacterUiPath();
    public override string CustomArmPaperTexturePath => "hand_3.png".CharacterUiPath();
    public override string CustomArmScissorsTexturePath => "hand_2.png".CharacterUiPath();
    public override string CustomVisualPath => "res://Crimsonfang/scenes/creature_visuals/crimsonfang.tscn";
    public override string CustomCharacterSelectBg => "res://Crimsonfang/scenes/char_select_bg_crimsonfang.tscn";
    public override string CharacterSelectSfx => "res://Crimsonfang/audio/bark.wav";
}

[HarmonyPatch(typeof(UnlockState), nameof(UnlockState.Characters), MethodType.Getter)]
public static class CrimsonfangLockPatch
{
    public static void Postfix(UnlockState __instance, ref IEnumerable<CharacterModel> __result)
    {
        var Charater = __result.ToList();
        if (!__instance.IsEpochRevealed<CrimsonfangEpochs.Crimsonfang5Epoch>())
            Charater.Remove(ModelDb.Character<Crimsonfang>());
        __result = Charater;
    }
		
}