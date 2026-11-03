using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Dictionary.Design;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiDataStoreSourceConverter))]
public abstract class StiDataStoreSource : StiDataSource
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiDataStoreSource;

	[StiNonSerialized]
	[Browsable(false)]
	[Obsolete("Please instead property DataName use property NameInSource.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public string DataName
	{
		get
		{
			return NameInSource;
		}
		set
		{
			NameInSource = value;
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the name of Data Source in source of data.")]
	[StiOrder(100)]
	public virtual string NameInSource { get; set; }

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "NameInSource", NameInSource);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string text = item.Name;
			if (text == "NameInSource")
			{
				NameInSource = StiJsonReportObjectHelper.DeserializeString(item);
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[4]
		{
			propertiesHelper.Name(),
			propertiesHelper.Alias(),
			propertiesHelper.NameInSource(),
			propertiesHelper.ConnectOnStart()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Data, value);
		return stiPropertyCollection;
	}

	public override string GetCategoryName()
	{
		StiDataAdapterService dataAdapter = StiDataAdapterService.GetDataAdapter(this);
		if (base.Dictionary != null)
		{
			StiData stiData = base.Dictionary.DataStore[NameInSource];
			if (stiData != null && dataAdapter != null)
			{
				return dataAdapter.GetDataCategoryName(stiData);
			}
		}
		if (StringExt.EndsWithInvariant(NameInSource, "." + base.Name) && NameInSource.Length > base.Name.Length + 1)
		{
			return NameInSource.Substring(0, NameInSource.Length - (base.Name.Length + 1));
		}
		int num = NameInSource.LastIndexOf(".", StringComparison.InvariantCulture);
		if (num != -1)
		{
			return NameInSource.Substring(0, num);
		}
		if (!string.IsNullOrEmpty(NameInSource))
		{
			return NameInSource;
		}
		return base.GetCategoryName();
	}

	public StiDataStoreSource()
		: this("", "", "")
	{
	}

	public StiDataStoreSource(string nameInSource, string name)
		: this(nameInSource, name, name)
	{
	}

	public StiDataStoreSource(string nameInSource, string name, string alias)
		: base(name, alias)
	{
		NameInSource = nameInSource;
	}

	public StiDataStoreSource(string nameInSource, string name, string alias, string key)
		: base(name, alias, key)
	{
		NameInSource = nameInSource;
	}
}
