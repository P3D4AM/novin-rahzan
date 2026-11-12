using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.App;
using Stimulsoft.Report.BarCodes;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Chart.Design;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Helpers;
using Stimulsoft.Report.Design.Tools;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Gauge.Helpers;
using Stimulsoft.Report.Maps;
using Stimulsoft.Report.Toolbox;

namespace Stimulsoft.Report.Design.Toolbars;

public class StiRibbonToolboxToolbarService : StiToolbarService
{
	private RibbonBar toolboxRibbonBar;

	private RibbonTabItem ribbonTabItemHome;

	private RibbonTabItem ribbonTabItemInsert;

	private StiPopup shapePopup;

	private bool showInsertTabDefault = true;

	public RibbonPanel RibbonPanel { get; set; }

	public bool IsForm
	{
		get
		{
			if (Designer != null && Report != null)
			{
				return Report.GetCurrentPage() is IStiForm;
			}
			return false;
		}
	}

	public bool IsScreen
	{
		get
		{
			if (Designer != null && Report != null)
			{
				return Report.GetCurrentPage() is IStiScreen;
			}
			return false;
		}
	}

	private bool IsDashboard
	{
		get
		{
			if (Designer != null && Report != null)
			{
				return Report.GetCurrentPage() is IStiDashboard;
			}
			return false;
		}
	}

	public override int Position => 10;

	public static StiRibbonToolboxToolbarService GetService()
	{
		return GetService(null);
	}

	public static StiRibbonToolboxToolbarService GetService(StiDesignerControl designer)
	{
		if (designer == null)
		{
			return StiDesignerOptions.Toolbars.FirstOrDefault((StiToolbarService c) => c.GetType() == typeof(StiRibbonToolboxToolbarService)) as StiRibbonToolboxToolbarService;
		}
		return designer.Services.GetServices(typeof(StiToolbarService)).OfType<StiRibbonToolboxToolbarService>().FirstOrDefault();
	}

	public override void Localize()
	{
		base.Localize();
		BuildingToolbox();
	}

	public override void Init(StiDesignerControl designer)
	{
		base.Init(designer);
		Designer.ResetingToolbox += OnResetingToolbox;
		Designer.BuidingToolbox += OnBuildingToolbox;
		Designer.RefreshDesigner += Refresh;
		RibbonPanel = ((Control)Designer.RibbonControl).Controls.Find("ribbonPanelInsert", false)[0] as RibbonPanel;
		IEnumerable<RibbonTabItem> source = Designer.RibbonControl.Items.OfType<RibbonTabItem>();
		ribbonTabItemHome = source.FirstOrDefault((RibbonTabItem x) => x.Name == "ribbonTabHome");
		ribbonTabItemInsert = source.FirstOrDefault((RibbonTabItem x) => x.Name == "ribbonTabInsert");
		BuildingToolbox();
		ribbonTabItemInsert.CheckedChanged += RibbonTabItemInsert_CheckedChanged;
	}

	public void ResetTools()
	{
		if (shapePopup != null)
		{
			((Control)shapePopup).Hide();
		}
		if (Designer.Services == null)
		{
			return;
		}
		StiServiceContainer services = Designer.Services.GetServices(typeof(StiToolService));
		if (Designer.Services != null)
		{
			foreach (StiToolService item in services)
			{
				item.Reset();
			}
		}
		foreach (object item2 in (ArrangedElementCollection)((Control)RibbonPanel).Controls)
		{
			if (!(item2 is RibbonBar ribbonBar))
			{
				continue;
			}
			foreach (object item3 in ribbonBar.Items)
			{
				UnCheckedButtons(item3);
			}
		}
	}

	private void UnCheckedButtons(object buttonObj)
	{
		if (!(buttonObj is ButtonItem buttonItem))
		{
			return;
		}
		buttonItem.Checked = false;
		foreach (object subItem in buttonItem.SubItems)
		{
			UnCheckedButtons(subItem);
		}
	}

	private void BuildingToolbox()
	{
		if (CheckRibbonTabVisible() && ribbonTabItemInsert != null && ribbonTabItemInsert.Checked)
		{
			((Control)RibbonPanel).SuspendLayout();
			((IEnumerable)((Control)RibbonPanel).Controls).OfType<Control>().ToList().ForEach(delegate(Control c)
			{
				((Component)(object)c).Dispose();
			});
			((Control)RibbonPanel).Controls.Clear();
			if (IsForm)
			{
				BuildingDialogsToolbox();
			}
			else if (IsScreen)
			{
				BuildingComponentUIsToolbox();
			}
			else if (IsDashboard)
			{
				BuildingDashboardsToolbox();
			}
			else
			{
				BuildingComponentsToolbox();
			}
			((Control)RibbonPanel).ResumeLayout();
		}
	}

	private void BuildingComponentUIsToolbox()
	{
		if (Designer == null)
		{
			return;
		}
		List<StiComponent> list = new List<StiComponent>();
		StiSettings.Load();
		CreateNewItemRibbonBar();
		IEnumerable<StiComponent> enumerable = StiOptions.Services.Components.Where((StiComponent s) => s.ServiceEnabled && (s is IStiScreen || s is IStiComponentUI));
		foreach (StiComponent item in enumerable)
		{
			Type type = item.GetType();
			StiToolboxAttribute[] array = (StiToolboxAttribute[])type.GetCustomAttributes(typeof(StiToolboxAttribute), inherit: true);
			if (array.Length != 0 && array[0].Enable && (!(type.GetCustomAttributes(typeof(StiEngineAttribute), inherit: true) is StiEngineAttribute[] array2) || array2.Length == 0 || array2[0].Version == StiEngineVersion.All || (Report != null && array2[0].Version == Report.EngineVersion)) && StiToolboxSetup.ComponentUIs.IsComponentVisible(item))
			{
				list.Add(item);
			}
		}
		toolboxRibbonBar = CreateRibbonBar(StiLocalization.Get("Report", "Components"));
		if (!toolboxRibbonBar.HotDragDrop)
		{
			toolboxRibbonBar.HotDragDrop = true;
			toolboxRibbonBar.StartDragDrop += ToolboxSetupRibbonBar_StartDragDrop;
		}
		foreach (StiComponent item2 in list)
		{
			ButtonItem button = null;
			AddRibbon(toolboxRibbonBar, ref button, null, null, item2.ServiceName, ButtonToolbox_Click);
			if (button != null)
			{
				button.Image = Designer.GetBigImageType(item2.GetType(), typeof(StiComponent));
				button.ImageSmall = Designer.GetImageType(item2.GetType(), typeof(StiComponent));
				button.Tag = item2;
				Designer.SetComponentToolTip(button, item2);
				button.AutoCheckOnClick = true;
			}
		}
		Designer.SelectTool.Activate();
		base.AddSeparatorToNextItem = true;
		string text = StiLocalization.Get("FormDesigner", "SetupToolbox");
		Bitmap image = StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.ToolBox.ToolBox" + StiScale.StepName + ".png");
		Bitmap image2 = StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.ToolBox.ToolBox" + StiScale.BigStepName + ".png");
		ButtonItem button2 = null;
		AddRibbon(toolboxRibbonBar, ref button2, (Image)(object)image2, (Image)(object)image, text, ButtonComponentUIsSetup_Click);
		if (Designer.IsRibbonGuiMode)
		{
			Graphics globalMeasureGraphics = StiReport.GlobalMeasureGraphics;
			Designer.SetToolTipOnlyHeaderEx(button2, globalMeasureGraphics, text, "user-manual/reports_designer_toolbox_shortcut_panel.htm");
		}
	}

