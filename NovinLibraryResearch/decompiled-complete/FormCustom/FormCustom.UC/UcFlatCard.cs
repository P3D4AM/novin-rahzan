using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using PacketParser.Entities;

namespace FormCustom.UC;

public class UcFlatCard : UserControl
{
	private FlatCardType _type = FlatCardType.Light;

	private IContainer components;

	private Label lblTitle;

	private SplitContainer splitContainer1;

	private Label lblValue;

	internal ContextMenuStrip CMA;

	internal ToolStripMenuItem cmToday;

	internal ToolStripMenuItem cmLast30;

	internal ToolStripMenuItem cmAllGardesh;

	private ToolStripSeparator toolStripMenuItem1;

	private ToolStripMenuItem btnMore;

	public string Title
	{
		get
		{
			return ((Control)lblTitle).Text;
		}
		set
		{
			((Control)lblTitle).Text = value;
		}
	}

	public string Value
	{
		get
		{
			return ((Control)lblValue).Text;
		}
		set
		{
			((Control)lblValue).Text = value;
		}
	}

	public Guid Guid { get; set; }

	public FlatCardType CardType
	{
		get
		{
			return _type;
		}
		set
		{
			_type = value;
			if (_type == FlatCardType.Light)
			{
				((Control)this).BackColor = Color.White;
			}
			else
			{
				((Control)this).BackColor = Color.FromArgb(210, 221, 251);
			}
		}
	}

	public event Action<Guid> TodayGardeshEvent;

	public event Action<Guid> Last30GardeshEvent;

	public event Action<Guid> AllGardeshEvent;

	public event Action<Guid> MoreGardeshEvent;

