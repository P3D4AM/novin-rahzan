using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DevComponents.DotNetBar.Design;

public class SuperTabStripDesigner : BarBaseControlDesigner
{
	private const string SysBackstageBackButtonName = "sys_backstage_back_button";

	protected bool InternalRemove;

	private SuperTabStrip _TabStrip;

	private SuperTabItem _SelectItem;

	public SuperTabStrip TabStrip
	{
		get
		{
			return _TabStrip;
		}
		set
		{
			_TabStrip = value;
		}
	}

	public override DesignerVerbCollection Verbs
	{
		get
		{
			DesignerVerb[] value = new DesignerVerb[12]
			{
				new DesignerVerb("Next Tab", SelectNextTab),
				new DesignerVerb("Previous Tab", SelectPreviousTab),
				new DesignerVerb("Create New Tab", CreateNewTab),
				new DesignerVerb("Add Button", CreateButton),
				new DesignerVerb("Add Text Box", CreateTextBox),
				new DesignerVerb("Add Combo Box", CreateComboBox),
				new DesignerVerb("Add Label", CreateLabel),
				new DesignerVerb("Add Color Picker", CreateColorPicker),
				new DesignerVerb("Add Progress bar", CreateProgressBar),
				new DesignerVerb("Add Check box", CreateCheckBox),
				new DesignerVerb("Add Switch Button", CreateSwitch),
				new DesignerVerb("Add Micro-Chart", CreateMicroChart)
			};
			return new DesignerVerbCollection(value);
		}
	}

	public override ICollection AssociatedComponents
	{
		get
		{
			ArrayList arrayList = new ArrayList(base.AssociatedComponents);
			foreach (BaseItem tab in _TabStrip.Tabs)
			{
				arrayList.Add(tab);
			}
			return arrayList;
		}
	}

	protected override bool DragInProgress
	{
		get
		{
			return base.DragInProgress;
		}
		set
		{
			if (value)
			{
				if (_TabStrip != null)
				{
					_SelectItem = _TabStrip.SelectedTab;
				}
			}
			else if (_TabStrip != null)
			{
				_TabStrip.SelectedTab = _SelectItem;
			}
			base.DragInProgress = value;
		}
	}

	public SuperTabStripDesigner()
	{
		EnableItemDragDrop = true;
	}

