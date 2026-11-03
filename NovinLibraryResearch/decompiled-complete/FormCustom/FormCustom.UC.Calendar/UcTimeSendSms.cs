using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormCustom.Calender;
using FormCustom.Properties;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.MetaData.Calendar;

namespace FormCustom.UC.Calendar;

public class UcTimeSendSms : UserControl, IHasContext
{
	public Func<Guid, Task> OnDelete;

	private IContainer components;

	private Label label1;

	private Label label2;

	private NumericUpDown nudHourFrom;

	private NumericUpDown nudMinutesFrom;

	private NumericUpDown nudHourTo;

	private NumericUpDown nudMinutesTo;

	private PictureBox picClose;

	private Label label3;

	private TextBox txtPattern;

	private ToolTip toolTip1;

	private Label label4;

	private Label label5;

	private UcGroupBox ucGroupBox1;

	private Label label6;

	private Label lblCustomerCount;

	private UcButtons btnSearchCustomer;

	internal PictureBox btnRemoveSelectedCustomers;

	public Guid Guid { get; private set; }

	public double HourFrom => (double)nudHourFrom.Value;

	public double MinutesFrom => (double)nudMinutesFrom.Value;

	public double HourTo => (double)nudHourTo.Value;

	public double MinutesTo => (double)nudMinutesTo.Value;

	public string TextPattern => ((Control)txtPattern).Text;

	public ICalendarAggregator Aggregator { get; private set; }

	public CalendarSms CalendarSms { get; private set; }

	public string ContextGuid { get; private set; }

