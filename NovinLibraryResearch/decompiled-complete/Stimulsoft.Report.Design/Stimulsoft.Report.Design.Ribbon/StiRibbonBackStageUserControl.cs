using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Server;
using Stimulsoft.Base.TaskScheduler;
using Stimulsoft.Base.Wizards;
using Stimulsoft.Controls;
using Stimulsoft.Controls.DesktopIntegration;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.App;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;
using Stimulsoft.Report.Design.Components;
using Stimulsoft.Report.Design.Dashboard.Actions;
using Stimulsoft.Report.Design.Helpers;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Design.Inplace;
using Stimulsoft.Report.Design.Properties;
using Stimulsoft.Report.Design.Wizards;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Design.Ribbon;

[ToolboxItem(false)]
public class StiRibbonBackStageUserControl : UserControl
{
	private List<ButtonX> newItems = new List<ButtonX>();

	public readonly StiDesignerControl Designer;

	private ElementHost startScreenElementHost;

	private IStiWizardStartScreenControl wizardStartScreenControl;

	private ElementHost newReportWizardElementHost;

	private IStiNewReportWizardControl newReportWizardControl;

	private ElementHost cloudOpenControlElementHost;

	private IStiCloudOpenControl cloudOpenControl;

	private ElementHost accountControlElementHost;

	private ElementHost schedulerControlElementHost;

	private IStiAccountControl accountControl;

	private IStiSchedulerItemsControl schedulerItemsControl;

	private IContainer components;

	public SuperTabControl BackStage;

	private SuperTabControlPanel superTabControlPanelInfo;

	private SuperTabControlPanel superTabControlPanelNew;

	internal SuperTabItem buttonInfo;

	internal SuperTabItem buttonNew;

	internal SuperTabItem buttonReportOpen;

	private SuperTabControlPanel superTabControlPanelOpen;

	private SuperTabControlPanel superTabControlPanelSchedule;

	private SuperTabControlPanel superTabControlPanelHelp;

	private Panel panelSaveAs;

	private LabelX labelTitleSaveAs;

	private StiCloudTabulator tabControlSaveAs;

	private StiCloudTabulator tabControlHelp;

	private StiCloudTabulatorPage pageCloudSave;

	private StiCloudTabulatorPage pageSaveAs;

	private StiCloudTabulatorPage pageHelpDocumentation;

	private StiCloudTabulatorPage pageHelpSupport;

	private StiCloudTabulatorPage pageHelpSeparator1;

	private StiCloudTabulatorPage pageHelpVideos;

	private StiCloudTabulatorPage pageHelpSamples;

	private StiCloudTabulatorPage pageHelpSeparator2;

	private StiCloudTabulatorPage pageHelpForum;

	private StiCloudTabulatorPage pageHelpDemos;

	private StiCloudTabulatorPage pageHelpTrainingCourses;

	private StiCloudTabulatorPage pageHelpGitHub;

	private StiCloudTabulatorPage pageHelpNuget;

	private SuperTabControlPanel superTabControlPanelSaveAs;

	private LabelX labelRecentSeparator;

	private ImageList imageList1;

	private FlowLayoutPanel reportFlowItems;

	private FlowLayoutPanel dashboardFlowItems;

	private FlowLayoutPanel formFlowItems;

	private Panel panelItems;

	internal SuperTabItem buttonPreview;

	internal SuperTabItem buttonOptions;

	internal SuperTabItem buttonHelp;

	internal SuperTabItem buttonReportSave;

	internal SuperTabItem buttonReportSaveAs;

	internal SuperTabItem buttonSchedule;

	internal SuperTabItem buttonShare;

	internal SuperTabItem buttonPublish;

	internal SuperTabItem buttonGetStarted;

	internal SuperTabItem buttonClose;

	internal SuperTabItem buttonAbout;

	private StiCloudTabulator tabControl;

	private Panel panelScheduleData;

	private StiCloudTabulatorPage pageRecent;

	private StiCloudTabulatorPage pageImport;

	private StiCloudTabulatorPage pageCloud;

	private StiCloudTabulatorPage pageOpen;

	private Panel panel8;

	private ButtonX biRecentLocation09;

	private LabelX labelRecentLocations;

	private ButtonX biRecentLocation08;

	private ButtonX biRecentLocation01;

	private ButtonX biRecentLocation07;

	private ButtonX biRecentLocation02;

	private ButtonX biRecentLocation06;

	private ButtonX biRecentLocation03;

	private ButtonX biRecentLocation05;

	private ButtonX biRecentLocation04;

	private Panel panel7;

	private ButtonX biRecentFile09;

	private ButtonX biRecentFile08;

	private ButtonX biRecentFile07;

	private ButtonX biRecentFile06;

	private ButtonX biRecentFile05;

	private ButtonX biRecentFile04;

	private ButtonX biRecentFile03;

	private ButtonX biRecentFile02;

	private ButtonX biRecentFile01;

	private LabelX labelRecentDocuments;

	private Panel panel1;

	private Panel panelScheduleTitle;

	private Panel panelHelp;

	private LabelX labelTitleOpen;

	private LabelX labelTitleSchedule;

	private LabelX labelTitleHelp;

	private Panel panelImport;

	private Panel panelCloud;

	private ButtonX buttonXtraReports;

	private ButtonX buttonTelerikReporting;

	private ButtonX buttonMSRS;

	private ButtonX buttonRTF;

	private ButtonX buttonReportSharpShooter;

	private ButtonX buttonFastReport;

	private ButtonX buttonCrystalReports;

	private ButtonX buttonComponentOne;

	private ButtonX buttonActiveReports;

	private LabelX labelImport;

	private ButtonX buttonVisualFoxPro;

	private Panel panel2;

	private LabelX labelTitleInfo;

	internal Button buttonReportOptions;

	internal Button buttonCheckIssue;

	private Button buttonProtectDocument;

	private LabelX labelCheckIssues;

	private LabelX labelProtectDocument;

	private LabelX labelReportOptions;

	private LabelX labelCheckIssuesTitle;

	private LabelX labelProtectDocumentTitle;

	private LabelX labelReportOptionsTitle;

	private Panel panel3;

	private LabelX labelTitleNew;

	private SuperTabControlPanel superTabControlPanel2;

	private SuperTabItem buttonBack;

	private LabelItem labelBackSeparator;

	private SuperTabControlPanel superTabControlPanelAccount;

	public SuperTabItem buttonAccount;

	private Panel panelAccount;

	private Panel panel4;

	private LabelX labelTitleAccount;

	public bool StartScreenVisible { get; private set; }

