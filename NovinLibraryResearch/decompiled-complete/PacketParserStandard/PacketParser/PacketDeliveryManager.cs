using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.EventArgs;
using PacketParser.TransferLayer.Interfaces;

namespace PacketParser;

public class PacketDeliveryManager
{
	private static readonly ConcurrentDictionary<string, PacketDeliveryManager> _dic = new ConcurrentDictionary<string, PacketDeliveryManager>();

	private static bool Inited = false;

	public short TryCount { get; set; }

	public ConcurrentDictionary<string, TaskCompletionSource<bool>> dicPacketsUniqueKey { get; set; } = new ConcurrentDictionary<string, TaskCompletionSource<bool>>();


	public ConcurrentBag<string> Guids { get; set; } = new ConcurrentBag<string>();


	public PacketDeliveryManager()
	{
		using (new JaegerMonitor(".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PacketDeliveryManager.cs"))
		{
		}
	}

	public static async Task<bool> Send(ISender sender, Packet packet, WebContextHolder webContext, int TimeOutSeconds, short RetryCount, short NextRetrySeconds, string connectionUniqueId)
	{
		if (packet == null)
		{
			return false;
		}
		checked
		{
			try
			{
				WebErrorLog.Instance.Trace($"http://DataSent.com{packet.PacketType}", "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PacketDeliveryManager.cs", 36);
				if (!Inited)
				{
					webContext.Packet.OnResponse += PacketInstance_OnResponse;
				}
				Inited = true;
				packet.DeliveryRequested = true;
				_dic.TryGetValue(packet.UniqueKey(), out var pManager);
				if (pManager == null)
				{
					pManager = new PacketDeliveryManager();
					_dic.TryAdd(packet.UniqueKey(), pManager);
				}
				for (int retryCount = 0; retryCount < RetryCount; retryCount++)
				{
					pManager.dicPacketsUniqueKey.TryAdd(packet.UniqueKey(), new TaskCompletionSource<bool>());
					Task.Run(() => sender.SendData(packet, connectionUniqueId));
					bool flag = await pManager.PartExistsWithTimeout(packet.Part, connectionUniqueId, packet, TimeOutSeconds);
					if (flag)
					{
						return flag;
					}
					await Task.Delay(NextRetrySeconds * 1000);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PacketDeliveryManager.cs", 65);
			}
			packet.SendResult = false;
			return false;
		}
	}

	private async Task<bool> PartExistsWithTimeout(int part, string guid, Packet packet, int timeOutSeconds)
	{
		try
		{
			Task timeout = Task.Delay(checked(timeOutSeconds * 1000));
			if (await Task.WhenAny(new Task[2]
			{
				dicPacketsUniqueKey[packet.UniqueKey()].Task,
				timeout
			}) == timeout)
			{
				return false;
			}
			if (guid == null)
			{
				return true;
			}
			return Guids.Contains(guid);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "PartExistsWithTimeout", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PacketDeliveryManager.cs", 87);
		}
		return false;
	}

	private static async Task PacketInstance_OnResponse(object sender, BaseEventArgs e)
	{
		int num = 0;
		using (new JaegerMonitor("PacketInstance_OnResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PacketDeliveryManager.cs"))
		{
			string text = "";
			try
			{
				num = 1;
				WebErrorLog.Instance.Trace("PacketInstance_OnResponse:" + e.ToString(), "", "PacketInstance_OnResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PacketDeliveryManager.cs", 99);
				num = 2;
				_dic.TryGetValue(e.PacketReferenceId.ToString() + e.Part, out var value);
				num = 3;
				if (value != null)
				{
					num = 40;
					text = text + "//e.UniqueKey():" + e.UniqueKey() + "//";
					num = 41;
					if (value.dicPacketsUniqueKey.ContainsKey(e.UniqueKey()))
					{
						num = 42;
						text = text + "//ConnectionUniqueId:" + e.ConnectionUniqueId + "//";
						num = 43;
						text = text + "//temp.guids:[" + string.Join(",", value.Guids) + "]//";
						num = 44;
						num = 50;
						value.Guids.Add(e.ConnectionUniqueId);
						num = 6;
						value.dicPacketsUniqueKey[e.UniqueKey()].TrySetResult(result: true);
						num = 7;
					}
					num = 8;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{num}*{text}*", "PacketInstance_OnResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PacketDeliveryManager.cs", 130);
			}
		}
	}
}
