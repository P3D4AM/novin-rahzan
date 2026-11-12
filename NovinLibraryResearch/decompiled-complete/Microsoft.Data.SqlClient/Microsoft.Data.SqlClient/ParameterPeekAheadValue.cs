using System.Collections.Generic;
using Microsoft.Data.SqlClient.Server;

namespace Microsoft.Data.SqlClient;

internal class ParameterPeekAheadValue
{
	internal IEnumerator<SqlDataRecord> Enumerator;

	internal SqlDataRecord FirstRecord;
}
