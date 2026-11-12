using System.Collections.Generic;
using PacketParser.Entities;
using PacketParser.TaxService;
using Server.Core.Assistance;

namespace Server.Core.Connection.Dal;

public class Initializer
{
	public static void Init(AggregateDal Dal, IEnumerable<WebContextHolder> cotexts, short countOfReceiveDataFromWp, ITaxConnectionManager taxManager)
	{
		Server.Core.Assistance.Initializer.Init(Dal);
		HubConnection.Init(cotexts, countOfReceiveDataFromWp, taxManager);
	}
}
