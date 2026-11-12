using System;
using System.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Design.ContextTools;

public class StiOrderCTService : StiCTService
{
	public override StiPageType PageType => StiPageType.All;

	public override string GroupCategory => "Layout";

	public override int Position => 31;

	private void BringToFront_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeBringToFront(base.Designer);
	}

	private void SendToBack_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeSendToBack(base.Designer);
	}

	private void MoveForward_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeMoveForward(base.Designer);
	}

	private void MoveBackward_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeMoveBackward(base.Designer);
	}

	public override Type GetInterfaceType()
	{
		return null;
	}

	public static StiOrderCTService GetService()
	{
		return StiDesignerOptions.ContextMenu.FirstOrDefault((StiCTService c) => c.GetType() == typeof(StiOrderCTService)) as StiOrderCTService;
	}

	public override void CreateMenuItem(StiDesignerControl designer)
	{
		base.Designer = designer;
		if (designer.SelectedComponentsOnPage.Count > 0 && !(designer.SelectedComponentsOnPage[0] is StiPage) && !StiOptions.Configuration.IsWeb)
		{
			ButtonItem buttonItem = new ButtonItem
			{
				Text = StiLocalization.Get("Toolbars", "Order")
			};
			designer.cmDesigner.SubItems.Add(buttonItem);
			ButtonItem buttonItem2 = new ButtonItem
			{
				Text = StiLocalization.Get("Toolbars", "BringToFront"),
				Image = StiReportDesignImages.Layout.BringToFront()
			};
			buttonItem2.Click += BringToFront_Click;
			buttonItem.SubItems.Add(buttonItem2);
			ButtonItem buttonItem3 = new ButtonItem
			{
				Text = StiLocalization.Get("Toolbars", "SendToBack"),
				Image = StiReportDesignImages.Layout.SendToBack()
			};
			buttonItem3.Click += SendToBack_Click;
			buttonItem.SubItems.Add(buttonItem3);
			ButtonItem buttonItem4 = new ButtonItem
			{
				Text = StiLocalization.Get("Toolbars", "MoveForward"),
				Image = StiReportDesignImages.Layout.MoveForward()
			};
			buttonItem4.Click += MoveForward_Click;
			buttonItem.SubItems.Add(buttonItem4);
			ButtonItem buttonItem5 = new ButtonItem
			{
				Text = StiLocalization.Get("Toolbars", "MoveBackward"),
				Image = StiReportDesignImages.Layout.MoveBackward()
			};
			buttonItem5.Click += MoveBackward_Click;
			buttonItem.SubItems.Add(buttonItem5);
		}
	}
}
