using BaseLib.Abstracts;
using Crimsonfang.CrimsonfangCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Models;

namespace Crimsonfang.CrimsonfangCode.Utilities;

public class DummyCardPool : CustomCardPoolModel
{
    public static CardModel[] Cards;
    
    protected override CardModel[] GenerateAllCards()
    {
        return Cards;
    }

    public static CardPoolModel CreateDummyPool(IEnumerable<CardModel> cards)
    {
        Cards = cards.ToArray();
        return ModelDb.CardPool<DummyCardPool>();
    }
    public override string Title => Character.Crimsonfang.CharacterId; //This is not a display name.

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
    public override float H => 0.93f; //Hue; changes the color.
    public override float S => 1f; //Saturation
    public override float V => 1f;
    public override bool IsColorless => false;
    public override Color DeckEntryCardColor => new("ffffff");
}