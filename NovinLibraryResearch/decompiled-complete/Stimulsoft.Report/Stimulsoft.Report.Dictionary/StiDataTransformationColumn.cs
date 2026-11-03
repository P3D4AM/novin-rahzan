using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Dictionary;

public class StiDataTransformationColumn : StiDataColumn
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiDataTransformation;

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

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Report.Components.Design.StiExpressionEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiOrder(400)]
	[Description("Gets or sets an expression of the calculated column.")]
	public string Expression { get; set; }

	[DefaultValue(StiDataTransformationMode.Dimension)]
	[StiSerializable]
	[Browsable(false)]
	public StiDataTransformationMode Mode { get; set; }

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyIdent(val, "Ident", "Transform");
		JsonExtensions.AddPropertyString(val, "Expression", Expression);
		JsonExtensions.AddPropertyEnum(val, "Mode", (Enum)Mode, (Enum)StiDataTransformationMode.Dimension);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string text = item.Name;
			if (!(text == "Expression"))
			{
				if (text == "Mode")
				{
					Mode = StiJsonReportObjectHelper.DeserializeEnum<StiDataTransformationMode>(item);
				}
			}
			else
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

	public StiDataColumn GetDictionaryColumn()
	{
		StiDataTransformation stiDataTransformation = (StiDataTransformation)base.DataSource;
		if (stiDataTransformation != null)
		{
			string columnPath = StiExpressionHelper.RemoveFunction(Expression);
			if (stiDataTransformation.GetDataSources(new string[1] { columnPath }).FirstOrDefault() is StiDataSource stiDataSource)
			{
				return stiDataSource.Columns.ToList().FirstOrDefault((StiDataColumn c) => c.GetColumnPath().ToLowerInvariant() == columnPath.ToLowerInvariant());
			}
		}
		return null;
	}

	public StiDataTransformationColumn()
		: this("DataTransformationColumn", "DataTransformationColumn", typeof(int), "", "", StiDataTransformationMode.Dimension)
	{
	}

	public StiDataTransformationColumn(string name, string alias, Type type, string expression)
		: base(name, alias, type)
	{
		Expression = expression;
	}

	public StiDataTransformationColumn(string name, string alias, Type type, string expression, string key, StiDataTransformationMode mode)
		: base(name, alias, type)
	{
		Expression = expression;
		base.Key = key;
		Mode = mode;
	}
}
