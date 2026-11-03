using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using PacketParser.BusinessLayer;
using PacketParser.DepartmentApiModel;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Attendance;
using PacketParser.Entities.MetaData.Attendance;
using PacketParser.EventArgs;

namespace PacketParser.Entities.Attendance;

public class WebAttDepartment : Serializable<WebAttDepartment>, IAttDepartment, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, INpDisposable, IDisposable, IReceiver
{
	private static string APIURL = HttpHelpers.ApiUrl + "api/Attendance/Section/";

	public short DepID { get; set; }

	public Guid Guid { get; set; }

	public bool Status { get; set; }

	public DateTime Modified { get; set; }

	public string Name { get; set; }

	public Guid? AdminGuid { get; set; }

	public DateTime CreatedDate { get; set; }

	public AttDepartmentMetaData MetaData { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed { get; private set; }

	public event Func<object, ConnectionEventArgs<WebAttDepartment>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebAttDepartment> list, int partitionId = 0)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttDepartment.cs"))
		{
			try
			{
				if (list == null || !list.Any())
				{
					throw new NullReferenceException();
				}
				object result;
				if (list == null)
				{
					result = null;
				}
				else
				{
					result = list.GeneratePackets(PacketType.AttDepartment, 1, null, isSync: false, null, partitionId, deliveryRequst: false, "GenerateSaveRequest");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttDepartment.cs", 43);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttDepartment.cs"))
		{
			try
			{
				if (responses == null || responses.Count() <= 0)
				{
					throw new NullReferenceException();
				}
				return responses?.GeneratePackets(PacketType.AttDepartment, 2, null, isSync: false, null, 0, deliveryRequst: false, "GenerateSaveResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttDepartment.cs", 57);
				throw;
			}
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				if (this.OnSaveRequest != null)
				{
					this.OnSaveRequest = null;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttDepartment.cs", 69);
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<WebAttDepartment>>> GetFromApiAsync(string depUserName, string deppass, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<List<WebAttDepartment>> response = new ReturnedSaveFuncInfoWithValue<List<WebAttDepartment>>();
		try
		{
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = await new RestTokenManagement(ENSource.TimeAttendance, depUserName, deppass).GetToken(token);
			response.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetFromApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttDepartment.cs", 78);
			if (response.HasError)
			{
				return response;
			}
			Dictionary<string, string> headers = new Dictionary<string, string>
			{
				{
					"Authorization",
					(returnedSaveFuncInfoWithValue.value ?? "") ?? ""
				},
				{
					"UserName",
					(depUserName ?? "") ?? ""
				},
				{ "IsAdmin", "yes" }
			};
			ReturnedSaveFuncInfoWithValue<WebResponse<ReturnedSaveFuncInfoWithValue<List<WebAttDepartment>>>> returnedSaveFuncInfoWithValue2 = await (APIURL + "GetAll").GetFromApi<ReturnedSaveFuncInfoWithValue<List<WebAttDepartment>>>(token, 3, headers);
			if (returnedSaveFuncInfoWithValue2.HasError)
			{
				foreach (Tuple<string, string> error in returnedSaveFuncInfoWithValue2.Errors)
				{
					response.Errors.Add(error);
				}
				return response;
			}
			if (returnedSaveFuncInfoWithValue2.value.Data.HasError)
			{
				foreach (Tuple<string, string> error2 in returnedSaveFuncInfoWithValue2.value.Data.Errors)
				{
					response.Errors.Add(error2);
				}
				return response;
			}
			response.value = returnedSaveFuncInfoWithValue2.value.Data.value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetFromApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttDepartment.cs", 99);
			await response.AddErrorAsync(ex.Message, "", "GetFromApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttDepartment.cs", 99);
		}
		return response;
	}

	public async Task DataReceived(Packet pack, string ConnectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttDepartment.cs"))
		{
			_ = 1;
			try
			{
				switch ((AttDepartmentActions)pack.PacketActions)
				{
				case AttDepartmentActions.SaveRequest:
					if (this.OnSaveRequest != null)
					{
						ConnectionEventArgs<WebAttDepartment> args2 = new ConnectionEventArgs<WebAttDepartment>(pack, ConnectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnSaveRequest, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttDepartment.cs");
					}
					break;
				case AttDepartmentActions.SaveResponse:
					if (this.OnSaveResponse != null)
					{
						SaveResponseEventArgs args = new SaveResponseEventArgs(pack, ConnectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnSaveResponse, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttDepartment.cs");
					}
					break;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebAttDepartment.cs", 123);
			}
		}
	}

	public string GetPacketActionName(short action)
	{
		return ((AttDepartmentActions)action).GetDisplay();
	}

	public string GetPacketActionEnumName(short action)
	{
		AttDepartmentActions attDepartmentActions = (AttDepartmentActions)action;
		return attDepartmentActions.ToString();
	}
}
