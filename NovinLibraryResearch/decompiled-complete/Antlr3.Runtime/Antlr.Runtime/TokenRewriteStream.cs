using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Antlr.Runtime;

[Serializable]
[DebuggerDisplay("TODO: TokenRewriteStream debugger display")]
public class TokenRewriteStream : CommonTokenStream
{
	protected class RewriteOperation
	{
		public int instructionIndex;

		public int index;

		public object text;

		protected TokenRewriteStream stream;

		protected RewriteOperation(TokenRewriteStream stream, int index)
		{
			this.stream = stream;
			this.index = index;
		}

		protected RewriteOperation(TokenRewriteStream stream, int index, object text)
		{
			this.index = index;
			this.text = text;
			this.stream = stream;
		}

		public virtual int Execute(StringBuilder buf)
		{
			return index;
		}

		public override string ToString()
		{
			string name = GetType().Name;
			int num = name.IndexOf('$');
			name = name.Substring(num + 1);
			return $"<{name}@{stream._tokens[index]}:\"{text}\">";
		}
	}

	private class InsertBeforeOp : RewriteOperation
	{
		public InsertBeforeOp(TokenRewriteStream stream, int index, object text)
			: base(stream, index, text)
		{
		}

		public override int Execute(StringBuilder buf)
		{
			buf.Append(text);
			if (stream._tokens[index].Type != -1)
			{
				buf.Append(stream._tokens[index].Text);
			}
			return index + 1;
		}
	}

	private class ReplaceOp : RewriteOperation
	{
		public int lastIndex;

		public ReplaceOp(TokenRewriteStream stream, int from, int to, object text)
			: base(stream, from, text)
		{
			lastIndex = to;
		}

		public override int Execute(StringBuilder buf)
		{
			if (text != null)
			{
				buf.Append(text);
			}
			return lastIndex + 1;
		}

		public override string ToString()
		{
			if (text == null)
			{
				return $"<DeleteOp@{stream._tokens[index]}..{stream._tokens[lastIndex]}>";
			}
			return $"<ReplaceOp@{stream._tokens[index]}..{stream._tokens[lastIndex]}:\"{text}\">";
		}
	}

	public const string DEFAULT_PROGRAM_NAME = "default";

	public const int PROGRAM_INIT_SIZE = 100;

	public const int MIN_TOKEN_INDEX = 0;

	protected IDictionary<string, IList<RewriteOperation>> programs;

	protected IDictionary<string, int> lastRewriteTokenIndexes;

	public TokenRewriteStream()
	{
		Init();
	}

	protected void Init()
	{
		programs = new Dictionary<string, IList<RewriteOperation>>();
		programs["default"] = new List<RewriteOperation>(100);
		lastRewriteTokenIndexes = new Dictionary<string, int>();
	}

	public TokenRewriteStream(ITokenSource tokenSource)
		: base(tokenSource)
	{
		Init();
	}

	public TokenRewriteStream(ITokenSource tokenSource, int channel)
		: base(tokenSource, channel)
	{
		Init();
	}

	public virtual void Rollback(int instructionIndex)
	{
		Rollback("default", instructionIndex);
	}

	public virtual void Rollback(string programName, int instructionIndex)
	{
		if (programs.TryGetValue(programName, out var value) && value != null)
		{
			List<RewriteOperation> list = new List<RewriteOperation>();
			for (int i = 0; i <= instructionIndex; i++)
			{
				list.Add(value[i]);
			}
			programs[programName] = list;
		}
	}

	public virtual void DeleteProgram()
	{
		DeleteProgram("default");
	}

	public virtual void DeleteProgram(string programName)
	{
		Rollback(programName, 0);
	}

	public virtual void InsertAfter(IToken t, object text)
	{
		InsertAfter("default", t, text);
	}

	public virtual void InsertAfter(int index, object text)
	{
		InsertAfter("default", index, text);
	}

	public virtual void InsertAfter(string programName, IToken t, object text)
	{
		InsertAfter(programName, t.TokenIndex, text);
	}

	public virtual void InsertAfter(string programName, int index, object text)
	{
		InsertBefore(programName, index + 1, text);
	}

	public virtual void InsertBefore(IToken t, object text)
	{
		InsertBefore("default", t, text);
	}

	public virtual void InsertBefore(int index, object text)
	{
		InsertBefore("default", index, text);
	}

	public virtual void InsertBefore(string programName, IToken t, object text)
	{
		InsertBefore(programName, t.TokenIndex, text);
	}

	public virtual void InsertBefore(string programName, int index, object text)
	{
		RewriteOperation rewriteOperation = new InsertBeforeOp(this, index, text);
		IList<RewriteOperation> program = GetProgram(programName);
		rewriteOperation.instructionIndex = program.Count;
		program.Add(rewriteOperation);
	}

	public virtual void Replace(int index, object text)
	{
		Replace("default", index, index, text);
	}

	public virtual void Replace(int from, int to, object text)
	{
		Replace("default", from, to, text);
	}

	public virtual void Replace(IToken indexT, object text)
	{
		Replace("default", indexT, indexT, text);
	}

