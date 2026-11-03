using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using AccSqlServerPersistence;
using AccSqlServerPersistence.AssistanceClasses;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace FormCustom.BackUp.Forms.SqlQuery;

public class FRMQuery : AccountingBaseForm
{
	private ENSource _source;

	private bool _isDisposed;

	private IContainer components;

	internal TextBox TxtSQLQuery;

	internal TextBox TXTConnectionString;

	internal Button BTNRun;

	public new bool IsDisposed
	{
		get
		{
			if (!base.IsDisposed)
			{
				return _isDisposed;
			}
			return true;
		}
	}

	public FRMQuery(ENSource source, string connectionString, string context)
		: base(context)
	{
		InitializeComponent();
		_source = source;
		((Control)TXTConnectionString).Text = connectionString;
		((Control)TXTConnectionString).Enabled = false;
		((TextBoxBase)TXTConnectionString).ReadOnly = true;
	}

	private async void BTNRun_Click(object sender, EventArgs e)
	{
		_ = 1;
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = await RunScript.Instance.RunAsync(BackupSettingsRepo.GetInstance(base.ContextGuid), "customQuery_frmQuery", checkCrcBeforeExecution: false, _source, ((Control)TxtSQLQuery).Text, ((Control)TXTConnectionString).Text);
			if (returnedSaveFuncInfo.HasError)
			{
				await this.ShowReturnedSavedFuncInfoAsync(returnedSaveFuncInfo, "خطا در اجرای کوئری نوشته شده توسط کاربر", "BTNRun_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\SqlQuery\\FRMQuery.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BTNRun_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\SqlQuery\\FRMQuery.cs", 33);
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				components?.Dispose();
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\BackUp\\Forms\\SqlQuery\\FRMQuery.cs", 54);
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
		TxtSQLQuery = new TextBox();
		TXTConnectionString = new TextBox();
		BTNRun = new Button();
		((Control)this).SuspendLayout();
		((Control)TxtSQLQuery).Anchor = (AnchorStyles)15;
		((Control)TxtSQLQuery).Location = new Point(12, 67);
		((TextBoxBase)TxtSQLQuery).Multiline = true;
		((Control)TxtSQLQuery).Name = "TxtSQLQuery";
		((Control)TxtSQLQuery).RightToLeft = (RightToLeft)0;
		((Control)TxtSQLQuery).Size = new Size(452, 225);
		((Control)TxtSQLQuery).TabIndex = 5;
		((Control)TXTConnectionString).Anchor = (AnchorStyles)13;
		((Control)TXTConnectionString).Location = new Point(12, 12);
		((Control)TXTConnectionString).Name = "TXTConnectionString";
		((Control)TXTConnectionString).RightToLeft = (RightToLeft)0;
		((Control)TXTConnectionString).Size = new Size(452, 20);
		((Control)TXTConnectionString).TabIndex = 4;
		((Control)TXTConnectionString).Text = "Data Source=.\\SQLEXPRESS;Initial Catalog=Novin1;Integrated Security=True";
		((Control)BTNRun).Anchor = (AnchorStyles)13;
		((Control)BTNRun).Location = new Point(12, 38);
		((Control)BTNRun).Name = "BTNRun";
		((Control)BTNRun).Size = new Size(452, 23);
		((Control)BTNRun).TabIndex = 3;
		((Control)BTNRun).Text = "[اجرا]";
		((ButtonBase)BTNRun).UseVisualStyleBackColor = true;
		((Control)BTNRun).Click += BTNRun_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(476, 304);
		((Control)this).Controls.Add((Control)(object)TxtSQLQuery);
		((Control)this).Controls.Add((Control)(object)TXTConnectionString);
		((Control)this).Controls.Add((Control)(object)BTNRun);
		((Control)this).Name = "FRMQuery";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Text = "[اجرای کوئری] SQL";
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
