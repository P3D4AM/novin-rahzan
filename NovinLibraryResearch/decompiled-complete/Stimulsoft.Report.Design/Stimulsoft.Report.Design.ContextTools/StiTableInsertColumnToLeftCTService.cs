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

public class StiTableInsertColumnToLeftCTService : StiCTService
{
	private StiTable table;

	public override StiPageType PageType => StiPageType.Page;

	public override string GroupCategory => "object";

	public override int Position => 91;

	public override Type GetInterfaceType()
	{
		return null;
	}

	public static StiTableInsertColumnToLeftCTService GetService()
	{
		return StiDesignerOptions.ContextMenu.FirstOrDefault((StiCTService c) => c.GetType() == typeof(StiTableInsertColumnToLeftCTService)) as StiTableInsertColumnToLeftCTService;
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
			BeginGroup = true,
			Text = StiLocalization.Get("MainMenu", "menuInsertColumnToLeft"),
			Image = StiReportDesignImages.Actions.InsertColumnToLeft()
		};
		buttonItem.Click += InsertColumnToLeft_Click;
		menuItem.SubItems.Add(buttonItem);
	}

	private void InsertColumnToLeft_Click(object sender, EventArgs e)
	{
		base.Designer.UndoRedoSave(StiLocalization.Get("MainMenu", "menuInsertColumnToLeft"), onlyCurrentPage: true);
		StiComponentsCollection selectedComponentsOnPage = base.Designer.SelectedComponentsOnPage;
		Stack<int> stack = new Stack<int>();
		for (int i = 0; i < selectedComponentsOnPage.Count; i++)
		{
			StiComponent stiComponent = selectedComponentsOnPage[i];
			if (((IStiTableCell)stiComponent).Join)
			{
				int num = table.Components.IndexOf(stiComponent);
				int num2 = num / table.ColumnCount;
				stack.Push(num - num2 * table.ColumnCount);
				num = table.Components.IndexOf(((IStiTableCell)stiComponent).GetJoinComponentByIndex(0));
				num2 = num / table.ColumnCount;
				stack.Push(num - num2 * table.ColumnCount);
			}
			else
			{
				int num3 = table.Components.IndexOf(stiComponent);
				int num4 = num3 / table.ColumnCount;
				stack.Push(num3 - num4 * table.ColumnCount);
			}
		}
		int num5 = stack.Pop();
		while (stack.Count > 0)
		{
			int num6 = stack.Pop();
			if (num5 > num6)
			{
				num5 = num6;
			}
		}
		table.InsertColumnToLeft(num5);
		((Control)base.Designer).Refresh();
	}
}
