using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens;

public static class UniqueId
{
	private const int RandomSaltSize = 16;

	private const string NcNamePrefix = "_";

	private const string UuidUriPrefix = "urn:uuid:";

	private static readonly string reusableUuid = GetRandomUuid();

	private static readonly string optimizedNcNamePrefix = "_" + reusableUuid + "-";

	public static string CreateUniqueId()
	{
		return optimizedNcNamePrefix + GetNextId();
	}

	public static string CreateUniqueId(string prefix)
	{
		if (string.IsNullOrEmpty(prefix))
		{
			throw LogHelper.LogArgumentNullException("prefix");
		}
		return prefix + reusableUuid + "-" + GetNextId();
	}

	public static string CreateRandomId()
	{
		return "_" + GetRandomUuid();
	}

	public static string CreateRandomId(string prefix)
	{
		if (string.IsNullOrEmpty(prefix))
		{
			throw LogHelper.LogArgumentNullException("prefix");
		}
		return prefix + GetRandomUuid();
	}

	public static Uri CreateRandomUri()
	{
		return new Uri("urn:uuid:" + GetRandomUuid());
	}

	private static string GetNextId()
	{
		using RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
		byte[] array = new byte[16];
		randomNumberGenerator.GetBytes(array);
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0:X2}", array[i]);
		}
		return stringBuilder.ToString();
	}

	private static string GetRandomUuid()
	{
		return Guid.NewGuid().ToString("D");
	}
}
