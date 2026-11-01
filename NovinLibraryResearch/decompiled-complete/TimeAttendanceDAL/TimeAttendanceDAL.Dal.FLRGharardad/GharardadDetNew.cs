using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PacketParser.Attendance.InterFaces;
using PacketParser.Entities.Interfaces;
using Serilog;
using SmartAssembly.Attributes;
using TimeAttendanceDAL.Dal.Salary;
using TimeAttendanceDAL.Utility;

namespace TimeAttendanceDAL.Dal.FLRGharardad;

[Serializable]
[DoNotPruneType]
public class GharardadDetNew : IContractDet, IHasContext
{
	private class GharardadDetNewWithoutCtor : GharardadDetNew
	{
		public GharardadDetNewWithoutCtor()
			: base("")
		{
		}

		public GharardadDetNew BaseClass(string contextGuid)
		{
			base.ContextGuid = contextGuid;
			return this;
		}
	}

	private FishHesab _fishHesab;

	private decimal _nValue;

	private decimal _account;

	public string ContextGuid { get; set; }

	public int HesabId { get; set; }

	public string Name { get; set; } = "";


	public decimal nValue
	{
		get
		{
			return _nValue.RoundPrice();
		}
		set
		{
			_nValue = value;
		}
	}

	public short Order { get; set; }

	public string Desc { get; set; } = "";


	public short TypeAccount { get; set; }

	public bool IsDecrease { get; set; }

	public decimal Account
	{
		get
		{
			return _account.RoundPrice();
		}
		set
		{
			_account = value;
		}
	}

	public string Formula { get; set; } = "";


	public int Privilege { get; set; }

	public async Task<bool> GetTaxableAsync(CancellationToken token)
	{
		try
		{
			return (await GetFishHesabAsync(token))?.Taxable ?? false;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}

	public async Task<bool> GetInsuranceAsync(CancellationToken token)
	{
		try
		{
			return (await GetFishHesabAsync(token))?.Insurance ?? false;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}

	public async Task<FishHesab> GetFishHesabAsync(CancellationToken token)
	{
		try
		{
			FishHesab fishHesab = _fishHesab;
			if (fishHesab == null)
			{
				fishHesab = (_fishHesab = await FishHesab.GetAsync(ContextGuid, HesabId, token));
			}
			return fishHesab;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}

	public GharardadDetNew(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public GharardadDetNew(string contextGuid, short type, int privilege, string pfurmula, string pName, string pDesc)
	{
		ContextGuid = contextGuid;
		TypeAccount = type;
		Privilege = privilege;
		nValue = -1m;
		Formula = pfurmula;
		Name = pName;
		Desc = pDesc;
	}

	public static async Task<GharardadDetNew> GetAsync(string contextGuid, int pHesabId, CancellationToken token)
	{
		try
		{
			GharardadDetNew ret = new GharardadDetNew(contextGuid);
			await ret.LoadHesabIdFieldsAsync(pHesabId, isFromconstructor: true, token);
			return ret;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}

	public GharardadDetNew(string contextGuid, GharardadDetNew pcls)
	{
		ContextGuid = contextGuid;
		try
		{
			HesabId = pcls.HesabId;
			Name = pcls.Name;
			nValue = pcls.nValue;
			Order = pcls.Order;
			Desc = pcls.Desc;
			TypeAccount = pcls.TypeAccount;
			IsDecrease = pcls.IsDecrease;
			Account = pcls.Account;
			Formula = pcls.Formula;
			Privilege = pcls.Privilege;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}

	private async Task LoadHesabIdFieldsAsync(int pHesabId, bool isFromconstructor = false, CancellationToken token = default(CancellationToken))
	{
		try
		{
			FishHesab fishHesab = await FishHesab.GetAsync(ContextGuid, pHesabId, token);
			if (isFromconstructor)
			{
				HesabId = fishHesab.Id;
				Desc = fishHesab.Description;
				nValue = fishHesab.Account;
			}
			Name = fishHesab.Name;
			TypeAccount = fishHesab.Type;
			Account = fishHesab.Account;
			Privilege = 0;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}

	public static GharardadDetNew GharardadDetToNew(string contextGuid, GharardadDet pGharardadDet)
	{
		try
		{
			GharardadDetNew gharardadDetNew = new GharardadDetNew(contextGuid);
			gharardadDetNew.HesabId = pGharardadDet.HesabId;
			gharardadDetNew.Name = pGharardadDet.Name;
			if (pGharardadDet.Credit > 0m)
			{
				gharardadDetNew.TypeAccount = 1;
				gharardadDetNew.nValue = pGharardadDet.Credit;
			}
			else
			{
				gharardadDetNew.TypeAccount = 2;
				gharardadDetNew.nValue = pGharardadDet.Debit;
			}
			gharardadDetNew.Order = pGharardadDet.Order;
			gharardadDetNew.Desc = pGharardadDet.Desc;
			gharardadDetNew.Account = pGharardadDet.Account;
			gharardadDetNew.IsDecrease = pGharardadDet.IsDecrease;
			return gharardadDetNew;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}

	public static List<GharardadDetNew> Deserialize(string contextGuid, string jsonString)
	{
		try
		{
			return (from p in JsonConvert.DeserializeObject<List<GharardadDetNewWithoutCtor>>(jsonString)
				select p.BaseClass(contextGuid))?.ToList();
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static string Serialize(List<GharardadDetNew> listGharardadDet)
	{
		try
		{
			return JsonConvert.SerializeObject((object)listGharardadDet);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}
}
