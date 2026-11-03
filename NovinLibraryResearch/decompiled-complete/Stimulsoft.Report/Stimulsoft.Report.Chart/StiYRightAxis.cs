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

public class StiYRightAxis : StiYAxis, IStiYRightAxis, IStiYAxis, IStiAxis, ICloneable, IStiJsonReportObject, IStiDefault
{
	public override StiComponentId ComponentId => StiComponentId.StiYRightAxis;

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
	public override StiShowYAxis ShowYAxis
	{
		get
		{
			return base.ShowYAxis;
		}
		set
		{
			base.ShowYAxis = value;
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "ShowYAxis");
		JsonExtensions.AddPropertyBool(val, "Visible", Visible, false, false);
		return val;
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		stiPropertyCollection.Add(StiPropertyCategories.Main, new StiPropertyObject[1] { propertyGrid.PropertiesHelper.YRightAxis() });
		return stiPropertyCollection;
	}

	protected override bool ShouldSerializeVisible()
	{
		return false;
	}

	protected override bool ShouldSerializeShowYAxis()
	{
		return false;
	}

	public StiYRightAxis()
	{
		Visible = false;
		base.Labels.TextAlignment = (StiHorAlignment)1;
		base.Core = new StiYRightAxisCoreXF(this);
	}

	public StiYRightAxis(IStiAxisLabels labels, IStiAxisRange range, IStiAxisTitle title, IStiAxisTicks ticks, StiArrowStyle arrowStyle, StiPenStyle lineStyle, Color lineColor, float lineWidth, bool visible, bool startFromZero, bool allowApplyStyle)
		: base(labels, range, title, ticks, arrowStyle, lineStyle, lineColor, lineWidth, visible, startFromZero, StiShowYAxis.Both, allowApplyStyle)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		base.Labels.TextAlignment = (StiHorAlignment)1;
		base.Core = new StiYRightAxisCoreXF(this);
	}

	public StiYRightAxis(IStiAxisLabels labels, IStiAxisRange range, IStiAxisTitle title, IStiAxisTicks ticks, IStiAxisInteraction interaction, StiArrowStyle arrowStyle, StiPenStyle lineStyle, Color lineColor, float lineWidth, bool visible, bool startFromZero, bool allowApplyStyle)
		: base(labels, range, title, ticks, interaction, arrowStyle, lineStyle, lineColor, lineWidth, visible, startFromZero, StiShowYAxis.Both, allowApplyStyle)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		base.Labels.TextAlignment = (StiHorAlignment)1;
		base.Core = new StiYRightAxisCoreXF(this);
	}

	[StiUniversalConstructor("YRightAxis")]
	public StiYRightAxis(IStiAxisLabels labels, IStiAxisRange range, IStiAxisTitle title, IStiAxisTicks ticks, IStiAxisInteraction interaction, StiArrowStyle arrowStyle, StiPenStyle lineStyle, Color lineColor, float lineWidth, bool visible, bool startFromZero, bool allowApplyStyle, bool logarithmicScale)
		: base(labels, range, title, ticks, interaction, arrowStyle, lineStyle, lineColor, lineWidth, visible, startFromZero, StiShowYAxis.Both, allowApplyStyle, logarithmicScale)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		base.Labels.TextAlignment = (StiHorAlignment)1;
		base.Core = new StiYRightAxisCoreXF(this);
	}
}
