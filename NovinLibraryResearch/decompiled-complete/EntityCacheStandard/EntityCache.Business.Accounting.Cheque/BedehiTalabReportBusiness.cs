using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Aghsat;
using EntityCache.EventArguments;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;
using PacketParser.Filters.Configs;
using PacketParser.Filters.Khazane;
using Serilog;

namespace EntityCache.Business.Accounting.Cheque;

public class BedehiTalabReportBusiness : IHasContext
{
	public readonly ReceptionCheckReportBusiness ChequeM;

	public readonly PardakhtCheckShReportBusiness ChequeSh;

	public readonly AghsatMBusiness AghsatM;

	public readonly AghsatShBusiness AghsatSh;

	public Guid Guid { get; set; }

	public short PriorityForUiBinding => Type switch
	{
		EnBedehiTalabType.CheckM => 3, 
		EnBedehiTalabType.CheckSh => 1, 
		EnBedehiTalabType.AghsatM => 4, 
		EnBedehiTalabType.AghsatSh => 2, 
		_ => 5, 
	};

	public EnBedehiTalabType Type
	{
		get
		{
			try
			{
				if (ChequeM != null)
				{
					return EnBedehiTalabType.CheckM;
				}
				if (ChequeSh != null)
				{
					return EnBedehiTalabType.CheckSh;
				}
				if (AghsatM != null)
				{
					return EnBedehiTalabType.AghsatM;
				}
				if (AghsatSh != null)
				{
					return EnBedehiTalabType.AghsatSh;
				}
				throw new InvalidOperationException("type sould not be detected!");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Type", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 83);
			}
			return (EnBedehiTalabType)0;
		}
	}

	public string TypeName => Type.GetDisplay().GetTranslate();

	public string BankName { get; set; }

	public decimal Price { get; set; }

