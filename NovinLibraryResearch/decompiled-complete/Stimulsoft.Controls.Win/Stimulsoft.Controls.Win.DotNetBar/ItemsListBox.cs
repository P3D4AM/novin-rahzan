using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
internal class ItemsListBox : ListBox
{
	private ArrayList m_Items;

	internal eDotNetBarStyle Style = eDotNetBarStyle.Office2003;

	internal ItemsListBox()
	{
		((ListBox)this).DrawMode = (DrawMode)1;
		m_Items = null;
		((ListBox)this).IntegralHeight = false;
		((Control)this).BackColor = SystemColors.Control;
	}

	protected override void OnDrawItem(DrawItemEventArgs e)
	{
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Invalid comparison between Unknown and I4
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Invalid comparison between Unknown and I4
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		((ListBox)this).OnDrawItem(e);
		if (m_Items == null || e.Index < 0)
		{
			return;
		}
		BaseItem baseItem = ((ListBox)this).Items[e.Index] as BaseItem;
		e.DrawBackground();
		if (baseItem != null)
		{
			baseItem.LeftInternal = e.Bounds.Left;
			baseItem.TopInternal = e.Bounds.Top;
			baseItem.WidthInternal = e.Bounds.Width;
			if ((e.State & 1) == 1)
			{
				baseItem.InternalMouseEnter();
				baseItem.InternalMouseMove(new MouseEventArgs((MouseButtons)0, 0, baseItem.LeftInternal + 2, baseItem.TopInternal + 2, 0));
			}
			ItemPaintArgs itemPaintArgs = new ItemPaintArgs(null, (Control)(object)this, e.Graphics, new ColorScheme(Style));
			itemPaintArgs.Renderer = GetRenderer();
			baseItem.Paint(itemPaintArgs);
			if ((e.State & 1) == 1)
			{
				baseItem.InternalMouseLeave();
			}
		}
	}

	private BaseRenderer GetRenderer()
	{
		return GlobalManager.Renderer;
	}

	internal void SetItems(ArrayList DotNetBarItems)
	{
		m_Items = DotNetBarItems;
		((ListBox)this).Items.Clear();
		int num = 0;
		foreach (BaseItem item in m_Items)
		{
			item.ContainerControl = this;
			item.SetIsOnCustomizeDialog(b: true);
			item.Visible = true;
			item.WidthInternal = ((Control)this).ClientSize.Width;
			item.RecalcSize();
			if (item.HeightInternal > num)
			{
				num = item.HeightInternal;
			}
		}
		if (num == 0)
		{
			num = 16;
		}
		((ListBox)this).ItemHeight = num;
		foreach (BaseItem item2 in m_Items)
		{
			item2.HeightInternal = num;
			((ListBox)this).Items.Add((object)item2);
		}
	}
}
