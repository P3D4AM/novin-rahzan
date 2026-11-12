using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Gis;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Licenses;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Server;
using Stimulsoft.Base.Services;
using Stimulsoft.Controls;
using Stimulsoft.Controls.DesktopIntegration;
using Stimulsoft.Controls.Menu;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Controls.Win.DotNetBar.Metro.ColorTables;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.App;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Check;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Design.Check;
using Stimulsoft.Report.Design.Components;
using Stimulsoft.Report.Design.ContextTools;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.CrossTab;
using Stimulsoft.Report.Design.Dashboard;
using Stimulsoft.Report.Design.Dashboard.Actions;
using Stimulsoft.Report.Design.Dashboard.Table;
using Stimulsoft.Report.Design.Dashboard.Viewer;
using Stimulsoft.Report.Design.Editors;
using Stimulsoft.Report.Design.Helpers;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Design.Inplace;
using Stimulsoft.Report.Design.Interaction;
using Stimulsoft.Report.Design.Panels;
using Stimulsoft.Report.Design.Properties;
using Stimulsoft.Report.Design.Ribbon;
using Stimulsoft.Report.Design.Toolbars;
using Stimulsoft.Report.Design.Tools;
using Stimulsoft.Report.Design.Trial;
using Stimulsoft.Report.Design.Wizards;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Dictionary.Design;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Helper;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Maps;
using Stimulsoft.Report.QuickButtons;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Design;

