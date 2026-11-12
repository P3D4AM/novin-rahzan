using System;

namespace Babel;

[Flags]
public enum TokenTriggers
{
	None = 0,
	MemberSelect = 1,
	MatchBraces = 2,
	ParameterStart = 0x10,
	ParameterNext = 0x20,
	ParameterEnd = 0x40,
	Parameter = 0x80,
	MethodTip = 0xF0
}