	public virtual void Replace(IToken from, IToken to, object text)
	{
		Replace("default", from, to, text);
	}

	public virtual void Replace(string programName, int from, int to, object text)
	{
		if (from > to || from < 0 || to < 0 || to >= _tokens.Count)
		{
			throw new ArgumentException("replace: range invalid: " + from + ".." + to + "(size=" + _tokens.Count + ")");
		}
		RewriteOperation rewriteOperation = new ReplaceOp(this, from, to, text);
		IList<RewriteOperation> program = GetProgram(programName);
		rewriteOperation.instructionIndex = program.Count;
		program.Add(rewriteOperation);
	}

	public virtual void Replace(string programName, IToken from, IToken to, object text)
	{
		Replace(programName, from.TokenIndex, to.TokenIndex, text);
	}

	public virtual void Delete(int index)
	{
		Delete("default", index, index);
	}

	public virtual void Delete(int from, int to)
	{
		Delete("default", from, to);
	}

	public virtual void Delete(IToken indexT)
	{
		Delete("default", indexT, indexT);
	}

	public virtual void Delete(IToken from, IToken to)
	{
		Delete("default", from, to);
	}

	public virtual void Delete(string programName, int from, int to)
	{
		Replace(programName, from, to, null);
	}

	public virtual void Delete(string programName, IToken from, IToken to)
	{
		Replace(programName, from, to, null);
	}

	public virtual int GetLastRewriteTokenIndex()
	{
		return GetLastRewriteTokenIndex("default");
	}

	protected virtual int GetLastRewriteTokenIndex(string programName)
	{
		if (lastRewriteTokenIndexes.TryGetValue(programName, out var value))
		{
			return value;
		}
		return -1;
	}

	protected virtual void SetLastRewriteTokenIndex(string programName, int i)
	{
		lastRewriteTokenIndexes[programName] = i;
	}

	protected virtual IList<RewriteOperation> GetProgram(string name)
	{
		if (!programs.TryGetValue(name, out var value) || value == null)
		{
			return InitializeProgram(name);
		}
		return value;
	}

	private IList<RewriteOperation> InitializeProgram(string name)
	{
		IList<RewriteOperation> list = new List<RewriteOperation>(100);
		programs[name] = list;
		return list;
	}

	public virtual string ToOriginalString()
	{
		Fill();
		return ToOriginalString(0, Count - 1);
	}

