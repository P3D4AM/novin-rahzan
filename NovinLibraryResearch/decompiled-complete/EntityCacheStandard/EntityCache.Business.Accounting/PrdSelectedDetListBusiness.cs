using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Assistance;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace EntityCache.Business.Accounting;

public class PrdSelectedDetListBusiness : IPrdSelectedDetList, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext, INotifyPropertyChanged
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private Guid prdGuid;

	private bool isChecked;

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; } = true;


	public ProductCompactBusiness Prd { get; private set; }

	public Guid MasterGuid { get; set; }

	public string Description { get; set; } = "";


	public bool IsChecked
	{
		get
		{
			return isChecked;
		}
		set
		{
			isChecked = value;
			if (this.PropertyChanged != null && UiInvokerHolder.Instance != null)
			{
				UiInvokerHolder.Instance.RunOnUiThread(delegate
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsChecked"));
				}, "IsChecked", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Product\\PrdSelectedDetListBusiness.cs");
			}
		}
	}

	public string PrdName => Prd?.Name;

	public string PrdCode => Prd?.IdProduct;

	public string ContextGuid { get; set; }

	public event PropertyChangedEventHandler PropertyChanged;

	public Guid GetPrdGuid()
	{
		return prdGuid;
	}

	public async Task SetPrdGuid(Guid value, EnSystem system, CancellationToken token = default(CancellationToken))
	{
		prdGuid = value;
		Prd = await ProductCompactBusiness.GetAsync(ContextGuid, value, system, null, "", token, true);
	}

	public PrdSelectedDetListBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
