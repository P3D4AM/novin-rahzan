using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Design.Editors;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Design.Wizards;

[ToolboxItem(false)]
public class StiColumnsSort : UserControl
{
	private StiDataBuilder dataBuilder = new StiDataBuilder();

	private bool lockSetSort;

	private StiDataSource dataSource;

	private StiBusinessObject businessObject;

	private string[] sort;

	private IContainer components;

	private StiZoneContainerControl zcSorts;

	public StiToolBar tbTool;

	private StiButtonUX tbAddLevel;

	[Browsable(false)]
	public StiDataSource DataSource
	{
		get
		{
			return dataSource;
		}
		set
		{
			if (dataSource != value)
			{
				dataSource = value;
				sort = new string[0];
				BuildSort();
			}
		}
	}

	[Browsable(false)]
	public StiBusinessObject BusinessObject
	{
		get
		{
			return businessObject;
		}
		set
		{
			if (businessObject != value)
			{
				businessObject = value;
				sort = new string[0];
				BuildSort();
			}
		}
	}

	[Browsable(false)]
	public string[] Sort
	{
		get
		{
			return sort;
		}
		set
		{
			sort = value;
			BuildSort();
		}
	}

	private string NoSort => "[" + Loc.Get("FormBand", "NoSort") + "]";

	private string Asc => StiLocalization.Get("FormBand", "Ascending");

	private string Desc => StiLocalization.Get("FormBand", "Descending");

