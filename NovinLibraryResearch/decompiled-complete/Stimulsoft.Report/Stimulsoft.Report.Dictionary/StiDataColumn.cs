using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Reflection;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dictionary.Design;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Dictionary;

[TypeConverter(typeof(StiDataColumnConverter))]
[StiSerializable]
public class StiDataColumn : ICloneable, IStiName, IStiAppDataColumn, IStiAppDataCell, IStiAppCell, IStiAppAlias, IStiInherited, IStiPropertyGridObject, IStiJsonReportObject
{
	public enum Order
	{
		NameInSource = 100,
		Name = 200,
		Alias = 300,
		Expression = 400,
		Type = 500
	}

	private string name;

	[Browsable(false)]
	public virtual StiComponentId ComponentId => StiComponentId.StiDataColumn;

	[Browsable(false)]
	public string PropName => name;

	[DefaultValue("Column")]
	[RefreshProperties(RefreshProperties.All)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a column name.")]
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
			if (DataSource?.Dictionary?.Report != null && DataSource.Dictionary.Report.IsDesigning && StiOptions.Designer.AutoCorrectDataColumnName)
			{
				value = StiNameValidator.CorrectName(value, DataSource.Dictionary.Report);
			}
			if (!(name == value))
			{
				if (name == NameInSource)
				{
					NameInSource = value;
				}
				if (name == Alias)
				{
					Alias = value;
				}
				name = value;
			}
		}
	}

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

	internal StiDataColumnsCollection DataColumnsCollection { get; set; }

	[Browsable(false)]
	public StiDataSource DataSource { get; set; }

	[Browsable(false)]
	public StiBusinessObject BusinessObject { get; set; }

	[Browsable(false)]
	public int Index { get; set; }

	[DefaultValue("Column")]
	[RefreshProperties(RefreshProperties.All)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a column original name.")]
	[ParenthesizePropertyName(true)]
	[StiOrder(100)]
	public virtual string NameInSource { get; set; } = "Column";


	[DefaultValue("Column")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets an alias of column data.")]
	[ParenthesizePropertyName(true)]
	[StiOrder(300)]
	public string Alias { get; set; }

	[DefaultValue(typeof(object))]
	[TypeConverter(typeof(StiTypeConverter))]
	[Editor("Stimulsoft.Base.Design.StiTypeEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the type of column data.")]
	[StiOrder(500)]
	public Type Type { get; set; }

	[DefaultValue(null)]
	[StiSerializable]
	[Browsable(false)]
	public string Key { get; set; }

	public virtual JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		JObject val = new JObject();
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Name", Name);
		JsonExtensions.AddPropertyInt(val, "Index", Index, 0);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "NameInSource", NameInSource);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Alias", Alias);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Type", StiDataColumnConverter.ConvertTypeToString(Type));
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Key", Key);
		return val;
	}

	public virtual void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Name":
				name = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Index":
				Index = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "NameInSource":
				NameInSource = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Alias":
				Alias = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Type":
			{
				Type type = null;
				string text = StiJsonReportObjectHelper.DeserializeString(item);
				try
				{
					type = StiTypeFinder.GetType(text);
				}
				catch
				{
				}
				Type = StiDataColumnConverter.CheckType(text, type);
				break;
			}
			case "Key":
				Key = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			}
		}
	}

	public virtual StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[4]
		{
			propertiesHelper.Alias(),
			propertiesHelper.Name(),
			propertiesHelper.NameInSource(),
			propertiesHelper.Type()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Data, value);
		return stiPropertyCollection;
	}

	public StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		return null;
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	string IStiAppDataColumn.GetNameInSource()
	{
		return NameInSource;
	}

	Type IStiAppDataColumn.GetDataType()
	{
		return Type;
	}

	string IStiAppDataCell.GetName()
	{
		return Name;
	}

	string IStiAppAlias.GetAlias()
	{
		return Alias;
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

	public string GetColumnPath()
	{
		return DataSource.Name + "." + Name;
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

	public static StiDataColumn GetDataColumnFromColumnName(StiDictionary dictionary, string column, bool allowRelationName = false)
	{
		try
		{
			if (column == null)
			{
				return null;
			}
			column = column.Trim();
			if (column.Length == 0)
			{
				return null;
			}
			string[] array = column.Split(new char[1] { '.' });
			StiDataSource stiDataSource = dictionary.DataSources[array[0]];
			if (stiDataSource == null)
			{
				return null;
			}
			int num = array.Length - 1;
			int num2 = 1;
			while (num > 0)
			{
				if (num >= 2)
				{
					stiDataSource = stiDataSource.GetParentDataSource(array[num2], allowRelationName);
					if (stiDataSource == null)
					{
						return null;
					}
					num2++;
					num--;
					continue;
				}
				if (stiDataSource.Columns.Contains(array[num2]))
				{
					return stiDataSource.Columns[array[num2]];
				}
				return null;
			}
		}
		catch
		{
		}
		return null;
	}

	public static object GetDataFromDataColumn(StiDictionary dictionary, string column)
	{
		return GetDataFromDataColumn(dictionary, column, useRelationName: true);
	}

	public static string GetRelationName(StiDictionary dictionary, StiDataSource dataSource, string relationName)
	{
		foreach (StiDataRelation relation in dictionary.Relations)
		{
			if (relation.ChildSource == dataSource && relation.Name == relationName)
			{
				return relation.NameInSource;
			}
		}
		return relationName;
	}

	public static object GetDataFromBusinessObject(StiDictionary dictionary, string column)
	{
		string[] array = column.Split(new char[1] { '.' });
		StiBusinessObject stiBusinessObject = dictionary.BusinessObjects[array[0]];
		if (stiBusinessObject == null)
		{
			return null;
		}
		for (int i = 1; i < array.Length; i++)
		{
			if (i == array.Length - 1)
			{
				return stiBusinessObject[array[i]];
			}
			stiBusinessObject = stiBusinessObject.BusinessObjects[array[i]];
		}
		return null;
	}

	public static StiBusinessObject GetBusinessObjectFromDataColumn(StiDictionary dictionary, string column)
	{
		string[] array = column.Split(new char[1] { '.' });
		StiBusinessObject stiBusinessObject = dictionary.BusinessObjects[array[0]];
		if (stiBusinessObject == null)
		{
			return null;
		}
		for (int i = 1; i < array.Length; i++)
		{
			if (i == array.Length - 1)
			{
				return stiBusinessObject;
			}
			stiBusinessObject = stiBusinessObject.BusinessObjects[array[i]];
		}
		return null;
	}

	public static object GetDataFromDataColumn(StiDictionary dictionary, string column, bool useRelationName)
	{
		try
		{
			if (column == null)
			{
				return null;
			}
			string[] array = column.Split(new char[1] { '.' });
			StiDataSource stiDataSource = dictionary.DataSources[array[0]];
			if (stiDataSource == null)
			{
				return GetDataFromBusinessObject(dictionary, column);
			}
			StiDataRow stiDataRow = null;
			int num = array.Length - 1;
			int num2 = 1;
			while (num > 0)
			{
				if (num >= 2)
				{
					string text = array[num2];
					if (!useRelationName)
					{
						text = GetRelationName(dictionary, stiDataSource, text);
					}
					stiDataRow = ((stiDataRow == null) ? stiDataSource.GetParentData(text) : stiDataRow.GetParentData(text));
					stiDataSource = stiDataSource.GetParentDataSource(text);
					num2++;
					num--;
					continue;
				}
				if (stiDataSource != null && stiDataSource.Columns[array[num2]] is StiCalcDataColumn stiCalcDataColumn)
				{
					if (dictionary.Report != null && dictionary.Report.IsDesigning)
					{
						return null;
					}
					if (dictionary.Report != null && dictionary.Report.CalculationMode == StiCalculationMode.Interpretation)
					{
						StiText component = new StiText
						{
							Name = "**StiCalcDataColumn**",
							Page = dictionary.Report.Pages[0]
						};
						return StiParser.ParseTextValue("{" + stiCalcDataColumn.Expression + "}", component);
					}
					string text2 = $"Get{StiNameValidator.CorrectName(stiCalcDataColumn.DataSource.Name, dictionary.Report)}_{StiNameValidator.CorrectName(stiCalcDataColumn.name, dictionary.Report)}";
					MethodInfo method = dictionary.Report.GetType().GetMethod(text2, new Type[0]);
					if (method != null)
					{
						return method.Invoke(dictionary.Report, new object[0]);
					}
				}
				if (stiDataRow != null)
				{
					return stiDataRow[array[num2]];
				}
				return stiDataSource?[array[num2]];
			}
		}
		catch
		{
		}
		return null;
	}

	public static StiDataSource GetDataSourceFromDataColumn(StiDictionary dictionary, string column)
	{
		try
		{
			string[] array = column.Split(new char[1] { '.' });
			return dictionary.DataSources[array[0]];
		}
		catch
		{
		}
		return null;
	}

	public static string GetColumnNameFromDataColumn(StiDictionary dictionary, string column)
	{
		try
		{
			string[] array = column.Split(new char[1] { '.' });
			return column.Substring(array[0].Length + 1);
		}
		catch
		{
		}
		return null;
	}

	public static List<object> GetDataListFromDataColumn(StiDictionary dictionary, string column, int? maxRows = null, bool firstPositionInDataSource = false)
	{
		GetDataSourceFromDataColumn(dictionary, column)?.First();
		return GetDatasFromDataColumn(dictionary, column, maxRows, firstPositionInDataSource)?.ToList();
	}

	public static object[] GetDatasFromDataColumn(StiDictionary dictionary, string column, int? maxRows = null, bool firstPositionInDataSource = false, bool useRelationName = true)
	{
		try
		{
			List<object> list = new List<object>();
			StiDataSource dataSourceFromDataColumn = GetDataSourceFromDataColumn(dictionary, column);
			if (dataSourceFromDataColumn != null)
			{
				if (firstPositionInDataSource)
				{
					dataSourceFromDataColumn.First();
				}
				int num = 0;
				while (!dataSourceFromDataColumn.IsEof)
				{
					object dataFromDataColumn = GetDataFromDataColumn(dictionary, column, useRelationName);
					if (dataFromDataColumn != null)
					{
						list.Add(dataFromDataColumn);
					}
					dataSourceFromDataColumn.Next();
					num++;
					if (maxRows.HasValue && maxRows == num)
					{
						break;
					}
				}
				dataSourceFromDataColumn.First();
			}
			StiBusinessObject businessObjectFromDataColumn = GetBusinessObjectFromDataColumn(dictionary, column);
			if (businessObjectFromDataColumn != null)
			{
				businessObjectFromDataColumn.SaveState("Totals");
				businessObjectFromDataColumn.CreateEnumerator();
				int num2 = 0;
				while (!businessObjectFromDataColumn.IsEof)
				{
					object dataFromDataColumn2 = GetDataFromDataColumn(dictionary, column);
					if (dataFromDataColumn2 != null)
					{
						list.Add(dataFromDataColumn2);
					}
					businessObjectFromDataColumn.Next();
					num2++;
					if (maxRows.HasValue && maxRows == num2)
					{
						break;
					}
				}
				businessObjectFromDataColumn.RestoreState("Totals");
			}
			return list.ToArray();
		}
		catch
		{
		}
		return new object[0];
	}

	public static object[] GetDatasFromDataSourceWithExpression(object data, string expression, int? maxRows = null, bool firstPositionInDataSource = false)
	{
		try
		{
			List<object> list = new List<object>();
			StiDataSource stiDataSource = data as StiDataSource;
			if (stiDataSource != null)
			{
				stiDataSource.SaveState("ExpressionCalculation");
				if (firstPositionInDataSource)
				{
					stiDataSource.First();
				}
				int num = 0;
				while (!stiDataSource.IsEof)
				{
					string text = StiReportParser.Parse(expression, (IStiReport)(object)stiDataSource.Dictionary.Report, allowCache: false, null, allowDataLoading: false, onlyExpression: true);
					if (text != null)
					{
						list.Add(text);
					}
					stiDataSource.Next();
					num++;
					if (maxRows.HasValue && maxRows == num)
					{
						break;
					}
				}
				stiDataSource.First();
				stiDataSource.RestoreState("ExpressionCalculation");
			}
			if (data is StiBusinessObject stiBusinessObject)
			{
				stiBusinessObject.SaveState("ExpressionCalculation");
				stiBusinessObject.CreateEnumerator();
				int num2 = 0;
				while (!stiBusinessObject.IsEof)
				{
					StiReport report = stiDataSource.Dictionary.Report;
					string text2 = StiReportParser.Parse(expression, (IStiReportComponent)(object)((report is IStiReportComponent) ? report : null), allowCache: false);
					if (text2 != null)
					{
						list.Add(text2);
					}
					stiBusinessObject.Next();
					num2++;
					if (maxRows.HasValue && maxRows == num2)
					{
						break;
					}
				}
				stiBusinessObject.RestoreState("ExpressionCalculation");
			}
			return list.ToArray();
		}
		catch
		{
		}
		return new object[0];
	}

	public StiDataColumn()
		: this("Column")
	{
	}

	public StiDataColumn(string name)
		: this(name, name, typeof(string))
	{
	}

	public StiDataColumn(string name, Type type)
		: this(name, name, type)
	{
	}

	public StiDataColumn(string name, string alias, Type type)
		: this(name, name, alias, type)
	{
	}

	public StiDataColumn(string nameInSource, string name, string alias, Type type)
	{
		NameInSource = nameInSource;
		this.name = name;
		Alias = alias;
		Type = type;
		Index = -1;
	}

	public StiDataColumn(string nameInSource, string name, string alias, Type type, string key)
	{
		NameInSource = nameInSource;
		this.name = name;
		Alias = alias;
		Type = type;
		Index = -1;
		Key = key;
	}
}
