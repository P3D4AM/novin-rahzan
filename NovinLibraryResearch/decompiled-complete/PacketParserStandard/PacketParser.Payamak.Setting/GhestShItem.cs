using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Connection.LowLevel.Payamak;
using PacketParser.Entities;
using PacketParser.Payamak.Entities;

namespace PacketParser.Payamak.Setting;

public class GhestShItem
{
	private readonly int _accountId;

	private readonly string _reciverNumber;

	public string Message { get; set; } = "";


	public string GhestNo { get; } = "";


	public decimal Price { get; }

	public DateTime DateM { get; } = DateTime.Now;


	public string DateSh => DateM.MiladiToShamsi();

	public DateTime DateSendM { get; } = DateTime.Now;


	public string DateSendSh => DateSendM.MiladiToShamsi();

	public string DateShHorofi => Calendar.MiladitoShamsiFarsi(DateM);

	public string FullName { get; } = "";


	public string Mobiles { get; } = "";


	public string MessageCalced
	{
		get
		{
			try
			{
				string text = Message.FixString();
				if (text.Contains(GhestSH.Girande))
				{
					text = text.Replace(GhestSH.Girande, FullName);
				}
				if (text.Contains(GhestSH.GhestNO))
				{
					text = text.Replace(GhestSH.GhestNO, GhestNo);
				}
				if (text.Contains(GhestSH.GhestDate))
				{
					text = text.Replace(GhestSH.GhestDate, DateSh);
				}
				if (text.Contains(GhestSH.GhestPrice))
				{
					text = text.Replace(GhestSH.GhestPrice, Price.NpToString("N0", 0));
				}
				return text;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MessageCalced", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Payamak\\Entities\\Setting\\GhestSHItem.cs", 40);
			}
			return "";
		}
	}

	public GhestShItem(string reciverNumber, int accountId, string ghestNo, DateTime dateSarresiM, DateTime dateSend, decimal price, string customerFullName)
	{
		try
		{
			_reciverNumber = reciverNumber;
			_accountId = accountId;
			Message = Message;
			GhestNo = ghestNo;
			Price = price;
			DateM = dateSarresiM;
			DateSendM = dateSend;
			FullName = customerFullName;
			Mobiles = Mobiles;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Payamak\\Entities\\Setting\\GhestSHItem.cs", 60);
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
			returnedSaveFuncInfo.AddReturnedValue(await service.SaveAsync(item, token), ReturnedState.None, "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Payamak\\Entities\\Setting\\GhestSHItem.cs", 76);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Payamak\\Entities\\Setting\\GhestSHItem.cs", 80);
			res.AddReturnedValue(ex, "", "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Payamak\\Entities\\Setting\\GhestSHItem.cs", 81);
		}
		return res;
	}
}
