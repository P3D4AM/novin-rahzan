using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Design;

public class StiPagesManagerForm : Form
{
	private StiDesignerControl designer;

	private IContainer components;

	private ListBox lbPages;

	private Bar stiToolBar1;

	private ButtonItem tbDown;

	private ButtonItem tbUp;

	private ToolTip toolTip;

	private ButtonItem tbPageNew;

	private ButtonItem tbDashboardNew;

	private ButtonItem tbPageSetup;

	private ButtonItem tbPageName;

	private ButtonItem tbPageDelete;

	private ButtonItem tbFormNew;

	public StiPagesManagerForm(StiDesignerControl designer)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		this.designer = designer;
		if (!((Component)this).DesignMode)
		{
			Build();
			Localize();
			StiSettings.Load();
			StiFormSettings.Load((Form)(object)this);
			((Form)this).ShowIcon = false;
		}
		DoScaling();
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("FormTitles", "PagesManagerForm");
		tbPageNew.Tooltip = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFilePageNew"));
		tbFormNew.Tooltip = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFileFormNew"));
		tbPageDelete.Tooltip = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFilePageDelete"));
		tbPageSetup.Tooltip = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFilePageSetup"));
		tbPageName.Tooltip = StiHint.CreateHint(StiLocalization.Get("Buttons", "Rename"));
		tbUp.Tooltip = StiHint.CreateHint(StiLocalization.Get("Buttons", "Up"));
		tbDown.Tooltip = StiHint.CreateHint(StiLocalization.Get("Buttons", "Down"));
	}

	private void DoScaling()
	{
		tbUp.Image = StiReportDesignImages.Actions.MoveUp();
		tbDown.Image = StiReportDesignImages.Actions.MoveDown();
		tbPageNew.Image = StiReportDesignImages.Actions.NewPage();
		tbDashboardNew.Image = StiReportDesignImages.Actions.NewDashboard();
		tbFormNew.Image = StiReportDesignImages.Actions.NewForm();
		tbPageDelete.Image = StiReportDesignImages.Actions.Remove();
		tbPageName.Image = StiReportDesignImages.Actions.Rename();
		tbPageSetup.Image = StiReportDesignImages.Actions.SetupPage();
		lbPages.ItemHeight = StiScale.I(16);
	}

	private void Build()
	{
		lbPages.Items.Clear();
		foreach (StiPage page in designer.Report.Pages)
		{
			lbPages.Items.Add((object)page.Name);
		}
		((ListControl)lbPages).SelectedIndex = 0;
	}

	private void tbPageNew_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokePageNew(designer);
		((Control)designer).Refresh();
		Build();
		((ListControl)lbPages).SelectedIndex = lbPages.Items.Count - 1;
	}

	private void tbDashboardNew_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokeDashboardNew(designer);
		((Control)designer).Refresh();
		Build();
		((ListControl)lbPages).SelectedIndex = lbPages.Items.Count - 1;
	}

	private void lbPages_SelectedIndexChanged(object sender, EventArgs e)
	{
		designer.Report.CurrentPage = ((ListControl)lbPages).SelectedIndex;
		designer.SelectTabPage(designer.Report.GetCurrentPage());
		((Control)designer).Refresh();
		tbUp.Enabled = ((ListControl)lbPages).SelectedIndex != 0;
		tbDown.Enabled = ((ListControl)lbPages).SelectedIndex != lbPages.Items.Count - 1;
	}

	private void tbPageDelete_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokePageDelete(designer);
		((Control)designer).Refresh();
		Build();
	}

	private void tbPageSetup_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokePageSetup(designer);
		((Control)designer).Refresh();
	}

	private void tbUp_Click(object sender, EventArgs e)
	{
		int selectedIndex = ((ListControl)lbPages).SelectedIndex;
		StiPage page = designer.Report.Pages[selectedIndex];
		designer.Report.Pages.RemoveAt(selectedIndex);
		designer.Report.Pages.Insert(selectedIndex - 1, page);
		designer.Builder.BuildReport();
		Build();
		((ListControl)lbPages).SelectedIndex = selectedIndex - 1;
	}

	private void tbDown_Click(object sender, EventArgs e)
	{
		int selectedIndex = ((ListControl)lbPages).SelectedIndex;
		StiPage page = designer.Report.Pages[selectedIndex];
		designer.Report.Pages.RemoveAt(selectedIndex);
		designer.Report.Pages.Insert(selectedIndex + 1, page);
		designer.Builder.BuildReport();
		Build();
		((ListControl)lbPages).SelectedIndex = selectedIndex + 1;
	}

	private void tbPageName_Click(object sender, EventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		StiPageRenameForm stiPageRenameForm = new StiPageRenameForm(designer.Report.GetCurrentPage());
		try
		{
			if (designer != null)
			{
				designer.AddOwnedForm((Form)(object)stiPageRenameForm);
			}
			((Form)stiPageRenameForm).ShowDialog();
			designer.Builder.BuildReport();
			Build();
		}
		finally
		{
			((IDisposable)stiPageRenameForm)?.Dispose();
		}
	}

	private void StiPagesManagerForm_Closed(object sender, EventArgs e)
	{
		StiFormSettings.Save((Form)(object)this);
	}

	private void tbFormNew_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokeFormNew(designer);
		((Control)designer).Refresh();
		Build();
		((ListControl)lbPages).SelectedIndex = lbPages.Items.Count - 1;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Expected O, but got Unknown
		components = new Container();
		lbPages = new ListBox();
		stiToolBar1 = new Bar();
		tbPageNew = new ButtonItem();
		tbDashboardNew = new ButtonItem();
		tbFormNew = new ButtonItem();
		tbPageDelete = new ButtonItem();
		tbPageSetup = new ButtonItem();
		tbPageName = new ButtonItem();
		tbUp = new ButtonItem();
		tbDown = new ButtonItem();
		toolTip = new ToolTip(components);
		((ISupportInitialize)stiToolBar1).BeginInit();
		((Control)this).SuspendLayout();
		((Control)lbPages).Anchor = (AnchorStyles)15;
		((Control)lbPages).BackColor = SystemColors.Window;
		lbPages.BorderStyle = (BorderStyle)0;
		lbPages.IntegralHeight = false;
		((Control)lbPages).Location = new Point(12, 38);
		((Control)lbPages).Name = "lbPages";
		((Control)lbPages).Size = new Size(305, 122);
		((Control)lbPages).TabIndex = 2;
		lbPages.SelectedIndexChanged += lbPages_SelectedIndexChanged;
		((Control)stiToolBar1).AccessibleDescription = "stiToolBar1 (stiToolBar1)";
		((Control)stiToolBar1).AccessibleName = "stiToolBar1";
		((Control)stiToolBar1).AccessibleRole = (AccessibleRole)22;
		((Control)stiToolBar1).Dock = (DockStyle)1;
		((Control)stiToolBar1).Font = new Font("Segoe UI", 9f);
		stiToolBar1.Items.AddRange(new BaseItem[8] { tbPageNew, tbDashboardNew, tbFormNew, tbPageDelete, tbPageSetup, tbPageName, tbUp, tbDown });
		stiToolBar1.Location = new Point(0, 0);
		stiToolBar1.Name = "stiToolBar1";
		stiToolBar1.RoundCorners = false;
		stiToolBar1.Size = new Size(330, 25);
		stiToolBar1.Stretch = true;
		stiToolBar1.Style = eDotNetBarStyle.Office2010;
		stiToolBar1.TabIndex = 11;
		stiToolBar1.TabStop = false;
		((Control)stiToolBar1).Text = "stiToolBar1";
		tbPageNew.Name = "tbPageNew";
		tbPageNew.Click += tbPageNew_Click;
		tbDashboardNew.Name = "tbDashboardNew";
		tbDashboardNew.Click += tbDashboardNew_Click;
		tbFormNew.Name = "tbFormNew";
		tbFormNew.Click += tbFormNew_Click;
		tbPageDelete.BeginGroup = true;
		tbPageDelete.Name = "tbPageClose";
		tbPageDelete.Click += tbPageDelete_Click;
		tbPageSetup.Name = "tbPageSetup";
		tbPageSetup.Click += tbPageSetup_Click;
		tbPageName.Name = "tbPageName";
		tbPageName.Click += tbPageName_Click;
		tbUp.BeginGroup = true;
		tbUp.ImageIndex = 3;
		tbUp.Name = "tbUp";
		tbUp.Click += tbUp_Click;
		tbDown.Name = "tbDown";
		tbDown.Click += tbDown_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(330, 174);
		((Control)this).Controls.Add((Control)(object)lbPages);
		((Control)this).Controls.Add((Control)(object)stiToolBar1);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiPagesManagerForm";
		((Form)this).ShowIcon = false;
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiPagesManagerForm";
		((Form)this).Closed += StiPagesManagerForm_Closed;
		((ISupportInitialize)stiToolBar1).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
