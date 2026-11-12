using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Microsoft.EntityFrameworkCore.Migrations.Internal;

public class MigrationsIdGenerator : IMigrationsIdGenerator
{
	private const string Format = "yyyyMMddHHmmss";

	private DateTime _lastTimestamp = DateTime.MinValue;

	private readonly object _lock = new object();

	public virtual string GetName(string id)
	{
		return id.Substring("yyyyMMddHHmmss".Length + 1);
	}

	public virtual bool IsValidId(string value)
	{
		return Regex.IsMatch(value, string.Format(CultureInfo.InvariantCulture, "[0-9]{{{0}}}_.+", "yyyyMMddHHmmss".Length), RegexOptions.None, TimeSpan.FromMilliseconds(1000.0));
	}

	public virtual string GenerateId(string name)
	{
		DateTime utcNow = DateTime.UtcNow;
		DateTime dateTime = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, utcNow.Hour, utcNow.Minute, utcNow.Second);
		lock (_lock)
		{
			if (dateTime <= _lastTimestamp)
			{
				dateTime = _lastTimestamp.AddSeconds(1.0);
			}
			_lastTimestamp = dateTime;
		}
		return dateTime.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture) + "_" + name;
	}
}