	public short StatusOrdinal
	{
		get
		{
			try
			{
				return Type switch
				{
					EnBedehiTalabType.CheckM => ChequeM.StatusOrdinal(), 
					EnBedehiTalabType.CheckSh => ChequeSh.StatusOrdinal(), 
					EnBedehiTalabType.AghsatM => 1, 
					EnBedehiTalabType.AghsatSh => 1, 
					_ => throw new ArgumentOutOfRangeException($"Type({Type}) is not valid! for StatusStr."), 
				};
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "StatusOrdinal", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 105);
			}
			return 0;
		}
	}

	public string StatusStr
	{
		get
		{
			try
			{
				return Type switch
				{
					EnBedehiTalabType.CheckM => ChequeM.StatusForDisplay, 
					EnBedehiTalabType.CheckSh => ChequeSh.StatusName, 
					EnBedehiTalabType.AghsatM => AghsatM.StatusName, 
					EnBedehiTalabType.AghsatSh => AghsatSh.StatusName, 
					_ => throw new ArgumentOutOfRangeException($"Type({Type}) is not valid! for StatusStr."), 
				};
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "StatusStr", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 124);
			}
			return null;
		}
	}

	public DateTime DateM { get; set; }

	public string DateSh => DateM.MiladiToCalture();

	public string Number { get; set; }

	public Guid TafsilGuid { get; set; }

	public string TafsilName { get; set; }

	public string Description { get; set; }

	public List<BedehiTalabAction> Actions
	{
		get
		{
			try
			{
				switch (Type)
				{
				case EnBedehiTalabType.CheckM:
					return ChequeM.Status switch
					{
						ENUMCheck.Mojod => new List<BedehiTalabAction>
						{
							new BedehiTalabAction(EnChequeOperation.BargashtZadan, new BedehiTalabParams(), BedehiTalabReportBusiness_BargashtZadanEvent),
							new BedehiTalabAction(EnChequeOperation.BatelKardan, new BedehiTalabParams(), BedehiTalabReportBusiness_BatelKardanEvent),
							new BedehiTalabAction(EnChequeOperation.VagozarKardan, new BedehiTalabParams(tafsilReq: true, HesabType.Bank), BedehiTalabReportBusiness_VagozarKardanEvent),
							new BedehiTalabAction(EnChequeOperation.NaghdKardan, new BedehiTalabParams(tafsilReq: true, HesabType.Sandogh), BedehiTalabReportBusiness_NaghdKardanEvent)
						}, 
						ENUMCheck.Bargashti => new List<BedehiTalabAction>
						{
							new BedehiTalabAction(EnChequeOperation.BargashtoToEx, new BedehiTalabParams(), BedehiTalabReportBusiness_BargashtiToExEvent),
							new BedehiTalabAction(EnChequeOperation.OdatBargashti, new BedehiTalabParams(), BedehiTalabReportBusiness_OdatBargashtiEvent)
						}, 
						ENUMCheck.KharjShode => new List<BedehiTalabAction>
						{
							new BedehiTalabAction(EnChequeOperation.BargashtZadan, new BedehiTalabParams(), BedehiTalabReportBusiness_BargashtZadanEvent),
							new BedehiTalabAction(EnChequeOperation.OdatKharjShode, new BedehiTalabParams(), BedehiTalabReportBusiness_OdatKharjShodeEvent)
						}, 
						ENUMCheck.VagozarShode => new List<BedehiTalabAction>
						{
							new BedehiTalabAction(EnChequeOperation.VagozarToMojod, new BedehiTalabParams(), BedehiTalabReportBusiness_VagozarToMojodEvent),
							new BedehiTalabAction(EnChequeOperation.VosolKardan, new BedehiTalabParams(), BedehiTalabReportBusiness_VosolKardanEvent),
							new BedehiTalabAction(EnChequeOperation.BargashtZadan, new BedehiTalabParams(), BedehiTalabReportBusiness_BargashtZadanEvent)
						}, 
						ENUMCheck.NaghdShode => new List<BedehiTalabAction>
						{
							new BedehiTalabAction(EnChequeOperation.NaghdToMojod, new BedehiTalabParams(), BedehiTalabReportBusiness_NaghdToMojodEvent)
						}, 
						ENUMCheck.BatelShode => new List<BedehiTalabAction>
						{
							new BedehiTalabAction(EnChequeOperation.BatelToMojod, new BedehiTalabParams(), BedehiTalabReportBusiness_BatelToMojodEvent)
						}, 
						ENUMCheck.VosolShode => new List<BedehiTalabAction>
						{
							new BedehiTalabAction(EnChequeOperation.VosolToVagozar, new BedehiTalabParams(), BedehiTalabReportBusiness_VosolToVagozarEvent)
						}, 
						ENUMCheck.Entezami_Mojod => new List<BedehiTalabAction>
						{
							new BedehiTalabAction(EnChequeOperation.OdatEntezami, new BedehiTalabParams(), BedehiTalabReportBusiness_ReceptionEntezamiOdatEvent)
						}, 
						ENUMCheck.Entezami_Odat => new List<BedehiTalabAction>
						{
							new BedehiTalabAction(EnChequeOperation.OdatToMojodEntezami, new BedehiTalabParams(), BedehiTalabReportBusiness_ReceptionEntezamiOdatToMojoEvent)
						}, 
						_ => throw new ArgumentOutOfRangeException("Status"), 
					};
				case EnBedehiTalabType.CheckSh:
					return ChequeSh.Status switch
					{
						ENCheckSHStatus.Pardakhtani => new List<BedehiTalabAction>
						{
							new BedehiTalabAction(EnChequeOperation.ElamPardakht, new BedehiTalabParams(), BedehiTalabReportBusiness_ElamPardakhtEvent),
							new BedehiTalabAction(EnChequeOperation.BargashtZadan, new BedehiTalabParams(), BedehiTalabReportBusiness_BargashtZadanEvent),
							new BedehiTalabAction(EnChequeOperation.BatelKardan, new BedehiTalabParams(), BedehiTalabReportBusiness_BatelKardanEvent)
						}, 
						ENCheckSHStatus.VosolShode => new List<BedehiTalabAction>
						{
							new BedehiTalabAction(EnChequeOperation.PardakhtShodeToPardakhtani, new BedehiTalabParams(), BedehiTalabReportBusiness_PardakhtShodeToPardakhtaniEvent)
						}, 
						ENCheckSHStatus.BatelShode => new List<BedehiTalabAction>
						{
							new BedehiTalabAction(EnChequeOperation.BatelToPardakhtani, new BedehiTalabParams(), BedehiTalabReportBusiness_BatelShodeToPardakhtaniEvent)
						}, 
						ENCheckSHStatus.BargashtShode => new List<BedehiTalabAction>
						{
							new BedehiTalabAction(EnChequeOperation.BargashtiToPardakhtani, new BedehiTalabParams(), BedehiTalabReportBusiness_BargashtiToPardakhtaniEvent)
						}, 
						ENCheckSHStatus.Entezami_Odat => new List<BedehiTalabAction>
						{
							new BedehiTalabAction(EnChequeOperation.OdatToMojodEntezami, new BedehiTalabParams(), BedehiTalabReportBusiness_PardakhtEntezamiOdatToMojodEvent)
						}, 
						ENCheckSHStatus.Entezami_Pardakhtani => new List<BedehiTalabAction>
						{
							new BedehiTalabAction(EnChequeOperation.OdatEntezami, new BedehiTalabParams(), BedehiTalabReportBusiness_PardakhtEntezamiOdatEvent)
						}, 
						_ => throw new ArgumentOutOfRangeException("Status"), 
					};
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Actions", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 239);
			}
			return null;
		}
	}

	public string ContextGuid { get; private set; }

	private BedehiTalabReportBusiness eventmavager => ClsCache.GetCache(ContextGuid).BusinessAggregator.BedehiTalab;

	public event Func<BedehiTalabEventArgs, Task<ReturnedSaveFuncInfo>> BargashtZadanEvent;

	public event Func<BedehiTalabEventArgs, Task<ReturnedSaveFuncInfo>> BatelKardanEvent;

	public event Func<BedehiTalabEventArgs, Task<ReturnedSaveFuncInfo>> VagozarKardanEvent;

	public event Func<BedehiTalabEventArgs, Task<ReturnedSaveFuncInfo>> NaghdKardanEvent;

	public event Func<BedehiTalabEventArgs, Task<ReturnedSaveFuncInfo>> BargashtoToExEvent;

	public event Func<BedehiTalabEventArgs, Task<ReturnedSaveFuncInfo>> OdatBargashtiEvent;

	public event Func<BedehiTalabEventArgs, Task<ReturnedSaveFuncInfo>> OdatKharjShodeEvent;

	public event Func<BedehiTalabEventArgs, Task<ReturnedSaveFuncInfo>> VagozarToMojodEvent;

	public event Func<BedehiTalabEventArgs, Task<ReturnedSaveFuncInfo>> VosolKardanEvent;

	public event Func<BedehiTalabEventArgs, Task<ReturnedSaveFuncInfo>> NaghdToMojodEvent;

	public event Func<BedehiTalabEventArgs, Task<ReturnedSaveFuncInfo>> BatelToMojodEvent;

	public event Func<BedehiTalabEventArgs, Task<ReturnedSaveFuncInfo>> VosolToVagozarEvent;

	public event Func<BedehiTalabEventArgs, Task<ReturnedSaveFuncInfo>> ElamPardakhtEvent;

	public event Func<BedehiTalabEventArgs, Task<ReturnedSaveFuncInfo>> PardakhtShodeToPardakhtaniEvent;

	public event Func<BedehiTalabEventArgs, Task<ReturnedSaveFuncInfo>> BatelToPardakhtaniEvent;

	public event Func<BedehiTalabEventArgs, Task<ReturnedSaveFuncInfo>> BargashtiToPardakhtaniEvent;

	public event Func<BedehiTalabEventArgs, Task<ReturnedSaveFuncInfo>> ReceptionEntezamiOdatEvent;

	public event Func<BedehiTalabEventArgs, Task<ReturnedSaveFuncInfo>> ReceptionEntezamiOdatToMojodEvent;

	public event Func<BedehiTalabEventArgs, Task<ReturnedSaveFuncInfo>> PardakhtEntezamiOdatEvent;

	public event Func<BedehiTalabEventArgs, Task<ReturnedSaveFuncInfo>> PardakhtEntezamiOdatToMojodEvent;

	public event Func<object, EventArgs, Task> OnChangeChequeStatus;

	private BedehiTalabReportBusiness(string contextGuid, ReceptionCheckReportBusiness chequeM)
	{
		try
		{
			ChequeM = chequeM ?? throw new NullReferenceException("chequeM");
			BankName = chequeM.BankName;
			DateM = chequeM.Date;
			Guid = chequeM.Guid;
			Number = chequeM.Number;
			Price = chequeM.Price;
			TafsilName = chequeM.TafsilName;
			TafsilGuid = chequeM.TafsilGuid ?? Guid.Empty;
			ContextGuid = contextGuid;
			Description = chequeM.Description;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 261);
		}
	}

	private BedehiTalabReportBusiness(string contextGuid, PardakhtCheckShReportBusiness chequeSh)
	{
		try
		{
			ChequeSh = chequeSh ?? throw new NullReferenceException("chequeSh");
			BankName = chequeSh.BankName;
			DateM = chequeSh.DateSaresid;
			Guid = chequeSh.Guid;
			Number = chequeSh.Number;
			Price = chequeSh.Price;
			TafsilName = chequeSh.TafsilName;
			TafsilGuid = chequeSh.TafsilGuid ?? Guid.Empty;
			ContextGuid = contextGuid;
			Description = chequeSh.Description;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 279);
		}
	}

	private BedehiTalabReportBusiness(string contextGuid, AghsatMBusiness aghsatM)
	{
		try
		{
			AghsatM = aghsatM ?? throw new NullReferenceException("aghsatM");
			BankName = aghsatM.Description;
			DateM = aghsatM.DateSarResid;
			Guid = aghsatM.Guid;
			Number = aghsatM.Id2.ToString();
			Price = aghsatM.TotalPrice;
			TafsilName = aghsatM.FullName;
			ContextGuid = contextGuid;
			Description = aghsatM.Description;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 295);
		}
	}

	private BedehiTalabReportBusiness(string contextGuid, AghsatShBusiness aghsatSh)
	{
		try
		{
			AghsatSh = aghsatSh ?? throw new NullReferenceException("aghsatSh");
			BankName = aghsatSh.Description;
			DateM = aghsatSh.DateSarResid;
			Guid = aghsatSh.Guid;
			Number = aghsatSh.ID2.ToString();
			Price = aghsatSh.Price;
			TafsilName = aghsatSh.FullName;
			TafsilGuid = aghsatSh.TafsilGuid;
			ContextGuid = contextGuid;
			Description = aghsatSh.Description;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 312);
		}
	}

	internal BedehiTalabReportBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	private static async Task<BedehiTalabReportBusiness> GetFactoryAsync(string contextGuid, AghsatMBusiness aghsatM)
	{
		try
		{
			return new BedehiTalabReportBusiness(contextGuid, aghsatM)
			{
				TafsilGuid = (await aghsatM.GetGroupAsync()).TafsilGuid
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetFactoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 327);
			throw;
		}
	}

	public static async Task<List<BedehiTalabReportBusiness>> GetAllAsync(string contextGuid, BedehiTalabFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		List<BedehiTalabReportBusiness> lst2 = new List<BedehiTalabReportBusiness>();
		try
		{
			if (filter == null)
			{
				filter = new BedehiTalabFilter();
			}
			BedehiTalabFilter bedehiTalabFilter = filter;
			if (bedehiTalabFilter.TypeList == null)
			{
				bedehiTalabFilter.TypeList = new List<EnBedehiTalabType>();
			}
			if (filter.TypeList.Count == 0)
			{
				foreach (EnBedehiTalabType value in Enum.GetValues(typeof(EnBedehiTalabType)))
				{
					filter.TypeList.Add(value);
				}
			}
			ENSource enSource = ClsCache.GetCache(contextGuid).Source;
			if (filter.TypeList.Any((EnBedehiTalabType a) => a == EnBedehiTalabType.CheckM))
			{
				ReceptionChequeFilter filter2 = new ReceptionChequeFilter
				{
					DateFrom = filter.Date1,
					DateTo = filter.Date2
				};
				IEnumerable<ReceptionCheckReportBusiness> enumerable = await ReceptionCheckBusiness.GetAllAsync(contextGuid, filter2, token, tranName);
				if (enSource != ENSource.Android_HesabYar)
				{
					enumerable = enumerable?.Where((ReceptionCheckReportBusiness a) => a.Status == ENUMCheck.Mojod || a.Status == ENUMCheck.VagozarShode);
				}
				if (enumerable != null)
				{
					lst2.AddRange(enumerable.Select((ReceptionCheckReportBusiness p) => new BedehiTalabReportBusiness(contextGuid, p)));
				}
			}
			if (filter.TypeList.Any((EnBedehiTalabType a) => a == EnBedehiTalabType.CheckSh))
			{
				PardakhtChequeFilter pardakhtChequeFilter = new PardakhtChequeFilter
				{
					DateFrom = filter.Date1,
					DateTo = filter.Date2,
					Status = ENCheckSHStatus.Pardakhtani
				};
				if (ClsCache.GetCache(contextGuid).Source == ENSource.Android_HesabYar)
				{
					pardakhtChequeFilter.Status = null;
				}
				IEnumerable<PardakhtCheckShReportBusiness> enumerable2 = await PardakhtCheckShBusiness.GetAllAsync(contextGuid, pardakhtChequeFilter, token, tranName);
				if (enumerable2 != null)
				{
					lst2.AddRange(enumerable2.Select((PardakhtCheckShReportBusiness p) => new BedehiTalabReportBusiness(contextGuid, p)));
				}
			}
			if (filter.TypeList.Any((EnBedehiTalabType a) => a == EnBedehiTalabType.AghsatSh))
			{
				AghsatShFilter aghsatShFilter = new AghsatShFilter
				{
					DateFrom = filter.Date1,
					DateTo = filter.Date2
				};
				if (enSource != ENSource.Android_HesabYar)
				{
					aghsatShFilter.Status = EnAghsatShStatus.NotPayed;
				}
				IEnumerable<AghsatShBusiness> enumerable3 = await AghsatShBusiness.GetAllAsync(contextGuid, aghsatShFilter, token, tranName);
				if (enumerable3 != null)
				{
					if (filter.Date1.HasValue)
					{
						enumerable3 = enumerable3.Where((AghsatShBusiness w) => w.DateSarResid >= filter.Date1.Value && w.DateSarResid <= filter.Date2.Value);
					}
					enumerable3 = enumerable3?.Where((AghsatShBusiness q) => q.PardakhtSum == 0m);
					lst2.AddRange(enumerable3.Select((AghsatShBusiness p) => new BedehiTalabReportBusiness(contextGuid, p)));
				}
			}
			if (filter.TypeList.Any((EnBedehiTalabType a) => a == EnBedehiTalabType.AghsatM))
			{
				AghsatMFilter aghsatMFilter = new AghsatMFilter
				{
					DateFrom = filter?.Date1,
					DateTo = filter?.Date2
				};
				if (enSource != ENSource.Android_HesabYar)
				{
					aghsatMFilter.Status = EnAghsatMStatus.NotRecieved;
				}
				IEnumerable<AghsatMBusiness> enumerable4 = await AghsatMBusiness.GetAllAsync(contextGuid, aghsatMFilter, token, tranName);
				if (enumerable4 != null)
				{
					if (filter.Date1.HasValue)
					{
						enumerable4 = enumerable4.Where((AghsatMBusiness w) => w.DateSarResid >= filter.Date1.Value && w.DateSarResid <= filter.Date2.Value);
					}
					if (enumerable4 != null && enumerable4.Any())
					{
						foreach (AghsatMBusiness item2 in enumerable4)
						{
							List<BedehiTalabReportBusiness> list = lst2;
							list.Add(await GetFactoryAsync(contextGuid, item2));
						}
					}
				}
			}
			if (!string.IsNullOrEmpty(filter.Search))
			{
				List<string> list2 = filter.Search.SplitString();
				if (list2 != null && list2.Count > 0)
				{
					foreach (string item in list2)
					{
						if (!string.IsNullOrEmpty(item) && item.Trim() != "")
						{
							lst2 = lst2.Where((BedehiTalabReportBusiness x) => x.BankName.ToLower().Contains(item) || x.TafsilName.ToLower().Contains(item) || x.Number.ToLower().Contains(item))?.ToList();
						}
					}
				}
			}
			List<BedehiTalabReportBusiness> list3 = lst2;
			if (list3 != null && list3.Count > 0 && filter.FromPrice.HasValue && filter.ToPrice.HasValue)
			{
				lst2 = lst2.Where(delegate(BedehiTalabReportBusiness s)
				{
					decimal price = s.Price;
					decimal? fromPrice = filter.FromPrice;
					if ((price >= fromPrice.GetValueOrDefault()) & fromPrice.HasValue)
					{
						decimal price2 = s.Price;
						fromPrice = filter.ToPrice;
						return (price2 <= fromPrice.GetValueOrDefault()) & fromPrice.HasValue;
					}
					return false;
				}).ToList();
			}
			IEnumerable<BedehiTalabReportBusiness> enumerable5 = await ApplyAccessAsync(contextGuid, lst2, EnSystem.UserInterface, token);
			if (enumerable5 == null || !enumerable5.Any())
			{
				return null;
			}
			return enumerable5?.OrderBy((BedehiTalabReportBusiness a) => a.DateM)?.ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 421);
			lst2 = null;
		}
		return lst2?.OrderBy((BedehiTalabReportBusiness p) => p.DateM)?.ToList();
	}

	private static async Task<IEnumerable<BedehiTalabReportBusiness>> ApplyAccessAsync(string contextGuid, IEnumerable<BedehiTalabReportBusiness> items, EnSystem system, CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token);
			if (cashierBusiness == null)
			{
				return items;
			}
			if (token.IsCancellationRequested)
			{
				return null;
			}
			UserAccBusiness userAccBusiness = await cashierBusiness.GetUserAsync(token);
			if (token.IsCancellationRequested)
			{
				return null;
			}
			if (userAccBusiness == null)
			{
				throw new NullReferenceException("UserIs Null!!");
			}
			if (!userAccBusiness.UserAccess.Check.Check_SeeList)
			{
				items = items?.Where((BedehiTalabReportBusiness q) => q.Type != EnBedehiTalabType.CheckM && q.Type != EnBedehiTalabType.CheckSh);
			}
			if (!userAccBusiness.UserAccess.Aghsat.Aghsat_M_Report)
			{
				items = items?.Where((BedehiTalabReportBusiness q) => q.Type != EnBedehiTalabType.AghsatM);
			}
			if (!userAccBusiness.UserAccess.Aghsat.Aghsat_SH_Report)
			{
				items = items?.Where((BedehiTalabReportBusiness q) => q.Type != EnBedehiTalabType.AghsatSh);
			}
			items = items?.ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 448);
			throw;
		}
		if (token.IsCancellationRequested)
		{
			return null;
		}
		return items;
	}

	public async Task<string> GetUiDescriptionAsync()
	{
		_ = 3;
		try
		{
			return Type switch
			{
				EnBedehiTalabType.CheckM => await ChequeM.GetUiDescriptionAsync(), 
				EnBedehiTalabType.CheckSh => await ChequeSh.GetUiDescriptionAsync(), 
				EnBedehiTalabType.AghsatM => await AghsatM.GetUiDescriptionAsync(), 
				EnBedehiTalabType.AghsatSh => await AghsatSh.GetUiDescriptionAsync(), 
				_ => throw new NotImplementedException($"type not defined! {Type}"), 
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetUiDescriptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 469);
			return "خطا در کد " + ex.Message;
		}
	}

	public static async Task<IEnumerable<BedehiTalabModel>> GetAllReportAsync(string contextGuid, BedehiTalabReportFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 3;
		try
		{
			IEnumerable<BedehiTalabModel> enumerable = null;
			switch (filter.Type)
			{
			case EnBedehiTalabType.CheckM:
				enumerable = await ReceptionCheckReportBusiness.GetAllReportAsync(contextGuid, filter, token, tranName);
				break;
			case EnBedehiTalabType.CheckSh:
				enumerable = await PardakhtCheckShReportBusiness.GetAllReportAsync(contextGuid, filter, token, tranName);
				break;
			case EnBedehiTalabType.AghsatM:
				enumerable = await AghsatMBusiness.GetAllReportAsync(contextGuid, filter, token, tranName);
				break;
			case EnBedehiTalabType.AghsatSh:
				enumerable = await AghsatShBusiness.GetAllReportAsync(contextGuid, filter, token, tranName);
				break;
			}
			if (enumerable == null || !enumerable.Any())
			{
				return null;
			}
			BedehiTalabModel bedehiTalabModel = null;
			List<BedehiTalabModel> list = enumerable?.ToList();
			foreach (BedehiTalabModel item in enumerable)
			{
				if (bedehiTalabModel == null)
				{
					bedehiTalabModel = item;
					continue;
				}
				DateTime dateTime = bedehiTalabModel.DateSh.ShamsiToMiladi();
				TimeSpan timeSpan = item.DateSh.ShamsiToMiladi() - dateTime;
				if (timeSpan.Days > 1)
				{
					for (int i = 1; i <= timeSpan.Days; i = checked(i + 1))
					{
						list.Add(new BedehiTalabModel
						{
							Count = 0m,
							Sum = 0m,
							DateSh = dateTime.AddDays(i).MiladiToCalture()
						});
					}
					bedehiTalabModel = item;
				}
			}
			return list?.OrderBy((BedehiTalabModel q) => q.DateSh)?.ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 526);
			throw;
		}
	}

	private async Task<ReturnedSaveFuncInfo> BedehiTalabReportBusiness_BargashtZadanEvent(BedehiTalabEventArgs e)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		if (e == null)
		{
			throw new ArgumentNullException("e");
		}
		try
		{
			if (Type == EnBedehiTalabType.CheckSh)
			{
				ChequeShOperationConfig config = new ChequeShOperationConfig
				{
					ChequeGuid = Guid,
					DateM = e.DateM,
					SanadNo = e.SanadNo
				};
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ChequeShOperationBusiness.BargashtZadanAsync(ContextGuid, config), ReturnedState.None, "BedehiTalabReportBusiness_BargashtZadanEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 544);
			}
			else if (Type == EnBedehiTalabType.CheckM)
			{
				ChequeMOperationConfig config2 = new ChequeMOperationConfig
				{
					SanadNo = e.SanadNo,
					DateM = e.DateM,
					ChequeGuid = Guid,
					DebitTafsilGuid = e.TafsilGuid
				};
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ChequeMOperationBusiness.BargashtZadanAsync(ContextGuid, config2), ReturnedState.None, "BedehiTalabReportBusiness_BargashtZadanEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 555);
			}
			if (!res.HasError && eventmavager.OnChangeChequeStatus != null)
			{
				PacketParser.Utilities.RaiseEvent(eventmavager.OnChangeChequeStatus, this, new EventArgs(), default(CancellationToken), 60, "BedehiTalabReportBusiness_BargashtZadanEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BedehiTalabReportBusiness_BargashtZadanEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 562);
			res.AddReturnedValue(ex, "", "BedehiTalabReportBusiness_BargashtZadanEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 563);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> BedehiTalabReportBusiness_BatelKardanEvent(BedehiTalabEventArgs e)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		if (e == null)
		{
			throw new ArgumentNullException("e");
		}
		try
		{
			if (Type == EnBedehiTalabType.CheckSh)
			{
				ChequeShOperationConfig config = new ChequeShOperationConfig
				{
					ChequeGuid = Guid,
					DateM = e.DateM,
					SanadNo = e.SanadNo
				};
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ChequeShOperationBusiness.EbtalAsync(ContextGuid, config), ReturnedState.None, "BedehiTalabReportBusiness_BatelKardanEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 581);
			}
			else if (Type == EnBedehiTalabType.CheckM)
			{
				ChequeMOperationConfig config2 = new ChequeMOperationConfig
				{
					SanadNo = e.SanadNo,
					DateM = e.DateM,
					ChequeGuid = Guid,
					DebitTafsilGuid = e.TafsilGuid
				};
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ChequeMOperationBusiness.EbtalAsync(ContextGuid, config2), ReturnedState.None, "BedehiTalabReportBusiness_BatelKardanEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 592);
			}
			if (!res.HasError && eventmavager.OnChangeChequeStatus != null)
			{
				PacketParser.Utilities.RaiseEvent(eventmavager.OnChangeChequeStatus, this, new EventArgs(), default(CancellationToken), 60, "BedehiTalabReportBusiness_BatelKardanEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BedehiTalabReportBusiness_BatelKardanEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 599);
			res.AddReturnedValue(ex, "", "BedehiTalabReportBusiness_BatelKardanEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 600);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> BedehiTalabReportBusiness_VagozarKardanEvent(BedehiTalabEventArgs e)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		if (e == null)
		{
			throw new ArgumentNullException("e");
		}
		try
		{
			if (Type == EnBedehiTalabType.CheckSh)
			{
				throw new InvalidOperationException();
			}
			ChequeMOperationConfig config = new ChequeMOperationConfig
			{
				SanadNo = e.SanadNo,
				DateM = e.DateM,
				ChequeGuid = Guid,
				DebitTafsilGuid = e.TafsilGuid
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await ChequeMOperationBusiness.VagozarKardanAsync(ContextGuid, config), ReturnedState.None, "BedehiTalabReportBusiness_VagozarKardanEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 618);
			if (!res.HasError && eventmavager.OnChangeChequeStatus != null)
			{
				PacketParser.Utilities.RaiseEvent(eventmavager.OnChangeChequeStatus, this, new EventArgs(), default(CancellationToken), 60, "BedehiTalabReportBusiness_VagozarKardanEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BedehiTalabReportBusiness_VagozarKardanEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 624);
			res.AddReturnedValue(ex, "", "BedehiTalabReportBusiness_VagozarKardanEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 625);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> BedehiTalabReportBusiness_NaghdKardanEvent(BedehiTalabEventArgs e)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		if (e == null)
		{
			throw new ArgumentNullException("e");
		}
		try
		{
			if (Type == EnBedehiTalabType.CheckSh)
			{
				throw new InvalidOperationException();
			}
			ChequeMOperationConfig config = new ChequeMOperationConfig
			{
				SanadNo = e.SanadNo,
				DateM = e.DateM,
				ChequeGuid = Guid,
				DebitTafsilGuid = e.TafsilGuid
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await ChequeMOperationBusiness.NaghdKardanAsync(ContextGuid, config), ReturnedState.None, "BedehiTalabReportBusiness_NaghdKardanEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 643);
			if (!res.HasError && eventmavager.OnChangeChequeStatus != null)
			{
				PacketParser.Utilities.RaiseEvent(eventmavager.OnChangeChequeStatus, this, new EventArgs(), default(CancellationToken), 60, "BedehiTalabReportBusiness_NaghdKardanEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BedehiTalabReportBusiness_NaghdKardanEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 649);
			res.AddReturnedValue(ex, "", "BedehiTalabReportBusiness_NaghdKardanEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 650);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> BedehiTalabReportBusiness_BargashtiToExEvent(BedehiTalabEventArgs e)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		if (e == null)
		{
			throw new ArgumentNullException("e");
		}
		try
		{
			if (Type == EnBedehiTalabType.CheckSh)
			{
				throw new InvalidOperationException();
			}
			ChequeMOperationConfig config = new ChequeMOperationConfig
			{
				SanadNo = e.SanadNo,
				DateM = e.DateM,
				ChequeGuid = Guid,
				DebitTafsilGuid = e.TafsilGuid
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await ChequeMOperationBusiness.BargashtiToBeforeStateAsync(ContextGuid, config), ReturnedState.None, "BedehiTalabReportBusiness_BargashtiToExEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 668);
			if (!res.HasError && eventmavager.OnChangeChequeStatus != null)
			{
				PacketParser.Utilities.RaiseEvent(eventmavager.OnChangeChequeStatus, this, new EventArgs(), default(CancellationToken), 60, "BedehiTalabReportBusiness_BargashtiToExEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BedehiTalabReportBusiness_BargashtiToExEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 674);
			res.AddReturnedValue(ex, "", "BedehiTalabReportBusiness_BargashtiToExEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 675);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> BedehiTalabReportBusiness_OdatBargashtiEvent(BedehiTalabEventArgs e)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		if (e == null)
		{
			throw new ArgumentNullException("e");
		}
		try
		{
			if (Type == EnBedehiTalabType.CheckSh)
			{
				throw new InvalidOperationException();
			}
			ChequeMOperationConfig config = new ChequeMOperationConfig
			{
				SanadNo = e.SanadNo,
				DateM = e.DateM,
				ChequeGuid = Guid,
				DebitTafsilGuid = e.TafsilGuid
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await ChequeMOperationBusiness.OdatBargashtiAsync(ContextGuid, config), ReturnedState.None, "BedehiTalabReportBusiness_OdatBargashtiEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 693);
			if (!res.HasError && eventmavager.OnChangeChequeStatus != null)
			{
				PacketParser.Utilities.RaiseEvent(eventmavager.OnChangeChequeStatus, this, new EventArgs(), default(CancellationToken), 60, "BedehiTalabReportBusiness_OdatBargashtiEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BedehiTalabReportBusiness_OdatBargashtiEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 699);
			res.AddReturnedValue(ex, "", "BedehiTalabReportBusiness_OdatBargashtiEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 700);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> BedehiTalabReportBusiness_OdatKharjShodeEvent(BedehiTalabEventArgs e)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		if (e == null)
		{
			throw new ArgumentNullException("e");
		}
		try
		{
			if (Type == EnBedehiTalabType.CheckSh)
			{
				throw new InvalidOperationException();
			}
			ChequeMOperationConfig config = new ChequeMOperationConfig
			{
				SanadNo = e.SanadNo,
				DateM = e.DateM,
				ChequeGuid = Guid,
				DebitTafsilGuid = e.TafsilGuid
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await ChequeMOperationBusiness.OdatKharjShodeAsync(ContextGuid, config), ReturnedState.None, "BedehiTalabReportBusiness_OdatKharjShodeEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 718);
			if (!res.HasError && eventmavager.OnChangeChequeStatus != null)
			{
				PacketParser.Utilities.RaiseEvent(eventmavager.OnChangeChequeStatus, this, new EventArgs(), default(CancellationToken), 60, "BedehiTalabReportBusiness_OdatKharjShodeEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BedehiTalabReportBusiness_OdatKharjShodeEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 724);
			res.AddReturnedValue(ex, "", "BedehiTalabReportBusiness_OdatKharjShodeEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 725);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> BedehiTalabReportBusiness_VosolKardanEvent(BedehiTalabEventArgs e)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		if (e == null)
		{
			throw new ArgumentNullException("e");
		}
		try
		{
			if (Type == EnBedehiTalabType.CheckSh)
			{
				throw new InvalidOperationException();
			}
			ChequeMOperationConfig config = new ChequeMOperationConfig
			{
				SanadNo = e.SanadNo,
				DateM = e.DateM,
				ChequeGuid = Guid,
				DebitTafsilGuid = e.TafsilGuid
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await ChequeMOperationBusiness.VosolKardanAsync(ContextGuid, config), ReturnedState.None, "BedehiTalabReportBusiness_VosolKardanEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 743);
			if (!res.HasError && eventmavager.OnChangeChequeStatus != null)
			{
				PacketParser.Utilities.RaiseEvent(eventmavager.OnChangeChequeStatus, this, new EventArgs(), default(CancellationToken), 60, "BedehiTalabReportBusiness_VosolKardanEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BedehiTalabReportBusiness_VosolKardanEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 749);
			res.AddReturnedValue(ex, "", "BedehiTalabReportBusiness_VosolKardanEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 750);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> BedehiTalabReportBusiness_VagozarToMojodEvent(BedehiTalabEventArgs e)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		if (e == null)
		{
			throw new ArgumentNullException("e");
		}
		try
		{
			if (Type == EnBedehiTalabType.CheckSh)
			{
				throw new InvalidOperationException();
			}
			ChequeMOperationConfig config = new ChequeMOperationConfig
			{
				SanadNo = e.SanadNo,
				DateM = e.DateM,
				ChequeGuid = Guid,
				DebitTafsilGuid = e.TafsilGuid
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await ChequeMOperationBusiness.VagozarToMojodAsync(ContextGuid, config), ReturnedState.None, "BedehiTalabReportBusiness_VagozarToMojodEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 768);
			if (!res.HasError && eventmavager.OnChangeChequeStatus != null)
			{
				PacketParser.Utilities.RaiseEvent(eventmavager.OnChangeChequeStatus, this, new EventArgs(), default(CancellationToken), 60, "BedehiTalabReportBusiness_VagozarToMojodEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BedehiTalabReportBusiness_VagozarToMojodEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 774);
			res.AddReturnedValue(ex, "", "BedehiTalabReportBusiness_VagozarToMojodEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 775);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> BedehiTalabReportBusiness_NaghdToMojodEvent(BedehiTalabEventArgs e)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		if (e == null)
		{
			throw new ArgumentNullException("e");
		}
		try
		{
			if (Type == EnBedehiTalabType.CheckSh)
			{
				throw new InvalidOperationException();
			}
			ChequeMOperationConfig config = new ChequeMOperationConfig
			{
				SanadNo = e.SanadNo,
				DateM = e.DateM,
				ChequeGuid = Guid,
				DebitTafsilGuid = e.TafsilGuid
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await ChequeMOperationBusiness.NaghdToMojodAsync(ContextGuid, config), ReturnedState.None, "BedehiTalabReportBusiness_NaghdToMojodEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 793);
			if (!res.HasError && eventmavager.OnChangeChequeStatus != null)
			{
				PacketParser.Utilities.RaiseEvent(eventmavager.OnChangeChequeStatus, this, new EventArgs(), default(CancellationToken), 60, "BedehiTalabReportBusiness_NaghdToMojodEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BedehiTalabReportBusiness_NaghdToMojodEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 799);
			res.AddReturnedValue(ex, "", "BedehiTalabReportBusiness_NaghdToMojodEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 800);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> BedehiTalabReportBusiness_BatelToMojodEvent(BedehiTalabEventArgs e)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		if (e == null)
		{
			throw new ArgumentNullException("e");
		}
		try
		{
			if (Type == EnBedehiTalabType.CheckSh)
			{
				throw new InvalidOperationException();
			}
			ChequeMOperationConfig config = new ChequeMOperationConfig
			{
				SanadNo = e.SanadNo,
				DateM = e.DateM,
				ChequeGuid = Guid,
				DebitTafsilGuid = e.TafsilGuid
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await ChequeMOperationBusiness.BatelToMojodAsync(ContextGuid, config), ReturnedState.None, "BedehiTalabReportBusiness_BatelToMojodEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 818);
			if (!res.HasError && eventmavager.OnChangeChequeStatus != null)
			{
				PacketParser.Utilities.RaiseEvent(eventmavager.OnChangeChequeStatus, this, new EventArgs(), default(CancellationToken), 60, "BedehiTalabReportBusiness_BatelToMojodEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BedehiTalabReportBusiness_BatelToMojodEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 824);
			res.AddReturnedValue(ex, "", "BedehiTalabReportBusiness_BatelToMojodEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 825);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> BedehiTalabReportBusiness_VosolToVagozarEvent(BedehiTalabEventArgs e)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		if (e == null)
		{
			throw new ArgumentNullException("e");
		}
		try
		{
			if (Type == EnBedehiTalabType.CheckSh)
			{
				throw new InvalidOperationException();
			}
			ChequeMOperationConfig config = new ChequeMOperationConfig
			{
				SanadNo = e.SanadNo,
				DateM = e.DateM,
				ChequeGuid = Guid,
				DebitTafsilGuid = e.TafsilGuid
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await ChequeMOperationBusiness.VosolToVagozarAsync(ContextGuid, config, 0), ReturnedState.None, "BedehiTalabReportBusiness_VosolToVagozarEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 843);
			if (!res.HasError && eventmavager.OnChangeChequeStatus != null)
			{
				PacketParser.Utilities.RaiseEvent(eventmavager.OnChangeChequeStatus, this, new EventArgs(), default(CancellationToken), 60, "BedehiTalabReportBusiness_VosolToVagozarEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BedehiTalabReportBusiness_VosolToVagozarEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 849);
			res.AddReturnedValue(ex, "", "BedehiTalabReportBusiness_VosolToVagozarEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 850);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> BedehiTalabReportBusiness_ReceptionEntezamiOdatEvent(BedehiTalabEventArgs e)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		if (e == null)
		{
			throw new ArgumentNullException("e");
		}
		try
		{
			if (Type == EnBedehiTalabType.CheckSh)
			{
				throw new InvalidOperationException();
			}
			ChequeMOperationConfig config = new ChequeMOperationConfig
			{
				SanadNo = e.SanadNo,
				DateM = e.DateM,
				ChequeGuid = Guid
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await ChequeMOperationBusiness.OdatEntezamiAsync(ContextGuid, config), ReturnedState.None, "BedehiTalabReportBusiness_ReceptionEntezamiOdatEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 867);
			if (!res.HasError && eventmavager.OnChangeChequeStatus != null)
			{
				PacketParser.Utilities.RaiseEvent(eventmavager.OnChangeChequeStatus, this, new EventArgs(), default(CancellationToken), 60, "BedehiTalabReportBusiness_ReceptionEntezamiOdatEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BedehiTalabReportBusiness_ReceptionEntezamiOdatEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 873);
			res.AddReturnedValue(ex, "", "BedehiTalabReportBusiness_ReceptionEntezamiOdatEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 874);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> BedehiTalabReportBusiness_ReceptionEntezamiOdatToMojoEvent(BedehiTalabEventArgs e)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		if (e == null)
		{
			throw new ArgumentNullException("e");
		}
		try
		{
			if (Type == EnBedehiTalabType.CheckSh)
			{
				throw new InvalidOperationException();
			}
			ChequeMOperationConfig config = new ChequeMOperationConfig
			{
				SanadNo = e.SanadNo,
				DateM = e.DateM,
				ChequeGuid = Guid
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await ChequeMOperationBusiness.OdatEntezamiToMojodAsync(ContextGuid, config), ReturnedState.None, "BedehiTalabReportBusiness_ReceptionEntezamiOdatToMojoEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 891);
			if (!res.HasError && eventmavager.OnChangeChequeStatus != null)
			{
				PacketParser.Utilities.RaiseEvent(eventmavager.OnChangeChequeStatus, this, new EventArgs(), default(CancellationToken), 60, "BedehiTalabReportBusiness_ReceptionEntezamiOdatToMojoEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BedehiTalabReportBusiness_ReceptionEntezamiOdatToMojoEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 897);
			res.AddReturnedValue(ex, "", "BedehiTalabReportBusiness_ReceptionEntezamiOdatToMojoEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 898);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> BedehiTalabReportBusiness_PardakhtShodeToPardakhtaniEvent(BedehiTalabEventArgs e)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		if (e == null)
		{
			throw new ArgumentNullException("e");
		}
		try
		{
			if (Type == EnBedehiTalabType.CheckM)
			{
				throw new InvalidOperationException();
			}
			ChequeShOperationConfig config = new ChequeShOperationConfig
			{
				ChequeGuid = Guid,
				DateM = e.DateM,
				SanadNo = e.SanadNo
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await ChequeShOperationBusiness.PardakhtShodeToPardakhtaniAsync(ContextGuid, config), ReturnedState.None, "BedehiTalabReportBusiness_PardakhtShodeToPardakhtaniEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 915);
			if (!res.HasError && eventmavager.OnChangeChequeStatus != null)
			{
				PacketParser.Utilities.RaiseEvent(eventmavager.OnChangeChequeStatus, this, new EventArgs(), default(CancellationToken), 60, "BedehiTalabReportBusiness_PardakhtShodeToPardakhtaniEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BedehiTalabReportBusiness_PardakhtShodeToPardakhtaniEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 921);
			res.AddReturnedValue(ex, "", "BedehiTalabReportBusiness_PardakhtShodeToPardakhtaniEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 922);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> BedehiTalabReportBusiness_ElamPardakhtEvent(BedehiTalabEventArgs e)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		if (e == null)
		{
			throw new ArgumentNullException("e");
		}
		try
		{
			if (Type == EnBedehiTalabType.CheckM)
			{
				throw new InvalidOperationException();
			}
			ChequeShOperationConfig config = new ChequeShOperationConfig
			{
				ChequeGuid = Guid,
				DateM = e.DateM,
				SanadNo = e.SanadNo
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await ChequeShOperationBusiness.ElamPardakhtAsync(ContextGuid, config), ReturnedState.None, "BedehiTalabReportBusiness_ElamPardakhtEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 939);
			if (!res.HasError && eventmavager.OnChangeChequeStatus != null)
			{
				PacketParser.Utilities.RaiseEvent(eventmavager.OnChangeChequeStatus, this, new EventArgs(), default(CancellationToken), 60, "BedehiTalabReportBusiness_ElamPardakhtEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BedehiTalabReportBusiness_ElamPardakhtEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 945);
			res.AddReturnedValue(ex, "", "BedehiTalabReportBusiness_ElamPardakhtEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 946);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> BedehiTalabReportBusiness_BatelShodeToPardakhtaniEvent(BedehiTalabEventArgs e)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		if (e == null)
		{
			throw new ArgumentNullException("e");
		}
		try
		{
			if (Type == EnBedehiTalabType.CheckM)
			{
				throw new InvalidOperationException();
			}
			ChequeShOperationConfig config = new ChequeShOperationConfig
			{
				ChequeGuid = Guid,
				DateM = e.DateM,
				SanadNo = e.SanadNo
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await ChequeShOperationBusiness.EbtalToPardakhtaniAsync(ContextGuid, config), ReturnedState.None, "BedehiTalabReportBusiness_BatelShodeToPardakhtaniEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 963);
			if (!res.HasError && eventmavager.OnChangeChequeStatus != null)
			{
				PacketParser.Utilities.RaiseEvent(eventmavager.OnChangeChequeStatus, this, new EventArgs(), default(CancellationToken), 60, "BedehiTalabReportBusiness_BatelShodeToPardakhtaniEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BedehiTalabReportBusiness_BatelShodeToPardakhtaniEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 969);
			res.AddReturnedValue(ex, "", "BedehiTalabReportBusiness_BatelShodeToPardakhtaniEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 970);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> BedehiTalabReportBusiness_BargashtiToPardakhtaniEvent(BedehiTalabEventArgs e)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		if (e == null)
		{
			throw new ArgumentNullException("e");
		}
		try
		{
			if (Type == EnBedehiTalabType.CheckM)
			{
				throw new InvalidOperationException();
			}
			ChequeShOperationConfig config = new ChequeShOperationConfig
			{
				ChequeGuid = Guid,
				DateM = e.DateM,
				SanadNo = e.SanadNo
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await ChequeShOperationBusiness.BargashtiToPardakhtaniAsync(ContextGuid, config), ReturnedState.None, "BedehiTalabReportBusiness_BargashtiToPardakhtaniEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 987);
			if (!res.HasError && eventmavager.OnChangeChequeStatus != null)
			{
				PacketParser.Utilities.RaiseEvent(eventmavager.OnChangeChequeStatus, this, new EventArgs(), default(CancellationToken), 60, "BedehiTalabReportBusiness_BargashtiToPardakhtaniEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BedehiTalabReportBusiness_BargashtiToPardakhtaniEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 993);
			res.AddReturnedValue(ex, "", "BedehiTalabReportBusiness_BargashtiToPardakhtaniEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 994);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> BedehiTalabReportBusiness_PardakhtEntezamiOdatEvent(BedehiTalabEventArgs e)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		if (e == null)
		{
			throw new ArgumentNullException("e");
		}
		try
		{
			if (Type == EnBedehiTalabType.CheckM)
			{
				throw new InvalidOperationException();
			}
			ChequeShOperationConfig config = new ChequeShOperationConfig
			{
				ChequeGuid = Guid,
				DateM = e.DateM,
				SanadNo = e.SanadNo
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await ChequeShOperationBusiness.OdatEntezamiAsync(ContextGuid, config), ReturnedState.None, "BedehiTalabReportBusiness_PardakhtEntezamiOdatEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 1011);
			if (!res.HasError && eventmavager.OnChangeChequeStatus != null)
			{
				PacketParser.Utilities.RaiseEvent(eventmavager.OnChangeChequeStatus, this, new EventArgs(), default(CancellationToken), 60, "BedehiTalabReportBusiness_PardakhtEntezamiOdatEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BedehiTalabReportBusiness_PardakhtEntezamiOdatEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 1017);
			res.AddReturnedValue(ex, "", "BedehiTalabReportBusiness_PardakhtEntezamiOdatEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 1018);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> BedehiTalabReportBusiness_PardakhtEntezamiOdatToMojodEvent(BedehiTalabEventArgs e)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		if (e == null)
		{
			throw new ArgumentNullException("e");
		}
		try
		{
			if (Type == EnBedehiTalabType.CheckM)
			{
				throw new InvalidOperationException();
			}
			ChequeShOperationConfig config = new ChequeShOperationConfig
			{
				ChequeGuid = Guid,
				DateM = e.DateM,
				SanadNo = e.SanadNo
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await ChequeShOperationBusiness.OdatToPardakhtaniEntezamiAsync(ContextGuid, config), ReturnedState.None, "BedehiTalabReportBusiness_PardakhtEntezamiOdatToMojodEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 1035);
			if (!res.HasError && eventmavager.OnChangeChequeStatus != null)
			{
				PacketParser.Utilities.RaiseEvent(eventmavager.OnChangeChequeStatus, this, new EventArgs(), default(CancellationToken), 60, "BedehiTalabReportBusiness_PardakhtEntezamiOdatToMojodEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BedehiTalabReportBusiness_PardakhtEntezamiOdatToMojodEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 1041);
			res.AddReturnedValue(ex, "", "BedehiTalabReportBusiness_PardakhtEntezamiOdatToMojodEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs", 1042);
		}
		return res;
	}

	public async Task<string> GetTafsifNameAsync(CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("GetTafsifNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs"))
		{
			try
			{
				if (TafsilName != null)
				{
					return TafsilName;
				}
				if (TafsilGuid == Guid.Empty)
				{
					return "";
				}
				return TafsilName = (await TafsilBusiness.GetAsync(ContextGuid, EnSystem.UserInterface, TafsilGuid, "", token, "GetTafsifNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Cheque\\BedehiTalabReportBusiness.cs"))?.Name ?? "";
			}
			catch (Exception ex)
			{
				Log.Error(ex, "");
				return "";
			}
		}
	}
}
