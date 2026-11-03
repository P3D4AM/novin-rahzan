using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Attendance;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Attendance.Biometric;
using PacketParser.Filters.Attendance;

namespace EntityCache.Business.Attendance;

public class BioDeviceRelationBusiness : IBioDeviceRelation, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	public Guid BioGuid { get; set; }

	public Guid DeviceGuid { get; set; }

	public int HardwareCode { get; set; }

	public EnAttendancePrivilage Privilage { get; set; }

	public DateTime? FromDateM { get; set; }

	public DateTime? ToDateM { get; set; }

	public bool Status { get; set; }

	public DateTime Modified { get; set; }

	public string ContextGuid { get; set; }

	public BioDeviceRelationBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static Task<BioDeviceRelationBusiness> GetAsync(string contextGuid, Guid bioGuid, Guid deviceGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.BioDeviceRelation.GetAsync(bioGuid, deviceGuid, token, tranName);
	}

	public static Task<BioDeviceRelationBusiness> GetByPersonAsync(string contextGuid, Guid deviceGuid, Guid personGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.BioDeviceRelation.GetByPersonAsync(deviceGuid, personGuid, token, tranName);
	}

	public static Task<BioDeviceRelationBusiness> GetAsync(string contextGuid, Guid deviceGuid, int hardwareCode, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.BioDeviceRelation.GetAsync(deviceGuid, hardwareCode, token, tranName);
	}

	public static Task<IEnumerable<BioDeviceRelationBusiness>> GetAllAsync(string contextGuid, BioDeviceRelationFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.BioDeviceRelation.GetAllAsync(filter, token, tranName);
	}

	public static Task<IEnumerable<BioDeviceRelationModel>> GetAllAsync(string contextGuid, Guid deviceGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.BioDeviceRelation.GetAllAsync(deviceGuid, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(CancellationToken token, DateTime insertedTime, EnDeviceMessageType messageType, string hardwareName = "", Guid? userGuid = null, bool awaitOnSemaphore = true, string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioDeviceRelationBusiness.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			bool transactionHasBeenCreated = false;
			try
			{
				_ = 7;
				ReturnedSaveFuncInfo result;
				try
				{
					using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "BioDeviceRelationBusiness/SaveAsync", 60000, token))
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (dis.Token.IsCancellationRequested)
						{
							result = res.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioDeviceRelationBusiness.cs", 52);
						}
						else
						{
							if (autoTran)
							{
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AttDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioDeviceRelationBusiness.cs", 53), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioDeviceRelationBusiness.cs", 53);
							}
							transactionHasBeenCreated = true;
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(dis.Token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioDeviceRelationBusiness.cs", 56);
							if (res.HasError)
							{
								result = res;
							}
							else
							{
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.BioDeviceRelation.SaveAsync(this, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioDeviceRelationBusiness.cs", 59);
								if (res.HasError)
								{
									result = res;
								}
								else
								{
									if (!string.IsNullOrEmpty(hardwareName))
									{
										goto IL_05ec;
									}
									if (BioGuid == Guid.Empty)
									{
										result = res;
									}
									else
									{
										BiometricBusiness biometricBusiness = await BiometricBusiness.GetAsync(ContextGuid, BioGuid, token, tranName);
										if ((biometricBusiness?.PersonGuid ?? Guid.Empty) == Guid.Empty)
										{
											result = res;
										}
										else
										{
											AttPersonBusiness attPersonBusiness = await AttPersonBusiness.GetAsync(ContextGuid, biometricBusiness.PersonGuid.Value, token, tranName);
											if (attPersonBusiness != null)
											{
												hardwareName = attPersonBusiness.LatinFullName;
												goto IL_05ec;
											}
											result = res;
										}
									}
								}
							}
						}
						goto end_IL_009c;
						IL_05ec:
						BioWriteDeviceBusiness obj = await BioWriteDeviceBusiness.GenerateNewAsync(ContextGuid, insertedTime, messageType, this, hardwareName, userGuid);
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await obj.SaveAsync(token, awaitOnSemaphore: false, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioDeviceRelationBusiness.cs", 75);
					}
					goto end_IL_008b;
					end_IL_009c:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioDeviceRelationBusiness.cs", 79);
					throw;
				}
				return result;
				end_IL_008b:;
			}
			finally
			{
				if (autoTran && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioDeviceRelationBusiness.cs", 85), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioDeviceRelationBusiness.cs", 85);
				}
			}
			return res;
		}
	}

	private async Task<ReturnedSaveFuncInfo> CheckValidationAsync(CancellationToken token)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (HardwareCode <= 0)
			{
				await res.AddErrorAsync("کد سخت افزاری معتبر نمی باشد", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioDeviceRelationBusiness.cs", 94);
			}
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioDeviceRelationBusiness.cs", 96);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioDeviceRelationBusiness.cs", 99);
			res.AddReturnedValue(ex2, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioDeviceRelationBusiness.cs", 100);
		}
		return res;
	}

	internal async Task<ReturnedSaveFuncInfo> RemoveAsync(bool awaitOnSemaphore, CancellationToken token, string tranName)
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		using (new JaegerMonitor("RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioDeviceRelationBusiness.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			bool transactionHasBeenCreated = false;
			try
			{
				_ = 2;
				ReturnedSaveFuncInfo result;
				try
				{
					using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "BioDeviceRelationBusiness/RemoveAsync", 300000, token))
					{
						if (!dis.Token.IsCancellationRequested)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo;
							if (autoTran)
							{
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AttDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioDeviceRelationBusiness.cs", 115), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioDeviceRelationBusiness.cs", 115);
							}
							transactionHasBeenCreated = true;
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.BioDeviceRelation.RemoveAsync(BioGuid, DeviceGuid, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioDeviceRelationBusiness.cs", 117);
							goto end_IL_013a;
						}
						result = res.AddErrorTokenIsCancelled("", "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioDeviceRelationBusiness.cs", 114);
						goto end_IL_009c;
						end_IL_013a:;
					}
					goto end_IL_008b;
					end_IL_009c:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioDeviceRelationBusiness.cs", 121);
					throw;
				}
				return result;
				end_IL_008b:;
			}
			finally
			{
				if (autoTran && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioDeviceRelationBusiness.cs", 127), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioDeviceRelationBusiness.cs", 127);
				}
			}
			return res;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(string contextGuid, List<BioDeviceRelationBusiness> list, Guid userGuid, EnDeviceMessageType messageType, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioDeviceRelationBusiness.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			bool transactionHasBeenCreated = false;
			try
			{
				_ = 4;
				ReturnedSaveFuncInfo result;
				try
				{
					if (!(list?.Any() ?? false))
					{
						throw new ArgumentNullException();
					}
					using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "BioDeviceRelationBusiness/SaveRangeAsync", 300000, token))
					{
						if (!dis.Token.IsCancellationRequested)
						{
							if (autoTran)
							{
								ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AttDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioDeviceRelationBusiness.cs", 143), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioDeviceRelationBusiness.cs", 143);
							}
							transactionHasBeenCreated = true;
							foreach (BioDeviceRelationBusiness item in list)
							{
								if (item == null)
								{
									continue;
								}
								BioDeviceRelationBusiness bioDeviceRelationBusiness = await GetAsync(contextGuid, item.DeviceGuid, item.HardwareCode, token, tranName);
								if (bioDeviceRelationBusiness == null || !(bioDeviceRelationBusiness.BioGuid != item.BioGuid))
								{
									goto IL_0407;
								}
								ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await bioDeviceRelationBusiness.RemoveAsync(awaitOnSemaphore: false, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioDeviceRelationBusiness.cs", 152);
								if (!res.HasError)
								{
									goto IL_0407;
								}
								result = res;
								goto end_IL_0095;
								IL_0407:
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(token, DateTime.Now.AddMinutes(5.0), messageType, "", userGuid, awaitOnSemaphore: false, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioDeviceRelationBusiness.cs", 155);
								if (!res.HasError)
								{
									continue;
								}
								result = res;
								goto end_IL_0095;
							}
							goto end_IL_0148;
						}
						result = res.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioDeviceRelationBusiness.cs", 142);
						goto end_IL_0095;
						end_IL_0148:;
					}
					goto end_IL_0084;
					end_IL_0095:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioDeviceRelationBusiness.cs", 161);
					throw;
				}
				return result;
				end_IL_0084:;
			}
			finally
			{
				if (autoTran && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioDeviceRelationBusiness.cs", 167), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioDeviceRelationBusiness.cs", 167);
				}
				BioWriteDeviceBusiness.ForceSendToService();
			}
			return res;
		}
	}
}
