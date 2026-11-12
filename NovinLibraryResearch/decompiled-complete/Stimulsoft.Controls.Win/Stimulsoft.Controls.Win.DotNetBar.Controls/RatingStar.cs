using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.Editors;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

[ToolboxBitmap(typeof(RatingStar), "Controls.RatingStar.ico")]
[ToolboxItem(false)]
[DefaultEvent("RatingChanged")]
[ComVisible(true)]
public class RatingStar : BaseItemControl, ICommandSource
{
	private RatingItem _RatingItem;

	private ICommand _Command;

	private object _CommandParameter;

	[DefaultValue(5)]
	[Category("Appearance")]
	[Description("Indicates number of stars used for the rating.")]
	public int NumberOfStars
	{
		get
		{
			return _RatingItem.NumberOfStars;
		}
		set
		{
			_RatingItem.NumberOfStars = value;
			if (((Control)this).AutoSize)
			{
				AdjustSize();
			}
		}
	}

	[DefaultValue(0)]
	[Category("Data")]
	[Description("Indicates rating value represented by the control.")]
	public int Rating
	{
		get
		{
			return _RatingItem.Rating;
		}
		set
		{
			_RatingItem.Rating = value;
		}
	}

	[DefaultValue(0.0)]
	[Category("Data")]
	[Description("Indicates average rating shown by control.")]
	public double AverageRating
	{
		get
		{
			return _RatingItem.AverageRating;
		}
		set
		{
			_RatingItem.AverageRating = value;
		}
	}

