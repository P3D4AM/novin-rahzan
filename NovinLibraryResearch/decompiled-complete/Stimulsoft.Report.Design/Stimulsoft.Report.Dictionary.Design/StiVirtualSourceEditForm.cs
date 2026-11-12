using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Wizards;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Dictionary.Design;

public class StiVirtualSourceEditForm : StiFormUX
{
	private string oldValue;

	private string originalName;

	private StiVirtualSource dataSource;

	private StiReport report;

	private StiDataSourcesCollection availableDataSources;

	private IContainer components;

	private StiCloudTabulator TabControl;

	private StiCloudTabulatorPage pageDataSource;

	private StiCloudTabulatorPage pageSort;

	private StiCloudTabulatorPage pageFilters;

	private StiCloudTabulatorPage pageGroups;

	private StiCloudTabulatorPage pageResults;

	private ImageList ilButtons;

	private ErrorProvider errorProvider;

	private StiMenuProvider stiMenuProvider1;

	private StiDataSourcePanel pnAvailableDataSources;

	private StiColumnsGroup columnsGroup;

	private StiColumnsSort columnsSort;

	public StiTextBox tbName;

	private StiLabelUX lbName;

	private StiLabelUX lbAlias;

	public StiTextBox tbAlias;

	private StiColumnsFilters columnsFilters;

	private StiButtonUX btCancel;

	private StiButtonUX btOk;

	private StiColumnsResults columnsResults;

	private StiButtonPanelUX stiGroupLine1;

