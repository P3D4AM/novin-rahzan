using System;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiYLeftAxis : StiYAxis, IStiYLeftAxis, IStiYAxis, IStiAxis, ICloneable, IStiJsonReportObject, IStiDefault
{
	public StiYLeftAxis()
	{
		base.Core = new StiYLeftAxisCoreXF(this);
	}

	public StiYLeftAxis(IStiAxisLabels labels, IStiAxisRange range, IStiAxisTitle title, IStiAxisTicks ticks, StiArrowStyle arrowStyle, StiPenStyle lineStyle, Color lineColor, float lineWidth, bool visible, bool startFromZero, StiShowYAxis showYAxis, bool allowApplyStyle)
		: base(labels, range, title, ticks, arrowStyle, lineStyle, lineColor, lineWidth, visible, startFromZero, showYAxis, allowApplyStyle)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		base.Core = new StiYLeftAxisCoreXF(this);
	}

	public StiYLeftAxis(IStiAxisLabels labels, IStiAxisRange range, IStiAxisTitle title, IStiAxisTicks ticks, IStiAxisInteraction interaction, StiArrowStyle arrowStyle, StiPenStyle lineStyle, Color lineColor, float lineWidth, bool visible, bool startFromZero, StiShowYAxis showYAxis, bool allowApplyStyle)
		: base(labels, range, title, ticks, interaction, arrowStyle, lineStyle, lineColor, lineWidth, visible, startFromZero, showYAxis, allowApplyStyle)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		base.Core = new StiYLeftAxisCoreXF(this);
	}

	[StiUniversalConstructor("YAxis")]
	public StiYLeftAxis(IStiAxisLabels labels, IStiAxisRange range, IStiAxisTitle title, IStiAxisTicks ticks, IStiAxisInteraction interaction, StiArrowStyle arrowStyle, StiPenStyle lineStyle, Color lineColor, float lineWidth, bool visible, bool startFromZero, StiShowYAxis showYAxis, bool allowApplyStyle, bool logarithmicScale)
		: base(labels, range, title, ticks, interaction, arrowStyle, lineStyle, lineColor, lineWidth, visible, startFromZero, showYAxis, allowApplyStyle, logarithmicScale)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		base.Core = new StiYLeftAxisCoreXF(this);
	}
}
