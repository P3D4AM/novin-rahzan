using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.ListBox;
using Stimulsoft.Dashboard.Drawing.Helpers;

namespace Stimulsoft.Dashboard.Viewer.Controls.ListBox;

[ToolboxItem(false)]
public class StiHorizontalListBox : Panel
{
	private int hotTrackingIndex = -1;

	private int mouseOverIndex = -1;

	private Hashtable cachedSizes = new Hashtable();

	private float scalingFactor = 1f;

	private StiControlColorCollection colors;

	public HScrollBar ScrollBar { get; set; } = new HScrollBar();


	public bool MultiSelection { get; set; } = true;


	public List<StiListBoxItem> Items { get; } = new List<StiListBoxItem>();


	private int MaxItemWidth => StiScaleUI.I(150);

	private int MaxItems => 40;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiControlColorCollection Colors
	{
		get
		{
			return colors;
		}
		set
		{
			if (value != colors)
			{
				colors = value;
				colors.Control = (Control)(object)this;
				((Control)this).Invalidate();
			}
		}
	}

	public object SelectedValue => Items.FirstOrDefault((StiListBoxItem i) => i.Checked)?.Value;

	public bool IsTransparent { get; set; }

	public override Color BackColor
	{
		get
		{
			if (!IsTransparent)
			{
				return ((Control)this).BackColor;
			}
			return Color.Transparent;
		}
		set
		{
			((Control)this).BackColor = (IsTransparent ? Color.Transparent : value);
		}
	}

	public event StiListBoxItemSelectionChangedEventHandler ItemSelectionChanged;

	protected override void OnPaint(PaintEventArgs e)
	{
		Graphics graphics = e.Graphics;
		int itemHeight = GetItemHeight();
		int num = 0;
		int num2 = -GetHorizontalPosition();
		foreach (StiListBoxItem item in Items)
		{
			int itemWidth = GetItemWidth(num, scalingFactor);
			if (num2 + itemWidth > 0)
			{
				Rectangle bounds = new Rectangle(num2, 0, itemWidth, itemHeight);
				if (bounds.IntersectsWith(e.ClipRectangle))
				{
					OnItemPaint(graphics, item, num, bounds);
				}
			}
			num++;
			num2 += itemWidth;
			if (num2 > ((Control)this).Width)
			{
				break;
			}
		}
	}

