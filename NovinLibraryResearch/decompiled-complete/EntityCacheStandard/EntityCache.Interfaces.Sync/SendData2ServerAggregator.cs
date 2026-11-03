using System;
using PacketParser.Entities;

namespace EntityCache.Interfaces.Sync;

public class SendData2ServerAggregator
{
	public ISendData2Server Customer { get; private set; }

	public ISendData2Server CustomerGroup { get; private set; }

	public ISendData2Server CustomerTitle { get; private set; }

	public ISendData2Server Kol { get; private set; }

	public ISendData2Server Moein { get; private set; }

	public ISendData2Server Tafsil { get; private set; }

	public ISendData2Server LkInfo { get; private set; }

	public ISendData2Server Tafsilgroup { get; private set; }

	public ISendData2Server Product { get; private set; }

	public ISendData2Server ProductGroup { get; private set; }

	public ISendData2Server ProductVahed { get; private set; }

	public ISendData2Server Depot { get; private set; }

	public ISendData2Server ProductAttiibute { get; private set; }

	public ISendData2Server ProductPriceLevel { get; private set; }

	public ISendData2Server User { get; private set; }

	public ISendData2Server Cashier { get; private set; }

	public ISendData2Server LastPrice { get; set; }

	public ISendData2Server GhestM { get; set; }

	public ISendData2Server GhestMGroup { get; set; }

	public ISendData2Server GhestSh { get; set; }

	public ISendData2Server CheckM { get; set; }

	public ISendData2Server CheckSh { get; set; }

	public ISendData2Server Desc { get; private set; }

	public ISendData2Server CallingLog { get; private set; }

	public ISendData2Server Files { get; private set; }

	public ISendData2Server ContactUs { get; private set; }

	public ISendData2Server Visit { get; private set; }

	public ISendData2Server Ticket { get; private set; }

	public ISendData2Server TicketDet { get; private set; }

	public ISendData2Server Reception { get; private set; }

	public ISendData2Server Pardakht { get; private set; }

	public ISendData2Server Order { get; private set; }

	public ISendData2Server ViewChequeM { get; private set; }

	public ISendData2Server ViewChequeSh { get; private set; }

	public ISendData2Server Serial { get; private set; }

	public ISendData2Server Account { get; private set; }

	public SendData2ServerAggregator(ISendData2Server customer, ISendData2Server customerGroup, ISendData2Server customerTitle, ISendData2Server kol, ISendData2Server moein, ISendData2Server tafsil, ISendData2Server tafsilgroup, ISendData2Server product, ISendData2Server productGroup, ISendData2Server productVahed, ISendData2Server depot, ISendData2Server productAttribute, ISendData2Server user, ISendData2Server cashier, ISendData2Server checkSh, ISendData2Server checkM, ISendData2Server ghestSh, ISendData2Server ghestM, ISendData2Server lastPrice, ISendData2Server desc, ISendData2Server callingLog, ISendData2Server files, ISendData2Server contactUs, ISendData2Server visit, ISendData2Server ticket, ISendData2Server ticketDet, ISendData2Server reception, ISendData2Server pardakht, ISendData2Server order, ISendData2Server vChequeM, ISendData2Server vChequesH, ISendData2Server serial, ISendData2Server account, ISendData2Server productPriceLevel)
	{
		try
		{
			Customer = customer;
			CustomerGroup = customerGroup;
			CustomerTitle = customerTitle;
			Kol = kol;
			Moein = moein;
			Tafsil = tafsil;
			Tafsilgroup = tafsilgroup;
			Product = product;
			ProductGroup = productGroup;
			ProductVahed = productVahed;
			Depot = depot;
			ProductAttiibute = productAttribute;
			User = user;
			Cashier = cashier;
			CheckSh = checkSh;
			CheckM = checkM;
			GhestSh = ghestSh;
			GhestM = ghestM;
			LastPrice = lastPrice;
			Desc = desc;
			CallingLog = callingLog;
			Files = files;
			ContactUs = contactUs;
			Visit = visit;
			Ticket = ticket;
			TicketDet = ticketDet;
			Reception = reception;
			Pardakht = pardakht;
			Order = order;
			ViewChequeM = vChequeM;
			ViewChequeSh = vChequesH;
			Serial = serial;
			Account = account;
			ProductPriceLevel = productPriceLevel;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Interfaces\\Sync\\SendData2ServerAggregator.cs", 66);
		}
	}

	public void Reset()
	{
		try
		{
			Customer?.Reset();
			CustomerGroup?.Reset();
			CustomerTitle?.Reset();
			Kol?.Reset();
			Moein?.Reset();
			Tafsil?.Reset();
			Tafsilgroup?.Reset();
			Product?.Reset();
			ProductGroup?.Reset();
			ProductVahed?.Reset();
			Depot?.Reset();
			ProductAttiibute?.Reset();
			User?.Reset();
			Cashier?.Reset();
			CheckSh?.Reset();
			CheckM?.Reset();
			GhestSh?.Reset();
			GhestM?.Reset();
			LastPrice?.Reset();
			Desc?.Reset();
			CallingLog?.Reset();
			Files?.Reset();
			ContactUs?.Reset();
			Visit?.Reset();
			Ticket?.Reset();
			TicketDet?.Reset();
			Reception?.Reset();
			Pardakht?.Reset();
			Order?.Reset();
			Serial?.Reset();
			Account?.Reset();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Reset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Interfaces\\Sync\\SendData2ServerAggregator.cs", 156);
		}
	}
}
