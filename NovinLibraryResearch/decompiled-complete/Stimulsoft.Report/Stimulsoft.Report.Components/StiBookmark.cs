using System.ComponentModel;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Engine;

namespace Stimulsoft.Report.Components;

public class StiBookmark
{
	private StiBookmarksCollection bookmarks;

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public StiBookmarksCollection Bookmarks
	{
		get
		{
			return bookmarks ?? (bookmarks = new StiBookmarksCollection());
		}
		set
		{
			bookmarks = value;
		}
	}

	[StiSerializable]
	[DefaultValue("")]
	public string Text { get; set; }

	[StiSerializable]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	public StiEngineVersion Engine { get; set; } = (StiEngineVersion)1;


	public StiBookmark this[string name] => Bookmarks[name];

	[StiSerializable]
	[DefaultValue("")]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	public string BookmarkText { get; set; }

	[StiEngine(/*Could not decode attribute arguments.*/)]
	public object ParentComponent { get; set; }

	[StiSerializable]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	[DefaultValue(null)]
	public string ComponentGuid { get; set; }

	[StiSerializable]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	[DefaultValue(false)]
	public bool IsManualBookmark { get; set; }

	[StiSerializable]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	[DefaultValue(-1)]
	public int PageIndex { get; set; } = -1;


	public void Add(string name)
	{
		if (Bookmarks.IndexOf(name) == -1)
		{
			Bookmarks.Add(new StiBookmark(name, this));
		}
	}

	public StiBookmark()
		: this(string.Empty, null)
	{
	}

	public StiBookmark(string text, object parentComponent)
		: this(text, "", parentComponent)
	{
	}

	public StiBookmark(string text, string bookmarkText, object parentComponent)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		Text = text;
		BookmarkText = bookmarkText;
		ParentComponent = parentComponent;
	}
}
