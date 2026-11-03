using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Report.Export.Win;

public class StiCsvExportSetupForm : Form
{
	private IContainer components;

	private Button btCancel;

	private Button btOk;

	private StiPagesRangeControl PagesRangeControl;

	private CheckBox cbOpenAfterExport;

	private ComboBox cbEncoding;

	private TextBox tbSeparator;

	private CheckBox checkBoxSkipColumnHeaders;

	private StiGroupBox gbExportSettings;

	private Label lbEncoding;

	private Label lbSeparator;

	private StiGroupLine glButtons;

	private Label lbDataExportMode;

	private ComboBox cbDataExportMode;

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

	public Encoding Encoding => StiEncodingHelper.GetEncodingFromName(cbEncoding.SelectedItem as string);

	public string Separator => ((Control)tbSeparator).Text;

	public bool SkipColumnHeaders => checkBoxSkipColumnHeaders.Checked;

	public int DataExportMode => ((ListControl)cbDataExportMode).SelectedIndex;

	public StiCsvExportSetupForm()
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
		Localize();
		((Control)tbSeparator).Text = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
		cbEncoding.Items.Clear();
		ObjectCollection items = cbEncoding.Items;
		object[] encodingNames = StiEncodingHelper.GetEncodingNames();
		items.AddRange(encodingNames);
		cbEncoding.SelectedItem = Encoding.Default.EncodingName;
		((ListControl)cbDataExportMode).SelectedIndex = 0;
		StiSettings.Load();
		cbOpenAfterExport.Checked = StiSettings.GetBool(((Control)this).Name, "OpenAfterExport", cbOpenAfterExport.Checked);
		gbExportSettings.Collapsed = StiSettings.GetBool(((Control)this).Name, "ExportSettings", gbExportSettings.Collapsed);
		((Control)tbSeparator).Text = StiSettings.GetStr(((Control)this).Name, "Separator", ((Control)tbSeparator).Text);
		checkBoxSkipColumnHeaders.Checked = StiSettings.GetBool(((Control)this).Name, "SkipColumnHeaders", checkBoxSkipColumnHeaders.Checked);
		int @int = StiSettings.GetInt(((Control)this).Name, "Encoding", 0);
		if (@int < cbEncoding.Items.Count)
		{
			((ListControl)cbEncoding).SelectedIndex = @int;
		}
		((ListControl)cbDataExportMode).SelectedIndex = StiSettings.GetInt(((Control)this).Name, "DataExportMode", 0);
		cbEncoding.DropDownWidth = StiScale.XXI((double)cbEncoding.DropDownWidth);
	}

	private void Localize()
	{
		((Control)gbExportSettings).Text = StiLocalization.Get("Export", "Settings");
		((Control)cbOpenAfterExport).Text = StiLocalization.Get("Export", "OpenAfterExport");
		((Control)checkBoxSkipColumnHeaders).Text = StiLocalization.Get("Export", "SkipColumnHeaders");
		((Control)lbEncoding).Text = StiLocalization.Get("Export", "Encoding");
		((Control)lbSeparator).Text = StiLocalization.Get("Export", "Separator");
		((Control)lbDataExportMode).Text = StiLocalization.Get("Export", "BandsFilter");
		cbDataExportMode.Items.Clear();
		cbDataExportMode.Items.Add((object)StiLocalization.Get("Export", "DataOnly"));
		cbDataExportMode.Items.Add((object)StiLocalization.Get("Export", "DataAndHeadersFooters"));
		cbDataExportMode.Items.Add((object)StiLocalization.Get("Export", "AllBands"));
		((Control)this).Text = StiLocalization.Get("Export", "title");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
	}

	private void ThisClosed(object sender, EventArgs e)
	{
		StiSettings.Load();
		StiSettings.Set(((Control)this).Name, "OpenAfterExport", cbOpenAfterExport.Checked);
		StiSettings.Set(((Control)this).Name, "ExportSettings", gbExportSettings.Collapsed);
		StiSettings.Set(((Control)this).Name, "Separator", ((Control)tbSeparator).Text);
		StiSettings.Set(((Control)this).Name, "Encoding", ((ListControl)cbEncoding).SelectedIndex);
		StiSettings.Set(((Control)this).Name, "SkipColumnHeaders", checkBoxSkipColumnHeaders.Checked);
		StiSettings.Set(((Control)this).Name, "DataExportMode", ((ListControl)cbDataExportMode).SelectedIndex);
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
		StiHelpProvider.ShowHelpViewer("user-manual/exports_data_csv.htm");
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
		//IL_08a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ac: Expected O, but got Unknown
		cbOpenAfterExport = new CheckBox();
		btOk = new Button();
		btCancel = new Button();
		glButtons = new StiGroupLine();
		gbExportSettings = new StiGroupBox();
		lbDataExportMode = new Label();
		cbDataExportMode = new ComboBox();
		checkBoxSkipColumnHeaders = new CheckBox();
		tbSeparator = new TextBox();
		lbSeparator = new Label();
		lbEncoding = new Label();
		cbEncoding = new ComboBox();
		PagesRangeControl = new StiPagesRangeControl();
		((Control)glButtons).SuspendLayout();
		((Control)gbExportSettings).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)cbOpenAfterExport).Anchor = (AnchorStyles)6;
		((Control)cbOpenAfterExport).AutoSize = true;
		((Control)cbOpenAfterExport).Location = new Point(12, 273);
		((Control)cbOpenAfterExport).Name = "cbOpenAfterExport";
		((Control)cbOpenAfterExport).Size = new Size(104, 17);
		((Control)cbOpenAfterExport).TabIndex = 2;
		((Control)cbOpenAfterExport).Text = "OpenAfterExport";
		((Control)btOk).Anchor = (AnchorStyles)10;
		btOk.DialogResult = (DialogResult)1;
		((Control)btOk).Location = new Point(160, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 0;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		btCancel.DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(240, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 1;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)glButtons).BackColor = Color.White;
		((Control)glButtons).Controls.Add((Control)(object)btCancel);
		((Control)glButtons).Controls.Add((Control)(object)btOk);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = (StiLinePosition)0;
		glButtons.LineStyle = (StiLineStyle)0;
		((Control)glButtons).Location = new Point(0, 296);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(324, 46);
		((Control)glButtons).TabIndex = 3;
		gbExportSettings.AllowCollapse = true;
		((Control)gbExportSettings).Controls.Add((Control)(object)lbDataExportMode);
		((Control)gbExportSettings).Controls.Add((Control)(object)cbDataExportMode);
		((Control)gbExportSettings).Controls.Add((Control)(object)checkBoxSkipColumnHeaders);
		((Control)gbExportSettings).Controls.Add((Control)(object)tbSeparator);
		((Control)gbExportSettings).Controls.Add((Control)(object)lbSeparator);
		((Control)gbExportSettings).Controls.Add((Control)(object)lbEncoding);
		((Control)gbExportSettings).Controls.Add((Control)(object)cbEncoding);
		((Control)gbExportSettings).Location = new Point(4, 142);
		((Control)gbExportSettings).Name = "gbExportSettings";
		gbExportSettings.ResHeight = 122;
		((Control)gbExportSettings).Size = new Size(316, 122);
		((Control)gbExportSettings).TabIndex = 1;
		((GroupBox)gbExportSettings).TabStop = false;
		((Control)gbExportSettings).Text = "ExportSettings";
		((Control)lbDataExportMode).AutoSize = true;
		((Control)lbDataExportMode).ForeColor = SystemColors.ControlText;
		((Control)lbDataExportMode).Location = new Point(5, 71);
		((Control)lbDataExportMode).Name = "lbDataExportMode";
		((Control)lbDataExportMode).Size = new Size(59, 13);
		((Control)lbDataExportMode).TabIndex = 5;
		((Control)lbDataExportMode).Text = "Bands filter";
		lbDataExportMode.TextAlign = (ContentAlignment)16;
		((Control)cbDataExportMode).BackColor = SystemColors.Window;
		cbDataExportMode.DropDownStyle = (ComboBoxStyle)2;
		cbDataExportMode.DropDownWidth = 168;
		((Control)cbDataExportMode).ForeColor = SystemColors.ControlText;
		cbDataExportMode.Items.AddRange(new object[3] { "Data only", "Data and Headers/Footers", "All bands" });
		((Control)cbDataExportMode).Location = new Point(128, 68);
		((Control)cbDataExportMode).Name = "cbDataExportMode";
		((Control)cbDataExportMode).Size = new Size(168, 21);
		((Control)cbDataExportMode).TabIndex = 2;
		((Control)checkBoxSkipColumnHeaders).AutoSize = true;
		((Control)checkBoxSkipColumnHeaders).Location = new Point(8, 98);
		((Control)checkBoxSkipColumnHeaders).Name = "checkBoxSkipColumnHeaders";
		((Control)checkBoxSkipColumnHeaders).Size = new Size(128, 17);
		((Control)checkBoxSkipColumnHeaders).TabIndex = 3;
		((Control)checkBoxSkipColumnHeaders).Text = "Skip Column Headers";
		((Control)tbSeparator).ForeColor = SystemColors.ControlText;
		((Control)tbSeparator).Location = new Point(128, 42);
		((Control)tbSeparator).Name = "tbSeparator";
		((Control)tbSeparator).Size = new Size(48, 20);
		((Control)tbSeparator).TabIndex = 1;
		((Control)tbSeparator).Text = ",";
		((Control)lbSeparator).AutoSize = true;
		((Control)lbSeparator).ForeColor = SystemColors.ControlText;
		((Control)lbSeparator).Location = new Point(5, 45);
		((Control)lbSeparator).Name = "lbSeparator";
		((Control)lbSeparator).Size = new Size(53, 13);
		((Control)lbSeparator).TabIndex = 2;
		((Control)lbSeparator).Text = "Separator";
		lbSeparator.TextAlign = (ContentAlignment)16;
		((Control)lbEncoding).AutoSize = true;
		((Control)lbEncoding).ForeColor = SystemColors.ControlText;
		((Control)lbEncoding).Location = new Point(5, 19);
		((Control)lbEncoding).Name = "lbEncoding";
		((Control)lbEncoding).Size = new Size(52, 13);
		((Control)lbEncoding).TabIndex = 0;
		((Control)lbEncoding).Text = "Encoding";
		lbEncoding.TextAlign = (ContentAlignment)16;
		((Control)cbEncoding).BackColor = SystemColors.Window;
		cbEncoding.DropDownStyle = (ComboBoxStyle)2;
		((Control)cbEncoding).ForeColor = SystemColors.ControlText;
		((Control)cbEncoding).Location = new Point(128, 16);
		((Control)cbEncoding).Name = "cbEncoding";
		((Control)cbEncoding).Size = new Size(168, 21);
		((Control)cbEncoding).TabIndex = 0;
		((Control)PagesRangeControl).BackColor = Color.White;
		PagesRangeControl.CurrentPage = 0;
		((Control)PagesRangeControl).Location = new Point(4, 4);
		((Control)PagesRangeControl).Name = "PagesRangeControl";
		((Control)PagesRangeControl).Size = new Size(316, 136);
		((Control)PagesRangeControl).TabIndex = 0;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(324, 342);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Control)this).Controls.Add((Control)(object)gbExportSettings);
		((Control)this).Controls.Add((Control)(object)PagesRangeControl);
		((Control)this).Controls.Add((Control)(object)cbOpenAfterExport);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiCsvExportSetupForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiCsvExportSetupForm";
		((Form)this).HelpButtonClicked += ThisHelpButtonClicked;
		((Form)this).Closed += ThisClosed;
		((Control)this).KeyDown += new KeyEventHandler(ThisKeyDown);
		((Control)glButtons).ResumeLayout(false);
		((Control)gbExportSettings).ResumeLayout(false);
		((Control)gbExportSettings).PerformLayout();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
