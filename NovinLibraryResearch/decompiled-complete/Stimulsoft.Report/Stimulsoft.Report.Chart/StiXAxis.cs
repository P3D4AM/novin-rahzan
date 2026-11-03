using System;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public abstract class StiXAxis : StiAxis, IStiXAxis, IStiAxis, ICloneable, IStiJsonReportObject, IStiDefault, IStiPropertyGridObject
{
	[Browsable(false)]
	public virtual StiComponentId ComponentId => StiComponentId.StiXAxis;

	[Browsable(false)]
	public string PropName => string.Empty;

	public override bool IsDefault
	{
		get
		{
			if (base.IsDefault && !ShowEdgeValues && !ShouldSerializeShowXAxis())
			{
				return !ShouldSerializeDateTimeStep();
			}
			return false;
		}
	}

	[DefaultValue(false)]
	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that first and last arguments on axis will be shown anyway.")]
	public bool ShowEdgeValues { get; set; }

	[StiSerializable]
	[DefaultValue(StiShowXAxis.Both)]
	[Description("Gets or sets type of drawing X axis.")]
	[TypeConverter(typeof(StiEnumConverter))]
	public virtual StiShowXAxis ShowXAxis { get; set; } = StiShowXAxis.Both;


	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiUniversalConverter))]
	[Description("Gets or sets date time step settings.")]
	public virtual IStiAxisDateTimeStep DateTimeStep { get; set; } = new StiAxisDateTimeStep();


	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyBool(val, "ShowEdgeValues", ShowEdgeValues, false, false);
		JsonExtensions.AddPropertyEnum(val, "ShowXAxis", (Enum)ShowXAxis, (Enum)StiShowXAxis.Both);
		JsonExtensions.AddPropertyJObject(val, "DateTimeStep", ((IStiJsonReportObject)DateTimeStep).SaveToJsonObject(mode));
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "ShowEdgeValues":
				ShowEdgeValues = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "ShowXAxis":
				ShowXAxis = StiJsonReportObjectHelper.DeserializeEnum<StiShowXAxis>(item);
				break;
			case "DateTimeStep":
				((IStiJsonReportObject)DateTimeStep).LoadFromJsonObject((JObject)item.Value);
				break;
			}
		}
	}

	public virtual StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		stiPropertyCollection.Add(StiPropertyCategories.Main, new StiPropertyObject[1] { propertyGrid.PropertiesHelper.XAxis() });
		return stiPropertyCollection;
	}

	public StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		return null;
	}

	protected virtual bool ShouldSerializeShowXAxis()
	{
		return ShowXAxis != StiShowXAxis.Both;
	}

	protected virtual bool ShouldSerializeDateTimeStep()
	{
		if (DateTimeStep != null)
		{
			return !((IStiDefault)DateTimeStep).IsDefault;
		}
		return true;
	}

	public StiXAxis()
	{
	}

	public StiXAxis(IStiAxisLabels labels, IStiAxisRange range, IStiAxisTitle title, IStiAxisTicks ticks, StiArrowStyle arrowStyle, StiPenStyle lineStyle, Color lineColor, float lineWidth, bool visible, bool startFromZero, StiShowXAxis showXAxis, bool allowApplyStyle)
		: this(labels, range, title, ticks, arrowStyle, lineStyle, lineColor, lineWidth, visible, startFromZero, showXAxis, showEdgeValues: false, allowApplyStyle)
	{
	}//IL_0008: Unknown result type (might be due to invalid IL or missing references)


	public StiXAxis(IStiAxisLabels labels, IStiAxisRange range, IStiAxisTitle title, IStiAxisTicks ticks, StiArrowStyle arrowStyle, StiPenStyle lineStyle, Color lineColor, float lineWidth, bool visible, bool startFromZero, StiShowXAxis showXAxis, bool showEdgeValues, bool allowApplyStyle)
		: base(labels, range, title, ticks, arrowStyle, lineStyle, lineColor, lineWidth, visible, startFromZero, allowApplyStyle)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		ShowXAxis = showXAxis;
		ShowEdgeValues = showEdgeValues;
	}

	public StiXAxis(IStiAxisLabels labels, IStiAxisRange range, IStiAxisTitle title, IStiAxisTicks ticks, IStiAxisInteraction interaction, StiArrowStyle arrowStyle, StiPenStyle lineStyle, Color lineColor, float lineWidth, bool visible, bool startFromZero, StiShowXAxis showXAxis, bool showEdgeValues, bool allowApplyStyle)
		: base(labels, range, title, ticks, interaction, arrowStyle, lineStyle, lineColor, lineWidth, visible, startFromZero, allowApplyStyle)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		ShowXAxis = showXAxis;
		DateTimeStep = new StiAxisDateTimeStep();
		ShowEdgeValues = showEdgeValues;
	}

	public StiXAxis(IStiAxisLabels labels, IStiAxisRange range, IStiAxisTitle title, IStiAxisTicks ticks, IStiAxisInteraction interaction, StiArrowStyle arrowStyle, StiPenStyle lineStyle, Color lineColor, float lineWidth, bool visible, bool startFromZero, StiShowXAxis showXAxis, bool showEdgeValues, bool allowApplyStyle, IStiAxisDateTimeStep dateTimeStep)
		: base(labels, range, title, ticks, interaction, arrowStyle, lineStyle, lineColor, lineWidth, visible, startFromZero, allowApplyStyle)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		ShowXAxis = showXAxis;
		DateTimeStep = dateTimeStep;
		ShowEdgeValues = showEdgeValues;
	}

	public StiXAxis(IStiAxisLabels labels, IStiAxisRange range, IStiAxisTitle title, IStiAxisTicks ticks, IStiAxisInteraction interaction, StiArrowStyle arrowStyle, StiPenStyle lineStyle, Color lineColor, float lineWidth, bool visible, bool startFromZero, StiShowXAxis showXAxis, bool showEdgeValues, bool allowApplyStyle, IStiAxisDateTimeStep dateTimeStep, bool logarithmicScale)
		: base(labels, range, title, ticks, interaction, arrowStyle, lineStyle, lineColor, lineWidth, visible, startFromZero, allowApplyStyle, logarithmicScale)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		ShowXAxis = showXAxis;
		DateTimeStep = dateTimeStep;
		ShowEdgeValues = showEdgeValues;
	}
}
