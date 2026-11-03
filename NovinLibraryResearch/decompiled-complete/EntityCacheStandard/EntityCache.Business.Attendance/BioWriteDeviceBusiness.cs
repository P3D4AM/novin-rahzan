using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ClientHub;
using EntityCache.Mapping.BusinessMappers.Attendance;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Attendance;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.Entities.CompositeKeys;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Attendance.Biometric;
using PacketParser.EventArgs;
using PacketParser.EventArgs.Attendance;
using PacketParser.Filters.Attendance;
using PacketParser.TransferLayer.Waiters.Attendance.AttpersonLeave;
using Serilog;

namespace EntityCache.Business.Attendance;

public class BioWriteDeviceBusiness : Serializable<BioWriteDeviceBusiness>, IBioWriteDevice, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private static TaskCompletionSource<bool> _newTaskAvailable = new TaskCompletionSource<bool>();

	private SemaphoreSlim _sendToServerSemaphore = new SemaphoreSlim(1, 1);

	private CancellationTokenSource _sendToken = new CancellationTokenSource();

	public Guid BioGuid { get; set; }

	public Guid DeviceGuid { get; set; }

	public int HardwareCode { get; set; }

	public string HarwareName { get; set; }

	public DateTime InsertedDateM { get; set; }

	public short TryCount { get; set; }

	public DateTime NextTry { get; set; }

	public Guid UserGuid { get; set; }

	public string Message { get; set; }

	public string ContextGuid { get; set; }

	public EnDeviceMessageType MessageType { get; set; }

	private BioWriteDeviceBusiness EventManager => ClsCache.GetCache(ContextGuid).BusinessAggregator.BioWriteDevice;

	public event Func<object, SaveResponse<BioWriteDeviceCompositeKey>, Task> OnSaveResult;

	private BioWriteDeviceBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static Task<BioWriteDeviceBusiness> GenerateNewAsync(string contextGuid, DateTime inseredTime, EnDeviceMessageType messageType, BioDeviceRelationBusiness relation = null, string hardwareName = "", Guid? userGuid = null)
	{
		try
		{
			BioWriteDeviceBusiness bioWriteDeviceBusiness = new BioWriteDeviceBusiness(contextGuid)
			{
				InsertedDateM = inseredTime,
				TryCount = 0,
				NextTry = DateTime.Now,
				HarwareName = hardwareName,
				MessageType = messageType
			};
			if ((userGuid ?? Guid.Empty) != Guid.Empty)
			{
				bioWriteDeviceBusiness.UserGuid = userGuid.Value;
			}
			if (relation != null)
			{
				bioWriteDeviceBusiness.BioGuid = relation.BioGuid;
				bioWriteDeviceBusiness.DeviceGuid = relation.DeviceGuid;
				bioWriteDeviceBusiness.HardwareCode = relation.HardwareCode;
			}
			return Task.FromResult(bioWriteDeviceBusiness);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceBusiness.cs", 67);
			throw;
		}
	}

	public static Task<IEnumerable<BioWriteDeviceModel>> GetAllAync(string contextGuid, CancellationToken token, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.BioWriteDevice.GetAllAync(contextGuid, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(CancellationToken token, bool awaitOnSemaphore = true, string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceBusiness.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			bool transactionHasBeenCreated = false;
			try
			{
				_ = 3;
				ReturnedSaveFuncInfo result;
				try
				{
					using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "BioWriteDeviceBusiness/SaveAsync", 60000, token))
					{
						if (dis.Token.IsCancellationRequested)
						{
							result = res.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceBusiness.cs", 83);
						}
						else
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo;
							if (autoTran)
							{
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AttDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceBusiness.cs", 84), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceBusiness.cs", 84);
							}
							transactionHasBeenCreated = true;
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.BioWriteDevice.SaveAsync(this, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceBusiness.cs", 86);
							if (!res.HasError)
							{
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await BioWriteDeviceLogBusiness.GenerateAndSaveLogAsync(ContextGuid, this, ReturnedState.None, "", token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceBusiness.cs", 88);
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
					WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceBusiness.cs", 92);
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceBusiness.cs", 98), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceBusiness.cs", 98);
				}
				if (EventManager?.OnSaveResult != null)
				{
					PacketParser.Utilities.RaiseEvent(EventManager.OnSaveResult, this, null, default(CancellationToken), 60, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceBusiness.cs");
				}
			}
			return res;
		}
	}

	internal static Task<BioWriteDeviceBusiness> FetchNextForSendAsync(string contextGuid, Guid deviceGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.BioWriteDevice.FetchNextForSendAsync(deviceGuid, token, tranName);
	}

	internal async Task<ReturnedSaveFuncInfo> FetchAndSendDataToServiceAsync(ClientHub.Buffer buffer, Guid deviceGuid, CancellationToken token)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (deviceGuid == Guid.Empty)
			{
				return res;
			}
			await Task.Delay(new Random().Next(500, 1000));
			if (base.IsDisposed || token.IsCancellationRequested)
			{
				return res;
			}
			BioWriteDeviceBusiness bioWriteDeviceBusiness = await FetchNextForSendAsync(ContextGuid, deviceGuid, token);
			while (bioWriteDeviceBusiness != null && !base.IsDisposed && !token.IsCancellationRequested)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await bioWriteDeviceBusiness.SendToServiceAsync(buffer, token), ReturnedState.None, "FetchAndSendDataToServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceBusiness.cs", 118);
				if (base.IsDisposed || token.IsCancellationRequested || res.HasError)
				{
					return res;
				}
				await Task.Delay(new Random().Next(500, 1000));
				if (base.IsDisposed || token.IsCancellationRequested)
				{
					return res;
				}
				bioWriteDeviceBusiness = await FetchNextForSendAsync(ContextGuid, deviceGuid, token);
			}
			await SendDisconnectRequestAsync(buffer, deviceGuid, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FetchAndSendDataToServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceBusiness.cs", 128);
			res.AddReturnedValue(ex, "", "FetchAndSendDataToServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceBusiness.cs", 129);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse<BioWriteDeviceCompositeKey> item, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AttDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceBusiness.cs", 141), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceBusiness.cs", 141);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.BioWriteDevice.SetSaveResult(item, token, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceBusiness.cs", 143);
			}
			catch (OperationCanceledException)
			{
				res.AddErrorTokenIsCancelled("", "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceBusiness.cs", 145);
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceBusiness.cs", 148);
				res.AddReturnedValue(ex2, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceBusiness.cs", 149);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceBusiness.cs", 154), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceBusiness.cs", 154);
			}
			if (EventManager?.OnSaveResult != null)
			{
				PacketParser.Utilities.RaiseEvent(EventManager.OnSaveResult, this, item, default(CancellationToken), 60, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceBusiness.cs");
			}
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> SendToServiceAsync(ClientHub.Buffer buffer, CancellationToken token, Priority priority = Priority.Low)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SendToServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceBusiness.cs"))
		{
			_ = 4;
			try
			{
				_sendToken?.Cancel();
				_sendToken = new CancellationTokenSource();
				CancellationToken token2 = _sendToken.Token;
				using SemaphoreUtil dis = await _sendToServerSemaphore.GaurdAsync(true, "BioWriteDeviceBusiness/SendToServiceAsync", 60000, token, token2);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendToServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceBusiness.cs", 171);
				}
				WebBioWriteDevice item = await BioWriteDeviceMapper.Instance.MapAsync(ContextGuid, this, new WebBioWriteDevice(), "", token);
				List<Packet> paks = WebBioWriteDevice.GenerateSaveRequest(new List<WebBioWriteDevice> { item });
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendToServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceBusiness.cs", 175);
				}
				WebContextHolder webContextHolder = WebContextHolder.GetWebContextHolder(ContextGuid);
				BioWriteDeviceCompositeKey key = new BioWriteDeviceCompositeKey
				{
					BioGuid = BioGuid,
					DeviceGuid = DeviceGuid,
					MessageType = MessageType
				};
				using AttBioWriteDeviceWaiters waiter = new AttBioWriteDeviceWaiters(webContextHolder, key, dis.Token);
				Task.Run(() => buffer.SendData(paks, priority), token);
				ReturnedSaveFuncInfoWithValue<SaveResponse<BioWriteDeviceCompositeKey>> saveRespose = await waiter.WaitForSaveResponse();
				ret.AddReturnedValue(saveRespose, ReturnedState.None, "SendToServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceBusiness.cs", 188);
				if (!ret.HasError && saveRespose?.value != null)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SetSaveResultAsync(saveRespose.value, token), ReturnedState.None, "SendToServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceBusiness.cs", 191);
					if (!saveRespose.value.SaveResultStatus && saveRespose.value.Key != null)
					{
						string message = "Error For Send Biometric to Service \r\n" + $"BioGuid:{saveRespose.value.Key.BioGuid} \r\n" + $"DeviceGuid:{saveRespose.value.Key.DeviceGuid} \r\n" + "Message:" + saveRespose.value.Message;
						await ret.AddErrorAsync(message, "", "SendToServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceBusiness.cs", 199);
						Log.Error(message);
					}
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SendToServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceBusiness.cs", 204);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SendToServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceBusiness.cs", 207);
				ret.AddReturnedValue(ex, "", "SendToServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceBusiness.cs", 208);
			}
			return ret;
		}
	}

	public async Task InitAsync(ClientHub.Buffer buffer, EnDeviceGroup group, CancellationToken token = default(CancellationToken))
	{
		while (!base.IsDisposed)
		{
			try
			{
				await Task.Delay(new Random().Next(500, 1000));
				if (buffer == null)
				{
					break;
				}
				List<DeviceModelEnumType> deviceModelList = group.GetDeviceModelList();
				if (deviceModelList == null || !deviceModelList.Any())
				{
					break;
				}
				List<DeviceInfoBusiness> list = await DeviceInfoBusiness.GetAllAsync(ContextGuid, new DeviceInfoFilter
				{
					ModelList = deviceModelList
				}, "", token);
				if (list == null || !list.Any())
				{
					await Task.WhenAny(new Task[2]
					{
						Task.Delay(300000),
						_newTaskAvailable.Task
					});
					continue;
				}
				ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
				list = list.OrderBy((DeviceInfoBusiness q) => q.DeviceNo).ToList();
				foreach (DeviceInfoBusiness item in list)
				{
					if (!((item?.Guid ?? Guid.Empty) == Guid.Empty))
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await FetchAndSendDataToServiceAsync(buffer, item.Guid, token), ReturnedState.None, "InitAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceBusiness.cs", 235);
					}
				}
				if (!res.HasError)
				{
					await Task.WhenAny(new Task[2]
					{
						Task.Delay(300000),
						_newTaskAvailable.Task
					});
				}
				else
				{
					await Task.WhenAny(new Task[2]
					{
						Task.Delay(60000),
						_newTaskAvailable.Task
					});
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "InitAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceBusiness.cs", 241);
			}
		}
	}

	public static void ForceSendToService()
	{
		try
		{
			_newTaskAvailable?.TrySetResult(result: true);
			_newTaskAvailable = new TaskCompletionSource<bool>();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ForceSendToService", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceBusiness.cs", 251);
		}
	}

	private async Task SendDisconnectRequestAsync(ClientHub.Buffer buffer, Guid deviceGuid, CancellationToken token)
	{
		try
		{
			if (buffer == null)
			{
				return;
			}
			DeviceInfoBusiness deviceInfoBusiness = await DeviceInfoBusiness.GetAsync(ContextGuid, deviceGuid, token);
			if (deviceInfoBusiness == null)
			{
				return;
			}
			Packet pack = WebBioWriteDevice.GenerateDisconnectRequest(new MicroDeviceInfo(deviceGuid, deviceInfoBusiness.Ip, deviceInfoBusiness.TcpPort, deviceInfoBusiness.DeviceNo, deviceInfoBusiness.CommunicationPass.ParseToInt(), deviceInfoBusiness.Model.GetDeviceGroup()));
			WebContextHolder webContextHolder = WebContextHolder.GetWebContextHolder(ContextGuid);
			using AttBioWriteDeviceWaiters waiter = new AttBioWriteDeviceWaiters(webContextHolder, null, token);
			Task.Run(() => buffer.SendData(pack, Priority.High), token);
			ReturnedSaveFuncInfoWithValue<BioMetricDisconnectEventArgs> returnedSaveFuncInfoWithValue = await waiter.WaitForDisconnectResponse();
			if (returnedSaveFuncInfoWithValue != null && returnedSaveFuncInfoWithValue.HasError)
			{
				Log.Error(returnedSaveFuncInfoWithValue.ErrorMessage);
			}
			if ((returnedSaveFuncInfoWithValue?.value?.Result?.HasError).GetValueOrDefault())
			{
				Log.Error(returnedSaveFuncInfoWithValue.value.Result.ErrorMessage);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendDisconnectRequestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\Biometric\\BioWriteDeviceBusiness.cs", 272);
		}
	}
}
