using System;

namespace Microsoft.Identity.Json.Serialization;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
internal sealed class OnErrorAttribute : Attribute
{
}
