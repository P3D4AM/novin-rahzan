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
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Design.Controls;

[ToolboxItem(false)]
public sealed class StiMeterListBox : ListBox
{
	private static int DefaultCloseButtonWidth = StiScaleUI.I(32);

	private string labelBeforeEdit;

	private StiMeterItem itemEditing;

	private int hotTrackingIndex = -1;

	private int currentSelectedIndex = -1;

	private Point dragOrigin;

	private bool isDragging;

	private bool isCurrentlySelected;

	private bool isCloseButtonOver;

	private bool isDragOverFromDictionary;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiDesignerControl Designer { get; set; }

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
	public StiMeterItem SelectedMeterItem
	{
		get
		{
			if (((ListControl)this).SelectedIndex < 0 || ((ListControl)this).SelectedIndex >= ((ListBox)this).Items.Count)
			{
				return null;
			}
			return ((ListBox)this).Items[((ListControl)this).SelectedIndex] as StiMeterItem;
		}
		set
		{
			((ListControl)this).SelectedIndex = ((value != null) ? ((ListBox)this).Items.IndexOf((object)value) : (-1));
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override int ItemHeight
	{
		get
		{
			return StiScaleUI.I(32);
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

	public event EventHandler ElementsChanged;

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

	internal void OnBeforeLabelEdit(string text)
	{
		labelBeforeEdit = text;
	}

	internal void OnAfterLabelEdit(string newText)
	{
		if (!(labelBeforeEdit == newText))
		{
			this.ElementsChanged?.Invoke(this, EventArgs.Empty);
		}
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Invalid comparison between Unknown and I4
		((Control)this).OnKeyDown(e);
		if ((int)e.KeyCode == 113 || (int)e.KeyCode == 13)
		{
			e.Handled = true;
			BeginEdit();
		}
	}

	protected override void OnGotFocus(EventArgs e)
	{
		((Control)this).OnGotFocus(e);
		ApplyCurrentlySelected();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		StiListBoxPainter.PaintBorder(e.Graphics, ((Control)this).ClientRectangle, ((ListBox)this).Items.Count, IsDragOverFromDictionary);
		if (((ListBox)this).Items.Count == 0)
		{
			StiListBoxPainter.PaintLabel(e.Graphics, ((Control)this).ClientRectangle, Loc.Get("Dashboard", "DragDropDataFromDictionary"));
		}
		OnPaintItems(e.Graphics);
	}

	protected override void OnPaintBackground(PaintEventArgs pevent)
	{
	}

	protected override void OnDrawItem(DrawItemEventArgs e)
	{
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
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
			StiDrawing.FillRectangle(graphics, backgroundColor, bounds);
			if (flag && ((Control)this).Focused && StiUX.IsDark)
			{
				StiDrawing.FillRectangle(graphics, StiUX.Accent.ChangeAlpha(50), bounds);
			}
			TextFormatFlags val = (TextFormatFlags)32804;
			TextRenderer.DrawText((IDeviceContext)(object)graphics, ((ListBox)this).Items[e.Index].ToString(), ((Control)this).Font, bounds, foregroundColor, val);
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

	protected override void OnMouseDoubleClick(MouseEventArgs e)
	{
		((Control)this).OnMouseDoubleClick(e);
		if (SelectedMeterItem != null && !IsCloseButtonOver)
		{
			BeginEdit();
		}
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		dragOrigin = e.Location;
		StiMeterItem itemAt = GetItemAt(e.Location);
		if (itemAt != null)
		{
			if (((Control)this).Width - DefaultCloseButtonWidth < e.X && ((Control)this).Width > e.X)
			{
				((ListBox)this).Items.Clear();
				this.ElementsChanged?.Invoke(this, (EventArgs)(object)e);
			}
			else
			{
				SelectedMeterItem = itemAt;
			}
		}
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
		IsDragOverFromDictionary = false;
		isDragging = false;
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Invalid comparison between Unknown and I4
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).OnMouseUp(e);
		IsCloseButtonOver = ((Control)this).Width - DefaultCloseButtonWidth < e.X && e.X < ((Control)this).Width;
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
		if (data != null && StiDesignerDragDrop.DragText != null && StiDesignerDragDrop.DragText.StartsWith("{") && StiDesignerDragDrop.DragText.EndsWith("}"))
		{
			string text = StiDesignerDragDrop.DragText.Substring(1, StiDesignerDragDrop.DragText.Length - 2);
			StiDataColumn dataColumnFromColumnName = StiDataColumn.GetDataColumnFromColumnName(Designer.Report.Dictionary, text, allowRelationName: true);
			if (dataColumnFromColumnName != null)
			{
				StiMeterItem item = new StiMeterItem(text);
				((ListBox)this).Items.Clear();
				AddItem(item);
				this.ElementsChanged?.Invoke(this, (EventArgs)(object)e);
				return;
			}
		}
		if (e.Data.GetData(typeof(StiMeterItem)) is StiMeterItem stiMeterItem)
		{
			StiMeterItem selectedMeterItem = SelectedMeterItem;
			if (stiMeterItem != selectedMeterItem)
			{
				((ListBox)this).BeginUpdate();
				int num = ((selectedMeterItem != null) ? ((ListBox)this).Items.IndexOf((object)selectedMeterItem) : (-1));
				StiMeterListBox list = stiMeterItem.List;
				((ListBox)list).Items.Remove((object)stiMeterItem);
				((ListBox)this).Items.Clear();
				AddItem(stiMeterItem);
				SelectedMeterItem = stiMeterItem;
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
			IsDragOverFromDictionary = true;
		}
		else if (e.Data.GetData(typeof(StiMeterItem)) is StiMeterItem stiMeterItem && stiMeterItem.List != this)
		{
			e.Effect = (DragDropEffects)2;
			IsDragOverFromDictionary = true;
		}
		else if (e.Data.GetData(typeof(StiMeterItem)) is StiMeterItem)
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
		StiMeterItem itemAt = GetItemAt(((Control)this).PointToClient(Cursor.Position));
		if (itemAt != null)
		{
			SelectedMeterItem = itemAt;
		}
	}

	private void ApplyCurrentlySelected()
	{
		if (((ListBox)this).Items.Count != 0)
		{
			IEnumerable<StiMeterListBox> source = (from c in GetAll((Control)(object)((Control)this).FindForm(), typeof(StiMeterListBox))
				where c != this
				select c).Cast<StiMeterListBox>();
			source.ToList().ForEach(delegate(StiMeterListBox c)
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

	public void BeginEdit()
	{
		if (SelectedMeterItem != null)
		{
			itemEditing = SelectedMeterItem;
			SelectedMeterItem = null;
			Rectangle itemRectangle = ((ListBox)this).GetItemRectangle(((ListBox)this).Items.IndexOf((object)itemEditing));
			itemRectangle.Inflate(-StiScaleUI.I3, -StiScaleUI.I3);
			itemRectangle.X += StiScaleUI.I(26);
			itemRectangle.Width -= StiScaleUI.I(30) + DefaultCloseButtonWidth;
			OnBeforeLabelEdit(itemEditing.Text);
			StiMeterTextBox stiMeterTextBox = new StiMeterTextBox();
			((Control)stiMeterTextBox).Text = itemEditing.Text;
			((Control)stiMeterTextBox).Left = itemRectangle.X;
			((Control)stiMeterTextBox).Width = itemRectangle.Width;
			((Control)stiMeterTextBox).Height = itemRectangle.Height;
			StiMeterTextBox textBox = stiMeterTextBox;
			((Control)textBox).Top = itemRectangle.Y + (itemRectangle.Height - ((Control)textBox).Height) / 2;
			((Control)textBox).Leave += delegate
			{
				OnAfterLabelEdit(((Control)textBox).Text);
				((Control)this).Controls.Remove((Control)(object)textBox);
				EndEdit();
			};
			((Control)this).Controls.Add((Control)(object)textBox);
			((TextBoxBase)textBox).Select(0, 0);
			((Control)textBox).Focus();
		}
	}

	internal void EndEdit()
	{
		SelectedMeterItem = itemEditing;
	}

	private void InvalidateCloseButton()
	{
		((Control)this).Invalidate(new Rectangle(((Control)this).Width - DefaultCloseButtonWidth - 1, 1, DefaultCloseButtonWidth, ((Control)this).Height - 2));
	}

	public int GetItemIndexAt(Point p)
	{
		Point point = ((Control)this).PointToClient(Cursor.Position);
		return ((ListBox)this).IndexFromPoint(point);
	}

	public StiMeterItem GetItemAt(Point p)
	{
		int itemIndexAt = GetItemIndexAt(p);
		if (itemIndexAt < 0 || itemIndexAt >= ((ListBox)this).Items.Count)
		{
			return null;
		}
		return ((ListBox)this).Items[itemIndexAt] as StiMeterItem;
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

	public void AddItem(StiMeterItem item)
	{
		item.List = this;
		((ListBox)this).Items.Add((object)item);
	}

	public StiMeterListBox()
	{
		((Control)this).SetStyle((ControlStyles)131072, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)2, true);
		((ListBox)this).BorderStyle = (BorderStyle)0;
		((ListBox)this).DrawMode = (DrawMode)1;
		((ListBox)this).IntegralHeight = false;
		((Control)this).AllowDrop = true;
	}
}
