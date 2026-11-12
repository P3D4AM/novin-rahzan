using System;
using System.Collections.Concurrent;

namespace Microsoft.Identity.Client.TelemetryCore.Internal;

internal class EventFilter : IEventFilter
{
	private readonly bool _enableAggregation;

	public EventFilter(bool enableAggregation)
	{
		_enableAggregation = enableAggregation;
	}

	public void SetShouldAggregate(bool shouldAggregate)
	{
		throw new NotImplementedException();
	}

	public bool ShouldAggregateAction(PropertyBagContents contents)
	{
		if (_enableAggregation)
		{
			if (IsSuccessfulAction(contents))
			{
				return IsSilentAction(contents);
			}
			return false;
		}
		return false;
	}

	private bool IsSuccessfulAction(PropertyBagContents contents)
	{
		if (IsOfActionType(ActionType.Msal, contents.StringProperties))
		{
			return IsMsalActionSuccessful(contents);
		}
		return HasActionOutcome(AuthOutcome.Succeeded, contents.StringProperties);
	}

	private bool IsMsalActionSuccessful(PropertyBagContents contents)
	{
		if (contents.StringProperties.TryGetValue("Microsoft_MSAL_is_successful", out var value))
		{
			if (value.Equals("Microsoft_MSAL_is_successful"))
			{
				return HasActionOutcome(AuthOutcome.Succeeded, contents.StringProperties);
			}
			return false;
		}
		return false;
	}

	public bool IsSilentAction(PropertyBagContents contents)
	{
		if (IsOfActionType(ActionType.Msal, contents.StringProperties))
		{
			return IsMsalActionSilent(contents);
		}
		if (contents.BoolProperties.TryGetValue("issilent", out var value))
		{
			return value;
		}
		return false;
	}

	private bool IsOfActionType(ActionType actionType, ConcurrentDictionary<string, string> properties)
	{
		if (properties.TryGetValue("actiontype", out var value))
		{
			return value.Equals(MatsConverter.AsString(actionType));
		}
		return false;
	}

	private bool HasActionOutcome(AuthOutcome outcome, ConcurrentDictionary<string, string> properties)
	{
		if (properties.TryGetValue("authoutcome", out var value))
		{
			return value.Equals(MatsConverter.AsString(outcome));
		}
		return false;
	}

	private bool IsMsalActionSilent(PropertyBagContents contents)
	{
		string value = string.Empty;
		contents.StringProperties.TryGetValue("", out value);
		string value2 = string.Empty;
		contents.StringProperties.TryGetValue("Microsoft_MSAL_ui_event_count", out value2);
		bool value3 = false;
		contents.BoolProperties.TryGetValue("blockingprompt", out value3);
		bool value4 = false;
		contents.BoolProperties.TryGetValue("askedforcreds", out value4);
		if (value == "false" || (string.IsNullOrEmpty(value) && value2 != "0" && !string.IsNullOrEmpty(value2)) || value3 || value4)
		{
			return false;
		}
		return true;
	}
}
