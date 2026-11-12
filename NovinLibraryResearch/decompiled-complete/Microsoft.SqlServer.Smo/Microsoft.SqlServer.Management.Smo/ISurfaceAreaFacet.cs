using System;
using System.ComponentModel;
using Microsoft.SqlServer.Management.Dmf;
using Microsoft.SqlServer.Management.Facets;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo;

[StateChangeEvent("AUDIT_SERVER_OPERATION_EVENT", "SERVER")]
[StateChangeEvent("CREATE_ENDPOINT", "SERVER")]
[StateChangeEvent("ALTER_ENDPOINT", "SERVER")]
[StateChangeEvent("SAC_ENDPOINT_CHANGE", "SERVER")]
[EvaluationMode(AutomatedPolicyEvaluationMode.CheckOnChanges | AutomatedPolicyEvaluationMode.CheckOnSchedule)]
[TypeConverter(typeof(LocalizableTypeConverter))]
[LocalizedPropertyResources("Microsoft.SqlServer.Management.Smo.FacetSR")]
[DisplayNameKey("ServerSurfaceAreaConfigurationName")]
[DisplayDescriptionKey("ServerSurfaceAreaConfigurationDesc")]
[CLSCompliant(false)]
public interface ISurfaceAreaFacet : IDmfFacet
{
	[DisplayNameKey("AdHocRemoteQueriesEnabledName")]
	[DisplayDescriptionKey("AdHocRemoteQueriesEnabledDesc")]
	bool AdHocRemoteQueriesEnabled { get; set; }

	[DisplayNameKey("DatabaseMailEnabledName")]
	[DisplayDescriptionKey("DatabaseMailEnabledDesc")]
	bool DatabaseMailEnabled { get; set; }

	[DisplayNameKey("ClrIntegrationEnabledName")]
	[DisplayDescriptionKey("ClrIntegrationEnabledDesc")]
	bool ClrIntegrationEnabled { get; set; }

	[DisplayNameKey("OleAutomationEnabledName")]
	[DisplayDescriptionKey("OleAutomationEnabledDesc")]
	bool OleAutomationEnabled { get; set; }

	[DisplayNameKey("RemoteDacEnabledName")]
	[DisplayDescriptionKey("RemoteDacEnabledDesc")]
	bool RemoteDacEnabled { get; set; }

	[DisplayNameKey("SqlMailEnabledName")]
	[DisplayDescriptionKey("SqlMailEnabledDesc")]
	bool SqlMailEnabled { get; set; }

	[DisplayNameKey("WebAssistantEnabledName")]
	[DisplayDescriptionKey("WebAssistantEnabledDesc")]
	bool WebAssistantEnabled { get; set; }

	[DisplayNameKey("XPCmdShellEnabledName")]
	[DisplayDescriptionKey("XPCmdShellEnabledDesc")]
	bool XPCmdShellEnabled { get; set; }

	[DisplayNameKey("ServiceBrokerEndpointActiveName")]
	[DisplayDescriptionKey("ServiceBrokerEndpointActiveDesc")]
	bool ServiceBrokerEndpointActive { get; set; }

	[DisplayNameKey("SoapEndpointsEnabledName")]
	[DisplayDescriptionKey("SoapEndpointsEnabledDesc")]
	bool SoapEndpointsEnabled { get; set; }
}
