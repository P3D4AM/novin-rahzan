using System;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.SqlScriptPublish;

public class ScriptEventArgs : EventArgs
{
	private Urn urn;

	private Exception error;

	private bool continueScripting;

	private bool completed;

	public Urn Urn => urn;

	public Exception Error => error;

	public bool ContinueScripting
	{
		get
		{
			return continueScripting;
		}
		set
		{
			continueScripting = value;
		}
	}

	public bool Completed => completed;

	public ScriptEventArgs(Urn urn, Exception error)
	{
		if (urn != null)
		{
			this.urn = urn;
		}
		this.error = error;
	}

	public ScriptEventArgs(Urn urn, Exception error, bool completed)
		: this(urn, error)
	{
		this.completed = completed;
	}
}
