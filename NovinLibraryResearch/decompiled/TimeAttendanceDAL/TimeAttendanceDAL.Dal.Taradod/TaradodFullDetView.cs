using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Assistance;
using PacketParser.Attendance.Entities;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.Entities.Interfaces;
using Serilog;
using TimeAttendanceDAL.Utility;

namespace TimeAttendanceDAL.Dal.Taradod;

public class TaradodFullDetView : TaradodFullDet, INotifyPropertyChanged, INpDisposable, IDisposable
{
	private Task _loadHozorTask;

	private Task _loadEkTask;

	private Task _loadKkTask;

	private Task _loadStatusTask;

	private bool _isDisposed;

	private CancellationTokenSource _cancellationTokenSource_loadTaradodSt;

	private CancellationTokenSource _cancellationTokenSource_ek;

	private CancellationTokenSource _cancellationTokenSource_kk;

	private CancellationTokenSource _cancellationTokenSource_hozor;

	private string _taradodStatusName;

	public TaskCompletionSource<bool> LoadDataFinished { get; private set; } = new TaskCompletionSource<bool>();


	public bool IsChecked { get; set; }

	public int Index { get; set; }

	public TimeSpan V1Time
	{
		get
		{
			List<LogOnPersonel> taradodsCurrentDay = base.TaradodsCurrentDay;
			if (taradodsCurrentDay == null || taradodsCurrentDay.Count <= 0)
			{
				return new TimeSpan(0L);
			}
			return base.TaradodsCurrentDay[0].DateTimeLogM.TimeOfDay;
		}
		set
		{
			if (TimeValueIsValid(value))
			{
				List<LogOnPersonel> taradodsCurrentDay = base.TaradodsCurrentDay;
				TimeSpan oldTime = ((taradodsCurrentDay != null && taradodsCurrentDay.Count > 0) ? base.TaradodsCurrentDay[0].DateTimeLogM.TimeOfDay : new TimeSpan(0L));
				ApplyChangeAsync(oldTime, value);
			}
		}
	}

	public TimeSpan V2Time
	{
		get
		{
			return base.CoupleTaradodCurrentDay[1].Vtaradod.DateTimeLogM.TimeOfDay;
		}
		set
		{
			base.CoupleTaradodCurrentDay[1].Vtaradod.DateTimeLogM.Date.AddSeconds(value.Seconds);
		}
	}

	public TimeSpan V3Time
	{
		get
		{
			return base.CoupleTaradodCurrentDay[2].Vtaradod.DateTimeLogM.TimeOfDay;
		}
		set
		{
			base.CoupleTaradodCurrentDay[2].Vtaradod.DateTimeLogM.Date.AddSeconds(value.Seconds);
		}
	}

	public TimeSpan V4Time
	{
		get
		{
			return base.CoupleTaradodCurrentDay[3].Vtaradod.DateTimeLogM.TimeOfDay;
		}
		set
		{
			base.CoupleTaradodCurrentDay[3].Vtaradod.DateTimeLogM.Date.AddSeconds(value.Seconds);
		}
	}

	public TimeSpan V5Time
	{
		get
		{
			return base.CoupleTaradodCurrentDay[4].Vtaradod.DateTimeLogM.TimeOfDay;
		}
		set
		{
			base.CoupleTaradodCurrentDay[4].Vtaradod.DateTimeLogM.Date.AddSeconds(value.Seconds);
		}
	}

	public TimeSpan V6Time
	{
		get
		{
			return base.CoupleTaradodCurrentDay[5].Vtaradod.DateTimeLogM.TimeOfDay;
		}
		set
		{
			base.CoupleTaradodCurrentDay[5].Vtaradod.DateTimeLogM.Date.AddSeconds(value.Seconds);
		}
	}

	public TimeSpan V7Time
	{
		get
		{
			return base.CoupleTaradodCurrentDay[6].Vtaradod.DateTimeLogM.TimeOfDay;
		}
		set
		{
			base.CoupleTaradodCurrentDay[6].Vtaradod.DateTimeLogM.Date.AddSeconds(value.Seconds);
		}
	}

	public TimeSpan V8Time
	{
		get
		{
			return base.CoupleTaradodCurrentDay[7].Vtaradod.DateTimeLogM.TimeOfDay;
		}
		set
		{
			base.CoupleTaradodCurrentDay[7].Vtaradod.DateTimeLogM.Date.AddSeconds(value.Seconds);
		}
	}

	public TimeSpan K1Time
	{
		get
		{
			return base.CoupleTaradodCurrentDay[0].Ktaradod.DateTimeLogM.TimeOfDay;
		}
		set
		{
			base.CoupleTaradodCurrentDay[0].Ktaradod.DateTimeLogM.Date.AddSeconds(value.Seconds);
		}
	}

	public TimeSpan K2Time
	{
		get
		{
			return base.CoupleTaradodCurrentDay[1].Ktaradod.DateTimeLogM.TimeOfDay;
		}
		set
		{
			base.CoupleTaradodCurrentDay[1].Ktaradod.DateTimeLogM.Date.AddSeconds(value.Seconds);
		}
	}

