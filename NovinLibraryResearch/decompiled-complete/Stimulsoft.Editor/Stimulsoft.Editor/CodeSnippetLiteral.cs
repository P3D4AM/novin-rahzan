using System.ComponentModel;

namespace Stimulsoft.Editor;

public class CodeSnippetLiteral : ICodeSnippetLiteral
{
	private string defaultValue = string.Empty;

	private string function = string.Empty;

	private string id = string.Empty;

	private string toolTip = string.Empty;

	private bool editable = true;

	private string type = string.Empty;

	public string Default
	{
		get
		{
			return defaultValue;
		}
		set
		{
			defaultValue = value;
		}
	}

	public string Function
	{
		get
		{
			return function;
		}
		set
		{
			function = value;
		}
	}

	public string ID
	{
		get
		{
			return id;
		}
		set
		{
			id = value;
		}
	}

	public string ToolTip
	{
		get
		{
			return toolTip;
		}
		set
		{
			toolTip = value;
		}
	}

	[DefaultValue(true)]
	public bool Editable
	{
		get
		{
			return editable;
		}
		set
		{
			editable = value;
		}
	}

	public string Type
	{
		get
		{
			return type;
		}
		set
		{
			type = value;
		}
	}
}
