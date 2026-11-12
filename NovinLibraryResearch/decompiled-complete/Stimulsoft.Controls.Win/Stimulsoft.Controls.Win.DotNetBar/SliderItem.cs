using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

[Browsable(false)]
public class SliderItem : BaseItem
{
	private int m_Maximum = 400;

	private int m_Minimum = 10;

	private int m_Medium = 100;

	private int m_Value = 100;

	private int m_Step = 1;

	private bool m_LabelVisible = true;

	private int m_Width = 136;

	private int m_LabelWidth = 38;

	private eSliderLabelPosition m_LabelPosition;

	private eSliderPart m_MouseOverPart;

	private eSliderPart m_MouseDownPart;

	private int m_ButtonSize = StiScale2.XXI(18.0);

	private Size m_SlideSize = new Size(StiScale2.XXI(11.0), StiScale2.YYI(15.0));

	private bool m_TrackMarker = true;

	private Color m_TextColor = Color.Empty;

	private string m_DecreaseTooltip = "";

	private string m_IncreaseTooltip = "";

	private string m_ItemTooltip = "";

	private Timer m_ClickTimer;

	private Timer m_DelayClickTimer;

	private Rectangle m_LabelBounds = Rectangle.Empty;

	private Rectangle m_DecreaseBounds = Rectangle.Empty;

	private Rectangle m_IncreaseBounds = Rectangle.Empty;

	private Rectangle m_SlideBounds = Rectangle.Empty;

	private Rectangle m_TrackBounds = Rectangle.Empty;

	private bool _EnableMarkup = true;

	private eOrientation _SliderOrientation;

