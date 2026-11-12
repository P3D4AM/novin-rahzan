using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Data.Functions;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Controls;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Panels;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Maps;
using Stimulsoft.Report.Maps.Helpers;

namespace Stimulsoft.Report.Dictionary.Design;

[Browsable(false)]
[ToolboxItem(false)]
public class StiDictionaryControl : UserControl
{
	public class DataInfo : IComparer<DataInfo>
	{
		public string Key { get; set; }

		public string Name { get; set; }

		public StiDatabase Database { get; set; }

		public string DataName { get; set; }

		public string SourceName { get; set; }

		public bool DataInStore { get; set; }

		public bool IsDataTransformation { get; set; }

		public StiData Data { get; set; }

		public int Compare(DataInfo x, DataInfo y)
		{
			return string.Compare(x.Name, y.Name, StringComparison.Ordinal);
		}

		public override string ToString()
		{
			return Name;
		}

		public DataInfo()
		{
		}

		public DataInfo(string key, string name, StiDatabase database, string dataName)
		{
			Key = key;
			Name = name;
			Database = database;
			DataName = dataName;
		}
	}

	public class HtmlTag
	{
		public string Tag { get; }

		public HtmlTag(string tag)
		{
			Tag = tag;
		}
	}

	private StiDataBuilder dataBuilder;

	private IContainer components;

	public StiTreeView tvData;

	private RichTextBox information;

	private ExpandableSplitter splitter;

	private Panel panelInformation;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Color BackColor
	{
		get
		{
			if (!StiUX.IsDark)
			{
				return ((Control)this).BackColor;
			}
			return StiUX.InputBackground;
		}
		set
		{
			if (!StiUX.IsDark)
			{
				((Control)this).BackColor = value;
			}
		}
	}

	internal StiDesignerControl Designer { get; set; }

	public bool SimpleDragDrop { get; set; }

	public bool AddParentheses { get; set; } = true;


	public bool IsDataExpression { get; set; }

	public bool CanDragDropDataSource { get; set; } = true;


	public bool CanDragDropBusinessObjectColumn { get; set; } = true;


	public bool CanDragDropDataColumn { get; set; } = true;


	public bool CanDragDropResource { get; set; } = true;


	public bool CanDragDropTextFormat { get; set; } = true;


	public bool CanDragDropHtmlTags { get; set; } = true;


	public bool CanDragDropProperty { get; set; } = true;


	public bool CanDragDropVariable { get; set; } = true;


	public bool CanDragDropFunction { get; set; } = true;


	public bool CanDragDropTotals { get; set; } = true;


	public bool CanDragDropDataParameter { get; set; } = true;


	public bool CanBuildTotals { get; set; } = true;


	public bool CanBuildFormats { get; set; } = true;


	public bool CanBuildHtmlTags { get; set; }

	public bool CanBuildDatabase { get; set; }

	public bool CanBuildParameters { get; set; }

	public bool CanDragDrop { get; set; }

	public TreeNode DraggedNode { get; private set; }

	internal StiNodesMemory NodesMemory { get; set; }

	internal Hashtable UsedDataSources { get; set; }

	internal Hashtable UsedColumns { get; set; }

	internal Hashtable UsedRelations { get; set; }