	public StiRibbonBackStageUserControl(StiDesignerControl designer)
	{
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Expected O, but got Unknown
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Expected O, but got Unknown
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Expected O, but got Unknown
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Expected O, but got Unknown
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Expected O, but got Unknown
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Expected O, but got Unknown
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Expected O, but got Unknown
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Expected O, but got Unknown
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Expected O, but got Unknown
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Expected O, but got Unknown
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Expected O, but got Unknown
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Expected O, but got Unknown
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Expected O, but got Unknown
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Expected O, but got Unknown
		//IL_0369: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Expected O, but got Unknown
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Expected O, but got Unknown
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Expected O, but got Unknown
		//IL_03d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e2: Expected O, but got Unknown
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0407: Expected O, but got Unknown
		//IL_0422: Unknown result type (might be due to invalid IL or missing references)
		//IL_042c: Expected O, but got Unknown
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_0451: Expected O, but got Unknown
		//IL_046c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0476: Expected O, but got Unknown
		//IL_0491: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Expected O, but got Unknown
		//IL_04b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c0: Expected O, but got Unknown
		//IL_04db: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e5: Expected O, but got Unknown
		//IL_0500: Unknown result type (might be due to invalid IL or missing references)
		//IL_050a: Expected O, but got Unknown
		//IL_0525: Unknown result type (might be due to invalid IL or missing references)
		//IL_052f: Expected O, but got Unknown
		//IL_054a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0554: Expected O, but got Unknown
		//IL_056f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0579: Expected O, but got Unknown
		//IL_0594: Unknown result type (might be due to invalid IL or missing references)
		//IL_059e: Expected O, but got Unknown
		//IL_05b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c3: Expected O, but got Unknown
		//IL_05de: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e8: Expected O, but got Unknown
		//IL_0603: Unknown result type (might be due to invalid IL or missing references)
		//IL_060d: Expected O, but got Unknown
		//IL_0628: Unknown result type (might be due to invalid IL or missing references)
		//IL_0632: Expected O, but got Unknown
		//IL_064d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0657: Expected O, but got Unknown
		//IL_0672: Unknown result type (might be due to invalid IL or missing references)
		//IL_067c: Expected O, but got Unknown
		//IL_0697: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a1: Expected O, but got Unknown
		//IL_06bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c6: Expected O, but got Unknown
		//IL_06e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06eb: Expected O, but got Unknown
		//IL_0706: Unknown result type (might be due to invalid IL or missing references)
		//IL_0710: Expected O, but got Unknown
		InitializeComponent();
		Designer = designer;
		Localize();
		buttonReportOpen.Image = null;
		buttonPreview.Image = null;
		buttonReportSave.Image = null;
		buttonReportSaveAs.Image = null;
		buttonClose.Image = null;
		buttonOptions.Image = null;
		buttonAbout.Image = null;
		buttonHelp.Image = null;
		labelRecentDocuments.BackgroundStyle.BorderBottom = eStyleBorderType.None;
		labelRecentLocations.BackgroundStyle.BorderBottom = eStyleBorderType.None;
		labelImport.BackgroundStyle.BorderBottom = eStyleBorderType.None;
		labelRecentSeparator.BackgroundStyle.BorderLeft = eStyleBorderType.None;
		((Control)labelRecentDocuments).ForeColor = Color.FromArgb(255, 85, 121, 174);
		((Control)labelRecentLocations).ForeColor = Color.FromArgb(255, 85, 121, 174);
		((Control)labelImport).ForeColor = Color.FromArgb(255, 85, 121, 174);
		((Control)labelRecentDocuments).Font = new Font("Microsoft Sans Serif", 15f);
		((Control)labelRecentLocations).Font = new Font("Microsoft Sans Serif", 15f);
		((Control)labelImport).Font = new Font("Microsoft Sans Serif", 15f);
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiRibbonBackStageUserControl));
		buttonBack.Image = StiReportDesignImages.SystemParts.Back(StiImageSize.Double);
		buttonInfo.Image = (Image)componentResourceManager.GetObject("Home" + StiScale.StepName);
		buttonNew.Image = (Image)componentResourceManager.GetObject("Report" + StiScale.StepName);
		buttonReportOpen.Image = (Image)componentResourceManager.GetObject("Folder" + StiScale.StepName);
		buttonReportSave.Image = (Image)componentResourceManager.GetObject("Empty" + StiScale.StepName);
		buttonReportSaveAs.Image = (Image)componentResourceManager.GetObject("Empty" + StiScale.StepName);
		buttonPreview.Image = (Image)componentResourceManager.GetObject("Empty" + StiScale.StepName);
		buttonSchedule.Image = (Image)componentResourceManager.GetObject("Empty" + StiScale.StepName);
		buttonShare.Image = (Image)componentResourceManager.GetObject("Empty" + StiScale.StepName);
		buttonPublish.Image = (Image)componentResourceManager.GetObject("Empty" + StiScale.StepName);
		buttonHelp.Image = (Image)componentResourceManager.GetObject("Empty" + StiScale.StepName);
		buttonGetStarted.Image = (Image)componentResourceManager.GetObject("Empty" + StiScale.StepName);
		buttonClose.Image = (Image)componentResourceManager.GetObject("Empty" + StiScale.StepName);
		buttonAccount.Image = (Image)componentResourceManager.GetObject("Empty" + StiScale.StepName);
		buttonOptions.Image = (Image)componentResourceManager.GetObject("Empty" + StiScale.StepName);
		buttonAbout.Image = (Image)componentResourceManager.GetObject("Empty" + StiScale.StepName);
		pageRecent.Image = (Image)componentResourceManager.GetObject("pageRecent.Image" + StiScale.StepName);
		pageImport.Image = (Image)componentResourceManager.GetObject("pageImport.Image" + StiScale.StepName);
		buttonVisualFoxPro.Image = (Image)componentResourceManager.GetObject("pageImport.Image" + StiScale.StepName);
		buttonXtraReports.Image = (Image)componentResourceManager.GetObject("pageImport.Image" + StiScale.StepName);
		buttonTelerikReporting.Image = (Image)componentResourceManager.GetObject("pageImport.Image" + StiScale.StepName);
		buttonMSRS.Image = (Image)componentResourceManager.GetObject("pageImport.Image" + StiScale.StepName);
		buttonRTF.Image = (Image)componentResourceManager.GetObject("pageImport.Image" + StiScale.StepName);
		buttonReportSharpShooter.Image = (Image)componentResourceManager.GetObject("pageImport.Image" + StiScale.StepName);
		buttonFastReport.Image = (Image)componentResourceManager.GetObject("pageImport.Image" + StiScale.StepName);
		buttonCrystalReports.Image = (Image)componentResourceManager.GetObject("pageImport.Image" + StiScale.StepName);
		buttonComponentOne.Image = (Image)componentResourceManager.GetObject("pageImport.Image" + StiScale.StepName);
		buttonActiveReports.Image = (Image)componentResourceManager.GetObject("pageImport.Image" + StiScale.StepName);
		pageCloud.Image = (Image)componentResourceManager.GetObject("pageCloud.Image" + StiScale.StepName);
		pageOpen.Image = (Image)componentResourceManager.GetObject("pageOpen.Image" + StiScale.StepName);
		pageCloudSave.Image = (Image)componentResourceManager.GetObject("pageCloudSave.Image" + StiScale.StepName);
		pageSaveAs.Image = (Image)componentResourceManager.GetObject("pageSaveAs.Image" + StiScale.StepName);
		pageHelpDocumentation.Image = (Image)componentResourceManager.GetObject("pageHelpDocumentation" + StiScale.StepName);
		pageHelpSupport.Image = (Image)componentResourceManager.GetObject("pageHelpSupport" + StiScale.StepName);
		pageHelpVideos.Image = (Image)componentResourceManager.GetObject("pageHelpVideos" + StiScale.StepName);
		pageHelpSamples.Image = (Image)componentResourceManager.GetObject("pageHelpSamples" + StiScale.StepName);
		pageHelpForum.Image = (Image)componentResourceManager.GetObject("pageHelpForum" + StiScale.StepName);
		pageHelpDemos.Image = (Image)componentResourceManager.GetObject("pageHelpDemos" + StiScale.StepName);
		pageHelpTrainingCourses.Image = (Image)componentResourceManager.GetObject("pageHelpTrainingCourses" + StiScale.StepName);
		((ButtonBase)buttonReportOptions).Image = StiReportDesignImages.Actions.ReportOptions(StiImageSize.Double);
		((ButtonBase)buttonCheckIssue).Image = StiReportDesignImages.Actions.CheckForIssues(StiImageSize.Double);
		((ButtonBase)buttonProtectDocument).Image = StiReportDesignImages.Actions.ProtectReport(StiImageSize.Double);
		buttonBack.Visible = true;
		InitTitles();
		buttonClose.Visible = StiOptions.Designer.Ribbon.ShowMainMenuClose;
		buttonHelp.Visible = StiOptions.Designer.Ribbon.ShowMainMenuHelp;
		buttonNew.Visible = StiOptions.Designer.Ribbon.ShowMainMenuNew;
		buttonOptions.Visible = StiOptions.Designer.Ribbon.ShowMainMenuOptions;
		((Control)buttonCheckIssue).Visible = StiOptions.Designer.Ribbon.ShowMainMenuCheckForIssues;
		((Control)buttonReportOptions).Visible = StiOptions.Designer.Ribbon.ShowMainMenuReportSetup;
		pageRecent.Invisible = !StiOptions.Designer.Ribbon.ShowMainMenuRecentFiles;
		CheckInvisiblePageImport();
		buttonPreview.Visible = StiOptions.Designer.Ribbon.ShowMainMenuReportPreview;
		buttonReportOpen.Visible = StiOptions.Designer.Ribbon.ShowMainMenuReportOpen;
		buttonReportSave.Visible = StiOptions.Designer.Ribbon.ShowMainMenuReportSave;
		buttonReportSaveAs.Visible = StiOptions.Designer.Ribbon.ShowMainMenuReportSaveAs;
		buttonAbout.Visible = StiOptions.Designer.Ribbon.ShowAbout;
		buttonReportOpen.Enabled = StiActions.MainMenu.States.GetReportOpenEnabled(Designer);
		buttonPreview.Enabled = StiActions.MainMenu.States.GetReportPreviewEnabled(Designer);
		buttonReportSave.Enabled = StiActions.MainMenu.States.GetReportSaveEnabled(Designer);
		buttonReportSaveAs.Enabled = StiActions.MainMenu.States.GetReportSaveAsEnabled(Designer);
		buttonClose.Enabled = StiActions.MainMenu.States.GetCloseEnabled(Designer);
		((Control)buttonCheckIssue).Enabled = StiActions.MainMenu.States.GetCheckIssuesEnabled(Designer);
		((Control)buttonReportOptions).Enabled = StiActions.MainMenu.States.GetReportSetupEnabled(Designer);
		buttonOptions.Enabled = StiActions.MainMenu.States.GetOptionsEnabled(Designer);
		buttonBack.FixedTabSize = new Size(0, StiScale.YYI(42.0));
		BackStage.TabVerticalSpacing = StiScale.YYI(BackStage.TabVerticalSpacing);
		BackStage.FixedTabSize = new Size(StiScale.XXI(150.0), 0);
		BackStage.ItemPadding.Left = StiScale.XXI(6.0);
		BackStage.ItemPadding.Right = StiScale.XXI(4.0);
		BackStage.ItemPadding.Top = StiScale.YYI(4.0);
		BackStage.TabHorizontalSpacing = StiScale.XXI(10.0);
		tabControl.TitleWidth = StiScale.XXI(tabControl.TitleWidth);
		tabControlSaveAs.TitleWidth = StiScale.XXI(tabControlSaveAs.TitleWidth);
		tabControlHelp.TitleWidth = StiScale.XXI(tabControlHelp.TitleWidth);
		((Control)pageCloud).Visible = false;
		pageCloud.TabClick += PageCloud_TabClick;
		pageCloudSave.TabClick += PageCloudSave_TabClick;
		pageSaveAs.TabClick += PageSaveAs_TabClick;
		pageHelpDocumentation.TabClick += PageHelpDocumentation_TabClick;
		pageHelpSupport.TabClick += PageHelpSupport_TabClick;
		pageHelpVideos.TabClick += PageHelpVideos_TabClick;
		pageHelpSamples.TabClick += PageHelpSamples_TabClick;
		pageHelpForum.TabClick += PageHelpForum_TabClick;
		pageHelpDemos.TabClick += PageHelpDemos_TabClick;
		pageHelpTrainingCourses.TabClick += PageHelpTrainingCourses_TabClick;
		if (StiAccountSettings.CurrentAccount == null)
		{
			buttonAccount.Visible = false;
		}
	}

	private void InitTitles()
	{
		InitFontTitle(labelTitleSaveAs);
		InitFontTitle(labelTitleOpen);
		InitFontTitle(labelTitleSchedule);
		InitFontTitle(labelTitleInfo);
		InitFontTitle(labelTitleHelp);
		InitFontTitle(labelTitleNew);
		InitFontTitle(labelTitleInfo);
		InitFontTitle(labelTitleAccount);
	}

	private void InitFontTitle(LabelX label)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		label.BackgroundStyle.BorderBottom = eStyleBorderType.None;
		((Control)label).ForeColor = Color.FromArgb(100, 68, 68, 68);
		((Control)label).Font = new Font("Microsoft Sans Serif", 24f);
	}

	private void InitNewReportWizard()
	{
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		if (StiAccountSettings.CurrentAccount != null)
		{
			newReportWizardControl = StiWizardsCreater.GetNewReportWizardControl(isWinFormsMode: true, Designer, Designer.Report);
			if (newReportWizardControl != null)
			{
				((Control)panelItems).Controls.Clear();
				newReportWizardControl.ReportCreated += NewReportWizard_ReportCreated;
				newReportWizardControl.ReportPathChanged += StoreWizard_ReportPathChanged;
				newReportWizardControl.IntPtr = ((Control)Designer).Handle;
				ElementHost val = new ElementHost();
				((Control)val).Dock = (DockStyle)5;
				_003F val2 = val;
				IStiNewReportWizardControl stiNewReportWizardControl = newReportWizardControl;
				((ElementHost)val2).Child = (UIElement)((stiNewReportWizardControl is UIElement) ? stiNewReportWizardControl : null);
				newReportWizardElementHost = val;
				((Control)panelItems).Controls.Add((Control)(object)newReportWizardElementHost);
			}
		}
	}

	private void ShowReport(StiImportResult importResult)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		if (importResult.Report != null)
		{
			Designer.Report = importResult.Report;
		}
		else if (importResult.Errors != null && importResult.Errors.Count > 0)
		{
			MessageBox.Show(importResult.Errors.ToString(), "Errors", (MessageBoxButtons)0, (MessageBoxIcon)64);
		}
	}

	private void CheckInvisiblePageImport()
	{
		if (StiOptions.Designer.MainMenu.ImportActiveReports == null || StiOptions.Designer.MainMenu.ImportComponentOneReports == null || StiOptions.Designer.MainMenu.ImportFastReports == null || StiOptions.Designer.MainMenu.ImportMicrosoftReportingServices == null || StiOptions.Designer.MainMenu.ImportReportSharpShooter == null || StiOptions.Designer.MainMenu.ImportRtf == null || StiOptions.Designer.MainMenu.ImportTelerikReports == null || StiOptions.Designer.MainMenu.ImportVisualFoxPro == null)
		{
			pageImport.Invisible = true;
		}
		else
		{
			pageImport.Invisible = false;
		}
	}

	private static Task<Image> GetThumbnailAsync(string file, double pageWidth)
	{
		return Task.Run(() => GetThumbnail(file, pageWidth));
	}

	private static Image GetThumbnail(string file, double pageWidth)
	{
		StiReportTemplatesService service = StiReportTemplatesService.GetService();
		StiReport reportTemplate = service.GetReportTemplate(file);
		if (reportTemplate == null)
		{
			return null;
		}
		return GetThumbnail(reportTemplate.Pages[0], pageWidth);
	}

	private static Image GetThumbnail(StiPage page, double pageWidth)
	{
		try
		{
			StiDbsThumbnaiModelHelper.Add(page);
			if (page is IStiDashboard)
			{
				double num = pageWidth;
				if (StiScale.Id != 0)
				{
					num *= 2.0;
				}
				return (Image)(object)page.GetThumbnail((int)num);
			}
			if (page.Width > page.Height)
			{
				double num2 = page.Width / page.Height;
				return (Image)(object)page.GetThumbnail((int)pageWidth, (int)(pageWidth / num2), isDesignTime: true);
			}
			double num3 = page.Height / page.Width;
			return (Image)(object)page.GetThumbnail((int)(pageWidth / num3), (int)pageWidth, isDesignTime: true);
		}
		catch
		{
			return null;
		}
		finally
		{
			StiDbsThumbnaiModelHelper.Remove(page);
		}
	}

	private static string GetFormattedString(string title, string description)
	{
		if (string.IsNullOrEmpty(title))
		{
			return description;
		}
		return title.Replace("&", "&&") + "\r\n<br>" + description + "</br>";
	}

	public virtual void RefreshRibbonRecentFiles()
	{
		Designer.DesignerConfig.InitializeRecentFiles();
		int num = Designer.RecentFiles.Count;
		if (num > 9)
		{
			num = 9;
		}
		if (num > 15)
		{
			num = 15;
		}
		for (int i = 0; i < num; i++)
		{
			ButtonX buttonX = null;
			switch (i)
			{
			case 0:
				buttonX = biRecentFile01;
				break;
			case 1:
				buttonX = biRecentFile02;
				break;
			case 2:
				buttonX = biRecentFile03;
				break;
			case 3:
				buttonX = biRecentFile04;
				break;
			case 4:
				buttonX = biRecentFile05;
				break;
			case 5:
				buttonX = biRecentFile06;
				break;
			case 6:
				buttonX = biRecentFile07;
				break;
			case 7:
				buttonX = biRecentFile08;
				break;
			case 8:
				buttonX = biRecentFile09;
				break;
			}
			StiDisplayRecentFileObjectEventArgs stiDisplayRecentFileObjectEventArgs = StiOptions.Engine.GlobalEvents.InvokeDisplayRecentFileInDesigner(Designer, Designer.RecentFiles[i]);
			bool flag = stiDisplayRecentFileObjectEventArgs.FileName.StartsWith("cloud");
			((Control)buttonX).Visible = stiDisplayRecentFileObjectEventArgs.Visible;
			buttonX.Tooltip = (flag ? "Stimulsoft Cloud" : stiDisplayRecentFileObjectEventArgs.FileName.Replace("&", "&&"));
			((Control)buttonX).Tag = Designer.RecentFiles[i];
			((Control)buttonX).ContextMenuStrip = GetConextMenu(Designer.RecentFiles[i], flag);
			if (stiDisplayRecentFileObjectEventArgs.FileName.StartsWith("cloud"))
			{
				if (StiDashboardRecentHelper.ContainsDbs(stiDisplayRecentFileObjectEventArgs.RecentFile.Tag as string))
				{
					buttonX.Image = StiReportDesignImages.Files.DashboardCloudFile();
				}
				else
				{
					buttonX.Image = StiReportDesignImages.Files.ReportCloudFile();
				}
			}
			else if (StiReportContentHelper.ContainsDbs(stiDisplayRecentFileObjectEventArgs.FileName))
			{
				buttonX.Image = StiReportDesignImages.Files.DashboardFile();
			}
			else
			{
				buttonX.Image = StiReportDesignImages.Files.ReportFile();
			}
			string text = (stiDisplayRecentFileObjectEventArgs.FileName.StartsWith("cloud") ? "Stimulsoft Cloud" : Path.GetDirectoryName(stiDisplayRecentFileObjectEventArgs.FileName));
			string measureText = StiStringHelper.GetMeasureText(stiDisplayRecentFileObjectEventArgs.DisplayName, ((Control)buttonX).Font, StiScale.XX(280.0));
			text = StiStringHelper.GetMeasureText(text, ((Control)buttonX).Font, StiScale.XX(280.0));
			((Control)buttonX).Text = GetFormattedString(measureText, text);
		}
		for (int j = num; j < 9; j++)
		{
			ButtonX buttonX2 = null;
			if (j == 0)
			{
				buttonX2 = biRecentFile01;
			}
			if (j == 1)
			{
				buttonX2 = biRecentFile02;
			}
			if (j == 2)
			{
				buttonX2 = biRecentFile03;
			}
			if (j == 3)
			{
				buttonX2 = biRecentFile04;
			}
			if (j == 4)
			{
				buttonX2 = biRecentFile05;
			}
			if (j == 5)
			{
				buttonX2 = biRecentFile06;
			}
			if (j == 6)
			{
				buttonX2 = biRecentFile07;
			}
			if (j == 7)
			{
				buttonX2 = biRecentFile08;
			}
			if (j == 8)
			{
				buttonX2 = biRecentFile09;
			}
			((Control)buttonX2).Visible = false;
		}
		ButtonX buttonX3 = biRecentFile01;
		ButtonX buttonX4 = biRecentFile02;
		ButtonX buttonX5 = biRecentFile03;
		ButtonX buttonX6 = biRecentFile04;
		ButtonX buttonX7 = biRecentFile05;
		ButtonX buttonX8 = biRecentFile06;
		ButtonX buttonX9 = biRecentFile07;
		ButtonX buttonX10 = biRecentFile08;
		bool flag2 = (((Control)biRecentFile09).Enabled = StiActions.MainMenu.States.GetRecentFilesEnabled(Designer));
		bool flag4 = (((Control)buttonX10).Enabled = flag2);
		bool flag6 = (((Control)buttonX9).Enabled = flag4);
		bool flag8 = (((Control)buttonX8).Enabled = flag6);
		bool flag10 = (((Control)buttonX7).Enabled = flag8);
		bool flag12 = (((Control)buttonX6).Enabled = flag10);
		bool flag14 = (((Control)buttonX5).Enabled = flag12);
		bool enabled = (((Control)buttonX4).Enabled = flag14);
		((Control)buttonX3).Enabled = enabled;
	}

	private ContextMenuStrip GetConextMenu(StiRecentFile recentFile, bool isCloud)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Expected O, but got Unknown
		ContextMenuStrip val = new ContextMenuStrip();
		ToolStripMenuItem val2 = new ToolStripMenuItem(StiLocalization.Get("Buttons", "Remove"), StiReportDesignImages.Actions.Remove(), (EventHandler)RecentFileRemove_Click);
		((ToolStripItem)val2).Tag = recentFile;
		((ToolStrip)val).Items.Add((ToolStripItem)(object)val2);
		ToolStripMenuItem val3 = new ToolStripMenuItem(StiLocalization.Get("Buttons", "RemoveAll"), StiReportDesignImages.Actions.Remove(), (EventHandler)RecentFileRemoveAll_Click);
		((ToolStripItem)val3).Tag = recentFile;
		((ToolStrip)val).Items.Add((ToolStripItem)(object)val3);
		if (!isCloud)
		{
			ToolStripMenuItem val4 = new ToolStripMenuItem(StiLocalization.Get("Buttons", "OpenContainingFolder"), StiReportDesignImages.Actions.OpenFile(), (EventHandler)RecentFileOpen_Click);
			((ToolStripItem)val4).Tag = recentFile;
			((ToolStrip)val).Items.Add((ToolStripItem)(object)val4);
		}
		return val;
	}

	public virtual void RefreshRibbonRecentLocations()
	{
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Expected O, but got Unknown
		int num = Designer.RecentFiles.Count;
		if (num > 9)
		{
			num = 9;
		}
		if (num > 15)
		{
			num = 15;
		}
		Hashtable hashtable = new Hashtable();
		List<string> list = new List<string>();
		for (int i = 0; i < num; i++)
		{
			string fileName = Designer.RecentFiles[i].FileName;
			if (!fileName.StartsWith("cloud"))
			{
				string directoryName = Path.GetDirectoryName(fileName);
				if (hashtable[directoryName] == null)
				{
					list.Add(directoryName);
					hashtable[directoryName] = directoryName;
				}
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			ButtonX buttonX = null;
			switch (j)
			{
			case 0:
				buttonX = biRecentLocation01;
				break;
			case 1:
				buttonX = biRecentLocation02;
				break;
			case 2:
				buttonX = biRecentLocation03;
				break;
			case 3:
				buttonX = biRecentLocation04;
				break;
			case 4:
				buttonX = biRecentLocation05;
				break;
			case 5:
				buttonX = biRecentLocation06;
				break;
			case 6:
				buttonX = biRecentLocation07;
				break;
			case 7:
				buttonX = biRecentLocation08;
				break;
			case 8:
				buttonX = biRecentLocation09;
				break;
			}
			string text = list[j];
			((Control)buttonX).Visible = true;
			buttonX.Tooltip = text;
			((Control)buttonX).Tag = text;
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text);
			string measureText = StiStringHelper.GetMeasureText(text, ((Control)buttonX).Font, StiScale.XX(280.0));
			fileNameWithoutExtension = StiStringHelper.GetMeasureText(fileNameWithoutExtension, ((Control)buttonX).Font, StiScale.XX(280.0));
			((Control)buttonX).Text = GetFormattedString(fileNameWithoutExtension, measureText);
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiRibbonBackStageUserControl));
			buttonX.Image = (Image)componentResourceManager.GetObject("pageOpen.Image" + StiScale.StepName);
		}
		for (int k = list.Count; k < 9; k++)
		{
			ButtonX buttonX2 = null;
			if (k == 0)
			{
				buttonX2 = biRecentLocation01;
			}
			if (k == 1)
			{
				buttonX2 = biRecentLocation02;
			}
			if (k == 2)
			{
				buttonX2 = biRecentLocation03;
			}
			if (k == 3)
			{
				buttonX2 = biRecentLocation04;
			}
			if (k == 4)
			{
				buttonX2 = biRecentLocation05;
			}
			if (k == 5)
			{
				buttonX2 = biRecentLocation06;
			}
			if (k == 6)
			{
				buttonX2 = biRecentLocation07;
			}
			if (k == 7)
			{
				buttonX2 = biRecentLocation08;
			}
			if (k == 8)
			{
				buttonX2 = biRecentLocation09;
			}
			((Control)buttonX2).Visible = false;
		}
		ButtonX buttonX3 = biRecentLocation01;
		ButtonX buttonX4 = biRecentLocation02;
		ButtonX buttonX5 = biRecentLocation03;
		ButtonX buttonX6 = biRecentLocation04;
		ButtonX buttonX7 = biRecentLocation05;
		ButtonX buttonX8 = biRecentLocation06;
		ButtonX buttonX9 = biRecentLocation07;
		ButtonX buttonX10 = biRecentLocation08;
		bool flag = (((Control)biRecentLocation09).Enabled = StiActions.MainMenu.States.GetRecentFilesEnabled(Designer));
		bool flag3 = (((Control)buttonX10).Enabled = flag);
		bool flag5 = (((Control)buttonX9).Enabled = flag3);
		bool flag7 = (((Control)buttonX8).Enabled = flag5);
		bool flag9 = (((Control)buttonX7).Enabled = flag7);
		bool flag11 = (((Control)buttonX6).Enabled = flag9);
		bool flag13 = (((Control)buttonX5).Enabled = flag11);
		bool enabled = (((Control)buttonX4).Enabled = flag13);
		((Control)buttonX3).Enabled = enabled;
	}

	internal async Task RefreshGuiAsync()
	{
		Localize();
		if (StartScreenVisible)
		{
			BackStage.SuperTabStrip.FixedTabSize = new Size(StiScale.XXI(300.0), StiScale.YYI(20.0));
			((Control)superTabControlPanel2).Visible = false;
			((Control)superTabControlPanelOpen).Visible = false;
			((Control)superTabControlPanelNew).Visible = false;
			((Control)superTabControlPanelInfo).Visible = false;
			((Control)superTabControlPanelSaveAs).Visible = false;
			return;
		}
		BackStage.SuperTabStrip.FixedTabSize = new Size(StiScale.XXI(150.0), 0);
		((Control)superTabControlPanel2).Visible = true;
		((Control)superTabControlPanelOpen).Visible = true;
		((Control)superTabControlPanelNew).Visible = true;
		((Control)superTabControlPanelInfo).Visible = true;
		((Control)superTabControlPanelSaveAs).Visible = true;
		((Control)buttonProtectDocument).Enabled = !Designer.ReportFileName.EndsWith(".mrx");
		RefreshRibbonRecentFiles();
		RefreshRibbonRecentLocations();
		((Control)reportFlowItems).Controls.Clear();
		((Control)dashboardFlowItems).Controls.Clear();
		((Control)formFlowItems).Controls.Clear();
		pageCloud.Invisible = StiAccountSettings.CurrentAccount == null;
		pageCloudSave.Invisible = StiAccountSettings.CurrentAccount == null;
		buttonPublish.Visible = StiAccountSettings.CurrentAccount != null && StiOptions.Designer.IsDeveloperDesigner;
		buttonGetStarted.Visible = StiAccountSettings.CurrentAccount != null;
		buttonShare.Visible = StiAccountSettings.CurrentAccount != null;
		buttonSchedule.Visible = StiDesignerAppStatus.IsRunning && StiTaskSchedulerSettings.Current != null;
		buttonNew.Enabled = StiActions.MainMenu.States.GetNewEnabled();
		buttonInfo.Enabled = StiActions.MainMenu.States.GetInfoEnabled(Designer);
		buttonReportOpen.Enabled = StiActions.MainMenu.States.GetReportOpenEnabled(Designer);
		buttonPreview.Enabled = StiActions.MainMenu.States.GetReportPreviewEnabled(Designer);
		buttonReportSave.Enabled = StiActions.MainMenu.States.GetReportSaveEnabled(Designer);
		buttonReportSaveAs.Enabled = StiActions.MainMenu.States.GetReportSaveAsEnabled(Designer);
		buttonClose.Enabled = StiActions.MainMenu.States.GetCloseEnabled(Designer);
		buttonShare.Enabled = StiActions.Account.States.GetShareEnabled(Designer);
		buttonSchedule.Enabled = StiActions.Account.States.GetSchedulerEnabled(Designer);
		buttonPublish.Enabled = StiActions.Account.States.GetPublishEnabled(Designer);
		((Control)buttonCheckIssue).Enabled = StiActions.MainMenu.States.GetCheckIssuesEnabled(Designer);
		((Control)buttonReportOptions).Enabled = StiActions.MainMenu.States.GetReportSetupEnabled(Designer);
		buttonOptions.Enabled = StiActions.MainMenu.States.GetOptionsEnabled(Designer);
		if (buttonReportOpen.Enabled)
		{
			BackStage.SelectedTab = buttonReportOpen;
			tabControl.SelectedTab = pageRecent;
		}
		else
		{
			BackStage.SelectedTab = buttonNew;
		}
		await InitWizards();
	}

	private async Task InitWizards()
	{
		List<IStiWizardService> services = StiWizardSettings.CurrentWizards;
		ButtonX buttonX = new ButtonX();
		string text2 = (buttonX.Tooltip = StiLocalization.Get("Wizards", "BlankReport").Replace("&", "").Replace("...", ""));
		((Control)buttonX).Text = text2;
		buttonX.Is20Button = true;
		((Control)buttonX).Text = StiLocalization.Get("Wizards", "BlankReport").Replace("&", "").Replace("...", "");
		buttonX.ColorTable = eButtonColor.Orange;
		((Control)buttonX).BackColor = Color.Transparent;
		buttonX.Image = StiImageResizer.GetImage((Image)(object)Resources.BlankReport);
		((Control)buttonX).Width = buttonX.Image.Width + StiScale.I(30);
		((Control)buttonX).Height = buttonX.Image.Height + StiScale.I(40);
		buttonX.ImagePosition = eImagePosition.Top;
		((Control)buttonX).Click += delegate
		{
			CloseBackStage();
			StiActions.MainMenu.InvokeReportNewBlank(Designer);
		};
		newItems.Add(buttonX);
		((Control)reportFlowItems).Controls.Add((Control)(object)buttonX);
		Graphics globalMeasureGraphics = StiReport.GlobalMeasureGraphics;
		foreach (StiWizardService item in StiDesignerOptions.Wizards.Where((StiWizardService s) => s.ServiceEnabled))
		{
			string serviceName = item.ServiceName;
			ButtonX obj = new ButtonX
			{
				Is20Button = true
			};
			((Control)obj).Text = serviceName;
			obj.ColorTable = eButtonColor.Orange;
			((Control)obj).BackColor = Color.Transparent;
			obj.Image = StiImageResizer.GetImage((Image)(object)item.Image);
			ButtonX buttonX2 = obj;
			string helpUrl = item.HelpUrl;
			if (helpUrl != null)
			{
				Designer.SetToolTipOnlyHeaderEx((IComponent)buttonX2, globalMeasureGraphics, serviceName, helpUrl);
			}
			((Control)buttonX2).Width = buttonX2.Image.Width + StiScale.I(30);
			((Control)buttonX2).Height = buttonX2.Image.Height + StiScale.I(40);
			buttonX2.ImagePosition = eImagePosition.Top;
			((Control)buttonX2).Tag = item;
			((Control)buttonX2).Click += delegate(object sender2, EventArgs e2)
			{
				try
				{
					CloseBackStage();
					if (!StiOptions.Engine.GlobalEvents.InvokeCreatingReportInDesigner(Designer))
					{
						ButtonX buttonX15 = sender2 as ButtonX;
						StiWizardService stiWizardService4 = ((Control)buttonX15).Tag as StiWizardService;
						bool flag4 = false;
						StiReport stiReport = Designer.Report;
						if (stiReport == null)
						{
							stiReport = new StiReport();
							Designer.Report = stiReport;
							flag4 = true;
						}
						StiReport stiReport2 = stiWizardService4.CreateReport(stiReport);
						if (stiReport2 != null)
						{
							StiActions.MainMenu.InvokeRegNewReport(Designer, stiReport2);
						}
						else if (flag4)
						{
							Designer.Report.IsModified = false;
							StiActions.MainMenu.InvokeClose(Designer);
						}
					}
				}
				finally
				{
					StiOptions.Engine.GlobalEvents.InvokeCreatedReportInDesigner(Designer);
				}
			};
			newItems.Add(buttonX2);
			((Control)reportFlowItems).Controls.Add((Control)(object)buttonX2);
		}
		if (services != null)
		{
			foreach (IStiWizardService item2 in services.Where((IStiWizardService x) => x.ReportType == StiDesignerWizardReportType.Report))
			{
				string serviceName2 = item2.ServiceName;
				ButtonX obj2 = new ButtonX
				{
					Is20Button = true
				};
				((Control)obj2).Text = serviceName2;
				obj2.ColorTable = eButtonColor.Orange;
				((Control)obj2).BackColor = Color.Transparent;
				obj2.Image = StiImageResizer.GetImage(item2.GetWinFormsImage());
				obj2.ImagePosition = eImagePosition.Top;
				((Control)obj2).Tag = item2;
				((Control)obj2).Width = StiScale.I(174);
				((Control)obj2).Height = StiScale.I(243);
				obj2.ImageFixedSize = StiScale.I(new Size(144, 203));
				ButtonX buttonX3 = obj2;
				((Control)buttonX3).Click += delegate(object sender2, EventArgs e2)
				{
					try
					{
						CloseBackStage();
						if (!StiOptions.Engine.GlobalEvents.InvokeCreatingReportInDesigner(Designer))
						{
							ButtonX buttonX14 = (ButtonX)sender2;
							IStiWizardService stiWizardService3 = (IStiWizardService)((Control)buttonX14).Tag;
							bool flag3 = false;
							StiReport report5 = Designer.Report;
							if (report5 == null)
							{
								report5 = new StiReport();
								Designer.Report = report5;
								flag3 = true;
							}
							if (stiWizardService3.Run(this, ((Control)Designer).Handle, Designer.Report) is StiReport report6)
							{
								Designer.Report = null;
								StiActions.MainMenu.InvokeRegNewReport(Designer, report6);
							}
							else if (flag3)
							{
								Designer.Report.IsModified = false;
								StiActions.MainMenu.InvokeClose(Designer);
							}
						}
					}
					finally
					{
						StiOptions.Engine.GlobalEvents.InvokeCreatedReportInDesigner(Designer);
					}
				};
				newItems.Add(buttonX3);
				((Control)reportFlowItems).Controls.Add((Control)(object)buttonX3);
			}
		}
		StiReportTemplatesService service = StiReportTemplatesService.GetService();
		if (service == null)
		{
			return;
		}
		string[] reportsNames = service.GetReportsNames();
		if (reportsNames != null)
		{
			string[] array = reportsNames;
			foreach (string file in array)
			{
				ButtonX buttonX4 = new ButtonX
				{
					Is20Button = true
				};
				((Control)buttonX4).Text = file;
				buttonX4.ColorTable = eButtonColor.Orange;
				((Control)buttonX4).BackColor = Color.Transparent;
				buttonX4.Tooltip = StiLocalization.Get("Wizards", "FromReportTemplate").Replace("&", "").Replace("...", "");
				ButtonX buttonX5 = buttonX4;
				buttonX5.Image = await GetThumbnailAsync(file, StiScale.I(210));
				buttonX4.ImagePosition = eImagePosition.Top;
				((Control)buttonX4).Tag = file;
				ButtonX buttonX6 = buttonX4;
				((Control)buttonX6).Width = buttonX6.Image.Width + StiScale.I(30);
				((Control)buttonX6).Height = buttonX6.Image.Height + StiScale.I(40);
				((Control)buttonX6).Click += delegate(object sender3, EventArgs e3)
				{
					StiReportTemplatesService service2 = StiReportTemplatesService.GetService();
					ButtonX buttonX13 = sender3 as ButtonX;
					string reportName = ((Control)buttonX13).Tag as string;
					StiReport reportTemplate = service2.GetReportTemplate(reportName);
					if (reportTemplate != null)
					{
						StiActions.MainMenu.InvokeRegNewReport(Designer, reportTemplate);
					}
					CloseBackStage();
				};
				newItems.Add(buttonX6);
				((Control)reportFlowItems).Controls.Add((Control)(object)buttonX6);
			}
		}
		if (StiDashboardAssembly.IsAssemblyLoaded)
		{
			ButtonX buttonX7 = new ButtonX();
			text2 = (buttonX7.Tooltip = StiLocalization.Get("Wizards", "BlankDashboard").Replace("&", "").Replace("...", ""));
			((Control)buttonX7).Text = text2;
			buttonX7.Is20Button = true;
			((Control)buttonX7).Text = StiLocalization.Get("Wizards", "BlankDashboard").Replace("&", "").Replace("...", "");
			buttonX7.ColorTable = eButtonColor.Orange;
			((Control)buttonX7).BackColor = Color.Transparent;
			buttonX7.Image = StiImageResizer.GetImage((Image)(object)Resources.BlankDashboard);
			((Control)buttonX7).Width = buttonX7.Image.Width + StiScale.I(30);
			((Control)buttonX7).Height = buttonX7.Image.Height + StiScale.I(40);
			buttonX7.ImagePosition = eImagePosition.Top;
			((Control)buttonX7).Click += delegate
			{
				CloseBackStage();
				StiActions.MainMenu.InvokeDashboardNewBlank(Designer);
			};
			newItems.Add(buttonX7);
			((Control)dashboardFlowItems).Controls.Add((Control)(object)buttonX7);
		}
		if (services != null)
		{
			foreach (IStiWizardService item3 in services.Where((IStiWizardService x) => x.ReportType == StiDesignerWizardReportType.Dashboard))
			{
				string serviceName3 = item3.ServiceName;
				ButtonX obj3 = new ButtonX
				{
					Is20Button = true
				};
				((Control)obj3).Text = serviceName3;
				obj3.ColorTable = eButtonColor.Orange;
				((Control)obj3).BackColor = Color.Transparent;
				obj3.Image = StiImageResizer.GetImage(item3.GetWinFormsImage());
				obj3.ImagePosition = eImagePosition.Top;
				((Control)obj3).Tag = item3;
				((Control)obj3).Width = StiScale.I(290);
				((Control)obj3).Height = StiScale.I(186);
				obj3.ImageFixedSize = StiScale.I(new Size(259, 144));
				ButtonX buttonX8 = obj3;
				((Control)buttonX8).Click += delegate(object sender2, EventArgs e2)
				{
					try
					{
						CloseBackStage();
						if (!StiOptions.Engine.GlobalEvents.InvokeCreatingReportInDesigner(Designer))
						{
							ButtonX buttonX12 = (ButtonX)sender2;
							IStiWizardService stiWizardService2 = (IStiWizardService)((Control)buttonX12).Tag;
							bool flag2 = false;
							StiReport report3 = Designer.Report;
							if (report3 == null)
							{
								report3 = new StiReport();
								Designer.Report = report3;
								flag2 = true;
							}
							if (stiWizardService2.Run(this, ((Control)Designer).Handle, Designer.Report) is StiReport report4)
							{
								Designer.Report = null;
								StiActions.MainMenu.InvokeRegNewReport(Designer, report4);
							}
							else if (flag2)
							{
								Designer.Report.IsModified = false;
								StiActions.MainMenu.InvokeClose(Designer);
							}
						}
					}
					finally
					{
						StiOptions.Engine.GlobalEvents.InvokeCreatedReportInDesigner(Designer);
					}
				};
				newItems.Add(buttonX8);
				((Control)dashboardFlowItems).Controls.Add((Control)(object)buttonX8);
			}
		}
		if (StiAppAssembly.IsAssemblyLoaded)
		{
			ButtonX buttonX9 = new ButtonX();
			text2 = (buttonX9.Tooltip = StiLocalization.Get("Wizards", "BlankScreen").Replace("&", "").Replace("...", ""));
			((Control)buttonX9).Text = text2;
			buttonX9.Is20Button = true;
			((Control)buttonX9).Text = StiLocalization.Get("Wizards", "BlankScreen").Replace("&", "").Replace("...", "");
			buttonX9.ColorTable = eButtonColor.Orange;
			((Control)buttonX9).BackColor = Color.Transparent;
			buttonX9.Image = StiImageResizer.GetImage((Image)(object)Resources.BlankDashboard);
			((Control)buttonX9).Width = buttonX9.Image.Width + StiScale.I(30);
			((Control)buttonX9).Height = buttonX9.Image.Height + StiScale.I(40);
			buttonX9.ImagePosition = eImagePosition.Top;
			((Control)buttonX9).Click += delegate
			{
				CloseBackStage();
				StiActions.MainMenu.InvokeScreenNewBlank(Designer);
			};
			newItems.Add(buttonX9);
			((Control)formFlowItems).Controls.Add((Control)(object)buttonX9);
		}
		if (services == null)
		{
			return;
		}
		foreach (IStiWizardService item4 in services.Where((IStiWizardService x) => x.ReportType == StiDesignerWizardReportType.Screen))
		{
			string serviceName4 = item4.ServiceName;
			ButtonX obj4 = new ButtonX
			{
				Is20Button = true
			};
			((Control)obj4).Text = serviceName4;
			obj4.ColorTable = eButtonColor.Orange;
			((Control)obj4).BackColor = Color.Transparent;
			obj4.Image = StiImageResizer.GetImage(item4.GetWinFormsImage());
			obj4.ImagePosition = eImagePosition.Top;
			((Control)obj4).Tag = item4;
			((Control)obj4).Width = StiScale.I(290);
			((Control)obj4).Height = StiScale.I(186);
			obj4.ImageFixedSize = StiScale.I(new Size(259, 144));
			ButtonX buttonX10 = obj4;
			((Control)buttonX10).Click += delegate(object sender2, EventArgs e2)
			{
				try
				{
					CloseBackStage();
					if (!StiOptions.Engine.GlobalEvents.InvokeCreatingReportInDesigner(Designer))
					{
						ButtonX buttonX11 = (ButtonX)sender2;
						IStiWizardService stiWizardService = (IStiWizardService)((Control)buttonX11).Tag;
						bool flag = false;
						StiReport report = Designer.Report;
						if (report == null)
						{
							report = new StiReport();
							Designer.Report = report;
							flag = true;
						}
						if (stiWizardService.Run(this, ((Control)Designer).Handle, Designer.Report) is StiReport report2)
						{
							Designer.Report = null;
							StiActions.MainMenu.InvokeRegNewReport(Designer, report2);
						}
						else if (flag)
						{
							Designer.Report.IsModified = false;
							StiActions.MainMenu.InvokeClose(Designer);
						}
					}
				}
				finally
				{
					StiOptions.Engine.GlobalEvents.InvokeCreatedReportInDesigner(Designer);
				}
			};
			newItems.Add(buttonX10);
			((Control)formFlowItems).Controls.Add((Control)(object)buttonX10);
		}
	}

	internal void DisableStartScreen()
	{
		StartScreenVisible = false;
		IStiWizardStartScreenControl stiWizardStartScreenControl = wizardStartScreenControl;
		Control val = (Control)((stiWizardStartScreenControl is Control) ? stiWizardStartScreenControl : null);
		if (val != null)
		{
			((Component)(object)val).Dispose();
			wizardStartScreenControl = null;
		}
		if (startScreenElementHost != null)
		{
			startScreenElementHost.Child = null;
			((Control)BackStage).Controls.Remove((Control)(object)startScreenElementHost);
			((Component)(object)startScreenElementHost).Dispose();
			((Control)startScreenElementHost).Parent = null;
			startScreenElementHost = null;
		}
		CloseBackStage();
	}

	internal void EnableStartScreen()
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		IStiWizardStartScreenControl stiWizardStartScreenControl = StiWizardsCreater.GetWizardStartScreenControl(isWinFormsMode: true, Designer);
		if (stiWizardStartScreenControl != null)
		{
			StartScreenVisible = true;
			wizardStartScreenControl = stiWizardStartScreenControl;
			wizardStartScreenControl.IntPtrParentControl = ((Control)this).Handle;
			wizardStartScreenControl.IntPtrParentForm = ((Control)Designer).Handle;
			startScreenElementHost = new ElementHost();
			_003F val = startScreenElementHost;
			IStiWizardStartScreenControl stiWizardStartScreenControl2 = wizardStartScreenControl;
			((ElementHost)val).Child = (UIElement)((stiWizardStartScreenControl2 is UIElement) ? stiWizardStartScreenControl2 : null);
			((Control)startScreenElementHost).Dock = (DockStyle)5;
			((Control)startScreenElementHost).Anchor = (AnchorStyles)15;
			((Control)startScreenElementHost).Location = new Point(StiScale.XXI(1.0), StiScale.YYI(1.0));
			((Control)startScreenElementHost).Width = ((Control)BackStage).Width - StiScale.XXI(6.0);
			((Control)startScreenElementHost).Height = ((Control)BackStage).Height - StiScale.YYI(2.0);
			((Control)BackStage).Controls.Add((Control)(object)startScreenElementHost);
			((Control)startScreenElementHost).BringToFront();
			wizardStartScreenControl.ReportChanged += WizardStartScreenControl_ReportChanged;
			wizardStartScreenControl.ReportPathChanged += WizardStartScreenControl_ReportPathChanged;
			wizardStartScreenControl.RecentFileChanged += WizardStartScreenControl_RecentFileChanged;
			wizardStartScreenControl.RefreshRecentFiles += WizardStartScreenControl_RefeshRecentFiles;
			wizardStartScreenControl.CloudReportOpened += WizardStartScreenControl_CloudReportOpened;
		}
	}

	private void LoadFromPath(string reportPath)
	{
		Designer.IsDesignerParamLoaded = false;
		if (reportPath.EndsWith(".mrt", StringComparison.InvariantCulture) || reportPath.EndsWith(".mrz", StringComparison.InvariantCulture) || reportPath.EndsWith(".mrx", StringComparison.InvariantCulture))
		{
			if (!File.Exists(reportPath))
			{
				return;
			}
			bool flag = false;
			try
			{
				flag = StiReportOpenDialog.OpenReport(Designer, Designer.Report, reportPath);
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
			finally
			{
				if (Designer.Report != null)
				{
					Designer.Report.Dictionary.Connect(loadData: false);
					Designer.Builder.BuildReport();
					Designer.InvokeBuidingToolbox();
					Designer.Builder.InvalidateRulers();
				}
			}
			if (flag)
			{
				StiOptions.Engine.GlobalEvents.InvokeLoadedReportInDesigner(Designer);
			}
			Windows7Taskbar.HideTaskbarProgress();
		}
		DisableStartScreen();
		Designer?.LoadDesignerParam();
		Designer?.AddRecentFile(reportPath, null);
	}

	private void CloseBackStage()
	{
		Designer.CloseBackStage();
		if (newReportWizardElementHost != null)
		{
			newReportWizardControl = null;
			((Control)panelItems).Controls.Remove((Control)(object)newReportWizardElementHost);
			DisableElementHost(newReportWizardElementHost);
		}
		if (cloudOpenControlElementHost != null)
		{
			cloudOpenControl = null;
			((Control)panelCloud).Controls.Remove((Control)(object)cloudOpenControlElementHost);
			DisableElementHost(cloudOpenControlElementHost);
		}
		if (accountControlElementHost != null)
		{
			accountControl = null;
			((Control)panelAccount).Controls.Remove((Control)(object)accountControlElementHost);
			DisableElementHost(accountControlElementHost);
		}
		if (schedulerControlElementHost != null)
		{
			accountControl = null;
			((Control)panelAccount).Controls.Remove((Control)(object)schedulerControlElementHost);
			DisableElementHost(schedulerControlElementHost);
			schedulerItemsControl = null;
		}
		if (startScreenElementHost != null)
		{
			startScreenElementHost.Child = null;
			((Control)BackStage).Controls.Remove((Control)(object)startScreenElementHost);
			((Component)(object)startScreenElementHost).Dispose();
			((Control)startScreenElementHost).Parent = null;
			startScreenElementHost = null;
		}
	}

	private void DisableElementHost(ElementHost elementHost)
	{
		((Component)(object)elementHost).Dispose();
		((Control)elementHost).Parent = null;
		elementHost = null;
	}

	private void CreateAccountControl()
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		if (accountControl == null)
		{
			accountControl = StiAccountSettings.CurrentAccount.AccountCreater.GetAccountControl(((Control)Designer).Handle);
			if (accountControl != null)
			{
				accountControl.Closed += ButtonBack_Click;
				((Control)panelAccount).Controls.Clear();
				ElementHost val = new ElementHost();
				((Control)val).Dock = (DockStyle)5;
				_003F val2 = val;
				IStiAccountControl stiAccountControl = accountControl;
				((ElementHost)val2).Child = (UIElement)((stiAccountControl is UIElement) ? stiAccountControl : null);
				accountControlElementHost = val;
				((Control)panelAccount).Controls.Add((Control)(object)accountControlElementHost);
			}
		}
	}

	internal void SelectedMyTeam()
	{
		if (StiAccountSettings.CurrentAccount != null)
		{
			CreateAccountControl();
			if (accountControl != null)
			{
				accountControl.SelectedMyTeam();
			}
		}
	}

	public void Localize()
	{
		buttonInfo.Text = StiLocalization.Get("ReportInfo", "Info");
		((Control)pageRecent).Text = StiLocalization.Get("FormDatabaseEdit", "RecentConnections");
		((Control)pageOpen).Text = StiLocalization.Get("ReportOpen", "Browse");
		((Control)pageImport).Text = StiLocalization.Get("ReportOpen", "Import");
		((Control)pageCloud).Text = "Stimulsoft Cloud";
		((Control)pageCloudSave).Text = "Stimulsoft Cloud";
		((Control)pageSaveAs).Text = StiLocalization.Get("ReportOpen", "Browse");
		((Control)pageHelpDocumentation).Text = StiLocalization.Get("MainMenu", "menuHelpDocumentation");
		((Control)pageHelpSupport).Text = StiLocalization.Get("MainMenu", "menuHelpSupport");
		((Control)pageHelpVideos).Text = StiLocalization.Get("MainMenu", "menuHelpVideos");
		((Control)pageHelpSamples).Text = StiLocalization.Get("MainMenu", "menuHelpSamples");
		((Control)pageHelpForum).Text = StiLocalization.Get("MainMenu", "menuHelpForum");
		((Control)pageHelpDemos).Text = StiLocalization.Get("MainMenu", "menuHelpDemos");
		((Control)pageHelpTrainingCourses).Text = StiLocalization.Get("MainMenu", "menuHelpTrainingCourses");
		buttonNew.Text = StiLocalization.Get("MainMenu", "menuFileNew").Replace("&", string.Empty).Replace("...", string.Empty);
		buttonReportOpen.Text = StiLocalization.Get("MainMenu", "menuFileOpen").Replace("&", string.Empty).Replace("...", string.Empty);
		buttonReportSave.Text = StiLocalization.Get("MainMenu", "menuFileSave").Replace("&", string.Empty).Replace("...", string.Empty);
		buttonReportSaveAs.Text = StiLocalization.Get("MainMenu", "menuFileSaveAs").Replace("&", string.Empty).Replace("...", string.Empty);
		buttonPreview.Text = StiLocalization.Get("MainMenu", "menuFileReportPreview").Replace("&", string.Empty).Replace("...", string.Empty);
		buttonShare.Text = StiLocalization.Get("Cloud", "ButtonShare");
		buttonPublish.Text = StiLocalization.Get("Buttons", "Publish");
		buttonSchedule.Text = StiLocalization.Get("Scheduler", "Schedule");
		((Control)labelTitleSchedule).Text = StiLocalization.Get("Scheduler", "Schedule");
		buttonGetStarted.Text = StiLocalization.Get("WelcomeScreen", "GetStarted");
		buttonClose.Text = StiLocalization.Get("MainMenu", "menuFileClose").Replace("&", string.Empty).Replace("...", string.Empty);
		buttonAccount.Text = StiLocalization.Get("Cloud", "Account");
		buttonOptions.Text = StiLocalization.Get("MainMenu", "menuToolsOptions").Replace("&", string.Empty).Replace("...", string.Empty);
		buttonAbout.Text = StiLocalization.Get("MainMenu", "menuHelpAboutProgramm").Replace("&", string.Empty).Replace("...", string.Empty);
		buttonHelp.Text = StiLocalization.Get("Buttons", "Help");
		((Control)buttonReportOptions).Text = StiLocalization.Get("ReportInfo", "ReportOptions");
		((Control)buttonProtectDocument).Text = StiLocalization.Get("ReportInfo", "EncryptWithPassword");
		((Control)buttonCheckIssue).Text = StiLocalization.Get("MainMenu", "menuCheckIssues");
		((Control)labelTitleAccount).Text = StiLocalization.Get("Cloud", "Account");
		((Control)labelTitleInfo).Text = StiLocalization.Get("ReportInfo", "Info");
		((Control)labelTitleNew).Text = StiLocalization.Get("MainMenu", "menuFileNew").Replace("&", string.Empty).Replace("...", string.Empty);
		((Control)labelTitleOpen).Text = StiLocalization.Get("MainMenu", "menuFileOpen").Replace("&", string.Empty).Replace("...", string.Empty);
		((Control)labelTitleSaveAs).Text = StiLocalization.Get("MainMenu", "menuFileSaveAs").Replace("&", string.Empty).Replace("...", string.Empty);
		((Control)labelReportOptionsTitle).Text = StiLocalization.Get("ReportInfo", "ReportOptions");
		((Control)labelReportOptions).Text = StiLocalization.Get("ReportInfo", "ReportOptionsAdditionalDescription");
		((Control)labelProtectDocumentTitle).Text = StiLocalization.Get("ReportInfo", "EncryptWithPasswordDescription");
		((Control)labelProtectDocument).Text = StiLocalization.Get("ReportInfo", "EncryptWithPasswordAdditionalDescription");
		((Control)labelCheckIssuesTitle).Text = StiLocalization.Get("MainMenu", "menuCheckIssues");
		((Control)labelCheckIssues).Text = StiLocalization.Get("ReportInfo", "CheckIssuesAdditionalDescription");
		LocalizeTabs();
		if (accountControl != null)
		{
			accountControl.Localize();
		}
	}

	private void LocalizeTabs()
	{
		((Control)labelRecentDocuments).Text = StiLocalization.Get("MainMenu", "menuFileRecentDocuments");
		((Control)labelRecentLocations).Text = StiLocalization.Get("MainMenu", "menuFileRecentLocations");
	}

	private void RecentFileOpen_Click(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		StiRecentFile stiRecentFile = (StiRecentFile)((ToolStripItem)(ToolStripMenuItem)sender).Tag;
		string fileName = stiRecentFile.FileName;
		if (File.Exists(fileName))
		{
			string arguments = "/select, \"" + fileName + "\"";
			Process.Start("explorer.exe", arguments);
		}
	}

	private void RecentFileRemove_Click(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		StiRecentFile item = (StiRecentFile)((ToolStripItem)(ToolStripMenuItem)sender).Tag;
		Designer.RecentFiles.Remove(item);
		StiSettings.Load();
		for (int i = 0; i < 15; i++)
		{
			StiSettings.Set("StiDesigner", $"RecentFile{i}", (i < Designer.RecentFiles.Count) ? Designer.RecentFiles[i].FileName : string.Empty);
		}
		StiSettings.Save();
		RefreshRibbonRecentFiles();
	}

	private void RecentFileRemoveAll_Click(object sender, EventArgs e)
	{
		StiSettings.Load();
		for (int i = 0; i < 15; i++)
		{
			StiSettings.Set("StiDesigner", $"RecentFile{i}", string.Empty);
		}
		StiSettings.Save();
		RefreshRibbonRecentFiles();
	}

	private void RecentFileOpenContainingFolder_Click(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		StiRecentFile stiRecentFile = (StiRecentFile)((ToolStripItem)(ToolStripMenuItem)sender).Tag;
	}

	private void NewReportWizard_ReportCreated(object sender, EventArgs e)
	{
		StiCreatedReportEventArgs stiCreatedReportEventArgs = e as StiCreatedReportEventArgs;
		StiReport stiReport = sender as StiReport;
		if (stiReport != null)
		{
			if (stiCreatedReportEventArgs != null && stiCreatedReportEventArgs.IsBlankReport && !stiCreatedReportEventArgs.IsCreatedReportFromDataWizard)
			{
				StiDictionaryInitializer.Initialize(Designer.Report, stiReport);
				Designer.LoadDesignerParam();
			}
			Designer.Report = stiReport;
		}
		if (stiCreatedReportEventArgs != null && stiCreatedReportEventArgs.IsCreatedReportFromDataWizard)
		{
			StiDataSource database = stiReport.Dictionary.DataSources.ToList().FirstOrDefault();
			Designer?.PageView?.GetCurrentPageView().View?.CreateDatabandComponentFromDatabase(database);
		}
	}

	private void StoreWizard_ReportPathChanged(object sender, EventArgs e)
	{
		string text = sender.ToString();
		if (!string.IsNullOrEmpty(text))
		{
			LoadFromPath(text);
		}
	}

	private void WizardStartScreenControl_CloudReportOpened(object sender, EventArgs e)
	{
		string text = sender as string;
		if (!string.IsNullOrEmpty(text))
		{
			string[] array = text.Split(new char[1] { '\\' });
			if (array.Length > 2)
			{
				string tag = array[1];
				Designer.AddRecentFile(text, tag);
			}
		}
	}

	private void WizardStartScreenControl_ReportChanged(object sender, EventArgs e)
	{
		StiReport stiReport = sender as StiReport;
		if (stiReport != null)
		{
			((Control)Designer).Visible = false;
			Designer.Report = stiReport;
			((Control)Designer).Visible = true;
		}
		DisableStartScreen();
		if (e is StiCreatedReportEventArgs { IsCreatedReportFromDataWizard: not false })
		{
			StiDataSource database = stiReport.Dictionary.DataSources.ToList().FirstOrDefault();
			Designer?.PageView?.GetCurrentPageView().View?.CreateDatabandComponentFromDatabase(database);
		}
		Designer?.LoadDesignerParam();
	}

	private async void WizardStartScreenControl_RecentFileChanged(object sender, EventArgs e)
	{
		StiRecentFile recentFile = sender as StiRecentFile;
		DisableStartScreen();
		Designer.IsDesignerParamLoaded = false;
		await StiActions.MainMenu.InvokeRecentFileAsync(Designer, recentFile);
		Designer.LoadDesignerParam();
	}

	private void WizardStartScreenControl_RefeshRecentFiles(object sender, EventArgs e)
	{
		RefreshRibbonRecentFiles();
	}

	private void WizardStartScreenControl_ReportPathChanged(object sender, EventArgs e)
	{
		string text = sender.ToString();
		if (!string.IsNullOrEmpty(text))
		{
			LoadFromPath(text);
		}
	}

	private void ButtonReportSaveAs_MouseDown(object sender, MouseEventArgs e)
	{
		if (buttonReportSaveAs.Enabled && pageCloudSave.Invisible)
		{
			CloseBackStage();
			StiActions.MainMenu.InvokeReportSaveAs(Designer);
		}
	}

	private void BackStage_VisibleChanged(object sender, EventArgs e)
	{
		StiInplaceDesigner.Close();
		StiElementActionHelper.Hide();
		((Control)BackStage.SuperTabStrip).Tag = Designer.StyleManager.ManagerStyle;
	}

	private void ButtonBack_Click(object sender, EventArgs e)
	{
		CloseBackStage();
	}

	private void ButtonNew_Click(object sender, MouseEventArgs e)
	{
		if (newReportWizardControl == null)
		{
			InitNewReportWizard();
		}
		if (newReportWizardControl != null)
		{
			newReportWizardControl.InitShow();
		}
	}

	public void ButtonAccount_Click(object sender, MouseEventArgs e)
	{
		if (StiAccountSettings.CurrentAccount != null)
		{
			CreateAccountControl();
		}
	}

	private void ButtonReportSave_Click(object sender, MouseEventArgs e)
	{
		if (buttonReportSave.Enabled)
		{
			CloseBackStage();
			if (StiAccountSettings.CurrentAccount == null || (StiAccountSettings.CurrentAccount.AccountChecker.CheckUserActivated(((Control)Designer).Handle) && StiAccountSettings.CurrentAccount.AccountChecker.CheckTrialExpired() && StiAccountSettings.CurrentAccount.AccountChecker.CheckProductExpired()))
			{
				StiActions.MainMenu.InvokeReportSave(Designer);
			}
		}
	}

	private void ButtonShare_Click(object sender, MouseEventArgs e)
	{
		if (buttonShare.Enabled)
		{
			CloseBackStage();
			if (StiAccountSettings.CurrentAccount == null || (StiAccountSettings.CurrentAccount.AccountChecker.CheckUserActivated(((Control)Designer).Handle) && StiAccountSettings.CurrentAccount.AccountChecker.CheckTrialExpired() && StiAccountSettings.CurrentAccount.AccountChecker.CheckProductExpired()))
			{
				StiActions.MainMenu.InvokeShare(Designer);
			}
		}
	}

	private void ButtonPublish_Click(object sender, MouseEventArgs e)
	{
		if (buttonPublish.Enabled)
		{
			CloseBackStage();
			StiActions.MainMenu.InvokePublish(Designer);
		}
	}

	private void ButtonGetStarted_Click(object sender, MouseEventArgs e)
	{
		CloseBackStage();
		StiActions.MainMenu.InvokeGetStarted(Designer);
	}

	private void ButtonClose_Click(object sender, MouseEventArgs e)
	{
		if (buttonClose.Enabled)
		{
			CloseBackStage();
			StiActions.MainMenu.InvokeClose(Designer);
		}
	}

	private void ButtonPreview_Click(object sender, EventArgs e)
	{
		if (buttonPreview.Enabled)
		{
			CloseBackStage();
			StiActions.MainMenu.InvokeReportPreview(Designer);
		}
	}

	private void ButtonOptions_Click(object sender, MouseEventArgs e)
	{
		if (buttonOptions.Enabled)
		{
			CloseBackStage();
			StiActions.MainMenu.InvokeOptions(Designer);
		}
	}

	private async void ButtonRecentFile_Click(object sender, EventArgs e)
	{
		CloseBackStage();
		Application.DoEvents();
		ButtonX buttonX = sender as ButtonX;
		StiRecentFile recentFile = ((Control)buttonX).Tag as StiRecentFile;
		await StiActions.MainMenu.InvokeRecentFileAsync(Designer, recentFile);
	}

	private void ButtonRecentLocation_Click(object sender, EventArgs e)
	{
		CloseBackStage();
		ButtonX buttonX = sender as ButtonX;
		string reportSaveLoadPath = ((Control)buttonX).Tag as string;
		string reportSaveLoadPath2 = StiOptions.Designer.ReportSaveLoadPath;
		try
		{
			StiOptions.Designer.ReportSaveLoadPath = reportSaveLoadPath;
			StiActions.MainMenu.InvokeReportOpen(Designer);
		}
		finally
		{
			StiOptions.Designer.ReportSaveLoadPath = reportSaveLoadPath2;
		}
	}

	private void ButtonAbout_Click(object sender, MouseEventArgs e)
	{
		CloseBackStage();
		StiActions.MainMenu.InvokeAbout(Designer);
	}

	private void ButtonReportOptions_Click(object sender, EventArgs e)
	{
		CloseBackStage();
		StiActions.MainMenu.InvokeReportSetup(Designer);
	}

	private void ButtonProtectDocument_Click(object sender, EventArgs e)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Invalid comparison between Unknown and I4
		CloseBackStage();
		StiSavePasswordForm stiSavePasswordForm = new StiSavePasswordForm();
		try
		{
			if ((int)((Form)stiSavePasswordForm).ShowDialog() == 1)
			{
				((Control)buttonProtectDocument).Enabled = false;
				Designer.ReportFileName = Path.ChangeExtension(Designer.ReportFileName, ".mrx");
				Designer.ReportKey = stiSavePasswordForm.Password;
				RefreshRibbonRecentFiles();
			}
		}
		finally
		{
			((IDisposable)stiSavePasswordForm)?.Dispose();
		}
	}

	private void ButtonCheckIssues_Click(object sender, EventArgs e)
	{
		CloseBackStage();
		StiInplaceDesigner.Close();
		StiCheckStatusDesigner stiCheckStatusDesigner = new StiCheckStatusDesigner(Designer);
		stiCheckStatusDesigner.DesignComponent(Designer.Report);
	}

	private void ButtonActiveReports_Click(object sender, EventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Invalid comparison between Unknown and I4
		CloseBackStage();
		if (StiOptions.Designer.MainMenu.ImportActiveReports == null)
		{
			return;
		}
		OpenFileDialog val = new OpenFileDialog();
		try
		{
			((FileDialog)val).Title = StiHint.CreateHint(StiLocalization.Get("FormViewer", "Open"));
			((FileDialog)val).Filter = "ActiveReports file (*.rpx)|*.rpx";
			if ((int)((CommonDialog)val).ShowDialog() == 1)
			{
				try
				{
					byte[] bytes = File.ReadAllBytes(((FileDialog)val).FileName);
					StiImportResult importResult = StiOptions.Designer.MainMenu.ImportActiveReports(bytes);
					ShowReport(importResult);
					return;
				}
				catch (Exception exception)
				{
					StiExceptionProvider.Show(exception);
					return;
				}
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void ButtonComponentOne_Click(object sender, EventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Invalid comparison between Unknown and I4
		CloseBackStage();
		if (StiOptions.Designer.MainMenu.ImportComponentOneReports == null)
		{
			return;
		}
		OpenFileDialog val = new OpenFileDialog();
		try
		{
			((FileDialog)val).Title = StiHint.CreateHint(StiLocalization.Get("FormViewer", "Open"));
			((FileDialog)val).Filter = "ComponentOne Reports file (*.xml)|*.xml";
			if ((int)((CommonDialog)val).ShowDialog() == 1)
			{
				try
				{
					byte[] bytes = File.ReadAllBytes(((FileDialog)val).FileName);
					StiImportResult importResult = StiOptions.Designer.MainMenu.ImportComponentOneReports(bytes);
					ShowReport(importResult);
					return;
				}
				catch (Exception exception)
				{
					StiExceptionProvider.Show(exception);
					return;
				}
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void ButtonFastReport_Click(object sender, EventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Invalid comparison between Unknown and I4
		CloseBackStage();
		if (StiOptions.Designer.MainMenu.ImportFastReports == null)
		{
			return;
		}
		OpenFileDialog val = new OpenFileDialog();
		try
		{
			((FileDialog)val).Title = StiHint.CreateHint(StiLocalization.Get("FormViewer", "Open"));
			((FileDialog)val).Filter = "FastReport.Net file (*.frx)|*.frx";
			if ((int)((CommonDialog)val).ShowDialog() == 1)
			{
				try
				{
					byte[] bytes = File.ReadAllBytes(((FileDialog)val).FileName);
					StiImportResult importResult = StiOptions.Designer.MainMenu.ImportFastReports(bytes);
					ShowReport(importResult);
					return;
				}
				catch (Exception exception)
				{
					StiExceptionProvider.Show(exception);
					return;
				}
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void ButtonMicrosoftReportingServices_Click(object sender, EventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Invalid comparison between Unknown and I4
		CloseBackStage();
		if (StiOptions.Designer.MainMenu.ImportMicrosoftReportingServices == null)
		{
			return;
		}
		OpenFileDialog val = new OpenFileDialog();
		try
		{
			((FileDialog)val).Title = StiHint.CreateHint(StiLocalization.Get("FormViewer", "Open"));
			((FileDialog)val).Filter = "Microsoft Reporting Services file (*.rdl;*.rdlc)|*.rdl;*.rdlc";
			if ((int)((CommonDialog)val).ShowDialog() == 1)
			{
				try
				{
					byte[] bytes = File.ReadAllBytes(((FileDialog)val).FileName);
					StiImportResult importResult = StiOptions.Designer.MainMenu.ImportMicrosoftReportingServices(bytes);
					ShowReport(importResult);
					return;
				}
				catch (Exception exception)
				{
					StiExceptionProvider.Show(exception);
					return;
				}
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void ButtonReportSharpShooter_Click(object sender, EventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Invalid comparison between Unknown and I4
		CloseBackStage();
		if (StiOptions.Designer.MainMenu.ImportReportSharpShooter == null)
		{
			return;
		}
		OpenFileDialog val = new OpenFileDialog();
		try
		{
			((FileDialog)val).Title = StiHint.CreateHint(StiLocalization.Get("FormViewer", "Open"));
			((FileDialog)val).Filter = "Report Sharp Shooter file (*.rst)|*.rst";
			if ((int)((CommonDialog)val).ShowDialog() == 1)
			{
				try
				{
					byte[] bytes = File.ReadAllBytes(((FileDialog)val).FileName);
					StiImportResult importResult = StiOptions.Designer.MainMenu.ImportReportSharpShooter(bytes);
					ShowReport(importResult);
					return;
				}
				catch (Exception exception)
				{
					StiExceptionProvider.Show(exception);
					return;
				}
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void ButtonRTF_Click(object sender, EventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Invalid comparison between Unknown and I4
		CloseBackStage();
		if (StiOptions.Designer.MainMenu.ImportRtf == null)
		{
			return;
		}
		OpenFileDialog val = new OpenFileDialog();
		try
		{
			((FileDialog)val).Title = StiHint.CreateHint(StiLocalization.Get("FormViewer", "Open"));
			((FileDialog)val).Filter = "Rich Text Format (*.rtf)|*.rtf";
			if ((int)((CommonDialog)val).ShowDialog() == 1)
			{
				try
				{
					byte[] bytes = File.ReadAllBytes(((FileDialog)val).FileName);
					StiImportResult importResult = StiOptions.Designer.MainMenu.ImportRtf(bytes);
					ShowReport(importResult);
					return;
				}
				catch (Exception exception)
				{
					StiExceptionProvider.Show(exception);
					return;
				}
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void ButtonTelerikReporting_Click(object sender, EventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Invalid comparison between Unknown and I4
		CloseBackStage();
		if (StiOptions.Designer.MainMenu.ImportTelerikReports == null)
		{
			return;
		}
		OpenFileDialog val = new OpenFileDialog();
		try
		{
			((FileDialog)val).Title = StiHint.CreateHint(StiLocalization.Get("FormViewer", "Open"));
			((FileDialog)val).Filter = "Telerik Reports file (*.trdx)|*.trdx";
			if ((int)((CommonDialog)val).ShowDialog() == 1)
			{
				try
				{
					byte[] bytes = File.ReadAllBytes(((FileDialog)val).FileName);
					StiImportResult importResult = StiOptions.Designer.MainMenu.ImportTelerikReports(bytes);
					ShowReport(importResult);
					return;
				}
				catch (Exception exception)
				{
					StiExceptionProvider.Show(exception);
					return;
				}
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void ButtonVisualFoxPro_Click(object sender, EventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Invalid comparison between Unknown and I4
		CloseBackStage();
		if (StiOptions.Designer.MainMenu.ImportVisualFoxPro == null)
		{
			return;
		}
		OpenFileDialog val = new OpenFileDialog();
		try
		{
			((FileDialog)val).Title = StiHint.CreateHint(StiLocalization.Get("FormViewer", "Open"));
			((FileDialog)val).Filter = "Visual FoxPro file (*.frx)|*.frx";
			if ((int)((CommonDialog)val).ShowDialog() == 1)
			{
				try
				{
					byte[] dataBytes = File.ReadAllBytes(((FileDialog)val).FileName);
					string path = Path.ChangeExtension(((FileDialog)val).FileName, "frt");
					byte[] memoBytes = File.ReadAllBytes(path);
					StiImportResult importResult = StiOptions.Designer.MainMenu.ImportVisualFoxPro(dataBytes, memoBytes);
					ShowReport(importResult);
					return;
				}
				catch (Exception exception)
				{
					StiExceptionProvider.Show(exception);
					return;
				}
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void ButtonXtraReports_Click(object sender, EventArgs e)
	{
		CloseBackStage();
		StiProcess.Start("https://github.com/stimulsoft/Importing.Tools/tree/master/Import.XtraReports");
	}

	private void ButtonCrystalReports_Click(object sender, EventArgs e)
	{
		CloseBackStage();
		StiProcess.Start("https://github.com/stimulsoft/Importing.Tools/tree/master/Import.CrystalReports");
	}

	private void PageSaveAs_TabClick(object sender, EventArgs e)
	{
		CloseBackStage();
		if (StiAccountSettings.CurrentAccount == null || (StiAccountSettings.CurrentAccount.AccountChecker.CheckUserActivated(((Control)Designer).Handle) && StiAccountSettings.CurrentAccount.AccountChecker.CheckTrialExpired() && StiAccountSettings.CurrentAccount.AccountChecker.CheckProductExpired()))
		{
			StiActions.MainMenu.InvokeReportSaveAs(Designer);
		}
	}

	private void PageHelpDocumentation_TabClick(object sender, EventArgs e)
	{
		CloseBackStage();
		StiHelpViewerForm.Show("user-manual/index.html?introduction.htm");
	}

	private void PageHelpSupport_TabClick(object sender, EventArgs e)
	{
		CloseBackStage();
		StiProcess.Start("mailto:support@stimulsoft.com");
	}

	private void PageHelpVideos_TabClick(object sender, EventArgs e)
	{
		CloseBackStage();
		StiProcess.Start("http://www.youtube.com/user/StimulsoftVideos");
	}

	private void PageHelpSamples_TabClick(object sender, EventArgs e)
	{
		CloseBackStage();
		StiProcess.Start("https://www.stimulsoft.com/en/samples");
	}

	private void PageHelpForum_TabClick(object sender, EventArgs e)
	{
		CloseBackStage();
		switch (CultureInfo.CurrentUICulture.TwoLetterISOLanguageName)
		{
		case "ru":
		case "be":
		case "uk":
		case "kz":
			StiProcess.Start("https://forumru.stimulsoft.com/");
			break;
		default:
			StiProcess.Start("https://forum.stimulsoft.com/");
			break;
		}
	}

	private void PageHelpDemos_TabClick(object sender, EventArgs e)
	{
		CloseBackStage();
		StiProcess.Start("http://demo.stimulsoft.com/");
	}

	private void PageHelpTrainingCourses_TabClick(object sender, EventArgs e)
	{
		CloseBackStage();
		StiProcess.Start("https://www.stimulsoft.com/en/training-courses");
	}

	private void PageCloudSave_TabClick(object sender, EventArgs e)
	{
		if (StiAccountSettings.CurrentAccount == null || !StiAccountSettings.CurrentAccount.AccountChecker.CheckUserActivated(((Control)Designer).Handle) || !StiAccountSettings.CurrentAccount.AccountChecker.CheckTrialExpired() || !StiAccountSettings.CurrentAccount.AccountChecker.CheckProductExpired())
		{
			return;
		}
		Point point = ((Control)((Control)Designer).FindForm()).PointToScreen(Point.Empty);
		double system = StiScale.System;
		double value = (double)(point.X + 445) * system;
		double value2 = (double)(point.Y + 127) * system;
		string fileName = Designer.Report.ReportFile;
		if (Designer != null)
		{
			fileName = Path.GetFileNameWithoutExtension(Designer.ReportFileName);
		}
		IStiCloudSaveWindow cloudSaveWindow = StiAccountSettings.CurrentAccount.AccountCreater.GetCloudSaveWindow(Designer.Report, Designer.ReportKey, fileName);
		if (!cloudSaveWindow.ShowDialog(((Control)Designer).Handle, value, value2).GetValueOrDefault())
		{
			return;
		}
		StiReport stiReport = cloudSaveWindow.Report as StiReport;
		if (!string.IsNullOrEmpty(cloudSaveWindow.ReportItemKey) && stiReport != null)
		{
			if (StiOptions.Designer.CanDesignerChangeReportFileName)
			{
				string file = "cloud\\" + cloudSaveWindow.ReportItemKey + "\\" + stiReport.ReportFile;
				Designer.AddRecentFile(file, cloudSaveWindow.ReportItemKey);
			}
			Designer.ReportItemKey = cloudSaveWindow.ReportItemKey;
			Designer.Report = stiReport;
			Designer.ReportFileName = stiReport.ReportFile;
			Designer.ReportNeverSaved = false;
			CloseBackStage();
			if (Designer != null && Designer.Report != null)
			{
				Designer.Builder.BuildReport();
			}
		}
	}

	private void PageCloud_TabClick(object sender, EventArgs e)
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		if (StiAccountSettings.CurrentAccount == null)
		{
			return;
		}
		if (cloudOpenControl == null)
		{
			cloudOpenControl = StiAccountSettings.CurrentAccount.AccountCreater.GetCloudOpenControl(((Control)Designer).Handle);
			if (cloudOpenControl != null)
			{
				((Control)panelCloud).Controls.Clear();
				cloudOpenControl.SelectedItem += CloudOpenControl_SelectedItem;
				ElementHost val = new ElementHost();
				((Control)val).Dock = (DockStyle)5;
				_003F val2 = val;
				IStiCloudOpenControl stiCloudOpenControl = cloudOpenControl;
				((ElementHost)val2).Child = (UIElement)((stiCloudOpenControl is UIElement) ? stiCloudOpenControl : null);
				cloudOpenControlElementHost = val;
				((Control)panelCloud).Controls.Add((Control)(object)cloudOpenControlElementHost);
			}
		}
		else
		{
			cloudOpenControl.RefreshAsync();
		}
	}

	private async void CloudOpenControl_SelectedItem(object sender, EventArgs e)
	{
		IStiCloudOpenControl stiCloudOpenControl = sender as IStiCloudOpenControl;
		if (!string.IsNullOrEmpty(stiCloudOpenControl.ReportItemKey))
		{
			CloseBackStage();
			await Designer.OpenRecentCloudFileAsync(stiCloudOpenControl.ReportItemKey);
		}
	}

	private async void TabControl_SelectedIndexChangedAsync(object sender, EventArgs e)
	{
		StiCloudTabulator tabulator = sender as StiCloudTabulator;
		if (tabulator.SelectedTab == pageOpen)
		{
			CloseBackStage();
			await StiActions.MainMenu.InvokeReportOpenAsync(Designer);
			tabulator.SelectedTab = pageRecent;
		}
	}

	private void BuildSchedulers()
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		if (schedulerItemsControl == null)
		{
			((Control)panelScheduleData).Controls.Clear();
			schedulerItemsControl = StiTaskSchedulerSettings.Current.GetSchedulerItemsControl(((Control)Designer).Handle);
			if (schedulerItemsControl != null)
			{
				schedulerItemsControl.AutoSaveReport += SchedulerItemsControl_AutoSaveReport;
				ElementHost val = new ElementHost();
				((Control)val).Dock = (DockStyle)5;
				_003F val2 = val;
				IStiSchedulerItemsControl stiSchedulerItemsControl = schedulerItemsControl;
				((ElementHost)val2).Child = (UIElement)((stiSchedulerItemsControl is UIElement) ? stiSchedulerItemsControl : null);
				schedulerControlElementHost = val;
				((Control)panelScheduleData).Controls.Add((Control)(object)schedulerControlElementHost);
				schedulerItemsControl.Update(Designer.Report, Designer.ReportFileName);
			}
		}
	}

	private void ButtonSchedule_MouseDown(object sender, MouseEventArgs e)
	{
		BuildSchedulers();
	}

	private void SchedulerItemsControl_AutoSaveReport(object sender, StiSchedulerSaveReportEventArgs e)
	{
		if (!File.Exists(Designer.ReportFileName))
		{
			StiActions.MainMenu.InvokeReportSave(Designer);
		}
		e.Path = Designer.ReportFileName;
	}

	protected override void Dispose(bool disposing)
	{
		foreach (ButtonX newItem in newItems)
		{
			newItem.Image = null;
			((Component)(object)newItem).Dispose();
		}
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Expected O, but got Unknown
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Expected O, but got Unknown
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Expected O, but got Unknown
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Expected O, but got Unknown
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Expected O, but got Unknown
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Expected O, but got Unknown
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Expected O, but got Unknown
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Expected O, but got Unknown
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Expected O, but got Unknown
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Expected O, but got Unknown
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Expected O, but got Unknown
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Expected O, but got Unknown
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Expected O, but got Unknown
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Expected O, but got Unknown
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Expected O, but got Unknown
		//IL_04d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04db: Expected O, but got Unknown
		//IL_04e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f1: Expected O, but got Unknown
		//IL_061b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0625: Expected O, but got Unknown
		//IL_0855: Unknown result type (might be due to invalid IL or missing references)
		//IL_085f: Expected O, but got Unknown
		//IL_0a44: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae0: Expected O, but got Unknown
		//IL_0b79: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cdf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0efc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f06: Expected O, but got Unknown
		//IL_0f42: Unknown result type (might be due to invalid IL or missing references)
		//IL_1207: Unknown result type (might be due to invalid IL or missing references)
		//IL_1211: Expected O, but got Unknown
		//IL_2131: Unknown result type (might be due to invalid IL or missing references)
		//IL_213b: Expected O, but got Unknown
		//IL_21c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_21cf: Expected O, but got Unknown
		//IL_220b: Unknown result type (might be due to invalid IL or missing references)
		//IL_241c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2426: Expected O, but got Unknown
		//IL_24b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_24be: Expected O, but got Unknown
		//IL_2596: Unknown result type (might be due to invalid IL or missing references)
		//IL_25a0: Expected O, but got Unknown
		//IL_2678: Unknown result type (might be due to invalid IL or missing references)
		//IL_2682: Expected O, but got Unknown
		//IL_2759: Unknown result type (might be due to invalid IL or missing references)
		//IL_2763: Expected O, but got Unknown
		//IL_283a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2844: Expected O, but got Unknown
		//IL_291b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2925: Expected O, but got Unknown
		//IL_29fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a06: Expected O, but got Unknown
		//IL_2add: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ae7: Expected O, but got Unknown
		//IL_2bbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bc8: Expected O, but got Unknown
		//IL_2c9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ca6: Expected O, but got Unknown
		//IL_2d77: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d81: Expected O, but got Unknown
		//IL_2dc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e96: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ea0: Expected O, but got Unknown
		//IL_2edc: Unknown result type (might be due to invalid IL or missing references)
		//IL_3095: Unknown result type (might be due to invalid IL or missing references)
		//IL_309f: Expected O, but got Unknown
		//IL_316f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3179: Expected O, but got Unknown
		//IL_3249: Unknown result type (might be due to invalid IL or missing references)
		//IL_3253: Expected O, but got Unknown
		//IL_32f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_32fb: Expected O, but got Unknown
		//IL_330b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3315: Expected O, but got Unknown
		//IL_3397: Unknown result type (might be due to invalid IL or missing references)
		//IL_370c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3716: Expected O, but got Unknown
		//IL_37a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_37b2: Expected O, but got Unknown
		//IL_37c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_37cc: Expected O, but got Unknown
		//IL_37e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_37f3: Expected O, but got Unknown
		//IL_3915: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b02: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b0c: Expected O, but got Unknown
		//IL_3b9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ba5: Expected O, but got Unknown
		//IL_3c34: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c3e: Expected O, but got Unknown
		//IL_3cf8: Unknown result type (might be due to invalid IL or missing references)
		//IL_3de1: Unknown result type (might be due to invalid IL or missing references)
		//IL_3eb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_40ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_40b5: Expected O, but got Unknown
		//IL_4147: Unknown result type (might be due to invalid IL or missing references)
		//IL_4151: Expected O, but got Unknown
		//IL_4161: Unknown result type (might be due to invalid IL or missing references)
		//IL_416b: Expected O, but got Unknown
		//IL_41ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_4298: Unknown result type (might be due to invalid IL or missing references)
		//IL_43f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_448f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4499: Expected O, but got Unknown
		//IL_44e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_4556: Unknown result type (might be due to invalid IL or missing references)
		//IL_4560: Expected O, but got Unknown
		//IL_459c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4611: Unknown result type (might be due to invalid IL or missing references)
		//IL_461b: Expected O, but got Unknown
		//IL_4657: Unknown result type (might be due to invalid IL or missing references)
		//IL_46cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_46d6: Expected O, but got Unknown
		//IL_4712: Unknown result type (might be due to invalid IL or missing references)
		//IL_47ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_483f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4849: Expected O, but got Unknown
		//IL_4885: Unknown result type (might be due to invalid IL or missing references)
		//IL_48fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_4904: Expected O, but got Unknown
		//IL_4940: Unknown result type (might be due to invalid IL or missing references)
		//IL_49ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a61: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a6b: Expected O, but got Unknown
		//IL_4aa7: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b26: Expected O, but got Unknown
		//IL_4b62: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bd7: Unknown result type (might be due to invalid IL or missing references)
		//IL_4be1: Expected O, but got Unknown
		//IL_4c1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c92: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c9c: Expected O, but got Unknown
		//IL_4cd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d57: Expected O, but got Unknown
		//IL_4d93: Unknown result type (might be due to invalid IL or missing references)
		//IL_4edf: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ee9: Expected O, but got Unknown
		//IL_4f7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f85: Expected O, but got Unknown
		//IL_4f95: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f9f: Expected O, but got Unknown
		//IL_4fbc: Unknown result type (might be due to invalid IL or missing references)
		//IL_4fc6: Expected O, but got Unknown
		//IL_501b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5025: Expected O, but got Unknown
		//IL_5035: Unknown result type (might be due to invalid IL or missing references)
		//IL_503f: Expected O, but got Unknown
		//IL_505c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5066: Expected O, but got Unknown
		//IL_5092: Unknown result type (might be due to invalid IL or missing references)
		//IL_509c: Expected O, but got Unknown
		//IL_50ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_50b6: Expected O, but got Unknown
		//IL_50d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_50dd: Expected O, but got Unknown
		//IL_511a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5124: Expected O, but got Unknown
		//IL_5134: Unknown result type (might be due to invalid IL or missing references)
		//IL_513e: Expected O, but got Unknown
		//IL_5167: Unknown result type (might be due to invalid IL or missing references)
		//IL_5171: Expected O, but got Unknown
		//IL_519d: Unknown result type (might be due to invalid IL or missing references)
		//IL_51a7: Expected O, but got Unknown
		//IL_51b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_51c1: Expected O, but got Unknown
		//IL_51ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_51f4: Expected O, but got Unknown
		//IL_5220: Unknown result type (might be due to invalid IL or missing references)
		//IL_522a: Expected O, but got Unknown
		//IL_523a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5244: Expected O, but got Unknown
		//IL_526d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5277: Expected O, but got Unknown
		//IL_52a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_52ad: Expected O, but got Unknown
		//IL_52bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_52c7: Expected O, but got Unknown
		//IL_52f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_52fa: Expected O, but got Unknown
		//IL_5332: Unknown result type (might be due to invalid IL or missing references)
		//IL_533c: Expected O, but got Unknown
		//IL_534c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5356: Expected O, but got Unknown
		//IL_5373: Unknown result type (might be due to invalid IL or missing references)
		//IL_537d: Expected O, but got Unknown
		//IL_53a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_53b3: Expected O, but got Unknown
		//IL_53c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_53cd: Expected O, but got Unknown
		//IL_53ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_53f4: Expected O, but got Unknown
		//IL_5420: Unknown result type (might be due to invalid IL or missing references)
		//IL_542a: Expected O, but got Unknown
		//IL_5446: Unknown result type (might be due to invalid IL or missing references)
		//IL_5450: Expected O, but got Unknown
		//IL_546d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5477: Expected O, but got Unknown
		//IL_54b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_54be: Expected O, but got Unknown
		//IL_54ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_54d8: Expected O, but got Unknown
		//IL_5574: Unknown result type (might be due to invalid IL or missing references)
		//IL_557e: Expected O, but got Unknown
		//IL_5600: Unknown result type (might be due to invalid IL or missing references)
		//IL_560a: Expected O, but got Unknown
		//IL_561a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5624: Expected O, but got Unknown
		//IL_5641: Unknown result type (might be due to invalid IL or missing references)
		//IL_564b: Expected O, but got Unknown
		//IL_56bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_57f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_57fc: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiRibbonBackStageUserControl));
		imageList1 = new ImageList(components);
		BackStage = new SuperTabControl();
		superTabControlPanel2 = new SuperTabControlPanel();
		buttonBack = new SuperTabItem();
		superTabControlPanelOpen = new SuperTabControlPanel();
		superTabControlPanelSchedule = new SuperTabControlPanel();
		superTabControlPanelHelp = new SuperTabControlPanel();
		tabControl = new StiCloudTabulator();
		panelScheduleData = new Panel();
		pageRecent = new StiCloudTabulatorPage();
		panel8 = new Panel();
		biRecentLocation09 = new ButtonX();
		labelRecentLocations = new LabelX();
		biRecentLocation08 = new ButtonX();
		biRecentLocation01 = new ButtonX();
		biRecentLocation07 = new ButtonX();
		biRecentLocation02 = new ButtonX();
		biRecentLocation06 = new ButtonX();
		biRecentLocation03 = new ButtonX();
		biRecentLocation05 = new ButtonX();
		biRecentLocation04 = new ButtonX();
		panel7 = new Panel();
		biRecentFile09 = new ButtonX();
		biRecentFile08 = new ButtonX();
		biRecentFile07 = new ButtonX();
		biRecentFile06 = new ButtonX();
		biRecentFile05 = new ButtonX();
		biRecentFile04 = new ButtonX();
		biRecentFile03 = new ButtonX();
		biRecentFile02 = new ButtonX();
		biRecentFile01 = new ButtonX();
		labelRecentDocuments = new LabelX();
		pageImport = new StiCloudTabulatorPage();
		panelImport = new Panel();
		labelImport = new LabelX();
		buttonVisualFoxPro = new ButtonX();
		buttonXtraReports = new ButtonX();
		buttonTelerikReporting = new ButtonX();
		buttonMSRS = new ButtonX();
		buttonRTF = new ButtonX();
		buttonReportSharpShooter = new ButtonX();
		buttonFastReport = new ButtonX();
		buttonCrystalReports = new ButtonX();
		buttonComponentOne = new ButtonX();
		buttonActiveReports = new ButtonX();
		pageCloud = new StiCloudTabulatorPage();
		panelCloud = new Panel();
		pageOpen = new StiCloudTabulatorPage();
		panel1 = new Panel();
		panelScheduleTitle = new Panel();
		panelHelp = new Panel();
		labelTitleOpen = new LabelX();
		labelTitleSchedule = new LabelX();
		labelTitleHelp = new LabelX();
		buttonReportOpen = new SuperTabItem();
		superTabControlPanelNew = new SuperTabControlPanel();
		panelItems = new Panel();
		reportFlowItems = new FlowLayoutPanel();
		dashboardFlowItems = new FlowLayoutPanel();
		formFlowItems = new FlowLayoutPanel();
		panel3 = new Panel();
		labelTitleNew = new LabelX();
		buttonNew = new SuperTabItem();
		superTabControlPanelInfo = new SuperTabControlPanel();
		labelCheckIssues = new LabelX();
		labelProtectDocument = new LabelX();
		labelReportOptions = new LabelX();
		labelCheckIssuesTitle = new LabelX();
		labelProtectDocumentTitle = new LabelX();
		labelReportOptionsTitle = new LabelX();
		buttonCheckIssue = new Button();
		buttonProtectDocument = new Button();
		buttonReportOptions = new Button();
		panel2 = new Panel();
		labelTitleInfo = new LabelX();
		buttonInfo = new SuperTabItem();
		superTabControlPanelSaveAs = new SuperTabControlPanel();
		tabControlSaveAs = new StiCloudTabulator();
		tabControlHelp = new StiCloudTabulator();
		pageCloudSave = new StiCloudTabulatorPage();
		pageSaveAs = new StiCloudTabulatorPage();
		pageHelpDocumentation = new StiCloudTabulatorPage();
		pageHelpSupport = new StiCloudTabulatorPage();
		pageHelpSeparator1 = new StiCloudTabulatorPage();
		pageHelpVideos = new StiCloudTabulatorPage();
		pageHelpSamples = new StiCloudTabulatorPage();
		pageHelpSeparator2 = new StiCloudTabulatorPage();
		pageHelpForum = new StiCloudTabulatorPage();
		pageHelpDemos = new StiCloudTabulatorPage();
		pageHelpTrainingCourses = new StiCloudTabulatorPage();
		pageHelpGitHub = new StiCloudTabulatorPage();
		pageHelpNuget = new StiCloudTabulatorPage();
		panelSaveAs = new Panel();
		labelTitleSaveAs = new LabelX();
		buttonReportSaveAs = new SuperTabItem();
		labelBackSeparator = new LabelItem();
		buttonReportSave = new SuperTabItem();
		buttonPreview = new SuperTabItem();
		buttonSchedule = new SuperTabItem();
		buttonShare = new SuperTabItem();
		buttonPublish = new SuperTabItem();
		buttonGetStarted = new SuperTabItem();
		buttonClose = new SuperTabItem();
		buttonOptions = new SuperTabItem();
		buttonAbout = new SuperTabItem();
		buttonHelp = new SuperTabItem();
		labelRecentSeparator = new LabelX();
		buttonAccount = new SuperTabItem();
		superTabControlPanelAccount = new SuperTabControlPanel();
		panel4 = new Panel();
		labelTitleAccount = new LabelX();
		panelAccount = new Panel();
		((ISupportInitialize)BackStage).BeginInit();
		((Control)BackStage).SuspendLayout();
		((Control)superTabControlPanelOpen).SuspendLayout();
		((Control)superTabControlPanelSchedule).SuspendLayout();
		((Control)superTabControlPanelHelp).SuspendLayout();
		((Control)tabControl).SuspendLayout();
		((Control)panelScheduleData).SuspendLayout();
		((Control)pageRecent).SuspendLayout();
		((Control)panel8).SuspendLayout();
		((Control)panel7).SuspendLayout();
		((Control)pageImport).SuspendLayout();
		((Control)panelImport).SuspendLayout();
		((Control)pageCloud).SuspendLayout();
		((Control)panel1).SuspendLayout();
		((Control)panelScheduleTitle).SuspendLayout();
		((Control)superTabControlPanelNew).SuspendLayout();
		((Control)panelItems).SuspendLayout();
		((Control)panel3).SuspendLayout();
		((Control)superTabControlPanelInfo).SuspendLayout();
		((Control)panel2).SuspendLayout();
		((Control)superTabControlPanelSaveAs).SuspendLayout();
		((Control)tabControlSaveAs).SuspendLayout();
		((Control)tabControlHelp).SuspendLayout();
		((Control)panelSaveAs).SuspendLayout();
		((Control)superTabControlPanelAccount).SuspendLayout();
		((Control)this).SuspendLayout();
		imageList1.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("imageList1.ImageStream");
		imageList1.TransparentColor = Color.Transparent;
		imageList1.Images.SetKeyName(0, "mrt");
		imageList1.Images.SetKeyName(1, "mrx");
		imageList1.Images.SetKeyName(2, "mrz");
		imageList1.Images.SetKeyName(3, "open");
		imageList1.Images.SetKeyName(4, "BigReportWizard.png");
		imageList1.Images.SetKeyName(5, "BigReportTemplate.png");
		imageList1.Images.SetKeyName(6, "report");
		((Control)BackStage).BackColor = Color.FromArgb(255, 255, 255);
		BackStage.ControlBox.CloseBox.Name = "";
		BackStage.ControlBox.MenuBox.Name = "";
		BackStage.ControlBox.Name = "";
		BackStage.ControlBox.SubItems.AddRange(new BaseItem[2]
		{
			BackStage.ControlBox.MenuBox,
			BackStage.ControlBox.CloseBox
		});
		BackStage.ControlBox.Visible = false;
		((Control)BackStage).Controls.Add((Control)(object)superTabControlPanelAccount);
		((Control)BackStage).Controls.Add((Control)(object)superTabControlPanel2);
		((Control)BackStage).Controls.Add((Control)(object)superTabControlPanelSchedule);
		((Control)BackStage).Controls.Add((Control)(object)superTabControlPanelOpen);
		((Control)BackStage).Controls.Add((Control)(object)superTabControlPanelHelp);
		((Control)BackStage).Controls.Add((Control)(object)superTabControlPanelNew);
		((Control)BackStage).Controls.Add((Control)(object)superTabControlPanelInfo);
		((Control)BackStage).Controls.Add((Control)(object)superTabControlPanelSaveAs);
		((Control)BackStage).Dock = (DockStyle)5;
		((Control)BackStage).Font = new Font("Microsoft Sans Serif", 9f);
		((Control)BackStage).ForeColor = Color.Black;
		BackStage.ItemPadding.Left = 6;
		BackStage.ItemPadding.Right = 4;
		BackStage.ItemPadding.Top = 4;
		((Control)BackStage).Location = new Point(0, 0);
		((Control)BackStage).Name = "BackStage";
		BackStage.ReorderTabsEnabled = false;
		BackStage.SelectedTabIndex = 1;
		((Control)BackStage).Size = new Size(1115, 813);
		BackStage.TabAlignment = eTabStripAlignment.Left;
		BackStage.TabHorizontalSpacing = 10;
		((Control)BackStage).TabIndex = 2;
		BackStage.Tabs.AddRange(new BaseItem[17]
		{
			labelBackSeparator, buttonBack, buttonInfo, buttonNew, buttonReportOpen, buttonReportSave, buttonReportSaveAs, buttonPreview, buttonSchedule, buttonShare,
			buttonPublish, buttonHelp, buttonGetStarted, buttonClose, buttonAccount, buttonOptions, buttonAbout
		});
		BackStage.TabStyle = eSuperTabStyle.Office2010BackstageBlue;
		BackStage.TabVerticalSpacing = 8;
		((Control)BackStage).Text = "superTabControl1";
		((Control)BackStage).Visible = false;
		((Control)BackStage).VisibleChanged += BackStage_VisibleChanged;
		((Control)superTabControlPanel2).Dock = (DockStyle)5;
		((Control)superTabControlPanel2).Location = new Point(99, 0);
		((Control)superTabControlPanel2).Margin = new Padding(2);
		((Control)superTabControlPanel2).Name = "superTabControlPanel2";
		((Control)superTabControlPanel2).Size = new Size(1016, 813);
		((Control)superTabControlPanel2).TabIndex = 0;
		superTabControlPanel2.TabItem = buttonBack;
		buttonBack.AttachedControl = (Control)(object)superTabControlPanel2;
		buttonBack.FixedTabSize = new Size(0, 42);
		buttonBack.GlobalItem = false;
		buttonBack.Image = (Image)componentResourceManager.GetObject("buttonBack.Image");
		buttonBack.Name = "buttonBack";
		buttonBack.Click += ButtonBack_Click;
		superTabControlPanelOpen.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		((Control)superTabControlPanelOpen).Controls.Add((Control)(object)tabControl);
		((Control)superTabControlPanelOpen).Controls.Add((Control)(object)panel1);
		((Control)superTabControlPanelOpen).Dock = (DockStyle)5;
		((Control)superTabControlPanelOpen).Location = new Point(99, 0);
		((Control)superTabControlPanelOpen).Name = "superTabControlPanelOpen";
		((Control)superTabControlPanelOpen).Padding = new Padding(40, 2, 2, 2);
		((Control)superTabControlPanelOpen).Size = new Size(1016, 813);
		((Control)superTabControlPanelOpen).TabIndex = 1;
		superTabControlPanelOpen.TabItem = buttonReportOpen;
		superTabControlPanelSchedule.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		((Control)superTabControlPanelSchedule).Controls.Add((Control)(object)panelScheduleData);
		((Control)superTabControlPanelSchedule).Controls.Add((Control)(object)panelScheduleTitle);
		((Control)superTabControlPanelSchedule).Dock = (DockStyle)5;
		((Control)superTabControlPanelSchedule).Location = new Point(99, 0);
		((Control)superTabControlPanelSchedule).Name = "superTabControlPanelSchedule";
		((Control)superTabControlPanelSchedule).Padding = new Padding(40, 2, 2, 2);
		((Control)superTabControlPanelSchedule).Size = new Size(1016, 813);
		((Control)superTabControlPanelSchedule).TabIndex = 1;
		superTabControlPanelSchedule.TabItem = buttonSchedule;
		superTabControlPanelHelp.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		((Control)superTabControlPanelHelp).Controls.Add((Control)(object)tabControlHelp);
		((Control)superTabControlPanelHelp).Controls.Add((Control)(object)panelHelp);
		((Control)superTabControlPanelHelp).Dock = (DockStyle)5;
		((Control)superTabControlPanelHelp).Location = new Point(99, 0);
		((Control)superTabControlPanelHelp).Name = "superTabControlPanelHelp";
		((Control)superTabControlPanelHelp).Padding = new Padding(40, 2, 2, 2);
		((Control)superTabControlPanelHelp).Size = new Size(1016, 813);
		((Control)superTabControlPanelHelp).TabIndex = 1;
		superTabControlPanelHelp.TabItem = buttonHelp;
		((Control)tabControl).Controls.Add((Control)(object)pageRecent);
		((Control)tabControl).Controls.Add((Control)(object)pageImport);
		((Control)tabControl).Controls.Add((Control)(object)pageCloud);
		((Control)tabControl).Controls.Add((Control)(object)pageOpen);
		((Control)tabControl).Dock = (DockStyle)5;
		((Control)tabControl).Location = new Point(40, 102);
		((Control)tabControl).Name = "tabControl";
		((Control)tabControl).Padding = new Padding(253, 0, 0, 0);
		tabControl.ScrollHeight = 0;
		tabControl.ScrollWidth = 100;
		((Control)tabControl).Size = new Size(974, 709);
		((Control)tabControl).TabIndex = 4;
		tabControl.TitleBackColor = Color.Transparent;
		tabControl.TitleInLine = true;
		tabControl.TitlePosition = StiTabTitlePosition.LeftHorizontal;
		tabControl.TitleWidth = 250;
		tabControl.SelectedIndexChanged += TabControl_SelectedIndexChangedAsync;
		((Control)panelScheduleData).Dock = (DockStyle)5;
		((Control)panelScheduleData).BackColor = Color.White;
		((Control)panelScheduleData).Location = new Point(0, 102);
		((Control)panelScheduleData).Name = "panelScheduleData";
		((Control)panelScheduleData).Size = new Size(974, 709);
		((Control)panelScheduleData).TabIndex = 4;
		((Control)pageRecent).Controls.Add((Control)(object)panel8);
		((Control)pageRecent).Controls.Add((Control)(object)panel7);
		pageRecent.Dock = (DockStyle)5;
		pageRecent.Image = (Image)componentResourceManager.GetObject("pageRecent.Image");
		pageRecent.InvisibleVisible = true;
		((Control)pageRecent).Location = new Point(253, 0);
		((Control)pageRecent).Name = "pageRecent";
		((Control)pageRecent).Padding = new Padding(1, 0, 1, 0);
		((Control)pageRecent).Size = new Size(721, 709);
		((Control)pageRecent).TabIndex = 0;
		((Control)pageRecent).Text = "Recent";
		((Control)pageRecent).Visible = false;
		((Control)panel8).BackColor = Color.White;
		((Control)panel8).Controls.Add((Control)(object)biRecentLocation09);
		((Control)panel8).Controls.Add((Control)(object)labelRecentLocations);
		((Control)panel8).Controls.Add((Control)(object)biRecentLocation08);
		((Control)panel8).Controls.Add((Control)(object)biRecentLocation01);
		((Control)panel8).Controls.Add((Control)(object)biRecentLocation07);
		((Control)panel8).Controls.Add((Control)(object)biRecentLocation02);
		((Control)panel8).Controls.Add((Control)(object)biRecentLocation06);
		((Control)panel8).Controls.Add((Control)(object)biRecentLocation03);
		((Control)panel8).Controls.Add((Control)(object)biRecentLocation05);
		((Control)panel8).Controls.Add((Control)(object)biRecentLocation04);
		((Control)panel8).Dock = (DockStyle)3;
		((Control)panel8).Location = new Point(399, 0);
		((Control)panel8).Name = "panel8";
		((Control)panel8).Size = new Size(398, 709);
		((Control)panel8).TabIndex = 6;
		((Control)biRecentLocation09).AccessibleRole = (AccessibleRole)43;
		biRecentLocation09.AntiAlias = true;
		biRecentLocation09.ColorTable = eButtonColor.Orange;
		biRecentLocation09.ImageTextSpacing = 5;
		biRecentLocation09.Is20Button = false;
		((Control)biRecentLocation09).Location = new Point(9, 440);
		((Control)biRecentLocation09).Name = "biRecentLocation09";
		((Control)biRecentLocation09).Size = new Size(343, 42);
		biRecentLocation09.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)biRecentLocation09).TabIndex = 8;
		((Control)biRecentLocation09).Text = "File";
		biRecentLocation09.TextAlignment = eButtonTextAlignment.Left;
		((Control)biRecentLocation09).Click += ButtonRecentLocation_Click;
		((Control)labelRecentLocations).BackColor = Color.Transparent;
		labelRecentLocations.BackgroundStyle.BorderBottom = eStyleBorderType.Dash;
		labelRecentLocations.BackgroundStyle.BorderBottomColor = Color.Gray;
		labelRecentLocations.BackgroundStyle.BorderBottomWidth = 1;
		labelRecentLocations.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)labelRecentLocations).Font = new Font("Microsoft Sans Serif", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)204);
		((Control)labelRecentLocations).Location = new Point(9, 27);
		((Control)labelRecentLocations).Name = "labelRecentLocations";
		((Control)labelRecentLocations).Size = new Size(343, 23);
		((Control)labelRecentLocations).TabIndex = 1;
		((Control)labelRecentLocations).Text = "Recent Locations";
		((Control)biRecentLocation08).AccessibleRole = (AccessibleRole)43;
		biRecentLocation08.AntiAlias = true;
		biRecentLocation08.ColorTable = eButtonColor.Orange;
		biRecentLocation08.ImageTextSpacing = 5;
		biRecentLocation08.Is20Button = false;
		((Control)biRecentLocation08).Location = new Point(9, 392);
		((Control)biRecentLocation08).Name = "biRecentLocation08";
		((Control)biRecentLocation08).Size = new Size(343, 42);
		biRecentLocation08.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)biRecentLocation08).TabIndex = 7;
		((Control)biRecentLocation08).Text = "File";
		biRecentLocation08.TextAlignment = eButtonTextAlignment.Left;
		((Control)biRecentLocation08).Click += ButtonRecentLocation_Click;
		((Control)biRecentLocation01).AccessibleRole = (AccessibleRole)43;
		biRecentLocation01.AntiAlias = true;
		biRecentLocation01.ColorTable = eButtonColor.Orange;
		biRecentLocation01.ImageTextSpacing = 5;
		biRecentLocation01.Is20Button = false;
		((Control)biRecentLocation01).Location = new Point(9, 56);
		((Control)biRecentLocation01).Name = "biRecentLocation01";
		((Control)biRecentLocation01).Size = new Size(343, 42);
		biRecentLocation01.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)biRecentLocation01).TabIndex = 0;
		((Control)biRecentLocation01).Text = "File";
		biRecentLocation01.TextAlignment = eButtonTextAlignment.Left;
		((Control)biRecentLocation01).Click += ButtonRecentLocation_Click;
		((Control)biRecentLocation07).AccessibleRole = (AccessibleRole)43;
		biRecentLocation07.AntiAlias = true;
		biRecentLocation07.ColorTable = eButtonColor.Orange;
		biRecentLocation07.ImageTextSpacing = 5;
		biRecentLocation07.Is20Button = false;
		((Control)biRecentLocation07).Location = new Point(9, 344);
		((Control)biRecentLocation07).Name = "biRecentLocation07";
		((Control)biRecentLocation07).Size = new Size(343, 42);
		biRecentLocation07.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)biRecentLocation07).TabIndex = 6;
		((Control)biRecentLocation07).Text = "File";
		biRecentLocation07.TextAlignment = eButtonTextAlignment.Left;
		((Control)biRecentLocation07).Click += ButtonRecentLocation_Click;
		((Control)biRecentLocation02).AccessibleRole = (AccessibleRole)43;
		biRecentLocation02.AntiAlias = true;
		biRecentLocation02.ColorTable = eButtonColor.Orange;
		biRecentLocation02.ImageTextSpacing = 5;
		biRecentLocation02.Is20Button = false;
		((Control)biRecentLocation02).Location = new Point(9, 104);
		((Control)biRecentLocation02).Name = "biRecentLocation02";
		((Control)biRecentLocation02).Size = new Size(343, 42);
		biRecentLocation02.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)biRecentLocation02).TabIndex = 1;
		((Control)biRecentLocation02).Text = "File";
		biRecentLocation02.TextAlignment = eButtonTextAlignment.Left;
		((Control)biRecentLocation02).Click += ButtonRecentLocation_Click;
		((Control)biRecentLocation06).AccessibleRole = (AccessibleRole)43;
		biRecentLocation06.AntiAlias = true;
		biRecentLocation06.ColorTable = eButtonColor.Orange;
		biRecentLocation06.ImageTextSpacing = 5;
		biRecentLocation06.Is20Button = false;
		((Control)biRecentLocation06).Location = new Point(9, 296);
		((Control)biRecentLocation06).Name = "biRecentLocation06";
		((Control)biRecentLocation06).Size = new Size(343, 42);
		biRecentLocation06.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)biRecentLocation06).TabIndex = 5;
		((Control)biRecentLocation06).Text = "File";
		biRecentLocation06.TextAlignment = eButtonTextAlignment.Left;
		((Control)biRecentLocation06).Click += ButtonRecentLocation_Click;
		((Control)biRecentLocation03).AccessibleRole = (AccessibleRole)43;
		biRecentLocation03.AntiAlias = true;
		biRecentLocation03.ColorTable = eButtonColor.Orange;
		biRecentLocation03.ImageTextSpacing = 5;
		biRecentLocation03.Is20Button = false;
		((Control)biRecentLocation03).Location = new Point(9, 152);
		((Control)biRecentLocation03).Name = "biRecentLocation03";
		((Control)biRecentLocation03).Size = new Size(343, 42);
		biRecentLocation03.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)biRecentLocation03).TabIndex = 2;
		((Control)biRecentLocation03).Text = "File";
		biRecentLocation03.TextAlignment = eButtonTextAlignment.Left;
		((Control)biRecentLocation03).Click += ButtonRecentLocation_Click;
		((Control)biRecentLocation05).AccessibleRole = (AccessibleRole)43;
		biRecentLocation05.AntiAlias = true;
		biRecentLocation05.ColorTable = eButtonColor.Orange;
		biRecentLocation05.ImageTextSpacing = 5;
		biRecentLocation05.Is20Button = false;
		((Control)biRecentLocation05).Location = new Point(9, 248);
		((Control)biRecentLocation05).Name = "biRecentLocation05";
		((Control)biRecentLocation05).Size = new Size(343, 42);
		biRecentLocation05.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)biRecentLocation05).TabIndex = 4;
		((Control)biRecentLocation05).Text = "File";
		biRecentLocation05.TextAlignment = eButtonTextAlignment.Left;
		((Control)biRecentLocation05).Click += ButtonRecentLocation_Click;
		((Control)biRecentLocation04).AccessibleRole = (AccessibleRole)43;
		biRecentLocation04.AntiAlias = true;
		biRecentLocation04.ColorTable = eButtonColor.Orange;
		biRecentLocation04.ImageTextSpacing = 5;
		biRecentLocation04.Is20Button = false;
		((Control)biRecentLocation04).Location = new Point(9, 200);
		((Control)biRecentLocation04).Name = "biRecentLocation04";
		((Control)biRecentLocation04).Size = new Size(343, 42);
		biRecentLocation04.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)biRecentLocation04).TabIndex = 3;
		((Control)biRecentLocation04).Text = "File";
		biRecentLocation04.TextAlignment = eButtonTextAlignment.Left;
		((Control)biRecentLocation04).Click += ButtonRecentLocation_Click;
		((Control)panel7).BackColor = Color.White;
		((Control)panel7).Controls.Add((Control)(object)biRecentFile09);
		((Control)panel7).Controls.Add((Control)(object)biRecentFile08);
		((Control)panel7).Controls.Add((Control)(object)biRecentFile07);
		((Control)panel7).Controls.Add((Control)(object)biRecentFile06);
		((Control)panel7).Controls.Add((Control)(object)biRecentFile05);
		((Control)panel7).Controls.Add((Control)(object)biRecentFile04);
		((Control)panel7).Controls.Add((Control)(object)biRecentFile03);
		((Control)panel7).Controls.Add((Control)(object)biRecentFile02);
		((Control)panel7).Controls.Add((Control)(object)biRecentFile01);
		((Control)panel7).Controls.Add((Control)(object)labelRecentDocuments);
		((Control)panel7).Dock = (DockStyle)3;
		((Control)panel7).Location = new Point(1, 0);
		((Control)panel7).Name = "panel7";
		((Control)panel7).Size = new Size(398, 709);
		((Control)panel7).TabIndex = 5;
		((Control)biRecentFile09).AccessibleRole = (AccessibleRole)43;
		biRecentFile09.AntiAlias = true;
		biRecentFile09.ColorTable = eButtonColor.Orange;
		biRecentFile09.ImageTextSpacing = 5;
		biRecentFile09.Is20Button = false;
		((Control)biRecentFile09).Location = new Point(27, 440);
		((Control)biRecentFile09).Name = "biRecentFile09";
		((Control)biRecentFile09).Size = new Size(343, 42);
		biRecentFile09.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)biRecentFile09).TabIndex = 8;
		((Control)biRecentFile09).Text = "File";
		biRecentFile09.TextAlignment = eButtonTextAlignment.Left;
		((Control)biRecentFile09).Click += ButtonRecentFile_Click;
		((Control)biRecentFile08).AccessibleRole = (AccessibleRole)43;
		biRecentFile08.AntiAlias = true;
		biRecentFile08.ColorTable = eButtonColor.Orange;
		biRecentFile08.ImageTextSpacing = 5;
		biRecentFile08.Is20Button = false;
		((Control)biRecentFile08).Location = new Point(27, 392);
		((Control)biRecentFile08).Name = "biRecentFile08";
		((Control)biRecentFile08).Size = new Size(343, 42);
		biRecentFile08.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)biRecentFile08).TabIndex = 7;
		((Control)biRecentFile08).Text = "File";
		biRecentFile08.TextAlignment = eButtonTextAlignment.Left;
		((Control)biRecentFile08).Click += ButtonRecentFile_Click;
		((Control)biRecentFile07).AccessibleRole = (AccessibleRole)43;
		biRecentFile07.AntiAlias = true;
		biRecentFile07.ColorTable = eButtonColor.Orange;
		biRecentFile07.ImageTextSpacing = 5;
		biRecentFile07.Is20Button = false;
		((Control)biRecentFile07).Location = new Point(27, 344);
		((Control)biRecentFile07).Name = "biRecentFile07";
		((Control)biRecentFile07).Size = new Size(343, 42);
		biRecentFile07.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)biRecentFile07).TabIndex = 6;
		((Control)biRecentFile07).Text = "File";
		biRecentFile07.TextAlignment = eButtonTextAlignment.Left;
		((Control)biRecentFile07).Click += ButtonRecentFile_Click;
		((Control)biRecentFile06).AccessibleRole = (AccessibleRole)43;
		biRecentFile06.AntiAlias = true;
		biRecentFile06.ColorTable = eButtonColor.Orange;
		biRecentFile06.ImageTextSpacing = 5;
		biRecentFile06.Is20Button = false;
		((Control)biRecentFile06).Location = new Point(27, 296);
		((Control)biRecentFile06).Name = "biRecentFile06";
		((Control)biRecentFile06).Size = new Size(343, 42);
		biRecentFile06.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)biRecentFile06).TabIndex = 5;
		((Control)biRecentFile06).Text = "File";
		biRecentFile06.TextAlignment = eButtonTextAlignment.Left;
		((Control)biRecentFile06).Click += ButtonRecentFile_Click;
		((Control)biRecentFile05).AccessibleRole = (AccessibleRole)43;
		biRecentFile05.AntiAlias = true;
		biRecentFile05.ColorTable = eButtonColor.Orange;
		biRecentFile05.ImageTextSpacing = 5;
		biRecentFile05.Is20Button = false;
		((Control)biRecentFile05).Location = new Point(27, 248);
		((Control)biRecentFile05).Name = "biRecentFile05";
		((Control)biRecentFile05).Size = new Size(343, 42);
		biRecentFile05.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)biRecentFile05).TabIndex = 4;
		((Control)biRecentFile05).Text = "File";
		biRecentFile05.TextAlignment = eButtonTextAlignment.Left;
		((Control)biRecentFile05).Click += ButtonRecentFile_Click;
		((Control)biRecentFile04).AccessibleRole = (AccessibleRole)43;
		biRecentFile04.AntiAlias = true;
		biRecentFile04.ColorTable = eButtonColor.Orange;
		biRecentFile04.ImageTextSpacing = 5;
		biRecentFile04.Is20Button = false;
		((Control)biRecentFile04).Location = new Point(27, 200);
		((Control)biRecentFile04).Name = "biRecentFile04";
		((Control)biRecentFile04).Size = new Size(343, 42);
		biRecentFile04.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)biRecentFile04).TabIndex = 3;
		((Control)biRecentFile04).Text = "File";
		biRecentFile04.TextAlignment = eButtonTextAlignment.Left;
		((Control)biRecentFile04).Click += ButtonRecentFile_Click;
		((Control)biRecentFile03).AccessibleRole = (AccessibleRole)43;
		biRecentFile03.AntiAlias = true;
		biRecentFile03.ColorTable = eButtonColor.Orange;
		biRecentFile03.ImageTextSpacing = 5;
		biRecentFile03.Is20Button = false;
		((Control)biRecentFile03).Location = new Point(27, 152);
		((Control)biRecentFile03).Name = "biRecentFile03";
		((Control)biRecentFile03).Size = new Size(343, 42);
		biRecentFile03.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)biRecentFile03).TabIndex = 2;
		((Control)biRecentFile03).Text = "File";
		biRecentFile03.TextAlignment = eButtonTextAlignment.Left;
		((Control)biRecentFile03).Click += ButtonRecentFile_Click;
		((Control)biRecentFile02).AccessibleRole = (AccessibleRole)43;
		biRecentFile02.AntiAlias = true;
		biRecentFile02.ColorTable = eButtonColor.Orange;
		biRecentFile02.ImageTextSpacing = 5;
		biRecentFile02.Is20Button = false;
		((Control)biRecentFile02).Location = new Point(27, 104);
		((Control)biRecentFile02).Name = "biRecentFile02";
		((Control)biRecentFile02).Size = new Size(343, 42);
		biRecentFile02.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)biRecentFile02).TabIndex = 1;
		((Control)biRecentFile02).Text = "File";
		biRecentFile02.TextAlignment = eButtonTextAlignment.Left;
		((Control)biRecentFile02).Click += ButtonRecentFile_Click;
		((Control)biRecentFile01).AccessibleRole = (AccessibleRole)43;
		biRecentFile01.AntiAlias = true;
		biRecentFile01.ColorTable = eButtonColor.Orange;
		biRecentFile01.ImageTextSpacing = 5;
		biRecentFile01.Is20Button = false;
		((Control)biRecentFile01).Location = new Point(27, 56);
		((Control)biRecentFile01).Name = "biRecentFile01";
		((Control)biRecentFile01).Size = new Size(343, 42);
		biRecentFile01.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)biRecentFile01).TabIndex = 0;
		((Control)biRecentFile01).Text = "File";
		biRecentFile01.TextAlignment = eButtonTextAlignment.Left;
		((Control)biRecentFile01).Click += ButtonRecentFile_Click;
		((Control)labelRecentDocuments).BackColor = Color.Transparent;
		labelRecentDocuments.BackgroundStyle.BorderBottom = eStyleBorderType.Dash;
		labelRecentDocuments.BackgroundStyle.BorderBottomColor = Color.Gray;
		labelRecentDocuments.BackgroundStyle.BorderBottomWidth = 1;
		labelRecentDocuments.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)labelRecentDocuments).Font = new Font("Microsoft Sans Serif", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)204);
		((Control)labelRecentDocuments).Location = new Point(27, 27);
		((Control)labelRecentDocuments).Name = "labelRecentDocuments";
		((Control)labelRecentDocuments).Size = new Size(343, 23);
		((Control)labelRecentDocuments).TabIndex = 1;
		((Control)labelRecentDocuments).Text = "Recent Documents";
		((Control)pageImport).Controls.Add((Control)(object)panelImport);
		pageImport.Dock = (DockStyle)5;
		pageImport.Image = (Image)componentResourceManager.GetObject("pageImport.Image");
		pageImport.InvisibleVisible = false;
		((Control)pageImport).Location = new Point(253, 0);
		((Control)pageImport).Name = "pageImport";
		((Control)pageImport).Padding = new Padding(1, 0, 0, 0);
		((Control)pageImport).Size = new Size(636, 713);
		((Control)pageImport).TabIndex = 2;
		((Control)pageImport).Text = "Import";
		((Control)pageImport).Visible = false;
		((Control)panelImport).BackColor = Color.White;
		((Control)panelImport).Controls.Add((Control)(object)labelImport);
		((Control)panelImport).Controls.Add((Control)(object)buttonVisualFoxPro);
		((Control)panelImport).Controls.Add((Control)(object)buttonXtraReports);
		((Control)panelImport).Controls.Add((Control)(object)buttonTelerikReporting);
		((Control)panelImport).Controls.Add((Control)(object)buttonMSRS);
		((Control)panelImport).Controls.Add((Control)(object)buttonRTF);
		((Control)panelImport).Controls.Add((Control)(object)buttonReportSharpShooter);
		((Control)panelImport).Controls.Add((Control)(object)buttonFastReport);
		((Control)panelImport).Controls.Add((Control)(object)buttonCrystalReports);
		((Control)panelImport).Controls.Add((Control)(object)buttonComponentOne);
		((Control)panelImport).Controls.Add((Control)(object)buttonActiveReports);
		((Control)panelImport).Dock = (DockStyle)3;
		((Control)panelImport).Location = new Point(1, 0);
		((Control)panelImport).Name = "panelImport";
		((Control)panelImport).Size = new Size(398, 713);
		((Control)panelImport).TabIndex = 6;
		((Control)labelImport).BackColor = Color.Transparent;
		labelImport.BackgroundStyle.BorderBottom = eStyleBorderType.Dash;
		labelImport.BackgroundStyle.BorderBottomColor = Color.Gray;
		labelImport.BackgroundStyle.BorderBottomWidth = 1;
		labelImport.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)labelImport).Font = new Font("Microsoft Sans Serif", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)204);
		((Control)labelImport).Location = new Point(27, 27);
		((Control)labelImport).Name = "labelImport";
		((Control)labelImport).Size = new Size(343, 23);
		((Control)labelImport).TabIndex = 9;
		((Control)labelImport).Text = "Import";
		((Control)buttonVisualFoxPro).AccessibleRole = (AccessibleRole)43;
		buttonVisualFoxPro.AntiAlias = true;
		buttonVisualFoxPro.ColorTable = eButtonColor.Orange;
		buttonVisualFoxPro.Image = (Image)componentResourceManager.GetObject("buttonVisualFoxPro.Image");
		buttonVisualFoxPro.ImageTextSpacing = 6;
		buttonVisualFoxPro.Is20Button = false;
		((Control)buttonVisualFoxPro).Location = new Point(27, 490);
		((Control)buttonVisualFoxPro).Name = "buttonVisualFoxPro";
		((Control)buttonVisualFoxPro).Size = new Size(343, 42);
		buttonVisualFoxPro.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)buttonVisualFoxPro).TabIndex = 9;
		((Control)buttonVisualFoxPro).Text = "Visual FoxPro";
		buttonVisualFoxPro.TextAlignment = eButtonTextAlignment.Left;
		((Control)buttonVisualFoxPro).Click += ButtonVisualFoxPro_Click;
		((Control)buttonXtraReports).AccessibleRole = (AccessibleRole)43;
		buttonXtraReports.AntiAlias = true;
		buttonXtraReports.ColorTable = eButtonColor.Orange;
		buttonXtraReports.Image = (Image)componentResourceManager.GetObject("buttonXtraReports.Image");
		buttonXtraReports.ImageTextSpacing = 6;
		buttonXtraReports.Is20Button = false;
		((Control)buttonXtraReports).Location = new Point(27, 200);
		((Control)buttonXtraReports).Name = "buttonXtraReports";
		((Control)buttonXtraReports).Size = new Size(343, 42);
		buttonXtraReports.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)buttonXtraReports).TabIndex = 9;
		((Control)buttonXtraReports).Text = "DevExpress XtraReports";
		buttonXtraReports.TextAlignment = eButtonTextAlignment.Left;
		((Control)buttonXtraReports).Click += ButtonXtraReports_Click;
		((Control)buttonTelerikReporting).AccessibleRole = (AccessibleRole)43;
		buttonTelerikReporting.AntiAlias = true;
		buttonTelerikReporting.ColorTable = eButtonColor.Orange;
		buttonTelerikReporting.Image = (Image)componentResourceManager.GetObject("buttonTelerikReporting.Image");
		buttonTelerikReporting.ImageTextSpacing = 6;
		buttonTelerikReporting.Is20Button = false;
		((Control)buttonTelerikReporting).Location = new Point(27, 441);
		((Control)buttonTelerikReporting).Name = "buttonTelerikReporting";
		((Control)buttonTelerikReporting).Size = new Size(343, 42);
		buttonTelerikReporting.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)buttonTelerikReporting).TabIndex = 7;
		((Control)buttonTelerikReporting).Text = "Telerik Reporting";
		buttonTelerikReporting.TextAlignment = eButtonTextAlignment.Left;
		((Control)buttonTelerikReporting).Click += ButtonTelerikReporting_Click;
		((Control)buttonMSRS).AccessibleRole = (AccessibleRole)43;
		buttonMSRS.AntiAlias = true;
		buttonMSRS.ColorTable = eButtonColor.Orange;
		buttonMSRS.Image = (Image)componentResourceManager.GetObject("buttonMSRS.Image");
		buttonMSRS.ImageTextSpacing = 6;
		buttonMSRS.Is20Button = false;
		((Control)buttonMSRS).Location = new Point(27, 296);
		((Control)buttonMSRS).Name = "buttonMSRS";
		((Control)buttonMSRS).Size = new Size(343, 42);
		buttonMSRS.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)buttonMSRS).TabIndex = 4;
		((Control)buttonMSRS).Text = "Microsoft Reporting Service";
		buttonMSRS.TextAlignment = eButtonTextAlignment.Left;
		((Control)buttonMSRS).Click += ButtonMicrosoftReportingServices_Click;
		((Control)buttonRTF).AccessibleRole = (AccessibleRole)43;
		buttonRTF.AntiAlias = true;
		buttonRTF.ColorTable = eButtonColor.Orange;
		buttonRTF.Image = (Image)componentResourceManager.GetObject("buttonRTF.Image");
		buttonRTF.ImageTextSpacing = 6;
		buttonRTF.Is20Button = false;
		((Control)buttonRTF).Location = new Point(27, 392);
		((Control)buttonRTF).Name = "buttonRTF";
		((Control)buttonRTF).Size = new Size(343, 42);
		buttonRTF.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)buttonRTF).TabIndex = 6;
		((Control)buttonRTF).Text = "Rich Text (RTF)";
		buttonRTF.TextAlignment = eButtonTextAlignment.Left;
		((Control)buttonRTF).Click += ButtonRTF_Click;
		((Control)buttonReportSharpShooter).AccessibleRole = (AccessibleRole)43;
		buttonReportSharpShooter.AntiAlias = true;
		buttonReportSharpShooter.ColorTable = eButtonColor.Orange;
		buttonReportSharpShooter.Image = (Image)componentResourceManager.GetObject("buttonReportSharpShooter.Image");
		buttonReportSharpShooter.ImageTextSpacing = 6;
		buttonReportSharpShooter.Is20Button = false;
		((Control)buttonReportSharpShooter).Location = new Point(27, 344);
		((Control)buttonReportSharpShooter).Name = "buttonReportSharpShooter";
		((Control)buttonReportSharpShooter).Size = new Size(343, 42);
		buttonReportSharpShooter.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)buttonReportSharpShooter).TabIndex = 5;
		((Control)buttonReportSharpShooter).Text = "Report Sharp Shooter";
		buttonReportSharpShooter.TextAlignment = eButtonTextAlignment.Left;
		((Control)buttonReportSharpShooter).Click += ButtonReportSharpShooter_Click;
		((Control)buttonFastReport).AccessibleRole = (AccessibleRole)43;
		buttonFastReport.AntiAlias = true;
		buttonFastReport.ColorTable = eButtonColor.Orange;
		buttonFastReport.Image = (Image)componentResourceManager.GetObject("buttonFastReport.Image");
		buttonFastReport.ImageTextSpacing = 6;
		buttonFastReport.Is20Button = false;
		((Control)buttonFastReport).Location = new Point(27, 248);
		((Control)buttonFastReport).Name = "buttonFastReport";
		((Control)buttonFastReport).Size = new Size(343, 42);
		buttonFastReport.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)buttonFastReport).TabIndex = 3;
		((Control)buttonFastReport).Text = "Fast Reports";
		buttonFastReport.TextAlignment = eButtonTextAlignment.Left;
		((Control)buttonFastReport).Click += ButtonFastReport_Click;
		((Control)buttonCrystalReports).AccessibleRole = (AccessibleRole)43;
		buttonCrystalReports.AntiAlias = true;
		buttonCrystalReports.ColorTable = eButtonColor.Orange;
		buttonCrystalReports.Image = (Image)componentResourceManager.GetObject("buttonCrystalReports.Image");
		buttonCrystalReports.ImageTextSpacing = 6;
		buttonCrystalReports.Is20Button = false;
		((Control)buttonCrystalReports).Location = new Point(27, 152);
		((Control)buttonCrystalReports).Name = "buttonCrystalReports";
		((Control)buttonCrystalReports).Size = new Size(343, 42);
		buttonCrystalReports.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)buttonCrystalReports).TabIndex = 2;
		((Control)buttonCrystalReports).Text = "Crystal Reports";
		buttonCrystalReports.TextAlignment = eButtonTextAlignment.Left;
		((Control)buttonCrystalReports).Click += ButtonCrystalReports_Click;
		((Control)buttonComponentOne).AccessibleRole = (AccessibleRole)43;
		buttonComponentOne.AntiAlias = true;
		buttonComponentOne.ColorTable = eButtonColor.Orange;
		buttonComponentOne.Image = (Image)componentResourceManager.GetObject("buttonComponentOne.Image");
		buttonComponentOne.ImageTextSpacing = 6;
		buttonComponentOne.Is20Button = false;
		((Control)buttonComponentOne).Location = new Point(27, 103);
		((Control)buttonComponentOne).Name = "buttonComponentOne";
		((Control)buttonComponentOne).Size = new Size(343, 42);
		buttonComponentOne.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)buttonComponentOne).TabIndex = 2;
		((Control)buttonComponentOne).Text = "Component One Reports";
		buttonComponentOne.TextAlignment = eButtonTextAlignment.Left;
		((Control)buttonComponentOne).Click += ButtonComponentOne_Click;
		((Control)buttonActiveReports).AccessibleRole = (AccessibleRole)43;
		buttonActiveReports.AntiAlias = true;
		buttonActiveReports.ColorTable = eButtonColor.Orange;
		buttonActiveReports.Image = (Image)componentResourceManager.GetObject("buttonActiveReports.Image");
		buttonActiveReports.ImageTextSpacing = 5;
		buttonActiveReports.Is20Button = false;
		((Control)buttonActiveReports).Location = new Point(27, 56);
		((Control)buttonActiveReports).Name = "buttonActiveReports";
		((Control)buttonActiveReports).Size = new Size(343, 42);
		buttonActiveReports.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)buttonActiveReports).TabIndex = 1;
		((Control)buttonActiveReports).Text = "Active Reports";
		buttonActiveReports.TextAlignment = eButtonTextAlignment.Left;
		((Control)buttonActiveReports).Click += ButtonActiveReports_Click;
		((Control)pageCloud).Controls.Add((Control)(object)panelCloud);
		pageCloud.Dock = (DockStyle)5;
		pageCloud.Image = (Image)componentResourceManager.GetObject("pageCloud.Image");
		pageCloud.Invisible = true;
		pageCloud.InvisibleVisible = false;
		((Control)pageCloud).Location = new Point(253, 0);
		((Control)pageCloud).Name = "pageCloud";
		((Control)pageCloud).Padding = new Padding(1, 0, 0, 0);
		((Control)pageCloud).Size = new Size(636, 713);
		((Control)pageCloud).TabIndex = 2;
		((Control)pageCloud).Text = "Stimulsoft Cloud";
		((Control)pageCloud).Visible = false;
		((Control)panelCloud).BackColor = Color.White;
		((Control)panelCloud).Dock = (DockStyle)3;
		((Control)panelCloud).Location = new Point(1, 0);
		((Control)panelCloud).Name = "panelCloud";
		((Control)panelCloud).Size = new Size(398, 713);
		((Control)panelCloud).TabIndex = 7;
		pageOpen.Dock = (DockStyle)5;
		pageOpen.Image = (Image)componentResourceManager.GetObject("pageOpen.Image");
		pageOpen.InvisibleVisible = false;
		((Control)pageOpen).Location = new Point(169, 0);
		((Control)pageOpen).Name = "pageOpen";
		((Control)pageOpen).Padding = new Padding(1, 0, 0, 0);
		((Control)pageOpen).Size = new Size(739, 713);
		((Control)pageOpen).TabIndex = 2;
		((Control)pageOpen).Text = "Open";
		((Control)pageOpen).Visible = false;
		((Control)panel1).BackColor = Color.Transparent;
		((Control)panel1).Controls.Add((Control)(object)labelTitleOpen);
		((Control)panel1).Dock = (DockStyle)1;
		((Control)panel1).Location = new Point(40, 2);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(974, 100);
		((Control)panel1).TabIndex = 9;
		((Control)panelScheduleTitle).BackColor = Color.Transparent;
		((Control)panelScheduleTitle).Controls.Add((Control)(object)labelTitleSchedule);
		((Control)panelScheduleTitle).Dock = (DockStyle)1;
		((Control)panelScheduleTitle).Location = new Point(40, 2);
		((Control)panelScheduleTitle).Name = "panelScheduleTitle";
		((Control)panelScheduleTitle).Size = new Size(974, 100);
		((Control)panelScheduleTitle).TabIndex = 9;
		((Control)labelTitleOpen).BackColor = Color.Transparent;
		labelTitleOpen.BackgroundStyle.BorderBottom = eStyleBorderType.Dash;
		labelTitleOpen.BackgroundStyle.BorderBottomColor = Color.Gray;
		labelTitleOpen.BackgroundStyle.BorderBottomWidth = 1;
		labelTitleOpen.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)labelTitleOpen).Dock = (DockStyle)5;
		((Control)labelTitleOpen).Font = new Font("Microsoft Sans Serif", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)204);
		((Control)labelTitleOpen).Location = new Point(0, 0);
		((Control)labelTitleOpen).Name = "labelTitleOpen";
		((Control)labelTitleOpen).Size = new Size(974, 100);
		((Control)labelTitleOpen).TabIndex = 1;
		((Control)labelTitleOpen).Text = "Open";
		((Control)labelTitleSchedule).BackColor = Color.Transparent;
		labelTitleSchedule.BackgroundStyle.BorderBottom = eStyleBorderType.Dash;
		labelTitleSchedule.BackgroundStyle.BorderBottomColor = Color.Gray;
		labelTitleSchedule.BackgroundStyle.BorderBottomWidth = 1;
		labelTitleSchedule.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)labelTitleSchedule).Dock = (DockStyle)5;
		((Control)labelTitleSchedule).Font = new Font("Microsoft Sans Serif", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)204);
		((Control)labelTitleSchedule).Location = new Point(0, 0);
		((Control)labelTitleSchedule).Name = "labelTitleSchedule";
		((Control)labelTitleSchedule).Size = new Size(974, 100);
		((Control)labelTitleSchedule).TabIndex = 1;
		((Control)labelTitleSchedule).Text = "Schedule";
		((Control)labelTitleHelp).BackColor = Color.Transparent;
		labelTitleHelp.BackgroundStyle.BorderBottom = eStyleBorderType.Dash;
		labelTitleHelp.BackgroundStyle.BorderBottomColor = Color.Gray;
		labelTitleHelp.BackgroundStyle.BorderBottomWidth = 1;
		labelTitleHelp.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)labelTitleHelp).Dock = (DockStyle)5;
		((Control)labelTitleHelp).Font = new Font("Microsoft Sans Serif", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)204);
		((Control)labelTitleHelp).Location = new Point(0, 0);
		((Control)labelTitleHelp).Name = "labelTitleHelp";
		((Control)labelTitleHelp).Size = new Size(974, 100);
		((Control)labelTitleHelp).TabIndex = 1;
		((Control)labelTitleHelp).Text = "Help";
		buttonReportOpen.EndGroup = true;
		buttonReportOpen.AttachedControl = (Control)(object)superTabControlPanelOpen;
		buttonReportOpen.GlobalItem = false;
		buttonReportOpen.Name = "buttonReportOpen";
		buttonReportOpen.SelectedTabFont = new Font("Microsoft Sans Serif", 9f);
		buttonReportOpen.TabFont = new Font("Microsoft Sans Serif", 9f);
		buttonReportOpen.Text = "Open";
		superTabControlPanelNew.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		((Control)superTabControlPanelNew).Controls.Add((Control)(object)panelItems);
		((Control)superTabControlPanelNew).Controls.Add((Control)(object)panel3);
		((Control)superTabControlPanelNew).Dock = (DockStyle)5;
		((Control)superTabControlPanelNew).Location = new Point(99, 0);
		((Control)superTabControlPanelNew).Name = "superTabControlPanel1";
		((Control)superTabControlPanelNew).Padding = new Padding(40, 2, 2, 2);
		((Control)superTabControlPanelNew).Size = new Size(1016, 813);
		((Control)superTabControlPanelNew).TabIndex = 1;
		superTabControlPanelNew.TabItem = buttonNew;
		((ScrollableControl)panelItems).AutoScroll = true;
		((Control)panelItems).AutoSize = true;
		((Control)panelItems).BackColor = Color.White;
		((Control)panelItems).Controls.Add((Control)(object)formFlowItems);
		((Control)panelItems).Controls.Add((Control)(object)dashboardFlowItems);
		((Control)panelItems).Controls.Add((Control)(object)reportFlowItems);
		((Control)panelItems).Dock = (DockStyle)5;
		((Control)panelItems).Location = new Point(40, 102);
		((Control)panelItems).Name = "panelItems";
		((Control)panelItems).Size = new Size(974, 709);
		((Control)panelItems).TabIndex = 8;
		((Control)reportFlowItems).AutoSize = true;
		((Control)reportFlowItems).BackColor = Color.White;
		((Control)reportFlowItems).Dock = (DockStyle)1;
		((Control)reportFlowItems).Location = new Point(0, 0);
		((Control)reportFlowItems).MinimumSize = new Size(0, 10);
		((Control)reportFlowItems).Name = "reportFlowItems";
		((Control)reportFlowItems).Size = new Size(974, 10);
		((Control)reportFlowItems).TabIndex = 7;
		((Control)dashboardFlowItems).AutoSize = true;
		((Control)dashboardFlowItems).BackColor = Color.White;
		((Control)dashboardFlowItems).Dock = (DockStyle)1;
		((Control)dashboardFlowItems).Location = new Point(0, 0);
		((Control)dashboardFlowItems).MinimumSize = new Size(0, 10);
		((Control)dashboardFlowItems).Name = "dashboardFlowItems";
		((Control)dashboardFlowItems).Size = new Size(974, 10);
		((Control)dashboardFlowItems).TabIndex = 7;
		((Control)formFlowItems).AutoSize = true;
		((Control)formFlowItems).BackColor = Color.White;
		((Control)formFlowItems).Dock = (DockStyle)1;
		((Control)formFlowItems).Location = new Point(0, 0);
		((Control)formFlowItems).MinimumSize = new Size(0, 10);
		((Control)formFlowItems).Name = "formFlowItems";
		((Control)formFlowItems).Size = new Size(974, 10);
		((Control)formFlowItems).TabIndex = 7;
		((Control)panel3).BackColor = Color.Transparent;
		((Control)panel3).Controls.Add((Control)(object)labelTitleNew);
		((Control)panel3).Dock = (DockStyle)1;
		((Control)panel3).Location = new Point(40, 2);
		((Control)panel3).Name = "panel3";
		((Control)panel3).Size = new Size(974, 100);
		((Control)panel3).TabIndex = 9;
		((Control)labelTitleNew).BackColor = Color.Transparent;
		labelTitleNew.BackgroundStyle.BorderBottom = eStyleBorderType.Dash;
		labelTitleNew.BackgroundStyle.BorderBottomColor = Color.Gray;
		labelTitleNew.BackgroundStyle.BorderBottomWidth = 1;
		labelTitleNew.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)labelTitleNew).Dock = (DockStyle)5;
		((Control)labelTitleNew).Font = new Font("Microsoft Sans Serif", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)204);
		((Control)labelTitleNew).Location = new Point(0, 0);
		((Control)labelTitleNew).Name = "labelTitleNew";
		((Control)labelTitleNew).Size = new Size(974, 100);
		((Control)labelTitleNew).TabIndex = 3;
		((Control)labelTitleNew).Text = "New";
		buttonNew.AttachedControl = (Control)(object)superTabControlPanelNew;
		buttonNew.GlobalItem = false;
		buttonNew.Name = "buttonNew";
		buttonNew.SelectedTabFont = new Font("Microsoft Sans Serif", 9f);
		buttonNew.TabFont = new Font("Microsoft Sans Serif", 9f);
		buttonNew.Text = "New";
		buttonNew.MouseDown += new MouseEventHandler(ButtonNew_Click);
		superTabControlPanelInfo.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		((Control)superTabControlPanelInfo).Controls.Add((Control)(object)labelCheckIssues);
		((Control)superTabControlPanelInfo).Controls.Add((Control)(object)labelProtectDocument);
		((Control)superTabControlPanelInfo).Controls.Add((Control)(object)labelReportOptions);
		((Control)superTabControlPanelInfo).Controls.Add((Control)(object)labelCheckIssuesTitle);
		((Control)superTabControlPanelInfo).Controls.Add((Control)(object)labelProtectDocumentTitle);
		((Control)superTabControlPanelInfo).Controls.Add((Control)(object)labelReportOptionsTitle);
		((Control)superTabControlPanelInfo).Controls.Add((Control)(object)buttonCheckIssue);
		((Control)superTabControlPanelInfo).Controls.Add((Control)(object)buttonProtectDocument);
		((Control)superTabControlPanelInfo).Controls.Add((Control)(object)buttonReportOptions);
		((Control)superTabControlPanelInfo).Controls.Add((Control)(object)panel2);
		((Control)superTabControlPanelInfo).Dock = (DockStyle)5;
		((Control)superTabControlPanelInfo).Location = new Point(99, 0);
		((Control)superTabControlPanelInfo).Name = "superTabControlPanelInfo";
		((Control)superTabControlPanelInfo).Padding = new Padding(40, 2, 2, 2);
		((Control)superTabControlPanelInfo).Size = new Size(1016, 813);
		((Control)superTabControlPanelInfo).TabIndex = 1;
		superTabControlPanelInfo.TabItem = buttonInfo;
		((Control)labelCheckIssues).BackColor = Color.Transparent;
		labelCheckIssues.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)labelCheckIssues).Location = new Point(145, 375);
		((Control)labelCheckIssues).Name = "labelCheckIssues";
		((Control)labelCheckIssues).Size = new Size(626, 23);
		((Control)labelCheckIssues).TabIndex = 8;
		((Control)labelCheckIssues).Text = "labelX2";
		((Control)labelProtectDocument).BackColor = Color.Transparent;
		labelProtectDocument.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)labelProtectDocument).Location = new Point(145, 263);
		((Control)labelProtectDocument).Name = "labelProtectDocument";
		((Control)labelProtectDocument).Size = new Size(626, 19);
		((Control)labelProtectDocument).TabIndex = 8;
		((Control)labelProtectDocument).Text = "labelX2";
		((Control)labelReportOptions).BackColor = Color.Transparent;
		labelReportOptions.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)labelReportOptions).Location = new Point(145, 145);
		((Control)labelReportOptions).Name = "labelReportOptions";
		((Control)labelReportOptions).Size = new Size(626, 23);
		((Control)labelReportOptions).TabIndex = 8;
		((Control)labelReportOptions).Text = "labelX2";
		((Control)labelCheckIssuesTitle).BackColor = Color.Transparent;
		labelCheckIssuesTitle.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)labelCheckIssuesTitle).Font = new Font("Microsoft Sans Serif", 9f, (FontStyle)1);
		((Control)labelCheckIssuesTitle).Location = new Point(145, 350);
		((Control)labelCheckIssuesTitle).Name = "labelCheckIssuesTitle";
		((Control)labelCheckIssuesTitle).Size = new Size(626, 23);
		((Control)labelCheckIssuesTitle).TabIndex = 7;
		((Control)labelCheckIssuesTitle).Text = "labelX1";
		((Control)labelProtectDocumentTitle).BackColor = Color.Transparent;
		labelProtectDocumentTitle.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)labelProtectDocumentTitle).Font = new Font("Microsoft Sans Serif", 9f, (FontStyle)1);
		((Control)labelProtectDocumentTitle).Location = new Point(145, 238);
		((Control)labelProtectDocumentTitle).Name = "labelProtectDocumentTitle";
		((Control)labelProtectDocumentTitle).Size = new Size(626, 19);
		((Control)labelProtectDocumentTitle).TabIndex = 7;
		((Control)labelProtectDocumentTitle).Text = "labelX1";
		((Control)labelReportOptionsTitle).BackColor = Color.Transparent;
		labelReportOptionsTitle.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)labelReportOptionsTitle).Font = new Font("Microsoft Sans Serif", 9f, (FontStyle)1);
		((Control)labelReportOptionsTitle).Location = new Point(145, 120);
		((Control)labelReportOptionsTitle).Name = "labelReportOptionsTitle";
		((Control)labelReportOptionsTitle).Size = new Size(626, 23);
		((Control)labelReportOptionsTitle).TabIndex = 7;
		((Control)labelReportOptionsTitle).Text = "labelX1";
		((Control)buttonCheckIssue).BackColor = Color.White;
		((ButtonBase)buttonCheckIssue).FlatStyle = (FlatStyle)0;
		((Control)buttonCheckIssue).ForeColor = Color.FromArgb(68, 68, 68);
		((ButtonBase)buttonCheckIssue).Image = (Image)(object)Resources.CheckForIssues;
		((Control)buttonCheckIssue).Location = new Point(40, 331);
		((Control)buttonCheckIssue).Margin = new Padding(0);
		((Control)buttonCheckIssue).Name = "buttonCheckIssue";
		((Control)buttonCheckIssue).Size = new Size(94, 94);
		((Control)buttonCheckIssue).TabIndex = 6;
		((Control)buttonCheckIssue).Text = "Check for Issues";
		((ButtonBase)buttonCheckIssue).TextAlign = (ContentAlignment)512;
		((ButtonBase)buttonCheckIssue).TextImageRelation = (TextImageRelation)1;
		((ButtonBase)buttonCheckIssue).UseVisualStyleBackColor = false;
		((Control)buttonCheckIssue).Click += ButtonCheckIssues_Click;
		((Control)buttonProtectDocument).BackColor = Color.White;
		((ButtonBase)buttonProtectDocument).FlatStyle = (FlatStyle)0;
		((Control)buttonProtectDocument).ForeColor = Color.FromArgb(68, 68, 68);
		((ButtonBase)buttonProtectDocument).Image = (Image)(object)Resources.Encrypt;
		((Control)buttonProtectDocument).Location = new Point(40, 217);
		((Control)buttonProtectDocument).Margin = new Padding(0);
		((Control)buttonProtectDocument).Name = "buttonProtectDocument";
		((Control)buttonProtectDocument).Size = new Size(94, 94);
		((Control)buttonProtectDocument).TabIndex = 6;
		((Control)buttonProtectDocument).Text = "Encrypt with Password";
		((ButtonBase)buttonProtectDocument).TextAlign = (ContentAlignment)512;
		((ButtonBase)buttonProtectDocument).TextImageRelation = (TextImageRelation)1;
		((ButtonBase)buttonProtectDocument).UseVisualStyleBackColor = false;
		((Control)buttonProtectDocument).Click += ButtonProtectDocument_Click;
		((Control)buttonReportOptions).BackColor = Color.White;
		((ButtonBase)buttonReportOptions).FlatStyle = (FlatStyle)0;
		((Control)buttonReportOptions).ForeColor = Color.FromArgb(68, 68, 68);
		((Control)buttonReportOptions).Location = new Point(40, 103);
		((Control)buttonReportOptions).Margin = new Padding(0);
		((Control)buttonReportOptions).Name = "buttonReportOptions";
		((Control)buttonReportOptions).Size = new Size(94, 94);
		((Control)buttonReportOptions).TabIndex = 6;
		((Control)buttonReportOptions).Text = "Report Options";
		((ButtonBase)buttonReportOptions).TextAlign = (ContentAlignment)512;
		((ButtonBase)buttonReportOptions).TextImageRelation = (TextImageRelation)1;
		((ButtonBase)buttonReportOptions).UseVisualStyleBackColor = false;
		((Control)buttonReportOptions).Click += ButtonReportOptions_Click;
		((Control)panelHelp).BackColor = Color.Transparent;
		((Control)panelHelp).Controls.Add((Control)(object)labelTitleHelp);
		((Control)panelHelp).Dock = (DockStyle)1;
		((Control)panelHelp).Location = new Point(40, 2);
		((Control)panelHelp).Name = "panelHelp";
		((Control)panelHelp).Size = new Size(974, 100);
		((Control)panelHelp).TabIndex = 0;
		((Control)panel2).BackColor = Color.Transparent;
		((Control)panel2).Controls.Add((Control)(object)labelTitleInfo);
		((Control)panel2).Dock = (DockStyle)1;
		((Control)panel2).Location = new Point(40, 2);
		((Control)panel2).Name = "panel2";
		((Control)panel2).Size = new Size(974, 100);
		((Control)panel2).TabIndex = 0;
		((Control)labelTitleInfo).BackColor = Color.Transparent;
		labelTitleInfo.BackgroundStyle.BorderBottom = eStyleBorderType.Dash;
		labelTitleInfo.BackgroundStyle.BorderBottomColor = Color.Gray;
		labelTitleInfo.BackgroundStyle.BorderBottomWidth = 1;
		labelTitleInfo.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)labelTitleInfo).Dock = (DockStyle)5;
		((Control)labelTitleInfo).Font = new Font("Microsoft Sans Serif", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)204);
		((Control)labelTitleInfo).Location = new Point(0, 0);
		((Control)labelTitleInfo).Name = "labelTitleInfo";
		((Control)labelTitleInfo).Size = new Size(974, 100);
		((Control)labelTitleInfo).TabIndex = 2;
		((Control)labelTitleInfo).Text = "Info";
		buttonInfo.AttachedControl = (Control)(object)superTabControlPanelInfo;
		buttonInfo.GlobalItem = false;
		buttonInfo.Name = "buttonInfo";
		buttonInfo.SelectedTabFont = new Font("Microsoft Sans Serif", 9f);
		buttonInfo.TabFont = new Font("Microsoft Sans Serif", 9f);
		buttonInfo.Text = "Info";
		superTabControlPanelSaveAs.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		((Control)superTabControlPanelSaveAs).Controls.Add((Control)(object)tabControlSaveAs);
		((Control)superTabControlPanelSaveAs).Controls.Add((Control)(object)panelSaveAs);
		((Control)superTabControlPanelSaveAs).Dock = (DockStyle)5;
		((Control)superTabControlPanelSaveAs).Location = new Point(99, 0);
		((Control)superTabControlPanelSaveAs).Name = "superTabControlPanelSaveAs";
		((Control)superTabControlPanelSaveAs).Padding = new Padding(40, 2, 2, 2);
		((Control)superTabControlPanelSaveAs).Size = new Size(1016, 813);
		((Control)superTabControlPanelSaveAs).TabIndex = 1;
		superTabControlPanelSaveAs.TabItem = buttonReportSaveAs;
		((Control)tabControlSaveAs).Controls.Add((Control)(object)pageCloudSave);
		((Control)tabControlSaveAs).Controls.Add((Control)(object)pageSaveAs);
		((Control)tabControlSaveAs).Dock = (DockStyle)5;
		((Control)tabControlSaveAs).Location = new Point(40, 102);
		((Control)tabControlSaveAs).Name = "tabControlSaveAs";
		((Control)tabControlSaveAs).Padding = new Padding(253, 0, 0, 0);
		tabControlSaveAs.ScrollHeight = 0;
		tabControlSaveAs.ScrollWidth = 100;
		((Control)tabControlSaveAs).Size = new Size(974, 709);
		((Control)tabControlSaveAs).TabIndex = 2;
		tabControlSaveAs.TitleBackColor = Color.Transparent;
		tabControlSaveAs.TitleInLine = true;
		tabControlSaveAs.TitlePosition = StiTabTitlePosition.LeftHorizontal;
		tabControlSaveAs.TitleWidth = 250;
		((Control)tabControlHelp).Controls.Add((Control)(object)pageHelpDocumentation);
		((Control)tabControlHelp).Controls.Add((Control)(object)pageHelpSupport);
		((Control)tabControlHelp).Controls.Add((Control)(object)pageHelpVideos);
		((Control)tabControlHelp).Controls.Add((Control)(object)pageHelpSamples);
		((Control)tabControlHelp).Controls.Add((Control)(object)pageHelpForum);
		((Control)tabControlHelp).Controls.Add((Control)(object)pageHelpDemos);
		((Control)tabControlHelp).Controls.Add((Control)(object)pageHelpTrainingCourses);
		((Control)tabControlHelp).Dock = (DockStyle)5;
		((Control)tabControlHelp).Location = new Point(40, 102);
		((Control)tabControlHelp).Name = "tabControlHelp";
		((Control)tabControlHelp).Padding = new Padding(253, 0, 0, 0);
		tabControlHelp.ScrollHeight = 0;
		tabControlHelp.ScrollWidth = 100;
		((Control)tabControlHelp).Size = new Size(974, 709);
		((Control)tabControlHelp).TabIndex = 2;
		tabControlHelp.TitleBackColor = Color.Transparent;
		tabControlHelp.TitleInLine = true;
		tabControlHelp.TitlePosition = StiTabTitlePosition.LeftHorizontal;
		tabControlHelp.TitleWidth = 250;
		pageCloudSave.Dock = (DockStyle)5;
		pageCloudSave.Image = (Image)componentResourceManager.GetObject("pageCloudSave.Image");
		pageCloudSave.Invisible = true;
		pageCloudSave.InvisibleVisible = true;
		((Control)pageCloudSave).Location = new Point(253, 0);
		((Control)pageCloudSave).Name = "pageCloudSave";
		((Control)pageCloudSave).Padding = new Padding(1, 0, 1, 0);
		((Control)pageCloudSave).Size = new Size(721, 709);
		((Control)pageCloudSave).TabIndex = 0;
		((Control)pageCloudSave).Text = "Stimulsoft Cloud";
		((Control)pageCloudSave).Visible = false;
		pageCloudSave.ShowSelected = false;
		pageSaveAs.Dock = (DockStyle)5;
		pageSaveAs.Image = (Image)componentResourceManager.GetObject("pageSaveAs.Image");
		pageSaveAs.InvisibleVisible = false;
		((Control)pageSaveAs).Location = new Point(253, 0);
		((Control)pageSaveAs).Name = "pageSaveAs";
		((Control)pageSaveAs).Padding = new Padding(1, 0, 0, 0);
		((Control)pageSaveAs).Size = new Size(636, 713);
		((Control)pageSaveAs).TabIndex = 2;
		((Control)pageSaveAs).Text = "Save As";
		((Control)pageSaveAs).Visible = false;
		pageSaveAs.ShowSelected = false;
		pageHelpDocumentation.Dock = (DockStyle)5;
		pageHelpDocumentation.Image = (Image)componentResourceManager.GetObject("pageHelpDocumentation");
		pageHelpDocumentation.InvisibleVisible = false;
		((Control)pageHelpDocumentation).Location = new Point(253, 0);
		((Control)pageHelpDocumentation).Name = "pageHelpDocumentation";
		((Control)pageHelpDocumentation).Padding = new Padding(1, 0, 1, 0);
		((Control)pageHelpDocumentation).Size = new Size(721, 709);
		((Control)pageHelpDocumentation).TabIndex = 0;
		((Control)pageHelpDocumentation).Text = "Documentation";
		((Control)pageHelpDocumentation).Visible = true;
		pageHelpDocumentation.ShowSelected = false;
		pageHelpSupport.Dock = (DockStyle)5;
		pageHelpSupport.Image = (Image)componentResourceManager.GetObject("pageHelpSupport");
		pageHelpSupport.InvisibleVisible = false;
		((Control)pageHelpSupport).Location = new Point(253, 0);
		((Control)pageHelpSupport).Name = "pageHelpSupport";
		((Control)pageHelpSupport).Padding = new Padding(1, 0, 1, 0);
		((Control)pageHelpSupport).Size = new Size(721, 709);
		((Control)pageHelpSupport).TabIndex = 0;
		((Control)pageHelpSupport).Text = "Stimulsoft Support";
		((Control)pageHelpSupport).Visible = true;
		pageHelpSupport.ShowSelected = false;
		pageHelpSupport.ShowSeparator = true;
		pageHelpSeparator1.Dock = (DockStyle)5;
		pageHelpSeparator1.ShowSeparator = true;
		pageHelpSeparator1.InvisibleVisible = false;
		((Control)pageHelpSeparator1).Location = new Point(253, 0);
		((Control)pageHelpSeparator1).Name = "pageHelpSeparator1";
		((Control)pageHelpSeparator1).Padding = new Padding(1, 0, 1, 0);
		((Control)pageHelpSeparator1).Size = new Size(721, 709);
		((Control)pageHelpSeparator1).TabIndex = 0;
		((Control)pageHelpSeparator1).Text = "";
		((Control)pageHelpSeparator1).Visible = true;
		pageHelpSeparator1.ShowSelected = false;
		pageHelpVideos.Dock = (DockStyle)5;
		pageHelpVideos.Image = (Image)componentResourceManager.GetObject("pageHelpVideos");
		pageHelpVideos.InvisibleVisible = false;
		((Control)pageHelpVideos).Location = new Point(253, 0);
		((Control)pageHelpVideos).Name = "pageHelpVideos";
		((Control)pageHelpVideos).Padding = new Padding(1, 0, 1, 0);
		((Control)pageHelpVideos).Size = new Size(721, 709);
		((Control)pageHelpVideos).TabIndex = 0;
		((Control)pageHelpVideos).Text = "Videos";
		((Control)pageHelpVideos).Visible = true;
		pageHelpVideos.ShowSelected = false;
		pageHelpSamples.Dock = (DockStyle)5;
		pageHelpSamples.Image = (Image)componentResourceManager.GetObject("pageHelpSamples");
		pageHelpSamples.InvisibleVisible = false;
		((Control)pageHelpSamples).Location = new Point(253, 0);
		((Control)pageHelpSamples).Name = "pageHelpSamples";
		((Control)pageHelpSamples).Padding = new Padding(1, 0, 1, 0);
		((Control)pageHelpSamples).Size = new Size(721, 709);
		((Control)pageHelpSamples).TabIndex = 0;
		((Control)pageHelpSamples).Text = "Samples";
		((Control)pageHelpSamples).Visible = true;
		pageHelpSamples.ShowSelected = false;
		pageHelpSeparator2.Dock = (DockStyle)5;
		pageHelpSeparator2.ShowSeparator = true;
		pageHelpSeparator2.InvisibleVisible = false;
		((Control)pageHelpSeparator2).Location = new Point(253, 0);
		((Control)pageHelpSeparator2).Name = "pageHelpSeparator2";
		((Control)pageHelpSeparator2).Padding = new Padding(1, 0, 1, 0);
		((Control)pageHelpSeparator2).Size = new Size(721, 709);
		((Control)pageHelpSeparator2).TabIndex = 0;
		((Control)pageHelpSeparator2).Text = "";
		((Control)pageHelpSeparator2).Visible = true;
		pageHelpSeparator2.ShowSelected = false;
		pageHelpForum.Dock = (DockStyle)5;
		pageHelpForum.Image = (Image)componentResourceManager.GetObject("pageHelpForum");
		pageHelpForum.InvisibleVisible = false;
		((Control)pageHelpForum).Location = new Point(253, 0);
		((Control)pageHelpForum).Name = "pageHelpForum";
		((Control)pageHelpForum).Padding = new Padding(1, 0, 1, 0);
		((Control)pageHelpForum).Size = new Size(721, 709);
		((Control)pageHelpForum).TabIndex = 0;
		((Control)pageHelpForum).Text = "Forum";
		((Control)pageHelpForum).Visible = true;
		pageHelpForum.ShowSelected = false;
		pageHelpDemos.Dock = (DockStyle)5;
		pageHelpDemos.Image = (Image)componentResourceManager.GetObject("pageHelpDemos");
		pageHelpDemos.InvisibleVisible = false;
		((Control)pageHelpDemos).Location = new Point(253, 0);
		((Control)pageHelpDemos).Name = "pageHelpDemos";
		((Control)pageHelpDemos).Padding = new Padding(1, 0, 1, 0);
		((Control)pageHelpDemos).Size = new Size(721, 709);
		((Control)pageHelpDemos).TabIndex = 0;
		((Control)pageHelpDemos).Text = "Demos";
		((Control)pageHelpDemos).Visible = true;
		pageHelpDemos.ShowSelected = false;
		pageHelpTrainingCourses.Dock = (DockStyle)5;
		pageHelpTrainingCourses.Image = (Image)componentResourceManager.GetObject("pageHelpTrainingCourses");
		pageHelpTrainingCourses.InvisibleVisible = false;
		((Control)pageHelpTrainingCourses).Location = new Point(253, 0);
		((Control)pageHelpTrainingCourses).Name = "pageHelpTrainingCourses";
		((Control)pageHelpTrainingCourses).Padding = new Padding(1, 0, 1, 0);
		((Control)pageHelpTrainingCourses).Size = new Size(721, 709);
		((Control)pageHelpTrainingCourses).TabIndex = 0;
		((Control)pageHelpTrainingCourses).Text = "Training courses";
		((Control)pageHelpTrainingCourses).Visible = true;
		pageHelpTrainingCourses.ShowSelected = false;
		pageHelpGitHub.Dock = (DockStyle)5;
		pageHelpGitHub.Image = (Image)componentResourceManager.GetObject("pageCloudSave.Image");
		pageHelpGitHub.InvisibleVisible = false;
		((Control)pageHelpGitHub).Location = new Point(253, 0);
		((Control)pageHelpGitHub).Name = "pageHelpGitHub";
		((Control)pageHelpGitHub).Padding = new Padding(1, 0, 1, 0);
		((Control)pageHelpGitHub).Size = new Size(721, 709);
		((Control)pageHelpGitHub).TabIndex = 0;
		((Control)pageHelpGitHub).Text = "GitHub";
		((Control)pageHelpGitHub).Visible = true;
		pageHelpGitHub.ShowSelected = false;
		pageHelpNuget.Dock = (DockStyle)5;
		pageHelpNuget.Image = (Image)componentResourceManager.GetObject("pageCloudSave.Image");
		pageHelpNuget.InvisibleVisible = false;
		((Control)pageHelpNuget).Location = new Point(253, 0);
		((Control)pageHelpNuget).Name = "pageHelpNuget";
		((Control)pageHelpNuget).Padding = new Padding(1, 0, 1, 0);
		((Control)pageHelpNuget).Size = new Size(721, 709);
		((Control)pageHelpNuget).TabIndex = 0;
		((Control)pageHelpNuget).Text = "Nuget";
		((Control)pageHelpNuget).Visible = true;
		pageHelpNuget.ShowSelected = false;
		((Control)panelSaveAs).BackColor = Color.Transparent;
		((Control)panelSaveAs).Controls.Add((Control)(object)labelTitleSaveAs);
		((Control)panelSaveAs).Dock = (DockStyle)1;
		((Control)panelSaveAs).Location = new Point(40, 2);
		((Control)panelSaveAs).Name = "panelSaveAs";
		((Control)panelSaveAs).Size = new Size(974, 100);
		((Control)panelSaveAs).TabIndex = 9;
		((Control)labelTitleSaveAs).BackColor = Color.Transparent;
		labelTitleSaveAs.BackgroundStyle.BorderBottom = eStyleBorderType.Dash;
		labelTitleSaveAs.BackgroundStyle.BorderBottomColor = Color.Gray;
		labelTitleSaveAs.BackgroundStyle.BorderBottomWidth = 1;
		labelTitleSaveAs.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)labelTitleSaveAs).Dock = (DockStyle)5;
		((Control)labelTitleSaveAs).Font = new Font("Microsoft Sans Serif", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)204);
		((Control)labelTitleSaveAs).Location = new Point(0, 0);
		((Control)labelTitleSaveAs).Name = "labelTitleSaveAs";
		((Control)labelTitleSaveAs).Size = new Size(974, 100);
		((Control)labelTitleSaveAs).TabIndex = 1;
		((Control)labelTitleSaveAs).Text = "Save As";
		buttonReportSaveAs.AttachedControl = (Control)(object)superTabControlPanelSaveAs;
		buttonReportSaveAs.GlobalItem = false;
		buttonReportSaveAs.Name = "buttonReportSaveAs";
		buttonReportSaveAs.SelectedTabFont = new Font("Microsoft Sans Serif", 9f);
		buttonReportSaveAs.TabFont = new Font("Microsoft Sans Serif", 9f);
		buttonReportSaveAs.Text = "Save As";
		buttonReportSaveAs.MouseDown += new MouseEventHandler(ButtonReportSaveAs_MouseDown);
		labelBackSeparator.Height = 8;
		labelBackSeparator.Name = "labelBackSeparator";
		labelBackSeparator.Width = 50;
		buttonReportSave.GlobalItem = false;
		buttonReportSave.Name = "buttonReportSave";
		buttonReportSave.SelectedTabFont = new Font("Microsoft Sans Serif", 9f);
		buttonReportSave.TabFont = new Font("Microsoft Sans Serif", 9f);
		buttonReportSave.Text = "Save";
		buttonReportSave.MouseDown += new MouseEventHandler(ButtonReportSave_Click);
		buttonPreview.GlobalItem = false;
		buttonPreview.Name = "buttonPreview";
		buttonPreview.SelectedTabFont = new Font("Microsoft Sans Serif", 9f);
		buttonPreview.TabFont = new Font("Microsoft Sans Serif", 9f);
		buttonPreview.Text = "Preview";
		buttonPreview.MouseDown += new MouseEventHandler(ButtonPreview_Click);
		buttonSchedule.Name = "buttonScheduler";
		buttonSchedule.AttachedControl = (Control)(object)superTabControlPanelSchedule;
		buttonSchedule.GlobalItem = false;
		buttonSchedule.SelectedTabFont = new Font("Microsoft Sans Serif", 9f);
		buttonSchedule.TabFont = new Font("Microsoft Sans Serif", 9f);
		buttonSchedule.Text = "Scheduler";
		buttonSchedule.Visible = false;
		buttonSchedule.MouseDown += new MouseEventHandler(ButtonSchedule_MouseDown);
		buttonShare.GlobalItem = false;
		buttonShare.Name = "buttonShare";
		buttonShare.SelectedTabFont = new Font("Microsoft Sans Serif", 9f);
		buttonShare.TabFont = new Font("Microsoft Sans Serif", 9f);
		buttonShare.Text = "Share";
		buttonShare.Visible = false;
		buttonShare.MouseDown += new MouseEventHandler(ButtonShare_Click);
		buttonPublish.GlobalItem = false;
		buttonPublish.Name = "buttonPublish";
		buttonPublish.SelectedTabFont = new Font("Microsoft Sans Serif", 9f);
		buttonPublish.TabFont = new Font("Microsoft Sans Serif", 9f);
		buttonPublish.Text = "Publish";
		buttonPublish.Visible = false;
		buttonPublish.MouseDown += new MouseEventHandler(ButtonPublish_Click);
		buttonGetStarted.GlobalItem = false;
		buttonGetStarted.Name = "buttonGetStarted";
		buttonGetStarted.SelectedTabFont = new Font("Microsoft Sans Serif", 9f);
		buttonGetStarted.TabFont = new Font("Microsoft Sans Serif", 9f);
		buttonGetStarted.Text = "Get Started";
		buttonGetStarted.Visible = false;
		buttonGetStarted.MouseDown += new MouseEventHandler(ButtonGetStarted_Click);
		buttonClose.EndGroup = true;
		buttonClose.GlobalItem = false;
		buttonClose.Name = "buttonClose";
		buttonClose.SelectedTabFont = new Font("Microsoft Sans Serif", 9f);
		buttonClose.TabFont = new Font("Microsoft Sans Serif", 9f);
		buttonClose.Text = "Close";
		buttonClose.MouseDown += new MouseEventHandler(ButtonClose_Click);
		buttonOptions.GlobalItem = false;
		buttonOptions.Name = "buttonOptions";
		buttonOptions.SelectedTabFont = new Font("Microsoft Sans Serif", 9f);
		buttonOptions.TabFont = new Font("Microsoft Sans Serif", 9f);
		buttonOptions.Text = "Options";
		buttonOptions.MouseDown += new MouseEventHandler(ButtonOptions_Click);
		buttonAbout.GlobalItem = false;
		buttonAbout.Name = "buttonAbout";
		buttonAbout.SelectedTabFont = new Font("Microsoft Sans Serif", 9f);
		buttonAbout.ShowSubItems = false;
		buttonAbout.TabFont = new Font("Microsoft Sans Serif", 9f);
		buttonAbout.Text = "About";
		buttonAbout.MouseDown += new MouseEventHandler(ButtonAbout_Click);
		buttonHelp.AttachedControl = (Control)(object)superTabControlPanelHelp;
		buttonHelp.GlobalItem = false;
		buttonHelp.Name = "buttonHelp";
		buttonHelp.SelectedTabFont = new Font("Microsoft Sans Serif", 9f);
		buttonHelp.TabFont = new Font("Microsoft Sans Serif", 9f);
		buttonHelp.Text = "Help";
		((Control)labelRecentSeparator).BackColor = Color.White;
		labelRecentSeparator.BackgroundStyle.BorderBottomWidth = 1;
		labelRecentSeparator.BackgroundStyle.BorderLeft = eStyleBorderType.Dash;
		labelRecentSeparator.BackgroundStyle.BorderLeftColor = Color.Gray;
		labelRecentSeparator.BackgroundStyle.BorderLeftWidth = 1;
		labelRecentSeparator.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)labelRecentSeparator).Dock = (DockStyle)3;
		((Control)labelRecentSeparator).Font = new Font("Microsoft Sans Serif", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)204);
		((Control)labelRecentSeparator).Location = new Point(0, 0);
		((Control)labelRecentSeparator).Name = "labelRecentSeparator";
		((Control)labelRecentSeparator).Size = new Size(17, 687);
		((Control)labelRecentSeparator).TabIndex = 3;
		buttonAccount.AttachedControl = (Control)(object)superTabControlPanelAccount;
		buttonAccount.GlobalItem = false;
		buttonAccount.Name = "buttonAccount";
		buttonAccount.SelectedTabFont = new Font("Microsoft Sans Serif", 9f);
		buttonAccount.TabFont = new Font("Microsoft Sans Serif", 9f);
		buttonAccount.Text = "Account";
		buttonAccount.MouseDown += new MouseEventHandler(ButtonAccount_Click);
		superTabControlPanelAccount.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		((Control)superTabControlPanelAccount).Controls.Add((Control)(object)panelAccount);
		((Control)superTabControlPanelAccount).Controls.Add((Control)(object)panel4);
		((Control)superTabControlPanelAccount).Dock = (DockStyle)5;
		((Control)superTabControlPanelAccount).Location = new Point(105, 0);
		((Control)superTabControlPanelAccount).Name = "superTabControlPanelAccount";
		((Control)superTabControlPanelAccount).Padding = new Padding(40, 2, 2, 2);
		((Control)superTabControlPanelAccount).Size = new Size(1010, 813);
		((Control)superTabControlPanelAccount).TabIndex = 0;
		superTabControlPanelAccount.TabItem = buttonAccount;
		((Control)panel4).BackColor = Color.Transparent;
		((Control)panel4).Controls.Add((Control)(object)labelTitleAccount);
		((Control)panel4).Dock = (DockStyle)1;
		((Control)panel4).Location = new Point(40, 2);
		((Control)panel4).Name = "panel4";
		((Control)panel4).Size = new Size(968, 100);
		((Control)panel4).TabIndex = 10;
		((Control)labelTitleAccount).BackColor = Color.Transparent;
		labelTitleAccount.BackgroundStyle.BorderBottom = eStyleBorderType.Dash;
		labelTitleAccount.BackgroundStyle.BorderBottomColor = Color.Gray;
		labelTitleAccount.BackgroundStyle.BorderBottomWidth = 1;
		labelTitleAccount.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)labelTitleAccount).Dock = (DockStyle)5;
		((Control)labelTitleAccount).Font = new Font("Microsoft Sans Serif", 9f, (FontStyle)1, (GraphicsUnit)3, (byte)204);
		((Control)labelTitleAccount).Location = new Point(0, 0);
		((Control)labelTitleAccount).Name = "labelTitleAccount";
		((Control)labelTitleAccount).Size = new Size(968, 100);
		((Control)labelTitleAccount).TabIndex = 3;
		((Control)labelTitleAccount).Text = "Account";
		((ScrollableControl)panelAccount).AutoScroll = true;
		((Control)panelAccount).AutoSize = true;
		((Control)panelAccount).BackColor = Color.White;
		((Control)panelAccount).Dock = (DockStyle)5;
		((Control)panelAccount).Location = new Point(40, 102);
		((Control)panelAccount).Name = "panelAccount";
		((Control)panelAccount).Size = new Size(968, 709);
		((Control)panelAccount).TabIndex = 11;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)BackStage);
		((Control)this).Name = "StiRibbonBackStageUserControl";
		((Control)this).Size = new Size(1115, 813);
		((ISupportInitialize)BackStage).EndInit();
		((Control)BackStage).ResumeLayout(false);
		((Control)superTabControlPanelOpen).ResumeLayout(false);
		((Control)superTabControlPanelSchedule).ResumeLayout(false);
		((Control)superTabControlPanelHelp).ResumeLayout(false);
		((Control)tabControl).ResumeLayout(false);
		((Control)pageRecent).ResumeLayout(false);
		((Control)panel8).ResumeLayout(false);
		((Control)panel7).ResumeLayout(false);
		((Control)pageImport).ResumeLayout(false);
		((Control)panelImport).ResumeLayout(false);
		((Control)pageCloud).ResumeLayout(false);
		((Control)panel1).ResumeLayout(false);
		((Control)panelScheduleTitle).ResumeLayout(false);
		((Control)panelScheduleData).ResumeLayout(false);
		((Control)panelHelp).ResumeLayout(false);
		((Control)superTabControlPanelNew).ResumeLayout(false);
		((Control)superTabControlPanelNew).PerformLayout();
		((Control)panelItems).ResumeLayout(false);
		((Control)panelItems).PerformLayout();
		((Control)panel3).ResumeLayout(false);
		((Control)superTabControlPanelInfo).ResumeLayout(false);
		((Control)panel2).ResumeLayout(false);
		((Control)superTabControlPanelSaveAs).ResumeLayout(false);
		((Control)tabControlSaveAs).ResumeLayout(false);
		((Control)tabControlHelp).ResumeLayout(false);
		((Control)panelSaveAs).ResumeLayout(false);
		((Control)superTabControlPanelAccount).ResumeLayout(false);
		((Control)superTabControlPanelAccount).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
