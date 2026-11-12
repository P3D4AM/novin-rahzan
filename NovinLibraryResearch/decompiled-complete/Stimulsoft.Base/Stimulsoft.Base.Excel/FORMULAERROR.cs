namespace Stimulsoft.Base.Excel;

internal enum FORMULAERROR : byte
{
	NULL = 0,
	DIV0 = 7,
	VALUE = 15,
	REF = 23,
	NAME = 29,
	NUM = 36,
	NA = 42
}
