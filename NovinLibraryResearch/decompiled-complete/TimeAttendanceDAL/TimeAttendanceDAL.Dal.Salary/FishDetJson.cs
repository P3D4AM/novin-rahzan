using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Attendance;
using Serilog;
using SmartAssembly.Attributes;
using TimeAttendanceDAL.Utility;

namespace TimeAttendanceDAL.Dal.Salary;

[Serializable]
[DoNotPruneType]
public class FishDetJson : IAttFishDetail, IHasContext
{
	private FishHesab _fishHesab;

	private short _typeAccount;

	private decimal _tmpValue;

	private decimal _debit;

	public string ContextGuid { get; set; }

	public int HesabId { get; set; }

	public decimal Debit
	{
		get
		{
			return _debit.RoundPrice();
		}
		set
		{
			_debit = value;
		}
	}

	public decimal Credit { get; set; }

	public decimal TmpValue
	{
		get
		{
			return _tmpValue.RoundPrice();
		}
		set
		{
			_tmpValue = value;
		}
	}

	public string Description { get; set; }

	public short TypeAccount
	{
		get
		{
			if (_typeAccount == 0 && Type > 0)
			{
				_typeAccount = Type;
			}
			return _typeAccount;
		}
		set
		{
			_typeAccount = value;
		}
	}

	public short Type { get; set; }

	public string Name { get; set; } = "";


	public bool IsDecrease { get; set; }

	public string Formula { get; set; } = "";


	public int Privilege { get; set; }

	public decimal TotalPayable { get; set; }

	public decimal RemainingBeforeBill { get; set; }

	public DateTime StartMounthPayM { get; set; }

	public bool Taxable { get; set; }

	public bool Insurance { get; set; }

	public bool DisableCompute { get; set; }

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
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task<long> GetCodeMoeinAsync(CancellationToken token)
	{
		try
		{
			return (await GetFishHesabAsync(token)).CodeMoein;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task<long> GetCodeTafsilAsync(CancellationToken token)
	{
		try
		{
			return (await GetFishHesabAsync(token)).CodeTafsil;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public FishDetJson()
	{
	}

	public FishDetJson(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public FishDetJson(string contextGuid, FishHesab pFishHesab)
	{
		ContextGuid = contextGuid;
		HesabId = pFishHesab.Id;
		Description = pFishHesab.Description;
		TypeAccount = pFishHesab.Type;
		Name = pFishHesab.Name;
		Taxable = pFishHesab.Taxable;
		Insurance = pFishHesab.Insurance;
		TmpValue = pFishHesab.Account;
		if (TypeAccount == 1)
		{
			Credit = pFishHesab.Account;
		}
		else
		{
			Debit = pFishHesab.Account;
		}
	}

	public static FishDetJson FishDetToNew(string contextGuid, FishDet pFishDet)
	{
		return new FishDetJson(contextGuid)
		{
			HesabId = pFishDet.HesabId,
			Name = pFishDet.Name,
			TypeAccount = pFishDet.Type,
			Credit = pFishDet.Credit,
			Debit = pFishDet.Debit,
			TmpValue = pFishDet.TmpValue,
			Description = pFishDet.Description,
			IsDecrease = pFishDet.IsDecrease
		};
	}

	public static List<FishDetJson> Deserialize(string jsonString, string contextGuid)
	{
		List<FishDetJson> list = new List<FishDetJson>();
		try
		{
			list = JsonConvert.DeserializeObject<List<FishDetJson>>(jsonString);
			foreach (FishDetJson item in list)
			{
				item.ContextGuid = contextGuid;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Deserialize", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Salary\\FishDetJson.cs", 174);
		}
		return list;
	}

	public static string Serialize(List<FishDetJson> listGharardadDet)
	{
		return JsonConvert.SerializeObject((object)listGharardadDet);
	}
}
