using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Core;
using EntityCache.Core;
using EntityCache.Interfaces.Sync;
using EntityCache.Mapping.BusinessMappers.Accounting;
using EntityCache.Utilities;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using PacketParser.Entities.MetaData;
using PacketParser.Entities.SerializeData;
using PacketParser.EventArgs;
using PacketParser.TransferLayer.Waiters.Accounting.Hesab;

namespace EntityCache.Business.Accounting;

public class TafsilGroupBusiness : BaseBusiness<TafsilGroupBusiness>, ITafsilGroup, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, ISyncFromClient2Server<TafsilGroupBusiness>, IDeliverableEntity, IEditableDelivery, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private Guid moeinPardakhtGuid;

	private Guid moeinGuid;

	private Guid moeinSellGuid;

	private Guid moeinBuyGuid;

	private Guid moeinBackSellGuid;

	private Guid moeinBackBuyGuid;

	private Guid moeinReceptionGuid;

	private Guid moeinAmaniMaNazdDigaranGuid;

	private Guid moeinAmaniDigaranNazdMaGuid;

	private SemaphoreSlim _sendToServerSemaphore = new SemaphoreSlim(1, 1);

	private CancellationTokenSource _sendToken = new CancellationTokenSource();

	private List<Guid> _moeinGuids;

	public override IDatabaseManager DatabaseManager => ClsCache.GetCache(ContextGuid).AccDatabaseManager;

	public override string ContextGuid { get; set; }

	public Guid RandomGuid => Guid.NewGuid();

	public Guid Guid { get; set; } = Guid.NewGuid();


	public short Id { get; set; }

	public DefualtBazaryabGuidInTafsilGroup BazaryabDefaultGuid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public string Name { get; set; }

	public string Tbl { get; set; }

	public string Description { get; set; } = "";


	public long CodeMin { get; set; }

	public long CodeMax { get; set; }

	public bool Status { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public byte[] ServerStatusImage
	{
		get
		{
			try
			{
				ServerStatusForClient serverStatus = ServerStatus;
				if ((serverStatus == ServerStatusForClient.Delivered || serverStatus == ServerStatusForClient.DirectDelivery) ? true : false)
				{
					return ImageResourceManager.ServerStatusDelivered;
				}
				if (ServerStatus == ServerStatusForClient.DeliveryError)
				{
					return ImageResourceManager.ServerStatusDeliveryFailed;
				}
				if (ServerStatus == ServerStatusForClient.Sent)
				{
					return ImageResourceManager.ServerStatusSent;
				}
				if (ServerStatus == ServerStatusForClient.SendError)
				{
					return ImageResourceManager.ServerStatusSentError;
				}
				return ImageResourceManager.ServerStatusNone;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ServerStatusImage", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 64);
			}
			return null;
		}
	}

	public Guid MoeinSellGuid
	{
		get
		{
			try
			{
				if (moeinSellGuid == Guid.Empty || moeinSellGuid == ParentDefaults.MoeinCoding.CLSMoein16)
				{
					moeinSellGuid = ParentDefaults.MoeinCoding.CLSMoein9;
				}
				return moeinSellGuid;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MoeinSellGuid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 78);
			}
			return Guid.Empty;
		}
		set
		{
			moeinSellGuid = value;
		}
	}

	public Guid MoeinBuyGuid
	{
		get
		{
			try
			{
				if (moeinBuyGuid == Guid.Empty || moeinBuyGuid == ParentDefaults.MoeinCoding.CLSMoein17)
				{
					moeinBuyGuid = ParentDefaults.MoeinCoding.CLSMoein9;
				}
				return moeinBuyGuid;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MoeinBuyGuid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 93);
			}
			return Guid.Empty;
		}
		set
		{
			moeinBuyGuid = value;
		}
	}

	public Guid MoeinBackSellGuid
	{
		get
		{
			try
			{
				if (moeinBackSellGuid == Guid.Empty || moeinBackSellGuid == ParentDefaults.MoeinCoding.CLSMoein32)
				{
					moeinBackSellGuid = ParentDefaults.MoeinCoding.CLSMoein9;
				}
				return moeinBackSellGuid;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MoeinBackSellGuid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 108);
			}
			return Guid.Empty;
		}
		set
		{
			moeinBackSellGuid = value;
		}
	}

	public Guid MoeinBackBuyGuid
	{
		get
		{
			try
			{
				if (moeinBackBuyGuid == Guid.Empty || moeinBackBuyGuid == ParentDefaults.MoeinCoding.CLSMoein33)
				{
					moeinBackBuyGuid = ParentDefaults.MoeinCoding.CLSMoein9;
				}
				return moeinBackBuyGuid;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MoeinBackBuyGuid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 123);
			}
			return Guid.Empty;
		}
		set
		{
			moeinBackBuyGuid = value;
		}
	}

	public Guid MoeinReceptionGuid
	{
		get
		{
			try
			{
				if (moeinReceptionGuid == Guid.Empty)
				{
					moeinReceptionGuid = ParentDefaults.MoeinCoding.CLSMoein9;
				}
				return moeinReceptionGuid;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MoeinReceptionGuid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 138);
			}
			return Guid.Empty;
		}
		set
		{
			moeinReceptionGuid = value;
		}
	}

	public Guid MoeinPardakhtGuid
	{
		get
		{
			try
			{
				if (moeinPardakhtGuid == Guid.Empty)
				{
					moeinPardakhtGuid = ParentDefaults.MoeinCoding.CLSMoein9;
				}
				return moeinPardakhtGuid;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MoeinPardakhtGuid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 154);
			}
			return Guid.Empty;
		}
		set
		{
			moeinPardakhtGuid = value;
		}
	}

	public Guid MoeinAmaniMaNazdDigaranGuid
	{
		get
		{
			try
			{
				if (moeinAmaniMaNazdDigaranGuid == Guid.Empty || moeinAmaniMaNazdDigaranGuid == ParentDefaults.MoeinCoding.CLSMoein29957)
				{
					moeinAmaniMaNazdDigaranGuid = ParentDefaults.MoeinCoding.CLSMoein29995;
				}
				return moeinAmaniMaNazdDigaranGuid;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MoeinAmaniMaNazdDigaranGuid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 169);
			}
			return Guid.Empty;
		}
		set
		{
			moeinAmaniMaNazdDigaranGuid = value;
		}
	}

	public Guid MoeinAmaniDigaranNazdMaGuid
	{
		get
		{
			try
			{
				if (moeinAmaniDigaranNazdMaGuid == Guid.Empty || moeinAmaniDigaranNazdMaGuid == ParentDefaults.MoeinCoding.CLSMoein29860)
				{
					moeinAmaniDigaranNazdMaGuid = ParentDefaults.MoeinCoding.CLSMoein29994;
				}
				return moeinAmaniDigaranNazdMaGuid;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MoeinAmaniDigaranNazdMaGuid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 184);
			}
			return Guid.Empty;
		}
		set
		{
			moeinAmaniDigaranNazdMaGuid = value;
		}
	}

	public Guid MoeinBazaryabSell1
	{
		get
		{
			try
			{
				if (BazaryabDefaultGuid == null)
				{
					DefualtBazaryabGuidInTafsilGroup defualtBazaryabGuidInTafsilGroup2 = (BazaryabDefaultGuid = new DefualtBazaryabGuidInTafsilGroup());
				}
				if (BazaryabDefaultGuid.VisitorSell1 == Guid.Empty)
				{
					BazaryabDefaultGuid.VisitorSell1 = ParentDefaults.MoeinCoding.CLSMoein9;
				}
				return BazaryabDefaultGuid.VisitorSell1;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MoeinBazaryabSell1", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 200);
			}
			return Guid.Empty;
		}
		set
		{
			try
			{
				if (BazaryabDefaultGuid == null)
				{
					DefualtBazaryabGuidInTafsilGroup defualtBazaryabGuidInTafsilGroup2 = (BazaryabDefaultGuid = new DefualtBazaryabGuidInTafsilGroup());
				}
				BazaryabDefaultGuid.VisitorSell1 = value;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MoeinBazaryabSell1", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 210);
			}
		}
	}

	public Guid MoeinBazaryabSell2
	{
		get
		{
			try
			{
				if (BazaryabDefaultGuid == null)
				{
					DefualtBazaryabGuidInTafsilGroup defualtBazaryabGuidInTafsilGroup2 = (BazaryabDefaultGuid = new DefualtBazaryabGuidInTafsilGroup());
				}
				if (BazaryabDefaultGuid.VisitorSell2 == Guid.Empty)
				{
					BazaryabDefaultGuid.VisitorSell2 = ParentDefaults.MoeinCoding.CLSMoein9;
				}
				return BazaryabDefaultGuid.VisitorSell2;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MoeinBazaryabSell2", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 224);
			}
			return Guid.Empty;
		}
		set
		{
			try
			{
				if (BazaryabDefaultGuid == null)
				{
					DefualtBazaryabGuidInTafsilGroup defualtBazaryabGuidInTafsilGroup2 = (BazaryabDefaultGuid = new DefualtBazaryabGuidInTafsilGroup());
				}
				BazaryabDefaultGuid.VisitorSell2 = value;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MoeinBazaryabSell2", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 234);
			}
		}
	}

	public Guid MoeinBazaryabSell3
	{
		get
		{
			try
			{
				if (BazaryabDefaultGuid == null)
				{
					DefualtBazaryabGuidInTafsilGroup defualtBazaryabGuidInTafsilGroup2 = (BazaryabDefaultGuid = new DefualtBazaryabGuidInTafsilGroup());
				}
				if (BazaryabDefaultGuid.VisitorSell3 == Guid.Empty)
				{
					BazaryabDefaultGuid.VisitorSell3 = ParentDefaults.MoeinCoding.CLSMoein9;
				}
				return BazaryabDefaultGuid.VisitorSell3;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MoeinBazaryabSell3", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 248);
			}
			return Guid.Empty;
		}
		set
		{
			try
			{
				if (BazaryabDefaultGuid == null)
				{
					DefualtBazaryabGuidInTafsilGroup defualtBazaryabGuidInTafsilGroup2 = (BazaryabDefaultGuid = new DefualtBazaryabGuidInTafsilGroup());
				}
				BazaryabDefaultGuid.VisitorSell3 = value;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MoeinBazaryabSell3", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 258);
			}
		}
	}

	public Guid MoeinBazaryabSell4
	{
		get
		{
			try
			{
				if (BazaryabDefaultGuid == null)
				{
					DefualtBazaryabGuidInTafsilGroup defualtBazaryabGuidInTafsilGroup2 = (BazaryabDefaultGuid = new DefualtBazaryabGuidInTafsilGroup());
				}
				if (BazaryabDefaultGuid.VisitorSell4 == Guid.Empty)
				{
					BazaryabDefaultGuid.VisitorSell4 = ParentDefaults.MoeinCoding.CLSMoein9;
				}
				return BazaryabDefaultGuid.VisitorSell4;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MoeinBazaryabSell4", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 272);
			}
			return Guid.Empty;
		}
		set
		{
			try
			{
				if (BazaryabDefaultGuid == null)
				{
					DefualtBazaryabGuidInTafsilGroup defualtBazaryabGuidInTafsilGroup2 = (BazaryabDefaultGuid = new DefualtBazaryabGuidInTafsilGroup());
				}
				BazaryabDefaultGuid.VisitorSell4 = value;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MoeinBazaryabSell4", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 282);
			}
		}
	}

	public Guid MoeinBazaryabBackSell1
	{
		get
		{
			try
			{
				if (BazaryabDefaultGuid == null)
				{
					DefualtBazaryabGuidInTafsilGroup defualtBazaryabGuidInTafsilGroup2 = (BazaryabDefaultGuid = new DefualtBazaryabGuidInTafsilGroup());
				}
				if (BazaryabDefaultGuid.VisitorBackSell1 == Guid.Empty)
				{
					BazaryabDefaultGuid.VisitorBackSell1 = ParentDefaults.MoeinCoding.CLSMoein9;
				}
				return BazaryabDefaultGuid.VisitorBackSell1;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MoeinBazaryabBackSell1", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 296);
			}
			return Guid.Empty;
		}
		set
		{
			try
			{
				if (BazaryabDefaultGuid == null)
				{
					DefualtBazaryabGuidInTafsilGroup defualtBazaryabGuidInTafsilGroup2 = (BazaryabDefaultGuid = new DefualtBazaryabGuidInTafsilGroup());
				}
				BazaryabDefaultGuid.VisitorBackSell1 = value;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MoeinBazaryabBackSell1", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 306);
			}
		}
	}

	public Guid MoeinBazaryabBackSell2
	{
		get
		{
			try
			{
				if (BazaryabDefaultGuid == null)
				{
					DefualtBazaryabGuidInTafsilGroup defualtBazaryabGuidInTafsilGroup2 = (BazaryabDefaultGuid = new DefualtBazaryabGuidInTafsilGroup());
				}
				if (BazaryabDefaultGuid.VisitorBackSell2 == Guid.Empty)
				{
					BazaryabDefaultGuid.VisitorBackSell2 = ParentDefaults.MoeinCoding.CLSMoein9;
				}
				return BazaryabDefaultGuid.VisitorBackSell2;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MoeinBazaryabBackSell2", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 320);
			}
			return Guid.Empty;
		}
		set
		{
			try
			{
				if (BazaryabDefaultGuid == null)
				{
					DefualtBazaryabGuidInTafsilGroup defualtBazaryabGuidInTafsilGroup2 = (BazaryabDefaultGuid = new DefualtBazaryabGuidInTafsilGroup());
				}
				BazaryabDefaultGuid.VisitorBackSell2 = value;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MoeinBazaryabBackSell2", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 330);
			}
		}
	}

	public Guid MoeinBazaryabBackSell3
	{
		get
		{
			try
			{
				if (BazaryabDefaultGuid == null)
				{
					DefualtBazaryabGuidInTafsilGroup defualtBazaryabGuidInTafsilGroup2 = (BazaryabDefaultGuid = new DefualtBazaryabGuidInTafsilGroup());
				}
				if (BazaryabDefaultGuid.VisitorBackSell3 == Guid.Empty)
				{
					BazaryabDefaultGuid.VisitorBackSell3 = ParentDefaults.MoeinCoding.CLSMoein9;
				}
				return BazaryabDefaultGuid.VisitorBackSell3;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MoeinBazaryabBackSell3", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 344);
			}
			return Guid.Empty;
		}
		set
		{
			try
			{
				if (BazaryabDefaultGuid == null)
				{
					DefualtBazaryabGuidInTafsilGroup defualtBazaryabGuidInTafsilGroup2 = (BazaryabDefaultGuid = new DefualtBazaryabGuidInTafsilGroup());
				}
				BazaryabDefaultGuid.VisitorBackSell3 = value;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MoeinBazaryabBackSell3", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 355);
			}
		}
	}

	public Guid MoeinBazaryabBackSell4
	{
		get
		{
			try
			{
				if (BazaryabDefaultGuid == null)
				{
					DefualtBazaryabGuidInTafsilGroup defualtBazaryabGuidInTafsilGroup2 = (BazaryabDefaultGuid = new DefualtBazaryabGuidInTafsilGroup());
				}
				if (BazaryabDefaultGuid.VisitorBackSell4 == Guid.Empty)
				{
					BazaryabDefaultGuid.VisitorBackSell4 = ParentDefaults.MoeinCoding.CLSMoein9;
				}
				return BazaryabDefaultGuid.VisitorBackSell4;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MoeinBazaryabBackSell4", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 369);
			}
			return Guid.Empty;
		}
		set
		{
			try
			{
				if (BazaryabDefaultGuid == null)
				{
					DefualtBazaryabGuidInTafsilGroup defualtBazaryabGuidInTafsilGroup2 = (BazaryabDefaultGuid = new DefualtBazaryabGuidInTafsilGroup());
				}
				BazaryabDefaultGuid.VisitorBackSell4 = value;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MoeinBazaryabBackSell4", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 379);
			}
		}
	}

	[JsonIgnore]
	public bool Checked { get; set; }

	[JsonIgnore]
	public bool IsChecked { get; set; }

	public EnTafsilGroupType Type { get; set; }

	public TafsilGroupMetaData MetaData { get; set; }

	public TafsilGroupBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public async Task<Guid> GetMoeinGuidAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (moeinGuid == Guid.Empty)
			{
				string contextGuid = ContextGuid;
				Guid? tafsilLevelGuid = Guid;
				CancellationToken? token2 = token;
				moeinGuid = (await MoeinBusiness.GetAllAsync(contextGuid, "", null, tafsilLevelGuid, token2, true, tranName))?.FirstOrDefault()?.Guid ?? Guid.Empty;
				if (moeinGuid == Guid.Empty)
				{
					switch (Tbl)
					{
					case "HAZ":
						moeinGuid = ParentDefaults.MoeinCoding.CLSMoein11;
						break;
					case "BNK":
						moeinGuid = ParentDefaults.MoeinCoding.CLSMoein4;
						break;
					case "SAN":
						moeinGuid = ParentDefaults.MoeinCoding.CLSMoein7;
						break;
					case "BAR":
						moeinGuid = ParentDefaults.MoeinCoding.CLSMoein3;
						break;
					default:
						moeinGuid = ParentDefaults.MoeinCoding.CLSMoein9;
						break;
					}
				}
			}
			return moeinGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetMoeinGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 409);
		}
		return Guid.Empty;
	}

	public async Task<List<Guid>> GetMoeinGuidListAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (_moeinGuids?.Any() ?? false)
			{
				return _moeinGuids;
			}
			string contextGuid = ContextGuid;
			Guid? tafsilLevelGuid = Guid;
			CancellationToken? token2 = token;
			_moeinGuids = (await MoeinBusiness.GetAllAsync(contextGuid, "", null, tafsilLevelGuid, token2, true, tranName))?.Select((MoeinBusiness q) => q.Guid)?.ToList();
			return _moeinGuids;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetMoeinGuidListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 423);
			throw;
		}
	}

	public static Task<IEnumerable<TafsilGroupBusiness>> GetAllAsync(string contextGuid, object searchString, CancellationToken token, bool status)
	{
		throw new NotImplementedException();
	}

	public void SetMoeinGuid(Guid value)
	{
		try
		{
			moeinGuid = value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetMoeinGuid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 434);
		}
	}

	public static async Task<IEnumerable<TafsilGroupBusiness>> GetAllAsync(string contextGuid, EnSystem system, bool? status = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			IEnumerable<TafsilGroupBusiness> enumerable = await ClsCache.GetCache(contextGuid).CacheRepository.ITafsilGroup.GetAllAsync(token, tranName);
			if (status.HasValue)
			{
				enumerable = enumerable.Where((TafsilGroupBusiness x) => x.Status == status);
			}
			return (await ApplyAccessAsync(contextGuid, enumerable, system, token, tranName))?.OrderBy((TafsilGroupBusiness q) => q.Name);
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 445);
		}
		return null;
	}

	public static async Task<TafsilGroupBusiness> GetAsync(string contextGuid, Guid guid, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			return await ApplyAccessAsync(contextGuid, await (ClsCache.GetCache(contextGuid).CacheRepository.ITafsilGroup?.GetAsync(guid, token, tranName)), system, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 457);
			throw;
		}
	}

	public static Task<ReturnedSaveFuncInfoWithValue<List<TafsilGroupBusiness>>> IsDuplicateCodeAsync(string contextGuid, long minCode, long maxCode, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ITafsilGroup.IsDuplicateCodeAsync(minCode, maxCode, guid, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, CancellationToken token, string tranName, bool isRemoveWithGuid, short autoFixTryCount, List<Guid> listMoeinGuid)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			TafsilGroupBusiness tafsilGroupBusiness = await GetAsync(ContextGuid, Name, system, token, tranName);
			bool flag = tafsilGroupBusiness != null && tafsilGroupBusiness.Guid != Guid;
			if (isRemoveWithGuid && flag)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await tafsilGroupBusiness.RemovePhysicallyAsync(token, awaitOnSemaphore: true, 10), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 477);
				return ret;
			}
			if (Guid == Guid.Empty)
			{
				ret.AddError(system, "شناسه تهی".GetTranslate(), "Guid", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 482);
			}
			if (DeliveryDate.HasValue && DeliveryDate.Value.Year < 2000)
			{
				DeliveryDate = null;
			}
			if (string.IsNullOrEmpty(Name))
			{
				ret.AddError(system, "گروه حساب تفصیلی فاقد نام قابل ثبت نمی باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 488);
			}
			if (Status && CodeMax < CodeMin)
			{
				ret.AddError(system, "بازه کدینگ نامعتبر است".GetTranslate() + "(" + $"{CodeMin}".GetTranslate() + "-" + $"{CodeMax}".GetTranslate() + ") (" + (Name ?? "").GetTranslate() + ")", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 490);
			}
			if (Status && CodeMax == CodeMin)
			{
				ret.AddError(system, "بازه کدینگ نامعتبر است".GetTranslate() + "(" + $"{CodeMin}".GetTranslate() + "-" + $"{CodeMax}".GetTranslate() + ")(" + (Name ?? "").GetTranslate() + ")", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 491);
			}
			if (Status && CodeMin <= 0)
			{
				ret.AddError(system, "بازه کدینگ نامعتبر است".GetTranslate() + "(" + $"{CodeMin}".GetTranslate() + "-" + $"{CodeMax}".GetTranslate() + ") (" + (Name ?? "").GetTranslate() + ")", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 492);
			}
			checked
			{
				if (flag)
				{
					if (autoFixTryCount > 0 && system != EnSystem.UserInterface)
					{
						Name += "_";
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						TafsilGroupBusiness tafsilGroupBusiness2 = this;
						short num = (short)(autoFixTryCount - 1);
						autoFixTryCount = num;
						returnedSaveFuncInfo.AddReturnedValue(await tafsilGroupBusiness2.CheckValidationAsync(system, token, tranName, isRemoveWithGuid, num, listMoeinGuid), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 499);
						return ret;
					}
					ret.AddError(system, "نام گروه تفصیلی".GetTranslate() + "(\"" + Name + "\")" + " تکراری می باشد".GetTranslate() + "\r\n" + $"CurrentGuid:{Guid}\r\n" + $"existsGuid:{tafsilGroupBusiness.Guid}", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 503);
				}
				ReturnedSaveFuncInfoWithValue<List<TafsilGroupBusiness>> returnedSaveFuncInfoWithValue = await IsDuplicateCodeAsync(ContextGuid, CodeMin, CodeMax, Guid, token, tranName);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 509);
				if (returnedSaveFuncInfoWithValue.value?.Any((TafsilGroupBusiness p) => p.Guid != Guid) ?? false)
				{
					if (system == EnSystem.Transfer && autoFixTryCount > 0)
					{
						long diff = CodeMax - CodeMin;
						CodeMin = await NextMinCodeAsync(ContextGuid, token, tranName);
						CodeMax = CodeMin + diff;
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						TafsilGroupBusiness tafsilGroupBusiness3 = this;
						short num = (short)(autoFixTryCount - 1);
						autoFixTryCount = num;
						returnedSaveFuncInfo.AddReturnedValue(await tafsilGroupBusiness3.CheckValidationAsync(system, token, tranName, isRemoveWithGuid, num, listMoeinGuid), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 517);
						return ret;
					}
					ret.AddError(system, "کد شروع بازه یا پایان بازه گروه تفصیلی".GetTranslate() + " " + (Name ?? "").GetTranslate() + " " + " با گروه دیگری همپوشانی دارد".GetTranslate() + " \r\n" + "کد شروع بازه".GetTranslate() + ":" + $"{CodeMin}\r\n" + "کد پایان بازه".GetTranslate() + ":" + $"{CodeMax}", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 522);
					foreach (TafsilGroupBusiness item in returnedSaveFuncInfoWithValue.value)
					{
						if (item != null)
						{
							ret.AddError("تداخل با گروه تفصیلی".GetTranslate() + "(" + (item.Name ?? "").GetTranslate() + ")-(" + item.CodeMin.NpToString("N0") + "=>" + item.CodeMax.NpToString("N0") + ")", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 527);
						}
					}
				}
				if (system == EnSystem.UserInterface && (listMoeinGuid?.Any() ?? false))
				{
					if (!listMoeinGuid.Contains(MoeinSellGuid))
					{
						ret.AddError("حساب معین فروش معتبر نمی باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 535);
					}
					if (!listMoeinGuid.Contains(MoeinBuyGuid))
					{
						ret.AddError("حساب معین خرید معتبر نمی باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 536);
					}
					if (!listMoeinGuid.Contains(MoeinBackSellGuid))
					{
						ret.AddError("حساب معین برگشت از فروش معتبر نمی باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 537);
					}
					if (!listMoeinGuid.Contains(MoeinBackBuyGuid))
					{
						ret.AddError("حساب معین برگشت از خرید معتبر نمی باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 538);
					}
					if (!listMoeinGuid.Contains(MoeinReceptionGuid))
					{
						ret.AddError("حساب معین دریافت معتبر نمی باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 539);
					}
					if (!listMoeinGuid.Contains(MoeinPardakhtGuid))
					{
						ret.AddError("حساب معین پرداخت معتبر نمی باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 540);
					}
					if (!listMoeinGuid.Contains(MoeinAmaniMaNazdDigaranGuid))
					{
						ret.AddError("حساب معین امانی ما نزد دیگران معتبر نمی باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 541);
					}
					if (!listMoeinGuid.Contains(MoeinAmaniDigaranNazdMaGuid))
					{
						ret.AddError("حساب معین امانی دیگران نزد ما معتبر نمی باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 542);
					}
					if (!listMoeinGuid.Contains(MoeinBazaryabSell1))
					{
						ret.AddError("حساب معین بازاریاب فروش".GetTranslate() + "1" + "معتبر نمی باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 543);
					}
					if (!listMoeinGuid.Contains(MoeinBazaryabSell2))
					{
						ret.AddError("حساب معین بازاریاب فروش".GetTranslate() + "2" + "معتبر نمی باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 544);
					}
					if (!listMoeinGuid.Contains(MoeinBazaryabSell3))
					{
						ret.AddError("حساب معین بازاریاب فروش".GetTranslate() + "3" + "معتبر نمی باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 545);
					}
					if (!listMoeinGuid.Contains(MoeinBazaryabSell4))
					{
						ret.AddError("حساب معین بازاریاب فروش".GetTranslate() + "4" + "معتبر نمی باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 546);
					}
					if (!listMoeinGuid.Contains(MoeinBazaryabBackSell1))
					{
						ret.AddError("حساب معین بازاریاب برگشت از فروش".GetTranslate() + "1" + " معتبر نمی باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 547);
					}
					if (!listMoeinGuid.Contains(MoeinBazaryabBackSell2))
					{
						ret.AddError("حساب معین بازاریاب برگشت از فروش".GetTranslate() + "2" + " معتبر نمی باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 548);
					}
					if (!listMoeinGuid.Contains(MoeinBazaryabBackSell3))
					{
						ret.AddError("حساب معین بازاریاب برگشت از فروش".GetTranslate() + "3" + " معتبر نمی باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 549);
					}
					if (!listMoeinGuid.Contains(MoeinBazaryabBackSell4))
					{
						ret.AddError("حساب معین بازاریاب برگشت از فروش".GetTranslate() + "4" + " معتبر نمی باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 550);
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 555);
			ret.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 556);
		}
		finally
		{
			if (ret.HasError && system != EnSystem.UserInterface)
			{
				WebErrorLog.Instance.StartLog("TafsilGroup Validation Has Error:" + ret.GetErrorMessage(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 561);
			}
		}
		return ret;
	}

	public static Task<long> NextMinCodeAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ITafsilGroup.NextMinCodeAsync(token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, bool send2Server, List<Guid> listMoeinGuid = null, Priority priority = Priority.Low, bool awaitOnSemaphore = true, bool await4send = false, bool isRemoveWithGuid = false, short tryCount = 10, string tranName = "", CancellationToken token = default(CancellationToken), short autoFixTyrCount = 0)
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		int line = 1;
		try
		{
			_ = 4;
			ReturnedSaveFuncInfo result;
			try
			{
				if (send2Server && ClsCache.GetCache(ContextGuid).Source == ENSource.Android_HesabYar)
				{
					throw new ArgumentException("برای نوع حسابیار نباید ارسال به سرور برای گروه های تفصیلی فراخوانی شود!!!".GetTranslate() + "\r\n" + $"Guid:{Guid}\r\n" + "Name:" + Name + "\r\n" + $"ServerSt:{ServerStatus}");
				}
				line = 2;
				if (ServerStatus != ServerStatusForClient.Delivered && ClsCache.GetCache(ContextGuid).Source == ENSource.Android_HesabYar)
				{
					throw new ArgumentException("برای نوع حسابیار نباید وضعیت گروههای تفصیلی در حالتی غیر از تحویل شده باید!!!".GetTranslate() + "\r\n" + $"ServerSt:{ServerStatus}\r\n" + "Name:" + Name + $"Guid:{Guid}");
				}
				line = 3;
				using (SemaphoreUtil semaphore = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "TafsilGroupBusiness/SaveAsync", 60000, token))
				{
					line = 4;
					if (autoTran)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 593), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 593);
					}
					transactionHasBeenCreated = true;
					if (semaphore.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 595);
					}
					else
					{
						line = 5;
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(system, token, tranName, isRemoveWithGuid, autoFixTyrCount, listMoeinGuid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 597);
						line = 60;
						if (semaphore.Token.IsCancellationRequested)
						{
							result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 599);
						}
						else
						{
							line = 61;
							if (system != EnSystem.UserInterface || !ret.HasError)
							{
								ret.throwExceptionIfError();
								line = 62;
								if (listMoeinGuid != null)
								{
									returnedSaveFuncInfo = ret;
									returnedSaveFuncInfo.AddReturnedValue(await MoeinTafsilGroupRelationsBusiness.SaveTafsilRelationsAsync(ContextGuid, Guid, listMoeinGuid, awaitOnSemaphore: true, tryCount, tranName, token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 605);
								}
								line = 63;
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ITafsilGroup.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 607);
								goto end_IL_02c1;
							}
							result = ret;
						}
					}
					goto end_IL_00f4;
					end_IL_02c1:;
				}
				goto end_IL_00e0;
				end_IL_00f4:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"#Line_{line}", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 611);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 612);
				goto end_IL_00e0;
			}
			return result;
			end_IL_00e0:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 617), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 617);
			}
			if (send2Server && !ret.HasError)
			{
				if (await4send)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Send2ServerAsync(token, system, priority), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 621);
				}
				else
				{
					Task.Run(() => Send2ServerAsync(token, system, priority));
				}
			}
			if (!token.IsCancellationRequested)
			{
				SaveLogAsync(system, ret);
			}
		}
		return ret;
	}

	private async Task SaveLogAsync(EnSystem system, ReturnedSaveFuncInfo ret)
	{
		_ = 1;
		try
		{
			if (ret.HasError && system != EnSystem.UserInterface && system != EnSystem.Transfer)
			{
				WebTafsilGroup webEntity = await TafsilGroupMapper.Instance.MapAsync(ContextGuid, this, new WebTafsilGroup());
				CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(ContextGuid);
				CashiersLogBusiness cashiersLogBusiness = new CashiersLogBusiness(ContextGuid);
				cashiersLogBusiness.Action = ENAction.Error;
				cashiersLogBusiness.ApplicationVersion = ClsCache.GetCache(ContextGuid)?.ApplicationVersion ?? "";
				cashiersLogBusiness.CashierGuid = CashierBusiness.GetCurrentCashierGuid(ContextGuid);
				cashiersLogBusiness.CashierName = cashierBusiness?.Name;
				cashiersLogBusiness.ContextGuid = ContextGuid;
				cashiersLogBusiness.DateM = DateTime.Now;
				cashiersLogBusiness.DateUtc = DateTime.Now.ToUniversalTime();
				cashiersLogBusiness.Guid = Guid.NewGuid();
				cashiersLogBusiness.System = system;
				cashiersLogBusiness.Source = ClsCache.GetCache(ContextGuid).Source;
				cashiersLogBusiness.SanadType = EnLog.TafsilGroup;
				cashiersLogBusiness.SanadGuid = Guid;
				cashiersLogBusiness.Title = "خطا در ذخیره گروه تفصیلی".GetTranslate() + " (" + (Name ?? "").GetTranslate() + ")-" + $" {Guid}\r\n" + ret.ErrorMessage + "\r\n" + Json.ToStringJson(webEntity, indented: true);
				cashiersLogBusiness.SaveAsync(system, sendToServer: true, awaitOnSemaphore: false, default(CancellationToken), 10);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 658);
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemovePhysicallyAsync(CancellationToken token = default(CancellationToken), bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			_ = 2;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "TafsilGroupBusiness/RemovePhysicalAsync", 60000, token))
				{
					if (!dis.Token.IsCancellationRequested)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemovePhysicallyAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 672), ReturnedState.None, "RemovePhysicallyAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 672);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ITafsilGroup.RemoveAsync(Guid, tryCount, token, tranName), ReturnedState.None, "RemovePhysicallyAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 674);
						goto end_IL_011b;
					}
					result = ret.AddErrorTokenIsCancelled("", "", "RemovePhysicallyAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 671);
					goto end_IL_007d;
					end_IL_011b:;
				}
				goto end_IL_006c;
				end_IL_007d:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemovePhysicallyAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 679);
				ret.AddReturnedValue(ex, "", "RemovePhysicallyAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 680);
				goto end_IL_006c;
			}
			return result;
			end_IL_006c:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "RemovePhysicallyAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 685), ReturnedState.None, "RemovePhysicallyAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 685);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(EnSystem system, IEnumerable<TafsilGroupBusiness> lstHesabsList, bool sendToServer, Priority priority = Priority.Low, bool isRemoveWithGuid = false, bool awaitOnSend = false, CancellationToken token = default(CancellationToken), short tryCount = 10, string tranName = "", Action<PercenteCompletedEventArgs> percentCompleted = null)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		CancellationTokenSource invokeToken2 = new CancellationTokenSource();
		try
		{
			if (!(lstHesabsList?.Any() ?? false))
			{
				return ret;
			}
			int current = 0;
			foreach (TafsilGroupBusiness item in lstHesabsList)
			{
				current = checked(current + 1);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 700);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(system, sendToServer, null, priority, awaitOnSemaphore: true, awaitOnSend, isRemoveWithGuid, tryCount, tranName, token, 10), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 701);
				if (percentCompleted != null)
				{
					invokeToken2?.Cancel();
					invokeToken2 = new CancellationTokenSource(60000);
					CancellationToken token2 = invokeToken2.Token;
					percentCompleted(new PercenteCompletedEventArgs(lstHesabsList.Count(), current, item.Name, token2));
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 713);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 714);
		}
		finally
		{
			if (percentCompleted != null)
			{
				invokeToken2?.Cancel();
				invokeToken2 = new CancellationTokenSource(60000);
				CancellationToken invokeTkn = invokeToken2.Token;
				Task.Run(delegate
				{
					percentCompleted(new PercenteCompletedEventArgs(lstHesabsList?.Count() ?? 0, lstHesabsList?.Count() ?? 0, "", invokeTkn));
				});
			}
		}
		return ret;
	}

	internal static async Task<ReturnedSaveFuncInfo> SaveRangeForHesabyarAsync(string contextGuid, IEnumerable<WebTafsilGroup> items, CancellationToken token)
	{
		string tranName = Guid.NewGuid().ToString();
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			_ = 2;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "TafsilGroupBusiness/SaveRangeForHesabyarAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 735);
					}
					else
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 736), ReturnedState.None, "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 736);
						if (!ret.HasError)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ITafsilGroup.SaveRangeAsync(items, token, tranName), ReturnedState.None, "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 738);
							goto end_IL_0104;
						}
						result = ret;
					}
					goto end_IL_006d;
					end_IL_0104:;
				}
				goto end_IL_005c;
				end_IL_006d:;
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 740);
				goto end_IL_005c;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 743);
				ret.AddReturnedValue(ex, "", "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 744);
				goto end_IL_005c;
			}
			return result;
			end_IL_005c:;
		}
		finally
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 748), ReturnedState.None, "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 748);
			if (ret.HasError && items.Count() == 1)
			{
				ret = await (await TafsilGroupMapper.Instance.MapAsync(contextGuid, items.Single(), null, "", token)).SaveAsync(EnSystem.SignalR, send2Server: false, null, Priority.Low, awaitOnSemaphore: true, await4send: false, isRemoveWithGuid: false, 10, "", default(CancellationToken), 10);
			}
			else if (ret.HasError && items.Count() > 1 && !token.IsCancellationRequested)
			{
				ret = new ReturnedSaveFuncInfo();
				List<Task<ReturnedSaveFuncInfo>> list = new List<Task<ReturnedSaveFuncInfo>>();
				List<WebTafsilGroup> list2 = new List<WebTafsilGroup>();
				WebTafsilGroup[] array = items.ToArray();
				for (int i = 0; i < items.Count() / 2; i = checked(i + 1))
				{
					list2.Add(array[i]);
				}
				list.Add(SaveRangeForHesabyarAsync(contextGuid, list2, token));
				list.Add(SaveRangeForHesabyarAsync(contextGuid, items.Except(list2), token));
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(list), ReturnedState.None, "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 763);
			}
		}
		return ret;
	}

	public static async Task<IEnumerable<TafsilGroupBusiness>> GetAllAsync(string contextGuid, EnSystem system, string search, CancellationToken token, bool status = true, string tranName = "")
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs"))
		{
			_ = 1;
			try
			{
				if (string.IsNullOrEmpty(search))
				{
					search = "";
				}
				IEnumerable<TafsilGroupBusiness> enumerable = await GetAllAsync(contextGuid, system, status);
				if (token.IsCancellationRequested)
				{
					return null;
				}
				List<string> list = search.SplitString();
				if (list != null && list.Count > 0)
				{
					foreach (string item in list)
					{
						if (token.IsCancellationRequested)
						{
							return null;
						}
						if (!string.IsNullOrEmpty(item) && !(item.Trim() == ""))
						{
							enumerable = enumerable?.Where((TafsilGroupBusiness x) => x.CodeMin.ToString().StringComparisons(item) || x.CodeMax.ToString().StringComparisons(item) || x.Name.StringComparisons(item));
						}
					}
				}
				return (await ApplyAccessAsync(contextGuid, enumerable, system, token, tranName))?.OrderBy((TafsilGroupBusiness o) => o.Name);
			}
			catch (OperationCanceledException)
			{
				return null;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 795);
				return new List<TafsilGroupBusiness>();
			}
		}
	}

	internal Guid GetOrderMoeinGuid(Document factorType)
	{
		try
		{
			switch (factorType)
			{
			case Document.OrderSell:
			case Document.PishFactor:
				return MoeinSellGuid;
			case Document.OrderBuy:
			case Document.OrderPreBuy:
				return MoeinBuyGuid;
			case Document.OrderBackBuy:
				return MoeinBackBuyGuid;
			case Document.OrderBackSell:
				return MoeinBackSellGuid;
			case Document.Zayeat:
				throw new NotImplementedException();
			case Document.AmaniVorod:
				return MoeinAmaniDigaranNazdMaGuid;
			case Document.AmaniKhoroj:
				return MoeinAmaniMaNazdDigaranGuid;
			default:
				throw new NotImplementedException();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetOrderMoeinGuid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 827);
			throw;
		}
	}

	public override IEditableDelivery GetRepository()
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.ITafsilGroup;
	}

	public async Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token = default(CancellationToken), short tryCount = 10, string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 840), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 840);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ITafsilGroup.SetSaveResult(item, token, tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 842);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 845);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 848);
				ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 849);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 854), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 854);
			}
		}
		return ret;
	}

	public Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName = "", short maxParalelTask = 20)
	{
		return ResendNotSentsAsync(contextGuid, token, tranName, maxParalelTask);
	}

	public static Task<TafsilGroupBusiness> GetFromDatabaseAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (!(guid == Guid.Empty))
		{
			return ClsCache.GetCache(contextGuid).CacheRepository.ITafsilGroup.GetFromDatabaseAsync(guid, token, tranName);
		}
		return Task.FromResult<TafsilGroupBusiness>(null);
	}

	public static async Task<ReturnedSaveFuncInfo> ResendNotSentsAsync(string contextGuid, CancellationToken token, string tranName = "", short maxParalelTask = 20)
	{
		using (new JaegerMonitor("ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs"))
		{
			int line = 0;
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			ClsCache cache = ClsCache.GetCache(contextGuid);
			ISendData2Server uiUpdater = cache?.SendData2ServerInstance?.Tafsilgroup;
			EnSystem system = EnSystem.SignalR;
			try
			{
				if (uiUpdater != null)
				{
					uiUpdater.ShortMessage = "در حال ارسال گروه های تفصیلی".GetTranslate();
					uiUpdater.Status = SyncStatus.Syncing;
					uiUpdater.FinalStatus = ret;
					uiUpdater.Group = SyncPart.Hesabs;
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 881);
				}
				if (cache == null)
				{
					return ret.AddError(system, "cache is null!!", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 882);
				}
				if (!cache.BufferEnable)
				{
					return ret.AddError(system, "cache doese not contains buffer!!", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 883);
				}
				_ = cache.BufferCancellationToken;
				line = 5;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 886);
				}
				if (ClsCache.GetCache(contextGuid).CacheRepository.ITafsilGroup == null)
				{
					throw new NullReferenceException("ClsCache.GetCache(contextGuid) .CacheRepository.ITafsilLevel is null");
				}
				List<Guid> list = (await ClsCache.GetCache(contextGuid).CacheRepository.ITafsilGroup.GetAllNotSentAsync(token, tranName)) ?? new List<Guid>();
				if (uiUpdater != null)
				{
					uiUpdater.TotalCount = list?.Count ?? 0;
				}
				line = 6;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 894);
				}
				if (list == null || !list.Any())
				{
					return ret.AddInformation("گروه های تفصیلی سینک هستند تعداد گروه تفصیلی سینک شده".GetTranslate() + ":" + $"{list?.Count ?? 0}", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 895);
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 896);
				}
				List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
				int current = 0;
				_ = (decimal)(list?.Count ?? 0);
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (list != null && list.Any())
				{
					foreach (Guid item in list)
					{
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 903);
						}
						current = checked(current + 1);
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 905);
						}
						TafsilGroupBusiness obj = await GetFromDatabaseAsync(contextGuid, item, token, tranName);
						if (uiUpdater != null)
						{
							uiUpdater.ShortMessage = "گروه جاری".GetTranslate() + ":" + (obj?.Name ?? "").GetTranslate();
							uiUpdater.Count = current;
						}
						if (obj.ServerStatus == ServerStatusForClient.Delivered || obj.ServerStatus == ServerStatusForClient.DirectDelivery)
						{
							ret.AddInformation("current status on database is delivered!\r\nEntityType: TafsilGroup \r\n" + $"EntityGuid:{obj?.Guid ?? Guid.Empty}\r\n" + "EntityName:" + obj?.Name, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 915);
							continue;
						}
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 921);
						}
						tasks.Add(Task.Run(() => obj.Send2ServerAsync(token, EnSystem.SignalR), token));
						if (tasks.Count >= maxParalelTask)
						{
							Task<ReturnedSaveFuncInfo> t = await Task.WhenAny(tasks);
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await t, ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 926);
							tasks.Remove(t);
						}
					}
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 930);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 931);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 933);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 936);
				ret.AddReturnedValue(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 937);
			}
			finally
			{
				if (uiUpdater != null)
				{
					if (ret.HasError)
					{
						uiUpdater.ShortMessage = "با شکست مواجه شد".GetTranslate();
						uiUpdater.Status = SyncStatus.SyncFailed;
					}
					else
					{
						uiUpdater.ShortMessage = "با موفقیت انجام شد.".GetTranslate();
						uiUpdater.Status = SyncStatus.SyncedOk;
					}
				}
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> Send2ServerAsync(CancellationToken token, EnSystem system, Priority priority = Priority.Low)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs"))
		{
			_ = 3;
			try
			{
				if (!ClsCache.GetCache(ContextGuid).BufferEnable)
				{
					return ret;
				}
				_sendToken?.Cancel();
				_sendToken = new CancellationTokenSource();
				CancellationToken token2 = _sendToken.Token;
				using SemaphoreUtil dis = await _sendToServerSemaphore.GaurdAsync(true, "TafsilGroupBusiness/Send2ServerAsync", 60000, token, token2);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 969);
				}
				List<WebTafsilGroup> list = new List<WebTafsilGroup>();
				List<WebTafsilGroup> list2 = list;
				list2.Add(await TafsilGroupMapper.Instance.MapAsync(ContextGuid, this, new WebTafsilGroup(), "", token));
				List<Packet> paks = WebTafsilGroup.GenerateSaveRequest(list);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 971);
				}
				using TafsilGroupWaiter waiter = new TafsilGroupWaiter(WebContextHolder.GetWebContextHolder(ContextGuid), Guid, dis.Token);
				Task.Run(() => ClsCache.GetCache(ContextGuid).SendData(paks, priority), token);
				ReturnedSaveFuncInfoWithValue<SaveResponse> returnedSaveFuncInfoWithValue = await waiter.WaitForSaveResponse("tafsilGreoupBusinessWaiter");
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 975);
				if (!ret.HasError && returnedSaveFuncInfoWithValue?.value != null)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SetSaveResultAsync(returnedSaveFuncInfoWithValue.value, token, 10), ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 977);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 979);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 982);
				ret.AddReturnedValue(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 983);
			}
			return ret;
		}
	}

	public static async Task<TafsilGroupBusiness> GetAsync(string contextGuid, string name, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			return await ApplyAccessAsync(contextGuid, await ClsCache.GetCache(contextGuid).CacheRepository.ITafsilGroup.GetAsync(name, token, tranName), system, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 996);
			throw;
		}
	}

	public static Task<TafsilGroupBusiness> GenerateNewWithDefaultsAsync(string contextGuid, EnSystem system, string description = "", Guid? guid = null, string tranName = "")
	{
		try
		{
			TafsilGroupBusiness tafsilGroupBusiness = new TafsilGroupBusiness(contextGuid)
			{
				ChangeStatus = EntityChangeSt.New
			};
			if (!string.IsNullOrEmpty(description))
			{
				tafsilGroupBusiness.Description = description.FixString();
			}
			if ((guid ?? Guid.Empty) != Guid.Empty)
			{
				tafsilGroupBusiness.Guid = guid.Value;
			}
			else
			{
				tafsilGroupBusiness.Guid = Guid.NewGuid();
			}
			return Task.FromResult(tafsilGroupBusiness);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateNewWithDefaultsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 1012);
			throw;
		}
	}

	public void SetRelatedMoeinGuid(Guid moeinGuid)
	{
		try
		{
			SetMoeinGuid(moeinGuid);
			Guid guid2 = (MoeinBuyGuid = moeinGuid);
			MoeinSellGuid = guid2;
			guid2 = (MoeinBackBuyGuid = moeinGuid);
			MoeinBackSellGuid = guid2;
			guid2 = (MoeinPardakhtGuid = moeinGuid);
			MoeinReceptionGuid = guid2;
			guid2 = (MoeinAmaniMaNazdDigaranGuid = moeinGuid);
			MoeinAmaniDigaranNazdMaGuid = guid2;
			guid2 = (MoeinBazaryabBackSell2 = moeinGuid);
			MoeinBazaryabBackSell1 = guid2;
			guid2 = (MoeinBazaryabBackSell4 = moeinGuid);
			MoeinBazaryabBackSell3 = guid2;
			guid2 = (MoeinBazaryabSell2 = moeinGuid);
			MoeinBazaryabSell1 = guid2;
			guid2 = (MoeinBazaryabSell4 = moeinGuid);
			MoeinBazaryabSell3 = guid2;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetRelatedMoeinGuid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 1030);
		}
	}

	private static async Task<IEnumerable<TafsilGroupBusiness>> ApplyAccessAsync(string contextGuid, IEnumerable<TafsilGroupBusiness> items, EnSystem system, CancellationToken token, string tranName = "")
	{
		_ = 1;
		try
		{
			if (token.IsCancellationRequested)
			{
				return null;
			}
			if (system != EnSystem.UserInterface)
			{
				return items;
			}
			CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token, tranName);
			if (cashierBusiness == null)
			{
				return items;
			}
			if (token.IsCancellationRequested)
			{
				return null;
			}
			UserAccBusiness user = await cashierBusiness.GetUserAsync(token, tranName);
			if (token.IsCancellationRequested)
			{
				return null;
			}
			if (user == null)
			{
				throw new NullReferenceException("UserIs Null!!");
			}
			if (user.UserAccess.TafsilGroupJustListGuid?.Any() ?? false)
			{
				items = items?.Where((TafsilGroupBusiness p) => p != null && user.UserAccess.TafsilGroupJustListGuid.Contains(p.Guid));
			}
			if (token.IsCancellationRequested)
			{
				return null;
			}
			items = items?.ToList();
			return items;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\TafsilGroupBusiness.cs", 1054);
			throw;
		}
	}

	private static async Task<TafsilGroupBusiness> ApplyAccessAsync(string contextGuid, TafsilGroupBusiness item, EnSystem system, CancellationToken token, string tranName)
	{
		if (system == EnSystem.UserInterface)
		{
			return (await ApplyAccessAsync(contextGuid, new List<TafsilGroupBusiness> { item }, system, token, tranName))?.SingleOrDefault();
		}
		return item;
	}
}
