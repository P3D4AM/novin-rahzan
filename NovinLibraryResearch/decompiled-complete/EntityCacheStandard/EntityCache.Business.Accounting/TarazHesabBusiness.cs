using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.Filters;

namespace EntityCache.Business.Accounting;

public class TarazHesabBusiness
{
	public decimal SD_Debit { get; set; }

	public decimal SD_Credit { get; set; }

	public decimal DD_Debit { get; set; }

	public decimal DD_Credtit { get; set; }

	public decimal ED_Debit { get; set; }

	public decimal ED_Credit { get; set; }

	public decimal AccountEndDore => DD_Credtit - DD_Debit;

	public decimal RemPayan2reDebit
	{
		get
		{
			if (!(AccountEndDore < 0m))
			{
				return 0m;
			}
			return -AccountEndDore;
		}
	}

	public decimal RemPayan2reCredit
	{
		get
		{
			if (!(AccountEndDore > 0m))
			{
				return 0m;
			}
			return AccountEndDore;
		}
	}

	public Guid KolGuid { get; set; }

	public int KolCode { get; set; }

	public string KolName { get; set; }

	public Guid MoeinGuid { get; set; }

	public string Code
	{
		get
		{
			switch (Type)
			{
			case TarazType.TarazKol:
				return KolCode.ToString();
			case TarazType.TarazMoein:
				return MoeinCode.ToString();
			case TarazType.TarazMoeinTafsil:
				if (TafsilGuid == Guid.Empty && MoeinCode > 0)
				{
					return MoeinCode.ToString();
				}
				return TafsilCode.ToString();
			case TarazType.TarazTafsil:
				return TafsilCode.ToString();
			default:
				return "";
			}
		}
	}

	public string Name => NameBySpace;

	public string NameBySlash => KolName + ((!string.IsNullOrEmpty(MoeinName)) ? ("/" + MoeinName) : "") + ((!string.IsNullOrEmpty(TafsilName)) ? ("/" + TafsilName) : "");

	public string NameBySpace
	{
		get
		{
			if (Type == TarazType.TarazTafsil)
			{
				return TafsilName ?? "";
			}
			if (TafsilGuid != Guid.Empty)
			{
				return "    " + TafsilName;
			}
			if (MoeinGuid != Guid.Empty)
			{
				return "  " + MoeinName;
			}
			return KolName;
		}
	}

	public long MoeinCode { get; set; }

	public string MoeinName { get; set; }

	public Guid TafsilGuid { get; set; }

	public long TafsilCode { get; set; }

	public string TafsilName { get; set; }

	public string ContextGuid { get; private set; }

	public TarazType Type { get; private set; }

	public bool IsHead { get; set; }

	public TarazHesabBusiness(string contextGuid, TarazType type)
	{
		ContextGuid = contextGuid;
		Type = type;
	}

	public static Task<List<TarazHesabBusiness>> GetAllAsync(string contextGuid, CancellationToken token, TarazHesabFilter filter, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Reports.GetAllTarazHesabAsync(token, filter, tranName);
	}
}
