using System.Runtime.InteropServices;

namespace MSWinsockLib;

[Guid("248DD894-BB45-11CF-9ABC-0080C7E7B78D")]
public enum StateConstants
{
	sckClosed,
	sckOpen,
	sckListening,
	sckConnectionPending,
	sckResolvingHost,
	sckHostResolved,
	sckConnecting,
	sckConnected,
	sckClosing,
	sckError
}
