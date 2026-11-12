using System;

namespace Microsoft.EntityFrameworkCore.Query;

[AttributeUsage(AttributeTargets.Parameter)]
public sealed class NotParameterizedAttribute : Attribute
{
}
