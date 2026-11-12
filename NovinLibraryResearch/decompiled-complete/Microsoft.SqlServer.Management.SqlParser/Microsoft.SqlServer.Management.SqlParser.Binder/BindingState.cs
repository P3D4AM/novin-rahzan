using System;

namespace Microsoft.SqlServer.Management.SqlParser.Binder;

[Flags]
internal enum BindingState : sbyte
{
	Unbound = 0,
	BindSucceeded = 1,
	BindFailed = 2,
	BindSkipped = 4,
	Bound = 7
}
