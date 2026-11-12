using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class KeyEventArgsEx : KeyEventArgs
{
	public char KeyChar;

	public KeyEventArgsEx(Keys keyData, char keyChar)
		: base(keyData)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		KeyChar = keyChar;
	}
}
