using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Models.Accounting;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Sanad;

namespace EntityCache.Business.Accounting.Sanad;

public class ViewSanadDetailCoverBusiness : IViewSanadDetailCover, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	public Guid Guid { get; set; }

	public Guid? DebitMoeinGuid { get; set; }

	public Guid? DebitTafsilGuid { get; set; }

	public DateTime Date { get; set; }

	public string DateSh => Date.MiladiToCalture();

	public Guid AsGuid { get; set; }

	public DocumentDet AsType { get; set; }

	public decimal Debit { get; set; }

	public decimal Credit { get; set; }

	public Guid? CashierGuid { get; set; }

	public int SanadNo { get; set; }

	public SanadSt SanadSt { get; set; }

	public string SanadStatusForDisplay => SanadSt.GetDisplay().GetTranslate();

	public int Id2 { get; set; }

	public Guid? CreditMoeinGuid { get; set; }

	public Guid? CreditTafsilGuid { get; set; }

	public decimal Qt { get; set; }

	public decimal Price { get; set; }

	public decimal Qt2 { get; set; }

	public decimal RDiscount { get; set; }

	public decimal RBazaryab { get; set; }

	public string Serial { get; set; }

	public string SanadDesc { get; set; }

	public string RDesc { get; set; }

	public Guid? ProductGuid { get; set; }

	public Guid? DepotGuid { get; set; }

	public Guid? VahedGuid { get; set; }

	public DateTime? DateMoaser { get; set; }

	public Guid? DecimalGuid { get; set; }

	public decimal DecimalQt { get; set; }

	public decimal DecimalPrice { get; set; }

	public Guid? DetailGuid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status { get; set; } = true;


	public Document AsTypeDocument { get; set; }

	public string ContextGuid { get; set; }

	public Guid? TafsilGuid2 { get; set; }

	public Guid? TafsilGuid3 { get; set; }

	public Guid? TafsilGuid4 { get; set; }

	public ViewSanadDetailCoverBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static Task<IEnumerable<ViewSanadDetailCoverBusiness>> GetAllInvalidRelationsAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ViewSanadDetailCover.GetAllInvalidRelationsAsync(token, tranName);
	}

	public static async Task<IEnumerable<InvalidRelationModel>> GetAllInvalidRelationsModelAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			IEnumerable<ViewSanadDetailCoverBusiness> enumerable = await GetAllInvalidRelationsAsync(contextGuid, token, tranName);
			if (enumerable == null || !enumerable.Any())
			{
				return null;
			}
			return from q in (from l in enumerable
					group l by l.AsType).ToDictionary((IGrouping<DocumentDet, ViewSanadDetailCoverBusiness> g) => g.Key, (IGrouping<DocumentDet, ViewSanadDetailCoverBusiness> g) => g.ToList())
				select new InvalidRelationModel
				{
					AsType = q.Key,
					Count = q.Value.Count,
					SanadList = q.Value.ToList()
				};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllInvalidRelationsModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\ViewSanadDetailCoverBusiness.cs", 78);
			throw;
		}
	}
}
