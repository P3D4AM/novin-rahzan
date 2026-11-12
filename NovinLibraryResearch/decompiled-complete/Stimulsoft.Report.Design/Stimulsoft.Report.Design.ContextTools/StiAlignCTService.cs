using System;
using System.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Design.ContextTools;

public class StiAlignCTService : StiCTService
{
	public override StiPageType PageType => StiPageType.All;

	public override string GroupCategory => "Layout";

	public override int Position => 30;

	private void AlignToGrid_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeAlignToGrid(base.Designer);
	}

	private void AlignLeft_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeAlignLeft(base.Designer);
	}

	private void AlignCenter_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeAlignCenter(base.Designer);
	}

	private void AlignRight_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeAlignRight(base.Designer);
	}

	private void AlignTop_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeAlignTop(base.Designer);
	}

	private void AlignMiddle_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeAlignMiddle(base.Designer);
	}

	private void AlignBottom_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeAlignBottom(base.Designer);
	}

	private void CenterVertically_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeCenterVertically(base.Designer);
	}

	private void CenterHorizontally_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeCenterHorizontally(base.Designer);
	}

	public override Type GetInterfaceType()
	{
		return null;
	}

	public static StiAlignCTService GetService()
	{
		return StiDesignerOptions.ContextMenu.FirstOrDefault((StiCTService c) => c.GetType() == typeof(StiAlignCTService)) as StiAlignCTService;
	}

	public override void CreateMenuItem(StiDesignerControl designer)
	{
		base.Designer = designer;
		if (designer.SelectedComponentsOnPage.Count > 0 && !(designer.SelectedComponentsOnPage[0] is StiPage) && !StiOptions.Configuration.IsWeb)
		{
			ButtonItem buttonItem = new ButtonItem
			{
				Text = StiLocalization.Get("Toolbars", "Align")
			};
			designer.cmDesigner.SubItems.Add(buttonItem);
			ButtonItem buttonItem2 = new ButtonItem
			{
				Text = StiLocalization.Get("Toolbars", "AlignToGrid"),
				Image = StiReportDesignImages.Layout.AlignToGrid()
			};
			buttonItem2.Click += AlignToGrid_Click;
			buttonItem.SubItems.Add(buttonItem2);
			if (designer.SelectedComponentsOnPage.Count > 1)
			{
				ButtonItem buttonItem3 = new ButtonItem
				{
					Text = StiLocalization.Get("Toolbars", "AlignLeft"),
					Image = StiReportDesignImages.Layout.ContentAlignLeft()
				};
				buttonItem3.Click += AlignLeft_Click;
				buttonItem3.BeginGroup = true;
				buttonItem.SubItems.Add(buttonItem3);
				ButtonItem buttonItem4 = new ButtonItem
				{
					Text = StiLocalization.Get("Toolbars", "AlignCenter"),
					Image = StiReportDesignImages.Layout.ContentAlignCenter()
				};
				buttonItem4.Click += AlignCenter_Click;
				buttonItem.SubItems.Add(buttonItem4);
				ButtonItem buttonItem5 = new ButtonItem
				{
					Text = StiLocalization.Get("Toolbars", "AlignRight"),
					Image = StiReportDesignImages.Layout.ContentAlignRight()
				};
				buttonItem5.Click += AlignRight_Click;
				buttonItem.SubItems.Add(buttonItem5);
				ButtonItem buttonItem6 = new ButtonItem
				{
					Text = StiLocalization.Get("Toolbars", "AlignTop"),
					Image = StiReportDesignImages.Layout.ContentAlignTop(),
					BeginGroup = true
				};
				buttonItem6.Click += AlignTop_Click;
				buttonItem.SubItems.Add(buttonItem6);
				ButtonItem buttonItem7 = new ButtonItem
				{
					Text = StiLocalization.Get("Toolbars", "AlignMiddle"),
					Image = StiReportDesignImages.Layout.ContentAlignMiddle()
				};
				buttonItem7.Click += AlignMiddle_Click;
				buttonItem.SubItems.Add(buttonItem7);
				ButtonItem buttonItem8 = new ButtonItem
				{
					Text = StiLocalization.Get("Toolbars", "AlignBottom"),
					Image = StiReportDesignImages.Layout.ContentAlignBottom()
				};
				buttonItem8.Click += AlignBottom_Click;
				buttonItem.SubItems.Add(buttonItem8);
				ButtonItem buttonItem9 = new ButtonItem
				{
					Text = StiLocalization.Get("Toolbars", "CenterHorizontally"),
					Image = StiReportDesignImages.Layout.CenterHorizontally(),
					BeginGroup = true
				};
				buttonItem9.Click += CenterHorizontally_Click;
				buttonItem.SubItems.Add(buttonItem9);
				ButtonItem buttonItem10 = new ButtonItem
				{
					Text = StiLocalization.Get("Toolbars", "CenterVertically"),
					Image = StiReportDesignImages.Layout.CenterVertically()
				};
				buttonItem10.Click += CenterVertically_Click;
				buttonItem.SubItems.Add(buttonItem10);
			}
		}
	}
}
