using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Server;
using Stimulsoft.Base.TaskScheduler;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Report.Design;

public class StiOptionsForm : StiFormUX
{
	private StiDesignerControl designer;

	private IContainer components;

	private StiCloudTabulator TabControl;

	private StiCloudTabulatorPage pageMain;

	private StiCloudTabulatorPage pageDesign;

	private StiCloudTabulatorPage pageQuickInfo;

	private StiCloudTabulatorPage pageScheduler;

	private StiCloudTabulatorPage pageSaving;

	private StiOffice2013GroupBox gbGridOptions;

	private StiOffice2013GroupBox gbGridSize;

	private StiOffice2013GroupBox gbGridDrawingOptions;

	private StiCheckBoxUX cbShowGrid;

	private StiCheckBoxUX cbAlignToGrid;

	private StiRadioButtonUX rbGridLines;

	private StiRadioButtonUX rbGridDots;

	private StiLabelUX lbPixels;

	private StiLabelUX label6;

	private StiTextBox ebPixels;

	private StiLabelUX lbMillimeters;

	private StiLabelUX lbCentimetres;

	private StiLabelUX lbHundredthsOfInch;

	private StiLabelUX labelSchedulerSenderEmail;

	private StiTextBox textBoxSchedulerSenderEmail;

	private StiLabelUX labelSchedulerHost;

	private StiTextBox textBoxSchedulerHost;

	private StiLabelUX labelSchedulerPort;

	private StiNumericUpDown textBoxSchedulerPort;

	private StiLabelUX labelSchedulerUserName;

	private StiTextBox textBoxSchedulerUserName;

	private StiLabelUX labelSchedulerPassword;

	private StiTextBox textBoxSchedulerPassword;

	private StiCheckBoxUX checkBoxSchedulerEnableSSL;

	private StiLabelUX lbInch;

	private StiLabelUX label4;

	private StiLabelUX label3;

	private StiLabelUX label2;

	private StiTextBox ebMillimeters;

	private StiTextBox ebCentimetres;

	private StiTextBox ebHundredthsOfInch;

	private StiTextBox ebInch;

	private StiLabelUX label1;

	private StiOffice2013GroupBox gbMainOptions;

	private StiCheckBoxUX checkBoxGenerateLocalizedName;

	private StiCheckBoxUX cbRunDesignerAfterInsert;

	private StiCheckBoxUX cbUseLastFormat;

	private StiOffice2013GroupBox gbScheduleSmtpSettings;

	private StiOffice2013GroupBox gbOptionsOfQuickInfo;

	private StiRadioButtonUX rbQuickInfoNone;

	private StiRadioButtonUX rbQuickInfoShowComponentsNames;

	private StiRadioButtonUX rbQuickInfoShowFieldsOnly;

	private StiRadioButtonUX rbQuickInfoShowFields;

	private StiRadioButtonUX rbQuickInfoShowContent;

	private StiRadioButtonUX rbQuickInfoShowEvents;

	private StiCheckBoxUX cbShowQuickInfoOverlay;

	private StiOffice2013GroupBox gbReportDisplayOptions;

	private StiCheckBoxUX cbDrawMarkersWhenMoving;

	private StiCheckBoxUX cbFillComponents;

	private StiCheckBoxUX cbFillBands;

	private StiCheckBoxUX cbUseComponentColor;

	private StiCheckBoxUX cbFillCrossBands;

	private StiCheckBoxUX cbFillContainers;

	private StiOffice2013GroupBox gbMarkersStyle;

	private StiRadioButtonUX rbMarkersStyleNone;

	private StiRadioButtonUX rbMarkersStyleCorners;

	private StiRadioButtonUX rbMarkersStyleDashedRectangle;

	private StiOffice2013GroupBox gbAutoSaveOptions;

	private StiCheckBoxUX checkBoxEnableAutoSave;

	private StiLabelUX labelSaveReportEvery;

	private StiComboBox comboBoxAutoSaveTime;

	private StiButtonUX btCancel;

	private StiButtonUX btOk;

	private StiButtonUX btRestoreDefaults;

	private StiButtonPanelUX panelFooter;

	private StiGroupLine quickInfoSeparator;

	private StiCheckBoxUX checkBoxShowDialogForms;

	private StiLabelUX labelAnimationPlaybackType;

	private ButtonX buttonAnimationPlaybackType;

	private ButtonItem buttonAnimationPlaybackTypeOnStart;

	private ButtonItem buttonAnimationPlaybackTypeOnPreview;

	private StiCheckBoxUX checkBoxShowInsertTab;

	private StiCheckBoxUX checkBoxShowToolbox;

	private StiComboBox comboBoxGaugeEditor;

	private StiLabelUX labelGaugeEditor;

	private StiComboBox comboBoxStartScreen;

	private StiLabelUX labelStartScreen;

	private StiPanelUX panelDeveloperCreator1;

	private StiPanelUX panelDeveloper1;

	private StiPanelUX panelStartScreen;

	private StiPanelUX panelScalingMode;

	private StiLabelUX labelDesignerScaleMode;

	private StiComboBox comboBoxDesignerScaleMode;

	private StiComboBox comboBoxChartEditor;

	private StiLabelUX labelChartEditor;

	private StiPanelUX panelNewReport;

	private StiLabelUX labelNewReportDictionaryBehavior;

	private StiComboBox comboBoxNewReportDictionaryBehavior;

	private StiPanelUX panelDefaultScriptMode;

	private StiLabelUX labelDefaultScriptMode;

	private StiComboBox comboBoxDefaultScriptMode;

	private StiOffice2013GroupBox gbSaveOther;

	private StiCheckBoxUX cbAutoSaveReport;

	private StiCheckBoxUX checkBoxRequestChanges;

	private StiOffice2013GroupBox gbViewOptions;

	private StiCheckBoxUX cbShowHeaders;

	private StiCheckBoxUX cbShowOrder;

	private StiCheckBoxUX cbShowRulers;

	private StiCheckBoxUX checkBoxShowDimensionLines;

