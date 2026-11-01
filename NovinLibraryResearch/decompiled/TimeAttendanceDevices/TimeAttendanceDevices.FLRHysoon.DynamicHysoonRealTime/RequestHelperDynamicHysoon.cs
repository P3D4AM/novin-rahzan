using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PacketParser.Attendance.Entities;
using PacketParser.BusinessLayer;
using TimeAttendanceDAL.Utility;

namespace TimeAttendanceDevices.FLRHysoon.DynamicHysoonRealTime;

public class RequestHelperDynamicHysoon
{
	public delegate void RealTimeLog(string message);

	public delegate void ExecutingDispatch(int state);

	private HttpListenerRequest request;

	public string ContextGuid { get; }

	public event RealTimeLog RealTimeLogEvent;

	public RequestHelperDynamicHysoon(string contextGuid, HttpListenerRequest request)
	{
		ContextGuid = contextGuid;
		this.request = request;
	}

	public async Task DispatchResources(ExecutingDispatch action, string deviceIp, IDeviceLogOnPersonRepository _dalRepository)
	{
		try
		{
			string? text = request.Headers.Get("request_code");
			string trans_id = request.Headers.Get("trans_id");
			string dev_id = request.Headers.Get("dev_id");
			string deviceModel = request.Headers.Get("dev_model");
			request.Headers.Get("token");
			request.Headers.Get("Connection");
			request.Headers.Get("Content-Length");
			request.Headers.Get("Content-Type");
			request.Headers.Get("Accept");
			request.Headers.Get("Accept-Encoding");
			request.Headers.Get("Accept-Language");
			request.Headers.Get("Host");
			request.Headers.Get("User-Agent");
			string text2 = new StreamReader(request.InputStream).ReadToEnd();
			if (text.Contains("realtime_glog"))
			{
				DynamicHysoonInfo.MonitoringLogs log2 = JsonConvert.DeserializeObject<DynamicHysoonInfo.MonitoringLogs>(text2);
				DeviceLogOnPerson log = RecordAllDevice.GetFromDynamicHysoonRealTime(ContextGuid, log2);
				log.DeviceSerial = dev_id;
				log.DeviceModel = deviceModel;
				ReturnedSaveFuncInfoWithValue<LogOnPersonSaveReqult> val = await _dalRepository.SaveAsync(log);
				if (((ReturnedSaveFuncInfo)val).HasError)
				{
					this.RealTimeLogEvent?.Invoke(((ReturnedSaveFuncInfo)val).ErrorMessage);
				}
				else
				{
					this.RealTimeLogEvent?.Invoke($"تردد جدید از دستگاه {dev_id} با کد {log.PersonNo} دریافت شد");
					DeviceEventsEntity.SaveEventMessage($"{dev_id}-{log.DeviceModel}-{log.IoMode}", log.DeviceNo, log.DeviceSerial, trans_id, log.FixedDateM, log.PersonNo, "taradodLog");
				}
			}
			else if (request.Headers.Get("request_code").Contains("realtime_enroll_data"))
			{
				JsonConvert.DeserializeObject<DynamicHysoonInfo.GetUsers>(text2);
			}
			else if (request.Headers.Get("request_code").Contains("receive_cmd"))
			{
				JsonConvert.DeserializeObject<DynamicHysoonInfo.httpTime>(text2);
			}
		}
		catch
		{
			action(405);
			return;
		}
		action(404);
	}
}
