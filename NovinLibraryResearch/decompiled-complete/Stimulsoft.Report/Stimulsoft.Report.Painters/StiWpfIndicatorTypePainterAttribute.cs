using System;

namespace Stimulsoft.Report.Painters;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class StiWpfIndicatorTypePainterAttribute : Attribute
{
	private string painterTypeName = string.Empty;

	public string PainterTypeName => painterTypeName;

	public StiWpfIndicatorTypePainterAttribute(string painterTypeName)
	{
		this.painterTypeName = painterTypeName;
	}

	public StiWpfIndicatorTypePainterAttribute(Type painterType)
	{
		painterTypeName = painterType.AssemblyQualifiedName;
	}
}
