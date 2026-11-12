using System;

namespace Microsoft.SqlServer.Management.RegisteredServers;

public class DuplicateFoundEventArgs : EventArgs
{
	private bool applyToAll;

	private bool confirm;

	private bool cancel;

	public bool ApplyToAll
	{
		get
		{
			return applyToAll;
		}
		set
		{
			applyToAll = value;
		}
	}

	public bool Confirm
	{
		get
		{
			return confirm;
		}
		set
		{
			confirm = value;
		}
	}

	public bool Cancel
	{
		get
		{
			return cancel;
		}
		set
		{
			cancel = value;
		}
	}
}