	private void BuildingDashboardsToolbox()
	{
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Expected O, but got Unknown
		//IL_01fd: Expected O, but got Unknown
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Expected O, but got Unknown
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Expected O, but got Unknown
		//IL_040e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0418: Expected O, but got Unknown
		if (Designer == null)
		{
			return;
		}
		List<StiComponent> list = new List<StiComponent>();
		StiSettings.Load();
		CreateNewItemRibbonBar();
		IEnumerable<StiComponent> enumerable = StiOptions.Services.Components.Where((StiComponent s) => s.ServiceEnabled && s is IStiElement);
		foreach (StiComponent item in enumerable)
		{
			Type type = item.GetType();
			StiToolboxAttribute[] array = (StiToolboxAttribute[])type.GetCustomAttributes(typeof(StiToolboxAttribute), inherit: true);
			if (array.Length != 0 && array[0].Enable && (!(type.GetCustomAttributes(typeof(StiEngineAttribute), inherit: true) is StiEngineAttribute[] array2) || array2.Length == 0 || array2[0].Version == StiEngineVersion.All || (Report != null && array2[0].Version == Report.EngineVersion)) && StiToolboxSetup.Dashboards.IsComponentVisible(item))
			{
				list.Add(item);
			}
		}
		bool flag = true;
		bool flag2 = true;
		toolboxRibbonBar = CreateRibbonBar(StiLocalization.Get("Report", "Components"));
		if (!toolboxRibbonBar.HotDragDrop)
		{
			toolboxRibbonBar.HotDragDrop = true;
			toolboxRibbonBar.StartDragDrop += ToolboxSetupRibbonBar_StartDragDrop;
		}
		foreach (StiComponent item2 in list)
		{
			ButtonItem button = null;
			if (item2 is IStiShapeElement)
			{
				AddRibbon(toolboxRibbonBar, ref button, null, null, item2.ServiceName, CategoryShapeButton_Click);
				button.ShowArrow = true;
				button.RibbonWordWrap = false;
				button.AutoExpandOnClick = true;
				shapePopup = new StiPopup((Control)(object)new StiShapeToolboxControl(Designer, new MouseEventHandler(ShapeElementToolbox_Click), new MouseEventHandler(ShapeElementToolbox_MouseMove), showBaseShapes: false, showBorder: true));
				((Control)shapePopup).Tag = item2;
				((ToolStripDropDown)shapePopup).Closing += (ToolStripDropDownClosingEventHandler)delegate
				{
					button.Checked = false;
				};
				button.Image = Designer.GetBigImageType("StiShapeElement", typeof(StiComponent));
				button.ImageSmall = Designer.GetImageType("StiShapeElement", typeof(StiComponent));
				Designer.SetComponentToolTip(button, item2);
				continue;
			}
			if (item2 is IStiRegionMapElement || item2 is IStiOnlineMapElement)
			{
				if (flag)
				{
					flag = false;
					AddRibbon(toolboxRibbonBar, ref button, null, null, StiLocalization.Get("Components", "StiMap"), null, new MouseEventHandler(CategoryButton_Click));
					FillMapElementItems(button);
					button.Image = Designer.GetBigImageType(typeof(StiMap), typeof(StiComponent));
					button.ImageSmall = Designer.GetImageType(typeof(StiMap), typeof(StiComponent));
					button.ShowArrow = true;
					button.AutoExpandOnClick = true;
					button.PopupClose += delegate
					{
						button.Checked = false;
					};
					Designer.SetComponentToolTip(button, "StiMap", StiLocalization.Get("PropertyMain", "Maps"), StiLocalization.Get("HelpComponents", "StiMapCategory"));
					continue;
				}
			}
			else if (item2 is IStiListBoxElement || item2 is IStiComboBoxElement || item2 is IStiTreeViewElement || item2 is IStiTreeViewBoxElement || item2 is IStiDatePickerElement)
			{
				if (flag2)
				{
					flag2 = false;
					AddRibbon(toolboxRibbonBar, ref button, null, null, StiLocalization.Get("PropertyMain", "Filter"), null, new MouseEventHandler(CategoryButton_Click));
					FillFilterElementItems(button);
					button.Image = Designer.GetBigImageType("StiFilterElement", typeof(StiComponent));
					button.ImageSmall = Designer.GetImageType("StiFilterElement", typeof(StiComponent));
					button.ShowArrow = true;
					button.AutoExpandOnClick = true;
					button.PopupClose += delegate
					{
						button.Checked = false;
					};
					Designer.SetComponentToolTip(button, "StiFilterElement", StiLocalization.Get("PropertyMain", "Filter"), StiLocalization.Get("HelpComponents", "StiFilterCategory"));
					continue;
				}
			}
			else
			{
				AddRibbon(toolboxRibbonBar, ref button, null, null, item2.ServiceName, ButtonToolbox_Click);
			}
			if (button != null)
			{
				button.Image = Designer.GetBigImageType(item2.GetType(), typeof(StiComponent));
				button.ImageSmall = Designer.GetImageType(item2.GetType(), typeof(StiComponent));
				button.Tag = item2;
				Designer.SetComponentToolTip(button, item2);
				button.AutoCheckOnClick = true;
			}
		}
		Designer.SelectTool.Activate();
		base.AddSeparatorToNextItem = true;
		string text = StiLocalization.Get("FormDesigner", "SetupToolbox");
		Bitmap image = StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.ToolBox.ToolBox" + StiScale.StepName + ".png");
		Bitmap image2 = StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.ToolBox.ToolBox" + StiScale.BigStepName + ".png");
		ButtonItem button2 = null;
		AddRibbon(toolboxRibbonBar, ref button2, (Image)(object)image2, (Image)(object)image, text, ButtonDashboardsSetup_Click);
		if (Designer.IsRibbonGuiMode)
		{
			Graphics globalMeasureGraphics = StiReport.GlobalMeasureGraphics;
			Designer.SetToolTipOnlyHeaderEx(button2, globalMeasureGraphics, text, "user-manual/reports_designer_toolbox_shortcut_panel.htm");
		}
	}