	public async Task InitAsync(string contextGuid, CalendarSms calendarSms, ICalendarAggregator agg = null, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (calendarSms == null)
			{
				calendarSms = new CalendarSms
				{
					TimeFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0),
					TimeTo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 0)
				};
			}
			if (calendarSms.Guid == Guid.Empty)
			{
				calendarSms.Guid = Guid.NewGuid();
			}
			Guid = calendarSms.Guid;
			Aggregator = agg;
			CalendarSms = calendarSms;
			ContextGuid = contextGuid;
			if (((Control)this).IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			while (!((Control)((Control)this).FindForm()).IsHandleCreated)
			{
				await Task.Delay(100);
				if (((Control)this).IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			if (((Control)this).IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				if (((Control)this).IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
				try
				{
					toolTip1.SetToolTip((Control)(object)picClose, "حذف الگوی پیامکی".GetTranslate());
					toolTip1.SetToolTip((Control)(object)btnRemoveSelectedCustomers, "حذف اشخاص انتخاب شده".GetTranslate());
					nudHourFrom.Value = calendarSms.TimeFrom.Hour;
					nudMinutesFrom.Value = calendarSms.TimeFrom.Minute;
					nudHourTo.Value = calendarSms.TimeTo.Hour;
					nudMinutesTo.Value = calendarSms.TimeTo.Minute;
					((Control)txtPattern).Text = calendarSms.SmsPattern;
					SetCustomerLables();
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "InitAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\UcTimeSendSms.cs", 66);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\UcTimeSendSms.cs", 69);
		}
	}

	private void SetCustomerLables()
	{
		try
		{
			List<Guid> customerGuidList = CalendarSms.CustomerGuidList;
			if (customerGuidList == null || !customerGuidList.Any())
			{
				((Control)lblCustomerCount).Text = "تمامی اشخاص مجاز به دریافت پیامک هستند".GetTranslate();
			}
			else
			{
				((Control)lblCustomerCount).Text = $"تعداد {CalendarSms.CustomerGuidList.Count} شخص مجاز به دریافت پیامک می باشند".GetTranslate();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetCustomerLables", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\UcTimeSendSms.cs", 78);
		}
	}

	public UcTimeSendSms()
	{
		try
		{
			InitializeComponent();
			((Control)(object)this).Transelate();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\UcTimeSendSms.cs", 88);
		}
	}

	private void picClose_Click(object sender, EventArgs e)
	{
		OnDelete?.Invoke(Guid);
	}

	private async void btnSearchCustomer_OnClick(object sender, EventArgs e)
	{
		try
		{
			if (Aggregator != null && CalendarSms != null)
			{
				CalendarSms calendarSms = CalendarSms;
				calendarSms.CustomerGuidList = await Aggregator.GetCustomerGuidListAsync(ContextGuid, CalendarSms.CustomerGuidList, (IWin32Window)(object)((Control)this).FindForm(), default(CancellationToken));
				SetCustomerLables();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btnSearchCustomer_OnClick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\UcTimeSendSms.cs", 100);
		}
	}

	private void btnRemoveSelectedCustomers_Click(object sender, EventArgs e)
	{
		try
		{
			if (CalendarSms != null)
			{
				CalendarSms.CustomerGuidList = null;
				SetCustomerLables();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "btnRemoveSelectedCustomers_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\UcTimeSendSms.cs", 110);
		}
	}

	private void UcTimeSendSms_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.KeyCode == 13)
			{
				if (((Control)txtPattern).Focused)
				{
					e.Handled = true;
				}
				else
				{
					SendKeys.Send("{tab}");
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UcTimeSendSms_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Calender\\UcTimeSendSms.cs", 128);
		}
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
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Expected O, but got Unknown
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Expected O, but got Unknown
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Expected O, but got Unknown
		//IL_034f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Expected O, but got Unknown
		//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f8: Expected O, but got Unknown
		//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04af: Expected O, but got Unknown
		//IL_0587: Unknown result type (might be due to invalid IL or missing references)
		//IL_0591: Expected O, but got Unknown
		//IL_061c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0626: Expected O, but got Unknown
		//IL_06e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ec: Expected O, but got Unknown
		//IL_07a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ab: Expected O, but got Unknown
		//IL_085d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0867: Expected O, but got Unknown
		//IL_0914: Unknown result type (might be due to invalid IL or missing references)
		//IL_091e: Expected O, but got Unknown
		//IL_09d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e2: Expected O, but got Unknown
		//IL_0a24: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a82: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8c: Expected O, but got Unknown
		//IL_0ad2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0adc: Expected O, but got Unknown
		//IL_0aff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b09: Expected O, but got Unknown
		//IL_0b3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e54: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e5e: Expected O, but got Unknown
		//IL_0e63: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea6: Expected O, but got Unknown
		components = new Container();
		label1 = new Label();
		label2 = new Label();
		nudHourFrom = new NumericUpDown();
		nudMinutesFrom = new NumericUpDown();
		nudHourTo = new NumericUpDown();
		nudMinutesTo = new NumericUpDown();
		label3 = new Label();
		txtPattern = new TextBox();
		toolTip1 = new ToolTip(components);
		label4 = new Label();
		label5 = new Label();
		label6 = new Label();
		lblCustomerCount = new Label();
		btnSearchCustomer = new UcButtons();
		ucGroupBox1 = new UcGroupBox();
		btnRemoveSelectedCustomers = new PictureBox();
		picClose = new PictureBox();
		((ISupportInitialize)nudHourFrom).BeginInit();
		((ISupportInitialize)nudMinutesFrom).BeginInit();
		((ISupportInitialize)nudHourTo).BeginInit();
		((ISupportInitialize)nudMinutesTo).BeginInit();
		((ISupportInitialize)btnRemoveSelectedCustomers).BeginInit();
		((ISupportInitialize)picClose).BeginInit();
		((Control)this).SuspendLayout();
		((Control)label1).Anchor = (AnchorStyles)9;
		((Control)label1).AutoSize = true;
		((Control)label1).BackColor = Color.FromArgb(249, 250, 255);
		((Control)label1).Font = new Font("B Yekan", 9.75f);
		((Control)label1).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)label1).Location = new Point(358, 12);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(65, 20);
		((Control)label1).TabIndex = 9;
		((Control)label1).Text = "[از ساعت]:";
		((Control)label2).Anchor = (AnchorStyles)9;
		((Control)label2).AutoSize = true;
		((Control)label2).BackColor = Color.FromArgb(249, 250, 255);
		((Control)label2).Font = new Font("B Yekan", 9.75f);
		((Control)label2).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)label2).Location = new Point(164, 12);
		((Control)label2).Name = "label2";
		((Control)label2).Size = new Size(62, 20);
		((Control)label2).TabIndex = 7;
		((Control)label2).Text = "[تا ساعت]:";
		((Control)nudHourFrom).Anchor = (AnchorStyles)9;
		((Control)nudHourFrom).Font = new Font("B Yekan", 9.75f);
		((Control)nudHourFrom).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)nudHourFrom).Location = new Point(251, 9);
		nudHourFrom.Maximum = new decimal(new int[4] { 23, 0, 0, 0 });
		((Control)nudHourFrom).Name = "nudHourFrom";
		((Control)nudHourFrom).Size = new Size(48, 27);
		((Control)nudHourFrom).TabIndex = 0;
		((Control)nudMinutesFrom).Anchor = (AnchorStyles)9;
		((Control)nudMinutesFrom).Font = new Font("B Yekan", 9.75f);
		((Control)nudMinutesFrom).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)nudMinutesFrom).Location = new Point(310, 9);
		nudMinutesFrom.Maximum = new decimal(new int[4] { 59, 0, 0, 0 });
		((Control)nudMinutesFrom).Name = "nudMinutesFrom";
		((Control)nudMinutesFrom).Size = new Size(48, 27);
		((Control)nudMinutesFrom).TabIndex = 1;
		((Control)nudHourTo).Anchor = (AnchorStyles)9;
		((Control)nudHourTo).Font = new Font("B Yekan", 9.75f);
		((Control)nudHourTo).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)nudHourTo).Location = new Point(49, 9);
		nudHourTo.Maximum = new decimal(new int[4] { 23, 0, 0, 0 });
		((Control)nudHourTo).Name = "nudHourTo";
		((Control)nudHourTo).Size = new Size(48, 27);
		((Control)nudHourTo).TabIndex = 2;
		nudHourTo.Value = new decimal(new int[4] { 23, 0, 0, 0 });
		((Control)nudMinutesTo).Anchor = (AnchorStyles)9;
		((Control)nudMinutesTo).Font = new Font("B Yekan", 9.75f);
		((Control)nudMinutesTo).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)nudMinutesTo).Location = new Point(110, 9);
		nudMinutesTo.Maximum = new decimal(new int[4] { 59, 0, 0, 0 });
		((Control)nudMinutesTo).Name = "nudMinutesTo";
		((Control)nudMinutesTo).Size = new Size(48, 27);
		((Control)nudMinutesTo).TabIndex = 3;
		nudMinutesTo.Value = new decimal(new int[4] { 59, 0, 0, 0 });
		((Control)label3).Anchor = (AnchorStyles)9;
		((Control)label3).AutoSize = true;
		((Control)label3).BackColor = Color.FromArgb(249, 250, 255);
		((Control)label3).Font = new Font("B Yekan", 9.75f);
		((Control)label3).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)label3).Location = new Point(363, 51);
		((Control)label3).Name = "label3";
		((Control)label3).Size = new Size(60, 20);
		((Control)label3).TabIndex = 10;
		((Control)label3).Text = "[متن پیام]";
		((Control)txtPattern).Anchor = (AnchorStyles)13;
		((Control)txtPattern).Font = new Font("B Yekan", 9.75f);
		((Control)txtPattern).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)txtPattern).Location = new Point(6, 77);
		((TextBoxBase)txtPattern).Multiline = true;
		((Control)txtPattern).Name = "txtPattern";
		txtPattern.ScrollBars = (ScrollBars)2;
		((Control)txtPattern).Size = new Size(416, 102);
		((Control)txtPattern).TabIndex = 4;
		((Control)label4).Anchor = (AnchorStyles)9;
		((Control)label4).AutoSize = true;
		((Control)label4).BackColor = Color.FromArgb(249, 250, 255);
		((Control)label4).Font = new Font("B Yekan", 9.75f);
		((Control)label4).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)label4).Location = new Point(299, 12);
		((Control)label4).Name = "label4";
		((Control)label4).Size = new Size(13, 20);
		((Control)label4).TabIndex = 8;
		((Control)label4).Text = ":";
		((Control)label5).Anchor = (AnchorStyles)9;
		((Control)label5).AutoSize = true;
		((Control)label5).BackColor = Color.FromArgb(249, 250, 255);
		((Control)label5).Font = new Font("B Yekan", 9.75f);
		((Control)label5).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)label5).Location = new Point(97, 12);
		((Control)label5).Name = "label5";
		((Control)label5).Size = new Size(13, 20);
		((Control)label5).TabIndex = 6;
		((Control)label5).Text = ":";
		((Control)label6).Anchor = (AnchorStyles)9;
		((Control)label6).AutoSize = true;
		((Control)label6).BackColor = Color.FromArgb(249, 250, 255);
		((Control)label6).Font = new Font("B Yekan", 9.75f);
		((Control)label6).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)label6).Location = new Point(333, 188);
		((Control)label6).Name = "label6";
		((Control)label6).Size = new Size(90, 20);
		((Control)label6).TabIndex = 11;
		((Control)label6).Text = "[گیرندگان مجاز]:";
		((Control)lblCustomerCount).Anchor = (AnchorStyles)13;
		((Control)lblCustomerCount).BackColor = Color.FromArgb(249, 250, 255);
		((Control)lblCustomerCount).Font = new Font("B Yekan", 9.75f);
		((Control)lblCustomerCount).ForeColor = Color.FromArgb(98, 116, 125);
		((Control)lblCustomerCount).Location = new Point(90, 186);
		((Control)lblCustomerCount).Name = "lblCustomerCount";
		((Control)lblCustomerCount).Size = new Size(237, 26);
		((Control)lblCustomerCount).TabIndex = 12;
		((Control)lblCustomerCount).Text = "---";
		lblCustomerCount.TextAlign = (ContentAlignment)16;
		((Control)btnSearchCustomer).BackColor = Color.Transparent;
		btnSearchCustomer.CornerType = EnButtonCornerType.Square;
		btnSearchCustomer.DisplayText = "...";
		((Control)btnSearchCustomer).Font = new Font("B Yekan", 9.75f);
		btnSearchCustomer.Guid = new Guid("00000000-0000-0000-0000-000000000000");
		btnSearchCustomer.IsShowMoreDetails = true;
		((Control)btnSearchCustomer).Location = new Point(39, 186);
		((Control)btnSearchCustomer).Margin = new Padding(4, 5, 4, 5);
		((Control)btnSearchCustomer).Name = "btnSearchCustomer";
		((Control)btnSearchCustomer).RightToLeft = (RightToLeft)1;
		((Control)btnSearchCustomer).Size = new Size(33, 28);
		((Control)btnSearchCustomer).TabIndex = 13;
		btnSearchCustomer.TitleFont = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		btnSearchCustomer.Type = EnButtonType.Normal;
		btnSearchCustomer.OnClick += btnSearchCustomer_OnClick;
		((Control)ucGroupBox1).BackColor = Color.Transparent;
		ucGroupBox1.DisplayFont = new Font("PeydaWeb Black", 14.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)ucGroupBox1).Dock = (DockStyle)5;
		((Control)ucGroupBox1).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		ucGroupBox1.HeadHeight = 40;
		ucGroupBox1.IsShowHeader = false;
		((Control)ucGroupBox1).Location = new Point(0, 0);
		((Control)ucGroupBox1).Margin = new Padding(4, 5, 4, 5);
		((Control)ucGroupBox1).Name = "ucGroupBox1";
		((Control)ucGroupBox1).RightToLeft = (RightToLeft)1;
		((Control)ucGroupBox1).Size = new Size(427, 224);
		((Control)ucGroupBox1).TabIndex = 0;
		ucGroupBox1.Title = "        عنوان";
		((Control)btnRemoveSelectedCustomers).Anchor = (AnchorStyles)6;
		((Control)btnRemoveSelectedCustomers).BackColor = Color.FromArgb(249, 250, 255);
		((Control)btnRemoveSelectedCustomers).Cursor = Cursors.Hand;
		btnRemoveSelectedCustomers.Image = (Image)(object)FormCustom.Properties.Resources.delete11;
		((Control)btnRemoveSelectedCustomers).Location = new Point(6, 186);
		((Control)btnRemoveSelectedCustomers).Name = "btnRemoveSelectedCustomers";
		((Control)btnRemoveSelectedCustomers).Size = new Size(30, 28);
		btnRemoveSelectedCustomers.SizeMode = (PictureBoxSizeMode)4;
		btnRemoveSelectedCustomers.TabIndex = 106;
		btnRemoveSelectedCustomers.TabStop = false;
		((Control)btnRemoveSelectedCustomers).Click += btnRemoveSelectedCustomers_Click;
		((Control)picClose).BackColor = Color.FromArgb(249, 250, 255);
		((Control)picClose).Cursor = Cursors.Hand;
		picClose.Image = (Image)(object)FormCustom.Properties.Resources._19382678031536572522_32;
		((Control)picClose).Location = new Point(3, 3);
		((Control)picClose).Name = "picClose";
		((Control)picClose).Size = new Size(15, 16);
		picClose.SizeMode = (PictureBoxSizeMode)4;
		picClose.TabIndex = 6;
		picClose.TabStop = false;
		((Control)picClose).Click += picClose_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 17f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)btnRemoveSelectedCustomers);
		((Control)this).Controls.Add((Control)(object)btnSearchCustomer);
		((Control)this).Controls.Add((Control)(object)txtPattern);
		((Control)this).Controls.Add((Control)(object)lblCustomerCount);
		((Control)this).Controls.Add((Control)(object)label6);
		((Control)this).Controls.Add((Control)(object)label3);
		((Control)this).Controls.Add((Control)(object)picClose);
		((Control)this).Controls.Add((Control)(object)nudMinutesTo);
		((Control)this).Controls.Add((Control)(object)nudHourTo);
		((Control)this).Controls.Add((Control)(object)nudMinutesFrom);
		((Control)this).Controls.Add((Control)(object)nudHourFrom);
		((Control)this).Controls.Add((Control)(object)label5);
		((Control)this).Controls.Add((Control)(object)label4);
		((Control)this).Controls.Add((Control)(object)label2);
		((Control)this).Controls.Add((Control)(object)label1);
		((Control)this).Controls.Add((Control)(object)ucGroupBox1);
		((Control)this).Font = new Font("B Yekan", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)this).Margin = new Padding(3, 4, 3, 4);
		((Control)this).Name = "UcTimeSendSms";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(427, 224);
		((Control)this).KeyDown += new KeyEventHandler(UcTimeSendSms_KeyDown);
		((ISupportInitialize)nudHourFrom).EndInit();
		((ISupportInitialize)nudMinutesFrom).EndInit();
		((ISupportInitialize)nudHourTo).EndInit();
		((ISupportInitialize)nudMinutesTo).EndInit();
		((ISupportInitialize)btnRemoveSelectedCustomers).EndInit();
		((ISupportInitialize)picClose).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
