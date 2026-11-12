namespace Babel;

public enum TokenType
{
	Text = 0,
	Keyword = 1,
	Comment = 2,
	Identifier = 3,
	Number = 5,
	SqlStoredProcedure = 7,
	SqlSystemTable = 8,
	SqlSystemFunction = 9,
	SqlOperator = 10,
	SqlString = 11,
	SqlCmdCommand = 12,
	Error = 13,
	Delimiter = 14
}
