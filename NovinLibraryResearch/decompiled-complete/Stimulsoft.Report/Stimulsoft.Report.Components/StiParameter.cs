using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components.Design;

namespace Stimulsoft.Report.Components;

[TypeConverter(typeof(StiParameterConverter))]
[RefreshProperties(RefreshProperties.All)]
public class StiParameter : IStiDefault, IStiJsonReportObject
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
	[Description("Gets or sets name of parameter.")]
	public string Name { get; set; } = string.Empty;


	[StiSerializable]
	[StiOrder(200)]
	[RefreshProperties(RefreshProperties.All)]
	[Description("Gets or sets expression to fill parameter.")]
	public StiExpression Expression { get; set; } = new StiExpression();


	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		JObject val = new JObject();
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Name", Name);
		JsonExtensions.AddPropertyJObject(val, "Expression", Expression.SaveToJsonObject(mode));
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

	public override string ToString()
	{
		if (Name == null)
		{
			return GetType().ToString();
		}
		return Name;
	}

	[StiUniversalConstructor("")]
	public StiParameter()
	{
	}
}
