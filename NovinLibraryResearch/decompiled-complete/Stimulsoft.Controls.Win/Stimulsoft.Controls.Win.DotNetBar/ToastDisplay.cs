using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Controls.Win.DotNetBar.TextMarkup;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public class ToastDisplay : Control
{
	private const int GlowSize = 4;

	private readonly Size TextPadding = new Size(4, 4);

	private int _Alpha = 255;

	private Image _BackgroundImage;

	private BodyElement _TextMarkup;

	private Color _ToastBackColor = Color.FromArgb(7, 7, 7);

	private eToastGlowColor _GlowColor = eToastGlowColor.Blue;

	private Image _Image;

	private eTextFormat TextFormat => eTextFormat.VerticalCenter | eTextFormat.WordBreak;

	[Obfuscation(Exclude = true, ApplyToMembers = false)]
	public int Alpha
	{
		get
		{
			return _Alpha;
		}
		set
		{
			if (value != _Alpha)
			{
				if (value < 0)
				{
					value = 0;
				}
				else if (value > 255)
				{
					value = 255;
				}
				_Alpha = value;
				if (_Alpha == 0 && _BackgroundImage != null)
				{
					_BackgroundImage.Dispose();
					_BackgroundImage = null;
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
			return _ToastBackColor;
		}
		set
		{
			_ToastBackColor = value;
		}
	}

	public eToastGlowColor GlowColor
	{
		get
		{
			return _GlowColor;
		}
		set
		{
			_GlowColor = value;
		}
	}

	public Image Image
	{
		get
		{
			return _Image;
		}
		set
		{
			_Image = value;
			((Control)this).Invalidate();
		}
	}

	public ToastDisplay()
	{
		((Control)this).SetStyle((ControlStyles)141314, true);
		((Control)this).ForeColor = Color.White;
	}

	protected override void Dispose(bool disposing)
	{
		if (_BackgroundImage != null)
		{
			_BackgroundImage.Dispose();
			_BackgroundImage = null;
		}
		((Control)this).Dispose(disposing);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Expected O, but got Unknown
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Expected O, but got Unknown
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Invalid comparison between Unknown and I4
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Invalid comparison between Unknown and I4
		BufferedGraphicsContext val = new BufferedGraphicsContext();
		Bitmap val2 = new Bitmap(((Control)this).Width, ((Control)this).Height, e.Graphics);
		try
		{
			Graphics val3 = Graphics.FromImage((Image)(object)val2);
			val3.SmoothingMode = (SmoothingMode)4;
			Color glowColor = GetGlowColor(_GlowColor);
			Rectangle clientRectangle = ((Control)this).ClientRectangle;
			Color toastBackColor = _ToastBackColor;
			clientRectangle.Inflate(-3, -3);
			if (!glowColor.IsEmpty)
			{
				for (int i = 0; i < 4; i++)
				{
					Pen val4 = new Pen(glowColor, 5f);
					try
					{
						val4.LineJoin = (LineJoin)2;
						val3.DrawRectangle(val4, clientRectangle);
					}
					finally
					{
						((IDisposable)val4)?.Dispose();
					}
					clientRectangle.Inflate(-1, -1);
				}
			}
			clientRectangle = ((Control)this).ClientRectangle;
			clientRectangle.Inflate(-4, -4);
			Pen val5 = new Pen(_ToastBackColor, 5f);
			try
			{
				val5.LineJoin = (LineJoin)2;
				clientRectangle.Inflate(-3, -3);
				val3.DrawRectangle(val5, clientRectangle);
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
			DisplayHelp.FillRectangle(val3, clientRectangle, _ToastBackColor);
			clientRectangle = ((Control)this).ClientRectangle;
			Size textPadding = TextPadding;
			int width = -(textPadding.Width + 4);
			textPadding = TextPadding;
			clientRectangle.Inflate(width, -(textPadding.Height + 4));
			if (_Image != null)
			{
				val3.DrawImage(_Image, clientRectangle.Location);
				int x = clientRectangle.X;
				int width2 = _Image.Width;
				textPadding = TextPadding;
				clientRectangle.X = x + (width2 + textPadding.Width);
				int width3 = clientRectangle.Width;
				int width4 = _Image.Width;
				textPadding = TextPadding;
				clientRectangle.Width = width3 - (width4 + textPadding.Width);
			}
			if (clientRectangle.Width > 1)
			{
				if (_TextMarkup == null)
				{
					eTextFormat textFormat = TextFormat;
					if ((int)((Control)this).RightToLeft == 1)
					{
						textFormat |= eTextFormat.RightToLeft;
					}
					TextRenderer.DrawText((IDeviceContext)(object)val3, ((Control)this).Text, ((Control)this).Font, clientRectangle, ((Control)this).ForeColor, _ToastBackColor);
				}
				else
				{
					MarkupDrawContext d = new MarkupDrawContext(val3, ((Control)this).Font, ((Control)this).ForeColor, (int)((Control)this).RightToLeft == 1, clientRectangle, hotKeyPrefixVisible: true);
					_TextMarkup.Arrange(clientRectangle, d);
					_TextMarkup.Render(d);
				}
			}
			val3.Dispose();
			if (_Alpha == 255)
			{
				e.Graphics.DrawImage((Image)(object)val2, 0, 0);
			}
			else
			{
				ColorMatrix val6 = new ColorMatrix();
				val6.Matrix33 = (float)_Alpha / 255f;
				ImageAttributes val7 = new ImageAttributes();
				try
				{
					val7.SetColorMatrix(val6);
					e.Graphics.DrawImage((Image)(object)val2, new Rectangle(0, 0, ((Image)val2).Width, ((Image)val2).Height), 0, 0, ((Image)val2).Width, ((Image)val2).Height, (GraphicsUnit)2, val7);
				}
				finally
				{
					((IDisposable)val7)?.Dispose();
				}
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		((Control)this).OnPaint(e);
	}

	internal Size DesiredSize(Padding margin, Rectangle parentBounds)
	{
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Invalid comparison between Unknown and I4
		parentBounds.Width -= margin.Horizontal;
		parentBounds.Height -= margin.Vertical;
		int width = parentBounds.Width;
		Size textPadding = TextPadding;
		parentBounds.Width = width - (4 + textPadding.Width * 2);
		int height = parentBounds.Height;
		textPadding = TextPadding;
		parentBounds.Height = height - (4 + textPadding.Height * 2);
		textPadding = TextPadding;
		int width2 = 8 + textPadding.Width * 2;
		textPadding = TextPadding;
		Size result = new Size(width2, 8 + textPadding.Height * 2);
		Graphics val = ((Control)this).CreateGraphics();
		try
		{
			if (_TextMarkup != null)
			{
				MarkupDrawContext d = new MarkupDrawContext(val, ((Control)this).Font, Color.Black, (int)((Control)this).RightToLeft == 1);
				_TextMarkup.Measure(new Size(parentBounds.Width, 1), d);
				Size size = _TextMarkup.Bounds.Size;
				_TextMarkup.Arrange(new Rectangle(Point.Empty, new Size(parentBounds.Width, 1)), d);
				size = _TextMarkup.Bounds.Size;
				if (_Image != null)
				{
					int width3 = size.Width;
					int width4 = _Image.Width;
					textPadding = TextPadding;
					size.Width = width3 + (width4 + textPadding.Width);
					size.Height = Math.Max(_Image.Height, size.Height);
				}
				result.Width += size.Width;
				result.Height += size.Height;
			}
			else
			{
				Size size2 = TextRenderer.MeasureText(((Control)this).Text, ((Control)this).Font, parentBounds.Size, (TextFormatFlags)16);
				if (_Image != null)
				{
					int width5 = size2.Width;
					int width6 = _Image.Width;
					textPadding = TextPadding;
					size2.Width = width5 + (width6 + textPadding.Width);
					size2.Height = Math.Max(_Image.Height, size2.Height);
				}
				result.Width += size2.Width;
				result.Height += size2.Height;
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		return result;
	}

	private Color GetGlowColor(eToastGlowColor glowColor)
	{
		return glowColor switch
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
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Expected O, but got Unknown
		if (_BackgroundImage != null)
		{
			pevent.Graphics.DrawImage(_BackgroundImage, Point.Empty);
			if (_Alpha == 255)
			{
				_BackgroundImage.Dispose();
				_BackgroundImage = null;
			}
		}
		else if (_Alpha != 255 && _BackgroundImage == null && ((Control)this).Width > 0 && ((Control)this).Height > 0)
		{
			_BackgroundImage = (Image)new Bitmap(((Control)this).Width, ((Control)this).Height, pevent.Graphics);
			Graphics val = Graphics.FromImage(_BackgroundImage);
			try
			{
				PaintEventArgs val2 = new PaintEventArgs(val, pevent.ClipRectangle);
				((Control)this).OnPaintBackground(val2);
				PaintParentControlOnBackground(val);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			pevent.Graphics.DrawImage(_BackgroundImage, Point.Empty);
		}
		else
		{
			((Control)this).OnPaintBackground(pevent);
			PaintParentControlOnBackground(pevent.Graphics);
		}
	}

	private void PaintParentControlOnBackground(Graphics g)
	{
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Expected O, but got Unknown
		if (((Control)this).Parent == null)
		{
			return;
		}
		for (int num = ((ArrangedElementCollection)((Control)this).Parent.Controls).Count - 1; num >= 0; num--)
		{
			Control val = ((Control)this).Parent.Controls[num];
			if (!(val is ToastDisplay) && val.Visible && val.Bounds.IntersectsWith(((Control)this).Bounds) && val.Bounds.Width > 0 && val.Bounds.Height > 0)
			{
				Bitmap val2 = new Bitmap(val.Width, val.Height, g);
				try
				{
					val.DrawToBitmap(val2, val.ClientRectangle);
					g.TranslateTransform((float)(val.Left - ((Control)this).Left), (float)(val.Top - ((Control)this).Top));
					g.DrawImage((Image)(object)val2, Point.Empty);
					g.TranslateTransform((float)(((Control)this).Left - val.Left), (float)(((Control)this).Top - val.Top));
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
		string text = ((Control)this).Text;
		if (MarkupParser.IsMarkup(ref text))
		{
			_TextMarkup = MarkupParser.Parse(text);
		}
		else
		{
			_TextMarkup = null;
		}
		((Control)this).Invalidate();
		((Control)this).OnTextChanged(e);
	}
}
