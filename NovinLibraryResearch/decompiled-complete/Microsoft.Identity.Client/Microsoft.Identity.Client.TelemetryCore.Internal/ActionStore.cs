using System;
using System.Collections.Generic;

namespace Microsoft.Identity.Client.TelemetryCore.Internal;

internal class ActionStore : IActionStore
{
	private readonly Dictionary<string, ActionPropertyBag> _actionIdToPropertyBag = new Dictionary<string, ActionPropertyBag>();

	private readonly object _lockActionIdToPropertyBag = new object();

	private readonly IErrorStore _errorStore;

	private readonly IEventFilter _eventFilter;

	private readonly int _maxActionDurationMillis;

	private readonly int _maxAggregationDurationMillis;

	private readonly HashSet<string> _telemetryAllowedScopes = new HashSet<string>();

	public ActionStore(int maxActionDurationMillis, int maxAggregationDurationMillis, IErrorStore errorStore, IEventFilter eventFilter, HashSet<string> telemetryAllowedScopes)
	{
		_maxActionDurationMillis = maxActionDurationMillis;
		_maxAggregationDurationMillis = maxAggregationDurationMillis;
		_errorStore = errorStore;
		_eventFilter = eventFilter;
		if (telemetryAllowedScopes == null)
		{
			return;
		}
		foreach (string telemetryAllowedScope in telemetryAllowedScopes)
		{
			_telemetryAllowedScopes.Add(telemetryAllowedScope);
		}
	}

	public void EndMsalAction(MatsAction action, AuthOutcome outcome, ErrorSource errorSource, string error, string errorDescription)
	{
		EndGenericAction(action.ActionId, MatsConverter.AsString(outcome), errorSource, error, errorDescription, string.Empty);
	}

	public IEnumerable<IPropertyBag> GetEventsForUpload()
	{
		lock (_lockActionIdToPropertyBag)
		{
			List<string> list = new List<string>();
			List<PropertyBag> list2 = new List<PropertyBag>();
			foreach (KeyValuePair<string, ActionPropertyBag> item in _actionIdToPropertyBag)
			{
				ActionPropertyBag value = item.Value;
				if (!value.ReadyForUpload)
				{
					if (!value.GetContents().Int64Properties.TryGetValue("starttime", out var value2))
					{
						_errorStore.ReportError("No start time on action", ErrorType.Action, ErrorSeverity.LibraryError);
						continue;
					}
					long millisecondsSinceEpoch = DateTimeUtils.GetMillisecondsSinceEpoch(DateTime.UtcNow);
					long num = millisecondsSinceEpoch - value2;
					if (value.IsAggregable && num > _maxActionDurationMillis)
					{
						value.ReadyForUpload = true;
					}
					else
					{
						if (num <= _maxAggregationDurationMillis)
						{
							continue;
						}
						value.Add("endtime", millisecondsSinceEpoch);
						value.Add("authoutcome", MatsConverter.AsString(AuthOutcome.Incomplete));
						value.ReadyForUpload = true;
					}
				}
				list2.Add(item.Value);
				list.Add(item.Key);
			}
			foreach (string item2 in list)
			{
				_actionIdToPropertyBag.Remove(item2);
			}
			return list2;
		}
	}

