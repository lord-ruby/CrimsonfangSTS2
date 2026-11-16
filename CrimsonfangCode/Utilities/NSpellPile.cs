using BaseLib.Abstracts;
using BaseLib.Patches.Content;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.HoverTips;

namespace Crimsonfang.CrimsonfangCode.Utilities;

public partial class NSpellPile : NCombatCardPile
{
    private List<NPreviewCardHolder> _previewHolders = [];
    private const int MaxPreviewCards = 3;
    private const float PreviewDefaultScale = 0.6f;
    private const float PreviewScaleDecrement = 0.15f;
    private const float PreviewYOffset = -15f;
    private const float PreviewXOffset = 140f;
    private const float PreviewSpacingX = -35f;

    private const float HideOffsetX = -150f;

    private const float TooltipOffsetY = -100f;

    protected override PileType Pile => SpellPile.SpellPileType;

    private static readonly string _scenePath = "res://Crimsonfang/scenes/SpellPile.tscn";
    private static readonly string megaLabelFont = "res://themes/kreon_bold_glyph_space_one.tres";
    
    public static AddedNode<NCombatPilesContainer, NSpellPile> Node =  new(container =>
    {
        var SpellPileButton = ResourceLoader.Load<PackedScene>(_scenePath).Instantiate<NSpellPile>();
        SpellPileButton.Name = "%SpellPile";
        SpellPileButton.Position = new Vector2(35, 700);

        var countLabel = SpellPileButton.GetNode<CrimsonfangMegaLabel>("CountContainer/Count");
        var font = PreloadManager.Cache.GetAsset<Font>(megaLabelFont);
        countLabel.AddThemeFontOverride(ThemeConstants.Label.Font, font);
        countLabel.MinFontSize = 20;
        countLabel.MaxFontSize = 26;

        return SpellPileButton;
    });

    public HoverTip _hoverTip;
    public override void _Ready()
    {
        ConnectSignals();
        _emptyPileMessage = new LocString("combat_messages", "OPEN_EMPTY_Spell");
        _hoverTip = new HoverTip(new LocString("static_hover_tips", "Spell_PILE.title"),
            new LocString("static_hover_tips", "Spell_PILE.description"));

        Visible = false;
        SetAnimInOutPositions();
        Disable();
    }

    protected override void SetAnimInOutPositions()
    {
        _showPosition = Position;
        _hidePosition = Position + new Vector2(HideOffsetX, 0f);
    }

    public override void Initialize(Player player)
    {
        _localPlayer = player;
        _pile = Pile.GetPile(_localPlayer);
        _pile.ContentsChanged += HandleContentsChanged;

        _currentCount = _pile.Cards.Count;
        _countLabel.SetTextAutoSize(_currentCount.ToString());

        if (_pile.Cards.Count <= 0) return;
        Visible = true;
        Enable();
    }

    private void HandleContentsChanged()
    {
        _currentCount = _pile.Cards.Count;
        _countLabel.SetTextAutoSize(_currentCount.ToString());

        if (_currentCount > 0)
        {
            Visible = true;
            Enable();
        }
        else
        {
            Visible = false;
            Disable();
        }
    }
    

    protected override void OnFocus()
    {
        NHoverTipSet.Remove(this);
        var tooltip = NHoverTipSet.CreateAndShow(this, _hoverTip);
        tooltip.GlobalPosition = GlobalPosition + new Vector2(0, TooltipOffsetY);
        _bumpTween?.Kill();
        _bumpTween = CreateTween();
        _bumpTween.TweenProperty(_icon, "scale", new Vector2(1.25f, 1.25f), 0.05);
    }

    public override void AnimIn()
    {
        base.AnimIn();
        Visible = true;
    }
}