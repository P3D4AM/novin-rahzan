using System;

namespace SmartAssembly.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class DoNotSealTypeAttribute : Attribute
{
}
