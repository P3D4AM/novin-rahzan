using System;
using System.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Design.ContextTools;

[StiServiceBitmap(typeof(StiCTService), "Stimulsoft.Report.Design.Images.ContextTools.Paste.png")]
public class StiObjectPasteCTService : StiCTService
{
	public override StiPageType PageType => StiPageType.All;

	public override string GroupCategory => "Object";

	public override int Position => 23;

	private void Paste_Click(object sender, EventArgs e)
	{
		StiActions.Clipboard.InvokePaste(base.Designer);
	}

	public override Type GetInterfaceType()
	{
		return null;
	}

	public static StiObjectPasteCTService GetService()
	{
		return StiDesignerOptions.ContextMenu.FirstOrDefault((StiCTService c) => c.GetType() == typeof(StiObjectPasteCTService)) as StiObjectPasteCTService;
	}

	public override void CreateMenuItem(StiDesignerControl designer)
	{
		base.Designer = designer;
		ButtonItem buttonItem = new ButtonItem
		{
			Text = StiLocalization.Get("MainMenu", "menuEditPaste"),
			Image = StiReportDesignImages.Editor.ClipboardPaste(),
			Enabled = StiActions.Clipboard.States.CanPaste(designer)
		};
		buttonItem.Click += Paste_Click;
		designer.cmDesigner.SubItems.Add(buttonItem);
	}
}
