using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Images;

namespace Stimulsoft.Report.Design.Dictionary;

public class StiRetrieveColumnsForm : StiFormUX
{
	private StiButtonUX btOk;

	private StiButtonUX btCancel;

	private StiButtonPanelUX glButtons;

	private StiTreeViewUX tvColumns;

	private ImageList ilButtons;

	private IContainer components;

	private StiBusinessObject businessObject;

	public StiRetrieveColumnsForm(StiBusinessObject businessObject)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			Localize();
		}
		StiSettings.Load();
		StiFormSettings.Load((Form)(object)this);
		this.businessObject = businessObject;
		Build(businessObject);
		DoScaling();
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
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Expected O, but got Unknown
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiRetrieveColumnsForm));
		btOk = new StiButtonUX();
		btCancel = new StiButtonUX();
		glButtons = new StiButtonPanelUX();
		tvColumns = new StiTreeViewUX();
		ilButtons = new ImageList(components);
		((Control)glButtons).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(186, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 0;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += This_OkClick;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		((Button)btCancel).DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(267, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 1;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)btCancel).Click += This_CancelClick;
		((Control)glButtons).BackColor = Color.White;
		((Control)glButtons).Controls.Add((Control)(object)btCancel);
		((Control)glButtons).Controls.Add((Control)(object)btOk);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = StiLinePosition.Top;
		glButtons.LineStyle = StiLineStyle.Dot;
		((Control)glButtons).Location = new Point(0, 412);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(354, 46);
		((Control)glButtons).TabIndex = 1;
		((Control)tvColumns).Anchor = (AnchorStyles)15;
		((Control)tvColumns).BackColor = Color.White;
		((TreeView)tvColumns).BorderStyle = (BorderStyle)0;
		((TreeView)tvColumns).CheckBoxes = true;
		((TreeView)tvColumns).HideSelection = false;
		((TreeView)tvColumns).ImageIndex = 0;
		((TreeView)tvColumns).ImageList = ilButtons;
		((Control)tvColumns).Location = new Point(12, 12);
		((Control)tvColumns).Name = "tvColumns";
		((TreeView)tvColumns).SelectedImageIndex = 0;
		((TreeView)tvColumns).ShowRootLines = false;
		((Control)tvColumns).Size = new Size(330, 385);
		((Control)tvColumns).TabIndex = 0;
		((TreeView)tvColumns).AfterCheck += new TreeViewEventHandler(TreeViewColumns_AfterCheck);
		ilButtons.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("ilButtons.ImageStream");
		ilButtons.TransparentColor = Color.Magenta;
		ilButtons.Images.SetKeyName(0, "");
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(354, 458);
		((Control)this).Controls.Add((Control)(object)tvColumns);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiRetrieveColumnsForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Retrieve Columns";
		((Control)glButtons).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}

	private void DoScaling()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		if (StiScale.Factor != 1.0)
		{
			ImageList val = new ImageList();
			val.ImageSize = new Size(StiScale.I16, StiScale.I16);
			val.ColorDepth = (ColorDepth)32;
			val.Images.Add(StiReportImages.Dictionary.DataColumn());
			((TreeView)tvColumns).ImageList = val;
		}
	}

	private void Build(StiBusinessObject businessObject)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		object businessObjectData = businessObject.GetBusinessObjectData(isColumnsRetrieve: true);
		StiDataColumnsCollection stiDataColumnsCollection = ((businessObjectData != null) ? StiBusinessObjectHelper.GetColumnsFromData(businessObjectData, includeChildDataSources: true) : null);
		((TreeView)tvColumns).Nodes.Clear();
		TreeNode val = new TreeNode(StiLocalization.Get("PropertyMain", "Columns"), 0, 0);
		((TreeView)tvColumns).Nodes.Add(val);
		if (stiDataColumnsCollection != null)
		{
			foreach (StiDataColumn item in stiDataColumnsCollection)
			{
				bool flag = false;
				foreach (StiDataColumn column in businessObject.Columns)
				{
					if (column.NameInSource == item.NameInSource)
					{
						flag = true;
						break;
					}
				}
				TreeNode val2 = new TreeNode(item.Name, 0, 0);
				val2.Tag = item;
				val2.Checked = !flag;
				if (val2.Checked && !StiBusinessObjectHelper.IsDataColumn(item.Type))
				{
					val2.Checked = false;
				}
				val.Nodes.Add(val2);
			}
		}
		val.ExpandAll();
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("FormDictionaryDesigner", "RetrieveColumns");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		foreach (TreeNode node in ((TreeView)tvColumns).Nodes[0].Nodes)
		{
			TreeNode val = node;
			if (!val.Checked)
			{
				continue;
			}
			bool flag = false;
			StiDataColumn stiDataColumn = val.Tag as StiDataColumn;
			foreach (StiDataColumn column in businessObject.Columns)
			{
				if (!(column.NameInSource != stiDataColumn.NameInSource))
				{
					column.Name = stiDataColumn.Name;
					column.Alias = stiDataColumn.Alias;
					column.Type = stiDataColumn.Type;
					if ((column.Type == typeof(DateTime) || column.Type == typeof(DateTime?)) && StiOptions.Dictionary.UseNullableDateTime)
					{
						column.Type = typeof(DateTime?);
					}
					else if (column.Type == typeof(TimeSpan) || (column.Type == typeof(TimeSpan?) && StiOptions.Dictionary.UseNullableTimeSpan))
					{
						column.Type = typeof(TimeSpan?);
					}
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				businessObject.Columns.Add(stiDataColumn);
			}
		}
		((Form)this).DialogResult = (DialogResult)1;
	}

	private void This_CancelClick(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)2;
	}

	private void TreeViewColumns_AfterCheck(object sender, TreeViewEventArgs e)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		foreach (TreeNode node in e.Node.Nodes)
		{
			TreeNode val = node;
			val.Checked = e.Node.Checked;
		}
	}
}
