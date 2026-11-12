using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Design.Panels;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Design.Wizards;

[ToolboxItem(false)]
public class StiDataSourcePanel : UserControl
{
	private StiReport report;

	private IContainer components;

	public StiTreeView tvDataSources;

	private ImageList ilData;

	public StiToolBar tbTool;

	private StiToolButton tbNewDataSource;

	public bool SelectedDataSourceOrBusinessObject
	{
		get
		{
			TreeNode selectedNode = ((TreeView)tvDataSources).SelectedNode;
			if (selectedNode == null)
			{
				return false;
			}
			if (!(selectedNode.Tag is StiDataSource))
			{
				return selectedNode.Tag is StiBusinessObject;
			}
			return true;
		}
	}

	public StiDataSource SelectedDataSource
	{
		get
		{
			if (((TreeView)tvDataSources).SelectedNode == null)
			{
				return null;
			}
			return ((TreeView)tvDataSources).SelectedNode.Tag as StiDataSource;
		}
	}

	public StiBusinessObject SelectedBusinessObject
	{
		get
		{
			if (((TreeView)tvDataSources).SelectedNode == null)
			{
				return null;
			}
			return ((TreeView)tvDataSources).SelectedNode.Tag as StiBusinessObject;
		}
	}

	public bool CheckedDataSourcesOrBusinessObject
	{
		get
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Expected O, but got Unknown
			if (!((TreeView)tvDataSources).CheckBoxes)
			{
				return false;
			}
			foreach (TreeNode node in ((TreeView)tvDataSources).Nodes)
			{
				TreeNode val = node;
				if ((val.Checked && val.Tag is StiDataSource) || CheckDataSources(val))
				{
					return true;
				}
				if ((val.Checked && val.Tag is StiBusinessObject) || CheckBusinessObjects(val))
				{
					return true;
				}
			}
			return false;
		}
	}

	public StiDataSourcesCollection CheckedDataSources
	{
		get
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Expected O, but got Unknown
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Expected O, but got Unknown
			if (!((TreeView)tvDataSources).CheckBoxes)
			{
				return null;
			}
			StiDataSourcesCollection stiDataSourcesCollection = new StiDataSourcesCollection(null);
			foreach (TreeNode node in ((TreeView)tvDataSources).Nodes)
			{
				TreeNode val = node;
				foreach (TreeNode node2 in val.Nodes)
				{
					TreeNode val2 = node2;
					if (val2.Checked && val2.Tag is StiDataSource)
					{
						stiDataSourcesCollection.Add(val2.Tag as StiDataSource);
					}
				}
			}
			if (stiDataSourcesCollection.Count == 0)
			{
				return null;
			}
			return stiDataSourcesCollection;
		}
	}

	public StiBusinessObjectsCollection CheckedBusinessObjects
	{
		get
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Expected O, but got Unknown
			if (!((TreeView)tvDataSources).CheckBoxes)
			{
				return null;
			}
			StiBusinessObjectsCollection datas = new StiBusinessObjectsCollection(null, null);
			foreach (TreeNode node in ((TreeView)tvDataSources).Nodes)
			{
				TreeNode val = node;
				if (val.Tag is StiBusinessObject stiBusinessObject)
				{
					if (val.Checked)
					{
						datas.Add(stiBusinessObject);
					}
					if (stiBusinessObject.BusinessObjects != null && stiBusinessObject.BusinessObjects.Count > 0)
					{
						CheckBusinessObjects(val, ref datas);
					}
				}
			}
			if (datas.Count == 0)
			{
				return null;
			}
			return datas;
		}
	}

	public StiDataSourcePanel()
	{
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			((Control)tbNewDataSource).Text = StiLocalization.Get("FormDictionaryDesigner", "DataSourceNew");
			StiDictionaryPanelService service = StiDictionaryPanelService.GetService();
			if ((service == null && report.Designer != null) || !StiOptions.Designer.Wizards.ShowNewDataSourceButton)
			{
				((Control)tbNewDataSource).Visible = false;
			}
			((TreeView)tvDataSources).ImageList = StiDataBuilder.Images;
			DoScaling();
		}
	}

	internal void DoScaling()
	{
		((ButtonBase)tbNewDataSource).Image = StiReportDesignImages.Actions.NewDataSource();
	}

	private void CheckBusinessObjects(TreeNode parentNode, ref StiBusinessObjectsCollection datas)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		foreach (TreeNode node in parentNode.Nodes)
		{
			TreeNode val = node;
			if (val.Tag is StiBusinessObject stiBusinessObject)
			{
				if (val.Checked)
				{
					datas.Add(stiBusinessObject);
				}
				if (stiBusinessObject.BusinessObjects != null && stiBusinessObject.BusinessObjects.Count > 0)
				{
					CheckBusinessObjects(val, ref datas);
				}
			}
		}
	}

	private bool CheckDataSources(TreeNode parentNode)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		foreach (TreeNode node in parentNode.Nodes)
		{
			TreeNode val = node;
			if ((val.Checked && val.Tag is StiDataSource) || (val.Nodes.Count > 0 && CheckDataSources(val)))
			{
				return true;
			}
		}
		return false;
	}

	private bool CheckBusinessObjects(TreeNode parentNode)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		foreach (TreeNode node in parentNode.Nodes)
		{
			TreeNode val = node;
			if ((val.Checked && val.Tag is StiBusinessObject) || (val.Nodes.Count > 0 && CheckBusinessObjects(val)))
			{
				return true;
			}
		}
		return false;
	}

	public void Build(StiDataSourcesCollection dataSources, StiReport report)
	{
		Build(dataSources, report, null, null);
	}

	public void Build(StiDataSourcesCollection dataSources, StiReport report, StiDataSource skipDataSource, string dataSourceName)
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Expected O, but got Unknown
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		((Control)tbNewDataSource).Enabled = report != null;
		if (report != null && report.Designer == null)
		{
			((Control)tbNewDataSource).Visible = false;
		}
		this.report = report;
		((TreeView)tvDataSources).Nodes.Clear();
		string text = "DataSource";
		string text2 = "Database";
		foreach (StiDataSource dataSource in dataSources)
		{
			if (dataSource == skipDataSource)
			{
				continue;
			}
			TreeNode val = new TreeNode(dataSource.ToString());
			val.ImageKey = text;
			val.SelectedImageKey = text;
			val.Tag = dataSource;
			string categoryName = dataSource.GetCategoryName();
			TreeNode val2 = null;
			foreach (TreeNode node in ((TreeView)tvDataSources).Nodes)
			{
				TreeNode val3 = node;
				if (val3.Text == categoryName)
				{
					val2 = val3;
					break;
				}
			}
			if (val2 == null)
			{
				val2 = new TreeNode(categoryName);
				val2.ImageKey = text2;
				val2.SelectedImageKey = text2;
				((TreeView)tvDataSources).Nodes.Add(val2);
			}
			val2.Nodes.Add(val);
			if (dataSource.Name == dataSourceName)
			{
				((TreeView)tvDataSources).SelectedNode = val;
			}
		}
		((TreeView)tvDataSources).ExpandAll();
	}

	public void Build(StiBusinessObjectsCollection coll, bool clear)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		if (clear)
		{
			((TreeView)tvDataSources).Nodes.Clear();
		}
		string text = "BusinessObject";
		foreach (StiBusinessObject item in coll)
		{
			TreeNode val = new TreeNode(item.ToString());
			val.ImageKey = text;
			val.SelectedImageKey = text;
			val.Tag = item;
			if (item.BusinessObjects != null && item.BusinessObjects.Count > 0)
			{
				BuildBusinessObject(item.BusinessObjects, val);
			}
			((TreeView)tvDataSources).Nodes.Add(val);
			val.ExpandAll();
		}
	}

	private void BuildBusinessObject(StiBusinessObjectsCollection coll, TreeNode parentNode)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		int num = 8;
		foreach (StiBusinessObject item in coll)
		{
			TreeNode val = new TreeNode(item.ToString(), num, num)
			{
				Tag = item
			};
			if (item.BusinessObjects != null && item.BusinessObjects.Count > 0)
			{
				BuildBusinessObject(item.BusinessObjects, val);
			}
			parentNode.Nodes.Add(val);
		}
	}

	private void TvDataSources_AfterCheck(object sender, TreeViewEventArgs e)
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		if (e.Node.Nodes.Count > 0)
		{
			return;
		}
		TreeNodeCollection nodes = ((TreeView)tvDataSources).Nodes;
		if (e.Node.Parent != null)
		{
			nodes = e.Node.Parent.Nodes;
		}
		nodes.Remove(e.Node);
		int num = 0;
		foreach (TreeNode item in nodes)
		{
			TreeNode val = item;
			if (!val.Checked)
			{
				break;
			}
			num++;
		}
		nodes.Insert(num, e.Node);
		((TreeView)tvDataSources).SelectedNode = e.Node;
	}

	private void TvDataSources_ItemDrag(object sender, ItemDragEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		if (e.Item is TreeNode && ((TreeNode)e.Item).Checked)
		{
			((Control)this).DoDragDrop(e.Item, (DragDropEffects)2);
		}
	}

	private void TvDataSources_DragDrop(object sender, DragEventArgs e)
	{
		object data = e.Data.GetData(typeof(TreeNode));
		TreeNode val = (TreeNode)((data is TreeNode) ? data : null);
		TreeNode selectedNode = ((TreeView)tvDataSources).SelectedNode;
		TreeNodeCollection nodes = ((TreeView)tvDataSources).Nodes;
		if (val.Parent != null)
		{
			nodes = val.Parent.Nodes;
		}
		if (val != selectedNode)
		{
			((TreeView)tvDataSources).BeginUpdate();
			int num = nodes.IndexOf(selectedNode);
			nodes.Remove(val);
			nodes.Insert(num, val);
			((TreeView)tvDataSources).SelectedNode = val;
			((TreeView)tvDataSources).EndUpdate();
		}
	}

	private void TvDataSources_DragEnter(object sender, DragEventArgs e)
	{
		e.Effect = (DragDropEffects)2;
	}

	private void TvDataSources_DragOver(object sender, DragEventArgs e)
	{
		TreeNode val = ((TreeView)tvDataSources).GetNodeAt(((Control)tvDataSources).PointToClient(Cursor.Position));
		if (val != null)
		{
			object data = e.Data.GetData(typeof(TreeNode));
			TreeNode val2 = (TreeNode)((data is TreeNode) ? data : null);
			if (val2.Parent != val.Parent || !val.Checked)
			{
				val = null;
			}
		}
		((TreeView)tvDataSources).SelectedNode = val;
	}

	private void TbNewDataSource_Click(object sender, EventArgs e)
	{
		if (report.Designer != null)
		{
			StiDictionaryPanelService service = StiDictionaryPanelService.GetService(report.Designer.DesignerControl as StiDesignerControl);
			service.InvokeConnectionNew();
			Build(report.DataSources, report);
			Build(report.Dictionary.BusinessObjects, clear: false);
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
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Expected O, but got Unknown
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Expected O, but got Unknown
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Expected O, but got Unknown
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Expected O, but got Unknown
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Expected O, but got Unknown
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Expected O, but got Unknown
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_042f: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiDataSourcePanel));
		tvDataSources = new StiTreeView();
		ilData = new ImageList(components);
		tbTool = new StiToolBar();
		tbNewDataSource = new StiToolButton();
		((Control)tbTool).SuspendLayout();
		((Control)this).SuspendLayout();
		((TreeView)tvDataSources).BorderStyle = (BorderStyle)0;
		((Control)tvDataSources).Dock = (DockStyle)5;
		((TreeView)tvDataSources).HideSelection = false;
		((Control)tvDataSources).Location = new Point(0, 62);
		((Control)tvDataSources).Margin = new Padding(24, 23, 24, 23);
		((Control)tvDataSources).Name = "tvDataSources";
		((Control)tvDataSources).Size = new Size(784, 771);
		((Control)tvDataSources).TabIndex = 3;
		((TreeView)tvDataSources).AfterCheck += new TreeViewEventHandler(TvDataSources_AfterCheck);
		((TreeView)tvDataSources).ItemDrag += new ItemDragEventHandler(TvDataSources_ItemDrag);
		((Control)tvDataSources).DragDrop += new DragEventHandler(TvDataSources_DragDrop);
		((Control)tvDataSources).DragEnter += new DragEventHandler(TvDataSources_DragEnter);
		((Control)tvDataSources).DragOver += new DragEventHandler(TvDataSources_DragOver);
		ilData.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("ilData.ImageStream");
		ilData.TransparentColor = Color.Magenta;
		ilData.Images.SetKeyName(0, "");
		ilData.Images.SetKeyName(1, "");
		ilData.Images.SetKeyName(2, "");
		ilData.Images.SetKeyName(3, "");
		ilData.Images.SetKeyName(4, "DataSourceNew.png");
		ilData.Images.SetKeyName(5, "BusinessObject.png");
		ilData.Images.SetKeyName(6, "Connection.png");
		ilData.Images.SetKeyName(7, "DataSource.png");
		ilData.Images.SetKeyName(8, "BusinessObject.png");
		((Control)tbTool).Controls.Add((Control)(object)tbNewDataSource);
		tbTool.ControlStyle = StiControlStyle.Office2013Blue;
		((Control)tbTool).Dock = (DockStyle)1;
		tbTool.LineStyle = StiToolBar.StiToolBarLineStyle.Bottom;
		((Control)tbTool).Location = new Point(0, 0);
		((Control)tbTool).Margin = new Padding(4, 4, 4, 4);
		((Control)tbTool).Name = "tbTool";
		((Control)tbTool).Padding = new Padding(6, 6, 6, 6);
		((Control)tbTool).Size = new Size(784, 62);
		((Control)tbTool).TabIndex = 4;
		((Control)tbTool).Text = "stiToolBar1";
		((Control)tbNewDataSource).AutoSize = true;
		((Control)tbNewDataSource).Dock = (DockStyle)3;
		tbNewDataSource.Draw3DButton = false;
		tbNewDataSource.DropDownMenu = null;
		tbNewDataSource.HotDragDrop = false;
		((ButtonBase)tbNewDataSource).Image = (Image)componentResourceManager.GetObject("tbNewDataSource.Image");
		((Control)tbNewDataSource).Location = new Point(6, 6);
		((Control)tbNewDataSource).Margin = new Padding(4, 4, 4, 4);
		((Control)tbNewDataSource).Name = "tbNewDataSource";
		((Control)tbNewDataSource).Size = new Size(211, 50);
		((Control)tbNewDataSource).TabIndex = 0;
		((Control)tbNewDataSource).Text = "New Data Source";
		((ButtonBase)tbNewDataSource).UseVisualStyleBackColor = false;
		((Control)tbNewDataSource).Click += TbNewDataSource_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)tvDataSources);
		((Control)this).Controls.Add((Control)(object)tbTool);
		((Control)this).Margin = new Padding(4, 4, 4, 4);
		((Control)this).Name = "StiDataSourcePanel";
		((Control)this).Size = new Size(784, 833);
		((Control)tbTool).ResumeLayout(false);
		((Control)tbTool).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
