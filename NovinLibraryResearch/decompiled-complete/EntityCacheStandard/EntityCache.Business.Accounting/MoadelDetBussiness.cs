using System;
using System.Threading;
using PacketParser;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business.Accounting;

public class MoadelDetBussiness : IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	public DateTime DateM { get; set; }

	public decimal Debit { get; set; }

	public decimal Credit { get; set; }

	public decimal Remain { get; set; }

	public string DateSH => DateM.MiladiToCalture();

	public string Desc { get; set; }

	public string ContextGuid { get; set; }

	public MoadelDetBussiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
