using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
[ToolboxBitmap(typeof(StiButton), "Toolbox.StiButton.bmp")]
public class StiButton : Button
{
	protected bool isMouseOver;

	protected bool isPressed;

	private bool wordWrap = true;

	private Color buttonColor = SystemColors.Control;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public FlatStyle FlatStyle
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((ButtonBase)this).FlatStyle;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((ButtonBase)this).FlatStyle = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Image BackgroundImage
	{
		get
		{
			return ((Control)this).BackgroundImage;
		}
		set
		{
			((Control)this).BackgroundImage = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Color BackColor
	{
		get
		{
			return ((ButtonBase)this).BackColor;
		}
		set
		{
			((ButtonBase)this).BackColor = value;
		}
	}

	[Category("Behavior")]
	[DefaultValue(true)]
	public bool WordWrap
	{
		get
		{
			return wordWrap;
		}
		set
		{
			wordWrap = value;
			((Control)this).Invalidate();
		}
	}

	[Category("Appearance")]
	[DefaultValue(true)]
	public Color ButtonColor
	{
		get
		{
			return buttonColor;
		}
		set
		{
			buttonColor = value;
			((Control)this).Invalidate();
		}
	}

	public static void DrawButton(Graphics g, Rectangle rect, string text, Font font, Color foreColor, Color backColor, ImageList imageList, int imageIndex, Image image, RightToLeft rightToLeft)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		DrawButton(g, rect, text, font, foreColor, backColor, imageList, imageIndex, image, rightToLeft, wordWrap: true, isEnabled: true, isMouseOver: false, isPressed: false, isDefault: false, isFocused: false, (ContentAlignment)32, (ContentAlignment)32);
	}

	public static void DrawButton(Graphics g, Rectangle rect, string text, Font font, Color foreColor, Color backColor, ImageList imageList, int imageIndex, Image image, RightToLeft rightToLeft, bool wordWrap, bool isEnabled, bool isMouseOver, bool isPressed, bool isDefault, bool isFocused, ContentAlignment imageAlign, ContentAlignment textAlign)
	{
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Expected O, but got Unknown
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Expected O, but got Unknown
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		Rectangle rc = new Rectangle(rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
		Color color = StiColorUtils.Light(backColor, 30);
		Color color2 = StiColorUtils.Dark(backColor, 10);
		Color color3 = StiColorUtils.Light(color, 20);
		Color color4 = StiColorUtils.Light(color2, 20);
		Color color5 = StiColorUtils.Dark(color, 20);
		Color color6 = StiColorUtils.Dark(color2, 20);
		Color color7 = StiColorUtils.Dark(color, 20);
		Color color8 = StiColorUtils.Dark(color2, 20);
		Color color9 = color;
		Color color10 = color2;
		if (isMouseOver)
		{
			color9 = color3;
			color10 = color4;
		}
		if (isPressed)
		{
			color9 = color5;
			color10 = color6;
		}
		Rectangle rectangle = new Rectangle(rc.X + 1, rc.Y + 1, rc.Width - 1, rc.Height - 1);
		LinearGradientBrush val = new LinearGradientBrush(rectangle, color7, color8, 90f);
		try
		{
			g.FillRectangle((Brush)(object)val, rectangle);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		Rectangle rectangle2 = new Rectangle(rc.X + 1, rc.Y + 1, rc.Width - 2, rc.Height - 2);
		LinearGradientBrush val2 = new LinearGradientBrush(rectangle2, color9, color10, 90f);
		try
		{
			g.FillRectangle((Brush)(object)val2, rectangle2);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		SmoothingMode smoothingMode = g.SmoothingMode;
		g.SmoothingMode = (SmoothingMode)4;
		Pen val3 = new Pen(StiColorUtils.Dark(SystemColors.ControlDark, 30));
		try
		{
			DrawRoundedRectangle(g, val3, rc, new Size(4, 4));
			if (isDefault)
			{
				g.DrawRectangle(val3, new Rectangle(rc.X + 1, rc.Y + 1, rc.Width - 2, rc.Height - 2));
			}
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
		g.SmoothingMode = smoothingMode;
		if (isFocused)
		{
			Rectangle rectangle3 = new Rectangle(rect.X + 4, rect.Y + 4, rect.Width - 7, rect.Height - 7);
			ControlPaint.DrawFocusRectangle(g, rectangle3, SystemColors.ControlDark, SystemColors.Control);
		}
		CalcRectangles(out var textRect, out var imageRect, new Rectangle(rect.X + 4, rect.Y + 4, rect.Width - 8, rect.Height - 8), g, isEnabled, imageList, imageIndex, image, text, wordWrap, rightToLeft, foreColor, backColor, font, imageAlign, textAlign, isPressed);
		try
		{
			DrawImage(g, isEnabled, imageRect, imageList, imageIndex, image);
		}
		catch
		{
		}
		DrawText(g, text, wordWrap, rightToLeft, isEnabled, foreColor, textRect, font, textAlign, imageAlign);
	}

	private static void CalcRectangles(out Rectangle textRect, out Rectangle imageRect, Rectangle rect, Graphics g, bool isEnabled, ImageList imageList, int imageIndex, Image image, string text, bool wordWrap, RightToLeft rightToLeft, Color foreColor, Color backColor, Font font, ContentAlignment imageAlign, ContentAlignment textAlign, bool isPressed)
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Invalid comparison between Unknown and I4
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Invalid comparison between Unknown and I4
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Invalid comparison between Unknown and I4
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Invalid comparison between Unknown and I4
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Expected I4, but got Unknown
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Invalid comparison between Unknown and I4
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Invalid comparison between Unknown and I4
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Invalid comparison between Unknown and I4
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Invalid comparison between Unknown and I4
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Invalid comparison between Unknown and I4
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Invalid comparison between Unknown and I4
		Size size = Size.Empty;
		if (imageList != null && imageIndex >= 0 && imageIndex < imageList.Images.Count)
		{
			size = imageList.ImageSize;
		}
		try
		{
			if (size.IsEmpty && image != null)
			{
				size = new Size(image.Width, image.Height);
			}
		}
		catch
		{
			size = new Size(16, 16);
		}
		StringFormat stringFormat = GetStringFormat(wordWrap, rightToLeft, textAlign, imageAlign);
		SizeF sizeF;
		try
		{
			sizeF = g.MeasureString(text, font, new SizeF(rect.Width, rect.Height), stringFormat);
		}
		finally
		{
			((IDisposable)stringFormat)?.Dispose();
		}
		textRect = new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
		imageRect = new Rectangle(rect.X, rect.Y, size.Width, size.Height);
		if ((int)textAlign == 32 && (int)imageAlign == 32)
		{
			textRect.X += size.Width;
			textRect.Width -= size.Width;
			int num = (int)(((float)(rect.Width - size.Width) - sizeF.Width) / 2f);
			imageRect.X = rect.X + num;
			imageRect.Y = rect.Y + (rect.Height - size.Height) / 2;
		}
		else if ((int)imageAlign <= 32)
		{
			switch (imageAlign - 1)
			{
			default:
				if ((int)imageAlign != 16)
				{
					if ((int)imageAlign == 32)
					{
						imageRect.X = rect.X + (rect.Width - size.Width) / 2;
						imageRect.Y = rect.Y + (rect.Height - size.Height) / 2;
					}
				}
				else
				{
					imageRect.X = rect.X;
					imageRect.Y = rect.Y + (rect.Height - size.Height) / 2;
				}
				break;
			case 0:
				imageRect.X = rect.X;
				imageRect.Y = rect.Y;
				break;
			case 1:
				imageRect.X = rect.X + (rect.Width - size.Width) / 2;
				imageRect.Y = rect.Y;
				break;
			case 3:
				imageRect.X = rect.Right - size.Width;
				imageRect.Y = rect.Y;
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
					imageRect.X = rect.X;
					imageRect.Y = rect.Bottom - size.Height;
				}
			}
			else
			{
				imageRect.X = rect.Right - size.Width;
				imageRect.Y = rect.Y + (rect.Height - size.Height) / 2;
			}
		}
		else if ((int)imageAlign != 512)
		{
			if ((int)imageAlign == 1024)
			{
				imageRect.X = rect.Right - size.Width;
				imageRect.Y = rect.Bottom - size.Height;
			}
		}
		else
		{
			imageRect.X = rect.X + (rect.Width - size.Width) / 2;
			imageRect.Y = rect.Bottom - size.Height;
		}
		if (isPressed)
		{
			textRect.X++;
			textRect.Y++;
			imageRect.X++;
			imageRect.Y++;
		}
	}

	private static void DrawImage(Graphics g, bool isEnabled, Rectangle imageRect, ImageList imageList, int imageIndex, Image image)
	{
		if ((imageList == null || imageIndex < 0 || imageIndex >= imageList.Images.Count) && image == null)
		{
			return;
		}
		if (imageList != null && imageIndex >= 0 && imageIndex < imageList.Images.Count)
		{
			if (isEnabled)
			{
				imageList.Draw(g, imageRect.X, imageRect.Y, imageRect.Width, imageRect.Height, imageIndex);
			}
			else
			{
				StiControlPaint.DrawImageDisabled(g, imageList.Images[imageIndex], imageRect.Left, imageRect.Top);
			}
		}
		else if (isEnabled)
		{
			g.DrawImage(image, imageRect.X, imageRect.Y, imageRect.Width, imageRect.Height);
		}
		else
		{
			StiControlPaint.DrawImageDisabled(g, image, imageRect.Left, imageRect.Top);
		}
	}

	private static StringFormat GetStringFormat(bool wordWrap, RightToLeft rightToLeft, ContentAlignment textAlign, ContentAlignment imageAlign)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Invalid comparison between Unknown and I4
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Invalid comparison between Unknown and I4
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Invalid comparison between Unknown and I4
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Invalid comparison between Unknown and I4
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Invalid comparison between Unknown and I4
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected I4, but got Unknown
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Invalid comparison between Unknown and I4
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Invalid comparison between Unknown and I4
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Invalid comparison between Unknown and I4
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Invalid comparison between Unknown and I4
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Invalid comparison between Unknown and I4
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Invalid comparison between Unknown and I4
		StringFormat val = new StringFormat
		{
			Trimming = (StringTrimming)3,
			FormatFlags = (StringFormatFlags)0,
			HotkeyPrefix = (HotkeyPrefix)1
		};
		if (!wordWrap)
		{
			val.FormatFlags = (StringFormatFlags)4096;
		}
		if ((int)rightToLeft == 1)
		{
			val.FormatFlags = (StringFormatFlags)(val.FormatFlags | 1);
		}
		if ((int)textAlign == 32 && (int)imageAlign == 32)
		{
			val.Alignment = (StringAlignment)1;
			val.LineAlignment = (StringAlignment)1;
		}
		else if ((int)textAlign <= 32)
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
			case 0:
				val.Alignment = (StringAlignment)0;
				val.LineAlignment = (StringAlignment)0;
				break;
			case 1:
				val.Alignment = (StringAlignment)1;
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
		return val;
	}

	private static void DrawText(Graphics g, string text, bool wordWrap, RightToLeft rightToLeft, bool isEnabled, Color foreColor, Rectangle textRect, Font font, ContentAlignment textAlign, ContentAlignment imageAlign)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		if (!isEnabled)
		{
			foreColor = SystemColors.GrayText;
		}
		StringFormat stringFormat = GetStringFormat(wordWrap, rightToLeft, textAlign, imageAlign);
		try
		{
			SolidBrush val = new SolidBrush(foreColor);
			try
			{
				g.DrawString(text, font, (Brush)(object)val, (RectangleF)textRect, stringFormat);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)stringFormat)?.Dispose();
		}
	}

