using System.Runtime.InteropServices;

namespace ADODB;

[Guid("0000052B-0000-0010-8000-00AA006D2EA4")]
public enum ParameterAttributesEnum
{
	adParamSigned = 0x10,
	adParamNullable = 0x40,
	adParamLong = 0x80
}
