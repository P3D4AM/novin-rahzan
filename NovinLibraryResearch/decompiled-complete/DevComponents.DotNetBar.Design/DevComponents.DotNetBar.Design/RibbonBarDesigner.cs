using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;

namespace DevComponents.DotNetBar.Design;

public class RibbonBarDesigner : BarBaseControlDesigner
{
	public override DesignerVerbCollection Verbs
	{
		get
		{
			_ = ((ControlDesigner)this).Control;
			DesignerVerb[] array = null;
			array = new DesignerVerb[16]
			{
				new DesignerVerb("Add Button", CreateButton),
				new DesignerVerb("Add Horizontal Container", CreateHorizontalContainer),
				new DesignerVerb("Add Vertical Container", CreateVerticalContainer),
				new DesignerVerb("Add Text Box", CreateTextBox),
				new DesignerVerb("Add Combo Box", CreateComboBox),
				new DesignerVerb("Add Label", CreateLabel),
				new DesignerVerb("Add Color Picker", CreateColorPicker),
				new DesignerVerb("Add Check Box", CreateCheckBox),
				new DesignerVerb("Add Micro-Chart", CreateMicroChart),
				new DesignerVerb("Add Switch Button", CreateSwitch),
				new DesignerVerb("Add Slider", CreateSliderItem),
				new DesignerVerb("Add Range Slider", CreateRangeSliderItem),
				new DesignerVerb("Add Rating Item", CreateRatingItem),
				new DesignerVerb("Add Circular Progress", CreateCircularProgressItem),
				new DesignerVerb("Add Gallery Container", CreateGalleryContainer),
				new DesignerVerb("Add Control Container", CreateControlContainer)
			};
			return new DesignerVerbCollection(array);
		}
	}

	[DevCoBrowsable(true)]
	[Description("Specifies the visual style of the control.")]
	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue(eDotNetBarStyle.Office2003)]
	public eDotNetBarStyle Style
	{
		get
		{
			RibbonBar ribbonBar = ((ControlDesigner)this).Control as RibbonBar;
			return ribbonBar.Style;
		}
		set
		{
			RibbonBar ribbonBar = ((ControlDesigner)this).Control as RibbonBar;
			ribbonBar.Style = value;
			if (((ComponentDesigner)this).GetService(typeof(IDesignerHost)) is IDesignerHost { Loading: false })
			{
				RibbonPredefinedColorSchemes.SetRibbonBarStyle(ribbonBar, value);
			}
		}
	}

	public RibbonBarDesigner()
	{
		EnableItemDragDrop = true;
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
		_ = ((ControlDesigner)this).Control;
		Style = eDotNetBarStyle.StyleManagerControlled;
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

	protected override void PreFilterProperties(IDictionary properties)
	{
		((ParentControlDesigner)this).PreFilterProperties(properties);
		properties["Style"] = TypeDescriptor.CreateProperty(typeof(RibbonBarDesigner), (PropertyDescriptor)properties["Style"]);
	}

	protected override void OnitemCreated(BaseItem item)
	{
		if (item is ButtonItem)
		{
			ButtonItem component = item as ButtonItem;
			TypeDescriptor.GetProperties(component)["SubItemsExpandWidth"].SetValue(component, 14);
		}
		base.OnitemCreated(item);
	}
}
