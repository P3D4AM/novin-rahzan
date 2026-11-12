using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Design;

public class StiReportSetupForm : StiFormUX
{
	private string oldValue;

	private StiDesignerControl designer;

	private StiReportLanguageType oldLanguage;

	private IContainer components;

	private StiButtonUX btCancel;

	private StiButtonUX btOk;

	private StiButtonPanelUX footerPanel;

	private StiCloudTabulator tabControl;

	private StiCloudTabulatorPage pageMain;

	private StiCloudTabulatorPage pageDescription;

	private StiOffice2013GroupBox gbMainParameters;

	private StiComboBox comboBoxReportCacheMode;

	private StiComboBox comboBoxNumberOfPass;

	private StiOffice2013GroupBox gbScript;

	private StiOffice2013GroupBox gbUnit;

	private StiOffice2013GroupBox gbNames;

	private StiOffice2013GroupBox gbDates;

	private StiOffice2013GroupBox gbDescription;

	private StiTextBox ebAuthor;

	private StiTextBox ebName;

	private StiTextBox ebAlias;

	private StiTextBoxUX ebDescription;

	private StiComboBox comboBoxCalculationMode;

	private StiLabelUX labelReportCacheMode;

	private StiLabelUX labelNumberOfPass;

	private StiCheckBoxUX checkBoxConvertNulls;

	private StiCheckBoxUX checkBoxCacheAllData;

	private StiRadioButtonUX rbCSharp;

	private StiRadioButtonUX rbVB;

	private StiRadioButtonUX rbCentimetres;

	private StiRadioButtonUX rbHundredthsOfInch;

	private StiRadioButtonUX rbMillimeters;

	private StiRadioButtonUX rbInch;

	private StiLabelUX lbName;

	private StiLabelUX lbAlias;

	private StiLabelUX lbAuthor;

	private StiLabelUX lbRChanged;

	private StiLabelUX lbRCreated;

	private StiLabelUX lbReportChanged;

	private StiLabelUX lbReportCreated;

	private StiLabelUX labelCalculationMode;

	private StiRadioButtonUX rbJS;

	private StiLabelUX labelLanguageJS;

