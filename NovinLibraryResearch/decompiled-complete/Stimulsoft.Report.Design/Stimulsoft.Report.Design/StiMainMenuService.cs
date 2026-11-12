using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.App;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design.Check;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Design.Panels;
using Stimulsoft.Report.Design.Toolbars;

namespace Stimulsoft.Report.Design;

public class StiMainMenuService : StiDesignerInitService
{
	private Hashtable menuItemToRecentFilesIndex = new Hashtable();

	public sealed override string ServiceCategory => StiLocalization.Get("Services", "categoryDesigner");

	public sealed override Type ServiceType => typeof(StiMainMenuService);

	internal bool PasteUndoRedoSave { get; set; } = true;


	protected Hashtable MenuItems { get; set; } = new Hashtable();


	protected List<ButtonItem> CreatedMenuItems { get; set; } = new List<ButtonItem>();


	[StiServiceParam]
	[Browsable(false)]
	[Obsolete("'MainMenuType' property is obsolete.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DefaultValue(StiMainMenuType.Enhanced)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public StiMainMenuType MainMenuType { get; set; } = StiMainMenuType.Enhanced;


	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowFile
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowFile;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowFile = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowFileReportNew
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowFileReportNew;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowFileReportNew = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowFilePageNew
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowFilePageNew;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowFilePageNew = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowFileFormNew
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowFileFormNew;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowFileFormNew = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowFileDashboardNew
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowFileDashboardNew;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowFileDashboardNew = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowFileScreenNew
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowFileScreenNew;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowFileScreenNew = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowFileReportOpen
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowFileReportOpen;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowFileReportOpen = value;
		}
	}

	[Obsolete("Field 'ShowFileReportOpenFromGoogleDocs' is not used more!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShowFileReportOpenFromGoogleDocs
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowFilePageOpen
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowFilePageOpen;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowFilePageOpen = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowFileReportSave
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowFileReportSave;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowFileReportSave = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowFileReportSaveAs
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowFileReportSaveAs;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowFileReportSaveAs = value;
		}
	}

	[Obsolete("Field 'ShowFileReportSaveAsToGoogleDocs' is not used more!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShowFileReportSaveAsToGoogleDocs
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowFilePageSaveAs
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowFilePageSaveAs;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowFilePageSaveAs = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowFilePageDelete
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowFilePageDelete;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowFilePageDelete = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowFileReportPreview
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowFileReportPreview;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowFileReportPreview = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowFilePageSetup
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowFilePageSetup;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowFilePageSetup = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowFileReportSetup
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowFileReportSetup;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowFileReportSetup = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowSelectLanguage
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowSelectLanguage;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowSelectLanguage = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowFileRecentFiles
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowFileRecentFiles;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowFileRecentFiles = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowFileExit
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowFileExit;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowFileExit = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowEdit
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowEdit;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowEdit = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowEditUndo
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowEditUndo;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowEditUndo = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowEditRedo
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowEditRedo;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowEditRedo = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowEditCut
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowEditCut;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowEditCut = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowEditCopy
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowEditCopy;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowEditCopy = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowEditPaste
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowEditPaste;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowEditPaste = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowEditDelete
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowEditDelete;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowEditDelete = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowEditSelectAll
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowEditSelectAll;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowEditSelectAll = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowEditClearContents
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowEditClearContents;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowEditClearContents = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowView
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowView;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowView = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowViewNormal
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowViewNormal;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowViewNormal = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowViewPageBreakPreview
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowViewPageBreakPreview;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowViewPageBreakPreview = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowViewShowGrid
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowViewShowGrid;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowViewShowGrid = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowViewAlignToGrid
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowViewAlignToGrid;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowViewAlignToGrid = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowViewShowHeaders
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowViewShowHeaders;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowViewShowHeaders = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowViewShowRulers
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowViewShowRulers;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowViewShowRulers = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowViewShowOrder
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowViewShowOrder;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowViewShowOrder = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowViewToolbars
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowViewToolbars;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowViewToolbars = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowViewPanels
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowViewPanels;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowViewPanels = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowTools
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowTools;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowTools = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowToolsDataStore
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowToolsDataStore;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowToolsDataStore = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowToolsPagesManager
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowToolsPagesManager;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowToolsPagesManager = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowToolsServicesConfigurator
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowToolsServicesConfigurator;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowToolsServicesConfigurator = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowToolsOptions
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowToolsOptions;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowToolsOptions = value;
		}
	}

