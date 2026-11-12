using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Design.Panels;
using Stimulsoft.Report.Design.Wizards;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Components.Design;

[ToolboxItem(false)]
public class StiDataSelectionControl : UserControl
{
	public enum StartTab
	{
		DataSource,
		DataRelation,
		MasterComponent,
		Sort,
		Filters
	}

	private bool pageRelationsFirstInit;

	private StiDataSource lastDataSourcePageRelations;

	private StiDataRelationsCollection relations;

	private StiDataBuilder dataBuilder = new StiDataBuilder();

	private Hashtable hashMasterComponentIcons = new Hashtable();

	private StiComponent component;

	private IContainer components;

	private StiCloudTabulator TabControl;

	private StiCloudTabulatorPage pageDataSource;

	private StiCloudTabulatorPage pageDataSourceDataRelation;

	private StiCloudTabulatorPage pageDataSourceMasterComponent;

	private StiCloudTabulatorPage pageDataSourceSort;

	private StiCloudTabulatorPage pageDataSourceFilters;

	private StiCloudTabulatorPage pageBusinessObjectSort;

	private StiCloudTabulatorPage pageBusinessObjectFilters;

	private StiTreeViewUX tvDataSources;

	private StiDataRelation selectedRelation;

	private StiColumnsSort columnsBusinessObjectSort;

	private StiColumnsFilters filtersBusinessObjectControl;

	private StiColumnsSort columnsDataSourceSort;

	private StiColumnsFilters filtersDataSourceControl;

	private StiTreeViewUX tvRelations;

	private StiTreeViewUX tvMasterComponent;

	private ImageList ilDataImages;

	public StiToolBar tbTool;

	public StiToolBar stiToolBar1;

	private StiGroupPanelUX gbCountData;

	private StiNumericUpDown nupCountData;

	private StiLabelUX lbCountData;

	private Timer timerMarker;

	private StiButtonUX tbNewDataSource;

	private StiButtonUX tbNewBusinessObject;

	private StiButtonUX tbNewDataRelation;

	public StiComponent Component
	{
		get
		{
			return component;
		}
		set
		{
			component = value;
			if (value != null)
			{
				Build(refreshMasterComponent: true);
				filtersDataSourceControl.Report = Report;
				filtersBusinessObjectControl.Report = Report;
			}
		}
	}

	public StartTab TabulatorTabIndex
	{
		get
		{
			return (StartTab)TabControl.SelectedIndex;
		}
		set
		{
			TabControl.SelectedIndex = (int)value;
		}
	}

	private StiReport Report => component.Page.Report;

	private IStiBusinessObject BusinessObject => component as IStiBusinessObject;

	private IStiDataSource DataSource => component as IStiDataSource;

	private IStiFilter Filter => component as IStiFilter;

	private IStiDataRelation DataRelation => component as IStiDataRelation;

	private IStiMasterComponent MasterComponent => component as IStiMasterComponent;

	private IStiSort Sort => component as IStiSort;

	public event EventHandler Close;

