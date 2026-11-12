using System;

namespace Microsoft.IdentityModel.Json;

[AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false)]
internal sealed class JsonConstructorAttribute : Attribute
{
}
