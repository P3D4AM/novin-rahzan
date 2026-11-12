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

public class StiTableInsertColumnToRightCTService : StiCTService
{
	private StiTable table;

	public override StiPageType PageType => StiPageType.Page;

	public override string GroupCategory => "object";

	public override int Position => 91;

	public override Type GetInterfaceType()
	{
		return null;
	}

	public static StiTableInsertColumnToRightCTService GetService()
	{
		return StiDesignerOptions.ContextMenu.FirstOrDefault((StiCTService c) => c.GetType() == typeof(StiTableInsertColumnToRightCTService)) as StiTableInsertColumnToRightCTService;
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
			Text = StiLocalization.Get("MainMenu", "menuInsertColumnToRight"),
			Image = StiReportDesignImages.Actions.InsertColumnToRight()
		};
		buttonItem.Click += InsertColumnToRight_Click;
		menuItem.SubItems.Add(buttonItem);
	}

	private void InsertColumnToRight_Click(object sender, EventArgs e)
	{
		base.Designer.UndoRedoSave(StiLocalization.Get("MainMenu", "menuInsertColumnToRight"), onlyCurrentPage: true);
		StiComponentsCollection selectedComponentsOnPage = base.Designer.SelectedComponentsOnPage;
		Stack<int> stack = new Stack<int>();
		for (int i = 0; i < selectedComponentsOnPage.Count; i++)
		{
			int num = table.Components.IndexOf(selectedComponentsOnPage[i]);
			int num2 = num / table.ColumnCount;
			stack.Push(num - num2 * table.ColumnCount);
		}
		int num3 = stack.Pop();
		while (stack.Count > 0)
		{
			int num4 = stack.Pop();
			if (num3 < num4)
			{
				num3 = num4;
			}
		}
		table.InsertColumnToRight(num3);
		((Control)base.Designer).Refresh();
	}
}
