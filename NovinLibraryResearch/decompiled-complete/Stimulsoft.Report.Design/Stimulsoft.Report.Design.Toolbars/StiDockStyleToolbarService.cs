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

public class StiDockStyleToolbarService : StiToolbarService
{
	private ButtonItem buttonDockStyleFill;

	private ButtonItem buttonDockStyleNone;

	private ButtonItem buttonDockStyleTop;

	private ButtonItem buttonDockStyleLeft;

	private ButtonItem buttonDockStyleBottom;

	private ButtonItem buttonDockStyleRight;

	public override string ServiceName => StiLocalization.Get("PropertyMain", "DockStyle");

	public override int Position => 6;

	public override int DefaultDockLine => 3;

	public override bool DefaultVisible => false;

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowDockStyleFill
	{
		get
		{
			return StiOptions.Designer.Toolbars.DockStyleToolbar.ShowDockStyleFill;
		}
		set
		{
			StiOptions.Designer.Toolbars.DockStyleToolbar.ShowDockStyleFill = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowDockStyleNone
	{
		get
		{
			return StiOptions.Designer.Toolbars.DockStyleToolbar.ShowDockStyleNone;
		}
		set
		{
			StiOptions.Designer.Toolbars.DockStyleToolbar.ShowDockStyleNone = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowDockStyleTop
	{
		get
		{
			return StiOptions.Designer.Toolbars.DockStyleToolbar.ShowDockStyleTop;
		}
		set
		{
			StiOptions.Designer.Toolbars.DockStyleToolbar.ShowDockStyleTop = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowDockStyleLeft
	{
		get
		{
			return StiOptions.Designer.Toolbars.DockStyleToolbar.ShowDockStyleLeft;
		}
		set
		{
			StiOptions.Designer.Toolbars.DockStyleToolbar.ShowDockStyleLeft = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowDockStyleBottom
	{
		get
		{
			return StiOptions.Designer.Toolbars.DockStyleToolbar.ShowDockStyleBottom;
		}
		set
		{
			StiOptions.Designer.Toolbars.DockStyleToolbar.ShowDockStyleBottom = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowDockStyleRight
	{
		get
		{
			return StiOptions.Designer.Toolbars.DockStyleToolbar.ShowDockStyleRight;
		}
		set
		{
			StiOptions.Designer.Toolbars.DockStyleToolbar.ShowDockStyleRight = value;
		}
	}

	private void Designer_RefreshDesigner(object sender, EventArgs e)
	{
		if (!base.ToolBar.Visible)
		{
			return;
		}
		object obj = ((Report != null) ? Report.GetCurrentPage().GetDockStyle() : null);
		if (obj != null && !CheckPropertyEnabled(Report, "DockStyle"))
		{
			obj = null;
		}
		if (obj != null)
		{
			base.ToolBar.SuspendLayout();
			StiDockStyle stiDockStyle = (StiDockStyle)obj;
			if (buttonDockStyleBottom != null && !buttonDockStyleBottom.Enabled)
			{
				buttonDockStyleBottom.Enabled = true;
			}
			if (buttonDockStyleLeft != null && !buttonDockStyleLeft.Enabled)
			{
				buttonDockStyleLeft.Enabled = true;
			}
			if (buttonDockStyleTop != null && !buttonDockStyleTop.Enabled)
			{
				buttonDockStyleTop.Enabled = true;
			}
			if (buttonDockStyleRight != null && !buttonDockStyleRight.Enabled)
			{
				buttonDockStyleRight.Enabled = true;
			}
			if (buttonDockStyleNone != null && !buttonDockStyleNone.Enabled)
			{
				buttonDockStyleNone.Enabled = true;
			}
			if (buttonDockStyleFill != null && !buttonDockStyleFill.Enabled)
			{
				buttonDockStyleFill.Enabled = true;
			}
			if (buttonDockStyleBottom != null)
			{
				buttonDockStyleBottom.Checked = stiDockStyle == StiDockStyle.Bottom;
			}
			if (buttonDockStyleTop != null)
			{
				buttonDockStyleTop.Checked = stiDockStyle == StiDockStyle.Top;
			}
			if (buttonDockStyleLeft != null)
			{
				buttonDockStyleLeft.Checked = stiDockStyle == StiDockStyle.Left;
			}
			if (buttonDockStyleRight != null)
			{
				buttonDockStyleRight.Checked = stiDockStyle == StiDockStyle.Right;
			}
			if (buttonDockStyleFill != null)
			{
				buttonDockStyleFill.Checked = stiDockStyle == StiDockStyle.Fill;
			}
			if (buttonDockStyleNone != null)
			{
				buttonDockStyleNone.Checked = stiDockStyle == StiDockStyle.None;
			}
			base.ToolBar.ResumeLayout();
		}
		else
		{
			base.ToolBar.SuspendLayout();
			if (buttonDockStyleBottom != null && buttonDockStyleBottom.Enabled)
			{
				buttonDockStyleBottom.Enabled = false;
			}
			if (buttonDockStyleLeft != null && buttonDockStyleLeft.Enabled)
			{
				buttonDockStyleLeft.Enabled = false;
			}
			if (buttonDockStyleTop != null && buttonDockStyleTop.Enabled)
			{
				buttonDockStyleTop.Enabled = false;
			}
			if (buttonDockStyleRight != null && buttonDockStyleRight.Enabled)
			{
				buttonDockStyleRight.Enabled = false;
			}
			if (buttonDockStyleNone != null && buttonDockStyleNone.Enabled)
			{
				buttonDockStyleNone.Enabled = false;
			}
			if (buttonDockStyleFill != null && buttonDockStyleFill.Enabled)
			{
				buttonDockStyleFill.Enabled = false;
			}
			base.ToolBar.ResumeLayout();
		}
	}

	private void ButtonDockStyleFill_Click(object sender, EventArgs e)
	{
		buttonDockStyleFill.Checked = true;
		buttonDockStyleNone.Checked = false;
		buttonDockStyleTop.Checked = false;
		buttonDockStyleLeft.Checked = false;
		buttonDockStyleBottom.Checked = false;
		buttonDockStyleRight.Checked = false;
		StiActions.DockStyle.InvokeDockStyleFill(Designer);
	}

	private void ButtonDockStyleNone_Click(object sender, EventArgs e)
	{
		buttonDockStyleFill.Checked = false;
		buttonDockStyleNone.Checked = true;
		buttonDockStyleTop.Checked = false;
		buttonDockStyleLeft.Checked = false;
		buttonDockStyleBottom.Checked = false;
		buttonDockStyleRight.Checked = false;
		StiActions.DockStyle.InvokeDockStyleNone(Designer);
	}

	private void ButtonDockStyleTop_Click(object sender, EventArgs e)
	{
		buttonDockStyleFill.Checked = false;
		buttonDockStyleNone.Checked = false;
		buttonDockStyleTop.Checked = true;
		buttonDockStyleLeft.Checked = false;
		buttonDockStyleBottom.Checked = false;
		buttonDockStyleRight.Checked = false;
		StiActions.DockStyle.InvokeDockStyleTop(Designer);
	}

	private void ButtonDockStyleLeft_Click(object sender, EventArgs e)
	{
		buttonDockStyleFill.Checked = false;
		buttonDockStyleNone.Checked = false;
		buttonDockStyleTop.Checked = false;
		buttonDockStyleLeft.Checked = true;
		buttonDockStyleBottom.Checked = false;
		buttonDockStyleRight.Checked = false;
		StiActions.DockStyle.InvokeDockStyleLeft(Designer);
	}

	private void ButtonDockStyleBottom_Click(object sender, EventArgs e)
	{
		buttonDockStyleFill.Checked = false;
		buttonDockStyleNone.Checked = false;
		buttonDockStyleTop.Checked = false;
		buttonDockStyleLeft.Checked = false;
		buttonDockStyleBottom.Checked = true;
		buttonDockStyleRight.Checked = false;
		StiActions.DockStyle.InvokeDockStyleBottom(Designer);
	}

	private void ButtonDockStyleRight_Click(object sender, EventArgs e)
	{
		buttonDockStyleFill.Checked = false;
		buttonDockStyleNone.Checked = false;
		buttonDockStyleTop.Checked = false;
		buttonDockStyleLeft.Checked = false;
		buttonDockStyleBottom.Checked = false;
		buttonDockStyleRight.Checked = true;
		StiActions.DockStyle.InvokeDockStyleRight(Designer);
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
			if (buttonDockStyleFill != null)
			{
				SetSuperToolTip(buttonDockStyleFill, StiHint.CreateHint(StiLocalization.Get("PropertyEnum", "StiDockStyleFill")), "DockStyleFill", globalMeasureGraphics);
			}
			if (buttonDockStyleNone != null)
			{
				SetSuperToolTip(buttonDockStyleNone, StiHint.CreateHint(StiLocalization.Get("PropertyEnum", "StiDockStyleNone")), "DockStyleNone", globalMeasureGraphics);
			}
			if (buttonDockStyleTop != null)
			{
				SetSuperToolTip(buttonDockStyleTop, StiHint.CreateHint(StiLocalization.Get("PropertyEnum", "StiDockStyleTop")), "DockStyleTop", globalMeasureGraphics);
			}
			if (buttonDockStyleLeft != null)
			{
				SetSuperToolTip(buttonDockStyleLeft, StiHint.CreateHint(StiLocalization.Get("PropertyEnum", "StiDockStyleLeft")), "DockStyleLeft", globalMeasureGraphics);
			}
			if (buttonDockStyleBottom != null)
			{
				SetSuperToolTip(buttonDockStyleBottom, StiHint.CreateHint(StiLocalization.Get("PropertyEnum", "StiDockStyleBottom")), "DockStyleBottom", globalMeasureGraphics);
			}
			if (buttonDockStyleRight != null)
			{
				SetSuperToolTip(buttonDockStyleRight, StiHint.CreateHint(StiLocalization.Get("PropertyEnum", "StiDockStyleRight")), "DockStyleRight", globalMeasureGraphics);
			}
		}
		else
		{
			if (buttonDockStyleFill != null)
			{
				buttonDockStyleFill.Tooltip = StiHint.CreateHint(StiLocalization.Get("PropertyEnum", "StiDockStyleFill"));
			}
			if (buttonDockStyleNone != null)
			{
				buttonDockStyleNone.Tooltip = StiHint.CreateHint(StiLocalization.Get("PropertyEnum", "StiDockStyleNone"));
			}
			if (buttonDockStyleTop != null)
			{
				buttonDockStyleTop.Tooltip = StiHint.CreateHint(StiLocalization.Get("PropertyEnum", "StiDockStyleTop"));
			}
			if (buttonDockStyleLeft != null)
			{
				buttonDockStyleLeft.Tooltip = StiHint.CreateHint(StiLocalization.Get("PropertyEnum", "StiDockStyleLeft"));
			}
			if (buttonDockStyleBottom != null)
			{
				buttonDockStyleBottom.Tooltip = StiHint.CreateHint(StiLocalization.Get("PropertyEnum", "StiDockStyleBottom"));
			}
			if (buttonDockStyleRight != null)
			{
				buttonDockStyleRight.Tooltip = StiHint.CreateHint(StiLocalization.Get("PropertyEnum", "StiDockStyleRight"));
			}
		}
	}

	public void InvokeDockStyleFill()
	{
		ButtonDockStyleFill_Click(null, EventArgs.Empty);
	}

	public void InvokeDockStyleNone_Click()
	{
		ButtonDockStyleNone_Click(null, EventArgs.Empty);
	}

	public void InvokeDockStyleTop_Click()
	{
		ButtonDockStyleTop_Click(null, EventArgs.Empty);
	}

	public void InvokeDockStyleLeft_Click()
	{
		ButtonDockStyleLeft_Click(null, EventArgs.Empty);
	}

	public void InvokeDockStyleBottom_Click()
	{
		ButtonDockStyleBottom_Click(null, EventArgs.Empty);
	}

	public void InvokeDockStyleRight()
	{
		ButtonDockStyleRight_Click(null, EventArgs.Empty);
	}

	public static StiDockStyleToolbarService GetService()
	{
		return GetService(null);
	}

	public static StiDockStyleToolbarService GetService(StiDesignerControl designer)
	{
		if (designer == null)
		{
			return StiDesignerOptions.Toolbars.FirstOrDefault((StiToolbarService c) => c.GetType() == typeof(StiDockStyleToolbarService)) as StiDockStyleToolbarService;
		}
		return designer.Services.GetServices(typeof(StiToolbarService)).OfType<StiDockStyleToolbarService>().FirstOrDefault();
	}

	public override void Localize()
	{
		base.Localize();
		LocalizeSuperToolTip();
		if (base.ToolBar != null)
		{
			((Control)base.ToolBar).Text = StiLocalization.Get("PropertyMain", "DockStyle");
		}
		if (buttonDockStyleFill != null)
		{
			buttonDockStyleFill.Text = StiHint.CreateHint(StiLocalization.Get("PropertyEnum", "StiDockStyleFill"));
		}
		if (buttonDockStyleNone != null)
		{
			buttonDockStyleNone.Text = StiHint.CreateHint(StiLocalization.Get("PropertyEnum", "StiDockStyleNone"));
		}
		if (buttonDockStyleTop != null)
		{
			buttonDockStyleTop.Text = StiHint.CreateHint(StiLocalization.Get("PropertyEnum", "StiDockStyleTop"));
		}
		if (buttonDockStyleLeft != null)
		{
			buttonDockStyleLeft.Text = StiHint.CreateHint(StiLocalization.Get("PropertyEnum", "StiDockStyleLeft"));
		}
		if (buttonDockStyleBottom != null)
		{
			buttonDockStyleBottom.Text = StiHint.CreateHint(StiLocalization.Get("PropertyEnum", "StiDockStyleBottom"));
		}
		if (buttonDockStyleRight != null)
		{
			buttonDockStyleRight.Text = StiHint.CreateHint(StiLocalization.Get("PropertyEnum", "StiDockStyleRight"));
		}
	}

	public override void Init(StiDesignerControl designer)
	{
		base.Init(designer);
		CreateToolbar();
		base.ToolBar.SuspendLayout();
		((Control)base.ToolBar).Text = StiLocalization.Get("PropertyMain", "DockStyle");
		if (ShowDockStyleFill)
		{
			Add(ref buttonDockStyleFill, StiReportDesignImages.Editor.DockStyleFill(), StiLocalization.Get("PropertyEnum", "StiDockStyleFill"), ButtonDockStyleFill_Click);
			buttonDockStyleFill.Name = "buttonDockStyleFill";
		}
		if (ShowDockStyleNone)
		{
			Add(ref buttonDockStyleNone, StiReportDesignImages.Editor.DockStyleNone(), StiLocalization.Get("PropertyEnum", "StiDockStyleNone"), ButtonDockStyleNone_Click);
			buttonDockStyleNone.Name = "buttonDockStyleNone";
		}
		AddToolButtonSeparator();
		if (ShowDockStyleTop)
		{
			Add(ref buttonDockStyleTop, StiReportDesignImages.Editor.DockStyleTop(), StiLocalization.Get("PropertyEnum", "StiDockStyleTop"), ButtonDockStyleTop_Click);
			buttonDockStyleTop.Name = "buttonDockStyleTop";
		}
		if (ShowDockStyleLeft)
		{
			Add(ref buttonDockStyleLeft, StiReportDesignImages.Editor.DockStyleLeft(), StiLocalization.Get("PropertyEnum", "StiDockStyleLeft"), ButtonDockStyleLeft_Click);
			buttonDockStyleLeft.Name = "buttonDockStyleLeft";
		}
		if (ShowDockStyleBottom)
		{
			Add(ref buttonDockStyleBottom, StiReportDesignImages.Editor.DockStyleBottom(), StiLocalization.Get("PropertyEnum", "StiDockStyleBottom"), ButtonDockStyleBottom_Click);
			buttonDockStyleBottom.Name = "buttonDockStyleBottom";
		}
		if (ShowDockStyleRight)
		{
			Add(ref buttonDockStyleRight, StiReportDesignImages.Editor.DockStyleRight(), StiLocalization.Get("PropertyEnum", "StiDockStyleRight"), ButtonDockStyleRight_Click);
			buttonDockStyleRight.Name = "buttonDockStyleRight";
		}
		AddToolbarToDockSite();
		LocalizeSuperToolTip();
		base.ToolBar.ResumeLayout();
		designer.RefreshDesigner += Designer_RefreshDesigner;
	}

	public override void Dispose()
	{
		base.Dispose();
		buttonDockStyleFill = null;
		buttonDockStyleNone = null;
		buttonDockStyleTop = null;
		buttonDockStyleLeft = null;
		buttonDockStyleBottom = null;
		buttonDockStyleRight = null;
	}
}
