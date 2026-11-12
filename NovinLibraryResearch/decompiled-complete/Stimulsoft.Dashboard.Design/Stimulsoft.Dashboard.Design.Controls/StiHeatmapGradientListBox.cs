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
using Stimulsoft.Dashboard.Components.OnlineMap;
using Stimulsoft.Dashboard.Design.Editors;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Components;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Dashboard.Design.Controls;

public sealed class StiHeatmapGradientListBox : StiItemListBox
{
	private int hotTrackingIndex = -1;

	private int currentSelectedIndex = -1;

	private Point dragOrigin;

	private bool isDragging;

	private bool isCurrentlySelected;

	private bool isCloseButtonOver;

	private bool isDragOverFromDictionary;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool AllowImage { get; set; } = true;


	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool AllowMultipleElements { get; set; } = true;


	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiDesignerControl Designer { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiElementEditorForm Editor { get; set; }

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
	private bool IsDragOverFromDictionary
	{
		get
		{
			return isDragOverFromDictionary;
		}
		set
		{
			if (isDragOverFromDictionary != value)
			{
				isDragOverFromDictionary = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiHeatmapGradientItem SelectedRangeItem
	{
		get
		{
			if (((ListControl)this).SelectedIndex < 0 || ((ListControl)this).SelectedIndex >= ((ListBox)this).Items.Count)
			{
				return null;
			}
			return ((ListBox)this).Items[((ListControl)this).SelectedIndex] as StiHeatmapGradientItem;
		}
		set
		{
			((ListControl)this).SelectedIndex = ((value != null) ? ((ListBox)this).Items.IndexOf((object)value) : (-1));
		}
	}

	public event EventHandler ElementsConverting;

	public event EventHandler ElementsChanged;

	public event EventHandler ElementsDragEnter;

	public event EventHandler ElementsDragDrop;

	public event EventHandler ElementsSelectedChanged;

	protected override void OnSelectedIndexChanged(EventArgs e)
	{
		if (currentSelectedIndex != -1 && currentSelectedIndex != ((ListControl)this).SelectedIndex)
		{
			InvalidateItem(currentSelectedIndex);
		}
		((ListBox)this).OnSelectedIndexChanged(e);
		InvalidateItem(((ListControl)this).SelectedIndex);
		currentSelectedIndex = ((ListControl)this).SelectedIndex;
		if (SelectedRangeItem != null)
		{
			this.ElementsSelectedChanged?.Invoke(this, e);
		}
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
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Invalid comparison between Unknown and I4
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Invalid comparison between Unknown and I4
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Invalid comparison between Unknown and I4
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Invalid comparison between Unknown and I4
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Invalid comparison between Unknown and I4
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Invalid comparison between Unknown and I4
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Invalid comparison between Unknown and I4
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Invalid comparison between Unknown and I4
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Invalid comparison between Unknown and I4
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Invalid comparison between Unknown and I4
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Invalid comparison between Unknown and I4
		((Control)this).OnKeyDown(e);
		if ((int)e.KeyCode == 46)
		{
			e.Handled = true;
			DeleteItem();
		}
		if ((int)e.KeyCode == 68 && (Control.ModifierKeys & 0x20000) > 0)
		{
			e.Handled = true;
			DuplicateItem();
		}
		if ((int)e.KeyCode == 67 && (Control.ModifierKeys & 0x20000) > 0)
		{
			e.Handled = true;
			CopyItemToClipboard();
		}
		if ((int)e.KeyCode == 45 && (Control.ModifierKeys & 0x20000) > 0)
		{
			e.Handled = true;
			CopyItemToClipboard();
		}
		if ((int)e.KeyCode == 86 && (Control.ModifierKeys & 0x20000) > 0)
		{
			e.Handled = true;
			PasteItemFromClipboard();
		}
		if ((int)e.KeyCode == 45 && (Control.ModifierKeys & 0x10000) > 0)
		{
			e.Handled = true;
			PasteItemFromClipboard();
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
			StiListBoxPainter.PaintLabel(e.Graphics, ((Control)this).ClientRectangle, Loc.Get("Dashboard", "NoRanges"));
		}
		OnPaintItems(e.Graphics);
	}

	protected override void OnPaintBackground(PaintEventArgs e)
	{
		StiListBoxPainter.PaintBorder(e.Graphics, ((Control)this).ClientRectangle, ((ListBox)this).Items.Count, IsDragOverFromDictionary);
	}

	protected override void OnDrawItem(DrawItemEventArgs e)
	{
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Expected O, but got Unknown
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		if (e.Index != -1)
		{
			Graphics graphics = e.Graphics;
			bool flag = ((ListControl)this).SelectedIndex == e.Index;
			bool flag2 = e.Index == hotTrackingIndex;
			Color backgroundColor = StiListBoxPainter.GetBackgroundColor(flag, flag2, IsCurrentlySelected);
			Color foregroundColor = StiListBoxPainter.GetForegroundColor(flag, flag2, IsCurrentlySelected);
			Rectangle bounds = e.Bounds;
			bounds.Inflate(-2, -1);
			if (e.Index == 0)
			{
				bounds.Y++;
				bounds.Height--;
			}
			StiHeatmapGradientItem stiHeatmapGradientItem = ((ListBox)this).Items[e.Index] as StiHeatmapGradientItem;
			StiDrawing.FillRectangle(graphics, backgroundColor, bounds);
			if (flag && ((Control)this).Focused && StiUX.IsDark)
			{
				StiDrawing.FillRectangle(graphics, StiUX.Accent.ChangeAlpha(50), bounds);
			}
			SolidBrush val = new SolidBrush(stiHeatmapGradientItem.Range.Color);
			try
			{
				TextFormatFlags val2 = (TextFormatFlags)32804;
				int num = (int)((double)(AllowImage ? 24 : 4) * StiScale.X);
				bounds.X += num;
				bounds.Width -= num;
				int num2 = (int)(24.0 * StiScale.X);
				Rectangle rectangle = new Rectangle(bounds.X, bounds.Y + StiScale.I4, num2, bounds.Height - StiScale.I8);
				rectangle.Inflate(-StiScale.I2, -StiScale.I2);
				graphics.FillRectangle((Brush)(object)val, rectangle);
				bounds.X += num2;
				bounds.Width -= num2;
				TextRenderer.DrawText((IDeviceContext)(object)graphics, stiHeatmapGradientItem.Range.GetStringRepresentation(), ((Control)this).Font, bounds, foregroundColor, val2);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			if (AllowImage)
			{
				OnPaintItemImage(e, flag);
			}
			if (flag2)
			{
				StiListBoxPainter.PaintCloseButton(e.Graphics, e.Bounds, e.Index == 0, IsCloseButtonOver);
			}
		}
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

	private void OnPaintItemImage(DrawItemEventArgs e, bool isSelected)
	{
		Graphics graphics = e.Graphics;
		Rectangle rectangle = new Rectangle(0, e.Bounds.Y, ((Control)this).Width, e.Bounds.Height);
		Image image = GetImage(((ListBox)this).Items[e.Index] as StiHeatmapGradientItem, isSelected);
		if (image != null)
		{
			Rectangle rectangle2 = new Rectangle(rectangle.X + StiScale.I3, rectangle.Y + (rectangle.Height - image.Height) / 2, image.Width, image.Height);
			graphics.DrawImage(image, rectangle2);
			image.Dispose();
		}
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		((Control)this).OnMouseDown(e);
		((Control)this).Focus();
		dragOrigin = e.Location;
		StiHeatmapGradientItem itemAt = GetItemAt(e.Location);
		if (itemAt == null)
		{
			return;
		}
		if (GetLeftFrameOfCloseButton() < e.X && GetRightFrameOfCloseButton() > e.X)
		{
			((ListBox)this).BeginUpdate();
			StiHeatmapGradientItem selectedRangeItem = null;
			if (SelectedRangeItem == itemAt)
			{
				if (itemAt.NextItem != null)
				{
					selectedRangeItem = itemAt.NextItem;
				}
				else if (itemAt.PrevItem != null)
				{
					selectedRangeItem = itemAt.PrevItem;
				}
			}
			SelectedRangeItem = selectedRangeItem;
			((ListBox)this).Items.Remove((object)itemAt);
			((ListBox)this).EndUpdate();
			this.ElementsChanged?.Invoke(this, (EventArgs)(object)e);
		}
		else
		{
			SelectedRangeItem = itemAt;
		}
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
		IsDragOverFromDictionary = false;
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
		IsDragOverFromDictionary = false;
		object data = e.Data.GetData(typeof(StiDictionaryControl));
		if (data != null && StiDragDropColumnHelper.IsDataColumnOrTableElementDragged(Designer))
		{
			this.ElementsDragDrop?.Invoke(this, EventArgs.Empty);
		}
		else
		{
			if (!(e.Data.GetData(typeof(StiHeatmapGradientItem)) is StiHeatmapGradientItem stiHeatmapGradientItem))
			{
				return;
			}
			StiHeatmapGradientItem selectedRangeItem = SelectedRangeItem;
			if (stiHeatmapGradientItem != selectedRangeItem)
			{
				((ListBox)this).BeginUpdate();
				int num = ((selectedRangeItem != null) ? ((ListBox)this).Items.IndexOf((object)selectedRangeItem) : (-1));
				StiHeatmapGradientListBox list = stiHeatmapGradientItem.List;
				((ListBox)list).Items.Remove((object)stiHeatmapGradientItem);
				if (AllowMultipleElements && num != -1)
				{
					InsertItem(num, stiHeatmapGradientItem);
				}
				else
				{
					((ListBox)this).Items.Clear();
					AddItem(stiHeatmapGradientItem);
				}
				if (list != this)
				{
					this.ElementsConverting?.Invoke(stiHeatmapGradientItem, (EventArgs)(object)e);
				}
				SelectedRangeItem = stiHeatmapGradientItem;
				((ListBox)this).EndUpdate();
				this.ElementsChanged?.Invoke(this, (EventArgs)(object)e);
			}
		}
	}

	protected override void OnDragEnter(DragEventArgs e)
	{
		((Control)this).OnDragOver(e);
		object data = e.Data.GetData(typeof(StiDictionaryControl));
		if (data != null && StiDragDropColumnHelper.IsDataColumnOrTableElementDragged(Designer))
		{
			e.Effect = (DragDropEffects)2;
			this.ElementsDragEnter?.Invoke(this, EventArgs.Empty);
			IsDragOverFromDictionary = true;
		}
		else if (e.Data.GetData(typeof(StiHeatmapGradientItem)) is StiHeatmapGradientItem stiHeatmapGradientItem && stiHeatmapGradientItem.List != this)
		{
			e.Effect = (DragDropEffects)2;
			this.ElementsDragEnter?.Invoke(this, EventArgs.Empty);
			IsDragOverFromDictionary = true;
		}
		else if (e.Data.GetData(typeof(StiHeatmapGradientItem)) is StiHeatmapGradientItem)
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
		isDragOverFromDictionary = false;
		((Control)this).Invalidate();
	}

	protected override void OnDragOver(DragEventArgs e)
	{
		((Control)this).OnDragOver(e);
		StiHeatmapGradientItem itemAt = GetItemAt(((Control)this).PointToClient(Cursor.Position));
		if (itemAt != null)
		{
			SelectedRangeItem = itemAt;
		}
	}

	private void ApplyCurrentlySelected()
	{
		if (((ListBox)this).Items.Count != 0)
		{
			IEnumerable<StiHeatmapGradientListBox> source = (from c in GetAll((Control)(object)Editor, typeof(StiHeatmapGradientListBox))
				where c != this
				select c).Cast<StiHeatmapGradientListBox>();
			source.ToList().ForEach(delegate(StiHeatmapGradientListBox c)
			{
				c.IsCurrentlySelected = false;
			});
			IsCurrentlySelected = true;
		}
	}

	public IEnumerable<Control> GetAll(Control control, Type type)
	{
		IEnumerable<Control> enumerable = ((IEnumerable)control.Controls).Cast<Control>();
		return from c in enumerable.SelectMany((Control ctrl) => GetAll(ctrl, type)).Concat(enumerable)
			where ((object)c).GetType() == type
			select c;
	}

	private bool IsOutsideDragZone(Point location)
	{
		int width = SystemInformation.DragSize.Width;
		int height = SystemInformation.DragSize.Height;
		return !new Rectangle(dragOrigin.X - width / 2, dragOrigin.Y - height / 2, width, height).Contains(location);
	}

	private Image GetImage(StiHeatmapGradientItem item, bool isSelected)
	{
		return null;
	}

	public void DuplicateItem()
	{
		if (SelectedRangeItem != null)
		{
			((ListBox)this).BeginUpdate();
			StiHeatmapGradientItem stiHeatmapGradientItem = SelectedRangeItem.Clone() as StiHeatmapGradientItem;
			stiHeatmapGradientItem.Range = stiHeatmapGradientItem.Range.Clone() as StiHeatmapGradientRange;
			int num = ((ListBox)this).Items.IndexOf((object)SelectedRangeItem);
			if (num != -1)
			{
				InsertItem(num + 1, stiHeatmapGradientItem);
			}
			else
			{
				AddItem(stiHeatmapGradientItem);
			}
			SelectedRangeItem = stiHeatmapGradientItem;
			((ListBox)this).EndUpdate();
			this.ElementsConverting?.Invoke(stiHeatmapGradientItem, EventArgs.Empty);
			this.ElementsChanged?.Invoke(this, EventArgs.Empty);
			Editor.RefreshElementView();
		}
	}

	public void DeleteItem()
	{
		if (SelectedRangeItem != null)
		{
			((ListBox)this).BeginUpdate();
			StiHeatmapGradientItem selectedRangeItem = null;
			if (SelectedRangeItem.NextItem != null)
			{
				selectedRangeItem = SelectedRangeItem.NextItem;
			}
			else if (SelectedRangeItem.PrevItem != null)
			{
				selectedRangeItem = SelectedRangeItem.PrevItem;
			}
			((ListBox)this).Items.Remove((object)SelectedRangeItem);
			SelectedRangeItem = selectedRangeItem;
			((ListBox)this).EndUpdate();
			this.ElementsChanged?.Invoke(this, EventArgs.Empty);
			Editor.RefreshElementView();
		}
	}

	public void Clear()
	{
		if (SelectedRangeItem != null)
		{
			((ListBox)this).BeginUpdate();
			((ListBox)this).Items.Clear();
			((ListBox)this).EndUpdate();
			this.ElementsChanged?.Invoke(this, EventArgs.Empty);
			Editor.RefreshElementView();
		}
	}

	public void CopyItemToClipboard()
	{
		if (SelectedRangeItem != null && SelectedRangeItem.Range != null)
		{
			string text = SelectedRangeItem.Range.SaveToString();
			Clipboard.SetText(text);
		}
	}

	public void PasteItemFromClipboard()
	{
		try
		{
			((ListBox)this).BeginUpdate();
			string text = Clipboard.GetText();
			StiHeatmapGradientRange range = StiHeatmapGradientRange.LoadFromJson(text);
			StiHeatmapGradientItem stiHeatmapGradientItem = new StiHeatmapGradientItem
			{
				Range = range
			};
			if (!AllowMultipleElements)
			{
				((ListBox)this).Items.Clear();
			}
			int num = ((ListBox)this).Items.IndexOf((object)SelectedRangeItem);
			if (num != -1)
			{
				InsertItem(num, stiHeatmapGradientItem);
			}
			else
			{
				AddItem(stiHeatmapGradientItem);
			}
			SelectedRangeItem = stiHeatmapGradientItem;
			((ListBox)this).EndUpdate();
			this.ElementsConverting?.Invoke(stiHeatmapGradientItem, EventArgs.Empty);
			this.ElementsChanged?.Invoke(this, EventArgs.Empty);
			Editor.RefreshElementView();
		}
		catch
		{
		}
	}

	public void MoveItemUp()
	{
		try
		{
			((ListBox)this).BeginUpdate();
			Editor.LockRefresh = true;
			int num = ((ListBox)this).Items.IndexOf((object)SelectedRangeItem);
			if (num > 0)
			{
				StiHeatmapGradientItem selectedRangeItem = SelectedRangeItem;
				((ListBox)this).Items.Remove((object)selectedRangeItem);
				InsertItem(num - 1, selectedRangeItem);
				SelectedRangeItem = selectedRangeItem;
			}
			Editor.LockRefresh = false;
			((ListBox)this).EndUpdate();
			this.ElementsChanged?.Invoke(this, EventArgs.Empty);
			Editor.RefreshElementView();
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
			Editor.LockRefresh = true;
			int num = ((ListBox)this).Items.IndexOf((object)SelectedRangeItem);
			if (num < ((ListBox)this).Items.Count - 1)
			{
				StiHeatmapGradientItem selectedRangeItem = SelectedRangeItem;
				((ListBox)this).Items.Remove((object)selectedRangeItem);
				InsertItem(num + 1, selectedRangeItem);
				SelectedRangeItem = selectedRangeItem;
			}
			Editor.LockRefresh = false;
			((ListBox)this).EndUpdate();
			this.ElementsChanged?.Invoke(this, EventArgs.Empty);
			Editor.RefreshElementView();
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
		Point point = ((Control)this).PointToClient(Cursor.Position);
		return ((ListBox)this).IndexFromPoint(point);
	}

	public StiHeatmapGradientItem GetItemAt(Point p)
	{
		int itemIndexAt = GetItemIndexAt(p);
		if (itemIndexAt < 0 || itemIndexAt >= ((ListBox)this).Items.Count)
		{
			return null;
		}
		return ((ListBox)this).Items[itemIndexAt] as StiHeatmapGradientItem;
	}

	public StiHeatmapGradientItem GetItemAtMouse()
	{
		return GetItemAt(((Control)this).PointToClient(Cursor.Position));
	}

	public List<StiHeatmapGradientItem> ToItems()
	{
		return ((IEnumerable)((ListBox)this).Items).Cast<StiHeatmapGradientItem>().ToList();
	}

	public void InvalidateSelectedItem()
	{
		if (SelectedRangeItem != null)
		{
			InvalidateItem(((ListControl)this).SelectedIndex);
		}
	}

	public StiHeatmapGradientItem FirstItem()
	{
		return ((IEnumerable)((ListBox)this).Items).Cast<StiHeatmapGradientItem>().FirstOrDefault();
	}

	public StiHeatmapGradientItem LastItem()
	{
		return ((IEnumerable)((ListBox)this).Items).Cast<StiHeatmapGradientItem>().LastOrDefault();
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

	public void InsertItem(int index, StiHeatmapGradientItem item)
	{
		item.List = this;
		((ListBox)this).Items.Insert(index, (object)item);
	}

	public void AddItems(IEnumerable<StiHeatmapGradientRange> ranges)
	{
		if (ranges == null)
		{
			return;
		}
		foreach (StiHeatmapGradientRange range in ranges)
		{
			AddItem(range);
		}
	}

	public StiHeatmapGradientItem AddItem(StiHeatmapGradientRange range)
	{
		if (range == null)
		{
			return null;
		}
		StiHeatmapGradientItem stiHeatmapGradientItem = new StiHeatmapGradientItem(range);
		AddItem(stiHeatmapGradientItem);
		return stiHeatmapGradientItem;
	}

	public void AddItem(StiHeatmapGradientItem item)
	{
		item.List = this;
		((ListBox)this).Items.Add((object)item);
	}

	public void SelectFirstItem()
	{
		SelectedRangeItem = FirstItem();
	}

	public void SelectLastItem()
	{
		SelectedRangeItem = LastItem();
	}

	private int GetRightFrameOfCloseButton()
	{
		return ((Control)this).Width - (base.IsVerticalScrollBarVisible ? SystemInformation.VerticalScrollBarWidth : 0);
	}

	private int GetLeftFrameOfCloseButton()
	{
		return ((Control)this).Width - StiListBoxPainter.CloseButtonWidth - (base.IsVerticalScrollBarVisible ? SystemInformation.VerticalScrollBarWidth : 0);
	}

	public StiHeatmapGradientListBox()
	{
		((Control)this).AllowDrop = true;
	}
}
