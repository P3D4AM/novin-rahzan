using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.ColorPickers;

[DefaultEvent("SelectedColorChanged")]
[ToolboxItem(true)]
[DefaultProperty("SelectedColor")]
[ToolboxBitmap(typeof(ColorCombControl), "ColorPickerItem.ColorCombControl.ico")]
public class ColorCombControl : Control
{
	private const float _2294 = 0.824f;

	private EventHandler Ṳ;

	private _22AB[] _2295 = new _22AB[144];

	private float[] _2296 = new float[6] { -0.5f, -1f, -0.5f, 0.5f, 1f, 0.5f };

	private float[] _2297 = new float[6] { 0.824f, 0f, -0.824f, -0.824f, 0f, 0.824f };

	private int _2298 = 7;

	private int _2299 = -1;

	private int _229A = -1;

	private Container Պ;

	private bool ե = true;

	private EventHandler _229B;

	[Browsable(true)]
	[Category("Appearance")]
	[Description("Gets or sets whether anti-aliasing is used while painting.")]
	[DefaultValue(true)]
	public bool AntiAlias
	{
		get
		{
			return ե;
		}
		set
		{
			if (ե != value)
			{
				ե = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Browsable(false)]
	public Color MouseOverColor
	{
		get
		{
			if (_2299 < 0)
			{
				return Color.Empty;
			}
			return _2295[_2299]._22AC;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public Color SelectedColor
	{
		get
		{
			if (_229A < 0)
			{
				return Color.Empty;
			}
			return _2295[_229A]._22AC;
		}
		set
		{
			if (value == Color.Empty && _229A >= 0)
			{
				_2295[_229A].ᒅ = false;
				_229A = -1;
				((Control)this).Invalidate();
				OnSelectedColorChanged(EventArgs.Empty);
				return;
			}
			int num = _22A9(value);
			if (num != _229A)
			{
				if (_229A >= 0)
				{
					_2295[_229A].ᒅ = false;
				}
				_229A = num;
				if (_229A >= 0)
				{
					_2295[_229A].ᒅ = true;
				}
				((Control)this).Invalidate();
				OnSelectedColorChanged(EventArgs.Empty);
			}
		}
	}

	public event EventHandler SelectedColorChanged
	{
		add
		{
			EventHandler eventHandler = Ṳ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ṳ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = Ṳ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref Ṳ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event EventHandler MouseOverColorChanged
	{
		add
		{
			EventHandler eventHandler = _229B;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _229B, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _229B;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _229B, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	protected virtual void OnSelectedColorChanged(EventArgs e)
	{
		Ṳ?.Invoke(this, e);
	}

	public ColorCombControl()
	{
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < _2295.Length; i++)
		{
			_2295[i] = new _22AB();
		}
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle(DisplayHelp.DoubleBufferFlag, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)2048, true);
		ـ();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && Պ != null)
		{
			Պ.Dispose();
		}
		((Control)this).Dispose(disposing);
	}

	private void ـ()
	{
		Պ = new Container();
	}

	private int _229C(int _0970)
	{
		int num = _0970 / (2 * _2298 - 1) + 1;
		if ((int)Math.Floor((double)num / 2.0) * 2 < num)
		{
			num--;
		}
		return num;
	}

	private void _229D()
	{
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		clientRectangle.Inflate(-8, -8);
		clientRectangle.Height -= _229C(Math.Min(clientRectangle.Height, clientRectangle.Width));
		if (clientRectangle.Height < clientRectangle.Width)
		{
			clientRectangle.Inflate(-(clientRectangle.Width - clientRectangle.Height) / 2, 0);
		}
		else
		{
			clientRectangle.Inflate(0, -(clientRectangle.Height - clientRectangle.Width) / 2);
		}
		int num = _229C(clientRectangle.Height);
		clientRectangle.Height -= num * 2;
		int num2 = (clientRectangle.Left + clientRectangle.Right) / 2;
		int num3 = (clientRectangle.Top + clientRectangle.Bottom) / 2;
		_2295[0]._22AC = Color.White;
		_2295[0]._22AF(num2, num3, num);
		int num4 = 1;
		for (int i = 1; i < _2298; i++)
		{
			float num5 = num2 + num * i;
			float num6 = num3;
			for (int j = 0; j < _2298 - 1; j++)
			{
				int num7 = (int)((float)num * _2296[j]);
				int num8 = (int)((float)num * _2297[j]);
				for (int k = 0; k < i; k++)
				{
					float num9 = _22A2(num5 - (float)num2, num6 - (float)num3);
					double _22A0 = 0.936 * (double)(_2298 - i) / (double)_2298 + 0.12;
					_2295[num4]._22AC = _229E(num9, _22A0, 1.0);
					_2295[num4]._22AF(num5, num6, num);
					num4++;
					num5 += (float)num7;
					num6 += (float)num8;
				}
			}
		}
		_2295[num4]._22AC = Color.Black;
		num4++;
		_2295[num4]._22AC = Color.White;
		int num10 = 15;
		int num11 = 255 - num10;
		num2 = clientRectangle.X + num * 3;
		num3 = clientRectangle.Bottom + num;
		for (int l = 0; l < 15; l++)
		{
			Color _22AC = Color.FromArgb(num11, num11, num11);
			_2295[num4]._22AC = _22AC;
			_2295[num4]._22AF(num2, num3, num);
			num2 += num;
			num4++;
			if (l == 7)
			{
				num2 = clientRectangle.X + (int)((double)num * 3.5);
				num3 += (int)((float)num * 0.824f);
			}
			num11 -= num10;
		}
		_2295[num4]._22AC = Color.Black;
	}

	private Color _229E(double _229F, double _22A0, double _22A1)
	{
		int red;
		int green;
		int blue;
		if (_22A1 == 0.0)
		{
			red = (green = (blue = (int)(_22A0 * 255.0)));
		}
		else
		{
			float num = ((!(_22A0 <= 0.5)) ? ((float)(_22A0 + _22A1 - _22A0 * _22A1)) : ((float)(_22A0 + _22A0 * _22A1)));
			float _22A6 = (float)(2.0 * _22A0 - (double)num);
			red = _22A5(_22A6, num, (float)(_229F + 120.0));
			green = _22A5(_22A6, num, (float)_229F);
			blue = _22A5(_22A6, num, (float)(_229F - 120.0));
		}
		return Color.FromArgb(red, green, blue);
	}

	private float _22A2(float _22A3, float _22A4)
	{
		double num = Math.Atan2(_22A4, _22A3);
		return (float)(num * 180.0 / Math.PI);
	}

	private int _22A5(float _22A6, float _22A7, float _22A8)
	{
		if (_22A8 > 360f)
		{
			_22A8 -= 360f;
		}
		else if (_22A8 < 0f)
		{
			_22A8 += 360f;
		}
		if (_22A8 < 60f)
		{
			_22A6 += (_22A7 - _22A6) * _22A8 / 60f;
		}
		else if (_22A8 < 180f)
		{
			_22A6 = _22A7;
		}
		else if (_22A8 < 240f)
		{
			_22A6 += (_22A7 - _22A6) * (240f - _22A8) / 60f;
		}
		return (int)(_22A6 * 255f);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		Graphics graphics = e.Graphics;
		if (ե)
		{
			e.Graphics.SmoothingMode = (SmoothingMode)4;
			e.Graphics.TextRenderingHint = DisplayHelp.AntiAliasTextRenderingHint;
		}
		SolidBrush val = new SolidBrush(((Control)this).BackColor);
		try
		{
			graphics.FillRectangle((Brush)(object)val, -1, -1, ((Control)this).Width + 1, ((Control)this).Height + 1);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		if (((Control)this).BackColor == Color.Transparent || ((Control)this).BackgroundImage != null)
		{
			((Control)this).OnPaintBackground(e);
		}
		_22AB[] array = _2295;
		foreach (_22AB _22AB2 in array)
		{
			_22AB2._22B1(graphics);
		}
		if (_2299 >= 0)
		{
			_2295[_2299]._22B1(graphics);
		}
		if (_229A >= 0)
		{
			_2295[_229A]._22B1(graphics);
		}
		((Control)this).OnPaint(e);
	}

	protected override void OnResize(EventArgs e)
	{
		_229D();
		((Control)this).OnResize(e);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		int _22AA = ڍ(e.X, e.Y);
		_0ECC(_22AA);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)e.Button == 1048576)
		{
			if (_229A >= 0)
			{
				_2295[_229A].ᒅ = false;
				((Control)this).Invalidate(_2295[_229A].ঋ);
			}
			_229A = -1;
			if (_2299 >= 0)
			{
				_229A = _2299;
				_2295[_229A].ᒅ = true;
				((Control)this).Invalidate(_2295[_229A].ঋ);
				OnSelectedColorChanged(EventArgs.Empty);
			}
		}
		((Control)this).OnMouseDown(e);
	}

	private int _22A9(Color ԝ)
	{
		for (int i = 0; i < _2295.Length; i++)
		{
			if (_2295[i]._22AC.Equals((object?)ԝ))
			{
				return i;
			}
		}
		return -1;
	}

	protected virtual void OnMouseOverColorChanged(EventArgs e)
	{
		_229B?.Invoke(this, e);
	}

	private void _0ECC(int _22AA)
	{
		if (_22AA != _2299)
		{
			if (_2299 >= 0)
			{
				_2295[_2299].ज़ = false;
				((Control)this).Invalidate(_2295[_2299].ঋ);
			}
			_2299 = _22AA;
			if (_2299 >= 0)
			{
				_2295[_2299].ज़ = true;
				((Control)this).Invalidate(_2295[_2299].ঋ);
			}
			OnMouseOverColorChanged(EventArgs.Empty);
		}
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
		_0ECC(-1);
	}

	private int ڍ(int و, int ى)
	{
		for (int i = 0; i < _2295.Length; i++)
		{
			if (_2295[i].ঋ.Contains(و, ى))
			{
				return i;
			}
		}
		return -1;
	}
}
