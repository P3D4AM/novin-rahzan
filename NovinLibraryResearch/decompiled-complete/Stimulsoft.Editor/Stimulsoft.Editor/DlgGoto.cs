using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

public class DlgGoto : Form, IGotoLineDialog
{
	public Label laEnterNewLine;

	public TextBox tbNewLineNumber;

	public Button btCancel;

	public Button btOK;

	public Panel panel1;

	public Container components;

	public int Line
	{
		get
		{
			return int.Parse(((Control)tbNewLineNumber).Text) - 1;
		}
		set
		{
			((Control)tbNewLineNumber).Text = (value + 1).ToString();
		}
	}

	public int Lines
	{
		set
		{
			((Control)laEnterNewLine).Text = string.Format(StringConsts.LineNumberCaption, 1, value);
		}
	}

	public DlgGoto()
	{
		InitializeComponent();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Expected O, but got Unknown
		laEnterNewLine = new Label();
		tbNewLineNumber = new TextBox();
		btCancel = new Button();
		btOK = new Button();
		panel1 = new Panel();
		((Control)panel1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)laEnterNewLine).AutoSize = true;
		((Control)laEnterNewLine).ForeColor = Color.DimGray;
		((Control)laEnterNewLine).Location = new Point(12, 12);
		((Control)laEnterNewLine).Name = "laEnterNewLine";
		((Control)laEnterNewLine).Size = new Size(65, 13);
		((Control)laEnterNewLine).TabIndex = 0;
		((Control)laEnterNewLine).Text = "Line number";
		((Control)tbNewLineNumber).Location = new Point(12, 32);
		((Control)tbNewLineNumber).Name = "tbNewLineNumber";
		((Control)tbNewLineNumber).Size = new Size(216, 20);
		((Control)tbNewLineNumber).TabIndex = 1;
		((Control)tbNewLineNumber).KeyPress += new KeyPressEventHandler(tbNewLineNumber_KeyPress);
		btCancel.DialogResult = (DialogResult)2;
		((ButtonBase)btCancel).FlatStyle = (FlatStyle)3;
		((Control)btCancel).Location = new Point(154, 8);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 23);
		((Control)btCancel).TabIndex = 1;
		((Control)btCancel).Text = "Cancel";
		btOK.DialogResult = (DialogResult)1;
		((ButtonBase)btOK).FlatStyle = (FlatStyle)3;
		((Control)btOK).Location = new Point(74, 8);
		((Control)btOK).Name = "btOK";
		((Control)btOK).Size = new Size(75, 23);
		((Control)btOK).TabIndex = 0;
		((Control)btOK).Text = "OK";
		((Control)panel1).Anchor = (AnchorStyles)14;
		((Control)panel1).Controls.Add((Control)(object)btCancel);
		((Control)panel1).Controls.Add((Control)(object)btOK);
		((Control)panel1).Location = new Point(0, 67);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(242, 40);
		((Control)panel1).TabIndex = 2;
		((Form)this).AcceptButton = (IButtonControl)(object)btOK;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(242, 105);
		((Control)this).Controls.Add((Control)(object)tbNewLineNumber);
		((Control)this).Controls.Add((Control)(object)laEnterNewLine);
		((Control)this).Controls.Add((Control)(object)panel1);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "DlgGoto";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "Goto Line";
		((Form)this).Load += DlgGoto_Load;
		((Control)panel1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}

	private void DlgGoto_Load(object sender, EventArgs e)
	{
		LoadFromResource();
	}

	private void LoadFromResource()
	{
		((Control)this).Text = StringConsts.DlgGotoCaption;
		((Control)laEnterNewLine).Text = StringConsts.EnterNewLineCaption;
		((Control)btOK).Text = StringConsts.OKCaption_GotoDlg;
		((Control)btCancel).Text = StringConsts.CancelCaption_GotoDlg;
	}

	public DialogResult Execute(object sender, int lines, ref int line)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Invalid comparison between Unknown and I4
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		Lines = lines;
		Line = line;
		DialogResult val = ((Form)this).ShowDialog();
		if ((int)val == 1)
		{
			line = Line;
		}
		return val;
	}

	private void tbNewLineNumber_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (e.KeyChar <= ' ')
		{
			return;
		}
		e.Handled = e.KeyChar < '0' || e.KeyChar > '9';
		if (!e.Handled)
		{
			try
			{
				string text = ((Control)tbNewLineNumber).Text;
				int.Parse(text.Insert(Math.Min(((TextBoxBase)tbNewLineNumber).SelectionStart, text.Length), e.KeyChar.ToString()));
				return;
			}
			catch
			{
				e.Handled = true;
				OSUtils.MessageBeep();
				return;
			}
		}
		OSUtils.MessageBeep();
	}
}
