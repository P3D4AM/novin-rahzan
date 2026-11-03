using System;
using System.Threading;
using System.Threading.Tasks;
using ClientHub.AccountingHubs;
using PacketParser.Entities;

namespace PayamakUi.Classes;

public class CLSMain
{
	public delegate void OutBox_InsertEventHandler();

	public static event OutBox_InsertEventHandler OutBox_Insert;

	public CLSMain()
	{
		try
		{
			Task.Run((Action)Process.StartGSMEngine);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\CLSMain.cs", 30);
		}
	}

	public static async Task<string> SentBoxCountAsync(string contextGuid, CancellationToken token = default(CancellationToken))
	{
		try
		{
			return "(" + (await PayamakClientHubInstances.GetInstance(contextGuid).SmsSent.CountAsync(token)).ToString() + ")";
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SentBoxCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\CLSMain.cs", 93);
			return "";
		}
	}

	public static void OutBox_Insert_()
	{
		try
		{
			CLSMain.OutBox_Insert?.Invoke();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OutBox_Insert_", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\CLSMain.cs", 106);
		}
	}
}
