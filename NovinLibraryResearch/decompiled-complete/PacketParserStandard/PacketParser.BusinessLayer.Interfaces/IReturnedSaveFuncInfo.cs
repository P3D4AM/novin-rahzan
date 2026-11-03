using System.Threading.Tasks;

namespace PacketParser.BusinessLayer.Interfaces;

public interface IReturnedSaveFuncInfo
{
	Task AddErrorAsync(string errorMessage, string key = "");

	Task AddWarningAsync(string warningMessage, string key = "");

	Task AddInformationAsync(string message, string key = "");
}
