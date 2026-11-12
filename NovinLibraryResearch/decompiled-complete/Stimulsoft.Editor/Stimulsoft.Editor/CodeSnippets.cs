using System;
using System.Collections;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Stimulsoft.Editor;

public class CodeSnippets : CodeCompletionProvider, ICodeSnippets, ICodeSnippetsProvider, ICodeCompletionProvider, IList, ICollection, IEnumerable
{
	private ICodeSnippetMember parent;

	public override int ColumnCount => 1;

	public new ICodeSnippet this[int index] => base[index] as ICodeSnippet;

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

	public override object XmlInfo
	{
		get
		{
			return new XmlCodeSnippetsInfo(this);
		}
		set
		{
			((XmlCodeSnippetsInfo)value).FixupReferences(this);
		}
	}

	public CodeSnippets()
	{
		base.ShowDescriptions = true;
		base.UseIndent = false;
		base.EditField = StringConsts.CodeSnippetsEditField;
	}

	public CodeSnippets(ICodeSnippetMember parent)
		: this()
	{
		this.parent = parent;
	}

	protected override Type GetXmlType()
	{
		return typeof(XmlCodeSnippetsInfo);
	}

	public ICodeSnippet AddSnippet()
	{
		ICodeSnippet codeSnippet = new CodeSnippet(this);
		base.Add(codeSnippet);
		return codeSnippet;
	}

	public ICodeSnippet InsertSnippet(int index)
	{
		ICodeSnippet codeSnippet = new CodeSnippet(this);
		base.Insert(index, codeSnippet);
		return codeSnippet;
	}

	public override ICodeCompletionProvider GetParent()
	{
		if (parent == null)
		{
			return null;
		}
		return parent.Parent;
	}

	public override string GetDescription(int index)
	{
		return this[index].Description;
	}

	public override string GetName(int index)
	{
		return this[index].Header.Title;
	}

	public override string GetText(int index)
	{
		return this[index].Code.Code;
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

	public ICodeSnippet FindByShortcut(string shortcut, bool caseSensitive)
	{
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				ICodeSnippet codeSnippet = (ICodeSnippet)enumerator.Current;
				if (string.Compare(shortcut, codeSnippet.Header.Shortcut, !caseSensitive) == 0)
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
