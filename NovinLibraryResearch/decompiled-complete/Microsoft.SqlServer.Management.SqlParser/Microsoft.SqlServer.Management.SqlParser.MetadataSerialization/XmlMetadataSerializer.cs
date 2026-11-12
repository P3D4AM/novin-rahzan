using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Microsoft.SqlServer.Management.SqlParser.Common;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal class XmlMetadataSerializer : ISerializationHandler<object>, IDisposable
{
	private sealed class SerializationFilter : ISerializationFilter
	{
		private readonly IXmlSerializationFilter xmlFilter;

		public SerializationFilter(IXmlSerializationFilter xmlFilter)
		{
			this.xmlFilter = xmlFilter;
		}

		public bool IsIncluded(IMetadataObject metadataObject)
		{
			return xmlFilter.IsIncluded(metadataObject);
		}
	}

	private XmlWriter writer;

	private MetadataSerializerCore<object> serializerCore;

	private SerializationIdGenerator idGenerator;

	private ISerializationFilter filter;

	private IXmlSerializationSettings serializationSettings;

	private Dictionary<IMetadataObject, IMetadataObject> valueParentMap;

	private Queue<IMetadataObject> workList;

	public XmlMetadataSerializer(XmlWriter writer, IXmlSerializationSettings serializationSettings, IXmlSerializationFilter xmlFilter, Func<IMetadataObject, MetadataObjectSelector> objectSelector)
	{
		this.writer = writer;
		this.serializationSettings = serializationSettings;
		filter = new SerializationFilter(xmlFilter);
		serializerCore = new MetadataSerializerCore<object>(this, filter, SerializationOptions.ExcludeServerName);
		valueParentMap = new Dictionary<IMetadataObject, IMetadataObject>();
		idGenerator = new SerializationIdGenerator(valueParentMap, objectSelector);
		workList = new Queue<IMetadataObject>();
		XmlUtils.WriteRootElementStart(this.writer, XmlConfig.RootElement, new XmlNamespace[2]
		{
			XmlConfig.MM,
			XmlConfig.RE
		});
	}

	public void Serialize(IDatabaseObject dbObject)
	{
		RegisterValue(dbObject, dbObject.Parent, isChild: true);
		while (workList.Count > 0)
		{
			IMetadataObject obj = workList.Dequeue();
			DataRow<object> metadataObjectData = serializerCore.GetMetadataObjectData(obj);
			MetadataObjectId id = idGenerator.GenerateId(obj);
			WriteObjectData(metadataObjectData, id, XmlNestingType.Standalone);
		}
	}

	public void Close()
	{
		XmlUtils.WriteEndElement(writer);
		writer = null;
		serializerCore = null;
	}

	public void WriteObjectData(DataRow<object> dataRow, MetadataObjectId id, XmlNestingType nestingType)
	{
		if (!XmlConfig.IsStaticTypeNesting(nestingType))
		{
			XmlUtils.WriteStartElement(writer, XmlConfig.GetInstanceElementName(dataRow.TypeInfo));
			if (!id.IsNull)
			{
				WriteObjectKey(id);
			}
		}
		MetadataPropertyInfo parentProperty = dataRow.TypeInfo.ParentProperty;
		if (!XmlConfig.IsInlineNesting(nestingType) && XmlConfig.IncludeParentProperty(dataRow.TypeInfo) && !serializationSettings.GetPropertyOverrideValue(dataRow.TypeInfo, parentProperty, out var _))
		{
			XmlUtils.WriteStartElement(writer, XmlConfig.ParentProperty);
			WriteObjectRef((MetadataObjectId)dataRow.Values[parentProperty.Index]);
			XmlUtils.WriteEndElement(writer);
		}
		for (int i = 0; i < dataRow.Values.Length; i++)
		{
			if (dataRow.Values[i] == null)
			{
				continue;
			}
			MetadataPropertyInfo metadataPropertyInfo = dataRow.TypeInfo.InstanceProperties[i];
			if (metadataPropertyInfo == parentProperty || !XmlConfig.IncludeProperty(dataRow.TypeInfo, metadataPropertyInfo, serializationSettings) || serializationSettings.GetPropertyOverrideValue(dataRow.TypeInfo, metadataPropertyInfo, out var _))
			{
				continue;
			}
			XmlQualifiedNameEx propertyElementName = XmlConfig.GetPropertyElementName(dataRow.TypeInfo, dataRow.TypeInfo.InstanceProperties[i]);
			TypeInfo returnType = metadataPropertyInfo.ReturnType;
			if (returnType.IsCollection)
			{
				MetadataTypeInfo metadataTypeInfo = (MetadataTypeInfo)((CollectionTypeInfo)returnType).ElementType;
				object[] array;
				if (XmlConfig.IsInlineNesting(serializationSettings.GetNestingType(metadataTypeInfo)))
				{
					XmlUtils.WriteStartElement(writer, propertyElementName);
					array = (object[])dataRow.Values[i];
					foreach (object obj in array)
					{
						WriteInlinedObject((DataRow<object>)obj, metadataTypeInfo);
					}
					XmlUtils.WriteEndElement(writer);
					continue;
				}
				XmlUtils.WriteStartElement(writer, propertyElementName);
				array = (object[])dataRow.Values[i];
				foreach (object obj2 in array)
				{
					XmlUtils.WriteStartElement(writer, XmlConfig.ReferenceElement);
					WriteObjectRef((MetadataObjectId)obj2);
					XmlUtils.WriteEndElement(writer);
				}
				XmlUtils.WriteEndElement(writer);
			}
			else if (returnType.IsMetadata)
			{
				XmlUtils.WriteStartElement(writer, propertyElementName);
				MetadataTypeInfo formalTypeInfo = (MetadataTypeInfo)returnType;
				if (dataRow.Values[i] is DataRow<object>)
				{
					WriteInlinedObject((DataRow<object>)dataRow.Values[i], formalTypeInfo);
				}
				else
				{
					WriteObjectRef((MetadataObjectId)dataRow.Values[i]);
				}
				XmlUtils.WriteEndElement(writer);
			}
			else
			{
				XmlUtils.WriteStartElement(writer, propertyElementName);
				WritePrimitive((PrimitiveTypeInfo)returnType, dataRow.Values[i]);
				XmlUtils.WriteEndElement(writer);
			}
		}
		if (!XmlConfig.IsStaticTypeNesting(nestingType))
		{
			XmlUtils.WriteEndElement(writer);
		}
	}

	private void WriteInlinedObject(DataRow<object> dataRow, TypeInfo formalTypeInfo)
	{
		XmlNestingType nestingType = serializationSettings.GetNestingType(dataRow.TypeInfo);
		WriteObjectData(dataRow, MetadataObjectId.Null, nestingType);
	}

	private static string GetStringFromId(MetadataObjectId id)
	{
		if (id.Selectors.Count == 0)
		{
			return "/";
		}
		StringBuilder stringBuilder = new StringBuilder();
		foreach (MetadataObjectSelector selector in id.Selectors)
		{
			string name = XmlConfig.GetInstanceElementName(selector.TypeInfo).Name;
			stringBuilder.Append("/" + name + "[" + selector.Name.Replace("]", "]]") + "]");
		}
		return stringBuilder.ToString();
	}

	private void WriteObjectKey(MetadataObjectId id)
	{
		XmlUtils.WriteAttribute(writer, XmlConfig.KeyAttribute, GetStringFromId(id));
	}

	private void WriteObjectRef(MetadataObjectId id)
	{
		XmlUtils.WriteAttribute(writer, XmlConfig.RefAttribute, GetStringFromId(id));
	}

	private void WritePrimitive(PrimitiveTypeInfo typeInfo, object value)
	{
		int id = typeInfo.Id;
		if (id <= 89)
		{
			switch (id)
			{
			default:
				_ = 89;
				return;
			case 1:
			case 2:
			case 11:
				break;
			case 0:
			case 4:
			case 7:
			case 9:
			case 10:
			case 12:
			case 13:
			case 14:
				goto IL_00af;
			case 5:
				XmlUtils.WriteStartElement(writer, XmlConfig.ICollation_Name);
				writer.WriteString(((CollationInfo)value).Name);
				XmlUtils.WriteEndElement(writer);
				return;
			case 6:
			{
				ComputedColumnInfo computedColumnInfo = (ComputedColumnInfo)value;
				XmlUtils.WriteStartElement(writer, XmlConfig.TextProperty);
				writer.WriteValue(computedColumnInfo.Text);
				XmlUtils.WriteEndElement(writer);
				XmlUtils.WriteStartElement(writer, XmlConfig.IsPersistedProperty);
				writer.WriteValue(computedColumnInfo.IsPersisted);
				XmlUtils.WriteEndElement(writer);
				return;
			}
			case 8:
				writer.WriteString(((DataTypeSpec)value).SqlDataType.ToString());
				return;
			case 3:
				return;
			}
		}
		else
		{
			switch (id)
			{
			case 139:
			case 146:
				break;
			case 138:
			case 141:
			case 142:
			case 143:
			case 144:
			case 145:
			case 147:
			case 148:
			case 149:
				goto IL_00af;
			case 137:
			{
				IdentityColumnInfo identityColumnInfo = (IdentityColumnInfo)value;
				XmlUtils.WriteStartElement(writer, XmlConfig.SeedProperty);
				writer.WriteValue(identityColumnInfo.Seed);
				XmlUtils.WriteEndElement(writer);
				XmlUtils.WriteStartElement(writer, XmlConfig.IncrementProperty);
				writer.WriteValue(identityColumnInfo.Increment);
				XmlUtils.WriteEndElement(writer);
				XmlUtils.WriteStartElement(writer, XmlConfig.NotForReplicationProperty);
				writer.WriteValue(identityColumnInfo.NotForReplication);
				XmlUtils.WriteEndElement(writer);
				return;
			}
			case 117:
				WriteTriggerEventSet((ITriggerEventTypeSet)value);
				return;
			default:
				return;
			}
		}
		writer.WriteString(value.ToString());
		return;
		IL_00af:
		writer.WriteString(Enum.GetName(typeInfo.Type, value));
	}

	private void WriteTriggerEventSet(ITriggerEventTypeSet value)
	{
		bool flag = true;
		foreach (string item in value)
		{
			if (!flag)
			{
				writer.WriteString(", ");
			}
			flag = false;
			writer.WriteString(item);
		}
	}

	public object RegisterValue(IMetadataObject obj, IMetadataObject parentObj, bool isChild)
	{
		if (obj == null)
		{
			return null;
		}
		SerializationInfo serializationInfo = serializerCore.GetSerializationInfo(obj);
		object result;
		if (XmlConfig.IsInlineNesting(serializationSettings.GetNestingType(serializationInfo.TypeInfo)))
		{
			result = serializerCore.GetMetadataObjectData(obj);
		}
		else
		{
			bool isValue = serializationInfo.TypeInfo.IsValue;
			if (isValue)
			{
				valueParentMap[obj] = parentObj;
			}
			result = idGenerator.GenerateId(obj);
			if ((isChild || isValue) && filter.IsIncluded(obj))
			{
				workList.Enqueue(obj);
			}
		}
		return result;
	}

	public object RegisterValue<T>(IMetadataCollection<T> obj, IMetadataObject parentObj, bool isChild) where T : IMetadataObject
	{
		return serializerCore.GetMetadataCollectionData(obj, parentObj, isChild);
	}

	public object RegisterValue(bool value)
	{
		return value;
	}

	public object RegisterValue(byte value)
	{
		return value;
	}

	public object RegisterValue(byte[] value)
	{
		return value;
	}

	public object RegisterValue(double value)
	{
		return value;
	}

	public object RegisterValue(int value)
	{
		return value;
	}

	public object RegisterValue(string value)
	{
		return value;
	}

	public object RegisterValue(ActivationOrder value)
	{
		return (int)value;
	}

	public object RegisterValue(DatabaseCompatibilityLevel value)
	{
		return (int)value;
	}

	public object RegisterValue(ExecutionContextType value)
	{
		return value;
	}

	public object RegisterValue(ForeignKeyAction value)
	{
		return value;
	}

	public object RegisterValue(GridDensity value)
	{
		return value;
	}

	public object RegisterValue(LoginType value)
	{
		return value;
	}

	public object RegisterValue(SortOrder value)
	{
		return value;
	}

	public object RegisterValue(TabularType value)
	{
		return value;
	}

	public object RegisterValue(UserType value)
	{
		return value;
	}

	public object RegisterValue(DatabasePermissionType value)
	{
		return value;
	}

	public object RegisterValue(PermissionState value)
	{
		return value;
	}

	public object RegisterValue(SynonymBaseType value)
	{
		return value;
	}

	public object RegisterValue(ComputedColumnInfo value)
	{
		return value;
	}

	public object RegisterValue(IdentityColumnInfo value)
	{
		return value;
	}

	public object RegisterValue(IndexType value)
	{
		return value;
	}

	public object RegisterValue(StatisticsType value)
	{
		return value;
	}

	public object RegisterValue(IPassword value)
	{
		return value;
	}

	public object RegisterValue(ITriggerEventTypeSet value)
	{
		return value;
	}

	public object RegisterValue(CollationInfo value)
	{
		return value;
	}

	public object RegisterValue(DataTypeSpec value)
	{
		return value;
	}

	public void Dispose()
	{
		Close();
	}
}
