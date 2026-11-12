using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro;
using DevComponents.Editors.DateTimeAdv;

namespace DevComponents.DotNetBar.Design;

public class BaseItemDesigner : ComponentDesigner, IDesignerServices
{
	protected bool m_AddingItem;

	protected bool m_CreatingItem;

	protected DesignerTransaction m_InsertItemTransaction;

	public override DesignerVerbCollection Verbs
	{
		get
		{
			DesignerVerb[] verbs = GetVerbs();
			return new DesignerVerbCollection(verbs);
		}
	}

	public override ICollection AssociatedComponents
	{
		get
		{
			ArrayList arrayList = new ArrayList();
			if (!(((ComponentDesigner)this).Component is BaseItem baseItem))
			{
				return ((ComponentDesigner)this).AssociatedComponents;
			}
			baseItem.SubItems.CopyTo(arrayList);
			return arrayList;
		}
	}

	[Category("Behavior")]
	[DefaultValue(true)]
	[Browsable(true)]
	[Description("Indicates whether item can be customized by user.")]
	[DevCoBrowsable(true)]
	public virtual bool CanCustomize
	{
		get
		{
			return (bool)((ComponentDesigner)this).ShadowProperties["CanCustomize"];
		}
		set
		{
			((ComponentDesigner)this).ShadowProperties["CanCustomize"] = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[Description("Gets or sets whether item is visible.")]
	[DefaultValue(true)]
	public bool Visible
	{
		get
		{
			return (bool)((ComponentDesigner)this).ShadowProperties["Visible"];
		}
		set
		{
			((ComponentDesigner)this).ShadowProperties["Visible"] = value;
		}
	}

	public override void Initialize(IComponent component)
	{
		((ComponentDesigner)this).Initialize(component);
		if (component.Site.DesignMode)
		{
			ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
			if (selectionService != null)
			{
				selectionService.SelectionChanged += OnSelectionChanged;
			}
			if (component is BaseItem baseItem)
			{
				Visible = baseItem.Visible;
			}
			IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
			if (componentChangeService != null)
			{
				componentChangeService.ComponentRemoved += OnComponentRemoved;
				componentChangeService.ComponentAdding += ComponentChangeComponentAdding;
				componentChangeService.ComponentAdded += ComponentChangeComponentAdded;
			}
		}
	}

	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		((ComponentDesigner)this).InitializeNewComponent(defaultValues);
		SetDesignTimeDefaults();
	}

	protected virtual void SetDesignTimeDefaults()
	{
	}

	protected virtual void ComponentChangeComponentAdded(object sender, ComponentEventArgs e)
	{
		if (m_AddingItem)
		{
			m_AddingItem = false;
			IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			BaseItem baseItem = ((ComponentDesigner)this).Component as BaseItem;
			componentChangeService?.OnComponentChanging(baseItem, TypeDescriptor.GetProperties(baseItem)["SubItems"]);
			baseItem.SubItems.Add(e.Component as BaseItem);
			componentChangeService?.OnComponentChanged(baseItem, TypeDescriptor.GetProperties(baseItem)["SubItems"], null, null);
			m_InsertItemTransaction.Commit();
			m_InsertItemTransaction = null;
			RecalcLayout();
		}
	}

	protected virtual void ComponentChangeComponentAdding(object sender, ComponentEventArgs e)
	{
		if (m_InsertItemTransaction == null && !m_AddingItem && !m_CreatingItem && e.Component is BaseItem && ((ComponentDesigner)this).GetService(typeof(ISelectionService)) is ISelectionService selectionService && selectionService.PrimarySelection == ((ComponentDesigner)this).Component)
		{
			m_AddingItem = true;
			IDesignerHost designerHost = ((ComponentDesigner)this).GetService(typeof(IDesignerHost)) as IDesignerHost;
			m_InsertItemTransaction = designerHost.CreateTransaction("Adding Item Clip");
		}
	}

	protected override void Dispose(bool disposing)
	{
		ISelectionService selectionService = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
		if (selectionService != null)
		{
			selectionService.SelectionChanged -= OnSelectionChanged;
		}
		IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
		if (componentChangeService != null)
		{
			componentChangeService.ComponentRemoved -= OnComponentRemoved;
		}
		((ComponentDesigner)this).Dispose(disposing);
	}

	private void OnComponentRemoved(object sender, ComponentEventArgs e)
	{
		if (e.Component is BaseItem)
		{
			BaseItem baseItem = ((ComponentDesigner)this).Component as BaseItem;
			if (e.Component is BaseItem baseItem2 && baseItem != null && baseItem.SubItems.Contains(baseItem2))
			{
				IComponentChangeService componentChangeService = (IComponentChangeService)((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
				componentChangeService?.OnComponentChanging(baseItem, TypeDescriptor.GetProperties(baseItem)["SubItems"]);
				baseItem.SubItems.Remove(baseItem2);
				componentChangeService?.OnComponentChanged(baseItem, TypeDescriptor.GetProperties(baseItem)["SubItems"], null, null);
				RecalcLayout();
			}
		}
		ComponentRemoved(e);
	}

	protected virtual void ComponentRemoved(ComponentEventArgs e)
	{
	}

	private void OnSelectionChanged(object sender, EventArgs e)
	{
		ISelectionService selectionService = (ISelectionService)sender;
		if (selectionService != null && selectionService.PrimarySelection != ((ComponentDesigner)this).Component)
		{
			((BaseItem)((ComponentDesigner)this).Component).DesignTimeMouseDownPoint = Point.Empty;
		}
		if (selectionService != null && selectionService.PrimarySelection != ((ComponentDesigner)this).Component && selectionService.PrimarySelection is BaseItem && ((ComponentDesigner)this).Component is BaseItem baseItem)
		{
			BaseItem baseItem2 = selectionService.PrimarySelection as BaseItem;
			if (baseItem.GetOwner() is IOwner owner && owner.GetItem(baseItem2.Name) != baseItem2)
			{
				owner.SetFocusItem(null);
			}
		}
		if (selectionService == null)
		{
			return;
		}
		ICollection selectedComponents = selectionService.GetSelectedComponents();
		bool flag = false;
		foreach (object item in selectedComponents)
		{
			if (item == ((ComponentDesigner)this).Component)
			{
				flag = true;
				break;
			}
		}
		BaseItem baseItem3 = ((ComponentDesigner)this).Component as BaseItem;
		if (flag)
		{
			if (!baseItem3.Focused)
			{
				baseItem3.OnGotFocus();
			}
		}
		else if (baseItem3.Focused)
		{
			baseItem3.OnLostFocus();
		}
	}

	protected virtual DesignerVerb[] GetVerbs()
	{
		return new DesignerVerb[14]
		{
			new DesignerVerb("Add Button", CreateButton),
			new DesignerVerb("Add Text Box", CreateTextBox),
			new DesignerVerb("Add Combo Box", CreateComboBox),
			new DesignerVerb("Add Label", CreateLabel),
			new DesignerVerb("Add Color Picker", CreateColorPicker),
			new DesignerVerb("Add Check Box", CreateCheckBox),
			new DesignerVerb("Add Control Container", CreateControlContainer),
			new DesignerVerb("Add Micro-Chart", CreateMicroChart),
			new DesignerVerb("Add Switch Button", CreateSwitch),
			new DesignerVerb("Add Slider", CreateSlider),
			new DesignerVerb("Add Rating Item", CreateRatingItem),
			new DesignerVerb("Add Horizontal Container", CreateHorizontalContainer),
			new DesignerVerb("Add Vertical Container", CreateVerticalContainer),
			new DesignerVerb("Add Gallery Container", CreateGallery)
		};
	}

	protected virtual void CreateMicroChart(object sender, EventArgs e)
	{
		CreateNewItem(typeof(MicroChartItem));
	}

	protected virtual void CreateCheckBox(object sender, EventArgs e)
	{
		CreateNewItem(typeof(CheckBoxItem));
	}

	protected virtual void CreateRatingItem(object sender, EventArgs e)
	{
		CreateNewItem(typeof(RatingItem));
	}

	protected virtual void CreateButton(object sender, EventArgs e)
	{
		CreateNewItem(typeof(ButtonItem));
	}

	protected virtual void CreateComboBox(object sender, EventArgs e)
	{
		CreateNewItem(typeof(ComboBoxItem));
	}

	protected virtual void CreateLabel(object sender, EventArgs e)
	{
		CreateNewItem(typeof(LabelItem));
	}

	protected virtual void CreateTextBox(object sender, EventArgs e)
	{
		CreateNewItem(typeof(TextBoxItem));
	}

	protected virtual void CreateColorPicker(object sender, EventArgs e)
	{
		CreateNewItem(typeof(ColorPickerDropDown));
	}

	protected virtual void CreateControlContainer(object sender, EventArgs e)
	{
		CreateNewItem(typeof(ControlContainerItem));
	}

	protected virtual void CreateSlider(object sender, EventArgs e)
	{
		CreateNewItem(typeof(SliderItem));
	}

	protected virtual void CreateRangeSlider(object sender, EventArgs e)
	{
		CreateNewItem(typeof(RangeSliderItem));
	}

	protected virtual void CreateSwitch(object sender, EventArgs e)
	{
		CreateNewItem(typeof(SwitchButtonItem));
	}

	protected virtual void CreateProgressBar(object sender, EventArgs e)
	{
		CreateNewItem(typeof(ProgressBarItem));
	}

	protected virtual void CreateCircularProgressItem(object sender, EventArgs e)
	{
		CreateNewItem(typeof(CircularProgressItem));
	}

	protected virtual void CreateMonthCalendar(object sender, EventArgs e)
	{
		CreateNewItem(typeof(MonthCalendarItem));
	}

	protected virtual void CreateMetroTile(object sender, EventArgs e)
	{
		CreateNewItem(typeof(MetroTileItem));
	}

	private void CreateVerticalContainer(object sender, EventArgs e)
	{
		CreateContainer(eOrientation.Vertical);
	}

	private void CreateHorizontalContainer(object sender, EventArgs e)
	{
		CreateContainer(eOrientation.Horizontal);
	}

	private void CreateContainer(eOrientation orientation)
	{
		try
		{
			m_CreatingItem = true;
			DesignerSupport.CreateItemContainer(this, (BaseItem)((ComponentDesigner)this).Component, orientation);
		}
		finally
		{
			m_CreatingItem = false;
		}
		RecalcLayout();
	}

	protected virtual void CreateGallery(object sender, EventArgs e)
	{
		CreateNewItem(typeof(GalleryContainer));
	}

	protected virtual void CreateNewItem(Type itemType)
	{
		_ = ((ComponentDesigner)this).Component;
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		DesignerTransaction designerTransaction = designerHost.CreateTransaction("Creating New Item");
		try
		{
			m_CreatingItem = true;
			if (designerHost.CreateComponent(itemType) is BaseItem baseItem)
			{
				if ((object)itemType != typeof(RatingItem))
				{
					baseItem.Text = baseItem.Name;
				}
				BeforeNewItemAdded(baseItem);
				AddNewItem(baseItem);
				AfterNewItemAdded(baseItem);
				RecalcLayout();
				NewItemAdded(baseItem);
			}
		}
		catch
		{
			designerTransaction.Cancel();
			throw;
		}
		finally
		{
			m_CreatingItem = false;
			if (!designerTransaction.Canceled)
			{
				designerTransaction.Commit();
			}
		}
	}

	protected virtual void AddNewItem(BaseItem newItem)
	{
		BaseItem baseItem = ((ComponentDesigner)this).Component as BaseItem;
		IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
		componentChangeService?.OnComponentChanging(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(baseItem).Find("SubItems", ignoreCase: true));
		baseItem.SubItems.Add(newItem);
		componentChangeService?.OnComponentChanged(((ComponentDesigner)this).Component, TypeDescriptor.GetProperties(baseItem).Find("SubItems", ignoreCase: true), null, null);
	}

	protected virtual void BeforeNewItemAdded(BaseItem item)
	{
	}

	protected virtual void AfterNewItemAdded(BaseItem item)
	{
		if (item is LabelItem && Helpers.IsOffice2007Style(item.EffectiveStyle))
		{
			LabelItem labelItem = item as LabelItem;
			if (item.Parent is PopupItem && ((PopupItem)item.Parent).PopupType == ePopupType.Menu)
			{
				labelItem.BackColor = ColorScheme.GetColor("DDE7EE");
				labelItem.BorderType = eBorderType.SingleLine;
				labelItem.SingleLineColor = ColorScheme.GetColor("C5C5C5");
				labelItem.ForeColor = ColorScheme.GetColor("00156E");
				labelItem.BorderSide = eBorderSide.Bottom;
				labelItem.PaddingTop = 1;
				labelItem.PaddingLeft = 10;
				labelItem.PaddingBottom = 1;
			}
		}
		else if (item is GalleryContainer && item.Parent is ButtonItem)
		{
			TypeDescriptor.GetProperties(item)["MinimumSize"].SetValue(item, new Size(150, 200));
			item.NeedRecalcSize = true;
			TypeDescriptor.GetProperties(((GalleryContainer)item).BackgroundStyle)["Class"].SetValue(((GalleryContainer)item).BackgroundStyle, "");
			if (item.Parent is PopupItem)
			{
				TypeDescriptor.GetProperties(item)["EnableGalleryPopup"].SetValue(item, false);
			}
		}
		else if (item is GalleryContainer && item.Parent is ItemContainer && !(item.ContainerControl is RibbonBar))
		{
			TypeDescriptor.GetProperties(item)["MinimumSize"].SetValue(item, new Size(150, 200));
			TypeDescriptor.GetProperties(((GalleryContainer)item).BackgroundStyle)["Class"].SetValue(((GalleryContainer)item).BackgroundStyle, "");
			TypeDescriptor.GetProperties(item)["EnableGalleryPopup"].SetValue(item, false);
			TypeDescriptor.GetProperties(item)["LayoutOrientation"].SetValue(item, eOrientation.Vertical);
			TypeDescriptor.GetProperties(item)["MultiLine"].SetValue(item, false);
			TypeDescriptor.GetProperties(item)["PopupUsesStandardScrollbars"].SetValue(item, false);
			item.NeedRecalcSize = true;
		}
	}

	protected virtual void RecalcLayout()
	{
		BaseItem baseItem = ((ComponentDesigner)this).Component as BaseItem;
		object containerControl = baseItem.ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val is Bar)
		{
			((Bar)(object)val).RecalcLayout();
		}
		else if (val is ExplorerBar)
		{
			((ExplorerBar)(object)val).RecalcLayout();
		}
		else if (val is BarBaseControl)
		{
			((BarBaseControl)(object)val).RecalcLayout();
		}
		else if (val is SideBar)
		{
			((SideBar)(object)val).RecalcLayout();
		}
		else if (val is ItemControl)
		{
			((ItemControl)(object)val).RecalcLayout();
		}
		else if (val is MenuPanel)
		{
			((MenuPanel)(object)val).RecalcSize();
		}
		if (baseItem.Expanded && baseItem is PopupItem && ((PopupItem)baseItem).PopupControl != null)
		{
			val = ((PopupItem)baseItem).PopupControl;
			if (val is MenuPanel)
			{
				((MenuPanel)(object)val).RecalcSize();
			}
			else if (val is Bar)
			{
				((Bar)(object)val).RecalcLayout();
			}
		}
	}

	protected virtual void NewItemAdded(BaseItem itemAdded)
	{
		BaseItem baseItem = ((ComponentDesigner)this).Component as BaseItem;
		object owner = baseItem.GetOwner();
		Control val = (Control)((owner is Control) ? owner : null);
		if (val is ItemControl)
		{
			((ItemControl)(object)val).DesignerNewItemAdded();
		}
	}

	protected override void PreFilterProperties(IDictionary properties)
	{
		((ComponentDesigner)this).PreFilterProperties(properties);
		properties["Visible"] = TypeDescriptor.CreateProperty(typeof(BaseItemDesigner), (PropertyDescriptor)properties["Visible"], new DefaultValueAttribute(value: true), new BrowsableAttribute(browsable: true), new CategoryAttribute("Layout"));
		properties["CanCustomize"] = TypeDescriptor.CreateProperty(typeof(BaseItemDesigner), (PropertyDescriptor)properties["CanCustomize"], new DefaultValueAttribute(value: true), new BrowsableAttribute(browsable: true), new CategoryAttribute("Behavior"), new DescriptionAttribute("Indicates whether item can be customized by user."));
	}

	object IDesignerServices.CreateComponent(Type componentClass)
	{
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return null;
		}
		object obj = null;
		try
		{
			m_CreatingItem = true;
			return designerHost.CreateComponent(componentClass);
		}
		finally
		{
			m_CreatingItem = false;
		}
	}

	object IDesignerServices.CreateComponent(Type componentClass, string name)
	{
		IDesignerHost designerHost = (IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost));
		if (designerHost == null)
		{
			return null;
		}
		object obj = null;
		try
		{
			m_CreatingItem = true;
			return designerHost.CreateComponent(componentClass, name);
		}
		finally
		{
			m_CreatingItem = false;
		}
	}

	void IDesignerServices.DestroyComponent(IComponent c)
	{
		((IDesignerHost)((ComponentDesigner)this).GetService(typeof(IDesignerHost)))?.DestroyComponent(c);
	}

	object IDesignerServices.GetService(Type serviceType)
	{
		return ((ComponentDesigner)this).GetService(serviceType);
	}
}
