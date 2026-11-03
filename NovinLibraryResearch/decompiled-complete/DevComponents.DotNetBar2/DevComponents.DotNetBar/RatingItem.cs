using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.Rendering;
using DevComponents.DotNetBar.Ribbon;
using DevComponents.DotNetBar.TextMarkup;
using DevComponents.Editors;

namespace DevComponents.DotNetBar;

[Designer("DevComponents.DotNetBar.Design.RatingItemDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ToolboxItem(false)]
[DesignTimeVisible(false)]
[DefaultEvent("RatingChanged")]
public class RatingItem : PopupItem
{
	private struct ⱡ
	{
		public Rectangle ঋ;

		public bool ᆼ;
	}

	private struct ⱨ
	{
		public Image ᇑ;

		public Rectangle Ⱪ;

		public ⱨ(Image image, Rectangle sourceBounds)
		{
			ᇑ = image;
			Ⱪ = sourceBounds;
		}
	}

	private EventHandler ⰺ;

	private EventHandler ⰻ;

	private RatingChangeEventHandler ⰼ;

	private EventHandler ⰽ;

	private EventHandler ⰾ;

	private MarkupLinkClickEventHandler Ӟ;

	private ParseIntegerValueEventHandler ⰿ;

	private ParseDoubleValueEventHandler ⱀ;

	private double _ⱇ;

	private int _ⱈ;

	private bool __29FF = true;

	private bool _ⱉ;

	private ⱡ[] _ⱊ = new ⱡ[5];

	private Size _ⱋ = new Size(13, 13);

	private Image _ⱌ;

	private int _ⱍ = -1;

	private int _ⱎ = 8;

	private Size _ᢧ = Size.Empty;

	private bool _ⱏ;

	private Size _ⱐ = Size.Empty;

	private ElementStyle _ᠩ = new ElementStyle();

	private RatingImages _ⱑ;

	private bool _ᣅ = true;

	private Color __0CD1 = Color.Empty;

	private int _ⱒ;

	private eOrientation _ⱓ;

	private int _ⱔ = 5;

	private bool __0EB9 = true;

	[DefaultValue(0.0)]
	[Description("Indicates average rating shown by control.")]
	[Category("Data")]
	public double AverageRating
	{
		get
		{
			return _ⱇ;
		}
		set
		{
			if (value < 0.0)
			{
				value = 0.0;
			}
			if (value > (double)_ⱔ)
			{
				value = _ⱔ;
			}
			if (value != _ⱇ)
			{
				_ⱇ = value;
				OnAverageRatingChanged(EventArgs.Empty);
				Refresh();
			}
		}
	}

	[Browsable(false)]
	[Bindable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[RefreshProperties(RefreshProperties.All)]
	[TypeConverter(typeof(StringConverter))]
	public object AverageRatingValue
	{
		get
		{
			return AverageRating;
		}
		set
		{
			if (ⱕ(value))
			{
				return;
			}
			if (IsNull(value))
			{
				AverageRating = 0.0;
				return;
			}
			if (value is double || value is int || value is float)
			{
				AverageRating = (double)value;
				return;
			}
			if (value is long)
			{
				string s = value.ToString();
				AverageRating = double.Parse(s);
				return;
			}
			if (value is string)
			{
				double result = 0.0;
				if (double.TryParse(value.ToString(), out result))
				{
					AverageRating = result;
					return;
				}
				throw new ArgumentException("AverageRatingValue property expects either null/nothing value or double type.");
			}
			throw new ArgumentException("AverageRatingValue property expects either null/nothing value or double type.");
		}
	}

	[Description("Indicates rating value represented by the control.")]
	[DefaultValue(0)]
	[Category("Data")]
	public int Rating
	{
		get
		{
			return _ⱈ;
		}
		set
		{
			if (value < 0)
			{
				value = 0;
			}
			if (_ⱉ && value > _ⱔ)
			{
				value = _ⱔ;
			}
			if (_ⱈ != value)
			{
				SetRating(value, eEventSource.Code);
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[RefreshProperties(RefreshProperties.All)]
	[Bindable(true)]
	public object RatingValue
	{
		get
		{
			return _ⱈ;
		}
		set
		{
			if (ⱖ(value))
			{
				return;
			}
			if (IsNull(value))
			{
				Rating = 0;
				return;
			}
			if (value is int)
			{
				Rating = (int)value;
				return;
			}
			if (value is string)
			{
				int result = 0;
				if (int.TryParse(value.ToString(), out result))
				{
					Rating = result;
					return;
				}
				throw new ArgumentException("RatingValue property expects either null/nothing value or int type.");
			}
			throw new ArgumentException("RatingValue property expects either null/nothing value or int type.");
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether rating can be edited.")]
	[Category("Behavior")]
	public bool IsEditable
	{
		get
		{
			return __29FF;
		}
		set
		{
			if (__29FF != value)
			{
				__29FF = value;
				ⱝ(-1);
			}
		}
	}

	internal Size ⱆ => _ⱐ;

	public override Rectangle Bounds
	{
		get
		{
			return base.Bounds;
		}
		set
		{
			if (!base.Bounds.Equals((object?)value))
			{
				base.Bounds = value;
				_2C5F();
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(true)]
	[Category("Style")]
	[Description("Gets or sets control background style.")]
	[DevCoBrowsable(true)]
	public ElementStyle BackgroundStyle => _ᠩ;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Images")]
	[Browsable(true)]
	[Description("Gets the reference to custom rating images.")]
	public RatingImages CustomImages => _ⱑ;

	[Category("Appearance")]
	[Description("Indicates whether text assigned to the check box is visible.")]
	[DefaultValue(true)]
	[Browsable(true)]
	public bool TextVisible
	{
		get
		{
			return _ᣅ;
		}
		set
		{
			_ᣅ = value;
			NeedRecalcSize = true;
			OnAppearanceChanged();
		}
	}

	[Description("Indicates text color.")]
	[Category("Appearance")]
	[Browsable(true)]
	public Color TextColor
	{
		get
		{
			return __0CD1;
		}
		set
		{
			__0CD1 = value;
			OnAppearanceChanged();
		}
	}

	[DefaultValue(0)]
	[Category("Appearance")]
	[Description("Gets or sets the spacing between optional text and the rating.")]
	public int TextSpacing
	{
		get
		{
			return _ⱒ;
		}
		set
		{
			if (_ⱒ != value)
			{
				_ⱒ = value;
				NeedRecalcSize = true;
				Refresh();
			}
		}
	}

	[Description("Gets or sets the orientation of rating control.")]
	[DefaultValue(eOrientation.Horizontal)]
	[Category("Appearance")]
	public eOrientation RatingOrientation
	{
		get
		{
			return _ⱓ;
		}
		set
		{
			if (_ⱓ != value)
			{
				_ⱓ = value;
				NeedRecalcSize = true;
				Refresh();
			}
		}
	}

	[Description("Indicates number of stars used for the rating.")]
	[Category("Appearance")]
	[DefaultValue(5)]
	public int NumberOfStars
	{
		get
		{
			return _ⱔ;
		}
		set
		{
			if (_ⱔ < 2)
			{
				_ⱔ = 2;
			}
			if (_ⱔ != value)
			{
				_ⱔ = value;
				if (_ⱈ > _ⱔ)
				{
					_ⱈ = _ⱔ;
				}
				_ⱊ = new ⱡ[_ⱔ];
				NeedRecalcSize = true;
				Refresh();
			}
		}
	}

	protected override bool IsMarkupSupported => __0EB9;

	[Category("Appearance")]
	[Description("Indicates whether text-markup support is enabled for items Text property.")]
	[DefaultValue(true)]
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
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override SubItemsCollection SubItems => base.SubItems;

	[Browsable(false)]
	public override bool ClickAutoRepeat
	{
		get
		{
			return base.ClickAutoRepeat;
		}
		set
		{
			base.ClickAutoRepeat = value;
		}
	}

	[Browsable(false)]
	public override int ClickRepeatInterval
	{
		get
		{
			return base.ClickRepeatInterval;
		}
		set
		{
			base.ClickRepeatInterval = value;
		}
	}

	[Browsable(false)]
	public override ePersonalizedMenus PersonalizedMenus
	{
		get
		{
			return base.PersonalizedMenus;
		}
		set
		{
			base.PersonalizedMenus = value;
		}
	}

	[Browsable(false)]
	public override ePopupAnimation PopupAnimation
	{
		get
		{
			return base.PopupAnimation;
		}
		set
		{
			base.PopupAnimation = value;
		}
	}

	[Browsable(false)]
	public override Font PopupFont
	{
		get
		{
			return base.PopupFont;
		}
		set
		{
			base.PopupFont = value;
		}
	}

	[Browsable(false)]
	public override ePopupSide PopupSide
	{
		get
		{
			return base.PopupSide;
		}
		set
		{
			base.PopupSide = value;
		}
	}

	[Browsable(false)]
	public override ePopupType PopupType
	{
		get
		{
			return base.PopupType;
		}
		set
		{
			base.PopupType = value;
		}
	}

	[Browsable(false)]
	public override int PopupWidth
	{
		get
		{
			return base.PopupWidth;
		}
		set
		{
			base.PopupWidth = value;
		}
	}

	[Browsable(false)]
	public override ShortcutsCollection Shortcuts
	{
		get
		{
			return base.Shortcuts;
		}
		set
		{
			base.Shortcuts = value;
		}
	}

	[Browsable(false)]
	public override bool ShowSubItems
	{
		get
		{
			return base.ShowSubItems;
		}
		set
		{
			base.ShowSubItems = value;
		}
	}

	[Browsable(false)]
	public override bool Stretch
	{
		get
		{
			return base.Stretch;
		}
		set
		{
			base.Stretch = value;
		}
	}

	[Browsable(false)]
	public override bool ThemeAware
	{
		get
		{
			return base.ThemeAware;
		}
		set
		{
			base.ThemeAware = value;
		}
	}

	[DefaultValue(false)]
	public override bool AutoCollapseOnClick
	{
		get
		{
			return base.AutoCollapseOnClick;
		}
		set
		{
			base.AutoCollapseOnClick = value;
		}
	}

	[Description("Occurs when Rating property has changed.")]
	public event EventHandler RatingChanged
	{
		add
		{
			EventHandler eventHandler = ⰺ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ⰺ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ⰺ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ⰺ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when Rating property has changed.")]
	public event EventHandler RatingValueChanged
	{
		add
		{
			EventHandler eventHandler = ⰻ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ⰻ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ⰻ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ⰻ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when Rating property has changed.")]
	public event RatingChangeEventHandler RatingChanging
	{
		add
		{
			RatingChangeEventHandler ratingChangeEventHandler = ⰼ;
			RatingChangeEventHandler ratingChangeEventHandler2;
			do
			{
				ratingChangeEventHandler2 = ratingChangeEventHandler;
				RatingChangeEventHandler value2 = (RatingChangeEventHandler)Delegate.Combine(ratingChangeEventHandler2, value);
				ratingChangeEventHandler = Interlocked.CompareExchange(ref ⰼ, value2, ratingChangeEventHandler2);
			}
			while ((object)ratingChangeEventHandler != ratingChangeEventHandler2);
		}
		remove
		{
			RatingChangeEventHandler ratingChangeEventHandler = ⰼ;
			RatingChangeEventHandler ratingChangeEventHandler2;
			do
			{
				ratingChangeEventHandler2 = ratingChangeEventHandler;
				RatingChangeEventHandler value2 = (RatingChangeEventHandler)Delegate.Remove(ratingChangeEventHandler2, value);
				ratingChangeEventHandler = Interlocked.CompareExchange(ref ⰼ, value2, ratingChangeEventHandler2);
			}
			while ((object)ratingChangeEventHandler != ratingChangeEventHandler2);
		}
	}

	[Description("Occurs when AverageRating property has changed.")]
	public event EventHandler AverageRatingChanged
	{
		add
		{
			EventHandler eventHandler = ⰽ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ⰽ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ⰽ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ⰽ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when AverageRatingValue property has changed.")]
	public event EventHandler AverageRatingValueChanged
	{
		add
		{
			EventHandler eventHandler = ⰾ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ⰾ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ⰾ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ⰾ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

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

	public event ParseIntegerValueEventHandler ParseRatingValue
	{
		add
		{
			ParseIntegerValueEventHandler parseIntegerValueEventHandler = ⰿ;
			ParseIntegerValueEventHandler parseIntegerValueEventHandler2;
			do
			{
				parseIntegerValueEventHandler2 = parseIntegerValueEventHandler;
				ParseIntegerValueEventHandler value2 = (ParseIntegerValueEventHandler)Delegate.Combine(parseIntegerValueEventHandler2, value);
				parseIntegerValueEventHandler = Interlocked.CompareExchange(ref ⰿ, value2, parseIntegerValueEventHandler2);
			}
			while ((object)parseIntegerValueEventHandler != parseIntegerValueEventHandler2);
		}
		remove
		{
			ParseIntegerValueEventHandler parseIntegerValueEventHandler = ⰿ;
			ParseIntegerValueEventHandler parseIntegerValueEventHandler2;
			do
			{
				parseIntegerValueEventHandler2 = parseIntegerValueEventHandler;
				ParseIntegerValueEventHandler value2 = (ParseIntegerValueEventHandler)Delegate.Remove(parseIntegerValueEventHandler2, value);
				parseIntegerValueEventHandler = Interlocked.CompareExchange(ref ⰿ, value2, parseIntegerValueEventHandler2);
			}
			while ((object)parseIntegerValueEventHandler != parseIntegerValueEventHandler2);
		}
	}

	public event ParseDoubleValueEventHandler ParseAverageRatingValue
	{
		add
		{
			ParseDoubleValueEventHandler parseDoubleValueEventHandler = ⱀ;
			ParseDoubleValueEventHandler parseDoubleValueEventHandler2;
			do
			{
				parseDoubleValueEventHandler2 = parseDoubleValueEventHandler;
				ParseDoubleValueEventHandler value2 = (ParseDoubleValueEventHandler)Delegate.Combine(parseDoubleValueEventHandler2, value);
				parseDoubleValueEventHandler = Interlocked.CompareExchange(ref ⱀ, value2, parseDoubleValueEventHandler2);
			}
			while ((object)parseDoubleValueEventHandler != parseDoubleValueEventHandler2);
		}
		remove
		{
			ParseDoubleValueEventHandler parseDoubleValueEventHandler = ⱀ;
			ParseDoubleValueEventHandler parseDoubleValueEventHandler2;
			do
			{
				parseDoubleValueEventHandler2 = parseDoubleValueEventHandler;
				ParseDoubleValueEventHandler value2 = (ParseDoubleValueEventHandler)Delegate.Remove(parseDoubleValueEventHandler2, value);
				parseDoubleValueEventHandler = Interlocked.CompareExchange(ref ⱀ, value2, parseDoubleValueEventHandler2);
			}
			while ((object)parseDoubleValueEventHandler != parseDoubleValueEventHandler2);
		}
	}

	public RatingItem()
	{
		_ᠩ.StyleChanged += ᅻ;
		_ⱑ = new RatingImages(this);
		AutoCollapseOnClick = false;
	}

	protected override void Dispose(bool disposing)
	{
		if (BarUtilities.DisposeItemImages && !DesignMode)
		{
			BarUtilities.ᆟ(ref _ⱌ);
			_ⱑ.Dispose();
		}
		base.Dispose(disposing);
	}

	public override BaseItem Copy()
	{
		RatingItem ratingItem = new RatingItem();
		ratingItem.Name = Name;
		CopyToItem(ratingItem);
		return ratingItem;
	}

	protected override void CopyToItem(BaseItem copy)
	{
		RatingItem ratingItem = copy as RatingItem;
		ratingItem.AverageRating = AverageRating;
		ratingItem.Rating = Rating;
		ratingItem.RatingOrientation = RatingOrientation;
		ratingItem.IsEditable = IsEditable;
		ratingItem.TextSpacing = TextSpacing;
		ratingItem.TextColor = TextColor;
		ratingItem.TextVisible = TextVisible;
		base.CopyToItem((BaseItem)ratingItem);
	}

	protected virtual void OnAverageRatingChanged(EventArgs eventArgs)
	{
		if (ⰽ != null)
		{
			ⰽ(this, eventArgs);
		}
		if (ⰾ != null)
		{
			ⰾ(this, eventArgs);
		}
	}

	private bool ⱕ(object ԝ)
	{
		ParseDoubleValueEventArgs parseDoubleValueEventArgs = new ParseDoubleValueEventArgs(ԝ);
		OnParseAverageRatingValue(parseDoubleValueEventArgs);
		if (parseDoubleValueEventArgs.IsParsed)
		{
			AverageRating = parseDoubleValueEventArgs.ParsedValue;
		}
		return parseDoubleValueEventArgs.IsParsed;
	}

	protected virtual void OnParseAverageRatingValue(ParseDoubleValueEventArgs e)
	{
		if (ⱀ != null)
		{
			ⱀ(this, e);
		}
	}

	public void SetRating(int newRating, eEventSource source)
	{
		if (newRating != _ⱈ)
		{
			RatingChangeEventArgs ratingChangeEventArgs = new RatingChangeEventArgs(newRating, _ⱈ, source);
			OnRatingChanging(ratingChangeEventArgs);
			if (!ratingChangeEventArgs.Cancel)
			{
				newRating = ratingChangeEventArgs.NewRating;
				_ⱈ = newRating;
				OnRatingChanged(ratingChangeEventArgs);
				ExecuteCommand();
				Refresh();
			}
		}
	}

	protected virtual void OnRatingChanging(RatingChangeEventArgs e)
	{
		if (ⰼ != null)
		{
			ⰼ(this, e);
		}
	}

	protected virtual void OnRatingChanged(EventArgs eventArgs)
	{
		ⰺ?.Invoke(this, eventArgs);
		ⰻ?.Invoke(this, eventArgs);
	}

	protected virtual bool IsNull(object value)
	{
		if (value == null || value is DBNull)
		{
			return true;
		}
		return false;
	}

	private bool ⱖ(object ԝ)
	{
		ParseIntegerValueEventArgs parseIntegerValueEventArgs = new ParseIntegerValueEventArgs(ԝ);
		OnParseRatingValue(parseIntegerValueEventArgs);
		if (parseIntegerValueEventArgs.IsParsed)
		{
			Rating = parseIntegerValueEventArgs.ParsedValue;
		}
		return parseIntegerValueEventArgs.IsParsed;
	}

	protected virtual void OnParseRatingValue(ParseIntegerValueEventArgs e)
	{
		if (ⰿ != null)
		{
			ⰿ(this, e);
		}
	}

	public override void Paint(ItemPaintArgs p)
	{
		_ⱉ = true;
		PaintBackground(p);
		Rectangle ल = DisplayRectangle;
		if (_ⱏ)
		{
			ဨ(p, ref ल);
		}
		ल = ᦑ(ल);
		if (_ᣅ && !_ᢧ.IsEmpty)
		{
			_2422(p, ल);
		}
		Graphics graphics = p.Graphics;
		if (_ⱍ >= 0)
		{
			ⱨ ⱨ = ⱛ();
			ⱨ ⱨ2 = ⱚ();
			for (int i = 0; i < _ⱔ; i++)
			{
				if (_ⱊ[i].ᆼ)
				{
					graphics.DrawImage(ⱨ.ᇑ, _ⱊ[i].ঋ, ⱨ.Ⱪ, (GraphicsUnit)2);
				}
				else
				{
					graphics.DrawImage(ⱨ2.ᇑ, _ⱊ[i].ঋ, ⱨ2.Ⱪ, (GraphicsUnit)2);
				}
			}
		}
		else
		{
			int num = Math.Min(_ⱈ, _ⱔ);
			if (_ⱇ > 0.0 && num == 0)
			{
				ⱨ ⱨ3 = ⱗ();
				ⱨ ⱨ4 = ⱙ();
				for (int j = 0; j < _ⱔ; j++)
				{
					if ((double)(j + 1) <= _ⱇ)
					{
						graphics.DrawImage(ⱨ3.ᇑ, _ⱊ[j].ঋ, ⱨ3.Ⱪ, (GraphicsUnit)2);
					}
					else if ((double)(j + 1) == Math.Ceiling(_ⱇ))
					{
						int num2 = (int)((double)((RatingOrientation == eOrientation.Horizontal) ? _ⱊ[j].ঋ.Width : _ⱊ[j].ঋ.Height) * (_ⱇ - Math.Floor(_ⱇ)));
						graphics.DrawImage(ⱨ4.ᇑ, _ⱊ[j].ঋ, ⱨ4.Ⱪ, (GraphicsUnit)2);
						if (num2 > 0)
						{
							Region clip = graphics.Clip;
							if (RatingOrientation == eOrientation.Horizontal)
							{
								graphics.SetClip(new Rectangle(_ⱊ[j].ঋ.X, _ⱊ[j].ঋ.Y, num2, _ⱊ[j].ঋ.Height), (CombineMode)1);
							}
							else
							{
								graphics.SetClip(new Rectangle(_ⱊ[j].ঋ.X + (_ⱊ[j].ঋ.Height - num2), _ⱊ[j].ঋ.Y, _ⱊ[j].ঋ.Width, num2), (CombineMode)1);
							}
							graphics.DrawImage(ⱨ3.ᇑ, _ⱊ[j].ঋ, ⱨ3.Ⱪ, (GraphicsUnit)2);
							graphics.Clip = clip;
						}
					}
					else
					{
						graphics.DrawImage(ⱨ4.ᇑ, _ⱊ[j].ঋ, ⱨ4.Ⱪ, (GraphicsUnit)2);
					}
				}
			}
			else
			{
				ⱨ ⱨ5 = ⱘ();
				ⱨ ⱨ6 = ⱙ();
				for (int k = 0; k < _ⱔ; k++)
				{
					if (num > 0 && k + 1 <= num)
					{
						graphics.DrawImage(ⱨ5.ᇑ, _ⱊ[k].ঋ, ⱨ5.Ⱪ, (GraphicsUnit)2);
					}
					else
					{
						graphics.DrawImage(ⱨ6.ᇑ, _ⱊ[k].ঋ, ⱨ6.Ⱪ, (GraphicsUnit)2);
					}
				}
			}
		}
		if (Focused && DesignMode)
		{
			Rectangle displayRectangle = DisplayRectangle;
			displayRectangle.Inflate(-1, -1);
			_1A6B._1A6C(graphics, displayRectangle, p.Colors.ItemDesignTimeBorder);
		}
	}

	private void _2422(ItemPaintArgs _065C, Rectangle ശ)
	{
		Color empty = Color.Empty;
		empty = (__0CD1.IsEmpty ? _1933(_065C) : __0CD1);
		bool rightToLeft = _065C.RightToLeft;
		Rectangle rectangle = ശ;
		if ((_ᠩ.TextAlignment == eStyleTextAlignment.Far && !rightToLeft) || (rightToLeft && _ᠩ.TextAlignment == eStyleTextAlignment.Near))
		{
			rectangle.X = rectangle.Right - _ᢧ.Width;
			rectangle.Width = _ᢧ.Width;
		}
		else
		{
			rectangle.Width = _ᢧ.Width;
		}
		Graphics graphics = _065C.Graphics;
		Font font = _065C.Font;
		eTextFormat ݽ = _ᠩ.ݽ;
		if (!_ᣅ || !(Text != "") || rectangle.IsEmpty || empty.IsEmpty)
		{
			return;
		}
		if (base.TextMarkupBody != null)
		{
			MarkupDrawContext markupDrawContext = new MarkupDrawContext(graphics, font, empty, rightToLeft);
			markupDrawContext.HotKeyPrefixVisible = (ݽ & eTextFormat.HidePrefix) != eTextFormat.HidePrefix;
			if ((ݽ & eTextFormat.VerticalCenter) == eTextFormat.VerticalCenter)
			{
				rectangle.Y = TopInternal + (Bounds.Height - rectangle.Height) / 2;
			}
			else if ((ݽ & eTextFormat.Bottom) == eTextFormat.Bottom)
			{
				rectangle.Y += Bounds.Height - rectangle.Height + 1;
			}
			base.TextMarkupBody.Bounds = rectangle;
			base.TextMarkupBody.Render(markupDrawContext);
		}
		else if (_065C.GlassEnabled && Parent is ᱭ && !(_065C.ContainerControl is Ꮮ))
		{
			if (!_065C.ໟ)
			{
				_21F8._21FC(graphics, Text, font, rectangle, TextDrawing.GetTextFormat(ݽ));
			}
		}
		else
		{
			TextDrawing.DrawString(graphics, Text, font, empty, rectangle, ݽ);
		}
	}

	private Color _1933(ItemPaintArgs _0BD2)
	{
		if (BarFunctions.IsOffice2007Style(EffectiveStyle) && _0BD2.ໝ is Office2007Renderer)
		{
			if ((_0BD2.IsOnMenu || _0BD2.IsOnPopupBar) && ((Office2007Renderer)_0BD2.ໝ).ColorTable.ButtonItemColors.Count > 0)
			{
				if (!GetEnabled(_0BD2.ContainerControl))
				{
					return _0BD2.Colors.ItemDisabledText;
				}
				return ((Office2007Renderer)_0BD2.ໝ).ColorTable.ButtonItemColors[0].Default.Text;
			}
			if ((_0BD2.ContainerControl is RibbonStrip || _0BD2.ContainerControl is Bar) && ((Office2007Renderer)_0BD2.ໝ).ColorTable.RibbonTabItemColors.Count > 0)
			{
				return ((Office2007Renderer)_0BD2.ໝ).ColorTable.RibbonTabItemColors[0].Default.Text;
			}
			Office2007ButtonItemColorTable office2007ButtonItemColorTable = ((Office2007Renderer)_0BD2.ໝ).ColorTable.ButtonItemColors[Enum.GetName(typeof(eButtonColor), eButtonColor.Orange)];
			if (office2007ButtonItemColorTable != null && !office2007ButtonItemColorTable.Default.Text.IsEmpty)
			{
				if (!GetEnabled(_0BD2.ContainerControl))
				{
					return _0BD2.Colors.ItemDisabledText;
				}
				return office2007ButtonItemColorTable.Default.Text;
			}
			return __0CD1;
		}
		return _0BD2.Colors.ItemText;
	}

	private void ဨ(ItemPaintArgs _0BD2, ref Rectangle ल)
	{
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Expected O, but got Unknown
		bool flag = base.IsOnMenu && !(Parent is ItemContainer);
		Size size = ო();
		if (flag && (EffectiveStyle == eDotNetBarStyle.OfficeXP || EffectiveStyle == eDotNetBarStyle.Office2003 || EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(EffectiveStyle)))
		{
			Graphics graphics = _0BD2.Graphics;
			size.Width += 7;
			ल.Width -= size.Width + _ⱎ;
			if (!_0BD2.RightToLeft)
			{
				ल.X += size.Width + _ⱎ;
			}
			if (base.IsOnCustomizeMenu)
			{
				size.Width += size.Height + 8;
			}
			Rectangle rectangle = new Rectangle(m_Rect.Left, m_Rect.Top, size.Width, m_Rect.Height);
			if (_0BD2.RightToLeft)
			{
				rectangle.X = m_Rect.Right - rectangle.Width;
			}
			if (!_0BD2.Colors.MenuSide2.IsEmpty)
			{
				LinearGradientBrush val = BarFunctions.ॴ(rectangle, _0BD2.Colors.MenuSide, _0BD2.Colors.MenuSide2, _0BD2.Colors.MenuSideGradientAngle);
				graphics.FillRectangle((Brush)(object)val, rectangle);
				((Brush)val).Dispose();
			}
			else
			{
				graphics.FillRectangle((Brush)new SolidBrush(_0BD2.Colors.MenuSide), rectangle);
			}
			if (BarFunctions.IsOffice2007Style(EffectiveStyle) && GlobalManager.Renderer is Office2007Renderer)
			{
				Office2007MenuColorTable menu = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.Menu;
				if (menu != null && !menu.SideBorder.IsEmpty)
				{
					if (_0BD2.RightToLeft)
					{
						DisplayHelp.DrawGradientLine(graphics, rectangle.X, rectangle.Y, rectangle.X, rectangle.Bottom - 1, menu.SideBorder, 1);
					}
					else
					{
						DisplayHelp.DrawGradientLine(graphics, rectangle.Right - 2, rectangle.Y, rectangle.Right - 2, rectangle.Bottom - 1, menu.SideBorder, 1);
					}
				}
				if (menu != null && !menu.SideBorderLight.IsEmpty)
				{
					if (_0BD2.RightToLeft)
					{
						DisplayHelp.DrawGradientLine(graphics, rectangle.X + 1, rectangle.Y, rectangle.X + 1, rectangle.Bottom - 1, menu.SideBorderLight, 1);
					}
					else
					{
						DisplayHelp.DrawGradientLine(graphics, rectangle.Right - 1, rectangle.Y, rectangle.Right - 1, rectangle.Bottom - 1, menu.SideBorderLight, 1);
					}
				}
			}
		}
		if (base.IsOnCustomizeMenu)
		{
			if (EffectiveStyle == eDotNetBarStyle.OfficeXP || EffectiveStyle == eDotNetBarStyle.Office2003 || EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(EffectiveStyle))
			{
				ल.X += size.Height + 8;
				ल.Width -= size.Height + 8;
			}
			else
			{
				ल.X += size.Height + 4;
				ल.Width -= size.Height + 4;
			}
		}
	}

	private ⱨ ⱗ()
	{
		if (_ⱑ.AverageRated != null)
		{
			return new ⱨ(_ⱑ.AverageRated, new Rectangle(Point.Empty, _ⱑ.AverageRated.Size));
		}
		ⱜ();
		return new ⱨ(_ⱌ, new Rectangle(0, 52, _ⱋ.Width, _ⱋ.Height));
	}

	private ⱨ ⱘ()
	{
		if (_ⱑ.Rated != null)
		{
			return new ⱨ(_ⱑ.Rated, new Rectangle(Point.Empty, _ⱑ.Rated.Size));
		}
		ⱜ();
		return new ⱨ(_ⱌ, new Rectangle(0, 26, _ⱋ.Width, _ⱋ.Height));
	}

	private ⱨ ⱙ()
	{
		if (_ⱑ.Unrated != null)
		{
			return new ⱨ(_ⱑ.Unrated, new Rectangle(Point.Empty, _ⱑ.Unrated.Size));
		}
		ⱜ();
		return new ⱨ(_ⱌ, new Rectangle(0, 0, _ⱋ.Width, _ⱋ.Height));
	}

	private ⱨ ⱚ()
	{
		if (_ⱑ.UnratedMouseOver != null)
		{
			return new ⱨ(_ⱑ.UnratedMouseOver, new Rectangle(Point.Empty, _ⱑ.UnratedMouseOver.Size));
		}
		ⱜ();
		return new ⱨ(_ⱌ, new Rectangle(0, 13, _ⱋ.Width, _ⱋ.Height));
	}

	private ⱨ ⱛ()
	{
		if (_ⱑ.RatedMouseOver != null)
		{
			return new ⱨ(_ⱑ.RatedMouseOver, new Rectangle(Point.Empty, _ⱑ.RatedMouseOver.Size));
		}
		ⱜ();
		return new ⱨ(_ⱌ, new Rectangle(0, 39, _ⱋ.Width, _ⱋ.Height));
	}

	private void ⱜ()
	{
		if (_ⱌ == null)
		{
			_ⱌ = (Image)(object)BarFunctions.ই("SystemImages.Rating.png");
		}
	}

	public override void InternalMouseMove(MouseEventArgs e)
	{
		if (__29FF)
		{
			int ⱞ = -1;
			if (RatingOrientation == eOrientation.Horizontal)
			{
				for (int i = 0; i < _ⱔ; i++)
				{
					if (e.X >= _ⱊ[i].ঋ.X && e.X <= _ⱊ[i].ঋ.Right)
					{
						ⱞ = i;
						break;
					}
				}
			}
			else
			{
				for (int j = 0; j < _ⱔ; j++)
				{
					if ((e.Y >= _ⱊ[j].ঋ.Y && e.Y <= _ⱊ[j].ঋ.Bottom && (!_ᣅ || _ᢧ.IsEmpty)) || _ⱊ[j].ঋ.Contains(e.X, e.Y))
					{
						ⱞ = j;
						break;
					}
				}
			}
			ⱝ(ⱞ);
		}
		else
		{
			ⱝ(-1);
		}
		base.InternalMouseMove(e);
	}

	public override void InternalClick(MouseButtons buttons, Point position)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		if (_ⱍ >= 0)
		{
			SetRating(_ⱍ + 1, eEventSource.Mouse);
		}
		base.InternalClick(buttons, position);
	}

	private void ⱝ(int ⱞ)
	{
		if (ⱞ != _ⱍ)
		{
			_ⱍ = ⱞ;
			for (int i = 0; i < _ⱔ; i++)
			{
				_ⱊ[i].ᆼ = ⱞ >= 0 && i <= ⱞ;
			}
			Refresh();
		}
	}

	public override void InternalMouseLeave()
	{
		if (__29FF)
		{
			ⱝ(-1);
		}
		base.InternalMouseLeave();
	}

	private Rectangle ᦑ()
	{
		return ᦑ(DisplayRectangle);
	}

	private Rectangle ᦑ(Rectangle ശ)
	{
		ശ.X += ᝈ.ᝐ(_ᠩ);
		ശ.Y += ᝈ._1753(_ᠩ);
		ശ.Width -= ᝈ.ᝍ(_ᠩ);
		ശ.Height -= ᝈ.ᝏ(_ᠩ);
		return ശ;
	}

	private void _2C5F()
	{
		Rectangle rectangle = ᦑ();
		if (_ᣅ && !_ᢧ.IsEmpty && ((_ᠩ.TextAlignment == eStyleTextAlignment.Far && IsRightToLeft) || (!IsRightToLeft && _ᠩ.TextAlignment == eStyleTextAlignment.Near)))
		{
			rectangle.X += _ⱒ + _ᢧ.Width + (_ⱏ ? _ⱎ : 0);
		}
		if (_ⱏ && ((_ᠩ.TextAlignment == eStyleTextAlignment.Far && IsRightToLeft) || (!IsRightToLeft && _ᠩ.TextAlignment == eStyleTextAlignment.Near)))
		{
			Size size = ო();
			rectangle.X += size.Width + Dpi.Width7;
			if (base.IsOnCustomizeMenu)
			{
				rectangle.X += rectangle.Height + Dpi.Width2;
			}
		}
		Size size2 = Dpi.ImageSize(DisplayHelp.ഝ(_ⱋ, _ⱑ._26EE));
		if (rectangle.Height > size2.Height && RatingOrientation == eOrientation.Horizontal)
		{
			rectangle.Y += (rectangle.Height - size2.Height) / 2;
		}
		if (RatingOrientation == eOrientation.Horizontal)
		{
			Point location = rectangle.Location;
			for (int i = 0; i < _ⱔ; i++)
			{
				_ⱊ[i].ঋ = new Rectangle(location, size2);
				location.X += size2.Width;
			}
		}
		else
		{
			Point location2 = new Point(rectangle.X, rectangle.Bottom - size2.Height);
			for (int j = 0; j < _ⱔ; j++)
			{
				_ⱊ[j].ঋ = new Rectangle(location2, size2);
				location2.Y -= size2.Height;
			}
		}
	}

	public override void RecalcSize()
	{
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Invalid comparison between Unknown and I4
		Rectangle displayRectangle = DisplayRectangle;
		Size size = Dpi.ImageSize(DisplayHelp.ഝ(_ⱋ, _ⱑ._26EE));
		if (RatingOrientation == eOrientation.Horizontal)
		{
			size.Width *= _ⱔ;
		}
		else
		{
			size.Height *= _ⱔ;
		}
		size.Width += ᝈ.ᝍ(_ᠩ);
		size.Height += ᝈ.ᝏ(_ᠩ);
		_ⱏ = false;
		if (base.IsOnMenu && !(Parent is ItemContainer) && (EffectiveStyle == eDotNetBarStyle.OfficeXP || EffectiveStyle == eDotNetBarStyle.Office2003 || EffectiveStyle == eDotNetBarStyle.VS2005 || BarFunctions.IsOffice2007Style(EffectiveStyle)))
		{
			Size size2 = ო();
			size.Width += size2.Width + Dpi.Width7;
			if (base.IsOnCustomizeMenu)
			{
				size.Width += size.Height + Dpi.Width2;
			}
			size.Width += _ⱎ;
			_ⱏ = true;
		}
		_ᢧ = Size.Empty;
		if (_ᣅ)
		{
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
			try
			{
				Size ᢧ = _20B1._20B2(this, val2, 0, val.Font, eTextFormat.Default, (int)val.RightToLeft == 1);
				ᢧ.Width += Dpi.Width1;
				size.Width += _ⱒ + ᢧ.Width;
				size.Height = Math.Max(size.Height, ᢧ.Height);
				_ᢧ = ᢧ;
			}
			finally
			{
				val2.Dispose();
			}
		}
		_ⱐ = size;
		displayRectangle.Size = size;
		if (!(ContainerControl is RatingStar))
		{
			m_Rect = displayRectangle;
		}
		_2C5F();
		base.RecalcSize();
	}

	private Size ო()
	{
		if (m_Parent != null)
		{
			if (m_Parent is ImageItem imageItem)
			{
				return imageItem.SubItemsImageSize;
			}
			return ImageSize;
		}
		return ImageSize;
	}

	protected override void OnLeftLocationChanged(int oldValue)
	{
		_2C5F();
		base.OnLeftLocationChanged(oldValue);
	}

	protected override void OnTopLocationChanged(int oldValue)
	{
		_2C5F();
		base.OnTopLocationChanged(oldValue);
	}

	protected virtual void PaintBackground(ItemPaintArgs p)
	{
		_ᠩ.SetColorScheme(p.Colors);
		Graphics graphics = p.Graphics;
		ElementStyleDisplay.Paint(new ElementStyleDisplayInfo(_ᠩ, graphics, DisplayRectangle));
	}

	internal void ᥘ(ElementStyle Ⱡ)
	{
		_ᠩ.StyleChanged -= ᅻ;
		_ᠩ = Ⱡ;
		_ᠩ.StyleChanged += ᅻ;
	}

	private void ᅻ(object _٠, EventArgs ؿ)
	{
		NeedRecalcSize = true;
		OnAppearanceChanged();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTextColor()
	{
		return !__0CD1.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTextColor()
	{
		TextColor = Color.Empty;
	}

	protected override void TextMarkupLinkClick(object sender, EventArgs e)
	{
		if (sender is _0CFE obj)
		{
			OnMarkupLinkClick(new MarkupLinkClickEventArgs(obj._0D01, obj._0D00));
		}
		base.TextMarkupLinkClick(sender, e);
	}

	protected virtual void OnMarkupLinkClick(MarkupLinkClickEventArgs e)
	{
		if (Ӟ != null)
		{
			Ӟ(this, e);
		}
	}
}
