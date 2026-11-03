using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Connection.LowLevel.Payamak;
using PacketParser.Entities;
using PacketParser.Payamak.Entities;

namespace PacketParser.Payamak.Setting;

public class CheckItem
{
	private int _accountId;

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


	public string Mobiles { get; }

	public string MessageCalced
	{
		get
		{
			try
			{
				string text = Message.FixString();
				if (text.Contains(CheckM.FullName))
				{
					text = text.Replace(CheckM.FullName, FullName);
				}
				if (text.Contains(CheckM.CheckNO))
				{
					text = text.Replace(CheckM.CheckNO, CheckSerial);
				}
				if (text.Contains(CheckM.CheckDate))
				{
					text = text.Replace(CheckM.CheckDate, DateSh);
				}
				if (text.Contains(CheckM.CheckPrice))
				{
					text = text.Replace(CheckM.CheckPrice, Price.NpToString("N0", 0));
				}
				return text;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MessageCalced", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Payamak\\Entities\\Setting\\CheckItem.cs", 40);
			}
			return "";
		}
	}

	public CheckItem(int accountId, string checkSerial, DateTime dateSarresiM, DateTime dateSend, decimal price, string customerFullName, string mobiles)
	{
		try
		{
			_accountId = accountId;
			Message = Message;
			CheckSerial = checkSerial;
			Price = price;
			DateM = dateSarresiM;
			DateSendM = dateSend;
			FullName = customerFullName;
			Mobiles = mobiles;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Payamak\\Entities\\Setting\\CheckItem.cs", 58);
		}
	}

	public CheckItem(int accountId, string checkSerial, DateTime dateSarresiM, decimal price, string customerFullName, string mobiles)
	{
		try
		{
			_accountId = accountId;
			Message = Message;
			CheckSerial = checkSerial;
			Price = price;
			DateM = dateSarresiM;
			FullName = customerFullName;
			Mobiles = mobiles;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Payamak\\Entities\\Setting\\CheckItem.cs", 72);
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
				Number = Mobiles,
				Priority = 1,
				SchadulingM = DateSendM,
				AccountId = _accountId
			};
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await service.SaveAsync(item, token), ReturnedState.None, "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Payamak\\Entities\\Setting\\CheckItem.cs", 88);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Payamak\\Entities\\Setting\\CheckItem.cs", 92);
			res.AddReturnedValue(ex, "", "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Payamak\\Entities\\Setting\\CheckItem.cs", 93);
		}
		return res;
	}
}
