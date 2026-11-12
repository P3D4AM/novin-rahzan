using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Design.Panels;
using Stimulsoft.Report.Design.Wizards;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Design.CrossTab;

[ToolboxItem(false)]
public class StiCrossTabDataControl : UserControl
{
	private bool lockBuild;

	private StiDataRelation selectedRelation;

	private StiDataRelationsCollection relations;

	private StiDataBuilder dataBuilder = new StiDataBuilder();

	private StiCrossTab crossTab;

	private IContainer components;

	private StiColumnsSort columnsBusinessObjectSort;

	private StiColumnsFilters filtersBusinessObjectControl;

	private StiColumnsSort columnsDataSourceSort;

	private StiColumnsFilters filtersDataSourceControl;

	private TreeView tvRelations;

	private ImageList ilDataImages;

	public StiToolBar tbTool;

	private StiToolButton tbNewDataSource;

	public StiToolBar stiToolBar1;

	private StiToolButton tbNewDataRelation;

	private StiToolButton tbNewBusinessObject;

	internal TreeView tvDataSources;

	internal StiCloudTabulator TabControl;

	internal StiCloudTabulatorPage pageDataSource;

	internal StiCloudTabulatorPage pageDataSourceDataRelation;

	internal StiCloudTabulatorPage pageDataSourceSort;

	internal StiCloudTabulatorPage pageBusinessObjectSort;

	internal StiCloudTabulatorPage pageBusinessObjectFilters;

	internal StiCloudTabulatorPage pageDataSourceFilters;

	private Timer timerMarker;

	internal StiCrossTabDataSourceForm.StartTab? StartTab { get; set; }

	private StiReport Report => crossTab.Page.Report;

	public event EventHandler NextPage;

	public event EventHandler UpdateDataSource;

