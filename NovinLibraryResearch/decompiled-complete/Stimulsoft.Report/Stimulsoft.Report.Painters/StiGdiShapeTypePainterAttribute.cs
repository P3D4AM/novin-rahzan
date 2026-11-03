using System;

namespace Stimulsoft.Report.Painters;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class StiGdiShapeTypePainterAttribute : Attribute
{
	private string painterTypeName = string.Empty;

	public string PainterTypeName => painterTypeName;

	public StiGdiShapeTypePainterAttribute(string painterTypeName)
	{
		this.painterTypeName = painterTypeName;
	}

	public StiGdiShapeTypePainterAttribute(Type painterType)
	{
		painterTypeName = painterType.AssemblyQualifiedName;
	}
}
