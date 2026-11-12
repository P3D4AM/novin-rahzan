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

public class StiTableSelectColumnCTService : StiCTService
{
	private StiTable table;

	public override StiPageType PageType => StiPageType.Page;

	public override string GroupCategory => "object";

	public override int Position => 95;

	private void SelectColumn_Click(object sender, EventArgs e)
	{
		StiComponentsCollection selectedComponentsOnPage = base.Designer.SelectedComponentsOnPage;
		Stack<int> stack = new Stack<int>();
		for (int i = 0; i < selectedComponentsOnPage.Count; i++)
		{
			int num = table.Components.IndexOf(selectedComponentsOnPage[i]);
			int num2 = num / table.ColumnCount;
			stack.Push(num - num2 * table.ColumnCount);
		}
		int num3 = stack.Pop();
		int num4 = num3;
		while (stack.Count > 0)
		{
			int num5 = stack.Pop();
			if (num3 < num5)
			{
				num3 = num5;
			}
			if (num4 > num5)
			{
				num4 = num5;
			}
		}
		for (int j = 0; j < table.RowCount; j++)
		{
			for (int k = num4; k < num3 + 1; k++)
			{
				table.Components[j * table.ColumnCount + k].Select();
			}
		}
		((Control)base.Designer).Refresh();
	}

	public override Type GetInterfaceType()
	{
		return null;
	}

	public static StiTableSelectColumnCTService GetService()
	{
		return StiDesignerOptions.ContextMenu.FirstOrDefault((StiCTService c) => c.GetType() == typeof(StiTableSelectColumnCTService)) as StiTableSelectColumnCTService;
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
			Text = StiLocalization.Get("MainMenu", "menuSelectColumn"),
			Image = StiReportDesignImages.Actions.SelectColumn()
		};
		buttonItem.Click += SelectColumn_Click;
		menuItem.SubItems.Add(buttonItem);
	}
}
