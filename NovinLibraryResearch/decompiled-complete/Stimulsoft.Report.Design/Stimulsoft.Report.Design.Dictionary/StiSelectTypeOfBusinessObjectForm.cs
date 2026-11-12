using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Design.Dictionary;

public class StiSelectTypeOfBusinessObjectForm : StiFormUX
{
	private StiBusinessObject parentBusinessObject;

	private StiReport report;

	private IContainer components;

	private StiButtonUX btOk;

	private StiButtonUX btCancel;

	private StiOffice2013GroupBox groupBoxChildOfBusinessObject;

	private StiOffice2013GroupBox groupBoxNewBusinessObject;

	private ErrorProvider errorProvider;

	private StiButtonPanelUX glButtons;

	public StiRadioButtonUX radioButtonChildOfBusinessObject;

	public StiRadioButtonUX radioButtonNewBusinessObject;

	private StiTreeViewUX treeViewBusinessObjects;

	private StiTreeViewUX treeViewBusinessObjectsStore;

	private StiPanelUX panel2;

	public StiSelectTypeOfBusinessObjectForm(StiReport report, StiBusinessObject parentBusinessObject)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		this.parentBusinessObject = parentBusinessObject;
		this.report = report;
		if (!((Component)(object)this).DesignMode)
		{
			Localize();
		}
		StiSettings.Load();
		StiFormSettings.Load((Form)(object)this);
		((TreeView)treeViewBusinessObjectsStore).ImageList = StiDataBuilder.Images;
		((TreeView)treeViewBusinessObjects).ImageList = StiDataBuilder.Images;
		((Control)treeViewBusinessObjectsStore).Enabled = report.BusinessObjectsStore.Count != 0;
		((Control)radioButtonChildOfBusinessObject).Enabled = parentBusinessObject != null;
		((Control)treeViewBusinessObjects).Enabled = parentBusinessObject != null;
		((RadioButton)radioButtonChildOfBusinessObject).Checked = parentBusinessObject != null;
		((RadioButton)radioButtonNewBusinessObject).Checked = !((RadioButton)radioButtonChildOfBusinessObject).Checked;
		if (((RadioButton)radioButtonChildOfBusinessObject).Checked)
		{
			((Control)treeViewBusinessObjects).Focus();
		}
		if (((RadioButton)radioButtonNewBusinessObject).Checked)
		{
			((Control)treeViewBusinessObjectsStore).Focus();
		}
		BuildBusinessObjectsStore();
		BuildChildBusinessObjects();
		CheckState();
		DoScaling();
	}

	private void DoScaling()
	{
		errorProvider.Icon = StiIconUtils.ResizeIcon(errorProvider.Icon, StiScale.I16, StiScale.I16);
	}

	public StiDataColumnsCollection GetColumns()
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		StiDataColumnsCollection stiDataColumnsCollection = new StiDataColumnsCollection();
		if (((TreeView)treeViewBusinessObjects).Nodes.Count > 0)
		{
			foreach (TreeNode node in ((TreeView)treeViewBusinessObjects).Nodes[0].Nodes)
			{
				TreeNode val = node;
				if (val.Checked)
				{
					stiDataColumnsCollection.Add(val.Tag as StiDataColumn);
				}
			}
		}
		return stiDataColumnsCollection;
	}

	public List<StiBusinessObjectData> GetBusinessObjectsData()
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		List<StiBusinessObjectData> list = new List<StiBusinessObjectData>();
		if (((TreeView)treeViewBusinessObjects).Nodes.Count > 0)
		{
			foreach (TreeNode node in ((TreeView)treeViewBusinessObjectsStore).Nodes[0].Nodes)
			{
				TreeNode val = node;
				if (val.Checked)
				{
					list.Add(val.Tag as StiBusinessObjectData);
				}
			}
		}
		return list;
	}

	private void BuildBusinessObjectsStore()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		TreeNode val = new TreeNode(StiLocalization.Get("Report", "BusinessObjects"));
		string imageKey = (val.SelectedImageKey = "BusinessObject");
		val.ImageKey = imageKey;
		((TreeView)treeViewBusinessObjectsStore).Nodes.Add(val);
		foreach (StiBusinessObjectData item in report.BusinessObjectsStore)
		{
			TreeNode val2 = new TreeNode(item.Name)
			{
				Tag = item,
				ImageKey = "BusinessObject",
				SelectedImageKey = "BusinessObject"
			};
			val.Nodes.Add(val2);
		}
		val.ExpandAll();
	}

	private void BuildChildBusinessObjects()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		TreeNode val = new TreeNode(StiLocalization.Get("Report", "BusinessObjects"))
		{
			ImageKey = "BusinessObject",
			SelectedImageKey = "BusinessObject"
		};
		((TreeView)treeViewBusinessObjects).Nodes.Add(val);
		if (parentBusinessObject != null)
		{
			object businessObjectData = parentBusinessObject.GetBusinessObjectData();
			if (businessObjectData != null)
			{
				StiDataColumnsCollection columnsFromData = StiBusinessObjectHelper.GetColumnsFromData(businessObjectData, includeChildDataSources: true);
				foreach (StiDataColumn item in columnsFromData)
				{
					if (!StiBusinessObjectHelper.IsDataColumn(item.Type))
					{
						TreeNode val2 = new TreeNode(item.Name)
						{
							Tag = item,
							ImageKey = "BusinessObject",
							SelectedImageKey = "BusinessObject"
						};
						val.Nodes.Add(val2);
					}
				}
			}
		}
		val.ExpandAll();
	}

	private void CheckState()
	{
		((Control)treeViewBusinessObjects).Enabled = ((RadioButton)radioButtonChildOfBusinessObject).Checked;
		((Control)treeViewBusinessObjectsStore).Enabled = ((RadioButton)radioButtonNewBusinessObject).Checked;
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("FormDictionaryDesigner", "NewBusinessObject");
		((Control)radioButtonNewBusinessObject).Text = StiLocalization.Get("FormDictionaryDesigner", "NewBusinessObject");
		((Control)radioButtonChildOfBusinessObject).Text = StiLocalization.Get("FormDictionaryDesigner", "ChildOfBusinessObject");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
	}

	private void btOk_Click(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)1;
	}

	private void btCancel_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void radioButtonNewBusinessObject_CheckedChanged(object sender, EventArgs e)
	{
		CheckState();
	}

	private void treeViewBusinessObjects_AfterCheck(object sender, TreeViewEventArgs e)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		foreach (TreeNode node in e.Node.Nodes)
		{
			TreeNode val = node;
			val.Checked = e.Node.Checked;
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
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Expected O, but got Unknown
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Expected O, but got Unknown
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c6: Expected O, but got Unknown
		//IL_0503: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiSelectTypeOfBusinessObjectForm));
		errorProvider = new ErrorProvider(components);
		panel2 = new StiPanelUX();
		groupBoxChildOfBusinessObject = new StiOffice2013GroupBox();
		treeViewBusinessObjects = new StiTreeViewUX();
		radioButtonChildOfBusinessObject = new StiRadioButtonUX();
		groupBoxNewBusinessObject = new StiOffice2013GroupBox();
		treeViewBusinessObjectsStore = new StiTreeViewUX();
		radioButtonNewBusinessObject = new StiRadioButtonUX();
		btOk = new StiButtonUX();
		btCancel = new StiButtonUX();
		glButtons = new StiButtonPanelUX();
		((ISupportInitialize)errorProvider).BeginInit();
		((Control)panel2).SuspendLayout();
		((Control)groupBoxChildOfBusinessObject).SuspendLayout();
		((Control)groupBoxNewBusinessObject).SuspendLayout();
		((Control)glButtons).SuspendLayout();
		((Control)this).SuspendLayout();
		errorProvider.ContainerControl = (ContainerControl)(object)this;
		errorProvider.DataMember = "";
		errorProvider.Icon = (Icon)componentResourceManager.GetObject("errorProvider.Icon");
		((Panel)panel2).AutoSizeMode = (AutoSizeMode)0;
		((Control)panel2).Controls.Add((Control)(object)groupBoxChildOfBusinessObject);
		((Control)panel2).Controls.Add((Control)(object)groupBoxNewBusinessObject);
		((Control)panel2).Dock = (DockStyle)1;
		((Control)panel2).Location = new Point(0, 0);
		((Control)panel2).Name = "panel2";
		((Control)panel2).Size = new Size(354, 454);
		((Control)panel2).TabIndex = 7;
		((Control)groupBoxChildOfBusinessObject).Controls.Add((Control)(object)treeViewBusinessObjects);
		((Control)groupBoxChildOfBusinessObject).Controls.Add((Control)(object)radioButtonChildOfBusinessObject);
		((Control)groupBoxChildOfBusinessObject).Dock = (DockStyle)1;
		groupBoxChildOfBusinessObject.HeaderHeight = 26;
		((Control)groupBoxChildOfBusinessObject).Location = new Point(0, 239);
		((Control)groupBoxChildOfBusinessObject).Name = "groupBoxChildOfBusinessObject";
		((Control)groupBoxChildOfBusinessObject).Padding = new Padding(2, 28, 2, 0);
		((Control)groupBoxChildOfBusinessObject).Size = new Size(354, 229);
		((Control)groupBoxChildOfBusinessObject).TabIndex = 4;
		((TreeView)treeViewBusinessObjects).BorderStyle = (BorderStyle)0;
		((TreeView)treeViewBusinessObjects).CheckBoxes = true;
		((TreeView)treeViewBusinessObjects).HideSelection = false;
		((Control)treeViewBusinessObjects).Location = new Point(12, 40);
		((Control)treeViewBusinessObjects).Name = "treeViewBusinessObjects";
		((TreeView)treeViewBusinessObjects).ShowRootLines = false;
		((Control)treeViewBusinessObjects).Size = new Size(330, 180);
		((Control)treeViewBusinessObjects).TabIndex = 2;
		((TreeView)treeViewBusinessObjects).AfterCheck += new TreeViewEventHandler(treeViewBusinessObjects_AfterCheck);
		((Control)radioButtonChildOfBusinessObject).BackColor = Color.Transparent;
		((RadioButton)radioButtonChildOfBusinessObject).Checked = true;
		((Control)radioButtonChildOfBusinessObject).Location = new Point(2, 2);
		((Control)radioButtonChildOfBusinessObject).Name = "radioButtonChildOfBusinessObject";
		((Control)radioButtonChildOfBusinessObject).Padding = new Padding(10, 2, 2, 2);
		((Control)radioButtonChildOfBusinessObject).Size = new Size(316, 21);
		((Control)radioButtonChildOfBusinessObject).TabIndex = 1;
		((RadioButton)radioButtonChildOfBusinessObject).TabStop = true;
		((Control)radioButtonChildOfBusinessObject).Text = "Child of Business Object";
		((ButtonBase)radioButtonChildOfBusinessObject).UseVisualStyleBackColor = false;
		((RadioButton)radioButtonChildOfBusinessObject).CheckedChanged += radioButtonNewBusinessObject_CheckedChanged;
		((Control)groupBoxNewBusinessObject).Controls.Add((Control)(object)treeViewBusinessObjectsStore);
		((Control)groupBoxNewBusinessObject).Controls.Add((Control)(object)radioButtonNewBusinessObject);
		((Control)groupBoxNewBusinessObject).Dock = (DockStyle)1;
		groupBoxNewBusinessObject.HeaderHeight = 26;
		((Control)groupBoxNewBusinessObject).Location = new Point(0, 0);
		((Control)groupBoxNewBusinessObject).Name = "groupBoxNewBusinessObject";
		((Control)groupBoxNewBusinessObject).Padding = new Padding(2, 28, 2, 0);
		((Control)groupBoxNewBusinessObject).Size = new Size(354, 239);
		((Control)groupBoxNewBusinessObject).TabIndex = 4;
		((Control)treeViewBusinessObjectsStore).Anchor = (AnchorStyles)15;
		((TreeView)treeViewBusinessObjectsStore).BorderStyle = (BorderStyle)0;
		((TreeView)treeViewBusinessObjectsStore).CheckBoxes = true;
		((TreeView)treeViewBusinessObjectsStore).HideSelection = false;
		((Control)treeViewBusinessObjectsStore).Location = new Point(12, 40);
		((Control)treeViewBusinessObjectsStore).Name = "treeViewBusinessObjectsStore";
		((TreeView)treeViewBusinessObjectsStore).ShowRootLines = false;
		((Control)treeViewBusinessObjectsStore).Size = new Size(330, 184);
		((Control)treeViewBusinessObjectsStore).TabIndex = 3;
		((TreeView)treeViewBusinessObjectsStore).AfterCheck += new TreeViewEventHandler(treeViewBusinessObjects_AfterCheck);
		((Control)radioButtonNewBusinessObject).BackColor = Color.Transparent;
		((Control)radioButtonNewBusinessObject).Location = new Point(2, 2);
		((Control)radioButtonNewBusinessObject).Name = "radioButtonNewBusinessObject";
		((Control)radioButtonNewBusinessObject).Padding = new Padding(10, 2, 2, 2);
		((Control)radioButtonNewBusinessObject).Size = new Size(316, 21);
		((Control)radioButtonNewBusinessObject).TabIndex = 0;
		((Control)radioButtonNewBusinessObject).Text = "New Business Object";
		((ButtonBase)radioButtonNewBusinessObject).UseVisualStyleBackColor = false;
		((RadioButton)radioButtonNewBusinessObject).CheckedChanged += radioButtonNewBusinessObject_CheckedChanged;
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(184, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 0;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += btOk_Click;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		((Button)btCancel).DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(265, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 1;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btCancel).Click += btCancel_Click;
		((Control)glButtons).Controls.Add((Control)(object)btCancel);
		((Control)glButtons).Controls.Add((Control)(object)btOk);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = StiLinePosition.Top;
		glButtons.LineStyle = StiLineStyle.Dot;
		((Control)glButtons).Location = new Point(0, 474);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(354, 46);
		((Control)glButtons).TabIndex = 3;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(354, 520);
		((Control)this).Controls.Add((Control)(object)panel2);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).MinimumSize = new Size(346, 39);
		((Control)this).Name = "StiSelectTypeOfBusinessObjectForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Select type";
		((ISupportInitialize)errorProvider).EndInit();
		((Control)panel2).ResumeLayout(false);
		((Control)groupBoxChildOfBusinessObject).ResumeLayout(false);
		((Control)groupBoxNewBusinessObject).ResumeLayout(false);
		((Control)glButtons).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
