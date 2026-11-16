using BaseLib.Cards;
using BaseLib.Extensions;
using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Crimsonfang.CrimsonfangCode.Utilities;

public static class CrimsonfangHoverTips
{
    [CustomEnum]
    public static StaticHoverTip Conjure;
    [CustomEnum]
    public static StaticHoverTip OnCast;
    [CustomEnum]
    public static StaticHoverTip CastDynamic;
    [CustomEnum]
    public static StaticHoverTip CastStatic;

    public static DynamicVar ConvertVar(this DynamicVar value, string name)
    {
        return new DynamicVar(name, value.BaseValue);
    }
}