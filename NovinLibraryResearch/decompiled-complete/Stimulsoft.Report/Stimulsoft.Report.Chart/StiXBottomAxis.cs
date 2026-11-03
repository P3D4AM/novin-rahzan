using System;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiXBottomAxis : StiXAxis, IStiXBottomAxis, IStiXAxis, IStiAxis, ICloneable, IStiJsonReportObject, IStiDefault
{
	public StiXBottomAxis()
	{
		base.Core = new StiXBottomAxisCoreXF(this);
	}

	public StiXBottomAxis(IStiAxisLabels labels, IStiAxisRange range, IStiAxisTitle title, IStiAxisTicks ticks, StiArrowStyle arrowStyle, StiPenStyle lineStyle, Color lineColor, float lineWidth, bool visible, bool startFromZero, StiShowXAxis showXAxis, bool allowApplyStyle)
		: this(labels, range, title, ticks, arrowStyle, lineStyle, lineColor, lineWidth, visible, startFromZero, showXAxis, showEdgeValues: false, allowApplyStyle)
	{
	}//IL_0008: Unknown result type (might be due to invalid IL or missing references)


	public StiXBottomAxis(IStiAxisLabels labels, IStiAxisRange range, IStiAxisTitle title, IStiAxisTicks ticks, StiArrowStyle arrowStyle, StiPenStyle lineStyle, Color lineColor, float lineWidth, bool visible, bool startFromZero, StiShowXAxis showXAxis, bool showEdgeValues, bool allowApplyStyle)
		: base(labels, range, title, ticks, arrowStyle, lineStyle, lineColor, lineWidth, visible, startFromZero, showXAxis, showEdgeValues, allowApplyStyle)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		base.Core = new StiXBottomAxisCoreXF(this);
	}

	public StiXBottomAxis(IStiAxisLabels labels, IStiAxisRange range, IStiAxisTitle title, IStiAxisTicks ticks, IStiAxisInteraction interaction, StiArrowStyle arrowStyle, StiPenStyle lineStyle, Color lineColor, float lineWidth, bool visible, bool startFromZero, StiShowXAxis showXAxis, bool showEdgeValues, bool allowApplyStyle)
		: base(labels, range, title, ticks, interaction, arrowStyle, lineStyle, lineColor, lineWidth, visible, startFromZero, showXAxis, showEdgeValues, allowApplyStyle)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		base.Core = new StiXBottomAxisCoreXF(this);
	}

	public StiXBottomAxis(IStiAxisLabels labels, IStiAxisRange range, IStiAxisTitle title, IStiAxisTicks ticks, IStiAxisInteraction interaction, StiArrowStyle arrowStyle, StiPenStyle lineStyle, Color lineColor, float lineWidth, bool visible, bool startFromZero, StiShowXAxis showXAxis, bool showEdgeValues, bool allowApplyStyle, IStiAxisDateTimeStep dateTimeStep)
		: base(labels, range, title, ticks, interaction, arrowStyle, lineStyle, lineColor, lineWidth, visible, startFromZero, showXAxis, showEdgeValues, allowApplyStyle, dateTimeStep)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		DateTimeStep = dateTimeStep;
		base.Core = new StiXBottomAxisCoreXF(this);
	}

	[StiUniversalConstructor("XAxis")]
	public StiXBottomAxis(IStiAxisLabels labels, IStiAxisRange range, IStiAxisTitle title, IStiAxisTicks ticks, IStiAxisInteraction interaction, StiArrowStyle arrowStyle, StiPenStyle lineStyle, Color lineColor, float lineWidth, bool visible, bool startFromZero, StiShowXAxis showXAxis, bool showEdgeValues, bool allowApplyStyle, IStiAxisDateTimeStep dateTimeStep, bool logarithmicScale)
		: base(labels, range, title, ticks, interaction, arrowStyle, lineStyle, lineColor, lineWidth, visible, startFromZero, showXAxis, showEdgeValues, allowApplyStyle, dateTimeStep, logarithmicScale)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		DateTimeStep = dateTimeStep;
		base.Core = new StiXBottomAxisCoreXF(this);
	}
}
