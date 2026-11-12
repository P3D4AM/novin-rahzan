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

public class StiTableInsertRowBelowCTService : StiCTService
{
	private StiTable table;

	public override StiPageType PageType => StiPageType.Page;

	public override string GroupCategory => "object";

	public override int Position => 96;

	public override Type GetInterfaceType()
	{
		return null;
	}

	public static StiTableInsertRowBelowCTService GetService()
	{
		return StiDesignerOptions.ContextMenu.FirstOrDefault((StiCTService c) => c.GetType() == typeof(StiTableInsertRowBelowCTService)) as StiTableInsertRowBelowCTService;
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
			Text = StiLocalization.Get("MainMenu", "menuInsertRowBelow"),
			Image = StiReportDesignImages.Actions.InsertRowBelow()
		};
		buttonItem.Click += InsertRowBelow_Click;
		menuItem.SubItems.Add(buttonItem);
	}

	private void InsertRowBelow_Click(object sender, EventArgs e)
	{
		base.Designer.UndoRedoSave(StiLocalization.Get("MainMenu", "menuInsertRowBelow"), onlyCurrentPage: true);
		StiComponentsCollection selectedComponentsOnPage = base.Designer.SelectedComponentsOnPage;
		Stack<int> stack = new Stack<int>();
		for (int i = 0; i < selectedComponentsOnPage.Count; i++)
		{
			StiComponent component = selectedComponentsOnPage[i];
			int num = table.Components.IndexOf(component);
			stack.Push(num / table.ColumnCount);
		}
		int num2 = stack.Pop();
		while (stack.Count > 0)
		{
			int num3 = stack.Pop();
			if (num2 < num3)
			{
				num2 = num3;
			}
		}
		table.InsertRowBelow(num2 + 1);
		((Control)base.Designer).Refresh();
	}
}
