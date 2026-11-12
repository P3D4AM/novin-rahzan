using System;
using System.Collections;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Controls.Menu;
using Stimulsoft.Dashboard.Viewer.Controls.Table;
using Stimulsoft.Dashboard.Viewer.Elements;
using Stimulsoft.Dashboard.Viewer.Elements.Table;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;

namespace Stimulsoft.Dashboard.Viewer.Controls;

public class StiColumnSelectionMenu
{
	protected IStiElement Element { get; }

	protected Control Control { get; }

	protected StiTableElementControl TableElementControl { get; }

	protected StiTableViewControl TableViewControl => TableElementControl.Child as StiTableViewControl;

	protected StiContextMenu Menu { get; }

	public virtual void CreateItems(IStiElement element)
	{
		Menu.IsDarkMode = StiDashboardStyleHelper.IsDarkStyle(element);
		((ToolStrip)Menu).Items.Clear();
		foreach (DataGridViewColumn item in from c in ((IEnumerable)((DataGridView)TableViewControl).Columns).Cast<DataGridViewColumn>().ToList()
			orderby c.DisplayIndex
			select c)
		{
			StiContextMenuItem stiContextMenuItem = new StiContextMenuItem();
			((ToolStripItem)stiContextMenuItem).Text = item.Name;
			((ToolStripMenuItem)stiContextMenuItem).Checked = ((DataGridViewBand)item).Visible;
			((ToolStripItem)stiContextMenuItem).Tag = item;
			StiContextMenuItem stiContextMenuItem2 = stiContextMenuItem;
			((ToolStripItem)stiContextMenuItem2).Click += delegate(object sender, EventArgs args)
			{
				StiContextMenuItem stiContextMenuItem3 = sender as StiContextMenuItem;
				((ToolStripMenuItem)stiContextMenuItem3).Checked = !((ToolStripMenuItem)stiContextMenuItem3).Checked;
				object tag = ((ToolStripItem)stiContextMenuItem3).Tag;
				DataGridViewColumn val = (DataGridViewColumn)((tag is DataGridViewColumn) ? tag : null);
				((DataGridViewBand)val).Visible = !((DataGridViewBand)val).Visible;
			};
			((ToolStrip)Menu).Items.Add((ToolStripItem)(object)stiContextMenuItem2);
		}
	}

	public StiColumnSelectionMenu(StiContextMenu expressionMenu, Control control, IStiElement element)
	{
		Menu = expressionMenu;
		Element = element;
		Control = control;
		TableElementControl = (control as StiHolderElementControl)?.Child as StiTableElementControl;
	}
}
