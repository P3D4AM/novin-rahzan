using System.Drawing;

namespace Stimulsoft.Base.Meters;

public interface IStiBubbleColumn
{
	bool AllowCustomColors { get; set; }

	Color PositiveColor { get; set; }

	Color NegativeColor { get; set; }
}
