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
using Stimulsoft.Report.Win.Export;

namespace Stimulsoft.Report.Export.Win;

public class StiTxtExportSetupForm : Form
{
	private IContainer components;

	private Button buttonCancel;

	private Button buttonOk;

	private StiCheckBoxInfo checkBoxDrawBorder;

	private StiGroupBox groupBoxBorderType;

	private StiRadioButtonInfo radioButtonSimple;

	private StiCheckBoxInfo checkBoxKillSpaceLines;

	private StiCheckBoxInfo checkBoxPutFeedPageCode;

	private StiGroupBox groupBoxZoom;

	private StiDropDownBox comboBoxZoomY;

	private StiLabelInfo labelX;

	private StiDropDownBox comboBoxZoomX;

	private StiLabelInfo labelY;

	private StiRadioButtonInfo radioButtonUnicodeSingle;

	private StiRadioButtonInfo radioButtonUnicodeDouble;

	private StiGroupBox grouBoxSettings;

	private StiPagesRangeControl PagesRangeControl;

	private StiCheckBoxInfo checkBoxOpenAfterExport;

	private StiLabelInfo labelEncoding;

	private StiComboBox comboBoxEncoding;

	private StiGroupLine glButtons;

	private StiCheckBoxInfo checkBoxCutLongLines;

	private Panel panelOpenAfterExport;

	private Label label2;

	private Label label1;

	public StiPagesRange PagesRange => PagesRangeControl.GetPagesRange();

