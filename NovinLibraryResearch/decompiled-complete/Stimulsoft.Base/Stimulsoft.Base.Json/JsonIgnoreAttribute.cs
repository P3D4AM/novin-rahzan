using System;

namespace Stimulsoft.Base.Json;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public sealed class JsonIgnoreAttribute : Attribute
{
}
