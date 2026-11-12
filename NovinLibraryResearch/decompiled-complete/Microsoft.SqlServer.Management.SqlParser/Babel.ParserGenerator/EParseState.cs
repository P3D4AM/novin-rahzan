namespace Babel.ParserGenerator;

public enum EParseState
{
	e_ParseNeutral,
	e_ParseEventNotifObjectOn,
	e_ParseIdxInclude,
	e_ParseBegin,
	e_ParseGDR,
	e_ParseDump,
	e_ParseLoad,
	e_ParseOpen,
	e_ParseClose,
	e_ParseWith,
	e_ParseAfterWith,
	e_ParseGroupBy
}
