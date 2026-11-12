using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Design.Images;
using Stimulsoft.Report.Design.Properties;

namespace Stimulsoft.Report.Design;

public class StiServicesForm : Form
{
	private bool removeCategory;

	private StiDesignerControl designer;

	private string filename = "";

	private bool isModified;

	private IContainer components;

	private StiTreeView tvServices;

	private Bar stiToolBar1;

	private ButtonItem tbRemove;

	private ToolTip toolTip;

	private ButtonItem tbUp;

	private ButtonItem tbDown;

	private PictureBox pbBitmap;

	private Label lbAssembly;

	private Label lbVersion;

	private GroupBox gbService;

	private TextBox tbServiceInfo;

	public StiReportPropertyGrid PropertyGrid;

	private ExpandableSplitter splitter1;

	private ButtonItem tbOpen;

	private ButtonItem tbSave;

	private ButtonItem tbNew;

	private DotNetBarManager dotNetBarManager1;

	private DockSite dockSite4;

	private DockSite dockSite1;

	private DockSite dockSite2;

	private DockSite dockSite3;

	private DockSite dockSite5;

	private DockSite dockSite6;

	private DockSite dockSite7;

	private Bar bar1;

	private ButtonItem biFile;

	private DockSite dockSite8;

	private Panel panel1;

	private ButtonItem miNew;

	private ButtonItem miOpen;

	private ButtonItem miSave;

	private ButtonItem miSaveAs;

	private ButtonItem miExit;

	private ButtonItem buttonItem1;

	private ButtonItem miAddServices;

	private ButtonItem miRemove;

	private ButtonItem miSynchronize;

	private StiServiceContainer Services => StiConfig.Services;

	public StiServicesForm(StiDesignerControl designer)
		: this(designer, loadServices: false)
	{
	}

