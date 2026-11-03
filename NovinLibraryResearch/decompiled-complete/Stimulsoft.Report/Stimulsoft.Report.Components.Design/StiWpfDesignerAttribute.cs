using System;

namespace Stimulsoft.Report.Components.Design;

[AttributeUsage(AttributeTargets.Class)]
public sealed class StiWpfDesignerAttribute : Attribute
{
	public string DesignerTypeName { get; }

	public StiWpfDesignerAttribute(string designerTypeName)
	{
		DesignerTypeName = designerTypeName;
	}

	public StiWpfDesignerAttribute(Type type)
	{
		DesignerTypeName = type.AssemblyQualifiedName;
	}
}
