using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;

namespace PacketParser.Entities.Message;

public class WebMessageLog
{
	private static string apiUrl = HttpHelpers.ApiUrl;

	public int ID { get; set; }

	public short DepID { get; set; }

	public long SenderNumber { get; set; }

	public string DepUsername { get; set; }

	public string Description { get; set; }

	public DateTime Date { get; set; }

	public string DateSH => Date.MiladiToCalture();

	public ENWebMessageLogActionType ActionType { get; set; }

	public string ActionTypeAsString => ActionType.GetDisplay();

	public static WebMessageLog Factory(short depid, long senderNumber, string depusername, string description, ENWebMessageLogActionType actiontype)
	{
		try
		{
			return new WebMessageLog
			{
				DepID = depid,
				SenderNumber = senderNumber,
				DepUsername = depusername,
				Description = description,
				ActionType = actiontype
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Factory", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\WebMessageLog.cs", 36);
			return null;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<WebMessageLog>>> GetLogsAsync(long senderNumber, string userName, string password, CancellationToken token, string apiAddress = null)
	{
		try
		{
			Dictionary<string, string> headers = new Dictionary<string, string> { 
			{
				"mobileNumber",
				senderNumber.ToString()
			} };
			ReturnedSaveFuncInfoWithValue<WebResponse<ReturnedSaveFuncInfoWithValue<List<WebMessageLog>>>> returnedSaveFuncInfoWithValue = await ((apiAddress ?? apiUrl) + "Message/Session/GetLogs").GetFromApi<ReturnedSaveFuncInfoWithValue<List<WebMessageLog>>>(userName, password, ENSource.SendMessageTelegram, token, 3, headers);
			if (returnedSaveFuncInfoWithValue.HasError)
			{
				returnedSaveFuncInfoWithValue.throwExceptionIfError();
			}
			return returnedSaveFuncInfoWithValue.value.Data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetLogsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\WebMessageLog.cs", 52);
			throw;
		}
	}
}
