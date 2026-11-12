using System;
using System.Collections;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Stimulsoft.Editor;

public class CodeSnippetMembers : CodeCompletionProvider, ICodeSnippetMembers, ICodeSnippetsProvider, ICodeCompletionProvider, IList, ICollection, IEnumerable
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

	[XmlIgnore]
	public ICodeSnippetMember Parent
	{
		get
		{
			return parent;
		}
		set
		{
			parent = value;
		}
	}

	[XmlIgnore]
	public new ICodeSnippetMember this[int index] => base[index] as ICodeSnippetMember;

	public CodeSnippetMember[] Members
	{
		get
		{
			CodeSnippetMember[] array = new CodeSnippetMember[Count];
			CopyTo(array);
			return array;
		}
		set
		{
			Clear();
			foreach (CodeSnippetMember value2 in value)
			{
				Add(value2);
			}
			FixupMembers();
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

	public override object XmlInfo
	{
		get
		{
			return new XmlCodeSnippetMembersInfo(this);
		}
		set
		{
			((XmlCodeSnippetMembersInfo)value).FixupReferences(this);
		}
	}

	public CodeSnippetMembers()
	{
		base.UseIndent = false;
		base.EditField = StringConsts.CodeSnippetsEditField;
	}

	public CodeSnippetMembers(ICodeSnippetMember parent)
		: this()
	{
		this.parent = parent;
	}

	protected void FixupMembers()
	{
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				ICodeSnippetMember codeSnippetMember = (ICodeSnippetMember)enumerator.Current;
				codeSnippetMember.Parent = this;
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
	}

	protected override Type GetXmlType()
	{
		return typeof(XmlCodeSnippetMembersInfo);
	}

	public override void OnClosePopup(object sender, ClosingEventArgs e)
	{
		if (e.Accepted && base.SelIndex >= 0 && base.SelIndex < Count)
		{
			e.Provider = this[base.SelIndex].SnippetsAndMembers;
			e.Handled = e.Provider.Count > 0;
		}
		base.OnClosePopup(sender, e);
	}

	public ICodeSnippetMember AddSnippetMember()
	{
		ICodeSnippetMember codeSnippetMember = new CodeSnippetMember(this);
		base.Add(codeSnippetMember);
		return codeSnippetMember;
	}

	public ICodeSnippetMember InsertSnippetMember(int index)
	{
		ICodeSnippetMember codeSnippetMember = new CodeSnippetMember();
		base.Insert(index, codeSnippetMember);
		return codeSnippetMember;
	}

	public override string GetName(int index)
	{
		return this[index].Name;
	}

	public override string GetText(int index)
	{
		return string.Empty;
	}

	public override int GetImageIndex(int index)
	{
		return this[index].ImageIndex;
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

	public override ICodeCompletionProvider GetParent()
	{
		if (parent == null)
		{
			return null;
		}
		return parent.Parent;
	}

	public ICodeSnippet FindByShortcut(string shortcut, bool caseSensitive)
	{
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				ICodeSnippetMember codeSnippetMember = (ICodeSnippetMember)enumerator.Current;
				ICodeSnippet codeSnippet = codeSnippetMember.Snippets.FindByShortcut(shortcut, caseSensitive);
				if (codeSnippet != null)
				{
					return codeSnippet;
				}
				codeSnippet = codeSnippetMember.Members.FindByShortcut(shortcut, caseSensitive);
				if (codeSnippet != null)
				{
					return codeSnippet;
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
