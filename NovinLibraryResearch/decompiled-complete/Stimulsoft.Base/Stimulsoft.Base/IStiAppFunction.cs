using System.Collections.Generic;

namespace Stimulsoft.Base;

public interface IStiAppFunction : IStiAppCell
{
	string GetName();

	object Invoke(IEnumerable<object> arguments);
}
