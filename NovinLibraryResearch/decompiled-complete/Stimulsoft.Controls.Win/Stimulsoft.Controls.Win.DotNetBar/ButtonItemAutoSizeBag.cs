using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ButtonItemAutoSizeBag : BaseItemAutoSizeBag
{
	private eButtonStyle m_ButtonStyle;

	private eImagePosition m_ImagePosition;

	private Size m_ImageFixedSize = Size.Empty;

	private string m_Text;

	public override void RecordSetting(BaseItem item)
	{
		if (!base.SettingsRecorded)
		{
			ButtonItem buttonItem = item as ButtonItem;
			m_ButtonStyle = buttonItem.ButtonStyle;
			m_ImagePosition = buttonItem.ImagePosition;
			m_ImageFixedSize = buttonItem.ImageFixedSize;
			if (buttonItem.TextMarkupBody != null && buttonItem.TextMarkupBody.HasExpandElement)
			{
				m_Text = buttonItem.Text;
			}
			base.RecordSetting(item);
		}
	}

	public override void RestoreSettings()
	{
		if (base.SettingsRecorded)
		{
			ButtonItem buttonItem = Item as ButtonItem;
			bool globalItem = buttonItem.GlobalItem;
			buttonItem.GlobalItem = false;
			buttonItem.ButtonStyle = m_ButtonStyle;
			buttonItem.ImagePosition = m_ImagePosition;
			buttonItem.ImageFixedSize = m_ImageFixedSize;
			if (m_Text != null)
			{
				buttonItem.Text = m_Text;
			}
			buttonItem.GlobalItem = globalItem;
			base.RestoreSettings();
		}
	}
}