	private void BuildingDialogsToolbox()
	{
		List<StiComponent> list = new List<StiComponent>();
		if (Designer == null)
		{
			return;
		}
		StiSettings.Load();
		CreateNewItemRibbonBar();
		IEnumerable<StiComponent> enumerable = StiOptions.Services.Components.Where((StiComponent s) => s.ServiceEnabled && s is IStiReportControl);
		foreach (StiComponent item in enumerable)
		{
			Type type = item.GetType();
			StiToolboxAttribute[] array = (StiToolboxAttribute[])type.GetCustomAttributes(typeof(StiToolboxAttribute), inherit: true);
			if (array.Length != 0 && array[0].Enable && (!(type.GetCustomAttributes(typeof(StiEngineAttribute), inherit: true) is StiEngineAttribute[] array2) || array2.Length == 0 || array2[0].Version == StiEngineVersion.All || (Report != null && array2[0].Version == Report.EngineVersion)) && StiToolboxSetup.Dialogs.IsComponentVisible(item))
			{
				list.Add(item);
			}
		}
		RibbonBar ribbonBar = CreateRibbonBar(StiLocalization.Get("Report", "Components"));
		if (!ribbonBar.HotDragDrop)
		{
			ribbonBar.HotDragDrop = true;
			ribbonBar.StartDragDrop += ToolboxSetupRibbonBar_StartDragDrop;
		}
		foreach (StiComponent item2 in list)
		{
			ButtonItem button = null;
			Image bigImageType = Designer.GetBigImageType(item2.GetType(), typeof(StiComponent));
			Image imageType = Designer.GetImageType(item2.GetType(), typeof(StiComponent));
			AddRibbon(ribbonBar, ref button, bigImageType, imageType, item2.ServiceName, ButtonToolbox_Click);
			button.PopupPositionAdjusted = false;
			button.SubItemsExpandWidth = StiScale.XXI(14.0);
			button.ButtonStyle = eButtonStyle.ImageAndText;
			button.Tag = item2;
			button.Text = item2.ServiceName;
			Designer.SetComponentToolTip(button, item2);
			button.Click += ButtonToolbox_Click;
			button.AutoCheckOnClick = true;
		}
		Designer.SelectTool.Activate();
		base.AddSeparatorToNextItem = true;
		string text = StiLocalization.Get("FormDesigner", "SetupToolbox");
		Bitmap image = StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.ToolBox.ToolBox" + StiScale.BigStepName + ".png");
		Bitmap image2 = StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.ToolBox.ToolBox" + StiScale.StepName + ".png");
		ButtonItem button2 = null;
		AddRibbon(ribbonBar, ref button2, (Image)(object)image, (Image)(object)image2, text, ButtonDialogsSetup_Click);
		if (Designer.IsRibbonGuiMode)
		{
			Graphics globalMeasureGraphics = StiReport.GlobalMeasureGraphics;
			Designer.SetToolTipOnlyHeaderEx(button2, globalMeasureGraphics, text, "user-manual/reports_designer_toolbox_shortcut_panel.htm");
		}
	}

	private RibbonBar CreateRibbonBar(string header)
	{
		RibbonBar obj = new RibbonBar
		{
			AutoOverflowEnabled = true,
			ContainerControlProcessDialogKey = true,
			DialogLauncherVisible = false
		};
		((Control)obj).Dock = (DockStyle)3;
		((Control)obj).Text = header;
		RibbonBar ribbonBar = obj;
		((Control)ribbonBar).MinimumSize = new Size(TextRenderer.MeasureText(header, ((Control)ribbonBar).Font).Width + StiScale.I6, 0);
		((Control)RibbonPanel).Controls.Add((Control)(object)ribbonBar);
		return ribbonBar;
	}

	private void CreateNewItemRibbonBar()
	{
		bool showPageNew = StiOptions.Designer.TabsContextMenu.ShowPageNew;
		bool flag = StiSettings.GetBool("Designer", "ShowDialogFormsV1", nullValue: false) && StiOptions.Designer.TabsContextMenu.ShowFormNew;
		bool flag2 = StiOptions.Designer.TabsContextMenu.ShowScreenNew && StiAppDesignAssembly.IsAssemblyLoaded;
		bool flag3 = StiOptions.Designer.TabsContextMenu.ShowDashboardNew && StiDashboardDesignAssembly.IsAssemblyLoaded;
		if (showPageNew || flag || flag3)
		{
			RibbonBar ribbonBar = CreateRibbonBar(StiLocalization.Get("FormDictionaryDesigner", "NewItem"));
			if (showPageNew)
			{
				ButtonItem button = null;
				Image bigImageType = Designer.GetBigImageType(typeof(StiPage), typeof(StiComponent));
				Image imageType = Designer.GetImageType(typeof(StiPage), typeof(StiComponent));
				AddRibbon(ribbonBar, ref button, bigImageType, imageType, StiLocalization.Get("Components", "StiPage"), NewPage_Click);
			}
			if (flag3)
			{
				ButtonItem button2 = null;
				Bitmap imageByName = StiResourcesHelper.GetImageByName("Stimulsoft.Report.Design.Resources.Office2013.ToolBox.StiDashboard" + StiScale.BigStepName + ".png");
				Bitmap imageByName2 = StiResourcesHelper.GetImageByName("Stimulsoft.Report.Design.Resources.Office2013.ToolBox.StiDashboard" + StiScale.StepName + ".png");
				AddRibbon(ribbonBar, ref button2, (Image)(object)imageByName, (Image)(object)imageByName2, StiLocalization.Get("Components", "StiDashboard"), NewDashboard_Click);
			}
			if (flag2)
			{
				ButtonItem button3 = null;
				Bitmap imageByName3 = StiResourcesHelper.GetImageByName("Stimulsoft.Report.Design.Resources.Office2013.ToolBox.StiScreen" + StiScale.BigStepName + ".png");
				Bitmap imageByName4 = StiResourcesHelper.GetImageByName("Stimulsoft.Report.Design.Resources.Office2013.ToolBox.StiScreen" + StiScale.StepName + ".png");
				AddRibbon(ribbonBar, ref button3, (Image)(object)imageByName3, (Image)(object)imageByName4, StiLocalization.Get("Components", "StiScreen"), NewScreen_Click);
			}
			if (flag)
			{
				string hint = StiLocalization.Get("Dialogs", "StiForm") + " (" + StiLocalization.Get("Report", "Obsoleted") + ")";
				ButtonItem button4 = null;
				Image bigImageType2 = Designer.GetBigImageType(typeof(StiForm), typeof(StiComponent));
				Image imageType2 = Designer.GetImageType(typeof(StiForm), typeof(StiComponent));
				AddRibbon(ribbonBar, ref button4, bigImageType2, imageType2, hint, NewForm_Click);
			}
		}
	}

