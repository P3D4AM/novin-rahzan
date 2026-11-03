using System;

namespace Stimulsoft.Report.Components.TextFormats;

[AttributeUsage(AttributeTargets.Class)]
public sealed class StiWpfFormatEditorAttribute : Attribute
{
	public string EditorTypeName { get; }

	public StiWpfFormatEditorAttribute(string editorTypeName)
	{
		EditorTypeName = editorTypeName;
	}

	public StiWpfFormatEditorAttribute(Type type)
	{
		EditorTypeName = type.AssemblyQualifiedName;
	}
}