	[Browsable(false)]
	public Form AboutForm { get; set; }

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowHelp
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowHelp;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowHelp = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue("http://stimulsoft.com/ReportsNet.aspx")]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public string ProductHomePage
	{
		get
		{
			return StiOptions.Designer.MainMenu.ProductHomePage;
		}
		set
		{
			StiOptions.Designer.MainMenu.ProductHomePage = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue("http://www.stimulsoft.com/ReportsNetFAQ.aspx")]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public string FaqPage
	{
		get
		{
			return StiOptions.Designer.MainMenu.FaqPage;
		}
		set
		{
			StiOptions.Designer.MainMenu.FaqPage = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue("mailto:support@stimulsoft.com")]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public string Support
	{
		get
		{
			return StiOptions.Designer.MainMenu.Support;
		}
		set
		{
			StiOptions.Designer.MainMenu.Support = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue("http://stimulsoft.com/support.aspx")]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public string SupportPage
	{
		get
		{
			return StiOptions.Designer.MainMenu.SupportPage;
		}
		set
		{
			StiOptions.Designer.MainMenu.SupportPage = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowHelpProductHomePage
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowHelpProductHomePage;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowHelpProductHomePage = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowHelpFaqPage
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowHelpFaqPage;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowHelpFaqPage = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowHelpSupport
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowHelpSupport;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowHelpSupport = value;
		}
	}

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	[Obsolete("Form 'Tip of the Day' is not used anymore.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShowHelpTipOfTheDay { get; set; } = true;


	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	public bool ShowHelpAboutProgramm
	{
		get
		{
			return StiOptions.Designer.MainMenu.ShowHelpAboutProgramm;
		}
		set
		{
			StiOptions.Designer.MainMenu.ShowHelpAboutProgramm = value;
		}
	}

	public override object Clone()
	{
		StiMainMenuService stiMainMenuService = base.Clone() as StiMainMenuService;
		stiMainMenuService.MenuItems = new Hashtable();
		stiMainMenuService.CreatedMenuItems = new List<ButtonItem>();
		return stiMainMenuService;
	}

	public virtual void RefreshToolbars()
	{
		if (!ShowViewToolbars || !ShowView)
		{
			return;
		}
		StiService[] toolbarServices = StiToolbarService.GetToolbarServices(Designer);
		StiService[] array = toolbarServices;
		for (int i = 0; i < array.Length; i++)
		{
			StiToolbarService stiToolbarService = (StiToolbarService)array[i];
			if (stiToolbarService.ServiceEnabled && stiToolbarService.ToolBar != null && !(stiToolbarService is StiToolboxToolbarService))
			{
				SetMenuChecked("ToolBar" + stiToolbarService.ServiceName, stiToolbarService.ToolBar.Visible);
			}
		}
	}

	public virtual void RefreshPanels()
	{
		if (ShowViewPanels && ShowView)
		{
			StiService[] panelServices = StiPanelService.GetPanelServices(Designer);
			StiService[] array = panelServices;
			for (int i = 0; i < array.Length; i++)
			{
				StiPanelService stiPanelService = (StiPanelService)array[i];
				string itemID = stiPanelService.GetType().ToString();
				SetMenuChecked(itemID, stiPanelService.DockingItem.Visible);
				SetAutoCheckOnClick(itemID, value: true);
				SetAutoCollapseOnClick(itemID, value: false);
			}
		}
	}

	public virtual void RefreshEditMenu()
	{
		if (Designer.TabControl.SelectedTabIndex >= Designer.PageCodeIndex || Designer.PageView.GetCurrentPageView() == null || !((Control)Designer.PageView.GetCurrentPageView().View).Focused)
		{
			if (ShowEditCut)
			{
				SetMenuEnabled("miEditCut", enabledValue: false);
			}
			if (ShowEditCopy)
			{
				SetMenuEnabled("miEditCopy", enabledValue: false);
			}
			if (ShowEditPaste)
			{
				SetMenuEnabled("miEditPaste", enabledValue: false);
			}
			if (ShowEditDelete)
			{
				SetMenuEnabled("miEditDelete", enabledValue: false);
			}
			if (ShowEditSelectAll)
			{
				SetMenuEnabled("miEditSelectAll", enabledValue: false);
			}
		}
		else
		{
			if (ShowEditCut)
			{
				SetMenuEnabled("miEditCut", StiActions.Clipboard.States.CanCut(Designer));
			}
			if (ShowEditCopy)
			{
				SetMenuEnabled("miEditCopy", StiActions.Clipboard.States.CanCopy(Designer));
			}
			if (ShowEditPaste)
			{
				SetMenuEnabled("miEditPaste", StiActions.Clipboard.States.CanPaste(Designer));
			}
			if (ShowEditDelete)
			{
				SetMenuEnabled("miEditDelete", StiActions.Clipboard.States.CanDelete(Designer));
			}
			if (ShowEditSelectAll)
			{
				SetMenuEnabled("miEditSelectAll", StiActions.Clipboard.States.CanSelectAll(Designer));
			}
		}
		SetMenuEnabled("miEditClearContents", Report != null && Report.IsPageDesigner);
		if (ShowFilePageSetup)
		{
			if (Report != null && Designer.IsFormSelected)
			{
				SetMenuEnabled("miFilePageSetup", enabledValue: false);
			}
			else if (Report != null && Designer.IsDashboardSelected)
			{
				SetMenuEnabled("miFilePageSetup", enabledValue: false);
			}
			else
			{
				SetMenuEnabled("miFilePageSetup", Report != null);
			}
		}
		if (Designer.TabControl.SelectedTabIndex >= Designer.PageCodeIndex)
		{
			if (ShowEditUndo)
			{
				SetMenuEnabled("miEditUndo", enabledValue: false);
			}
			if (ShowEditRedo)
			{
				SetMenuEnabled("miEditRedo", enabledValue: false);
			}
		}
		else
		{
			if (ShowEditUndo)
			{
				SetMenuEnabled("miEditUndo", StiActions.Clipboard.States.CanUndo(Designer));
			}
			if (ShowEditRedo)
			{
				SetMenuEnabled("miEditRedo", StiActions.Clipboard.States.CanRedo(Designer));
			}
			if (ShowEditUndo && Designer.TabControl.SelectedTabIndex < Designer.PageCodeIndex)
			{
				if (Designer.UndoRedo != null && Designer.UndoRedo.CanUndo)
				{
					SetMenuText("miEditUndo", Loc.Get("MainMenu", "menuEditUndo") + " " + Designer.UndoRedo.UndoText);
					SetMenuVisible("miEditUndo", visibleValue: false);
					SetMenuVisible("miEditUndo", visibleValue: true);
				}
				else
				{
					SetMenuText("miEditUndo", Loc.Get("MainMenu", "menuEditCantUndo"));
				}
			}
			if (ShowEditRedo && Designer.TabControl.SelectedTabIndex < Designer.PageCodeIndex)
			{
				if (Designer.UndoRedo != null && Designer.UndoRedo.CanRedo)
				{
					SetMenuText("miEditRedo", Loc.Get("MainMenu", "menuEditRedo") + " " + Designer.UndoRedo.RedoText);
					SetMenuVisible("miEditRedo", visibleValue: false);
					SetMenuVisible("miEditRedo", visibleValue: true);
				}
				else
				{
					SetMenuText("miEditRedo", Loc.Get("MainMenu", "menuEditCantRedo"));
				}
			}
		}
		string text = Designer.ReportFileName;
		if (!StiOptions.Designer.ShowFullFileNameInTitle && text.IndexOf("\\", StringComparison.InvariantCulture) != -1)
		{
			text = text.Substring(text.LastIndexOf("\\", StringComparison.InvariantCulture) + 1);
		}
		Form val = ((Control)Designer).FindForm();
		if (val != null && (val is StiDesigner || val is StiRibbonDesigner))
		{
			((Control)val).Text = StiOptions.Designer.GetDesignerTitleWithMask(text);
		}
	}

	public virtual void RefreshRecentFiles()
	{
		int num = ((Designer.RecentFiles != null) ? Designer.RecentFiles.Count : 0);
		if (!ShowFileRecentFiles)
		{
			num = 0;
		}
		if (num == 0)
		{
			SetMenuVisible("miFileRecentFileSeparator", visibleValue: false);
			for (int i = 0; i < 15; i++)
			{
				SetMenuVisible($"miFileRecentFile{i}", visibleValue: false);
			}
		}
		else
		{
			SetMenuVisible("miFileRecentFileSeparator", visibleValue: true);
		}
		if (Designer.RecentFiles == null)
		{
			return;
		}
		int num2 = 0;
		foreach (StiRecentFile recentFile in Designer.RecentFiles)
		{
			string text = num2.ToString();
			StiDisplayRecentFileObjectEventArgs stiDisplayRecentFileObjectEventArgs = StiOptions.Engine.GlobalEvents.InvokeDisplayRecentFileInDesigner(Designer, recentFile);
			string text2 = ((num2 < 9) ? $"&{num2 + 1} {stiDisplayRecentFileObjectEventArgs.DisplayName}" : stiDisplayRecentFileObjectEventArgs.DisplayName);
			SetMenuText("miFileRecentFile" + text, text2);
			SetMenuToolTip("miFileRecentFile" + text, stiDisplayRecentFileObjectEventArgs.FileName);
			SetMenuVisible("miFileRecentFile" + text, stiDisplayRecentFileObjectEventArgs.Visible && !string.IsNullOrEmpty(stiDisplayRecentFileObjectEventArgs.FileName));
			SetMenuTag("miFileRecentFile" + text, recentFile);
			num2++;
		}
		for (int j = num2; j < 15; j++)
		{
			SetMenuVisible("miFileRecentFile" + j, visibleValue: false);
		}
	}

	public virtual void RefreshReportOptions()
	{
		if (Report != null)
		{
			if (ShowViewShowGrid)
			{
				SetMenuChecked("miViewShowGrid", Report.Info.ShowGrid);
			}
			if (ShowViewAlignToGrid)
			{
				SetMenuChecked("miViewAlignToGrid", Report.Info.AlignToGrid);
			}
			if (ShowViewShowHeaders)
			{
				SetMenuChecked("miViewShowHeaders", Report.Info.ShowHeaders);
			}
			if (ShowViewShowRulers)
			{
				SetMenuChecked("miViewShowRulers", Report.Info.ShowRulers);
			}
			if (ShowViewShowOrder)
			{
				SetMenuChecked("miViewShowOrder", Report.Info.ShowOrder);
			}
			if (ShowViewNormal)
			{
				SetMenuChecked("miViewNormal", Designer.Report.Info.ViewMode == StiViewMode.Normal);
			}
			if (ShowViewPageBreakPreview)
			{
				SetMenuChecked("miViewPageBreakPreview", Designer.Report.Info.ViewMode == StiViewMode.PageBreakPreview);
			}
			SetMenuEnabled("miViewShowGrid", enabledValue: true);
			SetMenuEnabled("miViewAlignToGrid", enabledValue: true);
			SetMenuEnabled("miViewShowHeaders", enabledValue: true);
			SetMenuEnabled("miViewShowRulers", enabledValue: true);
			SetMenuEnabled("miViewShowOrder", enabledValue: true);
			SetMenuEnabled("miViewNormal", enabledValue: true);
			SetMenuEnabled("miViewPageBreakPreview", enabledValue: true);
		}
		else
		{
			SetMenuEnabled("miViewShowGrid", enabledValue: false);
			SetMenuEnabled("miViewAlignToGrid", enabledValue: false);
			SetMenuEnabled("miViewShowHeaders", enabledValue: false);
			SetMenuEnabled("miViewShowRulers", enabledValue: false);
			SetMenuEnabled("miViewShowOrder", enabledValue: false);
			SetMenuEnabled("miViewNormal", enabledValue: false);
			SetMenuEnabled("miViewPageBreakPreview", enabledValue: false);
		}
	}

	private void Designer_RefreshDesigner(object sender, EventArgs e)
	{
		RefreshEditMenu();
		RefreshRecentFiles();
		RefreshReportOptions();
		SetMenuEnabled("miFilePageDelete", Report != null && Report.Pages.Count > 1);
		if (Report != null && Report.IsPageDesigner)
		{
			SetMenuEnabled("miFileRecentFile1", enabledValue: false);
			SetMenuEnabled("miFileRecentFile2", enabledValue: false);
			SetMenuEnabled("miFileRecentFile3", enabledValue: false);
			SetMenuEnabled("miFileRecentFile4", enabledValue: false);
			SetMenuEnabled("miFileReportPreview", enabledValue: false);
			SetMenuEnabled("miFileReportNew", enabledValue: false);
			SetMenuEnabled("miFileReportOpen", enabledValue: false);
			SetMenuEnabled("miFileReportSave", enabledValue: false);
			SetMenuEnabled("miFileReportSaveAs", enabledValue: false);
			SetMenuEnabled("miFileReportSetup", enabledValue: false);
			SetMenuEnabled("miFilePageDelete", enabledValue: false);
			SetMenuEnabled("miFilePageNew", enabledValue: false);
			SetMenuEnabled("miFileFormNew", enabledValue: false);
			SetMenuEnabled("miFileDashboardNew", enabledValue: false);
		}
		if (Report != null && !Report.IsPageDesigner)
		{
			SetMenuEnabled("miFilePageNew", enabledValue: true);
			SetMenuEnabled("miFileFormNew", enabledValue: true);
			SetMenuEnabled("miFileDashboardNew", enabledValue: true);
			SetMenuEnabled("miFileReportPreview", enabledValue: true);
			SetMenuEnabled("miFilePageOpen", enabledValue: true);
			SetMenuEnabled("miFileReportSave", enabledValue: true);
			SetMenuEnabled("miFileReportSaveAs", enabledValue: true);
			SetMenuEnabled("miFilePageSaveAs", enabledValue: true);
			SetMenuEnabled("miFileReportSetup", enabledValue: true);
		}
		else
		{
			SetMenuEnabled("miFilePageNew", enabledValue: false);
			SetMenuEnabled("miFileFormNew", enabledValue: false);
			SetMenuEnabled("miFileDashboardNew", enabledValue: false);
			SetMenuEnabled("miFileReportPreview", enabledValue: false);
			SetMenuEnabled("miFilePageOpen", enabledValue: false);
			SetMenuEnabled("miFileReportSave", enabledValue: false);
			SetMenuEnabled("miFileReportSaveAs", enabledValue: false);
			SetMenuEnabled("miFilePageSaveAs", enabledValue: false);
			SetMenuEnabled("miFileReportSetup", enabledValue: false);
		}
		if (Report != null && Report.IsPageDesigner)
		{
			SetMenuEnabled("miToolsDataStore", enabledValue: false);
			SetMenuEnabled("miToolsPagesManager", enabledValue: false);
			SetMenuEnabled("miToolsServicesConfigurator", enabledValue: false);
			SetMenuEnabled("miToolsOptions", enabledValue: false);
		}
		else
		{
			SetMenuEnabled("miToolsDataStore", Report != null);
			SetMenuEnabled("miToolsPagesManager", Report != null);
			SetMenuEnabled("miToolsServicesConfigurator", enabledValue: true);
			SetMenuEnabled("miToolsOptions", enabledValue: true);
		}
		SetMenuEnabled("miFileSelectLanguage", Report != null);
	}

	private void ItemReportNew_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokeReportNew(Designer);
	}

	private void ItemPageNew_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokePageNew(Designer);
	}

	private void ItemFormNew_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokeFormNew(Designer);
	}

	private void ItemScreenNew_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokeScreenNew(Designer);
	}

	private void ItemDashboardNew_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokeDashboardNew(Designer);
	}

	private void ItemReportOpen_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokeReportOpen(Designer);
	}

	private void ItemPageOpen_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokePageOpen(Designer);
	}

	private void ItemReportSave_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokeReportSave(Designer);
	}

	private void ItemReportSaveAs_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokeReportSaveAs(Designer);
	}

	private void ItemPageSaveAs_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokePageSaveAs(Designer);
	}

	private void ItemPageDelete_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokePageDelete(Designer);
	}

	private void ItemReportPreview_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokeReportStandardGuiPreview(Designer);
	}

	private void ItemCheckIssues_Click(object sender, EventArgs e)
	{
		StiReportChecker.RunReportChecker(Designer.Report, checkKeyEscape: true);
	}

	private void ItemPageSetup_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokePageSetup(Designer);
	}

	private void ItemReportSetup_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokeReportSetup(Designer);
	}

	private void ItemSelectLanguage_Click(object sender, EventArgs e)
	{
		ButtonItem buttonItem = sender as ButtonItem;
		SetLocalization(buttonItem.Tag as string);
		ButtonItem buttonItem2 = sender as ButtonItem;
		buttonItem2.Checked = true;
	}

	private async void ItemRecentFile_Click(object sender, EventArgs e)
	{
		try
		{
			int index = (int)menuItemToRecentFilesIndex[sender];
			await StiActions.MainMenu.InvokeRecentFileAsync(Designer, Designer.RecentFiles[index]);
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ItemExit_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokeExit(Designer);
	}

	private void ItemUndo_Click(object sender, EventArgs e)
	{
		StiActions.Clipboard.InvokeUndo(Designer);
	}

	private void ItemRedo_Click(object sender, EventArgs e)
	{
		StiActions.Clipboard.InvokeRedo(Designer);
	}

	private void ItemCut_Click(object sender, EventArgs e)
	{
		StiActions.Clipboard.InvokeCut(Designer);
	}

	private void ItemCopy_Click(object sender, EventArgs e)
	{
		StiActions.Clipboard.InvokeCopy(Designer);
	}

	private void ItemPaste_Click(object sender, EventArgs e)
	{
		StiActions.Clipboard.InvokePaste(Designer);
	}

	private void ItemDelete_Click(object sender, EventArgs e)
	{
		StiActions.Clipboard.InvokeDelete(Designer);
	}

	private void ItemSelectAll_Click(object sender, EventArgs e)
	{
		StiActions.Clipboard.InvokeSelectAll(Designer);
	}

	private void ItemClearContents_Click(object sender, EventArgs e)
	{
		StiActions.Font.InvokeClearContents(Designer);
	}

	private void ItemNormal_Click(object sender, EventArgs e)
	{
		StiActions.ViewOptions.InvokeNormal(Designer);
	}

	private void ItemPageBreakPreview_Click(object sender, EventArgs e)
	{
		StiActions.ViewOptions.InvokePageBreakPreview(Designer);
	}

	private void ItemShowGrid_Click(object sender, EventArgs e)
	{
		StiActions.ViewOptions.InvokeShowGrid(Designer);
	}

	private void ItemAlignToGrid_Click(object sender, EventArgs e)
	{
		StiActions.ViewOptions.InvokeAlignToGrid(Designer);
	}

	private void ItemShowHeaders_Click(object sender, EventArgs e)
	{
		StiActions.ViewOptions.InvokeShowHeaders(Designer);
	}

	private void ItemShowRulers_Click(object sender, EventArgs e)
	{
		StiActions.ViewOptions.InvokeShowRulers(Designer);
	}

	private void ItemShowOrder_Click(object sender, EventArgs e)
	{
		StiActions.ViewOptions.InvokeShowOrder(Designer);
	}

	private void ItemToolbarSelect_Click(object sender, EventArgs e)
	{
		try
		{
			StiToolbarService stiToolbarService = GetMenuTag(sender) as StiToolbarService;
			stiToolbarService.ToolBar.Visible = !stiToolbarService.ToolBar.Visible;
			Designer.SaveDesignerParam();
			((Control)Designer).Refresh();
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ItemPanelSelect_Click(object sender, EventArgs e)
	{
		try
		{
			ButtonItem buttonItem = sender as ButtonItem;
			PanelSelect(GetMenuTag(sender) as StiPanelService, !buttonItem.IsMouseOver);
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ItemDataStore_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokeDataStore(Designer);
	}

	private void ItemPagesManager_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokePagesManager(Designer);
	}

	private void ItemServicesConfigurator_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokeServicesConfigurator(Designer);
	}

	private void ItemOptions_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokeOptions(Designer);
	}

	private void ItemAbout_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokeAbout(Designer);
	}

	private void ItemProductHomePage_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokeProductHomePage(Designer);
	}

	private void ItemSupport_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokeSupportPage(Designer);
	}

	private void ItemHelpContents_Click(object sender, EventArgs e)
	{
		StiOptions.Designer.CustomHelp.InvokeRunCustomHelp(Designer);
	}

	private void ItemFAQPage_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokeFAQPage(Designer);
	}

	protected virtual void InitMenuItems()
	{
		Designer.MainMenu.Items.Clear();
		MenuItems.Clear();
	}

	internal ButtonItem GetMenuItem(string itemID)
	{
		ButtonItem buttonItem = MenuItems[itemID] as ButtonItem;
		if (buttonItem == null)
		{
			buttonItem = new ButtonItem(itemID);
			CreatedMenuItems.Add(buttonItem);
			MenuItems[itemID] = buttonItem;
		}
		return buttonItem;
	}

	public virtual ButtonItem AddRootMenuItem(string rootID, string text)
	{
		ButtonItem menuItem = GetMenuItem(rootID);
		menuItem.Text = text;
		Bar mainMenu = Designer.MainMenu;
		mainMenu.Items.Add(menuItem);
		return menuItem;
	}

	public virtual ButtonItem AddMenuItem(string rootID, string itemID, string text, EventHandler handler, eShortcut shortcut)
	{
		return AddMenuItem(rootID, itemID, text, string.Empty, handler, shortcut);
	}

	public virtual ButtonItem AddMenuItem(string rootID, string itemID, string text, string imageName, EventHandler handler)
	{
		return AddMenuItem(rootID, itemID, text, imageName, handler, eShortcut.None);
	}

	public virtual ButtonItem AddMenuItem(string rootID, string itemID, string text, Image image, EventHandler handler)
	{
		return AddMenuItem(rootID, itemID, text, image, handler, eShortcut.None);
	}

	public virtual ButtonItem AddMenuItem(string rootID, string itemID, string text, string imageName, EventHandler handler, eShortcut shortcut)
	{
		Image image = null;
		if (!string.IsNullOrEmpty(imageName))
		{
			image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", imageName);
		}
		return AddMenuItem(rootID, itemID, text, image, handler, shortcut);
	}

	public virtual ButtonItem AddMenuItem(string rootID, string itemID, string text, Image image, EventHandler handler, eShortcut shortcut)
	{
		ButtonItem menuItem = GetMenuItem(rootID);
		ButtonItem menuItem2 = GetMenuItem(itemID);
		menuItem2.Text = text;
		menuItem2.Image = image;
		if (shortcut != 0)
		{
			menuItem2.Shortcuts.Add(shortcut);
		}
		menuItem2.Click += handler;
		menuItem.SubItems.Add(menuItem2);
		return menuItem2;
	}

	public virtual void SetMenuBeginGroup(string itemID)
	{
		ButtonItem menuItem = GetMenuItem(itemID);
		menuItem.BeginGroup = true;
	}

	public virtual void SetMenuChecked(string itemID, bool checkedValue)
	{
		ButtonItem menuItem = GetMenuItem(itemID);
		if (menuItem.Checked != checkedValue)
		{
			menuItem.Checked = checkedValue;
		}
	}

	public virtual void SetAutoCollapseOnClick(string itemID, bool value)
	{
		ButtonItem menuItem = GetMenuItem(itemID);
		menuItem.AutoCollapseOnClick = value;
	}

	public virtual void SetAutoCheckOnClick(string itemID, bool value)
	{
		ButtonItem menuItem = GetMenuItem(itemID);
		menuItem.AutoCheckOnClick = value;
	}

	public virtual void SetMenuTag(string itemID, object value)
	{
		ButtonItem menuItem = GetMenuItem(itemID);
		menuItem.Tag = value;
	}

	public virtual object GetMenuTag(object item)
	{
		ButtonItem buttonItem = item as ButtonItem;
		return buttonItem.Tag;
	}

	public virtual void SetMenuEnabled(string itemID, bool enabledValue)
	{
		ButtonItem menuItem = GetMenuItem(itemID);
		if (menuItem.Enabled != enabledValue)
		{
			menuItem.Enabled = enabledValue;
		}
	}

	public virtual void SetMenuVisible(string itemID, bool visibleValue)
	{
		ButtonItem menuItem = GetMenuItem(itemID);
		if (menuItem.Visible != visibleValue)
		{
			menuItem.Visible = visibleValue;
		}
	}

	public virtual void SetMenuText(string itemID, string text)
	{
		ButtonItem menuItem = GetMenuItem(itemID);
		if (menuItem.Text != text)
		{
			menuItem.Text = text;
		}
	}

	public virtual void SetMenuImage(string itemID, Image image)
	{
		ButtonItem menuItem = GetMenuItem(itemID);
		if (menuItem.Image != image)
		{
			menuItem.Image = image;
		}
	}

	public virtual void SetMenuToolTip(string itemID, string toolTip)
	{
		ButtonItem menuItem = GetMenuItem(itemID);
		if (menuItem.Tooltip != toolTip)
		{
			menuItem.Tooltip = toolTip;
		}
	}

	public virtual void ClearMenuItems(string itemID)
	{
		ButtonItem buttonItem = (ButtonItem)MenuItems[itemID];
		buttonItem.SubItems.Clear();
	}

	private void ViewMenuItem_PopupOpen(object sender, PopupOpenEventArgs e)
	{
		RefreshToolbars();
		RefreshPanels();
	}

	internal void RegNewReport(StiReport report)
	{
		StiActions.MainMenu.InvokeRegNewReport(Designer, report);
	}

	public void SetLocalization(string localizationFile)
	{
		if (string.IsNullOrEmpty(localizationFile))
		{
			StiLocalization.Localization = string.Empty;
			StiSettings.Load();
			StiSettings.Set("Localization", "FileName", string.Empty);
			StiSettings.Save();
			UpdateLocalization();
			return;
		}
		string directory = StiOptions.Localization.GetDirectory();
		localizationFile = Path.GetFileName(localizationFile);
		StiLocalization.GetParam(localizationFile, out var language, out var _, out var _, directory);
		if (!string.IsNullOrEmpty(language))
		{
			StiLocalization.Localization = localizationFile;
			StiSettings.Load();
			StiSettings.Set("Localization", "FileName", localizationFile);
			StiSettings.Save();
			UpdateLocalization();
		}
	}

	internal void PanelSelect(StiPanelService service, bool onlyActivate, bool panelFocus = true)
	{
		if (service.DockingItem.Visible)
		{
			if (service is StiPropertiesPanelService && onlyActivate)
			{
				if (service.DockingBar.Items.Contains(service.DockingItem))
				{
					service.DockingBar.SelectedDockContainerItem = service.DockingItem;
				}
				if (panelFocus)
				{
					((Control)((StiPropertiesPanelService)service).PropertyGrid).Focus();
				}
			}
			else
			{
				StiDesignerControl.DockPanelClose(service);
			}
		}
		else
		{
			StiDesignerControl.DockPanelOpen(service);
		}
		Designer.SaveDesignerParam();
		((Control)Designer).Refresh();
	}

	private void CreatePanels()
	{
		if (!ShowViewPanels)
		{
			return;
		}
		StiServiceContainer services = Designer.Services.GetServices(typeof(StiPanelService));
		if (services.Count <= 0)
		{
			return;
		}
		int num = 0;
		foreach (StiPanelService item in services)
		{
			Image val = null;
			string text = item.GetType().ToString() + "Menu";
			AddMenuItem(image: (Image)((item is StiDictionaryPanelService) ? StiReportDesignImages.Actions.Dictionary() : ((item is StiReportTreePanelService) ? StiReportDesignImages.Actions.ReportTree() : ((!(item is StiPropertiesPanelService)) ? ((object)(StiServiceBitmapAttribute.GetImage(item.GetType()) ?? StiServiceBitmapAttribute.GetImage(typeof(StiDockingPanel)))) : ((object)StiReportDesignImages.Actions.Properties())))), rootID: "miView", itemID: text, text: item.ServiceName, handler: ItemPanelSelect_Click, shortcut: item.Shortcut);
			SetMenuTag(text, item);
			if (num == 0)
			{
				SetMenuBeginGroup(text);
			}
			num++;
		}
	}

	public virtual void CreateToolbars()
	{
		ButtonItem menuItem = GetMenuItem("miViewToolbars");
		menuItem.SubItems.Clear();
		StiService[] toolbarServices = StiToolbarService.GetToolbarServices(Designer);
		StiService[] array = toolbarServices;
		for (int i = 0; i < array.Length; i++)
		{
			StiToolbarService stiToolbarService = (StiToolbarService)array[i];
			if (stiToolbarService.ServiceEnabled && stiToolbarService.ToolBar != null)
			{
				string itemID = "ToolBar" + stiToolbarService.ServiceName;
				AddMenuItem("miViewToolbars", itemID, stiToolbarService.ServiceName, string.Empty, ItemToolbarSelect_Click);
				SetMenuTag(itemID, stiToolbarService);
				SetMenuChecked(itemID, stiToolbarService.ToolBar.Visible);
				SetAutoCollapseOnClick(itemID, value: false);
				SetAutoCheckOnClick(itemID, value: true);
			}
		}
	}

	public static StiMainMenuService GetService()
	{
		return GetService(null);
	}

	public static StiMainMenuService GetService(StiDesignerControl designer)
	{
		if (designer != null && designer.Services != null)
		{
			return designer.Services.GetService(typeof(StiMainMenuService), getEnabled: false) as StiMainMenuService;
		}
		return StiDesignerOptions.MainMenu.FirstOrDefault();
	}

	public virtual void InvokeReportNew()
	{
		ItemReportNew_Click(null, EventArgs.Empty);
	}

	public virtual void InvokeReportBlankReportNew()
	{
		StiActions.MainMenu.InvokeReportNewBlank(Designer);
	}

	public virtual void InvokePageNew()
	{
		ItemPageNew_Click(null, EventArgs.Empty);
	}

	public virtual void InvokeFormNew()
	{
		ItemFormNew_Click(null, EventArgs.Empty);
	}

	public virtual void InvokeScreenNew()
	{
		ItemScreenNew_Click(null, EventArgs.Empty);
	}

	public virtual void InvokeDashboardNew()
	{
		ItemDashboardNew_Click(null, EventArgs.Empty);
	}

	public virtual bool InvokeReportOpen()
	{
		return StiActions.MainMenu.InvokeReportOpen(Designer);
	}

	public virtual bool InvokeReportSave()
	{
		return StiActions.MainMenu.InvokeReportSave(Designer);
	}

	public virtual bool InvokeReportSaveAs()
	{
		return StiActions.MainMenu.InvokeReportSaveAs(Designer);
	}

	public virtual void InvokePageSaveAs()
	{
		ItemPageSaveAs_Click(null, EventArgs.Empty);
	}

	public virtual void InvokePageOpen()
	{
		ItemPageOpen_Click(null, EventArgs.Empty);
	}

	public virtual void InvokePageDelete()
	{
		ItemPageDelete_Click(null, EventArgs.Empty);
	}

	public virtual void InvokeReportPreview()
	{
		ItemReportPreview_Click(null, EventArgs.Empty);
	}

	public virtual void InvokePageSetup()
	{
		ItemPageSetup_Click(null, EventArgs.Empty);
	}

	public virtual void InvokeReportSetup()
	{
		ItemReportSetup_Click(null, EventArgs.Empty);
	}

	public virtual void InvokeUndo()
	{
		((Control)Designer.PageView.GetCurrentPageView()).Focus();
		ItemUndo_Click(null, EventArgs.Empty);
	}

	public virtual void InvokeRedo()
	{
		((Control)Designer.PageView.GetCurrentPageView()).Focus();
		ItemRedo_Click(null, EventArgs.Empty);
	}

	public virtual void InvokeCut()
	{
		((Control)Designer.PageView.GetCurrentPageView()).Focus();
		ItemCut_Click(null, EventArgs.Empty);
	}

	public virtual void InvokeCopy()
	{
		((Control)Designer.PageView.GetCurrentPageView()).Focus();
		ItemCopy_Click(null, EventArgs.Empty);
	}

	public virtual void InvokePaste()
	{
		((Control)Designer.PageView.GetCurrentPageView()).Focus();
		ItemPaste_Click(null, EventArgs.Empty);
	}

	public void InvokePaste(bool undoRedoSave)
	{
		PasteUndoRedoSave = undoRedoSave;
		ItemPaste_Click(null, EventArgs.Empty);
		PasteUndoRedoSave = true;
	}

	public virtual void InvokeDelete()
	{
		((Control)Designer.PageView.GetCurrentPageView()).Focus();
		ItemDelete_Click(null, EventArgs.Empty);
	}

	public virtual void InvokeSelectAll()
	{
		((Control)Designer.PageView.GetCurrentPageView()).Focus();
		ItemSelectAll_Click(null, EventArgs.Empty);
	}

	public virtual void InvokeClearContents()
	{
		ItemClearContents_Click(null, EventArgs.Empty);
	}

	public virtual void InvokeShowGrid()
	{
		ItemShowGrid_Click(null, EventArgs.Empty);
	}

	public virtual void InvokeAlignToGrid()
	{
		ItemAlignToGrid_Click(null, EventArgs.Empty);
	}

	public virtual void InvokeShowHeaders()
	{
		ItemShowHeaders_Click(null, EventArgs.Empty);
	}

	public virtual void InvokeShowRulers()
	{
		ItemShowRulers_Click(null, EventArgs.Empty);
	}

	public virtual void InvokeShowOrder()
	{
		ItemShowOrder_Click(null, EventArgs.Empty);
	}

	public virtual void InvokeDataStore()
	{
		ItemDataStore_Click(null, EventArgs.Empty);
	}

	public virtual void InvokePagesManager()
	{
		ItemPagesManager_Click(null, EventArgs.Empty);
	}

	public void InvokeServicesConfigurator()
	{
		ItemServicesConfigurator_Click(null, EventArgs.Empty);
	}

	public virtual void InvokeOptions()
	{
		ItemOptions_Click(null, EventArgs.Empty);
	}

	public virtual void InvokeAbout()
	{
		ItemAbout_Click(null, EventArgs.Empty);
	}

	private void UpdateLocalization()
	{
		Designer.LocalizeDesigner();
		ButtonItem menuItem = GetMenuItem("miFileSelectLanguage");
		foreach (ButtonItem subItem in menuItem.SubItems)
		{
			subItem.Checked = false;
		}
	}

	private void BuildSelectLanguageMenuItem()
	{
		ClearMenuItems("miFileSelectLanguage");
		try
		{
			AddMenuItem("miFileSelectLanguage", "DefaultLanguage", StiLocalization.Get("FormOptions", "Default"), ItemSelectLanguage_Click, eShortcut.None);
			SetMenuChecked("DefaultLanguage", StiLocalization.Localization == string.Empty);
			SetAutoCheckOnClick("DefaultLanguage", value: true);
			string directory = StiOptions.Localization.GetDirectory();
			DirectoryInfo directoryInfo = new DirectoryInfo(directory);
			if (!directoryInfo.Exists)
			{
				return;
			}
			FileInfo[] files = directoryInfo.GetFiles("*.xml");
			int num = 0;
			FileInfo[] array = files;
			foreach (FileInfo fileInfo in array)
			{
				if (fileInfo.Name.Contains(".ext") || fileInfo.Name.Contains("server."))
				{
					continue;
				}
				StiLocalization.GetParam(fileInfo.FullName, out var language, out var description, out var _, directory);
				if (!string.IsNullOrEmpty(language))
				{
					string text = "Language" + description;
					AddMenuItem("miFileSelectLanguage", text, description, ItemSelectLanguage_Click, eShortcut.None);
					SetMenuChecked(text, fileInfo.Name == StiLocalization.Localization);
					SetAutoCheckOnClick(text, value: true);
					SetMenuTag(text, fileInfo.Name);
					if (num == 0)
					{
						SetMenuBeginGroup(text);
					}
					num++;
				}
			}
		}
		catch
		{
		}
	}

	public override void Localize()
	{
		base.Localize();
		SetMenuText("miFile", StiLocalization.Get("MainMenu", "menuFile"));
		SetMenuText("miFileReportNew", StiLocalization.Get("MainMenu", "menuFileReportNew"));
		SetMenuText("miFilePageNew", StiLocalization.Get("MainMenu", "menuFilePageNew"));
		SetMenuText("miFileFormNew", StiLocalization.Get("MainMenu", "menuFileFormNew"));
		SetMenuText("miFileDashboardNew", StiLocalization.Get("MainMenu", "menuFileDashboardNew"));
		SetMenuText("miFileReportOpen", StiLocalization.Get("MainMenu", "menuFileReportOpen"));
		SetMenuText("miFilePageOpen", StiLocalization.Get("MainMenu", "menuFilePageOpen"));
		SetMenuText("miFileReportSave", StiLocalization.Get("MainMenu", "menuFileReportSave"));
		SetMenuText("miFileReportSaveAs", StiLocalization.Get("MainMenu", "menuFileReportSaveAs"));
		SetMenuText("miFilePageSaveAs", StiLocalization.Get("MainMenu", "menuFilePageSaveAs"));
		SetMenuText("miFilePageDelete", StiLocalization.Get("MainMenu", "menuFilePageDelete"));
		SetMenuText("miFileReportPreview", StiLocalization.Get("MainMenu", "menuFileReportPreview"));
		SetMenuText("miFileReportSetup", StiLocalization.Get("MainMenu", "menuFileReportSetup"));
		SetMenuText("miFilePageSetup", StiLocalization.Get("MainMenu", "menuFilePageSetup"));
		SetMenuText("miFileSelectLanguage", StiLocalization.Get("FormOptions", "SelectUILanguage"));
		SetMenuText("miFileExit", StiLocalization.Get("MainMenu", "menuFileExit"));
		SetMenuText("miEdit", StiLocalization.Get("MainMenu", "menuEdit"));
		SetMenuText("miEditUndo", StiLocalization.Get("MainMenu", "menuEditUndo"));
		SetMenuText("miEditRedo", StiLocalization.Get("MainMenu", "menuEditRedo"));
		SetMenuText("miEditCut", StiLocalization.Get("MainMenu", "menuEditCut"));
		SetMenuText("miEditCopy", StiLocalization.Get("MainMenu", "menuEditCopy"));
		SetMenuText("miEditPaste", StiLocalization.Get("MainMenu", "menuEditPaste"));
		SetMenuText("miEditDelete", StiLocalization.Get("MainMenu", "menuEditDelete"));
		SetMenuText("miEditSelectAll", StiLocalization.Get("MainMenu", "menuEditSelectAll"));
		SetMenuText("miEditClearContents", StiLocalization.Get("MainMenu", "menuEditClearContents"));
		SetMenuText("miView", StiLocalization.Get("MainMenu", "menuView"));
		SetMenuText("miViewNormal", StiLocalization.Get("MainMenu", "menuViewNormal"));
		SetMenuText("miViewPageBreakPreview", StiLocalization.Get("MainMenu", "menuViewPageBreakPreview"));
		SetMenuText("miViewShowGrid", StiLocalization.Get("MainMenu", "menuViewShowGrid"));
		SetMenuText("miViewAlignToGrid", StiLocalization.Get("MainMenu", "menuViewAlignToGrid"));
		SetMenuText("miViewShowHeaders", StiLocalization.Get("MainMenu", "menuViewShowHeaders"));
		SetMenuText("miViewShowOrder", StiLocalization.Get("MainMenu", "menuViewShowOrder"));
		SetMenuText("miViewShowRulers", StiLocalization.Get("MainMenu", "menuViewShowRulers"));
		SetMenuText("miViewToolbars", StiLocalization.Get("MainMenu", "menuViewToolbars"));
		SetMenuText("miTools", StiLocalization.Get("MainMenu", "menuTools"));
		SetMenuText("miToolsDataStore", StiLocalization.Get("MainMenu", "menuToolsDataStore"));
		SetMenuText("miToolsPagesManager", StiLocalization.Get("MainMenu", "menuToolsPagesManager"));
		SetMenuText("miToolsServicesConfigurator", StiLocalization.Get("MainMenu", "menuToolsServicesConfigurator"));
		SetMenuText("miToolsOptions", StiLocalization.Get("MainMenu", "menuToolsOptions"));
		SetMenuText("miHelp", StiLocalization.Get("MainMenu", "menuHelp"));
		SetMenuText("miHelpFAQPage", StiLocalization.Get("MainMenu", "menuHelpFAQPage"));
		SetMenuText("miHelpHowToRegister", StiLocalization.Get("MainMenu", "menuHelpHowToRegister"));
		SetMenuText("miHelpSupport", StiLocalization.Get("MainMenu", "menuHelpSupport"));
		SetMenuText("miHelpProductHomePage", StiLocalization.Get("MainMenu", "menuHelpProductHomePage"));
		SetMenuText("miHelpAboutProgramm", StiLocalization.Get("MainMenu", "menuHelpAboutProgramm"));
		LocalizePanels();
		LocalizeToolbars();
		CreateToolbars();
	}

	private void LocalizePanels()
	{
		if (ShowViewPanels && ShowView)
		{
			StiService[] panelServices = StiPanelService.GetPanelServices(Designer);
			StiService[] array = panelServices;
			for (int i = 0; i < array.Length; i++)
			{
				StiPanelService stiPanelService = (StiPanelService)array[i];
				string itemID = stiPanelService.GetType().ToString();
				SetMenuText(itemID, stiPanelService.ServiceName);
			}
		}
	}

	private void LocalizeToolbars()
	{
		if (ShowViewPanels && ShowView)
		{
			StiService[] toolbarServices = StiToolbarService.GetToolbarServices(Designer);
			StiService[] array = toolbarServices;
			for (int i = 0; i < array.Length; i++)
			{
				StiToolbarService stiToolbarService = (StiToolbarService)array[i];
				string itemID = stiToolbarService.GetType().ToString();
				SetMenuText(itemID, stiToolbarService.ServiceInfo);
			}
		}
	}

	public override void Init(StiDesignerControl designer)
	{
		if (StiOptions.Configuration.IsWeb || !StiOptions.Engine.FullTrust)
		{
			return;
		}
		base.Init(designer);
		InitMenuItems();
		if (!designer.IsRibbonGuiMode && ShowFile)
		{
			AddRootMenuItem("miFile", StiLocalization.Get("MainMenu", "menuFile"));
			if (ShowFileReportNew || ShowFilePageNew || ShowFileFormNew || (ShowFileDashboardNew && StiDashboardAssembly.IsAssemblyLoaded))
			{
				if (ShowFileReportNew)
				{
					AddMenuItem("miFile", "miFileReportNew", StiLocalization.Get("MainMenu", "menuFileReportNew"), StiReportDesignImages.Actions.NewFile(), ItemReportNew_Click, eShortcut.CtrlN);
				}
				if (ShowFilePageNew)
				{
					AddMenuItem("miFile", "miFilePageNew", StiLocalization.Get("MainMenu", "menuFilePageNew"), StiReportDesignImages.Actions.NewPage(), ItemPageNew_Click, eShortcut.CtrlShiftN);
					SetMenuBeginGroup("miFilePageNew");
				}
				if (ShowFileScreenNew && StiAppAssembly.IsAssemblyLoaded)
				{
					AddMenuItem("miFile", "miFileScreenNew", StiLocalization.Get("MainMenu", "menuFileScreenNew"), StiReportDesignImages.Actions.NewScreen(), ItemScreenNew_Click, eShortcut.CtrlShiftF);
				}
				bool @bool = StiSettings.GetBool("Designer", "ShowDialogFormsV1", nullValue: false);
				if (ShowFileFormNew && @bool)
				{
					AddMenuItem("miFile", "miFileFormNew", StiLocalization.Get("MainMenu", "menuFileFormNew"), StiReportDesignImages.Actions.NewForm(), ItemFormNew_Click);
				}
				if (ShowFileDashboardNew && StiDashboardAssembly.IsAssemblyLoaded)
				{
					AddMenuItem("miFile", "miFileDashboardNew", StiLocalization.Get("MainMenu", "menuFileDashboardNew"), StiReportDesignImages.Actions.NewDashboard(), ItemDashboardNew_Click, eShortcut.CtrlShiftD);
				}
			}
			if (ShowFileReportOpen || ShowFilePageOpen)
			{
				if (ShowFileReportOpen)
				{
					AddMenuItem("miFile", "miFileReportOpen", StiLocalization.Get("MainMenu", "menuFileReportOpen"), StiReportDesignImages.Actions.OpenFile(), ItemReportOpen_Click, eShortcut.CtrlO);
					SetMenuBeginGroup("miFileReportOpen");
				}
				if (ShowFilePageOpen)
				{
					AddMenuItem("miFile", "miFilePageOpen", StiLocalization.Get("MainMenu", "menuFilePageOpen"), ItemPageOpen_Click, eShortcut.CtrlShiftO);
					if (!ShowFileReportOpen)
					{
						SetMenuBeginGroup("miFilePageOpen");
					}
				}
			}
			if (ShowFileReportSave || ShowFileReportSaveAs || ShowFilePageSaveAs)
			{
				if (ShowFileReportSave)
				{
					AddMenuItem("miFile", "miFileReportSave", StiLocalization.Get("MainMenu", "menuFileReportSave"), StiReportDesignImages.Actions.SaveFile(), ItemReportSave_Click, eShortcut.CtrlS);
					SetMenuBeginGroup("miFileReportSave");
				}
				if (ShowFileReportSaveAs)
				{
					AddMenuItem("miFile", "miFileReportSaveAs", StiLocalization.Get("MainMenu", "menuFileReportSaveAs"), ItemReportSaveAs_Click, eShortcut.F12);
					if (!ShowFileReportSave)
					{
						SetMenuBeginGroup("miFileReportSaveAs");
					}
				}
				if (ShowFilePageSaveAs)
				{
					AddMenuItem("miFile", "miFilePageSaveAs", StiLocalization.Get("MainMenu", "menuFilePageSaveAs"), string.Empty, ItemPageSaveAs_Click);
					if (!ShowFileReportSave && !ShowFileReportSaveAs)
					{
						SetMenuBeginGroup("miFilePageSaveAs");
					}
				}
			}
			if (ShowFilePageDelete)
			{
				AddMenuItem("miFile", "miFilePageDelete", StiLocalization.Get("MainMenu", "menuFilePageDelete"), StiReportDesignImages.Actions.Remove(), ItemPageDelete_Click);
				SetMenuBeginGroup("miFilePageDelete");
			}
			if (ShowFileReportPreview && designer.IsViewerPresent)
			{
				AddMenuItem("miFile", "miFileReportPreview", StiLocalization.Get("MainMenu", "menuFileReportPreview"), StiReportDesignImages.Actions.Preview(), ItemReportPreview_Click, eShortcut.F5);
				SetMenuBeginGroup("miFileReportPreview");
			}
			AddMenuItem("miFile", "biCheckIssues", StiLocalization.Get("MainMenu", "menuCheckIssues"), string.Empty, ItemCheckIssues_Click);
			if (ShowFileReportSetup || ShowFilePageSetup)
			{
				if (ShowFileReportSetup)
				{
					AddMenuItem("miFile", "miFileReportSetup", StiLocalization.Get("MainMenu", "menuFileReportSetup"), "", ItemReportSetup_Click);
					SetMenuBeginGroup("miFileReportSetup");
				}
				if (ShowFilePageSetup)
				{
					AddMenuItem("miFile", "miFilePageSetup", StiLocalization.Get("MainMenu", "menuFilePageSetup"), StiReportDesignImages.Actions.SetupPage(), ItemPageSetup_Click);
					if (!ShowFileReportSetup)
					{
						SetMenuBeginGroup("miFilePageSetup");
					}
				}
			}
			if (ShowSelectLanguage)
			{
				AddMenuItem("miFile", "miFileSelectLanguage", StiLocalization.Get("FormOptions", "SelectUILanguage"), "", null);
				BuildSelectLanguageMenuItem();
				SetMenuBeginGroup("miFileSelectLanguage");
			}
			if (ShowFileRecentFiles)
			{
				for (int i = 0; i < 15; i++)
				{
					ButtonItem key = AddMenuItem("miFile", "miFileRecentFile" + i, "Recent" + i, "", ItemRecentFile_Click);
					menuItemToRecentFilesIndex[key] = i;
					if (i == 0)
					{
						SetMenuBeginGroup("miFileRecentFile" + i);
					}
				}
			}
			if (ShowFileExit)
			{
				AddMenuItem("miFile", "miFileExit", StiLocalization.Get("MainMenu", "menuFileExit"), "", ItemExit_Click);
				SetMenuBeginGroup("miFileExit");
			}
		}
		if (ShowEdit)
		{
			AddRootMenuItem("miEdit", StiLocalization.Get("MainMenu", "menuEdit"));
			if (ShowEditUndo || ShowEditRedo)
			{
				if (ShowEditUndo)
				{
					AddMenuItem("miEdit", "miEditUndo", StiLocalization.Get("MainMenu", "menuEditUndo"), StiReportDesignImages.Editor.Undo(), ItemUndo_Click, eShortcut.CtrlZ);
				}
				if (ShowEditRedo)
				{
					AddMenuItem("miEdit", "miEditRedo", StiLocalization.Get("MainMenu", "menuEditRedo"), StiReportDesignImages.Editor.Redo(), ItemRedo_Click, eShortcut.CtrlY);
				}
			}
			if (ShowEditCut || ShowEditCopy || ShowEditPaste || ShowEditDelete)
			{
				if (ShowEditCut)
				{
					AddMenuItem("miEdit", "miEditCut", StiLocalization.Get("MainMenu", "menuEditCut"), StiReportDesignImages.Editor.ClipboardCut(), ItemCut_Click, eShortcut.CtrlX);
					SetMenuBeginGroup("miEditCut");
				}
				if (ShowEditCopy)
				{
					AddMenuItem("miEdit", "miEditCopy", StiLocalization.Get("MainMenu", "menuEditCopy"), StiReportDesignImages.Editor.ClipboardCopy(), ItemCopy_Click, eShortcut.CtrlC);
					if (!ShowEditCut)
					{
						SetMenuBeginGroup("miEditCopy");
					}
				}
				if (ShowEditPaste)
				{
					AddMenuItem("miEdit", "miEditPaste", StiLocalization.Get("MainMenu", "menuEditPaste"), StiReportDesignImages.Editor.ClipboardPaste(), ItemPaste_Click, eShortcut.CtrlV);
					if (!ShowEditCut && !ShowEditCopy)
					{
						SetMenuBeginGroup("miEditPaste");
					}
				}
				if (ShowEditDelete)
				{
					AddMenuItem("miEdit", "miEditDelete", StiLocalization.Get("MainMenu", "menuEditDelete"), StiReportDesignImages.Actions.Remove(), ItemDelete_Click);
					if (!ShowEditCut && !ShowEditCopy && !ShowEditPaste)
					{
						SetMenuBeginGroup("miEditDelete");
					}
				}
			}
			if (ShowEditSelectAll)
			{
				AddMenuItem("miEdit", "miEditSelectAll", StiLocalization.Get("MainMenu", "menuEditSelectAll"), StiReportDesignImages.Editor.SelectAll(), ItemSelectAll_Click, eShortcut.CtrlA);
				SetMenuBeginGroup("miEditSelectAll");
			}
			if (ShowEditClearContents)
			{
				AddMenuItem("miEdit", "miEditClearContents", StiLocalization.Get("MainMenu", "menuEditClearContents"), StiReportDesignImages.Editor.ClearContent(), ItemClearContents_Click);
				SetMenuBeginGroup("miEditClearContents");
			}
		}
		if (!designer.IsRibbonGuiMode && ShowView)
		{
			AddRootMenuItem("miView", StiLocalization.Get("MainMenu", "menuView"));
			if (MenuItems["miView"] is ButtonItem buttonItem)
			{
				buttonItem.PopupOpen += ViewMenuItem_PopupOpen;
			}
			if (ShowViewNormal || ShowViewPageBreakPreview)
			{
				if (ShowViewNormal)
				{
					AddMenuItem("miView", "miViewNormal", StiLocalization.Get("MainMenu", "menuViewNormal"), ItemNormal_Click, eShortcut.None);
				}
				if (ShowViewPageBreakPreview)
				{
					AddMenuItem("miView", "miViewPageBreakPreview", StiLocalization.Get("MainMenu", "menuViewPageBreakPreview"), ItemPageBreakPreview_Click, eShortcut.None);
				}
			}
			if (ShowViewShowGrid || ShowViewAlignToGrid || ShowViewShowHeaders || ShowViewShowOrder || ShowViewShowRulers || ShowViewToolbars)
			{
				if (ShowViewShowGrid)
				{
					AddMenuItem("miView", "miViewShowGrid", StiLocalization.Get("MainMenu", "menuViewShowGrid"), StiReportDesignImages.Editor.ShowGrid(), ItemShowGrid_Click);
					SetMenuBeginGroup("miViewShowGrid");
					SetAutoCheckOnClick("miViewShowGrid", value: true);
					SetAutoCollapseOnClick("miViewShowGrid", value: false);
				}
				if (ShowViewAlignToGrid)
				{
					AddMenuItem("miView", "miViewAlignToGrid", StiLocalization.Get("MainMenu", "menuViewAlignToGrid"), StiReportDesignImages.Layout.AlignToGrid(), ItemAlignToGrid_Click);
					SetAutoCheckOnClick("miViewAlignToGrid", value: true);
					SetAutoCollapseOnClick("miViewAlignToGrid", value: false);
					if (!ShowViewShowGrid)
					{
						SetMenuBeginGroup("miViewAlignToGrid");
					}
				}
				if (ShowViewShowHeaders)
				{
					AddMenuItem("miView", "miViewShowHeaders", StiLocalization.Get("MainMenu", "menuViewShowHeaders"), StiReportDesignImages.Editor.ShowHeaders(), ItemShowHeaders_Click);
					SetAutoCheckOnClick("miViewShowHeaders", value: true);
					SetAutoCollapseOnClick("miViewShowHeaders", value: false);
					if (!ShowViewShowGrid && !ShowViewAlignToGrid)
					{
						SetMenuBeginGroup("miViewShowHeaders");
					}
				}
				if (ShowViewShowOrder)
				{
					AddMenuItem("miView", "miViewShowOrder", StiLocalization.Get("MainMenu", "menuViewShowOrder"), StiReportDesignImages.Editor.ShowOrder(), ItemShowOrder_Click);
					SetAutoCheckOnClick("miViewShowOrder", value: true);
					SetAutoCollapseOnClick("miViewShowOrder", value: false);
					if (!ShowViewShowGrid && !ShowViewAlignToGrid && !ShowViewShowHeaders)
					{
						SetMenuBeginGroup("miViewShowOrder");
					}
				}
				if (ShowViewShowRulers)
				{
					AddMenuItem("miView", "miViewShowRulers", StiLocalization.Get("MainMenu", "menuViewShowRulers"), StiReportDesignImages.Editor.ShowRulers(), ItemShowRulers_Click);
					SetAutoCheckOnClick("miViewShowRulers", value: true);
					SetAutoCollapseOnClick("miViewShowRulers", value: false);
					if (!ShowViewShowGrid && !ShowViewAlignToGrid && !ShowViewShowHeaders && !ShowViewShowOrder)
					{
						SetMenuBeginGroup("miViewShowRulers");
					}
				}
			}
			if (ShowViewToolbars)
			{
				StiService[] toolbarServices = StiToolbarService.GetToolbarServices(Designer);
				if (toolbarServices.Length != 0)
				{
					AddMenuItem("miView", "miViewToolbars", StiLocalization.Get("MainMenu", "menuViewToolbars"), "", null);
					SetMenuBeginGroup("miViewToolbars");
					CreateToolbars();
				}
			}
		}
		if (!designer.IsRibbonGuiMode && ShowTools)
		{
			AddRootMenuItem("miTools", StiLocalization.Get("MainMenu", "menuTools"));
			if (ShowToolsDataStore || ShowToolsPagesManager || ShowToolsServicesConfigurator)
			{
				if (ShowToolsDataStore)
				{
					AddMenuItem("miTools", "miToolsDataStore", StiLocalization.Get("MainMenu", "menuToolsDataStore"), ItemDataStore_Click, eShortcut.CtrlF2);
				}
				if (ShowToolsPagesManager)
				{
					AddMenuItem("miTools", "miToolsPagesManager", StiLocalization.Get("MainMenu", "menuToolsPagesManager"), ItemPagesManager_Click, eShortcut.CtrlF4);
				}
				if (ShowToolsServicesConfigurator)
				{
					AddMenuItem("miTools", "miToolsServicesConfigurator", StiLocalization.Get("MainMenu", "menuToolsServicesConfigurator"), ItemServicesConfigurator_Click, eShortcut.CtrlF5);
				}
			}
			if (ShowToolsOptions)
			{
				AddMenuItem("miTools", "miToolsOptions", StiLocalization.Get("MainMenu", "menuToolsOptions"), "", ItemOptions_Click);
				SetMenuBeginGroup("miToolsOptions");
			}
		}
		if (!designer.IsRibbonGuiMode && ShowHelp)
		{
			AddRootMenuItem("miHelp", StiLocalization.Get("MainMenu", "menuHelp"));
			if (ShowHelpFaqPage)
			{
				AddMenuItem("miHelp", "miHelpFAQPage", StiLocalization.Get("MainMenu", "menuHelpFAQPage"), "", ItemFAQPage_Click);
			}
			if (ShowHelpSupport)
			{
				AddMenuItem("miHelp", "miHelpSupport", StiLocalization.Get("MainMenu", "menuHelpSupport"), "", ItemSupport_Click);
				SetMenuBeginGroup("miHelpSupport");
			}
			AddMenuItem("miHelp", "miHelpContents", StiLocalization.Get("MainMenu", "menuHelpContents").Replace("...", string.Empty), "", ItemHelpContents_Click);
			if (ShowHelpProductHomePage)
			{
				AddMenuItem("miHelp", "miHelpProductHomePage", StiLocalization.Get("MainMenu", "menuHelpProductHomePage"), "", ItemProductHomePage_Click);
			}
			if (ShowHelpAboutProgramm)
			{
				AddMenuItem("miHelp", "miHelpAboutProgramm", StiLocalization.Get("MainMenu", "menuHelpAboutProgramm").Replace("...", string.Empty), "", ItemAbout_Click);
				SetMenuBeginGroup("miHelpAboutProgramm");
			}
		}
		designer.RefreshDesigner += Designer_RefreshDesigner;
		CreatePanels();
	}

	public override void Dispose()
	{
		menuItemToRecentFilesIndex.Clear();
		foreach (ButtonItem createdMenuItem in CreatedMenuItems)
		{
			createdMenuItem.Dispose();
		}
		CreatedMenuItems.Clear();
		MenuItems.Clear();
		base.Dispose();
	}
}
