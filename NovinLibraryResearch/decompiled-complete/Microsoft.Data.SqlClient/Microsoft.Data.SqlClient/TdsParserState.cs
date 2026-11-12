namespace Microsoft.Data.SqlClient;

internal enum TdsParserState
{
	Closed,
	OpenNotLoggedIn,
	OpenLoggedIn,
	Broken
}
