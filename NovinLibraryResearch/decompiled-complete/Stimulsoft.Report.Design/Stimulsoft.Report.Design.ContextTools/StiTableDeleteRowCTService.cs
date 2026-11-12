using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Table;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Design.ContextTools;

public class StiTableDeleteRowCTService : StiCTService
{
	private StiTable table;

	public override StiPageType PageType => StiPageType.Page;

	public override string GroupCategory => "object";

	public override int Position => 98;

	public override Type GetInterfaceType()
	{
		return null;
	}

	public static StiTableDeleteRowCTService GetService()
	{
		return StiDesignerOptions.ContextMenu.FirstOrDefault((StiCTService c) => c.GetType() == typeof(StiTableDeleteRowCTService)) as StiTableDeleteRowCTService;
	}

	public override void CreateMenuItem(StiDesignerControl designer)
	{
	}

	public void CreateMenuItem(StiDesignerControl designer, ButtonItem menuItem, StiTable parent)
	{
		base.Designer = designer;
		table = parent;
		ButtonItem buttonItem = new ButtonItem
		{
			Text = StiLocalization.Get("MainMenu", "menuDeleteRow"),
			Image = StiReportDesignImages.Actions.DeleteRow(),
			Enabled = (table.RowCount > 1)
		};
		buttonItem.Click += DeleteRow_Click;
		menuItem.SubItems.Add(buttonItem);
	}

	private void DeleteRow_Click(object sender, EventArgs e)
	{
		base.Designer.UndoRedoSave(StiLocalization.Get("MainMenu", "menuDeleteRow"), onlyCurrentPage: true);
		StiComponentsCollection selectedComponentsOnPage = base.Designer.SelectedComponentsOnPage;
		Stack<int> stack = new Stack<int>();
		for (int i = 0; i < selectedComponentsOnPage.Count; i++)
		{
			int num = table.Components.IndexOf(selectedComponentsOnPage[i]);
			stack.Push(num / table.ColumnCount);
		}
		int num2 = stack.Pop();
		int num3 = num2;
		while (stack.Count > 0)
		{
			int num4 = stack.Pop();
			if (num2 < num4)
			{
				num2 = num4;
			}
			if (num3 > num4)
			{
				num3 = num4;
			}
		}
		if (table.HeaderRowsCount > 0 && num3 <= table.HeaderRowsCount - 1 && num2 <= table.HeaderRowsCount)
		{
			table.HeaderRowsCount -= num2 - num3 + 1;
		}
		else if (table.FooterRowsCount > 0 && num3 >= table.RowCount - table.FooterRowsCount && num2 < table.RowCount)
		{
			table.FooterRowsCount -= num2 - num3 + 1;
		}
		table.DeleteRows(num3, num2);
		((Control)base.Designer).Refresh();
	}
}
