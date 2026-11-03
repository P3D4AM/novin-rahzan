using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Report.Export.Win;

public class StiHtmlExportSetupForm : Form
{
	private StiExportFormat exportFormat = StiExportFormat.Html;

	private IContainer components;

	private ComboBox cbZoom;

	private ComboBox cbImageFormat;

	private StiPagesRangeControl PagesRangeControl;

	private CheckBox cbOpenAfterExport;

	private ComboBox cbExportMode;

	private CheckBox checkBoxAddPageBreaks;

	private Button btCancel;

	private Button btOk;

	private Label lbImageFormat;

	private Label lbZoom;

	private StiGroupBox gbExportSettings;

	private Label lbExportMode;

	private StiGroupLine glButtons;

	private StiGroupBox groupBox1;

	public StiExportFormat ExportFormat
	{
		set
		{
			exportFormat = value;
		}
	}

	public StiPagesRange PagesRange => PagesRangeControl.GetPagesRange();

	public bool OpenAfterExport => cbOpenAfterExport.Checked;

	public int CurrentPage
	{
		get
		{
			return PagesRangeControl.CurrentPage;
		}
		set
		{
			PagesRangeControl.CurrentPage = value;
		}
	}

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

	public bool AddPageBreaks => checkBoxAddPageBreaks.Checked;

	public float Zoom
	{
		get
		{
			float num = 100f;
			try
			{
				string text = ((Control)cbZoom).Text;
				if (text[text.Length - 1] == '%')
				{
					text = text.Substring(0, text.Length - 1);
				}
				num = float.Parse(text);
			}
			catch
			{
			}
			return num / 100f;
		}
	}

	public ImageFormat ImageFormat => (ImageFormat)(((Control)cbImageFormat).Text switch
	{
		"Bmp" => ImageFormat.Bmp, 
		"Gif" => ImageFormat.Gif, 
		"Png" => ImageFormat.Png, 
		_ => ImageFormat.Jpeg, 
	});

	public StiHtmlExportMode ExportMode
	{
		get
		{
			if (((ListControl)cbExportMode).SelectedIndex == 1)
			{
				return StiHtmlExportMode.Span;
			}
			if (((ListControl)cbExportMode).SelectedIndex == 2)
			{
				return StiHtmlExportMode.Div;
			}
			return StiHtmlExportMode.Table;
		}
	}

