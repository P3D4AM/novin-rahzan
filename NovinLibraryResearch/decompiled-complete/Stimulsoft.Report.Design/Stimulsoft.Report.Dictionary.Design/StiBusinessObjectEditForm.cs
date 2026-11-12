using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Dictionary;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Design.Images.Office2013;
using Stimulsoft.Report.Images;

namespace Stimulsoft.Report.Dictionary.Design;

public class StiBusinessObjectEditForm : StiFormUX
{
	private string oldValue;

	private StiBusinessObject businessObject;

	private StiDataColumnsCollection Columns;

	private IContainer components;

	private StiButtonUX btOk;

	private StiButtonUX btCancel;

	private StiLabelUX lbAlias;

	public StiTextBox tbAlias;

	private StiLabelUX lbName;

	private ErrorProvider errorProvider;

	private ToolTip toolTip1;

	public StiTextBox tbName;

	private StiPanelUX pnMain;

	private StiPanelUX pnColumns;

	private StiOffice2013GroupBox gbColumns;

	private StiReportPropertyGrid pgBusinessObject;

	private StiTreeViewUX tvColumns;

	private StiToolBar tbMain;

	private StiToolButton tbGetColumnsFromAssembly;

	private StiToolButton tbRetrieveColumns;

	private StiToolButton stiToolButton1;

	private StiToolButton tbDelete;

	private StiToolButton tbCalcColumnNew;

	private StiToolButton tbColumnNew;

	public StiTextBox textBoxCategory;

	private StiLabelUX labelCategory;

	private StiButtonPanelUX stiGroupLine1;

	public ContextMenuBar ContextMenuBar;

	private ButtonItem menuColumns;

	private ButtonItem ciColumnNew;

	private ButtonItem ciCalcColumnNew;

	private ButtonItem ciDelete;

	private ButtonItem ciRetrieveColumns;

	public DockSite DockSiteTop;

	public DockSite DockSiteBottom;

	public DockSite DockSiteLeft;

	public DockSite DockSiteRight;

	public DotNetBarManager DotNetBarManager;

