using System;

namespace Microsoft.Data.SqlClient;

public sealed class SqlInfoMessageEventArgs : EventArgs
{
	private SqlException exception;

	public SqlErrorCollection Errors => exception.Errors;

	public string Message => exception.Message;

	public string Source => exception.Source;

	internal SqlInfoMessageEventArgs(SqlException exception)
	{
		this.exception = exception;
	}

	private bool ShouldSerializeErrors()
	{
		if (exception != null)
		{
			return 0 < exception.Errors.Count;
		}
		return false;
	}

	public override string ToString()
	{
		return Message;
	}
}
