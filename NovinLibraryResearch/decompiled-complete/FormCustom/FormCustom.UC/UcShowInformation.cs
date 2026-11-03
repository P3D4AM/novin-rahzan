using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FormCustom.UC;

public class UcShowInformation : UserControl
{
	public class Informition
	{
	}

	private IContainer components;

	private TextBox textBox1;

	public UcShowInformation()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		textBox1 = new TextBox();
		((Control)this).SuspendLayout();
		((Control)textBox1).Anchor = (AnchorStyles)15;
		((Control)textBox1).BackColor = Color.FromArgb(249, 250, 255);
		((TextBoxBase)textBox1).BorderStyle = (BorderStyle)0;
		((Control)textBox1).Cursor = Cursors.Hand;
		((Control)textBox1).Font = new Font("PeydaWeb", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)textBox1).ForeColor = Color.DimGray;
		((Control)textBox1).Location = new Point(0, 0);
		((Control)textBox1).Margin = new Padding(0);
		((TextBoxBase)textBox1).Multiline = true;
		((Control)textBox1).Name = "textBox1";
		((Control)textBox1).Size = new Size(230, 115);
		((Control)textBox1).TabIndex = 0;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)textBox1);
		((Control)this).Name = "UcShowInformation";
		((Control)this).Size = new Size(230, 115);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
