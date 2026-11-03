using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using PacketParser.Entities;

namespace FormCustom.UC;

public class UcShowColor : UserControl
{
	private IContainer components;

	internal Label lblColorTemplate;

	public int Id { get; private set; }

	public Guid Guid { get; private set; }

	public event Action<int, Guid> OnClick;

	public void Init(int id, string tip, string hexColor)
	{
		try
		{
			Id = id;
			Init(tip, hexColor);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Init", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcShowColor.cs", 22);
		}
	}

	public void Init(Guid guid, string tip, string hexColor)
	{
		try
		{
			Guid = guid;
			Init(tip, hexColor);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Init", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcShowColor.cs", 31);
		}
	}

	private void Init(string tip, string hexColor)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			((Control)lblColorTemplate).BackColor = hexColor.RGBConverter();
			if (!string.IsNullOrEmpty(tip))
			{
				new ToolTip().SetToolTip((Control)(object)lblColorTemplate, tip);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Init", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcShowColor.cs", 41);
		}
	}

	public UcShowColor()
	{
		InitializeComponent();
	}

	private void lblColorTemplate_Click(object sender, EventArgs e)
	{
		this.OnClick?.Invoke(Id, Guid);
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
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Expected O, but got Unknown
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		lblColorTemplate = new Label();
		((Control)this).SuspendLayout();
		((Control)lblColorTemplate).BackColor = Color.Transparent;
		lblColorTemplate.BorderStyle = (BorderStyle)1;
		((Control)lblColorTemplate).Cursor = Cursors.Hand;
		((Control)lblColorTemplate).Dock = (DockStyle)5;
		((Control)lblColorTemplate).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblColorTemplate).Location = new Point(0, 0);
		((Control)lblColorTemplate).Name = "lblColorTemplate";
		((Control)lblColorTemplate).Size = new Size(25, 23);
		((Control)lblColorTemplate).TabIndex = 155;
		lblColorTemplate.TextAlign = (ContentAlignment)16;
		((Control)lblColorTemplate).Click += lblColorTemplate_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)lblColorTemplate);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UcShowColor";
		((Control)this).Size = new Size(25, 23);
		((Control)this).ResumeLayout(false);
	}
}
