using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Dictionary.Design;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiCsvSourceConverter))]
public class StiCsvSource : StiFileDataSource
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiCsvSource;

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(null)]
	[Description("Gets or sets a list separator.")]
	public string Separator { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(true)]
	[Description("Gets or sets a value indicating whether it is necessary to convert empty string to null.")]
	public bool ConvertEmptyStringToNull { get; set; }

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Path", base.Path);
		JsonExtensions.AddPropertyInt(val, "CodePage", base.CodePage, 0);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Separator", Separator);
		JsonExtensions.AddPropertyBool(val, "ConvertEmptyStringToNull", ConvertEmptyStringToNull, true, false);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Path":
				base.Path = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "CodePage":
				base.CodePage = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "Separator":
				Separator = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "ConvertEmptyStringToNull":
				ConvertEmptyStringToNull = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[8]
		{
			propertiesHelper.Name(),
			propertiesHelper.Alias(),
			propertiesHelper.NameInSource(),
			propertiesHelper.ConnectOnStart(),
			propertiesHelper.Path(),
			propertiesHelper.CodePage(),
			propertiesHelper.Separator(),
			propertiesHelper.ConvertEmptyStringToNull()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Data, value);
		return stiPropertyCollection;
	}

	protected override Type GetDataAdapterType()
	{
		return typeof(StiCsvAdapterService);
	}

	public override StiDataSource CreateNew()
	{
		return new StiCsvSource();
	}

	public StiCsvSource()
		: this("", "", "", 0, null)
	{
	}

	public StiCsvSource(string path, string name, string alias, int codePage)
		: this(path, name, alias, codePage, null)
	{
	}

	public StiCsvSource(string path, string name, string alias, int codePage, string separator)
		: base(path, name, alias, codePage)
	{
		ConvertEmptyStringToNull = true;
		Separator = separator;
	}

	public StiCsvSource(string path, string name, string alias, int codePage, string separator, string key)
		: base(path, name, alias, codePage, key)
	{
		ConvertEmptyStringToNull = true;
		Separator = separator;
	}
}