	private void BuildingComponentsToolbox()
	{
		//IL_067e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0688: Expected O, but got Unknown
		//IL_05dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e6: Expected O, but got Unknown
		//IL_073f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0749: Expected O, but got Unknown
		List<StiComponent> list = new List<StiComponent>();
		Graphics globalMeasureGraphics = StiReport.GlobalMeasureGraphics;
		toolboxRibbonBar = null;
		RibbonBar ribbonBar = null;
		if (Designer != null)
		{
			StiSettings.Load();
			CreateNewItemRibbonBar();
			Hashtable hashtable = new Hashtable();
			IEnumerable<StiComponent> mainList = StiOptions.Services.Components.Where((StiComponent s) => s.ServiceEnabled && !(s is IStiReportControl) && !(s is IStiElement));
			mainList = StiDesignerSpecificationHelper.CheckShowComponents(mainList);
			RibbonBar ribbonBar2 = CreateRibbonBar(StiLocalization.Get("PropertyMain", "Categories"));
			foreach (StiComponent item in mainList)
			{
				if (item is StiVerticalLinePrimitive || item is StiHorizontalLinePrimitive || item is StiRoundedRectanglePrimitive || item is StiRectanglePrimitive || (item is StiWinControl && !StiOptions.Designer.AllowUseWinControl) || item is StiPage)
				{
					continue;
				}
				Type type = item.GetType();
				StiToolboxAttribute[] array = (StiToolboxAttribute[])type.GetCustomAttributes(typeof(StiToolboxAttribute), inherit: true);
				if (array.Length == 0 || !array[0].Enable || (type.GetCustomAttributes(typeof(StiEngineAttribute), inherit: true) is StiEngineAttribute[] array2 && array2.Length != 0 && array2[0].Version != 0 && (Report == null || array2[0].Version != Report.EngineVersion)) || (item.GetType() == typeof(StiShape) && !StiToolboxSetup.Reports.ShowShapesCategory) || (item.GetType() == typeof(StiChart) && !StiToolboxSetup.Reports.ShowChartCategory) || (item.GetType() == typeof(StiGauge) && !StiToolboxSetup.Reports.ShowGaugeCategory) || (item.GetType() == typeof(StiMap) && !StiToolboxSetup.Reports.ShowMapCategory) || (item.GetType() == typeof(StiBarCode) && !StiToolboxSetup.Reports.ShowBarCodesCategory))
				{
					continue;
				}
				string text = item.LocalizedCategory;
				if (item is StiBarCode)
				{
					text = item.LocalizedName;
				}
				ButtonItem button = hashtable[text] as ButtonItem;
				if (button == null)
				{
					bool flag = false;
					Image image;
					Image image2;
					if (text == StiLocalization.Get("Report", "Components"))
					{
						flag = !StiToolboxSetup.Reports.ShowComponentsCategory;
						image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.ToolBox.catComponents" + StiScale.StepName + ".png");
						image2 = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.ToolBox.catComponents" + StiScale.BigStepName + ".png");
					}
					else if (text == StiLocalization.Get("Report", "Bands"))
					{
						flag = !StiToolboxSetup.Reports.ShowBandsCategory;
						image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.ToolBox.catBands" + StiScale.StepName + ".png");
						image2 = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.ToolBox.catBands" + StiScale.BigStepName + ".png");
					}
					else if (text == StiLocalization.Get("Report", "CrossBands"))
					{
						flag = !StiToolboxSetup.Reports.ShowCrossBandsCategory;
						image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.ToolBox.catCrossBands" + StiScale.StepName + ".png");
						image2 = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.ToolBox.catCrossBands" + StiScale.BigStepName + ".png");
					}
					else if (text == StiLocalization.Get("Report", "Shapes"))
					{
						image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.ToolBox.StiShape" + StiScale.StepName + ".png");
						image2 = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.ToolBox.StiShape" + StiScale.BigStepName + ".png");
					}
					else if (text == StiLocalization.Get("Components", "StiBarCode"))
					{
						image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.ToolBox.StiBarCode" + StiScale.StepName + ".png");
						image2 = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.ToolBox.StiBarCode" + StiScale.BigStepName + ".png");
					}
					else if (text == StiLocalization.Get("Components", "StiChart"))
					{
						image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.ToolBox.StiChart" + StiScale.StepName + ".png");
						image2 = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.ToolBox.StiChart" + StiScale.BigStepName + ".png");
					}
					else if (text == StiLocalization.Get("Components", "StiMap"))
					{
						image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.ToolBox.StiMap" + StiScale.StepName + ".png");
						image2 = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.ToolBox.StiMap" + StiScale.BigStepName + ".png");
					}
					else if (text == StiLocalization.Get("Components", "StiGauge"))
					{
						image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.ToolBox.StiGauge" + StiScale.StepName + ".png");
						image2 = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.ToolBox.StiGauge" + StiScale.BigStepName + ".png");
					}
					else
					{
						StiServiceCategoryBitmapAttribute[] array3 = type.GetCustomAttributes(typeof(StiServiceCategoryBitmapAttribute), inherit: true) as StiServiceCategoryBitmapAttribute[];
						image2 = StiImageResizer.GetImage((Image)(object)array3[0].GetImage());
						image = StiImageResizer.GetImage((Image)(object)array3[0].GetImage());
					}
					if (!flag)
					{
						AddRibbon(ribbonBar2, ref button, image2, image, text, null, new MouseEventHandler(CategoryButton_Click));
						button.RibbonWordWrap = true;
						button.AutoExpandOnClick = true;
						if (Designer.IsRibbonGuiMode)
						{
							Designer.SetToolTipOnlyHeaderEx(button, globalMeasureGraphics, text, "user-manual/reports_designer_toolbox.htm");
						}
						hashtable[text] = button;
					}
				}
				ButtonItem buttonItem = (ButtonItem)hashtable[text];
				if (buttonItem != null)
				{
					if (item is StiChart)
					{
						FillChartItems(buttonItem);
					}
					else if (item is StiMap)
					{
						button.Tag = item;
						button.Click += MapCategoryButton_Click;
						button.MouseMove += new MouseEventHandler(SubButton_MouseMove);
					}
					else if (item is StiGauge)
					{
						FillGaugeItems(button);
					}
					else if (item is StiBarCode)
					{
						FillBarcodeItems(buttonItem);
					}
					else if (item is StiShape)
					{
						button.ShowArrow = true;
						CreateShapeCategory(button);
					}
					else if (!(item is StiPrimitive))
					{
						ButtonItem buttonItem2 = new ButtonItem();
						buttonItem2.Image = Designer.GetImageType(type, typeof(StiComponent));
						buttonItem2.Text = item.ServiceName;
						buttonItem2.Tag = item;
						ButtonItem buttonItem3 = buttonItem2;
						buttonItem3.Click += ButtonToolbox_Click;
						buttonItem3.MouseMove += new MouseEventHandler(SubButton_MouseMove);
						Designer.SetComponentToolTip(buttonItem3, item);
						buttonItem.SubItems.Add(buttonItem3);
					}
				}
				if (StiToolboxSetup.Reports.IsComponentVisible(item))
				{
					list.Add(item);
				}
			}
			if (list.Count > 0)
			{
				toolboxRibbonBar = CreateRibbonBar(StiLocalization.Get("Report", "Components"));
				if (!toolboxRibbonBar.HotDragDrop)
				{
					toolboxRibbonBar.HotDragDrop = true;
					toolboxRibbonBar.StartDragDrop += ToolboxSetupRibbonBar_StartDragDrop;
				}
				foreach (StiComponent item2 in list)
				{
					if (!(item2 is StiChart) && !(item2 is StiMap) && !(item2 is StiGauge) && !(item2 is StiShape) && !(item2 is StiVerticalLinePrimitive) && !(item2 is StiHorizontalLinePrimitive) && !(item2 is StiRoundedRectanglePrimitive) && !(item2 is StiRectanglePrimitive))
					{
						ButtonItem button2 = null;
						AddRibbon(toolboxRibbonBar, ref button2, null, null, item2.ServiceName, ButtonToolbox_Click);
						button2.Tag = item2;
						button2.Image = Designer.GetBigImageType(item2.GetType(), typeof(StiComponent));
						button2.ImageSmall = Designer.GetImageType(item2.GetType(), typeof(StiComponent));
						Designer.SetComponentToolTip(button2, item2);
						button2.AutoCheckOnClick = true;
					}
				}
			}
			Designer.SelectTool.Activate();
		}
		if (toolboxRibbonBar == null)
		{
			toolboxRibbonBar = CreateRibbonBar(StiLocalization.Get("Report", "Components"));
		}
		else
		{
			base.AddSeparatorToNextItem = true;
		}
		if (ribbonBar == null)
		{
			ribbonBar = CreateRibbonBar("");
		}
		else
		{
			base.AddSeparatorToNextItem = true;
		}
		string text2 = StiLocalization.Get("FormDesigner", "SetupToolbox");
		Bitmap image3 = StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.ToolBox.ToolBox" + StiScale.StepName + ".png");
		Bitmap image4 = StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.ToolBox.ToolBox" + StiScale.BigStepName + ".png");
		ButtonItem button3 = null;
		AddRibbon(ribbonBar, ref button3, (Image)(object)image4, (Image)(object)image3, text2, ButtonReportsSetup_Click);
		if (Designer.IsRibbonGuiMode)
		{
			Designer.SetToolTipOnlyHeaderEx(button3, globalMeasureGraphics, text2, "user-manual/reports_designer_toolbox_shortcut_panel.htm");
		}
	}

