using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal class XmlMetadataDeserializer
{
	private sealed class OnCreateHandler : IDeserializationHandler
	{
		private readonly IXmlDeserializationHandler xmlHandler;

		public OnCreateHandler(IXmlDeserializationHandler xmlHandler)
		{
			this.xmlHandler = xmlHandler;
		}

		public IMetadataObject OnCreateEvent(IMetadataObject obj)
		{
			return xmlHandler.OnCreatEvent(obj);
		}
	}

	private abstract class ExternalReference
	{
		private sealed class ConstantExternalReference : ExternalReference
		{
			private readonly IMetadataObject metadataObject;

			public override IMetadataObject MetadataObject => metadataObject;

			public ConstantExternalReference(int id, IEnumerable<TypeInfo> typeConstraints, IMetadataObject metadataObject)
				: base(id, typeConstraints)
			{
				this.metadataObject = metadataObject;
			}
		}

		private sealed class StrIdExternalReference : ExternalReference
		{
			private readonly string strId;

			private readonly XmlIdResolver idResolver;

			private IMetadataObject metadataObject;

			public override IMetadataObject MetadataObject
			{
				get
				{
					if (metadataObject == null)
					{
						metadataObject = ((idResolver != null) ? idResolver.ResolveObject(strId) : null);
						if (metadataObject == null)
						{
							throw new DeserializationException(SR.DeserializationUnresolvedExternalReference(strId));
						}
					}
					return metadataObject;
				}
			}

			public StrIdExternalReference(int id, IEnumerable<TypeInfo> typeConstraints, string strId, XmlIdResolver idResolver)
				: base(id, typeConstraints)
			{
				this.strId = strId;
				this.idResolver = idResolver;
			}
		}

		private readonly int id;

		private readonly IEnumerable<TypeInfo> typeConstraints;

		public int Id => id;

		public IEnumerable<TypeInfo> TypeConstraints => typeConstraints;

		public abstract IMetadataObject MetadataObject { get; }

		private ExternalReference(int id, IEnumerable<TypeInfo> typeConstraints)
		{
			this.id = id;
			this.typeConstraints = typeConstraints;
		}

		public static ExternalReference CreateConstantExternalReference(int id, IEnumerable<TypeInfo> typeConstraints, IMetadataObject metadataObject)
		{
			return new ConstantExternalReference(id, typeConstraints, metadataObject);
		}

		public static ExternalReference CreateStrIdExternalReference(int id, IEnumerable<TypeInfo> typeConstraints, string strId, XmlIdResolver idResolver)
		{
			return new StrIdExternalReference(id, typeConstraints, strId, idResolver);
		}
	}

	private sealed class DeserializationDataProvider : IDeserializationDataProvider
	{
		private readonly List<DataRow<object>> dataRows;

		private readonly List<List<int>> collections;

		private readonly Dictionary<int, ExternalReference> externalReferences;

		public int ObjectCount => dataRows.Count;

		public int CollectionCount => collections.Count;

		public DeserializationDataProvider(List<DataRow<object>> dataRows, List<List<int>> collections, IEnumerable<ExternalReference> externalReferences)
		{
			this.dataRows = dataRows;
			this.collections = collections;
			this.externalReferences = new Dictionary<int, ExternalReference>();
			foreach (ExternalReference externalReference in externalReferences)
			{
				this.externalReferences.Add(externalReference.Id, externalReference);
			}
		}

		public DataRow<object> GetObjectData(int id)
		{
			return dataRows[id];
		}

		public IMetadataObject GetExternalObjectById(int id)
		{
			ExternalReference externalReference = externalReferences[id];
			IMetadataObject metadataObject = externalReference.MetadataObject;
			if (externalReference.TypeConstraints != null)
			{
				ValidateTypeConstraints(metadataObject.GetType(), externalReference.TypeConstraints);
			}
			return metadataObject;
		}

		public List<int> GetCollectionData(int id)
		{
			return collections[id];
		}
	}

	private readonly XmlReader reader;

	private readonly IXmlSerializationSettings serializationSettings;

	private readonly IXmlDeserializationHandler xmlHandler;

	private readonly List<DataRow<object>> dataRows;

	private readonly List<List<TypeInfo>> dataRowTypeConstraints;

	private readonly List<int> dataRowParents;

	private readonly List<List<int>> collections;

	private readonly List<string> dataRowsIds;

	private readonly Dictionary<string, int> objectIdMap;

	private readonly Dictionary<IMetadataObject, int> externalMetadataObjects;

	private int dataRowId;

	private int collectionId;

	public XmlMetadataDeserializer(XmlReader reader, IXmlSerializationSettings serializationSettings, IXmlDeserializationHandler xmlHandler)
	{
		this.reader = reader;
		this.serializationSettings = serializationSettings;
		this.xmlHandler = xmlHandler;
		objectIdMap = new Dictionary<string, int>();
		externalMetadataObjects = new Dictionary<IMetadataObject, int>();
		dataRows = new List<DataRow<object>>();
		dataRowTypeConstraints = new List<List<TypeInfo>>();
		dataRowParents = new List<int>();
		collections = new List<List<int>>();
		dataRowsIds = new List<string>();
		dataRows.Add(default(DataRow<object>));
		dataRowTypeConstraints.Add(null);
		dataRowParents.Add(0);
		collections.Add(null);
		dataRowsIds.Add(null);
	}

	public IServer Deserialize(IServer sourceServer)
	{
		XmlScanner scanner = new XmlScanner(reader);
		ReadXml(scanner);
		UpdateParentProperties();
		ValidateTypeConstraints();
		XmlIdResolver idResolver = ((sourceServer != null) ? new XmlIdResolver(sourceServer) : null);
		DeserializationModel deserializationModel = new DeserializationModel(new DeserializationDataProvider(dataRows, collections, GetExternalReferences(idResolver)), new OnCreateHandler(xmlHandler));
		InstantiateTopLevelObjects(deserializationModel);
		if (sourceServer == null)
		{
			int id = objectIdMap["/"];
			sourceServer = deserializationModel.GetObjectById<IServer>(id);
		}
		return sourceServer;
	}

	private IEnumerable<ExternalReference> GetExternalReferences(XmlIdResolver idResolver)
	{
		Dictionary<int, bool> visitedIds = new Dictionary<int, bool>();
		foreach (KeyValuePair<IMetadataObject, int> externalMetadataObject in externalMetadataObjects)
		{
			int id2 = externalMetadataObject.Value;
			yield return ExternalReference.CreateConstantExternalReference(id2, dataRowTypeConstraints[id2], externalMetadataObject.Key);
			visitedIds.Add(id2, value: true);
		}
		for (int id2 = 1; id2 < dataRows.Count; id2++)
		{
			if (!visitedIds.ContainsKey(id2) && dataRows[id2].TypeInfo == null)
			{
				yield return ExternalReference.CreateStrIdExternalReference(id2, dataRowTypeConstraints[id2], dataRowsIds[id2], idResolver);
			}
		}
	}

	private void InstantiateTopLevelObjects(DeserializationModel model)
	{
		for (int i = 1; i < dataRows.Count; i++)
		{
			if (dataRows[i].TypeInfo != null)
			{
				int num = dataRowParents[i];
				if (num == 0 || dataRows[num].TypeInfo == null)
				{
					model.GetObjectById<IMetadataObject>(i);
				}
			}
		}
	}

	private void UpdateParentProperties()
	{
		for (int i = 1; i < dataRows.Count; i++)
		{
			DataRow<object> dataRow = dataRows[i];
			if (dataRow.TypeInfo == null)
			{
				continue;
			}
			int num = dataRowParents[i];
			MetadataPropertyInfo parentProperty = dataRow.TypeInfo.ParentProperty;
			if (parentProperty != null)
			{
				if (num == 0)
				{
					throw new DeserializationException("Unable to infer parent id for object.");
				}
				dataRow.Values[parentProperty.Index] = num;
			}
			if (num <= 0)
			{
				continue;
			}
			MetadataTypeInfo typeInfo = dataRows[num].TypeInfo;
			if (typeInfo != null)
			{
				MetadataPropertyInfo collectionProperty = XmlConfig.GetCollectionProperty(typeInfo, dataRow.TypeInfo);
				if (collectionProperty != null && !XmlConfig.IncludeProperty(typeInfo, collectionProperty, serializationSettings))
				{
					int index = (int)dataRows[num].Values[collectionProperty.Index];
					collections[index].Add(i);
				}
			}
		}
	}

	private static void ValidateTypeConstraints(Type type, IEnumerable<TypeInfo> constraints)
	{
		foreach (TypeInfo constraint in constraints)
		{
			if (!constraint.Type.GetIsAssignableFrom(type))
			{
				throw new DeserializationException(SR.DeserializationInvalidObjectType(type.Name, constraint.Name));
			}
		}
	}

	private void ValidateTypeConstraints()
	{
		for (int i = 1; i < dataRows.Count; i++)
		{
			DataRow<object> dataRow = dataRows[i];
			List<TypeInfo> list = dataRowTypeConstraints[i];
			if (dataRow.TypeInfo != null && list != null)
			{
				ValidateTypeConstraints(dataRow.TypeInfo.Type, list);
			}
		}
	}

	private void ReadXml(XmlScanner scanner)
	{
		scanner.MatchElement(XmlConfig.RootElement, throwOnError: true);
		scanner.MoveToFirstChild();
		while (!scanner.IsEndOfChildren)
		{
			ReadMetadataObject(scanner, 0, null);
			scanner.MoveToNext();
		}
		scanner.MoveUp();
		scanner.MoveToNext();
		if (!scanner.IsEndOfFile)
		{
			throw new InvalidXmlException(SR.DeserializationUnexpectedXmlContent);
		}
	}

	private int NewDataRowId()
	{
		int result = ++dataRowId;
		dataRows.Add(default(DataRow<object>));
		dataRowTypeConstraints.Add(null);
		dataRowParents.Add(0);
		dataRowsIds.Add(null);
		return result;
	}

	private int RegisterCollection(List<int> items)
	{
		int result = ++collectionId;
		collections.Add(items);
		return result;
	}

	private int GetDataRowId(string strId)
	{
		if (!objectIdMap.TryGetValue(strId, out var value))
		{
			value = NewDataRowId();
			objectIdMap.Add(strId, value);
			dataRowsIds[value] = strId;
		}
		return value;
	}

	private void ReadMetadataObject(XmlScanner scanner, int id, MetadataTypeInfo typeInfo)
	{
		if (dataRows[id].TypeInfo != null)
		{
			throw new DeserializationException("Duplicate object: " + id);
		}
		if (typeInfo == null)
		{
			if (scanner.NodeType != XmlNodeType.Element)
			{
				throw new InvalidXmlException(SR.DeserializationUnexpectedXmlContent);
			}
			if (XmlConfig.RE.Urn.Equals(scanner.Namespace, StringComparison.Ordinal))
			{
				typeInfo = XmlConfig.GetTypeInfoByName(scanner.NameToken);
			}
			if (typeInfo == null)
			{
				throw new InvalidXmlException(SR.DeserializationInvalidXmlElement1(scanner.Name));
			}
		}
		if (id == 0)
		{
			string attribute = scanner.GetAttribute(XmlConfig.KeyAttribute);
			if (attribute == null)
			{
				throw new InvalidXmlException(SR.DeserializationMissingXmlAttribute(XmlConfig.KeyAttribute.ToString()));
			}
			id = GetDataRowId(attribute);
		}
		object[] values = new object[typeInfo.InstanceProperties.Count];
		scanner.MoveToFirstChild();
		while (!scanner.IsEndOfChildren)
		{
			ReadProperty(scanner, typeInfo, values, id);
			scanner.MoveToNext();
		}
		scanner.MoveUp();
		UpdateDefaultProperties(typeInfo, values, id);
		UpdateTypeConstraints(typeInfo, values);
		dataRows[id] = new DataRow<object>(typeInfo, values);
	}

	private void UpdateDefaultProperties(MetadataTypeInfo typeInfo, object[] values, int id)
	{
		for (int i = 0; i < values.Length; i++)
		{
			MetadataPropertyInfo metadataPropertyInfo = typeInfo.InstanceProperties[i];
			TypeInfo returnType = metadataPropertyInfo.ReturnType;
			if (values[i] != null)
			{
				continue;
			}
			object value;
			bool propertyOverrideValue = serializationSettings.GetPropertyOverrideValue(typeInfo, metadataPropertyInfo, out value);
			if (returnType.IsMetadata)
			{
				if (propertyOverrideValue)
				{
					if (value != null)
					{
						IMetadataObject key = value as IMetadataObject;
						if (!externalMetadataObjects.TryGetValue(key, out var value2))
						{
							value2 = NewDataRowId();
							externalMetadataObjects.Add(key, value2);
						}
						if (metadataPropertyInfo == typeInfo.ParentProperty)
						{
							SetParent(id, value2);
						}
						values[i] = value2;
					}
					else
					{
						values[i] = 0;
					}
				}
				else
				{
					values[i] = 0;
				}
			}
			else if (returnType.IsCollection)
			{
				if (propertyOverrideValue)
				{
					values[i] = 0;
				}
				else if (!((CollectionTypeInfo)returnType).IsOrdered)
				{
					values[i] = RegisterCollection(new List<int>());
				}
				else
				{
					values[i] = 0;
				}
			}
			else if (propertyOverrideValue)
			{
				values[i] = value;
			}
			else if (returnType.Type.GetIsValueType())
			{
				throw new InvalidXmlException(SR.DeserializationMissingXmlElement(XmlConfig.GetPropertyElementName(typeInfo, typeInfo.InstanceProperties[i]).ToString()));
			}
		}
	}

	private void UpdateTypeConstraints(MetadataTypeInfo typeInfo, object[] values)
	{
		foreach (MetadataPropertyInfo instanceProperty in typeInfo.InstanceProperties)
		{
			TypeInfo returnType = instanceProperty.ReturnType;
			if (returnType.IsMetadata)
			{
				int num = (int)values[instanceProperty.Index];
				if (num > 0)
				{
					AddTypeConstraint(num, returnType);
				}
			}
		}
	}

	private void ReadProperty(XmlScanner scanner, MetadataTypeInfo typeInfo, object[] values, int objId)
	{
		if (scanner.NodeType != XmlNodeType.Element)
		{
			throw new InvalidXmlException(SR.DeserializationUnexpectedXmlContent);
		}
		MetadataPropertyInfo metadataPropertyInfo = null;
		if (scanner.MatchElement(XmlConfig.ParentProperty))
		{
			metadataPropertyInfo = typeInfo.ParentProperty;
		}
		else if (scanner.Namespace.Equals(XmlConfig.RE.Urn, StringComparison.Ordinal))
		{
			metadataPropertyInfo = XmlConfig.GetPropertyInfoByName(typeInfo, scanner.NameToken);
		}
		if (metadataPropertyInfo == null || !XmlConfig.IncludeProperty(typeInfo, metadataPropertyInfo, serializationSettings))
		{
			throw new InvalidXmlException(SR.DeserializationInvalidXmlElement1(scanner.Name));
		}
		TypeInfo returnType = metadataPropertyInfo.ReturnType;
		int parentId = ((metadataPropertyInfo.PropertyType == PropertyType.Child) ? objId : 0);
		object obj;
		if (returnType.IsMetadata)
		{
			int num = ReadPropertyMetadataValue(scanner, (MetadataTypeInfo)returnType, parentId);
			if (metadataPropertyInfo == typeInfo.ParentProperty)
			{
				SetParent(objId, num);
			}
			obj = num;
		}
		else if (!returnType.IsCollection)
		{
			obj = ((!returnType.IsPrimitive) ? null : ReadPrimitiveValue(scanner, (PrimitiveTypeInfo)returnType));
		}
		else
		{
			TypeInfo elementType = ((CollectionTypeInfo)returnType).ElementType;
			List<int> items = ReadMetadataCollection(scanner, elementType, parentId);
			obj = RegisterCollection(items);
		}
		values[metadataPropertyInfo.Index] = obj;
	}

	private List<int> ReadMetadataCollection(XmlScanner scanner, TypeInfo elementType, int parentId)
	{
		if (scanner.NodeType != XmlNodeType.Element)
		{
			throw new InvalidXmlException(SR.DeserializationUnexpectedXmlContent);
		}
		List<int> list = new List<int>();
		scanner.MoveToFirstChild();
		while (!scanner.IsEndOfChildren)
		{
			if (scanner.NodeType != XmlNodeType.Element)
			{
				throw new InvalidXmlException(SR.DeserializationUnexpectedXmlContent);
			}
			string attribute = scanner.GetAttribute(XmlConfig.RefAttribute);
			int num;
			if (attribute != null)
			{
				scanner.MatchElement(XmlConfig.ReferenceElement, throwOnError: true);
				if (!scanner.IsEmptyElement)
				{
					throw new InvalidXmlException(SR.DeserializationUnexpectedXmlContent);
				}
				num = GetDataRowId(attribute);
			}
			else
			{
				num = NewDataRowId();
				ReadMetadataObject(scanner, num, null);
			}
			AddTypeConstraint(num, elementType);
			if (parentId > 0)
			{
				SetParent(num, parentId);
			}
			list.Add(num);
			scanner.MoveToNext();
		}
		scanner.MoveUp();
		return list;
	}

	private int ReadPropertyMetadataValue(XmlScanner scanner, MetadataTypeInfo returnType, int parentId)
	{
		string attribute = scanner.GetAttribute(XmlConfig.RefAttribute);
		int num;
		if (attribute != null)
		{
			if (!scanner.IsEmptyElement)
			{
				throw new InvalidXmlException(SR.DeserializationUnexpectedXmlContent);
			}
			num = GetDataRowId(attribute);
		}
		else
		{
			XmlNestingType nestingType = serializationSettings.GetNestingType(returnType);
			num = NewDataRowId();
			if (XmlConfig.IsStaticTypeNesting(nestingType))
			{
				ReadMetadataObject(scanner, num, returnType);
			}
			else
			{
				if (!scanner.MoveToFirstChild())
				{
					throw new InvalidXmlException(SR.DeserializationExpectedSingleXmlElement);
				}
				ReadMetadataObject(scanner, num, null);
				if (scanner.MoveToNext())
				{
					throw new InvalidXmlException(SR.DeserializationExpectedSingleXmlElement);
				}
				scanner.MoveUp();
			}
		}
		AddTypeConstraint(num, returnType);
		if (parentId > 0)
		{
			SetParent(num, parentId);
		}
		return num;
	}

	private void AddTypeConstraint(int id, TypeInfo typeInfo)
	{
		List<TypeInfo> list = dataRowTypeConstraints[id];
		if (list == null)
		{
			list = new List<TypeInfo>(1);
			dataRowTypeConstraints[id] = list;
		}
		if (!list.Contains(typeInfo))
		{
			list.Add(typeInfo);
		}
	}

	private void SetParent(int id, int parentId)
	{
		if (dataRowParents[id] != 0 && dataRowParents[id] != parentId)
		{
			throw new DeserializationException(SR.DeserializationObjectMustHaveOneParent(dataRowsIds[id], dataRowsIds[dataRowParents[id]], dataRowsIds[parentId]));
		}
		dataRowParents[id] = parentId;
	}

	private static object ReadSimpleValue(XmlScanner scanner, PrimitiveTypeInfo typeInfo)
	{
		if (scanner.IsEmptyElement)
		{
			return null;
		}
		string text;
		if (scanner.MoveToFirstChild())
		{
			if (!scanner.IsTextNodeType)
			{
				throw new InvalidXmlException(SR.DeserializationUnexpectedXmlContent);
			}
			text = scanner.ReadContentAsString();
			if (scanner.MoveToNext())
			{
				throw new InvalidXmlException(SR.DeserializationExpectedSingleXmlElement);
			}
		}
		else
		{
			text = string.Empty;
		}
		object result;
		switch (typeInfo.Id)
		{
		case 1:
			result = bool.Parse(text);
			break;
		case 2:
			result = byte.Parse(text, CultureInfo.InvariantCulture);
			break;
		case 11:
			result = double.Parse(text, CultureInfo.InvariantCulture);
			break;
		case 139:
			result = int.Parse(text, CultureInfo.InvariantCulture);
			break;
		case 140:
			result = long.Parse(text, CultureInfo.InvariantCulture);
			break;
		case 146:
			result = text;
			break;
		case 0:
		case 4:
		case 7:
		case 9:
		case 10:
		case 12:
		case 13:
		case 14:
		case 138:
		case 141:
		case 142:
		case 143:
		case 144:
		case 145:
		case 147:
		case 148:
		case 149:
			result = Enum.Parse(typeInfo.Type, text);
			break;
		case 8:
			result = ParseDataTypeSpec(text);
			break;
		case 117:
		{
			IMutableTriggerEventTypeSet mutableTriggerEventTypeSet = new TriggerEventTypeSet();
			ParseTriggerEventSet(text, mutableTriggerEventTypeSet);
			result = mutableTriggerEventTypeSet;
			break;
		}
		case 3:
		case 89:
			result = null;
			break;
		default:
			result = null;
			break;
		}
		scanner.MoveUp();
		return result;
	}

	private static object ReadPrimitiveValue(XmlScanner scanner, PrimitiveTypeInfo typeInfo)
	{
		return typeInfo.Id switch
		{
			6 => ReadComputedColumnInfo(scanner), 
			137 => ReadIdentityColumnInfo(scanner), 
			5 => ReadCollationInfo(scanner), 
			_ => ReadSimpleValue(scanner, typeInfo), 
		};
	}

	private static ComputedColumnInfo ReadComputedColumnInfo(XmlScanner scanner)
	{
		string text = null;
		bool? flag = null;
		scanner.MoveToFirstChild();
		while (!scanner.IsEndOfChildren)
		{
			if (scanner.NodeType != XmlNodeType.Element)
			{
				throw new InvalidXmlException("Expected element node.");
			}
			if (scanner.MatchElement(XmlConfig.TextProperty))
			{
				if (text != null)
				{
					throw new InvalidXmlException("Duplicate Text property.");
				}
				text = (string)ReadPrimitiveValue(scanner, (PrimitiveTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(146));
			}
			else
			{
				if (!scanner.MatchElement(XmlConfig.IsPersistedProperty))
				{
					throw new InvalidXmlException("Invalid property.");
				}
				if (flag.HasValue)
				{
					throw new InvalidXmlException("Duplicate IsPersisted property.");
				}
				flag = (bool)ReadPrimitiveValue(scanner, (PrimitiveTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(1));
			}
			scanner.MoveToNext();
		}
		scanner.MoveUp();
		if (text == null)
		{
			throw new Exception("Missing Text property.");
		}
		if (!flag.HasValue)
		{
			throw new Exception("Missing IsPersisted property.");
		}
		return new ComputedColumnInfo(text, flag.Value);
	}

	private static object ReadIdentityColumnInfo(XmlScanner scanner)
	{
		long? num = null;
		long? num2 = null;
		bool? flag = null;
		scanner.MoveToFirstChild();
		while (!scanner.IsEndOfChildren)
		{
			if (scanner.NodeType != XmlNodeType.Element)
			{
				throw new InvalidXmlException("Expected element node.");
			}
			if (scanner.MatchElement(XmlConfig.SeedProperty))
			{
				if (num.HasValue)
				{
					throw new InvalidXmlException("Duplicate Seed property.");
				}
				num = (long)ReadPrimitiveValue(scanner, (PrimitiveTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(140));
			}
			else if (scanner.MatchElement(XmlConfig.IncrementProperty))
			{
				if (num2.HasValue)
				{
					throw new InvalidXmlException("Duplicate Increment property.");
				}
				num2 = (long)ReadPrimitiveValue(scanner, (PrimitiveTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(140));
			}
			else
			{
				if (!scanner.MatchElement(XmlConfig.NotForReplicationProperty))
				{
					throw new InvalidXmlException("Invalid property.");
				}
				if (flag.HasValue)
				{
					throw new InvalidXmlException("Duplicate NotForReplicationProperty property.");
				}
				flag = (bool)ReadPrimitiveValue(scanner, (PrimitiveTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(1));
			}
			scanner.MoveToNext();
		}
		scanner.MoveUp();
		if (!num.HasValue)
		{
			throw new Exception("Missing Seed property.");
		}
		if (!num2.HasValue)
		{
			throw new Exception("Missing Increment property.");
		}
		if (!flag.HasValue)
		{
			throw new Exception("Missing NotForReplicationProperty property.");
		}
		return new IdentityColumnInfo(num.Value, num2.Value, flag.Value);
	}

	private static CollationInfo ReadCollationInfo(XmlScanner scanner)
	{
		string text = null;
		scanner.MoveToFirstChild();
		while (!scanner.IsEndOfChildren)
		{
			if (scanner.NodeType != XmlNodeType.Element)
			{
				throw new InvalidXmlException("Expected element node.");
			}
			if (scanner.MatchElement(XmlConfig.ICollation_Name))
			{
				if (text != null)
				{
					throw new InvalidXmlException("Duplicate Name property.");
				}
				text = (string)ReadPrimitiveValue(scanner, (PrimitiveTypeInfo)MetadataInfoProvider.MetadataInfo.GetTypeInfoById(146));
				scanner.MoveToNext();
				continue;
			}
			throw new InvalidXmlException("Invalid property.");
		}
		scanner.MoveUp();
		if (text == null)
		{
			throw new Exception("Missing Name property.");
		}
		return ParseCollationInfo(text);
	}

	private static CollationInfo ParseCollationInfo(string strValue)
	{
		CollationInfo collationInfo;
		if (CollationInfo.Ordinal.Name.Equals(strValue, StringComparison.OrdinalIgnoreCase))
		{
			collationInfo = CollationInfo.Ordinal;
		}
		else if (CollationInfo.OrdinalIgnoreCase.Name.Equals(strValue, StringComparison.OrdinalIgnoreCase))
		{
			collationInfo = CollationInfo.OrdinalIgnoreCase;
		}
		else
		{
			collationInfo = CollationInfo.GetCollationInfo(strValue);
			if (collationInfo == null)
			{
				throw new InvalidXmlException("Invalid collation info name.");
			}
		}
		return collationInfo;
	}

	private static object ParseDataTypeSpec(string strValue)
	{
		return DataTypeSpec.GetDataTypeSpec(strValue) ?? throw new InvalidXmlException("Invalid DataTypeSpec name.");
	}

	private static void ParseTriggerEventSet(string strValue, IMutableTriggerEventTypeSet eventSet)
	{
		string[] array = strValue.Split(new char[1] { ',' });
		foreach (string item in array)
		{
			try
			{
				eventSet.Add(item);
			}
			catch (ArgumentException innerException)
			{
				throw new InvalidXmlException("Invalid trigger event name.", innerException);
			}
		}
	}
}
