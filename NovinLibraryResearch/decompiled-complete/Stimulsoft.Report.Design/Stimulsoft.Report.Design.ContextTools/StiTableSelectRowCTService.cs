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

public class StiTableSelectRowCTService : StiCTService
{
	private StiTable table;

	public override StiPageType PageType => StiPageType.Page;

	public override string GroupCategory => "object";

	public override int Position => 99;

	public override Type GetInterfaceType()
	{
		return null;
	}

	public static StiTableSelectRowCTService GetService()
	{
		return StiDesignerOptions.ContextMenu.FirstOrDefault((StiCTService c) => c.GetType() == typeof(StiTableSelectRowCTService)) as StiTableSelectRowCTService;
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
			Text = StiLocalization.Get("MainMenu", "menuSelectRow"),
			Image = StiReportDesignImages.Actions.SelectRow()
		};
		buttonItem.Click += SelectRow_Click;
		menuItem.SubItems.Add(buttonItem);
	}

	private void SelectRow_Click(object sender, EventArgs e)
	{
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
		int num5 = num3 * table.ColumnCount;
		for (int j = num3; j < num2 + 1; j++)
		{
			for (int k = 0; k < table.ColumnCount; k++)
			{
				table.Components[num5].Select();
				num5++;
			}
		}
		((Control)base.Designer).Refresh();
	}
}
