using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

internal class ᆓ : ListBox
{
	private ArrayList ณ;

	internal eDotNetBarStyle ߕ = eDotNetBarStyle.Office2003;

	internal ᆓ()
	{
		((ListBox)this).DrawMode = (DrawMode)1;
		ณ = null;
		((ListBox)this).IntegralHeight = false;
		((Control)this).BackColor = SystemColors.Control;
	}

	protected override void OnDrawItem(DrawItemEventArgs e)
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Invalid comparison between Unknown and I4
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Invalid comparison between Unknown and I4
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		((ListBox)this).OnDrawItem(e);
		if (ณ == null || e.Index < 0)
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
			ItemPaintArgs itemPaintArgs = new ItemPaintArgs(null, (Control)(object)this, e.Graphics, new ColorScheme(ߕ));
			itemPaintArgs.ໝ = ဣ();
			baseItem.Paint(itemPaintArgs);
			if ((e.State & 1) == 1)
			{
				baseItem.InternalMouseLeave();
			}
		}
	}

	private BaseRenderer ဣ()
	{
		return GlobalManager.Renderer;
	}

	internal void ᆔ(ArrayList ᆕ)
	{
		ณ = ᆕ;
		((ListBox)this).Items.Clear();
		int num = 0;
		foreach (BaseItem item in ณ)
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
		foreach (BaseItem item2 in ณ)
		{
			item2.HeightInternal = num;
			((ListBox)this).Items.Add((object)item2);
		}
	}
}
