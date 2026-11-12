using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class SynonymFactory : ObjectFactoryBase<SynonymFactory>, ISynonymFactory
{
	public IMutableSynonym CreateSynonym(ISchema schema, string name)
	{
		if (schema == null)
		{
			throw new ArgumentNullException("schema");
		}
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		return new Synonym(schema, name);
	}
}
