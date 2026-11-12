using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design.Components;
using Stimulsoft.Report.Design.Inplace;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Report.Design.Check;

public class StiChecksViewerForm : StiInplaceEditorForm
{
	private StiReport report;

	private string fileName;

	private string safeFileName;

	private bool isReportChecker;

	private bool checkKeyEscape;

	private IContainer components;

	private StiPanelUX toolBarErrorPanel;

	private StiCheckStatusControl checkStatusControl;

	public event StiCheckStatusEventHandler Checked;

	public StiChecksViewerForm(StiReport report, bool checkKeyEscape)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			((Form)this).ShowIcon = false;
			if (StiOptions.Appearance.ShowFormsHelp)
			{
				((Form)this).HelpButton = true;
			}
			StiOptions.Localization.LoadDefault();
			this.checkKeyEscape = checkKeyEscape;
			if (checkKeyEscape)
			{
				((Control)checkStatusControl).KeyDown += new KeyEventHandler(This_KeyDown);
				((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
			}
			this.report = report;
			if (report == null)
			{
				isReportChecker = true;
				((Form)this).ShowInTaskbar = true;
			}
			else
			{
				((Form)this).ShowInTaskbar = false;
			}
			DoTheming();
			Localize();
			CheckStates();
			checkStatusControl.Checked += CheckStatusControl_Checked;
		}
	}

	private void InvokeChecked(StiCheckStatusEventArgs e)
	{
		if (this.Checked != null)
		{
			this.Checked(this, e);
		}
	}

	private void This_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyData == 27)
		{
			e.Handled = true;
			((Form)this).Close();
		}
	}

	private void This_Shown(object sender, EventArgs e)
	{
		if (report != null)
		{
			BuildChecks();
		}
	}

	private void This_FormClosing(object sender, FormClosingEventArgs e)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Invalid comparison between Unknown and I4
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Invalid comparison between Unknown and I4
		if (report == null || !isReportChecker || !checkStatusControl.IsReportChanged)
		{
			return;
		}
		DialogResult val = MessageBox.Show(string.Format(StiLocalization.Get("Questions", "qnSaveChanges"), safeFileName), "ReportChecker", (MessageBoxButtons)3, (MessageBoxIcon)64);
		if ((int)val == 2)
		{
			((CancelEventArgs)(object)e).Cancel = true;
		}
		else if ((int)val == 6)
		{
			try
			{
				report.Save(report.ReportFile);
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
		}
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("User-Manual/reports_designer_report_checker.htm");
	}

	private void CheckStatusControl_Checked(object sender, StiCheckStatusEventArgs e)
	{
		InvokeChecked(e);
	}

	private void ButtonSettings_Click(object sender, EventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		StiCheckSettingsForm stiCheckSettingsForm = new StiCheckSettingsForm();
		try
		{
			((Form)stiCheckSettingsForm).Owner = (Form)(object)this;
			((Form)stiCheckSettingsForm).ShowDialog();
		}
		finally
		{
			((IDisposable)stiCheckSettingsForm)?.Dispose();
		}
	}

	public override void Localize()
	{
		((Control)this).Text = StiLocalization.Get("Report", "ReportChecker");
	}

	private void DoTheming()
	{
		if (StiUX.IsDark)
		{
			((Control)checkStatusControl).BackColor = StiUX.Background;
		}
	}

	private void CheckStates()
	{
		if (!string.IsNullOrEmpty(fileName))
		{
			((Control)this).Text = string.Format("{0} - {1}", Path.GetFileName(fileName), StiLocalization.Get("Report", "ReportChecker"));
		}
		else
		{
			((Control)this).Text = StiLocalization.Get("Report", "ReportChecker");
		}
	}

	private void BuildChecks()
	{
		checkStatusControl.BuildChecks(report, compileReport: true);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		if (checkStatusControl != null)
		{
			if (checkStatusControl.superTooltip != null)
			{
				checkStatusControl.superTooltip.Dispose();
			}
			if (checkStatusControl.toolTip != null)
			{
				((Component)(object)checkStatusControl.toolTip).Dispose();
			}
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Expected O, but got Unknown
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Expected O, but got Unknown
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiChecksViewerForm));
		toolBarErrorPanel = new StiPanelUX();
		checkStatusControl = new StiCheckStatusControl(showSettings: true);
		((Control)this).SuspendLayout();
		((Control)toolBarErrorPanel).AutoSize = true;
		((Control)toolBarErrorPanel).Dock = (DockStyle)1;
		((Control)toolBarErrorPanel).Location = new Point(0, 0);
		((Control)toolBarErrorPanel).Name = "toolBarErrorPanel";
		((Control)toolBarErrorPanel).Size = new Size(749, 0);
		((Control)toolBarErrorPanel).TabIndex = 2;
		((Control)checkStatusControl).Dock = (DockStyle)5;
		((Control)checkStatusControl).Location = new Point(0, 0);
		((Control)checkStatusControl).Margin = new Padding(0);
		((Control)checkStatusControl).Name = "checkStatusControl";
		((Control)checkStatusControl).Padding = new Padding(2);
		((Control)checkStatusControl).Size = new Size(749, 552);
		((Control)checkStatusControl).TabIndex = 76;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(749, 552);
		((Control)this).Controls.Add((Control)(object)checkStatusControl);
		((Control)this).Controls.Add((Control)(object)toolBarErrorPanel);
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Control)this).MaximumSize = new Size(765, 1185);
		((Control)this).MinimumSize = new Size(765, 558);
		((Control)this).Name = "StiChecksViewerForm";
		((Form)this).SizeGripStyle = (SizeGripStyle)2;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "ChecksViewerForm";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).FormClosing += new FormClosingEventHandler(This_FormClosing);
		((Form)this).Shown += This_Shown;
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
