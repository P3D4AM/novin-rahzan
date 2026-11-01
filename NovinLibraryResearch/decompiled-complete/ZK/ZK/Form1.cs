using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Axzkemkeeper;

namespace ZK;

public class Form1 : Form
{
	private IContainer components;

	public Form1()
	{
		InitializeComponent();
	}

	private void AxCZKEM1_OnAttTransaction(object sender, _IZKEMEvents_OnAttTransactionEvent e)
	{
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
		((Control)this).SuspendLayout();
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(284, 261);
		((Control)this).Name = "Form1";
		((Control)this).Text = "Form1";
		((Control)this).ResumeLayout(false);
	}
}
