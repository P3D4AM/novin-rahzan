using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Images;

namespace Stimulsoft.Report.Dictionary.Design;

[ToolboxItem(false)]
public class StiDataControl : UserControl
{
	private IContainer components;

	private ImageList ilData;

	public StiTreeView tvData;

	public ContextMenuBar ContextMenuBar;

	private ButtonItem menuContext;

	private ButtonItem miExpandAll;

	private ButtonItem miCollapseAll;

	public DotNetBarManager DotNetBarManager;

	public DockSite DockSiteBottom;

	public DockSite DockSiteLeft;

	public DockSite DockSiteRight;

	public DockSite DockSiteTop;

	public StiDataControl()
	{
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			DoScaling();
			Localize();
		}
	}

	public void Localize()
	{
		miExpandAll.Text = StiLocalization.Get("Report", "ExpandAll");
		miCollapseAll.Text = StiLocalization.Get("Report", "CollapseAll");
	}

	private void DoScaling()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		ImageList val = new ImageList
		{
			ImageSize = new Size(StiScale.I16, StiScale.I16),
			ColorDepth = (ColorDepth)32
		};
		val.Images.Add(StiReportImages.Dictionary.Database());
		val.Images.Add(StiReportImages.Dictionary.DataSource());
		((TreeView)tvData).ImageList = val;
		((TreeView)tvData).ItemHeight = (int)Math.Ceiling(16.0 * StiScale.Y);
		miExpandAll.Image = StiReportDesignImages.Actions.ExpandAll();
		miCollapseAll.Image = StiReportDesignImages.Actions.CollapseAll();
	}

	public void Build(StiDataCollection dataStore)
	{
		Build(dataStore, null);
	}

	private void MenuItemCollapseAll_Click(object sender, EventArgs e)
	{
		((TreeView)tvData).CollapseAll();
	}

	private void MenuItemExpandAll_Click(object sender, EventArgs e)
	{
		((TreeView)tvData).ExpandAll();
	}

	private void TreeViewData_BeforeCheck(object sender, TreeViewCancelEventArgs e)
	{
		NodeSetCheck(e.Node, !e.Node.Checked);
	}

	private static void NodeSetCheck(TreeNode node, bool check)
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

	public void Build(StiDataCollection dataStore, StiDataAdapterService dataAdapter)
	{
		((TreeView)tvData).Nodes.Clear();
		if (dataAdapter == null)
		{
			foreach (StiDataAdapterService item in StiOptions.Services.DataAdapters.Where((StiDataAdapterService s) => s.ServiceEnabled))
			{
				try
				{
					TreeNode dataAdapterCategories = StiDataAdapterService.GetDataAdapterCategories(item, dataStore);
					TreeNode[] array = (TreeNode[])(object)new TreeNode[dataAdapterCategories.Nodes.Count];
					dataAdapterCategories.Nodes.CopyTo((Array)array, 0);
					((TreeView)tvData).Nodes.AddRange(array);
				}
				catch
				{
				}
			}
			return;
		}
		try
		{
			TreeNode dataAdapterCategories2 = StiDataAdapterService.GetDataAdapterCategories(dataAdapter, dataStore);
			TreeNode[] array2 = (TreeNode[])(object)new TreeNode[dataAdapterCategories2.Nodes.Count];
			dataAdapterCategories2.Nodes.CopyTo((Array)array2, 0);
			((TreeView)tvData).Nodes.AddRange(array2);
		}
		catch
		{
		}
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
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		//IL_0495: Unknown result type (might be due to invalid IL or missing references)
		//IL_049f: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiDataControl));
		ilData = new ImageList(components);
		tvData = new StiTreeView();
		DotNetBarManager = new DotNetBarManager(components);
		DockSiteBottom = new DockSite();
		DockSiteLeft = new DockSite();
		DockSiteRight = new DockSite();
		DockSiteTop = new DockSite();
		ContextMenuBar = new ContextMenuBar();
		menuContext = new ButtonItem();
		miExpandAll = new ButtonItem();
		miCollapseAll = new ButtonItem();
		((ISupportInitialize)ContextMenuBar).BeginInit();
		((Control)this).SuspendLayout();
		ilData.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("ilData.ImageStream");
		ilData.TransparentColor = Color.Magenta;
		ilData.Images.SetKeyName(0, "");
		ilData.Images.SetKeyName(1, "");
		ContextMenuBar.SetContextMenuEx((Control)(object)tvData, menuContext);
		((Control)tvData).Dock = (DockStyle)5;
		((TreeView)tvData).HideSelection = false;
		((TreeView)tvData).ItemHeight = 16;
		((Control)tvData).Location = new Point(0, 0);
		((Control)tvData).Name = "tvData";
		((Control)tvData).Size = new Size(248, 280);
		((Control)tvData).TabIndex = 0;
		((TreeView)tvData).BeforeCheck += new TreeViewCancelEventHandler(TreeViewData_BeforeCheck);
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
		((Control)DockSiteBottom).Location = new Point(0, 280);
		((Control)DockSiteBottom).Name = "DockSiteBottom";
		((Control)DockSiteBottom).Size = new Size(248, 0);
		((Control)DockSiteBottom).TabIndex = 32;
		((Control)DockSiteBottom).TabStop = false;
		((Control)DockSiteLeft).AccessibleRole = (AccessibleRole)9;
		((Control)DockSiteLeft).Dock = (DockStyle)3;
		((Control)DockSiteLeft).Location = new Point(0, 0);
		((Control)DockSiteLeft).Name = "DockSiteLeft";
		((Control)DockSiteLeft).Size = new Size(0, 280);
		((Control)DockSiteLeft).TabIndex = 30;
		((Control)DockSiteLeft).TabStop = false;
		((Control)DockSiteRight).AccessibleRole = (AccessibleRole)9;
		((Control)DockSiteRight).Dock = (DockStyle)4;
		((Control)DockSiteRight).Location = new Point(248, 0);
		((Control)DockSiteRight).Name = "DockSiteRight";
		((Control)DockSiteRight).Size = new Size(0, 280);
		((Control)DockSiteRight).TabIndex = 31;
		((Control)DockSiteRight).TabStop = false;
		((Control)DockSiteTop).AccessibleRole = (AccessibleRole)9;
		((Control)DockSiteTop).Dock = (DockStyle)1;
		((Control)DockSiteTop).Location = new Point(0, 0);
		((Control)DockSiteTop).Name = "DockSiteTop";
		((Control)DockSiteTop).Size = new Size(248, 0);
		((Control)DockSiteTop).TabIndex = 33;
		((Control)DockSiteTop).TabStop = false;
		ContextMenuBar.DockTabStripHeight = 50;
		((Control)ContextMenuBar).Font = new Font("Segoe UI", 9f);
		ContextMenuBar.Items.AddRange(new BaseItem[1] { menuContext });
		ContextMenuBar.Location = new Point(3, 38);
		ContextMenuBar.Name = "ContextMenuBar";
		ContextMenuBar.Size = new Size(145, 31);
		ContextMenuBar.Stretch = true;
		ContextMenuBar.Style = eDotNetBarStyle.Office2007;
		ContextMenuBar.TabIndex = 29;
		ContextMenuBar.TabStop = false;
		((Control)ContextMenuBar).Text = "ContextMenuBar";
		menuContext.AutoExpandOnClick = true;
		menuContext.ImagePaddingHorizontal = 16;
		menuContext.ImagePaddingVertical = 12;
		menuContext.Name = "menuContext";
		menuContext.Shortcuts.Add(eShortcut.CtrlTab);
		menuContext.SubItems.AddRange(new BaseItem[2] { miExpandAll, miCollapseAll });
		menuContext.Text = "menuContext";
		miExpandAll.ImagePaddingHorizontal = 16;
		miExpandAll.ImagePaddingVertical = 12;
		miExpandAll.Name = "miExpandAll";
		miExpandAll.Text = "Expand All";
		miExpandAll.Click += MenuItemExpandAll_Click;
		miCollapseAll.ImagePaddingHorizontal = 16;
		miCollapseAll.ImagePaddingVertical = 12;
		miCollapseAll.Name = "miCollapseAll";
		miCollapseAll.Text = "Collapse All";
		miCollapseAll.Click += MenuItemCollapseAll_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)ContextMenuBar);
		((Control)this).Controls.Add((Control)(object)tvData);
		((Control)this).Controls.Add((Control)(object)DockSiteTop);
		((Control)this).Controls.Add((Control)(object)DockSiteBottom);
		((Control)this).Controls.Add((Control)(object)DockSiteLeft);
		((Control)this).Controls.Add((Control)(object)DockSiteRight);
		((Control)this).Name = "StiDataControl";
		((Control)this).Size = new Size(248, 280);
		((ISupportInitialize)ContextMenuBar).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
