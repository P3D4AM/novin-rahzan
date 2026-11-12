using System.Runtime.InteropServices;

namespace ADODB;

[Guid("00000520-0000-0010-8000-00AA006D2EA4")]
public enum ConnectPromptEnum
{
	adPromptAlways = 1,
	adPromptComplete,
	adPromptCompleteRequired,
	adPromptNever
}