	public void ProcessMsalTelemetryBlob(IDictionary<string, string> blob)
	{
		if (!blob.TryGetValue("Microsoft.MSAL.correlation_id", out var value))
		{
			_errorStore.ReportError("No correlation ID found in telemetry blob", ErrorType.Action, ErrorSeverity.Warning);
			return;
		}
		List<ActionPropertyBag> msalPropertyBagsForCorrelationId = GetMsalPropertyBagsForCorrelationId(value);
		switch (msalPropertyBagsForCorrelationId.Count)
		{
		case 0:
			_errorStore.ReportError("No MSAL actions matched correlation ID", ErrorType.Action, ErrorSeverity.Warning);
			break;
		default:
			_errorStore.ReportError("Multiple MSAL actions matched correlation ID", ErrorType.Action, ErrorSeverity.Warning);
			break;
		case 1:
		{
			HashSet<string> msalAggregatedProperties = GetMsalAggregatedProperties();
			ActionPropertyBag actionPropertyBag = msalPropertyBagsForCorrelationId[0];
			{
				foreach (KeyValuePair<string, string> item in blob)
				{
					if (string.Compare(item.Key, "Microsoft.MSAL.correlation_id", StringComparison.OrdinalIgnoreCase) != 0)
					{
						string text = NormalizeValidPropertyName(item.Key);
						if (msalAggregatedProperties.Contains(text) && int.TryParse(item.Value, out var result))
						{
							actionPropertyBag.Add(text + "_max", result);
							actionPropertyBag.Add(text + "_min", result);
							actionPropertyBag.Add(text + "_sum", result);
						}
						actionPropertyBag.Add(item.Key, item.Value);
					}
				}
				break;
			}
		}
		}
	}

	private List<ActionPropertyBag> GetMsalPropertyBagsForCorrelationId(string correlationId)
	{
		List<ActionPropertyBag> list = new List<ActionPropertyBag>();
		lock (_lockActionIdToPropertyBag)
		{
			foreach (ActionPropertyBag value in _actionIdToPropertyBag.Values)
			{
				PropertyBagContents contents = value.GetContents();
				string strA = contents.StringProperties["actiontype"];
				string input = contents.StringProperties["correlationid"];
				input = input.TrimCurlyBraces();
				string strB = correlationId.TrimCurlyBraces();
				if (string.Compare(strA, MatsConverter.AsString(ActionType.Msal), StringComparison.OrdinalIgnoreCase) == 0 && string.Compare(input, strB, StringComparison.OrdinalIgnoreCase) == 0)
				{
					list.Add(value);
				}
			}
			return list;
		}
	}