	protected void AddRibbon(RibbonBar ribbonBar, ref ButtonItem button, Image image, Image imageSmall, string hint, EventHandler handler, MouseEventHandler mouseEvent = null)
	{
		button = new ButtonItem
		{
			Image = image,
			ImageSmall = imageSmall,
			BeginGroup = base.AddSeparatorToNextItem,
			ImagePosition = eImagePosition.Top,
			Text = StiHint.CreateHint(hint)
		};
		button.MouseDown += mouseEvent;
		button.Click += handler;
		base.AddSeparatorToNextItem = false;
		ribbonBar.Items.Add(button);
	}

	private void FillBarcodeItems(ButtonItem categoryButton)
	{
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Expected O, but got Unknown
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Expected O, but got Unknown
		LabelItem item = new LabelItem
		{
			BackColor = Color.FromArgb(255, 238, 238, 238),
			PaddingTop = 2,
			PaddingBottom = 2,
			Text = StiLocalization.Get("Components", "StiBarCode")
		};
		categoryButton.SubItems.Add(item);
		StiInfographicsToolboxInfo[] barCodesToolboxItems = StiToolboxHelper.GetBarCodesToolboxItems();
		StiInfographicsToolboxInfo[] array = barCodesToolboxItems;
		foreach (StiInfographicsToolboxInfo stiInfographicsToolboxInfo in array)
		{
			ButtonItem buttonItem = new ButtonItem
			{
				Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.Barcode." + stiInfographicsToolboxInfo.Icon + StiScale.StepName + ".png"),
				Text = stiInfographicsToolboxInfo.LocName,
				AutoExpandOnClick = true
			};
			categoryButton.SubItems.Add(buttonItem);
			if (!(stiInfographicsToolboxInfo.Tag is List<StiBarCodeTypeService> list))
			{
				continue;
			}
			foreach (StiBarCodeTypeService item2 in list)
			{
				Image image;
				try
				{
					image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.Barcode." + item2.ComponentId.ToString() + StiScale.StepName + ".png");
				}
				catch
				{
					image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.Barcode.StiPharmacodeBarCodeType" + StiScale.StepName + ".png");
				}
				ButtonItem buttonItem2 = new ButtonItem
				{
					Image = image,
					Text = item2.ServiceName,
					Tag = item2,
					BeginGroup = stiInfographicsToolboxInfo.BeginGroup
				};
				buttonItem2.MouseDown += new MouseEventHandler(ButtonBarCode_Click);
				buttonItem2.MouseMove += new MouseEventHandler(SubButton_MouseMove);
				buttonItem.SubItems.Add(buttonItem2);
			}
		}
	}

