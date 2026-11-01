using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClientHub;
using EntityCache;
using EntityCache.Business;
using EntityCache.Business.Attendance;
using FormCustom.Cache;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Attendance;
using PacketParser.Connection;
using PacketParser.Entities;
using PacketParser.Entities.Attendance.Biometric;
using PacketParser.Entities.CompositeKeys;
using PacketParser.EventArgs;
using PacketParser.Settings.Attendance;
using PacketParser.TransferLayer.Waiters.Attendance.AttpersonLeave;
using TimeAttendanceDAL.Utility;
using TimeAttendanceDAL.Utility.Settings;

namespace TimeAttendanceUI.Forms.Service;

public class DeviceServiceHandler
{
	private CancellationTokenSource ConnecToken = new CancellationTokenSource();

	private CancellationTokenSource _notifToken;

	private Buffer _bufferDevice;

	private CacheBaseForm _owner;

	private static ConcurrentDictionary<string, DeviceServiceHandler> _instanceHolder;

	public Buffer BufferDevice
	{
		get
		{
			return _bufferDevice;
		}
		set
		{
			try
			{
				if (value != null)
				{
					if (_bufferDevice != null)
					{
						_bufferDevice.Connected -= Buffer_Connected;
						_bufferDevice.Disconnected -= Buffer_Disconnected;
						_bufferDevice.Connecting -= Buffer_Connecting;
						_bufferDevice.LoggedIn -= Buffer_LoggedIn;
					}
					_bufferDevice = value;
					_bufferDevice.Connected += Buffer_Connected;
					_bufferDevice.Disconnected += Buffer_Disconnected;
					_bufferDevice.Connecting += Buffer_Connecting;
					_bufferDevice.LoggedIn += Buffer_LoggedIn;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Buffer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\ClsCache.cs", 97);
			}
		}
	}

	public string ContextGuid { get; private set; }

	private DeviceServiceHandler(string contextGuid, CacheBaseForm owner)
	{
		ContextGuid = contextGuid;
		_owner = owner;
	}

	public static DeviceServiceHandler GetInstance(string contextGuid, CacheBaseForm owner)
	{
		try
		{
			if (_instanceHolder == null)
			{
				_instanceHolder = new ConcurrentDictionary<string, DeviceServiceHandler>();
			}
			_instanceHolder.TryGetValue(contextGuid, out var value);
			if (value == null)
			{
				value = new DeviceServiceHandler(contextGuid, owner);
				_instanceHolder.TryAdd(contextGuid, value);
			}
			return value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetInstance", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\DeviceServiceHandler.cs", 86);
			throw;
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<LoginResponseEventArgs>> InitializeCommunicationDevice(string version, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<LoginResponseEventArgs> ret = new ReturnedSaveFuncInfoWithValue<LoginResponseEventArgs>();
		ClsCache cache = ClsCache.GetCache(ContextGuid) ?? throw new ArgumentNullException();
		try
		{
			if (BufferDevice != null)
			{
				await BufferDevice.DisposeAsync();
				BufferDevice = null;
			}
			DeviceServiceSettings val = await LocalSettingBusiness.GetDeviceServiceSettingsAsync(ContextGuid, token, true);
			if (val == null || !val.HandleService)
			{
				return ret;
			}
			long lkSerial = Utilities.ParseToLong(LkOperations.GetLkOperations(ContextGuid).LkSerial(), 0L);
			Guid guid = Guid.NewGuid();
			List<ClientConfig> list = new List<ClientConfig>
			{
				new ClientConfig
				{
					Source = (ENSource)57,
					Version = version,
					CompressedMode = (ENCompressedMode)1,
					LkSerial = lkSerial,
					CpuSerial = Utilities.UniqueIdentifier(),
					Url = val.Url + "BioMetricServerCore",
					UserName = Guid.NewGuid().ToString(),
					Password = Guid.NewGuid().ToString(),
					DbGuid = guid,
					Encryption = (EncryptionType)0
				}
			};
			BufferDevice = new Buffer((ENSource)57, DI.Kernel, WebContextHolder.GetWebContextHolder(ContextGuid), (EnConnectionType)1);
			if (BufferDevice != null)
			{
				cache.InitWebServiceHandlers();
			}
			ConnecToken?.Cancel();
			ConnecToken = new CancellationTokenSource();
			CancellationToken token2 = CancellationTokenSource.CreateLinkedTokenSource(ConnecToken.Token, token).Token;
			Buffer bufferDevice = BufferDevice;
			Guid? guid2 = guid;
			ReturnedSaveFuncInfoWithValue<LoginResponseEventArgs> val2 = await bufferDevice.ConnectAsync((IEnumerable<ClientConfig>)list, (CancellationToken?)token2, guid2, 30000);
			ret.AddReturnedValue((ReturnedSaveFuncInfo)(object)val2, (ReturnedState)0, "InitializeCommunicationDevice", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\DeviceServiceHandler.cs", 132);
			EnLoginResult? obj;
			if (val2 == null)
			{
				obj = null;
			}
			else
			{
				LoginResponseEventArgs value = val2.value;
				obj = ((value != null) ? new EnLoginResult?(value.Status) : null);
			}
			EnLoginResult? val3 = obj;
			if ((int)val3.GetValueOrDefault((EnLoginResult)16) == 1)
			{
				Utility.MessageBoxShowByNotif(showPopUp: false, (NotificationEnum)1, "ارتباط با سرویس دستگاه ها برقرار شد", "", 3);
			}
			else
			{
				Utility.MessageBoxShowByNotif(showPopUp: false, (NotificationEnum)4, "عدم امکان برقراری ارتباط با سرویس دستگاه ها", "", 3);
			}
			if (_owner.IsDisposed || BufferDevice == null)
			{
				return ret;
			}
			((Control)_owner).BeginInvoke((Delegate)(Func<ReturnedSaveFuncInfoWithValue<LoginResponseEventArgs>>)delegate
			{
				if (_owner.IsDisposed || BufferDevice == null)
				{
					return ret;
				}
				try
				{
					((Control)frmDeviceServiceNotify.GetInstance(ContextGuid, BufferDevice)).Show();
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "InitializeCommunicationDevice", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\DeviceServiceHandler.cs", 149);
				}
				return ret;
			});
			cache.BusinessAggregator.BioWriteDevice.OnSaveResult -= BioWriteDevice_OnSaveResult;
			cache.BusinessAggregator.BioWriteDevice.OnSaveResult += BioWriteDevice_OnSaveResult;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "InitializeCommunicationDevice", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\DeviceServiceHandler.cs", 158);
			ret.AddReturnedValue(ex, "", "InitializeCommunicationDevice", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\DeviceServiceHandler.cs", 159);
		}
		return ret;
	}

	private async Task BioWriteDevice_OnSaveResult(object sender, SaveResponse<BioWriteDeviceCompositeKey> e)
	{
		try
		{
			if (_owner == null || e == null)
			{
				return;
			}
			_notifToken?.Cancel();
			_notifToken = new CancellationTokenSource(60000);
			CancellationToken token = _notifToken.Token;
			await Task.Delay(1000);
			DeviceInfoBusiness device = await DeviceInfoBusiness.GetAsync(ContextGuid, e.Key.DeviceGuid, token, "");
			if (device == null)
			{
				return;
			}
			BioDeviceRelationBusiness relation = await BioDeviceRelationBusiness.GetAsync(ContextGuid, e.Key.BioGuid, e.Key.DeviceGuid, token, "");
			if (relation == null)
			{
				return;
			}
			string personNameForDisplay = $"{relation.HardwareCode}";
			BiometricBusiness val = await BiometricBusiness.GetAsync(ContextGuid, e.Key.BioGuid, token, "");
			if ((((val != null) ? val.PersonGuid : null) ?? Guid.Empty) != Guid.Empty)
			{
				AttPersonBusiness val2 = await AttPersonBusiness.GetAsync(ContextGuid, val.PersonGuid.Value, token, "");
				if (!string.IsNullOrEmpty((val2 != null) ? val2.FullName : null))
				{
					personNameForDisplay = $"{val2.FullName}({relation.HardwareCode})";
				}
			}
			if (token.IsCancellationRequested)
			{
				return;
			}
			string data = "ارسال اطلاعات " + personNameForDisplay + " به دستگاه " + device.DeviceName + " ";
			if (((BaseSaveResponse)e).SaveResultStatus)
			{
				data += ".. موفق";
			}
			else
			{
				data += ".. ناموفق";
			}
			while (!((Control)_owner).IsHandleCreated)
			{
				await Task.Delay(100);
				if (token.IsCancellationRequested)
				{
					return;
				}
			}
			if (token.IsCancellationRequested)
			{
				return;
			}
			((Control)_owner).BeginInvoke((Delegate)(Action)delegate
			{
				if (token.IsCancellationRequested)
				{
					return;
				}
				try
				{
					Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)(((BaseSaveResponse)e).SaveResultStatus ? 1 : 4), data, "سرویس ارتباط با دستگاه", 3);
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "BioWriteDevice_OnSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\DeviceServiceHandler.cs", 208);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BioWriteDevice_OnSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\DeviceServiceHandler.cs", 211);
		}
	}

	public async Task SendToServiceAsync(string contextGuid)
	{
		try
		{
			foreach (EnDeviceGroup item in Enum.GetValues(typeof(EnDeviceGroup)))
			{
				if ((int)item != 100)
				{
					BioWriteDeviceBusiness device = await BioWriteDeviceBusiness.GenerateNewAsync(contextGuid, DateTime.Now, (EnDeviceMessageType)0, (BioDeviceRelationBusiness)null, "", (Guid?)null);
					Task.Run(() => device.InitAsync(BufferDevice, item, default(CancellationToken)));
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendToServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\DeviceServiceHandler.cs", 224);
		}
	}

	public async Task<ReturnedSaveFuncInfo> SendReadRequestToServiceAsync(string contextGuid, Guid deviceGuid)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (BufferDevice == null)
			{
				return res.AddError("BufferDevice is Null", "", "SendReadRequestToServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\DeviceServiceHandler.cs", 231, (StringBuilder)null);
			}
			MicroDeviceInfo val = await DeviceInfoBusiness.GetMicroAsync(contextGuid, deviceGuid, default(CancellationToken), "");
			if (val == (MicroDeviceInfo)null)
			{
				return res.AddError("دستگاه یافت نشد", "", "SendReadRequestToServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\DeviceServiceHandler.cs", 233, (StringBuilder)null);
			}
			WebContextHolder webContextHolder = WebContextHolder.GetWebContextHolder(contextGuid);
			AttBioWriteDeviceWaiters waiter = new AttBioWriteDeviceWaiters(webContextHolder, (BioWriteDeviceCompositeKey)null, default(CancellationToken), 60000);
			try
			{
				Packet pack = WebBioWriteDevice.GenerateReadRequest(val, DateTime.Now);
				Task.Run(() => BufferDevice.SendData(pack, (Priority)2));
				ReturnedSaveFuncInfoWithValue<List<WebBiometricReadMaster>> val2 = await waiter.WaitForReadResponse();
				res.AddReturnedValue((ReturnedSaveFuncInfo)(object)val2, (ReturnedState)0, "SendReadRequestToServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\DeviceServiceHandler.cs", 241);
				if (res.HasError)
				{
					return res;
				}
				if (!(val2.value?.Any() ?? false))
				{
					return res.AddError("داده ای از دستگاه دریافت نشد", "", "SendReadRequestToServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\DeviceServiceHandler.cs", 243, (StringBuilder)null);
				}
				ReturnedSaveFuncInfo val3 = res;
				val3.AddReturnedValue(await BiometricBusiness.SaveRangeAsync(contextGuid, val2.value, default(CancellationToken)), (ReturnedState)0, "SendReadRequestToServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\DeviceServiceHandler.cs", 245);
			}
			finally
			{
				((IDisposable)waiter)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendReadRequestToServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\DeviceServiceHandler.cs", 249);
			res.AddReturnedValue(ex, "", "SendReadRequestToServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\DeviceServiceHandler.cs", 250);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> SendDeleteRequestToServiceAsync(string contextGuid, Guid deviceGuid)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (BufferDevice == null)
			{
				return res.AddError("BufferDevice is Null", "", "SendDeleteRequestToServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\DeviceServiceHandler.cs", 259, (StringBuilder)null);
			}
			MicroDeviceInfo val = await DeviceInfoBusiness.GetMicroAsync(contextGuid, deviceGuid, default(CancellationToken), "");
			if (val == (MicroDeviceInfo)null)
			{
				return res.AddError("دستگاه یافت نشد", "", "SendDeleteRequestToServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\DeviceServiceHandler.cs", 261, (StringBuilder)null);
			}
			WebContextHolder webContextHolder = WebContextHolder.GetWebContextHolder(contextGuid);
			AttBioWriteDeviceWaiters waiter = new AttBioWriteDeviceWaiters(webContextHolder, (BioWriteDeviceCompositeKey)null, default(CancellationToken), 60000);
			try
			{
				Packet pack = WebBioWriteDevice.GenerateReadRequest(val, DateTime.Now);
				Task.Run(() => BufferDevice.SendData(pack, (Priority)2));
				ReturnedSaveFuncInfoWithValue<List<WebBiometricReadMaster>> val2 = await waiter.WaitForReadResponse();
				res.AddReturnedValue((ReturnedSaveFuncInfo)(object)val2, (ReturnedState)0, "SendDeleteRequestToServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\DeviceServiceHandler.cs", 269);
				if (res.HasError)
				{
					return res;
				}
				if (val2.value?.Any() ?? false)
				{
					foreach (WebBiometricReadMaster item in val2.value)
					{
						if (!(((item != null) ? item.DeviceGuid : Guid.Empty) == Guid.Empty))
						{
							ReturnedSaveFuncInfo val3 = res;
							val3.AddReturnedValue(await BiometricBusiness.ChangeHardwareAsync(contextGuid, item.DeviceGuid, (Guid?)null, 0, default(CancellationToken), ""), (ReturnedState)0, "SendDeleteRequestToServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\DeviceServiceHandler.cs", 276);
						}
					}
				}
			}
			finally
			{
				((IDisposable)waiter)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendDeleteRequestToServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\DeviceServiceHandler.cs", 282);
			res.AddReturnedValue(ex, "", "SendDeleteRequestToServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\DeviceServiceHandler.cs", 283);
		}
		finally
		{
			BioWriteDeviceBusiness.ForceSendToService();
		}
		return res;
	}

	private Task Buffer_Connecting(object sender, EventArgs e)
	{
		return Task.CompletedTask;
	}

	private Task Buffer_Connected(object sender, BaseEventArgs e)
	{
		try
		{
			Utility.MessageBoxShowByNotif(showPopUp: false, (NotificationEnum)10, "در حال اتصال به سرویس ارتباط با دستگاه ...", "", 3);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Buffer_Connected", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\DeviceServiceHandler.cs", 295);
		}
		return Task.CompletedTask;
	}

	private Task Buffer_Disconnected(object sender, BaseEventArgs e)
	{
		try
		{
			Utility.MessageBoxShowByNotif(showPopUp: false, (NotificationEnum)5, "ارتباط سرویس دستگاه ها قطع شد", "", 3);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Buffer_Disconnected", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\DeviceServiceHandler.cs", 304);
		}
		return Task.CompletedTask;
	}

	private Task Buffer_LoggedIn(object sender, LoginResponseEventArgs e)
	{
		try
		{
			Utility.MessageBoxShowByNotif(showPopUp: false, (NotificationEnum)1, "سرویس ارتباط با دستگاه لاگین شد", "", 3);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Buffer_LoggedIn", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceUI\\Forms\\Service\\DeviceServiceHandler.cs", 313);
		}
		return Task.CompletedTask;
	}
}
