using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BehComponents;

[Description("ButtonX v1")]
[ToolboxItem(true)]
[ToolboxBitmap(typeof(ButtonX), "ButtonX.bmp")]
public class ButtonX : Button
{
	public enum ImageSizeModes
	{
		Normal,
		Stretch,
		Zoom
	}

	public enum ButtonStyles
	{
		System,
		Simple,
		Blue,
		Green
	}

	private bool _bMouseIn;

	private bool _bMouseDown;

	private bool _bEnterDown;

	private bool _bGotFocused;

	private int _iContentMargin = 2;

	private int _iMargin;

	private Size _ImageFixedSize = new Size(0, 0);

	private ImageSizeModes _ImageSizeMode;

	private bool _Pushed;

	private bool _ShowFirstBorder = true;

	private float _iFirstBorderWidth = 1f;

	private bool _ShowSecondBorder;

	private Color _SecondBorderColor = Color.Red;

	private float _SecondBorderWidth = 1f;

	private float _SecondBorderDistanceToEdge = 2f;

	private ButtonStyles _Style = ButtonStyles.Blue;

	private bool _IsTrailing;

	private Color _TrailingForeColor = Color.LightGray;

	private bool _IsBolded;

	private Color _BoldedForeColor = Color.Blue;

	private bool _IsHoliday;

	private Color _HolidayForeColor = Color.Red;

	private Rectangle _ButtonBound;

	[Browsable(false)]
	public string Programmer => "Saeed Mohammadi";

	[Browsable(false)]
	public string ProgrammerWebSite => "http://BehSoft.blogfa.com/";

	public bool IsTrailing
	{
		get
		{
			return _IsTrailing;
		}
		set
		{
			if (_IsTrailing != value)
			{
				_IsTrailing = value;
				((Control)this).Invalidate();
			}
		}
	}

	public Color TrailingForeColor
	{
		get
		{
			return _TrailingForeColor;
		}
		set
		{
			if (!(_TrailingForeColor == value))
			{
				_TrailingForeColor = value;
				((Control)this).Invalidate();
			}
		}
	}

	public bool IsBolded
	{
		get
		{
			return _IsBolded;
		}
		set
		{
			if (_IsBolded != value)
			{
				_IsBolded = value;
				((Control)this).Invalidate();
			}
		}
	}

	public Color BoldedForeColor
	{
		get
		{
			return _BoldedForeColor;
		}
		set
		{
			if (!(_BoldedForeColor == value))
			{
				_BoldedForeColor = value;
				((Control)this).Invalidate();
			}
		}
	}

	public bool IsHoliday
	{
		get
		{
			return _IsHoliday;
		}
		set
		{
			if (_IsHoliday != value)
			{
				_IsHoliday = value;
				((Control)this).Invalidate();
			}
		}
	}

	public Color HolidayForeColor
	{
		get
		{
			return _HolidayForeColor;
		}
		set
		{
			if (!(_HolidayForeColor == value))
			{
				_HolidayForeColor = value;
				((Control)this).Invalidate();
			}
		}
	}

	public Size ImageFixedSize
	{
		get
		{
			return _ImageFixedSize;
		}
		set
		{
			if (!(_ImageFixedSize == value))
			{
				_ImageFixedSize = value;
				((Control)this).Invalidate();
			}
		}
	}

	public ImageSizeModes ImageSizeMode
	{
		get
		{
			return _ImageSizeMode;
		}
		set
		{
			if (_ImageSizeMode != value)
			{
				_ImageSizeMode = value;
				((Control)this).Invalidate();
			}
		}
	}

	public bool PushedAlways
	{
		get
		{
			return _Pushed;
		}
		set
		{
			if (_Pushed != value)
			{
				_Pushed = value;
				((Control)this).Invalidate();
			}
		}
	}

	public bool ShowSecondBorder
	{
		get
		{
			return _ShowSecondBorder;
		}
		set
		{
			if (_ShowSecondBorder != value)
			{
				_ShowSecondBorder = value;
				((Control)this).Invalidate();
			}
		}
	}

	public Color SecondBorderColor
	{
		get
		{
			return _SecondBorderColor;
		}
		set
		{
			if (!(_SecondBorderColor == value))
			{
				_SecondBorderColor = value;
				((Control)this).Invalidate();
			}
		}
	}

