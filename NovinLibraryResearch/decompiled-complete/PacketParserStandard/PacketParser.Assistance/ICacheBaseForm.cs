using System;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Assistance;

public interface ICacheBaseForm : INpDisposable, IDisposable
{
	string ConnectionString { get; }

	string ContextGuid { get; }

	bool DataIsValid { get; set; }

	EnSystem system { get; }
}
