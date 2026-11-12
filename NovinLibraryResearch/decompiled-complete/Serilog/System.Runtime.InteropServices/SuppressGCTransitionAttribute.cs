using System.Diagnostics;

namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
[Conditional("MULTI_TARGETING_SUPPORT_ATTRIBUTES")]
internal sealed class SuppressGCTransitionAttribute : Attribute
{
}
