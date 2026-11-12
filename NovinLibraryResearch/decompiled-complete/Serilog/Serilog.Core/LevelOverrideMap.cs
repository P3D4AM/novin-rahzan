using System.Collections.Generic;
using System.Linq;
using Serilog.Events;

namespace Serilog.Core;

internal class LevelOverrideMap
{
	private readonly struct LevelOverride
	{
		public string Context { get; }

		public LoggingLevelSwitch LevelSwitch { get; }

		public LevelOverride(string context, LoggingLevelSwitch levelSwitch)
		{
			Context = context;
			LevelSwitch = levelSwitch;
		}
	}

	private readonly LogEventLevel _defaultMinimumLevel;

	private readonly LoggingLevelSwitch? _defaultLevelSwitch;

	private readonly LevelOverride[] _overrides;

	public LevelOverrideMap(IDictionary<string, LoggingLevelSwitch> overrides, LogEventLevel defaultMinimumLevel, LoggingLevelSwitch? defaultLevelSwitch)
	{
		Guard.AgainstNull(overrides, "overrides");
		_defaultLevelSwitch = defaultLevelSwitch;
		_defaultMinimumLevel = ((defaultLevelSwitch == null) ? defaultMinimumLevel : LogEventLevel.Verbose);
		_overrides = (from o in overrides
			orderby o.Key descending
			select new LevelOverride(o.Key, o.Value)).ToArray();
	}

	public void GetEffectiveLevel(string context, out LogEventLevel minimumLevel, out LoggingLevelSwitch? levelSwitch)
	{
		LevelOverride[] overrides = _overrides;
		for (int i = 0; i < overrides.Length; i++)
		{
			LevelOverride levelOverride = overrides[i];
			if (context.StartsWith(levelOverride.Context) && (context.Length == levelOverride.Context.Length || context[levelOverride.Context.Length] == '.'))
			{
				minimumLevel = LogEventLevel.Verbose;
				levelSwitch = levelOverride.LevelSwitch;
				return;
			}
		}
		minimumLevel = _defaultMinimumLevel;
		levelSwitch = _defaultLevelSwitch;
	}
}
