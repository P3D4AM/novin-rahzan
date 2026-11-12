using System;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Design.ContextTools;

[StiServiceBitmap(typeof(StiCTService), "Stimulsoft.Report.Design.Images.ContextTools.Properties.png")]
public class StiPropertiesCTService : StiCTService
{
	public override StiPageType PageType => StiPageType.All;

	public override string GroupCategory => "Properties";

	public override int Position => 102;

	private void Properties_Click(object sender, EventArgs e)
	{
		try
		{
			StiDockingPanelSelector.PropertiesPanelSelect(base.Designer);
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	public override Type GetInterfaceType()
	{
		return null;
	}

	public static StiPropertiesCTService GetService()
	{
		return StiDesignerOptions.ContextMenu.FirstOrDefault((StiCTService c) => c.GetType() == typeof(StiPropertiesCTService)) as StiPropertiesCTService;
	}

	public override void CreateMenuItem(StiDesignerControl designer)
	{
		base.Designer = designer;
		ButtonItem buttonItem = new ButtonItem
		{
			Text = StiLocalization.Get("Panels", "Properties"),
			Image = StiReportDesignImages.Actions.Properties()
		};
		buttonItem.Click += Properties_Click;
		designer.cmDesigner.SubItems.Add(buttonItem);
	}
}
