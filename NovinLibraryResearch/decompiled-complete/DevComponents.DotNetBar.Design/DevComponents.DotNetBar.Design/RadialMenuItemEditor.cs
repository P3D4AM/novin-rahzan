using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.AdvTree;
using DevComponents.DotNetBar.Events;

namespace DevComponents.DotNetBar.Design;

public class RadialMenuItemEditor : UserControl
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

	private RadialMenuDesigner _Designer;

	private RadialMenuContainer _RadialMenuContainer;

	private RadialMenu _RadialMenu;

	private IContainer components;

	private PropertyGrid propertyGrid1;

	private DevComponents.AdvTree.AdvTree advTree1;

	private NodeConnector nodeConnector1;

	private ElementStyle elementStyle1;

	private Bar bar1;

	private ButtonItem buttonNew;

	private ButtonItem buttonRemove;

	private Panel panel1;

	private SplitContainer splitContainer1;

	private Panel panel2;

	private RadialMenu radialMenu1;

	private ButtonX buttonClose;

	private Label label1;

	private ButtonItem buttonNewSubMenu;

	public RadialMenuDesigner Designer
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

	public RadialMenu RadialMenu
	{
		get
		{
			return _RadialMenu;
		}
		set
		{
			if (value != _RadialMenu)
			{
				RadialMenu radialMenu = _RadialMenu;
				_RadialMenu = value;
				OnRadialMenuChanged(radialMenu, value);
			}
		}
	}

	public RadialMenuContainer RadialMenuContainer
	{
		get
		{
			return _RadialMenuContainer;
		}
		set
		{
			if (value != _RadialMenuContainer)
			{
				RadialMenuContainer radialMenuContainer = _RadialMenuContainer;
				_RadialMenuContainer = value;
				OnRadialMenuContainerChanged(radialMenuContainer, value);
			}
		}
	}

	public RadialMenuItemEditor()
	{
		InitializeComponent();
	}

	private object GetDesignService(Type serviceType)
	{
		if (_Designer != null)
		{
			return _Designer.GetDesignService(serviceType);
		}
		return null;
	}

	private RadialMenuItem CreateItem()
	{
		IDesignerHost designerHost = (IDesignerHost)GetDesignService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return null;
		}
		RadialMenuItem radialMenuItem = (RadialMenuItem)designerHost.CreateComponent(typeof(RadialMenuItem));
		int num = 1;
		if (_RadialMenu != null)
		{
			num = _RadialMenu.Items.Count + 1;
		}
		else if (_RadialMenuContainer != null)
		{
			num = _RadialMenuContainer.SubItems.Count + 1;
		}
		radialMenuItem.Text = "Item " + num;
		return radialMenuItem;
	}

	private void AddNewItem()
	{
		AddNewItem(null);
	}

	private void OnComponentChanging(RadialMenuItem parent, IComponentChangeService cc)
	{
		if (parent == null)
		{
			if (_RadialMenu != null)
			{
				cc.OnComponentChanging(_RadialMenu, TypeDescriptor.GetProperties(_RadialMenu)["Items"]);
			}
			else if (_RadialMenuContainer != null)
			{
				cc.OnComponentChanging(_RadialMenuContainer, TypeDescriptor.GetProperties(_RadialMenuContainer)["SubItems"]);
			}
		}
		else
		{
			cc.OnComponentChanging(parent, TypeDescriptor.GetProperties(parent)["SubItems"]);
		}
	}

	private void OnComponentChanged(RadialMenuItem parent, IComponentChangeService cc)
	{
		if (parent == null)
		{
			if (_RadialMenu != null)
			{
				cc.OnComponentChanged(_RadialMenu, TypeDescriptor.GetProperties(_RadialMenu)["Items"], null, null);
			}
			else if (_RadialMenuContainer != null)
			{
				cc.OnComponentChanged(_RadialMenuContainer, TypeDescriptor.GetProperties(_RadialMenuContainer)["SubItems"], null, null);
			}
		}
		else
		{
			cc.OnComponentChanged(parent, TypeDescriptor.GetProperties(parent)["SubItems"], null, null);
		}
	}

	private void AddNewItem(RadialMenuItem parent)
	{
		IDesignerHost designerHost = (IDesignerHost)GetDesignService(typeof(IDesignerHost));
		DesignerTransaction designerTransaction = null;
		if (designerHost != null)
		{
			designerTransaction = designerHost.CreateTransaction("New RadialMenuItem");
		}
		RadialMenuItem radialMenuItem = CreateItem();
		if (radialMenuItem == null)
		{
			return;
		}
		IComponentChangeService componentChangeService = GetDesignService(typeof(IComponentChangeService)) as IComponentChangeService;
		if (componentChangeService != null)
		{
			OnComponentChanging(parent, componentChangeService);
		}
		if (parent == null)
		{
			if (_RadialMenu != null)
			{
				_RadialMenu.Items.Add(radialMenuItem);
			}
			else if (_RadialMenuContainer != null)
			{
				_RadialMenuContainer.SubItems.Add(radialMenuItem);
			}
		}
		else
		{
			parent.SubItems.Add(radialMenuItem);
		}
		if (componentChangeService != null)
		{
			OnComponentChanged(parent, componentChangeService);
		}
		designerTransaction?.Commit();
		Node node = CreateNodeForItem(radialMenuItem);
		if (parent == null)
		{
			advTree1.Nodes.Add(node);
		}
		else
		{
			advTree1.SelectedNode.Nodes.Add(node);
			advTree1.SelectedNode.Expand();
		}
		advTree1.SelectedNode = node;
	}

	private void RadialMenuItemEditor_Load(object sender, EventArgs e)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		object designService = GetDesignService(typeof(IUIService));
		IUIService val = (IUIService)((designService is IUIService) ? designService : null);
		if (val != null)
		{
			((object)propertyGrid1).GetType().GetProperty("ToolStripRenderer", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.SetValue(propertyGrid1, (object?)(ToolStripProfessionalRenderer)val.Styles["VsToolWindowRenderer"], null);
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
		if (_RadialMenu == null && _RadialMenuContainer == null)
		{
			return;
		}
		advTree1.BeginUpdate();
		SubItemsCollection subItemsCollection = null;
		subItemsCollection = ((_RadialMenu == null) ? _RadialMenuContainer.SubItems : _RadialMenu.Items);
		foreach (BaseItem item2 in subItemsCollection)
		{
			if (item2 is RadialMenuItem item)
			{
				Node node = CreateNodeForItem(item);
				advTree1.Nodes.Add(node);
				LoadSubItems(node, item);
			}
		}
		advTree1.EndUpdate();
	}

	private void LoadSubItems(Node parent, RadialMenuItem item)
	{
		foreach (BaseItem subItem in item.SubItems)
		{
			if (subItem is RadialMenuItem)
			{
				RadialMenuItem item2 = (RadialMenuItem)subItem;
				Node node = CreateNodeForItem(item2);
				parent.Nodes.Add(node);
				LoadSubItems(node, item2);
			}
		}
	}

	private Node CreateNodeForItem(RadialMenuItem item)
	{
		Node node = new Node();
		node.Expanded = true;
		node.Tag = item;
		node.Text = item.Text + " [" + item.Name + "]";
		return node;
	}

	private string GetText(RadialMenuItem item)
	{
		return item.Text + " [" + item.Name + "]";
	}

	private void OnRadialMenuChanged(RadialMenu oldValue, RadialMenu newValue)
	{
		UpdateDisplay();
	}

	protected virtual void OnRadialMenuContainerChanged(RadialMenuContainer oldValue, RadialMenuContainer newValue)
	{
		UpdateDisplay();
	}

	private void buttonAddItem_Click(object sender, EventArgs e)
	{
		AddNewItem(null);
	}

	private void buttonNewSubMenu_Click(object sender, EventArgs e)
	{
		RadialMenuItem parent = null;
		if (advTree1.SelectedNode != null)
		{
			parent = advTree1.SelectedNode.Tag as RadialMenuItem;
		}
		AddNewItem(parent);
	}

	private void buttonRemoveItem_Click(object sender, EventArgs e)
	{
		if (advTree1.SelectedNode != null && advTree1.SelectedNode.Tag is RadialMenuItem item)
		{
			advTree1.SelectedNode.Remove();
			DeleteItem(item);
		}
	}

	private void DeleteItem(RadialMenuItem item)
	{
		IDesignerHost designerHost = (IDesignerHost)GetDesignService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return;
		}
		IComponentChangeService componentChangeService = GetDesignService(typeof(IComponentChangeService)) as IComponentChangeService;
		RadialMenuItem radialMenuItem = item.Parent as RadialMenuItem;
		if (componentChangeService != null)
		{
			OnComponentChanging(radialMenuItem, componentChangeService);
		}
		if (radialMenuItem == null)
		{
			if (_RadialMenu != null)
			{
				_RadialMenu.Items.Remove(item);
			}
			else if (_RadialMenuContainer != null)
			{
				_RadialMenuContainer.SubItems.Remove(item);
			}
		}
		else
		{
			radialMenuItem.SubItems.Remove(item);
		}
		if (componentChangeService != null)
		{
			OnComponentChanged(radialMenuItem, componentChangeService);
		}
		designerHost.DestroyComponent(item);
	}

	private void advTree1_AfterNodeSelect(object sender, AdvTreeNodeEventArgs e)
	{
		buttonRemove.Enabled = e.Node != null;
		buttonNewSubMenu.Enabled = e.Node != null;
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
			RadialMenuItem radialMenuItem = e.Node.Tag as RadialMenuItem;
			RadialMenuItem radialMenuItem2 = radialMenuItem.Parent as RadialMenuItem;
			if (componentChangeService != null)
			{
				OnComponentChanging(radialMenuItem2, componentChangeService);
			}
			if (radialMenuItem2 == null)
			{
				if (_RadialMenu != null)
				{
					_RadialMenu.Items.Remove(radialMenuItem);
				}
				else if (_RadialMenuContainer != null)
				{
					_RadialMenuContainer.SubItems.Remove(radialMenuItem);
				}
			}
			else
			{
				radialMenuItem2.SubItems.Remove(radialMenuItem);
			}
			if (componentChangeService != null)
			{
				OnComponentChanged(radialMenuItem2, componentChangeService);
			}
			if (e.NewParentNode == null)
			{
				int index = advTree1.Nodes.IndexOf(e.Node);
				if (componentChangeService != null)
				{
					if (_RadialMenu != null)
					{
						componentChangeService.OnComponentChanging(_RadialMenu, TypeDescriptor.GetProperties(_RadialMenu)["Items"]);
					}
					else if (_RadialMenuContainer != null)
					{
						componentChangeService.OnComponentChanging(_RadialMenuContainer, TypeDescriptor.GetProperties(_RadialMenuContainer)["SubItems"]);
					}
				}
				if (_RadialMenu != null)
				{
					_RadialMenu.Items.Insert(index, radialMenuItem);
				}
				else if (_RadialMenuContainer != null)
				{
					_RadialMenuContainer.SubItems.Insert(index, radialMenuItem);
				}
				if (componentChangeService != null)
				{
					if (_RadialMenu != null)
					{
						componentChangeService.OnComponentChanged(_RadialMenu, TypeDescriptor.GetProperties(_RadialMenu)["Items"], null, null);
					}
					else if (_RadialMenuContainer != null)
					{
						componentChangeService.OnComponentChanged(_RadialMenuContainer, TypeDescriptor.GetProperties(_RadialMenuContainer)["SubItems"], null, null);
					}
				}
			}
			else
			{
				radialMenuItem2 = e.NewParentNode.Tag as RadialMenuItem;
				int index2 = e.NewParentNode.Nodes.IndexOf(e.Node);
				componentChangeService?.OnComponentChanging(radialMenuItem2, TypeDescriptor.GetProperties(radialMenuItem2)["SubItems"]);
				radialMenuItem2.SubItems.Insert(index2, radialMenuItem);
				componentChangeService?.OnComponentChanged(radialMenuItem2, TypeDescriptor.GetProperties(radialMenuItem2)["SubItems"], null, null);
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
			RadialMenuItem item = (RadialMenuItem)propertyGrid1.SelectedObject;
			if (e.ChangedItem.PropertyDescriptor.Name == "Text" || e.ChangedItem.PropertyDescriptor.Name == "Name")
			{
				selectedNode.Text = GetText(item);
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

	private void radialMenu1_BeforeMenuOpen(object sender, CancelableEventSourceArgs e)
	{
		SubItemsCollection subItemsCollection = null;
		if (_RadialMenu != null)
		{
			subItemsCollection = _RadialMenu.Items;
			radialMenu1.MenuType = _RadialMenu.MenuType;
			radialMenu1.Diameter = _RadialMenu.Diameter;
			radialMenu1.CenterButtonDiameter = _RadialMenu.CenterButtonDiameter;
			((Control)radialMenu1).Font = ((Control)_RadialMenu).Font;
		}
		else if (_RadialMenuContainer != null)
		{
			subItemsCollection = _RadialMenuContainer.SubItems;
			radialMenu1.MenuType = _RadialMenuContainer.MenuType;
			radialMenu1.Diameter = _RadialMenuContainer.Diameter;
			radialMenu1.CenterButtonDiameter = _RadialMenuContainer.CenterButtonDiameter;
		}
		if (subItemsCollection == null)
		{
			return;
		}
		if (radialMenu1.Items.Count > 0)
		{
			BaseItem[] array = new BaseItem[radialMenu1.Items.Count];
			radialMenu1.Items.CopyTo(array, 0);
			radialMenu1.Items.Clear();
			BaseItem[] array2 = array;
			foreach (BaseItem baseItem in array2)
			{
				baseItem.Dispose();
			}
		}
		foreach (BaseItem item2 in subItemsCollection)
		{
			if (item2 is RadialMenuItem)
			{
				BaseItem item = item2.Copy();
				radialMenu1.Items.Add(item);
			}
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
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Expected O, but got Unknown
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Expected O, but got Unknown
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Expected O, but got Unknown
		//IL_0878: Unknown result type (might be due to invalid IL or missing references)
		propertyGrid1 = new PropertyGrid();
		advTree1 = new DevComponents.AdvTree.AdvTree();
		nodeConnector1 = new NodeConnector();
		elementStyle1 = new ElementStyle();
		bar1 = new Bar();
		buttonNew = new ButtonItem();
		buttonNewSubMenu = new ButtonItem();
		buttonRemove = new ButtonItem();
		panel1 = new Panel();
		splitContainer1 = new SplitContainer();
		panel2 = new Panel();
		label1 = new Label();
		buttonClose = new ButtonX();
		radialMenu1 = new RadialMenu();
		((ISupportInitialize)advTree1).BeginInit();
		((ISupportInitialize)bar1).BeginInit();
		((Control)splitContainer1.Panel1).SuspendLayout();
		((Control)splitContainer1.Panel2).SuspendLayout();
		((Control)splitContainer1).SuspendLayout();
		((Control)panel2).SuspendLayout();
		((Control)this).SuspendLayout();
		propertyGrid1.CommandsVisibleIfAvailable = false;
		((Control)propertyGrid1).Dock = (DockStyle)5;
		((Control)propertyGrid1).Location = new Point(0, 0);
		((Control)propertyGrid1).Name = "propertyGrid1";
		propertyGrid1.PropertySort = (PropertySort)1;
		((Control)propertyGrid1).Size = new Size(310, 360);
		((Control)propertyGrid1).TabIndex = 0;
		propertyGrid1.PropertyValueChanged += new PropertyValueChangedEventHandler(propertyGrid1_PropertyValueChanged);
		((Control)advTree1).AccessibleRole = (AccessibleRole)35;
		((Control)advTree1).AllowDrop = true;
		((Control)advTree1).BackColor = SystemColors.Window;
		advTree1.BackgroundStyle.Class = "TreeBorderKey";
		advTree1.BackgroundStyle.CornerType = eCornerType.Square;
		((Control)advTree1).Dock = (DockStyle)5;
		((Control)advTree1).Location = new Point(0, 0);
		((Control)advTree1).Name = "advTree1";
		advTree1.NodesConnector = nodeConnector1;
		advTree1.NodeStyle = elementStyle1;
		advTree1.PathSeparator = ";";
		((Control)advTree1).Size = new Size(292, 360);
		advTree1.Styles.Add(elementStyle1);
		((Control)advTree1).TabIndex = 1;
		((Control)advTree1).Text = "advTree1";
		advTree1.AfterNodeDrop += advTree1_AfterNodeDrop;
		advTree1.AfterNodeSelect += advTree1_AfterNodeSelect;
		((Control)advTree1).MouseUp += new MouseEventHandler(advTree1_MouseUp);
		nodeConnector1.LineColor = SystemColors.ControlText;
		elementStyle1.CornerType = eCornerType.Square;
		elementStyle1.Name = "elementStyle1";
		elementStyle1.TextColor = SystemColors.ControlText;
		bar1.AntiAlias = true;
		((Control)bar1).Dock = (DockStyle)1;
		((Control)bar1).Font = new Font("Segoe UI", 9f);
		bar1.Items.AddRange(new BaseItem[3] { buttonNew, buttonNewSubMenu, buttonRemove });
		bar1.Location = new Point(4, 4);
		bar1.Name = "bar1";
		bar1.RoundCorners = false;
		bar1.Size = new Size(614, 28);
		bar1.Stretch = true;
		bar1.Style = eDotNetBarStyle.StyleManagerControlled;
		bar1.TabIndex = 2;
		bar1.TabStop = false;
		((Control)bar1).Text = "bar1";
		buttonNew.ButtonStyle = eButtonStyle.ImageAndText;
		buttonNew.Name = "buttonNew";
		buttonNew.Symbol = "\uf055";
		buttonNew.Text = "New Menu Item";
		buttonNew.Tooltip = "Use drag & drop below to re-order items you create";
		buttonNew.Click += buttonAddItem_Click;
		buttonNewSubMenu.ButtonStyle = eButtonStyle.ImageAndText;
		buttonNewSubMenu.Enabled = false;
		buttonNewSubMenu.Name = "buttonNewSubMenu";
		buttonNewSubMenu.Symbol = "\uf067";
		buttonNewSubMenu.Text = "New Sub Menu Item";
		buttonNewSubMenu.Click += buttonNewSubMenu_Click;
		buttonRemove.ButtonStyle = eButtonStyle.ImageAndText;
		buttonRemove.Enabled = false;
		buttonRemove.Name = "buttonRemove";
		buttonRemove.Symbol = "\uf057";
		buttonRemove.Text = "Remove Selected";
		buttonRemove.Click += buttonRemoveItem_Click;
		((Control)panel1).Dock = (DockStyle)1;
		((Control)panel1).Location = new Point(4, 32);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(614, 12);
		((Control)panel1).TabIndex = 3;
		splitContainer1.Dock = (DockStyle)5;
		((Control)splitContainer1).Location = new Point(4, 44);
		((Control)splitContainer1).Name = "splitContainer1";
		((Control)splitContainer1.Panel1).Controls.Add((Control)(object)advTree1);
		((Control)splitContainer1.Panel2).Controls.Add((Control)(object)propertyGrid1);
		((Control)splitContainer1).Size = new Size(614, 360);
		splitContainer1.SplitterDistance = 292;
		splitContainer1.SplitterWidth = 12;
		((Control)splitContainer1).TabIndex = 4;
		((Control)panel2).Controls.Add((Control)(object)label1);
		((Control)panel2).Controls.Add((Control)(object)buttonClose);
		((Control)panel2).Controls.Add((Control)(object)radialMenu1);
		((Control)panel2).Dock = (DockStyle)2;
		((Control)panel2).Location = new Point(4, 404);
		((Control)panel2).Name = "panel2";
		((Control)panel2).Size = new Size(614, 52);
		((Control)panel2).TabIndex = 5;
		((Control)label1).AutoSize = true;
		((Control)label1).Location = new Point(206, 21);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(60, 13);
		((Control)label1).TabIndex = 2;
		((Control)label1).Text = "Test menu:";
		((Control)buttonClose).AccessibleRole = (AccessibleRole)43;
		((Control)buttonClose).Anchor = (AnchorStyles)9;
		buttonClose.ColorTable = eButtonColor.OrangeWithBackground;
		((Control)buttonClose).Location = new Point(528, 14);
		((Control)buttonClose).Name = "buttonClose";
		((Control)buttonClose).Size = new Size(83, 26);
		buttonClose.Style = eDotNetBarStyle.StyleManagerControlled;
		((Control)buttonClose).TabIndex = 1;
		((Control)buttonClose).Text = "Close";
		((Control)buttonClose).Click += buttonX1_Click;
		((Control)radialMenu1).Location = new Point(282, 13);
		((Control)radialMenu1).Name = "radialMenu1";
		((Control)radialMenu1).Size = new Size(28, 28);
		radialMenu1.Symbol = "\uf043";
		((Control)radialMenu1).TabIndex = 0;
		((Control)radialMenu1).Text = "radialMenu1";
		radialMenu1.BeforeMenuOpen += radialMenu1_BeforeMenuOpen;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)splitContainer1);
		((Control)this).Controls.Add((Control)(object)panel2);
		((Control)this).Controls.Add((Control)(object)panel1);
		((Control)this).Controls.Add((Control)(object)bar1);
		((Control)this).Name = "RadialMenuItemEditor";
		((Control)this).Padding = new Padding(4);
		((Control)this).Size = new Size(622, 460);
		((UserControl)this).Load += RadialMenuItemEditor_Load;
		((ISupportInitialize)advTree1).EndInit();
		((ISupportInitialize)bar1).EndInit();
		((Control)splitContainer1.Panel1).ResumeLayout(false);
		((Control)splitContainer1.Panel2).ResumeLayout(false);
		((Control)splitContainer1).ResumeLayout(false);
		((Control)panel2).ResumeLayout(false);
		((Control)panel2).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
