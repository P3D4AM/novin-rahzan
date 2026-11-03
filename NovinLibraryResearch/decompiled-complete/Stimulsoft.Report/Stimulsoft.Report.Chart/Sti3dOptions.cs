using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Chart.Design;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

[TypeConverter(typeof(Sti3dOptionsConverter))]
public class Sti3dOptions : IStiSerializeToCodeAsClass, IStiPropertyGridObject, IStiDefault
{
	private float opacity = 1f;

	[Browsable(false)]
	public StiComponentId ComponentId => StiComponentId.Sti3dOptions;

	[Browsable(false)]
	public string PropName => string.Empty;

	[Browsable(false)]
	public virtual bool IsDefault => Opacity == 1f;

	[StiSerializable]
	[DefaultValue(1)]
	public float Opacity
	{
		get
		{
			return opacity;
		}
		set
		{
			if (value >= 0f && value <= 1f)
			{
				opacity = value;
			}
		}
	}

	public virtual JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		JObject val = new JObject();
		JsonExtensions.AddPropertyFloat(val, "Opacity", Opacity, 1f);
		return val;
	}

	public virtual void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (name == "Opacity")
			{
				Opacity = StiJsonReportObjectHelper.DeserializeFloat(item);
			}
		}
	}

	public StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		return new StiPropertyCollection();
	}

	public StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		return null;
	}
}
