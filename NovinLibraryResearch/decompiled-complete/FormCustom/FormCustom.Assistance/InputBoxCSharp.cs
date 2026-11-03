using System.Drawing;
using System.Windows.Forms;

namespace FormCustom.Assistance;

public class InputBoxCSharp
{
	public static DialogResult ShowInputDialog(ref string input, string Header)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		Size clientSize = new Size(400, 100);
		Form val = new Form
		{
			FormBorderStyle = (FormBorderStyle)3,
			ClientSize = clientSize,
			Text = Header
		};
		RichTextBox val2 = new RichTextBox();
		((TextBoxBase)val2).MaxLength = 500;
		((Control)val2).RightToLeft = (RightToLeft)1;
		((Control)val2).Size = new Size(clientSize.Width - 10, 45);
		((Control)val2).Location = new Point(5, 5);
		((Control)val2).Text = input;
		((Control)val).Controls.Add((Control)(object)val2);
		Button val3 = new Button();
		val3.DialogResult = (DialogResult)1;
		((Control)val3).Name = "okButton";
		((Control)val3).Size = new Size(70, 23);
		((Control)val3).Text = "&پذیرش";
		((Control)val3).Location = new Point(clientSize.Width - 190, 70);
		((Control)val).Controls.Add((Control)(object)val3);
		Button val4 = new Button();
		val4.DialogResult = (DialogResult)2;
		((Control)val4).Name = "cancelButton";
		((Control)val4).Size = new Size(70, 23);
		((Control)val4).Text = "&لغو";
		((Control)val4).Location = new Point(clientSize.Width - 270, 70);
		((Control)val).Controls.Add((Control)(object)val4);
		val.AcceptButton = (IButtonControl)(object)val3;
		val.CancelButton = (IButtonControl)(object)val4;
		val.StartPosition = (FormStartPosition)1;
		DialogResult result = val.ShowDialog();
		input = ((Control)val2).Text;
		return result;
	}
}
