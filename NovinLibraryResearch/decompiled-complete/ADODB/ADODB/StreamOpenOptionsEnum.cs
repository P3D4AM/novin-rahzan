using System.Runtime.InteropServices;

namespace ADODB;

[Guid("0000057A-0000-0010-8000-00AA006D2EA4")]
public enum StreamOpenOptionsEnum
{
	adOpenStreamUnspecified = -1,
	adOpenStreamAsync = 1,
	adOpenStreamFromRecord = 4
}
