using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using PacketParser.Entities;

namespace FormCustom.UC;

public class UcSevenSegmentisplay : UserControl
{
	private IContainer components;

	private Label lblF;

	private Label lblB;

	private Label lblC;

	private Label lblE;

	private Label lblD;

	private Label lblH;

	private Label lblA;

	private Label lbldat;

	private void GetValue(string number)
	{
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			switch (number)
			{
			case "0":
				((Control)lblA).BackColor = Color.Red;
				((Control)lblB).BackColor = Color.Red;
				((Control)lblC).BackColor = Color.Red;
				((Control)lblD).BackColor = Color.Red;
				((Control)lblE).BackColor = Color.Red;
				((Control)lblF).BackColor = Color.Red;
				break;
			case "1":
				((Control)lblB).BackColor = Color.Red;
				((Control)lblC).BackColor = Color.Red;
				break;
			case "2":
				((Control)lblA).BackColor = Color.Red;
				((Control)lblB).BackColor = Color.Red;
				((Control)lblH).BackColor = Color.Red;
				((Control)lblE).BackColor = Color.Red;
				((Control)lblD).BackColor = Color.Red;
				break;
			case "3":
				((Control)lblA).BackColor = Color.Red;
				((Control)lblB).BackColor = Color.Red;
				((Control)lblC).BackColor = Color.Red;
				((Control)lblD).BackColor = Color.Red;
				((Control)lblH).BackColor = Color.Red;
				break;
			case "4":
				((Control)lblB).BackColor = Color.Red;
				((Control)lblC).BackColor = Color.Red;
				((Control)lblF).BackColor = Color.Red;
				((Control)lblH).BackColor = Color.Red;
				break;
			case "5":
				((Control)lblA).BackColor = Color.Red;
				((Control)lblC).BackColor = Color.Red;
				((Control)lblD).BackColor = Color.Red;
				((Control)lblF).BackColor = Color.Red;
				((Control)lblH).BackColor = Color.Red;
				break;
			case "6":
				((Control)lblA).BackColor = Color.Red;
				((Control)lblC).BackColor = Color.Red;
				((Control)lblD).BackColor = Color.Red;
				((Control)lblE).BackColor = Color.Red;
				((Control)lblF).BackColor = Color.Red;
				((Control)lblH).BackColor = Color.Red;
				break;
			case "7":
				((Control)lblA).BackColor = Color.Red;
				((Control)lblB).BackColor = Color.Red;
				((Control)lblC).BackColor = Color.Red;
				break;
			case "8":
				((Control)lblA).BackColor = Color.Red;
				((Control)lblB).BackColor = Color.Red;
				((Control)lblC).BackColor = Color.Red;
				((Control)lblD).BackColor = Color.Red;
				((Control)lblE).BackColor = Color.Red;
				((Control)lblF).BackColor = Color.Red;
				((Control)lblH).BackColor = Color.Red;
				break;
			case "9":
				((Control)lblA).BackColor = Color.Red;
				((Control)lblB).BackColor = Color.Red;
				((Control)lblC).BackColor = Color.Red;
				((Control)lblD).BackColor = Color.Red;
				((Control)lblF).BackColor = Color.Red;
				((Control)lblH).BackColor = Color.Red;
				break;
			case ".":
				((Control)lbldat).BackColor = Color.Red;
				break;
			default:
				MessageBox.Show("مقدار صحیح وارد کنید ");
				break;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetValue", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcSevenSegmentisplay.cs", 100);
		}
	}

	public async Task InitAsync(string contextGuid, string number)
	{
		try
		{
			foreach (char item in number.ToList())
			{
				GetValue(item.ToString());
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcSevenSegmentisplay.cs", 112);
		}
	}

	public UcSevenSegmentisplay()
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
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		lblF = new Label();
		lblB = new Label();
		lblC = new Label();
		lblE = new Label();
		lblD = new Label();
		lblH = new Label();
		lblA = new Label();
		lbldat = new Label();
		((Control)this).SuspendLayout();
		((Control)lblF).Anchor = (AnchorStyles)7;
		((Control)lblF).BackColor = Color.FromArgb(249, 250, 255);
		((Control)lblF).Location = new Point(8, 11);
		((Control)lblF).Name = "lblF";
		((Control)lblF).Size = new Size(5, 50);
		((Control)lblF).TabIndex = 13;
		((Control)lblB).Anchor = (AnchorStyles)11;
		((Control)lblB).BackColor = Color.FromArgb(249, 250, 255);
		((Control)lblB).Location = new Point(68, 11);
		((Control)lblB).Name = "lblB";
		((Control)lblB).Size = new Size(5, 50);
		((Control)lblB).TabIndex = 12;
		((Control)lblC).Anchor = (AnchorStyles)11;
		((Control)lblC).BackColor = Color.FromArgb(249, 250, 255);
		((Control)lblC).Location = new Point(68, 67);
		((Control)lblC).Name = "lblC";
		((Control)lblC).Size = new Size(5, 50);
		((Control)lblC).TabIndex = 11;
		((Control)lblE).Anchor = (AnchorStyles)7;
		((Control)lblE).BackColor = Color.FromArgb(249, 250, 255);
		((Control)lblE).Location = new Point(8, 67);
		((Control)lblE).Name = "lblE";
		((Control)lblE).Size = new Size(5, 50);
		((Control)lblE).TabIndex = 10;
		((Control)lblD).Anchor = (AnchorStyles)14;
		((Control)lblD).BackColor = Color.FromArgb(249, 250, 255);
		((Control)lblD).Location = new Point(14, 117);
		((Control)lblD).Name = "lblD";
		((Control)lblD).Size = new Size(53, 5);
		((Control)lblD).TabIndex = 9;
		((Control)lblH).Anchor = (AnchorStyles)12;
		((Control)lblH).BackColor = Color.FromArgb(249, 250, 255);
		((Control)lblH).Location = new Point(14, 61);
		((Control)lblH).Name = "lblH";
		((Control)lblH).Size = new Size(53, 5);
		((Control)lblH).TabIndex = 8;
		((Control)lblA).Anchor = (AnchorStyles)13;
		((Control)lblA).BackColor = Color.FromArgb(249, 250, 255);
		((Control)lblA).Location = new Point(14, 6);
		((Control)lblA).Name = "lblA";
		((Control)lblA).Size = new Size(53, 5);
		((Control)lblA).TabIndex = 7;
		((Control)lbldat).BackColor = Color.FromArgb(249, 250, 255);
		((Control)lbldat).Location = new Point(58, 107);
		((Control)lbldat).Name = "lbldat";
		((Control)lbldat).Size = new Size(15, 15);
		((Control)lbldat).TabIndex = 14;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.FromArgb(249, 250, 255);
		((Control)this).Controls.Add((Control)(object)lbldat);
		((Control)this).Controls.Add((Control)(object)lblF);
		((Control)this).Controls.Add((Control)(object)lblB);
		((Control)this).Controls.Add((Control)(object)lblC);
		((Control)this).Controls.Add((Control)(object)lblE);
		((Control)this).Controls.Add((Control)(object)lblD);
		((Control)this).Controls.Add((Control)(object)lblH);
		((Control)this).Controls.Add((Control)(object)lblA);
		((Control)this).Name = "UcSevenSegmentisplay";
		((Control)this).Size = new Size(80, 129);
		((Control)this).ResumeLayout(false);
	}
}
