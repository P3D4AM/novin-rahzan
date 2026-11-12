using System;

namespace Microsoft.Identity.Client;

[AttributeUsage(AttributeTargets.All)]
internal class PreserveAttribute : Attribute
{
	public bool Conditional;

	public bool AllMembers;
}
