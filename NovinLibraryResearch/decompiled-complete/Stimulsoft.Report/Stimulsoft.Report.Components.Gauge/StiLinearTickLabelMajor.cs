using System.Collections.Generic;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Report.Components.Gauge.Primitives;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Components.Gauge;

public class StiLinearTickLabelMajor : StiLinearTickLabelBase
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiLinearTickLabelMajor;

	public override string LocalizeName => "LinearTickLabelMajor";

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyIdent(val, "Ident", GetType().Name);
		return val;
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
		value = new StiPropertyObject[1] { propertiesHelper.Placement() };
		stiPropertyCollection.Add(StiPropertyCategories.Tick, value);
		value = new StiPropertyObject[3]
		{
			propertiesHelper.Font(),
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
			base.TextBrush = style.Core.TickLabelMajorTextBrush;
			base.Font = style.Core.TickLabelMajorFont;
		}
	}

	public override StiGaugeElement CreateNew()
	{
		return new StiLinearTickLabelMajor();
	}

	protected override Dictionary<float, float> GetPointCollection()
	{
		return GetMajorCollections();
	}
}
