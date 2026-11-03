using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business.Accounting;

public class SanadDetRoznameKolBusiness : IHasContext
{
	public string KolName { get; set; }

	public long CodeKol { get; set; }

	public decimal Debit { get; set; }

	public decimal Credit { get; set; }

	public int SanadNo { get; set; }

	public DateTime DateM { get; set; }

	public string Datesh => DateM.MiladiToCalture();

	public string ContextGuid { get; set; }

	public SanadType SanadType { get; set; }

	public SanadDetRoznameKolBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static Task<List<SanadDetRoznameKolBusiness>> GetAllRoznameKolSanadDetAsync(string contextGuid, DateTime date1, DateTime date2, string tranName, CancellationToken token)
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ISanad.GetAllSanadDetRoznameKolAsync(date1, date2, tranName, token);
	}
}
