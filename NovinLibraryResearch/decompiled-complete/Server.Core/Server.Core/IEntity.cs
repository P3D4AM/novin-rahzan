using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Core.Connection;

namespace Server.Core;

public interface IEntity<T>
{
	Task<List<T>> ApplyAccessAsync(IEnumerable<T> lst, IClsConnectionData connection);
}
