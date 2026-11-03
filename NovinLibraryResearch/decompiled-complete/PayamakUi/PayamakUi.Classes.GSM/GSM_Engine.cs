using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using PacketParser.Entities;

namespace PayamakUi.Classes.GSM;

public class GSM_Engine
{
	public static Timer GSMCheckere = new Timer();

	public static bool Delivery { get; set; } = true;


	public static short PortNumber { get; set; } = 0;


	public static long PortSpeed { get; set; } = 0L;


	public GSM_Engine()
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
			WebErrorLog.Instance.StartLog(ex, "", "WebCheckere_Tick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\GSM\\GSM_Engine.cs", 29);
		}
		return true;
	}
}
