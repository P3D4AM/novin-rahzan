using System;
using System.Collections.Generic;
using Microsoft.Identity.Client.PlatformsCommon.Interfaces;
using Microsoft.Identity.Client.TelemetryCore.Internal;

namespace Microsoft.Identity.Client.TelemetryCore;

internal class TelemetryClient : ITelemetryClient, IDisposable
{
	private readonly IErrorStore _errorStore;

	private readonly IUploader _uploader;

	private readonly IActionStore _actionStore;

	private readonly IScenarioStore _scenarioStore;

	private readonly ContextStore _contextStore;

	private readonly bool _isScenarioUploadDisabled;

	private readonly object _lockObject = new object();

	private bool _disposedValue;

	public IMatsTelemetryManager TelemetryManager { get; }

	public static ITelemetryClient CreateMats(IApplicationConfiguration applicationConfiguration, IPlatformProxy platformProxy, ITelemetryConfig telemetryConfig)
	{
		string devicePlatformTelemetryId = platformProxy.GetDevicePlatformTelemetryId();
		if (!IsDeviceEnabled(telemetryConfig.AudienceType, devicePlatformTelemetryId))
		{
			return null;
		}
		string deviceNetworkState = platformProxy.GetDeviceNetworkState();
		int matsOsPlatformCode = platformProxy.GetMatsOsPlatformCode();
		IEventFilter eventFilter = new EventFilter(enableAggregation: true);
		ErrorStore errorStore = new ErrorStore();
		ScenarioStore scenarioStore = new ScenarioStore(900000, errorStore);
		HashSet<string> telemetryAllowedScopes = new HashSet<string>();
		ActionStore actionStore = new ActionStore(600000, 30000, errorStore, eventFilter, telemetryAllowedScopes);
		ContextStore contextStore = ContextStore.CreateContextStore(telemetryConfig.AudienceType, applicationConfiguration.ClientName, applicationConfiguration.ClientVersion, devicePlatformTelemetryId, deviceNetworkState, telemetryConfig.SessionId, matsOsPlatformCode);
		IUploader uploader = new TelemetryUploader(telemetryConfig.DispatchAction, platformProxy, applicationConfiguration.ClientName);
		bool isScenarioUploadDisabled = true;
		return new TelemetryClient(applicationConfiguration, platformProxy, errorStore, uploader, actionStore, scenarioStore, contextStore, isScenarioUploadDisabled);
	}

	private TelemetryClient(IApplicationConfiguration applicationConfiguration, IPlatformProxy platformProxy, IErrorStore errorStore, IUploader uploader, IActionStore actionStore, IScenarioStore scenarioStore, ContextStore contextStore, bool isScenarioUploadDisabled)
	{
		TelemetryManager = new TelemetryManager(applicationConfiguration, platformProxy, ProcessTelemetryCallback);
		_errorStore = errorStore;
		_uploader = uploader;
		_actionStore = actionStore;
		_scenarioStore = scenarioStore;
		_contextStore = contextStore;
		_isScenarioUploadDisabled = isScenarioUploadDisabled;
	}

	private static bool IsDeviceEnabled(TelemetryAudienceType audienceType, string dpti)
	{
		if (audienceType == TelemetryAudienceType.PreProduction)
		{
			return true;
		}
		return SampleUtils.ShouldEnableDevice(dpti);
	}

	public MatsScenario CreateScenario()
	{
		MatsScenario result = _scenarioStore.CreateScenario();
		_uploader.Upload(GetEventsForUpload());
		return result;
	}

	public MatsAction StartAction(MatsScenario scenario, string correlationId)
	{
		return StartActionWithScopes(scenario, correlationId, null);
	}

	public MatsAction StartActionWithScopes(MatsScenario scenario, string correlationId, IEnumerable<string> scopes)
	{
		return _actionStore.StartMsalAction(scenario, correlationId, scopes ?? new List<string>());
	}

