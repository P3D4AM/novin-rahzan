using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using EntityCache.Business.Accounting;
using EntityCache.Interfaces;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.MetaData;
using PacketParser.EventArgs.Sanad;
using PacketParser.Records;

namespace EntityCache.Business;

public class Sanad_DetBusiness : BaseKhazaneDetail, ISanad_Det, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, INotifyPropertyChanged
{
	private bool _isDisposed;

	private MoeinBusiness _moein;

	private TafsilBusiness _tafsil;

	private TafsilBusiness _tafsil2;

	private TafsilBusiness _tafsil3;

	private TafsilBusiness _tafsil4;

	private CancellationTokenSource _sumChangedToken;

	private CancellationTokenSource _fillMoeinToken;

	private CancellationTokenSource _fillTafsilToken;

	private CancellationTokenSource _fillTafsil2Token;

	private CancellationTokenSource _fillTafsil3Token;

	private CancellationTokenSource _fillTafsil4Token;

	private CancellationTokenSource _updateDescToken = new CancellationTokenSource();

	private decimal _debit;

	private decimal _credit;

	private string _codeTafsilForUi;

	private string _desc;

	private string _codeTafsil2ForUi;

	private string _codeTafsil3ForUi;

	private string _codeTafsil4ForUi;

	private Guid _moeinGuid;

	private Guid? _tafsilGuid;

	private Guid? _tafsilGuid2;

	private Guid? _tafsilGuid3;

	private Guid? _tafsilGuid4;

	private Guid? _decimalGuid;

	private string _codeMoeinForUi;

	private EnSystem _system;

	private int _ordinal = -1;

	private Guid guid = Guid.NewGuid();

	[JsonIgnore]
	[XmlIgnore]
	public ISanadBusiness MasterSanadItem { get; set; }

	public override KhazaneType Type
	{
		get
		{
			if (!(Debit > 0m))
			{
				return KhazaneType.EnteghalBes;
			}
			return KhazaneType.EnteghalBed;
		}
	}

	public new Guid Guid
	{
		get
		{
			if (guid == Guid.Empty)
			{
				guid = Guid.NewGuid();
			}
			return guid;
		}
		set
		{
			guid = value;
		}
	}

	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status { get; set; } = true;


	public Guid MoeinGuid => _moeinGuid;

	public override int Ordinal => _ordinal;

	public SanadDetMetaData MetaData { get; set; }

	public Guid? TafsilGuid => _tafsilGuid;

	public Guid? TafsilGuid2 => _tafsilGuid2;

	public Guid? TafsilGuid3 => _tafsilGuid3;

	public Guid? TafsilGuid4 => _tafsilGuid4;

