using System;
using System.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Design.ContextTools;

[StiServiceBitmap(typeof(StiCTService), "Stimulsoft.Report.Design.Images.ContextTools.ClearContent.png")]
public class StiClearContentCTService : StiCTService
{
	public override StiPageType PageType => StiPageType.Page;

	public override string GroupCategory => "Tools";

	public override int Position => 201;

	private void ClearContents_Click(object sender, EventArgs e)
	{
		StiActions.Font.InvokeClearContents(base.Designer);
	}

	public override Type GetInterfaceType()
	{
		return typeof(IStiText);
	}

	public static StiClearContentCTService GetService()
	{
		return StiDesignerOptions.ContextMenu.FirstOrDefault((StiCTService c) => c.GetType() == typeof(StiClearContentCTService)) as StiClearContentCTService;
	}

	public override void CreateMenuItem(StiDesignerControl designer)
	{
		base.Designer = designer;
		StiComponentsCollection selected = designer.GetSelected();
		base.Component = null;
		foreach (StiComponent item in selected)
		{
			if (item is IStiText)
			{
				base.Component = item;
				break;
			}
		}
		if (base.Component != null)
		{
			ButtonItem buttonItem = new ButtonItem
			{
				Text = StiLocalization.Get("MainMenu", "menuEditClearContents"),
				Image = StiReportDesignImages.Editor.ClearContent()
			};
			buttonItem.Click += ClearContents_Click;
			designer.cmDesigner.SubItems.Add(buttonItem);
		}
	}
}
