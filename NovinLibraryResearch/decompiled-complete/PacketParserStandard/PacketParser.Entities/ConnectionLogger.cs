using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public class ConnectionLogger : IConnectionLogger
{
	private static readonly ConcurrentDictionary<long, ConnectionLogger> _dic = new ConcurrentDictionary<long, ConnectionLogger>();

	private ConcurrentDictionary<PacketType, ConnectionLoggerDets> _LogReceived = new ConcurrentDictionary<PacketType, ConnectionLoggerDets>();

	private ConcurrentDictionary<PacketType, ConnectionLoggerDets> _LogSend = new ConcurrentDictionary<PacketType, ConnectionLoggerDets>();

	public long HashId { get; set; }

	public List<ConnectionLoggerDets> LogReceivedDets
	{
		get
		{
			return _LogReceived.Values?.ToList();
		}
		set
		{
		}
	}

	public List<ConnectionLoggerDets> LogSendDets
	{
		get
		{
			return _LogSend.Values?.ToList();
		}
		set
		{
		}
	}

	public static void SaveLogReceived(int hashId, Packet packet, string connectionUniqueId, long elapsedMiliseconds)
	{
		using (new JaegerMonitor("SaveLogReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\ConnectionLogger.cs"))
		{
			try
			{
				_dic.TryGetValue(hashId, out var value);
				if (value == null)
				{
					value = new ConnectionLogger
					{
						HashId = hashId
					};
					_dic.TryAdd(hashId, value);
				}
				value.SaveLogReceived(packet, connectionUniqueId, elapsedMiliseconds);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveLogReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\ConnectionLogger.cs", 61);
			}
		}
	}

	private void SaveLogReceived(Packet packet, string connectionUniqueId, long elapsedMiliseconds)
	{
		using (new JaegerMonitor("SaveLogReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\ConnectionLogger.cs"))
		{
			try
			{
				if (packet != null)
				{
					_LogReceived.TryGetValue(packet.PacketType, out var value);
					if (value == null)
					{
						value = new ConnectionLoggerDets();
						_LogReceived.TryAdd(packet.PacketType, value);
					}
					value.ProgressPacket(packet, connectionUniqueId, elapsedMiliseconds);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveLogReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\ConnectionLogger.cs", 79);
			}
		}
	}

	private void SaveLogSend(Packet packet, string connectionUniqueId, long elapsedMiliseconds)
	{
		using (new JaegerMonitor("SaveLogSend", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\ConnectionLogger.cs"))
		{
			try
			{
				if (packet != null)
				{
					_LogSend.TryGetValue(packet.PacketType, out var value);
					if (value == null)
					{
						value = new ConnectionLoggerDets();
						_LogSend.TryAdd(packet.PacketType, value);
					}
					value.ProgressPacket(packet, connectionUniqueId, elapsedMiliseconds);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveLogSend", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\ConnectionLogger.cs", 97);
			}
		}
	}

	public static void SaveLogSend(long hashId, Packet packet, string connectionUniqueId, long elapsedMiliseconds)
	{
		using (new JaegerMonitor("SaveLogSend", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\ConnectionLogger.cs"))
		{
			try
			{
				_dic.TryGetValue(hashId, out var value);
				if (value == null)
				{
					value = new ConnectionLogger
					{
						HashId = hashId
					};
					_dic.TryAdd(hashId, value);
				}
				value.SaveLogSend(packet, connectionUniqueId, elapsedMiliseconds);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveLogSend", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\ConnectionLogger.cs", 115);
			}
		}
	}

	public static ConnectionLogger GetAll(int hashId)
	{
		using (new JaegerMonitor("GetAll", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\ConnectionLogger.cs"))
		{
			try
			{
				_dic.TryGetValue(hashId, out var value);
				return value;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAll", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\ConnectionLogger.cs", 145);
				return null;
			}
		}
	}
}
