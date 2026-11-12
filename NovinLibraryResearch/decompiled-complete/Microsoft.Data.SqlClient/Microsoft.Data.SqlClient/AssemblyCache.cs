using System;
using Microsoft.Data.SqlClient.Server;

namespace Microsoft.Data.SqlClient;

internal sealed class AssemblyCache
{
	private AssemblyCache()
	{
	}

	internal static int GetLength(object inst)
	{
		return SerializationHelperSql9.SizeInBytes(inst);
	}

	internal static SqlUdtInfo GetInfoFromType(Type t)
	{
		Type type = t;
		do
		{
			SqlUdtInfo sqlUdtInfo = SqlUdtInfo.TryGetFromType(t);
			if (sqlUdtInfo != null)
			{
				return sqlUdtInfo;
			}
			t = t.BaseType;
		}
		while (t != null);
		throw SQL.UDTInvalidSqlType(type.AssemblyQualifiedName);
	}
}