	public StiColumnsSort()
	{
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			Localize();
			BuildSort();
			MeasureButtonSize();
		}
	}

	public void SetDataSourceOrBusinessObject(StiDataSource ds, StiBusinessObject bo)
	{
		dataSource = ds;
		businessObject = bo;
		sort = new string[0];
		BuildSort();
	}

	private void MeasureButtonSize()
	{
		Size size = TextRenderer.MeasureText(((Control)tbAddLevel).Text, ((Control)tbAddLevel).Font);
		((Control)tbAddLevel).Width = size.Width + StiScale.I20;
	}

	private void Localize()
	{
		((Control)tbAddLevel).Text = StiLocalization.Get("FormBand", "AddSort");
	}

	private string[] GetSort(string str, bool ascend)
	{
		List<string> list = new List<string>();
		if (ascend)
		{
			list.Add("ASC");
		}
		else
		{
			list.Add("DESC");
		}
		if (str.StartsWithInvariant("{") && str.EndsWithInvariant("}"))
		{
			list.Add(str);
		}
		else
		{
			string[] array = str.Split(new char[1] { '.' });
			if (array.Length >= 2 && DataSource != null)
			{
				StiDataSource parentSource = DataSource;
				for (int i = 0; i < array.Length - 1; i++)
				{
					StiDataRelationsCollection parentRelations = parentSource.GetParentRelations();
					foreach (StiDataRelation item in parentRelations)
					{
						if (!(item.Alias != array[i]))
						{
							array[i] = item.NameInSource;
							parentSource = item.ParentSource;
							break;
						}
					}
				}
			}
			list.AddRange(array);
		}
		return list.ToArray();
	}

	private void SetSorts()
	{
		List<string> list = new List<string>();
		for (int num = ((ArrangedElementCollection)((Control)zcSorts).Controls).Count - 1; num >= 0; num--)
		{
			StiSortZoneControl stiSortZoneControl = ((Control)zcSorts).Controls[num] as StiSortZoneControl;
			if (((Control)stiSortZoneControl.cbColumn).Text != NoSort)
			{
				list.AddRange(GetSort(((Control)stiSortZoneControl.cbColumn).Text, ((Control)stiSortZoneControl.cbOrder).Text == Asc));
			}
		}
		sort = list.ToArray();
	}

	private bool SelectNode(string[] strs, int index, TreeNodeCollection nodes, TreeView view)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		if (strs.Length <= index)
		{
			return false;
		}
		string text = strs[index];
		foreach (TreeNode node in nodes)
		{
			TreeNode val = node;
			string str = ((val.Tag is StiDataRelation) ? ((StiDataRelation)val.Tag).Name : val.Text);
			str = StiNameValidator.CorrectName(str, dataSource?.Dictionary?.Report);
			if (!(str == text))
			{
				continue;
			}
			if (val.Nodes.Count == 0)
			{
				if (strs.Length - 1 != index)
				{
					return false;
				}
				view.SelectedNode = val;
				return true;
			}
			if (val.Nodes.Count == 1 && val.Nodes[0].Text == "Loading")
			{
				ExpandTreeNode(val);
				val.Expand();
			}
			return SelectNode(strs, index + 1, val.Nodes, view);
		}
		return false;
	}

	private void ExpandTreeNode(TreeNode node)
	{
		if (node is TreeNodeEx node2 && node.Tag != null)
		{
			dataBuilder.ExpandedNode(node2, includeCalcColumn: true);
		}
	}

	private void BuildSort()
	{
		((Control)zcSorts).SuspendLayout();
		lockSetSort = true;
		((Control)zcSorts).Controls.Clear();
		lockSetSort = false;
		string[] array = ((Sort != null) ? (Sort.Clone() as string[]) : new string[0]);
		if (array.Length != 0)
		{
			int i = 0;
			while (i < array.Length)
			{
				bool flag = array[i++] == "ASC";
				int num = i;
				StiDataSource parentSource = DataSource;
				if (DataSource == null && BusinessObject == null)
				{
					break;
				}
				for (; i < array.Length - 1 && array[i + 1] != "ASC" && array[i + 1] != "DESC"; i++)
				{
					if (parentSource == null || (array[i].StartsWithInvariant("{") && array[i].EndsWithInvariant("{")))
					{
						continue;
					}
					StiDataRelationsCollection parentRelations = parentSource.GetParentRelations();
					foreach (StiDataRelation item in parentRelations)
					{
						if (item.NameInSource == array[i])
						{
							array[i] = item.Alias;
							parentSource = item.ParentSource;
							break;
						}
					}
				}
				string text = string.Empty;
				if (num < array.Length)
				{
					for (int j = num; j <= i; j++)
					{
						if (text.Length > 0)
						{
							text += ".";
						}
						text += array[j];
					}
				}
				i++;
				if (flag)
				{
					AddLevel(text, Asc);
				}
				else
				{
					AddLevel(text, Desc);
				}
			}
		}
		AddLevel();
		CheckSortBy();
		((Control)zcSorts).ResumeLayout();
	}

	private void AddLevel()
	{
		AddLevel(NoSort, Asc);
	}

	private void AddLevel(string column, string order)
	{
		StiSortZoneControl stiSortZoneControl = new StiSortZoneControl();
		stiSortZoneControl.cbColumn.ShowButton2 = BusinessObject == null;
		((ComboBox)stiSortZoneControl.cbOrder).Items.Add((object)Asc);
		((ComboBox)stiSortZoneControl.cbOrder).Items.Add((object)Desc);
		((Control)stiSortZoneControl.cbColumn).Text = column;
		((Control)stiSortZoneControl.cbOrder).Text = order;
		stiSortZoneControl.cbColumn.ValueChanged += ComboBoxColumn_ValueChanged;
		stiSortZoneControl.cbColumn.TreeNodesFill += ComboBoxColumn_TreeNodesFill;
		stiSortZoneControl.cbColumn.ButtonClick2 += ComboBoxColumn_ButtonClick2;
		stiSortZoneControl.cbColumn.DropUp += ComboBoxColumn_DropUp;
		((ComboBox)stiSortZoneControl.cbOrder).SelectedIndexChanged += ComboBoxOrder_SelectedIndexChanged;
		((Control)stiSortZoneControl).Width = 1;
		((Control)zcSorts).Controls.Add((Control)(object)stiSortZoneControl);
	}

	private void CheckSortBy()
	{
		for (int num = ((ArrangedElementCollection)((Control)zcSorts).Controls).Count - 1; num >= 0; num--)
		{
			StiSortZoneControl stiSortZoneControl = ((Control)zcSorts).Controls[num] as StiSortZoneControl;
			((Control)stiSortZoneControl.lbSortBy).Text = ((num == ((ArrangedElementCollection)((Control)zcSorts).Controls).Count - 1) ? StiLocalization.Get("FormBand", "SortBy") : StiLocalization.Get("FormBand", "ThenBy"));
		}
	}

	private void ButtonAddLevel_Click(object sender, EventArgs e)
	{
		AddLevel();
		SetSorts();
		CheckSortBy();
	}

	private void TreeNode_Expand(object sender, TreeViewCancelEventArgs e)
	{
		ExpandTreeNode(e.Node);
	}

	private void ComboBoxColumn_TreeNodesFill(object sender, StiTreeNodesFillEventArgs e)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		((TreeView)e.TreeView).ImageList = StiDataBuilder.Images;
		((TreeView)e.TreeView).BeforeExpand += new TreeViewCancelEventHandler(TreeNode_Expand);
		TreeNode val = new TreeNode(NoSort);
		string imageKey = (val.SelectedImageKey = "Close");
		val.ImageKey = imageKey;
		e.Nodes.Add(val);
		TreeNodeEx dataNode = new TreeNodeEx();
		if (DataSource != null)
		{
			dataBuilder.BuildDataSource(DataSource, ref dataNode, includeCalcColumn: false);
		}
		if (BusinessObject != null)
		{
			((TreeNode)dataNode).Tag = BusinessObject;
			((TreeNode)dataNode).Text = BusinessObject.ToString();
			TreeNodeEx treeNodeEx = dataNode;
			imageKey = (((TreeNode)dataNode).SelectedImageKey = "BusinessObject");
			((TreeNode)treeNodeEx).ImageKey = imageKey;
			dataBuilder.BuildBusinessObject(BusinessObject, (TreeNode)(object)dataNode, includeColumns: true, includeCalcColumns: false);
		}
		if (dataNode == null || ((TreeNode)dataNode).Nodes.Count <= 0)
		{
			return;
		}
		e.Nodes.Add((TreeNode)(object)dataNode);
		StiTreeViewBox stiTreeViewBox = sender as StiTreeViewBox;
		if (((Control)stiTreeViewBox).Text == NoSort)
		{
			((TreeView)e.TreeView).SelectedNode = e.Nodes[0];
			return;
		}
		string[] strs = ((Control)stiTreeViewBox).Text.Split(new char[1] { '.' });
		TreeNode val2 = e.Nodes[1];
		if (val2.Nodes.Count == 1 && val2.Nodes[0].Text == "Loading")
		{
			ExpandTreeNode(val2);
		}
		if (!SelectNode(strs, 0, val2.Nodes, (TreeView)(object)e.TreeView))
		{
			((TreeView)e.TreeView).SelectedNode = e.Nodes[0];
		}
	}

	private void ComboBoxColumn_ValueChanged(object sender, StiValueChangedEventArgs e)
	{
		if (e.Text == NoSort)
		{
			return;
		}
		TreeNode selectedNode = ((TreeView)e.TreeView).SelectedNode;
		if (selectedNode != null)
		{
			if (selectedNode.Tag is StiDataColumn)
			{
				string columnPathFromNodeWithAlias = StiDataBuilder.GetColumnPathFromNodeWithAlias(selectedNode);
				int num = 0;
				if ((selectedNode.Tag as StiDataColumn).BusinessObject == null)
				{
					num = columnPathFromNodeWithAlias.IndexOfInvariant(".");
				}
				else
				{
					num = columnPathFromNodeWithAlias.Length;
					for (int num2 = selectedNode.Level; num2 > 0; num2--)
					{
						num = columnPathFromNodeWithAlias.LastIndexOf(".", num - 1, num, StringComparison.InvariantCulture);
					}
				}
				e.Text = columnPathFromNodeWithAlias.Substring(num + 1);
			}
			else if (selectedNode.Tag is PropertyInfo)
			{
				e.Text = "{" + StiDataBuilder.GetPropertyInfoPathFromNode(selectedNode) + "}";
			}
			else
			{
				e.Text = NoSort;
			}
		}
		else
		{
			e.Text = NoSort;
		}
	}

	private void ComboBoxColumn_ButtonClick2(object sender, EventArgs e)
	{
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Invalid comparison between Unknown and I4
		StiTreeViewMultiBox stiTreeViewMultiBox = sender as StiTreeViewMultiBox;
		string text = ((Control)stiTreeViewMultiBox).Text;
		if (text == NoSort)
		{
			text = string.Empty;
		}
		else
		{
			if (text.StartsWithInvariant("{"))
			{
				text = text.Substring(1);
			}
			if (text.EndsWithInvariant("}"))
			{
				text = text.Substring(0, text.Length - 1);
			}
			if (dataSource != null && dataSource.Columns.Contains(text))
			{
				text = dataSource.Name + "." + text;
			}
		}
		StiReport report = ((dataSource == null) ? null : dataSource.Dictionary.Report);
		DialogResult val = StiTextEditor.EditText(report, ref text, StiCodeEditorMode.EditorCode);
		if ((int)val == 1)
		{
			((Control)stiTreeViewMultiBox).Text = "{" + text + "}";
			if (text.Trim().Length == 0)
			{
				text = NoSort;
			}
			SetSorts();
			CheckSortBy();
		}
	}

	private void ComboBoxColumn_DropUp(object sender, EventArgs e)
	{
		SetSorts();
		CheckSortBy();
	}

	private void ComboBoxOrder_SelectedIndexChanged(object sender, EventArgs e)
	{
		SetSorts();
		CheckSortBy();
	}

	private void ZoneSorts_ChangeFocusedControl(object sender, EventArgs e)
	{
		CheckSortBy();
	}

	private void ZoneSorts_ControlRemoved(object sender, ControlEventArgs e)
	{
		if (!lockSetSort)
		{
			SetSorts();
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
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Expected O, but got Unknown
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		tbTool = new StiToolBar();
		tbAddLevel = new StiButtonUX();
		zcSorts = new StiZoneContainerControl();
		((Control)tbTool).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)tbTool).Controls.Add((Control)(object)tbAddLevel);
		tbTool.ControlStyle = StiControlStyle.Office2013Blue;
		((Control)tbTool).Dock = (DockStyle)1;
		tbTool.LineStyle = StiToolBar.StiToolBarLineStyle.Bottom;
		((Control)tbTool).Location = new Point(0, 0);
		((Control)tbTool).Margin = new Padding(0);
		((Control)tbTool).Name = "tbTool";
		((Control)tbTool).Padding = new Padding(3);
		((Control)tbTool).Size = new Size(512, 47);
		((Control)tbTool).TabIndex = 57;
		((Control)tbTool).Text = "stiToolBar1";
		((Control)tbAddLevel).Location = new Point(11, 11);
		((Control)tbAddLevel).Name = "tbAddLevel";
		((Control)tbAddLevel).Size = new Size(113, 24);
		((Control)tbAddLevel).TabIndex = 78;
		((Control)tbAddLevel).Text = "Add Filter";
		((ButtonBase)tbAddLevel).UseVisualStyleBackColor = true;
		((Control)tbAddLevel).Click += ButtonAddLevel_Click;
		((Control)zcSorts).AllowDrop = true;
		((ScrollableControl)zcSorts).AutoScroll = true;
		((Control)zcSorts).BackColor = Color.White;
		((Control)zcSorts).Dock = (DockStyle)5;
		((Control)zcSorts).Location = new Point(0, 47);
		((Control)zcSorts).Margin = new Padding(1);
		((Control)zcSorts).Name = "zcSorts";
		((Control)zcSorts).Padding = new Padding(12);
		zcSorts.ShowBorder = false;
		((Control)zcSorts).Size = new Size(512, 302);
		((Control)zcSorts).TabIndex = 0;
		zcSorts.ChangeFocusedControl += ZoneSorts_ChangeFocusedControl;
		((Control)zcSorts).ControlRemoved += new ControlEventHandler(ZoneSorts_ControlRemoved);
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)zcSorts);
		((Control)this).Controls.Add((Control)(object)tbTool);
		((Control)this).Margin = new Padding(0);
		((Control)this).Name = "StiColumnsSort";
		((Control)this).Size = new Size(512, 349);
		((Control)tbTool).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
