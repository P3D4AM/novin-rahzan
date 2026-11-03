using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Assistance;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Selection;

namespace EntityCache.Business.Accounting.Selection.ProductGroup;

public class PrdGroupSelectedDetListBusiness : IPrdGroupSelectedDetList, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid, IHasContext, INotifyPropertyChanged
{
	private Guid _groupGuid;

	private bool _isChecked;

	public Guid Guid { get; set; }

	public Guid MasterGuid { get; set; }

	public string Description { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public string ContextGuid { get; set; }

	public PrdGroupBusiness Group { get; private set; }

	public bool IsChecked
	{
		get
		{
			return _isChecked;
		}
		set
		{
			_isChecked = value;
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsChecked"));
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "IsChecked", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedDetListBusiness.cs", 40);
				}
			}, "IsChecked", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\ProductGroup\\PrdGroupSelectedDetListBusiness.cs");
		}
	}

	public string GroupName => Group?.Name;

	public long GroupCode => Group?.Code ?? 0;

	public event PropertyChangedEventHandler PropertyChanged;

	public Guid GetGroupGuid()
	{
		return _groupGuid;
	}

	public async Task SetGroupGuid(Guid value, CancellationToken token = default(CancellationToken))
	{
		_groupGuid = value;
		Group = await PrdGroupBusiness.GetAsync(ContextGuid, value, token);
	}

	public PrdGroupSelectedDetListBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
