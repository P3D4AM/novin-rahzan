using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Dashboard;

public interface IStiButtonElementIconSet
{
	StiFontIcons? Icon { get; set; }

	StiFontIcons? CheckedIcon { get; set; }

	StiFontIcons? UncheckedIcon { get; set; }
}
