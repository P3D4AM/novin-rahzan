using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PayamakSettings.FlrSettings;

namespace PayamakSettings;

public class Font
{
	public static Font _FontProgram;

	public static async Task<Font> GetFontProgramAsync(CancellationToken token = default(CancellationToken))
	{
		if (_FontProgram == null)
		{
			string text = await ReadSettings.GetValueAsync("FontName", token);
			float num = 0f;
			if ((text == null) | string.IsNullOrEmpty(text))
			{
				text = "Tahoma";
				num = 8f;
			}
			_FontProgram = new Font(new FontFamily(text), num, (FontStyle)1);
		}
		return _FontProgram;
	}

	public static async Task SetFontProgramAsync(Font value, CancellationToken token = default(CancellationToken))
	{
		_ = 7;
		try
		{
			_FontProgram = value;
			await ReadSettings.SetValueAsync("FontName", (await GetFontProgramAsync(token)).Name, token);
			await ReadSettings.SetValueAsync("FontSize", (await GetFontProgramAsync(token)).Size.ToString(), token);
			await ReadSettings.SetValueAsync("FontItalic", (await GetFontProgramAsync(token)).Italic.ToString(), token);
			await ReadSettings.SetValueAsync("FontBold", (await GetFontProgramAsync(token)).Bold.ToString(), token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetFontProgramAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamakSettings\\Font.cs", 64);
		}
	}
}