	private void OnItemPaint(Graphics g, StiListBoxItem item, int itemIndex, Rectangle bounds)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		if (itemIndex == -1)
		{
			return;
		}
		bool @checked = Items[itemIndex].Checked;
		bool flag = itemIndex == hotTrackingIndex;
		Color foreColor = Colors.GetForeColor(@checked, flag);
		Color backColor = Colors.GetBackColor(@checked, flag);
		StringFormat val = new StringFormat();
		try
		{
			Rectangle rectangle = bounds;
			rectangle.Inflate(-StiScaleUI.I2, 0);
			if (@checked || flag)
			{
				StiDrawing.FillRectangle(g, backColor, rectangle);
			}
			Rectangle rectangle2 = rectangle;
			rectangle2.Inflate(-StiScaleUI.I1, -StiScaleUI.I1);
			TextRenderer.DrawText((IDeviceContext)(object)g, item.Label, ((Control)this).Font, rectangle2, foreColor, Color.Empty, (TextFormatFlags)32789);
			if (itemIndex == 0 && item.Label == StiLocHelper.LocAll)
			{
				StiDrawing.DrawLine(g, Colors.SeparatorColor, bounds.Right - 1, bounds.Y + StiScaleUI.I4, bounds.Right - 1, bounds.Bottom - StiScaleUI.I4);
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	protected override void OnSizeChanged(EventArgs e)
	{
		((Control)this).OnSizeChanged(e);
		RefreshUI();
		((Control)this).Invalidate();
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		RefreshHotTracking();
		RefreshMouseOver();
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
		RefreshHotTracking();
		RefreshMouseOver();
	}

	protected override void OnMouseHover(EventArgs e)
	{
		((Control)this).OnMouseHover(e);
		RefreshHotTracking();
		RefreshMouseOver();
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		((Control)this).OnMouseDown(e);
		Point pos = ((Control)this).PointToClient(Cursor.Position);
		int num = IndexFromPoint(pos);
		if (num != -1)
		{
			StiListBoxItem stiListBoxItem = Items[num];
			if (MultiSelection)
			{
				stiListBoxItem.Checked = !stiListBoxItem.Checked;
			}
			else
			{
				stiListBoxItem.Checked = true;
			}
			this.ItemSelectionChanged?.Invoke(this, new StiListBoxItemSelectionChangedEventArgs(stiListBoxItem, num));
			((Control)this).Invalidate();
		}
	}

	private void ScrollBar_ValueChanged(object sender, EventArgs e)
	{
		((Control)this).Invalidate();
	}

	private int GetHorizontalPosition()
	{
		if (!((Control)ScrollBar).Visible)
		{
			return 0;
		}
		return ((ScrollBar)ScrollBar).Value;
	}

	public void ResetAllItemsSelection()
	{
		Items.ForEach(delegate(StiListBoxItem i)
		{
			i.Checked = false;
		});
		if (Items.Count > 0)
		{
			((Control)this).Invalidate();
		}
	}

	public void SelectFirstItemOnly()
	{
		if (Items.Count > 0)
		{
			Items[0].Checked = true;
			((Control)this).Invalidate();
		}
	}

	public void SelectAllItems()
	{
		Items.ForEach(delegate(StiListBoxItem i)
		{
			i.Checked = true;
		});
		((Control)this).Invalidate();
	}

	public void ResetAllItemsSelectionAndSelectFirstOnly()
	{
		if (Items.Count > 0)
		{
			Items.ForEach(delegate(StiListBoxItem i)
			{
				i.Checked = false;
			});
			Items[0].Checked = true;
			((Control)this).Invalidate();
		}
	}

	private int MeasureText(string text)
	{
		if (cachedSizes[text] is int)
		{
			return (int)cachedSizes[text];
		}
		int width = TextRenderer.MeasureText(text, ((Control)this).Font, new Size(MaxItemWidth, 1000000)).Width;
		cachedSizes[text] = width;
		return width;
	}

	public void RefreshUI()
	{
		int num = 0;
		int num2 = 0;
		foreach (StiListBoxItem item in Items)
		{
			int itemWidth = GetItemWidth(num, 1f);
			num++;
			num2 += itemWidth;
			if (num >= MaxItems)
			{
				break;
			}
		}
		if (num >= MaxItems)
		{
			int num3 = num2 / MaxItems;
			num2 = Items.Count * num3;
		}
		if (num2 > ((Control)this).Width)
		{
			scalingFactor = 1f;
			((Control)ScrollBar).Visible = true;
			((ScrollBar)ScrollBar).LargeChange = ((Control)this).Width;
			((ScrollBar)ScrollBar).Maximum = num2;
		}
		else
		{
			scalingFactor = ((Items.Count == 0) ? 1f : ((float)((Control)this).Width / (float)num2));
			((Control)ScrollBar).Visible = false;
		}
	}

	private void RefreshHotTracking()
	{
		Point pos = ((Control)this).PointToClient(Cursor.Position);
		int num = IndexFromPoint(pos);
		if (hotTrackingIndex != num)
		{
			int index = hotTrackingIndex;
			hotTrackingIndex = num;
			InvalidateItem(index);
			InvalidateItem(hotTrackingIndex);
		}
	}

	private void RefreshMouseOver()
	{
		Point pos = ((Control)this).PointToClient(Cursor.Position);
		int num = IndexFromPoint(pos);
		if (mouseOverIndex != num)
		{
			int index = mouseOverIndex;
			mouseOverIndex = num;
			InvalidateItem(index);
			InvalidateItem(mouseOverIndex);
		}
	}

	private int IndexFromPoint(Point pos)
	{
		StiListBoxItem itemAt = GetItemAt(pos.X, pos.Y);
		if (itemAt == null)
		{
			return -1;
		}
		return Items.IndexOf(itemAt);
	}

	private void InvalidateItem(int index)
	{
		if (index != -1 && index < Items.Count)
		{
			((Control)this).Invalidate(GetItemRect(index));
		}
	}

	private StiListBoxItem GetItemAt(int x, int y)
	{
		int num = 0;
		foreach (StiListBoxItem item in Items)
		{
			if (GetItemRect(num).Contains(x, y))
			{
				return item;
			}
			num++;
			if (num >= MaxItems)
			{
				break;
			}
		}
		return null;
	}

	private Rectangle GetItemRect(int itemIndex)
	{
		int itemHeight = GetItemHeight();
		int num = -GetHorizontalPosition();
		int num2 = 0;
		foreach (StiListBoxItem item in Items)
		{
			int itemWidth = GetItemWidth(num2, scalingFactor);
			if (itemIndex == num2)
			{
				return new Rectangle(num, 0, itemWidth, itemHeight);
			}
			num += itemWidth;
			num2++;
			if (num2 >= MaxItems)
			{
				break;
			}
		}
		return Rectangle.Empty;
	}

	private int GetItemWidth(int itemIndex, float factor)
	{
		if (Items.Count >= MaxItems && factor != 1f)
		{
			return ((ScrollBar)ScrollBar).Maximum / Items.Count;
		}
		int val = MeasureText(Items[itemIndex].Label);
		return (int)Math.Round((float)(Math.Min(val, MaxItemWidth) + StiScaleUI.I16) * factor, 0);
	}

	private int GetItemHeight()
	{
		if (!((Control)ScrollBar).Visible)
		{
			return ((Control)this).Height;
		}
		return ((Control)this).Height - ((Control)ScrollBar).Height - StiScaleUI.I4;
	}

	protected override void Dispose(bool disposing)
	{
		((Control)this).Dispose(disposing);
		if (ScrollBar != null)
		{
			((Control)this).Controls.Clear();
			((Component)(object)ScrollBar).Dispose();
			cachedSizes.Clear();
		}
	}

	public StiHorizontalListBox()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		((Control)this).SetStyle((ControlStyles)131072, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		Colors = new StiControlColorCollection((Control)(object)this);
		((Control)ScrollBar).Height = SystemInformation.HorizontalScrollBarHeight;
		((Control)ScrollBar).Dock = (DockStyle)2;
		((Control)ScrollBar).Visible = false;
		((ScrollBar)ScrollBar).ValueChanged += ScrollBar_ValueChanged;
		((Control)this).Controls.Add((Control)(object)ScrollBar);
	}
}