	[Bindable(true)]
	[Browsable(false)]
	[RefreshProperties(RefreshProperties.All)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[TypeConverter(typeof(StringConverter))]
	public object AverageRatingValue
	{
		get
		{
			return _RatingItem.AverageRatingValue;
		}
		set
		{
			_RatingItem.AverageRatingValue = value;
		}
	}

	[Browsable(true)]
	[Category("Images")]
	[Description("Gets the reference to custom rating images.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public RatingImages CustomImages => _RatingItem.CustomImages;

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether text assigned to the check box is visible.")]
	public bool TextVisible
	{
		get
		{
			return _RatingItem.TextVisible;
		}
		set
		{
			_RatingItem.TextVisible = value;
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether text-markup support is enabled for items Text property.")]
	public bool EnableMarkup
	{
		get
		{
			return _RatingItem.EnableMarkup;
		}
		set
		{
			_RatingItem.EnableMarkup = value;
		}
	}

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether rating can be edited.")]
	public bool IsEditable
	{
		get
		{
			return _RatingItem.IsEditable;
		}
		set
		{
			_RatingItem.IsEditable = value;
		}
	}

	[DefaultValue(eOrientation.Horizontal)]
	[Category("Appearance")]
	[Description("Gets or sets the orientation of rating control.")]
	public eOrientation RatingOrientation
	{
		get
		{
			return _RatingItem.RatingOrientation;
		}
		set
		{
			_RatingItem.RatingOrientation = value;
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
			return _RatingItem.RatingValue;
		}
		set
		{
			_RatingItem.RatingValue = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates text color.")]
	public Color TextColor
	{
		get
		{
			return _RatingItem.TextColor;
		}
		set
		{
			_RatingItem.TextColor = value;
		}
	}

	[DefaultValue(0)]
	[Category("Appearance")]
	[Description("Gets or sets the spacing between optional text and the rating.")]
	public int TextSpacing
	{
		get
		{
			return _RatingItem.TextSpacing;
		}
		set
		{
			_RatingItem.TextSpacing = value;
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

	[Browsable(true)]
	[DefaultValue(false)]
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
				AdjustSize();
			}
		}
	}

	protected Size CalcSize => _RatingItem.CalcSize;

	[DefaultValue(null)]
	[Category("Commands")]
	[Description("Indicates the command assigned to the item.")]
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

	ICommand ICommandSource.Command
	{
		get
		{
			return _Command;
		}
		set
		{
			bool flag = false;
			if (_Command != value)
			{
				flag = true;
			}
			if (_Command != null)
			{
				CommandManager.UnRegisterCommandSource(this, _Command);
			}
			_Command = value;
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

	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Commands")]
	[Description("Indicates user defined data value that can be passed to the command when it is executed.")]
	[TypeConverter(typeof(StringConverter))]
	[Localizable(true)]
	public object CommandParameter
	{
		get
		{
			return _CommandParameter;
		}
		set
		{
			_CommandParameter = value;
		}
	}

	[Description("Occurs when Rating property has changed.")]
	public event EventHandler RatingChanged;

	[Description("Occurs when Rating property has changed.")]
	public event EventHandler RatingValueChanged;

	[Description("Occurs when Rating property has changed.")]
	public event RatingChangeEventHandler RatingChanging;

	[Description("Occurs when AverageRating property has changed.")]
	public event EventHandler AverageRatingChanged;

	[Description("Occurs when AverageRatingValue property has changed.")]
	public event EventHandler AverageRatingValueChanged;

	public event MarkupLinkClickEventHandler MarkupLinkClick;

	public event ParseIntegerValueEventHandler ParseRatingValue;

	public event ParseDoubleValueEventHandler ParseAverageRatingValue;

	public RatingStar()
	{
		((Control)this).SetStyle((ControlStyles)512, false);
		_RatingItem = new RatingItem();
		_RatingItem.Style = eDotNetBarStyle.Office2007;
		_RatingItem.RatingChanging += RatingItemRatingChanging;
		_RatingItem.RatingChanged += RatingItemRatingChanged;
		_RatingItem.AverageRatingChanged += RatingItemAverageRatingChanged;
		_RatingItem.ParseAverageRatingValue += RatingItemParseAverageRatingValue;
		_RatingItem.ParseRatingValue += RatingItemParseRatingValue;
		HostItem = _RatingItem;
	}

	private void RatingItemParseRatingValue(object sender, ParseIntegerValueEventArgs e)
	{
		OnParseRatingValue(e);
	}

	protected virtual void OnParseRatingValue(ParseIntegerValueEventArgs e)
	{
		if (this.ParseRatingValue != null)
		{
			this.ParseRatingValue(this, e);
		}
	}

	private void RatingItemParseAverageRatingValue(object sender, ParseDoubleValueEventArgs e)
	{
		OnParseAverageRatingValue(e);
	}

	protected virtual void OnParseAverageRatingValue(ParseDoubleValueEventArgs e)
	{
		if (this.ParseAverageRatingValue != null)
		{
			this.ParseAverageRatingValue(this, e);
		}
	}

	private void RatingItemAverageRatingChanged(object sender, EventArgs e)
	{
		OnAverageRatingChanged(e);
	}

	protected virtual void OnAverageRatingChanged(EventArgs eventArgs)
	{
		if (this.AverageRatingChanged != null)
		{
			this.AverageRatingChanged(this, eventArgs);
		}
		if (this.AverageRatingValueChanged != null)
		{
			this.AverageRatingValueChanged(this, eventArgs);
		}
	}

	private void RatingItemRatingChanged(object sender, EventArgs e)
	{
		OnRatingChanged(e);
	}

	protected virtual void OnRatingChanged(EventArgs eventArgs)
	{
		this.RatingChanged?.Invoke(this, eventArgs);
		this.RatingValueChanged?.Invoke(this, eventArgs);
	}

	private void RatingItemRatingChanging(object sender, RatingChangeEventArgs e)
	{
		OnRatingChanging(e);
	}

	protected virtual void OnRatingChanging(RatingChangeEventArgs e)
	{
		if (this.RatingChanging != null)
		{
			this.RatingChanging(this, e);
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
		_RatingItem.RecalcSize();
		Size calcSize = _RatingItem.CalcSize;
		calcSize.Width += 2;
		calcSize.Height += 2;
		if (!TextVisible)
		{
			calcSize.Width += 2;
		}
		calcSize.Width += ElementStyleLayout.HorizontalStyleWhiteSpace(GetBackgroundStyle());
		calcSize.Height += ElementStyleLayout.VerticalStyleWhiteSpace(GetBackgroundStyle());
		_RatingItem.Bounds = GetItemBounds();
		return calcSize;
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

	private void AdjustSize()
	{
		if (((Control)this).AutoSize)
		{
			((Control)this).Size = ((Control)this).PreferredSize;
		}
	}

	protected override void OnVisualPropertyChanged()
	{
		base.OnVisualPropertyChanged();
		AdjustSize();
	}

	protected override void OnTextChanged(EventArgs e)
	{
		base.OnTextChanged(e);
		AdjustSize();
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		((Control)this).OnHandleCreated(e);
		if (((Control)this).AutoSize)
		{
			AdjustSize();
		}
	}

	protected virtual void ExecuteCommand()
	{
		if (_Command != null)
		{
			CommandManager.ExecuteCommand(this);
		}
	}

	protected virtual void OnCommandChanged()
	{
	}
}
