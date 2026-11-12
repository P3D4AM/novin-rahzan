using System;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens;

public class CompressionProviderFactory
{
	private static CompressionProviderFactory _default;

	public static CompressionProviderFactory Default
	{
		get
		{
			return _default;
		}
		set
		{
			_default = value ?? throw LogHelper.LogArgumentNullException("Default");
		}
	}

	public ICompressionProvider CustomCompressionProvider { get; set; }

	static CompressionProviderFactory()
	{
		Default = new CompressionProviderFactory();
	}

	public CompressionProviderFactory()
	{
	}

	public CompressionProviderFactory(CompressionProviderFactory other)
	{
		if (other == null)
		{
			throw LogHelper.LogArgumentNullException("other");
		}
		CustomCompressionProvider = other.CustomCompressionProvider;
	}

	public virtual bool IsSupportedAlgorithm(string algorithm)
	{
		if (CustomCompressionProvider != null && CustomCompressionProvider.IsSupportedAlgorithm(algorithm))
		{
			return true;
		}
		return IsSupportedCompressionAlgorithm(algorithm);
	}

	private static bool IsSupportedCompressionAlgorithm(string algorithm)
	{
		return "DEF".Equals(algorithm, StringComparison.Ordinal);
	}

	public ICompressionProvider CreateCompressionProvider(string algorithm)
	{
		if (string.IsNullOrEmpty(algorithm))
		{
			throw LogHelper.LogArgumentNullException("algorithm");
		}
		if (CustomCompressionProvider != null && CustomCompressionProvider.IsSupportedAlgorithm(algorithm))
		{
			return CustomCompressionProvider;
		}
		if (algorithm.Equals("DEF", StringComparison.Ordinal))
		{
			return new DeflateCompressionProvider();
		}
		throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant("IDX10652: The algorithm '{0}' is not supported.", algorithm)));
	}
}
