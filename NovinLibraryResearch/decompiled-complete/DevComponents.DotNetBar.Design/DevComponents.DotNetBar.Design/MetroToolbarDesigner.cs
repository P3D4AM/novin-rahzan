using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar.Metro;

namespace DevComponents.DotNetBar.Design;

internal class MetroToolbarDesigner : BarBaseControlDesigner
{
	public override DesignerVerbCollection Verbs
	{
		get
		{
			_ = ((ControlDesigner)this).Control;
			DesignerVerb[] array = null;
			array = new DesignerVerb[12]
			{
				new DesignerVerb("Add Button", CreateButton),
				new DesignerVerb("Add Horizontal Container", CreateHorizontalContainer),
				new DesignerVerb("Add Vertical Container", CreateVerticalContainer),
				new DesignerVerb("Add Text Box", CreateTextBox),
				new DesignerVerb("Add Combo Box", CreateComboBox),
				new DesignerVerb("Add Label", CreateLabel),
				new DesignerVerb("Add Color Picker", CreateColorPicker),
				new DesignerVerb("Add Micro-Chart", CreateMicroChart),
				new DesignerVerb("Add Switch Button", CreateSwitch),
				new DesignerVerb("Add Progress bar", CreateProgressBar),
				new DesignerVerb("Add Check box", CreateCheckBox),
				new DesignerVerb("Add WinForms Control Container", CreateControlContainer)
			};
			return new DesignerVerbCollection(array);
		}
	}

	[DefaultValue(false)]
	[Category("Layout")]
	[Description("Indicates whether control is expanded or not. When control is expanded both main and extra toolbar items are visible.")]
	[Browsable(true)]
	public bool Expanded
	{
		get
		{
			return (bool)((ComponentDesigner)this).ShadowProperties["Expanded"];
		}
		set
		{
			((ComponentDesigner)this).ShadowProperties["Expanded"] = value;
		}
	}

	public override ICollection AssociatedComponents
	{
		get
		{
			ArrayList arrayList = new ArrayList(base.AssociatedComponents);
			MetroToolbar metroToolbar = (MetroToolbar)(object)((ControlDesigner)this).Control;
			foreach (BaseItem item in metroToolbar.Items)
			{
				if (item.DesignMode)
				{
					arrayList.Add(item);
				}
			}
			foreach (BaseItem extraItem in metroToolbar.ExtraItems)
			{
				if (extraItem.DesignMode)
				{
					arrayList.Add(extraItem);
				}
			}
			return arrayList;
		}
	}

	public MetroToolbarDesigner()
	{
		EnableItemDragDrop = true;
		((ComponentDesigner)this).ShadowProperties["Expanded"] = false;
	}

	public override void Initialize(IComponent component)
	{
		base.Initialize(component);
		if (component != null && component.Site != null)
		{
			_ = component.Site.DesignMode;
		}
	}

	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		((ParentControlDesigner)this).InitializeNewComponent(defaultValues);
		SetDesignTimeDefaults();
	}

	private void SetDesignTimeDefaults()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		MetroToolbar metroToolbar = ((ControlDesigner)this).Control as MetroToolbar;
		try
		{
			((Control)metroToolbar).Font = new Font("Segoe UI", 10.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		}
		catch
		{
		}
	}

	private void CreateVerticalContainer(object sender, EventArgs e)
	{
		CreateContainer(GetItemContainer(), eOrientation.Vertical);
	}

	private void CreateHorizontalContainer(object sender, EventArgs e)
	{
		CreateContainer(GetItemContainer(), eOrientation.Horizontal);
	}

	private void CreateContainer(BaseItem parent, eOrientation orientation)
	{
		m_CreatingItem = true;
		try
		{
			DesignerSupport.CreateItemContainer(this, parent, orientation);
		}
		finally
		{
			m_CreatingItem = false;
		}
		RecalcLayout();
	}

	protected override void OnIsSelectedChanged(bool oldValue, bool newValue)
	{
		if (newValue)
		{
			MetroToolbar metroToolbar = (MetroToolbar)(object)((ControlDesigner)this).Control;
			if (!Expanded && metroToolbar.ExpandButtonVisible)
			{
				metroToolbar.Expanded = true;
			}
		}
		else if (!Expanded)
		{
			MetroToolbar metroToolbar2 = (MetroToolbar)(object)((ControlDesigner)this).Control;
			metroToolbar2.Expanded = false;
			if (((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) is IComponentChangeService componentChangeService)
			{
				componentChangeService.OnComponentChanged(metroToolbar2, TypeDescriptor.GetProperties(componentChangeService)["Location"], null, null);
			}
		}
		base.OnIsSelectedChanged(oldValue, newValue);
	}

	protected override void PreFilterProperties(IDictionary properties)
	{
		((ParentControlDesigner)this).PreFilterProperties(properties);
		properties["Expanded"] = TypeDescriptor.CreateProperty(typeof(MetroToolbarDesigner), (PropertyDescriptor)properties["Expanded"], new DefaultValueAttribute(value: false), new BrowsableAttribute(browsable: true), new CategoryAttribute("Layout"), new DescriptionAttribute("Indicates whether control is expanded or not. When control is expanded both main and extra toolbar items are visible."));
	}

	protected override ArrayList GetAllAssociatedComponents()
	{
		ArrayList arrayList = new ArrayList(base.AssociatedComponents);
		MetroToolbar metroToolbar = (MetroToolbar)(object)((ControlDesigner)this).Control;
		BaseItem mainItemsContainer = ((MetroToolbarContainer)metroToolbar.GetBaseItemContainer()).MainItemsContainer;
		if (mainItemsContainer != null)
		{
			AddSubItems(mainItemsContainer, arrayList);
		}
		mainItemsContainer = ((MetroToolbarContainer)metroToolbar.GetBaseItemContainer()).ExtraItemsContainer;
		if (mainItemsContainer != null)
		{
			AddSubItems(mainItemsContainer, arrayList);
		}
		return arrayList;
	}

	protected override BaseItem GetControlItem(Control control)
	{
		MetroToolbar metroToolbar = (MetroToolbar)(object)((ControlDesigner)this).Control;
		BaseItem mainItemsContainer = ((MetroToolbarContainer)metroToolbar.GetBaseItemContainer()).MainItemsContainer;
		if (mainItemsContainer == null)
		{
			return null;
		}
		BaseItem controlItem = GetControlItem(control, mainItemsContainer);
		if (controlItem != null)
		{
			return controlItem;
		}
		mainItemsContainer = ((MetroToolbarContainer)metroToolbar.GetBaseItemContainer()).ExtraItemsContainer;
		if (mainItemsContainer == null)
		{
			return null;
		}
		return GetControlItem(control, mainItemsContainer);
	}

	protected override BaseItem GetDefaultNewItemContainer()
	{
		MetroToolbar metroToolbar = (MetroToolbar)(object)((ControlDesigner)this).Control;
		return ((MetroToolbarContainer)metroToolbar.GetBaseItemContainer()).MainItemsContainer;
	}
}
