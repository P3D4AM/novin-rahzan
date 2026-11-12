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
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report.Design.Components;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Dashboard.Design.Controls;

public class StiSimpleMeterListBox : StiItemListBox
{
	private int hotTrackingIndex = -1;

	private bool isCurrentlySelected;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool AllowImage { get; set; } = true;


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

	protected override void OnPaint(PaintEventArgs e)
	{
		OnPaintItems(e.Graphics);
	}

	protected override void OnPaintBackground(PaintEventArgs e)
	{
		StiListBoxPainter.PaintBorder(e.Graphics, ((Control)this).ClientRectangle, ((ListBox)this).Items.Count);
	}

	protected override void OnDrawItem(DrawItemEventArgs e)
	{
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		if (e.Index != -1)
		{
			Graphics graphics = e.Graphics;
			bool flag = ((ListControl)this).SelectedIndex == e.Index;
			bool isHotLight = e.Index == hotTrackingIndex;
			Color backgroundColor = StiListBoxPainter.GetBackgroundColor(flag, isHotLight, IsCurrentlySelected);
			Color foregroundColor = StiListBoxPainter.GetForegroundColor(flag, isHotLight, IsCurrentlySelected);
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
			TextRenderer.DrawText((IDeviceContext)(object)graphics, ((ListBox)this).Items[e.Index].ToString(), ((Control)this).Font, bounds, foregroundColor, val);
			if (AllowImage)
			{
				OnPaintItemImage(e, flag);
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
		Image image = GetImage(((ListBox)this).Items[e.Index] as StiMeterItem, isSelected);
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
		StiMeterItem itemAt = GetItemAt(e.Location);
		if (itemAt != null)
		{
			SelectedMeterItem = itemAt;
			((Control)this).Invalidate();
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
		if (index != -1 && index < ((ListBox)this).Items.Count)
		{
			((Control)this).Invalidate(((ListBox)this).GetItemRectangle(index));
		}
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

	public void AddItem(StiMeterItem item)
	{
		item.List = (ListBox)(object)this;
		((ListBox)this).Items.Add((object)item);
	}

	public void SelectFirstItem()
	{
		SelectedMeterItem = FirstItem();
	}

	public StiMeterItem FirstItem()
	{
		return ((IEnumerable)((ListBox)this).Items).Cast<StiMeterItem>().FirstOrDefault();
	}

	public StiMeterItem LastItem()
	{
		return ((IEnumerable)((ListBox)this).Items).Cast<StiMeterItem>().LastOrDefault();
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

	public StiMeterItem GetItemAt(Point p)
	{
		int itemIndexAt = GetItemIndexAt(p);
		if (itemIndexAt < 0 || itemIndexAt >= ((ListBox)this).Items.Count)
		{
			return null;
		}
		return ((ListBox)this).Items[itemIndexAt] as StiMeterItem;
	}

	public int GetItemIndexAt(Point p)
	{
		Point point = ((Control)this).PointToClient(Cursor.Position);
		return ((ListBox)this).IndexFromPoint(point);
	}
}
