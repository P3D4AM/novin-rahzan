namespace Microsoft.Data.Tools.Sql.BatchParser;

internal enum LexerTokenType
{
	None,
	Text,
	TextVerbatim,
	Whitespace,
	NewLine,
	String,
	Eof,
	Error,
	Comment,
	Go,
	Reset,
	Ed,
	Execute,
	Quit,
	Exit,
	Include,
	Serverlist,
	Setvar,
	List,
	ErrorCommand,
	Out,
	Perftrace,
	Connect,
	OnError,
	Help,
	Xml,
	ListVar
}