	public StiDictionaryControl()
	{
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			((TreeView)tvData).BorderStyle = (BorderStyle)0;
			((TextBoxBase)information).BorderStyle = (BorderStyle)0;
			((TreeView)tvData).ImageList = StiDataBuilder.Images;
			StiTreeView stiTreeView = tvData;
			Color foreColor = (((Control)information).ForeColor = StiUX.InputForeground);
			((Control)stiTreeView).ForeColor = foreColor;
			StiTreeView stiTreeView2 = tvData;
			RichTextBox obj = information;
			Color color = (splitter.BackColor = StiUX.InputBackground);
			foreColor = (((Control)obj).BackColor = color);
			((Control)stiTreeView2).BackColor = foreColor;
		}
	}

	public void BuildTotalsFunctions(TreeNode functionsNode)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		if (!CanBuildTotals)
		{
			return;
		}
		TreeNode val = new TreeNode(Loc.Get("PropertyMain", "Totals"));
		string imageKey = (val.SelectedImageKey = "Total");
		val.ImageKey = imageKey;
		foreach (StiAggregateFunctionService item in StiOptions.Services.AggregateFunctions.Where((StiAggregateFunctionService s) => s.ServiceEnabled && s.ServiceName.Length > 0))
		{
			TreeNode val2 = new TreeNode(item.ServiceName);
			imageKey = (val2.SelectedImageKey = "Total");
			val2.ImageKey = imageKey;
			val2.Tag = item;
			val.Nodes.Add(val2);
		}
		functionsNode.Nodes.Add(val);
	}

	private string GetCategoryImageKey(DataInfo info)
	{
		if (info.IsDataTransformation)
		{
			return "DatabaseTransformation";
		}
		if (info.Database == null)
		{
			if (!info.DataInStore)
			{
				return "DatabaseFail";
			}
			return "DataStore";
		}
		if (info.Database is StiUndefinedDatabase)
		{
			return "Undefined Database";
		}
		if (info.Database.Inherited)
		{
			return "LockedDatabase";
		}
		return "Database";
	}

	public void BuildDatabases(StiDatabaseCollection databases)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected O, but got Unknown
		TreeNode val = new TreeNode(Loc.Get("PropertyMain", "CategoryConnections"));
		string imageKey = (val.SelectedImageKey = "Database");
		val.ImageKey = imageKey;
		if (Designer != null)
		{
			foreach (StiDatabase database in databases)
			{
				if (Designer.Report.Dictionary.Restrictions.IsAllowShow(database.Name, StiDataType.Database))
				{
					TreeNode val2 = new TreeNode(database.ToString());
					imageKey = (val2.SelectedImageKey = (database.Inherited ? "LockedDatabase" : "Database"));
					val2.ImageKey = imageKey;
					if (database is StiUndefinedDatabase)
					{
						imageKey = (val2.SelectedImageKey = "UndefinedDatabase");
						val2.ImageKey = imageKey;
					}
					val2.Tag = database;
					val.Nodes.Add(val2);
				}
			}
		}
		((TreeView)tvData).Nodes.Add(val);
	}

	private void EnableInformation()
	{
		((Control)panelInformation).Visible = true;
		((Control)splitter).Visible = true;
		StiSettings.Load();
		int @int = StiSettings.GetInt(((Control)this).Name, "VerticalSplitter", ((Control)information).Height);
		if (@int != ((Control)information).Height)
		{
			((Control)information).Height = @int;
		}
		((TextBoxBase)information).SelectAll();
		((Control)information).Text = string.Empty;
		information.SelectionFont = ((Control)information).Font;
	}

	public void Build(StiReport report)
	{
		if (!((Component)this).DesignMode && report != null && report.Designer != null)
		{
			Designer = report.Designer.DesignerControl as StiDesignerControl;
			Build(report, showUsedObjects: false);
		}
	}

	public void Build(StiReport report, bool showUsedObjects)
	{
		((TreeView)tvData).BeginUpdate();
		if (showUsedObjects)
		{
			report.Dictionary.RetrievalData(out var usedRelations, out var usedDataSources, out var usedColumns);
			UsedRelations = usedRelations;
			UsedDataSources = usedDataSources;
			UsedColumns = usedColumns;
		}
		else
		{
			UsedRelations = null;
			UsedDataSources = null;
			UsedColumns = null;
		}
		if (NodesMemory != null)
		{
			NodesMemory.SaveExpNodes((TreeView)(object)tvData);
		}
		((TreeView)tvData).Nodes.Clear();
		BuildData(report.Dictionary);
		BuildBusinessObjects(report.Dictionary);
		BuildVariables(report.Dictionary);
		BuildSystemVariables();
		BuildFunctions();
		BuildFormats();
		BuildHtmlTags();
		BuildResources(report.Dictionary);
		((TreeView)tvData).EndUpdate();
		if (NodesMemory != null)
		{
			NodesMemory.LoadExpNodes((TreeView)(object)tvData, restoreSelectedNode: true);
		}
	}

	public void BuildData(StiDictionary dictionary)
	{
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Expected O, but got Unknown
		if (StiOptions.Dictionary.NotIdenticalNameAndAliasAtRegistrationOfNewData)
		{
			BuildDataX(dictionary);
			return;
		}
		if (dataBuilder == null)
		{
			dataBuilder = new StiDataBuilder(UsedRelations, UsedDataSources, UsedColumns, tvData);
		}
		dataBuilder.ShowParameters = true;
		bool flag = false;
		foreach (StiDatabase database in dictionary.Databases)
		{
			if (dictionary.Restrictions.IsAllowShow(database.Name, StiDataType.Database))
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			foreach (StiData item in dictionary.DataStore)
			{
				if (!item.IsReportData)
				{
					flag = true;
					break;
				}
			}
		}
		if (!flag)
		{
			foreach (StiDataSource dataSource in dictionary.DataSources)
			{
				if (dictionary.Restrictions.IsAllowShow(dataSource.Name, StiDataType.DataSource))
				{
					flag = true;
					break;
				}
			}
		}
		if (flag || StiOptions.Designer.Panels.Dictionary.ShowEmptyDataSourcesCategory)
		{
			TreeNodeEx treeNodeEx = new TreeNodeEx(Loc.Get("PropertyMain", "DataSources"));
			((TreeNode)treeNodeEx).Tag = "DataSources";
			((TreeNode)treeNodeEx).ImageKey = "DataSource";
			((TreeNode)treeNodeEx).SelectedImageKey = "DataSource";
			TreeNodeEx treeNodeEx2 = treeNodeEx;
			((TreeView)tvData).Nodes.Add((TreeNode)(object)treeNodeEx2);
			if (flag)
			{
				TreeNode val = new TreeNode("Loading");
				((TreeNode)treeNodeEx2).Nodes.Add(val);
				((TreeView)tvData).SelectedNode = (TreeNode)(object)treeNodeEx2;
				((TreeNode)treeNodeEx2).Expand();
			}
		}
	}

	private void ExpandedData(TreeNode dataSourcesNode)
	{
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Expected O, but got Unknown
		//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0407: Expected O, but got Unknown
		if (dataSourcesNode.Nodes.Count != 1 || dataSourcesNode.Nodes[0].Text != "Loading")
		{
			return;
		}
		dataSourcesNode.Nodes.Clear();
		StiDictionary dictionary = Designer.Report.Dictionary;
		dataBuilder.ShowParameters = true;
		List<DataInfo> list = new List<DataInfo>();
		Hashtable hashtable = new Hashtable();
		foreach (StiDatabase database in dictionary.Databases)
		{
			if (dictionary.Restrictions.IsAllowShow(database.Name, StiDataType.Database))
			{
				string name = database.ToString(StiOptions.Dictionary.ShowOnlyAliasForDatabase && Designer.UseAliases);
				DataInfo dataInfo = new DataInfo(database.Name.ToLowerInvariant(), name, database, null);
				hashtable[dataInfo.Key] = new List<StiDataSource>();
				list.Add(dataInfo);
			}
		}
		foreach (StiData item in dictionary.DataStore)
		{
			string text = item.Name;
			if (item.Data != null && item.Data is DataTable && item.Name.Contains("." + ((DataTable)item.Data).TableName))
			{
				text = item.Name.Substring(0, item.Name.Length - ((DataTable)item.Data).TableName.Length - 1);
			}
			if (!item.IsReportData && hashtable[item.Alias.ToLowerInvariant()] == null)
			{
				DataInfo dataInfo2 = new DataInfo(item.Alias.ToLowerInvariant(), item.Alias, null, item.Alias)
				{
					DataInStore = true,
					Data = item
				};
				List<StiDataSource> value = new List<StiDataSource>();
				hashtable[dataInfo2.Key] = value;
				hashtable[text.ToLowerInvariant()] = value;
				list.Add(dataInfo2);
			}
		}
		foreach (StiDataSource dataSource in dictionary.DataSources)
		{
			if (!dictionary.Restrictions.IsAllowShow(dataSource.Name, StiDataType.DataSource))
			{
				continue;
			}
			string text2 = dataSource.GetCategoryName();
			int num = text2.LastIndexOf("[", StringComparison.InvariantCulture);
			int num2 = text2.LastIndexOf("]", StringComparison.InvariantCulture);
			if (num != -1 && num2 != -1)
			{
				text2 = text2.Substring(num + 1, num2 - num - 1);
			}
			string key = text2.ToLowerInvariant();
			List<StiDataSource> list2 = hashtable[key] as List<StiDataSource>;
			if (list2 == null)
			{
				list2 = new List<StiDataSource>();
				DataInfo dataInfo3 = new DataInfo(key, text2, null, text2);
				if (dataSource is StiCrossTabDataSource || dataSource is StiUserSource || dataSource is StiVirtualSource || dataSource is StiDataTransformation)
				{
					dataInfo3.DataInStore = true;
				}
				dataInfo3.IsDataTransformation = dataSource is StiDataTransformation;
				if (dataSource is StiDBaseSource || dataSource is StiCsvSource)
				{
					StiDataStoreSource stiDataStoreSource = dataSource as StiDataStoreSource;
					if (!string.IsNullOrEmpty(stiDataStoreSource.NameInSource))
					{
						dataInfo3.DataInStore = true;
					}
				}
				hashtable[dataInfo3.Key] = list2;
				list.Add(dataInfo3);
			}
			list2.Add(dataSource);
		}
		list.Sort(new DataInfo());
		foreach (DataInfo item2 in list)
		{
			TreeNode val = null;
			if (item2.Database != null)
			{
				string text3 = item2.Database.ToString(StiOptions.Dictionary.ShowOnlyAliasForDatabase && Designer.UseAliases);
				val = new TreeNode
				{
					Text = text3,
					Tag = item2.Database
				};
			}
			else if (item2.DataName != null)
			{
				val = new TreeNode(item2.DataName);
			}
			TreeNode obj = val;
			string imageKey = (val.SelectedImageKey = GetCategoryImageKey(item2));
			obj.ImageKey = imageKey;
			dataSourcesNode.Nodes.Add(val);
			if (!(hashtable[item2.Key] is List<StiDataSource> list3))
			{
				continue;
			}
			foreach (StiDataSource item3 in list3)
			{
				TreeNodeEx dataNode = new TreeNodeEx();
				dataBuilder.BuildDataSource(item3, ref dataNode, includeCalcColumn: true);
				if (item3 is StiDataTransformation)
				{
					TreeNodeEx treeNodeEx = dataNode;
					imageKey = (((TreeNode)dataNode).SelectedImageKey = (item3.Inherited ? "LockedDataTransformation" : "DataTransformation"));
					((TreeNode)treeNodeEx).ImageKey = imageKey;
				}
				else
				{
					TreeNodeEx treeNodeEx2 = dataNode;
					imageKey = (((TreeNode)dataNode).SelectedImageKey = (item3.Inherited ? "LockedDataSource" : "DataSource"));
					((TreeNode)treeNodeEx2).ImageKey = imageKey;
				}
				if (item3 is StiUndefinedDataSource)
				{
					TreeNodeEx treeNodeEx3 = dataNode;
					imageKey = (((TreeNode)dataNode).SelectedImageKey = "UndefinedDataSource");
					((TreeNode)treeNodeEx3).ImageKey = imageKey;
				}
				val.Nodes.Add((TreeNode)(object)dataNode);
			}
		}
	}

	private void BuildDataX(StiDictionary dictionary)
	{
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Expected O, but got Unknown
		if (dataBuilder == null)
		{
			dataBuilder = new StiDataBuilder(UsedRelations, UsedDataSources, UsedColumns, tvData);
		}
		dataBuilder.ShowParameters = true;
		bool flag = false;
		foreach (StiDatabase database in dictionary.Databases)
		{
			if (dictionary.Restrictions.IsAllowShow(database.Name, StiDataType.Database))
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			foreach (StiData item in dictionary.DataStore)
			{
				if (!item.IsReportData)
				{
					flag = true;
					break;
				}
			}
		}
		if (!flag)
		{
			foreach (StiDataSource dataSource in dictionary.DataSources)
			{
				if (dictionary.Restrictions.IsAllowShow(dataSource.Name, StiDataType.DataSource))
				{
					flag = true;
					break;
				}
			}
		}
		if (flag)
		{
			TreeNodeEx treeNodeEx = new TreeNodeEx(Loc.Get("PropertyMain", "DataSources"));
			((TreeNode)treeNodeEx).Tag = "DataSourcesX";
			((TreeNode)treeNodeEx).ImageKey = "DataSource";
			((TreeNode)treeNodeEx).SelectedImageKey = "DataSource";
			TreeNodeEx treeNodeEx2 = treeNodeEx;
			TreeNode val = new TreeNode("Loading");
			((TreeNode)treeNodeEx2).Nodes.Add(val);
			((TreeView)tvData).Nodes.Add((TreeNode)(object)treeNodeEx2);
		}
	}

	private void ExpandedDataX(TreeNode dataSourcesNode)
	{
		//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Expected O, but got Unknown
		//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d6: Expected O, but got Unknown
		if (dataSourcesNode.Nodes.Count != 1 || dataSourcesNode.Nodes[0].Text != "Loading")
		{
			return;
		}
		dataSourcesNode.Nodes.Clear();
		StiDictionary dictionary = Designer.Report.Dictionary;
		dataBuilder.ShowParameters = true;
		List<DataInfo> list = new List<DataInfo>();
		Hashtable hashtable = new Hashtable();
		foreach (StiDatabase database in dictionary.Databases)
		{
			if (dictionary.Restrictions.IsAllowShow(database.Name, StiDataType.Database))
			{
				DataInfo dataInfo = new DataInfo(database.Name.ToLowerInvariant(), database.ToString(), database, null)
				{
					SourceName = database.Name
				};
				hashtable[dataInfo.Key] = new List<StiDataSource>();
				list.Add(dataInfo);
			}
		}
		foreach (StiData item in dictionary.DataStore)
		{
			if (!item.IsReportData && hashtable[item.Alias.ToLowerInvariant()] == null)
			{
				DataInfo dataInfo2 = new DataInfo(item.Alias.ToLowerInvariant(), item.Alias, null, item.Alias);
				if (item.Data != null && item.Data is DataTable && item.Name.Contains("." + ((DataTable)item.Data).TableName))
				{
					string sourceName = item.Name.Substring(0, item.Name.Length - ((DataTable)item.Data).TableName.Length - 1);
					dataInfo2.SourceName = sourceName;
				}
				else
				{
					dataInfo2.SourceName = item.Name;
				}
				dataInfo2.DataInStore = true;
				dataInfo2.Data = item;
				hashtable[dataInfo2.Key] = new List<StiDataSource>();
				list.Add(dataInfo2);
			}
		}
		foreach (StiDataSource dataSource in dictionary.DataSources)
		{
			if (!dictionary.Restrictions.IsAllowShow(dataSource.Name, StiDataType.DataSource))
			{
				continue;
			}
			string categoryName = dataSource.GetCategoryName();
			string text = categoryName;
			if (dataSource is StiDataTableSource)
			{
				int num = categoryName.LastIndexOf("[", StringComparison.InvariantCulture);
				int num2 = categoryName.LastIndexOf("]", StringComparison.InvariantCulture);
				if (num != -1 && num2 != -1)
				{
					text = categoryName.Substring(num + 1, num2 - num - 1);
				}
			}
			string key = string.Empty;
			bool flag = false;
			foreach (DataInfo item2 in list)
			{
				if (item2.SourceName == text || item2.Key == text.ToLowerInvariant())
				{
					key = item2.Key;
					flag = true;
					break;
				}
			}
			if (flag)
			{
				(hashtable[key] as List<StiDataSource>)?.Add(dataSource);
			}
		}
		list.Sort(new DataInfo());
		if (list.Count <= 0)
		{
			return;
		}
		foreach (DataInfo item3 in list)
		{
			TreeNode val = null;
			if (item3.Database != null)
			{
				val = new TreeNode(item3.Database.ToString())
				{
					Tag = item3.Database
				};
			}
			else if (item3.DataName != null)
			{
				val = new TreeNode(item3.SourceName);
			}
			TreeNode obj = val;
			string imageKey = (val.SelectedImageKey = GetCategoryImageKey(item3));
			obj.ImageKey = imageKey;
			if (hashtable[item3.Key] is List<StiDataSource> list2)
			{
				foreach (StiDataSource item4 in list2)
				{
					TreeNodeEx dataNode = new TreeNodeEx();
					dataBuilder.BuildDataSource(item4, ref dataNode, includeCalcColumn: true);
					if (item4 is StiDataTransformation)
					{
						TreeNodeEx treeNodeEx = dataNode;
						imageKey = (((TreeNode)dataNode).SelectedImageKey = (item4.Inherited ? "LockedDataTransformation" : "DataTransformation"));
						((TreeNode)treeNodeEx).ImageKey = imageKey;
					}
					else
					{
						TreeNodeEx treeNodeEx2 = dataNode;
						imageKey = (((TreeNode)dataNode).SelectedImageKey = (item4.Inherited ? "LockedDataSource" : "DataSource"));
						((TreeNode)treeNodeEx2).ImageKey = imageKey;
					}
					if (item4 is StiUndefinedDataSource)
					{
						TreeNodeEx treeNodeEx3 = dataNode;
						imageKey = (((TreeNode)dataNode).SelectedImageKey = "UndefinedDataSource");
						((TreeNode)treeNodeEx3).ImageKey = imageKey;
					}
					val.Nodes.Add((TreeNode)(object)dataNode);
				}
			}
			dataSourcesNode.Nodes.Add(val);
		}
	}

	public void BuildBusinessObjects(StiDictionary dictionary)
	{
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		if (StiOptions.Designer.IsBeginnerDesigner || StiOptions.Designer.IsBICreatorDesigner)
		{
			return;
		}
		if (dataBuilder == null)
		{
			dataBuilder = new StiDataBuilder(UsedRelations, UsedDataSources, UsedColumns, tvData);
		}
		dataBuilder.ShowParameters = true;
		bool flag = StiOptions.Designer.Panels.Dictionary.ShowEmptyBusinessObjectsCategory;
		if (!flag)
		{
			foreach (StiBusinessObject businessObject in dictionary.BusinessObjects)
			{
				if (businessObject.Dictionary == null || businessObject.Dictionary.Restrictions.IsAllowShow(businessObject.Name, StiDataType.BusinessObject))
				{
					flag = true;
					break;
				}
			}
		}
		if (flag)
		{
			TreeNode val = new TreeNode(Loc.Get("Report", "BusinessObjects"))
			{
				Tag = "BusinessObjects",
				ImageKey = "BusinessObject",
				SelectedImageKey = "BusinessObject"
			};
			dataBuilder.BuildBusinessObjects(dictionary.BusinessObjects, val, includeColumns: true, includeCalcColumns: true);
			((TreeView)tvData).Nodes.Add(val);
		}
	}

	public void BuildVariables(StiDictionary dictionary)
	{
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Expected O, but got Unknown
		TreeNodeEx treeNodeEx = new TreeNodeEx(Loc.Get("PropertyMain", "Variables"));
		((TreeNode)treeNodeEx).Tag = "Variables";
		((TreeNode)treeNodeEx).ImageKey = "Variables";
		((TreeNode)treeNodeEx).SelectedImageKey = "Variables";
		TreeNodeEx treeNodeEx2 = treeNodeEx;
		bool flag = false;
		foreach (StiVariable variable in dictionary.Variables)
		{
			if (dictionary.Restrictions.IsAllowShow(variable.Name, StiDataType.Variable))
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			TreeNode val = new TreeNode("Loading");
			((TreeNode)treeNodeEx2).Nodes.Add(val);
		}
		if (flag || StiOptions.Designer.Panels.Dictionary.ShowEmptyVariablesCategory)
		{
			((TreeView)tvData).Nodes.Add((TreeNode)(object)treeNodeEx2);
		}
	}

	private void ExpandedVariables(TreeNodeEx variablesNode)
	{
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Expected O, but got Unknown
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		if (((TreeNode)variablesNode).Nodes.Count != 1 || ((TreeNode)variablesNode).Nodes[0].Text != "Loading")
		{
			return;
		}
		((TreeNode)variablesNode).Nodes.Clear();
		StiDictionary dictionary = Designer.Report.Dictionary;
		foreach (StiVariable variable in dictionary.Variables)
		{
			if (!dictionary.Restrictions.IsAllowShow(variable.Name, StiDataType.Variable))
			{
				continue;
			}
			TreeNode val = (TreeNode)(object)variablesNode;
			if (variable.Category.Length > 0)
			{
				foreach (TreeNode node in ((TreeNode)variablesNode).Nodes)
				{
					TreeNode val2 = node;
					StiVariable stiVariable2 = val2.Tag as StiVariable;
					if (stiVariable2.Category == variable.Category)
					{
						val = val2;
					}
				}
				if (val == variablesNode)
				{
					TreeNode val3 = new TreeNode(variable.Category);
					string selectedImageKey = (val3.ImageKey = (variable.Inherited ? "LockedCategory" : "Category"));
					val3.SelectedImageKey = selectedImageKey;
					val3.Tag = variable;
					((TreeNode)variablesNode).Nodes.Add(val3);
					val = val3;
				}
			}
			if (variable.Name.Length > 0)
			{
				string text2 = variable.ToString(StiOptions.Dictionary.ShowOnlyAliasForVariable && Designer.UseAliases);
				string imageKeyFromVariable = StiDataBuilder.GetImageKeyFromVariable(variable);
				val.Nodes.Add(new TreeNode
				{
					Text = text2,
					ImageKey = imageKeyFromVariable,
					SelectedImageKey = imageKeyFromVariable,
					Tag = variable
				});
			}
		}
	}

	public void BuildSystemVariables()
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		TreeNodeEx treeNodeEx = new TreeNodeEx(Loc.Get("PropertyMain", "SystemVariables"));
		((TreeNode)treeNodeEx).Tag = "SystemVariables";
		((TreeNode)treeNodeEx).ImageKey = "SystemVariables";
		((TreeNode)treeNodeEx).SelectedImageKey = "SystemVariables";
		TreeNodeEx treeNodeEx2 = treeNodeEx;
		TreeNode val = new TreeNode("Loading");
		((TreeNode)treeNodeEx2).Nodes.Add(val);
		((TreeView)tvData).Nodes.Add((TreeNode)(object)treeNodeEx2);
	}

	private void ExpandedSystemVariables(TreeNodeEx systemVariablesNode)
	{
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		if (((TreeNode)systemVariablesNode).Nodes.Count != 1 || ((TreeNode)systemVariablesNode).Nodes[0].Text != "Loading")
		{
			return;
		}
		((TreeNode)systemVariablesNode).Nodes.Clear();
		StiDictionary dictionary = Designer.Report.Dictionary;
		List<string> systemVariables = StiSystemVariablesHelper.GetSystemVariables(dictionary.Report);
		foreach (string item in systemVariables)
		{
			TreeNode val = new TreeNode(item);
			string imageKey = (val.SelectedImageKey = "SystemVariable" + item);
			val.ImageKey = imageKey;
			val.Tag = "{" + item + "}";
			((TreeNode)systemVariablesNode).Nodes.Add(val);
		}
	}

	public void BuildFunctions()
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected O, but got Unknown
		TreeNodeEx treeNodeEx = new TreeNodeEx(Loc.Get("PropertyMain", "Functions"));
		((TreeNode)treeNodeEx).Tag = "Functions";
		((TreeNode)treeNodeEx).ImageKey = "Function";
		((TreeNode)treeNodeEx).SelectedImageKey = "Function";
		TreeNodeEx treeNodeEx2 = treeNodeEx;
		((TreeNode)treeNodeEx2).Nodes.Add(new TreeNode("Loading"));
		((TreeView)tvData).Nodes.Add((TreeNode)(object)treeNodeEx2);
	}

	private void ExpandedFunctions(TreeNodeEx functionsNode)
	{
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Expected O, but got Unknown
		if (((TreeNode)functionsNode).Nodes.Count != 1 || ((TreeNode)functionsNode).Nodes[0].Text != "Loading")
		{
			return;
		}
		((TreeNode)functionsNode).Nodes.Clear();
		StiDictionary dictionary = Designer.Report.Dictionary;
		Hashtable functionsGrouppedInCategories = StiFunctions.GetFunctionsGrouppedInCategories();
		string[] array = new string[functionsGrouppedInCategories.Keys.Count];
		functionsGrouppedInCategories.Keys.CopyTo(array, 0);
		Array.Sort(array);
		string[] array2 = array;
		foreach (string text in array2)
		{
			TreeNode val = new TreeNode(text);
			string selectedImageKey = (val.ImageKey = "Category");
			val.SelectedImageKey = selectedImageKey;
			((TreeNode)functionsNode).Nodes.Add(val);
			List<StiFunction> list = functionsGrouppedInCategories[text] as List<StiFunction>;
			Hashtable hashtable = new Hashtable();
			foreach (StiFunction item in list)
			{
				hashtable[item.GroupFunctionName] = item.GroupFunctionName;
			}
			string[] array3 = new string[hashtable.Count];
			hashtable.Keys.CopyTo(array3, 0);
			Array.Sort(array3);
			string[] array4 = array3;
			foreach (string text3 in array4)
			{
				StiFunction[] functions = StiFunctions.GetFunctions(dictionary.Report, text3, isCompile: false);
				Array.Sort(functions);
				TreeNode val2 = val;
				if (functions.Length > 1)
				{
					val2 = new TreeNode(text3);
					TreeNode obj = val2;
					selectedImageKey = (val2.SelectedImageKey = "Function");
					obj.ImageKey = selectedImageKey;
					val.Nodes.Add(val2);
				}
				StiFunction[] array5 = functions;
				foreach (StiFunction stiFunction in array5)
				{
					TreeNode val3 = new TreeNode(stiFunction.GetFunctionString(dictionary.Report.ScriptLanguage));
					selectedImageKey = (val3.SelectedImageKey = "Function");
					val3.ImageKey = selectedImageKey;
					val3.Tag = stiFunction;
					val2.Nodes.Add(val3);
				}
			}
		}
	}

	public void BuildFormats()
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		if (CanBuildFormats)
		{
			TreeNodeEx treeNodeEx = new TreeNodeEx(Loc.Get("PropertyMain", "Format"));
			((TreeNode)treeNodeEx).Tag = "Format";
			((TreeNode)treeNodeEx).ImageKey = "Format";
			((TreeNode)treeNodeEx).SelectedImageKey = "Format";
			TreeNodeEx treeNodeEx2 = treeNodeEx;
			TreeNode val = new TreeNode("Loading");
			((TreeNode)treeNodeEx2).Nodes.Add(val);
			((TreeView)tvData).Nodes.Add((TreeNode)(object)treeNodeEx2);
		}
	}

	private void ExpandedFormats(TreeNodeEx formatsNode)
	{
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Expected O, but got Unknown
		if (((TreeNode)formatsNode).Nodes.Count != 1 || ((TreeNode)formatsNode).Nodes[0].Text != "Loading")
		{
			return;
		}
		((TreeNode)formatsNode).Nodes.Clear();
		foreach (StiFormatService item in StiOptions.Services.Formats.Where((StiFormatService s) => s.ServiceEnabled))
		{
			string text = "Format";
			if (item is StiGeneralFormatService)
			{
				text = "FormatGeneral";
			}
			else if (item is StiPercentageFormatService)
			{
				text = "FormatPercentage";
			}
			else if (item is StiCurrencyFormatService)
			{
				text = "FormatCurrency";
			}
			else if (item is StiBooleanFormatService)
			{
				text = "FormatBoolean";
			}
			else if (item is StiDateFormatService)
			{
				text = "FormatDate";
			}
			else if (item is StiNumberFormatService)
			{
				text = "FormatNumber";
			}
			else if (item is StiTimeFormatService)
			{
				text = "FormatTime";
			}
			((TreeNode)formatsNode).Nodes.Add(new TreeNode(item.ServiceName)
			{
				ImageKey = text,
				SelectedImageKey = text,
				Tag = item
			});
		}
	}

	public void BuildHtmlTags()
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		if (CanBuildHtmlTags)
		{
			TreeNodeEx treeNodeEx = new TreeNodeEx(Loc.Get("PropertyMain", "HtmlTags"));
			((TreeNode)treeNodeEx).Tag = "HtmlTags";
			((TreeNode)treeNodeEx).ImageKey = "HtmlTag";
			((TreeNode)treeNodeEx).SelectedImageKey = "HtmlTag";
			TreeNodeEx treeNodeEx2 = treeNodeEx;
			TreeNode val = new TreeNode("Loading");
			((TreeNode)treeNodeEx2).Nodes.Add(val);
			((TreeView)tvData).Nodes.Add((TreeNode)(object)treeNodeEx2);
		}
	}

	private void ExpandedHtmlTags(TreeNodeEx htmlNode)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Expected O, but got Unknown
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Expected O, but got Unknown
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Expected O, but got Unknown
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Expected O, but got Unknown
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Expected O, but got Unknown
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Expected O, but got Unknown
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Expected O, but got Unknown
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Expected O, but got Unknown
		//IL_0307: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Expected O, but got Unknown
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Expected O, but got Unknown
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Expected O, but got Unknown
		//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Expected O, but got Unknown
		//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e9: Expected O, but got Unknown
		//IL_041a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Expected O, but got Unknown
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_0457: Expected O, but got Unknown
		//IL_0488: Unknown result type (might be due to invalid IL or missing references)
		//IL_048e: Expected O, but got Unknown
		//IL_04bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Expected O, but got Unknown
		//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fc: Expected O, but got Unknown
		//IL_052d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0533: Expected O, but got Unknown
		//IL_0564: Unknown result type (might be due to invalid IL or missing references)
		//IL_056a: Expected O, but got Unknown
		if (((TreeNode)htmlNode).Nodes.Count == 1 && !(((TreeNode)htmlNode).Nodes[0].Text != "Loading"))
		{
			((TreeNode)htmlNode).Nodes.Clear();
			TreeNode val = new TreeNode("<b> </b>");
			TreeNode obj = val;
			string imageKey = (val.SelectedImageKey = "HtmlTag");
			obj.ImageKey = imageKey;
			val.Tag = "<b>IDENT</b>";
			((TreeNode)htmlNode).Nodes.Add(val);
			val = new TreeNode("<i> </i>");
			TreeNode obj2 = val;
			imageKey = (val.SelectedImageKey = "HtmlTag");
			obj2.ImageKey = imageKey;
			val.Tag = "<i>IDENT</i>";
			((TreeNode)htmlNode).Nodes.Add(val);
			val = new TreeNode("<u> </u>");
			TreeNode obj3 = val;
			imageKey = (val.SelectedImageKey = "HtmlTag");
			obj3.ImageKey = imageKey;
			val.Tag = "<u>IDENT</u>";
			((TreeNode)htmlNode).Nodes.Add(val);
			val = new TreeNode("<s> </s>");
			TreeNode obj4 = val;
			imageKey = (val.SelectedImageKey = "HtmlTag");
			obj4.ImageKey = imageKey;
			val.Tag = "<s>IDENT</s>";
			((TreeNode)htmlNode).Nodes.Add(val);
			val = new TreeNode("<sub> </sub>");
			TreeNode obj5 = val;
			imageKey = (val.SelectedImageKey = "HtmlTag");
			obj5.ImageKey = imageKey;
			val.Tag = "<sub>IDENT</sub>";
			((TreeNode)htmlNode).Nodes.Add(val);
			val = new TreeNode("<sup> </sup>");
			TreeNode obj6 = val;
			imageKey = (val.SelectedImageKey = "HtmlTag");
			obj6.ImageKey = imageKey;
			val.Tag = "<sup>IDENT</sup>";
			((TreeNode)htmlNode).Nodes.Add(val);
			val = new TreeNode("<font color=\"red\" face=\"Arial\" size=\"8\"> </font>");
			TreeNode obj7 = val;
			imageKey = (val.SelectedImageKey = "HtmlTag");
			obj7.ImageKey = imageKey;
			val.Tag = "<font color=\"red\" face=\"Arial\" size=\"8\">IDENT</font>";
			((TreeNode)htmlNode).Nodes.Add(val);
			val = new TreeNode("<font-name=\"Arial\"> </font-name>");
			TreeNode obj8 = val;
			imageKey = (val.SelectedImageKey = "HtmlTag");
			obj8.ImageKey = imageKey;
			val.Tag = "<font-name=\"Arial\">IDENT</font-name>";
			((TreeNode)htmlNode).Nodes.Add(val);
			val = new TreeNode("<font-size=\"8\"> </font-size>");
			TreeNode obj9 = val;
			imageKey = (val.SelectedImageKey = "HtmlTag");
			obj9.ImageKey = imageKey;
			val.Tag = "<font-size=\"8\">IDENT</font-size>";
			((TreeNode)htmlNode).Nodes.Add(val);
			val = new TreeNode("<font-color=\"red\"> </font-color>");
			TreeNode obj10 = val;
			imageKey = (val.SelectedImageKey = "HtmlTag");
			obj10.ImageKey = imageKey;
			val.Tag = "<font-color=\"red\">IDENT</font-color>";
			((TreeNode)htmlNode).Nodes.Add(val);
			val = new TreeNode("<color=\"red\"> </color>");
			TreeNode obj11 = val;
			imageKey = (val.SelectedImageKey = "HtmlTag");
			obj11.ImageKey = imageKey;
			val.Tag = "<color=\"red\">IDENT</color>";
			((TreeNode)htmlNode).Nodes.Add(val);
			val = new TreeNode("<background-color=\"red\"> </background-color>");
			TreeNode obj12 = val;
			imageKey = (val.SelectedImageKey = "HtmlTag");
			obj12.ImageKey = imageKey;
			val.Tag = "<background-color=\"red\">IDENT</background-color>";
			((TreeNode)htmlNode).Nodes.Add(val);
			val = new TreeNode("<letter-spacing=\"0\"> </letter-spacing>");
			TreeNode obj13 = val;
			imageKey = (val.SelectedImageKey = "HtmlTag");
			obj13.ImageKey = imageKey;
			val.Tag = "<letter-spacing=\"0\">IDENT</letter-spacing>";
			((TreeNode)htmlNode).Nodes.Add(val);
			val = new TreeNode("<word-spacing=\"0\"> </word-spacing>");
			TreeNode obj14 = val;
			imageKey = (val.SelectedImageKey = "HtmlTag");
			obj14.ImageKey = imageKey;
			val.Tag = "<word-spacing=\"0\">IDENT</word-spacing>";
			((TreeNode)htmlNode).Nodes.Add(val);
			val = new TreeNode("<line-height=\"1\"> </line-height>");
			TreeNode obj15 = val;
			imageKey = (val.SelectedImageKey = "HtmlTag");
			obj15.ImageKey = imageKey;
			val.Tag = "<line-height=\"1\">IDENT</line-height>";
			((TreeNode)htmlNode).Nodes.Add(val);
			val = new TreeNode("<text-align=\"left\"> </text-align>");
			TreeNode obj16 = val;
			imageKey = (val.SelectedImageKey = "HtmlTag");
			obj16.ImageKey = imageKey;
			val.Tag = "<text-align=\"left\">IDENT</text-align>";
			((TreeNode)htmlNode).Nodes.Add(val);
			val = new TreeNode("<text-align=\"center\"> </text-align>");
			TreeNode obj17 = val;
			imageKey = (val.SelectedImageKey = "HtmlTag");
			obj17.ImageKey = imageKey;
			val.Tag = "<text-align=\"center\">IDENT</text-align>";
			((TreeNode)htmlNode).Nodes.Add(val);
			val = new TreeNode("<text-align=\"right\"> </text-align>");
			TreeNode obj18 = val;
			imageKey = (val.SelectedImageKey = "HtmlTag");
			obj18.ImageKey = imageKey;
			val.Tag = "<text-align=\"right\">IDENT</text-align>";
			((TreeNode)htmlNode).Nodes.Add(val);
			val = new TreeNode("<text-align=\"justify\"> </text-align>");
			TreeNode obj19 = val;
			imageKey = (val.SelectedImageKey = "HtmlTag");
			obj19.ImageKey = imageKey;
			val.Tag = "<text-align=\"justify\">IDENT</text-align>";
			((TreeNode)htmlNode).Nodes.Add(val);
			val = new TreeNode("<br>");
			TreeNode obj20 = val;
			imageKey = (val.SelectedImageKey = "HtmlTag");
			obj20.ImageKey = imageKey;
			val.Tag = "<br>";
			((TreeNode)htmlNode).Nodes.Add(val);
			val = new TreeNode("&amp;");
			TreeNode obj21 = val;
			imageKey = (val.SelectedImageKey = "HtmlTag");
			obj21.ImageKey = imageKey;
			val.Tag = "&amp;";
			((TreeNode)htmlNode).Nodes.Add(val);
			val = new TreeNode("&lt;");
			TreeNode obj22 = val;
			imageKey = (val.SelectedImageKey = "HtmlTag");
			obj22.ImageKey = imageKey;
			val.Tag = "&lt;";
			((TreeNode)htmlNode).Nodes.Add(val);
			val = new TreeNode("&gt;");
			TreeNode obj23 = val;
			imageKey = (val.SelectedImageKey = "HtmlTag");
			obj23.ImageKey = imageKey;
			val.Tag = "&gt;";
			((TreeNode)htmlNode).Nodes.Add(val);
			val = new TreeNode("&quot;");
			TreeNode obj24 = val;
			imageKey = (val.SelectedImageKey = "HtmlTag");
			obj24.ImageKey = imageKey;
			val.Tag = "&quot;";
			((TreeNode)htmlNode).Nodes.Add(val);
			val = new TreeNode("&nbsp;");
			TreeNode obj25 = val;
			imageKey = (val.SelectedImageKey = "HtmlTag");
			obj25.ImageKey = imageKey;
			val.Tag = "&nbsp;";
			((TreeNode)htmlNode).Nodes.Add(val);
		}
	}

	public void BuildResources(StiDictionary dictionary)
	{
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Expected O, but got Unknown
		TreeNodeEx treeNodeEx = new TreeNodeEx(Loc.Get("PropertyMain", "Resources"));
		((TreeNode)treeNodeEx).Tag = "Resources";
		((TreeNode)treeNodeEx).ImageKey = "Resources";
		((TreeNode)treeNodeEx).SelectedImageKey = "Resources";
		TreeNodeEx treeNodeEx2 = treeNodeEx;
		bool flag = false;
		foreach (StiResource resource in dictionary.Resources)
		{
			if (dictionary.Restrictions.IsAllowShow(resource.Name, StiDataType.Resource))
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			TreeNode val = new TreeNode("Loading");
			((TreeNode)treeNodeEx2).Nodes.Add(val);
		}
		if (flag || StiOptions.Designer.Panels.Dictionary.ShowEmptyResourcesCategory)
		{
			((TreeView)tvData).Nodes.Add((TreeNode)(object)treeNodeEx2);
		}
	}

	private void ExpandedResources(TreeNodeEx resourcesNode)
	{
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		if (((TreeNode)resourcesNode).Nodes.Count != 1 || ((TreeNode)resourcesNode).Nodes[0].Text != "Loading")
		{
			return;
		}
		((TreeNode)resourcesNode).Nodes.Clear();
		StiDictionary dictionary = Designer.Report.Dictionary;
		foreach (StiResource resource in dictionary.Resources)
		{
			if (dictionary.Restrictions.IsAllowShow(resource.Name, StiDataType.Resource))
			{
				if (resource.Name.Length > 0)
				{
					string text = resource.ToString(StiOptions.Dictionary.ShowOnlyAliasForResource && Designer.UseAliases);
					string imageKeyFromResource = StiDataBuilder.GetImageKeyFromResource(resource);
					((TreeNode)resourcesNode).Nodes.Add(new TreeNode
					{
						Text = text,
						ImageKey = imageKeyFromResource,
						SelectedImageKey = imageKeyFromResource,
						Tag = resource
					});
				}
			}
		}
	}

	private void TreeNode_Collapsed(object sender, TreeViewCancelEventArgs e)
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		if (e.Node is TreeNodeEx && e.Node.Tag != null)
		{
			object tag = e.Node.Tag;
			switch (tag as string)
			{
			case "DataSources":
			case "DataSourcesX":
			{
				e.Node.Nodes.Clear();
				TreeNode val = new TreeNode("Loading");
				e.Node.Nodes.Add(val);
				break;
			}
			case null:
				dataBuilder.CollapsedNode(e.Node);
				break;
			}
		}
	}

	private void TreeNode_Expand(object sender, TreeViewCancelEventArgs e)
	{
		if (!(e.Node is TreeNodeEx treeNodeEx) || e.Node.Tag == null)
		{
			return;
		}
		object tag = e.Node.Tag;
		if (tag is string text)
		{
			((TreeView)tvData).BeginUpdate();
			switch (text)
			{
			case "DataSources":
				ExpandedData((TreeNode)(object)treeNodeEx);
				break;
			case "DataSourcesX":
				ExpandedDataX((TreeNode)(object)treeNodeEx);
				break;
			case "Variables":
				ExpandedVariables(treeNodeEx);
				break;
			case "Resources":
				ExpandedResources(treeNodeEx);
				break;
			case "SystemVariables":
				ExpandedSystemVariables(treeNodeEx);
				break;
			case "Functions":
				ExpandedFunctions(treeNodeEx);
				break;
			case "Format":
				ExpandedFormats(treeNodeEx);
				break;
			case "HtmlTags":
				ExpandedHtmlTags(treeNodeEx);
				break;
			}
			((TreeView)tvData).EndUpdate();
		}
		else
		{
			dataBuilder.ExpandedNode(treeNodeEx, includeCalcColumn: true);
		}
	}

	public void ExpandTreeNode(TreeNode node)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		if (node != null)
		{
			TreeViewCancelEventArgs e = new TreeViewCancelEventArgs(node, false, (TreeViewAction)4);
			TreeNode_Expand(node.TreeView, e);
		}
	}

	private void SetInfoForFunction(string functionName, string longFunctionString, string description, string[] argumentNames, string[] argumentDescriptions, Type[] argumentTypes, string returnDescription)
	{
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Expected O, but got Unknown
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Expected O, but got Unknown
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Expected O, but got Unknown
		EnableInformation();
		string text2 = (((Control)information).Text = longFunctionString + "\n\n");
		string text3 = text2;
		((Control)information).Text = text3;
		int length = ((Control)information).Text.Length;
		RichTextBox obj = information;
		((Control)obj).Text = ((Control)obj).Text + description + "\n";
		int num = -1;
		int num2 = -1;
		if (argumentNames != null && argumentNames.Length != 0)
		{
			num = ((Control)information).Text.Length;
			RichTextBox obj2 = information;
			((Control)obj2).Text = ((Control)obj2).Text + "\n" + Loc.Get("PropertyMain", "Parameters") + " \n";
			num2 = ((Control)information).Text.Length;
			int num3 = 0;
			foreach (string text4 in argumentNames)
			{
				if (argumentDescriptions != null && num3 < argumentDescriptions.Length)
				{
					if (argumentTypes != null && argumentTypes[num3].IsArray)
					{
						RichTextBox obj3 = information;
						((Control)obj3).Text = ((Control)obj3).Text + argumentDescriptions[num3++] + "\n";
						continue;
					}
					RichTextBox val = information;
					((Control)val).Text = ((Control)val).Text + text4 + " - " + argumentDescriptions[num3++] + "\n";
				}
				else
				{
					RichTextBox obj4 = information;
					((Control)obj4).Text = ((Control)obj4).Text + text4 + "\n";
				}
			}
		}
		int num4 = -1;
		int num5 = -1;
		if (!string.IsNullOrEmpty(returnDescription))
		{
			num4 = ((Control)information).Text.Length;
			RichTextBox obj5 = information;
			((Control)obj5).Text = ((Control)obj5).Text + "\n" + Loc.Get("PropertyMain", "ReturnValue") + "\n";
			num5 = ((Control)information).Text.Length;
			RichTextBox obj6 = information;
			((Control)obj6).Text = ((Control)obj6).Text + returnDescription;
		}
		((TextBoxBase)information).Select(0, length);
		information.SelectionColor = (StiUX.IsDark ? StiUX.InputForeground : Color.Navy);
		int num6 = text3.IndexOf(functionName, StringComparison.InvariantCulture);
		int length2 = functionName.Length;
		((TextBoxBase)information).Select(num6, length2);
		information.SelectionFont = new Font("Arial", 8f, (FontStyle)1);
		information.SelectionColor = (StiUX.IsDark ? StiUX.InputForeground : Color.Black);
		if (num != -1)
		{
			((TextBoxBase)information).Select(num, num2 - num);
			information.SelectionFont = new Font("Arial", 8f, (FontStyle)1);
		}
		if (num5 != -1)
		{
			((TextBoxBase)information).Select(num4, num5 - num4);
			information.SelectionFont = new Font("Arial", 8f, (FontStyle)1);
		}
	}

	private TreeNode GetNodeByTag(TreeNodeCollection nodes, object tag)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		foreach (TreeNode node in nodes)
		{
			TreeNode val = node;
			if (val.Tag == tag)
			{
				return val;
			}
			TreeNode nodeByTag = GetNodeByTag(val.Nodes, tag);
			if (nodeByTag != null)
			{
				return nodeByTag;
			}
		}
		return null;
	}

	public TreeNode GetNodeByKey(string key)
	{
		return GetNodeByKey(((TreeView)tvData).Nodes, key);
	}

	private TreeNode GetNodeByKey(TreeNodeCollection nodes, string key)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		foreach (TreeNode node in nodes)
		{
			TreeNode val = node;
			if (val.Tag is IStiAppCell && ((IStiAppCell)val.Tag).GetKey() == key)
			{
				return val;
			}
			TreeNode nodeByKey = GetNodeByKey(val.Nodes, key);
			if (nodeByKey != null)
			{
				return nodeByKey;
			}
		}
		return null;
	}

	public TreeNode GetNodeByTag(object tag)
	{
		return GetNodeByTag(((TreeView)tvData).Nodes, tag);
	}

	public TreeNode SelectNodeByTag(object tag)
	{
		if (tag == null)
		{
			return null;
		}
		((TreeView)tvData).SelectedNode = GetNodeByTag(tag);
		if (((TreeView)tvData).SelectedNode != null)
		{
			((TreeView)tvData).SelectedNode.EnsureVisible();
		}
		return ((TreeView)tvData).SelectedNode;
	}

	public TreeNode SelectNodeByKey(string key)
	{
		if (key == null)
		{
			return null;
		}
		((TreeView)tvData).SelectedNode = GetNodeByKey(key);
		if (((TreeView)tvData).SelectedNode != null)
		{
			((TreeView)tvData).SelectedNode.EnsureVisible();
		}
		return ((TreeView)tvData).SelectedNode;
	}

	private void TreeViewData_ItemDrag(object sender, ItemDragEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb4: Unknown result type (might be due to invalid IL or missing references)
		if (!(e.Item is TreeNode) || ((TreeNode)e.Item).Parent == null || !CanDragDrop)
		{
			return;
		}
		TreeNode val2 = (DraggedNode = (TreeNode)/*isinst with value type is only supported in some contexts*/);
		bool flag = false;
		string text = null;
		string mapIdent = null;
		string textLabel = val2.Text.Replace("{", "").Replace("}", "");
		string text2 = null;
		if (val2.Tag is string text3)
		{
			text = text3;
		}
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		bool flag6 = false;
		bool flag7 = false;
		StiDataColumn stiDataColumn = val2.Tag as StiDataColumn;
		if (stiDataColumn != null && CanDragDropDataColumn)
		{
			text = StiDataBuilder.GetColumnPathFromNode(val2, useRelationName: false, IsDataExpression);
			textLabel = stiDataColumn.Alias;
			if (stiDataColumn.Type == typeof(bool))
			{
				if (Designer.IsFormSelected)
				{
					flag2 = true;
				}
				if (Designer.IsPageSelected)
				{
					flag3 = true;
				}
			}
			else if (stiDataColumn.Type == typeof(Image) || stiDataColumn.Type == typeof(byte[]))
			{
				if (Designer.IsDashboardSelected)
				{
					flag5 = true;
				}
				if (Designer.IsPageSelected)
				{
					flag4 = true;
				}
			}
		}
		StiResource stiResource = val2.Tag as StiResource;
		if (stiResource != null && CanDragDropResource && (stiResource.Type == StiResourceType.Image || stiResource.Type == StiResourceType.Rtf || stiResource.Type == StiResourceType.Report || stiResource.Type == StiResourceType.ReportSnapshot))
		{
			flag6 = Designer.IsPageSelected;
			flag7 = Designer.IsDashboardSelected;
			text2 = StiHyperlinkProcessor.CreateResourceName(stiResource.Name);
			textLabel = stiResource.Alias;
			text = text2;
		}
		if (stiResource != null && CanDragDropResource && stiResource.Type == StiResourceType.Map && StiCustomMapFinder.IsCorrentMap(Designer.Report, stiResource.Name))
		{
			flag6 = Designer.IsPageSelected;
			flag7 = Designer.IsDashboardSelected;
			text2 = StiHyperlinkProcessor.CreateResourceName(stiResource.Name);
			textLabel = stiResource.Alias;
			text = stiResource.Name;
			mapIdent = stiResource.Name;
			flag = true;
		}
		if (val2.Tag is StiDataParameter && CanDragDropDataParameter)
		{
			text = StiDataBuilder.GetParameterPathFromNode(val2);
			textLabel = null;
		}
		PropertyInfo propertyInfo = val2.Tag as PropertyInfo;
		if (propertyInfo != null && CanDragDropProperty)
		{
			text = StiDataBuilder.GetPropertyInfoPathFromNode(val2);
			textLabel = propertyInfo.Name;
		}
		StiAggregateFunctionService stiAggregateFunctionService = val2.Tag as StiAggregateFunctionService;
		if (stiAggregateFunctionService != null && CanDragDropTotals)
		{
			text = stiAggregateFunctionService.ServiceName + "()";
			textLabel = Loc.Get("PropertyMain", "Total");
		}
		StiVariable stiVariable = val2.Tag as StiVariable;
		if (stiVariable != null && CanDragDropVariable)
		{
			string expressionString = StiVariable.GetExpressionString(stiVariable);
			text = (IsDataExpression ? Funcs.ToExpression(expressionString) : StiNameValidator.CorrectName(expressionString, Designer?.Report));
			textLabel = stiVariable.Alias;
		}
		if (val2.Tag is StiFunction stiFunction && CanDragDropFunction)
		{
			text = $"{stiFunction.FunctionName}({((stiFunction.ArgumentNames == null || stiFunction.ArgumentNames.Length == 0) ? string.Empty : new string(',', stiFunction.ArgumentNames.Length - 1))})";
			textLabel = stiFunction.FunctionName;
		}
		StiFormatService stiFormatService = val2.Tag as StiFormatService;
		if (stiFormatService != null && CanDragDropTextFormat)
		{
			text = string.Empty;
			textLabel = null;
		}
		StiDataSource stiDataSource = val2.Tag as StiDataSource;
		if (stiDataSource != null && CanDragDropDataSource)
		{
			text = string.Empty;
			textLabel = null;
		}
		StiBusinessObject stiBusinessObject = val2.Tag as StiBusinessObject;
		if (stiBusinessObject != null && CanDragDropBusinessObjectColumn)
		{
			text = string.Empty;
			textLabel = null;
		}
		if (text == null)
		{
			return;
		}
		if (AddParentheses && text.Length > 0 && text.IndexOfInvariant("{") == -1)
		{
			text = "{" + text + "}";
		}
		StiComponent stiComponent;
		if (val2.Parent != null && val2.Parent.Text == Loc.Get("PropertyMain", "SystemVariables"))
		{
			if (Designer.IsFormSelected)
			{
				stiComponent = StiDialogsProvider.GetProvider(Designer.Report).CreateTextBoxControl() as StiComponent;
			}
			else if (Designer.IsDashboardSelected)
			{
				stiComponent = StiDashboardCreator.CreateTextElement(Designer.Report) as StiComponent;
				((IStiTextElement)stiComponent).Text = text;
			}
			else
			{
				stiComponent = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.SimpleComponents.Text) as StiText;
			}
		}
		else if (stiAggregateFunctionService != null)
		{
			if (Designer.IsFormSelected)
			{
				stiComponent = StiDialogsProvider.GetProvider(Designer.Report).CreateTextBoxControl() as StiComponent;
			}
			else if (Designer.IsDashboardSelected)
			{
				stiComponent = StiDashboardCreator.CreateTextElement(Designer.Report) as StiComponent;
				((IStiTextElement)stiComponent).Text = text;
			}
			else
			{
				stiComponent = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.SimpleComponents.Text) as StiText;
			}
		}
		else if (stiVariable != null && StiTypeFinder.FindType(stiVariable.Type, typeof(Image)))
		{
			if (Designer.IsFormSelected)
			{
				stiComponent = StiDialogsProvider.GetProvider(Designer.Report).CreatePictureBoxControl() as StiComponent;
			}
			else if (Designer.IsDashboardSelected)
			{
				stiComponent = StiDashboardCreator.CreateImageElement(Designer.Report) as StiComponent;
				((IStiImageElement)stiComponent).ImageHyperlink = StiHyperlinkProcessor.CreateVariableName(stiVariable.Name);
			}
			else
			{
				stiComponent = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.SimpleComponents.Image) as StiImage;
				((StiImage)stiComponent).ImageData.Value = "{" + stiVariable.Name + "}";
			}
		}
		else if (stiFormatService != null)
		{
			if (Designer.IsFormSelected)
			{
				stiComponent = null;
			}
			else if (Designer.IsDashboardSelected)
			{
				stiComponent = null;
			}
			else
			{
				stiComponent = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.SimpleComponents.Text) as StiText;
				((StiText)stiComponent).TextFormat = stiFormatService.Clone() as StiFormatService;
			}
		}
		else if (Designer.IsFormSelected)
		{
			stiComponent = StiDialogsProvider.GetProvider(Designer.Report).CreateTextBoxControl() as StiComponent;
		}
		else if (!Designer.IsDashboardSelected)
		{
			stiComponent = ((!flag) ? ((StiComponent)(StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.SimpleComponents.Text) as StiText)) : ((StiComponent)(StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.SimpleComponents.Map) as StiMap)));
		}
		else if (flag)
		{
			stiComponent = StiDashboardCreator.CreateMapElement(Designer.Report) as StiComponent;
			((IStiRegionMapElement)stiComponent).MapIdent = mapIdent;
		}
		else
		{
			if (stiVariable != null)
			{
				if (stiVariable.Type == typeof(DateTime) || stiVariable.Type == typeof(DateTime?) || stiVariable.Type == typeof(DateTimeRange))
				{
					stiComponent = StiDashboardCreator.CreateDatePickerElement(Designer.Report) as StiComponent;
					((IStiDatePickerElement)stiComponent).AddValueMeter(stiVariable);
				}
				else
				{
					stiComponent = StiDashboardCreator.CreateComboBoxElement(Designer.Report) as StiComponent;
					((IStiComboBoxElement)stiComponent).AddKeyMeter(stiVariable);
				}
			}
			else if (stiDataColumn != null && (stiDataColumn.Type == typeof(DateTime) || stiDataColumn.Type == typeof(DateTime?)))
			{
				stiComponent = StiDashboardCreator.CreateDatePickerElement(Designer.Report) as StiComponent;
				((IStiDatePickerElement)stiComponent).AddValueMeter(stiDataColumn);
			}
			else
			{
				stiComponent = StiDashboardCreator.CreateTextElement(Designer.Report) as StiComponent;
				((IStiTextElement)stiComponent).Text = text;
			}
			if (stiComponent is IStiTitleElement stiTitleElement && stiDataColumn != null)
			{
				stiTitleElement.Title.Text = StiHumanReadableHelper.GetHumanReadableName(stiDataColumn.Alias);
			}
		}
		if (flag2)
		{
			stiComponent = StiDialogsProvider.GetProvider(Designer.Report).CreateCheckBoxControl() as StiComponent;
			((IStiCheckBoxControl)stiComponent).Text = stiDataColumn.Alias;
			((IStiCheckBoxControl)stiComponent).CheckedBinding = text;
			textLabel = null;
		}
		if (flag4)
		{
			stiComponent = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.SimpleComponents.Image) as StiImage;
			((StiImage)stiComponent).DataColumn = text.Replace("{", "").Replace("}", "");
		}
		if (flag5)
		{
			string imageHyperlink = StiHyperlinkProcessor.CreateDataColumnName(text.Replace("{", "").Replace("}", ""));
			stiComponent = StiDashboardCreator.CreateImageElement(Designer.Report) as StiComponent;
			((IStiImageElement)stiComponent).ImageHyperlink = imageHyperlink;
			if (stiComponent is IStiTitleElement stiTitleElement2 && stiDataColumn != null)
			{
				stiTitleElement2.Title.Text = StiHumanReadableHelper.GetHumanReadableName(stiDataColumn.Alias);
			}
		}
		if (flag6)
		{
			switch (stiResource.Type)
			{
			case StiResourceType.Image:
				stiComponent = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.SimpleComponents.Image) as StiImage;
				((StiImage)stiComponent).ImageURL.Value = text2;
				break;
			case StiResourceType.Map:
				stiComponent = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.SimpleComponents.Map) as StiMap;
				((StiMap)stiComponent).MapIdent = mapIdent;
				break;
			case StiResourceType.Rtf:
				stiComponent = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.SimpleComponents.RickText) as StiRichText;
				((StiRichText)stiComponent).DataUrl.Value = text2;
				break;
			case StiResourceType.Report:
			case StiResourceType.ReportSnapshot:
				stiComponent = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.ComplexComponents.SubReport) as StiSubReport;
				((StiSubReport)stiComponent).SubReportUrl = text2;
				break;
			}
		}
		if (flag7)
		{
			if (flag)
			{
				stiComponent = StiDashboardCreator.CreateMapElement(Designer.Report) as StiComponent;
				((IStiRegionMapElement)stiComponent).MapIdent = mapIdent;
			}
			else
			{
				stiComponent = StiDashboardCreator.CreateImageElement(Designer.Report) as StiComponent;
				((IStiImageElement)stiComponent).ImageHyperlink = text2;
				if (stiComponent is IStiTitleElement stiTitleElement3 && stiResource != null)
				{
					stiTitleElement3.Title.Text = StiHumanReadableHelper.GetHumanReadableName(stiResource.Alias);
				}
			}
		}
		if (flag3)
		{
			stiComponent = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.SimpleComponents.CheckBox) as StiCheckBox;
			((StiCheckBox)stiComponent).Checked.Value = text;
		}
		if (stiDataSource != null || stiBusinessObject != null)
		{
			if (Designer.IsDashboardSelected)
			{
				IStiTableElement stiTableElement = StiDashboardCreator.CreateTableElement(Designer.Report);
				stiComponent = stiTableElement as StiComponent;
				if (stiDataSource != null)
				{
					if (stiTableElement is IStiTitleElement stiTitleElement4)
					{
						stiTitleElement4.Title.Text = StiHumanReadableHelper.GetHumanReadableName(stiDataSource.Alias);
					}
					stiTableElement.CreateMeters(stiDataSource);
					StiDashboardHelperCreator.CreateTableElementAutoSizer(stiTableElement)?.AutoMeasureToTable(stiTableElement);
				}
				string text4 = stiDataSource?.Name ?? stiBusinessObject?.Name;
				stiComponent.Name = StiNameCreation.CreateName(Designer.Report, Designer.Report.Info.GenerateLocalizedName ? (stiComponent.LocalizedName + text4) : ("Table" + text4), addOne: false, removeIncorrectSymbols: false, forceAdvancedNamingRule: true);
			}
			else
			{
				stiComponent = StiActivator.CreateObject(StiOptions.Designer.ComponentsTypes.Bands.DataBand) as StiDataBand;
				if (stiDataSource != null)
				{
					((StiDataBand)stiComponent).DataSourceName = StiNameValidator.CorrectName(stiDataSource.Name, Designer.Report);
				}
				else
				{
					((StiDataBand)stiComponent).BusinessObjectGuid = stiBusinessObject.Guid;
				}
				stiComponent.Name = StiNameCreation.CreateName(Designer.Report, Designer.Report.Info.GenerateLocalizedName ? (stiComponent.LocalizedName + ((StiDataBand)stiComponent).DataSourceName) : ("Data" + ((StiDataBand)stiComponent).DataSourceName), addOne: false, removeIncorrectSymbols: false, forceAdvancedNamingRule: true);
			}
		}
		if (stiComponent == null)
		{
			return;
		}
		if (!Designer.CreateLabel)
		{
			textLabel = null;
		}
		else
		{
			StiDictionaryPanelService service = StiDictionaryPanelService.GetService(Designer);
			if (!service.ShowCreateLabel)
			{
				textLabel = null;
			}
		}
		StiDragDropFromDictionaryEventArgs stiDragDropFromDictionaryEventArgs = new StiDragDropFromDictionaryEventArgs(text, textLabel, cancel: false);
		StiOptions.Engine.GlobalEvents.InvokeDragDropFromDictionary(this, stiDragDropFromDictionaryEventArgs);
		if (stiDragDropFromDictionaryEventArgs.Cancel)
		{
			return;
		}
		if (SimpleDragDrop)
		{
			if (stiFormatService != null)
			{
				text = "Format(\"" + stiFormatService.NativeFormatString + "\", IDENT)";
			}
			((Control)this).DoDragDrop((object)text, (DragDropEffects)2);
		}
		else
		{
			StiDesignerDragDrop.StartDragDrop(Designer, stiComponent, (Control)(object)this, stiDragDropFromDictionaryEventArgs.Text, stiDragDropFromDictionaryEventArgs.TextLabel);
		}
	}

	private void TreeViewData_DragDrop(object sender, DragEventArgs e)
	{
		Designer.SelectTool.Action = StiAction.None;
		Designer.Report.Info.IsComponentsMoving = false;
	}

	private void TreeViewData_AfterSelect(object sender, TreeViewEventArgs e)
	{
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Expected O, but got Unknown
		if (e.Node != null)
		{
			if (e.Node.Tag is StiFunction stiFunction)
			{
				string longFunctionString = stiFunction.GetLongFunctionString(Designer.Report.ScriptLanguage);
				string description = stiFunction.Description;
				string[] argumentNames = stiFunction.ArgumentNames;
				string[] argumentDescriptions = stiFunction.ArgumentDescriptions;
				string returnDescription = stiFunction.ReturnDescription;
				string functionName = stiFunction.FunctionName;
				SetInfoForFunction(functionName, longFunctionString, description, argumentNames, argumentDescriptions, stiFunction.ArgumentTypes, returnDescription);
				return;
			}
			if (e.Node.Parent != null && e.Node.Parent.Text == Loc.Get("PropertyMain", "SystemVariables"))
			{
				string text = e.Node.Text;
				string systemVariableInfo = StiSystemVariablesHelper.GetSystemVariableInfo(text);
				if (systemVariableInfo != null)
				{
					EnableInformation();
					((Control)information).Text = e.Node.Text + "\n";
					int length = ((Control)information).Text.Length;
					RichTextBox obj = information;
					((Control)obj).Text = ((Control)obj).Text + systemVariableInfo;
					((TextBoxBase)information).Select(0, length);
					information.SelectionFont = new Font("Arial", 8f, (FontStyle)1);
				}
				return;
			}
		}
		((Control)panelInformation).Visible = false;
		((Control)splitter).Visible = false;
	}

	private void Splitter_SplitterMoved(object sender, SplitterEventArgs e)
	{
		if (((Control)tvData).Visible)
		{
			StiSettings.Load();
			StiSettings.Set(((Control)this).Name, "VerticalSplitter", ((Control)information).Height);
			StiSettings.Save();
		}
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
		Designer.Report.Info.IsComponentsMoving = false;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			((TreeView)tvData).ImageList = null;
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Expected O, but got Unknown
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Expected O, but got Unknown
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Expected O, but got Unknown
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Expected O, but got Unknown
		//IL_03f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fe: Expected O, but got Unknown
		tvData = new StiTreeView();
		information = new RichTextBox();
		splitter = new ExpandableSplitter();
		panelInformation = new Panel();
		((Control)panelInformation).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)tvData).Dock = (DockStyle)5;
		((TreeView)tvData).HideSelection = false;
		((Control)tvData).Location = new Point(0, 0);
		((Control)tvData).Name = "tvData";
		((Control)tvData).Size = new Size(320, 268);
		((Control)tvData).TabIndex = 0;
		((TreeView)tvData).BeforeCollapse += new TreeViewCancelEventHandler(TreeNode_Collapsed);
		((TreeView)tvData).BeforeExpand += new TreeViewCancelEventHandler(TreeNode_Expand);
		((TreeView)tvData).ItemDrag += new ItemDragEventHandler(TreeViewData_ItemDrag);
		((TreeView)tvData).AfterSelect += new TreeViewEventHandler(TreeViewData_AfterSelect);
		((Control)tvData).DragDrop += new DragEventHandler(TreeViewData_DragDrop);
		((Control)information).Anchor = (AnchorStyles)15;
		((Control)information).BackColor = Color.White;
		((Control)information).ForeColor = SystemColors.InfoText;
		((Control)information).Location = new Point(12, 12);
		((Control)information).Name = "information";
		((TextBoxBase)information).ReadOnly = true;
		((Control)information).Size = new Size(308, 72);
		((Control)information).TabIndex = 1;
		((Control)information).Text = "";
		splitter.BackColor2 = Color.White;
		splitter.BackColor2SchemePart = eColorSchemePart.None;
		splitter.BackColorSchemePart = eColorSchemePart.PanelBackground;
		((Control)splitter).Dock = (DockStyle)2;
		splitter.Expandable = false;
		splitter.ExpandActionClick = false;
		splitter.ExpandFillColor = Color.FromArgb(170, 170, 170);
		splitter.ExpandFillColorSchemePart = eColorSchemePart.PanelBorder;
		splitter.ExpandLineColor = Color.FromArgb(25, 25, 25);
		splitter.ExpandLineColorSchemePart = eColorSchemePart.ItemText;
		((Control)splitter).ForeColor = Color.Black;
		splitter.GripDarkColor = Color.FromArgb(25, 25, 25);
		splitter.GripDarkColorSchemePart = eColorSchemePart.ItemText;
		splitter.GripLightColor = Color.FromArgb(255, 255, 255);
		splitter.GripLightColorSchemePart = eColorSchemePart.BarBackground;
		splitter.HotBackColor = Color.White;
		splitter.HotBackColor2 = Color.Empty;
		splitter.HotBackColor2SchemePart = eColorSchemePart.ItemPressedBackground2;
		splitter.HotBackColorSchemePart = eColorSchemePart.None;
		splitter.HotExpandFillColor = Color.FromArgb(170, 170, 170);
		splitter.HotExpandFillColorSchemePart = eColorSchemePart.PanelBorder;
		splitter.HotExpandLineColor = Color.FromArgb(25, 25, 25);
		splitter.HotExpandLineColorSchemePart = eColorSchemePart.ItemText;
		splitter.HotGripDarkColor = Color.FromArgb(170, 170, 170);
		splitter.HotGripDarkColorSchemePart = eColorSchemePart.PanelBorder;
		splitter.HotGripLightColor = Color.FromArgb(255, 255, 255);
		splitter.HotGripLightColorSchemePart = eColorSchemePart.BarBackground;
		((Control)splitter).Location = new Point(0, 268);
		((Control)splitter).Name = "splitter";
		((Control)splitter).Size = new Size(320, 4);
		splitter.Style = eSplitterStyle.Office2007;
		((Control)splitter).TabIndex = 2;
		((Splitter)splitter).TabStop = false;
		((Control)splitter).Visible = false;
		((Splitter)splitter).SplitterMoved += new SplitterEventHandler(Splitter_SplitterMoved);
		((Control)panelInformation).Controls.Add((Control)(object)information);
		((Control)panelInformation).Dock = (DockStyle)2;
		((Control)panelInformation).Location = new Point(0, 272);
		((Control)panelInformation).Name = "panelInformation";
		((Control)panelInformation).Size = new Size(320, 96);
		((Control)panelInformation).TabIndex = 3;
		((Control)panelInformation).Visible = false;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)tvData);
		((Control)this).Controls.Add((Control)(object)splitter);
		((Control)this).Controls.Add((Control)(object)panelInformation);
		((Control)this).Name = "StiDictionaryControl";
		((Control)this).Size = new Size(320, 368);
		((Control)panelInformation).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
