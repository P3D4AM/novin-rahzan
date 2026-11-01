using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace TimeAttendanceUI.UC;

public class UcLable : UserControl
{
	private IContainer components;

	private Label LblCaption;

	private Label LblValue;

	public string Caption
	{
		get
		{
			return ((Control)LblCaption).Text;
		}
		set
		{
			((Control)LblCaption).Text = value;
		}
	}

	public string Value
	{
		get
		{
			return ((Control)LblValue).Text;
		}
		set
		{
			((Control)LblValue).Text = value;
		}
	}

	public UcLable()
	{
		InitializeComponent();
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
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		LblCaption = new Label();
		LblValue = new Label();
		((Control)this).SuspendLayout();
		((Control)LblCaption).Anchor = (AnchorStyles)11;
		((Control)LblCaption).BackColor = Color.Transparent;
		((Control)LblCaption).Location = new Point(175, 0);
		((Control)LblCaption).Name = "LblCaption";
		((Control)LblCaption).RightToLeft = (RightToLeft)1;
		((Control)LblCaption).Size = new Size(123, 23);
		((Control)LblCaption).TabIndex = 0;
		((Control)LblCaption).Text = "عنوان :";
		LblCaption.TextAlign = (ContentAlignment)64;
		((Control)LblValue).Anchor = (AnchorStyles)15;
		((Control)LblValue).BackColor = Color.Transparent;
		((Control)LblValue).ForeColor = Color.DarkBlue;
		((Control)LblValue).Location = new Point(0, 0);
		((Control)LblValue).Name = "LblValue";
		((Control)LblValue).RightToLeft = (RightToLeft)1;
		((Control)LblValue).Size = new Size(169, 23);
		((Control)LblValue).TabIndex = 1;
		((Control)LblValue).Text = "---";
		LblValue.TextAlign = (ContentAlignment)16;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)LblValue);
		((Control)this).Controls.Add((Control)(object)LblCaption);
		((Control)this).Name = "UcLable";
		((Control)this).Size = new Size(298, 23);
		((Control)this).ResumeLayout(false);
	}
}
