using System;

namespace Microsoft.IdentityModel.Json;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
internal sealed class JsonRequiredAttribute : Attribute
{
}
