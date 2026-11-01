using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;
using TimeAttendanceDAL.Dal.Device;
using TimeAttendanceDevices;

namespace TimeAttendanceDAL.Dal.Enroll;

public sealed class EnrollManage : IHasContext
{
	private static ConcurrentDictionary<string, EnrollManage> _dic = new ConcurrentDictionary<string, EnrollManage>();

	public string ContextGuid { get; set; }

	public List<EnrollDet> EnrollDets { get; set; }

	public event EventNotification OnCompleteOperation;

	private EnrollManage(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static EnrollManage GetEnrollManage(string contextGuid)
	{
		try
		{
			_dic.TryGetValue(contextGuid, out var value);
			if (value == null)
			{
				value = new EnrollManage(contextGuid);
				_dic.TryAdd(contextGuid, value);
			}
			return value;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return null;
	}

	public async Task<List<EnrollData>> GetEnrollDataAsync(List<int> personIdList = null, CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			List<EnrollData> xret = new List<EnrollData>();
			foreach (DeviceInfo item in await DeviceInfo.GetAllAsync(ContextGuid, token))
			{
				List<EnrollData> list = xret;
				list.AddRange(await item.GetEnrollDatasAsync(personIdList, token));
			}
			return xret;
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

	public async Task<List<EnrollData>> GetEnrollDataFromDeviceListAsync(List<int> pDeviceList, CancellationToken token)
	{
		List<EnrollData> xret = new List<EnrollData>();
		try
		{
			List<DeviceInfo> list = (from q in await DeviceInfo.GetAllAsync(ContextGuid, token)
				join g in pDeviceList on q.Id equals g
				select q).ToList();
			foreach (DeviceInfo item in list)
			{
				List<EnrollData> list2 = xret;
				list2.AddRange(await item.GetEnrollDatasAsync(null, token));
			}
			return xret;
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

	public async Task PutEnrollDataAsync(List<EnrollData> enrollDataList, CancellationToken token)
	{
		try
		{
			foreach (DeviceInfo item in await DeviceInfo.GetAllAsync(ContextGuid, token))
			{
				item.PutEnrollData(enrollDataList);
			}
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

	public async Task PutEnrollDataToDeviceListAsync(List<EnrollData> enrollDataList, List<int> pDeviceList, CancellationToken token = default(CancellationToken))
	{
		try
		{
			foreach (DeviceInfo item in (from q in await DeviceInfo.GetAllAsync(ContextGuid, token)
				join g in pDeviceList on q.Id equals g
				select q).ToList())
			{
				item.PutEnrollData(enrollDataList);
			}
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

	public async Task<bool> ClearEnrollOnDeviceAsync(CancellationToken token = default(CancellationToken))
	{
		bool sw = true;
		try
		{
			foreach (DeviceInfo item in await DeviceInfo.GetAllAsync(ContextGuid, token))
			{
				if (item.ClearEnrollData() < 1)
				{
					sw = false;
				}
			}
			return sw;
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

	public async Task<bool> PutEnrollActivePersonIntoDeviceAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 3;
		try
		{
			foreach (DeviceInfo device in await DeviceInfo.GetAllAsync(ContextGuid, token))
			{
				List<ValidGharardad> list = await ValidGharardad.GetValidGharardadsAsync(ContextGuid, DateTime.Now, device.Id, token);
				if (list.Count <= 0)
				{
					continue;
				}
				foreach (ValidGharardad item in list)
				{
					List<LogPerson> sessionNo = LogPerson.GetSessionNo(ContextGuid, item.PersonId, item.DateStartMGharardad, item.DateFinishMGharardad);
					if (item.Periodic <= sessionNo.Count)
					{
						item.GharardadId = -1;
					}
				}
				List<List<EnrollData>> source = (from q in list
					where q.GharardadId > 0 && q.EnrollDataObj != null
					select q.EnrollDataObj).ToList();
				List<EnrollData> query3 = (from e in source
					from e2 in e
					select e2).Distinct().ToList();
				if (query3.Count > 0)
				{
					await PutEnrollDataAsync(query3, token);
					if (this.OnCompleteOperation != null)
					{
						string message = "نگاشت اطلاعات اثر انگشت افراد فعال بر روی دستگاه  " + device.DeviceName + " انجام شد.";
						EventNotification onCompleteOperation = this.OnCompleteOperation;
						object source2 = this;
						onCompleteOperation(source2, await EventArgsNotify.GetAsync(ContextGuid, query3[0].DeviceId, message, "نگاشت اثر انگشت", (NotificationEnum)3, 3, token));
					}
				}
			}
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
		return true;
	}

	public async Task<bool> SaveEnrollDataAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			List<EnrollData> ret = await GetEnrollDataAsync(null, token);
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(ContextGuid));
			bool result;
			try
			{
				SqlCommand val = new SqlCommand("[sp_EnrollData_Save]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(token);
					foreach (EnrollData item in ret)
					{
						((DbParameterCollection)(object)cmd.Parameters).Clear();
						cmd.Parameters.AddWithValue("PersonId", (object)item.PersonId);
						cmd.Parameters.AddWithValue("DeviceId", (object)item.DeviceId);
						string s = JsonConvert.SerializeObject((object)item);
						byte[] bytes = new ASCIIEncoding().GetBytes(s);
						cmd.Parameters.AddWithValue("EnrollData", (object)bytes);
						await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
					}
					((DbConnection)(object)cn).Close();
					result = true;
				}
				finally
				{
					((IDisposable)cmd)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)cn)?.Dispose();
			}
			return result;
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
}
