using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Server;
using Stimulsoft.Base.Services;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Controls.Win.DotNetBar.Events;
using Stimulsoft.Controls.Win.DotNetBar.Metro.ColorTables;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;
using Stimulsoft.Controls.Win.Editors;
using Stimulsoft.Report.App;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Table;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Design.Check;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Dashboard.Helper;
using Stimulsoft.Report.Design.Helpers;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Design.Images.Office2013;
using Stimulsoft.Report.Design.Inplace;
using Stimulsoft.Report.Design.Panels;
using Stimulsoft.Report.Design.Properties;
using Stimulsoft.Report.Design.Ribbon.Localizations;
using Stimulsoft.Report.Design.Ribbon.Styles;
using Stimulsoft.Report.Design.Toolbars;
using Stimulsoft.Report.Design.Wizards;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Gauge.Helpers;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Maps;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Design.Ribbon;

[ToolboxItem(false)]
public class StiRibbonDesignerUserControl : UserControl
{
	private class ShowMoreInfo
	{
		public bool IsCrossTabOnly;

		public bool IsChartOnly;

		public bool IsGaugeOnly;

		public bool IsMapOnly;

		public bool IsDialogOnly;

		public bool IsTableOnly;

		public bool IsCompsOnly;

		public string MapIdent = "USA";

		public StiProgressElementMode ProgressMode = StiProgressElementMode.Circle;

		public StiFontIcons IndicatorFontIcons = StiFontIcons.Rating4;

		public StiGaugeType? GaugeType;

		public StiBaseStyle SelectedStyle;

		public List<StiBaseStyle> styles;
	}

	private List<string> excludeLocalizeFile = new List<string>(new string[9] { "cs", "es", "eu-ES", "gl-ES", "hr", "nl", "pt-BR", "zh-CHS", "zh-CHT" });

	private List<ButtonItem> wizardsButtons = new List<ButtonItem>();

	private List<ButtonItem> templatesButtons = new List<ButtonItem>();

	private bool lockRefresh;

	private Timer timer;

	private StiPopup popup;

	private StiPopup menuPopup;

	private ElementHost accountMenuElementHost;

	private IStiAccountLoginMenuControl accountMenu;

	private RibbonTabItem ribbonTabPreview;

	private StiDesignerControl designer;

	private bool showTabPreview;

	private IContainer components;

	private RibbonPanel ribbonPanelHome;

	private RibbonPanel ribbonPanelInsert;

	private RibbonBar ribbonBarClipboard;

	private RibbonPanel ribbonPanelPage;

	private Office2007StartButton OfficeStartButton;

	private ItemContainer itemContainerMainMenu;

	private ItemContainer itemContainerMainMenu2;

	private ItemContainer itemContainerMainMenu3;

	private ButtonItem biBlankReport;

	private ButtonItem biBlankDashboard;

	private ButtonItem biBlankScreen;

	private ButtonItem biReportOpen;

	private ButtonItem biReportSave;

	private ButtonItem biClose;

	private GalleryContainer galleryRecentFiles;

	private LabelItem liRecentDocuments;

	private ButtonItem biRecentFile01;

	private ButtonItem biRecentFile02;

	private ButtonItem biRecentFile03;

	private ButtonItem biRecentFile05;

	private ItemContainer itemContainer4;

	private ButtonItem biOptions;

	private ButtonItem biExit;

	private ButtonItem biQuickUndo;

	private RibbonBar ribbonBarAlignment;

	private RibbonBar ribbonBarPageSetup;

	private RibbonBar ribbonBarFont;

	private ItemContainer itemContainer5;

	private ButtonItem biPaste;

	private ItemContainer itemContainer6;

	private ButtonItem biCopy;

	private ButtonItem biCut;

	private ItemContainer itemContainer7;

	private ComboBoxItem cbFont;

	private ComboBoxItem cbFontSize;

	private ButtonItem biFontGrow;

	private ButtonItem biFontShrink;

	private ItemContainer itemContainer8;

	private ItemContainer itemContainer10;

	private ItemContainer itemContainer11;

	private ItemContainer itemContainer12;

	private ButtonItem biFontBold;

	private ButtonItem biFontItalic;

	private ButtonItem biFontUnderline;

	private ButtonItem biFontStrikeout;

	private ItemContainer itemContainer13;

	private StiColorPickerItem biTextBrush;

	private ItemContainer itemContainer14;

	private ItemContainer itemContainer15;

	private ButtonItem biContentAlignTop;

	private ButtonItem biContentAlignMiddle;

	private ButtonItem biContentAlignBottom;

	private ItemContainer itemContainer16;

	private ButtonItem biRotateText;

	private ButtonItem biLineSpacing;

	private ItemContainer itemContainer17;

	private ItemContainer itemContainer18;

	private ButtonItem biContentAlignLeft;

	private ButtonItem biContentAlignCenter;

	private ButtonItem biContentAlignRight;

	private ButtonItem biContentAlignWidth;

	private RibbonBar ribbonBarBorders;

	private ItemContainer itemContainer19;

	private ItemContainer itemContainer20;

	private ItemContainer itemContainer22;

	private ButtonItem biBorderAll;

	private ButtonItem biBorderNone;

	private ItemContainer itemContainer21;

	private ItemContainer itemContainer23;

	private ButtonItem biBorderTop;

	private ButtonItem biBorderLeft;

	private ButtonItem biBorderBottom;

	private ButtonItem biBorderRight;

	private ItemContainer itemContainer24;

	private ButtonItem biBorderShadow;

	private ItemContainer itemContainer25;

	private StiColorPickerItem biFillBrush;

	private StiColorPickerItem biBorderColor;

	private ButtonItem biQuickReportSave;

	private ButtonItem biQuickRedo;

	private RibbonPanel ribbonPanelLayout;

	private RibbonBar ribbonBarTextFormat;

	private RibbonBar ribbonBarArrange;

	private RibbonBar ribbonBarStyle;

	private ButtonItem biAlignLeft;

	private ButtonItem biAlignCenter;

	private ItemContainer itemContainer28;

	private ButtonItem biClearContents;

	private ButtonItem biAlignRight;

	private ButtonItem biAlignTop;

	private ButtonItem biAlignMiddle;

	private ButtonItem biAlignBottom;

	private ButtonItem biCenterHorizontally;

	private ButtonItem biCenterVertically;

	private ButtonItem biMakeHorizontalSpacingEqual;

	private ButtonItem biMakeVerticalSpacingEqual;

	private ButtonItem biAlignToGrid;

	private ButtonItem biBringToFront;

	private ButtonItem biMoveForward;

	private ButtonItem biSendToBack;

	private ButtonItem biMoveBackward;

	private ButtonItem biAlign;

	private ButtonItem biSize;

	private ButtonItem biMakeSameWidth;

	private ButtonItem biMakeSameHeight;

	private ButtonItem biMakeSameSize;

	private RibbonBar ribbonBarViewOptions;

	private ButtonItem biGridMode;

	private ButtonItem biGridModeLines;

	private ButtonItem biGridModeDotes;

	private ItemContainer itemContainer29;

	private ButtonItem biLock;

	private ButtonItem biLink;

	private ButtonItem biReportPreview;

	private ButtonItem buttonItem2;

	private ButtonItem buttonItem3;

	private ButtonItem buttonItem4;

	private ButtonItem buttonItem5;

	private ButtonItem buttonItem6;

	private ButtonItem buttonItem7;

	private ButtonItem buttonItem12;

	private ButtonItem buttonItem13;

	private ButtonItem biSelectLanguage;

	private ButtonItem buttonItem14;

	private ButtonItem buttonItemEmpty;

	private ButtonItem biRecentFile06;

	private ButtonItem biRecentFile07;

	private ButtonItem biRecentFile08;

	private ButtonItem biRecentFile09;

	private ButtonItem buttonItem8;

	private ButtonItem buttonItem9;

	private ButtonItem buttonItem10;

	private ButtonItem buttonItem11;

	private ButtonItem biNew;

	private ButtonItem biSaveAs;

	private ButtonItem biDelete;

	private LabelItem labelItem8;

	private ButtonItem cmLineSpacing1;

	private ButtonItem cmLineSpacing115;

	private ButtonItem cmLineSpacing15;

	private ButtonItem cmLineSpacing2;

	private ButtonItem cmLineSpacing25;

	private ButtonItem cmLineSpacing3;

	private ButtonItem biRecentFile04;

	private ButtonItem cmRotateAngle0;

	private ItemContainer itemContainer9;

	private ButtonItem cmRotateAngle45;

	private ButtonItem cmRotateAngle90;

	private ButtonItem cmRotateAngle180;

	private ButtonItem cmRotateAngle270;

	private ItemContainer itemContainer34;

	private ButtonItem biTextFormat;

	private ButtonItem biFormatGeneral;

	private ButtonItem biFormatNumber;

	private ButtonItem biFormatCurrency;

	private ButtonItem biFormatDate;

	private ButtonItem biFormatTime;

	private ButtonItem biFormatPercentage;

	private ButtonItem biFormatBoolean;

	private ItemContainer itemContainer36;

	private ButtonItem biCurrencySymbol;

	private ButtonItem biCurrencySymbolTemp;

	private ItemContainer itemContainer35;

	private ItemContainer itemContainer37;

	private ButtonItem biDateTimeFormat;

	private StiPenStyleItem tbBorderStyleDashDot;

	private StiPenStyleItem tbBorderStyleDashDotDot;

	private StiPenStyleItem tbBorderStyleDot;

	private StiPenStyleItem tbBorderStyleDouble;

	private StiPenStyleItem tbBorderStyleNone;

	private StiPenStyleItem tbBorderStyleDash;

	private StiPenStyleItem biBorderStyle;

	private StiPenStyleItem tbBorderStyleSolid;

	private ButtonItem biDateTimeFormatTemp;

	private ButtonItem biMargins;

	private ButtonItem biOrientation;

	private ButtonItem biPageSize;

	private ButtonItem biColumns;

	private ButtonItem biMarginsNormal;

	private ButtonItem biMarginsWide;

	private ButtonItem biMarginsNarrow;

	private ButtonItem biOrientationPortrait;

	private ButtonItem biOrientationLandscape;

	private ButtonItem biPageSizeTemp;

	private ButtonItem biColumnsOne;

	private ButtonItem biColumnsTwo;

	private ButtonItem biColumnsThree;

	private ButtonItem biViewShowGrid;

	private ButtonItem biViewShowHeaders;

	private ButtonItem biViewShowOrder;

	private ButtonItem biViewShowRulers;

	private ButtonItem biViewAlignToGrid;

	private ItemContainer itemContainer38;

	private ItemContainer itemContainer39;

	private ButtonItem biConditions;

	private ButtonItem biInteraction;

	private ItemContainer itemContainer40;

	private ButtonItem biWordWrap;

	private ButtonItem biStyleDesigner;

	private ButtonItem biQuickInfo;

	private ButtonItem biQuickInfoNone;

	private ButtonItem biQuickInfoShowComponentsNames;

	private ButtonItem biQuickInfoShowAliases;

	private ButtonItem biQuickInfoShowContent;

	private ButtonItem biQuickInfoShowFields;

	private ButtonItem biQuickInfoShowFieldsOnly;

	private ButtonItem biQuickInfoShowEvents;

	private ButtonItem biQuickInfoOverlay;

	private ButtonItem biDockingPanels;

	private ButtonItem biDockingPanelsTemp;

	private GalleryContainer galleryContainer1;

	private ItemContainer itemContainer50;

	private ButtonItem biReportSetup;

	private ButtonItem biShowToolbox;

	private ItemContainer itemContainer1;

	private ItemContainer itemContainer2;

	private ItemContainer itemContainer3;

	private QatCustomizeItem qatCustomizeItem1;

	private ButtonItem biCheckIssues;

	private ButtonItem biWatermarkSettings;

	private ApplicationButton applicationButton1;

	private ButtonItem buttonItem1;

	private ButtonItem biLoginMenu;

	private ButtonItem biLogin;

	private ButtonItem biPublish;

	private ButtonItem biNotifications;

	private ButtonItem biShare;

	private ButtonItem biAbout;

	private StiStyleItem galleryStyles;

	private ButtonItem buttonItem16;

	private ButtonItem buttonItem17;

	private RibbonControl ribbonControl;

	private ButtonItem biCopyStyle;

	public RibbonTabItem ribbonTabHome;

	private RibbonTabItem ribbonTabInsert;

	private RibbonTabItem ribbonTabPage;

	private RibbonTabItem ribbonTabLayout;

	private ItemContainer itemContainer26;

	private RibbonBar ribbonBarView;

	private ButtonItem biSurfaceViewDesktop;

	private ButtonItem biSurfaceViewMobile;

	private ButtonItem biSurfaceViewRemoveMobileSurface;

	private ButtonItem biSurfaceViewComponents;

	public const int StyleItemWidth = 156;

	public const int StyleItemHeight = 38;

	public const int StyleItemBigHeight = 52;

	private ButtonItem itemShowDictionary;

	private ButtonItem itemShowProperties;

	private ButtonItem itemShowReportTree;

	private StiStyle NullStyle = new StiStyle
	{
		Name = StiLocalization.Get("FormConditions", "SelectStyle"),
		HorAlignment = StiTextHorAlignment.Center,
		VertAlignment = StiVertAlignment.Center,
		AllowUseBorderSides = false,
		AllowUseBorderFormatting = false,
		AllowUseBrush = false,
		AllowUseHorAlignment = true,
		Border = null
	};

	private StiStyle DashboardAutoStyle = new StiStyle
	{
		Name = StiLocalization.Get("PropertyEnum", "StiGaugeCalculationModeAuto"),
		HorAlignment = StiTextHorAlignment.Center,
		VertAlignment = StiVertAlignment.Center,
		AllowUseBorderSides = false,
		AllowUseBorderFormatting = false,
		AllowUseBrush = false,
		AllowUseHorAlignment = true,
		Border = null
	};

	public StiRibbonDesigner RibbonDesigner { get; set; }

	public RibbonTabItem RibbonTabPreview
	{
		get
		{
			return ribbonTabPreview;
		}
		set
		{
			ribbonTabPreview = value;
			if (RibbonDesigner != null)
			{
				ribbonTabPreview.CheckedChanged += RibbonDesigner.RibbonTabPreview_CheckedChanged;
			}
			else if (designer is StiRibbonDesignerControl)
			{
				ribbonTabPreview.CheckedChanged += ((StiRibbonDesignerControl)designer).RibbonTabPreview_CheckedChanged;
			}
			ShowTabPreview = showTabPreview;
		}
	}

	public StiDesignerControl Designer
	{
		get
		{
			return designer;
		}
		set
		{
			if (designer != value)
			{
				if (designer != null)
				{
					designer.RefreshDesigner -= OnRefreshDesigner;
					designer.LocalizationChanged -= OnLocalizationChanged;
				}
				designer = value;
				if (value != null)
				{
					designer.RefreshDesigner += OnRefreshDesigner;
					designer.LocalizationChanged += OnLocalizationChanged;
				}
			}
		}
	}

	public static bool ShowQuickToolbar { get; set; } = true;


	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	internal ButtonItem CopyStyleButton => biCopyStyle;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	internal RibbonControl RibbonControl => ribbonControl;

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	[Obsolete("ShowHelp property is obsolete.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShowHelp { get; set; }

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowLocalizationMenu
	{
		get
		{
			return biSelectLanguage.Visible;
		}
		set
		{
			biSelectLanguage.Visible = value;
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowOfficeButton
	{
		get
		{
			return OfficeStartButton.Visible;
		}
		set
		{
			OfficeStartButton.Visible = value;
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowTabHome
	{
		get
		{
			return ribbonTabHome.Visible;
		}
		set
		{
			bool flag = ribbonControl.SelectedRibbonTabItem == ribbonTabHome;
			ribbonTabHome.Visible = value;
			if (!flag)
			{
				return;
			}
			foreach (ButtonItem item in ribbonControl.Items)
			{
				if (item.Visible && item is RibbonTabItem)
				{
					((RibbonTabItem)item).Select();
					break;
				}
			}
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowTabHomeBarClipboard
	{
		get
		{
			return ((Control)ribbonBarClipboard).Visible;
		}
		set
		{
			((Control)ribbonBarClipboard).Visible = value;
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowTabHomeBarFont
	{
		get
		{
			return ((Control)ribbonBarFont).Visible;
		}
		set
		{
			((Control)ribbonBarFont).Visible = value;
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowTabHomeBarAlignment
	{
		get
		{
			return ((Control)ribbonBarAlignment).Visible;
		}
		set
		{
			((Control)ribbonBarAlignment).Visible = value;
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowTabHomeBarBorders
	{
		get
		{
			return ((Control)ribbonBarBorders).Visible;
		}
		set
		{
			((Control)ribbonBarBorders).Visible = value;
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowTabHomeBarTextFormat
	{
		get
		{
			return ((Control)ribbonBarTextFormat).Visible;
		}
		set
		{
			((Control)ribbonBarTextFormat).Visible = value;
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowTabHomeBarStyle
	{
		get
		{
			return ((Control)ribbonBarStyle).Visible;
		}
		set
		{
			((Control)ribbonBarStyle).Visible = value;
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowTabPage
	{
		get
		{
			return ribbonTabPage.Visible;
		}
		set
		{
			bool flag = ribbonControl.SelectedRibbonTabItem == ribbonTabPage;
			ribbonTabPage.Visible = value;
			if (!flag)
			{
				return;
			}
			foreach (ButtonItem item in ribbonControl.Items)
			{
				if (item is RibbonTabItem { Visible: not false } ribbonTabItem)
				{
					ribbonTabItem.Select();
					break;
				}
			}
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowTabPageBarPageSetup
	{
		get
		{
			return ((Control)ribbonBarPageSetup).Visible;
		}
		set
		{
			((Control)ribbonBarPageSetup).Visible = value;
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowTabLayout
	{
		get
		{
			return ribbonTabLayout.Visible;
		}
		set
		{
			bool flag = ribbonControl.SelectedRibbonTabItem == ribbonTabLayout;
			ribbonTabLayout.Visible = value;
			if (!flag)
			{
				return;
			}
			foreach (ButtonItem item in ribbonControl.Items)
			{
				if (item is RibbonTabItem && item.Visible)
				{
					((RibbonTabItem)item).Select();
					break;
				}
			}
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowTabLayoutBarArrange
	{
		get
		{
			return ((Control)ribbonBarArrange).Visible;
		}
		set
		{
			((Control)ribbonBarArrange).Visible = value;
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	[Obsolete("ShowTabLayoutBarDockStyle property is not used more!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShowTabLayoutBarDockStyle
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	[Obsolete("ShowTabView property is not used more!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShowTabView
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowTabViewBarViewOptions
	{
		get
		{
			return ((Control)ribbonBarViewOptions).Visible;
		}
		set
		{
			((Control)ribbonBarViewOptions).Visible = value;
		}
	}

	[DefaultValue(false)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowTabPreview
	{
		get
		{
			if (RibbonTabPreview != null)
			{
				return RibbonTabPreview.Visible;
			}
			return false;
		}
		set
		{
			showTabPreview = value;
			if (RibbonTabPreview != null)
			{
				RibbonTabPreview.Visible = value;
			}
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowMainMenuNew
	{
		get
		{
			return biNew.Visible;
		}
		set
		{
			biNew.Visible = value;
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	[Obsolete("ShowMainMenuReportNew property is obsolete! Please use ShowMainMenuBlankReport property instead it.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShowMainMenuReportNew
	{
		get
		{
			return ShowMainMenuBlankReport;
		}
		set
		{
			ShowMainMenuBlankReport = value;
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowMainMenuBlankReport
	{
		get
		{
			return biBlankReport.Visible;
		}
		set
		{
			biBlankReport.Visible = value;
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowMainMenuBlankDashboard
	{
		get
		{
			return biBlankDashboard.Visible;
		}
		set
		{
			biBlankDashboard.Visible = value;
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowMainMenuBlankForm
	{
		get
		{
			return biBlankScreen.Visible;
		}
		set
		{
			biBlankScreen.Visible = value;
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowMainMenuReportWizardNew
	{
		get
		{
			if (wizardsButtons.Count > 0)
			{
				return wizardsButtons[0].Visible;
			}
			return false;
		}
		set
		{
			foreach (ButtonItem wizardsButton in wizardsButtons)
			{
				wizardsButton.Visible = value;
			}
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	[Obsolete("ShowMainMenuPageNew property is not used more!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShowMainMenuPageNew
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	[Obsolete("ShowMainMenuFormNew property is not used more!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShowMainMenuFormNew
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowMainMenuReportOpen
	{
		get
		{
			return biReportOpen.Visible;
		}
		set
		{
			biReportOpen.Visible = value;
		}
	}

	[Obsolete("Property 'ShowMainMenuReportOpenFromGoogleDocs' is not used more!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShowMainMenuReportOpenFromGoogleDocs
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	[Obsolete("ShowMainMenuPageOpen property is not used more!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShowMainMenuPageOpen
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowMainMenuReportSave
	{
		get
		{
			return biReportSave.Visible;
		}
		set
		{
			biReportSave.Visible = value;
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	[Obsolete("ShowMainMenuSaveAs property is not used more! Please use ShowMainMenuReportSaveAs property instead it.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShowMainMenuSaveAs
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowMainMenuReportSaveAs
	{
		get
		{
			return biSaveAs.Visible;
		}
		set
		{
			biSaveAs.Visible = value;
		}
	}

	[Obsolete("Property 'ShowMainMenuReportSaveAsToGoogleDocs' is not used more!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShowMainMenuReportSaveAsToGoogleDocs
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	[Obsolete("ShowMainMenuPageSaveAs property is not used more!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShowMainMenuPageSaveAs
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	[Obsolete("ShowMainMenuPageDelete property is not used more!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShowMainMenuPageDelete
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	[Obsolete("ShowMainMenuReportPreview property is not used more!")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShowMainMenuReportPreview
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowMainMenuCheckForIssues
	{
		get
		{
			return biCheckIssues.Visible;
		}
		set
		{
			biCheckIssues.Visible = value;
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowMainMenuReportSetup
	{
		get
		{
			return biReportSetup.Visible;
		}
		set
		{
			biReportSetup.Visible = value;
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowMainMenuRecentFiles
	{
		get
		{
			return galleryRecentFiles.Visible;
		}
		set
		{
			galleryRecentFiles.Visible = value;
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowMainMenuClose
	{
		get
		{
			return biClose.Visible;
		}
		set
		{
			biClose.Visible = value;
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowMainMenuOptions
	{
		get
		{
			return biOptions.Visible;
		}
		set
		{
			biOptions.Visible = value;
		}
	}

	[DefaultValue(true)]
	[Category("RibbonDesigner.Parameters")]
	public bool ShowMainMenuExit
	{
		get
		{
			return biExit.Visible;
		}
		set
		{
			biExit.Visible = value;
		}
	}

	public StiRibbonDesignerUserControl(StiDesignerControl designer)
	{
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Expected O, but got Unknown
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Expected O, but got Unknown
		//IL_0ad4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ade: Expected O, but got Unknown
		//IL_0afa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b04: Expected O, but got Unknown
		//IL_0b20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2a: Expected O, but got Unknown
		//IL_0c81: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c93: Expected O, but got Unknown
		//IL_0cf8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d02: Expected O, but got Unknown
		//IL_0db1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d94: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dbb: Expected O, but got Unknown
		//IL_0dbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc2: Expected O, but got Unknown
		InitializeComponent();
		if (StiOptions.Designer.IsDeveloperDesigner)
		{
			biQuickInfo.Visible = true;
		}
		Designer = designer;
		designer.RibbonDesignerUserControl = this;
		if (((Component)this).DesignMode)
		{
			return;
		}
		StiRibbonGuiService service = StiRibbonGuiService.GetService();
		ShowTabPreview = service.ShowTabPreview;
		InitScaledOfficeButton();
		((Control)ribbonControl).Size = new Size(1125, StiScale.YYI(153.0));
		DoScaling();
		if (!StiOptions.Designer.CanCustomizeRibbon)
		{
			qatCustomizeItem1.Visible = false;
			ribbonControl.CanCustomize = false;
		}
		InitBorderStyle();
		int num = ((!(StiScale.Factor >= 2.5)) ? 3 : 0);
		ButtonItem buttonItem = biCurrencySymbol;
		ButtonItem buttonItem2 = biDateTimeFormat;
		StiPenStyleItem stiPenStyleItem = biBorderStyle;
		StiColorPickerItem stiColorPickerItem = biBorderColor;
		int num3 = (biFillBrush.DropArrowShift = num);
		int num5 = (stiColorPickerItem.DropArrowShift = num3);
		int num7 = (stiPenStyleItem.DropArrowShift = num5);
		int dropArrowShift = (buttonItem2.DropArrowShift = num7);
		buttonItem.DropArrowShift = dropArrowShift;
		((ComboBox)cbFontSize.ComboBoxEx).ItemHeight = StiScale.YYI(16.0);
		cbFontSize.ComboWidth = StiScale.XXI(50.0);
		cbFontSize.DropDownHeight = StiScale.YYI(400.0);
		cbFontSize.EnabledChanged += cbFontSize_EnabledChanged;
		((Control)cbFontSize.ComboBoxEx).KeyDown += new KeyEventHandler(cbFontSize_KeyDown);
		cbFont.ComboBoxEx.AllowCustomFontDrawing = StiOptions.Designer.AllowCustomFontDrawing;
		((ComboBox)cbFont.ComboBoxEx).ItemHeight = StiScale.YYI(16.0);
		cbFont.DropDownWidth = StiScale.XXI(400.0);
		cbFont.DropDownHeight = StiScale.YYI(500.0);
		cbFont.ComboWidth = StiScale.XXI(149.0);
		((ComboBox)cbFont.ComboBoxEx).SelectedIndexChanged += cbFont_SelectedIndexChanged;
		biTextFormat.FixedSize = new Size(StiScale.XXI(100.0), StiScale.YYI(24.0));
		itemContainer34.MinimumSize = new Size(StiScale.XXI(102.0), 0);
		galleryStyles.FixedSize = new Size(StiScale.XXI(160.0), 0);
		ribbonControl.TabGroupHeight = StiScale.YYI(14.0);
		Localize();
		StiOptions.Viewer.Windows.BackgroundColor = Color.FromArgb(230, 230, 230);
		ribbonControl.RibbonStripFont = new Font("Segoe UI", 9.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		OfficeStartButton.BackstageTabEnabled = true;
		StiOptions.Designer.AllowMultiColorTitle = false;
		RibbonForm.OffsetHitZone = StiScale.XXI(14.0);
		RibbonStrip.OffsetCaptionContainer = StiScale.XXI(11.0);
		ribbonTabHome.CustomColorName = "customDefault";
		ribbonTabInsert.CustomColorName = "customDefault";
		ribbonTabPage.CustomColorName = "customDefault";
		ribbonTabLayout.CustomColorName = "customDefault";
		biPublish.CustomColorName = "customHigh";
		biShare.CustomColorName = "customDefault";
		biNotifications.CustomColorName = "customDefault";
		biSelectLanguage.CustomColorName = "customDefault";
		biLoginMenu.CustomColorName = "customDefault";
		biLogin.CustomColorName = "customDefault";
		StyleManager.MetroColorGeneratorParameters = MetroColorGeneratorParameters.DarkBlue;
		ribbonControl.RibbonStrip.CaptionContainerItem.SubItems.RemoveAt(0);
		ribbonControl.RibbonStrip.CaptionContainerItem.SubItems.RemoveAt(0);
		ribbonControl.RibbonStrip.BackgroundStyle.BackColor = MetroColorGeneratorParameters.DarkBlue.BaseColor;
		qatCustomizeItem1.Visible = false;
		Color baseColor = MetroColorGeneratorParameters.DarkBlue.BaseColor;
		Office2007ColorTable colorTable = ((Office2007Renderer)GlobalManager.Renderer).ColorTable;
		colorTable.QuickAccessToolbar.Standalone.BottomBackground.Start = ribbonControl.RibbonStrip.BackgroundStyle.BackColor;
		colorTable.QuickAccessToolbar.Standalone.BottomBackground.End = ribbonControl.RibbonStrip.BackgroundStyle.BackColor;
		colorTable.SystemButton.Default.Foreground.Start = Color.White;
		colorTable.SystemButton.Pressed.Foreground = colorTable.SystemButton.MouseOver.Foreground;
		if (colorTable.SystemButtonClose != null)
		{
			colorTable.SystemButtonClose.Default.Foreground.Start = Color.White;
			colorTable.SystemButtonClose.Pressed.Foreground = colorTable.SystemButtonClose.MouseOver.Foreground;
		}
		colorTable.Form.Active.CaptionText = Color.White;
		colorTable.Form.Active.CaptionTopBackground.Start = baseColor;
		colorTable.Form.Active.CaptionBottomBackground.Start = baseColor;
		colorTable.Form.Inactive.CaptionText = Color.Gainsboro;
		colorTable.Form.Inactive.CaptionTopBackground.Start = baseColor;
		colorTable.Form.Inactive.CaptionBottomBackground.Start = baseColor;
		colorTable.ProgressBarItem.OuterBorder = Color.Empty;
		colorTable.ProgressBarItem.BackgroundColors.Colors.Clear();
		if (((Control)Designer).FindForm() is StiRibbonDesigner stiRibbonDesigner)
		{
			stiRibbonDesigner.progressState.BackStyle = stiRibbonDesigner.progressState.GetBackgroundStyle();
			stiRibbonDesigner.progressState.BackStyle.TextColor = Color.White;
		}
		colorTable.RibbonTabItemColors.Add(new Office2007RibbonTabItemColorTable
		{
			Name = "customDefault",
			CornerSize = 0,
			Default = 
			{
				Text = Color.White
			},
			MouseOver = 
			{
				Text = Color.White,
				Background = 
				{
					Start = StiColorUtils.Light(baseColor, 20)
				},
				OuterBorder = 
				{
					Start = StiColorUtils.Light(baseColor, 20)
				}
			},
			Selected = 
			{
				Text = baseColor,
				Background = 
				{
					Start = Color.White
				},
				OuterBorder = colorTable.RibbonControl.OuterBorder
			},
			SelectedMouseOver = 
			{
				Text = baseColor,
				Background = 
				{
					Start = Color.White
				},
				OuterBorder = colorTable.RibbonControl.OuterBorder
			}
		});
		colorTable.RibbonTabItemColors.Add(new Office2007RibbonTabItemColorTable
		{
			Name = "customHigh",
			CornerSize = 0,
			Default = 
			{
				Text = Color.Gainsboro,
				Background = 
				{
					Start = StiColorUtils.Dark(baseColor, 50)
				},
				OuterBorder = 
				{
					Start = StiColorUtils.Dark(baseColor, 50)
				}
			},
			MouseOver = 
			{
				Text = Color.White,
				Background = 
				{
					Start = StiColorUtils.Dark(baseColor, 50)
				},
				OuterBorder = 
				{
					Start = StiColorUtils.Dark(baseColor, 50)
				}
			},
			Selected = 
			{
				Text = baseColor,
				Background = 
				{
					Start = Color.White
				},
				OuterBorder = colorTable.RibbonControl.OuterBorder
			},
			SelectedMouseOver = 
			{
				Text = baseColor,
				Background = 
				{
					Start = Color.White
				},
				OuterBorder = colorTable.RibbonControl.OuterBorder
			}
		});
		colorTable.ButtonItemColors.Add(new Office2007ButtonItemColorTable
		{
			Name = "customDefault",
			Default = new Office2007ButtonItemStateColorTable
			{
				Text = Color.White,
				Background = new LinearGradientColorTable(baseColor)
			},
			MouseOver = new Office2007ButtonItemStateColorTable
			{
				Text = Color.White,
				Background = new LinearGradientColorTable(StiColorUtils.Light(baseColor, 20))
			},
			Pressed = new Office2007ButtonItemStateColorTable
			{
				Text = Color.White,
				Background = new LinearGradientColorTable(StiColorUtils.Light(baseColor, 20))
			},
			Expanded = new Office2007ButtonItemStateColorTable
			{
				Text = Color.White,
				Background = new LinearGradientColorTable(StiColorUtils.Light(baseColor, 20))
			}
		});
		colorTable.ButtonItemColors.Add(new Office2007ButtonItemColorTable
		{
			Name = "customHigh",
			Default = new Office2007ButtonItemStateColorTable
			{
				Text = Color.Gainsboro,
				Background = new LinearGradientColorTable(StiColorUtils.Dark(baseColor, 50))
			},
			MouseOver = new Office2007ButtonItemStateColorTable
			{
				Text = Color.White,
				Background = new LinearGradientColorTable(StiColorUtils.Dark(baseColor, 50))
			},
			Pressed = new Office2007ButtonItemStateColorTable
			{
				Text = Color.White,
				Background = new LinearGradientColorTable(StiColorUtils.Dark(baseColor, 50))
			},
			Expanded = new Office2007ButtonItemStateColorTable
			{
				Text = Color.White,
				Background = new LinearGradientColorTable(StiColorUtils.Dark(baseColor, 50))
			}
		});
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiRibbonDesignerUserControl));
		biQuickReportSave.CustomColorName = "customDefault";
		biQuickUndo.CustomColorName = "customDefault";
		biQuickRedo.CustomColorName = "customDefault";
		biQuickReportSave.Image = (Image)componentResourceManager.GetObject("biQuickReportSaveWhite.Image" + StiScale.StepName);
		biQuickUndo.Image = (Image)componentResourceManager.GetObject("biQuickReportUndoWhite.Image" + StiScale.StepName);
		biQuickRedo.Image = (Image)componentResourceManager.GetObject("biQuickReportRedoWhite.Image" + StiScale.StepName);
		StiRibbonGuiService service2 = StiRibbonGuiService.GetService();
		if (service2 != null && service2.ApplicationMenuItems != null)
		{
			foreach (StiRibbonGuiService.StiApplicationMenuItem applicationMenuItem in service2.ApplicationMenuItems)
			{
				ButtonItem buttonItem3 = new ButtonItem
				{
					ButtonStyle = eButtonStyle.ImageAndText,
					Name = applicationMenuItem.Text,
					Text = applicationMenuItem.Text,
					Tag = applicationMenuItem
				};
				buttonItem3.Click += delegate(object sender, EventArgs e)
				{
					StiRibbonGuiService.StiApplicationMenuItem stiApplicationMenuItem = ((ButtonItem)sender).Tag as StiRibbonGuiService.StiApplicationMenuItem;
					if (stiApplicationMenuItem.Handler != null)
					{
						stiApplicationMenuItem.Handler(Designer, EventArgs.Empty);
					}
				};
				buttonItem3.Image = applicationMenuItem.Image;
				buttonItem3.BeginGroup = applicationMenuItem.BeginGroup;
				if (applicationMenuItem.Index == -1)
				{
					itemContainerMainMenu3.SubItems.Add(buttonItem3);
				}
				else
				{
					itemContainerMainMenu3.SubItems.Insert(applicationMenuItem.Index, buttonItem3);
				}
			}
		}
		if (!ShowQuickToolbar)
		{
			biQuickReportSave.Visible = false;
			biQuickUndo.Visible = false;
			biQuickRedo.Visible = false;
			qatCustomizeItem1.Visible = false;
		}
		BuildWizards();
		BuildTemplates();
		BuildRightDockingPanels();
		if (cbFont != null)
		{
			cbFont.ComboBoxEx.LoadFonts();
		}
		galleryStyles.IsOnRibbonBar = true;
		timer = new Timer
		{
			Interval = 70
		};
		timer.Tick += TimerTick;
		timer.Start();
		if (StiAccountSettings.CurrentAccount == null)
		{
			return;
		}
		ComponentResourceManager componentResourceManager2 = new ComponentResourceManager(typeof(StiRibbonDesignerUserControl));
		biShare.Visible = true;
		biShare.Image = (Image)componentResourceManager2.GetObject("Share" + StiScaleUI.StepName);
		if (StiOptions.Designer.IsDeveloperDesigner)
		{
			biPublish.Visible = true;
		}
		StiAccountSettings.CurrentAccount.ShowNewNotifications += CurrentAccount_ShowNewNotifications;
		StiAccountSettings.CurrentAccount.ShowNewMessage += CurrentAccount_ShowNewMessage;
		try
		{
			IStiNotificationsControl notificationsControl = StiAccountSettings.CurrentAccount.AccountCreater.GetNotificationsControl(((Control)Designer).Handle);
			notificationsControl.ShowBorder(show: true);
			biNotifications.Image = ((notificationsControl.NotificationCount.GetValueOrDefault() > 0) ? ((Image)componentResourceManager2.GetObject("biNotificationsOrange.4Image" + StiScaleUI.StepName)) : ((Image)componentResourceManager2.GetObject("biNotificationsWhite.Image" + StiScaleUI.StepName)));
			ElementHost val = new ElementHost();
			((Control)val).Width = StiScale.XXI(450.0);
			((Control)val).Height = StiScale.YYI(450.0);
			((Control)val).Dock = (DockStyle)5;
			val.Child = (UIElement)((notificationsControl is UIElement) ? notificationsControl : null);
			ElementHost content = val;
			popup = new StiPopup((Control)(object)content);
			notificationsControl.ProcessClose += delegate
			{
				((Control)popup).Hide();
			};
			notificationsControl.NotificationCountChanged += NotificationControl_NotificationCountChanged;
		}
		catch
		{
		}
		BuildLoginMenu();
		if (StiAccountSettings.CurrentAccount != null)
		{
			StiAccountSettings.CurrentAccount.DesignerSpecificationChanged += User_DesignerSpecificationChanged;
		}
	}

	private void User_DesignerSpecificationChanged(object sender, EventArgs e)
	{
		ButtonItem buttonItem = biPublish;
		bool visible = (biQuickInfo.Visible = StiOptions.Designer.IsDeveloperDesigner);
		buttonItem.Visible = visible;
		Designer.InvokeBuidingToolbox();
		RibbonControl obj = Designer.RibbonControl;
		if (obj != null)
		{
			((Control)obj).Refresh();
		}
		((Control)Designer).Refresh();
	}

	private void BuildLoginMenu()
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected O, but got Unknown
		accountMenu = StiAccountSettings.CurrentAccount.AccountCreater.GetAccountLoginMenuControl();
		accountMenu.AccountClick += AccountMenu_AccountClick;
		accountMenu.LogoutClick += AccountMenu_LogoutClick;
		accountMenu.GoToCloudClick += AccountMenu_GoToCloudClick;
		accountMenu.CreateTeamClick += AccountMenu_CreateTeamClick;
		ElementHost val = new ElementHost();
		((Control)val).Width = StiScale.XXI(340.0);
		((Control)val).Height = accountMenu.GetHeightControl();
		((Control)val).Dock = (DockStyle)5;
		_003F val2 = val;
		IStiAccountLoginMenuControl stiAccountLoginMenuControl = accountMenu;
		((ElementHost)val2).Child = (UIElement)((stiAccountLoginMenuControl is UIElement) ? stiAccountLoginMenuControl : null);
		accountMenuElementHost = val;
		StiPopup stiPopup = menuPopup;
		if (stiPopup != null)
		{
			((Control)stiPopup).Hide();
		}
		((Component)(object)menuPopup)?.Dispose();
		StiPopup stiPopup2 = new StiPopup((Control)(object)accountMenuElementHost);
		((ToolStripDropDown)stiPopup2).DropShadowEnabled = false;
		menuPopup = stiPopup2;
	}

	internal void ResetShortcutOpenFile()
	{
		biReportOpen.Shortcuts.Clear();
	}

	internal void InitShortcutOpenFile()
	{
		biReportOpen.Shortcuts.Clear();
		biReportOpen.Shortcuts.Add(eShortcut.CtrlO);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0b8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b97: Expected O, but got Unknown
		//IL_0eac: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d3: Expected O, but got Unknown
		//IL_115c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1166: Expected O, but got Unknown
		//IL_1183: Unknown result type (might be due to invalid IL or missing references)
		//IL_118d: Expected O, but got Unknown
		//IL_120a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1214: Expected O, but got Unknown
		//IL_1231: Unknown result type (might be due to invalid IL or missing references)
		//IL_123b: Expected O, but got Unknown
		//IL_12ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_12b6: Expected O, but got Unknown
		//IL_12d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_12dd: Expected O, but got Unknown
		//IL_179a: Unknown result type (might be due to invalid IL or missing references)
		//IL_17a4: Expected O, but got Unknown
		//IL_17f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1802: Expected O, but got Unknown
		//IL_1856: Unknown result type (might be due to invalid IL or missing references)
		//IL_1860: Expected O, but got Unknown
		//IL_18b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_18be: Expected O, but got Unknown
		//IL_1912: Unknown result type (might be due to invalid IL or missing references)
		//IL_191c: Expected O, but got Unknown
		//IL_1970: Unknown result type (might be due to invalid IL or missing references)
		//IL_197a: Expected O, but got Unknown
		//IL_19ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_19d8: Expected O, but got Unknown
		//IL_1aef: Unknown result type (might be due to invalid IL or missing references)
		//IL_1af9: Expected O, but got Unknown
		//IL_1bb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bbf: Expected O, but got Unknown
		//IL_1ed8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ee2: Expected O, but got Unknown
		//IL_1f3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f44: Expected O, but got Unknown
		//IL_2020: Unknown result type (might be due to invalid IL or missing references)
		//IL_202a: Expected O, but got Unknown
		//IL_208e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2098: Expected O, but got Unknown
		//IL_20fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_2106: Expected O, but got Unknown
		//IL_216a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2174: Expected O, but got Unknown
		//IL_228f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2299: Expected O, but got Unknown
		//IL_264c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2656: Expected O, but got Unknown
		//IL_26ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_26b8: Expected O, but got Unknown
		//IL_2710: Unknown result type (might be due to invalid IL or missing references)
		//IL_271a: Expected O, but got Unknown
		//IL_27e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_27ee: Expected O, but got Unknown
		//IL_2a34: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a3e: Expected O, but got Unknown
		//IL_2b74: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b7e: Expected O, but got Unknown
		//IL_2bd6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2be0: Expected O, but got Unknown
		//IL_2c38: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c42: Expected O, but got Unknown
		//IL_2c9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ca4: Expected O, but got Unknown
		//IL_2d65: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d6f: Expected O, but got Unknown
		//IL_3451: Unknown result type (might be due to invalid IL or missing references)
		//IL_345b: Expected O, but got Unknown
		//IL_34bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_34c9: Expected O, but got Unknown
		//IL_352d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3537: Expected O, but got Unknown
		//IL_359b: Unknown result type (might be due to invalid IL or missing references)
		//IL_35a5: Expected O, but got Unknown
		//IL_3713: Unknown result type (might be due to invalid IL or missing references)
		//IL_371d: Expected O, but got Unknown
		//IL_3775: Unknown result type (might be due to invalid IL or missing references)
		//IL_377f: Expected O, but got Unknown
		//IL_3834: Unknown result type (might be due to invalid IL or missing references)
		//IL_383e: Expected O, but got Unknown
		//IL_39f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_39fb: Expected O, but got Unknown
		//IL_3a18: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a22: Expected O, but got Unknown
		//IL_3af5: Unknown result type (might be due to invalid IL or missing references)
		//IL_3aff: Expected O, but got Unknown
		//IL_3b63: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b6d: Expected O, but got Unknown
		//IL_3bd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bdb: Expected O, but got Unknown
		//IL_3c67: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f66: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f70: Expected O, but got Unknown
		//IL_3fe1: Unknown result type (might be due to invalid IL or missing references)
		//IL_3feb: Expected O, but got Unknown
		//IL_4068: Unknown result type (might be due to invalid IL or missing references)
		//IL_4072: Expected O, but got Unknown
		//IL_40f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_40fe: Expected O, but got Unknown
		//IL_4162: Unknown result type (might be due to invalid IL or missing references)
		//IL_416c: Expected O, but got Unknown
		//IL_423f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4249: Expected O, but got Unknown
		//IL_42ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_42c4: Expected O, but got Unknown
		//IL_4335: Unknown result type (might be due to invalid IL or missing references)
		//IL_433f: Expected O, but got Unknown
		//IL_43bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_43c6: Expected O, but got Unknown
		//IL_43e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_43ed: Expected O, but got Unknown
		//IL_47a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_47b2: Expected O, but got Unknown
		//IL_47cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_47d9: Expected O, but got Unknown
		//IL_4872: Unknown result type (might be due to invalid IL or missing references)
		//IL_487c: Expected O, but got Unknown
		//IL_4899: Unknown result type (might be due to invalid IL or missing references)
		//IL_48a3: Expected O, but got Unknown
		//IL_4a6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a79: Expected O, but got Unknown
		//IL_4a96: Unknown result type (might be due to invalid IL or missing references)
		//IL_4aa0: Expected O, but got Unknown
		//IL_4b42: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b4c: Expected O, but got Unknown
		//IL_4bb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bba: Expected O, but got Unknown
		//IL_4c1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c28: Expected O, but got Unknown
		//IL_4c98: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ca2: Expected O, but got Unknown
		//IL_4cbf: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cc9: Expected O, but got Unknown
		//IL_4d62: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d6c: Expected O, but got Unknown
		//IL_4dd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_4dda: Expected O, but got Unknown
		//IL_4e4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e54: Expected O, but got Unknown
		//IL_4e71: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e7b: Expected O, but got Unknown
		//IL_4f33: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f3d: Expected O, but got Unknown
		//IL_4f5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f64: Expected O, but got Unknown
		//IL_4fef: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ff9: Expected O, but got Unknown
		//IL_505d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5067: Expected O, but got Unknown
		//IL_50cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_50d5: Expected O, but got Unknown
		//IL_5145: Unknown result type (might be due to invalid IL or missing references)
		//IL_514f: Expected O, but got Unknown
		//IL_5209: Unknown result type (might be due to invalid IL or missing references)
		//IL_53c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_53d3: Expected O, but got Unknown
		//IL_53f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_53fa: Expected O, but got Unknown
		//IL_5476: Unknown result type (might be due to invalid IL or missing references)
		//IL_5480: Expected O, but got Unknown
		//IL_549d: Unknown result type (might be due to invalid IL or missing references)
		//IL_54a7: Expected O, but got Unknown
		//IL_5566: Unknown result type (might be due to invalid IL or missing references)
		//IL_5570: Expected O, but got Unknown
		//IL_55c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_55d2: Expected O, but got Unknown
		//IL_562a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5634: Expected O, but got Unknown
		//IL_5698: Unknown result type (might be due to invalid IL or missing references)
		//IL_56a2: Expected O, but got Unknown
		//IL_56fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_5704: Expected O, but got Unknown
		//IL_575c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5766: Expected O, but got Unknown
		//IL_57ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_57d4: Expected O, but got Unknown
		//IL_582c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5836: Expected O, but got Unknown
		//IL_589a: Unknown result type (might be due to invalid IL or missing references)
		//IL_58a4: Expected O, but got Unknown
		//IL_58fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_5906: Expected O, but got Unknown
		//IL_5976: Unknown result type (might be due to invalid IL or missing references)
		//IL_5980: Expected O, but got Unknown
		//IL_599d: Unknown result type (might be due to invalid IL or missing references)
		//IL_59a7: Expected O, but got Unknown
		//IL_5a0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a15: Expected O, but got Unknown
		//IL_5a32: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a3c: Expected O, but got Unknown
		//IL_5aa0: Unknown result type (might be due to invalid IL or missing references)
		//IL_5aaa: Expected O, but got Unknown
		//IL_5ac7: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ad1: Expected O, but got Unknown
		//IL_5b35: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b3f: Expected O, but got Unknown
		//IL_5b5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b66: Expected O, but got Unknown
		//IL_5be2: Unknown result type (might be due to invalid IL or missing references)
		//IL_5bec: Expected O, but got Unknown
		//IL_5c09: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c13: Expected O, but got Unknown
		//IL_5c91: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c9b: Expected O, but got Unknown
		//IL_5cff: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d09: Expected O, but got Unknown
		//IL_5d6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d77: Expected O, but got Unknown
		//IL_5e4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e57: Expected O, but got Unknown
		//IL_5ec7: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ed1: Expected O, but got Unknown
		//IL_60d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_60e1: Expected O, but got Unknown
		//IL_62ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_62b8: Expected O, but got Unknown
		//IL_653b: Unknown result type (might be due to invalid IL or missing references)
		//IL_6545: Expected O, but got Unknown
		//IL_6556: Unknown result type (might be due to invalid IL or missing references)
		//IL_6560: Expected O, but got Unknown
		//IL_65de: Unknown result type (might be due to invalid IL or missing references)
		//IL_65e8: Expected O, but got Unknown
		//IL_65f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_6603: Expected O, but got Unknown
		//IL_6674: Unknown result type (might be due to invalid IL or missing references)
		//IL_667e: Expected O, but got Unknown
		//IL_668f: Unknown result type (might be due to invalid IL or missing references)
		//IL_6699: Expected O, but got Unknown
		//IL_66fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_6704: Expected O, but got Unknown
		//IL_6715: Unknown result type (might be due to invalid IL or missing references)
		//IL_671f: Expected O, but got Unknown
		//IL_678c: Unknown result type (might be due to invalid IL or missing references)
		//IL_6796: Expected O, but got Unknown
		//IL_67a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_67b1: Expected O, but got Unknown
		//IL_6854: Unknown result type (might be due to invalid IL or missing references)
		//IL_685e: Expected O, but got Unknown
		//IL_68e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_68ef: Expected O, but got Unknown
		//IL_6900: Unknown result type (might be due to invalid IL or missing references)
		//IL_690a: Expected O, but got Unknown
		//IL_696e: Unknown result type (might be due to invalid IL or missing references)
		//IL_6978: Expected O, but got Unknown
		//IL_6a11: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a1b: Expected O, but got Unknown
		//IL_6a7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a89: Expected O, but got Unknown
		//IL_6a9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_6aa4: Expected O, but got Unknown
		//IL_6b64: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b6e: Expected O, but got Unknown
		//IL_6b7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b89: Expected O, but got Unknown
		//IL_7029: Unknown result type (might be due to invalid IL or missing references)
		//IL_7033: Expected O, but got Unknown
		//IL_70b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_70ba: Expected O, but got Unknown
		//IL_711f: Unknown result type (might be due to invalid IL or missing references)
		//IL_7129: Expected O, but got Unknown
		//IL_7171: Unknown result type (might be due to invalid IL or missing references)
		//IL_717b: Expected O, but got Unknown
		//IL_71d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_71dd: Expected O, but got Unknown
		//IL_7493: Unknown result type (might be due to invalid IL or missing references)
		//IL_749d: Expected O, but got Unknown
		//IL_74e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_74f1: Expected O, but got Unknown
		//IL_753b: Unknown result type (might be due to invalid IL or missing references)
		//IL_7545: Expected O, but got Unknown
		//IL_759b: Unknown result type (might be due to invalid IL or missing references)
		//IL_75a5: Expected O, but got Unknown
		//IL_75ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_75f9: Expected O, but got Unknown
		//IL_764f: Unknown result type (might be due to invalid IL or missing references)
		//IL_7659: Expected O, but got Unknown
		//IL_789b: Unknown result type (might be due to invalid IL or missing references)
		//IL_78a5: Expected O, but got Unknown
		//IL_78fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_7905: Expected O, but got Unknown
		//IL_7a9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_7aa7: Expected O, but got Unknown
		//IL_7b14: Unknown result type (might be due to invalid IL or missing references)
		//IL_7b1e: Expected O, but got Unknown
		//IL_7b8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_7b95: Expected O, but got Unknown
		//IL_7c1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_7c24: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiRibbonDesignerUserControl));
		ribbonControl = new RibbonControl();
		ribbonPanelHome = new RibbonPanel();
		ribbonBarStyle = new RibbonBar();
		itemContainer26 = new ItemContainer();
		biConditions = new ButtonItem();
		biInteraction = new ButtonItem();
		biCopyStyle = new ButtonItem();
		biStyleDesigner = new ButtonItem();
		galleryStyles = new StiStyleItem();
		buttonItem16 = new ButtonItem();
		buttonItem17 = new ButtonItem();
		ribbonBarTextFormat = new RibbonBar();
		itemContainer34 = new ItemContainer();
		itemContainer50 = new ItemContainer();
		biTextFormat = new ButtonItem();
		biFormatGeneral = new ButtonItem();
		biFormatNumber = new ButtonItem();
		biFormatCurrency = new ButtonItem();
		biFormatDate = new ButtonItem();
		biFormatTime = new ButtonItem();
		biFormatPercentage = new ButtonItem();
		biFormatBoolean = new ButtonItem();
		itemContainer35 = new ItemContainer();
		itemContainer36 = new ItemContainer();
		biCurrencySymbol = new ButtonItem();
		biCurrencySymbolTemp = new ButtonItem();
		biDateTimeFormat = new ButtonItem();
		biDateTimeFormatTemp = new ButtonItem();
		ribbonBarBorders = new RibbonBar();
		itemContainer19 = new ItemContainer();
		itemContainer20 = new ItemContainer();
		itemContainer22 = new ItemContainer();
		biBorderAll = new ButtonItem();
		biBorderNone = new ButtonItem();
		itemContainer23 = new ItemContainer();
		biBorderTop = new ButtonItem();
		biBorderLeft = new ButtonItem();
		biBorderBottom = new ButtonItem();
		biBorderRight = new ButtonItem();
		itemContainer21 = new ItemContainer();
		itemContainer24 = new ItemContainer();
		biBorderShadow = new ButtonItem();
		itemContainer25 = new ItemContainer();
		biFillBrush = new StiColorPickerItem();
		biBorderColor = new StiColorPickerItem();
		ribbonBarAlignment = new RibbonBar();
		itemContainer17 = new ItemContainer();
		itemContainer14 = new ItemContainer();
		itemContainer15 = new ItemContainer();
		biContentAlignTop = new ButtonItem();
		biContentAlignMiddle = new ButtonItem();
		biContentAlignBottom = new ButtonItem();
		itemContainer16 = new ItemContainer();
		biRotateText = new ButtonItem();
		cmRotateAngle0 = new ButtonItem();
		cmRotateAngle45 = new ButtonItem();
		cmRotateAngle90 = new ButtonItem();
		cmRotateAngle180 = new ButtonItem();
		cmRotateAngle270 = new ButtonItem();
		biWordWrap = new ButtonItem();
		itemContainer18 = new ItemContainer();
		itemContainer9 = new ItemContainer();
		biContentAlignLeft = new ButtonItem();
		biContentAlignCenter = new ButtonItem();
		biContentAlignRight = new ButtonItem();
		biContentAlignWidth = new ButtonItem();
		itemContainer40 = new ItemContainer();
		biLineSpacing = new ButtonItem();
		cmLineSpacing1 = new ButtonItem();
		cmLineSpacing115 = new ButtonItem();
		cmLineSpacing15 = new ButtonItem();
		cmLineSpacing2 = new ButtonItem();
		cmLineSpacing25 = new ButtonItem();
		cmLineSpacing3 = new ButtonItem();
		ribbonBarFont = new RibbonBar();
		itemContainer10 = new ItemContainer();
		itemContainer7 = new ItemContainer();
		cbFont = new ComboBoxItem();
		cbFontSize = new ComboBoxItem();
		itemContainer11 = new ItemContainer();
		itemContainer12 = new ItemContainer();
		biFontBold = new ButtonItem();
		biFontItalic = new ButtonItem();
		biFontUnderline = new ButtonItem();
		biFontStrikeout = new ButtonItem();
		itemContainer13 = new ItemContainer();
		biTextBrush = new StiColorPickerItem();
		itemContainer8 = new ItemContainer();
		biFontGrow = new ButtonItem();
		biFontShrink = new ButtonItem();
		itemContainer28 = new ItemContainer();
		biClearContents = new ButtonItem();
		ribbonBarClipboard = new RibbonBar();
		itemContainer5 = new ItemContainer();
		biPaste = new ButtonItem();
		itemContainer6 = new ItemContainer();
		biCut = new ButtonItem();
		biCopy = new ButtonItem();
		biDelete = new ButtonItem();
		ribbonPanelInsert = new RibbonPanel();
		ribbonPanelPage = new RibbonPanel();
		ribbonBarViewOptions = new RibbonBar();
		itemContainer38 = new ItemContainer();
		biViewShowGrid = new ButtonItem();
		biViewAlignToGrid = new ButtonItem();
		biGridMode = new ButtonItem();
		biGridModeLines = new ButtonItem();
		biGridModeDotes = new ButtonItem();
		itemContainer39 = new ItemContainer();
		biViewShowHeaders = new ButtonItem();
		biViewShowOrder = new ButtonItem();
		biViewShowRulers = new ButtonItem();
		biQuickInfo = new ButtonItem();
		biQuickInfoNone = new ButtonItem();
		biQuickInfoShowComponentsNames = new ButtonItem();
		biQuickInfoShowAliases = new ButtonItem();
		biQuickInfoShowContent = new ButtonItem();
		biQuickInfoShowFields = new ButtonItem();
		biQuickInfoShowFieldsOnly = new ButtonItem();
		biQuickInfoShowEvents = new ButtonItem();
		biQuickInfoOverlay = new ButtonItem();
		biDockingPanels = new ButtonItem();
		biDockingPanelsTemp = new ButtonItem();
		biShowToolbox = new ButtonItem();
		ribbonBarPageSetup = new RibbonBar();
		biMargins = new ButtonItem();
		biMarginsNormal = new ButtonItem();
		biMarginsNarrow = new ButtonItem();
		biMarginsWide = new ButtonItem();
		biOrientation = new ButtonItem();
		biOrientationPortrait = new ButtonItem();
		biOrientationLandscape = new ButtonItem();
		biPageSize = new ButtonItem();
		biPageSizeTemp = new ButtonItem();
		biColumns = new ButtonItem();
		biColumnsOne = new ButtonItem();
		biColumnsTwo = new ButtonItem();
		biColumnsThree = new ButtonItem();
		biWatermarkSettings = new ButtonItem();
		ribbonPanelLayout = new RibbonPanel();
		ribbonBarArrange = new RibbonBar();
		biAlignToGrid = new ButtonItem();
		biAlign = new ButtonItem();
		biAlignLeft = new ButtonItem();
		biAlignCenter = new ButtonItem();
		biAlignRight = new ButtonItem();
		biAlignTop = new ButtonItem();
		biAlignMiddle = new ButtonItem();
		biAlignBottom = new ButtonItem();
		biMakeHorizontalSpacingEqual = new ButtonItem();
		biMakeVerticalSpacingEqual = new ButtonItem();
		biCenterHorizontally = new ButtonItem();
		biCenterVertically = new ButtonItem();
		biBringToFront = new ButtonItem();
		biSendToBack = new ButtonItem();
		biMoveForward = new ButtonItem();
		biMoveBackward = new ButtonItem();
		biSize = new ButtonItem();
		biMakeSameSize = new ButtonItem();
		biMakeSameWidth = new ButtonItem();
		biMakeSameHeight = new ButtonItem();
		itemContainer29 = new ItemContainer();
		biLock = new ButtonItem();
		biLink = new ButtonItem();
		ribbonTabHome = new RibbonTabItem();
		ribbonTabInsert = new RibbonTabItem();
		ribbonTabPage = new RibbonTabItem();
		ribbonTabLayout = new RibbonTabItem();
		biPublish = new ButtonItem();
		biShare = new ButtonItem();
		biNotifications = new ButtonItem();
		biSelectLanguage = new ButtonItem();
		buttonItem14 = new ButtonItem();
		biLoginMenu = new ButtonItem();
		buttonItemEmpty = new ButtonItem();
		biLogin = new ButtonItem();
		OfficeStartButton = new Office2007StartButton();
		itemContainerMainMenu = new ItemContainer();
		itemContainerMainMenu2 = new ItemContainer();
		itemContainerMainMenu3 = new ItemContainer();
		biNew = new ButtonItem();
		biBlankReport = new ButtonItem();
		biBlankDashboard = new ButtonItem();
		biBlankScreen = new ButtonItem();
		biReportOpen = new ButtonItem();
		biReportSave = new ButtonItem();
		biSaveAs = new ButtonItem();
		biReportPreview = new ButtonItem();
		biCheckIssues = new ButtonItem();
		biReportSetup = new ButtonItem();
		biAbout = new ButtonItem();
		biClose = new ButtonItem();
		galleryRecentFiles = new GalleryContainer();
		liRecentDocuments = new LabelItem();
		biRecentFile01 = new ButtonItem();
		biRecentFile02 = new ButtonItem();
		biRecentFile03 = new ButtonItem();
		biRecentFile04 = new ButtonItem();
		biRecentFile05 = new ButtonItem();
		biRecentFile06 = new ButtonItem();
		biRecentFile07 = new ButtonItem();
		biRecentFile08 = new ButtonItem();
		biRecentFile09 = new ButtonItem();
		itemContainer4 = new ItemContainer();
		biOptions = new ButtonItem();
		biExit = new ButtonItem();
		biQuickReportSave = new ButtonItem();
		biQuickUndo = new ButtonItem();
		biQuickRedo = new ButtonItem();
		qatCustomizeItem1 = new QatCustomizeItem();
		applicationButton1 = new ApplicationButton();
		itemContainer1 = new ItemContainer();
		itemContainer2 = new ItemContainer();
		itemContainer3 = new ItemContainer();
		buttonItem2 = new ButtonItem();
		buttonItem3 = new ButtonItem();
		buttonItem4 = new ButtonItem();
		buttonItem5 = new ButtonItem();
		buttonItem6 = new ButtonItem();
		buttonItem7 = new ButtonItem();
		galleryContainer1 = new GalleryContainer();
		labelItem8 = new LabelItem();
		buttonItem8 = new ButtonItem();
		buttonItem9 = new ButtonItem();
		buttonItem10 = new ButtonItem();
		buttonItem11 = new ButtonItem();
		buttonItem1 = new ButtonItem();
		buttonItem12 = new ButtonItem();
		buttonItem13 = new ButtonItem();
		itemContainer37 = new ItemContainer();
		ribbonBarView = new RibbonBar();
		biSurfaceViewDesktop = new ButtonItem();
		biSurfaceViewMobile = new ButtonItem();
		biSurfaceViewRemoveMobileSurface = new ButtonItem();
		biSurfaceViewComponents = new ButtonItem();
		ribbonControl.SuspendLayout();
		((Control)ribbonPanelHome).SuspendLayout();
		((Control)ribbonPanelPage).SuspendLayout();
		((Control)ribbonPanelLayout).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)ribbonControl).BackColor = Color.FromArgb(255, 255, 255);
		ribbonControl.BackgroundStyle.CornerType = eCornerType.Square;
		ribbonControl.CaptionVisible = true;
		((Control)ribbonControl).Controls.Add((Control)(object)ribbonPanelPage);
		((Control)ribbonControl).Controls.Add((Control)(object)ribbonPanelInsert);
		((Control)ribbonControl).Controls.Add((Control)(object)ribbonPanelHome);
		((Control)ribbonControl).Controls.Add((Control)(object)ribbonPanelLayout);
		((Control)ribbonControl).Dock = (DockStyle)1;
		((Control)ribbonControl).ForeColor = Color.Black;
		ribbonControl.Items.AddRange(new BaseItem[10] { ribbonTabHome, ribbonTabInsert, ribbonTabPage, ribbonTabLayout, biShare, biPublish, biNotifications, biSelectLanguage, biLoginMenu, biLogin });
		ribbonControl.KeyTipsFont = new Font("Arial", 7f);
		((Control)ribbonControl).Location = new Point(0, 0);
		((Control)ribbonControl).Name = "ribbonControl";
		ribbonControl.Office2007ColorTable = eOffice2007ColorScheme.Blue;
		ribbonControl.QuickToolbarItems.AddRange(new BaseItem[5] { OfficeStartButton, biQuickReportSave, biQuickUndo, biQuickRedo, qatCustomizeItem1 });
		((Control)ribbonControl).Size = new Size(1125, 153);
		ribbonControl.Style = eDotNetBarStyle.StyleManagerControlled;
		ribbonControl.SystemText.MaximizeRibbonText = "&Maximize the Ribbon";
		ribbonControl.SystemText.MinimizeRibbonText = "Mi&nimize the Ribbon";
		ribbonControl.SystemText.QatAddItemText = "&Add to Quick Access Toolbar";
		ribbonControl.SystemText.QatCustomizeMenuLabel = "<b>Customize Quick Access Toolbar</b>";
		ribbonControl.SystemText.QatCustomizeText = "&Customize Quick Access Toolbar...";
		ribbonControl.SystemText.QatDialogAddButton = "&Add >>";
		ribbonControl.SystemText.QatDialogCancelButton = "Cancel";
		ribbonControl.SystemText.QatDialogCaption = "Customize Quick Access Toolbar";
		ribbonControl.SystemText.QatDialogCategoriesLabel = "&Choose commands from:";
		ribbonControl.SystemText.QatDialogOkButton = "OK";
		ribbonControl.SystemText.QatDialogPlacementCheckbox = "&Place Quick Access Toolbar below the Ribbon";
		ribbonControl.SystemText.QatDialogRemoveButton = "&Remove";
		ribbonControl.SystemText.QatPlaceAboveRibbonText = "&Place Quick Access Toolbar above the Ribbon";
		ribbonControl.SystemText.QatPlaceBelowRibbonText = "&Place Quick Access Toolbar below the Ribbon";
		ribbonControl.SystemText.QatRemoveItemText = "&Remove from Quick Access Toolbar";
		ribbonControl.TabGroupHeight = 14;
		ribbonControl.TabGroupsVisible = true;
		((Control)ribbonControl).TabIndex = 0;
		((Control)ribbonControl).Text = "ribbonControl1";
		ribbonControl.AfterAddItemToQuickAccessToolbar += RibbonControl_AfterAddItemToQuickAccessToolbar;
		ribbonControl.AfterRemoveItemFromQuickAccessToolbar += RibbonControl_AfterRemoveItemFromQuickAccessToolbar;
		((Control)ribbonControl).SizeChanged += RibbonControl_SizeChanged;
		ribbonPanelHome.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		((Control)ribbonPanelHome).Controls.Add((Control)(object)ribbonBarStyle);
		((Control)ribbonPanelHome).Controls.Add((Control)(object)ribbonBarTextFormat);
		((Control)ribbonPanelHome).Controls.Add((Control)(object)ribbonBarBorders);
		((Control)ribbonPanelHome).Controls.Add((Control)(object)ribbonBarAlignment);
		((Control)ribbonPanelHome).Controls.Add((Control)(object)ribbonBarFont);
		((Control)ribbonPanelHome).Controls.Add((Control)(object)ribbonBarClipboard);
		((Control)ribbonPanelHome).Dock = (DockStyle)5;
		ribbonPanelHome.Location = new Point(0, 57);
		((Control)ribbonPanelHome).Name = "ribbonPanelHome";
		((Control)ribbonPanelHome).Padding = new Padding(3, 0, 3, 3);
		ribbonPanelHome.Size = new Size(1125, 96);
		ribbonPanelHome.Style.CornerType = eCornerType.Square;
		ribbonPanelHome.StyleMouseDown.CornerType = eCornerType.Square;
		ribbonPanelHome.StyleMouseOver.CornerType = eCornerType.Square;
		((Control)ribbonPanelHome).TabIndex = 1;
		ribbonBarStyle.AutoOverflowEnabled = true;
		ribbonBarStyle.BackgroundMouseOverStyle.CornerType = eCornerType.Square;
		ribbonBarStyle.BackgroundStyle.CornerType = eCornerType.Square;
		ribbonBarStyle.ContainerControlProcessDialogKey = true;
		ribbonBarStyle.DialogLauncherVisible = true;
		((Control)ribbonBarStyle).Dock = (DockStyle)3;
		ribbonBarStyle.Items.AddRange(new BaseItem[2] { itemContainer26, galleryStyles });
		((Control)ribbonBarStyle).Location = new Point(722, 0);
		((Control)ribbonBarStyle).Name = "ribbonBarStyle";
		ribbonBarStyle.OverflowButtonImage = (Image)(object)Resources.home_style;
		((Control)ribbonBarStyle).Size = new Size(324, 93);
		ribbonBarStyle.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)ribbonBarStyle).TabIndex = 5;
		((Control)ribbonBarStyle).Text = "Style";
		ribbonBarStyle.TitleStyle.CornerType = eCornerType.Square;
		ribbonBarStyle.TitleStyleMouseOver.CornerType = eCornerType.Square;
		ribbonBarStyle.VerticalItemAlignment = eVerticalItemsAlignment.Middle;
		ribbonBarStyle.LaunchDialog += ribbonBarStyle_LaunchDialog;
		itemContainer26.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer26.Name = "itemContainer26";
		itemContainer26.SubItems.AddRange(new BaseItem[4] { biConditions, biInteraction, biCopyStyle, biStyleDesigner });
		itemContainer26.TitleStyle.CornerType = eCornerType.Square;
		biConditions.ButtonStyle = eButtonStyle.ImageAndText;
		biConditions.Image = (Image)componentResourceManager.GetObject("biConditions.Image");
		biConditions.ImagePosition = eImagePosition.Top;
		biConditions.KeyTips = "L";
		biConditions.Name = "biConditions";
		biConditions.Stretch = true;
		biConditions.SubItemsExpandWidth = 14;
		biConditions.Text = "Conditions";
		biConditions.Click += biConditions_Click;
		biInteraction.ButtonStyle = eButtonStyle.ImageAndText;
		biInteraction.Image = (Image)componentResourceManager.GetObject("biInteraction.Image");
		biInteraction.ImagePosition = eImagePosition.Top;
		biInteraction.ImageSmall = (Image)componentResourceManager.GetObject("biInteraction.ImageSmall");
		biInteraction.KeyTips = "L";
		biInteraction.Name = "biInteraction";
		biInteraction.Stretch = true;
		biInteraction.SubItemsExpandWidth = 14;
		biInteraction.Text = "Interaction";
		biInteraction.Click += biInteraction_Click;
		biCopyStyle.ButtonStyle = eButtonStyle.ImageAndText;
		biCopyStyle.Image = (Image)componentResourceManager.GetObject("biCopyStyle.Image");
		biCopyStyle.ImagePosition = eImagePosition.Top;
		biCopyStyle.ImageSmall = (Image)componentResourceManager.GetObject("biCopyStyle.ImageSmall");
		biCopyStyle.KeyTips = "C";
		biCopyStyle.Name = "biCopyStyle";
		biCopyStyle.SubItemsExpandWidth = 14;
		biCopyStyle.Text = "Copy Style";
		biCopyStyle.Click += biCopyStyle_Click;
		biStyleDesigner.ButtonStyle = eButtonStyle.ImageAndText;
		biStyleDesigner.Image = (Image)componentResourceManager.GetObject("biStyleDesigner.Image");
		biStyleDesigner.ImagePosition = eImagePosition.Top;
		biStyleDesigner.ImageSmall = (Image)componentResourceManager.GetObject("biStyleDesigner.ImageSmall");
		biStyleDesigner.KeyTips = "G";
		biStyleDesigner.Name = "biStyleDesigner";
		biStyleDesigner.SubItemsExpandWidth = 14;
		biStyleDesigner.Text = "Style Designer";
		biStyleDesigner.Click += biStyleDesigner_Click;
		galleryStyles.FixedSize = new Size(160, 0);
		galleryStyles.GaugeType = null;
		galleryStyles.ImagePosition = eImagePosition.Top;
		galleryStyles.IndicatorFontIcons = StiFontIcons.Rating4;
		galleryStyles.IsDashboardAutoStyle = false;
		galleryStyles.IsOnRibbonBar = false;
		galleryStyles.IsRootItem = false;
		galleryStyles.IsShowMore = false;
		galleryStyles.ShowArrowIfNoSubItems = true;
		galleryStyles.MapIdent = "USA";
		galleryStyles.Name = "galleryStyles";
		galleryStyles.ProgressMode = StiProgressElementMode.Circle;
		galleryStyles.SubItemsExpandWidth = 14;
		galleryStyles.Click += GalleryStyles_Click;
		buttonItem16.Name = "buttonItem16";
		buttonItem16.SubItems.AddRange(new BaseItem[1] { buttonItem17 });
		buttonItem16.Text = "buttonItem16";
		buttonItem17.Name = "buttonItem17";
		buttonItem17.Text = "buttonItem17";
		ribbonBarTextFormat.AutoOverflowEnabled = true;
		ribbonBarTextFormat.BackgroundMouseOverStyle.CornerType = eCornerType.Square;
		ribbonBarTextFormat.BackgroundStyle.CornerType = eCornerType.Square;
		ribbonBarTextFormat.ContainerControlProcessDialogKey = true;
		ribbonBarTextFormat.DialogLauncherKeyTip = "FM";
		ribbonBarTextFormat.DialogLauncherVisible = true;
		((Control)ribbonBarTextFormat).Dock = (DockStyle)3;
		ribbonBarTextFormat.Items.AddRange(new BaseItem[1] { itemContainer34 });
		((Control)ribbonBarTextFormat).Location = new Point(612, 0);
		((Control)ribbonBarTextFormat).Name = "ribbonBarTextFormat";
		ribbonBarTextFormat.OverflowButtonImage = (Image)(object)Resources.home_textformat;
		((Control)ribbonBarTextFormat).Size = new Size(110, 93);
		ribbonBarTextFormat.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)ribbonBarTextFormat).TabIndex = 4;
		((Control)ribbonBarTextFormat).Text = "Text Format";
		ribbonBarTextFormat.TitleStyle.CornerType = eCornerType.Square;
		ribbonBarTextFormat.TitleStyleMouseOver.CornerType = eCornerType.Square;
		ribbonBarTextFormat.VerticalItemAlignment = eVerticalItemsAlignment.Middle;
		ribbonBarTextFormat.LaunchDialog += ribbonBarTextFormat_LaunchDialog;
		itemContainer34.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer34.HorizontalItemAlignment = eHorizontalItemsAlignment.Center;
		itemContainer34.ItemSpacing = 8;
		itemContainer34.LayoutOrientation = eOrientation.Vertical;
		itemContainer34.MinimumSize = new Size(102, 0);
		itemContainer34.Name = "itemContainer34";
		itemContainer34.SubItems.AddRange(new BaseItem[2] { itemContainer50, itemContainer35 });
		itemContainer34.TitleStyle.CornerType = eCornerType.Square;
		itemContainer34.VerticalItemAlignment = eVerticalItemsAlignment.Middle;
		itemContainer50.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer50.BeginGroup = true;
		itemContainer50.HorizontalItemAlignment = eHorizontalItemsAlignment.Center;
		itemContainer50.Name = "itemContainer50";
		itemContainer50.SubItems.AddRange(new BaseItem[1] { biTextFormat });
		itemContainer50.TitleStyle.CornerType = eCornerType.Square;
		biTextFormat.AutoExpandOnClick = true;
		biTextFormat.ButtonStyle = eButtonStyle.ImageAndText;
		biTextFormat.CanCustomize = false;
		biTextFormat.FixedSize = new Size(100, 24);
		biTextFormat.KeyTips = "N";
		biTextFormat.Name = "biTextFormat";
		biTextFormat.Stretch = true;
		biTextFormat.SubItems.AddRange(new BaseItem[7] { biFormatGeneral, biFormatNumber, biFormatCurrency, biFormatDate, biFormatTime, biFormatPercentage, biFormatBoolean });
		biTextFormat.Text = "TextFormat";
		biTextFormat.PopupOpen += biTextFormat_PopupOpen;
		biFormatGeneral.CanCustomize = false;
		biFormatGeneral.Image = (Image)componentResourceManager.GetObject("biFormatGeneral.Image");
		biFormatGeneral.Name = "biFormatGeneral";
		biFormatGeneral.Text = "General";
		biFormatGeneral.Click += biFormatGeneral_Click;
		biFormatNumber.CanCustomize = false;
		biFormatNumber.Image = (Image)componentResourceManager.GetObject("biFormatNumber.Image");
		biFormatNumber.Name = "biFormatNumber";
		biFormatNumber.Text = "Number";
		biFormatNumber.Click += biFormatNumber_Click;
		biFormatCurrency.CanCustomize = false;
		biFormatCurrency.Image = (Image)componentResourceManager.GetObject("biFormatCurrency.Image");
		biFormatCurrency.Name = "biFormatCurrency";
		biFormatCurrency.Text = "Currency";
		biFormatCurrency.Click += biFormatCurrency_Click;
		biFormatDate.CanCustomize = false;
		biFormatDate.Image = (Image)componentResourceManager.GetObject("biFormatDate.Image");
		biFormatDate.Name = "biFormatDate";
		biFormatDate.Text = "Date";
		biFormatDate.Click += biFormatDate_Click;
		biFormatTime.CanCustomize = false;
		biFormatTime.Image = (Image)componentResourceManager.GetObject("biFormatTime.Image");
		biFormatTime.Name = "biFormatTime";
		biFormatTime.Text = "Time";
		biFormatTime.Click += biFormatTime_Click;
		biFormatPercentage.CanCustomize = false;
		biFormatPercentage.Image = (Image)componentResourceManager.GetObject("biFormatPercentage.Image");
		biFormatPercentage.Name = "biFormatPercentage";
		biFormatPercentage.Text = "Percentage";
		biFormatPercentage.Click += biFormatPercentage_Click;
		biFormatBoolean.CanCustomize = false;
		biFormatBoolean.Image = (Image)componentResourceManager.GetObject("biFormatBoolean.Image");
		biFormatBoolean.Name = "biFormatBoolean";
		biFormatBoolean.Text = "Boolean";
		biFormatBoolean.Click += biFormatBoolean_Click;
		itemContainer35.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer35.HorizontalItemAlignment = eHorizontalItemsAlignment.Center;
		itemContainer35.Name = "itemContainer35";
		itemContainer35.SubItems.AddRange(new BaseItem[1] { itemContainer36 });
		itemContainer35.TitleStyle.CornerType = eCornerType.Square;
		itemContainer36.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer36.BeginGroup = true;
		itemContainer36.Name = "itemContainer36";
		itemContainer36.SubItems.AddRange(new BaseItem[2] { biCurrencySymbol, biDateTimeFormat });
		itemContainer36.TitleStyle.CornerType = eCornerType.Square;
		biCurrencySymbol.AutoExpandOnClick = true;
		biCurrencySymbol.Image = (Image)componentResourceManager.GetObject("biCurrencySymbol.Image");
		biCurrencySymbol.ImagePaddingHorizontal = 26;
		biCurrencySymbol.KeyTips = "AN";
		biCurrencySymbol.Name = "biCurrencySymbol";
		biCurrencySymbol.SubItems.AddRange(new BaseItem[1] { biCurrencySymbolTemp });
		biCurrencySymbol.Text = "buttonItem25";
		biCurrencySymbol.PopupOpen += biCurrencySymbol_PopupOpen;
		biCurrencySymbolTemp.CanCustomize = false;
		biCurrencySymbolTemp.Name = "biCurrencySymbolTemp";
		biCurrencySymbolTemp.Text = "buttonItem26";
		biDateTimeFormat.AutoExpandOnClick = true;
		biDateTimeFormat.Image = (Image)componentResourceManager.GetObject("biDateTimeFormat.Image");
		biDateTimeFormat.ImagePaddingHorizontal = 26;
		biDateTimeFormat.KeyTips = "AD";
		biDateTimeFormat.Name = "biDateTimeFormat";
		biDateTimeFormat.SubItems.AddRange(new BaseItem[1] { biDateTimeFormatTemp });
		biDateTimeFormat.Text = "DateTimeFormat";
		biDateTimeFormat.PopupOpen += biDateTimeFormat_PopupOpen;
		biDateTimeFormatTemp.CanCustomize = false;
		biDateTimeFormatTemp.Name = "biDateTimeFormatTemp";
		biDateTimeFormatTemp.Text = "buttonItem16";
		ribbonBarBorders.AutoOverflowEnabled = true;
		ribbonBarBorders.BackgroundMouseOverStyle.CornerType = eCornerType.Square;
		ribbonBarBorders.BackgroundStyle.CornerType = eCornerType.Square;
		ribbonBarBorders.ContainerControlProcessDialogKey = true;
		ribbonBarBorders.DialogLauncherVisible = true;
		((Control)ribbonBarBorders).Dock = (DockStyle)3;
		ribbonBarBorders.Items.AddRange(new BaseItem[1] { itemContainer19 });
		((Control)ribbonBarBorders).Location = new Point(456, 0);
		((Control)ribbonBarBorders).Name = "ribbonBarBorders";
		ribbonBarBorders.OverflowButtonImage = (Image)(object)Resources.home_borders;
		((Control)ribbonBarBorders).Size = new Size(156, 93);
		ribbonBarBorders.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)ribbonBarBorders).TabIndex = 3;
		((Control)ribbonBarBorders).Text = "Borders";
		ribbonBarBorders.TitleStyle.CornerType = eCornerType.Square;
		ribbonBarBorders.TitleStyleMouseOver.CornerType = eCornerType.Square;
		ribbonBarBorders.VerticalItemAlignment = eVerticalItemsAlignment.Middle;
		ribbonBarBorders.LaunchDialog += ribbonBarBorders_LaunchDialog;
		itemContainer19.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer19.ItemSpacing = 8;
		itemContainer19.LayoutOrientation = eOrientation.Vertical;
		itemContainer19.Name = "itemContainer19";
		itemContainer19.SubItems.AddRange(new BaseItem[2] { itemContainer20, itemContainer21 });
		itemContainer19.TitleStyle.CornerType = eCornerType.Square;
		itemContainer19.VerticalItemAlignment = eVerticalItemsAlignment.Middle;
		itemContainer20.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer20.Name = "itemContainer20";
		itemContainer20.SubItems.AddRange(new BaseItem[2] { itemContainer22, itemContainer23 });
		itemContainer20.TitleStyle.CornerType = eCornerType.Square;
		itemContainer22.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer22.BeginGroup = true;
		itemContainer22.Name = "itemContainer22";
		itemContainer22.SubItems.AddRange(new BaseItem[2] { biBorderAll, biBorderNone });
		itemContainer22.TitleStyle.CornerType = eCornerType.Square;
		biBorderAll.Image = (Image)componentResourceManager.GetObject("biBorderAll.Image");
		biBorderAll.KeyTips = "BA";
		biBorderAll.Name = "biBorderAll";
		biBorderAll.Text = "Border All";
		biBorderAll.Click += biBorderAll_Click;
		biBorderNone.Image = (Image)componentResourceManager.GetObject("biBorderNone.Image");
		biBorderNone.KeyTips = "BN";
		biBorderNone.Name = "biBorderNone";
		biBorderNone.Text = "Border None";
		biBorderNone.Click += biBorderNone_Click;
		itemContainer23.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer23.BeginGroup = true;
		itemContainer23.Name = "itemContainer23";
		itemContainer23.SubItems.AddRange(new BaseItem[4] { biBorderTop, biBorderLeft, biBorderBottom, biBorderRight });
		itemContainer23.TitleStyle.CornerType = eCornerType.Square;
		biBorderTop.AutoCheckOnClick = true;
		biBorderTop.Image = (Image)componentResourceManager.GetObject("biBorderTop.Image");
		biBorderTop.KeyTips = "BT";
		biBorderTop.Name = "biBorderTop";
		biBorderTop.Text = "Border Top";
		biBorderTop.Click += biBorderTop_Click;
		biBorderLeft.AutoCheckOnClick = true;
		biBorderLeft.Image = (Image)componentResourceManager.GetObject("biBorderLeft.Image");
		biBorderLeft.KeyTips = "BL";
		biBorderLeft.Name = "biBorderLeft";
		biBorderLeft.Text = "Border Left";
		biBorderLeft.Click += biBorderLeft_Click;
		biBorderBottom.AutoCheckOnClick = true;
		biBorderBottom.Image = (Image)componentResourceManager.GetObject("biBorderBottom.Image");
		biBorderBottom.KeyTips = "BB";
		biBorderBottom.Name = "biBorderBottom";
		biBorderBottom.Text = "Border Bottom";
		biBorderBottom.Click += biBorderBottom_Click;
		biBorderRight.AutoCheckOnClick = true;
		biBorderRight.Image = (Image)componentResourceManager.GetObject("biBorderRight.Image");
		biBorderRight.KeyTips = "BR";
		biBorderRight.Name = "biBorderRight";
		biBorderRight.Text = "Border Right";
		biBorderRight.Click += biBorderRight_Click;
		itemContainer21.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer21.Name = "itemContainer21";
		itemContainer21.SubItems.AddRange(new BaseItem[2] { itemContainer24, itemContainer25 });
		itemContainer21.TitleStyle.CornerType = eCornerType.Square;
		itemContainer24.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer24.BeginGroup = true;
		itemContainer24.Name = "itemContainer24";
		itemContainer24.SubItems.AddRange(new BaseItem[1] { biBorderShadow });
		itemContainer24.TitleStyle.CornerType = eCornerType.Square;
		biBorderShadow.AutoCheckOnClick = true;
		biBorderShadow.Image = (Image)componentResourceManager.GetObject("biBorderShadow.Image");
		biBorderShadow.KeyTips = "BH";
		biBorderShadow.Name = "biBorderShadow";
		biBorderShadow.Text = "Border Shadow";
		biBorderShadow.Click += biBorderShadow_Click;
		itemContainer25.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer25.BeginGroup = true;
		itemContainer25.Name = "itemContainer25";
		itemContainer25.SubItems.AddRange(new BaseItem[2] { biFillBrush, biBorderColor });
		itemContainer25.TitleStyle.CornerType = eCornerType.Square;
		biFillBrush.AutoExpandOnClick = true;
		biFillBrush.KeyTips = "BC";
		biFillBrush.Name = "biFillBrush";
		biFillBrush.Text = "Border Brush";
		biFillBrush.SelectedColorChanged += biFillBrush_SelectedColorChanged;
		biBorderColor.AutoExpandOnClick = true;
		biBorderColor.KeyTips = "BF";
		biBorderColor.Name = "biBorderColor";
		biBorderColor.Text = "Border Color";
		biBorderColor.SelectedColorChanged += biBorderColor_SelectedColorChanged;
		ribbonBarAlignment.AutoOverflowEnabled = true;
		ribbonBarAlignment.BackgroundMouseOverStyle.CornerType = eCornerType.Square;
		ribbonBarAlignment.BackgroundStyle.CornerType = eCornerType.Square;
		ribbonBarAlignment.ContainerControlProcessDialogKey = true;
		((Control)ribbonBarAlignment).Dock = (DockStyle)3;
		ribbonBarAlignment.Items.AddRange(new BaseItem[1] { itemContainer17 });
		((Control)ribbonBarAlignment).Location = new Point(313, 0);
		((Control)ribbonBarAlignment).Name = "ribbonBarAlignment";
		ribbonBarAlignment.OverflowButtonImage = (Image)(object)Resources.home_alignment;
		((Control)ribbonBarAlignment).Size = new Size(143, 93);
		ribbonBarAlignment.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)ribbonBarAlignment).TabIndex = 2;
		((Control)ribbonBarAlignment).Text = "Alignment";
		ribbonBarAlignment.TitleStyle.CornerType = eCornerType.Square;
		ribbonBarAlignment.TitleStyleMouseOver.CornerType = eCornerType.Square;
		ribbonBarAlignment.VerticalItemAlignment = eVerticalItemsAlignment.Middle;
		itemContainer17.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer17.ItemSpacing = 8;
		itemContainer17.LayoutOrientation = eOrientation.Vertical;
		itemContainer17.Name = "itemContainer17";
		itemContainer17.SubItems.AddRange(new BaseItem[2] { itemContainer14, itemContainer18 });
		itemContainer17.TitleStyle.CornerType = eCornerType.Square;
		itemContainer17.VerticalItemAlignment = eVerticalItemsAlignment.Middle;
		itemContainer14.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer14.Name = "itemContainer14";
		itemContainer14.SubItems.AddRange(new BaseItem[2] { itemContainer15, itemContainer16 });
		itemContainer14.TitleStyle.CornerType = eCornerType.Square;
		itemContainer15.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer15.BeginGroup = true;
		itemContainer15.Name = "itemContainer15";
		itemContainer15.SubItems.AddRange(new BaseItem[3] { biContentAlignTop, biContentAlignMiddle, biContentAlignBottom });
		itemContainer15.TitleStyle.CornerType = eCornerType.Square;
		biContentAlignTop.Image = (Image)componentResourceManager.GetObject("biContentAlignTop.Image");
		biContentAlignTop.KeyTips = "AT";
		biContentAlignTop.Name = "biContentAlignTop";
		biContentAlignTop.Text = "Content Align Top";
		biContentAlignTop.Click += biContentAlignTop_Click;
		biContentAlignMiddle.Image = (Image)componentResourceManager.GetObject("biContentAlignMiddle.Image");
		biContentAlignMiddle.KeyTips = "AM";
		biContentAlignMiddle.Name = "biContentAlignMiddle";
		biContentAlignMiddle.Text = "Content Align Middle";
		biContentAlignMiddle.Click += biContentAlignMiddle_Click;
		biContentAlignBottom.Image = (Image)componentResourceManager.GetObject("biContentAlignBottom.Image");
		biContentAlignBottom.KeyTips = "AB";
		biContentAlignBottom.Name = "biContentAlignBottom";
		biContentAlignBottom.Text = "Content Align Bottom";
		biContentAlignBottom.Click += biContentAlignBottom_Click;
		itemContainer16.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer16.BeginGroup = true;
		itemContainer16.Name = "itemContainer16";
		itemContainer16.SubItems.AddRange(new BaseItem[2] { biRotateText, biWordWrap });
		itemContainer16.TitleStyle.CornerType = eCornerType.Square;
		biRotateText.AutoExpandOnClick = true;
		biRotateText.Image = (Image)componentResourceManager.GetObject("biRotateText.Image");
		biRotateText.KeyTips = "FQ";
		biRotateText.Name = "biRotateText";
		biRotateText.SubItems.AddRange(new BaseItem[5] { cmRotateAngle0, cmRotateAngle45, cmRotateAngle90, cmRotateAngle180, cmRotateAngle270 });
		biRotateText.Text = "Rotate Text";
		biRotateText.PopupOpen += biRotateText_PopupOpen;
		cmRotateAngle0.CanCustomize = false;
		cmRotateAngle0.KeyTips = "0";
		cmRotateAngle0.Name = "cmRotateAngle0";
		cmRotateAngle0.Text = "0";
		cmRotateAngle0.Click += cmRotateAngle0_Click;
		cmRotateAngle45.CanCustomize = false;
		cmRotateAngle45.KeyTips = "1";
		cmRotateAngle45.Name = "cmRotateAngle45";
		cmRotateAngle45.Text = "45";
		cmRotateAngle45.Click += cmRotateAngle45_Click;
		cmRotateAngle90.CanCustomize = false;
		cmRotateAngle90.KeyTips = "2";
		cmRotateAngle90.Name = "cmRotateAngle90";
		cmRotateAngle90.Text = "90";
		cmRotateAngle90.Click += cmRotateAngle90_Click;
		cmRotateAngle180.CanCustomize = false;
		cmRotateAngle180.KeyTips = "3";
		cmRotateAngle180.Name = "cmRotateAngle180";
		cmRotateAngle180.Text = "180";
		cmRotateAngle180.Click += cmRotateAngle180_Click;
		cmRotateAngle270.CanCustomize = false;
		cmRotateAngle270.KeyTips = "4";
		cmRotateAngle270.Name = "cmRotateAngle270";
		cmRotateAngle270.Text = "270";
		cmRotateAngle270.Click += cmRotateAngle270_Click;
		biWordWrap.AutoCheckOnClick = true;
		biWordWrap.Image = (Image)componentResourceManager.GetObject("biWordWrap.Image");
		biWordWrap.KeyTips = "W";
		biWordWrap.Name = "biWordWrap";
		biWordWrap.Text = "Word Wrap";
		biWordWrap.Click += biWordWrap_Click;
		itemContainer18.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer18.HorizontalItemAlignment = eHorizontalItemsAlignment.Center;
		itemContainer18.Name = "itemContainer18";
		itemContainer18.SubItems.AddRange(new BaseItem[2] { itemContainer9, itemContainer40 });
		itemContainer18.TitleStyle.CornerType = eCornerType.Square;
		itemContainer9.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer9.BeginGroup = true;
		itemContainer9.Name = "itemContainer9";
		itemContainer9.SubItems.AddRange(new BaseItem[4] { biContentAlignLeft, biContentAlignCenter, biContentAlignRight, biContentAlignWidth });
		itemContainer9.TitleStyle.CornerType = eCornerType.Square;
		biContentAlignLeft.Image = (Image)componentResourceManager.GetObject("biContentAlignLeft.Image");
		biContentAlignLeft.KeyTips = "AL";
		biContentAlignLeft.Name = "biContentAlignLeft";
		biContentAlignLeft.Text = "Content Align Left";
		biContentAlignLeft.Click += biContentAlignLeft_Click;
		biContentAlignCenter.Image = (Image)componentResourceManager.GetObject("biContentAlignCenter.Image");
		biContentAlignCenter.KeyTips = "AC";
		biContentAlignCenter.Name = "biContentAlignCenter";
		biContentAlignCenter.Text = "Content Align Center";
		biContentAlignCenter.Click += biContentAlignCenter_Click;
		biContentAlignRight.Image = (Image)componentResourceManager.GetObject("biContentAlignRight.Image");
		biContentAlignRight.KeyTips = "AR";
		biContentAlignRight.Name = "biContentAlignRight";
		biContentAlignRight.Text = "Content Align Right";
		biContentAlignRight.Click += biContentAlignRight_Click;
		biContentAlignWidth.Image = (Image)componentResourceManager.GetObject("biContentAlignWidth.Image");
		biContentAlignWidth.KeyTips = "AJ";
		biContentAlignWidth.Name = "biContentAlignWidth";
		biContentAlignWidth.Text = "Content Align Width";
		biContentAlignWidth.Click += biContentAlignWidth_Click;
		itemContainer40.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer40.BeginGroup = true;
		itemContainer40.Name = "itemContainer40";
		itemContainer40.SubItems.AddRange(new BaseItem[1] { biLineSpacing });
		itemContainer40.TitleStyle.CornerType = eCornerType.Square;
		biLineSpacing.AutoExpandOnClick = true;
		biLineSpacing.Image = (Image)componentResourceManager.GetObject("biLineSpacing.Image");
		biLineSpacing.KeyTips = "FQ";
		biLineSpacing.Name = "biLineSpacing";
		biLineSpacing.SubItems.AddRange(new BaseItem[6] { cmLineSpacing1, cmLineSpacing115, cmLineSpacing15, cmLineSpacing2, cmLineSpacing25, cmLineSpacing3 });
		biLineSpacing.Text = "Line Spacing";
		biLineSpacing.PopupOpen += biLineSpacing_PopupOpen;
		cmLineSpacing1.CanCustomize = false;
		cmLineSpacing1.KeyTips = "0";
		cmLineSpacing1.Name = "cmLineSpacing1";
		cmLineSpacing1.Text = "1.0";
		cmLineSpacing1.Click += cmLineSpacing1_Click;
		cmLineSpacing115.CanCustomize = false;
		cmLineSpacing115.KeyTips = "0";
		cmLineSpacing115.Name = "cmLineSpacing115";
		cmLineSpacing115.Text = "1.15";
		cmLineSpacing115.Click += cmLineSpacing115_Click;
		cmLineSpacing15.CanCustomize = false;
		cmLineSpacing15.KeyTips = "0";
		cmLineSpacing15.Name = "cmLineSpacing15";
		cmLineSpacing15.Text = "1.5";
		cmLineSpacing15.Click += cmLineSpacing15_Click;
		cmLineSpacing2.CanCustomize = false;
		cmLineSpacing2.KeyTips = "0";
		cmLineSpacing2.Name = "cmLineSpacing2";
		cmLineSpacing2.Text = "2.0";
		cmLineSpacing2.Click += cmLineSpacing2_Click;
		cmLineSpacing25.CanCustomize = false;
		cmLineSpacing25.KeyTips = "0";
		cmLineSpacing25.Name = "cmLineSpacing25";
		cmLineSpacing25.Text = "2.5";
		cmLineSpacing25.Click += cmLineSpacing25_Click;
		cmLineSpacing3.CanCustomize = false;
		cmLineSpacing3.KeyTips = "0";
		cmLineSpacing3.Name = "cmLineSpacing3";
		cmLineSpacing3.Text = "3.0";
		cmLineSpacing3.Click += cmLineSpacing3_Click;
		ribbonBarFont.AutoOverflowEnabled = true;
		ribbonBarFont.BackgroundMouseOverStyle.CornerType = eCornerType.Square;
		ribbonBarFont.BackgroundStyle.CornerType = eCornerType.Square;
		ribbonBarFont.ContainerControlProcessDialogKey = true;
		((Control)ribbonBarFont).Dock = (DockStyle)3;
		ribbonBarFont.Items.AddRange(new BaseItem[1] { itemContainer10 });
		((Control)ribbonBarFont).Location = new Point(116, 0);
		((Control)ribbonBarFont).Name = "ribbonBarFont";
		ribbonBarFont.OverflowButtonImage = (Image)(object)Resources.home_font;
		((Control)ribbonBarFont).Size = new Size(197, 93);
		ribbonBarFont.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)ribbonBarFont).TabIndex = 1;
		((Control)ribbonBarFont).Text = "Font";
		ribbonBarFont.TitleStyle.CornerType = eCornerType.Square;
		ribbonBarFont.TitleStyleMouseOver.CornerType = eCornerType.Square;
		ribbonBarFont.VerticalItemAlignment = eVerticalItemsAlignment.Middle;
		itemContainer10.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer10.ItemSpacing = 8;
		itemContainer10.LayoutOrientation = eOrientation.Vertical;
		itemContainer10.Name = "itemContainer10";
		itemContainer10.SubItems.AddRange(new BaseItem[2] { itemContainer7, itemContainer11 });
		itemContainer10.TitleStyle.CornerType = eCornerType.Square;
		itemContainer10.VerticalItemAlignment = eVerticalItemsAlignment.Middle;
		itemContainer7.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer7.Name = "itemContainer7";
		itemContainer7.SubItems.AddRange(new BaseItem[2] { cbFont, cbFontSize });
		itemContainer7.TitleStyle.CornerType = eCornerType.Square;
		cbFont.Caption = "comboBoxItem1";
		cbFont.ComboWidth = 127;
		cbFont.DropDownHeight = 500;
		cbFont.FontCombo = true;
		cbFont.KeyTips = "FF";
		cbFont.Name = "cbFont";
		cbFontSize.Caption = "comboBoxItem2";
		cbFontSize.ComboWidth = 50;
		cbFontSize.DropDownHeight = 106;
		cbFontSize.DropDownStyle = (ComboBoxStyle)1;
		cbFontSize.Items.AddRange(new object[19]
		{
			"5", "6", "7", "8", "9", "10", "11", "12", "14", "16",
			"18", "20", "22", "24", "26", "28", "36", "48", "72"
		});
		cbFontSize.KeyTips = "FS";
		cbFontSize.Name = "cbFontSize";
		cbFontSize.SelectedIndexChanged += cbFontSize_SelectedIndexChanged;
		itemContainer11.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer11.Name = "itemContainer11";
		itemContainer11.SubItems.AddRange(new BaseItem[4] { itemContainer12, itemContainer13, itemContainer8, itemContainer28 });
		itemContainer11.TitleStyle.CornerType = eCornerType.Square;
		itemContainer12.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer12.BeginGroup = true;
		itemContainer12.Name = "itemContainer12";
		itemContainer12.SubItems.AddRange(new BaseItem[4] { biFontBold, biFontItalic, biFontUnderline, biFontStrikeout });
		itemContainer12.TitleStyle.CornerType = eCornerType.Square;
		biFontBold.AutoCheckOnClick = true;
		biFontBold.Image = (Image)componentResourceManager.GetObject("biFontBold.Image");
		biFontBold.KeyTips = "1";
		biFontBold.Name = "biFontBold";
		biFontBold.Text = "Font Bold";
		biFontBold.Click += biFontBold_Click;
		biFontItalic.AutoCheckOnClick = true;
		biFontItalic.Image = (Image)componentResourceManager.GetObject("biFontItalic.Image");
		biFontItalic.KeyTips = "2";
		biFontItalic.Name = "biFontItalic";
		biFontItalic.Text = "Font Italic";
		biFontItalic.Click += biFontItalic_Click;
		biFontUnderline.AutoCheckOnClick = true;
		biFontUnderline.Image = (Image)componentResourceManager.GetObject("biFontUnderline.Image");
		biFontUnderline.KeyTips = "3";
		biFontUnderline.Name = "biFontUnderline";
		biFontUnderline.Text = "Font Underline";
		biFontUnderline.Click += biFontUnderline_Click;
		biFontStrikeout.AutoCheckOnClick = true;
		biFontStrikeout.Image = (Image)componentResourceManager.GetObject("biFontUnderline.Image");
		biFontStrikeout.KeyTips = "4";
		biFontStrikeout.Name = "biFontStrikeout";
		biFontStrikeout.Text = "Font Strikeout";
		biFontStrikeout.Click += biFontStrikeout_Click;
		itemContainer13.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer13.BeginGroup = true;
		itemContainer13.Name = "itemContainer13";
		itemContainer13.SubItems.AddRange(new BaseItem[1] { biTextBrush });
		itemContainer13.TitleStyle.CornerType = eCornerType.Square;
		biTextBrush.AutoExpandOnClick = true;
		biTextBrush.KeyTips = "FC";
		biTextBrush.Name = "biTextBrush";
		biTextBrush.Text = "Font Color";
		biTextBrush.SelectedColorChanged += biTextBrush_SelectedColorChanged;
		itemContainer8.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer8.BeginGroup = true;
		itemContainer8.Name = "itemContainer8";
		itemContainer8.SubItems.AddRange(new BaseItem[2] { biFontGrow, biFontShrink });
		itemContainer8.TitleStyle.CornerType = eCornerType.Square;
		biFontGrow.Image = (Image)componentResourceManager.GetObject("biFontGrow.Image");
		biFontGrow.KeyTips = "FG";
		biFontGrow.Name = "biFontGrow";
		biFontGrow.Text = "Grow Font";
		biFontGrow.Click += biFontGrow_Click;
		biFontShrink.Image = (Image)componentResourceManager.GetObject("biFontShrink.Image");
		biFontShrink.KeyTips = "FK";
		biFontShrink.Name = "biFontShrink";
		biFontShrink.Text = "Shrink Font";
		biFontShrink.Click += biFontShrink_Click;
		itemContainer28.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer28.BeginGroup = true;
		itemContainer28.Name = "itemContainer28";
		itemContainer28.SubItems.AddRange(new BaseItem[1] { biClearContents });
		itemContainer28.TitleStyle.CornerType = eCornerType.Square;
		biClearContents.Image = (Image)componentResourceManager.GetObject("biClearContents.Image");
		biClearContents.KeyTips = "E";
		biClearContents.Name = "biClearContents";
		biClearContents.Text = "Clear Contents";
		biClearContents.Click += biClearContents_Click;
		ribbonBarClipboard.AutoOverflowEnabled = true;
		ribbonBarClipboard.BackgroundMouseOverStyle.CornerType = eCornerType.Square;
		ribbonBarClipboard.BackgroundStyle.CornerType = eCornerType.Square;
		ribbonBarClipboard.ContainerControlProcessDialogKey = true;
		((Control)ribbonBarClipboard).Dock = (DockStyle)3;
		ribbonBarClipboard.Items.AddRange(new BaseItem[2] { itemContainer5, itemContainer6 });
		((Control)ribbonBarClipboard).Location = new Point(3, 0);
		((Control)ribbonBarClipboard).Name = "ribbonBarClipboard";
		ribbonBarClipboard.OverflowButtonImage = (Image)(object)Resources.home_clipboard;
		((Control)ribbonBarClipboard).Size = new Size(113, 93);
		ribbonBarClipboard.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)ribbonBarClipboard).TabIndex = 0;
		((Control)ribbonBarClipboard).Text = "Clipboard";
		ribbonBarClipboard.TitleStyle.CornerType = eCornerType.Square;
		ribbonBarClipboard.TitleStyleMouseOver.CornerType = eCornerType.Square;
		itemContainer5.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer5.LayoutOrientation = eOrientation.Vertical;
		itemContainer5.Name = "itemContainer5";
		itemContainer5.SubItems.AddRange(new BaseItem[1] { biPaste });
		itemContainer5.TitleStyle.CornerType = eCornerType.Square;
		biPaste.Image = (Image)componentResourceManager.GetObject("biPaste.Image");
		biPaste.ImagePosition = eImagePosition.Top;
		biPaste.ImageSmall = (Image)componentResourceManager.GetObject("biPaste.ImageSmall");
		biPaste.KeyTips = "V";
		biPaste.Name = "biPaste";
		biPaste.Text = "Paste";
		biPaste.Click += biPaste_Click;
		itemContainer6.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer6.LayoutOrientation = eOrientation.Vertical;
		itemContainer6.Name = "itemContainer6";
		itemContainer6.SubItems.AddRange(new BaseItem[3] { biCut, biCopy, biDelete });
		itemContainer6.TitleStyle.CornerType = eCornerType.Square;
		biCut.ButtonStyle = eButtonStyle.ImageAndText;
		biCut.Image = (Image)componentResourceManager.GetObject("biCut.Image");
		biCut.KeyTips = "X";
		biCut.Name = "biCut";
		biCut.Text = "Cut";
		biCut.Click += biCut_Click;
		biCopy.ButtonStyle = eButtonStyle.ImageAndText;
		biCopy.Image = (Image)componentResourceManager.GetObject("biCopy.Image");
		biCopy.KeyTips = "C";
		biCopy.Name = "biCopy";
		biCopy.Text = "Copy";
		biCopy.Click += biCopy_Click;
		biDelete.ButtonStyle = eButtonStyle.ImageAndText;
		biDelete.Image = (Image)componentResourceManager.GetObject("biDelete.Image");
		biDelete.KeyTips = "D";
		biDelete.Name = "biDelete";
		biDelete.Text = "Delete";
		biDelete.Click += biDelete_Click;
		ribbonPanelInsert.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		((Control)ribbonPanelInsert).Dock = (DockStyle)5;
		ribbonPanelInsert.Location = new Point(0, 57);
		((Control)ribbonPanelInsert).Name = "ribbonPanelInsert";
		((Control)ribbonPanelInsert).Padding = new Padding(3, 0, 3, 3);
		ribbonPanelInsert.Size = new Size(1125, 90);
		ribbonPanelInsert.Style.CornerType = eCornerType.Square;
		ribbonPanelInsert.StyleMouseDown.CornerType = eCornerType.Square;
		ribbonPanelInsert.StyleMouseOver.CornerType = eCornerType.Square;
		((Control)ribbonPanelInsert).TabIndex = 2;
		ribbonPanelPage.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		((Control)ribbonPanelPage).Controls.Add((Control)(object)ribbonBarViewOptions);
		((Control)ribbonPanelPage).Controls.Add((Control)(object)ribbonBarView);
		((Control)ribbonPanelPage).Controls.Add((Control)(object)ribbonBarPageSetup);
		((Control)ribbonPanelPage).Dock = (DockStyle)5;
		ribbonPanelPage.Location = new Point(0, 57);
		((Control)ribbonPanelPage).Name = "ribbonPanelPage";
		((Control)ribbonPanelPage).Padding = new Padding(3, 0, 3, 3);
		ribbonPanelPage.Size = new Size(1125, 90);
		ribbonPanelPage.Style.CornerType = eCornerType.Square;
		ribbonPanelPage.StyleMouseDown.CornerType = eCornerType.Square;
		ribbonPanelPage.StyleMouseOver.CornerType = eCornerType.Square;
		((Control)ribbonPanelPage).TabIndex = 3;
		ribbonPanelPage.Visible = false;
		ribbonBarViewOptions.AutoOverflowEnabled = true;
		ribbonBarViewOptions.BackgroundMouseOverStyle.CornerType = eCornerType.Square;
		ribbonBarViewOptions.BackgroundStyle.CornerType = eCornerType.Square;
		ribbonBarViewOptions.ContainerControlProcessDialogKey = true;
		((Control)ribbonBarViewOptions).Dock = (DockStyle)3;
		ribbonBarViewOptions.Items.AddRange(new BaseItem[5] { itemContainer38, itemContainer39, biQuickInfo, biDockingPanels, biShowToolbox });
		((Control)ribbonBarViewOptions).Location = new Point(310, 0);
		((Control)ribbonBarViewOptions).Name = "ribbonBarViewOptions";
		ribbonBarViewOptions.OverflowButtonImage = (Image)(object)Resources.view_viewoptions;
		((Control)ribbonBarViewOptions).Size = new Size(372, 87);
		ribbonBarViewOptions.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)ribbonBarViewOptions).TabIndex = 0;
		((Control)ribbonBarViewOptions).Text = "View Options";
		ribbonBarViewOptions.TitleStyle.CornerType = eCornerType.Square;
		ribbonBarViewOptions.TitleStyleMouseOver.CornerType = eCornerType.Square;
		itemContainer38.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer38.LayoutOrientation = eOrientation.Vertical;
		itemContainer38.Name = "itemContainer38";
		itemContainer38.SubItems.AddRange(new BaseItem[3] { biViewShowGrid, biViewAlignToGrid, biGridMode });
		itemContainer38.TitleStyle.CornerType = eCornerType.Square;
		biViewShowGrid.ButtonStyle = eButtonStyle.ImageAndText;
		biViewShowGrid.Image = (Image)componentResourceManager.GetObject("biViewShowGrid.Image");
		biViewShowGrid.KeyTips = "SG";
		biViewShowGrid.Name = "biViewShowGrid";
		biViewShowGrid.SubItemsExpandWidth = 14;
		biViewShowGrid.Text = "Show Grid";
		biViewShowGrid.Click += biViewShowGrid_Click;
		biViewAlignToGrid.ButtonStyle = eButtonStyle.ImageAndText;
		biViewAlignToGrid.Image = (Image)componentResourceManager.GetObject("biViewAlignToGrid.Image");
		biViewAlignToGrid.KeyTips = "AG";
		biViewAlignToGrid.Name = "biViewAlignToGrid";
		biViewAlignToGrid.SubItemsExpandWidth = 14;
		biViewAlignToGrid.Text = "Align to Grid";
		biViewAlignToGrid.Click += biViewAlignToGrid_Click;
		biGridMode.AutoExpandOnClick = true;
		biGridMode.ButtonStyle = eButtonStyle.ImageAndText;
		biGridMode.Image = (Image)componentResourceManager.GetObject("biGridMode.Image");
		biGridMode.KeyTips = "GM";
		biGridMode.Name = "biGridMode";
		biGridMode.SubItems.AddRange(new BaseItem[2] { biGridModeLines, biGridModeDotes });
		biGridMode.SubItemsExpandWidth = 14;
		biGridMode.Text = "Grid Mode";
		biGridModeLines.CanCustomize = false;
		biGridModeLines.Image = (Image)componentResourceManager.GetObject("biGridModeLines.Image");
		biGridModeLines.KeyTips = "L";
		biGridModeLines.Name = "biGridModeLines";
		biGridModeLines.Text = "Lines";
		biGridModeLines.Click += biGridModeLines_Click;
		biGridModeDotes.CanCustomize = false;
		biGridModeDotes.Image = (Image)componentResourceManager.GetObject("biGridModeDotes.Image");
		biGridModeDotes.KeyTips = "D";
		biGridModeDotes.Name = "biGridModeDotes";
		biGridModeDotes.Text = "Dotes";
		biGridModeDotes.Click += biGridModeDotes_Click;
		itemContainer39.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer39.LayoutOrientation = eOrientation.Vertical;
		itemContainer39.Name = "itemContainer39";
		itemContainer39.SubItems.AddRange(new BaseItem[3] { biViewShowHeaders, biViewShowOrder, biViewShowRulers });
		itemContainer39.TitleStyle.CornerType = eCornerType.Square;
		biViewShowHeaders.ButtonStyle = eButtonStyle.ImageAndText;
		biViewShowHeaders.Image = (Image)componentResourceManager.GetObject("biViewShowHeaders.Image");
		biViewShowHeaders.KeyTips = "SH";
		biViewShowHeaders.Name = "biViewShowHeaders";
		biViewShowHeaders.SubItemsExpandWidth = 14;
		biViewShowHeaders.Text = "Show Headers";
		biViewShowHeaders.Click += biViewShowHeaders_Click;
		biViewShowOrder.ButtonStyle = eButtonStyle.ImageAndText;
		biViewShowOrder.Image = (Image)componentResourceManager.GetObject("biViewShowOrder.Image");
		biViewShowOrder.KeyTips = "SO";
		biViewShowOrder.Name = "biViewShowOrder";
		biViewShowOrder.SubItemsExpandWidth = 14;
		biViewShowOrder.Text = "Show Order";
		biViewShowOrder.Click += biViewShowOrder_Click;
		biViewShowRulers.ButtonStyle = eButtonStyle.ImageAndText;
		biViewShowRulers.Image = (Image)componentResourceManager.GetObject("biViewShowRulers.Image");
		biViewShowRulers.KeyTips = "SR";
		biViewShowRulers.Name = "biViewShowRulers";
		biViewShowRulers.SubItemsExpandWidth = 14;
		biViewShowRulers.Text = "Show Rulers";
		biViewShowRulers.Click += biViewShowRulers_Click;
		biQuickInfo.AutoExpandOnClick = true;
		biQuickInfo.ButtonStyle = eButtonStyle.ImageAndText;
		biQuickInfo.Image = (Image)componentResourceManager.GetObject("biQuickInfo.Image");
		biQuickInfo.ImagePosition = eImagePosition.Top;
		biQuickInfo.ImageSmall = (Image)componentResourceManager.GetObject("biQuickInfo.ImageSmall");
		biQuickInfo.KeyTips = "Q";
		biQuickInfo.Name = "biQuickInfo";
		biQuickInfo.RibbonWordWrap = true;
		biQuickInfo.SubItems.AddRange(new BaseItem[8] { biQuickInfoNone, biQuickInfoShowComponentsNames, biQuickInfoShowAliases, biQuickInfoShowContent, biQuickInfoShowFields, biQuickInfoShowFieldsOnly, biQuickInfoShowEvents, biQuickInfoOverlay });
		biQuickInfo.SubItemsExpandWidth = 14;
		biQuickInfo.Text = "Quick Info";
		biQuickInfo.Visible = false;
		biQuickInfo.PopupOpen += biQuickInfo_PopupOpen;
		biQuickInfoNone.AutoCollapseOnClick = false;
		biQuickInfoNone.KeyTips = "N";
		biQuickInfoNone.Name = "biQuickInfoNone";
		biQuickInfoNone.Text = "None";
		biQuickInfoNone.Click += ciQuickInfoNone_Click;
		biQuickInfoShowComponentsNames.AutoCollapseOnClick = false;
		biQuickInfoShowComponentsNames.KeyTips = "M";
		biQuickInfoShowComponentsNames.Name = "biQuickInfoShowComponentsNames";
		biQuickInfoShowComponentsNames.Text = "Show Components Names";
		biQuickInfoShowComponentsNames.Click += ciQuickInfoShowComponentsNames_Click;
		biQuickInfoShowAliases.AutoCollapseOnClick = false;
		biQuickInfoShowAliases.KeyTips = "A";
		biQuickInfoShowAliases.Name = "biQuickInfoShowAliases";
		biQuickInfoShowAliases.Text = "Show Aliases";
		biQuickInfoShowAliases.Click += ciQuickInfoShowAliases_Click;
		biQuickInfoShowContent.AutoCollapseOnClick = false;
		biQuickInfoShowContent.KeyTips = "C";
		biQuickInfoShowContent.Name = "biQuickInfoShowContent";
		biQuickInfoShowContent.Text = "Show Content";
		biQuickInfoShowContent.Click += ciQuickInfoShowContent_Click;
		biQuickInfoShowFields.AutoCollapseOnClick = false;
		biQuickInfoShowFields.KeyTips = "F";
		biQuickInfoShowFields.Name = "biQuickInfoShowFields";
		biQuickInfoShowFields.Text = "Show Fields";
		biQuickInfoShowFields.Click += ciQuickInfoShowFields_Click;
		biQuickInfoShowFieldsOnly.AutoCollapseOnClick = false;
		biQuickInfoShowFieldsOnly.KeyTips = "O";
		biQuickInfoShowFieldsOnly.Name = "biQuickInfoShowFieldsOnly";
		biQuickInfoShowFieldsOnly.Text = "Show Fields Only";
		biQuickInfoShowFieldsOnly.Click += ciQuickInfoShowFieldsOnly_Click;
		biQuickInfoShowEvents.AutoCollapseOnClick = false;
		biQuickInfoShowEvents.KeyTips = "E";
		biQuickInfoShowEvents.Name = "biQuickInfoShowEvents";
		biQuickInfoShowEvents.Text = "Show Events";
		biQuickInfoShowEvents.Click += ciQuickInfoShowEvents_Click;
		biQuickInfoOverlay.AutoCheckOnClick = true;
		biQuickInfoOverlay.AutoCollapseOnClick = false;
		biQuickInfoOverlay.BeginGroup = true;
		biQuickInfoOverlay.KeyTips = "L";
		biQuickInfoOverlay.Name = "biQuickInfoOverlay";
		biQuickInfoOverlay.Text = "Overlay";
		biQuickInfoOverlay.Click += ciQuickInfoOverlay_Click;
		biDockingPanels.AutoExpandOnClick = true;
		biDockingPanels.BeginGroup = true;
		biDockingPanels.ButtonStyle = eButtonStyle.ImageAndText;
		biDockingPanels.CanCustomize = false;
		biDockingPanels.Image = (Image)componentResourceManager.GetObject("biDockingPanels.Image");
		biDockingPanels.ImagePosition = eImagePosition.Top;
		biDockingPanels.ImageSmall = (Image)componentResourceManager.GetObject("biDockingPanels.ImageSmall");
		biDockingPanels.KeyTips = "P";
		biDockingPanels.Name = "biDockingPanels";
		biDockingPanels.SubItems.AddRange(new BaseItem[1] { biDockingPanelsTemp });
		biDockingPanels.SubItemsExpandWidth = 14;
		biDockingPanels.Text = "Panels";
		biDockingPanelsTemp.Name = "biDockingPanelsTemp";
		biDockingPanelsTemp.Text = "buttonItem15";
		biShowToolbox.ButtonStyle = eButtonStyle.ImageAndText;
		biShowToolbox.Image = (Image)componentResourceManager.GetObject("biShowToolbox.Image");
		biShowToolbox.ImagePosition = eImagePosition.Top;
		biShowToolbox.ImageSmall = (Image)componentResourceManager.GetObject("biShowToolbox.ImageSmall");
		biShowToolbox.KeyTips = "L";
		biShowToolbox.Name = "biShowToolbox";
		biShowToolbox.SubItemsExpandWidth = 14;
		biShowToolbox.Text = "Toolbox";
		biShowToolbox.Click += biShowToolbox_Click;
		ribbonBarPageSetup.AutoOverflowEnabled = true;
		ribbonBarPageSetup.BackgroundMouseOverStyle.CornerType = eCornerType.Square;
		ribbonBarPageSetup.BackgroundStyle.CornerType = eCornerType.Square;
		ribbonBarPageSetup.ContainerControlProcessDialogKey = true;
		ribbonBarPageSetup.DialogLauncherKeyTip = "SP";
		ribbonBarPageSetup.DialogLauncherVisible = true;
		((Control)ribbonBarPageSetup).Dock = (DockStyle)3;
		ribbonBarPageSetup.Items.AddRange(new BaseItem[5] { biMargins, biOrientation, biPageSize, biColumns, biWatermarkSettings });
		((Control)ribbonBarPageSetup).Location = new Point(3, 0);
		((Control)ribbonBarPageSetup).Name = "ribbonBarPageSetup";
		ribbonBarPageSetup.OverflowButtonImage = (Image)(object)Resources.page_pagesetup;
		((Control)ribbonBarPageSetup).Size = new Size(307, 87);
		ribbonBarPageSetup.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)ribbonBarPageSetup).TabIndex = 0;
		((Control)ribbonBarPageSetup).Text = "Page Setup";
		ribbonBarPageSetup.TitleStyle.CornerType = eCornerType.Square;
		ribbonBarPageSetup.TitleStyleMouseOver.CornerType = eCornerType.Square;
		ribbonBarPageSetup.LaunchDialog += ribbonBarPageSetup_LaunchDialog;
		biMargins.AutoExpandOnClick = true;
		biMargins.ButtonStyle = eButtonStyle.ImageAndText;
		biMargins.Image = (Image)componentResourceManager.GetObject("biMargins.Image");
		biMargins.ImagePosition = eImagePosition.Top;
		biMargins.ImageSmall = (Image)componentResourceManager.GetObject("biMargins.ImageSmall");
		biMargins.KeyTips = "M";
		biMargins.Name = "biMargins";
		biMargins.SubItems.AddRange(new BaseItem[3] { biMarginsNormal, biMarginsNarrow, biMarginsWide });
		biMargins.SubItemsExpandWidth = 14;
		biMargins.Text = "Margins";
		biMargins.PopupOpen += biMargins_PopupOpen;
		biMarginsNormal.CanCustomize = false;
		biMarginsNormal.Image = (Image)componentResourceManager.GetObject("biMarginsNormal.Image");
		biMarginsNormal.KeyTips = "N";
		biMarginsNormal.Name = "biMarginsNormal";
		biMarginsNormal.Text = "<br><b>Normal</b></br>\r\n<br>Left: 10cm,  Right: 10cm</br>\r\n<br>Top: 10cm,  Bottom: 10cm</br>\r\n<br> </br>";
		biMarginsNormal.Click += biMarginsNormal_Click;
		biMarginsNarrow.CanCustomize = false;
		biMarginsNarrow.Image = (Image)componentResourceManager.GetObject("biMarginsNarrow.Image");
		biMarginsNarrow.KeyTips = "R";
		biMarginsNarrow.Name = "biMarginsNarrow";
		biMarginsNarrow.Text = "<br><b>Narrow</b></br>\r\n<br>Left: 10cm,  Right: 10cm</br>\r\n<br>Top: 10cm,  Bottom: 10cm</br>\r\n<br> </br>";
		biMarginsNarrow.Click += biMarginsNarrow_Click;
		biMarginsWide.CanCustomize = false;
		biMarginsWide.Image = (Image)componentResourceManager.GetObject("biMarginsWide.Image");
		biMarginsWide.KeyTips = "W";
		biMarginsWide.Name = "biMarginsWide";
		biMarginsWide.Text = "<br><b>Wide</b></br>\r\n<br>Left: 10cm,  Right: 10cm</br>\r\n<br>Top: 10cm,  Bottom: 10cm</br>\r\n<br> </br>";
		biMarginsWide.Click += biMarginsWide_Click;
		biOrientation.AutoExpandOnClick = true;
		biOrientation.ButtonStyle = eButtonStyle.ImageAndText;
		biOrientation.Image = (Image)componentResourceManager.GetObject("biOrientation.Image");
		biOrientation.ImagePosition = eImagePosition.Top;
		biOrientation.ImageSmall = (Image)componentResourceManager.GetObject("biOrientation.ImageSmall");
		biOrientation.KeyTips = "O";
		biOrientation.Name = "biOrientation";
		biOrientation.SubItems.AddRange(new BaseItem[2] { biOrientationPortrait, biOrientationLandscape });
		biOrientation.SubItemsExpandWidth = 14;
		biOrientation.Text = "Orientation";
		biOrientation.PopupOpen += biOrientation_PopupOpen;
		biOrientationPortrait.CanCustomize = false;
		biOrientationPortrait.Image = (Image)componentResourceManager.GetObject("biOrientationPortrait.Image");
		biOrientationPortrait.KeyTips = "P";
		biOrientationPortrait.Name = "biOrientationPortrait";
		biOrientationPortrait.Text = "Portrait";
		biOrientationPortrait.Click += biOrientationPortrait_Click;
		biOrientationLandscape.CanCustomize = false;
		biOrientationLandscape.Image = (Image)componentResourceManager.GetObject("biOrientationLandscape.Image");
		biOrientationLandscape.KeyTips = "L";
		biOrientationLandscape.Name = "biOrientationLandscape";
		biOrientationLandscape.Text = "Landscape";
		biOrientationLandscape.Click += biOrientationLandscape_Click;
		biPageSize.AutoExpandOnClick = true;
		biPageSize.ButtonStyle = eButtonStyle.ImageAndText;
		biPageSize.Image = (Image)componentResourceManager.GetObject("biPageSize.Image");
		biPageSize.ImagePosition = eImagePosition.Top;
		biPageSize.ImageSmall = (Image)componentResourceManager.GetObject("biPageSize.ImageSmall");
		biPageSize.KeyTips = "SZ";
		biPageSize.Name = "biPageSize";
		biPageSize.SubItems.AddRange(new BaseItem[1] { biPageSizeTemp });
		biPageSize.SubItemsExpandWidth = 14;
		biPageSize.Text = "Size";
		biPageSize.PopupOpen += biPageSize_PopupOpen;
		biPageSizeTemp.CanCustomize = false;
		biPageSizeTemp.Name = "biPageSizeTemp";
		biColumns.AutoExpandOnClick = true;
		biColumns.ButtonStyle = eButtonStyle.ImageAndText;
		biColumns.Image = (Image)componentResourceManager.GetObject("biColumns.Image");
		biColumns.ImagePosition = eImagePosition.Top;
		biColumns.ImageSmall = (Image)componentResourceManager.GetObject("biColumns.ImageSmall");
		biColumns.KeyTips = "J";
		biColumns.Name = "biColumns";
		biColumns.SubItems.AddRange(new BaseItem[3] { biColumnsOne, biColumnsTwo, biColumnsThree });
		biColumns.SubItemsExpandWidth = 14;
		biColumns.Text = "Columns";
		biColumnsOne.CanCustomize = false;
		biColumnsOne.Image = (Image)componentResourceManager.GetObject("biColumnsOne.Image");
		biColumnsOne.KeyTips = "1";
		biColumnsOne.Name = "biColumnsOne";
		biColumnsOne.Text = "One";
		biColumnsOne.Click += biColumnsOne_Click;
		biColumnsTwo.CanCustomize = false;
		biColumnsTwo.Image = (Image)componentResourceManager.GetObject("biColumnsTwo.Image");
		biColumnsTwo.KeyTips = "2";
		biColumnsTwo.Name = "biColumnsTwo";
		biColumnsTwo.Text = "Two";
		biColumnsTwo.Click += biColumnsTwo_Click;
		biColumnsThree.CanCustomize = false;
		biColumnsThree.Image = (Image)componentResourceManager.GetObject("biColumnsThree.Image");
		biColumnsThree.KeyTips = "3";
		biColumnsThree.Name = "biColumnsThree";
		biColumnsThree.Text = "Three";
		biColumnsThree.Click += biColumnsThree_Click;
		biWatermarkSettings.ButtonStyle = eButtonStyle.ImageAndText;
		biWatermarkSettings.CanCustomize = false;
		biWatermarkSettings.Image = (Image)componentResourceManager.GetObject("biWatermarkSettings.Image");
		biWatermarkSettings.ImagePosition = eImagePosition.Top;
		biWatermarkSettings.KeyTips = "B";
		biWatermarkSettings.Name = "biWatermarkSettings";
		biWatermarkSettings.Stretch = true;
		biWatermarkSettings.Text = "Watermarks";
		biWatermarkSettings.Click += biWatermarkSettings_Click;
		ribbonPanelLayout.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		((Control)ribbonPanelLayout).Controls.Add((Control)(object)ribbonBarArrange);
		((Control)ribbonPanelLayout).Dock = (DockStyle)5;
		ribbonPanelLayout.Location = new Point(0, 57);
		((Control)ribbonPanelLayout).Name = "ribbonPanelLayout";
		((Control)ribbonPanelLayout).Padding = new Padding(3, 0, 3, 3);
		ribbonPanelLayout.Size = new Size(1125, 90);
		ribbonPanelLayout.Style.CornerType = eCornerType.Square;
		ribbonPanelLayout.StyleMouseDown.CornerType = eCornerType.Square;
		ribbonPanelLayout.StyleMouseOver.CornerType = eCornerType.Square;
		((Control)ribbonPanelLayout).TabIndex = 4;
		ribbonPanelLayout.Visible = false;
		ribbonBarArrange.AutoOverflowEnabled = true;
		ribbonBarArrange.BackgroundMouseOverStyle.CornerType = eCornerType.Square;
		ribbonBarArrange.BackgroundStyle.CornerType = eCornerType.Square;
		ribbonBarArrange.ContainerControlProcessDialogKey = true;
		((Control)ribbonBarArrange).Dock = (DockStyle)3;
		ribbonBarArrange.Items.AddRange(new BaseItem[8] { biAlignToGrid, biAlign, biBringToFront, biSendToBack, biMoveForward, biMoveBackward, biSize, itemContainer29 });
		((Control)ribbonBarArrange).Location = new Point(3, 0);
		((Control)ribbonBarArrange).Name = "ribbonBarArrange";
		ribbonBarArrange.OverflowButtonImage = (Image)(object)Resources.layout_arrange;
		((Control)ribbonBarArrange).Size = new Size(446, 87);
		ribbonBarArrange.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)ribbonBarArrange).TabIndex = 0;
		((Control)ribbonBarArrange).Text = "Arrange";
		ribbonBarArrange.TitleStyle.CornerType = eCornerType.Square;
		ribbonBarArrange.TitleStyleMouseOver.CornerType = eCornerType.Square;
		biAlignToGrid.ButtonStyle = eButtonStyle.ImageAndText;
		biAlignToGrid.Image = (Image)componentResourceManager.GetObject("biAlignToGrid.Image");
		biAlignToGrid.ImagePosition = eImagePosition.Top;
		biAlignToGrid.ImageSmall = (Image)componentResourceManager.GetObject("biAlignToGrid.ImageSmall");
		biAlignToGrid.KeyTips = "G";
		biAlignToGrid.Name = "biAlignToGrid";
		biAlignToGrid.Text = "Align to \r\nGrid";
		biAlignToGrid.Click += biAlignToGrid_Click;
		biAlign.AutoExpandOnClick = true;
		biAlign.BeginGroup = true;
		biAlign.ButtonStyle = eButtonStyle.ImageAndText;
		biAlign.Image = (Image)componentResourceManager.GetObject("biAlign.Image");
		biAlign.ImagePosition = eImagePosition.Top;
		biAlign.ImageSmall = (Image)componentResourceManager.GetObject("biAlign.ImageSmall");
		biAlign.KeyTips = "A";
		biAlign.Name = "biAlign";
		biAlign.SubItems.AddRange(new BaseItem[10] { biAlignLeft, biAlignCenter, biAlignRight, biAlignTop, biAlignMiddle, biAlignBottom, biMakeHorizontalSpacingEqual, biMakeVerticalSpacingEqual, biCenterHorizontally, biCenterVertically });
		biAlign.Text = "Align";
		biAlignLeft.BeginGroup = true;
		biAlignLeft.Image = (Image)componentResourceManager.GetObject("biAlignLeft.Image");
		biAlignLeft.KeyTips = "L";
		biAlignLeft.Name = "biAlignLeft";
		biAlignLeft.Text = "Align Left";
		biAlignLeft.Click += biAlignLeft_Click;
		biAlignCenter.Image = (Image)componentResourceManager.GetObject("biAlignCenter.Image");
		biAlignCenter.KeyTips = "C";
		biAlignCenter.Name = "biAlignCenter";
		biAlignCenter.Text = "Align Center";
		biAlignCenter.Click += biAlignCenter_Click;
		biAlignRight.Image = (Image)componentResourceManager.GetObject("biAlignRight.Image");
		biAlignRight.KeyTips = "R";
		biAlignRight.Name = "biAlignRight";
		biAlignRight.Text = "Align Right";
		biAlignRight.Click += biAlignRight_Click;
		biAlignTop.BeginGroup = true;
		biAlignTop.Image = (Image)componentResourceManager.GetObject("biAlignTop.Image");
		biAlignTop.KeyTips = "T";
		biAlignTop.Name = "biAlignTop";
		biAlignTop.Text = "Align Top";
		biAlignTop.Click += biAlignTop_Click;
		biAlignMiddle.Image = (Image)componentResourceManager.GetObject("biAlignMiddle.Image");
		biAlignMiddle.KeyTips = "M";
		biAlignMiddle.Name = "biAlignMiddle";
		biAlignMiddle.Text = "Align Middle";
		biAlignMiddle.Click += biAlignMiddle_Click;
		biAlignBottom.Image = (Image)componentResourceManager.GetObject("biAlignBottom.Image");
		biAlignBottom.KeyTips = "B";
		biAlignBottom.Name = "biAlignBottom";
		biAlignBottom.Text = "Align Bottom";
		biAlignBottom.Click += biAlignBottom_Click;
		biMakeHorizontalSpacingEqual.BeginGroup = true;
		biMakeHorizontalSpacingEqual.Image = (Image)componentResourceManager.GetObject("biMakeHorizontalSpacingEqual.Image");
		biMakeHorizontalSpacingEqual.KeyTips = "H";
		biMakeHorizontalSpacingEqual.Name = "biMakeHorizontalSpacingEqual";
		biMakeHorizontalSpacingEqual.Text = "Make Horizontal Spacing Equal";
		biMakeHorizontalSpacingEqual.Click += biMakeHorizontalSpacingEqual_Click;
		biMakeVerticalSpacingEqual.Image = (Image)componentResourceManager.GetObject("biMakeVerticalSpacingEqual.Image");
		biMakeVerticalSpacingEqual.KeyTips = "V";
		biMakeVerticalSpacingEqual.Name = "biMakeVerticalSpacingEqual";
		biMakeVerticalSpacingEqual.Text = "Make Vertical Spacing Equal";
		biMakeVerticalSpacingEqual.Click += biMakeVerticalSpacingEqual_Click;
		biCenterHorizontally.BeginGroup = true;
		biCenterHorizontally.Image = (Image)componentResourceManager.GetObject("biCenterHorizontally.Image");
		biCenterHorizontally.KeyTips = "CH";
		biCenterHorizontally.Name = "biCenterHorizontally";
		biCenterHorizontally.Text = "Center Horizontally";
		biCenterHorizontally.Click += biCenterHorizontally_Click;
		biCenterVertically.Image = (Image)componentResourceManager.GetObject("biCenterVertically.Image");
		biCenterVertically.KeyTips = "CV";
		biCenterVertically.Name = "biCenterVertically";
		biCenterVertically.Text = "Center Vertically";
		biCenterVertically.Click += biCenterVertically_Click;
		biBringToFront.BeginGroup = true;
		biBringToFront.ButtonStyle = eButtonStyle.ImageAndText;
		biBringToFront.Image = (Image)componentResourceManager.GetObject("biBringToFront.Image");
		biBringToFront.ImagePosition = eImagePosition.Top;
		biBringToFront.ImageSmall = (Image)componentResourceManager.GetObject("biBringToFront.ImageSmall");
		biBringToFront.KeyTips = "BF";
		biBringToFront.Name = "biBringToFront";
		biBringToFront.Text = "Bring to Front";
		biBringToFront.Click += biBringToFront_Click;
		biSendToBack.ButtonStyle = eButtonStyle.ImageAndText;
		biSendToBack.Image = (Image)componentResourceManager.GetObject("biSendToBack.Image");
		biSendToBack.ImagePosition = eImagePosition.Top;
		biSendToBack.ImageSmall = (Image)componentResourceManager.GetObject("biSendToBack.ImageSmall");
		biSendToBack.KeyTips = "SB";
		biSendToBack.Name = "biSendToBack";
		biSendToBack.Text = "Send to Back";
		biSendToBack.Click += biSendToBack_Click;
		biMoveForward.ButtonStyle = eButtonStyle.ImageAndText;
		biMoveForward.Image = (Image)componentResourceManager.GetObject("biMoveForward.Image");
		biMoveForward.ImagePosition = eImagePosition.Top;
		biMoveForward.ImageSmall = (Image)componentResourceManager.GetObject("biMoveForward.ImageSmall");
		biMoveForward.KeyTips = "MF";
		biMoveForward.Name = "biMoveForward";
		biMoveForward.Text = "Move Forward";
		biMoveForward.Click += biMoveForward_Click;
		biMoveBackward.ButtonStyle = eButtonStyle.ImageAndText;
		biMoveBackward.Image = (Image)componentResourceManager.GetObject("biMoveBackward.Image");
		biMoveBackward.ImagePosition = eImagePosition.Top;
		biMoveBackward.ImageSmall = (Image)componentResourceManager.GetObject("biMoveBackward.ImageSmall");
		biMoveBackward.KeyTips = "MB";
		biMoveBackward.Name = "biMoveBackward";
		biMoveBackward.Text = "Move Backward";
		biMoveBackward.Click += biMoveBackward_Click;
		biSize.AutoExpandOnClick = true;
		biSize.BeginGroup = true;
		biSize.ButtonStyle = eButtonStyle.ImageAndText;
		biSize.Image = (Image)componentResourceManager.GetObject("biSize.Image");
		biSize.ImagePosition = eImagePosition.Top;
		biSize.ImageSmall = (Image)componentResourceManager.GetObject("biSize.ImageSmall");
		biSize.KeyTips = "S";
		biSize.Name = "biSize";
		biSize.SubItems.AddRange(new BaseItem[3] { biMakeSameSize, biMakeSameWidth, biMakeSameHeight });
		biSize.Text = "Size";
		biMakeSameSize.ButtonStyle = eButtonStyle.ImageAndText;
		biMakeSameSize.Image = (Image)componentResourceManager.GetObject("biMakeSameSize.Image");
		biMakeSameSize.KeyTips = "S";
		biMakeSameSize.Name = "biMakeSameSize";
		biMakeSameSize.Text = "Make Same Size";
		biMakeSameSize.Click += biMakeSameSize_Click;
		biMakeSameWidth.ButtonStyle = eButtonStyle.ImageAndText;
		biMakeSameWidth.Image = (Image)componentResourceManager.GetObject("biMakeSameWidth.Image");
		biMakeSameWidth.KeyTips = "W";
		biMakeSameWidth.Name = "biMakeSameWidth";
		biMakeSameWidth.Text = "Make Same Width";
		biMakeSameWidth.Click += biMakeSameWidth_Click;
		biMakeSameHeight.ButtonStyle = eButtonStyle.ImageAndText;
		biMakeSameHeight.Image = (Image)componentResourceManager.GetObject("biMakeSameHeight.Image");
		biMakeSameHeight.KeyTips = "H";
		biMakeSameHeight.Name = "biMakeSameHeight";
		biMakeSameHeight.Text = "Make Same Height";
		biMakeSameHeight.Click += biMakeSameHeight_Click;
		itemContainer29.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer29.LayoutOrientation = eOrientation.Vertical;
		itemContainer29.Name = "itemContainer29";
		itemContainer29.SubItems.AddRange(new BaseItem[2] { biLock, biLink });
		itemContainer29.TitleStyle.CornerType = eCornerType.Square;
		biLock.AutoCheckOnClick = true;
		biLock.ButtonStyle = eButtonStyle.ImageAndText;
		biLock.Image = (Image)componentResourceManager.GetObject("biLock.Image");
		biLock.KeyTips = "L";
		biLock.Name = "biLock";
		biLock.Text = "Lock";
		biLock.Click += biLock_Click;
		biLink.AutoCheckOnClick = true;
		biLink.ButtonStyle = eButtonStyle.ImageAndText;
		biLink.Image = (Image)componentResourceManager.GetObject("biLink.Image");
		biLink.KeyTips = "K";
		biLink.Name = "biLink";
		biLink.Text = "Link";
		biLink.Click += biLink_Click;
		ribbonTabHome.Checked = true;
		ribbonTabHome.KeyTips = "H";
		ribbonTabHome.Name = "ribbonTabHome";
		ribbonTabHome.Panel = ribbonPanelHome;
		ribbonTabHome.Text = "Home";
		ribbonTabInsert.KeyTips = "I";
		ribbonTabInsert.Name = "ribbonTabInsert";
		ribbonTabInsert.Panel = ribbonPanelInsert;
		ribbonTabInsert.Text = "Insert";
		ribbonTabPage.KeyTips = "P";
		ribbonTabPage.Name = "ribbonTabPage";
		ribbonTabPage.Panel = ribbonPanelPage;
		ribbonTabPage.Text = "Page";
		ribbonTabLayout.KeyTips = "L";
		ribbonTabLayout.Name = "ribbonTabLayout";
		ribbonTabLayout.Panel = ribbonPanelLayout;
		ribbonTabLayout.Text = "Layout";
		biPublish.ItemAlignment = Stimulsoft.Controls.Win.DotNetBar.eItemAlignment.Far;
		biPublish.Name = "biPublish";
		biPublish.Text = "Publish";
		biPublish.Visible = false;
		biPublish.Click += biPublish_Click;
		biShare.ItemAlignment = Stimulsoft.Controls.Win.DotNetBar.eItemAlignment.Far;
		biShare.Name = "biShare";
		biShare.Text = "Share";
		biShare.Visible = false;
		biShare.Click += biShare_Click;
		biNotifications.Image = (Image)componentResourceManager.GetObject("biNotifications.Image");
		biNotifications.ItemAlignment = Stimulsoft.Controls.Win.DotNetBar.eItemAlignment.Far;
		biNotifications.Name = "biNotifications";
		biNotifications.Text = "Notifications";
		biNotifications.Visible = false;
		biNotifications.Click += biNotifications_Click;
		biSelectLanguage.AutoExpandOnClick = true;
		biSelectLanguage.ButtonStyle = eButtonStyle.ImageAndText;
		biSelectLanguage.ImagePosition = eImagePosition.Right;
		biSelectLanguage.ItemAlignment = Stimulsoft.Controls.Win.DotNetBar.eItemAlignment.Far;
		biSelectLanguage.Name = "biSelectLanguage";
		biSelectLanguage.Text = "EN";
		biSelectLanguage.Click += biSelectLanguage_Click;
		biLoginMenu.AutoExpandOnClick = true;
		biLoginMenu.ButtonStyle = eButtonStyle.ImageAndText;
		biLoginMenu.ImagePosition = eImagePosition.Right;
		biLoginMenu.ItemAlignment = Stimulsoft.Controls.Win.DotNetBar.eItemAlignment.Far;
		biLoginMenu.Name = "biLoginMenu";
		biLoginMenu.Text = "Login Menu";
		biLoginMenu.Visible = false;
		biLoginMenu.Click += biLoginMenu_Click;
		buttonItemEmpty.Name = "buttonItemEmpty";
		buttonItemEmpty.Text = "buttonItemEmpty";
		biLogin.ItemAlignment = Stimulsoft.Controls.Win.DotNetBar.eItemAlignment.Far;
		biLogin.Name = "biLogin";
		biLogin.Text = "Login";
		biLogin.Visible = false;
		biLogin.Click += biLogin_Click;
		OfficeStartButton.AutoExpandOnClick = true;
		OfficeStartButton.CanCustomize = false;
		OfficeStartButton.HotTrackingStyle = eHotTrackingStyle.Image;
		OfficeStartButton.Image = (Image)componentResourceManager.GetObject("OfficeStartButton.Image");
		OfficeStartButton.ImagePaddingHorizontal = 2;
		OfficeStartButton.ImagePaddingVertical = 2;
		OfficeStartButton.KeyTips = "F";
		OfficeStartButton.Name = "OfficeStartButton";
		OfficeStartButton.Office2007StartButton = true;
		OfficeStartButton.ShowSubItems = false;
		OfficeStartButton.SubItems.AddRange(new BaseItem[1] { itemContainerMainMenu });
		OfficeStartButton.Text = "&File";
		OfficeStartButton.PopupOpen += OfficeStartButton_PopupOpen;
		itemContainerMainMenu.BackgroundStyle.Class = "RibbonFileMenuContainer";
		itemContainerMainMenu.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainerMainMenu.LayoutOrientation = eOrientation.Vertical;
		itemContainerMainMenu.Name = "itemContainerMainMenu";
		itemContainerMainMenu.SubItems.AddRange(new BaseItem[2] { itemContainerMainMenu2, itemContainer4 });
		itemContainerMainMenu.TitleStyle.CornerType = eCornerType.Square;
		itemContainerMainMenu2.BackgroundStyle.Class = "RibbonFileMenuTwoColumnContainer";
		itemContainerMainMenu2.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainerMainMenu2.ItemSpacing = 0;
		itemContainerMainMenu2.Name = "itemContainerMainMenu2";
		itemContainerMainMenu2.SubItems.AddRange(new BaseItem[2] { itemContainerMainMenu3, galleryRecentFiles });
		itemContainerMainMenu2.TitleStyle.CornerType = eCornerType.Square;
		itemContainerMainMenu3.BackgroundStyle.Class = "RibbonFileMenuColumnOneContainer";
		itemContainerMainMenu3.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainerMainMenu3.LayoutOrientation = eOrientation.Vertical;
		itemContainerMainMenu3.MinimumSize = new Size(120, 0);
		itemContainerMainMenu3.Name = "itemContainerMainMenu3";
		itemContainerMainMenu3.SubItems.AddRange(new BaseItem[9] { biNew, biReportOpen, biReportSave, biSaveAs, biReportPreview, biCheckIssues, biReportSetup, biAbout, biClose });
		itemContainerMainMenu3.TitleStyle.CornerType = eCornerType.Square;
		biNew.AutoExpandOnClick = true;
		biNew.ButtonStyle = eButtonStyle.ImageAndText;
		biNew.Image = (Image)componentResourceManager.GetObject("biNew.Image");
		biNew.ImageSmall = (Image)componentResourceManager.GetObject("biNew.ImageSmall");
		biNew.KeyTips = "N";
		biNew.Name = "biNew";
		biNew.SubItems.AddRange(new BaseItem[3] { biBlankReport, biBlankDashboard, biBlankScreen });
		biNew.Text = "New";
		biBlankReport.ButtonStyle = eButtonStyle.ImageAndText;
		biBlankReport.Image = (Image)componentResourceManager.GetObject("biBlankReport.Image");
		biBlankReport.ImageSmall = (Image)componentResourceManager.GetObject("biBlankReport.ImageSmall");
		biBlankReport.KeyTips = "B";
		biBlankReport.Name = "biBlankReport";
		biBlankReport.SubItemsExpandWidth = 24;
		biBlankReport.Text = "&Blank Report";
		biBlankReport.Click += biReportNew_Click;
		biBlankDashboard.ButtonStyle = eButtonStyle.ImageAndText;
		biBlankDashboard.Image = (Image)componentResourceManager.GetObject("biBlankDashboard.Image");
		biBlankDashboard.ImageSmall = (Image)componentResourceManager.GetObject("biBlankDashboard.ImageSmall");
		biBlankDashboard.Name = "biBlankDashboard";
		biBlankDashboard.SubItemsExpandWidth = 24;
		biBlankDashboard.Text = "&Blank Dashboard";
		biBlankDashboard.Click += biDashboardNew_Click;
		biBlankScreen.ButtonStyle = eButtonStyle.ImageAndText;
		biBlankScreen.Image = (Image)componentResourceManager.GetObject("biBlankDashboard.Image");
		biBlankScreen.ImageSmall = (Image)componentResourceManager.GetObject("biBlankDashboard.ImageSmall");
		biBlankScreen.Name = "biBlankScreen";
		biBlankScreen.SubItemsExpandWidth = 24;
		biBlankScreen.Text = "&Blank Screen";
		biBlankScreen.Click += biScreenNew_Click;
		biReportOpen.BeginGroup = true;
		biReportOpen.ButtonStyle = eButtonStyle.ImageAndText;
		biReportOpen.Image = (Image)componentResourceManager.GetObject("biReportOpen.Image");
		biReportOpen.ImageSmall = (Image)componentResourceManager.GetObject("biReportOpen.ImageSmall");
		biReportOpen.KeyTips = "O";
		biReportOpen.Name = "biReportOpen";
		biReportOpen.Shortcuts.Add(eShortcut.CtrlO);
		biReportOpen.SubItemsExpandWidth = 24;
		biReportOpen.Text = "&Open Report...";
		biReportOpen.Click += biReportOpen_Click;
		biReportSave.BeginGroup = true;
		biReportSave.ButtonStyle = eButtonStyle.ImageAndText;
		biReportSave.Image = (Image)(object)Resources.BigReportSave;
		biReportSave.ImageSmall = (Image)componentResourceManager.GetObject("biReportSave.ImageSmall");
		biReportSave.KeyTips = "S";
		biReportSave.Name = "biReportSave";
		biReportSave.Shortcuts.Add(eShortcut.CtrlS);
		biReportSave.SubItemsExpandWidth = 24;
		biReportSave.Text = "&Save...";
		biReportSave.Click += biReportSave_Click;
		biSaveAs.ButtonStyle = eButtonStyle.ImageAndText;
		biSaveAs.Image = (Image)componentResourceManager.GetObject("biSaveAs.Image");
		biSaveAs.ImageSmall = (Image)componentResourceManager.GetObject("biSaveAs.ImageSmall");
		biSaveAs.KeyTips = "A";
		biSaveAs.Name = "biSaveAs";
		biSaveAs.Text = "Save As";
		biSaveAs.Click += biReportSaveAs_Click;
		biReportPreview.ButtonStyle = eButtonStyle.ImageAndText;
		biReportPreview.Image = (Image)componentResourceManager.GetObject("biReportPreview.Image");
		biReportPreview.KeyTips = "P";
		biReportPreview.Name = "biReportPreview";
		biReportPreview.Shortcuts.Add(eShortcut.F5);
		biReportPreview.Shortcuts.Add(eShortcut.ShiftF5);
		biReportPreview.Text = "Preview";
		biReportPreview.Click += biReportPreview_Click;
		biCheckIssues.BeginGroup = true;
		biCheckIssues.ButtonStyle = eButtonStyle.ImageAndText;
		biCheckIssues.Image = (Image)componentResourceManager.GetObject("biCheckIssues.Image");
		biCheckIssues.KeyTips = "C";
		biCheckIssues.Name = "biCheckIssues";
		biCheckIssues.Text = "Check for Issues";
		biCheckIssues.Click += biCheckIssues_Click;
		biReportSetup.ButtonStyle = eButtonStyle.ImageAndText;
		biReportSetup.Image = (Image)componentResourceManager.GetObject("biReportSetup.Image");
		biReportSetup.ImageSmall = (Image)componentResourceManager.GetObject("biReportSetup.ImageSmall");
		biReportSetup.KeyTips = "RS";
		biReportSetup.Name = "biReportSetup";
		biReportSetup.Text = "Report Setup";
		biReportSetup.Click += biReportSetup_Click;
		biAbout.BeginGroup = true;
		biAbout.ButtonStyle = eButtonStyle.ImageAndText;
		biAbout.Name = "biAbout";
		biAbout.SubItemsExpandWidth = 24;
		biAbout.Text = "About";
		biAbout.Click += biAbout_Click;
		biClose.ButtonStyle = eButtonStyle.ImageAndText;
		biClose.Image = (Image)componentResourceManager.GetObject("biClose.Image");
		biClose.ImageSmall = (Image)componentResourceManager.GetObject("biClose.ImageSmall");
		biClose.KeyTips = "C";
		biClose.Name = "biClose";
		biClose.SubItemsExpandWidth = 24;
		biClose.Text = "&Close";
		biClose.Click += biClose_Click;
		galleryRecentFiles.BackgroundStyle.BackColor = Color.Transparent;
		galleryRecentFiles.BackgroundStyle.Class = "RibbonFileMenuColumnTwoContainer";
		galleryRecentFiles.BackgroundStyle.CornerType = eCornerType.Square;
		galleryRecentFiles.DefaultSize = new Size(200, 58);
		galleryRecentFiles.EnableGalleryPopup = false;
		galleryRecentFiles.LayoutOrientation = eOrientation.Vertical;
		galleryRecentFiles.MinimumSize = new Size(180, 240);
		galleryRecentFiles.MultiLine = false;
		galleryRecentFiles.Name = "galleryRecentFiles";
		galleryRecentFiles.PopupUsesStandardScrollbars = false;
		galleryRecentFiles.SubItems.AddRange(new BaseItem[10] { liRecentDocuments, biRecentFile01, biRecentFile02, biRecentFile03, biRecentFile04, biRecentFile05, biRecentFile06, biRecentFile07, biRecentFile08, biRecentFile09 });
		galleryRecentFiles.TitleStyle.CornerType = eCornerType.Square;
		liRecentDocuments.BorderSide = eBorderSide.Bottom;
		liRecentDocuments.BorderType = eBorderType.Etched;
		liRecentDocuments.CanCustomize = false;
		liRecentDocuments.Name = "liRecentDocuments";
		liRecentDocuments.PaddingBottom = 2;
		liRecentDocuments.PaddingTop = 2;
		liRecentDocuments.Stretch = true;
		liRecentDocuments.Text = "Recent Documents";
		biRecentFile01.FixedSize = new Size(200, 0);
		biRecentFile01.Name = "biRecentFile01";
		biRecentFile01.Text = "&1. File";
		biRecentFile01.Click += biRecentFile_Click;
		biRecentFile02.Name = "biRecentFile02";
		biRecentFile02.Text = "&2. File";
		biRecentFile02.Click += biRecentFile_Click;
		biRecentFile03.Name = "biRecentFile03";
		biRecentFile03.Text = "&3. File";
		biRecentFile03.Click += biRecentFile_Click;
		biRecentFile04.Name = "biRecentFile04";
		biRecentFile04.Text = "&4. File";
		biRecentFile04.Click += biRecentFile_Click;
		biRecentFile05.Name = "biRecentFile05";
		biRecentFile05.Text = "&5. File";
		biRecentFile05.Click += biRecentFile_Click;
		biRecentFile06.Name = "biRecentFile06";
		biRecentFile06.Text = "&6. File";
		biRecentFile06.Click += biRecentFile_Click;
		biRecentFile07.Name = "biRecentFile07";
		biRecentFile07.Text = "&7. File";
		biRecentFile07.Click += biRecentFile_Click;
		biRecentFile08.Name = "biRecentFile08";
		biRecentFile08.Text = "&8. File";
		biRecentFile08.Click += biRecentFile_Click;
		biRecentFile09.Name = "biRecentFile09";
		biRecentFile09.Text = "&9. File";
		biRecentFile09.Click += biRecentFile_Click;
		itemContainer4.BackgroundStyle.Class = "RibbonFileMenuBottomContainer";
		itemContainer4.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer4.HorizontalItemAlignment = eHorizontalItemsAlignment.Right;
		itemContainer4.ItemSpacing = 4;
		itemContainer4.Name = "itemContainer4";
		itemContainer4.SubItems.AddRange(new BaseItem[2] { biOptions, biExit });
		itemContainer4.TitleStyle.CornerType = eCornerType.Square;
		biOptions.ButtonStyle = eButtonStyle.ImageAndText;
		biOptions.ColorTable = eButtonColor.OrangeWithBackground;
		biOptions.Image = (Image)componentResourceManager.GetObject("biOptions.Image");
		biOptions.KeyTips = "I";
		biOptions.Name = "biOptions";
		biOptions.SubItemsExpandWidth = 24;
		biOptions.Text = "Opt&ions";
		biOptions.Click += biOptions_Click;
		biExit.ButtonStyle = eButtonStyle.ImageAndText;
		biExit.ColorTable = eButtonColor.OrangeWithBackground;
		biExit.Image = (Image)componentResourceManager.GetObject("biExit.Image");
		biExit.KeyTips = "X";
		biExit.Name = "biExit";
		biExit.SubItemsExpandWidth = 24;
		biExit.Text = "E&xit";
		biExit.Click += biExit_Click;
		biQuickReportSave.Image = (Image)componentResourceManager.GetObject("biQuickReportSave.Image");
		biQuickReportSave.Name = "biQuickReportSave";
		biQuickReportSave.Text = "Report Save";
		biQuickReportSave.Click += biReportSave_Click;
		biQuickUndo.Image = (Image)componentResourceManager.GetObject("biQuickUndo.Image");
		biQuickUndo.KeyTips = "U";
		biQuickUndo.Name = "biQuickUndo";
		biQuickUndo.Text = "Undo";
		biQuickUndo.Click += biQuickUndo_Click;
		biQuickRedo.Image = (Image)componentResourceManager.GetObject("biQuickRedo.Image");
		biQuickRedo.KeyTips = "R";
		biQuickRedo.Name = "biQuickRedo";
		biQuickRedo.Text = "Redo";
		biQuickRedo.Click += biQuickRedo_Click;
		qatCustomizeItem1.Name = "qatCustomizeItem1";
		qatCustomizeItem1.Tooltip = "qat_customizetooltip";
		applicationButton1.AutoExpandOnClick = true;
		applicationButton1.CanCustomize = false;
		applicationButton1.HotTrackingStyle = eHotTrackingStyle.Image;
		applicationButton1.ImagePaddingHorizontal = 2;
		applicationButton1.ImagePaddingVertical = 2;
		applicationButton1.Name = "applicationButton1";
		applicationButton1.Office2007StartButton = true;
		applicationButton1.ShowSubItems = false;
		applicationButton1.SubItems.AddRange(new BaseItem[1] { itemContainer1 });
		applicationButton1.Text = "&File";
		itemContainer1.BackgroundStyle.Class = "RibbonFileMenuContainer";
		itemContainer1.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer1.LayoutOrientation = eOrientation.Vertical;
		itemContainer1.Name = "itemContainer1";
		itemContainer1.SubItems.AddRange(new BaseItem[1] { itemContainer2 });
		itemContainer1.TitleStyle.CornerType = eCornerType.Square;
		itemContainer2.BackgroundStyle.Class = "RibbonFileMenuTwoColumnContainer";
		itemContainer2.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer2.ItemSpacing = 0;
		itemContainer2.Name = "itemContainer2";
		itemContainer2.SubItems.AddRange(new BaseItem[2] { itemContainer3, galleryContainer1 });
		itemContainer2.TitleStyle.CornerType = eCornerType.Square;
		itemContainer3.BackgroundStyle.Class = "RibbonFileMenuColumnOneContainer";
		itemContainer3.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer3.LayoutOrientation = eOrientation.Vertical;
		itemContainer3.MinimumSize = new Size(120, 0);
		itemContainer3.Name = "itemContainer3";
		itemContainer3.SubItems.AddRange(new BaseItem[6] { buttonItem2, buttonItem3, buttonItem4, buttonItem5, buttonItem6, buttonItem7 });
		itemContainer3.TitleStyle.CornerType = eCornerType.Square;
		buttonItem2.ButtonStyle = eButtonStyle.ImageAndText;
		buttonItem2.Image = (Image)componentResourceManager.GetObject("buttonItem2.Image");
		buttonItem2.Name = "buttonItem2";
		buttonItem2.SubItemsExpandWidth = 24;
		buttonItem2.Text = "&New";
		buttonItem3.ButtonStyle = eButtonStyle.ImageAndText;
		buttonItem3.Image = (Image)componentResourceManager.GetObject("buttonItem3.Image");
		buttonItem3.Name = "buttonItem3";
		buttonItem3.SubItemsExpandWidth = 24;
		buttonItem3.Text = "&Open...";
		buttonItem4.ButtonStyle = eButtonStyle.ImageAndText;
		buttonItem4.Image = (Image)componentResourceManager.GetObject("buttonItem4.Image");
		buttonItem4.Name = "buttonItem4";
		buttonItem4.SubItemsExpandWidth = 24;
		buttonItem4.Text = "&Save...";
		buttonItem5.BeginGroup = true;
		buttonItem5.ButtonStyle = eButtonStyle.ImageAndText;
		buttonItem5.Image = (Image)componentResourceManager.GetObject("buttonItem5.Image");
		buttonItem5.Name = "buttonItem5";
		buttonItem5.SubItemsExpandWidth = 24;
		buttonItem5.Text = "S&hare...";
		buttonItem6.ButtonStyle = eButtonStyle.ImageAndText;
		buttonItem6.Image = (Image)componentResourceManager.GetObject("buttonItem6.Image");
		buttonItem6.Name = "buttonItem6";
		buttonItem6.SubItemsExpandWidth = 24;
		buttonItem6.Text = "&Print...";
		buttonItem7.BeginGroup = true;
		buttonItem7.ButtonStyle = eButtonStyle.ImageAndText;
		buttonItem7.Image = (Image)componentResourceManager.GetObject("buttonItem7.Image");
		buttonItem7.Name = "buttonItem7";
		buttonItem7.SubItemsExpandWidth = 24;
		buttonItem7.Text = "&Close";
		galleryContainer1.BackgroundStyle.Class = "RibbonFileMenuColumnTwoContainer";
		galleryContainer1.BackgroundStyle.CornerType = eCornerType.Square;
		galleryContainer1.EnableGalleryPopup = false;
		galleryContainer1.LayoutOrientation = eOrientation.Vertical;
		galleryContainer1.MinimumSize = new Size(180, 240);
		galleryContainer1.MultiLine = false;
		galleryContainer1.Name = "galleryContainer1";
		galleryContainer1.PopupUsesStandardScrollbars = false;
		galleryContainer1.SubItems.AddRange(new BaseItem[5] { labelItem8, buttonItem8, buttonItem9, buttonItem10, buttonItem11 });
		galleryContainer1.TitleStyle.CornerType = eCornerType.Square;
		labelItem8.BorderSide = eBorderSide.Bottom;
		labelItem8.BorderType = eBorderType.Etched;
		labelItem8.CanCustomize = false;
		labelItem8.ForeColor = SystemColors.ControlText;
		labelItem8.Name = "labelItem8";
		labelItem8.PaddingBottom = 2;
		labelItem8.PaddingTop = 2;
		labelItem8.Stretch = true;
		labelItem8.Text = "Recent Documents";
		buttonItem8.Name = "buttonItem8";
		buttonItem8.Text = "&1. Short News 5-7.rtf";
		buttonItem9.Name = "buttonItem9";
		buttonItem9.Text = "&2. Prospect Email.rtf";
		buttonItem10.Name = "buttonItem10";
		buttonItem10.Text = "&3. Customer Email.rtf";
		buttonItem11.Name = "buttonItem11";
		buttonItem11.Text = "&4. example.rtf";
		buttonItem1.Name = "buttonItem1";
		buttonItem1.Text = "buttonItem1";
		buttonItem12.ButtonStyle = eButtonStyle.ImageAndText;
		buttonItem12.ColorTable = eButtonColor.OrangeWithBackground;
		buttonItem12.Image = (Image)componentResourceManager.GetObject("buttonItem12.Image");
		buttonItem12.Name = "buttonItem12";
		buttonItem12.SubItemsExpandWidth = 24;
		buttonItem12.Text = "Opt&ions";
		buttonItem13.ButtonStyle = eButtonStyle.ImageAndText;
		buttonItem13.ColorTable = eButtonColor.OrangeWithBackground;
		buttonItem13.Image = (Image)componentResourceManager.GetObject("buttonItem13.Image");
		buttonItem13.Name = "buttonItem13";
		buttonItem13.SubItemsExpandWidth = 24;
		buttonItem13.Text = "E&xit";
		itemContainer37.BackgroundStyle.CornerType = eCornerType.Square;
		itemContainer37.BeginGroup = true;
		itemContainer37.HorizontalItemAlignment = eHorizontalItemsAlignment.Center;
		itemContainer37.Name = "itemContainer37";
		itemContainer37.TitleStyle.CornerType = eCornerType.Square;
		ribbonBarView.AutoOverflowEnabled = true;
		ribbonBarView.BackgroundMouseOverStyle.CornerType = eCornerType.Square;
		ribbonBarView.BackgroundStyle.CornerType = eCornerType.Square;
		ribbonBarView.ContainerControlProcessDialogKey = true;
		((Control)ribbonBarView).Dock = (DockStyle)3;
		ribbonBarView.Items.AddRange(new BaseItem[4] { biSurfaceViewDesktop, biSurfaceViewMobile, biSurfaceViewRemoveMobileSurface, biSurfaceViewComponents });
		((Control)ribbonBarView).Location = new Point(310, 0);
		((Control)ribbonBarView).Name = "ribbonBarView";
		((Control)ribbonBarView).Size = new Size(112, 89);
		ribbonBarView.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)ribbonBarView).TabIndex = 1;
		((Control)ribbonBarView).Text = "View";
		ribbonBarView.TitleStyle.CornerType = eCornerType.Square;
		ribbonBarView.TitleStyleMouseOver.CornerType = eCornerType.Square;
		((Control)ribbonBarView).Visible = false;
		biSurfaceViewDesktop.Image = (Image)componentResourceManager.GetObject("biViewDesktop.Image");
		biSurfaceViewDesktop.ImagePosition = eImagePosition.Top;
		biSurfaceViewDesktop.Name = "biSurfaceViewDesktop";
		biSurfaceViewDesktop.Stretch = true;
		biSurfaceViewDesktop.SubItemsExpandWidth = 14;
		biSurfaceViewDesktop.Text = "Desktop";
		biSurfaceViewDesktop.Click += biSurfaceViewDesktop_Click;
		biSurfaceViewMobile.Image = (Image)componentResourceManager.GetObject("biViewMobile.Image");
		biSurfaceViewMobile.ImagePosition = eImagePosition.Top;
		biSurfaceViewMobile.Name = "biSurfaceViewMobile";
		biSurfaceViewMobile.Stretch = true;
		biSurfaceViewMobile.SubItemsExpandWidth = 14;
		biSurfaceViewMobile.Text = "Mobile";
		biSurfaceViewMobile.Click += biSurfaceViewMobile_Click;
		biSurfaceViewRemoveMobileSurface.Image = (Image)componentResourceManager.GetObject("biViewRemoveMobileSurface.Image");
		biSurfaceViewRemoveMobileSurface.ImagePosition = eImagePosition.Top;
		biSurfaceViewRemoveMobileSurface.Name = "biSurfaceViewRemoveMobileSurface";
		biSurfaceViewRemoveMobileSurface.BeginGroup = true;
		biSurfaceViewRemoveMobileSurface.Stretch = true;
		biSurfaceViewRemoveMobileSurface.SubItemsExpandWidth = 14;
		biSurfaceViewRemoveMobileSurface.Text = "Remove Mobile Surface";
		biSurfaceViewRemoveMobileSurface.Click += biSurfaceViewRemoveMobileSurface_Click;
		biSurfaceViewRemoveMobileSurface.Enabled = false;
		biSurfaceViewComponents.Image = (Image)componentResourceManager.GetObject("biSurfaceViewComponents.Image");
		biSurfaceViewComponents.ImagePosition = eImagePosition.Top;
		biSurfaceViewComponents.Name = "biSurfaceViewComponents";
		biSurfaceViewComponents.Stretch = true;
		biSurfaceViewComponents.SubItemsExpandWidth = 14;
		biSurfaceViewComponents.Text = "Components";
		biSurfaceViewComponents.Click += biSurfaceViewComponents_Click;
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
		((Control)this).BackColor = Color.FromArgb(191, 219, 255);
		((Control)this).Controls.Add((Control)(object)ribbonControl);
		((Control)this).Name = "StiRibbonDesignerUserControl";
		((Control)this).Size = new Size(1125, 173);
		ribbonControl.ResumeLayout(performLayout: false);
		((Control)ribbonControl).PerformLayout();
		((Control)ribbonPanelHome).ResumeLayout(false);
		((Control)ribbonPanelPage).ResumeLayout(false);
		((Control)ribbonPanelLayout).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}

	private void TimerTick(object sender, EventArgs e)
	{
		timer.Tick -= TimerTick;
		timer.Stop();
		((Component)(object)timer).Dispose();
		if (StiAccountSettings.CurrentAccount != null)
		{
			CheckStatePanel();
			StiAccountSettings.CurrentAccount.UserInfoChanged += CurrentAccount_UserInfoChanged;
		}
	}

	private void NotificationControl_NotificationCountChanged(object sender, EventArgs e)
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		int? num = (int?)sender;
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiRibbonDesignerUserControl));
		biNotifications.Image = ((num.GetValueOrDefault() > 0) ? ((Image)componentResourceManager.GetObject("biNotificationsOrange.Image" + StiScale.StepName)) : ((Image)componentResourceManager.GetObject("biNotificationsWhite.Image" + StiScale.StepName)));
	}

	private void CurrentAccount_ShowNewNotifications(object sender, EventArgs e)
	{
		try
		{
			if (e is StiAccountNotificationCollectionEventArgs stiAccountNotificationCollectionEventArgs)
			{
				IStiMessageWindow notificationCollectionWindow = StiAccountSettings.CurrentAccount.AccountCreater.GetNotificationCollectionWindow(stiAccountNotificationCollectionEventArgs);
				notificationCollectionWindow.ShowDialog(((Control)Designer).Handle);
				if (stiAccountNotificationCollectionEventArgs.Messages.Any((StiAccountNotificationEventArgs m) => m.Message.Ident == StiNoticeIdent.ActivationLockedAccount || m.Message.Ident == StiNoticeIdent.ActivationTrialExpired))
				{
					StiAccountSettings.CurrentAccount?.Logout();
				}
			}
		}
		catch
		{
		}
	}

	private void CurrentAccount_ShowNewMessage(object sender, EventArgs e)
	{
		try
		{
			if (e is StiAccountMessageEventArgs accountMessage)
			{
				IStiMessageWindow messageWindow = StiAccountSettings.CurrentAccount.AccountCreater.GetMessageWindow(accountMessage);
				messageWindow.ShowDialog(((Control)Designer).Handle);
			}
		}
		catch
		{
		}
	}

	private void CurrentAccount_UserInfoChanged(object sender, EventArgs e)
	{
		CheckStatePanel();
		RibbonDesigner.CheckLicenseExpired();
	}

	private Bitmap GetImageAccount(IStiAccountUser user)
	{
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Expected O, but got Unknown
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Expected O, but got Unknown
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Expected O, but got Unknown
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Expected O, but got Unknown
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Expected O, but got Unknown
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Expected O, but got Unknown
		if (user.Picture != null)
		{
			using (MemoryStream memoryStream = new MemoryStream(user.Picture))
			{
				int num = StiScale.XXI(17.0);
				Color color = Color.FromArgb(43, 86, 154);
				Image imgToResize = Image.FromStream((Stream)memoryStream);
				Image srcImage = ResizeImage(imgToResize, new Size(num, num));
				Image val = ClipToCircle(srcImage, color);
				Bitmap val2 = new Bitmap(val, new Size(num, num));
				val2.MakeTransparent(color);
				return val2;
			}
		}
		Color color2 = Color.White;
		if (!string.IsNullOrEmpty(user.UserKey))
		{
			string userKey = user.UserKey;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			if (userKey != null)
			{
				for (int i = 0; i < userKey.Length / 3; i += 3)
				{
					num2 += (byte)userKey[i];
					num3 += (byte)userKey[i + 1];
					num4 += (byte)userKey[i + 2];
				}
			}
			int num5 = num2 % 100 + 100;
			int num6 = num3 % 100 + 100;
			int num7 = num4 % 100 + 100;
			color2 = Color.FromArgb(255, (byte)num5, (byte)num6, (byte)num7);
		}
		Bitmap val3 = new Bitmap(StiScale.XXI(17.0), StiScale.YYI(17.0));
		StringFormat val4 = new StringFormat();
		try
		{
			Font val5 = new Font("Arial", 6f);
			try
			{
				SolidBrush val6 = new SolidBrush(Color.White);
				try
				{
					Graphics val7 = Graphics.FromImage((Image)(object)val3);
					try
					{
						val4.LineAlignment = (StringAlignment)1;
						val4.Alignment = (StringAlignment)1;
						val7.SmoothingMode = (SmoothingMode)4;
						if (!string.IsNullOrEmpty(user.UserKey))
						{
							val7.FillEllipse((Brush)new SolidBrush(color2), new RectangleF(0f, 0f, StiScale.XXI(16.0), StiScale.YYI(16.0)));
						}
						else
						{
							val7.DrawEllipse(new Pen((Brush)new SolidBrush(color2), 1f), new RectangleF(0f, 0f, StiScale.XXI(16.0), StiScale.YYI(16.0)));
						}
						string arg = "";
						string arg2 = "";
						if (!string.IsNullOrEmpty(user.FirstName) && user.FirstName.Length > 0)
						{
							arg = user.FirstName.Substring(0, 1);
						}
						if (!string.IsNullOrEmpty(user.LastName) && user.LastName.Length > 0)
						{
							arg2 = user.LastName.Substring(0, 1);
						}
						string text = $"{arg}{arg2}";
						val7.DrawString(text, val5, (Brush)(object)val6, new RectangleF(0f, 0f, StiScale.XXI(17.0), StiScale.YYI(17.0)), val4);
						val7.Dispose();
						return val3;
					}
					finally
					{
						((IDisposable)val7)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val6)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val4)?.Dispose();
		}
	}

	public static Image ResizeImage(Image imgToResize, Size size)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		return (Image)new Bitmap(imgToResize, size);
	}

	private static Image CropImage(Image img, Rectangle cropArea)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		Bitmap val = new Bitmap(img);
		return (Image)(object)val.Clone(cropArea, ((Image)val).PixelFormat);
	}

	public Image ClipToCircle(Image srcImage, Color backGround)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		Bitmap val = new Bitmap(srcImage.Width, srcImage.Height, srcImage.PixelFormat);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		SolidBrush val3 = new SolidBrush(backGround);
		try
		{
			val2.FillRectangle((Brush)(object)val3, 0, 0, ((Image)val).Width, ((Image)val).Height);
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
		GraphicsPath val4 = new GraphicsPath();
		val4.AddEllipse(0, 0, ((Image)val).Width, ((Image)val).Height);
		val2.SetClip(val4);
		val2.DrawImage(srcImage, 0, 0);
		val2.Dispose();
		return (Image)(object)val;
	}

	private void RibbonControl_SizeChanged(object sender, EventArgs e)
	{
		((Control)this).Size = ((Control)ribbonControl).Size;
	}

	private void biReportNew_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokeReportNewBlank(Designer);
	}

	private void biDashboardNew_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokeDashboardNewBlank(Designer);
	}

	private void biScreenNew_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokeScreenNewBlank(Designer);
	}

	private void biReportOpen_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokeReportOpen(Designer);
	}

	private void biReportSave_Click(object sender, EventArgs e)
	{
		if (StiAccountSettings.CurrentAccount == null || (StiAccountSettings.CurrentAccount.AccountChecker.CheckUserActivated(((Control)Designer).Handle) && StiAccountSettings.CurrentAccount.AccountChecker.CheckTrialExpired() && StiAccountSettings.CurrentAccount.AccountChecker.CheckProductExpired()))
		{
			StiActions.MainMenu.InvokeReportSave(Designer);
		}
	}

	private void biReportSaveAs_Click(object sender, EventArgs e)
	{
		if (StiAccountSettings.CurrentAccount == null || (StiAccountSettings.CurrentAccount.AccountChecker.CheckUserActivated(((Control)Designer).Handle) && StiAccountSettings.CurrentAccount.AccountChecker.CheckTrialExpired() && StiAccountSettings.CurrentAccount.AccountChecker.CheckProductExpired()))
		{
			StiActions.MainMenu.InvokeReportSaveAs(Designer);
		}
	}

	private void biReportPreview_Click(object sender, EventArgs e)
	{
		if (e is EventSourceArgs && (e as EventSourceArgs).Key == eShortcut.F5)
		{
			if (RibbonTabPreview != null)
			{
				if (!RibbonTabPreview.Checked)
				{
					RibbonTabPreview.Checked = true;
				}
			}
			else if (((ArrangedElementCollection)((Control)Designer.TabControl).Controls).Count > Designer.PagePreviewIndex)
			{
				Designer.TabControl.SelectedTab = Designer.TabControl.Tabs[Designer.PagePreviewIndex];
			}
		}
		else
		{
			StiActions.MainMenu.InvokeReportPreview(designer);
		}
	}

	private void biReportSetup_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokeReportSetup(Designer);
	}

	private void biClose_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokeClose(Designer);
	}

	private void biOptions_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokeOptions(Designer);
	}

	private void biQuickUndo_Click(object sender, EventArgs e)
	{
		StiActions.Clipboard.InvokeUndo(Designer);
	}

	private void biQuickRedo_Click(object sender, EventArgs e)
	{
		StiActions.Clipboard.InvokeRedo(Designer);
	}

	private void biPaste_Click(object sender, EventArgs e)
	{
		StiActions.Clipboard.InvokePaste(Designer);
	}

	private void biCopy_Click(object sender, EventArgs e)
	{
		StiActions.Clipboard.InvokeCopy(Designer);
	}

	private void biCut_Click(object sender, EventArgs e)
	{
		StiActions.Clipboard.InvokeCut(Designer);
	}

	private void biDelete_Click(object sender, EventArgs e)
	{
		StiActions.Clipboard.InvokeDelete(Designer);
	}

	private void biLogin_Click(object sender, EventArgs e)
	{
		if (StiAccountSettings.CurrentAccount != null)
		{
			IStiAuthorizationWindow authorizationWindow = StiAccountSettings.CurrentAccount.AccountCreater.GetAuthorizationWindow();
			authorizationWindow.ShowDialog(((Control)Designer).Handle);
		}
	}

	private void biShare_Click(object sender, EventArgs e)
	{
		if (StiAccountSettings.CurrentAccount == null || (StiAccountSettings.CurrentAccount.AccountChecker.CheckUserActivated(((Control)Designer).Handle) && StiAccountSettings.CurrentAccount.AccountChecker.CheckTrialExpired() && StiAccountSettings.CurrentAccount.AccountChecker.CheckProductExpired()))
		{
			StiActions.MainMenu.InvokeShare(Designer);
		}
	}

	private void biNotifications_Click(object sender, EventArgs e)
	{
		Rectangle rectangle = ((biNotifications.Parent != null) ? biNotifications.Parent.ScreenRectangle : biNotifications.ScreenRectangle);
		Point point = new Point(rectangle.Right, rectangle.Top + rectangle.Height);
		((ToolStripDropDown)popup).Show(point, (ToolStripDropDownDirection)2);
	}

	private void biPublish_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokePublish(Designer);
	}

	private void biAbout_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokeAbout(Designer);
	}

	private void biHelp_Click(object sender, EventArgs e)
	{
		StiHelpProvider.ShowHelpViewer(SuperTooltip.CurrentSuperTooltipHyperlink);
	}

	private async void biRecentFile_Click(object sender, EventArgs e)
	{
		ButtonItem buttonItem = sender as ButtonItem;
		StiRecentFile recentFile = buttonItem.Tag as StiRecentFile;
		await StiActions.MainMenu.InvokeRecentFileAsync(Designer, recentFile);
	}

	private void OnLocalizationChanged(object sender, EventArgs e)
	{
		Localize();
		UpdateSize(ribbonBarAlignment);
		UpdateSize(ribbonBarArrange);
		UpdateSize(ribbonBarBorders);
		UpdateSize(ribbonBarClipboard);
		UpdateSize(ribbonBarFont);
		UpdateSize(ribbonBarPageSetup);
		UpdateSize(ribbonBarStyle);
		UpdateSize(ribbonBarTextFormat);
		UpdateSize(ribbonBarViewOptions);
		UpdateWizardsLocalization();
		ribbonControl.RecalcLayout();
		((Control)ribbonControl).Invalidate();
	}

	private void UpdateWizardsLocalization()
	{
		foreach (ButtonItem wizardsButton in wizardsButtons)
		{
			wizardsButton.Text = ((StiWizardService)wizardsButton.Tag).ServiceName;
		}
	}

	private void OfficeStartButton_PopupOpen(object sender, PopupOpenEventArgs e)
	{
		if (RibbonTabPreview != null && RibbonTabPreview.Checked)
		{
			ribbonTabHome.Checked = true;
		}
		biReportSave.Image = (Image)(object)Resources.BigReportSave;
		RefreshRecentFiles();
	}

	private void biLoginMenu_Click(object sender, EventArgs e)
	{
		Rectangle rectangle = ((biLoginMenu.Parent != null) ? biLoginMenu.Parent.ScreenRectangle : biLoginMenu.ScreenRectangle);
		Point point = new Point(rectangle.Right, rectangle.Top + rectangle.Height);
		((ToolStripDropDown)menuPopup).Show(point, (ToolStripDropDownDirection)2);
	}

	private void AccountMenu_GoToCloudClick(object sender, EventArgs e)
	{
		StiPopup stiPopup = menuPopup;
		if (stiPopup != null)
		{
			((Control)stiPopup).Hide();
		}
	}

	private void AccountMenu_LogoutClick(object sender, EventArgs e)
	{
		StiPopup stiPopup = menuPopup;
		if (stiPopup != null)
		{
			((Control)stiPopup).Hide();
		}
		if (!designer.TestReportSave())
		{
			designer?.UndoRedo?.Clear();
			StiCacheCleaner.Clean(designer.Report);
			if (designer.Report != null)
			{
				designer.Report.IsModified = false;
			}
			StiAccountSettings.CurrentAccount?.Logout();
		}
	}

	private void AccountMenu_CreateTeamClick(object sender, EventArgs e)
	{
		StiPopup stiPopup = menuPopup;
		if (stiPopup != null)
		{
			((Control)stiPopup).Hide();
		}
		ApplicationButton ribbonApplicationButton = designer.GetRibbonApplicationButton();
		if (ribbonApplicationButton != null)
		{
			ribbonApplicationButton.Expanded = true;
			StiRibbonBackStageUserControl stiRibbonBackStageUserControl = (StiRibbonBackStageUserControl)ribbonApplicationButton.Tag;
			stiRibbonBackStageUserControl.BackStage.SelectedTab = stiRibbonBackStageUserControl.buttonAccount;
			stiRibbonBackStageUserControl.SelectedMyTeam();
		}
	}

	private void AccountMenu_AccountClick(object sender, EventArgs e)
	{
		StiPopup stiPopup = menuPopup;
		if (stiPopup != null)
		{
			((Control)stiPopup).Hide();
		}
		ApplicationButton ribbonApplicationButton = designer.GetRibbonApplicationButton();
		if (ribbonApplicationButton != null)
		{
			ribbonApplicationButton.Expanded = true;
			StiRibbonBackStageUserControl stiRibbonBackStageUserControl = (StiRibbonBackStageUserControl)ribbonApplicationButton.Tag;
			stiRibbonBackStageUserControl.BackStage.SelectedTab = stiRibbonBackStageUserControl.buttonAccount;
			stiRibbonBackStageUserControl.ButtonAccount_Click(null, null);
		}
	}

	private void biSelectLanguage_Click(object sender, EventArgs e)
	{
		new StiRibbonLocalizationPopup(this, biSelectLanguage).Show();
	}

	private void biExit_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokeExit(Designer);
	}

	private void RibbonControl_AfterAddItemToQuickAccessToolbar(object sender, RibbonCustomizeEventArgs e)
	{
		if (Designer != null)
		{
			Designer.SaveDesignerParam();
		}
	}

	private void RibbonControl_AfterRemoveItemFromQuickAccessToolbar(object sender, RibbonCustomizeEventArgs e)
	{
		if (Designer != null)
		{
			Designer.SaveDesignerParam();
		}
	}

	private void OnRefreshDesigner(object sender, EventArgs e)
	{
		lockRefresh = true;
		RefreshQuickBar();
		RefreshMainMenu();
		RefreshReportFile();
		RefreshClipboard();
		RefreshFont();
		RefreshBorders();
		RefreshContentAlignment();
		RefreshTextFormat();
		RefreshStyles();
		RefreshLayout();
		RefreshPage();
		RefreshWatermarkText();
		RefreshViewOptions();
		RefresgAccountPanel();
		lockRefresh = false;
	}

	private void biAlignToGrid_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeAlignToGrid(Designer);
	}

	private void biAlignLeft_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeAlignLeft(Designer);
	}

	private void biAlignCenter_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeAlignCenter(Designer);
	}

	private void biAlignRight_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeAlignRight(Designer);
	}

	private void biAlignTop_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeAlignTop(Designer);
	}

	private void biAlignMiddle_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeAlignMiddle(Designer);
	}

	private void biAlignBottom_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeAlignBottom(Designer);
	}

	private void biMakeHorizontalSpacingEqual_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeMakeHorizontalSpacingEqual(Designer);
	}

	private void biMakeVerticalSpacingEqual_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeMakeVerticalSpacingEqual(Designer);
	}

	private void biCenterHorizontally_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeCenterHorizontally(Designer);
	}

	private void biCenterVertically_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeCenterVertically(Designer);
	}

	private void biBringToFront_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeBringToFront(Designer);
	}

	private void biSendToBack_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeSendToBack(Designer);
	}

	private void biMoveForward_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeMoveForward(Designer);
	}

	private void biMoveBackward_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeMoveBackward(Designer);
	}

	private void biMakeSameSize_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeMakeSameSize(Designer);
	}

	private void biMakeSameWidth_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeMakeSameWidth(Designer);
	}

	private void biMakeSameHeight_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeMakeSameHeight(Designer);
	}

	private void biLock_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeLock(Designer, biLock.Checked);
	}

	private void biLink_Click(object sender, EventArgs e)
	{
		StiActions.Layout.InvokeLink(Designer, biLink.Checked);
	}

	private void biBorderAll_Click(object sender, EventArgs e)
	{
		biBorderTop.Checked = true;
		biBorderLeft.Checked = true;
		biBorderBottom.Checked = true;
		biBorderRight.Checked = true;
		StiActions.Borders.InvokeBorderAll(Designer);
	}

	private void biBorderNone_Click(object sender, EventArgs e)
	{
		biBorderTop.Checked = false;
		biBorderLeft.Checked = false;
		biBorderBottom.Checked = false;
		biBorderRight.Checked = false;
		StiActions.Borders.InvokeBorderNone(Designer);
	}

	private void biBorderTop_Click(object sender, EventArgs e)
	{
		StiActions.Borders.InvokeBorderTop(Designer, biBorderTop.Checked);
	}

	private void biBorderLeft_Click(object sender, EventArgs e)
	{
		StiActions.Borders.InvokeBorderLeft(Designer, biBorderLeft.Checked);
	}

	private void biBorderBottom_Click(object sender, EventArgs e)
	{
		StiActions.Borders.InvokeBorderBottom(Designer, biBorderBottom.Checked);
	}

	private void biBorderRight_Click(object sender, EventArgs e)
	{
		StiActions.Borders.InvokeBorderRight(Designer, biBorderRight.Checked);
	}

	private void biBorderShadow_Click(object sender, EventArgs e)
	{
		StiActions.Borders.InvokeBorderShadow(Designer, biBorderShadow.Checked);
	}

	private void biFillBrush_SelectedColorChanged(object sender, EventArgs e)
	{
		if (!lockRefresh)
		{
			StiActions.Borders.InvokeFillBrush(Designer, biFillBrush.SelectedColor);
		}
	}

	private void biBorderColor_SelectedColorChanged(object sender, EventArgs e)
	{
		if (!lockRefresh)
		{
			StiActions.Borders.InvokeBorderColor(Designer, biBorderColor.SelectedColor);
		}
	}

	private void tbBorderStyleSolid_Click(object sender, EventArgs e)
	{
		if (!lockRefresh)
		{
			StiActions.Borders.InvokeBorderStyle(Designer, StiPenStyle.Solid);
		}
	}

	private void tbBorderStyleDash_Click(object sender, EventArgs e)
	{
		StiActions.Borders.InvokeBorderStyle(Designer, StiPenStyle.Dash);
	}

	private void tbBorderStyleDashDot_Click(object sender, EventArgs e)
	{
		StiActions.Borders.InvokeBorderStyle(Designer, StiPenStyle.DashDot);
	}

	private void tbBorderStyleDashDotDot_Click(object sender, EventArgs e)
	{
		StiActions.Borders.InvokeBorderStyle(Designer, StiPenStyle.DashDotDot);
	}

	private void tbBorderStyleDot_Click(object sender, EventArgs e)
	{
		StiActions.Borders.InvokeBorderStyle(Designer, StiPenStyle.Dot);
	}

	private void tbBorderStyleDouble_Click(object sender, EventArgs e)
	{
		StiActions.Borders.InvokeBorderStyle(Designer, StiPenStyle.Double);
	}

	private void tbBorderStyleNone_Click(object sender, EventArgs e)
	{
		StiActions.Borders.InvokeBorderStyle(Designer, StiPenStyle.None);
	}

	private void ribbonBarBorders_LaunchDialog(object sender, EventArgs e)
	{
		StiActions.Borders.InvokeBordersDialog(Designer);
	}

	private void biFontBold_Click(object sender, EventArgs e)
	{
		StiActions.Font.InvokeBold(Designer, biFontBold.Checked);
	}

	private void biFontItalic_Click(object sender, EventArgs e)
	{
		StiActions.Font.InvokeItalic(Designer, biFontItalic.Checked);
	}

	private void biFontUnderline_Click(object sender, EventArgs e)
	{
		StiActions.Font.InvokeUnderline(Designer, biFontUnderline.Checked);
	}

	private void biFontStrikeout_Click(object sender, EventArgs e)
	{
		StiActions.Font.InvokeStrikeout(Designer, biFontStrikeout.Checked);
	}

	private void biClearContents_Click(object sender, EventArgs e)
	{
		StiActions.Font.InvokeClearContents(Designer);
	}

	private void cbFont_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!lockRefresh && ((ComboBox)cbFont.ComboBoxEx).SelectedItem != null)
		{
			try
			{
				ComboItem comboItem = (ComboItem)((ComboBox)cbFont.ComboBoxEx).SelectedItem;
				StiActions.Font.InvokeFontName(Designer, comboItem.FontName);
			}
			catch
			{
			}
		}
	}

	private void cbFontSize_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!lockRefresh && !string.IsNullOrEmpty(cbFontSize.Text))
		{
			try
			{
				float fontSize = float.Parse(cbFontSize.Text);
				StiActions.Font.InvokeFontSize(Designer, fontSize);
			}
			catch
			{
			}
		}
	}

	private void cbFontSize_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode != 13 || lockRefresh)
		{
			return;
		}
		if (!string.IsNullOrEmpty(cbFontSize.Text))
		{
			try
			{
				float fontSize = float.Parse(cbFontSize.Text);
				StiActions.Font.InvokeFontSize(Designer, fontSize);
			}
			catch
			{
			}
		}
		e.Handled = true;
	}

	private void cbFontSize_EnabledChanged(object sender, EventArgs e)
	{
		((ComboBox)cbFontSize.ComboBoxEx).DropDownStyle = (ComboBoxStyle)(cbFontSize.Enabled ? 1 : 2);
	}

	private void biTextBrush_SelectedColorChanged(object sender, EventArgs e)
	{
		if (!lockRefresh)
		{
			StiActions.Font.InvokeTextBrush(Designer, biTextBrush.SelectedColor);
		}
	}

	private void biFontGrow_Click(object sender, EventArgs e)
	{
		StiActions.Font.InvokeGrowFont(Designer);
	}

	private void biFontShrink_Click(object sender, EventArgs e)
	{
		StiActions.Font.InvokeShrinkFont(Designer);
	}

	private void biContentAlignTop_Click(object sender, EventArgs e)
	{
		StiActions.ContentAlignment.InvokeAlignTop(Designer);
	}

	private void biContentAlignMiddle_Click(object sender, EventArgs e)
	{
		StiActions.ContentAlignment.InvokeAlignMiddle(Designer);
	}

	private void biContentAlignBottom_Click(object sender, EventArgs e)
	{
		StiActions.ContentAlignment.InvokeAlignBottom(Designer);
	}

	private void biContentAlignLeft_Click(object sender, EventArgs e)
	{
		StiActions.ContentAlignment.InvokeAlignLeft(Designer);
	}

	private void biContentAlignCenter_Click(object sender, EventArgs e)
	{
		StiActions.ContentAlignment.InvokeAlignCenter(Designer);
	}

	private void biContentAlignRight_Click(object sender, EventArgs e)
	{
		StiActions.ContentAlignment.InvokeAlignRight(Designer);
	}

	private void biContentAlignWidth_Click(object sender, EventArgs e)
	{
		StiActions.ContentAlignment.InvokeAlignWidth(Designer);
	}

	private void biLineSpacing_PopupOpen(object sender, PopupOpenEventArgs e)
	{
		float? num = null;
		StiComponentsCollection selectedComponentsOnPage = designer.SelectedComponentsOnPage;
		foreach (StiComponent item in selectedComponentsOnPage)
		{
			if (item is StiText stiText)
			{
				if (num.HasValue)
				{
					num = null;
					break;
				}
				num = stiText.LineSpacing;
			}
		}
		cmLineSpacing1.Checked = false;
		cmLineSpacing115.Checked = false;
		cmLineSpacing15.Checked = false;
		cmLineSpacing2.Checked = false;
		cmLineSpacing25.Checked = false;
		cmLineSpacing3.Checked = false;
		ButtonItem buttonItem = null;
		if (num.HasValue)
		{
			if (num == 1f)
			{
				buttonItem = cmLineSpacing1;
			}
			if (num == 1.15f)
			{
				buttonItem = cmLineSpacing115;
			}
			if (num == 1.5f)
			{
				buttonItem = cmLineSpacing15;
			}
			if (num == 2f)
			{
				buttonItem = cmLineSpacing2;
			}
			if (num == 2.5f)
			{
				buttonItem = cmLineSpacing25;
			}
			if (num == 3f)
			{
				buttonItem = cmLineSpacing3;
			}
		}
		if (buttonItem != null)
		{
			buttonItem.Checked = true;
		}
	}

	private void biRotateText_PopupOpen(object sender, PopupOpenEventArgs e)
	{
		foreach (StiComponent item in Designer.SelectedComponentsOnPage)
		{
			if (item is IStiTextOptions)
			{
				cmRotateAngle45.Visible = true;
			}
			if (item is IStiEnumAngle)
			{
				cmRotateAngle45.Visible = false;
				break;
			}
		}
		object obj = null;
		bool flag = false;
		foreach (StiComponent item2 in Designer.SelectedComponentsOnPage)
		{
			if (item2 is IStiTextOptions && !flag)
			{
				float angle = ((IStiTextOptions)item2).TextOptions.Angle;
				if (obj == null)
				{
					obj = angle;
					continue;
				}
				if (angle == (float)obj)
				{
					continue;
				}
				flag = true;
				obj = null;
			}
			if (!(item2 is IStiEnumAngle))
			{
				continue;
			}
			cmRotateAngle45.Visible = false;
			if (!flag)
			{
				float num = (float)((IStiEnumAngle)item2).Angle;
				if (obj == null)
				{
					obj = num;
				}
				else if (num != (float)obj)
				{
					flag = true;
					obj = null;
				}
			}
		}
		cmRotateAngle0.Checked = obj != null && (float)obj == 0f;
		cmRotateAngle45.Checked = obj != null && (float)obj == 45f;
		cmRotateAngle90.Checked = obj != null && (float)obj == 90f;
		cmRotateAngle180.Checked = obj != null && (float)obj == 180f;
		cmRotateAngle270.Checked = obj != null && (float)obj == 270f;
	}

	private void cmLineSpacing1_Click(object sender, EventArgs e)
	{
		StiActions.ContentAlignment.InvokeLineSpacing(designer, 1f);
	}

	private void cmLineSpacing115_Click(object sender, EventArgs e)
	{
		StiActions.ContentAlignment.InvokeLineSpacing(designer, 1.15f);
	}

	private void cmLineSpacing15_Click(object sender, EventArgs e)
	{
		StiActions.ContentAlignment.InvokeLineSpacing(designer, 1.5f);
	}

	private void cmLineSpacing2_Click(object sender, EventArgs e)
	{
		StiActions.ContentAlignment.InvokeLineSpacing(designer, 2f);
	}

	private void cmLineSpacing25_Click(object sender, EventArgs e)
	{
		StiActions.ContentAlignment.InvokeLineSpacing(designer, 2.5f);
	}

	private void cmLineSpacing3_Click(object sender, EventArgs e)
	{
		StiActions.ContentAlignment.InvokeLineSpacing(designer, 3f);
	}

	private void cmRotateAngle0_Click(object sender, EventArgs e)
	{
		StiActions.ContentAlignment.InvokeRotate0(Designer);
	}

	private void cmRotateAngle45_Click(object sender, EventArgs e)
	{
		StiActions.ContentAlignment.InvokeRotate45(Designer);
	}

	private void cmRotateAngle90_Click(object sender, EventArgs e)
	{
		StiActions.ContentAlignment.InvokeRotate90(Designer);
	}

	private void cmRotateAngle180_Click(object sender, EventArgs e)
	{
		StiActions.ContentAlignment.InvokeRotate180(Designer);
	}

	private void cmRotateAngle270_Click(object sender, EventArgs e)
	{
		StiActions.ContentAlignment.InvokeRotate270(Designer);
	}

	private void biWordWrap_Click(object sender, EventArgs e)
	{
		StiActions.ContentAlignment.InvokeWordWrap(Designer, biWordWrap.Checked);
	}

	private void biCurrencySymbol_PopupOpen(object sender, PopupOpenEventArgs e)
	{
		InitCurrencySymbol();
	}

	private void biDateTimeFormat_PopupOpen(object sender, PopupOpenEventArgs e)
	{
		InitDateTimeFormats();
	}

	private void biTextFormat_PopupOpen(object sender, PopupOpenEventArgs e)
	{
		StiFormatService stiFormatService = ((Designer.Report != null) ? Designer.Report.GetCurrentPage().GetTextFormat() : null);
		biFormatGeneral.Checked = stiFormatService?.GetType().ToString().Contains("StiGeneralFormatService") ?? false;
		biFormatNumber.Checked = stiFormatService?.GetType().ToString().Contains("StiNumberFormatService") ?? false;
		biFormatCurrency.Checked = stiFormatService?.GetType().ToString().Contains("StiCurrencyFormatService") ?? false;
		biFormatDate.Checked = stiFormatService?.GetType().ToString().Contains("StiDateFormatService") ?? false;
		biFormatTime.Checked = stiFormatService?.GetType().ToString().Contains("StiTimeFormatService") ?? false;
		biFormatPercentage.Checked = stiFormatService?.GetType().ToString().Contains("StiPercentageFormatService") ?? false;
		biFormatBoolean.Checked = stiFormatService?.GetType().ToString().Contains("StiBooleanFormatService") ?? false;
		biFormatGeneral.Text = StiLocalization.Get("FormFormatEditor", "General");
		biFormatNumber.Text = StiLocalization.Get("FormFormatEditor", "Number");
		biFormatCurrency.Text = StiLocalization.Get("FormFormatEditor", "Currency");
		biFormatDate.Text = StiLocalization.Get("FormFormatEditor", "Date");
		biFormatTime.Text = StiLocalization.Get("FormFormatEditor", "Time");
		biFormatPercentage.Text = StiLocalization.Get("FormFormatEditor", "Percentage");
		biFormatBoolean.Text = StiLocalization.Get("FormFormatEditor", "Boolean");
	}

	private void biFormatGeneral_Click(object sender, EventArgs e)
	{
		StiActions.TextFormat.InvokeGeneralTextFormat(Designer);
	}

	private void biFormatNumber_Click(object sender, EventArgs e)
	{
		StiActions.TextFormat.InvokeNumberTextFormat(Designer);
	}

	private void biFormatCurrency_Click(object sender, EventArgs e)
	{
		StiActions.TextFormat.InvokeCurrencyTextFormat(Designer);
	}

	private void biFormatDate_Click(object sender, EventArgs e)
	{
		StiActions.TextFormat.InvokeDateTextFormat(Designer);
	}

	private void biFormatTime_Click(object sender, EventArgs e)
	{
		StiActions.TextFormat.InvokeTimeTextFormat(Designer);
	}

	private void biFormatPercentage_Click(object sender, EventArgs e)
	{
		StiActions.TextFormat.InvokePercentageTextFormat(Designer);
	}

	private void biFormatBoolean_Click(object sender, EventArgs e)
	{
		StiActions.TextFormat.InvokeBooleanTextFormat(Designer);
	}

	private void ribbonBarTextFormat_LaunchDialog(object sender, EventArgs e)
	{
		StiActions.TextFormat.InvokeTextFormatDialog(Designer);
	}

	private void OnCurrencySymbol_Click(object sender, EventArgs e)
	{
		ButtonItem buttonItem = sender as ButtonItem;
		StiCurrencyFormatService stiCurrencyFormatService = new StiCurrencyFormatService();
		stiCurrencyFormatService.Symbol = buttonItem.Tag as string;
		stiCurrencyFormatService.UseLocalSetting = false;
		StiCurrencyFormatService service = stiCurrencyFormatService;
		StiActions.TextFormat.InvokeTextFormat(Designer, service);
	}

	private void OnDateFormat_Click(object sender, EventArgs e)
	{
		ButtonItem buttonItem = sender as ButtonItem;
		StiDateFormatService stiDateFormatService = new StiDateFormatService();
		stiDateFormatService.StringFormat = buttonItem.Tag as string;
		StiDateFormatService service = stiDateFormatService;
		StiActions.TextFormat.InvokeTextFormat(Designer, service);
	}

	private void OnTimeFormat_Click(object sender, EventArgs e)
	{
		ButtonItem buttonItem = sender as ButtonItem;
		StiTimeFormatService stiTimeFormatService = new StiTimeFormatService();
		stiTimeFormatService.StringFormat = buttonItem.Tag as string;
		StiTimeFormatService service = stiTimeFormatService;
		StiActions.TextFormat.InvokeTextFormat(Designer, service);
	}

	private void biInteraction_Click(object sender, EventArgs e)
	{
		StiActions.Style.InvokeInteraction(Designer);
	}

	private void biConditions_Click(object sender, EventArgs e)
	{
		StiActions.Style.InvokeConditions(Designer);
	}

	private void biCopyStyle_Click(object sender, EventArgs e)
	{
		biCopyStyle.Checked = !biCopyStyle.Checked;
		if (biCopyStyle.Checked)
		{
			designer.StyleTool.Activate();
		}
		else
		{
			designer.SelectTool.Activate();
		}
	}

	private void biStyleDesigner_Click(object sender, EventArgs e)
	{
		StiActions.Style.InvokeStyleDesigner(Designer);
	}

	private void ribbonBarStyle_LaunchDialog(object sender, EventArgs e)
	{
		StiActions.Style.InvokeStyleDesigner(Designer);
	}

	private void biViewShowGrid_Click(object sender, EventArgs e)
	{
		StiActions.ViewOptions.InvokeShowGrid(Designer);
	}

	private void biViewAlignToGrid_Click(object sender, EventArgs e)
	{
		StiActions.ViewOptions.InvokeAlignToGrid(Designer);
	}

	private void biViewShowHeaders_Click(object sender, EventArgs e)
	{
		StiActions.ViewOptions.InvokeShowHeaders(Designer);
	}

	private void biViewShowOrder_Click(object sender, EventArgs e)
	{
		StiActions.ViewOptions.InvokeShowOrder(Designer);
	}

	private void biViewShowRulers_Click(object sender, EventArgs e)
	{
		StiActions.ViewOptions.InvokeShowRulers(Designer);
	}

	private void biShowToolbox_Click(object sender, EventArgs e)
	{
		StiToolboxToolbarService stiToolboxToolbarService = Designer.Services.GetService(typeof(StiToolbarService)) as StiToolboxToolbarService;
		StiSettings.Set("CloudDesigner", "ShowStandardToolbox", !stiToolboxToolbarService.ToolBar.Visible);
		designer.InvokeBuidingToolbox();
		Designer.SaveDesignerParam();
		((Control)Designer).Refresh();
	}

	private void biGridModeLines_Click(object sender, EventArgs e)
	{
		StiActions.ViewOptions.InvokeGridModeLines(Designer);
	}

	private void biGridModeDotes_Click(object sender, EventArgs e)
	{
		StiActions.ViewOptions.InvokeGridModeDots(Designer);
	}

	private void biQuickInfo_PopupOpen(object sender, PopupOpenEventArgs e)
	{
		biQuickInfoNone.Checked = Designer.Report.Info.QuickInfoType == StiQuickInfoType.None;
		biQuickInfoShowComponentsNames.Checked = Designer.Report.Info.QuickInfoType == StiQuickInfoType.ShowComponentsNames;
		biQuickInfoShowAliases.Checked = Designer.Report.Info.QuickInfoType == StiQuickInfoType.ShowAliases;
		biQuickInfoShowContent.Checked = Designer.Report.Info.QuickInfoType == StiQuickInfoType.ShowContent;
		biQuickInfoShowFields.Checked = Designer.Report.Info.QuickInfoType == StiQuickInfoType.ShowFields;
		biQuickInfoShowFieldsOnly.Checked = Designer.Report.Info.QuickInfoType == StiQuickInfoType.ShowFieldsOnly;
		biQuickInfoShowEvents.Checked = Designer.Report.Info.QuickInfoType == StiQuickInfoType.ShowEvents;
		biQuickInfoOverlay.Checked = Designer.Report.Info.QuickInfoOverlay;
	}

	private void ciQuickInfoNone_Click(object sender, EventArgs e)
	{
		biQuickInfoNone.Checked = true;
		biQuickInfoShowComponentsNames.Checked = false;
		biQuickInfoShowAliases.Checked = false;
		biQuickInfoShowContent.Checked = false;
		biQuickInfoShowFields.Checked = false;
		biQuickInfoShowFieldsOnly.Checked = false;
		biQuickInfoShowEvents.Checked = false;
		StiActions.ViewOptions.InvokeQuickInfoNone(Designer);
	}

	private void ciQuickInfoShowComponentsNames_Click(object sender, EventArgs e)
	{
		biQuickInfoNone.Checked = false;
		biQuickInfoShowComponentsNames.Checked = true;
		biQuickInfoShowAliases.Checked = false;
		biQuickInfoShowContent.Checked = false;
		biQuickInfoShowFields.Checked = false;
		biQuickInfoShowFieldsOnly.Checked = false;
		biQuickInfoShowEvents.Checked = false;
		StiActions.ViewOptions.InvokeQuickInfoShowComponentsNames(Designer);
	}

	private void ciQuickInfoShowAliases_Click(object sender, EventArgs e)
	{
		biQuickInfoNone.Checked = false;
		biQuickInfoShowComponentsNames.Checked = false;
		biQuickInfoShowAliases.Checked = true;
		biQuickInfoShowContent.Checked = false;
		biQuickInfoShowFields.Checked = false;
		biQuickInfoShowFieldsOnly.Checked = false;
		biQuickInfoShowEvents.Checked = false;
		StiActions.ViewOptions.InvokeQuickInfoShowAliases(Designer);
	}

	private void ciQuickInfoShowContent_Click(object sender, EventArgs e)
	{
		biQuickInfoNone.Checked = false;
		biQuickInfoShowComponentsNames.Checked = false;
		biQuickInfoShowAliases.Checked = false;
		biQuickInfoShowContent.Checked = true;
		biQuickInfoShowFields.Checked = false;
		biQuickInfoShowFieldsOnly.Checked = false;
		biQuickInfoShowEvents.Checked = false;
		StiActions.ViewOptions.InvokeQuickInfoShowContent(Designer);
	}

	private void ciQuickInfoShowFields_Click(object sender, EventArgs e)
	{
		biQuickInfoNone.Checked = false;
		biQuickInfoShowComponentsNames.Checked = false;
		biQuickInfoShowAliases.Checked = false;
		biQuickInfoShowContent.Checked = false;
		biQuickInfoShowFields.Checked = true;
		biQuickInfoShowFieldsOnly.Checked = false;
		biQuickInfoShowEvents.Checked = false;
		StiActions.ViewOptions.InvokeQuickInfoShowFields(Designer);
	}

	private void ciQuickInfoShowFieldsOnly_Click(object sender, EventArgs e)
	{
		biQuickInfoNone.Checked = false;
		biQuickInfoShowComponentsNames.Checked = false;
		biQuickInfoShowAliases.Checked = false;
		biQuickInfoShowContent.Checked = false;
		biQuickInfoShowFields.Checked = false;
		biQuickInfoShowFieldsOnly.Checked = true;
		biQuickInfoShowEvents.Checked = false;
		Designer.Report.Info.QuickInfoType = StiQuickInfoType.ShowFieldsOnly;
		((Control)Designer).Refresh();
	}

	private void ciQuickInfoOverlay_Click(object sender, EventArgs e)
	{
		StiActions.ViewOptions.InvokeQuickInfoOverlay(Designer);
	}

	private void ciQuickInfoShowEvents_Click(object sender, EventArgs e)
	{
		biQuickInfoNone.Checked = false;
		biQuickInfoShowComponentsNames.Checked = false;
		biQuickInfoShowAliases.Checked = false;
		biQuickInfoShowContent.Checked = false;
		biQuickInfoShowFields.Checked = false;
		biQuickInfoShowFieldsOnly.Checked = false;
		biQuickInfoShowEvents.Checked = true;
		StiActions.ViewOptions.InvokeQuickInfoShowEvents(Designer);
	}

	private void OnPanelClick(object sender, EventArgs e)
	{
		ButtonItem buttonItem = sender as ButtonItem;
		PanelSelect(buttonItem.Tag as StiPanelService, !buttonItem.IsMouseOver);
	}

	internal void PanelSelect(StiPanelService service, bool onlyActivate)
	{
		if (service.DockingItem.Visible)
		{
			if (service is StiPropertiesPanelService && onlyActivate)
			{
				if (service.DockingBar.Items.Contains(service.DockingItem))
				{
					service.DockingBar.SelectedDockContainerItem = service.DockingItem;
				}
				if (service is StiPropertiesPanelService)
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

	private void biOrientation_PopupOpen(object sender, PopupOpenEventArgs e)
	{
		biOrientationLandscape.Checked = StiActions.Page.States.GetOrientationLandscapeChecked(Designer);
		biOrientationPortrait.Checked = StiActions.Page.States.GetOrientationPortraitChecked(Designer);
	}

	private void biMargins_PopupOpen(object sender, PopupOpenEventArgs e)
	{
		biMarginsNormal.Text = StiLocalization.Get("FormDesigner", "MarginsNormal");
		biMarginsNarrow.Text = StiLocalization.Get("FormDesigner", "MarginsNarrow");
		biMarginsWide.Text = StiLocalization.Get("FormDesigner", "MarginsWide");
		StiPage currentPage = Designer.Report.GetCurrentPage();
		string text = string.Empty;
		if (Designer.Report.Unit is StiCentimetersUnit)
		{
			text = "cm";
		}
		else if (Designer.Report.Unit is StiMillimetersUnit)
		{
			text = "mm";
		}
		else if (Designer.Report.Unit is StiInchesUnit)
		{
			text = "in";
		}
		else if (Designer.Report.Unit is StiHundredthsOfInchUnit)
		{
			text = "hi";
		}
		decimal d = (decimal)Designer.Report.Unit.ConvertFromHInches(39.0);
		d = Math.Round(d, 1);
		biMarginsNormal.Text = string.Format("<br><b>{0}</b></br><br>{1}: {2}{3},  {4}: {5}{6}</br><br>{7}: {8}{9},  {10}: {11}{12}</br><br> </br>", StiLocalization.Get("FormDesigner", "MarginsNormal"), StiLocalization.Get("PropertyMain", "Left"), d, text, StiLocalization.Get("PropertyMain", "Right"), d, text, StiLocalization.Get("PropertyMain", "Top"), d, text, StiLocalization.Get("PropertyMain", "Bottom"), d, text);
		biMarginsNarrow.Text = string.Format("<br><b>{0}</b></br><br>{1}: {2}{3},  {4}: {5}{6}</br><br>{7}: {8}{9},  {10}: {11}{12}</br><br> </br>", StiLocalization.Get("FormDesigner", "MarginsNarrow"), StiLocalization.Get("PropertyMain", "Left"), d / 2m, text, StiLocalization.Get("PropertyMain", "Right"), d / 2m, text, StiLocalization.Get("PropertyMain", "Top"), d / 2m, text, StiLocalization.Get("PropertyMain", "Bottom"), d / 2m, text);
		biMarginsWide.Text = string.Format("<br><b>{0}</b></br><br>{1}: {2}{3},  {4}: {5}{6}</br><br>{7}: {8}{9},  {10}: {11}{12}</br><br> </br>", StiLocalization.Get("FormDesigner", "MarginsWide"), StiLocalization.Get("PropertyMain", "Left"), d * 2m, text, StiLocalization.Get("PropertyMain", "Right"), d * 2m, text, StiLocalization.Get("PropertyMain", "Top"), d * 2m, text, StiLocalization.Get("PropertyMain", "Bottom"), d * 2m, text);
		biMarginsNormal.Checked = (decimal)currentPage.Margins.Left == d && (decimal)currentPage.Margins.Right == d && (decimal)currentPage.Margins.Top == d && (decimal)currentPage.Margins.Bottom == d;
		biMarginsNarrow.Checked = (decimal)currentPage.Margins.Left == d / 2m && (decimal)currentPage.Margins.Right == d / 2m && (decimal)currentPage.Margins.Top == d / 2m && (decimal)currentPage.Margins.Bottom == d / 2m;
		biMarginsWide.Checked = (decimal)currentPage.Margins.Left == d * 2m && (decimal)currentPage.Margins.Right == d * 2m && (decimal)currentPage.Margins.Top == d * 2m && (decimal)currentPage.Margins.Bottom == d * 2m;
	}

	private void biMarginsNormal_Click(object sender, EventArgs e)
	{
		StiActions.Page.InvokeMarginsNormal(Designer);
	}

	private void biMarginsNarrow_Click(object sender, EventArgs e)
	{
		StiActions.Page.InvokeMarginsNarrow(Designer);
	}

	private void biMarginsWide_Click(object sender, EventArgs e)
	{
		StiActions.Page.InvokeMarginsWide(Designer);
	}

	private void biOrientationPortrait_Click(object sender, EventArgs e)
	{
		StiActions.Page.InvokeOrientationPortrait(Designer);
	}

	private void biOrientationLandscape_Click(object sender, EventArgs e)
	{
		StiActions.Page.InvokeOrientationLandscape(Designer);
	}

	private void biColumnsOne_Click(object sender, EventArgs e)
	{
		StiActions.Page.InvokeColumnsOne(Designer);
	}

	private void biColumnsTwo_Click(object sender, EventArgs e)
	{
		StiActions.Page.InvokeColumnsTwo(Designer);
	}

	private void biColumnsThree_Click(object sender, EventArgs e)
	{
		StiActions.Page.InvokeColumnsThree(Designer);
	}

	private void biSurfaceViewDesktop_Click(object sender, EventArgs e)
	{
		StiActions.Page.InvokeSurfaceViewDesktop(Designer);
	}

	private void biSurfaceViewMobile_Click(object sender, EventArgs e)
	{
		StiActions.Page.InvokeSurfaceViewMobile(Designer);
	}

	private void biSurfaceViewRemoveMobileSurface_Click(object sender, EventArgs e)
	{
		StiActions.Page.InvokeSurfaceViewRemoveMobileSurface(Designer);
	}

	private void biSurfaceViewComponents_Click(object sender, EventArgs e)
	{
		StiActions.Page.InvokeSurfaceViewComponents(Designer);
	}

	private void ribbonBarPageSetup_LaunchDialog(object sender, EventArgs e)
	{
		StiActions.Page.InvokePageSetupDialog(Designer);
	}

	private void biPageSize_PopupOpen(object sender, PopupOpenEventArgs e)
	{
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Expected O, but got Unknown
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Expected O, but got Unknown
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Expected O, but got Unknown
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Expected O, but got Unknown
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Expected O, but got Unknown
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Expected O, but got Unknown
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Expected O, but got Unknown
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Expected O, but got Unknown
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Expected O, but got Unknown
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0422: Unknown result type (might be due to invalid IL or missing references)
		//IL_0429: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05de: Invalid comparison between Unknown and I4
		//IL_054e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0555: Invalid comparison between Unknown and I4
		//IL_05f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f7: Invalid comparison between Unknown and I4
		//IL_056a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0571: Invalid comparison between Unknown and I4
		//IL_0609: Unknown result type (might be due to invalid IL or missing references)
		//IL_060f: Invalid comparison between Unknown and I4
		//IL_0586: Unknown result type (might be due to invalid IL or missing references)
		//IL_058c: Invalid comparison between Unknown and I4
		//IL_0621: Unknown result type (might be due to invalid IL or missing references)
		//IL_0627: Invalid comparison between Unknown and I4
		//IL_05a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a7: Invalid comparison between Unknown and I4
		//IL_0639: Unknown result type (might be due to invalid IL or missing references)
		//IL_063f: Invalid comparison between Unknown and I4
		//IL_05bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c2: Invalid comparison between Unknown and I4
		biPageSize.SubItems.Clear();
		StiPage currentPage = Designer.Report.GetCurrentPage();
		if (currentPage.IsDashboard)
		{
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(DesignerOffice2013Resource));
			ButtonItem buttonItem = new ButtonItem();
			buttonItem.Click += OnDashboardSize_Click;
			buttonItem.Tag = (object)new PaperSize("", 400, 710);
			buttonItem.Text = $"{9}:{16}";
			buttonItem.Image = (Image)componentResourceManager.GetObject("Size_9_16" + StiScale.StepName);
			biPageSize.SubItems.Add(buttonItem);
			ButtonItem buttonItem2 = new ButtonItem();
			buttonItem2.Click += OnDashboardSize_Click;
			buttonItem2.Tag = (object)new PaperSize("", 400, 800);
			buttonItem2.Text = $"{9}:{18}";
			buttonItem2.Image = (Image)componentResourceManager.GetObject("Size_9_18" + StiScale.StepName);
			biPageSize.SubItems.Add(buttonItem2);
			ButtonItem buttonItem3 = new ButtonItem();
			buttonItem3.Tag = (object)new PaperSize("", 800, 800);
			buttonItem3.Click += OnDashboardSize_Click;
			buttonItem3.Text = $"{1}:{1}";
			buttonItem3.Image = (Image)componentResourceManager.GetObject("Size_1_1" + StiScale.StepName);
			biPageSize.SubItems.Add(buttonItem3);
			ButtonItem buttonItem4 = new ButtonItem();
			buttonItem4.Click += OnDashboardSize_Click;
			buttonItem4.Tag = (object)new PaperSize("", 800, 640);
			buttonItem4.Text = $"{5}:{4}";
			buttonItem4.Image = (Image)componentResourceManager.GetObject("Size_5_4" + StiScale.StepName);
			biPageSize.SubItems.Add(buttonItem4);
			ButtonItem buttonItem5 = new ButtonItem();
			buttonItem5.Click += OnDashboardSize_Click;
			buttonItem5.Tag = (object)new PaperSize("", 1200, 750);
			buttonItem5.Text = $"{16}:{10}";
			buttonItem5.Image = (Image)componentResourceManager.GetObject("Size_16_10" + StiScale.StepName);
			biPageSize.SubItems.Add(buttonItem5);
			return;
		}
		int num = (int)Math.Round(currentPage.Unit.ConvertToHInches(currentPage.PageWidth), 0);
		int num2 = (int)Math.Round(currentPage.Unit.ConvertToHInches(currentPage.PageHeight), 0);
		PrinterSettings val = new PrinterSettings();
		PaperSizeCollection val2 = StiOptions.Print.CustomPaperSizes;
		if (val2 == null)
		{
			val2 = val.PaperSizes;
		}
		string text = string.Empty;
		if (Designer.Report.Unit is StiCentimetersUnit)
		{
			text = "cm";
		}
		else if (Designer.Report.Unit is StiMillimetersUnit)
		{
			text = "mm";
		}
		else if (Designer.Report.Unit is StiInchesUnit)
		{
			text = "in";
		}
		else if (Designer.Report.Unit is StiHundredthsOfInchUnit)
		{
			text = "hi";
		}
		foreach (PaperSize item in val2)
		{
			PaperSize val3 = item;
			bool @checked = false;
			if ((int)currentPage.PaperSize == 0)
			{
				if ((Math.Abs(val3.Width - num) < 10 && Math.Abs(val3.Height - num2) < 10) || (Math.Abs(val3.Height - num) < 10 && Math.Abs(val3.Width - num2) < 10))
				{
					@checked = true;
				}
			}
			else if (currentPage.PaperSize == val3.Kind)
			{
				@checked = true;
			}
			if ((int)val3.Kind == 0)
			{
				continue;
			}
			decimal num3 = val3.Width;
			decimal num4 = val3.Height;
			if (currentPage.Unit is StiCentimetersUnit || currentPage.Unit is StiMillimetersUnit)
			{
				num3 = PrinterUnitConvert.Convert(val3.Width, (PrinterUnit)1, (PrinterUnit)3);
				num4 = PrinterUnitConvert.Convert(val3.Height, (PrinterUnit)1, (PrinterUnit)3);
				if (currentPage.Unit is StiCentimetersUnit)
				{
					num3 /= 10m;
					num4 /= 10m;
				}
			}
			else
			{
				num3 = Math.Round((decimal)currentPage.ConvertFromHInches(currentPage.Unit, (double)num3), 2);
				num4 = Math.Round((decimal)currentPage.ConvertFromHInches(currentPage.Unit, (double)num4), 2);
			}
			ButtonItem buttonItem6 = new ButtonItem();
			buttonItem6.Click += OnPageSize_Click;
			buttonItem6.Tag = val3;
			buttonItem6.Text = val3.PaperName;
			if (currentPage.Orientation == StiPageOrientation.Portrait)
			{
				if ((int)val3.Kind == 9)
				{
					buttonItem6.Image = (Image)(object)DesignerOffice2013Resource.Size_A4;
				}
				else if ((int)val3.Kind == 11)
				{
					buttonItem6.Image = (Image)(object)DesignerOffice2013Resource.Size_A5;
				}
				else if ((int)val3.Kind == 7)
				{
					buttonItem6.Image = (Image)(object)DesignerOffice2013Resource.Size_Executive;
				}
				else if ((int)val3.Kind == 5)
				{
					buttonItem6.Image = (Image)(object)DesignerOffice2013Resource.Size_Legal;
				}
				else if ((int)val3.Kind == 1)
				{
					buttonItem6.Image = (Image)(object)DesignerOffice2013Resource.Size_Letter;
				}
			}
			else if ((int)val3.Kind == 9)
			{
				buttonItem6.Image = (Image)(object)DesignerOffice2013Resource.Landscape_Size_A4;
			}
			else if ((int)val3.Kind == 11)
			{
				buttonItem6.Image = (Image)(object)DesignerOffice2013Resource.Landscape_Size_A5;
			}
			else if ((int)val3.Kind == 7)
			{
				buttonItem6.Image = (Image)(object)DesignerOffice2013Resource.Landscape_Size_Executive;
			}
			else if ((int)val3.Kind == 5)
			{
				buttonItem6.Image = (Image)(object)DesignerOffice2013Resource.Landscape_Size_Legal;
			}
			else if ((int)val3.Kind == 1)
			{
				buttonItem6.Image = (Image)(object)DesignerOffice2013Resource.Landscape_Size_Letter;
			}
			buttonItem6.Image = StiImageResizer.GetImage(buttonItem6.Image);
			if (currentPage.Orientation == StiPageOrientation.Portrait)
			{
				buttonItem6.Text = $"<b>{val3.PaperName}</b><br>{num3}{text} x {num4}{text}</br><br> </br>";
			}
			else
			{
				buttonItem6.Text = $"<b>{val3.PaperName}</b><br>{num4}{text} x {num3}{text}</br><br> </br>";
			}
			buttonItem6.Checked = @checked;
			buttonItem6.CanCustomize = false;
			biPageSize.SubItems.Add(buttonItem6);
		}
		if (val2.Count == 0)
		{
			ButtonItem buttonItem7 = new ButtonItem();
			buttonItem7.Text = string.Format("<b>{0}</b><br>{1}{2} x {3}{4}</br><br> </br>", StiLocalization.Get("Wizards", "Custom"), num, text, num2, text);
			buttonItem7.Checked = true;
			biPageSize.SubItems.Add(buttonItem7);
		}
	}

	private void OnPageSize_Click(object sender, EventArgs e)
	{
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		designer.UndoRedoSave(StiLocalization.Get("PropertyMain", "Size"), onlyCurrentPage: true);
		ButtonItem buttonItem = sender as ButtonItem;
		object tag = buttonItem.Tag;
		PaperSize val = (PaperSize)((tag is PaperSize) ? tag : null);
		StiPage currentPage = Designer.Report.GetCurrentPage();
		double value = val.Width;
		double value2 = val.Height;
		if (currentPage.Unit is StiCentimetersUnit || currentPage.Unit is StiMillimetersUnit)
		{
			value = PrinterUnitConvert.Convert(val.Width, (PrinterUnit)1, (PrinterUnit)3);
			value2 = PrinterUnitConvert.Convert(val.Height, (PrinterUnit)1, (PrinterUnit)3);
			if (currentPage.Unit is StiCentimetersUnit)
			{
				value /= 10.0;
				value2 /= 10.0;
			}
		}
		else
		{
			value = Math.Round(currentPage.ConvertFromHInches(currentPage.Unit, value), 2);
			value2 = Math.Round(currentPage.ConvertFromHInches(currentPage.Unit, value2), 2);
		}
		if (currentPage.Orientation == StiPageOrientation.Landscape)
		{
			currentPage.PageWidth = value;
			currentPage.PageHeight = value2;
		}
		else
		{
			currentPage.PageWidth = value;
			currentPage.PageHeight = value2;
		}
		currentPage.PaperSize = val.Kind;
		Designer.Builder.BuildReport();
	}

	private void OnDashboardSize_Click(object sender, EventArgs e)
	{
		designer.UndoRedoSave(StiLocalization.Get("PropertyMain", "Size"), onlyCurrentPage: true);
		ButtonItem buttonItem = sender as ButtonItem;
		object tag = buttonItem.Tag;
		PaperSize val = (PaperSize)((tag is PaperSize) ? tag : null);
		StiPage currentPage = Designer.Report.GetCurrentPage();
		double width = val.Width;
		double height = val.Height;
		double width2 = currentPage.Width;
		double height2 = currentPage.Height;
		currentPage.Width = width;
		currentPage.Height = height;
		((StiDesignerControl)designer.DesignerControl)?.Builder.BuildReport();
		StiDashboardSizeHelper.Place(currentPage as IStiDashboard, width2, height2);
		Designer.Builder.BuildReport();
	}

	private void OnPanelVisibleChanged(object sender, EventArgs e)
	{
		DockContainerItem dockContainerItem = (DockContainerItem)sender;
		if (((StiPanelService)itemShowDictionary.Tag).DockingItem == dockContainerItem)
		{
			itemShowDictionary.Checked = dockContainerItem.Visible;
		}
		else if (((StiPanelService)itemShowProperties.Tag).DockingItem == dockContainerItem)
		{
			itemShowProperties.Checked = dockContainerItem.Visible;
		}
		else if (((StiPanelService)itemShowReportTree.Tag).DockingItem == dockContainerItem)
		{
			itemShowReportTree.Checked = dockContainerItem.Visible;
		}
	}

	private void biCheckIssues_Click(object sender, EventArgs e)
	{
		StiReportChecker.RunReportChecker(Designer.Report, checkKeyEscape: true);
	}

	private void biWatermarkSettings_Click(object sender, EventArgs e)
	{
		StiActions.Watermark.InvokeWatermarkEditor(designer);
	}

	private void BuildTemplates()
	{
		StiReportTemplatesService service = StiReportTemplatesService.GetService();
		if (service == null)
		{
			return;
		}
		string[] reportsNames = service.GetReportsNames();
		if (reportsNames == null)
		{
			return;
		}
		bool flag = reportsNames.Length > 5;
		ButtonItem buttonItem = null;
		GalleryContainer galleryContainer = null;
		if (flag)
		{
			buttonItem = new ButtonItem
			{
				Image = (Image)(object)Resources.BigReportTemplate,
				BeginGroup = true,
				Text = StiLocalization.Get("Wizards", "FromReportTemplate")
			};
			biNew.SubItems.Add(buttonItem);
			galleryContainer = new GalleryContainer
			{
				DefaultSize = new Size(420, 420)
			};
			buttonItem.SubItems.Add(galleryContainer);
		}
		int num = 0;
		string[] array = reportsNames;
		foreach (string text in array)
		{
			if (service.ServiceEnabled)
			{
				ButtonItem buttonItem2 = new ButtonItem
				{
					ButtonStyle = eButtonStyle.ImageAndText,
					Tag = text,
					Image = (Image)(object)Resources.BigReportTemplate,
					BeginGroup = (num == 0),
					Text = text
				};
				if (flag)
				{
					buttonItem2.FixedSize = new Size(200, 32);
				}
				buttonItem2.Tooltip = StiLocalization.Get("Wizards", "FromReportTemplate");
				buttonItem2.Click += OnReportTemplate;
				if (reportsNames.Length > 5)
				{
					galleryContainer.SubItems.Add(buttonItem2);
				}
				else
				{
					biNew.SubItems.Add(buttonItem2);
				}
				templatesButtons.Add(buttonItem2);
				num++;
			}
		}
	}

	private void OnReportTemplate(object sender, EventArgs e)
	{
		ButtonItem buttonItem = sender as ButtonItem;
		StiActions.MainMenu.InvokeReportTemplate(Designer, buttonItem.Tag as string);
	}

	private void BuildWizards()
	{
		IEnumerable<StiWizardService> enumerable = StiDesignerOptions.Wizards.Where((StiWizardService s) => s.ServiceEnabled);
		if (!enumerable.Any())
		{
			return;
		}
		int num = 0;
		foreach (StiWizardService item in enumerable)
		{
			ButtonItem buttonItem = new ButtonItem
			{
				Tag = item,
				Image = (Image)(object)Resources.BigReportWizard,
				BeginGroup = (num == 0),
				Text = item.ServiceName,
				Tooltip = StiLocalization.Get("Wizards", "UsingReportWizard")
			};
			buttonItem.Click += OnReportWizard;
			biNew.SubItems.Add(buttonItem);
			wizardsButtons.Add(buttonItem);
			num++;
		}
	}

	private void OnReportWizard(object sender, EventArgs e)
	{
		ButtonItem buttonItem = sender as ButtonItem;
		StiWizardService wizard = buttonItem.Tag as StiWizardService;
		StiActions.MainMenu.InvokeReportWizard(Designer, wizard);
	}

	private void CheckStatePanel()
	{
		biNotifications.Visible = false;
		biLogin.Visible = false;
		biLoginMenu.Visible = false;
		((Control)ribbonControl).Refresh();
		if (StiAccountSettings.CurrentAccount.User != null)
		{
			UpdateLoginMenu();
		}
		else
		{
			biLogin.Visible = true;
			IStiAuthorizationWindow authorizationWindow = StiAccountSettings.CurrentAccount.AccountCreater.GetAuthorizationWindow();
			authorizationWindow.CheckClose = true;
			if (!authorizationWindow.ShowDialog(((Control)Designer).Handle).GetValueOrDefault())
			{
				if (RibbonDesigner != null)
				{
					designer?.UndoRedo?.Clear();
					if (RibbonDesigner.Report != null)
					{
						RibbonDesigner.Report.IsModified = false;
					}
					((Form)RibbonDesigner).Close();
				}
			}
			else
			{
				UpdateLoginMenu();
			}
		}
		((Control)ribbonControl).Refresh();
	}

	private void UpdateLoginMenu()
	{
		bool flag = biLoginMenu.Text == "Login Menu";
		biLogin.Visible = false;
		biLoginMenu.Visible = true;
		if (StiAccountSettings.CurrentAccount.User == null)
		{
			StiAccountSettings.CurrentAccount.CheckLicense();
		}
		if (StiAccountSettings.CurrentAccount.User != null)
		{
			biLoginMenu.Text = StiAccountSettings.CurrentAccount.User.FirstName + " " + StiAccountSettings.CurrentAccount.User.LastName;
			biLoginMenu.Image = (Image)(object)GetImageAccount(StiAccountSettings.CurrentAccount.User);
			biLoginMenu.ImageSmall = (Image)(object)GetImageAccount(StiAccountSettings.CurrentAccount.User);
			CalculateRightButtonsSize();
		}
		BuildLoginMenu();
		if (flag)
		{
			InitDesignerSpecification();
			InitTrialDaysLeft();
			StiSettings.Load();
			if (StiSettings.GetBool("GetStartedWindow", "ShowNextTime", nullValue: true))
			{
				StiActions.MainMenu.InvokeGetStarted(Designer);
			}
		}
	}

	private void CalculateRightButtonsSize()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		Graphics globalMeasureGraphics = StiReport.GlobalMeasureGraphics;
		Font val = new Font("Segoe UI", 9.25f);
		try
		{
			SizeF sizeF = globalMeasureGraphics.MeasureString(biPublish.Text, val);
			biPublish.FixedSize = new Size((int)(sizeF.Width + (float)StiScale.XXI(18.0)), StiScale.YYI(24.0));
			biShare.FixedSize = new Size(StiScale.XXI(28.0), StiScale.YYI(24.0));
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void UpdateSize(RibbonBar bar)
	{
		((Control)bar).Width = bar.GetAutoSizeWidth();
	}

	private int GetOfficeStartPadding()
	{
		double factor = StiScale.Factor;
		if (factor != 1.0)
		{
			if (factor == 1.25)
			{
				return 15;
			}
			return StiScale.I20;
		}
		return 10;
	}

	private void InitScaledOfficeButton()
	{
		OfficeStartButton.Office2007StartButton = false;
		OfficeStartButton.ImagePaddingHorizontal = GetOfficeStartPadding();
		OfficeStartButton.ImagePaddingVertical = StiScale.I6 + 1;
		OfficeStartButton.Image = null;
		Image startButtonDefault = (GlobalManager.Renderer as Office2007Renderer).ColorTable.RibbonControl.StartButtonDefault;
		OfficeStartButton.ImageFixedSize = new Size(startButtonDefault.Width, 0);
		OfficeStartButton.LockImageState = true;
	}

	private void DoScaling()
	{
		string stepName = StiScale.StepName;
		if (biSaveAs != null)
		{
			ButtonItem buttonItem = biSaveAs;
			Image image = (biSaveAs.ImageSmall = StiReportDesignImages.Actions.SaveFile());
			buttonItem.Image = image;
		}
		if (biReportOpen != null)
		{
			ButtonItem buttonItem2 = biReportOpen;
			Image image = (biReportOpen.ImageSmall = StiReportDesignImages.Actions.OpenFile());
			buttonItem2.Image = image;
		}
		biSelectLanguage.Image = (Image)(object)StiButtonHelper.GetDropDownArrowBitmapLeftAligned(StiScale.I8, StiScale.I16, Color.White);
		biPaste.Image = StiReportDesignImages.Editor.ClipboardPaste(StiImageSize.Double);
		biCut.Image = StiReportDesignImages.Editor.ClipboardCut();
		biCopy.Image = StiReportDesignImages.Editor.ClipboardCopy();
		biDelete.Image = StiReportDesignImages.Actions.Remove();
		biFontBold.Image = StiReportDesignImages.Editor.FontStyleBold();
		biFontItalic.Image = StiReportDesignImages.Editor.FontStyleItalic();
		biFontUnderline.Image = StiReportDesignImages.Editor.FontStyleUnderline();
		biFontStrikeout.Image = StiReportDesignImages.Editor.FontStyleStrikethrough();
		biFontGrow.Image = StiReportDesignImages.Editor.FontSizeIncrease();
		biFontShrink.Image = StiReportDesignImages.Editor.FontSizeDecrease();
		biClearContents.Image = StiReportDesignImages.Editor.ClearContent();
		biTextBrush.ColorIcon = StiReportDesignImages.Editor.FontColor();
		biContentAlignTop.Image = StiReportDesignImages.Editor.AlignTop();
		biContentAlignMiddle.Image = StiReportDesignImages.Editor.AlignMiddle();
		biContentAlignBottom.Image = StiReportDesignImages.Editor.AlignBottom();
		biContentAlignLeft.Image = StiReportDesignImages.Editor.AlignLeft();
		biContentAlignCenter.Image = StiReportDesignImages.Editor.AlignCenter();
		biContentAlignRight.Image = StiReportDesignImages.Editor.AlignRight();
		biContentAlignWidth.Image = StiReportDesignImages.Editor.AlignJustify();
		biLineSpacing.Image = StiReportDesignImages.Editor.LineSpacing();
		biRotateText.Image = StiReportDesignImages.Editor.RotateText();
		biWordWrap.Image = StiReportDesignImages.Editor.WrapText();
		biFormatGeneral.Image = StiReportDesignImages.Format.General(StiImageSize.Double);
		biFormatNumber.Image = StiReportDesignImages.Format.Number(StiImageSize.Double);
		biFormatCurrency.Image = StiReportDesignImages.Format.Currency(StiImageSize.Double);
		biFormatDate.Image = StiReportDesignImages.Format.Date(StiImageSize.Double);
		biFormatTime.Image = StiReportDesignImages.Format.Time(StiImageSize.Double);
		biFormatPercentage.Image = StiReportDesignImages.Format.Percentage(StiImageSize.Double);
		biFormatBoolean.Image = StiReportDesignImages.Format.Boolean(StiImageSize.Double);
		biFormatCurrency.Image = StiReportDesignImages.Format.Currency(StiImageSize.Double);
		biCurrencySymbol.Image = StiReportDesignImages.Format.Currency();
		biDateTimeFormat.Image = StiReportDesignImages.Format.DateAndTime();
		biBorderAll.Image = StiReportDesignImages.Editor.BorderAll();
		biBorderNone.Image = StiReportDesignImages.Editor.BorderNone();
		biBorderTop.Image = StiReportDesignImages.Editor.BorderTop();
		biBorderLeft.Image = StiReportDesignImages.Editor.BorderLeft();
		biBorderBottom.Image = StiReportDesignImages.Editor.BorderBottom();
		biBorderRight.Image = StiReportDesignImages.Editor.BorderRight();
		biBorderShadow.Image = StiReportDesignImages.Editor.BorderShadow();
		biBorderColor.ColorIcon = StiReportDesignImages.Editor.Brush();
		biFillBrush.ColorIcon = StiReportDesignImages.Editor.Fill();
		biInteraction.ImageSmall = StiReportDesignImages.Actions.Interaction();
		biInteraction.Image = StiReportDesignImages.Actions.Interaction(StiImageSize.Double);
		biConditions.ImageSmall = StiReportDesignImages.Actions.Conditions();
		biConditions.Image = StiReportDesignImages.Actions.Conditions(StiImageSize.Double);
		biCopyStyle.ImageSmall = StiReportDesignImages.Actions.CopyStyle();
		biCopyStyle.Image = StiReportDesignImages.Actions.CopyStyle(StiImageSize.Double);
		biStyleDesigner.ImageSmall = StiReportDesignImages.Actions.RunStyleDesigner();
		biStyleDesigner.Image = StiReportDesignImages.Actions.RunStyleDesigner(StiImageSize.Double);
		biMargins.Image = StiReportDesignImages.Layout.Margins(StiImageSize.Double);
		StiResourcesHelper.SetImage(biMarginsNormal, "Stimulsoft.Report.Design.Resources.Office2013.Ribbon.Page.PageSetup.BigMargins-Normal" + stepName + ".png");
		StiResourcesHelper.SetImage(biMarginsNarrow, "Stimulsoft.Report.Design.Resources.Office2013.Ribbon.Page.PageSetup.BigMargins-Narrow" + stepName + ".png");
		StiResourcesHelper.SetImage(biMarginsWide, "Stimulsoft.Report.Design.Resources.Office2013.Ribbon.Page.PageSetup.BigMargins-Wide" + stepName + ".png");
		biOrientation.Image = StiReportDesignImages.Layout.Orientation(StiImageSize.Double);
		biOrientationPortrait.Image = StiReportDesignImages.Layout.OrientationPortrait(StiImageSize.Double);
		biOrientationLandscape.Image = StiReportDesignImages.Layout.OrientationLandscape(StiImageSize.Double);
		biPageSize.Image = StiReportDesignImages.Layout.Size(StiImageSize.Double);
		biWatermarkSettings.Image = StiReportDesignImages.Layout.Watermark(StiImageSize.Double);
		biColumns.Image = StiReportDesignImages.Layout.Columns(StiImageSize.Double);
		biColumnsOne.Image = StiReportDesignImages.Layout.ColumnsOne(StiImageSize.Double);
		biColumnsTwo.Image = StiReportDesignImages.Layout.ColumnsTwo(StiImageSize.Double);
		biColumnsThree.Image = StiReportDesignImages.Layout.ColumnsThree(StiImageSize.Double);
		biSurfaceViewDesktop.Image = StiReportDesignImages.Actions.ViewDesktop(StiImageSize.Double);
		biSurfaceViewMobile.Image = StiReportDesignImages.Actions.ViewMobile(StiImageSize.Double);
		biSurfaceViewRemoveMobileSurface.Image = StiReportDesignImages.Actions.RemoveMobileSurface(StiImageSize.Double);
		biSurfaceViewComponents.Image = StiReportDesignImages.Actions.Components(StiImageSize.Double);
		StiResourcesHelper.SetImage(biDockingPanels, "Stimulsoft.Report.Design.Resources.Office2013.Ribbon.Page.ViewOptions.Panel" + stepName + ".png");
		StiResourcesHelper.SetImage(biShowToolbox, "Stimulsoft.Report.Design.Resources.Office2013.Ribbon.Page.ViewOptions.Toolbox" + stepName + ".png");
		biViewShowGrid.Image = StiReportDesignImages.Editor.ShowGrid();
		biViewAlignToGrid.Image = StiReportDesignImages.Editor.SwitchAlignToGrid();
		biGridMode.Image = StiReportDesignImages.Editor.SwitchGridMode();
		biGridModeDotes.Image = StiReportDesignImages.Editor.SwitchGridModeToDots();
		biGridModeLines.Image = StiReportDesignImages.Editor.SwitchGridModeToLines();
		biViewShowHeaders.Image = StiReportDesignImages.Editor.ShowHeaders();
		biViewShowOrder.Image = StiReportDesignImages.Editor.ShowOrder();
		biViewShowRulers.Image = StiReportDesignImages.Editor.ShowRulers();
		biQuickInfo.Image = StiReportDesignImages.Editor.ShowQuickInfo(StiImageSize.Double);
		biAlignToGrid.Image = StiReportDesignImages.Layout.AlignToGrid(StiImageSize.Double);
		biAlign.Image = StiReportDesignImages.Layout.AlignToGrid(StiImageSize.Double);
		biAlignLeft.Image = StiReportDesignImages.Layout.ContentAlignLeft();
		biAlignCenter.Image = StiReportDesignImages.Layout.ContentAlignCenter();
		biAlignRight.Image = StiReportDesignImages.Layout.ContentAlignRight();
		biAlignTop.Image = StiReportDesignImages.Layout.ContentAlignTop();
		biAlignMiddle.Image = StiReportDesignImages.Layout.ContentAlignMiddle();
		biAlignBottom.Image = StiReportDesignImages.Layout.ContentAlignBottom();
		biMakeHorizontalSpacingEqual.Image = StiReportDesignImages.Layout.MakeHorizontalSpacingEqual();
		biMakeVerticalSpacingEqual.Image = StiReportDesignImages.Layout.MakeVerticalSpacingEqual();
		biCenterHorizontally.Image = StiReportDesignImages.Layout.CenterHorizontally();
		biCenterVertically.Image = StiReportDesignImages.Layout.CenterVertically();
		biBringToFront.Image = StiReportDesignImages.Layout.BringToFront(StiImageSize.Double);
		biSendToBack.Image = StiReportDesignImages.Layout.SendToBack(StiImageSize.Double);
		biMoveBackward.Image = StiReportDesignImages.Layout.MoveBackward(StiImageSize.Double);
		biMoveForward.Image = StiReportDesignImages.Layout.MoveForward(StiImageSize.Double);
		biSize.Image = StiReportDesignImages.Layout.MakeSameSizeAs(StiImageSize.Double);
		biMakeSameSize.Image = StiReportDesignImages.Layout.MakeSameSizeAs();
		biMakeSameWidth.Image = StiReportDesignImages.Layout.MakeSameWidthAs();
		biMakeSameHeight.Image = StiReportDesignImages.Layout.MakeSameHeightAs();
		biLock.Image = StiReportDesignImages.Layout.Lock();
		biLink.Image = StiReportDesignImages.Layout.Link();
	}

	private void InitDesignerSpecification()
	{
		if (StiAccountSettings.CurrentAccount.User != null)
		{
			if (StiAccountSettings.CurrentAccount.User.DesignerSpecification == StiDesignerSpecification.Auto)
			{
				StiActions.MainMenu.InvokeSkillLevel(Designer);
			}
			else
			{
				StiOptions.Designer.DesignerSpecification = StiAccountSettings.CurrentAccount.User.DesignerSpecification;
			}
		}
	}

	private void InitTrialDaysLeft()
	{
		IStiAccountUser stiAccountUser = StiAccountSettings.CurrentAccount?.User;
		if (stiAccountUser != null && stiAccountUser.Days > 0 && !stiAccountUser.Expired)
		{
			int num = stiAccountUser.Days - (int)(DateTime.Now - stiAccountUser.Created).TotalDays;
			if (num > 0 && (StiLicense.LicenseKey == null || !StiLicense.LicenseKey.Products.Any()))
			{
				StiActions.MainMenu.InvokeTrialDaysLeft(Designer);
			}
		}
	}

	private void InitCurrencySymbol()
	{
		biCurrencySymbol.SubItems.Clear();
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		bool flag = Thread.CurrentThread.CurrentUICulture.Name == "ja-JP";
		list.Add('$'.ToString());
		list.Add('£'.ToString());
		list.Add('€'.ToString());
		list.Add('¥'.ToString());
		list.Add('₣'.ToString());
		list2.Add("English U.S.");
		list2.Add("English U.K.");
		list2.Add("Euro");
		list2.Add(flag ? "Japanese" : "Chinese");
		list2.Add("French(Switzerland)");
		bool flag2 = false;
		string currencySymbol = CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol;
		foreach (string item in list)
		{
			if (item == currencySymbol)
			{
				flag2 = true;
				break;
			}
		}
		if (!flag2)
		{
			list.Insert(0, currencySymbol);
			list2.Insert(0, CultureInfo.CurrentCulture.NativeName);
		}
		StiCurrencyFormatService stiCurrencyFormatService = ((Designer.Report != null) ? Designer.Report.GetCurrentPage().GetTextFormat() : null) as StiCurrencyFormatService;
		int num = 0;
		foreach (string item2 in list)
		{
			ButtonItem buttonItem = new ButtonItem
			{
				Text = item2 + " " + list2[num],
				Tag = item2,
				CanCustomize = false,
				Checked = (stiCurrencyFormatService != null && stiCurrencyFormatService.Symbol == item2)
			};
			buttonItem.Click += OnCurrencySymbol_Click;
			biCurrencySymbol.SubItems.Add(buttonItem);
			num++;
		}
	}

	private void InitDateTimeFormats()
	{
		biDateTimeFormat.SubItems.Clear();
		string[] array = new string[12]
		{
			StiLocalization.Get("Formats", "date01"),
			StiLocalization.Get("Formats", "date02"),
			StiLocalization.Get("Formats", "date03"),
			StiLocalization.Get("Formats", "date04"),
			StiLocalization.Get("Formats", "date05"),
			StiLocalization.Get("Formats", "date06"),
			StiLocalization.Get("Formats", "date07"),
			StiLocalization.Get("Formats", "date08"),
			StiLocalization.Get("Formats", "date09"),
			StiLocalization.Get("Formats", "date10"),
			StiLocalization.Get("Formats", "date11"),
			StiLocalization.Get("Formats", "date12")
		};
		StiDateFormatService stiDateFormatService = ((Designer.Report != null) ? Designer.Report.GetCurrentPage().GetTextFormat() : null) as StiDateFormatService;
		StiDateFormatService stiDateFormatService2 = new StiDateFormatService();
		string[] array2 = array;
		foreach (string text in array2)
		{
			ButtonItem buttonItem = new ButtonItem();
			stiDateFormatService2.StringFormat = text;
			buttonItem.Text = stiDateFormatService2.Format(DateTime.Now);
			buttonItem.Tag = text;
			buttonItem.Click += OnDateFormat_Click;
			buttonItem.CanCustomize = false;
			buttonItem.Checked = stiDateFormatService != null && stiDateFormatService.StringFormat == text;
			biDateTimeFormat.SubItems.Add(buttonItem);
		}
		string[] array3 = new string[5]
		{
			StiLocalization.Get("Formats", "time01"),
			StiLocalization.Get("Formats", "time02"),
			StiLocalization.Get("Formats", "time03"),
			StiLocalization.Get("Formats", "time04"),
			StiLocalization.Get("Formats", "time06")
		};
		StiTimeFormatService stiTimeFormatService = ((Designer.Report != null) ? Designer.Report.GetCurrentPage().GetTextFormat() : null) as StiTimeFormatService;
		StiTimeFormatService stiTimeFormatService2 = new StiTimeFormatService();
		int num = 0;
		string[] array4 = array3;
		for (int j = 0; j < array4.Length; j++)
		{
			string text3 = (stiTimeFormatService2.StringFormat = array4[j]);
			ButtonItem buttonItem2 = new ButtonItem
			{
				BeginGroup = (num == 0),
				Text = stiTimeFormatService2.Format(DateTime.Now),
				Tag = text3,
				CanCustomize = false,
				Checked = (stiTimeFormatService != null && stiTimeFormatService.StringFormat == text3)
			};
			buttonItem2.Click += OnTimeFormat_Click;
			biDateTimeFormat.SubItems.Add(buttonItem2);
			num++;
		}
	}

	private void InitBorderStyle()
	{
		biBorderStyle = new StiPenStyleItem();
		biBorderStyle.PopupOpen += biBorderStyle_PopupOpen;
		tbBorderStyleSolid = new StiPenStyleItem();
		tbBorderStyleDash = new StiPenStyleItem();
		tbBorderStyleDashDot = new StiPenStyleItem();
		tbBorderStyleDashDotDot = new StiPenStyleItem();
		tbBorderStyleDot = new StiPenStyleItem();
		tbBorderStyleDouble = new StiPenStyleItem();
		tbBorderStyleNone = new StiPenStyleItem();
		tbBorderStyleSolid.SelectedStyle = StiPenStyle.Solid;
		tbBorderStyleDash.SelectedStyle = StiPenStyle.Dash;
		tbBorderStyleDashDot.SelectedStyle = StiPenStyle.DashDot;
		tbBorderStyleDashDotDot.SelectedStyle = StiPenStyle.DashDotDot;
		tbBorderStyleDot.SelectedStyle = StiPenStyle.Dot;
		tbBorderStyleDouble.SelectedStyle = StiPenStyle.Double;
		tbBorderStyleNone.SelectedStyle = StiPenStyle.None;
		biBorderStyle.AutoExpandOnClick = true;
		biBorderStyle.FixedSize = new Size(StiScale.XXI(48.0), StiScale.YYI(22.0));
		biBorderStyle.ImageListSizeSelection = eButtonImageListSelection.NotSet;
		biBorderStyle.ImagePaddingHorizontal = 8;
		biBorderStyle.Name = "biBorderStyle";
		biBorderStyle.CanCustomize = false;
		biBorderStyle.KeyTips = "BS";
		tbBorderStyleSolid.CanCustomize = false;
		tbBorderStyleSolid.FixedSize = new Size((int)Math.Ceiling(150.0 * StiScale.X), (int)Math.Ceiling(22.0 * StiScale.Y));
		tbBorderStyleSolid.ImageListSizeSelection = eButtonImageListSelection.NotSet;
		tbBorderStyleSolid.ImagePaddingHorizontal = 8;
		tbBorderStyleSolid.Name = "tbBorderStyleSolid";
		tbBorderStyleSolid.Text = "Solid";
		tbBorderStyleSolid.CanCustomize = false;
		tbBorderStyleSolid.Click += tbBorderStyleSolid_Click;
		tbBorderStyleSolid.KeyTips = "1";
		tbBorderStyleDash.CanCustomize = false;
		tbBorderStyleDash.FixedSize = new Size((int)Math.Ceiling(150.0 * StiScale.X), (int)Math.Ceiling(22.0 * StiScale.Y));
		tbBorderStyleDash.ImageListSizeSelection = eButtonImageListSelection.NotSet;
		tbBorderStyleDash.ImagePaddingHorizontal = 8;
		tbBorderStyleDash.Name = "tbBorderStyleDash";
		tbBorderStyleDash.Text = "Dash";
		tbBorderStyleDash.CanCustomize = false;
		tbBorderStyleDash.Click += tbBorderStyleDash_Click;
		tbBorderStyleDash.KeyTips = "2";
		tbBorderStyleDashDot.CanCustomize = false;
		tbBorderStyleDashDot.FixedSize = new Size((int)Math.Ceiling(150.0 * StiScale.X), (int)Math.Ceiling(22.0 * StiScale.Y));
		tbBorderStyleDashDot.ImageListSizeSelection = eButtonImageListSelection.NotSet;
		tbBorderStyleDashDot.ImagePaddingHorizontal = 8;
		tbBorderStyleDashDot.Name = "tbBorderStyleDashDot";
		tbBorderStyleDashDot.Text = "Dash Dot";
		tbBorderStyleDashDot.CanCustomize = false;
		tbBorderStyleDashDot.Click += tbBorderStyleDashDot_Click;
		tbBorderStyleDashDot.KeyTips = "3";
		tbBorderStyleDashDotDot.CanCustomize = false;
		tbBorderStyleDashDotDot.FixedSize = new Size((int)Math.Ceiling(150.0 * StiScale.X), (int)Math.Ceiling(22.0 * StiScale.Y));
		tbBorderStyleDashDotDot.ImageListSizeSelection = eButtonImageListSelection.NotSet;
		tbBorderStyleDashDotDot.ImagePaddingHorizontal = 8;
		tbBorderStyleDashDotDot.Name = "tbBorderStyleDashDotDot";
		tbBorderStyleDashDotDot.Text = "Dash Dot Dot";
		tbBorderStyleDashDotDot.CanCustomize = false;
		tbBorderStyleDashDotDot.Click += tbBorderStyleDashDotDot_Click;
		tbBorderStyleDashDotDot.KeyTips = "4";
		tbBorderStyleDot.CanCustomize = false;
		tbBorderStyleDot.FixedSize = new Size((int)Math.Ceiling(150.0 * StiScale.X), (int)Math.Ceiling(22.0 * StiScale.Y));
		tbBorderStyleDot.ImageListSizeSelection = eButtonImageListSelection.NotSet;
		tbBorderStyleDot.ImagePaddingHorizontal = 8;
		tbBorderStyleDot.Name = "tbBorderStyleDot";
		tbBorderStyleDot.Text = "Dot";
		tbBorderStyleDot.CanCustomize = false;
		tbBorderStyleDot.Click += tbBorderStyleDot_Click;
		tbBorderStyleDot.KeyTips = "5";
		tbBorderStyleDouble.CanCustomize = false;
		tbBorderStyleDouble.FixedSize = new Size((int)Math.Ceiling(150.0 * StiScale.X), (int)Math.Ceiling(22.0 * StiScale.Y));
		tbBorderStyleDouble.ImageListSizeSelection = eButtonImageListSelection.NotSet;
		tbBorderStyleDouble.ImagePaddingHorizontal = 8;
		tbBorderStyleDouble.Name = "tbBorderStyleDouble";
		tbBorderStyleDouble.Text = "Double";
		tbBorderStyleDouble.CanCustomize = false;
		tbBorderStyleDouble.Click += tbBorderStyleDouble_Click;
		tbBorderStyleDouble.KeyTips = "6";
		tbBorderStyleNone.CanCustomize = false;
		tbBorderStyleNone.FixedSize = new Size((int)Math.Ceiling(150.0 * StiScale.X), (int)Math.Ceiling(22.0 * StiScale.Y));
		tbBorderStyleNone.ImageListSizeSelection = eButtonImageListSelection.NotSet;
		tbBorderStyleNone.ImagePaddingHorizontal = 8;
		tbBorderStyleNone.Name = "tbBorderStyleNone";
		tbBorderStyleNone.Text = "None";
		tbBorderStyleNone.CanCustomize = false;
		tbBorderStyleNone.Click += tbBorderStyleNone_Click;
		tbBorderStyleNone.KeyTips = "7";
		biBorderStyle.SubItems.AddRange(new BaseItem[7] { tbBorderStyleSolid, tbBorderStyleDash, tbBorderStyleDashDot, tbBorderStyleDashDotDot, tbBorderStyleDot, tbBorderStyleDouble, tbBorderStyleNone });
		itemContainer25.SubItems.Add(biBorderStyle);
	}

	private void biBorderStyle_PopupOpen(object sender, PopupOpenEventArgs e)
	{
		tbBorderStyleSolid.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleSolid");
		tbBorderStyleDash.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleDash");
		tbBorderStyleDashDot.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleDashDot");
		tbBorderStyleDashDotDot.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleDashDotDot");
		tbBorderStyleDot.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleDot");
		tbBorderStyleDouble.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleDouble");
		tbBorderStyleNone.Text = StiLocalization.Get("PropertyEnum", "StiPenStyleNone");
		if (Designer?.Report?.GetCurrentPage()?.GetBorder() is StiBorder stiBorder)
		{
			tbBorderStyleDash.Checked = stiBorder != null && stiBorder.Style == StiPenStyle.Dash;
			tbBorderStyleDashDot.Checked = stiBorder != null && stiBorder.Style == StiPenStyle.DashDot;
			tbBorderStyleDashDotDot.Checked = stiBorder != null && stiBorder.Style == StiPenStyle.DashDotDot;
			tbBorderStyleDot.Checked = stiBorder != null && stiBorder.Style == StiPenStyle.Dot;
			tbBorderStyleDouble.Checked = stiBorder != null && stiBorder.Style == StiPenStyle.Double;
			tbBorderStyleNone.Checked = stiBorder != null && stiBorder.Style == StiPenStyle.None;
			tbBorderStyleSolid.Checked = stiBorder != null && stiBorder.Style == StiPenStyle.Solid;
		}
		if (Designer?.Report?.GetCurrentPage()?.GetBorder() is StiSimpleBorder stiSimpleBorder)
		{
			tbBorderStyleDash.Checked = stiSimpleBorder != null && stiSimpleBorder.Style == StiPenStyle.Dash;
			tbBorderStyleDashDot.Checked = stiSimpleBorder != null && stiSimpleBorder.Style == StiPenStyle.DashDot;
			tbBorderStyleDashDotDot.Checked = stiSimpleBorder != null && stiSimpleBorder.Style == StiPenStyle.DashDotDot;
			tbBorderStyleDot.Checked = stiSimpleBorder != null && stiSimpleBorder.Style == StiPenStyle.Dot;
			tbBorderStyleDouble.Checked = stiSimpleBorder != null && stiSimpleBorder.Style == StiPenStyle.Double;
			tbBorderStyleNone.Checked = stiSimpleBorder != null && stiSimpleBorder.Style == StiPenStyle.None;
			tbBorderStyleSolid.Checked = stiSimpleBorder != null && stiSimpleBorder.Style == StiPenStyle.Solid;
		}
	}

	internal void SelectLanguage(string localizationFile)
	{
		StiLocalizationHelper.SetLocalization(Designer, localizationFile);
		if (ribbonControl.Items.Count > 0)
		{
			if (ribbonControl.Items[0] is RibbonTabItem ribbonTabItem)
			{
				ribbonTabItem.Select();
			}
			else if (ribbonControl.Items.Count > 1 && ribbonControl.Items[1] is RibbonTabItem ribbonTabItem2)
			{
				ribbonTabItem2.Select();
			}
		}
	}

	private void BuildRightDockingPanels()
	{
		biDockingPanels.SubItems.Clear();
		StiServiceContainer services = Designer.Services.GetServices(typeof(StiPanelService));
		if (services.Count <= 0)
		{
			return;
		}
		foreach (StiPanelService item in services)
		{
			Image image = (Image)(object)StiServiceBitmapAttribute.GetImage(item.GetType());
			if (item is StiDictionaryPanelService)
			{
				image = StiReportDesignImages.Actions.Dictionary();
			}
			if (item is StiPropertiesPanelService)
			{
				image = StiReportDesignImages.Actions.Properties();
			}
			if (item is StiReportTreePanelService)
			{
				image = StiReportDesignImages.Actions.ReportTree();
			}
			ButtonItem buttonItem = null;
			if (item is StiDictionaryPanelService)
			{
				itemShowDictionary = new ButtonItem();
				buttonItem = itemShowDictionary;
			}
			else if (item is StiPropertiesPanelService)
			{
				itemShowProperties = new ButtonItem();
				buttonItem = itemShowProperties;
			}
			else if (item is StiReportTreePanelService)
			{
				itemShowReportTree = new ButtonItem();
				buttonItem = itemShowReportTree;
			}
			else
			{
				buttonItem = new ButtonItem();
			}
			buttonItem.CanCustomize = false;
			buttonItem.Text = item.ServiceName;
			buttonItem.Tag = item;
			buttonItem.Image = image;
			buttonItem.Checked = item.DockingItem.Visible;
			buttonItem.AutoCheckOnClick = true;
			buttonItem.AutoCollapseOnClick = false;
			item.DockingItem.VisibleChanged += OnPanelVisibleChanged;
			if (item.Shortcut != 0)
			{
				buttonItem.Shortcuts.Add(item.Shortcut);
			}
			buttonItem.Click += OnPanelClick;
			biDockingPanels.SubItems.Add(buttonItem);
		}
	}

	public virtual void RefresgAccountPanel()
	{
		biPublish.Enabled = StiActions.Account.States.GetPublishEnabled(Designer);
		biReportPreview.Enabled = StiActions.Account.States.GetReportPreviewEnabled(Designer);
	}

	public virtual void RefreshMainMenu()
	{
		biNew.Enabled = StiActions.MainMenu.States.GetNewEnabled();
		biReportOpen.Enabled = StiActions.MainMenu.States.GetReportOpenEnabled(Designer);
		biReportSave.Enabled = StiActions.MainMenu.States.GetReportSaveEnabled(Designer);
		biSaveAs.Enabled = StiActions.MainMenu.States.GetSaveAsEnabled(Designer);
		biCheckIssues.Enabled = StiActions.MainMenu.States.GetCheckIssuesEnabled(Designer);
		biReportSetup.Enabled = StiActions.MainMenu.States.GetReportSetupEnabled(Designer);
		biOptions.Enabled = StiActions.MainMenu.States.GetOptionsEnabled(Designer);
		ButtonItem buttonItem = biRecentFile01;
		ButtonItem buttonItem2 = biRecentFile02;
		ButtonItem buttonItem3 = biRecentFile03;
		ButtonItem buttonItem4 = biRecentFile04;
		ButtonItem buttonItem5 = biRecentFile05;
		ButtonItem buttonItem6 = biRecentFile06;
		ButtonItem buttonItem7 = biRecentFile07;
		ButtonItem buttonItem8 = biRecentFile08;
		bool flag = (biRecentFile09.Enabled = StiActions.MainMenu.States.GetRecentFilesEnabled(Designer));
		bool flag3 = (buttonItem8.Enabled = flag);
		bool flag5 = (buttonItem7.Enabled = flag3);
		bool flag7 = (buttonItem6.Enabled = flag5);
		bool flag9 = (buttonItem5.Enabled = flag7);
		bool flag11 = (buttonItem4.Enabled = flag9);
		bool flag13 = (buttonItem3.Enabled = flag11);
		bool enabled = (buttonItem2.Enabled = flag13);
		buttonItem.Enabled = enabled;
		biClose.Enabled = StiActions.MainMenu.States.GetCloseEnabled(Designer);
	}

	public virtual void RefreshReportFile()
	{
		if (!StiOptions.Designer.AllowMultiColorTitle || StiOptions.Designer.ShowFullFileNameInTitle || StiOptions.Designer.DesignerTitleMask != "{0} - {1}")
		{
			ribbonControl.TitleText = StiActions.MainMenu.States.GetReportFile(Designer);
			if (((Control)ribbonControl).FindForm() != null)
			{
				((Control)((Control)ribbonControl).FindForm()).Text = ribbonControl.TitleText;
			}
			return;
		}
		if (StiOptions.Designer.DesignerTitleText != null)
		{
			ribbonControl.TitleText = StiOptions.Designer.DesignerTitleText;
			if (((Control)ribbonControl).FindForm() != null)
			{
				((Control)((Control)ribbonControl).FindForm()).Text = ribbonControl.TitleText;
			}
			return;
		}
		string text = Designer.ReportFileName;
		if (text.IndexOf("\\", StringComparison.InvariantCulture) != -1)
		{
			text = text.Substring(text.LastIndexOf("\\", StringComparison.InvariantCulture) + 1);
		}
		if (!string.IsNullOrEmpty(text))
		{
			string text2 = "#222222";
			string text3 = "#3E6AAA";
			ribbonControl.TitleText = "<font color=\"" + text2 + "\">" + text + "</font> - <font color=\"" + text3 + "\">" + StiOptions.Designer.GetDesignerTitle() + "</font>";
			if (((Control)ribbonControl).FindForm() != null)
			{
				((Control)((Control)ribbonControl).FindForm()).Text = ribbonControl.TitleText;
			}
		}
		else
		{
			ribbonControl.TitleText = StiOptions.Designer.GetDesignerTitle();
			if (((Control)ribbonControl).FindForm() != null)
			{
				((Control)((Control)ribbonControl).FindForm()).Text = ribbonControl.TitleText;
			}
		}
	}

	public virtual void RefreshQuickBar()
	{
		biQuickUndo.Enabled = StiActions.Clipboard.States.CanUndo(Designer) && StiActions.Clipboard.States.GetUndoRedoEnabled(Designer);
		biQuickRedo.Enabled = StiActions.Clipboard.States.CanRedo(Designer) && StiActions.Clipboard.States.GetUndoRedoEnabled(Designer);
		biQuickUndo.Tooltip = StiActions.Clipboard.States.GetUndoText(Designer);
		biQuickRedo.Tooltip = StiActions.Clipboard.States.GetRedoText(Designer);
		biQuickReportSave.Enabled = StiActions.MainMenu.States.GetReportSaveEnabled(Designer);
	}

	public virtual void RefreshRecentFiles()
	{
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
			string fileName = Designer.RecentFiles[i].FileName;
			ButtonItem buttonItem = null;
			if (i == 0)
			{
				buttonItem = biRecentFile01;
			}
			if (i == 1)
			{
				buttonItem = biRecentFile02;
			}
			if (i == 2)
			{
				buttonItem = biRecentFile03;
			}
			if (i == 3)
			{
				buttonItem = biRecentFile04;
			}
			if (i == 4)
			{
				buttonItem = biRecentFile05;
			}
			if (i == 5)
			{
				buttonItem = biRecentFile06;
			}
			if (i == 6)
			{
				buttonItem = biRecentFile07;
			}
			if (i == 7)
			{
				buttonItem = biRecentFile08;
			}
			if (i == 8)
			{
				buttonItem = biRecentFile09;
			}
			StiDisplayRecentFileObjectEventArgs stiDisplayRecentFileObjectEventArgs = StiOptions.Engine.GlobalEvents.InvokeDisplayRecentFileInDesigner(Designer, Designer.RecentFiles[i]);
			if (i < 9)
			{
				buttonItem.Text = $"&{i + 1}. {stiDisplayRecentFileObjectEventArgs.DisplayName}";
			}
			else
			{
				buttonItem.Text = stiDisplayRecentFileObjectEventArgs.DisplayName;
			}
			buttonItem.Visible = stiDisplayRecentFileObjectEventArgs.Visible;
			buttonItem.Tooltip = fileName;
			buttonItem.Tag = Designer.RecentFiles[i];
		}
		for (int j = num; j < 9; j++)
		{
			ButtonItem buttonItem2 = null;
			if (j == 0)
			{
				buttonItem2 = biRecentFile01;
			}
			if (j == 1)
			{
				buttonItem2 = biRecentFile02;
			}
			if (j == 2)
			{
				buttonItem2 = biRecentFile03;
			}
			if (j == 3)
			{
				buttonItem2 = biRecentFile04;
			}
			if (j == 4)
			{
				buttonItem2 = biRecentFile05;
			}
			if (j == 5)
			{
				buttonItem2 = biRecentFile06;
			}
			if (j == 6)
			{
				buttonItem2 = biRecentFile07;
			}
			if (j == 7)
			{
				buttonItem2 = biRecentFile08;
			}
			if (j == 8)
			{
				buttonItem2 = biRecentFile09;
			}
			buttonItem2.Visible = false;
		}
	}

	public virtual void RefreshClipboard()
	{
		biCut.Enabled = StiActions.Clipboard.States.CanCut(Designer) && StiActions.Clipboard.States.GetClipboardEnabled(Designer);
		biCopy.Enabled = StiActions.Clipboard.States.CanCopy(Designer) && StiActions.Clipboard.States.GetClipboardEnabled(Designer);
		biPaste.Enabled = StiActions.Clipboard.States.CanPaste(Designer) && StiActions.Clipboard.States.GetClipboardEnabled(Designer);
		biDelete.Enabled = StiActions.Clipboard.States.CanDelete(Designer) && StiActions.Clipboard.States.GetClipboardEnabled(Designer);
	}

	public virtual void RefreshFont()
	{
		object fontObject = ((Designer.Report != null) ? Designer.Report.GetCurrentPage().GetFont() : null);
		StiBrush textBrush = ((Designer.Report != null) ? (Designer.Report.GetCurrentPage().GetTextBrush() as StiBrush) : null);
		cbFont.Enabled = StiActions.Font.States.GetFontNameEnabled(Designer, fontObject);
		cbFontSize.Enabled = StiActions.Font.States.GetFontSizeEnabled(Designer, fontObject);
		biFontBold.Enabled = StiActions.Font.States.GetFontBoldEnabled(Designer, fontObject);
		biFontItalic.Enabled = StiActions.Font.States.GetFontItalicEnabled(Designer, fontObject);
		biFontStrikeout.Enabled = StiActions.Font.States.GetFontStrikeoutEnabled(Designer, fontObject);
		biFontUnderline.Enabled = StiActions.Font.States.GetFontUnderlineEnabled(Designer, fontObject);
		biFontGrow.Enabled = StiActions.Font.States.GetFontGrowEnabled(Designer, fontObject);
		biFontShrink.Enabled = StiActions.Font.States.GetFontShrinkEnabled(Designer, fontObject);
		biClearContents.Enabled = StiActions.Font.States.GetClearContentsEnabled(Designer, fontObject);
		biTextBrush.Enabled = StiActions.Font.States.GetTextBrushEnabled(Designer, textBrush);
		if (Designer.Report == null)
		{
			return;
		}
		cbFont.Text = StiActions.Font.States.GetFontName(fontObject);
		cbFontSize.Text = StiActions.Font.States.GetFontSize(fontObject).ToString();
		biFontBold.Checked = StiActions.Font.States.GetFontBoldChecked(Designer, fontObject);
		biFontItalic.Checked = StiActions.Font.States.GetFontItalicChecked(Designer, fontObject);
		biFontUnderline.Checked = StiActions.Font.States.GetFontUnderlineChecked(Designer, fontObject);
		biFontStrikeout.Checked = StiActions.Font.States.GetFontStrikeoutChecked(Designer, fontObject);
		biTextBrush.SelectedColor = StiActions.Font.States.GetTextBrush(Designer, textBrush);
		biTextBrush.IsFromStyle = designer.Report.GetCurrentPage().IsDashboard;
		string text = ((cbFont.SelectedItem != null) ? ((ComboItem)cbFont.SelectedItem).FontName : string.Empty);
		while (cbFont.Items.Count > 0 && ((ComboItem)cbFont.Items[0]).IsResourceFontItem)
		{
			cbFont.Items.RemoveAt(0);
		}
		ComboItem comboItem = null;
		List<StiResource> allFonts = StiFontResourceHelper.GetAllFonts(Designer.Report);
		foreach (StiResource item in allFonts.OrderByDescending((StiResource f) => f.Name))
		{
			string resourceFontName = Designer.Report.GetResourceFontName(item.Name);
			ComboItem comboItem2 = new ComboItem
			{
				IsResourceFontItem = true,
				FontName = resourceFontName,
				Text = resourceFontName
			};
			if (text.Equals(resourceFontName))
			{
				comboItem = comboItem2;
			}
			cbFont.Items.Insert(0, (object)comboItem2);
		}
		if (comboItem != null)
		{
			cbFont.SelectedItem = comboItem;
		}
	}

	public virtual void RefreshContentAlignment()
	{
		object textHorizontalAlignmentObj = ((Designer.Report != null) ? Designer.Report.GetCurrentPage().GetTextHorAlignment() : null);
		object horizontalAlignmentObj = ((Designer.Report != null) ? Designer.Report.GetCurrentPage().GetHorAlignment() : null);
		object crossHorizontalAlignmentObj = ((Designer.Report != null) ? Designer.Report.GetCurrentPage().GetCrossHorAlignment() : null);
		object verticalAlignmentObj = ((Designer.Report != null) ? Designer.Report.GetCurrentPage().GetVertAlignment() : null);
		biContentAlignLeft.Enabled = StiActions.ContentAlignment.States.GetAlignLeftEnabled(Designer, textHorizontalAlignmentObj, horizontalAlignmentObj, crossHorizontalAlignmentObj);
		biContentAlignCenter.Enabled = StiActions.ContentAlignment.States.GetAlignCenterEnabled(Designer, textHorizontalAlignmentObj, horizontalAlignmentObj, crossHorizontalAlignmentObj);
		biContentAlignRight.Enabled = StiActions.ContentAlignment.States.GetAlignRightEnabled(Designer, textHorizontalAlignmentObj, horizontalAlignmentObj, crossHorizontalAlignmentObj);
		biContentAlignWidth.Enabled = StiActions.ContentAlignment.States.GetAlignWidthEnabled(Designer, textHorizontalAlignmentObj, crossHorizontalAlignmentObj);
		biContentAlignTop.Enabled = StiActions.ContentAlignment.States.GetAlignTopEnabled(Designer, verticalAlignmentObj);
		biContentAlignMiddle.Enabled = StiActions.ContentAlignment.States.GetAlignMiddleEnabled(Designer, verticalAlignmentObj);
		biContentAlignBottom.Enabled = StiActions.ContentAlignment.States.GetAlignBottomEnabled(Designer, verticalAlignmentObj);
		biRotateText.Enabled = StiActions.ContentAlignment.States.GetRotateTextEnabled(Designer);
		biLineSpacing.Enabled = StiActions.ContentAlignment.States.GetLineSpacingEnabled(Designer);
		biWordWrap.Enabled = StiActions.ContentAlignment.States.GetWordWrapEnabled(Designer);
		if (Designer.Report != null)
		{
			biContentAlignLeft.Checked = StiActions.ContentAlignment.States.GetAlignLeftChecked(Designer, textHorizontalAlignmentObj, horizontalAlignmentObj, crossHorizontalAlignmentObj);
			biContentAlignCenter.Checked = StiActions.ContentAlignment.States.GetAlignCenterChecked(Designer, textHorizontalAlignmentObj, horizontalAlignmentObj, crossHorizontalAlignmentObj);
			biContentAlignRight.Checked = StiActions.ContentAlignment.States.GetAlignRightChecked(Designer, textHorizontalAlignmentObj, horizontalAlignmentObj, crossHorizontalAlignmentObj);
			biContentAlignWidth.Checked = StiActions.ContentAlignment.States.GetAlignWidthChecked(Designer, textHorizontalAlignmentObj, crossHorizontalAlignmentObj);
			biContentAlignTop.Checked = StiActions.ContentAlignment.States.GetAlignTopChecked(Designer, verticalAlignmentObj);
			biContentAlignMiddle.Checked = StiActions.ContentAlignment.States.GetAlignMiddleChecked(Designer, verticalAlignmentObj);
			biContentAlignBottom.Checked = StiActions.ContentAlignment.States.GetAlignBottomChecked(Designer, verticalAlignmentObj);
			biWordWrap.Checked = StiActions.ContentAlignment.States.GetWordWrapChecked(Designer);
		}
	}

	public virtual void RefreshBorders()
	{
		object obj = ((Designer.Report != null) ? Designer.Report.GetCurrentPage().GetBorder() : null);
		object obj2 = ((Designer.Report != null) ? Designer.Report.GetCurrentPage().GetSimpleShadow() : null);
		StiBrush brush = ((Designer.Report != null) ? (Designer.Report.GetCurrentPage().GetBrush() as StiBrush) : null);
		object obj3 = ((Designer.Report != null) ? Designer.Report.GetCurrentPage().GetBorderColor() : null);
		biBorderAll.Enabled = StiActions.Borders.States.GetBordersAllEnabled(Designer, obj);
		biBorderNone.Enabled = StiActions.Borders.States.GetBordersNoneEnabled(Designer, obj);
		biBorderLeft.Enabled = StiActions.Borders.States.GetBordersLeftEnabled(Designer, obj);
		biBorderRight.Enabled = StiActions.Borders.States.GetBordersRightEnabled(Designer, obj);
		biBorderTop.Enabled = StiActions.Borders.States.GetBordersTopEnabled(Designer, obj);
		biBorderBottom.Enabled = StiActions.Borders.States.GetBordersBottomEnabled(Designer, obj);
		biBorderShadow.Enabled = StiActions.Borders.States.GetBordersShadowEnabled(Designer, obj2 ?? obj);
		biBorderColor.Enabled = StiActions.Borders.States.GetBordersColorEnabled(Designer, obj ?? obj3);
		biBorderStyle.Enabled = StiActions.Borders.States.GetBordersStyleEnabled(Designer, obj);
		biFillBrush.Enabled = StiActions.Borders.States.GetFillBrushEnabled(Designer, brush);
		ribbonBarBorders.DialogLauncherVisible = obj != null;
		if (Designer.Report != null)
		{
			biBorderAll.Checked = StiActions.Borders.States.GetBordersAllChecked(Designer, obj);
			biBorderNone.Checked = StiActions.Borders.States.GetBordersNoneChecked(Designer, obj);
			biBorderLeft.Checked = StiActions.Borders.States.GetBordersLeftChecked(Designer, obj);
			biBorderRight.Checked = StiActions.Borders.States.GetBordersRightChecked(Designer, obj);
			biBorderTop.Checked = StiActions.Borders.States.GetBordersTopChecked(Designer, obj);
			biBorderBottom.Checked = StiActions.Borders.States.GetBordersBottomChecked(Designer, obj);
			biBorderShadow.Checked = StiActions.Borders.States.GetBordersShadowChecked(Designer, obj2 ?? obj);
			biBorderColor.SelectedColor = StiActions.Borders.States.GetBordersColor(Designer, obj);
			biBorderStyle.SelectedStyle = StiActions.Borders.States.GetBordersStyle(Designer, obj);
			biFillBrush.SelectedColor = StiActions.Borders.States.GetFillBrushColor(Designer, brush);
			biFillBrush.IsFromStyle = designer.Report.GetCurrentPage().IsDashboard;
			ribbonBarBorders.RecalcLayout();
		}
	}

	public virtual void RefreshTextFormat()
	{
		StiFormatService service = ((Designer.Report != null) ? Designer.Report.GetCurrentPage().GetTextFormat() : null);
		string textFormatString = StiActions.TextFormat.States.GetTextFormatString(Designer, service);
		if (biTextFormat.Text != textFormatString)
		{
			biTextFormat.Text = textFormatString;
			UpdateSize(ribbonBarTextFormat);
			((Control)ribbonBarTextFormat).Invalidate();
		}
		biCurrencySymbol.Enabled = StiActions.TextFormat.States.GetCurrencySymbolEnabled(Designer, service);
		biDateTimeFormat.Enabled = StiActions.TextFormat.States.GetDateTimeFormatEnabled(Designer, service);
		biTextFormat.Enabled = StiActions.TextFormat.States.GetTextFormatEnabled(Designer, service);
		ribbonBarTextFormat.DialogLauncherVisible = StiActions.TextFormat.States.GetTextFormatEnabled(Designer, service);
		ribbonBarTextFormat.RecalcLayout();
	}

	private List<StiCrossTabStyle> GetCrossTabStyles()
	{
		List<StiCrossTabStyle> list = new List<StiCrossTabStyle>();
		Color[] styleColors = StiOptions.Designer.CrossTab.StyleColors;
		foreach (Color color in styleColors)
		{
			list.Add(new StiCrossTabStyle
			{
				Color = color
			});
		}
		return list;
	}

	private StiBaseStyle GetAutoStyle(bool isGaugeElementOnly, bool isChartElementOnly, bool isMapRegionElementOnly, bool isIndicatorElement, bool isControlElement, bool isProgressElement, bool isTableElementOnly, bool isPivotElementOnly, bool isCardsElementOnly)
	{
		StiBaseStyle stiBaseStyle = null;
		IStiDashboardElementStyle stiDashboardElementStyle = designer.Report.GetCurrentPage() as IStiDashboardElementStyle;
		StiElementStyleIdent style = stiDashboardElementStyle.Style;
		if (isGaugeElementOnly)
		{
			foreach (StiGaugeStyleXF item in StiOptions.Services.GaugeStyles.Where((StiGaugeStyleXF x) => x.AllowDashboard))
			{
				if (item.StyleIdent == style)
				{
					stiBaseStyle = item;
					break;
				}
			}
			if (stiBaseStyle == null)
			{
				stiBaseStyle = StiOptions.Services.GaugeStyles.Where((StiGaugeStyleXF x) => x.AllowDashboard).FirstOrDefault();
			}
		}
		else if (isChartElementOnly)
		{
			foreach (Stimulsoft.Report.Chart.StiChartStyle item2 in StiOptions.Services.ChartStyles.Where((Stimulsoft.Report.Chart.StiChartStyle x) => x.AllowDashboard))
			{
				if (item2.StyleIdent == style)
				{
					stiBaseStyle = item2;
					break;
				}
			}
			if (stiBaseStyle == null)
			{
				stiBaseStyle = StiOptions.Services.ChartStyles.Where((Stimulsoft.Report.Chart.StiChartStyle x) => x.AllowDashboard).FirstOrDefault();
			}
		}
		else if (isMapRegionElementOnly)
		{
			foreach (StiMapStyleFX item3 in StiOptions.Services.MapStyles.Where((StiMapStyleFX x) => x.AllowDashboard))
			{
				if (item3.StyleIdent == style)
				{
					stiBaseStyle = item3;
					break;
				}
			}
			if (stiBaseStyle == null)
			{
				stiBaseStyle = StiOptions.Services.MapStyles.Where((StiMapStyleFX x) => x.AllowDashboard).FirstOrDefault();
			}
		}
		else if (isIndicatorElement)
		{
			foreach (StiIndicatorElementStyle indicatorStyle in StiOptions.Services.Dashboards.IndicatorStyles)
			{
				if (indicatorStyle.Ident == style)
				{
					stiBaseStyle = indicatorStyle;
					break;
				}
			}
			if (stiBaseStyle == null)
			{
				stiBaseStyle = StiOptions.Services.Dashboards.IndicatorStyles.FirstOrDefault();
			}
		}
		else if (isControlElement)
		{
			foreach (StiControlElementStyle controlStyle in StiOptions.Services.Dashboards.ControlStyles)
			{
				if (controlStyle.Ident == style)
				{
					stiBaseStyle = controlStyle;
					break;
				}
			}
			if (stiBaseStyle == null)
			{
				stiBaseStyle = StiOptions.Services.Dashboards.ControlStyles.FirstOrDefault();
			}
		}
		else if (isProgressElement)
		{
			foreach (StiProgressElementStyle progressStyle in StiOptions.Services.Dashboards.ProgressStyles)
			{
				if (progressStyle.Ident == style)
				{
					stiBaseStyle = progressStyle;
					break;
				}
			}
			if (stiBaseStyle == null)
			{
				stiBaseStyle = StiOptions.Services.Dashboards.ProgressStyles.FirstOrDefault();
			}
		}
		else if (isTableElementOnly)
		{
			foreach (StiTableElementStyle tableStyle in StiOptions.Services.Dashboards.TableStyles)
			{
				if (tableStyle.Ident == style)
				{
					stiBaseStyle = tableStyle;
					break;
				}
			}
			if (stiBaseStyle == null)
			{
				stiBaseStyle = StiOptions.Services.Dashboards.TableStyles.FirstOrDefault();
			}
		}
		else if (isPivotElementOnly)
		{
			foreach (StiPivotElementStyle pivotStyle in StiOptions.Services.Dashboards.PivotStyles)
			{
				if (pivotStyle.Ident == style)
				{
					stiBaseStyle = pivotStyle;
					break;
				}
			}
			if (stiBaseStyle == null)
			{
				stiBaseStyle = StiOptions.Services.Dashboards.PivotStyles.FirstOrDefault();
			}
		}
		else if (isCardsElementOnly)
		{
			foreach (StiCardsElementStyle cardsStyle in StiOptions.Services.Dashboards.CardsStyles)
			{
				if (cardsStyle.Ident == style)
				{
					stiBaseStyle = cardsStyle;
					break;
				}
			}
			if (stiBaseStyle == null)
			{
				stiBaseStyle = StiOptions.Services.Dashboards.CardsStyles.FirstOrDefault();
			}
		}
		return stiBaseStyle;
	}

	public virtual void RefreshStyles()
	{
		if (Designer.SelectedComponentsOnPage == null)
		{
			return;
		}
		biInteraction.Enabled = StiActions.Style.States.GetInteractionEnabled(Designer);
		biInteraction.ShowMarker = StiActions.Style.States.GetInteractionMarked(Designer);
		biConditions.Enabled = StiActions.Style.States.GetConditionsEnabled(Designer);
		biConditions.ShowMarker = StiActions.Style.States.GetConditionsMarked(Designer);
		biCopyStyle.Enabled = StiActions.Style.States.GetCopyStyleEnabled(Designer);
		biStyleDesigner.Enabled = StiActions.Style.States.GetStyleDesignerEnabled(Designer);
		ribbonBarStyle.DialogLauncherVisible = StiActions.Style.States.GetStyleDesignerEnabled(Designer);
		galleryStyles.Enabled = StiActions.Style.States.GetStyleDesignerEnabled(Designer);
		List<StiComponent> list = Designer.SelectedComponentsOnPage.Cast<StiComponent>().ToList();
		if (StiInplaceDesigner.IsEditorActivated && StiInplaceDesigner.EditableComponent != null)
		{
			list = new List<StiComponent> { StiInplaceDesigner.EditableComponent };
		}
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		bool flag6 = false;
		bool flag7 = true;
		bool flag8 = false;
		bool flag9 = false;
		bool flag10 = false;
		bool flag11 = false;
		bool flag12 = false;
		bool flag13 = false;
		bool flag14 = false;
		bool flag15 = false;
		bool flag16 = false;
		bool flag17 = false;
		bool flag18 = false;
		bool flag19 = false;
		bool flag20 = false;
		bool flag21 = false;
		bool flag22 = false;
		StiProgressElementMode progressMode = StiProgressElementMode.Circle;
		if (list.Count > 0)
		{
			flag = list.All((StiComponent c) => c is StiCrossTab);
			flag2 = list.All((StiComponent c) => c is StiChart);
			flag3 = list.All((StiComponent c) => c is StiGauge);
			flag4 = list.All((StiComponent c) => c is StiMap);
			flag6 = list.All((StiComponent c) => c is IStiTableComponent) && list.FirstOrDefault((StiComponent c) => c is StiTable) != null;
			flag5 = list.All((StiComponent c) => c is StiReportControl);
			flag8 = list.All((StiComponent c) => c is IStiComponentUI && c is IStiDashboardElementStyle);
			flag10 = list.All((StiComponent c) => c is IStiChartElement);
			flag11 = list.All((StiComponent c) => c is IStiGaugeElement);
			flag12 = list.All((StiComponent c) => c is IStiRegionMapElement);
			flag13 = list.All((StiComponent c) => c is IStiOnlineMapElement);
			flag14 = list.All((StiComponent c) => c is IStiTableElement);
			flag9 = list.All((StiComponent c) => c is IStiCardsElement);
			flag16 = list.All((StiComponent c) => c is IStiIndicatorElement);
			flag17 = list.All((StiComponent c) => c is IStiControlElement);
			flag18 = list.All((StiComponent c) => c is IStiProgressElement);
			flag19 = list.All((StiComponent c) => c is IStiTextElement);
			flag15 = list.All((StiComponent c) => c is IStiPivotTableElement);
			flag20 = list.All((StiComponent c) => c is StiSparkline);
			flag7 = list.All((StiComponent c) => !(c is StiCrossTab) && !(c is StiChart) && !(c is StiGauge) && !(c is StiMap) && !(c is StiReportControl) && !(c is IStiTableComponent) && !(c is IStiComponentUI) && !(c is IStiChartElement) && !(c is IStiGaugeElement) && !(c is IStiRegionMapElement) && !(c is IStiTableElement) && !(c is IStiCardsElement) && !(c is IStiPivotTableElement) && !(c is IStiControlElement) && !(c is IStiIndicatorElement) && !(c is IStiProgressElement));
		}
		else if (designer.Report != null && designer.Report.GetCurrentPage() is IStiDashboard)
		{
			flag21 = true;
		}
		else if (designer.Report != null && designer.Report.GetCurrentPage() is IStiScreen)
		{
			flag22 = true;
		}
		string text = null;
		if (flag7)
		{
			foreach (StiComponent item in list)
			{
				if (text == null)
				{
					text = item.ComponentStyle.Trim();
				}
				else if (text != item.ComponentStyle)
				{
					text = null;
					break;
				}
			}
		}
		StiGaugeType? gaugeType = null;
		bool addGaugeRange = false;
		string mapIdent = "USA";
		StiFontIcons indicatorFontIcons = StiFontIcons.Rating4;
		StiBaseStyle stiBaseStyle = null;
		bool flag23 = false;
		if (flag2)
		{
			StiChart stiChart = (StiChart)list[0];
			if (Designer.Report != null)
			{
				foreach (StiBaseStyle style in Designer.Report.Styles)
				{
					if (style is StiChartStyle && stiChart.CustomStyleName == style.Name)
					{
						StiCustomStyle stiCustomStyle = new StiCustomStyle(style.Name);
						stiCustomStyle.CustomCore.ReportChartStyle = style as StiChartStyle;
						stiBaseStyle = stiCustomStyle;
					}
				}
			}
			if (stiBaseStyle == null)
			{
				foreach (Stimulsoft.Report.Chart.StiChartStyle chartStyle in StiOptions.Services.ChartStyles)
				{
					if (stiChart.Style.Equals(chartStyle))
					{
						stiBaseStyle = chartStyle;
						break;
					}
				}
			}
			StiStyleItem.GlobalChart = StiSimpleChartHelper.GetSimpleChart(stiChart);
		}
		else if (flag6 && list[0] is StiTable stiTable)
		{
			if (Designer.Report != null && !string.IsNullOrEmpty(stiTable.ComponentStyle))
			{
				IEnumerable<StiTableStyle> enumerable = Designer.Report.Styles.Cast<StiBaseStyle>().OfType<StiTableStyle>();
				foreach (StiTableStyle item2 in enumerable)
				{
					if (item2.Name == stiTable.ComponentStyle)
					{
						stiBaseStyle = item2;
						break;
					}
				}
			}
			if (stiBaseStyle == null && stiTable.TableStyleFX != null)
			{
				foreach (StiTableStyleFX tableStyle in StiOptions.Services.TableStyles)
				{
					if (tableStyle.StyleId == stiTable.TableStyleFX.StyleId)
					{
						stiBaseStyle = tableStyle;
						break;
					}
				}
			}
		}
		else if (flag4)
		{
			StiMap stiMap = (StiMap)list[0];
			mapIdent = stiMap.MapIdent;
			if (Designer.Report != null && !string.IsNullOrEmpty(stiMap.ComponentStyle))
			{
				IEnumerable<StiMapStyle> enumerable2 = Designer.Report.Styles.Cast<StiBaseStyle>().OfType<StiMapStyle>();
				foreach (StiMapStyle item3 in enumerable2)
				{
					if (item3.Name == stiMap.ComponentStyle)
					{
						stiBaseStyle = item3;
						break;
					}
				}
			}
			if (stiBaseStyle == null)
			{
				foreach (StiMapStyleFX item4 in StiOptions.Services.MapStyles.Where((StiMapStyleFX x) => x.AllowDashboard))
				{
					if (item4.StyleId == stiMap.MapStyle)
					{
						stiBaseStyle = item4;
						break;
					}
				}
			}
		}
		else if (flag)
		{
			StiCrossTab stiCrossTab = (StiCrossTab)list[0];
			int num = 0;
			List<StiCrossTabStyle> crossTabStyles = GetCrossTabStyles();
			foreach (StiCrossTabStyle item5 in crossTabStyles)
			{
				if (num == stiCrossTab.CrossTabStyleIndex)
				{
					stiBaseStyle = item5;
					break;
				}
				num++;
			}
			if (!string.IsNullOrEmpty(stiCrossTab.CrossTabStyle))
			{
				IEnumerable<StiCrossTabStyle> enumerable3 = Designer.Report.Styles.Cast<StiBaseStyle>().OfType<StiCrossTabStyle>();
				foreach (StiCrossTabStyle item6 in enumerable3)
				{
					if (item6.Name == stiCrossTab.CrossTabStyle)
					{
						stiBaseStyle = item6;
						break;
					}
				}
			}
		}
		else if (flag3)
		{
			StiGauge stiGauge = (StiGauge)list[0];
			if (StiGaugeV2InitHelper.IsGaugeV2(stiGauge))
			{
				gaugeType = stiGauge.Type;
			}
			if (Designer.Report != null && !string.IsNullOrEmpty(stiGauge.CustomStyleName))
			{
				IEnumerable<StiGaugeStyle> enumerable4 = Designer.Report.Styles.Cast<StiBaseStyle>().OfType<StiGaugeStyle>();
				foreach (StiGaugeStyle item7 in enumerable4)
				{
					if (item7.Name == stiGauge.CustomStyleName)
					{
						stiBaseStyle = item7;
						break;
					}
				}
			}
			if (stiBaseStyle == null && (stiGauge.Style != null || stiGauge.CustomStyleName != null))
			{
				foreach (StiGaugeStyleXF gaugeStyle in StiOptions.Services.GaugeStyles)
				{
					if (!string.IsNullOrEmpty(gaugeStyle.Name) && !string.IsNullOrEmpty(stiGauge.CustomStyleName) && gaugeStyle.Name == stiGauge.CustomStyleName)
					{
						stiBaseStyle = gaugeStyle;
						break;
					}
					if (stiGauge.Style != null && gaugeStyle.GetType() == stiGauge.Style.GetType())
					{
						stiBaseStyle = gaugeStyle;
						break;
					}
				}
			}
		}
		else if (flag11)
		{
			IStiGaugeElement stiGaugeElement = (IStiGaugeElement)list[0];
			if (Designer.Report != null && !string.IsNullOrEmpty(stiGaugeElement.CustomStyleName))
			{
				IEnumerable<StiGaugeStyle> enumerable5 = Designer.Report.Styles.Cast<StiBaseStyle>().OfType<StiGaugeStyle>();
				foreach (StiGaugeStyle item8 in enumerable5)
				{
					if (item8.Name == stiGaugeElement.CustomStyleName)
					{
						stiBaseStyle = item8;
						break;
					}
				}
			}
			gaugeType = stiGaugeElement.Type;
			addGaugeRange = stiGaugeElement.RangeType == StiGaugeRangeType.Color;
			if (stiBaseStyle == null)
			{
				if (stiGaugeElement.Style == StiElementStyleIdent.Auto)
				{
					flag23 = true;
				}
				else
				{
					foreach (StiGaugeStyleXF item9 in StiOptions.Services.GaugeStyles.Where((StiGaugeStyleXF x) => x.AllowDashboard))
					{
						if (item9.StyleIdent == stiGaugeElement.Style)
						{
							stiBaseStyle = item9;
							break;
						}
					}
				}
			}
		}
		else if (flag10)
		{
			IStiChartElement stiChartElement = (IStiChartElement)list[0];
			if (Designer.Report != null && !string.IsNullOrEmpty(stiChartElement.CustomStyleName))
			{
				IEnumerable<StiChartStyle> enumerable6 = Designer.Report.Styles.Cast<StiBaseStyle>().OfType<StiChartStyle>();
				foreach (StiChartStyle item10 in enumerable6)
				{
					if (item10.Name == stiChartElement.CustomStyleName)
					{
						stiBaseStyle = item10;
						break;
					}
				}
			}
			if (stiBaseStyle == null)
			{
				if (stiChartElement.Style == StiElementStyleIdent.Auto)
				{
					flag23 = true;
				}
				else
				{
					IEnumerable<Stimulsoft.Report.Chart.StiChartStyle> enumerable7 = from x in StiOptions.Services.ChartStyles
						orderby x.GetType() != typeof(StiStyle29)
						where x.AllowDashboard
						select x;
					foreach (Stimulsoft.Report.Chart.StiChartStyle item11 in enumerable7)
					{
						if (item11.StyleIdent == stiChartElement.Style)
						{
							stiBaseStyle = item11;
							break;
						}
					}
				}
			}
			StiStyleItem.GlobalChart = StiSimpleChartHelper.GetSimpleChart(stiChartElement);
		}
		else if (flag12)
		{
			IStiRegionMapElement stiRegionMapElement = (IStiRegionMapElement)list[0];
			if (Designer.Report != null && !string.IsNullOrEmpty(stiRegionMapElement.CustomStyleName))
			{
				IEnumerable<StiMapStyle> enumerable8 = Designer.Report.Styles.Cast<StiBaseStyle>().OfType<StiMapStyle>();
				foreach (StiMapStyle item12 in enumerable8)
				{
					if (item12.Name == stiRegionMapElement.CustomStyleName)
					{
						stiBaseStyle = item12;
						break;
					}
				}
			}
			mapIdent = stiRegionMapElement.MapIdent;
			if (stiBaseStyle == null)
			{
				if (stiRegionMapElement.Style == StiElementStyleIdent.Auto)
				{
					flag23 = true;
				}
				else
				{
					foreach (StiMapStyleFX item13 in StiOptions.Services.MapStyles.Where((StiMapStyleFX x) => x.AllowDashboard))
					{
						if (item13.StyleIdent == stiRegionMapElement.Style)
						{
							stiBaseStyle = item13;
							break;
						}
					}
				}
			}
		}
		else if (!flag13)
		{
			if (flag16)
			{
				IStiIndicatorElement stiIndicatorElement = (IStiIndicatorElement)list[0];
				indicatorFontIcons = stiIndicatorElement.Icon;
				if (Designer.Report != null && !string.IsNullOrEmpty(stiIndicatorElement.CustomStyleName))
				{
					IEnumerable<StiIndicatorStyle> enumerable9 = Designer.Report.Styles.Cast<StiBaseStyle>().OfType<StiIndicatorStyle>();
					foreach (StiIndicatorStyle item14 in enumerable9)
					{
						if (item14.Name == stiIndicatorElement.CustomStyleName)
						{
							stiBaseStyle = item14;
							break;
						}
					}
				}
				if (stiBaseStyle == null)
				{
					if (stiIndicatorElement.Style == StiElementStyleIdent.Auto)
					{
						flag23 = true;
					}
					else
					{
						foreach (StiIndicatorElementStyle indicatorStyle in StiOptions.Services.Dashboards.IndicatorStyles)
						{
							if (indicatorStyle.Ident == stiIndicatorElement.Style)
							{
								stiBaseStyle = indicatorStyle;
								break;
							}
						}
					}
				}
			}
			else if (flag20)
			{
				StiSparkline stiSparkline = (StiSparkline)list[0];
				indicatorFontIcons = StiFontIcons.LineChart;
				if (Designer.Report != null && !string.IsNullOrEmpty(stiSparkline.ComponentStyle))
				{
					IEnumerable<StiIndicatorStyle> enumerable10 = Designer.Report.Styles.Cast<StiBaseStyle>().OfType<StiIndicatorStyle>();
					foreach (StiIndicatorStyle item15 in enumerable10)
					{
						if (item15.Name == stiSparkline.ComponentStyle)
						{
							stiBaseStyle = item15;
							break;
						}
					}
				}
			}
			else if (flag17)
			{
				IStiControlElement stiControlElement = (IStiControlElement)list[0];
				if (Designer.Report != null && !string.IsNullOrEmpty(stiControlElement.CustomStyleName))
				{
					IEnumerable<StiDialogStyle> enumerable11 = Designer.Report.Styles.Cast<StiBaseStyle>().OfType<StiDialogStyle>();
					foreach (StiDialogStyle item16 in enumerable11)
					{
						if (item16.Name == stiControlElement.CustomStyleName)
						{
							stiBaseStyle = item16;
							break;
						}
					}
				}
				if (stiBaseStyle == null)
				{
					if (stiControlElement.Style == StiElementStyleIdent.Auto)
					{
						flag23 = true;
					}
					else
					{
						foreach (StiControlElementStyle controlStyle in StiOptions.Services.Dashboards.ControlStyles)
						{
							if (controlStyle.Ident == stiControlElement.Style)
							{
								stiBaseStyle = controlStyle;
								break;
							}
						}
					}
				}
			}
			else if (flag18)
			{
				IStiProgressElement stiProgressElement = (IStiProgressElement)list[0];
				progressMode = stiProgressElement.Mode;
				if (Designer.Report != null && !string.IsNullOrEmpty(stiProgressElement.CustomStyleName))
				{
					IEnumerable<StiProgressStyle> enumerable12 = Designer.Report.Styles.Cast<StiBaseStyle>().OfType<StiProgressStyle>();
					foreach (StiProgressStyle item17 in enumerable12)
					{
						if (item17.Name == stiProgressElement.CustomStyleName)
						{
							stiBaseStyle = item17;
							break;
						}
					}
				}
				if (stiBaseStyle == null)
				{
					if (stiProgressElement.Style == StiElementStyleIdent.Auto)
					{
						flag23 = true;
					}
					else
					{
						foreach (StiProgressElementStyle progressStyle in StiOptions.Services.Dashboards.ProgressStyles)
						{
							if (progressStyle.Ident == stiProgressElement.Style)
							{
								stiBaseStyle = progressStyle;
								break;
							}
						}
					}
				}
			}
			else if (flag8)
			{
				IStiDashboardElementStyle stiDashboardElementStyle = list[0] as IStiDashboardElementStyle;
				if (Designer.Report != null && !string.IsNullOrEmpty(stiDashboardElementStyle.CustomStyleName))
				{
					IEnumerable<StiDialogStyle> enumerable13 = Designer.Report.Styles.Cast<StiBaseStyle>().OfType<StiDialogStyle>();
					foreach (StiDialogStyle item18 in enumerable13)
					{
						if (item18.Name == stiDashboardElementStyle.CustomStyleName)
						{
							stiBaseStyle = item18;
							break;
						}
					}
				}
				if (stiBaseStyle == null)
				{
					if (stiDashboardElementStyle.Style == StiElementStyleIdent.Auto)
					{
						flag23 = true;
					}
					else
					{
						foreach (StiControlElementStyle controlStyle2 in StiOptions.Services.Dashboards.ControlStyles)
						{
							if (controlStyle2.Ident == stiDashboardElementStyle.Style)
							{
								stiBaseStyle = controlStyle2;
								break;
							}
						}
					}
				}
			}
			else if (flag14)
			{
				IStiTableElement stiTableElement = (IStiTableElement)list[0];
				if (Designer.Report != null && !string.IsNullOrEmpty(stiTableElement.CustomStyleName))
				{
					IEnumerable<StiTableStyle> enumerable14 = Designer.Report.Styles.Cast<StiBaseStyle>().OfType<StiTableStyle>();
					foreach (StiTableStyle item19 in enumerable14)
					{
						if (item19.Name == stiTableElement.CustomStyleName)
						{
							stiBaseStyle = item19;
							break;
						}
					}
				}
				if (stiBaseStyle == null)
				{
					if (stiTableElement.Style == StiElementStyleIdent.Auto)
					{
						flag23 = true;
					}
					else
					{
						foreach (StiTableElementStyle tableStyle2 in StiOptions.Services.Dashboards.TableStyles)
						{
							if (tableStyle2.Ident == stiTableElement.Style)
							{
								stiBaseStyle = tableStyle2;
								break;
							}
						}
					}
				}
			}
			else if (flag9)
			{
				IStiCardsElement stiCardsElement = (IStiCardsElement)list[0];
				if (Designer.Report != null && !string.IsNullOrEmpty(stiCardsElement.CustomStyleName))
				{
					IEnumerable<StiCardsStyle> enumerable15 = Designer.Report.Styles.Cast<StiBaseStyle>().OfType<StiCardsStyle>();
					foreach (StiCardsStyle item20 in enumerable15)
					{
						if (item20.Name == stiCardsElement.CustomStyleName)
						{
							stiBaseStyle = item20;
							break;
						}
					}
				}
				if (stiBaseStyle == null)
				{
					if (stiCardsElement.Style == StiElementStyleIdent.Auto)
					{
						flag23 = true;
					}
					else
					{
						foreach (StiCardsElementStyle cardsStyle in StiOptions.Services.Dashboards.CardsStyles)
						{
							if (cardsStyle.Ident == stiCardsElement.Style)
							{
								stiBaseStyle = cardsStyle;
								break;
							}
						}
					}
				}
			}
			else if (!flag19)
			{
				if (flag15)
				{
					IStiPivotTableElement stiPivotTableElement = (IStiPivotTableElement)list[0];
					if (Designer.Report != null && !string.IsNullOrEmpty(stiPivotTableElement.CustomStyleName))
					{
						IEnumerable<StiCrossTabStyle> enumerable16 = Designer.Report.Styles.Cast<StiBaseStyle>().OfType<StiCrossTabStyle>();
						foreach (StiCrossTabStyle item21 in enumerable16)
						{
							if (item21.Name == stiPivotTableElement.CustomStyleName)
							{
								stiBaseStyle = item21;
								break;
							}
						}
					}
					if (stiBaseStyle == null)
					{
						if (stiPivotTableElement.Style == StiElementStyleIdent.Auto)
						{
							flag23 = true;
						}
						else
						{
							foreach (StiPivotElementStyle pivotStyle in StiOptions.Services.Dashboards.PivotStyles)
							{
								if (pivotStyle.Ident == stiPivotTableElement.Style)
								{
									stiBaseStyle = pivotStyle;
									break;
								}
							}
						}
					}
				}
				else if (flag22)
				{
					IStiScreen stiScreen = (IStiScreen)designer.Report.GetCurrentPage();
					foreach (StiDashboardStyle dashboardStyle in StiOptions.Services.Dashboards.DashboardStyles)
					{
						if (dashboardStyle.Ident == stiScreen.Style)
						{
							stiBaseStyle = dashboardStyle;
							break;
						}
					}
				}
				else if (flag21)
				{
					IStiDashboard stiDashboard = (IStiDashboard)designer.Report.GetCurrentPage();
					foreach (StiDashboardStyle dashboardStyle2 in StiOptions.Services.Dashboards.DashboardStyles)
					{
						if (dashboardStyle2.Ident == stiDashboard.Style)
						{
							stiBaseStyle = dashboardStyle2;
							break;
						}
					}
				}
				else if (Designer.Report != null)
				{
					IEnumerable<StiBaseStyle> source = Designer.Report.Styles.Cast<StiBaseStyle>();
					if (list.Count > 0)
					{
						string compStyleName = list[0].ComponentStyle;
						stiBaseStyle = source.FirstOrDefault((StiBaseStyle x) => x.Name == compStyleName);
					}
				}
			}
		}
		if (Designer.Report != null)
		{
			StiReport report = designer.Report;
			bool isDashboardPage = report.GetCurrentPage() is IStiDashboard;
			bool flag24 = report.GetCurrentPage() is IStiScreen;
			galleryStyles.Collect();
			galleryStyles.IsRootItem = true;
			galleryStyles.MapIdent = mapIdent;
			galleryStyles.GaugeType = gaugeType;
			galleryStyles.AddGaugeRange = addGaugeRange;
			galleryStyles.ProgressMode = progressMode;
			galleryStyles.IndicatorFontIcons = indicatorFontIcons;
			galleryStyles.Report = report;
			galleryStyles.IsDashboardPage = isDashboardPage;
			galleryStyles.IsScreenPage = flag22;
			if (stiBaseStyle != null)
			{
				galleryStyles.IsDashboardAutoStyle = false;
				galleryStyles.SelectedStyle = stiBaseStyle;
			}
			else if (flag23)
			{
				galleryStyles.IsDashboardAutoStyle = true;
				galleryStyles.MapIdent = mapIdent;
				galleryStyles.Report = report;
				galleryStyles.ProgressMode = progressMode;
				galleryStyles.IndicatorFontIcons = indicatorFontIcons;
				galleryStyles.GaugeType = gaugeType;
				galleryStyles.SelectedStyle = GetAutoStyle(flag11, flag10, flag12, flag16, flag17, flag18, flag14, flag15, flag9);
			}
			else
			{
				galleryStyles.IsDashboardAutoStyle = false;
				galleryStyles.SelectedStyle = NullStyle;
			}
		}
		galleryStyles.Refresh();
		((Control)ribbonBarStyle).Invalidate();
	}

	private void GalleryStyles_Click(object sender, EventArgs e)
	{
		new StiRibbonStylesPopup(Designer, galleryStyles).Show();
	}

	public virtual void RefreshLayout()
	{
		StiComponentsCollection sizeComponentsCollection = StiActions.Layout.States.GetSizeComponentsCollection(Designer);
		StiComponentsCollection selectedComponentsCollection = StiActions.Layout.States.GetSelectedComponentsCollection(Designer);
		ButtonItem buttonItem = biMakeSameSize;
		string tooltip = (biMakeSameSize.Text = StiActions.Layout.States.GetMakeSameSizeText(Designer, sizeComponentsCollection));
		buttonItem.Tooltip = tooltip;
		ButtonItem buttonItem2 = biMakeSameWidth;
		tooltip = (biMakeSameWidth.Text = StiActions.Layout.States.GetMakeSameWidthText(Designer, sizeComponentsCollection));
		buttonItem2.Tooltip = tooltip;
		ButtonItem buttonItem3 = biMakeSameHeight;
		tooltip = (biMakeSameHeight.Text = StiActions.Layout.States.GetMakeSameHeightText(Designer, sizeComponentsCollection));
		buttonItem3.Tooltip = tooltip;
		biSize.Enabled = StiActions.Layout.States.GetSizeEnabled(Designer, selectedComponentsCollection);
		biMakeSameSize.Enabled = StiActions.Layout.States.GetMakeSameSizeEnabled(Designer, selectedComponentsCollection);
		biMakeSameWidth.Enabled = StiActions.Layout.States.GetMakeSameWidthEnabled(Designer, selectedComponentsCollection);
		biMakeSameHeight.Enabled = StiActions.Layout.States.GetMakeSameHeightEnabled(Designer, selectedComponentsCollection);
		biBringToFront.Enabled = StiActions.Layout.States.GetBringToFrontEnabled(Designer, selectedComponentsCollection);
		biSendToBack.Enabled = StiActions.Layout.States.GetBringToFrontEnabled(Designer, selectedComponentsCollection);
		biMoveForward.Enabled = StiActions.Layout.States.GetBringToFrontEnabled(Designer, selectedComponentsCollection);
		biMoveBackward.Enabled = StiActions.Layout.States.GetBringToFrontEnabled(Designer, selectedComponentsCollection);
		biAlignToGrid.Enabled = StiActions.Layout.States.GetAlignToGridEnabled(Designer, selectedComponentsCollection);
		biCenterHorizontally.Enabled = StiActions.Layout.States.GetCenterHorizontallyEnabled(Designer, selectedComponentsCollection);
		biCenterVertically.Enabled = StiActions.Layout.States.GetCenterVerticallyEnabled(Designer, selectedComponentsCollection);
		biAlign.Enabled = StiActions.Layout.States.GetAlignEnabled(Designer, selectedComponentsCollection);
		biAlignLeft.Enabled = StiActions.Layout.States.GetAlignLeftEnabled(Designer, selectedComponentsCollection);
		biAlignCenter.Enabled = StiActions.Layout.States.GetAlignCenterEnabled(Designer, selectedComponentsCollection);
		biAlignRight.Enabled = StiActions.Layout.States.GetAlignRightEnabled(Designer, selectedComponentsCollection);
		biAlignTop.Enabled = StiActions.Layout.States.GetAlignTopEnabled(Designer, selectedComponentsCollection);
		biAlignMiddle.Enabled = StiActions.Layout.States.GetAlignMiddleEnabled(Designer, selectedComponentsCollection);
		biAlignBottom.Enabled = StiActions.Layout.States.GetAlignBottomEnabled(Designer, selectedComponentsCollection);
		biMakeHorizontalSpacingEqual.Enabled = StiActions.Layout.States.GetMakeHorizontalSpacingEqualEnabled(Designer, selectedComponentsCollection);
		biMakeVerticalSpacingEqual.Enabled = StiActions.Layout.States.GetMakeVerticalSpacingEqualEnabled(Designer, selectedComponentsCollection);
		biLink.Enabled = StiActions.Layout.States.GetLinkEnabled(Designer, selectedComponentsCollection);
		biLock.Enabled = StiActions.Layout.States.GetLockEnabled(Designer, selectedComponentsCollection);
		if (Designer.Report != null)
		{
			biLock.Checked = StiActions.Layout.States.GetLockChecked(Designer);
			biLink.Checked = StiActions.Layout.States.GetLinkChecked(Designer);
		}
	}

	public virtual void RefreshPage()
	{
		biMargins.Enabled = StiActions.Page.States.GetMarginsEnabled(Designer);
		biOrientation.Enabled = StiActions.Page.States.GetOrientationEnabled(Designer);
		biPageSize.Enabled = StiActions.Page.States.GetPageSizeEnabled(Designer);
		biColumns.Enabled = StiActions.Page.States.GetColumnsEnabled(Designer);
		biColumnsOne.Checked = StiActions.Page.States.GetColumnsOneChecked(Designer);
		biColumnsTwo.Checked = StiActions.Page.States.GetColumnsTwoChecked(Designer);
		biColumnsThree.Checked = StiActions.Page.States.GetColumnsThreeChecked(Designer);
		StiPage stiPage = Designer.Report?.GetCurrentPage();
		biMargins.Visible = stiPage != null && !stiPage.IsDashboard;
		biColumns.Visible = stiPage != null && !stiPage.IsDashboard;
		((Control)ribbonBarView).Visible = stiPage?.IsDashboard ?? false;
		biSurfaceViewDesktop.Checked = StiActions.Page.States.GetSurfaceViewDesktopChecked(Designer);
		biSurfaceViewMobile.Checked = StiActions.Page.States.GetSurfaceViewMobileChecked(Designer);
		biSurfaceViewRemoveMobileSurface.Enabled = StiActions.Page.States.GetSurfaceViewMobilePresent(Designer);
		biSurfaceViewComponents.Checked = StiActions.Page.States.GetSurfaceViewComponentsVisible(Designer);
		biSurfaceViewComponents.Enabled = StiActions.Page.States.GetSurfaceViewComponentsEnabled(Designer);
		((Control)ribbonBarView).Refresh();
		ribbonBarPageSetup.DialogLauncherVisible = StiActions.Page.States.GetPageSetupDialogEnabled(Designer);
		((Control)ribbonBarPageSetup).Width = 0;
		((Control)ribbonBarPageSetup).Invalidate();
		ribbonPanelPage.LayoutRibbons();
		_ = Designer.Report;
	}

	public virtual void RefreshWatermarkText()
	{
		biWatermarkSettings.Enabled = StiActions.Watermark.States.GetEditorEnabled(Designer);
	}

	public virtual void RefreshViewOptions()
	{
		biViewAlignToGrid.Enabled = StiActions.ViewOptions.States.GetAlignToGridEnabled(Designer);
		biViewShowGrid.Enabled = StiActions.ViewOptions.States.GetShowGridEnabled(Designer);
		biViewShowHeaders.Enabled = StiActions.ViewOptions.States.GetShowHeadersEnabled(Designer);
		biViewShowOrder.Enabled = StiActions.ViewOptions.States.GetShowOrderEnabled(Designer);
		biViewShowRulers.Enabled = StiActions.ViewOptions.States.GetShowRulersEnabled(Designer);
		biGridMode.Enabled = StiActions.ViewOptions.States.GetGridModeEnabled(Designer);
		biQuickInfo.Enabled = StiActions.ViewOptions.States.GetQuickInfoEnabled(Designer);
		biQuickInfoShowContent.Enabled = StiActions.ViewOptions.States.GetQuickInfoShowContentEnabled(Designer);
		biQuickInfoShowFields.Enabled = StiActions.ViewOptions.States.GetQuickInfoShowFieldsEnabled(Designer);
		biQuickInfoShowFieldsOnly.Enabled = StiActions.ViewOptions.States.GetQuickInfoShowFieldsOnlyEnabled(Designer);
		if (Designer.Report != null)
		{
			biViewAlignToGrid.Checked = StiActions.ViewOptions.States.GetAlignToGridChecked(Designer);
			biViewShowGrid.Checked = StiActions.ViewOptions.States.GetShowGridChecked(Designer);
			biViewShowHeaders.Checked = StiActions.ViewOptions.States.GetShowHeadersChecked(Designer);
			biViewShowOrder.Checked = StiActions.ViewOptions.States.GetShowOrderChecked(Designer);
			biViewShowRulers.Checked = StiActions.ViewOptions.States.GetShowRulersChecked(Designer);
			biGridModeDotes.Checked = StiActions.ViewOptions.States.GetGridModeDotsChecked(Designer);
			biGridModeLines.Checked = StiActions.ViewOptions.States.GetGridModeLinesChecked(Designer);
			biShowToolbox.Checked = StiActions.ViewOptions.States.GetShowToolboxChecked(Designer);
		}
	}

	public void LocalizeToolTip()
	{
		if (StiOptions.Designer.ShowSuperToolTip)
		{
			Graphics globalMeasureGraphics = StiReport.GlobalMeasureGraphics;
			string url = "user-manual/reports_designer_ribbon_mode_2013_main_menu.htm";
			Designer.SetToolTipEx(OfficeStartButton, globalMeasureGraphics, "MainMenu", url);
			url = "user-manual/index.html?reports_designer_ribbon_mode_2013_ribbon_tabs_home_clipboard.htm";
			Designer.SetToolTipEx(biCut, globalMeasureGraphics, biCut.Text.Replace("&", "") + " (Ctrl + X)", "menuEditCut", url);
			Designer.SetToolTipEx(biCopy, globalMeasureGraphics, biCopy.Text.Replace("&", "") + " (Ctrl + C)", "menuEditCopy", url);
			Designer.SetToolTipEx(biDelete, globalMeasureGraphics, biDelete.Text.Replace("&", "") + " (Del)", "menuEditDelete", url);
			Designer.SetToolTipEx(biPaste, globalMeasureGraphics, biPaste.Text.Replace("&", "") + " (Ctrl + V)", "menuEditPaste", url);
			url = "user-manual/reports_designer_ribbon_mode_2013_ribbon_tabs_home_font.htm";
			Designer.SetToolTipEx(cbFont, globalMeasureGraphics, "FontName", url);
			Designer.SetToolTipEx(cbFontSize, globalMeasureGraphics, "FontSize", url);
			Designer.SetToolTipEx(biFontBold, globalMeasureGraphics, biFontBold.Text + " (Ctrl + B)", "FontStyleBold", url);
			Designer.SetToolTipEx(biFontItalic, globalMeasureGraphics, biFontItalic.Text + " (Ctrl + I)", "FontStyleItalic", url);
			Designer.SetToolTipEx(biFontUnderline, globalMeasureGraphics, biFontUnderline.Text + " (Ctrl + U)", "FontStyleUnderline", url);
			Designer.SetToolTipEx(biFontStrikeout, globalMeasureGraphics, biFontStrikeout.Text, "FontStyleStrikeout", url);
			Designer.SetToolTipEx(biFontGrow, globalMeasureGraphics, "FontGrow", url);
			Designer.SetToolTipEx(biFontShrink, globalMeasureGraphics, "FontShrink", url);
			Designer.SetToolTipEx(biClearContents, globalMeasureGraphics, "menuEditClearContents", url);
			Designer.SetToolTipEx(biTextBrush, globalMeasureGraphics, "TextColor", url);
			url = "user-manual/reports_designer_ribbon_mode_2013_ribbon_tabs_home_borders.htm";
			Designer.SetToolTipEx(biBorderAll, globalMeasureGraphics, "DockStyleFill", url);
			Designer.SetToolTipEx(biBorderNone, globalMeasureGraphics, "DockStyleNone", url);
			Designer.SetToolTipEx(biBorderTop, globalMeasureGraphics, "DockStyleTop", url);
			Designer.SetToolTipEx(biBorderLeft, globalMeasureGraphics, "DockStyleLeft", url);
			Designer.SetToolTipEx(biBorderBottom, globalMeasureGraphics, "DockStyleBottom", url);
			Designer.SetToolTipEx(biBorderRight, globalMeasureGraphics, "DockStyleRight", url);
			Designer.SetToolTipEx(biBorderShadow, globalMeasureGraphics, "Shadow", url);
			Designer.SetToolTipEx(biFillBrush, globalMeasureGraphics, "Background", url);
			Designer.SetToolTipEx(biBorderColor, globalMeasureGraphics, "BorderColor", url);
			Designer.SetToolTipEx(biBorderStyle, globalMeasureGraphics, "BorderStyle", url);
			url = "user-manual/reports_designer_ribbon_mode_2013_ribbon_tabs_home_format_text.htm";
			Designer.SetToolTipEx(biCurrencySymbol, globalMeasureGraphics, "CurrencySymbol", url);
			Designer.SetToolTipEx(biDateTimeFormat, globalMeasureGraphics, "DateTimeFormat", url);
			Designer.SetToolTipEx(biTextFormat, globalMeasureGraphics, "TextFormat", url);
			Designer.SetToolTipEx(biInteraction, globalMeasureGraphics, "Interaction", "user-manual/report_internals_interaction.htm");
			Designer.SetToolTipEx(biConditions, globalMeasureGraphics, "biConditions", "user-manual/report_internals_conditional_formatting.htm");
			Designer.SetToolTipEx(biCopyStyle, globalMeasureGraphics, "CopyStyle", "user-manual");
			Designer.SetToolTipEx(biStyleDesigner, globalMeasureGraphics, "StyleDesigner", "user-manual/report_internals_appearance_style_designer.htm");
			url = "user-manual/reports_designer_ribbon_mode_2013_ribbon_tabs_home_alignment.htm";
			Designer.SetToolTipEx(biContentAlignLeft, globalMeasureGraphics, biContentAlignLeft.Text + " (Ctrl + L)", "AlignLeft", url);
			Designer.SetToolTipEx(biContentAlignCenter, globalMeasureGraphics, biContentAlignCenter.Text + " (Ctrl + E)", "AlignCenter", url);
			Designer.SetToolTipEx(biContentAlignRight, globalMeasureGraphics, biContentAlignRight.Text + " (Ctrl + R)", "AlignRight", url);
			Designer.SetToolTipEx(biContentAlignWidth, globalMeasureGraphics, biContentAlignWidth.Text + " (Ctrl + J)", "AlignWidth", url);
			Designer.SetToolTipEx(biContentAlignTop, globalMeasureGraphics, "AlignTop", url);
			Designer.SetToolTipEx(biContentAlignMiddle, globalMeasureGraphics, "AlignMiddle", url);
			Designer.SetToolTipEx(biContentAlignBottom, globalMeasureGraphics, "AlignBottom", url);
			Designer.SetToolTipEx(biRotateText, globalMeasureGraphics, "Angle", url);
			Designer.SetToolTipEx(biLineSpacing, globalMeasureGraphics, "LineSpacing", url);
			Designer.SetToolTipEx(biWordWrap, globalMeasureGraphics, "WordWrap", url);
			url = "user-manual/reports_designer_ribbon_mode_2013_ribbon_tabs_page_page_setup.htm";
			Designer.SetToolTipEx(biMargins, globalMeasureGraphics, "Margins", url);
			Designer.SetToolTipEx(biOrientation, globalMeasureGraphics, "Orientation", url);
			Designer.SetToolTipEx(biPageSize, globalMeasureGraphics, "PageSize", url);
			Designer.SetToolTipEx(biColumns, globalMeasureGraphics, "Columns", url);
			url = "user-manual/reports_designer_ribbon_mode_2013_ribbon_tabs_page_viewing_options.htm";
			Designer.SetToolTipEx(biGridMode, globalMeasureGraphics, "GridMode", url);
			Designer.SetToolTipEx(biViewAlignToGrid, globalMeasureGraphics, "menuViewAlignToGrid", url);
			Designer.SetToolTipEx(biViewShowGrid, globalMeasureGraphics, "menuViewShowGrid", url);
			Designer.SetToolTipEx(biViewShowHeaders, globalMeasureGraphics, "menuViewShowHeaders", url);
			Designer.SetToolTipEx(biViewShowOrder, globalMeasureGraphics, "menuViewShowOrder", url);
			Designer.SetToolTipEx(biViewShowRulers, globalMeasureGraphics, "menuViewShowRulers", url);
			Designer.SetToolTipEx(biQuickInfo, globalMeasureGraphics, "menuViewQuickInfo", url);
			Designer.SetToolTipEx(biDockingPanels, globalMeasureGraphics, "DockingPanels", url);
			Designer.SetToolTipEx(biShowToolbox, globalMeasureGraphics, "ShowToolbox", url);
			url = "user-manual/reports_designer_ribbon_mode_2013_ribbon_tabs_layout_arrange.htm";
			Designer.SetToolTipEx(biAlign, globalMeasureGraphics, "Align", url);
			Designer.SetToolTipEx(biAlignToGrid, globalMeasureGraphics, "AlignToGrid", url);
			Designer.SetToolTipEx(biSize, globalMeasureGraphics, "ComponentSize", url);
			Designer.SetToolTipEx(biBringToFront, globalMeasureGraphics, "BringToFront", url);
			Designer.SetToolTipEx(biSendToBack, globalMeasureGraphics, "SendToBack", url);
			Designer.SetToolTipEx(biMoveForward, globalMeasureGraphics, "MoveForward", url);
			Designer.SetToolTipEx(biMoveBackward, globalMeasureGraphics, "MoveBackward", url);
			Designer.SetToolTipEx(biLock, globalMeasureGraphics, "Lock", url);
			Designer.SetToolTipEx(biLink, globalMeasureGraphics, "Link", url);
		}
		else
		{
			biFontBold.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "FontStyleBold")) + " (Ctrl + B)";
			biFontItalic.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "FontStyleItalic")) + " (Ctrl + I)";
			biFontUnderline.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "FontStyleUnderline")) + " (Ctrl + U)";
			biContentAlignLeft.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignLeft")) + " (Ctrl + L)";
			biContentAlignCenter.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignCenter")) + " (Ctrl + E)";
			biContentAlignRight.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignRight")) + " (Ctrl + R)";
			biContentAlignWidth.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignWidth")) + " (Ctrl + J)";
			biGridMode.Tooltip = biGridMode.Text;
			biViewAlignToGrid.Tooltip = biViewAlignToGrid.Text;
			biViewShowGrid.Tooltip = biViewShowGrid.Text;
			biViewShowHeaders.Tooltip = biViewShowHeaders.Text;
			biViewShowOrder.Tooltip = biViewShowOrder.Text;
			biViewShowRulers.Tooltip = biViewShowRulers.Text;
			biQuickInfo.Tooltip = biQuickInfo.Text;
			biDockingPanels.Tooltip = biDockingPanels.Text;
			biShowToolbox.Tooltip = biShowToolbox.Text;
			biWatermarkSettings.Tooltip = biWatermarkSettings.Text;
			biMargins.Tooltip = biMargins.Text;
			biOrientation.Tooltip = biOrientation.Text;
			biPageSize.Tooltip = biPageSize.Text;
			biColumns.Tooltip = biColumns.Text;
			biAlign.Tooltip = biAlign.Text;
			biAlignToGrid.Tooltip = biAlignToGrid.Text;
			biSize.Tooltip = biSize.Text;
			biBringToFront.Tooltip = biBringToFront.Text;
			biSendToBack.Tooltip = biSendToBack.Text;
			biMoveForward.Tooltip = biMoveForward.Text;
			biMoveBackward.Tooltip = biMoveBackward.Text;
			biLock.Tooltip = biLock.Text;
			biLink.Tooltip = biLink.Text;
			biInteraction.Tooltip = biInteraction.Text;
			biConditions.Tooltip = biConditions.Text;
			biCopyStyle.Tooltip = biCopyStyle.Text;
			biStyleDesigner.Tooltip = biStyleDesigner.Text;
			biCurrencySymbol.Tooltip = biCurrencySymbol.Text;
			biDateTimeFormat.Tooltip = biDateTimeFormat.Text;
			biTextFormat.Tooltip = biTextFormat.Text;
			biBorderShadow.Tooltip = biBorderShadow.Text;
			biFillBrush.Tooltip = biFillBrush.Text;
			biBorderColor.Tooltip = biBorderColor.Text;
			biBorderStyle.Tooltip = biBorderStyle.Text;
			biContentAlignTop.Tooltip = biContentAlignTop.Text;
			biContentAlignMiddle.Tooltip = biContentAlignMiddle.Text;
			biContentAlignBottom.Tooltip = biContentAlignBottom.Text;
			biRotateText.Tooltip = biRotateText.Text;
			biWordWrap.Tooltip = biWordWrap.Text;
			cbFont.Tooltip = cbFont.Text;
			cbFontSize.Tooltip = cbFontSize.Text;
			biFontGrow.Tooltip = biFontGrow.Text;
			biFontShrink.Tooltip = biFontShrink.Text;
			biClearContents.Tooltip = biClearContents.Text;
			biTextBrush.Tooltip = biTextBrush.Text;
			biCut.Tooltip = biCut.Text;
			biCopy.Tooltip = biCopy.Text;
			biDelete.Tooltip = biDelete.Text;
			biPaste.Tooltip = biPaste.Text;
			biBorderAll.Tooltip = biBorderAll.Text;
			biBorderNone.Tooltip = biBorderNone.Text;
			biBorderTop.Tooltip = biBorderTop.Text;
			biBorderLeft.Tooltip = biBorderLeft.Text;
			biBorderBottom.Tooltip = biBorderBottom.Text;
			biBorderRight.Tooltip = biBorderRight.Text;
		}
	}

	public void Localize()
	{
		LocalizeMainMenu();
		LocalizeBarClipboard();
		LocalizeBarFont();
		LocalizeBarAlignment();
		LocalizeBarBorders();
		LocalizeBarTextFormat();
		LocalizeBarStyles();
		LocalizeDashboardView();
		LocalizeBarArrange();
		LocalizeBarPageSetup();
		LocalizeBarWatermarkText();
		LocalizeBarViewOptions();
		LocalizeQuickBar();
		ribbonTabHome.Text = StiLocalization.Get("Toolbars", "TabHome");
		ribbonTabInsert.Text = StiLocalization.Get("PropertyMain", "Insert");
		ribbonTabLayout.Text = StiLocalization.Get("Toolbars", "TabLayout");
		ribbonTabPage.Text = StiLocalization.Get("Toolbars", "TabPage");
		biAbout.Text = StiLocalization.Get("MainMenu", "menuHelpAboutProgramm");
		biLogin.Text = StiLocalization.Get("Cloud", "Login");
		biPublish.Text = StiLocalization.Get("Buttons", "Publish");
		LocalizeToolTip();
		RefreshStyles();
		NullStyle.Name = StiLocalization.Get("FormConditions", "SelectStyle");
		DashboardAutoStyle.Name = StiLocalization.Get("PropertyEnum", "StiGaugeCalculationModeAuto");
		biSelectLanguage.Text = StiLocalization.CultureName.ToUpperInvariant();
		CalculateRightButtonsSize();
		accountMenu?.Localize();
	}

	public void LocalizeMainMenu()
	{
		liRecentDocuments.Text = StiLocalization.Get("MainMenu", "menuFileRecentDocuments");
		if (excludeLocalizeFile.Contains(StiLocalization.CultureName))
		{
			OfficeStartButton.Text = "File";
		}
		else
		{
			OfficeStartButton.Text = StiLocalization.Get("MainMenu", "menuFile");
		}
		biNew.Text = StiLocalization.Get("MainMenu", "menuFileNew");
		biBlankReport.Text = StiLocalization.Get("Wizards", "BlankReport");
		biBlankDashboard.Text = StiLocalization.Get("Wizards", "BlankDashboard");
		biBlankScreen.Text = StiLocalization.Get("Wizards", "BlankForm");
		biReportOpen.Text = StiLocalization.Get("MainMenu", "menuFileOpen");
		biReportSave.Text = StiLocalization.Get("MainMenu", "menuFileSave");
		biSaveAs.Text = StiLocalization.Get("MainMenu", "menuFileSaveAs");
		biCheckIssues.Text = StiLocalization.Get("MainMenu", "menuCheckIssues");
		biReportSetup.Text = StiLocalization.Get("MainMenu", "menuFileReportSetup");
		biClose.Text = StiLocalization.Get("MainMenu", "menuFileClose");
		biOptions.Text = StiLocalization.Get("MainMenu", "menuToolsOptions");
		biExit.Text = StiLocalization.Get("MainMenu", "menuFileExit");
		biNew.Tooltip = StiLocalization.Get("MainMenu", "menuFileNew");
		biBlankReport.Tooltip = StiLocalization.Get("Wizards", "BlankReport");
		biBlankDashboard.Tooltip = StiLocalization.Get("Wizards", "BlankDashboard");
		biBlankScreen.Tooltip = StiLocalization.Get("Wizards", "BlankForm");
		biReportOpen.Tooltip = StiLocalization.Get("MainMenu", "menuFileReportOpen");
		biReportSave.Tooltip = StiLocalization.Get("MainMenu", "menuFileReportSave");
		biSaveAs.Tooltip = StiLocalization.Get("MainMenu", "menuFileSaveAs");
		biReportSetup.Tooltip = StiLocalization.Get("MainMenu", "menuFileReportSetup");
		biClose.Tooltip = StiLocalization.Get("MainMenu", "menuFileClose");
		biOptions.Tooltip = StiLocalization.Get("MainMenu", "menuToolsOptions");
		biExit.Tooltip = StiLocalization.Get("MainMenu", "menuFileExit");
	}

	public void LocalizeBarClipboard()
	{
		biCut.Text = StiLocalization.Get("MainMenu", "menuEditCut");
		biCopy.Text = StiLocalization.Get("MainMenu", "menuEditCopy");
		biPaste.Text = StiLocalization.Get("MainMenu", "menuEditPaste");
		biDelete.Text = StiLocalization.Get("MainMenu", "menuEditDelete");
		((Control)ribbonBarClipboard).Text = StiLocalization.Get("Toolbars", "ToolbarClipboard");
	}

	public void LocalizeBarFont()
	{
		cbFont.Caption = StiHint.CreateHint(StiLocalization.Get("Toolbars", "FontName"));
		cbFontSize.Caption = StiHint.CreateHint(StiLocalization.Get("Toolbars", "FontSize"));
		biFontBold.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "FontStyleBold"));
		biFontItalic.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "FontStyleItalic"));
		biFontUnderline.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "FontStyleUnderline"));
		biFontGrow.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "FontGrow"));
		biFontShrink.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "FontShrink"));
		biClearContents.Text = StiLocalization.Get("MainMenu", "menuEditClearContents");
		biTextBrush.Text = StiHint.CreateHint(StiLocalization.Get("PropertyMain", "TextColor"));
		((Control)ribbonBarFont).Text = StiLocalization.Get("Toolbars", "ToolbarFont");
	}

	public void LocalizeBarAlignment()
	{
		cmLineSpacing1.Text = $"{1f:0.0}";
		cmLineSpacing115.Text = $"{1.15f:0.00}";
		cmLineSpacing15.Text = $"{1.5f:0.0}";
		cmLineSpacing2.Text = $"{2f:0.0}";
		cmLineSpacing25.Text = $"{2.5f:0.0}";
		cmLineSpacing3.Text = $"{3f:0.0}";
		cmRotateAngle0.Text = StiLocalization.Get("PropertyEnum", "StiAngleAngle0");
		cmRotateAngle45.Text = StiLocalization.Get("PropertyEnum", "StiAngleAngle45");
		cmRotateAngle90.Text = StiLocalization.Get("PropertyEnum", "StiAngleAngle90");
		cmRotateAngle180.Text = StiLocalization.Get("PropertyEnum", "StiAngleAngle180");
		cmRotateAngle270.Text = StiLocalization.Get("PropertyEnum", "StiAngleAngle270");
		biContentAlignLeft.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignLeft"));
		biContentAlignCenter.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignCenter"));
		biContentAlignRight.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignRight"));
		biContentAlignWidth.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignWidth"));
		biContentAlignTop.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignTop"));
		biContentAlignMiddle.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignMiddle"));
		biContentAlignBottom.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignBottom"));
		biRotateText.Text = StiHint.CreateHint(StiLocalization.Get("PropertyMain", "Angle"));
		biLineSpacing.Text = StiHint.CreateHint(StiLocalization.Get("PropertyMain", "LineSpacing"));
		biWordWrap.Text = StiLocalization.Get("PropertyMain", "WordWrap");
		((Control)ribbonBarAlignment).Text = StiLocalization.Get("Toolbars", "ToolbarAlignment");
	}

	public void LocalizeBarBorders()
	{
		biBorderAll.Text = StiHint.CreateHint(StiLocalization.Get("PropertyEnum", "StiBorderSidesAll"));
		biBorderNone.Text = StiHint.CreateHint(StiLocalization.Get("PropertyEnum", "StiBorderSidesNone"));
		biBorderTop.Text = StiHint.CreateHint(StiLocalization.Get("PropertyEnum", "StiBorderSidesTop"));
		biBorderLeft.Text = StiHint.CreateHint(StiLocalization.Get("PropertyEnum", "StiBorderSidesLeft"));
		biBorderBottom.Text = StiHint.CreateHint(StiLocalization.Get("PropertyEnum", "StiBorderSidesBottom"));
		biBorderRight.Text = StiHint.CreateHint(StiLocalization.Get("PropertyEnum", "StiBorderSidesRight"));
		biBorderShadow.Text = StiHint.CreateHint(StiLocalization.Get("PropertyMain", "Shadow"));
		biFillBrush.Text = StiHint.CreateHint(StiLocalization.Get("PropertyMain", "Background"));
		biBorderColor.Text = StiHint.CreateHint(StiLocalization.Get("PropertyMain", "BorderColor"));
		if (StiScale.X == 1.0)
		{
			biBorderStyle.Text = StiHint.CreateHint(StiLocalization.Get("PropertyMain", "BorderStyle"));
		}
		((Control)ribbonBarBorders).Text = StiLocalization.Get("Toolbars", "ToolbarBorders");
	}

	public void LocalizeBarTextFormat()
	{
		biCurrencySymbol.Text = StiHint.CreateHint(StiLocalization.Get("FormFormatEditor", "CurrencySymbol"));
		biDateTimeFormat.Text = StiHint.CreateHint(StiLocalization.Get("FormFormatEditor", "DateTimeFormat"));
		((Control)ribbonBarTextFormat).Text = StiLocalization.Get("Toolbars", "ToolbarTextFormat");
		biTextFormat.Text = StiLocalization.Get("Toolbars", "ToolbarTextFormat");
	}

	public void LocalizeBarStyles()
	{
		biInteraction.Text = Loc.GetMain("Interaction");
		biConditions.Text = StiLocalization.Get("Toolbars", "Conditions");
		biCopyStyle.Text = StiLocalization.Get("Toolbox", "Style");
		biStyleDesigner.Text = StiLocalization.Get("Toolbars", "StyleDesigner");
		((Control)ribbonBarStyle).Text = StiLocalization.Get("Toolbars", "ToolbarStyle");
		galleryRecentFiles.Text = StiLocalization.Get("PropertyMain", "Styles");
		biConditions.Refresh();
		biCopyStyle.Refresh();
		biStyleDesigner.Refresh();
		((Control)ribbonBarStyle).Refresh();
		galleryStyles.Refresh();
		galleryRecentFiles.Refresh();
	}

	public void LocalizeDashboardView()
	{
		biSurfaceViewDesktop.Text = Loc.Get("Dashboard", "ViewModeDesktop");
		biSurfaceViewMobile.Text = Loc.Get("Dashboard", "ViewModeMobile");
		biSurfaceViewComponents.Text = Loc.Get("Report", "Components");
		biSurfaceViewRemoveMobileSurface.Text = Loc.Get("Dashboard", "RemoveMobileSurface");
		((Control)ribbonBarView).Text = Loc.Get("Toolbars", "TabView");
	}

	public void LocalizeBarArrange()
	{
		biAlign.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "Align"));
		biAlignToGrid.Text = BreakTextToLines(StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignToGrid")));
		biAlignLeft.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignLeft"));
		biAlignCenter.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignCenter"));
		biAlignRight.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignRight"));
		biAlignTop.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignTop"));
		biAlignMiddle.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignMiddle"));
		biAlignBottom.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignBottom"));
		biSize.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "Size"));
		biMakeSameSize.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "MakeSameSize"));
		biMakeSameWidth.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "MakeSameWidth"));
		biMakeSameHeight.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "MakeSameHeight"));
		biMakeHorizontalSpacingEqual.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "MakeHorizontalSpacingEqual"));
		biMakeVerticalSpacingEqual.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "MakeVerticalSpacingEqual"));
		biCenterVertically.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "CenterVertically"));
		biCenterHorizontally.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "CenterHorizontally"));
		biBringToFront.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "BringToFront"));
		biSendToBack.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "SendToBack"));
		biMoveForward.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "MoveForward"));
		biMoveBackward.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "MoveBackward"));
		biLock.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "Lock"));
		biLink.Text = StiHint.CreateHint(StiLocalization.Get("Toolbars", "Link"));
		biAlignLeft.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignLeft"));
		biAlignCenter.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignCenter"));
		biAlignRight.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignRight"));
		biAlignTop.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignTop"));
		biAlignMiddle.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignMiddle"));
		biAlignBottom.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "AlignBottom"));
		biMakeSameSize.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "MakeSameSize"));
		biMakeSameWidth.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "MakeSameWidth"));
		biMakeSameHeight.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "MakeSameHeight"));
		biMakeHorizontalSpacingEqual.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "MakeHorizontalSpacingEqual"));
		biMakeVerticalSpacingEqual.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "MakeVerticalSpacingEqual"));
		biCenterVertically.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "CenterVertically"));
		biCenterHorizontally.Tooltip = StiHint.CreateHint(StiLocalization.Get("Toolbars", "CenterHorizontally"));
		((Control)ribbonBarArrange).Text = StiLocalization.Get("Toolbars", "ToolbarArrange");
	}

	public void LocalizeBarPageSetup()
	{
		biMargins.Text = StiLocalization.Get("FormPageSetup", "Margins");
		biOrientation.Text = StiLocalization.Get("FormPageSetup", "Orientation");
		biOrientationPortrait.Text = StiLocalization.Get("FormPageSetup", "PageOrientationPortrait");
		biOrientationLandscape.Text = StiLocalization.Get("FormPageSetup", "PageOrientationLandscape");
		biPageSize.Text = StiLocalization.Get("PropertyMain", "Size");
		biColumns.Text = StiLocalization.Get("FormPageSetup", "Columns");
		biColumnsOne.Text = StiLocalization.Get("FormDesigner", "ColumnsOne");
		biColumnsTwo.Text = StiLocalization.Get("FormDesigner", "ColumnsTwo");
		biColumnsThree.Text = StiLocalization.Get("FormDesigner", "ColumnsThree");
		biOrientationPortrait.Tooltip = StiLocalization.Get("FormPageSetup", "PageOrientationPortrait");
		biOrientationLandscape.Tooltip = StiLocalization.Get("FormPageSetup", "PageOrientationLandscape");
		biColumnsOne.Tooltip = StiLocalization.Get("FormDesigner", "ColumnsOne");
		biColumnsTwo.Tooltip = StiLocalization.Get("FormDesigner", "ColumnsTwo");
		biColumnsThree.Tooltip = StiLocalization.Get("FormDesigner", "ColumnsThree");
		((Control)ribbonBarPageSetup).Text = StiLocalization.Get("Toolbars", "ToolbarPageSetup");
	}

	public void LocalizeBarWatermarkText()
	{
		biWatermarkSettings.Text = StiLocalization.Get("PropertyMain", "Watermark");
	}

	public void LocalizeBarViewOptions()
	{
		biGridMode.Text = StiLocalization.Get("FormOptions", "GridMode");
		biGridModeDotes.Text = StiLocalization.Get("FormOptions", "GridDots");
		biGridModeLines.Text = StiLocalization.Get("FormOptions", "GridLines");
		biViewAlignToGrid.Text = StiLocalization.Get("MainMenu", "menuViewAlignToGrid");
		biViewShowGrid.Text = StiLocalization.Get("MainMenu", "menuViewShowGrid");
		biViewShowHeaders.Text = StiLocalization.Get("MainMenu", "menuViewShowHeaders");
		biViewShowOrder.Text = StiLocalization.Get("MainMenu", "menuViewShowOrder");
		biViewShowRulers.Text = StiLocalization.Get("MainMenu", "menuViewShowRulers");
		biQuickInfo.Text = StiLocalization.Get("MainMenu", "menuViewQuickInfo");
		biQuickInfoNone.Text = StiLocalization.Get("MainMenu", "menuViewQuickInfoNone");
		biQuickInfoShowComponentsNames.Text = StiLocalization.Get("MainMenu", "menuViewQuickInfoShowComponentsNames");
		biQuickInfoShowAliases.Text = StiLocalization.Get("MainMenu", "menuViewQuickInfoShowAliases");
		biQuickInfoShowContent.Text = StiLocalization.Get("MainMenu", "menuViewQuickInfoShowContent");
		biQuickInfoShowFields.Text = StiLocalization.Get("MainMenu", "menuViewQuickInfoShowFields");
		biQuickInfoShowFieldsOnly.Text = StiLocalization.Get("MainMenu", "menuViewQuickInfoShowFieldsOnly");
		biQuickInfoShowEvents.Text = StiLocalization.Get("MainMenu", "menuViewQuickInfoShowEvents");
		biQuickInfoOverlay.Text = StiLocalization.Get("MainMenu", "menuViewQuickInfoOverlay");
		biDockingPanels.Text = StiLocalization.Get("FormDesigner", "DockingPanels");
		((Control)ribbonBarViewOptions).Text = StiLocalization.Get("Toolbars", "ToolbarViewOptions");
		biShowToolbox.Text = StiLocalization.Get("Toolbox", "title");
		foreach (ButtonItem subItem in biDockingPanels.SubItems)
		{
			if (subItem.Tag is StiPanelService stiPanelService)
			{
				subItem.Text = stiPanelService.ServiceName;
			}
		}
	}

	public void LocalizeQuickBar()
	{
		biQuickReportSave.Text = StiLocalization.Get("MainMenu", "menuFileReportSave");
		biQuickReportSave.Tooltip = StiLocalization.Get("MainMenu", "menuFileReportSave");
		biQuickUndo.Text = StiLocalization.Get("MainMenu", "menuEditUndo");
		biQuickRedo.Text = StiLocalization.Get("MainMenu", "menuEditRedo");
		biShare.Tooltip = StiLocalization.Get("Cloud", "ShareWindowTitleNew");
	}

	public string BreakTextToLines(string text)
	{
		if (text.Contains(" "))
		{
			string[] array = text.Split(new char[1] { ' ' });
			switch (array.Length)
			{
			case 4:
				return array[0] + " " + array[1] + " \r\n" + array[2] + " " + array[3];
			case 3:
				return array[0] + " " + array[1] + " \r\n" + array[2];
			case 2:
				return array[0] + " \r\n" + array[1];
			}
		}
		return text;
	}
}
