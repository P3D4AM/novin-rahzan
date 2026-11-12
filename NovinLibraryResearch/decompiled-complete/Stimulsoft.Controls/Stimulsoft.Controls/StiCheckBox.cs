using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
[ToolboxBitmap(typeof(StiCheckBox), "Toolbox.StiCheckBox.bmp")]
public class StiCheckBox : CheckBox
{
	private bool isMouseOver;

	private bool flat = true;

	private bool drawLine;

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
			return ((CheckBox)this).TextAlign;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((CheckBox)this).TextAlign = value;
		}
	}

	[Browsable(false)]
	public ContentAlignment CheckAlign
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((CheckBox)this).CheckAlign;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((CheckBox)this).CheckAlign = value;
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

	[DefaultValue(true)]
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

	[DefaultValue(false)]
	[Category("Behavior")]
	public bool DrawLine
	{
		get
		{
			return drawLine;
		}
		set
		{
			drawLine = value;
			((Control)this).Invalidate();
		}
	}

	public static void DrawCheckBox(Graphics g, Rectangle rect, string text, Font font, Color foreColor, Color backColor, RightToLeft rightToLeft, bool isEnabled, bool isMouseOver, bool isFocused, bool drawLine, bool isChecked, CheckState checkState, Appearance appearance)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Expected O, but got Unknown
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Expected O, but got Unknown
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Expected O, but got Unknown
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Invalid comparison between Unknown and I4
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Expected O, but got Unknown
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Expected O, but got Unknown
		//IL_037a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Invalid comparison between Unknown and I4
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Invalid comparison between Unknown and I4
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Expected O, but got Unknown
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Invalid comparison between Unknown and I4
		Rectangle rectangle = new Rectangle(rect.X + StiScaleUI.I4, rect.Y, rect.Width - StiScaleUI.I4, rect.Height);
		Rectangle rectangle2 = (((int)rightToLeft != 0) ? new Rectangle(rect.Right - StiScaleUI.I15, rect.Y + (rect.Height - StiScaleUI.I12) / 2 - 1, StiScaleUI.I12, StiScaleUI.I12) : new Rectangle(rect.X, rect.Y + (rect.Height - StiScaleUI.I12) / 2 - 1, StiScaleUI.I12, StiScaleUI.I12));
		if ((int)appearance == 0)
		{
			if ((int)rightToLeft == 0)
			{
				rectangle.X += StiScaleUI.I10;
				rectangle.Width -= StiScaleUI.I10;
			}
			else
			{
				rectangle.Width -= StiScaleUI.XXI(16.0);
			}
			if (drawLine)
			{
				int num = rect.Y + rect.Height / 2;
				Pen val = new Pen(StiColorUtils.Light(SystemColors.Control, 30));
				try
				{
					Pen val2 = new Pen(StiColorUtils.Dark(SystemColors.Control, 50));
					try
					{
						int num2 = (int)g.MeasureString(text, font).Width;
						int num3 = num2 + StiScaleUI.XXI(16.0) + rect.X;
						if (num3 < rect.Right)
						{
							g.DrawLine(val2, num3, num, rect.Right, num);
							g.DrawLine(val, num3, num + 1, rect.Right, num + 1);
						}
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			if (isEnabled)
			{
				LinearGradientBrush val3 = new LinearGradientBrush(rectangle2, StiColors.ContentDark, SystemColors.ControlLightLight, 45f);
				try
				{
					g.FillRectangle((Brush)(object)val3, rectangle2);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			else
			{
				g.FillRectangle(SystemBrushes.Control, rectangle2);
			}
			if (isEnabled && (isMouseOver || isFocused))
			{
				g.DrawRectangle(StiPens.SelectedText, rectangle2);
			}
			else
			{
				g.DrawRectangle(SystemPens.ControlDark, rectangle2);
			}
			if (isChecked || (int)checkState == 2)
			{
				StiControlPaint.DrawCheck(g, rectangle2.X + StiScaleUI.I6, rectangle2.Y + StiScaleUI.I6, isEnabled);
			}
			if ((int)checkState == 2)
			{
				SolidBrush val4 = new SolidBrush(Color.FromArgb(200, SystemColors.Control));
				try
				{
					g.FillRectangle((Brush)(object)val4, rectangle2.X + StiScaleUI.I1, rectangle2.Y + StiScaleUI.I1, rectangle2.Width - StiScaleUI.I2, rectangle2.Height - StiScaleUI.I2);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
			}
			if (isFocused)
			{
				Rectangle rectangle3 = rectangle;
				SizeF sizeF = g.MeasureString(text, font);
				rectangle3.Width = (int)sizeF.Width;
				rectangle3.Y = (rectangle3.Height - ((int)sizeF.Height + StiScaleUI.I2)) / 2;
				rectangle3.Height = (int)sizeF.Height + StiScaleUI.I2;
				if ((int)rightToLeft == 1)
				{
					rectangle3.X = rectangle.Right - rectangle3.Width;
				}
				ControlPaint.DrawFocusRectangle(g, rectangle3);
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
		StringFormat val5 = new StringFormat();
		try
		{
			SolidBrush val6 = new SolidBrush(foreColor);
			try
			{
				val5.FormatFlags = (StringFormatFlags)4096;
				val5.LineAlignment = (StringAlignment)1;
				val5.Trimming = (StringTrimming)3;
				val5.HotkeyPrefix = (HotkeyPrefix)1;
				if ((int)rightToLeft == 1)
				{
					val5.FormatFlags = (StringFormatFlags)(val5.FormatFlags | 1);
				}
				if (isEnabled)
				{
					g.DrawString(text, font, (Brush)(object)val6, (RectangleF)rectangle, val5);
				}
				else
				{
					ControlPaint.DrawStringDisabled(g, text, font, backColor, (RectangleF)rectangle, val5);
				}
			}
			finally
			{
				((IDisposable)val6)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val5)?.Dispose();
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
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		if (!flat)
		{
			((ButtonBase)this).OnPaint(e);
			return;
		}
		Graphics graphics = e.Graphics;
		Rectangle rect = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		DrawCheckBox(graphics, rect, ((Control)this).Text, ((Control)this).Font, ((Control)this).ForeColor, SystemColors.Control, ((Control)this).RightToLeft, ((Control)this).Enabled, isMouseOver, ((Control)this).Focused, DrawLine, ((CheckBox)this).Checked, ((CheckBox)this).CheckState, ((CheckBox)this).Appearance);
	}

	public StiCheckBox()
	{
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)65536, true);
		((Control)this).SetStyle((ControlStyles)2048, true);
		((Control)this).SetStyle((ControlStyles)8192, false);
		((Control)this).SetStyle((ControlStyles)4, false);
	}
}
