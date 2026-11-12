using System;
using System.ComponentModel;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Base;

[TypeConverter(typeof(StiAppExpressionConverter))]
[StiSerializable]
public class StiAppExpression : ICloneable, IStiJsonReportObject
{
	[StiSerializable]
	public string Name { get; set; }

	[StiSerializable]
	public string Expression { get; set; }

	public bool IsEmpty
	{
		get
		{
			if (!string.IsNullOrWhiteSpace(Name))
			{
				return string.IsNullOrWhiteSpace(Expression);
			}
			return true;
		}
	}

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = new JObject();
		jObject.AddPropertyStringNullOrEmpty("Name", Name);
		jObject.AddPropertyStringNullOrEmpty("Expression", Expression);
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

	public object Clone()
	{
		return new StiAppExpression(Name, Expression);
	}

	public StiAppExpression(string name, string expression)
	{
		Name = name;
		Expression = expression;
	}
}
