using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Enchantments;

namespace Crimsonfang.CrimsonfangCode.Enchantments;

public class FragileEnchantment : CustomEnchantmentModel
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip>()
    {
        HoverTipFactory.FromKeyword(CardKeyword.Exhaust), HoverTipFactory.FromKeyword(CardKeyword.Ethereal)
    };
    protected override void OnEnchant() => CardCmd.ApplyKeyword(this.Card, CardKeyword.Exhaust, CardKeyword.Ethereal);
}