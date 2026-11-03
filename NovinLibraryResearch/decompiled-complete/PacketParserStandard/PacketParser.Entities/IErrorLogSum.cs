using System;

namespace PacketParser.Entities;

public interface IErrorLogSum
{
	int HashAll { get; set; }

	ENSource Source { get; set; }

	string Version { get; set; }

	string ClassName { get; set; }

	string FuncName { get; set; }

	short Status { get; set; }

	string ExceptionType { get; set; }

	string ExceptionMessage { get; set; }

	DateTime FirstLogTime { get; set; }

	int Count { get; set; }
}
