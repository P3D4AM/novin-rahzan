using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

[DesignTimeVisible(false)]
[ToolboxItem(false)]
public class CompletionEdit : UserControl
{
	private TextBox editBox;

	private Label label;

	private Label pathLabel;

	private IPainter painter;

	public TextBox EditBox => editBox;

	public Label Label => label;

	public Label PathLabel => pathLabel;

	public CompletionEdit()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Expected O, but got Unknown
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		painter = new GdiPainter();
		((Control)this).Height = ((Control)this).Font.Height;
		editBox = new TextBox();
		((TextBoxBase)editBox).Multiline = false;
		((Control)editBox).Parent = (Control)(object)this;
		((TextBoxBase)editBox).BorderStyle = (BorderStyle)0;
		((Control)editBox).Top = 0;
		((Control)editBox).KeyPress += new KeyPressEventHandler(DoKeyPress);
		pathLabel = new Label();
		((Control)pathLabel).Parent = (Control)(object)this;
		((Control)pathLabel).Font = new Font(((Control)PathLabel).Font, (FontStyle)4);
		((Control)pathLabel).Top = 0;
		((Control)pathLabel).AutoSize = true;
		((Control)pathLabel).ForeColor = EditConsts.DefaultUrlForeColor;
		label = new Label();
		((Control)label).Parent = (Control)(object)this;
		((Control)label).Font = new Font(((Control)label).Font, (FontStyle)1);
		((Control)label).Top = 0;
		((Control)label).AutoSize = true;
		((Control)this).BackColor = EditConsts.DefaultSnippetBackColor;
		((Control)editBox).BackColor = EditConsts.DefaultSnippetBackColor;
		((Control)label).BackColor = EditConsts.DefaultSnippetBackColor;
		((Control)pathLabel).BackColor = EditConsts.DefaultSnippetBackColor;
		UpdateSize();
	}

	protected void DoKeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = e.KeyChar == '\n' || e.KeyChar == '\r' || e.KeyChar == '\u001b';
	}

	protected internal void UpdateSize()
	{
		painter.Font = ((Control)pathLabel).Font;
		int num = Math.Max((((Control)pathLabel).Width - painter.StringWidth(((Control)pathLabel).Text)) / 2, 0);
		int num2 = ((Control)label).Width + ((Control)pathLabel).Width - num;
		int width = ((Control)this).ClientRectangle.Width;
		((Control)label).Left = Math.Min(width - 2 - num2, 0);
		((Control)pathLabel).Left = ((Control)label).Left + ((Control)label).Width;
		((Control)editBox).Left = ((Control)pathLabel).Left + ((Control)pathLabel).Width - num;
		((Control)editBox).Width = width - ((Control)editBox).Left;
	}
}