	private static void DrawRoundedRectangle(Graphics g, Pen p, Rectangle rc, Size size)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		SmoothingMode smoothingMode = g.SmoothingMode;
		g.SmoothingMode = (SmoothingMode)4;
		g.DrawLine(p, rc.Left + size.Width / 2, rc.Top, rc.Right - size.Width / 2, rc.Top);
		g.DrawArc(p, rc.Right - size.Width, rc.Top, size.Width, size.Height, 270, 90);
		g.DrawLine(p, rc.Right, rc.Top + size.Height / 2, rc.Right, rc.Bottom - size.Height / 2);
		g.DrawArc(p, rc.Right - size.Width, rc.Bottom - size.Height, size.Width, size.Height, 0, 90);
		g.DrawLine(p, rc.Right - size.Width / 2, rc.Bottom, rc.Left + size.Width / 2, rc.Bottom);
		g.DrawArc(p, rc.Left, rc.Bottom - size.Height, size.Width, size.Height, 90, 90);
		g.DrawLine(p, rc.Left, rc.Bottom - size.Height / 2, rc.Left, rc.Top + size.Height / 2);
		g.DrawArc(p, rc.Left, rc.Top, size.Width, size.Height, 180, 90);
		g.SmoothingMode = smoothingMode;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal bool ShouldSerializeButtonColor()
	{
		return ButtonColor != SystemColors.Control;
	}