	public TimeSpan K3Time
	{
		get
		{
			return base.CoupleTaradodCurrentDay[2].Ktaradod.DateTimeLogM.TimeOfDay;
		}
		set
		{
			base.CoupleTaradodCurrentDay[2].Ktaradod.DateTimeLogM.Date.AddSeconds(value.Seconds);
		}
	}

	public TimeSpan K4Time
	{
		get
		{
			return base.CoupleTaradodCurrentDay[3].Ktaradod.DateTimeLogM.TimeOfDay;
		}
		set
		{
			base.CoupleTaradodCurrentDay[3].Ktaradod.DateTimeLogM.Date.AddSeconds(value.Seconds);
		}
	}

	public TimeSpan K5Time
	{
		get
		{
			return base.CoupleTaradodCurrentDay[4].Ktaradod.DateTimeLogM.TimeOfDay;
		}
		set
		{
			base.CoupleTaradodCurrentDay[4].Ktaradod.DateTimeLogM.Date.AddSeconds(value.Seconds);
		}
	}

	public TimeSpan K6Time
	{
		get
		{
			return base.CoupleTaradodCurrentDay[5].Ktaradod.DateTimeLogM.TimeOfDay;
		}
		set
		{
			base.CoupleTaradodCurrentDay[5].Ktaradod.DateTimeLogM.Date.AddSeconds(value.Seconds);
		}
	}

	public TimeSpan K7Time
	{
		get
		{
			return base.CoupleTaradodCurrentDay[6].Ktaradod.DateTimeLogM.TimeOfDay;
		}
		set
		{
			base.CoupleTaradodCurrentDay[6].Ktaradod.DateTimeLogM.Date.AddSeconds(value.Seconds);
		}
	}

	public TimeSpan K8Time
	{
		get
		{
			return base.CoupleTaradodCurrentDay[7].Ktaradod.DateTimeLogM.TimeOfDay;
		}
		set
		{
			base.CoupleTaradodCurrentDay[7].Ktaradod.DateTimeLogM.Date.AddSeconds(value.Seconds);
		}
	}

	public bool IsDisposed => _isDisposed;

	public event PropertyChangedEventHandler PropertyChanged;

