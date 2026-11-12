using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Animation;
using Stimulsoft.Controls.Win.DotNetBar.Metro.Rendering;
using Stimulsoft.Controls.Win.DotNetBar.Primitives;
using Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

namespace Stimulsoft.Controls.Win.DotNetBar.Metro;

[DefaultEvent("Click")]
[DefaultProperty("Text")]
public class MetroTileItem : BaseItem
{
	private static readonly Size DefaultTileSize = new Size(180, 90);

	private Rectangle _LastRenderBounds = Rectangle.Empty;

	private Size _TileSize = DefaultTileSize;

	private bool _IsLeftMouseButtonDown;

	private bool _IsMouseOver;

	private bool _Checked;

	private eMetroTileCheckBehavior _CheckBehavior;

	private string _OptionGroup = "";

	private CustomCollection<MetroTileFrame> _Frames;

	private int _CurrentFrame;

	private int _LastFrame;

	private Stimulsoft.Controls.Win.DotNetBar.Animation.Animation _CurrentAnimation;

	private int _CurrentFrameOffset;

	private bool _AnimationEnabled = true;

	private int _AnimationDuration = 800;

	private int _AutoRotateFramesInterval;

	private Timer _AutoRotateTimer;

	private static readonly Color DefaultDisabledBackColor = Color.Empty;

	private Color _DisabledBackColor = DefaultDisabledBackColor;

	private bool _EnableMarkup = true;

