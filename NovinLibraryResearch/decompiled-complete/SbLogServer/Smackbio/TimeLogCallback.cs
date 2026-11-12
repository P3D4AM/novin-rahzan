using System;

namespace Smackbio;

public delegate bool TimeLogCallback(string TerminalType, int TerminalID, string SerialNumber, int TransactionID, DateTime LogTime, long UserID, int DoorID, string AttendanceStatus, string VerifyMode, int JobCode, string Antipass, byte[] Photo);
