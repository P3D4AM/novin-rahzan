using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;

namespace DevComponents.DotNetBar;

[ToolboxItem(false)]
[Designer("DevComponents.DotNetBar.Design.SimpleItemDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
public class CircularProgressItem : BaseItem
{
	private bool __237D;

	private int __237E = 12;

	private int __237F;

	internal static readonly int _2380 = 3;

	private Size _ᢧ = Size.Empty;

	private Rectangle __2381 = Rectangle.Empty;

	private double[] __2382;

	private eCircularProgressType __2383;

	private int _ᗵ = 100;

	private int _ᗶ;

	private Color __2384 = Color.Empty;

	private bool __2385;

	private string __2386 = "";

	private int _ᗸ;

	private EventHandler ᗲ;

	private bool __2387;

	private BackgroundWorker __2388;

	private bool __2389;

	private static readonly Color _238A = Color.DarkSlateGray;

	private Color __238B = _238A;

	private int __19F2 = 24;

	private eTextPosition __238C;

	private bool _ᣅ = true;

	private int __238D;

	private Padding _ᢾ = new Padding(0, 0, 0, 0);

	private Color __0CD1 = Color.Empty;

	private static readonly Color _238E = Color.FromArgb(50, Color.Black);

	private Color __238F = _238E;

	private static readonly Color _2390 = Color.FromArgb(255, Color.White);

	private Color __2391 = _2390;

	private static readonly Color _2392 = Color.FromArgb(48, Color.Black);

	private Color __2393 = _2392;

	private static readonly Color _2394 = Color.FromArgb(255, Color.White);

	private Color __2395 = _2394;

	private string __2396 = "{0}%";

	private int __2397 = 100;

	private bool _237C
	{
		get
		{
			if (__2385)
			{
				if (__2387)
				{
					return !string.IsNullOrEmpty(__2386);
				}
				return true;
			}
			return false;
		}
	}

	[Description("Indicates circular progress bar type.")]
	[Category("Appearance")]
	[DefaultValue(eCircularProgressType.Line)]
	public eCircularProgressType ProgressBarType
	{
		get
		{
			return __2383;
		}
		set
		{
			if (value != __2383)
			{
				eCircularProgressType ك = __2383;
				__2383 = value;
				_23AD(ك, value);
			}
		}
	}

	[Description("Indicates maximum value of the progress bar.")]
	[DefaultValue(100)]
	[Category("Behavior")]
	public int Maximum
	{
		get
		{
			return _ᗵ;
		}
		set
		{
			if (value != _ᗵ)
			{
				int ᗵ = _ᗵ;
				_ᗵ = value;
				_23AE(ᗵ, value);
			}
		}
	}

	[Description("Indicates minimum value of the progress bar.")]
	[DefaultValue(0)]
	[Category("Behavior")]
	public int Minimum
	{
		get
		{
			return _ᗶ;
		}
		set
		{
			if (value != _ᗶ)
			{
				int ᗶ = _ᗶ;
				_ᗶ = value;
				_23AF(ᗶ, value);
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates color of progress percentage text")]
	public Color ProgressTextColor
	{
		get
		{
			return __2384;
		}
		set
		{
			__2384 = value;
			Refresh();
		}
	}

	[Description("Indicates whether text that displays the progress bar completion percentage text is visible")]
	[Category("Appearance")]
	[DefaultValue(false)]
	public bool ProgressTextVisible
	{
		get
		{
			return __2385;
		}
		set
		{
			if (value != __2385)
			{
				bool ك = __2385;
				__2385 = value;
				_23B1(ك, value);
			}
		}
	}

	[Description("Indicates text displayed on top of the circular progress bar.")]
	[DefaultValue("")]
	[Category("Appearance")]
	public string ProgressText
	{
		get
		{
			return __2386;
		}
		set
		{
			if (value != __2386)
			{
				string ك = __2386;
				__2386 = value;
				_23B2(ك, value);
			}
		}
	}

	[DefaultValue(0)]
	[Category("Behavior")]
	[Description("Indicates current value of the progress bar.")]
	public int Value
	{
		get
		{
			return _ᗸ;
		}
		set
		{
			value = Math.Min(_ᗵ, Math.Max(value, _ᗶ));
			if (value != _ᗸ)
			{
				int ᗸ = _ᗸ;
				_ᗸ = value;
				_23B3(ᗸ, value);
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(false)]
	public bool IsRunning
	{
		get
		{
			return __2389;
		}
		set
		{
			if (__2389 != value)
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

	[Description("Indicates color of progress indicator.")]
	[Category("Columns")]
	public Color ProgressColor
	{
		get
		{
			return __238B;
		}
		set
		{
			__238B = value;
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
			return __19F2;
		}
		set
		{
			if (value != __19F2)
			{
				int ك = __19F2;
				__19F2 = value;
				_23B6(ك, value);
			}
		}
	}

	[Category("Appearance")]
	[DefaultValue(eTextPosition.Left)]
	[Description("Indicatesd text position in relation to the circular progress indicator.")]
	public eTextPosition TextPosition
	{
		get
		{
			return __238C;
		}
		set
		{
			if (value != __238C)
			{
				eTextPosition ك = __238C;
				__238C = value;
				_23B7(ك, value);
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
			return _ᣅ;
		}
		set
		{
			if (value != _ᣅ)
			{
				bool ᣅ = _ᣅ;
				_ᣅ = value;
				_23B8(ᣅ, value);
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates suggested text-width. If you want to make sure that text you set wraps over multiple lines you can set suggested text-width so word break is performed.")]
	[DefaultValue(0)]
	public int TextWidth
	{
		get
		{
			return __238D;
		}
		set
		{
			if (value != __238D)
			{
				int ك = __238D;
				__238D = value;
				_23B9(ك, value);
			}
		}
	}

	[Description("Gets or sets text padding.")]
	[Browsable(true)]
	[Category("Appearance")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public Padding TextPadding => _ᢾ;

	[Category("Columns")]
	[Description("Indicates color of text label.")]
	public Color TextColor
	{
		get
		{
			return __0CD1;
		}
		set
		{
			__0CD1 = value;
			Refresh();
		}
	}

	[Category("Pie")]
	[Description("Indicates color of pie progress bar dark border.")]
	public Color PieBorderDark
	{
		get
		{
			return __238F;
		}
		set
		{
			__238F = value;
			Refresh();
		}
	}

	[Description("Indicates color of pie progress bar light border. ")]
	[Category("Pie")]
	public Color PieBorderLight
	{
		get
		{
			return __2391;
		}
		set
		{
			__2391 = value;
			Refresh();
		}
	}

	[Category("Spoke")]
	[Description("Indicates color of spoke progress bar dark border.")]
	public Color SpokeBorderDark
	{
		get
		{
			return __2393;
		}
		set
		{
			__2393 = value;
			Refresh();
		}
	}

	[Category("Spoke")]
	[Description("Indicates color of spoke progress bar light border..")]
	public Color SpokeBorderLight
	{
		get
		{
			return __2395;
		}
		set
		{
			__2395 = value;
			Refresh();
		}
	}

	[Category("Appearance")]
	[DefaultValue("{0}%")]
	[Description("Indicates format string for progress value.")]
	public string ProgressTextFormat
	{
		get
		{
			return __2396;
		}
		set
		{
			if (value != __2396)
			{
				string oldValue = __2396;
				__2396 = value;
				OnProgressTextFormatChanged(oldValue, value);
			}
		}
	}

	[DefaultValue(100)]
	[Category("Behavior")]
	[Description("Indicates the animation speed for endless running progress. Lower number means faster running.")]
	public int AnimationSpeed
	{
		get
		{
			return __2397;
		}
		set
		{
			if (value < 1)
			{
				value = 1;
			}
			__2397 = value;
		}
	}

	public event EventHandler ValueChanged
	{
		add
		{
			EventHandler eventHandler = ᗲ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᗲ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ᗲ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ᗲ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

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
		__2382 = _23AB(__237E);
	}

	public override BaseItem Copy()
	{
		CircularProgressItem circularProgressItem = new CircularProgressItem(m_Name);
		CopyToItem(circularProgressItem);
		return circularProgressItem;
	}

	internal void _0EBA(ProgressBarItem _0EBB)
	{
		CopyToItem(_0EBB);
	}

	protected override void CopyToItem(BaseItem copy)
	{
		CircularProgressItem objCopy = copy as CircularProgressItem;
		base.CopyToItem(objCopy);
	}

	protected override void Dispose(bool disposing)
	{
		__237D = true;
		Stop(disposing: true);
		base.Dispose(disposing);
		__237D = false;
	}

	public override void Paint(ItemPaintArgs e)
	{
		if (__2383 == eCircularProgressType.Line)
		{
			_239E(e);
		}
		else if (__2383 == eCircularProgressType.Dot)
		{
			_239D(e);
		}
		else if (__2383 == eCircularProgressType.Donut)
		{
			_239C(e);
		}
		else if (__2383 == eCircularProgressType.Spoke)
		{
			_239A(e);
		}
		else if (__2383 == eCircularProgressType.Pie)
		{
			_2399(e);
		}
		_2398(e);
		if (Focused && DesignMode)
		{
			Rectangle displayRectangle = DisplayRectangle;
			displayRectangle.Inflate(-1, -1);
			_1A6B._1A6C(e.Graphics, displayRectangle, e.Colors.ItemDesignTimeBorder);
		}
		DrawInsertMarker(e.Graphics);
	}

	private void _2398(ItemPaintArgs ؿ)
	{
		if (_ᣅ && !string.IsNullOrEmpty(Text))
		{
			Font font = ؿ.Font;
			Graphics graphics = ؿ.Graphics;
			Color color = _1933(ؿ);
			Rectangle bounds = Rectangle.Empty;
			Rectangle rect = m_Rect;
			Rectangle rectangle = _23AA();
			eTextFormat eTextFormat2 = eTextFormat.NoClipping;
			int num = Dpi.Width(_2380);
			if (__238C == eTextPosition.Left)
			{
				bounds = new Rectangle(rect.X + _ᢾ.Left, rect.Y + _ᢾ.Top, m_Rect.Width - _ᢾ.Horizontal - num - rectangle.Width, m_Rect.Height - _ᢾ.Vertical);
				eTextFormat2 |= eTextFormat.VerticalCenter;
			}
			else if (__238C == eTextPosition.Right)
			{
				bounds = new Rectangle(rect.X + _ᢾ.Left + num + rectangle.Width, rect.Y + _ᢾ.Top, m_Rect.Width - _ᢾ.Horizontal - num - rectangle.Width, m_Rect.Height - _ᢾ.Vertical);
				eTextFormat2 |= eTextFormat.VerticalCenter;
			}
			else if (__238C == eTextPosition.Top)
			{
				bounds = new Rectangle(rect.X + _ᢾ.Left, rect.Y + _ᢾ.Top, m_Rect.Width - _ᢾ.Horizontal, m_Rect.Height - _ᢾ.Vertical - rectangle.Height - num);
				eTextFormat2 |= eTextFormat.HorizontalCenter;
			}
			else if (__238C == eTextPosition.Bottom)
			{
				bounds = new Rectangle(rect.X + _ᢾ.Left, rect.Y + _ᢾ.Top + num + rectangle.Height, m_Rect.Width - _ᢾ.Horizontal, m_Rect.Height - _ᢾ.Vertical - rectangle.Height - num);
				eTextFormat2 |= eTextFormat.HorizontalCenter;
			}
			if (__238D > 0)
			{
				bounds.Width = __238D;
				eTextFormat2 |= eTextFormat.WordBreak;
			}
			TextDrawing.DrawString(graphics, Text, font, color, bounds, eTextFormat2);
		}
	}

	private Color _1933(ItemPaintArgs ؿ)
	{
		if (!__0CD1.IsEmpty)
		{
			return __0CD1;
		}
		return LabelItem._1933(ؿ, EffectiveStyle, GetEnabled(), __0CD1);
	}

	private void _2399(ItemPaintArgs ؿ)
	{
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Expected O, but got Unknown
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Expected O, but got Unknown
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Expected O, but got Unknown
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Expected O, but got Unknown
		Graphics graphics = ؿ.Graphics;
		Rectangle rectangle = _23AA();
		new PointF(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);
		Math.Max(1.5, (double)rectangle.Height * 0.2);
		rectangle.Inflate(-1, -1);
		rectangle.Width--;
		rectangle.Height--;
		float num = 1f;
		if (rectangle.Height > 31)
		{
			num = (float)rectangle.Height * 0.05f;
		}
		if (!__2387)
		{
			int num2 = _239B();
			Rectangle rectangle2 = rectangle;
			rectangle2.Inflate(-(int)num, -(int)num);
			int num3 = (int)(360.0 * ((double)num2 / (double)Math.Max(1, _ᗵ - _ᗶ)));
			SolidBrush val = new SolidBrush(__238B);
			try
			{
				graphics.FillPie((Brush)(object)val, rectangle2, 270f, (float)num3);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		else
		{
			Rectangle rectangle3 = rectangle;
			rectangle3.Inflate(-(int)num, -(int)num);
			int num4 = (int)((double)(360 * __237F) / (double)__237E);
			int num5 = 90;
			SolidBrush val2 = new SolidBrush(__238B);
			try
			{
				graphics.FillPie((Brush)(object)val2, rectangle3, (float)num4, (float)num5);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		Rectangle rectangle4 = rectangle;
		rectangle4.Width--;
		rectangle4.Height--;
		rectangle4.Offset(1, 1);
		Pen val3 = new Pen(__238F, num);
		try
		{
			val3.Alignment = (PenAlignment)1;
			graphics.DrawEllipse(val3, rectangle4);
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
		rectangle4.Offset(-1, -1);
		Pen val4 = new Pen(__2391, num + 0.5f);
		try
		{
			val4.Alignment = (PenAlignment)1;
			graphics.DrawEllipse(val4, rectangle4);
			graphics.DrawEllipse(val4, rectangle4);
		}
		finally
		{
			((IDisposable)val4)?.Dispose();
		}
		if (_237C)
		{
			rectangle.Offset(1, 0);
			_23A0(graphics, rectangle, (int)((float)rectangle.Height * 0.4f), ؿ.Font);
		}
	}

	private void _239A(ItemPaintArgs ؿ)
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Expected O, but got Unknown
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Expected O, but got Unknown
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_033c: Expected O, but got Unknown
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_0377: Expected O, but got Unknown
		//IL_043d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0444: Expected O, but got Unknown
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Expected O, but got Unknown
		Graphics graphics = ؿ.Graphics;
		Rectangle rectangle = _23AA();
		rectangle.Width -= 2;
		rectangle.Height -= 2;
		PointF pointF = new PointF(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);
		Math.Max(1.5, (double)rectangle.Height * 0.2);
		GraphicsPath val = new GraphicsPath();
		Rectangle rectangle2 = rectangle;
		rectangle2.Inflate(-(int)((float)rectangle.Height / 3f), -(int)((float)rectangle.Height / 3f));
		val.AddEllipse(rectangle2);
		Region clip = graphics.Clip;
		graphics.SetClip(val, (CombineMode)4);
		if (!__2387)
		{
			int num = _239B();
			int num2 = (int)(360.0 * ((double)num / (double)Math.Max(1, _ᗵ - _ᗶ)));
			SolidBrush val2 = new SolidBrush(__238B);
			try
			{
				graphics.FillPie((Brush)(object)val2, rectangle, 270f, (float)num2);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		else
		{
			int num3 = (int)((double)(360 * __237F) / (double)__237E);
			int num4 = 90;
			SolidBrush val3 = new SolidBrush(__238B);
			try
			{
				graphics.FillPie((Brush)(object)val3, rectangle, (float)num3, (float)num4);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		int num5 = rectangle.Width / 2;
		PointF _23A7 = pointF;
		float num6 = 1f;
		float num7 = 1f;
		float num8 = 1f;
		num5--;
		Pen val4 = new Pen(__2393, num6);
		try
		{
			val4.Alignment = (PenAlignment)4;
			PointF pointF2 = _23A6(pointF, num5, 315.0);
			pointF2.X += 1f;
			PointF pointF3 = _23A6(_23A7, num5, 135.0);
			pointF3.X += 1f;
			graphics.DrawLine(val4, pointF2, pointF3);
			graphics.DrawLine(val4, pointF2, pointF3);
			pointF2 = _23A6(pointF, num5, 270.0);
			pointF2.X += 1f;
			pointF3 = _23A6(_23A7, num5, 90.0);
			pointF3.X += 1f;
			graphics.DrawLine(val4, pointF2, pointF3);
			pointF2 = _23A6(pointF, num5, 225.0);
			pointF2.Y += 1f;
			pointF3 = _23A6(_23A7, num5, 45.0);
			pointF3.Y += 1f;
			graphics.DrawLine(val4, pointF2, pointF3);
			pointF2 = _23A6(pointF, num5, 180.0);
			pointF2.Y += 1f;
			pointF3 = _23A6(_23A7, num5, 0.0);
			pointF3.Y += 1f;
			graphics.DrawLine(val4, pointF2, pointF3);
		}
		finally
		{
			((IDisposable)val4)?.Dispose();
		}
		Pen val5 = new Pen(__2393, num8);
		try
		{
			val5.Alignment = (PenAlignment)1;
			Rectangle rectangle3 = rectangle;
			rectangle3.Offset(1, 1);
			graphics.DrawEllipse(val5, rectangle3);
		}
		finally
		{
			((IDisposable)val5)?.Dispose();
		}
		Pen val6 = new Pen(__2395, num6);
		try
		{
			graphics.DrawLine(val6, _23A6(pointF, num5, 315.0), _23A6(pointF, num5, 135.0));
			graphics.DrawLine(val6, _23A6(pointF, num5, 270.0), _23A6(pointF, num5, 90.0));
			graphics.DrawLine(val6, _23A6(pointF, num5, 225.0), _23A6(pointF, num5, 45.0));
			graphics.DrawLine(val6, _23A6(pointF, num5, 180.0), _23A6(pointF, num5, 0.0));
		}
		finally
		{
			((IDisposable)val6)?.Dispose();
		}
		Pen val7 = new Pen(__2395, num7);
		try
		{
			val7.Alignment = (PenAlignment)1;
			graphics.DrawEllipse(val7, rectangle);
			graphics.DrawEllipse(val7, rectangle);
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
			graphics.DrawEllipse(val8, rectangle2);
			graphics.DrawEllipse(val8, rectangle2);
		}
		finally
		{
			((IDisposable)val8)?.Dispose();
		}
		if (_237C)
		{
			_23A0(graphics, rectangle, (int)((float)rectangle.Height * 0.35f), ؿ.Font);
		}
	}

	private int _239B()
	{
		int num = Math.Min(_ᗵ, Math.Max(_ᗶ, _ᗸ));
		if (DesignMode && num == _ᗶ)
		{
			num = (int)((double)_ᗵ * 0.75);
		}
		return num;
	}

	private void _239C(ItemPaintArgs ؿ)
	{
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Expected O, but got Unknown
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Expected O, but got Unknown
		Graphics graphics = ؿ.Graphics;
		RectangleF rectangleF = _23AA();
		new PointF(rectangleF.X + rectangleF.Width / 2f, rectangleF.Y + rectangleF.Height / 2f);
		float num = (float)Math.Max(1.5, (double)rectangleF.Height * 0.2);
		rectangleF.Inflate(0f - Math.Max(1f, num / 2f), 0f - Math.Max(1f, num / 2f));
		rectangleF.Width -= 1f;
		rectangleF.Height -= 1f;
		if (!__2387)
		{
			int num2 = _239B();
			int num3 = (int)(360.0 * ((double)num2 / (double)Math.Max(1, _ᗵ - _ᗶ)));
			Pen val = new Pen(__238B, num);
			try
			{
				graphics.DrawArc(val, rectangleF, 270f, (float)num3);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		else
		{
			int num4 = (int)((double)(360 * __237F) / (double)__237E);
			int num5 = 140;
			Pen val2 = new Pen(__238B, num);
			try
			{
				val2.EndCap = (LineCap)2;
				val2.StartCap = (LineCap)2;
				graphics.DrawArc(val2, rectangleF, (float)num4, (float)num5);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		if (_237C)
		{
			_23A0(graphics, rectangleF, (int)(rectangleF.Height * 0.4f), ؿ.Font);
		}
	}

	private void _239D(ItemPaintArgs ؿ)
	{
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Expected O, but got Unknown
		Graphics graphics = ؿ.Graphics;
		Rectangle rectangle = _23AA();
		PointF _23A7 = new PointF(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);
		int num = 14;
		int num2 = 2;
		num = (int)Math.Round((double)rectangle.Width * 0.4);
		num2 = Math.Max(1, (int)Math.Round((double)num * 0.25));
		int num3 = _239B();
		if (!__2387)
		{
			int num4 = (int)Math.Round((double)__237E * ((double)num3 / (double)Math.Max(1, _ᗵ - _ᗶ)));
			for (int i = 0; i < num4; i++)
			{
				PointF location = _23A6(_23A7, num, __2382[i]);
				RectangleF rectangleF = new RectangleF(location, Size.Empty);
				rectangleF.Inflate(num2, num2);
				SolidBrush val = new SolidBrush(_23A3(i));
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
		else if (__2389)
		{
			int num5 = __237F;
			for (int j = 0; j < __237E; j++)
			{
				num5 %= __237E;
				PointF location2 = _23A6(_23A7, num, __2382[num5]);
				RectangleF rectangleF2 = new RectangleF(location2, Size.Empty);
				rectangleF2.Inflate(num2, num2);
				SolidBrush val2 = new SolidBrush(_23A3(j));
				try
				{
					graphics.FillEllipse((Brush)(object)val2, rectangleF2);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
				num5++;
			}
		}
		if (_237C)
		{
			_23A0(graphics, rectangle, (int)((float)rectangle.Height * 0.35f), ؿ.Font);
		}
	}

	private void _239E(ItemPaintArgs ؿ)
	{
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Expected O, but got Unknown
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Expected O, but got Unknown
		Graphics graphics = ؿ.Graphics;
		Rectangle rectangle = _23AA();
		PointF _23A7 = new PointF(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);
		int num = 6;
		int num2 = 14;
		int num3 = 2;
		num2 = (int)Math.Round((double)rectangle.Width * 0.45);
		num = (int)Math.Round((double)num2 * 0.45);
		num3 = Math.Max(2, (int)Math.Round((double)num2 * 0.15));
		int num4 = _239B();
		if (!__2387)
		{
			int num5 = (int)Math.Round((double)__237E * ((double)num4 / (double)Math.Max(1, _ᗵ - _ᗶ)));
			for (int i = 0; i < num5; i++)
			{
				PointF pointF = _23A6(_23A7, num, __2382[i]);
				PointF pointF2 = _23A6(_23A7, num2, __2382[i]);
				Pen val = new Pen(_23A3(i), (float)num3);
				try
				{
					val.EndCap = (LineCap)2;
					val.StartCap = (LineCap)2;
					graphics.DrawLine(val, pointF, pointF2);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
		}
		else if (__2389)
		{
			int num6 = __237F;
			for (int j = 0; j < __237E; j++)
			{
				num6 %= __237E;
				PointF pointF3 = _23A6(_23A7, num, __2382[num6]);
				PointF pointF4 = _23A6(_23A7, num2, __2382[num6]);
				Pen val2 = new Pen(_23A3(j), (float)num3);
				try
				{
					val2.EndCap = (LineCap)2;
					val2.StartCap = (LineCap)2;
					graphics.DrawLine(val2, pointF3, pointF4);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
				num6++;
			}
		}
		if (_237C)
		{
			_23A0(graphics, rectangle, num, ؿ.Font);
		}
	}

	private string _239F()
	{
		try
		{
			if (!string.IsNullOrEmpty(__2386))
			{
				return __2386;
			}
			return string.Format(__2396, _ᗸ);
		}
		catch
		{
			return "Format Error";
		}
	}

	private void _23A0(Graphics _٥, RectangleF ശ, int _23A1, Font _23A2)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		eTextFormat format = eTextFormat.HorizontalCenter | eTextFormat.NoClipping | eTextFormat.NoPadding | eTextFormat.SingleLine | eTextFormat.VerticalCenter;
		float num = Math.Max(1f, (float)_23A1 / 1.8f);
		Color color = (__2384.IsEmpty ? __238B : __2384);
		Font val = new Font(_23A2.FontFamily, num, (FontStyle)0);
		try
		{
			TextDrawing.DrawString(_٥, _239F(), val, color, Rectangle.Round(ശ), format);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private Color _23A3(int _23A4, int _23A5)
	{
		return Color.FromArgb((int)(210.0 * (double)_23A4 / (double)_23A5) + 45, __238B);
	}

	private Color _23A3(int _23A4)
	{
		return _23A3(_23A4, __237E);
	}

	private PointF _23A6(PointF _23A7, int _23A8, double _23A9)
	{
		double num = Math.PI * _23A9 / 180.0;
		return new PointF(_23A7.X + (float)_23A8 * (float)Math.Cos(num), _23A7.Y + (float)_23A8 * (float)Math.Sin(num));
	}

	public override void RecalcSize()
	{
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Invalid comparison between Unknown and I4
		int num = __19F2;
		if (!(ContainerControl is CircularProgress))
		{
			num = Dpi.Width(__19F2);
		}
		Rectangle rect = new Rectangle(m_Rect.X, m_Rect.Y, num, num);
		if (_ᣅ && !string.IsNullOrEmpty(Text))
		{
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val != null)
			{
				Font font = val.Font;
				Graphics val2 = val.CreateGraphics();
				try
				{
					Size size = (_ᢧ = _20B1._20B2(this, val2, __238D, font, (__238D > 0) ? eTextFormat.WordBreak : eTextFormat.SingleLine, (int)val.RightToLeft == 1));
					size.Width += _ᢾ.Horizontal;
					size.Height += _ᢾ.Vertical;
					int num2 = Dpi.Width(_2380);
					if (__238C == eTextPosition.Left || __238C == eTextPosition.Right)
					{
						size.Width += num2;
						rect.Width += size.Width;
						rect.Height = Math.Max(rect.Height, size.Height);
					}
					else
					{
						size.Height += num2;
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

	private Rectangle _23AA()
	{
		int num = __19F2;
		if (!(ContainerControl is CircularProgress))
		{
			num = Dpi.Width(__19F2);
		}
		if (string.IsNullOrEmpty(Text) || !_ᣅ)
		{
			return new Rectangle(m_Rect.X + (m_Rect.Width - num) / 2, m_Rect.Y + (m_Rect.Height - num) / 2, num, num);
		}
		if (__238C == eTextPosition.Top)
		{
			return new Rectangle(m_Rect.X + (m_Rect.Width - num) / 2, m_Rect.Y + (m_Rect.Height - num), num, num);
		}
		if (__238C == eTextPosition.Right)
		{
			return new Rectangle(m_Rect.X, m_Rect.Y + (m_Rect.Height - num) / 2, num, num);
		}
		if (__238C == eTextPosition.Left)
		{
			return new Rectangle(m_Rect.Right - num, m_Rect.Y + (m_Rect.Height - num) / 2, num, num);
		}
		if (__238C == eTextPosition.Bottom)
		{
			return new Rectangle(m_Rect.X + (m_Rect.Width - num) / 2, m_Rect.Y, num, num);
		}
		return new Rectangle(m_Rect.X, m_Rect.Y, num, num);
	}

	private double[] _23AB(int _23AC)
	{
		double[] array = new double[_23AC];
		double num = 360.0 / (double)_23AC;
		for (int i = 0; i < _23AC; i++)
		{
			array[i] = ((i == 0) ? (270.0 + num) : (array[i - 1] + num));
		}
		return array;
	}

	private void _23AD(eCircularProgressType ك, eCircularProgressType ل)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("ProgressBarType"));
		Refresh();
	}

	private void _23AE(int ك, int ل)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("Maximum"));
		_23B0();
	}

	private void _23AF(int ك, int ل)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("Minimum"));
		_23B0();
	}

	private void _23B0()
	{
		int value = _ᗸ;
		if (_ᗸ < _ᗶ)
		{
			value = _ᗶ;
		}
		else if (_ᗸ > _ᗵ)
		{
			value = _ᗵ;
		}
		Value = value;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeProgressTextColor()
	{
		return !__2384.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetProgressTextColor()
	{
		ProgressTextColor = Color.Empty;
	}

	private void _23B1(bool ك, bool ل)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("ProgressTextVisible"));
		Refresh();
	}

	private void _23B2(string ك, string ل)
	{
		Refresh();
	}

	private void _23B3(int ك, int ل)
	{
		if (!__2387)
		{
			Refresh();
		}
		OnValueChanged(EventArgs.Empty);
		OnPropertyChanged(new PropertyChangedEventArgs("Value"));
	}

	protected virtual void OnValueChanged(EventArgs e)
	{
		ᗲ?.Invoke(this, e);
	}

	protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
	{
	}

	private void _23B4()
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		MethodInvoker val = null;
		if (!IsRunning || base.IsDisposed || __237D)
		{
			return;
		}
		if (base.InvokeRequired)
		{
			if (val == null)
			{
				val = (MethodInvoker)delegate
				{
					_23B4();
				};
			}
			BeginInvoke((Delegate)(object)val);
			return;
		}
		__237F = ++__237F % __237E;
		Refresh();
		object containerControl = ContainerControl;
		Control val2 = (Control)((containerControl is Control) ? containerControl : null);
		if (val2 != null)
		{
			val2.Update();
		}
	}

	public void Start()
	{
		if (!__2389)
		{
			__2387 = true;
			__2389 = true;
			__2388 = new BackgroundWorker();
			__2388.WorkerSupportsCancellation = true;
			__2388.DoWork += _23B5;
			__2388.RunWorkerAsync();
		}
	}

	private void _23B5(object _٠, DoWorkEventArgs ؿ)
	{
		BackgroundWorker backgroundWorker = (BackgroundWorker)_٠;
		while (!backgroundWorker.CancellationPending)
		{
			_23B4();
			try
			{
				using ManualResetEvent manualResetEvent = new ManualResetEvent(initialState: false);
				manualResetEvent.WaitOne(__2397);
			}
			catch
			{
				ؿ.Cancel = true;
			}
		}
		ؿ.Cancel = true;
	}

	public void Stop()
	{
		Stop(disposing: false);
	}

	public void Stop(bool disposing)
	{
		if (__2389)
		{
			__2387 = false;
			__2389 = false;
			BackgroundWorker backgroundWorker = __2388;
			__2388 = null;
			backgroundWorker.CancelAsync();
			while (backgroundWorker.IsBusy)
			{
				Application.DoEvents();
			}
			backgroundWorker.DoWork -= _23B5;
			backgroundWorker.Dispose();
			if (!disposing)
			{
				Refresh();
			}
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeProgressColor()
	{
		return __238B != _238A;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetProgressColor()
	{
		ProgressColor = _238A;
	}

	private void _23B6(int ك, int ل)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("Diameter"));
		NeedRecalcSize = true;
		Refresh();
	}

	private void _23B7(eTextPosition ك, eTextPosition ل)
	{
		NeedRecalcSize = true;
		Refresh();
		OnPropertyChanged(new PropertyChangedEventArgs("TextPosition"));
	}

	private void _23B8(bool ك, bool ل)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("TextVisible"));
		NeedRecalcSize = true;
		Refresh();
	}

	private void _23B9(int ك, int ل)
	{
		OnPropertyChanged(new PropertyChangedEventArgs("TextWidth"));
		NeedRecalcSize = true;
		Refresh();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeTextPadding()
	{
		if (_ᢾ.Bottom == 0 && _ᢾ.Top == 0 && _ᢾ.Left == 0)
		{
			return _ᢾ.Right != 0;
		}
		return true;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetTextPadding()
	{
		_ᢾ = new Padding(0, 0, 0, 0);
	}

	private void _23BA(object _٠, PropertyChangedEventArgs ؿ)
	{
		NeedRecalcSize = true;
		Refresh();
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

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializePieBorderDark()
	{
		return __238F != _238E;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetPieBorderDark()
	{
		PieBorderDark = _238E;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializePieBorderLight()
	{
		return __2391 != _2390;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetPieBorderLight()
	{
		PieBorderLight = _2390;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSpokeBorderDark()
	{
		return __2393 != _2392;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSpokeBorderDark()
	{
		SpokeBorderDark = _2392;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeSpokeBorderLight()
	{
		return __2395 != _2394;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetSpokeBorderLight()
	{
		SpokeBorderLight = _2394;
	}

	protected virtual void OnProgressTextFormatChanged(string oldValue, string newValue)
	{
		Refresh();
		OnPropertyChanged(new PropertyChangedEventArgs("ProgressTextFormat"));
	}

	[CompilerGenerated]
	private void _23BB()
	{
		_23B4();
	}
}
