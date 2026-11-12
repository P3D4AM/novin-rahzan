namespace Stimulsoft.Base;

public static class StiAppKey
{
	public static string GetOrGeneratedKey(IStiReportComponent component)
	{
		IStiApp app = component?.GetApp();
		return GetOrGeneratedKey(app);
	}

	public static string GetOrGeneratedKey(IStiApp app)
	{
		app?.SetKey(StiKeyHelper.GetOrGeneratedKey(app.GetKey()));
		return app?.GetKey();
	}

	public static string GetOrGeneratedKey(IStiAppDictionary dictionary)
	{
		IStiApp app = dictionary?.GetApp();
		return GetOrGeneratedKey(app);
	}

	public static string GetOrGeneratedKey(IStiAppDataSource dataSource)
	{
		IStiAppDictionary dictionary = dataSource?.GetDictionary();
		return GetOrGeneratedKey(dictionary);
	}
}
