using System;
using System.Collections;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Stimulsoft.Editor;

public class CodeSnippetsProvider : CodeCompletionProvider, ICodeSnippetsProvider, ICodeCompletionProvider, IList, ICollection, IEnumerable
{
	private ICodeSnippetMember parent;

	public override int ColumnCount => 1;

	[XmlIgnore]
	public override string EditPath
	{
		get
		{
			if (parent == null)
			{
				return string.Empty;
			}
			return parent.EditPath;
		}
	}

	public override ImageList Images
	{
		get
		{
			if (base.Images == null)
			{
				if (GetParent() == null)
				{
					return null;
				}
				return GetParent().Images;
			}
			return base.Images;
		}
		set
		{
			base.Images = value;
		}
	}

	public CodeSnippetsProvider()
	{
		base.ShowDescriptions = true;
		base.UseIndent = false;
		base.EditField = StringConsts.CodeSnippetsEditField;
	}

	public CodeSnippetsProvider(ICodeSnippetMember parent)
		: this()
	{
		this.parent = parent;
	}

	public override ICodeCompletionProvider GetParent()
	{
		if (parent == null)
		{
			return null;
		}
		return parent.Parent;
	}

	public override void OnClosePopup(object sender, ClosingEventArgs e)
	{
		if (e.Accepted && base.SelIndex >= 0 && base.SelIndex < Count)
		{
			object obj = this[base.SelIndex];
			if (obj is ICodeSnippetMember)
			{
				e.Provider = ((ICodeSnippetMember)obj).SnippetsAndMembers;
				e.Handled = e.Provider.Count > 0;
			}
		}
		base.OnClosePopup(sender, e);
	}

	public override string GetDescription(int index)
	{
		object obj = this[index];
		if (!(obj is ICodeSnippet))
		{
			return string.Empty;
		}
		return ((ICodeSnippet)obj).Description;
	}

	public override string GetName(int index)
	{
		object obj = this[index];
		if (obj is ICodeSnippet)
		{
			return ((ICodeSnippet)obj).Header.Title;
		}
		if (obj is ICodeSnippetMember)
		{
			return ((ICodeSnippetMember)obj).Name;
		}
		return string.Empty;
	}

	public override string GetText(int index)
	{
		object obj = this[index];
		if (!(obj is ICodeSnippet))
		{
			return string.Empty;
		}
		return ((ICodeSnippet)obj).Code.Code;
	}

	public override int GetImageIndex(int index)
	{
		object obj = this[index];
		if (obj is ICodeSnippet)
		{
			return ((ICodeSnippet)obj).ImageIndex;
		}
		if (obj is ICodeSnippetMember)
		{
			return ((ICodeSnippetMember)obj).ImageIndex;
		}
		return -1;
	}

	public override string GetColumnText(int index, int column)
	{
		if (column != 0)
		{
			return string.Empty;
		}
		return GetName(index);
	}

	public override bool ColumnVisible(int column)
	{
		return true;
	}

	public ICodeSnippet FindByShortcut(string shortcut, bool caseSensitive)
	{
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object current = enumerator.Current;
				if (current is ICodeSnippet)
				{
					ICodeSnippet codeSnippet = (ICodeSnippet)current;
					if (string.Compare(shortcut, codeSnippet.Header.Shortcut, !caseSensitive) == 0)
					{
						return codeSnippet;
					}
				}
				else if (current is ICodeSnippetMember)
				{
					ICodeSnippetMember codeSnippetMember = (ICodeSnippetMember)current;
					ICodeSnippet codeSnippet2 = codeSnippetMember.Snippets.FindByShortcut(shortcut, caseSensitive);
					if (codeSnippet2 != null)
					{
						return codeSnippet2;
					}
					codeSnippet2 = codeSnippetMember.Members.FindByShortcut(shortcut, caseSensitive);
					if (codeSnippet2 != null)
					{
						return codeSnippet2;
					}
				}
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
		return null;
	}
}
