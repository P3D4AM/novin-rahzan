using System.Runtime.InteropServices;

namespace ADODB;

[Guid("0000052C-0000-0010-8000-00AA006D2EA4")]
public enum ParameterDirectionEnum
{
	adParamUnknown,
	adParamInput,
	adParamOutput,
	adParamInputOutput,
	adParamReturnValue
}
