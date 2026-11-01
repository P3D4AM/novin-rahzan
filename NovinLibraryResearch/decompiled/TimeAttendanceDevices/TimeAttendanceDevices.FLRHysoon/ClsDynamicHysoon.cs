using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using Serilog;

namespace TimeAttendanceDevices.FLRHysoon;

public class ClsDynamicHysoon
{
	public Socket c_socket;

	public IPEndPoint point;

	public IAsyncResult asyncResult;

	public int DevicePwd;

	public string DeviceIp = "192.168.1.224";

	public int DevicePort = 5005;

	public const int PROTOCOLKEY = 404232216;

	public byte[] Header = new byte[32];

	public const int LEN = 4;

	public const int HEADERLEN = 32;

	public int ReceiveBufferSize = 409600;

	public const int PACKAGE_ID = 0;

	public ClsDynamicHysoon(string deviceIp, int devicePort, int devicePwd)
	{
		DeviceIp = deviceIp;
		DevicePort = devicePort;
		DevicePwd = devicePwd;
	}

	public bool CheckConnectionToDevice()
	{
		try
		{
			if (Connect())
			{
				return true;
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, $"Connect to ip({DeviceIp} and port ({DevicePort}) and devicePass ({DevicePwd})) failed!");
		}
		finally
		{
			try
			{
				DisConnect();
			}
			catch (Exception ex2)
			{
				Log.Error(ex2, "");
			}
		}
		return false;
	}

	public bool Connect()
	{
		try
		{
			c_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			IPAddress address = IPAddress.Parse(DeviceIp);
			point = new IPEndPoint(address, DevicePort);
			c_socket.SendTimeout = 20000;
			c_socket.ReceiveTimeout = 20000;
			c_socket.Connect(point);
			return true;
		}
		catch (Exception ex)
		{
			Log.Error(ex, $"Connect to ip({DeviceIp} and port ({DevicePort}) and devicePass ({DevicePwd})) failed!");
		}
		return false;
	}

