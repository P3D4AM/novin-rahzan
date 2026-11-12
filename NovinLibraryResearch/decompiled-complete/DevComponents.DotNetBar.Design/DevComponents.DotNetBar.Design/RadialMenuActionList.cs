using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DevComponents.DotNetBar.Design;

internal class RadialMenuActionList : DesignerActionList
{
	private RadialMenuDesigner _Designer;

	public eRadialMenuType MenuType
	{
		get
		{
			return ((RadialMenu)((DesignerActionList)this).Component).MenuType;
		}
		set
		{
			TypeDescriptor.GetProperties(((DesignerActionList)this).Component)["MenuType"].SetValue(((DesignerActionList)this).Component, value);
		}
	}

	public RadialMenuActionList(RadialMenuDesigner designer)
		: base(((ComponentDesigner)designer).Component)
	{
		_Designer = designer;
	}

	public override DesignerActionItemCollection GetSortedActionItems()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		DesignerActionItemCollection val = new DesignerActionItemCollection();
		val.Add((DesignerActionItem)new DesignerActionHeaderItem("Menu Items"));
		val.Add((DesignerActionItem)new DesignerActionHeaderItem("Menu Type"));
		val.Add((DesignerActionItem)new DesignerActionHeaderItem("Appearance"));
		val.Add((DesignerActionItem)new DesignerActionMethodItem((DesignerActionList)(object)this, "EditItems", "Edit Items", "Menu Items", true));
		val.Add((DesignerActionItem)new DesignerActionMethodItem((DesignerActionList)(object)this, "RightSize", "Right Size Control", "Appearance", true));
		val.Add((DesignerActionItem)new DesignerActionPropertyItem("MenuType", "Type of Radial Menu", "Menu Type", "Specifies the type of radial menu displayed."));
		return val;
	}

	public void EditItems()
	{
		_Designer.EditItems();
	}

	public void RightSize()
	{
		((Control)(RadialMenu)(object)((ControlDesigner)_Designer).Control).Size = new Size(28, 28);
	}
}
