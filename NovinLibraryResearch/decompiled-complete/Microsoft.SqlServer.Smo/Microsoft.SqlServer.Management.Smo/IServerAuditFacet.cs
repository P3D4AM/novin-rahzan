using System;
using System.ComponentModel;
using Microsoft.SqlServer.Management.Dmf;
using Microsoft.SqlServer.Management.Facets;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo;

[EvaluationMode(AutomatedPolicyEvaluationMode.CheckOnSchedule)]
[CLSCompliant(false)]
[TypeConverter(typeof(LocalizableTypeConverter))]
[LocalizedPropertyResources("Microsoft.SqlServer.Management.Smo.FacetSR")]
[DisplayNameKey("ServerAuditName")]
[DisplayDescriptionKey("ServerAuditDesc")]
public interface IServerAuditFacet : IDmfFacet
{
	[DisplayNameKey("DefaultTraceEnabledName")]
	[DisplayDescriptionKey("DefaultTraceEnabledDesc")]
	bool DefaultTraceEnabled { get; set; }

	[DisplayNameKey("C2AuditTracingEnabledName")]
	[DisplayDescriptionKey("C2AuditTracingEnabledDesc")]
	[PostConfigurationAction(PostConfigurationAction.RestartService)]
	bool C2AuditTracingEnabled { get; set; }

	[DisplayNameKey("LoginAuditLevelName")]
	[DisplayDescriptionKey("LoginAuditLevelDesc")]
	AuditLevel LoginAuditLevel { get; set; }
}
