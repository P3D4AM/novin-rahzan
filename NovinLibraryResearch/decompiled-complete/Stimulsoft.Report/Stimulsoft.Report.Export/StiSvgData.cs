using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Export;

public struct StiSvgData
{
	public double X { get; set; }

	public double Y { get; set; }

	public double Width { get; set; }

	public double Height { get; set; }

	public double Right => X + Width;

	public double Bottom => Y + Height;

	public StiComponent Component { get; set; }
}
