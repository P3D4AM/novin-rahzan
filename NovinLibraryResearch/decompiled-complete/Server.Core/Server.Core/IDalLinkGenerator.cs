using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace Server.Core;

public interface IDalLinkGenerator
{
	Task<WebLinkGenerator> GetAsync(short depId);

	Task<ReturnedSaveFuncInfo> SaveAsync(WebLinkGenerator item);
}
