namespace Stimulsoft.Report.Dictionary;

public class StiUserNameAndPassword
{
	public string UserName { get; }

	public string Password { get; }

	public bool CacheData { get; set; }

	public StiUserNameAndPassword(string userName, string password)
	{
		UserName = userName;
		Password = password;
	}

	public StiUserNameAndPassword(string userName, string password, bool cacheData)
	{
		UserName = userName;
		Password = password;
		CacheData = cacheData;
	}
}
