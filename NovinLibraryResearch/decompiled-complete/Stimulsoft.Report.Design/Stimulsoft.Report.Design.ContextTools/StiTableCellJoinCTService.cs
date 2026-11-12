using System;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Table;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Design.ContextTools;

public class StiTableCellJoinCTService : StiCTService
{
	private bool itemChecked;

	private StiTable table;

	public override StiPageType PageType => StiPageType.Page;

	public override string GroupCategory => "object";

	public override int Position => 91;

	public override Type GetInterfaceType()
	{
		return null;
	}

	public static StiTableCellJoinCTService GetService()
	{
		return StiDesignerOptions.ContextMenu.FirstOrDefault((StiCTService c) => c.GetType() == typeof(StiTableCellJoinCTService)) as StiTableCellJoinCTService;
	}

	public override void CreateMenuItem(StiDesignerControl designer)
	{
	}

	public void CreateMenuItem(StiDesignerControl designer, ButtonItem menuItem, StiTable parent, bool join)
	{
		base.Designer = designer;
		table = parent;
		ButtonItem buttonItem = new ButtonItem
		{
			BeginGroup = true,
			Text = StiLocalization.Get("MainMenu", "menuJoinCells"),
			Image = StiReportDesignImages.Actions.JoinCells()
		};
		if (designer.SelectedComponentsOnPage.Count == 1 && !((IStiTableCell)designer.SelectedComponentsOnPage[0]).Join)
		{
			buttonItem.Enabled = false;
		}
		else
		{
			buttonItem.Enabled = true;
			buttonItem.Checked = (itemChecked = join);
			buttonItem.Click += JoinCells_Click;
		}
		menuItem.SubItems.Add(buttonItem);
	}

	private void CreateJoin()
	{
		StiComponentsCollection selectedComponentsOnPage = base.Designer.SelectedComponentsOnPage;
		int[] array = new int[selectedComponentsOnPage.Count];
		int[] array2 = new int[selectedComponentsOnPage.Count];
		for (int i = 0; i < selectedComponentsOnPage.Count; i++)
		{
			int num = table.Components.IndexOf(selectedComponentsOnPage[i]);
			array2[i] = num / table.ColumnCount;
			array[i] = num - array2[i] * table.ColumnCount;
		}
		int num2 = array[0];
		int num3 = array2[0];
		for (int j = 1; j < array.Length; j++)
		{
			if (num2 < array[j])
			{
				num2 = array[j];
			}
			if (num3 < array2[j])
			{
				num3 = array2[j];
			}
		}
		(table.Components[num3 * table.ColumnCount + num2] as IStiTableCell).Join = true;
	}

	private void JoinCells_Click(object sender, EventArgs e)
	{
		base.Designer.UndoRedoSave(StiLocalization.Get("MainMenu", "menuJoinCells"), onlyCurrentPage: true);
		if (!itemChecked)
		{
			CreateJoin();
		}
		else
		{
			StiComponentsCollection selectedTextComponentsOnPage = base.Designer.SelectedTextComponentsOnPage;
			foreach (IStiTableCell item in selectedTextComponentsOnPage)
			{
				if (item.Join)
				{
					item.Join = false;
				}
			}
		}
		((Control)base.Designer).Refresh();
		table = null;
	}
}
