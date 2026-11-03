using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Mapping.BusinessMappers.Attendance;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Attendance;
using PacketParser.Entities;
using PacketParser.Entities.Attendance.Biometric;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Attendance.Biometric;

namespace EntityCache.Business.Attendance;

public class BiometricBusiness : IBiometric, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private List<BiometricDetailBusiness> _detailList;

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[Obsolete("", true)]
	public bool Status { get; set; }

	public string ContextGuid { get; set; }

	public EnDeviceGroup DeviceGroup { get; set; }

	public Guid? PersonGuid { get; set; }

	public short FingerPrintCount => checked((short)(DetailList?.Count((BiometricDetailBusiness q) => q.EnrolType == EnrolType.FingerPrint) ?? 0));

	public bool HasPalm => DetailList?.Any((BiometricDetailBusiness q) => q.EnrolType == EnrolType.Palm) ?? false;

	public bool HasFace => DetailList?.Any((BiometricDetailBusiness q) => q.EnrolType == EnrolType.Face) ?? false;

	public bool HasCard => DetailList?.Any((BiometricDetailBusiness q) => q.EnrolType == EnrolType.Card) ?? false;

	public bool HasPassword => DetailList?.Any((BiometricDetailBusiness q) => q.EnrolType == EnrolType.Password) ?? false;

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public IReadOnlyCollection<BiometricDetailBusiness> DetailList
	{
		get
		{
			if (_detailList == null)
			{
				_detailList = new List<BiometricDetailBusiness>();
			}
			_detailList.RemoveAll((BiometricDetailBusiness q) => q?.IsDisposed ?? true);
			return _detailList;
		}
	}

	public BiometricBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static Task<BiometricBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.BioMetricData.GetAsync(guid, token, tranName);
	}

	internal static Task<BiometricBusiness> GetAsync(string contextGuid, Guid personGuid, EnDeviceGroup deviceGroup, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.BioMetricData.GetAsync(personGuid, deviceGroup, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(CancellationToken token, bool awaitOnSemaphore = true, string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			bool transactionHasBeenCreated = false;
			try
			{
				_ = 3;
				ReturnedSaveFuncInfo result;
				try
				{
					using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "BiometricBusiness/SaveAsync", 60000, token))
					{
						if (dis.Token.IsCancellationRequested)
						{
							result = res.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 65);
						}
						else
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo;
							if (autoTran)
							{
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AttDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 66), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 66);
							}
							transactionHasBeenCreated = true;
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(dis.Token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 68);
							if (!res.HasError)
							{
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.BioMetricData.SaveAsync(this, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 70);
								goto end_IL_0140;
							}
							result = res;
						}
						goto end_IL_009c;
						end_IL_0140:;
					}
					goto end_IL_008b;
					end_IL_009c:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 74);
					res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 75);
					goto end_IL_008b;
				}
				return result;
				end_IL_008b:;
			}
			finally
			{
				if (autoTran && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 80), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 80);
				}
			}
			return res;
		}
	}

	private Task<ReturnedSaveFuncInfo> CheckValidationAsync(CancellationToken token)
	{
		return Task.FromResult(new ReturnedSaveFuncInfo());
	}

	internal static async Task<ReturnedSaveFuncInfo> SaveAsync(string contextGuid, HardWareCodeCustomerModel hardwareCodeCus, Guid userGuid, CancellationToken token, bool awaitOnSemaphore = true, string tranName = "")
	{
		if (hardwareCodeCus == null)
		{
			throw new ArgumentException();
		}
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			bool transactionHasBeenCreated = false;
			try
			{
				_ = 4;
				ReturnedSaveFuncInfo result;
				try
				{
					using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "BiometricBusiness/SaveAsync", 60000, token))
					{
						if (dis.Token.IsCancellationRequested)
						{
							result = res.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 110);
						}
						else
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo;
							if (autoTran)
							{
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AttDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 111), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 111);
							}
							transactionHasBeenCreated = true;
							BiometricBusiness biometricBusiness = await GetAsync(contextGuid, hardwareCodeCus.PersonGuid, hardwareCodeCus.DeviceType.GetDeviceGroup(), token, tranName);
							if (biometricBusiness == null)
							{
								biometricBusiness = new BiometricBusiness(contextGuid)
								{
									ContextGuid = contextGuid,
									Guid = Guid.NewGuid(),
									DeviceGroup = hardwareCodeCus.DeviceType.GetDeviceGroup(),
									Modified = DateTime.Now,
									PersonGuid = hardwareCodeCus.PersonGuid
								};
							}
							BioDeviceRelationBusiness deviceRelation = new BioDeviceRelationBusiness(contextGuid)
							{
								BioGuid = biometricBusiness.Guid,
								ContextGuid = contextGuid,
								DeviceGuid = hardwareCodeCus.DeviceGuid,
								HardwareCode = hardwareCodeCus.HardWareCode.ParseToInt(),
								Modified = DateTime.Now,
								Status = true,
								Privilage = EnAttendancePrivilage.Person
							};
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await biometricBusiness.SaveAsync(token, awaitOnSemaphore: false, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 134);
							if (!res.HasError)
							{
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await deviceRelation.SaveAsync(token, DateTime.Now, EnDeviceMessageType.AddOrUpdate, hardwareCodeCus.HardWareName, userGuid, awaitOnSemaphore: false, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 136);
								goto end_IL_014b;
							}
							result = res;
						}
						goto end_IL_00a3;
						end_IL_014b:;
					}
					goto end_IL_0092;
					end_IL_00a3:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 140);
					res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 141);
					goto end_IL_0092;
				}
				return result;
				end_IL_0092:;
			}
			finally
			{
				if (autoTran && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 146), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 146);
				}
			}
			return res;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(string contextGuid, List<HardWareCodeCustomerModel> lst, Guid userGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (!(lst?.Any() ?? false))
			{
				return res;
			}
			foreach (HardWareCodeCustomerModel item in lst)
			{
				if (token.IsCancellationRequested)
				{
					return res.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 158);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(contextGuid, item, userGuid, token, awaitOnSemaphore: true, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 159);
			}
			BioWriteDeviceBusiness.ForceSendToService();
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 163);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 166);
			res.AddReturnedValue(ex2, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 167);
		}
		return res;
	}

	public Task AddDetailAsync(BiometricDetailBusiness det)
	{
		try
		{
			if (det == null)
			{
				return Task.CompletedTask;
			}
			if (_detailList == null)
			{
				_detailList = new List<BiometricDetailBusiness>();
			}
			det.MasterGuid = Guid;
			_detailList.Add(det);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 180);
		}
		return Task.CompletedTask;
	}

	public async Task AddDetailListAsync(List<BiometricDetailBusiness> list)
	{
		try
		{
			if (!(list?.Any() ?? false))
			{
				return;
			}
			foreach (BiometricDetailBusiness item in list)
			{
				await AddDetailAsync(item);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddDetailListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 190);
		}
	}

	public Task RemoveDetailAsync(Guid detGuid)
	{
		try
		{
			BiometricDetailBusiness biometricDetailBusiness = _detailList?.SingleOrDefault((BiometricDetailBusiness q) => q.Guid == detGuid);
			if (biometricDetailBusiness == null)
			{
				return Task.CompletedTask;
			}
			biometricDetailBusiness.Dispose(disposing: true);
			_detailList.Remove(biometricDetailBusiness);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 201);
		}
		return Task.CompletedTask;
	}

	public void ClearDetailList()
	{
		_detailList?.Clear();
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(string contextGuid, List<WebBiometricReadMaster> list, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				if (!(list?.Any() ?? false))
				{
					res.AddError("BioMetric Data sync received but data is null!", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 213);
					return res;
				}
				foreach (WebBiometricReadMaster item in list)
				{
					if (item == null)
					{
						continue;
					}
					BioDeviceRelationBusiness bioDeviceRelationBusiness = await BioDeviceRelationBusiness.GetAsync(contextGuid, item.DeviceGuid, item.HardwareCode, token);
					if ((bioDeviceRelationBusiness?.BioGuid ?? Guid.Empty) == Guid.Empty)
					{
						continue;
					}
					BiometricBusiness bioData = await GetAsync(contextGuid, bioDeviceRelationBusiness.BioGuid, token);
					if (bioData == null)
					{
						continue;
					}
					bioData.ClearDetailList();
					List<BiometricDetailBusiness> list2 = await BiometricDetailMapper.Instance.MapListAsync(contextGuid, item.Details, new List<BiometricDetailBusiness>(), "", token);
					foreach (BiometricDetailBusiness item2 in list2)
					{
						item2.MasterGuid = bioData.Guid;
					}
					await bioData.AddDetailListAsync(list2);
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await bioData.SaveAsync(token), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 233);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 238);
				res.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 239);
			}
			return res;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> ChangeHardwareAsync(string contextGuid, Guid deviceGuid, Guid? personGuid, int newHardwareCode, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		using (new JaegerMonitor("ChangeHardwareAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			bool transactionHasBeenCreated = false;
			try
			{
				_ = 15;
				ReturnedSaveFuncInfo result;
				try
				{
					using SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "BiometricBusiness/ChangeHardwareAsync", 60000, token);
					DeviceInfoBusiness device;
					ReturnedSaveFuncInfo returnedSaveFuncInfo;
					if (dis.Token.IsCancellationRequested)
					{
						result = res.AddErrorTokenIsCancelled("", "", "ChangeHardwareAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 253);
					}
					else
					{
						if (autoTran)
						{
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AttDatabaseManager.BeginTransactionAsync(tranName, 60000, "ChangeHardwareAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 254), ReturnedState.None, "ChangeHardwareAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 254);
						}
						transactionHasBeenCreated = true;
						device = await DeviceInfoBusiness.GetAsync(contextGuid, deviceGuid, token, tranName);
						if (newHardwareCode <= 0)
						{
							goto IL_0444;
						}
						BioDeviceRelationBusiness bioDeviceRelationBusiness = await BioDeviceRelationBusiness.GetAsync(contextGuid, deviceGuid, newHardwareCode, token, tranName);
						if (bioDeviceRelationBusiness == null)
						{
							goto IL_0444;
						}
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await bioDeviceRelationBusiness.RemoveAsync(awaitOnSemaphore: true, token, tranName), ReturnedState.None, "ChangeHardwareAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 263);
						if (!res.HasError)
						{
							goto IL_0444;
						}
						result = res;
					}
					goto end_IL_0167;
					IL_0735:
					result = res;
					goto end_IL_0167;
					IL_0c3e:
					BioDeviceRelationBusiness existRelation2;
					existRelation2.HardwareCode = newHardwareCode;
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await existRelation2.SaveAsync(token, DateTime.Now.AddSeconds(10.0), EnDeviceMessageType.AddOrUpdate, "", null, awaitOnSemaphore: true, tranName), ReturnedState.None, "ChangeHardwareAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 332);
					goto end_IL_0087;
					IL_0444:
					existRelation2 = null;
					if ((personGuid ?? Guid.Empty) != Guid.Empty)
					{
						existRelation2 = await BioDeviceRelationBusiness.GetByPersonAsync(contextGuid, deviceGuid, personGuid.Value, token, tranName);
					}
					if (newHardwareCode <= 0)
					{
						if (existRelation2 == null)
						{
							goto IL_0735;
						}
						BioWriteDeviceBusiness obj = await BioWriteDeviceBusiness.GenerateNewAsync(contextGuid, DateTime.Now.AddSeconds(-10.0), EnDeviceMessageType.Delete, existRelation2);
						obj.BioGuid = existRelation2.BioGuid;
						obj.HarwareName = "";
						obj.HardwareCode = existRelation2.HardwareCode;
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await obj.SaveAsync(token, awaitOnSemaphore: true, tranName), ReturnedState.None, "ChangeHardwareAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 280);
						if (!res.HasError)
						{
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await existRelation2.RemoveAsync(awaitOnSemaphore: true, token, tranName), ReturnedState.None, "ChangeHardwareAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 282);
							goto IL_0735;
						}
						result = res;
					}
					else
					{
						if (existRelation2 != null)
						{
							if (existRelation2.HardwareCode != newHardwareCode)
							{
								BioWriteDeviceBusiness obj2 = await BioWriteDeviceBusiness.GenerateNewAsync(contextGuid, DateTime.Now.AddSeconds(-10.0), EnDeviceMessageType.Delete, existRelation2);
								obj2.BioGuid = existRelation2.BioGuid;
								obj2.HarwareName = "";
								obj2.HardwareCode = existRelation2.HardwareCode;
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await obj2.SaveAsync(token, awaitOnSemaphore: true, tranName), ReturnedState.None, "ChangeHardwareAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 295);
							}
							goto IL_0c3e;
						}
						existRelation2 = await BioDeviceRelationBusiness.GetAsync(contextGuid, deviceGuid, newHardwareCode, token, tranName);
						if (existRelation2 != null)
						{
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await existRelation2.RemoveAsync(awaitOnSemaphore: true, token, tranName), ReturnedState.None, "ChangeHardwareAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 303);
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: false, "ChangeHardwareAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 304), ReturnedState.None, "ChangeHardwareAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 304);
						}
						if (res.HasError)
						{
							result = res;
						}
						else
						{
							BiometricBusiness bio = new BiometricBusiness(contextGuid)
							{
								Guid = Guid.NewGuid(),
								PersonGuid = personGuid,
								DeviceGroup = device.Model.GetDeviceGroup()
							};
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await bio.SaveAsync(token, awaitOnSemaphore: false, tranName), ReturnedState.None, "ChangeHardwareAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 315);
							if (!res.HasError)
							{
								existRelation2 = new BioDeviceRelationBusiness(contextGuid)
								{
									BioGuid = bio.Guid,
									ContextGuid = contextGuid,
									DeviceGuid = deviceGuid,
									HardwareCode = newHardwareCode,
									Modified = DateTime.Now,
									Status = true,
									Privilage = EnAttendancePrivilage.Person
								};
								goto IL_0c3e;
							}
							result = res;
						}
					}
					end_IL_0167:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "ChangeHardwareAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 336);
					res.AddReturnedValue(ex, "", "ChangeHardwareAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 337);
					goto end_IL_0087;
				}
				return result;
				end_IL_0087:;
			}
			finally
			{
				if (autoTran && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "ChangeHardwareAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 342), ReturnedState.None, "ChangeHardwareAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BiometricBusiness.cs", 342);
				}
			}
			return res;
		}
	}
}
