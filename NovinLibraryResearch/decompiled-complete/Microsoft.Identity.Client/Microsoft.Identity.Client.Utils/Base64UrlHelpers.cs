using System;
using System.Globalization;
using System.Text;

namespace Microsoft.Identity.Client.Utils;

internal static class Base64UrlHelpers
{
	private const char Base64PadCharacter = '=';

	private const char Base64Character62 = '+';

	private const char Base64Character63 = '/';

	private const char Base64UrlCharacter62 = '-';

	private const char Base64UrlCharacter63 = '_';

	private static readonly Encoding TextEncoding = Encoding.UTF8;

	private static readonly string DoubleBase64PadCharacter = string.Format(CultureInfo.InvariantCulture, "{0}{0}", '=');

	public static string Encode(string arg)
	{
		if (arg == null)
		{
			return null;
		}
		return Encode(TextEncoding.GetBytes(arg));
	}

	public static string DecodeToString(string arg)
	{
		return CoreHelpers.CreateString(DecodeToBytes(arg));
	}

	public static byte[] DecodeToBytes(string arg)
	{
		string text = arg;
		text = text.Replace('-', '+');
		text = text.Replace('_', '/');
		switch (text.Length % 4)
		{
		case 2:
			text += DoubleBase64PadCharacter;
			break;
		case 3:
			text += "=";
			break;
		default:
			throw new ArgumentException("Illegal base64url string!", "arg");
		case 0:
			break;
		}
		return Convert.FromBase64String(text);
	}

	internal static string Encode(byte[] arg)
	{
		if (arg == null)
		{
			return null;
		}
		return Convert.ToBase64String(arg).Split(new char[1] { '=' })[0].Replace('+', '-').Replace('/', '_');
	}
}
