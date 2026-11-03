using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using DevComponents.DotNetBar.TextMarkup;

namespace DevComponents.DotNetBar;

[EditorBrowsable(EditorBrowsableState.Never)]
[ToolboxItem(false)]
public class ToastDisplay : Control
{
	private const int _2FC3 = 4;

	private readonly Size _2FC4 = new Size(4, 4);

	private int _ᗐ = 255;

	private Image _ⴴ;

	private _0CB4 __1A56;

	private Color __2FC5 = Color.FromArgb(7, 7, 7);

	private eToastGlowColor __2FC6 = eToastGlowColor.Blue;

	private PictureBox __2FC7;

	private Image _ነ;

	[CompilerGenerated]
	private eToastPosition? _2FC8;

	private eTextFormat ݽ => eTextFormat.VerticalCenter | eTextFormat.WordBreak;

	[Obfuscation(Exclude = true, ApplyToMembers = false)]
	public int Alpha
	{
		get
		{
			return _ᗐ;
		}
		set
		{
			if (value != _ᗐ)
			{
				if (value < 0)
				{
					value = 0;
				}
				else if (value > 255)
				{
					value = 255;
				}
				_ᗐ = value;
				if (_ᗐ == 0 && _ⴴ != null)
				{
					_ⴴ.Dispose();
					_ⴴ = null;
				}
				if (((Control)this).IsHandleCreated)
				{
					((Control)this).Invalidate();
				}
			}
		}
	}

	public Color ToastBackColor
	{
		get
		{
			return __2FC5;
		}
		set
		{
			__2FC5 = value;
		}
	}

	public eToastGlowColor GlowColor
	{
		get
		{
			return __2FC6;
		}
		set
		{
			__2FC6 = value;
		}
	}

	public Image Image
	{
		get
		{
			return _ነ;
		}
		set
		{
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Expected O, but got Unknown
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Expected O, but got Unknown
			_ነ = value;
			((Control)this).Invalidate();
			bool flag = false;
			if (_ነ != null && ((object)_ነ.RawFormat).Equals((object?)ImageFormat.Gif))
			{
				Guid[] frameDimensionsList = _ነ.FrameDimensionsList;
				Guid[] array = frameDimensionsList;
				foreach (Guid guid in array)
				{
					FrameDimension val = new FrameDimension(guid);
					if (((object)val).Equals((object?)FrameDimension.Time))
					{
						flag = _ነ.GetFrameCount(FrameDimension.Time) > 1;
						break;
					}
				}
			}
			if (flag)
			{
				if (__2FC7 == null)
				{
					__2FC7 = new PictureBox();
					__2FC7.Image = _ነ;
					((Control)this).Controls.Add((Control)(object)__2FC7);
				}
			}
			else if (__2FC7 != null)
			{
				((Control)this).Controls.Remove((Control)(object)__2FC7);
				((Component)(object)__2FC7).Dispose();
			}
		}
	}

	public eToastPosition? ToastPosition
	{
		[CompilerGenerated]
		get
		{
			return _2FC8;
		}
		[CompilerGenerated]
		set
		{
			_2FC8 = value;
		}
	}

	public ToastDisplay()
	{
		((Control)this).SetStyle((ControlStyles)141314, true);
		((Control)this).ForeColor = Color.White;
	}

