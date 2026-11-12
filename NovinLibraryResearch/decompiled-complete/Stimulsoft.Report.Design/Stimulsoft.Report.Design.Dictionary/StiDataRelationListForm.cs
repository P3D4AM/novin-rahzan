using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Design.Dictionary;

public class StiDataRelationListForm : StiFormUX
{
	public Hashtable categories;

	private IContainer components;

	private StiButtonUX btOk;

	private StiButtonUX btCancel;

	public StiTreeView tvRelations;

	private ImageList ilRelations;

	private StiButtonPanelUX stiGroupLine1;

	public StiDataRelationListForm(Hashtable categories, StiDataRelationsCollection relations)
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Expected O, but got Unknown
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (((Component)(object)this).DesignMode)
		{
			return;
		}
		Localize();
		this.categories = categories;
		foreach (StiDataRelation relation in relations)
		{
			TreeNode val = null;
			string text = categories[relation] as string;
			foreach (TreeNode node in ((TreeView)tvRelations).Nodes)
			{
				TreeNode val2 = node;
				if (val2.Text == text)
				{
					val = val2;
					break;
				}
			}
			if (val == null)
			{
				val = new TreeNode(text, 0, 0);
				((TreeView)tvRelations).Nodes.Add(val);
			}
			TreeNode val3 = new TreeNode(relation.NameInSource, 1, 1);
			val3.Tag = relation;
			val.Nodes.Add(val3);
		}
		if (relations.Count > 0)
		{
			((TreeView)tvRelations).ExpandAll();
			((TreeView)tvRelations).SelectedNode = ((TreeView)tvRelations).Nodes[0];
		}
		StiSettings.Load();
		StiFormSettings.Load((Form)(object)this);
		((Form)this).ShowIcon = false;
	}

	private void Localize()
	{
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
		((Control)this).Text = StiLocalization.Get("FormDictionaryDesigner", "ImportRelations");
	}

	private void btOk_Click(object sender, EventArgs e)
	{
		((Form)this).DialogResult = (DialogResult)1;
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

	private void tvRelations_BeforeCheck(object sender, TreeViewCancelEventArgs e)
	{
		NodeSetCheck(e.Node, !e.Node.Checked);
	}

	private void StiDataRelationListForm_Closed(object sender, EventArgs e)
	{
		StiFormSettings.Save((Form)(object)this);
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
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Expected O, but got Unknown
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiDataRelationListForm));
		btOk = new StiButtonUX();
		btCancel = new StiButtonUX();
		tvRelations = new StiTreeView();
		ilRelations = new ImageList(components);
		stiGroupLine1 = new StiButtonPanelUX();
		((Control)stiGroupLine1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(106, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 1;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += btOk_Click;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		((Button)btCancel).DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(187, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 2;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		((Control)tvRelations).Anchor = (AnchorStyles)15;
		((Control)tvRelations).BackColor = Color.White;
		((TreeView)tvRelations).BorderStyle = (BorderStyle)0;
		((TreeView)tvRelations).CheckBoxes = true;
		((TreeView)tvRelations).ImageIndex = 0;
		((TreeView)tvRelations).ImageList = ilRelations;
		((Control)tvRelations).Location = new Point(12, 12);
		((Control)tvRelations).Name = "tvRelations";
		((TreeView)tvRelations).SelectedImageIndex = 0;
		((Control)tvRelations).Size = new Size(250, 349);
		((Control)tvRelations).TabIndex = 0;
		((TreeView)tvRelations).BeforeCheck += new TreeViewCancelEventHandler(tvRelations_BeforeCheck);
		ilRelations.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("ilRelations.ImageStream");
		ilRelations.TransparentColor = Color.Magenta;
		ilRelations.Images.SetKeyName(0, "");
		ilRelations.Images.SetKeyName(1, "");
		((Control)stiGroupLine1).BackColor = Color.White;
		((Control)stiGroupLine1).Controls.Add((Control)(object)btCancel);
		((Control)stiGroupLine1).Controls.Add((Control)(object)btOk);
		((Control)stiGroupLine1).Dock = (DockStyle)2;
		stiGroupLine1.LinePosition = StiLinePosition.Top;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 379);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(274, 46);
		((Control)stiGroupLine1).TabIndex = 3;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(274, 425);
		((Control)this).Controls.Add((Control)(object)stiGroupLine1);
		((Control)this).Controls.Add((Control)(object)tvRelations);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiDataRelationListForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Select relations";
		((Form)this).Closed += StiDataRelationListForm_Closed;
		((Control)stiGroupLine1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
