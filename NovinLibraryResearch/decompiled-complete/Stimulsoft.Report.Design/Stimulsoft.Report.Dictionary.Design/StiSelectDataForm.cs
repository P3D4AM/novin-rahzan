using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Controls.Win.DotNetBar.Controls;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Help;

namespace Stimulsoft.Report.Dictionary.Design;

public class StiSelectDataForm : Form
{
	private class StiSortDataTableComparer : IComparer<DataTable>
	{
		public int Compare(DataTable x, DataTable y)
		{
			if (x.TableName == null)
			{
				if (y.TableName != null)
				{
					return -1;
				}
				return 0;
			}
			if (y.TableName != null)
			{
				return string.Compare(x.TableName, y.TableName, StringComparison.Ordinal);
			}
			return 1;
		}
	}

	private List<string> nodeCheckedList = new List<string>();

	private StiDatabaseInformation information;

	private bool isFormClosing;

	private bool lockObject;

	private bool allowExceptionRes;

	private StiReport report;

	private TreeNode dataSourcesNode;

	private List<StiDataSource> dataSources = new List<StiDataSource>();

	private StiDatabase selectedDatabase;

	private IContainer components;

	private ImageList imageList1;

	private StiTreeViewClicked treeViewObjects;

	private Button buttonOk;

	private Button buttonCancel;

	private Bar toolbarMain;

	private ButtonItem buttonNewQuery;

	private ButtonItem buttonEditObject;

	private StiGroupLine groupLineBottom;

	private StiGroupLine groupLineHeader;

	private StiGroupLine groupLineSeparator;

	private Panel panelObjects;

	private CircularProgress progressOperation;

	private BackgroundWorker workerView;

	private Label labelErrorMessage;

	private Panel panelFind;

	private StiUserData stiUserData1;

	private StiButtonEdit textBoxSearch;

	private bool IsQueryAllowed
	{
		get
		{
			if (!(selectedDatabase is StiNoSqlDatabase) && !(selectedDatabase is StiODataDatabase))
			{
				return !(selectedDatabase is StiFileDatabase);
			}
			return false;
		}
	}

	public StiDatabaseInformation InformationSelected { get; private set; }

