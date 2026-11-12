using System;
using System.ComponentModel.Design;
using DevComponents.DotNetBar.Metro;

namespace DevComponents.DotNetBar.Design;

public class ItemContainerDesigner : BaseItemDesigner
{
	public override DesignerVerbCollection Verbs
	{
		get
		{
			DesignerVerb[] value = ((!(((ComponentDesigner)this).Component is BaseItem) || !(((BaseItem)((ComponentDesigner)this).Component).ContainerControl is MetroTilePanel)) ? new DesignerVerb[19]
			{
				new DesignerVerb("Add Button", CreateButton),
				new DesignerVerb("Add Horizontal Container", CreateHorizontalContainer),
				new DesignerVerb("Add Vertical Container", CreateVerticalContainer),
				new DesignerVerb("Add Scrollable Container", CreateGallery),
				new DesignerVerb("Add Text Box", CreateTextBox),
				new DesignerVerb("Add Combo Box", CreateComboBox),
				new DesignerVerb("Add Label", CreateLabel),
				new DesignerVerb("Add Check Box", CreateCheckBox),
				new DesignerVerb("Add Micro-Chart", CreateMicroChart),
				new DesignerVerb("Add Switch button", CreateSwitch),
				new DesignerVerb("Add Control Container", CreateControlContainer),
				new DesignerVerb("Add Color Picker", CreateColorPicker),
				new DesignerVerb("Add Progress-Bar", CreateProgressBar),
				new DesignerVerb("Add Circular Progress", CreateCircularProgressItem),
				new DesignerVerb("Add Rating Item", CreateRatingItem),
				new DesignerVerb("Add Slider", CreateSlider),
				new DesignerVerb("Add Range Slider", CreateRangeSlider),
				new DesignerVerb("Add Month Calendar", CreateMonthCalendar),
				new DesignerVerb("Add Metro Tile", CreateMetroTile)
			} : new DesignerVerb[2]
			{
				new DesignerVerb("Add Metro Tile", CreateMetroTile),
				new DesignerVerb("Add Label", CreateLabel)
			});
			return new DesignerVerbCollection(value);
		}
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
			RecalcLayout();
		}
		finally
		{
			m_CreatingItem = false;
		}
	}
}
