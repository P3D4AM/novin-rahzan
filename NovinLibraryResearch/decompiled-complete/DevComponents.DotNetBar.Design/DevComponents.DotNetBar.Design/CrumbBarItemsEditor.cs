using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.AdvTree;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Design;

internal class CrumbBarItemsEditor : UserControl
{
	internal class PropertyGridSite : ISite, IServiceProvider
	{
		private IComponent comp;

		private bool inGetService;

		private IServiceProvider sp;

		public IComponent Component => comp;

		public IContainer Container => null;

		public bool DesignMode => false;

		public string Name
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public PropertyGridSite(IServiceProvider sp, IComponent comp)
		{
			this.sp = sp;
			this.comp = comp;
		}

		public object GetService(Type t)
		{
			if (!inGetService && sp != null)
			{
				try
				{
					inGetService = true;
					return sp.GetService(t);
				}
				finally
				{
					inGetService = false;
				}
			}
			return null;
		}
	}

	private DevComponents.AdvTree.AdvTree advTree1;

	private Node node1;

	private ElementStyle elementStyle1;

	private ButtonX buttonAddItem;

	private ColumnHeader columnHeader1;

	private ColumnHeader columnHeader2;

	private ButtonX buttonRemoveItem;

	private PropertyGrid propertyGrid1;

	internal ButtonX buttonX1;

	private CrumbBar _CrumbBar;

	private Label label1;

	private CrumbBarDesigner _Designer;

	private IContainer components;

	public CrumbBarDesigner Designer
	{
		get
		{
			return _Designer;
		}
		set
		{
			_Designer = value;
			if (_Designer != null)
			{
				((Component)(object)propertyGrid1).Site = new PropertyGridSite(((ComponentDesigner)_Designer).Component.Site, (IComponent)propertyGrid1);
			}
		}
	}

	public CrumbBar CrumbBar
	{
		get
		{
			return _CrumbBar;
		}
		set
		{
			if (value != _CrumbBar)
			{
				CrumbBar crumbBar = _CrumbBar;
				_CrumbBar = value;
				OnCrumbBarChanged(crumbBar, value);
			}
		}
	}

