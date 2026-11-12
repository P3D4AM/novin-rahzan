using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Dashboard.Interactions.Design;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Interactions;

[TypeConverter(typeof(StiDashboardDrillDownParameterConverter))]
[JsonConverter(typeof(StiDashboardDrillDownParameterJsonConverter))]
[RefreshProperties(RefreshProperties.All)]
public class StiDashboardDrillDownParameter : IStiDefault, ICloneable, IStiJsonReportObject, IStiDashboardDrillDownParameter
{
	[Browsable(false)]
	public bool IsDefault
	{
		get
		{
			if (string.IsNullOrEmpty(Name))
			{
				return string.IsNullOrEmpty(Expression);
			}
			return false;
		}
	}

	[DefaultValue("")]
	[StiSerializable]
	[StiOrder(100)]
	[RefreshProperties(RefreshProperties.All)]
	[Description("Gets or sets name of drill-down parameter.")]
	public string Name { get; set; } = string.Empty;


	[StiSerializable]
	[StiOrder(200)]
	[RefreshProperties(RefreshProperties.All)]
	[Description("Gets or sets expression to fill drill-down parameter.")]
	public string Expression { get; set; } = string.Empty;


	public object Clone()
	{
		return MemberwiseClone();
	}

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = new JObject();
		jObject.AddPropertyStringNullOrEmpty("Name", Name);
		jObject.AddPropertyStringNullOrEmpty("Expression", Expression);
		if (jObject.Count == 0)
		{
			return null;
		}
		return jObject;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (!(name == "Name"))
			{
				if (name == "Expression")
				{
					Expression = item.DeserializeString();
				}
			}
			else
			{
				Name = item.DeserializeString();
			}
		}
	}

	public static StiDashboardDrillDownParameter LoadFromJson(string json)
	{
		StiDashboardDrillDownParameter stiDashboardDrillDownParameter = new StiDashboardDrillDownParameter();
		stiDashboardDrillDownParameter.LoadFromJsonObject(JObject.Parse(json));
		return stiDashboardDrillDownParameter;
	}

	public static StiDashboardDrillDownParameter LoadFromJson(JObject json)
	{
		StiDashboardDrillDownParameter stiDashboardDrillDownParameter = new StiDashboardDrillDownParameter();
		stiDashboardDrillDownParameter.LoadFromJsonObject(json);
		return stiDashboardDrillDownParameter;
	}

	public List<StiDashboardDrillDownParameter> ToList()
	{
		return new List<StiDashboardDrillDownParameter> { this };
	}

	public static List<StiDashboardDrillDownParameter> ToList(StiDashboardDrillDownParameter[] parameters)
	{
		return parameters?.ToList();
	}

	public virtual string SaveToString()
	{
		return JsonConvert.SerializeObject(this, Formatting.Indented, StiJsonHelper.DefaultSerializerSettings);
	}

	public string GetStringRepresentation()
	{
		if (string.IsNullOrWhiteSpace(Name))
		{
			return Loc.Get("FormStyleDesigner", "NotSpecified");
		}
		if (!string.IsNullOrWhiteSpace(Expression))
		{
			return Name + " - " + Expression;
		}
		return Name;
	}

	[StiUniversalConstructor("")]
	public StiDashboardDrillDownParameter()
	{
	}

	public StiDashboardDrillDownParameter(string name, string expression)
	{
		Name = name;
		Expression = expression;
	}
}
