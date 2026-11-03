using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PacketParser.BusinessLayer;

namespace PacketParser.Entities;

public class WebShabaTemplate
{
	private static string apiUrl = HttpHelpers.ApiUrl + "ShabaTemplate/";

	public Guid Guid { get; set; }

	public EnBank BankType { get; set; }

	[JsonIgnore]
	public string BankTypeForDisplay => BankType.GetDisplay();

	public short Number { get; set; }

	public static async Task<List<WebShabaTemplate>> GetAllAsync(CancellationToken token = default(CancellationToken), int timeOutMiliseconds = 15000)
	{
		try
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<List<WebShabaTemplate>>> returnedSaveFuncInfoWithValue = await (apiUrl + "GetAll").GetFromApi<List<WebShabaTemplate>>(token, 3, null, timeOutMiliseconds);
			if (returnedSaveFuncInfoWithValue.HasError)
			{
				return null;
			}
			return returnedSaveFuncInfoWithValue.value.Data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebShabaTemplate.cs", 28);
		}
		return null;
	}
}
