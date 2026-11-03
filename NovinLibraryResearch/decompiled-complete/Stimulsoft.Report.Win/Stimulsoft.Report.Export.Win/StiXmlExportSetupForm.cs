using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Report.Export.Win;

public class StiXmlExportSetupForm : Form
{
	private IContainer components;

	private Button btCancel;

	private Button btOk;

	public CheckBox cbOpenAfterExport;

	private StiGroupLine glButtons;

	public bool OpenAfterExport => cbOpenAfterExport.Checked;

	public bool OpenAfterExportVisible
	{
		get
		{
			return ((Control)cbOpenAfterExport).Visible;
		}
		set
		{
			((Control)cbOpenAfterExport).Visible = value;
		}
	}

	public bool OpenAfterExportEnabled
	{
		get
		{
			return ((Control)cbOpenAfterExport).Enabled;
		}
		set
		{
			((Control)cbOpenAfterExport).Enabled = value;
		}
	}

	public StiXmlExportSetupForm()
	{
		StiFormExt.InitializeNetCoreFont((Form)(object)this);
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (((Component)this).DesignMode)
		{
			return;
		}
		if (((ReadOnlyCollectionBase)(object)Application.OpenForms).Count > 0)
		{
			Form val = Application.OpenForms[((ReadOnlyCollectionBase)(object)Application.OpenForms).Count - 1];
			if (val != null && val.TopMost)
			{
				((Form)this).TopMost = true;
			}
		}
		if (StiOptions.Appearance.ShowFormsHelp)
		{
			((Form)this).HelpButton = true;
		}
		StiSettings.Load();
		cbOpenAfterExport.Checked = StiSettings.GetBool(((Control)this).Name, "OpenAfterExport", cbOpenAfterExport.Checked);
		Localize();
	}

	private void Localize()
	{
		((Control)cbOpenAfterExport).Text = StiLocalization.Get("Export", "OpenAfterExport");
		((Control)this).Text = StiLocalization.Get("Export", "title");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
	}

	private void StiXmlExportSetupForm_Closed(object sender, EventArgs e)
	{
		StiSettings.Load();
		StiSettings.Set(((Control)this).Name, "OpenAfterExport", cbOpenAfterExport.Checked);
		StiSettings.Save();
	}

	private void StiXmlExportSetupForm_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			e.Handled = true;
			((Form)this).Close();
		}
	}

	private void ThisHelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/exports_data_xml.htm");
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
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Expected O, but got Unknown
		btCancel = new Button();
		btOk = new Button();
		cbOpenAfterExport = new CheckBox();
		glButtons = new StiGroupLine();
		((Control)glButtons).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)btCancel).Anchor = (AnchorStyles)10;
		btCancel.DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(232, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 3;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btOk).Anchor = (AnchorStyles)10;
		btOk.DialogResult = (DialogResult)1;
		((Control)btOk).Location = new Point(152, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 2;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)cbOpenAfterExport).AutoSize = true;
		((Control)cbOpenAfterExport).Location = new Point(12, 12);
		((Control)cbOpenAfterExport).Name = "cbOpenAfterExport";
		((Control)cbOpenAfterExport).Size = new Size(104, 17);
		((Control)cbOpenAfterExport).TabIndex = 8;
		((Control)cbOpenAfterExport).Text = "OpenAfterExport";
		((Control)glButtons).Controls.Add((Control)(object)btOk);
		((Control)glButtons).Controls.Add((Control)(object)btCancel);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = (StiLinePosition)0;
		glButtons.LineStyle = (StiLineStyle)0;
		((Control)glButtons).Location = new Point(0, 41);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(312, 46);
		((Control)glButtons).TabIndex = 4;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(312, 87);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Control)this).Controls.Add((Control)(object)cbOpenAfterExport);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiXmlExportSetupForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiHtmlExportSetupForm";
		((Form)this).HelpButtonClicked += ThisHelpButtonClicked;
		((Form)this).Closed += StiXmlExportSetupForm_Closed;
		((Control)this).KeyDown += new KeyEventHandler(StiXmlExportSetupForm_KeyDown);
		((Control)glButtons).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