	public StiServicesForm(StiDesignerControl designer, bool loadServices)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			if (!loadServices)
			{
				StiConfig.Services.Clear();
			}
			else
			{
				StiConfig.Load();
			}
			this.designer = designer;
			if (!StiOptions.Appearance.ShowTooltipsHelp)
			{
				dotNetBarManager1.AutoDispatchShortcuts.Remove(eShortcut.F1);
			}
			BuildServices();
			PropertyGrid.SetServicesPropertyTabs();
			((PropertyGrid)PropertyGrid).HelpVisible = StiOptions.Designer.PropertyGrid.ShowDescription;
			if (StiOptions.Designer.AllowUseDragDrop)
			{
				((Control)tvServices).AllowDrop = true;
			}
			StiSettings.Load();
			StiFormSettings.Load((Form)(object)this);
			((Control)PropertyGrid).Width = StiSettings.GetInt("StiServicesForm", "Spl", ((Control)PropertyGrid).Width);
			((Form)this).ShowIcon = false;
			if (StiOptions.Designer.AllowUseDragDrop)
			{
				((Control)tvServices).AllowDrop = true;
			}
			DoScaling();
		}
	}

	private void DoScaling()
	{
		miNew.Image = StiReportDesignImages.Actions.NewFile();
		miOpen.Image = StiReportDesignImages.Actions.OpenFile();
		miSave.Image = StiReportDesignImages.Actions.SaveFile();
		miSynchronize.Image = StiReportDesignImages.Actions.Synchronize();
		tbNew.Image = StiReportDesignImages.Actions.NewFile();
		tbOpen.Image = StiReportDesignImages.Actions.OpenFile();
		tbSave.Image = StiReportDesignImages.Actions.SaveFile();
		tbRemove.Image = StiReportDesignImages.Actions.Remove();
		tbUp.Image = StiReportDesignImages.Actions.MoveUp();
		tbDown.Image = StiReportDesignImages.Actions.MoveDown();
	}

	private void BuildServices()
	{
		BuildServices(tvServices, Services);
	}

	internal void BuildServices(StiTreeView StiTreeView, StiServiceContainer serviceContainer)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Expected O, but got Unknown
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Expected O, but got Unknown
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Expected O, but got Unknown
		ImageList val = new ImageList
		{
			ColorDepth = (ColorDepth)32,
			TransparentColor = Color.Magenta
		};
		val.Images.Add(StiImageResizer.GetImage((Image)(object)StiImageUtils.GetImage("Stimulsoft.Base", "Stimulsoft.Base.Bmp.Service.bmp")));
		TreeNode val2 = new TreeNode("Services", 0, 0);
		val2.Expand();
		((TreeView)StiTreeView).Nodes.Clear();
		if (Services.Count == 0)
		{
			tvServices_AfterSelect(this, null);
			return;
		}
		((TreeView)StiTreeView).Nodes.Add(val2);
		((TreeView)StiTreeView).ImageList = val;
		int num = 1;
		foreach (StiService item in serviceContainer)
		{
			Image val3 = null;
			Image val4 = null;
			try
			{
				val3 = StiImageResizer.GetImage((Image)(object)StiServiceBitmapAttribute.GetImage(item.GetType()));
				val4 = StiImageResizer.GetImage((Image)(object)StiServiceCategoryBitmapAttribute.GetImage(item.GetType()));
			}
			catch
			{
			}
			if (val3 == null)
			{
				val3 = StiImageResizer.GetImage((Image)(object)StiServiceBitmapAttribute.GetImage(typeof(StiService)));
			}
			if (val4 == null)
			{
				val4 = StiImageResizer.GetImage((Image)(object)StiServiceBitmapAttribute.GetImage(typeof(StiService)));
			}
			val.Images.Add(val3);
			TreeNode val5 = new TreeNode(item.GetType().Name, num, num++)
			{
				Tag = item,
				Checked = !Services.Contains(item)
			};
			string text = StiHint.CreateHint(item.ServiceCategory);
			TreeNode val6 = null;
			foreach (TreeNode node in val2.Nodes)
			{
				TreeNode val7 = node;
				if (val7.Text == text)
				{
					val6 = val7;
					break;
				}
			}
			if (val6 == null)
			{
				val.Images.Add(val4);
				val6 = new TreeNode(text, num, num++);
				val2.Nodes.Add(val6);
			}
			val6.Nodes.Add(val5);
		}
		val2.EnsureVisible();
		miAddServices.Enabled = true;
		miRemove.Enabled = true;
	}

	private bool TestSave()
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Invalid comparison between Unknown and I4
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Invalid comparison between Unknown and I4
		if (isModified)
		{
			string text = filename;
			if (text.Length == 0)
			{
				text = "Configuration";
			}
			DialogResult val = MessageBox.Show(string.Format(StiLocalization.Get("Questions", "qnSaveChanges"), text), ((Control)this).Text, (MessageBoxButtons)3);
			if ((int)val == 2)
			{
				return true;
			}
			if ((int)val == 6)
			{
				tbSave_Click(this, EventArgs.Empty);
			}
		}
		return false;
	}

	private void tbRemove_Click(object sender, EventArgs e)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Invalid comparison between Unknown and I4
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Invalid comparison between Unknown and I4
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		if (removeCategory)
		{
			if (((TreeView)tvServices).SelectedNode == null || (int)MessageBox.Show(StiLocalization.Get("Questions", "qnRemoveServiceCategory"), "Services", (MessageBoxButtons)4) != 6)
			{
				return;
			}
			foreach (TreeNode node in ((TreeView)tvServices).SelectedNode.Nodes)
			{
				TreeNode val = node;
				Services.Remove((StiService)val.Tag);
			}
			((TreeView)tvServices).SelectedNode.Parent.Nodes.Remove(((TreeView)tvServices).SelectedNode);
			isModified = true;
		}
		else if (((TreeView)tvServices).SelectedNode != null && ((TreeView)tvServices).SelectedNode.Tag != null && (int)MessageBox.Show(StiLocalization.Get("Questions", "qnRemoveService"), "Services", (MessageBoxButtons)4) == 6)
		{
			Services.Remove((StiService)((TreeView)tvServices).SelectedNode.Tag);
			TreeNode parent = ((TreeView)tvServices).SelectedNode.Parent;
			parent.Nodes.Remove(((TreeView)tvServices).SelectedNode);
			if (parent.Nodes.Count == 0)
			{
				parent.Parent.Nodes.Remove(parent);
			}
			isModified = true;
		}
	}

	private void ciExpandAll_Click(object sender, EventArgs e)
	{
		((TreeView)tvServices).ExpandAll();
	}

	private void ciCollapseAll_Click(object sender, EventArgs e)
	{
		((TreeView)tvServices).CollapseAll();
	}

	private void tvServices_AfterSelect(object sender, TreeViewEventArgs e)
	{
		if (((TreeView)tvServices).SelectedNode == null)
		{
			tbRemove.Enabled = false;
			tbUp.Enabled = false;
			tbDown.Enabled = false;
			((Control)gbService).Text = "";
			pbBitmap.Image = null;
			((Control)lbAssembly).Text = "";
			((Control)lbVersion).Text = "";
			((PropertyGrid)PropertyGrid).SelectedObject = null;
			return;
		}
		((PropertyGrid)PropertyGrid).SelectedObject = ((TreeView)tvServices).SelectedNode.Tag;
		if (((TreeView)tvServices).SelectedNode.Tag != null)
		{
			tbRemove.Tooltip = StiHint.CreateHint("Remove Service");
			removeCategory = false;
			tbRemove.Visible = true;
			if (((TreeView)tvServices).SelectedNode.Index == 0)
			{
				tbUp.Enabled = false;
			}
			else
			{
				tbUp.Enabled = true;
			}
			if (((TreeView)tvServices).SelectedNode.Index == ((TreeView)tvServices).SelectedNode.Parent.Nodes.Count - 1)
			{
				tbDown.Enabled = false;
			}
			else
			{
				tbDown.Enabled = true;
			}
			StiService stiService = ((TreeView)tvServices).SelectedNode.Tag as StiService;
			Assembly assembly = stiService.GetType().Assembly;
			((Control)gbService).Text = stiService.GetType().Name;
			pbBitmap.Image = ((TreeView)tvServices).ImageList.Images[((TreeView)tvServices).SelectedNode.ImageIndex];
			((Control)lbAssembly).Text = StiLocalization.Get("Report", "nameAssembly") + " : " + assembly.GetName().Name + ".dll";
			((Control)lbVersion).Text = StiLocalization.Get("PropertyMain", "Version") + " : " + assembly.GetName().Version.ToString();
			((Control)tbServiceInfo).Text = stiService.ServiceInfo;
		}
		else
		{
			tbUp.Enabled = false;
			tbDown.Enabled = false;
			if (((TreeView)tvServices).SelectedNode.Parent == null)
			{
				tbRemove.Enabled = false;
			}
			else
			{
				tbRemove.Tooltip = StiHint.CreateHint("Remove Service Category");
				removeCategory = true;
				tbRemove.Enabled = true;
			}
			((Control)gbService).Text = "";
			pbBitmap.Image = null;
			((Control)lbAssembly).Text = "";
			((Control)lbVersion).Text = "";
		}
	}

	private void tbUp_Click(object sender, EventArgs e)
	{
		TreeNode parent = ((TreeView)tvServices).SelectedNode.Parent;
		int index = ((TreeView)tvServices).SelectedNode.Index;
		int index2 = Services.IndexOf(parent.Nodes[index].Tag as StiService);
		int index3 = Services.IndexOf(parent.Nodes[index - 1].Tag as StiService);
		StiService value = Services[index2];
		Services[index2] = Services[index3];
		Services[index3] = value;
		((TreeView)tvServices).BeginUpdate();
		TreeNode selectedNode = ((TreeView)tvServices).SelectedNode;
		parent.Nodes.RemoveAt(index);
		parent.Nodes.Insert(index - 1, selectedNode);
		((TreeView)tvServices).SelectedNode = selectedNode;
		((TreeView)tvServices).EndUpdate();
		isModified = true;
	}

	private void tbDown_Click(object sender, EventArgs e)
	{
		TreeNode parent = ((TreeView)tvServices).SelectedNode.Parent;
		int index = ((TreeView)tvServices).SelectedNode.Index;
		int index2 = Services.IndexOf(parent.Nodes[index].Tag as StiService);
		int index3 = Services.IndexOf(parent.Nodes[index + 1].Tag as StiService);
		StiService value = Services[index2];
		Services[index2] = Services[index3];
		Services[index3] = value;
		((TreeView)tvServices).BeginUpdate();
		TreeNode selectedNode = ((TreeView)tvServices).SelectedNode;
		parent.Nodes.RemoveAt(index);
		parent.Nodes.Insert(index + 1, selectedNode);
		((TreeView)tvServices).SelectedNode = selectedNode;
		((TreeView)tvServices).EndUpdate();
		isModified = true;
	}

	private void tvServices_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Invalid comparison between Unknown and I4
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 46)
		{
			tbRemove_Click(sender, (EventArgs)(object)e);
		}
		if (e.Shift && (int)e.KeyCode == 38 && tbUp.Enabled)
		{
			e.Handled = true;
			tbUp_Click(sender, (EventArgs)(object)e);
		}
		if (e.Shift && (int)e.KeyCode == 40 && tbDown.Enabled)
		{
			e.Handled = true;
			tbDown_Click(sender, (EventArgs)(object)e);
		}
	}

	private void tvServices_ItemDrag(object sender, ItemDragEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		if (e.Item is TreeNode && ((TreeNode)e.Item).Parent != null)
		{
			((Control)this).DoDragDrop(e.Item, (DragDropEffects)2);
		}
	}

	private void tvServices_DragEnter(object sender, DragEventArgs e)
	{
		e.Effect = (DragDropEffects)2;
	}

	private void tvServices_DragOver(object sender, DragEventArgs e)
	{
		object data = e.Data.GetData(typeof(TreeNode));
		TreeNode val = (TreeNode)((data is TreeNode) ? data : null);
		TreeNode nodeAt = ((TreeView)tvServices).GetNodeAt(((Control)tvServices).PointToClient(Cursor.Position));
		if (val.Parent == nodeAt.Parent)
		{
			((TreeView)tvServices).SelectedNode = nodeAt;
		}
	}

	private void tvServices_DragDrop(object sender, DragEventArgs e)
	{
		object data = e.Data.GetData(typeof(TreeNode));
		TreeNode val = (TreeNode)((data is TreeNode) ? data : null);
		TreeNode selectedNode = ((TreeView)tvServices).SelectedNode;
		if (val.Parent == selectedNode.Parent && val != selectedNode)
		{
			((TreeView)tvServices).BeginUpdate();
			int num = selectedNode.Parent.Nodes.IndexOf(selectedNode);
			int index = Services.IndexOf(selectedNode.Tag as StiService);
			Services.Remove(val.Tag as StiService);
			selectedNode.Parent.Nodes.Remove(val);
			selectedNode.Parent.Nodes.Insert(num, val);
			Services.Insert(index, val.Tag as StiService);
			((TreeView)tvServices).SelectedNode = val;
			((TreeView)tvServices).EndUpdate();
			isModified = true;
		}
	}

	private void tbSynchronize_Click(object sender, EventArgs e)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Invalid comparison between Unknown and I4
		if ((int)MessageBox.Show(StiLocalization.Get("Questions", "qnSynchronizeServices"), "Services", (MessageBoxButtons)4) == 6)
		{
			BuildServices();
			isModified = true;
		}
	}

	private void StiServicesForm_Closed(object sender, EventArgs e)
	{
		StiFormSettings.Save((Form)(object)this);
		StiSettings.Load();
		StiSettings.Set("StiServicesForm", "Spl", ((Control)PropertyGrid).Width);
		StiSettings.Save();
	}

	private void tbOpen_Click(object sender, EventArgs e)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Invalid comparison between Unknown and I4
		if (TestSave())
		{
			return;
		}
		OpenFileDialog val = new OpenFileDialog();
		try
		{
			((FileDialog)val).Filter = "Stimulsoft Reports Config files (*.Config)|*.Config";
			((FileDialog)val).Title = "Services";
			((FileDialog)val).RestoreDirectory = true;
			if ((int)((CommonDialog)val).ShowDialog() == 1 && ((FileDialog)val).FileName != "")
			{
				isModified = false;
				filename = ((FileDialog)val).FileName;
				StiConfig.Load(filename);
				BuildServices();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void tbSave_Click(object sender, EventArgs e)
	{
		if (filename.Length == 0)
		{
			StiConfig.Save();
		}
		else
		{
			StiConfig.Save(filename);
		}
		isModified = false;
	}

	private void miSaveAs_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Invalid comparison between Unknown and I4
		SaveFileDialog val = new SaveFileDialog();
		try
		{
			((FileDialog)val).Filter = "Stimulsoft Reports Config files (*.Config)|*.Config";
			((FileDialog)val).Title = "Services";
			((FileDialog)val).RestoreDirectory = true;
			((FileDialog)val).FileName = filename;
			((FileDialog)val).DefaultExt = "Config";
			if ((int)((CommonDialog)val).ShowDialog() == 1 && ((FileDialog)val).FileName != "")
			{
				isModified = false;
				filename = ((FileDialog)val).FileName;
				StiConfig.Save(filename);
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void miAddServices_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Invalid comparison between Unknown and I4
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Invalid comparison between Unknown and I4
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Expected O, but got Unknown
		OpenFileDialog val = new OpenFileDialog();
		try
		{
			((FileDialog)val).Filter = "StiServices files (*.dll,*.exe)|*.dll;*.exe";
			((FileDialog)val).Title = "Services";
			((FileDialog)val).RestoreDirectory = true;
			if ((int)((CommonDialog)val).ShowDialog() != 1 || !(((FileDialog)val).FileName != ""))
			{
				return;
			}
			bool flag = false;
			StiServiceContainer stiServiceContainer = null;
			try
			{
				stiServiceContainer = StiServiceContainer.GetServicesFromAssembly(((FileDialog)val).FileName);
			}
			catch (Exception ex)
			{
				StiLogService.Write(((object)this).GetType(), "GetServicesFromAssembly...ERROR");
				StiLogService.Write(((object)this).GetType(), ex.Message);
				flag = true;
			}
			if (stiServiceContainer == null || stiServiceContainer.Count == 0 || flag)
			{
				MessageBox.Show(string.Format(StiLocalization.Get("Errors", "NoServices"), ((FileDialog)val).FileName));
				return;
			}
			StiServiceSelectForm stiServiceSelectForm = new StiServiceSelectForm();
			BuildServices(stiServiceSelectForm.tvServices, stiServiceContainer);
			if ((int)((Form)stiServiceSelectForm).ShowDialog((IWin32Window)(object)this) != 1)
			{
				return;
			}
			TreeNode val2 = ((TreeView)stiServiceSelectForm.tvServices).Nodes[0];
			foreach (TreeNode node in val2.Nodes)
			{
				TreeNode val3 = node;
				foreach (TreeNode node2 in val3.Nodes)
				{
					TreeNode val4 = node2;
					if (val4.Checked)
					{
						Services.Add(val4.Tag as StiService);
					}
				}
			}
			BuildServices();
			((TreeView)tvServices).Nodes[0].Expand();
			isModified = true;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void miExit_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void tbNew_Click(object sender, EventArgs e)
	{
		if (!TestSave())
		{
			filename = "";
			Services.Clear();
			BuildServices();
		}
	}

	private void StiServicesForm_Closing(object sender, CancelEventArgs e)
	{
		e.Cancel = TestSave();
	}

	private void PropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
	{
		isModified = true;
	}

	private void btClose_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void StiServicesForm_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			e.Handled = true;
			((Form)this).Close();
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
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Expected O, but got Unknown
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Expected O, but got Unknown
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Expected O, but got Unknown
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Expected O, but got Unknown
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Expected O, but got Unknown
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Expected O, but got Unknown
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Expected O, but got Unknown
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Expected O, but got Unknown
		//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_045e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0468: Expected O, but got Unknown
		//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c4: Expected O, but got Unknown
		//IL_0522: Unknown result type (might be due to invalid IL or missing references)
		//IL_052c: Expected O, but got Unknown
		//IL_058a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0594: Expected O, but got Unknown
		//IL_05f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fc: Expected O, but got Unknown
		//IL_064e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0658: Expected O, but got Unknown
		//IL_0725: Unknown result type (might be due to invalid IL or missing references)
		//IL_0749: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0859: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_096e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ddb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0edd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fde: Unknown result type (might be due to invalid IL or missing references)
		//IL_1061: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1194: Unknown result type (might be due to invalid IL or missing references)
		//IL_119e: Expected O, but got Unknown
		//IL_11e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1665: Unknown result type (might be due to invalid IL or missing references)
		//IL_16f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1714: Unknown result type (might be due to invalid IL or missing references)
		//IL_179f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1805: Unknown result type (might be due to invalid IL or missing references)
		//IL_180f: Expected O, but got Unknown
		//IL_1934: Unknown result type (might be due to invalid IL or missing references)
		//IL_199c: Unknown result type (might be due to invalid IL or missing references)
		//IL_19a6: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiServicesForm));
		tvServices = new StiTreeView();
		stiToolBar1 = new Bar();
		tbNew = new ButtonItem();
		tbOpen = new ButtonItem();
		tbSave = new ButtonItem();
		tbRemove = new ButtonItem();
		tbUp = new ButtonItem();
		tbDown = new ButtonItem();
		toolTip = new ToolTip(components);
		gbService = new GroupBox();
		lbAssembly = new Label();
		lbVersion = new Label();
		pbBitmap = new PictureBox();
		tbServiceInfo = new TextBox();
		splitter1 = new ExpandableSplitter();
		dotNetBarManager1 = new DotNetBarManager(components);
		dockSite4 = new DockSite();
		dockSite1 = new DockSite();
		dockSite2 = new DockSite();
		dockSite8 = new DockSite();
		dockSite5 = new DockSite();
		dockSite6 = new DockSite();
		dockSite7 = new DockSite();
		bar1 = new Bar();
		biFile = new ButtonItem();
		miNew = new ButtonItem();
		miOpen = new ButtonItem();
		miSave = new ButtonItem();
		miSaveAs = new ButtonItem();
		miExit = new ButtonItem();
		buttonItem1 = new ButtonItem();
		miAddServices = new ButtonItem();
		miRemove = new ButtonItem();
		miSynchronize = new ButtonItem();
		dockSite3 = new DockSite();
		panel1 = new Panel();
		PropertyGrid = new StiReportPropertyGrid();
		((ISupportInitialize)stiToolBar1).BeginInit();
		((Control)gbService).SuspendLayout();
		((ISupportInitialize)pbBitmap).BeginInit();
		dockSite7.SuspendLayout();
		((ISupportInitialize)bar1).BeginInit();
		((Control)panel1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)tvServices).Dock = (DockStyle)5;
		((TreeView)tvServices).HideSelection = false;
		((Control)tvServices).Location = new Point(0, 78);
		((Control)tvServices).Margin = new Padding(6, 6, 6, 6);
		((Control)tvServices).Name = "tvServices";
		((Control)tvServices).Size = new Size(632, 470);
		((Control)tvServices).TabIndex = 9;
		((TreeView)tvServices).ItemDrag += new ItemDragEventHandler(tvServices_ItemDrag);
		((TreeView)tvServices).AfterSelect += new TreeViewEventHandler(tvServices_AfterSelect);
		((Control)tvServices).DragDrop += new DragEventHandler(tvServices_DragDrop);
		((Control)tvServices).DragEnter += new DragEventHandler(tvServices_DragEnter);
		((Control)tvServices).DragOver += new DragEventHandler(tvServices_DragOver);
		((Control)tvServices).KeyDown += new KeyEventHandler(tvServices_KeyDown);
		((Control)stiToolBar1).AccessibleDescription = "stiToolBar1 (stiToolBar1)";
		((Control)stiToolBar1).AccessibleName = "stiToolBar1";
		((Control)stiToolBar1).AccessibleRole = (AccessibleRole)22;
		((Control)stiToolBar1).Dock = (DockStyle)1;
		stiToolBar1.DockTabStripHeight = 50;
		((Control)stiToolBar1).Font = new Font("Segoe UI", 9f);
		stiToolBar1.Items.AddRange(new BaseItem[6] { tbNew, tbOpen, tbSave, tbRemove, tbUp, tbDown });
		stiToolBar1.Location = new Point(0, 47);
		((Control)stiToolBar1).Margin = new Padding(6, 6, 6, 6);
		stiToolBar1.Name = "stiToolBar1";
		stiToolBar1.RoundCorners = false;
		stiToolBar1.Size = new Size(1088, 31);
		stiToolBar1.Stretch = true;
		stiToolBar1.Style = eDotNetBarStyle.Office2010;
		stiToolBar1.TabIndex = 10;
		stiToolBar1.TabStop = false;
		((Control)stiToolBar1).Text = "stiToolBar1";
		tbNew.Image = (Image)componentResourceManager.GetObject("tbNew.Image");
		tbNew.ImagePaddingHorizontal = 16;
		tbNew.ImagePaddingVertical = 12;
		tbNew.Name = "tbNew";
		tbNew.Click += tbNew_Click;
		tbOpen.Image = (Image)componentResourceManager.GetObject("tbOpen.Image");
		tbOpen.ImageIndex = 1;
		tbOpen.ImagePaddingHorizontal = 16;
		tbOpen.ImagePaddingVertical = 12;
		tbOpen.Name = "tbOpen";
		tbOpen.Click += tbOpen_Click;
		tbSave.Image = (Image)componentResourceManager.GetObject("tbSave.Image");
		tbSave.ImagePaddingHorizontal = 16;
		tbSave.ImagePaddingVertical = 12;
		tbSave.Name = "tbSave";
		tbSave.Click += tbSave_Click;
		tbRemove.BeginGroup = true;
		tbRemove.Image = (Image)componentResourceManager.GetObject("tbRemove.Image");
		tbRemove.ImagePaddingHorizontal = 16;
		tbRemove.ImagePaddingVertical = 12;
		tbRemove.Name = "tbRemove";
		tbRemove.Click += tbRemove_Click;
		tbUp.BeginGroup = true;
		tbUp.Image = (Image)componentResourceManager.GetObject("tbUp.Image");
		tbUp.ImagePaddingHorizontal = 16;
		tbUp.ImagePaddingVertical = 12;
		tbUp.Name = "tbUp";
		tbUp.Click += tbUp_Click;
		tbDown.Image = (Image)componentResourceManager.GetObject("tbDown.Image");
		tbDown.ImageIndex = 5;
		tbDown.ImagePaddingHorizontal = 16;
		tbDown.ImagePaddingVertical = 12;
		tbDown.Name = "tbDown";
		tbDown.Click += tbDown_Click;
		((Control)gbService).Controls.Add((Control)(object)lbAssembly);
		((Control)gbService).Controls.Add((Control)(object)lbVersion);
		((Control)gbService).Controls.Add((Control)(object)pbBitmap);
		((Control)gbService).Controls.Add((Control)(object)tbServiceInfo);
		((Control)gbService).Dock = (DockStyle)5;
		((Control)gbService).Location = new Point(8, 4);
		((Control)gbService).Margin = new Padding(6, 6, 6, 6);
		((Control)gbService).Name = "gbService";
		((Control)gbService).Padding = new Padding(6, 6, 6, 6);
		((Control)gbService).Size = new Size(616, 180);
		((Control)gbService).TabIndex = 11;
		gbService.TabStop = false;
		((Control)gbService).Text = "Service";
		((Control)lbAssembly).Anchor = (AnchorStyles)13;
		((Control)lbAssembly).ForeColor = SystemColors.ControlText;
		((Control)lbAssembly).Location = new Point(80, 31);
		((Control)lbAssembly).Margin = new Padding(6, 0, 6, 0);
		((Control)lbAssembly).Name = "lbAssembly";
		((Control)lbAssembly).Size = new Size(526, 46);
		((Control)lbAssembly).TabIndex = 4;
		((Control)lbAssembly).Text = "Assembly:";
		((Control)lbVersion).Anchor = (AnchorStyles)13;
		((Control)lbVersion).ForeColor = SystemColors.ControlText;
		((Control)lbVersion).Location = new Point(80, 77);
		((Control)lbVersion).Margin = new Padding(6, 0, 6, 0);
		((Control)lbVersion).Name = "lbVersion";
		((Control)lbVersion).Size = new Size(526, 46);
		((Control)lbVersion).TabIndex = 3;
		((Control)lbVersion).Text = "Version:";
		((Control)pbBitmap).Location = new Point(16, 31);
		((Control)pbBitmap).Margin = new Padding(6, 6, 6, 6);
		((Control)pbBitmap).Name = "pbBitmap";
		((Control)pbBitmap).Size = new Size(48, 46);
		pbBitmap.TabIndex = 1;
		pbBitmap.TabStop = false;
		((Control)tbServiceInfo).BackColor = Color.FromArgb(250, 250, 250);
		((TextBoxBase)tbServiceInfo).BorderStyle = (BorderStyle)0;
		((Control)tbServiceInfo).Dock = (DockStyle)2;
		((Control)tbServiceInfo).ForeColor = SystemColors.ControlText;
		((Control)tbServiceInfo).Location = new Point(6, 82);
		((Control)tbServiceInfo).Margin = new Padding(6, 6, 6, 6);
		((TextBoxBase)tbServiceInfo).Multiline = true;
		((Control)tbServiceInfo).Name = "tbServiceInfo";
		((TextBoxBase)tbServiceInfo).ReadOnly = true;
		((Control)tbServiceInfo).Size = new Size(604, 92);
		((Control)tbServiceInfo).TabIndex = 0;
		splitter1.BackColor = Color.FromArgb(227, 239, 255);
		splitter1.BackColor2 = Color.FromArgb(101, 147, 207);
		splitter1.BackColor2SchemePart = eColorSchemePart.PanelBorder;
		splitter1.BackColorSchemePart = eColorSchemePart.PanelBackground;
		((Control)splitter1).Dock = (DockStyle)4;
		splitter1.Expandable = false;
		splitter1.ExpandFillColor = Color.FromArgb(101, 147, 207);
		splitter1.ExpandFillColorSchemePart = eColorSchemePart.PanelBorder;
		splitter1.ExpandLineColor = Color.FromArgb(0, 0, 0);
		splitter1.ExpandLineColorSchemePart = eColorSchemePart.ItemText;
		((Control)splitter1).ForeColor = Color.Black;
		splitter1.GripDarkColor = Color.FromArgb(0, 0, 0);
		splitter1.GripDarkColorSchemePart = eColorSchemePart.ItemText;
		splitter1.GripLightColor = Color.FromArgb(227, 239, 255);
		splitter1.GripLightColorSchemePart = eColorSchemePart.BarBackground;
		splitter1.HotBackColor = Color.FromArgb(252, 151, 61);
		splitter1.HotBackColor2 = Color.FromArgb(255, 184, 94);
		splitter1.HotBackColor2SchemePart = eColorSchemePart.ItemPressedBackground2;
		splitter1.HotBackColorSchemePart = eColorSchemePart.ItemPressedBackground;
		splitter1.HotExpandFillColor = Color.FromArgb(101, 147, 207);
		splitter1.HotExpandFillColorSchemePart = eColorSchemePart.PanelBorder;
		splitter1.HotExpandLineColor = Color.FromArgb(0, 0, 0);
		splitter1.HotExpandLineColorSchemePart = eColorSchemePart.ItemText;
		splitter1.HotGripDarkColor = Color.FromArgb(101, 147, 207);
		splitter1.HotGripDarkColorSchemePart = eColorSchemePart.PanelBorder;
		splitter1.HotGripLightColor = Color.FromArgb(227, 239, 255);
		splitter1.HotGripLightColorSchemePart = eColorSchemePart.BarBackground;
		((Control)splitter1).Location = new Point(632, 78);
		((Control)splitter1).Margin = new Padding(6, 6, 6, 6);
		((Control)splitter1).Name = "splitter1";
		((Control)splitter1).Size = new Size(8, 662);
		splitter1.Style = eSplitterStyle.Office2007;
		((Control)splitter1).TabIndex = 15;
		((Splitter)splitter1).TabStop = false;
		dotNetBarManager1.AutoDispatchShortcuts.Add(eShortcut.F1);
		dotNetBarManager1.AutoDispatchShortcuts.Add(eShortcut.CtrlC);
		dotNetBarManager1.AutoDispatchShortcuts.Add(eShortcut.CtrlA);
		dotNetBarManager1.AutoDispatchShortcuts.Add(eShortcut.CtrlV);
		dotNetBarManager1.AutoDispatchShortcuts.Add(eShortcut.CtrlX);
		dotNetBarManager1.AutoDispatchShortcuts.Add(eShortcut.CtrlZ);
		dotNetBarManager1.AutoDispatchShortcuts.Add(eShortcut.CtrlY);
		dotNetBarManager1.AutoDispatchShortcuts.Add(eShortcut.Del);
		dotNetBarManager1.AutoDispatchShortcuts.Add(eShortcut.Ins);
		dotNetBarManager1.BottomDockSite = dockSite4;
		dotNetBarManager1.EnableFullSizeDock = false;
		dotNetBarManager1.LeftDockSite = dockSite1;
		dotNetBarManager1.ParentForm = (Form)(object)this;
		dotNetBarManager1.RightDockSite = dockSite2;
		dotNetBarManager1.Style = eDotNetBarStyle.Office2007;
		dotNetBarManager1.ToolbarBottomDockSite = dockSite8;
		dotNetBarManager1.ToolbarLeftDockSite = dockSite5;
		dotNetBarManager1.ToolbarRightDockSite = dockSite6;
		dotNetBarManager1.ToolbarTopDockSite = dockSite7;
		dotNetBarManager1.TopDockSite = dockSite3;
		((Control)dockSite4).AccessibleRole = (AccessibleRole)9;
		((Control)dockSite4).Dock = (DockStyle)2;
		((Control)dockSite4).Location = new Point(0, 740);
		((Control)dockSite4).Margin = new Padding(6, 6, 6, 6);
		((Control)dockSite4).Name = "dockSite4";
		((Control)dockSite4).Size = new Size(1088, 0);
		((Control)dockSite4).TabIndex = 19;
		((Control)dockSite4).TabStop = false;
		((Control)dockSite1).AccessibleRole = (AccessibleRole)9;
		((Control)dockSite1).Dock = (DockStyle)3;
		((Control)dockSite1).Location = new Point(0, 78);
		((Control)dockSite1).Margin = new Padding(6, 6, 6, 6);
		((Control)dockSite1).Name = "dockSite1";
		((Control)dockSite1).Size = new Size(0, 662);
		((Control)dockSite1).TabIndex = 16;
		((Control)dockSite1).TabStop = false;
		((Control)dockSite2).AccessibleRole = (AccessibleRole)9;
		((Control)dockSite2).Dock = (DockStyle)4;
		((Control)dockSite2).Location = new Point(632, 78);
		((Control)dockSite2).Margin = new Padding(6, 6, 6, 6);
		((Control)dockSite2).Name = "dockSite2";
		((Control)dockSite2).Size = new Size(0, 662);
		((Control)dockSite2).TabIndex = 17;
		((Control)dockSite2).TabStop = false;
		((Control)dockSite8).AccessibleRole = (AccessibleRole)9;
		((Control)dockSite8).Dock = (DockStyle)2;
		((Control)dockSite8).Location = new Point(0, 740);
		((Control)dockSite8).Margin = new Padding(6, 6, 6, 6);
		((Control)dockSite8).Name = "dockSite8";
		((Control)dockSite8).Size = new Size(1088, 0);
		((Control)dockSite8).TabIndex = 23;
		((Control)dockSite8).TabStop = false;
		((Control)dockSite5).AccessibleRole = (AccessibleRole)9;
		((Control)dockSite5).Dock = (DockStyle)3;
		((Control)dockSite5).Location = new Point(0, 47);
		((Control)dockSite5).Margin = new Padding(6, 6, 6, 6);
		((Control)dockSite5).Name = "dockSite5";
		((Control)dockSite5).Size = new Size(0, 693);
		((Control)dockSite5).TabIndex = 20;
		((Control)dockSite5).TabStop = false;
		((Control)dockSite6).AccessibleRole = (AccessibleRole)9;
		((Control)dockSite6).Dock = (DockStyle)4;
		((Control)dockSite6).Location = new Point(1088, 47);
		((Control)dockSite6).Margin = new Padding(6, 6, 6, 6);
		((Control)dockSite6).Name = "dockSite6";
		((Control)dockSite6).Size = new Size(0, 693);
		((Control)dockSite6).TabIndex = 21;
		((Control)dockSite6).TabStop = false;
		((Control)dockSite7).AccessibleRole = (AccessibleRole)9;
		((Control)dockSite7).Controls.Add((Control)(object)bar1);
		((Control)dockSite7).Dock = (DockStyle)1;
		((Control)dockSite7).Location = new Point(0, 0);
		((Control)dockSite7).Margin = new Padding(6, 6, 6, 6);
		((Control)dockSite7).Name = "dockSite7";
		((Control)dockSite7).Size = new Size(1088, 47);
		((Control)dockSite7).TabIndex = 22;
		((Control)dockSite7).TabStop = false;
		((Control)bar1).AccessibleDescription = "bar1 (bar1)";
		((Control)bar1).AccessibleName = "bar1";
		((Control)bar1).AccessibleRole = (AccessibleRole)2;
		bar1.DockSide = eDockSide.Top;
		bar1.DockTabStripHeight = 50;
		((Control)bar1).Font = new Font("Segoe UI", 9f);
		bar1.Items.AddRange(new BaseItem[2] { biFile, buttonItem1 });
		bar1.Location = new Point(0, 0);
		((Control)bar1).Margin = new Padding(6, 6, 6, 6);
		bar1.MenuBar = true;
		bar1.Name = "bar1";
		bar1.Size = new Size(1088, 46);
		bar1.Stretch = true;
		bar1.Style = eDotNetBarStyle.Office2007;
		bar1.TabIndex = 0;
		bar1.TabStop = false;
		((Control)bar1).Text = "bar1";
		biFile.ImagePaddingHorizontal = 16;
		biFile.ImagePaddingVertical = 12;
		biFile.Name = "biFile";
		biFile.SubItems.AddRange(new BaseItem[5] { miNew, miOpen, miSave, miSaveAs, miExit });
		biFile.Text = "&File";
		miNew.Image = (Image)(object)Resources.ReportNew;
		miNew.ImagePaddingHorizontal = 16;
		miNew.ImagePaddingVertical = 12;
		miNew.Name = "miNew";
		miNew.Text = "New";
		miNew.Click += tbNew_Click;
		miOpen.Image = (Image)(object)Resources.ReportOpen;
		miOpen.ImagePaddingHorizontal = 16;
		miOpen.ImagePaddingVertical = 12;
		miOpen.Name = "miOpen";
		miOpen.Text = "Open";
		miOpen.Click += tbOpen_Click;
		miSave.BeginGroup = true;
		miSave.Image = (Image)(object)Resources.ReportSave;
		miSave.ImagePaddingHorizontal = 16;
		miSave.ImagePaddingVertical = 12;
		miSave.Name = "miSave";
		miSave.Text = "&Save";
		miSave.Click += tbSave_Click;
		miSaveAs.ImagePaddingHorizontal = 16;
		miSaveAs.ImagePaddingVertical = 12;
		miSaveAs.Name = "miSaveAs";
		miSaveAs.Text = "Save &As...";
		miSaveAs.Click += miSaveAs_Click;
		miExit.BeginGroup = true;
		miExit.ImagePaddingHorizontal = 16;
		miExit.ImagePaddingVertical = 12;
		miExit.Name = "miExit";
		miExit.Text = "E&xit";
		miExit.Click += miExit_Click;
		buttonItem1.ImagePaddingHorizontal = 16;
		buttonItem1.ImagePaddingVertical = 12;
		buttonItem1.Name = "buttonItem1";
		buttonItem1.SubItems.AddRange(new BaseItem[3] { miAddServices, miRemove, miSynchronize });
		buttonItem1.Text = "&Edit";
		miAddServices.Enabled = false;
		miAddServices.ImagePaddingHorizontal = 16;
		miAddServices.ImagePaddingVertical = 12;
		miAddServices.Name = "miAddServices";
		miAddServices.Text = "&Add Services";
		miAddServices.Click += miAddServices_Click;
		miRemove.Enabled = false;
		miRemove.ImagePaddingHorizontal = 16;
		miRemove.ImagePaddingVertical = 12;
		miRemove.Name = "miRemove";
		miRemove.Text = "&Remove Service";
		miRemove.Click += tbRemove_Click;
		miSynchronize.ImagePaddingHorizontal = 16;
		miSynchronize.ImagePaddingVertical = 12;
		miSynchronize.Name = "miSynchronize";
		miSynchronize.Text = "&Synchronize";
		miSynchronize.Click += tbSynchronize_Click;
		((Control)dockSite3).AccessibleRole = (AccessibleRole)9;
		((Control)dockSite3).Dock = (DockStyle)1;
		((Control)dockSite3).Location = new Point(0, 47);
		((Control)dockSite3).Margin = new Padding(6, 6, 6, 6);
		((Control)dockSite3).Name = "dockSite3";
		((Control)dockSite3).Size = new Size(1088, 0);
		((Control)dockSite3).TabIndex = 18;
		((Control)dockSite3).TabStop = false;
		((Control)panel1).Controls.Add((Control)(object)gbService);
		((Control)panel1).Dock = (DockStyle)2;
		((Control)panel1).Location = new Point(0, 548);
		((Control)panel1).Margin = new Padding(6, 6, 6, 6);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Padding = new Padding(8, 4, 8, 8);
		((Control)panel1).Size = new Size(632, 192);
		((Control)panel1).TabIndex = 24;
		((PropertyGrid)PropertyGrid).CategoryForeColor = SystemColors.InactiveCaptionText;
		((Control)PropertyGrid).Dock = (DockStyle)4;
		((PropertyGrid)PropertyGrid).HelpBackColor = Color.White;
		((PropertyGrid)PropertyGrid).LineColor = SystemColors.ScrollBar;
		((Control)PropertyGrid).Location = new Point(640, 78);
		((Control)PropertyGrid).Margin = new Padding(6);
		((Control)PropertyGrid).Name = "PropertyGrid";
		PropertyGrid.ScrollOffset = 0;
		PropertyGrid.ShowEventsTag = true;
		((Control)PropertyGrid).Size = new Size(448, 662);
		((Control)PropertyGrid).TabIndex = 14;
		((PropertyGrid)PropertyGrid).PropertyValueChanged += new PropertyValueChangedEventHandler(PropertyGrid_PropertyValueChanged);
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).ClientSize = new Size(1088, 740);
		((Control)this).Controls.Add((Control)(object)tvServices);
		((Control)this).Controls.Add((Control)(object)panel1);
		((Control)this).Controls.Add((Control)(object)dockSite2);
		((Control)this).Controls.Add((Control)(object)dockSite1);
		((Control)this).Controls.Add((Control)(object)splitter1);
		((Control)this).Controls.Add((Control)(object)PropertyGrid);
		((Control)this).Controls.Add((Control)(object)stiToolBar1);
		((Control)this).Controls.Add((Control)(object)dockSite3);
		((Control)this).Controls.Add((Control)(object)dockSite4);
		((Control)this).Controls.Add((Control)(object)dockSite5);
		((Control)this).Controls.Add((Control)(object)dockSite6);
		((Control)this).Controls.Add((Control)(object)dockSite7);
		((Control)this).Controls.Add((Control)(object)dockSite8);
		((Form)this).KeyPreview = true;
		((Form)this).Margin = new Padding(6, 6, 6, 6);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiServicesForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Services Configurator";
		((Form)this).Closing += StiServicesForm_Closing;
		((Form)this).Closed += StiServicesForm_Closed;
		((Control)this).KeyDown += new KeyEventHandler(StiServicesForm_KeyDown);
		((ISupportInitialize)stiToolBar1).EndInit();
		((Control)gbService).ResumeLayout(false);
		((Control)gbService).PerformLayout();
		((ISupportInitialize)pbBitmap).EndInit();
		dockSite7.ResumeLayout(performLayout: false);
		((ISupportInitialize)bar1).EndInit();
		((Control)panel1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
