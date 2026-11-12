using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Dashboard;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Components.Helpers;
using Stimulsoft.Dashboard.Design.Editors;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Components;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Dictionary.Design;

namespace Stimulsoft.Dashboard.Design.Controls;

public sealed class StiMeterListBox : StiItemListBox
{
	private string labelBeforeEdit;

	private StiMeterItem itemEditing;

	private int hotTrackingIndex = -1;

	private int currentSelectedIndex = -1;

	private Point dragOrigin;

	private bool isDragging;

	private bool isCurrentlySelected;

	private bool isCloseButtonOver;

	private bool isDragOverFromDictionary;

	private bool isMouseOverShowMoreLabel;

	private bool showShowMoreLabel;

	private bool isMouseOverEnterDataManuallyLabel;

	private bool showEnterDataManuallyLabel;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Color ForeColor
	{
		get
		{
			if (!StiUX.IsDark)
			{
				return ((ListBox)this).ForeColor;
			}
			return StiUX.ButtonForeground;
		}
		set
		{
			if (!StiUX.IsDark)
			{
				((ListBox)this).ForeColor = value;
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Color BackColor
	{
		get
		{
			if (StiUX.IsDark)
			{
				if (!((Control)this).Focused)
				{
					return StiUX.ButtonBackground;
				}
				return StiUX.Accent;
			}
			return ((ListBox)this).BackColor;
		}
		set
		{
			if (!StiUX.IsDark)
			{
				((ListBox)this).BackColor = value;
			}
		}
	}

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
	private bool IsMouseOverShowMoreLabel
	{
		get
		{
			return isMouseOverShowMoreLabel;
		}
		set
		{
			if (isMouseOverShowMoreLabel != value)
			{
				isMouseOverShowMoreLabel = value;
				InvalidateShowMoreLabel();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool ShowShowMoreLabel
	{
		get
		{
			return showShowMoreLabel;
		}
		set
		{
			if (showShowMoreLabel != value)
			{
				showShowMoreLabel = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	private bool IsMouseOverEnterDataManuallyLabel
	{
		get
		{
			return isMouseOverEnterDataManuallyLabel;
		}
		set
		{
			if (isMouseOverEnterDataManuallyLabel != value)
			{
				isMouseOverEnterDataManuallyLabel = value;
				InvalidateEnterDataManually();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool ShowEnterDataManuallyLabel
	{
		get
		{
			return showEnterDataManuallyLabel;
		}
		set
		{
			if (showEnterDataManuallyLabel != value)
			{
				showEnterDataManuallyLabel = value;
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

	public event EventHandler ShowMoreLabelClicked;

	public event EventHandler EnterDataManuallyLabelClicked;

	public event EventHandler ElementsConverting;

	public event StiElementChangedEventHandler ElementsChanged;

	public event EventHandler ElementsDragEnter;

	public event EventHandler ElementsDragDrop;

	public event EventHandler ElementsSelectedChanged;

	internal void InvokeShowMoreLabelClicked()
	{
		this.ShowMoreLabelClicked?.Invoke(this, EventArgs.Empty);
	}

	internal void InvokeEnterDataManuallyLabelClicked()
	{
		this.EnterDataManuallyLabelClicked?.Invoke(this, EventArgs.Empty);
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
		if (SelectedMeterItem != null)
		{
			this.ElementsSelectedChanged?.Invoke(this, e);
		}
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
			string label = itemEditing.Meter.Label;
			string newName = newText;
			if (string.IsNullOrWhiteSpace(newText))
			{
				itemEditing.Meter.Label = string.Empty;
				itemEditing.Text = StiLabelHelper.GetLabel(itemEditing.Meter);
				newName = itemEditing.Text;
			}
			else
			{
				itemEditing.Meter.Label = newText;
				itemEditing.Text = newText;
			}
			this.ElementsChanged?.Invoke(this, StiElementChangedArgs.CreateRenamingArgs(label, newName));
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
		OnPaintDataDragDropLabel(e.Graphics);
		OnPaintEnterDataManuallyLabel(e.Graphics);
		OnPaintShowMoreLabel(e.Graphics);
		OnPaintItems(e.Graphics);
	}

	protected override void OnPaintBackground(PaintEventArgs e)
	{
		StiListBoxPainter.PaintBorder(e.Graphics, ((Control)this).ClientRectangle, ((ListBox)this).Items.Count, IsDragOverFromDictionary);
	}

	protected override void OnDrawItem(DrawItemEventArgs e)
	{
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
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
			StiMeterItem stiMeterItem = ((ListBox)this).Items[e.Index] as StiMeterItem;
			string text = stiMeterItem.ToString();
			string function = StiExpressionHelper.GetFunction(stiMeterItem.Meter.Expression);
			if (!string.IsNullOrWhiteSpace(function))
			{
				text = text + " (" + function + ")";
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

	private void OnPaintDataDragDropLabel(Graphics g)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		if (((ListBox)this).Items.Count != 0)
		{
			return;
		}
		string text = Loc.Get("Dashboard", "DragDropDataFromDictionary");
		Rectangle dataDragDropLabelRect = GetDataDragDropLabelRect();
		if (ShowEnterDataManuallyLabel)
		{
			dataDragDropLabelRect.Height /= 2;
			dataDragDropLabelRect.Height -= StiScaleUI.I10;
		}
		StringFormat val = new StringFormat();
		try
		{
			Font gdiFont = StiElementConsts.DragDrop.Font.GetGdiFont();
			try
			{
				SolidBrush val2 = new SolidBrush(StiUX.IsDark ? StiUX.InputForegroundHint : StiElementConsts.DragDrop.Font.Color);
				try
				{
					val.Alignment = (StringAlignment)1;
					val.LineAlignment = (StringAlignment)((!ShowEnterDataManuallyLabel) ? 1 : 2);
					g.DrawString(text, gdiFont, (Brush)(object)val2, (RectangleF)dataDragDropLabelRect, val);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)gdiFont)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void OnPaintShowMoreLabel(Graphics g)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		if (((ListBox)this).Items.Count != 0 || !ShowShowMoreLabel)
		{
			return;
		}
		Rectangle showMoreLabelRect = GetShowMoreLabelRect();
		string text = Loc.Get("Buttons", "ShowMore");
		Color color = (IsMouseOverShowMoreLabel ? Color.Red : Color.FromArgb(255, 43, 86, 154));
		StringFormat val = new StringFormat();
		try
		{
			Font gdiFont = StiElementConsts.DragDrop.Font.GetGdiFont();
			try
			{
				SolidBrush val2 = new SolidBrush(color);
				try
				{
					val.Alignment = (StringAlignment)1;
					val.LineAlignment = (StringAlignment)1;
					g.DrawString(text, gdiFont, (Brush)(object)val2, (RectangleF)showMoreLabelRect, val);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)gdiFont)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void OnPaintEnterDataManuallyLabel(Graphics g)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		if (((ListBox)this).Items.Count != 0 || !ShowEnterDataManuallyLabel)
		{
			return;
		}
		Color color = (IsMouseOverEnterDataManuallyLabel ? Color.Red : Color.FromArgb(255, 43, 86, 154));
		StringFormat val = new StringFormat();
		try
		{
			Font gdiFont = StiElementConsts.DragDrop.Font.GetGdiFont();
			try
			{
				SolidBrush val2 = new SolidBrush(color);
				try
				{
					SolidBrush val3 = new SolidBrush(StiElementConsts.DragDrop.Font.Color);
					try
					{
						Pen val4 = new Pen(Color.FromArgb(255, 43, 86, 154));
						try
						{
							val.Alignment = (StringAlignment)1;
							val.LineAlignment = (StringAlignment)1;
							Rectangle dataDragDropLabelRect = GetDataDragDropLabelRect();
							Rectangle rectangle = new Rectangle(dataDragDropLabelRect.X, dataDragDropLabelRect.Y + dataDragDropLabelRect.Height / 2 - StiScaleUI.I10, dataDragDropLabelRect.Width, StiScaleUI.I20);
							int num = rectangle.X + rectangle.Width / 2;
							int num2 = rectangle.Y + rectangle.Height / 2;
							string text = Loc.GetEnum("StiFilterModeOr").ToLowerInvariant();
							int num3 = TextRenderer.MeasureText(text, gdiFont).Width + StiScale.I4;
							Rectangle rectangle2 = new Rectangle(rectangle.X + (rectangle.Width - num3) / 2, rectangle.Y, num3, rectangle.Height);
							g.DrawString(text, gdiFont, (Brush)(object)val3, (RectangleF)rectangle2, val);
							int num4 = Math.Max(num - StiScaleUI.I(100), rectangle.X);
							int num5 = Math.Min(num + StiScaleUI.I(100), rectangle.Right);
							g.DrawLine(val4, num4, num2, rectangle2.X, num2);
							g.DrawLine(val4, rectangle2.Right, num2, num5, num2);
							RetrieveEnterDataManuallyLabels(out var labelLeft, out var labelRight);
							Rectangle enterDataManuallyRect = GetEnterDataManuallyRect(StiHorAlignment.Left);
							Rectangle enterDataManuallyRect2 = GetEnterDataManuallyRect(StiHorAlignment.Right);
							val.Alignment = (StringAlignment)2;
							g.DrawString(labelLeft, gdiFont, (Brush)(object)val2, (RectangleF)enterDataManuallyRect, val);
							val.Alignment = (StringAlignment)0;
							g.DrawString(labelRight, gdiFont, (Brush)(object)val3, (RectangleF)enterDataManuallyRect2, val);
						}
						finally
						{
							((IDisposable)val4)?.Dispose();
						}
					}
					finally
					{
						((IDisposable)val3)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)gdiFont)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
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
		Image image = GetImage(((ListBox)this).Items[e.Index] as StiMeterItem, isSelected);
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
		if (SelectedMeterItem != null && !IsCloseButtonOver)
		{
			BeginEdit();
		}
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		((Control)this).OnMouseDown(e);
		((Control)this).Focus();
		dragOrigin = e.Location;
		if (GetEnterDataManuallyRect(StiHorAlignment.Left).Contains(e.Location) && ShowEnterDataManuallyLabel)
		{
			InvokeEnterDataManuallyLabelClicked();
			return;
		}
		if (GetShowMoreLabelRect().Contains(e.Location) && ShowShowMoreLabel)
		{
			InvokeShowMoreLabelClicked();
			return;
		}
		StiMeterItem itemAtMouse = GetItemAtMouse();
		if (itemAtMouse == null)
		{
			return;
		}
		if (GetLeftFrameOfCloseButton() < e.X && GetRightFrameOfCloseButton() > e.X)
		{
			((ListBox)this).BeginUpdate();
			itemAtMouse.Text = itemAtMouse.Text;
			StiMeterItem selectedMeterItem = null;
			if (SelectedMeterItem == itemAtMouse)
			{
				if (itemAtMouse.NextItem != null)
				{
					selectedMeterItem = itemAtMouse.NextItem;
				}
				else if (itemAtMouse.PrevItem != null)
				{
					selectedMeterItem = itemAtMouse.PrevItem;
				}
			}
			SelectedMeterItem = selectedMeterItem;
			((ListBox)this).Items.Remove((object)itemAtMouse);
			((ListBox)this).EndUpdate();
			this.ElementsChanged?.Invoke(this, StiElementChangedArgs.CreateDeletingArgs(itemAtMouse?.Meter?.Label));
		}
		else
		{
			SelectedMeterItem = itemAtMouse;
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
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Invalid comparison between Unknown and I4
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).OnMouseMove(e);
		IsCloseButtonOver = GetLeftFrameOfCloseButton() < e.X && e.X < GetRightFrameOfCloseButton();
		IsMouseOverShowMoreLabel = ShowShowMoreLabel && GetShowMoreLabelRect().Contains(e.Location);
		IsMouseOverEnterDataManuallyLabel = ShowEnterDataManuallyLabel && GetEnterDataManuallyRect(StiHorAlignment.Left).Contains(e.Location);
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
		IsMouseOverShowMoreLabel = false;
		IsMouseOverEnterDataManuallyLabel = false;
		RefreshHotTracking();
	}

	protected override void OnMouseHover(EventArgs e)
	{
		((Control)this).OnMouseHover(e);
		RefreshHotTracking();
	}

	protected override void OnDragDrop(DragEventArgs e)
	{
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).OnDragDrop(e);
		isDragging = false;
		IsDragOverFromDictionary = false;
		object data = e.Data.GetData(typeof(StiDictionaryControl));
		if (data != null && StiDragDropColumnHelper.IsDataColumnOrVariableOrTableElementDragged(Designer))
		{
			this.ElementsDragDrop?.Invoke(this, EventArgs.Empty);
		}
		else
		{
			if (!(e.Data.GetData(typeof(StiMeterItem)) is StiMeterItem stiMeterItem))
			{
				return;
			}
			StiMeterItem selectedMeterItem = SelectedMeterItem;
			if (stiMeterItem != selectedMeterItem)
			{
				((ListBox)this).BeginUpdate();
				int index = ((selectedMeterItem != null) ? ((ListBox)this).Items.IndexOf((object)selectedMeterItem) : 0);
				ListBox list = stiMeterItem.List;
				if ((Control.ModifierKeys & 0x20000) == 0)
				{
					list.Items.Remove((object)stiMeterItem);
				}
				StiMeterItem stiMeterItem2 = stiMeterItem.Clone() as StiMeterItem;
				if (AllowMultipleElements)
				{
					InsertItem(index, stiMeterItem2);
				}
				else
				{
					((ListBox)this).Items.Clear();
					AddItem(stiMeterItem2);
				}
				if (list != this)
				{
					this.ElementsConverting?.Invoke(stiMeterItem2, (EventArgs)(object)e);
				}
				SelectedMeterItem = stiMeterItem2;
				((ListBox)this).EndUpdate();
				this.ElementsChanged?.Invoke(this, StiElementChangedArgs.CreateEmptyArgs());
			}
		}
	}

	protected override void OnDragEnter(DragEventArgs e)
	{
		((Control)this).OnDragOver(e);
		object data = e.Data.GetData(typeof(StiDictionaryControl));
		if (data != null && StiDragDropColumnHelper.IsDataColumnOrVariableOrTableElementDragged(Designer))
		{
			e.Effect = (DragDropEffects)2;
			this.ElementsDragEnter?.Invoke(this, EventArgs.Empty);
			IsDragOverFromDictionary = true;
		}
		else if (e.Data.GetData(typeof(StiMeterItem)) is StiMeterItem stiMeterItem && stiMeterItem.List != this)
		{
			e.Effect = (DragDropEffects)2;
			this.ElementsDragEnter?.Invoke(this, EventArgs.Empty);
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
		StiMeterItem itemAtMouse = GetItemAtMouse();
		if (itemAtMouse != null)
		{
			SelectedMeterItem = itemAtMouse;
		}
	}

	private Rectangle GetDataDragDropLabelRect()
	{
		Rectangle result = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		result.Inflate(-StiScaleUI.I10, -StiScaleUI.I10);
		return result;
	}

	private Rectangle GetShowMoreLabelRect()
	{
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		if (((ListBox)this).Items.Count != 0 && !ShowShowMoreLabel)
		{
			return Rectangle.Empty;
		}
		Rectangle rectangle = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		string text = Loc.Get("Buttons", "ShowMore");
		rectangle.Inflate(-StiScale.I10, -StiScale.I10);
		Font gdiFont = StiElementConsts.DragDrop.Font.GetGdiFont();
		try
		{
			SolidBrush val = new SolidBrush(StiElementConsts.DragDrop.Font.Color);
			try
			{
				Size size = TextRenderer.MeasureText(text, gdiFont);
				int num = size.Width + StiScale.I4;
				int height = size.Height;
				return new Rectangle(rectangle.X + (rectangle.Width - num) / 2, rectangle.Bottom - height, num, height);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)gdiFont)?.Dispose();
		}
	}

	private Rectangle GetEnterDataManuallyRect(StiHorAlignment alignment)
	{
		if (((ListBox)this).Items.Count != 0 || !ShowEnterDataManuallyLabel)
		{
			return Rectangle.Empty;
		}
		Font gdiFont = StiElementConsts.DragDrop.Font.GetGdiFont();
		try
		{
			Rectangle dataDragDropLabelRect = GetDataDragDropLabelRect();
			dataDragDropLabelRect = new Rectangle(dataDragDropLabelRect.X, dataDragDropLabelRect.Y + dataDragDropLabelRect.Height / 2 + StiScaleUI.I10, dataDragDropLabelRect.Width, StiScaleUI.I20);
			RetrieveEnterDataManuallyLabels(out var labelLeft, out var labelRight);
			int num = TextRenderer.MeasureText(labelLeft, gdiFont).Width + StiScale.I4;
			int num2 = ((!string.IsNullOrEmpty(labelRight)) ? (TextRenderer.MeasureText(labelRight, gdiFont).Width + StiScale.I10) : 0);
			int num3 = num + num2;
			Rectangle rectangle = new Rectangle(dataDragDropLabelRect.X + (dataDragDropLabelRect.Width - num3) / 2, dataDragDropLabelRect.Y, num, dataDragDropLabelRect.Height);
			Rectangle rectangle2 = new Rectangle(rectangle.Right, dataDragDropLabelRect.Y, num2, dataDragDropLabelRect.Height);
			return (alignment == StiHorAlignment.Left) ? rectangle : rectangle2;
		}
		finally
		{
			((IDisposable)gdiFont)?.Dispose();
		}
	}

	private static void RetrieveEnterDataManuallyLabels(out string labelLeft, out string labelRight)
	{
		labelLeft = Loc.GetReport("EnterDataManually");
		labelRight = "";
		if (labelLeft.StartsWith("Enter"))
		{
			labelRight = labelLeft.Substring("Enter".Length).TrimStart(new char[0]);
			labelLeft = "Enter";
		}
		if (labelLeft.StartsWith("Daten"))
		{
			labelRight = labelLeft.Substring("Daten".Length).TrimStart(new char[0]);
			labelLeft = "Daten";
		}
		if (labelLeft.StartsWith("Введите"))
		{
			labelRight = labelLeft.Substring("Введите".Length).TrimStart(new char[0]);
			labelLeft = "Введите";
		}
	}

	private void ApplyCurrentlySelected()
	{
		if (((ListBox)this).Items.Count != 0)
		{
			IEnumerable<StiMeterListBox> source = (from c in GetAll((Control)(object)Editor, typeof(StiMeterListBox))
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

	private Image GetImage(StiMeterItem item, bool isSelected)
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

	public void BeginEdit()
	{
		if (SelectedMeterItem != null)
		{
			itemEditing = SelectedMeterItem;
			SelectedMeterItem = null;
			Rectangle itemRectangle = ((ListBox)this).GetItemRectangle(((ListBox)this).Items.IndexOf((object)itemEditing));
			itemRectangle.Inflate(-StiScale.I3, -StiScale.I3);
			itemRectangle.X += (int)((double)(AllowImage ? 26 : 6) * StiScale.X);
			itemRectangle.Width -= (int)((double)(AllowImage ? 30 : 10) * StiScale.X) + StiListBoxPainter.CloseButtonWidth;
			OnBeforeLabelEdit(itemEditing.Text);
			StiMeterTextBox stiMeterTextBox = new StiMeterTextBox();
			((Control)stiMeterTextBox).Text = itemEditing.Text;
			((Control)stiMeterTextBox).Left = itemRectangle.X;
			((Control)stiMeterTextBox).Width = itemRectangle.Width;
			((Control)stiMeterTextBox).Height = itemRectangle.Height;
			StiMeterTextBox textBox = stiMeterTextBox;
			((Control)textBox).Top = itemRectangle.Y + (itemRectangle.Height - ((Control)textBox).Height) / 2 + (int)StiScale.Y;
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
		if (itemEditing != null)
		{
			SelectedMeterItem = itemEditing;
		}
	}

	public void DuplicateItem()
	{
		if (SelectedMeterItem != null)
		{
			((ListBox)this).BeginUpdate();
			StiMeterItem stiMeterItem = SelectedMeterItem.Clone() as StiMeterItem;
			stiMeterItem.Meter = stiMeterItem.Meter.Clone() as StiMeter;
			stiMeterItem.Meter.Key = StiKeyHelper.GenerateKey();
			int num = ((ListBox)this).Items.IndexOf((object)SelectedMeterItem);
			if (num != -1)
			{
				InsertItem(num + 1, stiMeterItem);
			}
			else
			{
				AddItem(stiMeterItem);
			}
			SelectedMeterItem = stiMeterItem;
			((ListBox)this).EndUpdate();
			this.ElementsConverting?.Invoke(stiMeterItem, StiElementChangedArgs.CreateEmptyArgs());
			this.ElementsChanged?.Invoke(this, StiElementChangedArgs.CreateEmptyArgs());
			Editor.RefreshElementView();
		}
	}

	public void DeleteItem()
	{
		if (SelectedMeterItem != null)
		{
			((ListBox)this).BeginUpdate();
			string name = SelectedMeterItem?.Meter?.Label;
			StiMeterItem selectedMeterItem = null;
			if (SelectedMeterItem.NextItem != null)
			{
				selectedMeterItem = SelectedMeterItem.NextItem;
			}
			else if (SelectedMeterItem.PrevItem != null)
			{
				selectedMeterItem = SelectedMeterItem.PrevItem;
			}
			((ListBox)this).Items.Remove((object)SelectedMeterItem);
			SelectedMeterItem = selectedMeterItem;
			((ListBox)this).EndUpdate();
			this.ElementsChanged?.Invoke(this, StiElementChangedArgs.CreateDeletingArgs(name));
			Editor.RefreshElementView();
		}
	}

	public void Clear()
	{
		if (SelectedMeterItem != null)
		{
			((ListBox)this).BeginUpdate();
			((ListBox)this).Items.Clear();
			((ListBox)this).EndUpdate();
			this.ElementsChanged?.Invoke(this, StiElementChangedArgs.CreateClearingAllArgs());
			Editor.RefreshElementView();
		}
	}

	public void CopyItemToClipboard()
	{
		if (SelectedMeterItem != null && SelectedMeterItem.Meter != null)
		{
			string text = SelectedMeterItem.Meter.SaveToString();
			Clipboard.SetText(text);
		}
	}

	public void PasteItemFromClipboard()
	{
		try
		{
			((ListBox)this).BeginUpdate();
			string text = Clipboard.GetText();
			StiMeter meter = StiMeterLoader.LoadFromJson(text);
			StiMeterItem stiMeterItem = new StiMeterItem
			{
				Text = StiLabelHelper.GetLabel(meter),
				Meter = meter
			};
			if (!AllowMultipleElements)
			{
				((ListBox)this).Items.Clear();
			}
			int num = ((ListBox)this).Items.IndexOf((object)SelectedMeterItem);
			if (num != -1)
			{
				InsertItem(num, stiMeterItem);
			}
			else
			{
				AddItem(stiMeterItem);
			}
			SelectedMeterItem = stiMeterItem;
			((ListBox)this).EndUpdate();
			this.ElementsConverting?.Invoke(stiMeterItem, EventArgs.Empty);
			this.ElementsChanged?.Invoke(this, StiElementChangedArgs.CreateEmptyArgs());
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
			int num = ((ListBox)this).Items.IndexOf((object)SelectedMeterItem);
			if (num > 0)
			{
				StiMeterItem selectedMeterItem = SelectedMeterItem;
				((ListBox)this).Items.Remove((object)selectedMeterItem);
				InsertItem(num - 1, selectedMeterItem);
				SelectedMeterItem = selectedMeterItem;
			}
			Editor.LockRefresh = false;
			((ListBox)this).EndUpdate();
			this.ElementsChanged?.Invoke(this, StiElementChangedArgs.CreateEmptyArgs());
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
			int num = ((ListBox)this).Items.IndexOf((object)SelectedMeterItem);
			if (num < ((ListBox)this).Items.Count - 1)
			{
				StiMeterItem selectedMeterItem = SelectedMeterItem;
				((ListBox)this).Items.Remove((object)selectedMeterItem);
				InsertItem(num + 1, selectedMeterItem);
				SelectedMeterItem = selectedMeterItem;
			}
			Editor.LockRefresh = false;
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

	private void InvalidateShowMoreLabel()
	{
		((Control)this).Invalidate(GetShowMoreLabelRect());
	}

	private void InvalidateEnterDataManually()
	{
		((Control)this).Invalidate(GetEnterDataManuallyRect(StiHorAlignment.Left));
	}

	public int GetItemIndexAtMouse()
	{
		Point point = ((Control)this).PointToClient(Cursor.Position);
		return ((ListBox)this).IndexFromPoint(point);
	}

	public StiMeterItem GetItemAtMouse()
	{
		int itemIndexAtMouse = GetItemIndexAtMouse();
		if (itemIndexAtMouse < 0 || itemIndexAtMouse >= ((ListBox)this).Items.Count)
		{
			return null;
		}
		return ((ListBox)this).Items[itemIndexAtMouse] as StiMeterItem;
	}

	public List<StiMeterItem> ToItems()
	{
		return ((IEnumerable)((ListBox)this).Items).Cast<StiMeterItem>().ToList();
	}

	public void SetSelectedText(string text)
	{
		if (SelectedMeterItem != null)
		{
			SelectedMeterItem.Text = text;
			InvalidateItem(((ListControl)this).SelectedIndex);
		}
	}

	public void SetSelectedImage(StiMeterItemImage image)
	{
		if (SelectedMeterItem != null)
		{
			SelectedMeterItem.Image = image;
			((Control)this).Invalidate();
		}
	}

	public StiMeterItem FirstItem()
	{
		return ((IEnumerable)((ListBox)this).Items).Cast<StiMeterItem>().FirstOrDefault();
	}

	public StiMeterItem LastItem()
	{
		return ((IEnumerable)((ListBox)this).Items).Cast<StiMeterItem>().LastOrDefault();
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

	public void InsertItem(int index, StiMeterItem item)
	{
		item.List = (ListBox)(object)this;
		((ListBox)this).Items.Insert(index, (object)item);
	}

	public void AddItems(IEnumerable<StiMeter> meters, StiMeterItemImage image = StiMeterItemImage.Measure)
	{
		if (meters == null)
		{
			return;
		}
		foreach (StiMeter meter in meters)
		{
			AddItem(meter, image);
		}
	}

	public StiMeterItem AddItem(StiMeter meter, StiMeterItemImage? image = null)
	{
		if (meter == null)
		{
			return null;
		}
		string label = StiLabelHelper.GetLabel(meter);
		StiMeterItem stiMeterItem = new StiMeterItem(label, meter, image);
		AddItem(stiMeterItem);
		return stiMeterItem;
	}

	public bool ItemsEqualTo(StiMeter meter)
	{
		if (meter != null)
		{
			return ItemsEqualTo(new List<StiMeter> { meter });
		}
		return ItemsEqualTo(new List<StiMeter>());
	}

	public bool ItemsEqualTo(IEnumerable<StiMeter> meters)
	{
		List<StiMeter> list = meters.ToList();
		if (list == null || list.Count != ((ListBox)this).Items.Count)
		{
			return false;
		}
		for (int i = 0; i < ((ListBox)this).Items.Count; i++)
		{
			if ((((ListBox)this).Items[i] as StiMeterItem).Meter != list[i])
			{
				return false;
			}
		}
		return true;
	}

	public void AddItem(StiMeterItem item)
	{
		item.List = (ListBox)(object)this;
		((ListBox)this).Items.Add((object)item);
	}

	public void SelectFirstItem()
	{
		SelectedMeterItem = FirstItem();
	}

	private int GetRightFrameOfCloseButton()
	{
		return ((Control)this).Width - (base.IsVerticalScrollBarVisible ? SystemInformation.VerticalScrollBarWidth : 0);
	}

	private int GetLeftFrameOfCloseButton()
	{
		return ((Control)this).Width - StiListBoxPainter.CloseButtonWidth - (base.IsVerticalScrollBarVisible ? SystemInformation.VerticalScrollBarWidth : 0);
	}

	public StiMeterListBox()
	{
		((Control)this).AllowDrop = true;
	}
}
