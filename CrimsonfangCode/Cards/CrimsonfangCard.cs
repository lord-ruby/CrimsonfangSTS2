using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Patches.Content;
using BaseLib.Utils;
using Crimsonfang.CrimsonfangCode.Character;
using Crimsonfang.CrimsonfangCode.Extensions;
using Crimsonfang.CrimsonfangCode.Utilities;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Enchantments;

namespace Crimsonfang.CrimsonfangCode.Cards;

[Pool(typeof(CrimsonfangCardPool))]
public abstract class CrimsonfangCard :
    CustomCardModel
{
    //Image size:
    //Normal art: 1000x760 (Using 500x380 should also work, it will simply be scaled.)
    //Full art: 606x852
    public override string CustomPortraitPath {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigCardImagePath();
            return ResourceLoader.Exists(path) ? path : "card.png".BigCardImagePath();
        }
    }

    //Smaller variants of card images for efficiency:
    //Smaller variant of fullart: 250x350
    //Smaller variant of normalart: 250x190

    //Uses card_portraits/card_name.png as image path. These should be smaller images.
    public override string PortraitPath
    {
        get
        {
            var path = $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
            return ResourceLoader.Exists(path) ? path : "card.png".CardImagePath();
        }
    } 
    public override string BetaPortraitPath => $"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
    public virtual int BASE_REPLAY => 0;
    public virtual int UPGRADE_REPLAY_INC => 0;

    public CrimsonfangCard(int cost, CardType type, CardRarity rarity, TargetType target) : base(cost, type, rarity,
        target)
    {
        _baseReplayCount = BASE_REPLAY;
    }

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play)
    {

        if (Spellcasting.IsBeingCast)
        {
            await OnCast(context, play);
        }
        else
        {
            await Spellcasting.Conjure(context, this);
        }
    }

    public virtual async Task OnCast(PlayerChoiceContext context, CardPlay play)
    {

    }

    protected override void OnUpgrade()
    {
        BaseReplayCount += UPGRADE_REPLAY_INC;
    }

    [CustomEnum] public static CardTag Conjure;

    public class CardFilters
    {
        public bool ForceIncludeMP = false;
        public List<CardType> Types = new List<CardType>() { CardType.Attack, CardType.Power, CardType.Skill};
        public List<CardRarity> Rarities = new List<CardRarity>() { CardRarity.Common, CardRarity.Uncommon, CardRarity.Rare };
        public List<CardTag> Tags = new List<CardTag>() { };
        public List<Type> Pools = new List<Type>() { };
        public bool InvertPools;
        public Player Owner;

        public List<CardModel> FilterCards() => FilterCards(ModelDb.AllCards);
        public List<CardModel> FilterCards(IEnumerable<CardModel> cards)
        {
            List<CardModel> cardResult = new List<CardModel>();
            bool IncludeMP = ForceIncludeMP ||
                             Owner.RunState.CardMultiplayerConstraint == CardMultiplayerConstraint.MultiplayerOnly;
            foreach (var card in cards)
            {
                bool add = (IncludeMP && card.MultiplayerConstraint == CardMultiplayerConstraint.MultiplayerOnly) || card.MultiplayerConstraint != CardMultiplayerConstraint.MultiplayerOnly;
                if(Types.Count > 0 && !Types.Contains(card.Type)) add = false;
                if(Rarities.Count > 0 && !Rarities.Contains(card.Rarity)) add = false;
                if(Tags.Count > 0 && !Tags.All(t => card.Tags.Contains(t))) add = false;
                if(add) {cardResult.Add(card);}

                if (!((!InvertPools && Pools.Contains(card.Pool.GetType())) ||
                    (InvertPools && !Pools.Contains(card.Pool.GetType()))))
                {
                    add = false;
                }
            }
            return cardResult;
        }

        public List<CardModel> FilterFromPools(List<CardPoolModel>  pools)
        {
            return FilterCards(pools.SelectMany(p => p.AllCards));
        }
    }

    public static class CardPoolPresets
    {
        public static List<Type> AllPools => ModelDb.AllCardPools.Select(e => e.GetType()).ToList();
        public static List<Type> AllCharacterPools => ModelDb.AllCardPools.Select(e => e.GetType()).Where(e => e != typeof(ColorlessCardPool)).ToList();
        public static List<Type> AllPoolsMinusCrimsonfang => ModelDb.AllCardPools.Select(e => e.GetType()).Where(e => e != typeof(CrimsonfangCardPool)).ToList();
        public static List<Type> AllCharacterPoolsMinusCrimsonfang => ModelDb.AllCardPools.Select(e => e.GetType()).Where(e => e != typeof(CrimsonfangCardPool) && e != typeof(ColorlessCardPool)).ToList();
        public static List<Type> Crimsonfang => [typeof(CrimsonfangCardPool)];
    }
}