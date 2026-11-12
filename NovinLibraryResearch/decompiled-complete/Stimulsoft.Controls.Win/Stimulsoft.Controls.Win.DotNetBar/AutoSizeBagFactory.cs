namespace Stimulsoft.Controls.Win.DotNetBar;

internal class AutoSizeBagFactory
{
	public static BaseItemAutoSizeBag CreateAutoSizeBag(ButtonItem item)
	{
		ButtonItemAutoSizeBag buttonItemAutoSizeBag = new ButtonItemAutoSizeBag();
		buttonItemAutoSizeBag.Item = item;
		return buttonItemAutoSizeBag;
	}

	public static ItemContainerAutoSizeBag CreateAutoSizeBag(ItemContainer item)
	{
		ItemContainerAutoSizeBag itemContainerAutoSizeBag = new ItemContainerAutoSizeBag();
		itemContainerAutoSizeBag.Item = item;
		return itemContainerAutoSizeBag;
	}
}
