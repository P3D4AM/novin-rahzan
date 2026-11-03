using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using FormCustom.Properties;
using PacketParser;
using PacketParser.Entities;

namespace FormCustom.VersionController;

public class UcVersionController : UserControl
{
	private int textHeight;

	public bool isColapsed;

	private IContainer components;

	private PanelEx panel1;

	private PanelEx panel2;

	private Label label2;

	private Label label4;

	private PictureBox pcArrow;

	public UcVersionController()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	private void Collapse()
	{
		try
		{
			for (int i = 56; i <= textHeight; i += 5)
			{
				((Control)this).Size = new Size(((Control)this).Size.Width, i);
			}
			pcArrow.Image = (Image)(object)FormCustom.Properties.Resources.down_chevron_up;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Collapse", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\VersionController\\UcVersionController.cs", 39);
		}
	}

	private void UnCollapse()
	{
		try
		{
			for (int num = textHeight; num >= 56; num -= 5)
			{
				((Control)this).Size = new Size(((Control)this).Size.Width, num);
			}
			pcArrow.Image = (Image)(object)FormCustom.Properties.Resources.down_chevron;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UnCollapse", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\VersionController\\UcVersionController.cs", 49);
		}
	}

	public void UpdateInfo(SoftwarePackage item)
	{
		try
		{
			string text = null;
			text += "  ";
			text += "نسخه:";
			text += DeleteWaste(item.Version.ToLower());
			text += "         ";
			text += "مرحله انتشار:";
			text += item.Channel.GetDisplay();
			text += "         ";
			text += "تاریخ:";
			text += item.CreationDateSh.ToString();
			((Control)label4).Text = text;
			((Control)label2).Text = item.DescriptionUrl.ToString();
			int sizeOf = GetSizeOf(((Control)label2).Text);
			textHeight = sizeOf * 34 + 56;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateInfo", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\VersionController\\UcVersionController.cs", 70);
		}
	}

	private int GetSizeOf(string text)
	{
		int num = 0;
		try
		{
			string text2 = text;
			while (text2.Contains("\r\n"))
			{
				int startIndex = text2.IndexOf("\r\n") + 4;
				text2 = text2.Substring(startIndex);
				num++;
			}
			if (num != 0)
			{
				num++;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSizeOf", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\VersionController\\UcVersionController.cs", 86);
		}
		return num;
	}

	private void label4_Click_1(object sender, EventArgs e)
	{
		try
		{
			if (!isColapsed)
			{
				Collapse();
				isColapsed = true;
			}
			else
			{
				UnCollapse();
				isColapsed = false;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "label4_Click_1", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\VersionController\\UcVersionController.cs", 104);
		}
	}

	private void pictureBox1_Click_1(object sender, EventArgs e)
	{
		label4_Click_1(sender, e);
	}

	private string DeleteWaste(string version)
	{
		try
		{
			string[] array = version.Split(new char[1] { '.' });
			return array[0] + "." + array[1] + "." + array[2];
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DeleteWaste", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\VersionController\\UcVersionController.cs", 115);
		}
		return "";
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
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Expected O, but got Unknown
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Expected O, but got Unknown
		panel1 = new PanelEx();
		label2 = new Label();
		panel2 = new PanelEx();
		pcArrow = new PictureBox();
		label4 = new Label();
		((Control)panel1).SuspendLayout();
		((Control)panel2).SuspendLayout();
		((ISupportInitialize)pcArrow).BeginInit();
		((Control)this).SuspendLayout();
		((Control)panel1).Anchor = (AnchorStyles)15;
		((Control)panel1).Controls.Add((Control)(object)label2);
		panel1.DisabledBackColor = Color.Empty;
		((Control)panel1).Location = new Point(4, 53);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(600, 0);
		panel1.Style.BackColor1.Color = Color.FromArgb(204, 204, 255);
		panel1.Style.BackColor2.Color = Color.FromArgb(204, 204, 255);
		panel1.Style.BorderColor.Color = Color.FromArgb(204, 204, 255);
		panel1.Style.CornerDiameter = 20;
		panel1.Style.CornerType = eCornerType.Rounded;
		((Control)panel1).TabIndex = 10;
		((Control)label2).BackColor = Color.FromArgb(204, 204, 255);
		((Control)label2).Dock = (DockStyle)5;
		((Control)label2).Font = new Font("PeydaWeb", 14.25f, (FontStyle)0, (GraphicsUnit)3);
		((Control)label2).ForeColor = Color.Black;
		((Control)label2).Location = new Point(0, 0);
		((Control)label2).Name = "label2";
		((Control)label2).Padding = new Padding(0, 5, 5, 5);
		((Control)label2).RightToLeft = (RightToLeft)1;
		((Control)label2).Size = new Size(600, 0);
		((Control)label2).TabIndex = 0;
		((Control)label2).Text = "label2";
		((Control)panel2).Anchor = (AnchorStyles)13;
		((Control)panel2).Controls.Add((Control)(object)pcArrow);
		((Control)panel2).Controls.Add((Control)(object)label4);
		panel2.DisabledBackColor = Color.Empty;
		((Control)panel2).Location = new Point(3, 0);
		((Control)panel2).Name = "panel2";
		((Control)panel2).Size = new Size(605, 43);
		panel2.Style.BackColor1.Color = Color.FromArgb(65, 81, 220);
		panel2.Style.BackColor2.Color = Color.FromArgb(65, 81, 220);
		panel2.Style.CornerDiameter = 12;
		panel2.Style.CornerType = eCornerType.Rounded;
		((Control)panel2).TabIndex = 11;
		((Control)pcArrow).Cursor = Cursors.Hand;
		((Control)pcArrow).Location = new Point(12, 5);
		((Control)pcArrow).Name = "pcArrow";
		((Control)pcArrow).Size = new Size(40, 35);
		pcArrow.SizeMode = (PictureBoxSizeMode)1;
		pcArrow.TabIndex = 2;
		pcArrow.TabStop = false;
		((Control)pcArrow).Click += pictureBox1_Click_1;
		((Control)label4).Anchor = (AnchorStyles)15;
		((Control)label4).Cursor = Cursors.Hand;
		((Control)label4).Font = new Font("PeydaWeb", 14.25f, (FontStyle)1, (GraphicsUnit)3);
		((Control)label4).ForeColor = Color.FromArgb(235, 237, 255);
		((Control)label4).Location = new Point(1, 0);
		((Control)label4).Name = "label4";
		((Control)label4).Size = new Size(605, 43);
		((Control)label4).TabIndex = 2;
		((Control)label4).Text = "nvbnbnbn";
		label4.TextAlign = (ContentAlignment)16;
		label4.UseCompatibleTextRendering = true;
		((Control)label4).Click += label4_Click_1;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 15f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).AutoSize = true;
		((Control)this).Controls.Add((Control)(object)panel2);
		((Control)this).Controls.Add((Control)(object)panel1);
		((Control)this).Name = "UserControl1";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(608, 55);
		((Control)panel1).ResumeLayout(false);
		((Control)panel2).ResumeLayout(false);
		((ISupportInitialize)pcArrow).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
