using System.Diagnostics;

namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
[Conditional("MULTI_TARGETING_SUPPORT_ATTRIBUTES")]
internal sealed class DisableRuntimeMarshallingAttribute : Attribute
{
}
