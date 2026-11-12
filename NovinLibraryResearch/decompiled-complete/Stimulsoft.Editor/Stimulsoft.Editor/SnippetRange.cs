using System.ComponentModel;
using System.Drawing;

namespace Stimulsoft.Editor;

public class SnippetRange : Range
{
	private string id;

	private string tooltip;

	private bool isEditable;

	[Description("Represents name of code snippet.")]
	public string Id
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

	[Description("Represents short description of code snippet.")]
	public string Tooltip
	{
		get
		{
			return tooltip;
		}
		set
		{
			tooltip = value;
		}
	}

	[Description("Gets or sets boolean value indicating whether snippet can be edited.")]
	public bool IsEditable
	{
		get
		{
			return isEditable;
		}
		set
		{
			isEditable = value;
		}
	}

	public SnippetRange(Point start, Point end, string id, string tooltip, bool isEditable)
		: base(start, end)
	{
		this.id = id;
		this.tooltip = tooltip;
		this.isEditable = isEditable;
	}
}
