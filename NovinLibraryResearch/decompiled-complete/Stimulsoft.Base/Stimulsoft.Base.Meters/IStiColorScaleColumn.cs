using System.Drawing;

namespace Stimulsoft.Base.Meters;

public interface IStiColorScaleColumn
{
	Color MinimumColor { get; set; }

	Color MaximumColor { get; set; }
}
