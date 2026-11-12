using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal class DeserializationModel
{
	private class Collection<T> : IMetadataOrderedCollection<T>, IMetadataCollection<T>, IEnumerable<T>, IEnumerable where T : class, IMetadataObject
	{
		private readonly DeserializationModel m_model;

		private readonly IEqualityComparer<string> m_comparer;

		private readonly List<int> m_ids;

		public T this[int index] => m_model.GetObjectById<T>(m_ids[index]);

		public int Count => m_ids.Count;

		public T this[string name]
		{
			get
			{
				using (IEnumerator<T> enumerator = GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						T current = enumerator.Current;
						if (m_comparer.Equals(current.Name, name))
						{
							return current;
						}
					}
				}
				return null;
			}
		}

		public IMetadataCollection<IMetadataObject> AsMetadataObjectCollection => Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Collection<IMetadataObject>.Convert(this);

		public Collection(DeserializationModel model, CollationInfo collationInfo, List<int> ids)
		{
			m_model = model;
			m_comparer = collationInfo.EqualityComparer;
			m_ids = ids;
		}

		public bool Contains(string name)
		{
			return this[name] != null;
		}

		public bool Contains(T item)
		{
			using (IEnumerator<T> enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == item)
					{
						return true;
					}
				}
			}
			return false;
		}

		public IEnumerable<T> FindAll(Predicate<T> match)
		{
			using IEnumerator<T> enumerator = GetEnumerator();
			while (enumerator.MoveNext())
			{
				T current = enumerator.Current;
				if (match(current))
				{
					yield return current;
				}
			}
		}

		public IEnumerable<T> FindAll(string name)
		{
			using IEnumerator<T> enumerator = GetEnumerator();
			while (enumerator.MoveNext())
			{
				T current = enumerator.Current;
				if (m_comparer.Equals(current.Name, name))
				{
					yield return current;
				}
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			for (int i = 0; i < m_ids.Count; i++)
			{
				yield return this[i];
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	private sealed class ModelMetadataProvider : MetadataProviderBase
	{
		public override IServer Server => null;

		public override MetadataProviderEventHandler BeforeBindHandler => null;

		public override MetadataProviderEventHandler AfterBindHandler => null;
	}

	private static class DataTypeArgName
	{
		public const string Length = "Length";

		public const string Precision = "Precision";

		public const string Scale = "Scale";
	}

	private readonly IDeserializationDataProvider objectDataProvider;

	private readonly IDeserializationHandler handler;

	private readonly MetadataDeserializerFactory factory;

	private readonly object[] objectCache;

	private readonly object[] collectionCache;

	private CollationInfo databaseCollationInfo;

	private readonly ModelMetadataProvider metadataProvider;

	public DeserializationModel(IDeserializationDataProvider objectDataProvider, IDeserializationHandler handler)
	{
		this.objectDataProvider = objectDataProvider;
		this.handler = handler;
		factory = new MetadataDeserializerFactory(this);
		objectCache = new object[objectDataProvider.ObjectCount];
		collectionCache = new object[objectDataProvider.CollectionCount];
		metadataProvider = new ModelMetadataProvider();
	}

	internal ICursorDataType GetCursorDataType()
	{
		return metadataProvider.MetadataFactory.DataType.Cursor;
	}

	internal IScalarDataType GetIntDataType()
	{
		return metadataProvider.SystemDataTypeLookup.Find(DataTypeSpec.Int, isMaximum: false);
	}

	internal IMetadataCollection<IXmlDataTypeMethod> GetXmlDataTypeMethodCollection()
	{
		return XmlDataTypeMethodCollection.Instance;
	}

	internal IMetadataCollection<IXmlDataTypeTableMethod> GetXmlDataTypeTableMethodCollection()
	{
		return XmlDataTypeTableMethodCollection.Instance;
	}

	public T GetObjectById<T>(int id) where T : class, IMetadataObject
	{
		if (id == 0)
		{
			return null;
		}
		object obj = objectCache[id];
		if (obj == null)
		{
			DataRow<object> objectData = objectDataProvider.GetObjectData(id);
			if (objectData.Values != null && objectData.TypeInfo != null)
			{
				lock (objectData.Values)
				{
					obj = objectCache[id];
					if (obj == null)
					{
						IMetadataObject metadataObject = factory.CreateMetadataObject(objectData.Values, objectData.TypeInfo);
						switch (objectData.TypeInfo.Id)
						{
						case 29:
							databaseCollationInfo = ((IDatabase)metadataObject).CollationInfo;
							break;
						case 107:
						case 108:
							metadataObject = FindSystemDataType((ISystemDataType)metadataObject);
							break;
						}
						obj = handler.OnCreateEvent(metadataObject);
						objectCache[id] = obj;
					}
				}
			}
			else
			{
				obj = objectDataProvider.GetExternalObjectById(id);
			}
		}
		return (T)obj;
	}

	public IMetadataCollection<T> GetCollectionById<T>(int id) where T : class, IMetadataObject
	{
		return GetCollectionByIdImpl<T>(id);
	}

	public IMetadataOrderedCollection<T> GetOrderedCollectionById<T>(int id) where T : class, IMetadataObject
	{
		return GetCollectionByIdImpl<T>(id);
	}

	private IMetadataOrderedCollection<T> GetCollectionByIdImpl<T>(int id) where T : class, IMetadataObject
	{
		if (id == 0)
		{
			return Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Collection<T>.EmptyOrdered;
		}
		object obj = collectionCache[id];
		if (obj == null)
		{
			List<int> collectionData = objectDataProvider.GetCollectionData(id);
			lock (collectionData)
			{
				obj = collectionCache[id];
				if (obj == null)
				{
					obj = new Collection<T>(this, databaseCollationInfo, collectionData);
					collectionCache[id] = obj;
				}
			}
		}
		return (Collection<T>)obj;
	}

	private ISystemDataType FindSystemDataType(ISystemDataType dataTypeData)
	{
		DataTypeSpec typeSpec = dataTypeData.TypeSpec;
		if (typeSpec.ArgSpec1 != null)
		{
			int dataTypeArgValue = GetDataTypeArgValue(dataTypeData, typeSpec.ArgSpec1);
			if (typeSpec.ArgSpec2 != null)
			{
				int dataTypeArgValue2 = GetDataTypeArgValue(dataTypeData, typeSpec.ArgSpec2);
				return SystemDataTypeLookup.Instance.Find(typeSpec, dataTypeArgValue, dataTypeArgValue2);
			}
			return SystemDataTypeLookup.Instance.Find(typeSpec, dataTypeArgValue);
		}
		return SystemDataTypeLookup.Instance.Find(typeSpec, isMaximum: false);
	}

	private static int GetDataTypeArgValue(ISystemDataType dataType, DataTypeArgSpec argSpec)
	{
		return argSpec.Name switch
		{
			"Length" => dataType.Length, 
			"Precision" => dataType.NumericPrecision, 
			"Scale" => dataType.NumericScale, 
			_ => 0, 
		};
	}
}
