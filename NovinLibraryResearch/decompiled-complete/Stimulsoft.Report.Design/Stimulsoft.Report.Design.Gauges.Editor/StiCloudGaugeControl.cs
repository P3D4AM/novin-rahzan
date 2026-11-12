using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Gauge.Design;
using Stimulsoft.Report.Components.Gauge;
using Stimulsoft.Report.Components.Gauge.Primitives;
using Stimulsoft.Report.Design.Chart.Editor;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Design.Properties;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Gauge.Primitives;

namespace Stimulsoft.Report.Design.Gauges.Editor;

[ToolboxItem(false)]
public class StiCloudGaugeControl : UserControl
{
	private class stateHelper
	{
		public bool linearScaleVisible;

		public bool radialScaleVisible;

		public bool linearRangeVisible;

		public bool radialRangeVisible;

		public bool tickCustomValueVisible;

		public bool scaleRangeListVisible;

		public bool stateIndicatorFilterVisible;
	}

	private StiGauge gauge;

	private object lastSelectedObject;

	private TreeNode selectedNode;

	private StiPropertyGrid propertyGrid;

	private bool updateContextMenu = true;

	private bool updateMenu = true;

	internal StiGaugePanel gaugePanel;

	private ButtonItem miAddRadialScale;

	private ButtonItem miAddLinearScale;

	private ButtonItem miAddRadialTickMarkMajor;

	private ButtonItem miAddRadialTickMarkMinor;

	private ButtonItem miAddRadialTickMarkCustom;

	private ButtonItem miAddRadialTickLabelMajor;

	private ButtonItem miAddRadialTickLabelMinor;

	private ButtonItem miAddRadialTickLabelCustom;

	private ButtonItem miAddRadialRangeList;

	private ButtonItem miAddRadialBar;

	private ButtonItem miAddRadialMarker;

	private ButtonItem miAddNeedle;

	private ButtonItem miAddLinearTickMarkMajor;

	private ButtonItem miAddLinearTickMarkMinor;

	private ButtonItem miAddLinearTickMarkCustom;

	private ButtonItem miAddLinearTickLabelMajor;

	private ButtonItem miAddLinearTickLabelMinor;

	private ButtonItem miAddLinearTickLabelCustom;

	private ButtonItem miAddLinearRangeList;

	private ButtonItem miAddLinearBar;

	private ButtonItem miAddLinearMarker;

	private ButtonItem miAddLinearStateIndicator;

	private ButtonItem miAddRadialRange;

	private ButtonItem miAddLinearRange;

	private ButtonItem miAddTickCustomValue;

	private ButtonItem miAddStateIndicatorFilter;

	private ButtonItem miAddBarRangeList;

	private ButtonItem ciAddRadialScale;

	private ButtonItem ciAddRadialTickMarkMajor;

	private ButtonItem ciAddRadialTickMarkMinor;

	private ButtonItem ciAddRadialTickMarkCustom;

	private ButtonItem ciAddRadialTickLabelMajor;

	private ButtonItem ciAddRadialTickLabelMinor;

	private ButtonItem ciAddRadialTickLabelCustom;

	private ButtonItem ciAddRadialRangeList;

	private ButtonItem ciAddRadialBar;

	private ButtonItem ciAddRadialMarker;

	private ButtonItem ciAddNeedle;

	private ButtonItem ciAddRadialStateIndicator;

	private ButtonItem ciGroupRadialScale;

	private ButtonItem ciGroupLinearScale;

	private ButtonItem ciAddTickCustomValue;

	private ButtonItem ciAddStateIndicatorFilter;

	private ButtonItem ciAddBarRangeList;

	private ButtonItem ciAddLinearScale;

	private ButtonItem ciAddLinearTickMarkMajor;

	private ButtonItem ciAddLinearTickMarkMinor;

	private ButtonItem ciAddLinearTickMarkCustom;

	private ButtonItem ciAddLinearTickLabelMajor;

	private ButtonItem ciAddLinearTickLabelMinor;

	private ButtonItem ciAddLinearTickLabelCustom;

	private ButtonItem ciAddLinearRangeList;

	private ButtonItem ciAddLinearBar;

	private ButtonItem ciAddLinearMarker;

	private ButtonItem ciAddLinearStateIndicator;

	private ButtonItem ciAddRadialRange;

	private ButtonItem ciAddLinearRange;

	private IContainer components;

	private Bar stiToolBar1;

	private ButtonItem miAddNewItem;

	private ButtonItem btRemoveItem;

	private ButtonItem btMoveUp;

	private ButtonItem btMoveDown;

	private ButtonItem miGroupRadialScale;

	private ButtonItem miGroupLinearScale;

	private ButtonItem miAddRadialStateIndicator;

	private TreeView tvGauge;

	private LabelItem miEmpty;

	private ImageList ilButtons;

	private StiGroupLine toolBarPanel;

	private StiGroupLine stiGroupLine1;

	private ContextMenuBar contextMenuBar1;

	private ButtonItem ciRoot;

