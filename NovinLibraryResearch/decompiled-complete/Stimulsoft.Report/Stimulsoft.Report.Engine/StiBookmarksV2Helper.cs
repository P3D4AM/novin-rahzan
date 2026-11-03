using System.Collections;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

public sealed class StiBookmarksV2Helper
{
	private static void FillTreeNodeFromBookmarks(StiBookmark parentBookmark, TreeNode parentNode)
	{
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		int num = 5;
		int num2 = 4;
		if (StiScale.Step > 1.0)
		{
			num -= 3;
			num2 -= 3;
		}
		if (StiScale.Step >= 2.0)
		{
			num += 3;
			num2 += 3;
		}
		lock (((ICollection)parentBookmark.Bookmarks).SyncRoot)
		{
			foreach (StiBookmark bookmark in parentBookmark.Bookmarks)
			{
				TreeNode val = new TreeNode(bookmark.Text, num, num);
				parentNode.Nodes.Add(val);
				val.Tag = bookmark;
				FillTreeNodeFromBookmarks(bookmark, val);
				if (val.Nodes.Count == 0)
				{
					val.SelectedImageIndex = num2;
					val.ImageIndex = num2;
				}
			}
		}
	}

	public static TreeNode GetTreeNodeFromBookmarks(StiBookmark bookmark)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		int num = 3;
		if (StiScale.Step >= 2.0)
		{
			num = 3;
		}
		TreeNode val = new TreeNode(bookmark.Text, num, num)
		{
			Tag = bookmark
		};
		FillTreeNodeFromBookmarks(bookmark, val);
		return val;
	}

	public static void Add(StiBookmark bookmark, string name)
	{
		StiBookmark stiBookmark = CreateBookmark(name);
		stiBookmark.IsManualBookmark = true;
		bookmark.Bookmarks.Add(stiBookmark);
	}

	public static StiBookmark GetBookmark(StiBookmark bookmark, string name)
	{
		int num = bookmark.Bookmarks.IndexOf(name);
		StiBookmark stiBookmark;
		if (num == -1)
		{
			stiBookmark = CreateBookmark(name);
			bookmark.Bookmarks.Add(stiBookmark);
		}
		else
		{
			stiBookmark = bookmark.Bookmarks[num];
		}
		return stiBookmark;
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

	public static void Pack(StiBookmark bookmark)
	{
		bookmark.ParentComponent = null;
		foreach (StiBookmark bookmark2 in bookmark.Bookmarks)
		{
			Pack(bookmark2);
		}
	}

	public static StiBookmark CreateBookmark(string text)
	{
		return CreateBookmark(text, null);
	}

	public static StiBookmark CreateBookmark(string text, string componentGuid)
	{
		return new StiBookmark
		{
			Text = text,
			ComponentGuid = componentGuid,
			Engine = (StiEngineVersion)2
		};
	}
}
