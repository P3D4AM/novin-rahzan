using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Design.Dictionary;

public class StiDialogInfoGetItemsFromColumnForm : StiFormUX
{
	private StiReport report;

	private StiDataBuilder dataBuilder = new StiDataBuilder();

	private IContainer components;

	private StiButtonPanelUX glButtons;

	private StiButtonUX buttonOk;

	private StiButtonUX buttonCancel;

	private StiLabelUX labelLabels;

	public StiTreeViewBox columnKeys;

	private StiLabelUX labelKeys;

	public StiTreeViewBox columnLabels;

	private string NoColumn => "[" + Loc.Get("Report", "No") + "]";

	private TreeNode SelectNode(TreeNode node, string[] strs, int index)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		TreeNode val = null;
		foreach (TreeNode node2 in node.Nodes)
		{
			TreeNode val2 = node2;
			if (StiNameValidator.CorrectName(val2.Text, report) == strs[index])
			{
				val = val2;
				break;
			}
		}
		if (val == null)
		{
			return node;
		}
		if (!val.IsExpanded)
		{
			val.Expand();
		}
		return SelectNode(val, strs, ++index);
	}

	private void Localize()
	{
		((Control)this).Text = Loc.Get("Wizards", "SelectColumns");
		((Control)labelKeys).Text = Loc.GetMain("Keys");
		((Control)labelLabels).Text = Loc.GetMain("Labels");
		((Control)buttonOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)buttonCancel).Text = StiLocalization.Get("Buttons", "Cancel");
	}

	private void TreeNode_Expand(object sender, TreeViewCancelEventArgs e)
	{
		if (e.Node is TreeNodeEx node && e.Node.Tag != null)
		{
			dataBuilder.ExpandedNode(node, includeCalcColumn: true);
		}
	}

	private void Column_TreeNodesFill(object sender, StiTreeNodesFillEventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		TreeNode val = new TreeNode(NoColumn);
		string imageKey = (val.SelectedImageKey = "Close");
		val.ImageKey = imageKey;
		((TreeView)e.TreeView).ImageList = StiDataBuilder.Images;
		e.Nodes.Add(val);
		((TreeView)e.TreeView).BeforeExpand += new TreeViewCancelEventHandler(TreeNode_Expand);
		TreeNodeEx dataNode = new TreeNodeEx();
		dataBuilder.BuildDataSource(null, ref dataNode, includeCalcColumn: true);
		dataBuilder.Build(e.Nodes, report.Dictionary, dictionaryDesign: true, includeCalcColumn: true);
		if (dataNode != null && ((TreeNode)dataNode).Nodes.Count > 0)
		{
			e.Nodes.Add((TreeNode)(object)dataNode);
			StiTreeViewBox stiTreeViewBox = sender as StiTreeViewBox;
			if (((Control)stiTreeViewBox).Text == NoColumn)
			{
				((TreeView)e.TreeView).SelectedNode = e.Nodes[0];
				return;
			}
			string[] strs = ((Control)stiTreeViewBox).Text.Split(new char[1] { '.' });
			((TreeView)e.TreeView).SelectedNode = SelectNode(e.Nodes[1], strs, 0);
		}
	}

	private void Column_ValueChanged(object sender, StiValueChangedEventArgs e)
	{
		if (!(e.Text != NoColumn))
		{
			return;
		}
		TreeNode selectedNode = ((TreeView)e.TreeView).SelectedNode;
		if (selectedNode != null)
		{
			if (!(selectedNode.Tag is StiDataColumn))
			{
				e.Text = NoColumn;
			}
			else
			{
				e.Text = StiDataBuilder.GetColumnPathFromNode(selectedNode);
			}
		}
		else
		{
			e.Text = NoColumn;
		}
	}

	private void This_OkClick(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)1;
	}

	private void This_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			((Form)this).Close();
		}
	}

	public StiDialogInfoGetItemsFromColumnForm(StiReport report)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		this.report = report;
		Localize();
		((Form)this).ShowIcon = false;
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
		//IL_0598: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a2: Expected O, but got Unknown
		glButtons = new StiButtonPanelUX();
		buttonOk = new StiButtonUX();
		buttonCancel = new StiButtonUX();
		labelLabels = new StiLabelUX();
		columnKeys = new StiTreeViewBox();
		labelKeys = new StiLabelUX();
		columnLabels = new StiTreeViewBox();
		((Control)glButtons).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)glButtons).BackColor = Color.White;
		((Control)glButtons).Controls.Add((Control)(object)buttonOk);
		((Control)glButtons).Controls.Add((Control)(object)buttonCancel);
		((Control)glButtons).Dock = (DockStyle)2;
		glButtons.LinePosition = StiLinePosition.Top;
		glButtons.LineStyle = StiLineStyle.Dot;
		((Control)glButtons).Location = new Point(0, 79);
		((Control)glButtons).Name = "glButtons";
		((Control)glButtons).Size = new Size(341, 46);
		((Control)glButtons).TabIndex = 2;
		((Control)buttonOk).Anchor = (AnchorStyles)10;
		((Control)buttonOk).Location = new Point(173, 12);
		((Control)buttonOk).Name = "buttonOk";
		((Control)buttonOk).Size = new Size(75, 24);
		((Control)buttonOk).TabIndex = 0;
		((Control)buttonOk).Text = "Ok";
		((ButtonBase)buttonOk).UseVisualStyleBackColor = true;
		((Control)buttonOk).Click += This_OkClick;
		((Control)buttonCancel).Anchor = (AnchorStyles)10;
		((Button)buttonCancel).DialogResult = (DialogResult)2;
		((Control)buttonCancel).Location = new Point(254, 12);
		((Control)buttonCancel).Name = "buttonCancel";
		((Control)buttonCancel).Size = new Size(75, 24);
		((Control)buttonCancel).TabIndex = 1;
		((Control)buttonCancel).Text = "Cancel";
		((ButtonBase)buttonCancel).UseVisualStyleBackColor = true;
		((Control)labelLabels).ForeColor = Color.DimGray;
		((Control)labelLabels).Location = new Point(12, 45);
		((Control)labelLabels).Name = "labelLabels";
		((Control)labelLabels).Size = new Size(119, 21);
		((Control)labelLabels).TabIndex = 4;
		((Control)labelLabels).Text = "Labels";
		((Label)labelLabels).TextAlign = (ContentAlignment)16;
		((Label)labelLabels).UseCompatibleTextRendering = true;
		((Control)columnKeys).Anchor = (AnchorStyles)13;
		((Control)columnKeys).BackColor = SystemColors.Window;
		columnKeys.DropDownWidth = 400;
		columnKeys.Flat = false;
		((Control)columnKeys).ForeColor = SystemColors.ControlText;
		((Control)columnKeys).Location = new Point(138, 12);
		columnKeys.MaxDropDownHeight = 300;
		((Control)columnKeys).Name = "columnKeys";
		((Control)columnKeys).Size = new Size(191, 21);
		((Control)columnKeys).TabIndex = 0;
		columnKeys.ValueChanged += Column_ValueChanged;
		columnKeys.TreeNodesFill += Column_TreeNodesFill;
		((Control)labelKeys).ForeColor = Color.DimGray;
		((Control)labelKeys).Location = new Point(12, 12);
		((Control)labelKeys).Name = "labelKeys";
		((Control)labelKeys).Size = new Size(118, 17);
		((Control)labelKeys).TabIndex = 3;
		((Control)labelKeys).Text = "Keys";
		((Label)labelKeys).TextAlign = (ContentAlignment)16;
		((Label)labelKeys).UseCompatibleTextRendering = true;
		((Control)columnLabels).Anchor = (AnchorStyles)13;
		((Control)columnLabels).BackColor = SystemColors.Window;
		columnLabels.DropDownWidth = 400;
		columnLabels.Flat = false;
		((Control)columnLabels).ForeColor = SystemColors.ControlText;
		((Control)columnLabels).Location = new Point(138, 45);
		columnLabels.MaxDropDownHeight = 300;
		((Control)columnLabels).Name = "columnLabels";
		((Control)columnLabels).Size = new Size(191, 21);
		((Control)columnLabels).TabIndex = 1;
		columnLabels.ValueChanged += Column_ValueChanged;
		columnLabels.TreeNodesFill += Column_TreeNodesFill;
		((Form)this).AcceptButton = (IButtonControl)(object)buttonOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)buttonCancel;
		((Form)this).ClientSize = new Size(341, 125);
		((Control)this).Controls.Add((Control)(object)labelLabels);
		((Control)this).Controls.Add((Control)(object)columnKeys);
		((Control)this).Controls.Add((Control)(object)labelKeys);
		((Control)this).Controls.Add((Control)(object)columnLabels);
		((Control)this).Controls.Add((Control)(object)glButtons);
		((Form)this).FormBorderStyle = (FormBorderStyle)1;
		((Form)this).KeyPreview = true;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiDialogInfoGetItemsFromColumnForm";
		((Form)this).ShowIcon = false;
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Columns";
		((Control)this).KeyDown += new KeyEventHandler(This_KeyDown);
		((Control)glButtons).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
