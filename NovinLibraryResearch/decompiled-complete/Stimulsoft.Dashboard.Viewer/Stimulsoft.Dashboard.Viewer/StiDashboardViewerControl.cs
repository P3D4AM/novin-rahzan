using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Gis;
using Stimulsoft.Base.Licenses;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Viewer;
using Stimulsoft.Controls.Menu;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Dashboard.Components.Panel;
using Stimulsoft.Dashboard.Drawing.Helpers;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Dashboard.Viewer.Controls;
using Stimulsoft.Dashboard.Viewer.Controls.Helpers;
using Stimulsoft.Dashboard.Viewer.Elements;
using Stimulsoft.Dashboard.Viewer.Elements.Chart;
using Stimulsoft.Dashboard.Viewer.Elements.RegionMap;
using Stimulsoft.Dashboard.Viewer.Extensions;
using Stimulsoft.Dashboard.Viewer.FullScreen;
using Stimulsoft.Dashboard.Viewer.Trial;
using Stimulsoft.Dashboard.Visualizers;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Surface;
using Stimulsoft.Report.Viewer;

namespace Stimulsoft.Dashboard.Viewer;

[ToolboxBitmap(typeof(StiDashboardViewerControl), "Toolbox.StiViewerControl.bmp")]
[Guid("A50AE83B-53D2-4B7B-A93C-CF41DA7C5D6A")]
public class StiDashboardViewerControl : UserControl, IStiDashboardViewer, IStiViewerAccessor
{
	private int dashboardHorizontallyMargin = StiScale.I5;

	private int dashboardVerticallyMargin = StiScale.I5;

	private StiViewerMenuActionButton buttonViewerMenu;

	private StiEditDashboardActionButton buttonEditDashboard;

	private StiRefreshDashboardActionButton buttonRefreshDashboard;

	private StiOpenDashboardActionButton buttonOpenDashboard;

	private StiOpenViewerFullScreenActionButton buttonOpenViewerFullScreen;

	private StiCloseViewerFullScreenActionButton buttonCloseViewerFullScreen;

	private bool blockPaintingWatermark = true;

	private Form ownerForm;

	private Timer timerRefreshReport;

	private Timer timerRenderReport;

	private Timer timerDelayedWatermark;

	private Dictionary<string, bool> isFirstRenderingDictionary = new Dictionary<string, bool>();

	private UserControl reportViewer;

	private StiPage selectedPage;

	private bool lockCreateTabButtons;

	private StiContextMenu menuActions;

	private List<StiPage> nestedPages = new List<StiPage>();

	private StiDashboard dashboard;

	private List<StiDashboard> dashboards;

	private StiReport report;

	private IContainer components;

	private Timer timerRefreshScrollView;

	private StiElementPanel panelElements;

	private Panel panelToolBar;

	private ToolTip toolTipActionButtons;

	private Panel panelDialogs;

	private Panel panelHolder;

	private VScrollBar VScrollBar;

	private Panel panelHScrollBar;

	private HScrollBar HScrollBar;

	private Panel panelCorner;

