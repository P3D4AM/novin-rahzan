using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls;

namespace Stimulsoft.Report.Design.Wizards;

[ToolboxItem(false)]
public class StiReportWizardForm : Form
{
	private StiReport report;

	private StiReport createdReport;

	private IContainer components;

	[Browsable(false)]
	public StiReport Report
	{
		get
		{
			return report;
		}
		set
		{
			report = value;
		}
	}

	[Browsable(false)]
	public StiReport CreatedReport
	{
		get
		{
			return createdReport;
		}
		set
		{
			createdReport = value;
		}
	}

	public StiReportWizardForm()
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			((Form)this).ShowIcon = false;
		}
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
		((Control)this).SuspendLayout();
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.FromArgb(250, 250, 250);
		((Form)this).ClientSize = new Size(292, 266);
		((Control)this).Name = "StiReportWizardForm";
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiReportWizardForm";
		((Control)this).ResumeLayout(false);
	}
}
