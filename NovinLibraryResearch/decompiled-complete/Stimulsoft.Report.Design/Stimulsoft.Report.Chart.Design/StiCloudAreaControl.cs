using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Chart.Design;

[ToolboxItem(false)]
public class StiCloudAreaControl : UserControl
{
	private StiCloudChartForm chartWindow;

	private Type lastAreaType;

	private IContainer components;

	private StiCloudTabulator TabControl;

	private StiCloudTabulatorPage pageCommon;

	private StiCloudTabulatorPage pageXAxis_IStiXAxis;

	private StiCloudTabulatorPage pageXAxis_IStiXRadarAxis;

	private StiCloudTabulatorPage pageYAxis_IStiYAxis;

	private StiCloudTabulatorPage pageXTopAxis_IStiXAxis;

	private StiCloudTabulatorPage pageYRightAxis_IStiYAxis;

	private StiCloudTabulatorPage pageYAxis_IStiYRadarAxis;

	private StiCloudTabulatorPage pageGridLinesHor_IStiGridLinesHor;

	private StiCloudTabulatorPage pageGridLinesVert_IStiGridLinesVert;

	private StiCloudTabulatorPage pageGridLinesHorRight_IStiGridLinesHor;

	private StiCloudTabulatorPage pageGridLinesHor_IStiRadarGridLinesHor;

	private StiCloudTabulatorPage pageGridLinesVert_IStiRadarGridLinesVert;

	private StiCloudTabulatorPage pageInterlacingHor_IStiInterlacingHor;

	private StiCloudTabulatorPage pageInterlacingVert_IStiInterlacingVert;

	private StiCloudTabulatorPage pageInterlacingHorRadar_IStiInterlacingHor;

	private StiCloudTabulatorPage pageInterlacingVertRadar_IStiInterlacingVert;

