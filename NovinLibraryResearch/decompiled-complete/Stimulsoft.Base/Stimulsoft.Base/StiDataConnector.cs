namespace Stimulsoft.Base;

public abstract class StiDataConnector
{
	public static bool AdvancedRetrievalModeOfDatabaseSchema { get; set; }

	public static bool GetSchemaColumnsMode { get; set; } = true;


	public abstract StiConnectionIdent ConnectionIdent { get; }

	public abstract StiConnectionOrder ConnectionOrder { get; }

	public abstract string Name { get; }

	public virtual string[] NuGetPackages => null;

	public virtual string NuGetVersion => null;

	public abstract bool IsAvailable { get; }

	public virtual void ResetSettings()
	{
	}

	public virtual StiDataConnector[] GetFamilyConnectors()
	{
		return new StiDataConnector[1] { this };
	}
}