	public bool OpenAfterExport => ((CheckBox)checkBoxOpenAfterExport).Checked;

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
			return ((Control)checkBoxOpenAfterExport).Visible;
		}
		set
		{
			((Control)checkBoxOpenAfterExport).Visible = value;
		}
	}

	public bool OpenAfterExportEnabled
	{
		get
		{
			return ((Control)checkBoxOpenAfterExport).Enabled;
		}
		set
		{
			((Control)checkBoxOpenAfterExport).Enabled = value;
		}
	}

	public bool DrawBorder => ((CheckBox)checkBoxDrawBorder).Checked;

	public bool KillSpaceLines => ((CheckBox)checkBoxKillSpaceLines).Checked;

	public bool PutFeedPageCode => ((CheckBox)checkBoxPutFeedPageCode).Checked;

	public bool CutLongLines => ((CheckBox)checkBoxCutLongLines).Checked;

	public Encoding Encoding => StiEncodingHelper.GetEncodingFromName(((ComboBox)comboBoxEncoding).SelectedItem as string);

	public StiTxtBorderType BorderType
	{
		get
		{
			if (((RadioButton)radioButtonUnicodeSingle).Checked)
			{
				return StiTxtBorderType.UnicodeSingle;
			}
			if (((RadioButton)radioButtonUnicodeDouble).Checked)
			{
				return StiTxtBorderType.UnicodeDouble;
			}
			return StiTxtBorderType.Simple;
		}
	}

	public float ZoomX
	{
		get
		{
			if (float.TryParse(((Control)comboBoxZoomX).Text, out var result))
			{
				return result / 100f;
			}
			return 1f;
		}
	}

	public float ZoomY
	{
		get
		{
			if (float.TryParse(((Control)comboBoxZoomY).Text, out var result))
			{
				return result / 100f;
			}
			return 1f;
		}
	}

	public StiTxtExportSetupForm()
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
		((ComboBox)comboBoxEncoding).Items.Clear();
		ObjectCollection items = ((ComboBox)comboBoxEncoding).Items;
		object[] encodingNames = StiEncodingHelper.GetEncodingNames();
		items.AddRange(encodingNames);
		((ComboBox)comboBoxEncoding).SelectedItem = Encoding.Default.EncodingName;
		Localize();
		LocalizeSuperToolTip();
		StiSettings.Load();
		((CheckBox)checkBoxOpenAfterExport).Checked = StiSettings.GetBool(((Control)this).Name, "OpenAfterExport", ((CheckBox)checkBoxOpenAfterExport).Checked);
		grouBoxSettings.Collapsed = StiSettings.GetBool(((Control)this).Name, "ExportSettings", grouBoxSettings.Collapsed);
		((CheckBox)checkBoxCutLongLines).Checked = StiSettings.GetBool(((Control)this).Name, "CutLongLines", ((CheckBox)checkBoxCutLongLines).Checked);
		((CheckBox)checkBoxDrawBorder).Checked = StiSettings.GetBool(((Control)this).Name, "DrawBorder", ((CheckBox)checkBoxDrawBorder).Checked);
		((CheckBox)checkBoxKillSpaceLines).Checked = StiSettings.GetBool(((Control)this).Name, "KillSpaceLines", ((CheckBox)checkBoxKillSpaceLines).Checked);
		((CheckBox)checkBoxPutFeedPageCode).Checked = StiSettings.GetBool(((Control)this).Name, "PutFeedPageCode", ((CheckBox)checkBoxPutFeedPageCode).Checked);
		((RadioButton)radioButtonSimple).Checked = StiSettings.GetBool(((Control)this).Name, "Simple", ((RadioButton)radioButtonSimple).Checked);
		((RadioButton)radioButtonUnicodeSingle).Checked = StiSettings.GetBool(((Control)this).Name, "UnicodeSingle", ((RadioButton)radioButtonUnicodeSingle).Checked);
		((RadioButton)radioButtonUnicodeDouble).Checked = StiSettings.GetBool(((Control)this).Name, "UnicodeDouble", ((RadioButton)radioButtonUnicodeDouble).Checked);
		((Control)comboBoxZoomX).Handle.ToString();
		((Control)comboBoxZoomX).Text = StiSettings.GetStr(((Control)this).Name, "ZoomX", ((Control)comboBoxZoomX).Text).Replace("%", "");
		((Control)comboBoxZoomY).Handle.ToString();
		((Control)comboBoxZoomY).Text = StiSettings.GetStr(((Control)this).Name, "ZoomY", ((Control)comboBoxZoomY).Text).Replace("%", "");
		int @int = StiSettings.GetInt(((Control)this).Name, "Encoding", 0);
		if (@int < ((ComboBox)comboBoxEncoding).Items.Count)
		{
			((ListControl)comboBoxEncoding).SelectedIndex = @int;
		}
		((ComboBox)comboBoxEncoding).DropDownWidth = StiScale.XXI((double)((ComboBox)comboBoxEncoding).DropDownWidth);
	}

	private void This_Closed(object sender, EventArgs e)
	{
		StiSettings.Load();
		StiSettings.Set(((Control)this).Name, "OpenAfterExport", ((CheckBox)checkBoxOpenAfterExport).Checked);
		StiSettings.Set(((Control)this).Name, "ExportSettings", grouBoxSettings.Collapsed);
		StiSettings.Set(((Control)this).Name, "CutLongLines", ((CheckBox)checkBoxCutLongLines).Checked);
		StiSettings.Set(((Control)this).Name, "DrawBorder", ((CheckBox)checkBoxDrawBorder).Checked);
		StiSettings.Set(((Control)this).Name, "KillSpaceLines", ((CheckBox)checkBoxKillSpaceLines).Checked);
		StiSettings.Set(((Control)this).Name, "PutFeedPageCode", ((CheckBox)checkBoxPutFeedPageCode).Checked);
		StiSettings.Set(((Control)this).Name, "Simple", ((RadioButton)radioButtonSimple).Checked);
		StiSettings.Set(((Control)this).Name, "UnicodeSingle", ((RadioButton)radioButtonUnicodeSingle).Checked);
		StiSettings.Set(((Control)this).Name, "UnicodeDouble", ((RadioButton)radioButtonUnicodeDouble).Checked);
		StiSettings.Set(((Control)this).Name, "ZoomX", ((Control)comboBoxZoomX).Text);
		StiSettings.Set(((Control)this).Name, "ZoomY", ((Control)comboBoxZoomY).Text);
		StiSettings.Set(((Control)this).Name, "Encoding", ((ListControl)comboBoxEncoding).SelectedIndex);
		StiSettings.Save();
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

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/exports_text_formats_txt.htm");
	}

	private void CheckBoxDrawBorder_CheckedChanged(object sender, EventArgs e)
	{
		((Control)groupBoxBorderType).Enabled = !((Control)groupBoxBorderType).Enabled;
	}

	private void Localize()
	{
		((Control)this).Text = string.Format("{0} - {1}", StiLocalization.Get("Export", "ExportTypeTxtFile").Replace("...", ""), StiLocalization.Get("Export", "title").Replace("&", "_"));
		((Control)labelEncoding).Text = StiLocalization.GetCleaned("Export", "Encoding");
		((Control)checkBoxDrawBorder).Text = StiLocalization.Get("Export", "TxtDrawBorder");
		((Control)groupBoxBorderType).Text = StiLocalization.Get("Export", "TxtBorderType");
		((Control)checkBoxCutLongLines).Text = StiLocalization.Get("Export", "TxtCutLongLines");
		((Control)radioButtonSimple).Text = StiLocalization.Get("Export", "TxtBorderTypeSimple");
		((Control)radioButtonUnicodeSingle).Text = StiLocalization.Get("Export", "TxtBorderTypeSingle");
		((Control)radioButtonUnicodeDouble).Text = StiLocalization.Get("Export", "TxtBorderTypeDouble");
		((Control)checkBoxKillSpaceLines).Text = StiLocalization.Get("Export", "TxtKillSpaceLines");
		((Control)checkBoxPutFeedPageCode).Text = StiLocalization.Get("Export", "TxtPutFeedPageCode");
		((Control)labelX).Text = StiLocalization.GetCleaned("Export", "X");
		((Control)labelY).Text = StiLocalization.GetCleaned("Export", "Y");
		((Control)grouBoxSettings).Text = StiLocalization.Get("Export", "Settings");
		((Control)groupBoxZoom).Text = StiLocalization.Get("Export", "Zoom").Replace(":", "");
		((Control)checkBoxOpenAfterExport).Text = StiLocalization.Get("Export", "OpenAfterExport");
		((Control)buttonOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = StiLocalization.Get("Buttons", "Cancel");
	}

	private void LocalizeSuperToolTip()
	{
		checkBoxKillSpaceLines.SetToolTip(StiLocalization.Get("HelpViewer", "KillSpaceLines"));
		checkBoxPutFeedPageCode.SetToolTip(StiLocalization.Get("HelpViewer", "PutFeedPageCode"));
		checkBoxDrawBorder.SetToolTip(StiLocalization.Get("HelpViewer", "DrawBorder"));
		checkBoxCutLongLines.SetToolTip(StiLocalization.Get("HelpViewer", "CutLongLines"));
		checkBoxOpenAfterExport.SetToolTip(StiLocalization.Get("HelpViewer", "OpenAfterExport"));
		radioButtonSimple.SetToolTip(StiLocalization.Get("HelpViewer", "BorderType"));
		radioButtonUnicodeSingle.SetToolTip(StiLocalization.Get("HelpViewer", "BorderType"));
		radioButtonUnicodeDouble.SetToolTip(StiLocalization.Get("HelpViewer", "BorderType"));
		labelEncoding.SetToolTip(StiLocalization.Get("HelpViewer", "Encoding"));
		labelX.SetToolTip(StiLocalization.Get("HelpViewer", "ZoomTxt"));
		labelY.SetToolTip(StiLocalization.Get("HelpViewer", "ZoomTxt"));
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
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Expected O, but got Unknown
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_05db: Unknown result type (might be due to invalid IL or missing references)
		//IL_076a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d60: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1194: Unknown result type (might be due to invalid IL or missing references)
		//IL_119e: Expected O, but got Unknown
		groupBoxBorderType = new StiGroupBox();
		radioButtonSimple = new StiRadioButtonInfo();
		radioButtonUnicodeSingle = new StiRadioButtonInfo();
		radioButtonUnicodeDouble = new StiRadioButtonInfo();
		buttonCancel = new Button();
		buttonOk = new Button();
		groupBoxZoom = new StiGroupBox();
		labelY = new StiLabelInfo();
		comboBoxZoomY = new StiDropDownBox();
		labelX = new StiLabelInfo();
		comboBoxZoomX = new StiDropDownBox();
		grouBoxSettings = new StiGroupBox();
		checkBoxCutLongLines = new StiCheckBoxInfo();
		labelEncoding = new StiLabelInfo();
		comboBoxEncoding = new StiComboBox();
		checkBoxDrawBorder = new StiCheckBoxInfo();
		checkBoxKillSpaceLines = new StiCheckBoxInfo();
		checkBoxPutFeedPageCode = new StiCheckBoxInfo();
		glButtons = new StiGroupLine();
		panelOpenAfterExport = new Panel();
		checkBoxOpenAfterExport = new StiCheckBoxInfo();
		PagesRangeControl = new StiPagesRangeControl();
		label1 = new Label();
		label2 = new Label();
		((Control)groupBoxBorderType).SuspendLayout();
		((Control)groupBoxZoom).SuspendLayout();
		((Control)grouBoxSettings).SuspendLayout();
		((Control)glButtons).SuspendLayout();
		((Control)panelOpenAfterExport).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)groupBoxBorderType).Controls.Add((Control)(object)radioButtonSimple);
		((Control)groupBoxBorderType).Controls.Add((Control)(object)radioButtonUnicodeSingle);
		((Control)groupBoxBorderType).Controls.Add((Control)(object)radioButtonUnicodeDouble);
		((Control)groupBoxBorderType).Location = new Point(12, 156);
		((Control)groupBoxBorderType).Name = "groupBoxBorderType";
		groupBoxBorderType.ResHeight = 0;
		((Control)groupBoxBorderType).Size = new Size(152, 121);
		((Control)groupBoxBorderType).TabIndex = 4;
		((GroupBox)groupBoxBorderType).TabStop = false;
		((Control)groupBoxBorderType).Text = "Border type";
		((Control)radioButtonSimple).AutoSize = true;
		((RadioButton)radioButtonSimple).Checked = true;
		((Control)radioButtonSimple).ForeColor = SystemColors.ControlText;
		((Control)radioButtonSimple).Location = new Point(12, 24);
		((Control)radioButtonSimple).Name = "radioButtonSimple";
		((Control)radioButtonSimple).Padding = new Padding(0, 0, 12, 0);
		((Control)radioButtonSimple).Size = new Size(68, 17);
		((Control)radioButtonSimple).TabIndex = 0;
		((RadioButton)radioButtonSimple).TabStop = true;
		((Control)radioButtonSimple).Text = "Simple";
		((Control)radioButtonUnicodeSingle).AutoSize = true;
		((Control)radioButtonUnicodeSingle).ForeColor = SystemColors.ControlText;
		((Control)radioButtonUnicodeSingle).Location = new Point(12, 57);
		((Control)radioButtonUnicodeSingle).Name = "radioButtonUnicodeSingle";
		((Control)radioButtonUnicodeSingle).Padding = new Padding(0, 0, 12, 0);
		((Control)radioButtonUnicodeSingle).Size = new Size(66, 17);
		((Control)radioButtonUnicodeSingle).TabIndex = 1;
		((Control)radioButtonUnicodeSingle).Text = "Single";
		((Control)radioButtonUnicodeDouble).AutoSize = true;
		((Control)radioButtonUnicodeDouble).ForeColor = SystemColors.ControlText;
		((Control)radioButtonUnicodeDouble).Location = new Point(12, 90);
		((Control)radioButtonUnicodeDouble).Name = "radioButtonUnicodeDouble";
		((Control)radioButtonUnicodeDouble).Padding = new Padding(0, 0, 12, 0);
		((Control)radioButtonUnicodeDouble).Size = new Size(71, 17);
		((Control)radioButtonUnicodeDouble).TabIndex = 2;
		((Control)radioButtonUnicodeDouble).Text = "Double";
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		buttonCancel.DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(284, 12);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(75, 24);
		((Control)buttonCancel).TabIndex = 2;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)buttonOk).Anchor = (AnchorStyles)10;
		buttonOk.DialogResult = (DialogResult)1;
		((Control)buttonOk).Location = new Point(204, 12);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(75, 24);
		((Control)buttonOk).TabIndex = 1;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)groupBoxZoom).Controls.Add((Control)(object)label2);
		((Control)groupBoxZoom).Controls.Add((Control)(object)label1);
		((Control)groupBoxZoom).Controls.Add((Control)(object)labelY);
		((Control)groupBoxZoom).Controls.Add((Control)(object)comboBoxZoomY);
		((Control)groupBoxZoom).Controls.Add((Control)(object)labelX);
		((Control)groupBoxZoom).Controls.Add((Control)(object)comboBoxZoomX);
		((Control)groupBoxZoom).Location = new Point(176, 156);
		((Control)groupBoxZoom).Name = "groupBoxZoom";
		groupBoxZoom.ResHeight = 0;
		((Control)groupBoxZoom).Size = new Size(156, 121);
		((Control)groupBoxZoom).TabIndex = 5;
		((GroupBox)groupBoxZoom).TabStop = false;
		((Control)groupBoxZoom).Text = "Zoom";
		((Control)labelY).AutoSize = true;
		((Control)labelY).ForeColor = Color.DimGray;
		((Control)labelY).Location = new Point(8, 63);
		((Control)labelY).Name = "labelY";
		((Control)labelY).Padding = new Padding(0, 0, 12, 0);
		((Control)labelY).Size = new Size(26, 13);
		((Control)labelY).TabIndex = 3;
		((Control)labelY).Text = "Y";
		((Label)labelY).TextAlign = (ContentAlignment)16;
		((Control)comboBoxZoomY).BackColor = SystemColors.Window;
		comboBoxZoomY.DrawMode = (DrawMode)2;
		((Control)comboBoxZoomY).ForeColor = SystemColors.ControlText;
		comboBoxZoomY.IsEditable = true;
		comboBoxZoomY.Items.AddRange(new object[7] { "25", "50", "75", "100", "125", "150", "200" });
		((Control)comboBoxZoomY).Location = new Point(48, 58);
		((Control)comboBoxZoomY).Name = "comboBoxZoomY";
		comboBoxZoomY.SelectedIndex = -1;
		comboBoxZoomY.SelectedItem = null;
		comboBoxZoomY.SelectedValue = null;
		((Control)comboBoxZoomY).Size = new Size(72, 21);
		((Control)comboBoxZoomY).TabIndex = 1;
		((Control)comboBoxZoomY).Text = "100%";
		((Control)labelX).AutoSize = true;
		((Control)labelX).ForeColor = Color.DimGray;
		((Control)labelX).Location = new Point(8, 29);
		((Control)labelX).Name = "labelX";
		((Control)labelX).Padding = new Padding(0, 0, 12, 0);
		((Control)labelX).Size = new Size(26, 13);
		((Control)labelX).TabIndex = 2;
		((Control)labelX).Text = "X";
		((Label)labelX).TextAlign = (ContentAlignment)16;
		((Control)comboBoxZoomX).BackColor = SystemColors.Window;
		comboBoxZoomX.DrawMode = (DrawMode)2;
		((Control)comboBoxZoomX).ForeColor = SystemColors.ControlText;
		comboBoxZoomX.IsEditable = true;
		comboBoxZoomX.Items.AddRange(new object[7] { "25", "50", "75", "100", "125", "150", "200" });
		((Control)comboBoxZoomX).Location = new Point(48, 24);
		((Control)comboBoxZoomX).Name = "comboBoxZoomX";
		comboBoxZoomX.SelectedIndex = -1;
		comboBoxZoomX.SelectedItem = null;
		comboBoxZoomX.SelectedValue = null;
		((Control)comboBoxZoomX).Size = new Size(72, 21);
		((Control)comboBoxZoomX).TabIndex = 0;
		((Control)comboBoxZoomX).Text = "100%";
		grouBoxSettings.AllowCollapse = true;
		((Control)grouBoxSettings).Anchor = (AnchorStyles)13;
		((Control)grouBoxSettings).BackColor = Color.White;
		((Control)grouBoxSettings).Controls.Add((Control)(object)checkBoxCutLongLines);
		((Control)grouBoxSettings).Controls.Add((Control)(object)labelEncoding);
		((Control)grouBoxSettings).Controls.Add((Control)(object)comboBoxEncoding);
		((Control)grouBoxSettings).Controls.Add((Control)(object)checkBoxDrawBorder);
		((Control)grouBoxSettings).Controls.Add((Control)(object)groupBoxBorderType);
		((Control)grouBoxSettings).Controls.Add((Control)(object)checkBoxKillSpaceLines);
		((Control)grouBoxSettings).Controls.Add((Control)(object)checkBoxPutFeedPageCode);
		((Control)grouBoxSettings).Controls.Add((Control)(object)groupBoxZoom);
		((Control)grouBoxSettings).Location = new Point(12, 175);
		((Control)grouBoxSettings).Name = "grouBoxSettings";
		grouBoxSettings.ResHeight = 250;
		((Control)grouBoxSettings).Size = new Size(345, 331);
		((Control)grouBoxSettings).TabIndex = 1;
		((GroupBox)grouBoxSettings).TabStop = false;
		((Control)grouBoxSettings).Text = "ExportSettings";
		((Control)checkBoxCutLongLines).AutoSize = true;
		((CheckBox)checkBoxCutLongLines).Checked = true;
		((CheckBox)checkBoxCutLongLines).CheckState = (CheckState)1;
		((Control)checkBoxCutLongLines).ForeColor = SystemColors.ControlText;
		((Control)checkBoxCutLongLines).Location = new Point(12, 123);
		((Control)checkBoxCutLongLines).Name = "checkBoxCutLongLines";
		((Control)checkBoxCutLongLines).Padding = new Padding(0, 0, 12, 0);
		((Control)checkBoxCutLongLines).Size = new Size(109, 17);
		((Control)checkBoxCutLongLines).TabIndex = 3;
		((Control)checkBoxCutLongLines).Text = "Cut Long Lines";
		((Control)labelEncoding).AutoSize = true;
		((Control)labelEncoding).ForeColor = Color.DimGray;
		((Control)labelEncoding).Location = new Point(12, 298);
		((Control)labelEncoding).Name = "labelEncoding";
		((Control)labelEncoding).Padding = new Padding(0, 0, 12, 0);
		((Control)labelEncoding).Size = new Size(64, 13);
		((Control)labelEncoding).TabIndex = 6;
		((Control)labelEncoding).Text = "Encoding";
		((Label)labelEncoding).TextAlign = (ContentAlignment)16;
		((Control)comboBoxEncoding).BackColor = SystemColors.Window;
		comboBoxEncoding.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxEncoding).ForeColor = SystemColors.ControlText;
		((Control)comboBoxEncoding).Location = new Point(176, 293);
		((Control)comboBoxEncoding).Name = "comboBoxEncoding";
		((Control)comboBoxEncoding).Size = new Size(155, 21);
		((Control)comboBoxEncoding).TabIndex = 7;
		((Control)checkBoxDrawBorder).AutoSize = true;
		((CheckBox)checkBoxDrawBorder).Checked = true;
		((CheckBox)checkBoxDrawBorder).CheckState = (CheckState)1;
		((Control)checkBoxDrawBorder).ForeColor = SystemColors.ControlText;
		((Control)checkBoxDrawBorder).Location = new Point(12, 90);
		((Control)checkBoxDrawBorder).Name = "checkBoxDrawBorder";
		((Control)checkBoxDrawBorder).Padding = new Padding(0, 0, 12, 0);
		((Control)checkBoxDrawBorder).Size = new Size(96, 17);
		((Control)checkBoxDrawBorder).TabIndex = 2;
		((Control)checkBoxDrawBorder).Text = "Draw border";
		((CheckBox)checkBoxDrawBorder).CheckedChanged += CheckBoxDrawBorder_CheckedChanged;
		((Control)checkBoxKillSpaceLines).AutoSize = true;
		((CheckBox)checkBoxKillSpaceLines).Checked = true;
		((CheckBox)checkBoxKillSpaceLines).CheckState = (CheckState)1;
		((Control)checkBoxKillSpaceLines).ForeColor = SystemColors.ControlText;
		((Control)checkBoxKillSpaceLines).Location = new Point(12, 24);
		((Control)checkBoxKillSpaceLines).Name = "checkBoxKillSpaceLines";
		((Control)checkBoxKillSpaceLines).Padding = new Padding(0, 0, 12, 0);
		((Control)checkBoxKillSpaceLines).Size = new Size(107, 17);
		((Control)checkBoxKillSpaceLines).TabIndex = 0;
		((Control)checkBoxKillSpaceLines).Text = "Kill space lines";
		((Control)checkBoxPutFeedPageCode).AutoSize = true;
		((CheckBox)checkBoxPutFeedPageCode).Checked = true;
		((CheckBox)checkBoxPutFeedPageCode).CheckState = (CheckState)1;
		((Control)checkBoxPutFeedPageCode).ForeColor = SystemColors.ControlText;
		((Control)checkBoxPutFeedPageCode).Location = new Point(12, 57);
		((Control)checkBoxPutFeedPageCode).Name = "checkBoxPutFeedPageCode";
		((Control)checkBoxPutFeedPageCode).Padding = new Padding(0, 0, 12, 0);
		((Control)checkBoxPutFeedPageCode).Size = new Size(132, 17);
		((Control)checkBoxPutFeedPageCode).TabIndex = 1;
		((Control)checkBoxPutFeedPageCode).Text = "Put feed page code";
		((Control)glButtons).Controls.Add((Control)(object)buttonCancel);
		((Control)glButtons).Controls.Add((Control)(object)buttonOk);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = (StiLinePosition)0;
		glButtons.LineStyle = (StiLineStyle)0;
		((Control)glButtons).Location = new Point(0, 549);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(370, 46);
		((Control)glButtons).TabIndex = 3;
		((Control)panelOpenAfterExport).Controls.Add((Control)(object)checkBoxOpenAfterExport);
		((Control)panelOpenAfterExport).Dock = (DockStyle)2;
		((Control)panelOpenAfterExport).Location = new Point(0, 507);
		((Control)panelOpenAfterExport).Name = "panelOpenAfterExport";
		((Control)panelOpenAfterExport).Size = new Size(370, 42);
		((Control)panelOpenAfterExport).TabIndex = 2;
		((Control)checkBoxOpenAfterExport).Anchor = (AnchorStyles)6;
		((Control)checkBoxOpenAfterExport).AutoSize = true;
		((Control)checkBoxOpenAfterExport).Location = new Point(12, 12);
		((Control)checkBoxOpenAfterExport).Name = "checkBoxOpenAfterExport";
		((Control)checkBoxOpenAfterExport).Padding = new Padding(0, 0, 12, 0);
		((Control)checkBoxOpenAfterExport).Size = new Size(116, 17);
		((Control)checkBoxOpenAfterExport).TabIndex = 2;
		((Control)checkBoxOpenAfterExport).Text = "OpenAfterExport";
		((Control)PagesRangeControl).Anchor = (AnchorStyles)13;
		((Control)PagesRangeControl).BackColor = Color.White;
		PagesRangeControl.CurrentPage = 0;
		((Control)PagesRangeControl).Location = new Point(12, 8);
		((Control)PagesRangeControl).Name = "PagesRangeControl";
		((Control)PagesRangeControl).Size = new Size(345, 159);
		((Control)PagesRangeControl).TabIndex = 0;
		((Control)label1).AutoSize = true;
		((Control)label1).ForeColor = Color.DimGray;
		((Control)label1).Location = new Point(126, 29);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(15, 13);
		((Control)label1).TabIndex = 20;
		((Control)label1).Text = "%";
		label1.TextAlign = (ContentAlignment)16;
		((Control)label2).AutoSize = true;
		((Control)label2).ForeColor = Color.DimGray;
		((Control)label2).Location = new Point(126, 63);
		((Control)label2).Name = "label2";
		((Control)label2).Size = new Size(15, 13);
		((Control)label2).TabIndex = 21;
		((Control)label2).Text = "%";
		label2.TextAlign = (ContentAlignment)16;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(370, 595);
		((Control)this).Controls.Add((Control)(object)panelOpenAfterExport);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Control)this).Controls.Add((Control)(object)grouBoxSettings);
		((Control)this).Controls.Add((Control)(object)PagesRangeControl);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiTxtExportSetupForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiTxtExportSetupForm";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).Closed += This_Closed;
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)groupBoxBorderType).ResumeLayout(false);
		((Control)groupBoxBorderType).PerformLayout();
		((Control)groupBoxZoom).ResumeLayout(false);
		((Control)groupBoxZoom).PerformLayout();
		((Control)grouBoxSettings).ResumeLayout(false);
		((Control)grouBoxSettings).PerformLayout();
		((Control)glButtons).ResumeLayout(false);
		((Control)panelOpenAfterExport).ResumeLayout(false);
		((Control)panelOpenAfterExport).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
