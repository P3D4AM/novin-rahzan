using System.Runtime.Serialization;

namespace Microsoft.SqlServer.Management.RegisteredServers;

[DataContract]
public class AzureDataStudioConnectionGroup
{
	[DataMember(Name = "color")]
	public string Color { get; set; }

	[DataMember(Name = "description")]
	public string Description { get; set; }

	[DataMember(Name = "id")]
	public string Id { get; set; }

	[DataMember(Name = "name")]
	public string Name { get; set; }

	[DataMember(Name = "parentId")]
	public string ParentId { get; set; }
}
