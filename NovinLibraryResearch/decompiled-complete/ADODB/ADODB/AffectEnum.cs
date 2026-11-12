using System.Runtime.InteropServices;

namespace ADODB;

[Guid("00000543-0000-0010-8000-00AA006D2EA4")]
public enum AffectEnum
{
	adAffectCurrent = 1,
	adAffectGroup,
	[TypeLibVar(64)]
	adAffectAll,
	adAffectAllChapters
}
