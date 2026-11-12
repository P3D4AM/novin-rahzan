using System.Runtime.InteropServices;

namespace ADODB;

[Guid("0000051B-0000-0010-8000-00AA006D2EA4")]
public enum CursorTypeEnum
{
	[TypeLibVar(64)]
	adOpenUnspecified = -1,
	adOpenForwardOnly,
	adOpenKeyset,
	adOpenDynamic,
	adOpenStatic
}
