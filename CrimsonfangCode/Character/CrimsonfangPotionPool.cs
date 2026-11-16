using BaseLib.Abstracts;
using Crimsonfang.CrimsonfangCode.Extensions;
using Godot;

namespace Crimsonfang.CrimsonfangCode.Character;

public class CrimsonfangPotionPool : CustomPotionPoolModel
{
    public override Color LabOutlineColor => Crimsonfang.Color;


    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}