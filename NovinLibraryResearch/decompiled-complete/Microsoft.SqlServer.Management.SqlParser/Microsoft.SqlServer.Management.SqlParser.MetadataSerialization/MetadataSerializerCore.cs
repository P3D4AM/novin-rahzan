using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal class MetadataSerializerCore<TId>
{
	private readonly ISerializationHandler<TId> handler;

	private readonly ISerializationFilter filter;

	private readonly SerializationVisitor serializationVisitor;

	public SerializationVisitor SerializationVisitor => serializationVisitor;

	public MetadataSerializerCore(ISerializationHandler<TId> handler, ISerializationFilter filter, SerializationOptions options)
	{
		this.handler = handler;
		this.filter = filter;
		serializationVisitor = new SerializationVisitor(options);
	}

	public SerializationInfo GetSerializationInfo(IMetadataObject obj)
	{
		return obj.Accept(serializationVisitor);
	}

	public DataRow<TId> GetMetadataObjectData(IMetadataObject obj)
	{
		SerializationInfo serializationInfo = obj.Accept(serializationVisitor);
		TId[] objectData = serializationInfo.GetObjectData(handler, obj);
		return new DataRow<TId>(serializationInfo.TypeInfo, objectData);
	}

	public TId[] GetMetadataCollectionData<T>(IMetadataCollection<T> obj, IMetadataObject parentObj, bool isChild) where T : IMetadataObject
	{
		if (obj == null || obj.Count == 0)
		{
			return null;
		}
		TId[] array = new TId[obj.Count];
		int num = 0;
		foreach (T item in obj)
		{
			if (!isChild || filter.IsIncluded(item))
			{
				array[num++] = handler.RegisterValue(item, parentObj, isChild);
			}
		}
		if (num == 0)
		{
			array = null;
		}
		else if (num < array.Length)
		{
			TId[] array2 = new TId[num];
			Array.Copy(array, array2, num);
			array = array2;
		}
		return array;
	}
}