	public StiCloudAreaControl(StiCloudChartForm chartWindow)
	{
		this.chartWindow = chartWindow;
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			Localize();
			BuildArea();
			TabControl.SelectedIndex = 0;
			TabControl_SelectedIndexChanged(null, null);
			DoScaling();
		}
	}

	private void Localize()
	{
		((Control)pageCommon).Text = StiLocalization.Get("Chart", "Common");
		((Control)pageGridLinesHor_IStiGridLinesHor).Text = StiLocalization.Get("PropertyMain", "GridLinesHor");
		((Control)pageGridLinesHorRight_IStiGridLinesHor).Text = StiLocalization.Get("PropertyMain", "GridLinesHorRight");
		((Control)pageGridLinesVert_IStiGridLinesVert).Text = StiLocalization.Get("PropertyMain", "GridLinesVert");
		((Control)pageInterlacingHor_IStiInterlacingHor).Text = StiLocalization.Get("PropertyMain", "InterlacingHor");
		((Control)pageInterlacingVert_IStiInterlacingVert).Text = StiLocalization.Get("PropertyMain", "InterlacingVert");
		((Control)pageXAxis_IStiXAxis).Text = StiLocalization.Get("PropertyMain", "XAxis");
		((Control)pageXTopAxis_IStiXAxis).Text = StiLocalization.Get("PropertyMain", "XTopAxis");
		((Control)pageYAxis_IStiYAxis).Text = StiLocalization.Get("PropertyMain", "YAxis");
		((Control)pageYRightAxis_IStiYAxis).Text = StiLocalization.Get("PropertyMain", "YRightAxis");
		((Control)pageGridLinesHor_IStiRadarGridLinesHor).Text = StiLocalization.Get("PropertyMain", "GridLinesHor");
		((Control)pageGridLinesVert_IStiRadarGridLinesVert).Text = StiLocalization.Get("PropertyMain", "GridLinesVert");
		((Control)pageInterlacingHorRadar_IStiInterlacingHor).Text = StiLocalization.Get("PropertyMain", "InterlacingHor");
		((Control)pageInterlacingVertRadar_IStiInterlacingVert).Text = StiLocalization.Get("PropertyMain", "InterlacingVert");
		((Control)pageXAxis_IStiXRadarAxis).Text = StiLocalization.Get("PropertyMain", "XAxis");
		((Control)pageYAxis_IStiYRadarAxis).Text = StiLocalization.Get("PropertyMain", "YAxis");
	}

	private void DoScaling()
	{
		DockPaddingEdges dockPadding = TabControl.DockPadding;
		int right = (TabControl.TitleWidth = StiScale.XXI(110.0));
		dockPadding.Right = right;
		pageCommon.Image = StiReportDesignImages.Tasks.ChartCommon(StiImageSize.Double);
		pageGridLinesHorRight_IStiGridLinesHor.Image = StiReportDesignImages.Tasks.ChartGridLinesHorizontalRight(StiImageSize.Double);
		pageGridLinesHor_IStiGridLinesHor.Image = StiReportDesignImages.Tasks.ChartGridLinesHorizontal(StiImageSize.Double);
		pageGridLinesVert_IStiGridLinesVert.Image = StiReportDesignImages.Tasks.ChartGridLinesVertical(StiImageSize.Double);
		pageGridLinesHor_IStiRadarGridLinesHor.Image = StiReportDesignImages.Tasks.ChartRadarGridLinesHorizontal(StiImageSize.Double);
		pageGridLinesVert_IStiRadarGridLinesVert.Image = StiReportDesignImages.Tasks.ChartRadarGridLinesVertical(StiImageSize.Double);
		pageInterlacingHorRadar_IStiInterlacingHor.Image = StiReportDesignImages.Tasks.ChartRadarInterlacingHorizontal(StiImageSize.Double);
		pageInterlacingVertRadar_IStiInterlacingVert.Image = StiReportDesignImages.Tasks.ChartRadarInterlacingVertical(StiImageSize.Double);
		pageInterlacingHor_IStiInterlacingHor.Image = StiReportDesignImages.Tasks.ChartInterlacingHorizontal(StiImageSize.Double);
		pageInterlacingVert_IStiInterlacingVert.Image = StiReportDesignImages.Tasks.ChartInterlacingVertical(StiImageSize.Double);
		pageXAxis_IStiXRadarAxis.Image = StiReportDesignImages.Tasks.ChartRadarXAxis(StiImageSize.Double);
		pageYAxis_IStiYRadarAxis.Image = StiReportDesignImages.Tasks.ChartRadarYAxis(StiImageSize.Double);
		pageXAxis_IStiXAxis.Image = StiReportDesignImages.Tasks.ChartXAxis(StiImageSize.Double);
		pageXTopAxis_IStiXAxis.Image = StiReportDesignImages.Tasks.ChartXTopAxis(StiImageSize.Double);
		pageYAxis_IStiYAxis.Image = StiReportDesignImages.Tasks.ChartYAxis(StiImageSize.Double);
		pageYRightAxis_IStiYAxis.Image = StiReportDesignImages.Tasks.ChartYRightAxis(StiImageSize.Double);
	}

	internal void BuildArea()
	{
		if (lastAreaType == chartWindow.Chart.Area.GetType())
		{
			return;
		}
		lastAreaType = chartWindow.Chart.Area.GetType();
		TabControl.SelectedIndex = 0;
		pageGridLinesHor_IStiGridLinesHor.Invisible = true;
		pageGridLinesHorRight_IStiGridLinesHor.Invisible = true;
		pageGridLinesVert_IStiGridLinesVert.Invisible = true;
		pageInterlacingHor_IStiInterlacingHor.Invisible = true;
		pageInterlacingVert_IStiInterlacingVert.Invisible = true;
		pageInterlacingHorRadar_IStiInterlacingHor.Invisible = true;
		pageInterlacingVertRadar_IStiInterlacingVert.Invisible = true;
		pageXAxis_IStiXAxis.Invisible = true;
		pageXTopAxis_IStiXAxis.Invisible = true;
		pageYAxis_IStiYAxis.Invisible = true;
		pageYRightAxis_IStiYAxis.Invisible = true;
		pageGridLinesHor_IStiRadarGridLinesHor.Invisible = true;
		pageGridLinesVert_IStiRadarGridLinesVert.Invisible = true;
		pageXAxis_IStiXRadarAxis.Invisible = true;
		pageYAxis_IStiYRadarAxis.Invisible = true;
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		bool flag6 = false;
		bool flag7 = false;
		bool flag8 = false;
		bool flag9 = false;
		bool flag10 = false;
		bool flag11 = false;
		bool flag12 = false;
		bool flag13 = false;
		bool flag14 = false;
		bool flag15 = false;
		IStiArea area = chartWindow.Chart.Area;
		if (area is StiClusteredColumnArea)
		{
			flag = true;
			flag2 = true;
			flag3 = true;
			flag4 = true;
			flag5 = true;
			flag8 = true;
			flag9 = true;
			flag10 = true;
			flag11 = true;
		}
		if (area is StiWaterfallArea)
		{
			flag = true;
			flag2 = true;
			flag3 = true;
			flag4 = true;
			flag5 = true;
			flag8 = true;
			flag10 = true;
		}
		if (area is StiRadarAreaArea)
		{
			flag12 = true;
			flag13 = true;
			flag6 = true;
			flag7 = true;
			flag14 = true;
			flag15 = true;
		}
		if (area is StiRadarLineArea)
		{
			flag12 = true;
			flag13 = true;
			flag6 = true;
			flag7 = true;
			flag14 = true;
			flag15 = true;
		}
		if (area is StiRadarPointArea)
		{
			flag12 = true;
			flag13 = true;
			flag6 = true;
			flag7 = true;
			flag14 = true;
			flag15 = true;
		}
		if (area is StiStackedColumnArea)
		{
			flag = true;
			flag2 = true;
			flag3 = true;
			flag4 = true;
			flag5 = true;
			flag8 = true;
			flag9 = true;
			flag10 = true;
			flag11 = true;
		}
		if (flag)
		{
			pageGridLinesHor_IStiGridLinesHor.Invisible = false;
			((Control)pageGridLinesHor_IStiGridLinesHor).Tag = ((IStiAxisArea)chartWindow.Chart.Area).GridLinesHor;
		}
		if (flag2)
		{
			pageGridLinesHorRight_IStiGridLinesHor.Invisible = false;
			((Control)pageGridLinesHorRight_IStiGridLinesHor).Tag = ((IStiAxisArea)chartWindow.Chart.Area).GridLinesHorRight;
		}
		if (flag3)
		{
			pageGridLinesVert_IStiGridLinesVert.Invisible = false;
			((Control)pageGridLinesVert_IStiGridLinesVert).Tag = ((IStiAxisArea)chartWindow.Chart.Area).GridLinesVert;
		}
		if (flag4)
		{
			pageInterlacingHor_IStiInterlacingHor.Invisible = false;
			if (chartWindow.Chart.Area is IStiAxisArea)
			{
				((Control)pageInterlacingHor_IStiInterlacingHor).Tag = ((IStiAxisArea)chartWindow.Chart.Area).InterlacingHor;
			}
			else
			{
				((Control)pageInterlacingHor_IStiInterlacingHor).Tag = ((IStiRadarArea)chartWindow.Chart.Area).InterlacingHor;
			}
		}
		if (flag5)
		{
			pageInterlacingVert_IStiInterlacingVert.Invisible = false;
			if (chartWindow.Chart.Area is IStiAxisArea)
			{
				((Control)pageInterlacingVert_IStiInterlacingVert).Tag = ((IStiAxisArea)chartWindow.Chart.Area).InterlacingVert;
			}
			else
			{
				((Control)pageInterlacingVert_IStiInterlacingVert).Tag = ((IStiRadarArea)chartWindow.Chart.Area).InterlacingVert;
			}
		}
		if (flag6)
		{
			pageInterlacingHorRadar_IStiInterlacingHor.Invisible = false;
			((Control)pageInterlacingHorRadar_IStiInterlacingHor).Tag = ((IStiRadarArea)chartWindow.Chart.Area).InterlacingHor;
		}
		if (flag7)
		{
			pageInterlacingVertRadar_IStiInterlacingVert.Invisible = false;
			((Control)pageInterlacingVertRadar_IStiInterlacingVert).Tag = ((IStiRadarArea)chartWindow.Chart.Area).InterlacingVert;
		}
		if (flag8)
		{
			pageXAxis_IStiXAxis.Invisible = false;
			((Control)pageXAxis_IStiXAxis).Tag = ((IStiAxisArea)chartWindow.Chart.Area).XAxis;
		}
		if (flag9)
		{
			pageXTopAxis_IStiXAxis.Invisible = false;
			((Control)pageXTopAxis_IStiXAxis).Tag = ((IStiAxisArea)chartWindow.Chart.Area).XTopAxis;
		}
		if (flag10)
		{
			pageYAxis_IStiYAxis.Invisible = false;
			((Control)pageYAxis_IStiYAxis).Tag = ((IStiAxisArea)chartWindow.Chart.Area).YAxis;
		}
		if (flag11)
		{
			pageYRightAxis_IStiYAxis.Invisible = false;
			((Control)pageYRightAxis_IStiYAxis).Tag = ((IStiAxisArea)chartWindow.Chart.Area).YRightAxis;
		}
		if (flag12)
		{
			pageGridLinesHor_IStiRadarGridLinesHor.Invisible = false;
			((Control)pageGridLinesHor_IStiRadarGridLinesHor).Tag = ((IStiRadarArea)chartWindow.Chart.Area).GridLinesHor;
		}
		if (flag13)
		{
			pageGridLinesVert_IStiRadarGridLinesVert.Invisible = false;
			((Control)pageGridLinesVert_IStiRadarGridLinesVert).Tag = ((IStiRadarArea)chartWindow.Chart.Area).GridLinesVert;
		}
		if (flag14)
		{
			pageXAxis_IStiXRadarAxis.Invisible = false;
			((Control)pageXAxis_IStiXRadarAxis).Tag = ((IStiRadarArea)chartWindow.Chart.Area).XAxis;
		}
		if (flag15)
		{
			pageYAxis_IStiYRadarAxis.Invisible = false;
			((Control)pageYAxis_IStiYRadarAxis).Tag = ((IStiRadarArea)chartWindow.Chart.Area).YAxis;
		}
	}

	internal void CheckPropertyGridValue()
	{
		if (TabControl.SelectedIndex == 0)
		{
			((PropertyGrid)chartWindow.PropertyGrid).SelectedObject = chartWindow.Chart.Area;
		}
		else
		{
			((PropertyGrid)chartWindow.PropertyGrid).SelectedObject = ((Control)TabControl.SelectedTab).Tag;
		}
	}

	private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
	{
		CheckPropertyGridValue();
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
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_034c: Expected O, but got Unknown
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_040b: Expected O, but got Unknown
		//IL_0433: Unknown result type (might be due to invalid IL or missing references)
		//IL_0457: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ca: Expected O, but got Unknown
		//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0516: Unknown result type (might be due to invalid IL or missing references)
		//IL_057f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0589: Expected O, but got Unknown
		//IL_05ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_063b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0645: Expected O, but got Unknown
		//IL_066a: Unknown result type (might be due to invalid IL or missing references)
		//IL_068e: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0701: Expected O, but got Unknown
		//IL_0729: Unknown result type (might be due to invalid IL or missing references)
		//IL_074d: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c0: Expected O, but got Unknown
		//IL_07e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_080c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0875: Unknown result type (might be due to invalid IL or missing references)
		//IL_087f: Expected O, but got Unknown
		//IL_08a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0934: Unknown result type (might be due to invalid IL or missing references)
		//IL_093e: Expected O, but got Unknown
		//IL_0966: Unknown result type (might be due to invalid IL or missing references)
		//IL_098a: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fd: Expected O, but got Unknown
		//IL_0a25: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abc: Expected O, but got Unknown
		//IL_0ae4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b71: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b7b: Expected O, but got Unknown
		//IL_0ba3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3a: Expected O, but got Unknown
		//IL_0c62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf9: Expected O, but got Unknown
		//IL_0d21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d45: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db8: Expected O, but got Unknown
		//IL_0de0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e04: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e77: Expected O, but got Unknown
		//IL_0e9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f41: Unknown result type (might be due to invalid IL or missing references)
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiCloudAreaControl));
		TabControl = new StiCloudTabulator();
		pageCommon = new StiCloudTabulatorPage();
		pageXAxis_IStiXAxis = new StiCloudTabulatorPage();
		pageXAxis_IStiXRadarAxis = new StiCloudTabulatorPage();
		pageYAxis_IStiYAxis = new StiCloudTabulatorPage();
		pageXTopAxis_IStiXAxis = new StiCloudTabulatorPage();
		pageYRightAxis_IStiYAxis = new StiCloudTabulatorPage();
		pageYAxis_IStiYRadarAxis = new StiCloudTabulatorPage();
		pageGridLinesHor_IStiGridLinesHor = new StiCloudTabulatorPage();
		pageGridLinesVert_IStiGridLinesVert = new StiCloudTabulatorPage();
		pageGridLinesHorRight_IStiGridLinesHor = new StiCloudTabulatorPage();
		pageGridLinesHor_IStiRadarGridLinesHor = new StiCloudTabulatorPage();
		pageGridLinesVert_IStiRadarGridLinesVert = new StiCloudTabulatorPage();
		pageInterlacingHor_IStiInterlacingHor = new StiCloudTabulatorPage();
		pageInterlacingVert_IStiInterlacingVert = new StiCloudTabulatorPage();
		pageInterlacingHorRadar_IStiInterlacingHor = new StiCloudTabulatorPage();
		pageInterlacingVertRadar_IStiInterlacingVert = new StiCloudTabulatorPage();
		((Control)TabControl).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)TabControl).Controls.Add((Control)(object)pageCommon);
		((Control)TabControl).Controls.Add((Control)(object)pageXAxis_IStiXAxis);
		((Control)TabControl).Controls.Add((Control)(object)pageXAxis_IStiXRadarAxis);
		((Control)TabControl).Controls.Add((Control)(object)pageYAxis_IStiYAxis);
		((Control)TabControl).Controls.Add((Control)(object)pageXTopAxis_IStiXAxis);
		((Control)TabControl).Controls.Add((Control)(object)pageYRightAxis_IStiYAxis);
		((Control)TabControl).Controls.Add((Control)(object)pageYAxis_IStiYRadarAxis);
		((Control)TabControl).Controls.Add((Control)(object)pageGridLinesHor_IStiGridLinesHor);
		((Control)TabControl).Controls.Add((Control)(object)pageGridLinesVert_IStiGridLinesVert);
		((Control)TabControl).Controls.Add((Control)(object)pageGridLinesHorRight_IStiGridLinesHor);
		((Control)TabControl).Controls.Add((Control)(object)pageGridLinesHor_IStiRadarGridLinesHor);
		((Control)TabControl).Controls.Add((Control)(object)pageGridLinesVert_IStiRadarGridLinesVert);
		((Control)TabControl).Controls.Add((Control)(object)pageInterlacingHor_IStiInterlacingHor);
		((Control)TabControl).Controls.Add((Control)(object)pageInterlacingVert_IStiInterlacingVert);
		((Control)TabControl).Controls.Add((Control)(object)pageInterlacingHorRadar_IStiInterlacingHor);
		((Control)TabControl).Controls.Add((Control)(object)pageInterlacingVertRadar_IStiInterlacingVert);
		((Control)TabControl).Dock = (DockStyle)5;
		((Control)TabControl).Location = new Point(0, 0);
		((Control)TabControl).Margin = new Padding(6, 6, 6, 6);
		((Control)TabControl).Name = "TabControl";
		((Control)TabControl).Padding = new Padding(0, 0, 178, 0);
		TabControl.ScrollHeight = 2816;
		TabControl.ScrollWidth = 100;
		((Control)TabControl).Size = new Size(1200, 2692);
		((Control)TabControl).TabIndex = 1;
		TabControl.TitleBackColor = SystemColors.Control;
		TabControl.TitleInLine = false;
		TabControl.TitlePosition = StiTabTitlePosition.RightHorizontal;
		TabControl.TitleWidth = 110;
		TabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
		pageCommon.Dock = (DockStyle)5;
		pageCommon.Image = (Image)componentResourceManager.GetObject("pageCommon.Image");
		pageCommon.InvisibleVisible = true;
		((Control)pageCommon).Location = new Point(0, 0);
		((Control)pageCommon).Margin = new Padding(6, 6, 6, 6);
		((Control)pageCommon).Name = "pageCommon";
		((Control)pageCommon).Padding = new Padding(0, 0, 2, 0);
		((Control)pageCommon).Size = new Size(1022, 2692);
		((Control)pageCommon).TabIndex = 0;
		((Control)pageCommon).Text = "Common";
		((Control)pageCommon).Visible = false;
		pageXAxis_IStiXAxis.Dock = (DockStyle)5;
		pageXAxis_IStiXAxis.Image = (Image)componentResourceManager.GetObject("pageXAxis_IStiXAxis.Image");
		pageXAxis_IStiXAxis.InvisibleVisible = false;
		((Control)pageXAxis_IStiXAxis).Location = new Point(0, 0);
		((Control)pageXAxis_IStiXAxis).Margin = new Padding(6, 6, 6, 6);
		((Control)pageXAxis_IStiXAxis).Name = "pageXAxis_IStiXAxis";
		((Control)pageXAxis_IStiXAxis).Padding = new Padding(0, 0, 2, 0);
		((Control)pageXAxis_IStiXAxis).Size = new Size(1022, 2692);
		((Control)pageXAxis_IStiXAxis).TabIndex = 1;
		((Control)pageXAxis_IStiXAxis).Text = "XAxis_IStiXAxis";
		((Control)pageXAxis_IStiXAxis).Visible = false;
		pageXAxis_IStiXRadarAxis.Dock = (DockStyle)5;
		pageXAxis_IStiXRadarAxis.Image = (Image)componentResourceManager.GetObject("pageXAxis_IStiXRadarAxis.Image");
		pageXAxis_IStiXRadarAxis.InvisibleVisible = false;
		((Control)pageXAxis_IStiXRadarAxis).Location = new Point(0, 0);
		((Control)pageXAxis_IStiXRadarAxis).Margin = new Padding(6, 6, 6, 6);
		((Control)pageXAxis_IStiXRadarAxis).Name = "pageXAxis_IStiXRadarAxis";
		((Control)pageXAxis_IStiXRadarAxis).Padding = new Padding(8, 8, 8, 8);
		((Control)pageXAxis_IStiXRadarAxis).Size = new Size(1022, 2692);
		((Control)pageXAxis_IStiXRadarAxis).TabIndex = 2;
		((Control)pageXAxis_IStiXRadarAxis).Text = "XAxis_IStiXRadarAxis";
		((Control)pageXAxis_IStiXRadarAxis).Visible = false;
		pageYAxis_IStiYAxis.Dock = (DockStyle)5;
		pageYAxis_IStiYAxis.Image = (Image)componentResourceManager.GetObject("pageYAxis_IStiYAxis.Image");
		pageYAxis_IStiYAxis.InvisibleVisible = false;
		((Control)pageYAxis_IStiYAxis).Location = new Point(0, 0);
		((Control)pageYAxis_IStiYAxis).Margin = new Padding(0);
		((Control)pageYAxis_IStiYAxis).Name = "pageYAxis_IStiYAxis";
		((Control)pageYAxis_IStiYAxis).Padding = new Padding(0, 0, 2, 0);
		((Control)pageYAxis_IStiYAxis).Size = new Size(1022, 2692);
		((Control)pageYAxis_IStiYAxis).TabIndex = 3;
		((Control)pageYAxis_IStiYAxis).Text = "YAxis_IStiYAxis";
		((Control)pageYAxis_IStiYAxis).Visible = false;
		pageXTopAxis_IStiXAxis.Dock = (DockStyle)5;
		pageXTopAxis_IStiXAxis.Image = (Image)componentResourceManager.GetObject("pageXTopAxis_IStiXAxis.Image");
		pageXTopAxis_IStiXAxis.InvisibleVisible = false;
		((Control)pageXTopAxis_IStiXAxis).Location = new Point(0, 0);
		((Control)pageXTopAxis_IStiXAxis).Margin = new Padding(0);
		((Control)pageXTopAxis_IStiXAxis).Name = "pageXTopAxis_IStiXAxis";
		((Control)pageXTopAxis_IStiXAxis).Padding = new Padding(0, 0, 2, 0);
		((Control)pageXTopAxis_IStiXAxis).Size = new Size(1022, 2692);
		((Control)pageXTopAxis_IStiXAxis).TabIndex = 4;
		((Control)pageXTopAxis_IStiXAxis).Text = "XTopAxis_IStiXAxis";
		((Control)pageXTopAxis_IStiXAxis).Visible = false;
		pageYRightAxis_IStiYAxis.Dock = (DockStyle)5;
		pageYRightAxis_IStiYAxis.Image = (Image)componentResourceManager.GetObject("pageYRightAxis_IStiYAxis.Image");
		pageYRightAxis_IStiYAxis.InvisibleVisible = false;
		((Control)pageYRightAxis_IStiYAxis).Location = new Point(0, 0);
		((Control)pageYRightAxis_IStiYAxis).Margin = new Padding(6, 6, 6, 6);
		((Control)pageYRightAxis_IStiYAxis).Name = "pageYRightAxis_IStiYAxis";
		((Control)pageYRightAxis_IStiYAxis).Padding = new Padding(0, 8, 2, 8);
		((Control)pageYRightAxis_IStiYAxis).Size = new Size(1022, 2692);
		((Control)pageYRightAxis_IStiYAxis).TabIndex = 3;
		((Control)pageYRightAxis_IStiYAxis).Text = "YRightAxis_IStiYAxis";
		((Control)pageYRightAxis_IStiYAxis).Visible = false;
		pageYAxis_IStiYRadarAxis.Dock = (DockStyle)5;
		pageYAxis_IStiYRadarAxis.Image = (Image)componentResourceManager.GetObject("pageYAxis_IStiYRadarAxis.Image");
		pageYAxis_IStiYRadarAxis.InvisibleVisible = false;
		((Control)pageYAxis_IStiYRadarAxis).Location = new Point(0, 0);
		((Control)pageYAxis_IStiYRadarAxis).Margin = new Padding(6, 6, 6, 6);
		((Control)pageYAxis_IStiYRadarAxis).Name = "pageYAxis_IStiYRadarAxis";
		((Control)pageYAxis_IStiYRadarAxis).Padding = new Padding(0, 8, 2, 8);
		((Control)pageYAxis_IStiYRadarAxis).Size = new Size(1022, 2692);
		((Control)pageYAxis_IStiYRadarAxis).TabIndex = 4;
		((Control)pageYAxis_IStiYRadarAxis).Text = "YAxis_IStiYRadarAxis";
		((Control)pageYAxis_IStiYRadarAxis).Visible = false;
		pageGridLinesHor_IStiGridLinesHor.Dock = (DockStyle)5;
		pageGridLinesHor_IStiGridLinesHor.Image = (Image)componentResourceManager.GetObject("pageGridLinesHor_IStiGridLinesHor.Image");
		pageGridLinesHor_IStiGridLinesHor.InvisibleVisible = false;
		((Control)pageGridLinesHor_IStiGridLinesHor).Location = new Point(0, 0);
		((Control)pageGridLinesHor_IStiGridLinesHor).Margin = new Padding(6, 6, 6, 6);
		((Control)pageGridLinesHor_IStiGridLinesHor).Name = "pageGridLinesHor_IStiGridLinesHor";
		((Control)pageGridLinesHor_IStiGridLinesHor).Padding = new Padding(0, 8, 2, 8);
		((Control)pageGridLinesHor_IStiGridLinesHor).Size = new Size(1022, 2692);
		((Control)pageGridLinesHor_IStiGridLinesHor).TabIndex = 4;
		((Control)pageGridLinesHor_IStiGridLinesHor).Text = "GridLinesHor_IStiGridLinesHor";
		((Control)pageGridLinesHor_IStiGridLinesHor).Visible = false;
		pageGridLinesVert_IStiGridLinesVert.Dock = (DockStyle)5;
		pageGridLinesVert_IStiGridLinesVert.Image = (Image)componentResourceManager.GetObject("pageGridLinesVert_IStiGridLinesVert.Image");
		pageGridLinesVert_IStiGridLinesVert.InvisibleVisible = false;
		((Control)pageGridLinesVert_IStiGridLinesVert).Location = new Point(0, 0);
		((Control)pageGridLinesVert_IStiGridLinesVert).Margin = new Padding(6, 6, 6, 6);
		((Control)pageGridLinesVert_IStiGridLinesVert).Name = "pageGridLinesVert_IStiGridLinesVert";
		((Control)pageGridLinesVert_IStiGridLinesVert).Padding = new Padding(0, 8, 2, 8);
		((Control)pageGridLinesVert_IStiGridLinesVert).Size = new Size(1022, 2692);
		((Control)pageGridLinesVert_IStiGridLinesVert).TabIndex = 4;
		((Control)pageGridLinesVert_IStiGridLinesVert).Text = "GridLinesVert_IStiGridLinesVert";
		((Control)pageGridLinesVert_IStiGridLinesVert).Visible = false;
		pageGridLinesHorRight_IStiGridLinesHor.Dock = (DockStyle)5;
		pageGridLinesHorRight_IStiGridLinesHor.Image = (Image)componentResourceManager.GetObject("pageGridLinesHorRight_IStiGridLinesHor.Image");
		pageGridLinesHorRight_IStiGridLinesHor.InvisibleVisible = false;
		((Control)pageGridLinesHorRight_IStiGridLinesHor).Location = new Point(0, 0);
		((Control)pageGridLinesHorRight_IStiGridLinesHor).Margin = new Padding(6, 6, 6, 6);
		((Control)pageGridLinesHorRight_IStiGridLinesHor).Name = "pageGridLinesHorRight_IStiGridLinesHor";
		((Control)pageGridLinesHorRight_IStiGridLinesHor).Padding = new Padding(0, 8, 2, 8);
		((Control)pageGridLinesHorRight_IStiGridLinesHor).Size = new Size(1022, 2692);
		((Control)pageGridLinesHorRight_IStiGridLinesHor).TabIndex = 4;
		((Control)pageGridLinesHorRight_IStiGridLinesHor).Text = "GridLinesHorRight_IStiGridLinesHor";
		((Control)pageGridLinesHorRight_IStiGridLinesHor).Visible = false;
		pageGridLinesHor_IStiRadarGridLinesHor.Dock = (DockStyle)5;
		pageGridLinesHor_IStiRadarGridLinesHor.Image = (Image)componentResourceManager.GetObject("pageGridLinesHor_IStiRadarGridLinesHor.Image");
		pageGridLinesHor_IStiRadarGridLinesHor.InvisibleVisible = false;
		((Control)pageGridLinesHor_IStiRadarGridLinesHor).Location = new Point(0, 0);
		((Control)pageGridLinesHor_IStiRadarGridLinesHor).Margin = new Padding(6, 6, 6, 6);
		((Control)pageGridLinesHor_IStiRadarGridLinesHor).Name = "pageGridLinesHor_IStiRadarGridLinesHor";
		((Control)pageGridLinesHor_IStiRadarGridLinesHor).Padding = new Padding(0, 8, 2, 8);
		((Control)pageGridLinesHor_IStiRadarGridLinesHor).Size = new Size(1022, 2692);
		((Control)pageGridLinesHor_IStiRadarGridLinesHor).TabIndex = 4;
		((Control)pageGridLinesHor_IStiRadarGridLinesHor).Text = "GridLinesHor_IStiRadarGridLinesHor";
		((Control)pageGridLinesHor_IStiRadarGridLinesHor).Visible = false;
		pageGridLinesVert_IStiRadarGridLinesVert.Dock = (DockStyle)5;
		pageGridLinesVert_IStiRadarGridLinesVert.Image = (Image)componentResourceManager.GetObject("pageGridLinesVert_IStiRadarGridLinesVert.Image");
		pageGridLinesVert_IStiRadarGridLinesVert.InvisibleVisible = false;
		((Control)pageGridLinesVert_IStiRadarGridLinesVert).Location = new Point(0, 0);
		((Control)pageGridLinesVert_IStiRadarGridLinesVert).Margin = new Padding(6, 6, 6, 6);
		((Control)pageGridLinesVert_IStiRadarGridLinesVert).Name = "pageGridLinesVert_IStiRadarGridLinesVert";
		((Control)pageGridLinesVert_IStiRadarGridLinesVert).Padding = new Padding(0, 8, 2, 8);
		((Control)pageGridLinesVert_IStiRadarGridLinesVert).Size = new Size(1022, 2692);
		((Control)pageGridLinesVert_IStiRadarGridLinesVert).TabIndex = 4;
		((Control)pageGridLinesVert_IStiRadarGridLinesVert).Text = "GridLinesVert_IStiRadarGridLinesVert";
		((Control)pageGridLinesVert_IStiRadarGridLinesVert).Visible = false;
		pageInterlacingHor_IStiInterlacingHor.Dock = (DockStyle)5;
		pageInterlacingHor_IStiInterlacingHor.Image = (Image)componentResourceManager.GetObject("pageInterlacingHor_IStiInterlacingHor.Image");
		pageInterlacingHor_IStiInterlacingHor.InvisibleVisible = false;
		((Control)pageInterlacingHor_IStiInterlacingHor).Location = new Point(0, 0);
		((Control)pageInterlacingHor_IStiInterlacingHor).Margin = new Padding(6, 6, 6, 6);
		((Control)pageInterlacingHor_IStiInterlacingHor).Name = "pageInterlacingHor_IStiInterlacingHor";
		((Control)pageInterlacingHor_IStiInterlacingHor).Padding = new Padding(0, 8, 2, 8);
		((Control)pageInterlacingHor_IStiInterlacingHor).Size = new Size(1022, 2692);
		((Control)pageInterlacingHor_IStiInterlacingHor).TabIndex = 4;
		((Control)pageInterlacingHor_IStiInterlacingHor).Text = "InterlacingHor_IStiInterlacingHor";
		((Control)pageInterlacingHor_IStiInterlacingHor).Visible = false;
		pageInterlacingVert_IStiInterlacingVert.Dock = (DockStyle)5;
		pageInterlacingVert_IStiInterlacingVert.Image = (Image)componentResourceManager.GetObject("pageInterlacingVert_IStiInterlacingVert.Image");
		pageInterlacingVert_IStiInterlacingVert.InvisibleVisible = false;
		((Control)pageInterlacingVert_IStiInterlacingVert).Location = new Point(0, 0);
		((Control)pageInterlacingVert_IStiInterlacingVert).Margin = new Padding(6, 6, 6, 6);
		((Control)pageInterlacingVert_IStiInterlacingVert).Name = "pageInterlacingVert_IStiInterlacingVert";
		((Control)pageInterlacingVert_IStiInterlacingVert).Padding = new Padding(0, 8, 2, 8);
		((Control)pageInterlacingVert_IStiInterlacingVert).Size = new Size(1022, 2692);
		((Control)pageInterlacingVert_IStiInterlacingVert).TabIndex = 4;
		((Control)pageInterlacingVert_IStiInterlacingVert).Text = "InterlacingVert_IStiInterlacingVert";
		((Control)pageInterlacingVert_IStiInterlacingVert).Visible = false;
		pageInterlacingHorRadar_IStiInterlacingHor.Dock = (DockStyle)5;
		pageInterlacingHorRadar_IStiInterlacingHor.Image = (Image)componentResourceManager.GetObject("pageInterlacingHorRadar_IStiInterlacingHor.Image");
		pageInterlacingHorRadar_IStiInterlacingHor.InvisibleVisible = false;
		((Control)pageInterlacingHorRadar_IStiInterlacingHor).Location = new Point(0, 0);
		((Control)pageInterlacingHorRadar_IStiInterlacingHor).Margin = new Padding(6, 6, 6, 6);
		((Control)pageInterlacingHorRadar_IStiInterlacingHor).Name = "pageInterlacingHorRadar_IStiInterlacingHor";
		((Control)pageInterlacingHorRadar_IStiInterlacingHor).Padding = new Padding(0, 8, 2, 8);
		((Control)pageInterlacingHorRadar_IStiInterlacingHor).Size = new Size(1022, 2692);
		((Control)pageInterlacingHorRadar_IStiInterlacingHor).TabIndex = 4;
		((Control)pageInterlacingHorRadar_IStiInterlacingHor).Text = "InterlacingHor_IStiInterlacingHor";
		((Control)pageInterlacingHorRadar_IStiInterlacingHor).Visible = false;
		pageInterlacingVertRadar_IStiInterlacingVert.Dock = (DockStyle)5;
		pageInterlacingVertRadar_IStiInterlacingVert.Image = (Image)componentResourceManager.GetObject("pageInterlacingVertRadar_IStiInterlacingVert.Image");
		pageInterlacingVertRadar_IStiInterlacingVert.InvisibleVisible = false;
		((Control)pageInterlacingVertRadar_IStiInterlacingVert).Location = new Point(0, 0);
		((Control)pageInterlacingVertRadar_IStiInterlacingVert).Margin = new Padding(6, 6, 6, 6);
		((Control)pageInterlacingVertRadar_IStiInterlacingVert).Name = "pageInterlacingVertRadar_IStiInterlacingVert";
		((Control)pageInterlacingVertRadar_IStiInterlacingVert).Padding = new Padding(0, 8, 2, 8);
		((Control)pageInterlacingVertRadar_IStiInterlacingVert).Size = new Size(1022, 2692);
		((Control)pageInterlacingVertRadar_IStiInterlacingVert).TabIndex = 4;
		((Control)pageInterlacingVertRadar_IStiInterlacingVert).Text = "InterlacingVert_IStiInterlacingVert";
		((Control)pageInterlacingVertRadar_IStiInterlacingVert).Visible = false;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)TabControl);
		((Control)this).Margin = new Padding(6, 6, 6, 6);
		((Control)this).Name = "StiCloudAreaControl";
		((Control)this).Size = new Size(1200, 2692);
		((Control)TabControl).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
