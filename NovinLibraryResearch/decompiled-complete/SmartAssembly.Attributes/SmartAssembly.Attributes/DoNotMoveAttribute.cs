using System;

namespace SmartAssembly.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public sealed class DoNotMoveAttribute : Attribute
{
}
