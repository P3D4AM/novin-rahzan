using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.ComboBox;
using Stimulsoft.Dashboard.Components.ListBox;
using Stimulsoft.Dashboard.Drawing.Helpers;

namespace Stimulsoft.Dashboard.Viewer.Controls.ListBox;

[ToolboxItem(false)]
public class StiCheckedListBox : CheckedListBox
{
	private int hotTrackingIndex = -1;

	private int currentSelectedIndex = -1;

	private bool isCurrentlySelected;

	private StiControlColorCollection colors;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override int ItemHeight
	{
		get
		{
			return StiScale.YYI(32.0);
		}
		set
		{
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

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool ShowCheckBoxes { get; set; } = true;


	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal bool IsCurrentlySelected
	{
		get
		{
			return isCurrentlySelected;
		}
		set
		{
			if (isCurrentlySelected != value)
			{
				isCurrentlySelected = value;
				((Control)this).Invalidate();
			}
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

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsTransparent { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool BlockSelectedIndexChanged { get; set; }

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
		if (!BlockSelectedIndexChanged)
		{
			if (currentSelectedIndex != -1 && currentSelectedIndex != ((ListControl)this).SelectedIndex)
			{
				InvalidateItem(currentSelectedIndex);
			}
			((CheckedListBox)this).OnSelectedIndexChanged(e);
			InvalidateItem(((ListControl)this).SelectedIndex);
			currentSelectedIndex = ((ListControl)this).SelectedIndex;
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		bool flag = false;
		for (int i = 0; i < ((ObjectCollection)((CheckedListBox)this).Items).Count; i++)
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
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Expected O, but got Unknown
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		if (e.Index == -1)
		{
			return;
		}
		Graphics graphics = e.Graphics;
		int checkSize = StiCheckPainter.GetCheckSize();
		bool flag = ((ListControl)this).SelectedIndex == e.Index;
		bool flag2 = e.Index == hotTrackingIndex;
		Color foreColor = Colors.GetForeColor(flag, flag2);
		Color backColor = Colors.GetBackColor(flag, flag2);
		Color glyphColor = Colors.GetGlyphColor(flag, flag2);
		Rectangle bounds = e.Bounds;
		bounds.Inflate((int)(-2.0 * StiScale.X), (int)(-1.0 * StiScale.Y));
		if (e.Index == 0)
		{
			bounds.Y += (int)StiScale.Y;
			bounds.Height -= (int)StiScale.Y;
		}
		if (flag2 || flag)
		{
			StiDrawing.FillRectangle(graphics, backColor, bounds);
		}
		StringFormat val = new StringFormat();
		try
		{
			val.Alignment = (StringAlignment)0;
			val.FormatFlags = (StringFormatFlags)4096;
			val.LineAlignment = (StringAlignment)1;
			val.Trimming = (StringTrimming)3;
			Rectangle rect = new Rectangle(bounds.X, (int)((double)bounds.Y + StiScale.Y), bounds.Width, bounds.Height);
			if (ShowCheckBoxes)
			{
				rect.X += checkSize + StiScale.I6;
				rect.Width -= checkSize + StiScale.I6;
			}
			string text = ((ObjectCollection)((CheckedListBox)this).Items)[e.Index].ToString();
			StiDrawing.DrawString(graphics, text, ((Control)this).Font, foreColor, rect, val);
			if (e.Index == 0 && text == StiLocHelper.LocAll)
			{
				StiDrawing.DrawLine(graphics, Colors.SeparatorColor, bounds.X, (float)((double)bounds.Y + 30.0 * StiScale.Y), bounds.Right, (float)((double)bounds.Y + 30.0 * StiScale.Y));
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		if (ShowCheckBoxes)
		{
			StiCheckState checkState = GetCheckState(((CheckedListBox)this).GetItemCheckState(e.Index));
			Rectangle rect2 = new Rectangle(StiScale.I4, e.Bounds.Y + (e.Bounds.Height - checkSize) / 2, checkSize, checkSize);
			StiCheckPainter.PaintCheckBox(graphics, rect2, checkState, glyphColor);
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

	protected override void OnItemCheck(ItemCheckEventArgs ice)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Invalid comparison between Unknown and I4
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Invalid comparison between Unknown and I4
		((CheckedListBox)this).OnItemCheck(ice);
		if (ice.Index != -1)
		{
			if (((ObjectCollection)((CheckedListBox)this).Items)[ice.Index] is StiComboBoxItem stiComboBoxItem)
			{
				stiComboBoxItem.Checked = (int)ice.NewValue == 1;
			}
			if (((ObjectCollection)((CheckedListBox)this).Items)[ice.Index] is StiListBoxItem stiListBoxItem)
			{
				stiListBoxItem.Checked = (int)ice.NewValue == 1;
			}
		}
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
		if (index != -1 && index < ((ObjectCollection)((CheckedListBox)this).Items).Count)
		{
			((Control)this).Invalidate(((ListBox)this).GetItemRectangle(index));
		}
	}

	private StiCheckState GetCheckState(CheckState state)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Invalid comparison between Unknown and I4
		if ((int)state != 0)
		{
			if ((int)state == 1)
			{
				return StiCheckState.Checked;
			}
			return StiCheckState.Indeterminate;
		}
		return StiCheckState.Unchecked;
	}

	public StiCheckedListBox()
	{
		((Control)this).SetStyle((ControlStyles)131072, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)2048, true);
		Colors = new StiControlColorCollection((Control)(object)this);
		((ListBox)this).BorderStyle = (BorderStyle)0;
		((CheckedListBox)this).DrawMode = (DrawMode)1;
		((ListBox)this).IntegralHeight = false;
		((CheckedListBox)this).CheckOnClick = true;
	}
}
