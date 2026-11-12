using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Design.Wizards;

[ToolboxItem(false)]
public class StiColumnsSelection : UserControl
{
	private StiDataBuilder dataBuilder = new StiDataBuilder();

	private StiDataSourcesCollection checkedDataSources;

	private StiDataSource selectedDataSource;

	private StiBusinessObject selectedBusinessObject;

	private StiBusinessObjectsCollection checkedBusinessObjects;

	private IContainer components;

	public TreeView tvColumns;

	private Button btMarkAll;

	private Button btReset;

	private StiGroupLine groupLine;

	private Panel panel1;

	[Browsable(false)]
	public StiDataSource SelectedDataSource
	{
		get
		{
			return selectedDataSource;
		}
		set
		{
			selectedDataSource = value;
			if (value != null)
			{
				Build();
			}
		}
	}

	[Browsable(false)]
	public StiBusinessObject SelectedBusinessObject
	{
		get
		{
			return selectedBusinessObject;
		}
		set
		{
			selectedBusinessObject = value;
			if (value != null)
			{
				Build();
			}
		}
	}

	[Browsable(false)]
	public StiDataSourcesCollection CheckedDataSources
	{
		get
		{
			return checkedDataSources;
		}
		set
		{
			checkedDataSources = value;
			if (value != null)
			{
				Build();
			}
		}
	}

	[Browsable(false)]
	public StiBusinessObjectsCollection CheckedBusinessObjects
	{
		get
		{
			return checkedBusinessObjects;
		}
		set
		{
			checkedBusinessObjects = value;
			if (value != null)
			{
				Build();
			}
		}
	}

	public TreeView Columns => tvColumns;

