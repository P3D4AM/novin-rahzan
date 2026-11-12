using System.Runtime.InteropServices;

namespace MSFlexGridLib;

[Guid("D8898463-742F-11CF-8AEA-00AA00C00905")]
public enum OLEDropEffectConstants
{
	flexOLEDropEffectNone = 0,
	flexOLEDropEffectCopy = 1,
	flexOLEDropEffectMove = 2,
	flexOLEDropEffectScroll = int.MinValue
}
