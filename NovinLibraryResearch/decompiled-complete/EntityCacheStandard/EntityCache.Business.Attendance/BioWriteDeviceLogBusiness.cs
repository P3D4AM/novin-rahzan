using System;
using System.Collections.Generic;
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

public class BioWriteDeviceLogBusiness : IBioWriteDeviceLog, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	[Obsolete("", true)]
	public bool Status { get; set; }

	public DateTime DateM { get; set; }

	public string Description { get; set; }

	public Guid DeviceGuid { get; set; }

	public Guid? BioGuid { get; set; }

	public int HardwareCode { get; set; }

	public string HardwareName { get; set; }

	public ReturnedState Result { get; set; }

	public Guid? UserGuid { get; set; }

	public BiometricBusiness BioInfo { get; set; }

	public string ContextGuid { get; set; }

	public BioWriteDeviceLogBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static Task<IEnumerable<BioWriteDeviceLogModel>> GetAllAsync(string contextGuid, BioWriteDeviceLogFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.BioWriteDeviceLog.GetAllAsync(filter, token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> GenerateAndSaveLogAsync(string contextGuid, BioWriteDeviceBusiness data, ReturnedState result, string userDesc, CancellationToken token, string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (data == null)
			{
				return null;
			}
			BioWriteDeviceLogBusiness item = new BioWriteDeviceLogBusiness(contextGuid)
			{
				BioGuid = data.BioGuid,
				ContextGuid = contextGuid,
				DateM = DateTime.Now,
				DeviceGuid = data.DeviceGuid,
				Guid = Guid.NewGuid(),
				HardwareCode = data.HardwareCode,
				HardwareName = data.HarwareName,
				Modified = DateTime.Now,
				Result = result,
				UserGuid = data.UserGuid
			};
			string personName = "";
			string deviceName = "";
			if ((data?.BioGuid ?? Guid.Empty) != Guid.Empty)
			{
				BioWriteDeviceLogBusiness bioWriteDeviceLogBusiness = item;
				bioWriteDeviceLogBusiness.BioInfo = await BiometricBusiness.GetAsync(contextGuid, data.BioGuid, token, tranName);
				if ((item?.BioInfo?.PersonGuid ?? Guid.Empty) != Guid.Empty)
				{
					personName = (await AttPersonBusiness.GetAsync(contextGuid, item.BioInfo.PersonGuid.Value, token, tranName))?.FullName;
				}
			}
			if (item.DeviceGuid != Guid.Empty)
			{
				deviceName = (await DeviceInfoBusiness.GetAsync(contextGuid, item.DeviceGuid, token, tranName))?.DeviceName;
			}
			if (result == ReturnedState.None && data.MessageType == EnDeviceMessageType.AddOrUpdate)
			{
				item.Description = "ارسال " + personName + " به دستگاه " + deviceName;
			}
			if (result == ReturnedState.None && data.MessageType == EnDeviceMessageType.Delete)
			{
				item.Description = $"ارسال دستور تخلیه حافظه {item.HardwareCode} متعلق به {personName} در دستگاه {deviceName}";
			}
			if (result == ReturnedState.Success && data.MessageType == EnDeviceMessageType.AddOrUpdate)
			{
				item.Description = "تحویل موفق " + personName + " به دستگاه " + deviceName;
			}
			if (result == ReturnedState.Success && data.MessageType == EnDeviceMessageType.Delete)
			{
				item.Description = $"تخلیه موفق حافظه {item.HardwareCode} متعلق به {item.HardwareName} در دستگاه {deviceName}";
			}
			if (result == ReturnedState.Error && data.MessageType == EnDeviceMessageType.AddOrUpdate)
			{
				item.Description = "تحویل ناموفق " + personName + " به دستگاه " + deviceName;
			}
			if (result == ReturnedState.Error && data.MessageType == EnDeviceMessageType.Delete)
			{
				item.Description = $"تخلیه ناموفق حافظه {item.HardwareCode} متعلق به {item.HardwareName} در دستگاه {deviceName}";
			}
			if (!string.IsNullOrEmpty(userDesc))
			{
				item.Description = item.Description + "\r\n " + userDesc;
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(token, tranName), ReturnedState.None, "GenerateAndSaveLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceLogBusiness.cs", 92);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateAndSaveLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceLogBusiness.cs", 96);
			res.AddReturnedValue(ex, "", "GenerateAndSaveLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceLogBusiness.cs", 97);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> SaveAsync(CancellationToken token, string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceLogBusiness.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			bool transactionHasBeenCreated = false;
			try
			{
				_ = 3;
				ReturnedSaveFuncInfo result;
				try
				{
					using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "BioWriteDeviceLogBusiness/SaveAsync", 60000, token))
					{
						if (!dis.Token.IsCancellationRequested)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo;
							if (autoTran)
							{
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AttDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceLogBusiness.cs", 112), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceLogBusiness.cs", 112);
							}
							transactionHasBeenCreated = true;
							if ((BioGuid ?? Guid.Empty) != Guid.Empty && BioInfo == null)
							{
								BioInfo = await BiometricBusiness.GetAsync(ContextGuid, BioGuid.Value, token, tranName);
							}
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.BioWriteDeviceLog.SaveAsync(this, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceLogBusiness.cs", 118);
							goto end_IL_013b;
						}
						result = res.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceLogBusiness.cs", 111);
						goto end_IL_009c;
						end_IL_013b:;
					}
					goto end_IL_008b;
					end_IL_009c:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceLogBusiness.cs", 122);
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceLogBusiness.cs", 128), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceLogBusiness.cs", 128);
				}
			}
			return res;
		}
	}
}
