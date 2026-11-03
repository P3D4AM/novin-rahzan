using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Business.Products;
using EntityCache.Models.Accounting.Prd;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Product;
using PacketParser.Entities;
using PacketParser.Filters.Configs;

namespace EntityCache.Business;

public class ProductPriceReportBusiness : INotifyPropertyChanged
{
	private decimal _sell1Percent;

	private decimal _sell2Percent;

	private decimal _sell3Percent;

	private decimal _sell4Percent;

	private decimal _sell5Percent;

	private decimal _sell6Percent;

	private decimal _sell7Percent;

	private decimal _sell8Percent;

	public ProductPriceReportConfig Config { get; }

	public PriceRelationModel PriceRelationList1 { get; private set; }

	public PriceRelationModel PriceRelationList2 { get; private set; }

	public PriceRelationModel PriceRelationList3 { get; private set; }

	public PriceRelationModel PriceRelationList4 { get; private set; }

	public PriceRelationModel PriceRelationList5 { get; private set; }

	public PriceRelationModel PriceRelationList6 { get; private set; }

	public PriceRelationModel PriceRelationList7 { get; private set; }

	public PriceRelationModel PriceRelationList8 { get; private set; }

	public Guid Guid { get; private set; }

	public string IdProduct { get; private set; }

	public string Name { get; private set; }

	public string Description { get; private set; }

	public Guid GuidVahed { get; private set; }

	public string VahedName { get; private set; }

	public decimal DiscountPercent { get; private set; }

	public decimal DiscountPrice { get; set; }

	public decimal KardexPrice { get; set; }

