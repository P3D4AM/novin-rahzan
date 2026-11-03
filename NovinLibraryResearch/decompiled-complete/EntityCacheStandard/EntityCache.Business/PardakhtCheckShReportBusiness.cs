using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Entities.MetaData;
using PacketParser.Filters;

namespace EntityCache.Business;

public class PardakhtCheckShReportBusiness : BaseKhazaneDetail
{
	private Guid? a2re_MoeinGuid;

	private Guid? a2re_TafsilGuid;

	private string _tafsilName = "";

	public new decimal Price { get; set; }

	public override KhazaneType Type => KhazaneType.ChequeSh;

	public DateTime Modified { get; set; } = DateTime.Now;


	public ServerStatusForClient ServerStatus { get; set; }

	public bool IsEnteghali { get; set; }

	public Guid? UserGuid { get; set; }

	public DateTime DateSaresid { get; set; } = DateTime.Now;


	public override DateTime DateM
	{
		get
		{
			return DateSaresid;
		}
		set
		{
			DateSaresid = value;
		}
	}

	public string DateSarresidSh => DateSaresid.MiladiToCalture();

	public string Description { get; set; }

	public string Number { get; set; }

	public ENCheckSHStatus Status { get; set; }

	public string StatusName => Status.GetDisplay();

	public Guid? CreditTafsilGuid { get; set; }

	public Guid CreditMoeinGuid { get; set; }

	public Guid BankTafsilGuid { get; set; }

	public DateTime PardakhtDate { get; set; } = DateTime.Now;


	public string PardakhtDateSh => PardakhtDate.MiladiToCalture();

	public bool Aval2re { get; set; }

	public Guid? A2re_MoeinGuid
	{
		get
		{
			return a2re_MoeinGuid;
		}
		set
		{
			if ((value ?? Guid.Empty) == Guid.Empty)
			{
				a2re_MoeinGuid = null;
			}
			else
			{
				a2re_MoeinGuid = value;
			}
		}
	}

	public Guid? A2re_TafsilGuid
	{
		get
		{
			return a2re_TafsilGuid;
		}
		set
		{
			if ((value ?? Guid.Empty) == Guid.Empty)
			{
				a2re_TafsilGuid = null;
			}
			else
			{
				a2re_TafsilGuid = value;
			}
		}
	}

	public string BankName { get; set; }

	public string ShortDescription => AndroidPrintText;

	public string AndroidPrintText => "چک س" + Number + " " + BankName + " ";

	public string PriceString => Price.NpToString("N0", 0);

	public bool IsChecked { get; set; }

	public Guid? DebitA2reMoeinGuid { get; set; }

	public Guid? DebitA2reTafsiGuid { get; set; }

	public Guid? TafsilGuid { get; set; }

	public string TafsilName
	{
		get
		{
			if (ClsCache.GetCache(base.ContextGuid).Source == ENSource.Baje)
			{
				return _tafsilName;
			}
			if (Aval2re && IsEnteghali)
			{
				return _tafsilName + " (ابتدای دوره انتقالی)";
			}
			if (Aval2re)
			{
				return _tafsilName + " (ابتدای دوره)";
			}
			return _tafsilName;
		}
		set
		{
			_tafsilName = value;
		}
	}

	public int? SanadNo { get; set; }

	public string SayadiNationalCode { get; set; }

	public string SayadiIdentity { get; set; }

	public string SayadiName { get; set; }

	public override string DescriptionForUi => Description;

	public PardakhtChequeShMetaData MetaData { get; set; }

	public bool SayadiRegistered { get; set; }

	public override FileManagement_Parts Part => FileManagement_Parts.Cheque;

	internal async Task<string> GetUiDescriptionAsync()
	{
		return Status switch
		{
			ENCheckSHStatus.Pardakhtani => "چک پرداختنی شخصی" + (string.IsNullOrEmpty(Description) ? "" : ("\r\n" + Description)), 
			ENCheckSHStatus.VosolShode => "چک شخصی پرداخت شده" + (string.IsNullOrEmpty(Description) ? "" : ("\r\n" + Description)), 
			ENCheckSHStatus.BargashtShode => "چک شخصی برگشت شده" + (string.IsNullOrEmpty(Description) ? "" : ("\r\n" + Description)), 
			ENCheckSHStatus.BatelShode => "چک شخصی باطل شده" + (string.IsNullOrEmpty(Description) ? "" : ("\r\n" + Description)), 
			ENCheckSHStatus.Entezami_Pardakhtani => "چک انتظامی موجود" + (string.IsNullOrEmpty(Description) ? "" : ("\r\n" + Description)), 
			ENCheckSHStatus.Entezami_Odat => "چک انتظامی عودت شده" + (string.IsNullOrEmpty(Description) ? "" : ("\r\n" + Description)), 
			_ => throw new NotImplementedException($"chequeSh status is not valid. {Status}"), 
		};
	}

	internal short StatusOrdinal()
	{
		return (short)Status;
	}

	public static Task<IEnumerable<BedehiTalabModel>> GetAllReportAsync(string contextGuid, BedehiTalabReportFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IPardakhtCheckSh.GetAllReportAsync(filter, token, tranName);
	}

	public override Task<string> GetShortDescriptionAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		throw new NotImplementedException();
	}
}
