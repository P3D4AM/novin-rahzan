using System;
using System.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Design.ContextTools;

[StiServiceBitmap(typeof(StiCTService), "Stimulsoft.Report.Design.Images.ContextTools.Copy.png")]
public class StiObjectCopyCTService : StiCTService
{
	public override StiPageType PageType => StiPageType.All;

	public override string GroupCategory => "Object";

	public override int Position => 22;

	private void Copy_Click(object sender, EventArgs e)
	{
		StiActions.Clipboard.InvokeCopy(base.Designer);
	}

	public override Type GetInterfaceType()
	{
		return null;
	}

	public static StiObjectCopyCTService GetService()
	{
		return StiDesignerOptions.ContextMenu.FirstOrDefault((StiCTService c) => c.GetType() == typeof(StiObjectCopyCTService)) as StiObjectCopyCTService;
	}

	public override void CreateMenuItem(StiDesignerControl designer)
	{
		base.Designer = designer;
		ButtonItem buttonItem = new ButtonItem
		{
			Text = StiLocalization.Get("MainMenu", "menuEditCopy"),
			Image = StiReportDesignImages.Editor.ClipboardCopy(),
			Enabled = StiActions.Clipboard.States.CanCopy(designer)
		};
		buttonItem.Click += Copy_Click;
		designer.cmDesigner.SubItems.Add(buttonItem);
	}
}