	protected override void OnPaint(PaintEventArgs p)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		Graphics graphics = p.Graphics;
		Rectangle rect = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		DrawButton(graphics, rect, ((Control)this).Text, ((Control)this).Font, ((Control)this).ForeColor, ButtonColor, ((ButtonBase)this).ImageList, ((ButtonBase)this).ImageIndex, ((ButtonBase)this).Image, ((Control)this).RightToLeft, WordWrap, ((Control)this).Enabled, isMouseOver, isPressed, ((ButtonBase)this).IsDefault, ((Control)this).Focused, ((ButtonBase)this).ImageAlign, ((ButtonBase)this).TextAlign);
	}

	protected override void OnGotFocus(EventArgs e)
	{
		((ButtonBase)this).OnGotFocus(e);
		((Control)this).Invalidate();
	}

	protected override void OnLostFocus(EventArgs e)
	{
		((ButtonBase)this).OnLostFocus(e);
		((Control)this).Invalidate();
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		((Button)this).OnMouseEnter(e);
		isMouseOver = true;
		((Control)this).Invalidate();
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Button)this).OnMouseLeave(e);
		isMouseOver = false;
		((Control)this).Invalidate();
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		((ButtonBase)this).OnMouseDown(e);
		if ((e.Button & 0x100000) > 0)
		{
			isPressed = true;
			((Control)this).Invalidate();
		}
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		((Button)this).OnMouseUp(e);
		if ((e.Button & 0x100000) > 0)
		{
			isPressed = false;
			((Control)this).Invalidate();
		}
	}

	protected override void OnClick(EventArgs e)
	{
		((Button)this).OnClick(e);
	}

	protected override void OnSystemColorsChanged(EventArgs e)
	{
		((Control)this).OnSystemColorsChanged(e);
		StiColors.InitColors();
	}

	public StiButton()
	{
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)2048, true);
		((Control)this).SetStyle((ControlStyles)4, false);
		BackColor = Color.Transparent;
	}
}
