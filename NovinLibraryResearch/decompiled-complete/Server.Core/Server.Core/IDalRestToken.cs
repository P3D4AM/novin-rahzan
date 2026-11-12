using System;
using System.Threading.Tasks;
using Server.Core.Entities;

namespace Server.Core;

public interface IDalRestToken
{
	Task<RestToken> GetAsync(string token);

	Task<RestToken> SaveAsync(string username, string password, TimeSpan offset);
}
