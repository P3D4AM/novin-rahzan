using System.Runtime.InteropServices;

namespace ADODB;

[Guid("00000552-0000-0010-8000-00AA006D2EA4")]
public enum SeekEnum
{
	adSeekFirstEQ = 1,
	adSeekLastEQ = 2,
	adSeekAfterEQ = 4,
	adSeekAfter = 8,
	adSeekBeforeEQ = 0x10,
	adSeekBefore = 0x20
}
