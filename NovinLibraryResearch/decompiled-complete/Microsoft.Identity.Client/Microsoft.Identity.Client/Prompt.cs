namespace Microsoft.Identity.Client;

public struct Prompt
{
	public static readonly Prompt SelectAccount = new Prompt("select_account");

	public static readonly Prompt ForceLogin = new Prompt("login");

	public static readonly Prompt Consent = new Prompt("consent");

	public static readonly Prompt NoPrompt = new Prompt("no_prompt");

	public static readonly Prompt Never = new Prompt("attempt_none");

	internal static readonly Prompt NotSpecified = new Prompt("not_specified");

	internal string PromptValue { get; }

	private Prompt(string promptValue)
	{
		PromptValue = promptValue;
	}

	public override bool Equals(object obj)
	{
		if (obj is Prompt)
		{
			return this == (Prompt)obj;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return PromptValue.GetHashCode();
	}

	public static bool operator ==(Prompt x, Prompt y)
	{
		return x.PromptValue == y.PromptValue;
	}

	public static bool operator !=(Prompt x, Prompt y)
	{
		return !(x == y);
	}
}
