using System;

namespace Stimulsoft.Controls.Win.Editors.Primitives;

internal class StartsWithPredicate
{
	private string _Prefix;

	private int _MaxMatches;

	private int _MatchCount;

	public string Prefix
	{
		get
		{
			return _Prefix;
		}
		set
		{
			_Prefix = value;
		}
	}

	public int MaxMatches
	{
		get
		{
			return _MaxMatches;
		}
		set
		{
			_MaxMatches = value;
		}
	}

	public Predicate<string> Match => IsMatch;

	public Predicate<string> MatchTop => IsMatchTop;

	public StartsWithPredicate(string prefix)
	{
		_Prefix = prefix;
	}

	public StartsWithPredicate(string prefix, int maxMatches)
	{
		_Prefix = prefix;
		_MaxMatches = maxMatches;
	}

	public void ResetMatchCount()
	{
		_MatchCount = 0;
	}

	private bool IsMatch(string s)
	{
		return s.ToLower().StartsWith(_Prefix);
	}

	private bool IsMatchTop(string s)
	{
		if (_MatchCount > _MaxMatches)
		{
			return false;
		}
		bool flag = s.ToLower().StartsWith(_Prefix);
		if (flag && _MaxMatches > 0)
		{
			_MatchCount++;
		}
		return flag;
	}
}
