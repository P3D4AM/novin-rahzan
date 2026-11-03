using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Controls;

[ToolboxItem(false)]
public class StiComboBox : ComboBox
{
	protected bool isMouseOver;

	protected bool lastIsMouseOverButton;

	private bool flat = true;

	private bool useFirstImage;

	private bool autoDropDownWidth = true;

	private ImageList imageList;

	private Image buttonBitmap;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DrawMode DrawMode
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((ComboBox)this).DrawMode;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((ComboBox)this).DrawMode = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Color ForeColor
	{
		get
		{
			if (!StiUX.IsDark)
			{
				return ((ComboBox)this).ForeColor;
			}
			return StiUX.InputForeground;
		}
		set
		{
			if (!StiUX.IsDark)
			{
				((ComboBox)this).ForeColor = value;
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Color BackColor
	{
		get
		{
			if (!StiUX.IsDark)
			{
				return ((ComboBox)this).BackColor;
			}
			return StiUX.InputBackground;
		}
		set
		{
			if (!StiUX.IsDark)
			{
				((ComboBox)this).BackColor = value;
			}
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	public virtual bool Flat
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

	[Category("Behavior")]
	[DefaultValue(false)]
	public virtual bool UseFirstImage
	{
		get
		{
			return useFirstImage;
		}
		set
		{
			useFirstImage = value;
			((Control)this).Invalidate();
		}
	}

	[Category("Behavior")]
	[DefaultValue(true)]
	public virtual bool AutoDropDownWidth
	{
		get
		{
			return autoDropDownWidth;
		}
		set
		{
			autoDropDownWidth = value;
			((Control)this).Invalidate();
		}
	}

	[Category("Behavior")]
	[DefaultValue(null)]
	[Browsable(true)]
	public virtual ImageList ImageList
	{
		get
		{
			return imageList;
		}
		set
		{
			imageList = value;
			((Control)this).Invalidate();
		}
	}

	[Browsable(false)]
	public bool IsMouseOverButton => StiControlPaint.GetButtonRect(new Rectangle(0, 0, ((Control)this).Width - 1, ((Control)this).Height - 1), Flat, ((Control)this).RightToLeft).Contains(((Control)this).PointToClient(Cursor.Position));

	private MeasureItemEventHandler GetMeasureItemHandler()
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		FieldInfo field = typeof(ComboBox).GetField("EVENT_MEASUREITEM", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.GetField);
		return (MeasureItemEventHandler)((Component)this).Events[field.GetValue(null)];
	}

	private DrawItemEventHandler GetDrawItemHandler()
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		FieldInfo field = typeof(ComboBox).GetField("EVENT_DRAWITEM", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.GetField);
		return (DrawItemEventHandler)((Component)this).Events[field.GetValue(null)];
	}

	public static void DrawComboBox(Graphics g, Rectangle rect, string text, Font font, Color foreColor, Color backColor, RightToLeft rightToLeft, bool isEnabled, bool isMouseOver, bool isMouseOverButton, bool isDroppedDown, bool isFocused, bool fillContent, ComboBoxStyle comboBoxStyle, Image buttonBitmap, bool flat)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		if (fillContent)
		{
			SolidBrush val = new SolidBrush(backColor);
			try
			{
				g.FillRectangle((Brush)(object)val, rect);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		StiControlPaint.DrawBorder(g, rect, isMouseOver || isFocused, isEnabled, flat);
		if (buttonBitmap != null && (int)comboBoxStyle != 0)
		{
			Rectangle buttonRect = StiControlPaint.GetButtonRect(rect, flat, rightToLeft);
			StiControlPaint.DrawButton(g, buttonRect, buttonBitmap, isDroppedDown, isFocused || isMouseOver, isMouseOverButton, isEnabled, flat);
		}
		if (text == null)
		{
			return;
		}
		StringFormat val2 = new StringFormat();
		try
		{
			SolidBrush val3 = new SolidBrush(foreColor);
			try
			{
				val2.FormatFlags = (StringFormatFlags)4096;
				val2.LineAlignment = (StringAlignment)1;
				g.DrawString(text, font, (Brush)(object)val3, (RectangleF)rect, val2);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	protected virtual void Draw(bool fillContent)
	{
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Invalid comparison between Unknown and I4
		Graphics val = Graphics.FromHwnd(((Control)this).Handle);
		try
		{
			Rectangle rectangle = new Rectangle(0, 0, ((Control)this).Width - 1, ((Control)this).Height - 1);
			if (flat)
			{
				Color color = ((Control)this).BackColor;
				if (!((Control)this).Enabled)
				{
					color = SystemColors.Control;
				}
				int width = SystemInformation.Border3DSize.Width;
				int height = SystemInformation.Border3DSize.Height;
				Rectangle rectangle2 = rectangle;
				if ((int)((ComboBox)this).DropDownStyle != 0)
				{
					rectangle2.Width -= SystemInformation.HorizontalScrollBarArrowWidth;
				}
				SolidBrush val2 = new SolidBrush(color);
				try
				{
					if ((int)((Control)this).RightToLeft == 1)
					{
						rectangle2.X += SystemInformation.HorizontalScrollBarArrowWidth;
						val.FillRectangle((Brush)(object)val2, rectangle2);
					}
					else
					{
						val.FillRectangle((Brush)(object)val2, 1, 1, rectangle2.Width - 1, height - 1);
						val.FillRectangle((Brush)(object)val2, 1, rectangle2.Bottom - height + 1, rectangle2.Width - 1, height - 1);
						val.FillRectangle((Brush)(object)val2, 1, 1, width - 1, rectangle2.Height - height);
						val.FillRectangle((Brush)(object)val2, rectangle2.Right - width + 1, 1, width - 1, rectangle2.Height - height);
					}
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			DrawComboBox(val, rectangle, null, ((Control)this).Font, ((Control)this).ForeColor, ((Control)this).BackColor, ((Control)this).RightToLeft, ((Control)this).Enabled, isMouseOver, IsMouseOverButton, ((ComboBox)this).DroppedDown, ((Control)this).Focused, fillContent: false, ((ComboBox)this).DropDownStyle, buttonBitmap, flat);
			lastIsMouseOverButton = IsMouseOverButton;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	protected override void OnSystemColorsChanged(EventArgs e)
	{
		((Control)this).OnSystemColorsChanged(e);
		StiColors.InitColors();
	}

	protected override void OnDropDown(EventArgs e)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		((ComboBox)this).OnDropDown(e);
		if (!AutoDropDownWidth)
		{
			return;
		}
		Graphics val = Graphics.FromHwnd(((Control)this).Handle);
		try
		{
			int num = 0;
			for (int i = 0; i < ((ComboBox)this).Items.Count; i++)
			{
				MeasureItemEventArgs val2 = new MeasureItemEventArgs(val, i++, ((ComboBox)this).ItemHeight);
				((ComboBox)this).OnMeasureItem(val2);
				num = Math.Max(val2.ItemWidth, num);
			}
			((ComboBox)this).DropDownWidth = Math.Max(num, ((Control)this).Width);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	protected override void OnMeasureItem(MeasureItemEventArgs e)
	{
		MeasureItemEventHandler measureItemHandler = GetMeasureItemHandler();
		if (measureItemHandler == null)
		{
			((ComboBox)this).OnMeasureItem(e);
			if (e.Index != -1)
			{
				Graphics graphics = e.Graphics;
				string text = ((ComboBox)this).Items[e.Index].ToString();
				e.ItemWidth = (int)graphics.MeasureString(text, ((Control)this).Font).Width + SystemInformation.HorizontalScrollBarThumbWidth + 10;
			}
		}
		else
		{
			measureItemHandler.Invoke((object)this, e);
		}
	}

	protected override void OnDrawItem(DrawItemEventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Invalid comparison between Unknown and I4
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		DrawItemEventHandler drawItemHandler = GetDrawItemHandler();
		if (drawItemHandler == null)
		{
			Graphics graphics = e.Graphics;
			Rectangle bounds = e.Bounds;
			if ((e.State & 1) > 0)
			{
				bounds.Width--;
			}
			DrawItemState state = e.State;
			int num = ((!UseFirstImage) ? e.Index : 0);
			string text = ((Control)this).Text;
			if (e.Index != -1)
			{
				text = ((ListControl)this).GetItemText(((ComboBox)this).Items[e.Index]);
			}
			StiControlPaint.DrawItem(graphics, bounds, state, text, ImageList, num, ((Control)this).Font, ((Control)this).BackColor, ((Control)this).ForeColor, ((Control)this).RightToLeft);
		}
		else
		{
			drawItemHandler.Invoke((object)this, e);
		}
	}

	protected override void OnGotFocus(EventArgs e)
	{
		((ComboBox)this).OnGotFocus(e);
		if (Flat)
		{
			Draw(fillContent: false);
		}
	}

	protected override void OnLostFocus(EventArgs e)
	{
		((ComboBox)this).OnLostFocus(e);
		if (Flat)
		{
			Draw(fillContent: false);
		}
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		((ComboBox)this).OnMouseEnter(e);
		isMouseOver = true;
		if (Flat)
		{
			Draw(fillContent: false);
		}
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((ComboBox)this).OnMouseLeave(e);
		isMouseOver = false;
		if (Flat)
		{
			Draw(fillContent: false);
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		if (lastIsMouseOverButton != IsMouseOverButton && Flat)
		{
			Draw(fillContent: false);
		}
	}

	protected override void WndProc(ref Message msg)
	{
		if (((Message)(ref msg)).Msg == 15)
		{
			((Control)this).DefWndProc(ref msg);
			if (Flat)
			{
				Draw(fillContent: true);
			}
		}
		else
		{
			((ComboBox)this).WndProc(ref msg);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && buttonBitmap != null)
		{
			buttonBitmap.Dispose();
		}
		((ComboBox)this).Dispose(disposing);
	}

	public StiComboBox()
	{
		try
		{
			buttonBitmap = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Controls", "Stimulsoft.Controls.Bmp.ComboDown.bmp");
		}
		catch
		{
		}
		((Control)this).SetStyle((ControlStyles)16, true);
		DrawMode = (DrawMode)2;
	}
}
