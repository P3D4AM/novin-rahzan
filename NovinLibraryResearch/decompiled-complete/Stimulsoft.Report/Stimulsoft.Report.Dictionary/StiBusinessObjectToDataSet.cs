using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;

namespace Stimulsoft.Report.Dictionary;

public sealed class StiBusinessObjectToDataSet
{
	private class StiRelation
	{
		public string Name;

		public string ParentTableName;

		public string ChildTableName;

		public string ParentColumnName;

		public string ChildColumnName;

		public StiRelation(string name, string parentTableName, string childTableName, string parentColumnName, string childColumnName)
		{
			Name = name;
			ParentTableName = parentTableName;
			ChildTableName = childTableName;
			ParentColumnName = parentColumnName;
			ChildColumnName = childColumnName;
		}
	}

	private DataSet dataSet;

	private Hashtable relations;

	private Hashtable uniques;

	private int level;

	[Obsolete("StiBusinessObjectToDataSet.PropertiesProcessingType property is obsolete. Please use StiOptions.Dictionary.BusinessObjects.PropertiesProcessingType property insead.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
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

	[Obsolete("StiBusinessObjectToDataSet.FieldsProcessingType property is obsolete. Please use StiOptions.Dictionary.BusinessObjects.FieldsProcessingType property insead.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
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

	[Obsolete("StiBusinessObjectToDataSet.Delimeter property is obsolete. Please use StiOptions.Dictionary.BusinessObjects.Delimeter property insead.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static char Delimeter
	{
		get
		{
			return StiOptions.Dictionary.BusinessObjects.Delimeter;
		}
		set
		{
			StiOptions.Dictionary.BusinessObjects.Delimeter = value;
		}
	}

	[Obsolete("StiBusinessObjectToDataSet.MaxLevel property is obsolete. Please use StiOptions.Dictionary.BusinessObjects.MaxLevel property insead.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static int MaxLevel
	{
		get
		{
			return StiOptions.Dictionary.BusinessObjects.MaxLevel;
		}
		set
		{
			StiOptions.Dictionary.BusinessObjects.MaxLevel = value;
		}
	}

	[Obsolete("StiBusinessObjectToDataSet.CheckTableDuplication property is obsolete. Please use StiOptions.Dictionary.BusinessObjects.CheckTableDuplication property insead.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static bool CheckTableDuplication
	{
		get
		{
			return StiOptions.Dictionary.BusinessObjects.CheckTableDuplication;
		}
		set
		{
			StiOptions.Dictionary.BusinessObjects.CheckTableDuplication = value;
		}
	}

	private static Type GetType(Type type)
	{
		if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
		{
			return Nullable.GetUnderlyingType(type);
		}
		return type;
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
		if (attribute != null && !((BrowsableAttribute)attribute).Browsable && StiOptions.Dictionary.BusinessObjects.PropertiesProcessingType == StiPropertiesProcessingType.Browsable)
		{
			return false;
		}
		return true;
	}

	internal static string GetAlias(PropertyInfo valueProp)
	{
		object[] customAttributes = valueProp.GetCustomAttributes(typeof(StiAliasAttribute), inherit: true);
		if (customAttributes.Length == 0)
		{
			return null;
		}
		return ((StiAliasAttribute)customAttributes[0]).Alias;
	}

	internal static string GetAlias(FieldInfo valueProp)
	{
		object[] customAttributes = valueProp.GetCustomAttributes(typeof(StiAliasAttribute), inherit: true);
		if (customAttributes.Length == 0)
		{
			return null;
		}
		return ((StiAliasAttribute)customAttributes[0]).Alias;
	}

	internal static string GetAlias(PropertyDescriptor valueProp)
	{
		Attribute attribute = valueProp.Attributes[typeof(StiAliasAttribute)];
		if (attribute == null)
		{
			return null;
		}
		return ((StiAliasAttribute)attribute).Alias;
	}

	internal static bool IsAllowUseProperty(PropertyInfo valueProp, Hashtable hashIsAllowUseProperty)
	{
		object obj = hashIsAllowUseProperty[valueProp];
		bool flag;
		if (obj == null)
		{
			flag = IsAllowUseProperty(valueProp);
			hashIsAllowUseProperty[valueProp] = flag;
		}
		else
		{
			flag = (bool)obj;
		}
		return flag;
	}

	internal static bool IsAllowUseProperty(PropertyInfo valueProp)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		object[] customAttributes = valueProp.GetCustomAttributes(typeof(StiBrowsableAttribute), inherit: true);
		if (customAttributes.Length != 0)
		{
			return ((StiBrowsableAttribute)customAttributes[0]).Browsable;
		}
		customAttributes = valueProp.GetCustomAttributes(typeof(BrowsableAttribute), inherit: true);
		if (customAttributes.Length != 0 && !((BrowsableAttribute)customAttributes[0]).Browsable && StiOptions.Dictionary.BusinessObjects.PropertiesProcessingType == StiPropertiesProcessingType.Browsable)
		{
			return false;
		}
		return true;
	}

	internal static bool IsAllowUseField(FieldInfo valueField)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		object[] customAttributes = valueField.GetCustomAttributes(typeof(StiBrowsableAttribute), inherit: true);
		if (customAttributes.Length != 0)
		{
			return ((StiBrowsableAttribute)customAttributes[0]).Browsable;
		}
		customAttributes = valueField.FieldType.GetCustomAttributes(typeof(BrowsableAttribute), inherit: true);
		if (customAttributes.Length != 0 && !((BrowsableAttribute)customAttributes[0]).Browsable && StiOptions.Dictionary.BusinessObjects.PropertiesProcessingType == StiPropertiesProcessingType.Browsable)
		{
			return false;
		}
		return true;
	}

	private bool NeedProcess(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (obj.GetType().IsValueType)
		{
			return false;
		}
		if (obj is string)
		{
			return false;
		}
		if (obj is Image)
		{
			return false;
		}
		if (obj.GetType().ToString() == "System.Windows.Media.ImageSource")
		{
			return false;
		}
		if (obj.GetType().ToString() == "System.Windows.Threading.Dispatcher")
		{
			return false;
		}
		if (obj is Point)
		{
			return false;
		}
		if (obj is Size)
		{
			return false;
		}
		if (obj is Rectangle)
		{
			return false;
		}
		if (obj is PointF)
		{
			return false;
		}
		if (obj is SizeF)
		{
			return false;
		}
		if (obj is RectangleF)
		{
			return false;
		}
		if (obj is IEnumerable || obj.GetType().IsClass)
		{
			return true;
		}
		return false;
	}

	private bool NeedProcess(Type type)
	{
		if (type == null)
		{
			return false;
		}
		if (type.IsValueType)
		{
			return false;
		}
		if (type == typeof(string))
		{
			return false;
		}
		if (StiTypeFinder.FindType(type, typeof(Image)))
		{
			return false;
		}
		if (type.ToString() == "System.Windows.Media.ImageSource")
		{
			return false;
		}
		if (type.ToString() == "System.Windows.Threading.Dispatcher")
		{
			return false;
		}
		if (type == typeof(Point))
		{
			return false;
		}
		if (type == typeof(Size))
		{
			return false;
		}
		if (type == typeof(Rectangle))
		{
			return false;
		}
		if (type == typeof(PointF))
		{
			return false;
		}
		if (type == typeof(SizeF))
		{
			return false;
		}
		if (type == typeof(RectangleF))
		{
			return false;
		}
		if (StiTypeFinder.FindInterface(type, typeof(IEnumerable)) || type.IsClass)
		{
			return true;
		}
		return false;
	}

	internal static DataColumn GetDataColumn(string name, Type type)
	{
		return GetDataColumn(name, name, type);
	}

	internal static DataColumn GetDataColumn(string name, string alias, Type type)
	{
		DataColumn dataColumn = new DataColumn(name, GetType(type));
		dataColumn.Caption = (string.IsNullOrEmpty(alias) ? name : alias);
		dataColumn.AllowDBNull = !StiOptions.Dictionary.UseAllowDBNullProperty || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)) || !type.IsValueType || type == typeof(string);
		return dataColumn;
	}

	private bool CheckTable(ArrayList tableList, ArrayList tableNames, string checkName, object checkObject)
	{
		if (!StiOptions.Dictionary.BusinessObjects.CheckTableDuplication)
		{
			return true;
		}
		for (int i = 0; i < tableNames.Count; i++)
		{
			string text = tableNames[i] as string;
			object obj = tableList[i];
			if (text == checkName && obj.GetType() == checkObject.GetType())
			{
				if (!(obj is IEnumerable) || !(checkObject is IEnumerable))
				{
					return false;
				}
				object valueFromEnumerable = GetValueFromEnumerable(obj as IEnumerable);
				object valueFromEnumerable2 = GetValueFromEnumerable(checkObject as IEnumerable);
				if (valueFromEnumerable != null && valueFromEnumerable2 != null && valueFromEnumerable.GetType() == valueFromEnumerable2.GetType())
				{
					return false;
				}
			}
		}
		return true;
	}

	private string GetTableName(string parentTableName, string columnName)
	{
		return $"{parentTableName}{StiOptions.Dictionary.BusinessObjects.Delimeter}{columnName}";
	}

	private string GetNameFromObject(object obj, string baseName)
	{
		if (obj is ITypedList)
		{
			string listName = ((ITypedList)obj).GetListName(null);
			if (listName != null && listName.Trim().Length > 0)
			{
				return listName;
			}
		}
		else if (obj is ICustomTypeDescriptor)
		{
			string componentName = ((ICustomTypeDescriptor)obj).GetComponentName();
			if (componentName != null && componentName.Trim().Length > 0)
			{
				return componentName;
			}
		}
		return baseName;
	}

	private void ProcessItem(DataTable table, object obj, Type typeOfData, DataRow row, string tableName, ArrayList tableList, ArrayList tableNames, string uniqueIndex, string columnName)
	{
		if (NeedProcess(obj) || NeedProcess(typeOfData))
		{
			if (row != null)
			{
				row[uniqueIndex] = table.Rows.Count;
			}
			if (level < StiOptions.Dictionary.BusinessObjects.MaxLevel)
			{
				string nameID = "parent";
				string text = GetTableName(tableName, columnName);
				if (obj != null)
				{
					text = GetNameFromObject(obj, text);
				}
				if (obj == null || CheckTable(tableList, tableNames, columnName, obj))
				{
					DataTable dataTable;
					if (obj != null)
					{
						tableList.Add(obj);
						tableNames.Add(columnName);
						dataTable = ConvertBusinessObjectToDataTable(text, tableList, tableNames, obj, typeOfData, ref nameID, table.Rows.Count);
						tableList.RemoveAt(tableList.Count - 1);
						tableNames.RemoveAt(tableNames.Count - 1);
					}
					else
					{
						string nameID2 = "parent";
						dataTable = ConvertBusinessObjectToDataTable(text, tableList, tableNames, null, typeOfData, ref nameID2, table.Rows.Count);
					}
					if (dataTable != null)
					{
						string text2 = text + "Relation";
						if (relations[text2] == null)
						{
							StiRelation value = new StiRelation(text2, tableName, text, uniqueIndex, GetUniqueName(nameID, dataTable));
							relations[text2] = value;
						}
					}
				}
			}
		}
		int num = table.Columns.IndexOf(columnName);
		if (num != -1 && row != null)
		{
			if (obj == null && StiOptions.Dictionary.ConvertNulls.UseDefaultDateTimeForNullValues && table.Columns[num].DataType == typeof(DateTime))
			{
				obj = StiOptions.Dictionary.ConvertNulls.DefaultDateTimeForNullValues;
			}
			row[num] = obj ?? DBNull.Value;
		}
	}

	private string GetUniqueName(string uniqueName, DataTable table)
	{
		if (uniqueName == null)
		{
			return null;
		}
		if (uniques[table] != null && uniques[table] is Hashtable hashtable && hashtable[uniqueName] != null)
		{
			return hashtable[uniqueName] as string;
		}
		int num = -1;
		string text;
		while (true)
		{
			text = "_" + uniqueName;
			if (num == -1)
			{
				text += "ID";
			}
			else if (num >= 0)
			{
				text += $"ID{num}";
			}
			bool flag = true;
			foreach (DataColumn column in table.Columns)
			{
				if (column.ColumnName == text)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				break;
			}
			num++;
		}
		Hashtable hashtable2 = uniques[table] as Hashtable;
		if (hashtable2 == null)
		{
			hashtable2 = new Hashtable();
			uniques[table] = hashtable2;
		}
		hashtable2[uniqueName] = text;
		return text;
	}

	private static Type GetTypeOfArrayElement(Type arrayType)
	{
		if (arrayType == null)
		{
			return null;
		}
		if (arrayType.IsGenericType)
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
		MethodInfo[] methods = arrayType.GetMethods();
		MethodInfo[] array = methods;
		foreach (MethodInfo methodInfo in array)
		{
			if (methodInfo.Name == "get_Item")
			{
				return methodInfo.ReturnType;
			}
		}
		string fullName = arrayType.FullName;
		return StiTypeFinder.GetType(fullName.Substring(0, fullName.Length - 2));
	}

	private DataTable GetTable(string tableName, object obj, Type typeOfData, string nameID, object valueID)
	{
		IEnumerable enumerable = obj as IEnumerable;
		DataTable dataTable = dataSet.Tables[tableName];
		if (dataTable != null)
		{
			return dataTable;
		}
		dataTable = new DataTable(tableName);
		if (enumerable is ITypedList)
		{
			ITypedList typedList = enumerable as ITypedList;
			PropertyDescriptorCollection itemProperties = typedList.GetItemProperties(null);
			foreach (PropertyDescriptor item in itemProperties)
			{
				if (IsAllowUseProperty(item))
				{
					DataColumn dataColumn = GetDataColumn(item.Name, GetAlias(item), item.PropertyType);
					if (!dataTable.Columns.Contains(dataColumn.ColumnName))
					{
						dataTable.Columns.Add(dataColumn);
					}
				}
			}
		}
		else if (enumerable is ICustomTypeDescriptor)
		{
			ICustomTypeDescriptor customTypeDescriptor = enumerable as ICustomTypeDescriptor;
			PropertyDescriptorCollection properties = customTypeDescriptor.GetProperties();
			foreach (PropertyDescriptor item2 in properties)
			{
				if (IsAllowUseProperty(item2))
				{
					DataColumn dataColumn2 = GetDataColumn(item2.Name, GetAlias(item2), item2.PropertyType);
					if (!dataTable.Columns.Contains(dataColumn2.ColumnName))
					{
						dataTable.Columns.Add(dataColumn2);
					}
				}
			}
		}
		else
		{
			Type type = null;
			Type type2 = null;
			if (enumerable != null)
			{
				type2 = enumerable.GetType();
			}
			if (type2 != null && type2.IsGenericType)
			{
				Type[] genericArguments = type2.GetGenericArguments();
				if (genericArguments.Length != 1)
				{
					type = typeof(object);
				}
				else if (genericArguments.Length == 1)
				{
					type = genericArguments[0];
				}
			}
			if (typeOfData != null)
			{
				type = typeOfData;
			}
			object obj2 = null;
			if (type == null)
			{
				obj2 = ((enumerable != null) ? GetValueFromEnumerable(enumerable) : obj);
				if (obj2 != null)
				{
					type = obj2.GetType();
				}
			}
			if (obj2 == null && type == null && type2 != null)
			{
				type = type2;
				type = GetTypeOfArrayElement(type);
			}
			if (obj2 != null || type != null)
			{
				if ((!type.IsClass || obj2 is string || obj2 is Image || obj2 is IEnumerable) && !(obj2 is DictionaryEntry) && type.ToString().IndexOf("KeyValuePair", StringComparison.InvariantCulture) == -1 && obj2 != null)
				{
					if (StiOptions.Dictionary.BusinessObjects.AllowUseDataColumn && !dataTable.Columns.Contains("Data"))
					{
						DataColumn dataColumn3 = GetDataColumn("Data", obj2.GetType());
						dataTable.Columns.Add(dataColumn3);
					}
				}
				else if (obj2 is ICustomTypeDescriptor)
				{
					ICustomTypeDescriptor customTypeDescriptor2 = obj2 as ICustomTypeDescriptor;
					PropertyDescriptorCollection properties2 = customTypeDescriptor2.GetProperties();
					foreach (PropertyDescriptor item3 in properties2)
					{
						if (IsAllowUseProperty(item3))
						{
							DataColumn dataColumn4 = GetDataColumn(item3.Name, GetAlias(item3), item3.PropertyType);
							if (!dataTable.Columns.Contains(dataColumn4.ColumnName))
							{
								dataTable.Columns.Add(dataColumn4);
							}
						}
					}
				}
				else
				{
					if (StiOptions.Dictionary.BusinessObjects.AllowUseProperties)
					{
						PropertyInfo[] array = null;
						if (obj2 == null)
						{
							if (type != typeof(string) && StiTypeFinder.FindInterface(type, typeof(IEnumerable)))
							{
								type = GetTypeOfArrayElement(type);
							}
							if (type != null)
							{
								array = type.GetProperties();
							}
						}
						else
						{
							array = obj2.GetType().GetProperties();
						}
						if (array != null)
						{
							PropertyInfo[] array2 = array;
							foreach (PropertyInfo propertyInfo in array2)
							{
								if (IsAllowUseProperty(propertyInfo) && !(propertyInfo.DeclaringType == typeof(string)) && !(propertyInfo.DeclaringType == typeof(Array)))
								{
									DataColumn dataColumn5 = GetDataColumn(propertyInfo.Name, GetAlias(propertyInfo), propertyInfo.PropertyType);
									if (!dataTable.Columns.Contains(dataColumn5.ColumnName))
									{
										dataTable.Columns.Add(dataColumn5);
									}
								}
							}
						}
					}
					if (StiOptions.Dictionary.BusinessObjects.AllowUseFields)
					{
						FieldInfo[] array3 = null;
						if (obj2 == null)
						{
							if (type != typeof(string) && StiTypeFinder.FindInterface(type, typeof(IEnumerable)))
							{
								type = GetTypeOfArrayElement(type);
							}
							if (type != null)
							{
								array3 = type.GetFields();
							}
						}
						else
						{
							array3 = obj2.GetType().GetFields();
						}
						if (array3 != null)
						{
							FieldInfo[] array4 = array3;
							foreach (FieldInfo fieldInfo in array4)
							{
								if (IsAllowUseField(fieldInfo))
								{
									DataColumn dataColumn6 = GetDataColumn(fieldInfo.Name, GetAlias(fieldInfo), fieldInfo.FieldType);
									if (!dataTable.Columns.Contains(dataColumn6.ColumnName))
									{
										dataTable.Columns.Add(dataColumn6);
									}
								}
							}
						}
					}
				}
			}
		}
		string uniqueName = GetUniqueName(string.Empty, dataTable);
		DataColumn column = new DataColumn(uniqueName, typeof(int));
		dataTable.Columns.Add(column);
		if (nameID != null)
		{
			nameID = GetUniqueName(nameID, dataTable);
			dataTable.Columns.Add(GetDataColumn(nameID, valueID.GetType()));
		}
		DataColumn column2 = new DataColumn("_Current", typeof(object));
		dataTable.Columns.Add(column2);
		dataSet.Tables.Add(dataTable);
		return dataTable;
	}

	private object GetValueFromEnumerable(IEnumerable enumerable)
	{
		IEnumerator enumerator = enumerable.GetEnumerator();
		if (StiOptions.Engine.AllowUseResetMethodInBusinessObject)
		{
			try
			{
				enumerator.Reset();
			}
			catch
			{
			}
		}
		while (enumerator.MoveNext())
		{
			if (enumerator.Current != null)
			{
				return enumerator.Current;
			}
		}
		return null;
	}

	private void ProcessUniqueIndex(string nameID, object valueID, DataTable table, DataRow row)
	{
		if (nameID != null)
		{
			string uniqueName = GetUniqueName(nameID, table);
			if (table.Columns[uniqueName] == null)
			{
				table.Columns.Add(uniqueName, typeof(int));
			}
			row[uniqueName] = valueID;
		}
	}

	private void FillDataTableFromType(DataTable table, string tableName, ArrayList tableList, ArrayList tableNames, string nameID, object valueID)
	{
		foreach (DataColumn column in table.Columns)
		{
			if (!(column.ColumnName == "_Current") && !(column.ColumnName == "Data") && !(column.ColumnName == "_parentID") && !(column.ColumnName == "_ID"))
			{
				string uniqueName = GetUniqueName(string.Empty, table);
				ProcessItem(table, null, column.DataType, null, tableName, tableList, tableNames, uniqueName, column.ColumnName);
			}
		}
	}

	private void FillDataTable(DataTable table, string tableName, ArrayList tableList, ArrayList tableNames, object obj, string nameID, object valueID)
	{
		if (obj == null)
		{
			FillDataTableFromType(table, tableName, tableList, tableNames, nameID, valueID);
			return;
		}
		IEnumerable enumerable = obj as IEnumerable;
		object obj2 = ((enumerable != null) ? GetValueFromEnumerable(enumerable) : obj);
		string uniqueName = GetUniqueName(string.Empty, table);
		if (enumerable is ITypedList || enumerable is ICustomTypeDescriptor)
		{
			ITypedList typedList = enumerable as ITypedList;
			ICustomTypeDescriptor customTypeDescriptor = enumerable as ICustomTypeDescriptor;
			PropertyDescriptorCollection propertyDescriptorCollection = ((typedList != null) ? typedList.GetItemProperties(null) : customTypeDescriptor.GetProperties());
			IEnumerator enumerator = enumerable.GetEnumerator();
			if (StiOptions.Engine.AllowUseResetMethodInBusinessObject)
			{
				try
				{
					enumerator.Reset();
				}
				catch
				{
				}
			}
			while (enumerator.MoveNext())
			{
				if (enumerator.Current == null)
				{
					continue;
				}
				foreach (PropertyDescriptor item in propertyDescriptorCollection)
				{
					if (table.Columns[item.Name] == null)
					{
						DataColumn dataColumn = GetDataColumn(item.Name, GetAlias(item), item.PropertyType);
						table.Columns.Add(dataColumn);
					}
				}
				DataRow dataRow = table.NewRow();
				dataRow["_Current"] = enumerator.Current;
				foreach (PropertyDescriptor item2 in propertyDescriptorCollection)
				{
					try
					{
						object value = item2.GetValue(enumerator.Current);
						if (value != null || StiOptions.Dictionary.BusinessObjects.AllowProcessNullItemsInEnumerables)
						{
							ProcessItem(table, value, item2.PropertyType, dataRow, tableName, tableList, tableNames, uniqueName, item2.Name);
						}
					}
					catch
					{
					}
				}
				ProcessUniqueIndex(nameID, valueID, table, dataRow);
				table.Rows.Add(dataRow);
			}
			return;
		}
		Hashtable hashtable = new Hashtable();
		if (enumerable == null)
		{
			FillOneItem(table, obj2, tableName, tableList, tableNames, nameID, valueID, uniqueName, hashtable);
		}
		else
		{
			IEnumerator enumerator4 = enumerable.GetEnumerator();
			if (StiOptions.Engine.AllowUseResetMethodInBusinessObject)
			{
				try
				{
					enumerator4.Reset();
				}
				catch
				{
				}
			}
			bool flag = false;
			ArrayList arrayList = new ArrayList();
			while (enumerator4.MoveNext())
			{
				arrayList.Add(enumerator4.Current);
			}
			foreach (object item3 in arrayList)
			{
				FillOneItem(table, item3, tableName, tableList, tableNames, nameID, valueID, uniqueName, hashtable);
				flag = true;
			}
			if (!flag)
			{
				FillDataTableFromType(table, tableName, tableList, tableNames, nameID, valueID);
			}
		}
		hashtable.Clear();
	}

	private void FillOneItem(DataTable table, object obj, string tableName, ArrayList tableList, ArrayList tableNames, string nameID, object valueID, string uniqueIndex, Hashtable hashIsAllowUseProperty)
	{
		DataRow dataRow = null;
		if (obj == null)
		{
			return;
		}
		if ((!obj.GetType().IsClass || obj is string || obj is Image || obj is IEnumerable) && !(obj is DictionaryEntry) && obj.GetType().ToString().IndexOf("KeyValuePair", StringComparison.InvariantCulture) == -1)
		{
			dataRow = table.NewRow();
			if (StiOptions.Dictionary.BusinessObjects.AllowUseDataColumn && !table.Columns.Contains("Data"))
			{
				DataColumn dataColumn = GetDataColumn("Data", obj.GetType());
				table.Columns.Add(dataColumn);
			}
			dataRow["_Current"] = obj;
			if (StiOptions.Dictionary.BusinessObjects.AllowUseDataColumn)
			{
				dataRow["Data"] = obj;
			}
			ProcessItem(table, obj, null, dataRow, tableName, tableList, tableNames, uniqueIndex, level.ToString());
		}
		else if (obj is ICustomTypeDescriptor)
		{
			ICustomTypeDescriptor customTypeDescriptor = obj as ICustomTypeDescriptor;
			PropertyDescriptorCollection properties = customTypeDescriptor.GetProperties();
			foreach (PropertyDescriptor item in properties)
			{
				if (IsAllowUseProperty(item) && table.Columns[item.Name] == null)
				{
					DataColumn dataColumn2 = GetDataColumn(item.Name, GetAlias(item), item.PropertyType);
					table.Columns.Add(dataColumn2);
				}
			}
			dataRow = table.NewRow();
			dataRow["_Current"] = obj;
			foreach (PropertyDescriptor item2 in properties)
			{
				if (IsAllowUseProperty(item2))
				{
					try
					{
						object value = item2.GetValue(obj);
						ProcessItem(table, value, null, dataRow, tableName, tableList, tableNames, uniqueIndex, item2.Name);
					}
					catch
					{
					}
				}
			}
		}
		else
		{
			if (StiOptions.Dictionary.BusinessObjects.AllowUseProperties)
			{
				PropertyInfo[] properties2 = obj.GetType().GetProperties();
				PropertyInfo[] array = properties2;
				foreach (PropertyInfo propertyInfo in array)
				{
					if (IsAllowUseProperty(propertyInfo, hashIsAllowUseProperty) && table.Columns[propertyInfo.Name] == null)
					{
						DataColumn dataColumn3 = GetDataColumn(propertyInfo.Name, GetAlias(propertyInfo), propertyInfo.PropertyType);
						table.Columns.Add(dataColumn3);
					}
				}
				dataRow = table.NewRow();
				dataRow["_Current"] = obj;
				PropertyInfo[] array2 = properties2;
				foreach (PropertyInfo propertyInfo2 in array2)
				{
					if (!IsAllowUseProperty(propertyInfo2, hashIsAllowUseProperty))
					{
						continue;
					}
					try
					{
						if (propertyInfo2.CanRead)
						{
							object value2 = propertyInfo2.GetValue(obj, null);
							ProcessItem(table, value2, propertyInfo2.PropertyType, dataRow, tableName, tableList, tableNames, uniqueIndex, propertyInfo2.Name);
						}
					}
					catch
					{
					}
				}
			}
			if (StiOptions.Dictionary.BusinessObjects.AllowUseFields)
			{
				FieldInfo[] fields = obj.GetType().GetFields();
				FieldInfo[] array3 = fields;
				foreach (FieldInfo fieldInfo in array3)
				{
					if (IsAllowUseField(fieldInfo) && table.Columns[fieldInfo.Name] == null)
					{
						DataColumn dataColumn4 = GetDataColumn(fieldInfo.Name, GetAlias(fieldInfo), fieldInfo.FieldType);
						table.Columns.Add(dataColumn4);
					}
				}
				if (dataRow == null)
				{
					dataRow = table.NewRow();
				}
				dataRow["_Current"] = obj;
				FieldInfo[] array4 = fields;
				foreach (FieldInfo fieldInfo2 in array4)
				{
					if (IsAllowUseField(fieldInfo2))
					{
						try
						{
							object value3 = fieldInfo2.GetValue(obj);
							ProcessItem(table, value3, null, dataRow, tableName, tableList, tableNames, uniqueIndex, fieldInfo2.Name);
						}
						catch
						{
						}
					}
				}
			}
		}
		if (dataRow == null)
		{
			dataRow = table.NewRow();
		}
		ProcessUniqueIndex(nameID, valueID, table, dataRow);
		for (int m = 0; m < dataRow.ItemArray.Length; m++)
		{
			if (dataRow.ItemArray[m] == DBNull.Value && !table.Columns[m].AllowDBNull)
			{
				table.Columns[m].AllowDBNull = true;
			}
		}
		table.Rows.Add(dataRow);
	}

	private DataTable ConvertBusinessObjectToDataTable(string tableName, ArrayList tableList, ArrayList tableNames, object obj, Type typeOfData, ref string nameID, object valueID)
	{
		level++;
		if (obj != null)
		{
			tableName = GetNameFromObject(obj, tableName);
		}
		DataTable table = GetTable(tableName, obj, typeOfData, nameID, valueID);
		if (table != null)
		{
			FillDataTable(table, tableName, tableList, tableNames, obj, nameID, valueID);
		}
		level--;
		return table;
	}

	public DataSet ConvertBusinessObjectToDataSet(string name, object obj)
	{
		dataSet = new DataSet(name);
		relations = new Hashtable();
		uniques = new Hashtable();
		ArrayList tableList = new ArrayList();
		ArrayList tableNames = new ArrayList();
		level = 0;
		string nameID = null;
		ConvertBusinessObjectToDataTable(name, tableList, tableNames, obj, null, ref nameID, null);
		StiRelation[] array = new StiRelation[relations.Count];
		relations.Values.CopyTo(array, 0);
		StiRelation[] array2 = array;
		foreach (StiRelation stiRelation in array2)
		{
			try
			{
				DataColumn[] array3 = new DataColumn[1];
				int index = dataSet.Tables.IndexOf(stiRelation.ParentTableName);
				array3[0] = dataSet.Tables[index].Columns[stiRelation.ParentColumnName];
				DataColumn[] array4 = new DataColumn[1];
				int index2 = dataSet.Tables.IndexOf(stiRelation.ChildTableName);
				array4[0] = dataSet.Tables[index2].Columns[stiRelation.ChildColumnName];
				dataSet.Relations.Add(stiRelation.Name, array3, array4);
			}
			catch
			{
			}
		}
		relations.Clear();
		relations = null;
		uniques.Clear();
		uniques = null;
		return dataSet;
	}
}
