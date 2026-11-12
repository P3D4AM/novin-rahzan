using System.Runtime.InteropServices;

namespace ADODB;

[Guid("00000529-0000-0010-8000-00AA006D2EA4")]
public enum PropertyAttributesEnum
{
	adPropNotSupported = 0,
	adPropRequired = 1,
	adPropOptional = 2,
	adPropRead = 0x200,
	adPropWrite = 0x400
}
