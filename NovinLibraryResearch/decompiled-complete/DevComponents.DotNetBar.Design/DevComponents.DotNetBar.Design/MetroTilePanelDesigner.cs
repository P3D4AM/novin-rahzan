using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Design;

public class MetroTilePanelDesigner : ItemPanelDesigner
{
	public override DesignerVerbCollection Verbs
	{
		get
		{
			_ = ((ControlDesigner)this).Control;
			DesignerVerb[] array = null;
			array = new DesignerVerb[2]
			{
				new DesignerVerb("Add Metro Tile", CreateMetroTile),
				new DesignerVerb("Add Tile Group", CreateHorizontalContainer)
			};
			return new DesignerVerbCollection(array);
		}
	}

	protected override void SetDesignTimeDefaults()
	{
		ItemPanel itemPanel = ((ControlDesigner)this).Control as ItemPanel;
		itemPanel.BackgroundStyle.Class = ElementStyleClassKeys.MetroTilePanelKey;
		ItemContainer itemContainer = CreateContainer(GetItemContainer(), eOrientation.Horizontal);
		itemContainer.TitleStyle.Class = ElementStyleClassKeys.MetroTileGroupTitleKey;
		itemContainer.TitleText = "First";
		CreateMetroTile(itemContainer);
		CreateMetroTile(itemContainer);
		CreateMetroTile(itemContainer);
		CreateMetroTile(itemContainer);
		itemContainer = CreateContainer(GetItemContainer(), eOrientation.Horizontal);
		itemContainer.TitleStyle.Class = ElementStyleClassKeys.MetroTileGroupTitleKey;
		itemContainer.TitleText = "Second";
		CreateMetroTile(itemContainer);
		CreateMetroTile(itemContainer);
		CreateMetroTile(itemContainer);
		CreateMetroTile(itemContainer);
	}

	protected override ItemContainer CreateContainer(BaseItem parent, eOrientation orientation)
	{
		ItemContainer itemContainer = base.CreateContainer(parent, orientation);
		if (itemContainer != null)
		{
			TypeDescriptor.GetProperties(itemContainer)["MultiLine"].SetValue(itemContainer, true);
		}
		return itemContainer;
	}
}
