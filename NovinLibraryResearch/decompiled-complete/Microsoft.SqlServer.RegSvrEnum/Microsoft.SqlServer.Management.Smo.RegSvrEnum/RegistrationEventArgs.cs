using System;

namespace Microsoft.SqlServer.Management.Smo.RegSvrEnum;

public sealed class RegistrationEventArgs : EventArgs
{
	private RegistrationInfo node;

	public RegistrationInfo Node => node;

	public ParentRegistrationInfo Parent => Node.Parent;

	public RegistrationEventArgs(RegistrationInfo node)
	{
		this.node = node;
	}
}
