using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormCustom.Froms.LkManage;
using FormCustom.UC.SevenSegment;
using FormCustom.Waiting;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace FormCustom;

public class frmTest : LkUiFactory
{
	private IContainer components;

	private Button button1;

	private Button button2;

	private Label label1;

	private Button button3;

	private Button button4;

	private UcSevenSegment ucSevenSegment1;

	public frmTest(string context)
		: base(context)
	{
		InitializeComponent();
	}

	private async void button1_Click(object sender, EventArgs e)
	{
		ErrorManager.Init(ENSource.Accounting, this);
		new ReturnedSaveFuncInfo();
		try
		{
			throw new Exception("Connection Timeout Expired.");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "button1_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\frmTest.cs", 25);
		}
		await Task.Delay(50);
		try
		{
			throw new Exception("Could not allocate a new page for database");
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", $"{Guid.NewGuid()}", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\frmTest.cs", 34);
		}
		await Task.Delay(50);
		try
		{
			throw new Exception("OutOfMemory");
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, "", "button1_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\frmTest.cs", 43);
		}
		await this.ShowReturnedSavedFuncInfoAsync(new ReturnedSaveFuncInfo(new Exception(), "", "button1_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\frmTest.cs", 45), "", "button1_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\frmTest.cs");
		await Task.Delay(50);
	}

	private void button3_Click(object sender, EventArgs e)
	{
		Task task = Task.Delay(4000);
		using (new Waiter("در حال پردازش", (Form)(object)this, task, freezeForm: false, EnWaiterType.Circle, null, "button3_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\frmTest.cs"))
		{
		}
	}

	private void button4_Click(object sender, EventArgs e)
	{
		Task task = Task.Delay(4000);
		Form parent = null;
		new Waiter("2در حال پردازش", parent, task, freezeForm: false, EnWaiterType.Circle, null, "button4_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\frmTest.cs");
	}

	private void frmTest_Load(object sender, EventArgs e)
	{
		ucSevenSegment1.Weight = 12.568m;
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			components?.Dispose();
			base.Dispose(disposing);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\frmTest.Designer.cs", 28);
		}
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
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0417: Unknown result type (might be due to invalid IL or missing references)
		button4 = new Button();
		button3 = new Button();
		label1 = new Label();
		button2 = new Button();
		button1 = new Button();
		ucSevenSegment1 = new UcSevenSegment();
		((Control)this).SuspendLayout();
		((Control)button4).Location = new Point(0, 29);
		((Control)button4).Margin = new Padding(4, 3, 4, 3);
		((Control)button4).Name = "button4";
		((Control)button4).Size = new Size(206, 23);
		((Control)button4).TabIndex = 4;
		((Control)button4).Text = "test waiter with waiterForm";
		((ButtonBase)button4).UseVisualStyleBackColor = true;
		((Control)button4).Click += button4_Click;
		((Control)button3).Location = new Point(0, 0);
		((Control)button3).Margin = new Padding(4, 3, 4, 3);
		((Control)button3).Name = "button3";
		((Control)button3).Size = new Size(206, 23);
		((Control)button3).TabIndex = 3;
		((Control)button3).Text = "test waiter with Waiter UserControl";
		((ButtonBase)button3).UseVisualStyleBackColor = true;
		((Control)button3).Click += button3_Click;
		((Control)label1).AutoSize = true;
		((Control)label1).Location = new Point(14, 145);
		((Control)label1).Margin = new Padding(4, 0, 4, 0);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(41, 13);
		((Control)label1).TabIndex = 1;
		((Control)label1).Text = "label1";
		((Control)button2).Location = new Point(321, 12);
		((Control)button2).Margin = new Padding(4, 3, 4, 3);
		((Control)button2).Name = "button2";
		((Control)button2).Size = new Size(251, 109);
		((Control)button2).TabIndex = 0;
		((Control)button2).Text = "Excel Test";
		((ButtonBase)button2).UseVisualStyleBackColor = true;
		((Control)button1).Location = new Point(14, 110);
		((Control)button1).Margin = new Padding(4, 3, 4, 3);
		((Control)button1).Name = "button1";
		((Control)button1).Size = new Size(88, 23);
		((Control)button1).TabIndex = 0;
		((Control)button1).Text = "button1";
		((ButtonBase)button1).UseVisualStyleBackColor = true;
		((Control)button1).Click += button1_Click;
		((Control)ucSevenSegment1).Location = new Point(67, 298);
		((Control)ucSevenSegment1).Margin = new Padding(4, 3, 4, 3);
		((Control)ucSevenSegment1).Name = "ucSevenSegment1";
		((Control)ucSevenSegment1).RightToLeft = (RightToLeft)0;
		((Control)ucSevenSegment1).Size = new Size(716, 150);
		((Control)ucSevenSegment1).TabIndex = 5;
		ucSevenSegment1.Weight = new decimal(new int[4] { 7256, 0, 0, -2147287040 });
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = SystemColors.ActiveCaption;
		((Form)this).ClientSize = new Size(1042, 534);
		((Control)this).Controls.Add((Control)(object)ucSevenSegment1);
		((Control)this).Controls.Add((Control)(object)button4);
		((Control)this).Controls.Add((Control)(object)button3);
		((Control)this).Controls.Add((Control)(object)label1);
		((Control)this).Controls.Add((Control)(object)button2);
		((Control)this).Controls.Add((Control)(object)button1);
		((Form)this).Margin = new Padding(5, 3, 5, 3);
		((Control)this).Name = "frmTest";
		((Control)this).Text = "frmTest";
		((Form)this).Load += frmTest_Load;
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
