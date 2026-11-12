namespace Stimulsoft.Controls.Win.DotNetBar;

internal class BaseItemAutoSizeBag
{
	public BaseItem Item;

	private bool m_SettingsRecorded;

	protected bool SettingsRecorded
	{
		get
		{
			return m_SettingsRecorded;
		}
		set
		{
			m_SettingsRecorded = value;
		}
	}

	public virtual void RecordSetting(BaseItem item)
	{
		Item = item;
		m_SettingsRecorded = true;
	}

	public virtual void RestoreSettings()
	{
		m_SettingsRecorded = false;
	}
}
