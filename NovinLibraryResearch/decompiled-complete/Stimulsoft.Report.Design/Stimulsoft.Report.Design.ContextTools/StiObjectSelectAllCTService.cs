using System;
using System.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Design.ContextTools;

[StiServiceBitmap(typeof(StiCTService), "Stimulsoft.Report.Design.Images.ContextTools.SelectAll.png")]
public class StiObjectSelectAllCTService : StiCTService
{
	public override StiPageType PageType => StiPageType.All;

	public override string GroupCategory => "SelectAll";

	public override int Position => 25;

	private void SelectAll_Click(object sender, EventArgs e)
	{
		StiActions.Clipboard.InvokeSelectAll(base.Designer);
	}

	public override Type GetInterfaceType()
	{
		return null;
	}

	public static StiObjectSelectAllCTService GetService()
	{
		return StiDesignerOptions.ContextMenu.FirstOrDefault((StiCTService c) => c.GetType() == typeof(StiObjectSelectAllCTService)) as StiObjectSelectAllCTService;
	}

	public override void CreateMenuItem(StiDesignerControl designer)
	{
		base.Designer = designer;
		ButtonItem buttonItem = new ButtonItem
		{
			Text = StiLocalization.Get("MainMenu", "menuEditSelectAll"),
			Image = StiReportDesignImages.Editor.SelectAll()
		};
		buttonItem.Click += SelectAll_Click;
		designer.cmDesigner.SubItems.Add(buttonItem);
	}
}
