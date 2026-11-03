using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Animation;
using DevComponents.DotNetBar.Metro.Rendering;
using DevComponents.DotNetBar.Primitives;
using DevComponents.DotNetBar.Rendering;
using DevComponents.DotNetBar.TextMarkup;

namespace DevComponents.DotNetBar.Metro;

[DefaultEvent("Click")]
[Designer("DevComponents.DotNetBar.Design.SimpleItemDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ToolboxItem(false)]
[DefaultProperty("Text")]
public class MetroTileItem : BaseItem
{
	private EventHandler _0E65;

	private OptionGroupChangingEventHandler _09D5;

	private static readonly Size ᨥ = new Size(180, 90);

	private MarkupLinkClickEventHandler Ӟ;

	private string _ຝ = "";

	private eNotificationMarkPosition _ພ;

	private int _ຟ;

	private Color _ຠ = Color.Empty;

	private static readonly Point ຢ = new Point(-2, -2);

	private Point _ຣ = ຢ;

	private bool _ມ;

	private Rectangle _ᨦ = Rectangle.Empty;

	private static readonly Size ᨧ = new Size(16, 16);

	private Size _ח = ᨥ;

	private bool _ᨨ;

	private bool _ᆿ;

	private EventHandler _0D83;

	private bool _ᚆ;

	private eMetroTileCheckBehavior _ᨩ;

	private string _ᨪ = "";

	private CustomCollection<MetroTileFrame> _ᨫ;

	private int _ᨬ;

	private int _ᨭ;

	private _1A18 __19EC;

	private int _ᨮ;

	private bool _ᢻ = true;

	private int _ᨯ = 800;

	private int _ᨰ;

	private Timer _ᨱ;

	private static readonly Color ᨲ = Color.Empty;

	private Color _ᖣ = ᨲ;

	private bool __0EB9 = true;

	[Description("Specifies maximum of 2 character text displayed inside of the notification mark on top of the button.")]
	[DefaultValue("")]
	[Category("Appearance")]
	public string NotificationMarkText
	{
		get
		{
			return _ຝ;
		}
		set
		{
			if (value != _ຝ)
			{
				string ຝ = _ຝ;
				_ຝ = value;
				OnNotificationMarkTextChanged(ຝ, value);
			}
		}
	}

	[Description("Indicates the position of the notification marker within the bounds of the button.")]
	[DefaultValue(eNotificationMarkPosition.TopLeft)]
	[Category("Appearance")]
	public eNotificationMarkPosition NotificationMarkPosition
	{
		get
		{
			return _ພ;
		}
		set
		{
			if (value != _ພ)
			{
				eNotificationMarkPosition ພ = _ພ;
				_ພ = value;
				OnNotificationMarkPositionChanged(ພ, value);
			}
		}
	}

	[Description("Specifies diameter of notification mark. When set to 0 system default value is used.")]
	[DefaultValue(0)]
	[Category("Appearance")]
	public int NotificationMarkSize
	{
		get
		{
			return _ຟ;
		}
		set
		{
			if (value != _ຟ)
			{
				int ຟ = _ຟ;
				_ຟ = value;
				OnNotificationMarkSizeChanged(ຟ, value);
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates background color of the notification mark.")]
	public Color NotificationMarkColor
	{
		get
		{
			return _ຠ;
		}
		set
		{
			_ຠ = value;
			Refresh();
		}
	}

	[Category("Appearance")]
	[Description("Specifies the offset for the notification mark relative to its position.")]
	public Point NotificationMarkOffset
	{
		get
		{
			return _ຣ;
		}
		set
		{
			if (value != _ຣ)
			{
				Point ຣ = _ຣ;
				_ຣ = value;
				OnNotificationMarkOffsetChanged(ຣ, value);
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates Tile Size")]
	public Size TileSize
	{
		get
		{
			return _ח;
		}
		set
		{
			if (value != _ח)
			{
				Size ח = _ח;
				_ח = value;
				OnTileSizeChanged(ח, value);
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates predefined tile color for default tile frame.")]
	public eMetroTileColor TileColor
	{
		get
		{
			return _ᨫ[0].TileColor;
		}
		set
		{
			_ᨫ[0].TileColor = value;
		}
	}

	[Category("Style")]
	[Description("Gets or sets Tile style for default tile frame.")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ElementStyle TileStyle => _ᨫ[0].TileStyle;

	[Editor("DevComponents.DotNetBar.Design.SymbolTypeEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Description("Indicates the symbol displayed on face of the tile instead of the image. Setting the symbol overrides the image setting.")]
	[DefaultValue("")]
	[Category("Appearance")]
	public string Symbol
	{
		get
		{
			return _ᨫ[0].Symbol;
		}
		set
		{
			_ᨫ[0].Symbol = value;
		}
	}

	[DefaultValue(eSymbolSet.Awesome)]
	[Browsable(false)]
	public eSymbolSet SymbolSet
	{
		get
		{
			return _ᨫ[0].SymbolSet;
		}
		set
		{
			_ᨫ[0].SymbolSet = value;
		}
	}

	[Description("Indicates the size of the symbol in points.")]
	[Category("Appearance")]
	[DefaultValue(0f)]
	public float SymbolSize
	{
		get
		{
			return _ᨫ[0].SymbolSize;
		}
		set
		{
			_ᨫ[0].SymbolSize = value;
		}
	}

	[Category("Columns")]
	[Description("Indicates color of the symbol.")]
	public Color SymbolColor
	{
		get
		{
			return _ᨫ[0].SymbolColor;
		}
		set
		{
			_ᨫ[0].SymbolColor = value;
		}
	}

	[Description("Indicates image displayed on the tile.")]
	[Category("Appearance")]
	[DefaultValue(null)]
	public Image Image
	{
		get
		{
			return _ᨫ[0].Image;
		}
		set
		{
			_ᨫ[0].Image = value;
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
			return _ᨫ[0].ImageTextAlignment;
		}
		set
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			_ᨫ[0].ImageTextAlignment = value;
		}
	}

	[Category("Appearance")]
	[Browsable(true)]
	[Localizable(true)]
	[Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[DefaultValue("")]
	[Description("The text contained in the item.")]
	public override string Text
	{
		get
		{
			return _ᨫ[0].Text;
		}
		set
		{
			_ᨫ[0].Text = value;
		}
	}

	[Browsable(false)]
	public bool IsLeftMouseButtonDown
	{
		get
		{
			return _ᨨ;
		}
		internal set
		{
			if (value != _ᨨ)
			{
				bool ᨨ = _ᨨ;
				_ᨨ = value;
				OnIsLeftMouseButtonDownChanged(ᨨ, value);
			}
		}
	}

	[Browsable(false)]
	public bool IsMouseOver
	{
		get
		{
			return _ᆿ;
		}
		internal set
		{
			if (value != _ᆿ)
			{
				bool ᆿ = _ᆿ;
				_ᆿ = value;
				OnIsMouseOverChanged(ᆿ, value);
			}
		}
	}

	[DefaultValue("")]
	[Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Description("Indicates tile title text displayed by default in lower left corner")]
	[Category("Appearance")]
	public string TitleText
	{
		get
		{
			return _ᨫ[0].TitleText;
		}
		set
		{
			_ᨫ[0].TitleText = value;
		}
	}

	[DefaultValue(null)]
	[Description("Gets or sets the title text font.")]
	[Category("Appearance")]
	public Font TitleTextFont
	{
		get
		{
			return _ᨫ[0].TitleTextFont;
		}
		set
		{
			_ᨫ[0].TitleTextFont = value;
		}
	}

	[Description("Indicates color of title text.")]
	[Category("Columns")]
	public Color TitleTextColor
	{
		get
		{
			return _ᨫ[0].TitleTextColor;
		}
		set
		{
			_ᨫ[0].TitleTextColor = value;
		}
	}

	[Category("Appearance")]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Description("Indicates title text alignment.")]
	public ContentAlignment TitleTextAlignment
	{
		get
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			return _ᨫ[0].TitleTextAlignment;
		}
		set
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			_ᨫ[0].TitleTextAlignment = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates  top-left location of the image.")]
	public Point ImageIndent
	{
		get
		{
			return _ᨫ[0].ImageIndent;
		}
		set
		{
			_ᨫ[0].ImageIndent = value;
		}
	}

	[Category("Appearance")]
	[DefaultValue(false)]
	[Description("Indicates whether tile is checked.")]
	public bool Checked
	{
		get
		{
			return _ᚆ;
		}
		set
		{
			if (value != _ᚆ)
			{
				bool ᚆ = _ᚆ;
				_ᚆ = value;
				OnCheckedChanged(ᚆ, value);
			}
		}
	}

	[Description("Indicates automatic check behavior of metro tile.")]
	[DefaultValue(eMetroTileCheckBehavior.Inherit)]
	[Category("Behavior")]
	public eMetroTileCheckBehavior CheckBehavior
	{
		get
		{
			return _ᨩ;
		}
		set
		{
			if (value != _ᨩ)
			{
				eMetroTileCheckBehavior ᨩ = _ᨩ;
				_ᨩ = value;
				OnCheckBehaviorChanged(ᨩ, value);
			}
		}
	}

	private eMetroTileCheckBehavior ᨣ
	{
		get
		{
			if (_ᨩ == eMetroTileCheckBehavior.Inherit)
			{
				object containerControl = ContainerControl;
				Control val = (Control)((containerControl is Control) ? containerControl : null);
				if (val is ItemPanel)
				{
					return eMetroTileCheckBehavior.RightMouseButtonClick;
				}
			}
			return _ᨩ;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Description("Gets or set the Group item belongs to. The groups allows a user to choose from mutually exclusive options within the group.")]
	[Category("Behavior")]
	public virtual string OptionGroup
	{
		get
		{
			return _ᨪ;
		}
		set
		{
			if (!(_ᨪ != value))
			{
				return;
			}
			_ᨪ = value;
			if (_ᨪ != "" && _ᚆ && Parent != null)
			{
				foreach (BaseItem subItem in Parent.SubItems)
				{
					if (subItem != this && subItem is MetroTileItem metroTileItem && metroTileItem.OptionGroup == _ᨪ && metroTileItem.Checked)
					{
						Checked = false;
					}
				}
			}
			OnAppearanceChanged();
			Refresh();
		}
	}

	[Description("List of tile frames that are displayed when frame animation is enabled using AnimationEnabled property.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Appearance")]
	public CustomCollection<MetroTileFrame> Frames => _ᨫ;

	[Description("Indicates index of currently displayed frame in Frames collection.")]
	[DefaultValue(0)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Category("Appearance")]
	public int CurrentFrame
	{
		get
		{
			return _ᨬ;
		}
		set
		{
			if (value != _ᨬ)
			{
				if (value < 0 || value >= _ᨫ.Count)
				{
					throw new ArgumentException("Frame value is invalid. It must be greater or equal than zero and less than Frames.Count");
				}
				int ᨬ = _ᨬ;
				_ᨬ = value;
				OnCurrentFrameChanged(ᨬ, value);
			}
		}
	}

	internal int ᨤ => _ᨭ;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public int CurrentFrameOffset
	{
		get
		{
			return _ᨮ;
		}
		set
		{
			if (_ᨮ != value)
			{
				_ᨮ = value;
				Invalidate();
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
			return _ᢻ;
		}
		set
		{
			_ᢻ = value;
		}
	}

	[DefaultValue(800)]
	[Description("Indicates frames animation duration in milliseconds.")]
	[Category("Behavior")]
	public int AnimationDuration
	{
		get
		{
			return _ᨯ;
		}
		set
		{
			if (value < 0)
			{
				value = 0;
			}
			_ᨯ = value;
		}
	}

	[Description("Indicates automatic tile frame rotation interval in milliseconds. When set it will change the CurrentFrame property so each frame from Frames collection is displayed after interval set here.")]
	[Category("Behavior")]
	[DefaultValue(0)]
	public int AutoRotateFramesInterval
	{
		get
		{
			return _ᨰ;
		}
		set
		{
			if (value < 0)
			{
				value = 0;
			}
			if (value != _ᨰ)
			{
				int ᨰ = _ᨰ;
				_ᨰ = value;
				OnAutoRotateFramesIntervalChanged(ᨰ, value);
			}
		}
	}

	[Description("Indicates tile background color when Enabled=false.")]
	[Category("Columns")]
	public Color DisabledBackColor
	{
		get
		{
			return _ᖣ;
		}
		set
		{
			_ᖣ = value;
		}
	}

	protected override bool IsMarkupSupported => __0EB9;

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether text-markup support is enabled for items Text property.")]
	public bool EnableMarkup
	{
		get
		{
			return __0EB9;
		}
		set
		{
			if (__0EB9 != value)
			{
				__0EB9 = value;
				NeedRecalcSize = true;
				OnTextChanged();
				OnAppearanceChanged();
			}
		}
	}

	[Description("Occurs after Checked property has changed.")]
	public event EventHandler CheckedChanged
	{
		add
		{
			EventHandler eventHandler = _0E65;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0E65, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _0E65;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0E65, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs before an item in option group is checked and provides opportunity to cancel that.")]
	public event OptionGroupChangingEventHandler OptionGroupChanging
	{
		add
		{
			OptionGroupChangingEventHandler optionGroupChangingEventHandler = _09D5;
			OptionGroupChangingEventHandler optionGroupChangingEventHandler2;
			do
			{
				optionGroupChangingEventHandler2 = optionGroupChangingEventHandler;
				OptionGroupChangingEventHandler value2 = (OptionGroupChangingEventHandler)Delegate.Combine(optionGroupChangingEventHandler2, value);
				optionGroupChangingEventHandler = Interlocked.CompareExchange(ref _09D5, value2, optionGroupChangingEventHandler2);
			}
			while ((object)optionGroupChangingEventHandler != optionGroupChangingEventHandler2);
		}
		remove
		{
			OptionGroupChangingEventHandler optionGroupChangingEventHandler = _09D5;
			OptionGroupChangingEventHandler optionGroupChangingEventHandler2;
			do
			{
				optionGroupChangingEventHandler2 = optionGroupChangingEventHandler;
				OptionGroupChangingEventHandler value2 = (OptionGroupChangingEventHandler)Delegate.Remove(optionGroupChangingEventHandler2, value);
				optionGroupChangingEventHandler = Interlocked.CompareExchange(ref _09D5, value2, optionGroupChangingEventHandler2);
			}
			while ((object)optionGroupChangingEventHandler != optionGroupChangingEventHandler2);
		}
	}

	[Description("Occurs when text markup link is clicked. Markup links can be created using a tag.")]
	public event MarkupLinkClickEventHandler MarkupLinkClick
	{
		add
		{
			MarkupLinkClickEventHandler markupLinkClickEventHandler = Ӟ;
			MarkupLinkClickEventHandler markupLinkClickEventHandler2;
			do
			{
				markupLinkClickEventHandler2 = markupLinkClickEventHandler;
				MarkupLinkClickEventHandler value2 = (MarkupLinkClickEventHandler)Delegate.Combine(markupLinkClickEventHandler2, value);
				markupLinkClickEventHandler = Interlocked.CompareExchange(ref Ӟ, value2, markupLinkClickEventHandler2);
			}
			while ((object)markupLinkClickEventHandler != markupLinkClickEventHandler2);
		}
		remove
		{
			MarkupLinkClickEventHandler markupLinkClickEventHandler = Ӟ;
			MarkupLinkClickEventHandler markupLinkClickEventHandler2;
			do
			{
				markupLinkClickEventHandler2 = markupLinkClickEventHandler;
				MarkupLinkClickEventHandler value2 = (MarkupLinkClickEventHandler)Delegate.Remove(markupLinkClickEventHandler2, value);
				markupLinkClickEventHandler = Interlocked.CompareExchange(ref Ӟ, value2, markupLinkClickEventHandler2);
			}
			while ((object)markupLinkClickEventHandler != markupLinkClickEventHandler2);
		}
	}

	public event EventHandler TitleTextMarkupLinkClick
	{
		add
		{
			EventHandler eventHandler = _0D83;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0D83, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _0D83;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _0D83, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	protected virtual void OnCheckedChanged(EventArgs e)
	{
		_0E65?.Invoke(this, e);
	}

	protected virtual void OnOptionGroupChanging(OptionGroupChangingEventArgs e)
	{
		_09D5?.Invoke(this, e);
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
		_ᨫ = new CustomCollection<MetroTileFrame>();
		_ᨫ.CollectionChanged += ᨴ;
		MetroTileFrame metroTileFrame = new MetroTileFrame();
		metroTileFrame.MarkupLinkClick += ᨳ;
		_ᨫ.Add(metroTileFrame);
	}

	protected virtual void OnMarkupLinkClick(MarkupLinkClickEventArgs e)
	{
		if (Ӟ != null)
		{
			Ӟ(this, e);
		}
	}

	private void ᨳ(object _٠, MarkupLinkClickEventArgs ؿ)
	{
		OnMarkupLinkClick(ؿ);
	}

	public override BaseItem Copy()
	{
		MetroTileItem metroTileItem = new MetroTileItem(m_Name);
		CopyToItem(metroTileItem);
		return metroTileItem;
	}

	internal void _0EBA(MetroTileItem _0EBB)
	{
		CopyToItem(_0EBB);
	}

	protected override void CopyToItem(BaseItem copy)
	{
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
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
		metroTileItem.SymbolSet = SymbolSet;
		metroTileItem.SymbolSize = SymbolSize;
		metroTileItem.SymbolColor = SymbolColor;
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
		metroTileItem.NotificationMarkColor = NotificationMarkColor;
		metroTileItem.NotificationMarkPosition = NotificationMarkPosition;
		metroTileItem.NotificationMarkSize = NotificationMarkSize;
		metroTileItem.NotificationMarkText = NotificationMarkText;
		for (int i = 1; i < Frames.Count; i++)
		{
			metroTileItem.Frames.Add((MetroTileFrame)Frames[i].Clone());
		}
	}

	protected override void Dispose(bool disposing)
	{
		ᨺ();
		ᨻ();
		_ᨫ.Dispose();
		base.Dispose(disposing);
	}

	protected virtual void OnNotificationMarkTextChanged(string oldValue, string newValue)
	{
		if (string.IsNullOrEmpty(newValue))
		{
			_ມ = true;
		}
		Refresh();
	}

	protected virtual void OnNotificationMarkPositionChanged(eNotificationMarkPosition oldValue, eNotificationMarkPosition newValue)
	{
		if (!string.IsNullOrEmpty(_ຝ))
		{
			Refresh();
		}
	}

	protected virtual void OnNotificationMarkSizeChanged(int oldValue, int newValue)
	{
		Refresh();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeNotificationMarkColor()
	{
		return !_ຠ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetNotificationMarkColor()
	{
		NotificationMarkColor = Color.Empty;
	}

	protected virtual void OnNotificationMarkOffsetChanged(Point oldValue, Point newValue)
	{
		Invalidate();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeNotificationMarkOffset()
	{
		return _ຣ != ຢ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetNotificationMarkOffset()
	{
		NotificationMarkOffset = ຢ;
	}

	protected override Rectangle GetInvalidateBounds()
	{
		if (_ມ || !string.IsNullOrEmpty(_ຝ))
		{
			_ມ = false;
			Rectangle invalidateBounds = base.GetInvalidateBounds();
			invalidateBounds.Inflate(6, 6);
			return invalidateBounds;
		}
		return base.GetInvalidateBounds();
	}

	private void ᨴ(object _٠, NotifyCollectionChangedEventArgs ؿ)
	{
		if (ؿ.OldItems != null && ؿ.OldItems.Count > 0)
		{
			foreach (MetroTileFrame oldItem in ؿ.OldItems)
			{
				oldItem.PropertyChanged -= ᨵ;
			}
		}
		if (ؿ.NewItems != null && ؿ.NewItems.Count > 0)
		{
			foreach (MetroTileFrame newItem in ؿ.NewItems)
			{
				newItem.PropertyChanged += ᨵ;
			}
		}
		if (_ᨬ >= _ᨫ.Count)
		{
			_ᨬ = 0;
			Refresh();
		}
	}

	private void ᨵ(object _٠, PropertyChangedEventArgs ؿ)
	{
		if (ؿ.PropertyName == "TileStyle" || ؿ.PropertyName == "TitleText" || ؿ.PropertyName == "TitleTextAlignment" || ؿ.PropertyName == "Image" || ؿ.PropertyName == "ImageIndent" || ؿ.PropertyName == "Text" || ؿ.PropertyName == "TextMarkupEnabled" || ؿ.PropertyName == "ImageTextAlignment" || ؿ.PropertyName == "Symbol" || ؿ.PropertyName == "SymbolSize")
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
		if (p._0EE1 && p._0EE2 != null && !_ᨦ.IsEmpty && m_Rect != _ᨦ)
		{
			Rectangle rect = m_Rect;
			m_Rect = _ᨦ;
			_ᨦ = Rectangle.Empty;
			ခ item = new ခ(this, "Bounds", m_Rect, rect);
			p._0EE2.Add(item);
			return;
		}
		MetroRender.Paint(this, p);
		if (Focused && DesignMode)
		{
			Rectangle rect2 = m_Rect;
			rect2.Inflate(-1, -1);
			_1A6B._1A6C(p.Graphics, rect2, p.Colors.ItemDesignTimeBorder);
		}
		_ᨦ = m_Rect;
		if (!string.IsNullOrEmpty(_ຝ))
		{
			_1A6E._139A(p.Graphics, Bounds, _ພ, _ຝ, (_ຟ > 0) ? new Size(_ຟ, _ຟ) : ᨧ, _ຣ, _ຠ, p.ContainerClipRectangle);
		}
		if (!(p.ContainerControl is MetroTilePanel))
		{
			DrawInsertMarker(p.Graphics);
		}
	}

	public override void RecalcSize()
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Invalid comparison between Unknown and I4
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Invalid comparison between Unknown and I4
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Invalid comparison between Unknown and I4
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Invalid comparison between Unknown and I4
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Invalid comparison between Unknown and I4
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Invalid comparison between Unknown and I4
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Invalid comparison between Unknown and I4
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Invalid comparison between Unknown and I4
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Invalid comparison between Unknown and I4
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Invalid comparison between Unknown and I4
		//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Invalid comparison between Unknown and I4
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Invalid comparison between Unknown and I4
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Invalid comparison between Unknown and I4
		//IL_042b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Invalid comparison between Unknown and I4
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Invalid comparison between Unknown and I4
		//IL_047a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0484: Invalid comparison between Unknown and I4
		//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e0: Invalid comparison between Unknown and I4
		//IL_0547: Unknown result type (might be due to invalid IL or missing references)
		//IL_054e: Invalid comparison between Unknown and I4
		//IL_059b: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a2: Invalid comparison between Unknown and I4
		//IL_05ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0605: Invalid comparison between Unknown and I4
		//IL_0652: Unknown result type (might be due to invalid IL or missing references)
		//IL_0658: Invalid comparison between Unknown and I4
		//IL_0688: Unknown result type (might be due to invalid IL or missing references)
		//IL_068e: Invalid comparison between Unknown and I4
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
		Size size = Dpi.Size(_ח);
		for (int i = 0; i < _ᨫ.Count; i++)
		{
			MetroTileFrame metroTileFrame = _ᨫ[i];
			bool ല = false;
			ElementStyle elementStyle = ElementStyleDisplay.റ(metroTileFrame.EffectiveStyle, out ല);
			int num = size.Width - elementStyle.PaddingHorizontal;
			int num2 = 0;
			if (m_SubItems != null && i < m_SubItems.Count)
			{
				BaseItem baseItem = m_SubItems[i];
				if (baseItem.Visible)
				{
					baseItem.LeftInternal = m_Rect.Left + elementStyle.PaddingLeft;
					baseItem.TopInternal = m_Rect.Top + elementStyle.PaddingTop;
					baseItem.WidthInternal = size.Width - elementStyle.PaddingHorizontal;
					baseItem.HeightInternal = size.Height - elementStyle.PaddingVertical;
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
			if (!string.IsNullOrEmpty(metroTileFrame.SymbolRealized))
			{
				Font font = Symbols.GetFont(metroTileFrame.SymbolSize, metroTileFrame.SymbolSet);
				Size size2 = TextDrawing.MeasureString(val2, "\uf00a", font);
				int num3 = (int)Math.Ceiling((float)font.FontFamily.GetCellDescent(font.Style) * font.Size / (float)font.FontFamily.GetEmHeight(font.Style));
				size2.Height -= num3;
				if ((int)imageTextAlignment == 1 || (int)imageTextAlignment == 4 || (int)imageTextAlignment == 256 || (int)imageTextAlignment == 1024 || (int)imageTextAlignment == 16 || (int)imageTextAlignment == 64)
				{
					num = Math.Max(12, num - size2.Width);
				}
			}
			else if (metroTileFrame.Image != null && ((int)imageTextAlignment == 1 || (int)imageTextAlignment == 4 || (int)imageTextAlignment == 256 || (int)imageTextAlignment == 1024 || (int)imageTextAlignment == 16 || (int)imageTextAlignment == 64))
			{
				num = Math.Max(12, num - Dpi.ImageWidth(metroTileFrame.Image.Width));
			}
			if (metroTileFrame.Text != "")
			{
				MetroTileFrame.MeasureText(metroTileFrame, val2, num, val.Font, eTextFormat.Default, rightToLeft);
			}
			Size size3 = Size.Empty;
			metroTileFrame.TitleTextBounds = Rectangle.Empty;
			if (metroTileFrame.TitleTextMarkupBody != null)
			{
				Font currentFont = ᨶ(metroTileFrame, elementStyle, val);
				Size availableSize = new Size(num, 1);
				MarkupDrawContext markupDrawContext = new MarkupDrawContext(val2, currentFont, Color.Empty, rightToLeft: false);
				metroTileFrame.TitleTextMarkupBody.Measure(availableSize, markupDrawContext);
				availableSize.Height = metroTileFrame.TitleTextMarkupBody.Bounds.Size.Height;
				markupDrawContext.RightToLeft = rightToLeft;
				metroTileFrame.TitleTextMarkupBody._0CC6(new Rectangle(0, 0, availableSize.Width, availableSize.Height), markupDrawContext);
				size3 = metroTileFrame.TitleTextMarkupBody.Bounds.Size;
			}
			else if (!string.IsNullOrEmpty(metroTileFrame.TitleText))
			{
				Font font2 = ᨶ(metroTileFrame, elementStyle, val);
				Size size4 = new Size(size.Width - elementStyle.PaddingHorizontal, size.Height);
				size3 = TextDrawing.MeasureString(val2, metroTileFrame.TitleText, font2, size4.Width, eTextFormat.SingleLine);
			}
			if (!size3.IsEmpty)
			{
				size3.Width = Math.Min(size3.Width + 2, size.Width);
				if ((int)metroTileFrame.TitleTextAlignment == 512)
				{
					metroTileFrame.TitleTextBounds = new Rectangle(elementStyle.PaddingLeft + (size.Width - elementStyle.PaddingHorizontal - size3.Width) / 2, size.Height - size3.Height - elementStyle.PaddingBottom - 3, size.Width, size3.Height + 1);
				}
				else if ((int)metroTileFrame.TitleTextAlignment == 256)
				{
					metroTileFrame.TitleTextBounds = new Rectangle(elementStyle.PaddingLeft, size.Height - size3.Height - elementStyle.PaddingBottom - 3, size3.Width, size3.Height + 1);
				}
				else if ((int)metroTileFrame.TitleTextAlignment == 1024)
				{
					metroTileFrame.TitleTextBounds = new Rectangle(size.Width - elementStyle.PaddingRight - size3.Width, size.Height - size3.Height - elementStyle.PaddingBottom - 3, size3.Width, size3.Height + 1);
				}
				else if ((int)metroTileFrame.TitleTextAlignment == 32)
				{
					metroTileFrame.TitleTextBounds = new Rectangle(elementStyle.PaddingLeft + (size.Width - elementStyle.PaddingHorizontal - size3.Width) / 2, elementStyle.PaddingTop + (size.Height - elementStyle.PaddingVertical - size3.Height) / 2, size.Width, size3.Height + 1);
				}
				else if ((int)metroTileFrame.TitleTextAlignment == 16)
				{
					metroTileFrame.TitleTextBounds = new Rectangle(elementStyle.PaddingLeft, elementStyle.PaddingTop + (size.Height - elementStyle.PaddingVertical - size3.Height) / 2, size3.Width, size3.Height + 1);
				}
				else if ((int)metroTileFrame.TitleTextAlignment == 64)
				{
					metroTileFrame.TitleTextBounds = new Rectangle(size.Width - elementStyle.PaddingRight - size3.Width, elementStyle.PaddingTop + (size.Height - elementStyle.PaddingVertical - size3.Height) / 2, size3.Width, size3.Height + 1);
				}
				else if ((int)metroTileFrame.TitleTextAlignment == 2)
				{
					metroTileFrame.TitleTextBounds = new Rectangle(elementStyle.PaddingLeft + (size.Width - elementStyle.PaddingHorizontal - size3.Width) / 2, elementStyle.PaddingTop, size3.Width, size3.Height + 1);
				}
				else if ((int)metroTileFrame.TitleTextAlignment == 1)
				{
					metroTileFrame.TitleTextBounds = new Rectangle(elementStyle.PaddingLeft, elementStyle.PaddingTop, size3.Width, size3.Height + 1);
				}
				else if ((int)metroTileFrame.TitleTextAlignment == 4)
				{
					metroTileFrame.TitleTextBounds = new Rectangle(size.Width - elementStyle.PaddingRight - size3.Width, elementStyle.PaddingTop, size3.Width, size3.Height + 1);
				}
			}
			if (ല)
			{
				elementStyle.Dispose();
			}
		}
		m_Rect = new Rectangle(m_Rect.X, m_Rect.Y, size.Width, size.Height);
		base.RecalcSize();
	}

	internal Font ᨶ(MetroTileFrame ᨷ, ElementStyle _0657, Control ॼ)
	{
		Font val = ᨷ.TitleTextFont;
		if (val == null && _0657 != null)
		{
			val = _0657.Font;
		}
		if (val == null && ॼ != null)
		{
			val = ॼ.Font;
		}
		if (val == null)
		{
			val = SystemFonts.DefaultFont;
		}
		return val;
	}

	private void ᨸ(object _٠, EventArgs ؿ)
	{
		NeedRecalcSize = true;
		OnAppearanceChanged();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTileSize()
	{
		return _ח != ᨥ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTileSize()
	{
		TileSize = ᨥ;
	}

	protected virtual void OnTileSizeChanged(Size oldValue, Size newValue)
	{
		NeedRecalcSize = true;
		OnAppearanceChanged();
	}

	protected virtual void OnSymbolSetChanged(eSymbolSet oldValue, eSymbolSet newValue)
	{
		NeedRecalcSize = true;
		OnAppearanceChanged();
		Refresh();
	}

	public override void InternalClick(MouseButtons mb, Point mpos)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		base.InternalClick(mb, mpos);
		MetroTileFrame metroTileFrame = ᨹ();
		if (metroTileFrame != null && metroTileFrame.TextMarkupBody != null)
		{
			_0CB4 textMarkupBody = metroTileFrame.TextMarkupBody;
			object containerControl = ContainerControl;
			textMarkupBody.દ((Control)((containerControl is Control) ? containerControl : null));
		}
	}

	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Invalid comparison between Unknown and I4
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Invalid comparison between Unknown and I4
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Invalid comparison between Unknown and I4
		bool enabled = GetEnabled();
		if ((int)objArg.Button == 1048576 && enabled)
		{
			IsLeftMouseButtonDown = true;
		}
		if (enabled)
		{
			eMetroTileCheckBehavior eMetroTileCheckBehavior2 = ᨣ;
			if (eMetroTileCheckBehavior2 == eMetroTileCheckBehavior.LeftMouseButtonClick && (int)objArg.Button == 1048576)
			{
				Checked = !Checked;
			}
			else if (eMetroTileCheckBehavior2 == eMetroTileCheckBehavior.MiddleMouseButtonClick && (int)objArg.Button == 4194304)
			{
				Checked = !Checked;
			}
			else if (eMetroTileCheckBehavior2 == eMetroTileCheckBehavior.RightMouseButtonClick && (int)objArg.Button == 2097152)
			{
				Checked = !Checked;
			}
		}
		MetroTileFrame metroTileFrame = ᨹ();
		if (metroTileFrame != null && metroTileFrame.TextMarkupBody != null)
		{
			_0CB4 textMarkupBody = metroTileFrame.TextMarkupBody;
			object containerControl = ContainerControl;
			textMarkupBody.ग़((Control)((containerControl is Control) ? containerControl : null), objArg);
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
		MetroTileFrame metroTileFrame = ᨹ();
		if (metroTileFrame != null && metroTileFrame.TextMarkupBody != null)
		{
			_0CB4 textMarkupBody = metroTileFrame.TextMarkupBody;
			object containerControl = ContainerControl;
			textMarkupBody.ફ((Control)((containerControl is Control) ? containerControl : null), objArg);
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
			MetroTileFrame metroTileFrame = ᨹ();
			if (metroTileFrame != null && metroTileFrame.TextMarkupBody != null)
			{
				_0CB4 textMarkupBody = metroTileFrame.TextMarkupBody;
				object containerControl = ContainerControl;
				textMarkupBody.મ((Control)((containerControl is Control) ? containerControl : null), objArg);
			}
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
		MetroTileFrame metroTileFrame = ᨹ();
		if (metroTileFrame != null && metroTileFrame.TextMarkupBody != null)
		{
			_0CB4 textMarkupBody = metroTileFrame.TextMarkupBody;
			object containerControl = ContainerControl;
			textMarkupBody.ભ((Control)((containerControl is Control) ? containerControl : null));
		}
		base.InternalMouseLeave();
	}

	private MetroTileFrame ᨹ()
	{
		if (CurrentFrame < 0 || CurrentFrame >= Frames.Count)
		{
			return null;
		}
		return Frames[CurrentFrame];
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
		_0D83?.Invoke(this, e);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTitleTextColor()
	{
		return _ᨫ[0].ShouldSerializeTitleTextColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTitleTextColor()
	{
		_ᨫ[0].ResetTitleTextColor();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeImageIndent()
	{
		return _ᨫ[0].ShouldSerializeImageIndent();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetImageIndent()
	{
		_ᨫ[0].ResetImageIndent();
	}

	protected virtual void OnCheckedChanged(bool oldValue, bool newValue)
	{
		if (newValue && _ᨪ.Length > 0 && Parent != null)
		{
			MetroTileItem metroTileItem = null;
			foreach (BaseItem subItem in Parent.SubItems)
			{
				if (subItem != this)
				{
					metroTileItem = subItem as MetroTileItem;
					if (metroTileItem != null && metroTileItem.OptionGroup == _ᨪ && metroTileItem.Checked)
					{
						break;
					}
				}
			}
			OptionGroupChangingEventArgs optionGroupChangingEventArgs = new OptionGroupChangingEventArgs(metroTileItem, this);
			OnOptionGroupChanging(optionGroupChangingEventArgs);
			if (optionGroupChangingEventArgs.Cancel)
			{
				_ᚆ = oldValue;
				return;
			}
		}
		if (ShouldSyncProperties)
		{
			BarFunctions.ढ़(this, "Checked");
		}
		if (_ᨪ != "" && newValue && Parent != null)
		{
			foreach (BaseItem subItem2 in Parent.SubItems)
			{
				if (subItem2 != this && subItem2 is MetroTileItem metroTileItem2 && metroTileItem2.OptionGroup == _ᨪ && metroTileItem2.Checked)
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

	private void ᨺ()
	{
		_1A18 obj = __19EC;
		__19EC = null;
		if (obj != null)
		{
			obj.Stop();
			obj.Dispose();
		}
	}

	private bool ఛ()
	{
		if (DesignMode)
		{
			return true;
		}
		if (Site != null && Site.DesignMode)
		{
			return true;
		}
		return false;
	}

	protected virtual void OnCurrentFrameChanged(int oldValue, int newValue)
	{
		_ᨭ = oldValue;
		if (_ᢻ && _ᨯ > 0 && !ఛ())
		{
			ᨺ();
			int num = Dpi.Height(_ח.Height);
			(__19EC = new _1A6A(new ခ(this, "CurrentFrameOffset", num, 0), AnimationEasing.EaseOutExpo, _ᨯ)).Start();
		}
		else
		{
			Refresh();
		}
	}

	private void ᨻ()
	{
		Timer ᨱ = _ᨱ;
		_ᨱ = null;
		if (ᨱ != null)
		{
			ᨱ.Stop();
			((Component)(object)ᨱ).Dispose();
		}
	}

	private void ᨼ()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		if (_ᨱ == null)
		{
			_ᨱ = new Timer();
			_ᨱ.Interval = _ᨰ;
			if (_ᨫ[_ᨬ].FrameDisplayDuration > 0)
			{
				_ᨱ.Interval = _ᨫ[_ᨬ].FrameDisplayDuration;
			}
			_ᨱ.Tick += ᨽ;
			_ᨱ.Start();
		}
	}

	private void ᨽ(object _٠, EventArgs ؿ)
	{
		if (Frames.Count > 1)
		{
			int num = CurrentFrame + 1;
			if (num >= Frames.Count)
			{
				num = 0;
			}
			CurrentFrame = num;
			if (_ᨫ[num].FrameDisplayDuration > 0)
			{
				_ᨱ.Interval = _ᨫ[num].FrameDisplayDuration;
			}
			else
			{
				_ᨱ.Interval = _ᨰ;
			}
		}
	}

	protected virtual void OnAutoRotateFramesIntervalChanged(int oldValue, int newValue)
	{
		if (newValue == 0 || ఛ())
		{
			ᨻ();
		}
		else if (Visible)
		{
			ᨼ();
		}
	}

	protected internal override void OnVisibleChanged(bool newValue)
	{
		if (newValue && !ఛ())
		{
			if (_ᨰ > 0 && _ᨫ.Count > 1 && _ᨱ == null)
			{
				ᨼ();
			}
		}
		else if (_ᨱ != null)
		{
			ᨻ();
		}
		base.OnVisibleChanged(newValue);
	}

	protected override void OnDragStartPointChanged(Point oldValue, Point newValue)
	{
		if (newValue.IsEmpty)
		{
			if (_ᨱ != null)
			{
				_ᨱ.Enabled = true;
			}
		}
		else if (_ᨱ != null)
		{
			_ᨱ.Enabled = false;
		}
		base.OnDragStartPointChanged(oldValue, newValue);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeDisabledBackColor()
	{
		return _ᖣ != ᨲ;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetDisabledBackColor()
	{
		DisabledBackColor = ᨲ;
	}
}
