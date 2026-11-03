using System;
using PacketParser;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business.Accounting.Models;

public class RasGiriDateRemModel : IHasContext
{
	public DateTime DateM { get; set; }

	public decimal Remain { get; set; }

	public string DateSh => DateM.MiladiToCalture();

	public string ContextGuid { get; set; }

	public RasGiriDateRemModel(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
