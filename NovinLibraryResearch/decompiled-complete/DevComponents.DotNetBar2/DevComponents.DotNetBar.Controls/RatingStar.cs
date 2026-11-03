using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using DevComponents.Editors;

namespace DevComponents.DotNetBar.Controls;

[ToolboxItem(true)]
[ToolboxBitmap(typeof(RatingStar), "Controls.RatingStar.ico")]
[DefaultEvent("RatingChanged")]
[ComVisible(true)]
public class RatingStar : BaseItemControl, ICommandSource
{
	private RatingItem _ⰹ;

	private EventHandler ⰺ;

	private EventHandler ⰻ;

	private RatingChangeEventHandler ⰼ;

	private EventHandler ⰽ;

	private EventHandler ⰾ;

	private MarkupLinkClickEventHandler Ӟ;

	private ParseIntegerValueEventHandler ⰿ;

	private ParseDoubleValueEventHandler ⱀ;

	private ICommand _૦;

	private object _૧;

	[Category("Appearance")]
	[DefaultValue(5)]
	[Description("Indicates number of stars used for the rating.")]
	public int NumberOfStars
	{
		get
		{
			return _ⰹ.NumberOfStars;
		}
		set
		{
			_ⰹ.NumberOfStars = value;
			if (((Control)this).AutoSize)
			{
				_21DF();
			}
		}
	}

	[Category("Data")]
	[Description("Indicates rating value represented by the control.")]
	[DefaultValue(0)]
	public int Rating
	{
		get
		{
			return _ⰹ.Rating;
		}
		set
		{
			_ⰹ.Rating = value;
		}
	}

	[Description("Indicates average rating shown by control.")]
	[Category("Data")]
	[DefaultValue(0.0)]
	public double AverageRating
	{
		get
		{
			return _ⰹ.AverageRating;
		}
		set
		{
			_ⰹ.AverageRating = value;
		}
	}

