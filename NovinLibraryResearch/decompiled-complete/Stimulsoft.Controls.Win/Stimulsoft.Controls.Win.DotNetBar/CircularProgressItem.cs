using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
public class CircularProgressItem : BaseItem
{
	private int _SpokeCount = 12;

	private int _EndlessProgressValue;

	internal static readonly int TextContentSpacing = 3;

	private Size _TextSize = Size.Empty;

	private Rectangle _ProgressBarBounds = Rectangle.Empty;

	private double[] _SpokeAngles;

	private eCircularProgressType _ProgressBarType;

	private int _Maximum = 100;

	private int _Minimum;

	private Color _ProgressTextColor = Color.Empty;

	private bool _ProgressTextVisible;

	private string _ProgressText = "";

	private int _Value;

	private bool _IsEndlessProgressBar;

	private BackgroundWorker _LoopWorker;

	private bool _IsRunning;

	private static readonly Color DefaultProgressColor = Color.DarkSlateGray;

	private Color _ProgressColor = DefaultProgressColor;

	private int _Diameter = 24;

	private eTextPosition _TextPosition;

	private bool _TextVisible = true;

	private int _TextWidth;

	private Padding _TextPadding = new Padding(0, 0, 0, 0);

	private Color _TextColor = Color.Empty;

	private static readonly Color DefaultPieBorderDarkColor = Color.FromArgb(50, Color.Black);

	private Color _PieBorderDark = DefaultPieBorderDarkColor;

	private static readonly Color DefaultPieBorderLightColor = Color.FromArgb(255, Color.White);

	private Color _PieBorderLight = DefaultPieBorderLightColor;

	private static readonly Color DefaultSpokeBorderDarkColor = Color.FromArgb(48, Color.Black);

	private Color _SpokeBorderDark = DefaultSpokeBorderDarkColor;

	private static readonly Color DefaultSpokeBorderLightColor = Color.FromArgb(255, Color.White);

	private Color _SpokeBorderLight = DefaultSpokeBorderLightColor;

	private string _ProgressTextFormat = "{0}%";

	private int _AnimationSpeed = 100;

	private bool RenderesProgressText
	{
		get
		{
			if (_ProgressTextVisible)
			{
				if (_IsEndlessProgressBar)
				{
					return !string.IsNullOrEmpty(_ProgressText);
				}
				return true;
			}
			return false;
		}
	}

	[DefaultValue(eCircularProgressType.Line)]
	[Category("Appearance")]
	[Description("Indicates circular progress bar type.")]
	public eCircularProgressType ProgressBarType
	{
		get
		{
			return _ProgressBarType;
		}
		set
		{
			if (value != _ProgressBarType)
			{
				eCircularProgressType progressBarType = _ProgressBarType;
				_ProgressBarType = value;
				OnProgressBarTypeChanged(progressBarType, value);
			}
		}
	}

	[Description("Indicates maximum value of the progress bar.")]
	[Category("Behavior")]
	[DefaultValue(100)]
	public int Maximum
	{
		get
		{
			return _Maximum;
		}
		set
		{
			if (value != _Maximum)
			{
				int maximum = _Maximum;
				_Maximum = value;
				OnMaximumChanged(maximum, value);
			}
		}
	}

