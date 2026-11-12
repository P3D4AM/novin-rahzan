using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Dashboard;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Drawing.Helpers;
using Stimulsoft.Dashboard.Viewer.Controls.Helpers;
using Stimulsoft.Dashboard.Viewer.Elements;

namespace Stimulsoft.Dashboard.Viewer.Controls.ComboBox;

[ToolboxItem(false)]
public class StiComboBox : ComboBox, IStiMouseEventsControl
{
	private int currentSelectedIndex = -1;

	private StiControlColorCollection colors;

	private bool isHot;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiControlColorCollection Colors
	{
		get
		{
			return colors;
		}
		set
		{
			if (value != colors)
			{
				colors = value;
				colors.Control = (Control)(object)this;
				((Control)this).Invalidate();
			}
		}
	}

	private bool IsHot
	{
		get
		{
			return isHot;
		}
		set
		{
			if (isHot != value)
			{
				isHot = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int ItemHeight
	{
		get
		{
			return ((ComboBox)this).ItemHeight;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DrawMode DrawMode
	{
		get
		{
			return (DrawMode)2;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IntegralHeight
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	protected override void OnDropDown(EventArgs e)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		((ComboBox)this).OnDropDown(e);
		Graphics val = Graphics.FromHwnd(((Control)this).Handle);
		try
		{
			int num = 0;
			for (int i = 0; i < ((ComboBox)this).Items.Count; i++)
			{
				MeasureItemEventArgs val2 = new MeasureItemEventArgs(val, i++, StiScale.YYI(StiElementConsts.ComboBox.ItemHeight));
				((ComboBox)this).OnMeasureItem(val2);
				num = Math.Max(val2.ItemWidth, num);
			}
			((ComboBox)this).DropDownWidth = Math.Max(num, ((Control)this).Width);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		((ComboBox)this).DropDownHeight = StiScale.YYI(StiElementConsts.ComboBox.ItemHeight) * Math.Min(((ComboBox)this).Items.Count, 10) + (int)(4.0 * StiScale.Y);
	}

	protected override void OnMeasureItem(MeasureItemEventArgs e)
	{
		e.ItemHeight = StiScale.YYI(StiElementConsts.ComboBox.ItemHeight);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		Rectangle rectangle = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		StiDrawing.FillRectangle(graphics, StiTransparentColor.Fix(Colors.BackColor), rectangle);
		Rectangle rect = new Rectangle(rectangle.Right - GetButtonWidth(), rectangle.Y, GetButtonWidth(), rectangle.Height);
		((ComboBox)this).OnDrawItem(new DrawItemEventArgs(graphics, ((Control)this).Font, rectangle, ((ListControl)this).SelectedIndex, (DrawItemState)4096));
		OnDrawButton(graphics, rect);
	}

	protected override void OnDrawItem(DrawItemEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Invalid comparison between Unknown and I4
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		bool flag = (e.State & 0x1000) > 0;
		if (e.Index == -1 && !flag)
		{
			return;
		}
		Graphics graphics = e.Graphics;
		bool flag2 = (flag ? IsHot : ((e.State & 1) > 0));
		bool isSelected = !flag && currentSelectedIndex == e.Index;
		Color foreColor = Colors.GetForeColor(isSelected, flag2);
		Color backColor = Colors.GetBackColor(isSelected, flag2);
		Rectangle bounds = e.Bounds;
		StiDrawing.FillRectangle(graphics, backColor, bounds);
		if (e.Index != -1)
		{
			TextFormatFlags val = (TextFormatFlags)32804;
			string text = ((ComboBox)this).Items[e.Index].ToString();
			Rectangle rectangle = bounds;
			rectangle.Width -= GetButtonWidth();
			TextRenderer.DrawText((IDeviceContext)(object)graphics, text, ((Control)this).Font, rectangle, foreColor, val);
			if (e.Index == 0 && text == StiLocHelper.LocAll && !flag)
			{
				StiDrawing.DrawLine(graphics, Colors.SeparatorColor, bounds.X, bounds.Bottom - (int)StiScale.Y, bounds.Right, bounds.Bottom - (int)StiScale.Y);
			}
		}
	}

	protected virtual void OnDrawButton(Graphics g, Rectangle rect)
	{
		StiButtonPainter.DrawButton(g, rect, IsHot, Colors.GlyphColor, Colors.HotGlyphColor);
	}

	protected override void OnSelectedIndexChanged(EventArgs e)
	{
		((ComboBox)this).OnSelectedIndexChanged(e);
		currentSelectedIndex = ((ListControl)this).SelectedIndex;
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		((ComboBox)this).OnMouseEnter(e);
		IsHot = true;
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((ComboBox)this).OnMouseLeave(e);
		IsHot = false;
	}

	protected override void OnDropDownClosed(EventArgs e)
	{
		((ComboBox)this).OnDropDownClosed(e);
		IsHot = ((Control)this).ClientRectangle.Contains(((Control)this).PointToClient(Cursor.Position));
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		IsHot = ((Control)this).ClientRectangle.Contains(((Control)this).PointToClient(Cursor.Position));
	}

	public void InvokeMouseEnter()
	{
		((Control)this).OnMouseEnter((EventArgs)null);
	}

	public void InvokeMouseLeave()
	{
		((Control)this).OnMouseLeave((EventArgs)null);
	}

	public void InvokeMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
	}

	protected virtual int GetButtonWidth()
	{
		return SystemInformation.HorizontalScrollBarArrowWidth;
	}

	public StiComboBox()
	{
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)131072, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((ComboBox)this).DropDownStyle = (ComboBoxStyle)2;
		Colors = new StiControlColorCollection((Control)(object)this);
		((ComboBox)this).DrawMode = (DrawMode)2;
		((ComboBox)this).IntegralHeight = false;
		((ComboBox)this).ItemHeight = StiScale.YYI(SystemInformation.HorizontalScrollBarArrowWidth);
	}
}