	internal eSliderPart MouseOverPart
	{
		get
		{
			return m_MouseOverPart;
		}
		set
		{
			m_MouseOverPart = value;
			UpdateTooltip();
			Refresh();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public eSliderPart MouseDownPart
	{
		get
		{
			return m_MouseDownPart;
		}
		set
		{
			m_MouseDownPart = value;
			Refresh();
		}
	}

	internal Rectangle LabelBounds => m_LabelBounds;

	internal Rectangle DecreaseBounds => m_DecreaseBounds;

	internal Rectangle IncreaseBounds => m_IncreaseBounds;

	internal Rectangle SlideBounds => m_SlideBounds;

	internal Rectangle TrackBounds
	{
		get
		{
			return m_TrackBounds;
		}
		set
		{
			m_TrackBounds = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("The text contained in the item.")]
	[Localizable(true)]
	[DefaultValue("")]
	public override string Text
	{
		get
		{
			return base.Text;
		}
		set
		{
			base.Text = value;
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
			}
		}
	}

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Description("Gets or sets the maximum value of the range of the control.")]
	[Category("Behavior")]
	[DefaultValue(100)]
	public int Maximum
	{
		get
		{
			return m_Maximum;
		}
		set
		{
			m_Maximum = value;
			Refresh();
			OnAppearanceChanged();
		}
	}

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Description("Gets or sets the medium value of the range of the control.")]
	[Category("Behavior")]
	[DefaultValue(100)]
	public int Medium
	{
		get
		{
			return m_Medium;
		}
		set
		{
			m_Medium = value;
			Refresh();
			OnAppearanceChanged();
		}
	}

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Description("Gets or sets the minimum value of the range of the control.")]
	[Category("Behavior")]
	[DefaultValue(0)]
	public int Minimum
	{
		get
		{
			return m_Minimum;
		}
		set
		{
			m_Minimum = value;
			Refresh();
			OnAppearanceChanged();
		}
	}

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Description("Gets or sets the current position of the slider.")]
	[Category("Behavior")]
	public int Value
	{
		get
		{
			return m_Value;
		}
		set
		{
			CancelIntValueEventArgs cancelIntValueEventArgs = new CancelIntValueEventArgs();
			cancelIntValueEventArgs.NewValue = value;
			OnValueChanging(cancelIntValueEventArgs);
			if (!cancelIntValueEventArgs.Cancel)
			{
				if (value < m_Minimum)
				{
					m_Value = m_Minimum;
				}
				else if (value > m_Maximum)
				{
					m_Value = m_Maximum;
				}
				else
				{
					m_Value = value;
				}
				OnValueChanged();
				Refresh();
				OnAppearanceChanged();
				ExecuteCommand();
			}
		}
	}

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Description("Gets or sets the amount by which a call to the PerformStep method increases the current position of the slider.")]
	[Category("Behavior")]
	[DefaultValue(1)]
	public int Step
	{
		get
		{
			return m_Step;
		}
		set
		{
			if (m_Step > 0)
			{
				m_Step = value;
			}
		}
	}

	private float Factor => (Maximum - Medium) / (Medium - Minimum);

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Description("Gets or sets whether the label text is displayed.")]
	[Category("Behavior")]
	[DefaultValue(true)]
	public bool LabelVisible
	{
		get
		{
			return m_LabelVisible;
		}
		set
		{
			m_LabelVisible = value;
			OnAppearanceChanged();
			Refresh();
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(136)]
	[Category("Layout")]
	[Description("Indicates the width of the slider part of the item in pixels.")]
	public int Width
	{
		get
		{
			return m_Width;
		}
		set
		{
			if (m_Width != value && value > 0)
			{
				m_Width = value;
				NeedRecalcSize = true;
				OnAppearanceChanged();
				Refresh();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(38)]
	[Category("Layout")]
	[Description("Indicates width of the label part of the item in pixels.")]
	public int LabelWidth
	{
		get
		{
			return m_LabelWidth;
		}
		set
		{
			if (m_LabelWidth != value && value > 0)
			{
				m_LabelWidth = value;
				NeedRecalcSize = true;
				OnAppearanceChanged();
				Refresh();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(eSliderLabelPosition.Left)]
	[Category("Layout")]
	[Description("Indicates text label position in relationship to the slider")]
	public eSliderLabelPosition LabelPosition
	{
		get
		{
			return m_LabelPosition;
		}
		set
		{
			m_LabelPosition = value;
			NeedRecalcSize = true;
			OnAppearanceChanged();
			Refresh();
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Gets or sets whether Click event will be auto repeated when mouse button is kept pressed over the item.")]
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

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DefaultValue(200)]
	[Category("Behavior")]
	[Description("Gets or sets the auto-repeat interval for the click event when mouse button is kept pressed over the item.")]
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

	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue(true)]
	[Description("Indicates whether vertical line track marker is displayed on the slide line.")]
	public virtual bool TrackMarker
	{
		get
		{
			return m_TrackMarker;
		}
		set
		{
			m_TrackMarker = value;
			Refresh();
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Indicates color of the label text.")]
	public Color TextColor
	{
		get
		{
			return m_TextColor;
		}
		set
		{
			m_TextColor = value;
			Refresh();
		}
	}

	[Browsable(false)]
	[Category("Appearance")]
	[DefaultValue("")]
	[Description("Indicates the Key Tips access key or keys for the item when on Ribbon Control or Ribbon Bar.")]
	public override string KeyTips
	{
		get
		{
			return base.KeyTips;
		}
		set
		{
			base.KeyTips = value;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[Category("Design")]
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
	[DevCoBrowsable(false)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Determines whether sub-items are displayed.")]
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
	[DevCoBrowsable(false)]
	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Specifies whether item is drawn using Themes when running on OS that supports themes like Windows XP.")]
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

	[Browsable(false)]
	[DevCoBrowsable(false)]
	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Indicates whether item will stretch to consume empty space. Items on stretchable, no-wrap Bars only.")]
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

	[Browsable(true)]
	[DefaultValue("")]
	[Localizable(true)]
	[Category("Appearance")]
	[Description("Indicates tooltip for the Increase button of the slider.")]
	public string IncreaseTooltip
	{
		get
		{
			return m_IncreaseTooltip;
		}
		set
		{
			m_IncreaseTooltip = value;
			UpdateTooltip();
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Localizable(true)]
	[Category("Appearance")]
	[Description("Indicates tooltip for the Increase button of the slider.")]
	public string DecreaseTooltip
	{
		get
		{
			return m_DecreaseTooltip;
		}
		set
		{
			m_DecreaseTooltip = value;
			UpdateTooltip();
		}
	}

	[DefaultValue(eOrientation.Horizontal)]
	[Category("Appearance")]
	[Description("Indicates slider orientation.")]
	public eOrientation SliderOrientation
	{
		get
		{
			return _SliderOrientation;
		}
		set
		{
			if (_SliderOrientation != value)
			{
				_SliderOrientation = value;
				NeedRecalcSize = true;
				Refresh();
			}
		}
	}

	public event EventHandler ValueChanged;

	public event CancelIntValueEventHandler ValueChanging;

	public SliderItem()
		: this("", "")
	{
	}

	public SliderItem(string sItemName)
		: this(sItemName, "")
	{
	}

	public SliderItem(string sItemName, string ItemText)
		: base(sItemName, ItemText)
	{
		ClickRepeatInterval = 200;
		MouseUpNotification = true;
		MouseDownCapture = true;
	}

	public override BaseItem Copy()
	{
		SliderItem sliderItem = new SliderItem(m_Name);
		CopyToItem(sliderItem);
		return sliderItem;
	}

	protected override void CopyToItem(BaseItem copy)
	{
		SliderItem sliderItem = copy as SliderItem;
		base.CopyToItem(sliderItem);
		sliderItem.Maximum = Maximum;
		sliderItem.Medium = Medium;
		sliderItem.Minimum = Minimum;
		sliderItem.Value = Value;
		sliderItem.Text = Text;
		sliderItem.LabelWidth = LabelWidth;
		sliderItem.LabelVisible = LabelVisible;
		sliderItem.LabelPosition = LabelPosition;
		sliderItem.Width = Width;
	}

	public new void Dispose()
	{
		DisposeClickTimer();
		DisposeDelayClickTimer();
		base.Dispose();
	}

	public override void Paint(ItemPaintArgs p)
	{
		BaseRenderer renderer = p.Renderer;
		if (renderer != null)
		{
			SliderItemRendererEventArgs sliderItemRendererEventArgs = new SliderItemRendererEventArgs(this, p.Graphics);
			sliderItemRendererEventArgs.ItemPaintArgs = p;
			renderer.DrawSliderItem(sliderItemRendererEventArgs);
		}
		else
		{
			SliderPainter sliderPainter = PainterFactory.CreateSliderPainter(this);
			if (sliderPainter != null)
			{
				SliderItemRendererEventArgs sliderItemRendererEventArgs2 = new SliderItemRendererEventArgs(this, p.Graphics);
				sliderItemRendererEventArgs2.ItemPaintArgs = p;
				sliderPainter.Paint(sliderItemRendererEventArgs2);
			}
		}
		if (DesignMode && Focused)
		{
			Rectangle rect = m_Rect;
			rect.Inflate(-1, -1);
			DesignTime.DrawDesignTimeSelection(p.Graphics, rect, p.Colors.ItemDesignTimeBorder);
		}
	}

	public override void RecalcSize()
	{
		Font font = GetFont(null);
		int buttonSize = m_ButtonSize;
		Size size = new Size(m_Width, buttonSize);
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
		if (m_LabelVisible)
		{
			Size size2 = ButtonItemLayout.MeasureItemText(this, val2, size.Width, font, eTextFormat.Default, IsRightToLeft);
			if (m_LabelPosition == eSliderLabelPosition.Left || m_LabelPosition == eSliderLabelPosition.Right)
			{
				size.Height = Math.Max(buttonSize, Math.Max(size2.Height, font.Height));
				size.Width += m_LabelWidth;
			}
			else
			{
				size.Height = buttonSize + Math.Max(size2.Height, font.Height);
			}
		}
		if (_SliderOrientation == eOrientation.Vertical)
		{
			size = new Size(size.Height, size.Width);
		}
		m_Rect.Size = size;
		UpdateLayout();
		base.RecalcSize();
	}

	public eSliderPart HitTest(int x, int y)
	{
		eSliderPart result = eSliderPart.None;
		Rectangle decreaseBounds = DecreaseBounds;
		decreaseBounds.Offset(Bounds.Location);
		if (decreaseBounds.Contains(x, y))
		{
			result = eSliderPart.DecreaseButton;
		}
		else
		{
			decreaseBounds = IncreaseBounds;
			decreaseBounds.Offset(Bounds.Location);
			if (decreaseBounds.Contains(x, y))
			{
				result = eSliderPart.IncreaseButton;
			}
			else
			{
				decreaseBounds = SlideBounds;
				decreaseBounds.Offset(Bounds.Location);
				if (decreaseBounds.Contains(x, y))
				{
					result = eSliderPart.TrackArea;
				}
			}
		}
		return result;
	}

	public override void InternalMouseMove(MouseEventArgs objArg)
	{
		if (GetEnabled() && !DesignMode)
		{
			if (MouseDownPart == eSliderPart.None)
			{
				eSliderPart eSliderPart2 = HitTest(objArg.X, objArg.Y);
				if (MouseOverPart != eSliderPart2)
				{
					MouseOverPart = eSliderPart2;
				}
			}
			else if (MouseDownPart == eSliderPart.TrackArea)
			{
				if (SliderOrientation == eOrientation.Horizontal)
				{
					int num = GetValueFromX(objArg.X);
					float num2 = (Medium - Minimum) / 10;
					float num3 = (Maximum - Medium) / 10;
					if ((float)num > (float)Medium - num2 && (float)num < (float)Medium + num3)
					{
						num = 100;
					}
					Value = num;
				}
				else
				{
					Value = GetValueFromY(objArg.Y);
				}
			}
		}
		base.InternalMouseMove(objArg);
	}

	public override void InternalMouseLeave()
	{
		if (m_MouseOverPart != 0)
		{
			MouseOverPart = eSliderPart.None;
		}
		DisposeClickTimer();
		DisposeDelayClickTimer();
		if (MouseDownPart != 0)
		{
			MouseDownPart = eSliderPart.None;
		}
		base.InternalMouseLeave();
	}

	private void DisposeClickTimer()
	{
		if (m_ClickTimer != null)
		{
			m_ClickTimer.Stop();
			((Component)(object)m_ClickTimer).Dispose();
			m_ClickTimer = null;
		}
	}

	private void DisposeDelayClickTimer()
	{
		if (m_DelayClickTimer != null)
		{
			m_DelayClickTimer.Stop();
			((Component)(object)m_DelayClickTimer).Dispose();
			m_DelayClickTimer = null;
		}
	}

	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Invalid comparison between Unknown and I4
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Expected O, but got Unknown
		if (GetEnabled() && !DesignMode && (int)objArg.Button == 1048576)
		{
			eSliderPart eSliderPart3 = (MouseDownPart = HitTest(objArg.X, objArg.Y));
			if (MouseDownPart == eSliderPart.DecreaseButton)
			{
				if (Value < Medium)
				{
					Value -= Step;
				}
				else
				{
					Value -= (int)((float)Step * Factor);
				}
			}
			else if (MouseDownPart == eSliderPart.IncreaseButton)
			{
				PerformStep();
			}
			else if (eSliderPart3 == eSliderPart.TrackArea && !m_TrackBounds.Contains(objArg.X, objArg.Y))
			{
				if (SliderOrientation == eOrientation.Horizontal)
				{
					Value = GetValueFromX(objArg.X);
				}
				else
				{
					Value = GetValueFromY(objArg.Y);
				}
			}
			if (eSliderPart3 == eSliderPart.IncreaseButton || eSliderPart3 == eSliderPart.DecreaseButton)
			{
				if (m_DelayClickTimer == null)
				{
					m_DelayClickTimer = new Timer();
				}
				m_DelayClickTimer.Interval = 400;
				m_DelayClickTimer.Tick += DelayClickTimerTick;
				m_DelayClickTimer.Start();
			}
		}
		base.InternalMouseDown(objArg);
	}

	private int GetValueFromX(int x)
	{
		x -= (m_SlideSize.Width - 2) / 2;
		Rectangle slideBounds = SlideBounds;
		slideBounds.Width -= m_SlideSize.Width;
		slideBounds.Offset(DisplayRectangle.Location);
		if (x <= slideBounds.X)
		{
			if (IsRightToLeft)
			{
				return Maximum;
			}
			return Minimum;
		}
		if (x >= slideBounds.Right)
		{
			if (IsRightToLeft)
			{
				return Minimum;
			}
			return Maximum;
		}
		int num = (slideBounds.Right - slideBounds.X) / 2;
		int num2 = x - slideBounds.X;
		if (IsRightToLeft)
		{
			num2 = slideBounds.Width - num2;
		}
		int num3 = 0;
		if (num2 < num)
		{
			float num4 = (float)num2 / (float)num;
			return Minimum + (int)(((float)Medium - (float)Minimum) * num4);
		}
		num2 -= num;
		float num5 = (float)num2 / (float)num;
		return Medium + (int)(((float)Maximum - (float)Medium) * num5);
	}

	private int GetValueFromY(int y)
	{
		y -= (m_SlideSize.Width - 2) / 2;
		Rectangle slideBounds = SlideBounds;
		slideBounds.Height -= m_SlideSize.Width;
		slideBounds.Offset(DisplayRectangle.Location);
		if (y <= slideBounds.Y)
		{
			return Maximum;
		}
		if (y >= slideBounds.Bottom)
		{
			return Minimum;
		}
		float num = 1f - (float)(y - slideBounds.Y) / (float)(slideBounds.Bottom - slideBounds.Y);
		int num2 = Minimum + (int)((float)(Maximum - Minimum) * num);
		if (Step > 1 && Math.Ceiling((float)(num2 - Minimum) / (float)Step) * (double)Step != (double)(num2 - Minimum))
		{
			num2 = Minimum + (int)Math.Ceiling((float)(num2 - Minimum) / (float)Step) * Step;
		}
		return num2;
	}

	private void DelayClickTimerTick(object sender, EventArgs e)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		if (m_ClickTimer == null)
		{
			m_ClickTimer = new Timer();
		}
		m_ClickTimer.Interval = ClickRepeatInterval;
		m_ClickTimer.Tick += ClickTimerTick;
		m_ClickTimer.Start();
	}

	private void ClickTimerTick(object sender, EventArgs e)
	{
		if (MouseDownPart == eSliderPart.DecreaseButton)
		{
			if (Value < Medium)
			{
				Value -= Step;
			}
			else
			{
				Value -= (int)((float)Step * Factor);
			}
		}
		else if (MouseDownPart == eSliderPart.IncreaseButton)
		{
			PerformStep();
		}
	}

	public override void InternalMouseUp(MouseEventArgs objArg)
	{
		DisposeClickTimer();
		DisposeDelayClickTimer();
		if (MouseDownPart != 0)
		{
			MouseDownPart = eSliderPart.None;
		}
		base.InternalMouseUp(objArg);
	}

	protected override void OnExternalSizeChange()
	{
		UpdateLayout();
		base.OnExternalSizeChange();
	}

	private void UpdateLayout()
	{
		bool isRightToLeft = IsRightToLeft;
		Rectangle rectangle = new Rectangle(Point.Empty, DisplayRectangle.Size);
		if (m_LabelVisible)
		{
			if ((m_LabelPosition == eSliderLabelPosition.Left && (!isRightToLeft || _SliderOrientation == eOrientation.Vertical)) || (m_LabelPosition == eSliderLabelPosition.Right && isRightToLeft))
			{
				if (_SliderOrientation == eOrientation.Vertical)
				{
					m_LabelBounds = new Rectangle(0, StiScale2.I2, rectangle.Width, m_LabelWidth - StiScale2.I2);
					rectangle.Height -= m_LabelWidth;
					rectangle.Y += m_LabelWidth;
				}
				else
				{
					m_LabelBounds = new Rectangle(StiScale2.I2, 0, m_LabelWidth - StiScale2.I2, rectangle.Height);
					rectangle.Width -= m_LabelWidth;
					rectangle.X += m_LabelWidth;
				}
			}
			else if ((m_LabelPosition == eSliderLabelPosition.Left && isRightToLeft) || (m_LabelPosition == eSliderLabelPosition.Right && (!isRightToLeft || _SliderOrientation == eOrientation.Vertical)))
			{
				if (_SliderOrientation == eOrientation.Vertical)
				{
					m_LabelBounds = new Rectangle(0, rectangle.Height - m_LabelWidth + StiScale2.I2, rectangle.Width, m_LabelWidth - StiScale2.I2);
					rectangle.Height -= m_LabelWidth;
				}
				else
				{
					m_LabelBounds = new Rectangle(rectangle.Width - m_LabelWidth + StiScale2.I2, 0, m_LabelWidth - StiScale2.I2, rectangle.Height);
					rectangle.Width -= m_LabelWidth;
				}
			}
			else if (m_LabelPosition == eSliderLabelPosition.Top)
			{
				Font font = GetFont(null);
				int num = ((font != null) ? font.Height : StiScale2.YYI(14.0));
				if (base.TextMarkupBody != null)
				{
					num = base.TextMarkupBody.Bounds.Height;
				}
				int num2 = m_ButtonSize + num;
				if (_SliderOrientation == eOrientation.Vertical)
				{
					m_LabelBounds = new Rectangle((rectangle.Width - num2) / 2, 0, num, rectangle.Height);
					rectangle.X = m_LabelBounds.Right;
					rectangle.Width = m_ButtonSize;
				}
				else
				{
					m_LabelBounds = new Rectangle(0, (rectangle.Height - num2) / 2, rectangle.Width, num);
					rectangle.Y = m_LabelBounds.Bottom;
					rectangle.Height = m_ButtonSize;
				}
			}
			else if (m_LabelPosition == eSliderLabelPosition.Bottom)
			{
				Font font2 = GetFont(null);
				int num3 = ((font2 != null) ? font2.Height : StiScale2.YYI(14.0));
				if (base.TextMarkupBody != null)
				{
					num3 = base.TextMarkupBody.Bounds.Height;
				}
				int num4 = m_ButtonSize + num3;
				if (_SliderOrientation == eOrientation.Vertical)
				{
					m_LabelBounds = new Rectangle((rectangle.Width - num4) / 2 + m_ButtonSize, 0, num3, rectangle.Height);
					rectangle.X = (rectangle.Width - num4) / 2;
					rectangle.Width = m_ButtonSize;
				}
				else
				{
					m_LabelBounds = new Rectangle(0, (rectangle.Height - num4) / 2 + m_ButtonSize, rectangle.Width, num3);
					rectangle.Y = (rectangle.Height - num4) / 2;
					rectangle.Height = m_ButtonSize;
				}
			}
		}
		else
		{
			m_LabelBounds = Rectangle.Empty;
		}
		if (isRightToLeft && _SliderOrientation == eOrientation.Horizontal)
		{
			m_DecreaseBounds = new Rectangle(rectangle.Right - m_ButtonSize, rectangle.Y + (rectangle.Height - m_ButtonSize) / 2, m_ButtonSize, m_ButtonSize);
			m_IncreaseBounds = new Rectangle(rectangle.X, rectangle.Y + (rectangle.Height - m_ButtonSize) / 2, m_ButtonSize, m_ButtonSize);
		}
		else if (_SliderOrientation == eOrientation.Horizontal)
		{
			m_DecreaseBounds = new Rectangle(rectangle.X, rectangle.Y + (rectangle.Height - m_ButtonSize) / 2, m_ButtonSize, m_ButtonSize);
			m_IncreaseBounds = new Rectangle(rectangle.Right - m_ButtonSize, rectangle.Y + (rectangle.Height - m_ButtonSize) / 2, m_ButtonSize, m_ButtonSize);
		}
		else
		{
			m_DecreaseBounds = new Rectangle(rectangle.X + (rectangle.Width - m_ButtonSize) / 2, rectangle.Bottom - m_ButtonSize, m_ButtonSize, m_ButtonSize);
			m_IncreaseBounds = new Rectangle(rectangle.X + (rectangle.Width - m_ButtonSize) / 2, rectangle.Y, m_ButtonSize, m_ButtonSize);
		}
		if (_SliderOrientation == eOrientation.Horizontal)
		{
			m_SlideBounds = new Rectangle(rectangle.X + m_ButtonSize, rectangle.Y, rectangle.Width - m_ButtonSize * 2, rectangle.Height);
		}
		else
		{
			m_SlideBounds = new Rectangle(rectangle.X, rectangle.Y + m_ButtonSize, rectangle.Width, rectangle.Height - m_ButtonSize * 2);
		}
	}

	private Font GetFont(ItemPaintArgs pa)
	{
		Font val = null;
		if (pa != null)
		{
			val = pa.Font;
		}
		if (val == null)
		{
			Control val2 = null;
			if (pa != null)
			{
				val2 = pa.ContainerControl;
			}
			if (val2 == null)
			{
				object containerControl = ContainerControl;
				val2 = (Control)((containerControl is Control) ? containerControl : null);
			}
			val = ((val2 == null || val2.Font == null) ? SystemInformation.MenuFont : val2.Font);
		}
		return val;
	}

	public virtual void PerformStep()
	{
		if (Value < Medium)
		{
			Value += m_Step;
		}
		else
		{
			Value += (int)((float)m_Step * Factor);
		}
	}

	public virtual void Increment(int value)
	{
		Value += value;
	}

	protected virtual void OnValueChanged()
	{
		if (this.ValueChanged != null)
		{
			this.ValueChanged(this, new EventArgs());
		}
	}

	protected virtual void OnValueChanging(CancelIntValueEventArgs e)
	{
		if (this.ValueChanging != null)
		{
			this.ValueChanging(this, e);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTextColor()
	{
		return !m_TextColor.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTextColor()
	{
		TextColor = Color.Empty;
	}

	protected override void OnTooltipChanged()
	{
		m_ItemTooltip = Tooltip;
		UpdateTooltip();
		base.OnTooltipChanged();
	}

	private void UpdateTooltip()
	{
		if (!DesignMode)
		{
			if (m_MouseOverPart == eSliderPart.DecreaseButton && m_DecreaseTooltip != "")
			{
				SetInternalTooltip(m_DecreaseTooltip);
			}
			else if (m_MouseOverPart == eSliderPart.IncreaseButton && m_IncreaseTooltip != "")
			{
				SetInternalTooltip(m_IncreaseTooltip);
			}
			else if (m_Tooltip != m_ItemTooltip)
			{
				SetInternalTooltip(m_ItemTooltip);
			}
		}
	}

	private void SetInternalTooltip(string t)
	{
		if (m_Tooltip != t)
		{
			m_Tooltip = t;
			if (base.ToolTipVisible)
			{
				ShowToolTip();
			}
		}
	}
}
