using System.Reflection;
using MegaCrit.Sts2.Core.Timeline;
using MegaCrit.Sts2.Core.Timeline.Epochs;

namespace Crimsonfang.CrimsonfangCode.Epochs;

public class EpochRegistration
{
    public static readonly Type[] CrimsonfangEpochTypes =
    {
        typeof(CrimsonfangEpochs.Crimsonfang1Epoch), typeof(CrimsonfangEpochs.Crimsonfang2Epoch), typeof(CrimsonfangEpochs.Crimsonfang3Epoch),
        typeof(CrimsonfangEpochs.Crimsonfang4Epoch), typeof(CrimsonfangEpochs.Crimsonfang5Epoch), typeof(CrimsonfangEpochs.Crimsonfang6Epoch),
        typeof(CrimsonfangEpochs.Crimsonfang7Epoch),
    };

    private const BindingFlags StaticNonPublic = BindingFlags.Static | BindingFlags.NonPublic;

    private static readonly FieldInfo? EpochById = Find(typeof(EpochModel), "_epochTypeDictionary");
    private static readonly FieldInfo? IdByType = Find(typeof(EpochModel), "_typeToIdDictionary");
    private static readonly FieldInfo? AllEpochs = Find(typeof(EpochModel), "_allEpochs");
    private static readonly FieldInfo? AllEpochIdsCache = Find(typeof(EpochModel), "_allEpochIds");
    private static readonly FieldInfo? StoryById = Find(typeof(StoryModel), "_storyTypeDictionary");

    // Nullable on purpose: a throw inside a static field initializer poisons the whole class
    private static FieldInfo? Find(Type type, string name) => type.GetField(name, StaticNonPublic);

    // The default game branch has no mutable _allEpochs, so mod epochs only work on public-beta
    public static bool Supported =>
        EpochById != null && IdByType != null && AllEpochs != null && AllEpochIdsCache != null
        && StoryById != null;

    private static bool _registered;

    public static void RegisterEpochs()
    {
        if (_registered) return;
        _registered = true;

        if (!Supported)
        {
            MainFile.Logger.Warn(
                "[Epochs] This build of the game has no mod-writable epoch registry, so the Timeline "
                + "feature is off. Every card, relic and potion is available instead of Timeline-gated. "
                + "The rest of the mod is unaffected.");
            return;
        }

        var epochById = (Dictionary<string, Type>)EpochById!.GetValue(null)!;
        var idByType = (Dictionary<Type, string>)IdByType!.GetValue(null)!;
        var allEpochs = (List<Type>)AllEpochs!.GetValue(null)!;

        foreach (var type in CrimsonfangEpochTypes)
        {
            var epoch = (EpochModel)Activator.CreateInstance(type)!;
            if (epochById.ContainsKey(epoch.Id)) continue;
            epochById[epoch.Id] = type;
            idByType[type] = epoch.Id;
            allEpochs.Add(type);
        }
        AllEpochIdsCache!.SetValue(null, null); // Clears the lazy cache so AllEpochIds rebuilds

        var storyById = (Dictionary<string, Type>)StoryById!.GetValue(null)!;
        storyById[CrimsonfangStory.StoryKey] = typeof(CrimsonfangStory);

        MainFile.Logger.Info($"[Epochs] Registered {CrimsonfangEpochTypes.Length} epochs + story '{CrimsonfangStory.StoryKey}'.");
    }

    public static IEnumerable<string> GatingEpochIds(EpochUnlockKind kind) =>
        CrimsonfangEpochTypes
            .Select(t => (CrimsonfangEpoch)Activator.CreateInstance(t)!)
            .Where(e => e.UnlockKind == kind)
            .Select(e => e.Id);

    // Assigned lazily, so every mod has registered into _allEpochs first
    private static readonly EpochEra[] PreferredEras =
    {
        EpochEra.Seeds0, EpochEra.Seeds3, EpochEra.Blight1, 
        EpochEra.Blight2, EpochEra.Flourish2, EpochEra.Invitation2,
        EpochEra.Invitation7,
    };
    private const int TopRow = 4;
    private static Dictionary<Type, (EpochEra era, int pos)>? _slots;

    public static (EpochEra era, int pos) SlotFor(Type epochType)
    {
        // Reached from every epoch's Era getter, so it must never throw
        if (!Supported) return (EpochEra.Seeds0, 0);
        var slots = _slots ??= AssignSlots();
        return slots.TryGetValue(epochType, out var s) ? s : (EpochEra.Seeds0, 0);
    }

    private static Dictionary<Type, (EpochEra, int)> AssignSlots()
    {
        var occupied = new HashSet<(EpochEra, int)>();
        foreach (var type in (List<Type>)AllEpochs!.GetValue(null)!)
        {
            if (typeof(CrimsonfangEpoch).IsAssignableFrom(type)) continue; // Ours would recurse into SlotFor
            try
            {
                var e = (EpochModel)Activator.CreateInstance(type)!;
                occupied.Add((e.Era, e.EraPosition));
            }
            catch { }
        }

        var slots = new Dictionary<Type, (EpochEra, int)>();
        int i = 0;
        foreach (var type in CrimsonfangEpochTypes)
        {
            var cell = FindFreeCell(occupied, i++);
            slots[type] = cell;
            occupied.Add(cell);
        }
        return slots;
    }

    private static (EpochEra, int) FindFreeCell(HashSet<(EpochEra, int)> occupied, int e)
    {
        for (var pos = TopRow; pos >= 0; pos--)
        {
            var era = PreferredEras[e];
            if (!occupied.Contains((era, pos)))
                return (era, pos);
        }

        return (EpochEra.Invitation7, 0);
    }
}