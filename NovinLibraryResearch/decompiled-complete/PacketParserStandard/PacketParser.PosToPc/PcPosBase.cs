using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using PacketParser.Entities;

namespace PacketParser.PosToPc;

public abstract class PcPosBase
{
	public enum cnType
	{
		LAN,
		SERIAL
	}

	public delegate void ResponseEventHandler(string response);

	private TcpClient client;

	private Thread tReader;

	public Dictionary<string, string> Message_Tags = new Dictionary<string, string>();

	public SerialPort serialPort;

	public byte[] finaMsg;

	public int BaudRate { get; set; }

	public string Amount { get; set; }

	public string Currency { get; set; }

	public string PrCode { get; set; }

	public string R1Holder { get; set; }

	public string R3Holder { get; set; }

	public string R5Holder { get; set; }

	public string R7Holder { get; set; }

	public string R9Holder { get; set; }

	public string R2Merchant { get; set; }

	public string R4Merchant { get; set; }

	public string R6Merchant { get; set; }

	public string R8Merchant { get; set; }

	public string R0Merchant { get; set; }

	public string T1Holder { get; set; }

	public string T2Merchant { get; set; }

	public string Service { get; set; }

	public string ServiceGroup { get; set; }

	public string Settel { get; set; }

	public string KeyValue { get; set; }

	public string BIllID { get; set; }

	public string PaymentID { get; set; }

	public string TerminalID { get; set; }

	public string FoodSafety { get; set; }

	public string ShiftOpen { get; set; }

	public string ShiftClose { get; set; }

	public string SignCode { get; set; }

	public string ComPort { get; set; }

	public string Ip { get; set; }

	public int Port { get; set; }

	public string Amount1 { get; set; }

	public string Amount2 { get; set; }

	public string Amount3 { get; set; }

	public string Amount4 { get; set; }

	public string Amount5 { get; set; }

	public string Amount6 { get; set; }

	public string Amount7 { get; set; }

	public string Amount8 { get; set; }

	public string Amount9 { get; set; }

	public string Amount10 { get; set; }

	public string ID1 { get; set; }

	public string ID2 { get; set; }

	public string ID3 { get; set; }

	public string ID4 { get; set; }

	public string ID5 { get; set; }

	public string ID6 { get; set; }

	public string ID7 { get; set; }

	public string ID8 { get; set; }

	public string ID9 { get; set; }

	public string ID10 { get; set; }

	public string D1 { get; set; }

	public string D2 { get; set; }

	public string D3 { get; set; }

	public string D4 { get; set; }

	public string D5 { get; set; }

	public string D6 { get; set; }

	public string D7 { get; set; }

	public string D8 { get; set; }

	public string D9 { get; set; }

	public string D10 { get; set; }

	public string Y1 { get; set; }

	public string Y2 { get; set; }

	public string Y3 { get; set; }

	public string Y4 { get; set; }

	public string Y5 { get; set; }

	public string Y6 { get; set; }

	public string Y7 { get; set; }

	public string Y8 { get; set; }

	public string Y9 { get; set; }

	public string Y10 { get; set; }

	public string CH { get; set; }

	public cnType ConnectionType { get; set; }

	public string Request { get; private set; }

	public PCPOSResponceBase Response { get; } = new PCPOSResponceBase();


	public event ResponseEventHandler GetResponse;

	public PcPosBase()
	{
		Currency = "364";
		PrCode = "000000";
	}

