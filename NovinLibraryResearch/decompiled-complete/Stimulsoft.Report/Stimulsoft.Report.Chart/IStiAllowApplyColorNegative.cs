using System.Drawing;

namespace Stimulsoft.Report.Chart;

public interface IStiAllowApplyColorNegative
{
	bool AllowApplyColorNegative { get; set; }

	Color LineColorNegative { get; set; }
}