	public void DisConnect()
	{
		try
		{
			if (c_socket != null && c_socket.Connected)
			{
				c_socket.Shutdown(SocketShutdown.Both);
				c_socket.Close();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
	}

	public byte[] GetSendBuff(byte[] sendBuff)
	{
		byte[] array = new byte[32 + sendBuff.Length];
		try
		{
			byte[] arry = new byte[4];
			if (ConvertIntToByteArray(sendBuff.Length, ref arry))
			{
				Buffer.BlockCopy(arry, 0, Header, 0, 4);
			}
			if (ConvertIntToByteArray(404232216, ref arry))
			{
				Buffer.BlockCopy(arry, 0, Header, 4, 4);
			}
			if (ConvertIntToByteArray(DevicePwd, ref arry))
			{
				Buffer.BlockCopy(arry, 0, Header, 8, 4);
			}
			Buffer.BlockCopy(Header, 0, array, 0, 32);
			Buffer.BlockCopy(sendBuff, 0, array, 32, sendBuff.Length);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return array;
	}

	public static bool ConvertIntToByteArray(int m, ref byte[] arry)
	{
		try
		{
			if (arry == null)
			{
				return false;
			}
			if (arry.Length < 4)
			{
				return false;
			}
			arry[0] = (byte)((uint)m & 0xFFu);
			arry[1] = (byte)((m & 0xFF00) >> 8);
			arry[2] = (byte)((m & 0xFF0000) >> 16);
			arry[3] = (byte)((uint)(m >> 24) & 0xFFu);
			return true;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return false;
	}

	public bool GetData(byte[] buffer, ref StringBuilder jsonStringBuilder)
	{
		try
		{
			for (int i = 0; i < 3; i++)
			{
				try
				{
					c_socket.Send(buffer);
				}
				catch
				{
					continue;
				}
				break;
			}
			Receive(ref jsonStringBuilder);
			return true;
		}
		catch (Exception ex)
		{
			Log.Error("send data to device has error:" + ex.ToString());
			return false;
		}
		finally
		{
			DisConnect();
		}
	}

	private bool SendData(ref StringBuilder jsonStringBuilder)
	{
		try
		{
			if (!Connect())
			{
				return false;
			}
			byte[] bytes = Encoding.UTF8.GetBytes(jsonStringBuilder.ToString());
			jsonStringBuilder = new StringBuilder("");
			if (GetData(GetSendBuff(bytes), ref jsonStringBuilder))
			{
				return true;
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return false;
	}

	public void Receive(ref StringBuilder jsonStringBuilder)
	{
		while (true)
		{
			try
			{
				if (!c_socket.Connected)
				{
					break;
				}
				byte[] array = new byte[ReceiveBufferSize];
				int num = c_socket.Receive(array);
				if (num == 0)
				{
					break;
				}
				if (num <= 1 || num == 32)
				{
					continue;
				}
				string @string = Encoding.UTF8.GetString(array, 0, num);
				if (@string.Length > 0)
				{
					jsonStringBuilder.Append(@string);
					@string = @string.Replace("\n", "");
					int num2 = @string.LastIndexOf("}\0");
					if (num < ReceiveBufferSize && num2 > 0)
					{
						break;
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error("The dynamicDevice has been disconnected!\r\n" + ex.Message);
				break;
			}
		}
	}

	public bool Receive(StringBuilder jsonStringBuilder, ref StringBuilder result)
	{
		try
		{
			string text = jsonStringBuilder.ToString();
			if (!text.Contains("cmd"))
			{
				result.AppendLine("فرمان دریافت داده از دستگاه نادرست است");
				return false;
			}
			DynamicHysoonInfo._AnswerInfo answerInfo = JsonConvert.DeserializeObject<DynamicHysoonInfo._AnswerInfo>(text);
			if (answerInfo.result_code == 0)
			{
				return true;
			}
			result.AppendLine($"فرمان دریافت داده از دستگاه با خطای کد ({answerInfo.result_code}) مواجه شد");
			return false;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			return false;
		}
	}

	public List<DynamicHysoonInfo.Logs> GetLogData(DateTime fromTime, DateTime toTime, bool onlyNewLog = false, bool clearNewLogs = false)
	{
		List<DynamicHysoonInfo.Logs> list = new List<DynamicHysoonInfo.Logs>();
		try
		{
			string beginTime = fromTime.Date.ToString("yyyyMMdd");
			string endTime = toTime.Date.ToString("yyyyMMdd");
			byte newLog = 0;
			if (onlyNewLog)
			{
				newLog = 1;
			}
			DynamicHysoonInfo.GetLogDataCmd data = new DynamicHysoonInfo.GetLogDataCmd(0, newLog, beginTime, endTime, Convert.ToByte(clearNewLogs));
			DynamicHysoonInfo._DeviceCmd<DynamicHysoonInfo.GetLogDataCmd> deviceCmd = new DynamicHysoonInfo._DeviceCmd<DynamicHysoonInfo.GetLogDataCmd>("GetLogData", data);
			while (true)
			{
				StringBuilder jsonStringBuilder = new StringBuilder(JsonConvert.SerializeObject((object)deviceCmd));
				SendData(ref jsonStringBuilder);
				StringBuilder result = new StringBuilder();
				if (!Receive(jsonStringBuilder, ref result))
				{
					break;
				}
				DynamicHysoonInfo._ResultInfo<DynamicHysoonInfo.RecordInfo<DynamicHysoonInfo.Logs>> resultInfo = JsonConvert.DeserializeObject<DynamicHysoonInfo._ResultInfo<DynamicHysoonInfo.RecordInfo<DynamicHysoonInfo.Logs>>>(jsonStringBuilder.ToString());
				if (resultInfo.result_data.logs == null)
				{
					return list;
				}
				foreach (DynamicHysoonInfo.Logs log in resultInfo.result_data.logs)
				{
					list.Add(log);
				}
				if (resultInfo.result_data.packageId != 0)
				{
					deviceCmd.data.packageId++;
					continue;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return list;
	}

	public bool SetDateTime(DateTime date)
	{
		try
		{
			StringBuilder result = new StringBuilder();
			DynamicHysoonInfo.SetTimeCmd data = new DynamicHysoonInfo.SetTimeCmd(date.ToString("yyyyMMddHHmmss"));
			StringBuilder jsonStringBuilder = new StringBuilder(JsonConvert.SerializeObject((object)new DynamicHysoonInfo._DeviceCmd<DynamicHysoonInfo.SetTimeCmd>("SetTime", data)));
			SendData(ref jsonStringBuilder);
			Receive(jsonStringBuilder, ref result);
			return true;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return false;
	}
}
