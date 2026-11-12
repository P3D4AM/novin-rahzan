using System;

namespace Stimulsoft.Report.Engine;

[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
public class StiEngineAttribute : Attribute
{
	public StiEngineVersion Version { get; }

	public StiEngineAttribute(StiEngineVersion version)
	{
		Version = version;
	}
}
