using System.Collections.Generic;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components.Gauge.Primitives;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Components.Gauge;

public class StiLinearTickLabelMinor : StiLinearTickLabelBase
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiLinearTickLabelMinor;

	[StiSerializable]
	[DefaultValue(true)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets whether to pass major values.")]
	public bool SkipMajorValues { get; set; } = true;


	protected override bool IsSkipMajorValues => SkipMajorValues;

	public override string LocalizeName => "LinearTickLabelMinor";

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyIdent(val, "Ident", GetType().Name);
		JsonExtensions.AddPropertyBool(val, "SkipMajorValues", SkipMajorValues, true, false);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (name == "SkipMajorValues")
			{
				SkipMajorValues = StiJsonReportObjectHelper.DeserializeBool(item);
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[5]
		{
			propertiesHelper.NullableMaximumValue(),
			propertiesHelper.NullableMinimumValue(),
			propertiesHelper.Offset(),
			propertiesHelper.SkipIndices(),
			propertiesHelper.SkipValues()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Value, value);
		value = new StiPropertyObject[4]
		{
			propertiesHelper.Font(),
			propertiesHelper.SkipMajorValues(),
			propertiesHelper.TextBrush(),
			propertiesHelper.TextFormatStr()
		};
		stiPropertyCollection.Add(StiPropertyCategories.TextAdditional, value);
		value = new StiPropertyObject[1] { propertiesHelper.AllowApplyStyle() };
		stiPropertyCollection.Add(StiPropertyCategories.Misc, value);
		return stiPropertyCollection;
	}

	public override void ApplyStyle(IStiGaugeStyle style)
	{
		if (AllowApplyStyle)
		{
			base.TextBrush = style.Core.TickLabelMinorTextBrush;
			base.Font = style.Core.TickLabelMinorFont;
		}
	}

	public override StiGaugeElement CreateNew()
	{
		return new StiLinearTickLabelMinor();
	}

	protected override Dictionary<float, float> GetPointCollection()
	{
		return GetMinorCollections();
	}
}
