using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiAllowHtmlTagsInTextCheck : StiComponentCheck
{
	public override bool PreviewVisible => true;

	public override string ShortMessage => StiLocalizationExt.Get("CheckComponent", "StiAllowHtmlTagsInTextCheckShort");

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiAllowHtmlTagsInTextCheckLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Warning;

	private bool Check()
	{
		if (base.Element is StiText { AllowHtmlTags: false } stiText && !string.IsNullOrEmpty(stiText.Text.Value))
		{
			if (stiText.Text.Value.Contains("&amp;") || stiText.Text.Value.Contains("&lt;") || stiText.Text.Value.Contains("&gt;") || stiText.Text.Value.Contains("&quot;") || stiText.Text.Value.Contains("&nbsp;"))
			{
				return true;
			}
			int num = 0;
			int num2 = 0;
			for (; num < stiText.Text.Value.Length; num++)
			{
				num = stiText.Text.Value.IndexOf("<", num);
				if (num == -1)
				{
					break;
				}
				num2 = stiText.Text.Value.IndexOf(">", num);
				if (num2 == -1)
				{
					break;
				}
				if (num2 <= num + 1)
				{
					continue;
				}
				string text = stiText.Text.Value.Substring(num + 1, num2 - num - 1).Trim();
				if (text.Length <= 0)
				{
					continue;
				}
				if (text.Length < 10)
				{
					switch (text)
					{
					case "b":
					case "/b":
					case "i":
					case "/i":
					case "u":
					case "/u":
					case "s":
					case "/s":
					case "sub":
					case "/sub":
					case "sup":
					case "/sup":
					case "br":
					case "/br":
					case "strong":
					case "/strong":
					case "p":
					case "/p":
					case "em":
					case "/em":
						goto IL_03ce;
					}
				}
				if (!text.StartsWith("font ") && !text.StartsWith("/font") && !text.StartsWith("font-face") && !text.StartsWith("/font-face") && !text.StartsWith("font-name") && !text.StartsWith("/font-name") && !text.StartsWith("font-family") && !text.StartsWith("/font-family") && !text.StartsWith("font-size") && !text.StartsWith("/font-size") && !text.StartsWith("font-color") && !text.StartsWith("/font-color") && !text.StartsWith("color") && !text.StartsWith("/color") && !text.StartsWith("background-color") && !text.StartsWith("/background-color") && !text.StartsWith("letter-spacing") && !text.StartsWith("/letter-spacing") && !text.StartsWith("word-spacing") && !text.StartsWith("/word-spacing") && !text.StartsWith("line-height") && !text.StartsWith("/line-height") && !text.StartsWith("text-align") && !text.StartsWith("/text-align"))
				{
					continue;
				}
				goto IL_03ce;
				IL_03ce:
				return true;
			}
		}
		return false;
	}

	public override object ProcessCheck(StiReport report, object obj)
	{
		base.Element = obj;
		try
		{
			if (Check())
			{
				StiAllowHtmlTagsInTextCheck stiAllowHtmlTagsInTextCheck = new StiAllowHtmlTagsInTextCheck();
				stiAllowHtmlTagsInTextCheck.Element = obj;
				stiAllowHtmlTagsInTextCheck.Actions.Add(new StiAllowHtmlTagsInTextAction());
				return stiAllowHtmlTagsInTextCheck;
			}
			return null;
		}
		finally
		{
			base.Element = null;
		}
	}
}
