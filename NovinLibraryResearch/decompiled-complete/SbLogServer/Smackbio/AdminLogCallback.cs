using System;

namespace Smackbio;

public delegate bool AdminLogCallback(string TerminalType, int TerminalID, string SerialNumber, int TransactionID, DateTime LogTime, long AdminID, long UserID, string Action, int Result);