	public virtual string ToOriginalString(int start, int end)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = start; i >= 0 && i <= end && i < _tokens.Count; i++)
		{
			if (Get(i).Type != -1)
			{
				stringBuilder.Append(Get(i).Text);
			}
		}
		return stringBuilder.ToString();
	}

	public override string ToString()
	{
		Fill();
		return ToString(0, Count - 1);
	}

	public virtual string ToString(string programName)
	{
		Fill();
		return ToString(programName, 0, Count - 1);
	}

	public override string ToString(int start, int end)
	{
		return ToString("default", start, end);
	}

	public virtual string ToString(string programName, int start, int end)
	{
		if (!programs.TryGetValue(programName, out var value))
		{
			value = null;
		}
		if (end > _tokens.Count - 1)
		{
			end = _tokens.Count - 1;
		}
		if (start < 0)
		{
			start = 0;
		}
		if (value == null || value.Count == 0)
		{
			return ToOriginalString(start, end);
		}
		StringBuilder stringBuilder = new StringBuilder();
		IDictionary<int, RewriteOperation> dictionary = ReduceToSingleOperationPerIndex(value);
		int num = start;
		while (num <= end && num < _tokens.Count)
		{
			RewriteOperation value2;
			bool flag = dictionary.TryGetValue(num, out value2);
			if (flag)
			{
				dictionary.Remove(num);
			}
			if (!flag || value2 == null)
			{
				IToken token = _tokens[num];
				if (token.Type != -1)
				{
					stringBuilder.Append(token.Text);
				}
				num++;
			}
			else
			{
				num = value2.Execute(stringBuilder);
			}
		}
		if (end == _tokens.Count - 1)
		{
			foreach (RewriteOperation value3 in dictionary.Values)
			{
				if (value3.index >= _tokens.Count - 1)
				{
					stringBuilder.Append(value3.text);
				}
			}
		}
		return stringBuilder.ToString();
	}

	protected virtual IDictionary<int, RewriteOperation> ReduceToSingleOperationPerIndex(IList<RewriteOperation> rewrites)
	{
		for (int i = 0; i < rewrites.Count; i++)
		{
			RewriteOperation rewriteOperation = rewrites[i];
			if (rewriteOperation == null || !(rewriteOperation is ReplaceOp))
			{
				continue;
			}
			ReplaceOp replaceOp = (ReplaceOp)rewrites[i];
			IList<RewriteOperation> kindOfOps = GetKindOfOps(rewrites, typeof(InsertBeforeOp), i);
			for (int j = 0; j < kindOfOps.Count; j++)
			{
				InsertBeforeOp insertBeforeOp = (InsertBeforeOp)kindOfOps[j];
				if (insertBeforeOp.index == replaceOp.index)
				{
					rewrites[insertBeforeOp.instructionIndex] = null;
					replaceOp.text = insertBeforeOp.text.ToString() + ((replaceOp.text != null) ? replaceOp.text.ToString() : string.Empty);
				}
				else if (insertBeforeOp.index > replaceOp.index && insertBeforeOp.index <= replaceOp.lastIndex)
				{
					rewrites[insertBeforeOp.instructionIndex] = null;
				}
			}
			IList<RewriteOperation> kindOfOps2 = GetKindOfOps(rewrites, typeof(ReplaceOp), i);
			for (int k = 0; k < kindOfOps2.Count; k++)
			{
				ReplaceOp replaceOp2 = (ReplaceOp)kindOfOps2[k];
				if (replaceOp2.index >= replaceOp.index && replaceOp2.lastIndex <= replaceOp.lastIndex)
				{
					rewrites[replaceOp2.instructionIndex] = null;
					continue;
				}
				bool flag = replaceOp2.lastIndex < replaceOp.index || replaceOp2.index > replaceOp.lastIndex;
				bool flag2 = replaceOp2.index == replaceOp.index && replaceOp2.lastIndex == replaceOp.lastIndex;
				if (replaceOp2.text == null && replaceOp.text == null && !flag)
				{
					rewrites[replaceOp2.instructionIndex] = null;
					replaceOp.index = Math.Min(replaceOp2.index, replaceOp.index);
					replaceOp.lastIndex = Math.Max(replaceOp2.lastIndex, replaceOp.lastIndex);
					Console.WriteLine("new rop " + replaceOp);
				}
				else if (!flag && !flag2)
				{
					throw new ArgumentException(string.Concat("replace op boundaries of ", replaceOp, " overlap with previous ", replaceOp2));
				}
			}
		}
		for (int l = 0; l < rewrites.Count; l++)
		{
			RewriteOperation rewriteOperation2 = rewrites[l];
			if (rewriteOperation2 == null || !(rewriteOperation2 is InsertBeforeOp))
			{
				continue;
			}
			InsertBeforeOp insertBeforeOp2 = (InsertBeforeOp)rewrites[l];
			IList<RewriteOperation> kindOfOps3 = GetKindOfOps(rewrites, typeof(InsertBeforeOp), l);
			for (int m = 0; m < kindOfOps3.Count; m++)
			{
				InsertBeforeOp insertBeforeOp3 = (InsertBeforeOp)kindOfOps3[m];
				if (insertBeforeOp3.index == insertBeforeOp2.index)
				{
					insertBeforeOp2.text = CatOpText(insertBeforeOp2.text, insertBeforeOp3.text);
					rewrites[insertBeforeOp3.instructionIndex] = null;
				}
			}
			IList<RewriteOperation> kindOfOps4 = GetKindOfOps(rewrites, typeof(ReplaceOp), l);
			for (int n = 0; n < kindOfOps4.Count; n++)
			{
				ReplaceOp replaceOp3 = (ReplaceOp)kindOfOps4[n];
				if (insertBeforeOp2.index == replaceOp3.index)
				{
					replaceOp3.text = CatOpText(insertBeforeOp2.text, replaceOp3.text);
					rewrites[l] = null;
				}
				else if (insertBeforeOp2.index >= replaceOp3.index && insertBeforeOp2.index <= replaceOp3.lastIndex)
				{
					throw new ArgumentException(string.Concat("insert op ", insertBeforeOp2, " within boundaries of previous ", replaceOp3));
				}
			}
		}
		IDictionary<int, RewriteOperation> dictionary = new Dictionary<int, RewriteOperation>();
		for (int num = 0; num < rewrites.Count; num++)
		{
			RewriteOperation rewriteOperation3 = rewrites[num];
			if (rewriteOperation3 != null)
			{
				if (dictionary.TryGetValue(rewriteOperation3.index, out var value) && value != null)
				{
					throw new Exception("should only be one op per index");
				}
				dictionary[rewriteOperation3.index] = rewriteOperation3;
			}
		}
		return dictionary;
	}

	protected virtual string CatOpText(object a, object b)
	{
		return string.Concat(a, b);
	}

	protected virtual IList<RewriteOperation> GetKindOfOps(IList<RewriteOperation> rewrites, Type kind)
	{
		return GetKindOfOps(rewrites, kind, rewrites.Count);
	}

	protected virtual IList<RewriteOperation> GetKindOfOps(IList<RewriteOperation> rewrites, Type kind, int before)
	{
		IList<RewriteOperation> list = new List<RewriteOperation>();
		for (int i = 0; i < before && i < rewrites.Count; i++)
		{
			RewriteOperation rewriteOperation = rewrites[i];
			if (rewriteOperation != null && (object)rewriteOperation.GetType() == kind)
			{
				list.Add(rewriteOperation);
			}
		}
		return list;
	}

	public virtual string ToDebugString()
	{
		return ToDebugString(0, Count - 1);
	}

	public virtual string ToDebugString(int start, int end)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = start; i >= 0 && i <= end && i < _tokens.Count; i++)
		{
			stringBuilder.Append(Get(i));
		}
		return stringBuilder.ToString();
	}
}
