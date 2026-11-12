using System.ComponentModel;
using System.Threading;
using Azure.Core;

namespace Azure;

public class AzureKeyCredential
{
	private string _key;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public string Key
	{
		get
		{
			return Volatile.Read(ref _key);
		}
		private set
		{
			Volatile.Write(ref _key, value);
		}
	}

	public AzureKeyCredential(string key)
	{
		Update(key);
	}

	public void Update(string key)
	{
		Argument.AssertNotNullOrEmpty(key, "key");
		Key = key;
	}
}
