using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Dashboard.Export.Settings;
using Stimulsoft.Report;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Dashboard.Viewer.Export;

public class StiDashboardHtmlExportSetupForm : Form
{
	private IContainer components;

	private StiGroupLine groupLineButtons;

	private Button buttonOk;

	private Button buttonCancel;

	private CheckBox checkBoxOpenAfterExport;

	private Label labelImageQuality;

	private StiDropDownBox comboBoxImageQuality;

	private Label labelScale;

	private StiDropDownBox comboBoxScale;

	private CheckBox checkBoxEnableAnimation;

	public StiDashboardHtmlExportSetupForm()
	{
		((Form)(object)this).InitializeNetCoreFont();
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			if (StiOptions.Appearance.ShowFormsHelp)
			{
				((Form)this).HelpButton = true;
			}
			Localize();
			LoadSettings();
		}
	}

	public StiDashboardHtmlExportSetupForm(IStiElement element)
		: this()
	{
		UpdateState(element);
	}

	private void Localize()
	{
		((Control)this).Text = Loc.Get("Export", "ExportTypeHtmlFile").Replace("...", "") + " - " + Loc.Get("Export", "title").Replace("&", "_");
		((Control)labelImageQuality).Text = Loc.Get("Export", "ImageQuality").Replace(":", "");
		((Control)labelScale).Text = Loc.Get("PropertyMain", "Scale");
		((Control)checkBoxEnableAnimation).Text = Loc.Get("Export", "EnableAnimation");
		((Control)checkBoxOpenAfterExport).Text = Loc.Get("Export", "OpenAfterExport");
		((Control)buttonOk).Text = Loc.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = Loc.Get("Buttons", "Cancel");
	}

	private void UpdateState(IStiElement element)
	{
	}

	public StiDashboardExportSettings GetExportSettings()
	{
		int.TryParse(((Control)comboBoxImageQuality).Text.Replace("%", ""), out var result);
		int.TryParse(((Control)comboBoxScale).Text.Replace("%", ""), out var result2);
		return new StiHtmlDashboardExportSettings
		{
			OpenAfterExport = checkBoxOpenAfterExport.Checked,
			ImageQuality = result,
			Scale = result2,
			EnableAnimation = checkBoxEnableAnimation.Checked
		};
	}

	private void LoadSettings()
	{
		StiSettings.Load();
		((Control)comboBoxImageQuality).Text = StiSettings.GetStr(((Control)this).Name, "ImageQuality", "200%");
		((Control)comboBoxScale).Text = StiSettings.GetStr(((Control)this).Name, "Scale", "100%");
		checkBoxEnableAnimation.Checked = StiSettings.GetBool(((Control)this).Name, "EnableAnimation", nullValue: true);
		checkBoxOpenAfterExport.Checked = StiSettings.GetBool(((Control)this).Name, "OpenAfterExport", nullValue: true);
	}

	private void SaveSettings()
	{
		StiSettings.Load();
		StiSettings.Set(((Control)this).Name, "ImageQuality", ((Control)comboBoxImageQuality).Text.EndsWith("%") ? ((Control)comboBoxImageQuality).Text : (((Control)comboBoxImageQuality).Text + "%"));
		StiSettings.Set(((Control)this).Name, "Scale", ((Control)comboBoxScale).Text.EndsWith("%") ? ((Control)comboBoxScale).Text : (((Control)comboBoxScale).Text + "%"));
		StiSettings.Set(((Control)this).Name, "EnableAnimation", checkBoxEnableAnimation.Checked);
		StiSettings.Set(((Control)this).Name, "OpenAfterExport", checkBoxOpenAfterExport.Checked);
		StiSettings.Save();
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpDefaultBrowser("user-manual/index.html?exports_dashboards.htm#htmlexportsettings");
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		SaveSettings();
		((Form)this).DialogResult = (DialogResult)1;
	}

	private void This_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			e.Handled = true;
			((Form)this).Close();
		}
	}

	private void This_Shown(object sender, EventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		((Control)this).Height = 0;
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val = item;
			if (val.Enabled)
			{
				((Control)this).Height = ((Control)this).Height + val.Height;
			}
			else
			{
				val.Visible = false;
			}
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
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0769: Unknown result type (might be due to invalid IL or missing references)
		//IL_0773: Expected O, but got Unknown
		groupLineButtons = new StiGroupLine();
		buttonOk = new Button();
		buttonCancel = new Button();
		checkBoxOpenAfterExport = new CheckBox();
		labelImageQuality = new Label();
		comboBoxImageQuality = new StiDropDownBox();
		labelScale = new Label();
		comboBoxScale = new StiDropDownBox();
		checkBoxEnableAnimation = new CheckBox();
		((Control)groupLineButtons).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)groupLineButtons).Controls.Add((Control)(object)buttonOk);
		((Control)groupLineButtons).Controls.Add((Control)(object)buttonCancel);
		((Control)groupLineButtons).Dock = (DockStyle)2;
		groupLineButtons.LinePosition = StiLinePosition.Top;
		groupLineButtons.LineStyle = StiLineStyle.Dot;
		((Control)groupLineButtons).Location = new Point(0, 144);
		((Control)groupLineButtons).Name = "groupLineButtons";
		((Control)groupLineButtons).Size = new Size(344, 46);
		((Control)groupLineButtons).TabIndex = 3;
		((Control)buttonOk).Anchor = (AnchorStyles)10;
		((Control)buttonOk).Location = new Point(175, 12);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(75, 24);
		((Control)buttonOk).TabIndex = 0;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)buttonOk).Click += This_OkClick;
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		buttonCancel.DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(258, 12);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(75, 24);
		((Control)buttonCancel).TabIndex = 1;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		checkBoxOpenAfterExport.Checked = true;
		checkBoxOpenAfterExport.CheckState = (CheckState)1;
		((Control)checkBoxOpenAfterExport).ForeColor = SystemColors.ControlText;
		((Control)checkBoxOpenAfterExport).Location = new Point(12, 111);
		((Control)checkBoxOpenAfterExport).Name = "checkBoxOpenAfterExport";
		((Control)checkBoxOpenAfterExport).Padding = new Padding(2, 0, 0, 0);
		((Control)checkBoxOpenAfterExport).Size = new Size(320, 22);
		((Control)checkBoxOpenAfterExport).TabIndex = 0;
		((Control)checkBoxOpenAfterExport).Text = "OpenAfterExport";
		((ButtonBase)checkBoxOpenAfterExport).UseVisualStyleBackColor = true;
		((Control)labelImageQuality).ForeColor = Color.DimGray;
		((Control)labelImageQuality).Location = new Point(12, 12);
		((Control)labelImageQuality).Name = "labelImageQuality";
		((Control)labelImageQuality).Size = new Size(157, 22);
		((Control)labelImageQuality).TabIndex = 0;
		((Control)labelImageQuality).Text = "ImageQuality";
		labelImageQuality.TextAlign = (ContentAlignment)16;
		((Control)comboBoxImageQuality).Anchor = (AnchorStyles)9;
		((Control)comboBoxImageQuality).BackColor = SystemColors.Window;
		comboBoxImageQuality.DrawMode = (DrawMode)2;
		((Control)comboBoxImageQuality).ForeColor = SystemColors.ControlText;
		comboBoxImageQuality.IsEditable = true;
		comboBoxImageQuality.Items.AddRange(new object[7] { "50%", "75%", "100%", "150%", "200%", "300%", "500%" });
		((Control)comboBoxImageQuality).Location = new Point(175, 12);
		((Control)comboBoxImageQuality).Name = "comboBoxImageQuality";
		comboBoxImageQuality.SelectedIndex = -1;
		comboBoxImageQuality.SelectedItem = null;
		comboBoxImageQuality.SelectedValue = null;
		((Control)comboBoxImageQuality).Size = new Size(75, 21);
		((Control)comboBoxImageQuality).TabIndex = 1;
		((Control)labelScale).ForeColor = Color.DimGray;
		((Control)labelScale).Location = new Point(12, 45);
		((Control)labelScale).Name = "labelScale";
		((Control)labelScale).Size = new Size(157, 22);
		((Control)labelScale).TabIndex = 5;
		((Control)labelScale).Text = "Scale";
		labelScale.TextAlign = (ContentAlignment)16;
		((Control)comboBoxScale).Anchor = (AnchorStyles)9;
		((Control)comboBoxScale).BackColor = SystemColors.Window;
		comboBoxScale.DrawMode = (DrawMode)2;
		((Control)comboBoxScale).ForeColor = SystemColors.ControlText;
		comboBoxScale.IsEditable = true;
		comboBoxScale.Items.AddRange(new object[7] { "25%", "50%", "75%", "100%", "150%", "200%", "300%" });
		((Control)comboBoxScale).Location = new Point(175, 45);
		((Control)comboBoxScale).Name = "comboBoxScale";
		comboBoxScale.SelectedIndex = -1;
		comboBoxScale.SelectedItem = null;
		comboBoxScale.SelectedValue = null;
		((Control)comboBoxScale).Size = new Size(75, 21);
		((Control)comboBoxScale).TabIndex = 4;
		checkBoxEnableAnimation.Checked = true;
		checkBoxEnableAnimation.CheckState = (CheckState)1;
		((Control)checkBoxEnableAnimation).ForeColor = SystemColors.ControlText;
		((Control)checkBoxEnableAnimation).Location = new Point(12, 78);
		((Control)checkBoxEnableAnimation).Name = "checkBoxEnableAnimation";
		((Control)checkBoxEnableAnimation).Padding = new Padding(2, 0, 0, 0);
		((Control)checkBoxEnableAnimation).Size = new Size(320, 22);
		((Control)checkBoxEnableAnimation).TabIndex = 6;
		((Control)checkBoxEnableAnimation).Text = "EnableAnimation";
		((ButtonBase)checkBoxEnableAnimation).UseVisualStyleBackColor = true;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(344, 190);
		((Control)this).Controls.Add((Control)(object)checkBoxEnableAnimation);
		((Control)this).Controls.Add((Control)(object)labelScale);
		((Control)this).Controls.Add((Control)(object)comboBoxScale);
		((Control)this).Controls.Add((Control)(object)checkBoxOpenAfterExport);
		((Control)this).Controls.Add((Control)(object)labelImageQuality);
		((Control)this).Controls.Add((Control)(object)comboBoxImageQuality);
		((Control)this).Controls.Add((Control)(object)groupLineButtons);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiDashboardHtmlExportSetupForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiExcelExportSetupForm";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).Shown += This_Shown;
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)groupLineButtons).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
