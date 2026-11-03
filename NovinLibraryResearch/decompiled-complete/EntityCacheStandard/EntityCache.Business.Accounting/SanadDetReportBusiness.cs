using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business.Accounting;

public class SanadDetReportBusiness : IHasContext
{
	private Guid _kolGuid;

	private MoeinBusiness _moein;

	private KolBusiness _kol;

	private TafsilBusiness _tafsil;

	public int RowIndex { get; set; }

	public MoeinBusiness Moein
	{
		get
		{
			if (_moein == null && MoeinGuid != Guid.Empty)
			{
				_moein = MoeinBusiness.Get(ContextGuid, MoeinGuid);
			}
			return _moein;
		}
	}

	public KolBusiness Kol
	{
		get
		{
			if (_kol == null && KolGuid != Guid.Empty)
			{
				_kol = KolBusiness.Get(ContextGuid, KolGuid);
			}
			return _kol;
		}
	}

	public TafsilBusiness Tafsil
	{
		get
		{
			if (_tafsil == null && TafsilGuid != Guid.Empty)
			{
				_tafsil = TafsilBusiness.Get(ContextGuid, TafsilGuid);
			}
			return _tafsil;
		}
	}

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public string KolName => KolBusiness.Get(ContextGuid, KolGuid)?.Name ?? "";

	public Guid KolGuid
	{
		get
		{
			if (_kolGuid == Guid.Empty && MoeinGuid != Guid.Empty)
			{
				_kolGuid = Moein.KolGuid;
			}
			return _kolGuid;
		}
		set
		{
			_kolGuid = value;
		}
	}

	public long CodeKol => Kol?.Code ?? 0;

	public long CodeMoein => Moein?.Code ?? 0;

	public long CodeTafsil => Tafsil?.Code ?? 0;

	public Guid MoeinGuid { get; set; }

	public string MoeinName => Moein?.Name ?? "";

	public Guid TafsilGuid { get; set; }

	public string TafsilName => Tafsil?.Name ?? "";

	public Guid UserGuid { get; set; }

	public string UserName { get; set; }

	public decimal Debit { get; set; }

	public decimal Credit { get; set; }

	public Guid DecimalGuid { get; set; }

	public string Description { get; set; } = "";


	public float Qt { get; set; }

	public Guid MasterGuid { get; set; }

	public string SanadNo { get; set; }

	public short AsType { get; set; }

	public Guid AsGuid { get; set; }

	public decimal Remain { get; set; }

	public string RemForDisplay => Math.Abs(Remain).NpToString("N0", 0);

	public string RemainForDisplay
	{
		get
		{
			try
			{
				if (Remain == 0m)
				{
					return "بی حساب".GetTranslate();
				}
				if (Remain < 0m)
				{
					return Math.Abs(Remain).NpToString("N0", 0);
				}
				return Remain.NpToString("N0", 0);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemainForDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadDetReportBusiness.cs", 92);
				return "";
			}
		}
	}

	public string Tashkhis => Remain.AccountTashkhis();

	public bool IsChecked { get; set; }

	public DateTime DateM { get; set; }

	public string DateSh => DateM.MiladiToCalture();

	[Obsolete("Use Account Table", true)]
	public decimal CurrentTafsilAccount => Tafsil?.Account ?? 0m;

	public string ContextGuid { get; set; }

	[Obsolete]
	public int AsId { get; set; }

	public SanadDetReportBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static void UpdateList(IEnumerable<SanadDetReportBusiness> list)
	{
		try
		{
			decimal num = default(decimal);
			if (list == null)
			{
				return;
			}
			foreach (SanadDetReportBusiness item in list)
			{
				if (item.RowIndex == 1)
				{
					num = default(decimal);
				}
				num = (item.Remain = num + item.Credit - item.Debit);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadDetReportBusiness.cs", 127);
			throw;
		}
	}

	public static async Task<Guid> AsId_To_AsGuidAsync(string contextGuid, int asId, Document asType, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		Guid empty = Guid.Empty;
		try
		{
			if (ClsCache.GetCache(contextGuid).Source != ENSource.Accounting)
			{
				return empty;
			}
			empty = await ClsCache.GetCache(contextGuid).CacheRepository.ISanad.AsId_To_AsGuidAsync(asId, asType, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AsId_To_AsGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadDetReportBusiness.cs", 142);
			throw;
		}
		return empty;
	}
}
