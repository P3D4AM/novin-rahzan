namespace Babel.ParserGenerator;

public enum QueryState
{
	None,
	IN_SELECT_LIST,
	IN_ORDER_BY_CLAUSE,
	IN_HAVING_CLAUSE,
	IN_WHERE_CLAUSE,
	IN_ON_CLAUSE,
	IN_FROM_CLAUSE,
	IN_OPTION_CLAUSE
}
