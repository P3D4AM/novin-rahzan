using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Scaffolding.Internal;

public class CSharpUtilities : ICSharpUtilities
{
	private static readonly HashSet<string> _cSharpKeywords = new HashSet<string>
	{
		"abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked",
		"class", "const", "continue", "decimal", "default", "delegate", "do", "double", "else", "enum",
		"event", "explicit", "extern", "false", "finally", "fixed", "float", "for", "foreach", "goto",
		"if", "implicit", "in", "int", "interface", "internal", "is", "lock", "long", "namespace",
		"new", "null", "object", "operator", "out", "override", "params", "private", "protected", "public",
		"readonly", "ref", "return", "sbyte", "sealed", "short", "sizeof", "stackalloc", "static", "string",
		"struct", "switch", "this", "throw", "true", "try", "typeof", "uint", "ulong", "unchecked",
		"unsafe", "ushort", "using", "virtual", "void", "volatile", "while"
	};

	private static readonly Regex _invalidCharsRegex = new Regex("[^\\p{Ll}\\p{Lu}\\p{Lt}\\p{Lo}\\p{Nd}\\p{Nl}\\p{Mn}\\p{Mc}\\p{Cf}\\p{Pc}\\p{Lm}]", RegexOptions.None, TimeSpan.FromMilliseconds(1000.0));

	public virtual bool IsCSharpKeyword(string identifier)
	{
		return _cSharpKeywords.Contains(identifier);
	}

	public virtual string GenerateCSharpIdentifier(string identifier, ICollection<string> existingIdentifiers, Func<string, string> singularizePluralizer)
	{
		return GenerateCSharpIdentifier(identifier, existingIdentifiers, singularizePluralizer, Uniquifier);
	}

	public virtual string GenerateCSharpIdentifier(string identifier, ICollection<string> existingIdentifiers, Func<string, string> singularizePluralizer, Func<string, ICollection<string>, string> uniquifier)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(identifier, "identifier");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(uniquifier, "uniquifier");
		string text = ((identifier.Length > 1 && identifier[0] == '@') ? ("@" + _invalidCharsRegex.Replace(identifier.Substring(1), "_")) : _invalidCharsRegex.Replace(identifier, "_"));
		if (string.IsNullOrEmpty(text))
		{
			text = "_";
		}
		char c = text[0];
		if (!char.IsLetter(c) && c != '_' && c != '@')
		{
			text = "_" + text;
		}
		else if (IsCSharpKeyword(text))
		{
			text = "_" + text;
		}
		if (singularizePluralizer != null)
		{
			text = singularizePluralizer(text);
		}
		return uniquifier(text, existingIdentifiers);
	}

	public virtual string Uniquifier(string proposedIdentifier, ICollection<string> existingIdentifiers)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(proposedIdentifier, "proposedIdentifier");
		if (existingIdentifiers == null)
		{
			return proposedIdentifier;
		}
		string text = proposedIdentifier;
		int num = 1;
		while (existingIdentifiers.Contains(text))
		{
			text = proposedIdentifier + num;
			num++;
		}
		return text;
	}

	public virtual bool IsValidIdentifier(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return false;
		}
		if (!IsIdentifierStartCharacter(name[0]))
		{
			return false;
		}
		int length = name.Length;
		for (int i = 1; i < length; i++)
		{
			if (!IsIdentifierPartCharacter(name[i]))
			{
				return false;
			}
		}
		return true;
	}

	private static bool IsIdentifierStartCharacter(char ch)
	{
		if (ch < 'a')
		{
			if (ch >= 'A')
			{
				if (ch > 'Z')
				{
					return ch == '_';
				}
				return true;
			}
			return false;
		}
		if (ch <= 'z')
		{
			return true;
		}
		if (ch > '\u007f')
		{
			return IsLetterChar(CharUnicodeInfo.GetUnicodeCategory(ch));
		}
		return false;
	}

	private static bool IsIdentifierPartCharacter(char ch)
	{
		if (ch < 'a')
		{
			if (ch >= 'A')
			{
				if (ch > 'Z')
				{
					return ch == '_';
				}
				return true;
			}
			if (ch >= '0')
			{
				return ch <= '9';
			}
			return false;
		}
		if (ch <= 'z')
		{
			return true;
		}
		if (ch <= '\u007f')
		{
			return false;
		}
		UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(ch);
		if (IsLetterChar(unicodeCategory))
		{
			return true;
		}
		switch (unicodeCategory)
		{
		case UnicodeCategory.NonSpacingMark:
		case UnicodeCategory.SpacingCombiningMark:
		case UnicodeCategory.DecimalDigitNumber:
		case UnicodeCategory.Format:
		case UnicodeCategory.ConnectorPunctuation:
			return true;
		default:
			return false;
		}
	}

	private static bool IsLetterChar(UnicodeCategory cat)
	{
		if ((uint)cat <= 4u || cat == UnicodeCategory.LetterNumber)
		{
			return true;
		}
		return false;
	}
}
