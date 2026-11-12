#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Microsoft.SqlServer.Management.RegisteredServers;

[DataContract]
public class AzureDataStudioConnectionStore
{
	[DataMember(Name = "datasource.connectionGroups")]
	public List<AzureDataStudioConnectionGroup> Groups { get; set; }

	[DataMember(Name = "datasource.connections")]
	public List<AzureDataStudioConnection> Connections { get; set; }

	public static AzureDataStudioConnectionStore LoadAzureDataStudioConnections(string settingsFile = null)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		if (string.IsNullOrEmpty(settingsFile))
		{
			settingsFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "azuredatastudio\\User\\settings.json");
		}
		if (File.Exists(settingsFile))
		{
			try
			{
				using FileStream fileStream = new FileStream(settingsFile, FileMode.Open, FileAccess.Read);
				AzureDataStudioConnectionStore obj = (AzureDataStudioConnectionStore)((XmlObjectSerializer)new DataContractJsonSerializer(typeof(AzureDataStudioConnectionStore), new DataContractJsonSerializerSettings
				{
					UseSimpleDictionaryFormat = true
				})).ReadObject((Stream)fileStream);
				obj.Groups = obj.Groups ?? new List<AzureDataStudioConnectionGroup>();
				obj.Connections = obj.Connections ?? new List<AzureDataStudioConnection>();
				return obj;
			}
			catch (Exception ex)
			{
				Trace.TraceInformation("Unable to read Azure Data Studio settings: {0}", ex.ToString());
			}
		}
		return new AzureDataStudioConnectionStore
		{
			Groups = new List<AzureDataStudioConnectionGroup>(),
			Connections = new List<AzureDataStudioConnection>()
		};
	}
}
