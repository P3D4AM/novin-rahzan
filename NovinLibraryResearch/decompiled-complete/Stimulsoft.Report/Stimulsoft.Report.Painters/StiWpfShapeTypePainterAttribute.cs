using System;

namespace Stimulsoft.Report.Painters;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class StiWpfShapeTypePainterAttribute : Attribute
{
	private string painterTypeName = string.Empty;

	public string PainterTypeName => painterTypeName;

	public StiWpfShapeTypePainterAttribute(string painterTypeName)
	{
		this.painterTypeName = painterTypeName;
	}

	public StiWpfShapeTypePainterAttribute(Type painterType)
	{
		painterTypeName = painterType.AssemblyQualifiedName;
	}
}
