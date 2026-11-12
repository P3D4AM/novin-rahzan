using System;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Design.ContextTools;

[StiServiceBitmap(typeof(StiCTService), "Stimulsoft.Report.Design.Images.ContextTools.Design.png")]
public class StiDesignComponentCTService : StiCTService
{
	public override StiPageType PageType => StiPageType.Page | StiPageType.Dashboard;

	public override string GroupCategory => "Design";

	public override int Position => 0;

	private void Design_Click(object sender, EventArgs e)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			base.Designer.UndoRedoSave(StiLocalization.Get("Buttons", "Design"), onlyCurrentPage: true);
			StiComponentDesigner componentDesigner = StiComponentDesigner.GetComponentDesigner(base.Designer, base.Component.GetType());
			componentDesigner.Design(base.Component);
			base.Designer.Builder.Redraw();
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	public override Type GetInterfaceType()
	{
		return typeof(IStiComponentDesigner);
	}

	public static StiDesignComponentCTService GetService()
	{
		return StiDesignerOptions.ContextMenu.FirstOrDefault((StiCTService c) => c.GetType() == typeof(StiDesignComponentCTService)) as StiDesignComponentCTService;
	}

	public override void CreateMenuItem(StiDesignerControl designer)
	{
		base.Designer = designer;
		StiComponentsCollection selected = designer.GetSelected();
		if (selected.Count > 0)
		{
			base.Component = selected[0];
			ButtonItem buttonItem = new ButtonItem
			{
				Text = StiLocalization.Get("MainMenu", "menuContextDesign"),
				Image = StiReportDesignImages.Actions.Design()
			};
			buttonItem.Click += Design_Click;
			designer.cmDesigner.SubItems.Add(buttonItem);
		}
	}
}
