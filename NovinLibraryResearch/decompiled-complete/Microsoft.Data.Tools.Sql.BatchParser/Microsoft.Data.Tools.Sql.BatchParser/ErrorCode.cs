namespace Microsoft.Data.Tools.Sql.BatchParser;

internal enum ErrorCode
{
	ErrorCodeBase = 0,
	Success = 0,
	UnsupportedCommand = 1,
	UnrecognizedToken = 2,
	StringNotTerminated = 3,
	CommentNotTerminated = 4,
	InvalidVariableName = 6,
	InvalidNumber = 7,
	TokenExpected = 8,
	Aborted = 9,
	CircularReference = 10,
	VariableNotDefined = 11
}
