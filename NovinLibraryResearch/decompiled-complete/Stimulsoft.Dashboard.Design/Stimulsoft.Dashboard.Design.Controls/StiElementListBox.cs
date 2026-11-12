using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Components.ComboBox;
using Stimulsoft.Dashboard.Components.DatePicker;
using Stimulsoft.Dashboard.Components.Image;
using Stimulsoft.Dashboard.Components.Text;
using Stimulsoft.Dashboard.Components.TreeViewBox;
using Stimulsoft.Dashboard.Design.Editors;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design.Components;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Surface;

namespace Stimulsoft.Dashboard.Design.Controls;

public sealed class StiElementListBox : StiItemListBox
{
	private int hotTrackingIndex = -1;

	private int currentSelectedIndex = -1;

	private Point dragOrigin;

	private bool isDragging;

	private Hashtable cacheImages = new Hashtable();

	private bool isCurrentlySelected;

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
	public override int ItemHeight
	{
		get
		{
			return StiScale.YYI(144.0);
		}
		set
		{
		}
	}

	protected override void OnMeasureItem(MeasureItemEventArgs e)
	{
		if (e.Index >= 0 && e.Index < ((ListBox)this).Items.Count)
		{
			StiElement stiElement = ((ListBox)this).Items[e.Index] as StiElement;
			if (stiElement is StiComboBoxElement || stiElement is StiTreeViewBoxElement || stiElement is StiDatePickerElement)
			{
				e.ItemHeight = StiScale.YYI(stiElement.Height);
			}
		}
		((ListBox)this).OnMeasureItem(e);
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
		OnPaintItems(e.Graphics);
	}

	protected override void OnDrawItem(DrawItemEventArgs e)
	{
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Expected O, but got Unknown
		if (e.Index == -1 || e.Bounds.IsEmpty)
		{
			return;
		}
		Graphics graphics = e.Graphics;
		bool flag = ((ListControl)this).SelectedIndex == e.Index;
		bool isHotLight = e.Index == hotTrackingIndex;
		Color backgroundColor = StiListBoxPainter.GetBackgroundColor(flag, isHotLight, IsCurrentlySelected);
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
		StiComponent stiComponent = ((ListBox)this).Items[e.Index] as StiComponent;
		if (!(stiComponent is IStiAltProperties { AltClientRectangle: var altClientRectangle } stiAltProperties))
		{
			return;
		}
		double num = ((altClientRectangle.Width == 0.0) ? 100.0 : stiAltProperties.AltClientRectangle.Width);
		double num2 = ((stiAltProperties.AltClientRectangle.Height == 0.0) ? 100.0 : stiAltProperties.AltClientRectangle.Height);
		IStiElementKey stiElementKey = stiComponent as IStiElementKey;
		IStiMargin stiMargin = stiComponent as IStiMargin;
		double zoom = stiComponent.Report.Info.Zoom;
		StiMargin margin = stiMargin.Margin;
		RectangleD clientRectangle = stiComponent.ClientRectangle;
		if (!(stiComponent is StiComboBoxElement) && !(stiComponent is StiTreeViewBoxElement) && !(stiComponent is StiDatePickerElement))
		{
			num2 = ((!(stiComponent is StiTextElement) && !(stiComponent is StiImageElement)) ? (num = Math.Min(num, num2)) : (num = Math.Min(bounds.Width, bounds.Height)));
		}
		try
		{
			lock (cacheImages)
			{
				stiElementKey.Key = StiKeyHelper.GetOrGeneratedKey(stiElementKey.Key);
				object? obj = cacheImages[stiElementKey.Key];
				Bitmap val = (Bitmap)((obj is Bitmap) ? obj : null);
				if (val == null)
				{
					bool useNewHtmlEngine = StiOptions.Engine.UseNewHtmlEngine;
					StiOptions.Engine.UseNewHtmlEngine = true;
					val = new Bitmap((int)(num * StiScale.Factor) + 1, (int)(num2 * StiScale.Factor) + 1);
					Graphics val2 = Graphics.FromImage((Image)(object)val);
					try
					{
						val2.Clear(((Control)this).BackColor);
						stiComponent.Report.Info.Zoom = 1.0;
						stiMargin.Margin = new StiMargin(0.0);
						stiComponent.ClientRectangle = new RectangleD(0.0, 0.0, num, num2);
						stiComponent.Paint(new StiPaintEventArgs(val2, RectangleD.Empty));
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
					cacheImages[stiElementKey.Key] = val;
					StiOptions.Engine.UseNewHtmlEngine = useNewHtmlEngine;
				}
				Rectangle rectangle = new Rectangle(bounds.X + StiScale.I4, bounds.Y + StiScale.I4, bounds.Width - StiScale.I8, bounds.Height - StiScale.I8);
				e.Graphics.DrawImage((Image)(object)val, rectangle);
			}
		}
		finally
		{
			stiMargin.Margin = margin;
			stiComponent.ClientRectangle = clientRectangle;
			stiComponent.Report.Info.Zoom = zoom;
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

	protected override void OnMouseDown(MouseEventArgs e)
	{
		((Control)this).OnMouseDown(e);
		((Control)this).Focus();
		dragOrigin = e.Location;
		((ListControl)this).SelectedIndex = GetItemIndexAt(e.Location);
		isDragging = false;
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
		isDragging = false;
		((Control)this).Invalidate();
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			((Control)this).OnMouseMove(e);
			RefreshHotTracking();
			if (AllowDragItems && !isDragging && (int)e.Button == 1048576 && IsOutsideDragZone(e.Location) && ((ListBox)this).SelectedItem != null)
			{
				isDragging = true;
				((Control)this).DoDragDrop(((ListBox)this).SelectedItem, (DragDropEffects)2);
			}
		}
		catch
		{
		}
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

	protected override void Dispose(bool disposing)
	{
		((Control)this).Dispose(disposing);
		lock (cacheImages)
		{
			DisposeCache();
		}
	}

	internal void DisposeCache()
	{
		if (cacheImages.Count == 0)
		{
			return;
		}
		cacheImages.Values.Cast<Image>().ToList().ForEach(delegate(Image i)
		{
			try
			{
				i.Dispose();
			}
			catch
			{
			}
		});
		cacheImages.Clear();
	}

	private void ApplyCurrentlySelected()
	{
		if (((ListBox)this).Items.Count != 0)
		{
			IEnumerable<StiDataFilterListBox> source = (from c in GetAll((Control)(object)Editor, typeof(StiDataFilterListBox))
				where c != this
				select c).Cast<StiDataFilterListBox>();
			source.ToList().ForEach(delegate(StiDataFilterListBox c)
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

	public int GetItemIndexAt(Point p)
	{
		Point point = ((Control)this).PointToClient(Cursor.Position);
		return ((ListBox)this).IndexFromPoint(point);
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
}