	public StiColumnsSelection()
	{
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			Localize();
		}
	}

	private void Localize()
	{
		((Control)btMarkAll).Text = StiLocalization.Get("Wizards", "MarkAll");
		((Control)btReset).Text = StiLocalization.Get("Wizards", "Reset");
	}

	internal void SetCheckedDataSourcesOrBusinessObject(StiDataSourcesCollection ds, StiBusinessObjectsCollection bo)
	{
		checkedDataSources = ds;
		checkedBusinessObjects = bo;
		if (ds != null || bo != null)
		{
			Build();
		}
	}

	internal void SetDataSourcesOrBusinessObject(StiDataSource ds, StiBusinessObject bo)
	{
		selectedDataSource = ds;
		selectedBusinessObject = bo;
		if (ds != null || bo != null)
		{
			Build();
		}
	}

	public void Build()
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		tvColumns.CheckBoxes = true;
		tvColumns.ImageList = StiDataBuilder.Images;
		if (checkedDataSources != null || checkedBusinessObjects != null)
		{
			Hashtable hashtable = new Hashtable();
			foreach (TreeNode node in tvColumns.Nodes)
			{
				TreeNode val = node;
				hashtable[val.Tag] = val;
			}
			tvColumns.Nodes.Clear();
			if (checkedDataSources != null)
			{
				foreach (StiDataSource checkedDataSource in checkedDataSources)
				{
					object? obj = hashtable[checkedDataSource];
					TreeNode val2 = (TreeNode)((obj is TreeNode) ? obj : null);
					if (val2 != null)
					{
						tvColumns.Nodes.Add(val2);
						val2.Expand();
					}
					else
					{
						BuildDataSource(checkedDataSource);
					}
				}
			}
			if (checkedBusinessObjects != null)
			{
				foreach (StiBusinessObject checkedBusinessObject in checkedBusinessObjects)
				{
					BuildDataSource(checkedBusinessObject);
				}
			}
		}
		else
		{
			tvColumns.Nodes.Clear();
			if (selectedBusinessObject != null)
			{
				BuildDataSource(selectedBusinessObject);
			}
			if (selectedDataSource != null)
			{
				BuildDataSource(selectedDataSource);
			}
		}
		if (tvColumns.Nodes.Count > 0)
		{
			tvColumns.Nodes[0].EnsureVisible();
		}
	}

	public void BuildDataSource(StiDataSource dataSource)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		TreeNodeEx dataNode = new TreeNodeEx();
		dataBuilder.BuildDataSource(dataSource, ref dataNode, includeCalcColumn: true);
		tvColumns.Nodes.Add((TreeNode)(object)dataNode);
		foreach (TreeNode node in tvColumns.Nodes)
		{
			TreeNode val = node;
			val.Expand();
		}
		if (tvColumns.Nodes.Count > 0)
		{
			tvColumns.Nodes[0].EnsureVisible();
		}
	}

	public void BuildDataSource(StiBusinessObject dataSource)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Expected O, but got Unknown
		TreeNode val = new TreeNode
		{
			ImageKey = "BusinessObject",
			SelectedImageKey = "BusinessObject",
			Tag = dataSource
		};
		if (!string.IsNullOrEmpty(dataSource.Alias) && dataSource.Alias != dataSource.Name)
		{
			val.Text = $"{dataSource.Name} [{dataSource.Alias}]";
		}
		else
		{
			val.Text = dataSource.Name;
		}
		dataBuilder.BuildBusinessObject(dataSource, val, includeColumns: true, includeCalcColumns: true);
		tvColumns.Nodes.Add(val);
		foreach (TreeNode node in tvColumns.Nodes)
		{
			TreeNode val2 = node;
			val2.Expand();
		}
		if (tvColumns.Nodes.Count > 0)
		{
			tvColumns.Nodes[0].EnsureVisible();
		}
	}

	private bool ColumnsIsSelected(TreeNodeCollection nodes)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		foreach (TreeNode node in nodes)
		{
			TreeNode val = node;
			if (val.Tag is StiDataColumn && val.Checked)
			{
				return true;
			}
			if (ColumnsIsSelected(val.Nodes))
			{
				return true;
			}
		}
		return false;
	}

	public bool ColumnIsSelect()
	{
		return ColumnsIsSelected(tvColumns.Nodes);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		((Control)this).OnPaint(e);
		Graphics graphics = e.Graphics;
		Pen val = new Pen(Color.FromArgb(170, 170, 170));
		try
		{
			val.DashPattern = new float[2] { 1f, 2f };
			Size clientSize = ((Control)this).ClientSize;
			Point point = new Point(clientSize.Width - 1, 0);
			Point point2 = new Point(clientSize.Width - 1, clientSize.Height);
			graphics.DrawLine(val, point, point2);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void TreeViewColumns_BeforeExpand(object sender, TreeViewCancelEventArgs e)
	{
		if (e.Node is TreeNodeEx node && e.Node.Tag != null)
		{
			dataBuilder.ExpandedNode(node, includeCalcColumn: true);
		}
	}

	private void TreeViewColumns_AfterCheck(object sender, TreeViewEventArgs e)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		TreeNode node = e.Node;
		if (node.Nodes.Count == 1 && node.Nodes[0].Text == "Loading")
		{
			node.Expand();
		}
		foreach (TreeNode node2 in node.Nodes)
		{
			TreeNode val = node2;
			if (val.Nodes.Count == 0)
			{
				val.Checked = node.Checked;
			}
		}
	}

	private void ButtonMarkAll_Click(object sender, EventArgs e)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		foreach (TreeNode node in tvColumns.Nodes)
		{
			TreeNode val = node;
			val.Checked = true;
		}
	}

	private void ButtonReset_Click(object sender, EventArgs e)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		foreach (TreeNode node in tvColumns.Nodes)
		{
			TreeNode val = node;
			val.Checked = false;
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			tvColumns.ImageList = null;
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Expected O, but got Unknown
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		tvColumns = new TreeView();
		btMarkAll = new Button();
		btReset = new Button();
		groupLine = new StiGroupLine();
		panel1 = new Panel();
		((Control)groupLine).SuspendLayout();
		((Control)this).SuspendLayout();
		tvColumns.BorderStyle = (BorderStyle)0;
		((Control)tvColumns).Dock = (DockStyle)5;
		((Control)tvColumns).Location = new Point(0, 0);
		((Control)tvColumns).Name = "tvColumns";
		((Control)tvColumns).Size = new Size(408, 409);
		((Control)tvColumns).TabIndex = 0;
		tvColumns.AfterCheck += new TreeViewEventHandler(TreeViewColumns_AfterCheck);
		tvColumns.BeforeExpand += new TreeViewCancelEventHandler(TreeViewColumns_BeforeExpand);
		((Control)btMarkAll).Anchor = (AnchorStyles)10;
		((Control)btMarkAll).Location = new Point(162, 12);
		((Control)btMarkAll).Name = "btMarkAll";
		((Control)btMarkAll).Size = new Size(112, 24);
		((Control)btMarkAll).TabIndex = 1;
		((Control)btMarkAll).Text = "Mark All";
		((ButtonBase)btMarkAll).UseVisualStyleBackColor = true;
		((Control)btMarkAll).Click += ButtonMarkAll_Click;
		((Control)btReset).Anchor = (AnchorStyles)10;
		((Control)btReset).Location = new Point(284, 12);
		((Control)btReset).Name = "btReset";
		((Control)btReset).Size = new Size(112, 24);
		((Control)btReset).TabIndex = 1;
		((Control)btReset).Text = "Reset";
		((ButtonBase)btReset).UseVisualStyleBackColor = true;
		((Control)btReset).Click += ButtonReset_Click;
		((Control)groupLine).BackColor = Color.White;
		((Control)groupLine).Controls.Add((Control)(object)btMarkAll);
		((Control)groupLine).Controls.Add((Control)(object)btReset);
		((Control)groupLine).Dock = (DockStyle)2;
		groupLine.LinePosition = StiLinePosition.Top;
		groupLine.LineStyle = StiLineStyle.Dot;
		((Control)groupLine).Location = new Point(0, 421);
		((Control)groupLine).Name = "groupLine";
		((Control)groupLine).Size = new Size(408, 46);
		((Control)groupLine).TabIndex = 2;
		((Control)panel1).Dock = (DockStyle)2;
		((Control)panel1).Location = new Point(0, 409);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(408, 12);
		((Control)panel1).TabIndex = 3;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)tvColumns);
		((Control)this).Controls.Add((Control)(object)panel1);
		((Control)this).Controls.Add((Control)(object)groupLine);
		((Control)this).Name = "StiColumnsSelection";
		((Control)this).Size = new Size(408, 467);
		((Control)groupLine).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
