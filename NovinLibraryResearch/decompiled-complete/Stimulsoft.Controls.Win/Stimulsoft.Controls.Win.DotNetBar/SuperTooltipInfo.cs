using System.ComponentModel;
using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

[TypeConverter("Stimulsoft.Controls.Win.DotNetBar.Design.SuperTooltipInfoConverter, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a")]
[DesignTimeVisible(false)]
[ToolboxItem(false)]
[Localizable(true)]
public class SuperTooltipInfo
{
	public bool HeaderVisible = true;

	public bool FooterVisible = true;

	public string HeaderText = string.Empty;

	public string FooterText = string.Empty;

	public string BodyText = string.Empty;

	public Image BodyImage;

	public Image FooterImage;

	public Size CustomSize = Size.Empty;

	public eTooltipColor Color = eTooltipColor.Gray;

	public string Hyperlink;

	public SuperTooltipInfo()
	{
	}

	public SuperTooltipInfo(string headerText, string footerText, string bodyText, Image bodyImage, Image footerImage, eTooltipColor color, bool headerVisible, bool footerVisible, Size customSize)
	{
		HeaderText = headerText;
		FooterText = footerText;
		BodyText = bodyText;
		BodyImage = bodyImage;
		FooterImage = footerImage;
		HeaderVisible = headerVisible;
		FooterVisible = footerVisible;
		CustomSize = customSize;
		Color = color;
	}

	public SuperTooltipInfo(string headerText, string footerText, string bodyText, Image bodyImage, Image footerImage, eTooltipColor color)
	{
		HeaderText = headerText;
		FooterText = footerText;
		BodyText = bodyText;
		BodyImage = bodyImage;
		FooterImage = footerImage;
		Color = color;
	}
}
