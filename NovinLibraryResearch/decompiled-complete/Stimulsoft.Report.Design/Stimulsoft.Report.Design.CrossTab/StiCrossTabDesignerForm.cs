using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.CrossTab.Core;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Design.Inplace;
using Stimulsoft.Report.Design.Panels;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Design.CrossTab;

public class StiCrossTabDesignerForm : StiInplaceToolForm
{
	private int originalCrossTabStyleIndex;

	private StiCrossTab crossTab;

	private StiCrossTab editableCrossTab;

	private Hashtable createdTotals = new Hashtable();

	private List<StiCrossTotal> rowTotals = new List<StiCrossTotal>();

	private List<StiCrossTotal> colTotals = new List<StiCrossTotal>();

	private List<StiCrossSummaryHeader> sumHeaders = new List<StiCrossSummaryHeader>();

	private StiDataBuilder dataBuilder = new StiDataBuilder();

	private IContainer components;

	private StiGroupLine panelFooter;

	private Button buttonOk;

	private Button buttonCancel;

	private TreeView sourceTreeView;

	private ImageList ilData;

	private Panel panelSample;

	private StiTreeView treeViewDataSource;

	private StiCrossTreeView treeViewRows;

	private StiCrossTreeView treeViewColumns;

	private StiCrossTreeView treeViewSummary;

	internal StiCrossPanel panelSampleGrid;

	private ImageList ilRemove;

	private ImageList ilArrows;

	private StiOperationButton buttonSwap;

	private ToolTip toolTip1;

	private ImageList ilDirection;

	private StiOperationButton buttonSummaryDirection;

	private Label labelRows;

	private Label labelColumns;

	private Label labelSummary;

	private ImageList ilDataImages;

	private Button buttonShowMore;

	private StiTreeViewBox treeViewBoxDataSource;

	private Label labelDataSource;

	private Panel panelDataSource;

	private Label labelSelectDataSource;

	private PictureBox pictureBoxSelectDataSource;

	public override bool AllowResize => false;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool ShowMore { get; set; }

	private bool IsEmpty
	{
		get
		{
			if (((TreeView)treeViewColumns).Nodes.Count == 0 && ((TreeView)treeViewRows).Nodes.Count == 0)
			{
				return ((TreeView)treeViewSummary).Nodes.Count == 0;
			}
			return false;
		}
	}

	private bool IsDataNotSelected
	{
		get
		{
			if (SelectedDataSource == null)
			{
				return SelectedBusinessObject == null;
			}
			return false;
		}
	}

	[Browsable(false)]
	internal StiDataSource SelectedDataSource { get; set; }

	[Browsable(false)]
	internal StiBusinessObject SelectedBusinessObject { get; set; }

	private StiDesignerControl Designer => crossTab.Report.Designer.DesignerControl as StiDesignerControl;

