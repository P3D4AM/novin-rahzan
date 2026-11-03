using System;

namespace Stimulsoft.Report.Painters;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class StiGdiPainterAttribute : Attribute
{
	public string PainterTypeName { get; }

	public StiGdiPainterAttribute(string painterTypeName)
	{
		PainterTypeName = painterTypeName;
	}

	public StiGdiPainterAttribute(Type painterType)
	{
		PainterTypeName = painterType.AssemblyQualifiedName;
	}
}
