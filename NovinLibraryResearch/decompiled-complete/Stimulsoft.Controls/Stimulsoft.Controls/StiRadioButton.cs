using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
[ToolboxBitmap(typeof(StiRadioButton), "Toolbox.StiRadioButton.bmp")]
public class StiRadioButton : RadioButton
{
	private bool isMouseOver;

	private bool flat = true;

	[Browsable(false)]
	public ImageList ImageList
	{
		get
		{
			return ((ButtonBase)this).ImageList;
		}
		set
		{
			((ButtonBase)this).ImageList = value;
		}
	}

	[Browsable(false)]
	public int ImageIndex
	{
		get
		{
			return ((ButtonBase)this).ImageIndex;
		}
		set
		{
			((ButtonBase)this).ImageIndex = value;
		}
	}

	[Browsable(false)]
	public Image Image
	{
		get
		{
			return ((ButtonBase)this).Image;
		}
		set
		{
			((ButtonBase)this).Image = value;
		}
	}

	[Browsable(false)]
	public ContentAlignment ImageAlign
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((ButtonBase)this).ImageAlign;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((ButtonBase)this).ImageAlign = value;
		}
	}

	[Browsable(false)]
	public override ContentAlignment TextAlign
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((RadioButton)this).TextAlign;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((RadioButton)this).TextAlign = value;
		}
	}

	[Browsable(false)]
	public ContentAlignment CheckAlign
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((RadioButton)this).CheckAlign;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((RadioButton)this).CheckAlign = value;
		}
	}

	[Browsable(false)]
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

	[DefaultValue(false)]
	[Category("Behavior")]
	public bool Flat
	{
		get
		{
			return flat;
		}
		set
		{
			flat = value;
			((Control)this).Invalidate();
		}
	}

	public static void DrawRadioButton(Graphics g, Rectangle rect, string text, Font font, Color foreColor, Color backColor, RightToLeft rightToLeft, bool isEnabled, bool isMouseOver, bool isFocused, bool isChecked, Appearance appearance)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_033b: Invalid comparison between Unknown and I4
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Expected O, but got Unknown
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Expected O, but got Unknown
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Expected O, but got Unknown
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Invalid comparison between Unknown and I4
		Rectangle empty = Rectangle.Empty;
		Rectangle empty2 = Rectangle.Empty;
		empty2 = new Rectangle(rect.X + 4, rect.Y, rect.Width - 4, rect.Height);
		empty = (((int)rightToLeft != 0) ? new Rectangle(rect.Right - 15, rect.Y + (rect.Height - 12) / 2 - 1, 12, 12) : new Rectangle(rect.X, rect.Y + (rect.Height - 12) / 2 - 1, 12, 12));
		if ((int)appearance == 0)
		{
			if ((int)rightToLeft == 0)
			{
				empty2.X += 10;
				empty2.Width -= 10;
			}
			else
			{
				empty2.Width -= 16;
			}
			SmoothingMode smoothingMode = g.SmoothingMode;
			g.SmoothingMode = (SmoothingMode)4;
			if (isEnabled && (isMouseOver || isFocused))
			{
				g.DrawEllipse(StiPens.SelectedText, empty.X, empty.Y, empty.Width, empty.Height);
			}
			else if (!isEnabled)
			{
				Pen val = new Pen(StiColorUtils.Dark(SystemColors.Control, 30));
				try
				{
					g.DrawEllipse(val, empty.X, empty.Y, empty.Width, empty.Height);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			else
			{
				g.DrawEllipse(SystemPens.ControlDark, empty.X, empty.Y, empty.Width, empty.Height);
			}
			if (isEnabled)
			{
				LinearGradientBrush val2 = new LinearGradientBrush(empty, StiColors.ContentDark, SystemColors.ControlLightLight, 45f);
				try
				{
					g.FillEllipse((Brush)(object)val2, empty.X + 1, empty.Y + 1, 10, 10);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			else
			{
				g.FillEllipse(SystemBrushes.Control, empty.X + 1, empty.Y + 1, 10, 10);
			}
			if (isChecked)
			{
				if (isEnabled)
				{
					g.FillEllipse(Brushes.DarkGray, empty.X + 3, empty.Y + 3, 6, 6);
					g.FillEllipse(Brushes.Black, empty.X + 4, empty.Y + 4, 4, 4);
				}
				else
				{
					g.FillEllipse(Brushes.DarkGray, empty.X + 3, empty.Y + 3, 6, 6);
					g.FillEllipse(Brushes.DimGray, empty.X + 4, empty.Y + 4, 4, 4);
				}
			}
			g.SmoothingMode = smoothingMode;
			if (isFocused)
			{
				Rectangle rectangle = empty2;
				SizeF sizeF = g.MeasureString(text, font);
				rectangle.Width = (int)sizeF.Width;
				rectangle.Y = (rectangle.Height - ((int)sizeF.Height + 2)) / 2;
				rectangle.Height = (int)sizeF.Height + 2;
				if ((int)rightToLeft == 1)
				{
					rectangle.X = empty2.Right - rectangle.Width;
				}
				ControlPaint.DrawFocusRectangle(g, rectangle);
			}
		}
		else
		{
			StiButton.DrawButton(g, rect, text, font, foreColor, backColor, null, -1, null, rightToLeft, wordWrap: false, isEnabled, isMouseOver, isChecked, isDefault: false, isFocused, (ContentAlignment)512, (ContentAlignment)32);
		}
		if ((int)appearance != 0)
		{
			return;
		}
		StringFormat val3 = new StringFormat();
		try
		{
			val3.FormatFlags = (StringFormatFlags)4096;
			val3.LineAlignment = (StringAlignment)1;
			val3.Trimming = (StringTrimming)3;
			val3.HotkeyPrefix = (HotkeyPrefix)1;
			if ((int)rightToLeft == 1)
			{
				val3.FormatFlags = (StringFormatFlags)(val3.FormatFlags | 1);
			}
			SolidBrush val4 = new SolidBrush(foreColor);
			try
			{
				if (isEnabled)
				{
					g.DrawString(text, font, (Brush)(object)val4, (RectangleF)empty2, val3);
				}
				else
				{
					ControlPaint.DrawStringDisabled(g, text, font, backColor, (RectangleF)empty2, val3);
				}
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
	}

	protected override void OnSystemColorsChanged(EventArgs e)
	{
		((Control)this).OnSystemColorsChanged(e);
		StiColors.InitColors();
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		((ButtonBase)this).OnMouseEnter(e);
		isMouseOver = true;
		((Control)this).Invalidate();
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((ButtonBase)this).OnMouseLeave(e);
		isMouseOver = false;
		((Control)this).Invalidate();
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

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		if (!flat)
		{
			((ButtonBase)this).OnPaint(e);
			return;
		}
		Graphics graphics = e.Graphics;
		Rectangle rect = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		DrawRadioButton(graphics, rect, ((Control)this).Text, ((Control)this).Font, ((Control)this).ForeColor, SystemColors.Control, ((Control)this).RightToLeft, ((Control)this).Enabled, isMouseOver, ((Control)this).Focused, ((RadioButton)this).Checked, ((RadioButton)this).Appearance);
	}

	public StiRadioButton()
	{
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)65536, true);
		((Control)this).SetStyle((ControlStyles)2048, true);
		((Control)this).SetStyle((ControlStyles)8192, false);
		((Control)this).SetStyle((ControlStyles)4, false);
	}
}