	public StiCrossTabDesignerForm(StiCrossTab crossTab)
	{
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			if (StiOptions.Appearance.ShowFormsHelp)
			{
				((Form)this).HelpButton = true;
			}
			this.crossTab = crossTab;
			editableCrossTab = (StiCrossTab)crossTab.Clone();
			originalCrossTabStyleIndex = crossTab.CrossTabStyleIndex;
			Localize();
			DoScaling();
			((Form)this).ShowIcon = false;
			Init();
			StiSettings.Load();
			ShowMore = StiSettings.GetBool(((Control)this).Name, "ShowMore", nullValue: false);
			RefreshStates();
			if (StiOptions.Designer.AllowUseDragDrop)
			{
				((Control)treeViewDataSource).AllowDrop = true;
				((Control)treeViewRows).AllowDrop = true;
				((Control)treeViewColumns).AllowDrop = true;
				((Control)treeViewSummary).AllowDrop = true;
			}
			Designer.SelectedStylesChanged += Designer_SelectedStylesChanged;
		}
	}

	public override void Localize()
	{
		((Control)this).Text = StiLocalization.Get("Components", "StiCrossTab");
		((Control)buttonOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((Control)labelColumns).Text = Loc.GetCleaned("FormCrossTabDesigner", "Columns");
		((Control)labelRows).Text = Loc.GetCleaned("FormCrossTabDesigner", "Rows");
		((Control)labelSummary).Text = Loc.GetCleaned("FormCrossTabDesigner", "Summary");
		((Control)labelDataSource).Text = Loc.GetCleaned("FormCrossTabDesigner", "DataSource");
		((Control)labelSelectDataSource).Text = Loc.GetCleaned("Wizards", "infoDataSource");
		toolTip1.SetToolTip((Control)(object)buttonSwap, StiLocalization.Get("FormCrossTabDesigner", "Swap"));
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/report_internals_crosstable_cross_table_editor.htm");
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		crossTab.DataSourceName = ((SelectedDataSource != null) ? SelectedDataSource.Name : string.Empty);
		crossTab.BusinessObjectGuid = ((SelectedBusinessObject != null) ? SelectedBusinessObject.Guid : string.Empty);
		crossTab.DataRelationName = editableCrossTab.DataRelationName;
		crossTab.Sort = editableCrossTab.Sort;
		crossTab.Filters.Clear();
		crossTab.Filters.AddRange(editableCrossTab.Filters);
		crossTab.FilterMode = editableCrossTab.FilterMode;
		UpdateCrossTab();
		crossTab.SummaryDirection = editableCrossTab.SummaryDirection;
		crossTab.Components.Clear();
		crossTab.CrossTabStyleIndex = editableCrossTab.CrossTabStyleIndex;
		crossTab.Components.AddRange(editableCrossTab.Components);
		((Form)this).Close();
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		crossTab.CrossTabStyleIndex = originalCrossTabStyleIndex;
		((Control)Designer).Refresh();
		((Form)this).Close();
	}

	private void This_FormClosed(object sender, FormClosedEventArgs e)
	{
		Designer.SelectedStylesChanged -= Designer_SelectedStylesChanged;
		StiSettings.Load();
		StiSettings.Set(((Control)this).Name, "ShowMore", ShowMore);
		StiSettings.Save();
	}

	private void RefreshStates()
	{
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		((Control)labelColumns).Visible = !IsDataNotSelected;
		((Control)labelRows).Visible = !IsDataNotSelected;
		((Control)labelSummary).Visible = !IsDataNotSelected;
		((Control)treeViewColumns).Visible = !IsDataNotSelected;
		((Control)treeViewRows).Visible = !IsDataNotSelected;
		((Control)treeViewSummary).Visible = !IsDataNotSelected;
		((Control)buttonSwap).Visible = !IsDataNotSelected;
		((Control)pictureBoxSelectDataSource).Visible = IsDataNotSelected;
		((Control)labelSelectDataSource).Visible = IsDataNotSelected;
		bool showMore = ShowMore;
		((Control)panelSample).Visible = showMore;
		int width = ((Form)this).ClientSize.Width;
		Padding padding = ((Control)this).Padding;
		int top = ((Padding)(ref padding)).Top;
		padding = ((Control)this).Padding;
		((Form)this).ClientSize = new Size(width, top + ((Padding)(ref padding)).Bottom + ((Control)panelFooter).Height + ((Control)panelDataSource).Height + (showMore ? ((Control)panelSample).Height : 0) + StiScale.I4);
		((Control)buttonShowMore).Text = (showMore ? Loc.Get("Buttons", "ShowLess") : Loc.Get("Buttons", "ShowMore"));
	}

	private void RefreshSelectedDataSourceText()
	{
		if (SelectedDataSource != null)
		{
			((Control)treeViewBoxDataSource).Text = SelectedDataSource.ToString();
		}
		else if (SelectedBusinessObject != null)
		{
			((Control)treeViewBoxDataSource).Text = SelectedBusinessObject.ToString();
		}
		else
		{
			((Control)treeViewBoxDataSource).Text = "[" + Loc.Get("Report", "NotAssigned") + "]";
		}
	}

	public void Init()
	{
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Expected O, but got Unknown
		StiCrossTab stiCrossTab = editableCrossTab;
		double left = (editableCrossTab.Top = 0.0);
		stiCrossTab.Left = left;
		SelectedDataSource = editableCrossTab.DataSource;
		if (SelectedDataSource != null)
		{
			BuildColumnsOfDataSource();
		}
		SelectedBusinessObject = editableCrossTab.BusinessObject;
		if (SelectedBusinessObject != null)
		{
			BuildColumnsOfBusinessObject();
		}
		RefreshSelectedDataSourceText();
		foreach (StiComponent component in editableCrossTab.Components)
		{
			if (component is StiCrossSummary || component is StiCrossColumn || component is StiCrossRow)
			{
				TreeNode val = new TreeNode(component.Alias)
				{
					Tag = component
				};
				if (component is StiCrossSummary)
				{
					((TreeView)treeViewSummary).Nodes.Add(val);
				}
				if (component is StiCrossColumn)
				{
					((TreeView)treeViewColumns).Nodes.Add(val);
				}
				if (component is StiCrossRow)
				{
					((TreeView)treeViewRows).Nodes.Add(val);
				}
			}
			if (component is StiCrossSummaryHeader item)
			{
				sumHeaders.Add(item);
			}
		}
		UpdateCrossTab();
	}

	internal void SetNewData(StiDataSource dataSource, StiBusinessObject businessObject)
	{
		SelectedDataSource = dataSource;
		if (dataSource != null)
		{
			BuildColumnsOfDataSource();
		}
		SelectedBusinessObject = businessObject;
		if (businessObject != null)
		{
			BuildColumnsOfBusinessObject();
		}
		((TreeView)treeViewColumns).Nodes.Clear();
		((TreeView)treeViewRows).Nodes.Clear();
		((TreeView)treeViewSummary).Nodes.Clear();
		UpdateCrossTab();
		UpdatePropertyGrid();
		RefreshStates();
	}

	private void DoScaling()
	{
		((TreeView)treeViewColumns).ItemHeight = StiScale.I(24);
		((TreeView)treeViewRows).ItemHeight = StiScale.I(24);
		((TreeView)treeViewSummary).ItemHeight = StiScale.I(24);
		((ButtonBase)buttonSwap).Image = StiReportDesignImages.Actions.SwapCells();
		((ButtonBase)buttonSummaryDirection).Image = StiReportDesignImages.Actions.VerticalPlacement();
		_003F val = pictureBoxSelectDataSource;
		Image obj = StiReportDesignImages.Actions.MoveLeft(StiImageSize.Double);
		((PictureBox)val).Image = (Image)(object)StiImageUtils.ConvertToDisabled((Bitmap)(object)((obj is Bitmap) ? obj : null));
	}

	private string GetColumn()
	{
		return "{" + StiDataBuilder.GetColumnPathFromNode(((TreeView)treeViewDataSource).SelectedNode) + "}";
	}

	private void UpdatePropertyGrid()
	{
		StiComponentsCollection selectedComponents = editableCrossTab.GetSelectedComponents();
		StiComponent[] array = new StiComponent[selectedComponents.Count];
		selectedComponents.CopyTo(array, 0);
		StiDesignerControl designer = Designer;
		object[] selectedObjects = array;
		StiPropertiesPanelHelper.ActivatePanel(designer, selectedObjects);
	}

	private StiCrossRowTotal CreateRowTotal(string rowGuid)
	{
		StiCrossRowTotal stiCrossRowTotal = createdTotals[rowGuid] as StiCrossRowTotal;
		if (stiCrossRowTotal == null)
		{
			stiCrossRowTotal = new StiCrossRowTotal();
			editableCrossTab.ApplyFieldStyle(stiCrossRowTotal);
		}
		createdTotals[rowGuid] = stiCrossRowTotal;
		return stiCrossRowTotal;
	}

	private StiCrossColumnTotal CreateColTotal(string colGuid)
	{
		StiCrossColumnTotal stiCrossColumnTotal = createdTotals[colGuid] as StiCrossColumnTotal;
		if (stiCrossColumnTotal == null)
		{
			stiCrossColumnTotal = new StiCrossColumnTotal();
			editableCrossTab.ApplyFieldStyle(stiCrossColumnTotal);
		}
		createdTotals[colGuid] = stiCrossColumnTotal;
		colTotals.Add(stiCrossColumnTotal);
		return stiCrossColumnTotal;
	}

	internal void UpdateCrossTab()
	{
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Expected O, but got Unknown
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Expected O, but got Unknown
		//IL_0a1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a24: Expected O, but got Unknown
		//IL_0bdd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be4: Expected O, but got Unknown
		//IL_08b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b7: Expected O, but got Unknown
		if (editableCrossTab.Report.Designer != null)
		{
			editableCrossTab.Report.Designer.TextAliasesHash.Clear();
		}
		rowTotals.Clear();
		colTotals.Clear();
		List<StiCrossRow> list = new List<StiCrossRow>();
		List<StiCrossColumn> list2 = new List<StiCrossColumn>();
		Hashtable hashtable = new Hashtable();
		int num = 0;
		foreach (TreeNode node in ((TreeView)treeViewRows).Nodes)
		{
			TreeNode val = node;
			StiCrossRow stiCrossRow = val.Tag as StiCrossRow;
			stiCrossRow.Page = null;
			stiCrossRow.Name = editableCrossTab.Name + "_Row" + (num + 1);
			stiCrossRow.Page = editableCrossTab.Page;
			if (editableCrossTab.Components.Contains(stiCrossRow))
			{
				editableCrossTab.Components.Remove(stiCrossRow);
			}
			editableCrossTab.Components.Add(stiCrossRow);
			hashtable[stiCrossRow] = stiCrossRow;
			list.Add(stiCrossRow);
			StiCrossTotal stiCrossTotal = stiCrossRow.Total;
			if (stiCrossTotal == null)
			{
				stiCrossTotal = CreateRowTotal(stiCrossRow.Guid);
				stiCrossRow.TotalGuid = stiCrossTotal.Guid;
				editableCrossTab.Components.Add(stiCrossTotal);
			}
			hashtable[stiCrossTotal] = stiCrossTotal;
			rowTotals.Add(stiCrossTotal);
			num++;
		}
		int num2 = 0;
		foreach (TreeNode node2 in ((TreeView)treeViewColumns).Nodes)
		{
			TreeNode val2 = node2;
			StiCrossColumn stiCrossColumn = val2.Tag as StiCrossColumn;
			stiCrossColumn.Page = null;
			stiCrossColumn.Name = $"{editableCrossTab}_Column{(num2 + 1).ToString()}";
			stiCrossColumn.Page = editableCrossTab.Page;
			if (editableCrossTab.Components.Contains(stiCrossColumn))
			{
				editableCrossTab.Components.Remove(stiCrossColumn);
			}
			editableCrossTab.Components.Add(stiCrossColumn);
			hashtable[stiCrossColumn] = stiCrossColumn;
			list2.Add(stiCrossColumn);
			StiCrossTotal stiCrossTotal2 = stiCrossColumn.Total;
			if (stiCrossTotal2 == null)
			{
				stiCrossTotal2 = CreateColTotal(stiCrossColumn.Guid);
				stiCrossColumn.TotalGuid = stiCrossTotal2.Guid;
				editableCrossTab.Components.Add(stiCrossTotal2);
			}
			hashtable[stiCrossTotal2] = stiCrossTotal2;
			colTotals.Add(stiCrossTotal2);
			num2++;
		}
		int num3 = 1;
		foreach (TreeNode node3 in ((TreeView)treeViewSummary).Nodes)
		{
			TreeNode val3 = node3;
			StiCrossSummary stiCrossSummary = val3.Tag as StiCrossSummary;
			stiCrossSummary.Page = null;
			stiCrossSummary.Name = editableCrossTab.Name + "_Sum" + num3;
			stiCrossSummary.Page = editableCrossTab.Page;
			hashtable[stiCrossSummary] = stiCrossSummary;
			if (editableCrossTab.Components.Contains(stiCrossSummary))
			{
				editableCrossTab.Components.Remove(stiCrossSummary);
			}
			editableCrossTab.Components.Add(stiCrossSummary);
			num3++;
		}
		num3 = 1;
		if (((TreeView)treeViewSummary).Nodes.Count < 2)
		{
			foreach (StiCrossSummaryHeader sumHeader in sumHeaders)
			{
				if (editableCrossTab.Components.Contains(sumHeader))
				{
					editableCrossTab.Components.Remove(sumHeader);
				}
			}
			sumHeaders.Clear();
		}
		else if (sumHeaders.Count > ((TreeView)treeViewSummary).Nodes.Count)
		{
			while (sumHeaders.Count > ((TreeView)treeViewSummary).Nodes.Count)
			{
				StiCrossSummaryHeader component = sumHeaders[sumHeaders.Count - 1];
				if (editableCrossTab.Components.Contains(component))
				{
					editableCrossTab.Components.Remove(component);
				}
				sumHeaders.RemoveAt(sumHeaders.Count - 1);
			}
		}
		else if (sumHeaders.Count < ((TreeView)treeViewSummary).Nodes.Count)
		{
			while (sumHeaders.Count < ((TreeView)treeViewSummary).Nodes.Count)
			{
				StiCrossSummaryHeader stiCrossSummaryHeader = new StiCrossSummaryHeader
				{
					Text = 
					{
						Value = (sumHeaders.Count + 1).ToString()
					},
					Page = null,
					Name = editableCrossTab.Name + "_SumHeader" + (sumHeaders.Count + 1)
				};
				stiCrossSummaryHeader.Page = editableCrossTab.Page;
				editableCrossTab.ApplyFieldStyle(stiCrossSummaryHeader);
				editableCrossTab.Components.Add(stiCrossSummaryHeader);
				sumHeaders.Add(stiCrossSummaryHeader);
			}
		}
		foreach (StiCrossSummaryHeader sumHeader2 in sumHeaders)
		{
			hashtable[sumHeader2] = sumHeader2;
		}
		int num4 = 1;
		foreach (StiCrossTotal rowTotal in rowTotals)
		{
			rowTotal.Page = null;
			rowTotal.Name = editableCrossTab.Name + "_RowTotal" + num4++;
			rowTotal.Page = editableCrossTab.Page;
		}
		int num5 = 1;
		foreach (StiCrossColumnTotal colTotal in colTotals)
		{
			colTotal.Page = null;
			colTotal.Name = editableCrossTab.Name + "_ColTotal" + num5++;
			colTotal.Page = editableCrossTab.Page;
		}
		bool flag = ((TreeView)treeViewColumns).Nodes.Count > 0 && ((TreeView)treeViewRows).Nodes.Count > 0;
		List<StiCrossTitle> list3 = new List<StiCrossTitle>();
		List<StiCrossTitle> list4 = new List<StiCrossTitle>();
		StiCrossTitle stiCrossTitle = null;
		StiCrossTitle stiCrossTitle2 = null;
		StiCrossTitle stiCrossTitle3 = null;
		foreach (StiCrossField component2 in editableCrossTab.Components)
		{
			if (component2 is StiCrossTitle)
			{
				StiCrossTitle stiCrossTitle4 = component2 as StiCrossTitle;
				if (stiCrossTitle4.TypeOfComponent.StartsWithInvariant("Col:"))
				{
					list3.Add(stiCrossTitle4);
				}
				if (stiCrossTitle4.TypeOfComponent.StartsWithInvariant("Row:"))
				{
					list4.Add(stiCrossTitle4);
				}
				if (stiCrossTitle4.TypeOfComponent.StartsWithInvariant("LeftTitle"))
				{
					stiCrossTitle = stiCrossTitle4;
				}
				if (stiCrossTitle4.TypeOfComponent.StartsWithInvariant("RightTitle"))
				{
					stiCrossTitle2 = stiCrossTitle4;
				}
				if (stiCrossTitle4.TypeOfComponent.StartsWithInvariant("SummaryTitle"))
				{
					stiCrossTitle3 = stiCrossTitle4;
				}
			}
		}
		if (((TreeView)treeViewRows).Nodes.Count == 0)
		{
			foreach (StiCrossColumn item in list2)
			{
				string text = "Col:" + item.Name;
				bool flag2 = false;
				foreach (StiCrossTitle item2 in list3)
				{
					if (item2.TypeOfComponent == text)
					{
						list3.Remove(item2);
						flag2 = true;
						hashtable[item2] = item2;
						break;
					}
				}
				if (flag2)
				{
					continue;
				}
				string value = string.Empty;
				foreach (TreeNode node4 in ((TreeView)treeViewColumns).Nodes)
				{
					TreeNode val4 = node4;
					if (val4.Tag == item)
					{
						value = val4.Text;
					}
				}
				StiCrossTitle stiCrossTitle5 = new StiCrossTitle
				{
					Name = item.Name + "_Title",
					Text = 
					{
						Value = value
					},
					TypeOfComponent = "Col:" + item.Name
				};
				editableCrossTab.ApplyFieldStyle(stiCrossTitle5);
				editableCrossTab.Components.Add(stiCrossTitle5);
				hashtable[stiCrossTitle5] = stiCrossTitle5;
			}
		}
		foreach (StiCrossRow item3 in list)
		{
			string text2 = "Row:" + item3.Name;
			bool flag3 = false;
			foreach (StiCrossTitle item4 in list4)
			{
				if (item4.TypeOfComponent == text2)
				{
					list4.Remove(item4);
					flag3 = true;
					hashtable[item4] = item4;
					break;
				}
			}
			if (flag3)
			{
				continue;
			}
			string value2 = string.Empty;
			foreach (TreeNode node5 in ((TreeView)treeViewRows).Nodes)
			{
				TreeNode val5 = node5;
				if (val5.Tag == item3)
				{
					value2 = val5.Text;
				}
			}
			StiCrossTitle stiCrossTitle6 = new StiCrossTitle();
			stiCrossTitle6.Name = item3.Name + "_Title";
			stiCrossTitle6.Text.Value = value2;
			stiCrossTitle6.TypeOfComponent = "Row:" + item3.Name;
			editableCrossTab.ApplyFieldStyle(stiCrossTitle6);
			editableCrossTab.Components.Add(stiCrossTitle6);
			hashtable[stiCrossTitle6] = stiCrossTitle6;
		}
		if (stiCrossTitle == null)
		{
			if (flag)
			{
				stiCrossTitle = new StiCrossTitle
				{
					Name = editableCrossTab.Name + "_LeftTitle"
				};
				if (SelectedDataSource != null)
				{
					stiCrossTitle.Text.Value = SelectedDataSource.Name;
				}
				else if (SelectedBusinessObject != null)
				{
					stiCrossTitle.Text.Value = SelectedBusinessObject.Name;
				}
				else
				{
					stiCrossTitle.Text.Value = string.Empty;
				}
				stiCrossTitle.TypeOfComponent = "LeftTitle";
				editableCrossTab.ApplyFieldStyle(stiCrossTitle);
				editableCrossTab.Components.Add(stiCrossTitle);
				hashtable[stiCrossTitle] = stiCrossTitle;
			}
		}
		else if (flag)
		{
			hashtable[stiCrossTitle] = stiCrossTitle;
		}
		if (stiCrossTitle2 == null)
		{
			if (flag)
			{
				string text3 = string.Empty;
				bool flag4 = true;
				foreach (TreeNode node6 in ((TreeView)treeViewColumns).Nodes)
				{
					TreeNode val6 = node6;
					if (!flag4)
					{
						text3 += ", ";
					}
					text3 += val6.Text;
					flag4 = false;
				}
				stiCrossTitle2 = new StiCrossTitle();
				stiCrossTitle2.Name = editableCrossTab.Name + "_RightTitle";
				stiCrossTitle2.Text.Value = text3;
				stiCrossTitle2.TypeOfComponent = "RightTitle";
				editableCrossTab.ApplyFieldStyle(stiCrossTitle2);
				editableCrossTab.Components.Add(stiCrossTitle2);
				hashtable[stiCrossTitle2] = stiCrossTitle2;
			}
		}
		else if (flag)
		{
			hashtable[stiCrossTitle2] = stiCrossTitle2;
		}
		bool flag5 = (((TreeView)treeViewColumns).Nodes.Count == 0 || ((TreeView)treeViewRows).Nodes.Count == 0) && ((TreeView)treeViewSummary).Nodes.Count > 0;
		string text4 = string.Join(", ", (from TreeNode s in (IEnumerable)((TreeView)treeViewSummary).Nodes
			select s.Text).ToArray());
		if (stiCrossTitle3 == null)
		{
			if (flag5)
			{
				stiCrossTitle3 = new StiCrossTitle();
				stiCrossTitle3.Name = editableCrossTab.Name + "_SummaryTitle";
				stiCrossTitle3.Text.Value = text4;
				stiCrossTitle3.TypeOfComponent = "SummaryTitle";
				editableCrossTab.ApplyFieldStyle(stiCrossTitle3);
				editableCrossTab.Components.Add(stiCrossTitle3);
				hashtable[stiCrossTitle3] = stiCrossTitle3;
			}
		}
		else if (flag5)
		{
			if (string.IsNullOrEmpty(stiCrossTitle3.Text.Value) || stiCrossTitle3.Text.Value.StartsWith(text4) || text4.StartsWith(stiCrossTitle3.Text.Value))
			{
				stiCrossTitle3.Text.Value = text4;
			}
			hashtable[stiCrossTitle3] = stiCrossTitle3;
		}
		int num6 = 0;
		while (num6 < editableCrossTab.Components.Count)
		{
			StiComponent key = editableCrossTab.Components[num6];
			if (hashtable[key] == null)
			{
				editableCrossTab.Components.RemoveAt(num6);
			}
			else
			{
				num6++;
			}
		}
		RectangleM empty = RectangleM.Empty;
		StiCrossTabHelper.BuildCross(editableCrossTab, designTime: true);
		foreach (StiCrossField component3 in editableCrossTab.Components)
		{
			if (editableCrossTab.CrossTabInfo.Cross.Fields[component3] is Point)
			{
				Point point = (Point)editableCrossTab.CrossTabInfo.Cross.Fields[component3];
				editableCrossTab.CrossTabInfo.Cross.SetTextOfCell(point.X, point.Y, component3.CellText);
			}
		}
		editableCrossTab.CrossTabInfo.Cross.MaxWidth = (decimal)editableCrossTab.Page.Width;
		editableCrossTab.CrossTabInfo.Cross.MaxHeight = (decimal)editableCrossTab.Page.Height;
		editableCrossTab.CrossTabInfo.Cross.DoAutoSize();
		Hashtable hashtable2 = new Hashtable();
		foreach (StiCrossField component4 in editableCrossTab.Components)
		{
			if (component4 is StiCrossHeader { IsTotalVisible: false } stiCrossHeader)
			{
				hashtable2[stiCrossHeader.Total] = stiCrossHeader.Total;
			}
		}
		decimal num7 = editableCrossTab.Report.Unit.ConvertFromHInches(2m);
		decimal num8 = editableCrossTab.Report.Unit.ConvertFromHInches(2m);
		foreach (StiCrossField component5 in editableCrossTab.Components)
		{
			if (!(editableCrossTab.CrossTabInfo.Cross.Fields[component5] is Point))
			{
				continue;
			}
			Point point2 = (Point)editableCrossTab.CrossTabInfo.Cross.Fields[component5];
			RectangleM rect = editableCrossTab.Report.Unit.ConvertToHInches(StiCrossTabHelper.GetCellRect(editableCrossTab, point2.X, point2.Y));
			empty.Width = Math.Max(empty.Width, rect.Right);
			empty.Height = Math.Max(empty.Height, rect.Bottom);
			rect = editableCrossTab.Report.Unit.ConvertFromHInches(rect);
			if (hashtable2[component5] != null)
			{
				rect.Width = 0m;
				rect.Height = 0m;
			}
			decimal num9 = default(decimal);
			decimal num10 = default(decimal);
			if (component5 is StiCrossTitle stiCrossTitle7)
			{
				if (stiCrossTitle7.TypeOfComponent.StartsWithInvariant("LeftTitle"))
				{
					num9 = default(decimal);
					num10 = default(decimal);
				}
				if (stiCrossTitle7.TypeOfComponent.StartsWithInvariant("RightTitle"))
				{
					num9 = num7;
					num10 = default(decimal);
				}
				if (stiCrossTitle7.TypeOfComponent.StartsWithInvariant("SummaryTitle"))
				{
					if (((TreeView)treeViewRows).Nodes.Count == 0)
					{
						num9 = default(decimal);
						num10 = num8 * 2m;
					}
					if (((TreeView)treeViewColumns).Nodes.Count == 0)
					{
						num9 = num7;
						num10 = num8;
					}
				}
				if (stiCrossTitle7.TypeOfComponent.StartsWithInvariant("Row:"))
				{
					num9 = default(decimal);
					num10 = num8;
				}
				if (stiCrossTitle7.TypeOfComponent.StartsWithInvariant("Col:"))
				{
					num9 = default(decimal);
					num10 = num8;
				}
			}
			if (component5 is StiCrossRow)
			{
				num9 = default(decimal);
				num10 = num8 * 2m;
			}
			if (component5 is StiCrossRowTotal)
			{
				num9 = default(decimal);
				num10 = num8 * 2m;
			}
			if (component5 is StiCrossColumn)
			{
				num9 = num7;
				num10 = num8;
			}
			if (component5 is StiCrossColumnTotal)
			{
				num9 = num7;
				num10 = num8;
			}
			if (component5 is StiCrossSummary)
			{
				num9 = num7;
				num10 = num8 * 2m;
			}
			if (component5 is StiCrossSummaryHeader)
			{
				if (editableCrossTab.SummaryDirection == StiSummaryDirection.LeftToRight)
				{
					num9 = num7;
					num10 = num8;
				}
				else
				{
					num9 = default(decimal);
					num10 = num8 * 2m;
				}
			}
			rect.X += num9;
			rect.Y += num10;
			component5.ClientRectangle = rect.ToRectangleD();
		}
		((Control)buttonSummaryDirection).Visible = ((TreeView)treeViewSummary).Nodes.Count > 1;
		((ButtonBase)buttonSummaryDirection).Image = ((editableCrossTab.SummaryDirection == StiSummaryDirection.LeftToRight) ? StiReportDesignImages.Actions.HorizontalPlacement() : StiReportDesignImages.Actions.VerticalPlacement());
		editableCrossTab.UpdateStyles();
		((Control)panelSampleGrid).Invalidate();
	}

	private void KillRightTitle()
	{
		foreach (StiComponent component in editableCrossTab.Components)
		{
			if (component is StiCrossTitle stiCrossTitle && stiCrossTitle.TypeOfComponent.StartsWith("RightTitle", StringComparison.InvariantCulture))
			{
				editableCrossTab.Components.Remove(component);
				break;
			}
		}
	}

	private void BuildColumnsOfDataSource()
	{
		((TreeView)treeViewDataSource).Nodes.Clear();
		((TreeView)treeViewDataSource).ImageList = StiDataBuilder.Images;
		TreeNodeEx dataNode = new TreeNodeEx();
		dataBuilder.BuildDataSource(SelectedDataSource, ref dataNode, includeCalcColumn: true);
		((TreeView)treeViewDataSource).Nodes.Add((TreeNode)(object)dataNode);
		((TreeNode)dataNode).Expand();
	}

	private void BuildColumnsOfBusinessObject()
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		((TreeView)treeViewDataSource).Nodes.Clear();
		((TreeView)treeViewDataSource).ImageList = StiDataBuilder.Images;
		TreeNode val = new TreeNode(SelectedBusinessObject.ToString());
		val.Tag = SelectedBusinessObject;
		string imageKey = (val.StateImageKey = "BusinessObject");
		val.ImageKey = imageKey;
		dataBuilder.BuildBusinessObject(SelectedBusinessObject, val, includeColumns: true, includeCalcColumns: true);
		((TreeView)treeViewDataSource).Nodes.Add(val);
		val.Expand();
	}

	private StiCrossHeader CopySummaryToRow(StiCrossSummary field)
	{
		StiCrossRow stiCrossRow = new StiCrossRow();
		CopyFieldToField(stiCrossRow, field);
		StiDisplayCrossValueExpression displayValue = stiCrossRow.DisplayValue;
		string value2 = (stiCrossRow.Value.Value = field.Value.Value);
		displayValue.Value = value2;
		return stiCrossRow;
	}

	private StiCrossHeader CopySummaryToColumn(StiCrossSummary field)
	{
		StiCrossColumn stiCrossColumn = new StiCrossColumn();
		CopyFieldToField(stiCrossColumn, field);
		StiDisplayCrossValueExpression displayValue = stiCrossColumn.DisplayValue;
		string value2 = (stiCrossColumn.Value.Value = field.Value.Value);
		displayValue.Value = value2;
		return stiCrossColumn;
	}

	private StiCrossSummary CopyHeaderToSummary(StiCrossHeader header)
	{
		StiCrossSummary stiCrossSummary = new StiCrossSummary();
		CopyFieldToField(stiCrossSummary, header);
		stiCrossSummary.Value.Value = header.Value.Value;
		return stiCrossSummary;
	}

	private StiCrossColumn CopyRowToColumn(StiCrossRow row)
	{
		StiCrossColumn stiCrossColumn = new StiCrossColumn();
		CopyFieldToField(stiCrossColumn, row);
		stiCrossColumn.Value.Value = row.Value.Value;
		stiCrossColumn.DisplayValue.Value = row.DisplayValue.Value;
		return stiCrossColumn;
	}

	private StiCrossRow CopyColumnToRow(StiCrossColumn column)
	{
		StiCrossRow stiCrossRow = new StiCrossRow();
		CopyFieldToField(stiCrossRow, column);
		stiCrossRow.Value.Value = column.Value.Value;
		stiCrossRow.DisplayValue.Value = column.DisplayValue.Value;
		return stiCrossRow;
	}

	private StiCrossColumnTotal CopyRowTotalToColumnTotal(StiCrossRowTotal row)
	{
		StiCrossColumnTotal stiCrossColumnTotal = new StiCrossColumnTotal();
		CopyFieldToField(stiCrossColumnTotal, row);
		stiCrossColumnTotal.Text.Value = row.Text.Value;
		return stiCrossColumnTotal;
	}

	private StiCrossRowTotal CopyColumnTotalToRowTotal(StiCrossColumnTotal column)
	{
		StiCrossRowTotal stiCrossRowTotal = new StiCrossRowTotal();
		CopyFieldToField(stiCrossRowTotal, column);
		stiCrossRowTotal.Text.Value = column.Text.Value;
		return stiCrossRowTotal;
	}

	private void CopyFieldToField(StiCrossField dest, StiCrossField source)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		dest.Name = source.Name;
		dest.Alias = source.Alias;
		dest.Guid = source.Guid;
		dest.Border = (StiBorder)source.Border.Clone();
		dest.Brush = (StiBrush)source.Brush.Clone();
		dest.Font = (Font)source.Font.Clone();
		dest.TextFormat = (StiFormatService)source.TextFormat.Clone();
		dest.TextBrush = (StiBrush)source.TextBrush.Clone();
		dest.TextOptions = (StiTextOptions)source.TextOptions.Clone();
		if (source is StiCrossHeader && dest is StiCrossHeader)
		{
			((StiCrossHeader)dest).ShowTotal = ((StiCrossHeader)source).ShowTotal;
			((StiCrossHeader)dest).SortDirection = ((StiCrossHeader)source).SortDirection;
			((StiCrossHeader)dest).SortType = ((StiCrossHeader)source).SortType;
			((StiCrossHeader)dest).PrintOnAllPages = ((StiCrossHeader)source).PrintOnAllPages;
		}
		dest.HorAlignment = source.HorAlignment;
		dest.VertAlignment = source.VertAlignment;
	}

	private void ConvertColumnTotal(StiCrossColumn sourceColumn, StiCrossRow destRow)
	{
		if (sourceColumn.TotalGuid != null)
		{
			StiCrossColumnTotal stiCrossColumnTotal = sourceColumn.Total as StiCrossColumnTotal;
			editableCrossTab.Components.Remove(stiCrossColumnTotal);
			StiCrossRowTotal stiCrossRowTotal = CopyColumnTotalToRowTotal(stiCrossColumnTotal);
			editableCrossTab.Components.Add(stiCrossRowTotal);
			destRow.TotalGuid = stiCrossRowTotal.Guid;
		}
	}

	private void ConvertRowTotal(StiCrossRow sourceRow, StiCrossColumn destColumn)
	{
		if (sourceRow.TotalGuid != null)
		{
			StiCrossRowTotal stiCrossRowTotal = sourceRow.Total as StiCrossRowTotal;
			editableCrossTab.Components.Remove(stiCrossRowTotal);
			StiCrossColumnTotal stiCrossColumnTotal = CopyRowTotalToColumnTotal(stiCrossRowTotal);
			editableCrossTab.Components.Add(stiCrossColumnTotal);
			destColumn.TotalGuid = stiCrossColumnTotal.Guid;
		}
	}

	private void CheckSelectedNode(TreeNodeCollection nodes, StiTreeView view)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		foreach (TreeNode node in nodes)
		{
			TreeNode val = node;
			if (val.Tag == SelectedBusinessObject)
			{
				((TreeView)view).SelectedNode = val;
			}
			if (!val.IsExpanded)
			{
				val.Expand();
			}
			CheckSelectedNode(val.Nodes, view);
		}
	}

	private void SetImageIndex(TreeNodeCollection nodes, int imageIndex)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		foreach (TreeNode node in nodes)
		{
			TreeNode val = node;
			int imageIndex2 = (val.SelectedImageIndex = imageIndex);
			val.ImageIndex = imageIndex2;
			SetImageIndex(val.Nodes, imageIndex);
		}
	}

	private void SelectFieldNode(StiComponent comp)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected O, but got Unknown
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Expected O, but got Unknown
		foreach (TreeNode node in ((TreeView)treeViewColumns).Nodes)
		{
			TreeNode val = node;
			if (val.Tag == comp)
			{
				ResetAllNodesSelection();
				((TreeView)treeViewColumns).SelectedNode = val;
				((Control)treeViewColumns).Focus();
				return;
			}
		}
		foreach (TreeNode node2 in ((TreeView)treeViewRows).Nodes)
		{
			TreeNode val2 = node2;
			if (val2.Tag == comp)
			{
				ResetAllNodesSelection();
				((TreeView)treeViewRows).SelectedNode = val2;
				((Control)treeViewRows).Focus();
				return;
			}
		}
		foreach (TreeNode node3 in ((TreeView)treeViewSummary).Nodes)
		{
			TreeNode val3 = node3;
			if (val3.Tag == comp)
			{
				ResetAllNodesSelection();
				((TreeView)treeViewSummary).SelectedNode = val3;
				((Control)treeViewSummary).Focus();
				break;
			}
		}
	}

	private void ResetAllNodesSelection()
	{
		((TreeView)treeViewColumns).SelectedNode = null;
		((TreeView)treeViewRows).SelectedNode = null;
		((TreeView)treeViewSummary).SelectedNode = null;
	}

	private void OrderRows()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		int num = 1;
		foreach (TreeNode node in ((TreeView)treeViewRows).Nodes)
		{
			TreeNode val = node;
			StiCrossRow stiCrossRow = val.Tag as StiCrossRow;
			StiCrossTitle crossRowTitle = stiCrossRow.GetCrossRowTitle();
			StiCrossRowTotal crossRowTotal = stiCrossRow.GetCrossRowTotal();
			stiCrossRow.Name = $"{editableCrossTab.Name}_Row{num}";
			if (crossRowTitle != null)
			{
				crossRowTitle.Name = stiCrossRow.Name + "_Title";
				crossRowTitle.TypeOfComponent = "Row:" + stiCrossRow.Name;
			}
			if (crossRowTotal != null)
			{
				crossRowTotal.Name = $"{editableCrossTab.Name}_RowTotal{num}";
			}
			num++;
		}
	}

	private void ButtonShowMore_Click(object sender, EventArgs e)
	{
		ShowMore = !ShowMore;
		RefreshStates();
	}

	private void TreeViewDataSource_BeforeExpand(object sender, TreeViewCancelEventArgs e)
	{
		if (e.Node is TreeNodeEx node && e.Node.Tag != null)
		{
			dataBuilder.ExpandedNode(node, includeCalcColumn: true);
		}
	}

	private void TreeView_DragEnter(object sender, DragEventArgs e)
	{
		if (sender is StiCrossTreeView)
		{
			((StiCrossTreeView)sender).IsDragOverFromDictionary = true;
		}
	}

	private void TreeView_DragOver(object sender, DragEventArgs e)
	{
		object data = e.Data.GetData(typeof(TreeNode));
		TreeNode val = (TreeNode)((data is TreeNode) ? data : null);
		if (val == null)
		{
			return;
		}
		e.Effect = (DragDropEffects)2;
		TreeView val2 = (TreeView)((sender is TreeView) ? sender : null);
		if (val2 != treeViewDataSource)
		{
			val = val2.GetNodeAt(((Control)val2).PointToClient(Cursor.Position));
			if (val != null)
			{
				val2.SelectedNode = val;
			}
		}
	}

	private void TreeView_DragDrop(object sender, DragEventArgs e)
	{
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Expected O, but got Unknown
		TreeView val = (TreeView)((sender is TreeView) ? sender : null);
		if (sender is StiCrossTreeView)
		{
			((StiCrossTreeView)sender).IsDragOverFromDictionary = false;
		}
		bool flag = false;
		object data = e.Data.GetData(typeof(TreeNode));
		TreeNode val2 = (TreeNode)((data is TreeNode) ? data : null);
		if (val2 == null)
		{
			return;
		}
		StiCrossField stiCrossField = val2.Tag as StiCrossField;
		if (stiCrossField == null)
		{
			flag = true;
			if (val == treeViewColumns)
			{
				stiCrossField = new StiCrossColumn();
				stiCrossField.Alias = val2.Text;
				StiDisplayCrossValueExpression displayValue = ((StiCrossHeader)stiCrossField).DisplayValue;
				string value = (((StiCrossHeader)stiCrossField).Value.Value = GetColumn());
				displayValue.Value = value;
			}
			else if (val == treeViewRows)
			{
				stiCrossField = new StiCrossRow();
				stiCrossField.Alias = val2.Text;
				StiDisplayCrossValueExpression displayValue2 = ((StiCrossHeader)stiCrossField).DisplayValue;
				string value = (((StiCrossHeader)stiCrossField).Value.Value = GetColumn());
				displayValue2.Value = value;
			}
			else if (val == treeViewSummary)
			{
				stiCrossField = new StiCrossSummary();
				stiCrossField.Alias = val2.Text;
				((StiCrossSummary)stiCrossField).Value.Value = GetColumn();
			}
		}
		editableCrossTab.ApplyFieldStyle(stiCrossField);
		if (sourceTreeView != null && sourceTreeView != treeViewDataSource)
		{
			sourceTreeView.Nodes.Remove(val2);
		}
		if (val != treeViewDataSource)
		{
			if (!flag && val != sourceTreeView)
			{
				if (stiCrossField is StiCrossSummary && val == treeViewRows)
				{
					stiCrossField = CopySummaryToRow(stiCrossField as StiCrossSummary);
				}
				else if (stiCrossField is StiCrossSummary && val == treeViewColumns)
				{
					stiCrossField = CopySummaryToColumn(stiCrossField as StiCrossSummary);
				}
				else if (stiCrossField is StiCrossHeader)
				{
					if (val == treeViewSummary)
					{
						stiCrossField = CopyHeaderToSummary(stiCrossField as StiCrossHeader);
					}
					if (val == treeViewRows)
					{
						StiCrossColumn stiCrossColumn = stiCrossField as StiCrossColumn;
						StiCrossRow stiCrossRow = CopyColumnToRow(stiCrossColumn);
						ConvertColumnTotal(stiCrossColumn, stiCrossRow);
						stiCrossField = stiCrossRow;
					}
					if (val == treeViewColumns)
					{
						StiCrossRow stiCrossRow2 = stiCrossField as StiCrossRow;
						StiCrossColumn stiCrossColumn2 = CopyRowToColumn(stiCrossRow2);
						ConvertRowTotal(stiCrossRow2, stiCrossColumn2);
						stiCrossField = stiCrossColumn2;
					}
				}
			}
			editableCrossTab.ResetSelection();
			stiCrossField.IsSelected = true;
			UpdatePropertyGrid();
			TreeNode nodeAt = val.GetNodeAt(((Control)val).PointToClient(Cursor.Position));
			TreeNode val3 = new TreeNode(val2.Text);
			val3.Tag = stiCrossField;
			if (nodeAt != null)
			{
				val.Nodes.Insert(val.Nodes.IndexOf(nodeAt), val3);
			}
			else
			{
				val.Nodes.Add(val3);
			}
			val.SelectedNode = val3;
			((Control)val).Focus();
			KillRightTitle();
		}
		UpdateCrossTab();
		if (val.SelectedNode != null)
		{
			StiPropertiesPanelHelper.ActivatePanel(Designer, val.SelectedNode.Tag);
		}
	}

	private void TreeView_MouseDown(object sender, MouseEventArgs e)
	{
		TreeView val = (TreeView)((sender is TreeView) ? sender : null);
		TreeNode nodeAt = val.GetNodeAt(((Control)val).PointToClient(Cursor.Position));
		if (nodeAt != null)
		{
			val.SelectedNode = nodeAt;
			if (val != treeViewDataSource)
			{
				editableCrossTab.ResetSelection();
				((StiCrossField)val.SelectedNode.Tag).IsSelected = true;
				UpdatePropertyGrid();
			}
		}
	}

	private void TreeView_DragLeave(object sender, EventArgs e)
	{
		if (sender is StiCrossTreeView)
		{
			((StiCrossTreeView)sender).IsDragOverFromDictionary = false;
		}
	}

	private void TreeView_ItemDrag(object sender, ItemDragEventArgs e)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		TreeView val = (TreeView)((sender is TreeView) ? sender : null);
		object item = e.Item;
		TreeNode val2 = (TreeNode)((item is TreeNode) ? item : null);
		if (val2 != null && val2.Tag != null && (val != treeViewDataSource || ((TreeView)treeViewDataSource).SelectedNode.Tag is StiDataColumn))
		{
			sourceTreeView = val;
			((Control)val).DoDragDrop((object)val2, (DragDropEffects)2);
		}
	}

	private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
	{
		TreeView val = (TreeView)((sender is TreeView) ? sender : null);
		if (val.SelectedNode != null)
		{
			editableCrossTab.ResetSelection();
			StiPropertiesPanelHelper.ActivatePanel(Designer, val.SelectedNode.Tag);
			StiComponent stiComponent = val.SelectedNode.Tag as StiComponent;
			stiComponent.IsSelected = true;
			((Control)panelSampleGrid).Invalidate();
		}
	}

	private void TreeViewRows_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 46)
		{
			e.Handled = true;
			ButtonRemoveRow_Click(null, EventArgs.Empty);
		}
	}

	private void TreeViewColumns_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 46)
		{
			e.Handled = true;
			ButtonRemoveColumn_Click(null, EventArgs.Empty);
		}
	}

	private void TeeeViewSummary_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 46)
		{
			e.Handled = true;
			ButtonRemoveSummary_Click(null, EventArgs.Empty);
		}
	}

	private void TreeViewBoxDataSource_TreeNodesFill(object sender, StiTreeNodesFillEventArgs e)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Expected O, but got Unknown
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Expected O, but got Unknown
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Expected O, but got Unknown
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Expected O, but got Unknown
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Expected O, but got Unknown
		StiTreeView treeView = e.TreeView;
		((TreeView)treeView).BeforeExpand += new TreeViewCancelEventHandler(TreeViewBoxDataSourceTreeNode_BeforeExpand);
		((TreeView)treeView).Nodes.Clear();
		((TreeView)treeView).Nodes.Add(new TreeNode("[" + Loc.Get("Report", "NotAssigned") + "]")
		{
			ImageKey = "Close",
			SelectedImageKey = "Close"
		});
		((TreeView)treeView).SelectedNode = ((TreeView)treeView).Nodes[0];
		((TreeView)treeView).ImageList = StiDataBuilder.Images;
		TreeNode val = new TreeNode(StiLocalization.Get("PropertyMain", "DataSources"))
		{
			ImageKey = "DataSource",
			SelectedImageKey = "DataSource"
		};
		((TreeView)treeView).Nodes.Add(val);
		StiDataSourcesCollection dataSources = editableCrossTab.Report.Dictionary.DataSources;
		foreach (StiDataSource item in dataSources)
		{
			TreeNode val2 = new TreeNode(item.ToString())
			{
				Tag = item,
				ImageKey = "DataSource",
				SelectedImageKey = "DataSource"
			};
			StiDataAdapterService dataAdapter = StiDataAdapterService.GetDataAdapter(item);
			string categoryName = item.GetCategoryName();
			TreeNode val3 = null;
			foreach (TreeNode node in val.Nodes)
			{
				TreeNode val4 = node;
				if (val4.Text == categoryName)
				{
					val3 = val4;
					break;
				}
			}
			if (val3 == null)
			{
				val3 = new TreeNode(categoryName)
				{
					ImageKey = "Database",
					SelectedImageKey = "Database"
				};
				val.Nodes.Add(val3);
			}
			val3.Nodes.Add(val2);
			if (item == SelectedDataSource)
			{
				((TreeView)treeView).SelectedNode = val2;
			}
		}
		if (editableCrossTab.Report.Dictionary.BusinessObjects.Count > 0)
		{
			TreeNode val5 = new TreeNode(StiLocalization.Get("Report", "BusinessObjects"), 145, 145);
			dataBuilder.BuildBusinessObjects(editableCrossTab.Report.Dictionary.BusinessObjects, val5, includeColumns: false, includeCalcColumns: false);
			((TreeView)treeView).Nodes.Add(val5);
			SetImageIndex(val5.Nodes, 145);
			CheckSelectedNode(val5.Nodes, treeView);
		}
		((TreeView)treeView).ExpandAll();
	}

	private void TreeViewBoxDataSource_ValueChanged(object sender, StiValueChangedEventArgs e)
	{
		((Control)treeViewBoxDataSource).Text = e.Text;
		if (((TreeView)e.TreeView).SelectedNode == ((TreeView)e.TreeView).Nodes[0])
		{
			SelectedDataSource = null;
			((TreeView)treeViewDataSource).Nodes.Clear();
			RefreshStates();
		}
		else if (((TreeView)e.TreeView).SelectedNode.Tag is StiDataSource)
		{
			SelectedDataSource = ((TreeView)e.TreeView).SelectedNode.Tag as StiDataSource;
			SetNewData(SelectedDataSource, null);
		}
		else if (((TreeView)e.TreeView).SelectedNode.Tag is StiBusinessObject)
		{
			SelectedBusinessObject = ((TreeView)e.TreeView).SelectedNode.Tag as StiBusinessObject;
			SetNewData(null, SelectedBusinessObject);
		}
		RefreshSelectedDataSourceText();
		((TreeView)treeViewColumns).Nodes.Clear();
		((TreeView)treeViewRows).Nodes.Clear();
		((TreeView)treeViewSummary).Nodes.Clear();
		UpdateCrossTab();
		UpdatePropertyGrid();
	}

	private void TreeViewBoxDataSourceTreeNode_BeforeExpand(object sender, TreeViewCancelEventArgs e)
	{
		if (e.Node is TreeNodeEx node && e.Node.Tag != null)
		{
			dataBuilder.ExpandedNode(node, includeCalcColumn: false);
		}
	}

	private void TreeViewColumns_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
	{
		if (treeViewColumns.IsCloseButtonClicked(e))
		{
			((TreeView)treeViewColumns).Nodes.Remove(e.Node);
			KillRightTitle();
			UpdateCrossTab();
		}
	}

	private void TreeViewSummary_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
	{
		if (treeViewSummary.IsCloseButtonClicked(e))
		{
			((TreeView)treeViewSummary).Nodes.Remove(e.Node);
			KillRightTitle();
			UpdateCrossTab();
		}
	}

	private void TreeViewRows_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
	{
		if (treeViewRows.IsCloseButtonClicked(e))
		{
			((TreeView)treeViewRows).Nodes.Remove(e.Node);
			KillRightTitle();
			UpdateCrossTab();
		}
	}

	private void TreeViewDataSource_DragOver(object sender, DragEventArgs e)
	{
		object data = e.Data.GetData(typeof(TreeNode));
		TreeNode val = (TreeNode)((data is TreeNode) ? data : null);
		if (val != null)
		{
			e.Effect = (DragDropEffects)0;
		}
		else
		{
			e.Effect = (DragDropEffects)2;
		}
	}

	private void TreeViewDataSource_DragDrop(object sender, DragEventArgs e)
	{
		object data = e.Data.GetData(typeof(TreeNode));
		TreeNode val = (TreeNode)((data is TreeNode) ? data : null);
		if (val == null)
		{
			if (sender is StiCrossTreeView)
			{
				((StiCrossTreeView)sender).IsDragOverFromDictionary = false;
			}
			StiDataColumn draggedDataColumn = StiDragDropColumnHelper.GetDraggedDataColumn(Designer);
			if (draggedDataColumn != null)
			{
				SetNewData(draggedDataColumn.DataSource, null);
				RefreshSelectedDataSourceText();
			}
			StiDataBand draggedDataBand = StiDragDropColumnHelper.GetDraggedDataBand(Designer);
			if (draggedDataBand != null)
			{
				SetNewData(draggedDataBand.DataSource, null);
				RefreshSelectedDataSourceText();
			}
		}
	}

	private void PanelSampleGrid_MouseDown(object sender, MouseEventArgs e)
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Invalid comparison between Unknown and I4
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		foreach (StiCrossField component in editableCrossTab.Components)
		{
			if (StiScale.I(editableCrossTab.Report.Unit.ConvertToHInches(component.ClientRectangle).ToRectangle()).Contains(e.X - StiScale.I4, e.Y - StiScale.I4))
			{
				if ((Control.ModifierKeys & 0x10000) > 0)
				{
					component.Invert();
				}
				else
				{
					editableCrossTab.ResetSelection();
					component.IsSelected = true;
					SelectFieldNode(component);
				}
				((Control)panelSampleGrid).Invalidate();
				UpdatePropertyGrid();
				return;
			}
		}
		if ((Control.ModifierKeys & 0x20000) == 0)
		{
			editableCrossTab.ResetSelection();
		}
		((Control)panelSampleGrid).Invalidate();
		UpdatePropertyGrid();
	}

	private void ButtonRemoveRow_Click(object sender, EventArgs e)
	{
		if (((TreeView)treeViewRows).Nodes.Count > 0)
		{
			TreeNode selectedNode = ((TreeView)treeViewRows).SelectedNode;
			((TreeView)treeViewRows).Nodes.Remove(selectedNode);
			StiCrossRow stiCrossRow = selectedNode.Tag as StiCrossRow;
			StiCrossTitle crossRowTitle = stiCrossRow.GetCrossRowTitle();
			StiCrossRowTotal crossRowTotal = stiCrossRow.GetCrossRowTotal();
			if (editableCrossTab.Components.Contains(stiCrossRow))
			{
				editableCrossTab.Components.Remove(stiCrossRow);
			}
			if (crossRowTitle != null && editableCrossTab.Components.Contains(crossRowTitle))
			{
				editableCrossTab.Components.Remove(crossRowTitle);
			}
			if (crossRowTotal != null && editableCrossTab.Components.Contains(crossRowTotal))
			{
				editableCrossTab.Components.Remove(crossRowTotal);
			}
			OrderRows();
			UpdateCrossTab();
		}
	}

	private void ButtonRemoveSummary_Click(object sender, EventArgs e)
	{
		if (((TreeView)treeViewSummary).Nodes.Count > 0)
		{
			((TreeView)treeViewSummary).Nodes.Remove(((TreeView)treeViewSummary).SelectedNode);
			UpdateCrossTab();
		}
	}

	private void ButtonRemoveColumn_Click(object sender, EventArgs e)
	{
		if (((TreeView)treeViewColumns).Nodes.Count > 0)
		{
			((TreeView)treeViewColumns).Nodes.Remove(((TreeView)treeViewColumns).SelectedNode);
			KillRightTitle();
			UpdateCrossTab();
		}
	}

	private void ButtonSwap_Click(object sender, EventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Expected O, but got Unknown
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Expected O, but got Unknown
		List<StiCrossRow> list = new List<StiCrossRow>();
		foreach (TreeNode node in ((TreeView)treeViewColumns).Nodes)
		{
			TreeNode val = node;
			StiCrossColumn stiCrossColumn = val.Tag as StiCrossColumn;
			StiCrossRow stiCrossRow = CopyColumnToRow(stiCrossColumn);
			list.Add(stiCrossRow);
			ConvertColumnTotal(stiCrossColumn, stiCrossRow);
		}
		((TreeView)treeViewColumns).Nodes.Clear();
		foreach (TreeNode node2 in ((TreeView)treeViewRows).Nodes)
		{
			TreeNode val2 = node2;
			StiCrossRow stiCrossRow2 = val2.Tag as StiCrossRow;
			StiCrossColumn stiCrossColumn2 = CopyRowToColumn(stiCrossRow2);
			TreeNode val3 = new TreeNode(stiCrossColumn2.Alias)
			{
				Tag = stiCrossColumn2
			};
			((TreeView)treeViewColumns).Nodes.Add(val3);
			ConvertRowTotal(stiCrossRow2, stiCrossColumn2);
		}
		((TreeView)treeViewRows).Nodes.Clear();
		foreach (StiCrossRow item in list)
		{
			TreeNode val4 = new TreeNode(item.Alias)
			{
				Tag = item
			};
			((TreeView)treeViewRows).Nodes.Add(val4);
		}
		UpdateCrossTab();
	}

	private void ButtonSummaryDirection_Click(object sender, EventArgs e)
	{
		editableCrossTab.SummaryDirection = ((editableCrossTab.SummaryDirection == StiSummaryDirection.LeftToRight) ? StiSummaryDirection.UpToDown : StiSummaryDirection.LeftToRight);
		UpdateCrossTab();
	}

	private void PanelCrossGrid_Paint(object sender, PaintEventArgs e)
	{
		if (editableCrossTab == null || IsEmpty)
		{
			TextRenderer.DrawText((IDeviceContext)(object)e.Graphics, Loc.Get("Dashboard", "DataNotDefined"), ((Control)panelSampleGrid).Font, new Rectangle(0, 0, ((Control)panelSampleGrid).Width, ((Control)panelSampleGrid).Height), Color.DimGray, (TextFormatFlags)5);
		}
		if (editableCrossTab != null)
		{
			e.Graphics.TranslateTransform((float)StiScale.I4, (float)StiScale.I4);
			double zoom = editableCrossTab.Report.Info.Zoom;
			editableCrossTab.Report.Info.Zoom = 1.0;
			RectangleD paintRectangle = editableCrossTab.GetPaintRectangle(convertToHInches: true, convertByZoom: true);
			e.Graphics.TranslateTransform(0f - (float)paintRectangle.X, 0f - (float)paintRectangle.Y);
			StiPaintEventArgs e2 = new StiPaintEventArgs(e.Graphics, paintRectangle);
			StiCrossTabGdiPainter stiCrossTabGdiPainter = StiPainter.GetPainter(editableCrossTab.GetType(), StiGuiMode.Gdi) as StiCrossTabGdiPainter;
			stiCrossTabGdiPainter.PaintComponents(editableCrossTab, e2);
			editableCrossTab.Report.Info.Zoom = zoom;
		}
	}

	private void PanelSample_Paint(object sender, PaintEventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		Pen val = new Pen(Color.FromArgb(170, 170, 170));
		try
		{
			val.DashPattern = new float[2] { 1f, 2f };
			graphics.DrawLine(val, 0, 0, ((Control)panelSample).Width, 0);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void PanelDataSource_Paint(object sender, PaintEventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		Pen val = new Pen(Color.FromArgb(170, 170, 170));
		try
		{
			val.DashPattern = new float[2] { 1f, 2f };
			graphics.DrawLine(val, ((Control)panelDataSource).Width / 2, 0, ((Control)panelDataSource).Width / 2, ((Control)panelDataSource).Height);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void Designer_SelectedStylesChanged(object sender, EventArgs e)
	{
		editableCrossTab.CrossTabStyleIndex = crossTab.CrossTabStyleIndex;
		((Control)panelSampleGrid).Invalidate();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Expected O, but got Unknown
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Expected O, but got Unknown
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Expected O, but got Unknown
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Expected O, but got Unknown
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Expected O, but got Unknown
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Expected O, but got Unknown
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Expected O, but got Unknown
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Expected O, but got Unknown
		//IL_04fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0508: Expected O, but got Unknown
		//IL_057c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0586: Expected O, but got Unknown
		//IL_0593: Unknown result type (might be due to invalid IL or missing references)
		//IL_059d: Expected O, but got Unknown
		//IL_090f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0919: Expected O, but got Unknown
		//IL_0926: Unknown result type (might be due to invalid IL or missing references)
		//IL_0930: Expected O, but got Unknown
		//IL_093d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0947: Expected O, but got Unknown
		//IL_0954: Unknown result type (might be due to invalid IL or missing references)
		//IL_095e: Expected O, but got Unknown
		//IL_096b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0975: Expected O, but got Unknown
		//IL_0ac5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b34: Expected O, but got Unknown
		//IL_0b41: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4b: Expected O, but got Unknown
		//IL_0b58: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b62: Expected O, but got Unknown
		//IL_0b6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b79: Expected O, but got Unknown
		//IL_0b86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b90: Expected O, but got Unknown
		//IL_0b9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba7: Expected O, but got Unknown
		//IL_0bcb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd5: Expected O, but got Unknown
		//IL_0be2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bec: Expected O, but got Unknown
		//IL_0c35: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca4: Expected O, but got Unknown
		//IL_0cb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cbb: Expected O, but got Unknown
		//IL_0cc8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd2: Expected O, but got Unknown
		//IL_0cdf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce9: Expected O, but got Unknown
		//IL_0cf6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d00: Expected O, but got Unknown
		//IL_0d0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d17: Expected O, but got Unknown
		//IL_0d3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d45: Expected O, but got Unknown
		//IL_0d52: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d5c: Expected O, but got Unknown
		//IL_0df3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dfd: Expected O, but got Unknown
		//IL_0e0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e14: Expected O, but got Unknown
		//IL_0e21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e2b: Expected O, but got Unknown
		//IL_0e38: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e42: Expected O, but got Unknown
		//IL_0e4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e59: Expected O, but got Unknown
		//IL_0e66: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e70: Expected O, but got Unknown
		//IL_0e94: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e9e: Expected O, but got Unknown
		//IL_0eab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb5: Expected O, but got Unknown
		//IL_0f0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f15: Expected O, but got Unknown
		//IL_11a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_11aa: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiCrossTabDesignerForm));
		ilDataImages = new ImageList(components);
		buttonOk = new Button();
		buttonCancel = new Button();
		panelFooter = new StiGroupLine();
		buttonShowMore = new Button();
		ilData = new ImageList(components);
		panelSample = new Panel();
		panelSampleGrid = new StiCrossPanel();
		ilDirection = new ImageList(components);
		ilRemove = new ImageList(components);
		ilArrows = new ImageList(components);
		buttonSummaryDirection = new StiOperationButton();
		buttonSwap = new StiOperationButton();
		toolTip1 = new ToolTip(components);
		labelColumns = new Label();
		labelSummary = new Label();
		labelRows = new Label();
		treeViewDataSource = new StiTreeView();
		treeViewBoxDataSource = new StiTreeViewBox();
		labelDataSource = new Label();
		treeViewRows = new StiCrossTreeView();
		treeViewSummary = new StiCrossTreeView();
		treeViewColumns = new StiCrossTreeView();
		panelDataSource = new Panel();
		labelSelectDataSource = new Label();
		pictureBoxSelectDataSource = new PictureBox();
		((Control)panelFooter).SuspendLayout();
		((Control)panelSample).SuspendLayout();
		((ISupportInitialize)pictureBoxSelectDataSource).BeginInit();
		((Control)this).SuspendLayout();
		ilDataImages.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("ilDataImages.ImageStream");
		ilDataImages.TransparentColor = Color.Magenta;
		ilDataImages.Images.SetKeyName(0, "");
		ilDataImages.Images.SetKeyName(1, "BusinessObjectNew.png");
		((Control)buttonOk).Anchor = (AnchorStyles)9;
		buttonOk.DialogResult = (DialogResult)1;
		((Control)buttonOk).Location = new Point(474, 12);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(75, 24);
		((Control)buttonOk).TabIndex = 1;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)buttonOk).Click += This_OkClick;
		((Control)buttonCancel).Anchor = (AnchorStyles)9;
		buttonCancel.DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(555, 12);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(75, 24);
		((Control)buttonCancel).TabIndex = 2;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)buttonCancel).Click += This_CancelClick;
		((Control)panelFooter).BackColor = Color.White;
		((Control)panelFooter).Controls.Add((Control)(object)buttonShowMore);
		((Control)panelFooter).Controls.Add((Control)(object)buttonOk);
		((Control)panelFooter).Controls.Add((Control)(object)buttonCancel);
		((Control)panelFooter).Dock = (DockStyle)2;
		panelFooter.LinePosition = StiLinePosition.Top;
		panelFooter.LineStyle = StiLineStyle.Dot;
		((Control)panelFooter).Location = new Point(0, 623);
		((Control)panelFooter).Name = "panelFooter";
		((Control)panelFooter).Size = new Size(643, 46);
		((Control)panelFooter).TabIndex = 1;
		((Control)buttonShowMore).Location = new Point(12, 12);
		((Control)buttonShowMore).Name = "buttonShowMore";
		((Control)buttonShowMore).Size = new Size(103, 24);
		((Control)buttonShowMore).TabIndex = 0;
		((Control)buttonShowMore).Text = "Show More";
		((ButtonBase)buttonShowMore).UseVisualStyleBackColor = true;
		((Control)buttonShowMore).Click += ButtonShowMore_Click;
		ilData.ColorDepth = (ColorDepth)8;
		ilData.ImageSize = new Size(16, 16);
		ilData.TransparentColor = Color.Magenta;
		((ScrollableControl)panelSample).AutoScroll = true;
		((Control)panelSample).BackColor = Color.White;
		((Control)panelSample).Controls.Add((Control)(object)panelSampleGrid);
		((Control)panelSample).Dock = (DockStyle)2;
		((Control)panelSample).Location = new Point(0, 407);
		((Control)panelSample).Name = "panelSample";
		((Control)panelSample).Size = new Size(643, 216);
		((Control)panelSample).TabIndex = 0;
		((Control)panelSample).Paint += new PaintEventHandler(PanelSample_Paint);
		((Control)panelSampleGrid).Anchor = (AnchorStyles)15;
		((Control)panelSampleGrid).BackColor = Color.White;
		((Control)panelSampleGrid).Location = new Point(12, 12);
		((Control)panelSampleGrid).Name = "panelSampleGrid";
		((Control)panelSampleGrid).Size = new Size(618, 192);
		((Control)panelSampleGrid).TabIndex = 0;
		((Control)panelSampleGrid).Paint += new PaintEventHandler(PanelCrossGrid_Paint);
		((Control)panelSampleGrid).MouseDown += new MouseEventHandler(PanelSampleGrid_MouseDown);
		ilDirection.ColorDepth = (ColorDepth)8;
		ilDirection.ImageSize = new Size(16, 16);
		ilDirection.TransparentColor = Color.Magenta;
		ilRemove.ColorDepth = (ColorDepth)8;
		ilRemove.ImageSize = new Size(16, 16);
		ilRemove.TransparentColor = Color.Magenta;
		ilArrows.ColorDepth = (ColorDepth)8;
		ilArrows.ImageSize = new Size(16, 16);
		ilArrows.TransparentColor = Color.Magenta;
		((Control)buttonSummaryDirection).Location = new Point(367, 172);
		((Control)buttonSummaryDirection).Name = "buttonSummaryDirection";
		((Control)buttonSummaryDirection).Size = new Size(24, 24);
		((Control)buttonSummaryDirection).TabIndex = 48;
		((ButtonBase)buttonSummaryDirection).UseVisualStyleBackColor = true;
		((Control)buttonSummaryDirection).Click += ButtonSummaryDirection_Click;
		((Control)buttonSwap).Location = new Point(394, 172);
		((Control)buttonSwap).Name = "buttonSwap";
		((Control)buttonSwap).Size = new Size(24, 24);
		((Control)buttonSwap).TabIndex = 45;
		((ButtonBase)buttonSwap).UseVisualStyleBackColor = true;
		((Control)buttonSwap).Click += ButtonSwap_Click;
		((Control)labelColumns).ForeColor = Color.DimGray;
		((Control)labelColumns).Location = new Point(430, 9);
		((Control)labelColumns).Name = "labelColumns";
		((Control)labelColumns).Size = new Size(201, 24);
		((Control)labelColumns).TabIndex = 52;
		((Control)labelColumns).Text = "Columns";
		labelColumns.TextAlign = (ContentAlignment)16;
		((Control)labelSummary).ForeColor = Color.DimGray;
		((Control)labelSummary).Location = new Point(430, 209);
		((Control)labelSummary).Name = "labelSummary";
		((Control)labelSummary).Size = new Size(200, 24);
		((Control)labelSummary).TabIndex = 53;
		((Control)labelSummary).Text = "Summary";
		labelSummary.TextAlign = (ContentAlignment)16;
		((Control)labelRows).BackColor = Color.White;
		((Control)labelRows).ForeColor = Color.DimGray;
		((Control)labelRows).Location = new Point(218, 209);
		((Control)labelRows).Name = "labelRows";
		((Control)labelRows).Size = new Size(200, 24);
		((Control)labelRows).TabIndex = 51;
		((Control)labelRows).Text = "Rows";
		labelRows.TextAlign = (ContentAlignment)16;
		((Control)treeViewDataSource).AllowDrop = true;
		((Control)treeViewDataSource).Anchor = (AnchorStyles)13;
		((TreeView)treeViewDataSource).BorderStyle = (BorderStyle)0;
		((TreeView)treeViewDataSource).HideSelection = false;
		((Control)treeViewDataSource).Location = new Point(12, 73);
		((Control)treeViewDataSource).Name = "treeViewDataSource";
		((Control)treeViewDataSource).Size = new Size(181, 321);
		((Control)treeViewDataSource).TabIndex = 1;
		((TreeView)treeViewDataSource).BeforeExpand += new TreeViewCancelEventHandler(TreeViewDataSource_BeforeExpand);
		((TreeView)treeViewDataSource).ItemDrag += new ItemDragEventHandler(TreeView_ItemDrag);
		((Control)treeViewDataSource).DragDrop += new DragEventHandler(TreeViewDataSource_DragDrop);
		((Control)treeViewDataSource).DragOver += new DragEventHandler(TreeViewDataSource_DragOver);
		((Control)treeViewDataSource).MouseDown += new MouseEventHandler(TreeView_MouseDown);
		((Control)treeViewBoxDataSource).Anchor = (AnchorStyles)13;
		((Control)treeViewBoxDataSource).BackColor = SystemColors.Window;
		((Control)treeViewBoxDataSource).Location = new Point(12, 39);
		((Control)treeViewBoxDataSource).Name = "treeViewBoxDataSource";
		((Control)treeViewBoxDataSource).Size = new Size(181, 21);
		((Control)treeViewBoxDataSource).TabIndex = 2;
		treeViewBoxDataSource.ValueChanged += TreeViewBoxDataSource_ValueChanged;
		treeViewBoxDataSource.TreeNodesFill += TreeViewBoxDataSource_TreeNodesFill;
		((Control)labelDataSource).ForeColor = Color.DimGray;
		((Control)labelDataSource).Location = new Point(12, 12);
		((Control)labelDataSource).Name = "labelDataSource";
		((Control)labelDataSource).Size = new Size(147, 24);
		((Control)labelDataSource).TabIndex = 52;
		((Control)labelDataSource).Text = "DataSource";
		labelDataSource.TextAlign = (ContentAlignment)16;
		((Control)treeViewRows).AllowDrop = true;
		((TreeView)treeViewRows).HideSelection = false;
		((TreeView)treeViewRows).ItemHeight = 21;
		((Control)treeViewRows).Location = new Point(218, 236);
		((Control)treeViewRows).Margin = new Padding(3, 3, 3, 0);
		((Control)treeViewRows).Name = "treeViewRows";
		((TreeView)treeViewRows).ShowPlusMinus = false;
		((TreeView)treeViewRows).ShowRootLines = false;
		((Control)treeViewRows).Size = new Size(200, 160);
		((Control)treeViewRows).TabIndex = 3;
		((TreeView)treeViewRows).ItemDrag += new ItemDragEventHandler(TreeView_ItemDrag);
		((TreeView)treeViewRows).AfterSelect += new TreeViewEventHandler(TreeView_AfterSelect);
		((TreeView)treeViewRows).NodeMouseClick += new TreeNodeMouseClickEventHandler(TreeViewRows_NodeMouseClick);
		((Control)treeViewRows).DragDrop += new DragEventHandler(TreeView_DragDrop);
		((Control)treeViewRows).DragEnter += new DragEventHandler(TreeView_DragEnter);
		((Control)treeViewRows).DragOver += new DragEventHandler(TreeView_DragOver);
		((Control)treeViewRows).DragLeave += TreeView_DragLeave;
		((Control)treeViewRows).KeyDown += new KeyEventHandler(TreeViewRows_KeyDown);
		((Control)treeViewRows).MouseDown += new MouseEventHandler(TreeView_MouseDown);
		((Control)treeViewSummary).AllowDrop = true;
		((TreeView)treeViewSummary).HideSelection = false;
		((TreeView)treeViewSummary).ItemHeight = 21;
		((Control)treeViewSummary).Location = new Point(430, 236);
		((Control)treeViewSummary).Margin = new Padding(3, 3, 3, 0);
		((Control)treeViewSummary).Name = "treeViewSummary";
		((TreeView)treeViewSummary).ShowPlusMinus = false;
		((TreeView)treeViewSummary).ShowRootLines = false;
		((Control)treeViewSummary).Size = new Size(200, 160);
		((Control)treeViewSummary).TabIndex = 4;
		((TreeView)treeViewSummary).ItemDrag += new ItemDragEventHandler(TreeView_ItemDrag);
		((TreeView)treeViewSummary).AfterSelect += new TreeViewEventHandler(TreeView_AfterSelect);
		((TreeView)treeViewSummary).NodeMouseClick += new TreeNodeMouseClickEventHandler(TreeViewSummary_NodeMouseClick);
		((Control)treeViewSummary).DragDrop += new DragEventHandler(TreeView_DragDrop);
		((Control)treeViewSummary).DragEnter += new DragEventHandler(TreeView_DragEnter);
		((Control)treeViewSummary).DragOver += new DragEventHandler(TreeView_DragOver);
		((Control)treeViewSummary).DragLeave += TreeView_DragLeave;
		((Control)treeViewSummary).KeyDown += new KeyEventHandler(TeeeViewSummary_KeyDown);
		((Control)treeViewSummary).MouseDown += new MouseEventHandler(TreeView_MouseDown);
		((Control)treeViewColumns).AllowDrop = true;
		((TreeView)treeViewColumns).HideSelection = false;
		((TreeView)treeViewColumns).ItemHeight = 21;
		((Control)treeViewColumns).Location = new Point(430, 36);
		((Control)treeViewColumns).Name = "treeViewColumns";
		((TreeView)treeViewColumns).ShowPlusMinus = false;
		((TreeView)treeViewColumns).ShowRootLines = false;
		((Control)treeViewColumns).Size = new Size(200, 160);
		((Control)treeViewColumns).TabIndex = 2;
		((TreeView)treeViewColumns).ItemDrag += new ItemDragEventHandler(TreeView_ItemDrag);
		((TreeView)treeViewColumns).AfterSelect += new TreeViewEventHandler(TreeView_AfterSelect);
		((TreeView)treeViewColumns).NodeMouseClick += new TreeNodeMouseClickEventHandler(TreeViewColumns_NodeMouseClick);
		((Control)treeViewColumns).DragDrop += new DragEventHandler(TreeView_DragDrop);
		((Control)treeViewColumns).DragEnter += new DragEventHandler(TreeView_DragEnter);
		((Control)treeViewColumns).DragOver += new DragEventHandler(TreeView_DragOver);
		((Control)treeViewColumns).DragLeave += TreeView_DragLeave;
		((Control)treeViewColumns).KeyDown += new KeyEventHandler(TreeViewColumns_KeyDown);
		((Control)treeViewColumns).MouseDown += new MouseEventHandler(TreeView_MouseDown);
		((Control)panelDataSource).Location = new Point(203, 0);
		((Control)panelDataSource).Name = "panelDataSource";
		((Control)panelDataSource).Size = new Size(3, 406);
		((Control)panelDataSource).TabIndex = 53;
		((Control)panelDataSource).Paint += new PaintEventHandler(PanelDataSource_Paint);
		((Control)labelSelectDataSource).ForeColor = Color.DimGray;
		((Control)labelSelectDataSource).Location = new Point(248, 34);
		((Control)labelSelectDataSource).Name = "labelSelectDataSource";
		((Control)labelSelectDataSource).Size = new Size(303, 30);
		((Control)labelSelectDataSource).TabIndex = 54;
		((Control)labelSelectDataSource).Text = "Select data source";
		labelSelectDataSource.TextAlign = (ContentAlignment)16;
		((Control)labelSelectDataSource).Visible = false;
		((Control)pictureBoxSelectDataSource).BackgroundImageLayout = (ImageLayout)3;
		((Control)pictureBoxSelectDataSource).Location = new Point(213, 34);
		((Control)pictureBoxSelectDataSource).Name = "pictureBoxSelectDataSource";
		((Control)pictureBoxSelectDataSource).Size = new Size(32, 32);
		pictureBoxSelectDataSource.TabIndex = 55;
		pictureBoxSelectDataSource.TabStop = false;
		((Control)pictureBoxSelectDataSource).Visible = false;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(643, 669);
		((Control)this).Controls.Add((Control)(object)pictureBoxSelectDataSource);
		((Control)this).Controls.Add((Control)(object)buttonSummaryDirection);
		((Control)this).Controls.Add((Control)(object)buttonSwap);
		((Control)this).Controls.Add((Control)(object)treeViewRows);
		((Control)this).Controls.Add((Control)(object)treeViewColumns);
		((Control)this).Controls.Add((Control)(object)labelRows);
		((Control)this).Controls.Add((Control)(object)labelColumns);
		((Control)this).Controls.Add((Control)(object)treeViewSummary);
		((Control)this).Controls.Add((Control)(object)labelSummary);
		((Control)this).Controls.Add((Control)(object)panelDataSource);
		((Control)this).Controls.Add((Control)(object)labelDataSource);
		((Control)this).Controls.Add((Control)(object)treeViewBoxDataSource);
		((Control)this).Controls.Add((Control)(object)treeViewDataSource);
		((Control)this).Controls.Add((Control)(object)panelSample);
		((Control)this).Controls.Add((Control)(object)panelFooter);
		((Control)this).Controls.Add((Control)(object)labelSelectDataSource);
		((Control)this).Name = "StiCrossTabDesignerForm";
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "CrossTab Editor";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).FormClosed += new FormClosedEventHandler(This_FormClosed);
		((Control)panelFooter).ResumeLayout(false);
		((Control)panelSample).ResumeLayout(false);
		((ISupportInitialize)pictureBoxSelectDataSource).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
