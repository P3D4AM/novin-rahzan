using System.Collections;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Gauge.Collections;

namespace Stimulsoft.Report.Components.Gauge.Primitives;

public abstract class StiBarBase : StiIndicatorBase
{
	private bool useRangeColor;

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(210)]
	[Description("Gets or sets the brush for the empty part of the bar indicator.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiBrush EmptyBrush { get; set; } = (StiBrush)new StiEmptyBrush();


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(220)]
	[Description("Gets or sets the border brush for the empty part of the bar indicator.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiBrush EmptyBorderBrush { get; set; } = (StiBrush)new StiEmptyBrush();


	[StiSerializable]
	[DefaultValue(0f)]
	[StiOrder(230)]
	[Description("Gets or sets the border width for the empty part of the bar indicator.")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public float EmptyBorderWidth { get; set; }

	[DefaultValue(0f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the offset ratio of an item.")]
	public float Offset { get; set; }

	[DefaultValue(0f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets start width of the bar indicator.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public float StartWidth { get; set; } = 0.05f;


	[DefaultValue(0f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets end width of the bar indicator.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public float EndWidth { get; set; } = 0.05f;


	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates whether we should use Brush property of the StiIndicatorBase class as brush for this indicator.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public bool UseRangeColor
	{
		get
		{
			return useRangeColor;
		}
		set
		{
			useRangeColor = value;
			OnRangeColorChanged();
		}
	}

	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public StiBarRangeListCollection RangeList { get; set; }

	[Browsable(false)]
	protected abstract StiBarRangeListType BarType { get; }

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyBrush(val, "EmptyBrush", EmptyBrush);
		JsonExtensions.AddPropertyBrush(val, "EmptyBorderBrush", EmptyBorderBrush);
		JsonExtensions.AddPropertyFloat(val, "EmptyBorderWidth", EmptyBorderWidth, 0f);
		JsonExtensions.AddPropertyFloat(val, "Offset", Offset, 0f);
		JsonExtensions.AddPropertyFloat(val, "StartWidth", StartWidth, 0f);
		JsonExtensions.AddPropertyFloat(val, "EndWidth", EndWidth, 0.05f);
		JsonExtensions.AddPropertyBool(val, "UseRangeColor", UseRangeColor, false, false);
		JsonExtensions.AddPropertyJObject(val, "RangeList", RangeList.SaveToJsonObject(mode));
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "EmptyBrush":
				EmptyBrush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "EmptyBorderBrush":
				EmptyBorderBrush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "EmptyBorderWidth":
				EmptyBorderWidth = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "Offset":
				Offset = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "StartWidth":
				StartWidth = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "EndWidth":
				EndWidth = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "UseRangeColor":
				UseRangeColor = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "RangeList":
				RangeList.LoadFromJsonObject((JObject)item.Value);
				break;
			}
		}
	}

	public override object Clone()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		StiBarBase stiBarBase = (StiBarBase)base.Clone();
		stiBarBase.EmptyBrush = (StiBrush)EmptyBrush.Clone();
		stiBarBase.EmptyBorderBrush = (StiBrush)EmptyBorderBrush.Clone();
		stiBarBase.RangeList = new StiBarRangeListCollection(BarType);
		lock (((ICollection)RangeList).SyncRoot)
		{
			foreach (StiIndicatorRangeInfo range in RangeList)
			{
				stiBarBase.RangeList.Add((StiIndicatorRangeInfo)range.Clone());
			}
			return stiBarBase;
		}
	}

	protected abstract void OnRangeColorChanged();

	protected abstract void CheckActualBrushForTopGeometry();

	protected override void OnValueChanged()
	{
		CheckActualBrushForTopGeometry();
	}

	public StiBarBase()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		RangeList = new StiBarRangeListCollection(BarType);
	}
}
