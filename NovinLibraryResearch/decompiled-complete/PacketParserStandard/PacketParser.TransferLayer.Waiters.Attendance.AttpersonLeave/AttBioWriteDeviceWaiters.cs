using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Attendance.Biometric;
using PacketParser.Entities.CompositeKeys;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.EventArgs.Attendance;

namespace PacketParser.TransferLayer.Waiters.Attendance.AttpersonLeave;

public class AttBioWriteDeviceWaiters : INpDisposable, IDisposable
{
	private bool _isDisposed;

	private readonly BioWriteDeviceCompositeKey _key;

	private readonly WebContextHolder _instance;

	private TaskCompletionSource<SaveResponse<BioWriteDeviceCompositeKey>> _saveWaiter { get; set; }

	private TaskCompletionSource<List<WebBiometricReadMaster>> _readWaiter { get; set; }

	private TaskCompletionSource<BioMetricDisconnectEventArgs> _disconnectWaiter { get; set; }

	private CancellationToken _token { get; set; }

	private int _timeout { get; set; }

	public bool IsDisposed => _isDisposed;

	public AttBioWriteDeviceWaiters(WebContextHolder instance, BioWriteDeviceCompositeKey key, CancellationToken token = default(CancellationToken), int timeOutMiliSeconds = 60000)
	{
		try
		{
			_saveWaiter = new TaskCompletionSource<SaveResponse<BioWriteDeviceCompositeKey>>();
			_readWaiter = new TaskCompletionSource<List<WebBiometricReadMaster>>();
			_disconnectWaiter = new TaskCompletionSource<BioMetricDisconnectEventArgs>();
			_key = key;
			_instance = instance;
			_token = token;
			_timeout = timeOutMiliSeconds;
			_instance.BioMetricWriteDevice.OnSaveResponse += Instance_OnSaveResponse;
			_instance.BioMetricWriteDevice.OnReadResponse += Instance_OnReadResponse;
			_instance.BioMetricWriteDevice.OnDisconnectResponse += BioMetricWriteDevice_OnDisconnectResponse;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Attendance\\AttBioWriteDeviceWaiters.cs", 46);
		}
	}

	private Task BioMetricWriteDevice_OnDisconnectResponse(object sender, BioMetricDisconnectEventArgs e)
	{
		try
		{
			_disconnectWaiter.TrySetResult(e);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BioMetricWriteDevice_OnDisconnectResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Attendance\\AttBioWriteDeviceWaiters.cs", 55);
		}
		return Task.CompletedTask;
	}

	private Task Instance_OnReadResponse(object sender, ConnectionEventArgs<WebBiometricReadMaster> e)
	{
		try
		{
			_readWaiter.TrySetResult(e.ConnectionData);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Instance_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Attendance\\AttBioWriteDeviceWaiters.cs", 64);
		}
		return Task.CompletedTask;
	}

