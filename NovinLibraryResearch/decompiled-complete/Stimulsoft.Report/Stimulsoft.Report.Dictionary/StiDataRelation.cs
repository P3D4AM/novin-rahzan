using System;
using System.Collections.Generic;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Dictionary.Design;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiDataRelationConverter))]
public class StiDataRelation : ICloneable, IStiName, IStiAlias, IStiInherited, IStiPropertyGridObject, IStiJsonReportObject, IStiAppDataRelation, IStiAppCell
{
	public enum Order
	{
		NameInSource = 100,
		Name = 200,
		Alias = 300,
		Active = 350,
		ParentSource = 400,
		ChildSource = 500,
		ParentColumns = 600,
		ChildColumns = 700
	}

	private string name;

	[Browsable(false)]
	public StiComponentId ComponentId => StiComponentId.StiDataRelation;

	[Browsable(false)]
	public string PropName => name;

	[Browsable(false)]
	[DefaultValue(false)]
	[StiSerializable]
	public bool Inherited { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets relation name.")]
	[ParenthesizePropertyName(true)]
	[StiOrder(200)]
	public string Name
	{
		get
		{
			return name;
		}
		set
		{
			if (Dictionary != null && Dictionary.Report != null && Dictionary.Report.IsDesigning && StiOptions.Designer.AutoCorrectDataRelationName)
			{
				value = StiNameValidator.CorrectName(value, Dictionary.Report);
			}
			name = value;
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets alias of relation.")]
	[ParenthesizePropertyName(true)]
	[StiOrder(300)]
	public string Alias { get; set; }

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	public StiDictionary Dictionary { get; set; }

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets Parent data source.")]
	[StiOrder(400)]
	public StiDataSource ParentSource { get; set; }

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets Child data source.")]
	[StiOrder(500)]
	public StiDataSource ChildSource { get; set; }

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets collection of child column names.")]
	[TypeConverter(typeof(StiChildColumnsConverter))]
	[StiOrder(700)]
	public string[] ChildColumns { get; set; }

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets collection of parent column names.")]
	[TypeConverter(typeof(StiParentColumnsConverter))]
	[StiOrder(600)]
	public string[] ParentColumns { get; set; }

	[StiNonSerialized]
	[Browsable(false)]
	[Obsolete("Please instead property RelationName use property NameInSource.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public string RelationName
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
	[Description("Gets or sets name of relation in source of data.")]
	[StiOrder(100)]
	[ParenthesizePropertyName(true)]
	public string NameInSource { get; set; }

	[Browsable(false)]
	[DefaultValue(false)]
	[StiSerializable]
	public bool IsCloud { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets value which indicates that this data relation is active.")]
	[StiOrder(350)]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	public bool Active { get; set; }

	[DefaultValue(null)]
	[StiSerializable]
	[Browsable(false)]
	public string Key { get; set; }

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		JObject val = new JObject();
		JsonExtensions.AddPropertyBool(val, "Inherited", Inherited, false, false);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Name", Name);
		JsonExtensions.AddPropertyStringArray(val, "ChildColumns", ChildColumns);
		JsonExtensions.AddPropertyStringArray(val, "ParentColumns", ParentColumns);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "NameInSource", NameInSource);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Alias", Alias);
		JsonExtensions.AddPropertyBool(val, "IsCloud", IsCloud, false, false);
		JsonExtensions.AddPropertyBool(val, "Active", Active, false, false);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Key", Key);
		if (ParentSource != null)
		{
			JsonExtensions.AddPropertyStringNullOrEmpty(val, "ParentSource", ParentSource.Name);
		}
		if (ChildSource != null)
		{
			JsonExtensions.AddPropertyStringNullOrEmpty(val, "ChildSource", ChildSource.Name);
		}
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Inherited":
				Inherited = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Active":
				Active = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Name":
				name = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "ChildColumns":
				ChildColumns = StiJsonReportObjectHelper.DeserializeStringArray(item);
				break;
			case "ParentColumns":
				ParentColumns = StiJsonReportObjectHelper.DeserializeStringArray(item);
				break;
			case "NameInSource":
				NameInSource = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Alias":
				Alias = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "IsCloud":
			{
				bool? flag = item.Value.ToObject<bool?>();
				IsCloud = flag.HasValue && flag.Value;
				break;
			}
			case "Key":
				Key = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "ParentSource":
				ParentSource = Dictionary.DataSources[StiJsonReportObjectHelper.DeserializeString(item)];
				break;
			case "ChildSource":
				ChildSource = Dictionary.DataSources[StiJsonReportObjectHelper.DeserializeString(item)];
				break;
			}
		}
	}

	public StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[3]
		{
			propertiesHelper.NameInSource(),
			propertiesHelper.Alias(),
			propertiesHelper.Name()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Data, value);
		return stiPropertyCollection;
	}

	public StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		return null;
	}

