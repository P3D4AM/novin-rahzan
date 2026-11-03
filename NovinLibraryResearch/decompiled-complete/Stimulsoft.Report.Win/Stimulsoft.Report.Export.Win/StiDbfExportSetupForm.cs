using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Report.Export.Win;

public class StiDbfExportSetupForm : Form
{
	private IContainer components;

	private Button btCancel;

	private Button btOk;

	private StiPagesRangeControl PagesRangeControl;

	private StiGroupBox gbExportSettings;

	private CheckBox cbOpenAfterExport;

	private Label lbEncoding;

	private ComboBox cbEncoding;

	private StiGroupLine glButtons;

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

	public int EncodingSelectedIndex => ((ListControl)cbEncoding).SelectedIndex;

	public StiDbfExportSetupForm()
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
		cbEncoding.Items.Clear();
		ObjectCollection items = cbEncoding.Items;
		object[] copePageNames = StiDbfExportService.copePageNames;
		items.AddRange(copePageNames);
		((ListControl)cbEncoding).SelectedIndex = 0;
		StiSettings.Load();
		cbOpenAfterExport.Checked = StiSettings.GetBool(((Control)this).Name, "OpenAfterExport", cbOpenAfterExport.Checked);
		gbExportSettings.Collapsed = StiSettings.GetBool(((Control)this).Name, "ExportSettings", gbExportSettings.Collapsed);
		int @int = StiSettings.GetInt(((Control)this).Name, "Encoding", 0);
		if (@int < cbEncoding.Items.Count)
		{
			((ListControl)cbEncoding).SelectedIndex = @int;
		}
		cbEncoding.DropDownWidth = StiScale.XXI((double)cbEncoding.DropDownWidth);
	}

	private void Localize()
	{
		((Control)gbExportSettings).Text = StiLocalization.Get("Export", "Settings");
		((Control)cbOpenAfterExport).Text = StiLocalization.Get("Export", "OpenAfterExport");
		((Control)lbEncoding).Text = StiLocalization.Get("Export", "Encoding");
		((Control)this).Text = StiLocalization.Get("Export", "title");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
	}

	private void ThisClosed(object sender, EventArgs e)
	{
		StiSettings.Load();
		StiSettings.Set(((Control)this).Name, "OpenAfterExport", cbOpenAfterExport.Checked);
		StiSettings.Set(((Control)this).Name, "ExportSettings", gbExportSettings.Collapsed);
		StiSettings.Set(((Control)this).Name, "Encoding", ((ListControl)cbEncoding).SelectedIndex);
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
		StiHelpProvider.ShowHelpViewer("user-manual/exports_data_dbf.htm");
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
		//IL_05a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05af: Expected O, but got Unknown
		btCancel = new Button();
		btOk = new Button();
		PagesRangeControl = new StiPagesRangeControl();
		gbExportSettings = new StiGroupBox();
		lbEncoding = new Label();
		cbEncoding = new ComboBox();
		cbOpenAfterExport = new CheckBox();
		glButtons = new StiGroupLine();
		((Control)gbExportSettings).SuspendLayout();
		((Control)glButtons).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)btCancel).Anchor = (AnchorStyles)10;
		btCancel.DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(240, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 2;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btOk).Anchor = (AnchorStyles)10;
		btOk.DialogResult = (DialogResult)1;
		((Control)btOk).Location = new Point(160, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 1;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)PagesRangeControl).BackColor = Color.White;
		PagesRangeControl.CurrentPage = 0;
		((Control)PagesRangeControl).Location = new Point(4, 4);
		((Control)PagesRangeControl).Name = "PagesRangeControl";
		((Control)PagesRangeControl).Size = new Size(316, 136);
		((Control)PagesRangeControl).TabIndex = 0;
		gbExportSettings.AllowCollapse = true;
		((Control)gbExportSettings).Controls.Add((Control)(object)lbEncoding);
		((Control)gbExportSettings).Controls.Add((Control)(object)cbEncoding);
		((Control)gbExportSettings).Location = new Point(4, 142);
		((Control)gbExportSettings).Name = "gbExportSettings";
		gbExportSettings.ResHeight = 50;
		((Control)gbExportSettings).Size = new Size(316, 50);
		((Control)gbExportSettings).TabIndex = 1;
		((GroupBox)gbExportSettings).TabStop = false;
		((Control)gbExportSettings).Text = "ExportSettings";
		((Control)lbEncoding).AutoSize = true;
		((Control)lbEncoding).ForeColor = SystemColors.ControlText;
		((Control)lbEncoding).Location = new Point(5, 19);
		((Control)lbEncoding).Name = "lbEncoding";
		((Control)lbEncoding).Size = new Size(52, 13);
		((Control)lbEncoding).TabIndex = 3;
		((Control)lbEncoding).Text = "Encoding";
		lbEncoding.TextAlign = (ContentAlignment)16;
		((Control)cbEncoding).BackColor = SystemColors.Window;
		cbEncoding.DropDownStyle = (ComboBoxStyle)2;
		cbEncoding.DropDownWidth = 192;
		((Control)cbEncoding).ForeColor = SystemColors.ControlText;
		((Control)cbEncoding).Location = new Point(128, 16);
		cbEncoding.MaxDropDownItems = 15;
		((Control)cbEncoding).Name = "cbEncoding";
		((Control)cbEncoding).Size = new Size(176, 21);
		((Control)cbEncoding).TabIndex = 0;
		((Control)cbOpenAfterExport).Anchor = (AnchorStyles)6;
		((Control)cbOpenAfterExport).AutoSize = true;
		((Control)cbOpenAfterExport).Location = new Point(12, 200);
		((Control)cbOpenAfterExport).Name = "cbOpenAfterExport";
		((Control)cbOpenAfterExport).Size = new Size(104, 17);
		((Control)cbOpenAfterExport).TabIndex = 2;
		((Control)cbOpenAfterExport).Text = "OpenAfterExport";
		((Control)glButtons).BackColor = Color.White;
		((Control)glButtons).Controls.Add((Control)(object)btCancel);
		((Control)glButtons).Controls.Add((Control)(object)btOk);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = (StiLinePosition)0;
		glButtons.LineStyle = (StiLineStyle)0;
		((Control)glButtons).Location = new Point(0, 224);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(324, 46);
		((Control)glButtons).TabIndex = 3;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(324, 270);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Control)this).Controls.Add((Control)(object)gbExportSettings);
		((Control)this).Controls.Add((Control)(object)PagesRangeControl);
		((Control)this).Controls.Add((Control)(object)cbOpenAfterExport);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiDbfExportSetupForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiHtmlExportSetupForm";
		((Form)this).HelpButtonClicked += ThisHelpButtonClicked;
		((Form)this).Closed += ThisClosed;
		((Control)this).KeyDown += new KeyEventHandler(ThisKeyDown);
		((Control)gbExportSettings).ResumeLayout(false);
		((Control)gbExportSettings).PerformLayout();
		((Control)glButtons).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