[ToolboxBitmap(typeof(StiDesignerControl), "Toolbox.StiDesignerControl.bmp")]
[Designer("Stimulsoft.Report.Design.StiDesignerControlDesigner, Stimulsoft.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiDesignerControl : UserControl, IStiDesignerBase
{
	private string cloudReport;

	private bool showViewerRibbonMenu;

	private StiPopup popup;

	private Hashtable quickButtonsHash = new Hashtable();

	private Hashtable compImages = new Hashtable();

	private Hashtable compBigImages = new Hashtable();

	private bool firstPaint = true;

	private Hashtable ribbonTabGroups = new Hashtable();

	private bool lockZoom;

	private bool lockSetZoom;

	private Timer startTimer = new Timer();

	private bool codeTabSelected;

	private Control oldParent;

	private bool lockStandardDesignerPopup;

	private StiContextMenu sortFilterContextMenu;

	private StiTableElementSortFilterMenu sortFilterMenu;

	public const int MaxRecentFilesCount = 15;

	private IContainer components;

	internal ToolTip ToolTip;

	internal TabControl TabControl;

	public ContextMenuBar ContextMenuBar;

	public ButtonItem cmDesigner;

	public DotNetBarManager DotNetBarManager;

	public DockSite DockSiteBottom;

	public DockSite DockSiteLeft;

	public DockSite DockSiteRight;

	public DockSite DockSiteTop;

	public DockSite DockSiteLeftBars;

	public DockSite DockSiteRightBars;

	public DockSite DockSiteTopBars;

	public DockSite DockSiteBottomBars;

	public Bar MainMenu;

	private ButtonItem buttonItem1;

	private ButtonItem cmPageContextMenu;

	private ButtonItem ciPageNew;

	private ButtonItem ciFormNew;

	private ButtonItem ciScreenNew;

	private ButtonItem ciDashboardNew;

	private ButtonItem ciPageDelete;

	private ButtonItem ciPageViewQuery;

	private ButtonItem ciPageSetup;

	public ButtonItem cmQuickButtons;

	private Bar bar1;

	private ButtonItem sbpUnit;

	private ButtonItem tbCentimeters;

	private ButtonItem tbHundredthsOfInch;

	private ButtonItem tbInches;

	private ButtonItem tbMillimeters;

	private ButtonItem sbpComponent;

	private ExtendedButtonItem sbpReportChecker;

	private LabelItem sbpLocation;

	private LabelItem labelItem1;

	private ItemContainer icZoom;

	private ItemContainer icPageViewMode;

	private ButtonItem biZoomPageWidth;

	private ButtonItem biZoomPageHeight;

	private ButtonItem biZoomOnePage;

	private ButtonItem biZoom100;

	private LabelItem lbZoom;

	private SliderItem siZoom;

	private LabelItem liSeparator2;

	private Timer timerZoomSlider;

	private ButtonItem ciPageMoveLeft;

	private ButtonItem ciPageMoveRight;

	private ButtonItem ciPageDuplicate;

	public StyleManager StyleManager;

	private ButtonItem ciShowCodeTab;

	private ButtonItem ciPageOpen;

	private ButtonItem ciPageSave;

	private Timer timerAutoSave;

	private ButtonItem ciPageRename;

	private StiSubscriptionPanel panelUpdateLicense;

	private Label labelSubscriptionRequired;

	private Button buttonUpdate;

	private bool useAliasesLoaded;

	private bool useAliases;

	private StiReport report;

	private bool showMainMenu = true;

	private bool showRibbonStatusBar;

	private bool showToolbarStandard = true;

	private bool showToolbarBorders = true;

	private bool showToolbarDockStyle;

	private bool showToolbarFormatting = true;

	private bool showToolbarLayout = true;

	private bool showToolbarStatusBar = true;

	private bool showToolbarStyle = true;

	private bool showToolbarTextFormat;

	private bool showToolbarTools;

	private bool showPanelDictionary = true;

	private bool showPanelProperties = true;

	private bool showPanelReportTree = true;

	[Browsable(false)]
	public bool UseLastFormat
	{
		get
		{
			if (report == null)
			{
				return false;
			}
			if (StiDashboardAssembly.IsAssemblyLoaded && report.GetCurrentPage() is IStiDashboard)
			{
				return false;
			}
			return report.Info.UseLastFormat;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal StiRibbonDesignerUserControl RibbonDesignerUserControl { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal bool IsCloudSaving { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal bool? IsReportPreviewMode { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal string SpecialStatusBarInformation { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal bool SaveAsToSilverlight { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal Bar DockBarLeft { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal Bar DockBarRight { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal Bar DockBarTop { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal Bar DockBarBottom { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal int ClickedPageIndex { get; set; } = -1;


	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal int PreviousSelectedIndex { get; set; } = -1;


	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal List<ButtonItem> TextFormatButtons { get; set; } = new List<ButtonItem>();


	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public CompilerResults CompilerResults { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal bool DontAskSaveReportInDesigner { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal StiCheckHelper CheckHelper { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Hashtable DictionaryNodes { get; set; } = new Hashtable();


	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal bool LockRefreshDesigner { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal bool LockAddSelectedToPropertyGrid { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal bool LockReportPreview { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool LockTabControlRefresh { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal bool LockReportPropertyRefresh { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal bool InplaceTextEditorActivated { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal bool IsDesignerParamLoaded { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool AutoSaveReportBeforePreview { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual RibbonControl RibbonControl { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string HtmlReportFile { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public object[] SelectedObjects { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public object SelectedObject
	{
		get
		{
			if (SelectedObjects == null || SelectedObjects.Length == 0)
			{
				return null;
			}
			return SelectedObjects[0];
		}
		set
		{
			if (value != null)
			{
				SelectedObjects = new object[1] { value };
			}
			else
			{
				SelectedObjects = new object[0];
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiToolService CurrentTool { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiSelectToolService SelectTool { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiStyleToolService StyleTool { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiCreateToolService CreateTool { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiUndoRedo UndoRedoProperty { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public List<StiRecentFile> RecentFiles { get; } = new List<StiRecentFile>();


	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool CreateFieldOnDoubleClick
	{
		get
		{
			StiSettings.Load();
			return StiSettings.GetBool("StiDesignerOptions", "CreateFieldOnDoubleClick-V2", nullValue: false);
		}
		set
		{
			StiSettings.Load();
			StiSettings.Set("StiDesignerOptions", "CreateFieldOnDoubleClick-V2", value);
			StiSettings.Save();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool CreateLabel
	{
		get
		{
			StiSettings.Load();
			return StiSettings.GetBool("StiDesignerOptions", "CreateLabel-V2", nullValue: false);
		}
		set
		{
			StiSettings.Load();
			StiSettings.Set("StiDesignerOptions", "CreateLabel-V2", value);
			StiSettings.Save();
		}
	}

	[Browsable(false)]
	public StiDesignerConfigService DesignerConfig { get; private set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiBuilder Builder { get; private set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiUndoRedoService UndoRedo { get; private set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiServiceContainer Services { get; private set; } = new StiServiceContainer();


	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Obsolete]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool AutoSortDictionaryItems { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal Size DictionaryDesignerSize { get; set; } = new Size(500, 480);


	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiBaseStyle CurrentStyle { get; set; } = new StiStyle();


	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiElementStyleIdent CurrentDashboardStyleIdent { get; set; } = StiElementStyleIdent.Auto;


	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string CurrentDashboardCustomStyleName { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Type CurrentDashboardElementType { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string ReportKey { get; set; } = "";


	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string ReportFileName { get; set; } = "";


	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool ReportNeverSaved { get; set; } = true;


	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string ReportItemKey { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiPageView PageView { get; private set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiPageViewControl CurrentPageView => PageView?.GetCurrentPageView();

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsNotPageSelected
	{
		get
		{
			StiPage stiPage = Report?.GetCurrentPage();
			if (stiPage != null)
			{
				if (!stiPage.IsForm && !stiPage.IsScreen)
				{
					return stiPage.IsDashboard;
				}
				return true;
			}
			return false;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsPageSelected => (Report?.GetCurrentPage())?.IsPage ?? false;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsFormSelected => (Report?.GetCurrentPage())?.IsForm ?? false;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsScreenSelected => (Report?.GetCurrentPage())?.IsScreen ?? false;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsFormOrScreenSelected
	{
		get
		{
			StiPage stiPage = Report?.GetCurrentPage();
			if (stiPage != null)
			{
				if (!stiPage.IsForm)
				{
					return stiPage.IsScreen;
				}
				return true;
			}
			return false;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsDashboardSelected => (Report?.GetCurrentPage())?.IsDashboard ?? false;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal StiPlacementComponentService PlacementComponentService { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public IStiHighlightComponentService HighlightComponentService { get; set; }

	internal bool IsPropertiesPanelFocused
	{
		get
		{
			StiPropertiesPanelService service = StiPropertiesPanelService.GetService(this);
			if (service == null)
			{
				return false;
			}
			return ((Control)service.PropertyGrid).ContainsFocus;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public IStiDesignerBase DesignerControl => this;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiDesignerInfo Info { get; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiNodesMemory NodesMemory { get; } = new StiNodesMemory();


	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool UseAliases
	{
		get
		{
			if (useAliasesLoaded)
			{
				return useAliases;
			}
			StiSettings.Load();
			useAliases = StiSettings.GetBool("StiDesignerOptions", "UseAliases", nullValue: false);
			useAliasesLoaded = true;
			return useAliases;
		}
		set
		{
			StiSettings.Load();
			StiSettings.Set("StiDesignerOptions", "UseAliases", value);
			StiSettings.Save();
			useAliases = value;
			useAliasesLoaded = true;
		}
	}

	[Category("Report")]
	[DefaultValue(null)]
	public StiReport Report
	{
		get
		{
			return report;
		}
		set
		{
			SetReport(value);
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Hashtable TextAliasesHash { get; } = new Hashtable();


	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiComponentsCollection ComponentsOnReport { get; private set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiComponentsCollection ComponentsOnPage { get; private set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiComponentsCollection SelectedTextComponentsOnPage { get; private set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiComponentsCollection SelectedComponentsOnPage { get; private set; }

	[DefaultValue(true)]
	[Category("Designer.Parameters")]
	public bool ShowMainMenu
	{
		get
		{
			return showMainMenu;
		}
		set
		{
			showMainMenu = value;
			MainMenu.Visible = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool ShowRibbonStatusBar
	{
		get
		{
			return showRibbonStatusBar;
		}
		set
		{
			if (showRibbonStatusBar != value)
			{
				showRibbonStatusBar = value;
				bar1.Visible = value;
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool ShowLineNumbersInEditor { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool ShowAutoGeneratedCode { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool AllowSQLFormatting { get; set; } = true;


	[DefaultValue(true)]
	[Category("Designer.Toolbars")]
	public virtual bool ShowToolbarStandard
	{
		get
		{
			return showToolbarStandard;
		}
		set
		{
			showToolbarStandard = value;
			StiStandardToolbarService service = StiStandardToolbarService.GetService(this);
			if (service != null && service.ServiceEnabled && service.ToolBar != null)
			{
				service.ToolBar.Visible = value;
			}
		}
	}

	[DefaultValue(true)]
	[Category("Designer.Toolbars")]
	public virtual bool ShowToolbarBorders
	{
		get
		{
			return showToolbarBorders;
		}
		set
		{
			showToolbarBorders = value;
			StiBordersToolbarService service = StiBordersToolbarService.GetService(this);
			if (service != null && service.ServiceEnabled && service.ToolBar != null)
			{
				service.ToolBar.Visible = value;
			}
		}
	}

	[DefaultValue(false)]
	[Category("Designer.Toolbars")]
	public virtual bool ShowToolbarDockStyle
	{
		get
		{
			return showToolbarDockStyle;
		}
		set
		{
			showToolbarDockStyle = value;
			StiDockStyleToolbarService service = StiDockStyleToolbarService.GetService(this);
			if (service != null && service.ServiceEnabled && service.ToolBar != null)
			{
				service.ToolBar.Visible = value;
			}
		}
	}

	[DefaultValue(true)]
	[Category("Designer.Toolbars")]
	public virtual bool ShowToolbarFormatting
	{
		get
		{
			return showToolbarFormatting;
		}
		set
		{
			showToolbarFormatting = value;
			StiFormattingToolbarService service = StiFormattingToolbarService.GetService(this);
			if (service != null && service.ServiceEnabled && service.ToolBar != null)
			{
				service.ToolBar.Visible = value;
			}
		}
	}

	[DefaultValue(true)]
	[Category("Designer.Toolbars")]
	public virtual bool ShowToolbarLayout
	{
		get
		{
			return showToolbarLayout;
		}
		set
		{
			showToolbarLayout = value;
			StiLayoutToolbarService service = StiLayoutToolbarService.GetService(this);
			if (service != null && service.ServiceEnabled && service.ToolBar != null)
			{
				service.ToolBar.Visible = value;
			}
		}
	}

	[DefaultValue(true)]
	[Category("Designer.Toolbars")]
	public virtual bool ShowToolbarStatusBar
	{
		get
		{
			return showToolbarStatusBar;
		}
		set
		{
			showToolbarStatusBar = value;
			StiStatusBarService service = StiStatusBarService.GetService(this);
			if (service != null && service.ServiceEnabled && service.ToolBar != null)
			{
				service.ToolBar.Visible = value;
			}
		}
	}

	[DefaultValue(true)]
	[Category("Designer.Toolbars")]
	public virtual bool ShowToolbarStyle
	{
		get
		{
			return showToolbarStyle;
		}
		set
		{
			showToolbarStyle = value;
			StiStyleToolbarService service = StiStyleToolbarService.GetService(this);
			if (service != null && service.ServiceEnabled && service.ToolBar != null)
			{
				service.ToolBar.Visible = value;
			}
		}
	}

	[DefaultValue(false)]
	[Category("Designer.Toolbars")]
	public virtual bool ShowToolbarTextFormat
	{
		get
		{
			return showToolbarTextFormat;
		}
		set
		{
			showToolbarTextFormat = value;
			StiTextFormatToolbarService service = StiTextFormatToolbarService.GetService(this);
			if (service != null && service.ServiceEnabled && service.ToolBar != null)
			{
				service.ToolBar.Visible = value;
			}
		}
	}

	[DefaultValue(false)]
	[Category("Designer.Toolbars")]
	public virtual bool ShowToolbarTools
	{
		get
		{
			return showToolbarTools;
		}
		set
		{
			showToolbarTools = value;
			StiToolsToolbarService service = StiToolsToolbarService.GetService(this);
			if (service != null && service.ServiceEnabled && service.ToolBar != null)
			{
				service.ToolBar.Visible = value;
			}
		}
	}

	[DefaultValue(true)]
	[Category("Designer.Panels")]
	public virtual bool ShowPanelDictionary
	{
		get
		{
			return showPanelDictionary;
		}
		set
		{
			showPanelDictionary = value;
			StiDictionaryPanelService service = StiDictionaryPanelService.GetService(this);
			if (service != null && service.ServiceEnabled)
			{
				if (value)
				{
					DockPanelOpen(service);
				}
				else
				{
					DockPanelClose(service);
				}
			}
		}
	}

	[DefaultValue(true)]
	[Category("Designer.Panels")]
	[Browsable(false)]
	[Obsolete("Messages panel replaced by Report Checker and not available more!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual bool ShowPanelMessages { get; set; } = true;


	[DefaultValue(true)]
	[Category("Designer.Panels")]
	public virtual bool ShowPanelProperties
	{
		get
		{
			return showPanelProperties;
		}
		set
		{
			showPanelProperties = value;
			StiPropertiesPanelService service = StiPropertiesPanelService.GetService(this);
			if (service != null && service.ServiceEnabled)
			{
				if (value)
				{
					DockPanelOpen(service);
				}
				else
				{
					DockPanelClose(service);
				}
			}
		}
	}

	[DefaultValue(true)]
	[Category("Designer.Panels")]
	public virtual bool ShowPanelReportTree
	{
		get
		{
			return showPanelReportTree;
		}
		set
		{
			showPanelReportTree = value;
			StiReportTreePanelService service = StiReportTreePanelService.GetService(this);
			if (service != null && service.ServiceEnabled)
			{
				if (value)
				{
					DockPanelOpen(service);
				}
				else
				{
					DockPanelClose(service);
				}
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual bool IsModalMode { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual bool IsRibbonGuiMode { get; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual bool IsViewerPresent { get; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal int PageCodeIndex
	{
		get
		{
			if (Report == null)
			{
				return 0;
			}
			return Report.Pages.Count;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal int PagePreviewIndex
	{
		get
		{
			if (Report == null)
			{
				return 1;
			}
			return Report.Pages.Count + 1;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiDesignerPreviewControl DesignerPreviewControl { get; set; }

	internal bool StartCheckExpressionAfterOpeningSystemTextEditorForm { get; set; }

	public SuperTooltip SuperToolTip { get; internal set; }

	public eTooltipColor CurrentTooltipColor { get; }

	public event EventHandler LocalizationChanged;

	public event EventHandler ZoomChanged;

	public event EventHandler SelectedObjectsChanged;

	public event EventHandler SelectedStylesChanged;

	public event EventHandler ResetingToolbox;

	public event EventHandler BuidingToolbox;

	public event EventHandler RefreshStatusBar;

	public event EventHandler ClearingCompilerResults;

	public event EventHandler ShowingCompilerResults;

	public event EventHandler ClearingReportCheckerResults;

	public event StiCheckStatusEventHandler ShowingReportCheckerResults;

	public event EventHandler ShowingReportRenderingMessages;

	public event EventHandler RefreshDesigner;

	public event EventHandler RebuildDesigner;

	public event EventHandler RefreshDictionary;

	static StiDesignerControl()
	{
		StiWizardHelper.InitServices();
	}

	public StiDesignerControl()
		: this(null)
	{
	}

	public StiDesignerControl(StiReport report)
		: this(report, isRibbonGuiMode: false)
	{
	}

	internal StiDesignerControl(StiReport report, bool isRibbonGuiMode)
		: this(report, null, isRibbonGuiMode)
	{
	}

	internal StiDesignerControl(StiReport report, string cloudReport, bool isRibbonGuiMode)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_0433: Unknown result type (might be due to invalid IL or missing references)
		//IL_043d: Expected O, but got Unknown
		//IL_0753: Unknown result type (might be due to invalid IL or missing references)
		//IL_075d: Expected O, but got Unknown
		//IL_0765: Unknown result type (might be due to invalid IL or missing references)
		//IL_076f: Expected O, but got Unknown
		this.cloudReport = cloudReport;
		report = CreateBlankReport(report);
		CheckHelper = new StiCheckHelper();
		StiLocExt.Init();
		if (StiAccountSettings.CurrentAccount != null)
		{
			StiAccountSettings.CurrentAccount.CheckLicense();
		}
		StiComponentProgressHelper.Init();
		StiComponentProgressHelper.Tick += StiElementProgressHelper_Tick;
		StiComponentProgressHelper.CompletedProgress += StiElementProgressHelper_CompledProgressElement;
		StiTableElementClickHelper.TableHeaderClick += StiTableElementClickHelper_TableHeaderClick;
		StiPivotTableElementClickHelper.TableHeaderClick += StiPivotTableElementClickHelper_TableHeaderClick;
		StiOnlineMapRepaintHelper.Init();
		StiOnlineMapRepaintHelper.Tick += StiElementMapRepaintHelper_Tick;
		StiGisMapRepaintHelper.Init();
		StiGisMapRepaintHelper.Tick += StiGisMapRepaintHelper_Tick;
		SuperToolTip = new SuperTooltip();
		CurrentTooltipColor = eTooltipColor.StiOffice2013;
		Info = new StiDesignerInfo();
		IsRibbonGuiMode = isRibbonGuiMode;
		if (!((Component)this).DesignMode)
		{
			try
			{
				IsViewerPresent = StiTypeFinder.GetType("Stimulsoft.Report.Viewer.StiViewerControl, Stimulsoft.Report.Win, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a") != null;
			}
			catch
			{
			}
		}
		StiOptions.Localization.LoadDefault();
		InitializeComponent();
		DoScaling();
		((Control)this).ParentChanged += This_ParentChanged;
		((Control)this).Size = StiScale.SizeI(603, 449);
		((Control)DockSiteLeft).Location = StiScale.PointI(0, 25);
		((Control)DockSiteLeft).Size = StiScale.SizeI(0, 380);
		((Control)DockSiteRight).Size = StiScale.SizeI(0, 380);
		((Control)DockSiteBottomBars).Location = StiScale.PointI(0, 405);
		((Control)DockSiteBottomBars).Size = StiScale.SizeI(552, 0);
		((Control)DockSiteLeftBars).Location = StiScale.PointI(0, 25);
		((Control)DockSiteLeftBars).Size = StiScale.SizeI(0, 380);
		((Control)DockSiteRightBars).Location = StiScale.PointI(552, 25);
		((Control)DockSiteRightBars).Size = StiScale.SizeI(0, 380);
		((Control)DockSiteTopBars).Size = StiScale.SizeI(552, 25);
		((Control)DockSiteTop).Location = StiScale.PointI(0, 25);
		((Control)DockSiteTop).Size = StiScale.SizeI(552, 0);
		siZoom.LabelWidth = (int)Math.Ceiling(36.0 * StiScale.X);
		siZoom.Width = (int)Math.Ceiling(130.0 * StiScale.X);
		lbZoom.Width = (int)Math.Ceiling(60.0 * StiScale.X);
		if (!((Component)this).DesignMode)
		{
			StiSettings.Load();
		}
		if (!StiOptions.Appearance.ShowTooltipsHelp)
		{
			DotNetBarManager.AutoDispatchShortcuts.Remove(eShortcut.F1);
		}
		UpdateLabel();
		if (report != null)
		{
			siZoom.Value = (int)report.Info.Zoom * 100;
		}
		else
		{
			Info.Zoom = StiSettings.GetDouble("StiDesigner", "Zoom", 1.0);
		}
		StiOptions.Engine.GlobalEvents.InvokeLoadingDesigner(this, EventArgs.Empty);
		((Control)this).MouseWheel += new MouseEventHandler(This_MouseWheel);
		((UserControl)this).Load += This_FirstLoad;
		InitDesignerServices(StiDesignerOptions.Toolbars.Where((StiToolbarService s) => s.ServiceEnabled).Cast<StiService>().ToList());
		InitDesignerServices(StiDesignerOptions.Tools.Where((StiToolService s) => s.ServiceEnabled).Cast<StiService>().ToList());
		InitDesignerServices(StiDesignerOptions.Panels.Where((StiPanelService s) => s.ServiceEnabled).Cast<StiService>().ToList());
		InitDesignerServices(StiDesignerOptions.UndoRedo.Cast<StiService>().ToList());
		InitDesignerServices(StiDesignerOptions.DesignerConfig.Cast<StiService>().ToList());
		InitDesignerServices(StiDesignerOptions.MainMenu.Cast<StiService>().ToList());
		InitToolbars();
		StiServiceContainer services = Services.GetServices(typeof(StiPanelService));
		foreach (StiPanelService item in services)
		{
			item.Init(this);
		}
		DockBarLeft = null;
		DockBarRight = null;
		DockBarTop = null;
		DockBarBottom = null;
		UndoRedo = Services.GetService(typeof(StiUndoRedoService)) as StiUndoRedoService;
		if (UndoRedo == null)
		{
			UndoRedo = new StiUndoRedoService();
		}
		UndoRedo.Init(this);
		DesignerConfig = Services.GetService(typeof(StiDesignerConfigService)) as StiDesignerConfigService;
		if (DesignerConfig == null)
		{
			DesignerConfig = new StiDesignerConfigService();
		}
		DesignerConfig.Init(this);
		Builder = new StiBuilder(this);
		PageView = new StiPageView(this);
		if (!((Component)this).DesignMode)
		{
			Localize();
			LocalizePanels();
		}
		try
		{
			LockReportPropertyRefresh = true;
			Report = report;
		}
		finally
		{
			LockReportPropertyRefresh = false;
		}
		InvokeBuidingToolbox();
		if (StiOptions.Designer.ShowDesignerControlMainMenu || IsRibbonGuiMode)
		{
			MainMenu.Visible = false;
		}
		StiOptions.Engine.GlobalEvents.InvokeLoadedDesigner(this, EventArgs.Empty);
		SetAutoSaveMode();
		PlacementComponentService = new StiPlacementComponentService(this);
		HighlightComponentService = new StiHighlightComponentService(this);
		TabControl.CanReorderTabs = false;
		ContextMenuBar.SetContextMenuEx((Control)(object)TabControl.TabStrip, cmPageContextMenu);
		ApplyStyle();
		RefreshStatusBar += This_RefreshStatusBar;
		RefreshDesigner += This_RefreshDesigner;
		if (IsRibbonGuiMode)
		{
			ShowingReportCheckerResults += delegate(object sender, StiCheckStatusEventArgs e)
			{
				if (sender is StiDesignerControl stiDesignerControl)
				{
					StiRibbonDesigner stiRibbonDesigner = ((Control)stiDesignerControl).FindForm() as StiRibbonDesigner;
					if (stiRibbonDesigner == null)
					{
						UpdateCheckHelperButton(sbpReportChecker, e.Errors, e.Warnings, e.InformationMessages, e.RenderingMessages);
					}
				}
			};
			ClearingReportCheckerResults += delegate
			{
				RefreshCheckHelperButton(sbpReportChecker);
			};
		}
		StiReportChecker.GoToCode += StiReportChecker_GoToCode;
		((Control)this).AllowDrop = true;
		((Control)this).DragOver += new DragEventHandler(This_DragOver);
		((Control)this).DragDrop += new DragEventHandler(This_DragDropAsync);
		if (StiAccountSettings.CurrentAccount != null)
		{
			StiAccountSettings.CurrentAccount.UserInfoChanged += CurrentAccount_UserInfoChanged;
			AttachCloudReportActions();
		}
	}

	private void AttachCloudReportActions()
	{
		if (StiAccountSettings.CurrentAccount != null)
		{
			StiAccountSettings.CurrentAccount.Actions.SaveReportStart += Actions_SaveReportStart;
			StiAccountSettings.CurrentAccount.Actions.SaveReportCompleted += Actions_SaveReportCompleted;
			StiAccountSettings.CurrentAccount.Actions.OpenReportStart += Actions_OpenReportStart;
			StiAccountSettings.CurrentAccount.Actions.OpenReportCompleted += Actions_OpenReportCompleted;
			StiAccountSettings.CurrentAccount.Actions.OpenReportProgressChanged += Actions_OpenReportProgressChanged;
		}
	}

	private static StiReport CreateBlankReport(StiReport report)
	{
		if (report != null)
		{
			return report;
		}
		string str = StiSettings.GetStr("Designer", "StartScreen", "Welcome");
		if (!(str == "BlankReport"))
		{
			if (str == "BlankDashboard")
			{
				return StiReport.CreateNewDashboard();
			}
			return null;
		}
		return StiReport.CreateNewReport();
	}

	private void RunStartTimer()
	{
		startTimer.Interval = 500;
		startTimer.Tick += delegate
		{
			startTimer.Stop();
			if (StiOptions.Designer.RunWizardAfterLoad)
			{
				StiActions.MainMenu.InvokeReportNew(this);
			}
			if (StiOptions.Designer.RunSpecificWizardAfterLoad != null)
			{
				StiWizardService stiWizardService = StiDesignerOptions.Wizards.FirstOrDefault((StiWizardService w) => w.GetType().ToString() == StiOptions.Designer.RunSpecificWizardAfterLoad);
				if (stiWizardService == null && StiOptions.Designer.RunSpecificWizardAfterLoad == "Stimulsoft.Report.Design.Wizards.StiBlankDashboardWizardService")
				{
					stiWizardService = new StiBlankDashboardWizardService();
				}
				StiActions.MainMenu.InvokeReportWizard(this, stiWizardService);
			}
			if (StiOptions.Designer.ShowDesignerInStylesMode)
			{
				StiActions.Style.InvokeStyleDesigner(this);
			}
		};
		startTimer.Start();
	}

	internal void ResetCloudSettings()
	{
		SaveAsToSilverlight = false;
	}

	public StiDictionaryPanelService GetDictionaryService()
	{
		StiService[] panelServices = StiPanelService.GetPanelServices(this);
		for (int i = 0; i < panelServices.Length; i++)
		{
			StiPanelService stiPanelService = (StiPanelService)panelServices[i];
			if (stiPanelService is StiDictionaryPanelService)
			{
				return (StiDictionaryPanelService)stiPanelService;
			}
		}
		return null;
	}

	public StiPropertiesPanelService GetPropertiesService()
	{
		StiService[] panelServices = StiPanelService.GetPanelServices(this);
		if (panelServices != null)
		{
			StiService[] array = panelServices;
			for (int i = 0; i < array.Length; i++)
			{
				StiPanelService stiPanelService = (StiPanelService)array[i];
				if (stiPanelService is StiPropertiesPanelService)
				{
					return (StiPropertiesPanelService)stiPanelService;
				}
			}
		}
		return null;
	}

	internal void BuildDictionary()
	{
		StiService[] panelServices = StiPanelService.GetPanelServices(this);
		for (int i = 0; i < panelServices.Length; i++)
		{
			StiPanelService stiPanelService = (StiPanelService)panelServices[i];
			if (stiPanelService is StiDictionaryPanelService)
			{
				((StiDictionaryPanelService)stiPanelService).BuildDictionary();
				break;
			}
		}
	}

	public void Invalidate(StiComponent component)
	{
		RectangleD rectangleD = RectangleD.CreateFromRectangle(StiConvert.RectangleToScreen(this, component.GetDisplayRectangle()));
		Builder.Redraw(rectangleD.ToRectangle());
	}

	public string GetDesignerTitle()
	{
		string text = ReportFileName;
		if (!StiOptions.Designer.ShowFullFileNameInTitle && text.IndexOf("\\", StringComparison.InvariantCulture) != -1)
		{
			text = text.Substring(text.LastIndexOf("\\", StringComparison.InvariantCulture) + 1);
		}
		return StiOptions.Designer.GetDesignerTitleWithMask(text);
	}

	public void SetZoom(float zoom)
	{
		if (Report != null && Report.Info.Zoom != (double)zoom && !lockSetZoom)
		{
			lockSetZoom = true;
			Report.Info.Zoom = zoom;
			InvokeZoomChanged();
			((Control)this).Refresh();
			lockSetZoom = false;
		}
	}

	public void SetZoom(double zoom)
	{
		SetZoom((float)zoom);
	}

	public virtual void ApplyStyle()
	{
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Expected O, but got Unknown
		EnableRibbonBackState();
		Form val = ((Control)this).FindForm();
		if (val is StiRibbonDesigner)
		{
			((Control)val).BackColor = StiGuiHelper.GetControlColor();
		}
		((Control)this).BackColor = StiGuiHelper.GetControlColor();
		StiGuiHelper.ApplyStyle((Control)(object)this);
		foreach (StiService service in Services)
		{
			if (service is StiDesignerInitService)
			{
				((StiDesignerInitService)service).ApplyStyle();
			}
		}
		if (IsViewerPresent)
		{
			ApplyStyleTabControl();
		}
		Form val2 = ((Control)this).FindForm();
		if (val2 == null)
		{
			return;
		}
		foreach (Control item in (ArrangedElementCollection)((Control)val2).Controls)
		{
			Control val3 = item;
			if (!(val3 is RibbonControl))
			{
				continue;
			}
			RibbonControl ribbonControl = val3 as RibbonControl;
			foreach (Control item2 in (ArrangedElementCollection)((Control)ribbonControl).Controls)
			{
				Control val4 = item2;
				val4.Refresh();
			}
		}
	}

	public void ApplyStyleTabControl()
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		foreach (TabItem tab in TabControl.Tabs)
		{
			foreach (Control item in (ArrangedElementCollection)tab.AttachedControl.Controls)
			{
				Control val = item;
				if (val is StiDesignerPreviewControl stiDesignerPreviewControl)
				{
					stiDesignerPreviewControl.ApplyStyle();
				}
				if (val is StiPageViewControl stiPageViewControl)
				{
					((Control)stiPageViewControl).BackColor = StiGuiHelper.GetWorkspaceColor();
				}
			}
		}
	}

	internal void SetAutoSaveMode()
	{
		if (Report != null && Report.Info != null && Report.Info.EnableAutoSaveMode)
		{
			timerAutoSave.Enabled = false;
			timerAutoSave.Interval = Report.Info.AutoSaveInterval * 1000 * 60;
			timerAutoSave.Enabled = true;
		}
	}

	internal void DeleteHtmlReportFile()
	{
		if (string.IsNullOrEmpty(HtmlReportFile))
		{
			return;
		}
		try
		{
			string directoryName = Path.GetDirectoryName(HtmlReportFile);
			directoryName = Path.Combine(directoryName, Path.GetFileNameWithoutExtension(HtmlReportFile));
			directoryName += ".files";
			if (Directory.Exists(directoryName))
			{
				Directory.Delete(directoryName, recursive: true);
			}
			if (File.Exists(HtmlReportFile))
			{
				File.Delete(HtmlReportFile);
			}
		}
		catch
		{
		}
	}

	public static void DockPanelClose(StiPanelService service)
	{
		if (service?.DockingItem != null && service.DockingBar != null && service.DockingItem.Visible)
		{
			service.DockingBar.CloseDockTab(service.DockingItem);
		}
	}

	public static void DockPanelOpen(StiPanelService service)
	{
		if (service != null && service.DockingItem != null && !service.DockingItem.Visible)
		{
			BarUtilities.SetDockContainerVisible(service.DockingItem, visible: true);
			if (service.DockingBar.Items.Contains(service.DockingItem))
			{
				service.DockingBar.SelectedDockContainerItem = service.DockingItem;
			}
			if (service is StiPropertiesPanelService)
			{
				((Control)((StiPropertiesPanelService)service).PropertyGrid).Focus();
			}
		}
	}

	private void DisposeComponent(Component component)
	{
		try
		{
			component.Dispose();
		}
		catch
		{
		}
	}

	internal void GotoEditor(int line, int column)
	{
		foreach (TabItem tab in TabControl.Tabs)
		{
			if (tab.Tag is StiBuilder.Tabs && (StiBuilder.Tabs)tab.Tag == StiBuilder.Tabs.Code)
			{
				if (RibbonDesignerUserControl.RibbonTabPreview != null && RibbonDesignerUserControl.RibbonTabPreview.Checked)
				{
					RibbonDesignerUserControl.ribbonTabHome.Checked = true;
				}
				tab.Visible = true;
				StiDesignerCodeControl codeEditor = Builder.GetCodeEditor();
				codeEditor.CheckCodeControl();
				codeEditor.CodeEditor.ForceShowAutoGeneratedCode = true;
				((Control)codeEditor.CodeEditor).Focus();
				if (line != -1)
				{
					codeEditor.CodeEditor.GotoEditor(line, column);
				}
				break;
			}
		}
	}

	private void InitDesignerServices(List<StiService> services)
	{
		foreach (StiService service2 in services)
		{
			if (!IsRibbonGuiMode || !(service2 is StiToolbarService) || service2 is StiToolboxToolbarService)
			{
				StiService service = service2.Clone() as StiService;
				Services.Add(service);
			}
		}
	}

	public void CreateUndoRedoProperty()
	{
		if (UndoRedo.UndoLimit > 0 && UndoRedo.ServiceEnabled)
		{
			UndoRedoProperty = StiUndoRedo.CreateURFromObject("PropertyChange", Report, Report.CurrentPage);
		}
	}

	public void RefreshToolbars()
	{
		InitToolbars();
	}

	private void DoScaling()
	{
		ciPageNew.Image = StiReportDesignImages.Actions.NewPage();
		ciDashboardNew.Image = StiReportDesignImages.Actions.NewDashboard();
		ciScreenNew.Image = StiReportDesignImages.Actions.NewScreen();
		ciFormNew.Image = StiReportDesignImages.Actions.NewForm();
		ciPageDuplicate.Image = StiReportDesignImages.Actions.Duplicate();
		ciPageRename.Image = StiReportDesignImages.Actions.Rename();
		ciPageDelete.Image = StiReportDesignImages.Actions.Remove();
		ciPageViewQuery.Image = StiReportDesignImages.Actions.ViewData();
		ciPageMoveLeft.Image = StiReportDesignImages.Actions.MoveLeft();
		ciPageMoveRight.Image = StiReportDesignImages.Actions.MoveRight();
		ciPageSetup.Image = StiReportDesignImages.Actions.SetupPage();
		ciPageOpen.Image = StiReportDesignImages.Actions.OpenFile();
		ciPageSave.Image = StiReportDesignImages.Actions.SaveFile();
		biZoom100.Image = StiReportDesignImages.Actions.Zoom100();
		biZoomOnePage.Image = StiReportDesignImages.Actions.ZoomSize();
		biZoomPageWidth.Image = StiReportDesignImages.Actions.ZoomWidth();
		biZoomPageHeight.Image = StiReportDesignImages.Actions.ZoomHeight();
		biZoom100.ImagePaddingHorizontal = StiScale.I20;
		biZoom100.ImagePaddingVertical = StiScale.I10;
		biZoomOnePage.ImagePaddingHorizontal = StiScale.I20;
		biZoomOnePage.ImagePaddingVertical = StiScale.I10;
		biZoomPageWidth.ImagePaddingHorizontal = StiScale.I20;
		biZoomPageWidth.ImagePaddingVertical = StiScale.I10;
		biZoomPageHeight.ImagePaddingHorizontal = StiScale.I20;
		biZoomPageHeight.ImagePaddingVertical = StiScale.I10;
		((Control)labelSubscriptionRequired).Top = StiScale.I(9);
		((Control)panelUpdateLicense).Height = StiScale.I(30);
		((Control)buttonUpdate).Height = StiScale.I(24);
		((Control)buttonUpdate).Width = StiScale.I(80);
		((Control)buttonUpdate).Top = StiScale.I(3);
		((Control)buttonUpdate).Left = ((Control)panelUpdateLicense).Width - StiScale.I(90);
		sbpReportChecker.Image = StiReportDesignImages.Actions.Run();
	}

	internal void InitToolbars()
	{
		DockSiteTopBars.SuspendLayout();
		StiService[] toolbarServices = StiToolbarService.GetToolbarServices(this);
		StiService[] array = toolbarServices;
		for (int i = 0; i < array.Length; i++)
		{
			StiToolbarService stiToolbarService = (StiToolbarService)array[i];
			if (!(stiToolbarService is StiRibbonToolboxToolbarService) || RibbonControl != null)
			{
				stiToolbarService.Init(this);
			}
		}
		DockSiteTopBars.ResumeLayout();
		StiMainMenuService service = StiMainMenuService.GetService(this);
		if (service != null)
		{
			service.Init(this);
		}
		else
		{
			MainMenu.Visible = false;
		}
	}

	internal Image GetBigImageType(Type type, Type baseType)
	{
		return GetBigImageType(type.Name, baseType);
	}

	internal Image GetBigImageType(string typeName, Type baseType)
	{
		object? obj = compBigImages[typeName];
		Image val = (Image)((obj is Image) ? obj : null);
		if (val != null)
		{
			return val;
		}
		val = (Image)(object)StiResourcesHelper.GetImageByName("Stimulsoft.Report.Design.Resources.Office2013.ToolBox." + typeName + StiScale.BigStepName + ".png");
		if (val == null)
		{
			val = StiImageResizer.GetImage((Image)(object)StiResourcesHelper.GetImageByName("Stimulsoft.Report.Design.Resources.Office2013.ToolBox." + typeName + "_x2.png"));
		}
		if (val != null)
		{
			compBigImages[typeName] = val;
		}
		return val;
	}

	internal Image GetImageType(Type type, Type baseType)
	{
		return GetImageType(type, baseType, category: false);
	}

	internal Image GetImageType(string typeName, Type baseType)
	{
		object? obj = compImages[typeName];
		Image val = (Image)((obj is Image) ? obj : null);
		if (val == null)
		{
			val = (Image)(object)StiResourcesHelper.GetImageByName("Stimulsoft.Report.Design.Resources.Office2013.ToolBox." + typeName + StiScale.StepName + ".png");
			if (val == null)
			{
				val = StiImageResizer.GetImage((Image)(object)StiResourcesHelper.GetImageByName("Stimulsoft.Report.Design.Resources.Office2013.ToolBox." + typeName + ".png"));
			}
			if (val != null)
			{
				compImages[typeName] = val;
			}
		}
		if (val == null)
		{
			object? obj2 = compImages[baseType.Name];
			val = (Image)((obj2 is Image) ? obj2 : null);
			if (val == null)
			{
				val = StiImageResizer.GetImage((Image)(object)StiServiceBitmapAttribute.GetImage(baseType));
			}
			if (val != null)
			{
				compImages[baseType.Name] = val;
			}
		}
		return val;
	}

	internal Image GetImageType(Type type, Type baseType, bool category)
	{
		object? obj = compImages[type.Name];
		Image val = (Image)((obj is Image) ? obj : null);
		if (val == null)
		{
			val = (Image)(object)StiResourcesHelper.GetImageByName("Stimulsoft.Report.Design.Resources.Office2013.ToolBox." + type.Name + StiScale.StepName + ".png");
			if (val == null)
			{
				val = StiImageResizer.GetImage((Image)(object)StiResourcesHelper.GetImageByName("Stimulsoft.Report.Design.Resources.Office2013.ToolBox." + type.Name + ".png"));
			}
			if (val == null)
			{
				val = StiImageResizer.GetImage((Image)(object)(category ? StiServiceCategoryBitmapAttribute.GetImage(type) : StiServiceBitmapAttribute.GetImage(type)));
			}
			if (val != null)
			{
				compImages[type.Name] = val;
			}
		}
		if (val == null)
		{
			object? obj2 = compImages[baseType.Name];
			val = (Image)((obj2 is Image) ? obj2 : null);
			if (val == null)
			{
				val = StiImageResizer.GetImage((Image)(object)(category ? StiServiceCategoryBitmapAttribute.GetImage(baseType) : StiServiceBitmapAttribute.GetImage(baseType)));
			}
			if (val != null)
			{
				compImages[baseType.Name] = val;
			}
		}
		return val;
	}

	internal void AddSelectedToPropertyGrid()
	{
		AddSelectedToPropertyGrid(updateSelectedObjects: true);
	}

	internal void AddSelectedToPropertyGrid(bool updateSelectedObjects)
	{
		if (!LockAddSelectedToPropertyGrid && Report != null)
		{
			object[] selected = Report.GetSelected();
			if (selected != null && selected.Length != 0)
			{
				SelectedObjects = selected;
			}
			else
			{
				StiPage currentPage = Report.GetCurrentPage();
				currentPage.Select();
				SelectedObject = currentPage;
			}
			if (updateSelectedObjects)
			{
				InvokeSelectedObjectsChanged();
			}
		}
	}

	internal void AddSelectedToPropertyGrid(object obj)
	{
		if (!LockAddSelectedToPropertyGrid)
		{
			SelectedObject = obj;
			InvokeSelectedObjectsChanged();
		}
	}

	private StiCellGeom GetSelectedChartElement(float x, float y, StiCellGeom parent)
	{
		if (parent == null)
		{
			return null;
		}
		float x2 = x - parent.ClientRectangle.X;
		float y2 = y - parent.ClientRectangle.Y;
		if (parent.ChildGeoms != null)
		{
			for (int num = parent.ChildGeoms.Count - 1; num >= 0; num--)
			{
				StiCellGeom selectedChartElement = GetSelectedChartElement(x2, y2, parent.ChildGeoms[num]);
				if (selectedChartElement != null)
				{
					return selectedChartElement;
				}
			}
		}
		if ((parent is StiLegendGeom || parent is StiChartTitleGeom || parent is StiConstantLinesYGeom || parent is StiConstantLinesVerticalGeom || parent is StiStripsXGeom || parent is StiStripsYGeom || parent is StiChartTableGeom || parent is StiXAxisGeom || parent is StiYAxisGeom || parent is StiSeriesElementGeom || parent is StiSeriesLabelsGeom) && parent.Contains(x, y))
		{
			return parent;
		}
		return null;
	}

	internal void SaveNodes()
	{
		StiDictionaryPanelService service = StiDictionaryPanelService.GetService(this);
		if (service != null)
		{
			NodesMemory.SaveExpNodes((TreeView)(object)service.DictionaryControl.tvData);
		}
	}

	internal void LoadNodes()
	{
		StiDictionaryPanelService service = StiDictionaryPanelService.GetService(this);
		if (service != null)
		{
			NodesMemory.LoadExpNodes((TreeView)(object)service.DictionaryControl.tvData, restoreSelectedNode: true);
		}
	}

	public bool TestReportSave()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Invalid comparison between Unknown and I4
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Invalid comparison between Unknown and I4
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		if (IsCloudSaving)
		{
			MessageBox.Show(StiLocalization.Get("Cloud", "SavingToStimulsoftCloudPleaseWait"), StiOptions.Designer.GetDesignerTitle(), (MessageBoxButtons)0, (MessageBoxIcon)64);
			return true;
		}
		if (Report == null || !Report.IsModified)
		{
			return false;
		}
		if (DontAskSaveReportInDesigner)
		{
			return false;
		}
		DialogResult val = (DialogResult)6;
		if (!StiOptions.Designer.DontAskSaveReport)
		{
			StiSaveChangesInDialogBoxDesignerEvenArgs stiSaveChangesInDialogBoxDesignerEvenArgs = new StiSaveChangesInDialogBoxDesignerEvenArgs
			{
				Message = string.Format(Loc.Get("Questions", "qnSaveChanges"), ReportFileName)
			};
			StiOptions.Engine.GlobalEvents.InvokeSaveChangesInDialogBoxDesigner(this, stiSaveChangesInDialogBoxDesignerEvenArgs);
			val = ((!stiSaveChangesInDialogBoxDesignerEvenArgs.ShowDefaultDialogBox) ? stiSaveChangesInDialogBoxDesignerEvenArgs.DialogResult : MessageBox.Show((IWin32Window)(object)((Control)this).FindForm(), stiSaveChangesInDialogBoxDesignerEvenArgs.Message, StiOptions.Designer.GetDesignerTitle(), (MessageBoxButtons)3));
		}
		if ((int)val == 2)
		{
			return true;
		}
		if ((int)val == 6)
		{
			if (StiAccountSettings.CurrentAccount != null)
			{
				if (!StiAccountSettings.CurrentAccount.AccountChecker.CheckUserActivated(((Control)this).Handle))
				{
					return true;
				}
				if (!StiAccountSettings.CurrentAccount.AccountChecker.CheckTrialExpired())
				{
					return true;
				}
				if (!StiAccountSettings.CurrentAccount.AccountChecker.CheckProductExpired())
				{
					return true;
				}
			}
			return !StiActions.MainMenu.InvokeReportSave(this);
		}
		return false;
	}

	internal StiServiceContainer GetComponentsContextServices(StiComponent component)
	{
		Attribute[] array = new Attribute[0];
		if (component != null)
		{
			array = (Attribute[])component.GetType().GetCustomAttributes(typeof(StiContextToolAttribute), inherit: false);
		}
		IEnumerable<StiCTService> enumerable = StiDesignerOptions.ContextMenu.Where((StiCTService s) => s.ServiceEnabled);
		StiServiceContainer stiServiceContainer = new StiServiceContainer();
		foreach (StiCTService item in enumerable)
		{
			StiPage currentPage = Report.GetCurrentPage();
			if (currentPage == null || ((item.PageType & StiPageType.Dashboard) == 0 && currentPage.IsDashboard) || ((item.PageType & StiPageType.Form) == 0 && currentPage.IsForm) || ((item.PageType & StiPageType.Screen) == 0 && currentPage.IsScreen) || ((item.PageType & StiPageType.Page) == 0 && currentPage.IsPage))
			{
				continue;
			}
			if (item.GetInterfaceType() == null)
			{
				stiServiceContainer.Add(item);
				continue;
			}
			Attribute[] array2 = array;
			foreach (Attribute attribute in array2)
			{
				StiContextToolAttribute stiContextToolAttribute = (StiContextToolAttribute)attribute;
				if (item.GetInterfaceType() == stiContextToolAttribute.Type)
				{
					stiServiceContainer.Add(item);
				}
			}
		}
		return stiServiceContainer;
	}

	internal void SelectTabPage(StiPage page)
	{
		int num = page.Report.Pages.IndexOf(page);
		for (int i = 0; i < Report.Pages.Count; i++)
		{
			if ((int)TabControl.Tabs[i].Tag == num)
			{
				TabControl.SelectedTabIndex = i;
				break;
			}
		}
	}

	public void SaveDesignerParam()
	{
		if (!((Component)this).DesignMode && ((Control)this).Visible && IsDesignerParamLoaded)
		{
			DesignerConfig.Save();
		}
	}

	public void LoadDesignerParam()
	{
		if (!((Component)this).DesignMode && ((Control)this).Visible)
		{
			DesignerConfig.Load();
			IsDesignerParamLoaded = true;
		}
	}

	internal void SortItems()
	{
		StiDictionaryPanelService dictionaryService = GetDictionaryService();
		if (dictionaryService != null && dictionaryService.CurrentSort != StiDictionarySortOrder.None && Report != null && Report.Dictionary != null)
		{
			StiSortOrder order = ((dictionaryService.CurrentSort != 0) ? StiSortOrder.Desc : StiSortOrder.Asc);
			Report.Dictionary.SortItems(order);
		}
	}

	internal void AssignTagProperty()
	{
		for (int i = 0; i < Report.Pages.Count; i++)
		{
			TabItem tabItem = TabControl.Tabs[i];
			tabItem.Tag = i;
		}
	}

	internal void GetStyleFromComponents()
	{
		if (!UseLastFormat)
		{
			return;
		}
		for (int num = SelectedComponentsOnPage.Count - 1; num >= 0; num--)
		{
			if (!(SelectedComponentsOnPage[num] is IStiIgnoryStyle))
			{
				StiComponent stiComponent = SelectedComponentsOnPage[num];
				if (stiComponent is IStiDashboardElementStyle)
				{
					CurrentDashboardStyleIdent = ((IStiDashboardElementStyle)stiComponent).Style;
				}
				else
				{
					CurrentStyle.GetStyleFromComponent(stiComponent, StiStyleElements.All);
				}
			}
		}
	}

	internal void RemoveRecentFile(StiRecentFile file)
	{
		RecentFiles.Remove(file);
		SaveDesignerParam();
	}

	public void AddRecentFile(string file, object tag)
	{
		int num = -1;
		int num2 = 0;
		string[] array = file.Split(new char[1] { '\\' });
		if (array.Length == 3 && array[0].Equals("cloud"))
		{
			string text = array[1];
			foreach (StiRecentFile recentFile in RecentFiles)
			{
				string[] array2 = recentFile.FileName.Split(new char[1] { '\\' });
				if (array2.Length == 3 && array2[0].Equals("cloud") && text == array2[1])
				{
					num = num2;
					break;
				}
				num2++;
			}
		}
		else
		{
			foreach (StiRecentFile recentFile2 in RecentFiles)
			{
				if (recentFile2.FileName == file)
				{
					num = num2;
					break;
				}
				num2++;
			}
		}
		if (num == -1)
		{
			RecentFiles.Insert(0, new StiRecentFile(file, Path.GetFileNameWithoutExtension(file), tag));
			if (RecentFiles.Count > 15)
			{
				RecentFiles.RemoveAt(15);
			}
		}
		else
		{
			RecentFiles.RemoveAt(num);
			RecentFiles.Insert(0, new StiRecentFile(file, Path.GetFileNameWithoutExtension(file), tag));
		}
		SaveDesignerParam();
	}

	public async Task OpenRecentCloudFileAsync(StiRecentFile recentFile)
	{
		if (StiAccountSettings.CurrentAccount?.User == null || (StiAccountSettings.CurrentAccount.User != null && string.IsNullOrEmpty(StiAccountSettings.CurrentAccount.User.SessionKey)))
		{
			IStiAuthorizationWindow stiAuthorizationWindow = StiAccountSettings.CurrentAccount?.AccountCreater.GetAuthorizationWindow();
			if (stiAuthorizationWindow != null && !stiAuthorizationWindow.ShowDialog(((Control)this).Handle).GetValueOrDefault())
			{
				return;
			}
		}
		try
		{
			StiDictionaryPanelHelper.ActivatePanel(this);
			StiDictionaryPanelHelper.ShowProgress(this, Loc.Get("DesignerFx", "LoadingReport"));
			string key = recentFile.Tag.ToString();
			string fileName = recentFile.DisplayName;
			StiLoadResult stiLoadResult = await StiAccountSettings.CurrentAccount.Actions.LoadReportByteArrayAsync(key, ((Control)this).Handle);
			if (stiLoadResult?.Content == null)
			{
				MessageBox.Show(string.Format(StiLocalization.Get("Errors", "FileNotFound"), fileName));
				RemoveRecentFile(recentFile);
				return;
			}
			string itemName = stiLoadResult.Name;
			byte[] content = stiLoadResult.Content;
			StiReport stiReport = new StiReport();
			if (content.Length > 3 && content[0] == 109 && content[1] == 114 && content[2] == 120)
			{
				StiLoadPasswordForm stiLoadPasswordForm = new StiLoadPasswordForm();
				try
				{
					if ((int)((Form)stiLoadPasswordForm).ShowDialog() == 1)
					{
						stiReport.LoadEncryptedReport(content, stiLoadPasswordForm.Password);
						if (StiDesigner.CanDesignerChangeReportFileName)
						{
							stiReport.ReportFile = itemName + ".mrx";
						}
					}
				}
				finally
				{
					((IDisposable)stiLoadPasswordForm)?.Dispose();
				}
			}
			else
			{
				stiReport.Load(content);
				if (StiDesigner.CanDesignerChangeReportFileName)
				{
					stiReport.ReportFile = itemName + ".mrt";
				}
			}
			StiAdapterHelper.CheckDownloadAdapter(stiReport, this);
			StiDictionaryPanelHelper.ShowProgress(this, Loc.Get("DesignerFx", "LoadingData"), useTimer: false);
			await SetReportAsync(stiReport);
			ReportNeverSaved = false;
			ReportItemKey = key;
			if (StiOptions.Designer.CanDesignerChangeReportFileName)
			{
				string file = "cloud\\" + key + "\\" + itemName;
				AddRecentFile(file, key);
			}
		}
		catch (StiDashboardNotSupportedException e)
		{
			StiLogService.Write(((object)this).GetType(), "Open report...ERROR");
			StiLogService.Write(((object)this).GetType(), e);
			StiDashboardNotSupportedForm stiDashboardNotSupportedForm = new StiDashboardNotSupportedForm();
			try
			{
				AddOwnedForm((Form)(object)stiDashboardNotSupportedForm);
				((Form)stiDashboardNotSupportedForm).ShowDialog();
			}
			finally
			{
				((IDisposable)stiDashboardNotSupportedForm)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			StiLogService.Write(((object)this).GetType(), "Open report...ERROR");
			StiLogService.Write(((object)this).GetType(), ex);
			StiExceptionProvider.Show(ex);
		}
		finally
		{
			StiDictionaryPanelHelper.HideProgress(this);
		}
	}

	public async Task OpenRecentCloudFileAsync(string key)
	{
		if (StiAccountSettings.CurrentAccount == null || StiAccountSettings.CurrentAccount.User == null || (StiAccountSettings.CurrentAccount.User != null && string.IsNullOrEmpty(StiAccountSettings.CurrentAccount.User.SessionKey)))
		{
			IStiAuthorizationWindow authorizationWindow = StiAccountSettings.CurrentAccount.AccountCreater.GetAuthorizationWindow();
			if (!authorizationWindow.ShowDialog(((Control)this).Handle).GetValueOrDefault())
			{
				return;
			}
		}
		try
		{
			StiLoadResult stiLoadResult = await StiAccountSettings.CurrentAccount.Actions.LoadReportByteArrayAsync(key, ((Control)this).Handle);
			if (stiLoadResult?.Content == null)
			{
				MessageBox.Show(string.Format(StiLocalization.Get("Errors", "FileNotFound"), key));
				return;
			}
			string itemName = stiLoadResult.Name;
			byte[] content = stiLoadResult.Content;
			StiReport stiReport = new StiReport();
			if (content.Length > 3 && content[0] == 109 && content[1] == 114 && content[2] == 120)
			{
				StiLoadPasswordForm stiLoadPasswordForm = new StiLoadPasswordForm();
				try
				{
					if ((int)((Form)stiLoadPasswordForm).ShowDialog() == 1)
					{
						stiReport.LoadEncryptedReport(content, stiLoadPasswordForm.Password);
						if (StiDesigner.CanDesignerChangeReportFileName)
						{
							stiReport.ReportFile = itemName + ".mrx";
						}
					}
				}
				finally
				{
					((IDisposable)stiLoadPasswordForm)?.Dispose();
				}
			}
			else
			{
				stiReport.Load(content);
				if (StiDesigner.CanDesignerChangeReportFileName)
				{
					stiReport.ReportFile = itemName + ".mrt";
				}
			}
			StiAdapterHelper.CheckDownloadAdapter(stiReport, this);
			StiDictionaryPanelHelper.ShowProgress(this, Loc.Get("DesignerFx", "LoadingData"), useTimer: false);
			await SetReportAsync(stiReport);
			ReportNeverSaved = false;
			ReportItemKey = key;
			if (StiOptions.Designer.CanDesignerChangeReportFileName)
			{
				string file = "cloud\\" + key + "\\" + itemName;
				AddRecentFile(file, key);
			}
		}
		catch (StiDashboardNotSupportedException e)
		{
			StiLogService.Write(((object)this).GetType(), "Open report...ERROR");
			StiLogService.Write(((object)this).GetType(), e);
			StiDashboardNotSupportedForm stiDashboardNotSupportedForm = new StiDashboardNotSupportedForm();
			try
			{
				AddOwnedForm((Form)(object)stiDashboardNotSupportedForm);
				((Form)stiDashboardNotSupportedForm).ShowDialog();
			}
			finally
			{
				((IDisposable)stiDashboardNotSupportedForm)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			StiLogService.Write(((object)this).GetType(), "Open report...ERROR");
			StiLogService.Write(((object)this).GetType(), ex);
			StiExceptionProvider.Show(ex);
		}
		finally
		{
			StiDictionaryPanelHelper.HideProgress(this);
		}
	}

	public async Task OpenFileAsync(string name)
	{
		if (!File.Exists(name))
		{
			MessageBox.Show(string.Format(StiLocalization.Get("Errors", "FileNotFound"), name));
			return;
		}
		if (Report == null)
		{
			await SetReportAsync(StiActivator.CreateObject(StiOptions.Engine.BaseReportType) as StiReport);
		}
		ReportKey = null;
		if (StiDesigner.CanDesignerChangeReportFileName)
		{
			ReportFileName = name;
		}
		TabControl.SelectedTabIndex = 0;
		try
		{
			if (name.EndsWith(".mrx"))
			{
				StiLoadPasswordForm stiLoadPasswordForm = new StiLoadPasswordForm(name);
				try
				{
					if ((int)((Form)stiLoadPasswordForm).ShowDialog() != 1)
					{
						return;
					}
					report.LoadEncryptedReport(name, stiLoadPasswordForm.Password);
					StiAdapterHelper.CheckDownloadAdapter(report, this);
				}
				finally
				{
					((IDisposable)stiLoadPasswordForm)?.Dispose();
				}
			}
			else
			{
				Report.Load(name);
				StiAdapterHelper.CheckDownloadAdapter(Report, this);
			}
		}
		catch (StiDashboardNotSupportedException)
		{
			StiDashboardNotSupportedForm stiDashboardNotSupportedForm = new StiDashboardNotSupportedForm();
			try
			{
				AddOwnedForm((Form)(object)stiDashboardNotSupportedForm);
				((Form)stiDashboardNotSupportedForm).ShowDialog();
			}
			finally
			{
				((IDisposable)stiDashboardNotSupportedForm)?.Dispose();
			}
		}
		Report.Pages.SortByPriority();
		StiDictionaryPanelService dictionaryService = GetDictionaryService();
		if (dictionaryService != null)
		{
			switch (dictionaryService.CurrentSort)
			{
			case StiDictionarySortOrder.Asc:
				report.Dictionary.SortItems(StiSortOrder.Asc);
				break;
			case StiDictionarySortOrder.Desc:
				report.Dictionary.SortItems(StiSortOrder.Desc);
				break;
			}
		}
		try
		{
			StiDictionaryPanelHelper.ActivatePanel(this);
			StiDictionaryPanelHelper.ShowProgress(this, Loc.Get("DesignerFx", "LoadingData"));
			AddRecentFile(name, null);
			UndoRedo.Clear();
			report.CurrentPage = 0;
			report.ResetSelection();
			report.Pages[0].Select();
			Builder.BuildReport();
			InvokeBuidingToolbox();
			Builder.InvalidateRulers();
			StiInplaceDesigner.Close();
			StiElementActionHelper.Hide();
			ReportNeverSaved = false;
			ReportItemKey = null;
			await Report.Dictionary.ConnectAsync(loadData: false);
			await StiDictionary.DoAutoSynchronizeAsync(Report);
			((Control)this).Refresh();
		}
		finally
		{
			StiDictionaryPanelHelper.HideProgress(this);
		}
	}

	public void OpenFile(string name)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Invalid comparison between Unknown and I4
		if (!File.Exists(name))
		{
			MessageBox.Show(string.Format(StiLocalization.Get("Errors", "FileNotFound"), name));
			return;
		}
		if (Report == null)
		{
			Report = StiActivator.CreateObject(StiOptions.Engine.BaseReportType) as StiReport;
		}
		ReportKey = null;
		if (StiDesigner.CanDesignerChangeReportFileName)
		{
			ReportFileName = name;
		}
		TabControl.SelectedTabIndex = 0;
		try
		{
			if (name.EndsWith(".mrx"))
			{
				StiLoadPasswordForm stiLoadPasswordForm = new StiLoadPasswordForm(name);
				try
				{
					if ((int)((Form)stiLoadPasswordForm).ShowDialog() != 1)
					{
						return;
					}
					report.LoadEncryptedReport(name, stiLoadPasswordForm.Password);
					StiAdapterHelper.CheckDownloadAdapter(report, this);
				}
				finally
				{
					((IDisposable)stiLoadPasswordForm)?.Dispose();
				}
			}
			else
			{
				Report.Load(name);
				StiAdapterHelper.CheckDownloadAdapter(Report, this);
			}
		}
		catch (StiDashboardNotSupportedException)
		{
			StiDashboardNotSupportedForm stiDashboardNotSupportedForm = new StiDashboardNotSupportedForm();
			try
			{
				AddOwnedForm((Form)(object)stiDashboardNotSupportedForm);
				((Form)stiDashboardNotSupportedForm).ShowDialog();
			}
			finally
			{
				((IDisposable)stiDashboardNotSupportedForm)?.Dispose();
			}
		}
		Report.Pages.SortByPriority();
		StiDictionaryPanelService dictionaryService = GetDictionaryService();
		if (dictionaryService != null)
		{
			switch (dictionaryService.CurrentSort)
			{
			case StiDictionarySortOrder.Asc:
				report.Dictionary.SortItems(StiSortOrder.Asc);
				break;
			case StiDictionarySortOrder.Desc:
				report.Dictionary.SortItems(StiSortOrder.Desc);
				break;
			}
		}
		AddRecentFile(name, null);
		UndoRedo.Clear();
		Report.Dictionary.Connect(loadData: false);
		report.CurrentPage = 0;
		report.ResetSelection();
		report.Pages[0].Select();
		Builder.BuildReport();
		InvokeBuidingToolbox();
		Builder.InvalidateRulers();
		StiInplaceDesigner.Close();
		StiElementActionHelper.Hide();
		ReportNeverSaved = false;
		ReportItemKey = null;
	}

	public void OpenReport(StiReport report)
	{
		Report = report;
		TabControl.SelectedTabIndex = 0;
		Report.Pages.SortByPriority();
		UndoRedo.Clear();
		Report.Dictionary.Connect(loadData: false);
		report.CurrentPage = 0;
		report.ResetSelection();
		report.Pages[0].Select();
		Builder.BuildReport();
		InvokeBuidingToolbox();
		Builder.InvalidateRulers();
		ReportNeverSaved = false;
	}

	internal void OpenUnsavedReportFile(string name)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Invalid comparison between Unknown and I4
		if (!File.Exists(name))
		{
			MessageBox.Show(string.Format(StiLocalization.Get("Errors", "FileNotFound"), name));
			return;
		}
		if (Report == null)
		{
			Report = StiActivator.CreateObject(StiOptions.Engine.BaseReportType) as StiReport;
		}
		ReportKey = null;
		ReportNeverSaved = true;
		ReportItemKey = null;
		TabControl.SelectedTabIndex = 0;
		try
		{
			if (name.EndsWith(".mrx"))
			{
				StiLoadPasswordForm stiLoadPasswordForm = new StiLoadPasswordForm(name);
				try
				{
					if ((int)((Form)stiLoadPasswordForm).ShowDialog() != 1)
					{
						return;
					}
					report.LoadEncryptedReport(name, stiLoadPasswordForm.Password);
					StiAdapterHelper.CheckDownloadAdapter(report, this);
				}
				finally
				{
					((IDisposable)stiLoadPasswordForm)?.Dispose();
				}
			}
			else
			{
				Report.Load(name);
				StiAdapterHelper.CheckDownloadAdapter(Report, this);
			}
		}
		catch (StiDashboardNotSupportedException)
		{
			StiDashboardNotSupportedForm stiDashboardNotSupportedForm = new StiDashboardNotSupportedForm();
			try
			{
				AddOwnedForm((Form)(object)stiDashboardNotSupportedForm);
				((Form)stiDashboardNotSupportedForm).ShowDialog();
			}
			finally
			{
				((IDisposable)stiDashboardNotSupportedForm)?.Dispose();
			}
		}
		Report.IsModified = true;
		Report.Pages.SortByPriority();
		StiDictionaryPanelService dictionaryService = GetDictionaryService();
		if (dictionaryService != null)
		{
			switch (dictionaryService.CurrentSort)
			{
			case StiDictionarySortOrder.Asc:
				report.Dictionary.SortItems(StiSortOrder.Asc);
				break;
			case StiDictionarySortOrder.Desc:
				report.Dictionary.SortItems(StiSortOrder.Desc);
				break;
			}
		}
		UndoRedo.Clear();
		Report.Dictionary.Connect(loadData: false);
		report.CurrentPage = 0;
		report.ResetSelection();
		report.Pages[0].Select();
		Builder.BuildReport();
		InvokeBuidingToolbox();
		Builder.InvalidateRulers();
		StiInplaceDesigner.Close();
		StiElementActionHelper.Hide();
	}

	private void SetToolboxEnabled(bool enabled)
	{
	}

	internal void AssignReport(StiReport report)
	{
		if (report != null)
		{
			report.Designer = this;
			report.CurrentPage = 0;
			StiDictionaryPanelService dictionaryService = GetDictionaryService();
			if (dictionaryService != null)
			{
				switch (dictionaryService.CurrentSort)
				{
				case StiDictionarySortOrder.Asc:
					report.Dictionary.SortItems(StiSortOrder.Asc);
					break;
				case StiDictionarySortOrder.Desc:
					report.Dictionary.SortItems(StiSortOrder.Desc);
					break;
				}
			}
			report.ResetSelection();
			report.Pages[0].Select();
			if (report.ReportFile == string.Empty)
			{
				ReportFileName = report.ReportName + ".mrt";
			}
			else
			{
				ReportFileName = report.ReportFile;
				ReportNeverSaved = false;
			}
		}
		else
		{
			ReportFileName = string.Empty;
			ReportNeverSaved = false;
		}
		ReportKey = null;
	}

	internal void DisableTimerAutoSave()
	{
		if (timerAutoSave != null)
		{
			timerAutoSave.Enabled = false;
		}
	}

	internal void DisposeDesigner()
	{
		DisableTimerAutoSave();
		if (Services == null)
		{
			return;
		}
		if (SuperToolTip != null)
		{
			SuperToolTip.Dispose();
			SuperToolTip = null;
		}
		LockRefreshDesigner = true;
		StiLogService.Write(((object)this).GetType(), "Closing report designer");
		DeleteHtmlReportFile();
		StiServiceContainer services = Services.GetServices(typeof(StiDesignerInitService));
		foreach (StiDesignerInitService item in services)
		{
			item.Dispose();
		}
		services = Services.GetServices(typeof(StiToolbarService));
		foreach (StiDesignerInitService item2 in services)
		{
			item2.Dispose();
		}
		services = Services.GetServices(typeof(StiToolService));
		foreach (StiDesignerInitService item3 in services)
		{
			item3.Dispose();
		}
		services = Services.GetServices(typeof(StiPanelService));
		foreach (StiDesignerInitService item4 in services)
		{
			item4.Dispose();
		}
		services = Services.GetServices(typeof(StiUndoRedoService));
		foreach (StiDesignerInitService item5 in services)
		{
			item5.Dispose();
		}
		services = Services.GetServices(typeof(StiDesignerConfigService));
		foreach (StiDesignerInitService item6 in services)
		{
			item6.Dispose();
		}
		services = Services.GetServices(typeof(StiMainMenuService));
		foreach (StiDesignerInitService item7 in services)
		{
			item7.Dispose();
		}
		TextAliasesHash.Clear();
		quickButtonsHash.Clear();
		DesignerConfig.Dispose();
		DesignerConfig = null;
		PageView.Dispose();
		PageView = null;
		if (UndoRedo != null)
		{
			UndoRedo.Dispose();
		}
		UndoRedo = null;
		CurrentStyle = null;
		UndoRedoProperty = null;
		if (ComponentsOnReport != null)
		{
			ComponentsOnReport.Clear();
		}
		ComponentsOnReport = null;
		if (ComponentsOnPage != null)
		{
			ComponentsOnPage.Clear();
		}
		ComponentsOnPage = null;
		if (SelectedTextComponentsOnPage != null)
		{
			SelectedTextComponentsOnPage.Clear();
		}
		SelectedTextComponentsOnPage = null;
		if (SelectedComponentsOnPage != null)
		{
			SelectedComponentsOnPage.Clear();
		}
		SelectedComponentsOnPage = null;
		if (RecentFiles != null)
		{
			RecentFiles.Clear();
		}
		if (Builder != null)
		{
			Builder.Dispose();
		}
		Builder = null;
		if (IsViewerPresent)
		{
			DisposeDesignerPreviewControl();
		}
		if (DictionaryNodes != null)
		{
			DictionaryNodes.Clear();
		}
		DictionaryNodes = null;
		if (Services != null)
		{
			Services.Clear();
		}
		Services = null;
		if (Report != null)
		{
			Report.Designer = null;
		}
		if (ribbonTabGroups != null && ribbonTabGroups.Count > 0)
		{
			foreach (RibbonTabItemGroup value in ribbonTabGroups.Values)
			{
				value.Dispose();
			}
		}
		report = null;
		StiTableElementClickHelper.TableHeaderClick -= StiTableElementClickHelper_TableHeaderClick;
		StiPivotTableElementClickHelper.TableHeaderClick -= StiPivotTableElementClickHelper_TableHeaderClick;
		StiComponentProgressHelper.Dispose();
		StiComponentProgressHelper.Tick -= StiElementProgressHelper_Tick;
		StiComponentProgressHelper.CompletedProgress -= StiElementProgressHelper_CompledProgressElement;
		StiOnlineMapRepaintHelper.Dispose();
		StiOnlineMapRepaintHelper.Tick -= StiElementMapRepaintHelper_Tick;
		StiGisMapRepaintHelper.Tick -= StiGisMapRepaintHelper_Tick;
	}

	private void DisposeDesignerPreviewControl()
	{
		if (DesignerPreviewControl?.ViewerControl != null)
		{
			DesignerPreviewControl.ViewerControl.Report = null;
		}
		if (DesignerPreviewControl != null)
		{
			DisposeComponent((Component)(object)DesignerPreviewControl);
		}
		DesignerPreviewControl = null;
	}

	public void SetStyleToComponent(StiComponent component)
	{
		if (Report != null && component != null)
		{
			if (component is IStiDashboardElementStyle stiDashboardElementStyle)
			{
				stiDashboardElementStyle.Style = CurrentDashboardStyleIdent;
			}
			CurrentStyle?.SetStyleToComponent(component);
		}
	}

	public void CheckClosing(CancelEventArgs e)
	{
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Invalid comparison between Unknown and I4
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		DisableTimerAutoSave();
		try
		{
			StiOptions.Engine.GlobalEvents.InvokeClosingDesigner(this, e);
			if (e.Cancel)
			{
				return;
			}
			if (TabControl.SelectedTab?.Tag is StiBuilder.Tabs tabs && tabs == StiBuilder.Tabs.Code)
			{
				Builder?.GetCodeEditor()?.CodeEditor?.SaveScript();
			}
			if (Report != null && !Report.IsPageDesigner)
			{
				e.Cancel = TestReportSave();
				if (!e.Cancel)
				{
					StiCacheCleaner.Clean(Report);
				}
			}
			else
			{
				DialogResult val;
				if (Report != null && Report.IsModified)
				{
					val = MessageBox.Show(StiLocalization.Get("Questions", "qnSaveChangesToPreviewPage"), StiOptions.Designer.GetDesignerTitle(), (MessageBoxButtons)3);
					if ((int)val == 2)
					{
						e.Cancel = true;
					}
				}
				else
				{
					val = (DialogResult)7;
				}
				Form val2 = ((Control)this).FindForm();
				if (val2 != null)
				{
					val2.DialogResult = val;
				}
			}
			SaveDesignerParam();
		}
		finally
		{
			if (!e.Cancel)
			{
				DisableTimerAutoSave();
			}
			else
			{
				SetAutoSaveMode();
			}
		}
	}

	internal void SaveReportBeforePreview()
	{
		if (AutoSaveReportBeforePreview)
		{
			StiActions.MainMenu.InvokeReportSave(this);
		}
	}

	internal void UpdateDesignerPreviewControl1(object sender, EventArgs e)
	{
		DesignerPreviewControl?.ViewerControl.SaveConfig();
	}

	internal void UpdateDesignerPreviewControl2(object sender, EventArgs e)
	{
		if (report != null)
		{
			Windows7Taskbar.ShowTaskbarProgress(report.Designer, Windows7Taskbar.ThumbnailProgressState.Indeterminate);
		}
		StiSettings.Load();
		AddRibbonViewerMenu();
		StiSettings.Save();
		DesignerPreviewControl.ButtonRefresh_Click(sender, e);
		Windows7Taskbar.HideTaskbarProgress();
	}

	internal void RefreshPanelUpdate()
	{
		string text = "";
		if (StiLicenseKeyValidator.IsTrial(StiLicense.LicenseKey))
		{
			text = StiLocalizationExt.Get("CheckLicense", "StiLicenseTrialCheckLong");
		}
		if (StiAccountSettings.CurrentAccount != null && string.IsNullOrEmpty(text) && !StiLicenseKeyValidator.IsValidOnAnyPlatform(StiLicense.LicenseKey))
		{
			text = StiLocalizationExt.Get("CheckLicense", "StiValidSubscriptioRequiredCheckLong");
		}
		((Control)panelUpdateLicense).Visible = !string.IsNullOrEmpty(text);
		((Control)labelSubscriptionRequired).Text = text;
	}

	private void UpdateLabel()
	{
		if (Report != null)
		{
			lbZoom.Text = (int)(Report.Info.Zoom * 100.0) + "%";
		}
	}

	internal bool CheckKeyTips()
	{
		if (RibbonControl != null)
		{
			return RibbonControl.RibbonStrip.InvokeSysKeyUpKeyTips();
		}
		return false;
	}

	public static string GetDefaultDesignerConfigPath()
	{
		string defaultDesignerConfigPath = StiDesigner.DefaultDesignerConfigPath;
		if (!string.IsNullOrEmpty(defaultDesignerConfigPath))
		{
			return defaultDesignerConfigPath;
		}
		defaultDesignerConfigPath = Path.Combine("Stimulsoft", "Stimulsoft.Report.Design.config");
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
		if (folderPath.Length != 0)
		{
			defaultDesignerConfigPath = Path.Combine(folderPath, defaultDesignerConfigPath);
		}
		folderPath = Path.GetDirectoryName(defaultDesignerConfigPath);
		if (!Directory.Exists(folderPath))
		{
			Directory.CreateDirectory(folderPath);
		}
		return defaultDesignerConfigPath;
	}

	public static string GetDefaultDockingPanelsConfigPath()
	{
		string defaultDockingPanelsConfigPath = StiDesigner.DefaultDockingPanelsConfigPath;
		if (!string.IsNullOrEmpty(defaultDockingPanelsConfigPath))
		{
			return defaultDockingPanelsConfigPath;
		}
		defaultDockingPanelsConfigPath = Path.Combine("Stimulsoft", "DesignerDockingPanels.config");
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
		if (!string.IsNullOrEmpty(folderPath))
		{
			defaultDockingPanelsConfigPath = Path.Combine(folderPath, defaultDockingPanelsConfigPath);
		}
		folderPath = Path.GetDirectoryName(defaultDockingPanelsConfigPath);
		if (!Directory.Exists(folderPath))
		{
			Directory.CreateDirectory(folderPath);
		}
		return defaultDockingPanelsConfigPath;
	}

	public static string GetDefaultRibbonDockingPanelsConfigPath()
	{
		string defaultRibbonDesignerConfigPath = StiDesigner.DefaultRibbonDesignerConfigPath;
		if (!string.IsNullOrEmpty(defaultRibbonDesignerConfigPath))
		{
			return defaultRibbonDesignerConfigPath;
		}
		defaultRibbonDesignerConfigPath = Path.Combine("Stimulsoft", "RibbonDesignerDockingPanels.config");
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
		if (folderPath.Length != 0)
		{
			defaultRibbonDesignerConfigPath = Path.Combine(folderPath, defaultRibbonDesignerConfigPath);
		}
		folderPath = Path.GetDirectoryName(defaultRibbonDesignerConfigPath);
		if (!Directory.Exists(folderPath))
		{
			Directory.CreateDirectory(folderPath);
		}
		return defaultRibbonDesignerConfigPath;
	}

	public static string GetDefaultRibbonQatConfigPath()
	{
		string defaultRibbonDesignerQatConfigPath = StiDesigner.DefaultRibbonDesignerQatConfigPath;
		if (!string.IsNullOrEmpty(defaultRibbonDesignerQatConfigPath))
		{
			return defaultRibbonDesignerQatConfigPath;
		}
		defaultRibbonDesignerQatConfigPath = Path.Combine("Stimulsoft", "RibbonDesignerQat.config");
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
		if (!string.IsNullOrEmpty(folderPath))
		{
			defaultRibbonDesignerQatConfigPath = Path.Combine(folderPath, defaultRibbonDesignerQatConfigPath);
		}
		folderPath = Path.GetDirectoryName(defaultRibbonDesignerQatConfigPath);
		if (!Directory.Exists(folderPath))
		{
			Directory.CreateDirectory(folderPath);
		}
		return defaultRibbonDesignerQatConfigPath;
	}

	private void RemoveRibbonViewerMenu()
	{
		if (!showViewerRibbonMenu)
		{
			return;
		}
		RibbonControl ribbonControl = RibbonControl;
		RibbonTabItem ribbonTabItem = (RibbonTabItem)ribbonControl.Items[0];
		((Control)ribbonControl).Controls.Remove((Control)(object)ribbonTabItem.Panel);
		ribbonControl.Items.Remove(ribbonTabItem);
		for (int i = 0; i < ribbonControl.Items.Count; i++)
		{
			if (ribbonControl.Items[i] is RibbonTabItem ribbonTabItem2)
			{
				ribbonTabItem2.Visible = true;
			}
			else if (ribbonControl.Items[i] is ButtonItem && i == 0)
			{
				ButtonItem buttonItem = ribbonControl.Items[i] as ButtonItem;
				buttonItem.Visible = true;
			}
		}
		if (ribbonControl.Items.Count > 0)
		{
			if (ribbonControl.Items[0] is RibbonTabItem)
			{
				((RibbonTabItem)ribbonControl.Items[0]).Select();
			}
			else if (ribbonControl.Items.Count > 1 && ribbonControl.Items[1] is RibbonTabItem)
			{
				((RibbonTabItem)ribbonControl.Items[1]).Select();
			}
		}
		showViewerRibbonMenu = false;
	}

	internal RibbonTabItem GetRibbonViewerTab()
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		Type type = Type.GetType("Stimulsoft.Report.Win.Ribbon.StiRibbonViewerUserControl, Stimulsoft.Report.Win, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a");
		if (type == null)
		{
			throw new Exception("Assembly 'Stimulsoft.Report.Win' is not found");
		}
		Control val = ((IEnumerable)((Control)DesignerPreviewControl.ViewerControl).Controls).Cast<Control>().FirstOrDefault();
		if (val == null)
		{
			return null;
		}
		UserControl val2 = (UserControl)StiActivator.CreateObject(type, new object[1] { val });
		RibbonPanel ribbonPanel = new RibbonPanel();
		RibbonTabItem ribbonTabItem = new RibbonTabItem
		{
			Panel = ribbonPanel,
			Text = StiLocalization.Get("FormDesigner", "Preview"),
			Tag = val2
		};
		ribbonTabItem.CustomColorName = "customHigh";
		((Control)ribbonPanel).Dock = (DockStyle)5;
		((Control)ribbonPanel).Padding = new Padding(3, 0, 3, 3);
		int count = ((ArrangedElementCollection)((Control)val2).Controls).Count;
		for (int i = 0; i < count; i++)
		{
			if (((Control)val2).Controls[0] is RibbonBar ribbonBar)
			{
				((Control)ribbonPanel).Controls.Add((Control)(object)ribbonBar);
			}
		}
		return ribbonTabItem;
	}

	private void AddRibbonViewerMenu()
	{
		if (StiOptions.Designer.Ribbon.ShowTabPreview)
		{
			return;
		}
		RibbonControl ribbonControl = RibbonControl;
		if (ribbonControl == null)
		{
			return;
		}
		RibbonControl ribbonControl2 = RibbonControl;
		for (int i = 0; i < ribbonControl2.Items.Count; i++)
		{
			if (ribbonControl2.Items[i] is RibbonTabItem ribbonTabItem)
			{
				ribbonTabItem.Visible = false;
			}
			else if (ribbonControl2.Items[i] is ButtonItem && i == 0)
			{
				ButtonItem buttonItem = ribbonControl2.Items[i] as ButtonItem;
				buttonItem.Visible = false;
			}
		}
		RibbonTabItem ribbonViewerTab = GetRibbonViewerTab();
		if (ribbonViewerTab != null)
		{
			((Control)ribbonControl2).Controls.Add((Control)(object)ribbonViewerTab.Panel);
			ribbonControl2.Items.Insert(0, ribbonViewerTab);
			ribbonViewerTab.Select();
			showViewerRibbonMenu = true;
			StiGuiHelper.ApplyStyle((Control)(object)ribbonControl2);
		}
	}

	private void LocalizeRibbonViewerMenu()
	{
		if (showViewerRibbonMenu)
		{
			RemoveRibbonViewerMenu();
			AddRibbonViewerMenu();
		}
	}

	internal void RefreshCheckHelperButton(ExtendedButtonItem button)
	{
		UpdateCheckHelperButton(button, 0, 0, 0, 0);
	}

	internal void UpdateCheckHelperButton()
	{
		UpdateCheckHelperButton(sbpReportChecker);
	}

	internal void UpdateCheckHelperButton(ExtendedButtonItem button, int errors, int warnings, int informationMessages, int renderingMessages)
	{
		List<Image> list = new List<Image>();
		List<string> list2 = new List<string>();
		if (errors > 0)
		{
			list.Add(StiReportDesignImages.Actions.Error());
			list2.Add(errors.ToString());
		}
		if (warnings > 0)
		{
			list.Add(StiReportDesignImages.Actions.Warning());
			list2.Add(warnings.ToString());
		}
		if (informationMessages > 0)
		{
			list.Add(StiReportDesignImages.Actions.Information());
			list2.Add(informationMessages.ToString());
		}
		if (renderingMessages > 0)
		{
			list.Add(StiReportDesignImages.Actions.RenderingMessage());
			list2.Add(renderingMessages.ToString());
		}
		button.AfterImages = list;
		button.AfterTexts = list2;
		button.RecalcSize();
		button.Parent?.RecalcSize();
		button.Parent.Refresh();
	}

	internal void UpdateCheckHelperButton(object sender)
	{
		StiDesignerControl stiDesignerControl = DesignerControl as StiDesignerControl;
		ExtendedButtonItem button = sender as ExtendedButtonItem;
		if (!((Control)stiDesignerControl).Visible && !Report.GetCurrentPage().IsDashboard && ((Control)this).FindForm() is StiRibbonDesigner stiRibbonDesigner)
		{
			button = stiRibbonDesigner.sbpReportCheckerViewer;
		}
		UpdateCheckHelperButton(button, CheckHelper.ErrorsCount, CheckHelper.WarningsCount, CheckHelper.InformationMessagesCount, CheckHelper.ReportRenderingMessagesCount);
		if (CheckHelper.ErrorsCount > 0 && !StiInplaceDesigner.AllForms.Any((StiInplaceEditorForm f) => f is StiChecksViewerForm))
		{
			RunReportChecker(button);
		}
	}

	internal void RunReportChecker(ExtendedButtonItem button)
	{
		if (codeTabSelected)
		{
			StiDesignerCodeControl codeEditor = Builder.GetCodeEditor();
			if (codeEditor != null && codeEditor.CodeEditor != null)
			{
				codeEditor.CodeEditor.SaveScript();
			}
		}
		StiInplaceDesigner.Close();
		StiCheckStatusDesigner stiCheckStatusDesigner = new StiCheckStatusDesigner(this);
		stiCheckStatusDesigner.DesignComponent(Report);
	}

	internal void ActivatePlacementComponentService(StiComponent comp, StiComponent draggingLabelComponent)
	{
		if (StiOptions.Designer.AllowUsePlacementWizard && comp != null && comp.Parent is StiBand && ((comp.Top < 0.0) | (comp.Bottom > comp.Parent.Height)))
		{
			PlacementComponentService.Activate(comp, draggingLabelComponent);
		}
	}

	internal void ActivateHighlightComponentService(StiText highlightedComp, StiText dragingComponent, StiComponent labelComp)
	{
		if (StiOptions.Designer.AllowUseHighlightWizard && HighlightComponentService != null)
		{
			HighlightComponentService.Activate(highlightedComp, dragingComponent, labelComp);
		}
	}

	private void StiTableElementClickHelper_TableHeaderClick(object sender, StiTableElementClickEventArgs e)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Invalid comparison between Unknown and I4
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Invalid comparison between Unknown and I4
		if (!(sender is IStiTableElement stiTableElement))
		{
			return;
		}
		IStiTableDashboardInteraction stiTableDashboardInteraction = ((stiTableElement is IStiElementInteraction stiElementInteraction) ? stiElementInteraction.DashboardInteraction : null) as IStiTableDashboardInteraction;
		bool flag = (Control.ModifierKeys & 0x20000) > 0;
		bool flag2 = (Control.ModifierKeys & 0x10000) > 0;
		if (flag || flag2)
		{
			if (stiTableDashboardInteraction == null || stiTableDashboardInteraction.AllowUserSorting)
			{
				List<StiDataSortRule> rules = stiTableElement.UserSorts;
				List<string> columnKeys = (from c in stiTableElement.FetchAllMeters()?.Select((IStiMeter c) => c?.Key)
					where c != null
					select c).ToList();
				StiDataSortDirection sortDirection = StiDataSortRuleHelper.GetSortDirection(rules, e.ColumnKey);
				sortDirection = ((sortDirection != StiDataSortDirection.Ascending) ? StiDataSortDirection.Ascending : StiDataSortDirection.Descending);
				if (flag2)
				{
					rules = new List<StiDataSortRule>();
				}
				stiTableElement.UserSorts = StiDataSortRuleHelper.SetSortDirection(rules, columnKeys, e.ColumnKey, sortDirection);
			}
		}
		else if (stiTableDashboardInteraction == null || stiTableDashboardInteraction.AllowUserSorting || stiTableDashboardInteraction.AllowUserFiltering)
		{
			lockStandardDesignerPopup = true;
			if (sortFilterContextMenu == null)
			{
				sortFilterContextMenu = new StiContextMenu();
			}
			if (sortFilterMenu == null)
			{
				sortFilterMenu = new StiTableElementSortFilterMenu((ContextMenuStrip)(object)sortFilterContextMenu, stiTableElement, ((Control)this).FindForm());
				sortFilterMenu.SortChanged += delegate
				{
					((Control)this).Refresh();
				};
				sortFilterMenu.FiltersChanged += delegate
				{
					((Control)this).Refresh();
				};
			}
			else
			{
				sortFilterMenu.TableElement = stiTableElement;
			}
			List<IStiMeter> meters = stiTableElement.GetMeters();
			sortFilterMenu.CreateItems(meters, meters.IndexOf(meters.FirstOrDefault((IStiMeter m) => m.Key == e.ColumnKey)), stiTableElement);
			StiPage currentPage = Report.GetCurrentPage();
			Point point = ((Control)PageView.GetCurrentPageView().View).PointToScreen(Point.Empty);
			sortFilterContextMenu.IsDarkMode = StiDashboardStyleHelper.IsDarkStyle(stiTableElement);
			((ToolStripDropDown)sortFilterContextMenu).Show((int)((double)point.X + e.Rect.X + currentPage.Zoom * currentPage.Margins.Left * StiScaleUI.Factor), (int)((double)point.Y + e.Rect.Y + e.Rect.Height + currentPage.Zoom * currentPage.Margins.Top * StiScaleUI.Factor));
		}
		StiPropertyGrid propertyGrid = StiPropertiesPanelHelper.GetPropertyGrid(this);
		IStiMeter stiMeter = stiTableElement.GetMeters().FirstOrDefault((IStiMeter c) => c.Key == e.ColumnKey);
		if (stiMeter != null)
		{
			StiPropertiesPanelHelper.ActivatePanel(this, stiMeter);
		}
	}

	private void StiPivotTableElementClickHelper_TableHeaderClick(object sender, StiPivotTableElementClickEventArgs e)
	{
		if (sender is IStiPivotTableElement stiPivotTableElement)
		{
			StiPropertyGrid propertyGrid = StiPropertiesPanelHelper.GetPropertyGrid(this);
			IStiMeter stiMeter = stiPivotTableElement.GetMeters().FirstOrDefault((IStiMeter c) => c.Key == e.MeterKey);
			if (stiMeter != null)
			{
				StiPropertiesPanelHelper.ActivatePanel(this, stiMeter);
			}
		}
	}

	private void StiElementProgressHelper_Tick(object sender, EventArgs e)
	{
		try
		{
			if (report == null)
			{
				return;
			}
			foreach (RectangleD item in from StiComponent c in StiComponentProgressHelper.FetchAllComponents()
				where c.Page != null && c.Report != null
				select c.GetDisplayRectangle())
			{
				RectangleD rectangleD = RectangleD.CreateFromRectangle(StiConvert.RectangleToScreen(this, item));
				Builder.Redraw(rectangleD.ToRectangle());
			}
		}
		catch
		{
		}
	}

	private void StiGisMapRepaintHelper_Tick(object sender, EventArgs e)
	{
		try
		{
			if (Report == null)
			{
				return;
			}
			List<object> list = StiGisMapRepaintHelper.FetchAllComponents();
			if (list == null)
			{
				return;
			}
			foreach (RectangleD item in from StiComponent c in list
				where c.Page != null && c.Report != null
				select c.GetDisplayRectangle())
			{
				RectangleD rectangleD = RectangleD.CreateFromRectangle(StiConvert.RectangleToScreen(this, item));
				Builder.Redraw(rectangleD.ToRectangle());
			}
		}
		catch
		{
		}
	}

	private void StiElementMapRepaintHelper_Tick(object sender, EventArgs e)
	{
		try
		{
			List<IStiAppComponent> list = StiOnlineMapRepaintHelper.FetchAllComponents();
			if (list == null)
			{
				return;
			}
			foreach (RectangleD item in from StiComponent c in list
				where c.Page != null && c.Report != null
				select c.GetDisplayRectangle())
			{
				RectangleD rectangleD = ((report == null) ? RectangleD.Empty : RectangleD.CreateFromRectangle(StiConvert.RectangleToScreen(this, item)));
				Builder.Redraw(rectangleD.ToRectangle());
			}
		}
		catch
		{
		}
	}

	private void StiElementProgressHelper_CompledProgressElement(object sender, EventArgs e)
	{
		try
		{
			if (sender is StiComponent stiComponent && Report != null)
			{
				RectangleD rectangleD = RectangleD.CreateFromRectangle(StiConvert.RectangleToScreen(this, stiComponent.GetDisplayRectangle()));
				Builder.Redraw(rectangleD.ToRectangle());
			}
		}
		catch
		{
		}
	}

	private void Form_Closing(object sender, CancelEventArgs e)
	{
		CheckClosing(e);
	}

	private void Form_Closed(object sender, EventArgs e)
	{
		StiOptions.Engine.GlobalEvents.InvokeClosedDesigner(this, EventArgs.Empty);
		DisposeDesigner();
		StiOptions.Windows.GlobalGuiStyleLoaded = false;
	}

	protected override void OnSizeChanged(EventArgs e)
	{
		((Control)this).OnSizeChanged(e);
		StiElementActionHelper.Hide();
	}

	protected override void OnParentChanged(EventArgs e)
	{
		((ContainerControl)this).OnParentChanged(e);
		if (oldParent != ((Control)this).Parent && oldParent != null)
		{
			Form val = oldParent.FindForm();
			if (val != null)
			{
				val.Closed -= Form_Closed;
				val.Closing -= Form_Closing;
			}
		}
		if (((Control)this).Parent != null)
		{
			Form val2 = ((Control)this).Parent.FindForm();
			if (val2 != null)
			{
				val2.Closed += Form_Closed;
				val2.Closing += Form_Closing;
			}
			oldParent = ((Control)this).Parent;
		}
	}

	private void TimerAutoSave_Tick(object sender, EventArgs e)
	{
		if (timerAutoSave != null)
		{
			timerAutoSave.Enabled = false;
			if (StiAccountSettings.CurrentAccount != null)
			{
				StiAccountSettings.CurrentAccount.Actions.SkipRequestChangesSaving = true;
			}
			StiActions.MainMenu.InvokeReportSave(this);
			if (StiAccountSettings.CurrentAccount != null)
			{
				StiAccountSettings.CurrentAccount.Actions.SkipRequestChangesSaving = false;
			}
			timerAutoSave.Enabled = true;
		}
	}

	protected override async void OnLoad(EventArgs e)
	{
		_003C_003En__0(e);
		Report?.Dictionary?.Connect(loadData: false);
		Builder?.BuildReport();
		if (cloudReport != null && StiAccountSettings.CurrentAccount != null)
		{
			if (StiAccountSettings.CurrentAccount.User != null)
			{
				await OpenRecentCloudFileAsync(cloudReport);
				cloudReport = null;
			}
			else
			{
				StiAccountSettings.CurrentAccount.UserInfoChanged += FirstInitAccount_UserInfoChanged;
			}
		}
	}

	private void This_ParentChanged(object sender, EventArgs e)
	{
		Form val = ((Control)this).FindForm();
		if (val != null)
		{
			val.Shown += Form_Shown;
		}
	}

	private void Form_Shown(object sender, EventArgs e)
	{
		DoScaling();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).OnPaint(e);
		if (firstPaint)
		{
			firstPaint = false;
			if ((Control.ModifierKeys & 0x10000) == 0)
			{
				LoadDesignerParam();
				DesignerConfig.LoadLayout();
			}
			RunStartTimer();
		}
	}

	private void TabControl_SelectedTabChanged(object sender, TabStripTabChangedEventArgs e)
	{
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Expected O, but got Unknown
		//IL_039e: Unknown result type (might be due to invalid IL or missing references)
		StiInplaceDesigner.Close();
		StiElementActionHelper.Hide();
		if (DesignerPreviewControl != null)
		{
			DesignerPreviewControl.ViewerControl.ResetShortcuts();
		}
		if (RibbonDesignerUserControl != null)
		{
			RibbonDesignerUserControl.InitShortcutOpenFile();
		}
		RemoveRibbonViewerMenu();
		if (codeTabSelected && TabControl.Tabs.Count > PageCodeIndex && e.OldTab == TabControl.Tabs[PageCodeIndex])
		{
			StiDesignerCodeControl codeEditor = Builder.GetCodeEditor();
			if (codeEditor != null)
			{
				codeEditor.CheckCodeControl();
				((Control)codeEditor.CodeEditor).Focus();
				codeEditor.CodeEditor.CloseEditor();
			}
			codeTabSelected = false;
		}
		if (!LockTabControlRefresh && Report != null)
		{
			if (TabControl.SelectedTabIndex != PagePreviewIndex)
			{
				Report.IsStopped = true;
			}
			if (TabControl.SelectedTabIndex >= PageCodeIndex)
			{
				Report.CurrentPage = 0;
			}
			else
			{
				Report.CurrentPage = TabControl.SelectedTabIndex;
			}
			if (TabControl.SelectedTabIndex == PageCodeIndex)
			{
				codeTabSelected = true;
				Builder.CheckCodeTab(showCodeTab: true);
				StiDesignerCodeControl codeEditor2 = Builder.GetCodeEditor();
				if (codeEditor2 != null)
				{
					codeEditor2.CheckCodeControl();
					((Control)codeEditor2.CodeEditor).Focus();
					codeEditor2.CodeEditor.OpenEditor();
				}
			}
			if (TabControl.SelectedTabIndex < PageCodeIndex)
			{
				Report.IsRendering = false;
				Report.ResetSelection();
				Report.GetCurrentPage().Select();
				AddSelectedToPropertyGrid(updateSelectedObjects: true);
				foreach (Control item in (ArrangedElementCollection)((Control)TabControl).Controls[TabControl.SelectedTabIndex].Controls)
				{
					Control val = item;
					if (val is StiPageViewControl)
					{
						val.Focus();
						break;
					}
				}
				InvokeBuidingToolbox();
			}
			SetToolboxEnabled(TabControl.SelectedTabIndex < PageCodeIndex);
			if (TabControl.SelectedTabIndex < PageCodeIndex)
			{
				if (IsViewerPresent)
				{
					UpdateDesignerPreviewControl1(sender, e);
				}
				Report.CompiledReport = null;
				Report.IsStopped = true;
			}
			else if (TabControl.SelectedTabIndex == PagePreviewIndex)
			{
				Control val2 = ((TabControl.SelectedTab.AttachedControl.Controls[0].Name == "dashboardViewerControl") ? ((Control)this).Controls[0] : null);
				if (val2 != null)
				{
					TabControl.SelectedTab.AttachedControl.Controls.RemoveAt(0);
				}
				if (StiDashboardHelper.IsDashboardPresent(Report))
				{
					UserControl viewerControl = StiDashboardViewerAssembly.GetViewerControl(Report, TabControl.SelectedTabIndex);
					if (viewerControl != null)
					{
						((Control)viewerControl).Dock = (DockStyle)5;
						((Control)viewerControl).Location = new Point(0, 0);
						((Control)viewerControl).Name = "dashboardViewerControl";
						((Control)viewerControl).Size = new Size(603, 449);
						((Control)viewerControl).TabIndex = 0;
						TabControl.SelectedTab.AttachedControl.Controls.Add((Control)(object)viewerControl);
						TabControl.SelectedTab.AttachedControl.Controls.SetChildIndex((Control)(object)viewerControl, 0);
					}
					else
					{
						MessageBox.Show((IWin32Window)(object)((Control)this).FindForm(), "Dashboard Viewer from the 'Stimulsoft.Dashboard.Viewer.dll' wasn't found!", "Designer");
					}
				}
				else if (IsViewerPresent)
				{
					SaveReportBeforePreview();
					UpdateDesignerPreviewControl2(sender, e);
					if (DesignerPreviewControl != null)
					{
						DesignerPreviewControl.ViewerControl.InitShortcuts();
					}
					if (RibbonDesignerUserControl != null)
					{
						RibbonDesignerUserControl.ResetShortcutOpenFile();
					}
				}
			}
			int num = TabControl.Tabs.IndexOf(e.OldTab);
			if (num == PagePreviewIndex && DesignerPreviewControl != null)
			{
				DesignerPreviewControl.ClearOldReport();
			}
			if (TabControl.SelectedTabIndex != PageCodeIndex)
			{
				((Control)this).Refresh();
			}
		}
		StiElementActionHelper.Hide();
	}

	private void ContextMenuDesigner_PopupOpen(object sender, PopupOpenEventArgs e)
	{
		StiComponentsCollection selected = GetSelected();
		if (lockStandardDesignerPopup)
		{
			lockStandardDesignerPopup = false;
			return;
		}
		bool flag = true;
		foreach (StiComponent item in selected)
		{
			if (!StiRestrictionsHelper.IsAllowChange(item))
			{
				flag = false;
				break;
			}
		}
		StiServiceContainer componentsContextServices = GetComponentsContextServices(selected[0]);
		if (componentsContextServices.Count <= 0)
		{
			return;
		}
		StiService[] array = componentsContextServices.ToArray();
		Hashtable hashtable = new Hashtable();
		StiStringCollection stiStringCollection = new StiStringCollection();
		StiService[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			StiCTService stiCTService = (StiCTService)array2[i];
			if (hashtable[stiCTService.GroupCategory] == null)
			{
				hashtable.Add(stiCTService.GroupCategory, stiCTService.GroupCategory);
				stiStringCollection.Add(stiCTService.GroupCategory);
			}
		}
		cmDesigner.SubItems.Clear();
		foreach (string item2 in stiStringCollection)
		{
			bool beginGroup = true;
			StiService[] array3 = array;
			for (int j = 0; j < array3.Length; j++)
			{
				StiCTService stiCTService2 = (StiCTService)array3[j];
				if (!(item2 != stiCTService2.GroupCategory))
				{
					stiCTService2.CreateMenuItem(this);
					cmDesigner.SubItems[cmDesigner.SubItems.Count - 1].BeginGroup = beginGroup;
					beginGroup = false;
				}
			}
		}
		Hashtable hashtable2 = new Hashtable();
		int num = 0;
		foreach (StiComponent item3 in selected)
		{
			PropertyDescriptorCollection propertiesFromComponent = StiPropertiesTab.GetPropertiesFromComponent(item3, null, (PropertySort)1, StiGuiMode.Gdi);
			Hashtable hashtable3 = new Hashtable();
			foreach (PropertyDescriptor item4 in propertiesFromComponent)
			{
				try
				{
					PropertyInfo property = item4.ComponentType.GetProperty(item4.Name, item4.PropertyType);
					if (property.GetCustomAttributes(typeof(StiShowInContextMenuAttribute), inherit: false) is StiShowInContextMenuAttribute[] array4 && array4.Length != 0 && (!(property.GetCustomAttributes(typeof(StiEngineAttribute), inherit: true) is StiEngineAttribute[] array5) || array5.Length == 0 || array5[0].Version == Report.EngineVersion || array5[0].Version == StiEngineVersion.All) && (!(property.GetCustomAttributes(typeof(BrowsableAttribute), inherit: true) is BrowsableAttribute[] array6) || array6.Length == 0 || array6[0].Browsable))
					{
						hashtable3[property.Name] = (bool)property.GetValue(item3, new object[0]);
					}
				}
				catch
				{
				}
			}
			if (num == 0)
			{
				hashtable2 = hashtable3;
			}
			else
			{
				Hashtable hashtable4 = new Hashtable();
				foreach (string key in hashtable2.Keys)
				{
					if (hashtable2[key] != null && hashtable3[key] != null)
					{
						if (hashtable2[key] != null)
						{
							hashtable4[key] = hashtable2[key];
						}
						else if (hashtable3[key] != null)
						{
							hashtable4[key] = hashtable3[key];
						}
					}
				}
				hashtable2 = hashtable4;
			}
			num++;
		}
		object[] array7 = new object[hashtable2.Keys.Count];
		hashtable2.Keys.CopyTo(array7, 0);
		Array.Sort(array7);
		int num2 = 0;
		object[] array8 = array7;
		for (int k = 0; k < array8.Length; k++)
		{
			string text2 = (string)array8[k];
			ButtonItem buttonItem = new ButtonItem();
			try
			{
				buttonItem.Text = StiLocalization.Get("PropertyMain", text2);
			}
			catch
			{
				buttonItem.Text = text2;
			}
			buttonItem.Checked = (bool)hashtable2[text2];
			buttonItem.Click += MenuItemShowInContextMenu_Click;
			buttonItem.Tag = text2;
			buttonItem.BeginGroup = num2 == 0;
			buttonItem.AutoCheckOnClick = true;
			buttonItem.AutoCollapseOnClick = false;
			cmDesigner.SubItems.Add(buttonItem);
			num2++;
		}
		if (flag)
		{
			return;
		}
		foreach (ButtonItem subItem in cmDesigner.SubItems)
		{
			subItem.Enabled = false;
		}
	}

	private void MenuItemShowInContextMenu_Click(object sender, EventArgs e)
	{
		StiComponentsCollection selected = GetSelected();
		ButtonItem buttonItem = sender as ButtonItem;
		UndoRedoSave(buttonItem.Text, onlyCurrentPage: true);
		foreach (StiComponent item in selected)
		{
			try
			{
				PropertyInfo property = item.GetType().GetProperty(buttonItem.Tag as string, BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
				property.SetValue(item, buttonItem.Checked, new object[0]);
				if (Report.GetCurrentPage().IsDashboard)
				{
					Builder.Redraw();
				}
			}
			catch
			{
			}
		}
		InvokeSelectedObjectsChanged();
	}

	private void ContextMenuPage_PopupOpen(object sender, PopupOpenEventArgs e)
	{
		StiMainMenuService service = StiMainMenuService.GetService(this);
		if (service == null)
		{
			return;
		}
		int num = 0;
		int num2 = -1;
		foreach (TabItem tab in TabControl.Tabs)
		{
			if (tab.IsMouseOver)
			{
				num2 = num;
				break;
			}
			num++;
		}
		if (Report != null && Report.Pages.Count > num2)
		{
			ClickedPageIndex = num2;
		}
		else
		{
			ClickedPageIndex = -1;
		}
		bool @bool = StiSettings.GetBool("Designer", "ShowDialogFormsV1", nullValue: false);
		ciPageNew.Visible = service.ShowFilePageNew && StiOptions.Designer.TabsContextMenu.ShowPageNew;
		ciFormNew.Visible = service.ShowFileFormNew && StiOptions.Designer.TabsContextMenu.ShowFormNew && @bool;
		ciScreenNew.Visible = service.ShowFileScreenNew && StiOptions.Designer.TabsContextMenu.ShowScreenNew && StiAppDesignAssembly.IsAssemblyLoaded;
		ciDashboardNew.Visible = service.ShowFileDashboardNew && StiOptions.Designer.TabsContextMenu.ShowDashboardNew && StiDashboardDesignAssembly.IsAssemblyLoaded;
		ciPageSetup.Visible = service.ShowFilePageSetup && StiOptions.Designer.TabsContextMenu.ShowPageSetup;
		ciPageDelete.Visible = service.ShowFilePageDelete && StiOptions.Designer.TabsContextMenu.ShowPageDelete;
		ciPageViewQuery.Visible = ClickedPageIndex != -1 && Report != null && Report.Pages[ClickedPageIndex].IsDashboard;
		ciPageDuplicate.Visible = StiOptions.Designer.TabsContextMenu.ShowPageClone;
		ciPageRename.Visible = StiOptions.Designer.TabsContextMenu.ShowPageRename;
		ciPageMoveLeft.Visible = StiOptions.Designer.TabsContextMenu.ShowPageMoveLeft;
		ciPageMoveRight.Visible = StiOptions.Designer.TabsContextMenu.ShowPageMoveRight;
		ciPageSetup.Visible = service.ShowFilePageSetup && StiOptions.Designer.TabsContextMenu.ShowPageSetup;
		ciPageOpen.Visible = service.ShowFilePageOpen && StiOptions.Designer.TabsContextMenu.ShowPageOpen;
		ciPageSave.Visible = service.ShowFilePageSaveAs && StiOptions.Designer.IsDeveloperDesigner && StiOptions.Designer.TabsContextMenu.ShowPageSave;
		StiSettings.Load();
		bool bool2 = StiSettings.GetBool("StiDesignerOptions", "ShowCodeTab", nullValue: false);
		ciShowCodeTab.Visible = StiOptions.Designer.IsCodeTabVisible;
		ciShowCodeTab.Checked = bool2;
		if (!ciPageNew.Visible && !ciFormNew.Visible && !ciScreenNew.Visible && !ciDashboardNew.Visible && !ciPageSetup.Visible && !ciPageOpen.Visible && !ciPageSave.Visible && !ciPageDelete.Visible && !ciPageDuplicate.Visible && !ciPageRename.Visible && !ciPageViewQuery.Visible && !ciPageMoveLeft.Visible && !ciPageMoveRight.Visible && !ciPageSetup.Visible)
		{
			e.Cancel = true;
		}
		int num3 = ClickedPageIndex;
		if (num3 == -1)
		{
			num3 = TabControl.SelectedTabIndex;
		}
		ciPageNew.Enabled = Report != null;
		ciFormNew.Enabled = Report != null;
		ciScreenNew.Enabled = Report != null;
		ciDashboardNew.Enabled = Report != null;
		ciPageDelete.Enabled = Report != null && Report.Pages.Count > 1 && num3 < Report.Pages.Count;
		ciPageSetup.Enabled = Report != null && num3 < Report.Pages.Count;
		ciPageViewQuery.Enabled = Report != null && num3 < Report.Pages.Count;
		ciPageMoveLeft.Enabled = Report != null && num3 > 0 && num3 < Report.Pages.Count;
		ciPageMoveRight.Enabled = Report != null && num3 < Report.Pages.Count - 1;
		ciPageDuplicate.Enabled = Report != null && num3 < Report.Pages.Count;
		ciPageRename.Enabled = Report != null && num3 < Report.Pages.Count;
		ciPageOpen.Enabled = Report != null;
		ciPageSave.Enabled = Report != null;
		ciShowCodeTab.Enabled = Report != null;
		ciPageSetup.BeginGroup = ciPageNew.Visible || ciFormNew.Visible || ciScreenNew.Visible || ciDashboardNew.Visible || ciPageDelete.Visible;
	}

	private void ButtonItemShowCodeTab_Click(object sender, EventArgs e)
	{
		StiSettings.Load();
		StiSettings.Set("StiDesignerOptions", "ShowCodeTab", ciShowCodeTab.Checked);
		StiSettings.Save();
		Builder.BuildReport();
	}

	private void ButtonUpdate_Click(object sender, EventArgs e)
	{
		StiProcess.Start("https://www.stimulsoft.com/en/online-store");
	}

	private async void FirstInitAccount_UserInfoChanged(object sender, EventArgs e)
	{
		StiAccountSettings.CurrentAccount.UserInfoChanged -= FirstInitAccount_UserInfoChanged;
		if (cloudReport != null)
		{
			await OpenRecentCloudFileAsync(cloudReport);
			cloudReport = null;
		}
	}

	private void CurrentAccount_UserInfoChanged(object sender, EventArgs e)
	{
		RefreshPanelUpdate();
	}

	private void Actions_OpenReportStart(object sender, EventArgs e)
	{
		StiDictionaryPanelHelper.ActivatePanel(this);
		StiDictionaryPanelHelper.ShowProgress(this, Loc.Get("DesignerFx", "LoadingReport") + "... 0%");
	}

	private void Actions_OpenReportCompleted(object sender, EventArgs e)
	{
		StiDictionaryPanelHelper.HideProgress(this);
	}

	private void Actions_OpenReportProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		StiDictionaryPanelHelper.UpdateMessageProgress(this, string.Format("{0}... {1}%", Loc.Get("DesignerFx", "LoadingReport"), e.ProgressPercentage));
	}

	private void Actions_SaveReportCompleted(object sender, EventArgs e)
	{
	}

	private void Actions_SaveReportStart(object sender, EventArgs e)
	{
	}

	public void ButtonReportChecker_Click(object sender, EventArgs e)
	{
		RunReportChecker(sender as ExtendedButtonItem);
	}

	private void StiReportChecker_GoToCode(object sender, EventArgs e)
	{
		if (popup != null)
		{
			((ToolStripDropDown)popup).Close();
		}
	}

	private void ButtonCentimeters_Click(object sender, EventArgs e)
	{
		Report.Unit = new StiCentimetersUnit();
		((Control)this).Refresh();
		InvokeSelectedObjectsChanged();
	}

	private void ButtonHundredthsOfInch_Click(object sender, EventArgs e)
	{
		Report.Unit = new StiHundredthsOfInchUnit();
		((Control)this).Refresh();
		InvokeSelectedObjectsChanged();
	}

	private void ButtonInches_Click(object sender, EventArgs e)
	{
		Report.Unit = new StiInchesUnit();
		((Control)this).Refresh();
		InvokeSelectedObjectsChanged();
	}

	private void ButtonMillimeters_Click(object sender, EventArgs e)
	{
		Report.Unit = new StiMillimetersUnit();
		((Control)this).Refresh();
		InvokeSelectedObjectsChanged();
	}

	private void ButtonUnit_PopupOpen(object sender, PopupOpenEventArgs e)
	{
		tbCentimeters.Checked = Report.Unit is StiCentimetersUnit;
		tbHundredthsOfInch.Checked = Report.Unit is StiHundredthsOfInchUnit;
		tbInches.Checked = Report.Unit is StiInchesUnit;
		tbMillimeters.Checked = Report.Unit is StiMillimetersUnit;
		tbCentimeters.Enabled = true;
		tbHundredthsOfInch.Enabled = true;
		tbInches.Enabled = true;
		tbMillimeters.Enabled = true;
	}

	private void ButtonZoomPageWidth_Click(object sender, EventArgs e)
	{
		StiActions.Zoom.SetZoomPageWidth(this);
	}

	private void ButtonZoomPageHeight_Click(object sender, EventArgs e)
	{
		StiActions.Zoom.SetZoomPageHeight(this);
	}

	private void ButtonZoomOnePage_Click(object sender, EventArgs e)
	{
		StiActions.Zoom.SetZoomOnePage(this);
	}

	private void ButtonZoom100_Click(object sender, EventArgs e)
	{
		StiActions.Zoom.SetZoom100(this);
	}

	private void SliderZoom_ValueChanged(object sender, EventArgs e)
	{
		if (!lockZoom)
		{
			UpdateLabel();
			if (Report != null)
			{
				timerZoomSlider.Enabled = true;
			}
		}
	}

	private void TimerZoomSlider_Tick(object sender, EventArgs e)
	{
		if (Report != null)
		{
			Report.Info.Zoom = (float)siZoom.Value / 100f;
			timerZoomSlider.Enabled = false;
			Builder.InvalidateRulers();
			Builder.BuildReport();
		}
	}

	private void ContextMenuPage_Click(object sender, EventArgs e)
	{
		if (PageView.GetCurrentPageView() != null)
		{
			int pageIndex = PageView.GetCurrentPageView().PageIndex;
			int num = pageIndex + 1;
			StiPage page = ((num >= Report.Pages.Count) ? Report.Pages[0] : Report.Pages[num]);
			SelectTabPage(page);
		}
	}

	private void PanelUpdateLicense_Paint(object sender, PaintEventArgs e)
	{
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		DateTime now = DateTime.Now;
		bool flag = (now - StiVersion.Created).Days < 30;
		((Control)labelSubscriptionRequired).Location = (flag ? new Point(StiScaleUI.I(28), StiScaleUI.I8) : new Point(StiScaleUI.I10, StiScaleUI.I8));
		if (!flag)
		{
			return;
		}
		Panel val = (Panel)((sender is Panel) ? sender : null);
		Graphics graphics = e.Graphics;
		Rectangle rect = new Rectangle(0, 0, ((Control)val).Height, ((Control)val).Height);
		rect.Inflate(-StiScaleUI.I4, -StiScaleUI.I4);
		if (rect.Contains(((Control)val).PointToClient(Cursor.Position)))
		{
			StiDrawing.FillRectangle(graphics, StiColor.Get("d4e0f1"), rect);
		}
		Pen val2 = new Pen(Color.Black, (float)StiScale.I2);
		try
		{
			SmoothingMode smoothingMode = graphics.SmoothingMode;
			graphics.SmoothingMode = (SmoothingMode)4;
			int num = (int)((double)((Control)val).Height / 2.7);
			int i = StiScaleUI.I1;
			graphics.DrawLine(val2, num - i, num - i, ((Control)val).Height - num - i, ((Control)val).Height - num - i);
			graphics.DrawLine(val2, num - i, ((Control)val).Height - num - i, ((Control)val).Height - num - i, num - i);
			graphics.SmoothingMode = smoothingMode;
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	private void PanelUpdateLicense_MouseMove(object sender, MouseEventArgs e)
	{
		((Control)panelUpdateLicense).Invalidate();
	}

	private void PanelUpdateLicense_MouseLeave(object sender, EventArgs e)
	{
		((Control)panelUpdateLicense).Invalidate();
	}

	private void PanelUpdateLicense_MouseClick(object sender, MouseEventArgs e)
	{
		DateTime now = DateTime.Now;
		if ((now - StiVersion.Created).Days < 30)
		{
			Panel val = (Panel)((sender is Panel) ? sender : null);
			Rectangle rectangle = new Rectangle(0, 0, ((Control)val).Height, ((Control)val).Height);
			rectangle.Inflate(-StiScale.I4, -StiScale.I4);
			if (rectangle.Contains(e.Location))
			{
				((Control)panelUpdateLicense).Visible = false;
			}
		}
	}

	private void This_RefreshStatusBar(object sender, EventArgs e)
	{
		if (Report == null)
		{
			sbpComponent.Text = string.Empty;
			sbpLocation.Text = string.Empty;
			sbpUnit.Text = string.Empty;
			sbpUnit.Enabled = false;
			sbpReportChecker.Enabled = false;
			return;
		}
		sbpUnit.Enabled = true;
		if (IsFormSelected)
		{
			sbpUnit.Text = StiLocalization.Get("PropertyEnum", "StiReportUnitTypePixels");
		}
		else if (IsDashboardSelected)
		{
			sbpUnit.Text = StiLocalization.Get("PropertyEnum", "StiMapTypePoints");
		}
		else
		{
			sbpUnit.Text = Report.GetCurrentPage().Unit.ToString();
		}
		sbpReportChecker.Enabled = true;
		if (SelectedObject != null)
		{
			object selectedObject = SelectedObject;
			if (selectedObject is StiReport)
			{
				sbpComponent.Text = ((StiReport)selectedObject).ReportName;
			}
			if (selectedObject is StiComponent)
			{
				sbpComponent.Text = ((StiComponent)selectedObject).Name;
			}
		}
		if (CurrentTool != null)
		{
			sbpLocation.Text = CurrentTool.GetStatusBarInformation();
		}
	}

	private void This_Load(object sender, EventArgs e)
	{
		RefreshPanelUpdate();
	}

	private void This_FirstLoad(object sender, EventArgs e)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		((UserControl)this).Load -= This_FirstLoad;
		Timer val = new Timer
		{
			Interval = 3000
		};
		val.Tick += TrialTimer_Tick;
		val.Start();
	}

	private void TrialTimer_Tick(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		((Timer)sender).Stop();
		if (StiDesignerAppStatus.IsRunning || StiLicenseKey.Get(StiLicense.Key) != null || ((Component)this).DesignMode)
		{
			return;
		}
		TimeSpan timeSpan = DateTime.Now - StiVersion.Created;
		if (timeSpan.Days <= 60)
		{
			return;
		}
		StiUsingTrialVersionForm stiUsingTrialVersionForm = new StiUsingTrialVersionForm(timeSpan.Days);
		try
		{
			AddOwnedForm((Form)(object)stiUsingTrialVersionForm);
			((Form)stiUsingTrialVersionForm).ShowDialog();
		}
		finally
		{
			((IDisposable)stiUsingTrialVersionForm)?.Dispose();
		}
	}

	private void This_MouseWheel(object sender, MouseEventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Invalid comparison between Unknown and I4
		if (report == null || TabControl.SelectedTabIndex >= PageCodeIndex || PageView.GetCurrentPageView().View == null || !((Control)PageView.GetCurrentPageView().View).Focused)
		{
			return;
		}
		if ((Control.ModifierKeys & 0x20000) > 0)
		{
			double num = Report.GetCurrentPage().Zoom * 1000.0;
			num += (double)e.Delta * Report.GetCurrentPage().Zoom;
			num = Math.Round(num / 1000.0, 2);
			if (num < 0.1)
			{
				num = 0.1;
			}
			if (num > 10.0)
			{
				num = 10.0;
			}
			SetZoom((float)num);
			((Control)this).Refresh();
		}
		else
		{
			StiVScrollBar vScrollBar = PageView.GetCurrentPageView().VScrollBar;
			if (((Control)vScrollBar).Visible)
			{
				int num2 = ((ScrollBar)vScrollBar).Value - e.Delta;
				if (num2 < 0)
				{
					num2 = 0;
				}
				if (num2 + ((ScrollBar)vScrollBar).LargeChange > ((ScrollBar)vScrollBar).Maximum)
				{
					num2 = ((ScrollBar)vScrollBar).Maximum - ((ScrollBar)vScrollBar).LargeChange;
				}
				((ScrollBar)vScrollBar).Value = num2;
			}
		}
		StiInplaceDesigner.Close();
		StiElementActionHelper.Hide();
	}

	private void This_RefreshDesigner(object sender, EventArgs e)
	{
		lockZoom = true;
		if (Report != null)
		{
			UpdateLabel();
			siZoom.Value = (int)(Report.Info.Zoom * 100.0);
			siZoom.Enabled = true;
			biZoom100.Enabled = true;
			biZoomOnePage.Enabled = true;
			biZoomPageWidth.Enabled = true;
			biZoomPageHeight.Enabled = true;
			sbpUnit.Enabled = true;
		}
		else
		{
			siZoom.Enabled = false;
			biZoom100.Enabled = false;
			biZoomOnePage.Enabled = false;
			biZoomPageWidth.Enabled = false;
			biZoomPageHeight.Enabled = false;
			sbpUnit.Enabled = false;
		}
		sbpUnit.Visible = StiOptions.Designer.Toolbars.StatusBar.ShowUnits;
		sbpReportChecker.Visible = StiOptions.Designer.Toolbars.StatusBar.ShowReportChecker;
		sbpLocation.Visible = StiOptions.Designer.Toolbars.StatusBar.ShowLocation;
		sbpComponent.Visible = StiOptions.Designer.Toolbars.StatusBar.ShowComponentName;
		if (IsDashboardSelected)
		{
			sbpUnit.Visible = false;
		}
		lockZoom = false;
	}

	protected override void Dispose(bool disposing)
	{
		DisposeDesigner();
		if (disposing && components != null)
		{
			components.Dispose();
		}
		LockRefreshDesigner = true;
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Expected O, but got Unknown
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Expected O, but got Unknown
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Expected O, but got Unknown
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Expected O, but got Unknown
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_040e: Expected O, but got Unknown
		//IL_07a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07af: Expected O, but got Unknown
		//IL_091c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0926: Expected O, but got Unknown
		//IL_0988: Unknown result type (might be due to invalid IL or missing references)
		//IL_0992: Expected O, but got Unknown
		//IL_10d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_10da: Expected O, but got Unknown
		//IL_1263: Unknown result type (might be due to invalid IL or missing references)
		//IL_126d: Expected O, but got Unknown
		//IL_156c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1576: Expected O, but got Unknown
		//IL_176f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1779: Expected O, but got Unknown
		//IL_17e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_17f0: Expected O, but got Unknown
		//IL_185d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1867: Expected O, but got Unknown
		//IL_18d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_18de: Expected O, but got Unknown
		//IL_1b25: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b2f: Expected O, but got Unknown
		//IL_1b3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b46: Expected O, but got Unknown
		//IL_1b6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b74: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiDesignerControl));
		ToolTip = new ToolTip(components);
		timerAutoSave = new Timer(components);
		TabControl = new TabControl();
		ContextMenuBar = new ContextMenuBar();
		cmDesigner = new ButtonItem();
		cmPageContextMenu = new ButtonItem();
		ciPageNew = new ButtonItem();
		ciDashboardNew = new ButtonItem();
		ciFormNew = new ButtonItem();
		ciScreenNew = new ButtonItem();
		ciPageDuplicate = new ButtonItem();
		ciPageRename = new ButtonItem();
		ciPageDelete = new ButtonItem();
		ciPageViewQuery = new ButtonItem();
		ciPageMoveLeft = new ButtonItem();
		ciPageMoveRight = new ButtonItem();
		ciPageSetup = new ButtonItem();
		ciPageOpen = new ButtonItem();
		ciPageSave = new ButtonItem();
		ciShowCodeTab = new ButtonItem();
		cmQuickButtons = new ButtonItem();
		DotNetBarManager = new DotNetBarManager(components);
		DockSiteBottom = new DockSite();
		DockSiteLeft = new DockSite();
		DockSiteRight = new DockSite();
		DockSiteBottomBars = new DockSite();
		DockSiteLeftBars = new DockSite();
		DockSiteRightBars = new DockSite();
		DockSiteTopBars = new DockSite();
		MainMenu = new Bar();
		DockSiteTop = new DockSite();
		buttonItem1 = new ButtonItem();
		bar1 = new Bar();
		sbpUnit = new ButtonItem();
		tbCentimeters = new ButtonItem();
		tbHundredthsOfInch = new ButtonItem();
		tbInches = new ButtonItem();
		tbMillimeters = new ButtonItem();
		sbpReportChecker = new ExtendedButtonItem();
		sbpComponent = new ButtonItem();
		sbpLocation = new LabelItem();
		labelItem1 = new LabelItem();
		icZoom = new ItemContainer();
		icPageViewMode = new ItemContainer();
		biZoomPageWidth = new ButtonItem();
		biZoomPageHeight = new ButtonItem();
		biZoomOnePage = new ButtonItem();
		biZoom100 = new ButtonItem();
		siZoom = new SliderItem();
		liSeparator2 = new LabelItem();
		lbZoom = new LabelItem();
		timerZoomSlider = new Timer(components);
		StyleManager = new StyleManager(components);
		panelUpdateLicense = new StiSubscriptionPanel();
		buttonUpdate = new Button();
		labelSubscriptionRequired = new Label();
		((ISupportInitialize)TabControl).BeginInit();
		((ISupportInitialize)ContextMenuBar).BeginInit();
		DockSiteTopBars.SuspendLayout();
		((ISupportInitialize)MainMenu).BeginInit();
		((ISupportInitialize)bar1).BeginInit();
		((Control)panelUpdateLicense).SuspendLayout();
		((Control)this).SuspendLayout();
		timerAutoSave.Tick += TimerAutoSave_Tick;
		((Control)TabControl).BackColor = Color.FromArgb(191, 219, 255);
		TabControl.CanReorderTabs = false;
		((Control)TabControl).Dock = (DockStyle)5;
		((Control)TabControl).Location = new Point(0, 67);
		((Control)TabControl).Name = "TabControl";
		TabControl.SelectedTabFont = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)1);
		TabControl.SelectedTabIndex = -1;
		((Control)TabControl).Size = new Size(1600, 916);
		TabControl.Style = eTabStripStyle.Office2007Dock;
		((Control)TabControl).TabIndex = 2;
		TabControl.TabLayoutType = eTabLayoutType.MultilineNoNavigationBox;
		((Control)TabControl).Text = "tabControl1";
		TabControl.SelectedTabChanged += TabControl_SelectedTabChanged;
		ContextMenuBar.DockTabStripHeight = 50;
		((Control)ContextMenuBar).Font = new Font("Segoe UI", 9f);
		ContextMenuBar.Items.AddRange(new BaseItem[3] { cmDesigner, cmPageContextMenu, cmQuickButtons });
		ContextMenuBar.Location = new Point(199, 162);
		ContextMenuBar.Name = "ContextMenuBar";
		ContextMenuBar.Size = new Size(145, 135);
		ContextMenuBar.Stretch = true;
		ContextMenuBar.Style = eDotNetBarStyle.Office2007;
		ContextMenuBar.TabIndex = 1;
		ContextMenuBar.TabStop = false;
		((Control)ContextMenuBar).Text = "ContextMenuBar";
		cmDesigner.AutoExpandOnClick = true;
		cmDesigner.ImagePaddingHorizontal = 16;
		cmDesigner.ImagePaddingVertical = 12;
		cmDesigner.Name = "cmDesigner";
		cmDesigner.Text = "cmDesigner";
		cmDesigner.PopupOpen += ContextMenuDesigner_PopupOpen;
		cmPageContextMenu.AutoExpandOnClick = true;
		cmPageContextMenu.ImagePaddingHorizontal = 16;
		cmPageContextMenu.ImagePaddingVertical = 12;
		cmPageContextMenu.Name = "cmPageContextMenu";
		cmPageContextMenu.Shortcuts.Add(eShortcut.CtrlTab);
		cmPageContextMenu.SubItems.AddRange(new BaseItem[14]
		{
			ciPageNew, ciDashboardNew, ciScreenNew, ciFormNew, ciPageDuplicate, ciPageRename, ciPageDelete, ciPageViewQuery, ciPageMoveLeft, ciPageMoveRight,
			ciPageSetup, ciPageOpen, ciPageSave, ciShowCodeTab
		});
		cmPageContextMenu.Text = "cmPageContextMenu";
		cmPageContextMenu.PopupOpen += ContextMenuPage_PopupOpen;
		cmPageContextMenu.Click += ContextMenuPage_Click;
		ciPageNew.ImagePaddingHorizontal = 16;
		ciPageNew.ImagePaddingVertical = 12;
		ciPageNew.Name = "ciPageNew";
		ciPageNew.Text = "Page New";
		ciPageNew.Click += OnPageNewClick;
		ciDashboardNew.ImagePaddingHorizontal = 16;
		ciDashboardNew.ImagePaddingVertical = 12;
		ciDashboardNew.Name = "ciDashboardNew";
		ciDashboardNew.Text = "Dashboard New";
		ciDashboardNew.Click += OnDashboardNewClick;
		ciScreenNew.ImagePaddingHorizontal = 16;
		ciScreenNew.ImagePaddingVertical = 12;
		ciScreenNew.Name = "ciScreenNew";
		ciScreenNew.Text = "Screen New";
		ciScreenNew.Click += OnScreenNewClick;
		ciFormNew.ImagePaddingHorizontal = 16;
		ciFormNew.ImagePaddingVertical = 12;
		ciFormNew.Name = "ciFormNew";
		ciFormNew.Text = "Form New";
		ciFormNew.Click += OnFormNewClick;
		ciPageDuplicate.BeginGroup = true;
		ciPageDuplicate.Image = (Image)componentResourceManager.GetObject("ciPageDuplicate.Image");
		ciPageDuplicate.ImagePaddingHorizontal = 16;
		ciPageDuplicate.ImagePaddingVertical = 12;
		ciPageDuplicate.Name = "ciPageDuplicate";
		ciPageDuplicate.Text = "Duplicate";
		ciPageDuplicate.Click += OnPageDuplicateClick;
		ciPageRename.ImagePaddingHorizontal = 16;
		ciPageRename.ImagePaddingVertical = 12;
		ciPageRename.Name = "ciPageRename";
		ciPageRename.Text = "Rename";
		ciPageRename.Click += OnPageRenameClick;
		ciPageDelete.ImagePaddingHorizontal = 16;
		ciPageDelete.ImagePaddingVertical = 12;
		ciPageDelete.Name = "ciPageDelete";
		ciPageDelete.Text = "Page Delete";
		ciPageDelete.Click += OnPageDeleteClick;
		ciPageViewQuery.BeginGroup = true;
		ciPageViewQuery.ImagePaddingHorizontal = 16;
		ciPageViewQuery.ImagePaddingVertical = 12;
		ciPageViewQuery.Name = "ciPageViewQuery";
		ciPageViewQuery.Text = "Move Left";
		ciPageViewQuery.Click += OnPageViewQueryClick;
		ciPageMoveLeft.BeginGroup = true;
		ciPageMoveLeft.Image = (Image)componentResourceManager.GetObject("ciPageMoveLeft.Image");
		ciPageMoveLeft.ImagePaddingHorizontal = 16;
		ciPageMoveLeft.ImagePaddingVertical = 12;
		ciPageMoveLeft.Name = "ciPageMoveLeft";
		ciPageMoveLeft.Text = "Move Left";
		ciPageMoveLeft.Click += OnPageMoveLeftClick;
		ciPageMoveRight.Image = (Image)componentResourceManager.GetObject("ciPageMoveRight.Image");
		ciPageMoveRight.ImagePaddingHorizontal = 16;
		ciPageMoveRight.ImagePaddingVertical = 12;
		ciPageMoveRight.Name = "ciPageMoveRight";
		ciPageMoveRight.Text = "Move Right";
		ciPageMoveRight.Click += OnPageMoveRightClick;
		ciPageSetup.BeginGroup = true;
		ciPageSetup.ImagePaddingHorizontal = 16;
		ciPageSetup.ImagePaddingVertical = 12;
		ciPageSetup.Name = "ciPageSetup";
		ciPageSetup.Text = "Page Setup";
		ciPageSetup.Click += OnPageSetupClick;
		ciPageOpen.BeginGroup = true;
		ciPageOpen.Image = (Image)(object)Resources.ReportOpen;
		ciPageOpen.ImagePaddingHorizontal = 16;
		ciPageOpen.ImagePaddingVertical = 12;
		ciPageOpen.Name = "ciPageOpen";
		ciPageOpen.Text = "Open Page";
		ciPageOpen.Click += OnPageOpen_Click;
		ciPageSave.Image = (Image)(object)Resources.ReportSave;
		ciPageSave.ImagePaddingHorizontal = 16;
		ciPageSave.ImagePaddingVertical = 12;
		ciPageSave.Name = "ciPageSave";
		ciPageSave.Text = "Save Page";
		ciPageSave.Click += OnPageSave_Click;
		ciShowCodeTab.AutoCheckOnClick = true;
		ciShowCodeTab.AutoCollapseOnClick = false;
		ciShowCodeTab.BeginGroup = true;
		ciShowCodeTab.ImagePaddingHorizontal = 16;
		ciShowCodeTab.ImagePaddingVertical = 12;
		ciShowCodeTab.Name = "ciShowCodeTab";
		ciShowCodeTab.Text = "Show Code Tab";
		ciShowCodeTab.Click += ButtonItemShowCodeTab_Click;
		cmQuickButtons.AutoExpandOnClick = true;
		cmQuickButtons.ImagePaddingHorizontal = 16;
		cmQuickButtons.ImagePaddingVertical = 12;
		cmQuickButtons.Name = "cmQuickButtons";
		cmQuickButtons.Text = "cmQuickButtons";
		DotNetBarManager.AutoDispatchShortcuts.Add(eShortcut.F1);
		DotNetBarManager.AutoDispatchShortcuts.Add(eShortcut.CtrlC);
		DotNetBarManager.AutoDispatchShortcuts.Add(eShortcut.CtrlA);
		DotNetBarManager.AutoDispatchShortcuts.Add(eShortcut.CtrlV);
		DotNetBarManager.AutoDispatchShortcuts.Add(eShortcut.CtrlX);
		DotNetBarManager.AutoDispatchShortcuts.Add(eShortcut.CtrlZ);
		DotNetBarManager.AutoDispatchShortcuts.Add(eShortcut.CtrlY);
		DotNetBarManager.AutoDispatchShortcuts.Add(eShortcut.Del);
		DotNetBarManager.AutoDispatchShortcuts.Add(eShortcut.Ins);
		DotNetBarManager.BottomDockSite = DockSiteBottom;
		DotNetBarManager.EnableFullSizeDock = false;
		DotNetBarManager.LeftDockSite = DockSiteLeft;
		DotNetBarManager.ParentForm = null;
		DotNetBarManager.ParentUserControl = (Control)(object)this;
		DotNetBarManager.RightDockSite = DockSiteRight;
		DotNetBarManager.Style = eDotNetBarStyle.StyleManagerControlled;
		DotNetBarManager.ToolbarBottomDockSite = DockSiteBottomBars;
		DotNetBarManager.ToolbarLeftDockSite = DockSiteLeftBars;
		DotNetBarManager.ToolbarRightDockSite = DockSiteRightBars;
		DotNetBarManager.ToolbarTopDockSite = DockSiteTopBars;
		DotNetBarManager.TopDockSite = DockSiteTop;
		((Control)DockSiteBottom).AccessibleRole = (AccessibleRole)9;
		((Control)DockSiteBottom).Dock = (DockStyle)2;
		DockSiteBottom.DocumentDockContainer = new DocumentDockContainer();
		((Control)DockSiteBottom).Location = new Point(0, 1031);
		((Control)DockSiteBottom).Name = "DockSiteBottom";
		((Control)DockSiteBottom).Size = new Size(1600, 0);
		((Control)DockSiteBottom).TabIndex = 6;
		((Control)DockSiteBottom).TabStop = false;
		((Control)DockSiteLeft).AccessibleRole = (AccessibleRole)9;
		((Control)DockSiteLeft).Dock = (DockStyle)3;
		DockSiteLeft.DocumentDockContainer = new DocumentDockContainer();
		((Control)DockSiteLeft).Location = new Point(0, 67);
		((Control)DockSiteLeft).Name = "DockSiteLeft";
		((Control)DockSiteLeft).Size = new Size(0, 964);
		((Control)DockSiteLeft).TabIndex = 3;
		((Control)DockSiteLeft).TabStop = false;
		((Control)DockSiteRight).AccessibleRole = (AccessibleRole)9;
		((Control)DockSiteRight).Dock = (DockStyle)4;
		DockSiteRight.DocumentDockContainer = new DocumentDockContainer();
		((Control)DockSiteRight).Location = new Point(1600, 67);
		((Control)DockSiteRight).Name = "DockSiteRight";
		((Control)DockSiteRight).Size = new Size(0, 964);
		((Control)DockSiteRight).TabIndex = 4;
		((Control)DockSiteRight).TabStop = false;
		((Control)DockSiteBottomBars).AccessibleRole = (AccessibleRole)9;
		((Control)DockSiteBottomBars).Dock = (DockStyle)2;
		((Control)DockSiteBottomBars).Location = new Point(0, 1031);
		((Control)DockSiteBottomBars).Name = "DockSiteBottomBars";
		((Control)DockSiteBottomBars).Size = new Size(1600, 0);
		((Control)DockSiteBottomBars).TabIndex = 10;
		((Control)DockSiteBottomBars).TabStop = false;
		((Control)DockSiteLeftBars).AccessibleRole = (AccessibleRole)9;
		((Control)DockSiteLeftBars).Dock = (DockStyle)3;
		((Control)DockSiteLeftBars).Location = new Point(0, 67);
		((Control)DockSiteLeftBars).Name = "DockSiteLeftBars";
		((Control)DockSiteLeftBars).Size = new Size(0, 964);
		((Control)DockSiteLeftBars).TabIndex = 7;
		((Control)DockSiteLeftBars).TabStop = false;
		((Control)DockSiteRightBars).AccessibleRole = (AccessibleRole)9;
		((Control)DockSiteRightBars).Dock = (DockStyle)4;
		((Control)DockSiteRightBars).Location = new Point(1600, 67);
		((Control)DockSiteRightBars).Name = "DockSiteRightBars";
		((Control)DockSiteRightBars).Size = new Size(0, 964);
		((Control)DockSiteRightBars).TabIndex = 8;
		((Control)DockSiteRightBars).TabStop = false;
		((Control)DockSiteTopBars).AccessibleRole = (AccessibleRole)9;
		((Control)DockSiteTopBars).Controls.Add((Control)(object)MainMenu);
		((Control)DockSiteTopBars).Dock = (DockStyle)1;
		((Control)DockSiteTopBars).Location = new Point(0, 42);
		((Control)DockSiteTopBars).Name = "DockSiteTopBars";
		((Control)DockSiteTopBars).Size = new Size(1600, 25);
		((Control)DockSiteTopBars).TabIndex = 9;
		((Control)DockSiteTopBars).TabStop = false;
		((Control)MainMenu).AccessibleDescription = "DotNetBar Bar (MainMenu)";
		((Control)MainMenu).AccessibleName = "DotNetBar Bar";
		((Control)MainMenu).AccessibleRole = (AccessibleRole)2;
		MainMenu.DockSide = eDockSide.Top;
		MainMenu.DockTabStripHeight = 50;
		((Control)MainMenu).Font = new Font("Segoe UI", 9f);
		MainMenu.Location = new Point(0, 0);
		MainMenu.MenuBar = true;
		MainMenu.Name = "MainMenu";
		MainMenu.Size = new Size(36, 24);
		MainMenu.Stretch = true;
		MainMenu.Style = eDotNetBarStyle.StyleManagerControlled;
		MainMenu.TabIndex = 0;
		MainMenu.TabStop = false;
		((Control)MainMenu).Text = "Main Menu";
		((Control)DockSiteTop).AccessibleRole = (AccessibleRole)9;
		((Control)DockSiteTop).Dock = (DockStyle)1;
		DockSiteTop.DocumentDockContainer = new DocumentDockContainer();
		((Control)DockSiteTop).Location = new Point(0, 67);
		((Control)DockSiteTop).Name = "DockSiteTop";
		((Control)DockSiteTop).Size = new Size(1600, 0);
		((Control)DockSiteTop).TabIndex = 12;
		((Control)DockSiteTop).TabStop = false;
		buttonItem1.ImagePaddingHorizontal = 16;
		buttonItem1.ImagePaddingVertical = 12;
		buttonItem1.Name = "buttonItem1";
		((Control)bar1).AccessibleDescription = "DotNetBar Bar (bar1)";
		((Control)bar1).AccessibleName = "DotNetBar Bar";
		((Control)bar1).AccessibleRole = (AccessibleRole)23;
		bar1.BarType = eBarType.StatusBar;
		((Control)bar1).Dock = (DockStyle)2;
		bar1.DockTabStripHeight = 50;
		((Control)bar1).Font = new Font("Segoe UI", 9f);
		bar1.GrabHandleStyle = eGrabHandleStyle.ResizeHandle;
		bar1.Items.AddRange(new BaseItem[6] { sbpUnit, sbpReportChecker, sbpComponent, sbpLocation, labelItem1, icZoom });
		bar1.Location = new Point(0, 983);
		bar1.Name = "bar1";
		bar1.Size = new Size(1600, 48);
		bar1.Stretch = true;
		bar1.Style = eDotNetBarStyle.Office2007;
		bar1.TabIndex = 13;
		bar1.TabStop = false;
		((Control)bar1).Text = "bar1";
		bar1.Visible = false;
		sbpUnit.AutoExpandOnClick = true;
		sbpUnit.ImagePaddingHorizontal = 16;
		sbpUnit.ImagePaddingVertical = 12;
		sbpUnit.Name = "sbpUnit";
		sbpUnit.PopupSide = ePopupSide.Top;
		sbpUnit.SubItems.AddRange(new BaseItem[4] { tbCentimeters, tbHundredthsOfInch, tbInches, tbMillimeters });
		sbpUnit.Text = "Unit";
		sbpUnit.PopupOpen += ButtonUnit_PopupOpen;
		tbCentimeters.ImagePaddingHorizontal = 16;
		tbCentimeters.ImagePaddingVertical = 12;
		tbCentimeters.Name = "tbCentimeters";
		tbCentimeters.Text = "Centimeters";
		tbCentimeters.Click += ButtonCentimeters_Click;
		tbHundredthsOfInch.ImagePaddingHorizontal = 16;
		tbHundredthsOfInch.ImagePaddingVertical = 12;
		tbHundredthsOfInch.Name = "tbHundredthsOfInch";
		tbHundredthsOfInch.Text = "HundredthsOfInch";
		tbHundredthsOfInch.Click += ButtonHundredthsOfInch_Click;
		tbInches.ImagePaddingHorizontal = 16;
		tbInches.ImagePaddingVertical = 12;
		tbInches.Name = "tbInches";
		tbInches.Text = "Inches";
		tbInches.Click += ButtonInches_Click;
		tbMillimeters.ImagePaddingHorizontal = 16;
		tbMillimeters.ImagePaddingVertical = 12;
		tbMillimeters.Name = "tbMillimeters";
		tbMillimeters.Text = "Millimeters";
		tbMillimeters.Click += ButtonMillimeters_Click;
		sbpReportChecker.AutoExpandOnClick = true;
		sbpReportChecker.BeginGroup = true;
		sbpReportChecker.ButtonStyle = eButtonStyle.ImageAndText;
		sbpReportChecker.Image = (Image)componentResourceManager.GetObject("sbpReportChecker.Image");
		sbpReportChecker.ImagePaddingHorizontal = 16;
		sbpReportChecker.ImagePaddingVertical = 12;
		sbpReportChecker.Name = "sbpReportChecker";
		sbpReportChecker.Text = "Report Checker";
		sbpReportChecker.Click += ButtonReportChecker_Click;
		sbpComponent.BeginGroup = true;
		sbpComponent.ImagePaddingHorizontal = 16;
		sbpComponent.ImagePaddingVertical = 12;
		sbpComponent.Name = "sbpComponent";
		sbpComponent.Text = "Component";
		sbpLocation.BeginGroup = true;
		sbpLocation.Name = "sbpLocation";
		sbpLocation.Text = "Location";
		labelItem1.ItemAlignment = eItemAlignment.Far;
		labelItem1.Name = "labelItem1";
		labelItem1.Width = 10;
		icZoom.BackgroundStyle.Class = "Office2007StatusBarBackground2";
		icZoom.BackgroundStyle.CornerType = eCornerType.Square;
		icZoom.HorizontalItemAlignment = eHorizontalItemsAlignment.Right;
		icZoom.Name = "icZoom";
		icZoom.SubItems.AddRange(new BaseItem[3] { icPageViewMode, siZoom, liSeparator2 });
		icZoom.TitleStyle.CornerType = eCornerType.Square;
		icPageViewMode.BackgroundStyle.CornerType = eCornerType.Square;
		icPageViewMode.BeginGroup = true;
		icPageViewMode.Name = "icPageViewMode";
		icPageViewMode.SubItems.AddRange(new BaseItem[4] { biZoomPageWidth, biZoomPageHeight, biZoomOnePage, biZoom100 });
		icPageViewMode.TitleStyle.CornerType = eCornerType.Square;
		biZoomPageWidth.Image = (Image)componentResourceManager.GetObject("biZoomPageWidth.Image");
		biZoomPageWidth.ImageIndex = 26;
		biZoomPageWidth.ImagePaddingHorizontal = 6;
		biZoomPageWidth.ImagePaddingVertical = 4;
		biZoomPageWidth.Name = "biZoomPageWidth";
		biZoomPageWidth.Text = "Page Width";
		biZoomPageWidth.Click += ButtonZoomPageWidth_Click;
		biZoomPageHeight.Image = (Image)componentResourceManager.GetObject("biZoomPageHeight.Image");
		biZoomPageHeight.ImageIndex = 27;
		biZoomPageHeight.ImagePaddingHorizontal = 6;
		biZoomPageHeight.ImagePaddingVertical = 4;
		biZoomPageHeight.Name = "biZoomPageHeight";
		biZoomPageHeight.Text = "Page Height";
		biZoomPageHeight.Click += ButtonZoomPageHeight_Click;
		biZoomOnePage.Image = (Image)componentResourceManager.GetObject("biZoomOnePage.Image");
		biZoomOnePage.ImageIndex = 28;
		biZoomOnePage.ImagePaddingHorizontal = 6;
		biZoomOnePage.ImagePaddingVertical = 4;
		biZoomOnePage.Name = "biZoomOnePage";
		biZoomOnePage.Text = "One Page";
		biZoomOnePage.Click += ButtonZoomOnePage_Click;
		biZoom100.Image = (Image)componentResourceManager.GetObject("biZoom100.Image");
		biZoom100.ImagePaddingHorizontal = 16;
		biZoom100.ImagePaddingVertical = 12;
		biZoom100.Name = "biZoom100";
		biZoom100.Text = "buttonItem3";
		biZoom100.Click += ButtonZoom100_Click;
		siZoom.ClickRepeatInterval = 50;
		siZoom.LabelVisible = false;
		siZoom.LabelWidth = 36;
		siZoom.Maximum = 400;
		siZoom.Minimum = 10;
		siZoom.Name = "siZoom";
		siZoom.Step = 10;
		siZoom.Text = "100%";
		siZoom.Value = 100;
		siZoom.Width = 130;
		siZoom.ValueChanged += SliderZoom_ValueChanged;
		liSeparator2.Name = "liSeparator2";
		liSeparator2.Width = 10;
		lbZoom.Name = "lbZoom";
		lbZoom.Text = "100%";
		lbZoom.TextAlignment = (StringAlignment)1;
		lbZoom.Width = 40;
		timerZoomSlider.Tick += TimerZoomSlider_Tick;
		StyleManager.ManagerStyle = eStyle.Office2007Blue;
		StyleManager.MetroColorParameters = new MetroColorGeneratorParameters(Color.White, Color.FromArgb(43, 87, 154));
		((Control)panelUpdateLicense).BackColor = Color.FromArgb(251, 244, 225);
		((Control)panelUpdateLicense).Controls.Add((Control)(object)buttonUpdate);
		((Control)panelUpdateLicense).Controls.Add((Control)(object)labelSubscriptionRequired);
		((Control)panelUpdateLicense).Dock = (DockStyle)1;
		((Control)panelUpdateLicense).Location = new Point(0, 0);
		((Control)panelUpdateLicense).Name = "panelUpdateLicense";
		((Control)panelUpdateLicense).Size = new Size(1600, 42);
		((Control)panelUpdateLicense).TabIndex = 14;
		((Control)panelUpdateLicense).Visible = false;
		((Control)panelUpdateLicense).Paint += new PaintEventHandler(PanelUpdateLicense_Paint);
		((Control)panelUpdateLicense).MouseClick += new MouseEventHandler(PanelUpdateLicense_MouseClick);
		((Control)panelUpdateLicense).MouseLeave += PanelUpdateLicense_MouseLeave;
		((Control)panelUpdateLicense).MouseMove += new MouseEventHandler(PanelUpdateLicense_MouseMove);
		((Control)buttonUpdate).Anchor = (AnchorStyles)11;
		((Control)buttonUpdate).Location = new Point(1517, 9);
		((Control)buttonUpdate).Name = "buttonUpdate";
		((Control)buttonUpdate).Size = new Size(80, 23);
		((Control)buttonUpdate).TabIndex = 2;
		((Control)buttonUpdate).Text = "Update";
		((ButtonBase)buttonUpdate).UseVisualStyleBackColor = true;
		((Control)buttonUpdate).Click += ButtonUpdate_Click;
		((Control)labelSubscriptionRequired).AutoSize = true;
		((Control)labelSubscriptionRequired).Location = new Point(50, 14);
		((Control)labelSubscriptionRequired).Name = "labelSubscriptionRequired";
		((Control)labelSubscriptionRequired).Size = new Size(1036, 25);
		((Control)labelSubscriptionRequired).TabIndex = 1;
		((Control)labelSubscriptionRequired).Text = "Subscription Required to Save. Start your free one month trial or sign in to activate an existing subscription.";
		((Control)this).BackColor = Color.FromArgb(191, 219, 255);
		((Control)this).Controls.Add((Control)(object)ContextMenuBar);
		((Control)this).Controls.Add((Control)(object)TabControl);
		((Control)this).Controls.Add((Control)(object)bar1);
		((Control)this).Controls.Add((Control)(object)DockSiteLeft);
		((Control)this).Controls.Add((Control)(object)DockSiteRight);
		((Control)this).Controls.Add((Control)(object)DockSiteBottom);
		((Control)this).Controls.Add((Control)(object)DockSiteTop);
		((Control)this).Controls.Add((Control)(object)DockSiteLeftBars);
		((Control)this).Controls.Add((Control)(object)DockSiteRightBars);
		((Control)this).Controls.Add((Control)(object)DockSiteBottomBars);
		((Control)this).Controls.Add((Control)(object)DockSiteTopBars);
		((Control)this).Controls.Add((Control)(object)panelUpdateLicense);
		((Control)this).Name = "StiDesignerControl";
		((Control)this).Size = new Size(1600, 1031);
		((UserControl)this).Load += This_Load;
		((ISupportInitialize)TabControl).EndInit();
		((ISupportInitialize)ContextMenuBar).EndInit();
		DockSiteTopBars.ResumeLayout(performLayout: false);
		((ISupportInitialize)MainMenu).EndInit();
		((ISupportInitialize)bar1).EndInit();
		((Control)panelUpdateLicense).ResumeLayout(false);
		((Control)panelUpdateLicense).PerformLayout();
		((Control)this).ResumeLayout(false);
	}

	private void This_DragOver(object sender, DragEventArgs e)
	{
		if (Report != null)
		{
			return;
		}
		if (e.Data.GetData(DataFormats.FileDrop) is string[] array)
		{
			List<string> exts = StiFileDialogHelper.GetAllResourceExtensions().ToList();
			exts.Add("dct");
			if (array.Any((string file) => exts.Any(file.EndsWithInvariantIgnoreCase)))
			{
				e.Effect = (DragDropEffects)(-2147483645);
				return;
			}
			string[] array2 = array;
			foreach (string file2 in array2)
			{
				if (file2.EndsWithInvariantIgnoreCase(".mrt") || file2.EndsWithInvariantIgnoreCase(".mrz") || file2.EndsWithInvariantIgnoreCase(".mrx") || file2.EndsWithInvariantIgnoreCase(".dct") || file2.EndsWithInvariantIgnoreCase(".dbf") || file2.EndsWithInvariantIgnoreCase(".csv") || file2.EndsWithInvariantIgnoreCase(".xml") || file2.EndsWithInvariantIgnoreCase(".json") || file2.EndsWithInvariantIgnoreCase(".xls") || file2.EndsWithInvariantIgnoreCase(".xlsx") || StiFileDialogHelper.FontExts.Any((string ext) => file2.EndsWithInvariantIgnoreCase(ext)) || StiFileDialogHelper.ImageExts.Any((string ext) => file2.EndsWithInvariantIgnoreCase(ext)))
				{
					e.Effect = (DragDropEffects)(-2147483645);
					return;
				}
			}
		}
		e.Effect = (DragDropEffects)0;
	}

	private async void This_DragDropAsync(object sender, DragEventArgs e)
	{
		if (Report != null || !(e.Data.GetData(DataFormats.FileDrop) is string[] files))
		{
			return;
		}
		List<string> exts = StiFileDialogHelper.GetAllResourceExtensions().ToList();
		exts.Add("dct");
		string[] array = files;
		foreach (string file in array)
		{
			if (file.EndsWithInvariantIgnoreCase(".mdc") || file.EndsWithInvariantIgnoreCase(".mdz"))
			{
				if (!File.Exists(file))
				{
					break;
				}
				try
				{
					StiReport stiReport = new StiReport();
					stiReport.LoadDocument(file);
					stiReport.Show();
					break;
				}
				catch (Exception exception)
				{
					StiExceptionProvider.Show(exception);
					break;
				}
			}
			if (file.EndsWithInvariantIgnoreCase(".mrt") || file.EndsWithInvariantIgnoreCase(".mrz") || file.EndsWithInvariantIgnoreCase(".mrx"))
			{
				if (!File.Exists(file))
				{
					break;
				}
				bool flag = false;
				try
				{
					flag = StiReportOpenDialog.OpenReport(this, Report, file);
				}
				catch (Exception exception2)
				{
					StiExceptionProvider.Show(exception2);
				}
				finally
				{
					if (Report != null)
					{
						Report.Dictionary.Connect(loadData: false);
						Builder.BuildReport();
						InvokeBuidingToolbox();
						Builder.InvalidateRulers();
					}
				}
				if (flag)
				{
					StiOptions.Engine.GlobalEvents.InvokeLoadedReportInDesigner(this);
				}
				Windows7Taskbar.HideTaskbarProgress();
				break;
			}
			if (file.EndsWithInvariantIgnoreCase(".dct"))
			{
				if (!File.Exists(file))
				{
					continue;
				}
				try
				{
					StiReport stiReport2 = Report;
					if (stiReport2 == null)
					{
						stiReport2 = new StiReport();
					}
					stiReport2.Dictionary.Load(file);
					if (Report == null)
					{
						Report = stiReport2;
					}
					BuildDictionary();
					break;
				}
				catch (Exception exception3)
				{
					StiExceptionProvider.Show(exception3);
					break;
				}
			}
			if (exts.Any(file.EndsWithInvariantIgnoreCase))
			{
				string extension = Path.GetExtension(file).ToLowerInvariant();
				StiResourceType? typeFromExtension = StiResourceTypeHelper.GetTypeFromExtension(extension);
				if (typeFromExtension.HasValue)
				{
					StiReport stiReport3 = Report;
					if (stiReport3 == null)
					{
						stiReport3 = new StiReport();
					}
					string newResourceName = StiDictionaryDropFilesControl.GetNewResourceName(file, stiReport3);
					StiResource resource = new StiResource(newResourceName, newResourceName, typeFromExtension.GetValueOrDefault(), File.ReadAllBytes(file));
					stiReport3.Dictionary.Resources.Add(resource);
					if (Report == null)
					{
						Report = stiReport3;
					}
				}
			}
			else if (StiFileDialogHelper.ImageExts.Any((string ext) => file.EndsWithInvariantIgnoreCase(ext)) || StiFileDialogHelper.FontExts.Any((string ext) => file.EndsWithInvariantIgnoreCase(ext)) || StiFileDialogHelper.TextExts.Any((string ext) => file.EndsWithInvariantIgnoreCase(ext)) || StiFileDialogHelper.DocumentExts.Any((string ext) => file.EndsWithInvariantIgnoreCase(ext)))
			{
				string extension2 = Path.GetExtension(file).ToLowerInvariant();
				StiResourceType? typeFromExtension2 = StiResourceTypeHelper.GetTypeFromExtension(extension2);
				if (typeFromExtension2.HasValue)
				{
					StiReport stiReport4 = Report;
					if (stiReport4 == null)
					{
						stiReport4 = new StiReport();
					}
					string newResourceName2 = StiNameHelper.GetNewResourceName(file, stiReport4);
					StiResource resource2 = new StiResource(newResourceName2, newResourceName2, typeFromExtension2.GetValueOrDefault(), File.ReadAllBytes(file));
					stiReport4.Dictionary.Resources.Add(resource2);
					if (Report == null)
					{
						Report = stiReport4;
					}
					StiDictionaryPanelService service = StiDictionaryPanelService.GetService();
					service.BuildDictionary();
				}
			}
			else
			{
				if (!file.EndsWithInvariantIgnoreCase(".dbf") && !file.EndsWithInvariantIgnoreCase(".csv") && !file.EndsWithInvariantIgnoreCase(".xml") && !file.EndsWithInvariantIgnoreCase(".json") && !file.EndsWithInvariantIgnoreCase(".xls") && !file.EndsWithInvariantIgnoreCase(".xlsx"))
				{
					continue;
				}
				try
				{
					StiDictionaryPanelHelper.ActivatePanel(DesignerControl as StiDesignerControl);
					StiDictionaryPanelHelper.ShowProgress(DesignerControl as StiDesignerControl, Loc.Get("DesignerFx", "LoadingData"));
					StiFileDatabase database = StiDictionaryPanelService.NewDatabaseFromExtension(file);
					if (database == null)
					{
						break;
					}
					StiReport report = Report;
					if (report == null)
					{
						report = new StiReport();
					}
					string name = (database.Alias = StiNameHelper.GetNewDatabaseName(file, report));
					database.Name = name;
					database.PathData = file;
					bool synchronize = false;
					StiXmlDatabase stiXmlDatabase = database as StiXmlDatabase;
					if (file.EndsWithInvariantIgnoreCase(".xml") && stiXmlDatabase != null)
					{
						stiXmlDatabase.PathSchema = files.FirstOrDefault((string f) => f.ToLowerInvariant() == Path.ChangeExtension(file.ToLowerInvariant(), "xsd"));
						stiXmlDatabase.XmlType = (string.IsNullOrWhiteSpace(stiXmlDatabase.PathSchema) ? StiXmlType.Xml : StiXmlType.AdoNetXml);
						if (file.EndsWithInvariantIgnoreCase("demo.xml") && stiXmlDatabase.PathSchema != null && stiXmlDatabase.PathSchema.EndsWithInvariantIgnoreCase("demo.xsd"))
						{
							synchronize = true;
						}
					}
					report.Dictionary.Databases.Add(database);
					await database.CreateDataSourcesAsync(report.Dictionary);
					if (synchronize)
					{
						await report.Dictionary.SynchronizeRelationsAsync(database.Name);
					}
					if (Report == null)
					{
						Report = report;
					}
					BuildDictionary();
				}
				catch
				{
				}
				finally
				{
					StiDictionaryPanelHelper.HideProgress(DesignerControl as StiDesignerControl);
				}
			}
		}
	}

	public void InvokeLocalizationChanged()
	{
		OnLocalizationChanged(EventArgs.Empty);
	}

	protected virtual void OnLocalizationChanged(EventArgs e)
	{
		this.LocalizationChanged?.Invoke(this, e);
	}

	public void InvokeZoomChanged()
	{
		OnZoomChanged(EventArgs.Empty);
	}

	protected virtual void OnZoomChanged(EventArgs e)
	{
		this.ZoomChanged?.Invoke(this, e);
		Builder.InvalidateRulers();
	}

	public void InvokeSelectedObjectsChanged()
	{
		OnSelectedObjectsChanged(EventArgs.Empty);
	}

	protected virtual void OnSelectedObjectsChanged(EventArgs e)
	{
		this.SelectedObjectsChanged?.Invoke(this, e);
	}

	public void InvokeSelectedStylesChanged()
	{
		OnSelectedStylesChanged(EventArgs.Empty);
	}

	protected virtual void OnSelectedStylesChanged(EventArgs e)
	{
		this.SelectedStylesChanged?.Invoke(this, e);
	}

	public void InvokeResetingToolbox()
	{
		OnResetingToolbox(EventArgs.Empty);
	}

	protected virtual void OnResetingToolbox(EventArgs e)
	{
		this.ResetingToolbox?.Invoke(this, EventArgs.Empty);
	}

	public void InvokeBuidingToolbox()
	{
		OnBuidingToolbox(EventArgs.Empty);
	}

	protected virtual void OnBuidingToolbox(EventArgs e)
	{
		this.BuidingToolbox?.Invoke(this, EventArgs.Empty);
	}

	public void InvokeRefreshStatusBar()
	{
		OnRefreshStatusBar(EventArgs.Empty);
	}

	protected virtual void OnRefreshStatusBar(EventArgs e)
	{
		this.RefreshStatusBar?.Invoke(this, EventArgs.Empty);
	}

	public void InvokeClearingCompilerResults()
	{
		OnClearingCompilerResults(EventArgs.Empty);
	}

	protected virtual void OnClearingCompilerResults(EventArgs e)
	{
		this.ClearingCompilerResults?.Invoke(this, EventArgs.Empty);
	}

	public void InvokeShowingCompilerResults(CompilerResults results)
	{
		CompilerResults = results;
		OnShowingCompilerResults(EventArgs.Empty);
	}

	protected virtual void OnShowingCompilerResults(EventArgs e)
	{
		this.ShowingCompilerResults?.Invoke(this, EventArgs.Empty);
	}

	public void InvokeClearingReportCheckerResults()
	{
		OnClearingReportCheckerResults(EventArgs.Empty);
	}

	protected virtual void OnClearingReportCheckerResults(EventArgs e)
	{
		this.ClearingReportCheckerResults?.Invoke(this, EventArgs.Empty);
	}

	public void InvokeShowingReportCheckerResults(StiCheckStatusEventArgs e)
	{
		OnShowingReportCheckerResults(e);
	}

	protected virtual void OnShowingReportCheckerResults(StiCheckStatusEventArgs e)
	{
		this.ShowingReportCheckerResults?.Invoke(this, e);
	}

	public void InvokeShowingReportRenderingMessages()
	{
		OnShowingReportRenderingMessages(EventArgs.Empty);
	}

	protected virtual void OnShowingReportRenderingMessages(EventArgs e)
	{
		this.ShowingReportRenderingMessages?.Invoke(this, EventArgs.Empty);
	}

	public void InvokeRefreshDesigner()
	{
		OnRefreshDesigner(EventArgs.Empty);
	}

	protected virtual void OnRefreshDesigner(EventArgs e)
	{
		this.RefreshDesigner?.Invoke(this, e);
	}

	public void InvokeRebuildDesigner()
	{
		OnRebuildDesigner(EventArgs.Empty);
	}

	protected virtual void OnRebuildDesigner(EventArgs e)
	{
		this.RebuildDesigner?.Invoke(this, e);
	}

	public void InvokeRefreshDictionary()
	{
		OnRefreshDictionary(EventArgs.Empty);
	}

	protected virtual void OnRefreshDictionary(EventArgs e)
	{
		this.RefreshDictionary?.Invoke(this, e);
	}

	private void OnPageNewClick(object sender, EventArgs e)
	{
		if (ClickedPageIndex != -1)
		{
			StiActions.MainMenu.InvokePageNew(this, ClickedPageIndex + 1);
		}
		else
		{
			StiActions.MainMenu.InvokePageNew(this);
		}
	}

	private void OnFormNewClick(object sender, EventArgs e)
	{
		if (ClickedPageIndex != -1)
		{
			StiActions.MainMenu.InvokeFormNew(this, ClickedPageIndex + 1);
		}
		else
		{
			StiActions.MainMenu.InvokeFormNew(this);
		}
	}

	private void OnDashboardNewClick(object sender, EventArgs e)
	{
		if (ClickedPageIndex != -1)
		{
			StiActions.MainMenu.InvokeDashboardNew(this, ClickedPageIndex + 1);
		}
		else
		{
			StiActions.MainMenu.InvokeDashboardNew(this);
		}
	}

	private void OnScreenNewClick(object sender, EventArgs e)
	{
		if (ClickedPageIndex != -1)
		{
			StiActions.MainMenu.InvokeScreenNew(this, ClickedPageIndex + 1);
		}
		else
		{
			StiActions.MainMenu.InvokeScreenNew(this);
		}
	}

	private void OnPageOpen_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokePageOpen(this);
		TabControl.SelectedTabIndex = Report.Pages.Count - 1;
	}

	private void OnPageSave_Click(object sender, EventArgs e)
	{
		if (ClickedPageIndex != -1)
		{
			TabControl.SelectedTabIndex = ClickedPageIndex;
		}
		StiActions.MainMenu.InvokePageSaveAs(this);
	}

	private void OnPageDeleteClick(object sender, EventArgs e)
	{
		if (ClickedPageIndex != -1)
		{
			TabControl.SelectedTabIndex = ClickedPageIndex;
		}
		StiActions.MainMenu.InvokePageDelete(this);
	}

	private void OnPageSetupClick(object sender, EventArgs e)
	{
		if (ClickedPageIndex != -1)
		{
			TabControl.SelectedTabIndex = ClickedPageIndex;
		}
		StiActions.MainMenu.InvokePageSetup(this);
	}

	private void OnPageViewQueryClick(object sender, EventArgs e)
	{
		if (ClickedPageIndex != -1 && Report?.Pages[ClickedPageIndex] is IStiDashboard element)
		{
			StiViewDashboardDataForm stiViewDashboardDataForm = new StiViewDashboardDataForm(element);
			((Form)stiViewDashboardDataForm).Show((IWin32Window)(object)((Control)this).FindForm());
		}
	}

	private void OnPageMoveLeftClick(object sender, EventArgs e)
	{
		if (ClickedPageIndex != -1)
		{
			TabControl.SelectedTabIndex = ClickedPageIndex;
		}
		DesignerControl.UndoRedoSave("Page Move Left");
		Report.IsModified = true;
		StiPage value = Report.Pages[TabControl.SelectedTabIndex - 1];
		Report.Pages[TabControl.SelectedTabIndex - 1] = Report.Pages[TabControl.SelectedTabIndex];
		Report.Pages[TabControl.SelectedTabIndex] = value;
		TabControl.SelectedTabIndex--;
		Builder.BuildReport();
	}

	private void OnPageMoveRightClick(object sender, EventArgs e)
	{
		if (ClickedPageIndex != -1)
		{
			TabControl.SelectedTabIndex = ClickedPageIndex;
		}
		DesignerControl.UndoRedoSave("Page Move Right");
		Report.IsModified = true;
		StiPage value = Report.Pages[TabControl.SelectedTabIndex + 1];
		Report.Pages[TabControl.SelectedTabIndex + 1] = Report.Pages[TabControl.SelectedTabIndex];
		Report.Pages[TabControl.SelectedTabIndex] = value;
		TabControl.SelectedTabIndex++;
		Builder.BuildReport();
	}

	private void OnPageDuplicateClick(object sender, EventArgs e)
	{
		int num = TabControl.SelectedTabIndex;
		if (ClickedPageIndex != -1)
		{
			num = ClickedPageIndex;
		}
		DesignerControl.UndoRedoSave("Page Duplicate");
		StiPage stiPage = Report.Pages[num];
		StiPage stiPage2 = null;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			stiPage.Save(memoryStream);
			memoryStream.Seek(0L, SeekOrigin.Begin);
			stiPage2 = ((stiPage is StiForm) ? new StiForm() : ((!(stiPage is IStiDashboard)) ? (StiActivator.CreateObject("Stimulsoft.Report.Components.StiPage") as StiPage) : (StiDashboardCreator.CreateDashboard(DesignerControl.Report) as StiPage)));
			stiPage2.NewGuid();
			stiPage2.NewCacheGuid();
			stiPage2.Report = Report;
			stiPage2.Name = "TempName";
			stiPage2.Load(memoryStream);
			stiPage2.Name = StiNameCreation.CreateName(Report, StiNameCreation.GenerateName(Report, stiPage.LocalizedName, stiPage.GetType()));
			stiPage2.NewGuid();
		}
		Report.Pages.Insert(num + 1, stiPage2);
		Report.IsModified = true;
		StiComponentsCollection stiComponentsCollection = stiPage2.GetComponents();
		foreach (StiComponent item in stiComponentsCollection)
		{
			item.Name = StiNameCreation.CreateName(Report, StiNameCreation.GenerateName(Report, item.LocalizedName, item.GetType()));
		}
		Builder.BuildReport();
		TabControl.SelectedTabIndex = num + 1;
	}

	private void OnPageRenameClick(object sender, EventArgs e)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Invalid comparison between Unknown and I4
		int index = TabControl.SelectedTabIndex;
		if (ClickedPageIndex != -1)
		{
			index = ClickedPageIndex;
		}
		StiPageRenameForm stiPageRenameForm = new StiPageRenameForm(Report.Pages[index]);
		try
		{
			AddOwnedForm((Form)(object)stiPageRenameForm);
			if ((int)((Form)stiPageRenameForm).ShowDialog() == 1)
			{
				Builder.RefreshPageNames();
			}
		}
		finally
		{
			((IDisposable)stiPageRenameForm)?.Dispose();
		}
	}

	public void InvokePageNewClick()
	{
		OnPageNewClick(this, EventArgs.Empty);
	}

	public void InvokeFormNewClick()
	{
		OnFormNewClick(this, EventArgs.Empty);
	}

	public void InvokeDashboardNewClick()
	{
		OnDashboardNewClick(this, EventArgs.Empty);
	}

	public void InvokeFormUINewClick()
	{
		OnScreenNewClick(this, EventArgs.Empty);
	}

	public void InvokePageDeleteClick()
	{
		OnPageDeleteClick(this, EventArgs.Empty);
	}

	public void InvokePageSetupClick()
	{
		OnPageSetupClick(this, EventArgs.Empty);
	}

	public void SetReport(StiReport value)
	{
		if (report != value)
		{
			StiMapLoader.DeleteAllCustomMaps();
			if (Info != null)
			{
				Info.Report = value;
			}
			report = value;
			if (report != null)
			{
				report.Info = Info;
			}
			AssignReport(report);
			if (!LockReportPropertyRefresh && Report != null)
			{
				bool canDesignerChangeReportFileName = StiDesigner.CanDesignerChangeReportFileName;
				StiDesigner.CanDesignerChangeReportFileName = false;
				StiActions.MainMenu.InvokeRegNewReport(this, report);
				StiDesigner.CanDesignerChangeReportFileName = canDesignerChangeReportFileName;
			}
		}
		SetAutoSaveMode();
	}

	public async Task SetReportAsync(StiReport value)
	{
		if (report != value)
		{
			if (Info != null)
			{
				Info.Report = value;
			}
			report = value;
			if (report != null)
			{
				report.Info = Info;
			}
			AssignReport(report);
			if (!LockReportPropertyRefresh && Report != null)
			{
				bool oldValue = StiDesigner.CanDesignerChangeReportFileName;
				StiDesigner.CanDesignerChangeReportFileName = false;
				await StiActions.MainMenu.InvokeRegNewReportAsync(this, report);
				StiDesigner.CanDesignerChangeReportFileName = oldValue;
			}
		}
		SetAutoSaveMode();
	}

	public StiQuickButton[] GetQuickButtons(StiComponent component)
	{
		if (quickButtonsHash[component.GetHashCode()] is StiQuickButton[] result)
		{
			return result;
		}
		StiQuickButton[] array = StiQuickButtonHelper.GetQuickButtons(component.GetType());
		if (array == null)
		{
			array = new StiQuickButton[0];
		}
		quickButtonsHash[component.GetHashCode()] = array;
		return array;
	}

	public double XToPage(int x)
	{
		return StiConvert.XToPage(this, x);
	}

	public double YToPage(int y)
	{
		return StiConvert.YToPage(this, y);
	}

	public async Task UndoRedoSaveAsync(string name, bool onlyCurrentPage = false)
	{
		await Task.Run(delegate
		{
			UndoRedoSave(name, onlyCurrentPage);
		});
	}

	public void UndoRedoSave(string name)
	{
		UndoRedoSave(name, onlyCurrentPage: false);
	}

	public void UndoRedoSave(string name, bool onlyCurrentPage)
	{
		if (Report != null)
		{
			if (UndoRedo.UndoLimit > 0 && UndoRedo.ServiceEnabled && !Report.IsPageDesigner)
			{
				UndoRedo.Save(name.Replace("&", string.Empty).Replace("...", string.Empty), Report, onlyCurrentPage, this);
			}
			Report.IsModified = true;
			StiOptions.Engine.GlobalEvents.InvokeReportChangedInDesigner(this, EventArgs.Empty);
		}
	}

	public override void Refresh()
	{
		if (LockRefreshDesigner)
		{
			return;
		}
		TextAliasesHash.Clear();
		if (Report != null && report.Pages.Count != TabControl.Tabs.Count - Builder.PagesCount)
		{
			Builder.BuildReport();
			return;
		}
		if (ComponentsOnReport != null)
		{
			ComponentsOnReport.Clear();
		}
		if (ComponentsOnPage != null)
		{
			ComponentsOnPage.Clear();
		}
		if (SelectedComponentsOnPage != null)
		{
			SelectedComponentsOnPage.Clear();
		}
		if (SelectedTextComponentsOnPage != null)
		{
			SelectedTextComponentsOnPage.Clear();
		}
		if (Report != null)
		{
			ComponentsOnReport = Report.GetComponents();
			ComponentsOnPage = Report.GetCurrentPage().GetComponents();
			SelectedComponentsOnPage = Report.GetCurrentPage().GetSelectedComponents();
			SelectedTextComponentsOnPage = new StiComponentsCollection();
			foreach (StiComponent item in ComponentsOnPage)
			{
				if ((item is IStiText) & item.IsSelected)
				{
					SelectedTextComponentsOnPage.Add(item);
				}
			}
		}
		else
		{
			ComponentsOnReport = new StiComponentsCollection();
			ComponentsOnPage = new StiComponentsCollection();
			SelectedComponentsOnPage = new StiComponentsCollection();
			SelectedTextComponentsOnPage = new StiComponentsCollection();
		}
		if (TabControl.SelectedTabIndex < PageCodeIndex)
		{
			AddSelectedToPropertyGrid(updateSelectedObjects: false);
		}
		InvokeRefreshStatusBar();
		InvokeRefreshDesigner();
		Builder.RefreshDesigner();
	}

	public void AddOwnedForm(Form form)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		Form obj = ((Control)this).FindForm();
		if (obj != null)
		{
			obj.AddOwnedForm(form);
		}
		form.FormClosed += new FormClosedEventHandler(Form_FormClosed);
	}

	private void Form_FormClosed(object sender, FormClosedEventArgs e)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		((Form)((sender is Form) ? sender : null)).Owner = null;
		((Form)((sender is Form) ? sender : null)).FormClosed -= new FormClosedEventHandler(Form_FormClosed);
	}

	public StiComponentsCollection GetSelected()
	{
		StiComponentsCollection selectedComponents = Report.GetCurrentPage().GetSelectedComponents();
		if (selectedComponents.Count == 0)
		{
			selectedComponents.Add(Report.GetCurrentPage());
		}
		return selectedComponents;
	}

	internal void Localize()
	{
		ciPageNew.Text = Loc.Get("MainMenu", "menuFilePageNew");
		ciFormNew.Text = Loc.Get("MainMenu", "menuFileFormNew") + " (" + Loc.Get("Report", "Obsoleted") + ")";
		ciScreenNew.Text = Loc.Get("MainMenu", "menuFileScreenNew");
		ciDashboardNew.Text = Loc.Get("MainMenu", "menuFileDashboardNew");
		ciPageDelete.Text = Loc.Get("Buttons", "Delete");
		ciPageSetup.Text = Loc.Get("MainMenu", "menuFilePageSetup");
		ciPageViewQuery.Text = Loc.Get("FormDictionaryDesigner", "ViewQuery");
		ciPageMoveLeft.Text = Loc.Get("Buttons", "MoveLeft");
		ciPageMoveRight.Text = Loc.Get("Buttons", "MoveRight");
		ciPageRename.Text = Loc.Get("Buttons", "Rename");
		ciPageDuplicate.Text = Loc.Get("Buttons", "Duplicate");
		ciPageOpen.Text = Loc.Get("MainMenu", "menuFilePageOpen");
		ciPageSave.Text = Loc.Get("MainMenu", "menuFilePageSaveAs");
		ciShowCodeTab.Text = Loc.Get("FormDesigner", "Code");
		if (((ArrangedElementCollection)((Control)TabControl).Controls).Count > PageCodeIndex)
		{
			((Control)TabControl).Controls[PageCodeIndex].Text = Loc.Get("FormDesigner", "Code");
		}
		if (((ArrangedElementCollection)((Control)TabControl).Controls).Count > PagePreviewIndex)
		{
			((Control)TabControl).Controls[PagePreviewIndex].Text = Loc.Get("FormDesigner", "Preview");
		}
		biZoom100.Text = Loc.Get("Zoom", "ZoomTo100");
		biZoomOnePage.Text = Loc.Get("Zoom", "OnePage");
		biZoomPageWidth.Text = Loc.Get("Zoom", "PageWidth");
		biZoomPageHeight.Text = Loc.Get("Zoom", "PageHeight");
		biZoom100.Tooltip = Loc.Get("Zoom", "ZoomTo100");
		biZoomOnePage.Tooltip = Loc.Get("Zoom", "OnePage");
		biZoomPageWidth.Tooltip = Loc.Get("Zoom", "PageWidth");
		biZoomPageHeight.Tooltip = Loc.Get("Zoom", "PageHeight");
		tbCentimeters.Text = Loc.Get("PropertyEnum", "StiReportUnitTypeCentimeters");
		tbHundredthsOfInch.Text = Loc.Get("PropertyEnum", "StiReportUnitTypeHundredthsOfInch");
		tbInches.Text = Loc.Get("PropertyEnum", "StiReportUnitTypeInches");
		tbMillimeters.Text = Loc.Get("PropertyEnum", "StiReportUnitTypeMillimeters");
		((Control)buttonUpdate).Text = Loc.Get("Buttons", "Upgrade");
		LocalizeRibbonViewerMenu();
		InvokeRefreshStatusBar();
		sbpReportChecker.Text = Loc.Get("Report", "ReportChecker");
		StiInplaceDesigner.Localize();
		StiElementActionHelper.Localize();
		Builder?.Localize();
		RefreshPanelUpdate();
	}

	public void LocalizeDesigner()
	{
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Expected O, but got Unknown
		if (SuperToolTip != null)
		{
			SuperToolTip.RemoveAllSuperTooltip();
			SuperToolTip = null;
		}
		SuperToolTip = new SuperTooltip();
		StiLocalization.LoadCurrentLocalization();
		StiCodeEditor.LocalizeEditor();
		Localize();
		LocalizeRibbonToolbox();
		LocalizeToolbars();
		LocalizePanels();
		LocalizeTabs();
		if (TabControl != null && ((ArrangedElementCollection)((Control)TabControl).Controls).Count > PagePreviewIndex && ((ArrangedElementCollection)((Control)TabControl).Controls[PagePreviewIndex].Controls).Count > 0 && IsViewerPresent)
		{
			LocalizeDesignerPreviewControl();
		}
		StiMainMenuService.GetService(this)?.Localize();
		if (TabControl.SelectedTab != null)
		{
			foreach (Control item in (ArrangedElementCollection)TabControl.SelectedTab.AttachedControl.Controls)
			{
				Control val = item;
				if (val is StiHorRollerLine || val is StiVerRollerLine)
				{
					val.Invalidate();
				}
			}
		}
		InvokeLocalizationChanged();
	}

	private void LocalizeDesignerPreviewControl()
	{
		if (TabControl.Tabs[PagePreviewIndex].AttachedControl.Controls[0] is StiDesignerPreviewControl stiDesignerPreviewControl)
		{
			stiDesignerPreviewControl.Localize();
		}
	}

	private void LocalizeTabs()
	{
		foreach (TabItem tab in TabControl.Tabs)
		{
			if (tab.Tag != null && tab.Tag is StiBuilder.Tabs)
			{
				StiBuilder.Tabs tabs = (StiBuilder.Tabs)tab.Tag;
				if (tabs == StiBuilder.Tabs.Code)
				{
					tab.Text = Loc.Get("FormDesigner", "Code");
				}
				if (tabs == StiBuilder.Tabs.Preview)
				{
					tab.Text = Loc.Get("FormDesigner", "Preview");
				}
			}
		}
	}

	internal void LocalizePanels()
	{
		StiServiceContainer services = Services.GetServices(typeof(StiPanelService));
		foreach (StiPanelService item in services)
		{
			item.Localize();
		}
	}

	internal void LocalizeRibbonToolbox()
	{
		StiRibbonToolboxToolbarService service = StiRibbonToolboxToolbarService.GetService();
		service.Localize();
	}

	internal void LocalizeToolbars()
	{
		StiServiceContainer services = Services.GetServices(typeof(StiToolbarService));
		foreach (StiToolbarService item in services)
		{
			item.Localize();
		}
	}

	internal void CloseBackStage()
	{
		ApplicationButton ribbonApplicationButton = GetRibbonApplicationButton();
		if (ribbonApplicationButton != null)
		{
			ribbonApplicationButton.Expanded = true;
			ribbonApplicationButton.Expanded = false;
		}
	}

	internal ApplicationButton GetRibbonApplicationButton()
	{
		if (RibbonControl != null)
		{
			return RibbonControl.Items.OfType<ApplicationButton>().FirstOrDefault();
		}
		return null;
	}

	internal void EnableRibbonBackState()
	{
		ApplicationButton ribbonApplicationButton = GetRibbonApplicationButton();
		if (ribbonApplicationButton != null && ribbonApplicationButton.BackstageTab == null)
		{
			StiRibbonBackStageUserControl stiRibbonBackStageUserControl = new StiRibbonBackStageUserControl(this);
			SuperTabControl backStage = stiRibbonBackStageUserControl.BackStage;
			((Control)stiRibbonBackStageUserControl).Dock = (DockStyle)0;
			((Control)stiRibbonBackStageUserControl).Controls.Remove((Control)(object)backStage);
			Form val = ((Control)this).FindForm();
			if (val != null)
			{
				((Control)val).Controls.Add((Control)(object)backStage);
			}
			else
			{
				((Control)this).Controls.Add((Control)(object)backStage);
			}
			ribbonApplicationButton.BackstageTab = backStage;
			((Control)backStage).Visible = false;
			ribbonApplicationButton.BackstageTabEnabled = true;
			ribbonApplicationButton.Tag = stiRibbonBackStageUserControl;
			ribbonApplicationButton.PopupOpen += ButtonPopupOpen;
			ribbonApplicationButton.PopupClose += ButtonPopupClose;
		}
	}

	private void DisableRibbonBackState()
	{
		ApplicationButton ribbonApplicationButton = GetRibbonApplicationButton();
		if (ribbonApplicationButton != null && ribbonApplicationButton.BackstageTabEnabled)
		{
			SuperTabControl backstageTab = ribbonApplicationButton.BackstageTab;
			if (backstageTab != null && ((Control)ribbonApplicationButton.BackstageTab).Parent != null && ((Control)ribbonApplicationButton.BackstageTab).Parent.Controls.Contains((Control)(object)ribbonApplicationButton.BackstageTab))
			{
				((Control)ribbonApplicationButton.BackstageTab).Parent.Controls.Remove((Control)(object)ribbonApplicationButton.BackstageTab);
			}
			ribbonApplicationButton.BackstageTab = null;
			ribbonApplicationButton.BackstageTabEnabled = false;
		}
	}

	private async void ButtonPopupOpen(object sender, PopupOpenEventArgs e)
	{
		Office2007ColorTable colorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
		colorTable.Form.Active.CaptionText = Color.Black;
		colorTable.Form.Active.CaptionTopBackground.Start = Color.White;
		colorTable.Form.Active.CaptionBottomBackground.Start = Color.White;
		colorTable.Form.Inactive.CaptionText = Color.Black;
		colorTable.Form.Inactive.CaptionTopBackground.Start = Color.White;
		colorTable.Form.Inactive.CaptionBottomBackground.Start = Color.White;
		colorTable.SystemButton.Default.Foreground.Start = Color.Black;
		if (colorTable.SystemButtonClose != null)
		{
			colorTable.SystemButtonClose.Default.Foreground.Start = Color.Black;
		}
		ApplicationButton applicationButton = (ApplicationButton)sender;
		StiRibbonBackStageUserControl stiRibbonBackStageUserControl = (StiRibbonBackStageUserControl)applicationButton.Tag;
		await stiRibbonBackStageUserControl.RefreshGuiAsync();
	}

	private void ButtonPopupClose(object sender, EventArgs e)
	{
		ApplicationButton applicationButton = (ApplicationButton)sender;
		StiRibbonBackStageUserControl stiRibbonBackStageUserControl = (StiRibbonBackStageUserControl)applicationButton.Tag;
		if (e is PopupOpenEventArgs popupOpenEventArgs && stiRibbonBackStageUserControl.StartScreenVisible)
		{
			popupOpenEventArgs.Cancel = true;
			return;
		}
		Color baseColor = MetroColorGeneratorParameters.DarkBlue.BaseColor;
		Office2007ColorTable colorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
		colorTable.Form.Active.CaptionText = Color.White;
		colorTable.Form.Active.CaptionTopBackground.Start = baseColor;
		colorTable.Form.Active.CaptionBottomBackground.Start = baseColor;
		colorTable.Form.Inactive.CaptionText = Color.Gainsboro;
		colorTable.Form.Inactive.CaptionTopBackground.Start = baseColor;
		colorTable.Form.Inactive.CaptionBottomBackground.Start = baseColor;
		colorTable.SystemButton.Default.Foreground.Start = Color.White;
		colorTable.SystemButton.Pressed.Foreground = colorTable.SystemButton.MouseOver.Foreground;
		colorTable.SystemButtonClose.Default.Foreground.Start = Color.White;
		colorTable.SystemButtonClose.Pressed.Foreground = colorTable.SystemButtonClose.MouseOver.Foreground;
	}

	public void ShowReportCheckerError(StiReportCheckerErrorParameters parameters)
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Invalid comparison between Unknown and I4
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0456: Unknown result type (might be due to invalid IL or missing references)
		StiPopup stiPopup = popup;
		if (stiPopup != null)
		{
			((ToolStripDropDown)stiPopup).Close();
		}
		if (report == null || (parameters.CompName == null && parameters.Component == null) || parameters.PropName == null)
		{
			return;
		}
		if (parameters.Component != null && parameters.PropName == "Name")
		{
			string text = parameters.CompName;
			DialogResult val = StiTextEditor.EditText(null, ref text, StiCodeEditorMode.EditorText);
			if ((int)val == 1)
			{
				if (parameters.Component is StiComponent)
				{
					(parameters.Component as StiComponent).Name = text;
				}
				if (parameters.Component is StiDataSource)
				{
					(parameters.Component as StiDataSource).Name = text;
				}
				if (parameters.Component is StiDataRelation)
				{
					(parameters.Component as StiDataRelation).Name = text;
				}
			}
			return;
		}
		StiComponent componentByName = report.GetComponentByName(parameters.CompName);
		if (componentByName == null && parameters.CompName.Contains("."))
		{
			string[] array = parameters.CompName.Split(new char[1] { '.' });
			StiComponent componentByName2 = report.GetComponentByName(array[0]);
			if (componentByName2 != null)
			{
				UndoRedoSave(StiLocalization.Get("Buttons", "Design"), onlyCurrentPage: true);
				StiComponentDesigner componentDesigner = StiComponentDesigner.GetComponentDesigner(this, componentByName2.GetType(), StiGuiMode.Gdi);
				componentDesigner.Design(componentByName2);
				return;
			}
		}
		if (parameters.PropName.EndsWith("Event"))
		{
			if (componentByName != null)
			{
				StiReportCheckerHelper.InvokeEvent(this, componentByName, parameters.PropName);
			}
			else
			{
				StiReportCheckerHelper.InvokeEvent(this, report, parameters.PropName);
			}
			return;
		}
		if (componentByName is IStiElement)
		{
			(DesignerControl as StiDesignerControl)?.RibbonControl?.SelectFirstVisibleRibbonTab();
			StiComponentDesigner componentDesigner2 = StiComponentDesigner.GetComponentDesigner(this, componentByName.GetType(), StiGuiMode.Gdi);
			componentDesigner2.Design(componentByName);
		}
		if (componentByName != null)
		{
			if (componentByName.GetType().GetProperty(parameters.PropName) == null)
			{
				return;
			}
			switch (parameters.PropName)
			{
			case "Conditions":
				StiReportCheckerHelper.InvokeConditions(this, componentByName);
				return;
			case "ExcelValue":
				StiReportCheckerHelper.InvokeTextProperty(this, componentByName, "ExcelValue");
				return;
			case "ExcelSheet":
				StiReportCheckerHelper.InvokeTextProperty(this, componentByName, "ExcelSheet");
				return;
			case "Bookmark":
				StiReportCheckerHelper.InvokeInteraction(this, componentByName, StiInteractionEditorForm.StartTab.PageBookmark);
				return;
			case "Hyperlink":
				StiReportCheckerHelper.InvokeInteraction(this, componentByName, StiInteractionEditorForm.StartTab.PageHyperlink);
				break;
			case "Tag":
				StiReportCheckerHelper.InvokeInteraction(this, componentByName, StiInteractionEditorForm.StartTab.PageTag);
				break;
			case "ToolTip":
				StiReportCheckerHelper.InvokeInteraction(this, componentByName, StiInteractionEditorForm.StartTab.PageTooltip);
				break;
			}
			if (componentByName is StiTextInCells)
			{
				string propName = parameters.PropName;
				if (propName == "Text")
				{
					StiReportCheckerHelper.InvokeTextProperty(this, componentByName, "Text");
				}
				return;
			}
			if (componentByName is StiText)
			{
				string propName2 = parameters.PropName;
				if (propName2 == "Text")
				{
					UndoRedoSave(StiLocalization.Get("Buttons", "Design"), onlyCurrentPage: true);
					StiComponentDesigner componentDesigner3 = StiComponentDesigner.GetComponentDesigner(this, componentByName.GetType(), StiGuiMode.Gdi);
					StartCheckExpressionAfterOpeningSystemTextEditorForm = true;
					componentDesigner3.Design(componentByName);
					StartCheckExpressionAfterOpeningSystemTextEditorForm = false;
				}
				return;
			}
			if (componentByName is StiImage)
			{
				string propName3 = parameters.PropName;
				if (propName3 == "ImageData")
				{
					UndoRedoSave(StiLocalization.Get("Buttons", "Design"), onlyCurrentPage: true);
					StiComponentDesigner componentDesigner4 = StiComponentDesigner.GetComponentDesigner(this, componentByName.GetType(), StiGuiMode.Gdi);
					componentDesigner4.Design(componentByName);
				}
				return;
			}
			if (componentByName is StiGroupHeaderBand)
			{
				string propName4 = parameters.PropName;
				if (!(propName4 == "Condition"))
				{
					if (propName4 == "SummaryExpression")
					{
						StiReportCheckerHelper.InvokeTextProperty(this, componentByName, "SummaryExpression");
					}
				}
				else
				{
					UndoRedoSave(StiLocalization.Get("Buttons", "Design"), onlyCurrentPage: true);
					StiComponentDesigner componentDesigner5 = StiComponentDesigner.GetComponentDesigner(this, componentByName.GetType(), StiGuiMode.Gdi);
					componentDesigner5.Design(componentByName);
				}
				return;
			}
			if (componentByName is StiRichText richText)
			{
				string propName5 = parameters.PropName;
				if (propName5 == "Text")
				{
					StiReportCheckerHelper.InvokeRichTextEditor(this, richText);
				}
				return;
			}
			StiChart stiChart = componentByName as StiChart;
			if (stiChart != null)
			{
				return;
			}
			if (componentByName is StiDataBand)
			{
				string propName6 = parameters.PropName;
				if (propName6 == "Filter")
				{
					StiReportCheckerHelper.InvokeDataBandEditor(this, componentByName, StiDataSelectionControl.StartTab.Filters);
				}
			}
			else if (componentByName is StiCrossTab crossTab)
			{
				string propName7 = parameters.PropName;
				if (propName7 == "Filter")
				{
					StiReportCheckerHelper.InvokeCrossTabDataSourceEditor(this, crossTab, StiCrossTabDataSourceForm.StartTab.Filters);
				}
			}
		}
		else if (report.Dictionary.Variables.Contains(parameters.CompName))
		{
			StiVariable stiVariable = report.Dictionary.Variables[parameters.CompName];
			if (stiVariable.Name.Length <= 0)
			{
				return;
			}
			StiDictionaryPanelService stiDictionaryPanelService = null;
			foreach (object service in Services.GetServices(typeof(StiPanelService)))
			{
				stiDictionaryPanelService = service as StiDictionaryPanelService;
				if (stiDictionaryPanelService != null)
				{
					break;
				}
			}
			StiActions.Dictionary.InvokeVariableEdit(stiDictionaryPanelService, stiVariable, this);
		}
		else if (report.Dictionary.DataSources.Contains(parameters.CompName))
		{
			StiDataSource stiDataSource = report.Dictionary.DataSources[parameters.CompName];
			if (stiDataSource.Columns[parameters.PropName] is StiCalcDataColumn column)
			{
				StiActions.Dictionary.InvokeCalcDataColumnEdit(null, column, this);
			}
		}
	}

	internal void SetComponentToolTip(IComponent item, StiComponent service)
	{
		if (item == null || service == null)
		{
			return;
		}
		string serviceName = service.ServiceName;
		Type type = service.GetType();
		string text = ((!(service is StiReportControl)) ? StiLocalization.Get("HelpComponents", type.Name, throwError: false) : StiLocalization.Get("HelpDialogs", type.Name, throwError: false));
		if (text == null)
		{
			object[] customAttributes = type.GetCustomAttributes(typeof(StiSuperToolTipTextAttribute), inherit: true);
			if (customAttributes.Length != 0)
			{
				text = ((StiSuperToolTipTextAttribute)customAttributes[0]).Text;
			}
			if (service is IStiSuperToolTipLocalization)
			{
				text = ((IStiSuperToolTipLocalization)service).SuperTooltipText;
			}
		}
		string helpUrl = service.HelpUrl;
		SuperTooltipInfo superTooltipInfo;
		if (helpUrl == null || !StiOptions.Appearance.ShowTooltipsHelp)
		{
			superTooltipInfo = new SuperTooltipInfo(serviceName, null, text, null, null, CurrentTooltipColor);
			superTooltipInfo.FooterVisible = false;
		}
		else
		{
			string footerText = StiLocalization.Get("HelpDesigner", "PressF1");
			superTooltipInfo = new SuperTooltipInfo(serviceName, footerText, text, null, StiReportDesignImages.Actions.Help(), CurrentTooltipColor);
			superTooltipInfo.Hyperlink = helpUrl;
		}
		SuperToolTip.SetSuperTooltip(item, superTooltipInfo);
	}

	internal void SetComponentToolTip(IComponent item, string serviceName, string header, string body)
	{
		if (item != null && !string.IsNullOrEmpty(serviceName))
		{
			SuperTooltipInfo superTooltipInfo = new SuperTooltipInfo(header, null, body, null, null, CurrentTooltipColor);
			superTooltipInfo.FooterVisible = false;
			SuperToolTip.SetSuperTooltip(item, superTooltipInfo);
		}
	}

	internal void SetToolTipOnlyHeaderEx(IComponent item, Graphics g, string header, string url)
	{
		SizeF sizeF = SuperToolTip.MeasureHeaderText(g, header);
		int num = (int)Math.Round(sizeF.Width + (float)StiScale.I1) + StiScale.I(30);
		SuperTooltipInfo superTooltipInfo;
		int num2;
		if (StiOptions.Appearance.ShowTooltipsHelp)
		{
			string footerText = StiLocalization.Get("HelpDesigner", "PressF1");
			superTooltipInfo = new SuperTooltipInfo(header, footerText, null, null, StiReportDesignImages.Actions.Help(), CurrentTooltipColor)
			{
				Hyperlink = url
			};
			SizeF sizeF2 = SuperToolTip.MeasureFooterText(g, footerText);
			sizeF2.Width += StiScale.I(40);
			if (sizeF2.Width > (float)num)
			{
				num = (int)sizeF2.Width;
			}
			num2 = StiScale.I(20);
		}
		else
		{
			superTooltipInfo = new SuperTooltipInfo(header, null, null, null, null, CurrentTooltipColor)
			{
				FooterVisible = false
			};
			num2 = -StiScale.I7;
		}
		int num3 = ((StiScale.Factor == 1.0) ? StiScale.I(15) : StiScale.I(6));
		superTooltipInfo.CustomSize = new Size(num, (int)Math.Round(sizeF.Height) + num2 + num3);
		SuperToolTip.SetSuperTooltip(item, superTooltipInfo);
	}

	internal void SetToolTipEx(BaseItem item, Graphics g, string header, string localizeName, string url)
	{
		string bodyText = StiLocalization.Get("HelpDesigner", localizeName);
		SizeF sizeF = SuperToolTip.MeasureHeaderText(g, header);
		SizeF sizeF2 = SuperToolTip.MeasureBodyText(g, bodyText);
		int num = ((sizeF.Width > sizeF2.Width) ? ((int)Math.Round((double)sizeF.Width + 0.5) + StiScale.I(25)) : ((int)Math.Round((double)sizeF2.Width + 0.5) + StiScale.I(25)));
		int num2 = 0;
		SuperTooltipInfo superTooltipInfo;
		if (StiOptions.Appearance.ShowTooltipsHelp)
		{
			string footerText = StiLocalization.Get("HelpDesigner", "PressF1");
			superTooltipInfo = new SuperTooltipInfo(header, footerText, bodyText, null, StiReportDesignImages.Actions.Help(), CurrentTooltipColor)
			{
				Hyperlink = url
			};
			SizeF sizeF3 = SuperToolTip.MeasureFooterText(g, footerText);
			sizeF3.Width += StiScale.I(40);
			if (sizeF3.Width > (float)num)
			{
				num = (int)sizeF3.Width;
			}
			num2 = StiScale.I(28);
		}
		else
		{
			superTooltipInfo = new SuperTooltipInfo(header, null, bodyText, null, null, CurrentTooltipColor)
			{
				FooterVisible = false
			};
		}
		superTooltipInfo.CustomSize = new Size(num, (int)Math.Round(sizeF2.Height + sizeF.Height) + num2 + StiScale.I(15));
		SuperToolTip.SetSuperTooltip(item, superTooltipInfo);
	}

	internal void SetToolTipEx(BaseItem item, Graphics g, string localizeName, string url)
	{
		if (item == null)
		{
			return;
		}
		string headerText = item.Text.Replace("&", string.Empty);
		string bodyText = StiLocalization.Get("HelpDesigner", localizeName);
		SizeF sizeF = SuperToolTip.MeasureHeaderText(g, headerText);
		SizeF sizeF2 = SuperToolTip.MeasureBodyText(g, bodyText);
		int num = ((sizeF.Width > sizeF2.Width) ? ((int)Math.Round((double)sizeF.Width + 0.5) + StiScale.I(15)) : ((int)Math.Round((double)sizeF2.Width + 0.5) + StiScale.I(25)));
		int num2 = 0;
		SuperTooltipInfo superTooltipInfo;
		if (StiOptions.Appearance.ShowTooltipsHelp)
		{
			string footerText = StiLocalization.Get("HelpDesigner", "PressF1");
			superTooltipInfo = new SuperTooltipInfo(headerText, footerText, bodyText, null, StiReportDesignImages.Actions.Help(), CurrentTooltipColor)
			{
				Hyperlink = url
			};
			SizeF sizeF3 = SuperToolTip.MeasureFooterText(g, footerText);
			sizeF3.Width += StiScale.I(40);
			if (sizeF3.Width > (float)num)
			{
				num = (int)sizeF3.Width;
			}
			num2 = StiScale.I(28);
		}
		else
		{
			superTooltipInfo = new SuperTooltipInfo(headerText, null, bodyText, null, null, CurrentTooltipColor)
			{
				FooterVisible = false
			};
		}
		superTooltipInfo.CustomSize = new Size(num, (int)Math.Round(sizeF2.Height + sizeF.Height) + num2 + StiScale.I(15));
		SuperToolTip.SetSuperTooltip(item, superTooltipInfo);
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private void _003C_003En__0(EventArgs e)
	{
		((UserControl)this).OnLoad(e);
	}
}
