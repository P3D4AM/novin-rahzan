using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Design.Components;

namespace Stimulsoft.Report.Design.Controls;

[ToolboxItem(false)]
public sealed class StiColumnListBox : ListBox
{
	private class StiNumberingHelper
	{
		private StiColumnListBox listBox;

		private List<int> items = new List<int>();

		public int[] Items => items.ToArray();

		public StiNumberingHelper(StiColumnListBox listBox)
		{
			this.listBox = listBox;
		}

		public int GetSelectedIndex(int itemNumber)
		{
			if (items.Contains(itemNumber))
			{
				return items.IndexOf(itemNumber);
			}
			return -1;
		}

		public void Update()
		{
			SelectedIndexCollection selectedIndices = ((ListBox)listBox).SelectedIndices;
			if (selectedIndices.Count == 0)
			{
				items.Clear();
				return;
			}
			if (selectedIndices.Count == 1)
			{
				items.Clear();
				items.Add(selectedIndices[0]);
				return;
			}
			for (int i = 0; i < items.Count; i++)
			{
				int num = items[i];
				if (!selectedIndices.Contains(num))
				{
					items.RemoveAt(i);
				}
			}
			foreach (int item in selectedIndices)
			{
				if (!items.Contains(item))
				{
					items.Add(item);
				}
			}
		}
	}

	private StiNumberingHelper numberingHelper;

	private int hotTrackingIndex = -1;

	private int currentSelectedIndex = -1;

	private bool isCurrentlySelected;

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
			return StiUX.InputForeground;
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
			if (!StiUX.IsDark)
			{
				return ((ListBox)this).BackColor;
			}
			return StiUX.InputBackground;
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

	public bool DrawSelectedNumber { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override int ItemHeight
	{
		get
		{
			return StiScale.I20;
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
			return (BorderStyle)1;
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

	protected override void OnSelectedIndexChanged(EventArgs e)
	{
		numberingHelper.Update();
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

	protected override void OnPaintBackground(PaintEventArgs pevent)
	{
	}

	protected override void OnDrawItem(DrawItemEventArgs e)
	{
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		if (e.Index == -1)
		{
			return;
		}
		Graphics graphics = e.Graphics;
		bool flag = ((ListBox)this).SelectedIndices.IndexOf(e.Index) != -1;
		bool isHotLight = e.Index == hotTrackingIndex;
		Color backgroundColor = StiListBoxPainter.GetBackgroundColor(flag, isHotLight, IsCurrentlySelected);
		Color foregroundColor = StiListBoxPainter.GetForegroundColor(flag, isHotLight, IsCurrentlySelected);
		Rectangle bounds = e.Bounds;
		bounds.Inflate(-2, -1);
		if (e.Index == 0)
		{
			bounds.Y++;
			bounds.Height--;
		}
		if (e.Index >= ((ListBox)this).Items.Count)
		{
			return;
		}
		StiDrawing.FillRectangle(graphics, backgroundColor, bounds);
		if (flag && ((Control)this).Focused && StiUX.IsDark)
		{
			StiDrawing.FillRectangle(graphics, StiUX.Accent.ChangeAlpha(50), bounds);
		}
		TextFormatFlags val = (TextFormatFlags)32804;
		TextRenderer.DrawText((IDeviceContext)(object)graphics, ((ListBox)this).Items[e.Index].ToString(), ((Control)this).Font, bounds, foregroundColor, val);
		if (DrawSelectedNumber)
		{
			int selectedIndex = numberingHelper.GetSelectedIndex(e.Index);
			if (selectedIndex != -1)
			{
				selectedIndex++;
				val = (TextFormatFlags)32806;
				TextRenderer.DrawText((IDeviceContext)(object)graphics, selectedIndex.ToString(), ((Control)this).Font, bounds, foregroundColor, val);
			}
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
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

	protected override void OnSelectedValueChanged(EventArgs e)
	{
		((ListBox)this).OnSelectedValueChanged(e);
		((Control)this).Invalidate();
	}

	public string[] GetSelectedItems()
	{
		int[] items = numberingHelper.Items;
		string[] array = new string[items.Length];
		for (int i = 0; i < items.Length; i++)
		{
			array[i] = ((items[i] < ((ListBox)this).Items.Count) ? ((string)((ListBox)this).Items[items[i]]) : null);
		}
		return array.Where((string r) => r != null).ToArray();
	}

	private void ApplyCurrentlySelected()
	{
		if (((ListBox)this).Items.Count != 0)
		{
			IEnumerable<StiColumnListBox> source = (from c in GetAll((Control)(object)((Control)this).FindForm(), typeof(StiColumnListBox))
				where c != this
				select c).Cast<StiColumnListBox>();
			source.ToList().ForEach(delegate(StiColumnListBox c)
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

	public StiColumnListBox()
	{
		((Control)this).SetStyle((ControlStyles)131072, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((ListBox)this).BorderStyle = (BorderStyle)1;
		((ListBox)this).DrawMode = (DrawMode)1;
		((ListBox)this).IntegralHeight = false;
		numberingHelper = new StiNumberingHelper(this);
		if (StiUX.IsDark)
		{
			((ListBox)this).BackColor = StiUX.InputBackground;
		}
	}
}
