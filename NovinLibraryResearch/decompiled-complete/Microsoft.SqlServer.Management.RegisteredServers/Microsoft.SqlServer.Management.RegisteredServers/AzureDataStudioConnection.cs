using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.SqlServer.Management.RegisteredServers;

[DataContract]
public class AzureDataStudioConnection
{
	[DataMember(Name = "groupId")]
	public string GroupId { get; set; }

	[DataMember(Name = "id")]
	public string Id { get; set; }

	[DataMember(Name = "options")]
	public Dictionary<string, string> Options { get; set; }

	[DataMember(Name = "providerName")]
	public string ProviderName { get; set; }

	[DataMember(Name = "savePassword")]
	public bool SavePassword { get; set; }
}
