using System;
using System.Configuration;

namespace Microsoft.Data.SqlClient;

internal class SqlConfigurableRetryConnectionSection : ConfigurationSection, ISqlConfigurableRetryConnectionSection
{
	public const string Name = "SqlConfigurableRetryLogicConnection";

	[ConfigurationProperty("retryLogicType")]
	public string RetryLogicType
	{
		get
		{
			return ((ConfigurationElement)this)["retryLogicType"] as string;
		}
		set
		{
			((ConfigurationElement)this)["retryLogicType"] = value;
		}
	}

	[ConfigurationProperty("retryMethod", IsRequired = true)]
	public string RetryMethod
	{
		get
		{
			return ((ConfigurationElement)this)["retryMethod"] as string;
		}
		set
		{
			((ConfigurationElement)this)["retryMethod"] = value;
		}
	}

	[ConfigurationProperty("numberOfTries", IsRequired = true, DefaultValue = 1)]
	[IntegerValidator(MinValue = 1, MaxValue = 60, ExcludeRange = false)]
	public int NumberOfTries
	{
		get
		{
			return (int)((ConfigurationElement)this)["numberOfTries"];
		}
		set
		{
			((ConfigurationElement)this)["numberOfTries"] = value;
		}
	}

	[ConfigurationProperty("deltaTime", IsRequired = true)]
	[TimeSpanValidator(MinValueString = "00:00:00", MaxValueString = "00:02:00", ExcludeRange = false)]
	public TimeSpan DeltaTime
	{
		get
		{
			return (TimeSpan)((ConfigurationElement)this)["deltaTime"];
		}
		set
		{
			((ConfigurationElement)this)["deltaTime"] = value;
		}
	}

	[ConfigurationProperty("minTime", IsRequired = false, DefaultValue = "00:00:00")]
	[TimeSpanValidator(MinValueString = "00:00:00", MaxValueString = "00:02:00", ExcludeRange = false)]
	public TimeSpan MinTimeInterval
	{
		get
		{
			return (TimeSpan)((ConfigurationElement)this)["minTime"];
		}
		set
		{
			((ConfigurationElement)this)["minTime"] = value;
		}
	}

	[ConfigurationProperty("maxTime", IsRequired = false)]
	[TimeSpanValidator(MinValueString = "00:00:00", MaxValueString = "00:02:00", ExcludeRange = false)]
	public TimeSpan MaxTimeInterval
	{
		get
		{
			return (TimeSpan)((ConfigurationElement)this)["maxTime"];
		}
		set
		{
			((ConfigurationElement)this)["maxTime"] = value;
		}
	}

	[ConfigurationProperty("transientErrors", IsRequired = false)]
	[RegexStringValidator("^([ \\t]*(|-)\\d+(?:[ \\t]*,[ \\t]*(|-)\\d+)*[ \\t]*)*$")]
	public string TransientErrors
	{
		get
		{
			return ((ConfigurationElement)this)["transientErrors"] as string;
		}
		set
		{
			((ConfigurationElement)this)["transientErrors"] = value;
		}
	}
}
