using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design.Images;

namespace Stimulsoft.Report.Chart.Design;

[ToolboxItem(false)]
public class StiCloudChartControl : UserControl
{
	private StiCloudChartForm chartWindow;

	private bool isConstantLinesBuild;

	private bool isStripsBuild;

	private bool LockCheckSamplePosition;

	private IContainer components;

	private StiCloudTabulator TabControl;

	private StiCloudTabulatorPage pageCommon;

	private StiCloudTabulatorPage pageLegend;

	private StiCloudTabulatorPage pageTitle;

	private StiCloudTabulatorPage pageConstantLines;

	private StiCloudTabulatorPage pageStrips;

	private StiCloudTabulatorPage pageTable;

	public StiToolBar toolBarConstantLines;

	private StiToolButton buttonRemoveConstantLines;

	private StiToolButton buttonAddConstantLines;

	private StiToolButton buttonMoveDownConstantLines;

	private StiToolButton buttonMoveUpConstantLines;

	private StiToolButton separatorConstantLines;

	private StiTreeView tvChartConstantLines;

	private StiTreeView tvChartStrips;

	public StiToolBar toolBarStrips;

	private StiToolButton buttonMoveDownStrips;

	private StiToolButton buttonMoveUpStrips;

	private StiToolButton separatorStrips;

	private StiToolButton buttonRemoveStrips;

	private StiToolButton buttonAddStrips;

	private Panel tvChartConstantLinesPanel;

	private Panel panel1;

