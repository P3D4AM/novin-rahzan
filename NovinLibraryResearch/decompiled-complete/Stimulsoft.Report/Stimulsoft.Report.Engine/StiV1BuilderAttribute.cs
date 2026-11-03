using System;

namespace Stimulsoft.Report.Engine;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class StiV1BuilderAttribute : Attribute
{
	public string BuilderTypeName { get; }

	public StiV1BuilderAttribute(string builderTypeName)
	{
		BuilderTypeName = builderTypeName;
	}

	public StiV1BuilderAttribute(Type builderType)
	{
		BuilderTypeName = builderType.AssemblyQualifiedName;
	}
}
