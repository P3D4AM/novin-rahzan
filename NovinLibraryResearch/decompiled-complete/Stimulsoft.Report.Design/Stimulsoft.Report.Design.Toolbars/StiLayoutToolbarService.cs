using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Design.Toolbars;

public class StiLayoutToolbarService : StiToolbarService
{
	private ButtonItem tbAlignToGrid;

	private ButtonItem tbAlignLeft;

	private ButtonItem tbAlignCenter;

	private ButtonItem tbAlignRight;

	private ButtonItem tbAlignTop;

	private ButtonItem tbAlignMiddle;

	private ButtonItem tbAlignBottom;

	private ButtonItem tbMakeSameSize;

	private ButtonItem tbMakeSameWidth;

	private ButtonItem tbMakeSameHeight;

	private ButtonItem tbMakeVerticalSpacingEqual;

	private ButtonItem tbMakeHorizontalSpacingEqual;

	private ButtonItem tbCenterVertically;

	private ButtonItem tbCenterHorizontally;

	private ButtonItem tbBringToFront;

	private ButtonItem tbSendToBack;

	private ButtonItem tbMoveForward;

	private ButtonItem tbMoveBackward;

	private ButtonItem tbLock;

	private ButtonItem tbLink;

	public override string ServiceName => StiLocalization.Get("Toolbars", "ToolbarLayout");

	public override int Position => 5;