	public CrumbBarItemsEditor()
	{
		InitializeComponent();
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
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Expected O, but got Unknown
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Expected O, but got Unknown
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Expected O, but got Unknown
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a1: Expected O, but got Unknown
		propertyGrid1 = new PropertyGrid();
		buttonX1 = new ButtonX();
		buttonRemoveItem = new ButtonX();
		buttonAddItem = new ButtonX();
		advTree1 = new DevComponents.AdvTree.AdvTree();
		columnHeader1 = new ColumnHeader();
		columnHeader2 = new ColumnHeader();
		node1 = new Node();
		elementStyle1 = new ElementStyle();
		label1 = new Label();
		((ISupportInitialize)advTree1).BeginInit();
		((Control)this).SuspendLayout();
		((Control)propertyGrid1).Anchor = (AnchorStyles)15;
		((Control)propertyGrid1).Location = new Point(300, 3);
		((Control)propertyGrid1).Name = "propertyGrid1";
		((Control)propertyGrid1).Size = new Size(284, 331);
		((Control)propertyGrid1).TabIndex = 4;
		propertyGrid1.PropertyValueChanged += new PropertyValueChangedEventHandler(propertyGrid1_PropertyValueChanged);
		((Control)buttonX1).AccessibleRole = (AccessibleRole)43;
		((Control)buttonX1).Anchor = (AnchorStyles)10;
		buttonX1.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)buttonX1).Location = new Point(515, 341);
		((Control)buttonX1).Name = "buttonX1";
		buttonX1.Shape = new RoundRectangleShapeDescriptor();
		((Control)buttonX1).Size = new Size(69, 24);
		((Control)buttonX1).TabIndex = 5;
		((Control)buttonX1).Text = "&Close";
		((Control)buttonX1).Click += buttonX1_Click;
		((Control)buttonRemoveItem).AccessibleRole = (AccessibleRole)43;
		((Control)buttonRemoveItem).Anchor = (AnchorStyles)6;
		buttonRemoveItem.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)buttonRemoveItem).Enabled = false;
		buttonRemoveItem.FocusCuesEnabled = false;
		((Control)buttonRemoveItem).Font = new Font("Microsoft Sans Serif", 14.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)buttonRemoveItem).Location = new Point(32, 341);
		((Control)buttonRemoveItem).Name = "buttonRemoveItem";
		buttonRemoveItem.Shape = new RoundRectangleShapeDescriptor();
		((Control)buttonRemoveItem).Size = new Size(24, 24);
		((Control)buttonRemoveItem).TabIndex = 3;
		((Control)buttonRemoveItem).Click += buttonRemoveItem_Click;
		((Control)buttonAddItem).AccessibleRole = (AccessibleRole)43;
		((Control)buttonAddItem).Anchor = (AnchorStyles)6;
		buttonAddItem.ColorTable = eButtonColor.OrangeWithBackground;
		buttonAddItem.FocusCuesEnabled = false;
		((Control)buttonAddItem).Font = new Font("Microsoft Sans Serif", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)buttonAddItem).Location = new Point(4, 341);
		((Control)buttonAddItem).Name = "buttonAddItem";
		buttonAddItem.Shape = new RoundRectangleShapeDescriptor();
		((Control)buttonAddItem).Size = new Size(24, 24);
		((Control)buttonAddItem).TabIndex = 1;
		((Control)buttonAddItem).Click += buttonAddItem_Click;
		((Control)advTree1).AccessibleRole = (AccessibleRole)35;
		((Control)advTree1).AllowDrop = true;
		((Control)advTree1).Anchor = (AnchorStyles)7;
		((Control)advTree1).BackColor = SystemColors.Window;
		advTree1.BackgroundStyle.Class = "TreeBorderKey";
		advTree1.Columns.Add(columnHeader1);
		advTree1.Columns.Add(columnHeader2);
		advTree1.ExpandButtonType = eExpandButtonType.Triangle;
		advTree1.ExpandWidth = 18;
		((Control)advTree1).Location = new Point(3, 3);
		((Control)advTree1).Name = "advTree1";
		advTree1.Nodes.AddRange(new Node[1] { node1 });
		advTree1.NodeStyle = elementStyle1;
		advTree1.PathSeparator = ";";
		advTree1.SelectionBoxStyle = eSelectionStyle.FullRowSelect;
		((Control)advTree1).Size = new Size(291, 332);
		advTree1.Styles.Add(elementStyle1);
		advTree1.SuspendPaint = false;
		((Control)advTree1).TabIndex = 0;
		((Control)advTree1).Text = "advTree1";
		advTree1.AfterNodeDrop += advTree1_AfterNodeDrop;
		((Control)advTree1).MouseUp += new MouseEventHandler(advTree1_MouseUp);
		advTree1.AfterNodeSelect += advTree1_AfterNodeSelect;
		columnHeader1.Name = "columnHeader1";
		columnHeader1.Text = "Text";
		columnHeader1.Width.Relative = 70;
		columnHeader2.Name = "columnHeader2";
		columnHeader2.Text = "Name";
		columnHeader2.Width.Relative = 30;
		node1.Expanded = true;
		node1.Name = "node1";
		node1.Text = "node1";
		elementStyle1.Name = "elementStyle1";
		elementStyle1.TextColor = SystemColors.ControlText;
		((Control)label1).AutoSize = true;
		((Control)label1).Location = new Point(153, 352);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(141, 13);
		((Control)label1).TabIndex = 6;
		((Control)label1).Text = "Drag && drop items to re-order";
		((Control)label1).Anchor = (AnchorStyles)6;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)label1);
		((Control)this).Controls.Add((Control)(object)buttonX1);
		((Control)this).Controls.Add((Control)(object)propertyGrid1);
		((Control)this).Controls.Add((Control)(object)buttonRemoveItem);
		((Control)this).Controls.Add((Control)(object)buttonAddItem);
		((Control)this).Controls.Add((Control)(object)advTree1);
		((Control)this).Name = "CrumbBarItemsEditor";
		((Control)this).Size = new Size(587, 373);
		((UserControl)this).Load += CrumbBarItemsEditor_Load;
		((ISupportInitialize)advTree1).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}

	private object GetDesignService(Type serviceType)
	{
		if (_Designer != null)
		{
			return _Designer.GetDesignService(serviceType);
		}
		return null;
	}

	private CrumbBarItem CreateItem()
	{
		IDesignerHost designerHost = (IDesignerHost)GetDesignService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return null;
		}
		CrumbBarItem crumbBarItem = (CrumbBarItem)designerHost.CreateComponent(typeof(CrumbBarItem));
		crumbBarItem.Text = crumbBarItem.Name;
		return crumbBarItem;
	}

	private void AddNewItem()
	{
		AddNewItem(null);
	}

	private void AddNewItem(CrumbBarItem parent)
	{
		IDesignerHost designerHost = (IDesignerHost)GetDesignService(typeof(IDesignerHost));
		DesignerTransaction designerTransaction = null;
		if (designerHost != null)
		{
			designerTransaction = designerHost.CreateTransaction("New CrumbBarItem");
		}
		bool flag = advTree1.Nodes.Count == 0;
		CrumbBarItem crumbBarItem = CreateItem();
		if (crumbBarItem == null)
		{
			return;
		}
		IComponentChangeService componentChangeService = GetDesignService(typeof(IComponentChangeService)) as IComponentChangeService;
		if (componentChangeService != null)
		{
			if (parent == null)
			{
				componentChangeService.OnComponentChanging(_CrumbBar, TypeDescriptor.GetProperties(_CrumbBar)["Items"]);
			}
			else
			{
				componentChangeService.OnComponentChanging(parent, TypeDescriptor.GetProperties(parent)["SubItems"]);
			}
		}
		if (parent == null)
		{
			_CrumbBar.Items.Add(crumbBarItem);
		}
		else
		{
			parent.SubItems.Add(crumbBarItem);
		}
		if (componentChangeService != null)
		{
			if (parent == null)
			{
				componentChangeService.OnComponentChanged(_CrumbBar, TypeDescriptor.GetProperties(_CrumbBar)["Items"], null, null);
			}
			else
			{
				componentChangeService.OnComponentChanged(parent, TypeDescriptor.GetProperties(parent)["SubItems"], null, null);
			}
		}
		designerTransaction?.Commit();
		if (parent == null)
		{
			advTree1.Nodes.Add(CreateNodeForItem(crumbBarItem));
		}
		else
		{
			advTree1.SelectedNode.Nodes.Add(CreateNodeForItem(crumbBarItem));
			advTree1.SelectedNode.Expand();
		}
		if (flag && advTree1.SelectedNode == null && advTree1.Nodes.Count > 0)
		{
			advTree1.SelectedNode = advTree1.Nodes[0];
		}
	}

	private void CrumbBarItemsEditor_Load(object sender, EventArgs e)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Expected O, but got Unknown
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		Bitmap val = new Bitmap(16, 16, (PixelFormat)2498570);
		Color color = Color.DarkGray;
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			color = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.CheckBoxItem.Default.Text;
		}
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			Brush val3 = (Brush)new SolidBrush(color);
			try
			{
				val2.FillRectangle(val3, 7, 2, 3, 13);
				val2.FillRectangle(val3, 2, 7, 13, 3);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		buttonAddItem.Image = (Image)(object)val;
		val = new Bitmap(16, 16, (PixelFormat)2498570);
		Graphics val4 = Graphics.FromImage((Image)(object)val);
		try
		{
			Brush val5 = (Brush)new SolidBrush(color);
			try
			{
				val4.FillRectangle(val5, 2, 7, 13, 3);
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val4)?.Dispose();
		}
		buttonRemoveItem.Image = (Image)(object)val;
		object designService = GetDesignService(typeof(IUIService));
		IUIService val6 = (IUIService)((designService is IUIService) ? designService : null);
		if (val6 != null)
		{
			((object)propertyGrid1).GetType().GetProperty("ToolStripRenderer", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.SetValue(propertyGrid1, (object?)(ToolStripProfessionalRenderer)val6.Styles["VsToolWindowRenderer"], null);
		}
	}

	private void advTree1_MouseUp(object sender, MouseEventArgs e)
	{
		Node nodeAt = advTree1.GetNodeAt(e.Y);
		if (nodeAt == null)
		{
			advTree1.SelectedNode = null;
		}
	}

	internal void UpdateDisplay()
	{
		advTree1.Nodes.Clear();
		if (_CrumbBar == null)
		{
			return;
		}
		advTree1.BeginUpdate();
		foreach (CrumbBarItem item in _CrumbBar.Items)
		{
			Node node = CreateNodeForItem(item);
			advTree1.Nodes.Add(node);
			LoadSubItems(node, item);
		}
		advTree1.EndUpdate();
	}

	private void LoadSubItems(Node parent, CrumbBarItem item)
	{
		foreach (BaseItem subItem in item.SubItems)
		{
			if (subItem is CrumbBarItem)
			{
				CrumbBarItem item2 = (CrumbBarItem)subItem;
				Node node = CreateNodeForItem(item2);
				parent.Nodes.Add(node);
				LoadSubItems(node, item2);
			}
		}
	}

	private Node CreateNodeForItem(CrumbBarItem item)
	{
		Node node = new Node();
		node.Expanded = true;
		node.Tag = item;
		node.Text = item.Text;
		node.Image = item.GetItemImage();
		node.Cells.Add(new Cell(item.Name));
		return node;
	}

	private void OnCrumbBarChanged(CrumbBar oldValue, CrumbBar newValue)
	{
		UpdateDisplay();
	}

	private void buttonAddItem_Click(object sender, EventArgs e)
	{
		CrumbBarItem parent = null;
		if (advTree1.SelectedNode != null)
		{
			parent = advTree1.SelectedNode.Tag as CrumbBarItem;
		}
		AddNewItem(parent);
	}

	private void buttonRemoveItem_Click(object sender, EventArgs e)
	{
		if (advTree1.SelectedNode != null && advTree1.SelectedNode.Tag is CrumbBarItem item)
		{
			advTree1.SelectedNode.Remove();
			DeleteItem(item);
		}
	}

	private void DeleteItem(CrumbBarItem item)
	{
		((IDesignerHost)GetDesignService(typeof(IDesignerHost)))?.DestroyComponent(item);
	}

	private void advTree1_AfterNodeSelect(object sender, AdvTreeNodeEventArgs e)
	{
		((Control)buttonRemoveItem).Enabled = e.Node != null;
		if (e.Node != null)
		{
			propertyGrid1.SelectedObject = e.Node.Tag;
		}
		else
		{
			propertyGrid1.SelectedObject = null;
		}
	}

	private void advTree1_AfterNodeDrop(object sender, TreeDragDropEventArgs e)
	{
		IDesignerHost designerHost = (IDesignerHost)GetDesignService(typeof(IDesignerHost));
		DesignerTransaction designerTransaction = null;
		if (designerHost != null)
		{
			designerTransaction = designerHost.CreateTransaction("Move items");
		}
		IComponentChangeService componentChangeService = GetDesignService(typeof(IComponentChangeService)) as IComponentChangeService;
		try
		{
			CrumbBarItem crumbBarItem = e.Node.Tag as CrumbBarItem;
			CrumbBarItem crumbBarItem2 = (CrumbBarItem)crumbBarItem.Parent;
			if (componentChangeService != null)
			{
				if (crumbBarItem2 == null)
				{
					componentChangeService.OnComponentChanging(_CrumbBar, TypeDescriptor.GetProperties(_CrumbBar)["Items"]);
				}
				else
				{
					componentChangeService.OnComponentChanging(crumbBarItem2, TypeDescriptor.GetProperties(crumbBarItem2)["SubItems"]);
				}
			}
			if (crumbBarItem2 == null)
			{
				_CrumbBar.Items.Remove(crumbBarItem);
			}
			else
			{
				crumbBarItem2.SubItems.Remove(crumbBarItem);
			}
			if (componentChangeService != null)
			{
				if (crumbBarItem2 == null)
				{
					componentChangeService.OnComponentChanged(_CrumbBar, TypeDescriptor.GetProperties(_CrumbBar)["Items"], null, null);
				}
				else
				{
					componentChangeService.OnComponentChanged(crumbBarItem2, TypeDescriptor.GetProperties(crumbBarItem2)["SubItems"], null, null);
				}
			}
			if (e.NewParentNode == null)
			{
				int index = advTree1.Nodes.IndexOf(e.Node);
				componentChangeService?.OnComponentChanging(_CrumbBar, TypeDescriptor.GetProperties(_CrumbBar)["Items"]);
				_CrumbBar.Items.Insert(index, crumbBarItem);
				componentChangeService?.OnComponentChanged(_CrumbBar, TypeDescriptor.GetProperties(_CrumbBar)["Items"], null, null);
			}
			else
			{
				crumbBarItem2 = e.NewParentNode.Tag as CrumbBarItem;
				int index2 = e.NewParentNode.Nodes.IndexOf(e.Node);
				componentChangeService?.OnComponentChanging(crumbBarItem2, TypeDescriptor.GetProperties(crumbBarItem2)["SubItems"]);
				crumbBarItem2.SubItems.Insert(index2, crumbBarItem);
				componentChangeService?.OnComponentChanged(crumbBarItem2, TypeDescriptor.GetProperties(crumbBarItem2)["SubItems"], null, null);
			}
		}
		catch
		{
			designerTransaction?.Cancel();
			throw;
		}
		finally
		{
			if (designerTransaction != null && !designerTransaction.Canceled)
			{
				designerTransaction.Commit();
			}
		}
	}

	private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
	{
		if (advTree1.SelectedNode != null)
		{
			Node selectedNode = advTree1.SelectedNode;
			CrumbBarItem crumbBarItem = (CrumbBarItem)propertyGrid1.SelectedObject;
			if (e.ChangedItem.PropertyDescriptor.Name == "Text")
			{
				selectedNode.Text = crumbBarItem.Text;
			}
			else if (e.ChangedItem.PropertyDescriptor.Name == "Image" || e.ChangedItem.PropertyDescriptor.Name == "ImageIndex")
			{
				selectedNode.Image = crumbBarItem.GetItemImage();
			}
			else if (e.ChangedItem.PropertyDescriptor.Name == "Name")
			{
				selectedNode.Cells[1].Text = crumbBarItem.Name;
			}
		}
	}

	private void buttonX1_Click(object sender, EventArgs e)
	{
		Form val = ((Control)this).FindForm();
		if (val != null)
		{
			val.Close();
		}
	}
}
