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

public class StiTableInsertRowAboveCTService : StiCTService
{
	private StiTable table;

	public override StiPageType PageType => StiPageType.Page;

	public override string GroupCategory => "object";

	public override int Position => 96;

	public override Type GetInterfaceType()
	{
		return null;
	}

	public static StiTableInsertRowAboveCTService GetService()
	{
		return StiDesignerOptions.ContextMenu.FirstOrDefault((StiCTService c) => c.GetType() == typeof(StiTableInsertRowAboveCTService)) as StiTableInsertRowAboveCTService;
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
			Text = StiLocalization.Get("MainMenu", "menuInsertRowAbove"),
			Image = StiReportDesignImages.Actions.InsertRowAbove()
		};
		buttonItem.Click += InsertRowAbove_Click;
		menuItem.SubItems.Add(buttonItem);
	}

	private void InsertRowAbove_Click(object sender, EventArgs e)
	{
		base.Designer.UndoRedoSave(StiLocalization.Get("MainMenu", "menuInsertRowAbove"), onlyCurrentPage: true);
		StiComponentsCollection selectedComponentsOnPage = base.Designer.SelectedComponentsOnPage;
		Stack<int> stack = new Stack<int>();
		for (int i = 0; i < selectedComponentsOnPage.Count; i++)
		{
			StiTableCell stiTableCell = (StiTableCell)selectedComponentsOnPage[i];
			if (stiTableCell.Join)
			{
				int num = table.Components.IndexOf(stiTableCell.GetJoinComponentByIndex(0));
				stack.Push(num / table.ColumnCount);
			}
			else
			{
				int num2 = table.Components.IndexOf(stiTableCell);
				stack.Push(num2 / table.ColumnCount);
			}
		}
		int num3 = stack.Pop();
		while (stack.Count > 0)
		{
			int num4 = stack.Pop();
			if (num3 > num4)
			{
				num3 = num4;
			}
		}
		table.InsertRowAbove(num3);
		((Control)base.Designer).Refresh();
	}
}
