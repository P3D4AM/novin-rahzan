using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace PayamakUi.UC.UCSMS;

public class UCSMS : UserControl, IHasContext
{
	public const string x = ".,-!?@~_\"\\/&';^|:(){}[]=€$£§%¥*+#¿¡¤¢«»®©°<> ABCDEFGHIJKLMNOPQRSTUVWXYZ abcdefghijklmnopqrstuvwxyz01234567890";

	private IContainer components;

	internal Label LblCount;

	internal TextBoxX TxtContent;

	public string ContextGuid { get; set; }

	public string Text
	{
		get
		{
			try
			{
				return ((Control)TxtContent).Text;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Text", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSMS.cs", 187);
				return "";
			}
		}
		set
		{
			try
			{
				((Control)TxtContent).Text = value;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Text", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSMS.cs", 198);
			}
		}
	}

	public UCSMS(string contextGuid_)
	{
		try
		{
			ContextGuid = contextGuid_;
			InitializeComponent();
			((Control)TxtContent).Focus();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSMS.cs", 35);
		}
	}

	public UCSMS()
	{
		try
		{
			InitializeComponent();
			((Control)TxtContent).Focus();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSMS.cs", 47);
		}
	}

	private void RefreshLabel()
	{
		try
		{
			int length = ((Control)TxtContent).Text.Length;
			if (IsLatin(((Control)TxtContent).Text.Trim()))
			{
				if (length <= 160)
				{
					((Control)LblCount).Text = "( تعداد کاراکتر : " + ((Control)TxtContent).Text.Length + "/160          تعداد پیامک : 1/10)";
				}
				else if (160 < length && length <= 306)
				{
					((Control)LblCount).Text = "( تعداد کاراکتر : " + ((Control)TxtContent).Text.Length + "/306          تعداد پیامک : 2/10)";
				}
				else if (306 < length && length <= 459)
				{
					((Control)LblCount).Text = "( تعداد کاراکتر : " + ((Control)TxtContent).Text.Length + "/459          تعداد پیامک : 3/10)";
				}
				else if (459 < length && length <= 612)
				{
					((Control)LblCount).Text = "( تعداد کاراکتر : " + ((Control)TxtContent).Text.Length + "/612          تعداد پیامک : 4/10)";
				}
				else if (612 < length && length <= 765)
				{
					((Control)LblCount).Text = "( تعداد کاراکتر : " + ((Control)TxtContent).Text.Length + "/765          تعداد پیامک : 5/10)";
				}
				else if (765 < length && length <= 918)
				{
					((Control)LblCount).Text = "( تعداد کاراکتر : " + ((Control)TxtContent).Text.Length + "/918          تعداد پیامک : 6/10)";
				}
				else if (918 < length && length <= 1071)
				{
					((Control)LblCount).Text = "( تعداد کاراکتر : " + ((Control)TxtContent).Text.Length + "/1071         تعداد پیامک : 7/10)";
				}
				else if (1071 < length && length <= 1224)
				{
					((Control)LblCount).Text = "( تعداد کاراکتر : " + ((Control)TxtContent).Text.Length + "/1224         تعداد پیامک : 8/10)";
				}
				else if (1224 < length && length <= 1377)
				{
					((Control)LblCount).Text = "( تعداد کاراکتر : " + ((Control)TxtContent).Text.Length + "/1377         تعداد پیامک : 9/10)";
				}
				else if (1377 < length && length <= 1530)
				{
					((Control)LblCount).Text = "( تعداد کاراکتر : " + ((Control)TxtContent).Text.Length + "/1530         تعداد پیامک : 10/10)";
				}
				else
				{
					((Control)LblCount).Text = "( تعداد کاراکتر : " + ((Control)TxtContent).Text.Length + "         تعداد پیامک : بیش از 10 پیامک لاتین)";
				}
			}
			else if (length <= 70)
			{
				((Control)LblCount).Text = "( تعداد کاراکتر : " + ((Control)TxtContent).Text.Length + "/70          تعداد پیامک : 1/10)";
			}
			else if (70 < length && length <= 134)
			{
				((Control)LblCount).Text = "( تعداد کاراکتر : " + ((Control)TxtContent).Text.Length + "/134         تعداد پیامک : 2/10)";
			}
			else if (134 < length && length <= 201)
			{
				((Control)LblCount).Text = "( تعداد کاراکتر : " + ((Control)TxtContent).Text.Length + "/201         تعداد پیامک : 3/10)";
			}
			else if (201 < length && length <= 268)
			{
				((Control)LblCount).Text = "( تعداد کاراکتر : " + ((Control)TxtContent).Text.Length + "/268         تعداد پیامک : 4/10)";
			}
			else if (268 < length && length <= 335)
			{
				((Control)LblCount).Text = "( تعداد کاراکتر : " + ((Control)TxtContent).Text.Length + "/335         تعداد پیامک : 5/10)";
			}
			else if (335 < length && length <= 402)
			{
				((Control)LblCount).Text = "( تعداد کاراکتر : " + ((Control)TxtContent).Text.Length + "/402         تعداد پیامک : 6/10)";
			}
			else if (402 < length && length <= 469)
			{
				((Control)LblCount).Text = "( تعداد کاراکتر : " + ((Control)TxtContent).Text.Length + "/469         تعداد پیامک : 7/10)";
			}
			else if (469 < length && length <= 536)
			{
				((Control)LblCount).Text = "( تعداد کاراکتر : " + ((Control)TxtContent).Text.Length + "/536         تعداد پیامک : 8/10)";
			}
			else if (536 < length && length <= 603)
			{
				((Control)LblCount).Text = "( تعداد کاراکتر : " + ((Control)TxtContent).Text.Length + "/603         تعداد پیامک : 9/10)";
			}
			else if (603 < length && length <= 670)
			{
				((Control)LblCount).Text = "( تعداد کاراکتر : " + ((Control)TxtContent).Text.Length + "/670         تعداد پیامک : 10/10)";
			}
			else
			{
				((Control)LblCount).Text = "( تعداد کاراکتر : " + ((Control)TxtContent).Text.Length + "         تعداد پیامک : بیش از 10 پیامک فارسی)";
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RefreshLabel", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSMS.cs", 150);
		}
	}

	public bool IsLatin(string txt)
	{
		try
		{
			int i = 0;
			for (int num = txt.Length - 1; i <= num; i++)
			{
				string value = txt.Substring(i, 1).ToUpper();
				if (!".,-!?@~_\"\\/&';^|:(){}[]=€$£§%¥*+#¿¡¤¢«»®©°<> ABCDEFGHIJKLMNOPQRSTUVWXYZ abcdefghijklmnopqrstuvwxyz01234567890".Contains(value))
				{
					return false;
				}
			}
			return true;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "IsLatin", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSMS.cs", 172);
			return false;
		}
	}

	private void TxtContent_GotFocus(object sender, EventArgs e)
	{
		try
		{
			((Control)TxtContent).BackColor = Color.Yellow;
			((TextBoxBase)TxtContent).SelectAll();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TxtContent_GotFocus", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSMS.cs", 209);
		}
	}

	private void TxtContent_LostFocus(object sender, EventArgs e)
	{
		try
		{
			((Control)TxtContent).BackColor = Color.White;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TxtContent_LostFocus", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSMS.cs", 220);
		}
	}

	private void TxtContent_TextChanged(object sender, EventArgs e)
	{
		try
		{
			RefreshLabel();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TxtContent_TextChanged", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSMS.cs", 232);
		}
	}

	private void TXTMessage_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Invalid comparison between Unknown and I4
		try
		{
			Keys keyCode = e.KeyCode;
			if ((int)keyCode == 113)
			{
				e.Handled = true;
				SendKeys.Send("[field2]");
			}
			else if ((int)keyCode == 114)
			{
				e.Handled = true;
				SendKeys.Send("[field3]");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TXTMessage_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\UCSMS\\UCSMS.cs", 259);
		}
	}

	protected override void Dispose(bool disposing)
	{
		components?.Dispose();
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Expected O, but got Unknown
		LblCount = new Label();
		TxtContent = new TextBoxX();
		((Control)this).SuspendLayout();
		((Control)LblCount).Dock = (DockStyle)1;
		((Control)LblCount).Location = new Point(0, 0);
		((Control)LblCount).Name = "LblCount";
		((Control)LblCount).RightToLeft = (RightToLeft)0;
		((Control)LblCount).Size = new Size(316, 23);
		((Control)LblCount).TabIndex = 10;
		((Control)LblCount).Text = "(0\\160             0\\10)";
		TxtContent.Border.Class = "TextBoxBorder";
		TxtContent.Border.CornerType = eCornerType.Square;
		((Control)TxtContent).Dock = (DockStyle)5;
		((Control)TxtContent).Location = new Point(0, 23);
		((TextBoxBase)TxtContent).Multiline = true;
		((Control)TxtContent).Name = "TxtContent";
		((TextBox)TxtContent).ScrollBars = (ScrollBars)2;
		((Control)TxtContent).Size = new Size(316, 244);
		((Control)TxtContent).TabIndex = 9;
		((Control)TxtContent).TextChanged += TxtContent_TextChanged;
		((Control)TxtContent).Enter += TxtContent_GotFocus;
		((Control)TxtContent).KeyDown += new KeyEventHandler(TXTMessage_KeyDown);
		((Control)TxtContent).Leave += TxtContent_LostFocus;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)TxtContent);
		((Control)this).Controls.Add((Control)(object)LblCount);
		((Control)this).Name = "UCSMS";
		((Control)this).Size = new Size(316, 267);
		((Control)this).ResumeLayout(false);
	}
}
