using System;

namespace Stimulsoft.Report.Painters;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class StiGdiIndicatorTypePainterAttribute : Attribute
{
	private string painterTypeName = string.Empty;

	public string PainterTypeName => painterTypeName;

	public StiGdiIndicatorTypePainterAttribute(string painterTypeName)
	{
		this.painterTypeName = painterTypeName;
	}

	public StiGdiIndicatorTypePainterAttribute(Type painterType)
	{
		painterTypeName = painterType.AssemblyQualifiedName;
	}
}
