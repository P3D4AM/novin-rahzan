using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using PacketParser.Entities;
using Serilog;

namespace PacketParser.LogPacket;

public class ExpiredPacketLogger
{
	private int delay = 300000;

	public static ConcurrentDictionary<PacketSignature, ExpirationProperties> queue = new ConcurrentDictionary<PacketSignature, ExpirationProperties>();

	private static List<string> _exceptions = new List<string>();

	public ExpiredPacketLogger(int delay)
	{
		this.delay = delay;
	}

	public void GetOrAdd(Packet packet, Action<PacketSignature> action, bool isReceivedPacket, string actionString)
	{
		try
		{
			if (action == null)
			{
				action = delegate(PacketSignature p)
				{
					try
					{
						if (queue.TryGetValue(p, out var value2) && !value2.IsProcessed)
						{
							Log.Warning("Packet not processed.\r\n" + $"Type({p.Type}):{p.Type.GetDisplay()}\r\n" + $"Action({p.Action}):{actionString}\r\n" + $"IsReceivedPacket:{isReceivedPacket}");
						}
					}
					catch (Exception ex3)
					{
						WebErrorLog.Instance.StartLog(ex3, "", "GetOrAdd", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\LogPacket\\ExpiredPacketLogger.cs", 41);
					}
				};
			}
			PacketSignature packetSignature = default(PacketSignature);
			packetSignature.Action = packet.PacketActions;
			packetSignature.Type = packet.PacketType;
			packetSignature.IsReceivedPacket = isReceivedPacket;
			PacketSignature key = packetSignature;
			if (queue.TryGetValue(key, out var _) || queue.GetOrAdd(key, new ExpirationProperties
			{
				Action = action,
				IsProcessed = false,
				Timeout = Task.Delay(delay)
			}).IsProcessed)
			{
				return;
			}
			Task.Run(delegate
			{
				try
				{
					action?.Invoke(new PacketSignature
					{
						Action = packet.PacketActions,
						Type = packet.PacketType
					});
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "GetOrAdd", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\LogPacket\\ExpiredPacketLogger.cs", 61);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetOrAdd", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\LogPacket\\ExpiredPacketLogger.cs", 64);
		}
	}

	public static void MarkPacketAsProcced(PacketType type, short action, bool isReceivedPacket)
	{
		try
		{
			PacketSignature packetSignature = default(PacketSignature);
			packetSignature.Action = action;
			packetSignature.Type = type;
			packetSignature.IsReceivedPacket = isReceivedPacket;
			PacketSignature key = packetSignature;
			if (queue.TryGetValue(key, out var value))
			{
				value.IsProcessed = true;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MarkPacketAsProcced", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\LogPacket\\ExpiredPacketLogger.cs", 90);
		}
	}
}
