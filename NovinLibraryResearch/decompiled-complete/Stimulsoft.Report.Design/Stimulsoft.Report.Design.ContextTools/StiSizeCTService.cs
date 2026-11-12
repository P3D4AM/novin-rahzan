using System;
using System.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Design.ContextTools;

public class StiSizeCTService : StiCTService
{
	public override StiPageType PageType => StiPageType.All;

	public override string GroupCategory => "Layout";

	public override int Position => 32;

	private void MakeSameSize_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeMakeSameSize(base.Designer);
	}

	private void MakeSameWidth_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeMakeSameWidth(base.Designer);
	}

	private void MakeSameHeight_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeMakeSameHeight(base.Designer);
	}

	private void MakeHorizontalSpacingEqual_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeMakeHorizontalSpacingEqual(base.Designer);
	}

	private void MakeVerticalSpacingEqual_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeMakeVerticalSpacingEqual(base.Designer);
	}

	public override Type GetInterfaceType()
	{
		return null;
	}

	public static StiSizeCTService GetService()
	{
		return StiDesignerOptions.ContextMenu.FirstOrDefault((StiCTService c) => c.GetType() == typeof(StiSizeCTService)) as StiSizeCTService;
	}

	public override void CreateMenuItem(StiDesignerControl designer)
	{
		base.Designer = designer;
		if (designer.SelectedComponentsOnPage.Count <= 1)
		{
			return;
		}
		StiComponentsCollection stiComponentsCollection = new StiComponentsCollection();
		foreach (StiComponent item in designer.SelectedComponentsOnPage)
		{
			if (StiRestrictionsHelper.IsAllowChange(item))
			{
				stiComponentsCollection.Add(item);
			}
		}
		stiComponentsCollection.SortBySelectionTick();
		ButtonItem buttonItem = new ButtonItem
		{
			Text = StiLocalization.Get("Toolbars", "Size")
		};
		designer.cmDesigner.SubItems.Add(buttonItem);
		ButtonItem buttonItem2 = new ButtonItem
		{
			Text = string.Format(StiLocalization.Get("Toolbars", "MakeSameSize"), stiComponentsCollection[0].Name),
			Image = StiReportDesignImages.Layout.MakeSameSizeAs()
		};
		buttonItem2.Click += MakeSameSize_Click;
		buttonItem.SubItems.Add(buttonItem2);
		ButtonItem buttonItem3 = new ButtonItem
		{
			Text = string.Format(StiLocalization.Get("Toolbars", "MakeSameWidth"), stiComponentsCollection[0].Name),
			Image = StiReportDesignImages.Layout.MakeSameWidthAs()
		};
		buttonItem3.Click += MakeSameWidth_Click;
		buttonItem.SubItems.Add(buttonItem3);
		ButtonItem buttonItem4 = new ButtonItem
		{
			Text = string.Format(StiLocalization.Get("Toolbars", "MakeSameHeight"), stiComponentsCollection[0].Name),
			Image = StiReportDesignImages.Layout.MakeSameHeightAs()
		};
		buttonItem4.Click += MakeSameHeight_Click;
		buttonItem.SubItems.Add(buttonItem4);
		ButtonItem buttonItem5 = new ButtonItem
		{
			Text = StiLocalization.Get("Toolbars", "MakeHorizontalSpacingEqual"),
			Image = StiReportDesignImages.Layout.MakeHorizontalSpacingEqual(),
			BeginGroup = true
		};
		buttonItem5.Click += MakeHorizontalSpacingEqual_Click;
		buttonItem.SubItems.Add(buttonItem5);
		ButtonItem buttonItem6 = new ButtonItem
		{
			Text = StiLocalization.Get("Toolbars", "MakeVerticalSpacingEqual"),
			Image = StiReportDesignImages.Layout.MakeVerticalSpacingEqual()
		};
		buttonItem6.Click += MakeVerticalSpacingEqual_Click;
		buttonItem.SubItems.Add(buttonItem6);
	}
}
