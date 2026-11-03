using System;
using System.Threading.Tasks;
using PacketParser.Entities;

namespace PayamakUi.Classes.Web;

internal class WebEngine
{
	public WebEngine()
	{
		Task.Run(() => WebCheckere_Tick());
	}

	private static async Task<bool> WebCheckere_Tick()
	{
		try
		{
			Process.StartWebEngine();
			await Task.Delay(1800000);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "WebCheckere_Tick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\Web\\WebEngine.cs", 24);
		}
		return true;
	}
}
