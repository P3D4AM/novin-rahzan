using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls.Menu;

[ToolboxItem(false)]
public class StiMenuCheckedListBox : CheckedListBox
{
	private int hotTrackingIndex = -1;

	private int currentSelectedIndex = -1;

	private bool isCurrentlySelected;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override int ItemHeight
	{
		get
		{
			return StiScaleUI.YYI(22.0);
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

	public bool IsDarkMode
	{
		get
		{
			if (((Control)this).Parent is StiContextMenu)
			{
				return (((Control)this).Parent as StiContextMenu).IsDarkMode;
			}
			return false;
		}
	}

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

	protected override void OnSelectedIndexChanged(EventArgs e)
	{
		if (currentSelectedIndex != -1 && currentSelectedIndex != ((ListControl)this).SelectedIndex)
		{
			InvalidateItem(currentSelectedIndex);
		}
		((CheckedListBox)this).OnSelectedIndexChanged(e);
		InvalidateItem(((ListControl)this).SelectedIndex);
		currentSelectedIndex = ((ListControl)this).SelectedIndex;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		for (int i = 0; i < ((ObjectCollection)((CheckedListBox)this).Items).Count; i++)
		{
			Rectangle itemRectangle = ((ListBox)this).GetItemRectangle(i);
			((ListBox)this).OnDrawItem(new DrawItemEventArgs(e.Graphics, ((Control)this).Font, itemRectangle, i, (DrawItemState)32));
		}
	}

	protected override void OnPaintBackground(PaintEventArgs e)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		Color color = (IsDarkMode ? StiColor.Get("2b2b2b") : StiColor.Get("ffffff"));
		SolidBrush val = new SolidBrush(color);
		try
		{
			e.Graphics.FillRectangle((Brush)(object)val, e.ClipRectangle);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	protected override void OnDrawItem(DrawItemEventArgs e)
	{
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Expected O, but got Unknown
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		if (e.Index != -1)
		{
			Graphics graphics = e.Graphics;
			int checkSize = StiCheckPainter.GetCheckSize();
			bool flag = e.Index == hotTrackingIndex;
			Color color = (IsDarkMode ? StiColor.Get("f8f8f8") : StiColor.Get("333333"));
			Color color2 = (IsDarkMode ? StiColor.Get("f8f8f8") : StiColor.Get("333333"));
			Color color3 = (IsDarkMode ? StiColor.Get("2b2b2b") : StiColor.Get("ffffff"));
			if (flag)
			{
				color3 = (IsDarkMode ? StiColor.Get("414141") : StiColor.Get("e2e2e2"));
			}
			Rectangle bounds = e.Bounds;
			bounds.Inflate((int)(-2.0 * StiScaleUI.X), (int)(-1.0 * StiScaleUI.Y));
			if (e.Index == 0)
			{
				bounds.Y += (int)StiScaleUI.Y;
				bounds.Height -= (int)StiScaleUI.Y;
			}
			StringFormat val = new StringFormat();
			try
			{
				StiDrawing.FillRectangle(graphics, color3, bounds);
				val.Alignment = (StringAlignment)0;
				val.FormatFlags = (StringFormatFlags)4096;
				val.LineAlignment = (StringAlignment)1;
				val.Trimming = (StringTrimming)3;
				Rectangle rect = new Rectangle(bounds.X + checkSize + StiScaleUI.I6, (int)((double)bounds.Y + StiScaleUI.Y), bounds.Width - checkSize - StiScaleUI.I6, bounds.Height);
				string text = ((ObjectCollection)((CheckedListBox)this).Items)[e.Index].ToString();
				StiDrawing.DrawString(graphics, text, ((Control)this).Font, color, rect, val);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			StiCheckState checkState = GetCheckState(((CheckedListBox)this).GetItemCheckState(e.Index));
			Rectangle rect2 = new Rectangle(StiScaleUI.I4, e.Bounds.Y + (e.Bounds.Height - checkSize) / 2, checkSize, checkSize);
			StiCheckPainter.PaintCheckBox(graphics, rect2, checkState, color2);
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

	public StiMenuCheckedListBox()
	{
		((Control)this).SetStyle((ControlStyles)131072, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)512, false);
		((Control)this).TabStop = false;
		((ListBox)this).BorderStyle = (BorderStyle)0;
		((CheckedListBox)this).DrawMode = (DrawMode)1;
		((ListBox)this).IntegralHeight = false;
		((CheckedListBox)this).CheckOnClick = true;
	}
}