	public StiReportSetupForm(StiDesignerControl designer)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			DoTheming();
			DoScaling();
			((Form)this).ShowIcon = false;
			if (StiOptions.Appearance.ShowFormsHelp)
			{
				((Form)this).HelpButton = true;
			}
			this.designer = designer;
			Localize();
			((Control)ebName).Text = designer.Report.ReportName;
			((Control)ebAlias).Text = designer.Report.ReportAlias;
			((Control)ebAuthor).Text = designer.Report.ReportAuthor;
			((Control)ebDescription).Text = designer.Report.ReportDescription;
			((CheckBox)checkBoxConvertNulls).Checked = designer.Report.ConvertNulls;
			((CheckBox)checkBoxCacheAllData).Checked = designer.Report.CacheAllData;
			((RadioButton)rbCentimetres).Checked = designer.Report.Unit is StiCentimetersUnit;
			((RadioButton)rbMillimeters).Checked = designer.Report.Unit is StiMillimetersUnit;
			((RadioButton)rbInch).Checked = designer.Report.Unit is StiInchesUnit;
			((RadioButton)rbHundredthsOfInch).Checked = designer.Report.Unit is StiHundredthsOfInchUnit;
			if (designer.Report.NumberOfPass == StiNumberOfPass.SinglePass)
			{
				((Control)comboBoxNumberOfPass).Text = StiLocalization.Get("PropertyEnum", "StiNumberOfPassSinglePass");
			}
			else
			{
				((Control)comboBoxNumberOfPass).Text = StiLocalization.Get("PropertyEnum", "StiNumberOfPassDoublePass");
			}
			if (designer.Report.ReportCacheMode == StiReportCacheMode.Off)
			{
				((Control)comboBoxReportCacheMode).Text = StiLocalization.Get("PropertyEnum", "StiReportCacheModeOff");
			}
			else if (designer.Report.ReportCacheMode == StiReportCacheMode.On)
			{
				((Control)comboBoxReportCacheMode).Text = StiLocalization.Get("PropertyEnum", "StiReportCacheModeOn");
			}
			else if (designer.Report.ReportCacheMode == StiReportCacheMode.Auto)
			{
				((Control)comboBoxReportCacheMode).Text = StiLocalization.Get("PropertyEnum", "StiReportCacheModeAuto");
			}
			if (designer.Report.CalculationMode == StiCalculationMode.Compilation)
			{
				((Control)comboBoxCalculationMode).Text = StiLocalization.Get("PropertyEnum", "StiCalculationModeCompilation");
			}
			else if (designer.Report.CalculationMode == StiCalculationMode.Interpretation)
			{
				((Control)comboBoxCalculationMode).Text = StiLocalization.Get("PropertyEnum", "StiCalculationModeInterpretation");
			}
			((Control)lbRCreated).Text = designer.Report.ReportCreated.ToString();
			((Control)lbRChanged).Text = designer.Report.ReportChanged.ToString();
			((RadioButton)rbCSharp).Checked = designer.Report.ScriptLanguage == StiReportLanguageType.CSharp;
			((RadioButton)rbVB).Checked = designer.Report.ScriptLanguage == StiReportLanguageType.VB;
			((RadioButton)rbJS).Checked = designer.Report.ScriptLanguage == StiReportLanguageType.JS;
			oldLanguage = designer.Report.ScriptLanguage;
			if (!((RadioButton)rbJS).Checked)
			{
				((Control)rbJS).Visible = false;
				((Control)labelLanguageJS).Visible = false;
				StiOffice2013GroupBox stiOffice2013GroupBox = gbScript;
				((Control)stiOffice2013GroupBox).Height = ((Control)stiOffice2013GroupBox).Height - (((Control)rbJS).Height + StiScaleUI.I12);
			}
			if (!StiOptions.Designer.IsDeveloperDesigner)
			{
				pageMain.Invisible = true;
				tabControl.SelectedIndex = 1;
			}
			else
			{
				tabControl.SelectedIndex = 0;
			}
		}
	}

	private void Localize()
	{
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((Control)this).Text = StiLocalization.Get("FormReportSetup", "title");
		((Control)lbReportCreated).Text = Loc.GetCleaned("FormReportSetup", "ReportCreated");
		((Control)lbReportChanged).Text = Loc.GetCleaned("FormReportSetup", "ReportChanged");
		((Control)labelReportCacheMode).Text = Loc.GetCleaned("FormReportSetup", "labelReportCacheMode");
		((Control)labelNumberOfPass).Text = Loc.GetCleaned("FormReportSetup", "labelNumberOfPass");
		((Control)labelCalculationMode).Text = Loc.GetCleaned("PropertyMain", "CalculationMode");
		((Control)checkBoxCacheAllData).Text = StiLocalization.Get("PropertyMain", "CacheAllData");
		((Control)checkBoxConvertNulls).Text = StiLocalization.Get("PropertyMain", "ConvertNulls");
		((ComboBox)comboBoxNumberOfPass).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiNumberOfPassSinglePass"));
		((ComboBox)comboBoxNumberOfPass).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiNumberOfPassDoublePass"));
		((ComboBox)comboBoxReportCacheMode).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiReportCacheModeOff"));
		((ComboBox)comboBoxReportCacheMode).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiReportCacheModeOn"));
		((ComboBox)comboBoxReportCacheMode).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiReportCacheModeAuto"));
		((ComboBox)comboBoxCalculationMode).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiCalculationModeCompilation"));
		((ComboBox)comboBoxCalculationMode).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiCalculationModeInterpretation"));
		((Control)pageMain).Text = StiLocalization.Get("PropertyCategory", "MainCategory");
		((Control)pageDescription).Text = StiLocalization.Get("PropertyCategory", "DescriptionCategory");
		((Control)lbName).Text = Loc.GetCleaned("Report", "LabelName");
		((Control)lbAlias).Text = Loc.GetCleaned("Report", "LabelAlias");
		((Control)lbAuthor).Text = Loc.GetCleaned("Report", "LabelAuthor");
		((Control)rbCentimetres).Text = StiLocalization.Get("PropertyEnum", "StiReportUnitTypeCentimeters");
		((Control)rbHundredthsOfInch).Text = StiLocalization.Get("PropertyEnum", "StiReportUnitTypeHundredthsOfInch");
		((Control)rbInch).Text = StiLocalization.Get("PropertyEnum", "StiReportUnitTypeInches");
		((Control)rbMillimeters).Text = StiLocalization.Get("PropertyEnum", "StiReportUnitTypeMillimeters");
		((Control)gbDates).Text = StiLocalization.Get("FormReportSetup", "groupDates");
		((Control)gbDescription).Text = StiLocalization.Get("FormReportSetup", "groupDescription");
		((Control)gbMainParameters).Text = StiLocalization.Get("FormReportSetup", "groupMainParameters");
		((Control)gbNames).Text = StiLocalization.Get("FormReportSetup", "groupNames");
		((Control)gbScript).Text = StiLocalization.Get("FormReportSetup", "groupScript");
		((Control)gbUnit).Text = StiLocalization.Get("FormReportSetup", "groupUnits");
	}

	private void DoTheming()
	{
		if (StiUX.IsDark)
		{
			StiLabelUX stiLabelUX = lbRCreated;
			Color backColor = (((Control)lbRChanged).BackColor = StiUX.InputBackground);
			((Control)stiLabelUX).BackColor = backColor;
			StiLabelUX stiLabelUX2 = lbRCreated;
			backColor = (((Control)lbRChanged).ForeColor = StiUX.InputForeground);
			((Control)stiLabelUX2).ForeColor = backColor;
		}
	}

	private void DoScaling()
	{
		DockPaddingEdges dockPadding = tabControl.DockPadding;
		int right = (tabControl.TitleWidth = StiScale.XXI(110.0));
		dockPadding.Right = right;
		pageDescription.Image = StiReportDesignImages.Tasks.ReportOptionDescription(StiImageSize.Double);
		pageMain.Image = StiReportDesignImages.Tasks.ReportOptionMain(StiImageSize.Double);
	}

	private void btOk_Click(object sender, EventArgs e)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Invalid comparison between Unknown and I4
		StiReportLanguageType stiReportLanguageType = StiReportLanguageType.CSharp;
		if (((RadioButton)rbVB).Checked)
		{
			stiReportLanguageType = StiReportLanguageType.VB;
		}
		if (((RadioButton)rbJS).Checked)
		{
			stiReportLanguageType = StiReportLanguageType.JS;
		}
		bool flag = true;
		if (oldLanguage != stiReportLanguageType)
		{
			flag = (int)MessageBox.Show(StiLocalization.Get("Questions", "qnLanguageNew"), StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuFileReportSetup")), (MessageBoxButtons)4) == 6;
		}
		if (flag)
		{
			designer.UndoRedoSave(StiLocalization.Get("MainMenu", "menuFileReportSetup"));
			if (designer.Report.ReportName != ((Control)ebName).Text)
			{
				string newValue = StiNameValidator.CorrectName(((Control)ebName).Text, designer.Report);
				string text = StiNameValidator.CorrectName(designer.Report.ReportName, designer.Report);
				string classKeyword = ((StiCodeGenerator)(object)designer.Report.Language.GetProvider().CreateGenerator()).GetClassKeyword();
				designer.Report.Script = StiLexer.ReplaceWithPrefix(designer.Report.Script, classKeyword, text, newValue);
				designer.Report.Script = StiLexer.ReplaceWithPrefix(designer.Report.Script, "public", text, newValue);
			}
			designer.Report.ReportName = ((Control)ebName).Text;
			designer.Report.ReportAlias = ((Control)ebAlias).Text;
			designer.Report.ReportAuthor = ((Control)ebAuthor).Text;
			designer.Report.ReportDescription = ((Control)ebDescription).Text;
			designer.Report.ScriptLanguage = stiReportLanguageType;
			if (oldLanguage != stiReportLanguageType)
			{
				designer.Report.ScriptNew();
			}
			if (((RadioButton)rbCentimetres).Checked)
			{
				designer.Report.Unit = new StiCentimetersUnit();
			}
			if (((RadioButton)rbMillimeters).Checked)
			{
				designer.Report.Unit = new StiMillimetersUnit();
			}
			if (((RadioButton)rbInch).Checked)
			{
				designer.Report.Unit = new StiInchesUnit();
			}
			if (((RadioButton)rbHundredthsOfInch).Checked)
			{
				designer.Report.Unit = new StiHundredthsOfInchUnit();
			}
			if (((Control)comboBoxNumberOfPass).Text == StiLocalization.Get("PropertyEnum", "StiNumberOfPassSinglePass"))
			{
				designer.Report.NumberOfPass = StiNumberOfPass.SinglePass;
			}
			else
			{
				designer.Report.NumberOfPass = StiNumberOfPass.DoublePass;
			}
			if (((Control)comboBoxReportCacheMode).Text == StiLocalization.Get("PropertyEnum", "StiReportCacheModeOff"))
			{
				designer.Report.ReportCacheMode = StiReportCacheMode.Off;
			}
			else if (((Control)comboBoxReportCacheMode).Text == StiLocalization.Get("PropertyEnum", "StiReportCacheModeOn"))
			{
				designer.Report.ReportCacheMode = StiReportCacheMode.On;
			}
			else if (((Control)comboBoxReportCacheMode).Text == StiLocalization.Get("PropertyEnum", "StiReportCacheModeAuto"))
			{
				designer.Report.ReportCacheMode = StiReportCacheMode.Auto;
			}
			if (((Control)comboBoxCalculationMode).Text == StiLocalization.Get("PropertyEnum", "StiCalculationModeCompilation"))
			{
				designer.Report.CalculationMode = StiCalculationMode.Compilation;
			}
			else if (((Control)comboBoxCalculationMode).Text == StiLocalization.Get("PropertyEnum", "StiCalculationModeInterpretation"))
			{
				designer.Report.CalculationMode = StiCalculationMode.Interpretation;
			}
			designer.Report.ConvertNulls = ((CheckBox)checkBoxConvertNulls).Checked;
			designer.Report.CacheAllData = ((CheckBox)checkBoxCacheAllData).Checked;
			((Form)this).Close();
		}
	}

	private void ebName_Enter(object sender, EventArgs e)
	{
		oldValue = ((Control)ebName).Text;
	}

	private void ebName_Leave(object sender, EventArgs e)
	{
		if (oldValue == ((Control)ebAlias).Text)
		{
			((Control)ebAlias).Text = ((Control)ebName).Text;
		}
	}

	private void ThisHelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/reports_designer_ribbon_mode_2013_main_menu_dialog_report_setup.htm");
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
		//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ad: Expected O, but got Unknown
		//IL_04e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bd: Expected O, but got Unknown
		//IL_0f27: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f31: Expected O, but got Unknown
		//IL_0f69: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1142: Unknown result type (might be due to invalid IL or missing references)
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiReportSetupForm));
		btOk = new StiButtonUX();
		btCancel = new StiButtonUX();
		tabControl = new StiCloudTabulator();
		pageMain = new StiCloudTabulatorPage();
		gbUnit = new StiOffice2013GroupBox();
		rbCentimetres = new StiRadioButtonUX();
		rbHundredthsOfInch = new StiRadioButtonUX();
		rbMillimeters = new StiRadioButtonUX();
		rbInch = new StiRadioButtonUX();
		gbScript = new StiOffice2013GroupBox();
		labelLanguageJS = new StiLabelUX();
		rbCSharp = new StiRadioButtonUX();
		rbJS = new StiRadioButtonUX();
		rbVB = new StiRadioButtonUX();
		gbMainParameters = new StiOffice2013GroupBox();
		labelCalculationMode = new StiLabelUX();
		comboBoxCalculationMode = new StiComboBox();
		labelReportCacheMode = new StiLabelUX();
		labelNumberOfPass = new StiLabelUX();
		comboBoxReportCacheMode = new StiComboBox();
		comboBoxNumberOfPass = new StiComboBox();
		checkBoxConvertNulls = new StiCheckBoxUX();
		checkBoxCacheAllData = new StiCheckBoxUX();
		pageDescription = new StiCloudTabulatorPage();
		gbDates = new StiOffice2013GroupBox();
		lbRChanged = new StiLabelUX();
		lbRCreated = new StiLabelUX();
		lbReportChanged = new StiLabelUX();
		lbReportCreated = new StiLabelUX();
		gbDescription = new StiOffice2013GroupBox();
		ebDescription = new StiTextBoxUX();
		gbNames = new StiOffice2013GroupBox();
		ebAuthor = new StiTextBox();
		ebName = new StiTextBox();
		ebAlias = new StiTextBox();
		lbName = new StiLabelUX();
		lbAlias = new StiLabelUX();
		lbAuthor = new StiLabelUX();
		footerPanel = new StiButtonPanelUX();
		((Control)tabControl).SuspendLayout();
		((Control)pageMain).SuspendLayout();
		((Control)gbUnit).SuspendLayout();
		((Control)gbScript).SuspendLayout();
		((Control)gbMainParameters).SuspendLayout();
		((Control)pageDescription).SuspendLayout();
		((Control)gbDates).SuspendLayout();
		((Control)gbDescription).SuspendLayout();
		((Control)gbNames).SuspendLayout();
		((Control)footerPanel).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)btOk).Anchor = (AnchorStyles)10;
		btOk.FlatStyle = (FlatStyle)0;
		((Control)btOk).Location = new Point(425, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 2;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += btOk_Click;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		((Button)btCancel).DialogResult = (DialogResult)2;
		btCancel.FlatStyle = (FlatStyle)0;
		((Control)btCancel).Location = new Point(506, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 3;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)tabControl).Controls.Add((Control)(object)pageMain);
		((Control)tabControl).Controls.Add((Control)(object)pageDescription);
		((Control)tabControl).Dock = (DockStyle)5;
		((Control)tabControl).Location = new Point(0, 0);
		((Control)tabControl).Margin = new Padding(0, 3, 3, 3);
		((Control)tabControl).Name = "tabControl";
		((Control)tabControl).Padding = new Padding(0, 0, 119, 0);
		tabControl.ScrollHeight = 0;
		tabControl.ScrollWidth = 100;
		((Control)tabControl).Size = new Size(594, 481);
		((Control)tabControl).TabIndex = 59;
		tabControl.TitleBackColor = SystemColors.Control;
		tabControl.TitleInLine = false;
		tabControl.TitlePosition = StiTabTitlePosition.RightHorizontal;
		tabControl.TitleWidth = 110;
		((Control)pageMain).Controls.Add((Control)(object)gbUnit);
		((Control)pageMain).Controls.Add((Control)(object)gbScript);
		((Control)pageMain).Controls.Add((Control)(object)gbMainParameters);
		pageMain.Dock = (DockStyle)5;
		pageMain.Image = (Image)componentResourceManager.GetObject("pageMain.Image");
		pageMain.InvisibleVisible = false;
		((Control)pageMain).Location = new Point(0, 0);
		((Control)pageMain).Name = "pageMain";
		((Control)pageMain).Padding = new Padding(0, 0, 1, 0);
		((Control)pageMain).Size = new Size(475, 481);
		((Control)pageMain).TabIndex = 0;
		((Control)pageMain).Text = "Main";
		((Control)pageMain).Visible = false;
		((Control)gbUnit).Controls.Add((Control)(object)rbCentimetres);
		((Control)gbUnit).Controls.Add((Control)(object)rbHundredthsOfInch);
		((Control)gbUnit).Controls.Add((Control)(object)rbMillimeters);
		((Control)gbUnit).Controls.Add((Control)(object)rbInch);
		((Control)gbUnit).Dock = (DockStyle)1;
		gbUnit.HeaderHeight = 24;
		((Control)gbUnit).Location = new Point(0, 324);
		((Control)gbUnit).Name = "gbUnit";
		((Control)gbUnit).Size = new Size(474, 154);
		((Control)gbUnit).TabIndex = 2;
		((Control)gbUnit).TabStop = false;
		((Control)gbUnit).Text = "Size and coordinates in a report will be in specified units";
		((Control)rbCentimetres).AutoSize = true;
		((Control)rbCentimetres).Location = new Point(15, 39);
		((Control)rbCentimetres).Name = "rbCentimetres";
		((Control)rbCentimetres).Size = new Size(80, 17);
		((Control)rbCentimetres).TabIndex = 4;
		((Control)rbCentimetres).Text = "Centimetres";
		((ButtonBase)rbCentimetres).UseVisualStyleBackColor = false;
		((Control)rbHundredthsOfInch).AutoSize = true;
		((Control)rbHundredthsOfInch).Location = new Point(15, 126);
		((Control)rbHundredthsOfInch).Name = "rbHundredthsOfInch";
		((Control)rbHundredthsOfInch).Size = new Size(127, 17);
		((Control)rbHundredthsOfInch).TabIndex = 7;
		((Control)rbHundredthsOfInch).Text = "Hundredths of Inches";
		((ButtonBase)rbHundredthsOfInch).UseVisualStyleBackColor = false;
		((Control)rbMillimeters).AutoSize = true;
		((Control)rbMillimeters).Location = new Point(15, 68);
		((Control)rbMillimeters).Name = "rbMillimeters";
		((Control)rbMillimeters).Size = new Size(73, 17);
		((Control)rbMillimeters).TabIndex = 5;
		((Control)rbMillimeters).Text = "Millimeters";
		((ButtonBase)rbMillimeters).UseVisualStyleBackColor = false;
		((Control)rbInch).AutoSize = true;
		((Control)rbInch).Location = new Point(15, 97);
		((Control)rbInch).Name = "rbInch";
		((Control)rbInch).Size = new Size(57, 17);
		((Control)rbInch).TabIndex = 6;
		((Control)rbInch).Text = "Inches";
		((ButtonBase)rbInch).UseVisualStyleBackColor = false;
		((Control)gbScript).Controls.Add((Control)(object)labelLanguageJS);
		((Control)gbScript).Controls.Add((Control)(object)rbCSharp);
		((Control)gbScript).Controls.Add((Control)(object)rbJS);
		((Control)gbScript).Controls.Add((Control)(object)rbVB);
		((Control)gbScript).Dock = (DockStyle)1;
		gbScript.HeaderHeight = 24;
		((Control)gbScript).Location = new Point(0, 198);
		((Control)gbScript).Name = "gbScript";
		((Control)gbScript).Size = new Size(474, 126);
		((Control)gbScript).TabIndex = 1;
		((Control)gbScript).TabStop = false;
		((Control)gbScript).Text = "Script language of your report";
		((Control)labelLanguageJS).AutoSize = true;
		((Control)labelLanguageJS).Font = new Font("Microsoft Sans Serif", 6.75f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)labelLanguageJS).Location = new Point(49, 99);
		((Control)labelLanguageJS).Name = "labelLanguageJS";
		((Control)labelLanguageJS).Size = new Size(86, 12);
		((Control)labelLanguageJS).TabIndex = 3;
		((Control)labelLanguageJS).Text = "(Works for JS only!)";
		((Control)rbCSharp).AutoSize = true;
		((Control)rbCSharp).Location = new Point(15, 39);
		((Control)rbCSharp).Name = "rbCSharp";
		((Control)rbCSharp).Size = new Size(39, 17);
		((Control)rbCSharp).TabIndex = 0;
		((Control)rbCSharp).Text = "C#";
		((ButtonBase)rbCSharp).UseVisualStyleBackColor = false;
		((Control)rbJS).AutoSize = true;
		((Control)rbJS).BackColor = Color.White;
		((Control)rbJS).Location = new Point(15, 97);
		((Control)rbJS).Name = "rbJS";
		((Control)rbJS).Size = new Size(37, 17);
		((Control)rbJS).TabIndex = 2;
		((Control)rbJS).Text = "JS";
		((ButtonBase)rbJS).UseVisualStyleBackColor = false;
		((Control)rbVB).AutoSize = true;
		((Control)rbVB).Location = new Point(15, 68);
		((Control)rbVB).Name = "rbVB";
		((Control)rbVB).Size = new Size(59, 17);
		((Control)rbVB).TabIndex = 1;
		((Control)rbVB).Text = "VB.Net";
		((ButtonBase)rbVB).UseVisualStyleBackColor = false;
		((Control)gbMainParameters).Controls.Add((Control)(object)labelCalculationMode);
		((Control)gbMainParameters).Controls.Add((Control)(object)comboBoxCalculationMode);
		((Control)gbMainParameters).Controls.Add((Control)(object)labelReportCacheMode);
		((Control)gbMainParameters).Controls.Add((Control)(object)labelNumberOfPass);
		((Control)gbMainParameters).Controls.Add((Control)(object)comboBoxReportCacheMode);
		((Control)gbMainParameters).Controls.Add((Control)(object)comboBoxNumberOfPass);
		((Control)gbMainParameters).Controls.Add((Control)(object)checkBoxConvertNulls);
		((Control)gbMainParameters).Controls.Add((Control)(object)checkBoxCacheAllData);
		((Control)gbMainParameters).Dock = (DockStyle)1;
		gbMainParameters.HeaderHeight = 24;
		((Control)gbMainParameters).Location = new Point(0, 0);
		((Control)gbMainParameters).Name = "gbMainParameters";
		((Control)gbMainParameters).Size = new Size(474, 198);
		((Control)gbMainParameters).TabIndex = 0;
		((Control)gbMainParameters).TabStop = false;
		((Control)gbMainParameters).Text = "Parameters which effect on report rendering";
		((Control)labelCalculationMode).Location = new Point(12, 163);
		((Control)labelCalculationMode).Name = "labelCalculationMode";
		((Control)labelCalculationMode).Size = new Size(195, 21);
		((Control)labelCalculationMode).TabIndex = 6;
		((Control)labelCalculationMode).Text = "Calculation Mode";
		((Label)labelCalculationMode).TextAlign = (ContentAlignment)16;
		comboBoxCalculationMode.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxCalculationMode).FormattingEnabled = true;
		((Control)comboBoxCalculationMode).Location = new Point(213, 163);
		((Control)comboBoxCalculationMode).Name = "comboBoxCalculationMode";
		((Control)comboBoxCalculationMode).Size = new Size(180, 21);
		((Control)comboBoxCalculationMode).TabIndex = 7;
		((Control)labelReportCacheMode).Location = new Point(12, 130);
		((Control)labelReportCacheMode).Name = "labelReportCacheMode";
		((Control)labelReportCacheMode).Size = new Size(195, 21);
		((Control)labelReportCacheMode).TabIndex = 4;
		((Control)labelReportCacheMode).Text = "Report Cache Mode";
		((Label)labelReportCacheMode).TextAlign = (ContentAlignment)16;
		((Control)labelNumberOfPass).Location = new Point(12, 97);
		((Control)labelNumberOfPass).Name = "labelNumberOfPass";
		((Control)labelNumberOfPass).Size = new Size(199, 21);
		((Control)labelNumberOfPass).TabIndex = 2;
		((Control)labelNumberOfPass).Text = "Number of Pass";
		((Label)labelNumberOfPass).TextAlign = (ContentAlignment)16;
		comboBoxReportCacheMode.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxReportCacheMode).FormattingEnabled = true;
		((Control)comboBoxReportCacheMode).Location = new Point(213, 130);
		((Control)comboBoxReportCacheMode).Name = "comboBoxReportCacheMode";
		((Control)comboBoxReportCacheMode).Size = new Size(180, 21);
		((Control)comboBoxReportCacheMode).TabIndex = 5;
		comboBoxNumberOfPass.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxNumberOfPass).FormattingEnabled = true;
		((Control)comboBoxNumberOfPass).Location = new Point(213, 97);
		((Control)comboBoxNumberOfPass).Name = "comboBoxNumberOfPass";
		((Control)comboBoxNumberOfPass).Size = new Size(180, 21);
		((Control)comboBoxNumberOfPass).TabIndex = 3;
		((Control)checkBoxConvertNulls).AutoSize = true;
		((Control)checkBoxConvertNulls).Location = new Point(12, 68);
		((Control)checkBoxConvertNulls).Name = "checkBoxConvertNulls";
		((Control)checkBoxConvertNulls).Size = new Size(89, 17);
		((Control)checkBoxConvertNulls).TabIndex = 1;
		((Control)checkBoxConvertNulls).Text = "Convert Nulls";
		((ButtonBase)checkBoxConvertNulls).UseVisualStyleBackColor = false;
		((Control)checkBoxCacheAllData).AutoSize = true;
		((Control)checkBoxCacheAllData).Location = new Point(12, 39);
		((Control)checkBoxCacheAllData).Name = "checkBoxCacheAllData";
		((Control)checkBoxCacheAllData).Size = new Size(97, 17);
		((Control)checkBoxCacheAllData).TabIndex = 0;
		((Control)checkBoxCacheAllData).Text = "Cache All Data";
		((ButtonBase)checkBoxCacheAllData).UseVisualStyleBackColor = false;
		((Control)pageDescription).Controls.Add((Control)(object)gbDates);
		((Control)pageDescription).Controls.Add((Control)(object)gbDescription);
		((Control)pageDescription).Controls.Add((Control)(object)gbNames);
		pageDescription.Dock = (DockStyle)5;
		pageDescription.Image = (Image)componentResourceManager.GetObject("pageDescription.Image");
		pageDescription.InvisibleVisible = true;
		((Control)pageDescription).Location = new Point(0, 0);
		((Control)pageDescription).Name = "pageDescription";
		((Control)pageDescription).Padding = new Padding(0, 0, 1, 0);
		((Control)pageDescription).Size = new Size(475, 481);
		((Control)pageDescription).TabIndex = 0;
		((Control)pageDescription).Text = "Description";
		((Control)pageDescription).Visible = false;
		((Control)gbDates).Controls.Add((Control)(object)lbRChanged);
		((Control)gbDates).Controls.Add((Control)(object)lbRCreated);
		((Control)gbDates).Controls.Add((Control)(object)lbReportChanged);
		((Control)gbDates).Controls.Add((Control)(object)lbReportCreated);
		((Control)gbDates).Dock = (DockStyle)1;
		gbDates.HeaderHeight = 24;
		((Control)gbDates).Location = new Point(0, 271);
		((Control)gbDates).Name = "gbDates";
		((Control)gbDates).Size = new Size(474, 99);
		((Control)gbDates).TabIndex = 2;
		((Control)gbDates).TabStop = false;
		((Control)gbDates).Text = "A date of report creation and a date of the last report change";
		((Control)lbRChanged).BackColor = Color.WhiteSmoke;
		((Control)lbRChanged).Location = new Point(157, 71);
		((Control)lbRChanged).Margin = new Padding(3);
		((Control)lbRChanged).Name = "lbRChanged";
		((Control)lbRChanged).Size = new Size(302, 22);
		((Control)lbRChanged).TabIndex = 9;
		((Control)lbRChanged).Text = "StiTextBox3";
		((Label)lbRChanged).TextAlign = (ContentAlignment)16;
		((Control)lbRCreated).BackColor = Color.WhiteSmoke;
		((Control)lbRCreated).Location = new Point(157, 39);
		((Control)lbRCreated).Margin = new Padding(3);
		((Control)lbRCreated).Name = "lbRCreated";
		((Control)lbRCreated).Size = new Size(302, 22);
		((Control)lbRCreated).TabIndex = 8;
		((Control)lbRCreated).Text = "StiTextBox3";
		((Label)lbRCreated).TextAlign = (ContentAlignment)16;
		((Control)lbReportChanged).Location = new Point(12, 71);
		((Control)lbReportChanged).Name = "lbReportChanged";
		((Control)lbReportChanged).Size = new Size(139, 20);
		((Control)lbReportChanged).TabIndex = 7;
		((Control)lbReportChanged).Text = "ReportChanged";
		((Label)lbReportChanged).TextAlign = (ContentAlignment)16;
		((Control)lbReportCreated).Location = new Point(12, 39);
		((Control)lbReportCreated).Name = "lbReportCreated";
		((Control)lbReportCreated).Size = new Size(139, 20);
		((Control)lbReportCreated).TabIndex = 6;
		((Control)lbReportCreated).Text = "ReportCreated";
		((Label)lbReportCreated).TextAlign = (ContentAlignment)16;
		((Control)gbDescription).Controls.Add((Control)(object)ebDescription);
		((Control)gbDescription).Dock = (DockStyle)1;
		gbDescription.HeaderHeight = 24;
		((Control)gbDescription).Location = new Point(0, 139);
		((Control)gbDescription).Name = "gbDescription";
		((Control)gbDescription).Size = new Size(474, 132);
		((Control)gbDescription).TabIndex = 1;
		((Control)gbDescription).TabStop = false;
		((Control)gbDescription).Text = "Report description";
		((Control)ebDescription).ForeColor = SystemColors.ControlText;
		((Control)ebDescription).Location = new Point(12, 39);
		((TextBoxBase)ebDescription).Multiline = true;
		((Control)ebDescription).Name = "ebDescription";
		((TextBox)ebDescription).ScrollBars = (ScrollBars)2;
		((Control)ebDescription).Size = new Size(449, 80);
		((Control)ebDescription).TabIndex = 4;
		((Control)ebDescription).Text = "StiTextBox4";
		((Control)gbNames).Controls.Add((Control)(object)ebAuthor);
		((Control)gbNames).Controls.Add((Control)(object)ebName);
		((Control)gbNames).Controls.Add((Control)(object)ebAlias);
		((Control)gbNames).Controls.Add((Control)(object)lbName);
		((Control)gbNames).Controls.Add((Control)(object)lbAlias);
		((Control)gbNames).Controls.Add((Control)(object)lbAuthor);
		((Control)gbNames).Dock = (DockStyle)1;
		gbNames.HeaderHeight = 24;
		((Control)gbNames).Location = new Point(0, 0);
		((Control)gbNames).Name = "gbNames";
		((Control)gbNames).Size = new Size(474, 139);
		((Control)gbNames).TabIndex = 0;
		((Control)gbNames).TabStop = false;
		((Control)gbNames).Text = "Report name, report alias, and report author";
		((Control)ebAuthor).Location = new Point(157, 103);
		((Control)ebAuthor).Name = "ebAuthor";
		((Control)ebAuthor).Size = new Size(302, 21);
		((Control)ebAuthor).TabIndex = 8;
		((Control)ebAuthor).Text = "StiTextBox3";
		((Control)ebName).Location = new Point(157, 39);
		((Control)ebName).Name = "ebName";
		((Control)ebName).Size = new Size(302, 21);
		((Control)ebName).TabIndex = 0;
		((Control)ebName).Text = "StiTextBox1";
		((Control)ebName).Enter += ebName_Enter;
		((Control)ebName).Leave += ebName_Leave;
		((Control)ebAlias).Location = new Point(157, 71);
		((Control)ebAlias).Name = "ebAlias";
		((Control)ebAlias).Size = new Size(302, 21);
		((Control)ebAlias).TabIndex = 1;
		((Control)ebAlias).Text = "StiTextBox2";
		((Control)lbName).Location = new Point(12, 39);
		((Control)lbName).Name = "lbName";
		((Control)lbName).Size = new Size(139, 20);
		((Control)lbName).TabIndex = 9;
		((Control)lbName).Text = "Name";
		((Label)lbName).TextAlign = (ContentAlignment)16;
		((Control)lbAlias).Location = new Point(12, 71);
		((Control)lbAlias).Name = "lbAlias";
		((Control)lbAlias).Size = new Size(139, 20);
		((Control)lbAlias).TabIndex = 10;
		((Control)lbAlias).Text = "Alias";
		((Label)lbAlias).TextAlign = (ContentAlignment)16;
		((Control)lbAuthor).Location = new Point(12, 103);
		((Control)lbAuthor).Name = "lbAuthor";
		((Control)lbAuthor).Size = new Size(139, 20);
		((Control)lbAuthor).TabIndex = 11;
		((Control)lbAuthor).Text = "Author";
		((Label)lbAuthor).TextAlign = (ContentAlignment)16;
		((Control)footerPanel).Controls.Add((Control)(object)btCancel);
		((Control)footerPanel).Controls.Add((Control)(object)btOk);
		((Control)footerPanel).Dock = (DockStyle)2;
		footerPanel.LinePosition = StiLinePosition.Top;
		footerPanel.LineStyle = StiLineStyle.Dot;
		((Control)footerPanel).Location = new Point(0, 481);
		((Control)footerPanel).Name = "footerPanel";
		((Control)footerPanel).Size = new Size(594, 48);
		((Control)footerPanel).TabIndex = 0;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(594, 529);
		((Control)this).Controls.Add((Control)(object)tabControl);
		((Control)this).Controls.Add((Control)(object)footerPanel);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiReportSetupForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiReportSetupForm";
		((Form)this).HelpButtonClicked += ThisHelpButtonClicked;
		((Control)tabControl).ResumeLayout(false);
		((Control)pageMain).ResumeLayout(false);
		((Control)gbUnit).ResumeLayout(false);
		((Control)gbUnit).PerformLayout();
		((Control)gbScript).ResumeLayout(false);
		((Control)gbScript).PerformLayout();
		((Control)gbMainParameters).ResumeLayout(false);
		((Control)gbMainParameters).PerformLayout();
		((Control)pageDescription).ResumeLayout(false);
		((Control)gbDates).ResumeLayout(false);
		((Control)gbDescription).ResumeLayout(false);
		((Control)gbDescription).PerformLayout();
		((Control)gbNames).ResumeLayout(false);
		((Control)footerPanel).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
