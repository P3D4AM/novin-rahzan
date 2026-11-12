using System.Collections;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DevComponents.DotNetBar.Design;

public class NavigationPanePanelDesigner : PanelExDesigner
{
	public override SelectionRules SelectionRules => (SelectionRules)(-1073741824);

	public override void InitializeNewComponent(IDictionary defaultValues)
	{
		base.InitializeNewComponent(defaultValues);
		SetDesignTimeDefaults();
	}

	private new void SetDesignTimeDefaults()
	{
		if (((ControlDesigner)this).Control is PanelEx panelEx)
		{
			panelEx.ApplyPanelStyle();
			panelEx.Style.BackColor2.ColorSchemePart = eColorSchemePart.None;
			((Control)panelEx).Text = "";
		}
	}
}
