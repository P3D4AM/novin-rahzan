using System;
using System.Runtime.Serialization;

namespace Microsoft.Data.SqlClient;

[Serializable]
public sealed class SqlError
{
	private readonly string _source = "Framework Microsoft SqlClient Data Provider";

	private readonly int _number;

	private readonly byte _state;

	private readonly byte _errorClass;

	[OptionalField(VersionAdded = 2)]
	private readonly string _server;

	private readonly string _message;

	private readonly string _procedure;

	private readonly int _lineNumber;

	[OptionalField(VersionAdded = 4)]
	private readonly int _win32ErrorCode;

	public string Source => _source;

	public int Number => _number;

	public byte State => _state;

	public byte Class => _errorClass;

	public string Server => _server;

	public string Message => _message;

	public string Procedure => _procedure;

	public int LineNumber => _lineNumber;

	internal int Win32ErrorCode => _win32ErrorCode;

	internal SqlError(int infoNumber, byte errorState, byte errorClass, string server, string errorMessage, string procedure, int lineNumber, uint win32ErrorCode)
		: this(infoNumber, errorState, errorClass, server, errorMessage, procedure, lineNumber)
	{
		_win32ErrorCode = (int)win32ErrorCode;
	}

	internal SqlError(int infoNumber, byte errorState, byte errorClass, string server, string errorMessage, string procedure, int lineNumber)
	{
		_number = infoNumber;
		_state = errorState;
		_errorClass = errorClass;
		_server = server;
		_message = errorMessage;
		_procedure = procedure;
		_lineNumber = lineNumber;
		if (errorClass != 0)
		{
			SqlClientEventSource.Log.TryTraceEvent("<sc.SqlError.SqlError|ERR> infoNumber={0}, errorState={1}, errorClass={2}, errorMessage='{3}', procedure='{4}', lineNumber={5}", infoNumber, (int)errorState, (int)errorClass, errorMessage, procedure, lineNumber);
		}
		_win32ErrorCode = 0;
	}

	public override string ToString()
	{
		return typeof(SqlError).ToString() + ": " + _message;
	}
}
