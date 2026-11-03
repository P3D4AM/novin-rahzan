using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Mapping.BusinessMappers;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Accounting;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business.Operations;

public class HesabTreesSeparation : IHasContext
{
	private List<KolBusiness> _listKol;

	private List<TafsilBusiness> _listTafsil;

	private List<MoeinBusiness> _listMoein;

	public List<KolBusiness> ListKol => _listKol?.Where((KolBusiness p) => p != null).ToList();

	public List<TafsilBusiness> ListTafsil => _listTafsil?.Where((TafsilBusiness p) => p != null).ToList();

	public List<MoeinBusiness> ListMoein => _listMoein?.Where((MoeinBusiness p) => p != null).ToList();

	public string ContextGuid { get; set; }

	public static async Task<HesabTreesSeparation> GetFactoryAsync(string contextGuid, IEnumerable<WebHesabTree> lstWebHesabtree)
	{
		try
		{
			HesabTreesSeparation cls = new HesabTreesSeparation(contextGuid, lstWebHesabtree);
			await cls.CreateListsAsync(contextGuid, lstWebHesabtree);
			return cls;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetFactoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Operations\\HesabTreesSeparation.cs", 38);
		}
		return null;
	}

	private HesabTreesSeparation(string contextGuid, IEnumerable<WebHesabTree> lstWebHesabtree)
	{
	}

	public HesabTreesSeparation(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	private async Task<ReturnedSaveFuncInfo> CreateListsAsync(string contextGuid, IEnumerable<WebHesabTree> lstWebHesabtree)
	{
		int line = 0;
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			ContextGuid = contextGuid;
			_listKol = new List<KolBusiness>();
			_listMoein = new List<MoeinBusiness>();
			_listTafsil = new List<TafsilBusiness>();
			if (lstWebHesabtree == null)
			{
				return ret.AddInformation("list is null", "", "CreateListsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Operations\\HesabTreesSeparation.cs", 91);
			}
			foreach (WebHesabTree webHesabTreeItem in lstWebHesabtree)
			{
				line = 1;
				switch (webHesabTreeItem.Level)
				{
				case ENHesabLevel.Kol:
				{
					ReturnedSaveFuncInfoWithValue<KolBusiness> returnedSaveFuncInfoWithValue = await CreateKolObjectAsync(webHesabTreeItem, contextGuid);
					ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "CreateListsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Operations\\HesabTreesSeparation.cs", 101);
					_listKol.Add(returnedSaveFuncInfoWithValue.value);
					break;
				}
				case ENHesabLevel.Moein:
					_listMoein.Add(CreateMoeinObject(webHesabTreeItem));
					break;
				case ENHesabLevel.Tafsil:
					_listTafsil.Add(CreateTafsilObject(webHesabTreeItem));
					break;
				}
				if (webHesabTreeItem.GardeshTafsili == null)
				{
					continue;
				}
				foreach (WebGardeshTafsili item in webHesabTreeItem.GardeshTafsili)
				{
					line = 2;
					item.TafsilGuid = webHesabTreeItem.TafsilGuid;
					line = 3;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"line:{line}", "CreateListsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Operations\\HesabTreesSeparation.cs", 126);
		}
		return ret;
	}

	private async Task<List<GardeshTafsilBusiness>> CreateListGardeshTafsilAsync(List<GardeshTafsil> gardeshTafsili)
	{
		try
		{
			CancellationToken token = default(CancellationToken);
			if (gardeshTafsili == null)
			{
				return null;
			}
			return await GardeshTafsilMapper.Instance.MapListAsync(ContextGuid, gardeshTafsili, new List<GardeshTafsilBusiness>(), "", token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CreateListGardeshTafsilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Operations\\HesabTreesSeparation.cs", 141);
			return null;
		}
	}

	private TafsilBusiness CreateTafsilObject(WebHesabTree webHesabTreeItem)
	{
		try
		{
			return new TafsilBusiness(ContextGuid)
			{
				Guid = webHesabTreeItem.TafsilGuid,
				Description = (webHesabTreeItem.Description ?? ""),
				HType = webHesabTreeItem.HType,
				Modified = DateTime.Now,
				Name = webHesabTreeItem.Name,
				Status = webHesabTreeItem.Status,
				System = webHesabTreeItem.System,
				TafsilGroupGuid = webHesabTreeItem.TafsilGroupGuid,
				DateM = webHesabTreeItem.DateM,
				SerializedData = webHesabTreeItem.SerializedData,
				ServerStatus = ServerStatusForClient.Delivered,
				Code = webHesabTreeItem.Code.ParseToLong(0L)
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CreateTafsilObject", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Operations\\HesabTreesSeparation.cs", 170);
			return null;
		}
	}

	private MoeinBusiness CreateMoeinObject(WebHesabTree webHesabTreeItem)
	{
		try
		{
			return new MoeinBusiness(ContextGuid)
			{
				Guid = webHesabTreeItem.Guid,
				Account = webHesabTreeItem.Account,
				Description = (webHesabTreeItem.Description ?? ""),
				Mahiat = webHesabTreeItem.Mahiat,
				Modified = DateTime.Now,
				Name = webHesabTreeItem.Name,
				Status = webHesabTreeItem.Status,
				System = webHesabTreeItem.System,
				KolGuid = webHesabTreeItem.KolGuid,
				Peygiri = webHesabTreeItem.Peygiri,
				Tedadi = webHesabTreeItem.Tedadi,
				DateM = webHesabTreeItem.DateM,
				ServerStatus = ServerStatusForClient.Delivered,
				Code = webHesabTreeItem.CodeMoein.ParseToLong(0L)
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CreateMoeinObject", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Operations\\HesabTreesSeparation.cs", 199);
			return null;
		}
	}

	private async Task<ReturnedSaveFuncInfoWithValue<KolBusiness>> CreateKolObjectAsync(WebHesabTree webHesabTreeItem, string contextGuid)
	{
		ReturnedSaveFuncInfoWithValue<KolBusiness> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<KolBusiness>();
		try
		{
			returnedSaveFuncInfoWithValue.value = new KolBusiness(ContextGuid)
			{
				Guid = webHesabTreeItem.Guid,
				Account = webHesabTreeItem.Account,
				Code = webHesabTreeItem.CodeKol.ParseToInt(),
				Description = (webHesabTreeItem.Description ?? ""),
				Modified = DateTime.Now,
				Name = webHesabTreeItem.Name,
				Status = webHesabTreeItem.Status,
				System = webHesabTreeItem.System,
				DateM = webHesabTreeItem.DateM,
				ServerStatus = ServerStatusForClient.Delivered
			};
			returnedSaveFuncInfoWithValue.value.SetGroupGuid(EnSystem.None, webHesabTreeItem.GroupGuid);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CreateKolObjectAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Operations\\HesabTreesSeparation.cs", 226);
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "CreateKolObjectAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Operations\\HesabTreesSeparation.cs", 227);
		}
		return returnedSaveFuncInfoWithValue;
	}
}
