namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ItemContainerAutoSizeBag : BaseItemAutoSizeBag
{
	private bool m_MultiLine;

	private eOrientation m_LayoutOrientation;

	public override void RecordSetting(BaseItem item)
	{
		if (!base.SettingsRecorded)
		{
			ItemContainer itemContainer = item as ItemContainer;
			m_MultiLine = itemContainer.MultiLine;
			m_LayoutOrientation = itemContainer.LayoutOrientation;
			base.RecordSetting(item);
		}
	}

	public override void RestoreSettings()
	{
		if (base.SettingsRecorded)
		{
			ItemContainer itemContainer = Item as ItemContainer;
			itemContainer.MultiLine = m_MultiLine;
			itemContainer.LayoutOrientation = m_LayoutOrientation;
			base.RestoreSettings();
		}
	}
}
