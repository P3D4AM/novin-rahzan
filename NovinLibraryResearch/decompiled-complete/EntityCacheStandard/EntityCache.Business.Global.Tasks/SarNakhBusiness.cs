using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Business.Global.Tasks.SarNakh;
using EntityCache.Business.Report;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Global.Tasks;
using PacketParser.Entities.MetaData;
using PacketParser.Filters.Global;

namespace EntityCache.Business.Global.Tasks;

public class SarNakhBusiness : Serializable<SarNakhBusiness>, ISarNakh, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext, IHasSanadStatusReport
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private List<SarNakhLocationBusiness> _locationlst;

	private List<SarNakhNumbersBusiness> _numberslst;

	private bool _isDisposed;

	public Guid Guid { get; set; }

	public string ContextGuid { get; set; }

	public string Company { get; set; }

	public string FullName { get; set; }

	public string Description { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public Guid CashierGuid { get; set; }

	public SarNakhMetaData MetaData { get; set; }

	public short StatusId { get; set; }

	public Guid? IntroductionGuid { get; set; }

	public DateTime CreationDate { get; set; }

	public EntityChangeSt ChangeStatus { get; set; }

	public Document AsType => Document.SarNakh;

	public IReadOnlyCollection<SarNakhLocationBusiness> LocationList
	{
		get
		{
			if (_locationlst == null)
			{
				_locationlst = new List<SarNakhLocationBusiness>();
			}
			_locationlst.RemoveAll((SarNakhLocationBusiness q) => q?.IsDisposed ?? true);
			return _locationlst;
		}
	}

	public IReadOnlyCollection<SarNakhNumbersBusiness> NumbersList
	{
		get
		{
			if (_numberslst == null)
			{
				_numberslst = new List<SarNakhNumbersBusiness>();
			}
			_numberslst.RemoveAll((SarNakhNumbersBusiness q) => q?.IsDisposed ?? true);
			return _numberslst;
		}
	}

	public event Func<object, EventArgs, Task> OnItemDisposed;

	public SarNakhBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<SarNakhBusiness> GenerateDefaultAsync(string contextGuid, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			SarNakhBusiness item = new SarNakhBusiness(contextGuid)
			{
				Guid = Guid.NewGuid(),
				Status = true,
				Modified = DateTime.Now,
				CreationDate = DateTime.Now,
				ChangeStatus = EntityChangeSt.New,
				CashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid)
			};
			await SanadStatusReportBusiness.ApplySanadStatusIdAsync(contextGuid, item, system, token, tranName);
			if (item.StatusId <= 0)
			{
				item.StatusId = checked((short)((await SanadStatusReportBusiness.GetAsync(contextGuid, ParentDefaults.SarNakh.Start, token, tranName))?.Id ?? 0));
			}
			return item;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateDefaultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\SarNakh\\SarNakhBusiness.cs", 88);
			throw;
		}
	}

	public Task AddDetailLocationListAsync(SarNakhLocationBusiness det)
	{
		using (new JaegerMonitor("AddDetailLocationListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\SarNakh\\SarNakhBusiness.cs"))
		{
			if (det == null)
			{
				return Task.CompletedTask;
			}
			try
			{
				if (_locationlst == null)
				{
					_locationlst = new List<SarNakhLocationBusiness>();
				}
				SarNakhLocationBusiness sarNakhLocationBusiness = _locationlst?.FirstOrDefault((SarNakhLocationBusiness q) => q.Ordinal == det.Ordinal);
				if (sarNakhLocationBusiness != null)
				{
					_locationlst.Remove(sarNakhLocationBusiness);
				}
				det.OnDisposed += Location_OnDisposed;
				det.SarNakhGuid = Guid;
				_locationlst.Add(det);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AddDetailLocationListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\SarNakh\\SarNakhBusiness.cs", 105);
			}
			return Task.CompletedTask;
		}
	}

	public Task AddDetailNumbersListAsync(SarNakhNumbersBusiness det)
	{
		using (new JaegerMonitor("AddDetailNumbersListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\SarNakh\\SarNakhBusiness.cs"))
		{
			if (det == null)
			{
				return Task.CompletedTask;
			}
			try
			{
				if (_numberslst == null)
				{
					_numberslst = new List<SarNakhNumbersBusiness>();
				}
				SarNakhNumbersBusiness sarNakhNumbersBusiness = _numberslst?.FirstOrDefault((SarNakhNumbersBusiness q) => q.Number == det.Number);
				if (sarNakhNumbersBusiness != null)
				{
					_numberslst.Remove(sarNakhNumbersBusiness);
				}
				det.OnDisposed += Number_OnDisposed;
				det.SarNakhGuid = Guid;
				_numberslst.Add(det);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AddDetailNumbersListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\SarNakh\\SarNakhBusiness.cs", 121);
			}
			return Task.CompletedTask;
		}
	}

	public Task RemoveDetailLocationListAsync(int ordinal)
	{
		try
		{
			SarNakhLocationBusiness sarNakhLocationBusiness = _locationlst?.SingleOrDefault((SarNakhLocationBusiness q) => q.Ordinal == ordinal);
			if (sarNakhLocationBusiness == null)
			{
				return Task.CompletedTask;
			}
			_locationlst.Remove(sarNakhLocationBusiness);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveDetailLocationListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\SarNakh\\SarNakhBusiness.cs", 132);
		}
		return Task.CompletedTask;
	}

	public Task RemoveDetailNumbersListAsync(string number)
	{
		if (string.IsNullOrEmpty(number))
		{
			return Task.CompletedTask;
		}
		try
		{
			SarNakhNumbersBusiness sarNakhNumbersBusiness = _numberslst?.SingleOrDefault((SarNakhNumbersBusiness q) => q.Number == number);
			if (sarNakhNumbersBusiness == null)
			{
				return Task.CompletedTask;
			}
			_numberslst.Remove(sarNakhNumbersBusiness);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveDetailNumbersListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\SarNakh\\SarNakhBusiness.cs", 144);
		}
		return Task.CompletedTask;
	}

	public override void Dispose()
	{
		try
		{
			if (_isDisposed)
			{
				return;
			}
			_isDisposed = true;
			IReadOnlyCollection<SarNakhNumbersBusiness> numbersList = NumbersList;
			if (numbersList != null && numbersList.Any())
			{
				foreach (SarNakhNumbersBusiness numbers in NumbersList)
				{
					numbers?.Dispose();
				}
			}
			IReadOnlyCollection<SarNakhLocationBusiness> locationList = LocationList;
			if (locationList != null && locationList.Any())
			{
				foreach (SarNakhLocationBusiness location in LocationList)
				{
					location?.Dispose();
				}
			}
			base.Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\SarNakh\\SarNakhBusiness.cs", 164);
		}
	}

	private Task Location_OnDisposed(object sender, EventArgs e)
	{
		using (new JaegerMonitor("Location_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\SarNakh\\SarNakhBusiness.cs"))
		{
			try
			{
				if ((_locationlst?.ToList()?.Any()).GetValueOrDefault())
				{
					foreach (SarNakhLocationBusiness item in _locationlst?.ToList()?.Where((SarNakhLocationBusiness p) => p == null || p == sender)?.ToList() ?? new List<SarNakhLocationBusiness>())
					{
						item?.Dispose();
					}
				}
				_locationlst?.RemoveAll((SarNakhLocationBusiness p) => p == null || p == sender);
				if (this.OnItemDisposed != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnItemDisposed, this, new EventArgs(), default(CancellationToken), 60, "Location_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\SarNakh\\SarNakhBusiness.cs");
				}
				return Task.CompletedTask;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Location_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\SarNakh\\SarNakhBusiness.cs", 186);
				throw;
			}
		}
	}

	private Task Number_OnDisposed(object sender, EventArgs e)
	{
		using (new JaegerMonitor("Number_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\SarNakh\\SarNakhBusiness.cs"))
		{
			try
			{
				if ((_numberslst?.ToList()?.Any()).GetValueOrDefault())
				{
					foreach (SarNakhNumbersBusiness item in _numberslst?.ToList()?.Where((SarNakhNumbersBusiness p) => p == null || p == sender)?.ToList() ?? new List<SarNakhNumbersBusiness>())
					{
						item?.Dispose();
					}
				}
				_numberslst?.RemoveAll((SarNakhNumbersBusiness p) => p == null || p == sender);
				if (this.OnItemDisposed != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnItemDisposed, this, new EventArgs(), default(CancellationToken), 60, "Number_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\SarNakh\\SarNakhBusiness.cs");
				}
				return Task.CompletedTask;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Number_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\SarNakh\\SarNakhBusiness.cs", 210);
				throw;
			}
		}
	}

	public int GetNextOrdinal()
	{
		try
		{
			List<SarNakhLocationBusiness> locationlst = _locationlst;
			if (locationlst != null && locationlst.Any())
			{
				return checked((_locationlst?.Max((SarNakhLocationBusiness p) => p.Ordinal) ?? (-1)) + 1);
			}
			return 1;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetNextOrdinal", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\SarNakh\\SarNakhBusiness.cs", 224);
			throw;
		}
	}

	public static Task<IEnumerable<SarNakhReportBusiness>> GetAllAsync(string contextGuid, SarNakhFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ISarNakh.GetAllAsync(filter, token, tranName);
	}

	public static Task<List<string>> GetStateListAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ISarNakh.GetStateListAsync(token, tranName);
	}

	public static Task<List<string>> GetCityListAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ISarNakh.GetCityListAsync(token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 3;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "SarNakhBusiness/SaveAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = res.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\SarNakh\\SarNakhBusiness.cs", 243);
					}
					else
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\SarNakh\\SarNakhBusiness.cs", 244), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\SarNakh\\SarNakhBusiness.cs", 244);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\SarNakh\\SarNakhBusiness.cs", 246);
						if (!res.HasError)
						{
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ISarNakh.SaveAsync(this, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\SarNakh\\SarNakhBusiness.cs", 248);
							goto end_IL_011c;
						}
						result = res;
					}
					goto end_IL_007d;
					end_IL_011c:;
				}
				goto end_IL_006c;
				end_IL_007d:;
			}
			catch (OperationCanceledException exception)
			{
				res.AddReturnedValue(exception, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\SarNakh\\SarNakhBusiness.cs", 250);
				goto end_IL_006c;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\SarNakh\\SarNakhBusiness.cs", 253);
				res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\SarNakh\\SarNakhBusiness.cs", 254);
				goto end_IL_006c;
			}
			return result;
			end_IL_006c:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\SarNakh\\SarNakhBusiness.cs", 259), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\SarNakh\\SarNakhBusiness.cs", 259);
			}
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> CheckValidationAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (string.IsNullOrEmpty(FullName))
			{
				await res.AddErrorAsync("لطفا نام و نام خانوادگی را وارد نمایید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\SarNakh\\SarNakhBusiness.cs", 268);
			}
			if ((IntroductionGuid ?? Guid.Empty) == Guid.Empty)
			{
				IntroductionGuid = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\SarNakh\\SarNakhBusiness.cs", 273);
			res.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\Tasks\\SarNakh\\SarNakhBusiness.cs", 274);
		}
		return res;
	}

	public static Task<SarNakhBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ISarNakh.GetAsync(guid, token, tranName);
	}

	public Task<int> NextId2Async(short? statusId, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return Task.FromResult(0);
	}

	public void SetId2(int value, CancellationToken token)
	{
	}
}