	bool IStiViewerAccessor.BlockFormClose { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public StiDashboard Dashboard
	{
		get
		{
			return dashboard;
		}
		set
		{
			if (selectedPage == value)
			{
				return;
			}
			dashboard = value;
			StopRefreshReportTimer();
			StopDelayedWatermarkTimer();
			if (dashboard != null)
			{
				RenderView();
				RunRenderData();
				Form obj = ((Control)this).FindForm();
				if (obj != null)
				{
					((Control)obj).Invalidate();
				}
				((Control)this).Invalidate();
				if (dashboard?.Report != null && dashboard.Report.RefreshTime > 0)
				{
					StartRefreshReportTimer(dashboard.Report.RefreshTime);
				}
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal Hashtable ReportsCache { get; set; } = new Hashtable();


	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	private StiDashboardContentAlignment Alignment => Dashboard.ContentAlignment;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal IStiParametersAccessor Parameters { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal IStiProgressInformation Progress { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal List<StiDashboard> Dashboards
	{
		get
		{
			return dashboards;
		}
		set
		{
			dashboards = value;
			Dashboard = dashboards?.FirstOrDefault();
			isFirstRenderingDictionary.Clear();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiReport Report
	{
		get
		{
			return report;
		}
		set
		{
			if (report == value)
			{
				return;
			}
			if (report != null)
			{
				report.RefreshViewer -= Viewer_RefreshViewer;
				report.RefreshControl -= Viewer_RefreshControl;
				if (report.CompiledReport != null)
				{
					report.CompiledReport.RefreshViewer -= Viewer_RefreshViewer;
				}
				if (report.CompiledReport != null)
				{
					report.CompiledReport.RefreshControl -= Viewer_RefreshControl;
				}
			}
			if (value != null)
			{
				value.RefreshViewer += Viewer_RefreshViewer;
				value.RefreshControl += Viewer_RefreshControl;
				if (value.CompiledReport != null)
				{
					value.CompiledReport.RefreshViewer += Viewer_RefreshViewer;
				}
				if (value.CompiledReport != null)
				{
					value.CompiledReport.RefreshControl += Viewer_RefreshControl;
				}
			}
			report = value;
			if (value != null && value.NeedsCompiling && !value.IsCompiled && value.CalculationMode == StiCalculationMode.Compilation)
			{
				value.Compile();
			}
			CleanUp(detachEvents: false);
			Parameters.CleanAllVariableStates();
			if (value != null)
			{
				StiCacheCleaner.Clean(value);
			}
			StopRefreshReportTimer();
			StopDelayedWatermarkTimer();
			Dashboards = StiDashboardViewerHelper.GetDashboardList(value);
			if (value != null)
			{
				if ((value.CalculationMode == StiCalculationMode.Interpretation || StiOptions.Engine.ForceInterpretationMode) && value.Variables == null)
				{
					StiParser.PrepareReportVariables(value);
				}
				StiDashboardVariables.FillParameters(value);
				StiVariableHelper.SetDefaultValueForRequestFromUserVariables(value.CompiledReport ?? value, haveVars: true, allowParseQuery: true);
			}
			Parameters.RefreshDialogs();
			ApplyStyleColorsToDialogs();
			StiReport reportForCheck = value?.CompiledReport ?? value;
			((Control)panelDialogs).Visible = reportForCheck != null && reportForCheck.Dictionary.Variables.ToList().Any((StiVariable v) => v.RequestFromUser && (reportForCheck.GetType().GetField(v.Name) != null || reportForCheck.CalculationMode == StiCalculationMode.Interpretation || StiOptions.Engine.ForceInterpretationMode));
			CreateTabButtons();
			SelectedTab();
			IsFirstRendering = true;
			RenderView();
			RunRenderData();
			Form obj = ((Control)this).FindForm();
			if (obj != null)
			{
				((Control)obj).Invalidate();
			}
			((Control)this).Invalidate();
			if (dashboard?.Report != null && dashboard.Report.RefreshTime > 0)
			{
				StartRefreshReportTimer(dashboard.Report.RefreshTime);
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	internal Hashtable ElementToControl { get; } = new Hashtable();


	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	internal ToolTip ToolTipActionButtons => toolTipActionButtons;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	private bool IsRendering { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	private bool IsFirstRendering
	{
		get
		{
			string key = ((Dashboard?.GetKey() != null) ? Dashboard.GetKey() : "");
			if (isFirstRenderingDictionary.ContainsKey(key))
			{
				return isFirstRenderingDictionary[key];
			}
			return false;
		}
		set
		{
			string key = ((Dashboard?.GetKey() != null) ? Dashboard.GetKey() : "");
			isFirstRenderingDictionary[key] = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public StiElementFullScreenHelper ElementFullScreen { get; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public StiViewerFullScreenHelper ViewerFullScreen { get; }

	private bool IsShowWatermark
	{
		get
		{
			if (Dashboard?.DashboardWatermark != null)
			{
				return Dashboard.DashboardWatermark.IsVisible;
			}
			return false;
		}
	}

	public StiDashboardViewerControl()
		: this(null)
	{
	}

	public StiDashboardViewerControl(StiReport report, int startPage = -1)
	{
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Expected O, but got Unknown
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Expected O, but got Unknown
		StiGisMapRepaintHelper.Init();
		StiRegionMapZoomHelper.Reset();
		ElementFullScreen = new StiElementFullScreenHelper(this);
		ViewerFullScreen = new StiViewerFullScreenHelper(this);
		InitializeComponent();
		menuActions = new StiContextMenu();
		((Control)panelElements).Dock = (DockStyle)5;
		((Control)panelHScrollBar).Visible = false;
		((Control)VScrollBar).Visible = false;
		((Control)this).SetStyle((ControlStyles)131072, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		CreateParameters();
		lockCreateTabButtons = true;
		Report = report;
		report?.ProcessAutoLocalizeReportOnRun();
		lockCreateTabButtons = false;
		CreateToolbar(report);
		CreateTabButtons();
		SelectedDefaultTab(startPage);
		IsFirstRendering = true;
		StiCacheCleaner.Clean(dashboard);
		((UserControl)this).Load += This_Load;
		((Component)this).Disposed += This_Disposed;
		StiLicenseKey licenseKey = StiLicenseKeyValidator.GetLicenseKey();
		bool flag = !StiLicenseKeyValidator.IsValidInDashboardsDesignerOrOnPlatform(StiProductIdent.DbsWin, licenseKey);
		if (!typeof(StiLicense).AssemblyQualifiedName.Contains("PublicKeyToken=ebe6666cba19647a"))
		{
			flag = true;
		}
		if (!flag)
		{
			try
			{
				using RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(512);
				using SHA1CryptoServiceProvider halg = new SHA1CryptoServiceProvider();
				rSACryptoServiceProvider.FromXmlString("<RSAKeyValue><Modulus>2GCA4rfVor/2q7m+NVisL3ZFVvnklg14zeP0F+yNpBdoSm9iuOUBLodsOH6I3Xb0j5/OznYKFpii35RcjiPvww7Uthz1he2G8PGCvFMKuA4Wzgp5Kdv0uR7zSQ8J9lA/rZ7u8wh+h72umBku1pLFL2nvJIT56u4Qe7+OJFLrqWk=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>");
				flag = !rSACryptoServiceProvider.VerifyData(licenseKey.GetCheckBytes(), halg, licenseKey.GetSignatureBytes());
			}
			catch (Exception)
			{
				flag = true;
			}
		}
		if (flag)
		{
			string text = "2022.1.1.0";
			if (text.EndsWithInvariant(".0"))
			{
				text = text.Substring(0, text.Length - 2);
			}
			Label val = new Label
			{
				Text = text + " Trial Version",
				Font = new Font("Arial", 12f),
				BackColor = StiDashboardStyleHelper.GetDashboardBackColor(Dashboard, isViewer: false),
				ForeColor = Color.Red,
				TextAlign = (ContentAlignment)32,
				Anchor = (AnchorStyles)10
			};
			Size size = TextRenderer.MeasureText(((Control)val).Text, ((Control)val).Font);
			((Control)val).Height = size.Height * 2;
			((Control)val).Width = (int)((double)size.Width * 1.2);
			((Control)val).Top = ((Control)this).Bottom - ((Control)val).Height;
			((Control)val).Left = ((Control)this).Right - ((Control)val).Width;
			((Control)this).Controls.Add((Control)(object)val);
			((Control)val).BringToFront();
		}
	}

	void IStiViewerAccessor.CheckParametersButton()
	{
	}

	void IStiViewerAccessor.UncheckParametersButton()
	{
	}

	void IStiViewerAccessor.EnableParametersButton()
	{
	}

	IStiReport IStiViewerAccessor.GetReport()
	{
		return report;
	}

	IStiReport IStiViewerAccessor.GetCurrentReport()
	{
		return report;
	}

	Panel IStiViewerAccessor.GetPanelDialogs()
	{
		return panelDialogs;
	}

	IContainer IStiViewerAccessor.GetIContainer()
	{
		return components;
	}

	bool IStiViewerAccessor.OnSubmit()
	{
		Parameters.CheckVariables();
		ReportsCache.Remove(selectedPage.Guid);
		StiCacheCleaner.Clean(Dashboard);
		CleanUp(detachEvents: false, removeViewerControl: false);
		RefreshViewer();
		return true;
	}

	bool IStiViewerAccessor.OnReset()
	{
		return false;
	}

	void IStiViewerAccessor.OnShowParameters()
	{
		ApplyStyleColorsToDialogs();
	}

	public Panel GetElementsPanel()
	{
		return (Panel)(object)panelElements;
	}

	public Panel GetToolBarPanel()
	{
		return panelToolBar;
	}

	private void StartRefreshReportTimer(int timeout)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		if (timerRefreshReport == null)
		{
			timerRefreshReport = new Timer();
			timerRefreshReport.Tick += TimerRefreshReport_Tick;
		}
		else
		{
			Timer obj = timerRefreshReport;
			if (obj != null)
			{
				obj.Stop();
			}
		}
		timerRefreshReport.Interval = timeout * 1000;
		timerRefreshReport.Start();
	}

	private void StopRefreshReportTimer()
	{
		if (timerRefreshReport != null)
		{
			timerRefreshReport.Stop();
			timerRefreshReport.Tick -= TimerRefreshReport_Tick;
			((Component)(object)timerRefreshReport).Dispose();
			timerRefreshReport = null;
		}
	}

	private void StartDelayedWatermarkTimer()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		if (timerDelayedWatermark == null)
		{
			timerDelayedWatermark = new Timer();
			timerDelayedWatermark.Interval = 300;
			timerDelayedWatermark.Tick += TimerDelayedWatermark_Tick;
		}
		else
		{
			Timer obj = timerDelayedWatermark;
			if (obj != null)
			{
				obj.Stop();
			}
		}
		blockPaintingWatermark = true;
		timerDelayedWatermark.Start();
	}

	private void StopDelayedWatermarkTimer()
	{
		if (timerDelayedWatermark != null)
		{
			timerDelayedWatermark.Stop();
			timerDelayedWatermark.Tick -= TimerDelayedWatermark_Tick;
			((Component)(object)timerDelayedWatermark).Dispose();
			timerDelayedWatermark = null;
		}
	}

	private void CreateWatermarkImage()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		if (IsShowWatermark && !blockPaintingWatermark)
		{
			Bitmap val = new Bitmap(((Control)this).Width, ((Control)this).Height);
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				val2.Clear(StiDashboardStyleHelper.GetDashboardBackColor(Dashboard, isViewer: true).RemoveAlpha());
				double scale = 1.0;
				StiAdvancedWatermark watermark = Dashboard?.DashboardWatermark;
				StiAdvancedWatermarkGdiPainter.PaintWatermark(val2, watermark, null, new RectangleD(0.0, 0.0, ((Control)this).Width, ((Control)this).Height), scale);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			Image backgroundImage = ((Control)this).BackgroundImage;
			((Control)this).BackgroundImage = (Image)(object)val;
			if (backgroundImage != null)
			{
				backgroundImage.Dispose();
			}
		}
	}

	private void ApplyStyleColorsToDialogs()
	{
		StiControlFinder.FindControls<Label>((Control)(object)panelDialogs)?.ToList().ForEach(delegate(Label c)
		{
			if (StiDashboardStyleHelper.GetStyle(Dashboard) == StiElementStyleIdent.AliceBlue)
			{
				((Control)c).ForeColor = StiColor.Get("e4ffff");
			}
			else
			{
				((Control)c).ForeColor = StiDashboardStyleHelper.GetForeColor(Dashboard);
			}
		});
	}

	private void CreateParameters()
	{
		Type type = Type.GetType("Stimulsoft.Report.Viewer.StiParametersAccessor, Stimulsoft.Report.Win");
		if (type != null)
		{
			Parameters = StiActivator.CreateObject(type, new object[1] { this }) as IStiParametersAccessor;
		}
	}

	private void CheckUiState()
	{
		if (IsShowWatermark)
		{
			((Control)panelToolBar).BackColor = Color.Transparent;
		}
		else
		{
			((Control)panelToolBar).BackColor = ((selectedPage == null || selectedPage.IsDashboard) ? StiDashboardStyleHelper.GetDashboardBackColor(Dashboard, isViewer: true) : Color.White);
		}
		StiPage currentShowPage = GetCurrentShowPage();
		if (buttonViewerMenu != null && currentShowPage != null)
		{
			((Control)buttonViewerMenu).Enabled = currentShowPage.IsDashboard;
		}
		RefreshViewerButtons(Report);
		SetDarkModeAllTabs();
		ApplyStyleColorsToDialogs();
	}

	public void RenderData()
	{
		if (Dashboard == null || IsRendering)
		{
			return;
		}
		try
		{
			IsRendering = true;
			((Control)this).SuspendLayout();
			if (IsFirstRendering)
			{
				RenderDataFirstTime();
			}
			else
			{
				RefreshStatusAllElements(checkProgressStatus: true);
			}
			SetActionButtonToolTips();
		}
		finally
		{
			((Control)this).ResumeLayout();
			IsRendering = false;
		}
	}

	public void RenderView()
	{
		StiPage currentShowPage = GetCurrentShowPage();
		if (currentShowPage != null)
		{
			if (currentShowPage.IsDashboard)
			{
				RenderElements();
			}
			else
			{
				ShowReportPages();
			}
		}
	}

	public void RenderElements()
	{
		//IL_08c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cd: Invalid comparison between Unknown and I4
		panelElements.IsEmptyDashboard = Dashboard == null || Dashboard.Components.Count == 0;
		panelElements.IsShowWatermark = IsShowWatermark;
		if (Dashboard == null)
		{
			return;
		}
		try
		{
			((Control)this).SuspendLayout();
			ElementFullScreen.Close();
			Panel obj = panelCorner;
			int width = (((Control)VScrollBar).Width = SystemInformation.VerticalScrollBarWidth);
			((Control)obj).Width = width;
			((Control)panelHScrollBar).Height = SystemInformation.HorizontalScrollBarHeight;
			Panel obj2 = panelHScrollBar;
			Color backColor = (((Control)panelCorner).BackColor = StiDashboardStyleHelper.GetDashboardBackColor(Dashboard, isViewer: true));
			((Control)obj2).BackColor = backColor;
			CorrectElementLocations();
			AdoptDashboardViewMode();
			((Control)panelElements).SendToBack();
			List<IStiElement> elementList = StiDashboardViewerHelper.GetElementList(Dashboard);
			Dictionary<IStiElement, Rectangle> rects = elementList.Cast<StiComponent>().ToDictionary((StiComponent e) => e as IStiElement, (StiComponent e) => StiRectangleHelper.Round(e.GetPaintRectangle(convertToHInches: false, convertByZoom: false)));
			if (Alignment == StiDashboardContentAlignment.StretchXY || panelElements.IsEmptyDashboard)
			{
				((Control)panelElements).Dock = (DockStyle)5;
				((Control)panelHScrollBar).Visible = false;
				((Control)VScrollBar).Visible = false;
			}
			else if (Alignment == StiDashboardContentAlignment.StretchX)
			{
				((ScrollBar)VScrollBar).Value = 0;
				((Control)panelHScrollBar).Visible = false;
				((Control)panelElements).Dock = (DockStyle)0;
				((Control)panelElements).Location = new Point(0, 0);
				((Control)panelElements).Width = ((Control)panelHolder).Width - SystemInformation.VerticalScrollBarWidth;
				((Control)panelElements).Height = (int)(Dashboard.Height * StiScale.Factor) + dashboardVerticallyMargin * 2;
			}
			else if (Alignment == StiDashboardContentAlignment.Left || Alignment == StiDashboardContentAlignment.Center || Alignment == StiDashboardContentAlignment.Right)
			{
				((ScrollBar)HScrollBar).Value = 0;
				((ScrollBar)VScrollBar).Value = 0;
				((Control)panelElements).Dock = (DockStyle)0;
				((Control)panelElements).Location = new Point(0, 0);
				((Control)panelElements).Width = (int)(Dashboard.Width * StiScale.Factor) + dashboardHorizontallyMargin * 2;
				((Control)panelElements).Height = (int)(Dashboard.Height * StiScale.Factor) + dashboardVerticallyMargin * 2;
				if (Alignment == StiDashboardContentAlignment.Right)
				{
					if (((Control)panelHolder).Width > ((Control)panelElements).Width)
					{
						((Control)panelElements).Location = new Point(((Control)panelHolder).Width - ((Control)panelElements).Width, 0);
					}
				}
				else if (Alignment == StiDashboardContentAlignment.Center && ((Control)panelHolder).Width > ((Control)panelElements).Width)
				{
					((Control)panelElements).Location = new Point((((Control)panelHolder).Width - ((Control)panelElements).Width) / 2, 0);
				}
			}
			int width2 = ((Control)panelElements).Width;
			int height = ((Control)panelElements).Height;
			double num = (double)(width2 - dashboardHorizontallyMargin * 2) / Dashboard.Width;
			if (Alignment == StiDashboardContentAlignment.Left)
			{
				num = StiScale.Factor;
			}
			int num2 = dashboardHorizontallyMargin;
			int num3 = ((!((Control)panelToolBar).Visible) ? dashboardVerticallyMargin : 0);
			List<int> allLocations = GetAllLocations(elementList, rects);
			List<StiRangeBand> list = new List<StiRangeBand>();
			for (int i = 0; i < allLocations.Count - 1; i++)
			{
				list.Add(new StiRangeBand(allLocations[i], allLocations[i + 1]));
			}
			foreach (IStiElement element2 in elementList.Where((IStiElement e) => e is IStiFixedHeightElement && (e as IStiFixedHeightElement).IsFixedHeight))
			{
				list.Where((StiRangeBand b) => b.Intersect(rects[element2])).ToList().ForEach(delegate(StiRangeBand c)
				{
					c.IsFixed = true;
				});
			}
			int num4 = list.Where((StiRangeBand b) => b.IsFixed).Sum((StiRangeBand b) => b.Height);
			double num5 = (double)(height - dashboardVerticallyMargin * 2 - StiScale.I(num4)) / (Dashboard.Height - (double)num4);
			if (Alignment == StiDashboardContentAlignment.StretchX)
			{
				num5 = num;
				if (rects.Any())
				{
					((Control)panelElements).Height = (int)((double)rects.Max((KeyValuePair<IStiElement, Rectangle> r) => r.Value.Bottom) * num5) + dashboardVerticallyMargin * 2;
				}
				if (((Control)panelElements).Height < ((Control)panelHolder).Height)
				{
					StiElementPanel stiElementPanel = panelElements;
					((Control)stiElementPanel).Left = ((Control)stiElementPanel).Left + SystemInformation.VerticalScrollBarWidth / 2;
					((Control)VScrollBar).Visible = false;
				}
				else
				{
					((Control)VScrollBar).Visible = true;
					((ScrollBar)VScrollBar).LargeChange = ((Control)panelHolder).Height;
					((ScrollBar)VScrollBar).Maximum = ((Control)panelElements).Height;
				}
			}
			else if (Alignment == StiDashboardContentAlignment.Left || Alignment == StiDashboardContentAlignment.Center || Alignment == StiDashboardContentAlignment.Right)
			{
				num5 = num;
				((Control)panelHScrollBar).Visible = ((Control)panelElements).Width > ((Control)panelHolder).Width;
				VScrollBar vScrollBar = VScrollBar;
				bool visible = (((Control)panelCorner).Visible = ((Control)panelElements).Height > ((Control)panelHolder).Height);
				((Control)vScrollBar).Visible = visible;
				int num6 = ((Control)panelHolder).Height - (((Control)panelHScrollBar).Visible ? ((Control)panelHScrollBar).Height : 0);
				if (num6 > 0)
				{
					((ScrollBar)VScrollBar).LargeChange = num6;
				}
				((ScrollBar)VScrollBar).Maximum = ((Control)panelElements).Height;
				int num7 = ((Control)panelHolder).Width - (((Control)VScrollBar).Visible ? ((Control)VScrollBar).Width : 0);
				if (num7 > 0)
				{
					((ScrollBar)HScrollBar).LargeChange = num7;
				}
				((ScrollBar)HScrollBar).Maximum = ((Control)panelElements).Width;
			}
			int num8 = 0;
			foreach (StiRangeBand item in list)
			{
				item.Top = num8;
				int num9 = StiRectangleHelper.Round(item.IsFixed ? ((double)StiScale.YYI(item.OriginalHeight)) : ((double)item.OriginalHeight * num5));
				item.Bottom = item.Top + num9;
				num8 += num9;
			}
			foreach (IStiElement element in elementList)
			{
				StiComponent stiComponent = element as StiComponent;
				if (!stiComponent.ClientRectangle.IsEmpty)
				{
					int num10 = StiRectangleHelper.Round((double)rects[element].Left * num) + num2;
					int num11 = StiRectangleHelper.Round((double)rects[element].Right * num) + num2;
					int num12 = list.FirstOrDefault((StiRangeBand b) => b.OriginalTop == rects[element].Top).Top + num3;
					int num13 = list.FirstOrDefault((StiRangeBand b) => b.OriginalBottom == rects[element].Bottom).Bottom + num3;
					IStiMargin stiMargin = element as IStiMargin;
					if (stiMargin?.Margin != null)
					{
						num10 += (int)(stiMargin.Margin.Left * StiScale.X);
						num11 -= (int)(stiMargin.Margin.Right * StiScale.X);
						num12 += (int)(stiMargin.Margin.Top * StiScale.Y);
						num13 -= (int)(stiMargin.Margin.Bottom * StiScale.Y);
					}
					Control val = RenderElement(element);
					if ((int)val.Dock != 5)
					{
						if (!(stiComponent.Parent is StiPage))
						{
							object? obj3 = ElementToControl[stiComponent.Parent];
							Control val2 = (Control)((obj3 is Control) ? obj3 : null);
							if (val2 != null)
							{
								num10 -= val2.Left;
								num11 -= val2.Left;
								num12 -= val2.Top;
								num13 -= val2.Top;
							}
						}
						val.SetBounds(num10, num12, num11 - num10, num13 - num12);
					}
					val.Visible = true;
				}
				else
				{
					Control val3 = RenderElement(element);
					val3.Visible = false;
				}
			}
			SetActionButtonToolTips();
		}
		finally
		{
			((Control)this).ResumeLayout();
		}
	}

	private void CorrectElementLocations()
	{
		List<IStiElement> elementList = StiDashboardViewerHelper.GetElementList(Dashboard);
		foreach (StiComponent item in elementList)
		{
			if (item.Page != Dashboard)
			{
				continue;
			}
			if (item.Right > Dashboard.Width)
			{
				item.Left = Dashboard.Width - item.Width;
			}
			if (item.Bottom > Dashboard.Height)
			{
				item.Top = Dashboard.Height - item.Height;
			}
			if (item.Left < 0.0)
			{
				item.Left = 0.0;
				if (item.Width > Dashboard.Width)
				{
					item.Width = Dashboard.Width;
				}
			}
			if (item.Top < 0.0)
			{
				item.Top = 0.0;
				if (item.Height > Dashboard.Height)
				{
					item.Height = Dashboard.Height;
				}
			}
		}
	}

	private void AdoptDashboardViewMode()
	{
		if (Dashboard.IsMobileSurfacePresent)
		{
			int num = Dashboard.DeviceWidth;
			if (num <= 0)
			{
				num = 480;
			}
			StiSurfaceViewMode value = ((((Control)this).Width <= StiScale.I(num)) ? StiSurfaceViewMode.Mobile : StiSurfaceViewMode.Desktop);
			Dashboard.SwitchSurfaceViewMode(value);
		}
	}

	internal void RefreshViewer()
	{
		RenderView();
		RenderData();
	}

	internal void NewTab(StiPage page, string value, List<DictionaryEntry> parameters)
	{
		StiReport cloneReport = StiReportCopier.Copy(report);
		cloneReport.Key = StiKeyHelper.GenerateKey();
		(from p in cloneReport.Pages.ToList()
			where p.Guid != page.Guid
			select p).ToList().ForEach(delegate(StiPage p)
		{
			p.Enabled = false;
		});
		DictionaryEntry dictionaryEntry = parameters.FirstOrDefault((DictionaryEntry p) => p.Key?.ToString().ToLowerInvariant() == "title");
		StiPage stiPage = cloneReport.Pages.ToList().FirstOrDefault((StiPage p) => p.Guid == page.Guid);
		stiPage.Enabled = true;
		stiPage.Alias = ((!string.IsNullOrEmpty(dictionaryEntry.Value?.ToString())) ? dictionaryEntry.Value.ToString() : value);
		parameters?.ForEach(delegate(DictionaryEntry p)
		{
			cloneReport[p.Key?.ToString()] = p.Value;
		});
		cloneReport.Render(showProgress: false);
		ReportsCache[stiPage.Guid] = cloneReport;
		nestedPages.Add(stiPage);
		CreateTabButtons();
		SelectLastPage();
	}

	private void RenderDataFirstTime()
	{
		List<StiElementControl> source = (from c in StiDashboardViewerHelper.GetElementControlList(this)
			where c.IsQuerable && c.Element?.Page == Dashboard && !c.IsManuallyEnteredData
			select c).ToList();
		source.Where((StiElementControl c) => c.IsDefined).ToList().ForEach(delegate(StiElementControl c)
		{
			c.ShowProgress();
		});
		source.Where((StiElementControl c) => !c.IsDefined).ToList().ForEach(delegate(StiElementControl c)
		{
			c.IsShowNoResult = true;
		});
		List<IStiFilterElement> filterElements = StiDashboardViewerHelper.GetFilterElementList(Dashboard);
		if (filterElements.Any())
		{
			BackgroundWorker worker = new BackgroundWorker();
			worker.DoWork += delegate
			{
				filterElements.ForEach(delegate(IStiFilterElement f)
				{
					f.ApplyDefaultFilters();
				});
			};
			worker.RunWorkerCompleted += delegate
			{
				try
				{
					worker.Dispose();
					RefreshStatusAllElements(checkProgressStatus: false);
					IsFirstRendering = false;
				}
				catch
				{
				}
			};
			worker.RunWorkerAsync();
		}
		else
		{
			RefreshStatusAllElements(checkProgressStatus: false);
			IsFirstRendering = false;
		}
	}

	private Control RenderElement(IStiElement element)
	{
		object? obj = ElementToControl[element];
		Control val = (Control)((obj is Control) ? obj : null);
		if (val != null)
		{
			return val;
		}
		if (!(StiVisualizer.GetVisualizer(element.GetType(), StiGuiMode.Gdi) is StiGdiVisualizer stiGdiVisualizer))
		{
			return null;
		}
		Control val2 = stiGdiVisualizer.Render(element);
		bool showActionButtons = val2 is StiElementControl && ((StiElementControl)(object)val2).ShowActionButtons;
		val = (Control)(object)((element is IStiButtonElement || element is StiPanelElement) ? ((StiHolderElementControl)(object)val2) : new StiHolderElementControl(element, val2, showActionButtons));
		ElementToControl[element] = val;
		AddChild(element, val);
		StiActionButtonsHelper.AttachEvents(val, element, menuActions);
		return val;
	}

	private void AddChild(IStiElement element, Control control)
	{
		StiComponent stiComponent = element as StiComponent;
		ControlCollection val = null;
		if (stiComponent.Parent is StiPage)
		{
			val = StiDashboardViewerHelper.GetViewerControls(this);
		}
		else if (stiComponent.Parent.IsEnabled)
		{
			object? obj = ElementToControl[stiComponent.Parent];
			Control val2 = (Control)((obj is Control) ? obj : null);
			if (val2 == null)
			{
				return;
			}
			val = val2.Controls;
		}
		if (val != null)
		{
			val.Add(control);
			val.SetChildIndex(control, 0);
		}
	}

	private void RefreshStatusAllElements(bool checkProgressStatus)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		if (((Control)this).InvokeRequired)
		{
			((Control)this).Invoke((Delegate)(MethodInvoker)delegate
			{
				StiDashboardViewerHelper.GetElementControlList(this).ForEach(delegate(StiElementControl c)
				{
					c.RefreshStatus(checkProgressStatus);
				});
			});
		}
		else
		{
			StiDashboardViewerHelper.GetElementControlList(this).ForEach(delegate(StiElementControl c)
			{
				c.RefreshStatus(checkProgressStatus);
			});
		}
	}

	private void RunRenderData()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		if (timerRenderReport == null)
		{
			timerRenderReport = new Timer();
			timerRenderReport.Tick += delegate
			{
				timerRenderReport.Stop();
				RenderData();
			};
		}
		else if (timerRefreshReport != null)
		{
			timerRefreshReport.Stop();
		}
		timerRenderReport.Interval = 300;
		timerRenderReport.Start();
	}

	private void SetActionButtonToolTips()
	{
		ToolTipActionButtons.RemoveAll();
		List<Control> controlList = StiDashboardViewerHelper.GetControlList(((IEnumerable)((Control)this).Controls).Cast<Control>());
		(from StiActionButton c in controlList.Where((Control c) => c is StiActionButton)
			where !string.IsNullOrWhiteSpace(c.ToolTip)
			select c).ToList().ForEach(delegate(StiActionButton c)
		{
			ToolTipActionButtons.SetToolTip((Control)(object)c, c.ToolTip);
		});
		(from StiActionCheckBox c in controlList.Where((Control c) => c is StiActionCheckBox)
			where !string.IsNullOrWhiteSpace(c.ToolTip)
			select c).ToList().ForEach(delegate(StiActionCheckBox c)
		{
			ToolTipActionButtons.SetToolTip((Control)(object)c, c.ToolTip);
		});
	}

	private List<int> GetAllLocations(IEnumerable<IStiElement> elements, Dictionary<IStiElement, Rectangle> rects)
	{
		List<int> list = new List<int>();
		foreach (IStiElement element in elements)
		{
			list.Add(StiRectangleHelper.Round(rects[element].Top));
			list.Add(StiRectangleHelper.Round(rects[element].Bottom));
		}
		list.Add(0);
		list.Add(StiRectangleHelper.Round(Dashboard.Height));
		return (from y in list.Distinct()
			orderby y
			select y).ToList();
	}

	private void SelectedDefaultTab(int startPage)
	{
		if (startPage >= 0 && startPage < report.Pages.Count)
		{
			string guid = report.Pages[startPage].Guid;
			IEnumerable<CheckBox> controlsOfType = ((Control)panelToolBar).Controls.GetControlsOfType<CheckBox>();
			foreach (CheckBox item in controlsOfType)
			{
				StiPage stiPage = ((Control)item).Tag as StiPage;
				if (stiPage.Guid.Equals(guid))
				{
					CheckBoxButton_Click(item, EventArgs.Empty);
					return;
				}
			}
		}
		SelectedTab();
	}

	private void SelectedTab(int startPage = -1)
	{
		IEnumerable<CheckBox> controlsOfType = ((Control)panelToolBar).Controls.GetControlsOfType<CheckBox>();
		CheckBox sender = (CheckBox)((startPage != -1) ? ((startPage < controlsOfType.Count()) ? ((object)controlsOfType.ElementAt(startPage)) : ((object)controlsOfType.FirstOrDefault())) : controlsOfType.FirstOrDefault());
		CheckBoxButton_Click(sender, EventArgs.Empty);
	}

	private void CreateToolbar(StiReport report)
	{
		RefreshViewerButtons(report);
		((Control)panelToolBar).Visible = GetMainToolbarVisibility(report);
	}

	internal bool GetMainToolbarVisibility(StiReport report)
	{
		if (report == null || (report.DashboardViewerSettings & StiDashboardViewerSettings.ShowToolBar) > StiDashboardViewerSettings.None)
		{
			return StiOptions.Viewer.Windows.ShowToolbar.GetValueOrDefault(true);
		}
		return false;
	}

	internal void RefreshViewerButtons(StiReport report)
	{
		StiControlColorCollection colors = ((selectedPage != null && selectedPage.IsPage) ? StiControlColorCollection.GetActionColors() : StiControlColorCollection.GetActionColors(Dashboard));
		bool visible = GetCurrentShowPage()?.IsDashboard ?? false;
		(from Control c in (IEnumerable)((Control)panelToolBar).Controls
			where c is StiActionButton
			select c).ToList().ForEach(delegate(Control c)
		{
			((Control)panelToolBar).Controls.Remove(c);
		});
		int num = ((Control)this).Width - StiScale.XXI(dashboardHorizontallyMargin) - StiScale.I1;
		if (StiOptions.Viewer.Windows.ShowSaveButton.GetValueOrDefault(true) && (StiOptions.Viewer.Windows.Exports.ShowImage || StiOptions.Viewer.Windows.Exports.ShowPdf || StiOptions.Viewer.Windows.Exports.ShowExcel) && (report == null || (report.DashboardViewerSettings & StiDashboardViewerSettings.ShowMenuButton) > StiDashboardViewerSettings.None))
		{
			buttonViewerMenu = new StiViewerMenuActionButton(colors);
			((Control)buttonViewerMenu).Location = new Point(num - StiScale.XXI(24.0), StiScale.I4);
			((Control)buttonViewerMenu).Visible = visible;
			((Control)buttonViewerMenu).Click += delegate(object sender, EventArgs e)
			{
				Control val = (Control)((sender is Control) ? sender : null);
				new StiActionsMenu(menuActions, (Control)(object)panelElements, Dashboard).CreateItems(Dashboard);
				Point point = new Point(val.Bounds.Right, val.Bounds.Bottom + StiScale.I1);
				point = ((Control)this).PointToScreen(point);
				((ToolStripDropDown)menuActions).Show(point, (ToolStripDropDownDirection)2);
			};
			((Control)panelToolBar).Controls.Add((Control)(object)buttonViewerMenu);
			((Control)buttonViewerMenu).BringToFront();
			num -= ((Control)buttonViewerMenu).Width + StiScale.I4;
		}
		if (report != null && (report.DashboardViewerSettings & StiDashboardViewerSettings.ShowFullScreenButton) > StiDashboardViewerSettings.None && !ViewerFullScreen.IsActivated && StiOptions.Viewer.Windows.ShowFullScreen.GetValueOrDefault(true))
		{
			buttonOpenViewerFullScreen = new StiOpenViewerFullScreenActionButton(colors);
			((Control)buttonOpenViewerFullScreen).Location = new Point(num - StiScale.XXI(24.0), StiScale.I4);
			((Control)buttonOpenViewerFullScreen).Visible = visible;
			((Control)buttonOpenViewerFullScreen).Click += delegate
			{
				ViewerFullScreen.Show();
			};
			((Control)panelToolBar).Controls.Add((Control)(object)buttonOpenViewerFullScreen);
			((Control)buttonOpenViewerFullScreen).BringToFront();
			num -= ((Control)buttonOpenViewerFullScreen).Width + StiScale.I4;
		}
		if (report != null && (report.DashboardViewerSettings & StiDashboardViewerSettings.ShowFullScreenButton) > StiDashboardViewerSettings.None && ViewerFullScreen.IsActivated && StiOptions.Viewer.Windows.ShowFullScreen.GetValueOrDefault(true))
		{
			string text = Loc.Get("Buttons", "Close");
			StiCloseViewerFullScreenActionButton stiCloseViewerFullScreenActionButton = new StiCloseViewerFullScreenActionButton(colors, new Point(0, StiScale.I4));
			((Control)stiCloseViewerFullScreenActionButton).Text = text;
			((Control)stiCloseViewerFullScreenActionButton).Visible = visible;
			buttonCloseViewerFullScreen = stiCloseViewerFullScreenActionButton;
			Size size = TextRenderer.MeasureText(text, ((Control)buttonCloseViewerFullScreen).Font);
			((Control)buttonCloseViewerFullScreen).Width = StiScale.XXI(22.0) + size.Width + StiScaleUI.I10;
			((Control)buttonCloseViewerFullScreen).Left = num - ((Control)buttonCloseViewerFullScreen).Width - StiScale.I2;
			((Control)buttonCloseViewerFullScreen).Click += delegate
			{
				ViewerFullScreen.Close();
			};
			((Control)panelToolBar).Controls.Add((Control)(object)buttonCloseViewerFullScreen);
			((Control)buttonCloseViewerFullScreen).BringToFront();
			num -= ((Control)buttonCloseViewerFullScreen).Width + StiScale.I4;
		}
		if (report != null && (report.DashboardViewerSettings & StiDashboardViewerSettings.ShowEditButton) > StiDashboardViewerSettings.None && StiOptions.Viewer.Windows.ShowPageDesignButton.GetValueOrDefault(true))
		{
			buttonEditDashboard = new StiEditDashboardActionButton(colors);
			((Control)buttonEditDashboard).Location = new Point(num - StiScale.XXI(24.0), StiScale.I4);
			((Control)buttonEditDashboard).Visible = visible;
			((Control)buttonEditDashboard).Click += delegate
			{
				//IL_0098: Unknown result type (might be due to invalid IL or missing references)
				//IL_004c: Unknown result type (might be due to invalid IL or missing references)
				try
				{
					if (Report.CalculationMode == StiCalculationMode.Compilation)
					{
						string text2 = (Loc.IsRussian ? "https://www.stimulsoft.com/ru/documentation/online/user-manual/index.html?reports_designer_template_calculation_mode.htm" : "https://www.stimulsoft.com/en/documentation/online/user-manual/index.html?reports_designer_template_calculation_mode.htm");
						MessageBox.Show((IWin32Window)(object)((Control)this).FindForm(), Loc.Get("Dashboard", "ViewEditCompilationNotice"), ((Control)((Control)this).FindForm()).Text, (MessageBoxButtons)0, (MessageBoxIcon)48, (MessageBoxDefaultButton)0, (MessageBoxOptions)0, text2, (HelpNavigator)(-2147483645));
					}
					else
					{
						StiOptions.Engine.GlobalEvents.ClosedDesigner += GlobalEvents_ClosedDesigner;
						StiReport stiReport = new StiReport();
						stiReport.IsPageDesigner = true;
						stiReport.LoadFromString(Report.SaveToString());
						stiReport.RegData(Report.DataStore);
						stiReport.Design(dialogForm: false);
					}
				}
				catch (Exception exception2)
				{
					StiExceptionProvider.Show(exception2);
				}
			};
			((Control)panelToolBar).Controls.Add((Control)(object)buttonEditDashboard);
			((Control)buttonEditDashboard).BringToFront();
			num -= ((Control)buttonEditDashboard).Width + StiScale.I4;
		}
		if ((report == null || (report.DashboardViewerSettings & StiDashboardViewerSettings.ShowOpenButton) > StiDashboardViewerSettings.None) && StiOptions.Viewer.Windows.ShowOpenButton.GetValueOrDefault(true))
		{
			buttonOpenDashboard = new StiOpenDashboardActionButton(colors);
			((Control)buttonOpenDashboard).Location = new Point(num - StiScale.XXI(24.0), StiScale.I4);
			((Control)buttonOpenDashboard).Visible = visible;
			((Control)buttonOpenDashboard).Click += delegate
			{
				try
				{
					StiOpenDialog stiOpenDialog = new StiOpenDialog(this);
					stiOpenDialog.Load();
					stiOpenDialog.Clear();
					((Control)panelToolBar).Visible = Dashboard != null;
				}
				catch (Exception exception)
				{
					StiExceptionProvider.Show(exception);
				}
			};
			((Control)panelToolBar).Controls.Add((Control)(object)buttonOpenDashboard);
			((Control)buttonOpenDashboard).BringToFront();
			num -= ((Control)buttonOpenDashboard).Width + StiScale.I4;
		}
		if (report != null && (report.DashboardViewerSettings & StiDashboardViewerSettings.ShowRefreshButton) > StiDashboardViewerSettings.None && StiOptions.Viewer.Windows.ShowRefreshButton.GetValueOrDefault(true))
		{
			buttonRefreshDashboard = new StiRefreshDashboardActionButton(colors);
			((Control)buttonRefreshDashboard).Location = new Point(num - StiScale.XXI(24.0), StiScale.I4);
			((Control)buttonRefreshDashboard).Visible = visible;
			((Control)buttonRefreshDashboard).Click += delegate
			{
				RefreshDashboard();
			};
			((Control)panelToolBar).Controls.Add((Control)(object)buttonRefreshDashboard);
			((Control)buttonRefreshDashboard).BringToFront();
		}
	}

	private void RefreshDashboard()
	{
		try
		{
			Dashboard?.Report.InvokeRefreshing();
			ReportsCache.Remove(selectedPage.Guid);
			StiCacheCleaner.Clean(Dashboard);
			RefreshViewer();
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	private void CreateTabButtons()
	{
		if (lockCreateTabButtons)
		{
			return;
		}
		(from Control c in (IEnumerable)((Control)panelToolBar).Controls
			where c is StiDashboardTabsPanel || c is StiTabButton
			select c).ToList().ForEach(delegate(Control c)
		{
			((Control)panelToolBar).Controls.Remove(c);
		});
		StiDashboardTabsPanel stiDashboardTabsPanel = new StiDashboardTabsPanel();
		((Control)stiDashboardTabsPanel).Height = ((Control)panelToolBar).Height;
		((Control)stiDashboardTabsPanel).Width = ((Control)panelToolBar).Width - 154 - dashboardHorizontallyMargin;
		StiDashboardTabsPanel stiDashboardTabsPanel2 = stiDashboardTabsPanel;
		((Control)panelToolBar).Controls.Add((Control)(object)stiDashboardTabsPanel2);
		int num = StiScale.I4;
		int i = StiScale.I4;
		List<StiPage> list = StiReportPagesHelper.FetchAllPages(report)?.Where((StiPage d) => d.IsEnabled).ToList();
		if (list == null)
		{
			return;
		}
		list.AddRange(nestedPages);
		foreach (StiPage item in list)
		{
			string text = ((!string.IsNullOrEmpty(item.Alias)) ? item.Alias : item.Name);
			bool isCheck = item.Equals(selectedPage);
			StiTabButton checkBoxNoFocus = GetCheckBoxNoFocus(text, new Point(num, i), isCheck, item);
			((Control)checkBoxNoFocus).Visible = list.Count > 1;
			((Control)stiDashboardTabsPanel2).Controls.Add((Control)(object)checkBoxNoFocus);
			num += ((Control)checkBoxNoFocus).Width + StiScale.I4;
		}
	}

	internal void SelectLastPage()
	{
		Control val = ((IEnumerable)((Control)panelToolBar).Controls).Cast<Control>().LastOrDefault((Func<Control, bool>)((Control c) => c is StiDashboardTabsPanel));
		if (val != null && ((IEnumerable)val.Controls).Cast<Control>().LastOrDefault((Func<Control, bool>)((Control c) => c is StiDashboardTabsPanel || c is StiTabButton)) is StiTabButton sender)
		{
			CheckBoxButton_Click(sender, EventArgs.Empty);
		}
	}

	private StiTabButton GetCheckBoxNoFocus(string text, Point location, bool isCheck, object tag)
	{
		StiTabButton stiTabButton = new StiTabButton(StiControlColorCollection.GetActionColors(Dashboard), location);
		stiTabButton.ShowCloseButton = true;
		((Control)stiTabButton).Tag = tag;
		((CheckBox)stiTabButton).Checked = isCheck;
		((Control)stiTabButton).Text = GetMeasureText(text, ((Control)stiTabButton).Font, StiScale.XXI(120.0));
		stiTabButton.CalculateAutoWidth();
		((Control)stiTabButton).Click += CheckBoxButton_Click;
		stiTabButton.CloseButtonClick += CheckBoxButton_CloseButtonClick;
		return stiTabButton;
	}

	private string GetMeasureText(string text, Font font, double maxWidth)
	{
		for (int i = 0; text.Length > i; i++)
		{
			string text2 = text.Substring(0, i);
			if (maxWidth < (double)TextRenderer.MeasureText(text2, font).Width)
			{
				return text2 + "...";
			}
		}
		return text;
	}

	internal void RemoveFilter(IStiElement element)
	{
		if (element is IStiUserFilters stiUserFilters)
		{
			stiUserFilters.UserFilters = new List<StiDataFilterRule>();
			RenderData();
		}
	}

	internal IStiElement GetElementFromControl(Control elementControl)
	{
		if (!(elementControl is StiHolderElementControl))
		{
			return null;
		}
		return (elementControl as StiHolderElementControl).Element;
	}

	internal void CleanUp(bool detachEvents = true, bool removeViewerControl = true)
	{
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Expected O, but got Unknown
		isFirstRenderingDictionary.Clear();
		ElementToControl.Clear();
		if (removeViewerControl)
		{
			((Control)GetElementsPanel()).Controls.Clear();
		}
		else
		{
			ControlCollection controls = ((Control)GetElementsPanel()).Controls;
			for (int num = ((ArrangedElementCollection)controls).Count - 1; num >= 0; num--)
			{
				if (!(controls[num] is IStiViewerControl))
				{
					controls.RemoveAt(num);
				}
			}
		}
		if (ownerForm != null && detachEvents)
		{
			((Control)ownerForm).KeyDown -= new KeyEventHandler(This_KeyDown);
		}
		((Control)this).Controls.GetControlsOfType<StiElementControl>().ToList().ForEach(delegate(StiElementControl c)
		{
			c.RemoveProgress();
		});
	}

	private void HideAllElementToControl()
	{
		ElementToControl.Values.OfType<Control>().ToList().ForEach(delegate(Control c)
		{
			c.Visible = false;
		});
	}

	internal void ResetAllTabs()
	{
		((Control)panelToolBar).Controls.GetControlsOfType<CheckBox>().ToList().ForEach(delegate(CheckBox c)
		{
			c.Checked = false;
		});
	}

	private void SetDarkModeAllTabs()
	{
		StiControlColorCollection colors = ((selectedPage != null && selectedPage.IsPage) ? StiControlColorCollection.GetActionColors() : StiControlColorCollection.GetActionColors(Dashboard));
		if (StiDashboardStyleHelper.GetStyle(Dashboard) == StiElementStyleIdent.AliceBlue)
		{
			colors.ForeColor = StiColor.Get("e4ffff");
			colors.HotForeColor = StiColor.Get("e4ffff");
			colors.HotSelectedForeColor = StiColor.Get("e4ffff");
		}
		((Control)panelToolBar).Controls.GetControlsOfType<StiActionCheckBox>().ToList().ForEach(delegate(StiActionCheckBox c)
		{
			c.Colors = colors;
		});
	}

	private StiPage GetCurrentShowPage()
	{
		CheckBox selectedTab = GetSelectedTab();
		if (selectedTab != null)
		{
			return (StiPage)((Control)selectedTab).Tag;
		}
		return null;
	}

	internal CheckBox GetSelectedTab()
	{
		return ((Control)panelToolBar).Controls.GetControlsOfType<CheckBox>().FirstOrDefault((Func<CheckBox, bool>)((CheckBox c) => c.Checked));
	}

	private void ShowReportPages()
	{
		((Control)panelElements).Dock = (DockStyle)5;
		if (reportViewer == null)
		{
			UserControl viewerControl = GetViewerControl();
			((Control)viewerControl).Dock = (DockStyle)5;
			((object)viewerControl).GetType().GetProperty("HideDisabledPagesBeforeExport")?.SetValue(viewerControl, true);
			((object)viewerControl).GetType().GetProperty("ReportTabHelper")?.SetValue(viewerControl, new StiDashboardReportTabHelper(this));
			((Control)panelElements).Controls.Add((Control)(object)viewerControl);
		}
		if (!((Control)panelElements).Controls.Contains((Control)(object)reportViewer))
		{
			((Control)panelElements).Controls.Add((Control)(object)reportViewer);
		}
		if (ReportsCache.ContainsKey(selectedPage.Guid))
		{
			((IStiViewerControl)reportViewer).Report = ReportsCache[selectedPage.Guid] as StiReport;
		}
		else if (report.IsRendered)
		{
			ReportsCache[selectedPage.Guid] = report;
		}
		else
		{
			StiReport stiReport = StiReportCopier.Copy(report);
			(from p in stiReport.Pages.OfType<StiPage>().ToList()
				where p.Guid != selectedPage.Guid
				select p).ToList().ForEach(delegate(StiPage p)
			{
				p.Enabled = false;
			});
			if (!stiReport.IsRendered)
			{
				stiReport.Render(showProgress: false);
			}
			((IStiViewerControl)reportViewer).Report = stiReport;
			ReportsCache[selectedPage.Guid] = stiReport;
		}
		((Control)reportViewer).Visible = true;
	}

	public void AddReportToTabs(StiPage sourcePage, StiReport report)
	{
		sourcePage = sourcePage.Clone() as StiPage;
		sourcePage.NewGuid();
		if (report.Variables != null && !string.IsNullOrWhiteSpace(report.Variables["Title"] as string))
		{
			sourcePage.Alias = report.Variables["Title"] as string;
		}
		else if (!string.IsNullOrWhiteSpace(report.ReportAlias))
		{
			sourcePage.Alias = report.ReportAlias;
		}
		else if (!string.IsNullOrWhiteSpace(report.ReportName))
		{
			sourcePage.Alias = report.ReportName;
		}
		nestedPages.Add(sourcePage);
		ReportsCache[sourcePage.Guid] = report;
		CreateTabButtons();
		SelectLastPage();
	}

	private void HideReportPages()
	{
		if (reportViewer != null)
		{
			((Control)reportViewer).Visible = false;
		}
	}

	private UserControl GetViewerControl()
	{
		if (reportViewer == null)
		{
			Type type = Type.GetType("Stimulsoft.Report.Viewer.StiViewerControl, Stimulsoft.Report.Win, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a");
			if (type == null)
			{
				throw new Exception("Assembly 'Stimulsoft.Report.Win' is not found");
			}
			ref UserControl reference = ref reportViewer;
			object obj = StiActivator.CreateObject(type, new object[1]);
			reference = (UserControl)((obj is UserControl) ? obj : null);
		}
		return reportViewer;
	}

	protected override void OnMouseWheel(MouseEventArgs e)
	{
		((ScrollableControl)this).OnMouseWheel(e);
		if (!((Control)VScrollBar).Visible)
		{
			return;
		}
		Point pos = ((Control)panelElements).PointToClient(Cursor.Position);
		StiHolderElementControl stiHolderElementControl = ((IEnumerable)((Control)panelElements).Controls).Cast<Control>().FirstOrDefault((Func<Control, bool>)((Control c) => c.Bounds.Contains(pos))) as StiHolderElementControl;
		StiChartElementControl stiChartElementControl = stiHolderElementControl?.Child as StiChartElementControl;
		if (!(stiHolderElementControl?.Child is StiRegionMapElementControl) && (stiChartElementControl == null || !stiChartElementControl.IsPie3d))
		{
			int num = ((ScrollBar)VScrollBar).Value - e.Delta;
			if (num < 0)
			{
				num = 0;
			}
			if (num + ((ScrollBar)VScrollBar).LargeChange > ((ScrollBar)VScrollBar).Maximum)
			{
				num = ((ScrollBar)VScrollBar).Maximum - ((ScrollBar)VScrollBar).LargeChange;
			}
			((ScrollBar)VScrollBar).Value = num;
		}
	}

	private void This_Disposed(object sender, EventArgs e)
	{
		((Component)this).Disposed -= This_Disposed;
		StopRefreshReportTimer();
		StopDelayedWatermarkTimer();
	}

	private void This_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			ElementFullScreen.Close();
		}
	}

	private void This_Load(object sender, EventArgs e)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		((UserControl)this).Load -= This_Load;
		Timer val = new Timer
		{
			Interval = 3000
		};
		val.Tick += TrialTimer_Tick;
		val.Start();
	}

	protected override void OnParentChanged(EventArgs e)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		((ContainerControl)this).OnParentChanged(e);
		if (ownerForm == null)
		{
			ownerForm = ((Control)this).FindForm();
			if (ownerForm != null)
			{
				((Control)ownerForm).KeyDown += new KeyEventHandler(This_KeyDown);
			}
		}
	}

	protected override void OnPaintBackground(PaintEventArgs e)
	{
		if (((Control)this).BackgroundImage != null)
		{
			((ScrollableControl)this).OnPaintBackground(e);
		}
		else
		{
			e.Graphics.Clear(StiDashboardStyleHelper.GetDashboardBackColor(Dashboard, isViewer: true).RemoveAlpha());
		}
	}

	protected override void OnSizeChanged(EventArgs e)
	{
		((Control)this).OnSizeChanged(e);
		StartDelayedWatermarkTimer();
	}

	private void Viewer_RefreshViewer(object sender, EventArgs e)
	{
		RefreshDashboard();
		((Control)panelElements).Invalidate();
	}

	private void Viewer_RefreshControl(object sender, StiReportControlUpdateEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		if (((Control)this).InvokeRequired)
		{
			((Control)this).Invoke((Delegate)(MethodInvoker)delegate
			{
				Viewer_RefreshControlInternal(sender, e);
			});
		}
		else
		{
			Viewer_RefreshControlInternal(sender, e);
		}
	}

	private void Viewer_RefreshControlInternal(object sender, StiReportControlUpdateEventArgs e)
	{
		IStiElement stiElement = sender as IStiElement;
		if (sender == Dashboard)
		{
			if (e.PropertyName == "Style")
			{
				((Control)this).Invalidate();
				StiDashboardViewerHelper.GetRefreshControlList(this).ForEach(delegate(IStiRefreshControl c)
				{
					c.RefreshControl(c.Element, e);
				});
				ApplyStyleColorsToDialogs();
				CheckUiState();
			}
			return;
		}
		switch (e.PropertyName)
		{
		case "DataFilters":
		case "UserFilters":
		case "UserSorts":
		case "SeriesColors":
			RenderData();
			break;
		case "Margin":
			RenderView();
			if (stiElement is IStiSimpleShadow stiSimpleShadow && stiSimpleShadow?.Shadow != null && stiSimpleShadow.Shadow.Visible)
			{
				((Control)this).Invalidate();
			}
			break;
		case "CornerRadius":
			if (stiElement is IStiSimpleShadow stiSimpleShadow2 && stiSimpleShadow2?.Shadow != null && stiSimpleShadow2.Shadow.Visible)
			{
				((Control)this).Invalidate();
			}
			break;
		case "Shadow":
			((Control)this).Invalidate();
			break;
		case "Enabled":
		{
			Control controlByElement = StiDashboardViewerHelper.GetControlByElement(this, stiElement);
			if (controlByElement != null)
			{
				controlByElement.Visible = (sender as StiElement).Enabled;
			}
			break;
		}
		}
		StiDashboardViewerHelper.GetRefreshControlList(this, stiElement).ForEach(delegate(IStiRefreshControl c)
		{
			c.RefreshControl(sender, e);
		});
	}

	private void GlobalEvents_ClosedDesigner(object sender, EventArgs e)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Invalid comparison between Unknown and I4
		IStiDesignerBase stiDesignerBase = sender as IStiDesignerBase;
		if (!(stiDesignerBase.Report.Key != Report.Key))
		{
			IStiDesignerBase obj = ((stiDesignerBase is Control) ? stiDesignerBase : null);
			Form val = ((obj != null) ? ((Control)obj).FindForm() : null);
			if (val != null && (int)val.DialogResult == 6)
			{
				StiReport stiReport = Report;
				stiReport.LoadFromString(stiDesignerBase.Report.SaveToString());
				Report = null;
				Report = stiReport;
				ReportsCache.Clear();
				StiCacheCleaner.Clean(Dashboard);
				RefreshViewer();
			}
		}
	}

	private void PanelElements_SizeChanged(object sender, EventArgs e)
	{
		StartDelayedWatermarkTimer();
		ViewerFullScreen.Close();
		ElementFullScreen.Close();
		RenderView();
	}

	private void CheckBoxButton_Click(object sender, EventArgs e)
	{
		if (lockCreateTabButtons)
		{
			return;
		}
		lockCreateTabButtons = true;
		blockPaintingWatermark = true;
		Image backgroundImage = ((Control)this).BackgroundImage;
		try
		{
			((Control)this).SuspendLayout();
			((Control)this).BackgroundImage = null;
			CheckBox val = (CheckBox)((sender is CheckBox) ? sender : null);
			if (val == null || val.Checked)
			{
				return;
			}
			ResetAllTabs();
			val.Checked = true;
			HideAllElementToControl();
			if (((Control)val).Tag is StiPage stiPage)
			{
				if (stiPage.IsDashboard)
				{
					HideReportPages();
					Dashboard = stiPage as StiDashboard;
				}
				selectedPage = stiPage;
				if (!stiPage.IsDashboard)
				{
					ShowReportPages();
					Dashboard = null;
					Form obj = ((Control)this).FindForm();
					if (obj != null)
					{
						((Control)obj).Invalidate();
					}
					((Control)this).Invalidate();
				}
			}
			CheckUiState();
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
		finally
		{
			((Control)this).BackgroundImage = backgroundImage;
			((Control)this).ResumeLayout();
			blockPaintingWatermark = false;
			lockCreateTabButtons = false;
		}
	}

	private void CheckBoxButton_CloseButtonClick(object sender, EventArgs e)
	{
		CheckBox val = (CheckBox)((sender is CheckBox) ? sender : null);
		if (val == null)
		{
			return;
		}
		List<CheckBox> list = ((Control)panelToolBar).Controls.GetControlsOfType<CheckBox>().ToList();
		int num = list.IndexOf(val);
		List<StiPage> list2 = StiReportPagesHelper.FetchAllPages(report)?.Where((StiPage d) => d.Enabled).ToList();
		if (list2 != null)
		{
			if (num < list2.Count)
			{
				list2[num].Enabled = false;
			}
			else if (num < list2.Count + nestedPages.Count)
			{
				nestedPages.RemoveAt(num - list2.Count);
			}
			num = Math.Min(num, list2.Count + nestedPages.Count - 1);
			num = Math.Max(num, 0);
			CreateTabButtons();
			SelectedTab(num);
		}
	}

	private void TimerRefreshScrollView_Tick(object sender, EventArgs e)
	{
		timerRefreshScrollView.Stop();
		StiDashboardViewerHelper.GetSeriesControlList(this).ForEach(delegate(IStiSeriesControl c)
		{
			c.RefreshScrollView();
		});
	}

	private void TimerRefreshReport_Tick(object sender, EventArgs e)
	{
		Dashboard?.Report.InvokeRefreshing();
		ReportsCache.Clear();
		StiCacheCleaner.Clean(Dashboard);
		if (StiOptions.Viewer.DashboardViewerRefreshing == StiViewerRefreshingMode.Full)
		{
			CleanUp(detachEvents: false);
		}
		RefreshViewer();
	}

	private void TimerDelayedWatermark_Tick(object sender, EventArgs e)
	{
		timerDelayedWatermark.Stop();
		blockPaintingWatermark = false;
		CreateWatermarkImage();
	}

	internal void FullScreenForm_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			e.Handled = true;
			ViewerFullScreen.Close();
		}
	}

	private void TrialTimer_Tick(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
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
			((Form)stiUsingTrialVersionForm).ShowDialog();
		}
		finally
		{
			((IDisposable)stiUsingTrialVersionForm)?.Dispose();
		}
	}

	private void VScrollBar_ValueChanged(object sender, EventArgs e)
	{
		((Control)panelElements).Top = -((ScrollBar)VScrollBar).Value;
	}

	private void HScrollBar_ValueChanged(object sender, EventArgs e)
	{
		((Control)panelElements).Left = -((ScrollBar)HScrollBar).Value;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
		CleanUp();
		StiCacheCleaner.Clean(Dashboard?.GetDictionary());
		StiOptions.Engine.GlobalEvents.ClosedDesigner -= GlobalEvents_ClosedDesigner;
		((Component)(object)menuActions).Dispose();
	}

	private void InitializeComponent()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0485: Unknown result type (might be due to invalid IL or missing references)
		//IL_051f: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		timerRefreshScrollView = new Timer(components);
		panelElements = new StiElementPanel();
		panelToolBar = new Panel();
		toolTipActionButtons = new ToolTip(components);
		panelDialogs = new Panel();
		panelHolder = new Panel();
		VScrollBar = new VScrollBar();
		panelHScrollBar = new Panel();
		HScrollBar = new HScrollBar();
		panelCorner = new Panel();
		((Control)panelHolder).SuspendLayout();
		((Control)panelHScrollBar).SuspendLayout();
		((Control)this).SuspendLayout();
		timerRefreshScrollView.Tick += TimerRefreshScrollView_Tick;
		panelElements.IsEmptyDashboard = false;
		((Control)panelElements).Location = new Point(24, 27);
		((Control)panelElements).Margin = new Padding(4, 4, 4, 4);
		((Control)panelElements).Name = "panelElements";
		((Control)panelElements).Size = new Size(94, 94);
		((Control)panelElements).TabIndex = 13;
		((Control)panelToolBar).BackColor = Color.Transparent;
		((Control)panelToolBar).Dock = (DockStyle)1;
		((Control)panelToolBar).Location = new Point(0, 0);
		((Control)panelToolBar).Margin = new Padding(4, 4, 4, 4);
		((Control)panelToolBar).Name = "panelToolBar";
		((Control)panelToolBar).Size = new Size(540, 54);
		((Control)panelToolBar).TabIndex = 14;
		((Control)panelDialogs).BackColor = Color.Transparent;
		((Control)panelDialogs).Dock = (DockStyle)1;
		((Control)panelDialogs).Location = new Point(0, 54);
		((Control)panelDialogs).Margin = new Padding(12, 12, 12, 12);
		((Control)panelDialogs).Name = "panelDialogs";
		((Control)panelDialogs).Size = new Size(540, 54);
		((Control)panelDialogs).TabIndex = 15;
		((Control)panelDialogs).Visible = false;
		((Control)panelHolder).BackColor = Color.Transparent;
		((Control)panelHolder).Controls.Add((Control)(object)VScrollBar);
		((Control)panelHolder).Controls.Add((Control)(object)panelElements);
		((Control)panelHolder).Controls.Add((Control)(object)panelHScrollBar);
		((Control)panelHolder).Dock = (DockStyle)5;
		((Control)panelHolder).Location = new Point(0, 108);
		((Control)panelHolder).Margin = new Padding(4, 4, 4, 4);
		((Control)panelHolder).Name = "panelHolder";
		((Control)panelHolder).Size = new Size(540, 552);
		((Control)panelHolder).TabIndex = 16;
		((Control)panelHolder).SizeChanged += PanelElements_SizeChanged;
		((Control)VScrollBar).Dock = (DockStyle)4;
		((Control)VScrollBar).Location = new Point(522, 0);
		((Control)VScrollBar).Name = "VScrollBar";
		((Control)VScrollBar).Size = new Size(18, 517);
		((Control)VScrollBar).TabIndex = 14;
		((ScrollBar)VScrollBar).ValueChanged += VScrollBar_ValueChanged;
		((Control)panelHScrollBar).Controls.Add((Control)(object)HScrollBar);
		((Control)panelHScrollBar).Controls.Add((Control)(object)panelCorner);
		((Control)panelHScrollBar).Dock = (DockStyle)2;
		((Control)panelHScrollBar).Location = new Point(0, 517);
		((Control)panelHScrollBar).Margin = new Padding(2, 2, 2, 2);
		((Control)panelHScrollBar).Name = "panelHScrollBar";
		((Control)panelHScrollBar).Size = new Size(540, 35);
		((Control)panelHScrollBar).TabIndex = 15;
		((Control)HScrollBar).Dock = (DockStyle)5;
		((Control)HScrollBar).Location = new Point(0, 0);
		((Control)HScrollBar).Name = "HScrollBar";
		((Control)HScrollBar).Size = new Size(508, 35);
		((Control)HScrollBar).TabIndex = 16;
		((ScrollBar)HScrollBar).ValueChanged += HScrollBar_ValueChanged;
		((Control)panelCorner).Dock = (DockStyle)4;
		((Control)panelCorner).Location = new Point(508, 0);
		((Control)panelCorner).Margin = new Padding(2, 2, 2, 2);
		((Control)panelCorner).Name = "panelCorner";
		((Control)panelCorner).Size = new Size(32, 35);
		((Control)panelCorner).TabIndex = 16;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)panelHolder);
		((Control)this).Controls.Add((Control)(object)panelDialogs);
		((Control)this).Controls.Add((Control)(object)panelToolBar);
		((Control)this).Margin = new Padding(4, 4, 4, 4);
		((Control)this).Name = "StiDashboardViewerControl";
		((Control)this).Size = new Size(540, 660);
		((Control)panelHolder).ResumeLayout(false);
		((Control)panelHScrollBar).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