	public float SecondBorderWidth
	{
		get
		{
			return _SecondBorderWidth;
		}
		set
		{
			if (_SecondBorderWidth != value)
			{
				_SecondBorderWidth = value;
				((Control)this).Invalidate();
			}
		}
	}

	public float SecondBorderDistanceToEdge
	{
		get
		{
			return _SecondBorderDistanceToEdge;
		}
		set
		{
			if (_SecondBorderDistanceToEdge != value)
			{
				_SecondBorderDistanceToEdge = value;
				((Control)this).Invalidate();
			}
		}
	}

	public ButtonStyles Style
	{
		get
		{
			return _Style;
		}
		set
		{
			if (_Style != value)
			{
				_Style = value;
				((Control)this).Invalidate();
			}
		}
	}

	public bool ShowFirstBorder
	{
		get
		{
			return _ShowFirstBorder;
		}
		set
		{
			if (_ShowFirstBorder != value)
			{
				_ShowFirstBorder = value;
				((Control)this).Invalidate();
			}
		}
	}

	public ButtonX()
	{
		InitializeComponent();
	}

	public ButtonX(ButtonX button)
	{
		InitializeComponent();
		InitNewVars(button);
	}

	private void InitNewVars(ButtonX button)
	{
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		_BoldedForeColor = button._BoldedForeColor;
		_HolidayForeColor = button._HolidayForeColor;
		_iFirstBorderWidth = button._iFirstBorderWidth;
		_ImageFixedSize = button._ImageFixedSize;
		_ImageSizeMode = button._ImageSizeMode;
		_iContentMargin = button._iContentMargin;
		_IsBolded = button._IsBolded;
		_IsHoliday = button._IsHoliday;
		_IsTrailing = button._IsTrailing;
		_Pushed = button._Pushed;
		_SecondBorderColor = button._SecondBorderColor;
		_SecondBorderDistanceToEdge = button._SecondBorderDistanceToEdge;
		_SecondBorderWidth = button._SecondBorderWidth;
		_ShowFirstBorder = button._ShowFirstBorder;
		_ShowSecondBorder = button._ShowSecondBorder;
		_Style = button._Style;
		_TrailingForeColor = button._TrailingForeColor;
		((Control)this).BackColor = ((Control)button).BackColor;
		((Control)this).Enabled = ((Control)button).Enabled;
		((Control)this).Font = ((Control)button).Font;
		((Control)this).ForeColor = ((Control)button).ForeColor;
		((ButtonBase)this).Image = ((ButtonBase)button).Image;
		((ButtonBase)this).ImageAlign = ((ButtonBase)button).ImageAlign;
		((Control)this).Location = ((Control)button).Location;
		((Control)this).RightToLeft = ((Control)button).RightToLeft;
		((Control)this).Size = ((Control)button).Size;
		((Control)this).Text = ((Control)button).Text;
		((ButtonBase)this).TextAlign = ((ButtonBase)button).TextAlign;
		((Control)this).Visible = ((Control)button).Visible;
	}

	private void InitializeComponent()
	{
		_bMouseIn = false;
		_bMouseDown = false;
		_bEnterDown = false;
		_bGotFocused = false;
		_iFirstBorderWidth = 1f;
		_iContentMargin = 2;
		_iMargin = 0;
		_ImageFixedSize = new Size(0, 0);
		_ImageSizeMode = ImageSizeModes.Normal;
		_Pushed = false;
		_ShowSecondBorder = false;
		_SecondBorderColor = Color.Red;
		_SecondBorderWidth = 3f;
		_SecondBorderDistanceToEdge = 3f;
		_Style = ButtonStyles.Blue;
		_ShowFirstBorder = true;
		_ButtonBound = new Rectangle(_iMargin, _iMargin, ((Control)this).Width - 2 * _iMargin, ((Control)this).Height - 2 * _iMargin);
	}

	protected override void OnPaint(PaintEventArgs pevent)
	{
		((ButtonBase)this).OnPaint(pevent);
		_ButtonBound = new Rectangle(_iMargin, _iMargin, ((Control)this).Width - 2 * _iMargin, ((Control)this).Height - 2 * _iMargin);
		PaintButton(pevent.Graphics);
	}

	private void PaintButton(Graphics g)
	{
		if (_Style != 0)
		{
			DrawStyle(g);
			DrawImage(g);
			DrawText(g);
		}
		DrawBorder(g);
	}

