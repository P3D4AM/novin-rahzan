using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
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

public class StiToolboxToolbarService : StiToolbarService
{
	private string stepScaleName = StiScale.StepName;

	private ButtonItem buttonSetup;

	private StiPopup popupShape;

	private Rectangle dragBoxFromMouseDown;

	public override string ServiceName => StiLocalization.Get("Toolbox", "title");

	public override int Position => 10;

	public override int DefaultDockLine => 1;

	public override bool DefaultVisible => true;

	public override DockStyle DefaultDock => (DockStyle)3;

	private int ShapePopupCloseTime { get; set; }

	private bool IsForm
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

	private bool IsScreen
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

	private void Designer_RefreshDesigner(object sender, EventArgs e)
	{
		if (base.ToolBar != null)
		{
			((Control)base.ToolBar).Enabled = Report != null;
		}
	}

	private void Designer_ResetingToolbox(object sender, EventArgs e)
	{
		ResetTools();
	}

	private void ToolBar_StartDragDrop(object sender, EventArgs e)
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
			StiDesignerDragDrop.StartDragDrop(Designer, stiComponent, (Control)(object)base.ToolBar);
		}
		else
		{
			StiDesignerDragDrop.StartDragDrop(Designer, (Control)(object)base.ToolBar);
		}
		Designer.SelectTool.IsDragDropFromToolBox = false;
	}

	private void CategoryButton_Click(object sender, EventArgs e)
	{
		Designer.CreateTool.ComponentForCreating = null;
	}

	private void ButtonShapeCategory_Click(object sender, EventArgs e)
	{
		Designer.CreateTool.ComponentForCreating = null;
		int num = Environment.TickCount - ShapePopupCloseTime;
		ButtonItem buttonItem = sender as ButtonItem;
		if (num > 500 || ShapePopupCloseTime == 0)
		{
			((ToolStripDropDown)popupShape).Show(new Point(buttonItem.ScreenRectangle.Right, buttonItem.ScreenRectangle.Y), (ToolStripDropDownDirection)5);
		}
		else
		{
			buttonItem.Checked = false;
		}
	}

	private void ButtonBarCode_Click(object sender, MouseEventArgs e)
	{
		try
		{
			SetDragBox(e.Location);
			ResetTools();
			ButtonItem buttonItem = (ButtonItem)sender;
			buttonItem.Checked = true;
			StiBarCode stiBarCode = new StiBarCode();
			stiBarCode.BarCodeType = ((StiBarCodeTypeService)buttonItem.Tag).CreateNew();
			Designer.CreateTool.ComponentForCreating = stiBarCode;
			Designer.CurrentTool = Designer.CreateTool;
			Designer.CreateTool.Action = StiCreateAction.CreateComponent;
			buttonItem.Checked = true;
			Designer.CreateTool.InvokeActivated(buttonItem, (EventArgs)(object)e);
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ButtonChartSeries_Click(object sender, MouseEventArgs e)
	{
		try
		{
			SetDragBox(e.Location);
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
			Designer.CreateTool.InvokeActivated(buttonItem, (EventArgs)(object)e);
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ButtonGaugeSeries_Click(object sender, MouseEventArgs e)
	{
		try
		{
			SetDragBox(e.Location);
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
			Designer.CreateTool.InvokeActivated(buttonItem, (EventArgs)(object)e);
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void MapSeries_Click(object sender, MouseEventArgs e)
	{
		try
		{
			SetDragBox(e.Location);
			ResetTools();
			ButtonItem buttonItem = (ButtonItem)sender;
			buttonItem.Checked = true;
			StiMap componentForCreating = new StiMap
			{
				MapIdent = (string)buttonItem.Tag
			};
			Designer.CreateTool.ComponentForCreating = componentForCreating;
			Designer.CurrentTool = Designer.CreateTool;
			Designer.CreateTool.Action = StiCreateAction.CreateComponent;
			buttonItem.Checked = true;
			Designer.CreateTool.InvokeActivated(buttonItem, (EventArgs)(object)e);
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void ButtonComponent_Click(object sender, EventArgs e)
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
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Invalid comparison between Unknown and I4
		bool flag = dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y);
		if ((int)e.Button == 1048576)
		{
			if (!flag)
			{
				return;
			}
			try
			{
				IStiShapeElement stiShapeElement = ((Control)popupShape).Tag as IStiShapeElement;
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
			base.ToolBar.InvokeStartDragDrop(sender);
			Designer.CreateTool.ComponentForCreating = null;
		}
		else
		{
			dragBoxFromMouseDown = Rectangle.Empty;
		}
	}

	private void ShapeElementToolbox_Click(object sender, MouseEventArgs e)
	{
		try
		{
			SetDragBox(e.Location);
			ResetTools();
			IStiShapeElement stiShapeElement = ((Control)popupShape).Tag as IStiShapeElement;
			object obj = null;
			if (sender is StiToolboxButton stiToolboxButton)
			{
				obj = ((Control)stiToolboxButton).Tag;
			}
			if (!(obj is StiAction))
			{
				StiShape stiShape = obj as StiShape;
				stiShapeElement.ShapeType = stiShape.ShapeType;
				Designer.CreateTool.ComponentForCreating = stiShapeElement as StiComponent;
				Designer.CurrentTool = Designer.CreateTool;
				Designer.CreateTool.Action = StiCreateAction.CreateComponent;
				Designer.CreateTool.InvokeActivated(null, (EventArgs)(object)e);
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

	private void ButtonDashboardSetup_Click(object sender, EventArgs e)
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

	private void ButtonScreenSetup_Click(object sender, EventArgs e)
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

	private void ButtonSetup_Click(object sender, EventArgs e)
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

	private void Designer_BuildingToolbox(object s, EventArgs e)
	{
		if (base.ToolBar != null && Designer != null)
		{
			bool @bool = StiSettings.GetBool("CloudDesigner", "ShowStandardToolbox", DefaultVisible);
			base.ToolBar.Visible = @bool;
			if (@bool)
			{
				BuildingToolbox();
				base.ToolBar.RecalcLayout();
			}
		}
	}

	private void MapCategoryButton_Click(object sender, EventArgs e)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Invalid comparison between Unknown and I4
		StiMapCategoriesForm stiMapCategoriesForm = new StiMapCategoriesForm(Report);
		try
		{
			Designer.AddOwnedForm((Form)(object)stiMapCategoriesForm);
			if ((int)((Form)stiMapCategoriesForm).ShowDialog() == 1 && !string.IsNullOrEmpty(stiMapCategoriesForm.MapID))
			{
				try
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
			((IDisposable)stiMapCategoriesForm)?.Dispose();
		}
	}

	private void Button_MouseMove(object sender, MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)e.Button == 1048576)
		{
			base.ToolBar.InvokeStartDragDrop(sender);
		}
	}

	private void ButtonCategory_MouseMove(object sender, MouseEventArgs e)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Invalid comparison between Unknown and I4
		bool flag = dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y);
		if ((int)e.Button == 1048576)
		{
			if (flag)
			{
				base.ToolBar.InvokeStartDragDrop(sender);
			}
		}
		else
		{
			dragBoxFromMouseDown = Rectangle.Empty;
		}
	}

	private void SetDragBox(Point point)
	{
		Point location = new Point(point.X - SystemInformation.DragSize.Width / 2, point.Y - SystemInformation.DragSize.Height / 2);
		dragBoxFromMouseDown = new Rectangle(location, SystemInformation.DragSize);
	}

	public override void Localize()
	{
		base.Localize();
		BuildingToolbox();
	}

	public override void Init(StiDesignerControl designer)
	{
		base.Init(designer);
		CreateToolbar();
		base.ToolBar.SuspendLayout();
		((Control)base.ToolBar).Text = StiLocalization.Get("Toolbox", "title");
		((Control)base.ToolBar).AllowDrop = false;
		if (designer.IsRibbonGuiMode)
		{
			base.ToolBar.GrabHandleStyle = eGrabHandleStyle.None;
			base.ToolBar.RoundCorners = false;
		}
		BuildingToolbox();
		Designer.ResetingToolbox += Designer_ResetingToolbox;
		Designer.BuidingToolbox += Designer_BuildingToolbox;
		Designer.RefreshDesigner += Designer_RefreshDesigner;
		AddToolbarToDockSite();
		base.ToolBar.ResumeLayout();
	}

	public override void Dispose()
	{
		base.Dispose();
		buttonSetup = null;
	}

	public static StiToolboxToolbarService GetService()
	{
		return GetService(null);
	}

	public static StiToolboxToolbarService GetService(StiDesignerControl designer)
	{
		if (designer == null)
		{
			return StiDesignerOptions.Toolbars.FirstOrDefault((StiToolbarService c) => c.GetType() == typeof(StiToolboxToolbarService)) as StiToolboxToolbarService;
		}
		return designer.Services.GetServices(typeof(StiToolbarService)).OfType<StiToolboxToolbarService>().FirstOrDefault();
	}

	public void ResetTools()
	{
		if (popupShape != null)
		{
			((Control)popupShape).Hide();
		}
		StiServiceContainer services = Designer.Services.GetServices(typeof(StiToolService));
		foreach (StiToolService item in services)
		{
			item.Reset();
		}
		foreach (object item2 in base.ToolBar.Items)
		{
			UnCheckedButtons(item2);
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
	}

	private void BuildingComponentsToolbox()
	{
		//IL_05db: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e5: Expected O, but got Unknown
		//IL_0723: Unknown result type (might be due to invalid IL or missing references)
		//IL_072d: Expected O, but got Unknown
		if (base.ToolBar == null)
		{
			return;
		}
		base.ToolBar.Items.Clear();
		List<StiComponent> list = new List<StiComponent>();
		Graphics globalMeasureGraphics = StiReport.GlobalMeasureGraphics;
		if (Designer != null)
		{
			StiServiceContainer services = Designer.Services.GetServices(typeof(StiToolService));
			foreach (StiToolService item in services)
			{
				item.Designer = Designer;
				if (item is StiSelectToolService)
				{
					Designer.SelectTool = item as StiSelectToolService;
				}
				else if (item is StiCreateToolService)
				{
					Designer.CreateTool = item as StiCreateToolService;
				}
				else if (item is StiStyleToolService)
				{
					Designer.StyleTool = item as StiStyleToolService;
				}
			}
			if (Designer.SelectTool == null)
			{
				string message = string.Format(StiLocalization.Get("Errors", "ServiceNotFound"), "StiSelectToolService");
				StiLogService.Write(GetType(), message);
				throw new Exception(message);
			}
			if (Designer.CreateTool == null)
			{
				string message2 = string.Format(StiLocalization.Get("Errors", "ServiceNotFound"), "StiCreateToolService");
				StiLogService.Write(GetType(), message2);
				throw new Exception(message2);
			}
			StiSettings.Load();
			Hashtable hashtable = new Hashtable();
			IEnumerable<StiComponent> mainList = StiOptions.Services.Components.Where((StiComponent s) => s.ServiceEnabled && !(s is IStiReportControl) && !(s is IStiElement) && !(s is IStiComponentUI));
			mainList = StiDesignerSpecificationHelper.CheckShowComponents(mainList);
			foreach (StiComponent item2 in mainList)
			{
				if (item2 is StiVerticalLinePrimitive || item2 is StiHorizontalLinePrimitive || item2 is StiRoundedRectanglePrimitive || item2 is StiRectanglePrimitive || (item2 is StiWinControl && !StiOptions.Designer.AllowUseWinControl) || item2 is StiPage)
				{
					continue;
				}
				Type type = item2.GetType();
				StiToolboxAttribute[] array = (StiToolboxAttribute[])type.GetCustomAttributes(typeof(StiToolboxAttribute), inherit: true);
				if (array.Length == 0 || !array[0].Enable || (type.GetCustomAttributes(typeof(StiEngineAttribute), inherit: true) is StiEngineAttribute[] array2 && array2.Length != 0 && array2[0].Version != 0 && (Report == null || array2[0].Version != Report.EngineVersion)) || (item2.GetType() == typeof(StiShape) && !StiToolboxSetup.Reports.ShowShapesCategory) || (item2.GetType() == typeof(StiChart) && !StiToolboxSetup.Reports.ShowChartCategory) || (item2.GetType() == typeof(StiGauge) && !StiToolboxSetup.Reports.ShowGaugeCategory) || (item2.GetType() == typeof(StiMap) && !StiToolboxSetup.Reports.ShowMapCategory) || (item2.GetType() == typeof(StiBarCode) && !StiToolboxSetup.Reports.ShowBarCodesCategory))
				{
					continue;
				}
				string localizedCategory = item2.LocalizedCategory;
				ButtonItem button = hashtable[localizedCategory] as ButtonItem;
				if (button == null)
				{
					Image val = null;
					string text = "Stimulsoft.Report.Design";
					string text2 = text + ".Resources.Office2013.ToolBox";
					bool flag = false;
					if (localizedCategory == StiLocalization.Get("Report", "Components"))
					{
						flag = !StiToolboxSetup.Reports.ShowComponentsCategory;
						val = (Image)(object)StiImageUtils.GetImage(text, text2 + ".catComponents" + stepScaleName + ".png");
					}
					else if (localizedCategory == StiLocalization.Get("Report", "Bands"))
					{
						flag = !StiToolboxSetup.Reports.ShowBandsCategory;
						val = (Image)(object)StiImageUtils.GetImage(text, text2 + ".catBands" + stepScaleName + ".png");
					}
					else if (localizedCategory == StiLocalization.Get("Report", "CrossBands"))
					{
						flag = !StiToolboxSetup.Reports.ShowCrossBandsCategory;
						val = (Image)(object)StiImageUtils.GetImage(text, text2 + ".catCrossBands" + stepScaleName + ".png");
					}
					else if (localizedCategory == StiLocalization.Get("Report", "Shapes"))
					{
						val = (Image)(object)StiImageUtils.GetImage(text, text2 + ".StiShape" + stepScaleName + ".png");
					}
					else if (localizedCategory == StiLocalization.Get("Components", "StiChart"))
					{
						val = (Image)(object)StiImageUtils.GetImage(text, text2 + ".StiChart" + stepScaleName + ".png");
					}
					else if (localizedCategory == StiLocalization.Get("Components", "StiGauge"))
					{
						val = (Image)(object)StiImageUtils.GetImage(text, text2 + ".StiGauge" + stepScaleName + ".png");
					}
					else if (localizedCategory == StiLocalization.Get("Components", "StiMap"))
					{
						val = (Image)(object)StiImageUtils.GetImage(text, text2 + ".StiMap" + stepScaleName + ".png");
					}
					else if (localizedCategory == StiLocalization.Get("Components", "StiBarCode"))
					{
						val = (Image)(object)StiImageUtils.GetImage(text, text2 + ".StiBarCode" + stepScaleName + ".png");
					}
					else
					{
						StiServiceCategoryBitmapAttribute[] array3 = type.GetCustomAttributes(typeof(StiServiceCategoryBitmapAttribute), inherit: true) as StiServiceCategoryBitmapAttribute[];
						val = (Image)(object)array3[0].GetImage();
					}
					if (!flag)
					{
						Add(ref button, val, localizedCategory, null);
						button.MouseDown += new MouseEventHandler(CategoryButton_Click);
						button.AutoExpandOnClick = true;
						if (Designer.IsRibbonGuiMode)
						{
							Designer.SetToolTipOnlyHeaderEx(button, globalMeasureGraphics, localizedCategory, "user-manual/reports_designer_toolbox.htm");
						}
						hashtable[localizedCategory] = button;
					}
				}
				ButtonItem buttonItem = (ButtonItem)hashtable[localizedCategory];
				if (buttonItem != null)
				{
					if (item2 is StiChart)
					{
						FillChartItems(buttonItem);
					}
					else if (item2 is StiMap)
					{
						button.Tag = item2;
						button.Click += MapCategoryButton_Click;
					}
					else if (item2 is StiGauge)
					{
						FillGaugeItems(button);
					}
					else if (item2 is StiShape)
					{
						button.ShowArrow = true;
						CreateShapeCategory(button);
					}
					else if (item2 is StiBarCode)
					{
						FillBarCodeItems(buttonItem);
					}
					else if (!(item2 is StiPrimitive))
					{
						ButtonItem buttonItem2 = new ButtonItem();
						buttonItem2.Image = Designer.GetImageType(type, typeof(StiComponent));
						buttonItem2.Text = item2.ServiceName;
						buttonItem2.Tag = item2;
						ButtonItem buttonItem3 = buttonItem2;
						buttonItem3.Click += ButtonComponent_Click;
						buttonItem3.MouseMove += new MouseEventHandler(Button_MouseMove);
						Designer.SetComponentToolTip(buttonItem3, item2);
						buttonItem.SubItems.Add(buttonItem3);
					}
				}
				if (StiToolboxSetup.Reports.IsComponentVisible(item2))
				{
					list.Add(item2);
				}
			}
			AddToolButtonSeparator();
			foreach (StiComponent item3 in list)
			{
				if (!(item3 is StiChart) && !(item3 is StiMap) && !(item3 is StiGauge) && !(item3 is StiShape) && !(item3 is StiVerticalLinePrimitive) && !(item3 is StiHorizontalLinePrimitive) && !(item3 is StiRoundedRectanglePrimitive) && !(item3 is StiRectanglePrimitive))
				{
					ButtonItem button2 = null;
					Add(ref button2, null, item3.ServiceName, ButtonComponent_Click);
					button2.Tag = item3;
					button2.Image = Designer.GetImageType(item3.GetType(), typeof(StiComponent));
					button2.Text = item3.ServiceName;
					Designer.SetComponentToolTip(button2, item3);
					button2.AutoCheckOnClick = true;
				}
			}
			Designer.SelectTool.Activate();
		}
		AddToolButtonSeparator();
		string text3 = "Resources.Office2013.ToolBox.ToolBox" + stepScaleName + ".png";
		string text4 = StiLocalization.Get("FormDesigner", "SetupToolbox");
		Add(ref buttonSetup, (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design." + text3), text4, ButtonSetup_Click);
		if (Designer.IsRibbonGuiMode)
		{
			Designer.SetToolTipOnlyHeaderEx(buttonSetup, globalMeasureGraphics, text4, "user-manual/reports_designer_toolbox_shortcut_panel.htm");
		}
		if (!base.ToolBar.HotDragDrop)
		{
			base.ToolBar.HotDragDrop = true;
			base.ToolBar.StartDragDrop += ToolBar_StartDragDrop;
		}
		CreateToolButtons();
		base.ToolBar.RecalcLayout();
	}

	private void BuildingDialogsToolbox()
	{
		if (base.ToolBar == null)
		{
			return;
		}
		base.ToolBar.Items.Clear();
		List<StiComponent> list = new List<StiComponent>();
		if (Designer != null)
		{
			StiSettings.Load();
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
			foreach (StiComponent item2 in list)
			{
				ButtonItem button = null;
				Add(ref button, null, item2.ServiceName, ButtonComponent_Click);
				button.Tag = item2;
				button.Image = Designer.GetImageType(item2.GetType(), typeof(StiComponent));
				button.Text = item2.ServiceName;
				Designer.SetComponentToolTip(button, item2);
				button.Click += ButtonComponent_Click;
				button.AutoCheckOnClick = true;
			}
			Designer.SelectTool.Activate();
		}
		AddToolButtonSeparator();
		string text = "Resources.Office2013.ToolBox.ToolBox" + stepScaleName + ".png";
		string text2 = StiLocalization.Get("FormDesigner", "SetupToolbox");
		Add(ref buttonSetup, (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design." + text), text2, ButtonDialogsSetup_Click);
		if (Designer.IsRibbonGuiMode)
		{
			Graphics globalMeasureGraphics = StiReport.GlobalMeasureGraphics;
			Designer.SetToolTipOnlyHeaderEx(buttonSetup, globalMeasureGraphics, text2, "user-manual/reports_designer_toolbox_shortcut_panel.htm");
		}
		if (!base.ToolBar.HotDragDrop)
		{
			base.ToolBar.HotDragDrop = true;
			base.ToolBar.StartDragDrop += ToolBar_StartDragDrop;
		}
		CreateToolButtons();
		base.ToolBar.RecalcLayout();
	}

	private void BuildingDashboardsToolbox()
	{
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Expected O, but got Unknown
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Expected O, but got Unknown
		//IL_01ee: Expected O, but got Unknown
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Expected O, but got Unknown
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Expected O, but got Unknown
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Expected O, but got Unknown
		if (base.ToolBar == null)
		{
			return;
		}
		base.ToolBar.Items.Clear();
		List<StiComponent> list = new List<StiComponent>();
		if (Designer != null)
		{
			StiSettings.Load();
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
			foreach (StiComponent item2 in list)
			{
				ButtonItem button = null;
				if (item2 is IStiShapeElement)
				{
					Add(ref button, null, item2.ServiceName, null);
					button.ShowArrow = true;
					button.AutoExpandOnClick = true;
					button.AutoCheckOnClick = false;
					button.MouseDown += new MouseEventHandler(CategoryButton_Click);
					button.Click += ButtonShapeCategory_Click;
					popupShape = new StiPopup((Control)(object)new StiShapeToolboxControl(Designer, new MouseEventHandler(ShapeElementToolbox_Click), new MouseEventHandler(ShapeElementToolbox_MouseMove), showBaseShapes: false, showBorder: true));
					((Control)popupShape).Tag = item2;
					((ToolStripDropDown)popupShape).Closed += (ToolStripDropDownClosedEventHandler)delegate
					{
						button.Checked = false;
						ShapePopupCloseTime = Environment.TickCount;
					};
					button.AutoCheckOnClick = true;
					button.Image = Designer.GetImageType("StiShapeElement", typeof(StiComponent));
					Designer.SetComponentToolTip(button, item2);
					continue;
				}
				if (item2 is IStiRegionMapElement || item2 is IStiOnlineMapElement)
				{
					if (flag)
					{
						flag = false;
						Image imageType = Designer.GetImageType(typeof(StiMap), typeof(StiComponent));
						Add(ref button, imageType, "StiMap", null);
						FillMapElementItems(button);
						button.MouseDown += new MouseEventHandler(CategoryButton_Click);
						button.ShowArrow = true;
						button.Text = item2.ServiceName;
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
						Image imageType2 = Designer.GetImageType("StiFilterElement", typeof(StiComponent));
						Add(ref button, imageType2, "StiFilter", null);
						FillFilterElementItems(button);
						button.MouseDown += new MouseEventHandler(CategoryButton_Click);
						button.ShowArrow = true;
						button.Text = item2.ServiceName;
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
					Add(ref button, null, item2.ServiceName, ButtonComponent_Click);
					button.Click += ButtonComponent_Click;
				}
				if (button != null)
				{
					button.Tag = item2;
					button.Image = Designer.GetImageType(item2.GetType(), typeof(StiComponent));
					button.Text = item2.ServiceName;
					Designer.SetComponentToolTip(button, item2);
					button.AutoCheckOnClick = true;
				}
			}
			Designer.SelectTool.Activate();
		}
		AddToolButtonSeparator();
		string text = "Resources.Office2013.ToolBox.ToolBox" + stepScaleName + ".png";
		string text2 = StiLocalization.Get("FormDesigner", "SetupToolbox");
		Add(ref buttonSetup, (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design." + text), text2, ButtonDashboardSetup_Click);
		if (Designer.IsRibbonGuiMode)
		{
			Graphics globalMeasureGraphics = StiReport.GlobalMeasureGraphics;
			Designer.SetToolTipOnlyHeaderEx(buttonSetup, globalMeasureGraphics, text2, "user-manual/reports_designer_toolbox_shortcut_panel.htm");
		}
		if (!base.ToolBar.HotDragDrop)
		{
			base.ToolBar.HotDragDrop = true;
			base.ToolBar.StartDragDrop += ToolBar_StartDragDrop;
		}
		CreateToolButtons();
		base.ToolBar.RecalcLayout();
	}

	private void BuildingComponentUIsToolbox()
	{
		if (base.ToolBar == null)
		{
			return;
		}
		base.ToolBar.Items.Clear();
		List<StiComponent> list = new List<StiComponent>();
		if (Designer != null)
		{
			StiSettings.Load();
			IEnumerable<StiComponent> enumerable = StiOptions.Services.Components.Where((StiComponent s) => s.ServiceEnabled && s is IStiComponentUI);
			foreach (StiComponent item in enumerable)
			{
				Type type = item.GetType();
				StiToolboxAttribute[] array = (StiToolboxAttribute[])type.GetCustomAttributes(typeof(StiToolboxAttribute), inherit: true);
				if (array.Length != 0 && array[0].Enable && (!(type.GetCustomAttributes(typeof(StiEngineAttribute), inherit: true) is StiEngineAttribute[] array2) || array2.Length == 0 || array2[0].Version == StiEngineVersion.All || (Report != null && array2[0].Version == Report.EngineVersion)) && StiToolboxSetup.ComponentUIs.IsComponentVisible(item))
				{
					list.Add(item);
				}
			}
			foreach (StiComponent item2 in list)
			{
				ButtonItem button = null;
				Add(ref button, null, item2.ServiceName, ButtonComponent_Click);
				button.Click += ButtonComponent_Click;
				if (button != null)
				{
					button.Tag = item2;
					button.Image = Designer.GetImageType(item2.GetType(), typeof(StiComponent));
					button.Text = item2.ServiceName;
					Designer.SetComponentToolTip(button, item2);
					button.AutoCheckOnClick = true;
				}
			}
			Designer.SelectTool.Activate();
		}
		AddToolButtonSeparator();
		string text = "Resources.Office2013.ToolBox.ToolBox" + stepScaleName + ".png";
		string text2 = StiLocalization.Get("FormDesigner", "SetupToolbox");
		Add(ref buttonSetup, (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design." + text), text2, ButtonScreenSetup_Click);
		if (Designer.IsRibbonGuiMode)
		{
			Graphics globalMeasureGraphics = StiReport.GlobalMeasureGraphics;
			Designer.SetToolTipOnlyHeaderEx(buttonSetup, globalMeasureGraphics, text2, "user-manual/reports_designer_toolbox_shortcut_panel.htm");
		}
		if (!base.ToolBar.HotDragDrop)
		{
			base.ToolBar.HotDragDrop = true;
			base.ToolBar.StartDragDrop += ToolBar_StartDragDrop;
		}
		CreateToolButtons();
		base.ToolBar.RecalcLayout();
	}

	private void FillBarCodeItems(ButtonItem categoryButton)
	{
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Expected O, but got Unknown
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Expected O, but got Unknown
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
				Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.Barcode." + stiInfographicsToolboxInfo.Icon + stepScaleName + ".png"),
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
					image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", $"Stimulsoft.Report.Design.Resources.Office2013.Barcode.{item2.ComponentId}{stepScaleName}.png");
				}
				catch
				{
					image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.Barcode.StiPharmacodeBarCodeType" + stepScaleName + ".png");
				}
				ButtonItem buttonItem2 = new ButtonItem
				{
					Image = image,
					Text = item2.ServiceName,
					Tag = item2,
					BeginGroup = stiInfographicsToolboxInfo.BeginGroup
				};
				buttonItem2.MouseDown += new MouseEventHandler(ButtonBarCode_Click);
				buttonItem2.MouseMove += new MouseEventHandler(ButtonCategory_MouseMove);
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
				buttonItem2.MouseMove += new MouseEventHandler(ButtonCategory_MouseMove);
				buttonItem.SubItems.Add(buttonItem2);
			}
		}
	}

	private void FillMapElementItems(ButtonItem categoryButton)
	{
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
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
				AddSub(ref button, categoryButton, null, item2.ServiceName, ButtonComponent_Click, new MouseEventHandler(Button_MouseMove));
				button.Tag = item2;
				button.Image = Designer.GetImageType(item2.GetType(), typeof(StiComponent));
				button.Text = item2.ServiceName;
				button.AutoCheckOnClick = true;
				Designer.SetComponentToolTip(button, item2);
			}
		}
	}

	private void FillFilterElementItems(ButtonItem categoryButton)
	{
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Expected O, but got Unknown
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
				AddSub(ref button, categoryButton, null, item2.ServiceName, ButtonComponent_Click, new MouseEventHandler(Button_MouseMove));
				button.Tag = item2;
				button.Image = Designer.GetImageType(item2.GetType(), typeof(StiComponent));
				button.Text = item2.ServiceName;
				button.AutoCheckOnClick = true;
				Designer.SetComponentToolTip(button, item2);
			}
		}
	}

	private void CreateShapeCategory(ButtonItem buttonCategory)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_0051: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		buttonCategory.Click += ButtonShapeCategory_Click;
		popupShape = new StiPopup((Control)(object)new StiShapeToolboxControl(Designer, new MouseEventHandler(ButtonComponent_Click), new MouseEventHandler(Button_MouseMove), showBaseShapes: true, showBorder: true));
		((ToolStripDropDown)popupShape).Closed += (ToolStripDropDownClosedEventHandler)delegate
		{
			buttonCategory.Checked = false;
			ShapePopupCloseTime = Environment.TickCount;
		};
	}

	private void FillGaugeItems(ButtonItem categoryButton)
	{
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
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
				Image = (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Office2013.Gauge." + stiInfographicsToolboxInfo.Icon + ".png", makeTransparent: false),
				Text = stiInfographicsToolboxInfo.LocName
			};
			buttonItem.MouseDown += new MouseEventHandler(ButtonGaugeSeries_Click);
			buttonItem.MouseMove += new MouseEventHandler(ButtonCategory_MouseMove);
			categoryButton.SubItems.Add(buttonItem);
		}
	}
}
