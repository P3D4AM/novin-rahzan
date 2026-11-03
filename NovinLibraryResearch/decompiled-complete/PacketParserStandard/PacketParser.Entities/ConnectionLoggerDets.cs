using System;

namespace PacketParser.Entities;

public class ConnectionLoggerDets
{
	public string ConnectionUniqueId;

	public PacketType packType { get; set; }

	public long totalBytes { get; set; }

	public int count { get; set; }

	public long TotalElapsedMiliseconds { get; set; }

	public DateTime StarTime { get; set; } = DateTime.Now;


	public DateTime LastTime { get; set; } = DateTime.Now;


	public void ProgressPacket(Packet pack, string ConnectionUniqueId, long elapsedTime)
	{
		checked
		{
			using (new JaegerMonitor("ProgressPacket", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\ConnectionLoggerDets.cs"))
			{
				int num = 0;
				try
				{
					num = 1;
					if (pack != null)
					{
						num = 2;
						packType = pack.PacketType;
						num = 3;
						count++;
						num = 4;
						TotalElapsedMiliseconds += elapsedTime;
						num = 5;
						totalBytes += pack.ToBytes().Length;
						num = 6;
						LastTime = DateTime.Now;
						num = 7;
					}
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, $"#Line_{num}\r\n" + $"Count:{count}\r\n" + "Data:" + (pack?.ToString() ?? "is null!!"), "ProgressPacket", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\ConnectionLoggerDets.cs", 37);
					throw;
				}
			}
		}
	}
}
