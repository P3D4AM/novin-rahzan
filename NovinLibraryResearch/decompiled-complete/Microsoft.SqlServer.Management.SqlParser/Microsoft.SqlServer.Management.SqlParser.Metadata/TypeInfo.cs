using System;

namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public abstract class TypeInfo : IComparable<TypeInfo>, IEquatable<TypeInfo>
{
	private readonly Type type;

	private readonly int id;

	public string Name => type.Name;

	public Type Type => type;

	public int Id => id;

	public bool IsMetadata => this is MetadataTypeInfo;

	public bool IsCollection => this is CollectionTypeInfo;

	public bool IsPrimitive => this is PrimitiveTypeInfo;

	protected TypeInfo(Type type, int id)
	{
		this.type = type;
		this.id = id;
	}

	public int CompareTo(TypeInfo other)
	{
		if (other == null)
		{
			return 1;
		}
		return StringComparer.Ordinal.Compare(Name, other.Name);
	}

	public override int GetHashCode()
	{
		return id;
	}

	public override bool Equals(object obj)
	{
		return Equals(obj as TypeInfo);
	}

	public bool Equals(TypeInfo other)
	{
		if (other == null)
		{
			return false;
		}
		return id == other.id;
	}
}
