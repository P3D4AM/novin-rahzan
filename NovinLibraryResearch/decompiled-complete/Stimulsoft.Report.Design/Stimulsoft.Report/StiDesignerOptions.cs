using System.Collections.Generic;
using System.Linq;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.ContextTools;
using Stimulsoft.Report.Design.Panels;
using Stimulsoft.Report.Design.Toolbars;
using Stimulsoft.Report.Design.Tools;
using Stimulsoft.Report.Design.Wizards;

namespace Stimulsoft.Report;

public sealed class StiDesignerOptions
{
	private static List<StiCTService> contextMenu;

	private static List<StiDesignerConfigService> designerConfig;

	private static List<StiMainMenuService> mainMenu;

	private static List<StiPanelService> panels;

	private static List<StiReportTemplatesService> reportTemplates;

	private static List<StiRibbonGuiService> ribbonGui;

	private static List<StiToolbarService> toolbars;

	private static List<StiToolService> tools;

	private static List<StiUndoRedoService> undoRedo;

	private static List<StiWizardService> wizards;

	private static object lockObject = new object();

	public static List<StiCTService> ContextMenu
	{
		get
		{
			lock (lockObject)
			{
				return contextMenu ?? (contextMenu = new List<StiCTService>
				{
					new StiAlignCTService(),
					new StiClearContentCTService(),
					new StiDesignComponentCTService(),
					new StiObjectCopyCTService(),
					new StiObjectCutCTService(),
					new StiObjectDeleteCTService(),
					new StiObjectPasteCTService(),
					new StiObjectSelectAllCTService(),
					new StiPropertiesCTService(),
					new StiTableCTService(),
					new StiOrderCTService(),
					new StiTemplatesCTService(),
					new StiSizeCTService(),
					new StiTextFormatCTService(),
					new StiInteractionCTService(),
					new StiTitleElementCTService()
				}.OrderBy((StiCTService c) => c.Position).ToList());
			}
		}
	}

	public static List<StiDesignerConfigService> DesignerConfig
	{
		get
		{
			lock (lockObject)
			{
				object obj = designerConfig;
				if (obj == null)
				{
					obj = new List<StiDesignerConfigService>
					{
						new StiDesignerConfigService()
					};
					designerConfig = (List<StiDesignerConfigService>)obj;
				}
				return (List<StiDesignerConfigService>)obj;
			}
		}
	}

	public static List<StiMainMenuService> MainMenu
	{
		get
		{
			lock (lockObject)
			{
				object obj = mainMenu;
				if (obj == null)
				{
					obj = new List<StiMainMenuService>
					{
						new StiMainMenuService()
					};
					mainMenu = (List<StiMainMenuService>)obj;
				}
				return (List<StiMainMenuService>)obj;
			}
		}
	}

	public static List<StiPanelService> Panels
	{
		get
		{
			lock (lockObject)
			{
				object obj = panels;
				if (obj == null)
				{
					obj = new List<StiPanelService>
					{
						new StiPropertiesPanelService(),
						new StiDictionaryPanelService(),
						new StiReportTreePanelService()
					};
					panels = (List<StiPanelService>)obj;
				}
				return (List<StiPanelService>)obj;
			}
		}
	}

	public static List<StiReportTemplatesService> ReportTemplates
	{
		get
		{
			lock (lockObject)
			{
				object obj = reportTemplates;
				if (obj == null)
				{
					obj = new List<StiReportTemplatesService>
					{
						new StiReportTemplatesService()
					};
					reportTemplates = (List<StiReportTemplatesService>)obj;
				}
				return (List<StiReportTemplatesService>)obj;
			}
		}
	}

	public static List<StiRibbonGuiService> RibbonGui
	{
		get
		{
			lock (lockObject)
			{
				object obj = ribbonGui;
				if (obj == null)
				{
					obj = new List<StiRibbonGuiService>
					{
						new StiRibbonGuiService()
					};
					ribbonGui = (List<StiRibbonGuiService>)obj;
				}
				return (List<StiRibbonGuiService>)obj;
			}
		}
	}

	public static List<StiToolbarService> Toolbars
	{
		get
		{
			lock (lockObject)
			{
				return toolbars ?? (toolbars = new List<StiToolbarService>
				{
					new StiBordersToolbarService(),
					new StiDockStyleToolbarService(),
					new StiFormattingToolbarService(),
					new StiLayoutToolbarService(),
					new StiStandardToolbarService(),
					new StiStyleToolbarService(),
					new StiTextFormatToolbarService(),
					new StiToolboxToolbarService(),
					new StiStatusBarService(),
					new StiRibbonToolboxToolbarService()
				}.OrderBy((StiToolbarService s) => s.Position).ToList());
			}
		}
	}

	public static List<StiToolService> Tools
	{
		get
		{
			lock (lockObject)
			{
				return tools ?? (tools = new List<StiToolService>
				{
					new StiCreateToolService(),
					new StiSelectToolService(),
					new StiStyleToolService(),
					new StiTextEditorToolService()
				}.OrderBy((StiToolService s) => s.Position).ToList());
			}
		}
	}

	public static List<StiUndoRedoService> UndoRedo
	{
		get
		{
			lock (lockObject)
			{
				object obj = undoRedo;
				if (obj == null)
				{
					obj = new List<StiUndoRedoService>
					{
						new StiUndoRedoService()
					};
					undoRedo = (List<StiUndoRedoService>)obj;
				}
				return (List<StiUndoRedoService>)obj;
			}
		}
	}

	public static List<StiWizardService> Wizards
	{
		get
		{
			lock (lockObject)
			{
				if (wizards == null)
				{
					wizards = new List<StiWizardService>();
					wizards.Add(new StiStandardWizardService());
					wizards.Add(new StiMasterDetailWizardService());
					wizards.Add(new StiLabelWizardService());
					wizards.Add(new StiChartWizardService());
					wizards.Add(new StiCrossTabWizardService());
				}
				return wizards;
			}
		}
	}
}
