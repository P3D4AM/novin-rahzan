using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
[ToolboxBitmap(typeof(StiComboBox), "Toolbox.StiComboBox.bmp")]
public class StiComboBox : ComboBox
{
	private int buttonWidth = SystemInformation.HorizontalScrollBarArrowWidth;

	private bool isMouseOverButton;

	private bool isMouseOver;

	private Font drawingFont;

	private bool useFirstImage;

	private bool autoDropDownWidth = true;

	private ImageList imageList;

	private bool IsMouseOverButton
	{
		get
		{
			return isMouseOverButton;
		}
		set
		{
			if (isMouseOverButton != value)
			{
				isMouseOverButton = value;
				((Control)this).Invalidate();
			}
		}
	}

	private bool IsMouseOver
	{
		get
		{
			return isMouseOver;
		}
		set
		{
			if (isMouseOver != value)
			{
				isMouseOver = value;
				((Control)this).Invalidate();
			}
		}
	}

	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Category("Appearance")]
	public ComboBoxStyle DropDownStyle
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((ComboBox)this).DropDownStyle;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Invalid comparison between Unknown and I4
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected O, but got Unknown
			((ComboBox)this).DropDownStyle = value;
			if ((int)value == 1)
			{
				DrawMode = (DrawMode)0;
				((Control)this).Font = new Font("Microsoft Sans Serif", 7.875f, (GraphicsUnit)3);
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Font DrawingFont
	{
		get
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Expected O, but got Unknown
			if (StiScaleUI.Factor < 2.0)
			{
				return ((Control)this).Font;
			}
			if (drawingFont == null)
			{
				drawingFont = new Font("Microsoft Sans Serif", 8f);
			}
			return drawingFont;
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
			if (useFirstImage != value)
			{
				useFirstImage = value;
				((Control)this).Invalidate();
			}
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
			if (autoDropDownWidth != value)
			{
				autoDropDownWidth = value;
				((Control)this).Invalidate();
			}
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
			if (imageList != value)
			{
				imageList = value;
				((Control)this).Invalidate();
			}
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
	public override Font Font
	{
		get
		{
			return ((Control)this).Font;
		}
		set
		{
			((Control)this).Font = value;
		}
	}

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

	private bool GetIsMouseOver()
	{
		return ((Control)this).ClientRectangle.Contains(((Control)this).PointToClient(Cursor.Position));
	}

	private bool GetIsMouseOverButton()
	{
		return GetDownButtonRect().Contains(((Control)this).PointToClient(Cursor.Position));
	}

	private Color GetBorderColor()
	{
		if (!((Control)this).Enabled)
		{
			return StiUX.InputBorder;
		}
		if ((IsMouseOver || ((Control)this).Focused) && !((ComboBox)this).DroppedDown)
		{
			return StiUX.Accent;
		}
		return StiUX.InputBorder;
	}

	private Color GetButtonBackground()
	{
		if (((ComboBox)this).DroppedDown)
		{
			return StiUX.ButtonBackgroundPressed;
		}
		if (((Control)this).Focused || IsMouseOverButton)
		{
			return StiUX.ButtonBackgroundHover;
		}
		return StiUX.InputBackground;
	}

	private Rectangle GetDownButtonRect()
	{
		return new Rectangle(((Control)this).Width - buttonWidth - 1, 0, buttonWidth + 1, ((Control)this).Height);
	}

	private void DrawDownArrow(Graphics g)
	{
		Rectangle downButtonRect = GetDownButtonRect();
		StiButtonHelper.DrawDownArrow(g, downButtonRect, ((Control)this).Enabled);
	}

	protected override void OnSystemColorsChanged(EventArgs e)
	{
		((Control)this).OnSystemColorsChanged(e);
		StiColors.InitColors();
	}

	protected override void OnDropDown(EventArgs e)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		((ComboBox)this).OnDropDown(e);
		if (StiScaleUI.Factor == 1.0 || !AutoDropDownWidth)
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
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		if ((int)DropDownStyle == 1)
		{
			((ComboBox)this).OnMeasureItem(e);
			return;
		}
		MeasureItemEventHandler measureItemHandler = GetMeasureItemHandler();
		if (measureItemHandler == null)
		{
			((ComboBox)this).OnMeasureItem(e);
			if (e.Index != -1)
			{
				Graphics graphics = e.Graphics;
				string text = ((ComboBox)this).Items[e.Index].ToString();
				e.ItemWidth = (int)graphics.MeasureString(text, DrawingFont).Width + SystemInformation.HorizontalScrollBarThumbWidth + StiScaleUI.I10;
			}
		}
		else
		{
			measureItemHandler.Invoke((object)this, e);
		}
	}

	protected override void OnDrawItem(DrawItemEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Invalid comparison between Unknown and I4
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		if ((int)DropDownStyle == 1)
		{
			((ComboBox)this).OnDrawItem(e);
			return;
		}
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
			int imageIndex = ((!UseFirstImage) ? e.Index : 0);
			string text = ((Control)this).Text;
			if (e.Index != -1)
			{
				text = ((ListControl)this).GetItemText(((ComboBox)this).Items[e.Index]);
			}
			Color foreColor = (((Control)this).Enabled ? ((Control)this).ForeColor : StiUX.InputForeground);
			StiControlPaint.DrawItem(graphics, bounds, state, text, ImageList, imageIndex, DrawingFont, ((Control)this).BackColor, foreColor, ((Control)this).RightToLeft);
		}
		else
		{
			drawItemHandler.Invoke((object)this, e);
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		if (StiUX.IsDark)
		{
			IsMouseOver = GetIsMouseOver();
			IsMouseOverButton = GetIsMouseOverButton();
		}
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((ComboBox)this).OnMouseLeave(e);
		if (StiUX.IsDark)
		{
			IsMouseOver = false;
			IsMouseOverButton = false;
		}
	}

	protected override void OnGotFocus(EventArgs e)
	{
		((ComboBox)this).OnGotFocus(e);
		if (StiUX.IsDark)
		{
			((Control)this).Invalidate();
		}
	}

	protected override void OnLostFocus(EventArgs e)
	{
		((ComboBox)this).OnLostFocus(e);
		if (StiUX.IsDark)
		{
			((Control)this).Invalidate();
		}
	}

	protected override void WndProc(ref Message m)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		((ComboBox)this).WndProc(ref m);
		if (!StiUX.IsDark || ((Message)(ref m)).Msg != 15)
		{
			return;
		}
		Graphics val = Graphics.FromHwnd(((Control)this).Handle);
		try
		{
			Pen val2 = new Pen(StiUX.InputBackground, 2f);
			try
			{
				val.DrawRectangle(val2, 1, 1, ((Control)this).Width - buttonWidth - 2, ((Control)this).Height - 2);
				Rectangle downButtonRect = GetDownButtonRect();
				Color buttonBackground = GetButtonBackground();
				Color borderColor = GetBorderColor();
				StiDrawing.FillRectangle(val, buttonBackground, downButtonRect);
				if (IsMouseOverButton && !((ComboBox)this).DroppedDown)
				{
					StiDrawing.FillRectangle(val, StiUX.Accent.ChangeAlpha(50), downButtonRect);
				}
				StiDrawing.DrawRectangle(val, borderColor, downButtonRect);
				DrawDownArrow(val);
				StiControlPaint.DrawBorder(val, new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height), (IsMouseOver || ((Control)this).Focused) && !((ComboBox)this).DroppedDown, ((Control)this).Enabled, flat: true);
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

	public StiComboBox()
	{
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)131072, true);
		if (StiUX.IsDark)
		{
			((Control)this).SetStyle((ControlStyles)8192, true);
			((ComboBox)this).FlatStyle = (FlatStyle)0;
		}
		if (StiScaleUI.Factor >= 2.0)
		{
			drawingFont = ((Control)this).Font;
		}
		if (StiScaleUI.Factor >= 2.5)
		{
			((Control)this).Font = new Font("Microsoft Sans Serif", (float)(StiScaleUI.Factor * 4.0));
			DrawMode = (DrawMode)2;
		}
		else if (StiScaleUI.Factor >= 2.25)
		{
			((Control)this).Font = new Font("Microsoft Sans Serif", (float)(StiScaleUI.Factor * 5.0));
			DrawMode = (DrawMode)2;
		}
		else if (StiScaleUI.Factor >= 2.0)
		{
			((Control)this).Font = new Font("Microsoft Sans Serif", (float)(StiScaleUI.Factor * 5.5));
			DrawMode = (DrawMode)2;
		}
	}
}