	private void DrawStyle(Graphics g)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Expected O, but got Unknown
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Expected O, but got Unknown
		if (_Style != 0)
		{
			Brush val2;
			if (((Control)this).BackColor == Color.Transparent)
			{
				Control val = (Control)(object)this;
				try
				{
					while (val.BackColor == Color.Transparent)
					{
						val = val.Parent;
					}
				}
				catch
				{
				}
				val2 = (Brush)new SolidBrush(val.BackColor);
			}
			else
			{
				val2 = (Brush)new SolidBrush(((Control)this).BackColor);
			}
			g.FillRectangle(val2, 0, 0, ((Control)this).Width, ((Control)this).Height);
			val2.Dispose();
		}
		if (!((Control)this).Enabled)
		{
			Brush val3 = (Brush)new SolidBrush(Color.WhiteSmoke);
			g.FillRectangle(val3, _ButtonBound);
			val3.Dispose();
		}
		else if (_Pushed)
		{
			int iUpColor = 215;
			int iUpColor2 = 170;
			int iUpColor3 = 125;
			int iDownColor = 215;
			int iDownColor2 = 170;
			int iDownColor3 = 125;
			int iColorSeed = 0;
			int iColorSeed2 = 2;
			int iColorSeed3 = 2;
			_DrawStyle(g, iUpColor, iUpColor2, iUpColor3, iDownColor, iDownColor2, iDownColor3, iColorSeed, iColorSeed2, iColorSeed3);
		}
		else if (!_bMouseIn && !_bEnterDown)
		{
			if (_Style == ButtonStyles.Blue)
			{
				int iUpColor = 200;
				int iUpColor2 = 220;
				int iUpColor3 = 255;
				int iDownColor = 174;
				int iDownColor2 = 209;
				int iDownColor3 = 255;
				int iColorSeed = 2;
				int iColorSeed2 = 2;
				int iColorSeed3 = 0;
				_DrawStyle(g, iUpColor, iUpColor2, iUpColor3, iDownColor, iDownColor2, iDownColor3, iColorSeed, iColorSeed2, iColorSeed3);
				return;
			}
			if (_Style == ButtonStyles.Green)
			{
				int iUpColor = 225;
				int iUpColor2 = 227;
				int iUpColor3 = 120;
				int iDownColor = 225;
				int iDownColor2 = 220;
				int iDownColor3 = 0;
				int iColorSeed = 0;
				int iColorSeed2 = 1;
				int iColorSeed3 = 2;
				_DrawStyle(g, iUpColor, iUpColor2, iUpColor3, iDownColor, iDownColor2, iDownColor3, iColorSeed, iColorSeed2, iColorSeed3);
				return;
			}
			Brush val5;
			if (((Control)this).BackColor == Color.Transparent)
			{
				Control val4 = (Control)(object)this;
				try
				{
					while (val4.BackColor == Color.Transparent)
					{
						val4 = val4.Parent;
					}
				}
				catch
				{
				}
				val5 = (Brush)new SolidBrush(val4.BackColor);
			}
			else
			{
				val5 = (Brush)new SolidBrush(((Control)this).BackColor);
			}
			g.FillRectangle(val5, _ButtonBound);
			val5.Dispose();
		}
		else if (_bMouseIn && !_bMouseDown && !_bEnterDown)
		{
			int iUpColor = 255;
			int iUpColor2 = 230;
			int iUpColor3 = 140;
			int iDownColor = 255;
			int iDownColor2 = 215;
			int iDownColor3 = 75;
			int iColorSeed = 0;
			int iColorSeed2 = 1;
			int iColorSeed3 = 1;
			_DrawStyle(g, iUpColor, iUpColor2, iUpColor3, iDownColor, iDownColor2, iDownColor3, iColorSeed, iColorSeed2, iColorSeed3);
		}
		else if (_bMouseDown || _bEnterDown)
		{
			int iUpColor = 253;
			int iUpColor2 = 163;
			int iUpColor3 = 97;
			int iDownColor = 250;
			int iDownColor2 = 140;
			int iDownColor3 = 60;
			int iColorSeed = 0;
			int iColorSeed2 = 1;
			int iColorSeed3 = 1;
			_DrawStyle(g, iUpColor, iUpColor2, iUpColor3, iDownColor, iDownColor2, iDownColor3, iColorSeed, iColorSeed2, iColorSeed3);
		}
	}

	private void _DrawStyle(Graphics g, int iUpColor1, int iUpColor2, int iUpColor3, int iDownColor1, int iDownColor2, int iDownColor3, int iColorSeed1, int iColorSeed2, int iColorSeed3)
	{
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Expected O, but got Unknown
		int width = _ButtonBound.Width;
		int height = _ButtonBound.Height;
		float num = (float)height / 2f / 10f;
		float num2 = _ButtonBound.X;
		float num3 = (float)_ButtonBound.Y + (float)height / 2f - num;
		int num4 = 0;
		do
		{
			if (num3 <= (float)_ButtonBound.Y)
			{
				num4++;
				num3 = _ButtonBound.Y;
			}
			Brush val = (Brush)new SolidBrush(Color.FromArgb(iUpColor1, iUpColor2, iUpColor3));
			g.FillRectangle(val, num2, num3, (float)width, num);
			iUpColor1 += iColorSeed1;
			iUpColor2 += iColorSeed2;
			iUpColor3 += iColorSeed3;
			if (iUpColor1 > 255)
			{
				iUpColor1 = 255;
			}
			if (iUpColor2 > 255)
			{
				iUpColor2 = 255;
			}
			if (iUpColor3 > 255)
			{
				iUpColor3 = 255;
			}
			if (iUpColor1 < 0)
			{
				iUpColor1 = 0;
			}
			if (iUpColor2 < 0)
			{
				iUpColor2 = 0;
			}
			if (iUpColor3 < 0)
			{
				iUpColor3 = 0;
			}
			num3 -= num;
		}
		while (num4 <= 0);
		num3 = (float)_ButtonBound.Y + (float)height / 2f;
		num4 = 0;
		do
		{
			if (num3 >= (float)(_ButtonBound.Y + _ButtonBound.Height))
			{
				num4++;
				num3 = (float)(_ButtonBound.Y + _ButtonBound.Height) - num;
			}
			Brush val2 = (Brush)new SolidBrush(Color.FromArgb(iDownColor1, iDownColor2, iDownColor3));
			g.FillRectangle(val2, num2, num3, (float)width, num);
			iDownColor1 += iColorSeed1;
			iDownColor2 += iColorSeed2;
			iDownColor3 += iColorSeed3;
			if (iDownColor1 > 255)
			{
				iDownColor1 = 255;
			}
			if (iDownColor2 > 255)
			{
				iDownColor2 = 255;
			}
			if (iDownColor3 > 255)
			{
				iDownColor3 = 255;
			}
			if (iDownColor1 < 0)
			{
				iDownColor1 = 0;
			}
			if (iDownColor2 < 0)
			{
				iDownColor2 = 0;
			}
			if (iDownColor3 < 0)
			{
				iDownColor3 = 0;
			}
			num3 += num;
		}
		while (num4 <= 0);
	}

	private void DrawBorder(Graphics g)
	{
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Expected O, but got Unknown
		if (_ShowFirstBorder && _Style != 0)
		{
			Pen val;
			if (((Control)this).Enabled)
			{
				int red = 120;
				int green = 150;
				int blue = 185;
				val = new Pen(Color.FromArgb(red, green, blue), _iFirstBorderWidth);
			}
			else
			{
				int red = 170;
				int green = 170;
				int blue = 170;
				val = new Pen(Color.FromArgb(red, green, blue), _iFirstBorderWidth);
			}
			float num = (float)_ButtonBound.X + _iFirstBorderWidth / 2f;
			float num2 = (float)_ButtonBound.Y + _iFirstBorderWidth / 2f;
			float num3 = (float)_ButtonBound.Width - _iFirstBorderWidth - 0.5f;
			float num4 = (float)_ButtonBound.Height - _iFirstBorderWidth - 0.5f;
			g.DrawRectangle(val, num, num2, num3, num4);
		}
		if (_ShowSecondBorder)
		{
			Pen val2 = ((!((Control)this).Enabled) ? new Pen(Color.Tan, _SecondBorderWidth) : new Pen(_SecondBorderColor, _SecondBorderWidth));
			float num = (float)_ButtonBound.X + _SecondBorderDistanceToEdge + _SecondBorderWidth / 2f;
			float num2 = (float)_ButtonBound.Y + _SecondBorderDistanceToEdge + _SecondBorderWidth / 2f;
			float num3 = (float)_ButtonBound.Width - 2f * _SecondBorderDistanceToEdge - _SecondBorderWidth - 0.5f;
			float num4 = (float)_ButtonBound.Height - 2f * _SecondBorderDistanceToEdge - _SecondBorderWidth - 0.5f;
			g.DrawRectangle(val2, num, num2, num3, num4);
		}
		if (_bGotFocused && ((Control)this).TabStop && _Style != 0)
		{
			float num5 = 2f;
			float num6 = 1f;
			float num = (float)_ButtonBound.X + num5 + num6 / 2f;
			float num2 = (float)_ButtonBound.Y + num5 + num6 / 2f;
			float num3 = (float)_ButtonBound.Width - 2f * num5 - num6 - 0.5f;
			float num4 = (float)_ButtonBound.Height - 2f * num5 - num6 - 0.5f;
			float[] dashPattern = new float[2] { 1f, 1f };
			Pen val3 = new Pen(Brushes.Black, num6);
			val3.DashPattern = dashPattern;
			g.DrawRectangle(val3, num, num2, num3, num4);
		}
	}

	private void DrawImage(Graphics g)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Invalid comparison between Unknown and I4
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Invalid comparison between Unknown and I4
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected I4, but got Unknown
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Invalid comparison between Unknown and I4
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Invalid comparison between Unknown and I4
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Invalid comparison between Unknown and I4
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Invalid comparison between Unknown and I4
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Invalid comparison between Unknown and I4
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Invalid comparison between Unknown and I4
		if (((ButtonBase)this).Image == null)
		{
			return;
		}
		Size imageSize = GetImageSize();
		Rectangle rectangle = default(Rectangle);
		rectangle.Size = imageSize;
		ContentAlignment imageAlign = ((ButtonBase)this).ImageAlign;
		if ((int)imageAlign <= 32)
		{
			switch (imageAlign - 1)
			{
			default:
				if ((int)imageAlign != 16)
				{
					if ((int)imageAlign == 32)
					{
						int num = (_ButtonBound.Width - imageSize.Width - 2 * _iContentMargin) / 2;
						rectangle.X = _ButtonBound.X + _iContentMargin + num;
						num = (_ButtonBound.Height - imageSize.Height - 2 * _iContentMargin) / 2;
						rectangle.Y = _ButtonBound.Y + _iContentMargin + num;
					}
				}
				else
				{
					rectangle.X = _ButtonBound.X + _iContentMargin;
					int num = (_ButtonBound.Height - imageSize.Height - 2 * _iContentMargin) / 2;
					rectangle.Y = _ButtonBound.Y + _iContentMargin + num;
				}
				break;
			case 1:
			{
				int num = (_ButtonBound.Width - imageSize.Width - 2 * _iContentMargin) / 2;
				rectangle.X = _ButtonBound.X + _iContentMargin + num;
				rectangle.Y = _ButtonBound.Y + _iContentMargin;
				break;
			}
			case 0:
				rectangle.X = _ButtonBound.X + _iContentMargin;
				rectangle.Y = _ButtonBound.Y + _iContentMargin;
				break;
			case 3:
				rectangle.X = _ButtonBound.X + _ButtonBound.Width - _iContentMargin - imageSize.Width;
				rectangle.Y = _ButtonBound.Y + _iContentMargin;
				break;
			case 2:
				break;
			}
		}
		else if ((int)imageAlign <= 256)
		{
			if ((int)imageAlign != 64)
			{
				if ((int)imageAlign == 256)
				{
					rectangle.X = _ButtonBound.X + _iContentMargin;
					rectangle.Y = _ButtonBound.Y + _ButtonBound.Height - _iContentMargin - imageSize.Height;
				}
			}
			else
			{
				rectangle.X = _ButtonBound.X + _ButtonBound.Width - _iContentMargin - imageSize.Width;
				int num = (_ButtonBound.Height - imageSize.Height - 2 * _iContentMargin) / 2;
				rectangle.Y = _ButtonBound.Y + _iContentMargin + num;
			}
		}
		else if ((int)imageAlign != 512)
		{
			if ((int)imageAlign == 1024)
			{
				rectangle.X = _ButtonBound.X + _ButtonBound.Width - _iContentMargin - imageSize.Width;
				rectangle.Y = _ButtonBound.Y + _ButtonBound.Height - _iContentMargin - imageSize.Height;
			}
		}
		else
		{
			int num = (_ButtonBound.Width - imageSize.Width - 2 * _iContentMargin) / 2;
			rectangle.X = _ButtonBound.X + _iContentMargin + num;
			rectangle.Y = _ButtonBound.Y + _ButtonBound.Height - _iContentMargin - imageSize.Height;
		}
		if (_ImageSizeMode == ImageSizeModes.Normal)
		{
			g.DrawImageUnscaledAndClipped(((ButtonBase)this).Image, rectangle);
		}
		else if (_ImageSizeMode == ImageSizeModes.Stretch)
		{
			g.DrawImage(((ButtonBase)this).Image, rectangle);
		}
		else if (_ImageSizeMode == ImageSizeModes.Zoom)
		{
			g.DrawImage(((ButtonBase)this).Image, rectangle);
		}
	}

	private Size GetImageSize()
	{
		Size result = _ImageFixedSize;
		if (((ButtonBase)this).Image != null)
		{
			int num = _ButtonBound.Height - 2 * _iContentMargin;
			if (num < 0)
			{
				num = 0;
			}
			int num2 = _ButtonBound.Width - 2 * _iContentMargin;
			if (num2 < 0)
			{
				num2 = 0;
			}
			if (result.Width == 0 && result.Height == 0)
			{
				result.Width = ((ButtonBase)this).Image.Width;
				result.Height = ((ButtonBase)this).Image.Height;
			}
			if (result.Height > num)
			{
				result.Height = num;
			}
			if (result.Width > num2)
			{
				result.Width = num2;
			}
			if (_ImageSizeMode == ImageSizeModes.Zoom)
			{
				Size size = new Size(((ButtonBase)this).Image.Width, ((ButtonBase)this).Image.Height);
				while (size.Width > result.Width || size.Height > result.Height)
				{
					size.Width = (int)((float)size.Width * 0.99f);
					size.Height = (int)((float)size.Height * 0.99f);
				}
				result = size;
			}
		}
		return result;
	}

	private void DrawText(Graphics g)
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Expected O, but got Unknown
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Invalid comparison between Unknown and I4
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Invalid comparison between Unknown and I4
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Invalid comparison between Unknown and I4
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Invalid comparison between Unknown and I4
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected I4, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Invalid comparison between Unknown and I4
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Invalid comparison between Unknown and I4
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Invalid comparison between Unknown and I4
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Invalid comparison between Unknown and I4
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Invalid comparison between Unknown and I4
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Invalid comparison between Unknown and I4
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Invalid comparison between Unknown and I4
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Expected O, but got Unknown
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Invalid comparison between Unknown and I4
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Invalid comparison between Unknown and I4
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Expected O, but got Unknown
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Expected O, but got Unknown
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Invalid comparison between Unknown and I4
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Invalid comparison between Unknown and I4
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Expected O, but got Unknown
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Expected O, but got Unknown
		int num = _ButtonBound.Width - 2 * _iContentMargin;
		int num2 = _ButtonBound.Height - 2 * _iContentMargin;
		RectangleF rectangleF = default(RectangleF);
		rectangleF.X = _ButtonBound.X + _iContentMargin;
		Size imageSize = GetImageSize();
		if (((ButtonBase)this).ImageAlign == ((ButtonBase)this).TextAlign && ((ButtonBase)this).Image != null)
		{
			if ((int)((ButtonBase)this).TextAlign == 256 || (int)((ButtonBase)this).TextAlign == 16 || (int)((ButtonBase)this).TextAlign == 1)
			{
				rectangleF.X = _ButtonBound.X + _iContentMargin + imageSize.Width;
				num -= imageSize.Width;
			}
			else if ((int)((ButtonBase)this).TextAlign == 1024 || (int)((ButtonBase)this).TextAlign == 64 || (int)((ButtonBase)this).TextAlign == 4)
			{
				num -= imageSize.Width;
			}
		}
		rectangleF.Y = _ButtonBound.Y + _iContentMargin;
		rectangleF.Width = num;
		rectangleF.Height = num2;
		StringFormat val = new StringFormat();
		if ((int)((Control)this).RightToLeft == 1)
		{
			val.FormatFlags = (StringFormatFlags)1;
		}
		ContentAlignment textAlign = ((ButtonBase)this).TextAlign;
		if ((int)textAlign <= 32)
		{
			switch (textAlign - 1)
			{
			default:
				if ((int)textAlign != 16)
				{
					if ((int)textAlign == 32)
					{
						val.Alignment = (StringAlignment)1;
						val.LineAlignment = (StringAlignment)1;
					}
				}
				else
				{
					val.Alignment = (StringAlignment)0;
					val.LineAlignment = (StringAlignment)1;
				}
				break;
			case 1:
				val.Alignment = (StringAlignment)1;
				val.LineAlignment = (StringAlignment)0;
				break;
			case 0:
				val.Alignment = (StringAlignment)0;
				val.LineAlignment = (StringAlignment)0;
				break;
			case 3:
				val.Alignment = (StringAlignment)2;
				val.LineAlignment = (StringAlignment)0;
				break;
			case 2:
				break;
			}
		}
		else if ((int)textAlign <= 256)
		{
			if ((int)textAlign != 64)
			{
				if ((int)textAlign == 256)
				{
					val.Alignment = (StringAlignment)0;
					val.LineAlignment = (StringAlignment)2;
				}
			}
			else
			{
				val.Alignment = (StringAlignment)2;
				val.LineAlignment = (StringAlignment)1;
			}
		}
		else if ((int)textAlign != 512)
		{
			if ((int)textAlign == 1024)
			{
				val.Alignment = (StringAlignment)2;
				val.LineAlignment = (StringAlignment)2;
			}
		}
		else
		{
			val.Alignment = (StringAlignment)1;
			val.LineAlignment = (StringAlignment)2;
		}
		Font val2 = ((Control)this).Font;
		if (_IsBolded)
		{
			val2 = new Font(val2, (FontStyle)1);
		}
		Brush val3;
		if (((Control)this).Enabled)
		{
			val3 = (Brush)new SolidBrush(((Control)this).ForeColor);
			if (_IsTrailing)
			{
				val3 = (Brush)new SolidBrush(_TrailingForeColor);
			}
			else if (_IsHoliday)
			{
				val3 = (Brush)new SolidBrush(_HolidayForeColor);
			}
			else if (_IsBolded)
			{
				val3 = (Brush)new SolidBrush(_BoldedForeColor);
			}
		}
		else
		{
			val3 = (Brush)new SolidBrush(Color.FromArgb(150, 150, 150));
		}
		g.DrawString(((Control)this).Text, val2, val3, rectangleF, val);
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		((Button)this).OnMouseEnter(e);
		if (!_bEnterDown)
		{
			_bMouseIn = true;
			((Control)this).Invalidate();
		}
	}

	protected override void OnMouseDown(MouseEventArgs mevent)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		((ButtonBase)this).OnMouseDown(mevent);
		if ((int)mevent.Button == 1048576 && !_bEnterDown)
		{
			_bMouseDown = true;
			((Control)this).Invalidate();
		}
	}

	protected override void OnMouseUp(MouseEventArgs mevent)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		((Button)this).OnMouseUp(mevent);
		if ((int)mevent.Button == 1048576 && !_bEnterDown)
		{
			_bMouseDown = false;
			((Control)this).Invalidate();
		}
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Button)this).OnMouseLeave(e);
		_bMouseIn = false;
		((Control)this).Invalidate();
	}

	protected override void OnGotFocus(EventArgs e)
	{
		((ButtonBase)this).OnGotFocus(e);
		_bGotFocused = true;
		((Control)this).Invalidate();
	}

	protected override void OnLostFocus(EventArgs e)
	{
		((ButtonBase)this).OnLostFocus(e);
		_bGotFocused = false;
		((Control)this).Invalidate();
	}

	protected override void OnKeyDown(KeyEventArgs kevent)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Invalid comparison between Unknown and I4
		if ((int)kevent.KeyCode == 32 || (int)kevent.KeyCode == 13 || (int)kevent.KeyCode == 13)
		{
			_bEnterDown = true;
			((Control)this).Invalidate();
		}
		((ButtonBase)this).OnKeyDown(kevent);
	}

	protected override void OnKeyUp(KeyEventArgs kevent)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Invalid comparison between Unknown and I4
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Invalid comparison between Unknown and I4
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Invalid comparison between Unknown and I4
		((ButtonBase)this).OnKeyUp(kevent);
		if (((int)kevent.KeyCode == 32 || (int)kevent.KeyCode == 13 || (int)kevent.KeyCode == 13) && _bEnterDown)
		{
			_bEnterDown = false;
			((Control)this).Invalidate();
		}
		else if ((int)kevent.KeyCode == 27)
		{
			_bEnterDown = false;
			((Control)this).Invalidate();
		}
	}

	protected override void OnClick(EventArgs e)
	{
		if (!_Pushed)
		{
			((Button)this).OnClick(e);
		}
	}
}
