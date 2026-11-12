using System;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal class PropertyInfo
{
	private enum Category
	{
		None,
		MetadataCollection,
		MetadataOrderedCollection,
		MetadataReference,
		Reference
	}

	private readonly Category category;

	public readonly string Name;

	public readonly Type PropertyType;

	public bool IsMetadataReference => category == Category.MetadataReference;

	public bool IsMetadataCollection => category == Category.MetadataCollection;

	public bool IsMetadataOrderedCollection => category == Category.MetadataOrderedCollection;

	private PropertyInfo(string name, Type propertyType, Category category)
	{
		Name = name;
		PropertyType = propertyType;
		this.category = category;
	}

	public static PropertyInfo Primitive(string name, Type propertyType)
	{
		return new PropertyInfo(name, propertyType, Category.None);
	}

	public static PropertyInfo Enum(string name, Type propertyType)
	{
		return new PropertyInfo(name, propertyType, Category.None);
	}

	public static PropertyInfo Reference(string name, Type propertyType)
	{
		return new PropertyInfo(name, propertyType, Category.Reference);
	}

	public static PropertyInfo MetadataReference(string name, Type propertyType)
	{
		return new PropertyInfo(name, propertyType, Category.MetadataReference);
	}

	public static PropertyInfo MetadataCollection(string name, Type propertyType)
	{
		return new PropertyInfo(name, propertyType, Category.MetadataCollection);
	}

	public static PropertyInfo MetadataOrderedCollection(string name, Type propertyType)
	{
		return new PropertyInfo(name, propertyType, Category.MetadataOrderedCollection);
	}
}
