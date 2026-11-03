using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Controls;

[ToolboxItem(false)]
public class SideNavStrip : ItemControl
{
	private SimpleItemContainer _ᐵ;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public SubItemsCollection Items => _ᐵ.SubItems;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public SideNavItem SelectedItem
	{
		get
		{
			foreach (BaseItem item in Items)
			{
				if (item is SideNavItem && ((SideNavItem)item).Checked)
				{
					return (SideNavItem)item;
				}
			}
			return null;
		}
		set
		{
			if (value != null && value.Panel != null)
			{
				value.Checked = true;
			}
		}
	}

	public SideNavStrip()
	{
		_ᐵ = new SimpleItemContainer();
		_ᐵ.GlobalItem = false;
		_ᐵ.ContainerControl = this;
		_ᐵ.Stretch = true;
		_ᐵ.Displayed = true;
		_ᐵ.Style = eDotNetBarStyle.StyleManagerControlled;
		_ᐵ.LayoutOrientation = eOrientation.Vertical;
		base.ColorScheme.ߕ = eDotNetBarStyle.StyleManagerControlled;
		_ᐵ.SetOwner(this);
		SetBaseItemContainer(_ᐵ);
		base.BackgroundStyle.Class = ElementStyleClassKeys.SideNavStripKey;
		StyleManager.Register((Control)(object)this);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
	{
		_ᐵ.NeedRecalcSize = true;
	}

	protected override void PaintControl(ItemPaintArgs pa)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		base.PaintControl(pa);
		SideNavItem selectedItem = SelectedItem;
		if (selectedItem == null)
		{
			return;
		}
		Color color = _20C1();
		if (color.IsEmpty)
		{
			return;
		}
		Graphics graphics = pa.Graphics;
		Pen val = new Pen(color);
		try
		{
			if (selectedItem.TopInternal > 0)
			{
				graphics.DrawLine(val, ((Control)this).Width - 1, 0, ((Control)this).Width - 1, selectedItem.TopInternal - 1);
			}
			if (selectedItem.Bounds.Bottom < ((Control)this).Height)
			{
				graphics.DrawLine(val, ((Control)this).Width - 1, selectedItem.Bounds.Bottom, ((Control)this).Width - 1, ((Control)this).Height);
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private Color _20C1()
	{
		SideNavColorTable sideNavColorTable = ጧ();
		if (sideNavColorTable.BorderColors != null && sideNavColorTable.BorderColors.Length > 0)
		{
			return sideNavColorTable.BorderColors[0];
		}
		return Color.Empty;
	}

	private SideNavColorTable ጧ()
	{
		return ((Office2007Renderer)GlobalManager.Renderer).ColorTable.SideNav;
	}

	internal void _1030()
	{
		foreach (BaseItem item in Items)
		{
			if (item.Visible && item.Enabled && item is SideNavItem)
			{
				((SideNavItem)item).Checked = true;
				break;
			}
		}
	}

	protected override void OnButtonCheckedChanged(ButtonItem item, EventArgs e)
	{
		if (((Control)this).Parent is SideNav)
		{
			((SideNav)(object)((Control)this).Parent)._20E5();
		}
		base.OnButtonCheckedChanged(item, e);
	}
}