	private void FillChartItems(ButtonItem categoryButton)
	{
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Expected O, but got Unknown
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Expected O, but got Unknown
		LabelItem item = new LabelItem
		{
			BackColor = Color.FromArgb(255, 238, 238, 238),
			PaddingTop = 2,
			PaddingBottom = 2,
			Text = StiLocalization.Get("Components", "StiChart")
		};
		categoryButton.SubItems.Add(item);
		StiInfographicsToolboxInfo[] chartToolboxItems = StiToolboxHelper.GetChartToolboxItems();
		StiInfographicsToolboxInfo[] array = chartToolboxItems;
		foreach (StiInfographicsToolboxInfo stiInfographicsToolboxInfo in array)
		{
			ButtonItem buttonItem = new ButtonItem
			{
				Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.Chart." + stiInfographicsToolboxInfo.Icon + ".png"),
				Text = stiInfographicsToolboxInfo.LocName,
				AutoExpandOnClick = true
			};
			categoryButton.SubItems.Add(buttonItem);
			if (stiInfographicsToolboxInfo.Infos == null)
			{
				continue;
			}
			foreach (StiInfographicsToolboxInfo info in stiInfographicsToolboxInfo.Infos)
			{
				ButtonItem buttonItem2 = new ButtonItem
				{
					Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.Chart." + info.Icon + ".png"),
					Text = info.LocName,
					Tag = info.Tag,
					BeginGroup = stiInfographicsToolboxInfo.BeginGroup
				};
				buttonItem2.MouseDown += new MouseEventHandler(ButtonChartSeries_Click);
				buttonItem2.MouseMove += new MouseEventHandler(SubButton_MouseMove);
				buttonItem.SubItems.Add(buttonItem2);
			}
		}
	}

	private void FillMapElementItems(ButtonItem categoryButton)
	{
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		ResetTools();
		LabelItem item = new LabelItem
		{
			BackColor = Color.FromArgb(255, 238, 238, 238),
			PaddingTop = 2,
			PaddingBottom = 2,
			Text = StiLocalization.Get("Components", "StiMap")
		};
		categoryButton.SubItems.Add(item);
		List<StiComponent> list = StiOptions.Services.Components.Where((StiComponent s) => s.ServiceEnabled && (s is IStiRegionMapElement || s is IStiOnlineMapElement)).ToList();
		foreach (StiComponent item2 in list)
		{
			if (StiToolboxSetup.Dashboards.IsComponentVisible(item2))
			{
				ButtonItem button = null;
				AddSub(ref button, categoryButton, null, item2.ServiceName, ButtonToolbox_Click, new MouseEventHandler(SubButton_MouseMove));
				button.Tag = item2;
				button.Image = Designer.GetBigImageType(item2.GetType(), typeof(StiComponent));
				button.ImageSmall = Designer.GetImageType(item2.GetType(), typeof(StiComponent));
				button.Text = item2.ServiceName;
				button.AutoCheckOnClick = true;
				Designer.SetComponentToolTip(button, item2);
			}
		}
	}

	private void FillFilterElementItems(ButtonItem categoryButton)
	{
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Expected O, but got Unknown
		ResetTools();
		LabelItem item = new LabelItem
		{
			BackColor = Color.FromArgb(255, 238, 238, 238),
			PaddingTop = 2,
			PaddingBottom = 2,
			Text = StiLocalization.Get("PropertyMain", "FilterElements")
		};
		categoryButton.SubItems.Add(item);
		List<StiComponent> source = StiOptions.Services.Components.Where((StiComponent s) => s.ServiceEnabled && (s is IStiListBoxElement || s is IStiComboBoxElement || s is IStiTreeViewElement || s is IStiTreeViewBoxElement || s is IStiDatePickerElement)).ToList();
		foreach (StiComponent item2 in source.OrderBy((StiComponent s) => s.GetType().Name))
		{
			if (StiToolboxSetup.Dashboards.IsComponentVisible(item2))
			{
				ButtonItem button = null;
				AddSub(ref button, categoryButton, null, item2.ServiceName, ButtonToolbox_Click, new MouseEventHandler(SubButton_MouseMove));
				button.Tag = item2;
				button.Image = Designer.GetBigImageType(item2.GetType(), typeof(StiComponent));
				button.ImageSmall = Designer.GetImageType(item2.GetType(), typeof(StiComponent));
				button.Text = item2.ServiceName;
				button.AutoCheckOnClick = true;
				Designer.SetComponentToolTip(button, item2);
			}
		}
	}

	private void FillGaugeItems(ButtonItem categoryButton)
	{
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Expected O, but got Unknown
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Expected O, but got Unknown
		LabelItem item = new LabelItem
		{
			BackColor = Color.FromArgb(255, 238, 238, 238),
			PaddingTop = 2,
			PaddingBottom = 2,
			Text = StiLocalization.Get("Components", "StiGauge")
		};
		categoryButton.SubItems.Add(item);
		StiInfographicsToolboxInfo[] gaugeToolboxItems = StiToolboxHelper.GetGaugeToolboxItems(allowGaugeV2: true);
		StiInfographicsToolboxInfo[] array = gaugeToolboxItems;
		foreach (StiInfographicsToolboxInfo stiInfographicsToolboxInfo in array)
		{
			ButtonItem buttonItem = new ButtonItem
			{
				Tag = stiInfographicsToolboxInfo.Tag,
				Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.Gauge." + stiInfographicsToolboxInfo.Icon + ".png"),
				Text = stiInfographicsToolboxInfo.LocName
			};
			buttonItem.MouseDown += new MouseEventHandler(ButtonGaugeSeries_Click);
			buttonItem.MouseMove += new MouseEventHandler(SubButton_MouseMove);
			categoryButton.SubItems.Add(buttonItem);
		}
	}

