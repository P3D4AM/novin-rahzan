using System;

namespace Microsoft.Identity.Client.Platforms.net45;

internal class SilentWebUIDoneEventArgs : EventArgs
{
	public Exception TransferedException { get; private set; }

	public SilentWebUIDoneEventArgs(Exception e)
	{
		TransferedException = e;
	}
}