	public StiCloudGaugeControl(StiCloudGaugeForm cloudGaugeDesigner)
	{
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			gauge = cloudGaugeDesigner.Gauge;
			gaugePanel = cloudGaugeDesigner.GaugePanel;
			Localize();
			Build();
			contextMenuBar1.SetContextMenuEx((Control)(object)tvGauge, ciRoot);
			propertyGrid = cloudGaugeDesigner.PropertyGrid;
			propertyGrid.SetStandardPropertyTabs();
			((PropertyGrid)propertyGrid).HelpVisible = false;
		}
		DoScaling();
	}

	private void tvGauge_AfterSelect(object sender, TreeViewEventArgs e)
	{
		updateContextMenu = true;
		updateMenu = true;
		if (tvGauge.SelectedNode != null)
		{
			object tag = tvGauge.SelectedNode.Tag;
			((PropertyGrid)propertyGrid).SelectedObject = tag;
			miAddNewItem.Enabled = true;
			if (tvGauge.SelectedNode.Tag is StiGauge)
			{
				btRemoveItem.Enabled = false;
				btMoveUp.Enabled = false;
				btMoveDown.Enabled = false;
			}
			else
			{
				btRemoveItem.Enabled = true;
				CheckStateMoveButtons(tag);
			}
		}
		else
		{
			((PropertyGrid)propertyGrid).SelectedObject = null;
			miAddNewItem.Enabled = false;
			btRemoveItem.Enabled = false;
			btMoveUp.Enabled = false;
			btMoveDown.Enabled = false;
		}
	}

	private void tvGauge_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 46)
		{
			e.Handled = true;
			btRemoveItem_Click(null, null);
		}
	}

	private void miAddNewItem_PopupShowing(object sender, EventArgs e)
	{
		CheckState(checkContextMenu: false);
	}

	private void miAddNewItem_PopupOpen(object sender, PopupOpenEventArgs e)
	{
		miAddNewItem.PopupOpen -= miAddNewItem_PopupOpen;
		CreateMenu();
	}

	private void btRemoveItem_Click(object sender, EventArgs e)
	{
		TreeNode val = tvGauge.SelectedNode;
		if (val == null)
		{
			return;
		}
		object tag = val.Tag;
		if (tag is StiGauge)
		{
			return;
		}
		object obj = null;
		if (val.Parent.Nodes.Count > 1)
		{
			int num = val.Parent.Nodes.IndexOf(val);
			obj = ((num > 0) ? val.Parent.Nodes[num - 1].Tag : val.Parent.Nodes[num + 1].Tag);
		}
		else
		{
			obj = val.Parent.Tag;
		}
		object tag2 = val.Parent.Tag;
		if (tag is StiRangeBase)
		{
			((StiScaleRangeList)tag2).Ranges.Remove(tag as StiRangeBase);
		}
		else if (tag is StiCustomValueBase)
		{
			((IStiTickCustom)tag2).Values.Remove(tag as StiCustomValueBase);
		}
		else if (tag is StiIndicatorRangeInfo)
		{
			((StiBarBase)tag2).RangeList.Remove(tag as StiIndicatorRangeInfo);
		}
		else if (tag is StiStateIndicatorFilter)
		{
			((StiStateIndicator)tag2).Filters.Remove(tag as StiStateIndicatorFilter);
		}
		else if (tag is StiElementBase)
		{
			if (tag is StiScaleBase)
			{
				((StiGauge)tag2).Scales.Remove(tag as StiScaleBase);
			}
			else if (tag is StiGaugeElement)
			{
				((StiScaleBase)tag2).Items.Remove(tag as StiGaugeElement);
			}
		}
		Build();
		SelectedNodeByTag(tvGauge.Nodes, obj);
	}

	private void btMoveUp_Click(object sender, EventArgs e)
	{
		TreeNode val = tvGauge.SelectedNode;
		if (val == null)
		{
			return;
		}
		object tag = val.Tag;
		if (!(tag is StiGauge))
		{
			object tag2 = val.Parent.Tag;
			bool flag = false;
			if (tag is StiRangeBase)
			{
				flag = ((StiScaleRangeList)tag2).Ranges.MoveUp(tag as StiRangeBase);
			}
			else if (tag is StiCustomValueBase)
			{
				flag = ((IStiTickCustom)tag2).Values.MoveUp(tag as StiCustomValueBase);
			}
			else if (tag is StiIndicatorRangeInfo)
			{
				flag = ((StiBarBase)tag2).RangeList.MoveUp(tag as StiIndicatorRangeInfo);
			}
			else if (tag is StiStateIndicatorFilter)
			{
				flag = ((StiStateIndicator)tag2).Filters.MoveUp(tag as StiStateIndicatorFilter);
			}
			else if (tag is StiScaleBase)
			{
				flag = ((StiGauge)tag2).Scales.MoveUp(tag as StiScaleBase);
			}
			else if (tag is StiGaugeElement)
			{
				flag = ((StiScaleBase)tag2).Items.MoveUp(tag as StiGaugeElement);
			}
			if (flag)
			{
				Build();
			}
		}
	}

	private void btMoveDown_Click(object sender, EventArgs e)
	{
		TreeNode val = tvGauge.SelectedNode;
		if (val == null)
		{
			return;
		}
		object tag = val.Tag;
		if (!(tag is StiGauge))
		{
			object tag2 = val.Parent.Tag;
			bool flag = false;
			if (tag is StiRangeBase)
			{
				flag = ((StiScaleRangeList)tag2).Ranges.MoveDown(tag as StiRangeBase);
			}
			else if (tag is StiCustomValueBase)
			{
				flag = ((IStiTickCustom)tag2).Values.MoveDown(tag as StiCustomValueBase);
			}
			else if (tag is StiIndicatorRangeInfo)
			{
				flag = ((StiBarBase)tag2).RangeList.MoveDown(tag as StiIndicatorRangeInfo);
			}
			else if (tag is StiStateIndicatorFilter)
			{
				flag = ((StiStateIndicator)tag2).Filters.MoveDown(tag as StiStateIndicatorFilter);
			}
			else if (tag is StiScaleBase)
			{
				flag = ((StiGauge)tag2).Scales.MoveDown(tag as StiScaleBase);
			}
			else if (tag is StiGaugeElement)
			{
				flag = ((StiScaleBase)tag2).Items.MoveDown(tag as StiGaugeElement);
			}
			if (flag)
			{
				Build();
			}
		}
	}

	private void AddNewScale_Click(object sender, EventArgs e)
	{
		Type type = ((ButtonItem)sender).Tag as Type;
		StiScaleBase scale = StiActivator.CreateObject(type) as StiScaleBase;
		AddNewScale(scale, type);
	}

	private void AddNewGaugeElement_Click(object sender, EventArgs e)
	{
		Type type = ((ButtonItem)sender).Tag as Type;
		StiGaugeElement element = StiActivator.CreateObject(type) as StiGaugeElement;
		AddNewGaugeElement(element, type);
	}

	private void AddNewRange_Click(object sender, EventArgs e)
	{
		Type type = ((ButtonItem)sender).Tag as Type;
		StiRangeBase rangeBase = StiActivator.CreateObject(type) as StiRangeBase;
		AddNewRange(rangeBase);
	}

	private void AddNewTickCustomValue_Click(object sender, EventArgs e)
	{
		AddNewTickCustomValue();
	}

	private void AddNewBarRangeList_Click(object sender, EventArgs e)
	{
		AddNewBarRangeList();
	}

	private void AddNewStateIndicatorFilter_Click(object sender, EventArgs e)
	{
		AddNewStateIndicatorFilter();
	}

	private void ciRoot_PopupOpen(object sender, PopupOpenEventArgs e)
	{
		Point position = Cursor.Position;
		position = ((Control)tvGauge).PointToClient(position);
		TreeNode nodeAt = tvGauge.GetNodeAt(position);
		if (nodeAt == null)
		{
			e.Cancel = true;
			return;
		}
		tvGauge.SelectedNode = nodeAt;
		if (ciRoot.SubItems.Count == 0)
		{
			CreateContextMenu();
		}
		CheckState(checkContextMenu: true);
	}

	private void Localize()
	{
		btRemoveItem.Tooltip = StiHint.CreateHint(StiLocalization.Get("FormDictionaryDesigner", "Delete"));
		btMoveUp.Tooltip = StiHint.CreateHint(StiLocalization.Get("Chart", "MoveSeriesUp"));
		btMoveDown.Tooltip = StiHint.CreateHint(StiLocalization.Get("Chart", "MoveSeriesDown"));
	}

	private void Build()
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		if (tvGauge.SelectedNode != null)
		{
			lastSelectedObject = tvGauge.SelectedNode.Tag;
		}
		((Control)tvGauge).SuspendLayout();
		tvGauge.Nodes.Clear();
		TreeNode val = new TreeNode(gauge.Name, 3, 3);
		val.Tag = gauge;
		foreach (StiScaleBase scale in gauge.Scales)
		{
			TreeNode val2 = null;
			if (scale is StiLinearScale)
			{
				val2 = new TreeNode("LinearScale", 4, 4);
				BuildLinearScale(scale as StiLinearScale, val2);
			}
			else
			{
				val2 = new TreeNode("RadialScale", 5, 5);
				BuildRadialScale(scale as StiRadialScale, val2);
			}
			val2.Tag = scale;
			val.Nodes.Add(val2);
			if (lastSelectedObject == scale)
			{
				selectedNode = val2;
			}
		}
		tvGauge.Nodes.Add(val);
		tvGauge.ExpandAll();
		tvGauge.SelectedNode = ((selectedNode == null) ? tvGauge.Nodes[0] : selectedNode);
		((Control)tvGauge).ResumeLayout(false);
		((Control)gaugePanel).Invalidate();
		lastSelectedObject = null;
		selectedNode = null;
	}

	private void BuildLinearScale(StiLinearScale scale, TreeNode scaleNode)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Expected O, but got Unknown
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Expected O, but got Unknown
		foreach (StiGaugeElement item in scale.Items)
		{
			TreeNode val = null;
			if (item is StiLinearTickMarkMajor)
			{
				val = new TreeNode("TickMarkMajor", 6, 6);
			}
			else if (item is StiLinearTickMarkMinor)
			{
				val = new TreeNode("TickMarkMinor", 7, 7);
			}
			else if (item is StiLinearTickMarkCustom)
			{
				val = new TreeNode("TickMarkCustom", 8, 8);
				BuildTickCustom(item as IStiTickCustom, val, 8);
			}
			else if (item is StiLinearTickLabelMajor)
			{
				val = new TreeNode("TickLabelMajor", 9, 9);
			}
			else if (item is StiLinearTickLabelMinor)
			{
				val = new TreeNode("TickLabelMinor", 10, 10);
			}
			else if (item is StiLinearTickLabelCustom)
			{
				val = new TreeNode("TickLabelCustom", 11, 11);
				BuildTickCustom(item as IStiTickCustom, val, 11);
			}
			else if (item is StiLinearRangeList)
			{
				val = new TreeNode("RangeList", 12, 12);
				BuildLinearRangeList(item as StiLinearRangeList, val);
			}
			else if (item is StiLinearBar)
			{
				val = new TreeNode("LinearBar", 13, 13);
				BuildBar(item as StiBarBase, val, 13);
			}
			else if (item is StiLinearMarker)
			{
				val = new TreeNode("LinearMarker", 14, 14);
			}
			else if (item is StiStateIndicator)
			{
				val = new TreeNode("StateIndicator", 15, 15);
				BuildIndicatorState(item as StiStateIndicator, val, 15);
			}
			if (val != null)
			{
				val.Tag = item;
				scaleNode.Nodes.Add(val);
				if (lastSelectedObject == item)
				{
					selectedNode = val;
				}
			}
		}
	}

	private void BuildRadialScale(StiRadialScale scale, TreeNode scaleNode)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Expected O, but got Unknown
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Expected O, but got Unknown
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Expected O, but got Unknown
		foreach (StiGaugeElement item in scale.Items)
		{
			TreeNode val = null;
			if (item is StiRadialTickMarkMajor)
			{
				val = new TreeNode("TickMarkMajor", 16, 16);
			}
			else if (item is StiRadialTickMarkMinor)
			{
				val = new TreeNode("TickMarkMinor", 17, 17);
			}
			else if (item is StiRadialTickMarkCustom)
			{
				val = new TreeNode("TickMarkCustom", 18, 18);
				BuildTickCustom(item as IStiTickCustom, val, 18);
			}
			else if (item is StiRadialTickLabelMajor)
			{
				val = new TreeNode("TickLabelMajor", 19, 19);
			}
			else if (item is StiRadialTickLabelMinor)
			{
				val = new TreeNode("TickLabelMinor", 20, 20);
			}
			else if (item is StiRadialTickLabelCustom)
			{
				val = new TreeNode("TickLabelCustom", 21, 21);
				BuildTickCustom(item as IStiTickCustom, val, 21);
			}
			else if (item is StiRadialRangeList)
			{
				val = new TreeNode("RangeList", 22, 22);
				BuildRadialRangeList(item as StiRadialRangeList, val);
			}
			else if (item is StiRadialBar)
			{
				val = new TreeNode("RadialBar", 23, 23);
				BuildBar(item as StiBarBase, val, 23);
			}
			else if (item is StiRadialMarker)
			{
				val = new TreeNode("RadialMarker", 24, 24);
			}
			else if (item is StiNeedle)
			{
				val = new TreeNode("Needle", 25, 25);
			}
			else if (item is StiStateIndicator)
			{
				val = new TreeNode("StateIndicator", 26, 26);
				BuildIndicatorState(item as StiStateIndicator, val, 26);
			}
			if (val != null)
			{
				val.Tag = item;
				scaleNode.Nodes.Add(val);
				if (lastSelectedObject == item)
				{
					selectedNode = val;
				}
			}
		}
	}

	private void BuildLinearRangeList(StiLinearRangeList rangeList, TreeNode rangeListNode)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		foreach (StiLinearRange range in rangeList.Ranges)
		{
			TreeNode val = new TreeNode($"Range: {range.StartValue}-{range.EndValue}", 27, 27);
			val.Tag = range;
			rangeListNode.Nodes.Add(val);
			if (lastSelectedObject == range)
			{
				selectedNode = val;
			}
		}
	}

	private void BuildRadialRangeList(StiRadialRangeList rangeList, TreeNode rangeListNode)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		foreach (StiRadialRange range in rangeList.Ranges)
		{
			TreeNode val = new TreeNode($"Range: {range.StartValue}-{range.EndValue}", 28, 28);
			val.Tag = range;
			rangeListNode.Nodes.Add(val);
			if (lastSelectedObject == range)
			{
				selectedNode = val;
			}
		}
	}

	private void BuildTickCustom(IStiTickCustom tickCustom, TreeNode rootNode, int imageIndex)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		foreach (StiCustomValueBase value in tickCustom.Values)
		{
			TreeNode val = new TreeNode($"{value.LocalizedName}: Value={value.Value}", 31, 31);
			val.Tag = value;
			rootNode.Nodes.Add(val);
			if (lastSelectedObject == value)
			{
				selectedNode = val;
			}
		}
	}

	private void BuildBar(StiBarBase bar, TreeNode rootNode, int imageIndex)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		foreach (StiIndicatorRangeInfo range in bar.RangeList)
		{
			TreeNode val = new TreeNode("IndicatorRangeInfo: Value=" + range.Value, 29, 29);
			val.Tag = range;
			rootNode.Nodes.Add(val);
			if (lastSelectedObject == range)
			{
				selectedNode = val;
			}
		}
	}

	private void BuildIndicatorState(StiStateIndicator indicator, TreeNode rootNode, int imageIndex)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		foreach (StiStateIndicatorFilter filter in indicator.Filters)
		{
			TreeNode val = new TreeNode(filter.ToString(), 30, 30);
			val.Tag = filter;
			rootNode.Nodes.Add(val);
			if (lastSelectedObject == filter)
			{
				selectedNode = val;
			}
		}
	}

	private bool SelectedNodeByTag(TreeNodeCollection nodes, object tag)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		foreach (TreeNode node in nodes)
		{
			TreeNode val = node;
			if (val.Tag == tag)
			{
				tvGauge.SelectedNode = val;
				return true;
			}
			if (val.Nodes.Count > 0 && SelectedNodeByTag(val.Nodes, tag))
			{
				return true;
			}
		}
		return false;
	}

	private void CreateMenu()
	{
		((Control)this).SuspendLayout();
		stiToolBar1.BeginInit();
		miAddNewItem.SubItems.RemoveAt(0);
		miEmpty = null;
		miGroupRadialScale = new ButtonItem();
		miAddRadialScale = new ButtonItem();
		miAddRadialTickMarkMajor = new ButtonItem();
		miAddRadialTickMarkMinor = new ButtonItem();
		miAddRadialTickMarkCustom = new ButtonItem();
		miAddRadialTickLabelMajor = new ButtonItem();
		miAddRadialTickLabelMinor = new ButtonItem();
		miAddRadialTickLabelCustom = new ButtonItem();
		miAddRadialRangeList = new ButtonItem();
		miAddRadialBar = new ButtonItem();
		miAddRadialMarker = new ButtonItem();
		miAddNeedle = new ButtonItem();
		miAddRadialStateIndicator = new ButtonItem();
		miGroupLinearScale = new ButtonItem();
		miAddLinearScale = new ButtonItem();
		miAddLinearTickMarkMajor = new ButtonItem();
		miAddLinearTickMarkMinor = new ButtonItem();
		miAddLinearTickMarkCustom = new ButtonItem();
		miAddLinearTickLabelMajor = new ButtonItem();
		miAddLinearTickLabelMinor = new ButtonItem();
		miAddLinearTickLabelCustom = new ButtonItem();
		miAddLinearRangeList = new ButtonItem();
		miAddLinearBar = new ButtonItem();
		miAddLinearMarker = new ButtonItem();
		miAddLinearStateIndicator = new ButtonItem();
		miAddRadialRange = new ButtonItem();
		miAddLinearRange = new ButtonItem();
		miAddTickCustomValue = new ButtonItem();
		miAddStateIndicatorFilter = new ButtonItem();
		miAddBarRangeList = new ButtonItem();
		miGroupRadialScale.Image = (Image)(object)Resources.radial_scale;
		miGroupRadialScale.Text = "Radial Scale";
		miGroupRadialScale.SubItems.AddRange(new BaseItem[12]
		{
			miAddRadialScale, miAddRadialTickMarkMajor, miAddRadialTickMarkMinor, miAddRadialTickMarkCustom, miAddRadialTickLabelMajor, miAddRadialTickLabelMinor, miAddRadialTickLabelCustom, miAddRadialRangeList, miAddRadialBar, miAddRadialMarker,
			miAddNeedle, miAddRadialStateIndicator
		});
		miAddRadialScale.Image = (Image)(object)Resources.radial_scale;
		miAddRadialScale.Tag = typeof(StiRadialScale);
		miAddRadialScale.Text = "Radial Scale";
		miAddRadialScale.Click += AddNewScale_Click;
		miAddRadialTickMarkMajor.BeginGroup = true;
		miAddRadialTickMarkMajor.Image = (Image)(object)Resources.radial_tick_mark_major;
		miAddRadialTickMarkMajor.Tag = typeof(StiRadialTickMarkMajor);
		miAddRadialTickMarkMajor.Text = "Radial Tick Mark Major";
		miAddRadialTickMarkMajor.Click += AddNewGaugeElement_Click;
		miAddRadialTickMarkMinor.Image = (Image)(object)Resources.radial_tick_mark_minor;
		miAddRadialTickMarkMinor.Tag = typeof(StiRadialTickMarkMinor);
		miAddRadialTickMarkMinor.Text = "Radial Tick Mark Minor";
		miAddRadialTickMarkMinor.Click += AddNewGaugeElement_Click;
		miAddRadialTickMarkCustom.Image = (Image)(object)Resources.radial_tick_mark_custom;
		miAddRadialTickMarkCustom.Tag = typeof(StiRadialTickMarkCustom);
		miAddRadialTickMarkCustom.Text = "Radial Tick Mark Custom";
		miAddRadialTickMarkCustom.Click += AddNewGaugeElement_Click;
		miAddRadialTickLabelMajor.Image = (Image)(object)Resources.radial_tick_label_major;
		miAddRadialTickLabelMajor.Tag = typeof(StiRadialTickLabelMajor);
		miAddRadialTickLabelMajor.Text = "Radial Tick Label Major";
		miAddRadialTickLabelMajor.Click += AddNewGaugeElement_Click;
		miAddRadialTickLabelMinor.Image = (Image)(object)Resources.radial_tick_label_minor;
		miAddRadialTickLabelMinor.Tag = typeof(StiRadialTickLabelMinor);
		miAddRadialTickLabelMinor.Text = "Radial Tick Label Minor";
		miAddRadialTickLabelMinor.Click += AddNewGaugeElement_Click;
		miAddRadialTickLabelCustom.Image = (Image)(object)Resources.radial_tick_label_custom;
		miAddRadialTickLabelCustom.Tag = typeof(StiRadialTickLabelCustom);
		miAddRadialTickLabelCustom.Text = "Radial Tick Label Custom";
		miAddRadialTickLabelCustom.Click += AddNewGaugeElement_Click;
		miAddRadialRangeList.BeginGroup = true;
		miAddRadialRangeList.Tag = typeof(StiRadialRangeList);
		miAddRadialRangeList.Image = (Image)(object)Resources.radial_range;
		miAddRadialRangeList.Text = "Radial Range List";
		miAddRadialRangeList.Click += AddNewGaugeElement_Click;
		miAddRadialBar.BeginGroup = true;
		miAddRadialBar.Image = (Image)(object)Resources.radial_bar;
		miAddRadialBar.Tag = typeof(StiRadialBar);
		miAddRadialBar.Text = "Radial Bar";
		miAddRadialBar.Click += AddNewGaugeElement_Click;
		miAddRadialMarker.Image = (Image)(object)Resources.radial_marker;
		miAddRadialMarker.Tag = typeof(StiRadialMarker);
		miAddRadialMarker.Text = "Radial Marker";
		miAddRadialMarker.Click += AddNewGaugeElement_Click;
		miAddNeedle.Image = (Image)(object)Resources.needle;
		miAddNeedle.Tag = typeof(StiNeedle);
		miAddNeedle.Text = "Needle";
		miAddNeedle.Click += AddNewGaugeElement_Click;
		miAddRadialStateIndicator.BeginGroup = true;
		miAddRadialStateIndicator.Image = (Image)(object)Resources.state_indicator;
		miAddRadialStateIndicator.Tag = typeof(StiStateIndicator);
		miAddRadialStateIndicator.Text = "State Indicator";
		miAddRadialStateIndicator.Click += AddNewGaugeElement_Click;
		miGroupLinearScale.Image = (Image)(object)Resources.linear_scale;
		miGroupLinearScale.Text = "Linear Scale";
		miGroupLinearScale.SubItems.AddRange(new BaseItem[11]
		{
			miAddLinearScale, miAddLinearTickMarkMajor, miAddLinearTickMarkMinor, miAddLinearTickMarkCustom, miAddLinearTickLabelMajor, miAddLinearTickLabelMinor, miAddLinearTickLabelCustom, miAddLinearRangeList, miAddLinearBar, miAddLinearMarker,
			miAddLinearStateIndicator
		});
		miAddLinearScale.Image = (Image)(object)Resources.linear_scale;
		miAddLinearScale.Tag = typeof(StiLinearScale);
		miAddLinearScale.Text = "Linear Scale";
		miAddLinearScale.Click += AddNewScale_Click;
		miAddLinearTickMarkMajor.BeginGroup = true;
		miAddLinearTickMarkMajor.Tag = typeof(StiLinearTickMarkMajor);
		miAddLinearTickMarkMajor.Image = (Image)(object)Resources.linear_tick_mark_major;
		miAddLinearTickMarkMajor.Text = "Linear Tick Mark Major";
		miAddLinearTickMarkMajor.Click += AddNewGaugeElement_Click;
		miAddLinearTickMarkMinor.Image = (Image)(object)Resources.linear_tick_mark_minor;
		miAddLinearTickMarkMinor.Tag = typeof(StiLinearTickMarkMinor);
		miAddLinearTickMarkMinor.Text = "Linear Tick Mark Minor";
		miAddLinearTickMarkMinor.Click += AddNewGaugeElement_Click;
		miAddLinearTickMarkCustom.Image = (Image)(object)Resources.linear_tick_mark_custom;
		miAddLinearTickMarkCustom.Tag = typeof(StiLinearTickMarkCustom);
		miAddLinearTickMarkCustom.Text = "Linear Tick Mark Custom";
		miAddLinearTickMarkCustom.Click += AddNewGaugeElement_Click;
		miAddLinearTickLabelMajor.Image = (Image)(object)Resources.linear_tick_label_major;
		miAddLinearTickLabelMajor.Tag = typeof(StiLinearTickLabelMajor);
		miAddLinearTickLabelMajor.Text = "Linear Tick Label Major";
		miAddLinearTickLabelMajor.Click += AddNewGaugeElement_Click;
		miAddLinearTickLabelMinor.Image = (Image)(object)Resources.linear_tick_label_minor;
		miAddLinearTickLabelMinor.Tag = typeof(StiLinearTickLabelMinor);
		miAddLinearTickLabelMinor.Text = "Linear Tick Label Minor";
		miAddLinearTickLabelMinor.Click += AddNewGaugeElement_Click;
		miAddLinearTickLabelCustom.Image = (Image)(object)Resources.linear_tick_label_custom;
		miAddLinearTickLabelCustom.Tag = typeof(StiLinearTickLabelCustom);
		miAddLinearTickLabelCustom.Text = "Linear Tick Label Custom";
		miAddLinearTickLabelCustom.Click += AddNewGaugeElement_Click;
		miAddLinearRangeList.BeginGroup = true;
		miAddLinearRangeList.Image = (Image)(object)Resources.Linear_Range;
		miAddLinearRangeList.Tag = typeof(StiLinearRangeList);
		miAddLinearRangeList.Text = "Linear Range List";
		miAddLinearRangeList.Click += AddNewGaugeElement_Click;
		miAddLinearBar.BeginGroup = true;
		miAddLinearBar.Image = (Image)(object)Resources.linear_bar;
		miAddLinearBar.Tag = typeof(StiLinearBar);
		miAddLinearBar.Text = "Linear Bar";
		miAddLinearBar.Click += AddNewGaugeElement_Click;
		miAddLinearMarker.Image = (Image)(object)Resources.linear_maker;
		miAddLinearMarker.Tag = typeof(StiLinearMarker);
		miAddLinearMarker.Text = "Linear Marker";
		miAddLinearMarker.Click += AddNewGaugeElement_Click;
		miAddLinearStateIndicator.BeginGroup = true;
		miAddLinearStateIndicator.Tag = typeof(StiStateIndicator);
		miAddLinearStateIndicator.Image = (Image)(object)Resources.state_indicator_linear;
		miAddLinearStateIndicator.Text = "State Indicator";
		miAddLinearStateIndicator.Click += AddNewGaugeElement_Click;
		miAddRadialRange.BeginGroup = true;
		miAddRadialRange.Tag = typeof(StiRadialRange);
		miAddRadialRange.Image = (Image)(object)Resources.radial_range_list;
		miAddRadialRange.Text = "Radial Range";
		miAddRadialRange.Click += AddNewRange_Click;
		miAddLinearRange.Image = (Image)(object)Resources.linear_range_list;
		miAddLinearRange.Tag = typeof(StiLinearRange);
		miAddLinearRange.Text = "Linear Range";
		miAddLinearRange.Click += AddNewRange_Click;
		miAddTickCustomValue.BeginGroup = true;
		miAddTickCustomValue.Image = (Image)(object)Resources.TickCustomValue;
		miAddTickCustomValue.Text = "Tick Custom Value";
		miAddTickCustomValue.Click += AddNewTickCustomValue_Click;
		miAddStateIndicatorFilter.BeginGroup = true;
		miAddStateIndicatorFilter.Image = (Image)(object)Resources.StateIndicatorFilter;
		miAddStateIndicatorFilter.Text = "State Indicator Filter";
		miAddStateIndicatorFilter.Click += AddNewStateIndicatorFilter_Click;
		miAddBarRangeList.BeginGroup = true;
		miAddBarRangeList.Image = (Image)(object)Resources.BarRangeList;
		miAddBarRangeList.Text = "Bar Range List";
		miAddBarRangeList.Click += AddNewBarRangeList_Click;
		miAddNewItem.SubItems.AddRange(new BaseItem[7] { miGroupRadialScale, miGroupLinearScale, miAddRadialRange, miAddLinearRange, miAddTickCustomValue, miAddStateIndicatorFilter, miAddBarRangeList });
		miGroupRadialScale.Image = StiImageResizer.GetImage(miGroupRadialScale.Image);
		miGroupLinearScale.Image = StiImageResizer.GetImage(miGroupLinearScale.Image);
		miAddBarRangeList.Image = StiImageResizer.GetImage(miAddBarRangeList.Image);
		miAddLinearBar.Image = StiImageResizer.GetImage(miAddLinearBar.Image);
		miAddLinearMarker.Image = StiImageResizer.GetImage(miAddLinearMarker.Image);
		miAddLinearRange.Image = StiImageResizer.GetImage(miAddLinearRange.Image);
		miAddLinearRangeList.Image = StiImageResizer.GetImage(miAddLinearRangeList.Image);
		miAddLinearScale.Image = StiImageResizer.GetImage(miAddLinearScale.Image);
		miAddLinearStateIndicator.Image = StiImageResizer.GetImage(miAddLinearStateIndicator.Image);
		miAddLinearTickLabelCustom.Image = StiImageResizer.GetImage(miAddLinearTickLabelCustom.Image);
		miAddLinearTickLabelMajor.Image = StiImageResizer.GetImage(miAddLinearTickLabelMajor.Image);
		miAddLinearTickLabelMinor.Image = StiImageResizer.GetImage(miAddLinearTickLabelMinor.Image);
		miAddLinearTickMarkCustom.Image = StiImageResizer.GetImage(miAddLinearTickMarkCustom.Image);
		miAddLinearTickMarkMajor.Image = StiImageResizer.GetImage(miAddLinearTickMarkMajor.Image);
		miAddLinearTickMarkMinor.Image = StiImageResizer.GetImage(miAddLinearTickMarkMinor.Image);
		miAddNeedle.Image = StiImageResizer.GetImage(miAddNeedle.Image);
		miAddRadialBar.Image = StiImageResizer.GetImage(miAddRadialBar.Image);
		miAddRadialMarker.Image = StiImageResizer.GetImage(miAddRadialMarker.Image);
		miAddRadialRange.Image = StiImageResizer.GetImage(miAddRadialRange.Image);
		miAddRadialRangeList.Image = StiImageResizer.GetImage(miAddRadialRangeList.Image);
		miAddRadialScale.Image = StiImageResizer.GetImage(miAddRadialScale.Image);
		miAddRadialStateIndicator.Image = StiImageResizer.GetImage(miAddRadialStateIndicator.Image);
		miAddRadialTickLabelCustom.Image = StiImageResizer.GetImage(miAddRadialTickLabelCustom.Image);
		miAddRadialTickLabelMajor.Image = StiImageResizer.GetImage(miAddRadialTickLabelMajor.Image);
		miAddRadialTickLabelMinor.Image = StiImageResizer.GetImage(miAddRadialTickLabelMinor.Image);
		miAddRadialTickMarkCustom.Image = StiImageResizer.GetImage(miAddRadialTickMarkCustom.Image);
		miAddRadialTickMarkMajor.Image = StiImageResizer.GetImage(miAddRadialTickMarkMajor.Image);
		miAddRadialTickMarkMinor.Image = StiImageResizer.GetImage(miAddRadialTickMarkMinor.Image);
		miAddStateIndicatorFilter.Image = StiImageResizer.GetImage(miAddStateIndicatorFilter.Image);
		miAddTickCustomValue.Image = StiImageResizer.GetImage(miAddTickCustomValue.Image);
		stiToolBar1.EndInit();
		((Control)this).ResumeLayout(false);
	}

	private void CreateContextMenu()
	{
		contextMenuBar1.BeginInit();
		ciGroupRadialScale = new ButtonItem();
		ciAddRadialScale = new ButtonItem();
		ciAddRadialTickMarkMajor = new ButtonItem();
		ciAddRadialTickMarkMinor = new ButtonItem();
		ciAddRadialTickMarkCustom = new ButtonItem();
		ciAddRadialTickLabelMajor = new ButtonItem();
		ciAddRadialTickLabelMinor = new ButtonItem();
		ciAddRadialTickLabelCustom = new ButtonItem();
		ciAddRadialRangeList = new ButtonItem();
		ciAddRadialBar = new ButtonItem();
		ciAddRadialMarker = new ButtonItem();
		ciAddNeedle = new ButtonItem();
		ciAddRadialStateIndicator = new ButtonItem();
		ciGroupLinearScale = new ButtonItem();
		ciAddLinearScale = new ButtonItem();
		ciAddLinearTickMarkMajor = new ButtonItem();
		ciAddLinearTickMarkMinor = new ButtonItem();
		ciAddLinearTickMarkCustom = new ButtonItem();
		ciAddLinearTickLabelMajor = new ButtonItem();
		ciAddLinearTickLabelMinor = new ButtonItem();
		ciAddLinearTickLabelCustom = new ButtonItem();
		ciAddLinearRangeList = new ButtonItem();
		ciAddLinearBar = new ButtonItem();
		ciAddLinearMarker = new ButtonItem();
		ciAddLinearStateIndicator = new ButtonItem();
		ciAddRadialRange = new ButtonItem();
		ciAddLinearRange = new ButtonItem();
		ciAddTickCustomValue = new ButtonItem();
		ciAddStateIndicatorFilter = new ButtonItem();
		ciAddBarRangeList = new ButtonItem();
		ciGroupRadialScale.Image = (Image)(object)Resources.radial_scale;
		ciGroupRadialScale.Text = "Radial Scale";
		ciGroupRadialScale.SubItems.AddRange(new BaseItem[12]
		{
			ciAddRadialScale, ciAddRadialTickMarkMajor, ciAddRadialTickMarkMinor, ciAddRadialTickMarkCustom, ciAddRadialTickLabelMajor, ciAddRadialTickLabelMinor, ciAddRadialTickLabelCustom, ciAddRadialRangeList, ciAddRadialBar, ciAddRadialMarker,
			ciAddNeedle, ciAddRadialStateIndicator
		});
		ciAddRadialScale.Image = (Image)(object)Resources.radial_scale;
		ciAddRadialScale.Tag = typeof(StiRadialScale);
		ciAddRadialScale.Text = "Radial Scale";
		ciAddRadialScale.Click += AddNewScale_Click;
		ciAddRadialTickMarkMajor.BeginGroup = true;
		ciAddRadialTickMarkMajor.Image = (Image)(object)Resources.radial_tick_mark_major;
		ciAddRadialTickMarkMajor.Tag = typeof(StiRadialTickMarkMajor);
		ciAddRadialTickMarkMajor.Text = "Radial Tick Mark Major";
		ciAddRadialTickMarkMajor.Click += AddNewGaugeElement_Click;
		ciAddRadialTickMarkMinor.Image = (Image)(object)Resources.radial_tick_mark_minor;
		ciAddRadialTickMarkMinor.Tag = typeof(StiRadialTickMarkMinor);
		ciAddRadialTickMarkMinor.Text = "Radial Tick Mark Minor";
		ciAddRadialTickMarkMinor.Click += AddNewGaugeElement_Click;
		ciAddRadialTickMarkCustom.Image = (Image)(object)Resources.radial_tick_mark_custom;
		ciAddRadialTickMarkCustom.Tag = typeof(StiRadialTickMarkCustom);
		ciAddRadialTickMarkCustom.Text = "Radial Tick Mark Custom";
		ciAddRadialTickMarkCustom.Click += AddNewGaugeElement_Click;
		ciAddRadialTickLabelMajor.Image = (Image)(object)Resources.radial_tick_label_major;
		ciAddRadialTickLabelMajor.Tag = typeof(StiRadialTickLabelMajor);
		ciAddRadialTickLabelMajor.Text = "Radial Tick Label Major";
		ciAddRadialTickLabelMajor.Click += AddNewGaugeElement_Click;
		ciAddRadialTickLabelMinor.Image = (Image)(object)Resources.radial_tick_label_minor;
		ciAddRadialTickLabelMinor.Tag = typeof(StiRadialTickLabelMinor);
		ciAddRadialTickLabelMinor.Text = "Radial Tick Label Minor";
		ciAddRadialTickLabelMinor.Click += AddNewGaugeElement_Click;
		ciAddRadialTickLabelCustom.Image = (Image)(object)Resources.radial_tick_label_custom;
		ciAddRadialTickLabelCustom.Tag = typeof(StiRadialTickLabelCustom);
		ciAddRadialTickLabelCustom.Text = "Radial Tick Label Custom";
		ciAddRadialTickLabelCustom.Click += AddNewGaugeElement_Click;
		ciAddRadialRangeList.BeginGroup = true;
		ciAddRadialRangeList.Image = (Image)(object)Resources.radial_range;
		ciAddRadialRangeList.Tag = typeof(StiRadialRangeList);
		ciAddRadialRangeList.Text = "Radial Range List";
		ciAddRadialRangeList.Click += AddNewGaugeElement_Click;
		ciAddRadialBar.BeginGroup = true;
		ciAddRadialBar.Image = (Image)(object)Resources.radial_bar;
		ciAddRadialBar.Tag = typeof(StiRadialBar);
		ciAddRadialBar.Text = "Radial Bar";
		ciAddRadialBar.Click += AddNewGaugeElement_Click;
		ciAddRadialMarker.Image = (Image)(object)Resources.radial_marker;
		ciAddRadialMarker.Tag = typeof(StiRadialMarker);
		ciAddRadialMarker.Text = "Radial Marker";
		ciAddRadialMarker.Click += AddNewGaugeElement_Click;
		ciAddNeedle.Image = (Image)(object)Resources.needle;
		ciAddNeedle.Tag = typeof(StiNeedle);
		ciAddNeedle.Text = "Needle";
		ciAddNeedle.Click += AddNewGaugeElement_Click;
		ciAddRadialStateIndicator.BeginGroup = true;
		ciAddRadialStateIndicator.Image = (Image)(object)Resources.state_indicator;
		ciAddRadialStateIndicator.Tag = typeof(StiStateIndicator);
		ciAddRadialStateIndicator.Text = "Radial State Indicator";
		ciAddRadialStateIndicator.Click += AddNewGaugeElement_Click;
		ciGroupLinearScale.Image = (Image)(object)Resources.linear_scale;
		ciGroupLinearScale.Text = "Linear Scale";
		ciGroupLinearScale.SubItems.AddRange(new BaseItem[11]
		{
			ciAddLinearScale, ciAddLinearTickMarkMajor, ciAddLinearTickMarkMinor, ciAddLinearTickMarkCustom, ciAddLinearTickLabelMajor, ciAddLinearTickLabelMinor, ciAddLinearTickLabelCustom, ciAddLinearRangeList, ciAddLinearBar, ciAddLinearMarker,
			ciAddLinearStateIndicator
		});
		ciAddLinearScale.Image = (Image)(object)Resources.linear_scale;
		ciAddLinearScale.Tag = typeof(StiLinearScale);
		ciAddLinearScale.Text = "Linear Scale";
		ciAddLinearScale.Click += AddNewScale_Click;
		ciAddLinearTickMarkMajor.BeginGroup = true;
		ciAddLinearTickMarkMajor.Tag = typeof(StiLinearTickMarkMajor);
		ciAddLinearTickMarkMajor.Image = (Image)(object)Resources.linear_tick_mark_major;
		ciAddLinearTickMarkMajor.Text = "Linear Tick Mark Major";
		ciAddLinearTickMarkMajor.Click += AddNewGaugeElement_Click;
		ciAddLinearTickMarkMinor.Image = (Image)(object)Resources.linear_tick_mark_minor;
		ciAddLinearTickMarkMinor.Tag = typeof(StiLinearTickMarkMinor);
		ciAddLinearTickMarkMinor.Text = "Linear Tick Mark Minor";
		ciAddLinearTickMarkMinor.Click += AddNewGaugeElement_Click;
		ciAddLinearTickMarkCustom.Image = (Image)(object)Resources.linear_tick_mark_custom;
		ciAddLinearTickMarkCustom.Tag = typeof(StiLinearTickMarkCustom);
		ciAddLinearTickMarkCustom.Text = "Linear Tick Mark Custom";
		ciAddLinearTickMarkCustom.Click += AddNewGaugeElement_Click;
		ciAddLinearTickLabelMajor.Image = (Image)(object)Resources.linear_tick_label_major;
		ciAddLinearTickLabelMajor.Tag = typeof(StiLinearTickLabelMajor);
		ciAddLinearTickLabelMajor.Text = "Linear Tick Label Major";
		ciAddLinearTickLabelMajor.Click += AddNewGaugeElement_Click;
		ciAddLinearTickLabelMinor.Image = (Image)(object)Resources.linear_tick_label_minor;
		ciAddLinearTickLabelMinor.Tag = typeof(StiLinearTickLabelMinor);
		ciAddLinearTickLabelMinor.Text = "Linear Tick Label Minor";
		ciAddLinearTickLabelMinor.Click += AddNewGaugeElement_Click;
		ciAddLinearTickLabelCustom.Image = (Image)(object)Resources.linear_tick_label_custom;
		ciAddLinearTickLabelCustom.Tag = typeof(StiLinearTickLabelCustom);
		ciAddLinearTickLabelCustom.Text = "Linear Tick Label Custom";
		ciAddLinearTickLabelCustom.Click += AddNewGaugeElement_Click;
		ciAddLinearRangeList.BeginGroup = true;
		ciAddLinearRangeList.Image = (Image)(object)Resources.Linear_Range;
		ciAddLinearRangeList.Tag = typeof(StiLinearRangeList);
		ciAddLinearRangeList.Text = "Linear Range List";
		ciAddLinearRangeList.Click += AddNewGaugeElement_Click;
		ciAddLinearBar.BeginGroup = true;
		ciAddLinearBar.Image = (Image)(object)Resources.linear_bar;
		ciAddLinearBar.Tag = typeof(StiLinearBar);
		ciAddLinearBar.Text = "Linear Bar";
		ciAddLinearBar.Click += AddNewGaugeElement_Click;
		ciAddLinearMarker.Image = (Image)(object)Resources.linear_maker;
		ciAddLinearMarker.Tag = typeof(StiLinearMarker);
		ciAddLinearMarker.Text = "Linear Marker";
		ciAddLinearMarker.Click += AddNewGaugeElement_Click;
		ciAddLinearStateIndicator.BeginGroup = true;
		ciAddLinearStateIndicator.Image = (Image)(object)Resources.state_indicator_linear;
		ciAddLinearStateIndicator.Tag = typeof(StiStateIndicator);
		ciAddLinearStateIndicator.Text = "Linear State Indicator";
		ciAddLinearStateIndicator.Click += AddNewGaugeElement_Click;
		ciAddRadialRange.BeginGroup = true;
		ciAddRadialRange.Image = (Image)(object)Resources.radial_range_list;
		ciAddRadialRange.Tag = typeof(StiRadialRange);
		ciAddRadialRange.Text = "Radial Range";
		ciAddRadialRange.Click += AddNewRange_Click;
		ciAddLinearRange.Image = (Image)(object)Resources.linear_range_list;
		ciAddLinearRange.Tag = typeof(StiLinearRange);
		ciAddLinearRange.Text = "Linear Range";
		ciAddLinearRange.Click += AddNewRange_Click;
		ciAddTickCustomValue.BeginGroup = true;
		ciAddTickCustomValue.Image = (Image)(object)Resources.TickCustomValue;
		ciAddTickCustomValue.Text = "Tick Custom Value";
		ciAddTickCustomValue.Click += AddNewTickCustomValue_Click;
		ciAddStateIndicatorFilter.BeginGroup = true;
		ciAddStateIndicatorFilter.Image = (Image)(object)Resources.StateIndicatorFilter;
		ciAddStateIndicatorFilter.Text = "State Indicator Filter";
		ciAddStateIndicatorFilter.Click += AddNewStateIndicatorFilter_Click;
		ciAddBarRangeList.BeginGroup = true;
		ciAddBarRangeList.Image = (Image)(object)Resources.BarRangeList;
		ciAddBarRangeList.Text = "Bar Range List";
		ciAddBarRangeList.Click += AddNewBarRangeList_Click;
		ciRoot.SubItems.AddRange(new BaseItem[7] { ciGroupRadialScale, ciGroupLinearScale, ciAddRadialRange, ciAddLinearRange, ciAddTickCustomValue, ciAddStateIndicatorFilter, ciAddBarRangeList });
		ciGroupRadialScale.Image = StiImageResizer.GetImage(ciGroupRadialScale.Image);
		ciGroupLinearScale.Image = StiImageResizer.GetImage(ciGroupLinearScale.Image);
		ciAddBarRangeList.Image = StiImageResizer.GetImage(ciAddBarRangeList.Image);
		ciAddLinearBar.Image = StiImageResizer.GetImage(ciAddLinearBar.Image);
		ciAddLinearMarker.Image = StiImageResizer.GetImage(ciAddLinearMarker.Image);
		ciAddLinearRange.Image = StiImageResizer.GetImage(ciAddLinearRange.Image);
		ciAddLinearRangeList.Image = StiImageResizer.GetImage(ciAddLinearRangeList.Image);
		ciAddLinearScale.Image = StiImageResizer.GetImage(ciAddLinearScale.Image);
		ciAddLinearStateIndicator.Image = StiImageResizer.GetImage(ciAddLinearStateIndicator.Image);
		ciAddLinearTickLabelCustom.Image = StiImageResizer.GetImage(ciAddLinearTickLabelCustom.Image);
		ciAddLinearTickLabelMajor.Image = StiImageResizer.GetImage(ciAddLinearTickLabelMajor.Image);
		ciAddLinearTickLabelMinor.Image = StiImageResizer.GetImage(ciAddLinearTickLabelMinor.Image);
		ciAddLinearTickMarkCustom.Image = StiImageResizer.GetImage(ciAddLinearTickMarkCustom.Image);
		ciAddLinearTickMarkMajor.Image = StiImageResizer.GetImage(ciAddLinearTickMarkMajor.Image);
		ciAddLinearTickMarkMinor.Image = StiImageResizer.GetImage(ciAddLinearTickMarkMinor.Image);
		ciAddNeedle.Image = StiImageResizer.GetImage(ciAddNeedle.Image);
		ciAddRadialBar.Image = StiImageResizer.GetImage(ciAddRadialBar.Image);
		ciAddRadialMarker.Image = StiImageResizer.GetImage(ciAddRadialMarker.Image);
		ciAddRadialRange.Image = StiImageResizer.GetImage(ciAddRadialRange.Image);
		ciAddRadialRangeList.Image = StiImageResizer.GetImage(ciAddRadialRangeList.Image);
		ciAddRadialScale.Image = StiImageResizer.GetImage(ciAddRadialScale.Image);
		ciAddRadialStateIndicator.Image = StiImageResizer.GetImage(ciAddRadialStateIndicator.Image);
		ciAddRadialTickLabelCustom.Image = StiImageResizer.GetImage(ciAddRadialTickLabelCustom.Image);
		ciAddRadialTickLabelMajor.Image = StiImageResizer.GetImage(ciAddRadialTickLabelMajor.Image);
		ciAddRadialTickLabelMinor.Image = StiImageResizer.GetImage(ciAddRadialTickLabelMinor.Image);
		ciAddRadialTickMarkCustom.Image = StiImageResizer.GetImage(ciAddRadialTickMarkCustom.Image);
		ciAddRadialTickMarkMajor.Image = StiImageResizer.GetImage(ciAddRadialTickMarkMajor.Image);
		ciAddRadialTickMarkMinor.Image = StiImageResizer.GetImage(ciAddRadialTickMarkMinor.Image);
		ciAddStateIndicatorFilter.Image = StiImageResizer.GetImage(ciAddStateIndicatorFilter.Image);
		ciAddTickCustomValue.Image = StiImageResizer.GetImage(ciAddTickCustomValue.Image);
		contextMenuBar1.EndInit();
	}

	private void DoScaling()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		ImageList val = new ImageList();
		val.ImageSize = new Size(StiScale.I16, StiScale.I16);
		val.ColorDepth = (ColorDepth)32;
		foreach (Image image2 in ilButtons.Images)
		{
			Image image = image2;
			val.Images.Add(StiImageResizer.GetImage(image, allowDispose: false));
		}
		tvGauge.ImageList = val;
		miAddNewItem.Image = StiReportDesignImages.Actions.NewGauge();
		btMoveUp.Image = StiReportDesignImages.Actions.MoveUp();
		btMoveDown.Image = StiReportDesignImages.Actions.MoveDown();
		btRemoveItem.Image = StiReportDesignImages.Actions.Remove();
	}

	internal void CheckStatePropertyGrid()
	{
		updateContextMenu = true;
		updateMenu = true;
		if (tvGauge.SelectedNode != null)
		{
			object tag = tvGauge.SelectedNode.Tag;
			((PropertyGrid)propertyGrid).SelectedObject = tag;
			miAddNewItem.Enabled = true;
			if (tvGauge.SelectedNode.Tag is StiGauge)
			{
				btRemoveItem.Enabled = false;
				btMoveUp.Enabled = false;
				btMoveDown.Enabled = false;
			}
			else
			{
				btRemoveItem.Enabled = true;
				CheckStateMoveButtons(tag);
			}
		}
		else
		{
			((PropertyGrid)propertyGrid).SelectedObject = null;
			miAddNewItem.Enabled = false;
			btRemoveItem.Enabled = false;
			btMoveUp.Enabled = false;
			btMoveDown.Enabled = false;
		}
	}

	private void CheckStateMoveButtons(object tag)
	{
		bool enabled = false;
		bool enabled2 = false;
		if (tag is StiScaleBase)
		{
			StiScaleBase stiScaleBase = tag as StiScaleBase;
			if (stiScaleBase.Gauge.Scales.Count > 1)
			{
				int num = stiScaleBase.Gauge.Scales.IndexOf(stiScaleBase);
				if (num != 0)
				{
					enabled = true;
				}
				if (num != stiScaleBase.Gauge.Scales.Count - 1)
				{
					enabled2 = true;
				}
			}
		}
		else if (tag is StiRadialTickMarkCustomValue)
		{
			StiRadialTickMarkCustomValue element = tag as StiRadialTickMarkCustomValue;
			StiRadialTickMarkCustom stiRadialTickMarkCustom = tvGauge.SelectedNode.Parent.Tag as StiRadialTickMarkCustom;
			if (stiRadialTickMarkCustom.Values.Count > 1)
			{
				int num2 = stiRadialTickMarkCustom.Values.IndexOf(element);
				if (num2 != 0)
				{
					enabled = true;
				}
				if (num2 != stiRadialTickMarkCustom.Values.Count - 1)
				{
					enabled2 = true;
				}
			}
		}
		else if (tag is StiLinearTickMarkCustomValue)
		{
			StiLinearTickMarkCustomValue element2 = tag as StiLinearTickMarkCustomValue;
			StiLinearTickMarkCustom stiLinearTickMarkCustom = tvGauge.SelectedNode.Parent.Tag as StiLinearTickMarkCustom;
			if (stiLinearTickMarkCustom.Values.Count > 1)
			{
				int num3 = stiLinearTickMarkCustom.Values.IndexOf(element2);
				if (num3 != 0)
				{
					enabled = true;
				}
				if (num3 != stiLinearTickMarkCustom.Values.Count - 1)
				{
					enabled2 = true;
				}
			}
		}
		else if (tag is StiRadialTickLabelCustomValue)
		{
			StiRadialTickLabelCustomValue element3 = tag as StiRadialTickLabelCustomValue;
			StiRadialTickLabelCustom stiRadialTickLabelCustom = tvGauge.SelectedNode.Parent.Tag as StiRadialTickLabelCustom;
			if (stiRadialTickLabelCustom.Values.Count > 1)
			{
				int num4 = stiRadialTickLabelCustom.Values.IndexOf(element3);
				if (num4 != 0)
				{
					enabled = true;
				}
				if (num4 != stiRadialTickLabelCustom.Values.Count - 1)
				{
					enabled2 = true;
				}
			}
		}
		else if (tag is StiRadialTickLabelCustomValue)
		{
			StiLinearTickLabelCustomValue element4 = tag as StiLinearTickLabelCustomValue;
			StiLinearTickLabelCustom stiLinearTickLabelCustom = tvGauge.SelectedNode.Parent.Tag as StiLinearTickLabelCustom;
			if (stiLinearTickLabelCustom.Values.Count > 1)
			{
				int num5 = stiLinearTickLabelCustom.Values.IndexOf(element4);
				if (num5 != 0)
				{
					enabled = true;
				}
				if (num5 != stiLinearTickLabelCustom.Values.Count - 1)
				{
					enabled2 = true;
				}
			}
		}
		else if (tag is StiStateIndicatorFilter)
		{
			StiStateIndicatorFilter element5 = tag as StiStateIndicatorFilter;
			StiStateIndicator stiStateIndicator = tvGauge.SelectedNode.Parent.Tag as StiStateIndicator;
			if (stiStateIndicator.Filters.Count > 1)
			{
				int num6 = stiStateIndicator.Filters.IndexOf(element5);
				if (num6 != 0)
				{
					enabled = true;
				}
				if (num6 != stiStateIndicator.Filters.Count - 1)
				{
					enabled2 = true;
				}
			}
		}
		else if (tag is StiRangeBase)
		{
			StiRangeBase stiRangeBase = tag as StiRangeBase;
			if (stiRangeBase.RangeList.Ranges.Count > 1)
			{
				int num7 = stiRangeBase.RangeList.Ranges.IndexOf(stiRangeBase);
				if (num7 != 0)
				{
					enabled = true;
				}
				if (num7 != stiRangeBase.RangeList.Ranges.Count - 1)
				{
					enabled2 = true;
				}
			}
		}
		else if (tag is StiGaugeElement)
		{
			StiGaugeElement stiGaugeElement = tag as StiGaugeElement;
			if (stiGaugeElement.Scale.Items.Count > 1)
			{
				int num8 = stiGaugeElement.Scale.Items.IndexOf(stiGaugeElement);
				if (num8 != 0)
				{
					enabled = true;
				}
				if (num8 != stiGaugeElement.Scale.Items.Count - 1)
				{
					enabled2 = true;
				}
			}
		}
		btMoveUp.Enabled = enabled;
		btMoveDown.Enabled = enabled2;
	}

	private void CheckState(bool checkContextMenu)
	{
		stateHelper stateHelper = new stateHelper();
		TreeNode parent = tvGauge.SelectedNode;
		if (parent != null)
		{
			object tag = parent.Tag;
			if (tag is StiCustomValueBase || tag is IStiTickCustom)
			{
				stateHelper.tickCustomValueVisible = true;
				parent = parent.Parent;
			}
			else if (tag is StiIndicatorRangeInfo)
			{
				stateHelper.scaleRangeListVisible = true;
				parent = parent.Parent;
			}
			else if (tag is StiStateIndicatorFilter)
			{
				stateHelper.stateIndicatorFilterVisible = true;
				parent = parent.Parent;
			}
			if (tag is StiLinearScale)
			{
				stateHelper.linearScaleVisible = true;
			}
			else if (tag is StiRadialScale)
			{
				stateHelper.radialScaleVisible = true;
			}
			else if (tag is StiLinearRange || tag is StiLinearRangeList)
			{
				stateHelper.linearScaleVisible = true;
				stateHelper.linearRangeVisible = true;
			}
			else if (tag is StiRadialRange || tag is StiRadialRangeList)
			{
				stateHelper.radialScaleVisible = true;
				stateHelper.radialRangeVisible = true;
			}
			else if (tag is StiGaugeElement stiGaugeElement)
			{
				if (stiGaugeElement.ElementType == StiGaugeElemenType.LinearElement)
				{
					stateHelper.linearScaleVisible = true;
				}
				else if (stiGaugeElement.ElementType == StiGaugeElemenType.RadialElement)
				{
					stateHelper.radialScaleVisible = true;
				}
				else if (parent.Parent.Tag is StiLinearScale)
				{
					stateHelper.linearScaleVisible = true;
				}
				else if (parent.Parent.Tag is StiRadialScale)
				{
					stateHelper.radialScaleVisible = true;
				}
			}
			if (tag is StiBarBase)
			{
				stateHelper.scaleRangeListVisible = true;
			}
			if (tag is StiStateIndicator)
			{
				stateHelper.stateIndicatorFilterVisible = true;
			}
		}
		SetVisibleItems(stateHelper, checkContextMenu);
	}

	private void SetVisibleItems(stateHelper state, bool checkContextMenu)
	{
		if (checkContextMenu)
		{
			if (updateContextMenu)
			{
				updateContextMenu = false;
				ciAddRadialTickMarkMajor.Enabled = state.radialScaleVisible;
				ciAddRadialTickMarkMinor.Enabled = state.radialScaleVisible;
				ciAddRadialTickMarkCustom.Enabled = state.radialScaleVisible;
				ciAddRadialTickLabelMajor.Enabled = state.radialScaleVisible;
				ciAddRadialTickLabelMinor.Enabled = state.radialScaleVisible;
				ciAddRadialTickLabelCustom.Enabled = state.radialScaleVisible;
				ciAddRadialRangeList.Enabled = state.radialScaleVisible;
				ciAddRadialBar.Enabled = state.radialScaleVisible;
				ciAddRadialMarker.Enabled = state.radialScaleVisible;
				ciAddNeedle.Enabled = state.radialScaleVisible;
				ciAddRadialStateIndicator.Enabled = state.radialScaleVisible;
				ciAddRadialRange.Enabled = state.radialRangeVisible;
				ciAddLinearTickMarkMajor.Enabled = state.linearScaleVisible;
				ciAddLinearTickMarkMinor.Enabled = state.linearScaleVisible;
				ciAddLinearTickMarkCustom.Enabled = state.linearScaleVisible;
				ciAddLinearTickLabelMajor.Enabled = state.linearScaleVisible;
				ciAddLinearTickLabelMinor.Enabled = state.linearScaleVisible;
				ciAddLinearTickLabelCustom.Enabled = state.linearScaleVisible;
				ciAddLinearRangeList.Enabled = state.linearScaleVisible;
				ciAddLinearBar.Enabled = state.linearScaleVisible;
				ciAddLinearMarker.Enabled = state.linearScaleVisible;
				ciAddLinearStateIndicator.Enabled = state.linearScaleVisible;
				ciAddLinearRange.Enabled = state.linearRangeVisible;
				ciAddTickCustomValue.Enabled = state.tickCustomValueVisible;
				ciAddStateIndicatorFilter.Enabled = state.stateIndicatorFilterVisible;
				ciAddBarRangeList.Enabled = state.scaleRangeListVisible;
			}
		}
		else if (updateMenu)
		{
			updateMenu = false;
			miAddRadialTickMarkMajor.Enabled = state.radialScaleVisible;
			miAddRadialTickMarkMinor.Enabled = state.radialScaleVisible;
			miAddRadialTickMarkCustom.Enabled = state.radialScaleVisible;
			miAddRadialTickLabelMajor.Enabled = state.radialScaleVisible;
			miAddRadialTickLabelMinor.Enabled = state.radialScaleVisible;
			miAddRadialTickLabelCustom.Enabled = state.radialScaleVisible;
			miAddRadialRangeList.Enabled = state.radialScaleVisible;
			miAddRadialBar.Enabled = state.radialScaleVisible;
			miAddRadialMarker.Enabled = state.radialScaleVisible;
			miAddNeedle.Enabled = state.radialScaleVisible;
			miAddRadialStateIndicator.Enabled = state.radialScaleVisible;
			miAddRadialRange.Enabled = state.radialRangeVisible;
			miAddLinearTickMarkMajor.Enabled = state.linearScaleVisible;
			miAddLinearTickMarkMinor.Enabled = state.linearScaleVisible;
			miAddLinearTickMarkCustom.Enabled = state.linearScaleVisible;
			miAddLinearTickLabelMajor.Enabled = state.linearScaleVisible;
			miAddLinearTickLabelMinor.Enabled = state.linearScaleVisible;
			miAddLinearTickLabelCustom.Enabled = state.linearScaleVisible;
			miAddLinearRangeList.Enabled = state.linearScaleVisible;
			miAddLinearBar.Enabled = state.linearScaleVisible;
			miAddLinearMarker.Enabled = state.linearScaleVisible;
			miAddLinearStateIndicator.Enabled = state.linearScaleVisible;
			miAddLinearRange.Enabled = state.linearRangeVisible;
			miAddTickCustomValue.Enabled = state.tickCustomValueVisible;
			miAddStateIndicatorFilter.Enabled = state.stateIndicatorFilterVisible;
			miAddBarRangeList.Enabled = state.scaleRangeListVisible;
		}
	}

	private void AddNewScale(StiScaleBase scale, Type type)
	{
		gauge.Scales.Add(scale);
		Build();
		SelectedNodeByTag(tvGauge.Nodes, scale);
	}

	private void AddNewGaugeElement(StiGaugeElement element, Type type)
	{
		StiScaleBase stiScaleBase = null;
		if (tvGauge.SelectedNode.Tag is StiScaleBase)
		{
			stiScaleBase = tvGauge.SelectedNode.Tag as StiScaleBase;
		}
		else if (tvGauge.SelectedNode.Parent.Tag is StiScaleBase)
		{
			stiScaleBase = tvGauge.SelectedNode.Parent.Tag as StiScaleBase;
		}
		if (stiScaleBase != null)
		{
			stiScaleBase.Items.Add(element);
			Build();
		}
		SelectedNodeByTag(tvGauge.Nodes, element);
	}

	private void AddNewRange(StiRangeBase rangeBase)
	{
		StiScaleRangeList stiScaleRangeList = null;
		if (tvGauge.SelectedNode.Tag is StiScaleRangeList)
		{
			stiScaleRangeList = tvGauge.SelectedNode.Tag as StiScaleRangeList;
		}
		else if (tvGauge.SelectedNode.Parent.Tag is StiScaleRangeList)
		{
			stiScaleRangeList = tvGauge.SelectedNode.Parent.Tag as StiScaleRangeList;
		}
		if (stiScaleRangeList != null)
		{
			stiScaleRangeList.Ranges.Add(rangeBase);
			Build();
		}
		SelectedNodeByTag(tvGauge.Nodes, rangeBase);
	}

	private void AddNewTickCustomValue()
	{
		StiCustomValueBase stiCustomValueBase = null;
		object tag = tvGauge.SelectedNode.Tag;
		if (tag is StiCustomValueBase || tag is IStiTickCustom)
		{
			if (tag is StiLinearTickLabelCustomValue || tag is StiLinearTickLabelCustom)
			{
				stiCustomValueBase = new StiLinearTickLabelCustomValue();
			}
			else if (tag is StiLinearTickLabelCustomValue || tag is StiLinearTickMarkCustom)
			{
				stiCustomValueBase = new StiLinearTickMarkCustomValue();
			}
			else if (tag is StiRadialTickLabelCustomValue || tag is StiRadialTickLabelCustom)
			{
				stiCustomValueBase = new StiRadialTickLabelCustomValue();
			}
			else if (tag is StiRadialTickMarkCustomValue || tag is StiRadialTickMarkCustom)
			{
				stiCustomValueBase = new StiRadialTickMarkCustomValue();
			}
			if (tag is StiCustomValueBase)
			{
				tag = tvGauge.SelectedNode.Parent.Tag;
			}
			((IStiTickCustom)tag).Values.Add(stiCustomValueBase);
			Build();
			SelectedNodeByTag(tvGauge.Nodes, stiCustomValueBase);
		}
	}

	private void AddNewBarRangeList()
	{
		StiIndicatorRangeInfo stiIndicatorRangeInfo = null;
		object tag = tvGauge.SelectedNode.Tag;
		if (tag is StiBarBase || tag is StiIndicatorRangeInfo)
		{
			stiIndicatorRangeInfo = ((!(tag is StiRadialBar) && !(tag is StiRadialIndicatorRangeInfo)) ? ((StiIndicatorRangeInfo)new StiLinearIndicatorRangeInfo()) : ((StiIndicatorRangeInfo)new StiRadialIndicatorRangeInfo()));
			if (tag is StiIndicatorRangeInfo)
			{
				tag = tvGauge.SelectedNode.Parent.Tag;
			}
			((StiBarBase)tag).RangeList.Add(stiIndicatorRangeInfo);
			Build();
			SelectedNodeByTag(tvGauge.Nodes, stiIndicatorRangeInfo);
		}
	}

	private void AddNewStateIndicatorFilter()
	{
		object tag = tvGauge.SelectedNode.Tag;
		if (tag is StiStateIndicator || tag is StiStateIndicatorFilter)
		{
			if (tag is StiStateIndicatorFilter)
			{
				tag = tvGauge.SelectedNode.Parent.Tag;
			}
			StiStateIndicatorFilter stiStateIndicatorFilter = new StiStateIndicatorFilter();
			((StiStateIndicator)tag).Filters.Add(stiStateIndicatorFilter);
			Build();
			SelectedNodeByTag(tvGauge.Nodes, stiStateIndicatorFilter);
		}
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
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
		//IL_0688: Unknown result type (might be due to invalid IL or missing references)
		//IL_0692: Expected O, but got Unknown
		//IL_069f: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a9: Expected O, but got Unknown
		//IL_07a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b1: Expected O, but got Unknown
		//IL_0829: Unknown result type (might be due to invalid IL or missing references)
		//IL_091d: Unknown result type (might be due to invalid IL or missing references)
		//IL_096a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0974: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiCloudGaugeControl));
		ilButtons = new ImageList(components);
		miAddNewItem = new ButtonItem();
		miEmpty = new LabelItem();
		btRemoveItem = new ButtonItem();
		btMoveUp = new ButtonItem();
		btMoveDown = new ButtonItem();
		tvGauge = new TreeView();
		toolBarPanel = new StiGroupLine();
		stiToolBar1 = new Bar();
		stiGroupLine1 = new StiGroupLine();
		contextMenuBar1 = new ContextMenuBar();
		ciRoot = new ButtonItem();
		((Control)toolBarPanel).SuspendLayout();
		((ISupportInitialize)stiToolBar1).BeginInit();
		((Control)stiGroupLine1).SuspendLayout();
		((ISupportInitialize)contextMenuBar1).BeginInit();
		((Control)this).SuspendLayout();
		ilButtons.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("ilButtons.ImageStream");
		ilButtons.TransparentColor = Color.Magenta;
		ilButtons.Images.SetKeyName(0, "Delete.png");
		ilButtons.Images.SetKeyName(1, "StyleUp.png");
		ilButtons.Images.SetKeyName(2, "StyleDown.png");
		ilButtons.Images.SetKeyName(3, "gauge.png");
		ilButtons.Images.SetKeyName(4, "linearScale.png");
		ilButtons.Images.SetKeyName(5, "radialScale.png");
		ilButtons.Images.SetKeyName(6, "linearTickMarkMajor.png");
		ilButtons.Images.SetKeyName(7, "linearTickMarkMinor.png");
		ilButtons.Images.SetKeyName(8, "linearTickMarkCustom.png");
		ilButtons.Images.SetKeyName(9, "linearTickLabelMajor.png");
		ilButtons.Images.SetKeyName(10, "linearTickLabelMinor.png");
		ilButtons.Images.SetKeyName(11, "linearTickLabelCustom.png");
		ilButtons.Images.SetKeyName(12, "linearRangeList.png");
		ilButtons.Images.SetKeyName(13, "linearBar.png");
		ilButtons.Images.SetKeyName(14, "linearMaker.png");
		ilButtons.Images.SetKeyName(15, "linearStateIndicator.png");
		ilButtons.Images.SetKeyName(16, "radialTickMarkMajor.png");
		ilButtons.Images.SetKeyName(17, "radialTickMarkMinor.png");
		ilButtons.Images.SetKeyName(18, "radialTickMarkCustom.png");
		ilButtons.Images.SetKeyName(19, "radialTickLabelMajor.png");
		ilButtons.Images.SetKeyName(20, "radialTickLabelMinor.png");
		ilButtons.Images.SetKeyName(21, "radialTickLabelCustom.png");
		ilButtons.Images.SetKeyName(22, "radialRangeList.png");
		ilButtons.Images.SetKeyName(23, "radialBar.png");
		ilButtons.Images.SetKeyName(24, "radialMarker.png");
		ilButtons.Images.SetKeyName(25, "needle.png");
		ilButtons.Images.SetKeyName(26, "radialStateIndicator.png");
		ilButtons.Images.SetKeyName(27, "linearRange.png");
		ilButtons.Images.SetKeyName(28, "radialRange.png");
		ilButtons.Images.SetKeyName(29, "barRangeList.png");
		ilButtons.Images.SetKeyName(30, "stateIndicatorFilter.png");
		ilButtons.Images.SetKeyName(31, "tickCustomValue.png");
		ilButtons.Images.SetKeyName(32, "gaugeNew.png");
		miAddNewItem.AutoExpandOnClick = true;
		miAddNewItem.ButtonStyle = eButtonStyle.ImageAndText;
		miAddNewItem.Name = "miAddNewItem";
		miAddNewItem.SubItems.AddRange(new BaseItem[1] { miEmpty });
		miAddNewItem.Text = "Add New Item";
		miAddNewItem.PopupOpen += miAddNewItem_PopupOpen;
		miAddNewItem.PopupShowing += miAddNewItem_PopupShowing;
		miEmpty.BackColor = Color.FromArgb(221, 231, 238);
		miEmpty.BorderSide = eBorderSide.Bottom;
		miEmpty.BorderType = eBorderType.SingleLine;
		miEmpty.ForeColor = Color.FromArgb(0, 21, 110);
		miEmpty.Name = "miEmpty";
		miEmpty.PaddingBottom = 1;
		miEmpty.PaddingLeft = 10;
		miEmpty.PaddingTop = 1;
		miEmpty.SingleLineColor = Color.FromArgb(197, 197, 197);
		miEmpty.Text = "Loading...";
		btRemoveItem.ButtonStyle = eButtonStyle.ImageAndText;
		btRemoveItem.ImageIndex = 0;
		btRemoveItem.Name = "btRemoveItem";
		btRemoveItem.Click += btRemoveItem_Click;
		btMoveUp.BeginGroup = true;
		btMoveUp.ImageIndex = 1;
		btMoveUp.Name = "btMoveUp";
		btMoveUp.Click += btMoveUp_Click;
		btMoveDown.ImageIndex = 2;
		btMoveDown.Name = "btMoveDown";
		btMoveDown.Text = "buttonItem6";
		btMoveDown.Click += btMoveDown_Click;
		tvGauge.BorderStyle = (BorderStyle)0;
		((Control)tvGauge).Dock = (DockStyle)5;
		tvGauge.HideSelection = false;
		tvGauge.ImageIndex = 3;
		tvGauge.ImageList = ilButtons;
		((Control)tvGauge).Location = new Point(0, 0);
		((Control)tvGauge).Name = "tvGauge";
		tvGauge.SelectedImageIndex = 3;
		((Control)tvGauge).Size = new Size(238, 512);
		((Control)tvGauge).TabIndex = 0;
		tvGauge.AfterSelect += new TreeViewEventHandler(tvGauge_AfterSelect);
		((Control)tvGauge).KeyDown += new KeyEventHandler(tvGauge_KeyDown);
		((Control)toolBarPanel).BackColor = Color.White;
		((Control)toolBarPanel).Controls.Add((Control)(object)stiToolBar1);
		((Control)toolBarPanel).Dock = (DockStyle)1;
		toolBarPanel.LinePosition = StiLinePosition.Bottom;
		toolBarPanel.LineStyle = StiLineStyle.Dot;
		((Control)toolBarPanel).Location = new Point(0, 0);
		((Control)toolBarPanel).Name = "toolBarPanel";
		((Control)toolBarPanel).Size = new Size(753, 28);
		((Control)toolBarPanel).TabIndex = 75;
		((Control)stiToolBar1).AccessibleDescription = "stiToolBar1 (stiToolBar1)";
		((Control)stiToolBar1).AccessibleName = "stiToolBar1";
		((Control)stiToolBar1).AccessibleRole = (AccessibleRole)22;
		stiToolBar1.BarType = eBarType.MenuBar;
		((Control)stiToolBar1).Dock = (DockStyle)1;
		stiToolBar1.DockSide = eDockSide.Top;
		stiToolBar1.DockTabStripHeight = 28;
		((Control)stiToolBar1).Font = new Font("Segoe UI", 9f);
		stiToolBar1.Images = ilButtons;
		stiToolBar1.Items.AddRange(new BaseItem[4] { miAddNewItem, btRemoveItem, btMoveUp, btMoveDown });
		stiToolBar1.Location = new Point(0, 0);
		stiToolBar1.Name = "stiToolBar1";
		stiToolBar1.Padding = new Padding(12, 3, 3, 3);
		stiToolBar1.RoundCorners = false;
		stiToolBar1.Size = new Size(753, 25);
		stiToolBar1.Stretch = true;
		stiToolBar1.Style = eDotNetBarStyle.StyleManagerControlled;
		stiToolBar1.TabIndex = 72;
		stiToolBar1.TabStop = false;
		((Control)stiToolBar1).Text = "stiToolBar1";
		((Control)stiGroupLine1).BackColor = Color.FromArgb(250, 250, 250);
		((Control)stiGroupLine1).Controls.Add((Control)(object)tvGauge);
		((Control)stiGroupLine1).Dock = (DockStyle)3;
		stiGroupLine1.LinePosition = StiLinePosition.Right;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 28);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Padding = new Padding(0, 0, 2, 0);
		((Control)stiGroupLine1).Size = new Size(240, 512);
		((Control)stiGroupLine1).TabIndex = 76;
		contextMenuBar1.AntiAlias = true;
		((Control)contextMenuBar1).Font = new Font("Segoe UI", 9f);
		contextMenuBar1.Items.AddRange(new BaseItem[1] { ciRoot });
		contextMenuBar1.Location = new Point(0, 0);
		contextMenuBar1.Name = "contextMenuBar1";
		contextMenuBar1.Size = new Size(75, 25);
		contextMenuBar1.Stretch = true;
		contextMenuBar1.Style = eDotNetBarStyle.Office2007;
		contextMenuBar1.TabIndex = 75;
		contextMenuBar1.TabStop = false;
		((Control)contextMenuBar1).Text = "contextMenuBar1";
		ciRoot.AutoExpandOnClick = true;
		ciRoot.Name = "ciRoot";
		ciRoot.Text = "ContextMenu";
		ciRoot.PopupOpen += ciRoot_PopupOpen;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)stiGroupLine1);
		((Control)this).Controls.Add((Control)(object)toolBarPanel);
		((Control)this).Controls.Add((Control)(object)contextMenuBar1);
		((Control)this).Name = "StiCloudGaugeControl";
		((Control)this).Size = new Size(753, 540);
		((Control)toolBarPanel).ResumeLayout(false);
		((ISupportInitialize)stiToolBar1).EndInit();
		((Control)stiGroupLine1).ResumeLayout(false);
		((ISupportInitialize)contextMenuBar1).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