	[RefreshProperties(RefreshProperties.All)]
	[TypeConverter(typeof(StringConverter))]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Bindable(true)]
	public object AverageRatingValue
	{
		get
		{
			return _ⰹ.AverageRatingValue;
		}
		set
		{
			_ⰹ.AverageRatingValue = value;
		}
	}

	[Category("Images")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Gets the reference to custom rating images.")]
	[Browsable(true)]
	public RatingImages CustomImages => _ⰹ.CustomImages;

	[DefaultValue(true)]
	[Browsable(true)]
	[Description("Indicates whether text assigned to the check box is visible.")]
	[Category("Appearance")]
	public bool TextVisible
	{
		get
		{
			return _ⰹ.TextVisible;
		}
		set
		{
			_ⰹ.TextVisible = value;
		}
	}

	[Description("Indicates whether text-markup support is enabled for items Text property.")]
	[Category("Appearance")]
	[DefaultValue(true)]
	public bool EnableMarkup
	{
		get
		{
			return _ⰹ.EnableMarkup;
		}
		set
		{
			_ⰹ.EnableMarkup = value;
		}
	}

	[Description("Indicates whether rating can be edited.")]
	[DefaultValue(true)]
	[Category("Behavior")]
	public bool IsEditable
	{
		get
		{
			return _ⰹ.IsEditable;
		}
		set
		{
			_ⰹ.IsEditable = value;
		}
	}

	[Category("Appearance")]
	[Description("Gets or sets the orientation of rating control.")]
	[DefaultValue(eOrientation.Horizontal)]
	public eOrientation RatingOrientation
	{
		get
		{
			return _ⰹ.RatingOrientation;
		}
		set
		{
			_ⰹ.RatingOrientation = value;
		}
	}

	[RefreshProperties(RefreshProperties.All)]
	[Browsable(false)]
	[Bindable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public object RatingValue
	{
		get
		{
			return _ⰹ.RatingValue;
		}
		set
		{
			_ⰹ.RatingValue = value;
		}
	}

	[Description("Indicates text color.")]
	[Browsable(true)]
	[Category("Appearance")]
	public Color TextColor
	{
		get
		{
			return _ⰹ.TextColor;
		}
		set
		{
			_ⰹ.TextColor = value;
		}
	}

	[Description("Gets or sets the spacing between optional text and the rating.")]
	[DefaultValue(0)]
	[Category("Appearance")]
	public int TextSpacing
	{
		get
		{
			return _ⰹ.TextSpacing;
		}
		set
		{
			_ⰹ.TextSpacing = value;
		}
	}

	[Localizable(true)]
	[Browsable(false)]
	public Padding Padding
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).Padding;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).Padding = value;
		}
	}

	[DefaultValue(false)]
	[Browsable(true)]
	[EditorBrowsable(EditorBrowsableState.Always)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	public override bool AutoSize
	{
		get
		{
			return ((Control)this).AutoSize;
		}
		set
		{
			if (((Control)this).AutoSize != value)
			{
				((Control)this).AutoSize = value;
				_21DF();
			}
		}
	}

	protected Size CalcSize => _ⰹ.ⱆ;

	[Category("Commands")]
	[Description("Indicates the command assigned to the item.")]
	[DefaultValue(null)]
	public Command Command
	{
		get
		{
			return (Command)((ICommandSource)this).Command;
		}
		set
		{
			((ICommandSource)this).Command = value;
		}
	}

	ICommand ICommandSource.ᒍ
	{
		get
		{
			return _૦;
		}
		set
		{
			bool flag = false;
			if (_૦ != value)
			{
				flag = true;
			}
			if (_૦ != null)
			{
				CommandManager.UnRegisterCommandSource(this, _૦);
			}
			_૦ = value;
			if (value != null)
			{
				CommandManager.RegisterCommand(this, value);
			}
			if (flag)
			{
				OnCommandChanged();
			}
		}
	}

	[Description("Indicates user defined data value that can be passed to the command when it is executed.")]
	[TypeConverter(typeof(StringConverter))]
	[Localizable(true)]
	[DefaultValue(null)]
	[Browsable(true)]
	[Category("Commands")]
	public object CommandParameter
	{
		get
		{
			return _૧;
		}
		set
		{
			_૧ = value;
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

	public RatingStar()
	{
		((Control)this).SetStyle((ControlStyles)512, false);
		_ⰹ = new RatingItem();
		_ⰹ.Style = eDotNetBarStyle.Office2007;
		_ⰹ.RatingChanging += ⱅ;
		_ⰹ.RatingChanged += ⱄ;
		_ⰹ.AverageRatingChanged += ⱃ;
		_ⰹ.ParseAverageRatingValue += ⱂ;
		_ⰹ.ParseRatingValue += ⱁ;
		HostItem = _ⰹ;
	}

	private void ⱁ(object _٠, ParseIntegerValueEventArgs ؿ)
	{
		OnParseRatingValue(ؿ);
	}

	protected virtual void OnParseRatingValue(ParseIntegerValueEventArgs e)
	{
		if (ⰿ != null)
		{
			ⰿ(this, e);
		}
	}

	private void ⱂ(object _٠, ParseDoubleValueEventArgs ؿ)
	{
		OnParseAverageRatingValue(ؿ);
	}

	protected virtual void OnParseAverageRatingValue(ParseDoubleValueEventArgs e)
	{
		if (ⱀ != null)
		{
			ⱀ(this, e);
		}
	}

	private void ⱃ(object _٠, EventArgs ؿ)
	{
		OnAverageRatingChanged(ؿ);
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

	private void ⱄ(object _٠, EventArgs ؿ)
	{
		OnRatingChanged(ؿ);
	}

	protected virtual void OnRatingChanged(EventArgs eventArgs)
	{
		ⰺ?.Invoke(this, eventArgs);
		ⰻ?.Invoke(this, eventArgs);
	}

	private void ⱅ(object _٠, RatingChangeEventArgs ؿ)
	{
		OnRatingChanging(ؿ);
	}

	protected virtual void OnRatingChanging(RatingChangeEventArgs e)
	{
		if (ⰼ != null)
		{
			ⰼ(this, e);
		}
	}

	protected override void OnResize(EventArgs e)
	{
		base.OnResize(e);
	}

	public override Size GetPreferredSize(Size proposedSize)
	{
		if (!BarFunctions.IsHandleValid((Control)(object)this))
		{
			return ((Control)this).GetPreferredSize(proposedSize);
		}
		_ⰹ.RecalcSize();
		Size ⱆ = _ⰹ.ⱆ;
		ⱆ.Width += 2;
		ⱆ.Height += 2;
		if (!TextVisible)
		{
			ⱆ.Width += 2;
		}
		ⱆ.Width += ᝈ.ᝍ(GetBackgroundStyle());
		ⱆ.Height += ᝈ.ᝏ(GetBackgroundStyle());
		_ⰹ.Bounds = GetItemBounds();
		return ⱆ;
	}

	protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		if (((Control)this).AutoSize)
		{
			Size preferredSize = ((Control)this).PreferredSize;
			width = preferredSize.Width;
			height = preferredSize.Height;
		}
		((Control)this).SetBoundsCore(x, y, width, height, specified);
	}

	private void _21DF()
	{
		if (((Control)this).AutoSize)
		{
			((Control)this).Size = ((Control)this).PreferredSize;
		}
	}

	protected override void OnVisualPropertyChanged()
	{
		base.OnVisualPropertyChanged();
		_21DF();
	}

	protected override void OnTextChanged(EventArgs e)
	{
		base.OnTextChanged(e);
		_21DF();
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		((Control)this).OnHandleCreated(e);
		if (((Control)this).AutoSize)
		{
			_21DF();
		}
	}

	protected virtual void ExecuteCommand()
	{
		if (_૦ != null)
		{
			CommandManager._૭(this);
		}
	}

	protected virtual void OnCommandChanged()
	{
	}
}
