using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using FormCustom.Forms;
using FormCustom.Properties;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace FormCustom.UC;

public class UcPercentAmount : UserControl, IHasContext
{
	private decimal _inputPrice;

	private IContainer components;

	private TextBox txtAmount;

	private Panel panel1;

	private PictureBox picNewForm;

	private PictureBox picDelete;

	public string ContextGuid { get; private set; }

	public decimal InputPrice => _inputPrice;

	public decimal TotalPrice { get; set; }

	public EnWageCalculationMethod? Type { get; private set; }

	public string Title { get; set; }

	public Color BackColor
	{
		get
		{
			return ((Control)picDelete).BackColor;
		}
		set
		{
			PictureBox obj = picDelete;
			Color backColor = (((Control)picNewForm).BackColor = value);
			((Control)obj).BackColor = backColor;
		}
	}

	public event Action<decimal> OnChangValue;

	public void Init(string contextGuid, EnWageCalculationMethod? type)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ContextGuid = contextGuid;
			Type = type;
			SetInputPrice(0m);
			ToolTip val = new ToolTip();
			val.SetToolTip((Control)(object)picNewForm, "درج مبلغ (Alt + C)");
			val.SetToolTip((Control)(object)picDelete, "حذف مبلغ (Alt + D)");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Init", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcPercentAmount.cs", 37);
		}
	}

	public void SetType(EnWageCalculationMethod? type)
	{
		Type = type;
	}

	public void SetInputPrice(decimal value)
	{
		try
		{
			_inputPrice = value;
			if (!Type.HasValue || Type == EnWageCalculationMethod.Price)
			{
				((Control)txtAmount).Text = value.NpToString("N0", 0);
			}
			else
			{
				((Control)txtAmount).Text = ((value <= 0m) ? value.ToString() : $"{value}%");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetInputPrice", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcPercentAmount.cs", 48);
		}
	}

	public UcPercentAmount()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	private void picDelete_Click(object sender, EventArgs e)
	{
		try
		{
			_inputPrice = default(decimal);
			((TextBoxBase)txtAmount).Clear();
			((Control)txtAmount).Text = "0";
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "picDelete_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcPercentAmount.cs", 65);
		}
	}

	private void picNewForm_Click(object sender, EventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Invalid comparison between Unknown and I4
		try
		{
			frmKeybord frmKeybord = new frmKeybord(ContextGuid, InputPrice, Type, Title);
			if ((int)((Form)frmKeybord).ShowDialog() != 1)
			{
				return;
			}
			SetInputPrice(frmKeybord.InputPrice);
			this.OnChangValue?.Invoke(InputPrice);
			if (!Type.HasValue && frmKeybord.Type.HasValue && TotalPrice != 0m && InputPrice != 0m)
			{
				if (frmKeybord.Type == EnWageCalculationMethod.Percent)
				{
					((Control)txtAmount).Text = $"({InputPrice}%) {TotalPrice * InputPrice / 100m:N0}";
				}
				else if (frmKeybord.Type == EnWageCalculationMethod.Price)
				{
					double num = (double)Math.Round(InputPrice * 100m / TotalPrice, 3);
					((Control)txtAmount).Text = string.Format("({0:N0}) {1}%", InputPrice, num.ToString().Replace(".", "/"));
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "picNewForm_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcPercentAmount.cs", 86);
		}
	}

	private void UcPercentAmount_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Invalid comparison between Unknown and I4
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Invalid comparison between Unknown and I4
		try
		{
			if (e.Alt && (int)e.KeyCode == 67)
			{
				((Control)picNewForm).PerformLayout();
			}
			if (e.Alt && (int)e.KeyCode == 68)
			{
				((Control)picDelete).PerformLayout();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UcPercentAmount_KeyDown", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcPercentAmount.cs", 95);
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
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d3: Expected O, but got Unknown
		//IL_03d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0407: Unknown result type (might be due to invalid IL or missing references)
		//IL_0411: Expected O, but got Unknown
		txtAmount = new TextBox();
		panel1 = new Panel();
		picNewForm = new PictureBox();
		picDelete = new PictureBox();
		((Control)panel1).SuspendLayout();
		((ISupportInitialize)picNewForm).BeginInit();
		((ISupportInitialize)picDelete).BeginInit();
		((Control)this).SuspendLayout();
		((Control)txtAmount).Anchor = (AnchorStyles)15;
		((Control)txtAmount).BackColor = Color.FromArgb(252, 252, 255);
		((Control)txtAmount).Enabled = false;
		((Control)txtAmount).Font = new Font("B Titr", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)178);
		((Control)txtAmount).Location = new Point(22, 1);
		((Control)txtAmount).Margin = new Padding(0);
		((Control)txtAmount).Name = "txtAmount";
		((TextBoxBase)txtAmount).ReadOnly = true;
		((Control)txtAmount).Size = new Size(170, 30);
		((Control)txtAmount).TabIndex = 83;
		txtAmount.TextAlign = (HorizontalAlignment)2;
		((Control)panel1).BackColor = Color.FromArgb(252, 252, 255);
		((Control)panel1).Controls.Add((Control)(object)picNewForm);
		((Control)panel1).Controls.Add((Control)(object)picDelete);
		((Control)panel1).Controls.Add((Control)(object)txtAmount);
		((Control)panel1).Dock = (DockStyle)5;
		((Control)panel1).Location = new Point(0, 0);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(214, 32);
		((Control)panel1).TabIndex = 84;
		((Control)picNewForm).Anchor = (AnchorStyles)7;
		((Control)picNewForm).BackColor = Color.FromArgb(252, 252, 255);
		((Control)picNewForm).Cursor = Cursors.Hand;
		picNewForm.Image = (Image)(object)FormCustom.Properties.Resources.download;
		((Control)picNewForm).Location = new Point(0, 1);
		((Control)picNewForm).Margin = new Padding(0);
		((Control)picNewForm).Name = "picNewForm";
		((Control)picNewForm).Size = new Size(22, 30);
		picNewForm.SizeMode = (PictureBoxSizeMode)4;
		picNewForm.TabIndex = 85;
		picNewForm.TabStop = false;
		((Control)picNewForm).Click += picNewForm_Click;
		((Control)picDelete).Anchor = (AnchorStyles)11;
		((Control)picDelete).BackColor = Color.FromArgb(252, 252, 255);
		((Control)picDelete).Cursor = Cursors.Hand;
		picDelete.Image = (Image)(object)FormCustom.Properties.Resources._19382678031536572522_32;
		((Control)picDelete).Location = new Point(192, 2);
		((Control)picDelete).Margin = new Padding(0);
		((Control)picDelete).Name = "picDelete";
		((Control)picDelete).Size = new Size(22, 30);
		picDelete.SizeMode = (PictureBoxSizeMode)4;
		picDelete.TabIndex = 86;
		picDelete.TabStop = false;
		((Control)picDelete).Click += picDelete_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		BackColor = Color.FromArgb(252, 252, 255);
		((Control)this).Controls.Add((Control)(object)panel1);
		((Control)this).Font = new Font("Microsoft Sans Serif", 8.249999f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)this).Margin = new Padding(3, 4, 3, 4);
		((Control)this).Name = "UcPercentAmount";
		((Control)this).Size = new Size(214, 32);
		((Control)this).KeyDown += new KeyEventHandler(UcPercentAmount_KeyDown);
		((Control)panel1).ResumeLayout(false);
		((Control)panel1).PerformLayout();
		((ISupportInitialize)picNewForm).EndInit();
		((ISupportInitialize)picDelete).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
