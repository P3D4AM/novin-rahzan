using System;
using System.ComponentModel;
using Microsoft.IdentityModel.Logging;

namespace Microsoft.IdentityModel.Tokens;

public abstract class TokenHandler
{
	private int _defaultTokenLifetimeInMinutes = DefaultTokenLifetimeInMinutes;

	private int _maximumTokenSizeInBytes = 256000;

	public static readonly int DefaultTokenLifetimeInMinutes = 60;

	public virtual int MaximumTokenSizeInBytes
	{
		get
		{
			return _maximumTokenSizeInBytes;
		}
		set
		{
			if (value < 1)
			{
				throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException("value", LogHelper.FormatInvariant("IDX10101: MaximumTokenSizeInBytes must be greater than zero. value: '{0}'", value)));
			}
			_maximumTokenSizeInBytes = value;
		}
	}

	[DefaultValue(true)]
	public bool SetDefaultTimesOnTokenCreation { get; set; } = true;


	public int TokenLifetimeInMinutes
	{
		get
		{
			return _defaultTokenLifetimeInMinutes;
		}
		set
		{
			if (value < 1)
			{
				throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException("value", LogHelper.FormatInvariant("IDX10104: TokenLifetimeInMinutes must be greater than zero. value: '{0}'", value)));
			}
			_defaultTokenLifetimeInMinutes = value;
		}
	}
}
