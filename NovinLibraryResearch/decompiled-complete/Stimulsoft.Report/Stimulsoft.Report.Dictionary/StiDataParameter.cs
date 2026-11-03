using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dictionary.Design;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Dictionary;

[StiSerializable]
[TypeConverter(typeof(StiDataParameterConverter))]
public class StiDataParameter : StiExpression, IStiName, IStiInherited, IStiPropertyGridObject
{
	public enum Order
	{
		Name = 100,
		Expression = 200,
		Size = 300,
		Type = 400
	}

	internal StiDataParametersCollection dataParametersCollection;

	[Browsable(false)]
	public StiComponentId ComponentId => StiComponentId.StiDataParameter;

	[Browsable(false)]
	public string PropName => Name;

	[Browsable(false)]
	[DefaultValue(false)]
	public bool Inherited
	{
		get
		{
			if (DataSource != null)
			{
				return DataSource.Inherited;
			}
			return false;
		}
		set
		{
		}
	}

	[DefaultValue("Parameter")]
	[RefreshProperties(RefreshProperties.All)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a parameter name.")]
	[ParenthesizePropertyName(true)]
	[StiOrder(100)]
	public string Name { get; set; }

	[Browsable(false)]
	public override bool ApplyFormat => false;

	[Browsable(false)]
	[StiNonSerialized]
	public override string Value
	{
		get
		{
			return base.Value;
		}
		set
		{
			base.Value = value;
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Report.Components.Design.StiExpressionEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiOrder(200)]
	[Description("Gets or sets expression of the calculated column.")]
	public string Expression
	{
		get
		{
			return Value;
		}
		set
		{
			Value = value;
		}
	}

	[Browsable(false)]
	public object ParameterValue { get; set; }

	[Browsable(false)]
	public StiDataSource DataSource { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiDataParameterTypeConverter))]
	[Editor("Stimulsoft.Report.Dictionary.Design.StiDataParameterTypeEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiOrder(400)]
	[Description("Gets or sets type of parameter.")]
	public virtual int Type { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(300)]
	[Description("Gets or sets a size of the parameter.")]
	public virtual int Size { get; set; }

	[DefaultValue(null)]
	[StiSerializable]
	[Browsable(false)]
	public string Key { get; set; }

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "Value");
		JsonExtensions.AddPropertyString(val, "Name", Name);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Expression", Expression);
		JsonExtensions.AddPropertyInt(val, "Type", Type, 0);
		JsonExtensions.AddPropertyInt(val, "Size", Size, 0);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Key", Key);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Name":
				Name = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Expression":
				Expression = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Type":
				Type = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "Size":
				Size = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "Key":
				Key = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			}
		}
	}

	public StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[4]
		{
			propertiesHelper.Expression(),
			propertiesHelper.Name(),
			propertiesHelper.DataParameterSize(),
			propertiesHelper.DataParameterType()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Data, value);
		return stiPropertyCollection;
	}

	public StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		return null;
	}

	public object GetParameterValue()
	{
		Hashtable hashtable = null;
		if (DataSource != null && DataSource.Dictionary != null && DataSource.Dictionary.Report != null)
		{
			hashtable = DataSource.Dictionary.Report.Variables;
		}
		object obj;
		if (hashtable != null && hashtable.ContainsKey(Name))
		{
			obj = hashtable[Name];
			if (obj is string && (string)obj == "#null#")
			{
				obj = null;
			}
		}
		else
		{
			obj = ParameterValue;
		}
		return obj;
	}

	public override string ToString()
	{
		return Name;
	}

	public StiDataParameter()
		: this("Parameter", 0, 0)
	{
	}

	public StiDataParameter(string name, int type, int size)
		: this(name, string.Empty, type, size)
	{
	}

	public StiDataParameter(string name, int type, int size, string key)
		: this(name, string.Empty, type, size, key)
	{
	}

	public StiDataParameter(string name, string value, int type, int size)
	{
		Type = type;
		Name = name;
		Value = value;
		Size = size;
	}

	public StiDataParameter(string name, string value, int type, int size, string key)
	{
		Type = type;
		Name = name;
		Value = value;
		Size = size;
		Key = key;
	}
}