	protected override void Dispose(bool disposing)
	{
		if (_ⴴ != null)
		{
			_ⴴ.Dispose();
			_ⴴ = null;
		}
		((Control)this).Dispose(disposing);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Expected O, but got Unknown
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Expected O, but got Unknown
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Invalid comparison between Unknown and I4
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Invalid comparison between Unknown and I4
		new BufferedGraphicsContext();
		Bitmap val = new Bitmap(((Control)this).Width, ((Control)this).Height, e.Graphics);
		try
		{
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			val2.SmoothingMode = (SmoothingMode)4;
			Color color = _2FCC(__2FC6);
			Rectangle clientRectangle = ((Control)this).ClientRectangle;
			clientRectangle.Inflate(-3, -3);
			if (!color.IsEmpty)
			{
				for (int i = 0; i < 4; i++)
				{
					Pen val3 = new Pen(color, 5f);
					try
					{
						val3.LineJoin = (LineJoin)2;
						val2.DrawRectangle(val3, clientRectangle);
					}
					finally
					{
						((IDisposable)val3)?.Dispose();
					}
					clientRectangle.Inflate(-1, -1);
				}
			}
			clientRectangle = ((Control)this).ClientRectangle;
			clientRectangle.Inflate(-4, -4);
			Pen val4 = new Pen(__2FC5, 5f);
			try
			{
				val4.LineJoin = (LineJoin)2;
				clientRectangle.Inflate(-3, -3);
				val2.DrawRectangle(val4, clientRectangle);
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
			DisplayHelp.FillRectangle(val2, clientRectangle, __2FC5);
			clientRectangle = ((Control)this).ClientRectangle;
			Size size = _2FC4;
			int width = -(size.Width + 4);
			Size size2 = _2FC4;
			clientRectangle.Inflate(width, -(size2.Height + 4));
			if (_ነ != null)
			{
				if (__2FC7 != null)
				{
					((Control)__2FC7).Bounds = new Rectangle(clientRectangle.Location, _ነ.Size);
				}
				else
				{
					val2.DrawImage(_ነ, clientRectangle.Location);
				}
				int x = clientRectangle.X;
				int width2 = _ነ.Width;
				Size size3 = _2FC4;
				clientRectangle.X = x + (width2 + size3.Width);
				int width3 = clientRectangle.Width;
				int width4 = _ነ.Width;
				Size size4 = _2FC4;
				clientRectangle.Width = width3 - (width4 + size4.Width);
			}
			if (clientRectangle.Width > 1)
			{
				if (__1A56 == null)
				{
					eTextFormat eTextFormat2 = ݽ;
					if ((int)((Control)this).RightToLeft == 1)
					{
						eTextFormat2 |= eTextFormat.RightToLeft;
					}
					TextRenderer.DrawText((IDeviceContext)(object)val2, ((Control)this).Text, ((Control)this).Font, clientRectangle, ((Control)this).ForeColor, __2FC5);
				}
				else
				{
					MarkupDrawContext markupDrawContext = new MarkupDrawContext(val2, ((Control)this).Font, ((Control)this).ForeColor, (int)((Control)this).RightToLeft == 1, clientRectangle, hotKeyPrefixVisible: true);
					__1A56._0CC6(clientRectangle, markupDrawContext);
					__1A56.Render(markupDrawContext);
				}
			}
			val2.Dispose();
			if (_ᗐ == 255)
			{
				e.Graphics.DrawImage((Image)(object)val, 0, 0);
			}
			else
			{
				ColorMatrix val5 = new ColorMatrix();
				val5.Matrix33 = (float)_ᗐ / 255f;
				ImageAttributes val6 = new ImageAttributes();
				try
				{
					val6.SetColorMatrix(val5);
					e.Graphics.DrawImage((Image)(object)val, new Rectangle(0, 0, ((Image)val).Width, ((Image)val).Height), 0, 0, ((Image)val).Width, ((Image)val).Height, (GraphicsUnit)2, val6);
				}
				finally
				{
					((IDisposable)val6)?.Dispose();
				}
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		((Control)this).OnPaint(e);
	}

	internal Size _2FC9(Padding _2FCA, Rectangle _2FCB)
	{
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Invalid comparison between Unknown and I4
		_2FCB.Width -= _2FCA.Horizontal;
		_2FCB.Height -= _2FCA.Vertical;
		int width = _2FCB.Width;
		Size size = _2FC4;
		_2FCB.Width = width - (4 + size.Width * 2);
		int height = _2FCB.Height;
		Size size2 = _2FC4;
		_2FCB.Height = height - (4 + size2.Height * 2);
		Size size3 = _2FC4;
		int width2 = 8 + size3.Width * 2;
		Size size4 = _2FC4;
		Size result = new Size(width2, 8 + size4.Height * 2);
		Graphics val = ((Control)this).CreateGraphics();
		try
		{
			if (__1A56 != null)
			{
				MarkupDrawContext markupDrawContext = new MarkupDrawContext(val, ((Control)this).Font, Color.Black, (int)((Control)this).RightToLeft == 1);
				__1A56.Measure(new Size(_2FCB.Width, 1), markupDrawContext);
				Size size5 = __1A56.Bounds.Size;
				__1A56._0CC6(new Rectangle(Point.Empty, new Size(_2FCB.Width, 1)), markupDrawContext);
				size5 = __1A56.Bounds.Size;
				if (_ነ != null)
				{
					int width3 = size5.Width;
					int width4 = _ነ.Width;
					Size size6 = _2FC4;
					size5.Width = width3 + (width4 + size6.Width);
					size5.Height = Math.Max(_ነ.Height, size5.Height);
				}
				result.Width += size5.Width;
				result.Height += size5.Height;
			}
			else
			{
				Size size7 = TextRenderer.MeasureText(((Control)this).Text, ((Control)this).Font, _2FCB.Size, (TextFormatFlags)16);
				if (_ነ != null)
				{
					int width5 = size7.Width;
					int width6 = _ነ.Width;
					Size size8 = _2FC4;
					size7.Width = width5 + (width6 + size8.Width);
					size7.Height = Math.Max(_ነ.Height, size7.Height);
				}
				result.Width += size7.Width;
				result.Height += size7.Height;
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		return result;
	}

	private Color _2FCC(eToastGlowColor _2FCD)
	{
		return _2FCD switch
		{
			eToastGlowColor.Blue => ColorScheme.GetColor(32, 28864), 
			eToastGlowColor.Green => Color.FromArgb(32, Color.Green), 
			eToastGlowColor.Orange => Color.FromArgb(32, Color.Orange), 
			eToastGlowColor.Red => Color.FromArgb(32, Color.Red), 
			eToastGlowColor.None => Color.Empty, 
			eToastGlowColor.Custom => ToastNotification.CustomGlowColor, 
			_ => Color.FromArgb(50, Color.Red), 
		};
	}

	protected override void OnPaintBackground(PaintEventArgs pevent)
	{
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		if (_ⴴ != null)
		{
			pevent.Graphics.DrawImage(_ⴴ, Point.Empty);
			if (_ᗐ == 255)
			{
				_ⴴ.Dispose();
				_ⴴ = null;
			}
		}
		else if (_ᗐ != 255 && _ⴴ == null && ((Control)this).Width > 0 && ((Control)this).Height > 0)
		{
			_ⴴ = (Image)new Bitmap(((Control)this).Width, ((Control)this).Height, pevent.Graphics);
			Graphics val = Graphics.FromImage(_ⴴ);
			try
			{
				PaintEventArgs val2 = new PaintEventArgs(val, pevent.ClipRectangle);
				((Control)this).OnPaintBackground(val2);
				_2481(val);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			pevent.Graphics.DrawImage(_ⴴ, Point.Empty);
		}
		else
		{
			((Control)this).OnPaintBackground(pevent);
			_2481(pevent.Graphics);
		}
	}

	private void _2481(Graphics _٥)
	{
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		if (((Control)this).Parent == null)
		{
			return;
		}
		for (int num = ((ArrangedElementCollection)((Control)this).Parent.Controls).Count - 1; num >= 0; num--)
		{
			Control val = ((Control)this).Parent.Controls[num];
			if (!(val is ToastDisplay) && val.Visible && val.Bounds.IntersectsWith(((Control)this).Bounds) && val.Bounds.Width > 0 && val.Bounds.Height > 0)
			{
				Bitmap val2 = new Bitmap(val.Width, val.Height, _٥);
				try
				{
					val.DrawToBitmap(val2, val.ClientRectangle);
					_٥.TranslateTransform((float)(val.Left - ((Control)this).Left), (float)(val.Top - ((Control)this).Top));
					_٥.DrawImage((Image)(object)val2, Point.Empty);
					_٥.TranslateTransform((float)(((Control)this).Left - val.Left), (float)(((Control)this).Top - val.Top));
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
		}
	}

	protected override void OnTextChanged(EventArgs e)
	{
		string _06EA = ((Control)this).Text;
		if (ሒ.መ(ref _06EA))
		{
			__1A56 = ሒ.ሕ(_06EA);
		}
		else
		{
			__1A56 = null;
		}
		((Control)this).Invalidate();
		((Control)this).OnTextChanged(e);
	}
}
