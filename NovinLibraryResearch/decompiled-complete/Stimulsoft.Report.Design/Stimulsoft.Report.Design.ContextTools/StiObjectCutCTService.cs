using System;
using System.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Design.ContextTools;

[StiServiceBitmap(typeof(StiCTService), "Stimulsoft.Report.Design.Images.ContextTools.Cut.png")]
public class StiObjectCutCTService : StiCTService
{
	public override StiPageType PageType => StiPageType.All;

	public override string GroupCategory => "Object";

	public override int Position => 21;

	private void Cut_Click(object sender, EventArgs e)
	{
		StiActions.Clipboard.InvokeCut(base.Designer);
	}

	public override Type GetInterfaceType()
	{
		return null;
	}

	public static StiObjectCutCTService GetService()
	{
		return StiDesignerOptions.ContextMenu.FirstOrDefault((StiCTService c) => c.GetType() == typeof(StiObjectCutCTService)) as StiObjectCutCTService;
	}

	public override void CreateMenuItem(StiDesignerControl designer)
	{
		base.Designer = designer;
		ButtonItem buttonItem = new ButtonItem
		{
			Text = StiLocalization.Get("MainMenu", "menuEditCut"),
			Image = StiReportDesignImages.Editor.ClipboardCut(),
			Enabled = StiActions.Clipboard.States.CanCut(designer)
		};
		buttonItem.Click += Cut_Click;
		designer.cmDesigner.SubItems.Add(buttonItem);
	}
}