	string IStiAppDataRelation.GetName()
	{
		return Name;
	}

	IStiAppDictionary IStiAppDataRelation.GetDictionary()
	{
		return (IStiAppDictionary)(object)Dictionary;
	}

	IStiAppDataSource IStiAppDataRelation.GetParentDataSource()
	{
		return (IStiAppDataSource)(object)ParentSource;
	}

	IStiAppDataSource IStiAppDataRelation.GetChildDataSource()
	{
		return (IStiAppDataSource)(object)ChildSource;
	}

	IEnumerable<string> IStiAppDataRelation.FetchParentColumns()
	{
		return ParentColumns;
	}

	IEnumerable<string> IStiAppDataRelation.FetchChildColumns()
	{
		return ChildColumns;
	}

	bool IStiAppDataRelation.GetActiveState()
	{
		return Active;
	}

	string IStiAppCell.GetKey()
	{
		Key = StiKeyHelper.GetOrGeneratedKey(Key);
		return Key;
	}

	void IStiAppCell.SetKey(string key)
	{
		Key = key;
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	public override string ToString()
	{
		return ToString(onlyAlias: false);
	}

	public string ToString(bool onlyAlias)
	{
		if (onlyAlias && !string.IsNullOrWhiteSpace(Alias))
		{
			return Alias;
		}
		if (Name == Alias || string.IsNullOrWhiteSpace(Alias))
		{
			return Name;
		}
		return Name + " [" + Alias + "]";
	}

	public StiDataRelation()
		: this("", null, null, new string[0], new string[0])
	{
	}

	public StiDataRelation(string nameInSource, StiDataSource parentSource, StiDataSource childSource, string[] parentColumns, string[] childColumns)
		: this(nameInSource, nameInSource, nameInSource, parentSource, childSource, parentColumns, childColumns)
	{
	}

	public StiDataRelation(string nameInSource, string name, string alias, StiDataSource parentSource, StiDataSource childSource, string[] parentColumns, string[] childColumns)
	{
		NameInSource = nameInSource;
		this.name = name;
		Alias = alias;
		ParentSource = parentSource;
		ChildSource = childSource;
		ParentColumns = parentColumns;
		ChildColumns = childColumns;
	}

	public StiDataRelation(string nameInSource, string name, string alias, StiDataSource parentSource, StiDataSource childSource, string[] parentColumns, string[] childColumns, string key)
	{
		NameInSource = nameInSource;
		this.name = name;
		Alias = alias;
		ParentSource = parentSource;
		ChildSource = childSource;
		ParentColumns = parentColumns;
		ChildColumns = childColumns;
		Key = key;
	}

	public StiDataRelation(string nameInSource, string name, string alias, StiDataSource parentSource, StiDataSource childSource, string[] parentColumns, string[] childColumns, bool active)
	{
		NameInSource = nameInSource;
		this.name = name;
		Alias = alias;
		ParentSource = parentSource;
		ChildSource = childSource;
		ParentColumns = parentColumns;
		ChildColumns = childColumns;
		Active = active;
	}

	public StiDataRelation(string nameInSource, string name, string alias, StiDataSource parentSource, StiDataSource childSource, string[] parentColumns, string[] childColumns, string key, bool active)
	{
		NameInSource = nameInSource;
		this.name = name;
		Alias = alias;
		ParentSource = parentSource;
		ChildSource = childSource;
		ParentColumns = parentColumns;
		ChildColumns = childColumns;
		Key = key;
		Active = active;
	}
}