	private void CreateShapeCategory(ButtonItem categoryButton)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0038: Expected O, but got Unknown
		categoryButton.Click += CategoryShapeButton_Click;
		shapePopup = new StiPopup((Control)(object)new StiShapeToolboxControl(Designer, new MouseEventHandler(ButtonToolbox_Click), new MouseEventHandler(SubButton_MouseMove), showBaseShapes: true, showBorder: true));
	}

	internal bool CheckRibbonTabVisible()
	{
		bool @bool = StiSettings.GetBool("CloudDesigner", "ShowInsertTab", showInsertTabDefault);
		if (ribbonTabItemHome != null && !@bool)
		{
			ribbonTabItemHome.Select();
		}
		if (ribbonTabItemInsert != null)
		{
			ribbonTabItemInsert.Visible = @bool;
		}
		return @bool;
	}

	private void MapCategoryButton_Click(object sender, EventArgs e)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Invalid comparison between Unknown and I4
		try
		{
			StiMapCategoriesForm stiMapCategoriesForm = new StiMapCategoriesForm(Report);
			try
			{
				Designer.AddOwnedForm((Form)(object)stiMapCategoriesForm);
				if ((int)((Form)stiMapCategoriesForm).ShowDialog() == 1 && !string.IsNullOrEmpty(stiMapCategoriesForm.MapID))
				{
					ResetTools();
					ButtonItem buttonItem = (ButtonItem)sender;
					buttonItem.Checked = true;
					StiMap componentForCreating = new StiMap
					{
						MapIdent = stiMapCategoriesForm.MapID
					};
					Designer.CreateTool.ComponentForCreating = componentForCreating;
					Designer.CurrentTool = Designer.CreateTool;
					Designer.CreateTool.Action = StiCreateAction.CreateComponent;
					buttonItem.Checked = true;
					Designer.CreateTool.InvokeActivated(buttonItem, e);
				}
			}
			finally
			{
				((IDisposable)stiMapCategoriesForm)?.Dispose();
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ToolboxSetupRibbonBar_StartDragDrop(object sender, EventArgs e)
	{
		Designer.SelectTool.Activate();
		StiComponent stiComponent = null;
		if (sender is StiComponent)
		{
			stiComponent = sender as StiComponent;
		}
		else if (sender is BaseItem)
		{
			stiComponent = ((BaseItem)sender).Tag as StiComponent;
		}
		else if (sender is StiToolboxButton)
		{
			stiComponent = ((Control)(StiToolboxButton)sender).Tag as StiComponent;
		}
		Designer.SelectTool.IsDragDropFromToolBox = true;
		if (stiComponent != null)
		{
			StiDesignerDragDrop.StartDragDrop(Designer, stiComponent, (Control)(object)RibbonPanel);
		}
		else
		{
			StiDesignerDragDrop.StartDragDrop(Designer, (Control)(object)RibbonPanel);
		}
		Designer.SelectTool.IsDragDropFromToolBox = false;
	}

	private void SubButton_MouseMove(object sender, MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)e.Button == 1048576)
		{
			toolboxRibbonBar.InvokeStartDragDrop(sender);
		}
	}

	private void RibbonTabItemInsert_CheckedChanged(object sender, EventArgs e)
	{
		BuildingToolbox();
	}

	private void Refresh(object sender, EventArgs e)
	{
		if (RibbonPanel != null)
		{
			((Control)RibbonPanel).Enabled = Report != null;
		}
	}

	private void OnResetingToolbox(object sender, EventArgs e)
	{
		ResetTools();
	}

	private void OnBuildingToolbox(object s, EventArgs e)
	{
		if (Designer.TabControl.SelectedTabIndex != Designer.PagePreviewIndex && CheckRibbonTabVisible())
		{
			BuildingToolbox();
		}
	}

	private void CategoryButton_Click(object sender, EventArgs e)
	{
		Designer.CreateTool.ComponentForCreating = null;
	}

	private void CategoryShapeButton_Click(object sender, EventArgs e)
	{
		Designer.CreateTool.ComponentForCreating = null;
		ButtonItem buttonItem = sender as ButtonItem;
		((ToolStripDropDown)shapePopup).Show(new Point(buttonItem.ScreenRectangle.Left, buttonItem.ScreenRectangle.Bottom), (ToolStripDropDownDirection)5);
	}

	private void NewPage_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokePageNew(Designer);
	}

	private void NewDashboard_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokeDashboardNew(Designer);
	}

	private void NewScreen_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokeScreenNew(Designer);
	}

	private void NewForm_Click(object sender, EventArgs e)
	{
		StiActions.MainMenu.InvokeFormNew(Designer);
	}

	private void ButtonToolbox_Click(object sender, EventArgs e)
	{
		try
		{
			ResetTools();
			ButtonItem buttonItem = sender as ButtonItem;
			if (buttonItem != null)
			{
				buttonItem.Checked = true;
				if (!(buttonItem.Tag is StiAction))
				{
					Designer.CreateTool.ComponentForCreating = buttonItem.Tag as StiComponent;
					Designer.CurrentTool = Designer.CreateTool;
					Designer.CreateTool.Action = StiCreateAction.CreateComponent;
					buttonItem.Checked = true;
					Designer.CreateTool.InvokeActivated(buttonItem, e);
				}
				return;
			}
			object obj = null;
			if (sender is StiToolboxButton stiToolboxButton)
			{
				obj = ((Control)stiToolboxButton).Tag;
			}
			if (!(obj is StiAction))
			{
				Designer.CreateTool.ComponentForCreating = obj as StiComponent;
				Designer.CurrentTool = Designer.CreateTool;
				Designer.CreateTool.Action = StiCreateAction.CreateComponent;
				Designer.CreateTool.InvokeActivated(buttonItem, e);
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ShapeElementToolbox_MouseMove(object sender, MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)e.Button != 1048576)
		{
			return;
		}
		try
		{
			IStiShapeElement stiShapeElement = ((Control)shapePopup).Tag as IStiShapeElement;
			object obj = null;
			if (sender is StiToolboxButton stiToolboxButton)
			{
				obj = ((Control)stiToolboxButton).Tag;
			}
			if (!(obj is StiAction))
			{
				StiShape stiShape = obj as StiShape;
				stiShapeElement.ShapeType = stiShape.ShapeType;
				sender = stiShapeElement;
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
		toolboxRibbonBar.InvokeStartDragDrop(sender);
		Designer.CreateTool.ComponentForCreating = null;
	}

	private void ShapeElementToolbox_Click(object sender, EventArgs e)
	{
		try
		{
			ResetTools();
			IStiShapeElement stiShapeElement = ((Control)shapePopup).Tag as IStiShapeElement;
			object obj = null;
			if (sender is StiToolboxButton stiToolboxButton)
			{
				obj = ((Control)stiToolboxButton).Tag;
			}
			if (!(obj is StiAction))
			{
				stiShapeElement.ShapeType = (obj as StiShape)?.ShapeType;
				Designer.CreateTool.ComponentForCreating = stiShapeElement as StiComponent;
				Designer.CurrentTool = Designer.CreateTool;
				Designer.CreateTool.Action = StiCreateAction.CreateComponent;
				Designer.CreateTool.InvokeActivated(null, e);
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ButtonDashboardsSetup_Click(object sender, EventArgs e)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Invalid comparison between Unknown and I4
		try
		{
			StiComponentsToolboxForm stiComponentsToolboxForm = new StiComponentsToolboxForm(StiToolboxMode.Dashboards);
			try
			{
				Designer.AddOwnedForm((Form)(object)stiComponentsToolboxForm);
				if ((int)((Form)stiComponentsToolboxForm).ShowDialog() == 1)
				{
					Designer.InvokeBuidingToolbox();
					RibbonControl ribbonControl = Designer.RibbonControl;
					if (ribbonControl != null)
					{
						((Control)ribbonControl).Refresh();
					}
					((Control)Designer).Refresh();
				}
			}
			finally
			{
				((IDisposable)stiComponentsToolboxForm)?.Dispose();
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ButtonComponentUIsSetup_Click(object sender, EventArgs e)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Invalid comparison between Unknown and I4
		try
		{
			StiComponentsToolboxForm stiComponentsToolboxForm = new StiComponentsToolboxForm(StiToolboxMode.ComponentUIs);
			try
			{
				Designer.AddOwnedForm((Form)(object)stiComponentsToolboxForm);
				if ((int)((Form)stiComponentsToolboxForm).ShowDialog() == 1)
				{
					Designer.InvokeBuidingToolbox();
					RibbonControl ribbonControl = Designer.RibbonControl;
					if (ribbonControl != null)
					{
						((Control)ribbonControl).Refresh();
					}
					((Control)Designer).Refresh();
				}
			}
			finally
			{
				((IDisposable)stiComponentsToolboxForm)?.Dispose();
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ButtonDialogsSetup_Click(object sender, EventArgs e)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Invalid comparison between Unknown and I4
		try
		{
			StiComponentsToolboxForm stiComponentsToolboxForm = new StiComponentsToolboxForm(StiToolboxMode.Dialogs);
			try
			{
				Designer.AddOwnedForm((Form)(object)stiComponentsToolboxForm);
				if ((int)((Form)stiComponentsToolboxForm).ShowDialog() == 1)
				{
					Designer.InvokeBuidingToolbox();
					RibbonControl ribbonControl = Designer.RibbonControl;
					if (ribbonControl != null)
					{
						((Control)ribbonControl).Refresh();
					}
					((Control)Designer).Refresh();
				}
			}
			finally
			{
				((IDisposable)stiComponentsToolboxForm)?.Dispose();
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ButtonReportsSetup_Click(object sender, EventArgs e)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Invalid comparison between Unknown and I4
		try
		{
			StiComponentsToolboxForm stiComponentsToolboxForm = new StiComponentsToolboxForm(StiToolboxMode.Reports);
			try
			{
				Designer.AddOwnedForm((Form)(object)stiComponentsToolboxForm);
				if ((int)((Form)stiComponentsToolboxForm).ShowDialog() == 1)
				{
					Designer.InvokeBuidingToolbox();
					RibbonControl ribbonControl = Designer.RibbonControl;
					if (ribbonControl != null)
					{
						((Control)ribbonControl).Refresh();
					}
					((Control)Designer).Refresh();
				}
			}
			finally
			{
				((IDisposable)stiComponentsToolboxForm)?.Dispose();
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ButtonBarCode_Click(object sender, EventArgs e)
	{
		try
		{
			ResetTools();
			ButtonItem buttonItem = (ButtonItem)sender;
			buttonItem.Checked = true;
			StiBarCode stiBarCode = new StiBarCode();
			stiBarCode.BarCodeType = ((StiBarCodeTypeService)buttonItem.Tag).CreateNew();
			Designer.CreateTool.ComponentForCreating = stiBarCode;
			Designer.CurrentTool = Designer.CreateTool;
			Designer.CreateTool.Action = StiCreateAction.CreateComponent;
			buttonItem.Checked = true;
			Designer.CreateTool.InvokeActivated(buttonItem, e);
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ButtonChartSeries_Click(object sender, EventArgs e)
	{
		try
		{
			ResetTools();
			ButtonItem buttonItem = (ButtonItem)sender;
			buttonItem.Checked = true;
			StiChart stiChart = new StiChart();
			StiSeries stiSeries = (StiSeries)StiActivator.CreateObject((Type)buttonItem.Tag);
			if (StiOptions.Designer.Editors.ChartEditor.SimpleEditorByDefault)
			{
				StiChartDesigner.PredefinedSeriesType = StiChartDesigner.ConvertSeriesToChartSeriesType(stiSeries);
			}
			else
			{
				stiChart.Series.Add(stiSeries);
				stiSeries.Core.ApplyStyle(stiChart.Style, stiChart.Style.Core.GetColorBySeries(stiSeries, stiSeries.Core?.SeriesColors));
			}
			Designer.CreateTool.ComponentForCreating = stiChart;
			Designer.CurrentTool = Designer.CreateTool;
			Designer.CreateTool.Action = StiCreateAction.CreateComponent;
			buttonItem.Checked = true;
			Designer.CreateTool.InvokeActivated(buttonItem, e);
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ButtonGaugeSeries_Click(object sender, EventArgs e)
	{
		try
		{
			ResetTools();
			ButtonItem buttonItem = (ButtonItem)sender;
			buttonItem.Checked = true;
			StiGauge stiGauge = new StiGauge();
			if (buttonItem.Tag is StiGaugeType)
			{
				stiGauge.Type = (StiGaugeType)buttonItem.Tag;
				StiGaugeV2InitHelper.Init(stiGauge, (StiGaugeType)buttonItem.Tag, addRange: true, skipText: false);
			}
			else if ((StiGaugeElemenType)buttonItem.Tag == StiGaugeElemenType.LinearElement)
			{
				StiGaugeHelper.DefaultLinearGauge(stiGauge, Designer.Report);
			}
			else
			{
				StiGaugeHelper.DefaultRadialGauge(stiGauge, Designer.Report);
			}
			Designer.CreateTool.ComponentForCreating = stiGauge;
			Designer.CurrentTool = Designer.CreateTool;
			Designer.CreateTool.Action = StiCreateAction.CreateComponent;
			buttonItem.Checked = true;
			Designer.CreateTool.InvokeActivated(buttonItem, e);
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}
}
