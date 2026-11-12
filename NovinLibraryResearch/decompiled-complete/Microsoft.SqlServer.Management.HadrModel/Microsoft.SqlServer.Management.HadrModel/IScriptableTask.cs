using System.Collections.Generic;
using Microsoft.SqlServer.Management.Common;

namespace Microsoft.SqlServer.Management.HadrModel;

public interface IScriptableTask
{
	List<ServerConnection> ScriptingConnections { get; }
}
