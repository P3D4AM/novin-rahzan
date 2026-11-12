using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar.Metro;

namespace DevComponents.DotNetBar.Design;

public class MetroStatusBarDesigner : BarBaseControlDesigner
{
	public override DesignerVerbCollection Verbs
	{
		get
		{
			_ = ((ControlDesigner)this).Control;
			DesignerVerb[] array = null;
			array = new DesignerVerb[13]
			{
				new DesignerVerb("Add Button", CreateButton),
				new DesignerVerb("Add Horizontal Container", CreateHorizontalContainer),
				new DesignerVerb("Add Vertical Container", CreateVerticalContainer),
				new DesignerVerb("Add Slider", CreateSliderItem),
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

	public MetroStatusBarDesigner()
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
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		MetroStatusBar metroStatusBar = ((ControlDesigner)this).Control as MetroStatusBar;
		((Control)metroStatusBar).Dock = (DockStyle)2;
		try
		{
			((Control)metroStatusBar).Font = new Font("Segoe UI", 10.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
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
}
