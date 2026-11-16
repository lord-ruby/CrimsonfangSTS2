using MegaCrit.Sts2.Core.Timeline;

namespace Crimsonfang.CrimsonfangCode.Epochs;

public class CrimsonfangEpochs
{
    public class Crimsonfang1Epoch : CrimsonfangEpoch
    {
        public override string Id => "CRIMSONFANG-CRIMSONFANG1_EPOCH";
        public override void QueueUnlocks() {}
    }
    
    public class Crimsonfang2Epoch : CrimsonfangEpoch
    {
        public override string Id => "CRIMSONFANG-CRIMSONFANG2_EPOCH";
        public override void QueueUnlocks() {}
    }
    
    public class Crimsonfang3Epoch : CrimsonfangEpoch
    {
        public override string Id => "CRIMSONFANG-CRIMSONFANG3_EPOCH";
        public override void QueueUnlocks() {}
    }
    
    public class Crimsonfang4Epoch : CrimsonfangEpoch
    {
        public override string Id => "CRIMSONFANG-CRIMSONFANG4_EPOCH";
        public override void QueueUnlocks() {}
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
        public override void QueueUnlocks() {}
    }
    
    public class Crimsonfang7Epoch : CrimsonfangEpoch
    {
        public override string Id => "CRIMSONFANG-CRIMSONFANG7_EPOCH";
        public override void QueueUnlocks() {}
    }
}
