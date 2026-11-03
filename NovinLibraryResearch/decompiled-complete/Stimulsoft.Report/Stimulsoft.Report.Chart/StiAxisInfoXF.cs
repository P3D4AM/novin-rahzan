using System;
using System.Collections.Generic;

namespace Stimulsoft.Report.Chart;

public class StiAxisInfoXF : ICloneable
{
	public StiStripLinesXF StripLines;

	public float[] StripPositions;

	public List<StiStripPositionXF> TicksCollection;

	public List<StiStripPositionXF> LabelsCollection;

	public List<StiAxisLabelInfoXF> LabelInfoCollection;

	public double Minimum;

	public double Maximum;

	public double Dpi { get; set; }

	public float Step { get; set; }

	public double Range => Maximum - Minimum;

	public object Clone()
	{
		StiAxisInfoXF stiAxisInfoXF = MemberwiseClone() as StiAxisInfoXF;
		stiAxisInfoXF.StripLines = StripLines.Clone() as StiStripLinesXF;
		return stiAxisInfoXF;
	}
}
