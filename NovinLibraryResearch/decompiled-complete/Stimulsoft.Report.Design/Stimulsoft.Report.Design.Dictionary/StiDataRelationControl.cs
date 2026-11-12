using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Design.Dictionary;

[ToolboxItem(false)]
public class StiDataRelationControl : UserControl
{
	private IContainer components;

	public StiTreeView tvRelations;

	private ImageList ilRelation;

	public StiDataRelationControl()
	{
		InitializeComponent();
	}

	public void Build(StiDataRelationsCollection relations, bool checkBoxes)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		((TreeView)tvRelations).Nodes.Clear();
		foreach (StiDataRelation relation in relations)
		{
			TreeNode val = new TreeNode(relation.ToString());
			val.Tag = relation;
			((TreeView)tvRelations).Nodes.Add(val);
		}
		((TreeView)tvRelations).CheckBoxes = checkBoxes;
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
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiDataRelationControl));
		tvRelations = new StiTreeView();
		ilRelation = new ImageList(components);
		((Control)this).SuspendLayout();
		((TreeView)tvRelations).CheckBoxes = true;
		((Control)tvRelations).Dock = (DockStyle)5;
		((TreeView)tvRelations).ImageIndex = 0;
		((TreeView)tvRelations).ImageList = ilRelation;
		((Control)tvRelations).Location = new Point(0, 0);
		((Control)tvRelations).Name = "tvRelations";
		((TreeView)tvRelations).SelectedImageIndex = 0;
		((TreeView)tvRelations).ShowPlusMinus = false;
		((TreeView)tvRelations).ShowRootLines = false;
		((Control)tvRelations).Size = new Size(288, 312);
		((Control)tvRelations).TabIndex = 6;
		ilRelation.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("ilRelation.ImageStream");
		ilRelation.TransparentColor = Color.Magenta;
		ilRelation.Images.SetKeyName(0, "");
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)tvRelations);
		((Control)this).Name = "StiDataRelationControl";
		((Control)this).Size = new Size(288, 312);
		((Control)this).ResumeLayout(false);
	}
}
