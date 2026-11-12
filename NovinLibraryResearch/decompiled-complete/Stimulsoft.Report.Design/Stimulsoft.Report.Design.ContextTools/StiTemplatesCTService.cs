using System;
using System.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Design.ContextTools;

public class StiTemplatesCTService : StiCTService
{
	public override StiPageType PageType => StiPageType.Page;

	public override string GroupCategory => "Layout";

	public override int Position => 33;

	private void SaveTemplate_Click(object sender, EventArgs e)
	{
		StiActions.Templates.InvokeSave(base.Designer);
	}

	private void OpenTemplate_Click(object sender, EventArgs e)
	{
		StiActions.Templates.InvokeOpen(base.Designer);
	}

	public override Type GetInterfaceType()
	{
		return null;
	}

	public static StiTemplatesCTService GetService()
	{
		return StiDesignerOptions.ContextMenu.FirstOrDefault((StiCTService c) => c.GetType() == typeof(StiTemplatesCTService)) as StiTemplatesCTService;
	}

	public override void CreateMenuItem(StiDesignerControl designer)
	{
		if (StiOptions.Designer.IsDeveloperDesigner)
		{
			base.Designer = designer;
			if (!StiOptions.Configuration.IsWeb)
			{
				ButtonItem buttonItem = new ButtonItem
				{
					Text = StiLocalization.Get("Wizards", "groupTemplates")
				};
				designer.cmDesigner.SubItems.Add(buttonItem);
				ButtonItem buttonItem2 = new ButtonItem
				{
					Text = StiLocalization.Get("Buttons", "Open"),
					Image = StiReportDesignImages.Actions.OpenFile()
				};
				buttonItem2.Click += OpenTemplate_Click;
				buttonItem.SubItems.Add(buttonItem2);
				ButtonItem buttonItem3 = new ButtonItem
				{
					Text = StiLocalization.Get("Buttons", "Save"),
					Image = StiReportDesignImages.Actions.SaveFile(),
					Enabled = (designer.SelectedComponentsOnPage.Count > 0)
				};
				buttonItem3.Click += SaveTemplate_Click;
				buttonItem.SubItems.Add(buttonItem3);
			}
		}
	}
}
