using System;

namespace Stimulsoft.Report.CodeDom;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
public sealed class StiSerialiazableToCodeAttributre : Attribute
{
}
