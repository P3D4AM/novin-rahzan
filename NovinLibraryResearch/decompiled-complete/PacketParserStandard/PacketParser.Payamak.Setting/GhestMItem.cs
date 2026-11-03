using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Connection.LowLevel.Payamak;
using PacketParser.Entities;
using PacketParser.Payamak.Entities;

namespace PacketParser.Payamak.Setting;

public class GhestMItem
{
	private int _accountId;

	public string Message { get; set; } = "";


	public string GhestNo { get; } = "";


	public decimal Price { get; }

	public int Count { get; }

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
				if (text.Contains(GhestM.Pardazande))
				{
					text = text.Replace(GhestM.Pardazande, FullName);
				}
				if (text.Contains(GhestM.GhestNO))
				{
					text = text.Replace(GhestM.GhestNO, GhestNo);
				}
				if (text.Contains(GhestM.GhestDate))
				{
					text = text.Replace(GhestM.GhestDate, DateSh);
				}
				if (text.Contains(GhestM.GhestPrice))
				{
					text = text.Replace(GhestM.GhestPrice, Price.NpToString("N0", 0));
				}
				if (text.Contains(GhestM.Count))
				{
					text = text.Replace(GhestM.Count, Count.ToString("N0"));
				}
				return text;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MessageCalced", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Payamak\\Entities\\Setting\\GhestMItem.cs", 41);
			}
			return "";
		}
	}

	public GhestMItem(int accountId, string ghestNo, DateTime dateSarresiM, DateTime dateSend, decimal price, string customerFullName, string mobiles, int count)
	{
		try
		{
			_accountId = accountId;
			Message = Message;
			GhestNo = ghestNo;
			Price = price;
			DateM = dateSarresiM;
			DateSendM = dateSend;
			FullName = customerFullName;
			Mobiles = mobiles;
			Count = count;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Payamak\\Entities\\Setting\\GhestMItem.cs", 60);
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
			returnedSaveFuncInfo.AddReturnedValue(await service.SaveAsync(item, token), ReturnedState.None, "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Payamak\\Entities\\Setting\\GhestMItem.cs", 76);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Payamak\\Entities\\Setting\\GhestMItem.cs", 80);
			res.AddReturnedValue(ex, "", "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Payamak\\Entities\\Setting\\GhestMItem.cs", 81);
		}
		return res;
	}
}