	public UcFlatCard()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	private void lblTitle_Click(object sender, EventArgs e)
	{
		try
		{
			((ToolStripDropDown)CMA).Show(Cursor.Position.X, Cursor.Position.Y);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "lblTitle_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcFlatCard.cs", 47);
		}
	}

	private void cmToday_Click(object sender, EventArgs e)
	{
		this.TodayGardeshEvent?.Invoke(Guid);
	}

	private void cmLast30_Click(object sender, EventArgs e)
	{
		this.Last30GardeshEvent?.Invoke(Guid);
	}

	private void cmAllGardesh_Click(object sender, EventArgs e)
	{
		this.AllGardeshEvent?.Invoke(Guid);
	}

	private void btnMore_Click(object sender, EventArgs e)
	{
		this.MoreGardeshEvent?.Invoke(Guid);
	}

	protected override void Dispose(bool disposing)
	{
		components?.Dispose();
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
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Expected O, but got Unknown
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Expected O, but got Unknown
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e1: Expected O, but got Unknown
		//IL_043f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0449: Expected O, but got Unknown
		//IL_04a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b1: Expected O, but got Unknown
		//IL_053c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0546: Expected O, but got Unknown
		//IL_05de: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e8: Expected O, but got Unknown
		//IL_05ed: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		lblTitle = new Label();
		splitContainer1 = new SplitContainer();
		lblValue = new Label();
		CMA = new ContextMenuStrip(components);
		cmToday = new ToolStripMenuItem();
		cmLast30 = new ToolStripMenuItem();
		cmAllGardesh = new ToolStripMenuItem();
		toolStripMenuItem1 = new ToolStripSeparator();
		btnMore = new ToolStripMenuItem();
		((ISupportInitialize)splitContainer1).BeginInit();
		((Control)splitContainer1.Panel1).SuspendLayout();
		((Control)splitContainer1.Panel2).SuspendLayout();
		((Control)splitContainer1).SuspendLayout();
		((Control)CMA).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)lblTitle).Cursor = Cursors.Hand;
		((Control)lblTitle).Dock = (DockStyle)5;
		((Control)lblTitle).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblTitle).ForeColor = Color.Black;
		((Control)lblTitle).Location = new Point(0, 0);
		((Control)lblTitle).Margin = new Padding(5, 0, 5, 0);
		((Control)lblTitle).Name = "lblTitle";
		((Control)lblTitle).Padding = new Padding(0, 0, 5, 0);
		((Control)lblTitle).Size = new Size(141, 38);
		((Control)lblTitle).TabIndex = 1;
		lblTitle.TextAlign = (ContentAlignment)16;
		((Control)lblTitle).Click += lblTitle_Click;
		splitContainer1.Dock = (DockStyle)5;
		((Control)splitContainer1).Location = new Point(0, 0);
		((Control)splitContainer1).Name = "splitContainer1";
		((Control)splitContainer1.Panel1).Controls.Add((Control)(object)lblTitle);
		((Control)splitContainer1.Panel1).RightToLeft = (RightToLeft)1;
		((Control)splitContainer1.Panel2).Controls.Add((Control)(object)lblValue);
		((Control)splitContainer1.Panel2).RightToLeft = (RightToLeft)1;
		((Control)splitContainer1).Size = new Size(296, 38);
		splitContainer1.SplitterDistance = 141;
		((Control)splitContainer1).TabIndex = 2;
		((Control)splitContainer1).Click += lblTitle_Click;
		((Control)lblValue).Cursor = Cursors.Hand;
		((Control)lblValue).Dock = (DockStyle)5;
		((Control)lblValue).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblValue).ForeColor = Color.Black;
		((Control)lblValue).Location = new Point(0, 0);
		((Control)lblValue).Margin = new Padding(5, 0, 5, 0);
		((Control)lblValue).Name = "lblValue";
		((Control)lblValue).Padding = new Padding(0, 0, 5, 0);
		((Control)lblValue).Size = new Size(151, 38);
		((Control)lblValue).TabIndex = 2;
		lblValue.TextAlign = (ContentAlignment)64;
		((Control)lblValue).Click += lblTitle_Click;
		((ToolStrip)CMA).Items.AddRange((ToolStripItem[])(object)new ToolStripItem[5]
		{
			(ToolStripItem)cmToday,
			(ToolStripItem)cmLast30,
			(ToolStripItem)cmAllGardesh,
			(ToolStripItem)toolStripMenuItem1,
			(ToolStripItem)btnMore
		});
		((Control)CMA).Name = "CMA";
		((Control)CMA).RightToLeft = (RightToLeft)1;
		((Control)CMA).Size = new Size(213, 128);
		((ToolStripItem)cmToday).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((ToolStripItem)cmToday).Name = "cmToday";
		((ToolStripItem)cmToday).Size = new Size(212, 24);
		((ToolStripItem)cmToday).Text = "گزارش گردش عملکرد امروز";
		((ToolStripItem)cmToday).Click += cmToday_Click;
		((ToolStripItem)cmLast30).Font = new Font("B Yekan", 9.75f);
		((ToolStripItem)cmLast30).Name = "cmLast30";
		((ToolStripItem)cmLast30).Size = new Size(212, 24);
		((ToolStripItem)cmLast30).Text = "گزارش 30 گردش آخر";
		((ToolStripItem)cmLast30).Click += cmLast30_Click;
		((ToolStripItem)cmAllGardesh).Font = new Font("B Yekan", 9.75f);
		((ToolStripItem)cmAllGardesh).Name = "cmAllGardesh";
		((ToolStripItem)cmAllGardesh).Size = new Size(212, 24);
		((ToolStripItem)cmAllGardesh).Text = "گزارش کلیه گردشها";
		((ToolStripItem)cmAllGardesh).Click += cmAllGardesh_Click;
		((ToolStripItem)toolStripMenuItem1).Name = "toolStripMenuItem1";
		((ToolStripItem)toolStripMenuItem1).Size = new Size(209, 6);
		((ToolStripItem)btnMore).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((ToolStripItem)btnMore).Name = "btnMore";
		((ToolStripItem)btnMore).Size = new Size(212, 24);
		((ToolStripItem)btnMore).Text = "نمایش سایر فیلترها";
		((ToolStripItem)btnMore).Click += btnMore_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)splitContainer1);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UcFlatCard";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(296, 38);
		((Control)splitContainer1.Panel1).ResumeLayout(false);
		((Control)splitContainer1.Panel2).ResumeLayout(false);
		((ISupportInitialize)splitContainer1).EndInit();
		((Control)splitContainer1).ResumeLayout(false);
		((Control)CMA).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
