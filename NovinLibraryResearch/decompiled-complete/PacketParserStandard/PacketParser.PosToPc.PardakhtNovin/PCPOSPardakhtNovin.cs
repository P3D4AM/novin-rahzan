using System;
using System.IO.Ports;
using System.Net.Sockets;
using PacketParser.Entities;

namespace PacketParser.PosToPc.PardakhtNovin;

public class PCPOSPardakhtNovin : PcPosBase
{
	public override void SendToSerialPort()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			SerialPort obj = serialPort;
			if (obj != null)
			{
				obj.Close();
			}
			serialPort = new SerialPort(base.ComPort, base.BaudRate, (Parity)0, 8, (StopBits)1);
			serialPort.DataReceived += new SerialDataReceivedEventHandler(base.DataReceivedHandler);
			serialPort.RtsEnable = false;
			serialPort.DtrEnable = false;
			serialPort.ReadTimeout = 12000;
			serialPort.Open();
			serialPort.DiscardInBuffer();
			serialPort.DiscardOutBuffer();
			serialPort.Write(finaMsg, 0, finaMsg.Length);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendToSerialPort", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PardakhtNovin\\PCPOSPardakhtNovin.cs", 28);
			base.Response.RawResponse = "0018RS013RS00281PD0011";
			ReturnResponse(base.Response.RawResponse);
			SerialPort obj2 = serialPort;
			if (obj2 != null)
			{
				obj2.Close();
			}
		}
	}

	public bool TestConnection()
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		try
		{
			if (base.ConnectionType == cnType.LAN)
			{
				TcpClient tcpClient = new TcpClient();
				tcpClient.Connect(base.Ip, base.Port);
				if (!tcpClient.Connected)
				{
					return false;
				}
				tcpClient.Close();
				return true;
			}
			if (base.ConnectionType != cnType.SERIAL)
			{
				return false;
			}
			SerialPort val = new SerialPort(base.ComPort, base.BaudRate, (Parity)0, 8, (StopBits)1);
			val.Open();
			if (!val.IsOpen)
			{
				return false;
			}
			val.Close();
			return true;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TestConnection", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PardakhtNovin\\PCPOSPardakhtNovin.cs", 278);
			return false;
		}
	}
}
