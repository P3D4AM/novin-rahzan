using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Base;
using Stimulsoft.Base.Data.Connectors.Google;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Controls.Win.DotNetBar.Controls;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Design.Dictionary;

public class StiGoogleAnalyticsDatabaseEditForm : StiFormUX
{
	private class StiGAEntityInfo
	{
		public string Name { get; set; }

		public string Id { get; set; }

		public override string ToString()
		{
			return Name;
		}

		public StiGAEntityInfo(string name, string id)
		{
			Name = name;
			Id = id;
		}
	}

	private StiReport report;

	private string oldValue;

	private string originalName;

	private List<KeyValuePair<string, Dictionary<string, string>>> metricsInfo;

	private List<KeyValuePair<string, Dictionary<string, string>>> dimensionsInfo;

	private bool lockCheckedNode;

	private string textSearchMetrics = string.Empty;

	private string textSearchDimensions = string.Empty;

	private IContainer components;

	private StiButtonPanelUX glButtons;

	private StiButtonUX btOk;

	private StiButtonUX btCancel;

	private StiLabelUX labelAlias;

	public StiTextBox tbAlias;

	private StiLabelUX labelName;

	public StiTextBox tbName;

	private StiLabelUX labelServiceAccountKey;

	private ErrorProvider errorProvider;

	private StiTextBoxUX tbServiceAccountKey;

	private StiButtonUX btOpenServiceAccountKeyFile;

	private StiLabelUX labelAccount;

	private StiComboBox comboBoxAccount;

	private StiComboBox comboBoxProperty;

	private StiLabelUX labelProperty;

	private StiComboBox comboBoxView;

	private StiLabelUX labelView;

	private StiLabelUX labelDimensions;

	public StiTreeViewBox treeBoxDimensions;

	public StiTreeViewBox treeBoxMetrics;

	private StiLabelUX labelMetrics;

	private StiComboBox comboBoxEndDate;

	private StiComboBox comboBoxStartDate;

	private StiLabelUX labelEndDate;

	private StiLabelUX labelStartDate;

	private CircularProgress progressPreview;

	public DateTimePicker datePickerEndValue;

	public DateTimePicker datePickerStartValue;

	private StiNumericUpDown numericEndDate;

	private StiNumericUpDown numericStartDate;

	private StiButtonUX btRemoveServiceAccountKeyFile;

