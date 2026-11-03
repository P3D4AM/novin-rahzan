using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Threading;
using System.Windows.Forms;
using DevComponents.Schedule;

namespace DevComponents.DotNetBar.Controls;

[DefaultEvent("ValueChanged")]
[ToolboxBitmap(typeof(AnalogClockControl), "AnalogClock.AnalogClockControl.bmp")]
[Description("Analog Clock Control")]
[DefaultProperty("Value")]
public class AnalogClockControl : Control
{
	private const float __29F7 = 100f;

	private Point __29F8;

	private eClockEditStates __29F9;

	private Size __29FA;

	private Timer __23C3;

	private bool _᮰;

	private bool __29FB;

	private eClockStyles __29FC;

	private ClockStyleData __29FD;

	private eClockIndicatorStyles __29FE;

	private bool __29FF;

	private bool __2A00;

	private bool __2A01;

	private TimeValueChangingEventHandler _18AB;

	private EventHandler ᗲ;

	private DateTime _ᗸ;

	private int __2A02 = -1;

	private int __2A03 = -1;

	private DevComponents.Schedule.TimeZoneInfo __2A04;

	private string __2A05 = string.Empty;

	protected override Size DefaultMinimumSize => new Size(100, 100);

	[Description("Indicates whether Anti-aliasing is used when rendering the control.")]
	[Category("Appearance")]
	[DefaultValue(true)]
	public bool AntiAlias
	{
		get
		{
			return _᮰;
		}
		set
		{
			_᮰ = value;
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(false)]
	[Description("Toggles the state for automatic mode.  When true the clock will auto redraw once a second and display the current date/time.")]
	[Category("Behavior")]
	public bool AutomaticMode
	{
		get
		{
			return __29FB;
		}
		set
		{
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Expected O, but got Unknown
			__29FB = value;
			if (__29FB)
			{
				_ᗸ = DateTime.Now;
				__29FF = false;
				if (!((Component)this).DesignMode)
				{
					if (__23C3 != null)
					{
						((Component)(object)__23C3).Dispose();
					}
					__23C3 = new Timer();
					__23C3.Interval = 1000;
					__23C3.Tick += _2A0D;
					__23C3.Enabled = true;
				}
			}
			else if (__23C3 != null)
			{
				__23C3.Enabled = false;
				((Component)(object)__23C3).Dispose();
				__23C3 = null;
			}
			((Control)this).Invalidate();
		}
	}

	[RefreshProperties(RefreshProperties.All)]
	[Description("The clock style for the control.")]
	[Category("Appearance")]
	[DefaultValue(eClockStyles.Style1)]
	public eClockStyles ClockStyle
	{
		get
		{
			return __29FC;
		}
		set
		{
			__29FC = value;
			if (__29FC != eClockStyles.Custom)
			{
				if (__29FD != null)
				{
					__29FD.Parent = null;
					__29FD.Dispose();
				}
				__29FD = new ClockStyleData(__29FC, this);
			}
			((Control)this).Invalidate();
		}
	}

	[RefreshProperties(RefreshProperties.All)]
	[Category("Appearance")]
	[Description("The clock style data for the control.")]
	public ClockStyleData ClockStyleData
	{
		get
		{
			return __29FD;
		}
		set
		{
			if (__29FD != null)
			{
				__29FD.Parent = null;
			}
			__29FD = value;
			__29FD.Parent = this;
			__29FD.Style = eClockStyles.Custom;
			ClockStyle = eClockStyles.Custom;
			((Control)this).Invalidate();
		}
	}

	[Category("Appearance")]
	[DefaultValue(eClockIndicatorStyles.Ticks)]
	[Description("Indicates which indicator style the clock will use.")]
	public eClockIndicatorStyles IndicatorStyle
	{
		get
		{
			return __29FE;
		}
		set
		{
			__29FE = value;
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether the time can be changed by moving the clock hands. If AutomaticMode is enabled, this setting is ignored.")]
	public bool IsEditable
	{
		get
		{
			return __29FF;
		}
		set
		{
			if (!AutomaticMode)
			{
				__29FF = value;
			}
			((Control)this).Invalidate();
		}
	}

	[Category("Appearance")]
	[DefaultValue(true)]
	[Description("Indicates whether the glass overlay on the clock control will be displayed or not.")]
	public bool ShowGlassOverlay
	{
		get
		{
			return __2A00;
		}
		set
		{
			__2A00 = value;
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether the second hand on the clock control will be displayed or not.")]
	public bool ShowSecondHand
	{
		get
		{
			return __2A01;
		}
		set
		{
			__2A01 = value;
			((Control)this).Invalidate();
		}
	}

	[Category("Behavior")]
	[Description("The current date/time value for this control. If AutomaticMode is enabled, assigned values are ignored.")]
	public DateTime Value
	{
		get
		{
			return _ᗸ;
		}
		set
		{
			_ᗸ = value;
			OnValueChanged(EventArgs.Empty);
			((Control)this).Invalidate();
		}
	}

	[Editor("DevComponents.DotNetBar.Design.TimeZoneSelectionEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Category("Behavior")]
	[Description("Indicates time-zone string identifier that is used to display the time when AutomaticMode=true and clock is displaying current time.")]
	[DefaultValue("")]
	public string TimeZone
	{
		get
		{
			return __2A05;
		}
		set
		{
			if (value != __2A05)
			{
				DevComponents.Schedule.TimeZoneInfo timeZoneInfo = null;
				if (!string.IsNullOrEmpty(value))
				{
					timeZoneInfo = DevComponents.Schedule.TimeZoneInfo.FindSystemTimeZoneById(value);
				}
				if (timeZoneInfo != null || string.IsNullOrEmpty(value))
				{
					string oldValue = __2A05;
					__2A05 = value;
					__2A04 = timeZoneInfo;
					OnTimeZoneChanged(oldValue, value);
				}
			}
		}
	}

	[Description("Occurs while user is dragging the mouse in order to change time.")]
	public event TimeValueChangingEventHandler ValueChanging
	{
		add
		{
			TimeValueChangingEventHandler timeValueChangingEventHandler = _18AB;
			TimeValueChangingEventHandler timeValueChangingEventHandler2;
			do
			{
				timeValueChangingEventHandler2 = timeValueChangingEventHandler;
				TimeValueChangingEventHandler value2 = (TimeValueChangingEventHandler)Delegate.Combine(timeValueChangingEventHandler2, value);
				timeValueChangingEventHandler = Interlocked.CompareExchange(ref _18AB, value2, timeValueChangingEventHandler2);
			}
			while ((object)timeValueChangingEventHandler != timeValueChangingEventHandler2);
		}
		remove
		{
			TimeValueChangingEventHandler timeValueChangingEventHandler = _18AB;
			TimeValueChangingEventHandler timeValueChangingEventHandler2;
			do
			{
				timeValueChangingEventHandler2 = timeValueChangingEventHandler;
				TimeValueChangingEventHandler value2 = (TimeValueChangingEventHandler)Delegate.Remove(timeValueChangingEventHandler2, value);
				timeValueChangingEventHandler = Interlocked.CompareExchange(ref _18AB, value2, timeValueChangingEventHandler2);
			}
			while ((object)timeValueChangingEventHandler != timeValueChangingEventHandler2);
		}
	}

	[Description("Occurs when Value i.e. time clock is displaying has changed.")]
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

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetClockStyleData()
	{
		ClockStyle = eClockStyles.Style1;
	}

	protected virtual void OnValueChanging(TimeValueChangingEventArgs e)
	{
		_18AB?.Invoke(this, e);
	}

	protected virtual void OnValueChanged(EventArgs e)
	{
		ᗲ?.Invoke(this, e);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetValue()
	{
		Value = DateTime.Now;
	}

	public AnalogClockControl()
	{
		((Control)this).SetStyle((ControlStyles)141330, true);
		_᮰ = true;
		__29FB = false;
		__29FC = eClockStyles.Style1;
		__29FD = new ClockStyleData();
		__29FD.Parent = this;
		__29F9 = eClockEditStates.None;
		__29FE = eClockIndicatorStyles.Ticks;
		__29FF = false;
		__2A00 = true;
		__2A01 = true;
		_ᗸ = DateTime.Now;
		((Control)this).MinimumSize = new Size(100, 100);
		((Control)this).Size = ((Control)this).MinimumSize;
	}

	protected override void Dispose(bool disposing)
	{
		((Control)this).Dispose(disposing);
		if (disposing)
		{
			if (__29FD != null)
			{
				__29FD.Parent = null;
				__29FD.Dispose();
			}
			if (__23C3 != null)
			{
				((Component)(object)__23C3).Dispose();
				__23C3 = null;
			}
		}
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		((Control)this).OnMouseDown(e);
		if ((int)e.Button != 1048576 || !__29FF)
		{
			return;
		}
		float num = __29FD.BezelWidth * (float)(((Control)this).ClientRectangle.Width - 1);
		RectangleF boundingRect = new RectangleF((float)((Control)this).ClientRectangle.X + num, (float)((Control)this).ClientRectangle.Y + num, (float)(((Control)this).ClientRectangle.Width - 1) - num * 2f, (float)(((Control)this).ClientRectangle.Height - 1) - num * 2f);
		float angle = _2A0B((float)_ᗸ.TimeOfDay.TotalHours * 30f);
		if (__29FD.HourHandStyle.ContainsPoint(boundingRect, angle, e.Location))
		{
			__2A02 = _ᗸ.Hour;
			__2A03 = _ᗸ.Hour;
			if (_ᗸ.Hour > 12)
			{
				__2A02 -= 12;
			}
			__29F9 = eClockEditStates.Hour;
		}
		else
		{
			angle = (float)_ᗸ.TimeOfDay.TotalMinutes * 6f;
			if (__29FD.MinuteHandStyle.ContainsPoint(boundingRect, angle, e.Location))
			{
				__29F9 = eClockEditStates.Minute;
			}
			else
			{
				angle = (float)_ᗸ.TimeOfDay.Seconds * 6f;
				if (__2A01 && __29FD.SecondHandStyle.ContainsPoint(boundingRect, angle, e.Location))
				{
					__29F9 = eClockEditStates.Second;
				}
			}
		}
		if (__29F9 != 0)
		{
			__29F8 = e.Location;
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		if (!__29FF)
		{
			return;
		}
		if (__29F9 != 0)
		{
			__29F8 = e.Location;
			if (__29F9 == eClockEditStates.Hour)
			{
				int num = _2A07(__29F8);
				if (num == 1 && __2A02 == 12)
				{
					__2A02 = 0;
				}
				else if (num == 12 && __2A02 == 1)
				{
					__2A02 = 13;
				}
				__2A03 += num - __2A02;
				if (__2A03 < 0)
				{
					__2A03 = 23;
				}
				else if (__2A03 > 23)
				{
					__2A03 = 0;
				}
				__2A02 = num;
			}
			OnValueChanging(new TimeValueChangingEventArgs(_2A0A()));
			((Control)this).Invalidate();
			return;
		}
		float num2 = __29FD.BezelWidth * (float)(((Control)this).ClientRectangle.Width - 1);
		RectangleF boundingRect = new RectangleF((float)((Control)this).ClientRectangle.X + num2, (float)((Control)this).ClientRectangle.Y + num2, (float)(((Control)this).ClientRectangle.Width - 1) - num2 * 2f, (float)(((Control)this).ClientRectangle.Height - 1) - num2 * 2f);
		float angle = _2A0B((float)_ᗸ.TimeOfDay.TotalHours * 30f);
		if (__29FD.HourHandStyle.ContainsPoint(boundingRect, angle, e.Location))
		{
			((Control)this).Cursor = Cursors.Hand;
			return;
		}
		angle = (float)_ᗸ.TimeOfDay.TotalMinutes * 6f;
		if (__29FD.MinuteHandStyle.ContainsPoint(boundingRect, angle, e.Location))
		{
			((Control)this).Cursor = Cursors.Hand;
			return;
		}
		angle = (float)_ᗸ.TimeOfDay.Seconds * 6f;
		if (__2A01 && __29FD.SecondHandStyle.ContainsPoint(boundingRect, angle, e.Location))
		{
			((Control)this).Cursor = Cursors.Hand;
		}
		else if (((Control)this).Cursor != Cursors.Default)
		{
			((Control)this).Cursor = Cursors.Default;
		}
	}

	private float _2A06(Point _065C)
	{
		float num = (float)_2A76._2A77(_22A2(_065C));
		for (num += 90f; num < 0f; num += 360f)
		{
		}
		while (num >= 360f)
		{
			num -= 360f;
		}
		return num;
	}

	private int _2A07(Point _065C)
	{
		float num = _2A0B(_2A06(_065C));
		int num2 = (int)Math.Round((double)num / 30.0, 0);
		if (num2 == 0)
		{
			num2 = 12;
		}
		return num2;
	}

	private int _2A08(Point _065C)
	{
		float num = _2A06(_065C);
		int num2 = (int)Math.Round((double)num / 6.0, 0);
		if (num2 == 60)
		{
			num2 = 0;
		}
		return num2;
	}

	private int _2A09(Point _065C)
	{
		float num = _2A06(_065C);
		int num2 = (int)Math.Round((double)num / 6.0, 0);
		if (num2 == 60)
		{
			num2 = 0;
		}
		return num2;
	}

	private DateTime _2A0A()
	{
		DateTime result = _ᗸ;
		switch (__29F9)
		{
		case eClockEditStates.Hour:
			result = new DateTime(_ᗸ.Year, _ᗸ.Month, _ᗸ.Day, __2A03, _ᗸ.Minute, _ᗸ.Second);
			break;
		case eClockEditStates.Minute:
			result = new DateTime(_ᗸ.Year, _ᗸ.Month, _ᗸ.Day, _ᗸ.Hour, _2A08(__29F8), _ᗸ.Second);
			break;
		case eClockEditStates.Second:
			result = new DateTime(_ᗸ.Year, _ᗸ.Month, _ᗸ.Day, _ᗸ.Hour, _ᗸ.Minute, _2A09(__29F8));
			break;
		}
		return result;
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		if (__29F9 != 0)
		{
			__29F8 = e.Location;
			Value = _2A0A();
			__29F8 = Point.Empty;
			__29F9 = eClockEditStates.None;
			((Control)this).Cursor = Cursors.Default;
		}
		((Control)this).OnMouseUp(e);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).OnPaint(e);
		Graphics graphics = e.Graphics;
		TextRenderingHint textRenderingHint = graphics.TextRenderingHint;
		SmoothingMode smoothingMode = graphics.SmoothingMode;
		if (_᮰)
		{
			graphics.TextRenderingHint = BarUtilities.AntiAliasTextRenderingHint;
			graphics.SmoothingMode = (SmoothingMode)4;
		}
		float num = __29FD.BezelWidth * (float)(((Control)this).ClientRectangle.Width - 1);
		RectangleF rect = new RectangleF((float)((Control)this).ClientRectangle.X + num, (float)((Control)this).ClientRectangle.Y + num, (float)(((Control)this).ClientRectangle.Width - 1) - num * 2f, (float)(((Control)this).ClientRectangle.Height - 1) - num * 2f);
		e.Graphics.TranslateTransform(rect.X + rect.Width * 0.5f, rect.Y + rect.Height * 0.5f);
		switch (__29FE)
		{
		case eClockIndicatorStyles.Ticks:
			DrawTicks(graphics, rect);
			break;
		case eClockIndicatorStyles.Numbers:
			DrawNumbers(graphics, rect);
			break;
		}
		DrawHands(graphics, rect, overCap: false);
		DrawCap(graphics, rect);
		DrawHands(graphics, rect, overCap: true);
		if (__2A00)
		{
			DrawGlassOverlay(graphics, rect);
		}
		graphics.TextRenderingHint = textRenderingHint;
		graphics.SmoothingMode = smoothingMode;
	}

	protected override void OnPaintBackground(PaintEventArgs pevent)
	{
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Expected O, but got Unknown
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		((Control)this).OnPaintBackground(pevent);
		RectangleF rectangleF = default(RectangleF);
		Graphics graphics = pevent.Graphics;
		graphics.SmoothingMode = (SmoothingMode)(_᮰ ? 4 : 0);
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		if (__29FD.BezelColor.BorderWidth > 0f)
		{
			int num = (int)(__29FD.BezelColor.BorderWidth * (float)Math.Min(clientRectangle.Width, clientRectangle.Height) / 2f);
			clientRectangle.Inflate(-num, -num);
		}
		float num2 = Math.Min(clientRectangle.Width, clientRectangle.Height);
		float num3 = __29FD.BezelWidth * (num2 - 1f);
		RectangleF rectangleF2 = new RectangleF((float)clientRectangle.X + num3, (float)clientRectangle.Y + num3, (float)(clientRectangle.Width - 1) - num3 * 2f, (float)(clientRectangle.Height - 1) - num3 * 2f);
		RectangleF rectangleF3 = new RectangleF(clientRectangle.X, clientRectangle.Y, clientRectangle.Width - 1, clientRectangle.Height - 1);
		GraphicsPath val = new GraphicsPath();
		GraphicsPath val2 = new GraphicsPath();
		if (__29FD.ClockShape == eClockShapes.Round)
		{
			val.AddEllipse(rectangleF3);
			val.CloseAllFigures();
			val2.AddEllipse(rectangleF2);
			val2.CloseAllFigures();
		}
		else
		{
			val.AddRectangle(rectangleF3);
			val2.AddRectangle(rectangleF2);
		}
		if (__29FD.FaceBackgroundImage != null)
		{
			graphics.SetClip(val);
			float num4 = ((__29FD.FaceBackgroundImage.Width >= __29FD.FaceBackgroundImage.Height) ? (rectangleF2.Height / (float)__29FD.FaceBackgroundImage.Height) : (rectangleF2.Width / (float)__29FD.FaceBackgroundImage.Width));
			rectangleF.X = rectangleF2.X + (rectangleF2.Width / 2f - num4 * (float)__29FD.FaceBackgroundImage.Width / 2f);
			rectangleF.Y = rectangleF2.Y + (rectangleF2.Height / 2f - num4 * (float)__29FD.FaceBackgroundImage.Height / 2f);
			rectangleF.Width = num4 * (float)__29FD.FaceBackgroundImage.Width;
			rectangleF.Height = num4 * (float)__29FD.FaceBackgroundImage.Height;
			graphics.DrawImage(__29FD.FaceBackgroundImage, rectangleF);
		}
		graphics.ResetClip();
		graphics.SetClip(val2, (CombineMode)4);
		Brush brush = __29FD.BezelColor.GetBrush(val);
		graphics.FillPath(brush, val);
		brush.Dispose();
		if (__29FD.BezelColor.BorderWidth > 0f)
		{
			Pen borderPen = __29FD.BezelColor.GetBorderPen(num2, (PenAlignment)0);
			graphics.DrawPath(borderPen, val);
			borderPen.Dispose();
		}
		graphics.ResetClip();
		if (__29FD.FaceBackgroundImage == null)
		{
			brush = __29FD.FaceColor.GetBrush(val2, new PointF(0.5f, 0.5f));
			graphics.FillPath(brush, val2);
			brush.Dispose();
		}
		if (__29FD.FaceColor.BorderWidth > 0f)
		{
			Pen borderPen = __29FD.FaceColor.GetBorderPen(num2, (PenAlignment)0);
			graphics.DrawPath(borderPen, val2);
			borderPen.Dispose();
		}
		val2.Dispose();
		val.Dispose();
	}

	protected override void OnResize(EventArgs e)
	{
		((Control)this).OnResize(e);
		if (__29FA.Width != ((Control)this).Width)
		{
			((Control)this).Height = ((Control)this).Width;
		}
		else
		{
			((Control)this).Width = ((Control)this).Height;
		}
		__29FA = ((Control)this).Size;
	}

	protected virtual void DrawCap(Graphics gfx, RectangleF rect)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		float num = Math.Min(rect.Width, rect.Height);
		float num2 = __29FD.CapSize * num;
		RectangleF rectangleF = new RectangleF((0f - num2) / 2f, (0f - num2) / 2f, num2, num2);
		GraphicsPath val = new GraphicsPath();
		val.AddEllipse(rectangleF);
		Brush brush = __29FD.CapColor.GetBrush(val);
		gfx.FillPath(brush, val);
		if (__29FD.CapColor.BorderWidth > 0f)
		{
			Pen borderPen = __29FD.CapColor.GetBorderPen(num, (PenAlignment)2);
			gfx.DrawPath(borderPen, val);
			borderPen.Dispose();
		}
		brush.Dispose();
		val.Dispose();
	}

	protected virtual void DrawGlassOverlay(Graphics gfx, RectangleF rect)
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected O, but got Unknown
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected O, but got Unknown
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		GraphicsState val = gfx.Save();
		rect.Width *= 0.95f;
		rect.Height *= 0.95f;
		rect.X = (0f - rect.Width) / 2f;
		rect.Y = (0f - rect.Height) / 2f;
		float num = Math.Min(rect.Width, rect.Height) / 2f;
		GraphicsPath val2 = new GraphicsPath();
		val2.AddEllipse(rect);
		PathGradientBrush val3 = new PathGradientBrush(val2);
		val3.CenterPoint = new PointF(0f, 0f);
		val3.CenterColor = Color.FromArgb(192, Color.White);
		val3.SurroundColors = new Color[1] { Color.FromArgb(64, Color.White) };
		GraphicsPath val4 = new GraphicsPath();
		val4.AddArc(rect, 180f, 180f);
		PointF[] array = new PointF[5];
		array[0].X = 0f - num;
		array[0].Y = 0f;
		array[1].X = (0f - num) * 0.5f;
		array[1].Y = (0f - num) * 0.175f;
		array[2].X = 0f;
		array[2].Y = (0f - num) * 0.25f;
		array[3].X = num * 0.5f;
		array[3].Y = (0f - num) * 0.175f;
		array[4].X = num;
		array[4].Y = 0f;
		val4.AddCurve(array);
		val4.CloseAllFigures();
		gfx.RotateTransform((float)__29FD.GlassAngle);
		gfx.FillPath((Brush)(object)val3, val4);
		gfx.Restore(val);
		((Brush)val3).Dispose();
		val2.Dispose();
		val4.Dispose();
	}

	private static float _2A0B(float _23A9)
	{
		return (float)Math.Floor(_23A9 / 30f) * 30f;
	}

	protected virtual void DrawHands(Graphics gfx, RectangleF rect, bool overCap)
	{
		float scaleFactor = Math.Min(rect.Width, rect.Height);
		GraphicsState val = gfx.Save();
		gfx.ResetTransform();
		if (__29FD.HourHandStyle.DrawOverCap == overCap)
		{
			float num = ((__29F9 != eClockEditStates.Hour) ? ((float)((_ᗸ.TimeOfDay.TotalHours > 12.0) ? (_ᗸ.TimeOfDay.TotalHours - 12.0) : _ᗸ.TimeOfDay.TotalHours) * 30f) : ((float)_2A76._2A77(_22A2(__29F8)) + 90f));
			if (__29FF && !__29FB)
			{
				num = _2A0B(num);
			}
			GraphicsPath val2 = __29FD.HourHandStyle.GenerateHandPath(rect, num);
			Brush brush = __29FD.HourHandStyle.HandColor.GetBrush(val2, __29FD.HourHandStyle.HandColor.BrushAngle + num);
			gfx.FillPath(brush, val2);
			if (__29FD.HourHandStyle.HandColor.BorderWidth > 0f)
			{
				Pen borderPen = __29FD.HourHandStyle.HandColor.GetBorderPen(scaleFactor, (PenAlignment)2);
				gfx.DrawPath(borderPen, val2);
				borderPen.Dispose();
			}
			brush.Dispose();
			val2.Dispose();
		}
		if (__29FD.MinuteHandStyle.DrawOverCap == overCap)
		{
			float num = ((__29F9 != eClockEditStates.Minute) ? ((float)_ᗸ.TimeOfDay.TotalMinutes * 6f) : ((float)_2A76._2A77(_22A2(__29F8)) + 90f));
			GraphicsPath val2 = __29FD.MinuteHandStyle.GenerateHandPath(rect, num);
			Brush brush = __29FD.MinuteHandStyle.HandColor.GetBrush(val2, __29FD.MinuteHandStyle.HandColor.BrushAngle + num);
			gfx.FillPath(brush, val2);
			if (__29FD.MinuteHandStyle.HandColor.BorderWidth > 0f)
			{
				Pen borderPen = __29FD.MinuteHandStyle.HandColor.GetBorderPen(scaleFactor, (PenAlignment)2);
				gfx.DrawPath(borderPen, val2);
				borderPen.Dispose();
			}
			brush.Dispose();
			val2.Dispose();
		}
		if (__2A01 && __29FD.SecondHandStyle.DrawOverCap == overCap)
		{
			float num = ((__29F9 != eClockEditStates.Second) ? ((float)_ᗸ.TimeOfDay.Seconds * 6f) : ((float)_2A76._2A77(_22A2(__29F8)) + 90f));
			GraphicsPath val2 = __29FD.SecondHandStyle.GenerateHandPath(rect, num);
			Brush brush = __29FD.SecondHandStyle.HandColor.GetBrush(val2, __29FD.SecondHandStyle.HandColor.BrushAngle + num);
			gfx.FillPath(brush, val2);
			if (__29FD.SecondHandStyle.HandColor.BorderWidth > 0f)
			{
				Pen borderPen = __29FD.SecondHandStyle.HandColor.GetBorderPen(scaleFactor, (PenAlignment)2);
				gfx.DrawPath(borderPen, val2);
				borderPen.Dispose();
			}
			brush.Dispose();
			val2.Dispose();
		}
		gfx.Restore(val);
	}

	protected virtual void DrawNumbers(Graphics gfx, RectangleF rect)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Expected O, but got Unknown
		StringFormat val = new StringFormat();
		val.Alignment = (StringAlignment)1;
		val.LineAlignment = (StringAlignment)1;
		float num = Math.Min(rect.Width, rect.Height) / 100f;
		Font val2 = new Font(__29FD.NumberFont.FontFamily, __29FD.NumberFont.Size * num, __29FD.NumberFont.Style, (GraphicsUnit)2);
		new PointF(rect.X + rect.Width / 2f, rect.Y + rect.Height / 2f);
		float num2 = rect.Width / 2f - gfx.MeasureString("12", val2).Height / 1.25f;
		float num3 = (float)_2A76._2A79(30.0);
		float num4 = (float)_2A76._2A79(-60.0);
		Brush val3 = (Brush)new SolidBrush(__29FD.NumberColor);
		PointF pointF = default(PointF);
		for (int i = 1; i <= 12; i++)
		{
			pointF.X = (float)((double)num2 * Math.Cos(num4));
			pointF.Y = (float)((double)num2 * Math.Sin(num4));
			gfx.DrawString(i.ToString(), val2, val3, pointF, val);
			num4 += num3;
		}
		val3.Dispose();
		val2.Dispose();
	}

	protected virtual void DrawTicks(Graphics gfx, RectangleF rect)
	{
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Expected O, but got Unknown
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Expected O, but got Unknown
		Pen val = null;
		Pen val2 = null;
		float num = Math.Min(rect.Width, rect.Height);
		PointF[] array = new PointF[4];
		array[0].X = (0f - __29FD.LargeTickWidth) * 0.5f * num;
		array[0].Y = -0.45f * num;
		array[1].X = (0f - __29FD.LargeTickWidth) * 0.5f * num;
		array[1].Y = (-0.45f + __29FD.LargeTickLength) * num;
		array[2].X = __29FD.LargeTickWidth * 0.5f * num;
		array[2].Y = (-0.45f + __29FD.LargeTickLength) * num;
		array[3].X = __29FD.LargeTickWidth * 0.5f * num;
		array[3].Y = -0.45f * num;
		PointF[] array2 = new PointF[4];
		array2[0].X = (0f - __29FD.SmallTickWidth) * -0.5f * num;
		array2[0].Y = -0.45f * num;
		array2[1].X = (0f - __29FD.SmallTickWidth) * -0.5f * num;
		array2[1].Y = (-0.45f + __29FD.SmallTickLength) * num;
		array2[2].X = __29FD.SmallTickWidth * -0.5f * num;
		array2[2].Y = (-0.45f + __29FD.SmallTickLength) * num;
		array2[3].X = __29FD.SmallTickWidth * -0.5f * num;
		array2[3].Y = -0.45f * num;
		GraphicsPath val3 = new GraphicsPath();
		val3.AddPolygon(array);
		GraphicsPath val4 = new GraphicsPath();
		val4.AddPolygon(array2);
		Brush brush = __29FD.LargeTickColor.GetBrush(val3);
		if (__29FD.LargeTickColor.BorderWidth > 0f)
		{
			val = __29FD.LargeTickColor.GetBorderPen(num, (PenAlignment)0);
		}
		Brush brush2 = __29FD.LargeTickColor.GetBrush(val3);
		if (__29FD.SmallTickColor.BorderWidth > 0f)
		{
			val2 = __29FD.SmallTickColor.GetBorderPen(num, (PenAlignment)0);
		}
		GraphicsState val5 = gfx.Save();
		for (int i = 0; i < 12; i++)
		{
			gfx.FillPolygon(brush, array);
			if (val != null)
			{
				gfx.DrawPolygon(val, array);
			}
			for (int j = 0; j < 4; j++)
			{
				gfx.RotateTransform(6f);
				gfx.FillPolygon(brush2, array2);
				if (val2 != null)
				{
					gfx.DrawPolygon(val2, array2);
				}
			}
			gfx.RotateTransform(6f);
		}
		gfx.Restore(val5);
		val3.Dispose();
		val4.Dispose();
		brush.Dispose();
		brush2.Dispose();
		if (val != null)
		{
			val.Dispose();
		}
		if (val2 != null)
		{
			val2.Dispose();
		}
	}

	private float _22A2(PointF ᓏ)
	{
		PointF pointF = new PointF((float)((Control)this).ClientRectangle.X + (float)((Control)this).ClientRectangle.Width / 2f, (float)((Control)this).ClientRectangle.Y + (float)((Control)this).ClientRectangle.Height / 2f);
		ᓏ.X -= pointF.X;
		ᓏ.Y -= pointF.Y;
		return (float)Math.Atan2(ᓏ.Y, ᓏ.X);
	}

	private void _2A0C()
	{
		if (__2A04 != null)
		{
			Value = DevComponents.Schedule.TimeZoneInfo.ConvertTime(DateTime.Now, __2A04);
		}
		else
		{
			Value = DateTime.Now;
		}
	}

	private void _2A0D(object _٠, EventArgs ؿ)
	{
		_2A0C();
	}

	protected virtual void OnTimeZoneChanged(string oldValue, string newValue)
	{
		if (__29FB)
		{
			_2A0C();
		}
	}
}
