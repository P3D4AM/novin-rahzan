using System;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public class StiXTopAxis : StiXAxis, IStiXTopAxis, IStiXAxis, IStiAxis, ICloneable, IStiJsonReportObject, IStiDefault
{
	public override StiComponentId ComponentId => StiComponentId.StiXTopAxis;

	[Browsable(false)]
	[StiNonSerialized]
	public override IStiAxisRange Range
	{
		get
		{
			return base.Range;
		}
		set
		{
			base.Range = value;
		}
	}

	[DefaultValue(false)]
	public override bool Visible
	{
		get
		{
			return base.Visible;
		}
		set
		{
			base.Visible = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override StiShowXAxis ShowXAxis
	{
		get
		{
			return base.ShowXAxis;
		}
		set
		{
			base.ShowXAxis = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override IStiAxisDateTimeStep DateTimeStep
	{
		get
		{
			return base.DateTimeStep;
		}
		set
		{
			base.DateTimeStep = value;
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "Range");
		JsonExtensions.RemoveProperty(val, "ShowXAxis");
		JsonExtensions.RemoveProperty(val, "DateTimeStep");
		JsonExtensions.AddPropertyBool(val, "Visible", Visible, false, false);
		return val;
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		stiPropertyCollection.Add(StiPropertyCategories.Main, new StiPropertyObject[1] { propertyGrid.PropertiesHelper.XTopAxis() });
		return stiPropertyCollection;
	}

	protected override bool ShouldSerializeRange()
	{
		return false;
	}

	protected override bool ShouldSerializeVisible()
	{
		return false;
	}

	protected override bool ShouldSerializeShowXAxis()
	{
		return false;
	}

	protected override bool ShouldSerializeDateTimeStep()
	{
		return false;
	}

	public StiXTopAxis()
	{
		Visible = false;
		base.Core = new StiXTopAxisCoreXF(this);
	}

	public StiXTopAxis(IStiAxisLabels labels, IStiAxisRange range, IStiAxisTitle title, IStiAxisTicks ticks, StiArrowStyle arrowStyle, StiPenStyle lineStyle, Color lineColor, float lineWidth, bool visible, bool startFromZero, bool allowApplyStyle)
		: this(labels, range, title, ticks, arrowStyle, lineStyle, lineColor, lineWidth, visible, startFromZero, showEdgeValues: false, allowApplyStyle)
	{
	}//IL_0008: Unknown result type (might be due to invalid IL or missing references)


	public StiXTopAxis(IStiAxisLabels labels, IStiAxisRange range, IStiAxisTitle title, IStiAxisTicks ticks, StiArrowStyle arrowStyle, StiPenStyle lineStyle, Color lineColor, float lineWidth, bool visible, bool startFromZero, bool showEdgeValues, bool allowApplyStyle)
		: base(labels, range, title, ticks, arrowStyle, lineStyle, lineColor, lineWidth, visible, startFromZero, StiShowXAxis.Both, showEdgeValues, allowApplyStyle)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		base.Core = new StiXTopAxisCoreXF(this);
	}

	public StiXTopAxis(IStiAxisLabels labels, IStiAxisRange range, IStiAxisTitle title, IStiAxisTicks ticks, IStiAxisInteraction interaction, StiArrowStyle arrowStyle, StiPenStyle lineStyle, Color lineColor, float lineWidth, bool visible, bool startFromZero, bool showEdgeValues, bool allowApplyStyle)
		: base(labels, range, title, ticks, interaction, arrowStyle, lineStyle, lineColor, lineWidth, visible, startFromZero, StiShowXAxis.Both, showEdgeValues, allowApplyStyle)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		base.Core = new StiXTopAxisCoreXF(this);
	}

	[StiUniversalConstructor("XTopAxis")]
	public StiXTopAxis(IStiAxisLabels labels, IStiAxisRange range, IStiAxisTitle title, IStiAxisTicks ticks, IStiAxisInteraction interaction, StiArrowStyle arrowStyle, StiPenStyle lineStyle, Color lineColor, float lineWidth, bool visible, bool startFromZero, bool showEdgeValues, bool allowApplyStyle, bool logarithmicScale)
		: base(labels, range, title, ticks, interaction, arrowStyle, lineStyle, lineColor, lineWidth, visible, startFromZero, StiShowXAxis.Both, showEdgeValues, allowApplyStyle, new StiAxisDateTimeStep(), logarithmicScale)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		base.Core = new StiXTopAxisCoreXF(this);
	}
}
