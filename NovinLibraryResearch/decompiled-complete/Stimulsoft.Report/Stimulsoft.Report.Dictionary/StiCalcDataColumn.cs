using System;
using System.ComponentModel;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Dictionary;

public class StiCalcDataColumn : StiDataColumn, IStiAppCalcDataColumn, IStiAppDataColumn, IStiAppDataCell, IStiAppCell
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiCalcDataColumn;

	[Browsable(false)]
	public override string NameInSource
	{
		get
		{
			return base.NameInSource;
		}
		set
		{
			base.NameInSource = value;
		}
	}

	[DefaultValue("Column")]
	[RefreshProperties(RefreshProperties.All)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[StiNonSerialized]
	public string Value { get; set; }

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Report.Components.Design.StiExpressionEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiOrder(400)]
	[Description("Gets or sets an expression of the calculated column.")]
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

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyIdent(val, "Ident", "Calc");
		JsonExtensions.AddPropertyString(val, "Expression", Expression);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string text = item.Name;
			if (text == "Expression")
			{
				Expression = StiJsonReportObjectHelper.DeserializeString(item);
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[5]
		{
			propertiesHelper.Alias(),
			propertiesHelper.Name(),
			propertiesHelper.NameInSource(),
			propertiesHelper.StringExpression(),
			propertiesHelper.Type()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Data, value);
		return stiPropertyCollection;
	}

	public StiCalcDataColumn()
		: this("CalcColumn", "CalcColumn", typeof(int), "")
	{
	}

	public StiCalcDataColumn(string name, Type type)
		: this(name, name, type)
	{
	}

	public StiCalcDataColumn(string name, string alias, Type type)
		: base(name, alias, type)
	{
	}

	public StiCalcDataColumn(string name, string alias, Type type, string value)
		: base(name, alias, type)
	{
		Value = value;
	}

	public StiCalcDataColumn(string name, string alias, Type type, string value, string key)
		: base(name, alias, type)
	{
		Value = value;
		base.Key = key;
	}
}