	public string Base64EncodedAuthSecret
	{
		get
		{
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(((Control)tbServiceAccountKey).Text));
		}
		set
		{
			try
			{
				byte[] bytes = Convert.FromBase64String(value ?? string.Empty);
				((Control)tbServiceAccountKey).Text = Encoding.UTF8.GetString(bytes);
			}
			catch (FormatException)
			{
				((Control)tbServiceAccountKey).Text = string.Empty;
			}
		}
	}

	public string AccountId { get; set; }

	public string PropertyId { get; set; }

	public string ViewId { get; set; }

	public string StartDate { get; set; }

	public string EndDate { get; set; }

	public string Metrics { get; set; }

	public string Dimensions { get; set; }

	public string ConnectionString => "Base64EncodedAuthSecret=" + Base64EncodedAuthSecret + ";AccountId=" + AccountId + ";PropertyId=" + PropertyId + ";ViewId=" + ViewId + ";Metrics=" + Metrics + ";Dimensions=" + Dimensions + ";StartDate=" + StartDate + ";EndDate=" + EndDate;

	public StiGoogleAnalyticsDatabaseEditForm(StiReport report, string originalName)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			this.report = report;
			this.originalName = originalName;
			((Form)this).ShowIcon = false;
			if (StiOptions.Appearance.ShowFormsHelp)
			{
				((Form)this).HelpButton = true;
			}
			numericStartDate.Minimum = 0m;
			numericEndDate.Minimum = 0m;
			Localize();
			DoScaling();
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

	private async void ButtonOpenServiceAccountKeyFile_Click(object sender, EventArgs e)
	{
		OpenFileDialog dialog = new OpenFileDialog();
		try
		{
			((FileDialog)dialog).DefaultExt = "json";
			((FileDialog)dialog).Filter = "JSON files|*.json";
			((FileDialog)dialog).RestoreDirectory = true;
			if ((int)((CommonDialog)dialog).ShowDialog() == 1)
			{
				((TextBoxBase)tbServiceAccountKey).Lines = File.ReadAllLines(((FileDialog)dialog).FileName);
				RunProgress();
				List<KeyValuePair<string, string>> accounts = await StiGoogleAnalyticsConnector.Get(ConnectionString).GetAccountsAsync();
				StopProgress();
				FillAccounts(accounts);
			}
		}
		finally
		{
			((IDisposable)dialog)?.Dispose();
		}
	}

	private void ButtonRemoveServiceAccountKeyFile_Click(object sender, EventArgs e)
	{
		((TextBoxBase)tbServiceAccountKey).Lines = null;
		((ComboBox)comboBoxAccount).Items.Clear();
		((ComboBox)comboBoxProperty).Items.Clear();
		((ComboBox)comboBoxView).Items.Clear();
	}

	private async void ComboBoxAccount_SelectedIndexChanged(object sender, EventArgs e)
	{
		AccountId = ((StiGAEntityInfo)((ComboBox)comboBoxAccount).SelectedItem).Id;
		if (AccountId != null)
		{
			RunProgress();
			List<KeyValuePair<string, string>> properties = await StiGoogleAnalyticsConnector.Get(ConnectionString).GetPropertiesAsync(AccountId);
			StopProgress();
			FillPproperties(properties);
		}
	}

	private async void ComboBoxProperty_SelectedIndexChanged(object sender, EventArgs e)
	{
		PropertyId = ((StiGAEntityInfo)((ComboBox)comboBoxProperty).SelectedItem).Id;
		if (AccountId != null && PropertyId != null)
		{
			RunProgress();
			List<KeyValuePair<string, string>> views = await StiGoogleAnalyticsConnector.Get(ConnectionString).GetViewsAsync(AccountId, PropertyId);
			StopProgress();
			FillViews(views);
		}
	}

	private void ComboBoxView_SelectedIndexChanged(object sender, EventArgs e)
	{
		ViewId = ((StiGAEntityInfo)((ComboBox)comboBoxView).SelectedItem).Id;
	}

	private void TreeBoxMetrics_TreeNodesFill(object sender, StiTreeNodesFillEventArgs e)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Expected O, but got Unknown
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_033c: Expected O, but got Unknown
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_0399: Expected O, but got Unknown
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Expected O, but got Unknown
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Expected O, but got Unknown
		((TreeView)e.TreeView).CheckBoxes = true;
		((Control)e.TreeView).Dock = (DockStyle)5;
		((TreeView)e.TreeView).ShowNodeToolTips = true;
		((TreeView)e.TreeView).AfterCheck -= new TreeViewEventHandler(TreeViewMetric_AfterCheck);
		((TreeView)e.TreeView).AfterCheck += new TreeViewEventHandler(TreeViewMetric_AfterCheck);
		((Control)e.TreeView).Controls.Clear();
		e.Nodes.Clear();
		if (((ArrangedElementCollection)((Control)e.TreeView).Parent.Controls).Count == 1)
		{
			Panel val = new Panel
			{
				Dock = (DockStyle)1,
				Size = StiScale.I(new Size(((Control)e.TreeView).Width - 2, StiScale.I(33))),
				Padding = new Padding(3)
			};
			((Control)val).Height = StiScale2.YYI(46.0);
			StiHintTextBox stiHintTextBox = new StiHintTextBox();
			((Control)stiHintTextBox).Text = textSearchMetrics;
			((Control)stiHintTextBox).Dock = (DockStyle)5;
			stiHintTextBox.Hint = StiLocalization.Get("Editor", "TypeToSearch");
			((Control)stiHintTextBox).Width = StiScale2.XXI(280.0);
			StiHintTextBox stiHintTextBox2 = stiHintTextBox;
			((Control)stiHintTextBox2).TextChanged += MetricsSearch_TextChanged;
			((Control)stiHintTextBox2).Focus();
			Label val2 = new Label();
			((Control)val2).ForeColor = Color.DimGray;
			((Control)val2).Height = StiScale2.YYI(15.0);
			((Control)val2).Dock = (DockStyle)2;
			((Control)val2).Text = Loc.Get("Editor", "BetweenMetrics");
			StiDotSeparator stiDotSeparator = new StiDotSeparator();
			((Control)stiDotSeparator).Dock = (DockStyle)2;
			StiDotSeparator stiDotSeparator2 = stiDotSeparator;
			((Control)val).Controls.Add((Control)(object)val2);
			((Control)val).Controls.Add((Control)(object)stiDotSeparator2);
			((Control)val).Controls.Add((Control)(object)stiHintTextBox2);
			((Control)e.TreeView).Parent.Controls.Add((Control)(object)val);
		}
		List<string> list = (from d in Metrics?.Split(new char[1] { ',' })
			select d.Trim() into m
			where !string.IsNullOrEmpty(m)
			select m).ToList();
		if (!string.IsNullOrEmpty(textSearchMetrics))
		{
			foreach (KeyValuePair<string, Dictionary<string, string>> item in metricsInfo)
			{
				if (item.Key.Contains(textSearchMetrics))
				{
					TreeNode val3 = new TreeNode(item.Key)
					{
						Checked = (list?.Contains(item.Key) ?? false),
						ToolTipText = item.Value["description"]
					};
					e.Nodes.Add(val3);
				}
			}
			return;
		}
		List<TreeNode> list2 = new List<TreeNode>();
		foreach (KeyValuePair<string, Dictionary<string, string>> item2 in metricsInfo)
		{
			string groupName = item2.Value["group"];
			if (!list2.Where((TreeNode n) => n.Text == groupName).Any())
			{
				list2.Add(new TreeNode(groupName));
			}
			TreeNode val4 = list2.Where((TreeNode n) => n.Text == groupName).FirstOrDefault();
			bool flag = list?.Contains(item2.Key) ?? false;
			TreeNode val5 = new TreeNode(item2.Key)
			{
				Checked = flag,
				ToolTipText = item2.Value["description"]
			};
			val4.Nodes.Add(val5);
			if (flag)
			{
				val4.Expand();
			}
		}
		foreach (TreeNode item3 in list2)
		{
			bool @checked = true;
			foreach (TreeNode node in item3.Nodes)
			{
				TreeNode val6 = node;
				if (!val6.Checked)
				{
					@checked = false;
					break;
				}
			}
			lockCheckedNode = true;
			item3.Checked = @checked;
			lockCheckedNode = false;
			e.Nodes.Add(item3);
		}
	}

	private void MetricsSearch_TextChanged(object sender, EventArgs e)
	{
		StiHintTextBox stiHintTextBox = sender as StiHintTextBox;
		textSearchMetrics = ((Control)stiHintTextBox).Text;
		treeBoxMetrics.RefreshTreeNodesFill();
	}

	private void TreeViewMetric_AfterCheck(object sender, TreeViewEventArgs e)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Expected O, but got Unknown
		if (lockCheckedNode)
		{
			return;
		}
		foreach (TreeNode node in e.Node.Nodes)
		{
			TreeNode val = node;
			lockCheckedNode = true;
			val.Checked = e.Node.Checked;
			lockCheckedNode = false;
		}
		if (e.Node.Parent != null)
		{
			bool @checked = true;
			foreach (TreeNode node2 in e.Node.Parent.Nodes)
			{
				TreeNode val2 = node2;
				if (!val2.Checked)
				{
					@checked = false;
					break;
				}
			}
			lockCheckedNode = true;
			e.Node.Parent.Checked = @checked;
			lockCheckedNode = false;
		}
		if (e.Node.Nodes.Count > 0)
		{
			foreach (TreeNode node3 in e.Node.Nodes)
			{
				TreeNode val3 = node3;
				if (e.Node.Checked)
				{
					AddKeyMetric(val3.Text);
				}
				else
				{
					RemoveKeyMetric(val3.Text);
				}
			}
			return;
		}
		if (e.Node.Checked)
		{
			AddKeyMetric(e.Node.Text);
		}
		else
		{
			RemoveKeyMetric(e.Node.Text);
		}
	}

	private void DimensionsSearch_TextChanged(object sender, EventArgs e)
	{
		StiHintTextBox stiHintTextBox = sender as StiHintTextBox;
		textSearchDimensions = ((Control)stiHintTextBox).Text;
		treeBoxDimensions.RefreshTreeNodesFill();
	}

	private void TreeBoxDimensions_TreeNodesFill(object sender, StiTreeNodesFillEventArgs e)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Expected O, but got Unknown
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_033c: Expected O, but got Unknown
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_0399: Expected O, but got Unknown
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Expected O, but got Unknown
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Expected O, but got Unknown
		((TreeView)e.TreeView).CheckBoxes = true;
		((Control)e.TreeView).Dock = (DockStyle)5;
		((TreeView)e.TreeView).ShowNodeToolTips = true;
		((TreeView)e.TreeView).AfterCheck -= new TreeViewEventHandler(TreeViewDimension_AfterCheck);
		((TreeView)e.TreeView).AfterCheck += new TreeViewEventHandler(TreeViewDimension_AfterCheck);
		((Control)e.TreeView).Controls.Clear();
		e.Nodes.Clear();
		if (((ArrangedElementCollection)((Control)e.TreeView).Parent.Controls).Count == 1)
		{
			Panel val = new Panel
			{
				Dock = (DockStyle)1,
				Size = StiScale.I(new Size(((Control)e.TreeView).Width - 2, StiScale.I(33))),
				Padding = new Padding(3)
			};
			((Control)val).Height = StiScale2.YYI(46.0);
			StiHintTextBox stiHintTextBox = new StiHintTextBox();
			((Control)stiHintTextBox).Text = textSearchDimensions;
			((Control)stiHintTextBox).Dock = (DockStyle)5;
			stiHintTextBox.Hint = StiLocalization.Get("Editor", "TypeToSearch");
			((Control)stiHintTextBox).Width = StiScale2.XXI(280.0);
			StiHintTextBox stiHintTextBox2 = stiHintTextBox;
			((Control)stiHintTextBox2).TextChanged += DimensionsSearch_TextChanged;
			((Control)stiHintTextBox2).Focus();
			Label val2 = new Label();
			((Control)val2).ForeColor = Color.DimGray;
			((Control)val2).Height = StiScale2.YYI(15.0);
			((Control)val2).Dock = (DockStyle)2;
			((Control)val2).Text = Loc.Get("Editor", "MaximumDimensions");
			StiDotSeparator stiDotSeparator = new StiDotSeparator();
			((Control)stiDotSeparator).Dock = (DockStyle)2;
			StiDotSeparator stiDotSeparator2 = stiDotSeparator;
			((Control)val).Controls.Add((Control)(object)val2);
			((Control)val).Controls.Add((Control)(object)stiDotSeparator2);
			((Control)val).Controls.Add((Control)(object)stiHintTextBox2);
			((Control)e.TreeView).Parent.Controls.Add((Control)(object)val);
		}
		List<string> list = (from d in Dimensions?.Split(new char[1] { ',' })
			select d.Trim() into m
			where !string.IsNullOrEmpty(m)
			select m).ToList();
		if (!string.IsNullOrEmpty(textSearchDimensions))
		{
			foreach (KeyValuePair<string, Dictionary<string, string>> item in dimensionsInfo)
			{
				if (item.Key.Contains(textSearchDimensions))
				{
					TreeNode val3 = new TreeNode(item.Key)
					{
						Checked = (list?.Contains(item.Key) ?? false),
						ToolTipText = item.Value["description"]
					};
					e.Nodes.Add(val3);
				}
			}
			return;
		}
		List<TreeNode> list2 = new List<TreeNode>();
		foreach (KeyValuePair<string, Dictionary<string, string>> item2 in dimensionsInfo)
		{
			string groupName = item2.Value["group"];
			if (!list2.Where((TreeNode n) => n.Text == groupName).Any())
			{
				list2.Add(new TreeNode(groupName));
			}
			TreeNode val4 = list2.Where((TreeNode n) => n.Text == groupName).FirstOrDefault();
			bool flag = list?.Contains(item2.Key) ?? false;
			TreeNode val5 = new TreeNode(item2.Key)
			{
				Checked = flag,
				ToolTipText = item2.Value["description"]
			};
			val4.Nodes.Add(val5);
			if (flag)
			{
				val4.Expand();
			}
		}
		foreach (TreeNode item3 in list2)
		{
			bool @checked = true;
			foreach (TreeNode node in item3.Nodes)
			{
				TreeNode val6 = node;
				if (!val6.Checked)
				{
					@checked = false;
					break;
				}
			}
			lockCheckedNode = true;
			item3.Checked = @checked;
			lockCheckedNode = false;
			e.Nodes.Add(item3);
		}
	}

	private void TreeViewDimension_AfterCheck(object sender, TreeViewEventArgs e)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Expected O, but got Unknown
		if (lockCheckedNode)
		{
			return;
		}
		foreach (TreeNode node in e.Node.Nodes)
		{
			TreeNode val = node;
			lockCheckedNode = true;
			val.Checked = e.Node.Checked;
			lockCheckedNode = false;
		}
		if (e.Node.Parent != null)
		{
			bool @checked = true;
			foreach (TreeNode node2 in e.Node.Parent.Nodes)
			{
				TreeNode val2 = node2;
				if (!val2.Checked)
				{
					@checked = false;
					break;
				}
			}
			lockCheckedNode = true;
			e.Node.Parent.Checked = @checked;
			lockCheckedNode = false;
		}
		if (e.Node.Nodes.Count > 0)
		{
			foreach (TreeNode node3 in e.Node.Nodes)
			{
				TreeNode val3 = node3;
				if (e.Node.Checked)
				{
					AddKeyDimension(val3.Text);
				}
				else
				{
					RemoveKeyDimension(val3.Text);
				}
			}
			return;
		}
		if (e.Node.Checked)
		{
			AddKeyDimension(e.Node.Text);
		}
		else
		{
			RemoveKeyDimension(e.Node.Text);
		}
	}

	private void ComboBoxStartDate_SelectedIndexChanged(object sender, EventArgs e)
	{
		SetStartDate();
	}

	private void ComboBoxEndDate_SelectedIndexChanged(object sender, EventArgs e)
	{
		SetEndDate();
	}

	private void NumericStartDate_ValueChanged(object sender, EventArgs e)
	{
		SetStartDate();
	}

	private void NumericEndDate_ValueChanged(object sender, EventArgs e)
	{
		SetEndDate();
	}

	private void DatePickerStartValue_ValueChanged(object sender, EventArgs e)
	{
		SetStartDate();
	}

	private void DatePickerEndValue_ValueChanged(object sender, EventArgs e)
	{
		SetEndDate();
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/index.html?data_data_dictionary_datasources.htm");
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		if (((Control)tbName).Text.Trim().Length == 0)
		{
			errorProvider.SetError((Control)(object)tbName, StiFieldChecker.NameRequire);
			((Control)tbName).Focus();
		}
		else if (((Control)tbName).Text != originalName && StiNameCreation.IsConnectionNameExists(report, ((Control)tbName).Text))
		{
			errorProvider.SetError((Control)(object)tbName, StiFieldChecker.NameExists(((Control)tbName).Text));
			((Control)tbName).Focus();
		}
		else
		{
			((Form)this).DialogResult = (DialogResult)1;
		}
	}

	private async void This_Load(object sender, EventArgs e)
	{
		((Control)tbName).Focus();
		await BuildAsync();
	}

	private void AddKeyMetric(string key)
	{
		string empty = string.Empty;
		List<string> list = (from d in Metrics?.Split(new char[1] { ',' })
			select d.Trim() into m
			where !string.IsNullOrEmpty(m)
			select m).ToList();
		if (list == null || list.Count == 0)
		{
			empty = (Metrics = key + ",");
			((Control)treeBoxMetrics).Text = empty;
		}
		else if (!list.Contains(key))
		{
			empty = (Metrics = (string.IsNullOrEmpty(Metrics) ? (key + ",") : (Metrics + " " + key + ",")));
			((Control)treeBoxMetrics).Text = empty;
		}
	}

	private void RemoveKeyMetric(string key)
	{
		string text = string.Empty;
		List<string> list = (from d in Metrics?.Split(new char[1] { ',' })
			select d.Trim() into m
			where !string.IsNullOrEmpty(m)
			select m).ToList();
		if (list != null)
		{
			list.Remove(key);
			foreach (string item in list)
			{
				text = (string.IsNullOrEmpty(text) ? (item + ",") : (text + " " + item + ","));
			}
		}
		Metrics = text;
		((Control)treeBoxMetrics).Text = text;
	}

	private void AddKeyDimension(string key)
	{
		string empty = string.Empty;
		List<string> list = (from d in Dimensions?.Split(new char[1] { ',' })
			select d.Trim() into m
			where !string.IsNullOrEmpty(m)
			select m).ToList();
		if (list == null || list.Count == 0)
		{
			empty = (Dimensions = key + ",");
			((Control)treeBoxDimensions).Text = empty;
		}
		else if (!list.Contains(key))
		{
			empty = (Dimensions = (string.IsNullOrEmpty(Dimensions) ? (key + ",") : (Dimensions + " " + key + ",")));
			((Control)treeBoxDimensions).Text = empty;
		}
	}

	private void RemoveKeyDimension(string key)
	{
		string text = string.Empty;
		List<string> list = (from d in Dimensions?.Split(new char[1] { ',' })
			select d.Trim() into m
			where !string.IsNullOrEmpty(m)
			select m).ToList();
		if (list != null)
		{
			list.Remove(key);
			foreach (string item in list)
			{
				text = (string.IsNullOrEmpty(text) ? (item + ",") : (text + " " + item + ","));
			}
		}
		Dimensions = text;
		((Control)treeBoxDimensions).Text = text;
	}

	private async Task BuildAsync()
	{
		RunProgress();
		try
		{
			await LoadMetricsAsync();
			if (!string.IsNullOrEmpty(Base64EncodedAuthSecret))
			{
				FillAccounts(await StiGoogleAnalyticsConnector.Get(ConnectionString).GetAccountsAsync());
			}
			((Control)treeBoxMetrics).Text = Metrics;
			((Control)treeBoxDimensions).Text = Dimensions;
			BuildStartDate();
			BuildEndDate();
		}
		catch
		{
		}
		finally
		{
			StopProgress();
		}
	}

	private void BuildStartDate()
	{
		if (!string.IsNullOrEmpty(StartDate))
		{
			if (DateTime.TryParseExact(StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
			{
				((Control)datePickerStartValue).Visible = true;
				datePickerStartValue.Value = result;
				((Control)comboBoxStartDate).Text = Loc.Get("FormFormatEditor", "Custom");
			}
			else if (StartDate == "today")
			{
				((Control)comboBoxStartDate).Text = Loc.Get("DatePickerRanges", "Today");
			}
			else if (StartDate == "yesterday")
			{
				((Control)comboBoxStartDate).Text = Loc.Get("DatePickerRanges", "Yesterday");
			}
			else if (StartDate.EndsWith("daysago"))
			{
				((Control)numericStartDate).Visible = true;
				if (int.TryParse(StartDate.Replace("daysago", ""), out var result2))
				{
					numericStartDate.Value = result2;
				}
				((Control)comboBoxStartDate).Text = string.Format(Loc.Get("DatePickerRanges", "DaysAgo"), "N");
			}
		}
		else
		{
			StartDate = "yesterday";
		}
	}

	private void BuildEndDate()
	{
		if (!string.IsNullOrEmpty(EndDate))
		{
			if (DateTime.TryParseExact(EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
			{
				((Control)datePickerEndValue).Visible = true;
				datePickerEndValue.Value = result;
				((Control)comboBoxEndDate).Text = Loc.Get("FormFormatEditor", "Custom");
			}
			else if (EndDate == "today")
			{
				((Control)comboBoxEndDate).Text = Loc.Get("DatePickerRanges", "Today");
			}
			else if (EndDate == "yesterday")
			{
				((Control)comboBoxEndDate).Text = Loc.Get("DatePickerRanges", "Yesterday");
			}
			else if (EndDate.EndsWith("daysago"))
			{
				((Control)numericEndDate).Visible = true;
				if (int.TryParse(EndDate.Replace("daysago", ""), out var result2))
				{
					numericEndDate.Value = result2;
				}
				((Control)comboBoxEndDate).Text = string.Format(Loc.Get("DatePickerRanges", "DaysAgo"), "N");
			}
		}
		else
		{
			EndDate = "today";
		}
	}

	private void SetStartDate()
	{
		((Control)numericStartDate).Visible = false;
		((Control)datePickerStartValue).Visible = false;
		if (!string.IsNullOrEmpty(((Control)comboBoxStartDate).Text))
		{
			string text = ((Control)comboBoxStartDate).Text;
			if (text == Loc.Get("FormFormatEditor", "Custom"))
			{
				((Control)datePickerStartValue).Visible = true;
				StartDate = datePickerStartValue.Value.ToString("yyyy-MM-dd");
			}
			else if (text == Loc.Get("DatePickerRanges", "Today"))
			{
				StartDate = "today";
			}
			else if (text == Loc.Get("DatePickerRanges", "Yesterday"))
			{
				StartDate = "yesterday";
			}
			else if (text == string.Format(Loc.Get("DatePickerRanges", "DaysAgo"), "N"))
			{
				((Control)numericStartDate).Visible = true;
				StartDate = $"{numericStartDate.Value}daysago";
			}
		}
	}

	private void SetEndDate()
	{
		((Control)numericEndDate).Visible = false;
		((Control)datePickerEndValue).Visible = false;
		if (!string.IsNullOrEmpty(((Control)comboBoxEndDate).Text))
		{
			string text = ((Control)comboBoxEndDate).Text;
			if (text == Loc.Get("FormFormatEditor", "Custom"))
			{
				((Control)datePickerEndValue).Visible = true;
				EndDate = datePickerEndValue.Value.ToString("yyyy-MM-dd");
			}
			else if (text == Loc.Get("DatePickerRanges", "Today"))
			{
				EndDate = "today";
			}
			else if (text == Loc.Get("DatePickerRanges", "Yesterday"))
			{
				EndDate = "yesterday";
			}
			else if (text == string.Format(Loc.Get("DatePickerRanges", "DaysAgo"), "N"))
			{
				((Control)numericEndDate).Visible = true;
				EndDate = $"{numericEndDate.Value}daysago";
			}
		}
	}

	public async Task LoadMetricsAsync()
	{
		await Task.Run(delegate
		{
			LoadMetrics();
		});
	}

	private void LoadMetrics()
	{
		metricsInfo = StiGoogleAnalyticsConnector.Get(ConnectionString).GetMetrics();
		dimensionsInfo = StiGoogleAnalyticsConnector.Get(ConnectionString).GetDimensions();
	}

	public void Localize()
	{
		((Control)labelName).Text = Loc.GetCleaned("Report", "LabelName");
		((Control)labelAlias).Text = Loc.GetCleaned("Report", "LabelAlias");
		((Control)labelServiceAccountKey).Text = "Service Account Key File";
		((Control)labelStartDate).Text = Loc.GetCleaned("Cloud", "LabelStartDate");
		((Control)labelEndDate).Text = Loc.GetCleaned("Cloud", "LabelEndDate");
		((Control)labelAccount).Text = Loc.Get("Cloud", "Account");
		((Control)labelProperty).Text = Loc.Get("PropertyMain", "Property");
		((Control)labelView).Text = Loc.Get("Cloud", "ButtonView");
		((Control)labelMetrics).Text = Loc.Get("Dashboard", "Metrics");
		((Control)labelDimensions).Text = Loc.Get("Dashboard", "Dimensions");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((ComboBox)comboBoxStartDate).Items.Clear();
		((ComboBox)comboBoxStartDate).Items.Add((object)Loc.Get("DatePickerRanges", "Today"));
		((ComboBox)comboBoxStartDate).Items.Add((object)Loc.Get("DatePickerRanges", "Yesterday"));
		((ComboBox)comboBoxStartDate).Items.Add((object)string.Format(Loc.Get("DatePickerRanges", "DaysAgo"), "N"));
		((ComboBox)comboBoxStartDate).Items.Add((object)Loc.Get("FormFormatEditor", "Custom"));
		((ListControl)comboBoxStartDate).SelectedIndex = 1;
		((ComboBox)comboBoxEndDate).Items.Clear();
		((ComboBox)comboBoxEndDate).Items.Add((object)Loc.Get("DatePickerRanges", "Today"));
		((ComboBox)comboBoxEndDate).Items.Add((object)Loc.Get("DatePickerRanges", "Yesterday"));
		((ComboBox)comboBoxEndDate).Items.Add((object)string.Format(Loc.Get("DatePickerRanges", "DaysAgo"), "N"));
		((ComboBox)comboBoxEndDate).Items.Add((object)Loc.Get("FormFormatEditor", "Custom"));
		((ListControl)comboBoxEndDate).SelectedIndex = 0;
	}

	private void DoScaling()
	{
		((ButtonBase)btOpenServiceAccountKeyFile).Image = StiReportDesignImages.Actions.OpenFile();
		((ButtonBase)btRemoveServiceAccountKeyFile).Image = StiReportDesignImages.Actions.Remove();
		errorProvider.Icon = StiIconUtils.ResizeIcon(errorProvider.Icon, StiScale.I16, StiScale.I16);
	}

	private void FillAccounts(List<KeyValuePair<string, string>> accounts)
	{
		((ComboBox)comboBoxAccount).Items.Clear();
		int num = -1;
		int num2 = 0;
		foreach (KeyValuePair<string, string> account in accounts)
		{
			StiGAEntityInfo stiGAEntityInfo = new StiGAEntityInfo(account.Value, account.Key);
			((ComboBox)comboBoxAccount).Items.Add((object)stiGAEntityInfo);
			if (account.Key == AccountId)
			{
				num = num2;
			}
			num2++;
		}
		if (num == -1 && ((ComboBox)comboBoxAccount).Items.Count > 0)
		{
			num = 0;
		}
		((ListControl)comboBoxAccount).SelectedIndex = num;
	}

	private void FillPproperties(List<KeyValuePair<string, string>> properties)
	{
		((ComboBox)comboBoxProperty).Items.Clear();
		int num = -1;
		int num2 = 0;
		foreach (KeyValuePair<string, string> property in properties)
		{
			StiGAEntityInfo stiGAEntityInfo = new StiGAEntityInfo(property.Value, property.Key);
			((ComboBox)comboBoxProperty).Items.Add((object)stiGAEntityInfo);
			if (property.Key == PropertyId)
			{
				num = num2;
			}
			num2++;
		}
		if (num == -1 && ((ComboBox)comboBoxProperty).Items.Count > 0)
		{
			num = 0;
		}
		((ListControl)comboBoxProperty).SelectedIndex = num;
	}

	private void FillViews(List<KeyValuePair<string, string>> views)
	{
		((ComboBox)comboBoxView).Items.Clear();
		int num = -1;
		int num2 = 0;
		foreach (KeyValuePair<string, string> view in views)
		{
			StiGAEntityInfo stiGAEntityInfo = new StiGAEntityInfo(view.Value, view.Key);
			((ComboBox)comboBoxView).Items.Add((object)stiGAEntityInfo);
			if (view.Key == ViewId)
			{
				num = num2;
			}
			num2++;
		}
		if (num == -1 && ((ComboBox)comboBoxView).Items.Count > 0)
		{
			num = 0;
		}
		((ListControl)comboBoxView).SelectedIndex = num;
	}

	private void RunProgress()
	{
		progressPreview.IsRunning = true;
		((Control)progressPreview).Visible = true;
	}

	private void StopProgress()
	{
		progressPreview.IsRunning = false;
		((Control)progressPreview).Visible = false;
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
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Expected O, but got Unknown
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Expected O, but got Unknown
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Expected O, but got Unknown
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Expected O, but got Unknown
		//IL_08ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_092c: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dcd: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiGoogleAnalyticsDatabaseEditForm));
		labelAlias = new StiLabelUX();
		labelName = new StiLabelUX();
		labelServiceAccountKey = new StiLabelUX();
		errorProvider = new ErrorProvider(components);
		tbServiceAccountKey = new StiTextBoxUX();
		btOpenServiceAccountKeyFile = new StiButtonUX();
		labelAccount = new StiLabelUX();
		labelProperty = new StiLabelUX();
		labelView = new StiLabelUX();
		labelDimensions = new StiLabelUX();
		labelMetrics = new StiLabelUX();
		btOk = new StiButtonUX();
		btCancel = new StiButtonUX();
		treeBoxMetrics = new StiTreeViewBox();
		treeBoxDimensions = new StiTreeViewBox();
		comboBoxView = new StiComboBox();
		comboBoxProperty = new StiComboBox();
		comboBoxAccount = new StiComboBox();
		tbAlias = new StiTextBox();
		tbName = new StiTextBox();
		glButtons = new StiButtonPanelUX();
		progressPreview = new CircularProgress();
		labelStartDate = new StiLabelUX();
		labelEndDate = new StiLabelUX();
		comboBoxStartDate = new StiComboBox();
		comboBoxEndDate = new StiComboBox();
		datePickerStartValue = new DateTimePicker();
		datePickerEndValue = new DateTimePicker();
		numericStartDate = new StiNumericUpDown();
		numericEndDate = new StiNumericUpDown();
		btRemoveServiceAccountKeyFile = new StiButtonUX();
		((ISupportInitialize)errorProvider).BeginInit();
		((Control)glButtons).SuspendLayout();
		((ISupportInitialize)numericStartDate).BeginInit();
		((ISupportInitialize)numericEndDate).BeginInit();
		((Control)this).SuspendLayout();
		((Control)labelAlias).Location = new Point(12, 45);
		((Control)labelAlias).Name = "labelAlias";
		((Control)labelAlias).Size = new Size(134, 20);
		((Control)labelAlias).TabIndex = 2;
		((Control)labelAlias).Text = "Alias";
		((Label)labelAlias).TextAlign = (ContentAlignment)16;
		((Control)labelName).Location = new Point(12, 12);
		((Control)labelName).Name = "labelName";
		((Control)labelName).Size = new Size(134, 20);
		((Control)labelName).TabIndex = 0;
		((Control)labelName).Text = "Name";
		((Label)labelName).TextAlign = (ContentAlignment)16;
		((Control)labelServiceAccountKey).Location = new Point(12, 78);
		((Control)labelServiceAccountKey).Name = "labelServiceAccountKey";
		((Control)labelServiceAccountKey).Size = new Size(134, 24);
		((Control)labelServiceAccountKey).TabIndex = 4;
		((Control)labelServiceAccountKey).Text = "Service Account Key";
		((Label)labelServiceAccountKey).TextAlign = (ContentAlignment)16;
		errorProvider.ContainerControl = (ContainerControl)(object)this;
		errorProvider.DataMember = "";
		errorProvider.Icon = (Icon)componentResourceManager.GetObject("errorProvider.Icon");
		((TextBox)tbServiceAccountKey).AcceptsReturn = true;
		((Control)tbServiceAccountKey).Anchor = (AnchorStyles)13;
		((Control)tbServiceAccountKey).Location = new Point(152, 78);
		((Control)tbServiceAccountKey).Margin = new Padding(2);
		((TextBoxBase)tbServiceAccountKey).Multiline = true;
		((Control)tbServiceAccountKey).Name = "tbServiceAccountKey";
		((TextBoxBase)tbServiceAccountKey).ReadOnly = true;
		((Control)tbServiceAccountKey).Size = new Size(287, 87);
		((Control)tbServiceAccountKey).TabIndex = 2;
		((Control)tbServiceAccountKey).TabStop = false;
		((TextBoxBase)tbServiceAccountKey).WordWrap = false;
		((Control)btOpenServiceAccountKeyFile).Anchor = (AnchorStyles)9;
		((ButtonBase)btOpenServiceAccountKeyFile).Image = (Image)componentResourceManager.GetObject("btOpenServiceAccountKeyFile.Image");
		((Control)btOpenServiceAccountKeyFile).Location = new Point(444, 78);
		((Control)btOpenServiceAccountKeyFile).Name = "btOpenServiceAccountKeyFile";
		((Control)btOpenServiceAccountKeyFile).Size = new Size(24, 24);
		((Control)btOpenServiceAccountKeyFile).TabIndex = 3;
		((ButtonBase)btOpenServiceAccountKeyFile).UseVisualStyleBackColor = true;
		((Control)btOpenServiceAccountKeyFile).Click += ButtonOpenServiceAccountKeyFile_Click;
		((Control)labelAccount).Location = new Point(12, 176);
		((Control)labelAccount).Name = "labelAccount";
		((Control)labelAccount).Size = new Size(134, 20);
		((Control)labelAccount).TabIndex = 7;
		((Control)labelAccount).Text = "Account";
		((Label)labelAccount).TextAlign = (ContentAlignment)16;
		((Control)labelProperty).Location = new Point(12, 209);
		((Control)labelProperty).Name = "labelProperty";
		((Control)labelProperty).Size = new Size(134, 20);
		((Control)labelProperty).TabIndex = 9;
		((Control)labelProperty).Text = "Property";
		((Label)labelProperty).TextAlign = (ContentAlignment)16;
		((Control)labelView).Location = new Point(12, 242);
		((Control)labelView).Name = "labelView";
		((Control)labelView).Size = new Size(134, 20);
		((Control)labelView).TabIndex = 11;
		((Control)labelView).Text = "View";
		((Label)labelView).TextAlign = (ContentAlignment)16;
		((Control)labelDimensions).Location = new Point(12, 309);
		((Control)labelDimensions).Name = "labelDimensions";
		((Control)labelDimensions).Size = new Size(134, 20);
		((Control)labelDimensions).TabIndex = 15;
		((Control)labelDimensions).Text = "Dimensions";
		((Label)labelDimensions).TextAlign = (ContentAlignment)16;
		((Control)labelMetrics).Location = new Point(12, 276);
		((Control)labelMetrics).Name = "labelMetrics";
		((Control)labelMetrics).Size = new Size(134, 20);
		((Control)labelMetrics).TabIndex = 13;
		((Control)labelMetrics).Text = "Metrics";
		((Label)labelMetrics).TextAlign = (ContentAlignment)16;
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(310, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 0;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += This_OkClick;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		((Button)btCancel).DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(391, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 1;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)treeBoxMetrics).Anchor = (AnchorStyles)13;
		treeBoxMetrics.Flat = false;
		((Control)treeBoxMetrics).Location = new Point(152, 276);
		treeBoxMetrics.MaxDropDownHeight = 360;
		((Control)treeBoxMetrics).Name = "treeBoxMetrics";
		treeBoxMetrics.SelectOnlyTagNotNull = true;
		((Control)treeBoxMetrics).Size = new Size(288, 21);
		((Control)treeBoxMetrics).TabIndex = 8;
		treeBoxMetrics.TreeNodesFill += TreeBoxMetrics_TreeNodesFill;
		((Control)treeBoxDimensions).Anchor = (AnchorStyles)13;
		treeBoxDimensions.Flat = false;
		((Control)treeBoxDimensions).Location = new Point(152, 309);
		treeBoxDimensions.MaxDropDownHeight = 360;
		((Control)treeBoxDimensions).Name = "treeBoxDimensions";
		treeBoxDimensions.SelectOnlyTagNotNull = true;
		((Control)treeBoxDimensions).Size = new Size(288, 21);
		((Control)treeBoxDimensions).TabIndex = 9;
		treeBoxDimensions.TreeNodesFill += TreeBoxDimensions_TreeNodesFill;
		comboBoxView.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxView).Location = new Point(152, 243);
		((Control)comboBoxView).Margin = new Padding(2);
		((Control)comboBoxView).Name = "comboBoxView";
		((Control)comboBoxView).Size = new Size(288, 21);
		((Control)comboBoxView).TabIndex = 7;
		((ComboBox)comboBoxView).SelectedIndexChanged += ComboBoxView_SelectedIndexChanged;
		comboBoxProperty.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxProperty).Location = new Point(152, 210);
		((Control)comboBoxProperty).Margin = new Padding(2);
		((Control)comboBoxProperty).Name = "comboBoxProperty";
		((Control)comboBoxProperty).Size = new Size(288, 21);
		((Control)comboBoxProperty).TabIndex = 6;
		((ComboBox)comboBoxProperty).SelectedIndexChanged += ComboBoxProperty_SelectedIndexChanged;
		comboBoxAccount.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxAccount).Location = new Point(152, 177);
		((Control)comboBoxAccount).Margin = new Padding(2);
		((Control)comboBoxAccount).Name = "comboBoxAccount";
		((Control)comboBoxAccount).Size = new Size(288, 21);
		((Control)comboBoxAccount).TabIndex = 5;
		((ComboBox)comboBoxAccount).SelectedIndexChanged += ComboBoxAccount_SelectedIndexChanged;
		((Control)tbAlias).Anchor = (AnchorStyles)13;
		((Control)tbAlias).Location = new Point(152, 45);
		((Control)tbAlias).Name = "tbAlias";
		((Control)tbAlias).Size = new Size(315, 21);
		((Control)tbAlias).TabIndex = 1;
		((Control)tbName).Anchor = (AnchorStyles)13;
		((Control)tbName).Location = new Point(152, 12);
		((Control)tbName).Name = "tbName";
		((Control)tbName).Size = new Size(315, 21);
		((Control)tbName).TabIndex = 0;
		((Control)tbName).Enter += TextBoxName_Enter;
		((Control)tbName).Leave += TextBoxName_Leave;
		((Control)glButtons).Controls.Add((Control)(object)progressPreview);
		((Control)glButtons).Controls.Add((Control)(object)btOk);
		((Control)glButtons).Controls.Add((Control)(object)btCancel);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = StiLinePosition.Top;
		glButtons.LineStyle = StiLineStyle.Dot;
		((Control)glButtons).Location = new Point(0, 408);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(479, 48);
		((Control)glButtons).TabIndex = 14;
		((Control)glButtons).TabStop = false;
		((Control)progressPreview).Anchor = (AnchorStyles)9;
		((Control)progressPreview).BackColor = Color.White;
		progressPreview.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)progressPreview).Location = new Point(283, 14);
		((Control)progressPreview).Name = "progressPreview";
		progressPreview.ProgressBarType = eCircularProgressType.Donut;
		progressPreview.ProgressColor = Color.SteelBlue;
		((Control)progressPreview).Size = new Size(20, 20);
		progressPreview.Style = eDotNetBarStyle.OfficeXP;
		((Control)progressPreview).TabIndex = 2;
		((Control)progressPreview).TabStop = false;
		((Control)progressPreview).Visible = false;
		((Control)labelStartDate).Location = new Point(12, 341);
		((Control)labelStartDate).Name = "labelStartDate";
		((Control)labelStartDate).Size = new Size(134, 20);
		((Control)labelStartDate).TabIndex = 17;
		((Control)labelStartDate).Text = "Start Date";
		((Label)labelStartDate).TextAlign = (ContentAlignment)16;
		((Control)labelEndDate).Location = new Point(12, 376);
		((Control)labelEndDate).Name = "labelEndDate";
		((Control)labelEndDate).Size = new Size(134, 20);
		((Control)labelEndDate).TabIndex = 20;
		((Control)labelEndDate).Text = "End Date";
		((Label)labelEndDate).TextAlign = (ContentAlignment)16;
		comboBoxStartDate.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxStartDate).Location = new Point(152, 342);
		((Control)comboBoxStartDate).Margin = new Padding(2);
		((Control)comboBoxStartDate).Name = "comboBoxStartDate";
		((Control)comboBoxStartDate).Size = new Size(144, 21);
		((Control)comboBoxStartDate).TabIndex = 10;
		((ComboBox)comboBoxStartDate).SelectedIndexChanged += ComboBoxStartDate_SelectedIndexChanged;
		comboBoxEndDate.DropDownStyle = (ComboBoxStyle)2;
		((Control)comboBoxEndDate).Location = new Point(152, 375);
		((Control)comboBoxEndDate).Margin = new Padding(2);
		((Control)comboBoxEndDate).Name = "comboBoxEndDate";
		((Control)comboBoxEndDate).Size = new Size(144, 21);
		((Control)comboBoxEndDate).TabIndex = 12;
		((ComboBox)comboBoxEndDate).SelectedIndexChanged += ComboBoxEndDate_SelectedIndexChanged;
		((Control)datePickerStartValue).Anchor = (AnchorStyles)13;
		datePickerStartValue.Format = (DateTimePickerFormat)2;
		((Control)datePickerStartValue).Location = new Point(300, 342);
		((Control)datePickerStartValue).Name = "datePickerStartValue";
		((Control)datePickerStartValue).Size = new Size(140, 20);
		((Control)datePickerStartValue).TabIndex = 20;
		((Control)datePickerStartValue).Visible = false;
		datePickerStartValue.ValueChanged += DatePickerStartValue_ValueChanged;
		((Control)datePickerEndValue).Anchor = (AnchorStyles)13;
		datePickerEndValue.Format = (DateTimePickerFormat)2;
		((Control)datePickerEndValue).Location = new Point(300, 375);
		((Control)datePickerEndValue).Name = "datePickerEndValue";
		((Control)datePickerEndValue).Size = new Size(140, 20);
		((Control)datePickerEndValue).TabIndex = 23;
		((Control)datePickerEndValue).Visible = false;
		datePickerEndValue.ValueChanged += DatePickerEndValue_ValueChanged;
		numericStartDate.DecimalPlaces = 0;
		((Control)numericStartDate).Location = new Point(300, 342);
		((Control)numericStartDate).Name = "numericStartDate";
		((Control)numericStartDate).Size = new Size(140, 21);
		((Control)numericStartDate).TabIndex = 11;
		((Control)numericStartDate).Visible = false;
		numericStartDate.ValueChanged += NumericStartDate_ValueChanged;
		numericEndDate.DecimalPlaces = 0;
		((Control)numericEndDate).Location = new Point(300, 375);
		((Control)numericEndDate).Name = "numericEndDate";
		((Control)numericEndDate).Size = new Size(140, 21);
		((Control)numericEndDate).TabIndex = 13;
		((Control)numericEndDate).Visible = false;
		numericEndDate.ValueChanged += NumericEndDate_ValueChanged;
		((Control)btRemoveServiceAccountKeyFile).Anchor = (AnchorStyles)9;
		((Control)btRemoveServiceAccountKeyFile).Location = new Point(444, 108);
		((Control)btRemoveServiceAccountKeyFile).Name = "btRemoveServiceAccountKeyFile";
		((Control)btRemoveServiceAccountKeyFile).Size = new Size(24, 24);
		((Control)btRemoveServiceAccountKeyFile).TabIndex = 4;
		((ButtonBase)btRemoveServiceAccountKeyFile).UseVisualStyleBackColor = true;
		((Control)btRemoveServiceAccountKeyFile).Click += ButtonRemoveServiceAccountKeyFile_Click;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(479, 456);
		((Control)this).Controls.Add((Control)(object)btRemoveServiceAccountKeyFile);
		((Control)this).Controls.Add((Control)(object)numericEndDate);
		((Control)this).Controls.Add((Control)(object)numericStartDate);
		((Control)this).Controls.Add((Control)(object)datePickerEndValue);
		((Control)this).Controls.Add((Control)(object)datePickerStartValue);
		((Control)this).Controls.Add((Control)(object)comboBoxEndDate);
		((Control)this).Controls.Add((Control)(object)comboBoxStartDate);
		((Control)this).Controls.Add((Control)(object)labelEndDate);
		((Control)this).Controls.Add((Control)(object)labelStartDate);
		((Control)this).Controls.Add((Control)(object)treeBoxMetrics);
		((Control)this).Controls.Add((Control)(object)labelMetrics);
		((Control)this).Controls.Add((Control)(object)treeBoxDimensions);
		((Control)this).Controls.Add((Control)(object)labelDimensions);
		((Control)this).Controls.Add((Control)(object)comboBoxView);
		((Control)this).Controls.Add((Control)(object)labelView);
		((Control)this).Controls.Add((Control)(object)comboBoxProperty);
		((Control)this).Controls.Add((Control)(object)labelProperty);
		((Control)this).Controls.Add((Control)(object)comboBoxAccount);
		((Control)this).Controls.Add((Control)(object)labelAccount);
		((Control)this).Controls.Add((Control)(object)btOpenServiceAccountKeyFile);
		((Control)this).Controls.Add((Control)(object)tbServiceAccountKey);
		((Control)this).Controls.Add((Control)(object)labelServiceAccountKey);
		((Control)this).Controls.Add((Control)(object)labelAlias);
		((Control)this).Controls.Add((Control)(object)tbAlias);
		((Control)this).Controls.Add((Control)(object)labelName);
		((Control)this).Controls.Add((Control)(object)tbName);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiGoogleAnalyticsDatabaseEditForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "StiBigQueryDatabaseEditForm";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Form)this).Load += This_Load;
		((ISupportInitialize)errorProvider).EndInit();
		((Control)glButtons).ResumeLayout(false);
		((ISupportInitialize)numericStartDate).EndInit();
		((ISupportInitialize)numericEndDate).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
