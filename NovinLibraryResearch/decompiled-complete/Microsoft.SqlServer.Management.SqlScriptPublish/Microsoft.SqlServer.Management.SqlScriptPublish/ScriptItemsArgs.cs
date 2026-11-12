using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.SqlScriptPublish;

public class ScriptItemsArgs : EventArgs
{
	private IEnumerable<Urn> urns;

	public IEnumerable<Urn> Urns => urns;

	public ScriptItemsArgs(IEnumerable<Urn> urns)
	{
		this.urns = urns;
	}
}
