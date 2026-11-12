using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Design.Components.FontIcons;

internal struct StiFontIconMetadata
{
	public StiFontIcons Icon { get; set; }

	public StiFontIconGroup IconGroup { get; set; }

	public override string ToString()
	{
		return IconGroup.ToString() + Icon;
	}

	internal StiFontIconMetadata(StiFontIconGroup iconGroup, StiFontIcons icon)
	{
		IconGroup = iconGroup;
		Icon = icon;
	}
}
