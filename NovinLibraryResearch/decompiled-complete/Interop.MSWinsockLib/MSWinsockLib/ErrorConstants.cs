using System.Runtime.InteropServices;

namespace MSWinsockLib;

[Guid("248DD891-BB45-11CF-9ABC-0080C7E7B78D")]
public enum ErrorConstants
{
	sckInvalidPropertyValue = 380,
	sckGetNotSupported = 394,
	sckSetNotSupported = 383,
	sckOutOfMemory = 7,
	sckBadState = 40006,
	sckInvalidArg = 40014,
	sckSuccess = 40017,
	sckUnsupported = 40018,
	sckInvalidOp = 40020,
	sckOutOfRange = 40021,
	sckWrongProtocol = 40026,
	sckOpCanceled = 10004,
	sckInvalidArgument = 10014,
	sckWouldBlock = 10035,
	sckInProgress = 10036,
	sckAlreadyComplete = 10037,
	sckNotSocket = 10038,
	sckMsgTooBig = 10040,
	sckPortNotSupported = 10043,
	sckAddressInUse = 10048,
	sckAddressNotAvailable = 10049,
	sckNetworkSubsystemFailed = 10050,
	sckNetworkUnreachable = 10051,
	sckNetReset = 10052,
	sckConnectAborted = 10053,
	sckConnectionReset = 10054,
	sckNoBufferSpace = 10055,
	sckAlreadyConnected = 10056,
	sckNotConnected = 10057,
	sckSocketShutdown = 10058,
	sckTimedout = 10060,
	sckConnectionRefused = 10061,
	sckNotInitialized = 10093,
	sckHostNotFound = 11001,
	sckHostNotFoundTryAgain = 11002,
	sckNonRecoverableError = 11003,
	sckNoData = 11004
}
