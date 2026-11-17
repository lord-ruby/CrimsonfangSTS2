using Crimsonfang.CrimsonfangCode.Cards;
using Crimsonfang.CrimsonfangCode.Potions;
using Crimsonfang.CrimsonfangCode.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Timeline;

namespace Crimsonfang.CrimsonfangCode.Epochs;

public class CrimsonfangEpochs
{
    public class Crimsonfang1Epoch : CrimsonfangEpoch
    {
        public override string Id => "CRIMSONFANG-CRIMSONFANG1_EPOCH";
        public override EpochUnlockKind UnlockKind => EpochUnlockKind.Relics;

        protected override List<RelicModel> Relics => [ModelDb.Relic<Starfruit>(), ModelDb.Relic<BloodOrangeDaquiri>(), ModelDb.Relic<BrassKnuckles>()];
    }
    
    public class Crimsonfang2Epoch : CrimsonfangEpoch
    {
        public override string Id => "CRIMSONFANG-CRIMSONFANG2_EPOCH";
        public override EpochUnlockKind UnlockKind => EpochUnlockKind.Potions;

        protected override List<PotionModel> Potions => [ModelDb.Potion<SpectralPotion>(), ModelDb.Potion<CrystallineSolution>(), ModelDb.Potion<BitterFlask>()];
    }
    
    public class Crimsonfang3Epoch : CrimsonfangEpoch
    {
        public override string Id => "CRIMSONFANG-CRIMSONFANG3_EPOCH";
        public override EpochUnlockKind UnlockKind => EpochUnlockKind.Cards;

        //protected override List<CardModel> Cards => [ModelDb.Card<Decantation>(), ModelDb.Card<SuccumbToRot>(), ModelDb.Card<Overflow>()];
    }
    
    public class Crimsonfang4Epoch : CrimsonfangEpoch
    {
        public override string Id => "CRIMSONFANG-CRIMSONFANG4_EPOCH";
        public override EpochUnlockKind UnlockKind => EpochUnlockKind.Cards;

        //protected override List<CardModel> Cards => [ModelDb.Card<Crystalization>(), ModelDb.Card<WildStrike>(), ModelDb.Card<Chrysanthemum>()];
    }
    
    public class Crimsonfang5Epoch : CrimsonfangEpoch
    {
        public override string Id => "CRIMSONFANG-CRIMSONFANG5_EPOCH";
        public override EpochModel[] GetTimelineExpansion() => new[]
        {
            Get<Crimsonfang2Epoch>(), Get<Crimsonfang3Epoch>(), Get<Crimsonfang4Epoch>(),
            Get<Crimsonfang1Epoch>(), Get<Crimsonfang6Epoch>(), Get<Crimsonfang7Epoch>(),
        };
        public override void QueueUnlocks() => QueueTimelineExpansion(GetTimelineExpansion());
    }
    
    public class Crimsonfang6Epoch : CrimsonfangEpoch
    {
        public override string Id => "CRIMSONFANG-CRIMSONFANG6_EPOCH";
        public override EpochUnlockKind UnlockKind => EpochUnlockKind.Relics;

        protected override List<RelicModel> Relics => [ModelDb.Relic<PaintMixer>(), ModelDb.Relic<PolishedJewel>(), ModelDb.Relic<EchoingHorn>()];
    }
    
    public class Crimsonfang7Epoch : CrimsonfangEpoch
    {
        public override string Id => "CRIMSONFANG-CRIMSONFANG7_EPOCH";
        public override EpochUnlockKind UnlockKind => EpochUnlockKind.Cards;

        //protected override List<CardModel> Cards => [ModelDb.Card<ReachBeyond>(), ModelDb.Card<ManaSurge>(), ModelDb.Card<Hexed>()];
    }
}