	public StiOptionsForm(StiDesignerControl designer)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (((Component)(object)this).DesignMode)
		{
			return;
		}
		((Form)this).ShowIcon = false;
		if (StiOptions.Appearance.ShowFormsHelp)
		{
			((Form)this).HelpButton = true;
		}
		this.designer = designer;
		Localize();
		DoScaling();
		Build(designer.Info, StiSmtpConfig.Get());
		((CheckBox)checkBoxEnableAutoSave).Checked = designer.Info.EnableAutoSaveMode;
		int num = 0;
		int[] autoSaveIntervals = StiOptions.Designer.AutoSaveIntervals;
		foreach (int num2 in autoSaveIntervals)
		{
			if (num2 == designer.Info.AutoSaveInterval)
			{
				((ListControl)comboBoxAutoSaveTime).SelectedIndex = num;
			}
			num++;
		}
		((Control)comboBoxAutoSaveTime).Enabled = ((CheckBox)checkBoxEnableAutoSave).Checked;
		pageScheduler.Invisible = StiTaskSchedulerSettings.Current == null;
		TabControl.SelectedIndex = 0;
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("FormOptions", "title");
		((Control)labelSchedulerSenderEmail).Text = StiLocalization.Get("Scheduler", "SenderEmail");
		((Control)labelSchedulerHost).Text = StiLocalization.Get("Scheduler", "Host");
		((Control)labelSchedulerPort).Text = StiLocalization.Get("Cloud", "Port");
		((Control)labelSchedulerUserName).Text = StiLocalization.Get("Cloud", "labelUserName").Replace(":", "");
		((Control)labelSchedulerPassword).Text = StiLocalization.Get("Cloud", "labelPassword").Replace(":", "");
		((Control)checkBoxSchedulerEnableSSL).Text = StiLocalization.Get("Scheduler", "EnableSSL").Replace(":", "");
		((Control)labelChartEditor).Text = StiLocalization.Get("Chart", "ChartEditorForm");
		((ComboBox)comboBoxChartEditor).Items.Add((object)StiLocalization.Get("PropertyMain", "Simple"));
		((ComboBox)comboBoxChartEditor).Items.Add((object)StiLocalization.Get("PropertyMain", "Advanced"));
		((Control)labelGaugeEditor).Text = StiLocalization.Get("Gauge", "GaugeEditorForm");
		((ComboBox)comboBoxGaugeEditor).Items.Add((object)StiLocalization.Get("PropertyMain", "Simple"));
		((ComboBox)comboBoxGaugeEditor).Items.Add((object)StiLocalization.Get("PropertyMain", "Advanced"));
		((Control)labelDefaultScriptMode).Text = StiLocalization.Get("FormOptions", "DefaultScriptMode");
		((ComboBox)comboBoxDefaultScriptMode).Items.Add((object)"Blocks");
		((ComboBox)comboBoxDefaultScriptMode).Items.Add((object)StiLocalization.Get("FormDesigner", "Code"));
		((Control)labelStartScreen).Text = StiLocalization.Get("FormOptions", "StartScreen");
		((ComboBox)comboBoxStartScreen).Items.Add((object)StiLocalization.Get("FormOptions", "Welcome"));
		((ComboBox)comboBoxStartScreen).Items.Add((object)StiLocalization.Get("FormOptions", "BlankReport"));
		((ComboBox)comboBoxStartScreen).Items.Add((object)StiLocalization.Get("FormOptions", "BlankDashboard"));
		((Control)labelDesignerScaleMode).Text = StiLocalization.Get("FormOptions", "ScaleMode");
		((ComboBox)comboBoxDesignerScaleMode).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiDesignerScaleModeAutomaticScaling"));
		((ComboBox)comboBoxDesignerScaleMode).Items.Add((object)StiLocalization.Get("PropertyEnum", "StiDesignerScaleModeScaling100"));
		((Control)labelNewReportDictionaryBehavior).Text = StiLocalization.Get("Wizards", "title");
		((ComboBox)comboBoxNewReportDictionaryBehavior).Items.Add((object)StiLocalization.Get("FormDictionaryDesigner", "DictionaryMerge").Replace("...", ""));
		((ComboBox)comboBoxNewReportDictionaryBehavior).Items.Add((object)StiLocalization.Get("FormDictionaryDesigner", "DictionaryNew").Replace("...", ""));
		((Control)pageMain).Text = StiLocalization.Get("FormOptions", "Main");
		((Control)pageDesign).Text = StiLocalization.Get("Buttons", "Design").Replace("&", "");
		((Control)pageQuickInfo).Text = StiLocalization.Get("MainMenu", "menuViewQuickInfo").Replace("&", "");
		((Control)pageScheduler).Text = StiLocalization.Get("Scheduler", "Scheduler");
		((Control)pageSaving).Text = StiLocalization.Get("DesignerFx", "Saving");
		((Control)gbViewOptions).Text = StiLocalization.Get("Toolbars", "ToolbarViewOptions");
		((Control)gbSaveOther).Text = StiLocalization.Get("FormDesigner", "Others");
		((Control)gbScheduleSmtpSettings).Text = StiLocalization.Get("Scheduler", "SmtpSettings");
		((Control)btRestoreDefaults).Text = StiLocalization.Get("Buttons", "RestoreDefaults");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((Control)btRestoreDefaults).Width = TextRenderer.MeasureText(((Control)btRestoreDefaults).Text, ((Control)btRestoreDefaults).Font).Width + StiScale.I(20);
		((Control)lbCentimetres).Text = Loc.GetCleaned("Report", "LabelCentimeters");
		((Control)lbHundredthsOfInch).Text = Loc.GetCleaned("Report", "LabelHundredthsOfInch");
		((Control)lbInch).Text = Loc.GetCleaned("Report", "LabelInches");
		((Control)lbMillimeters).Text = Loc.GetCleaned("Report", "LabelMillimeters");
		((Control)lbPixels).Text = Loc.GetCleaned("Report", "LabelPixels");
		((Control)rbQuickInfoNone).Text = StiLocalization.Get("MainMenu", "menuViewQuickInfoNone");
		((Control)rbQuickInfoShowComponentsNames).Text = StiLocalization.Get("MainMenu", "menuViewQuickInfoShowComponentsNames");
		((Control)rbQuickInfoShowContent).Text = StiLocalization.Get("MainMenu", "menuViewQuickInfoShowContent");
		((Control)rbQuickInfoShowFields).Text = StiLocalization.Get("MainMenu", "menuViewQuickInfoShowFields");
		((Control)rbQuickInfoShowFieldsOnly).Text = StiLocalization.Get("MainMenu", "menuViewQuickInfoShowFieldsOnly");
		((Control)rbQuickInfoShowEvents).Text = StiLocalization.Get("MainMenu", "menuViewQuickInfoShowEvents");
		((Control)cbShowQuickInfoOverlay).Text = StiLocalization.Get("MainMenu", "menuViewQuickInfoOverlay");
		((Control)gbGridSize).Text = StiLocalization.Get("FormOptions", "GridSize");
		((Control)cbAlignToGrid).Text = StiLocalization.Get("MainMenu", "menuViewAlignToGrid");
		((Control)cbShowGrid).Text = StiLocalization.Get("MainMenu", "menuViewShowGrid");
		((Control)cbShowHeaders).Text = StiLocalization.Get("MainMenu", "menuViewShowHeaders");
		((Control)cbShowOrder).Text = StiLocalization.Get("MainMenu", "menuViewShowOrder");
		((Control)cbShowRulers).Text = StiLocalization.Get("MainMenu", "menuViewShowRulers");
		((Control)checkBoxShowToolbox).Text = StiLocalization.Get("MainMenu", "menuViewShowToolbox");
		((Control)checkBoxShowInsertTab).Text = StiLocalization.Get("MainMenu", "menuViewShowInsertTab");
		((Control)cbRunDesignerAfterInsert).Text = StiLocalization.Get("FormOptions", "EditAfterInsert");
		((Control)cbUseLastFormat).Text = StiLocalization.Get("FormOptions", "UseLastFormat");
		((Control)cbAutoSaveReport).Text = StiLocalization.Get("FormOptions", "AutoSaveReportToReportClass");
		((Control)checkBoxShowDimensionLines).Text = StiLocalization.Get("FormOptions", "ShowDimensionLines");
		((Control)checkBoxGenerateLocalizedName).Text = StiLocalization.Get("FormOptions", "GenerateLocalizedName");
		((Control)checkBoxRequestChanges).Text = StiLocalization.Get("Cloud", "RequestChangesWhenSavingToCloud");
		((Control)checkBoxShowDialogForms).Text = StiLocalization.Get("FormOptions", "ShowDialogForms") + " (" + StiLocalization.Get("Report", "Obsoleted") + ")";
		((Control)labelAnimationPlaybackType).Text = StiLocalization.Get("Export", "EnableAnimation");
		buttonAnimationPlaybackTypeOnStart.Text = StiLocalization.Get("PropertyEnum", "StiFormStartModeOnStart");
		buttonAnimationPlaybackTypeOnPreview.Text = StiLocalization.Get("PropertyEnum", "StiFormStartModeOnPreview");
		((Control)cbFillBands).Text = StiLocalization.Get("FormOptions", "FillBands");
		((Control)cbFillCrossBands).Text = StiLocalization.Get("FormOptions", "FillCrossBands");
		((Control)cbFillContainers).Text = StiLocalization.Get("FormOptions", "FillContainers");
		((Control)cbFillComponents).Text = StiLocalization.Get("FormOptions", "FillComponents");
		((Control)cbUseComponentColor).Text = StiLocalization.Get("FormOptions", "UseComponentColor");
		((Control)rbGridLines).Text = StiLocalization.Get("FormOptions", "GridLines");
		((Control)rbGridDots).Text = StiLocalization.Get("FormOptions", "GridDots");
		((Control)cbDrawMarkersWhenMoving).Text = StiLocalization.Get("FormOptions", "DrawMarkersWhenMoving");
		((Control)gbMarkersStyle).Text = StiLocalization.Get("FormOptions", "MarkersStyle");
		((Control)rbMarkersStyleNone).Text = StiLocalization.Get("FormOptions", "MarkersStyleNone");
		((Control)rbMarkersStyleCorners).Text = StiLocalization.Get("FormOptions", "MarkersStyleCorners");
		((Control)rbMarkersStyleDashedRectangle).Text = StiLocalization.Get("FormOptions", "MarkersStyleDashedRectangle");
		((Control)checkBoxEnableAutoSave).Text = StiLocalization.Get("FormOptions", "EnableAutoSaveMode");
		((Control)labelSaveReportEvery).Text = Loc.GetCleaned("FormOptions", "SaveReportEvery");
		((ComboBox)comboBoxAutoSaveTime).Items.Clear();
		int[] autoSaveIntervals = StiOptions.Designer.AutoSaveIntervals;
		foreach (int num in autoSaveIntervals)
		{
			((ComboBox)comboBoxAutoSaveTime).Items.Add((object)string.Format(StiLocalization.Get("FormOptions", "Minutes"), num));
		}
		((Control)gbAutoSaveOptions).Text = StiLocalization.Get("FormOptions", "groupAutoSaveOptions");
		((Control)gbGridDrawingOptions).Text = StiLocalization.Get("FormOptions", "groupGridDrawingOptions");
		((Control)gbGridOptions).Text = StiLocalization.Get("FormOptions", "groupGridOptions");
		((Control)gbGridSize).Text = StiLocalization.Get("FormOptions", "groupGridSize");
		((Control)gbMainOptions).Text = StiLocalization.Get("FormOptions", "groupMainOptions");
		((Control)gbMarkersStyle).Text = StiLocalization.Get("FormOptions", "groupMarkersStyle");
		((Control)gbOptionsOfQuickInfo).Text = StiLocalization.Get("FormOptions", "groupOptionsOfQuickInfo");
		((Control)gbReportDisplayOptions).Text = StiLocalization.Get("FormOptions", "groupReportDisplayOptions");
	}

	private void DoScaling()
	{
		DockPaddingEdges dockPadding = TabControl.DockPadding;
		int right = (TabControl.TitleWidth = StiScale.XXI(110.0));
		dockPadding.Right = right;
		pageSaving.Image = StiReportDesignImages.Tasks.DesignerOptionAutoSave(StiImageSize.Double);
		pageDesign.Image = StiReportDesignImages.Tasks.DesignerOptionGrid(StiImageSize.Double);
		pageMain.Image = StiReportDesignImages.Tasks.DesignerOptionMain(StiImageSize.Double);
		pageQuickInfo.Image = StiReportDesignImages.Tasks.DesignerOptionQuickInfo(StiImageSize.Double);
		pageScheduler.Image = StiReportDesignImages.Schedule.Monthly();
		((Control)buttonAnimationPlaybackType).Size = ((Control)comboBoxChartEditor).Size;
	}

	private void Build(StiDesignerInfo info, StiSmtpConfig smtpConfig)
	{
		((Control)checkBoxRequestChanges).Visible = StiAccountSettings.CurrentAccount != null;
		if (!StiOptions.Designer.IsDeveloperDesigner)
		{
			((Control)TabControl).Controls.Remove((Control)(object)pageQuickInfo);
			((Control)panelDeveloper1).Visible = false;
		}
		((Control)panelNewReport).Visible = !StiOptions.Designer.IsBeginnerDesigner;
		StiSettings.Load();
		((CheckBox)checkBoxShowToolbox).Checked = StiSettings.GetBool("CloudDesigner", "ShowStandardToolbox", nullValue: true);
		((CheckBox)checkBoxShowInsertTab).Checked = StiSettings.GetBool("CloudDesigner", "ShowInsertTab", nullValue: true);
		((CheckBox)checkBoxRequestChanges).Checked = StiSettings.GetBool("Cloud", "RequestChangesWhenSavingToCloud", nullValue: true);
		((CheckBox)checkBoxShowDialogForms).Checked = StiSettings.GetBool("Designer", "ShowDialogFormsV1", nullValue: false);
		buttonAnimationPlaybackTypeOnStart.Checked = StiOptions.Viewer.Windows.AnimationPlaybackType.HasFlag(StiAnimationPlaybackType.OnStart);
		buttonAnimationPlaybackTypeOnPreview.Checked = StiOptions.Viewer.Windows.AnimationPlaybackType.HasFlag(StiAnimationPlaybackType.OnPreview);
		ButtonAnimationPlaybackTypeChange_Click(null, null);
		((ListControl)comboBoxChartEditor).SelectedIndex = ((!StiOptions.Designer.Editors.ChartEditor.SimpleEditorByDefault) ? 1 : 0);
		((ListControl)comboBoxGaugeEditor).SelectedIndex = (StiOptions.Designer.Editors.GaugeEditor.AllowOldEditor ? 1 : 0);
		((ListControl)comboBoxDefaultScriptMode).SelectedIndex = ((!StiOptions.Designer.Editors.ScriptEditor.BlocksScriptEditorByDefault) ? 1 : 0);
		((Control)textBoxSchedulerSenderEmail).Text = smtpConfig.SenderEmail ?? string.Empty;
		((Control)textBoxSchedulerHost).Text = smtpConfig.Host ?? string.Empty;
		textBoxSchedulerPort.Value = smtpConfig.Port;
		((Control)textBoxSchedulerUserName).Text = smtpConfig.UserName ?? string.Empty;
		((Control)textBoxSchedulerPassword).Text = smtpConfig.Password ?? string.Empty;
		((CheckBox)checkBoxSchedulerEnableSSL).Checked = smtpConfig.EnableSsl;
		switch (StiOptions.Designer.DesignerScale)
		{
		case StiDesignerScaleMode.AutomaticScaling:
			((ListControl)comboBoxDesignerScaleMode).SelectedIndex = 0;
			break;
		case StiDesignerScaleMode.Scaling100:
			((ListControl)comboBoxDesignerScaleMode).SelectedIndex = 1;
			break;
		}
		switch (StiSettings.GetStr("Designer", "StartScreen", "Welcome"))
		{
		case "Welcome":
			((ListControl)comboBoxStartScreen).SelectedIndex = 0;
			break;
		case "BlankReport":
			((ListControl)comboBoxStartScreen).SelectedIndex = 1;
			break;
		case "BlankDashboard":
			((ListControl)comboBoxStartScreen).SelectedIndex = 2;
			break;
		}
		switch (StiOptions.Designer.NewReport.DictionaryBehavior)
		{
		case StiNewReportDictionaryBehavior.MergeDictionary:
			((ListControl)comboBoxNewReportDictionaryBehavior).SelectedIndex = 0;
			break;
		case StiNewReportDictionaryBehavior.NewDictionary:
			((ListControl)comboBoxNewReportDictionaryBehavior).SelectedIndex = 1;
			break;
		}
		((CheckBox)cbAlignToGrid).Checked = info.AlignToGrid;
		((CheckBox)cbShowGrid).Checked = info.ShowGrid;
		((CheckBox)cbShowHeaders).Checked = info.ShowHeaders;
		((CheckBox)cbShowOrder).Checked = info.ShowOrder;
		((CheckBox)cbShowRulers).Checked = info.ShowRulers;
		((CheckBox)cbRunDesignerAfterInsert).Checked = info.RunDesignerAfterInsert;
		((CheckBox)cbUseLastFormat).Checked = info.UseLastFormat;
		((CheckBox)cbAutoSaveReport).Checked = info.AutoSaveReportToReportClass;
		((CheckBox)checkBoxShowDimensionLines).Checked = info.ShowDimensionLines;
		((CheckBox)checkBoxGenerateLocalizedName).Checked = info.GenerateLocalizedName;
		((Control)ebCentimetres).Text = info.GridSizeCentimetres.ToString(CultureInfo.CurrentCulture);
		((Control)ebMillimeters).Text = info.GridSizeMillimeters.ToString(CultureInfo.CurrentCulture);
		((Control)ebInch).Text = info.GridSizeInch.ToString(CultureInfo.CurrentCulture);
		((Control)ebHundredthsOfInch).Text = info.GridSizeHundredthsOfInch.ToString(CultureInfo.CurrentCulture);
		((Control)ebPixels).Text = info.GridSizePixels.ToString(CultureInfo.CurrentCulture);
		((CheckBox)cbFillBands).Checked = info.FillBands;
		((CheckBox)cbFillCrossBands).Checked = info.FillCrossBands;
		((CheckBox)cbFillContainers).Checked = info.FillContainer;
		((CheckBox)cbFillComponents).Checked = info.FillComponent;
		((CheckBox)cbUseComponentColor).Checked = info.UseComponentColor;
		((CheckBox)cbDrawMarkersWhenMoving).Checked = info.DrawMarkersWhenMoving;
		((RadioButton)rbMarkersStyleNone).Checked = info.MarkersStyle == StiMarkersStyle.None;
		((RadioButton)rbMarkersStyleCorners).Checked = info.MarkersStyle == StiMarkersStyle.Corners;
		((RadioButton)rbMarkersStyleDashedRectangle).Checked = info.MarkersStyle == StiMarkersStyle.DashedRectangle;
		((RadioButton)rbGridLines).Checked = info.GridMode == StiGridMode.Lines;
		((RadioButton)rbGridDots).Checked = info.GridMode == StiGridMode.Dots;
		((RadioButton)rbQuickInfoNone).Checked = info.QuickInfoType == StiQuickInfoType.None;
		((RadioButton)rbQuickInfoShowComponentsNames).Checked = info.QuickInfoType == StiQuickInfoType.ShowComponentsNames;
		((RadioButton)rbQuickInfoShowContent).Checked = info.QuickInfoType == StiQuickInfoType.ShowContent;
		((RadioButton)rbQuickInfoShowFields).Checked = info.QuickInfoType == StiQuickInfoType.ShowFields;
		((RadioButton)rbQuickInfoShowFieldsOnly).Checked = info.QuickInfoType == StiQuickInfoType.ShowFieldsOnly;
		((RadioButton)rbQuickInfoShowEvents).Checked = info.QuickInfoType == StiQuickInfoType.ShowEvents;
		((CheckBox)cbShowQuickInfoOverlay).Checked = info.QuickInfoOverlay;
		((CheckBox)checkBoxEnableAutoSave).Checked = info.EnableAutoSaveMode;
		int num = 0;
		int[] autoSaveIntervals = StiOptions.Designer.AutoSaveIntervals;
		foreach (int num2 in autoSaveIntervals)
		{
			if (designer.Report != null && num2 == designer.Report.Info.AutoSaveInterval)
			{
				((ListControl)comboBoxAutoSaveTime).SelectedIndex = num;
				break;
			}
			num++;
		}
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		//IL_063d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			StiSettings.Load();
			StiSettings.Set("CloudDesigner", "ShowStandardToolbox", ((CheckBox)checkBoxShowToolbox).Checked);
			StiSettings.Set("CloudDesigner", "ShowInsertTab", ((CheckBox)checkBoxShowInsertTab).Checked);
			designer.LockRefreshDesigner = true;
			StiSettings.Set("Cloud", "RequestChangesWhenSavingToCloud", ((CheckBox)checkBoxRequestChanges).Checked);
			StiSettings.Set("Designer", "ShowDialogFormsV1", ((CheckBox)checkBoxShowDialogForms).Checked);
			StiOptions.Viewer.Windows.AnimationPlaybackType = (buttonAnimationPlaybackTypeOnStart.Checked ? StiAnimationPlaybackType.OnStart : StiAnimationPlaybackType.None) | (buttonAnimationPlaybackTypeOnPreview.Checked ? StiAnimationPlaybackType.OnPreview : StiAnimationPlaybackType.None);
			bool flag = false;
			StiDesignerScaleMode stiDesignerScaleMode = StiDesignerScaleMode.AutomaticScaling;
			switch (((ListControl)comboBoxDesignerScaleMode).SelectedIndex)
			{
			case 0:
				stiDesignerScaleMode = StiDesignerScaleMode.AutomaticScaling;
				break;
			case 1:
				stiDesignerScaleMode = StiDesignerScaleMode.Scaling100;
				break;
			}
			if (StiOptions.Designer.DesignerScale != stiDesignerScaleMode)
			{
				flag = true;
				StiOptions.Designer.DesignerScale = stiDesignerScaleMode;
			}
			switch (((ListControl)comboBoxStartScreen).SelectedIndex)
			{
			case 0:
				StiSettings.Set("Designer", "StartScreen", "Welcome");
				break;
			case 1:
				StiSettings.Set("Designer", "StartScreen", "BlankReport");
				break;
			case 2:
				StiSettings.Set("Designer", "StartScreen", "BlankDashboard");
				break;
			}
			StiOptions.Designer.Editors.ScriptEditor.BlocksScriptEditorByDefault = ((ListControl)comboBoxDefaultScriptMode).SelectedIndex == 0;
			StiOptions.Designer.Editors.ChartEditor.SimpleEditorByDefault = ((ListControl)comboBoxChartEditor).SelectedIndex == 0;
			bool flag2 = ((ListControl)comboBoxGaugeEditor).SelectedIndex == 1;
			if (StiOptions.Designer.Editors.GaugeEditor.AllowOldEditor != flag2)
			{
				designer.InvokeBuidingToolbox();
			}
			StiOptions.Designer.Editors.GaugeEditor.AllowOldEditor = flag2;
			StiOptions.Designer.NewReport.DictionaryBehavior = ((((ListControl)comboBoxNewReportDictionaryBehavior).SelectedIndex != 0) ? StiNewReportDictionaryBehavior.NewDictionary : StiNewReportDictionaryBehavior.MergeDictionary);
			designer.Info.AlignToGrid = ((CheckBox)cbAlignToGrid).Checked;
			designer.Info.ShowGrid = ((CheckBox)cbShowGrid).Checked;
			designer.Info.ShowHeaders = ((CheckBox)cbShowHeaders).Checked;
			designer.Info.ShowOrder = ((CheckBox)cbShowOrder).Checked;
			designer.Info.ShowRulers = ((CheckBox)cbShowRulers).Checked;
			designer.Info.RunDesignerAfterInsert = ((CheckBox)cbRunDesignerAfterInsert).Checked;
			designer.Info.UseLastFormat = ((CheckBox)cbUseLastFormat).Checked;
			designer.Info.AutoSaveReportToReportClass = ((CheckBox)cbAutoSaveReport).Checked;
			designer.Info.ShowDimensionLines = ((CheckBox)checkBoxShowDimensionLines).Checked;
			designer.Info.GenerateLocalizedName = ((CheckBox)checkBoxGenerateLocalizedName).Checked;
			if (((RadioButton)rbGridLines).Checked)
			{
				designer.Info.GridMode = StiGridMode.Lines;
			}
			else
			{
				designer.Info.GridMode = StiGridMode.Dots;
			}
			if (((RadioButton)rbMarkersStyleNone).Checked)
			{
				designer.Info.MarkersStyle = StiMarkersStyle.None;
			}
			else if (((RadioButton)rbMarkersStyleCorners).Checked)
			{
				designer.Info.MarkersStyle = StiMarkersStyle.Corners;
			}
			else
			{
				designer.Info.MarkersStyle = StiMarkersStyle.DashedRectangle;
			}
			if (((RadioButton)rbQuickInfoNone).Checked)
			{
				designer.Info.QuickInfoType = StiQuickInfoType.None;
			}
			if (((RadioButton)rbQuickInfoShowComponentsNames).Checked)
			{
				designer.Info.QuickInfoType = StiQuickInfoType.ShowComponentsNames;
			}
			if (((RadioButton)rbQuickInfoShowContent).Checked)
			{
				designer.Info.QuickInfoType = StiQuickInfoType.ShowContent;
			}
			if (((RadioButton)rbQuickInfoShowFields).Checked)
			{
				designer.Info.QuickInfoType = StiQuickInfoType.ShowFields;
			}
			if (((RadioButton)rbQuickInfoShowFieldsOnly).Checked)
			{
				designer.Info.QuickInfoType = StiQuickInfoType.ShowFieldsOnly;
			}
			if (((RadioButton)rbQuickInfoShowEvents).Checked)
			{
				designer.Info.QuickInfoType = StiQuickInfoType.ShowEvents;
			}
			designer.Info.QuickInfoOverlay = ((CheckBox)cbShowQuickInfoOverlay).Checked;
			if (double.TryParse(((Control)ebCentimetres).Text, out var result))
			{
				designer.Info.GridSizeCentimetres = result;
			}
			if (double.TryParse(((Control)ebMillimeters).Text, out result))
			{
				designer.Info.GridSizeMillimeters = result;
			}
			if (double.TryParse(((Control)ebInch).Text, out result))
			{
				designer.Info.GridSizeInch = result;
			}
			if (double.TryParse(((Control)ebHundredthsOfInch).Text, out result))
			{
				designer.Info.GridSizeHundredthsOfInch = result;
			}
			if (double.TryParse(((Control)ebPixels).Text, out result))
			{
				designer.Info.GridSizePixels = result;
			}
			designer.Info.FillBands = ((CheckBox)cbFillBands).Checked;
			designer.Info.FillCrossBands = ((CheckBox)cbFillCrossBands).Checked;
			designer.Info.FillContainer = ((CheckBox)cbFillContainers).Checked;
			designer.Info.FillComponent = ((CheckBox)cbFillComponents).Checked;
			designer.Info.UseComponentColor = ((CheckBox)cbUseComponentColor).Checked;
			designer.Info.DrawMarkersWhenMoving = ((CheckBox)cbDrawMarkersWhenMoving).Checked;
			designer.Info.AutoSaveInterval = StiOptions.Designer.AutoSaveIntervals[((ListControl)comboBoxAutoSaveTime).SelectedIndex];
			designer.Info.EnableAutoSaveMode = ((CheckBox)checkBoxEnableAutoSave).Checked;
			StiSmtpConfig stiSmtpConfig = StiSmtpConfig.Get();
			stiSmtpConfig.SenderEmail = ((Control)textBoxSchedulerSenderEmail).Text;
			stiSmtpConfig.Host = ((Control)textBoxSchedulerHost).Text;
			stiSmtpConfig.Port = (int)textBoxSchedulerPort.Value;
			stiSmtpConfig.UserName = ((Control)textBoxSchedulerUserName).Text;
			stiSmtpConfig.Password = ((Control)textBoxSchedulerPassword).Text;
			stiSmtpConfig.EnableSsl = ((CheckBox)checkBoxSchedulerEnableSSL).Checked;
			stiSmtpConfig.TrySave();
			if (flag)
			{
				MessageBox.Show((IWin32Window)(object)this, StiLocalization.Get("Report", "RestartDesigner"), StiOptions.Designer.GetDesignerTitle(), (MessageBoxButtons)0, (MessageBoxIcon)64);
			}
			designer.Builder.BuildReport();
			designer.SaveDesignerParam();
			((Form)this).Close();
			designer.LockRefreshDesigner = false;
			designer.InvokeBuidingToolbox();
			((Control)designer).Refresh();
			designer.SetAutoSaveMode();
		}
		finally
		{
			StiSettings.Save();
		}
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)2;
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/reports_designer_ribbon_mode_2013_main_menu_dialog_options.htm");
	}

	private void ButtonRestoreDefaults_Click(object sender, EventArgs e)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			StiRestoreDefaultForm stiRestoreDefaultForm = new StiRestoreDefaultForm();
			try
			{
				designer?.AddOwnedForm((Form)(object)this);
				((Form)stiRestoreDefaultForm).ShowDialog();
				if (stiRestoreDefaultForm.IsDesignerSettingsChanged || stiRestoreDefaultForm.IsSmtpSettingsChanged)
				{
					Build(new StiDesignerInfo(designer.Report), StiSmtpConfig.Get());
				}
			}
			finally
			{
				((IDisposable)stiRestoreDefaultForm)?.Dispose();
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void CheckBoxEnableAutoSave_CheckedChanged(object sender, EventArgs e)
	{
		((Control)comboBoxAutoSaveTime).Enabled = ((CheckBox)checkBoxEnableAutoSave).Checked;
	}

	private void CheckBoxShowToolbox_CheckedChanged(object sender, EventArgs e)
	{
		bool @checked = ((CheckBox)checkBoxShowToolbox).Checked;
		bool checked2 = ((CheckBox)checkBoxShowInsertTab).Checked;
		if (!@checked && !checked2)
		{
			((CheckBox)checkBoxShowInsertTab).Checked = true;
		}
	}

	private void CheckBoxShowInsertTab_CheckedChanged(object sender, EventArgs e)
	{
		bool @checked = ((CheckBox)checkBoxShowToolbox).Checked;
		bool checked2 = ((CheckBox)checkBoxShowInsertTab).Checked;
		if (!@checked && !checked2)
		{
			((CheckBox)checkBoxShowToolbox).Checked = true;
		}
	}

	private void ButtonAnimationPlaybackTypeChange_Click(object sender, EventArgs e)
	{
		((Control)buttonAnimationPlaybackType).Text = "";
		if (buttonAnimationPlaybackTypeOnStart.Checked)
		{
			((Control)buttonAnimationPlaybackType).Text = buttonAnimationPlaybackTypeOnStart.Text;
		}
		if (buttonAnimationPlaybackTypeOnPreview.Checked)
		{
			if (((Control)buttonAnimationPlaybackType).Text.Length > 0)
			{
				ButtonX buttonX = buttonAnimationPlaybackType;
				((Control)buttonX).Text = ((Control)buttonX).Text + ", ";
			}
			ButtonX buttonX2 = buttonAnimationPlaybackType;
			((Control)buttonX2).Text = ((Control)buttonX2).Text + buttonAnimationPlaybackTypeOnPreview.Text;
		}
		if (((Control)buttonAnimationPlaybackType).Text.Length == 0)
		{
			((Control)buttonAnimationPlaybackType).Text = StiLocalization.Get("MainMenu", "menuViewQuickInfoNone");
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
		//IL_0762: Unknown result type (might be due to invalid IL or missing references)
		//IL_0897: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a1: Expected O, but got Unknown
		//IL_08c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a71: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d63: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0edc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1069: Unknown result type (might be due to invalid IL or missing references)
		//IL_1226: Unknown result type (might be due to invalid IL or missing references)
		//IL_1284: Unknown result type (might be due to invalid IL or missing references)
		//IL_13e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_150d: Unknown result type (might be due to invalid IL or missing references)
		//IL_17c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_17d2: Expected O, but got Unknown
		//IL_180a: Unknown result type (might be due to invalid IL or missing references)
		//IL_254c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2556: Expected O, but got Unknown
		//IL_258e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ab9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ac3: Expected O, but got Unknown
		//IL_2ae8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f33: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f3d: Expected O, but got Unknown
		//IL_2f75: Unknown result type (might be due to invalid IL or missing references)
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiOptionsForm));
		btOk = new StiButtonUX();
		btCancel = new StiButtonUX();
		TabControl = new StiCloudTabulator();
		pageMain = new StiCloudTabulatorPage();
		panelNewReport = new StiPanelUX();
		labelNewReportDictionaryBehavior = new StiLabelUX();
		comboBoxNewReportDictionaryBehavior = new StiComboBox();
		panelScalingMode = new StiPanelUX();
		labelDesignerScaleMode = new StiLabelUX();
		comboBoxDesignerScaleMode = new StiComboBox();
		panelStartScreen = new StiPanelUX();
		labelStartScreen = new StiLabelUX();
		comboBoxStartScreen = new StiComboBox();
		panelDefaultScriptMode = new StiPanelUX();
		labelDefaultScriptMode = new StiLabelUX();
		comboBoxDefaultScriptMode = new StiComboBox();
		panelDeveloper1 = new StiPanelUX();
		labelAnimationPlaybackType = new StiLabelUX();
		buttonAnimationPlaybackType = new ButtonX();
		buttonAnimationPlaybackTypeOnStart = new ButtonItem();
		buttonAnimationPlaybackTypeOnPreview = new ButtonItem();
		comboBoxChartEditor = new StiComboBox();
		labelChartEditor = new StiLabelUX();
		cbRunDesignerAfterInsert = new StiCheckBoxUX();
		cbUseLastFormat = new StiCheckBoxUX();
		comboBoxGaugeEditor = new StiComboBox();
		checkBoxGenerateLocalizedName = new StiCheckBoxUX();
		checkBoxShowDialogForms = new StiCheckBoxUX();
		labelGaugeEditor = new StiLabelUX();
		panelDeveloperCreator1 = new StiPanelUX();
		checkBoxShowInsertTab = new StiCheckBoxUX();
		checkBoxShowToolbox = new StiCheckBoxUX();
		gbMainOptions = new StiOffice2013GroupBox();
		pageDesign = new StiCloudTabulatorPage();
		gbGridSize = new StiOffice2013GroupBox();
		lbPixels = new StiLabelUX();
		label6 = new StiLabelUX();
		ebPixels = new StiTextBox();
		lbMillimeters = new StiLabelUX();
		lbCentimetres = new StiLabelUX();
		lbHundredthsOfInch = new StiLabelUX();
		lbInch = new StiLabelUX();
		label4 = new StiLabelUX();
		label3 = new StiLabelUX();
		label2 = new StiLabelUX();
		ebMillimeters = new StiTextBox();
		ebCentimetres = new StiTextBox();
		ebHundredthsOfInch = new StiTextBox();
		ebInch = new StiTextBox();
		label1 = new StiLabelUX();
		gbGridDrawingOptions = new StiOffice2013GroupBox();
		rbGridLines = new StiRadioButtonUX();
		rbGridDots = new StiRadioButtonUX();
		gbGridOptions = new StiOffice2013GroupBox();
		cbShowGrid = new StiCheckBoxUX();
		cbAlignToGrid = new StiCheckBoxUX();
		gbViewOptions = new StiOffice2013GroupBox();
		cbShowHeaders = new StiCheckBoxUX();
		cbShowOrder = new StiCheckBoxUX();
		cbShowRulers = new StiCheckBoxUX();
		checkBoxShowDimensionLines = new StiCheckBoxUX();
		pageQuickInfo = new StiCloudTabulatorPage();
		gbOptionsOfQuickInfo = new StiOffice2013GroupBox();
		quickInfoSeparator = new StiGroupLine();
		cbShowQuickInfoOverlay = new StiCheckBoxUX();
		rbQuickInfoNone = new StiRadioButtonUX();
		rbQuickInfoShowComponentsNames = new StiRadioButtonUX();
		rbQuickInfoShowFieldsOnly = new StiRadioButtonUX();
		rbQuickInfoShowFields = new StiRadioButtonUX();
		rbQuickInfoShowContent = new StiRadioButtonUX();
		rbQuickInfoShowEvents = new StiRadioButtonUX();
		pageSaving = new StiCloudTabulatorPage();
		gbSaveOther = new StiOffice2013GroupBox();
		cbAutoSaveReport = new StiCheckBoxUX();
		checkBoxRequestChanges = new StiCheckBoxUX();
		gbAutoSaveOptions = new StiOffice2013GroupBox();
		checkBoxEnableAutoSave = new StiCheckBoxUX();
		labelSaveReportEvery = new StiLabelUX();
		comboBoxAutoSaveTime = new StiComboBox();
		pageScheduler = new StiCloudTabulatorPage();
		gbScheduleSmtpSettings = new StiOffice2013GroupBox();
		labelSchedulerSenderEmail = new StiLabelUX();
		textBoxSchedulerSenderEmail = new StiTextBox();
		labelSchedulerHost = new StiLabelUX();
		textBoxSchedulerHost = new StiTextBox();
		labelSchedulerPort = new StiLabelUX();
		textBoxSchedulerPort = new StiNumericUpDown();
		labelSchedulerUserName = new StiLabelUX();
		textBoxSchedulerUserName = new StiTextBox();
		labelSchedulerPassword = new StiLabelUX();
		textBoxSchedulerPassword = new StiTextBox();
		checkBoxSchedulerEnableSSL = new StiCheckBoxUX();
		panelFooter = new StiButtonPanelUX();
		btRestoreDefaults = new StiButtonUX();
		gbMarkersStyle = new StiOffice2013GroupBox();
		rbMarkersStyleNone = new StiRadioButtonUX();
		rbMarkersStyleCorners = new StiRadioButtonUX();
		rbMarkersStyleDashedRectangle = new StiRadioButtonUX();
		gbReportDisplayOptions = new StiOffice2013GroupBox();
		cbDrawMarkersWhenMoving = new StiCheckBoxUX();
		cbFillComponents = new StiCheckBoxUX();
		cbFillBands = new StiCheckBoxUX();
		cbUseComponentColor = new StiCheckBoxUX();
		cbFillCrossBands = new StiCheckBoxUX();
		cbFillContainers = new StiCheckBoxUX();
		((Control)TabControl).SuspendLayout();
		((Control)pageMain).SuspendLayout();
		((Control)panelNewReport).SuspendLayout();
		((Control)panelScalingMode).SuspendLayout();
		((Control)panelStartScreen).SuspendLayout();
		((Control)panelDefaultScriptMode).SuspendLayout();
		((Control)panelDeveloper1).SuspendLayout();
		((Control)panelDeveloperCreator1).SuspendLayout();
		((Control)pageDesign).SuspendLayout();
		((Control)gbGridSize).SuspendLayout();
		((Control)gbGridDrawingOptions).SuspendLayout();
		((Control)gbGridOptions).SuspendLayout();
		((Control)gbViewOptions).SuspendLayout();
		((Control)pageQuickInfo).SuspendLayout();
		((Control)gbOptionsOfQuickInfo).SuspendLayout();
		((Control)pageSaving).SuspendLayout();
		((Control)gbSaveOther).SuspendLayout();
		((Control)gbAutoSaveOptions).SuspendLayout();
		((Control)pageScheduler).SuspendLayout();
		((Control)gbScheduleSmtpSettings).SuspendLayout();
		((ISupportInitialize)textBoxSchedulerPort).BeginInit();
		((Control)panelFooter).SuspendLayout();
		((Control)gbMarkersStyle).SuspendLayout();
		((Control)gbReportDisplayOptions).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(348, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 1;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += This_OkClick;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		((Button)btCancel).DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(429, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 2;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btCancel).Click += This_CancelClick;
		((Control)TabControl).Controls.Add((Control)(object)pageMain);
		((Control)TabControl).Controls.Add((Control)(object)pageDesign);
		((Control)TabControl).Controls.Add((Control)(object)pageQuickInfo);
		((Control)TabControl).Controls.Add((Control)(object)pageSaving);
		((Control)TabControl).Controls.Add((Control)(object)pageScheduler);
		((Control)TabControl).Dock = (DockStyle)5;
		((Control)TabControl).Location = new Point(0, 0);
		((Control)TabControl).Name = "TabControl";
		((Control)TabControl).Padding = new Padding(0, 0, 119, 0);
		TabControl.ScrollHeight = 0;
		TabControl.ScrollWidth = 100;
		((Control)TabControl).Size = new Size(516, 537);
		((Control)TabControl).TabIndex = 0;
		TabControl.TitleBackColor = SystemColors.Control;
		TabControl.TitleInLine = false;
		TabControl.TitlePosition = StiTabTitlePosition.RightHorizontal;
		TabControl.TitleWidth = 110;
		((Control)pageMain).Controls.Add((Control)(object)panelNewReport);
		((Control)pageMain).Controls.Add((Control)(object)panelScalingMode);
		((Control)pageMain).Controls.Add((Control)(object)panelStartScreen);
		((Control)pageMain).Controls.Add((Control)(object)panelDefaultScriptMode);
		((Control)pageMain).Controls.Add((Control)(object)panelDeveloper1);
		((Control)pageMain).Controls.Add((Control)(object)panelDeveloperCreator1);
		((Control)pageMain).Controls.Add((Control)(object)gbMainOptions);
		pageMain.Dock = (DockStyle)5;
		pageMain.Image = (Image)componentResourceManager.GetObject("pageMain.Image");
		pageMain.InvisibleVisible = true;
		((Control)pageMain).Location = new Point(0, 0);
		((Control)pageMain).Margin = new Padding(1);
		((Control)pageMain).Name = "pageMain";
		((Control)pageMain).Padding = new Padding(0, 0, 1, 0);
		((Control)pageMain).Size = new Size(397, 537);
		((Control)pageMain).TabIndex = 0;
		((Control)pageMain).Text = "Main";
		((Control)pageMain).Visible = false;
		((Control)panelNewReport).Controls.Add((Control)(object)labelNewReportDictionaryBehavior);
		((Control)panelNewReport).Controls.Add((Control)(object)comboBoxNewReportDictionaryBehavior);
		((Control)panelNewReport).Dock = (DockStyle)1;
		((Control)panelNewReport).Location = new Point(0, 412);
		((Control)panelNewReport).Name = "panelNewReport";
		((Control)panelNewReport).Size = new Size(396, 32);
		((Control)panelNewReport).TabIndex = 6;
		((Control)labelNewReportDictionaryBehavior).Location = new Point(12, 3);
		((Control)labelNewReportDictionaryBehavior).Margin = new Padding(2, 0, 2, 0);
		((Control)labelNewReportDictionaryBehavior).Name = "labelNewReportDictionaryBehavior";
		((Control)labelNewReportDictionaryBehavior).Size = new Size(189, 17);
		((Control)labelNewReportDictionaryBehavior).TabIndex = 15;
		((Control)labelNewReportDictionaryBehavior).Text = "New Report";
		((Label)labelNewReportDictionaryBehavior).TextAlign = (ContentAlignment)16;
		((Control)comboBoxNewReportDictionaryBehavior).Anchor = (AnchorStyles)13;
		comboBoxNewReportDictionaryBehavior.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxNewReportDictionaryBehavior).FormattingEnabled = true;
		((Control)comboBoxNewReportDictionaryBehavior).Location = new Point(205, 0);
		((Control)comboBoxNewReportDictionaryBehavior).Margin = new Padding(2);
		((Control)comboBoxNewReportDictionaryBehavior).Name = "comboBoxNewReportDictionaryBehavior";
		((Control)comboBoxNewReportDictionaryBehavior).Size = new Size(178, 21);
		((Control)comboBoxNewReportDictionaryBehavior).TabIndex = 16;
		((Control)panelScalingMode).Controls.Add((Control)(object)labelDesignerScaleMode);
		((Control)panelScalingMode).Controls.Add((Control)(object)comboBoxDesignerScaleMode);
		((Control)panelScalingMode).Dock = (DockStyle)1;
		((Control)panelScalingMode).Location = new Point(0, 380);
		((Control)panelScalingMode).Name = "panelScalingMode";
		((Control)panelScalingMode).Size = new Size(396, 32);
		((Control)panelScalingMode).TabIndex = 5;
		((Control)labelDesignerScaleMode).Location = new Point(12, 3);
		((Control)labelDesignerScaleMode).Margin = new Padding(2, 0, 2, 0);
		((Control)labelDesignerScaleMode).Name = "labelDesignerScaleMode";
		((Control)labelDesignerScaleMode).Size = new Size(189, 17);
		((Control)labelDesignerScaleMode).TabIndex = 15;
		((Control)labelDesignerScaleMode).Text = "Scaling Mode";
		((Label)labelDesignerScaleMode).TextAlign = (ContentAlignment)16;
		((Control)comboBoxDesignerScaleMode).Anchor = (AnchorStyles)13;
		comboBoxDesignerScaleMode.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxDesignerScaleMode).FormattingEnabled = true;
		((Control)comboBoxDesignerScaleMode).Location = new Point(205, 0);
		((Control)comboBoxDesignerScaleMode).Margin = new Padding(2);
		((Control)comboBoxDesignerScaleMode).Name = "comboBoxDesignerScaleMode";
		((Control)comboBoxDesignerScaleMode).Size = new Size(178, 21);
		((Control)comboBoxDesignerScaleMode).TabIndex = 16;
		((Control)panelStartScreen).Controls.Add((Control)(object)labelStartScreen);
		((Control)panelStartScreen).Controls.Add((Control)(object)comboBoxStartScreen);
		((Control)panelStartScreen).Dock = (DockStyle)1;
		((Control)panelStartScreen).Location = new Point(0, 346);
		((Control)panelStartScreen).Name = "panelStartScreen";
		((Control)panelStartScreen).Size = new Size(396, 34);
		((Control)panelStartScreen).TabIndex = 4;
		((Control)labelStartScreen).Location = new Point(12, 3);
		((Control)labelStartScreen).Margin = new Padding(2, 0, 2, 0);
		((Control)labelStartScreen).Name = "labelStartScreen";
		((Control)labelStartScreen).Size = new Size(189, 17);
		((Control)labelStartScreen).TabIndex = 15;
		((Control)labelStartScreen).Text = "Start Screen";
		((Label)labelStartScreen).TextAlign = (ContentAlignment)16;
		((Control)comboBoxStartScreen).Anchor = (AnchorStyles)13;
		comboBoxStartScreen.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxStartScreen).FormattingEnabled = true;
		((Control)comboBoxStartScreen).Location = new Point(205, 0);
		((Control)comboBoxStartScreen).Margin = new Padding(2);
		((Control)comboBoxStartScreen).Name = "comboBoxStartScreen";
		((Control)comboBoxStartScreen).Size = new Size(178, 21);
		((Control)comboBoxStartScreen).TabIndex = 16;
		((Control)panelDefaultScriptMode).Controls.Add((Control)(object)labelDefaultScriptMode);
		((Control)panelDefaultScriptMode).Controls.Add((Control)(object)comboBoxDefaultScriptMode);
		((Control)panelDefaultScriptMode).Dock = (DockStyle)1;
		((Control)panelDefaultScriptMode).Location = new Point(0, 314);
		((Control)panelDefaultScriptMode).Name = "panelDefaultScriptMode";
		((Control)panelDefaultScriptMode).Size = new Size(396, 32);
		((Control)panelDefaultScriptMode).TabIndex = 3;
		((Control)labelDefaultScriptMode).Location = new Point(12, 3);
		((Control)labelDefaultScriptMode).Margin = new Padding(2, 0, 2, 0);
		((Control)labelDefaultScriptMode).Name = "labelDefaultScriptMode";
		((Control)labelDefaultScriptMode).Size = new Size(189, 17);
		((Control)labelDefaultScriptMode).TabIndex = 15;
		((Control)labelDefaultScriptMode).Text = "Default Script Mode";
		((Label)labelDefaultScriptMode).TextAlign = (ContentAlignment)16;
		((Control)comboBoxDefaultScriptMode).Anchor = (AnchorStyles)13;
		comboBoxDefaultScriptMode.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxDefaultScriptMode).FormattingEnabled = true;
		((Control)comboBoxDefaultScriptMode).Location = new Point(205, 0);
		((Control)comboBoxDefaultScriptMode).Margin = new Padding(2);
		((Control)comboBoxDefaultScriptMode).Name = "comboBoxDefaultScriptMode";
		((Control)comboBoxDefaultScriptMode).Size = new Size(178, 21);
		((Control)comboBoxDefaultScriptMode).TabIndex = 16;
		((Control)panelDeveloper1).Controls.Add((Control)(object)labelAnimationPlaybackType);
		((Control)panelDeveloper1).Controls.Add((Control)(object)buttonAnimationPlaybackType);
		((Control)panelDeveloper1).Controls.Add((Control)(object)comboBoxChartEditor);
		((Control)panelDeveloper1).Controls.Add((Control)(object)labelChartEditor);
		((Control)panelDeveloper1).Controls.Add((Control)(object)cbRunDesignerAfterInsert);
		((Control)panelDeveloper1).Controls.Add((Control)(object)cbUseLastFormat);
		((Control)panelDeveloper1).Controls.Add((Control)(object)comboBoxGaugeEditor);
		((Control)panelDeveloper1).Controls.Add((Control)(object)checkBoxGenerateLocalizedName);
		((Control)panelDeveloper1).Controls.Add((Control)(object)checkBoxShowDialogForms);
		((Control)panelDeveloper1).Controls.Add((Control)(object)labelGaugeEditor);
		((Control)panelDeveloper1).Dock = (DockStyle)1;
		((Control)panelDeveloper1).Location = new Point(0, 94);
		((Control)panelDeveloper1).Name = "panelDeveloper1";
		((Control)panelDeveloper1).Size = new Size(396, 220);
		((Control)panelDeveloper1).TabIndex = 2;
		((Control)labelAnimationPlaybackType).Location = new Point(12, 120);
		((Control)labelAnimationPlaybackType).Margin = new Padding(2, 0, 2, 0);
		((Control)labelAnimationPlaybackType).Name = "labelAnimationPlaybackType";
		((Control)labelAnimationPlaybackType).Size = new Size(189, 17);
		((Control)labelAnimationPlaybackType).TabIndex = 5;
		((Label)labelAnimationPlaybackType).TextAlign = (ContentAlignment)16;
		((Control)buttonAnimationPlaybackType).AccessibleRole = (AccessibleRole)43;
		buttonAnimationPlaybackType.AutoExpandOnClick = true;
		buttonAnimationPlaybackType.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)buttonAnimationPlaybackType).Location = new Point(205, 120);
		buttonAnimationPlaybackType.LockClosePopup = false;
		((Control)buttonAnimationPlaybackType).Name = "buttonAnimationPlaybackType";
		((Control)buttonAnimationPlaybackType).Size = new Size(176, 22);
		buttonAnimationPlaybackType.SubItems.AddRange(new BaseItem[2] { buttonAnimationPlaybackTypeOnStart, buttonAnimationPlaybackTypeOnPreview });
		((Control)buttonAnimationPlaybackType).TabIndex = 4;
		buttonAnimationPlaybackType.TextAlignment = eButtonTextAlignment.Left;
		buttonAnimationPlaybackTypeOnStart.AutoCheckOnClick = true;
		buttonAnimationPlaybackTypeOnStart.AutoCollapseOnClick = false;
		buttonAnimationPlaybackTypeOnStart.Name = "buttonAnimationPlaybackTypeOnStart";
		buttonAnimationPlaybackTypeOnStart.Click += ButtonAnimationPlaybackTypeChange_Click;
		buttonAnimationPlaybackTypeOnPreview.AutoCheckOnClick = true;
		buttonAnimationPlaybackTypeOnPreview.AutoCollapseOnClick = false;
		buttonAnimationPlaybackTypeOnPreview.Name = "buttonAnimationPlaybackTypeOnPreview";
		buttonAnimationPlaybackTypeOnPreview.Click += ButtonAnimationPlaybackTypeChange_Click;
		((Control)comboBoxChartEditor).Anchor = (AnchorStyles)13;
		comboBoxChartEditor.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxChartEditor).FormattingEnabled = true;
		((Control)comboBoxChartEditor).Location = new Point(205, 153);
		((Control)comboBoxChartEditor).Margin = new Padding(2);
		((Control)comboBoxChartEditor).Name = "comboBoxChartEditor";
		((Control)comboBoxChartEditor).Size = new Size(178, 21);
		((Control)comboBoxChartEditor).TabIndex = 6;
		((Control)labelChartEditor).Location = new Point(12, 156);
		((Control)labelChartEditor).Margin = new Padding(2, 0, 2, 0);
		((Control)labelChartEditor).Name = "labelChartEditor";
		((Control)labelChartEditor).Size = new Size(189, 17);
		((Control)labelChartEditor).TabIndex = 5;
		((Control)labelChartEditor).Text = "Chart Editor";
		((Label)labelChartEditor).TextAlign = (ContentAlignment)16;
		((Control)cbRunDesignerAfterInsert).Location = new Point(12, 0);
		((Control)cbRunDesignerAfterInsert).Name = "cbRunDesignerAfterInsert";
		((Control)cbRunDesignerAfterInsert).Size = new Size(371, 20);
		((Control)cbRunDesignerAfterInsert).TabIndex = 0;
		((Control)cbRunDesignerAfterInsert).Text = "RunDesignerAfterInsert";
		((ButtonBase)cbRunDesignerAfterInsert).UseVisualStyleBackColor = false;
		((Control)cbUseLastFormat).Location = new Point(12, 30);
		((Control)cbUseLastFormat).Name = "cbUseLastFormat";
		((Control)cbUseLastFormat).Size = new Size(371, 20);
		((Control)cbUseLastFormat).TabIndex = 1;
		((Control)cbUseLastFormat).Text = "UseLastFormat";
		((ButtonBase)cbUseLastFormat).UseVisualStyleBackColor = false;
		((Control)comboBoxGaugeEditor).Anchor = (AnchorStyles)13;
		comboBoxGaugeEditor.DropDownStyle = (ComboBoxStyle)2;
		((ListControl)comboBoxGaugeEditor).FormattingEnabled = true;
		((Control)comboBoxGaugeEditor).Location = new Point(205, 186);
		((Control)comboBoxGaugeEditor).Margin = new Padding(2);
		((Control)comboBoxGaugeEditor).Name = "comboBoxGaugeEditor";
		((Control)comboBoxGaugeEditor).Size = new Size(178, 21);
		((Control)comboBoxGaugeEditor).TabIndex = 8;
		((Control)checkBoxGenerateLocalizedName).Location = new Point(12, 60);
		((Control)checkBoxGenerateLocalizedName).Name = "checkBoxGenerateLocalizedName";
		((Control)checkBoxGenerateLocalizedName).Size = new Size(371, 20);
		((Control)checkBoxGenerateLocalizedName).TabIndex = 3;
		((Control)checkBoxGenerateLocalizedName).Text = "Generate Localized Name";
		((ButtonBase)checkBoxGenerateLocalizedName).UseVisualStyleBackColor = false;
		((Control)checkBoxShowDialogForms).Location = new Point(12, 90);
		((Control)checkBoxShowDialogForms).Name = "checkBoxShowDialogForms";
		((Control)checkBoxShowDialogForms).Size = new Size(371, 20);
		((Control)checkBoxShowDialogForms).TabIndex = 4;
		((Control)checkBoxShowDialogForms).Text = "Show Dialog Forms (Version 1)";
		((ButtonBase)checkBoxShowDialogForms).UseVisualStyleBackColor = false;
		((Control)labelGaugeEditor).Location = new Point(12, 189);
		((Control)labelGaugeEditor).Margin = new Padding(2, 0, 2, 0);
		((Control)labelGaugeEditor).Name = "labelGaugeEditor";
		((Control)labelGaugeEditor).Size = new Size(189, 17);
		((Control)labelGaugeEditor).TabIndex = 7;
		((Control)labelGaugeEditor).Text = "Gauge Editor";
		((Label)labelGaugeEditor).TextAlign = (ContentAlignment)16;
		((Control)panelDeveloperCreator1).Controls.Add((Control)(object)checkBoxShowInsertTab);
		((Control)panelDeveloperCreator1).Controls.Add((Control)(object)checkBoxShowToolbox);
		((Control)panelDeveloperCreator1).Dock = (DockStyle)1;
		((Control)panelDeveloperCreator1).Location = new Point(0, 25);
		((Control)panelDeveloperCreator1).Name = "panelDeveloperCreator1";
		((Control)panelDeveloperCreator1).Size = new Size(396, 69);
		((Control)panelDeveloperCreator1).TabIndex = 1;
		((Control)checkBoxShowInsertTab).Location = new Point(12, 39);
		((Control)checkBoxShowInsertTab).Name = "checkBoxShowInsertTab";
		((Control)checkBoxShowInsertTab).Size = new Size(343, 20);
		((Control)checkBoxShowInsertTab).TabIndex = 4;
		((Control)checkBoxShowInsertTab).Text = "Show Insert Tab";
		((ButtonBase)checkBoxShowInsertTab).UseVisualStyleBackColor = false;
		((CheckBox)checkBoxShowInsertTab).CheckedChanged += CheckBoxShowInsertTab_CheckedChanged;
		((Control)checkBoxShowToolbox).Location = new Point(12, 9);
		((Control)checkBoxShowToolbox).Name = "checkBoxShowToolbox";
		((Control)checkBoxShowToolbox).Size = new Size(343, 20);
		((Control)checkBoxShowToolbox).TabIndex = 3;
		((Control)checkBoxShowToolbox).Text = "Show Toolbox";
		((ButtonBase)checkBoxShowToolbox).UseVisualStyleBackColor = false;
		((CheckBox)checkBoxShowToolbox).CheckedChanged += CheckBoxShowToolbox_CheckedChanged;
		((Control)gbMainOptions).Dock = (DockStyle)1;
		gbMainOptions.HeaderHeight = 24;
		((Control)gbMainOptions).Location = new Point(0, 0);
		((Control)gbMainOptions).Name = "gbMainOptions";
		((Control)gbMainOptions).Size = new Size(396, 25);
		((Control)gbMainOptions).TabIndex = 0;
		((Control)gbMainOptions).TabStop = false;
		((Control)gbMainOptions).Text = "Main options for working with report designer";
		((Control)pageDesign).Controls.Add((Control)(object)gbGridSize);
		((Control)pageDesign).Controls.Add((Control)(object)gbGridDrawingOptions);
		((Control)pageDesign).Controls.Add((Control)(object)gbGridOptions);
		((Control)pageDesign).Controls.Add((Control)(object)gbViewOptions);
		pageDesign.Dock = (DockStyle)5;
		pageDesign.Image = (Image)componentResourceManager.GetObject("pageDesign.Image");
		pageDesign.InvisibleVisible = false;
		((Control)pageDesign).Location = new Point(0, 0);
		((Control)pageDesign).Name = "pageDesign";
		((Control)pageDesign).Padding = new Padding(0, 0, 1, 0);
		((Control)pageDesign).Size = new Size(397, 537);
		((Control)pageDesign).TabIndex = 1;
		((Control)pageDesign).Text = "Design";
		((Control)pageDesign).Visible = false;
		((Control)gbGridSize).Controls.Add((Control)(object)lbPixels);
		((Control)gbGridSize).Controls.Add((Control)(object)label6);
		((Control)gbGridSize).Controls.Add((Control)(object)ebPixels);
		((Control)gbGridSize).Controls.Add((Control)(object)lbMillimeters);
		((Control)gbGridSize).Controls.Add((Control)(object)lbCentimetres);
		((Control)gbGridSize).Controls.Add((Control)(object)lbHundredthsOfInch);
		((Control)gbGridSize).Controls.Add((Control)(object)lbInch);
		((Control)gbGridSize).Controls.Add((Control)(object)label4);
		((Control)gbGridSize).Controls.Add((Control)(object)label3);
		((Control)gbGridSize).Controls.Add((Control)(object)label2);
		((Control)gbGridSize).Controls.Add((Control)(object)ebMillimeters);
		((Control)gbGridSize).Controls.Add((Control)(object)ebCentimetres);
		((Control)gbGridSize).Controls.Add((Control)(object)ebHundredthsOfInch);
		((Control)gbGridSize).Controls.Add((Control)(object)ebInch);
		((Control)gbGridSize).Controls.Add((Control)(object)label1);
		((Control)gbGridSize).Dock = (DockStyle)1;
		gbGridSize.HeaderHeight = 24;
		((Control)gbGridSize).Location = new Point(0, 340);
		((Control)gbGridSize).Name = "gbGridSize";
		((Control)gbGridSize).Size = new Size(396, 191);
		((Control)gbGridSize).TabIndex = 3;
		((Control)gbGridSize).TabStop = false;
		((Control)gbGridSize).Text = "Grid size";
		((Control)lbPixels).Location = new Point(12, 164);
		((Control)lbPixels).Name = "lbPixels";
		((Control)lbPixels).Size = new Size(138, 20);
		((Control)lbPixels).TabIndex = 34;
		((Control)lbPixels).Text = "Pixels";
		((Label)lbPixels).TextAlign = (ContentAlignment)16;
		((Control)label6).Location = new Point(211, 164);
		((Control)label6).Name = "label6";
		((Control)label6).Size = new Size(32, 20);
		((Control)label6).TabIndex = 36;
		((Control)label6).Text = "px";
		((Label)label6).TextAlign = (ContentAlignment)16;
		((Control)ebPixels).Location = new Point(155, 164);
		((Control)ebPixels).Name = "ebPixels";
		((Control)ebPixels).Size = new Size(48, 21);
		((Control)ebPixels).TabIndex = 4;
		((Control)ebPixels).Text = "StiTextBox4";
		((Control)lbMillimeters).Location = new Point(12, 132);
		((Control)lbMillimeters).Name = "lbMillimeters";
		((Control)lbMillimeters).Size = new Size(138, 20);
		((Control)lbMillimeters).TabIndex = 31;
		((Control)lbMillimeters).Text = "Millimeters";
		((Label)lbMillimeters).TextAlign = (ContentAlignment)16;
		((Control)lbCentimetres).Location = new Point(12, 100);
		((Control)lbCentimetres).Name = "lbCentimetres";
		((Control)lbCentimetres).Size = new Size(138, 20);
		((Control)lbCentimetres).TabIndex = 28;
		((Control)lbCentimetres).Text = "Centimetres";
		((Label)lbCentimetres).TextAlign = (ContentAlignment)16;
		((Control)lbHundredthsOfInch).Location = new Point(12, 68);
		((Control)lbHundredthsOfInch).Name = "lbHundredthsOfInch";
		((Control)lbHundredthsOfInch).Size = new Size(138, 20);
		((Control)lbHundredthsOfInch).TabIndex = 25;
		((Control)lbHundredthsOfInch).Text = "HundredthsOfInch";
		((Label)lbHundredthsOfInch).TextAlign = (ContentAlignment)16;
		((Control)lbInch).Location = new Point(12, 36);
		((Control)lbInch).Name = "lbInch";
		((Control)lbInch).Size = new Size(138, 20);
		((Control)lbInch).TabIndex = 22;
		((Control)lbInch).Text = "Inch";
		((Label)lbInch).TextAlign = (ContentAlignment)16;
		((Control)label4).Location = new Point(211, 132);
		((Control)label4).Name = "label4";
		((Control)label4).Size = new Size(32, 20);
		((Control)label4).TabIndex = 33;
		((Control)label4).Text = "mm";
		((Label)label4).TextAlign = (ContentAlignment)16;
		((Control)label3).Location = new Point(211, 100);
		((Control)label3).Name = "label3";
		((Control)label3).Size = new Size(32, 20);
		((Control)label3).TabIndex = 30;
		((Control)label3).Text = "cm";
		((Label)label3).TextAlign = (ContentAlignment)16;
		((Control)label2).Location = new Point(211, 68);
		((Control)label2).Name = "label2";
		((Control)label2).Size = new Size(32, 20);
		((Control)label2).TabIndex = 27;
		((Control)label2).Text = "hi";
		((Label)label2).TextAlign = (ContentAlignment)16;
		((Control)ebMillimeters).Location = new Point(155, 132);
		((Control)ebMillimeters).Name = "ebMillimeters";
		((Control)ebMillimeters).Size = new Size(48, 21);
		((Control)ebMillimeters).TabIndex = 3;
		((Control)ebMillimeters).Text = "StiTextBox4";
		((Control)ebCentimetres).Location = new Point(155, 100);
		((Control)ebCentimetres).Name = "ebCentimetres";
		((Control)ebCentimetres).Size = new Size(48, 21);
		((Control)ebCentimetres).TabIndex = 2;
		((Control)ebCentimetres).Text = "StiTextBox3";
		((Control)ebHundredthsOfInch).Location = new Point(155, 68);
		((Control)ebHundredthsOfInch).Name = "ebHundredthsOfInch";
		((Control)ebHundredthsOfInch).Size = new Size(48, 21);
		((Control)ebHundredthsOfInch).TabIndex = 1;
		((Control)ebHundredthsOfInch).Text = "StiTextBox2";
		((Control)ebInch).Location = new Point(155, 36);
		((Control)ebInch).Name = "ebInch";
		((Control)ebInch).Size = new Size(48, 21);
		((Control)ebInch).TabIndex = 0;
		((Control)ebInch).Text = "StiTextBox1";
		((Control)label1).Location = new Point(211, 36);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(32, 20);
		((Control)label1).TabIndex = 24;
		((Control)label1).Text = "in";
		((Label)label1).TextAlign = (ContentAlignment)16;
		((Control)gbGridDrawingOptions).Controls.Add((Control)(object)rbGridLines);
		((Control)gbGridDrawingOptions).Controls.Add((Control)(object)rbGridDots);
		((Control)gbGridDrawingOptions).Dock = (DockStyle)1;
		gbGridDrawingOptions.HeaderHeight = 24;
		((Control)gbGridDrawingOptions).Location = new Point(0, 247);
		((Control)gbGridDrawingOptions).Name = "gbGridDrawingOptions";
		((Control)gbGridDrawingOptions).Size = new Size(396, 93);
		((Control)gbGridDrawingOptions).TabIndex = 2;
		((Control)gbGridDrawingOptions).TabStop = false;
		((Control)gbGridDrawingOptions).Text = "Grid drawing options";
		((Control)rbGridLines).Location = new Point(12, 35);
		((Control)rbGridLines).Name = "rbGridLines";
		((Control)rbGridLines).Size = new Size(343, 20);
		((Control)rbGridLines).TabIndex = 0;
		((Control)rbGridLines).Text = "Lines";
		((ButtonBase)rbGridLines).UseVisualStyleBackColor = false;
		((Control)rbGridDots).Location = new Point(12, 65);
		((Control)rbGridDots).Name = "rbGridDots";
		((Control)rbGridDots).Size = new Size(343, 20);
		((Control)rbGridDots).TabIndex = 1;
		((Control)rbGridDots).Text = "Dots";
		((ButtonBase)rbGridDots).UseVisualStyleBackColor = false;
		((Control)gbGridOptions).Controls.Add((Control)(object)cbShowGrid);
		((Control)gbGridOptions).Controls.Add((Control)(object)cbAlignToGrid);
		((Control)gbGridOptions).Dock = (DockStyle)1;
		gbGridOptions.HeaderHeight = 24;
		((Control)gbGridOptions).Location = new Point(0, 154);
		((Control)gbGridOptions).Name = "gbGridOptions";
		((Control)gbGridOptions).Size = new Size(396, 93);
		((Control)gbGridOptions).TabIndex = 1;
		((Control)gbGridOptions).TabStop = false;
		((Control)gbGridOptions).Text = "Grid options";
		((Control)cbShowGrid).Location = new Point(12, 65);
		((Control)cbShowGrid).Name = "cbShowGrid";
		((Control)cbShowGrid).Size = new Size(343, 20);
		((Control)cbShowGrid).TabIndex = 1;
		((Control)cbShowGrid).Text = "ShowGrid";
		((ButtonBase)cbShowGrid).UseVisualStyleBackColor = false;
		((Control)cbAlignToGrid).Location = new Point(12, 35);
		((Control)cbAlignToGrid).Name = "cbAlignToGrid";
		((Control)cbAlignToGrid).Size = new Size(343, 20);
		((Control)cbAlignToGrid).TabIndex = 0;
		((Control)cbAlignToGrid).Text = "AlignToGrid";
		((ButtonBase)cbAlignToGrid).UseVisualStyleBackColor = false;
		((Control)gbViewOptions).Controls.Add((Control)(object)cbShowHeaders);
		((Control)gbViewOptions).Controls.Add((Control)(object)cbShowOrder);
		((Control)gbViewOptions).Controls.Add((Control)(object)cbShowRulers);
		((Control)gbViewOptions).Controls.Add((Control)(object)checkBoxShowDimensionLines);
		((Control)gbViewOptions).Dock = (DockStyle)1;
		gbViewOptions.HeaderHeight = 24;
		((Control)gbViewOptions).Location = new Point(0, 0);
		((Control)gbViewOptions).Name = "gbViewOptions";
		((Control)gbViewOptions).Size = new Size(396, 154);
		((Control)gbViewOptions).TabIndex = 0;
		((Control)gbViewOptions).TabStop = false;
		((Control)gbViewOptions).Text = "View Options";
		((Control)cbShowHeaders).Location = new Point(12, 35);
		((Control)cbShowHeaders).Name = "cbShowHeaders";
		((Control)cbShowHeaders).Size = new Size(343, 20);
		((Control)cbShowHeaders).TabIndex = 0;
		((Control)cbShowHeaders).Text = "ShowHeaders";
		((ButtonBase)cbShowHeaders).UseVisualStyleBackColor = false;
		((Control)cbShowOrder).Location = new Point(12, 95);
		((Control)cbShowOrder).Name = "cbShowOrder";
		((Control)cbShowOrder).Size = new Size(343, 20);
		((Control)cbShowOrder).TabIndex = 2;
		((Control)cbShowOrder).Text = "Show Order";
		((ButtonBase)cbShowOrder).UseVisualStyleBackColor = false;
		((Control)cbShowRulers).Location = new Point(12, 65);
		((Control)cbShowRulers).Name = "cbShowRulers";
		((Control)cbShowRulers).Size = new Size(343, 20);
		((Control)cbShowRulers).TabIndex = 1;
		((Control)cbShowRulers).Text = "ShowRulers";
		((ButtonBase)cbShowRulers).UseVisualStyleBackColor = false;
		((Control)checkBoxShowDimensionLines).Location = new Point(12, 125);
		((Control)checkBoxShowDimensionLines).Name = "checkBoxShowDimensionLines";
		((Control)checkBoxShowDimensionLines).Size = new Size(343, 20);
		((Control)checkBoxShowDimensionLines).TabIndex = 3;
		((Control)checkBoxShowDimensionLines).Text = "Show Dimension Lines";
		((ButtonBase)checkBoxShowDimensionLines).UseVisualStyleBackColor = false;
		((Control)pageQuickInfo).Controls.Add((Control)(object)gbOptionsOfQuickInfo);
		pageQuickInfo.Dock = (DockStyle)5;
		pageQuickInfo.Image = (Image)componentResourceManager.GetObject("pageQuickInfo.Image");
		pageQuickInfo.InvisibleVisible = false;
		((Control)pageQuickInfo).Location = new Point(0, 0);
		((Control)pageQuickInfo).Name = "pageQuickInfo";
		((Control)pageQuickInfo).Padding = new Padding(0, 0, 1, 0);
		((Control)pageQuickInfo).Size = new Size(397, 537);
		((Control)pageQuickInfo).TabIndex = 2;
		((Control)pageQuickInfo).Text = "Quick Info";
		((Control)pageQuickInfo).Visible = false;
		((Control)gbOptionsOfQuickInfo).Controls.Add((Control)(object)quickInfoSeparator);
		((Control)gbOptionsOfQuickInfo).Controls.Add((Control)(object)cbShowQuickInfoOverlay);
		((Control)gbOptionsOfQuickInfo).Controls.Add((Control)(object)rbQuickInfoNone);
		((Control)gbOptionsOfQuickInfo).Controls.Add((Control)(object)rbQuickInfoShowComponentsNames);
		((Control)gbOptionsOfQuickInfo).Controls.Add((Control)(object)rbQuickInfoShowFieldsOnly);
		((Control)gbOptionsOfQuickInfo).Controls.Add((Control)(object)rbQuickInfoShowFields);
		((Control)gbOptionsOfQuickInfo).Controls.Add((Control)(object)rbQuickInfoShowContent);
		((Control)gbOptionsOfQuickInfo).Controls.Add((Control)(object)rbQuickInfoShowEvents);
		((Control)gbOptionsOfQuickInfo).Dock = (DockStyle)1;
		gbOptionsOfQuickInfo.HeaderHeight = 24;
		((Control)gbOptionsOfQuickInfo).Location = new Point(0, 0);
		((Control)gbOptionsOfQuickInfo).Name = "gbOptionsOfQuickInfo";
		((Control)gbOptionsOfQuickInfo).Size = new Size(396, 380);
		((Control)gbOptionsOfQuickInfo).TabIndex = 2;
		((Control)gbOptionsOfQuickInfo).TabStop = false;
		((Control)gbOptionsOfQuickInfo).Text = "Options of Quick Info";
		quickInfoSeparator.LinePosition = StiLinePosition.Top;
		quickInfoSeparator.LineStyle = StiLineStyle.Dot;
		((Control)quickInfoSeparator).Location = new Point(3, 212);
		((Control)quickInfoSeparator).Name = "quickInfoSeparator";
		((Control)quickInfoSeparator).Size = new Size(389, 2);
		((Control)quickInfoSeparator).TabIndex = 14;
		((Control)cbShowQuickInfoOverlay).AutoSize = true;
		((Control)cbShowQuickInfoOverlay).Location = new Point(12, 226);
		((Control)cbShowQuickInfoOverlay).Name = "cbShowQuickInfoOverlay";
		((Control)cbShowQuickInfoOverlay).Size = new Size(135, 17);
		((Control)cbShowQuickInfoOverlay).TabIndex = 13;
		((Control)cbShowQuickInfoOverlay).Text = "ShowQuickInfoOverlay";
		((ButtonBase)cbShowQuickInfoOverlay).UseVisualStyleBackColor = false;
		((Control)rbQuickInfoNone).AutoSize = true;
		((Control)rbQuickInfoNone).Location = new Point(12, 36);
		((Control)rbQuickInfoNone).Name = "rbQuickInfoNone";
		((Control)rbQuickInfoNone).Size = new Size(51, 17);
		((Control)rbQuickInfoNone).TabIndex = 7;
		((Control)rbQuickInfoNone).Text = "None";
		((ButtonBase)rbQuickInfoNone).UseVisualStyleBackColor = true;
		((Control)rbQuickInfoShowComponentsNames).AutoSize = true;
		((Control)rbQuickInfoShowComponentsNames).Location = new Point(12, 66);
		((Control)rbQuickInfoShowComponentsNames).Name = "rbQuickInfoShowComponentsNames";
		((Control)rbQuickInfoShowComponentsNames).Size = new Size(144, 17);
		((Control)rbQuickInfoShowComponentsNames).TabIndex = 8;
		((Control)rbQuickInfoShowComponentsNames).Text = "ShowComponentsNames";
		((ButtonBase)rbQuickInfoShowComponentsNames).UseVisualStyleBackColor = true;
		((Control)rbQuickInfoShowFieldsOnly).AutoSize = true;
		((Control)rbQuickInfoShowFieldsOnly).Location = new Point(12, 153);
		((Control)rbQuickInfoShowFieldsOnly).Name = "rbQuickInfoShowFieldsOnly";
		((Control)rbQuickInfoShowFieldsOnly).Size = new Size(100, 17);
		((Control)rbQuickInfoShowFieldsOnly).TabIndex = 11;
		((Control)rbQuickInfoShowFieldsOnly).Text = "ShowFieldsOnly";
		((ButtonBase)rbQuickInfoShowFieldsOnly).UseVisualStyleBackColor = true;
		((Control)rbQuickInfoShowFields).AutoSize = true;
		((Control)rbQuickInfoShowFields).Location = new Point(12, 124);
		((Control)rbQuickInfoShowFields).Name = "rbQuickInfoShowFields";
		((Control)rbQuickInfoShowFields).Size = new Size(79, 17);
		((Control)rbQuickInfoShowFields).TabIndex = 10;
		((Control)rbQuickInfoShowFields).Text = "ShowFields";
		((ButtonBase)rbQuickInfoShowFields).UseVisualStyleBackColor = true;
		((Control)rbQuickInfoShowContent).AutoSize = true;
		((Control)rbQuickInfoShowContent).Location = new Point(12, 95);
		((Control)rbQuickInfoShowContent).Name = "rbQuickInfoShowContent";
		((Control)rbQuickInfoShowContent).Size = new Size(89, 17);
		((Control)rbQuickInfoShowContent).TabIndex = 9;
		((Control)rbQuickInfoShowContent).Text = "ShowContent";
		((ButtonBase)rbQuickInfoShowContent).UseVisualStyleBackColor = true;
		((Control)rbQuickInfoShowEvents).AutoSize = true;
		((Control)rbQuickInfoShowEvents).Location = new Point(12, 182);
		((Control)rbQuickInfoShowEvents).Name = "rbQuickInfoShowEvents";
		((Control)rbQuickInfoShowEvents).Size = new Size(85, 17);
		((Control)rbQuickInfoShowEvents).TabIndex = 12;
		((Control)rbQuickInfoShowEvents).Text = "ShowEvents";
		((ButtonBase)rbQuickInfoShowEvents).UseVisualStyleBackColor = true;
		((Control)pageSaving).Controls.Add((Control)(object)gbSaveOther);
		((Control)pageSaving).Controls.Add((Control)(object)gbAutoSaveOptions);
		pageSaving.Dock = (DockStyle)5;
		pageSaving.Image = (Image)componentResourceManager.GetObject("pageSaving.Image");
		pageSaving.InvisibleVisible = false;
		((Control)pageSaving).Location = new Point(0, 0);
		((Control)pageSaving).Margin = new Padding(0);
		((Control)pageSaving).Name = "pageSaving";
		((Control)pageSaving).Padding = new Padding(0, 0, 1, 0);
		((Control)pageSaving).Size = new Size(397, 537);
		((Control)pageSaving).TabIndex = 4;
		((Control)pageSaving).Text = "Save";
		((Control)pageSaving).Visible = false;
		((Control)gbSaveOther).Controls.Add((Control)(object)cbAutoSaveReport);
		((Control)gbSaveOther).Controls.Add((Control)(object)checkBoxRequestChanges);
		((Control)gbSaveOther).Dock = (DockStyle)1;
		gbSaveOther.HeaderHeight = 24;
		((Control)gbSaveOther).Location = new Point(0, 98);
		((Control)gbSaveOther).Name = "gbSaveOther";
		((Control)gbSaveOther).Size = new Size(396, 94);
		((Control)gbSaveOther).TabIndex = 1;
		((Control)gbSaveOther).TabStop = false;
		((Control)gbSaveOther).Text = "Other";
		((Control)cbAutoSaveReport).Location = new Point(12, 35);
		((Control)cbAutoSaveReport).Name = "cbAutoSaveReport";
		((Control)cbAutoSaveReport).Size = new Size(343, 20);
		((Control)cbAutoSaveReport).TabIndex = 0;
		((Control)cbAutoSaveReport).Text = "Auto Save Report to C# (Vb.Net)  file";
		((ButtonBase)cbAutoSaveReport).UseVisualStyleBackColor = false;
		((Control)checkBoxRequestChanges).Location = new Point(12, 65);
		((Control)checkBoxRequestChanges).Name = "checkBoxRequestChanges";
		((Control)checkBoxRequestChanges).Size = new Size(343, 20);
		((Control)checkBoxRequestChanges).TabIndex = 1;
		((Control)checkBoxRequestChanges).Text = "Request Changes when Saving to Cloud";
		((ButtonBase)checkBoxRequestChanges).UseVisualStyleBackColor = false;
		((Control)gbAutoSaveOptions).Controls.Add((Control)(object)checkBoxEnableAutoSave);
		((Control)gbAutoSaveOptions).Controls.Add((Control)(object)labelSaveReportEvery);
		((Control)gbAutoSaveOptions).Controls.Add((Control)(object)comboBoxAutoSaveTime);
		((Control)gbAutoSaveOptions).Dock = (DockStyle)1;
		gbAutoSaveOptions.HeaderHeight = 24;
		((Control)gbAutoSaveOptions).Location = new Point(0, 0);
		((Control)gbAutoSaveOptions).Name = "gbAutoSaveOptions";
		((Control)gbAutoSaveOptions).Size = new Size(396, 98);
		((Control)gbAutoSaveOptions).TabIndex = 0;
		((Control)gbAutoSaveOptions).TabStop = false;
		((Control)gbAutoSaveOptions).Text = "Auto save options";
		((Control)checkBoxEnableAutoSave).Location = new Point(12, 35);
		((Control)checkBoxEnableAutoSave).Name = "checkBoxEnableAutoSave";
		((Control)checkBoxEnableAutoSave).Size = new Size(285, 17);
		((Control)checkBoxEnableAutoSave).TabIndex = 0;
		((Control)checkBoxEnableAutoSave).Text = "Enable Auto Save Mode";
		((ButtonBase)checkBoxEnableAutoSave).UseVisualStyleBackColor = false;
		((CheckBox)checkBoxEnableAutoSave).CheckedChanged += CheckBoxEnableAutoSave_CheckedChanged;
		((Control)labelSaveReportEvery).Location = new Point(12, 64);
		((Control)labelSaveReportEvery).Name = "labelSaveReportEvery";
		((Control)labelSaveReportEvery).Size = new Size(187, 21);
		((Control)labelSaveReportEvery).TabIndex = 1;
		((Control)labelSaveReportEvery).Text = "Save Report Every";
		((Label)labelSaveReportEvery).TextAlign = (ContentAlignment)16;
		((Control)comboBoxAutoSaveTime).Anchor = (AnchorStyles)13;
		comboBoxAutoSaveTime.DropDownStyle = (ComboBoxStyle)2;
		((ComboBox)comboBoxAutoSaveTime).Items.AddRange(new object[5] { "5", "10", "20", "30", "60" });
		((Control)comboBoxAutoSaveTime).Location = new Point(205, 64);
		((Control)comboBoxAutoSaveTime).Name = "comboBoxAutoSaveTime";
		((Control)comboBoxAutoSaveTime).Size = new Size(178, 21);
		((Control)comboBoxAutoSaveTime).TabIndex = 2;
		((Control)pageScheduler).Controls.Add((Control)(object)gbScheduleSmtpSettings);
		pageScheduler.Dock = (DockStyle)5;
		pageScheduler.Image = (Image)componentResourceManager.GetObject("pageScheduler.Image");
		pageScheduler.InvisibleVisible = false;
		((Control)pageScheduler).Location = new Point(0, 0);
		((Control)pageScheduler).Name = "pageScheduler";
		((Control)pageScheduler).Padding = new Padding(0, 0, 1, 0);
		((Control)pageScheduler).Size = new Size(397, 537);
		((Control)pageScheduler).TabIndex = 2;
		((Control)pageScheduler).Text = "Scheduler";
		((Control)pageScheduler).Visible = false;
		((Control)gbScheduleSmtpSettings).Controls.Add((Control)(object)labelSchedulerSenderEmail);
		((Control)gbScheduleSmtpSettings).Controls.Add((Control)(object)textBoxSchedulerSenderEmail);
		((Control)gbScheduleSmtpSettings).Controls.Add((Control)(object)labelSchedulerHost);
		((Control)gbScheduleSmtpSettings).Controls.Add((Control)(object)textBoxSchedulerHost);
		((Control)gbScheduleSmtpSettings).Controls.Add((Control)(object)labelSchedulerPort);
		((Control)gbScheduleSmtpSettings).Controls.Add((Control)(object)textBoxSchedulerPort);
		((Control)gbScheduleSmtpSettings).Controls.Add((Control)(object)labelSchedulerUserName);
		((Control)gbScheduleSmtpSettings).Controls.Add((Control)(object)textBoxSchedulerUserName);
		((Control)gbScheduleSmtpSettings).Controls.Add((Control)(object)labelSchedulerPassword);
		((Control)gbScheduleSmtpSettings).Controls.Add((Control)(object)textBoxSchedulerPassword);
		((Control)gbScheduleSmtpSettings).Controls.Add((Control)(object)checkBoxSchedulerEnableSSL);
		((Control)gbScheduleSmtpSettings).Dock = (DockStyle)1;
		gbScheduleSmtpSettings.HeaderHeight = 24;
		((Control)gbScheduleSmtpSettings).Location = new Point(0, 0);
		((Control)gbScheduleSmtpSettings).Name = "gbScheduleSmtpSettings";
		((Control)gbScheduleSmtpSettings).Size = new Size(396, 380);
		((Control)gbScheduleSmtpSettings).TabIndex = 2;
		((Control)gbScheduleSmtpSettings).TabStop = false;
		((Control)gbScheduleSmtpSettings).Text = "Smtp Settings";
		((Control)labelSchedulerSenderEmail).Location = new Point(12, 36);
		((Control)labelSchedulerSenderEmail).Name = "labelSchedulerSenderEmail";
		((Control)labelSchedulerSenderEmail).Size = new Size(138, 20);
		((Control)labelSchedulerSenderEmail).TabIndex = 80;
		((Control)labelSchedulerSenderEmail).Text = "Sender Email";
		((Label)labelSchedulerSenderEmail).TextAlign = (ContentAlignment)16;
		((Control)textBoxSchedulerSenderEmail).Location = new Point(155, 36);
		((Control)textBoxSchedulerSenderEmail).Name = "textBoxSchedulerSenderEmail";
		((Control)textBoxSchedulerSenderEmail).Size = new Size(200, 21);
		((Control)textBoxSchedulerSenderEmail).TabIndex = 81;
		((Control)labelSchedulerHost).Location = new Point(12, 69);
		((Control)labelSchedulerHost).Name = "labelSchedulerHost";
		((Control)labelSchedulerHost).Size = new Size(138, 20);
		((Control)labelSchedulerHost).TabIndex = 82;
		((Control)labelSchedulerHost).Text = "Host";
		((Label)labelSchedulerHost).TextAlign = (ContentAlignment)16;
		((Control)textBoxSchedulerHost).Location = new Point(155, 69);
		((Control)textBoxSchedulerHost).Name = "textBoxSchedulerHost";
		((Control)textBoxSchedulerHost).Size = new Size(200, 21);
		((Control)textBoxSchedulerHost).TabIndex = 83;
		((Control)labelSchedulerPort).Location = new Point(12, 102);
		((Control)labelSchedulerPort).Name = "labelSchedulerPort";
		((Control)labelSchedulerPort).Size = new Size(138, 20);
		((Control)labelSchedulerPort).TabIndex = 84;
		((Control)labelSchedulerPort).Text = "Port";
		((Label)labelSchedulerPort).TextAlign = (ContentAlignment)16;
		textBoxSchedulerPort.DecimalPlaces = 0;
		((Control)textBoxSchedulerPort).Location = new Point(155, 102);
		((Control)textBoxSchedulerPort).Name = "textBoxSchedulerPort";
		((Control)textBoxSchedulerPort).Size = new Size(200, 21);
		((Control)textBoxSchedulerPort).TabIndex = 85;
		((Control)labelSchedulerUserName).Location = new Point(12, 135);
		((Control)labelSchedulerUserName).Name = "labelSchedulerUserName";
		((Control)labelSchedulerUserName).Size = new Size(138, 20);
		((Control)labelSchedulerUserName).TabIndex = 86;
		((Control)labelSchedulerUserName).Text = "User Name";
		((Label)labelSchedulerUserName).TextAlign = (ContentAlignment)16;
		((Control)textBoxSchedulerUserName).Location = new Point(155, 135);
		((Control)textBoxSchedulerUserName).Name = "textBoxSchedulerUserName";
		((Control)textBoxSchedulerUserName).Size = new Size(200, 21);
		((Control)textBoxSchedulerUserName).TabIndex = 87;
		((Control)labelSchedulerPassword).Location = new Point(12, 168);
		((Control)labelSchedulerPassword).Name = "labelSchedulerPassword";
		((Control)labelSchedulerPassword).Size = new Size(138, 20);
		((Control)labelSchedulerPassword).TabIndex = 88;
		((Control)labelSchedulerPassword).Text = "Password";
		((Label)labelSchedulerPassword).TextAlign = (ContentAlignment)16;
		((Control)textBoxSchedulerPassword).Location = new Point(155, 168);
		((Control)textBoxSchedulerPassword).Name = "textBoxSchedulerPassword";
		textBoxSchedulerPassword.PasswordChar = '●';
		((Control)textBoxSchedulerPassword).Size = new Size(200, 21);
		((Control)textBoxSchedulerPassword).TabIndex = 89;
		((Control)checkBoxSchedulerEnableSSL).Location = new Point(155, 201);
		((Control)checkBoxSchedulerEnableSSL).Name = "checkBoxSchedulerEnableSSL";
		((Control)checkBoxSchedulerEnableSSL).Size = new Size(200, 20);
		((Control)checkBoxSchedulerEnableSSL).TabIndex = 90;
		((Control)checkBoxSchedulerEnableSSL).Text = "Enable SSL";
		((ButtonBase)checkBoxSchedulerEnableSSL).UseVisualStyleBackColor = false;
		((Control)panelFooter).Controls.Add((Control)(object)btRestoreDefaults);
		((Control)panelFooter).Controls.Add((Control)(object)btCancel);
		((Control)panelFooter).Controls.Add((Control)(object)btOk);
		((Control)panelFooter).Dock = (DockStyle)2;
		panelFooter.LinePosition = StiLinePosition.Top;
		panelFooter.LineStyle = StiLineStyle.Dot;
		((Control)panelFooter).Location = new Point(0, 537);
		((Control)panelFooter).Name = "panelFooter";
		((Control)panelFooter).Size = new Size(516, 46);
		((Control)panelFooter).TabIndex = 1;
		((Control)btRestoreDefaults).Anchor = (AnchorStyles)6;
		((Control)btRestoreDefaults).Location = new Point(12, 12);
		((Control)btRestoreDefaults).Name = "btRestoreDefaults";
		((Control)btRestoreDefaults).Size = new Size(179, 24);
		((Control)btRestoreDefaults).TabIndex = 0;
		((Control)btRestoreDefaults).Text = "Restore Defaults";
		((ButtonBase)btRestoreDefaults).UseVisualStyleBackColor = true;
		((Control)btRestoreDefaults).Click += ButtonRestoreDefaults_Click;
		((Control)gbMarkersStyle).Controls.Add((Control)(object)rbMarkersStyleNone);
		((Control)gbMarkersStyle).Controls.Add((Control)(object)rbMarkersStyleCorners);
		((Control)gbMarkersStyle).Controls.Add((Control)(object)rbMarkersStyleDashedRectangle);
		((Control)gbMarkersStyle).Dock = (DockStyle)1;
		gbMarkersStyle.HeaderHeight = 24;
		((Control)gbMarkersStyle).Location = new Point(0, 178);
		((Control)gbMarkersStyle).Name = "gbMarkersStyle";
		((Control)gbMarkersStyle).Size = new Size(389, 116);
		((Control)gbMarkersStyle).TabIndex = 3;
		((Control)gbMarkersStyle).TabStop = false;
		((Control)gbMarkersStyle).Text = "Marker Style";
		((Control)rbMarkersStyleNone).BackColor = Color.White;
		((Control)rbMarkersStyleNone).Location = new Point(14, 30);
		((Control)rbMarkersStyleNone).Name = "rbMarkersStyleNone";
		((Control)rbMarkersStyleNone).Size = new Size(344, 24);
		((Control)rbMarkersStyleNone).TabIndex = 11;
		((Control)rbMarkersStyleNone).Text = "None";
		((ButtonBase)rbMarkersStyleNone).UseVisualStyleBackColor = false;
		((Control)rbMarkersStyleCorners).BackColor = Color.White;
		((Control)rbMarkersStyleCorners).Location = new Point(14, 54);
		((Control)rbMarkersStyleCorners).Name = "rbMarkersStyleCorners";
		((Control)rbMarkersStyleCorners).Size = new Size(344, 24);
		((Control)rbMarkersStyleCorners).TabIndex = 12;
		((Control)rbMarkersStyleCorners).Text = "Corners";
		((ButtonBase)rbMarkersStyleCorners).UseVisualStyleBackColor = false;
		((Control)rbMarkersStyleDashedRectangle).BackColor = Color.White;
		((Control)rbMarkersStyleDashedRectangle).Location = new Point(14, 78);
		((Control)rbMarkersStyleDashedRectangle).Name = "rbMarkersStyleDashedRectangle";
		((Control)rbMarkersStyleDashedRectangle).Size = new Size(344, 24);
		((Control)rbMarkersStyleDashedRectangle).TabIndex = 13;
		((Control)rbMarkersStyleDashedRectangle).Text = "DashedRectangle";
		((ButtonBase)rbMarkersStyleDashedRectangle).UseVisualStyleBackColor = false;
		((Control)gbReportDisplayOptions).Controls.Add((Control)(object)cbDrawMarkersWhenMoving);
		((Control)gbReportDisplayOptions).Controls.Add((Control)(object)cbFillComponents);
		((Control)gbReportDisplayOptions).Controls.Add((Control)(object)cbFillBands);
		((Control)gbReportDisplayOptions).Controls.Add((Control)(object)cbUseComponentColor);
		((Control)gbReportDisplayOptions).Controls.Add((Control)(object)cbFillCrossBands);
		((Control)gbReportDisplayOptions).Controls.Add((Control)(object)cbFillContainers);
		((Control)gbReportDisplayOptions).Dock = (DockStyle)1;
		gbReportDisplayOptions.HeaderHeight = 24;
		((Control)gbReportDisplayOptions).Location = new Point(0, 0);
		((Control)gbReportDisplayOptions).Name = "gbReportDisplayOptions";
		((Control)gbReportDisplayOptions).Size = new Size(389, 178);
		((Control)gbReportDisplayOptions).TabIndex = 2;
		((Control)gbReportDisplayOptions).TabStop = false;
		((Control)gbReportDisplayOptions).Text = "Report display options";
		((Control)cbDrawMarkersWhenMoving).BackColor = Color.White;
		((Control)cbDrawMarkersWhenMoving).Location = new Point(14, 148);
		((Control)cbDrawMarkersWhenMoving).Name = "cbDrawMarkersWhenMoving";
		((Control)cbDrawMarkersWhenMoving).Size = new Size(344, 24);
		((Control)cbDrawMarkersWhenMoving).TabIndex = 12;
		((Control)cbDrawMarkersWhenMoving).Text = "DrawMarkersWhenMoving";
		((ButtonBase)cbDrawMarkersWhenMoving).UseVisualStyleBackColor = false;
		((Control)cbFillComponents).BackColor = Color.White;
		((Control)cbFillComponents).Location = new Point(14, 100);
		((Control)cbFillComponents).Name = "cbFillComponents";
		((Control)cbFillComponents).Size = new Size(344, 24);
		((Control)cbFillComponents).TabIndex = 10;
		((Control)cbFillComponents).Text = "FillComponents";
		((ButtonBase)cbFillComponents).UseVisualStyleBackColor = false;
		((Control)cbFillBands).BackColor = Color.White;
		((Control)cbFillBands).Location = new Point(14, 28);
		((Control)cbFillBands).Name = "cbFillBands";
		((Control)cbFillBands).Size = new Size(344, 24);
		((Control)cbFillBands).TabIndex = 7;
		((Control)cbFillBands).Text = "FillBands";
		((ButtonBase)cbFillBands).UseVisualStyleBackColor = false;
		((Control)cbUseComponentColor).BackColor = Color.White;
		((Control)cbUseComponentColor).Location = new Point(14, 124);
		((Control)cbUseComponentColor).Name = "cbUseComponentColor";
		((Control)cbUseComponentColor).Size = new Size(344, 24);
		((Control)cbUseComponentColor).TabIndex = 11;
		((Control)cbUseComponentColor).Text = "UseComponentColor";
		((ButtonBase)cbUseComponentColor).UseVisualStyleBackColor = false;
		((Control)cbFillCrossBands).BackColor = Color.White;
		((Control)cbFillCrossBands).Location = new Point(14, 52);
		((Control)cbFillCrossBands).Name = "cbFillCrossBands";
		((Control)cbFillCrossBands).Size = new Size(344, 24);
		((Control)cbFillCrossBands).TabIndex = 8;
		((Control)cbFillCrossBands).Text = "FillCrossBands";
		((ButtonBase)cbFillCrossBands).UseVisualStyleBackColor = false;
		((Control)cbFillContainers).BackColor = Color.White;
		((Control)cbFillContainers).Location = new Point(14, 76);
		((Control)cbFillContainers).Name = "cbFillContainers";
		((Control)cbFillContainers).Size = new Size(344, 24);
		((Control)cbFillContainers).TabIndex = 9;
		((Control)cbFillContainers).Text = "FillContainers";
		((ButtonBase)cbFillContainers).UseVisualStyleBackColor = false;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(516, 583);
		((Control)this).Controls.Add((Control)(object)TabControl);
		((Control)this).Controls.Add((Control)(object)panelFooter);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiOptionsForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Options";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Control)TabControl).ResumeLayout(false);
		((Control)pageMain).ResumeLayout(false);
		((Control)panelNewReport).ResumeLayout(false);
		((Control)panelScalingMode).ResumeLayout(false);
		((Control)panelStartScreen).ResumeLayout(false);
		((Control)panelDefaultScriptMode).ResumeLayout(false);
		((Control)panelDeveloper1).ResumeLayout(false);
		((Control)panelDeveloperCreator1).ResumeLayout(false);
		((Control)pageDesign).ResumeLayout(false);
		((Control)gbGridSize).ResumeLayout(false);
		((Control)gbGridDrawingOptions).ResumeLayout(false);
		((Control)gbGridOptions).ResumeLayout(false);
		((Control)gbViewOptions).ResumeLayout(false);
		((Control)pageQuickInfo).ResumeLayout(false);
		((Control)gbOptionsOfQuickInfo).ResumeLayout(false);
		((Control)gbOptionsOfQuickInfo).PerformLayout();
		((Control)pageSaving).ResumeLayout(false);
		((Control)gbSaveOther).ResumeLayout(false);
		((Control)gbAutoSaveOptions).ResumeLayout(false);
		((Control)pageScheduler).ResumeLayout(false);
		((Control)gbScheduleSmtpSettings).ResumeLayout(false);
		((ISupportInitialize)textBoxSchedulerPort).EndInit();
		((Control)panelFooter).ResumeLayout(false);
		((Control)gbMarkersStyle).ResumeLayout(false);
		((Control)gbReportDisplayOptions).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