	public override int DefaultDockLine => 2;

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowAlignToGrid
	{
		get
		{
			return StiOptions.Designer.Toolbars.LayoutToolbar.ShowAlignToGrid;
		}
		set
		{
			StiOptions.Designer.Toolbars.LayoutToolbar.ShowAlignToGrid = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowAlignLeft
	{
		get
		{
			return StiOptions.Designer.Toolbars.LayoutToolbar.ShowAlignLeft;
		}
		set
		{
			StiOptions.Designer.Toolbars.LayoutToolbar.ShowAlignLeft = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowAlignCenter
	{
		get
		{
			return StiOptions.Designer.Toolbars.LayoutToolbar.ShowAlignCenter;
		}
		set
		{
			StiOptions.Designer.Toolbars.LayoutToolbar.ShowAlignCenter = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowAlignRight
	{
		get
		{
			return StiOptions.Designer.Toolbars.LayoutToolbar.ShowAlignRight;
		}
		set
		{
			StiOptions.Designer.Toolbars.LayoutToolbar.ShowAlignRight = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowAlignTop
	{
		get
		{
			return StiOptions.Designer.Toolbars.LayoutToolbar.ShowAlignTop;
		}
		set
		{
			StiOptions.Designer.Toolbars.LayoutToolbar.ShowAlignTop = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowAlignMiddle
	{
		get
		{
			return StiOptions.Designer.Toolbars.LayoutToolbar.ShowAlignMiddle;
		}
		set
		{
			StiOptions.Designer.Toolbars.LayoutToolbar.ShowAlignMiddle = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowAlignBottom
	{
		get
		{
			return StiOptions.Designer.Toolbars.LayoutToolbar.ShowAlignBottom;
		}
		set
		{
			StiOptions.Designer.Toolbars.LayoutToolbar.ShowAlignBottom = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowMakeSameSize
	{
		get
		{
			return StiOptions.Designer.Toolbars.LayoutToolbar.ShowMakeSameSize;
		}
		set
		{
			StiOptions.Designer.Toolbars.LayoutToolbar.ShowMakeSameSize = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowMakeSameWidth
	{
		get
		{
			return StiOptions.Designer.Toolbars.LayoutToolbar.ShowMakeSameWidth;
		}
		set
		{
			StiOptions.Designer.Toolbars.LayoutToolbar.ShowMakeSameWidth = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowMakeSameHeight
	{
		get
		{
			return StiOptions.Designer.Toolbars.LayoutToolbar.ShowMakeSameHeight;
		}
		set
		{
			StiOptions.Designer.Toolbars.LayoutToolbar.ShowMakeSameHeight = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowMakeVerticalSpacingEqual
	{
		get
		{
			return StiOptions.Designer.Toolbars.LayoutToolbar.ShowMakeVerticalSpacingEqual;
		}
		set
		{
			StiOptions.Designer.Toolbars.LayoutToolbar.ShowMakeVerticalSpacingEqual = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowMakeHorizontalSpacingEqual
	{
		get
		{
			return StiOptions.Designer.Toolbars.LayoutToolbar.ShowMakeHorizontalSpacingEqual;
		}
		set
		{
			StiOptions.Designer.Toolbars.LayoutToolbar.ShowMakeHorizontalSpacingEqual = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowCenterVertically
	{
		get
		{
			return StiOptions.Designer.Toolbars.LayoutToolbar.ShowCenterVertically;
		}
		set
		{
			StiOptions.Designer.Toolbars.LayoutToolbar.ShowCenterVertically = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowCenterHorizontally
	{
		get
		{
			return StiOptions.Designer.Toolbars.LayoutToolbar.ShowCenterHorizontally;
		}
		set
		{
			StiOptions.Designer.Toolbars.LayoutToolbar.ShowCenterHorizontally = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowBringToFront
	{
		get
		{
			return StiOptions.Designer.Toolbars.LayoutToolbar.ShowBringToFront;
		}
		set
		{
			StiOptions.Designer.Toolbars.LayoutToolbar.ShowBringToFront = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowSendToBack
	{
		get
		{
			return StiOptions.Designer.Toolbars.LayoutToolbar.ShowSendToBack;
		}
		set
		{
			StiOptions.Designer.Toolbars.LayoutToolbar.ShowSendToBack = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowMoveForward
	{
		get
		{
			return StiOptions.Designer.Toolbars.LayoutToolbar.ShowMoveForward;
		}
		set
		{
			StiOptions.Designer.Toolbars.LayoutToolbar.ShowMoveForward = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowMoveBackward
	{
		get
		{
			return StiOptions.Designer.Toolbars.LayoutToolbar.ShowMoveBackward;
		}
		set
		{
			StiOptions.Designer.Toolbars.LayoutToolbar.ShowMoveBackward = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowLock
	{
		get
		{
			return StiOptions.Designer.Toolbars.LayoutToolbar.ShowLock;
		}
		set
		{
			StiOptions.Designer.Toolbars.LayoutToolbar.ShowLock = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowLink
	{
		get
		{
			return StiOptions.Designer.Toolbars.LayoutToolbar.ShowLink;
		}
		set
		{
			StiOptions.Designer.Toolbars.LayoutToolbar.ShowLink = value;
		}
	}

	public bool BringToFrontButtonEnabled
	{
		get
		{
			return tbBringToFront.Enabled;
		}
		set
		{
			tbBringToFront.Enabled = value;
		}
	}

	public bool SendToBackButtonEnabled
	{
		get
		{
			return tbSendToBack.Enabled;
		}
		set
		{
			tbBringToFront.Enabled = value;
		}
	}

	private void Refresh(object sender, EventArgs e)
	{
		if (!base.ToolBar.Visible)
		{
			return;
		}
		base.ToolBar.SuspendLayout();
		StiComponentsCollection stiComponentsCollection = new StiComponentsCollection();
		foreach (StiComponent item in Designer.SelectedComponentsOnPage)
		{
			if (StiRestrictionsHelper.IsAllowChange(item))
			{
				stiComponentsCollection.Add(item);
			}
		}
		if (stiComponentsCollection.Count >= 1 && Report != null)
		{
			stiComponentsCollection.SortBySelectionTick();
			if (tbMakeSameSize != null)
			{
				tbMakeSameSize.Tooltip = StiHint.CreateHint(string.Format(StiLocalization.Get("Toolbars", "MakeSameSize"), stiComponentsCollection[0].Name));
			}
			if (tbMakeSameWidth != null)
			{
				tbMakeSameWidth.Tooltip = StiHint.CreateHint(string.Format(StiLocalization.Get("Toolbars", "MakeSameWidth"), stiComponentsCollection[0].Name));
			}
			if (tbMakeSameHeight != null)
			{
				tbMakeSameHeight.Tooltip = StiHint.CreateHint(string.Format(StiLocalization.Get("Toolbars", "MakeSameHeight"), stiComponentsCollection[0].Name));
			}
		}
		if (stiComponentsCollection.Count >= 1 && Designer.ComponentsOnPage.Count >= 2 && Report != null)
		{
			if (tbBringToFront != null && !tbBringToFront.Enabled)
			{
				tbBringToFront.Enabled = true;
			}
			if (tbSendToBack != null && !tbSendToBack.Enabled)
			{
				tbSendToBack.Enabled = true;
			}
			if (tbMoveForward != null && !tbMoveForward.Enabled)
			{
				tbMoveForward.Enabled = true;
			}
			if (tbMoveBackward != null && !tbMoveBackward.Enabled)
			{
				tbMoveBackward.Enabled = true;
			}
		}
		else
		{
			if (tbBringToFront != null && tbBringToFront.Enabled)
			{
				tbBringToFront.Enabled = false;
			}
			if (tbSendToBack != null && tbSendToBack.Enabled)
			{
				tbSendToBack.Enabled = false;
			}
			if (tbMoveForward != null && tbMoveForward.Enabled)
			{
				tbMoveForward.Enabled = false;
			}
			if (tbMoveBackward != null && tbMoveBackward.Enabled)
			{
				tbMoveBackward.Enabled = false;
			}
		}
		if (stiComponentsCollection.Count >= 1 && Designer.ComponentsOnPage.Count >= 1 && Report != null)
		{
			if (tbLock != null && !tbLock.Enabled)
			{
				tbLock.Enabled = CheckPropertyEnabled(Report, "Locked");
			}
			if (tbLink != null && !tbLink.Enabled)
			{
				tbLink.Enabled = CheckPropertyEnabled(Report, "Linked");
			}
			object @lock = Designer.Report.GetCurrentPage().GetLock();
			if (@lock == null)
			{
				if (tbLock != null)
				{
					tbLock.Checked = false;
				}
			}
			else if (tbLock != null)
			{
				tbLock.Checked = (bool)@lock;
			}
			object link = Designer.Report.GetCurrentPage().GetLink();
			if (link == null)
			{
				if (tbLink != null)
				{
					tbLink.Checked = false;
				}
			}
			else if (tbLink != null)
			{
				tbLink.Checked = (bool)link;
			}
			bool flag = CheckPropertyEnabled(Report, "Left");
			bool flag2 = CheckPropertyEnabled(Report, "Top");
			if (tbAlignToGrid != null && flag && flag2 && !tbAlignToGrid.Enabled)
			{
				tbAlignToGrid.Enabled = true;
			}
			if (tbCenterHorizontally != null && flag && !tbCenterHorizontally.Enabled)
			{
				tbCenterHorizontally.Enabled = true;
			}
			if (tbCenterVertically != null && flag2 && !tbCenterVertically.Enabled)
			{
				tbCenterVertically.Enabled = true;
			}
		}
		else
		{
			if (tbLock != null && tbLock.Enabled)
			{
				tbLock.Enabled = false;
			}
			if (tbLink != null && tbLink.Enabled)
			{
				tbLink.Enabled = false;
			}
			if (tbAlignToGrid != null && tbAlignToGrid.Enabled)
			{
				tbAlignToGrid.Enabled = false;
			}
			if (tbCenterHorizontally != null && tbCenterHorizontally.Enabled)
			{
				tbCenterHorizontally.Enabled = false;
			}
			if (tbCenterVertically != null && tbCenterVertically.Enabled)
			{
				tbCenterVertically.Enabled = false;
			}
		}
		if (stiComponentsCollection.Count < 2 || Report == null)
		{
			if (tbAlignLeft != null && tbAlignLeft.Enabled)
			{
				tbAlignLeft.Enabled = false;
			}
			if (tbAlignCenter != null && tbAlignCenter.Enabled)
			{
				tbAlignCenter.Enabled = false;
			}
			if (tbAlignRight != null && tbAlignRight.Enabled)
			{
				tbAlignRight.Enabled = false;
			}
			if (tbAlignTop != null && tbAlignTop.Enabled)
			{
				tbAlignTop.Enabled = false;
			}
			if (tbAlignMiddle != null && tbAlignMiddle.Enabled)
			{
				tbAlignMiddle.Enabled = false;
			}
			if (tbAlignBottom != null && tbAlignBottom.Enabled)
			{
				tbAlignBottom.Enabled = false;
			}
			if (tbMakeSameSize != null && tbMakeSameSize.Enabled)
			{
				tbMakeSameSize.Enabled = false;
			}
			if (tbMakeSameWidth != null && tbMakeSameWidth.Enabled)
			{
				tbMakeSameWidth.Enabled = false;
			}
			if (tbMakeSameHeight != null && tbMakeSameHeight.Enabled)
			{
				tbMakeSameHeight.Enabled = false;
			}
			if (tbMakeHorizontalSpacingEqual != null && tbMakeHorizontalSpacingEqual.Enabled)
			{
				tbMakeHorizontalSpacingEqual.Enabled = false;
			}
			if (tbMakeVerticalSpacingEqual != null && tbMakeVerticalSpacingEqual.Enabled)
			{
				tbMakeVerticalSpacingEqual.Enabled = false;
			}
		}
		else
		{
			bool flag3 = CheckPropertyEnabled(Report, "Left");
			bool flag4 = CheckPropertyEnabled(Report, "Top");
			bool flag5 = CheckPropertyEnabled(Report, "Width");
			bool flag6 = CheckPropertyEnabled(Report, "Height");
			if (tbAlignLeft != null && flag3 && !tbAlignLeft.Enabled)
			{
				tbAlignLeft.Enabled = true;
			}
			if (tbAlignCenter != null && flag3 && !tbAlignCenter.Enabled)
			{
				tbAlignCenter.Enabled = true;
			}
			if (tbAlignRight != null && flag3 && !tbAlignRight.Enabled)
			{
				tbAlignRight.Enabled = true;
			}
			if (tbAlignTop != null && flag4 && !tbAlignTop.Enabled)
			{
				tbAlignTop.Enabled = true;
			}
			if (tbAlignMiddle != null && flag4 && !tbAlignMiddle.Enabled)
			{
				tbAlignMiddle.Enabled = true;
			}
			if (tbAlignBottom != null && flag4 && !tbAlignBottom.Enabled)
			{
				tbAlignBottom.Enabled = true;
			}
			if (tbMakeSameSize != null && flag5 && flag6 && !tbMakeSameSize.Enabled)
			{
				tbMakeSameSize.Enabled = true;
			}
			if (tbMakeSameWidth != null && flag5 && !tbMakeSameWidth.Enabled)
			{
				tbMakeSameWidth.Enabled = true;
			}
			if (tbMakeSameHeight != null && flag6 && !tbMakeSameHeight.Enabled)
			{
				tbMakeSameHeight.Enabled = true;
			}
			if (tbMakeHorizontalSpacingEqual != null && flag3 && !tbMakeHorizontalSpacingEqual.Enabled)
			{
				tbMakeHorizontalSpacingEqual.Enabled = true;
			}
			if (tbMakeVerticalSpacingEqual != null && flag4 && !tbMakeVerticalSpacingEqual.Enabled)
			{
				tbMakeVerticalSpacingEqual.Enabled = true;
			}
		}
		base.ToolBar.ResumeLayout();
	}

	private void tbAlignToGrid_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeAlignToGrid(Designer);
	}

	private void tbAlignLeft_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeAlignLeft(Designer);
	}

	private void tbAlignCenter_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeAlignCenter(Designer);
	}

	private void tbAlignRight_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeAlignRight(Designer);
	}

	private void tbAlignTop_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeAlignTop(Designer);
	}

	private void tbAlignMiddle_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeAlignMiddle(Designer);
	}

	private void tbAlignBottom_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeAlignBottom(Designer);
	}

	private void tbMakeSameSize_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeMakeSameSize(Designer);
	}

	private void tbMakeSameWidth_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeMakeSameWidth(Designer);
	}

	private void tbMakeSameHeight_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeMakeSameHeight(Designer);
	}

	private void tbMakeHorizontalSpacingEqual_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeMakeHorizontalSpacingEqual(Designer);
	}

	private void tbMakeVerticalSpacingEqual_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeMakeVerticalSpacingEqual(Designer);
	}

	private void tbCenterVertically_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeCenterVertically(Designer);
	}

	private void tbCenterHorizontally_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeCenterHorizontally(Designer);
	}

	private void tbBringToFront_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeBringToFront(Designer);
	}

	private void tbSendToBack_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeSendToBack(Designer);
	}

	private void tbMoveForward_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeMoveForward(Designer);
	}

	private void tbMoveBackward_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeMoveBackward(Designer);
	}

	private void tbLock_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeLock(Designer, tbLock.Checked);
	}

	private void tbLink_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeLink(Designer, tbLink.Checked);
	}

	private void LocalizeSuperToolTip()
	{
		if (Designer == null)
		{
			return;
		}
		if (StiOptions.Designer.ShowSuperToolTip)
		{
			Graphics globalMeasureGraphics = StiReport.GlobalMeasureGraphics;
			if (tbAlignToGrid != null)
			{
				SetSuperToolTip(tbAlignToGrid, StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignToGrid")), "AlignToGrid", globalMeasureGraphics);
			}
			if (tbAlignLeft != null)
			{
				SetSuperToolTip(tbAlignLeft, StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignLeft")), "AlignComponentLeft", globalMeasureGraphics);
			}
			if (tbAlignCenter != null)
			{
				SetSuperToolTip(tbAlignCenter, StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignCenter")), "AlignComponentCenter", globalMeasureGraphics);
			}
			if (tbAlignRight != null)
			{
				SetSuperToolTip(tbAlignRight, StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignRight")), "AlignComponentRight", globalMeasureGraphics);
			}
			if (tbAlignTop != null)
			{
				SetSuperToolTip(tbAlignTop, StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignTop")), "AlignComponentTop", globalMeasureGraphics);
			}
			if (tbAlignMiddle != null)
			{
				SetSuperToolTip(tbAlignMiddle, StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignMiddle")), "AlignComponentMiddle", globalMeasureGraphics);
			}
			if (tbAlignBottom != null)
			{
				SetSuperToolTip(tbAlignBottom, StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignBottom")), "AlignComponentBottom", globalMeasureGraphics);
			}
			if (tbMakeHorizontalSpacingEqual != null)
			{
				SetSuperToolTip(tbMakeHorizontalSpacingEqual, StiHint.CreateHint(StiLocalization.Get("Toolbars", "MakeHorizontalSpacingEqual")), "MakeHorizontalSpacingEqual", globalMeasureGraphics);
			}
			if (tbMakeVerticalSpacingEqual != null)
			{
				SetSuperToolTip(tbMakeVerticalSpacingEqual, StiHint.CreateHint(StiLocalization.Get("Toolbars", "MakeVerticalSpacingEqual")), "MakeVerticalSpacingEqual", globalMeasureGraphics);
			}
			if (tbCenterVertically != null)
			{
				SetSuperToolTip(tbCenterVertically, StiHint.CreateHint(StiLocalization.Get("Toolbars", "CenterVertically")), "CenterVertically", globalMeasureGraphics);
			}
			if (tbCenterHorizontally != null)
			{
				SetSuperToolTip(tbCenterHorizontally, StiHint.CreateHint(StiLocalization.Get("Toolbars", "CenterHorizontally")), "CenterHorizontally", globalMeasureGraphics);
			}
			if (tbBringToFront != null)
			{
				SetSuperToolTip(tbBringToFront, StiHint.CreateHint(StiLocalization.Get("Toolbars", "BringToFront")), "BringToFront", globalMeasureGraphics);
			}
			if (tbSendToBack != null)
			{
				SetSuperToolTip(tbSendToBack, StiHint.CreateHint(StiLocalization.Get("Toolbars", "SendToBack")), "SendToBack", globalMeasureGraphics);
			}
			if (tbMoveForward != null)
			{
				SetSuperToolTip(tbMoveForward, StiHint.CreateHint(StiLocalization.Get("Toolbars", "MoveForward")), "MoveForward", globalMeasureGraphics);
			}
			if (tbMoveBackward != null)
			{
				SetSuperToolTip(tbMoveBackward, StiHint.CreateHint(StiLocalization.Get("Toolbars", "MoveBackward")), "MoveBackward", globalMeasureGraphics);
			}
			if (tbLock != null)
			{
				SetSuperToolTip(tbLock, StiHint.CreateHint(StiLocalization.Get("Toolbars", "Lock")), "Lock", globalMeasureGraphics);
			}
			if (tbLink != null)
			{
				SetSuperToolTip(tbLink, StiHint.CreateHint(StiLocalization.Get("Toolbars", "Link")), "Link", globalMeasureGraphics);
			}
			if (tbMakeSameSize != null)
			{
				tbMakeSameSize.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "MakeSameSize"));
			}
			if (tbMakeSameWidth != null)
			{
				tbMakeSameWidth.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "MakeSameWidth"));
			}
			if (tbMakeSameHeight != null)
			{
				tbMakeSameHeight.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "MakeSameHeight"));
			}
		}
		else
		{
			if (tbAlignToGrid != null)
			{
				tbAlignToGrid.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignToGrid"));
			}
			if (tbAlignLeft != null)
			{
				tbAlignLeft.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignLeft"));
			}
			if (tbAlignCenter != null)
			{
				tbAlignCenter.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignMiddle"));
			}
			if (tbAlignRight != null)
			{
				tbAlignRight.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignRight"));
			}
			if (tbAlignTop != null)
			{
				tbAlignTop.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignTop"));
			}
			if (tbAlignMiddle != null)
			{
				tbAlignMiddle.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignCenter"));
			}
			if (tbAlignBottom != null)
			{
				tbAlignBottom.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignBottom"));
			}
			if (tbMakeHorizontalSpacingEqual != null)
			{
				tbMakeHorizontalSpacingEqual.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "MakeHorizontalSpacingEqual"));
			}
			if (tbMakeVerticalSpacingEqual != null)
			{
				tbMakeVerticalSpacingEqual.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "MakeVerticalSpacingEqual"));
			}
			if (tbCenterVertically != null)
			{
				tbCenterVertically.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "CenterVertically"));
			}
			if (tbCenterHorizontally != null)
			{
				tbCenterHorizontally.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "CenterHorizontally"));
			}
			if (tbBringToFront != null)
			{
				tbBringToFront.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "BringToFront"));
			}
			if (tbSendToBack != null)
			{
				tbSendToBack.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "SendToBack"));
			}
			if (tbMoveForward != null)
			{
				tbMoveForward.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "MoveForward"));
			}
			if (tbMoveBackward != null)
			{
				tbMoveBackward.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "MoveBackward"));
			}
			if (tbLock != null)
			{
				tbLock.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "Lock"));
			}
			if (tbLink != null)
			{
				tbLink.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "Link"));
			}
		}
	}

	public void InvokeAlignToGrid()
	{
		tbAlignToGrid_Click(null, EventArgs.Empty);
	}

	public void InvokeAlignLeft()
	{
		tbAlignLeft_Click(null, EventArgs.Empty);
	}

	public void InvokeAlignCenter()
	{
		tbAlignCenter_Click(null, EventArgs.Empty);
	}

	public void InvokeAlignRight()
	{
		tbAlignRight_Click(null, EventArgs.Empty);
	}

	public void InvokeAlignTop()
	{
		tbAlignTop_Click(null, EventArgs.Empty);
	}

	public void InvokeAlignMiddle()
	{
		tbAlignMiddle_Click(null, EventArgs.Empty);
	}

	public void InvokeAlignBottom()
	{
		tbAlignBottom_Click(null, EventArgs.Empty);
	}

	public void InvokeMakeSameSize()
	{
		tbMakeSameSize_Click(null, EventArgs.Empty);
	}

	public void InvokeMakeSameWidth()
	{
		tbMakeSameWidth_Click(null, EventArgs.Empty);
	}

	public void InvokeMakeSameHeight()
	{
		tbMakeSameHeight_Click(null, EventArgs.Empty);
	}

	public void InvokeMakeHorizontalSpacingEqual()
	{
		tbMakeHorizontalSpacingEqual_Click(null, EventArgs.Empty);
	}

	public void InvokeMakeVerticalSpacingEqual()
	{
		tbMakeVerticalSpacingEqual_Click(null, EventArgs.Empty);
	}

	public void InvokeCenterVertically()
	{
		tbCenterVertically_Click(null, EventArgs.Empty);
	}

	public void InvokeCenterHorizontally()
	{
		tbCenterHorizontally_Click(null, EventArgs.Empty);
	}

	public void InvokeBringToFront()
	{
		tbBringToFront_Click(null, EventArgs.Empty);
	}

	public void InvokeSendToBack()
	{
		tbSendToBack_Click(null, EventArgs.Empty);
	}

	public void InvokeMoveForward()
	{
		tbMoveForward_Click(null, EventArgs.Empty);
	}

	public void InvokeMoveBackward()
	{
		tbMoveBackward_Click(null, EventArgs.Empty);
	}

	public void InvokeLock()
	{
		tbLock_Click(null, EventArgs.Empty);
	}

	public void InvokeLink()
	{
		tbLink_Click(null, EventArgs.Empty);
	}

	public static StiLayoutToolbarService GetService()
	{
		return GetService(null);
	}

	public static StiLayoutToolbarService GetService(StiDesignerControl designer)
	{
		if (designer == null)
		{
			return StiDesignerOptions.Toolbars.FirstOrDefault((StiToolbarService c) => c.GetType() == typeof(StiLayoutToolbarService)) as StiLayoutToolbarService;
		}
		return designer.Services.GetServices(typeof(StiToolbarService)).OfType<StiLayoutToolbarService>().FirstOrDefault();
	}

	public override void Localize()
	{
		base.Localize();
		LocalizeSuperToolTip();
		if (base.ToolBar != null)
		{
			((Control)base.ToolBar).Text = StiLocalization.Get("Toolbars", "ToolbarLayout");
		}
		if (tbAlignToGrid != null)
		{
			tbAlignToGrid.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignToGrid"));
		}
		if (tbAlignLeft != null)
		{
			tbAlignLeft.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignLeft"));
		}
		if (tbAlignCenter != null)
		{
			tbAlignCenter.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignCenter"));
		}
		if (tbAlignRight != null)
		{
			tbAlignRight.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignRight"));
		}
		if (tbAlignTop != null)
		{
			tbAlignTop.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignTop"));
		}
		if (tbAlignMiddle != null)
		{
			tbAlignMiddle.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignMiddle"));
		}
		if (tbAlignBottom != null)
		{
			tbAlignBottom.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignBottom"));
		}
		if (tbMakeSameSize != null)
		{
			tbMakeSameSize.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "MakeSameSize"));
		}
		if (tbMakeSameWidth != null)
		{
			tbMakeSameWidth.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "MakeSameWidth"));
		}
		if (tbMakeSameHeight != null)
		{
			tbMakeSameHeight.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "MakeSameHeight"));
		}
		if (tbMakeHorizontalSpacingEqual != null)
		{
			tbMakeHorizontalSpacingEqual.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "MakeHorizontalSpacingEqual"));
		}
		if (tbMakeVerticalSpacingEqual != null)
		{
			tbMakeVerticalSpacingEqual.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "MakeVerticalSpacingEqual"));
		}
		if (tbCenterVertically != null)
		{
			tbCenterVertically.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "CenterVertically"));
		}
		if (tbCenterHorizontally != null)
		{
			tbCenterHorizontally.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "CenterHorizontally"));
		}
		if (tbBringToFront != null)
		{
			tbBringToFront.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "BringToFront"));
		}
		if (tbSendToBack != null)
		{
			tbSendToBack.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "SendToBack"));
		}
		if (tbMoveForward != null)
		{
			tbMoveForward.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "MoveForward"));
		}
		if (tbMoveBackward != null)
		{
			tbMoveBackward.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "MoveBackward"));
		}
		if (tbLock != null)
		{
			tbLock.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "Lock"));
		}
		if (tbLink != null)
		{
			tbLink.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "Link"));
		}
	}

	public override void Init(StiDesignerControl designer)
	{
		base.Init(designer);
		CreateToolbar();
		base.ToolBar.SuspendLayout();
		((Control)base.ToolBar).Text = StiLocalization.Get("Toolbars", "ToolbarLayout");
		if (ShowAlignToGrid)
		{
			Add(ref tbAlignToGrid, StiReportDesignImages.Layout.AlignToGrid(), StiLocalization.Get("Toolbars", "AlignToGrid"), tbAlignToGrid_Click);
			tbAlignToGrid.Name = "tbAlignToGrid";
		}
		AddToolButtonSeparator();
		if (ShowAlignLeft)
		{
			Add(ref tbAlignLeft, StiReportDesignImages.Layout.ContentAlignLeft(), StiLocalization.Get("Toolbars", "AlignLeft"), tbAlignLeft_Click);
			tbAlignLeft.Name = "tbAlignLeft";
		}
		if (ShowAlignCenter)
		{
			Add(ref tbAlignCenter, StiReportDesignImages.Layout.ContentAlignCenter(), StiLocalization.Get("Toolbars", "AlignCenter"), tbAlignCenter_Click);
			tbAlignCenter.Name = "tbAlignCenter";
		}
		if (ShowAlignRight)
		{
			Add(ref tbAlignRight, StiReportDesignImages.Layout.ContentAlignRight(), StiLocalization.Get("Toolbars", "AlignRight"), tbAlignRight_Click);
			tbAlignRight.Name = "tbAlignRight";
		}
		AddToolButtonSeparator();
		if (ShowAlignTop)
		{
			Add(ref tbAlignTop, StiReportDesignImages.Layout.ContentAlignTop(), StiLocalization.Get("Toolbars", "AlignTop"), tbAlignTop_Click);
			tbAlignTop.Name = "tbAlignTop";
		}
		if (ShowAlignMiddle)
		{
			Add(ref tbAlignMiddle, StiReportDesignImages.Layout.ContentAlignMiddle(), StiLocalization.Get("Toolbars", "AlignMiddle"), tbAlignMiddle_Click);
			tbAlignMiddle.Name = "tbAlignMiddle";
		}
		if (ShowAlignBottom)
		{
			Add(ref tbAlignBottom, StiReportDesignImages.Layout.ContentAlignBottom(), StiLocalization.Get("Toolbars", "AlignBottom"), tbAlignBottom_Click);
			tbAlignBottom.Name = "tbAlignBottom";
		}
		AddToolButtonSeparator();
		if (ShowMakeSameSize)
		{
			Add(ref tbMakeSameSize, StiReportDesignImages.Layout.MakeSameSizeAs(), StiLocalization.Get("Toolbars", "MakeSameSize"), tbMakeSameSize_Click);
			tbMakeSameSize.Name = "tbMakeSameSize";
		}
		if (ShowMakeSameWidth)
		{
			Add(ref tbMakeSameWidth, StiReportDesignImages.Layout.MakeSameWidthAs(), StiLocalization.Get("Toolbars", "MakeSameWidth"), tbMakeSameWidth_Click);
			tbMakeSameWidth.Name = "tbMakeSameWidth";
		}
		if (ShowMakeSameHeight)
		{
			Add(ref tbMakeSameHeight, StiReportDesignImages.Layout.MakeSameHeightAs(), StiLocalization.Get("Toolbars", "MakeSameHeight"), tbMakeSameHeight_Click);
			tbMakeSameHeight.Name = "tbMakeSameHeight";
		}
		AddToolButtonSeparator();
		if (ShowMakeHorizontalSpacingEqual)
		{
			Add(ref tbMakeHorizontalSpacingEqual, StiReportDesignImages.Layout.MakeHorizontalSpacingEqual(), StiLocalization.Get("Toolbars", "MakeHorizontalSpacingEqual"), tbMakeHorizontalSpacingEqual_Click);
			tbMakeHorizontalSpacingEqual.Name = "tbMakeHorizontalSpacingEqual";
		}
		if (ShowMakeVerticalSpacingEqual)
		{
			Add(ref tbMakeVerticalSpacingEqual, StiReportDesignImages.Layout.MakeVerticalSpacingEqual(), StiLocalization.Get("Toolbars", "MakeVerticalSpacingEqual"), tbMakeVerticalSpacingEqual_Click);
			tbMakeVerticalSpacingEqual.Name = "tbMakeVerticalSpacingEqual";
		}
		AddToolButtonSeparator();
		if (ShowCenterHorizontally)
		{
			Add(ref tbCenterHorizontally, StiReportDesignImages.Layout.CenterHorizontally(), StiLocalization.Get("Toolbars", "CenterHorizontally"), tbCenterHorizontally_Click);
			tbCenterHorizontally.Name = "tbCenterHorizontally";
		}
		if (ShowCenterVertically)
		{
			Add(ref tbCenterVertically, StiReportDesignImages.Layout.CenterVertically(), StiLocalization.Get("Toolbars", "CenterVertically"), tbCenterVertically_Click);
			tbCenterVertically.Name = "tbCenterVertically";
		}
		AddToolButtonSeparator();
		if (ShowBringToFront)
		{
			Add(ref tbBringToFront, StiReportDesignImages.Layout.BringToFront(), StiLocalization.Get("Toolbars", "BringToFront"), tbBringToFront_Click);
			tbBringToFront.Name = "tbBringToFront";
		}
		if (ShowSendToBack)
		{
			Add(ref tbSendToBack, StiReportDesignImages.Layout.SendToBack(), StiLocalization.Get("Toolbars", "SendToBack"), tbSendToBack_Click);
			tbSendToBack.Name = "tbSendToBack";
		}
		AddToolButtonSeparator();
		if (ShowMoveForward)
		{
			Add(ref tbMoveForward, StiReportDesignImages.Layout.MoveForward(), StiLocalization.Get("Toolbars", "MoveForward"), tbMoveForward_Click);
			tbMoveForward.Name = "tbMoveForward";
		}
		if (ShowMoveBackward)
		{
			Add(ref tbMoveBackward, StiReportDesignImages.Layout.MoveBackward(), StiLocalization.Get("Toolbars", "MoveBackward"), tbMoveBackward_Click);
			tbMoveBackward.Name = "tbMoveBackward";
		}
		AddToolButtonSeparator();
		if (ShowLock)
		{
			Add(ref tbLock, StiReportDesignImages.Layout.Lock(), StiLocalization.Get("Toolbars", "Lock"), tbLock_Click);
			tbLock.Name = "tbLock";
			tbLock.AutoCheckOnClick = true;
		}
		if (ShowLink)
		{
			Add(ref tbLink, StiReportDesignImages.Layout.Link(), StiLocalization.Get("Toolbars", "Link"), tbLink_Click);
			tbLink.Name = "tbLink";
			tbLink.AutoCheckOnClick = true;
		}
		AddToolbarToDockSite();
		base.ToolBar.ResumeLayout();
		LocalizeSuperToolTip();
		designer.RefreshDesigner += Refresh;
	}

	public override void Dispose()
	{
		base.Dispose();
		tbAlignToGrid = null;
		tbAlignLeft = null;
		tbAlignCenter = null;
		tbAlignRight = null;
		tbAlignTop = null;
		tbAlignMiddle = null;
		tbAlignBottom = null;
		tbMakeSameSize = null;
		tbMakeSameWidth = null;
		tbMakeSameHeight = null;
		tbMakeVerticalSpacingEqual = null;
		tbMakeHorizontalSpacingEqual = null;
		tbCenterVertically = null;
		tbCenterHorizontally = null;
		tbBringToFront = null;
		tbSendToBack = null;
		tbMoveForward = null;
		tbMoveBackward = null;
		tbLock = null;
		tbLink = null;
	}
}
