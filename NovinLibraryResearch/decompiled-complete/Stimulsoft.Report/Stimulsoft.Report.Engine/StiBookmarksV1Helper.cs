using System.Windows.Forms;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

public sealed class StiBookmarksV1Helper
{
	public static void Pack(StiBookmark bookmark)
	{
		bookmark.ParentComponent = null;
		foreach (StiBookmark bookmark2 in bookmark.Bookmarks)
		{
			Pack(bookmark2);
		}
	}

	public static void Add(StiBookmark bookmark, string name)
	{
		if (bookmark.Bookmarks.IndexOf(name) == -1)
		{
			bookmark.Bookmarks.Add(CreateBookmark(name, bookmark));
		}
	}

	public static void AddBookmarks(StiBookmark bookmark, TreeNode treeNode)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		int num = 5;
		int num2 = 4;
		foreach (StiBookmark bookmark2 in bookmark.Bookmarks)
		{
			TreeNode val = new TreeNode(bookmark2.Text, num, num);
			treeNode.Nodes.Add(val);
			if (string.IsNullOrEmpty(bookmark2.BookmarkText))
			{
				val.Tag = bookmark2.Text;
			}
			else
			{
				val.Tag = bookmark2.BookmarkText;
			}
			AddBookmarks(bookmark2, val);
			if (val.Nodes.Count == 0)
			{
				val.SelectedImageIndex = num2;
				val.ImageIndex = num2;
			}
		}
	}

	public static TreeNode GetTreeNode(StiBookmark bookmark)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		int num = 3;
		TreeNode val = new TreeNode(bookmark.Text, num, num)
		{
			Tag = (string.IsNullOrEmpty(bookmark.BookmarkText) ? bookmark.Text : bookmark.BookmarkText)
		};
		AddBookmarks(bookmark, val);
		return val;
	}

	public static void PrepareBookmark(StiBookmark bookmark)
	{
		int num = 0;
		while (num < bookmark.Bookmarks.Count - 1)
		{
			if (bookmark.Bookmarks[num].Text == bookmark.Bookmarks[num + 1].Text)
			{
				bookmark.Bookmarks[num].Bookmarks.AddRange(bookmark.Bookmarks[num + 1].Bookmarks);
				bookmark.Bookmarks.Remove(bookmark.Bookmarks[num + 1]);
			}
			else
			{
				num++;
			}
		}
		for (int i = 0; i < bookmark.Bookmarks.Count; i++)
		{
			StiBookmark stiBookmark = bookmark.Bookmarks[i];
			int num2 = i + 1;
			while (num2 < bookmark.Bookmarks.Count)
			{
				StiBookmark stiBookmark2 = bookmark.Bookmarks[num2];
				if (stiBookmark.Text == stiBookmark2.Text && stiBookmark.ParentComponent == stiBookmark2.ParentComponent)
				{
					stiBookmark.Bookmarks.AddRange(stiBookmark2.Bookmarks);
					bookmark.Bookmarks.RemoveAt(num2);
				}
				else
				{
					num2++;
				}
			}
		}
		foreach (StiBookmark bookmark2 in bookmark.Bookmarks)
		{
			PrepareBookmark(bookmark2);
		}
	}

	public static StiBookmark GetBookmark(StiBookmark bookmark, string name)
	{
		int num = bookmark.Bookmarks.IndexOf(name);
		StiBookmark stiBookmark;
		if (num == -1)
		{
			stiBookmark = CreateBookmark(name, bookmark.Bookmarks);
			bookmark.Bookmarks.Add(stiBookmark);
		}
		else
		{
			stiBookmark = bookmark.Bookmarks[num];
		}
		return stiBookmark;
	}

	public static StiBookmark CreateBookmark(string text, object parentComponent)
	{
		return CreateBookmark(text, string.Empty, parentComponent);
	}

	public static StiBookmark CreateBookmark(string text, string bookmarkText, object parentComponent)
	{
		return new StiBookmark
		{
			Text = text,
			BookmarkText = bookmarkText,
			ParentComponent = parentComponent,
			Engine = (StiEngineVersion)1
		};
	}
}