	public StiSelectDataForm(StiReport report, StiDatabase selectedDatabase)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			if (StiOptions.Appearance.ShowFormsHelp)
			{
				((Form)this).HelpButton = true;
			}
			DoScaling();
			this.report = report;
			this.selectedDatabase = selectedDatabase;
			buttonNewQuery.Visible = IsQueryAllowed;
			buttonEditObject.Visible = IsQueryAllowed;
			((Control)groupLineHeader).Visible = IsQueryAllowed;
			((Control)groupLineSeparator).Visible = IsQueryAllowed;
			CheckState();
			StiSettings.Load();
			StiFormSettings.Load((Form)(object)this);
			((Form)this).ShowIcon = false;
			Localize();
		}
	}

	private void DoScaling()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		double step = StiScale.Step;
		if (step >= 1.5)
		{
			string stepName = StiScale.StepName;
			imageList1 = new ImageList(components);
			imageList1.ImageSize = new Size((int)Math.Ceiling(16.0 * step), (int)Math.Ceiling(16.0 * step));
			imageList1.TransparentColor = Color.Transparent;
			imageList1.Images.Add("QueryNew2013.png", StiReportDesignImages.Actions.Queries());
			imageList1.Images.Add("Query2013.png", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Query" + stepName + ".png"));
			imageList1.Images.Add("Views2013.png", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Views" + stepName + ".png"));
			imageList1.Images.Add("DataTable2013.png", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.DataTable" + stepName + ".png"));
			imageList1.Images.Add("DataTables2013.png", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.DataTables" + stepName + ".png"));
			imageList1.Images.Add("Edit2013.png", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Edit" + stepName + ".png"));
			imageList1.Images.Add("Query2013.png", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Queries" + stepName + ".png"));
			imageList1.Images.Add("View2013.png", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.DataView" + stepName + ".png"));
			imageList1.Images.Add("Parameter2013.png", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.Parameter" + stepName + ".png"));
			imageList1.Images.Add("StoredProcedure2013.png", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.StoredProcedure" + stepName + ".png"));
			imageList1.Images.Add("StoredProcedures2013.png", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.StoredProcedures" + stepName + ".png"));
			imageList1.Images.Add("DataColumnBinary2013.png", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.DataColumnBinary" + stepName + ".png"));
			imageList1.Images.Add("DataColumnBool2013.png", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.DataColumnBool" + stepName + ".png"));
			imageList1.Images.Add("DataColumnChar2013.png", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.DataColumnChar" + stepName + ".png"));
			imageList1.Images.Add("DataColumnDateTime2013.png", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.DataColumnDateTime" + stepName + ".png"));
			imageList1.Images.Add("DataColumnDecimal2013.png", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.DataColumnDecimal" + stepName + ".png"));
			imageList1.Images.Add("DataColumnFloat2013.png", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.DataColumnFloat" + stepName + ".png"));
			imageList1.Images.Add("DataColumnImage2013.png", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.DataColumnImage" + stepName + ".png"));
			imageList1.Images.Add("DataColumnInt2013.png", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.DataColumnInt" + stepName + ".png"));
			imageList1.Images.Add("DataColumnString2013.png", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.DataColumnString" + stepName + ".png"));
			imageList1.Images.Add("Synchronize2013.png", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report.Design", "Stimulsoft.Report.Design.Resources.Synchronize" + stepName + ".png"));
			imageList1.Images.Add("DataRelation2013.png", (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Dictionary.Office2013.DataRelation" + stepName + ".png"));
			((TreeView)treeViewObjects).ImageList = imageList1;
			((TreeView)treeViewObjects).ImageIndex = 0;
		}
		else
		{
			((TreeView)treeViewObjects).ImageIndex = 21;
		}
		buttonNewQuery.Image = StiReportDesignImages.Actions.NewQuery();
		buttonEditObject.Image = StiReportDesignImages.Actions.EditData();
		textBoxSearch.Image = StiReportDesignImages.Actions.SearchSmall();
	}

	public void Localize()
	{
		((Control)this).Text = StiLocalization.Get("FormDatabaseEdit", "SelectData");
		textBoxSearch.Hint = StiLocalization.Get("FormViewer", "Find");
		((Control)buttonOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		buttonNewQuery.Text = StiLocalization.Get("FormDictionaryDesigner", "QueryNew");
		buttonEditObject.Text = StiLocalization.Get("FormDictionaryDesigner", "EditQuery");
	}

	private void BuildDataSources()
	{
		dataSourcesNode.Nodes.Clear();
		FillTreeNode(dataSourcesNode, dataSources);
		dataSourcesNode.Expand();
	}

	private void BuildObjects()
	{
		((TreeView)treeViewObjects).Nodes.Clear();
		if (selectedDatabase != null)
		{
			BuildQueries();
			BuildTables();
			BuildViews();
			BuildStoredProcedures();
		}
	}

	private void BuildQueries()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		if (IsQueryAllowed)
		{
			string text = "Queries2013.png";
			dataSourcesNode = new TreeNode(StiLocalization.Get("FormDictionaryDesigner", "Queries"));
			dataSourcesNode.Tag = "Queries";
			TreeNode obj = dataSourcesNode;
			string imageKey = (dataSourcesNode.SelectedImageKey = text);
			obj.ImageKey = imageKey;
			((TreeView)treeViewObjects).Nodes.Add(dataSourcesNode);
			FillTreeNode(dataSourcesNode, dataSources);
			dataSourcesNode.Expand();
		}
	}

	private void BuildTables()
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		if (information != null)
		{
			string text = "DataTables2013.png";
			string text2 = ((selectedDatabase is StiMongoDbDatabase) ? StiLocalization.Get("QueryBuilder", "Collections") : StiLocalization.Get("QueryBuilder", "Tables"));
			TreeNode val = new TreeNode(text2);
			val.Tag = "Tables";
			string imageKey = (val.SelectedImageKey = text);
			val.ImageKey = imageKey;
			((TreeView)treeViewObjects).Nodes.Add(val);
			information.Tables.Sort(new StiSortDataTableComparer());
			text = "DataTable2013.png";
			FillTreeNode(val, information.Tables, information.Relations, text);
			val.Expand();
		}
	}

	private void BuildViews()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		if (IsQueryAllowed && information != null)
		{
			string text = "Views2013.png";
			TreeNode val = new TreeNode(StiLocalization.Get("QueryBuilder", "Views"));
			val.Tag = "Views";
			string imageKey = (val.SelectedImageKey = text);
			val.ImageKey = imageKey;
			((TreeView)treeViewObjects).Nodes.Add(val);
			information.Views.Sort(new StiSortDataTableComparer());
			text = "View2013.png";
			FillTreeNode(val, information.Views, null, text);
			val.Expand();
		}
	}

	private void BuildStoredProcedures()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		if (IsQueryAllowed && information != null)
		{
			string text = "StoredProcedures2013.png";
			TreeNode val = new TreeNode(StiLocalization.Get("QueryBuilder", "Procedures"));
			val.Tag = "StoredProcedures";
			string imageKey = (val.SelectedImageKey = text);
			val.ImageKey = imageKey;
			((TreeView)treeViewObjects).Nodes.Add(val);
			information.StoredProcedures.Sort(new StiSortDataTableComparer());
			text = "StoredProcedure2013.png";
			FillTreeNode(val, information.StoredProcedures, null, text, isParameter: true);
			val.Expand();
		}
	}

	private void CheckState()
	{
		buttonEditObject.Visible = ((TreeView)treeViewObjects).SelectedNode != null && ((TreeView)treeViewObjects).SelectedNode.Tag is StiDataSource;
	}

	private string GetImageKeyFromType(Type type)
	{
		if (type == typeof(bool) || type == typeof(bool?))
		{
			return "DataColumnBool2013.png";
		}
		if (type == typeof(char) || type == typeof(char?))
		{
			return "DataColumnChar2013.png";
		}
		if (type == typeof(DateTime) || type == typeof(TimeSpan) || type == typeof(DateTime?) || type == typeof(TimeSpan?))
		{
			return "DataColumnDateTime2013.png";
		}
		if (type == typeof(decimal) || type == typeof(decimal?))
		{
			return "DataColumnDecimal2013.png";
		}
		if (type == typeof(int) || type == typeof(uint) || type == typeof(long) || type == typeof(ulong) || type == typeof(byte) || type == typeof(sbyte) || type == typeof(short) || type == typeof(ushort) || type == typeof(int?) || type == typeof(uint?) || type == typeof(long?) || type == typeof(ulong?) || type == typeof(byte?) || type == typeof(sbyte?) || type == typeof(short?) || type == typeof(ushort?))
		{
			return "DataColumnInt2013.png";
		}
		if (type == typeof(float) || type == typeof(double) || type == typeof(float?) || type == typeof(double?))
		{
			return "DataColumnFloat2013.png";
		}
		if (type == typeof(Image))
		{
			return "DataColumnImage2013.png";
		}
		if (type.IsArray)
		{
			return "DataColumnBinary2013.png";
		}
		return "DataColumnString2013.png";
	}

	private void FillTreeNode(TreeNode parentNode, List<DataTable> tables, List<DataRelation> relations, string imageKey, bool isParameter = false)
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Expected O, but got Unknown
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Expected O, but got Unknown
		string value = ((Control)textBoxSearch).Text.ToLower();
		lockObject = true;
		foreach (DataTable table in tables)
		{
			if (!string.IsNullOrEmpty(value) && !table.TableName.ToLower().Contains(value))
			{
				continue;
			}
			TreeNode val = new TreeNode(table.TableName);
			string imageKey2 = (val.SelectedImageKey = imageKey);
			val.ImageKey = imageKey2;
			val.Tag = table.ExtendedProperties["Query"];
			parentNode.Nodes.Add(val);
			bool @checked = (val.Checked = ContainsInCheckedList(val));
			if (relations != null)
			{
				foreach (DataRelation item in relations.Where((DataRelation r) => r.ChildTable == table))
				{
					TreeNode val2 = new TreeNode(item.RelationName)
					{
						Tag = item,
						Checked = @checked,
						ImageKey = "DataRelation2013.png",
						SelectedImageKey = "DataRelation2013.png"
					};
					val.Nodes.Add(val2);
				}
			}
			foreach (DataColumn column in table.Columns)
			{
				TreeNode val3 = new TreeNode(column.ColumnName)
				{
					Tag = column,
					Checked = @checked
				};
				if (isParameter && column.Caption != "Columns")
				{
					imageKey2 = (val3.SelectedImageKey = "Parameter2013.png");
					val3.ImageKey = imageKey2;
				}
				else
				{
					imageKey2 = (val3.SelectedImageKey = GetImageKeyFromType(column.DataType));
					val3.ImageKey = imageKey2;
				}
				val.Nodes.Add(val3);
			}
		}
		lockObject = false;
	}

	private void FillTreeNode(TreeNode parentNode, List<StiDataSource> dataSources)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Expected O, but got Unknown
		foreach (StiDataSource dataSource in dataSources)
		{
			TreeNode val = new TreeNode(dataSource.Name);
			val.Tag = dataSource;
			if (dataSource is StiSqlSource)
			{
				string imageKey = (val.SelectedImageKey = "Query2013.png");
				val.ImageKey = imageKey;
			}
			else
			{
				string imageKey = (val.SelectedImageKey = "DataSource2013.png");
				val.ImageKey = imageKey;
			}
			parentNode.Nodes.Add(val);
			foreach (StiDataColumn column in dataSource.Columns)
			{
				TreeNode val2 = new TreeNode(column.Name);
				val2.Tag = column;
				string imageKey = (val2.SelectedImageKey = GetImageKeyFromType(column.Type));
				val2.ImageKey = imageKey;
				val.Nodes.Add(val2);
			}
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

	public TreeNode GetNodeByTag(object tag)
	{
		return GetNodeByTag(((TreeView)treeViewObjects).Nodes, tag);
	}

	public void SelectNodeByTag(object tag)
	{
		((TreeView)treeViewObjects).SelectedNode = GetNodeByTag(tag);
		if (((TreeView)treeViewObjects).SelectedNode != null)
		{
			((TreeView)treeViewObjects).SelectedNode.EnsureVisible();
		}
	}

	private void RefreshObjects()
	{
		This_SizeChanged(null, null);
		progressOperation.Value = 0;
		progressOperation.IsRunning = true;
		((Control)progressOperation).Visible = true;
		toolbarMain.Visible = false;
		((Control)groupLineBottom).Visible = false;
		((Control)groupLineSeparator).Visible = false;
		((Control)panelFind).Visible = false;
		if (StiOptions.Designer.AllowAsyncDataCommands)
		{
			workerView.RunWorkerAsync();
			return;
		}
		WorkerView_DoWork(null, null);
		WorkerView_RunWorkerCompleted(null, null);
	}

	private void ShowErrorMessage(string message)
	{
		((Control)labelErrorMessage).Invoke((Delegate)(Action)delegate
		{
			((Control)labelErrorMessage).Text = message;
			((Control)labelErrorMessage).Visible = true;
		});
	}

	private void HideErrorMessage()
	{
		((Control)labelErrorMessage).Invoke((Delegate)(Action)delegate
		{
			((Control)labelErrorMessage).Visible = false;
		});
	}

	private bool ContainsInCheckedList(TreeNode node)
	{
		if (node.Parent.Tag != null && (node.Parent.Tag.Equals("Tables") || node.Parent.Tag.Equals("Views")))
		{
			string item = $"{node.Parent.Tag}{node.Text}";
			return nodeCheckedList.Contains(item);
		}
		return false;
	}

	private void TextBoxSearch_ButtonClick(object sender, EventArgs e)
	{
		if (((Control)textBoxSearch).Text.Length > 0)
		{
			((Control)textBoxSearch).Text = string.Empty;
		}
	}

	private void TextBoxSearch_TextChanged(object sender, EventArgs e)
	{
		BuildObjects();
		textBoxSearch.Image = ((((Control)textBoxSearch).Text.Length > 0) ? StiReportDesignImages.Actions.Remove() : StiReportDesignImages.Actions.SearchSmall());
	}

	private void TreeViewObjects_AfterCheck(object sender, TreeViewEventArgs e)
	{
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Expected O, but got Unknown
		if (lockObject)
		{
			return;
		}
		bool @checked = e.Node.Checked;
		if (e.Node.Parent != null && e.Node.Parent.Tag != null && (e.Node.Parent.Tag.Equals("Tables") || e.Node.Parent.Tag.Equals("Views")))
		{
			string item = $"{e.Node.Parent.Tag}{e.Node.Text}";
			if (@checked)
			{
				nodeCheckedList.Add(item);
			}
			else if (nodeCheckedList.Contains(item))
			{
				nodeCheckedList.Remove(item);
			}
		}
		if (@checked && e.Node.Tag is DataColumn)
		{
			lockObject = true;
			e.Node.Parent.Checked = true;
			lockObject = false;
		}
		foreach (TreeNode node in e.Node.Nodes)
		{
			TreeNode val = node;
			val.Checked = @checked;
		}
	}

	private void ButtonNewQuery_Click(object sender, EventArgs e)
	{
		if (!(selectedDatabase is StiSqlDatabase stiSqlDatabase))
		{
			return;
		}
		StiSqlAdapterService dataAdapter = stiSqlDatabase.GetDataAdapter();
		if (dataAdapter != null)
		{
			StiDictionary dictionary = report.Dictionary;
			StiSqlSource stiSqlSource = dataAdapter.Create(dictionary) as StiSqlSource;
			stiSqlSource.NameInSource = stiSqlDatabase.Name;
			if (!dataAdapter.New(dictionary, stiSqlSource))
			{
				dictionary.DataSources.Remove(stiSqlSource);
				return;
			}
			dataSources.Add(stiSqlSource);
			BuildDataSources();
			SelectNodeByTag(stiSqlSource);
		}
	}

	private void ButtonEditObject_Click(object sender, EventArgs e)
	{
		StiDataSource stiDataSource = ((TreeView)treeViewObjects).SelectedNode.Tag as StiDataSource;
		if (selectedDatabase is StiSqlDatabase stiSqlDatabase && stiDataSource != null)
		{
			StiSqlAdapterService dataAdapter = stiSqlDatabase.GetDataAdapter();
			if (dataAdapter != null && dataAdapter.Edit(report.Dictionary, stiDataSource))
			{
				BuildDataSources();
				SelectNodeByTag(stiDataSource);
			}
		}
	}

	private void WorkerView_DoWork(object sender, DoWorkEventArgs e)
	{
		HideErrorMessage();
		try
		{
			allowExceptionRes = selectedDatabase.AllowException;
			selectedDatabase.AllowException = true;
			information = selectedDatabase.GetDatabaseInformation(report);
			selectedDatabase.AllowException = allowExceptionRes;
		}
		catch (Exception ex)
		{
			ShowErrorMessage(ex.Message);
		}
	}

	private void WorkerView_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		if (isFormClosing)
		{
			return;
		}
		try
		{
			BuildObjects();
			progressOperation.IsRunning = false;
			((Control)progressOperation).Visible = false;
			toolbarMain.Visible = IsQueryAllowed;
			((Control)groupLineBottom).Visible = true;
			((Control)groupLineSeparator).Visible = IsQueryAllowed;
			((Control)panelFind).Visible = true;
		}
		catch
		{
		}
	}

	private void This_Load(object sender, EventArgs e)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		((Control)progressOperation).Parent.Controls.Remove((Control)(object)progressOperation);
		((Control)this).Controls.Add((Control)(object)progressOperation);
		((Control)progressOperation).BringToFront();
		RefreshObjects();
		Timer timer = new Timer
		{
			Interval = 200
		};
		timer.Tick += delegate
		{
			timer.Stop();
			((Component)(object)timer).Dispose();
			((Control)textBoxSearch).Focus();
		};
		timer.Start();
	}

	private void This_FormClosed(object sender, FormClosedEventArgs e)
	{
		StiFormSettings.Save((Form)(object)this);
	}

	private void This_FormClosing(object sender, FormClosingEventArgs e)
	{
		isFormClosing = true;
	}

	private void This_SizeChanged(object sender, EventArgs e)
	{
		((Control)progressOperation).Left = (((Form)this).ClientSize.Width - ((Control)progressOperation).Width) / 2;
		((Control)progressOperation).Top = (((Form)this).ClientSize.Height - ((Control)progressOperation).Height) / 2;
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Expected O, but got Unknown
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Expected O, but got Unknown
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Expected O, but got Unknown
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Expected O, but got Unknown
		((Control)textBoxSearch).Text = string.Empty;
		InformationSelected = new StiDatabaseInformation();
		StiDatabaseInformation stiDatabaseInformation = new StiDatabaseInformation();
		foreach (TreeNode node in ((TreeView)treeViewObjects).Nodes)
		{
			TreeNode val = node;
			if ((string)val.Tag == "Tables" || (string)val.Tag == "Views")
			{
				foreach (TreeNode node2 in val.Nodes)
				{
					TreeNode val2 = node2;
					if (!val2.Checked)
					{
						continue;
					}
					DataTable dataTable = new DataTable(val2.Text);
					if (val2.Tag is string)
					{
						dataTable.ExtendedProperties["Query"] = val2.Tag as string;
					}
					DataTable dataTable2 = new DataTable(val2.Text);
					if (val2.Tag is string)
					{
						dataTable2.ExtendedProperties["Query"] = val2.Tag as string;
					}
					if ((string)val.Tag == "Tables")
					{
						InformationSelected.Tables.Add(dataTable);
						stiDatabaseInformation.Tables.Add(dataTable2);
					}
					else if ((string)val.Tag == "Views")
					{
						InformationSelected.Views.Add(dataTable);
					}
					foreach (TreeNode node3 in val2.Nodes)
					{
						TreeNode val3 = node3;
						if (val3.Tag is DataColumn dataColumn && dataColumn != null)
						{
							dataTable2.Columns.Add(new DataColumn(val3.Text, dataColumn.DataType));
							if (val3.Checked)
							{
								DataColumn column = new DataColumn(val3.Text, dataColumn.DataType);
								dataTable.Columns.Add(column);
							}
						}
					}
					foreach (TreeNode node4 in val2.Nodes)
					{
						TreeNode val4 = node4;
						if (val4.Tag is DataRelation dataRelation && dataRelation != null && val4.Checked)
						{
							DataRelation item = new DataRelation(dataRelation.RelationName, dataRelation.ParentColumns, dataRelation.ChildColumns);
							InformationSelected.Relations.Add(item);
						}
					}
				}
			}
			if (!((string)val.Tag == "StoredProcedures"))
			{
				continue;
			}
			foreach (TreeNode node5 in val.Nodes)
			{
				TreeNode val5 = node5;
				if (!val5.Checked)
				{
					continue;
				}
				DataTable dataTable3 = new DataTable(val5.Text);
				if (val5.Tag is string)
				{
					dataTable3.ExtendedProperties["Query"] = val5.Tag as string;
				}
				foreach (TreeNode node6 in val5.Nodes)
				{
					TreeNode val6 = node6;
					if (val6.Checked && val6.Tag is DataColumn)
					{
						DataColumn dataColumn2 = new DataColumn(((DataColumn)val6.Tag).ColumnName, ((DataColumn)val6.Tag).DataType);
						dataColumn2.Caption = ((DataColumn)val6.Tag).Caption;
						dataTable3.Columns.Add(dataColumn2);
					}
				}
				InformationSelected.StoredProcedures.Add(dataTable3);
			}
		}
		selectedDatabase.ApplyDatabaseInformation(InformationSelected, report, stiDatabaseInformation);
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)2;
		foreach (StiDataSource dataSource in dataSources)
		{
			report.Dictionary.DataSources.Remove(dataSource);
		}
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/data_data_dictionary_datasources_creating_data_source.htm");
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_05c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cc: Expected O, but got Unknown
		//IL_081a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0824: Expected O, but got Unknown
		//IL_088c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb2: Expected O, but got Unknown
		//IL_0ce2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f60: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f6a: Expected O, but got Unknown
		//IL_0f72: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f7c: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiSelectDataForm));
		imageList1 = new ImageList(components);
		treeViewObjects = new StiTreeViewClicked();
		buttonOk = new Button();
		buttonCancel = new Button();
		groupLineBottom = new StiGroupLine();
		toolbarMain = new Bar();
		buttonNewQuery = new ButtonItem();
		buttonEditObject = new ButtonItem();
		groupLineHeader = new StiGroupLine();
		groupLineSeparator = new StiGroupLine();
		panelObjects = new Panel();
		progressOperation = new CircularProgress();
		workerView = new BackgroundWorker();
		labelErrorMessage = new Label();
		stiUserData1 = new StiUserData();
		panelFind = new Panel();
		textBoxSearch = new StiButtonEdit();
		((Control)groupLineBottom).SuspendLayout();
		((ISupportInitialize)toolbarMain).BeginInit();
		((Control)groupLineHeader).SuspendLayout();
		((Control)panelObjects).SuspendLayout();
		((Control)panelFind).SuspendLayout();
		((Control)this).SuspendLayout();
		imageList1.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("imageList1.ImageStream");
		imageList1.TransparentColor = Color.Transparent;
		imageList1.Images.SetKeyName(0, "QueryNew.png");
		imageList1.Images.SetKeyName(1, "Query.png");
		imageList1.Images.SetKeyName(2, "Views.png");
		imageList1.Images.SetKeyName(3, "DataTable.png");
		imageList1.Images.SetKeyName(4, "DataTables.png");
		imageList1.Images.SetKeyName(5, "Edit.png");
		imageList1.Images.SetKeyName(6, "Queries.png");
		imageList1.Images.SetKeyName(7, "View.png");
		imageList1.Images.SetKeyName(8, "Parameter.png");
		imageList1.Images.SetKeyName(9, "StoredProcedure.png");
		imageList1.Images.SetKeyName(10, "StoredProcedures.png");
		imageList1.Images.SetKeyName(11, "DataColumnBinary.png");
		imageList1.Images.SetKeyName(12, "DataColumnBool.png");
		imageList1.Images.SetKeyName(13, "DataColumnChar.png");
		imageList1.Images.SetKeyName(14, "DataColumnDateTime.png");
		imageList1.Images.SetKeyName(15, "DataColumnDecimal.png");
		imageList1.Images.SetKeyName(16, "DataColumnFloat.png");
		imageList1.Images.SetKeyName(17, "DataColumnImage.png");
		imageList1.Images.SetKeyName(18, "DataColumnInt.png");
		imageList1.Images.SetKeyName(19, "DataColumnString.png");
		imageList1.Images.SetKeyName(20, "Synchronize.png");
		imageList1.Images.SetKeyName(21, "QueryNew2013.png");
		imageList1.Images.SetKeyName(22, "Query2013.png");
		imageList1.Images.SetKeyName(23, "Views2013.png");
		imageList1.Images.SetKeyName(24, "DataTable2013.png");
		imageList1.Images.SetKeyName(25, "DataTables2013.png");
		imageList1.Images.SetKeyName(26, "Edit2013.png");
		imageList1.Images.SetKeyName(27, "Queries2013.png");
		imageList1.Images.SetKeyName(28, "View2013.png");
		imageList1.Images.SetKeyName(29, "Parameter2013.png");
		imageList1.Images.SetKeyName(30, "StoredProcedure2013.png");
		imageList1.Images.SetKeyName(31, "StoredProcedures2013.png");
		imageList1.Images.SetKeyName(32, "DataColumnBinary2013.png");
		imageList1.Images.SetKeyName(33, "DataColumnBool2013.png");
		imageList1.Images.SetKeyName(34, "DataColumnChar2013.png");
		imageList1.Images.SetKeyName(35, "DataColumnDateTime2013.png");
		imageList1.Images.SetKeyName(36, "DataColumnDecimal2013.png");
		imageList1.Images.SetKeyName(37, "DataColumnFloat2013.png");
		imageList1.Images.SetKeyName(38, "DataColumnImage2013.png");
		imageList1.Images.SetKeyName(39, "DataColumnInt2013.png");
		imageList1.Images.SetKeyName(40, "DataColumnString2013.png");
		imageList1.Images.SetKeyName(41, "Synchronize2013.png");
		((Control)treeViewObjects).Anchor = (AnchorStyles)15;
		((Control)treeViewObjects).BackColor = Color.White;
		((TreeView)treeViewObjects).BorderStyle = (BorderStyle)0;
		((TreeView)treeViewObjects).CheckBoxes = true;
		((TreeView)treeViewObjects).HideSelection = false;
		((TreeView)treeViewObjects).ImageIndex = 0;
		((TreeView)treeViewObjects).ImageList = imageList1;
		((Control)treeViewObjects).Location = new Point(12, 12);
		((Control)treeViewObjects).Name = "treeViewObjects";
		((TreeView)treeViewObjects).SelectedImageIndex = 0;
		((Control)treeViewObjects).Size = new Size(640, 238);
		((Control)treeViewObjects).TabIndex = 4;
		((TreeView)treeViewObjects).AfterCheck += new TreeViewEventHandler(TreeViewObjects_AfterCheck);
		((Control)buttonOk).Anchor = (AnchorStyles)10;
		buttonOk.DialogResult = (DialogResult)1;
		((Control)buttonOk).Location = new Point(498, 12);
		((Control)buttonOk).Name = "btOk";
		((Control)buttonOk).Size = new Size(75, 24);
		((Control)buttonOk).TabIndex = 5;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)buttonOk).Click += This_OkClick;
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		buttonCancel.DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(579, 12);
		((Control)buttonCancel).Name = "btCancel";
		((Control)buttonCancel).Size = new Size(75, 24);
		((Control)buttonCancel).TabIndex = 6;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)buttonCancel).Click += This_CancelClick;
		((Control)groupLineBottom).BackColor = Color.White;
		((Control)groupLineBottom).Controls.Add((Control)(object)buttonOk);
		((Control)groupLineBottom).Controls.Add((Control)(object)buttonCancel);
		((Control)groupLineBottom).Dock = (DockStyle)2;
		groupLineBottom.LinePosition = StiLinePosition.Top;
		groupLineBottom.LineStyle = StiLineStyle.Dot;
		((Control)groupLineBottom).Location = new Point(0, 396);
		((Control)groupLineBottom).Name = "groupLineBottom";
		((Control)groupLineBottom).Size = new Size(666, 46);
		((Control)groupLineBottom).TabIndex = 33;
		((Control)toolbarMain).AccessibleDescription = "toolbarMain (toolbarMain)";
		((Control)toolbarMain).AccessibleName = "toolbarMain";
		((Control)toolbarMain).AccessibleRole = (AccessibleRole)22;
		((Control)toolbarMain).BackColor = Color.White;
		toolbarMain.BarType = eBarType.MenuBar;
		((Control)toolbarMain).Dock = (DockStyle)1;
		toolbarMain.DockSide = eDockSide.Top;
		toolbarMain.DockTabStripHeight = 50;
		((Control)toolbarMain).Font = new Font("Segoe UI", 9f);
		toolbarMain.Items.AddRange(new BaseItem[2] { buttonNewQuery, buttonEditObject });
		toolbarMain.Location = new Point(0, 0);
		((Control)toolbarMain).MinimumSize = new Size(0, 30);
		toolbarMain.Name = "toolbarMain";
		toolbarMain.Padding = new Padding(12, 3, 3, 3);
		toolbarMain.PaddingLeft = 5;
		toolbarMain.RoundCorners = false;
		toolbarMain.Size = new Size(666, 31);
		toolbarMain.Stretch = true;
		toolbarMain.Style = eDotNetBarStyle.StyleManagerControlled;
		toolbarMain.TabIndex = 30;
		toolbarMain.TabStop = false;
		((Control)toolbarMain).Text = "toolbarMain";
		buttonNewQuery.BeginGroup = true;
		buttonNewQuery.ButtonStyle = eButtonStyle.ImageAndText;
		buttonNewQuery.ImageIndex = 0;
		buttonNewQuery.ImagePaddingHorizontal = 16;
		buttonNewQuery.ImagePaddingVertical = 12;
		buttonNewQuery.Name = "tbNewQuery";
		buttonNewQuery.Text = "New Query";
		buttonNewQuery.Click += ButtonNewQuery_Click;
		buttonEditObject.ButtonStyle = eButtonStyle.ImageAndText;
		buttonEditObject.ImageIndex = 5;
		buttonEditObject.ImagePaddingHorizontal = 16;
		buttonEditObject.ImagePaddingVertical = 12;
		buttonEditObject.Name = "tbEditObject";
		buttonEditObject.Text = "Edit";
		buttonEditObject.Click += ButtonEditObject_Click;
		((Control)groupLineHeader).BackColor = Color.White;
		((Control)groupLineHeader).Controls.Add((Control)(object)toolbarMain);
		((Control)groupLineHeader).Dock = (DockStyle)1;
		groupLineHeader.LinePosition = StiLinePosition.None;
		groupLineHeader.LineStyle = StiLineStyle.Dot;
		((Control)groupLineHeader).Location = new Point(0, 0);
		((Control)groupLineHeader).Name = "headerPanel";
		((Control)groupLineHeader).Size = new Size(666, 26);
		((Control)groupLineHeader).TabIndex = 34;
		((Control)groupLineSeparator).BackColor = Color.White;
		((Control)groupLineSeparator).Dock = (DockStyle)1;
		groupLineSeparator.LinePosition = StiLinePosition.Bottom;
		groupLineSeparator.LineStyle = StiLineStyle.Dot;
		((Control)groupLineSeparator).Location = new Point(0, 26);
		((Control)groupLineSeparator).Name = "groupLineSeparator";
		((Control)groupLineSeparator).Size = new Size(666, 4);
		((Control)groupLineSeparator).TabIndex = 35;
		((Control)panelObjects).Controls.Add((Control)(object)progressOperation);
		((Control)panelObjects).Controls.Add((Control)(object)treeViewObjects);
		((Control)panelObjects).Dock = (DockStyle)5;
		((Control)panelObjects).Location = new Point(0, 68);
		((Control)panelObjects).Margin = new Padding(2);
		((Control)panelObjects).Name = "panelObjects";
		((Control)panelObjects).Size = new Size(666, 267);
		((Control)panelObjects).TabIndex = 36;
		((Control)progressOperation).BackColor = Color.White;
		progressOperation.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)progressOperation).Location = new Point(307, 156);
		((Control)progressOperation).Name = "progressOperation";
		progressOperation.ProgressBarType = eCircularProgressType.Donut;
		progressOperation.ProgressColor = Color.SteelBlue;
		((Control)progressOperation).Size = new Size(52, 54);
		progressOperation.Style = eDotNetBarStyle.OfficeXP;
		((Control)progressOperation).TabIndex = 8;
		((Control)progressOperation).TabStop = false;
		((Control)progressOperation).Visible = false;
		workerView.WorkerSupportsCancellation = true;
		workerView.DoWork += WorkerView_DoWork;
		workerView.RunWorkerCompleted += WorkerView_RunWorkerCompleted;
		((Control)labelErrorMessage).BackColor = Color.FromArgb(252, 220, 220);
		((Control)labelErrorMessage).Dock = (DockStyle)2;
		((Control)labelErrorMessage).Font = new Font("Microsoft Sans Serif", 7.875f, (FontStyle)0, (GraphicsUnit)3, (byte)204);
		((Control)labelErrorMessage).ForeColor = Color.Red;
		((Control)labelErrorMessage).Location = new Point(0, 335);
		((Control)labelErrorMessage).Margin = new Padding(2, 0, 2, 0);
		((Control)labelErrorMessage).Name = "labelErrorMessage";
		((Control)labelErrorMessage).Padding = new Padding(12, 2, 12, 2);
		((Control)labelErrorMessage).Size = new Size(666, 61);
		((Control)labelErrorMessage).TabIndex = 37;
		((Control)labelErrorMessage).Text = "Error message";
		labelErrorMessage.TextAlign = (ContentAlignment)16;
		((Control)labelErrorMessage).Visible = false;
		((Control)panelFind).Controls.Add((Control)(object)textBoxSearch);
		((Control)panelFind).Dock = (DockStyle)1;
		((Control)panelFind).Location = new Point(0, 30);
		((Control)panelFind).Name = "panel1";
		((Control)panelFind).Size = new Size(666, 38);
		((Control)panelFind).TabIndex = 9;
		((Control)textBoxSearch).Anchor = (AnchorStyles)13;
		((Control)textBoxSearch).BackColor = Color.White;
		((Control)textBoxSearch).Location = new Point(12, 12);
		((Control)textBoxSearch).Name = "textBoxSearch";
		((Control)textBoxSearch).Size = new Size(640, 21);
		((Control)textBoxSearch).TabIndex = 0;
		textBoxSearch.ButtonClick += TextBoxSearch_ButtonClick;
		((Control)textBoxSearch).TextChanged += TextBoxSearch_TextChanged;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(666, 442);
		((Control)this).Controls.Add((Control)(object)panelObjects);
		((Control)this).Controls.Add((Control)(object)panelFind);
		((Control)this).Controls.Add((Control)(object)groupLineSeparator);
		((Control)this).Controls.Add((Control)(object)groupLineHeader);
		((Control)this).Controls.Add((Control)(object)labelErrorMessage);
		((Control)this).Controls.Add((Control)(object)groupLineBottom);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiSelectDataForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Select Data";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).FormClosing += new FormClosingEventHandler(This_FormClosing);
		((Form)this).FormClosed += new FormClosedEventHandler(This_FormClosed);
		((Form)this).Load += This_Load;
		((Control)this).SizeChanged += This_SizeChanged;
		((Control)groupLineBottom).ResumeLayout(false);
		((ISupportInitialize)toolbarMain).EndInit();
		((Control)groupLineHeader).ResumeLayout(false);
		((Control)panelObjects).ResumeLayout(false);
		((Control)panelFind).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
