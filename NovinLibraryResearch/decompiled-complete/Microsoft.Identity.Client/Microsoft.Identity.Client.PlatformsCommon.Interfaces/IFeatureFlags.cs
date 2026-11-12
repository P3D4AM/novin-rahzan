namespace Microsoft.Identity.Client.PlatformsCommon.Interfaces;

internal interface IFeatureFlags
{
	bool IsFociEnabled { get; }
}
