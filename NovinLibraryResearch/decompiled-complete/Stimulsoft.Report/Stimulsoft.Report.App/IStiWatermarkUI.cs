using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.App;

public interface IStiWatermarkUI
{
	StiAdvancedWatermark WatermarkUI { get; set; }

	IStiApp GetApp();
}
