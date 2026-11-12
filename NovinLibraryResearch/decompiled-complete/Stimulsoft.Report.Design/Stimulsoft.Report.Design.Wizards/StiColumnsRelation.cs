using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Design.Wizards;

[ToolboxItem(false)]
public class StiColumnsRelation : UserControl
{
	private StiDataSource dataSource;

	private IContainer components;

	public TreeView tvRelations;

	private ImageList ilRelations;

	[Browsable(false)]
	public StiDataSource DataSource
	{
		get
		{
			return dataSource;
		}
		set
		{
			if (dataSource != value)
			{
				dataSource = value;
				BuildRelations();
			}
		}
	}

	[Browsable(false)]
	public StiDataSource MasterDataSource { get; set; }

	public StiColumnsRelation()
	{
		InitializeComponent();
	}

	private void BuildRelations()
	{
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Expected O, but got Unknown
		StiDataRelationsCollection parentRelations = DataSource.GetParentRelations();
		StiDataRelationsCollection stiDataRelationsCollection = new StiDataRelationsCollection(null);
		foreach (StiDataRelation item in parentRelations)
		{
			if (item.ParentSource == MasterDataSource)
			{
				stiDataRelationsCollection.Add(item);
			}
		}
		tvRelations.Nodes.Clear();
		tvRelations.Nodes.Add(new TreeNode("[" + StiLocalization.Get("Report", "NotAssigned") + "]", 0, 0));
		tvRelations.SelectedNode = tvRelations.Nodes[0];
		foreach (StiDataRelation item2 in stiDataRelationsCollection)
		{
			tvRelations.Nodes.Add(new TreeNode(item2.ToString(), 1, 1)
			{
				Tag = item2
			});
		}
		tvRelations.ExpandAll();
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
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiColumnsRelation));
		tvRelations = new TreeView();
		ilRelations = new ImageList(components);
		((Control)this).SuspendLayout();
		tvRelations.BorderStyle = (BorderStyle)0;
		((Control)tvRelations).Dock = (DockStyle)5;
		tvRelations.HideSelection = false;
		tvRelations.ImageIndex = 0;
		tvRelations.ImageList = ilRelations;
		((Control)tvRelations).Location = new Point(0, 0);
		((Control)tvRelations).Name = "tvRelations";
		tvRelations.SelectedImageIndex = 0;
		((Control)tvRelations).Size = new Size(384, 296);
		((Control)tvRelations).TabIndex = 4;
		ilRelations.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("ilRelations.ImageStream");
		ilRelations.TransparentColor = Color.Magenta;
		ilRelations.Images.SetKeyName(0, "");
		ilRelations.Images.SetKeyName(1, "");
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)tvRelations);
		((Control)this).Name = "StiColumnsRelation";
		((Control)this).Size = new Size(384, 296);
		((Control)this).ResumeLayout(false);
	}
}
