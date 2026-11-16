using BaseLib.Abstracts;
using Crimsonfang.CrimsonfangCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Crimsonfang.CrimsonfangCode.Character;

public class CrimsonfangRelicPool : CustomRelicPoolModel
{
    public override Color LabOutlineColor => Crimsonfang.Color;

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}