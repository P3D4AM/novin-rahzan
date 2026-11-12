using System;

namespace Stimulsoft.Base.Serializing;

[AttributeUsage(AttributeTargets.Property)]
public sealed class StiNonSerializedAttribute : Attribute
{
}
