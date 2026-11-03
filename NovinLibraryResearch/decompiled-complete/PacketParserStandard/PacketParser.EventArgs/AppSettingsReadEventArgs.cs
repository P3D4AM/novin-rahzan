using System;
using PacketParser.Entities;

namespace PacketParser.EventArgs;

public class AppSettingsReadEventArgs : BaseEventArgs
{
	public DateTime Modified { get; set; }

	public static AppSettingsReadEventArgs FromString(Packet pack, string data)
	{
		AppSettingsReadEventArgs appSettingsReadEventArgs = data.FromJson<AppSettingsReadEventArgs>("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\AppSettingsReadEventArgs.cs", 12);
		appSettingsReadEventArgs.Part = pack.Part;
		appSettingsReadEventArgs.TotalParts = pack.TotalParts;
		appSettingsReadEventArgs.PacketReferenceId = pack.PacketReferenceId;
		return appSettingsReadEventArgs;
	}

	public override string ToString()
	{
		using (new JaegerMonitor("ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\AppSettingsReadEventArgs.cs"))
		{
			try
			{
				return Json.ToStringJson(this);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\AppSettingsReadEventArgs.cs", 28);
				throw;
			}
		}
	}
}