	public StiVirtualSourceEditForm(StiDictionary dictionary, StiVirtualSource dataSource, string originalName)
	{
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Expected O, but got Unknown
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			((Form)this).ShowIcon = false;
			if (StiOptions.Appearance.ShowFormsHelp)
			{
				((Form)this).HelpButton = true;
			}
			report = dictionary.Report;
			this.dataSource = dataSource;
			this.originalName = originalName;
			availableDataSources = report.DataSources;
			((Control)tbName).Text = dataSource.Name;
			((Control)tbAlias).Text = dataSource.Alias;
			Localize();
			StiSettings.Load();
			StiFormSettings.Load((Form)(object)this);
			((TreeView)pnAvailableDataSources.tvDataSources).AfterSelect += new TreeViewEventHandler(TreeViewDataSources_AfterSelect);
			BuildDataSources();
			BuildFilters();
			BuildSort();
			BuildResults();
			BuildGroups();
			((Control)pnAvailableDataSources.tvDataSources).DoubleClick += TreeViewDataSources_DoubleClick;
			TabControl.SelectedIndex = 0;
			((Control)tbName).Focus();
			((TreeView)pnAvailableDataSources.tvDataSources).SelectedNode = ((IEnumerable)((TreeView)pnAvailableDataSources.tvDataSources).Nodes).Cast<TreeNode>().FirstOrDefault();
			TreeNode selectedNode = ((TreeView)pnAvailableDataSources.tvDataSources).SelectedNode;
			if (selectedNode != null)
			{
				selectedNode.EnsureVisible();
			}
			DoScaling();
		}
	}

	private void Localize()
	{
		((Control)lbName).Text = Loc.GetCleaned("Report", "LabelName");
		((Control)lbAlias).Text = Loc.GetCleaned("Report", "LabelAlias");
		((Control)pageDataSource).Text = StiLocalization.Get("PropertyMain", "DataSource");
		((Control)pageSort).Text = StiLocalization.Get("PropertyMain", "Sort");
		((Control)pageFilters).Text = StiLocalization.Get("PropertyMain", "Filters");
		((Control)pageGroups).Text = StiLocalization.Get("Wizards", "Groups");
		((Control)pageResults).Text = StiLocalization.Get("Wizards", "Results");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
	}

	private void This_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			e.Handled = true;
			((Form)this).Close();
		}
	}

	private void This_Closed(object sender, EventArgs e)
	{
		StiFormSettings.Save((Form)(object)this);
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/index.html?data_data_sources_data_from_other_data_source.htm");
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		bool flag = true;
		if (((Control)tbAlias).Text.Trim().Length == 0)
		{
			errorProvider.SetError((Control)(object)tbAlias, StiFieldChecker.AliasRequire);
			flag = false;
			((Control)tbAlias).Focus();
		}
		else
		{
			errorProvider.SetError((Control)(object)tbAlias, string.Empty);
		}
		if (((Control)tbName).Text.Trim().Length == 0)
		{
			errorProvider.SetError((Control)(object)tbName, StiFieldChecker.NameRequire);
			flag = false;
			((Control)tbName).Focus();
		}
		else
		{
			if (((Control)tbName).Text != originalName && StiNameCreation.Exists(dataSource, report, ((Control)tbName).Text))
			{
				TabControl.SelectedTab = pageDataSource;
				errorProvider.SetError((Control)(object)tbName, StiFieldChecker.NameExists(((Control)tbName).Text));
				((Control)tbName).Focus();
				return;
			}
			errorProvider.SetError((Control)(object)tbName, "");
		}
		if (flag)
		{
			SaveParam(ref dataSource);
			((Form)this).DialogResult = (DialogResult)1;
		}
	}

	private void This_Load(object sender, EventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		Timer timer = new Timer
		{
			Interval = 250
		};
		timer.Tick += delegate
		{
			timer.Stop();
			((Control)tbName).Focus();
			((Component)(object)timer).Dispose();
		};
		timer.Start();
	}

	private void DoScaling()
	{
		pageDataSource.Image = StiImageResizer.GetImage(pageDataSource.Image);
		pageFilters.Image = StiImageResizer.GetImage(pageFilters.Image);
		pageSort.Image = StiImageResizer.GetImage(pageSort.Image);
		pageGroups.Image = StiImageResizer.GetImage(pageGroups.Image);
		pageResults.Image = StiImageResizer.GetImage(pageResults.Image);
		DockPaddingEdges dockPadding = TabControl.DockPadding;
		int right = (TabControl.TitleWidth = StiScale.XXI(110.0));
		dockPadding.Right = right;
		errorProvider.Icon = StiIconUtils.ResizeIcon(errorProvider.Icon, StiScale.I16, StiScale.I16);
	}

	private void BuildDataSources()
	{
		pnAvailableDataSources.Build(availableDataSources, report, dataSource, dataSource.NameInSource);
	}

	private void BuildFilters()
	{
		columnsFilters.BuildFilters(dataSource.Filters);
		((RadioButton)columnsFilters.rbAnd).Checked = dataSource.FilterMode == StiFilterMode.And;
		((RadioButton)columnsFilters.rbOr).Checked = dataSource.FilterMode == StiFilterMode.Or;
		((Control)columnsFilters.cbFilterOn).Visible = false;
	}

	private void BuildSort()
	{
		columnsSort.Sort = dataSource.Sort;
	}

	private void BuildGroups()
	{
		columnsGroup.SetResults(dataSource.GroupColumns);
	}

	private void BuildResults()
	{
		columnsResults.SetResults(dataSource.Results);
	}

	private void SaveParam(ref StiVirtualSource dataSourceWithParam)
	{
		StiDataColumnsCollection stiDataColumnsCollection = new StiDataColumnsCollection(dataSourceWithParam);
		foreach (StiDataColumn column2 in dataSourceWithParam.Columns)
		{
			if (column2 is StiCalcDataColumn)
			{
				stiDataColumnsCollection.Add(column2);
			}
		}
		Hashtable hashtable = new Hashtable();
		foreach (StiDataColumn column3 in dataSourceWithParam.Columns)
		{
			hashtable[column3.Name] = column3.Type;
		}
		dataSourceWithParam.Columns.Clear();
		if (dataSourceWithParam == null)
		{
			dataSourceWithParam = StiActivator.CreateObject(dataSource.GetType()) as StiVirtualSource;
		}
		dataSourceWithParam.Name = ((Control)tbName).Text;
		dataSourceWithParam.Alias = ((Control)tbAlias).Text;
		dataSourceWithParam.NameInSource = ((pnAvailableDataSources.SelectedDataSource == null) ? string.Empty : pnAvailableDataSources.SelectedDataSource.Name);
		dataSourceWithParam.FilterMode = ((!((RadioButton)columnsFilters.rbAnd).Checked) ? StiFilterMode.Or : StiFilterMode.And);
		columnsFilters.FillFilters(dataSourceWithParam.Filters);
		dataSourceWithParam.Sort = columnsSort.Sort;
		dataSourceWithParam.GroupColumns = columnsGroup.GetResults();
		Hashtable hashtable2 = new Hashtable();
		string[] groupColumns = dataSourceWithParam.GroupColumns;
		foreach (string text in groupColumns)
		{
			string text2 = text;
			if (text2.StartsWithInvariant("DESC"))
			{
				if (columnsGroup.DataSource != null)
				{
					StiDataColumn stiDataColumn3 = columnsGroup.DataSource.Columns[text2];
					StiDataColumn stiDataColumn4 = columnsGroup.DataSource.Columns[text2.Substring(4)];
					text2 = ((stiDataColumn3 != null && stiDataColumn4 == null) ? text : text2.Substring(4));
				}
				else
				{
					text2 = text2.Substring(4);
				}
			}
			else if (text2.StartsWithInvariant("NONE"))
			{
				text2 = text2.Substring(4);
			}
			StiDataColumn stiDataColumn5 = new StiDataColumn(text2);
			stiDataColumn5.Type = hashtable[stiDataColumn5.Name] as Type;
			if (stiDataColumn5.Type == null)
			{
				stiDataColumn5.Type = typeof(object);
				if (columnsGroup.DataSource != null)
				{
					StiDataColumn stiDataColumn6 = columnsGroup.DataSource.Columns[text2];
					if (stiDataColumn6 != null)
					{
						stiDataColumn5.Type = stiDataColumn6.Type;
					}
				}
			}
			dataSourceWithParam.Columns.Add(stiDataColumn5);
			hashtable2[text2] = text2;
		}
		dataSourceWithParam.Results = columnsResults.GetResults(hashtable2);
		int num = 0;
		while (num < dataSourceWithParam.Results.Length)
		{
			string name = dataSourceWithParam.Results[num++];
			string text3 = dataSourceWithParam.Results[num++];
			string name2 = dataSourceWithParam.Results[num++];
			StiDataColumn stiDataColumn7 = new StiDataColumn(name2);
			if (text3 == "First" || text3 == "Last" || text3.Length == 0)
			{
				stiDataColumn7.Type = hashtable[stiDataColumn7.Name] as Type;
				if (stiDataColumn7.Type == null)
				{
					stiDataColumn7.Type = typeof(object);
					if (columnsResults.DataSource != null)
					{
						StiDataColumn stiDataColumn8 = columnsResults.DataSource.Columns[name];
						if (stiDataColumn8 != null)
						{
							stiDataColumn7.Type = stiDataColumn8.Type;
						}
					}
				}
			}
			else
			{
				switch (text3)
				{
				case "Count":
				case "CountDistinct":
					stiDataColumn7.Type = typeof(long);
					break;
				case "MinDate":
				case "MaxDate":
					stiDataColumn7.Type = typeof(DateTime);
					break;
				case "MinTime":
				case "MaxTime":
					stiDataColumn7.Type = typeof(TimeSpan);
					break;
				case "MinStr":
				case "MaxStr":
					stiDataColumn7.Type = typeof(string);
					break;
				default:
					stiDataColumn7.Type = hashtable[stiDataColumn7.Name] as Type;
					if (stiDataColumn7.Type == null)
					{
						stiDataColumn7.Type = typeof(decimal);
					}
					break;
				}
			}
			dataSourceWithParam.Columns.Add(stiDataColumn7);
		}
		if (stiDataColumnsCollection.Count <= 0)
		{
			return;
		}
		foreach (StiDataColumn item in stiDataColumnsCollection)
		{
			dataSourceWithParam.Columns.Add(item);
		}
	}

	private void TextBoxName_Enter(object sender, EventArgs e)
	{
		oldValue = ((Control)tbName).Text;
	}

	private void TextBoxName_Leave(object sender, EventArgs e)
	{
		if (oldValue == ((Control)tbAlias).Text)
		{
			((Control)tbAlias).Text = ((Control)tbName).Text;
		}
	}

	private void TreeViewDataSources_AfterSelect(object sender, TreeViewEventArgs e)
	{
		columnsSort.DataSource = pnAvailableDataSources.SelectedDataSource;
		columnsFilters.DataSource = pnAvailableDataSources.SelectedDataSource;
		columnsResults.DataSource = pnAvailableDataSources.SelectedDataSource;
		columnsGroup.DataSource = pnAvailableDataSources.SelectedDataSource;
	}

	private void TreeViewDataSources_DoubleClick(object sender, EventArgs e)
	{
		TabControl.SelectedIndex++;
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
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Expected O, but got Unknown
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_036f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_0468: Unknown result type (might be due to invalid IL or missing references)
		//IL_0572: Unknown result type (might be due to invalid IL or missing references)
		//IL_057c: Expected O, but got Unknown
		//IL_05a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0642: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0754: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0864: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e1: Expected O, but got Unknown
		//IL_0909: Unknown result type (might be due to invalid IL or missing references)
		//IL_092d: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a45: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4f: Expected O, but got Unknown
		//IL_0a77: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bbd: Expected O, but got Unknown
		//IL_0be5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c89: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d06: Expected O, but got Unknown
		//IL_0d2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d52: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e2c: Expected O, but got Unknown
		//IL_0eae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f32: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiVirtualSourceEditForm));
		ilButtons = new ImageList(components);
		errorProvider = new ErrorProvider(components);
		btOk = new StiButtonUX();
		btCancel = new StiButtonUX();
		stiGroupLine1 = new StiButtonPanelUX();
		TabControl = new StiCloudTabulator();
		pageDataSource = new StiCloudTabulatorPage();
		lbAlias = new StiLabelUX();
		lbName = new StiLabelUX();
		tbName = new StiTextBox();
		tbAlias = new StiTextBox();
		pnAvailableDataSources = new StiDataSourcePanel();
		pageSort = new StiCloudTabulatorPage();
		columnsSort = new StiColumnsSort();
		pageFilters = new StiCloudTabulatorPage();
		columnsFilters = new StiColumnsFilters();
		pageGroups = new StiCloudTabulatorPage();
		columnsGroup = new StiColumnsGroup();
		pageResults = new StiCloudTabulatorPage();
		columnsResults = new StiColumnsResults();
		stiMenuProvider1 = new StiMenuProvider();
		((ISupportInitialize)errorProvider).BeginInit();
		((Control)stiGroupLine1).SuspendLayout();
		((Control)TabControl).SuspendLayout();
		((Control)pageDataSource).SuspendLayout();
		((Control)pageSort).SuspendLayout();
		((Control)pageFilters).SuspendLayout();
		((Control)pageGroups).SuspendLayout();
		((Control)pageResults).SuspendLayout();
		((Control)this).SuspendLayout();
		ilButtons.ColorDepth = (ColorDepth)24;
		ilButtons.ImageSize = new Size(16, 16);
		ilButtons.TransparentColor = Color.Magenta;
		errorProvider.ContainerControl = (ContainerControl)(object)this;
		errorProvider.Icon = (Icon)componentResourceManager.GetObject("errorProvider.Icon");
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(476, 12);
		((Control)btOk).Margin = new Padding(2, 2, 2, 2);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 0;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += This_OkClick;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		((Button)btCancel).DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(555, 12);
		((Control)btCancel).Margin = new Padding(2, 2, 2, 2);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 1;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)stiGroupLine1).BackColor = Color.White;
		((Control)stiGroupLine1).Controls.Add((Control)(object)btCancel);
		((Control)stiGroupLine1).Controls.Add((Control)(object)btOk);
		((Control)stiGroupLine1).Dock = (DockStyle)2;
		stiGroupLine1.LinePosition = StiLinePosition.Top;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 336);
		((Control)stiGroupLine1).Margin = new Padding(2, 2, 2, 2);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(642, 46);
		((Control)stiGroupLine1).TabIndex = 0;
		((Control)TabControl).Anchor = (AnchorStyles)15;
		((Control)TabControl).Controls.Add((Control)(object)pageDataSource);
		((Control)TabControl).Controls.Add((Control)(object)pageSort);
		((Control)TabControl).Controls.Add((Control)(object)pageFilters);
		((Control)TabControl).Controls.Add((Control)(object)pageGroups);
		((Control)TabControl).Controls.Add((Control)(object)pageResults);
		((Control)TabControl).Location = new Point(0, 0);
		((Control)TabControl).Margin = new Padding(2, 2, 2, 2);
		((Control)TabControl).Name = "TabControl";
		((Control)TabControl).Padding = new Padding(0, 0, 119, 0);
		TabControl.ScrollHeight = 0;
		TabControl.ScrollWidth = 100;
		((Control)TabControl).Size = new Size(642, 341);
		((Control)TabControl).TabIndex = 13;
		TabControl.TitleBackColor = Color.White;
		TabControl.TitleInLine = false;
		TabControl.TitlePosition = StiTabTitlePosition.RightHorizontal;
		TabControl.TitleWidth = 110;
		((Control)pageDataSource).Controls.Add((Control)(object)lbAlias);
		((Control)pageDataSource).Controls.Add((Control)(object)lbName);
		((Control)pageDataSource).Controls.Add((Control)(object)tbName);
		((Control)pageDataSource).Controls.Add((Control)(object)tbAlias);
		((Control)pageDataSource).Controls.Add((Control)(object)pnAvailableDataSources);
		pageDataSource.Dock = (DockStyle)5;
		pageDataSource.Image = (Image)componentResourceManager.GetObject("pageDataSource.Image");
		pageDataSource.InvisibleVisible = false;
		((Control)pageDataSource).Location = new Point(0, 0);
		((Control)pageDataSource).Margin = new Padding(2, 2, 2, 2);
		((Control)pageDataSource).Name = "pageDataSource";
		((Control)pageDataSource).Padding = new Padding(4, 4, 4, 4);
		((Control)pageDataSource).Size = new Size(523, 341);
		((Control)pageDataSource).TabIndex = 0;
		((Control)pageDataSource).Text = "Data Source";
		((Control)pageDataSource).Visible = false;
		((Control)lbAlias).ForeColor = Color.DimGray;
		((Control)lbAlias).Location = new Point(12, 45);
		((Control)lbAlias).Margin = new Padding(2, 0, 2, 0);
		((Control)lbAlias).Name = "lbAlias";
		((Control)lbAlias).Size = new Size(128, 21);
		((Control)lbAlias).TabIndex = 14;
		((Control)lbAlias).Text = "Alias";
		((Label)lbAlias).TextAlign = (ContentAlignment)16;
		((Control)lbName).ForeColor = Color.DimGray;
		((Control)lbName).Location = new Point(12, 12);
		((Control)lbName).Margin = new Padding(2, 0, 2, 0);
		((Control)lbName).Name = "lbName";
		((Control)lbName).Size = new Size(128, 21);
		((Control)lbName).TabIndex = 13;
		((Control)lbName).Text = "Name";
		((Label)lbName).TextAlign = (ContentAlignment)16;
		((Control)tbName).Anchor = (AnchorStyles)13;
		((Control)tbName).Location = new Point(144, 12);
		((Control)tbName).Margin = new Padding(2, 2, 2, 2);
		((Control)tbName).Name = "tbName";
		((Control)tbName).Size = new Size(237, 21);
		((Control)tbName).TabIndex = 0;
		((Control)tbName).Enter += TextBoxName_Enter;
		((Control)tbName).Leave += TextBoxName_Leave;
		((Control)tbAlias).Anchor = (AnchorStyles)13;
		((Control)tbAlias).Location = new Point(144, 45);
		((Control)tbAlias).Margin = new Padding(2, 2, 2, 2);
		((Control)tbAlias).Name = "tbAlias";
		((Control)tbAlias).Size = new Size(237, 21);
		((Control)tbAlias).TabIndex = 1;
		((Control)pnAvailableDataSources).Anchor = (AnchorStyles)15;
		((Control)pnAvailableDataSources).BackColor = Color.White;
		((Control)pnAvailableDataSources).Location = new Point(4, 78);
		((Control)pnAvailableDataSources).Margin = new Padding(1, 1, 1, 1);
		((Control)pnAvailableDataSources).Name = "pnAvailableDataSources";
		((Control)pnAvailableDataSources).Size = new Size(515, 257);
		((Control)pnAvailableDataSources).TabIndex = 2;
		((Control)pageSort).Controls.Add((Control)(object)columnsSort);
		pageSort.Dock = (DockStyle)5;
		pageSort.Image = (Image)componentResourceManager.GetObject("pageSort.Image");
		pageSort.InvisibleVisible = false;
		((Control)pageSort).Location = new Point(0, 0);
		((Control)pageSort).Margin = new Padding(2, 2, 2, 2);
		((Control)pageSort).Name = "pageSort";
		((Control)pageSort).Padding = new Padding(4, 4, 4, 4);
		((Control)pageSort).Size = new Size(523, 341);
		((Control)pageSort).TabIndex = 2;
		((Control)pageSort).Text = "Sort";
		((Control)pageSort).Visible = false;
		((Control)columnsSort).BackColor = Color.White;
		columnsSort.BusinessObject = null;
		columnsSort.DataSource = null;
		((Control)columnsSort).Dock = (DockStyle)5;
		((Control)columnsSort).Location = new Point(4, 4);
		((Control)columnsSort).Margin = new Padding(0);
		((Control)columnsSort).Name = "columnsSort";
		((Control)columnsSort).Size = new Size(515, 333);
		columnsSort.Sort = null;
		((Control)columnsSort).TabIndex = 0;
		((Control)pageFilters).Controls.Add((Control)(object)columnsFilters);
		pageFilters.Dock = (DockStyle)5;
		pageFilters.Image = (Image)componentResourceManager.GetObject("pageFilters.Image");
		pageFilters.InvisibleVisible = false;
		((Control)pageFilters).Location = new Point(0, 0);
		((Control)pageFilters).Margin = new Padding(2, 2, 2, 2);
		((Control)pageFilters).Name = "pageFilters";
		((Control)pageFilters).Padding = new Padding(4, 4, 4, 4);
		((Control)pageFilters).Size = new Size(523, 341);
		((Control)pageFilters).TabIndex = 3;
		((Control)pageFilters).Text = "Filters";
		((Control)pageFilters).Visible = false;
		((Control)columnsFilters).BackColor = Color.White;
		columnsFilters.BusinessObject = null;
		columnsFilters.DataSource = null;
		((Control)columnsFilters).Dock = (DockStyle)5;
		((Control)columnsFilters).Location = new Point(4, 4);
		((Control)columnsFilters).Margin = new Padding(0);
		((Control)columnsFilters).Name = "columnsFilters";
		((Control)columnsFilters).Size = new Size(515, 333);
		((Control)columnsFilters).TabIndex = 4;
		((Control)columnsFilters).TabStop = false;
		((Control)pageGroups).Controls.Add((Control)(object)columnsGroup);
		pageGroups.Dock = (DockStyle)5;
		pageGroups.Image = (Image)componentResourceManager.GetObject("pageGroups.Image");
		pageGroups.InvisibleVisible = false;
		((Control)pageGroups).Location = new Point(0, 0);
		((Control)pageGroups).Margin = new Padding(2, 2, 2, 2);
		((Control)pageGroups).Name = "pageGroups";
		((Control)pageGroups).Padding = new Padding(4, 4, 4, 4);
		((Control)pageGroups).Size = new Size(523, 341);
		((Control)pageGroups).TabIndex = 4;
		((Control)pageGroups).Text = "Groups";
		((Control)pageGroups).Visible = false;
		columnsGroup.DataSource = null;
		((Control)columnsGroup).Dock = (DockStyle)5;
		((Control)columnsGroup).Location = new Point(4, 4);
		((Control)columnsGroup).Margin = new Padding(1, 1, 1, 1);
		((Control)columnsGroup).Name = "columnsGroup";
		((Control)columnsGroup).Size = new Size(515, 333);
		((Control)columnsGroup).TabIndex = 0;
		((Control)pageResults).Controls.Add((Control)(object)columnsResults);
		pageResults.Dock = (DockStyle)5;
		pageResults.Image = (Image)componentResourceManager.GetObject("pageResults.Image");
		pageResults.InvisibleVisible = true;
		((Control)pageResults).Location = new Point(0, 0);
		((Control)pageResults).Margin = new Padding(2, 2, 2, 2);
		((Control)pageResults).Name = "pageResults";
		((Control)pageResults).Padding = new Padding(4, 4, 4, 4);
		((Control)pageResults).Size = new Size(523, 341);
		((Control)pageResults).TabIndex = 5;
		((Control)pageResults).Text = "Results";
		((Control)pageResults).Visible = false;
		columnsResults.DataSource = null;
		((Control)columnsResults).Dock = (DockStyle)5;
		((Control)columnsResults).Location = new Point(4, 4);
		((Control)columnsResults).Margin = new Padding(1, 1, 1, 1);
		((Control)columnsResults).Name = "columnsResults";
		((Control)columnsResults).Size = new Size(515, 333);
		((Control)columnsResults).TabIndex = 0;
		stiMenuProvider1.Font = new Font("Arial", 16f);
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(642, 382);
		((Control)this).Controls.Add((Control)(object)stiGroupLine1);
		((Control)this).Controls.Add((Control)(object)TabControl);
		((Form)this).KeyPreview = true;
		((Form)this).Margin = new Padding(2, 2, 2, 2);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiVirtualSourceEditForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Edit Data Source";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).Closed += This_Closed;
		((Form)this).Load += This_Load;
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((ISupportInitialize)errorProvider).EndInit();
		((Control)stiGroupLine1).ResumeLayout(false);
		((Control)TabControl).ResumeLayout(false);
		((Control)pageDataSource).ResumeLayout(false);
		((Control)pageSort).ResumeLayout(false);
		((Control)pageFilters).ResumeLayout(false);
		((Control)pageGroups).ResumeLayout(false);
		((Control)pageResults).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
