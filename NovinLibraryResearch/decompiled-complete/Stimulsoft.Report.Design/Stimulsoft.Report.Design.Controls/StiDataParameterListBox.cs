using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Design.Components;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Design.Controls;

public sealed class StiDataParameterListBox : StiItemListBox
{
	private int hotTrackingIndex = -1;

	private int currentSelectedIndex = -1;

	private Point dragOrigin;

	private bool isDragging;

	private bool isCurrentlySelected;

	private bool isCloseButtonOver;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool AllowDragItems { get; set; } = true;


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
	private bool IsCloseButtonOver
	{
		get
		{
			return isCloseButtonOver;
		}
		set
		{
			if (isCloseButtonOver != value)
			{
				isCloseButtonOver = value;
				InvalidateCloseButton();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiDataParameter SelectedParameter
	{
		get
		{
			if (((ListControl)this).SelectedIndex < 0 || ((ListControl)this).SelectedIndex >= ((ListBox)this).Items.Count)
			{
				return null;
			}
			return ((ListBox)this).Items[((ListControl)this).SelectedIndex] as StiDataParameter;
		}
		set
		{
			((ListControl)this).SelectedIndex = ((value != null) ? ((ListBox)this).Items.IndexOf((object)value) : (-1));
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

	protected override void OnKeyDown(KeyEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Invalid comparison between Unknown and I4
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Invalid comparison between Unknown and I4
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Invalid comparison between Unknown and I4
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Invalid comparison between Unknown and I4
		((Control)this).OnKeyDown(e);
		if ((int)e.KeyCode == 46)
		{
			e.Handled = true;
			DeleteItem();
		}
		if ((int)e.KeyCode == 38 && (Control.ModifierKeys & 0x10000) > 0)
		{
			e.Handled = true;
			MoveItemUp();
		}
		if ((int)e.KeyCode == 40 && (Control.ModifierKeys & 0x10000) > 0)
		{
			e.Handled = true;
			MoveItemDown();
		}
	}

	protected override void WndProc(ref Message m)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		((ListBox)this).WndProc(ref m);
		if (StiListBoxPainter.IsVerticalScrollbar(m))
		{
			((Control)this).Invalidate();
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (((ListBox)this).Items.Count == 0)
		{
			StiListBoxPainter.PaintLabel(e.Graphics, ((Control)this).ClientRectangle, Loc.GetMain("NoElements"));
		}
		OnPaintItems(e.Graphics);
	}

	protected override void OnPaintBackground(PaintEventArgs e)
	{
		StiListBoxPainter.PaintBorder(e.Graphics, ((Control)this).ClientRectangle, ((ListBox)this).Items.Count);
	}

	protected override void OnDrawItem(DrawItemEventArgs e)
	{
		if (e.Index != -1 && ((ListBox)this).Items[e.Index] is StiDataParameter parameter)
		{
			Graphics graphics = e.Graphics;
			bool flag = ((ListControl)this).SelectedIndex == e.Index;
			bool flag2 = e.Index == hotTrackingIndex;
			Color backgroundColor = StiListBoxPainter.GetBackgroundColor(flag, flag2, IsCurrentlySelected);
			Color foregroundColor = StiListBoxPainter.GetForegroundColor(flag, flag2, IsCurrentlySelected);
			Rectangle bounds = e.Bounds;
			bounds.Inflate(-StiScale.I2, -StiScale.I1);
			if (e.Index == 0)
			{
				bounds.Y++;
				bounds.Height--;
			}
			StiDrawing.FillRectangle(graphics, backgroundColor, bounds);
			if (flag && ((Control)this).Focused && StiUX.IsDark)
			{
				StiDrawing.FillRectangle(graphics, StiUX.Accent.ChangeAlpha(50), bounds);
			}
			if (flag2)
			{
				bounds.Width -= StiListBoxPainter.CloseButtonWidth;
			}
			OnPaintText(graphics, parameter, foregroundColor, bounds);
			if (flag2)
			{
				StiListBoxPainter.PaintCloseButton(e.Graphics, e.Bounds, e.Index == 0, IsCloseButtonOver);
			}
		}
	}

	private void OnPaintText(Graphics g, StiDataParameter parameter, Color foreground, Rectangle rect)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		TextFormatFlags val = (TextFormatFlags)32804;
		Rectangle rectangle = rect;
		rectangle.X += StiScaleUI.I4;
		rectangle.Width -= StiScaleUI.I4;
		TextRenderer.DrawText((IDeviceContext)(object)g, parameter.ToString(), ((Control)this).Font, rectangle, foreground, val);
	}

	private void OnPaintItems(Graphics g)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		for (int i = 0; i < ((ListBox)this).Items.Count; i++)
		{
			Rectangle itemRectangle = ((ListBox)this).GetItemRectangle(i);
			((ListBox)this).OnDrawItem(new DrawItemEventArgs(g, ((Control)this).Font, itemRectangle, i, (DrawItemState)32));
		}
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		((Control)this).OnMouseDown(e);
		((Control)this).Focus();
		dragOrigin = e.Location;
		StiDataParameter itemAt = GetItemAt(e.Location);
		if (itemAt == null)
		{
			return;
		}
		if (GetLeftFrameOfCloseButton() < e.X && GetRightFrameOfCloseButton() > e.X)
		{
			((ListBox)this).BeginUpdate();
			int num = ((ListBox)this).Items.IndexOf((object)itemAt);
			((ListBox)this).Items.Remove((object)itemAt);
			if (((ListBox)this).Items.Count > 0)
			{
				if (num >= ((ListBox)this).Items.Count)
				{
					((ListControl)this).SelectedIndex = ((ListBox)this).Items.Count - 1;
				}
				else
				{
					((ListControl)this).SelectedIndex = 0;
				}
			}
			((ListBox)this).EndUpdate();
		}
		else
		{
			SelectedParameter = itemAt;
		}
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
		isDragging = false;
		((Control)this).Invalidate();
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Invalid comparison between Unknown and I4
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).OnMouseMove(e);
		IsCloseButtonOver = GetLeftFrameOfCloseButton() < e.X && e.X < GetRightFrameOfCloseButton();
		RefreshHotTracking();
		if (AllowDragItems && !isDragging && (int)e.Button == 1048576 && IsOutsideDragZone(e.Location) && ((ListBox)this).SelectedItem != null)
		{
			isDragging = true;
			((Control)this).DoDragDrop(((ListBox)this).SelectedItem, (DragDropEffects)2);
		}
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
		IsCloseButtonOver = false;
		RefreshHotTracking();
	}

	protected override void OnMouseHover(EventArgs e)
	{
		((Control)this).OnMouseHover(e);
		RefreshHotTracking();
	}

	protected override void OnDragDrop(DragEventArgs e)
	{
		((Control)this).OnDragDrop(e);
		isDragging = false;
		if (!(e.Data.GetData(typeof(StiDataParameter)) is StiDataParameter stiDataParameter))
		{
			return;
		}
		StiDataParameter selectedParameter = SelectedParameter;
		if (stiDataParameter != selectedParameter)
		{
			((ListBox)this).BeginUpdate();
			int num = ((selectedParameter != null) ? ((ListBox)this).Items.IndexOf((object)selectedParameter) : (-1));
			((ListBox)this).Items.Remove((object)stiDataParameter);
			if (num != -1)
			{
				((ListBox)this).Items.Insert(num, (object)stiDataParameter);
			}
			SelectedParameter = stiDataParameter;
			((ListBox)this).EndUpdate();
		}
	}

	protected override void OnDragEnter(DragEventArgs e)
	{
		((Control)this).OnDragOver(e);
		if (e.Data.GetData(typeof(StiDataParameter)) is StiDataParameter)
		{
			e.Effect = (DragDropEffects)2;
		}
		else
		{
			e.Effect = (DragDropEffects)0;
		}
	}

	protected override void OnDragLeave(EventArgs e)
	{
		((Control)this).OnDragLeave(e);
		isDragging = false;
		((Control)this).Invalidate();
	}

	protected override void OnDragOver(DragEventArgs e)
	{
		((Control)this).OnDragOver(e);
		StiDataParameter itemAt = GetItemAt(((Control)this).PointToClient(Cursor.Position));
		if (itemAt != null)
		{
			SelectedParameter = itemAt;
		}
	}

	private bool IsOutsideDragZone(Point location)
	{
		int width = SystemInformation.DragSize.Width;
		int height = SystemInformation.DragSize.Height;
		return !new Rectangle(dragOrigin.X - width / 2, dragOrigin.Y - height / 2, width, height).Contains(location);
	}

	public void DeleteItem()
	{
		if (SelectedParameter == null)
		{
			return;
		}
		((ListBox)this).BeginUpdate();
		int selectedIndex = ((ListControl)this).SelectedIndex;
		((ListBox)this).Items.Remove((object)SelectedParameter);
		if (((ListBox)this).Items.Count > 0)
		{
			if (selectedIndex >= ((ListBox)this).Items.Count)
			{
				((ListControl)this).SelectedIndex = ((ListBox)this).Items.Count - 1;
			}
			else
			{
				((ListControl)this).SelectedIndex = selectedIndex;
			}
		}
		((ListBox)this).EndUpdate();
	}

	public void MoveItemUp()
	{
		try
		{
			((ListBox)this).BeginUpdate();
			int num = ((ListBox)this).Items.IndexOf((object)SelectedParameter);
			if (num > 0)
			{
				StiDataParameter selectedParameter = SelectedParameter;
				((ListBox)this).Items.Remove((object)selectedParameter);
				((ListBox)this).Items.Insert(num - 1, (object)selectedParameter);
				SelectedParameter = selectedParameter;
			}
			((ListBox)this).EndUpdate();
		}
		catch
		{
		}
	}

	public void MoveItemDown()
	{
		try
		{
			((ListBox)this).BeginUpdate();
			int num = ((ListBox)this).Items.IndexOf((object)SelectedParameter);
			if (num < ((ListBox)this).Items.Count - 1)
			{
				StiDataParameter selectedParameter = SelectedParameter;
				((ListBox)this).Items.Remove((object)selectedParameter);
				((ListBox)this).Items.Insert(num + 1, (object)selectedParameter);
				SelectedParameter = selectedParameter;
			}
			((ListBox)this).EndUpdate();
		}
		catch
		{
		}
	}

	private void InvalidateCloseButton()
	{
		Rectangle rectangle = new Rectangle(((Control)this).Width - StiListBoxPainter.CloseButtonWidth - 1, 1, StiListBoxPainter.CloseButtonWidth, ((Control)this).Height - 2);
		if (base.IsVerticalScrollBarVisible)
		{
			rectangle.X -= SystemInformation.VerticalScrollBarWidth;
		}
		((Control)this).Invalidate(rectangle);
	}

	public int GetItemIndexAt(Point p)
	{
		return ((ListBox)this).IndexFromPoint(p);
	}

	public StiDataParameter GetItemAt(Point p)
	{
		int itemIndexAt = GetItemIndexAt(p);
		if (itemIndexAt < 0 || itemIndexAt >= ((ListBox)this).Items.Count)
		{
			return null;
		}
		return ((ListBox)this).Items[itemIndexAt] as StiDataParameter;
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

	private int GetRightFrameOfCloseButton()
	{
		return ((Control)this).Width - (base.IsVerticalScrollBarVisible ? SystemInformation.VerticalScrollBarWidth : 0);
	}

	private int GetLeftFrameOfCloseButton()
	{
		return ((Control)this).Width - StiListBoxPainter.CloseButtonWidth - (base.IsVerticalScrollBarVisible ? SystemInformation.VerticalScrollBarWidth : 0);
	}

	public StiDataParameterListBox()
	{
		((Control)this).AllowDrop = true;
	}
}
