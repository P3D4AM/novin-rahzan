using System.Threading.Tasks;
using PayamakSettings.FlrSettings;

namespace PayamakSettings;

public class Answer
{
	public static Task<string> GetTemplateCallAsync()
	{
		return ReadSettings.GetValueAsync("TC");
	}

	public static async Task SetTemplateCallAsync(string value)
	{
		await ReadSettings.SetValueAsync("TC", value);
	}

	public static Task<string> GetTemplateMsgAsync()
	{
		return ReadSettings.GetValueAsync("TM");
	}

	public static Task SetTemplateMsgAsync(string value)
	{
		return ReadSettings.SetValueAsync("TM", value);
	}

	public static Task<bool> GetIsCallAsync()
	{
		return ReadSettings.GetBoolValueAsync("IsC", def: true);
	}

	public static Task SetIsCallAsync(bool value)
	{
		return ReadSettings.SetValueAsync("IsC", value.ToString());
	}

	public static Task<bool> GetIsMsgAsync()
	{
		return ReadSettings.GetBoolValueAsync("IsM", def: true);
	}

	public static Task SetIsMsgAsync(bool value)
	{
		return ReadSettings.SetValueAsync("IsM", value.ToString());
	}

	public static Task<bool> GetIsByCallAsync()
	{
		return ReadSettings.GetBoolValueAsync("IsBC", def: true);
	}

	public static Task SetIsByCallAsync(bool value)
	{
		return ReadSettings.SetValueAsync("IsBC", value.ToString());
	}

	public static Task<bool> GetIsByMsgAsync()
	{
		return ReadSettings.GetBoolValueAsync("IsBM", def: true);
	}

	public static async Task SetIsByMsgAsync(bool value)
	{
		await ReadSettings.SetValueAsync("IsBM", value.ToString());
	}
}
