using System.ComponentModel;
using System.Drawing;

namespace DevComponents.DotNetBar;

[ToolboxItem(false)]
[TypeConverter("DevComponents.DotNetBar.Design.SuperTooltipInfoConverter, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[DesignTimeVisible(false)]
[Localizable(true)]
public class SuperTooltipInfo
{
	private bool ⷈ = true;

	private bool ⷉ = true;

	private string ⷊ = "";

	private string ⷋ = "";

	private Image ⷌ;

	private string ⷍ = "";

	private Image ⷎ;

	private Size _2DCF = Size.Empty;

	private eTooltipColor ൠ = eTooltipColor.Gray;

	[Description("Indicates whether header text is visible.")]
	[Browsable(true)]
	[Localizable(true)]
	[DefaultValue(true)]
	public bool HeaderVisible
	{
		get
		{
			return ⷈ;
		}
		set
		{
			ⷈ = value;
		}
	}

	[DefaultValue(true)]
	[Browsable(true)]
	[Description("Indicates whether footer text is visible.")]
	[Localizable(true)]
	public bool FooterVisible
	{
		get
		{
			return ⷉ;
		}
		set
		{
			ⷉ = value;
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Description("Indicates header text.")]
	[Localizable(true)]
	public string HeaderText
	{
		get
		{
			return ⷊ;
		}
		set
		{
			ⷊ = value;
		}
	}

	[Localizable(true)]
	[Description("Indicates footer text.")]
	[DefaultValue("")]
	[Browsable(true)]
	public string FooterText
	{
		get
		{
			return ⷋ;
		}
		set
		{
			ⷋ = value;
		}
	}

	[Localizable(true)]
	[Description("Indicates body text.")]
	[DefaultValue("")]
	[Browsable(true)]
	public string BodyText
	{
		get
		{
			return ⷍ;
		}
		set
		{
			ⷍ = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[Localizable(true)]
	[Description("Indicates body image displayed to the left of body text.")]
	public Image BodyImage
	{
		get
		{
			return ⷎ;
		}
		set
		{
			ⷎ = value;
		}
	}

	[DefaultValue(null)]
	[Browsable(true)]
	[Localizable(true)]
	[Description("Indicates footer image displayed to the left of footer text.")]
	public Image FooterImage
	{
		get
		{
			return ⷌ;
		}
		set
		{
			ⷌ = value;
		}
	}

	[Browsable(true)]
	[Localizable(true)]
	[Description("Indicates custom size for tooltip.")]
	public Size CustomSize
	{
		get
		{
			return _2DCF;
		}
		set
		{
			_2DCF = value;
		}
	}

	[Localizable(true)]
	[Description("Indicates predefined tooltip color.")]
	[Browsable(true)]
	[DefaultValue(eTooltipColor.Gray)]
	public eTooltipColor Color
	{
		get
		{
			return ൠ;
		}
		set
		{
			ൠ = value;
		}
	}

	public SuperTooltipInfo()
	{
	}

	public SuperTooltipInfo(string headerText, string footerText, string bodyText, Image bodyImage, Image footerImage, eTooltipColor color, bool headerVisible, bool footerVisible, Size customSize)
	{
		ⷊ = headerText;
		ⷋ = footerText;
		ⷍ = bodyText;
		ⷎ = bodyImage;
		ⷌ = footerImage;
		ⷈ = headerVisible;
		ⷉ = footerVisible;
		_2DCF = customSize;
		ൠ = color;
	}

	public SuperTooltipInfo(string headerText, string footerText, string bodyText, Image bodyImage, Image footerImage, eTooltipColor color)
	{
		ⷊ = headerText;
		ⷋ = footerText;
		ⷍ = bodyText;
		ⷎ = bodyImage;
		ⷌ = footerImage;
		ൠ = color;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeCustomSize()
	{
		return !_2DCF.IsEmpty;
	}
}
