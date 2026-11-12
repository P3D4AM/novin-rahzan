using System;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace SharedActivation;

public interface ICusDal : IHasContext
{
	Task<WebCus> GetAsync(Guid tafsilGuid);

	WebCus Get(Guid tafsilGuid);
}
