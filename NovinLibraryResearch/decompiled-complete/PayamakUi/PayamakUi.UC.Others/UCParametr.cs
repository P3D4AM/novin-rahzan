using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using PacketParser.Entities;

namespace PayamakUi.UC.Others;

public class UCParametr : UserControl
{
	private IContainer components;

	internal GroupPanel GpParametr;

	internal ListBox LBParametr;

	internal BindingSource PhoneBookBindingSource;

	public string LastSelectedParametrText { get; private set; }

	public event EventHandler<string> OnParameterSelected;

	public UCParametr()
	{
		InitializeComponent();
	}

	private void LBParametr_Click(object sender, EventArgs e)
	{
		try
		{
			if (((ListControl)LBParametr).SelectedIndex >= 0)
			{
				LastSelectedParametrText = " " + LBParametr.Items[((ListControl)LBParametr).SelectedIndex].ToString() + " ";
				this.OnParameterSelected?.Invoke(this, LastSelectedParametrText);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LBParametr_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\UC\\Others\\UCParametr.cs", 33);
		}
	}

	protected override void Dispose(bool disposing)
	{
		components?.Dispose();
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		components = new Container();
		GpParametr = new GroupPanel();
		LBParametr = new ListBox();
		PhoneBookBindingSource = new BindingSource(components);
		((Control)GpParametr).SuspendLayout();
		((ISupportInitialize)PhoneBookBindingSource).BeginInit();
		((Control)this).SuspendLayout();
		((Control)GpParametr).BackColor = Color.Transparent;
		GpParametr.CanvasColor = Color.Transparent;
		GpParametr.ColorSchemeStyle = eDotNetBarStyle.Office2007;
		((Control)GpParametr).Controls.Add((Control)(object)LBParametr);
		GpParametr.DisabledBackColor = Color.Empty;
		((Control)GpParametr).Dock = (DockStyle)5;
		((Control)GpParametr).Location = new Point(0, 0);
		((Control)GpParametr).Name = "GpParametr";
		((Control)GpParametr).Size = new Size(139, 246);
		GpParametr.Style.BackColor2SchemePart = eColorSchemePart.PanelBackground2;
		GpParametr.Style.BackColorGradientAngle = 90;
		GpParametr.Style.BackColorSchemePart = eColorSchemePart.PanelBackground;
		GpParametr.Style.BorderBottom = eStyleBorderType.Solid;
		GpParametr.Style.BorderBottomWidth = 1;
		GpParametr.Style.BorderColorSchemePart = eColorSchemePart.PanelBorder;
		GpParametr.Style.BorderLeft = eStyleBorderType.Solid;
		GpParametr.Style.BorderLeftWidth = 1;
		GpParametr.Style.BorderRight = eStyleBorderType.Solid;
		GpParametr.Style.BorderRightWidth = 1;
		GpParametr.Style.BorderTop = eStyleBorderType.Solid;
		GpParametr.Style.BorderTopWidth = 1;
		GpParametr.Style.CornerDiameter = 4;
		GpParametr.Style.CornerType = eCornerType.Rounded;
		GpParametr.Style.TextAlignment = eStyleTextAlignment.Center;
		GpParametr.Style.TextColorSchemePart = eColorSchemePart.PanelText;
		GpParametr.Style.TextLineAlignment = eStyleTextAlignment.Near;
		GpParametr.StyleMouseDown.CornerType = eCornerType.Square;
		GpParametr.StyleMouseOver.CornerType = eCornerType.Square;
		((Control)GpParametr).TabIndex = 0;
		((Control)GpParametr).Text = "پارامتـــــر ها";
		GpParametr.TitleImagePosition = eTitleImagePosition.Right;
		((Control)LBParametr).Dock = (DockStyle)5;
		((ListControl)LBParametr).FormattingEnabled = true;
		LBParametr.Items.AddRange(new object[13]
		{
			"نام", "نام خانوادگی", "عنوان", "کد ملی", "کدپستی", "شماره ثابت", "شماره همراه", "شماره فکس", "کدپستی", "تاریخ تولد",
			"تاریخ انقضای اعتبار", "نام شرکت", "آدرس"
		});
		((Control)LBParametr).Location = new Point(0, 0);
		((Control)LBParametr).Name = "LBParametr";
		((Control)LBParametr).RightToLeft = (RightToLeft)1;
		((Control)LBParametr).Size = new Size(133, 225);
		((Control)LBParametr).TabIndex = 0;
		LBParametr.Click += LBParametr_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)GpParametr);
		((Control)this).Name = "UCParametr";
		((Control)this).Size = new Size(139, 246);
		((Control)GpParametr).ResumeLayout(false);
		((ISupportInitialize)PhoneBookBindingSource).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
