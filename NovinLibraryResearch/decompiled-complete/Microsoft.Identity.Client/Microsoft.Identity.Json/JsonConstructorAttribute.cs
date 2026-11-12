using System;

namespace Microsoft.Identity.Json;

[AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false)]
internal sealed class JsonConstructorAttribute : Attribute
{
}
