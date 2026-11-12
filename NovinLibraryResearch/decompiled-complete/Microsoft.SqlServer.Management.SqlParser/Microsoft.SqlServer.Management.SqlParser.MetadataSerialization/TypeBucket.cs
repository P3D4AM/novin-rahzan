using System.Collections.Generic;
using System.Globalization;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal abstract class TypeBucket : ITypeInfo
{
	private readonly int id;

	private readonly string baseName;

	private readonly string typeName;

	public int Id => id;

	public string Name => typeName;

	public string BaseName => baseName;

	protected TypeBucket(string name, int id)
	{
		this.id = id;
		baseName = name;
		typeName = string.Format(CultureInfo.InvariantCulture, "{0}{1}", name, id);
	}

	public abstract void GenerateType(TypeGenerator typeGenerator, int rowId);

	public abstract IEnumerable<uint> GetPackedValues(int rowId);
}