	private Task Instance_OnSaveResponse(object sender, SaveResponseEventArgs<BioWriteDeviceCompositeKey> e)
	{
		try
		{
			if (_key == null)
			{
				return Task.CompletedTask;
			}
			List<SaveResponse<BioWriteDeviceCompositeKey>> saveResponses = e.SaveResponses;
			if (saveResponses != null && saveResponses.Any((SaveResponse<BioWriteDeviceCompositeKey> p) => p.Key == null))
			{
				throw new NullReferenceException("save responses contains null keys!");
			}
			if (e.SaveResponses.Any((SaveResponse<BioWriteDeviceCompositeKey> x) => x.Key.BioGuid == _key.BioGuid && x.Key.DeviceGuid == _key.DeviceGuid))
			{
				_saveWaiter.TrySetResult(e.SaveResponses.SingleOrDefault((SaveResponse<BioWriteDeviceCompositeKey> x) => x.Key.BioGuid == _key.BioGuid && x.Key.DeviceGuid == _key.DeviceGuid));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Instance_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Attendance\\AttBioWriteDeviceWaiters.cs", 76);
		}
		return Task.CompletedTask;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<SaveResponse<BioWriteDeviceCompositeKey>>> WaitForSaveResponse()
	{
		ReturnedSaveFuncInfoWithValue<SaveResponse<BioWriteDeviceCompositeKey>> ret = new ReturnedSaveFuncInfoWithValue<SaveResponse<BioWriteDeviceCompositeKey>>();
		try
		{
			if (await Task.WhenAny(new Task[2]
			{
				Task.Delay(_timeout, _token),
				_saveWaiter.Task
			}) == _saveWaiter.Task)
			{
				ReturnedSaveFuncInfoWithValue<SaveResponse<BioWriteDeviceCompositeKey>> returnedSaveFuncInfoWithValue = ret;
				returnedSaveFuncInfoWithValue.value = await _saveWaiter.Task;
			}
			else
			{
				string text = $"((BioWriteDevice))_saveWaiter timedOut or token Canceled after {_timeout}(ms). EnSystem.SignalR";
				ret.AddError(EnSystem.SignalR, text, "WaitForSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Attendance\\AttBioWriteDeviceWaiters.cs", 90);
				ret.value = new SaveResponse<BioWriteDeviceCompositeKey>(0, new BioWriteDeviceCompositeKey
				{
					BioGuid = _key.BioGuid,
					DeviceGuid = _key.DeviceGuid,
					MessageType = _key.MessageType
				}, DateTime.Now, saveResultStatus: false, text);
			}
		}
		catch (OperationCanceledException)
		{
			ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "WaitForSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Attendance\\AttBioWriteDeviceWaiters.cs", 103);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "WaitForSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Attendance\\AttBioWriteDeviceWaiters.cs", 106);
			ret.AddReturnedValue(ex2, "", "WaitForSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Attendance\\AttBioWriteDeviceWaiters.cs", 107);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<WebBiometricReadMaster>>> WaitForReadResponse()
	{
		ReturnedSaveFuncInfoWithValue<List<WebBiometricReadMaster>> ret = new ReturnedSaveFuncInfoWithValue<List<WebBiometricReadMaster>>();
		try
		{
			if (await Task.WhenAny(new Task[2]
			{
				Task.Delay(_timeout, _token),
				_readWaiter.Task
			}) == _readWaiter.Task)
			{
				ReturnedSaveFuncInfoWithValue<List<WebBiometricReadMaster>> returnedSaveFuncInfoWithValue = ret;
				returnedSaveFuncInfoWithValue.value = await _readWaiter.Task;
			}
			else
			{
				string message = $"((BioWriteDevice))_readWaiter timedOut or token Canceled after {_timeout}(ms). EnSystem.SignalR";
				ret.AddError(EnSystem.SignalR, message, "WaitForReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Attendance\\AttBioWriteDeviceWaiters.cs", 122);
			}
		}
		catch (OperationCanceledException)
		{
			ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "WaitForReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Attendance\\AttBioWriteDeviceWaiters.cs", 125);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "WaitForReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Attendance\\AttBioWriteDeviceWaiters.cs", 128);
			ret.AddReturnedValue(ex2, "", "WaitForReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Attendance\\AttBioWriteDeviceWaiters.cs", 129);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<BioMetricDisconnectEventArgs>> WaitForDisconnectResponse()
	{
		ReturnedSaveFuncInfoWithValue<BioMetricDisconnectEventArgs> ret = new ReturnedSaveFuncInfoWithValue<BioMetricDisconnectEventArgs>();
		try
		{
			if (await Task.WhenAny(new Task[2]
			{
				Task.Delay(_timeout, _token),
				_disconnectWaiter.Task
			}) == _disconnectWaiter.Task)
			{
				ReturnedSaveFuncInfoWithValue<BioMetricDisconnectEventArgs> returnedSaveFuncInfoWithValue = ret;
				returnedSaveFuncInfoWithValue.value = await _disconnectWaiter.Task;
			}
			else
			{
				string message = $"((BioWriteDevice))_disconnectWaiter timedOut or token Canceled after {_timeout}(ms). EnSystem.SignalR";
				ret.AddError(EnSystem.SignalR, message, "WaitForDisconnectResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Attendance\\AttBioWriteDeviceWaiters.cs", 144);
			}
		}
		catch (OperationCanceledException)
		{
			ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "WaitForDisconnectResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Attendance\\AttBioWriteDeviceWaiters.cs", 147);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "WaitForDisconnectResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Attendance\\AttBioWriteDeviceWaiters.cs", 150);
			ret.AddReturnedValue(ex2, "", "WaitForDisconnectResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Attendance\\AttBioWriteDeviceWaiters.cs", 151);
		}
		return ret;
	}

	public void Dispose(bool disposing)
	{
		try
		{
			if (!IsDisposed)
			{
				_isDisposed = true;
				_saveWaiter?.TrySetCanceled(_token);
				_readWaiter?.TrySetCanceled(_token);
				_disconnectWaiter?.TrySetCanceled(_token);
				_instance.BioMetricWriteDevice.OnSaveResponse -= Instance_OnSaveResponse;
				_instance.BioMetricWriteDevice.OnReadResponse -= Instance_OnReadResponse;
				_instance.BioMetricWriteDevice.OnDisconnectResponse -= BioMetricWriteDevice_OnDisconnectResponse;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.Error(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Attendance\\AttBioWriteDeviceWaiters.cs", 170);
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
	}
}
