using System;
using System.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Design.ContextTools;

[StiServiceBitmap(typeof(StiCTService), "Stimulsoft.Report.Design.Images.ContextTools.Delete.png")]
public class StiObjectDeleteCTService : StiCTService
{
	public override StiPageType PageType => StiPageType.All;

	public override string GroupCategory => "Object";

	public override int Position => 24;

	private void Delete_Click(object sender, EventArgs e)
	{
		StiActions.Clipboard.InvokeDelete(base.Designer);
	}

	public override Type GetInterfaceType()
	{
		return null;
	}

	public static StiObjectDeleteCTService GetService()
	{
		return StiDesignerOptions.ContextMenu.FirstOrDefault((StiCTService c) => c.GetType() == typeof(StiObjectDeleteCTService)) as StiObjectDeleteCTService;
	}

	public override void CreateMenuItem(StiDesignerControl designer)
	{
		base.Designer = designer;
		ButtonItem buttonItem = new ButtonItem
		{
			Text = StiLocalization.Get("MainMenu", "menuEditDelete"),
			Image = StiReportDesignImages.Actions.Remove(),
			Enabled = StiActions.Clipboard.States.CanDelete(designer)
		};
		buttonItem.Click += Delete_Click;
		designer.cmDesigner.SubItems.Add(buttonItem);
	}
}
