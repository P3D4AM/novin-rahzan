using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Design.Components;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Chart.Design;

public sealed class StiSeriesListBox : StiItemListBox
{
	private class StiDraggableItem
	{
		public StiSeries Series { get; set; }

		public StiDraggableItem(StiSeries series)
		{
			Series = series;
		}
	}

	private int hotTrackingIndex = -1;

	private int currentSelectedIndex = -1;

	private Point dragOrigin;

	private bool isDragging;

	private bool isCurrentlySelected;

	private bool isCloseButtonOver;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiChartSeriesEditorForm Editor { get; set; }

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
	public StiSeries SelectedSeries
	{
		get
		{
			if (((ListControl)this).SelectedIndex < 0 || ((ListControl)this).SelectedIndex >= ((ListBox)this).Items.Count)
			{
				return null;
			}
			return ((ListBox)this).Items[((ListControl)this).SelectedIndex] as StiSeries;
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
		ApplyCurrentlySelected();
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

	protected override void OnGotFocus(EventArgs e)
	{
		((Control)this).OnGotFocus(e);
		ApplyCurrentlySelected();
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
		if (e.Index != -1 && ((ListBox)this).Items[e.Index] is StiSeries series)
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
			OnPaintName(graphics, series, foregroundColor, bounds);
			OnPaintItemImage(e, flag);
			if (flag2)
			{
				StiListBoxPainter.PaintCloseButton(e.Graphics, e.Bounds, e.Index == 0, IsCloseButtonOver);
			}
		}
	}

	private void OnPaintItemImage(DrawItemEventArgs e, bool isSelected)
	{
		Graphics graphics = e.Graphics;
		Rectangle rectangle = new Rectangle(0, e.Bounds.Y, ((Control)this).Width, e.Bounds.Height);
		Image image = GetImage(((ListBox)this).Items[e.Index] as StiSeries, isSelected);
		if (image != null)
		{
			Rectangle rectangle2 = new Rectangle(rectangle.X + StiScale.I3, rectangle.Y + (rectangle.Height - image.Height) / 2, image.Width, image.Height);
			graphics.DrawImage(image, rectangle2);
			image.Dispose();
		}
	}

	private void OnPaintName(Graphics g, StiSeries series, Color foreground, Rectangle rect)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		TextFormatFlags val = (TextFormatFlags)32804;
		rect.X += StiScale.I20;
		rect.Width -= StiScale.I20;
		TextRenderer.DrawText((IDeviceContext)(object)g, (string)series.Title, ((Control)this).Font, rect, foreground, val);
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
		StiSeries itemAt = GetItemAt(e.Location);
		if (itemAt == null)
		{
			return;
		}
		if (GetLeftFrameOfCloseButton() < e.X && GetRightFrameOfCloseButton() > e.X)
		{
			((ListBox)this).BeginUpdate();
			int num = ((ListBox)this).Items.IndexOf((object)itemAt);
			((ListBox)this).Items.Remove((object)itemAt);
			itemAt.Chart.Series.Remove(itemAt);
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
			Editor?.UpdateChartOnDesign();
		}
		else
		{
			SelectedSeries = itemAt;
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
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).OnMouseMove(e);
		IsCloseButtonOver = GetLeftFrameOfCloseButton() < e.X && e.X < GetRightFrameOfCloseButton();
		RefreshHotTracking();
		if (AllowDragItems && !isDragging && (int)e.Button == 1048576 && IsOutsideDragZone(e.Location) && ((ListBox)this).SelectedItem != null)
		{
			isDragging = true;
			((Control)this).DoDragDrop((object)new StiDraggableItem(SelectedSeries), (DragDropEffects)2);
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
		StiSeries stiSeries = ((e.Data.GetData(typeof(StiDraggableItem)) is StiDraggableItem stiDraggableItem) ? stiDraggableItem.Series : null);
		if (stiSeries == null)
		{
			return;
		}
		StiSeries selectedSeries = SelectedSeries;
		if (stiSeries != selectedSeries)
		{
			((ListBox)this).BeginUpdate();
			int num = ((selectedSeries != null) ? ((ListBox)this).Items.IndexOf((object)selectedSeries) : (-1));
			((ListBox)this).Items.Remove((object)stiSeries);
			stiSeries.Chart.Series.Remove(stiSeries);
			if (num != -1)
			{
				((ListBox)this).Items.Insert(num, (object)stiSeries);
				stiSeries.Chart.Series.Insert(num, stiSeries);
			}
			SelectedSeries = stiSeries;
			((ListBox)this).EndUpdate();
			Editor?.UpdateChartOnDesign();
		}
	}

	protected override void OnDragEnter(DragEventArgs e)
	{
		((Control)this).OnDragOver(e);
		StiSeries stiSeries = ((e.Data.GetData(typeof(StiDraggableItem)) is StiDraggableItem stiDraggableItem) ? stiDraggableItem.Series : null);
		if (stiSeries != null)
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
		StiSeries itemAt = GetItemAt(((Control)this).PointToClient(Cursor.Position));
		if (itemAt != null)
		{
			SelectedSeries = itemAt;
		}
	}

	private void ApplyCurrentlySelected()
	{
		if (((ListBox)this).Items.Count != 0)
		{
			IEnumerable<StiSeriesListBox> source = (from c in GetAll((Control)(object)Editor, typeof(StiSeriesListBox))
				where c != this
				select c).Cast<StiSeriesListBox>();
			source.ToList().ForEach(delegate(StiSeriesListBox c)
			{
				c.IsCurrentlySelected = false;
			});
			IsCurrentlySelected = true;
		}
	}

	public IEnumerable<Control> GetAll(Control control, Type type)
	{
		if (control == null)
		{
			return new List<Control>();
		}
		IEnumerable<Control> enumerable = ((IEnumerable)control.Controls).Cast<Control>();
		return from c in enumerable.SelectMany((Control ctrl) => GetAll(ctrl, type)).Concat(enumerable)
			where ((object)c).GetType() == type
			select c;
	}

	private Image GetImage(StiSeries item, bool isSelected)
	{
		string text = item.ComponentId.ToString().Substring(3);
		string text2 = (isSelected ? "White" : "");
		if (text.EndsWith("Series"))
		{
			return StiReportDesignImages.GetImage("Meters.Charts." + text2 + text.Replace("Series", ""), StiImageSize.Normal, allowCache: false);
		}
		return null;
	}

	private bool IsOutsideDragZone(Point location)
	{
		int width = SystemInformation.DragSize.Width;
		int height = SystemInformation.DragSize.Height;
		return !new Rectangle(dragOrigin.X - width / 2, dragOrigin.Y - height / 2, width, height).Contains(location);
	}

	public void DeleteItem()
	{
		if (SelectedSeries == null)
		{
			return;
		}
		((ListBox)this).BeginUpdate();
		int selectedIndex = ((ListControl)this).SelectedIndex;
		((ListBox)this).Items.Remove((object)SelectedSeries);
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
		SelectedSeries.Chart.Series.Remove(SelectedSeries);
		((ListBox)this).EndUpdate();
		Editor?.UpdateChartOnDesign();
	}

	public void MoveItemUp()
	{
		try
		{
			((ListBox)this).BeginUpdate();
			int num = ((ListBox)this).Items.IndexOf((object)SelectedSeries);
			if (num > 0)
			{
				StiSeries selectedSeries = SelectedSeries;
				((ListBox)this).Items.Remove((object)selectedSeries);
				((ListBox)this).Items.Insert(num - 1, (object)selectedSeries);
				SelectedSeries = selectedSeries;
				selectedSeries.Chart.Series.Remove(selectedSeries);
				selectedSeries.Chart.Series.Insert(num - 1, selectedSeries);
			}
			((ListBox)this).EndUpdate();
			Editor?.UpdateChartOnDesign();
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
			int num = ((ListBox)this).Items.IndexOf((object)SelectedSeries);
			if (num < ((ListBox)this).Items.Count - 1)
			{
				StiSeries selectedSeries = SelectedSeries;
				((ListBox)this).Items.Remove((object)selectedSeries);
				((ListBox)this).Items.Insert(num + 1, (object)selectedSeries);
				SelectedSeries = selectedSeries;
				selectedSeries.Chart.Series.Remove(selectedSeries);
				selectedSeries.Chart.Series.Insert(num + 1, selectedSeries);
			}
			((ListBox)this).EndUpdate();
			Editor?.UpdateChartOnDesign();
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

	public StiSeries GetItemAt(Point p)
	{
		int itemIndexAt = GetItemIndexAt(p);
		if (itemIndexAt < 0 || itemIndexAt >= ((ListBox)this).Items.Count)
		{
			return null;
		}
		return ((ListBox)this).Items[itemIndexAt] as StiSeries;
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

	public StiSeriesListBox()
	{
		((Control)this).AllowDrop = true;
	}
}
