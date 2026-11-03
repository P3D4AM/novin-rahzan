using System;
using System.Runtime.CompilerServices;
using PacketParser.Entities;

namespace PacketParser;

public interface ILogger
{
	void SaveLog(string message, EnGroup group = EnGroup.None, DateTime? date = null, short trycount = 10, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);
}