	public StiBusinessObjectEditForm(StiBusinessObject businessObject, bool newForm)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (((Component)(object)this).DesignMode)
		{
			return;
		}
		DoTheming();
		DoScaling();
		this.businessObject = businessObject;
		if (newForm)
		{
			((Control)this).Text = StiHint.CreateHint(StiLocalization.Get("MainMenu", "menuEditBusinessObjectNew"));
		}
		Build();
		((Control)textBoxCategory).Text = businessObject.Category;
		((Control)tbName).Text = businessObject.Name;
		((Control)tbAlias).Text = businessObject.Alias;
		if (businessObject.ParentBusinessObject != null)
		{
			((Control)labelCategory).Enabled = false;
			((Control)textBoxCategory).Enabled = false;
		}
		Columns = new StiDataColumnsCollection();
		foreach (StiDataColumn column in businessObject.Columns)
		{
			Columns.Add(column.Clone() as StiDataColumn);
		}
		Localize();
		((Form)this).ShowIcon = false;
	}

	private void Localize()
	{
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((Control)labelCategory).Text = Loc.GetCleaned("PropertyMain", "Category");
		((Control)lbName).Text = Loc.GetCleaned("Report", "LabelName");
		((Control)lbAlias).Text = Loc.GetCleaned("Report", "LabelAlias");
		((Control)gbColumns).Text = StiLocalization.Get("PropertyMain", "Columns");
		ciColumnNew.Text = StiLocalization.Get("FormDictionaryDesigner", "ColumnNew");
		ciCalcColumnNew.Text = StiLocalization.Get("FormDictionaryDesigner", "CalcColumnNew");
		ciDelete.Text = StiLocalization.Get("FormDictionaryDesigner", "Delete");
		ciRetrieveColumns.Text = StiLocalization.Get("FormDictionaryDesigner", "RetrieveColumns");
		((Control)tbRetrieveColumns).Text = StiLocalization.Get("FormDictionaryDesigner", "RetrieveColumns");
		((Control)tbGetColumnsFromAssembly).Text = StiLocalization.Get("FormDictionaryDesigner", "GetColumnsFromAssembly");
		toolTip1.SetToolTip((Control)(object)tbRetrieveColumns, StiHint.CreateHint(StiLocalization.Get("FormDictionaryDesigner", "RetrieveColumns")));
		toolTip1.SetToolTip((Control)(object)tbColumnNew, StiHint.CreateHint(StiLocalization.Get("FormDictionaryDesigner", "ColumnNew")));
		toolTip1.SetToolTip((Control)(object)tbCalcColumnNew, StiHint.CreateHint(StiLocalization.Get("FormDictionaryDesigner", "CalcColumnNew")));
		toolTip1.SetToolTip((Control)(object)tbDelete, StiHint.CreateHint(StiLocalization.Get("FormDictionaryDesigner", "Delete")));
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		bool flag = true;
		if (((Control)tbAlias).Text.Trim().Length == 0)
		{
			errorProvider.SetError((Control)(object)tbAlias, string.Format(StiLocalization.Get("Errors", "FieldRequire"), StiLocalization.Get("PropertyMain", "Alias")));
			flag = false;
			((Control)tbAlias).Focus();
		}
		else
		{
			errorProvider.SetError((Control)(object)tbAlias, string.Empty);
		}
		if (((Control)tbName).Text.Trim().Length == 0)
		{
			errorProvider.SetError((Control)(object)tbName, string.Format(StiLocalization.Get("Errors", "FieldRequire"), StiLocalization.Get("PropertyMain", "Name")));
			flag = false;
			((Control)tbName).Focus();
		}
		else
		{
			errorProvider.SetError((Control)(object)tbName, string.Empty);
		}
		if (flag)
		{
			((Form)this).DialogResult = (DialogResult)1;
			SaveParam(ref businessObject);
		}
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		businessObject.Columns.Clear();
		foreach (StiDataColumn column in Columns)
		{
			businessObject.Columns.Add(column);
		}
		((Form)this).Close();
	}

	private void This_Load(object sender, EventArgs e)
	{
		StiFormSettings.Load(((Control)this).Name, (Form)(object)this);
	}

	private void This_Closed(object sender, EventArgs e)
	{
		StiFormSettings.Save(((Control)this).Name, (Form)(object)this);
	}

	private void TreeViewColumns_AfterSelect(object sender, TreeViewEventArgs e)
	{
		((PropertyGrid)pgBusinessObject).SelectedObject = ((TreeView)tvColumns).SelectedNode.Tag;
	}

	private void ButtonColumnNew_Click(object sender, EventArgs e)
	{
		StiDataColumn stiDataColumn = new StiDataColumn();
		businessObject.Columns.Add(stiDataColumn);
		Build();
		SelectNodeByTag(stiDataColumn);
	}

	private void ButtonCalcColumnNew_Click(object sender, EventArgs e)
	{
		StiCalcDataColumn stiCalcDataColumn = new StiCalcDataColumn();
		businessObject.Columns.Add(stiCalcDataColumn);
		Build();
		SelectNodeByTag(stiCalcDataColumn);
	}

	private void ButtonDelete_Click(object sender, EventArgs e)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Invalid comparison between Unknown and I4
		if (((TreeView)tvColumns).SelectedNode != null && (int)MessageBox.Show(StiLocalization.Get("Questions", "qnRemove"), StiLocalization.Get("FormDictionaryDesigner", "title"), (MessageBoxButtons)4) == 6)
		{
			if (((TreeView)tvColumns).SelectedNode.Tag == null && ((TreeView)tvColumns).SelectedNode.Text == StiLocalization.Get("PropertyMain", "Columns"))
			{
				businessObject.Columns.Clear();
				Build();
			}
			else if (((TreeView)tvColumns).SelectedNode.Tag is StiDataColumn)
			{
				businessObject.Columns.Remove(((TreeView)tvColumns).SelectedNode.Tag as StiDataColumn);
				object nextNodeTag = GetNextNodeTag(((TreeView)tvColumns).SelectedNode);
				Build();
				SelectNodeByTag(nextNodeTag);
			}
		}
	}

	private void ButtonRetrieveColumns_Click(object sender, EventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		StiRetrieveColumnsForm stiRetrieveColumnsForm = new StiRetrieveColumnsForm(businessObject);
		try
		{
			if ((int)((Form)stiRetrieveColumnsForm).ShowDialog((IWin32Window)(object)this) == 1)
			{
				Build();
			}
		}
		finally
		{
			((IDisposable)stiRetrieveColumnsForm)?.Dispose();
		}
	}

	private void TreeViewColumns_BeforeCheck(object sender, TreeViewCancelEventArgs e)
	{
		NodeSetCheck(e.Node, !e.Node.Checked);
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

	private void PropertyGridDataSource_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
	{
		Build();
	}

	private void ButtonGetColumnsFromAssembly_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Invalid comparison between Unknown and I4
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Invalid comparison between Unknown and I4
		OpenFileDialog val = new OpenFileDialog();
		try
		{
			((FileDialog)val).Filter = "Assembly files (*.exe;*.dll)|*.exe;*.dll";
			((FileDialog)val).Title = StiLocalization.Get("FormDictionaryDesigner", "OpenAssembly");
			((FileDialog)val).RestoreDirectory = true;
			if ((int)((CommonDialog)val).ShowDialog() != 1 || !(((FileDialog)val).FileName != ""))
			{
				return;
			}
			Assembly assembly = null;
			try
			{
				FileStream fileStream = null;
				try
				{
					fileStream = new FileStream(((FileDialog)val).FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
					byte[] array = new byte[fileStream.Length];
					fileStream.Seek(0L, SeekOrigin.Begin);
					fileStream.Read(array, 0, (int)fileStream.Length);
					assembly = Assembly.Load(array);
				}
				finally
				{
					if (fileStream != null)
					{
						fileStream.Close();
						fileStream = null;
					}
				}
			}
			catch (Exception exception)
			{
				StiExceptionProvider.Show(exception);
			}
			StiSelectPropertiesInAssemblyForm stiSelectPropertiesInAssemblyForm = new StiSelectPropertiesInAssemblyForm(assembly);
			try
			{
				if ((int)((Form)stiSelectPropertiesInAssemblyForm).ShowDialog((IWin32Window)(object)this) != 1)
				{
					return;
				}
				List<TreeNode> selectColumns = stiSelectPropertiesInAssemblyForm.SelectColumns;
				foreach (TreeNode item in selectColumns)
				{
					businessObject.Columns.Add(new StiDataColumn(item.Text, item.Text, item.Tag as Type));
				}
				Build();
			}
			finally
			{
				((IDisposable)stiSelectPropertiesInAssemblyForm)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void TreeViewColumns_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 46 && ((Control)tbDelete).Enabled)
		{
			e.Handled = true;
			ButtonDelete_Click(sender, (EventArgs)(object)e);
		}
	}

	private void DoTheming()
	{
		gbColumns.IsDarkerBackground = true;
	}

	private void DoScaling()
	{
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Expected O, but got Unknown
		tbMain.ControlStyle = StiControlStyle.Office2013Blue;
		StiToolButton button = tbColumnNew;
		Image image = StiImageResizer.GetImage((Image)(object)PanelDictionaryOffice2013Resource.ColumnNew);
		StiResourcesHelper.SetImage((ButtonBase)(object)button, (Bitmap)(object)((image is Bitmap) ? image : null));
		StiToolButton button2 = tbCalcColumnNew;
		Image image2 = StiImageResizer.GetImage((Image)(object)PanelDictionaryOffice2013Resource.CalcColumnNew);
		StiResourcesHelper.SetImage((ButtonBase)(object)button2, (Bitmap)(object)((image2 is Bitmap) ? image2 : null));
		((ButtonBase)tbDelete).Image = StiReportDesignImages.Actions.Remove();
		((ButtonBase)tbColumnNew).Image = StiReportDesignImages.Actions.NewDataColumn();
		((ButtonBase)tbCalcColumnNew).Image = StiReportDesignImages.Actions.NewCalculatedColumn();
		((ButtonBase)tbDelete).Image = StiReportDesignImages.Actions.Remove();
		((ButtonBase)tbRetrieveColumns).Image = StiReportDesignImages.Actions.RetrieveColumnsDown();
		((ButtonBase)tbGetColumnsFromAssembly).Image = StiReportDesignImages.Actions.RetrieveColumnsPlus();
		ciColumnNew.Image = StiReportDesignImages.Actions.NewDataColumn();
		ciCalcColumnNew.Image = StiReportDesignImages.Actions.NewCalculatedColumn();
		ciDelete.Image = StiReportDesignImages.Actions.Remove();
		ciRetrieveColumns.Image = StiReportDesignImages.Actions.RetrieveColumnsDown();
		if (StiScale.Factor != 1.0)
		{
			ImageList val = new ImageList
			{
				ImageSize = new Size(StiScale.I16, StiScale.I16),
				ColorDepth = (ColorDepth)32
			};
			val.Images.Add("BusinessObject", StiReportImages.Dictionary.BusinessObject());
			val.Images.Add("DataColumn", StiReportImages.Dictionary.DataColumn());
			val.Images.Add("CalcColumn", StiReportImages.Dictionary.CalcColumn());
			((TreeView)tvColumns).ImageList = val;
			errorProvider.Icon = StiIconUtils.ResizeIcon(errorProvider.Icon, StiScale.I16, StiScale.I16);
		}
	}

	private void Build()
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		object obj = null;
		if (((TreeView)tvColumns).SelectedNode != null)
		{
			obj = ((TreeView)tvColumns).SelectedNode.Tag;
		}
		((TreeView)tvColumns).Nodes.Clear();
		TreeNode val = new TreeNode(StiLocalization.Get("PropertyMain", "Columns"));
		string imageKey = (val.SelectedImageKey = "BusinessObject");
		val.ImageKey = imageKey;
		((TreeView)tvColumns).Nodes.Add(val);
		foreach (StiDataColumn column in businessObject.Columns)
		{
			TreeNode val2 = new TreeNode(column.Name)
			{
				Tag = column
			};
			imageKey = (val2.SelectedImageKey = "DataColumn");
			val2.ImageKey = imageKey;
			if (column is StiCalcDataColumn)
			{
				imageKey = (val2.SelectedImageKey = "CalcColumn");
				val2.ImageKey = imageKey;
			}
			if (obj == column)
			{
				((TreeView)tvColumns).SelectedNode = val2;
			}
			val.Nodes.Add(val2);
		}
		if (((TreeView)tvColumns).SelectedNode == null)
		{
			((TreeView)tvColumns).SelectedNode = val;
		}
		((TreeView)tvColumns).ExpandAll();
	}

	private static object GetNextNodeTag(TreeNode node)
	{
		if (node.Parent == null)
		{
			return node.Tag;
		}
		int num = node.Parent.Nodes.IndexOf(node);
		if (num < node.Parent.Nodes.Count - 1)
		{
			node = node.Parent.Nodes[num + 1];
		}
		else if (num > 0)
		{
			node = node.Parent.Nodes[num - 1];
		}
		return node.Tag;
	}

	private static TreeNode GetNodeByTag(TreeNodeCollection nodes, object tag)
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
		return GetNodeByTag(((TreeView)tvColumns).Nodes, tag);
	}

	public void SelectNodeByTag(object tag)
	{
		((TreeView)tvColumns).SelectedNode = GetNodeByTag(tag);
		TreeNode selectedNode = ((TreeView)tvColumns).SelectedNode;
		if (selectedNode != null)
		{
			selectedNode.EnsureVisible();
		}
	}

	private void SaveParam(ref StiBusinessObject businessObject)
	{
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		if (businessObject == null)
		{
			businessObject = StiActivator.CreateObject(this.businessObject.GetType()) as StiBusinessObject;
		}
		StiDataColumnsCollection stiDataColumnsCollection = new StiDataColumnsCollection();
		stiDataColumnsCollection.AddRange(businessObject.Columns);
		businessObject.Columns.Clear();
		foreach (TreeNode node in ((TreeView)tvColumns).Nodes[0].Nodes)
		{
			TreeNode val = node;
			StiDataColumn stiDataColumn = val.Tag as StiDataColumn;
			StiDataColumn stiDataColumn2 = businessObject.Columns[stiDataColumn.Name];
			if (stiDataColumn2 == null)
			{
				businessObject.Columns.Add(stiDataColumn);
			}
			else
			{
				businessObject.Columns.Add(stiDataColumn2);
			}
		}
		businessObject.Category = ((Control)textBoxCategory).Text;
		businessObject.Name = ((Control)tbName).Text;
		businessObject.Alias = ((Control)tbAlias).Text;
	}

	private void NodeSetCheck(TreeNode node, bool check)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		foreach (TreeNode node2 in node.Nodes)
		{
			TreeNode val = node2;
			val.Checked = check;
			NodeSetCheck(val, check);
		}
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
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_0409: Unknown result type (might be due to invalid IL or missing references)
		//IL_0413: Expected O, but got Unknown
		//IL_0568: Unknown result type (might be due to invalid IL or missing references)
		//IL_0572: Expected O, but got Unknown
		//IL_063e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0738: Unknown result type (might be due to invalid IL or missing references)
		//IL_0742: Expected O, but got Unknown
		//IL_074f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0759: Expected O, but got Unknown
		//IL_0766: Unknown result type (might be due to invalid IL or missing references)
		//IL_0770: Expected O, but got Unknown
		//IL_0841: Unknown result type (might be due to invalid IL or missing references)
		//IL_1232: Unknown result type (might be due to invalid IL or missing references)
		//IL_123c: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiBusinessObjectEditForm));
		lbAlias = new StiLabelUX();
		tbAlias = new StiTextBox();
		lbName = new StiLabelUX();
		tbName = new StiTextBox();
		errorProvider = new ErrorProvider(components);
		toolTip1 = new ToolTip(components);
		pnColumns = new StiPanelUX();
		pgBusinessObject = new StiReportPropertyGrid();
		gbColumns = new StiOffice2013GroupBox();
		tvColumns = new StiTreeViewUX();
		tbMain = new StiToolBar();
		tbGetColumnsFromAssembly = new StiToolButton();
		tbRetrieveColumns = new StiToolButton();
		stiToolButton1 = new StiToolButton();
		tbDelete = new StiToolButton();
		tbCalcColumnNew = new StiToolButton();
		tbColumnNew = new StiToolButton();
		pnMain = new StiPanelUX();
		labelCategory = new StiLabelUX();
		textBoxCategory = new StiTextBox();
		btOk = new StiButtonUX();
		btCancel = new StiButtonUX();
		stiGroupLine1 = new StiButtonPanelUX();
		DotNetBarManager = new DotNetBarManager(components);
		DockSiteBottom = new DockSite();
		DockSiteLeft = new DockSite();
		DockSiteRight = new DockSite();
		DockSiteTop = new DockSite();
		ContextMenuBar = new ContextMenuBar();
		menuColumns = new ButtonItem();
		ciColumnNew = new ButtonItem();
		ciCalcColumnNew = new ButtonItem();
		ciDelete = new ButtonItem();
		ciRetrieveColumns = new ButtonItem();
		((ISupportInitialize)errorProvider).BeginInit();
		((Control)pnColumns).SuspendLayout();
		((Control)gbColumns).SuspendLayout();
		((Control)tbMain).SuspendLayout();
		((Control)pnMain).SuspendLayout();
		((Control)stiGroupLine1).SuspendLayout();
		((ISupportInitialize)ContextMenuBar).BeginInit();
		((Control)this).SuspendLayout();
		((Control)lbAlias).ForeColor = Color.DimGray;
		((Control)lbAlias).Location = new Point(12, 76);
		((Control)lbAlias).Name = "lbAlias";
		((Control)lbAlias).Size = new Size(129, 20);
		((Control)lbAlias).TabIndex = 10;
		((Control)lbAlias).Text = "Alias";
		((Label)lbAlias).TextAlign = (ContentAlignment)16;
		((Control)tbAlias).Anchor = (AnchorStyles)13;
		((Control)tbAlias).BackColor = SystemColors.Window;
		((Control)tbAlias).Location = new Point(147, 76);
		((Control)tbAlias).Name = "tbAlias";
		((Control)tbAlias).Size = new Size(224, 21);
		((Control)tbAlias).TabIndex = 2;
		((Control)lbName).ForeColor = Color.DimGray;
		((Control)lbName).Location = new Point(12, 44);
		((Control)lbName).Name = "lbName";
		((Control)lbName).Size = new Size(129, 20);
		((Control)lbName).TabIndex = 9;
		((Control)lbName).Text = "Name";
		((Label)lbName).TextAlign = (ContentAlignment)16;
		((Control)tbName).Anchor = (AnchorStyles)13;
		((Control)tbName).BackColor = SystemColors.Window;
		((Control)tbName).Location = new Point(147, 44);
		((Control)tbName).Name = "tbName";
		((Control)tbName).Size = new Size(224, 21);
		((Control)tbName).TabIndex = 1;
		((Control)tbName).Enter += TextBoxName_Enter;
		((Control)tbName).Leave += TextBoxName_Leave;
		errorProvider.ContainerControl = (ContainerControl)(object)this;
		errorProvider.DataMember = "";
		errorProvider.Icon = (Icon)componentResourceManager.GetObject("errorProvider.Icon");
		((Control)pnColumns).Controls.Add((Control)(object)pgBusinessObject);
		((Control)pnColumns).Controls.Add((Control)(object)gbColumns);
		((Control)pnColumns).Controls.Add((Control)(object)tbMain);
		((Control)pnColumns).Dock = (DockStyle)5;
		((Control)pnColumns).Location = new Point(0, 0);
		((Control)pnColumns).Name = "pnColumns";
		((Control)pnColumns).Size = new Size(496, 400);
		((Control)pnColumns).TabIndex = 5;
		((Control)pgBusinessObject).Anchor = (AnchorStyles)15;
		((PropertyGrid)pgBusinessObject).CategoryForeColor = SystemColors.InactiveCaptionText;
		((PropertyGrid)pgBusinessObject).CommandsForeColor = SystemColors.ActiveCaption;
		((PropertyGrid)pgBusinessObject).HelpBackColor = Color.White;
		((PropertyGrid)pgBusinessObject).LineColor = SystemColors.ScrollBar;
		((Control)pgBusinessObject).Location = new Point(217, 44);
		((Control)pgBusinessObject).Name = "pgBusinessObject";
		pgBusinessObject.ScrollOffset = 0;
		pgBusinessObject.ShowEventsTag = true;
		((Control)pgBusinessObject).Size = new Size(266, 341);
		((Control)pgBusinessObject).TabIndex = 1;
		((PropertyGrid)pgBusinessObject).PropertyValueChanged += new PropertyValueChangedEventHandler(PropertyGridDataSource_PropertyValueChanged);
		((Control)gbColumns).Anchor = (AnchorStyles)7;
		((Control)gbColumns).Controls.Add((Control)(object)ContextMenuBar);
		((Control)gbColumns).Controls.Add((Control)(object)tvColumns);
		((Control)gbColumns).Controls.Add((Control)(object)DockSiteTop);
		((Control)gbColumns).Controls.Add((Control)(object)DockSiteBottom);
		((Control)gbColumns).Controls.Add((Control)(object)DockSiteLeft);
		((Control)gbColumns).Controls.Add((Control)(object)DockSiteRight);
		gbColumns.HeaderHeight = 24;
		((Control)gbColumns).Location = new Point(12, 45);
		((Control)gbColumns).Name = "gbColumns";
		((Control)gbColumns).Padding = new Padding(0, 25, 0, 0);
		((Control)gbColumns).Size = new Size(192, 338);
		((Control)gbColumns).TabIndex = 1;
		((Control)gbColumns).TabStop = false;
		((Control)gbColumns).Text = "Columns";
		((Control)tvColumns).Anchor = (AnchorStyles)15;
		((TreeView)tvColumns).BorderStyle = (BorderStyle)0;
		ContextMenuBar.SetContextMenuEx((Control)(object)tvColumns, menuColumns);
		((Control)tvColumns).ForeColor = SystemColors.ControlText;
		((TreeView)tvColumns).HideSelection = false;
		((Control)tvColumns).Location = new Point(0, 38);
		((Control)tvColumns).Name = "tvColumns";
		((TreeView)tvColumns).ShowRootLines = false;
		((Control)tvColumns).Size = new Size(189, 300);
		((Control)tvColumns).TabIndex = 0;
		((TreeView)tvColumns).BeforeCheck += new TreeViewCancelEventHandler(TreeViewColumns_BeforeCheck);
		((TreeView)tvColumns).AfterSelect += new TreeViewEventHandler(TreeViewColumns_AfterSelect);
		((Control)tvColumns).KeyDown += new KeyEventHandler(TreeViewColumns_KeyDown);
		((Control)tbMain).Controls.Add((Control)(object)tbGetColumnsFromAssembly);
		((Control)tbMain).Controls.Add((Control)(object)tbRetrieveColumns);
		((Control)tbMain).Controls.Add((Control)(object)stiToolButton1);
		((Control)tbMain).Controls.Add((Control)(object)tbDelete);
		((Control)tbMain).Controls.Add((Control)(object)tbCalcColumnNew);
		((Control)tbMain).Controls.Add((Control)(object)tbColumnNew);
		tbMain.ControlStyle = StiControlStyle.Office2010;
		((Control)tbMain).Dock = (DockStyle)1;
		tbMain.LineStyle = StiToolBar.StiToolBarLineStyle.TopBottom;
		((Control)tbMain).Location = new Point(0, 0);
		((Control)tbMain).Name = "tbMain";
		((Control)tbMain).Padding = new Padding(3);
		((Control)tbMain).Size = new Size(496, 32);
		((Control)tbMain).TabIndex = 0;
		((Control)tbGetColumnsFromAssembly).AutoSize = true;
		((Control)tbGetColumnsFromAssembly).Dock = (DockStyle)3;
		tbGetColumnsFromAssembly.Draw3DButton = false;
		tbGetColumnsFromAssembly.HotDragDrop = false;
		((Control)tbGetColumnsFromAssembly).Location = new Point(192, 3);
		((Control)tbGetColumnsFromAssembly).Name = "tbGetColumnsFromAssembly";
		((Control)tbGetColumnsFromAssembly).Size = new Size(157, 26);
		((Control)tbGetColumnsFromAssembly).TabIndex = 12;
		((Control)tbGetColumnsFromAssembly).Text = "Get Columns from Assembly";
		((ButtonBase)tbGetColumnsFromAssembly).UseVisualStyleBackColor = false;
		((Control)tbGetColumnsFromAssembly).Click += ButtonGetColumnsFromAssembly_Click;
		((Control)tbRetrieveColumns).AutoSize = true;
		((Control)tbRetrieveColumns).Dock = (DockStyle)3;
		tbRetrieveColumns.Draw3DButton = false;
		tbRetrieveColumns.HotDragDrop = false;
		((Control)tbRetrieveColumns).Location = new Point(89, 3);
		((Control)tbRetrieveColumns).Name = "tbRetrieveColumns";
		((Control)tbRetrieveColumns).Size = new Size(103, 26);
		((Control)tbRetrieveColumns).TabIndex = 7;
		((Control)tbRetrieveColumns).Text = "Retrieve Columns";
		((ButtonBase)tbRetrieveColumns).UseVisualStyleBackColor = false;
		((Control)tbRetrieveColumns).Click += ButtonRetrieveColumns_Click;
		((Control)stiToolButton1).Dock = (DockStyle)3;
		stiToolButton1.Draw3DButton = false;
		stiToolButton1.HotDragDrop = false;
		((Control)stiToolButton1).Location = new Point(81, 3);
		((Control)stiToolButton1).Name = "stiToolButton1";
		((Control)stiToolButton1).Size = new Size(8, 26);
		stiToolButton1.Style = StiToolBarButtonStyle.Separator;
		((Control)stiToolButton1).TabIndex = 9;
		((ButtonBase)stiToolButton1).UseVisualStyleBackColor = false;
		((Control)tbDelete).Dock = (DockStyle)3;
		tbDelete.Draw3DButton = false;
		tbDelete.HotDragDrop = false;
		((ButtonBase)tbDelete).ImageIndex = 8;
		((Control)tbDelete).Location = new Point(55, 3);
		((Control)tbDelete).Name = "tbDelete";
		((Control)tbDelete).Size = new Size(26, 26);
		((Control)tbDelete).TabIndex = 5;
		((ButtonBase)tbDelete).UseVisualStyleBackColor = false;
		((Control)tbDelete).Click += ButtonDelete_Click;
		((Control)tbCalcColumnNew).Dock = (DockStyle)3;
		tbCalcColumnNew.Draw3DButton = false;
		tbCalcColumnNew.HotDragDrop = false;
		((ButtonBase)tbCalcColumnNew).ImageIndex = 10;
		((Control)tbCalcColumnNew).Location = new Point(29, 3);
		((Control)tbCalcColumnNew).Name = "tbCalcColumnNew";
		((Control)tbCalcColumnNew).Size = new Size(26, 26);
		((Control)tbCalcColumnNew).TabIndex = 10;
		((ButtonBase)tbCalcColumnNew).UseVisualStyleBackColor = false;
		((Control)tbCalcColumnNew).Click += ButtonCalcColumnNew_Click;
		((Control)tbColumnNew).Dock = (DockStyle)3;
		tbColumnNew.Draw3DButton = false;
		tbColumnNew.HotDragDrop = false;
		((ButtonBase)tbColumnNew).ImageIndex = 7;
		((Control)tbColumnNew).Location = new Point(3, 3);
		((Control)tbColumnNew).Name = "tbColumnNew";
		((Control)tbColumnNew).Size = new Size(26, 26);
		((Control)tbColumnNew).TabIndex = 4;
		((ButtonBase)tbColumnNew).UseVisualStyleBackColor = false;
		((Control)tbColumnNew).Click += ButtonColumnNew_Click;
		((Control)pnMain).Anchor = (AnchorStyles)15;
		((Control)pnMain).Controls.Add((Control)(object)pnColumns);
		((Control)pnMain).Location = new Point(0, 115);
		((Control)pnMain).Name = "pnMain";
		((Control)pnMain).Size = new Size(496, 400);
		((Control)pnMain).TabIndex = 3;
		((Control)labelCategory).ForeColor = Color.DimGray;
		((Control)labelCategory).Location = new Point(12, 12);
		((Control)labelCategory).Name = "labelCategory";
		((Control)labelCategory).Size = new Size(129, 20);
		((Control)labelCategory).TabIndex = 9;
		((Control)labelCategory).Text = "Category";
		((Label)labelCategory).TextAlign = (ContentAlignment)16;
		((Control)textBoxCategory).Anchor = (AnchorStyles)13;
		((Control)textBoxCategory).BackColor = SystemColors.Window;
		((Control)textBoxCategory).Location = new Point(147, 12);
		((Control)textBoxCategory).Name = "textBoxCategory";
		((Control)textBoxCategory).Size = new Size(224, 21);
		((Control)textBoxCategory).TabIndex = 0;
		((Control)textBoxCategory).Enter += TextBoxName_Enter;
		((Control)textBoxCategory).Leave += TextBoxName_Leave;
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(328, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 4;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += This_OkClick;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		((Button)btCancel).DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(409, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 5;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btCancel).Click += This_CancelClick;
		((Control)stiGroupLine1).BackColor = Color.White;
		((Control)stiGroupLine1).Controls.Add((Control)(object)btCancel);
		((Control)stiGroupLine1).Controls.Add((Control)(object)btOk);
		((Control)stiGroupLine1).Dock = (DockStyle)2;
		stiGroupLine1.LinePosition = StiLinePosition.Top;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 517);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(496, 46);
		((Control)stiGroupLine1).TabIndex = 11;
		DotNetBarManager.AutoDispatchShortcuts.Add(eShortcut.F1);
		DotNetBarManager.AutoDispatchShortcuts.Add(eShortcut.CtrlC);
		DotNetBarManager.AutoDispatchShortcuts.Add(eShortcut.CtrlA);
		DotNetBarManager.AutoDispatchShortcuts.Add(eShortcut.CtrlV);
		DotNetBarManager.AutoDispatchShortcuts.Add(eShortcut.CtrlX);
		DotNetBarManager.AutoDispatchShortcuts.Add(eShortcut.CtrlZ);
		DotNetBarManager.AutoDispatchShortcuts.Add(eShortcut.CtrlY);
		DotNetBarManager.AutoDispatchShortcuts.Add(eShortcut.Del);
		DotNetBarManager.AutoDispatchShortcuts.Add(eShortcut.Ins);
		DotNetBarManager.BottomDockSite = DockSiteBottom;
		DotNetBarManager.EnableFullSizeDock = false;
		DotNetBarManager.LeftDockSite = DockSiteLeft;
		DotNetBarManager.ParentForm = null;
		DotNetBarManager.RightDockSite = DockSiteRight;
		DotNetBarManager.Style = eDotNetBarStyle.StyleManagerControlled;
		DotNetBarManager.TopDockSite = DockSiteTop;
		((Control)DockSiteBottom).AccessibleRole = (AccessibleRole)9;
		((Control)DockSiteBottom).Dock = (DockStyle)2;
		((Control)DockSiteBottom).Location = new Point(0, 338);
		((Control)DockSiteBottom).Name = "DockSiteBottom";
		((Control)DockSiteBottom).Size = new Size(192, 0);
		((Control)DockSiteBottom).TabIndex = 27;
		((Control)DockSiteBottom).TabStop = false;
		((Control)DockSiteLeft).AccessibleRole = (AccessibleRole)9;
		((Control)DockSiteLeft).Dock = (DockStyle)3;
		((Control)DockSiteLeft).Location = new Point(0, 25);
		((Control)DockSiteLeft).Name = "DockSiteLeft";
		((Control)DockSiteLeft).Size = new Size(0, 313);
		((Control)DockSiteLeft).TabIndex = 25;
		((Control)DockSiteLeft).TabStop = false;
		((Control)DockSiteRight).AccessibleRole = (AccessibleRole)9;
		((Control)DockSiteRight).Dock = (DockStyle)4;
		((Control)DockSiteRight).Location = new Point(192, 25);
		((Control)DockSiteRight).Name = "DockSiteRight";
		((Control)DockSiteRight).Size = new Size(0, 313);
		((Control)DockSiteRight).TabIndex = 26;
		((Control)DockSiteRight).TabStop = false;
		((Control)DockSiteTop).AccessibleRole = (AccessibleRole)9;
		((Control)DockSiteTop).Dock = (DockStyle)1;
		((Control)DockSiteTop).Location = new Point(0, 25);
		((Control)DockSiteTop).Name = "DockSiteTop";
		((Control)DockSiteTop).Size = new Size(192, 0);
		((Control)DockSiteTop).TabIndex = 28;
		((Control)DockSiteTop).TabStop = false;
		ContextMenuBar.DockTabStripHeight = 50;
		((Control)ContextMenuBar).Font = new Font("Segoe UI", 9f);
		ContextMenuBar.Items.AddRange(new BaseItem[1] { menuColumns });
		ContextMenuBar.Location = new Point(3, 38);
		ContextMenuBar.Name = "ContextMenuBar";
		ContextMenuBar.Size = new Size(145, 31);
		ContextMenuBar.Stretch = true;
		ContextMenuBar.Style = eDotNetBarStyle.Office2007;
		ContextMenuBar.TabIndex = 24;
		ContextMenuBar.TabStop = false;
		((Control)ContextMenuBar).Text = "ContextMenuBar";
		menuColumns.AutoExpandOnClick = true;
		menuColumns.ImagePaddingHorizontal = 16;
		menuColumns.ImagePaddingVertical = 12;
		menuColumns.Name = "menuColumns";
		menuColumns.Shortcuts.Add(eShortcut.CtrlTab);
		menuColumns.SubItems.AddRange(new BaseItem[4] { ciColumnNew, ciCalcColumnNew, ciDelete, ciRetrieveColumns });
		menuColumns.Text = "menuColumns";
		ciColumnNew.ImagePaddingHorizontal = 16;
		ciColumnNew.ImagePaddingVertical = 12;
		ciColumnNew.Name = "ciColumnNew";
		ciColumnNew.Text = "Column New";
		ciColumnNew.Click += ButtonColumnNew_Click;
		ciCalcColumnNew.ImagePaddingHorizontal = 16;
		ciCalcColumnNew.ImagePaddingVertical = 12;
		ciCalcColumnNew.Name = "ciCalcColumnNew";
		ciCalcColumnNew.Text = "Calc Column New";
		ciCalcColumnNew.Click += ButtonCalcColumnNew_Click;
		ciDelete.BeginGroup = true;
		ciDelete.Name = "ciDelete";
		ciDelete.Text = "Delete";
		ciDelete.Click += ButtonDelete_Click;
		ciRetrieveColumns.BeginGroup = true;
		ciRetrieveColumns.Name = "ciRetrieveColumns";
		ciRetrieveColumns.Text = "Retrieve Columns";
		ciRetrieveColumns.Click += ButtonRetrieveColumns_Click;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(496, 563);
		((Control)this).Controls.Add((Control)(object)stiGroupLine1);
		((Control)this).Controls.Add((Control)(object)textBoxCategory);
		((Control)this).Controls.Add((Control)(object)tbName);
		((Control)this).Controls.Add((Control)(object)tbAlias);
		((Control)this).Controls.Add((Control)(object)pnMain);
		((Control)this).Controls.Add((Control)(object)labelCategory);
		((Control)this).Controls.Add((Control)(object)lbAlias);
		((Control)this).Controls.Add((Control)(object)lbName);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiBusinessObjectEditForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Edit Business Object";
		((Form)this).Closed += This_Closed;
		((Form)this).Load += This_Load;
		((ISupportInitialize)errorProvider).EndInit();
		((Control)pnColumns).ResumeLayout(false);
		((Control)gbColumns).ResumeLayout(false);
		((Control)tbMain).ResumeLayout(false);
		((Control)tbMain).PerformLayout();
		((Control)pnMain).ResumeLayout(false);
		((Control)stiGroupLine1).ResumeLayout(false);
		((ISupportInitialize)ContextMenuBar).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