	public decimal Debit
	{
		get
		{
			return _debit;
		}
		set
		{
			try
			{
				if (!IsDisposed)
				{
					bool num = _debit != value && this.OnSumChanged != null;
					_debit = value;
					if (Debit != 0m)
					{
						_credit = default(decimal);
					}
					if (num)
					{
						_sumChangedToken?.Cancel();
						_sumChangedToken = new CancellationTokenSource();
						CancellationToken token = _sumChangedToken.Token;
						PacketParser.Utilities.RaiseEvent(this.OnSumChanged, this, new EventArgs(), token, 60, "Debit", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\Sanad_DetBusiness.cs");
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Debit", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\Sanad_DetBusiness.cs", 91);
			}
		}
	}

	public decimal Credit
	{
		get
		{
			return _credit;
		}
		set
		{
			try
			{
				_sumChangedToken?.Cancel();
				_sumChangedToken = new CancellationTokenSource();
				CancellationToken token = _sumChangedToken.Token;
				bool num = _credit != value && this.OnSumChanged != null;
				_credit = value;
				if (_credit != 0m)
				{
					_debit = default(decimal);
				}
				if (num)
				{
					PacketParser.Utilities.RaiseEvent(this.OnSumChanged, this, new EventArgs(), token, 60, "Credit", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\Sanad_DetBusiness.cs");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Credit", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\Sanad_DetBusiness.cs", 110);
			}
		}
	}

	public Guid? DecimalGuid
	{
		get
		{
			return _decimalGuid;
		}
		set
		{
			_decimalGuid = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public decimal Qt { get; set; }

	public new Guid MasterGuid { get; set; }

	public Document AsType { get; set; }

	public Guid AsGuid { get; set; }

	public Tuple<Guid, Document> AsGuidAsType => new Tuple<Guid, Document>(AsGuid, AsType);

	public string Serial { get; set; }

	public string CodeMoeinForUi
	{
		get
		{
			return _codeMoeinForUi;
		}
		set
		{
			_codeMoeinForUi = value?.FixString() ?? "";
			_fillMoeinToken?.Cancel();
			_fillMoeinToken = new CancellationTokenSource();
			CancellationToken token = _fillMoeinToken.Token;
			Task.Run(() => FillMoeinByCodeAsync(CodeMoeinForUi?.ParseToLong(0L) ?? 0, token));
		}
	}

	public string CodeTafsilForUi
	{
		get
		{
			return _tafsil?.Code.ToString() ?? _codeTafsilForUi ?? "";
		}
		set
		{
			if (!(_codeTafsilForUi == value))
			{
				_fillTafsilToken?.Cancel();
				_fillTafsilToken = new CancellationTokenSource();
				CancellationToken token = _fillTafsilToken.Token;
				Task.Run(() => UpdateTafsilByCodeAsync(value?.ParseToLong(0L) ?? 0, EnSystem.UserInterface, "", token));
				_codeTafsilForUi = value?.FixString() ?? "";
			}
		}
	}

	public string CodeTafsil2ForUi
	{
		get
		{
			return _tafsil2?.Code.ToString() ?? _codeTafsil2ForUi ?? "";
		}
		set
		{
			if (!(_codeTafsil2ForUi == value))
			{
				_fillTafsil2Token?.Cancel();
				_fillTafsil2Token = new CancellationTokenSource();
				CancellationToken token = _fillTafsil2Token.Token;
				Task.Run(() => UpdateTafsil2ByCodeAsync(value?.ParseToLong(0L) ?? 0, EnSystem.UserInterface, "", token));
				_codeTafsil2ForUi = value?.FixString() ?? "";
			}
		}
	}

	public string CodeTafsil3ForUi
	{
		get
		{
			return _tafsil3?.Code.ToString() ?? _codeTafsil3ForUi ?? "";
		}
		set
		{
			if (!(_codeTafsil3ForUi == value))
			{
				_fillTafsil3Token?.Cancel();
				_fillTafsil3Token = new CancellationTokenSource();
				CancellationToken token = _fillTafsil3Token.Token;
				Task.Run(() => UpdateTafsil3ByCodeAsync(value?.ParseToLong(0L) ?? 0, EnSystem.UserInterface, "", token));
				_codeTafsil3ForUi = value?.FixString() ?? "";
			}
		}
	}

	public string CodeTafsil4ForUi
	{
		get
		{
			return _tafsil4?.Code.ToString() ?? _codeTafsil4ForUi ?? "";
		}
		set
		{
			if (!(_codeTafsil4ForUi == value))
			{
				_fillTafsil4Token?.Cancel();
				_fillTafsil4Token = new CancellationTokenSource();
				CancellationToken token = _fillTafsil4Token.Token;
				Task.Run(() => UpdateTafsil4ByCodeAsync(value?.ParseToLong(0L) ?? 0, EnSystem.UserInterface, "", token));
				_codeTafsil4ForUi = value?.FixString() ?? "";
			}
		}
	}

	public long CodeMoein => _moein?.Code ?? 0;

	[JsonIgnore]
	public long CodeTafsil => _tafsil?.Code ?? 0;

	[JsonIgnore]
	public string TafsilName => _tafsil?.Name ?? "";

	[JsonIgnore]
	public string Tafsil2Name => _tafsil2?.Name ?? "";

	[JsonIgnore]
	public string Tafsil3Name => _tafsil3?.Name ?? "";

	[JsonIgnore]
	public string Tafsil4Name => _tafsil4?.Name ?? "";

	[JsonIgnore]
	public string MoeinName => _moein?.Name ?? "";

	public string DebitString
	{
		get
		{
			return Debit.NpToString("N0", 0);
		}
		set
		{
			Debit = value.ParseToDecimal(0m, DigitAfterPoint);
		}
	}

	public string CreditString
	{
		get
		{
			return Credit.NpToString("N0", 0);
		}
		set
		{
			Credit = value.ParseToDecimal(0m, DigitAfterPoint);
		}
	}

	public DateTime? DateSarresid { get; set; }

	public string RowDesc { get; set; }

	[JsonIgnore]
	public long CodeTafsil2 => _tafsil2?.Code ?? 0;

	[JsonIgnore]
	public long CodeTafsil3 => _tafsil3?.Code ?? 0;

	[JsonIgnore]
	public long CodeTafsil4 => _tafsil4?.Code ?? 0;

	public override string DescriptionForUi
	{
		get
		{
			if (!string.IsNullOrEmpty(_desc))
			{
				return _desc;
			}
			_updateDescToken?.Cancel();
			_updateDescToken = new CancellationTokenSource();
			CancellationToken token = _updateDescToken.Token;
			Task.Run(() => UpdateDescForDisplayAsync(token));
			return _desc;
		}
	}

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

	public override FileManagement_Parts Part => FileManagement_Parts.Sanad;

	[JsonIgnore]
	public short DigitAfterPoint { get; }

	public event Func<object, EventArgs, Task> OnSumChanged;

	public event Func<object, DisposedEventArgs, Task> OnDispose;

	public new event Func<object, OrdinalChangedEventArgs, Task> OnOrdinalChanged;

	public new event PropertyChangedEventHandler PropertyChanged;

	public Sanad_DetBusiness(string contextGuid, EnSystem system, Guid asGuid, Document asType, int ordinal)
	{
		try
		{
			base.ContextGuid = contextGuid;
			AsGuid = asGuid;
			AsType = asType;
			DigitAfterPoint = (ClsCache.GetCache(base.ContextGuid)?.MultiLanguageConfig?.DigitAfterPoint).GetValueOrDefault();
			_ordinal = ordinal;
			_system = system;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\Sanad_DetBusiness.cs", 226);
		}
	}

	private async Task UpdateDescForDisplayAsync(CancellationToken token)
	{
		try
		{
			if (token.IsCancellationRequested || (TafsilGuid ?? Guid.Empty) == Guid.Empty)
			{
				return;
			}
			TafsilBusiness tafsilBusiness = await TafsilBusiness.GetAsync(base.ContextGuid, EnSystem.UserInterface, TafsilGuid.Value, "", token, "UpdateDescForDisplayAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\Sanad_DetBusiness.cs");
			if (tafsilBusiness == null)
			{
				_desc = RowDesc;
			}
			else
			{
				if (Debit > 0m)
				{
					_desc = "انتقال بدهی به".GetTranslate() + " " + tafsilBusiness.Name + " ";
				}
				else if (Credit > 0m)
				{
					_desc = "انتقال بستانکاری به".GetTranslate() + " " + tafsilBusiness.Name;
				}
				else
				{
					_desc = RowDesc;
				}
				if (!string.IsNullOrEmpty(RowDesc))
				{
					_desc = _desc + " " + "شرح سطر".GetTranslate() + ": " + RowDesc;
				}
			}
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DescriptionForUi"));
				}
				catch (Exception ex5)
				{
					WebErrorLog.Instance.StartLog(ex5, "", "UpdateDescForDisplayAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\Sanad_DetBusiness.cs", 246);
				}
			}, "UpdateDescForDisplayAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\Sanad_DetBusiness.cs");
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DescriptionForUi"));
				}
				catch (Exception ex4)
				{
					WebErrorLog.Instance.StartLog(ex4, "", "UpdateDescForDisplayAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\Sanad_DetBusiness.cs", 247);
				}
			}, "UpdateDescForDisplayAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\Sanad_DetBusiness.cs");
		}
		catch (OperationCanceledException)
		{
		}
		catch (ThreadAbortException)
		{
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, "", "UpdateDescForDisplayAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\Sanad_DetBusiness.cs", 254);
			throw;
		}
	}

	[Obsolete("pass ensystem", true)]
	public override void Dispose(bool disposing)
	{
		Dispose(EnSystem.None, disposing);
	}

	public void Dispose(EnSystem system)
	{
		Dispose(system, disposing: true);
	}

	public void Dispose(EnSystem system, bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				this.OnOrdinalChanged = null;
				if (this.OnDispose != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnDispose, this, new DisposedEventArgs(system), default(CancellationToken), 60, "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\Sanad_DetBusiness.cs");
				}
				this.OnDispose = null;
				this.OnOrdinalChanged = null;
				this.OnSumChanged = null;
				_sumChangedToken?.Cancel();
				_sumChangedToken?.Dispose();
				_sumChangedToken = null;
				_fillMoeinToken?.Cancel();
				_fillMoeinToken?.Dispose();
				_fillMoeinToken = null;
				base.Dispose(disposing: true);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\Sanad_DetBusiness.cs", 286);
		}
	}

	public ReturnedSaveFuncInfo CheckValidation(EnSystem system, int index, bool isDeleteAction)
	{
		using (new JaegerMonitor("CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\Sanad_DetBusiness.cs"))
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
			try
			{
				if (Debit <= 0m && Credit <= 0m && system != EnSystem.Transfer && !isDeleteAction)
				{
					returnedSaveFuncInfo.AddError(system, "ستون بدهکار یا بستانکار در ردیف".GetTranslate() + " " + $" {index}".GetTranslate() + " باید پر شود".GetTranslate(), "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\Sanad_DetBusiness.cs", 295);
				}
				if (ClsCache.GetCache(base.ContextGuid).Source != ENSource.Android_HesabYar && MoeinGuid == Guid.Empty && !isDeleteAction)
				{
					returnedSaveFuncInfo.AddError(system, "طرف حساب معین سند در ردیف ".GetTranslate() + " " + $"{index}".GetTranslate() + " " + " باید پر شود".GetTranslate() + "\r\n" + "شرح سطر".GetTranslate() + ":" + RowDesc + "\r\n" + "مبلغ بدهکار".GetTranslate() + ":" + DebitString + "\r\n" + "مبلغ بستانکار".GetTranslate() + ":" + CreditString + "\r\n" + $"MoeinGuid:{MoeinGuid}\r\n" + $"TafsilGuid:{TafsilGuid}", "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\Sanad_DetBusiness.cs", 298);
				}
				if (system != EnSystem.Transfer && string.IsNullOrEmpty(RowDesc) && !isDeleteAction)
				{
					returnedSaveFuncInfo.AddError(system, "شرح سطر سند در ردیف".GetTranslate() + " " + $" {index}" + " باید پر شود".GetTranslate() + "\r\n" + "مبلغ بدهکار".GetTranslate() + ":" + DebitString + "\r\n" + "مبلغ بستانکار".GetTranslate() + ":" + CreditString + "\r\n" + $"MoeinGuid:{MoeinGuid}\r\n" + $"TafsilGuid:{TafsilGuid}", "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\Sanad_DetBusiness.cs", 306);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\Sanad_DetBusiness.cs", 314);
				returnedSaveFuncInfo.AddReturnedValue(ex, "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\Sanad_DetBusiness.cs", 315);
			}
			return returnedSaveFuncInfo;
		}
	}

	private async Task UpdateTafsilByCodeAsync(long code, EnSystem system, string tranName, CancellationToken token)
	{
		_ = 2;
		try
		{
			await Task.Delay(new Random().Next(500));
			if (!IsDisposed && !token.IsCancellationRequested)
			{
				TafsilBusiness tafsilBusiness = await TafsilBusiness.GetAsync(base.ContextGuid, system, code, tranName, token, "UpdateTafsilByCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\Sanad_DetBusiness.cs");
				if (!IsDisposed && !token.IsCancellationRequested)
				{
					await SetTafsilGuidAsync(tafsilBusiness?.Guid, tranName, token);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateTafsilByCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\Sanad_DetBusiness.cs", 329);
		}
	}

	private async Task UpdateTafsil2ByCodeAsync(long code, EnSystem system, string tranName, CancellationToken token)
	{
		_ = 2;
		try
		{
			await Task.Delay(new Random().Next(500));
			if (!IsDisposed && !token.IsCancellationRequested)
			{
				TafsilBusiness tafsilBusiness = await TafsilBusiness.GetAsync(base.ContextGuid, system, code, tranName, token, "UpdateTafsil2ByCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\Sanad_DetBusiness.cs");
				if (!IsDisposed && !token.IsCancellationRequested)
				{
					await SetTafsil2GuidAsync(tafsilBusiness?.Guid, tranName, token);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateTafsil2ByCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\Sanad_DetBusiness.cs", 341);
		}
	}

	private async Task UpdateTafsil3ByCodeAsync(long code, EnSystem system, string tranName, CancellationToken token)
	{
		_ = 2;
		try
		{
			await Task.Delay(new Random().Next(500));
			if (!IsDisposed && !token.IsCancellationRequested)
			{
				TafsilBusiness tafsilBusiness = await TafsilBusiness.GetAsync(base.ContextGuid, system, code, tranName, token, "UpdateTafsil3ByCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\Sanad_DetBusiness.cs");
				if (!IsDisposed && !token.IsCancellationRequested)
				{
					await SetTafsil3GuidAsync(tafsilBusiness?.Guid, tranName, token);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateTafsil3ByCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\Sanad_DetBusiness.cs", 353);
		}
	}

	private async Task UpdateTafsil4ByCodeAsync(long code, EnSystem system, string tranName, CancellationToken token)
	{
		_ = 2;
		try
		{
			await Task.Delay(new Random().Next(500));
			if (!IsDisposed && !token.IsCancellationRequested)
			{
				TafsilBusiness tafsilBusiness = await TafsilBusiness.GetAsync(base.ContextGuid, system, code, tranName, token, "UpdateTafsil4ByCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\Sanad_DetBusiness.cs");
				if (!IsDisposed && !token.IsCancellationRequested)
				{
					await SetTafsil4GuidAsync(tafsilBusiness?.Guid, tranName, token);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateTafsil4ByCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\Sanad_DetBusiness.cs", 365);
		}
	}

	public bool CheckIsContain(IEnumerable<Tuple<Document, Guid>> sanad)
	{
		return sanad?.Any((Tuple<Document, Guid> p) => p.Item1 == AsType && p.Item2 == AsGuid) ?? false;
	}

	private void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		try
		{
			if (IsDisposed)
			{
				return;
			}
			PropertyChangedEventHandler handler = this.PropertyChanged;
			if (handler == null || this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					handler(this, new PropertyChangedEventArgs(propertyName));
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "OnPropertyChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\Sanad_DetBusiness.cs", 376);
				}
			}, "OnPropertyChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\Sanad_DetBusiness.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnPropertyChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\Sanad_DetBusiness.cs", 378);
		}
	}

	public void SetOrdinalChange(EnSystem system, int value, bool throwOnOrdinalChanged, CancellationToken token = default(CancellationToken))
	{
		if (!IsDisposed)
		{
			if (value < 0)
			{
				throw new ArgumentException($"value({value}) is not valid for SanadDetail.Ordinal");
			}
			int ordinal = _ordinal;
			_ordinal = value;
			if (throwOnOrdinalChanged && _ordinal >= 0 && this.OnOrdinalChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnOrdinalChanged, this, new OrdinalChangedEventArgs(ordinal, value, Guid, system), token, 60, "SetOrdinalChange", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\Sanad_DetBusiness.cs");
			}
		}
	}

	public async Task SetMoeinGuidAsync(Guid moeinGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			_moeinGuid = moeinGuid;
			_moein = await MoeinBusiness.GetAsync(base.ContextGuid, _moeinGuid, tranName, token);
			if (_moein != null)
			{
				_codeMoeinForUi = _moein.Code.ToString();
			}
			OnPropertyChanged("MoeinName");
			OnPropertyChanged("CodeMoeinForUi");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetMoeinGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\Sanad_DetBusiness.cs", 399);
		}
	}

	private async Task FillMoeinByCodeAsync(long code, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			await Task.Delay(new Random().Next(500));
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			_moein = await MoeinBusiness.GetAsync(base.ContextGuid, code, token, tranName);
			if (!IsDisposed && !token.IsCancellationRequested)
			{
				if (_moein != null)
				{
					_codeMoeinForUi = _moein.Code.ToString();
				}
				_moeinGuid = _moein?.Guid ?? Guid.Empty;
				OnPropertyChanged("CodeMoeinForUi");
				OnPropertyChanged("MoeinName");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FillMoeinByCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\Sanad_DetBusiness.cs", 416);
		}
	}

	public async Task SetTafsilGuidAsync(Guid? tafsilGuid, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if ((_tafsilGuid ?? Guid.Empty) == (tafsilGuid ?? Guid.Empty))
			{
				return;
			}
			_desc = "";
			_tafsilGuid = tafsilGuid;
			if (token.IsCancellationRequested)
			{
				return;
			}
			if ((_tafsilGuid ?? Guid.Empty) == Guid.Empty)
			{
				_tafsil = null;
			}
			else
			{
				if (token.IsCancellationRequested)
				{
					return;
				}
				TafsilBusiness tafsil = await TafsilBusiness.GetAsync(base.ContextGuid, _system, _tafsilGuid ?? Guid.Empty, tranName, token, "SetTafsilGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\Sanad_DetBusiness.cs");
				if (token.IsCancellationRequested)
				{
					return;
				}
				_tafsil = tafsil;
				_codeTafsilForUi = _tafsil?.Code.ToString() ?? "";
			}
			OnPropertyChanged("TafsilName");
			OnPropertyChanged("CodeTafsilForUi");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetTafsilGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\Sanad_DetBusiness.cs", 440);
			throw;
		}
	}

	public async Task SetTafsil2GuidAsync(Guid? tafsilGuid, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if ((_tafsilGuid2 ?? Guid.Empty) == (tafsilGuid ?? Guid.Empty))
			{
				return;
			}
			_tafsilGuid2 = tafsilGuid;
			if (token.IsCancellationRequested)
			{
				return;
			}
			if ((_tafsilGuid2 ?? Guid.Empty) == Guid.Empty)
			{
				_tafsil2 = null;
			}
			else
			{
				if (token.IsCancellationRequested)
				{
					return;
				}
				TafsilBusiness tafsil = await TafsilBusiness.GetAsync(base.ContextGuid, _system, _tafsilGuid2 ?? Guid.Empty, tranName, token, "SetTafsil2GuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\Sanad_DetBusiness.cs");
				if (token.IsCancellationRequested)
				{
					return;
				}
				_tafsil2 = tafsil;
				_codeTafsil2ForUi = _tafsil2?.Code.ToString() ?? "";
			}
			OnPropertyChanged("CodeTafsil2ForUi");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetTafsil2GuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\Sanad_DetBusiness.cs", 464);
			throw;
		}
	}

	public async Task SetTafsil3GuidAsync(Guid? tafsilGuid, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if ((_tafsilGuid3 ?? Guid.Empty) == (tafsilGuid ?? Guid.Empty))
			{
				return;
			}
			_tafsilGuid3 = tafsilGuid;
			if (token.IsCancellationRequested)
			{
				return;
			}
			if ((_tafsilGuid3 ?? Guid.Empty) == Guid.Empty)
			{
				_tafsil3 = null;
			}
			else
			{
				if (token.IsCancellationRequested)
				{
					return;
				}
				TafsilBusiness tafsil = await TafsilBusiness.GetAsync(base.ContextGuid, _system, _tafsilGuid3 ?? Guid.Empty, tranName, token, "SetTafsil3GuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\Sanad_DetBusiness.cs");
				if (token.IsCancellationRequested)
				{
					return;
				}
				_tafsil3 = tafsil;
				_codeTafsil3ForUi = _tafsil3?.Code.ToString() ?? "";
			}
			OnPropertyChanged("CodeTafsil3ForUi");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetTafsil3GuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\Sanad_DetBusiness.cs", 488);
			throw;
		}
	}

	public async Task SetTafsil4GuidAsync(Guid? tafsilGuid, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if ((_tafsilGuid4 ?? Guid.Empty) == (tafsilGuid ?? Guid.Empty))
			{
				return;
			}
			_tafsilGuid4 = tafsilGuid;
			if (token.IsCancellationRequested)
			{
				return;
			}
			if ((_tafsilGuid4 ?? Guid.Empty) == Guid.Empty)
			{
				_tafsil4 = null;
			}
			else
			{
				if (token.IsCancellationRequested)
				{
					return;
				}
				TafsilBusiness tafsil = await TafsilBusiness.GetAsync(base.ContextGuid, _system, _tafsilGuid4 ?? Guid.Empty, tranName, token, "SetTafsil4GuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\Sanad_DetBusiness.cs");
				if (token.IsCancellationRequested)
				{
					return;
				}
				_tafsil4 = tafsil;
				_codeTafsil4ForUi = _tafsil4?.Code.ToString() ?? "";
			}
			OnPropertyChanged("CodeTafsil4ForUi");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetTafsil4GuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\Sanad_DetBusiness.cs", 512);
			throw;
		}
	}

	public override Task<string> GetShortDescriptionAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		throw new NotImplementedException();
	}

	public static List<SmsTemplateRecord> GetSmsTemplateList()
	{
		return new List<SmsTemplateRecord>
		{
			new SmsTemplateRecord("[نام مشتری]"),
			new SmsTemplateRecord("[مبلغ]"),
			new SmsTemplateRecord("[طرف حساب دریافت]"),
			new SmsTemplateRecord("[حساب فعلی]"),
			new SmsTemplateRecord("[عنوان فروشگاه]"),
			new SmsTemplateRecord("[تلفن فروشگاه]")
		};
	}

	public void ClearDescription()
	{
		_desc = "";
	}
}
