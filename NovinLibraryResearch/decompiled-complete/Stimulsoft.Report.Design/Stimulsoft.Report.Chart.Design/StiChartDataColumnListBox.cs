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
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Components;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Chart.Design;

public sealed class StiChartDataColumnListBox : StiItemListBox
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
	public StiChartEditorFormV2 Editor { get; set; }

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
	public StiChartItem SelectedChartItem
	{
		get
		{
			if (((ListControl)this).SelectedIndex < 0 || ((ListControl)this).SelectedIndex >= ((ListBox)this).Items.Count)
			{
				return null;
			}
			return ((ListBox)this).Items[((ListControl)this).SelectedIndex] as StiChartItem;
		}
		set
		{
			((ListControl)this).SelectedIndex = ((value != null) ? ((ListBox)this).Items.IndexOf((object)value) : (-1));
		}
	}

	public event EventHandler ElementsConverting;

	public event StiElementChangedEventHandler ElementsChanged;

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
		if (SelectedChartItem != null)
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
			StiListBoxPainter.PaintLabel(e.Graphics, ((Control)this).ClientRectangle, Loc.Get("Dashboard", "DragDropDataFromDictionary"));
		}
		OnPaintItems(e.Graphics);
	}

	protected override void OnPaintBackground(PaintEventArgs e)
	{
		StiListBoxPainter.PaintBorder(e.Graphics, ((Control)this).ClientRectangle, ((ListBox)this).Items.Count, IsDragOverFromDictionary);
	}

	protected override void OnDrawItem(DrawItemEventArgs e)
	{
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		if (e.Index != -1)
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
			TextFormatFlags val = (TextFormatFlags)32804;
			int num = (int)((double)(AllowImage ? 24 : 4) * StiScale.X);
			bounds.X += num;
			bounds.Width -= num;
			StiChartItem stiChartItem = ((ListBox)this).Items[e.Index] as StiChartItem;
			string text = stiChartItem.ToString();
			if (string.IsNullOrWhiteSpace(text))
			{
				text = "[" + Loc.Get("Dashboard", "DataNotDefined") + "]";
			}
			TextRenderer.DrawText((IDeviceContext)(object)graphics, text, ((Control)this).Font, bounds, foregroundColor, val);
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
		Image image = GetImage(((ListBox)this).Items[e.Index] as StiChartItem, isSelected);
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
		StiChartItem itemAt = GetItemAt(e.Location);
		if (itemAt == null)
		{
			return;
		}
		if (GetLeftFrameOfCloseButton() < e.X && GetRightFrameOfCloseButton() > e.X)
		{
			((ListBox)this).BeginUpdate();
			itemAt.Text = itemAt.Text;
			StiChartItem selectedChartItem = null;
			if (SelectedChartItem == itemAt)
			{
				if (itemAt.NextItem != null)
				{
					selectedChartItem = itemAt.NextItem;
				}
				else if (itemAt.PrevItem != null)
				{
					selectedChartItem = itemAt.PrevItem;
				}
			}
			SelectedChartItem = selectedChartItem;
			((ListBox)this).Items.Remove((object)itemAt);
			((ListBox)this).EndUpdate();
			this.ElementsChanged?.Invoke(this, StiElementChangedArgs.CreateDeletingArgs(itemAt?.Meter?.DataColumn));
		}
		else
		{
			SelectedChartItem = itemAt;
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
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).OnDragDrop(e);
		if (Designer == null)
		{
			return;
		}
		isDragging = false;
		IsDragOverFromDictionary = false;
		object data = e.Data.GetData(typeof(StiDictionaryControl));
		if (data != null && StiDragDropColumnHelper.IsDataColumnOrVariableOrTableElementDragged(Designer))
		{
			this.ElementsDragDrop?.Invoke(this, EventArgs.Empty);
		}
		else
		{
			if (!(e.Data.GetData(typeof(StiChartItem)) is StiChartItem stiChartItem))
			{
				return;
			}
			StiChartItem selectedChartItem = SelectedChartItem;
			if (stiChartItem != selectedChartItem)
			{
				((ListBox)this).BeginUpdate();
				int num = ((selectedChartItem != null) ? ((ListBox)this).Items.IndexOf((object)selectedChartItem) : (-1));
				ListBox list = stiChartItem.List;
				if ((Control.ModifierKeys & 0x20000) == 0)
				{
					list.Items.Remove((object)stiChartItem);
				}
				StiChartItem stiChartItem2 = stiChartItem.Clone() as StiChartItem;
				if (AllowMultipleElements && num != -1)
				{
					InsertItem(num, stiChartItem2);
				}
				else
				{
					((ListBox)this).Items.Clear();
					AddItem(stiChartItem2);
				}
				if (list != this)
				{
					this.ElementsConverting?.Invoke(stiChartItem2, (EventArgs)(object)e);
				}
				SelectedChartItem = stiChartItem2;
				((ListBox)this).EndUpdate();
				this.ElementsChanged?.Invoke(this, StiElementChangedArgs.CreateEmptyArgs());
			}
		}
	}

	protected override void OnDragEnter(DragEventArgs e)
	{
		((Control)this).OnDragOver(e);
		if (Designer != null)
		{
			object data = e.Data.GetData(typeof(StiDictionaryControl));
			if (data != null && StiDragDropColumnHelper.IsDataColumnOrVariableOrTableElementDragged(Designer))
			{
				e.Effect = (DragDropEffects)2;
				this.ElementsDragEnter?.Invoke(this, EventArgs.Empty);
				IsDragOverFromDictionary = true;
			}
			else if (e.Data.GetData(typeof(StiChartItem)) is StiChartItem stiChartItem && stiChartItem.List != this)
			{
				e.Effect = (DragDropEffects)2;
				this.ElementsDragEnter?.Invoke(this, EventArgs.Empty);
				IsDragOverFromDictionary = true;
			}
			else if (e.Data.GetData(typeof(StiChartItem)) is StiChartItem)
			{
				e.Effect = (DragDropEffects)2;
			}
			else
			{
				e.Effect = (DragDropEffects)0;
			}
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
		StiChartItem itemAt = GetItemAt(((Control)this).PointToClient(Cursor.Position));
		if (itemAt != null)
		{
			SelectedChartItem = itemAt;
		}
	}

	private void ApplyCurrentlySelected()
	{
		if (((ListBox)this).Items.Count != 0)
		{
			IEnumerable<StiChartDataColumnListBox> source = (from c in GetAll((Control)(object)Editor, typeof(StiChartDataColumnListBox))
				where c != this
				select c).Cast<StiChartDataColumnListBox>();
			source.ToList().ForEach(delegate(StiChartDataColumnListBox c)
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

	private bool IsOutsideDragZone(Point location)
	{
		int width = SystemInformation.DragSize.Width;
		int height = SystemInformation.DragSize.Height;
		return !new Rectangle(dragOrigin.X - width / 2, dragOrigin.Y - height / 2, width, height).Contains(location);
	}

	private Image GetImage(StiChartItem item, bool isSelected)
	{
		if (item == null || !item.Image.HasValue)
		{
			return null;
		}
		string text = (isSelected ? "White" : "");
		if (item.Image.ToString().StartsWith("Series"))
		{
			return StiReportDesignImages.GetImage("Meters.Charts." + text + item.Image.ToString().Substring(6), StiImageSize.Normal, allowCache: false);
		}
		return StiReportDesignImages.GetImage($"Meters.{text}{item.Image}", StiImageSize.Normal, allowCache: false);
	}

	public void DeleteItem()
	{
		if (SelectedChartItem != null)
		{
			((ListBox)this).BeginUpdate();
			string name = SelectedChartItem?.Meter?.DataColumn;
			StiChartItem selectedChartItem = null;
			if (SelectedChartItem.NextItem != null)
			{
				selectedChartItem = SelectedChartItem.NextItem;
			}
			else if (SelectedChartItem.PrevItem != null)
			{
				selectedChartItem = SelectedChartItem.PrevItem;
			}
			((ListBox)this).Items.Remove((object)SelectedChartItem);
			SelectedChartItem = selectedChartItem;
			((ListBox)this).EndUpdate();
			this.ElementsChanged?.Invoke(this, StiElementChangedArgs.CreateDeletingArgs(name));
			Editor?.RefreshElementView();
		}
	}

	public void Clear()
	{
		if (SelectedChartItem != null)
		{
			((ListBox)this).BeginUpdate();
			((ListBox)this).Items.Clear();
			((ListBox)this).EndUpdate();
			this.ElementsChanged?.Invoke(this, StiElementChangedArgs.CreateClearingAllArgs());
			Editor?.RefreshElementView();
		}
	}

	public void MoveItemUp()
	{
		try
		{
			((ListBox)this).BeginUpdate();
			if (Editor != null)
			{
				Editor.LockRefresh = true;
			}
			int num = ((ListBox)this).Items.IndexOf((object)SelectedChartItem);
			if (num > 0)
			{
				StiChartItem selectedChartItem = SelectedChartItem;
				((ListBox)this).Items.Remove((object)selectedChartItem);
				InsertItem(num - 1, selectedChartItem);
				SelectedChartItem = selectedChartItem;
			}
			if (Editor != null)
			{
				Editor.LockRefresh = false;
			}
			((ListBox)this).EndUpdate();
			this.ElementsChanged?.Invoke(this, StiElementChangedArgs.CreateEmptyArgs());
			Editor?.RefreshElementView();
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
			if (Editor != null)
			{
				Editor.LockRefresh = true;
			}
			int num = ((ListBox)this).Items.IndexOf((object)SelectedChartItem);
			if (num < ((ListBox)this).Items.Count - 1)
			{
				StiChartItem selectedChartItem = SelectedChartItem;
				((ListBox)this).Items.Remove((object)selectedChartItem);
				InsertItem(num + 1, selectedChartItem);
				SelectedChartItem = selectedChartItem;
			}
			if (Editor != null)
			{
				Editor.LockRefresh = false;
			}
			((ListBox)this).EndUpdate();
			this.ElementsChanged?.Invoke(this, StiElementChangedArgs.CreateEmptyArgs());
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

	public StiChartItem GetItemAt(Point p)
	{
		int itemIndexAt = GetItemIndexAt(p);
		if (itemIndexAt < 0 || itemIndexAt >= ((ListBox)this).Items.Count)
		{
			return null;
		}
		return ((ListBox)this).Items[itemIndexAt] as StiChartItem;
	}

	public StiChartItem GetItemAtMouse()
	{
		return GetItemAt(((Control)this).PointToClient(Cursor.Position));
	}

	public List<StiChartItem> ToItems()
	{
		return ((IEnumerable)((ListBox)this).Items).Cast<StiChartItem>().ToList();
	}

	public void SetSelectedItem(string text, StiDataColumn dataColumn)
	{
		if (SelectedChartItem != null)
		{
			SelectedChartItem.Text = text;
			SelectedChartItem.Meter.DataColumn = dataColumn.DataSource.Name + "." + dataColumn.Name;
			InvalidateItem(((ListControl)this).SelectedIndex);
		}
	}

	public void SetSelectedImage(StiMeterItemImage image)
	{
		if (SelectedChartItem != null)
		{
			SelectedChartItem.Image = image;
			((Control)this).Invalidate();
		}
	}

	public StiChartItem FirstItem()
	{
		return ((IEnumerable)((ListBox)this).Items).Cast<StiChartItem>().FirstOrDefault();
	}

	public StiChartItem LastItem()
	{
		return ((IEnumerable)((ListBox)this).Items).Cast<StiChartItem>().LastOrDefault();
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

	public void InsertItem(int index, StiChartItem item)
	{
		item.List = (ListBox)(object)this;
		((ListBox)this).Items.Insert(index, (object)item);
	}

	public void AddItems(IEnumerable<StiDataColumnMeter> meters, StiMeterItemImage image = StiMeterItemImage.Measure)
	{
		if (meters == null)
		{
			return;
		}
		foreach (StiDataColumnMeter meter in meters)
		{
			AddItem(meter, image);
		}
	}

	public StiChartItem AddItem(StiDataColumnMeter meter, StiMeterItemImage? image = null)
	{
		if (meter == null)
		{
			return null;
		}
		StiChartItem stiChartItem = new StiChartItem(meter.DataColumn, meter, image);
		AddItem(stiChartItem);
		return stiChartItem;
	}

	public bool ItemsEqualTo(StiDataColumnMeter meter)
	{
		return ItemsEqualTo(new List<StiDataColumnMeter> { meter });
	}

	public bool ItemsEqualTo(IEnumerable<StiDataColumnMeter> meters)
	{
		List<StiDataColumnMeter> list = meters.ToList();
		if (list == null || list.Count != ((ListBox)this).Items.Count)
		{
			return false;
		}
		for (int i = 0; i < ((ListBox)this).Items.Count; i++)
		{
			if ((((ListBox)this).Items[i] as StiChartItem).Meter != list[i])
			{
				return false;
			}
		}
		return true;
	}

	public void AddItem(StiChartItem item)
	{
		item.List = (ListBox)(object)this;
		((ListBox)this).Items.Add((object)item);
	}

	public void SelectFirstItem()
	{
		SelectedChartItem = FirstItem();
	}

	private int GetRightFrameOfCloseButton()
	{
		return ((Control)this).Width - (base.IsVerticalScrollBarVisible ? SystemInformation.VerticalScrollBarWidth : 0);
	}

	private int GetLeftFrameOfCloseButton()
	{
		return ((Control)this).Width - StiListBoxPainter.CloseButtonWidth - (base.IsVerticalScrollBarVisible ? SystemInformation.VerticalScrollBarWidth : 0);
	}

	public StiChartDataColumnListBox()
	{
		((Control)this).AllowDrop = true;
	}
}
