using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Design.Components;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Report.Design.DataTransformation.Controls;

[ToolboxItem(false)]
public sealed class StiDataTransformColumnListBox : ListBox
{
	private static int DefaultCloseButtonWidth = StiScale.I(28);

	private string labelBeforeEdit;

	private StiDataTransformationColumn itemEditing;

	private int hotTrackingIndex = -1;

	private int currentSelectedIndex = -1;

	private Point dragOrigin;

	private bool isDragging;

	private Timer updateTimer;

	private bool isCloseButtonOver;

	private bool isDragOverFromDictionary;

	private bool IsScrollBarVisible => ((ListBox)this).Items.Count * ((ListBox)this).ItemHeight > ((Control)this).Height;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiDataTransformationEditorForm Editor { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool AllowDragItems { get; set; } = true;


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
	public StiDataTransformationColumn SelectedDataColumnItem
	{
		get
		{
			if (((ListControl)this).SelectedIndex < 0 || ((ListControl)this).SelectedIndex >= ((ListBox)this).Items.Count)
			{
				return null;
			}
			return ((ListBox)this).Items[((ListControl)this).SelectedIndex] as StiDataTransformationColumn;
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
			return StiScale.I(28);
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
		if (SelectedDataColumnItem != null && !isDragging && !IsDragOverFromDictionary)
		{
			this.ElementsSelectedChanged?.Invoke(this, e);
		}
	}

	internal void OnBeforeLabelEdit(string text)
	{
		labelBeforeEdit = text;
	}

	internal void OnAfterLabelEdit(string newText)
	{
		if (!(labelBeforeEdit == newText))
		{
			itemEditing.Name = StiNameCreation.CreateColumnName(Editor.Transformation, string.IsNullOrWhiteSpace(newText) ? "Column" : newText);
			labelBeforeEdit = itemEditing.Name;
			this.ElementsChanged?.Invoke(this, EventArgs.Empty);
		}
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Invalid comparison between Unknown and I4
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Invalid comparison between Unknown and I4
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Invalid comparison between Unknown and I4
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Invalid comparison between Unknown and I4
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Invalid comparison between Unknown and I4
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Invalid comparison between Unknown and I4
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Invalid comparison between Unknown and I4
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Invalid comparison between Unknown and I4
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Invalid comparison between Unknown and I4
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Invalid comparison between Unknown and I4
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Invalid comparison between Unknown and I4
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Invalid comparison between Unknown and I4
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Invalid comparison between Unknown and I4
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Invalid comparison between Unknown and I4
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Invalid comparison between Unknown and I4
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Invalid comparison between Unknown and I4
		((Control)this).OnKeyDown(e);
		if ((int)e.KeyCode == 113 || (int)e.KeyCode == 13)
		{
			e.Handled = true;
			BeginEdit();
		}
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
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		if (e.Index != -1)
		{
			Graphics graphics = e.Graphics;
			bool flag = ((ListControl)this).SelectedIndex == e.Index;
			bool flag2 = e.Index == hotTrackingIndex;
			Color backgroundColor = StiListBoxPainter.GetBackgroundColor(flag, flag2, ((Control)this).Focused);
			Color foregroundColor = StiListBoxPainter.GetForegroundColor(flag, flag2, ((Control)this).Focused);
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
			bounds.X += StiScale.I(24);
			bounds.Width -= StiScale.I(24);
			TextRenderer.DrawText((IDeviceContext)(object)graphics, ((ListBox)this).Items[e.Index].ToString(), ((Control)this).Font, bounds, foregroundColor, val);
			OnPaintItemImage(e, flag);
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
		Image image = GetImage(((ListBox)this).Items[e.Index] as StiDataTransformationColumn, isSelected);
		if (image != null)
		{
			Rectangle rectangle2 = new Rectangle(rectangle.X + StiScale.I3, rectangle.Y + (rectangle.Height - image.Height) / 2, image.Width, image.Height);
			graphics.DrawImage(image, rectangle2);
			image.Dispose();
		}
	}

	protected override void OnMouseDoubleClick(MouseEventArgs e)
	{
		((Control)this).OnMouseDoubleClick(e);
		if (SelectedDataColumnItem != null && !IsCloseButtonOver)
		{
			BeginEdit();
		}
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		dragOrigin = e.Location;
		StiDataTransformationColumn itemAt = GetItemAt(e.Location);
		if (itemAt == null)
		{
			return;
		}
		Rectangle closeZoneRect = GetCloseZoneRect();
		if (closeZoneRect.Left < e.X && e.X < closeZoneRect.Right)
		{
			((ListBox)this).BeginUpdate();
			StiDataTransformationColumn selectedDataColumnItem = null;
			if (SelectedDataColumnItem == itemAt)
			{
				if (NextItem(itemAt) != null)
				{
					selectedDataColumnItem = NextItem(itemAt);
				}
				else if (PrevItem(itemAt) != null)
				{
					selectedDataColumnItem = PrevItem(itemAt);
				}
			}
			SelectedDataColumnItem = selectedDataColumnItem;
			((ListBox)this).Items.Remove((object)itemAt);
			((ListBox)this).EndUpdate();
			updateTimer.Stop();
			updateTimer.Start();
		}
		else
		{
			SelectedDataColumnItem = itemAt;
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
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Invalid comparison between Unknown and I4
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).OnMouseUp(e);
		Rectangle closeZoneRect = GetCloseZoneRect();
		IsCloseButtonOver = closeZoneRect.Left < e.X && e.X < closeZoneRect.Right;
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
		if (data != null && Editor.IsDragDropAllowed(Editor.Designer))
		{
			this.ElementsDragDrop?.Invoke(this, EventArgs.Empty);
		}
		else
		{
			if (!(e.Data.GetData(typeof(StiDataTransformationColumn)) is StiDataTransformationColumn stiDataTransformationColumn))
			{
				return;
			}
			StiDataTransformationColumn selectedDataColumnItem = SelectedDataColumnItem;
			if (stiDataTransformationColumn != selectedDataColumnItem)
			{
				((ListBox)this).BeginUpdate();
				int num = ((selectedDataColumnItem != null) ? ((ListBox)this).Items.IndexOf((object)selectedDataColumnItem) : (-1));
				((ListBox)this).Items.Remove((object)stiDataTransformationColumn);
				if (num != -1)
				{
					((ListBox)this).Items.Insert(num, (object)stiDataTransformationColumn);
				}
				else
				{
					((ListBox)this).Items.Clear();
					((ListBox)this).Items.Add((object)stiDataTransformationColumn);
				}
				SelectedDataColumnItem = stiDataTransformationColumn;
				((ListBox)this).EndUpdate();
				this.ElementsChanged?.Invoke(this, (EventArgs)(object)e);
			}
		}
	}

	protected override void OnDragEnter(DragEventArgs e)
	{
		((Control)this).OnDragEnter(e);
		object data = e.Data.GetData(typeof(StiDictionaryControl));
		if (data != null && Editor.IsDragDropAllowed(Editor.Designer))
		{
			e.Effect = (DragDropEffects)2;
			this.ElementsDragEnter?.Invoke(this, EventArgs.Empty);
			IsDragOverFromDictionary = true;
		}
		else if (e.Data.GetData(typeof(StiDataTransformationColumn)) is StiDataTransformationColumn)
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
		StiDataTransformationColumn itemAt = GetItemAt(((Control)this).PointToClient(Cursor.Position));
		if (itemAt != null)
		{
			SelectedDataColumnItem = itemAt;
		}
	}

	private void UpdateTimerTick(object sender, EventArgs e)
	{
		updateTimer.Stop();
		this.ElementsChanged?.Invoke(this, EventArgs.Empty);
		Editor.RefreshElementView();
	}

	public void InvokeDragDrop(DragEventArgs e)
	{
		((Control)this).OnDragDrop(e);
	}

	public IEnumerable<Control> GetAll(Control control, Type type)
	{
		IEnumerable<Control> enumerable = ((IEnumerable)control.Controls).Cast<Control>();
		return from c in enumerable.SelectMany((Control ctrl) => GetAll(ctrl, type)).Concat(enumerable)
			where ((object)c).GetType() == type
			select c;
	}

	private Rectangle GetCloseZoneRect()
	{
		int num = ((Control)this).Width - DefaultCloseButtonWidth;
		int num2 = ((Control)this).Width;
		if (IsScrollBarVisible)
		{
			num -= SystemInformation.VerticalScrollBarWidth;
			num2 -= SystemInformation.VerticalScrollBarWidth;
		}
		return new Rectangle(num, 1, num2 - num - 1, ((Control)this).Height - 2);
	}

	private bool IsOutsideDragZone(Point location)
	{
		int width = SystemInformation.DragSize.Width;
		int height = SystemInformation.DragSize.Height;
		return !new Rectangle(dragOrigin.X - width / 2, dragOrigin.Y - height / 2, width, height).Contains(location);
	}

	private Image GetImage(StiDataTransformationColumn item, bool isSelected)
	{
		if (item == null)
		{
			return null;
		}
		if (!isSelected && item.Mode == StiDataTransformationMode.Dimension)
		{
			return StiReportDesignImages.Actions.Dimension(StiImageSize.Normal, allowCache: false);
		}
		if (!isSelected && item.Mode == StiDataTransformationMode.Measure)
		{
			return StiReportDesignImages.Actions.Measure(StiImageSize.Normal, allowCache: false);
		}
		string text = (isSelected ? "White" : "");
		return StiReportDesignImages.GetImage("Meters." + text + GetImageKey(item), StiImageSize.Normal, allowCache: false);
	}

	private string GetImageKey(StiDataTransformationColumn column)
	{
		if (column.Mode != StiDataTransformationMode.Measure)
		{
			return "Dimension";
		}
		return "Measure";
	}

	public void BeginEdit()
	{
		if (SelectedDataColumnItem != null)
		{
			itemEditing = SelectedDataColumnItem;
			SelectedDataColumnItem = null;
			Rectangle itemRectangle = ((ListBox)this).GetItemRectangle(((ListBox)this).Items.IndexOf((object)itemEditing));
			itemRectangle.Width = ((Control)this).Width - GetCloseZoneRect().Width;
			itemRectangle.Inflate(-StiScale.I3, -StiScale.I3);
			itemRectangle.X += StiScale.I(24);
			itemRectangle.Width -= StiScale.I(34);
			OnBeforeLabelEdit(itemEditing.Name);
			StiDataTransformColumnTextBox stiDataTransformColumnTextBox = new StiDataTransformColumnTextBox();
			((Control)stiDataTransformColumnTextBox).Text = itemEditing.Name;
			((Control)stiDataTransformColumnTextBox).Left = itemRectangle.X;
			((Control)stiDataTransformColumnTextBox).Width = itemRectangle.Width;
			((Control)stiDataTransformColumnTextBox).Height = itemRectangle.Height;
			StiDataTransformColumnTextBox textBox = stiDataTransformColumnTextBox;
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
		SelectedDataColumnItem = itemEditing;
	}

	public void DuplicateItem()
	{
		if (SelectedDataColumnItem != null)
		{
			((ListBox)this).BeginUpdate();
			StiDataTransformationColumn stiDataTransformationColumn = SelectedDataColumnItem.Clone() as StiDataTransformationColumn;
			stiDataTransformationColumn.Name = StiNameCreation.CreateColumnName(Editor.Transformation, stiDataTransformationColumn.Name);
			int num = ((ListBox)this).Items.IndexOf((object)SelectedDataColumnItem);
			if (num != -1)
			{
				((ListBox)this).Items.Insert(num + 1, (object)stiDataTransformationColumn);
			}
			else
			{
				((ListBox)this).Items.Add((object)stiDataTransformationColumn);
			}
			SelectedDataColumnItem = stiDataTransformationColumn;
			((ListBox)this).EndUpdate();
			this.ElementsChanged?.Invoke(this, EventArgs.Empty);
			Editor.RefreshElementView();
		}
	}

	public void DeleteItem()
	{
		if (SelectedDataColumnItem != null)
		{
			((ListBox)this).BeginUpdate();
			StiDataTransformationColumn selectedDataColumnItem = null;
			if (NextItem(SelectedDataColumnItem) != null)
			{
				selectedDataColumnItem = NextItem(SelectedDataColumnItem);
			}
			else if (PrevItem(SelectedDataColumnItem) != null)
			{
				selectedDataColumnItem = PrevItem(SelectedDataColumnItem);
			}
			((ListBox)this).Items.Remove((object)SelectedDataColumnItem);
			SelectedDataColumnItem = selectedDataColumnItem;
			((ListBox)this).EndUpdate();
			updateTimer.Stop();
			updateTimer.Start();
		}
	}

	public void Clear()
	{
		if (SelectedDataColumnItem != null)
		{
			((ListBox)this).BeginUpdate();
			((ListBox)this).Items.Clear();
			((ListBox)this).EndUpdate();
			updateTimer.Stop();
			updateTimer.Start();
		}
	}

	public void CopyItemToClipboard()
	{
		if (SelectedDataColumnItem != null)
		{
			string text = SelectedDataColumnItem.SaveToJsonObject(StiJsonSaveMode.Report).ToObject<string>();
			Clipboard.SetText(text);
		}
	}

	public void PasteItemFromClipboard()
	{
		try
		{
			((ListBox)this).BeginUpdate();
			string text = Clipboard.GetText();
			StiDataTransformationColumn stiDataTransformationColumn = new StiDataTransformationColumn();
			stiDataTransformationColumn.LoadFromJsonObject(JObject.Parse(text));
			int num = ((ListBox)this).Items.IndexOf((object)SelectedDataColumnItem);
			if (num != -1)
			{
				((ListBox)this).Items.Insert(num, (object)stiDataTransformationColumn);
			}
			else
			{
				((ListBox)this).Items.Add((object)stiDataTransformationColumn);
			}
			SelectedDataColumnItem = stiDataTransformationColumn;
			((ListBox)this).EndUpdate();
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
			int num = ((ListBox)this).Items.IndexOf((object)SelectedDataColumnItem);
			if (num > 0)
			{
				StiDataTransformationColumn selectedDataColumnItem = SelectedDataColumnItem;
				((ListBox)this).Items.Remove((object)selectedDataColumnItem);
				((ListBox)this).Items.Insert(num - 1, (object)selectedDataColumnItem);
				SelectedDataColumnItem = selectedDataColumnItem;
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
			int num = ((ListBox)this).Items.IndexOf((object)SelectedDataColumnItem);
			if (num < ((ListBox)this).Items.Count - 1)
			{
				StiDataTransformationColumn selectedDataColumnItem = SelectedDataColumnItem;
				((ListBox)this).Items.Remove((object)selectedDataColumnItem);
				((ListBox)this).Items.Insert(num + 1, (object)selectedDataColumnItem);
				SelectedDataColumnItem = selectedDataColumnItem;
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
		((Control)this).Invalidate(GetCloseZoneRect());
	}

	public int GetItemIndexAt(Point p)
	{
		Point point = ((Control)this).PointToClient(Cursor.Position);
		return ((ListBox)this).IndexFromPoint(point);
	}

	public StiDataTransformationColumn GetItemAt(Point p)
	{
		int itemIndexAt = GetItemIndexAt(p);
		if (itemIndexAt < 0 || itemIndexAt >= ((ListBox)this).Items.Count)
		{
			return null;
		}
		return ((ListBox)this).Items[itemIndexAt] as StiDataTransformationColumn;
	}

	public StiDataTransformationColumn GetItemAtMouse()
	{
		return GetItemAt(((Control)this).PointToClient(Cursor.Position));
	}

	public List<StiDataTransformationColumn> ToItems()
	{
		return ((IEnumerable)((ListBox)this).Items).Cast<StiDataTransformationColumn>().ToList();
	}

	public void RefreshSelectedImage()
	{
		if (SelectedDataColumnItem != null)
		{
			InvalidateItem(((ListBox)this).Items.IndexOf(((ListBox)this).SelectedItem));
		}
	}

	public StiDataTransformationColumn FirstItem()
	{
		return ((IEnumerable)((ListBox)this).Items).Cast<StiDataTransformationColumn>().FirstOrDefault();
	}

	public StiDataTransformationColumn LastItem()
	{
		return ((IEnumerable)((ListBox)this).Items).Cast<StiDataTransformationColumn>().LastOrDefault();
	}

	public StiDataTransformationColumn PrevItem(StiDataTransformationColumn item)
	{
		int num = ((ListBox)this).Items.IndexOf((object)item);
		if (num < 1)
		{
			return null;
		}
		return ((ListBox)this).Items[num - 1] as StiDataTransformationColumn;
	}

	public StiDataTransformationColumn NextItem(StiDataTransformationColumn item)
	{
		int num = ((ListBox)this).Items.IndexOf((object)item);
		if (num < 0 || num >= ((ListBox)this).Items.Count - 1)
		{
			return null;
		}
		return ((ListBox)this).Items[num + 1] as StiDataTransformationColumn;
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

	public void SelectFirstItem()
	{
		SelectedDataColumnItem = FirstItem();
	}

	public StiDataTransformColumnListBox()
	{
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		((Control)this).SetStyle((ControlStyles)131072, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)2, true);
		((ListBox)this).BorderStyle = (BorderStyle)0;
		((ListBox)this).DrawMode = (DrawMode)1;
		((ListBox)this).IntegralHeight = false;
		((Control)this).AllowDrop = true;
		updateTimer = new Timer();
		updateTimer.Interval = 300;
		updateTimer.Tick += UpdateTimerTick;
	}
}
