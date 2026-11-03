using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Attendance;

namespace PacketParser.Entities.Attendance;

public class WebAttendanceDeviceInfo : Serializable<WebAttendanceDeviceInfo>, IAttendanceDeviceInfo, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	private static string Url = HttpHelpers.ApiUrl + "AttendanceDeviceInfo/";

	public virtual short? DepId { get; set; }

	public string Name { get; set; }

	public long SerialId { get; set; }

	public int TerminalId { get; set; }

	public DeviceModelEnumType TerminalType { get; set; }

	[JsonIgnore]
	[XmlIgnore]
	public string TerminalTypeString => TerminalType.GetDisplay();

	public int PortNumber { get; set; }

	public string Description { get; set; }

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public virtual DateTime? LastPingTime { get; set; }

	public string SerialIdString { get; set; }

	public string DepName { get; set; } = "";


	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed { get; private set; }

	public static DeviceModelEnumType DetectDeviceType(string devicTypeString)
	{
		try
		{
			switch (devicTypeString)
			{
			case "S200":
				return DeviceModelEnumType.NP2815;
			case "S260":
				return DeviceModelEnumType.NP261;
			case "S560":
				return DeviceModelEnumType.NP261;
			case "M61":
				return DeviceModelEnumType.NP550;
			case "Hyson":
				return DeviceModelEnumType.NP587;
			case "Hysoon":
				return DeviceModelEnumType.NP587;
			default:
				WebErrorLog.Instance.StartLog("devicTypeString is not valid {@devicTypeString}", devicTypeString, "DetectDeviceType", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 54);
				WebTelegramMessage.GetAttendanceRealTimeMonitoring_bot().Sent2web("❌❌DevicType Send  IS=> " + devicTypeString + " ❌❌", null, "DetectDeviceType", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 55);
				return DeviceModelEnumType.NP2815;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DetectDeviceType", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 61);
			return DeviceModelEnumType.EC160;
		}
	}

	public static string DetectDevicePort(string devicTypeString)
	{
		try
		{
			switch (devicTypeString)
			{
			case "S200":
				return "5005";
			case "S260":
				return "5005";
			case "Hyson":
				return "7005";
			default:
				WebTelegramMessage.GetAttendanceRealTimeMonitoring_bot().Sent2web("❌❌DevicPort Send  IS=> " + devicTypeString + " ❌❌", null, "DetectDevicePort", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 76);
				return "5005";
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DetectDevicePort", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 82);
			return "5005";
		}
	}

	public static ReturnedSaveFuncInfo CheckValidtion(string serialNumber, long terminalId, string portNumber)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			if (string.IsNullOrEmpty(serialNumber) && serialNumber.Count() > 20)
			{
				returnedSaveFuncInfo.AddReturnedValue(ReturnedState.Error, "شماره دستگاه " + serialNumber + " معتبر نمیباشد.", "serialNumber", "CheckValidtion", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 94);
			}
			if (portNumber.ParseToInt() != 0 && portNumber.ParseToInt() < 100)
			{
				returnedSaveFuncInfo.AddReturnedValue(ReturnedState.Error, "ترمینال صحیح نمی باشد" + serialNumber + " معتبر نمیباشد.", "serialNumber", "CheckValidtion", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 96);
			}
			if (terminalId < 0)
			{
				returnedSaveFuncInfo.AddReturnedValue(ReturnedState.Error, "پورت دستگاه " + serialNumber + " معتبر نمیباشد.", "serialNumber", "CheckValidtion", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 98);
			}
			if (returnedSaveFuncInfo.HasError)
			{
				return returnedSaveFuncInfo;
			}
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "CheckValidtion", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 103);
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidtion", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 104);
		}
		return returnedSaveFuncInfo;
	}

	public static async Task<ReturnedSaveFuncInfo> GetLastRecordBySerialString(string serailString)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			UriBuilder uriBuilder = new UriBuilder(HttpHelpers.ApiUrl + $"Department/LogOnPerson/GetLastTardodSerailString/{serailString}/{100}");
			HttpUtility.ParseQueryString(uriBuilder.Query);
			using HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Add("UserName", DepAdmin.UserName);
			client.DefaultRequestHeaders.Add("Password", DepAdmin.Password);
			HttpResponseMessage httpResponseMessage;
			try
			{
				httpResponseMessage = await client.GetAsync(uriBuilder.ToString());
			}
			catch (OperationCanceledException)
			{
				return ret;
			}
			catch (InvalidOperationException)
			{
				return ret;
			}
			catch (HttpRequestException)
			{
				return ret;
			}
			catch (Exception ex4)
			{
				WebErrorLog.Instance.StartLog(ex4, "", "GetLastRecordBySerialString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 129);
				ret.AddReturnedValue(ex4, "", "GetLastRecordBySerialString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 130);
				return ret;
			}
			if (httpResponseMessage.IsSuccessStatusCode)
			{
				foreach (WebAttendanceLogOnPerson item in (await httpResponseMessage.Content.ReadAsStringAsync()).FromJson<List<WebAttendanceLogOnPerson>>("GetLastRecordBySerialString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 136))
				{
					ret.AddInformation($"Device Serial Number iS :{item.DeviceInfo.SerialIdString} => ⏰ LogTime: {item.LogTime} , Personal Code: {item.UserId}  ⏰", "", "GetLastRecordBySerialString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 138);
				}
			}
			else
			{
				string message = (await httpResponseMessage.Content.ReadAsStringAsync()).FromJson<string>("GetLastRecordBySerialString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 143);
				ret.AddError(EnSystem.None, message, "serailString", "GetLastRecordBySerialString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 144);
			}
		}
		catch (Exception ex5)
		{
			WebErrorLog.Instance.StartLog(ex5, "", "GetLastRecordBySerialString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 149);
			ret.AddReturnedValue(ex5, "", "GetLastRecordBySerialString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 150);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> GetLastRecord(string depid)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			UriBuilder uriBuilder = new UriBuilder(HttpHelpers.ApiUrl + $"Department/LogOnPerson/GetLastTardod/{depid}/{100}");
			HttpUtility.ParseQueryString(uriBuilder.Query);
			using HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Add("UserName", DepAdmin.UserName);
			client.DefaultRequestHeaders.Add("Password", DepAdmin.Password);
			HttpResponseMessage httpResponseMessage;
			try
			{
				httpResponseMessage = await client.GetAsync(uriBuilder.ToString());
			}
			catch (OperationCanceledException)
			{
				return ret;
			}
			catch (InvalidOperationException)
			{
				return ret;
			}
			catch (HttpRequestException)
			{
				return ret;
			}
			catch (Exception ex4)
			{
				WebErrorLog.Instance.StartLog(ex4, "", "GetLastRecord", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 176);
				ret.AddReturnedValue(ex4, "", "GetLastRecord", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 177);
				return ret;
			}
			if (httpResponseMessage.IsSuccessStatusCode)
			{
				foreach (WebAttendanceLogOnPerson item in (await httpResponseMessage.Content.ReadAsStringAsync()).FromJson<List<WebAttendanceLogOnPerson>>("GetLastRecord", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 183))
				{
					ret.AddInformation($"Device Serial Number iS :{item.DeviceInfo.SerialIdString} => ⏰ LogTime: {item.LogTime} , Personal Code: {item.UserId}  ⏰", "", "GetLastRecord", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 185);
				}
			}
			else
			{
				string message = (await httpResponseMessage.Content.ReadAsStringAsync()).FromJson<string>("GetLastRecord", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 190);
				ret.AddError(EnSystem.None, message, "depid", "GetLastRecord", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 191);
			}
		}
		catch (Exception ex5)
		{
			WebErrorLog.Instance.StartLog(ex5, "", "GetLastRecord", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 196);
			ret.AddReturnedValue(ex5, "", "GetLastRecord", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 197);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<WebResponse<WebAttendanceDeviceInfo>>> GetDataAsync(string serialNumber, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<WebResponse<WebAttendanceDeviceInfo>> ret = new ReturnedSaveFuncInfoWithValue<WebResponse<WebAttendanceDeviceInfo>>();
		try
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<WebAttendanceDeviceInfo>> returnedSaveFuncInfoWithValue = await (Url + "GetDeviceBySerial/" + serialNumber.ToString()).GetFromApi<WebAttendanceDeviceInfo>(token, 1, null, 30000);
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 208);
			ret.value = returnedSaveFuncInfoWithValue.value;
		}
		catch (WebException ex)
		{
			WebErrorLog.Instance.Warning(ex, "", "GetDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 213);
			ret.AddReturnedValue(ex, "", "GetDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 214);
		}
		catch (Exception ex2)
		{
			ret.AddReturnedValue(ex2, "", "GetDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 218);
			WebErrorLog.Instance.StartLog(ex2, "", "GetDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 219);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<WebResponse<Guid>>> Save_Device(WebAttendanceDeviceInfo item)
	{
		ReturnedSaveFuncInfoWithValue<WebResponse<Guid>> ret = new ReturnedSaveFuncInfoWithValue<WebResponse<Guid>>();
		try
		{
			string url = Url + "SaveDeviceInfoAsync";
			ReturnedSaveFuncInfoWithValue<WebResponse<Guid>> returnedSaveFuncInfoWithValue = await HttpHelpers.PostToApi<Guid, WebAttendanceDeviceInfo>(item, url, null, strongDeserialize: false, 15000, 3);
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Save_Device", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 230);
			ret.value = returnedSaveFuncInfoWithValue.value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Save_Device", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 235);
			ret.AddReturnedValue(ex, "", "Save_Device", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 236);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<WebResponse<ReturnedSaveFuncInfo>>> UpdateLastOnlineTime(long item)
	{
		ReturnedSaveFuncInfoWithValue<WebResponse<ReturnedSaveFuncInfo>> ret = new ReturnedSaveFuncInfoWithValue<WebResponse<ReturnedSaveFuncInfo>>();
		try
		{
			string url = Url + "/LastPing/" + item;
			ReturnedSaveFuncInfoWithValue<WebResponse<WebResponse<ReturnedSaveFuncInfo>>> returnedSaveFuncInfoWithValue = await HttpHelpers.PostToApi<WebResponse<ReturnedSaveFuncInfo>, long>(item, url, null, strongDeserialize: false, 15000, 3);
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "UpdateLastOnlineTime", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 247);
			returnedSaveFuncInfoWithValue.value = returnedSaveFuncInfoWithValue.value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateLastOnlineTime", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 252);
			ret.AddReturnedValue(ex, "", "UpdateLastOnlineTime", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 253);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<WebResponse<WebAttendanceDeviceInfo>>> Del_Department_Id(Guid deviceGuid, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<WebResponse<WebAttendanceDeviceInfo>> ret = new ReturnedSaveFuncInfoWithValue<WebResponse<WebAttendanceDeviceInfo>>();
		try
		{
			string url = Url;
			Guid guid = deviceGuid;
			ReturnedSaveFuncInfoWithValue<WebResponse<WebAttendanceDeviceInfo>> returnedSaveFuncInfoWithValue = await (url + "SetDepartmentId/" + guid).GetFromApi<WebAttendanceDeviceInfo>(token, 1, null, 30000);
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Del_Department_Id", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 264);
			ret.value = returnedSaveFuncInfoWithValue.value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Del_Department_Id", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 269);
			ret.AddReturnedValue(ex, "", "Del_Department_Id", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 270);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<WebResponse<WebAttendanceDeviceInfo>>> SetDepartmentId(Guid deviceGuid, short depId, string depName, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<WebResponse<WebAttendanceDeviceInfo>> ret = new ReturnedSaveFuncInfoWithValue<WebResponse<WebAttendanceDeviceInfo>>();
		try
		{
			string[] obj = new string[7] { Url, "SetDepartmentId/", null, null, null, null, null };
			Guid guid = deviceGuid;
			obj[2] = guid.ToString();
			obj[3] = "/";
			obj[4] = depId.ToString();
			obj[5] = "/";
			obj[6] = depName;
			ReturnedSaveFuncInfoWithValue<WebResponse<WebAttendanceDeviceInfo>> returnedSaveFuncInfoWithValue = await string.Concat(obj).GetFromApi<WebAttendanceDeviceInfo>(token, 1, null, 30000);
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "SetDepartmentId", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 281);
			ret.value = returnedSaveFuncInfoWithValue.value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetDepartmentId", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 286);
			ret.AddReturnedValue(ex, "", "SetDepartmentId", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 287);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<WebAttendanceDeviceInfo>>> LoadALLDataAsync(short? depid, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfoWithValue<List<WebAttendanceDeviceInfo>> ret = new ReturnedSaveFuncInfoWithValue<List<WebAttendanceDeviceInfo>>();
		try
		{
			string url = Url + "GetListOfDevice";
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			if (!depid.HasValue)
			{
				dictionary.Add("depId", "0");
			}
			else
			{
				dictionary.Add("depId", depid.Value.ToString());
			}
			dictionary.Add("UserName", DepAdmin.UserName);
			dictionary.Add("Password", DepAdmin.Password);
			ReturnedSaveFuncInfoWithValue<WebResponse<List<WebAttendanceDeviceInfo>>> returnedSaveFuncInfoWithValue = await url.GetFromApi<List<WebAttendanceDeviceInfo>>(token, 1, dictionary, 30000);
			if (returnedSaveFuncInfoWithValue.HasError)
			{
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "LoadALLDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 303);
			}
			else
			{
				ret.value = returnedSaveFuncInfoWithValue.value.Data;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadALLDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 309);
			ret.AddReturnedValue(ex, "", "LoadALLDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 310);
		}
		return ret;
	}

	public override void Dispose(bool disposing)
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttendanceDeviceInfo.cs", 322);
		}
	}
}
