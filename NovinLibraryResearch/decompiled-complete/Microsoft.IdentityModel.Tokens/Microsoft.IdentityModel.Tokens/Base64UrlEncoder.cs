using System;
using System.Text;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens;

public static class Base64UrlEncoder
{
	private static char base64PadCharacter = '=';

	private static string doubleBase64PadCharacter = "==";

	private static char base64Character62 = '+';

	private static char base64Character63 = '/';

	private static char base64UrlCharacter62 = '-';

	private static char _base64UrlCharacter63 = '_';

	public static string Encode(string arg)
	{
		if (arg == null)
		{
			throw LogHelper.LogArgumentNullException("arg");
		}
		return Encode(Encoding.UTF8.GetBytes(arg));
	}

	public static string Encode(byte[] inArray, int offset, int length)
	{
		if (inArray == null)
		{
			throw LogHelper.LogArgumentNullException("inArray");
		}
		return Convert.ToBase64String(inArray, offset, length).Split(new char[1] { base64PadCharacter })[0].Replace(base64Character62, base64UrlCharacter62).Replace(base64Character63, _base64UrlCharacter63);
	}

	public static string Encode(byte[] inArray)
	{
		if (inArray == null)
		{
			throw LogHelper.LogArgumentNullException("inArray");
		}
		return Convert.ToBase64String(inArray, 0, inArray.Length).Split(new char[1] { base64PadCharacter })[0].Replace(base64Character62, base64UrlCharacter62).Replace(base64Character63, _base64UrlCharacter63);
	}

	public static byte[] DecodeBytes(string str)
	{
		if (str == null)
		{
			throw LogHelper.LogExceptionMessage(new ArgumentNullException("str"));
		}
		str = str.Replace(base64UrlCharacter62, base64Character62);
		str = str.Replace(_base64UrlCharacter63, base64Character63);
		switch (str.Length % 4)
		{
		case 2:
			str += doubleBase64PadCharacter;
			break;
		case 3:
			str += base64PadCharacter;
			break;
		default:
			throw LogHelper.LogExceptionMessage(new FormatException(LogHelper.FormatInvariant("IDX10400: Unable to decode: '{0}' as Base64url encoded string.", str)));
		case 0:
			break;
		}
		return Convert.FromBase64String(str);
	}

	public static string Decode(string arg)
	{
		return Encoding.UTF8.GetString(DecodeBytes(arg));
	}
}
