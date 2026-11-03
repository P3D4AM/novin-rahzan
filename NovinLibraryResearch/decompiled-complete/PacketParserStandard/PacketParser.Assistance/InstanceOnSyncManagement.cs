using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using PacketParser.Entities;

namespace PacketParser.Assistance;

public class InstanceOnSyncManagement
{
	private class InnerContainer
	{
		private List<int> PacketIds = new List<int>();

		private int Max { get; set; }

		public InnerContainer(int max)
		{
			Max = max;
		}

		private bool PacketCompelete()
		{
			try
			{
				if (PacketIds.Count == Max)
				{
					return true;
				}
				return false;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "PacketCompelete", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\InstanceOnSyncManagement.cs", 104);
				return false;
			}
		}

		public bool AddPacketId(int packetId)
		{
			try
			{
				if (!PacketIds.Contains(packetId))
				{
					PacketIds.Add(packetId);
				}
				return PacketCompelete();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AddPacketId", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\InstanceOnSyncManagement.cs", 118);
				return false;
			}
		}
	}

	private static ConcurrentDictionary<Guid, InnerContainer> Dic_Packet = new ConcurrentDictionary<Guid, InnerContainer>();

	private static ConcurrentDictionary<Guid, InnerContainer> Dic_Reset_Packet = new ConcurrentDictionary<Guid, InnerContainer>();

	public static bool PacketManager(int PacketId, int MaxLength, Guid PacketGuid, bool Result = true)
	{
		try
		{
			if (!Result)
			{
				Dic_Packet.TryRemove(PacketGuid, out var _);
				return false;
			}
			if (PacketGuid == Guid.Empty)
			{
				WebErrorLog.Instance.StartLog(new Exception("PacketGuid  is empty"), "", "PacketManager", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\InstanceOnSyncManagement.cs", 23);
			}
			if (MaxLength <= 1)
			{
				return true;
			}
			if (!Dic_Packet.ContainsKey(PacketGuid))
			{
				InnerContainer innerContainer = new InnerContainer(MaxLength);
				bool num = innerContainer.AddPacketId(PacketId);
				Dic_Packet.TryAdd(PacketGuid, innerContainer);
				if (num)
				{
					Dic_Packet.TryRemove(PacketGuid, out var _);
				}
				return num;
			}
			Dic_Packet.TryGetValue(PacketGuid, out var value3);
			bool num2 = value3.AddPacketId(PacketId);
			if (num2)
			{
				Dic_Packet.TryRemove(PacketGuid, out var _);
			}
			return num2;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "PacketManager", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\InstanceOnSyncManagement.cs", 49);
			return false;
		}
	}

	public static bool ResetPacketManager(int PacketId, int MaxLength, Guid PacketGuid)
	{
		try
		{
			if (MaxLength <= 1 || PacketGuid == Guid.Empty)
			{
				return true;
			}
			if (!Dic_Reset_Packet.ContainsKey(PacketGuid))
			{
				InnerContainer innerContainer = new InnerContainer(MaxLength);
				innerContainer.AddPacketId(PacketId);
				Dic_Reset_Packet.TryAdd(PacketGuid, innerContainer);
				return true;
			}
			Dic_Reset_Packet.TryGetValue(PacketGuid, out var value);
			if (value.AddPacketId(PacketId))
			{
				Dic_Reset_Packet.TryRemove(PacketGuid, out var _);
			}
			return false;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ResetPacketManager", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\InstanceOnSyncManagement.cs", 79);
			return false;
		}
	}
}
