using System.Threading.Tasks;
using PayamakSettings.FlrSettings;

namespace PayamakSettings;

public class Sound
{
	public static Task<string> GetPathSendAsync()
	{
		return ReadSettings.GetValueAsync("PS1");
	}

	public static Task SetPathSendAsync(string value)
	{
		return ReadSettings.SetValueAsync("PS1", value);
	}

	public static Task<bool> GetIsPlaySendAsync()
	{
		return ReadSettings.GetBoolValueAsync("PS2", def: true);
	}

	public static Task SetIsPlaySendAsync(bool value)
	{
		return ReadSettings.SetValueAsync("PS2", value.ToString());
	}

	public static Task<string> GetPathRecieveAsync()
	{
		return ReadSettings.GetValueAsync("PR1");
	}

	public static Task SetPathRecieveAsync(string value)
	{
		return ReadSettings.SetValueAsync("PR1", value);
	}

	public static Task<bool> GetIsPlayRecieveAsync()
	{
		return ReadSettings.GetBoolValueAsync("PR2", def: true);
	}

	public static Task SetIsPlayRecieve(bool value)
	{
		return ReadSettings.SetValueAsync("PR2", value.ToString());
	}

	public static Task<string> GetPathCallAsync()
	{
		return ReadSettings.GetValueAsync("PC1");
	}

	public static Task SetPathCallAsync(string value)
	{
		return ReadSettings.SetValueAsync("PC1", value);
	}

	public static Task<bool> GetIsPlayCallAsync()
	{
		return ReadSettings.GetBoolValueAsync("PC2", def: true);
	}

	public static Task SetIsPlayCallAsync(bool value)
	{
		return ReadSettings.SetValueAsync("PC2", value.ToString());
	}

	public static Task<string> GetPathDeliveryAsync()
	{
		return ReadSettings.GetValueAsync("PD1");
	}

	public static Task SetPathDeliveryAsync(string value)
	{
		return ReadSettings.SetValueAsync("PD1", value);
	}

	public static async Task<bool> GetIsPlaydeliveryAsync()
	{
		return await ReadSettings.GetBoolValueAsync("PD2", def: true);
	}

	public static Task SetIsPlaydeliveryAsync(bool value)
	{
		return ReadSettings.SetValueAsync("PD2", value.ToString());
	}
}