	public StiCloudChartControl(StiCloudChartForm chartWindow)
	{
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			this.chartWindow = chartWindow;
			Localize();
			DoScaling();
			TabControl.SelectedIndex = 0;
			TabControl_SelectedIndexChanged(null, null);
		}
	}

	private void Localize()
	{
		((Control)pageCommon).Text = StiLocalization.Get("Chart", "Common");
		((Control)pageLegend).Text = StiLocalization.Get("Chart", "Legend");
		((Control)pageConstantLines).Text = StiLocalization.Get("PropertyMain", "ConstantLines");
		((Control)pageStrips).Text = StiLocalization.Get("PropertyMain", "Strips");
		((Control)pageTitle).Text = StiLocalization.Get("PropertyMain", "Title");
		((Control)pageTable).Text = StiLocalization.Get("PropertyMain", "Table");
		((Control)buttonAddConstantLines).Text = StiLocalization.Get("Chart", "AddConstantLine");
		((Control)buttonAddStrips).Text = StiLocalization.Get("Chart", "AddStrip");
	}

	private void DoScaling()
	{
		DockPaddingEdges dockPadding = TabControl.DockPadding;
		int right = (TabControl.TitleWidth = StiScale.XXI(110.0));
		dockPadding.Right = right;
		pageCommon.Image = StiReportDesignImages.Tasks.ChartCommon(StiImageSize.Double);
		pageConstantLines.Image = StiReportDesignImages.Tasks.ChartConstantLines(StiImageSize.Double);
		pageLegend.Image = StiReportDesignImages.Tasks.ChartLegend(StiImageSize.Double);
		pageStrips.Image = StiReportDesignImages.Tasks.ChartStrips(StiImageSize.Double);
		pageTable.Image = StiReportDesignImages.Tasks.ChartTable(StiImageSize.Double);
		pageTitle.Image = StiReportDesignImages.Tasks.ChartTitle(StiImageSize.Double);
		((ButtonBase)buttonAddConstantLines).Image = StiReportDesignImages.Actions.NewConstantLine();
		((ButtonBase)buttonAddStrips).Image = StiReportDesignImages.Actions.NewSeries();
		((ButtonBase)buttonRemoveConstantLines).Image = StiReportDesignImages.Actions.Remove();
		((ButtonBase)buttonRemoveStrips).Image = StiReportDesignImages.Actions.Remove();
		((ButtonBase)buttonMoveUpConstantLines).Image = StiReportDesignImages.Actions.MoveUp();
		((ButtonBase)buttonMoveDownConstantLines).Image = StiReportDesignImages.Actions.MoveDown();
		((ButtonBase)buttonMoveUpStrips).Image = StiReportDesignImages.Actions.MoveUp();
		((ButtonBase)buttonMoveDownStrips).Image = StiReportDesignImages.Actions.MoveDown();
	}

	internal void CheckSamplePosition()
	{
		if (!LockCheckSamplePosition)
		{
			LockCheckSamplePosition = true;
			if (TabControl.SelectedTab == pageConstantLines || TabControl.SelectedTab == pageStrips)
			{
				chartWindow.UpdateSamplePosition(1, 1, 1, 1, showRightSeparator: true, showSample: true);
			}
			else
			{
				chartWindow.UpdateSamplePosition(0, 0, 2, 2, showRightSeparator: true, showSample: true);
			}
			TabControl_SelectedIndexChanged(null, null);
			LockCheckSamplePosition = false;
		}
	}

	internal void CheckStateTabItem()
	{
		StiSeriesCollection series = chartWindow.Chart.Series;
		if (series.Count != 0)
		{
			if (series[0] is IStiPictorialSeries || series[0] is IStiTreemapSeries || series[0] is IStiSunburstSeries)
			{
				pageTable.Invisible = true;
				pageStrips.Invisible = true;
				pageConstantLines.Invisible = true;
			}
			else if (series[0] is IStiBoxAndWhiskerSeries)
			{
				pageTable.Invisible = true;
			}
			else
			{
				pageTable.Invisible = false;
				pageStrips.Invisible = false;
				pageConstantLines.Invisible = false;
			}
		}
	}

	internal void TabControl_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (TabControl.SelectedTab == pageCommon)
		{
			((PropertyGrid)chartWindow.PropertyGrid).SelectedObject = new StiChartCommon(chartWindow.Chart);
		}
		else if (TabControl.SelectedTab == pageLegend)
		{
			((PropertyGrid)chartWindow.PropertyGrid).SelectedObject = chartWindow.Chart.Legend;
		}
		else if (TabControl.SelectedTab == pageConstantLines)
		{
			if (!isConstantLinesBuild)
			{
				isConstantLinesBuild = true;
				BuildConstantLines();
				UpdateStateConstantLines();
			}
			else
			{
				((PropertyGrid)chartWindow.PropertyGrid).SelectedObject = ((((TreeView)tvChartConstantLines).SelectedNode != null) ? ((TreeView)tvChartConstantLines).SelectedNode.Tag : null);
			}
		}
		else if (TabControl.SelectedTab == pageStrips)
		{
			if (!isStripsBuild)
			{
				isStripsBuild = true;
				BuildStrips();
				UpdateStateStrips();
			}
			else
			{
				((PropertyGrid)chartWindow.PropertyGrid).SelectedObject = ((((TreeView)tvChartStrips).SelectedNode != null) ? ((TreeView)tvChartStrips).SelectedNode.Tag : null);
			}
		}
		else if (TabControl.SelectedTab == pageTitle)
		{
			((PropertyGrid)chartWindow.PropertyGrid).SelectedObject = chartWindow.Chart.Title;
		}
		else if (TabControl.SelectedTab == pageTable)
		{
			((PropertyGrid)chartWindow.PropertyGrid).SelectedObject = chartWindow.Chart.Table;
		}
		CheckSamplePosition();
	}

	private void tvChartConstantLinesPanel_Paint(object sender, PaintEventArgs e)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		Pen val = new Pen(Color.FromArgb(170, 170, 170));
		try
		{
			val.DashPattern = new float[2] { 1f, 2f };
			Point point = new Point(StiScale.I(204), 0);
			Point point2 = new Point(StiScale.I(204), ((Control)tvChartConstantLinesPanel).Height);
			e.Graphics.DrawLine(val, point, point2);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void BuildConstantLines()
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		if (chartWindow.Chart.ConstantLines.Count > 0)
		{
			foreach (StiConstantLines constantLine in chartWindow.Chart.ConstantLines)
			{
				TreeNode val = new TreeNode
				{
					Text = constantLine.Text,
					Tag = constantLine
				};
				((TreeView)tvChartConstantLines).Nodes.Add(val);
			}
			((TreeView)tvChartConstantLines).SelectedNode = ((TreeView)tvChartConstantLines).Nodes[0];
		}
		else
		{
			((PropertyGrid)chartWindow.PropertyGrid).SelectedObject = null;
		}
	}

	private void UpdateStateConstantLines()
	{
		if (((TreeView)tvChartConstantLines).Nodes.Count > 0 && ((TreeView)tvChartConstantLines).SelectedNode != null)
		{
			TreeNode selectedNode = ((TreeView)tvChartConstantLines).SelectedNode;
			int num = ((TreeView)tvChartConstantLines).Nodes.IndexOf(selectedNode);
			if (num == 0)
			{
				((Control)buttonMoveUpConstantLines).Enabled = false;
			}
			else
			{
				((Control)buttonMoveUpConstantLines).Enabled = true;
			}
			if (num == ((TreeView)tvChartConstantLines).Nodes.Count - 1)
			{
				((Control)buttonMoveDownConstantLines).Enabled = false;
			}
			else
			{
				((Control)buttonMoveDownConstantLines).Enabled = true;
			}
		}
		else
		{
			((Control)buttonMoveUpConstantLines).Enabled = false;
			((Control)buttonMoveDownConstantLines).Enabled = false;
		}
		((Control)buttonRemoveConstantLines).Enabled = ((TreeView)tvChartConstantLines).Nodes.Count > 0;
	}

	private void tvChartConstantLines_AfterSelect(object sender, TreeViewEventArgs e)
	{
		if (e.Node != null)
		{
			((PropertyGrid)chartWindow.PropertyGrid).SelectedObject = e.Node.Tag;
		}
		else
		{
			((PropertyGrid)chartWindow.PropertyGrid).SelectedObject = null;
		}
		UpdateStateConstantLines();
	}

	private void buttonAddConstantLines_Click(object sender, EventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		StiConstantLines stiConstantLines = new StiConstantLines();
		chartWindow.Chart.ConstantLines.Add(stiConstantLines);
		TreeNode val = new TreeNode
		{
			Text = stiConstantLines.Text,
			Tag = stiConstantLines
		};
		((TreeView)tvChartConstantLines).Nodes.Add(val);
		((TreeView)tvChartConstantLines).SelectedNode = val;
		chartWindow.UpdateSample();
	}

	private void buttonRemoveConstantLines_Click(object sender, EventArgs e)
	{
		StiConstantLines value = (StiConstantLines)((TreeView)tvChartConstantLines).SelectedNode.Tag;
		chartWindow.Chart.ConstantLines.Remove(value);
		TreeNode selectedNode = ((TreeView)tvChartConstantLines).SelectedNode;
		int num = ((TreeView)tvChartConstantLines).Nodes.IndexOf(selectedNode);
		TreeNode val = null;
		if (num == 0)
		{
			if (((TreeView)tvChartConstantLines).Nodes.Count > 1)
			{
				val = ((TreeView)tvChartConstantLines).Nodes[1];
			}
		}
		else
		{
			num--;
			if (num < ((TreeView)tvChartConstantLines).Nodes.Count)
			{
				val = ((TreeView)tvChartConstantLines).Nodes[num];
			}
		}
		((TreeView)tvChartConstantLines).Nodes.Remove(selectedNode);
		if (val != null)
		{
			((TreeView)tvChartConstantLines).SelectedNode = val;
		}
		chartWindow.UpdateSample();
		UpdateStateConstantLines();
	}

	private void buttonMoveUpConstantLines_Click(object sender, EventArgs e)
	{
		TreeNode selectedNode = ((TreeView)tvChartConstantLines).SelectedNode;
		StiConstantLines value = (StiConstantLines)selectedNode.Tag;
		int num = ((TreeView)tvChartConstantLines).Nodes.IndexOf(selectedNode);
		((TreeView)tvChartConstantLines).Nodes.RemoveAt(num);
		chartWindow.Chart.ConstantLines.RemoveAt(num);
		num--;
		((TreeView)tvChartConstantLines).Nodes.Insert(num, selectedNode);
		chartWindow.Chart.ConstantLines.Insert(num, value);
		((TreeView)tvChartConstantLines).SelectedNode = selectedNode;
	}

	private void buttonMoveDownConstantLines_Click(object sender, EventArgs e)
	{
		TreeNode selectedNode = ((TreeView)tvChartConstantLines).SelectedNode;
		StiConstantLines value = (StiConstantLines)selectedNode.Tag;
		int num = ((TreeView)tvChartConstantLines).Nodes.IndexOf(selectedNode);
		((TreeView)tvChartConstantLines).Nodes.RemoveAt(num);
		chartWindow.Chart.ConstantLines.RemoveAt(num);
		num++;
		((TreeView)tvChartConstantLines).Nodes.Insert(num, selectedNode);
		chartWindow.Chart.ConstantLines.Insert(num, value);
		((TreeView)tvChartConstantLines).SelectedNode = selectedNode;
	}

	private void BuildStrips()
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		if (chartWindow.Chart.Strips.Count > 0)
		{
			foreach (StiStrips strip in chartWindow.Chart.Strips)
			{
				TreeNode val = new TreeNode
				{
					Text = strip.Text,
					Tag = strip
				};
				((TreeView)tvChartStrips).Nodes.Add(val);
			}
			((TreeView)tvChartStrips).SelectedNode = ((TreeView)tvChartStrips).Nodes[0];
		}
		else
		{
			((PropertyGrid)chartWindow.PropertyGrid).SelectedObject = null;
		}
	}

	private void UpdateStateStrips()
	{
		if (((TreeView)tvChartStrips).Nodes.Count > 0 && ((TreeView)tvChartStrips).SelectedNode != null)
		{
			TreeNode selectedNode = ((TreeView)tvChartStrips).SelectedNode;
			int num = ((TreeView)tvChartStrips).Nodes.IndexOf(selectedNode);
			if (num == 0)
			{
				((Control)buttonMoveUpStrips).Enabled = false;
			}
			else
			{
				((Control)buttonMoveUpStrips).Enabled = true;
			}
			if (num == ((TreeView)tvChartStrips).Nodes.Count - 1)
			{
				((Control)buttonMoveDownStrips).Enabled = false;
			}
			else
			{
				((Control)buttonMoveDownStrips).Enabled = true;
			}
		}
		else
		{
			((Control)buttonMoveUpStrips).Enabled = false;
			((Control)buttonMoveDownStrips).Enabled = false;
		}
		((Control)buttonRemoveStrips).Enabled = ((TreeView)tvChartStrips).Nodes.Count > 0;
	}

	private void tvChartStrips_AfterSelect(object sender, TreeViewEventArgs e)
	{
		if (e.Node != null)
		{
			((PropertyGrid)chartWindow.PropertyGrid).SelectedObject = e.Node.Tag;
		}
		else
		{
			((PropertyGrid)chartWindow.PropertyGrid).SelectedObject = null;
		}
		UpdateStateStrips();
	}

	private void buttonAddStrips_Click(object sender, EventArgs e)
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		StiStrips stiStrips = new StiStrips();
		chartWindow.Chart.Strips.Add(stiStrips);
		StiGradientBrush stripBrush = new StiGradientBrush(Color.FromArgb(100, 250, 240, 150), Color.FromArgb(100, 150, 120, 60), 90.0);
		stiStrips.StripBrush = stripBrush;
		TreeNode val = new TreeNode
		{
			Text = stiStrips.Text,
			Tag = stiStrips
		};
		((TreeView)tvChartStrips).Nodes.Add(val);
		((TreeView)tvChartStrips).SelectedNode = val;
		chartWindow.UpdateSample();
	}

	private void buttonRemoveStrips_Click(object sender, EventArgs e)
	{
		int num = ((TreeView)tvChartStrips).Nodes.IndexOf(((TreeView)tvChartStrips).SelectedNode);
		chartWindow.Chart.Strips.RemoveAt(num);
		((TreeView)tvChartStrips).Nodes.RemoveAt(num);
		TreeNode val = null;
		if (num == 0)
		{
			if (((TreeView)tvChartStrips).Nodes.Count > 1)
			{
				val = ((TreeView)tvChartStrips).Nodes[1];
			}
		}
		else
		{
			num--;
			if (num < ((TreeView)tvChartStrips).Nodes.Count)
			{
				val = ((TreeView)tvChartStrips).Nodes[num];
			}
		}
		if (val != null)
		{
			((TreeView)tvChartStrips).SelectedNode = val;
		}
		chartWindow.UpdateSample();
		UpdateStateStrips();
	}

	private void buttonMoveUpStrips_Click(object sender, EventArgs e)
	{
		TreeNode selectedNode = ((TreeView)tvChartStrips).SelectedNode;
		StiStrips value = (StiStrips)selectedNode.Tag;
		int num = ((TreeView)tvChartStrips).Nodes.IndexOf(selectedNode);
		((TreeView)tvChartStrips).Nodes.RemoveAt(num);
		chartWindow.Chart.Strips.RemoveAt(num);
		num--;
		((TreeView)tvChartStrips).Nodes.Insert(num, selectedNode);
		chartWindow.Chart.Strips.Insert(num, value);
		((TreeView)tvChartStrips).SelectedNode = selectedNode;
	}

	private void buttonMoveDownStrips_Click(object sender, EventArgs e)
	{
		TreeNode selectedNode = ((TreeView)tvChartStrips).SelectedNode;
		StiStrips value = (StiStrips)selectedNode.Tag;
		int num = ((TreeView)tvChartStrips).Nodes.IndexOf(selectedNode);
		((TreeView)tvChartStrips).Nodes.RemoveAt(num);
		chartWindow.Chart.Strips.RemoveAt(num);
		num++;
		((TreeView)tvChartStrips).Nodes.Insert(num, selectedNode);
		chartWindow.Chart.Strips.Insert(num, value);
		((TreeView)tvChartStrips).SelectedNode = selectedNode;
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
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Expected O, but got Unknown
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_037a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0384: Expected O, but got Unknown
		//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0425: Unknown result type (might be due to invalid IL or missing references)
		//IL_042f: Expected O, but got Unknown
		//IL_0464: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0503: Expected O, but got Unknown
		//IL_0528: Unknown result type (might be due to invalid IL or missing references)
		//IL_054c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0620: Unknown result type (might be due to invalid IL or missing references)
		//IL_062a: Expected O, but got Unknown
		//IL_06a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b2: Expected O, but got Unknown
		//IL_0771: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0805: Expected O, but got Unknown
		//IL_08bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c5: Expected O, but got Unknown
		//IL_097b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a19: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a23: Expected O, but got Unknown
		//IL_0ad9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae3: Expected O, but got Unknown
		//IL_0ba5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0baf: Expected O, but got Unknown
		//IL_0bd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ccb: Expected O, but got Unknown
		//IL_0d49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d53: Expected O, but got Unknown
		//IL_0e12: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea6: Expected O, but got Unknown
		//IL_0f5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f66: Expected O, but got Unknown
		//IL_1016: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_10be: Expected O, but got Unknown
		//IL_1171: Unknown result type (might be due to invalid IL or missing references)
		//IL_117b: Expected O, but got Unknown
		//IL_120e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1218: Expected O, but got Unknown
		//IL_1250: Unknown result type (might be due to invalid IL or missing references)
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiCloudChartControl));
		TabControl = new StiCloudTabulator();
		pageCommon = new StiCloudTabulatorPage();
		pageLegend = new StiCloudTabulatorPage();
		pageTitle = new StiCloudTabulatorPage();
		pageConstantLines = new StiCloudTabulatorPage();
		tvChartConstantLinesPanel = new Panel();
		tvChartConstantLines = new StiTreeView();
		toolBarConstantLines = new StiToolBar();
		buttonMoveDownConstantLines = new StiToolButton();
		buttonMoveUpConstantLines = new StiToolButton();
		separatorConstantLines = new StiToolButton();
		buttonRemoveConstantLines = new StiToolButton();
		buttonAddConstantLines = new StiToolButton();
		pageStrips = new StiCloudTabulatorPage();
		panel1 = new Panel();
		tvChartStrips = new StiTreeView();
		toolBarStrips = new StiToolBar();
		buttonMoveDownStrips = new StiToolButton();
		buttonMoveUpStrips = new StiToolButton();
		separatorStrips = new StiToolButton();
		buttonRemoveStrips = new StiToolButton();
		buttonAddStrips = new StiToolButton();
		pageTable = new StiCloudTabulatorPage();
		((Control)TabControl).SuspendLayout();
		((Control)pageConstantLines).SuspendLayout();
		((Control)tvChartConstantLinesPanel).SuspendLayout();
		((Control)toolBarConstantLines).SuspendLayout();
		((Control)pageStrips).SuspendLayout();
		((Control)panel1).SuspendLayout();
		((Control)toolBarStrips).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)TabControl).Controls.Add((Control)(object)pageCommon);
		((Control)TabControl).Controls.Add((Control)(object)pageLegend);
		((Control)TabControl).Controls.Add((Control)(object)pageTitle);
		((Control)TabControl).Controls.Add((Control)(object)pageConstantLines);
		((Control)TabControl).Controls.Add((Control)(object)pageStrips);
		((Control)TabControl).Controls.Add((Control)(object)pageTable);
		((Control)TabControl).Dock = (DockStyle)5;
		((Control)TabControl).Location = new Point(0, 0);
		((Control)TabControl).Name = "TabControl";
		((Control)TabControl).Padding = new Padding(0, 0, 119, 0);
		TabControl.ScrollHeight = 0;
		TabControl.ScrollWidth = 100;
		((Control)TabControl).Size = new Size(600, 500);
		((Control)TabControl).TabIndex = 1;
		TabControl.TitleBackColor = SystemColors.Control;
		TabControl.TitleInLine = false;
		TabControl.TitlePosition = StiTabTitlePosition.RightHorizontal;
		TabControl.TitleWidth = 110;
		TabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
		pageCommon.Dock = (DockStyle)5;
		pageCommon.Image = (Image)componentResourceManager.GetObject("pageCommon.Image");
		pageCommon.InvisibleVisible = false;
		((Control)pageCommon).Location = new Point(0, 0);
		((Control)pageCommon).Name = "pageCommon";
		((Control)pageCommon).Padding = new Padding(0, 0, 1, 0);
		((Control)pageCommon).Size = new Size(481, 500);
		((Control)pageCommon).TabIndex = 0;
		((Control)pageCommon).Text = "Common";
		((Control)pageCommon).Visible = false;
		pageLegend.Dock = (DockStyle)5;
		pageLegend.Image = (Image)componentResourceManager.GetObject("pageLegend.Image");
		pageLegend.InvisibleVisible = false;
		((Control)pageLegend).Location = new Point(0, 0);
		((Control)pageLegend).Name = "pageLegend";
		((Control)pageLegend).Padding = new Padding(0, 0, 1, 0);
		((Control)pageLegend).Size = new Size(481, 500);
		((Control)pageLegend).TabIndex = 1;
		((Control)pageLegend).Text = "Legend";
		((Control)pageLegend).Visible = false;
		pageTitle.Dock = (DockStyle)5;
		pageTitle.Image = (Image)componentResourceManager.GetObject("pageTitle.Image");
		pageTitle.InvisibleVisible = false;
		((Control)pageTitle).Location = new Point(0, 0);
		((Control)pageTitle).Name = "pageTitle";
		((Control)pageTitle).Padding = new Padding(4);
		((Control)pageTitle).Size = new Size(481, 500);
		((Control)pageTitle).TabIndex = 2;
		((Control)pageTitle).Text = "Title";
		((Control)pageTitle).Visible = false;
		((Control)pageConstantLines).Controls.Add((Control)(object)tvChartConstantLinesPanel);
		((Control)pageConstantLines).Controls.Add((Control)(object)toolBarConstantLines);
		pageConstantLines.Dock = (DockStyle)5;
		pageConstantLines.Image = (Image)componentResourceManager.GetObject("pageConstantLines.Image");
		pageConstantLines.InvisibleVisible = true;
		((Control)pageConstantLines).Location = new Point(0, 0);
		((Control)pageConstantLines).Margin = new Padding(0);
		((Control)pageConstantLines).Name = "pageConstantLines";
		((Control)pageConstantLines).Padding = new Padding(0, 0, 1, 0);
		((Control)pageConstantLines).Size = new Size(481, 500);
		((Control)pageConstantLines).TabIndex = 3;
		((Control)pageConstantLines).Text = "Constant Lines";
		((Control)pageConstantLines).Visible = false;
		((Control)tvChartConstantLinesPanel).Anchor = (AnchorStyles)7;
		((Control)tvChartConstantLinesPanel).BackColor = Color.White;
		((Control)tvChartConstantLinesPanel).Controls.Add((Control)(object)tvChartConstantLines);
		((Control)tvChartConstantLinesPanel).Location = new Point(3, 31);
		((Control)tvChartConstantLinesPanel).Name = "tvChartConstantLinesPanel";
		((Control)tvChartConstantLinesPanel).Size = new Size(210, 466);
		((Control)tvChartConstantLinesPanel).TabIndex = 7;
		((Control)tvChartConstantLinesPanel).Paint += new PaintEventHandler(tvChartConstantLinesPanel_Paint);
		((Control)tvChartConstantLines).Anchor = (AnchorStyles)15;
		((Control)tvChartConstantLines).BackColor = Color.White;
		((TreeView)tvChartConstantLines).BorderStyle = (BorderStyle)0;
		((Control)tvChartConstantLines).Location = new Point(1, 1);
		((Control)tvChartConstantLines).Name = "tvChartConstantLines";
		((Control)tvChartConstantLines).Size = new Size(200, 464);
		((Control)tvChartConstantLines).TabIndex = 6;
		((TreeView)tvChartConstantLines).AfterSelect += new TreeViewEventHandler(tvChartConstantLines_AfterSelect);
		((Control)toolBarConstantLines).Controls.Add((Control)(object)buttonMoveDownConstantLines);
		((Control)toolBarConstantLines).Controls.Add((Control)(object)buttonMoveUpConstantLines);
		((Control)toolBarConstantLines).Controls.Add((Control)(object)separatorConstantLines);
		((Control)toolBarConstantLines).Controls.Add((Control)(object)buttonRemoveConstantLines);
		((Control)toolBarConstantLines).Controls.Add((Control)(object)buttonAddConstantLines);
		toolBarConstantLines.ControlStyle = StiControlStyle.Office2013Blue;
		((Control)toolBarConstantLines).Dock = (DockStyle)1;
		toolBarConstantLines.LineStyle = StiToolBar.StiToolBarLineStyle.Bottom;
		((Control)toolBarConstantLines).Location = new Point(0, 0);
		((Control)toolBarConstantLines).Name = "toolBarConstantLines";
		((Control)toolBarConstantLines).Padding = new Padding(12, 3, 3, 3);
		((Control)toolBarConstantLines).Size = new Size(480, 28);
		((Control)toolBarConstantLines).TabIndex = 5;
		((Control)toolBarConstantLines).Text = "stiToolBar1";
		((Control)buttonMoveDownConstantLines).AutoSize = true;
		((Control)buttonMoveDownConstantLines).Dock = (DockStyle)3;
		buttonMoveDownConstantLines.Draw3DButton = false;
		buttonMoveDownConstantLines.DropDownMenu = null;
		buttonMoveDownConstantLines.HotDragDrop = false;
		((ButtonBase)buttonMoveDownConstantLines).Image = (Image)componentResourceManager.GetObject("buttonMoveDownConstantLines.Image");
		((Control)buttonMoveDownConstantLines).Location = new Point(200, 3);
		((Control)buttonMoveDownConstantLines).Name = "buttonMoveDownConstantLines";
		((Control)buttonMoveDownConstantLines).Size = new Size(26, 22);
		((Control)buttonMoveDownConstantLines).TabIndex = 6;
		((ButtonBase)buttonMoveDownConstantLines).UseVisualStyleBackColor = false;
		((Control)buttonMoveDownConstantLines).Click += buttonMoveDownConstantLines_Click;
		((Control)buttonMoveUpConstantLines).AutoSize = true;
		((Control)buttonMoveUpConstantLines).Dock = (DockStyle)3;
		buttonMoveUpConstantLines.Draw3DButton = false;
		buttonMoveUpConstantLines.DropDownMenu = null;
		buttonMoveUpConstantLines.HotDragDrop = false;
		((ButtonBase)buttonMoveUpConstantLines).Image = (Image)componentResourceManager.GetObject("buttonMoveUpConstantLines.Image");
		((Control)buttonMoveUpConstantLines).Location = new Point(174, 3);
		((Control)buttonMoveUpConstantLines).Name = "buttonMoveUpConstantLines";
		((Control)buttonMoveUpConstantLines).Size = new Size(26, 22);
		((Control)buttonMoveUpConstantLines).TabIndex = 5;
		((ButtonBase)buttonMoveUpConstantLines).UseVisualStyleBackColor = false;
		((Control)buttonMoveUpConstantLines).Click += buttonMoveUpConstantLines_Click;
		((Control)separatorConstantLines).Dock = (DockStyle)3;
		separatorConstantLines.Draw3DButton = false;
		separatorConstantLines.DropDownMenu = null;
		separatorConstantLines.HotDragDrop = false;
		((Control)separatorConstantLines).Location = new Point(166, 3);
		((Control)separatorConstantLines).Margin = new Padding(0);
		((Control)separatorConstantLines).Name = "separatorConstantLines";
		((Control)separatorConstantLines).Size = new Size(8, 22);
		separatorConstantLines.Style = StiToolBarButtonStyle.Separator;
		((Control)separatorConstantLines).TabIndex = 4;
		((ButtonBase)separatorConstantLines).UseVisualStyleBackColor = false;
		((Control)buttonRemoveConstantLines).AutoSize = true;
		((Control)buttonRemoveConstantLines).Dock = (DockStyle)3;
		buttonRemoveConstantLines.Draw3DButton = false;
		buttonRemoveConstantLines.DropDownMenu = null;
		buttonRemoveConstantLines.HotDragDrop = false;
		((ButtonBase)buttonRemoveConstantLines).Image = (Image)componentResourceManager.GetObject("buttonRemoveConstantLines.Image");
		((Control)buttonRemoveConstantLines).Location = new Point(140, 3);
		((Control)buttonRemoveConstantLines).Name = "buttonRemoveConstantLines";
		((Control)buttonRemoveConstantLines).Size = new Size(26, 22);
		((Control)buttonRemoveConstantLines).TabIndex = 2;
		((ButtonBase)buttonRemoveConstantLines).UseVisualStyleBackColor = false;
		((Control)buttonRemoveConstantLines).Click += buttonRemoveConstantLines_Click;
		((Control)buttonAddConstantLines).AutoSize = true;
		((Control)buttonAddConstantLines).Dock = (DockStyle)3;
		buttonAddConstantLines.Draw3DButton = false;
		buttonAddConstantLines.DropDownMenu = null;
		buttonAddConstantLines.HotDragDrop = false;
		((ButtonBase)buttonAddConstantLines).Image = (Image)componentResourceManager.GetObject("buttonAddConstantLines.Image");
		((Control)buttonAddConstantLines).Location = new Point(12, 3);
		((Control)buttonAddConstantLines).Name = "buttonAddConstantLines";
		((Control)buttonAddConstantLines).Size = new Size(128, 22);
		((Control)buttonAddConstantLines).TabIndex = 1;
		((Control)buttonAddConstantLines).Text = "Add Constant Lines";
		((ButtonBase)buttonAddConstantLines).UseVisualStyleBackColor = false;
		((Control)buttonAddConstantLines).Click += buttonAddConstantLines_Click;
		((Control)pageStrips).Controls.Add((Control)(object)panel1);
		((Control)pageStrips).Controls.Add((Control)(object)toolBarStrips);
		pageStrips.Dock = (DockStyle)5;
		pageStrips.Image = (Image)componentResourceManager.GetObject("pageStrips.Image");
		pageStrips.InvisibleVisible = false;
		((Control)pageStrips).Location = new Point(0, 0);
		((Control)pageStrips).Margin = new Padding(0);
		((Control)pageStrips).Name = "pageStrips";
		((Control)pageStrips).Padding = new Padding(0, 0, 1, 0);
		((Control)pageStrips).Size = new Size(481, 500);
		((Control)pageStrips).TabIndex = 4;
		((Control)pageStrips).Text = "Strips";
		((Control)pageStrips).Visible = false;
		((Control)panel1).BackColor = Color.White;
		((Control)panel1).Controls.Add((Control)(object)tvChartStrips);
		((Control)panel1).Location = new Point(3, 31);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(210, 466);
		((Control)panel1).TabIndex = 9;
		((Control)panel1).Paint += new PaintEventHandler(tvChartConstantLinesPanel_Paint);
		((Control)tvChartStrips).Anchor = (AnchorStyles)15;
		((Control)tvChartStrips).BackColor = Color.White;
		((TreeView)tvChartStrips).BorderStyle = (BorderStyle)0;
		((Control)tvChartStrips).Location = new Point(1, 1);
		((Control)tvChartStrips).Name = "tvChartStrips";
		((Control)tvChartStrips).Size = new Size(200, 464);
		((Control)tvChartStrips).TabIndex = 8;
		((TreeView)tvChartStrips).AfterSelect += new TreeViewEventHandler(tvChartStrips_AfterSelect);
		((Control)toolBarStrips).Controls.Add((Control)(object)buttonMoveDownStrips);
		((Control)toolBarStrips).Controls.Add((Control)(object)buttonMoveUpStrips);
		((Control)toolBarStrips).Controls.Add((Control)(object)separatorStrips);
		((Control)toolBarStrips).Controls.Add((Control)(object)buttonRemoveStrips);
		((Control)toolBarStrips).Controls.Add((Control)(object)buttonAddStrips);
		toolBarStrips.ControlStyle = StiControlStyle.Office2013Blue;
		((Control)toolBarStrips).Dock = (DockStyle)1;
		toolBarStrips.LineStyle = StiToolBar.StiToolBarLineStyle.Bottom;
		((Control)toolBarStrips).Location = new Point(0, 0);
		((Control)toolBarStrips).Name = "toolBarStrips";
		((Control)toolBarStrips).Padding = new Padding(12, 3, 3, 3);
		((Control)toolBarStrips).Size = new Size(480, 28);
		((Control)toolBarStrips).TabIndex = 7;
		((Control)toolBarStrips).Text = "stiToolBar1";
		((Control)buttonMoveDownStrips).AutoSize = true;
		((Control)buttonMoveDownStrips).Dock = (DockStyle)3;
		buttonMoveDownStrips.Draw3DButton = false;
		buttonMoveDownStrips.DropDownMenu = null;
		buttonMoveDownStrips.HotDragDrop = false;
		((ButtonBase)buttonMoveDownStrips).Image = (Image)componentResourceManager.GetObject("buttonMoveDownStrips.Image");
		((Control)buttonMoveDownStrips).Location = new Point(130, 3);
		((Control)buttonMoveDownStrips).Name = "buttonMoveDownStrips";
		((Control)buttonMoveDownStrips).Size = new Size(26, 22);
		((Control)buttonMoveDownStrips).TabIndex = 6;
		((ButtonBase)buttonMoveDownStrips).UseVisualStyleBackColor = false;
		((Control)buttonMoveDownStrips).Click += buttonMoveDownStrips_Click;
		((Control)buttonMoveUpStrips).AutoSize = true;
		((Control)buttonMoveUpStrips).Dock = (DockStyle)3;
		buttonMoveUpStrips.Draw3DButton = false;
		buttonMoveUpStrips.DropDownMenu = null;
		buttonMoveUpStrips.HotDragDrop = false;
		((ButtonBase)buttonMoveUpStrips).Image = (Image)componentResourceManager.GetObject("buttonMoveUpStrips.Image");
		((Control)buttonMoveUpStrips).Location = new Point(104, 3);
		((Control)buttonMoveUpStrips).Name = "buttonMoveUpStrips";
		((Control)buttonMoveUpStrips).Size = new Size(26, 22);
		((Control)buttonMoveUpStrips).TabIndex = 5;
		((ButtonBase)buttonMoveUpStrips).UseVisualStyleBackColor = false;
		((Control)buttonMoveUpStrips).Click += buttonMoveUpStrips_Click;
		((Control)separatorStrips).Dock = (DockStyle)3;
		separatorStrips.Draw3DButton = false;
		separatorStrips.DropDownMenu = null;
		separatorStrips.HotDragDrop = false;
		((Control)separatorStrips).Location = new Point(96, 3);
		((Control)separatorStrips).Margin = new Padding(0);
		((Control)separatorStrips).Name = "separatorStrips";
		((Control)separatorStrips).Size = new Size(8, 22);
		separatorStrips.Style = StiToolBarButtonStyle.Separator;
		((Control)separatorStrips).TabIndex = 4;
		((ButtonBase)separatorStrips).UseVisualStyleBackColor = false;
		((Control)buttonRemoveStrips).AutoSize = true;
		((Control)buttonRemoveStrips).Dock = (DockStyle)3;
		buttonRemoveStrips.Draw3DButton = false;
		buttonRemoveStrips.DropDownMenu = null;
		buttonRemoveStrips.HotDragDrop = false;
		((ButtonBase)buttonRemoveStrips).Image = (Image)componentResourceManager.GetObject("buttonRemoveStrips.Image");
		((Control)buttonRemoveStrips).Location = new Point(70, 3);
		((Control)buttonRemoveStrips).Name = "buttonRemoveStrips";
		((Control)buttonRemoveStrips).Size = new Size(26, 22);
		((Control)buttonRemoveStrips).TabIndex = 2;
		((ButtonBase)buttonRemoveStrips).UseVisualStyleBackColor = false;
		((Control)buttonRemoveStrips).Click += buttonRemoveStrips_Click;
		((Control)buttonAddStrips).AutoSize = true;
		((Control)buttonAddStrips).Dock = (DockStyle)3;
		buttonAddStrips.Draw3DButton = false;
		buttonAddStrips.DropDownMenu = null;
		buttonAddStrips.HotDragDrop = false;
		((ButtonBase)buttonAddStrips).Image = (Image)componentResourceManager.GetObject("buttonAddStrips.Image");
		((Control)buttonAddStrips).Location = new Point(12, 3);
		((Control)buttonAddStrips).Name = "buttonAddStrips";
		((Control)buttonAddStrips).Size = new Size(58, 22);
		((Control)buttonAddStrips).TabIndex = 1;
		((Control)buttonAddStrips).Text = "Strips";
		((ButtonBase)buttonAddStrips).UseVisualStyleBackColor = false;
		((Control)buttonAddStrips).Click += buttonAddStrips_Click;
		pageTable.Dock = (DockStyle)5;
		pageTable.Image = (Image)componentResourceManager.GetObject("pageTable.Image");
		pageTable.InvisibleVisible = false;
		((Control)pageTable).Location = new Point(0, 0);
		((Control)pageTable).Name = "pageTable";
		((Control)pageTable).Padding = new Padding(0, 4, 1, 4);
		((Control)pageTable).Size = new Size(481, 500);
		((Control)pageTable).TabIndex = 3;
		((Control)pageTable).Text = "Table";
		((Control)pageTable).Visible = false;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)TabControl);
		((Control)this).Name = "StiCloudChartControl";
		((Control)this).Size = new Size(600, 500);
		((Control)TabControl).ResumeLayout(false);
		((Control)pageConstantLines).ResumeLayout(false);
		((Control)tvChartConstantLinesPanel).ResumeLayout(false);
		((Control)toolBarConstantLines).ResumeLayout(false);
		((Control)toolBarConstantLines).PerformLayout();
		((Control)pageStrips).ResumeLayout(false);
		((Control)panel1).ResumeLayout(false);
		((Control)toolBarStrips).ResumeLayout(false);
		((Control)toolBarStrips).PerformLayout();
		((Control)this).ResumeLayout(false);
	}

	private void @this(object sender, TreeViewEventArgs e)
	{
		throw new NotImplementedException();
	}
}
