using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PacketParser.BusinessLayer;

namespace PacketParser.Entities;

public class WebChequeQrParserTemplate
{
	private static string apiUrl = HttpHelpers.ApiUrl + "ChequeQrParserTemplate/";

	public Guid Guid { get; set; }

	public EnBank BankType { get; set; }

	[JsonIgnore]
	public short BankTypeShort => checked((short)BankType);

	[JsonIgnore]
	public string BankTypeForDisplay => BankType.GetDisplay();

	public List<string> RegexList { get; set; }

	[JsonIgnore]
	public int RegexCount => RegexList?.Count ?? 0;

	public static async Task<List<WebChequeQrParserTemplate>> GetAllAsync(CancellationToken token = default(CancellationToken), int timeOutMiliseconds = 15000)
	{
		try
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<List<WebChequeQrParserTemplate>>> returnedSaveFuncInfoWithValue = await (apiUrl + "GetAll").GetFromApi<List<WebChequeQrParserTemplate>>(token, 3, null, timeOutMiliseconds);
			if (returnedSaveFuncInfoWithValue.HasError)
			{
				return null;
			}
			return returnedSaveFuncInfoWithValue.value.Data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebChequeQrParserTemplate.cs", 31);
		}
		return null;
	}
}
