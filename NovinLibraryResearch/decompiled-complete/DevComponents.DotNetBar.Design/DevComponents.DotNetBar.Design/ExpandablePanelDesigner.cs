using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DevComponents.DotNetBar.Design;

public class ExpandablePanelDesigner : PanelExDesigner
{
	[DefaultValue(eDotNetBarStyle.Office2003)]
	[Description("Gets or sets color scheme style.")]
	[Category("Style")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	public eDotNetBarStyle ColorSchemeStyle
	{
		get
		{
			ExpandablePanel expandablePanel = ((ControlDesigner)this).Control as ExpandablePanel;
			return expandablePanel.ColorSchemeStyle;
		}
		set
		{
			ExpandablePanel expandablePanel = ((ControlDesigner)this).Control as ExpandablePanel;
			expandablePanel.ColorSchemeStyle = value;
			if (((ComponentDesigner)this).GetService(typeof(IDesignerHost)) is IDesignerHost { Loading: false })
			{
				if (Helpers.IsOffice2007Style(value))
				{
					expandablePanel.TitleStyle.Border = eBorderType.RaisedInner;
				}
				else
				{
					expandablePanel.TitleStyle.Border = eBorderType.SingleLine;
				}
			}
		}
	}

	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		base.InitializeNewComponent(defaultValues);
		SetDesignTimeDefaults();
	}

	private new void SetDesignTimeDefaults()
	{
		if (((ControlDesigner)this).Control is ExpandablePanel expandablePanel)
		{
			expandablePanel.ApplyLabelStyle();
			expandablePanel.Style.BorderColor.ColorSchemePart = eColorSchemePart.BarDockedBorder;
			expandablePanel.Style.Border = eBorderType.SingleLine;
			expandablePanel.Style.BackColor1.ColorSchemePart = eColorSchemePart.PanelBackground;
			expandablePanel.Style.BackColor2.ColorSchemePart = eColorSchemePart.PanelBackground2;
			((Control)expandablePanel).Text = "";
			expandablePanel.TitlePanel.ApplyPanelStyle();
			expandablePanel.HideControlsWhenCollapsed = true;
			ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		}
	}

	protected override void PreFilterProperties(IDictionary properties)
	{
		((ParentControlDesigner)this).PreFilterProperties(properties);
		properties["ColorSchemeStyle"] = TypeDescriptor.CreateProperty(typeof(ExpandablePanelDesigner), (PropertyDescriptor)properties["ColorSchemeStyle"], new DefaultValueAttribute(eDotNetBarStyle.Office2003), new BrowsableAttribute(browsable: true), new CategoryAttribute("Style"), new DescriptionAttribute("Gets or sets color scheme style."));
	}

	protected override bool GetHitTest(Point pt)
	{
		if (((ControlDesigner)this).Control is ExpandablePanel expandablePanel)
		{
			Point pt2 = ((Control)expandablePanel).PointToClient(pt);
			PanelExTitle panelExTitle = expandablePanel.TitlePanel as PanelExTitle;
			if (panelExTitle != null && panelExTitle.ExpandChangeButton != null && panelExTitle.ExpandChangeButton.DisplayRectangle.Contains(pt2))
			{
				return true;
			}
			if (panelExTitle != null && panelExTitle.ExpandChangeButton != null)
			{
				panelExTitle.ExpandChangeButton.InternalMouseLeave();
			}
		}
		return ((ScrollableControlDesigner)this).GetHitTest(pt);
	}
}
