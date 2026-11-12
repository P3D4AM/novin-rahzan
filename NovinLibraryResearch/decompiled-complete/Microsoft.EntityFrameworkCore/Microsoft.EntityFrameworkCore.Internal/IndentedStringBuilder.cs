using System;
using System.IO;
using System.Text;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Internal;

public class IndentedStringBuilder
{
	private sealed class Indenter : IDisposable
	{
		private readonly IndentedStringBuilder _stringBuilder;

		public Indenter(IndentedStringBuilder stringBuilder)
		{
			_stringBuilder = stringBuilder;
			_stringBuilder.IncrementIndent();
		}

		public void Dispose()
		{
			_stringBuilder.DecrementIndent();
		}
	}

	private const byte IndentSize = 4;

	private byte _indent;

	private bool _indentPending = true;

	private readonly StringBuilder _stringBuilder = new StringBuilder();

	public virtual int Length => _stringBuilder.Length;

	public IndentedStringBuilder()
	{
	}

	public IndentedStringBuilder([JetBrains.Annotations.NotNull] IndentedStringBuilder from)
	{
		_indent = from._indent;
	}

	public virtual IndentedStringBuilder Append([JetBrains.Annotations.NotNull] object o)
	{
		DoIndent();
		_stringBuilder.Append(o);
		return this;
	}

	public virtual IndentedStringBuilder AppendLine()
	{
		AppendLine(string.Empty);
		return this;
	}

	public virtual IndentedStringBuilder AppendLine([JetBrains.Annotations.NotNull] object o)
	{
		string text = o.ToString();
		if (text.Length != 0)
		{
			DoIndent();
		}
		_stringBuilder.AppendLine(text);
		_indentPending = true;
		return this;
	}

	public virtual IndentedStringBuilder AppendLines([JetBrains.Annotations.NotNull] object o, bool skipFinalNewline = false)
	{
		using (StringReader stringReader = new StringReader(o.ToString()))
		{
			bool flag = true;
			string text;
			while ((text = stringReader.ReadLine()) != null)
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					AppendLine();
				}
				if (text.Length != 0)
				{
					Append(text);
				}
			}
		}
		if (!skipFinalNewline)
		{
			AppendLine();
		}
		return this;
	}

	public virtual IndentedStringBuilder Clear()
	{
		_stringBuilder.Clear();
		return this;
	}

	public virtual IndentedStringBuilder IncrementIndent()
	{
		_indent++;
		return this;
	}

	public virtual IndentedStringBuilder DecrementIndent()
	{
		if (_indent > 0)
		{
			_indent--;
		}
		return this;
	}

	public virtual IDisposable Indent()
	{
		return new Indenter(this);
	}

	public override string ToString()
	{
		return _stringBuilder.ToString();
	}

	private void DoIndent()
	{
		if (_indentPending && _indent > 0)
		{
			_stringBuilder.Append(new string(' ', _indent * 4));
		}
		_indentPending = false;
	}
}
