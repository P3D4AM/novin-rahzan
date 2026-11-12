namespace Microsoft.Identity.Client.ApiConfig.Executors;

internal static class ClientExecutorFactory
{
	private static bool IsMatsEnabled(ClientApplicationBase clientApplicationBase)
	{
		return clientApplicationBase.ServiceBundle.Mats != null;
	}

	public static IPublicClientApplicationExecutor CreatePublicClientExecutor(PublicClientApplication publicClientApplication)
	{
		IPublicClientApplicationExecutor publicClientApplicationExecutor = new PublicClientExecutor(publicClientApplication.ServiceBundle, publicClientApplication);
		if (IsMatsEnabled(publicClientApplication))
		{
			publicClientApplicationExecutor = new TelemetryPublicClientExecutor(publicClientApplicationExecutor, publicClientApplication.ServiceBundle.Mats);
		}
		return publicClientApplicationExecutor;
	}

	public static IConfidentialClientApplicationExecutor CreateConfidentialClientExecutor(ConfidentialClientApplication confidentialClientApplication)
	{
		IConfidentialClientApplicationExecutor confidentialClientApplicationExecutor = new ConfidentialClientExecutor(confidentialClientApplication.ServiceBundle, confidentialClientApplication);
		if (IsMatsEnabled(confidentialClientApplication))
		{
			confidentialClientApplicationExecutor = new TelemetryConfidentialClientExecutor(confidentialClientApplicationExecutor, confidentialClientApplication.ServiceBundle.Mats);
		}
		return confidentialClientApplicationExecutor;
	}

	public static IClientApplicationBaseExecutor CreateClientApplicationBaseExecutor(ClientApplicationBase clientApplicationBase)
	{
		IClientApplicationBaseExecutor clientApplicationBaseExecutor = new ClientApplicationBaseExecutor(clientApplicationBase.ServiceBundle, clientApplicationBase);
		if (IsMatsEnabled(clientApplicationBase))
		{
			clientApplicationBaseExecutor = new TelemetryClientApplicationBaseExecutor(clientApplicationBaseExecutor, clientApplicationBase.ServiceBundle.Mats);
		}
		return clientApplicationBaseExecutor;
	}
}