	public void EndAction(MatsAction action, AuthenticationResult authenticationResult)
	{
		AuthOutcome outcome = AuthOutcome.Succeeded;
		ErrorSource errorSource = ErrorSource.Service;
		string empty = string.Empty;
		string empty2 = string.Empty;
		EndAction(action, outcome, errorSource, empty, empty2);
	}

	public void EndAction(MatsAction action, AuthOutcome outcome, ErrorSource errorSource, string error, string errorDescription)
	{
		TelemetryManager.Flush(action.CorrelationId);
		_actionStore.EndMsalAction(action, outcome, errorSource, error, errorDescription);
		_uploader.Upload(GetEventsForUpload());
	}

	public void EndAction(MatsAction action, Exception ex)
	{
		AuthOutcome outcome = AuthOutcome.Failed;
		ErrorSource errorSource = ErrorSource.AuthSdk;
		string error = ex.GetType().Name;
		string message = ex.Message;
		if (!(ex is MsalUiRequiredException ex2))
		{
			if (!(ex is MsalServiceException ex3))
			{
				if (!(ex is MsalClientException ex4))
				{
					if (ex is MsalException ex5)
					{
						errorSource = ErrorSource.AuthSdk;
						error = ex5.ErrorCode;
					}
					else
					{
						errorSource = ErrorSource.AuthSdk;
					}
				}
				else
				{
					errorSource = ErrorSource.Client;
					error = ex4.ErrorCode;
				}
			}
			else
			{
				errorSource = ErrorSource.Service;
				error = ex3.ErrorCode;
			}
		}
		else
		{
			errorSource = ErrorSource.Service;
			error = ex2.ErrorCode;
		}
		EndAction(action, outcome, errorSource, error, message);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposedValue)
		{
			if (disposing)
			{
				UploadCompletedEvents();
				UploadErrorEvents();
			}
			_disposedValue = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
	}

	private void UploadErrorEvents()
	{
		List<PropertyBagContents> list = new List<PropertyBagContents>();
		IEnumerable<IPropertyBag> eventsForUpload = _errorStore.GetEventsForUpload();
		_contextStore.AddContext(eventsForUpload);
		foreach (IPropertyBag item in eventsForUpload)
		{
			list.Add(item.GetContents());
		}
		_uploader.Upload(list);
	}

	private void UploadCompletedEvents()
	{
		_uploader.Upload(GetEventsForUpload());
	}

	private IEnumerable<PropertyBagContents> GetEventsForUpload()
	{
		List<PropertyBagContents> list = new List<PropertyBagContents>();
		lock (_lockObject)
		{
			IEnumerable<IPropertyBag> eventsForUpload = _actionStore.GetEventsForUpload();
			if (_isScenarioUploadDisabled)
			{
				_contextStore.AddContext(eventsForUpload);
			}
			foreach (IPropertyBag item in eventsForUpload)
			{
				PropertyBagContents contents = item.GetContents();
				if (contents.StringProperties.TryGetValue("scenarioid", out var value))
				{
					_scenarioStore.NotifyActionCompleted(value);
					list.Add(contents);
				}
				else
				{
					ReportError("Trying to upload an Action with no Scenario Id", ErrorType.Action, ErrorSeverity.LibraryError);
				}
			}
			if (_isScenarioUploadDisabled)
			{
				_scenarioStore.ClearCompletedScenarios();
			}
			else
			{
				IEnumerable<IPropertyBag> eventsForUpload2 = _scenarioStore.GetEventsForUpload();
				_contextStore.AddContext(eventsForUpload2);
				foreach (IPropertyBag item2 in eventsForUpload2)
				{
					list.Add(item2.GetContents());
				}
			}
		}
		return list;
	}

	private void ReportError(string errorMessage, ErrorType errorType, ErrorSeverity errorSeverity)
	{
		_errorStore.ReportError(errorMessage, errorType, errorSeverity);
	}

	public void ProcessTelemetryCallback(List<Dictionary<string, string>> events)
	{
		foreach (Dictionary<string, string> @event in events)
		{
			_actionStore.ProcessMsalTelemetryBlob(@event);
		}
	}
}