	public StiDataSelectionControl()
	{
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			((TreeView)tvDataSources).ImageList = StiDataBuilder.Images;
			((TreeView)tvRelations).ImageList = StiDataBuilder.Images;
			Localize();
			((Control)tbNewDataSource).Visible = StiOptions.Designer.Panels.Dictionary.ShowDataSourceNewMenuItem;
			((Control)tbNewBusinessObject).Visible = StiOptions.Designer.Panels.Dictionary.ShowBusinessObjectNewMenuItem && !StiOptions.Designer.IsBeginnerDesigner && !StiOptions.Designer.IsBICreatorDesigner;
			if (!StiOptions.Designer.Panels.Dictionary.ShowDataSourceNewMenuItem && !StiOptions.Designer.Panels.Dictionary.ShowBusinessObjectNewMenuItem)
			{
				((Control)tbTool).Visible = false;
				((Control)tvDataSources).Top = StiScale.I3;
				StiTreeViewUX stiTreeViewUX = tvDataSources;
				((Control)stiTreeViewUX).Height = ((Control)stiTreeViewUX).Height + StiScale.I(28);
			}
			((Control)tbNewDataRelation).Enabled = StiOptions.Designer.Panels.Dictionary.ShowRelationNewMenuItem;
			MeasureButtonSize();
		}
	}

	private void Localize()
	{
		((Control)pageDataSource).Text = StiLocalization.Get("PropertyMain", "DataSource");
		((Control)pageDataSourceDataRelation).Text = StiLocalization.Get("PropertyMain", "Relation");
		((Control)pageDataSourceMasterComponent).Text = StiLocalization.Get("PropertyMain", "MasterComponent");
		((Control)pageDataSourceSort).Text = StiLocalization.Get("PropertyMain", "Sort");
		((Control)pageDataSourceFilters).Text = StiLocalization.Get("PropertyMain", "Filters");
		((Control)pageBusinessObjectSort).Text = StiLocalization.Get("PropertyMain", "Sort");
		((Control)pageBusinessObjectFilters).Text = StiLocalization.Get("PropertyMain", "Filters");
		((Control)tbNewDataSource).Text = StiLocalization.Get("FormDictionaryDesigner", "DataSourceNew");
		((Control)tbNewBusinessObject).Text = StiLocalization.Get("FormDictionaryDesigner", "NewBusinessObject");
		((Control)tbNewDataRelation).Text = StiLocalization.Get("FormDictionaryDesigner", "RelationNew");
		((Control)lbCountData).Text = Loc.GetCleaned("Report", "LabelCountData");
	}

	internal void DoScaling()
	{
		DockPaddingEdges dockPadding = TabControl.DockPadding;
		int right = (TabControl.TitleWidth = StiScale.XXI(110.0));
		dockPadding.Right = right;
		pageBusinessObjectFilters.Image = StiReportDesignImages.Tasks.DataFilters(StiImageSize.Double);
		pageBusinessObjectSort.Image = StiReportDesignImages.Tasks.DataSort(StiImageSize.Double);
		pageDataSource.Image = StiReportDesignImages.Tasks.DataDataSource(StiImageSize.Double);
		pageDataSourceDataRelation.Image = StiReportDesignImages.Tasks.DataRelation(StiImageSize.Double);
		pageDataSourceFilters.Image = StiReportDesignImages.Tasks.DataFilters(StiImageSize.Double);
		pageDataSourceMasterComponent.Image = StiReportDesignImages.Tasks.DataMasterComponent(StiImageSize.Double);
		pageDataSourceSort.Image = StiReportDesignImages.Tasks.DataSort(StiImageSize.Double);
	}

	private void MeasureButtonSize()
	{
		string text = ((Control)tbNewDataSource).Text;
		string text2 = ((Control)tbNewBusinessObject).Text;
		string text3 = ((Control)tbNewDataRelation).Text;
		Size size = TextRenderer.MeasureText(text, ((Control)tbNewDataSource).Font);
		Size size2 = TextRenderer.MeasureText(text2, ((Control)tbNewBusinessObject).Font);
		Size size3 = TextRenderer.MeasureText(text3, ((Control)tbNewDataRelation).Font);
		((Control)tbNewDataSource).Width = size.Width + StiScale.I20;
		((Control)tbNewBusinessObject).Width = size2.Width + StiScale.I20;
		((Control)tbNewDataRelation).Width = size3.Width + StiScale.I20;
		((Control)tbNewBusinessObject).Location = new Point(((Control)tbNewDataSource).Right + StiScale.I11, ((Control)tbNewBusinessObject).Location.Y);
	}

	private void BuildDataSources()
	{
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Expected O, but got Unknown
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Expected O, but got Unknown
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Expected O, but got Unknown
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Expected O, but got Unknown
		if (DataSource is StiDataBand)
		{
			nupCountData.Value = ((StiDataBand)DataSource).CountData;
		}
		else
		{
			((Control)nupCountData).Enabled = false;
		}
		StiDataSourcesCollection dataSources = Report.Dictionary.DataSources;
		((TreeView)tvDataSources).Nodes.Clear();
		((TreeView)tvDataSources).Nodes.Add(new TreeNode("[" + Loc.Get("Report", "NotAssigned") + "]", 0, 0));
		foreach (StiDataSource item in dataSources)
		{
			TreeNode val = new TreeNode(item.ToString());
			string imageKey = (val.SelectedImageKey = GetDataSourceImageKey(item));
			val.ImageKey = imageKey;
			val.Tag = item;
			string categoryName = item.GetCategoryName();
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
				TreeNode obj = val2;
				imageKey = (val2.SelectedImageKey = "Database");
				obj.ImageKey = imageKey;
				((TreeView)tvDataSources).Nodes.Add(val2);
			}
			val2.Nodes.Add(val);
			if (item == DataSource.DataSource)
			{
				((TreeView)tvDataSources).SelectedNode = val;
			}
		}
		if (Report.Dictionary.BusinessObjects.Count > 0)
		{
			TreeNode val4 = new TreeNode(StiLocalization.Get("Report", "BusinessObjects"));
			string imageKey = (val4.SelectedImageKey = "BusinessObject");
			val4.ImageKey = imageKey;
			((TreeView)tvDataSources).Nodes.Add(val4);
			dataBuilder.BuildBusinessObjects(Report.Dictionary.BusinessObjects, val4, includeColumns: false, includeCalcColumns: false);
			if (((TreeView)tvDataSources).SelectedNode == null)
			{
				SelectNode(BusinessObject, ((TreeView)tvDataSources).Nodes);
			}
		}
		if (((TreeView)tvDataSources).SelectedNode == null)
		{
			((TreeView)tvDataSources).SelectedNode = ((TreeView)tvDataSources).Nodes[0];
		}
		((TreeView)tvDataSources).ExpandAll();
		TreeNode selectedNode = ((TreeView)tvDataSources).SelectedNode;
		if (selectedNode != null)
		{
			selectedNode.EnsureVisible();
		}
	}

	private static string GetDataSourceImageKey(StiDataSource dataSource)
	{
		if (!(dataSource is StiDataTransformation))
		{
			return "DataSource";
		}
		return "DataTransformation";
	}

	private void SelectNode(IStiBusinessObject businessObject, TreeNodeCollection nodes)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		if (string.IsNullOrEmpty(BusinessObject.BusinessObjectGuid))
		{
			return;
		}
		foreach (TreeNode node in nodes)
		{
			TreeNode val = node;
			if (val.Tag is StiBusinessObject && ((StiBusinessObject)val.Tag).Guid == businessObject.BusinessObjectGuid)
			{
				((TreeView)tvDataSources).SelectedNode = val;
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
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Expected O, but got Unknown
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Expected O, but got Unknown
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Expected O, but got Unknown
		StiDataSource selectedDataSource = GetSelectedDataSource();
		if (!pageRelationsFirstInit && selectedDataSource == lastDataSourcePageRelations)
		{
			return;
		}
		pageRelationsFirstInit = false;
		if (selectedDataSource != null)
		{
			relations = selectedDataSource.GetParentRelations();
		}
		else
		{
			relations = new StiDataRelationsCollection(Report.Dictionary);
			if (MasterComponent != null && MasterComponent.MasterComponent != null && ((IStiDataSource)MasterComponent.MasterComponent).DataSource != null)
			{
				relations = ((IStiDataSource)MasterComponent.MasterComponent).DataSource.GetChildRelations();
			}
			relations = new StiDataRelationsCollection(Report.Dictionary);
		}
		((TreeView)tvRelations).Nodes.Clear();
		((TreeView)tvRelations).Nodes.Add(new TreeNode("[" + StiLocalization.Get("Report", "NotAssigned") + "]", 0, 0));
		((TreeView)tvRelations).SelectedNode = ((TreeView)tvRelations).Nodes[0];
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
				foreach (TreeNode node in ((TreeView)tvRelations).Nodes)
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
					val2 = new TreeNode(categoryName)
					{
						ImageKey = text2,
						SelectedImageKey = text2
					};
					((TreeView)tvRelations).Nodes.Add(val2);
				}
				val2.Nodes.Add(val);
			}
			else
			{
				((TreeView)tvRelations).Nodes.Add(val);
			}
			if (relation == selectedRelation)
			{
				((TreeView)tvRelations).SelectedNode = val;
			}
		}
		((TreeView)tvRelations).ExpandAll();
	}

	private void BuildMasterComponent()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Expected O, but got Unknown
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Expected O, but got Unknown
		ImageList val = new ImageList
		{
			ColorDepth = (ColorDepth)32,
			ImageSize = new Size(StiScale.I16, StiScale.I16)
		};
		val.Images.Add("Close", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.Close" + StiScale.StepName + ".png"));
		StiComponentsCollection stiComponentsCollection = new StiComponentsCollection();
		stiComponentsCollection.AddRange(component.Page.GetComponents());
		StiSubReport subReportForPage = StiSubReport.GetSubReportForPage(component.Page);
		if (subReportForPage != null && subReportForPage.Parent != null)
		{
			StiComponent stiComponent = subReportForPage.Parent;
			if (stiComponent is StiChildBand)
			{
				StiComponent master = (stiComponent as StiChildBand).GetMaster();
				if (master != null)
				{
					stiComponent = master;
				}
			}
			else if (stiComponent is StiPanel)
			{
				while (stiComponent is StiPanel && stiComponent.Parent != null)
				{
					stiComponent = stiComponent.Parent;
				}
			}
			stiComponentsCollection.Add(stiComponent);
		}
		if (component is StiCrossDataBand)
		{
			stiComponentsCollection.Add(component.Parent);
		}
		if (component is IStiChartComponent)
		{
			stiComponentsCollection.Add(component.Parent);
		}
		((TreeView)tvMasterComponent).Nodes.Clear();
		((TreeView)tvMasterComponent).Nodes.Add(new TreeNode("[" + StiLocalization.Get("Report", "NotAssigned") + "]", 0, 0));
		((TreeView)tvMasterComponent).SelectedNode = ((TreeView)tvMasterComponent).Nodes[0];
		((TreeView)tvMasterComponent).ImageList = val;
		Hashtable hashtable = new Hashtable();
		foreach (StiComponent item in stiComponentsCollection)
		{
			if (!(item is IStiMasterComponent) || (item.Name == component.Name && item.Page == component.Page) || (item as IStiMasterComponent).MasterComponent == component || item is StiEmptyBand || hashtable[item] != null)
			{
				continue;
			}
			Type type = item.GetType();
			int num;
			if (hashMasterComponentIcons.ContainsKey(type))
			{
				num = (int)hashMasterComponentIcons[type];
			}
			else
			{
				Image image = StiImageResizer.GetImage((Image)(object)StiResourcesHelper.GetImageByName("Stimulsoft.Report.Design.Resources.Office2013.ToolBox." + item.GetType().Name + ".png"));
				if (image == null)
				{
					image = StiImageResizer.GetImage((Image)(object)StiServiceBitmapAttribute.GetImage(typeof(StiComponent)));
				}
				val.Images.Add(image);
				num = val.Images.Count - 1;
				hashMasterComponentIcons.Add(type, num);
			}
			TreeNode val2 = new TreeNode(item.Name, num, num)
			{
				Tag = item
			};
			((TreeView)tvMasterComponent).Nodes.Add(val2);
			if (item == MasterComponent.MasterComponent)
			{
				((TreeView)tvMasterComponent).SelectedNode = val2;
			}
			hashtable[item] = item;
		}
		hashtable.Clear();
		((TreeView)tvMasterComponent).ExpandAll();
	}

	private void BuildSort()
	{
		columnsDataSourceSort.Sort = Sort.Sort;
		columnsDataSourceSort.DataSource = DataSource.DataSource;
		columnsBusinessObjectSort.BusinessObject = BusinessObject.BusinessObject;
		columnsBusinessObjectSort.Sort = Sort.Sort;
	}

	private void BuildFilters()
	{
		filtersDataSourceControl.DataSource = DataSource.DataSource;
		filtersDataSourceControl.BuildFilters(Filter.Filters);
		((RadioButton)filtersDataSourceControl.rbAnd).Checked = Filter.FilterMode == StiFilterMode.And;
		((RadioButton)filtersDataSourceControl.rbOr).Checked = Filter.FilterMode == StiFilterMode.Or;
		StiDataBand stiDataBand = component as StiDataBand;
		if (stiDataBand != null)
		{
			((Control)filtersDataSourceControl.comboBoxFilterEngine).Enabled = true;
			((ListControl)filtersDataSourceControl.comboBoxFilterEngine).SelectedIndex = ((stiDataBand.FilterEngine != 0) ? 1 : 0);
		}
		else
		{
			((Control)filtersDataSourceControl.comboBoxFilterEngine).Enabled = false;
		}
		((CheckBox)filtersDataSourceControl.cbFilterOn).Checked = Filter.FilterOn;
		filtersBusinessObjectControl.BusinessObject = BusinessObject.BusinessObject;
		filtersBusinessObjectControl.BuildFilters(Filter.Filters);
		((RadioButton)filtersBusinessObjectControl.rbAnd).Checked = Filter.FilterMode == StiFilterMode.And;
		((RadioButton)filtersBusinessObjectControl.rbOr).Checked = Filter.FilterMode == StiFilterMode.Or;
		if (stiDataBand != null)
		{
			((Control)filtersBusinessObjectControl.comboBoxFilterEngine).Enabled = true;
			((ListControl)filtersBusinessObjectControl.comboBoxFilterEngine).SelectedIndex = ((stiDataBand.FilterEngine != 0) ? 1 : 0);
		}
		else
		{
			((Control)filtersBusinessObjectControl.comboBoxFilterEngine).Enabled = false;
		}
		((CheckBox)filtersBusinessObjectControl.cbFilterOn).Checked = Filter.FilterOn;
	}

	private void Build(bool refreshMasterComponent)
	{
		if ((component is IStiDataSource || component is IStiBusinessObject) && !(component is StiEmptyBand))
		{
			if (!(component is StiDataBand))
			{
				((Control)gbCountData).Visible = false;
				StiTreeViewUX stiTreeViewUX = tvDataSources;
				((Control)stiTreeViewUX).Height = ((Control)stiTreeViewUX).Height + ((Control)gbCountData).Height;
			}
			BuildDataSources();
		}
		else
		{
			pageDataSource.Invisible = true;
		}
		if (!StiOptions.Designer.DataBandEditor.DataSourceVisible)
		{
			pageDataSource.Invisible = true;
		}
		if (component is IStiDataRelation && !(component is StiEmptyBand))
		{
			selectedRelation = ((IStiDataRelation)component).DataRelation;
			BuildDataRelations();
		}
		else
		{
			pageDataSourceDataRelation.Invisible = true;
		}
		if (!StiOptions.Designer.DataBandEditor.DataRelationVisible)
		{
			pageDataSourceDataRelation.Invisible = true;
		}
		if (refreshMasterComponent)
		{
			if (component is IStiMasterComponent)
			{
				BuildMasterComponent();
			}
			else
			{
				pageDataSourceMasterComponent.Invisible = true;
			}
			if (!StiOptions.Designer.DataBandEditor.MasterComponentVisible)
			{
				pageDataSourceMasterComponent.Invisible = true;
			}
		}
		if (component is IStiSort && !(component is StiEmptyBand))
		{
			BuildSort();
		}
		else
		{
			pageDataSourceSort.Invisible = true;
		}
		if (!StiOptions.Designer.DataBandEditor.SortVisible)
		{
			pageDataSourceSort.Invisible = true;
		}
		if (component is IStiFilter && !(component is StiEmptyBand))
		{
			BuildFilters();
		}
		else
		{
			pageDataSourceFilters.Invisible = true;
		}
		if (!StiOptions.Designer.DataBandEditor.FiltersVisible)
		{
			pageDataSourceFilters.Invisible = true;
		}
		if (component is IStiSort && !(component is StiEmptyBand))
		{
			BuildSort();
		}
		else
		{
			pageBusinessObjectSort.Invisible = true;
		}
		if (!StiOptions.Designer.DataBandEditor.SortVisible)
		{
			pageBusinessObjectSort.Invisible = true;
		}
		if (component is IStiFilter && !(component is StiEmptyBand))
		{
			BuildFilters();
		}
		else
		{
			pageBusinessObjectFilters.Invisible = true;
		}
		if (!StiOptions.Designer.DataBandEditor.FiltersVisible)
		{
			pageBusinessObjectFilters.Invisible = true;
		}
	}

	private StiDataSource GetSelectedDataSource()
	{
		if (component is StiSparkline)
		{
			return (component as StiSparkline).GetDataSource();
		}
		if (pageDataSource.Invisible)
		{
			return null;
		}
		if (component is StiDataBand)
		{
			((StiDataBand)component).CountData = (int)nupCountData.Value;
		}
		if (((TreeView)tvDataSources).SelectedNode != null)
		{
			return ((TreeView)tvDataSources).SelectedNode.Tag as StiDataSource;
		}
		return null;
	}

	private StiBusinessObject GetSelectedBusinessObject()
	{
		if (pageDataSource.Invisible)
		{
			return null;
		}
		if (((TreeView)tvDataSources).SelectedNode != null)
		{
			return ((TreeView)tvDataSources).SelectedNode.Tag as StiBusinessObject;
		}
		return null;
	}

	public void SaveData()
	{
		timerMarker.Stop();
		if (component != null && component.Report != null && component.Report.Designer != null)
		{
			component.Report.Designer.UndoRedoSave(StiLocalization.Get("Components", "StiDataBand"), onlyCurrentPage: true);
		}
		StiDataSource selectedDataSource = GetSelectedDataSource();
		if (selectedDataSource != null)
		{
			if (DataSource != null)
			{
				DataSource.DataSourceName = selectedDataSource.Name;
			}
		}
		else if (DataSource != null)
		{
			DataSource.DataSourceName = string.Empty;
		}
		if (nupCountData.Value > 0m)
		{
			DataSource.DataSourceName = string.Empty;
			((StiDataBand)component).CountData = (int)nupCountData.Value;
		}
		if (!pageDataSourceDataRelation.Invisible && DataRelation != null)
		{
			if (((TreeView)tvRelations).SelectedNode != null && ((TreeView)tvRelations).SelectedNode.Tag != null)
			{
				DataRelation.DataRelationName = ((StiDataRelation)((TreeView)tvRelations).SelectedNode.Tag).NameInSource;
			}
			else
			{
				DataRelation.DataRelationName = null;
			}
		}
		if (!pageDataSourceMasterComponent.Invisible && MasterComponent != null)
		{
			if (((TreeView)tvMasterComponent).SelectedNode.Tag != null)
			{
				MasterComponent.MasterComponent = (StiComponent)((TreeView)tvMasterComponent).SelectedNode.Tag;
			}
			else
			{
				MasterComponent.MasterComponent = null;
			}
		}
		if (!pageDataSourceSort.Invisible && Sort != null)
		{
			Sort.Sort = columnsDataSourceSort.Sort;
		}
		if (!pageDataSourceFilters.Invisible && Filter != null)
		{
			Filter.FilterMode = ((!((RadioButton)filtersDataSourceControl.rbAnd).Checked) ? StiFilterMode.Or : StiFilterMode.And);
			if (component is StiDataBand stiDataBand)
			{
				stiDataBand.FilterEngine = ((((ListControl)filtersDataSourceControl.comboBoxFilterEngine).SelectedIndex != 0) ? StiFilterEngine.SQLQuery : StiFilterEngine.ReportEngine);
			}
			filtersDataSourceControl.FillFilters(Filter.Filters);
			Filter.FilterOn = ((CheckBox)filtersDataSourceControl.cbFilterOn).Checked;
		}
		if (BusinessObject != null)
		{
			StiBusinessObject selectedBusinessObject = GetSelectedBusinessObject();
			if (selectedBusinessObject != null)
			{
				BusinessObject.BusinessObjectGuid = selectedBusinessObject.Guid;
			}
			else
			{
				BusinessObject.BusinessObjectGuid = string.Empty;
			}
		}
		if (!pageBusinessObjectSort.Invisible)
		{
			Sort.Sort = columnsBusinessObjectSort.Sort;
		}
		if (!pageBusinessObjectFilters.Invisible)
		{
			Filter.FilterMode = ((!((RadioButton)filtersBusinessObjectControl.rbAnd).Checked) ? StiFilterMode.Or : StiFilterMode.And);
			if (component is StiDataBand stiDataBand2)
			{
				stiDataBand2.FilterEngine = ((((ListControl)filtersBusinessObjectControl.comboBoxFilterEngine).SelectedIndex != 0) ? StiFilterEngine.SQLQuery : StiFilterEngine.ReportEngine);
			}
			filtersBusinessObjectControl.FillFilters(Filter.Filters);
			Filter.FilterOn = ((CheckBox)filtersBusinessObjectControl.cbFilterOn).Checked;
		}
	}

	protected virtual void OnClose(object sender)
	{
	}

	public void InvokeClose()
	{
		this.Close?.Invoke(this, EventArgs.Empty);
		OnClose(this);
	}

	private void ButtonNewDataSource_Click(object sender, EventArgs e)
	{
		if (Report.Designer != null)
		{
			StiDictionaryPanelService service = StiDictionaryPanelService.GetService(Report.Designer.DesignerControl as StiDesignerControl);
			service.InvokeConnectionNew();
			Build(refreshMasterComponent: false);
		}
	}

	private void ButtonNewBusinessObject_Click(object sender, EventArgs e)
	{
		if (Report.Designer != null)
		{
			StiDictionaryPanelService service = StiDictionaryPanelService.GetService(Report.Designer.DesignerControl as StiDesignerControl);
			service.InvokeBusinessObjectNew();
			Build(refreshMasterComponent: false);
		}
	}

	private void Close_Click(object sender, EventArgs e)
	{
		InvokeClose();
	}

	private void TvDataSources_AfterSelect(object sender, TreeViewEventArgs e)
	{
		StiDataSource selectedDataSource = GetSelectedDataSource();
		StiBusinessObject selectedBusinessObject = GetSelectedBusinessObject();
		if (selectedBusinessObject != null)
		{
			pageDataSourceDataRelation.Invisible = true;
			pageDataSourceMasterComponent.Invisible = false;
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
			pageDataSourceMasterComponent.Invisible = false;
			pageDataSourceSort.Invisible = false;
			pageDataSourceFilters.Invisible = false;
			pageBusinessObjectSort.Invisible = true;
			pageBusinessObjectFilters.Invisible = true;
			columnsBusinessObjectSort.BusinessObject = null;
			columnsDataSourceSort.DataSource = selectedDataSource;
			filtersBusinessObjectControl.BusinessObject = null;
			filtersDataSourceControl.DataSource = selectedDataSource;
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
		if (((TreeView)tvRelations).SelectedNode != null && ((TreeView)tvRelations).SelectedNode.Tag is StiDataRelation)
		{
			selectedRelation = ((TreeView)tvRelations).SelectedNode.Tag as StiDataRelation;
		}
	}

	private void NupCountData_ValueChanged(object sender, EventArgs e)
	{
		((Control)tvDataSources).Enabled = nupCountData.Value == 0m;
	}

	private void ButtonNewDataRelation_Click(object sender, EventArgs e)
	{
		if (Report.Designer != null)
		{
			StiDictionaryPanelService service = StiDictionaryPanelService.GetService(Report.Designer.DesignerControl as StiDesignerControl);
			service.InvokeRelationNew();
			Build(refreshMasterComponent: false);
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
			int showMarker;
			if ((int)nupCountData.Value == 0)
			{
				TreeNode selectedNode = ((TreeView)tvDataSources).SelectedNode;
				showMarker = ((((selectedNode != null) ? selectedNode.Tag : null) is StiDataSource) ? 1 : 0);
			}
			else
			{
				showMarker = 1;
			}
			stiCloudTabulatorPage.ShowMarker = (byte)showMarker != 0;
			StiCloudTabulatorPage stiCloudTabulatorPage2 = pageDataSourceDataRelation;
			TreeNode selectedNode2 = ((TreeView)tvRelations).SelectedNode;
			stiCloudTabulatorPage2.ShowMarker = ((selectedNode2 != null) ? selectedNode2.Tag : null) is StiDataRelation;
			pageDataSourceFilters.ShowMarker = ((ArrangedElementCollection)((Control)filtersDataSourceControl.ZoneContainerControl).Controls).Count > 0;
			StiCloudTabulatorPage stiCloudTabulatorPage3 = pageDataSourceMasterComponent;
			TreeNode selectedNode3 = ((TreeView)tvMasterComponent).SelectedNode;
			stiCloudTabulatorPage3.ShowMarker = ((selectedNode3 != null) ? selectedNode3.Tag : null) != null;
			pageDataSourceSort.ShowMarker = columnsDataSourceSort.Sort != null && columnsDataSourceSort.Sort.Length != 0;
			pageBusinessObjectFilters.ShowMarker = ((ArrangedElementCollection)((Control)filtersBusinessObjectControl.ZoneContainerControl).Controls).Count > 0;
			pageBusinessObjectSort.ShowMarker = columnsBusinessObjectSort.Sort != null && columnsBusinessObjectSort.Sort.Length != 0;
		}
		catch
		{
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
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Expected O, but got Unknown
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		//IL_0466: Unknown result type (might be due to invalid IL or missing references)
		//IL_0470: Expected O, but got Unknown
		//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0570: Unknown result type (might be due to invalid IL or missing references)
		//IL_057a: Expected O, but got Unknown
		//IL_0587: Unknown result type (might be due to invalid IL or missing references)
		//IL_0591: Expected O, but got Unknown
		//IL_0621: Unknown result type (might be due to invalid IL or missing references)
		//IL_08be: Unknown result type (might be due to invalid IL or missing references)
		//IL_0961: Unknown result type (might be due to invalid IL or missing references)
		//IL_096b: Expected O, but got Unknown
		//IL_09a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab5: Expected O, but got Unknown
		//IL_0b2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c17: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c21: Expected O, but got Unknown
		//IL_0c56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d72: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d7c: Expected O, but got Unknown
		//IL_0da1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0edd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee7: Expected O, but got Unknown
		//IL_0f0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1048: Unknown result type (might be due to invalid IL or missing references)
		//IL_1052: Expected O, but got Unknown
		//IL_108a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1123: Unknown result type (might be due to invalid IL or missing references)
		//IL_11a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ac: Expected O, but got Unknown
		//IL_11e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_127d: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiDataSelectionControl));
		ilDataImages = new ImageList(components);
		TabControl = new StiCloudTabulator();
		pageDataSource = new StiCloudTabulatorPage();
		tvDataSources = new StiTreeViewUX();
		tbTool = new StiToolBar();
		tbNewBusinessObject = new StiButtonUX();
		tbNewDataSource = new StiButtonUX();
		gbCountData = new StiGroupPanelUX();
		nupCountData = new StiNumericUpDown();
		lbCountData = new StiLabelUX();
		pageDataSourceDataRelation = new StiCloudTabulatorPage();
		tvRelations = new StiTreeViewUX();
		stiToolBar1 = new StiToolBar();
		tbNewDataRelation = new StiButtonUX();
		pageDataSourceMasterComponent = new StiCloudTabulatorPage();
		tvMasterComponent = new StiTreeViewUX();
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
		((Control)gbCountData).SuspendLayout();
		((ISupportInitialize)nupCountData).BeginInit();
		((Control)pageDataSourceDataRelation).SuspendLayout();
		((Control)stiToolBar1).SuspendLayout();
		((Control)pageDataSourceMasterComponent).SuspendLayout();
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
		((Control)TabControl).Controls.Add((Control)(object)pageDataSourceMasterComponent);
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
		((Control)TabControl).Size = new Size(613, 424);
		((Control)TabControl).TabIndex = 0;
		TabControl.TitleBackColor = SystemColors.Control;
		TabControl.TitleInLine = false;
		TabControl.TitlePosition = StiTabTitlePosition.RightHorizontal;
		TabControl.TitleWidth = 110;
		TabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
		((Control)pageDataSource).Controls.Add((Control)(object)tvDataSources);
		((Control)pageDataSource).Controls.Add((Control)(object)tbTool);
		((Control)pageDataSource).Controls.Add((Control)(object)gbCountData);
		pageDataSource.Dock = (DockStyle)5;
		pageDataSource.Image = (Image)componentResourceManager.GetObject("pageDataSource.Image");
		pageDataSource.InvisibleVisible = false;
		((Control)pageDataSource).Location = new Point(0, 0);
		((Control)pageDataSource).Name = "pageDataSource";
		((Control)pageDataSource).Padding = new Padding(0, 0, 1, 0);
		((Control)pageDataSource).Size = new Size(494, 424);
		((Control)pageDataSource).TabIndex = 0;
		((Control)pageDataSource).Text = "DataSource";
		((Control)pageDataSource).Visible = false;
		((Control)tvDataSources).Anchor = (AnchorStyles)15;
		((TreeView)tvDataSources).BorderStyle = (BorderStyle)0;
		((TreeView)tvDataSources).HideSelection = false;
		((Control)tvDataSources).Location = new Point(12, 59);
		((Control)tvDataSources).Name = "tvDataSources";
		((Control)tvDataSources).Size = new Size(468, 307);
		((Control)tvDataSources).TabIndex = 2;
		((TreeView)tvDataSources).BeforeExpand += new TreeViewCancelEventHandler(TreeViewDataSources_BeforeExpand);
		((TreeView)tvDataSources).AfterSelect += new TreeViewEventHandler(TvDataSources_AfterSelect);
		((Control)tvDataSources).DoubleClick += Close_Click;
		((Control)tbTool).Controls.Add((Control)(object)tbNewBusinessObject);
		((Control)tbTool).Controls.Add((Control)(object)tbNewDataSource);
		tbTool.ControlStyle = StiControlStyle.Office2013Blue;
		((Control)tbTool).Dock = (DockStyle)1;
		tbTool.LineStyle = StiToolBar.StiToolBarLineStyle.Bottom;
		((Control)tbTool).Location = new Point(0, 0);
		((Control)tbTool).Name = "tbTool";
		((Control)tbTool).Padding = new Padding(3);
		((Control)tbTool).Size = new Size(493, 47);
		((Control)tbTool).TabIndex = 4;
		((Control)tbTool).Text = "stiToolBar1";
		((Control)tbNewBusinessObject).Location = new Point(133, 11);
		((Control)tbNewBusinessObject).Name = "tbNewBusinessObject";
		((Control)tbNewBusinessObject).Size = new Size(113, 24);
		((Control)tbNewBusinessObject).TabIndex = 79;
		((Control)tbNewBusinessObject).Text = "New Business";
		((ButtonBase)tbNewBusinessObject).UseVisualStyleBackColor = true;
		((Control)tbNewBusinessObject).Click += ButtonNewBusinessObject_Click;
		((Control)tbNewDataSource).Location = new Point(11, 11);
		((Control)tbNewDataSource).Name = "tbNewDataSource";
		((Control)tbNewDataSource).Size = new Size(113, 24);
		((Control)tbNewDataSource).TabIndex = 78;
		((Control)tbNewDataSource).Text = "New Data Source";
		((ButtonBase)tbNewDataSource).UseVisualStyleBackColor = true;
		((Control)tbNewDataSource).Click += ButtonNewDataSource_Click;
		((Control)gbCountData).BackColor = Color.White;
		((Control)gbCountData).Controls.Add((Control)(object)nupCountData);
		((Control)gbCountData).Controls.Add((Control)(object)lbCountData);
		((Control)gbCountData).Dock = (DockStyle)2;
		gbCountData.LinePosition = StiLinePosition.Top;
		gbCountData.LineStyle = StiLineStyle.Dot;
		((Control)gbCountData).Location = new Point(0, 376);
		((Control)gbCountData).Name = "gbCountData";
		((Control)gbCountData).Size = new Size(493, 48);
		((Control)gbCountData).TabIndex = 3;
		((Control)gbCountData).TabStop = false;
		((Control)nupCountData).BackColor = SystemColors.Window;
		nupCountData.DecimalPlaces = 0;
		((Control)nupCountData).Location = new Point(176, 13);
		((Control)nupCountData).Name = "nupCountData";
		((Control)nupCountData).Size = new Size(80, 21);
		((Control)nupCountData).TabIndex = 0;
		nupCountData.Minimum = 0m;
		nupCountData.ValueChanged += NupCountData_ValueChanged;
		((Control)lbCountData).ForeColor = Color.DimGray;
		((Control)lbCountData).Location = new Point(10, 13);
		((Control)lbCountData).Margin = new Padding(0, 3, 3, 0);
		((Control)lbCountData).Name = "lbCountData";
		((Control)lbCountData).Size = new Size(158, 20);
		((Control)lbCountData).TabIndex = 1;
		((Control)lbCountData).Text = "Count Data";
		((Label)lbCountData).TextAlign = (ContentAlignment)16;
		((Control)pageDataSourceDataRelation).Controls.Add((Control)(object)tvRelations);
		((Control)pageDataSourceDataRelation).Controls.Add((Control)(object)stiToolBar1);
		pageDataSourceDataRelation.Dock = (DockStyle)5;
		pageDataSourceDataRelation.Image = (Image)componentResourceManager.GetObject("pageDataSourceDataRelation.Image");
		pageDataSourceDataRelation.InvisibleVisible = false;
		((Control)pageDataSourceDataRelation).Location = new Point(0, 0);
		((Control)pageDataSourceDataRelation).Name = "pageDataSourceDataRelation";
		((Control)pageDataSourceDataRelation).Padding = new Padding(0, 0, 1, 0);
		((Control)pageDataSourceDataRelation).Size = new Size(494, 424);
		((Control)pageDataSourceDataRelation).TabIndex = 1;
		((Control)pageDataSourceDataRelation).Text = "DataRelation";
		((Control)pageDataSourceDataRelation).Visible = false;
		((Control)pageDataSourceDataRelation).DoubleClick += Close_Click;
		((Control)tvRelations).Anchor = (AnchorStyles)15;
		((TreeView)tvRelations).BorderStyle = (BorderStyle)0;
		((TreeView)tvRelations).HideSelection = false;
		((TreeView)tvRelations).ImageIndex = 0;
		((TreeView)tvRelations).ImageList = ilDataImages;
		((Control)tvRelations).Location = new Point(12, 59);
		((Control)tvRelations).Name = "tvRelations";
		((TreeView)tvRelations).SelectedImageIndex = 0;
		((Control)tvRelations).Size = new Size(469, 352);
		((Control)tvRelations).TabIndex = 3;
		((TreeView)tvRelations).AfterSelect += new TreeViewEventHandler(TreeViewRelations_AfterSelect);
		((Control)tvRelations).DoubleClick += Close_Click;
		((Control)stiToolBar1).Controls.Add((Control)(object)tbNewDataRelation);
		stiToolBar1.ControlStyle = StiControlStyle.Office2013Blue;
		((Control)stiToolBar1).Dock = (DockStyle)1;
		stiToolBar1.LineStyle = StiToolBar.StiToolBarLineStyle.Bottom;
		((Control)stiToolBar1).Location = new Point(0, 0);
		((Control)stiToolBar1).Name = "stiToolBar1";
		((Control)stiToolBar1).Padding = new Padding(3);
		((Control)stiToolBar1).Size = new Size(493, 47);
		((Control)stiToolBar1).TabIndex = 5;
		((Control)stiToolBar1).Text = "stiToolBar1";
		((Control)tbNewDataRelation).Location = new Point(11, 11);
		((Control)tbNewDataRelation).Name = "tbNewDataRelation";
		((Control)tbNewDataRelation).Size = new Size(113, 24);
		((Control)tbNewDataRelation).TabIndex = 79;
		((Control)tbNewDataRelation).Text = "New Data Relation";
		((ButtonBase)tbNewDataRelation).UseVisualStyleBackColor = true;
		((Control)tbNewDataRelation).Click += ButtonNewDataRelation_Click;
		((Control)pageDataSourceMasterComponent).Controls.Add((Control)(object)tvMasterComponent);
		pageDataSourceMasterComponent.Dock = (DockStyle)5;
		pageDataSourceMasterComponent.Image = (Image)componentResourceManager.GetObject("pageDataSourceMasterComponent.Image");
		pageDataSourceMasterComponent.InvisibleVisible = false;
		((Control)pageDataSourceMasterComponent).Location = new Point(0, 0);
		((Control)pageDataSourceMasterComponent).Name = "pageDataSourceMasterComponent";
		((Control)pageDataSourceMasterComponent).Padding = new Padding(4);
		((Control)pageDataSourceMasterComponent).Size = new Size(494, 424);
		((Control)pageDataSourceMasterComponent).TabIndex = 2;
		((Control)pageDataSourceMasterComponent).Text = "MasterComponent";
		((Control)pageDataSourceMasterComponent).Visible = false;
		((Control)pageDataSourceMasterComponent).DoubleClick += Close_Click;
		((Control)tvMasterComponent).Anchor = (AnchorStyles)15;
		((TreeView)tvMasterComponent).BorderStyle = (BorderStyle)0;
		((TreeView)tvMasterComponent).HideSelection = false;
		((Control)tvMasterComponent).Location = new Point(12, 12);
		((Control)tvMasterComponent).Name = "tvMasterComponent";
		((Control)tvMasterComponent).Size = new Size(469, 397);
		((Control)tvMasterComponent).TabIndex = 4;
		((Control)tvMasterComponent).DoubleClick += Close_Click;
		((Control)pageDataSourceSort).Controls.Add((Control)(object)columnsDataSourceSort);
		pageDataSourceSort.Dock = (DockStyle)5;
		pageDataSourceSort.Image = (Image)componentResourceManager.GetObject("pageDataSourceSort.Image");
		pageDataSourceSort.InvisibleVisible = false;
		((Control)pageDataSourceSort).Location = new Point(0, 0);
		((Control)pageDataSourceSort).Margin = new Padding(0);
		((Control)pageDataSourceSort).Name = "pageDataSourceSort";
		((Control)pageDataSourceSort).Padding = new Padding(0, 0, 1, 0);
		((Control)pageDataSourceSort).Size = new Size(494, 424);
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
		((Control)columnsDataSourceSort).Size = new Size(493, 424);
		columnsDataSourceSort.Sort = null;
		((Control)columnsDataSourceSort).TabIndex = 0;
		((Control)pageDataSourceFilters).Controls.Add((Control)(object)filtersDataSourceControl);
		pageDataSourceFilters.Dock = (DockStyle)5;
		pageDataSourceFilters.Image = (Image)componentResourceManager.GetObject("pageDataSourceFilters.Image");
		pageDataSourceFilters.InvisibleVisible = false;
		((Control)pageDataSourceFilters).Location = new Point(0, 0);
		((Control)pageDataSourceFilters).Margin = new Padding(0);
		((Control)pageDataSourceFilters).Name = "pageDataSourceFilters";
		((Control)pageDataSourceFilters).Padding = new Padding(0, 0, 1, 0);
		((Control)pageDataSourceFilters).Size = new Size(494, 424);
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
		((Control)filtersDataSourceControl).Size = new Size(493, 424);
		((Control)filtersDataSourceControl).TabIndex = 2;
		((Control)filtersDataSourceControl).TabStop = false;
		((Control)pageBusinessObjectSort).Controls.Add((Control)(object)columnsBusinessObjectSort);
		pageBusinessObjectSort.Dock = (DockStyle)5;
		pageBusinessObjectSort.Image = (Image)componentResourceManager.GetObject("pageBusinessObjectSort.Image");
		pageBusinessObjectSort.InvisibleVisible = false;
		((Control)pageBusinessObjectSort).Location = new Point(0, 0);
		((Control)pageBusinessObjectSort).Name = "pageBusinessObjectSort";
		((Control)pageBusinessObjectSort).Padding = new Padding(0, 4, 1, 4);
		((Control)pageBusinessObjectSort).Size = new Size(494, 424);
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
		((Control)columnsBusinessObjectSort).Size = new Size(493, 416);
		columnsBusinessObjectSort.Sort = null;
		((Control)columnsBusinessObjectSort).TabIndex = 0;
		((Control)pageBusinessObjectFilters).Controls.Add((Control)(object)filtersBusinessObjectControl);
		pageBusinessObjectFilters.Dock = (DockStyle)5;
		pageBusinessObjectFilters.Image = (Image)componentResourceManager.GetObject("pageBusinessObjectFilters.Image");
		pageBusinessObjectFilters.InvisibleVisible = true;
		((Control)pageBusinessObjectFilters).Location = new Point(0, 0);
		((Control)pageBusinessObjectFilters).Name = "pageBusinessObjectFilters";
		((Control)pageBusinessObjectFilters).Padding = new Padding(0, 4, 1, 4);
		((Control)pageBusinessObjectFilters).Size = new Size(494, 424);
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
		((Control)filtersBusinessObjectControl).Size = new Size(493, 416);
		((Control)filtersBusinessObjectControl).TabIndex = 2;
		((Control)filtersBusinessObjectControl).TabStop = false;
		timerMarker.Enabled = true;
		timerMarker.Interval = 500;
		timerMarker.Tick += TimerMarker_Tick;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)TabControl);
		((Control)this).Name = "StiDataSelectionControl";
		((Control)this).Size = new Size(613, 424);
		((Control)TabControl).ResumeLayout(false);
		((Control)pageDataSource).ResumeLayout(false);
		((Control)tbTool).ResumeLayout(false);
		((Control)gbCountData).ResumeLayout(false);
		((ISupportInitialize)nupCountData).EndInit();
		((Control)pageDataSourceDataRelation).ResumeLayout(false);
		((Control)stiToolBar1).ResumeLayout(false);
		((Control)pageDataSourceMasterComponent).ResumeLayout(false);
		((Control)pageDataSourceSort).ResumeLayout(false);
		((Control)pageDataSourceFilters).ResumeLayout(false);
		((Control)pageBusinessObjectSort).ResumeLayout(false);
		((Control)pageBusinessObjectFilters).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
