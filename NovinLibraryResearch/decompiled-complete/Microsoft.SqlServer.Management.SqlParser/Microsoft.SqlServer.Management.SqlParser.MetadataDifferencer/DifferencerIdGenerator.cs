using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataServices;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataDifferencer;

internal class DifferencerIdGenerator : DatabaseRelativeIdGenerator
{
	public DifferencerIdGenerator(IDatabaseObject sourceRoot, IDatabaseObject targetRoot)
		: base(sourceRoot, targetRoot)
	{
	}

	public override MetadataObjectSelector GetObjectSelector(IMetadataObject obj)
	{
		string name = (MetadataUtils.IsValueTypeObject(obj) ? string.Empty : obj.Name);
		return new MetadataObjectSelector(obj.TypeInfo(), name);
	}
}
