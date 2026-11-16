using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Unlocks;

namespace Crimsonfang.CrimsonfangCode.Epochs;

public class EpochGating
{
    private sealed record Gates(
        Dictionary<ModelId, Func<UnlockState, bool>> Cards,
        Dictionary<ModelId, Func<UnlockState, bool>> Relics,
        Dictionary<ModelId, Func<UnlockState, bool>> Potions);

    private static Gates? _gates;

    // IsEpochRevealed is generic, so each epoch needs its own compile-time predicate
    private static readonly (Type Epoch, Func<UnlockState, bool> Revealed)[] Revealers =
    {
        (typeof(CrimsonfangEpochs.Crimsonfang2Epoch), us => us.IsEpochRevealed<CrimsonfangEpochs.Crimsonfang2Epoch>()),
        (typeof(CrimsonfangEpochs.Crimsonfang3Epoch), us => us.IsEpochRevealed<CrimsonfangEpochs.Crimsonfang3Epoch>()),
        (typeof(CrimsonfangEpochs.Crimsonfang4Epoch), us => us.IsEpochRevealed<CrimsonfangEpochs.Crimsonfang4Epoch>()),
        (typeof(CrimsonfangEpochs.Crimsonfang5Epoch), us => us.IsEpochRevealed<CrimsonfangEpochs.Crimsonfang5Epoch>()),
        (typeof(CrimsonfangEpochs.Crimsonfang6Epoch), us => us.IsEpochRevealed<CrimsonfangEpochs.Crimsonfang6Epoch>()),
        (typeof(CrimsonfangEpochs.Crimsonfang7Epoch), us => us.IsEpochRevealed<CrimsonfangEpochs.Crimsonfang7Epoch>()),
    };

    public static bool CardUnlocked(ModelId id, UnlockState unlockState) => Unlocked(Built.Cards, id, unlockState);
    public static bool RelicUnlocked(ModelId id, UnlockState unlockState) => Unlocked(Built.Relics, id, unlockState);
    public static bool PotionUnlocked(ModelId id, UnlockState unlockState) => Unlocked(Built.Potions, id, unlockState);

    private static bool Unlocked(Dictionary<ModelId, Func<UnlockState, bool>> gates, ModelId id, UnlockState unlockState)
    {
        // Without a registry the gates were never built, so honouring them would hide most of the mod
        if (!EpochRegistration.Supported) return true;
        return !gates.TryGetValue(id, out var revealed) || revealed(unlockState);
    }

    private static Gates Built => _gates ??= Build();

    private static Gates Build()
    {
        var cards = new Dictionary<ModelId, Func<UnlockState, bool>>();
        var relics = new Dictionary<ModelId, Func<UnlockState, bool>>();
        var potions = new Dictionary<ModelId, Func<UnlockState, bool>>();
        foreach (var (type, revealed) in Revealers)
        {
            var epoch = (CrimsonfangEpoch)Activator.CreateInstance(type)!;
            foreach (var c in epoch.GatedCards) cards[c.Id] = revealed;
            foreach (var r in epoch.GatedRelics) relics[r.Id] = revealed;
            foreach (var p in epoch.GatedPotions) potions[p.Id] = revealed;
        }
        return new Gates(cards, relics, potions);
    }
}