using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormCustom.Properties;
using PacketParser;
using PacketParser.Entities;

namespace FormCustom.UC.SevenSegment;

public class UcSevenSegment : UserControl
{
	private CancellationTokenSource _loadImageToken;

	private bool _isDisposed;

	private decimal weight;

	private IContainer components;

	private TableLayoutPanel tableLayoutPanel1;

	private PictureBox p1;

	private PictureBox p2;

	private TableLayoutPanel tableLayoutPanel2;

	private PictureBox p3;

	private PictureBox p4;

	private PictureBox p5;

	public bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return ((Control)this).IsDisposed;
			}
			return true;
		}
	}

	public decimal Weight
	{
		get
		{
			return weight;
		}
		set
		{
			if (!IsDisposed)
			{
				weight = Math.Round(value, 3);
				if (weight > 100m)
				{
					throw new OutOfMemoryException("حداکثر وزن قابل نمایش  99 کیلوگرم میباشد- وزن دریافتی:");
				}
				if (weight <= -10m)
				{
					throw new OutOfMemoryException("حداقل وزن قابل نمایش در حالت منفی 9 کیلوگرم میباشد");
				}
				Image img1 = null;
				Image img2 = null;
				Image img3 = null;
				Image img4 = null;
				Image img5 = null;
				string text = weight.ToString();
				while (text.Replace(PacketParser.TextManager.Instance.DecimalPoint, "").Length < 5)
				{
					text = ((text.IndexOf(PacketParser.TextManager.Instance.DecimalPoint) >= 2) ? (text + "0") : ("0" + text));
				}
				img1 = (Image)((weight < 0m) ? ((object)FormCustom.Properties.Resources.SevenSegment_Mines) : ((object)GetImage(text.Replace(PacketParser.TextManager.Instance.DecimalPoint, "")[0])));
				img2 = GetImage(text.Replace(PacketParser.TextManager.Instance.DecimalPoint, "")[1], useDot: true);
				img3 = GetImage(text.Replace(PacketParser.TextManager.Instance.DecimalPoint, "")[2]);
				img4 = GetImage(text.Replace(PacketParser.TextManager.Instance.DecimalPoint, "")[3]);
				img5 = GetImage(text.Replace(PacketParser.TextManager.Instance.DecimalPoint, "")[4]);
				_loadImageToken?.Cancel();
				_loadImageToken = new CancellationTokenSource();
				CancellationToken token = _loadImageToken.Token;
				Task.Run(() => UpdateImagesAsync(img1, img2, img3, img4, img5, token));
			}
		}
	}

	public UcSevenSegment()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	private async Task UpdateImagesAsync(Image img1, Image img2, Image img3, Image img4, Image img5, CancellationToken token)
	{
		try
		{
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(100);
				if (IsDisposed || token.IsCancellationRequested)
				{
					return;
				}
			}
			((Control)this).BeginInvoke((Delegate)(Action)delegate
			{
				try
				{
					p1.Image = img1;
					p2.Image = img2;
					p3.Image = img3;
					p4.Image = img4;
					p5.Image = img5;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "UpdateImagesAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\SevenSegment\\UcSevenSegment.cs", 76);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateImagesAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\SevenSegment\\UcSevenSegment.cs", 79);
		}
	}

	private Image GetImage(char number, bool useDot = false)
	{
		if (number == '0' && useDot)
		{
			return (Image)(object)FormCustom.Properties.Resources.SevenSegmentDot_0;
		}
		if (number == '1' && useDot)
		{
			return (Image)(object)FormCustom.Properties.Resources.SevenSegmentDot_1;
		}
		if (number == '2' && useDot)
		{
			return (Image)(object)FormCustom.Properties.Resources.SevenSegmentDot_2;
		}
		if (number == '3' && useDot)
		{
			return (Image)(object)FormCustom.Properties.Resources.SevenSegmentDot_3;
		}
		if (number == '4' && useDot)
		{
			return (Image)(object)FormCustom.Properties.Resources.SevenSegmentDot_4;
		}
		if (number == '5' && useDot)
		{
			return (Image)(object)FormCustom.Properties.Resources.SevenSegmentDot_5;
		}
		if (number == '6' && useDot)
		{
			return (Image)(object)FormCustom.Properties.Resources.SevenSegmentDot_6;
		}
		if (number == '7' && useDot)
		{
			return (Image)(object)FormCustom.Properties.Resources.SevenSegmentDot_7;
		}
		if (number == '8' && useDot)
		{
			return (Image)(object)FormCustom.Properties.Resources.SevenSegmentDot_8;
		}
		if (number == '9' && useDot)
		{
			return (Image)(object)FormCustom.Properties.Resources.SevenSegmentDot_9;
		}
		return (Image)(number switch
		{
			'0' => FormCustom.Properties.Resources.SevenSegment_0, 
			'1' => FormCustom.Properties.Resources.SevenSegment_1, 
			'2' => FormCustom.Properties.Resources.SevenSegment_2, 
			'3' => FormCustom.Properties.Resources.SevenSegment_3, 
			'4' => FormCustom.Properties.Resources.SevenSegment_4, 
			'5' => FormCustom.Properties.Resources.SevenSegment_5, 
			'6' => FormCustom.Properties.Resources.SevenSegment_6, 
			'7' => FormCustom.Properties.Resources.SevenSegment_7, 
			'8' => FormCustom.Properties.Resources.SevenSegment_8, 
			'9' => FormCustom.Properties.Resources.SevenSegment_9, 
			_ => throw new InvalidOperationException($"value({number}) is not valid!!"), 
		});
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_loadImageToken?.Cancel();
				_loadImageToken?.Dispose();
				_loadImageToken = null;
				components?.Dispose();
				((ContainerControl)this).Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\SevenSegment\\UcSevenSegment.cs", 122);
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
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Expected O, but got Unknown
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Expected O, but got Unknown
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Expected O, but got Unknown
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Expected O, but got Unknown
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Expected O, but got Unknown
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Expected O, but got Unknown
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Expected O, but got Unknown
		tableLayoutPanel1 = new TableLayoutPanel();
		tableLayoutPanel2 = new TableLayoutPanel();
		p4 = new PictureBox();
		p5 = new PictureBox();
		p3 = new PictureBox();
		p2 = new PictureBox();
		p1 = new PictureBox();
		((Control)tableLayoutPanel1).SuspendLayout();
		((Control)tableLayoutPanel2).SuspendLayout();
		((ISupportInitialize)p4).BeginInit();
		((ISupportInitialize)p5).BeginInit();
		((ISupportInitialize)p3).BeginInit();
		((ISupportInitialize)p2).BeginInit();
		((ISupportInitialize)p1).BeginInit();
		((Control)this).SuspendLayout();
		tableLayoutPanel1.ColumnCount = 3;
		tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle((SizeType)2, 50f));
		tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle((SizeType)2, 25f));
		tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle((SizeType)2, 25f));
		tableLayoutPanel1.Controls.Add((Control)(object)tableLayoutPanel2, 0, 0);
		tableLayoutPanel1.Controls.Add((Control)(object)p2, 1, 0);
		tableLayoutPanel1.Controls.Add((Control)(object)p1, 2, 0);
		((Control)tableLayoutPanel1).Dock = (DockStyle)5;
		((Control)tableLayoutPanel1).Location = new Point(0, 0);
		((Control)tableLayoutPanel1).Name = "tableLayoutPanel1";
		tableLayoutPanel1.RowCount = 1;
		tableLayoutPanel1.RowStyles.Add(new RowStyle((SizeType)2, 100f));
		((Control)tableLayoutPanel1).Size = new Size(1253, 150);
		((Control)tableLayoutPanel1).TabIndex = 0;
		tableLayoutPanel2.ColumnCount = 3;
		tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle((SizeType)2, 33.33333f));
		tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle((SizeType)2, 33.33334f));
		tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle((SizeType)2, 33.33334f));
		tableLayoutPanel2.Controls.Add((Control)(object)p4, 0, 1);
		tableLayoutPanel2.Controls.Add((Control)(object)p5, 0, 1);
		tableLayoutPanel2.Controls.Add((Control)(object)p3, 2, 1);
		((Control)tableLayoutPanel2).Dock = (DockStyle)5;
		((Control)tableLayoutPanel2).Location = new Point(3, 3);
		((Control)tableLayoutPanel2).Name = "tableLayoutPanel2";
		tableLayoutPanel2.RowCount = 2;
		tableLayoutPanel2.RowStyles.Add(new RowStyle((SizeType)2, 20f));
		tableLayoutPanel2.RowStyles.Add(new RowStyle((SizeType)2, 80f));
		((Control)tableLayoutPanel2).Size = new Size(620, 144);
		((Control)tableLayoutPanel2).TabIndex = 2;
		((Control)p4).Dock = (DockStyle)5;
		p4.Image = (Image)(object)FormCustom.Properties.Resources.SevenSegment_0;
		((Control)p4).Location = new Point(209, 31);
		((Control)p4).Name = "p4";
		((Control)p4).Size = new Size(200, 110);
		p4.SizeMode = (PictureBoxSizeMode)4;
		p4.TabIndex = 8;
		p4.TabStop = false;
		((Control)p5).Dock = (DockStyle)5;
		p5.Image = (Image)(object)FormCustom.Properties.Resources.SevenSegment_0;
		((Control)p5).Location = new Point(3, 31);
		((Control)p5).Name = "p5";
		((Control)p5).Size = new Size(200, 110);
		p5.SizeMode = (PictureBoxSizeMode)4;
		p5.TabIndex = 7;
		p5.TabStop = false;
		((Control)p3).Dock = (DockStyle)5;
		p3.Image = (Image)(object)FormCustom.Properties.Resources.SevenSegment_0;
		((Control)p3).Location = new Point(415, 31);
		((Control)p3).Name = "p3";
		((Control)p3).Size = new Size(202, 110);
		p3.SizeMode = (PictureBoxSizeMode)4;
		p3.TabIndex = 6;
		p3.TabStop = false;
		((Control)p2).Dock = (DockStyle)5;
		p2.Image = (Image)(object)FormCustom.Properties.Resources.SevenSegment_0;
		((Control)p2).Location = new Point(629, 3);
		((Control)p2).Name = "p2";
		((Control)p2).Size = new Size(307, 144);
		p2.SizeMode = (PictureBoxSizeMode)4;
		p2.TabIndex = 3;
		p2.TabStop = false;
		((Control)p1).Dock = (DockStyle)5;
		p1.Image = (Image)(object)FormCustom.Properties.Resources.SevenSegment_0;
		((Control)p1).Location = new Point(942, 3);
		((Control)p1).Name = "p1";
		((Control)p1).Size = new Size(308, 144);
		p1.SizeMode = (PictureBoxSizeMode)4;
		p1.TabIndex = 4;
		p1.TabStop = false;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)tableLayoutPanel1);
		((Control)this).Name = "UcSevenSegment";
		((Control)this).Size = new Size(1253, 150);
		((Control)tableLayoutPanel1).ResumeLayout(false);
		((Control)tableLayoutPanel2).ResumeLayout(false);
		((ISupportInitialize)p4).EndInit();
		((ISupportInitialize)p5).EndInit();
		((ISupportInitialize)p3).EndInit();
		((ISupportInitialize)p2).EndInit();
		((ISupportInitialize)p1).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
