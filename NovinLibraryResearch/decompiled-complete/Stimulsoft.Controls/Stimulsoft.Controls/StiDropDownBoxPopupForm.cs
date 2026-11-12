using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

internal class StiDropDownBoxPopupForm : StiPopupForm
{
	private Container components;

	private int hotTrackingItemIndex = -1;

	private StiListBoxUX listBoxItems;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public object SelectedObject { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int HotTrackingItemIndex
	{
		get
		{
			return hotTrackingItemIndex;
		}
		set
		{
			if (value != hotTrackingItemIndex)
			{
				int index = hotTrackingItemIndex;
				hotTrackingItemIndex = value;
				InvalidateItem(index);
				InvalidateItem(hotTrackingItemIndex);
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiListBoxUX ListBoxItems
	{
		get
		{
			return listBoxItems;
		}
		set
		{
			if (listBoxItems != value)
			{
				if (value == null)
				{
					DetachListBox();
				}
				listBoxItems = value;
			}
		}
	}

	private void InitializeComponent()
	{
		((Control)this).SuspendLayout();
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((Control)this).BackColor = SystemColors.Window;
		((Form)this).ClientSize = new Size(250, 240);
		((Control)this).Name = "StiDropDownBoxPopupForm";
		((Control)this).Text = "StiDropDownBoxPopupForm";
		((Control)this).ResumeLayout(false);
	}

	private void ListBoxItems_DoubleClick(object sender, EventArgs e)
	{
		SelectedObject = ((ListBox)ListBoxItems).SelectedItem;
		ClosePopupForm();
	}

	private void ListBoxItems_Click(object sender, EventArgs e)
	{
		SelectedObject = ((ListBox)ListBoxItems).SelectedItem;
		ClosePopupForm();
	}

	private void ListBoxItems_DrawItem(object sender, DrawItemEventArgs e)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Invalid comparison between Unknown and I4
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		Graphics graphics = e.Graphics;
		Rectangle bounds = e.Bounds;
		if ((e.State & 1) > 0)
		{
			bounds.Width--;
		}
		DrawItemState val = e.State;
		string text = ((Control)ListBoxItems).Text;
		if (e.Index != -1)
		{
			text = ((ListBox)ListBoxItems).Items[e.Index].ToString();
		}
		if (HotTrackingItemIndex == e.Index)
		{
			val = (DrawItemState)(val | 1);
		}
		StiControlPaint.DrawItem(graphics, bounds, val, text, null, -1, ((Control)ListBoxItems).Font, ((Control)ListBoxItems).BackColor, ((Control)ListBoxItems).ForeColor, ((Control)ListBoxItems).RightToLeft);
	}

	private void InvalidateItem(int index)
	{
		if (ListBoxItems != null && index != -1 && index <= ((ListBox)ListBoxItems).Items.Count)
		{
			Rectangle itemRectangle = ((ListBox)ListBoxItems).GetItemRectangle(index);
			((Control)ListBoxItems).Invalidate(itemRectangle);
		}
	}

	private void ListBoxItems_MouseMove(object sender, MouseEventArgs e)
	{
		int num = ((ListBox)ListBoxItems).IndexFromPoint(((Control)this).PointToClient(Cursor.Position));
		if (num != HotTrackingItemIndex)
		{
			HotTrackingItemIndex = num;
		}
	}

	private void ListBoxItems_MouseLeave(object sender, EventArgs e)
	{
		if (HotTrackingItemIndex != -1)
		{
			HotTrackingItemIndex = -1;
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		((Form)this).OnPaint(e);
		Graphics graphics = e.Graphics;
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		StiDrawing.FillRectangle(graphics, StiUX.InputBackground, clientRectangle.X, clientRectangle.Y, clientRectangle.Width, clientRectangle.Height);
		StiDrawing.DrawRectangle(graphics, StiUX.InputBorder, clientRectangle.X, clientRectangle.Y, clientRectangle.Width - 1, clientRectangle.Height - 1);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			components?.Dispose();
			DetachListBox();
		}
		base.Dispose(disposing);
	}

	public override void SetLocation(int x, int y, int width, int height)
	{
		base.SetLocation(x, y, width, height);
		((ScrollableControl)this).DockPadding.Left = StiScaleUI.I2;
		((ScrollableControl)this).DockPadding.Top = StiScaleUI.I2;
		((ScrollableControl)this).DockPadding.Right = StiScaleUI.I2;
		((ScrollableControl)this).DockPadding.Bottom = StiScaleUI.I2;
	}

	public void SetItems(ObjectCollection items, object dataSource)
	{
		((ListBox)ListBoxItems).Items.Clear();
		((ListBox)ListBoxItems).Items.AddRange(items);
		((ListControl)ListBoxItems).DataSource = dataSource;
	}

	public Size GetItemsSize(int maxDropDownItems)
	{
		int num = 0;
		maxDropDownItems = Math.Min(maxDropDownItems, ((ListBox)ListBoxItems).Items.Count);
		int num2 = ((ListBox)ListBoxItems).Items.Count * ((ListBox)ListBoxItems).ItemHeight;
		foreach (object item in from object i in (IEnumerable)((ListBox)ListBoxItems).Items
			where i != null
			select i)
		{
			num = Math.Max(num, TextRenderer.MeasureText(item.ToString(), ((Control)ListBoxItems).Font).Width);
		}
		return new Size(num + StiScaleUI.I4, num2 + StiScaleUI.I4);
	}

	public void DetachListBox()
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		if (ListBoxItems != null)
		{
			((Control)ListBoxItems).DoubleClick -= ListBoxItems_DoubleClick;
			((ListBox)ListBoxItems).Click -= ListBoxItems_Click;
			((ListBox)ListBoxItems).DrawItem -= new DrawItemEventHandler(ListBoxItems_DrawItem);
			((Control)ListBoxItems).MouseMove -= new MouseEventHandler(ListBoxItems_MouseMove);
			((Control)ListBoxItems).MouseLeave -= ListBoxItems_MouseLeave;
		}
	}

	public StiDropDownBoxPopupForm(StiListBoxUX listBox)
		: this(null, listBox)
	{
	}

	public StiDropDownBoxPopupForm(Control parentControl, StiListBoxUX listBox)
		: base(parentControl)
	{
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Expected O, but got Unknown
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		((Control)this).SetStyle((ControlStyles)512, false);
		ListBoxItems = listBox;
		((ListBox)ListBoxItems).BorderStyle = (BorderStyle)0;
		((Control)ListBoxItems).Dock = (DockStyle)5;
		((Control)this).Controls.Add((Control)(object)ListBoxItems);
		InitializeComponent();
		((ListBox)ListBoxItems).IntegralHeight = false;
		((ListBox)ListBoxItems).ItemHeight = StiScaleUI.I(21);
		((ListBox)ListBoxItems).DrawMode = (DrawMode)2;
		((Control)ListBoxItems).DoubleClick += ListBoxItems_DoubleClick;
		((ListBox)ListBoxItems).Click += ListBoxItems_Click;
		((ListBox)ListBoxItems).DrawItem += new DrawItemEventHandler(ListBoxItems_DrawItem);
		((Control)ListBoxItems).MouseMove += new MouseEventHandler(ListBoxItems_MouseMove);
		((Control)ListBoxItems).MouseLeave += ListBoxItems_MouseLeave;
	}
}
