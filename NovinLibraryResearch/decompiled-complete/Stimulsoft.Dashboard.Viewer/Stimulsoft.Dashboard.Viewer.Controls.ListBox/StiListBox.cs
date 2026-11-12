using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Dashboard;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Drawing.Helpers;

namespace Stimulsoft.Dashboard.Viewer.Controls.ListBox;

[ToolboxItem(false)]
public class StiListBox : ListBox
{
	private int hotTrackingIndex = -1;

	private int currentSelectedIndex = -1;

	private StiControlColorCollection colors;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override int ItemHeight
	{
		get
		{
			return ((ListBox)this).ItemHeight;
		}
		set
		{
			((ListBox)this).ItemHeight = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override DrawMode DrawMode
	{
		get
		{
			return (DrawMode)1;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public BorderStyle BorderStyle
	{
		get
		{
			return (BorderStyle)0;
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

	public bool IsTransparent { get; set; }

	public override Color BackColor
	{
		get
		{
			if (!IsTransparent)
			{
				return ((ListBox)this).BackColor;
			}
			return Color.Transparent;
		}
		set
		{
			((ListBox)this).BackColor = (IsTransparent ? Color.Transparent : value);
		}
	}

	protected override void OnSelectedIndexChanged(EventArgs e)
	{
		if (currentSelectedIndex != -1 && currentSelectedIndex != ((ListControl)this).SelectedIndex)
		{
			InvalidateItem(currentSelectedIndex);
		}
		((ListBox)this).OnSelectedIndexChanged(e);
		InvalidateItem(((ListControl)this).SelectedIndex);
		currentSelectedIndex = ((ListControl)this).SelectedIndex;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		bool flag = false;
		for (int i = 0; i < ((ListBox)this).Items.Count; i++)
		{
			Rectangle itemRectangle = ((ListBox)this).GetItemRectangle(i);
			if (itemRectangle.IntersectsWith(e.ClipRectangle))
			{
				((ListBox)this).OnDrawItem(new DrawItemEventArgs(e.Graphics, ((Control)this).Font, itemRectangle, i, (DrawItemState)32));
				flag = true;
			}
			else if (flag)
			{
				break;
			}
		}
	}

	protected override void OnDrawItem(DrawItemEventArgs e)
	{
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		if (e.Index == -1 || e.Index >= ((ListBox)this).Items.Count)
		{
			return;
		}
		Graphics graphics = e.Graphics;
		Rectangle bounds = e.Bounds;
		string text = ((ListBox)this).Items[e.Index].ToString();
		if (text == "-")
		{
			StiDrawing.DrawLine(graphics, Colors.SeparatorColor, bounds.X + StiScale.I4, bounds.Y + bounds.Height / 2, bounds.Right - StiScale.I4 * 2, bounds.Y + bounds.Height / 2);
			return;
		}
		bool flag = ((ListControl)this).SelectedIndex == e.Index;
		bool flag2 = e.Index == hotTrackingIndex;
		Color foreColor = Colors.GetForeColor(flag, flag2);
		Color backColor = Colors.GetBackColor(flag, flag2);
		if (e.Index == 0)
		{
			bounds.Y++;
			bounds.Height--;
		}
		if (flag || flag2)
		{
			StiDrawing.FillRectangle(graphics, backColor, bounds);
		}
		TextFormatFlags val = (TextFormatFlags)32804;
		TextRenderer.DrawText((IDeviceContext)(object)graphics, text, ((Control)this).Font, bounds, foreColor, val);
		if (e.Index == 0 && text == StiLocHelper.LocAll)
		{
			StiDrawing.DrawLine(graphics, Colors.SeparatorColor, bounds.X, bounds.Bottom - 1, bounds.Right, bounds.Bottom - 1);
		}
	}

	protected override void OnMeasureItem(MeasureItemEventArgs e)
	{
		((ListBox)this).OnMeasureItem(e);
		if (e.Index != -1 && e.Index < ((ListBox)this).Items.Count)
		{
			string text = ((ListBox)this).Items[e.Index].ToString();
			if (text == "-")
			{
				e.ItemHeight = StiScale.I3;
			}
			else
			{
				e.ItemHeight = ((ListBox)this).ItemHeight;
			}
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		RefreshHotTracking();
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
		RefreshHotTracking();
	}

	protected override void OnMouseHover(EventArgs e)
	{
		((Control)this).OnMouseHover(e);
		RefreshHotTracking();
	}

	private void RefreshHotTracking()
	{
		Point point = ((Control)this).PointToClient(Cursor.Position);
		int num = ((ListBox)this).IndexFromPoint(point);
		if (hotTrackingIndex != num)
		{
			int index = hotTrackingIndex;
			hotTrackingIndex = num;
			InvalidateItem(index);
			InvalidateItem(hotTrackingIndex);
		}
	}

	private void InvalidateItem(int index)
	{
		if (index != -1 && index < ((ListBox)this).Items.Count)
		{
			((Control)this).Invalidate(((ListBox)this).GetItemRectangle(index));
		}
	}

	public StiListBox()
	{
		((Control)this).SetStyle((ControlStyles)131072, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)2048, true);
		Colors = new StiControlColorCollection((Control)(object)this);
		((ListBox)this).BorderStyle = (BorderStyle)0;
		((ListBox)this).DrawMode = (DrawMode)2;
		((ListBox)this).IntegralHeight = false;
		((ListBox)this).ItemHeight = StiScale.YYI(StiElementConsts.ListBox.ItemHeight);
	}
}