	[Description("Indicates Tile Size")]
	[Category("Appearance")]
	public Size TileSize
	{
		get
		{
			return _TileSize;
		}
		set
		{
			if (value != _TileSize)
			{
				Size tileSize = _TileSize;
				_TileSize = value;
				OnTileSizeChanged(tileSize, value);
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates predefined tile color for default tile frame.")]
	public eMetroTileColor TileColor
	{
		get
		{
			return _Frames[0].TileColor;
		}
		set
		{
			_Frames[0].TileColor = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Style")]
	[Description("Gets or sets Tile style for default tile frame.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle TileStyle => _Frames[0].TileStyle;

	[DefaultValue("")]
	[Category("Appearance")]
	[Description("Indicates the symbol displayed on face of the tile instead of the image. Setting the symbol overrides the image setting.")]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.SymbolTypeEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public string Symbol
	{
		get
		{
			return _Frames[0].Symbol;
		}
		set
		{
			_Frames[0].Symbol = value;
		}
	}

	[DefaultValue(0f)]
	[Category("Appearance")]
	[Description("Indicates the size of the symbol in points.")]
	public float SymbolSize
	{
		get
		{
			return _Frames[0].SymbolSize;
		}
		set
		{
			_Frames[0].SymbolSize = value;
		}
	}

	[Category("Columns")]
	[Description("Indicates color of the symbol.")]
	public Color SymbolColor
	{
		get
		{
			return _Frames[0].SymbolColor;
		}
		set
		{
			_Frames[0].SymbolColor = value;
		}
	}

	[DefaultValue(null)]
	[Category("Appearance")]
	[Description("Indicates image displayed on the tile.")]
	public Image Image
	{
		get
		{
			return _Frames[0].Image;
		}
		set
		{
			_Frames[0].Image = value;
		}
	}

	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Category("Appearance")]
	[Description("Indicates the image alignment in relation to text.")]
	public ContentAlignment ImageTextAlignment
	{
		get
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			return _Frames[0].ImageTextAlignment;
		}
		set
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			_Frames[0].ImageTextAlignment = value;
		}
	}

	[Browsable(true)]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.TextMarkupUIEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Category("Appearance")]
	[Description("The text contained in the item.")]
	[Localizable(true)]
	[DefaultValue("")]
	public override string Text
	{
		get
		{
			return _Frames[0].Text;
		}
		set
		{
			_Frames[0].Text = value;
		}
	}

	[Browsable(false)]
	public bool IsLeftMouseButtonDown
	{
		get
		{
			return _IsLeftMouseButtonDown;
		}
		internal set
		{
			if (value != _IsLeftMouseButtonDown)
			{
				bool isLeftMouseButtonDown = _IsLeftMouseButtonDown;
				_IsLeftMouseButtonDown = value;
				OnIsLeftMouseButtonDownChanged(isLeftMouseButtonDown, value);
			}
		}
	}

	[Browsable(false)]
	public bool IsMouseOver
	{
		get
		{
			return _IsMouseOver;
		}
		internal set
		{
			if (value != _IsMouseOver)
			{
				bool isMouseOver = _IsMouseOver;
				_IsMouseOver = value;
				OnIsMouseOverChanged(isMouseOver, value);
			}
		}
	}

	[DefaultValue("")]
	[Category("Appearance")]
	[Description("Indicates tile title text displayed by default in lower left corner")]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.TextMarkupUIEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public string TitleText
	{
		get
		{
			return _Frames[0].TitleText;
		}
		set
		{
			_Frames[0].TitleText = value;
		}
	}

	[DefaultValue(null)]
	[Category("Appearance")]
	[Description("Gets or sets the title text font.")]
	public Font TitleTextFont
	{
		get
		{
			return _Frames[0].TitleTextFont;
		}
		set
		{
			_Frames[0].TitleTextFont = value;
		}
	}

	[Category("Columns")]
	[Description("Indicates color of title text.")]
	public Color TitleTextColor
	{
		get
		{
			return _Frames[0].TitleTextColor;
		}
		set
		{
			_Frames[0].TitleTextColor = value;
		}
	}

	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Category("Appearance")]
	[Description("Indicates title text alignment.")]
	public ContentAlignment TitleTextAlignment
	{
		get
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			return _Frames[0].TitleTextAlignment;
		}
		set
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			_Frames[0].TitleTextAlignment = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates  top-left location of the image.")]
	public Point ImageIndent
	{
		get
		{
			return _Frames[0].ImageIndent;
		}
		set
		{
			_Frames[0].ImageIndent = value;
		}
	}

	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Indicates whether tile is checked.")]
	public bool Checked
	{
		get
		{
			return _Checked;
		}
		set
		{
			if (value != _Checked)
			{
				bool @checked = _Checked;
				_Checked = value;
				OnCheckedChanged(@checked, value);
			}
		}
	}

	[DefaultValue(eMetroTileCheckBehavior.Inherit)]
	[Category("Behavior")]
	[Description("Indicates automatic check behavior of metro tile.")]
	public eMetroTileCheckBehavior CheckBehavior
	{
		get
		{
			return _CheckBehavior;
		}
		set
		{
			if (value != _CheckBehavior)
			{
				eMetroTileCheckBehavior checkBehavior = _CheckBehavior;
				_CheckBehavior = value;
				OnCheckBehaviorChanged(checkBehavior, value);
			}
		}
	}

	private eMetroTileCheckBehavior EffectiveCheckBehavior
	{
		get
		{
			if (_CheckBehavior == eMetroTileCheckBehavior.Inherit)
			{
				object containerControl = ContainerControl;
				Control val = (Control)((containerControl is Control) ? containerControl : null);
				if (val is ItemPanel)
				{
					return eMetroTileCheckBehavior.RightMouseButtonClick;
				}
			}
			return _CheckBehavior;
		}
	}

	[Browsable(true)]
	[Category("Behavior")]
	[Description("Gets or set the Group item belongs to. The groups allows a user to choose from mutually exclusive options within the group.")]
	[DefaultValue("")]
	public virtual string OptionGroup
	{
		get
		{
			return _OptionGroup;
		}
		set
		{
			if (!(_OptionGroup != value))
			{
				return;
			}
			_OptionGroup = value;
			if (_OptionGroup != "" && _Checked && Parent != null)
			{
				foreach (BaseItem subItem in Parent.SubItems)
				{
					if (subItem != this && subItem is MetroTileItem metroTileItem && metroTileItem.OptionGroup == _OptionGroup && metroTileItem.Checked)
					{
						Checked = false;
					}
				}
			}
			OnAppearanceChanged();
			Refresh();
		}
	}

	[Category("Appearance")]
	[Description("List of tile frames that are displayed when frame animation is enabled using AnimationEnabled property.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public CustomCollection<MetroTileFrame> Frames => _Frames;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DefaultValue(0)]
	[Category("Appearance")]
	[Description("Indicates index of currently displayed frame in Frames collection.")]
	public int CurrentFrame
	{
		get
		{
			return _CurrentFrame;
		}
		set
		{
			if (value != _CurrentFrame)
			{
				if (value < 0 || value >= _Frames.Count)
				{
					throw new ArgumentException("Frame value is invalid. It must be greater or equal than zero and less than Frames.Count");
				}
				int currentFrame = _CurrentFrame;
				_CurrentFrame = value;
				OnCurrentFrameChanged(currentFrame, value);
			}
		}
	}

	internal int LastFrame => _LastFrame;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public int CurrentFrameOffset
	{
		get
		{
			return _CurrentFrameOffset;
		}
		set
		{
			if (_CurrentFrameOffset != value)
			{
				_CurrentFrameOffset = value;
				Invalidate();
				Update();
			}
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether Frame animation is enabled when CurrentFrame has changed.")]
	public bool AnimationEnabled
	{
		get
		{
			return _AnimationEnabled;
		}
		set
		{
			_AnimationEnabled = value;
		}
	}

	[DefaultValue(800)]
	[Category("Behavior")]
	[Description("Indicates frames animation duration in milliseconds.")]
	public int AnimationDuration
	{
		get
		{
			return _AnimationDuration;
		}
		set
		{
			if (value < 0)
			{
				value = 0;
			}
			_AnimationDuration = value;
		}
	}

	[DefaultValue(0)]
	[Category("Behavior")]
	[Description("Indicates automatic tile frame rotation interval in milliseconds. When set it will change the CurrentFrame property so each frame from Frames collection is displayed after interval set here.")]
	public int AutoRotateFramesInterval
	{
		get
		{
			return _AutoRotateFramesInterval;
		}
		set
		{
			if (value < 0)
			{
				value = 0;
			}
			if (value != _AutoRotateFramesInterval)
			{
				int autoRotateFramesInterval = _AutoRotateFramesInterval;
				_AutoRotateFramesInterval = value;
				OnAutoRotateFramesIntervalChanged(autoRotateFramesInterval, value);
			}
		}
	}

	[Category("Columns")]
	[Description("Indicates tile background color when Enabled=false.")]
	public Color DisabledBackColor
	{
		get
		{
			return _DisabledBackColor;
		}
		set
		{
			_DisabledBackColor = value;
		}
	}

	protected override bool IsMarkupSupported => _EnableMarkup;

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether text-markup support is enabled for items Text property.")]
	public bool EnableMarkup
	{
		get
		{
			return _EnableMarkup;
		}
		set
		{
			if (_EnableMarkup != value)
			{
				_EnableMarkup = value;
				NeedRecalcSize = true;
				OnTextChanged();
				OnAppearanceChanged();
			}
		}
	}

	[Description("Occurs after Checked property has changed.")]
	public event EventHandler CheckedChanged;

	[Description("Occurs before an item in option group is checked and provides opportunity to cancel that.")]
	public event OptionGroupChangingEventHandler OptionGroupChanging;

	public event EventHandler TitleTextMarkupLinkClick;

	protected virtual void OnCheckedChanged(EventArgs e)
	{
		this.CheckedChanged?.Invoke(this, e);
	}

	protected virtual void OnOptionGroupChanging(OptionGroupChangingEventArgs e)
	{
		this.OptionGroupChanging?.Invoke(this, e);
		if (!e.Cancel)
		{
			GetIOwnerItemEvents()?.InvokeOptionGroupChanging(this, e);
		}
	}

	public MetroTileItem()
		: this("", "")
	{
	}

	public MetroTileItem(string sItemName)
		: this(sItemName, "")
	{
	}

	public MetroTileItem(string sItemName, string ItemText)
		: base(sItemName, ItemText)
	{
		_Frames = new CustomCollection<MetroTileFrame>();
		_Frames.CollectionChanged += FramesCollectionChanged;
		MetroTileFrame metroTileFrame = new MetroTileFrame();
		metroTileFrame.MarkupLinkClick += MarkupLinkClick;
		_Frames.Add(metroTileFrame);
	}

	private void MarkupLinkClick(object sender, MarkupLinkClickEventArgs e)
	{
		OnTitleTextMarkupLinkClick(e);
	}

	public override BaseItem Copy()
	{
		MetroTileItem metroTileItem = new MetroTileItem(m_Name);
		CopyToItem(metroTileItem);
		return metroTileItem;
	}

	internal void InternalCopyToItem(MetroTileItem copy)
	{
		CopyToItem(copy);
	}

	protected override void CopyToItem(BaseItem copy)
	{
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		base.CopyToItem(copy);
		MetroTileItem metroTileItem = copy as MetroTileItem;
		metroTileItem.AnimationDuration = AnimationDuration;
		metroTileItem.AnimationEnabled = AnimationEnabled;
		metroTileItem.AutoRotateFramesInterval = AutoRotateFramesInterval;
		metroTileItem.CheckBehavior = CheckBehavior;
		metroTileItem.Checked = Checked;
		metroTileItem.DisabledBackColor = DisabledBackColor;
		metroTileItem.EnableMarkup = EnableMarkup;
		metroTileItem.Image = Image;
		metroTileItem.Symbol = Symbol;
		metroTileItem.SymbolSize = SymbolSize;
		metroTileItem.ImageIndent = ImageIndent;
		metroTileItem.ImageTextAlignment = ImageTextAlignment;
		metroTileItem.OptionGroup = OptionGroup;
		metroTileItem.TileColor = TileColor;
		metroTileItem.TileSize = TileSize;
		metroTileItem.TitleText = TitleText;
		metroTileItem.TitleTextAlignment = TitleTextAlignment;
		metroTileItem.TitleTextColor = TitleTextColor;
		metroTileItem.TitleTextFont = TitleTextFont;
		metroTileItem.Text = Text;
		for (int i = 1; i < Frames.Count; i++)
		{
			metroTileItem.Frames.Add((MetroTileFrame)Frames[i].Clone());
		}
	}

	protected override void Dispose(bool disposing)
	{
		DisposeCurrentAnimation();
		DestroyAutoRotateTimer();
		base.Dispose(disposing);
	}

	private void FramesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
	{
		if (e.OldItems != null && e.OldItems.Count > 0)
		{
			foreach (MetroTileFrame oldItem in e.OldItems)
			{
				oldItem.PropertyChanged -= FramePropertyChanged;
			}
		}
		if (e.NewItems != null && e.NewItems.Count > 0)
		{
			foreach (MetroTileFrame newItem in e.NewItems)
			{
				newItem.PropertyChanged += FramePropertyChanged;
			}
		}
		if (_CurrentFrame >= _Frames.Count)
		{
			_CurrentFrame = 0;
			Refresh();
		}
	}

	private void FramePropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "TileStyle" || e.PropertyName == "TitleText" || e.PropertyName == "TitleTextAlignment" || e.PropertyName == "Image" || e.PropertyName == "ImageIndent" || e.PropertyName == "Text" || e.PropertyName == "TextMarkupEnabled" || e.PropertyName == "ImageTextAlignment" || e.PropertyName == "Symbol" || e.PropertyName == "SymbolSize")
		{
			NeedRecalcSize = true;
		}
		Refresh();
	}

	protected override void OnClick()
	{
		base.OnClick();
		ExecuteCommand();
	}

	public override void Paint(ItemPaintArgs p)
	{
		if (p.AnimationEnabled && p.AnimationRequests != null && !_LastRenderBounds.IsEmpty && m_Rect != _LastRenderBounds)
		{
			Rectangle rect = m_Rect;
			m_Rect = _LastRenderBounds;
			_LastRenderBounds = Rectangle.Empty;
			AnimationRequest item = new AnimationRequest(this, "Bounds", m_Rect, rect);
			p.AnimationRequests.Add(item);
			return;
		}
		MetroRender.Paint(this, p);
		if (Focused && DesignMode)
		{
			Rectangle rect2 = m_Rect;
			rect2.Inflate(-1, -1);
			DesignTime.DrawDesignTimeSelection(p.Graphics, rect2, p.Colors.ItemDesignTimeBorder);
		}
		_LastRenderBounds = m_Rect;
		if (!(p.ContainerControl is MetroTilePanel))
		{
			DrawInsertMarker(p.Graphics);
		}
	}

	public override void RecalcSize()
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Invalid comparison between Unknown and I4
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Invalid comparison between Unknown and I4
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Invalid comparison between Unknown and I4
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Invalid comparison between Unknown and I4
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Invalid comparison between Unknown and I4
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Invalid comparison between Unknown and I4
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Invalid comparison between Unknown and I4
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Invalid comparison between Unknown and I4
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Invalid comparison between Unknown and I4
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Invalid comparison between Unknown and I4
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c8: Invalid comparison between Unknown and I4
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Invalid comparison between Unknown and I4
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Invalid comparison between Unknown and I4
		//IL_0431: Unknown result type (might be due to invalid IL or missing references)
		//IL_043b: Invalid comparison between Unknown and I4
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Invalid comparison between Unknown and I4
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_048c: Invalid comparison between Unknown and I4
		//IL_04e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ee: Invalid comparison between Unknown and I4
		//IL_0559: Unknown result type (might be due to invalid IL or missing references)
		//IL_0560: Invalid comparison between Unknown and I4
		//IL_05a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b0: Invalid comparison between Unknown and I4
		//IL_060d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0613: Invalid comparison between Unknown and I4
		//IL_0661: Unknown result type (might be due to invalid IL or missing references)
		//IL_0667: Invalid comparison between Unknown and I4
		//IL_0697: Unknown result type (might be due to invalid IL or missing references)
		//IL_069d: Invalid comparison between Unknown and I4
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val == null || val.Disposing || val.IsDisposed)
		{
			return;
		}
		Graphics val2 = BarFunctions.CreateGraphics(val);
		if (val2 == null)
		{
			return;
		}
		bool rightToLeft = (int)val.RightToLeft == 1;
		for (int i = 0; i < _Frames.Count; i++)
		{
			MetroTileFrame metroTileFrame = _Frames[i];
			bool dispose = false;
			ElementStyle elementStyle = ElementStyleDisplay.GetElementStyle(metroTileFrame.EffectiveStyle, out dispose);
			int num = _TileSize.Width - elementStyle.PaddingHorizontal;
			int num2 = 0;
			if (m_SubItems != null && i < m_SubItems.Count)
			{
				BaseItem baseItem = m_SubItems[i];
				if (baseItem.Visible)
				{
					baseItem.LeftInternal = m_Rect.Left + elementStyle.PaddingLeft;
					baseItem.TopInternal = m_Rect.Top + elementStyle.PaddingTop;
					baseItem.WidthInternal = _TileSize.Width - elementStyle.PaddingHorizontal;
					baseItem.HeightInternal = _TileSize.Height - elementStyle.PaddingVertical;
					baseItem.RecalcSize();
					baseItem.Displayed = true;
					num2 = Math.Max(num2, baseItem.WidthInternal);
				}
				else
				{
					baseItem.Displayed = false;
				}
			}
			num = Math.Max(12, num - num2);
			ContentAlignment imageTextAlignment = metroTileFrame.ImageTextAlignment;
			if (!string.IsNullOrEmpty(metroTileFrame.Symbol))
			{
				Font fontAwesome = Symbols.GetFontAwesome(metroTileFrame.SymbolSize);
				Size size = TextDrawing.MeasureString(val2, "\uf00a", fontAwesome);
				int num3 = (int)Math.Ceiling((float)fontAwesome.FontFamily.GetCellDescent(fontAwesome.Style) * fontAwesome.Size / (float)fontAwesome.FontFamily.GetEmHeight(fontAwesome.Style));
				size.Height -= num3;
				if ((int)imageTextAlignment == 1 || (int)imageTextAlignment == 4 || (int)imageTextAlignment == 256 || (int)imageTextAlignment == 1024 || (int)imageTextAlignment == 16 || (int)imageTextAlignment == 64)
				{
					num = Math.Max(12, num - size.Width);
				}
			}
			else if (metroTileFrame.Image != null && ((int)imageTextAlignment == 1 || (int)imageTextAlignment == 4 || (int)imageTextAlignment == 256 || (int)imageTextAlignment == 1024 || (int)imageTextAlignment == 16 || (int)imageTextAlignment == 64))
			{
				num = Math.Max(12, num - metroTileFrame.Image.Width);
			}
			if (metroTileFrame.Text != "")
			{
				MetroTileFrame.MeasureText(metroTileFrame, val2, num, val.Font, eTextFormat.Default, rightToLeft);
			}
			Size size2 = Size.Empty;
			metroTileFrame.TitleTextBounds = Rectangle.Empty;
			if (metroTileFrame.TitleTextMarkupBody != null)
			{
				Font titleTextFont = GetTitleTextFont(metroTileFrame, elementStyle, val);
				Size availableSize = new Size(num, 1);
				MarkupDrawContext markupDrawContext = new MarkupDrawContext(val2, titleTextFont, Color.Empty, rightToLeft: false);
				metroTileFrame.TitleTextMarkupBody.Measure(availableSize, markupDrawContext);
				availableSize.Height = metroTileFrame.TitleTextMarkupBody.Bounds.Size.Height;
				markupDrawContext.RightToLeft = rightToLeft;
				metroTileFrame.TitleTextMarkupBody.Arrange(new Rectangle(0, 0, availableSize.Width, availableSize.Height), markupDrawContext);
				size2 = metroTileFrame.TitleTextMarkupBody.Bounds.Size;
			}
			else if (!string.IsNullOrEmpty(metroTileFrame.TitleText))
			{
				Font titleTextFont2 = GetTitleTextFont(metroTileFrame, elementStyle, val);
				Size size3 = new Size(_TileSize.Width - elementStyle.PaddingHorizontal, _TileSize.Height);
				size2 = TextDrawing.MeasureString(val2, metroTileFrame.TitleText, titleTextFont2, size3.Width, eTextFormat.SingleLine);
			}
			if (!size2.IsEmpty)
			{
				size2.Width = Math.Min(size2.Width + 2, _TileSize.Width);
				if ((int)metroTileFrame.TitleTextAlignment == 512)
				{
					metroTileFrame.TitleTextBounds = new Rectangle(elementStyle.PaddingLeft + (_TileSize.Width - elementStyle.PaddingHorizontal - size2.Width) / 2, _TileSize.Height - size2.Height - elementStyle.PaddingBottom, _TileSize.Width, size2.Height + 1);
				}
				else if ((int)metroTileFrame.TitleTextAlignment == 256)
				{
					metroTileFrame.TitleTextBounds = new Rectangle(elementStyle.PaddingLeft, _TileSize.Height - size2.Height - elementStyle.PaddingBottom, size2.Width, size2.Height + 1);
				}
				else if ((int)metroTileFrame.TitleTextAlignment == 1024)
				{
					metroTileFrame.TitleTextBounds = new Rectangle(_TileSize.Width - elementStyle.PaddingRight - size2.Width, _TileSize.Height - size2.Height - elementStyle.PaddingBottom, size2.Width, size2.Height + 1);
				}
				else if ((int)metroTileFrame.TitleTextAlignment == 32)
				{
					metroTileFrame.TitleTextBounds = new Rectangle(elementStyle.PaddingLeft + (_TileSize.Width - elementStyle.PaddingHorizontal - size2.Width) / 2, elementStyle.PaddingTop + (_TileSize.Height - size2.Height) / 2, _TileSize.Width, size2.Height + 1);
				}
				else if ((int)metroTileFrame.TitleTextAlignment == 16)
				{
					metroTileFrame.TitleTextBounds = new Rectangle(elementStyle.PaddingLeft, elementStyle.PaddingTop + (_TileSize.Height - size2.Height) / 2, size2.Width, size2.Height + 1);
				}
				else if ((int)metroTileFrame.TitleTextAlignment == 64)
				{
					metroTileFrame.TitleTextBounds = new Rectangle(_TileSize.Width - elementStyle.PaddingRight - size2.Width, elementStyle.PaddingTop + (_TileSize.Height - size2.Height) / 2, size2.Width, size2.Height + 1);
				}
				else if ((int)metroTileFrame.TitleTextAlignment == 2)
				{
					metroTileFrame.TitleTextBounds = new Rectangle(elementStyle.PaddingLeft + (_TileSize.Width - elementStyle.PaddingHorizontal - size2.Width) / 2, elementStyle.PaddingTop, size2.Width, size2.Height + 1);
				}
				else if ((int)metroTileFrame.TitleTextAlignment == 1)
				{
					metroTileFrame.TitleTextBounds = new Rectangle(elementStyle.PaddingLeft, elementStyle.PaddingTop, size2.Width, size2.Height + 1);
				}
				else if ((int)metroTileFrame.TitleTextAlignment == 4)
				{
					metroTileFrame.TitleTextBounds = new Rectangle(_TileSize.Width - elementStyle.PaddingRight - size2.Width, elementStyle.PaddingTop, size2.Width, size2.Height + 1);
				}
			}
			if (dispose)
			{
				elementStyle.Dispose();
			}
		}
		m_Rect = new Rectangle(m_Rect.X, m_Rect.Y, _TileSize.Width, _TileSize.Height);
		base.RecalcSize();
	}

	internal Font GetTitleTextFont(MetroTileFrame frame, ElementStyle style, Control control)
	{
		Font val = frame.TitleTextFont;
		if (val == null && style != null)
		{
			val = style.Font;
		}
		if (val == null && control != null)
		{
			val = control.Font;
		}
		if (val == null)
		{
			val = SystemFonts.DefaultFont;
		}
		return val;
	}

	private void TileStyleChanged(object sender, EventArgs e)
	{
		NeedRecalcSize = true;
		OnAppearanceChanged();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTileSize()
	{
		return _TileSize != DefaultTileSize;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTileSize()
	{
		TileSize = DefaultTileSize;
	}

	protected virtual void OnTileSizeChanged(Size oldValue, Size newValue)
	{
		NeedRecalcSize = true;
		OnAppearanceChanged();
	}

	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Invalid comparison between Unknown and I4
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Invalid comparison between Unknown and I4
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Invalid comparison between Unknown and I4
		if ((int)objArg.Button == 1048576 && GetEnabled())
		{
			IsLeftMouseButtonDown = true;
		}
		eMetroTileCheckBehavior effectiveCheckBehavior = EffectiveCheckBehavior;
		if (effectiveCheckBehavior == eMetroTileCheckBehavior.LeftMouseButtonClick && (int)objArg.Button == 1048576)
		{
			Checked = !Checked;
		}
		else if (effectiveCheckBehavior == eMetroTileCheckBehavior.MiddleMouseButtonClick && (int)objArg.Button == 4194304)
		{
			Checked = !Checked;
		}
		else if (effectiveCheckBehavior == eMetroTileCheckBehavior.RightMouseButtonClick && (int)objArg.Button == 2097152)
		{
			Checked = !Checked;
		}
		base.InternalMouseDown(objArg);
	}

	public override void InternalMouseUp(MouseEventArgs objArg)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Invalid comparison between Unknown and I4
		if ((int)objArg.Button == 1048576 || (int)Control.MouseButtons != 1048576)
		{
			IsLeftMouseButtonDown = false;
		}
		base.InternalMouseUp(objArg);
	}

	public override void InternalMouseMove(MouseEventArgs objArg)
	{
		if (GetEnabled())
		{
			IsMouseOver = true;
		}
		try
		{
			if (!DesignMode)
			{
				SetIsContainer(b: true);
			}
			base.InternalMouseMove(objArg);
		}
		finally
		{
			SetIsContainer(b: false);
		}
	}

	public override void InternalMouseLeave()
	{
		IsMouseOver = false;
		IsLeftMouseButtonDown = false;
		base.InternalMouseLeave();
	}

	protected virtual void OnIsLeftMouseButtonDownChanged(bool oldValue, bool newValue)
	{
		Refresh();
	}

	protected virtual void OnIsMouseOverChanged(bool oldValue, bool newValue)
	{
		Refresh();
	}

	protected virtual void OnTitleTextMarkupLinkClick(EventArgs e)
	{
		this.TitleTextMarkupLinkClick?.Invoke(this, e);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTitleTextColor()
	{
		return _Frames[0].ShouldSerializeTitleTextColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTitleTextColor()
	{
		_Frames[0].ResetTitleTextColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeImageIndent()
	{
		return _Frames[0].ShouldSerializeImageIndent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetImageIndent()
	{
		_Frames[0].ResetImageIndent();
	}

	protected virtual void OnCheckedChanged(bool oldValue, bool newValue)
	{
		if (newValue && _OptionGroup.Length > 0 && Parent != null)
		{
			MetroTileItem metroTileItem = null;
			foreach (BaseItem subItem in Parent.SubItems)
			{
				if (subItem != this)
				{
					metroTileItem = subItem as MetroTileItem;
					if (metroTileItem != null && metroTileItem.OptionGroup == _OptionGroup && metroTileItem.Checked)
					{
						break;
					}
				}
			}
			OptionGroupChangingEventArgs optionGroupChangingEventArgs = new OptionGroupChangingEventArgs(metroTileItem, this);
			OnOptionGroupChanging(optionGroupChangingEventArgs);
			if (optionGroupChangingEventArgs.Cancel)
			{
				_Checked = oldValue;
				return;
			}
		}
		if (ShouldSyncProperties)
		{
			BarFunctions.SyncProperty(this, "Checked");
		}
		if (_OptionGroup != "" && newValue && Parent != null)
		{
			foreach (BaseItem subItem2 in Parent.SubItems)
			{
				if (subItem2 != this && subItem2 is MetroTileItem metroTileItem2 && metroTileItem2.OptionGroup == _OptionGroup && metroTileItem2.Checked)
				{
					metroTileItem2.Checked = false;
				}
			}
		}
		Refresh();
		OnCheckedChanged(EventArgs.Empty);
	}

	protected virtual void OnCheckBehaviorChanged(eMetroTileCheckBehavior oldValue, eMetroTileCheckBehavior newValue)
	{
	}

	private void DisposeCurrentAnimation()
	{
		Stimulsoft.Controls.Win.DotNetBar.Animation.Animation currentAnimation = _CurrentAnimation;
		_CurrentAnimation = null;
		if (currentAnimation != null)
		{
			currentAnimation.Stop();
			currentAnimation.Dispose();
		}
	}

	protected virtual void OnCurrentFrameChanged(int oldValue, int newValue)
	{
		_LastFrame = oldValue;
		if (_AnimationEnabled && _AnimationDuration > 0 && !DesignMode)
		{
			DisposeCurrentAnimation();
			int height = _TileSize.Height;
			(_CurrentAnimation = new AnimationInt(new AnimationRequest(this, "CurrentFrameOffset", height, 0), AnimationEasing.EaseOutExpo, _AnimationDuration)).Start();
		}
		else
		{
			Refresh();
		}
	}

	private void DestroyAutoRotateTimer()
	{
		Timer autoRotateTimer = _AutoRotateTimer;
		_AutoRotateTimer = null;
		if (autoRotateTimer != null)
		{
			autoRotateTimer.Stop();
			((Component)(object)autoRotateTimer).Dispose();
		}
	}

	private void CreateAutoRotateTimer()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		if (_AutoRotateTimer == null)
		{
			_AutoRotateTimer = new Timer();
			_AutoRotateTimer.Interval = _AutoRotateFramesInterval;
			if (_Frames[_CurrentFrame].FrameDisplayDuration > 0)
			{
				_AutoRotateTimer.Interval = _Frames[_CurrentFrame].FrameDisplayDuration;
			}
			_AutoRotateTimer.Tick += AutoRotateTimerTick;
			_AutoRotateTimer.Start();
		}
	}

	private void AutoRotateTimerTick(object sender, EventArgs e)
	{
		if (Frames.Count > 1)
		{
			int num = CurrentFrame + 1;
			if (num >= Frames.Count)
			{
				num = 0;
			}
			CurrentFrame = num;
			if (_Frames[num].FrameDisplayDuration > 0)
			{
				_AutoRotateTimer.Interval = _Frames[num].FrameDisplayDuration;
			}
			else
			{
				_AutoRotateTimer.Interval = _AutoRotateFramesInterval;
			}
		}
	}

	protected virtual void OnAutoRotateFramesIntervalChanged(int oldValue, int newValue)
	{
		if (newValue == 0 || DesignMode)
		{
			DestroyAutoRotateTimer();
		}
		else if (Visible)
		{
			CreateAutoRotateTimer();
		}
	}

	protected internal override void OnVisibleChanged(bool newValue)
	{
		if (newValue && !DesignMode)
		{
			if (_AutoRotateFramesInterval > 0 && _Frames.Count > 1 && _AutoRotateTimer == null)
			{
				CreateAutoRotateTimer();
			}
		}
		else if (_AutoRotateTimer != null)
		{
			DestroyAutoRotateTimer();
		}
		base.OnVisibleChanged(newValue);
	}

	protected override void OnDragStartPointChanged(Point oldValue, Point newValue)
	{
		if (newValue.IsEmpty)
		{
			if (_AutoRotateTimer != null)
			{
				_AutoRotateTimer.Enabled = true;
			}
		}
		else if (_AutoRotateTimer != null)
		{
			_AutoRotateTimer.Enabled = false;
		}
		base.OnDragStartPointChanged(oldValue, newValue);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeDisabledBackColor()
	{
		return _DisabledBackColor != DefaultDisabledBackColor;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetDisabledBackColor()
	{
		DisabledBackColor = DefaultDisabledBackColor;
	}
}
