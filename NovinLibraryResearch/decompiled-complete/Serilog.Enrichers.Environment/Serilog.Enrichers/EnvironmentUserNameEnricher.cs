using System;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Enrichers;

public class EnvironmentUserNameEnricher : CachedPropertyEnricher
{
	public const string EnvironmentUserNamePropertyName = "EnvironmentUserName";

	protected override LogEventProperty CreateProperty(ILogEventPropertyFactory propertyFactory)
	{
		string userDomainName = Environment.UserDomainName;
		string userName = Environment.UserName;
		string value = ((!string.IsNullOrWhiteSpace(userDomainName)) ? (userDomainName + "\\" + userName) : userName);
		return propertyFactory.CreateProperty("EnvironmentUserName", value);
	}
}
