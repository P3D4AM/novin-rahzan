using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

public abstract class MetadataObjectBase : IMetadataObject
{
	protected readonly string m_name;

	public string Name => m_name;

	protected MetadataObjectBase(string name)
	{
		m_name = name;
	}

	public override string ToString()
	{
		return ExtensionMethods.ToString(this);
	}

	public abstract T Accept<T>(IMetadataObjectVisitor<T> visitor);
}
