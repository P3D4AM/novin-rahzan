using System.Data;
using System.Data.Common;

namespace Microsoft.Data.SqlClient;

public sealed class SqlRowUpdatedEventArgs : RowUpdatedEventArgs
{
	public new SqlCommand Command => (SqlCommand)base.Command;

	public SqlRowUpdatedEventArgs(DataRow row, IDbCommand command, StatementType statementType, DataTableMapping tableMapping)
		: base(row, command, statementType, tableMapping)
	{
	}
}
