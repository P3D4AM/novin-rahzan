using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO.Ports;
using System.Reflection;
using PacketParser.Entities;
using PacketParser.PosToPc.Sadad.Classes.EventArguments;
using PacketParser.PosToPc.Sadad.Interface;

namespace PacketParser.PosToPc.Sadad.Classes;

public class PcPosSadad
{
	internal static readonly string URL;

	private static Dictionary<string, IModem> modems;

	private static Dictionary<string, bool> progresses;

	private static MagicBusiness magic;

	private Stack<object> activity = new Stack<object>();

	private IModem modem;

	private EnSadadPcPosConnectionType connectionType;

	private int port;

	private int[] responseTimeOut;

	private string ip;

	private string comportName;

	private string amount;

	private string serialNo;

	private string Opt2;

	private string Opt4;

	private string Opt5;

	private string Opt8;

	private TimeSpan minimumLanTimeout = TimeSpan.FromSeconds(180.0);

	private TimeSpan minimumSerialTimeout = TimeSpan.FromSeconds(180.0);

	public virtual string Amount
	{
		get
		{
			return amount.TrimEnd(new char[1]);
		}
		set
		{
			amount = value;
		}
	}

	public virtual string ComPortName
	{
		get
		{
			return comportName;
		}
		set
		{
			try
			{
				Name = value;
				if (modem != null && modem is SerialModem && !((modem as SerialModem).Connection.PortName != value))
				{
					comportName = value;
					return;
				}
				comportName = value;
				if (responseTimeOut == null || (double)responseTimeOut[0] < minimumSerialTimeout.TotalMilliseconds)
				{
					responseTimeOut = new int[1] { checked((int)minimumSerialTimeout.TotalMilliseconds) };
				}
				responseTimeOut[0] = Math.Max(responseTimeOut[0], 0);
				SerialModem value2 = new SerialModem(EnSadadDeviceType.BlueBird, value, responseTimeOut[0], 0);
				Init(value, 0);
				if (modems.ContainsKey(value))
				{
					modems.Remove(value);
				}
				modems.Add(value, value2);
				ChangeProgressState(value: false);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ComPortName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\Sadad\\Classes\\PcPosSadad.cs", 57);
			}
		}
	}

	public virtual EnSadadPcPosConnectionType ConnectionType
	{
		get
		{
			return connectionType;
		}
		set
		{
			connectionType = value;
		}
	}

	public virtual EnSadadDeviceType DeviceType { get; set; }

	private string Field2
	{
		get
		{
			return Opt2;
		}
		set
		{
			Opt2 = ((!string.IsNullOrEmpty(value)) ? value : string.Empty);
		}
	}

	private string Field4
	{
		get
		{
			return Opt4;
		}
		set
		{
			Opt4 = value;
		}
	}

	private string Field5
	{
		get
		{
			return Opt5;
		}
		set
		{
			Opt5 = value;
		}
	}

	private string Field8
	{
		get
		{
			return Opt8;
		}
		set
		{
			Opt8 = value;
		}
	}

