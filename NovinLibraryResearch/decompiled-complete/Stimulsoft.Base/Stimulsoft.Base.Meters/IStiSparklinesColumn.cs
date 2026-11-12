using System.Drawing;

namespace Stimulsoft.Base.Meters;

public interface IStiSparklinesColumn
{
	bool AllowCustomColors { get; set; }

	bool ShowHighLowPoints { get; set; }

	bool ShowFirstLastPoints { get; set; }

	Color PositiveColor { get; set; }

	Color NegativeColor { get; set; }
}