	public override void Initialize(IComponent component)
	{
		base.Initialize(component);
		if (component.Site.DesignMode)
		{
			_TabStrip = component as SuperTabStrip;
			ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
			if (selectionService != null)
			{
				selectionService.SelectionChanged += OnSelectionChanged;
			}
		}
	}

	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		((ParentControlDesigner)this).InitializeNewComponent(defaultValues);
		SetDesignTimeDefaults();
	}

	private void SetDesignTimeDefaults()
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		if (((ComponentDesigner)this).Component != null && ((ComponentDesigner)this).Component.Site != null && ((ComponentDesigner)this).Component.Site.DesignMode)
		{
			_TabStrip.TabStyle = eSuperTabStyle.Office2007;
			_TabStrip.TabFont = ((Control)_TabStrip).Font;
			_TabStrip.SelectedTabFont = new Font(_TabStrip.TabFont, (FontStyle)1);
			CreateNewTab(null, null);
		}
	}

	public virtual SuperTabItem CreateNewTab()
	{
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		SuperTabItem superTabItem = null;
		if (designerHost != null)
		{
			DesignerTransaction designerTransaction = designerHost.CreateTransaction();
			try
			{
				m_CreatingItem = true;
				IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
				componentChangeService?.OnComponentChanging(((ComponentDesigner)this).Component, null);
				superTabItem = designerHost.CreateComponent(typeof(SuperTabItem)) as SuperTabItem;
				if (superTabItem != null)
				{
					superTabItem.Text = superTabItem.Name;
					_TabStrip.Tabs.Add(superTabItem);
				}
				componentChangeService?.OnComponentChanged(((ComponentDesigner)this).Component, null, null, null);
				OnitemCreated(superTabItem);
			}
			catch
			{
				designerTransaction.Cancel();
			}
			finally
			{
				if (!designerTransaction.Canceled)
				{
					designerTransaction.Commit();
				}
				m_CreatingItem = false;
			}
		}
		return superTabItem;
	}

	private void CreateNewTab(object sender, EventArgs e)
	{
		CreateNewTab();
	}

	protected virtual void SelectNextTab(object sender, EventArgs e)
	{
		if (_TabStrip.SelectedTabIndex < _TabStrip.Tabs.Count - 1)
		{
			_TabStrip.SelectNextTab();
		}
	}

	protected virtual void SelectPreviousTab(object sender, EventArgs e)
	{
		if (_TabStrip.SelectedTabIndex > 0)
		{
			_TabStrip.SelectPreviousTab();
		}
	}

	private void OnSelectionChanged(object sender, EventArgs e)
	{
		if (_TabStrip == null || ((Control)_TabStrip).IsDisposed)
		{
			return;
		}
		ISelectionService selectionService = (ISelectionService)sender;
		BaseItem baseItem = null;
		if (selectionService != null && selectionService.PrimarySelection != ((ControlDesigner)this).Control)
		{
			baseItem = selectionService.PrimarySelection as BaseItem;
			if (baseItem != null && !_TabStrip.Tabs.Contains(baseItem))
			{
				baseItem = null;
			}
		}
		if (_TabStrip.DesignTimeSelection != baseItem)
		{
			_TabStrip.DesignTimeSelection = baseItem;
			if (baseItem != null)
			{
				ClosePopups();
			}
		}
	}

	protected override void ComponentChangeComponentAdded(object sender, ComponentEventArgs e)
	{
		if (!(((ComponentDesigner)this).GetService(typeof(ISelectionService)) is ISelectionService selectionService) || selectionService.PrimarySelection != _TabStrip)
		{
			return;
		}
		if (e.Component is BaseItem)
		{
			if (m_InsertItemTransaction == null && sender is IDesignerHost designerHost)
			{
				m_InsertItemTransaction = designerHost.CreateTransaction("Adding Item Clip");
			}
			IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			componentChangeService?.OnComponentChanging(_TabStrip, TypeDescriptor.GetProperties(_TabStrip)["SubItems"]);
			_TabStrip.Tabs.Add(e.Component as BaseItem);
			componentChangeService?.OnComponentChanged(_TabStrip, TypeDescriptor.GetProperties(_TabStrip)["SubItems"], null, null);
			if (m_InsertItemTransaction != null)
			{
				m_InsertItemTransaction.Commit();
				m_InsertItemTransaction = null;
			}
			RecalcLayout();
			if (e.Component is SuperTabItem)
			{
				_TabStrip.SelectedTab = e.Component as SuperTabItem;
			}
		}
		else if (e.Component is SuperTabControlPanel)
		{
			throw new Exception("Invalid component addition");
		}
	}

	protected override void OtherComponentRemoving(object sender, ComponentEventArgs e)
	{
		if (e.Component is BaseItem baseItem)
		{
			if (baseItem.Parent != null && baseItem.Parent.SubItems.Contains(baseItem))
			{
				baseItem.Parent.SubItems.Remove(baseItem);
			}
			DestroySubItems(baseItem);
			RecalcLayout();
		}
	}

	protected override void OnMouseDragBegin(int x, int y)
	{
		base.OnMouseDragBegin(x, y);
		if (DragInProgress)
		{
			_SelectItem = _TabStrip.SelectedTab;
		}
	}

	protected override void Dispose(bool disposing)
	{
		ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
		if (selectionService != null)
		{
			selectionService.SelectionChanged -= OnSelectionChanged;
		}
		base.Dispose(disposing);
	}

	protected override bool GetHitTest(Point pt)
	{
		if (_TabStrip.TabStyle == eSuperTabStyle.Office2010BackstageBlue)
		{
			Point point = ((Control)_TabStrip).PointToClient(pt);
			BaseItem baseItem = _TabStrip.HitTest(point.X, point.Y);
			if (baseItem != null && baseItem.Name == "sys_backstage_back_button")
			{
				return true;
			}
		}
		return base.GetHitTest(pt);
	}
}
