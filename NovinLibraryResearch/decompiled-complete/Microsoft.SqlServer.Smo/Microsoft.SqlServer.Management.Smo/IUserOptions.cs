using System;
using System.ComponentModel;
using Microsoft.SqlServer.Management.Dmf;
using Microsoft.SqlServer.Management.Facets;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo;

[CLSCompliant(false)]
[StateChangeEvent("CREATE_USER", "USER", "SQL USER")]
[StateChangeEvent("ALTER_USER", "USER", "SQL USER")]
[StateChangeEvent("CREATE_USER", "USER", "WINDOWS USER")]
[StateChangeEvent("ALTER_USER", "USER", "WINDOWS USER")]
[StateChangeEvent("CREATE_USER", "USER", "GROUP USER")]
[StateChangeEvent("ALTER_USER", "USER", "GROUP USER")]
[StateChangeEvent("CREATE_USER", "USER", "CERTIFICATE USER")]
[StateChangeEvent("ALTER_USER", "USER", "CERTIFICATE USER")]
[StateChangeEvent("CREATE_USER", "USER", "ASYMMETRIC KEY USER")]
[StateChangeEvent("ALTER_USER", "USER", "ASYMMETRIC KEY USER")]
[EvaluationMode(AutomatedPolicyEvaluationMode.Enforce | AutomatedPolicyEvaluationMode.CheckOnChanges | AutomatedPolicyEvaluationMode.CheckOnSchedule)]
[LocalizedPropertyResources("Microsoft.SqlServer.Management.Smo.LocalizableResources")]
[TypeConverter(typeof(LocalizableTypeConverter))]
[DisplayNameKey("IUserOptions_Name")]
[DisplayDescriptionKey("IUserOptions_Desc")]
public interface IUserOptions : IDmfFacet
{
	[DisplayNameKey("User_AsymmetricKeyName")]
	[DisplayDescriptionKey("User_AsymmetricKeyDesc")]
	string AsymmetricKey { get; }

	[DisplayNameKey("User_CertificateName")]
	[DisplayDescriptionKey("User_CertificateDesc")]
	string Certificate { get; }

	[DisplayNameKey("User_CreateDateName")]
	[DisplayDescriptionKey("User_CreateDateDesc")]
	DateTime CreateDate { get; }

	[DisplayNameKey("User_DefaultSchemaName")]
	[DisplayDescriptionKey("User_DefaultSchemaDesc")]
	string DefaultSchema { get; set; }

	[DisplayNameKey("User_IDName")]
	[DisplayDescriptionKey("User_IDDesc")]
	int ID { get; }

	[DisplayNameKey("User_IsSystemObjectName")]
	[DisplayDescriptionKey("User_IsSystemObjectDesc")]
	bool IsSystemObject { get; }

	[DisplayNameKey("User_LoginName")]
	[DisplayDescriptionKey("User_LoginDesc")]
	string Login { get; }

	[DisplayNameKey("User_LoginTypeName")]
	[DisplayDescriptionKey("User_LoginTypeDesc")]
	LoginType LoginType { get; }

	[DisplayNameKey("NamedSmoObject_NameName")]
	[DisplayDescriptionKey("NamedSmoObject_NameDesc")]
	string Name { get; }

	[DisplayNameKey("User_SidName")]
	[DisplayDescriptionKey("User_SidDesc")]
	byte[] Sid { get; }

	[DisplayNameKey("User_UserTypeName")]
	[DisplayDescriptionKey("User_UserTypeDesc")]
	UserType UserType { get; }
}
