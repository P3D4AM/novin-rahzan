using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Attendance;

namespace PacketParser.Entities.Attendance;

public class WebCalender : Serializable<WebCalender>, ICalender, IHasJustGuid, IHasStatus
{
	private static string _url = HttpHelpers.ApiUrl + "Attendance/Calendar/";

	public DateTime DateM { get; set; }

	public string Monasebat { get; set; }

	public string Description { get; set; }

	public bool STRasmi { get; set; }

	public DateTime Modified { get; set; }

	public Guid Guid { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public static async Task<ReturnedSaveFuncInfoWithValue<List<WebCalender>>> GetAllAsync(CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<List<WebCalender>> ret = new ReturnedSaveFuncInfoWithValue<List<WebCalender>>
		{
			value = new List<WebCalender>()
		};
		try
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<List<WebCalender>>> returnedSaveFuncInfoWithValue = await (_url + "WebTaghvimNew").GetFromApi<List<WebCalender>>(token, 1, null, 60000);
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebCalender.cs", 31);
			if (ret.HasError)
			{
				return ret;
			}
			if (returnedSaveFuncInfoWithValue.value.HttpClietnStatusCode == HttpStatusCode.OK)
			{
				ret.value = returnedSaveFuncInfoWithValue.value.Data;
			}
			else
			{
				ret.AddError(EnSystem.None, returnedSaveFuncInfoWithValue.value.HttpClietnStatusCode.ToString(), "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebCalender.cs", 36);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebCalender.cs", 40);
			ret.AddReturnedValue(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Attendance\\WebCalender.cs", 41);
		}
		return ret;
	}
}