	public async Task ForceToLoadDataAsync_(CancellationToken token)
	{
		_ = 3;
		try
		{
			_cancellationTokenSource_loadTaradodSt?.Cancel();
			_cancellationTokenSource_loadTaradodSt = new CancellationTokenSource();
			CancellationToken token2 = _cancellationTokenSource_loadTaradodSt.Token;
			await LoadStatusTaradodAsync(token2);
			if (!token.IsCancellationRequested)
			{
				TaradodFullDet.CheckValidationVorodAndKHoroj(base.CoupleTaradodCurrentDay, token);
				_cancellationTokenSource_hozor?.Cancel();
				_cancellationTokenSource_hozor = new CancellationTokenSource();
				CancellationToken token3 = _cancellationTokenSource_hozor.Token;
				await LoadHozorAsync(token3);
				_cancellationTokenSource_ek?.Cancel();
				_cancellationTokenSource_ek = new CancellationTokenSource();
				CancellationToken token4 = _cancellationTokenSource_ek.Token;
				await LoadEzafeKarAsync(token4);
				_cancellationTokenSource_kk?.Cancel();
				_cancellationTokenSource_kk = new CancellationTokenSource();
				CancellationToken token5 = _cancellationTokenSource_kk.Token;
				await LoadKasreKarAsync(token5);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public async Task CalcEk_Kk_DataAsync(CancellationToken token)
	{
		_ = 3;
		try
		{
			await GetStatuseTaradodAsync(token);
			await GetKarkardAsync(token);
			await GetEzafeKarAsync(token);
			await GetKasrKarAsync(token);
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

	private void CheckFinishedAllTasks()
	{
		try
		{
			if (_loadStatusTask != null && _loadEkTask != null && _loadKkTask != null && _loadHozorTask != null)
			{
				LoadDataFinished?.TrySetResult(result: true);
			}
			else if (IsDisposed)
			{
				LoadDataFinished?.TrySetCanceled();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	private async Task LoadEzafeKarAsync(CancellationToken token, bool IsFromUi = true)
	{
		try
		{
			if (token.IsCancellationRequested || IsDisposed)
			{
				return;
			}
			await GetEzafeKarAsync(token);
			if (token.IsCancellationRequested || IsDisposed || !IsFromUi || this.PropertyChanged == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread((Action)delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("EzafeKar"));
				}
				catch (Exception ex2)
				{
					Log.Error(ex2, "");
				}
			}, "LoadEzafeKarAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDetView.cs");
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		finally
		{
			_loadEkTask = Task.FromResult(result: true);
			CheckFinishedAllTasks();
		}
	}

	private async Task LoadKasreKarAsync(CancellationToken token, bool IsFromUi = true)
	{
		try
		{
			if (token.IsCancellationRequested || IsDisposed)
			{
				return;
			}
			await GetKasrKarAsync(token);
			if (token.IsCancellationRequested || IsDisposed || !IsFromUi || this.PropertyChanged == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread((Action)delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("KasreKar"));
				}
				catch (Exception ex2)
				{
					Log.Error(ex2, "");
				}
			}, "LoadKasreKarAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDetView.cs");
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		finally
		{
			_loadKkTask = Task.FromResult(result: true);
			CheckFinishedAllTasks();
		}
	}

	private async Task LoadHozorAsync(CancellationToken token, bool IsFromUi = true)
	{
		try
		{
			if (token.IsCancellationRequested || IsDisposed)
			{
				return;
			}
			await GetKarkardAsync(token);
			if (token.IsCancellationRequested || IsDisposed || !IsFromUi || this.PropertyChanged == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread((Action)delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Hozor"));
				}
				catch (Exception ex2)
				{
					Log.Error(ex2, "");
				}
			}, "LoadHozorAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDetView.cs");
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		finally
		{
			_loadHozorTask = Task.FromResult(result: true);
			CheckFinishedAllTasks();
		}
	}

	private async Task LoadStatusTaradodAsync(CancellationToken token, bool IsFromUi = true)
	{
		try
		{
			if (token.IsCancellationRequested || IsDisposed)
			{
				return;
			}
			await GetStatuseTaradodAsync(token);
			if (token.IsCancellationRequested || IsDisposed || !IsFromUi || this.PropertyChanged == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread((Action)delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TaradodStatusName"));
				}
				catch (Exception ex2)
				{
					Log.Error(ex2, "");
				}
			}, "LoadStatusTaradodAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDetView.cs");
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		finally
		{
			_loadStatusTask = Task.FromResult(result: true);
			CheckFinishedAllTasks();
		}
	}

	private bool TimeValueIsValid(TimeSpan timeValue)
	{
		try
		{
			if (timeValue.TotalMinutes > 1439.0)
			{
				TimeAttendanceDAL.Utility.Utility.MessageBoxShowByNotif(showPopUp: true, (NotificationEnum)4, "داده نامعتبر", "", 3);
				return false;
			}
			return true;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return false;
	}

	private async Task<bool> ApplyChangeAsync(TimeSpan oldTime, TimeSpan newTime)
	{
		_ = 1;
		try
		{
			TimeSpan timeSpan = new TimeSpan(0L);
			TimeSpan timeSpan2 = timeSpan;
			int deviceId_ = 0;
			int deviceNo = 0;
			if (base.TaradodsCurrentDay?.Any() ?? false)
			{
				timeSpan2 = base.TaradodsCurrentDay[0]?.DateTimeLogM.TimeOfDay ?? timeSpan;
				deviceId_ = base.TaradodsCurrentDay[0]?.DeviceId_ ?? 0;
				deviceNo = base.TaradodsCurrentDay[0]?.DeviceNo ?? 0;
			}
			if (newTime == timeSpan2)
			{
				return false;
			}
			if (newTime == timeSpan && timeSpan2 > timeSpan)
			{
				base.TaradodsCurrentDay[0].DeleteAsync();
			}
			else if (newTime > timeSpan && timeSpan2 == timeSpan)
			{
				await new LogOnPersonel(base.ContextGuid, base.DateM.Date)
				{
					DateTimeLogM = base.DateM.Date.AddMinutes(newTime.TotalMinutes),
					TypeTaradod = (TypeTaradodEnumType)3,
					IsVirtual = (SavedTaradodEnumType)5,
					PersonId = ((PersonBase)base.PersonInfo).Id,
					PersonNo = int.Parse(base.PersonHardwareCodes?.First()?.HardwareCode ?? "0"),
					DeviceNo = deviceNo,
					DeviceId_ = deviceId_
				}.SaveAsync("TaradodFullDetView.ApplyChange", default(CancellationToken), "ApplyChangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDetView.cs");
			}
			else
			{
				if (!(newTime > timeSpan) || !(timeSpan2 > timeSpan))
				{
					throw new Exception("وضعیت داده مشخص نشد");
				}
				base.TaradodsCurrentDay[0].DateTimeLogM = base.TaradodsCurrentDay[0].DateTimeLogM.Date.AddMinutes(newTime.TotalMinutes);
				base.TaradodsCurrentDay[0].IsVirtual = (SavedTaradodEnumType)2;
				await base.TaradodsCurrentDay[0].SaveAsync("taradodFUllDetView.applyChange2", default(CancellationToken), "ApplyChangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TaradodFullDetView.cs");
			}
			return true;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return false;
	}

	public void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_cancellationTokenSource_loadTaradodSt?.Cancel();
				_cancellationTokenSource_loadTaradodSt?.Dispose();
				_cancellationTokenSource_loadTaradodSt = null;
				_cancellationTokenSource_ek?.Cancel();
				_cancellationTokenSource_ek?.Dispose();
				_cancellationTokenSource_ek = null;
				_cancellationTokenSource_kk?.Cancel();
				_cancellationTokenSource_kk?.Dispose();
				_cancellationTokenSource_kk = null;
				_cancellationTokenSource_hozor?.Cancel();
				_cancellationTokenSource_hozor?.Dispose();
				_cancellationTokenSource_hozor = null;
				LoadDataFinished?.TrySetCanceled();
				LoadDataFinished = null;
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}
}
