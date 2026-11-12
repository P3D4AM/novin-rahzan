using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Dashboard.Export.Settings;
using Stimulsoft.Data.Exceptions;
using Stimulsoft.Report;
using Stimulsoft.Report.Export;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Dashboard.Viewer.Export;

public class StiDashboardImageExportSetupForm : Form
{
	private IContainer components;

	private StiGroupLine glButtons;

	private Button buttonOk;

	private Button buttonCancel;

	private CheckBox checkBoxOpenAfterExport;

	private Label labelScale;

	private StiDropDownBox comboBoxScale;

	private StiComboBox comboBoxImageType;

	private Label labelImageType;

	public StiDashboardImageExportSetupForm()
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
			UpdateState();
		}
	}

	private void Localize()
	{
		((Control)this).Text = Loc.Get("Export", "ExportTypeImageFile").Replace("...", "") + " - " + Loc.Get("Export", "title").Replace("&", "_");
		((Control)labelImageType).Text = Loc.Get("Export", "ImageType");
		((Control)labelScale).Text = Loc.Get("PropertyMain", "Scale");
		((Control)checkBoxOpenAfterExport).Text = Loc.Get("Export", "OpenAfterExport");
		((Control)buttonOk).Text = Loc.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = Loc.Get("Buttons", "Cancel");
		((ComboBox)comboBoxImageType).Items.Clear();
		if (StiOptions.Viewer.Windows.Exports.ShowBmp)
		{
			((ComboBox)comboBoxImageType).Items.Add((object)"BMP");
		}
		if (StiOptions.Viewer.Windows.Exports.ShowGif)
		{
			((ComboBox)comboBoxImageType).Items.Add((object)"GIF");
		}
		if (StiOptions.Viewer.Windows.Exports.ShowPng)
		{
			((ComboBox)comboBoxImageType).Items.Add((object)"PNG");
		}
		if (StiOptions.Viewer.Windows.Exports.ShowTiff)
		{
			((ComboBox)comboBoxImageType).Items.Add((object)"TIFF");
		}
		if (StiOptions.Viewer.Windows.Exports.ShowJpeg)
		{
			((ComboBox)comboBoxImageType).Items.Add((object)"JPEG");
		}
		if (StiOptions.Viewer.Windows.Exports.ShowPcx)
		{
			((ComboBox)comboBoxImageType).Items.Add((object)"PCX");
		}
		if (StiOptions.Viewer.Windows.Exports.ShowMetafile)
		{
			((ComboBox)comboBoxImageType).Items.Add((object)"EMF");
		}
		if (StiOptions.Viewer.Windows.Exports.ShowSvg)
		{
			((ComboBox)comboBoxImageType).Items.Add((object)"SVG");
		}
		if (StiOptions.Viewer.Windows.Exports.ShowSvgz)
		{
			((ComboBox)comboBoxImageType).Items.Add((object)"SVGZ");
		}
	}

	private void UpdateState()
	{
		((Control)comboBoxScale).Enabled = ((Control)comboBoxImageType).Text != "SVG" && ((Control)comboBoxImageType).Text != "SVGZ";
	}

	public StiDashboardExportSettings GetExportSettings()
	{
		int.TryParse(((Control)comboBoxScale).Text.Replace("%", ""), out var result);
		return new StiImageDashboardExportSettings(GetImageType())
		{
			ImageType = GetImageType(),
			OpenAfterExport = checkBoxOpenAfterExport.Checked,
			Scale = result
		};
	}

	private StiImageType GetImageType()
	{
		return ((Control)comboBoxImageType).Text switch
		{
			"BMP" => StiImageType.Bmp, 
			"GIF" => StiImageType.Gif, 
			"PNG" => StiImageType.Png, 
			"TIFF" => StiImageType.Tiff, 
			"JPEG" => StiImageType.Jpeg, 
			"PCX" => StiImageType.Pcx, 
			"EMF" => StiImageType.Emf, 
			"SVG" => StiImageType.Svg, 
			"SVGZ" => StiImageType.Svgz, 
			_ => throw new StiTypeNotRecognizedException(((Control)comboBoxImageType).Text), 
		};
	}

	private void LoadSettings()
	{
		StiSettings.Load();
		((Control)comboBoxImageType).Text = StiSettings.GetStr(((Control)this).Name, "ImageType", "PNG");
		((Control)comboBoxScale).Text = StiSettings.GetStr(((Control)this).Name, "Scale", "100%");
		checkBoxOpenAfterExport.Checked = StiSettings.GetBool(((Control)this).Name, "OpenAfterExport", nullValue: true);
	}

	private void SaveSettings()
	{
		StiSettings.Load();
		StiSettings.Set(((Control)this).Name, "ImageType", ((Control)comboBoxImageType).Text);
		StiSettings.Set(((Control)this).Name, "Scale", ((Control)comboBoxScale).Text.EndsWith("%") ? ((Control)comboBoxScale).Text : (((Control)comboBoxScale).Text + "%"));
		StiSettings.Set(((Control)this).Name, "OpenAfterExport", checkBoxOpenAfterExport.Checked);
		StiSettings.Save();
	}

	private void ComboBoxImageType_SelectedIndexChanged(object sender, EventArgs e)
	{
		UpdateState();
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpDefaultBrowser("user-manual/index.html?exports_dashboards.htm#imageexportsettings");
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
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ab: Expected O, but got Unknown
		glButtons = new StiGroupLine();
		buttonOk = new Button();
		buttonCancel = new Button();
		checkBoxOpenAfterExport = new CheckBox();
		labelScale = new Label();
		comboBoxScale = new StiDropDownBox();
		comboBoxImageType = new StiComboBox();
		labelImageType = new Label();
		((Control)glButtons).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)glButtons).Controls.Add((Control)(object)buttonOk);
		((Control)glButtons).Controls.Add((Control)(object)buttonCancel);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = StiLinePosition.Top;
		glButtons.LineStyle = StiLineStyle.Dot;
		((Control)glButtons).Location = new Point(0, 111);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(344, 46);
		((Control)glButtons).TabIndex = 5;
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
		((Control)checkBoxOpenAfterExport).Location = new Point(12, 78);
		((Control)checkBoxOpenAfterExport).Name = "checkBoxOpenAfterExport";
		((Control)checkBoxOpenAfterExport).Padding = new Padding(2, 0, 0, 0);
		((Control)checkBoxOpenAfterExport).Size = new Size(320, 22);
		((Control)checkBoxOpenAfterExport).TabIndex = 4;
		((Control)checkBoxOpenAfterExport).Text = "OpenAfterExport";
		((ButtonBase)checkBoxOpenAfterExport).UseVisualStyleBackColor = true;
		((Control)labelScale).ForeColor = Color.DimGray;
		((Control)labelScale).Location = new Point(12, 45);
		((Control)labelScale).Name = "labelScale";
		((Control)labelScale).Size = new Size(157, 22);
		((Control)labelScale).TabIndex = 3;
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
		((Control)comboBoxScale).TabIndex = 2;
		((Control)comboBoxImageType).Anchor = (AnchorStyles)9;
		((Control)comboBoxImageType).BackColor = SystemColors.Window;
		comboBoxImageType.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxImageType).ForeColor = SystemColors.ControlText;
		((ComboBox)comboBoxImageType).Items.AddRange(new object[9] { "BMP", "GIF", "PNG", "TIFF", "JPEG", "PCX", "EMF", "SVG", "SVGZ" });
		((Control)comboBoxImageType).Location = new Point(175, 12);
		((ComboBox)comboBoxImageType).MaxDropDownItems = 20;
		((Control)comboBoxImageType).Name = "comboBoxImageType";
		((Control)comboBoxImageType).Size = new Size(160, 21);
		((Control)comboBoxImageType).TabIndex = 0;
		((ComboBox)comboBoxImageType).SelectedIndexChanged += ComboBoxImageType_SelectedIndexChanged;
		((Control)labelImageType).ForeColor = Color.DimGray;
		((Control)labelImageType).Location = new Point(12, 12);
		((Control)labelImageType).Name = "labelImageType";
		((Control)labelImageType).Size = new Size(157, 22);
		((Control)labelImageType).TabIndex = 1;
		((Control)labelImageType).Text = "ImageType";
		labelImageType.TextAlign = (ContentAlignment)16;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(344, 157);
		((Control)this).Controls.Add((Control)(object)checkBoxOpenAfterExport);
		((Control)this).Controls.Add((Control)(object)labelScale);
		((Control)this).Controls.Add((Control)(object)comboBoxImageType);
		((Control)this).Controls.Add((Control)(object)comboBoxScale);
		((Control)this).Controls.Add((Control)(object)labelImageType);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiDashboardImageExportSetupForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiImageExportSetupForm";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)glButtons).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
