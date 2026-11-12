using System;

namespace Microsoft.Identity.Json;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
internal sealed class JsonRequiredAttribute : Attribute
{
}
