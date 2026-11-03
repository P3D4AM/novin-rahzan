using System;

namespace Stimulsoft.Report.Components.Design;

[AttributeUsage(AttributeTargets.Class)]
public sealed class StiElementInteractionDesignerAttribute : Attribute
{
	public string DesignerTypeName { get; }

	public StiElementInteractionDesignerAttribute(string designerTypeName)
	{
		DesignerTypeName = designerTypeName;
	}

	public StiElementInteractionDesignerAttribute(Type type)
	{
		DesignerTypeName = type.AssemblyQualifiedName;
	}
}
