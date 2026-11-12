namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

public class MetadataPropertyInfo
{
	private readonly int id;

	private readonly int index;

	private readonly PropertyType propertyType;

	private readonly string name;

	private readonly TypeInfo returnType;

	private readonly bool isParent;

	private readonly bool isReadWrite;

	private readonly PropertyValueExpr valueExpr;

	public int Id => id;

	public int Index => index;

	public PropertyType PropertyType => propertyType;

	public string Name => name;

	public bool IsParent => isParent;

	public bool IsReadWrite => isReadWrite;

	public TypeInfo ReturnType => returnType;

	public PropertyValueExpr ValueExpr => valueExpr;

	public MetadataPropertyInfo(int id, int index, string name, TypeInfo returnType, PropertyType propertyType, bool isParent, bool isReadWrite, PropertyValueExpr valueExpr)
	{
		this.id = id;
		this.index = index;
		this.name = name;
		this.propertyType = propertyType;
		this.isParent = isParent;
		this.returnType = returnType;
		this.isReadWrite = isReadWrite;
		this.valueExpr = valueExpr;
	}

	internal MetadataPropertyInfo(int id, MetadataPropertyInfo other)
	{
		this.id = id;
		index = other.index;
		propertyType = other.propertyType;
		name = other.name;
		returnType = other.returnType;
		isReadWrite = other.isReadWrite;
		valueExpr = other.valueExpr;
	}
}
