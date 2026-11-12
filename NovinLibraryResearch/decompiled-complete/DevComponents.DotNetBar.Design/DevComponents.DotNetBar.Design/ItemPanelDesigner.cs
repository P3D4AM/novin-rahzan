using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Design;

public class ItemPanelDesigner : BarBaseControlDesigner
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
				new DesignerVerb("Add Dock Container", CreateDockContainer),
				new DesignerVerb("Add Text Box", CreateTextBox),
				new DesignerVerb("Add Combo Box", CreateComboBox),
				new DesignerVerb("Add Label", CreateLabel),
				new DesignerVerb("Add Color Picker", CreateColorPicker),
				new DesignerVerb("Add Micro-Chart", CreateMicroChart),
				new DesignerVerb("Add Switch Button", CreateSwitch),
				new DesignerVerb("Add Progress bar", CreateProgressBar),
				new DesignerVerb("Add Check box", CreateCheckBox),
				new DesignerVerb("Add WinForms Control Container", CreateControlContainer),
				new DesignerVerb("Add Metro Tile", CreateMetroTile),
				new DesignerVerb("Apply Panel Style", ApplyPanelStyle),
				new DesignerVerb("Apply Default Style", ApplyDefaultStyle)
			};
			return new DesignerVerbCollection(array);
		}
	}

	public ItemPanelDesigner()
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

	protected virtual void SetDesignTimeDefaults()
	{
		ItemPanel itemPanel = ((ControlDesigner)this).Control as ItemPanel;
		itemPanel.LayoutOrientation = eOrientation.Vertical;
		itemPanel.BackgroundStyle.Class = ElementStyleClassKeys.ItemPanelKey;
	}

	private void ApplyPanelStyle(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is ItemPanel itemPanel)
		{
			IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			componentChangeService?.OnComponentChanging(((ComponentDesigner)this).Component, null);
			ElementStyle backgroundStyle = itemPanel.BackgroundStyle;
			backgroundStyle.Reset();
			backgroundStyle.Border = eStyleBorderType.Solid;
			backgroundStyle.BorderWidth = 1;
			backgroundStyle.BorderColorSchemePart = eColorSchemePart.PanelBorder;
			backgroundStyle.BackColorSchemePart = eColorSchemePart.PanelBackground;
			backgroundStyle.BackColor2SchemePart = eColorSchemePart.PanelBackground2;
			backgroundStyle.BackColorGradientAngle = 90;
			componentChangeService?.OnComponentChanged(((ComponentDesigner)this).Component, null, null, null);
		}
	}

	private void ApplyDefaultStyle(object sender, EventArgs e)
	{
		if (((ControlDesigner)this).Control is ItemPanel itemPanel)
		{
			IComponentChangeService componentChangeService = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService)) as IComponentChangeService;
			componentChangeService?.OnComponentChanging(((ComponentDesigner)this).Component, null);
			ElementStyle backgroundStyle = itemPanel.BackgroundStyle;
			backgroundStyle.Reset();
			backgroundStyle.Border = eStyleBorderType.Solid;
			backgroundStyle.BorderWidth = 1;
			backgroundStyle.BorderColorSchemePart = eColorSchemePart.PanelBorder;
			backgroundStyle.BackColor = Color.White;
			componentChangeService?.OnComponentChanged(((ComponentDesigner)this).Component, null, null, null);
		}
	}

	private void CreateDockContainer(object sender, EventArgs e)
	{
		CreateDockContainer(GetItemContainer());
	}

	private void CreateVerticalContainer(object sender, EventArgs e)
	{
		CreateContainer(GetItemContainer(), eOrientation.Vertical);
	}

	protected virtual void CreateHorizontalContainer(object sender, EventArgs e)
	{
		CreateContainer(GetItemContainer(), eOrientation.Horizontal);
	}

	protected virtual ItemContainer CreateContainer(BaseItem parent, eOrientation orientation)
	{
		ItemContainer result = null;
		m_CreatingItem = true;
		try
		{
			result = DesignerSupport.CreateItemContainer(this, parent, orientation);
		}
		finally
		{
			m_CreatingItem = false;
		}
		RecalcLayout();
		return result;
	}

	protected virtual ItemDockContainer CreateDockContainer(BaseItem parent)
	{
		ItemDockContainer result = null;
		m_CreatingItem = true;
		try
		{
			result = DesignerSupport.CreateItemDockContainer(this, parent);
		}
		finally
		{
			m_CreatingItem = false;
		}
		RecalcLayout();
		return result;
	}

	protected override bool GetHitTest(Point pt)
	{
		if (((ControlDesigner)this).Control is ItemPanel itemPanel && ((Control)itemPanel).IsHandleCreated && itemPanel.AutoScroll)
		{
			Point pt2 = ((Control)itemPanel).PointToClient(pt);
			if (itemPanel.VScrollBar != null && ((Control)itemPanel.VScrollBar).Bounds.Contains(pt2))
			{
				return true;
			}
			if (itemPanel.HScrollBar != null && ((Control)itemPanel.HScrollBar).Bounds.Contains(pt2))
			{
				return true;
			}
		}
		return base.GetHitTest(pt);
	}
}
