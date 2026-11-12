namespace Babel.ParserGenerator;

public enum QueryStatus
{
	None = 0,
	Q_BYALL = 1,
	Q_DERIVED = 2,
	Q_VIEW = 4,
	Q_BYCUBE = 8,
	Q_BYROLLUP = 0x10,
	Q_ALTDB = 0x20,
	P_BREAKDECPOINT = 0x40,
	Q_ODBC_ESCAPE = 0x80,
	Q_CRDB = 0x100,
	Q_CR_AL_FTI = 0x200,
	Q_FT_PROP_REF = 0x400,
	Q_MERGE = 0x800,
	Q_SERVER = 0x1000,
	Q_DATABASE = 0x2000,
	Q_FN_PARSE = 0x4000
}
