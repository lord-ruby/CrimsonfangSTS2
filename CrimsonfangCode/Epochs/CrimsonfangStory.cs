using MegaCrit.Sts2.Core.Timeline;

namespace Crimsonfang.CrimsonfangCode.Epochs;

public sealed class CrimsonfangStory : StoryModel
{
    public const string StoryKey = "CRIMSONFANG";
    protected override string Id => StoryKey;

    public override EpochModel[] Epochs => new[]
    {
        EpochModel.Get<CrimsonfangEpochs.Crimsonfang1Epoch>(),
        EpochModel.Get<CrimsonfangEpochs.Crimsonfang2Epoch>(),
        EpochModel.Get<CrimsonfangEpochs.Crimsonfang3Epoch>(),
        EpochModel.Get<CrimsonfangEpochs.Crimsonfang4Epoch>(),
        EpochModel.Get<CrimsonfangEpochs.Crimsonfang5Epoch>(),
        EpochModel.Get<CrimsonfangEpochs.Crimsonfang6Epoch>(),
        EpochModel.Get<CrimsonfangEpochs.Crimsonfang7Epoch>(),
    };
}