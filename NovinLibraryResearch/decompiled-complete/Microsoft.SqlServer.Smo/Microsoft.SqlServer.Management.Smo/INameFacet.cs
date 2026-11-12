using System;
using System.ComponentModel;
using Microsoft.SqlServer.Management.Dmf;
using Microsoft.SqlServer.Management.Facets;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo;

[CLSCompliant(false)]
[EvaluationMode(AutomatedPolicyEvaluationMode.CheckOnSchedule)]
[TypeConverter(typeof(LocalizableTypeConverter))]
[LocalizedPropertyResources("Microsoft.SqlServer.Management.Smo.FacetSR")]
[DisplayNameKey("NameName")]
[DisplayDescriptionKey("NameDesc")]
public interface INameFacet : IDmfFacet
{
	[DisplayNameKey("NameName")]
	[DisplayDescriptionKey("NameDesc")]
	string Name { get; }
}
