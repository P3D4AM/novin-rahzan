using System;

namespace SmartAssembly.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field)]
[DoNotObfuscate]
[DoNotPrune]
public sealed class DoNotCaptureAttribute : Attribute
{
}
