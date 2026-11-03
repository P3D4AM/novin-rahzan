namespace PacketParser.Entities.Interfaces;

internal interface IPerfMonitor
{
	string ClassName { get; set; }

	string FuncName { get; set; }

	int Duration { get; set; }

	int ExcuteCount { get; set; }

	int Min { get; set; }

	int Max { get; set; }

	long MinMemory { get; set; }

	long MaxMemory { get; set; }

	long MemoryUsage { get; set; }

	string UserDescription { get; set; }
}
