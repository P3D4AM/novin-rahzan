using System.Runtime.InteropServices;

namespace Microsoft.SqlServer.Management.Diagnostics;

[StructLayout(LayoutKind.Sequential, Size = 1)]
internal struct SQLToolsCommonTraceLvl
{
	public const uint L1 = 1u;

	public const uint L2 = 2u;

	public const uint L3 = 4u;

	public const uint L4 = 8u;

	public const uint Always = 268435456u;

	public const uint Warning = 536870912u;

	public const uint Error = 1073741824u;
}