	public byte[] BuildMessage()
	{
		try
		{
			string text = "";
			foreach (KeyValuePair<string, string> message_Tag in Message_Tags)
			{
				if (!string.IsNullOrEmpty(message_Tag.Value))
				{
					text = text + message_Tag.Key.PadLeft(2, ' ') + message_Tag.Value.Length.ToString().PadLeft(3, '0') + message_Tag.Value;
				}
			}
			text = (Request = "RQ".PadLeft(2, ' ') + text.Length.ToString().PadLeft(3, '0') + text);
			return Encoding.GetEncoding(1256).GetBytes(text.Length.ToString().PadLeft(4, '0') + text);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BuildMessage", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PcPosBase.cs", 40);
			throw;
		}
	}

	private byte[] BuildMessageWithExtra()
	{
		checked
		{
			try
			{
				string text = "";
				foreach (KeyValuePair<string, string> message_Tag in Message_Tags)
				{
					if (!string.IsNullOrEmpty(message_Tag.Value))
					{
						text = text + message_Tag.Key.PadLeft(2, ' ') + message_Tag.Value.Length.ToString().PadLeft(3, '0') + message_Tag.Value;
					}
				}
				if (!string.IsNullOrEmpty(Settel))
				{
					Settel = Settel.Replace("\r\n", "\n");
					char[] separator = new char[1] { '\n' };
					string[] array = Settel.Split(separator);
					for (int i = 0; i < array.Length; i++)
					{
						string text2 = "";
						char[] separator2 = new char[1] { '=' };
						string[] array2 = array[i].Split(separator2);
						if (array2.Length == 2)
						{
							text2 = text2 + "AC".PadLeft(2, ' ') + array2[0].Length.ToString().PadLeft(3, '0') + array2[0] + "AM".PadLeft(2, ' ') + array2[1].Length.ToString().PadLeft(3, '0') + array2[1];
							text = text + "ST".PadLeft(2, ' ') + text2.Length.ToString().PadLeft(3, '0') + text2;
						}
					}
				}
				if (!string.IsNullOrEmpty(KeyValue))
				{
					KeyValue = KeyValue.Replace("\r\n", "\n");
					char[] separator3 = new char[1] { '\n' };
					string[] array = KeyValue.Split(separator3);
					for (int i = 0; i < array.Length; i++)
					{
						string text3 = "";
						char[] separator4 = new char[1] { '=' };
						string[] array3 = array[i].Split(separator4);
						if (array3.Length == 2)
						{
							text3 = text3 + "KY".PadLeft(2, ' ') + array3[0].Length.ToString().PadLeft(3, '0') + array3[0] + "VL".PadLeft(2, ' ') + array3[1].Length.ToString().PadLeft(3, '0') + array3[1];
							text = text + "AV".PadLeft(2, ' ') + text3.Length.ToString().PadLeft(3, '0') + text3 + "PV".PadLeft(2, ' ') + text3.Length.ToString().PadLeft(3, '0') + text3;
						}
					}
				}
				text = (Request = "RQ".PadLeft(2, ' ') + text.Length.ToString().PadLeft(3, '0') + text);
				return Encoding.GetEncoding(1256).GetBytes(text.Length.ToString().PadLeft(4, '0') + text);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "BuildMessageWithExtra", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PcPosBase.cs", 94);
				throw;
			}
		}
	}

	private void FillMessageAdviceTransaction()
	{
		try
		{
			Message_Tags.Clear();
			Message_Tags.Add("GA", "1");
			Message_Tags.Add("PD", "1");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FillMessageAdviceTransaction", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PcPosBase.cs", 104);
			throw;
		}
	}

	private void FillMessageCancelTransaction()
	{
		try
		{
			Message_Tags.Clear();
			Message_Tags.Add("TC", "1");
			Message_Tags.Add("PD", "1");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FillMessageCancelTransaction", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PcPosBase.cs", 114);
			throw;
		}
	}

	private void FillMessageFoodSafety()
	{
		try
		{
			Message_Tags.Clear();
			Message_Tags.Add("PR", PrCode);
			Message_Tags.Add("AM", Amount);
			Message_Tags.Add("CU", Currency);
			Message_Tags.Add("SD", SignCode);
			Message_Tags.Add("R1", R1Holder);
			Message_Tags.Add("R2", R2Merchant);
			Message_Tags.Add("R3", R3Holder);
			Message_Tags.Add("R4", R4Merchant);
			Message_Tags.Add("R5", R5Holder);
			Message_Tags.Add("R6", R6Merchant);
			Message_Tags.Add("R7", R7Holder);
			Message_Tags.Add("R8", R8Merchant);
			Message_Tags.Add("R9", R9Holder);
			Message_Tags.Add("R0", R0Merchant);
			Message_Tags.Add("T1", T1Holder);
			Message_Tags.Add("T2", T2Merchant);
			Message_Tags.Add("SV", Service);
			Message_Tags.Add("SG", ServiceGroup);
			Message_Tags.Add("AD", "");
			Message_Tags.Add("FS", FoodSafety);
			Message_Tags.Add("PD", "1");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FillMessageFoodSafety", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PcPosBase.cs", 143);
			throw;
		}
	}

	private void FillMessageGetLastTrxnParams()
	{
		try
		{
			Message_Tags.Clear();
			Message_Tags.Add("LT", "GetLastTrxnInfo");
			Message_Tags.Add("PD", "1");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FillMessageGetLastTrxnParams", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PcPosBase.cs", 153);
			throw;
		}
	}

	private void FillMessageGetTerminalNumber()
	{
		try
		{
			Message_Tags.Clear();
			Message_Tags.Add("GT", "1");
			Message_Tags.Add("PD", "1");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FillMessageGetTerminalNumber", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PcPosBase.cs", 163);
			throw;
		}
	}

	private void FillMessageParams()
	{
		try
		{
			Message_Tags.Clear();
			Message_Tags.Add("PR", PrCode);
			Message_Tags.Add("AM", Amount);
			Message_Tags.Add("CU", Currency);
			Message_Tags.Add("TL", TerminalID);
			Message_Tags.Add("SD", SignCode);
			Message_Tags.Add("R1", R1Holder);
			Message_Tags.Add("R2", R2Merchant);
			Message_Tags.Add("R3", R3Holder);
			Message_Tags.Add("R4", R4Merchant);
			Message_Tags.Add("R5", R5Holder);
			Message_Tags.Add("R6", R6Merchant);
			Message_Tags.Add("R7", R7Holder);
			Message_Tags.Add("R8", R8Merchant);
			Message_Tags.Add("R9", R9Holder);
			Message_Tags.Add("R0", R0Merchant);
			Message_Tags.Add("T1", T1Holder);
			Message_Tags.Add("T2", T2Merchant);
			Message_Tags.Add("SV", Service);
			Message_Tags.Add("SG", ServiceGroup);
			Message_Tags.Add("AD", "");
			Message_Tags.Add("A1", Amount1);
			Message_Tags.Add("I1", ID1);
			Message_Tags.Add("D1", D1);
			Message_Tags.Add("Y1", Y1);
			Message_Tags.Add("A2", Amount2);
			Message_Tags.Add("I2", ID2);
			Message_Tags.Add("D2", D2);
			Message_Tags.Add("Y2", Y2);
			Message_Tags.Add("A3", Amount3);
			Message_Tags.Add("I3", ID3);
			Message_Tags.Add("D3", D3);
			Message_Tags.Add("A4", Amount4);
			Message_Tags.Add("I4", ID4);
			Message_Tags.Add("D4", D4);
			Message_Tags.Add("A5", Amount5);
			Message_Tags.Add("I5", ID5);
			Message_Tags.Add("D5", D5);
			Message_Tags.Add("A6", Amount6);
			Message_Tags.Add("I6", ID6);
			Message_Tags.Add("D6", D6);
			Message_Tags.Add("A7", Amount7);
			Message_Tags.Add("I7", ID7);
			Message_Tags.Add("D7", D7);
			Message_Tags.Add("A8", Amount8);
			Message_Tags.Add("I8", ID8);
			Message_Tags.Add("D8", D8);
			Message_Tags.Add("A9", Amount9);
			Message_Tags.Add("I9", ID9);
			Message_Tags.Add("D9", D9);
			Message_Tags.Add("A0", Amount10);
			Message_Tags.Add("I0", ID10);
			Message_Tags.Add("D0", D10);
			Message_Tags.Add("PD", "1");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FillMessageParams", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PcPosBase.cs", 224);
			throw;
		}
	}

	private void FillMessageParamsBillPayment()
	{
		try
		{
			Message_Tags.Clear();
			Message_Tags.Add("PR", PrCode);
			Message_Tags.Add("CU", Currency);
			Message_Tags.Add("SD", SignCode);
			Message_Tags.Add("R1", R1Holder);
			Message_Tags.Add("R2", R2Merchant);
			Message_Tags.Add("R3", R3Holder);
			Message_Tags.Add("R4", R4Merchant);
			Message_Tags.Add("R5", R5Holder);
			Message_Tags.Add("R6", R6Merchant);
			Message_Tags.Add("R7", R7Holder);
			Message_Tags.Add("R8", R8Merchant);
			Message_Tags.Add("R9", R9Holder);
			Message_Tags.Add("R0", R0Merchant);
			Message_Tags.Add("T1", T1Holder);
			Message_Tags.Add("T2", T2Merchant);
			Message_Tags.Add("BI", BIllID);
			Message_Tags.Add("PI", PaymentID);
			Message_Tags.Add("PD", "1");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FillMessageParamsBillPayment", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PcPosBase.cs", 250);
			throw;
		}
	}

	private void FillMessageParamsCharge()
	{
		try
		{
			Message_Tags.Clear();
			Message_Tags.Add("PR", PrCode);
			Message_Tags.Add("CU", Currency);
			Message_Tags.Add("SD", SignCode);
			Message_Tags.Add("R1", R1Holder);
			Message_Tags.Add("R2", R2Merchant);
			Message_Tags.Add("R3", R3Holder);
			Message_Tags.Add("R4", R4Merchant);
			Message_Tags.Add("R5", R5Holder);
			Message_Tags.Add("R6", R6Merchant);
			Message_Tags.Add("R7", R7Holder);
			Message_Tags.Add("R8", R8Merchant);
			Message_Tags.Add("R9", R9Holder);
			Message_Tags.Add("R0", R0Merchant);
			Message_Tags.Add("T1", T1Holder);
			Message_Tags.Add("T2", T2Merchant);
			Message_Tags.Add("CH", CH);
			Message_Tags.Add("PD", "1");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FillMessageParamsCharge", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PcPosBase.cs", 275);
			throw;
		}
	}

	private void FillMessageReversTransaction()
	{
		try
		{
			Message_Tags.Clear();
			Message_Tags.Add("GR", "1");
			Message_Tags.Add("PD", "1");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FillMessageReversTransaction", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PcPosBase.cs", 285);
			throw;
		}
	}

	private void FillMessageShiftCloseParams()
	{
		try
		{
			Message_Tags.Clear();
			Message_Tags.Add("SC", "1");
			Message_Tags.Add("PD", "1");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FillMessageShiftCloseParams", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PcPosBase.cs", 295);
			throw;
		}
	}

	private void FillMessageShiftOpenParams()
	{
		try
		{
			Message_Tags.Clear();
			Message_Tags.Add("SO", "1");
			Message_Tags.Add("PD", "1");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FillMessageShiftOpenParams", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PcPosBase.cs", 305);
			throw;
		}
	}

	private void FillMessageWareHoff()
	{
		try
		{
			Message_Tags.Clear();
			Message_Tags.Add("PR", PrCode);
			Message_Tags.Add("CU", Currency);
			Message_Tags.Add("SD", SignCode);
			Message_Tags.Add("R1", R1Holder);
			Message_Tags.Add("R2", R2Merchant);
			Message_Tags.Add("R3", R3Holder);
			Message_Tags.Add("R4", R4Merchant);
			Message_Tags.Add("R5", R5Holder);
			Message_Tags.Add("R6", R6Merchant);
			Message_Tags.Add("R7", R7Holder);
			Message_Tags.Add("R8", R8Merchant);
			Message_Tags.Add("R9", R9Holder);
			Message_Tags.Add("R0", R0Merchant);
			Message_Tags.Add("T1", T1Holder);
			Message_Tags.Add("T2", T2Merchant);
			Message_Tags.Add("AM", Amount);
			Message_Tags.Add("BI", BIllID);
			Message_Tags.Add("PI", PaymentID);
			Message_Tags.Add("PD", "1");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FillMessageWareHoff", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PcPosBase.cs", 332);
			throw;
		}
	}

	public void GetTerminalNumber()
	{
		try
		{
			FillMessageGetTerminalNumber();
			finaMsg = BuildMessage();
			if (ConnectionType == cnType.LAN)
			{
				SendToLan();
			}
			else
			{
				SendToSerialPort();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetTerminalNumber", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PcPosBase.cs", 343);
			throw;
		}
	}

	private void ReadLANResponse(NetworkStream nwStream)
	{
		checked
		{
			try
			{
				byte[] array = new byte[500];
				string text = "";
				do
				{
					int num = nwStream.Read(array, 0, array.Length);
					if (num == 0)
					{
						break;
					}
					text += Encoding.ASCII.GetString(array, 0, num);
				}
				while (text.Length < 4 || text.Length != int.Parse(text.Substring(0, 4)) + 4);
				text = text.Substring(0, int.Parse(text.Substring(0, 4)) + 4);
				Console.WriteLine("********* ReadLANResponse: " + text);
				Response.RawResponse = text;
				ReturnResponse(Response.RawResponse);
				nwStream.Close();
				client.Close();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ReadLANResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PcPosBase.cs", 373);
				Response.RawResponse = "0018RS013RS00281PD0011";
				ReturnResponse(Response.RawResponse);
				client?.Close();
			}
		}
	}

	public void ReturnResponse(string request)
	{
		try
		{
			if (this.GetResponse != null)
			{
				this.GetResponse(request);
				return;
			}
			Response.RawResponse = "0018RS013RS00281PD0011";
			ReturnResponse(Response.RawResponse);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ReturnResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PcPosBase.cs", 390);
			throw;
		}
	}

	public void Send_transaction()
	{
		try
		{
			FillMessageParams();
			finaMsg = BuildMessageWithExtra();
			if (ConnectionType == cnType.LAN)
			{
				SendToLan();
			}
			else
			{
				SendToSerialPort();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Send_transaction", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PcPosBase.cs", 401);
			throw;
		}
	}

	public void Send_transaction_bill_payment()
	{
		try
		{
			FillMessageParamsBillPayment();
			finaMsg = BuildMessage();
			if (ConnectionType == cnType.LAN)
			{
				SendToLan();
			}
			else
			{
				SendToSerialPort();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Send_transaction_bill_payment", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PcPosBase.cs", 412);
			throw;
		}
	}

	public void Send_transaction_charge()
	{
		try
		{
			FillMessageParamsCharge();
			finaMsg = BuildMessage();
			if (ConnectionType == cnType.LAN)
			{
				SendToLan();
			}
			else
			{
				SendToSerialPort();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Send_transaction_charge", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PcPosBase.cs", 423);
			throw;
		}
	}

	public void Send_transaction_food_safety()
	{
		try
		{
			FillMessageFoodSafety();
			finaMsg = BuildMessage();
			if (ConnectionType == cnType.LAN)
			{
				SendToLan();
			}
			else
			{
				SendToSerialPort();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Send_transaction_food_safety", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PcPosBase.cs", 434);
			throw;
		}
	}

	public void Send_transaction_Get_Lats_Trxn()
	{
		try
		{
			FillMessageGetLastTrxnParams();
			finaMsg = BuildMessage();
			if (ConnectionType == cnType.LAN)
			{
				SendToLan();
			}
			else
			{
				SendToSerialPort();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Send_transaction_Get_Lats_Trxn", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PcPosBase.cs", 445);
			throw;
		}
	}

	public void Send_transaction_Shift_Close()
	{
		try
		{
			FillMessageShiftCloseParams();
			finaMsg = BuildMessage();
			if (ConnectionType == cnType.LAN)
			{
				SendToLan();
			}
			else
			{
				SendToSerialPort();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Send_transaction_Shift_Close", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PcPosBase.cs", 456);
			throw;
		}
	}

	public void Send_transaction_Shift_Open()
	{
		try
		{
			FillMessageShiftOpenParams();
			finaMsg = BuildMessage();
			if (ConnectionType == cnType.LAN)
			{
				SendToLan();
			}
			else
			{
				SendToSerialPort();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Send_transaction_Shift_Open", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PcPosBase.cs", 467);
			throw;
		}
	}

	public void Send_transaction_Trx_Advice()
	{
		try
		{
			FillMessageAdviceTransaction();
			finaMsg = BuildMessage();
			if (ConnectionType == cnType.LAN)
			{
				SendToLan();
			}
			else
			{
				SendToSerialPort();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Send_transaction_Trx_Advice", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PcPosBase.cs", 478);
			throw;
		}
	}

	public void Send_transaction_Trx_Cancel()
	{
		try
		{
			FillMessageCancelTransaction();
			finaMsg = BuildMessage();
			if (ConnectionType == cnType.LAN)
			{
				SendToLan();
			}
			else
			{
				SendToSerialPort();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Send_transaction_Trx_Cancel", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PcPosBase.cs", 489);
			throw;
		}
	}

	public void Send_transaction_Trx_Revers()
	{
		try
		{
			FillMessageReversTransaction();
			finaMsg = BuildMessage();
			if (ConnectionType == cnType.LAN)
			{
				SendToLan();
			}
			else
			{
				SendToSerialPort();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Send_transaction_Trx_Revers", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PcPosBase.cs", 500);
			throw;
		}
	}

	public void Send_transaction_WareHoff()
	{
		try
		{
			FillMessageWareHoff();
			finaMsg = BuildMessage();
			if (ConnectionType == cnType.LAN)
			{
				SendToLan();
			}
			else
			{
				SendToSerialPort();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Send_transaction_WareHoff", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PcPosBase.cs", 511);
			throw;
		}
	}

	public void SendToLan()
	{
		try
		{
			Thread thread = tReader;
			if (thread != null && thread.IsAlive)
			{
				tReader.Abort();
			}
			client?.Close();
			client = new TcpClient(Ip, Port);
			NetworkStream nwStream = client.GetStream();
			nwStream.Write(finaMsg, 0, finaMsg.Length);
			tReader = new Thread((ThreadStart)delegate
			{
				ReadLANResponse(nwStream);
			})
			{
				IsBackground = true
			};
			tReader.Start();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendToLan", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PcPosBase.cs", 527);
			Response.RawResponse = "0018RS013RS00281PD0011";
			ReturnResponse(Response.RawResponse);
			client?.Close();
		}
	}

	public abstract void SendToSerialPort();

	public void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
	{
		try
		{
			string text = "";
			int num = 0;
			while (text.Length < 4)
			{
				text += serialPort.ReadExisting();
			}
			num = int.Parse(text.Substring(0, 4));
			while (text.Length < num)
			{
				text += serialPort.ReadExisting();
			}
			Response.RawResponse = text;
			ReturnResponse(Response.RawResponse);
			SerialPort obj = serialPort;
			if (obj != null)
			{
				obj.Close();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DataReceivedHandler", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\PcPosBase.cs", 563);
			Response.RawResponse = "0018RS013RS00281PD0011";
			ReturnResponse(Response.RawResponse);
			SerialPort obj2 = serialPort;
			if (obj2 != null)
			{
				obj2.Close();
			}
		}
	}
}
