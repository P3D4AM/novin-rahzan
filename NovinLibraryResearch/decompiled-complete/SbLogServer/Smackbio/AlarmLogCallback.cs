using System;

namespace Smackbio;

public delegate bool AlarmLogCallback(string TerminalType, int TerminalID, string SerialNumber, int TransactionID, DateTime LogTime, long UserID, int DoorID, string AlarmType);
