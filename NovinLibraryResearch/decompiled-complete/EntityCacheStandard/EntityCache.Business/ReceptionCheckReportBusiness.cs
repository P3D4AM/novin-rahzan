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

public class ReceptionCheckReportBusiness : BaseKhazaneDetail
{
	private Guid? a2re_PardazandeTafsilGuid;

	private Guid? a2re_PardazandeMoeinGuid;

	private string _tafsilName = "";

	public new decimal Price { get; set; }

	public override KhazaneType Type => KhazaneType.ChequeM;

	public DateTime Modified { get; set; } = DateTime.Now;


	public string BankName { get; set; }

	public DateTime Date { get; set; }

	public string DateSh => Date.MiladiToCalture();

	public DateTime? ReceptionDate { get; set; }

	public string ReceptionMasterDateSh => ReceptionDate.MiladiToCalture();

	public DateTime? DateBargashtOrVosol { get; set; }

	public string DateBargashtOrVosolSh => DateBargashtOrVosol.MiladiToCalture();

	public Guid? VisitorGuid { get; set; }

	public ENUMCheck Status { get; set; }

	public string StatusForDisplay => Status.GetDisplay();

	public bool IsEnteghali { get; set; }

	public Guid? UserGuid { get; set; }

	public Guid? A2re_PardazandeTafsilGuid
	{
		get
		{
			return a2re_PardazandeTafsilGuid;
		}
		set
		{
			a2re_PardazandeTafsilGuid = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid? A2re_PardazandeMoeinGuid
	{
		get
		{
			return a2re_PardazandeMoeinGuid;
		}
		set
		{
			a2re_PardazandeMoeinGuid = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public DateTime? A2re_ReceptionDate { get; set; }

	public bool Aval2re { get; set; }

	public string Description { get; set; }

	public string HesabNum { get; set; }

	public string Number { get; set; }

	public bool Personal { get; set; }

	public string ChequeTypeForDisplay
	{
		get
		{
			if (!Personal)
			{
				return "چک اشخاص";
			}
			return "چک شخصی";
		}
	}

	public long PoshtNomreh { get; set; }

	public Guid DebitMoeinGuid { get; set; }

	public Guid? DebitTafsilGuid { get; set; }

	public string TafsilName
	{
		get
		{
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

	public Guid? TafsilGuid { get; set; }

	public Guid? CreditA2reMoeinGuid { get; set; }

	public Guid? CreditA2reTafsilGuid { get; set; }

	public bool IsChecked { get; set; }

	public string Place { get; set; }

	public Guid? PlaceGuid { get; set; }

	public DateTime? DateToPlace { get; set; }

	public string DateToPlaceSh => DateToPlace.MiladiToCalture();

	public int? SanadNo { get; set; }

	public string SayadiNationalCode { get; set; }

	public string SayadiIdentity { get; set; }

	public string SayadiName { get; set; }

	public ReceptionChequeMetaData MetaData { get; set; }

	public EnChequeType ChequeType => MetaData?.ChequeType ?? EnChequeType.Traditional;

	public override string DescriptionForUi => Description;

	public bool SayadiRegistered { get; set; }

	public override FileManagement_Parts Part => FileManagement_Parts.Cheque;

	internal short StatusOrdinal()
	{
		return checked((short)Status);
	}

	public static Task<IEnumerable<BedehiTalabModel>> GetAllReportAsync(string contextGuid, BedehiTalabReportFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IReceptionCheckRepository.GetAllReportAsync(filter, token, tranName);
	}

	public override Task<string> GetShortDescriptionAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		throw new NotImplementedException();
	}

	internal Task<string> GetUiDescriptionAsync()
	{
		return Status switch
		{
			ENUMCheck.Mojod => Task.FromResult("چک دریافتنی موجود در صندوق".GetTranslate() + (string.IsNullOrEmpty(Description) ? "" : ("\r\n" + Description))), 
			ENUMCheck.Bargashti => Task.FromResult("چک دریافتنی برگشتی".GetTranslate() + (string.IsNullOrEmpty(Description) ? "" : ("\r\n" + Description))), 
			ENUMCheck.KharjShode => Task.FromResult("خرج شده به".GetTranslate() + Place + (string.IsNullOrEmpty(Description) ? "" : ("\r\n" + Description))), 
			ENUMCheck.VagozarShode => Task.FromResult("واگذار شده به".GetTranslate() + Place + (string.IsNullOrEmpty(Description) ? "" : ("\r\n" + Description))), 
			ENUMCheck.NaghdShode => Task.FromResult("نقد شده به".GetTranslate() + Place + (string.IsNullOrEmpty(Description) ? "" : ("\r\n" + Description))), 
			ENUMCheck.BatelShode => Task.FromResult("باطل شده".GetTranslate() + (string.IsNullOrEmpty(Description) ? "" : ("\r\n" + Description))), 
			ENUMCheck.VosolShode => Task.FromResult("وصول شده به".GetTranslate() + " " + Place + (string.IsNullOrEmpty(Description) ? "" : ("\r\n" + Description))), 
			ENUMCheck.Entezami_Mojod => Task.FromResult("چک انتظامی".GetTranslate() + " " + Place + (string.IsNullOrEmpty(Description) ? "" : ("\r\n" + Description))), 
			ENUMCheck.Entezami_Odat => Task.FromResult("چک انتظامی عودت شده ".GetTranslate() + Place + (string.IsNullOrEmpty(Description) ? "" : ("\r\n" + Description))), 
			_ => throw new NotImplementedException($"ChequeM status is not valid. {Status}"), 
		};
	}
}