	[Description("Indicates minimum value of the progress bar.")]
	[Category("Behavior")]
	[DefaultValue(0)]
	public int Minimum
	{
		get
		{
			return _Minimum;
		}
		set
		{
			if (value != _Minimum)
			{
				int minimum = _Minimum;
				_Minimum = value;
				OnMinimumChanged(minimum, value);
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates color of progress percentage text")]
	public Color ProgressTextColor
	{
		get
		{
			return _ProgressTextColor;
		}
		set
		{
			_ProgressTextColor = value;
			Refresh();
		}
	}

	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Indicates whether text that displays the progress bar completion percentage text is visible")]
	public bool ProgressTextVisible
	{
		get
		{
			return _ProgressTextVisible;
		}
		set
		{
			if (value != _ProgressTextVisible)
			{
				bool progressTextVisible = _ProgressTextVisible;
				_ProgressTextVisible = value;
				OnProgressTextVisibleChanged(progressTextVisible, value);
			}
		}
	}

	[DefaultValue("")]
	[Category("Appearance")]
	[Description("Indicates text displayed on top of the circular progress bar.")]
	public string ProgressText
	{
		get
		{
			return _ProgressText;
		}
		set
		{
			if (value != _ProgressText)
			{
				string progressText = _ProgressText;
				_ProgressText = value;
				OnProgressTextChanged(progressText, value);
			}
		}
	}

	[Description("Indicates current value of the progress bar.")]
	[Category("Behavior")]
	[DefaultValue(0)]
	public int Value
	{
		get
		{
			return _Value;
		}
		set
		{
			value = Math.Min(_Maximum, Math.Max(value, _Minimum));
			if (value != _Value)
			{
				int value2 = _Value;
				_Value = value;
				OnValueChanged(value2, value);
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(false)]
	public bool IsRunning
	{
		get
		{
			return _IsRunning;
		}
		set
		{
			if (_IsRunning != value)
			{
				if (value)
				{
					Start();
				}
				else
				{
					Stop();
				}
			}
		}
	}

	[Category("Columns")]
	[Description("Indicates color of progress indicator.")]
	public Color ProgressColor
	{
		get
		{
			return _ProgressColor;
		}
		set
		{
			_ProgressColor = value;
			Refresh();
		}
	}

	[DefaultValue(24)]
	[Category("Appearance")]
	[Description("Indicates circular progress indicator diameter in pixels.")]
	public int Diameter
	{
		get
		{
			return _Diameter;
		}
		set
		{
			if (value != _Diameter)
			{
				int diameter = _Diameter;
				_Diameter = value;
				OnDiameterChanged(diameter, value);
			}
		}
	}

	[DefaultValue(eTextPosition.Left)]
	[Category("Appearance")]
	[Description("Indicatesd text position in relation to the circular progress indicator.")]
	public eTextPosition TextPosition
	{
		get
		{
			return _TextPosition;
		}
		set
		{
			if (value != _TextPosition)
			{
				eTextPosition textPosition = _TextPosition;
				_TextPosition = value;
				OnTextPositionChanged(textPosition, value);
			}
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether caption/label set using Text property is visible.")]
	public bool TextVisible
	{
		get
		{
			return _TextVisible;
		}
		set
		{
			if (value != _TextVisible)
			{
				bool textVisible = _TextVisible;
				_TextVisible = value;
				OnTextVisibleChanged(textVisible, value);
			}
		}
	}

	[DefaultValue(0)]
	[Category("Appearance")]
	[Description("Indicates suggested text-width. If you want to make sure that text you set wraps over multiple lines you can set suggested text-width so word break is performed.")]
	public int TextWidth
	{
		get
		{
			return _TextWidth;
		}
		set
		{
			if (value != _TextWidth)
			{
				int textWidth = _TextWidth;
				_TextWidth = value;
				OnTextWidthChanged(textWidth, value);
			}
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Gets or sets text padding.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public Padding TextPadding => _TextPadding;

	[Category("Columns")]
	[Description("Indicates color of text label.")]
	public Color TextColor
	{
		get
		{
			return _TextColor;
		}
		set
		{
			_TextColor = value;
			Refresh();
		}
	}

	[Category("Pie")]
	[Description("Indicates color of pie progress bar dark border.")]
	public Color PieBorderDark
	{
		get
		{
			return _PieBorderDark;
		}
		set
		{
			_PieBorderDark = value;
			Refresh();
		}
	}

	[Category("Pie")]
	[Description("Indicates color of pie progress bar light border. ")]
	public Color PieBorderLight
	{
		get
		{
			return _PieBorderLight;
		}
		set
		{
			_PieBorderLight = value;
			Refresh();
		}
	}

	[Category("Spoke")]
	[Description("Indicates color of spoke progress bar dark border.")]
	public Color SpokeBorderDark
	{
		get
		{
			return _SpokeBorderDark;
		}
		set
		{
			_SpokeBorderDark = value;
			Refresh();
		}
	}

	[Category("Spoke")]
	[Description("Indicates color of spoke progress bar light border..")]
	public Color SpokeBorderLight
	{
		get
		{
			return _SpokeBorderLight;
		}
		set
		{
			_SpokeBorderLight = value;
			Refresh();
		}
	}

	[DefaultValue("{0}%")]
	[Category("Appearance")]
	[Description("Indicates format string for progress value.")]
	public string ProgressTextFormat
	{
		get
		{
			return _ProgressTextFormat;
		}
		set
		{
			if (value != _ProgressTextFormat)
			{
				string progressTextFormat = _ProgressTextFormat;
				_ProgressTextFormat = value;
				OnProgressTextFormatChanged(progressTextFormat, value);
			}
		}
	}

	[DefaultValue(100)]
	[Description("Indicates the animation speed for endless running progress. Lower number means faster running.")]
	[Category("Behavior")]
	public int AnimationSpeed
	{
		get
		{
			return _AnimationSpeed;
		}
		set
		{
			if (value < 1)
			{
				value = 1;
			}
			_AnimationSpeed = value;
		}
	}

	public event EventHandler ValueChanged;

	public CircularProgressItem()
		: this("", "")
	{
	}

	public CircularProgressItem(string sItemName)
		: this(sItemName, "")
	{
	}

	public CircularProgressItem(string sItemName, string ItemText)
		: base(sItemName, ItemText)
	{
		_SpokeAngles = GetSpokeAngles(_SpokeCount);
	}

	public override BaseItem Copy()
	{
		CircularProgressItem circularProgressItem = new CircularProgressItem(m_Name);
		CopyToItem(circularProgressItem);
		return circularProgressItem;
	}

	internal void InternalCopyToItem(ProgressBarItem copy)
	{
		CopyToItem(copy);
	}

	protected override void CopyToItem(BaseItem copy)
	{
		CircularProgressItem objCopy = copy as CircularProgressItem;
		base.CopyToItem(objCopy);
	}

	protected override void Dispose(bool disposing)
	{
		Stop();
		base.Dispose(disposing);
	}

	public override void Paint(ItemPaintArgs e)
	{
		if (_ProgressBarType == eCircularProgressType.Line)
		{
			PaintLineProgressBar(e);
		}
		else if (_ProgressBarType == eCircularProgressType.Dot)
		{
			PaintDotProgressBar(e);
		}
		else if (_ProgressBarType == eCircularProgressType.Donut)
		{
			PaintDonutProgressBar(e);
		}
		else if (_ProgressBarType == eCircularProgressType.Dashboard)
		{
			PaintDashboardProgressBar(e);
		}
		else if (_ProgressBarType == eCircularProgressType.Spoke)
		{
			PaintSpokeProgressBar(e);
		}
		else if (_ProgressBarType == eCircularProgressType.Pie)
		{
			PaintPieProgressBar(e);
		}
		PaintLabel(e);
		if (Focused && DesignMode)
		{
			Rectangle displayRectangle = DisplayRectangle;
			displayRectangle.Inflate(-1, -1);
			DesignTime.DrawDesignTimeSelection(e.Graphics, displayRectangle, e.Colors.ItemDesignTimeBorder);
		}
		DrawInsertMarker(e.Graphics);
	}

	private void PaintLabel(ItemPaintArgs e)
	{
		if (_TextVisible && !string.IsNullOrEmpty(Text))
		{
			Font font = e.Font;
			Graphics graphics = e.Graphics;
			Color textColor = GetTextColor(e);
			Rectangle bounds = Rectangle.Empty;
			Rectangle rect = m_Rect;
			Rectangle progressBarBounds = GetProgressBarBounds();
			eTextFormat eTextFormat2 = eTextFormat.NoClipping;
			if (_TextPosition == eTextPosition.Left)
			{
				bounds = new Rectangle(rect.X + _TextPadding.Left, rect.Y + _TextPadding.Top, m_Rect.Width - _TextPadding.Horizontal - TextContentSpacing - progressBarBounds.Width, m_Rect.Height - _TextPadding.Vertical);
				eTextFormat2 |= eTextFormat.VerticalCenter;
			}
			else if (_TextPosition == eTextPosition.Right)
			{
				bounds = new Rectangle(rect.X + _TextPadding.Left + TextContentSpacing + progressBarBounds.Width, rect.Y + _TextPadding.Top, m_Rect.Width - _TextPadding.Horizontal - TextContentSpacing - progressBarBounds.Width, m_Rect.Height - _TextPadding.Vertical);
				eTextFormat2 |= eTextFormat.VerticalCenter;
			}
			else if (_TextPosition == eTextPosition.Top)
			{
				bounds = new Rectangle(rect.X + _TextPadding.Left, rect.Y + _TextPadding.Top, m_Rect.Width - _TextPadding.Horizontal, m_Rect.Height - _TextPadding.Vertical - progressBarBounds.Height - TextContentSpacing);
				eTextFormat2 |= eTextFormat.HorizontalCenter;
			}
			else if (_TextPosition == eTextPosition.Bottom)
			{
				bounds = new Rectangle(rect.X + _TextPadding.Left, rect.Y + _TextPadding.Top + TextContentSpacing + progressBarBounds.Height, m_Rect.Width - _TextPadding.Horizontal, m_Rect.Height - _TextPadding.Vertical - progressBarBounds.Height - TextContentSpacing);
				eTextFormat2 |= eTextFormat.HorizontalCenter;
			}
			if (_TextWidth > 0)
			{
				bounds.Width = _TextWidth;
				eTextFormat2 |= eTextFormat.WordBreak;
			}
			TextDrawing.DrawString(graphics, Text, font, textColor, bounds, eTextFormat2);
		}
	}

	private Color GetTextColor(ItemPaintArgs e)
	{
		if (!_TextColor.IsEmpty)
		{
			return _TextColor;
		}
		return LabelItem.GetTextColor(e, EffectiveStyle, GetEnabled(), _TextColor);
	}

	private void PaintPieProgressBar(ItemPaintArgs e)
	{
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Expected O, but got Unknown
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Expected O, but got Unknown
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Expected O, but got Unknown
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		Rectangle progressBarBounds = GetProgressBarBounds();
		PointF pointF = new PointF(progressBarBounds.X + progressBarBounds.Width / 2, progressBarBounds.Y + progressBarBounds.Height / 2);
		float num = (float)Math.Max(1.5, (double)progressBarBounds.Height * 0.2);
		progressBarBounds.Inflate(-1, -1);
		progressBarBounds.Width--;
		progressBarBounds.Height--;
		float num2 = 1f;
		if (progressBarBounds.Height > 31)
		{
			num2 = (float)progressBarBounds.Height * 0.05f;
		}
		if (!_IsEndlessProgressBar)
		{
			int value = GetValue();
			Rectangle rectangle = progressBarBounds;
			rectangle.Inflate(-(int)num2, -(int)num2);
			int num3 = (int)(360.0 * ((double)value / (double)Math.Max(1, _Maximum - _Minimum)));
			SolidBrush val = new SolidBrush(_ProgressColor);
			try
			{
				graphics.FillPie((Brush)(object)val, rectangle, 270f, (float)num3);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		else
		{
			Rectangle rectangle2 = progressBarBounds;
			rectangle2.Inflate(-(int)num2, -(int)num2);
			int num4 = (int)((double)(360 * _EndlessProgressValue) / (double)_SpokeCount);
			int num5 = 90;
			SolidBrush val2 = new SolidBrush(_ProgressColor);
			try
			{
				graphics.FillPie((Brush)(object)val2, rectangle2, (float)num4, (float)num5);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		Rectangle rectangle3 = progressBarBounds;
		rectangle3.Width--;
		rectangle3.Height--;
		rectangle3.Offset(1, 1);
		Pen val3 = new Pen(_PieBorderDark, num2);
		try
		{
			val3.Alignment = (PenAlignment)1;
			graphics.DrawEllipse(val3, rectangle3);
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
		rectangle3.Offset(-1, -1);
		Pen val4 = new Pen(_PieBorderLight, num2 + 0.5f);
		try
		{
			val4.Alignment = (PenAlignment)1;
			graphics.DrawEllipse(val4, rectangle3);
			graphics.DrawEllipse(val4, rectangle3);
		}
		finally
		{
			((IDisposable)val4)?.Dispose();
		}
		if (RenderesProgressText)
		{
			progressBarBounds.Offset(1, 0);
			PaintProgressText(graphics, progressBarBounds, (int)((float)progressBarBounds.Height * 0.4f), e.Font);
		}
	}

	private void PaintSpokeProgressBar(ItemPaintArgs e)
	{
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Expected O, but got Unknown
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Expected O, but got Unknown
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Expected O, but got Unknown
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Expected O, but got Unknown
		//IL_0461: Unknown result type (might be due to invalid IL or missing references)
		//IL_0468: Expected O, but got Unknown
		//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b4: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		Rectangle progressBarBounds = GetProgressBarBounds();
		progressBarBounds.Width -= 2;
		progressBarBounds.Height -= 2;
		PointF pointF = new PointF(progressBarBounds.X + progressBarBounds.Width / 2, progressBarBounds.Y + progressBarBounds.Height / 2);
		float num = (float)Math.Max(1.5, (double)progressBarBounds.Height * 0.2);
		GraphicsPath val = new GraphicsPath();
		Rectangle rectangle = progressBarBounds;
		rectangle.Inflate(-(int)((float)progressBarBounds.Height / 3f), -(int)((float)progressBarBounds.Height / 3f));
		val.AddEllipse(rectangle);
		Region clip = graphics.Clip;
		graphics.SetClip(val, (CombineMode)4);
		if (!_IsEndlessProgressBar)
		{
			int value = GetValue();
			int num2 = (int)(360.0 * ((double)value / (double)Math.Max(1, _Maximum - _Minimum)));
			SolidBrush val2 = new SolidBrush(_ProgressColor);
			try
			{
				graphics.FillPie((Brush)(object)val2, progressBarBounds, 270f, (float)num2);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		else
		{
			int num3 = (int)((double)(360 * _EndlessProgressValue) / (double)_SpokeCount);
			int num4 = 90;
			SolidBrush val3 = new SolidBrush(_ProgressColor);
			try
			{
				graphics.FillPie((Brush)(object)val3, progressBarBounds, (float)num3, (float)num4);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		int num5 = progressBarBounds.Width / 2;
		PointF centerPoint = pointF;
		float num6 = 1f;
		float num7 = 1f;
		float num8 = 1f;
		num5--;
		Pen val4 = new Pen(_SpokeBorderDark, num6);
		try
		{
			val4.Alignment = (PenAlignment)4;
			PointF coordinate = GetCoordinate(pointF, num5, 315.0);
			coordinate.X++;
			PointF coordinate2 = GetCoordinate(centerPoint, num5, 135.0);
			coordinate2.X++;
			graphics.DrawLine(val4, coordinate, coordinate2);
			graphics.DrawLine(val4, coordinate, coordinate2);
			coordinate = GetCoordinate(pointF, num5, 270.0);
			coordinate.X++;
			coordinate2 = GetCoordinate(centerPoint, num5, 90.0);
			coordinate2.X++;
			graphics.DrawLine(val4, coordinate, coordinate2);
			coordinate = GetCoordinate(pointF, num5, 225.0);
			coordinate.Y++;
			coordinate2 = GetCoordinate(centerPoint, num5, 45.0);
			coordinate2.Y++;
			graphics.DrawLine(val4, coordinate, coordinate2);
			coordinate = GetCoordinate(pointF, num5, 180.0);
			coordinate.Y++;
			coordinate2 = GetCoordinate(centerPoint, num5, 0.0);
			coordinate2.Y++;
			graphics.DrawLine(val4, coordinate, coordinate2);
		}
		finally
		{
			((IDisposable)val4)?.Dispose();
		}
		Pen val5 = new Pen(_SpokeBorderDark, num8);
		try
		{
			val5.Alignment = (PenAlignment)1;
			Rectangle rectangle2 = progressBarBounds;
			rectangle2.Offset(1, 1);
			graphics.DrawEllipse(val5, rectangle2);
		}
		finally
		{
			((IDisposable)val5)?.Dispose();
		}
		Pen val6 = new Pen(_SpokeBorderLight, num6);
		try
		{
			graphics.DrawLine(val6, GetCoordinate(pointF, num5, 315.0), GetCoordinate(pointF, num5, 135.0));
			graphics.DrawLine(val6, GetCoordinate(pointF, num5, 270.0), GetCoordinate(pointF, num5, 90.0));
			graphics.DrawLine(val6, GetCoordinate(pointF, num5, 225.0), GetCoordinate(pointF, num5, 45.0));
			graphics.DrawLine(val6, GetCoordinate(pointF, num5, 180.0), GetCoordinate(pointF, num5, 0.0));
		}
		finally
		{
			((IDisposable)val6)?.Dispose();
		}
		Pen val7 = new Pen(_SpokeBorderLight, num7);
		try
		{
			val7.Alignment = (PenAlignment)1;
			graphics.DrawEllipse(val7, progressBarBounds);
			graphics.DrawEllipse(val7, progressBarBounds);
		}
		finally
		{
			((IDisposable)val7)?.Dispose();
		}
		graphics.Clip = clip;
		clip.Dispose();
		float num9 = 1f;
		Pen val8 = new Pen(Color.White, num9);
		try
		{
			val8.Alignment = (PenAlignment)1;
			graphics.DrawEllipse(val8, rectangle);
			graphics.DrawEllipse(val8, rectangle);
		}
		finally
		{
			((IDisposable)val8)?.Dispose();
		}
		if (RenderesProgressText)
		{
			PaintProgressText(graphics, progressBarBounds, (int)((float)progressBarBounds.Height * 0.35f), e.Font);
		}
	}

	private int GetValue()
	{
		int num = Math.Min(_Maximum, Math.Max(_Minimum, _Value));
		if (DesignMode && num == _Minimum)
		{
			num = (int)((double)_Maximum * 0.75);
		}
		return num;
	}

	private void PaintDashboardProgressBar(ItemPaintArgs e)
	{
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Expected O, but got Unknown
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Expected O, but got Unknown
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Expected O, but got Unknown
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		Graphics graphics = e.Graphics;
		RectangleF rectangleF = GetProgressBarBounds();
		PointF pointF = new PointF(rectangleF.X + rectangleF.Width / 2f, rectangleF.Y + rectangleF.Height / 2f);
		float num = (float)Math.Max(1.5, (double)rectangleF.Height * 0.2);
		rectangleF.Inflate(0f - Math.Max(1f, num / 2f), 0f - Math.Max(1f, num / 2f));
		rectangleF.Width--;
		rectangleF.Height--;
		if (!_IsEndlessProgressBar)
		{
			int value = GetValue();
			int num2 = (int)(360.0 * ((double)value / (double)Math.Max(1, _Maximum - _Minimum)));
			Pen val = new Pen(_ProgressColor, num);
			try
			{
				graphics.DrawArc(val, rectangleF, 270f, (float)num2);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		else
		{
			float num3 = Math.Min(rectangleF.Width, rectangleF.Height);
			if (num3 <= 0f)
			{
				return;
			}
			if (num3 > 80f)
			{
				num3 = 80f;
			}
			int num4 = (int)((double)num3 * 0.1);
			RectangleF rectangleF2 = default(RectangleF);
			rectangleF2.X = rectangleF.X + (rectangleF.Width - num3) / 2f;
			rectangleF2.Y = rectangleF.Y + (rectangleF.Height - num3) / 2f;
			rectangleF2.Width = num3;
			rectangleF2.Height = num3;
			RectangleF rectangleF3 = rectangleF2;
			SmoothingMode smoothingMode = graphics.SmoothingMode;
			graphics.SmoothingMode = (SmoothingMode)4;
			GraphicsPath val2 = new GraphicsPath();
			try
			{
				int num5 = (int)((double)(360 * _EndlessProgressValue) / (double)_SpokeCount);
				val2.AddArc(rectangleF3, (float)num5, 140f);
				Pen val3 = new Pen(Color.FromArgb(230, 230, 230), (float)num4);
				try
				{
					graphics.DrawEllipse(val3, rectangleF3);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
				Pen val4 = new Pen(Color.FromArgb(200, 74, 125, 177), (float)num4);
				try
				{
					graphics.DrawPath(val4, val2);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			graphics.SmoothingMode = smoothingMode;
		}
		if (RenderesProgressText)
		{
			PaintProgressText(graphics, rectangleF, (int)(rectangleF.Height * 0.4f), e.Font);
		}
	}

	private void PaintDonutProgressBar(ItemPaintArgs e)
	{
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Expected O, but got Unknown
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		RectangleF rectangleF = GetProgressBarBounds();
		PointF pointF = new PointF(rectangleF.X + rectangleF.Width / 2f, rectangleF.Y + rectangleF.Height / 2f);
		float num = (float)Math.Max(1.5, (double)rectangleF.Height * 0.2);
		rectangleF.Inflate(0f - Math.Max(1f, num / 2f), 0f - Math.Max(1f, num / 2f));
		rectangleF.Width--;
		rectangleF.Height--;
		if (!_IsEndlessProgressBar)
		{
			int value = GetValue();
			int num2 = (int)(360.0 * ((double)value / (double)Math.Max(1, _Maximum - _Minimum)));
			Pen val = new Pen(_ProgressColor, num);
			try
			{
				graphics.DrawArc(val, rectangleF, 270f, (float)num2);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		else
		{
			int num3 = (int)((double)(360 * _EndlessProgressValue) / (double)_SpokeCount);
			int num4 = 140;
			Pen val2 = new Pen(_ProgressColor, num);
			try
			{
				val2.EndCap = (LineCap)2;
				val2.StartCap = (LineCap)2;
				graphics.DrawArc(val2, rectangleF, (float)num3, (float)num4);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		if (RenderesProgressText)
		{
			PaintProgressText(graphics, rectangleF, (int)(rectangleF.Height * 0.4f), e.Font);
		}
	}

	private void PaintDotProgressBar(ItemPaintArgs e)
	{
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		Rectangle progressBarBounds = GetProgressBarBounds();
		PointF centerPoint = new PointF(progressBarBounds.X + progressBarBounds.Width / 2, progressBarBounds.Y + progressBarBounds.Height / 2);
		int num = 14;
		int num2 = 2;
		num = (int)Math.Round((double)progressBarBounds.Width * 0.4);
		num2 = Math.Max(1, (int)Math.Round((double)num * 0.25));
		int value = GetValue();
		if (!_IsEndlessProgressBar)
		{
			int num3 = (int)Math.Round((double)_SpokeCount * ((double)value / (double)Math.Max(1, _Maximum - _Minimum)));
			for (int i = 0; i < num3; i++)
			{
				PointF coordinate = GetCoordinate(centerPoint, num, _SpokeAngles[i]);
				RectangleF rectangleF = new RectangleF(coordinate, Size.Empty);
				rectangleF.Inflate(num2, num2);
				SolidBrush val = new SolidBrush(ColorFromSpokeIndex(i));
				try
				{
					graphics.FillEllipse((Brush)(object)val, rectangleF);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
		}
		else if (_IsRunning)
		{
			int num4 = _EndlessProgressValue;
			for (int j = 0; j < _SpokeCount; j++)
			{
				num4 %= _SpokeCount;
				PointF coordinate2 = GetCoordinate(centerPoint, num, _SpokeAngles[num4]);
				RectangleF rectangleF2 = new RectangleF(coordinate2, Size.Empty);
				rectangleF2.Inflate(num2, num2);
				SolidBrush val2 = new SolidBrush(ColorFromSpokeIndex(j));
				try
				{
					graphics.FillEllipse((Brush)(object)val2, rectangleF2);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
				num4++;
			}
		}
		if (RenderesProgressText)
		{
			PaintProgressText(graphics, progressBarBounds, (int)((float)progressBarBounds.Height * 0.35f), e.Font);
		}
	}

	private void PaintLineProgressBar(ItemPaintArgs e)
	{
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Expected O, but got Unknown
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		Rectangle progressBarBounds = GetProgressBarBounds();
		PointF centerPoint = new PointF(progressBarBounds.X + progressBarBounds.Width / 2, progressBarBounds.Y + progressBarBounds.Height / 2);
		int num = 6;
		int num2 = 14;
		int num3 = 2;
		num2 = (int)Math.Round((double)progressBarBounds.Width * 0.45);
		num = (int)Math.Round((double)num2 * 0.45);
		num3 = Math.Max(2, (int)Math.Round((double)num2 * 0.15));
		int value = GetValue();
		if (!_IsEndlessProgressBar)
		{
			int num4 = (int)Math.Round((double)_SpokeCount * ((double)value / (double)Math.Max(1, _Maximum - _Minimum)));
			for (int i = 0; i < num4; i++)
			{
				PointF coordinate = GetCoordinate(centerPoint, num, _SpokeAngles[i]);
				PointF coordinate2 = GetCoordinate(centerPoint, num2, _SpokeAngles[i]);
				Pen val = new Pen(ColorFromSpokeIndex(i), (float)num3);
				try
				{
					val.EndCap = (LineCap)2;
					val.StartCap = (LineCap)2;
					graphics.DrawLine(val, coordinate, coordinate2);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
		}
		else if (_IsRunning)
		{
			int num5 = _EndlessProgressValue;
			for (int j = 0; j < _SpokeCount; j++)
			{
				num5 %= _SpokeCount;
				PointF coordinate3 = GetCoordinate(centerPoint, num, _SpokeAngles[num5]);
				PointF coordinate4 = GetCoordinate(centerPoint, num2, _SpokeAngles[num5]);
				Pen val2 = new Pen(ColorFromSpokeIndex(j), (float)num3);
				try
				{
					val2.EndCap = (LineCap)2;
					val2.StartCap = (LineCap)2;
					graphics.DrawLine(val2, coordinate3, coordinate4);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
				num5++;
			}
		}
		if (RenderesProgressText)
		{
			PaintProgressText(graphics, progressBarBounds, num, e.Font);
		}
	}

	private string GetProgressValueText()
	{
		try
		{
			return string.Format(_ProgressTextFormat, _Value);
		}
		catch
		{
			return "Format Error";
		}
	}

	private void PaintProgressText(Graphics g, RectangleF bounds, int innerRadius, Font baseFont)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		eTextFormat format = eTextFormat.HorizontalCenter | eTextFormat.NoClipping | eTextFormat.NoPadding | eTextFormat.SingleLine | eTextFormat.VerticalCenter;
		float num = Math.Max(1f, (float)innerRadius / 1.8f);
		Color color = (_ProgressTextColor.IsEmpty ? _ProgressColor : _ProgressTextColor);
		Font val = new Font(baseFont.FontFamily, num, (FontStyle)0);
		try
		{
			TextDrawing.DrawString(g, GetProgressValueText(), val, color, Rectangle.Round(bounds), format);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private Color ColorFromSpokeIndex(int spokeIndex, int spokeCount)
	{
		return Color.FromArgb((int)(210.0 * (double)spokeIndex / (double)spokeCount) + 45, _ProgressColor);
	}

	private Color ColorFromSpokeIndex(int spokeIndex)
	{
		return ColorFromSpokeIndex(spokeIndex, _SpokeCount);
	}

	private PointF GetCoordinate(PointF centerPoint, int radius, double angle)
	{
		double num = Math.PI * angle / 180.0;
		return new PointF(centerPoint.X + (float)radius * (float)Math.Cos(num), centerPoint.Y + (float)radius * (float)Math.Sin(num));
	}

	public override void RecalcSize()
	{
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Invalid comparison between Unknown and I4
		Rectangle rect = new Rectangle(m_Rect.X, m_Rect.Y, _Diameter, _Diameter);
		if (_TextVisible && !string.IsNullOrEmpty(Text))
		{
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val != null)
			{
				Font font = val.Font;
				Graphics val2 = val.CreateGraphics();
				try
				{
					Size size = (_TextSize = ButtonItemLayout.MeasureItemText(this, val2, _TextWidth, font, (_TextWidth > 0) ? eTextFormat.WordBreak : eTextFormat.SingleLine, (int)val.RightToLeft == 1));
					size.Width += _TextPadding.Horizontal;
					size.Height += _TextPadding.Vertical;
					if (_TextPosition == eTextPosition.Left || _TextPosition == eTextPosition.Right)
					{
						size.Width += TextContentSpacing;
						rect.Width += size.Width;
						rect.Height = Math.Max(rect.Height, size.Height);
					}
					else
					{
						size.Height += TextContentSpacing;
						rect.Height += size.Height;
						rect.Width = Math.Max(rect.Width, size.Width);
					}
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
		}
		m_Rect = rect;
		base.RecalcSize();
	}

	private Rectangle GetProgressBarBounds()
	{
		if (string.IsNullOrEmpty(Text) || !_TextVisible)
		{
			return new Rectangle(m_Rect.X + (m_Rect.Width - _Diameter) / 2, m_Rect.Y + (m_Rect.Height - _Diameter) / 2, _Diameter, _Diameter);
		}
		if (_TextPosition == eTextPosition.Top)
		{
			return new Rectangle(m_Rect.X + (m_Rect.Width - _Diameter) / 2, m_Rect.Y + (m_Rect.Height - _Diameter), _Diameter, _Diameter);
		}
		if (_TextPosition == eTextPosition.Right)
		{
			return new Rectangle(m_Rect.X, m_Rect.Y + (m_Rect.Height - _Diameter) / 2, _Diameter, _Diameter);
		}
		if (_TextPosition == eTextPosition.Left)
		{
			return new Rectangle(m_Rect.Right - _Diameter, m_Rect.Y + (m_Rect.Height - _Diameter) / 2, _Diameter, _Diameter);
		}
		if (_TextPosition == eTextPosition.Bottom)
		{
			return new Rectangle(m_Rect.X + (m_Rect.Width - _Diameter) / 2, m_Rect.Y, _Diameter, _Diameter);
		}
		return new Rectangle(m_Rect.X, m_Rect.Y, _Diameter, _Diameter);
	}

	private double[] GetSpokeAngles(int numberOfSpokes)
	{
		double[] array = new double[numberOfSpokes];
		double num = 360.0 / (double)numberOfSpokes;
		for (int i = 0; i < numberOfSpokes; i++)
		{
			array[i] = ((i == 0) ? (270.0 + num) : (array[i - 1] + num));
		}
		return array;
	}

	private void OnProgressBarTypeChanged(eCircularProgressType oldValue, eCircularProgressType newValue)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("ProgressBarType"));
		Refresh();
	}

	private void OnMaximumChanged(int oldValue, int newValue)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("Maximum"));
		CoerceValue();
	}

	private void OnMinimumChanged(int oldValue, int newValue)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("Minimum"));
		CoerceValue();
	}

	private void CoerceValue()
	{
		int value = _Value;
		if (_Value < _Minimum)
		{
			value = _Minimum;
		}
		else if (_Value > _Maximum)
		{
			value = _Maximum;
		}
		Value = value;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeProgressTextColor()
	{
		return !_ProgressTextColor.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetProgressTextColor()
	{
		ProgressTextColor = Color.Empty;
	}

	private void OnProgressTextVisibleChanged(bool oldValue, bool newValue)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("ProgressTextVisible"));
		Refresh();
	}

	private void OnProgressTextChanged(string oldValue, string newValue)
	{
		Refresh();
	}

	private void OnValueChanged(int oldValue, int newValue)
	{
		if (!_IsEndlessProgressBar)
		{
			Refresh();
		}
		OnValueChanged(EventArgs.Empty);
		OnPropertyChanged(new PropertyChangedEventArgs("Value"));
	}

	protected virtual void OnValueChanged(EventArgs e)
	{
		this.ValueChanged?.Invoke(this, e);
	}

	protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
	{
	}

	private void MoveEndlessProgressBar()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		if (!IsRunning || base.IsDisposed)
		{
			return;
		}
		if (base.InvokeRequired)
		{
			Invoke((Delegate)(MethodInvoker)delegate
			{
				MoveEndlessProgressBar();
			});
			return;
		}
		_EndlessProgressValue = ++_EndlessProgressValue % _SpokeCount;
		Refresh();
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val != null)
		{
			val.Update();
		}
	}

	public void Start()
	{
		if (!_IsRunning)
		{
			_IsEndlessProgressBar = true;
			_IsRunning = true;
			_LoopWorker = new BackgroundWorker();
			_LoopWorker.WorkerSupportsCancellation = true;
			_LoopWorker.DoWork += LoopWorkerDoWork;
			_LoopWorker.RunWorkerAsync();
		}
	}

	private void LoopWorkerDoWork(object sender, DoWorkEventArgs e)
	{
		BackgroundWorker backgroundWorker = (BackgroundWorker)sender;
		while (!backgroundWorker.CancellationPending)
		{
			MoveEndlessProgressBar();
			Thread.Sleep(_AnimationSpeed);
		}
	}

	public void Stop()
	{
		if (_IsRunning)
		{
			_IsEndlessProgressBar = false;
			_IsRunning = false;
			BackgroundWorker loopWorker = _LoopWorker;
			_LoopWorker = null;
			loopWorker.CancelAsync();
			loopWorker.DoWork -= LoopWorkerDoWork;
			loopWorker.Dispose();
			Refresh();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeProgressColor()
	{
		return _ProgressColor != DefaultProgressColor;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetProgressColor()
	{
		ProgressColor = DefaultProgressColor;
	}

	private void OnDiameterChanged(int oldValue, int newValue)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("Diameter"));
		NeedRecalcSize = true;
		Refresh();
	}

	private void OnTextPositionChanged(eTextPosition oldValue, eTextPosition newValue)
	{
		NeedRecalcSize = true;
		Refresh();
		OnPropertyChanged(new PropertyChangedEventArgs("TextPosition"));
	}

	private void OnTextVisibleChanged(bool oldValue, bool newValue)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("TextVisible"));
		NeedRecalcSize = true;
		Refresh();
	}

	private void OnTextWidthChanged(int oldValue, int newValue)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("TextWidth"));
		NeedRecalcSize = true;
		Refresh();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTextPadding()
	{
		if (_TextPadding.Bottom == 0 && _TextPadding.Top == 0 && _TextPadding.Left == 0)
		{
			return _TextPadding.Right != 0;
		}
		return true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTextPadding()
	{
		_TextPadding = new Padding(0, 0, 0, 0);
	}

	private void TextPaddingPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		NeedRecalcSize = true;
		Refresh();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTextColor()
	{
		return !_TextColor.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTextColor()
	{
		TextColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializePieBorderDark()
	{
		return _PieBorderDark != DefaultPieBorderDarkColor;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetPieBorderDark()
	{
		PieBorderDark = DefaultPieBorderDarkColor;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializePieBorderLight()
	{
		return _PieBorderLight != DefaultPieBorderLightColor;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetPieBorderLight()
	{
		PieBorderLight = DefaultPieBorderLightColor;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSpokeBorderDark()
	{
		return _SpokeBorderDark != DefaultSpokeBorderDarkColor;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSpokeBorderDark()
	{
		SpokeBorderDark = DefaultSpokeBorderDarkColor;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSpokeBorderLight()
	{
		return _SpokeBorderLight != DefaultSpokeBorderLightColor;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSpokeBorderLight()
	{
		SpokeBorderLight = DefaultSpokeBorderLightColor;
	}

	protected virtual void OnProgressTextFormatChanged(string oldValue, string newValue)
	{
		Refresh();
		OnPropertyChanged(new PropertyChangedEventArgs("ProgressTextFormat"));
	}
}
