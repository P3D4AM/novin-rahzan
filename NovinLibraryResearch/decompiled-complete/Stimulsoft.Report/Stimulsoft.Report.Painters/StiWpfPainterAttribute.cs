using System;

namespace Stimulsoft.Report.Painters;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class StiWpfPainterAttribute : Attribute
{
	public string PainterTypeName { get; }

	public StiWpfPainterAttribute(string painterTypeName)
	{
		PainterTypeName = painterTypeName;
	}

	public StiWpfPainterAttribute(Type painterType)
	{
		PainterTypeName = painterType.AssemblyQualifiedName;
	}
}