	public decimal Sell1
	{
		get
		{
			return PriceRelationList1?.Price ?? 0m;
		}
		set
		{
			if (PriceRelationList1 == null)
			{
				return;
			}
			PriceRelationList1.Price = value;
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sell1"));
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Sell1", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs", 59);
				}
			}, "Sell1", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs");
		}
	}

	public decimal Sell2
	{
		get
		{
			return PriceRelationList2?.Price ?? 0m;
		}
		set
		{
			if (PriceRelationList2 == null)
			{
				return;
			}
			PriceRelationList2.Price = value;
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sell2"));
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Sell2", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs", 77);
				}
			}, "Sell2", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs");
		}
	}

	public decimal Sell3
	{
		get
		{
			return PriceRelationList3?.Price ?? 0m;
		}
		set
		{
			if (PriceRelationList3 == null)
			{
				return;
			}
			PriceRelationList3.Price = value;
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sell3"));
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Sell3", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs", 95);
				}
			}, "Sell3", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs");
		}
	}

	public decimal Sell4
	{
		get
		{
			return PriceRelationList4?.Price ?? 0m;
		}
		set
		{
			if (PriceRelationList4 == null)
			{
				return;
			}
			PriceRelationList4.Price = value;
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sell4"));
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Sell4", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs", 113);
				}
			}, "Sell4", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs");
		}
	}

	public decimal Sell5
	{
		get
		{
			return PriceRelationList5?.Price ?? 0m;
		}
		set
		{
			if (PriceRelationList5 == null)
			{
				return;
			}
			PriceRelationList5.Price = value;
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sell5"));
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Sell5", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs", 131);
				}
			}, "Sell5", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs");
		}
	}

	public decimal Sell6
	{
		get
		{
			return PriceRelationList6?.Price ?? 0m;
		}
		set
		{
			if (PriceRelationList6 == null)
			{
				return;
			}
			PriceRelationList6.Price = value;
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sell6"));
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Sell6", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs", 149);
				}
			}, "Sell6", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs");
		}
	}

	public decimal Sell7
	{
		get
		{
			return PriceRelationList7?.Price ?? 0m;
		}
		set
		{
			if (PriceRelationList7 == null)
			{
				return;
			}
			PriceRelationList7.Price = value;
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sell7"));
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Sell7", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs", 167);
				}
			}, "Sell7", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs");
		}
	}

	public decimal Sell8
	{
		get
		{
			return PriceRelationList8?.Price ?? 0m;
		}
		set
		{
			if (PriceRelationList8 == null)
			{
				return;
			}
			PriceRelationList8.Price = value;
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sell8"));
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Sell8", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs", 185);
				}
			}, "Sell8", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs");
		}
	}

	public decimal Sell1Qt
	{
		get
		{
			return (PriceRelationList1?.Quantity ?? 0m).FixDecimal(18, 3);
		}
		set
		{
			if (PriceRelationList1 == null)
			{
				return;
			}
			PriceRelationList1.Quantity = value;
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sell1Qt"));
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Sell1Qt", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs", 204);
				}
			}, "Sell1Qt", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs");
		}
	}

	public decimal Sell2Qt
	{
		get
		{
			return (PriceRelationList2?.Quantity ?? 0m).FixDecimal(18, 3);
		}
		set
		{
			if (PriceRelationList2 == null)
			{
				return;
			}
			PriceRelationList2.Quantity = value;
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sell2Qt"));
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Sell2Qt", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs", 222);
				}
			}, "Sell2Qt", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs");
		}
	}

	public decimal Sell3Qt
	{
		get
		{
			return (PriceRelationList3?.Quantity ?? 0m).FixDecimal(18, 3);
		}
		set
		{
			if (PriceRelationList3 == null)
			{
				return;
			}
			PriceRelationList3.Quantity = value;
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sell3Qt"));
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Sell3Qt", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs", 240);
				}
			}, "Sell3Qt", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs");
		}
	}

	public decimal Sell4Qt
	{
		get
		{
			return (PriceRelationList4?.Quantity ?? 0m).FixDecimal(18, 3);
		}
		set
		{
			if (PriceRelationList4 == null)
			{
				return;
			}
			PriceRelationList4.Quantity = value;
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sell4Qt"));
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Sell4Qt", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs", 258);
				}
			}, "Sell4Qt", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs");
		}
	}

	public decimal Sell5Qt
	{
		get
		{
			return (PriceRelationList5?.Quantity ?? 0m).FixDecimal(18, 3);
		}
		set
		{
			if (PriceRelationList5 == null)
			{
				return;
			}
			PriceRelationList5.Quantity = value;
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sell5Qt"));
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Sell5Qt", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs", 276);
				}
			}, "Sell5Qt", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs");
		}
	}

	public decimal Sell6Qt
	{
		get
		{
			return (PriceRelationList6?.Quantity ?? 0m).FixDecimal(18, 3);
		}
		set
		{
			if (PriceRelationList6 == null)
			{
				return;
			}
			PriceRelationList6.Quantity = value;
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sell6Qt"));
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Sell6Qt", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs", 294);
				}
			}, "Sell6Qt", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs");
		}
	}

	public decimal Sell7Qt
	{
		get
		{
			return (PriceRelationList7?.Quantity ?? 0m).FixDecimal(18, 3);
		}
		set
		{
			if (PriceRelationList7 == null)
			{
				return;
			}
			PriceRelationList7.Quantity = value;
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sell7Qt"));
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Sell7Qt", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs", 312);
				}
			}, "Sell7Qt", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs");
		}
	}

	public decimal Sell8Qt
	{
		get
		{
			return (PriceRelationList8?.Quantity ?? 0m).FixDecimal(18, 3);
		}
		set
		{
			if (PriceRelationList8 == null)
			{
				return;
			}
			PriceRelationList8.Quantity = value;
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sell8Qt"));
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Sell8Qt", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs", 330);
				}
			}, "Sell8Qt", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs");
		}
	}

	public decimal Sell1Percent => _sell1Percent;

	public decimal Sell2Percent => _sell2Percent;

	public decimal Sell3Percent => _sell3Percent;

	public decimal Sell4Percent => _sell4Percent;

	public decimal Sell5Percent => _sell5Percent;

	public decimal Sell6Percent => _sell6Percent;

	public decimal Sell7Percent => _sell7Percent;

	public decimal Sell8Percent => _sell8Percent;

	public decimal Sell1Sood
	{
		get
		{
			return Sell1 - KardexPrice;
		}
		set
		{
			decimal num = Sell1 - KardexPrice;
			if (value - num == 0m)
			{
				return;
			}
			Sell1 += value - num;
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sell1Sood"));
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Sell1Sood", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs", 359);
				}
			}, "Sell1Sood", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs");
		}
	}

	public decimal Sell2Sood
	{
		get
		{
			return Sell2 - KardexPrice;
		}
		set
		{
			decimal num = Sell2 - KardexPrice;
			if (value - num == 0m)
			{
				return;
			}
			Sell2 += value - num;
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sell2Sood"));
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Sell2Sood", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs", 378);
				}
			}, "Sell2Sood", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs");
		}
	}

	public decimal Sell3Sood
	{
		get
		{
			return Sell3 - KardexPrice;
		}
		set
		{
			decimal num = Sell3 - KardexPrice;
			if (value - num == 0m)
			{
				return;
			}
			Sell3 += value - num;
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sell3Sood"));
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Sell3Sood", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs", 397);
				}
			}, "Sell3Sood", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs");
		}
	}

	public decimal Sell4Sood
	{
		get
		{
			return Sell4 - KardexPrice;
		}
		set
		{
			decimal num = Sell4 - KardexPrice;
			if (value - num == 0m)
			{
				return;
			}
			Sell4 += value - num;
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sell4Sood"));
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Sell4Sood", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs", 416);
				}
			}, "Sell4Sood", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs");
		}
	}

	public decimal Sell5Sood
	{
		get
		{
			return Sell5 - KardexPrice;
		}
		set
		{
			decimal num = Sell5 - KardexPrice;
			if (value - num == 0m)
			{
				return;
			}
			Sell5 += value - num;
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sell5Sood"));
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Sell5Sood", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs", 435);
				}
			}, "Sell5Sood", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs");
		}
	}

	public decimal Sell6Sood
	{
		get
		{
			return Sell6 - KardexPrice;
		}
		set
		{
			decimal num = Sell6 - KardexPrice;
			if (value - num == 0m)
			{
				return;
			}
			Sell6 += value - num;
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sell6Sood"));
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Sell6Sood", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs", 454);
				}
			}, "Sell6Sood", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs");
		}
	}

	public decimal Sell7Sood
	{
		get
		{
			return Sell7 - KardexPrice;
		}
		set
		{
			decimal num = Sell7 - KardexPrice;
			if (value - num == 0m)
			{
				return;
			}
			Sell7 += value - num;
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sell7Sood"));
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Sell7Sood", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs", 473);
				}
			}, "Sell7Sood", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs");
		}
	}

	public decimal Sell8Sood
	{
		get
		{
			return Sell8 - KardexPrice;
		}
		set
		{
			decimal num = Sell8 - KardexPrice;
			if (value - num == 0m)
			{
				return;
			}
			Sell8 += value - num;
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sell8Sood"));
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Sell8Sood", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs", 492);
				}
			}, "Sell8Sood", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs");
		}
	}

	public decimal Sell1PercentSaved
	{
		get
		{
			return (PriceRelationList1?.Profit ?? 0m).FixDecimal(18, 3);
		}
		set
		{
			if (PriceRelationList1 == null)
			{
				return;
			}
			PriceRelationList1.Profit = value;
			Sell1Sood = Sood(KardexPrice, Sell1PercentSaved);
			decimal num = SetKasr(Sell1Sood + KardexPrice, Sell1Kasr);
			if (Sell1 == num)
			{
				return;
			}
			Sell1 = num;
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sell1PercentSaved"));
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Sell1PercentSaved", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs", 517);
				}
			}, "Sell1PercentSaved", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs");
		}
	}

	public decimal Sell2PercentSaved
	{
		get
		{
			return (PriceRelationList2?.Profit ?? 0m).FixDecimal(18, 3);
		}
		set
		{
			if (PriceRelationList2 == null)
			{
				return;
			}
			PriceRelationList2.Profit = value;
			Sell2Sood = Sood(KardexPrice, Sell2PercentSaved);
			decimal num = SetKasr(Sell2Sood + KardexPrice, Sell2Kasr);
			if (Sell2 == num)
			{
				return;
			}
			Sell2 = num;
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sell2PercentSaved"));
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Sell2PercentSaved", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs", 541);
				}
			}, "Sell2PercentSaved", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs");
		}
	}

	public decimal Sell3PercentSaved
	{
		get
		{
			return (PriceRelationList3?.Profit ?? 0m).FixDecimal(18, 3);
		}
		set
		{
			if (PriceRelationList3 == null)
			{
				return;
			}
			PriceRelationList3.Profit = value;
			Sell3Sood = Sood(KardexPrice, Sell3PercentSaved);
			decimal num = SetKasr(Sell3Sood + KardexPrice, Sell3Kasr);
			if (Sell3 == num)
			{
				return;
			}
			Sell3 = num;
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sell3PercentSaved"));
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Sell3PercentSaved", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs", 565);
				}
			}, "Sell3PercentSaved", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs");
		}
	}

	public decimal Sell4PercentSaved
	{
		get
		{
			return (PriceRelationList4?.Profit ?? 0m).FixDecimal(18, 3);
		}
		set
		{
			if (PriceRelationList4 == null)
			{
				return;
			}
			PriceRelationList4.Profit = value;
			Sell4Sood = Sood(KardexPrice, Sell4PercentSaved);
			decimal num = SetKasr(Sell4Sood + KardexPrice, Sell4Kasr);
			if (Sell4 == num)
			{
				return;
			}
			Sell4 = num;
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sell4PercentSaved"));
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Sell4PercentSaved", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs", 589);
				}
			}, "Sell4PercentSaved", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs");
		}
	}

	public decimal Sell5PercentSaved
	{
		get
		{
			return (PriceRelationList5?.Profit ?? 0m).FixDecimal(18, 3);
		}
		set
		{
			if (PriceRelationList5 == null)
			{
				return;
			}
			PriceRelationList5.Profit = value;
			Sell5Sood = Sood(KardexPrice, Sell5PercentSaved);
			decimal num = SetKasr(Sell5Sood + KardexPrice, Sell5Kasr);
			if (Sell5 == num)
			{
				return;
			}
			Sell5 = num;
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sell5PercentSaved"));
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Sell5PercentSaved", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs", 613);
				}
			}, "Sell5PercentSaved", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs");
		}
	}

	public decimal Sell6PercentSaved
	{
		get
		{
			return (PriceRelationList6?.Profit ?? 0m).FixDecimal(18, 3);
		}
		set
		{
			if (PriceRelationList6 == null)
			{
				return;
			}
			PriceRelationList6.Profit = value;
			Sell6Sood = Sood(KardexPrice, Sell6PercentSaved);
			decimal num = SetKasr(Sell6Sood + KardexPrice, Sell6Kasr);
			if (Sell6 == num)
			{
				return;
			}
			Sell6 = num;
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sell6PercentSaved"));
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Sell6PercentSaved", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs", 637);
				}
			}, "Sell6PercentSaved", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs");
		}
	}

	public decimal Sell7PercentSaved
	{
		get
		{
			return (PriceRelationList7?.Profit ?? 0m).FixDecimal(18, 3);
		}
		set
		{
			if (PriceRelationList7 == null)
			{
				return;
			}
			PriceRelationList7.Profit = value;
			Sell7Sood = Sood(KardexPrice, Sell7PercentSaved);
			decimal num = SetKasr(Sell7Sood + KardexPrice, Sell7Kasr);
			if (Sell7 == num)
			{
				return;
			}
			Sell7 = num;
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sell7PercentSaved"));
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Sell7PercentSaved", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs", 661);
				}
			}, "Sell7PercentSaved", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs");
		}
	}

	public decimal Sell8PercentSaved
	{
		get
		{
			return (PriceRelationList8?.Profit ?? 0m).FixDecimal(18, 3);
		}
		set
		{
			if (PriceRelationList8 == null)
			{
				return;
			}
			PriceRelationList8.Profit = value;
			Sell8Sood = Sood(KardexPrice, Sell8PercentSaved);
			decimal num = SetKasr(Sell8Sood + KardexPrice, Sell8Kasr);
			if (Sell8 == num)
			{
				return;
			}
			Sell8 = num;
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sell8PercentSaved"));
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Sell8PercentSaved", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs", 685);
				}
			}, "Sell8PercentSaved", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs");
		}
	}

	public decimal Sell1Kasr
	{
		get
		{
			return PriceRelationList1?.KasrRial ?? 0m;
		}
		set
		{
			if (PriceRelationList1 == null)
			{
				return;
			}
			PriceRelationList1.KasrRial = value;
			Sell1 = SetKasr(Sell1, value);
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sell1Kasr"));
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Sell1Kasr", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs", 705);
				}
			}, "Sell1Kasr", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs");
		}
	}

	public decimal Sell2Kasr
	{
		get
		{
			return PriceRelationList2?.KasrRial ?? 0m;
		}
		set
		{
			if (PriceRelationList2 == null)
			{
				return;
			}
			PriceRelationList2.KasrRial = value;
			Sell2 = SetKasr(Sell2, value);
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sell2Kasr"));
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Sell2Kasr", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs", 724);
				}
			}, "Sell2Kasr", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs");
		}
	}

	public decimal Sell3Kasr
	{
		get
		{
			return PriceRelationList3?.KasrRial ?? 0m;
		}
		set
		{
			if (PriceRelationList3 == null)
			{
				return;
			}
			PriceRelationList3.KasrRial = value;
			Sell3 = SetKasr(Sell3, value);
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sell3Kasr"));
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Sell3Kasr", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs", 743);
				}
			}, "Sell3Kasr", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs");
		}
	}

	public decimal Sell4Kasr
	{
		get
		{
			return PriceRelationList4?.KasrRial ?? 0m;
		}
		set
		{
			if (PriceRelationList4 == null)
			{
				return;
			}
			PriceRelationList4.KasrRial = value;
			Sell4 = SetKasr(Sell4, value);
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sell4Kasr"));
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Sell4Kasr", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs", 762);
				}
			}, "Sell4Kasr", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs");
		}
	}

	public decimal Sell5Kasr
	{
		get
		{
			return PriceRelationList5?.KasrRial ?? 0m;
		}
		set
		{
			if (PriceRelationList5 == null)
			{
				return;
			}
			PriceRelationList5.KasrRial = value;
			Sell5 = SetKasr(Sell5, value);
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sell5Kasr"));
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Sell5Kasr", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs", 781);
				}
			}, "Sell5Kasr", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs");
		}
	}

	public decimal Sell6Kasr
	{
		get
		{
			return PriceRelationList6?.KasrRial ?? 0m;
		}
		set
		{
			if (PriceRelationList6 == null)
			{
				return;
			}
			PriceRelationList6.KasrRial = value;
			Sell6 = SetKasr(Sell6, value);
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sell6Kasr"));
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Sell6Kasr", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs", 800);
				}
			}, "Sell6Kasr", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs");
		}
	}

	public decimal Sell7Kasr
	{
		get
		{
			return PriceRelationList7?.KasrRial ?? 0m;
		}
		set
		{
			if (PriceRelationList7 == null)
			{
				return;
			}
			PriceRelationList7.KasrRial = value;
			Sell7 = SetKasr(Sell7, value);
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sell7Kasr"));
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Sell7Kasr", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs", 819);
				}
			}, "Sell7Kasr", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs");
		}
	}

	public decimal Sell8Kasr
	{
		get
		{
			return PriceRelationList8?.KasrRial ?? 0m;
		}
		set
		{
			if (PriceRelationList8 == null)
			{
				return;
			}
			PriceRelationList8.KasrRial = value;
			Sell8 = SetKasr(Sell8, value);
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sell8Kasr"));
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Sell8Kasr", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs", 838);
				}
			}, "Sell8Kasr", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs");
		}
	}

	[JsonIgnore]
	public short DigitForRound { get; }

	[JsonIgnore]
	public short DigitAfterPoint { get; }

	public event PropertyChangedEventHandler PropertyChanged;

	public ProductPriceReportBusiness(string contextGuid, ProductPriceReportContainer container, ProductPriceReportConfig config)
	{
		checked
		{
			try
			{
				Config = config;
				DigitForRound = (ClsCache.GetCache(contextGuid)?.MultiLanguageConfig?.DigitForRound).GetValueOrDefault(3);
				DigitAfterPoint = (ClsCache.GetCache(contextGuid)?.MultiLanguageConfig?.DigitAfterPoint).GetValueOrDefault();
				if (container?.Product != null)
				{
					Guid = container.Product.Guid;
					IdProduct = container.Product.IdProduct;
					Name = container.Product.Name;
					Description = container.Product.Description;
					GuidVahed = container.Product.GuidVahed;
					VahedName = container.Product.VahedName;
					DiscountPercent = container.Product.DiscountPercent;
				}
				if (!(container?.PriceRelationList?.Any()).GetValueOrDefault())
				{
					return;
				}
				KardexPrice = container.PriceRelationList.FirstOrDefault().KardexPrice;
				for (int i = 0; i < container.PriceRelationList.Count - 1; i++)
				{
					if (i == 0)
					{
						PriceRelationList1 = container.PriceRelationList.SingleOrDefault((PriceRelationModel q) => q.Ordinal == 1);
						_sell1Percent = CalcSoodPercent(Sell1, KardexPrice);
					}
					if (i == 1)
					{
						PriceRelationList2 = container.PriceRelationList.SingleOrDefault((PriceRelationModel q) => q.Ordinal == 2);
						_sell2Percent = CalcSoodPercent(Sell2, KardexPrice);
					}
					if (i == 2)
					{
						PriceRelationList3 = container.PriceRelationList.SingleOrDefault((PriceRelationModel q) => q.Ordinal == 3);
						_sell3Percent = CalcSoodPercent(Sell3, KardexPrice);
					}
					if (i == 3)
					{
						PriceRelationList4 = container.PriceRelationList.SingleOrDefault((PriceRelationModel q) => q.Ordinal == 4);
						_sell4Percent = CalcSoodPercent(Sell4, KardexPrice);
					}
					if (i == 4)
					{
						PriceRelationList5 = container.PriceRelationList.SingleOrDefault((PriceRelationModel q) => q.Ordinal == 5);
						_sell5Percent = CalcSoodPercent(Sell5, KardexPrice);
					}
					if (i == 5)
					{
						PriceRelationList6 = container.PriceRelationList.SingleOrDefault((PriceRelationModel q) => q.Ordinal == 6);
						_sell6Percent = CalcSoodPercent(Sell6, KardexPrice);
					}
					if (i == 6)
					{
						PriceRelationList7 = container.PriceRelationList.SingleOrDefault((PriceRelationModel q) => q.Ordinal == 7);
						_sell7Percent = CalcSoodPercent(Sell7, KardexPrice);
					}
					if (i == 7)
					{
						PriceRelationList8 = container.PriceRelationList.SingleOrDefault((PriceRelationModel q) => q.Ordinal == 8);
						_sell8Percent = CalcSoodPercent(Sell8, KardexPrice);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs", 912);
			}
		}
	}

	private decimal SetKasr(decimal p, decimal kasr)
	{
		try
		{
			if (kasr <= 0m)
			{
				return p;
			}
			return p - p % kasr;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetKasr", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs", 922);
		}
		return 0m;
	}

	private decimal Sood(decimal price, decimal percent)
	{
		return FinancialCalculator.CalculateProfit(price, percent, Config.SoodUp2Down, DigitAfterPoint);
	}

	private decimal CalcSoodPercent(decimal sell, decimal kardex)
	{
		return FinancialCalculator.CalculateProfitPercent(sell, kardex, Config.SoodUp2Down, DigitForRound);
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(string contextGuid, IEnumerable<ProductPriceReportBusiness> items, CancellationToken token = default(CancellationToken), EnSystem system = EnSystem.UserInterface)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (!(items?.Any() ?? false))
			{
				return res;
			}
			foreach (ProductPriceReportBusiness item in items)
			{
				ProductBusiness str = await ProductBusiness.GetAsync(contextGuid, item.Guid, token);
				if (item.Sell1 > 0m)
				{
					str.AddToPriceList(new ProductPriceRelationBusiness(contextGuid)
					{
						Price = item.PriceRelationList1.Price,
						Quantity = item.PriceRelationList1.Quantity,
						KasrRial = item.PriceRelationList1.KasrRial,
						Profit = item.PriceRelationList1.Profit,
						PriceLevelGuid = item.PriceRelationList1.PriceLevelGuid
					});
				}
				if (item.Sell2 > 0m)
				{
					str.AddToPriceList(new ProductPriceRelationBusiness(contextGuid)
					{
						Price = item.PriceRelationList2.Price,
						Quantity = item.PriceRelationList2.Quantity,
						KasrRial = item.PriceRelationList2.KasrRial,
						Profit = item.PriceRelationList2.Profit,
						PriceLevelGuid = item.PriceRelationList2.PriceLevelGuid
					});
				}
				if (item.Sell3 > 0m)
				{
					str.AddToPriceList(new ProductPriceRelationBusiness(contextGuid)
					{
						Price = item.PriceRelationList3.Price,
						Quantity = item.PriceRelationList3.Quantity,
						KasrRial = item.PriceRelationList3.KasrRial,
						Profit = item.PriceRelationList3.Profit,
						PriceLevelGuid = item.PriceRelationList3.PriceLevelGuid
					});
				}
				if (item.Sell4 > 0m)
				{
					str.AddToPriceList(new ProductPriceRelationBusiness(contextGuid)
					{
						Price = item.PriceRelationList4.Price,
						Quantity = item.PriceRelationList4.Quantity,
						KasrRial = item.PriceRelationList4.KasrRial,
						Profit = item.PriceRelationList4.Profit,
						PriceLevelGuid = item.PriceRelationList4.PriceLevelGuid
					});
				}
				if (item.Sell5 > 0m)
				{
					str.AddToPriceList(new ProductPriceRelationBusiness(contextGuid)
					{
						Price = item.PriceRelationList5.Price,
						Quantity = item.PriceRelationList5.Quantity,
						KasrRial = item.PriceRelationList5.KasrRial,
						Profit = item.PriceRelationList5.Profit,
						PriceLevelGuid = item.PriceRelationList5.PriceLevelGuid
					});
				}
				if (item.Sell6 > 0m)
				{
					str.AddToPriceList(new ProductPriceRelationBusiness(contextGuid)
					{
						Price = item.PriceRelationList6.Price,
						Quantity = item.PriceRelationList6.Quantity,
						KasrRial = item.PriceRelationList6.KasrRial,
						Profit = item.PriceRelationList6.Profit,
						PriceLevelGuid = item.PriceRelationList6.PriceLevelGuid
					});
				}
				if (item.Sell7 > 0m)
				{
					str.AddToPriceList(new ProductPriceRelationBusiness(contextGuid)
					{
						Price = item.PriceRelationList7.Price,
						Quantity = item.PriceRelationList7.Quantity,
						KasrRial = item.PriceRelationList7.KasrRial,
						Profit = item.PriceRelationList7.Profit,
						PriceLevelGuid = item.PriceRelationList7.PriceLevelGuid
					});
				}
				if (item.Sell8 > 0m)
				{
					str.AddToPriceList(new ProductPriceRelationBusiness(contextGuid)
					{
						Price = item.PriceRelationList8.Price,
						Quantity = item.PriceRelationList8.Quantity,
						KasrRial = item.PriceRelationList8.KasrRial,
						Profit = item.PriceRelationList8.Profit,
						PriceLevelGuid = item.PriceRelationList8.PriceLevelGuid
					});
				}
				IReadOnlyList<PrdTechnicalBusiness> readOnlyList = await str.GetPrdTechnicalListAsync(system, token);
				Dictionary<Guid, List<PrdTechnicalBusiness>> technicalDic = new Dictionary<Guid, List<PrdTechnicalBusiness>> { 
				{
					str.Guid,
					readOnlyList?.ToList()
				} };
				List<BarcodeBusiness> value = await BarcodeBusiness.GetByPrdGuidAsync(contextGuid, new List<Guid> { str.Guid }, token);
				Dictionary<Guid, List<BarcodeBusiness>> barcodes = new Dictionary<Guid, List<BarcodeBusiness>> { { str.Guid, value } };
				str.Modified = DateTime.Now;
				str.ServerStatus = ServerStatusForClient.None;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await str.SaveAsync(sendToServer: true, 0, token, system, awaitOnSemaphore: true, await4send: false, "", "", 60000, technicalDic, null, 10, "", null, updateAmount: false, barcodes), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs", 1010);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs", 1015);
			res.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceReportBusiness.cs", 1016);
		}
		return res;
	}
}