	public StiCrossTabDataControl()
	{
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			tvDataSources.ImageList = StiDataBuilder.Images;
			tvRelations.ImageList = StiDataBuilder.Images;
			Localize();
			DoScaling();
			((Control)tbNewDataSource).Enabled = StiOptions.Designer.Panels.Dictionary.ShowDataSourceNewMenuItem;
			((Control)tbNewBusinessObject).Enabled = StiOptions.Designer.Panels.Dictionary.ShowBusinessObjectNewMenuItem;
			((Control)tbNewDataRelation).Enabled = StiOptions.Designer.Panels.Dictionary.ShowRelationNewMenuItem;
		}
	}

	private void Localize()
	{
		((Control)pageDataSource).Text = StiLocalization.Get("PropertyMain", "DataSource");
		((Control)pageDataSourceDataRelation).Text = StiLocalization.Get("PropertyMain", "Relation");
		((Control)pageDataSourceSort).Text = StiLocalization.Get("PropertyMain", "Sort");
		((Control)pageDataSourceFilters).Text = StiLocalization.Get("PropertyMain", "Filters");
		((Control)pageBusinessObjectSort).Text = StiLocalization.Get("PropertyMain", "Sort");
		((Control)pageBusinessObjectFilters).Text = StiLocalization.Get("PropertyMain", "Filters");
		((Control)tbNewDataSource).Text = StiLocalization.Get("FormDictionaryDesigner", "DataSourceNew");
		((Control)tbNewBusinessObject).Text = StiLocalization.Get("FormDictionaryDesigner", "NewBusinessObject");
		((Control)tbNewDataRelation).Text = StiLocalization.Get("FormDictionaryDesigner", "RelationNew");
	}

	private void DoScaling()
	{
		DockPaddingEdges dockPadding = TabControl.DockPadding;
		int right = (TabControl.TitleWidth = StiScale.XXI(110.0));
		dockPadding.Right = right;
		pageBusinessObjectFilters.Image = StiImageResizer.GetImage(pageBusinessObjectFilters.Image);
		pageBusinessObjectSort.Image = StiImageResizer.GetImage(pageBusinessObjectSort.Image);
		pageDataSource.Image = StiImageResizer.GetImage(pageDataSource.Image);
		pageDataSourceDataRelation.Image = StiImageResizer.GetImage(pageDataSourceDataRelation.Image);
		pageDataSourceFilters.Image = StiImageResizer.GetImage(pageDataSourceFilters.Image);
		pageDataSourceSort.Image = StiImageResizer.GetImage(pageDataSourceSort.Image);
		((ButtonBase)tbNewBusinessObject).Image = StiReportDesignImages.Actions.NewBusinessObject();
		((ButtonBase)tbNewDataRelation).Image = StiReportDesignImages.Actions.NewDataRelation();
		((ButtonBase)tbNewDataSource).Image = StiReportDesignImages.Actions.NewDataSource();
	}

	internal void SetCrossTab(StiCrossTab crossTab)
	{
		this.crossTab = crossTab;
		Build();
		filtersDataSourceControl.Report = Report;
		filtersBusinessObjectControl.Report = Report;
		pageDataSourceSort.Invisible = true;
	}

	private void BuildDataSources()
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Expected O, but got Unknown
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Expected O, but got Unknown
		lockBuild = true;
		StiDataSourcesCollection dataSources = Report.Dictionary.DataSources;
		tvDataSources.Nodes.Clear();
		tvDataSources.Nodes.Add(new TreeNode("[" + StiLocalization.Get("Report", "NotAssigned") + "]", 0, 0));
		foreach (StiDataSource item in dataSources)
		{
			TreeNode val = new TreeNode(item.ToString());
			string imageKey = (val.SelectedImageKey = "DataSource");
			val.ImageKey = imageKey;
			val.Tag = item;
			string categoryName = item.GetCategoryName();
			TreeNode val2 = null;
			foreach (TreeNode node in tvDataSources.Nodes)
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
				TreeNode obj = val2;
				imageKey = (val2.SelectedImageKey = "Category");
				obj.ImageKey = imageKey;
				tvDataSources.Nodes.Add(val2);
			}
			val2.Nodes.Add(val);
			if (item == crossTab.DataSource)
			{
				tvDataSources.SelectedNode = val;
			}
		}
		if (Report.Dictionary.BusinessObjects.Count > 0)
		{
			TreeNode val4 = new TreeNode(StiLocalization.Get("Report", "BusinessObjects"));
			string imageKey = (val4.SelectedImageKey = "BusinessObject");
			val4.ImageKey = imageKey;
			tvDataSources.Nodes.Add(val4);
			dataBuilder.BuildBusinessObjects(Report.Dictionary.BusinessObjects, val4, includeColumns: false, includeCalcColumns: false);
			if (tvDataSources.SelectedNode == null)
			{
				SelectNode(crossTab, tvDataSources.Nodes);
			}
		}
		if (tvDataSources.SelectedNode == null)
		{
			tvDataSources.SelectedNode = tvDataSources.Nodes[0];
		}
		tvDataSources.ExpandAll();
		lockBuild = false;
	}

	private void SelectNode(IStiBusinessObject businessObject, TreeNodeCollection nodes)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		if (string.IsNullOrEmpty(crossTab.BusinessObjectGuid))
		{
			return;
		}
		foreach (TreeNode node in nodes)
		{
			TreeNode val = node;
			if (val.Tag is StiBusinessObject && ((StiBusinessObject)val.Tag).Guid == businessObject.BusinessObjectGuid)
			{
				tvDataSources.SelectedNode = val;
				break;
			}
			if (val.Nodes.Count > 0 && !val.IsExpanded)
			{
				val.Expand();
			}
			SelectNode(businessObject, val.Nodes);
		}
	}

	private void BuildDataRelations()
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Expected O, but got Unknown
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Expected O, but got Unknown
		StiDataSource selectedDataSource = GetSelectedDataSource();
		relations = ((selectedDataSource != null) ? selectedDataSource.GetParentRelations() : new StiDataRelationsCollection(Report.Dictionary));
		tvRelations.Nodes.Clear();
		tvRelations.Nodes.Add(new TreeNode("[" + Loc.Get("Report", "NotAssigned") + "]", 0, 0));
		tvRelations.SelectedNode = tvRelations.Nodes[0];
		string text = "DataRelation";
		string text2 = "Database";
		foreach (StiDataRelation relation in relations)
		{
			TreeNode val = new TreeNode(relation.ToString())
			{
				ImageKey = text,
				SelectedImageKey = text,
				Tag = relation
			};
			StiDataSource stiDataSource = relation.ChildSource;
			if (stiDataSource == null)
			{
				stiDataSource = relation.ParentSource;
			}
			if (stiDataSource != null)
			{
				string categoryName = stiDataSource.GetCategoryName();
				TreeNode val2 = null;
				foreach (TreeNode node in tvRelations.Nodes)
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
					tvRelations.Nodes.Add(val2);
				}
				val2.Nodes.Add(val);
			}
			else
			{
				tvRelations.Nodes.Add(val);
			}
			if (relation == selectedRelation)
			{
				tvRelations.SelectedNode = val;
			}
		}
		tvRelations.ExpandAll();
	}

	private void BuildSort()
	{
		columnsDataSourceSort.Sort = crossTab.Sort;
		columnsDataSourceSort.DataSource = crossTab.DataSource;
		columnsBusinessObjectSort.BusinessObject = crossTab.BusinessObject;
		columnsBusinessObjectSort.Sort = crossTab.Sort;
	}

	private void BuildFilters()
	{
		filtersDataSourceControl.DataSource = crossTab.DataSource;
		filtersDataSourceControl.BuildFilters(crossTab.Filters);
		((RadioButton)filtersDataSourceControl.rbAnd).Checked = crossTab.FilterMode == StiFilterMode.And;
		((RadioButton)filtersDataSourceControl.rbOr).Checked = crossTab.FilterMode == StiFilterMode.Or;
		((Control)filtersDataSourceControl.comboBoxFilterEngine).Enabled = false;
		((CheckBox)filtersDataSourceControl.cbFilterOn).Checked = crossTab.FilterOn;
		filtersBusinessObjectControl.BusinessObject = crossTab.BusinessObject;
		filtersBusinessObjectControl.BuildFilters(crossTab.Filters);
		((RadioButton)filtersBusinessObjectControl.rbAnd).Checked = crossTab.FilterMode == StiFilterMode.And;
		((RadioButton)filtersBusinessObjectControl.rbOr).Checked = crossTab.FilterMode == StiFilterMode.Or;
		((Control)filtersBusinessObjectControl.comboBoxFilterEngine).Enabled = false;
		((CheckBox)filtersBusinessObjectControl.cbFilterOn).Checked = crossTab.FilterOn;
	}

	private void Build()
	{
		BuildDataSources();
		if (!StiOptions.Designer.DataBandEditor.DataSourceVisible)
		{
			pageDataSource.Invisible = true;
		}
		selectedRelation = crossTab.DataRelation;
		BuildDataRelations();
		BuildSort();
		BuildFilters();
	}

	public StiDataSource GetSelectedDataSource()
	{
		if (tvDataSources.SelectedNode != null)
		{
			return tvDataSources.SelectedNode.Tag as StiDataSource;
		}
		return null;
	}

	public StiBusinessObject GetSelectedBusinessObject()
	{
		if (!pageDataSource.Invisible && tvDataSources.SelectedNode != null)
		{
			return tvDataSources.SelectedNode.Tag as StiBusinessObject;
		}
		return null;
	}

	public void SaveData()
	{
		timerMarker.Stop();
		crossTab.Report.Designer.UndoRedoSave(StiLocalization.Get("Components", "StiDataBand"), onlyCurrentPage: true);
		StiDataSource selectedDataSource = GetSelectedDataSource();
		if (selectedDataSource != null)
		{
			crossTab.DataSourceName = selectedDataSource.Name;
		}
		else
		{
			crossTab.DataSourceName = string.Empty;
		}
		if (tvRelations.SelectedNode.Tag != null)
		{
			crossTab.DataRelationName = ((StiDataRelation)tvRelations.SelectedNode.Tag).NameInSource;
		}
		else
		{
			crossTab.DataRelationName = null;
		}
		if (!pageDataSourceSort.Invisible)
		{
			crossTab.Sort = columnsDataSourceSort.Sort;
		}
		if (!pageDataSourceFilters.Invisible)
		{
			crossTab.FilterMode = ((!((RadioButton)filtersDataSourceControl.rbAnd).Checked) ? StiFilterMode.Or : StiFilterMode.And);
			filtersDataSourceControl.FillFilters(crossTab.Filters);
			crossTab.FilterOn = ((CheckBox)filtersDataSourceControl.cbFilterOn).Checked;
		}
		StiBusinessObject selectedBusinessObject = GetSelectedBusinessObject();
		if (selectedBusinessObject != null)
		{
			crossTab.BusinessObjectGuid = selectedBusinessObject.Guid;
		}
		else
		{
			crossTab.BusinessObjectGuid = string.Empty;
		}
		if (!pageBusinessObjectSort.Invisible)
		{
			crossTab.Sort = columnsBusinessObjectSort.Sort;
		}
		if (!pageBusinessObjectFilters.Invisible)
		{
			crossTab.FilterMode = ((!((RadioButton)filtersBusinessObjectControl.rbAnd).Checked) ? StiFilterMode.Or : StiFilterMode.And);
			filtersBusinessObjectControl.FillFilters(crossTab.Filters);
			crossTab.FilterOn = ((CheckBox)filtersBusinessObjectControl.cbFilterOn).Checked;
		}
	}

	public void InvokeNextPage()
	{
		this.NextPage?.Invoke(this, EventArgs.Empty);
	}

	public void InvokeUpdateDataSource()
	{
		this.UpdateDataSource?.Invoke(this, EventArgs.Empty);
	}

	private void ButtonNewDataSource_Click(object sender, EventArgs e)
	{
		if (Report.Designer != null)
		{
			StiDictionaryPanelService service = StiDictionaryPanelService.GetService(Report.Designer.DesignerControl as StiDesignerControl);
			service.InvokeConnectionNew();
			Build();
		}
	}

	private void ButtonNewBusinessObject_Click(object sender, EventArgs e)
	{
		if (Report.Designer != null)
		{
			StiDictionaryPanelService service = StiDictionaryPanelService.GetService(Report.Designer.DesignerControl as StiDesignerControl);
			service.InvokeBusinessObjectNew();
			Build();
		}
	}

	private void TreeViewDataSources_DoubleClick(object sender, EventArgs e)
	{
		StiCrossTabDataSourceForm stiCrossTabDataSourceForm = ((Control)this).FindForm() as StiCrossTabDataSourceForm;
		stiCrossTabDataSourceForm.Save();
		((Form)stiCrossTabDataSourceForm).Close();
	}

	private void TreeViewDataSources_AfterSelect(object sender, TreeViewEventArgs e)
	{
		StiDataSource selectedDataSource = GetSelectedDataSource();
		StiBusinessObject selectedBusinessObject = GetSelectedBusinessObject();
		if (selectedBusinessObject != null)
		{
			pageDataSourceDataRelation.Invisible = true;
			pageDataSourceSort.Invisible = true;
			pageDataSourceFilters.Invisible = true;
			pageBusinessObjectSort.Invisible = false;
			pageBusinessObjectFilters.Invisible = false;
			filtersDataSourceControl.DataSource = null;
			filtersBusinessObjectControl.BusinessObject = selectedBusinessObject;
			columnsDataSourceSort.DataSource = null;
			columnsBusinessObjectSort.BusinessObject = selectedBusinessObject;
		}
		else
		{
			pageDataSourceDataRelation.Invisible = false;
			pageDataSourceSort.Invisible = false;
			pageDataSourceFilters.Invisible = false;
			pageBusinessObjectSort.Invisible = true;
			pageBusinessObjectFilters.Invisible = true;
			columnsBusinessObjectSort.BusinessObject = null;
			columnsDataSourceSort.DataSource = selectedDataSource;
			filtersBusinessObjectControl.BusinessObject = null;
			filtersDataSourceControl.DataSource = selectedDataSource;
		}
		if (!lockBuild)
		{
			InvokeUpdateDataSource();
		}
	}

	private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (TabControl.SelectedIndex == 1)
		{
			BuildDataRelations();
		}
	}

	private void TreeViewRelations_AfterSelect(object sender, TreeViewEventArgs e)
	{
		if (tvRelations.SelectedNode != null && tvRelations.SelectedNode.Tag is StiDataRelation)
		{
			selectedRelation = tvRelations.SelectedNode.Tag as StiDataRelation;
		}
	}

	private void ButtonNewDataRelation_Click(object sender, EventArgs e)
	{
		if (Report.Designer != null)
		{
			StiDictionaryPanelService service = StiDictionaryPanelService.GetService(Report.Designer.DesignerControl as StiDesignerControl);
			service.InvokeRelationNew();
			Build();
		}
	}

	private void TreeViewDataSources_BeforeExpand(object sender, TreeViewCancelEventArgs e)
	{
		if (e.Node is TreeNodeEx node && e.Node.Tag != null)
		{
			object tag = e.Node.Tag;
			if (tag is StiBusinessObject)
			{
				dataBuilder.ExpandedNode(node, includeColumn: false, includeCalcColumn: false);
			}
		}
	}

	private void TimerMarker_Tick(object sender, EventArgs e)
	{
		try
		{
			StiCloudTabulatorPage stiCloudTabulatorPage = pageDataSource;
			TreeNode selectedNode = tvDataSources.SelectedNode;
			stiCloudTabulatorPage.ShowMarker = ((selectedNode != null) ? selectedNode.Tag : null) is StiDataSource;
			StiCloudTabulatorPage stiCloudTabulatorPage2 = pageDataSourceDataRelation;
			TreeNode selectedNode2 = tvRelations.SelectedNode;
			stiCloudTabulatorPage2.ShowMarker = ((selectedNode2 != null) ? selectedNode2.Tag : null) is StiDataRelation;
			pageDataSourceFilters.ShowMarker = ((ArrangedElementCollection)((Control)filtersDataSourceControl.ZoneContainerControl).Controls).Count > 0;
			pageDataSourceSort.ShowMarker = columnsDataSourceSort.Sort != null && columnsDataSourceSort.Sort.Length != 0;
			pageBusinessObjectFilters.ShowMarker = ((ArrangedElementCollection)((Control)filtersBusinessObjectControl.ZoneContainerControl).Controls).Count > 0;
			pageBusinessObjectSort.ShowMarker = columnsBusinessObjectSort.Sort != null && columnsBusinessObjectSort.Sort.Length != 0;
		}
		catch
		{
		}
	}

	private void This_Load(object sender, EventArgs e)
	{
		if (!StartTab.HasValue)
		{
			TabControl.SelectedIndex = 0;
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
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Expected O, but got Unknown
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f5: Expected O, but got Unknown
		//IL_0502: Unknown result type (might be due to invalid IL or missing references)
		//IL_050c: Expected O, but got Unknown
		//IL_05a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_062a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0634: Expected O, but got Unknown
		//IL_06fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0704: Expected O, but got Unknown
		//IL_07c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cd: Expected O, but got Unknown
		//IL_0805: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ff: Expected O, but got Unknown
		//IL_0966: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fa: Expected O, but got Unknown
		//IL_0aa3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aad: Expected O, but got Unknown
		//IL_0ad2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c18: Expected O, but got Unknown
		//IL_0c3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cfa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d79: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d83: Expected O, but got Unknown
		//IL_0dbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e54: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eed: Expected O, but got Unknown
		//IL_0f25: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fbe: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiCrossTabDataControl));
		ilDataImages = new ImageList(components);
		TabControl = new StiCloudTabulator();
		pageDataSource = new StiCloudTabulatorPage();
		tvDataSources = new TreeView();
		tbTool = new StiToolBar();
		tbNewBusinessObject = new StiToolButton();
		tbNewDataSource = new StiToolButton();
		pageDataSourceDataRelation = new StiCloudTabulatorPage();
		tvRelations = new TreeView();
		stiToolBar1 = new StiToolBar();
		tbNewDataRelation = new StiToolButton();
		pageDataSourceSort = new StiCloudTabulatorPage();
		columnsDataSourceSort = new StiColumnsSort();
		pageDataSourceFilters = new StiCloudTabulatorPage();
		filtersDataSourceControl = new StiColumnsFilters();
		pageBusinessObjectSort = new StiCloudTabulatorPage();
		columnsBusinessObjectSort = new StiColumnsSort();
		pageBusinessObjectFilters = new StiCloudTabulatorPage();
		filtersBusinessObjectControl = new StiColumnsFilters();
		timerMarker = new Timer(components);
		((Control)TabControl).SuspendLayout();
		((Control)pageDataSource).SuspendLayout();
		((Control)tbTool).SuspendLayout();
		((Control)pageDataSourceDataRelation).SuspendLayout();
		((Control)stiToolBar1).SuspendLayout();
		((Control)pageDataSourceSort).SuspendLayout();
		((Control)pageDataSourceFilters).SuspendLayout();
		((Control)pageBusinessObjectSort).SuspendLayout();
		((Control)pageBusinessObjectFilters).SuspendLayout();
		((Control)this).SuspendLayout();
		ilDataImages.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("ilDataImages.ImageStream");
		ilDataImages.TransparentColor = Color.Magenta;
		ilDataImages.Images.SetKeyName(0, "");
		ilDataImages.Images.SetKeyName(1, "");
		ilDataImages.Images.SetKeyName(2, "");
		ilDataImages.Images.SetKeyName(3, "");
		ilDataImages.Images.SetKeyName(4, "");
		ilDataImages.Images.SetKeyName(5, "Connection.png");
		ilDataImages.Images.SetKeyName(6, "DataSource.png");
		ilDataImages.Images.SetKeyName(7, "Relation.png");
		((Control)TabControl).Controls.Add((Control)(object)pageDataSource);
		((Control)TabControl).Controls.Add((Control)(object)pageDataSourceDataRelation);
		((Control)TabControl).Controls.Add((Control)(object)pageDataSourceSort);
		((Control)TabControl).Controls.Add((Control)(object)pageDataSourceFilters);
		((Control)TabControl).Controls.Add((Control)(object)pageBusinessObjectSort);
		((Control)TabControl).Controls.Add((Control)(object)pageBusinessObjectFilters);
		((Control)TabControl).Dock = (DockStyle)5;
		((Control)TabControl).Location = new Point(0, 0);
		((Control)TabControl).Name = "TabControl";
		((Control)TabControl).Padding = new Padding(0, 0, 119, 0);
		TabControl.ScrollHeight = 0;
		TabControl.ScrollWidth = 100;
		((Control)TabControl).Size = new Size(488, 403);
		((Control)TabControl).TabIndex = 0;
		TabControl.TitleBackColor = SystemColors.Control;
		TabControl.TitleInLine = false;
		TabControl.TitlePosition = StiTabTitlePosition.RightHorizontal;
		TabControl.TitleWidth = 110;
		TabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
		((Control)pageDataSource).Controls.Add((Control)(object)tvDataSources);
		((Control)pageDataSource).Controls.Add((Control)(object)tbTool);
		pageDataSource.Dock = (DockStyle)5;
		pageDataSource.Image = (Image)componentResourceManager.GetObject("pageDataSource.Image");
		pageDataSource.InvisibleVisible = true;
		((Control)pageDataSource).Location = new Point(0, 0);
		((Control)pageDataSource).Name = "pageDataSource";
		((Control)pageDataSource).Padding = new Padding(0, 0, 1, 0);
		((Control)pageDataSource).Size = new Size(369, 403);
		((Control)pageDataSource).TabIndex = 0;
		((Control)pageDataSource).Text = "DataSource";
		((Control)pageDataSource).Visible = false;
		((Control)tvDataSources).Anchor = (AnchorStyles)15;
		tvDataSources.BorderStyle = (BorderStyle)0;
		tvDataSources.HideSelection = false;
		((Control)tvDataSources).Location = new Point(3, 31);
		((Control)tvDataSources).Name = "tvDataSources";
		((Control)tvDataSources).Size = new Size(362, 369);
		((Control)tvDataSources).TabIndex = 2;
		tvDataSources.BeforeExpand += new TreeViewCancelEventHandler(TreeViewDataSources_BeforeExpand);
		tvDataSources.AfterSelect += new TreeViewEventHandler(TreeViewDataSources_AfterSelect);
		((Control)tvDataSources).DoubleClick += TreeViewDataSources_DoubleClick;
		((Control)tbTool).Controls.Add((Control)(object)tbNewBusinessObject);
		((Control)tbTool).Controls.Add((Control)(object)tbNewDataSource);
		tbTool.ControlStyle = StiControlStyle.Office2013Blue;
		((Control)tbTool).Dock = (DockStyle)1;
		tbTool.LineStyle = StiToolBar.StiToolBarLineStyle.Bottom;
		((Control)tbTool).Location = new Point(0, 0);
		((Control)tbTool).Name = "tbTool";
		((Control)tbTool).Padding = new Padding(12, 3, 3, 3);
		((Control)tbTool).Size = new Size(368, 28);
		((Control)tbTool).TabIndex = 4;
		((Control)tbTool).Text = "stiToolBar1";
		((Control)tbNewBusinessObject).AutoSize = true;
		((Control)tbNewBusinessObject).Dock = (DockStyle)3;
		tbNewBusinessObject.Draw3DButton = false;
		tbNewBusinessObject.DropDownMenu = null;
		tbNewBusinessObject.HotDragDrop = false;
		((ButtonBase)tbNewBusinessObject).Image = (Image)componentResourceManager.GetObject("tbNewBusinessObject.Image");
		((Control)tbNewBusinessObject).Location = new Point(130, 3);
		((Control)tbNewBusinessObject).Name = "tbNewBusinessObject";
		((Control)tbNewBusinessObject).Size = new Size(118, 22);
		((Control)tbNewBusinessObject).TabIndex = 2;
		((Control)tbNewBusinessObject).Text = "New Data Source";
		((ButtonBase)tbNewBusinessObject).UseVisualStyleBackColor = false;
		((Control)tbNewBusinessObject).Click += ButtonNewBusinessObject_Click;
		((Control)tbNewDataSource).AutoSize = true;
		((Control)tbNewDataSource).Dock = (DockStyle)3;
		tbNewDataSource.Draw3DButton = false;
		tbNewDataSource.DropDownMenu = null;
		tbNewDataSource.HotDragDrop = false;
		((ButtonBase)tbNewDataSource).Image = (Image)componentResourceManager.GetObject("tbNewDataSource.Image");
		((Control)tbNewDataSource).Location = new Point(12, 3);
		((Control)tbNewDataSource).Name = "tbNewDataSource";
		((Control)tbNewDataSource).Size = new Size(118, 22);
		((Control)tbNewDataSource).TabIndex = 1;
		((Control)tbNewDataSource).Text = "New Data Source";
		((ButtonBase)tbNewDataSource).UseVisualStyleBackColor = false;
		((Control)tbNewDataSource).Click += ButtonNewDataSource_Click;
		((Control)pageDataSourceDataRelation).Controls.Add((Control)(object)tvRelations);
		((Control)pageDataSourceDataRelation).Controls.Add((Control)(object)stiToolBar1);
		pageDataSourceDataRelation.Dock = (DockStyle)5;
		pageDataSourceDataRelation.Image = (Image)componentResourceManager.GetObject("pageDataSourceDataRelation.Image");
		pageDataSourceDataRelation.InvisibleVisible = true;
		((Control)pageDataSourceDataRelation).Location = new Point(0, 0);
		((Control)pageDataSourceDataRelation).Name = "pageDataSourceDataRelation";
		((Control)pageDataSourceDataRelation).Padding = new Padding(0, 0, 1, 0);
		((Control)pageDataSourceDataRelation).Size = new Size(369, 403);
		((Control)pageDataSourceDataRelation).TabIndex = 1;
		((Control)pageDataSourceDataRelation).Text = "DataRelation";
		((Control)pageDataSourceDataRelation).Visible = false;
		((Control)tvRelations).Anchor = (AnchorStyles)15;
		tvRelations.BorderStyle = (BorderStyle)0;
		tvRelations.HideSelection = false;
		tvRelations.ImageIndex = 0;
		tvRelations.ImageList = ilDataImages;
		((Control)tvRelations).Location = new Point(3, 31);
		((Control)tvRelations).Name = "tvRelations";
		tvRelations.SelectedImageIndex = 0;
		((Control)tvRelations).Size = new Size(362, 369);
		((Control)tvRelations).TabIndex = 3;
		tvRelations.AfterSelect += new TreeViewEventHandler(TreeViewRelations_AfterSelect);
		((Control)stiToolBar1).Controls.Add((Control)(object)tbNewDataRelation);
		stiToolBar1.ControlStyle = StiControlStyle.Office2013Blue;
		((Control)stiToolBar1).Dock = (DockStyle)1;
		stiToolBar1.LineStyle = StiToolBar.StiToolBarLineStyle.Bottom;
		((Control)stiToolBar1).Location = new Point(0, 0);
		((Control)stiToolBar1).Name = "stiToolBar1";
		((Control)stiToolBar1).Padding = new Padding(12, 3, 3, 3);
		((Control)stiToolBar1).Size = new Size(368, 28);
		((Control)stiToolBar1).TabIndex = 5;
		((Control)stiToolBar1).Text = "stiToolBar1";
		((Control)tbNewDataRelation).AutoSize = true;
		((Control)tbNewDataRelation).Dock = (DockStyle)3;
		tbNewDataRelation.Draw3DButton = false;
		tbNewDataRelation.DropDownMenu = null;
		tbNewDataRelation.HotDragDrop = false;
		((ButtonBase)tbNewDataRelation).Image = (Image)componentResourceManager.GetObject("tbNewDataRelation.Image");
		((Control)tbNewDataRelation).Location = new Point(12, 3);
		((Control)tbNewDataRelation).Name = "tbNewDataRelation";
		((Control)tbNewDataRelation).Size = new Size(123, 22);
		((Control)tbNewDataRelation).TabIndex = 1;
		((Control)tbNewDataRelation).Text = "New Data Relation";
		((ButtonBase)tbNewDataRelation).UseVisualStyleBackColor = false;
		((Control)tbNewDataRelation).Click += ButtonNewDataRelation_Click;
		((Control)pageDataSourceSort).Controls.Add((Control)(object)columnsDataSourceSort);
		pageDataSourceSort.Dock = (DockStyle)5;
		pageDataSourceSort.Image = (Image)componentResourceManager.GetObject("pageDataSourceSort.Image");
		pageDataSourceSort.InvisibleVisible = true;
		((Control)pageDataSourceSort).Location = new Point(0, 0);
		((Control)pageDataSourceSort).Margin = new Padding(0);
		((Control)pageDataSourceSort).Name = "pageDataSourceSort";
		((Control)pageDataSourceSort).Padding = new Padding(0, 0, 1, 0);
		((Control)pageDataSourceSort).Size = new Size(369, 403);
		((Control)pageDataSourceSort).TabIndex = 3;
		((Control)pageDataSourceSort).Text = "Sort";
		((Control)pageDataSourceSort).Visible = false;
		((Control)columnsDataSourceSort).BackColor = Color.White;
		columnsDataSourceSort.BusinessObject = null;
		columnsDataSourceSort.DataSource = null;
		((Control)columnsDataSourceSort).Dock = (DockStyle)5;
		((Control)columnsDataSourceSort).Location = new Point(0, 0);
		((Control)columnsDataSourceSort).Margin = new Padding(0);
		((Control)columnsDataSourceSort).Name = "columnsDataSourceSort";
		((Control)columnsDataSourceSort).Size = new Size(368, 403);
		columnsDataSourceSort.Sort = null;
		((Control)columnsDataSourceSort).TabIndex = 0;
		((Control)pageDataSourceFilters).Controls.Add((Control)(object)filtersDataSourceControl);
		pageDataSourceFilters.Dock = (DockStyle)5;
		pageDataSourceFilters.Image = (Image)componentResourceManager.GetObject("pageDataSourceFilters.Image");
		pageDataSourceFilters.InvisibleVisible = true;
		((Control)pageDataSourceFilters).Location = new Point(0, 0);
		((Control)pageDataSourceFilters).Margin = new Padding(0);
		((Control)pageDataSourceFilters).Name = "pageDataSourceFilters";
		((Control)pageDataSourceFilters).Padding = new Padding(0, 0, 1, 0);
		((Control)pageDataSourceFilters).Size = new Size(369, 403);
		((Control)pageDataSourceFilters).TabIndex = 4;
		((Control)pageDataSourceFilters).Text = "Filters";
		((Control)pageDataSourceFilters).Visible = false;
		((Control)filtersDataSourceControl).BackColor = Color.White;
		filtersDataSourceControl.BusinessObject = null;
		filtersDataSourceControl.DataSource = null;
		((Control)filtersDataSourceControl).Dock = (DockStyle)5;
		((Control)filtersDataSourceControl).Location = new Point(0, 0);
		((Control)filtersDataSourceControl).Margin = new Padding(0);
		((Control)filtersDataSourceControl).Name = "filtersDataSourceControl";
		((Control)filtersDataSourceControl).Size = new Size(368, 403);
		((Control)filtersDataSourceControl).TabIndex = 2;
		((Control)filtersDataSourceControl).TabStop = false;
		((Control)pageBusinessObjectSort).Controls.Add((Control)(object)columnsBusinessObjectSort);
		pageBusinessObjectSort.Dock = (DockStyle)5;
		pageBusinessObjectSort.Image = (Image)componentResourceManager.GetObject("pageBusinessObjectSort.Image");
		pageBusinessObjectSort.InvisibleVisible = true;
		((Control)pageBusinessObjectSort).Location = new Point(0, 0);
		((Control)pageBusinessObjectSort).Name = "pageBusinessObjectSort";
		((Control)pageBusinessObjectSort).Padding = new Padding(0, 4, 1, 4);
		((Control)pageBusinessObjectSort).Size = new Size(369, 403);
		((Control)pageBusinessObjectSort).TabIndex = 3;
		((Control)pageBusinessObjectSort).Text = "Sort";
		((Control)pageBusinessObjectSort).Visible = false;
		((Control)columnsBusinessObjectSort).BackColor = Color.White;
		columnsBusinessObjectSort.BusinessObject = null;
		columnsBusinessObjectSort.DataSource = null;
		((Control)columnsBusinessObjectSort).Dock = (DockStyle)5;
		((Control)columnsBusinessObjectSort).Location = new Point(0, 4);
		((Control)columnsBusinessObjectSort).Margin = new Padding(0);
		((Control)columnsBusinessObjectSort).Name = "columnsBusinessObjectSort";
		((Control)columnsBusinessObjectSort).Size = new Size(368, 395);
		columnsBusinessObjectSort.Sort = null;
		((Control)columnsBusinessObjectSort).TabIndex = 0;
		((Control)pageBusinessObjectFilters).BackColor = Color.White;
		((Control)pageBusinessObjectFilters).Controls.Add((Control)(object)filtersBusinessObjectControl);
		pageBusinessObjectFilters.Dock = (DockStyle)5;
		pageBusinessObjectFilters.Image = (Image)componentResourceManager.GetObject("pageBusinessObjectFilters.Image");
		pageBusinessObjectFilters.InvisibleVisible = true;
		((Control)pageBusinessObjectFilters).Location = new Point(0, 0);
		((Control)pageBusinessObjectFilters).Name = "pageBusinessObjectFilters";
		((Control)pageBusinessObjectFilters).Padding = new Padding(0, 4, 1, 4);
		((Control)pageBusinessObjectFilters).Size = new Size(369, 403);
		((Control)pageBusinessObjectFilters).TabIndex = 4;
		((Control)pageBusinessObjectFilters).Text = "Filters";
		((Control)pageBusinessObjectFilters).Visible = false;
		((Control)filtersBusinessObjectControl).BackColor = Color.White;
		filtersBusinessObjectControl.BusinessObject = null;
		filtersBusinessObjectControl.DataSource = null;
		((Control)filtersBusinessObjectControl).Dock = (DockStyle)5;
		((Control)filtersBusinessObjectControl).Location = new Point(0, 4);
		((Control)filtersBusinessObjectControl).Margin = new Padding(0);
		((Control)filtersBusinessObjectControl).Name = "filtersBusinessObjectControl";
		((Control)filtersBusinessObjectControl).Size = new Size(368, 395);
		((Control)filtersBusinessObjectControl).TabIndex = 2;
		((Control)filtersBusinessObjectControl).TabStop = false;
		timerMarker.Enabled = true;
		timerMarker.Interval = 500;
		timerMarker.Tick += TimerMarker_Tick;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)TabControl);
		((Control)this).Name = "StiCrossTabDataControl";
		((Control)this).Size = new Size(488, 403);
		((UserControl)this).Load += This_Load;
		((Control)TabControl).ResumeLayout(false);
		((Control)pageDataSource).ResumeLayout(false);
		((Control)tbTool).ResumeLayout(false);
		((Control)tbTool).PerformLayout();
		((Control)pageDataSourceDataRelation).ResumeLayout(false);
		((Control)stiToolBar1).ResumeLayout(false);
		((Control)stiToolBar1).PerformLayout();
		((Control)pageDataSourceSort).ResumeLayout(false);
		((Control)pageDataSourceFilters).ResumeLayout(false);
		((Control)pageBusinessObjectSort).ResumeLayout(false);
		((Control)pageBusinessObjectFilters).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