	public StiHtmlExportSetupForm()
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
		((Control)cbImageFormat).Text = "Jpeg";
		Localize();
		StiSettings.Load();
		cbOpenAfterExport.Checked = StiSettings.GetBool(((Control)this).Name, "OpenAfterExport", cbOpenAfterExport.Checked);
		checkBoxAddPageBreaks.Checked = StiSettings.GetBool(((Control)this).Name, "AddPageBreaks", checkBoxAddPageBreaks.Checked);
		gbExportSettings.Collapsed = StiSettings.GetBool(((Control)this).Name, "ExportSettings", gbExportSettings.Collapsed);
		((Control)cbZoom).Handle.ToString();
		((Control)cbZoom).Text = StiSettings.GetStr(((Control)this).Name, "Zoom", ((Control)cbZoom).Text);
		cbImageFormat.SelectedItem = StiSettings.GetStr(((Control)this).Name, "ImageFormat", ((Control)cbImageFormat).Text);
		cbExportMode.SelectedItem = StiSettings.GetStr(((Control)this).Name, "ExportMode", ((Control)cbExportMode).Text);
	}

	private void Localize()
	{
		((Control)lbImageFormat).Text = StiLocalization.Get("Export", "ImageFormat");
		((Control)lbZoom).Text = StiLocalization.Get("Export", "Zoom");
		((Control)gbExportSettings).Text = StiLocalization.Get("Export", "Settings");
		((Control)cbOpenAfterExport).Text = StiLocalization.Get("Export", "OpenAfterExport");
		((Control)lbExportMode).Text = StiLocalization.Get("Export", "ExportMode");
		((Control)checkBoxAddPageBreaks).Text = StiLocalization.Get("Export", "AddPageBreaks");
		((ListControl)cbExportMode).SelectedIndex = 0;
		((Control)this).Text = StiLocalization.Get("Export", "title");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
	}

	private void ThisClosed(object sender, EventArgs e)
	{
		StiSettings.Load();
		StiSettings.Set(((Control)this).Name, "OpenAfterExport", cbOpenAfterExport.Checked);
		StiSettings.Set(((Control)this).Name, "ExportSettings", gbExportSettings.Collapsed);
		StiSettings.Set(((Control)this).Name, "AddPageBreaks", checkBoxAddPageBreaks.Checked);
		StiSettings.Set(((Control)this).Name, "Zoom", ((Control)cbZoom).Text);
		StiSettings.Set(((Control)this).Name, "ImageFormat", cbImageFormat.SelectedItem);
		StiSettings.Set(((Control)this).Name, "ExportMode", cbExportMode.SelectedItem);
		StiSettings.Save();
	}

	private void ThisKeyDown(object sender, KeyEventArgs e)
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
		string url = ((exportFormat == StiExportFormat.Html) ? "online/user-manual/exports_web_documents_html.htm" : "user-manual/exports_web_documents_mht.htm");
		StiHelpProvider.ShowHelpViewer(url);
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
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_09d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09dc: Expected O, but got Unknown
		btCancel = new Button();
		btOk = new Button();
		lbZoom = new Label();
		cbZoom = new ComboBox();
		cbImageFormat = new ComboBox();
		lbImageFormat = new Label();
		gbExportSettings = new StiGroupBox();
		groupBox1 = new StiGroupBox();
		cbExportMode = new ComboBox();
		lbExportMode = new Label();
		checkBoxAddPageBreaks = new CheckBox();
		cbOpenAfterExport = new CheckBox();
		glButtons = new StiGroupLine();
		PagesRangeControl = new StiPagesRangeControl();
		((Control)gbExportSettings).SuspendLayout();
		((Control)groupBox1).SuspendLayout();
		((Control)glButtons).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)btCancel).Anchor = (AnchorStyles)6;
		btCancel.DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(232, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 1;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btOk).Anchor = (AnchorStyles)6;
		btOk.DialogResult = (DialogResult)1;
		((Control)btOk).Location = new Point(152, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 0;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)lbZoom).AutoSize = true;
		((Control)lbZoom).ForeColor = SystemColors.ControlText;
		((Control)lbZoom).Location = new Point(5, 14);
		((Control)lbZoom).Name = "lbZoom";
		((Control)lbZoom).Size = new Size(34, 13);
		((Control)lbZoom).TabIndex = 5;
		((Control)lbZoom).Text = "Zoom";
		lbZoom.TextAlign = (ContentAlignment)16;
		((Control)cbZoom).BackColor = SystemColors.Window;
		((Control)cbZoom).ForeColor = SystemColors.ControlText;
		cbZoom.Items.AddRange(new object[7] { "25%", "50%", "75%", "100%", "125%", "150%", "200%" });
		((Control)cbZoom).Location = new Point(168, 11);
		((Control)cbZoom).Name = "cbZoom";
		((Control)cbZoom).Size = new Size(88, 21);
		((Control)cbZoom).TabIndex = 0;
		((Control)cbZoom).Text = "100%";
		((Control)cbImageFormat).BackColor = SystemColors.Window;
		((ListControl)cbImageFormat).DisplayMember = "Jpeg";
		cbImageFormat.DropDownStyle = (ComboBoxStyle)2;
		((Control)cbImageFormat).ForeColor = SystemColors.ControlText;
		cbImageFormat.Items.AddRange(new object[4] { "Jpeg", "Gif", "Bmp", "Png" });
		((Control)cbImageFormat).Location = new Point(168, 35);
		((Control)cbImageFormat).Name = "cbImageFormat";
		((Control)cbImageFormat).Size = new Size(88, 21);
		((Control)cbImageFormat).TabIndex = 1;
		((Control)lbImageFormat).AutoSize = true;
		((Control)lbImageFormat).ForeColor = SystemColors.ControlText;
		((Control)lbImageFormat).Location = new Point(5, 38);
		((Control)lbImageFormat).Name = "lbImageFormat";
		((Control)lbImageFormat).Size = new Size(68, 13);
		((Control)lbImageFormat).TabIndex = 6;
		((Control)lbImageFormat).Text = "ImageFormat";
		lbImageFormat.TextAlign = (ContentAlignment)16;
		gbExportSettings.AllowCollapse = true;
		((Control)gbExportSettings).Controls.Add((Control)(object)groupBox1);
		((Control)gbExportSettings).Controls.Add((Control)(object)checkBoxAddPageBreaks);
		((Control)gbExportSettings).Location = new Point(4, 142);
		((Control)gbExportSettings).Name = "gbExportSettings";
		gbExportSettings.ResHeight = 141;
		((Control)gbExportSettings).Size = new Size(304, 140);
		((Control)gbExportSettings).TabIndex = 1;
		((GroupBox)gbExportSettings).TabStop = false;
		((Control)gbExportSettings).Text = "ExportSettings";
		((Control)groupBox1).Controls.Add((Control)(object)cbExportMode);
		((Control)groupBox1).Controls.Add((Control)(object)lbZoom);
		((Control)groupBox1).Controls.Add((Control)(object)lbImageFormat);
		((Control)groupBox1).Controls.Add((Control)(object)lbExportMode);
		((Control)groupBox1).Controls.Add((Control)(object)cbZoom);
		((Control)groupBox1).Controls.Add((Control)(object)cbImageFormat);
		((Control)groupBox1).Location = new Point(8, 17);
		((Control)groupBox1).Name = "groupBox1";
		groupBox1.ResHeight = 0;
		((Control)groupBox1).Size = new Size(288, 91);
		((Control)groupBox1).TabIndex = 0;
		((GroupBox)groupBox1).TabStop = false;
		((Control)cbExportMode).BackColor = SystemColors.Window;
		((ListControl)cbExportMode).DisplayMember = "Jpeg";
		cbExportMode.DropDownStyle = (ComboBoxStyle)2;
		((Control)cbExportMode).ForeColor = SystemColors.ControlText;
		cbExportMode.Items.AddRange(new object[3] { "Table", "Span", "Div" });
		((Control)cbExportMode).Location = new Point(168, 59);
		((Control)cbExportMode).Name = "cbExportMode";
		((Control)cbExportMode).Size = new Size(88, 21);
		((Control)cbExportMode).TabIndex = 2;
		((Control)lbExportMode).AutoSize = true;
		((Control)lbExportMode).ForeColor = SystemColors.ControlText;
		((Control)lbExportMode).Location = new Point(5, 62);
		((Control)lbExportMode).Name = "lbExportMode";
		((Control)lbExportMode).Size = new Size(64, 13);
		((Control)lbExportMode).TabIndex = 6;
		((Control)lbExportMode).Text = "ExportMode";
		lbExportMode.TextAlign = (ContentAlignment)16;
		((Control)checkBoxAddPageBreaks).Anchor = (AnchorStyles)13;
		((Control)checkBoxAddPageBreaks).Location = new Point(8, 116);
		((Control)checkBoxAddPageBreaks).Name = "checkBoxAddPageBreaks";
		((Control)checkBoxAddPageBreaks).Size = new Size(288, 17);
		((Control)checkBoxAddPageBreaks).TabIndex = 1;
		((Control)checkBoxAddPageBreaks).Text = "Add Page Breaks";
		((Control)cbOpenAfterExport).Anchor = (AnchorStyles)6;
		((Control)cbOpenAfterExport).BackColor = Color.Transparent;
		((Control)cbOpenAfterExport).Location = new Point(12, 296);
		((Control)cbOpenAfterExport).Name = "cbOpenAfterExport";
		((Control)cbOpenAfterExport).Size = new Size(296, 17);
		((Control)cbOpenAfterExport).TabIndex = 2;
		((Control)cbOpenAfterExport).Text = "OpenAfterExport";
		((ButtonBase)cbOpenAfterExport).UseVisualStyleBackColor = false;
		((Control)glButtons).Controls.Add((Control)(object)btOk);
		((Control)glButtons).Controls.Add((Control)(object)btCancel);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = (StiLinePosition)0;
		glButtons.LineStyle = (StiLineStyle)0;
		((Control)glButtons).Location = new Point(0, 320);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(312, 46);
		((Control)glButtons).TabIndex = 3;
		((Control)PagesRangeControl).BackColor = Color.White;
		PagesRangeControl.CurrentPage = 0;
		((Control)PagesRangeControl).Location = new Point(4, 4);
		((Control)PagesRangeControl).Name = "PagesRangeControl";
		((Control)PagesRangeControl).Size = new Size(304, 136);
		((Control)PagesRangeControl).TabIndex = 0;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(312, 366);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Control)this).Controls.Add((Control)(object)PagesRangeControl);
		((Control)this).Controls.Add((Control)(object)gbExportSettings);
		((Control)this).Controls.Add((Control)(object)cbOpenAfterExport);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiHtmlExportSetupForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiHtmlExportSetupForm";
		((Form)this).HelpButtonClicked += ThisHelpButtonClicked;
		((Form)this).Closed += ThisClosed;
		((Control)this).KeyDown += new KeyEventHandler(ThisKeyDown);
		((Control)gbExportSettings).ResumeLayout(false);
		((Control)groupBox1).ResumeLayout(false);
		((Control)groupBox1).PerformLayout();
		((Control)glButtons).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
