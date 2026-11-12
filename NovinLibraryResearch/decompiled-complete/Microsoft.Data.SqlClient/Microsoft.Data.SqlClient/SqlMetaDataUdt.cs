using System;

namespace Microsoft.Data.SqlClient;

internal sealed class SqlMetaDataUdt
{
	internal Type Type;

	internal string DatabaseName;

	internal string SchemaName;

	internal string TypeName;

	internal string AssemblyQualifiedName;

	public void CopyFrom(SqlMetaDataUdt original)
	{
		if (original != null)
		{
			Type = original.Type;
			DatabaseName = original.DatabaseName;
			SchemaName = original.SchemaName;
			TypeName = original.TypeName;
			AssemblyQualifiedName = original.AssemblyQualifiedName;
		}
	}
}
