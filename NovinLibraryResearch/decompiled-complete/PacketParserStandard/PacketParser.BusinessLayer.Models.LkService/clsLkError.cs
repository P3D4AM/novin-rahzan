using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models.LkService;

public class clsLkError
{
	public bool HasError { get; set; }

	public LkClientServerErrors Error { get; set; }

	public clsLkError(bool hasError, LkClientServerErrors error)
	{
		HasError = hasError;
		Error = error;
	}
}
