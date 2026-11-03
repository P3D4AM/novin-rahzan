using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using PacketParser.Entities;

namespace PacketParser.Assistance.Net;

public class NetworkAssistance : Serializable<NetworkAssistance>
{
	public string GateWayAddress { get; set; }

	public static string FindGateWay()
	{
		try
		{
			return (from g in (from n in NetworkInterface.GetAllNetworkInterfaces()
					where n.OperationalStatus == OperationalStatus.Up
					where n.NetworkInterfaceType != NetworkInterfaceType.Loopback
					select n).SelectMany((NetworkInterface n) => n.GetIPProperties()?.GatewayAddresses)
				select g?.Address).FirstOrDefault((IPAddress a) => a != null)?.ToString() ?? null;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FindGateWay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\NetworkAssistance.cs", 34);
			return null;
		}
	}

	private static Task<bool> PingHost(string nameOrAddress)
	{
		bool result = false;
		Ping ping = null;
		try
		{
			ping = new Ping();
			PingReply pingReply = ping.Send(nameOrAddress);
			result = pingReply != null && pingReply.Status == IPStatus.Success;
		}
		catch (PingException)
		{
			result = false;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "PingHost", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\NetworkAssistance.cs", 49);
		}
		finally
		{
			ping?.Dispose();
		}
		return Task.FromResult(result);
	}

	public static async Task<bool> CheckIntenetAccessAsync()
	{
		_ = 1;
		try
		{
			List<Task<bool>> lst = new List<Task<bool>>
			{
				PingHost("194.36.174.1"),
				PingHost("194.36.174.15"),
				PingHost("194.36.174.136"),
				PingHost("185.231.113.133"),
				PingHost("185.231.113.1"),
				PingHost("185.231.113.129"),
				PingHost("8.8.8.8"),
				PingHost("4.2.2.4"),
				PingHost("1.1.1.1"),
				PingHost("1.1.1.1")
			};
			while (lst.Any())
			{
				Task<bool> task = await Task.WhenAny(lst);
				lst.Remove(task);
				if (await task)
				{
					return true;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckIntenetAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\NetworkAssistance.cs", 78);
		}
		return false;
	}

	public static string GetLocalIpAddress()
	{
		string text = "";
		try
		{
			text = GetLocalIpAddressFromIfConfig();
			if (string.IsNullOrEmpty(text))
			{
				text = GetLocalIpAddressFromCheckIp();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetLocalIpAddress", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\NetworkAssistance.cs", 92);
		}
		return text;
	}

	public static string GetNetworkIpAddress()
	{
		try
		{
			IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
			foreach (IPAddress iPAddress in addressList)
			{
				if (iPAddress.AddressFamily == AddressFamily.InterNetwork)
				{
					return iPAddress.ToString();
				}
			}
			throw new Exception("No network adapters with an IPv4 address in the system!");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetNetworkIpAddress", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\NetworkAssistance.cs", 109);
			return null;
		}
	}

	public static string GetHtmlCode(string url)
	{
		string result = "";
		using (new JaegerMonitor("GetHtmlCode", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\NetworkAssistance.cs"))
		{
			try
			{
				HttpWebResponse httpWebResponse = (HttpWebResponse)((HttpWebRequest)WebRequest.Create(url)).GetResponse();
				if (httpWebResponse.StatusCode == HttpStatusCode.OK)
				{
					Stream responseStream = httpWebResponse.GetResponseStream();
					StreamReader streamReader = null;
					if (responseStream != null)
					{
						streamReader = (string.IsNullOrWhiteSpace(httpWebResponse.CharacterSet) ? new StreamReader(responseStream) : new StreamReader(responseStream, Encoding.GetEncoding(httpWebResponse.CharacterSet)));
					}
					result = streamReader?.ReadToEnd();
					httpWebResponse.Close();
					streamReader?.Close();
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetHtmlCode", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\NetworkAssistance.cs", 141);
			}
			return result;
		}
	}

	public static Task<List<NetworkAssistance>> GetAllAsync(string gateWayPath)
	{
		List<NetworkAssistance> list = new List<NetworkAssistance>();
		try
		{
			if (File.Exists(gateWayPath))
			{
				list = File.ReadAllText(gateWayPath).FromJson<List<NetworkAssistance>>("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\NetworkAssistance.cs", 155);
				if (list == null || !list.Any())
				{
					list = GetDefaultGateWays(gateWayPath);
				}
			}
			else
			{
				list = GetDefaultGateWays(gateWayPath);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\NetworkAssistance.cs", 160);
		}
		return Task.FromResult(list);
	}

	private static List<NetworkAssistance> GetDefaultGateWays(string gateWayPath)
	{
		List<NetworkAssistance> list = new List<NetworkAssistance>();
		try
		{
			list = new List<NetworkAssistance>
			{
				new NetworkAssistance
				{
					GateWayAddress = "192.168.1.5"
				},
				new NetworkAssistance
				{
					GateWayAddress = "192.168.1.2"
				},
				new NetworkAssistance
				{
					GateWayAddress = "192.168.1.4"
				}
			};
			if (string.IsNullOrEmpty(File.ReadAllText(gateWayPath)))
			{
				File.WriteAllText(gateWayPath, Json.ToStringJson(list));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetDefaultGateWays", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\NetworkAssistance.cs", 180);
		}
		return list;
	}

	public static async Task<NetworkAssistance> GetNextGateWayAsync(string gateWayPath)
	{
		NetworkAssistance ret = null;
		try
		{
			string gateWayNow = FindGateWay();
			List<NetworkAssistance> list = await GetAllAsync(gateWayPath);
			if (list.Count() == 2)
			{
				ret = ((gateWayNow == list?.FirstOrDefault()?.GateWayAddress) ? list[1] : list[0]);
			}
			else if (list.Count() == 3)
			{
				int index = new Random().Next(0, list.Count());
				ret = list[index];
			}
			else
			{
				ret = list[0];
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetNextGateWayAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\NetworkAssistance.cs", 202);
		}
		return ret;
	}

	private static string GetLocalIpAddressFromIfConfig()
	{
		try
		{
			HttpWebRequest obj = (HttpWebRequest)WebRequest.Create("http://ifconfig.me");
			obj.UserAgent = "curl";
			obj.Method = "GET";
			string text;
			using (WebResponse webResponse = obj.GetResponse())
			{
				using StreamReader streamReader = new StreamReader(webResponse.GetResponseStream());
				text = streamReader.ReadToEnd();
			}
			return text.Replace("\n", "");
		}
		catch (Exception ex)
		{
			WebTelegramMessage.GetNp_Advertise_bot().Sent2web("#GetLocalIpAddress//#Exception//" + ex.Message, null, "GetLocalIpAddressFromIfConfig", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\NetworkAssistance.cs", 225);
			return null;
		}
	}

	private static string GetLocalIpAddressFromCheckIp()
	{
		checked
		{
			try
			{
				string text = "";
				using (WebResponse webResponse = WebRequest.Create("http://checkip.dyndns.org/").GetResponse())
				{
					using StreamReader streamReader = new StreamReader(webResponse.GetResponseStream());
					text = streamReader.ReadToEnd();
				}
				int num = text.IndexOf("Address: ") + 9;
				int num2 = text.LastIndexOf("</body>");
				return text.Substring(num, num2 - num);
			}
			catch (Exception ex)
			{
				WebTelegramMessage.GetNp_Advertise_bot().Sent2web("#GetLocalIpAddress//#Exception//" + ex.Message, null, "GetLocalIpAddressFromCheckIp", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\NetworkAssistance.cs", 247);
				return null;
			}
		}
	}
}
