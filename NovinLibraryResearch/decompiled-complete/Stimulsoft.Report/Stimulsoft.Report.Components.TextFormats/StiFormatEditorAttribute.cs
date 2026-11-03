using System;

namespace Stimulsoft.Report.Components.TextFormats;

[AttributeUsage(AttributeTargets.Class)]
public sealed class StiFormatEditorAttribute : Attribute
{
	public string EditorTypeName { get; }

	public StiFormatEditorAttribute(string editorTypeName)
	{
		EditorTypeName = editorTypeName;
	}

	public StiFormatEditorAttribute(Type type)
	{
		EditorTypeName = type.AssemblyQualifiedName;
	}
}
