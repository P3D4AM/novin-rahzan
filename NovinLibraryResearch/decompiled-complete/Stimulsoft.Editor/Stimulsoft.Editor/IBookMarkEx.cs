namespace Stimulsoft.Editor;

public interface IBookMarkEx : IBookMark
{
	string Name { get; set; }

	string Description { get; set; }

	string Url { get; set; }
}