	private bool IsValidPropertyName(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return false;
		}
		if (!MatsUtils.ContainsCharsThatAreEitherAlphaNumericOrDotsOrUnderscore(name))
		{
			return false;
		}
		return true;
	}

	private string NormalizeValidPropertyName(string name)
	{
		if (!IsValidPropertyName(name))
		{
			return string.Empty;
		}
		return name.Replace('.', '_');
	}

	private HashSet<string> GetMsalAggregatedProperties()
	{
		return new HashSet<string> { "Microsoft_MSAL_cache_event_count", "Microsoft_MSAL_http_event_count", "Microsoft_MSAL_response_time" };
	}

	public MatsAction StartMsalAction(MatsScenario scenario, string correlationId, IEnumerable<string> scopes)
	{
		ActionArtifacts actionArtifacts = CreateGenericAction(scenario, correlationId, ActionType.Msal);
		actionArtifacts.PropertyBag.Add("identityservice", MatsConverter.AsString(IdentityService.AAD));
		SetScopesProperty(actionArtifacts.PropertyBag, scopes);
		return actionArtifacts.Action;
	}

	private void SetScopesProperty(ActionPropertyBag propertyBag, IEnumerable<string> scopes)
	{
		foreach (string scope in scopes)
		{
			if (_telemetryAllowedScopes.Contains(scope))
			{
				propertyBag.Add("scope", scope);
			}
			else if (!string.IsNullOrEmpty(scope))
			{
				propertyBag.Add("scope", "ScopeRedacted");
			}
		}
	}

	private ActionArtifacts CreateGenericAction(MatsScenario scenario, string correlationId, ActionType actionType)
	{
		string text = MatsId.Create();
		MatsAction action = new MatsAction(text, scenario, correlationId);
		string value = correlationId.TrimCurlyBraces();
		ActionPropertyBag actionPropertyBag = new ActionPropertyBag(_errorStore);
		DateTime utcNow = DateTime.UtcNow;
		actionPropertyBag.Add("uploadid", MatsId.Create());
		actionPropertyBag.Add("actiontype", MatsConverter.AsString(actionType));
		actionPropertyBag.Add("scenarioid", scenario?.ScenarioId);
		actionPropertyBag.Add("correlationid", value);
		actionPropertyBag.Add("starttime", DateTimeUtils.GetMillisecondsSinceEpoch(utcNow));
		lock (_lockActionIdToPropertyBag)
		{
			_actionIdToPropertyBag[text] = actionPropertyBag;
		}
		return new ActionArtifacts(action, actionPropertyBag);
	}

	private void EndGenericAction(string actionId, string outcome, ErrorSource errorSource, string error, string errorDescription, string accountCid)
	{
		if (string.IsNullOrEmpty(actionId))
		{
			_errorStore.ReportError("Tried to end an action with an empty actionId", ErrorType.Action, ErrorSeverity.Warning);
			return;
		}
		ActionPropertyBag actionPropertyBagFromId = GetActionPropertyBagFromId(actionId);
		if (actionPropertyBagFromId == null)
		{
			_errorStore.ReportError("Trying to end an action that doesn't exist or was already uploaded", ErrorType.Action, ErrorSeverity.Warning);
		}
		else if (!actionPropertyBagFromId.ReadyForUpload)
		{
			int value = 1;
			DateTime utcNow = DateTime.UtcNow;
			actionPropertyBagFromId.Add("authoutcome", outcome);
			actionPropertyBagFromId.Add("errorsource", MatsConverter.AsString(errorSource));
			actionPropertyBagFromId.Add("error", error);
			actionPropertyBagFromId.Add("errordescription", errorDescription);
			actionPropertyBagFromId.Add("endtime", DateTimeUtils.GetMillisecondsSinceEpoch(utcNow));
			actionPropertyBagFromId.Add("count", value);
			PopulateDuration(actionPropertyBagFromId);
			PropertyBagContents contents = actionPropertyBagFromId.GetContents();
			if (_eventFilter.ShouldAggregateAction(contents))
			{
				EndAggregatedAction(actionId, actionPropertyBagFromId);
			}
			else
			{
				actionPropertyBagFromId.ReadyForUpload = true;
			}
		}
	}

	private void EndAggregatedAction(string actionId, ActionPropertyBag propertyBag)
	{
		lock (_lockActionIdToPropertyBag)
		{
			propertyBag.IsAggregable = true;
			bool flag = false;
			foreach (ActionPropertyBag value in _actionIdToPropertyBag.Values)
			{
				if (ActionComparer.IsEquivalentClass(value, propertyBag))
				{
					ActionAggregator.AggregateActions(value, propertyBag);
					flag = true;
					break;
				}
			}
			if (flag)
			{
				_actionIdToPropertyBag.Remove(actionId);
			}
		}
	}

	private void PopulateDuration(ActionPropertyBag propertyBag)
	{
		PropertyBagContents contents = propertyBag.GetContents();
		if (!contents.Int64Properties.TryGetValue("starttime", out var value))
		{
			_errorStore.ReportError("Could not retrieve start time for duration calculation.", ErrorType.Action, ErrorSeverity.LibraryError);
			return;
		}
		if (!contents.Int64Properties.TryGetValue("endtime", out var value2))
		{
			_errorStore.ReportError("Could not retrieve end time for duration calculation.", ErrorType.Action, ErrorSeverity.LibraryError);
			return;
		}
		long value3 = value2 - value;
		propertyBag.Add("duration_sum", value3);
		propertyBag.Add("duration_max", value3);
		propertyBag.Add("duration_min", value3);
	}

	private ActionPropertyBag GetActionPropertyBagFromId(string actionId)
	{
		lock (_lockActionIdToPropertyBag)
		{
			if (_actionIdToPropertyBag.TryGetValue(actionId, out var value))
			{
				return value;
			}
			return null;
		}
	}
}
