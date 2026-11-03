using System.Drawing;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiAxisLabelInfoXF
{
	internal PointF TextPoint = PointF.Empty;

	public RectangleF ClientRectangle { get; set; }

	public float Angle { get; set; }

	public float Width { get; set; }

	public bool WordWrap { get; set; }

	public StiRotationMode RotationMode { get; set; }

	public string Text { get; set; }

	public StiStripLineXF StripLine { get; set; }
}
