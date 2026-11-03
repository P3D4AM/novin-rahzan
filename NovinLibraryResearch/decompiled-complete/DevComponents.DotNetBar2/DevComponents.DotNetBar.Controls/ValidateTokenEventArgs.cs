using System;

namespace DevComponents.DotNetBar.Controls;

public class ValidateTokenEventArgs : EventArgs
{
	public bool IsValid = true;

	public EditToken Token;

	public readonly bool IsNewToken;

	public ValidateTokenEventArgs(EditToken token, bool isNewToken)
	{
		Token = token;
		IsNewToken = isNewToken;
	}
}
