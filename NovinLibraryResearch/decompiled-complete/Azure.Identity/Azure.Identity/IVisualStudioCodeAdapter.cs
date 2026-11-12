namespace Azure.Identity;

internal interface IVisualStudioCodeAdapter
{
	string GetUserSettingsPath();

	string GetCredentials(string serviceName, string accountName);
}