	public virtual string Ip
	{
		get
		{
			return ip;
		}
		set
		{
			try
			{
				Name = value;
				if (Modem != null && modem is NetworkModem)
				{
					ip = value;
					return;
				}
				ip = value;
				if (responseTimeOut == null || (double)responseTimeOut[0] < minimumLanTimeout.TotalMilliseconds)
				{
					responseTimeOut = new int[1] { checked((int)minimumLanTimeout.TotalMilliseconds) };
				}
				responseTimeOut[0] = Math.Max(responseTimeOut[0], 0);
				NetworkModem value2 = new NetworkModem(EnSadadDeviceType.BlueBird, value, 8888, responseTimeOut[0]);
				if (modems.ContainsKey(value))
				{
					modems.Remove(value);
				}
				modems.Add(value, value2);
				ChangeProgressState(value: false);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Ip", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\Sadad\\Classes\\PcPosSadad.cs", 90);
			}
		}
	}

	private bool IsInProgress
	{
		get
		{
			progresses.TryGetValue(Name, out var value);
			return value;
		}
	}

	public virtual MagicData MagicData { get; set; }

	private IModem Modem
	{
		get
		{
			try
			{
				if (string.IsNullOrEmpty(Name))
				{
					return null;
				}
				if (!modems.TryGetValue(Name, out modem))
				{
					return null;
				}
				if (modem.MediaType == EnSadadPcPosConnectionType.Serial)
				{
					if ((modem as SerialModem).Connection != null)
					{
						return modem;
					}
					modems.Remove(Name);
					modem = null;
					if (!string.IsNullOrEmpty(comportName))
					{
						ComPortName = comportName;
						modems.TryGetValue(Name, out modem);
						if (modem != null)
						{
							return modem;
						}
					}
					return null;
				}
				if ((modem as NetworkModem).Connection != null)
				{
					return modem;
				}
				modems.Remove(Name);
				modem = null;
				if (!string.IsNullOrEmpty(ip))
				{
					Ip = ip;
					modems.TryGetValue(Name, out modem);
					if (modem != null)
					{
						return modem;
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Modem", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\Sadad\\Classes\\PcPosSadad.cs", 133);
			}
			return null;
		}
	}

	private string Name { get; set; }

	public virtual int Port
	{
		get
		{
			return port;
		}
		set
		{
			port = value;
		}
	}

	public event EventHandler<PosResult> OnSaleResult;

	public PcPosSadad(EnSadadDeviceType deviceType)
	{
		try
		{
			DeviceType = deviceType;
			Field8 = Assembly.GetExecutingAssembly().GetName().Version.ToString();
			modems = new Dictionary<string, IModem>();
			progresses = new Dictionary<string, bool>();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\Sadad\\Classes\\PcPosSadad.cs", 149);
		}
	}

	private void ChangeProgressState(bool value)
	{
		try
		{
			if (!progresses.TryGetValue(Name, out var _))
			{
				progresses.Add(Name, value);
			}
			else
			{
				progresses[Name] = value;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ChangeProgressState", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\Sadad\\Classes\\PcPosSadad.cs", 159);
		}
	}

	private PosResult ConvertMagicResponse(int magicResponse, PCPOSEventArg magicResult)
	{
		PosResult result = new PosResult();
		try
		{
			PosResult posResult = new PosResult();
			if (magicResponse == 0)
			{
				if (magicResult.PosRes == "00" || magicResult.PosRes == "0")
				{
					posResult.PcPosStatusCode = 4;
					if (magicResult.SwitchRes == "00" || magicResult.SwitchRes == "0")
					{
						posResult.ResponseCode = "00";
					}
					else
					{
						string responseCode = "";
						switch (magicResult.SwitchRes)
						{
						case "02":
							responseCode = "475";
							break;
						case "03":
							responseCode = "476";
							break;
						case "06":
							responseCode = "477";
							break;
						case "12":
							responseCode = "478";
							break;
						case "13":
							responseCode = "479";
							break;
						case "14":
							responseCode = "480";
							break;
						case "15":
							responseCode = "481";
							break;
						case "16":
							responseCode = "482";
							break;
						case "19":
							responseCode = "483";
							break;
						case "25":
							responseCode = "484";
							break;
						case "30":
							responseCode = "485";
							break;
						case "33":
							responseCode = "486";
							break;
						case "34":
							responseCode = "487";
							break;
						case "41":
							responseCode = "488";
							break;
						case "43":
							responseCode = "489";
							break;
						case "51":
							responseCode = "490";
							break;
						case "55":
							responseCode = "491";
							break;
						case "56":
							responseCode = "492";
							break;
						case "57":
							responseCode = "493";
							break;
						case "61":
							responseCode = "494";
							break;
						case "62":
							responseCode = "495";
							break;
						case "65":
							responseCode = "496";
							break;
						case "66":
							responseCode = "497";
							break;
						case "75":
							responseCode = "498";
							break;
						case "77":
							responseCode = "499";
							break;
						case "78":
							responseCode = "500";
							break;
						case "79":
							responseCode = "501";
							break;
						case "80":
							responseCode = "502";
							break;
						case "81":
							responseCode = "503";
							break;
						case "83":
							responseCode = "504";
							break;
						case "84":
							responseCode = "505";
							break;
						case "85":
							responseCode = "506";
							break;
						case "86":
							responseCode = "507";
							break;
						case "87":
							responseCode = "508";
							break;
						case "88":
							responseCode = "509";
							break;
						case "89":
							responseCode = "510";
							break;
						case "90":
							responseCode = "511";
							break;
						case "91":
							responseCode = "512";
							break;
						case "92":
							responseCode = "513";
							break;
						case "93":
							responseCode = "514";
							break;
						case "94":
							responseCode = "515";
							break;
						case "96":
							responseCode = "516";
							break;
						}
						posResult.ResponseCode = responseCode;
					}
				}
				else
				{
					string responseCode2 = "";
					switch (magicResult.PosRes)
					{
					case "01":
						responseCode2 = "461";
						break;
					case "02":
						responseCode2 = "462";
						break;
					case "03":
						responseCode2 = "463";
						break;
					case "04":
						responseCode2 = "464";
						break;
					case "05":
						responseCode2 = "465";
						break;
					case "06":
						responseCode2 = "466";
						break;
					case "07":
						responseCode2 = "467";
						break;
					case "08":
						responseCode2 = "468";
						break;
					case "09":
						responseCode2 = "469";
						break;
					case "10":
						responseCode2 = "470";
						break;
					case "11":
						responseCode2 = "471";
						break;
					case "98":
						responseCode2 = "472";
						break;
					case "99":
						responseCode2 = "473";
						break;
					}
					posResult.PcPosStatusCode = 7;
					posResult.ResponseCode = responseCode2;
				}
			}
			else if (magicResponse >= 0)
			{
				posResult.PcPosStatusCode = 4;
				string responseCode3 = "";
				switch (magicResult.PosRes)
				{
				case "01":
					responseCode3 = "461";
					break;
				case "02":
					responseCode3 = "462";
					break;
				case "03":
					responseCode3 = "463";
					break;
				case "04":
					responseCode3 = "464";
					break;
				case "05":
					responseCode3 = "465";
					break;
				case "06":
					responseCode3 = "466";
					posResult.PcPosStatusCode = 3;
					break;
				case "07":
					responseCode3 = "467";
					break;
				case "08":
					responseCode3 = "468";
					break;
				case "09":
					responseCode3 = "469";
					break;
				case "10":
					responseCode3 = "470";
					break;
				case "11":
					responseCode3 = "471";
					break;
				case "98":
					responseCode3 = "472";
					break;
				case "99":
					responseCode3 = "473";
					break;
				}
				posResult.ResponseCode = responseCode3;
			}
			else if (magicResponse <= -80 && magicResponse >= -86)
			{
				posResult.PcPosStatusCode = 5;
				switch (magicResponse)
				{
				case -86:
					posResult.ResponseCode = "530";
					break;
				case -85:
					posResult.ResponseCode = "529";
					break;
				case -84:
					posResult.ResponseCode = "528";
					break;
				case -83:
					posResult.ResponseCode = "527";
					break;
				case -82:
					posResult.ResponseCode = "526";
					break;
				case -81:
					posResult.ResponseCode = "525";
					break;
				case -80:
					posResult.ResponseCode = "524";
					break;
				}
			}
			else
			{
				switch (magicResponse)
				{
				case -80:
				case -10:
					posResult.PcPosStatusCode = 5;
					switch (magicResponse)
					{
					case -80:
						posResult.ResponseCode = "524";
						break;
					case -10:
						posResult.ResponseCode = "523";
						break;
					}
					break;
				case -8:
				case -7:
				case -6:
				case -5:
				case -4:
				case -3:
					posResult.PcPosStatusCode = 5;
					switch (magicResponse)
					{
					case -8:
						posResult.ResponseCode = "522";
						break;
					case -7:
						posResult.ResponseCode = "521";
						break;
					case -6:
						posResult.ResponseCode = "520";
						break;
					case -5:
						posResult.ResponseCode = "519";
						break;
					case -4:
						posResult.ResponseCode = "518";
						break;
					case -3:
						posResult.ResponseCode = "517";
						break;
					}
					break;
				default:
				{
					string responseCode4 = "";
					switch (magicResult.PosRes)
					{
					case "01":
						responseCode4 = "461";
						break;
					case "02":
						responseCode4 = "462";
						break;
					case "03":
						responseCode4 = "463";
						break;
					case "04":
						responseCode4 = "464";
						break;
					case "05":
						responseCode4 = "465";
						break;
					case "06":
						responseCode4 = "466";
						break;
					case "07":
						responseCode4 = "467";
						break;
					case "08":
						responseCode4 = "468";
						break;
					case "09":
						responseCode4 = "469";
						break;
					case "10":
						responseCode4 = "470";
						break;
					case "11":
						responseCode4 = "471";
						break;
					case "98":
						responseCode4 = "472";
						break;
					case "99":
						responseCode4 = "473";
						break;
					}
					posResult.PcPosStatusCode = 7;
					posResult.ResponseCode = responseCode4;
					break;
				}
				}
			}
			posResult.Amount = magicResult.Amount;
			posResult.CardNo = magicResult.CardNo;
			posResult.MerchantId = magicResult.MerchantNo;
			posResult.Rrn = magicResult.RefNo;
			posResult.TerminalId = magicResult.TerminalNo;
			posResult.OrderId = magicResult.RequestId;
			if (magicResult.SwitchDate.Substring(0, 4) != "0000")
			{
				if (!magicResult.SwitchDate.Contains("/"))
				{
					posResult.TransactionDate = $"{magicResult.SwitchDate.Substring(0, 4)}/{magicResult.SwitchDate.Substring(4, 2)}/{magicResult.SwitchDate.Substring(6, 2)}";
				}
				else
				{
					posResult.TransactionDate = magicResult.SwitchDate;
				}
				if (!magicResult.SwitchTime.Contains(":"))
				{
					posResult.TransactionTime = $"{magicResult.SwitchTime.Substring(0, 2)}:{magicResult.SwitchTime.Substring(2, 2)}:{magicResult.SwitchTime.Substring(2, 2)}";
				}
				else
				{
					posResult.TransactionTime = magicResult.SwitchTime;
				}
			}
			else
			{
				posResult.TransactionDate = Utility.GetPersianPcDate();
				posResult.TransactionTime = Utility.GetPersianPcTime();
			}
			posResult.TransactionNo = magicResult.TraceNo;
			result = posResult;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ConvertMagicResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\Sadad\\Classes\\PcPosSadad.cs", 356);
		}
		return result;
	}

	private void HandleSerialPort()
	{
		try
		{
			if (Modem is SerialPort)
			{
				modems.TryGetValue(Name, out modem);
				((Component)(object)(modem as SerialModem).Connection)?.Dispose();
				modems.Remove(Name);
				modem = null;
				ComPortName = (comportName = "");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "HandleSerialPort", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\Sadad\\Classes\\PcPosSadad.cs", 377);
		}
	}

	public void Init(string portName, int timeout)
	{
		try
		{
			Name = portName;
			if (Modem == null)
			{
				SerialModem value = new SerialModem(EnSadadDeviceType.BlueBird, portName, timeout, 0);
				modems.Add(portName, value);
				ChangeProgressState(value: false);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Init", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\Sadad\\Classes\\PcPosSadad.cs", 391);
		}
	}

	private PosResult MagicSaleTransaction()
	{
		try
		{
			PCPOSEventArg pSendTransResult = new PCPOSEventArg();
			PersianCalendar persianCalendar = new PersianCalendar();
			string text = persianCalendar.GetYear(DateTime.Now).ToString().PadLeft(4, '0');
			string text2 = persianCalendar.GetMonth(DateTime.Now).ToString().PadLeft(2, '0');
			string pTransDate = text + text2 + persianCalendar.GetDayOfMonth(DateTime.Now).ToString().PadLeft(2, '0');
			string text3 = DateTime.Now.Hour.ToString().PadLeft(2, '0');
			string text4 = DateTime.Now.Minute.ToString().PadLeft(2, '0');
			string pTransTime = text3 + text4 + DateTime.Now.Second.ToString().PadLeft(2, '0');
			int pTimeout = 150000;
			int magicResponse = magic.SendTransSynch(Field5, ComPortName, MagicData.TerminalId, MagicData.MerchantId, MagicData.BillNo, MagicData.Amount, pTransDate, pTransTime, pTimeout, ref pSendTransResult);
			PosResult posResult = ConvertMagicResponse(magicResponse, pSendTransResult);
			ChangeProgressState(value: false);
			if (posResult == null)
			{
				return posResult;
			}
			this.OnSaleResult?.BeginInvoke(this, posResult, null, null);
			return posResult;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MagicSaleTransaction", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\Sadad\\Classes\\PcPosSadad.cs", 427);
			throw;
		}
	}

	private PosResult SaleTransaction(decimal amount)
	{
		try
		{
			if (IsInProgress)
			{
				return new PosResult
				{
					PcPosStatusCode = 6,
					Amount = amount.ToString(),
					TransactionDate = Utility.GetPersianPcDate(),
					TransactionTime = Utility.GetPersianPcTime(),
					ApprovalCode = "",
					CardNo = "",
					MerchantId = "",
					SaleId = "",
					OrderId = ""
				};
			}
			if (DeviceType == EnSadadDeviceType.Magic)
			{
				return MagicSaleTransaction();
			}
			Transaction transaction = new Transaction(Modem, Field8);
			transaction.SetAmount(amount);
			activity.Push(transaction);
			ChangeProgressState(value: true);
			PosResult result = transaction.Start(serialNo, "");
			HandleSerialPort();
			if (activity.Count > 0)
			{
				activity.Pop();
			}
			ChangeProgressState(value: false);
			return result;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaleTransaction", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\Sadad\\Classes\\PcPosSadad.cs", 465);
			throw;
		}
	}

	public virtual PosResult SyncSaleTransaction()
	{
		try
		{
			_ = Field2;
			_ = Field5;
			_ = Field4;
			decimal num = default(decimal);
			num = ((DeviceType == EnSadadDeviceType.Magic) ? MagicData.Amount.ParseToDecimal() : Amount.ParseToDecimal());
			return SaleTransaction(num);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SyncSaleTransaction", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\PosToPc\\Sadad\\Classes\\PcPosSadad.cs", 483);
			throw;
		}
	}
}
