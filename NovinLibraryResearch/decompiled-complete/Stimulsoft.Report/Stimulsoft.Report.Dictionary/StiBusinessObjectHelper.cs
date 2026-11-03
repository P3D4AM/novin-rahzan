using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Report.BarCodes;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Controls;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.Engine;

namespace Stimulsoft.Report.Dictionary;

public static class StiBusinessObjectHelper
{
	public static StiPropertiesProcessingType PropertiesProcessingType
	{
		get
		{
			return StiOptions.Dictionary.BusinessObjects.PropertiesProcessingType;
		}
		set
		{
			StiOptions.Dictionary.BusinessObjects.PropertiesProcessingType = value;
		}
	}

	public static StiFieldsProcessingType FieldsProcessingType
	{
		get
		{
			return StiOptions.Dictionary.BusinessObjects.FieldsProcessingType;
		}
		set
		{
			StiOptions.Dictionary.BusinessObjects.FieldsProcessingType = value;
		}
	}

	public static object GetElementFromEnumerable(IEnumerable enumerable)
	{
		try
		{
			IEnumerator enumerator = enumerable.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Current != null)
				{
					return enumerator.Current;
				}
			}
			return null;
		}
		catch
		{
			return null;
		}
	}

	public static Type GetElementType(Type arrayType)
	{
		if (arrayType != null)
		{
			if (arrayType.IsGenericType && (arrayType.FullName.StartsWith("System.Collections.Generic.List") || arrayType.FullName.StartsWith("System.Collections.Generic.IList") || arrayType.FullName.StartsWith("System.Collections.Generic.ICollection") || arrayType.FullName.IndexOf("WrappedEntityCollection`1") != -1))
			{
				try
				{
					Type[] genericArguments = arrayType.GetGenericArguments();
					if (genericArguments.Length > 1)
					{
						return typeof(object);
					}
					if (genericArguments.Length == 1)
					{
						return genericArguments[0];
					}
				}
				catch
				{
				}
			}
			if (arrayType.GetElementType() != null)
			{
				return arrayType.GetElementType();
			}
			string fullName = arrayType.FullName;
			if (fullName.Length > 2 && fullName[fullName.Length - 2] == '`')
			{
				return StiTypeFinder.GetType(fullName.Substring(0, fullName.Length - 2));
			}
			return StiTypeFinder.GetType(fullName);
		}
		return null;
	}

	private static object GetElement(object value)
	{
		if (value is IEnumerable)
		{
			object elementFromEnumerable = GetElementFromEnumerable(value as IEnumerable);
			if (elementFromEnumerable != null)
			{
				return elementFromEnumerable;
			}
		}
		if (value == null)
		{
			return null;
		}
		if (value is Type)
		{
			return GetElementType(value as Type);
		}
		Type type = value.GetType();
		return GetElementType(type);
	}

	internal static string GetAlias(PropertyInfo valueProp)
	{
		object[] customAttributes = valueProp.GetCustomAttributes(typeof(StiAliasAttribute), inherit: true);
		if (customAttributes != null && customAttributes.Length != 0)
		{
			return ((StiAliasAttribute)customAttributes[0]).Alias;
		}
		customAttributes = valueProp.GetCustomAttributes(typeof(DisplayNameAttribute), inherit: true);
		if (customAttributes != null && customAttributes.Length != 0)
		{
			return ((DisplayNameAttribute)customAttributes[0]).DisplayName;
		}
		return null;
	}

	internal static string GetAlias(FieldInfo valueProp)
	{
		object[] customAttributes = valueProp.GetCustomAttributes(typeof(StiAliasAttribute), inherit: true);
		if (customAttributes != null && customAttributes.Length != 0)
		{
			return ((StiAliasAttribute)customAttributes[0]).Alias;
		}
		return null;
	}

	internal static string GetAlias(PropertyDescriptor valueProp)
	{
		Attribute attribute = valueProp.Attributes[typeof(StiAliasAttribute)];
		if (attribute != null)
		{
			return ((StiAliasAttribute)attribute).Alias;
		}
		return null;
	}

	internal static bool IsAllowUseProperty(PropertyDescriptor valueProp)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		Attribute attribute = valueProp.Attributes[typeof(StiBrowsableAttribute)];
		if (attribute != null)
		{
			return ((StiBrowsableAttribute)attribute).Browsable;
		}
		attribute = valueProp.Attributes[typeof(BrowsableAttribute)];
		if (attribute != null && !((BrowsableAttribute)attribute).Browsable && PropertiesProcessingType == StiPropertiesProcessingType.Browsable)
		{
			return false;
		}
		return true;
	}

	internal static bool IsAllowUseProperty(PropertyInfo valueProp)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		object[] customAttributes = valueProp.GetCustomAttributes(typeof(StiBrowsableAttribute), inherit: true);
		if (customAttributes != null && customAttributes.Length != 0)
		{
			return ((StiBrowsableAttribute)customAttributes[0]).Browsable;
		}
		customAttributes = valueProp.GetCustomAttributes(typeof(BrowsableAttribute), inherit: true);
		if (customAttributes != null && customAttributes.Length != 0 && !((BrowsableAttribute)customAttributes[0]).Browsable && PropertiesProcessingType == StiPropertiesProcessingType.Browsable)
		{
			return false;
		}
		return true;
	}

	internal static bool IsAllowUseField(FieldInfo valueField)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		object[] customAttributes = valueField.GetCustomAttributes(typeof(StiBrowsableAttribute), inherit: true);
		if (customAttributes != null && customAttributes.Length != 0)
		{
			return ((StiBrowsableAttribute)customAttributes[0]).Browsable;
		}
		customAttributes = valueField.FieldType.GetCustomAttributes(typeof(BrowsableAttribute), inherit: true);
		if (customAttributes != null && customAttributes.Length != 0 && !((BrowsableAttribute)customAttributes[0]).Browsable && PropertiesProcessingType == StiPropertiesProcessingType.Browsable)
		{
			return false;
		}
		return true;
	}

	private static Type GetType(Type type)
	{
		return type;
	}

	private static StiDataColumn GetDataColumn(string name, Type type)
	{
		return GetDataColumn(name, name, type);
	}

	private static StiDataColumn GetDataColumn(string name, string alias, Type type)
	{
		if (alias == null)
		{
			alias = name;
		}
		return new StiDataColumn(name, alias, GetType(type));
	}

	private static StiDataColumn GetDataColumn(DataColumn dataColumn)
	{
		return new StiDataColumn(dataColumn.ColumnName, dataColumn.Caption, dataColumn.DataType);
	}

	private static StiDataColumnsCollection GetColumnsFromObject(object value)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		if (value is BindingSource)
		{
			value = ((BindingSource)value).DataSource;
		}
		if (value is DataSet)
		{
			return GetColumnsFromDataSet(value as DataSet);
		}
		if (value is DataTable)
		{
			return GetColumnsFromDataTable(value as DataTable);
		}
		if (value is ITypedList)
		{
			return GetColumnsFromITypedList(value as ITypedList);
		}
		if (value is ICustomTypeDescriptor)
		{
			return GetColumnsFromICustomTypeDescriptor(value as ICustomTypeDescriptor);
		}
		StiDataColumnsCollection result = new StiDataColumnsCollection();
		if (value == null)
		{
			return result;
		}
		object obj = GetElement(value);
		if (obj == null)
		{
			obj = value;
		}
		if (obj is Type)
		{
			Type type = obj as Type;
			if (type == typeof(string))
			{
				return GetColumnsFromString();
			}
			if (type == typeof(Image))
			{
				return GetColumnsFromImage();
			}
			if (type == typeof(DictionaryEntry))
			{
				return GetColumnsFromDictionaryEntry();
			}
			if ((obj as Type).IsValueType)
			{
				return GetColumnsFromValueType(obj);
			}
			return GetColumnsFromClass(obj);
		}
		if (obj is ITypedList)
		{
			return GetColumnsFromITypedList(obj as ITypedList);
		}
		if (obj is ICustomTypeDescriptor)
		{
			return GetColumnsFromICustomTypeDescriptor(obj as ICustomTypeDescriptor);
		}
		if (obj is string)
		{
			return GetColumnsFromString();
		}
		if (obj is Image)
		{
			return GetColumnsFromImage();
		}
		if (obj is DictionaryEntry)
		{
			return GetColumnsFromDictionaryEntry();
		}
		Type type2 = obj.GetType();
		if (type2.IsGenericType && type2.GetGenericTypeDefinition() == typeof(KeyValuePair<, >))
		{
			return GetColumnsFromKeyValuePair(obj);
		}
		if (type2.IsValueType)
		{
			return GetColumnsFromValueType(obj);
		}
		return GetColumnsFromClass(obj);
	}

	private static StiDataColumnsCollection GetColumnsFromITypedList(ITypedList typedList)
	{
		StiDataColumnsCollection stiDataColumnsCollection = new StiDataColumnsCollection();
		PropertyDescriptorCollection itemProperties = typedList.GetItemProperties(null);
		foreach (PropertyDescriptor item in itemProperties)
		{
			if (IsAllowUseProperty(item))
			{
				StiDataColumn dataColumn = GetDataColumn(item.Name, GetAlias(item), item.PropertyType);
				stiDataColumnsCollection.Add(dataColumn);
			}
		}
		return stiDataColumnsCollection;
	}

	private static StiDataColumnsCollection GetColumnsFromICustomTypeDescriptor(ICustomTypeDescriptor customTypeDescriptor)
	{
		StiDataColumnsCollection stiDataColumnsCollection = new StiDataColumnsCollection();
		PropertyDescriptorCollection properties = customTypeDescriptor.GetProperties();
		foreach (PropertyDescriptor item in properties)
		{
			if (IsAllowUseProperty(item))
			{
				StiDataColumn dataColumn = GetDataColumn(item.Name, GetAlias(item), item.PropertyType);
				stiDataColumnsCollection.Add(dataColumn);
			}
		}
		return stiDataColumnsCollection;
	}

	private static StiDataColumnsCollection GetColumnsFromString()
	{
		StiDataColumnsCollection stiDataColumnsCollection = new StiDataColumnsCollection();
		StiDataColumn dataColumn = GetDataColumn("Data", "Data", typeof(string));
		stiDataColumnsCollection.Add(dataColumn);
		return stiDataColumnsCollection;
	}

	private static StiDataColumnsCollection GetColumnsFromImage()
	{
		StiDataColumnsCollection stiDataColumnsCollection = new StiDataColumnsCollection();
		StiDataColumn dataColumn = GetDataColumn("Image", "Image", typeof(Image));
		stiDataColumnsCollection.Add(dataColumn);
		return stiDataColumnsCollection;
	}

	private static StiDataColumnsCollection GetColumnsFromDictionaryEntry()
	{
		StiDataColumnsCollection stiDataColumnsCollection = new StiDataColumnsCollection();
		StiDataColumn dataColumn = GetDataColumn("Key", "Key", typeof(object));
		stiDataColumnsCollection.Add(dataColumn);
		dataColumn = GetDataColumn("Value", "Value", typeof(object));
		stiDataColumnsCollection.Add(dataColumn);
		return stiDataColumnsCollection;
	}

	private static StiDataColumnsCollection GetColumnsFromKeyValuePair(object item)
	{
		Type[] genericArguments = item.GetType().GetGenericArguments();
		StiDataColumnsCollection stiDataColumnsCollection = new StiDataColumnsCollection();
		StiDataColumn dataColumn = GetDataColumn("Key", "Key", genericArguments[0]);
		stiDataColumnsCollection.Add(dataColumn);
		dataColumn = GetDataColumn("Value", "Value", genericArguments[1]);
		stiDataColumnsCollection.Add(dataColumn);
		return stiDataColumnsCollection;
	}

	private static StiDataColumnsCollection GetColumnsFromValueType(object value)
	{
		StiDataColumnsCollection stiDataColumnsCollection = new StiDataColumnsCollection();
		if (value is Type)
		{
			StiDataColumn dataColumn = GetDataColumn("Data", "Data", (Type)value);
			stiDataColumnsCollection.Add(dataColumn);
		}
		else
		{
			StiDataColumn dataColumn2 = GetDataColumn("Data", "Data", value.GetType());
			stiDataColumnsCollection.Add(dataColumn2);
		}
		return stiDataColumnsCollection;
	}

	private static StiDataColumnsCollection GetColumnsFromClass(object value)
	{
		StiDataColumnsCollection stiDataColumnsCollection = new StiDataColumnsCollection();
		if (StiOptions.Dictionary.BusinessObjects.AllowUseProperties)
		{
			PropertyInfo[] array = ((value is Type) ? ((Type)value).GetProperties() : value.GetType().GetProperties());
			PropertyInfo[] array2 = array;
			foreach (PropertyInfo propertyInfo in array2)
			{
				if (IsAllowUseProperty(propertyInfo) && !(propertyInfo.PropertyType.FullName == "System.Windows.Threading.Dispatcher") && !(propertyInfo.PropertyType.FullName == "System.Windows.DependencyObjectType"))
				{
					StiDataColumn dataColumn = GetDataColumn(propertyInfo.Name, GetAlias(propertyInfo), GetType(propertyInfo.PropertyType));
					stiDataColumnsCollection.Add(dataColumn);
				}
			}
		}
		if (StiOptions.Dictionary.BusinessObjects.AllowUseFields)
		{
			FieldInfo[] array3 = ((value is Type) ? ((Type)value).GetFields() : value.GetType().GetFields());
			FieldInfo[] array4 = array3;
			foreach (FieldInfo fieldInfo in array4)
			{
				if (IsAllowUseField(fieldInfo))
				{
					StiDataColumn dataColumn2 = GetDataColumn(fieldInfo.Name, GetAlias(fieldInfo), GetType(fieldInfo.FieldType));
					stiDataColumnsCollection.Add(dataColumn2);
				}
			}
		}
		return stiDataColumnsCollection;
	}

	private static StiDataColumnsCollection GetColumnsFromDataTable(DataTable table)
	{
		StiDataColumnsCollection stiDataColumnsCollection = new StiDataColumnsCollection();
		foreach (DataRelation childRelation in table.ChildRelations)
		{
			stiDataColumnsCollection.Add(new StiDataColumn(childRelation.RelationName, typeof(DataTable)));
		}
		foreach (DataColumn column in table.Columns)
		{
			stiDataColumnsCollection.Add(GetDataColumn(column));
		}
		return stiDataColumnsCollection;
	}

	private static StiDataColumnsCollection GetColumnsFromDataSet(DataSet dataSet)
	{
		StiDataColumnsCollection stiDataColumnsCollection = new StiDataColumnsCollection();
		foreach (DataTable table in dataSet.Tables)
		{
			stiDataColumnsCollection.Add(new StiDataColumn(table.TableName, table.TableName, typeof(object)));
		}
		return stiDataColumnsCollection;
	}

	public static StiDataColumnsCollection GetColumnsFromBusinessObjectData(StiBusinessObjectData data)
	{
		return GetColumnsFromBusinessObjectData(data, includeChildDataSources: false);
	}

	public static StiDataColumnsCollection GetColumnsFromBusinessObjectData(StiBusinessObjectData data, bool includeChildDataSources)
	{
		return GetColumnsFromData(data.BusinessObjectValue, includeChildDataSources);
	}

	public static StiDataColumnsCollection GetColumnsFromData(object data)
	{
		return GetColumnsFromData(data, includeChildDataSources: false);
	}

	public static StiDataColumnsCollection GetColumnsFromData(object data, bool includeChildDataSources)
	{
		StiDataColumnsCollection columnsFromObject = GetColumnsFromObject(data);
		if (includeChildDataSources)
		{
			return columnsFromObject;
		}
		StiDataColumnsCollection stiDataColumnsCollection = new StiDataColumnsCollection();
		foreach (StiDataColumn item in columnsFromObject)
		{
			if (IsDataColumn(item.Type))
			{
				stiDataColumnsCollection.Add(item);
			}
		}
		return stiDataColumnsCollection;
	}

	public static StiDataColumnsCollection GetColumnsFromBusinessObject(StiBusinessObject data)
	{
		return GetColumnsFromBusinessObject(data, includeChildDataSources: false);
	}

	public static StiDataColumnsCollection GetColumnsFromBusinessObject(StiBusinessObject data, bool includeChildDataSources)
	{
		StiDataColumnsCollection columnsFromObject = GetColumnsFromObject(data.BusinessObjectValue);
		StiDataColumnsCollection stiDataColumnsCollection = new StiDataColumnsCollection();
		foreach (StiDataColumn item in columnsFromObject)
		{
			if (IsDataColumn(item.Type))
			{
				stiDataColumnsCollection.Add(item);
			}
		}
		return stiDataColumnsCollection;
	}

	public static IEnumerator GetEnumeratorFromObject(object value)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		if (value is BindingSource)
		{
			value = ((BindingSource)value).DataSource;
		}
		if (value == null)
		{
			return null;
		}
		if (value is DataSet)
		{
			return GetEnumeratorFromDataSet(value as DataSet);
		}
		if (value is DataTable)
		{
			return GetEnumeratorFromDataTable(value as DataTable);
		}
		if (value is IEnumerable)
		{
			return GetEnumeratorFromIEnumerable(value as IEnumerable);
		}
		if (value is IEnumerator)
		{
			return GetEnumeratorFromIEnumerator(value as IEnumerator);
		}
		return GetEnumeratorFromClass(value);
	}

	public static IEnumerator GetEnumeratorFromDataSet(DataSet dataSet)
	{
		return GetEnumeratorFromIEnumerable(new object[1] { dataSet });
	}

	public static IEnumerator GetEnumeratorFromDataTable(DataTable dataTable)
	{
		return GetEnumeratorFromIEnumerable(dataTable.Rows);
	}

	public static IEnumerator GetEnumeratorFromIEnumerable(IEnumerable enumerable)
	{
		try
		{
			return GetEnumeratorFromIEnumerator(enumerable.GetEnumerator());
		}
		catch
		{
			return null;
		}
	}

	public static IEnumerator GetEnumeratorFromIEnumerator(IEnumerator enumerator)
	{
		return enumerator;
	}

	public static IEnumerator GetEnumeratorFromClass(object value)
	{
		return GetEnumeratorFromIEnumerable(new object[1] { value });
	}

	public static object GetElementFromObject(object value, string name)
	{
		return GetElementFromObject(value, name, isColumnsRetrieve: false);
	}

	public static object GetElementFromObject(object value, string name, bool isColumnsRetrieve)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		if (value is BindingSource)
		{
			value = ((BindingSource)value).DataSource;
		}
		if (value == null)
		{
			return null;
		}
		if (value is DataSet)
		{
			return GetValueFromDataSet(value as DataSet, name);
		}
		if (value is DataTable)
		{
			return GetValueFromDataTable(value as DataTable, name);
		}
		if (value is ITypedList)
		{
			return GetValueFromITypedList(value as ITypedList, name);
		}
		if (value is ICustomTypeDescriptor)
		{
			return GetValueFromICustomTypeDescriptor(value as ICustomTypeDescriptor, name);
		}
		if (value is string)
		{
			return value as string;
		}
		if (value is Image)
		{
			return (value is Image) ? value : null;
		}
		if (value is DictionaryEntry)
		{
			return GetValueFromDictionaryEntry((DictionaryEntry)value, name);
		}
		if (IsValueExistInClass(value, name))
		{
			return GetValueFromClass(value, name, isColumnsRetrieve);
		}
		object obj = GetElement(value);
		if (obj == null)
		{
			obj = value;
		}
		if (obj is DataSet)
		{
			return GetValueFromDataSet(obj as DataSet, name);
		}
		if (obj is DataTable)
		{
			return GetValueFromDataTable(obj as DataTable, name);
		}
		if (obj is ITypedList)
		{
			return GetValueFromITypedList(obj as ITypedList, name);
		}
		if (obj is ICustomTypeDescriptor)
		{
			return GetValueFromICustomTypeDescriptor(obj as ICustomTypeDescriptor, name);
		}
		if (obj is string)
		{
			return obj as string;
		}
		if (obj is Image)
		{
			return (value is Image) ? value : null;
		}
		if (obj is DictionaryEntry)
		{
			return GetValueFromDictionaryEntry((DictionaryEntry)obj, name);
		}
		if (obj == null)
		{
			obj = value;
		}
		if (obj.GetType().IsValueType)
		{
			return obj;
		}
		return GetValueFromClass(obj, name, isColumnsRetrieve);
	}

	public static object GetValueFromObject(object value, string name)
	{
		return GetValueFromObject(value, name, isColumnsRetrieve: false);
	}

	public static object GetValueFromObject(object value, string name, bool isColumnsRetrieve)
	{
		if (value is DataRow[] && name != "Length" && value is DataRow[] array && array.Length != 0)
		{
			value = array[0];
		}
		if (value is IEnumerable && !(value is string))
		{
			object elementFromEnumerable = GetElementFromEnumerable(value as IEnumerable);
			if (elementFromEnumerable != null)
			{
				value = elementFromEnumerable;
			}
		}
		if (value == null)
		{
			return null;
		}
		if (value is DataSet)
		{
			return GetValueFromDataSet(value as DataSet, name);
		}
		if (value is DataRow)
		{
			return GetValueFromDataRow(value as DataRow, name);
		}
		if (value is ITypedList)
		{
			return GetValueFromITypedList(value as ITypedList, name);
		}
		if (value is ICustomTypeDescriptor)
		{
			return GetValueFromICustomTypeDescriptor(value as ICustomTypeDescriptor, name);
		}
		if (value is string)
		{
			return value as string;
		}
		if (value is Image)
		{
			return (value is Image) ? value : null;
		}
		if (value is DictionaryEntry)
		{
			return GetValueFromDictionaryEntry((DictionaryEntry)value, name);
		}
		Type type = value.GetType();
		if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(KeyValuePair<, >))
		{
			return GetValueFromKeyValuePair(value, name);
		}
		if (type.IsValueType)
		{
			return value;
		}
		return GetValueFromClass(value, name, isColumnsRetrieve);
	}

	public static object GetValueFromDataSet(DataSet dataSet, string name)
	{
		if (dataSet.Tables.Contains(name))
		{
			return dataSet.Tables[name];
		}
		return null;
	}

	public static object GetValueFromDataTable(DataTable dataTable, string name)
	{
		if (dataTable.ChildRelations.Contains(name))
		{
			return dataTable.ChildRelations[name].ChildTable;
		}
		if (dataTable.Columns.Contains(name))
		{
			return dataTable.Columns[name];
		}
		return null;
	}

	public static object GetValueFromDataRow(DataRow dataRow, string name)
	{
		if (dataRow.Table.Columns.Contains(name))
		{
			return dataRow[name];
		}
		if (dataRow.Table.ChildRelations.Contains(name))
		{
			return dataRow.GetChildRows(name);
		}
		return null;
	}

	public static object GetValueFromITypedList(ITypedList typedList, string name)
	{
		PropertyDescriptorCollection itemProperties = typedList.GetItemProperties(null);
		PropertyDescriptor propertyDescriptor = itemProperties[name];
		return propertyDescriptor.GetValue(typedList);
	}

	public static object GetValueFromITypedList(ITypedList typedList, string name, object value)
	{
		PropertyDescriptorCollection itemProperties = typedList.GetItemProperties(null);
		return itemProperties[name]?.GetValue(value);
	}

	public static object GetValueFromICustomTypeDescriptor(ICustomTypeDescriptor customTypeDescriptor, string name)
	{
		PropertyDescriptorCollection properties = customTypeDescriptor.GetProperties();
		PropertyDescriptor propertyDescriptor = properties[name];
		return propertyDescriptor.GetValue(customTypeDescriptor);
	}

	public static object GetValueFromICustomTypeDescriptor(ICustomTypeDescriptor customTypeDescriptor, string name, object value)
	{
		PropertyDescriptorCollection properties = customTypeDescriptor.GetProperties();
		return properties[name]?.GetValue(value);
	}

	public static object GetValueFromDictionaryEntry(DictionaryEntry entry, string name)
	{
		if (name == "Key")
		{
			return entry.Key;
		}
		if (name == "Value")
		{
			return entry.Value;
		}
		return null;
	}

	public static object GetValueFromKeyValuePair(object item, string name)
	{
		if (name == "Key" || name == "Value")
		{
			try
			{
				return item.GetType().GetProperty(name).GetValue(item, null);
			}
			catch
			{
			}
		}
		return null;
	}

	public static object GetValueFromClass(object value, string name)
	{
		return GetValueFromClass(value, name, isColumnsRetrieve: false);
	}

	public static object GetValueFromClass(object value, string name, bool isColumnsRetrieve)
	{
		if (value == null)
		{
			return null;
		}
		if (StiOptions.Dictionary.BusinessObjects.AllowUseProperties)
		{
			PropertyInfo[] array = ((value is Type) ? ((Type)value).GetProperties() : value.GetType().GetProperties());
			PropertyInfo[] array2 = array;
			foreach (PropertyInfo propertyInfo in array2)
			{
				if (!(propertyInfo.Name == name))
				{
					continue;
				}
				if (value is Type)
				{
					return propertyInfo.PropertyType;
				}
				try
				{
					object value2 = propertyInfo.GetValue(value, null);
					if (value2 != null)
					{
						return value2;
					}
					if (IsTypeImplementICollection(propertyInfo.PropertyType) || StiBusinessObject.FieldsIgnoreList.ContainsKey(propertyInfo.ReflectedType.Name + "." + propertyInfo.Name))
					{
						return null;
					}
					return propertyInfo.PropertyType;
				}
				catch
				{
					return propertyInfo.PropertyType;
				}
			}
		}
		if (StiOptions.Dictionary.BusinessObjects.AllowUseFields)
		{
			FieldInfo[] array3 = ((value is Type) ? ((Type)value).GetFields() : value.GetType().GetFields());
			FieldInfo[] array4 = array3;
			foreach (FieldInfo fieldInfo in array4)
			{
				if (fieldInfo.Name == name)
				{
					if (value is Type)
					{
						return fieldInfo.FieldType;
					}
					object value3 = fieldInfo.GetValue(value);
					if (value3 != null)
					{
						return value3;
					}
					return fieldInfo.FieldType;
				}
			}
		}
		return null;
	}

	public static bool IsTypeImplementICollection(Type type)
	{
		try
		{
			Type[] interfaces = type.GetInterfaces();
			foreach (Type type2 in interfaces)
			{
				if (type2 == typeof(ICollection))
				{
					return true;
				}
			}
		}
		catch
		{
		}
		return false;
	}

	public static bool IsValueExistInClass(object value, string name)
	{
		if (value == null)
		{
			return false;
		}
		if (StiOptions.Dictionary.BusinessObjects.AllowUseProperties)
		{
			PropertyInfo[] array = ((value is Type) ? ((Type)value).GetProperties() : value.GetType().GetProperties());
			PropertyInfo[] array2 = array;
			foreach (PropertyInfo propertyInfo in array2)
			{
				if (propertyInfo.Name == name)
				{
					return true;
				}
			}
		}
		if (StiOptions.Dictionary.BusinessObjects.AllowUseFields)
		{
			FieldInfo[] array3 = ((value is Type) ? ((Type)value).GetFields() : value.GetType().GetFields());
			FieldInfo[] array4 = array3;
			foreach (FieldInfo fieldInfo in array4)
			{
				if (fieldInfo.Name == name)
				{
					return true;
				}
			}
		}
		return false;
	}

	public static bool IsDataColumn(Type type)
	{
		if (type.IsValueType)
		{
			return true;
		}
		if (type == typeof(string))
		{
			return true;
		}
		if (type == typeof(byte[]))
		{
			return true;
		}
		if (type == typeof(Image))
		{
			return true;
		}
		return false;
	}

	public static string GetBusinessObjectFullName(StiBusinessObject businessObject)
	{
		string text = businessObject.Name;
		while (businessObject.ParentBusinessObject != null)
		{
			businessObject = businessObject.ParentBusinessObject;
			text = businessObject.Name + "." + text;
		}
		return text;
	}

	public static string GetBusinessObjectFullAlias(StiBusinessObject businessObject)
	{
		string text = businessObject.Alias;
		while (businessObject.ParentBusinessObject != null)
		{
			businessObject = businessObject.ParentBusinessObject;
			text = businessObject.Alias + "." + text;
		}
		return text;
	}

	public static StiBusinessObject GetBusinessObjectFromGuid(StiReport report, string guid)
	{
		List<StiBusinessObject> businessObjectsFromReport = GetBusinessObjectsFromReport(report);
		if (businessObjectsFromReport == null)
		{
			return null;
		}
		foreach (StiBusinessObject item in businessObjectsFromReport)
		{
			if (item.Guid == guid)
			{
				return item;
			}
		}
		return null;
	}

	public static List<StiBusinessObject> GetBusinessObjectsFromReport(StiBusinessObjectsCollection businessObjects)
	{
		List<StiBusinessObject> list = null;
		foreach (StiBusinessObject businessObject in businessObjects)
		{
			List<StiBusinessObject> businessObjectsFromReport = GetBusinessObjectsFromReport(businessObject.BusinessObjects);
			if (businessObjectsFromReport != null)
			{
				if (list == null)
				{
					list = new List<StiBusinessObject>();
				}
				foreach (StiBusinessObject item in businessObjectsFromReport)
				{
					list.Add(item);
				}
			}
			if (list == null)
			{
				list = new List<StiBusinessObject>();
			}
			list.Add(businessObject);
		}
		return list;
	}

	public static List<StiBusinessObject> GetBusinessObjectsFromReport(StiReport report)
	{
		return GetBusinessObjectsFromReport(report.Dictionary.BusinessObjects);
	}

	public static List<string> GetUsedBusinessObjectsNamesList(StiReport report)
	{
		return GetUsedBusinessObjectsNamesList(report, addColumns: false);
	}

	public static List<string> GetUsedBusinessObjectsNamesList(StiReport report, bool addColumns)
	{
		Hashtable usedBusinessObjectsNames = GetUsedBusinessObjectsNames(report, addColumns);
		List<string> list = new List<string>();
		foreach (DictionaryEntry item in usedBusinessObjectsNames)
		{
			list.Add((string)item.Value);
		}
		list.Sort();
		return list;
	}

	public static Hashtable GetUsedBusinessObjectsNames(StiReport report)
	{
		return GetUsedBusinessObjectsNames(report, addColumns: false);
	}

	public static Hashtable GetUsedBusinessObjectsNames(StiReport report, bool addColumns)
	{
		Hashtable hashtable = new Hashtable();
		foreach (StiComponent component in report.GetComponents())
		{
			try
			{
				if (component is StiDataBand stiDataBand && stiDataBand.BusinessObject.Name != null && stiDataBand.BusinessObject.Name.Length > 0)
				{
					string businessObjectFullName = GetBusinessObjectFullName(stiDataBand.BusinessObject);
					hashtable[businessObjectFullName] = businessObjectFullName;
				}
				if (component is StiCrossTab stiCrossTab && stiCrossTab.BusinessObject.Name != null && stiCrossTab.BusinessObject.Name.Length > 0)
				{
					string businessObjectFullName2 = GetBusinessObjectFullName(stiCrossTab.BusinessObject);
					hashtable[businessObjectFullName2] = businessObjectFullName2;
				}
				if (component is StiGroupHeaderBand stiGroupHeaderBand)
				{
					CheckExpression(stiGroupHeaderBand.Condition.Value, component, hashtable, addColumns);
				}
				if (component is StiSimpleText stiSimpleText)
				{
					CheckExpression(stiSimpleText.Text.Value, component, hashtable, addColumns);
				}
				if (component is StiText stiText)
				{
					CheckExpression(stiText.ExcelValue.Value, component, hashtable, addColumns);
				}
				if (component is StiRichText stiRichText)
				{
					RichTextBox val = (RichTextBox)(object)new StiRichTextBox(drawTransparent: false);
					try
					{
						val.Rtf = stiRichText.RtfText;
						CheckExpression(((Control)val).Text, component, hashtable, addColumns);
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
				}
				if (component is StiImage stiImage)
				{
					CheckExpression(stiImage.ImageData.Value, component, hashtable, addColumns);
					CheckExpression("{" + stiImage.DataColumn + "}", component, hashtable, addColumns);
				}
				if (component is StiBarCode stiBarCode)
				{
					CheckExpression(stiBarCode.Code.Value, component, hashtable, addColumns);
				}
				if (component is StiZipCode stiZipCode)
				{
					CheckExpression(stiZipCode.Code.Value, component, hashtable, addColumns);
				}
				if (component is StiCheckBox stiCheckBox)
				{
					CheckExpression(stiCheckBox.Checked.Value, component, hashtable, addColumns);
				}
				if (component is StiChart stiChart)
				{
					string businessObjectFullName3 = GetBusinessObjectFullName(stiChart.BusinessObject);
					if (!string.IsNullOrEmpty(businessObjectFullName3))
					{
						hashtable[businessObjectFullName3] = businessObjectFullName3;
					}
					foreach (StiSeries item3 in stiChart.Series)
					{
						CheckExpression(item3.Argument.Value, component, hashtable, addColumns);
						CheckExpression("{" + item3.ArgumentDataColumn + "}", component, hashtable, addColumns);
					}
				}
				if (component is StiPage stiPage)
				{
					CheckExpression(stiPage.ExcelSheet.Value, component, hashtable, addColumns);
				}
				IStiConditions stiConditions = component;
				if (stiConditions == null || stiConditions.Conditions.Count <= 0)
				{
					continue;
				}
				List<DictionaryEntry> list = new List<DictionaryEntry>();
				foreach (StiBaseCondition condition in stiConditions.Conditions)
				{
					StiCondition stiCondition = condition as StiCondition;
					string text = null;
					if (condition is StiMultiCondition)
					{
						StiMultiCondition stiMultiCondition = condition as StiMultiCondition;
						if (!stiMultiCondition.FilterOn || stiMultiCondition.Filters.Count <= 0)
						{
							continue;
						}
						StringBuilder stringBuilder = new StringBuilder("{");
						for (int i = 0; i < stiMultiCondition.Filters.Count; i++)
						{
							StiFilter stiFilter = stiMultiCondition.Filters[i];
							stringBuilder.Append("(");
							stringBuilder.Append(StiDataHelper.GetFilterExpression(stiFilter, stiFilter.Column, report));
							stringBuilder.Append(")");
							if (i < stiMultiCondition.Filters.Count - 1)
							{
								stringBuilder.Append((stiMultiCondition.FilterMode == StiFilterMode.And) ? " && " : " || ");
							}
						}
						stringBuilder.Append("}");
						DictionaryEntry item = new DictionaryEntry(stiMultiCondition, stringBuilder.ToString());
						list.Add(item);
					}
					else if (stiCondition != null)
					{
						text = "{" + StiDataHelper.GetFilterExpression(stiCondition, stiCondition.Column, report) + "}";
						DictionaryEntry item2 = new DictionaryEntry(stiCondition, text);
						list.Add(item2);
					}
				}
				if (list.Count <= 0)
				{
					continue;
				}
				foreach (DictionaryEntry item4 in list)
				{
					CheckExpression((string)item4.Value, component, hashtable, addColumns);
				}
			}
			catch
			{
			}
		}
		return hashtable;
	}

	private static void CheckExpression(string expression, StiComponent component, Hashtable businessobjectsNames, bool addColumns)
	{
		try
		{
			bool storeToPrint = false;
			object obj = StiParser.ParseTextValue(expression, component, ref storeToPrint, executeIfStoreToPrint: false, returnAsmList: true);
			if (obj == null || !(obj is List<StiParser.StiAsmCommand>))
			{
				return;
			}
			foreach (StiParser.StiAsmCommand item in obj as List<StiParser.StiAsmCommand>)
			{
				if (item.Type != StiParser.StiAsmCommandType.PushBusinessObjectField)
				{
					continue;
				}
				string text = (string)item.Parameter1;
				string[] array = text.Split(new char[1] { '.' });
				try
				{
					StiBusinessObject stiBusinessObject = component.Report.Dictionary.BusinessObjects[array[0]];
					string businessObjectFullName = GetBusinessObjectFullName(stiBusinessObject);
					businessobjectsNames[businessObjectFullName] = businessObjectFullName;
					string text2 = array[1];
					stiBusinessObject = stiBusinessObject.BusinessObjects[array[1]];
					if (array.Length > 2)
					{
						businessObjectFullName = GetBusinessObjectFullName(stiBusinessObject);
						businessobjectsNames[businessObjectFullName] = businessObjectFullName;
						for (int i = 2; i < array.Length - 1; i++)
						{
							text2 = array[i];
							stiBusinessObject = stiBusinessObject.BusinessObjects[array[i]];
							businessObjectFullName = GetBusinessObjectFullName(stiBusinessObject);
							businessobjectsNames[businessObjectFullName] = businessObjectFullName;
						}
					}
					else if (stiBusinessObject != null)
					{
						businessObjectFullName = GetBusinessObjectFullName(stiBusinessObject);
						businessobjectsNames[businessObjectFullName] = businessObjectFullName;
					}
					if (addColumns)
					{
						businessobjectsNames[text] = text;
					}
				}
				catch
				{
					businessobjectsNames[text] = text;
				}
			}
		}
		catch
		{
		}
	}
}
