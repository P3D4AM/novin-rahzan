using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components.Design;

namespace Stimulsoft.Report.Components;

[TypeConverter(typeof(StiDrillDownParameterConverter))]
[RefreshProperties(RefreshProperties.All)]
public class StiDrillDownParameter : IStiDefault, IStiJsonReportObject, ICloneable
{
	[Browsable(false)]
	public bool IsDefault
	{
		get
		{
			if (string.IsNullOrEmpty(Name))
			{
				if (Expression != null)
				{
					return Expression.Value.Length == 0;
				}
				return true;
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
	public StiExpression Expression { get; set; } = new StiExpression();


	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		JObject val = new JObject();
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Name", Name);
		JsonExtensions.AddPropertyJObject(val, "Expression", Expression.SaveToJsonObject(mode));
		if (((JContainer)val).Count == 0)
		{
			return null;
		}
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (!(name == "Name"))
			{
				if (name == "Expression")
				{
					Expression.LoadFromJsonObject((JObject)item.Value);
				}
			}
			else
			{
				Name = StiJsonReportObjectHelper.DeserializeString(item);
			}
		}
	}

	public object Clone()
	{
		StiDrillDownParameter stiDrillDownParameter = MemberwiseClone() as StiDrillDownParameter;
		stiDrillDownParameter.Expression = Expression.Clone() as StiExpression;
		return stiDrillDownParameter;
	}

	[StiUniversalConstructor("")]
	public StiDrillDownParameter()
	{
	}

	public StiDrillDownParameter(string name)
	{
		Name = name;
	}
}
