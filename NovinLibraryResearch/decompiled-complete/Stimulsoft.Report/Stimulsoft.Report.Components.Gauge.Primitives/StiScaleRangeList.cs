using System.Collections;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Gauge.Collections;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Components.Gauge.Primitives;

public abstract class StiScaleRangeList : StiGaugeElement
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiScaleRangeList;

	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public StiRangeCollection Ranges { get; set; }

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyJObject(val, "Ranges", Ranges.SaveToJsonObject(mode));
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (name == "Ranges")
			{
				Ranges.LoadFromJsonObject((JObject)item.Value);
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[1] { propertiesHelper.AllowApplyStyle() };
		stiPropertyCollection.Add(StiPropertyCategories.Misc, value);
		return stiPropertyCollection;
	}

	public override object Clone()
	{
		StiScaleRangeList stiScaleRangeList = (StiScaleRangeList)base.Clone();
		stiScaleRangeList.Ranges = new StiRangeCollection(stiScaleRangeList);
		lock (((ICollection)Ranges).SyncRoot)
		{
			foreach (StiRangeBase range in Ranges)
			{
				stiScaleRangeList.Ranges.Add((StiRangeBase)range.Clone());
			}
			return stiScaleRangeList;
		}
	}

	public StiScaleRangeList()
	{
		Ranges = new StiRangeCollection(this);
	}
}
