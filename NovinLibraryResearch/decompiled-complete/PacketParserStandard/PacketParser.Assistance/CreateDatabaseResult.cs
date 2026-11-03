using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace PacketParser.Assistance;

public class CreateDatabaseResult
{
	public string ConnectionString { get; set; }

	public ReturnedSaveFuncInfo ReturnedSaveFuncInfo { get; set; } = new ReturnedSaveFuncInfo();


	public EnCreateDataBase Status { get; set; }
}
