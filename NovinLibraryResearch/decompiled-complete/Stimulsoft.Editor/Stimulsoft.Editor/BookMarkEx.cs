using System.ComponentModel;

namespace Stimulsoft.Editor;

public class BookMarkEx : BookMark, IBookMarkEx, IBookMark
{
	private string name = string.Empty;

	private string description = string.Empty;

	private string url = string.Empty;

	[Description("Gets or sets name of the bookmark.")]
	public string Name
	{
		get
		{
			return name;
		}
		set
		{
			name = value;
		}
	}

	[Description("Gets or sets bookmark description.")]
	public string Description
	{
		get
		{
			return description;
		}
		set
		{
			description = value;
		}
	}

	[Description("Gets or sets bookmark url.")]
	public string Url
	{
		get
		{
			return url;
		}
		set
		{
			url = value;
		}
	}

	public BookMarkEx(int line, int pos, int index, string name, string description, string url)
		: base(line, pos, index)
	{
		this.name = name;
		this.description = description;
		this.url = url;
	}

	public BookMarkEx()
		: this(0, 0, 0, string.Empty, string.Empty, string.Empty)
	{
	}

	public override void Assign(IBookMark source)
	{
		base.Assign(source);
		if (source is IBookMarkEx)
		{
			IBookMarkEx bookMarkEx = (IBookMarkEx)source;
			Name = bookMarkEx.Name;
			Description = bookMarkEx.Description;
			Url = bookMarkEx.Url;
		}
	}
}
