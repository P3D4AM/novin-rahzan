using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Connection.LowLevel.Payamak;
using PacketParser.Entities;
using PacketParser.Payamak.Entities;

namespace PacketParser.Payamak.Setting;

public class CheckItemSh
{
	private int _accountId;

	private string _reciverNumber;

	public string Message { get; set; } = "";


	public string CheckSerial { get; } = "";


	public string HesabNum { get; } = "";


	public decimal Price { get; }

	public DateTime DateM { get; } = DateTime.Now;


	public string DateSh => DateM.MiladiToShamsi();

	public DateTime DateSendM { get; } = DateTime.Now;


	public string DateSendSh => DateSendM.MiladiToShamsi();

	public string DateShHorofi => Calendar.MiladitoShamsiFarsi(DateM);

	public string FullName { get; } = "";


	public string Mobiles { get; } = "";


	public string BankName { get; }

	public string MessageCalced
	{
		get
		{
			try
			{
				string text = Message.FixString();
				if (text.Contains(CheckSH.FullName))
				{
					text = text.Replace(CheckSH.FullName, FullName);
				}
				if (text.Contains(CheckSH.CheckNO))
				{
					text = text.Replace(CheckSH.CheckNO, CheckSerial);
				}
				if (text.Contains(CheckSH.CheckDate))
				{
					text = text.Replace(CheckSH.CheckDate, DateSh);
				}
				if (text.Contains(CheckSH.CheckPrice))
				{
					text = text.Replace(CheckSH.CheckPrice, Price.NpToString("N0", 0));
				}
				if (text.Contains(CheckSH.BankName))
				{
					text = text.Replace(CheckSH.BankName, BankName);
				}
				return text;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MessageCalced", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Payamak\\Entities\\Setting\\CheckItemSh.cs", 43);
			}
			return "";
		}
	}

	public CheckItemSh(string reciverNumber, int accountId, string checkSerial, DateTime dateSarresiM, DateTime dateSend, decimal price, string customerFullName, string bankName)
	{
		try
		{
			_reciverNumber = reciverNumber;
			_accountId = accountId;
			Message = Message;
			CheckSerial = checkSerial;
			Price = price;
			DateM = dateSarresiM;
			DateSendM = dateSend;
			FullName = customerFullName;
			Mobiles = Mobiles;
			BankName = bankName;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Payamak\\Entities\\Setting\\CheckItemSh.cs", 64);
		}
	}

	public CheckItemSh(string reciverNumber, int accountId, string checkSerial, DateTime dateSarresiM, decimal price, string customerFullName, string bankName)
	{
		try
		{
			_reciverNumber = reciverNumber;
			_accountId = accountId;
			Message = Message;
			CheckSerial = checkSerial;
			Price = price;
			DateM = dateSarresiM;
			DateSendM = DateTime.Now;
			FullName = customerFullName;
			Mobiles = Mobiles;
			BankName = bankName;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Payamak\\Entities\\Setting\\CheckItemSh.cs", 83);
		}
	}

	public async Task<ReturnedSaveFuncInfo> SendAsync(ISmsOutBoxSignalRClient service, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			BaseSmsOutBox item = new BaseSmsOutBox
			{
				Message = MessageCalced,
				Number = _reciverNumber,
				Priority = 1,
				SchadulingM = DateSendM,
				AccountId = _accountId
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await service.SaveAsync(item, token), ReturnedState.None, "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Payamak\\Entities\\Setting\\CheckItemSh.cs", 99);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Payamak\\Entities\\Setting\\CheckItemSh.cs", 103);
			res.AddReturnedValue(ex, "", "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Payamak\\Entities\\Setting\\CheckItemSh.cs", 104);
		}
		return res;
	}
}
