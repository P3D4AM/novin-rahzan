using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Dashboard.Components.Chart;
using Stimulsoft.Report.Design.Components;
using Stimulsoft.Report.Design.Controls;

namespace Stimulsoft.Dashboard.Design.Controls;

public sealed class StiChartConstantLineListBox : StiItemListBox
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
	public StiChartConstantLines SelectedConstantLine
	{
		get
		{
			if (((ListControl)this).SelectedIndex < 0 || ((ListControl)this).SelectedIndex >= ((ListBox)this).Items.Count)
			{
				return null;
			}
			return ((ListBox)this).Items[((ListControl)this).SelectedIndex] as StiChartConstantLines;
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
			StiListBoxPainter.PaintLabel(e.Graphics, ((Control)this).ClientRectangle, Loc.Get("Dashboard", "DragDropDataFromDictionary"));
		}
		OnPaintItems(e.Graphics);
	}

	protected override void OnPaintBackground(PaintEventArgs e)
	{
		StiListBoxPainter.PaintBorder(e.Graphics, ((Control)this).ClientRectangle, ((ListBox)this).Items.Count);
	}

	protected override void OnDrawItem(DrawItemEventArgs e)
	{
		if (e.Index != -1 && ((ListBox)this).Items[e.Index] is StiChartConstantLines stiChartConstantLines)
		{
			Graphics graphics = e.Graphics;
			bool flag = ((ListControl)this).SelectedIndex == e.Index;
			bool flag2 = e.Index == hotTrackingIndex;
			Color color = ((stiChartConstantLines.LineColor == Color.Transparent) ? Color.DimGray : stiChartConstantLines.LineColor);
			Color backgroundColor = StiListBoxPainter.GetBackgroundColor(flag, flag2, IsCurrentlySelected);
			Color foregroundColor = StiListBoxPainter.GetForegroundColor(flag, flag2, IsCurrentlySelected, color);
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
			OnPaintAxisTitle(graphics, stiChartConstantLines, foregroundColor, bounds);
			OnPaintAxisValue(stiChartConstantLines, graphics, foregroundColor, bounds);
			OnPaintConstantLine(graphics, stiChartConstantLines, color, bounds);
			if (flag2)
			{
				StiListBoxPainter.PaintCloseButton(e.Graphics, e.Bounds, e.Index == 0, IsCloseButtonOver);
			}
		}
	}

	private Size GetAxisValueSize(StiChartConstantLines line)
	{
		if (!string.IsNullOrWhiteSpace(line.AxisValue))
		{
			return TextRenderer.MeasureText(line.AxisValue, ((Control)this).Font);
		}
		return Size.Empty;
	}

	private void OnPaintConstantLine(Graphics g, StiChartConstantLines line, Color lineColor, Rectangle rect)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		Size axisValueSize = GetAxisValueSize(line);
		if (!axisValueSize.IsEmpty)
		{
			rect.X += axisValueSize.Width;
			rect.Width -= axisValueSize.Width;
		}
		float num = Math.Max(line.LineWidth, 1f);
		Pen val = new Pen(lineColor, num);
		try
		{
			val.DashStyle = StiPenUtils.GetPenStyle(line.LineStyle);
			g.DrawLine(val, rect.X + StiScaleUI.I4, rect.Y + rect.Height / 2, rect.Right - StiScaleUI.I4, rect.Y + rect.Height / 2);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void OnPaintAxisValue(StiChartConstantLines line, Graphics g, Color foreground, Rectangle rect)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		Size axisValueSize = GetAxisValueSize(line);
		if (!axisValueSize.IsEmpty)
		{
			TextFormatFlags val = (TextFormatFlags)32804;
			Rectangle rectangle = new Rectangle(rect.X, rect.Y + (rect.Height - axisValueSize.Height) / 2, axisValueSize.Width, axisValueSize.Height);
			TextRenderer.DrawText((IDeviceContext)(object)g, line.AxisValue, ((Control)this).Font, rectangle, foreground, val);
		}
	}

	private void OnPaintAxisTitle(Graphics g, StiChartConstantLines line, Color foreground, Rectangle rect)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		TextFormatFlags val = (TextFormatFlags)32804;
		Size axisValueSize = GetAxisValueSize(line);
		Rectangle rectangle = rect;
		rectangle.X += StiScaleUI.I4;
		rectangle.Width -= StiScaleUI.I4;
		switch (line.Position)
		{
		case StiTextPosition.LeftTop:
		case StiTextPosition.CenterTop:
		case StiTextPosition.RightTop:
			rectangle.Height /= 2;
			break;
		case StiTextPosition.LeftBottom:
		case StiTextPosition.CenterBottom:
		case StiTextPosition.RightBottom:
			rectangle.Height /= 2;
			rectangle.Y += rectangle.Height;
			break;
		}
		switch (line.Position)
		{
		case StiTextPosition.LeftTop:
		case StiTextPosition.LeftBottom:
			val = (TextFormatFlags)(val | 0);
			break;
		case StiTextPosition.CenterTop:
		case StiTextPosition.CenterBottom:
			val = (TextFormatFlags)(val | 1);
			break;
		case StiTextPosition.RightTop:
		case StiTextPosition.RightBottom:
			val = (TextFormatFlags)(val | 2);
			break;
		}
		if (!axisValueSize.IsEmpty)
		{
			rectangle.X += axisValueSize.Width;
			rectangle.Width -= axisValueSize.Width;
		}
		string text = (string.IsNullOrWhiteSpace(line.Text) ? ("(" + Loc.Get("Chart", "ConstantLine") + ")") : line.Text);
		TextRenderer.DrawText((IDeviceContext)(object)g, text, ((Control)this).Font, rectangle, foreground, val);
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
		StiChartConstantLines itemAt = GetItemAt(e.Location);
		if (itemAt == null)
		{
			return;
		}
		if (GetLeftFrameOfCloseButton() < e.X && GetRightFrameOfCloseButton() > e.X)
		{
			((ListBox)this).BeginUpdate();
			itemAt.Text = itemAt.Text;
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
			SelectedConstantLine = itemAt;
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
		if (!(e.Data.GetData(typeof(StiChartConstantLines)) is StiChartConstantLines stiChartConstantLines))
		{
			return;
		}
		StiChartConstantLines selectedConstantLine = SelectedConstantLine;
		if (stiChartConstantLines != selectedConstantLine)
		{
			((ListBox)this).BeginUpdate();
			int num = ((selectedConstantLine != null) ? ((ListBox)this).Items.IndexOf((object)selectedConstantLine) : (-1));
			((ListBox)this).Items.Remove((object)stiChartConstantLines);
			if (num != -1)
			{
				((ListBox)this).Items.Insert(num, (object)stiChartConstantLines);
			}
			SelectedConstantLine = stiChartConstantLines;
			((ListBox)this).EndUpdate();
		}
	}

	protected override void OnDragEnter(DragEventArgs e)
	{
		((Control)this).OnDragOver(e);
		if (e.Data.GetData(typeof(StiChartConstantLines)) is StiChartConstantLines)
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
		StiChartConstantLines itemAt = GetItemAt(((Control)this).PointToClient(Cursor.Position));
		if (itemAt != null)
		{
			SelectedConstantLine = itemAt;
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
		if (SelectedConstantLine == null)
		{
			return;
		}
		((ListBox)this).BeginUpdate();
		int selectedIndex = ((ListControl)this).SelectedIndex;
		((ListBox)this).Items.Remove((object)SelectedConstantLine);
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
			int num = ((ListBox)this).Items.IndexOf((object)SelectedConstantLine);
			if (num > 0)
			{
				StiChartConstantLines selectedConstantLine = SelectedConstantLine;
				((ListBox)this).Items.Remove((object)selectedConstantLine);
				((ListBox)this).Items.Insert(num - 1, (object)selectedConstantLine);
				SelectedConstantLine = selectedConstantLine;
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
			int num = ((ListBox)this).Items.IndexOf((object)SelectedConstantLine);
			if (num < ((ListBox)this).Items.Count - 1)
			{
				StiChartConstantLines selectedConstantLine = SelectedConstantLine;
				((ListBox)this).Items.Remove((object)selectedConstantLine);
				((ListBox)this).Items.Insert(num + 1, (object)selectedConstantLine);
				SelectedConstantLine = selectedConstantLine;
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

	public StiChartConstantLines GetItemAt(Point p)
	{
		int itemIndexAt = GetItemIndexAt(p);
		if (itemIndexAt < 0 || itemIndexAt >= ((ListBox)this).Items.Count)
		{
			return null;
		}
		return ((ListBox)this).Items[itemIndexAt] as StiChartConstantLines;
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

	public StiChartConstantLineListBox()
	{
		((Control)this).AllowDrop = true;
	}
}
