namespace PacketParser.Entities;

public enum KarinCommands : short
{
	[PersianNameAttribute.PersianName("دریافت فاکتور خوانده نشده")]
	GetUnReadFactorsFromScale = 206,
	[PersianNameAttribute.PersianName("نوشتن حافظه PLU")]
	WritePlu = 7,
	[PersianNameAttribute.PersianName("رفتن به حالت ترازو")]
	ScaleMode = 255,
	[PersianNameAttribute.PersianName("Halt normal operations and enter to communication mode")]
	CommunicationMode = 254,
	[PersianNameAttribute.PersianName("Exit communication mode and resume normal operations")]
	ReturnToScaleMode = 252,
	[PersianNameAttribute.PersianName("Set Baud Rate")]
	SetBaudRate = 253,
	[PersianNameAttribute.PersianName("Set Port Function & Peripheral Device")]
	SetPort = 251,
	[PersianNameAttribute.PersianName("Set Receipt Data on port after closing a Receipt")]
	SetReceptData = 250,
	[PersianNameAttribute.PersianName("Read Receipt Data from one Address")]
	ReadReceiptDatafromOneAddress = 200,
	[PersianNameAttribute.PersianName("Read Next Receipt Address Counter value where next Receipt Data will be stored")]
	ReadNextReceptAddress = 201,
	[PersianNameAttribute.PersianName("Read recent N*Receipt Data")]
	ReadRecentData = 202,
	[PersianNameAttribute.PersianName("Read/Erase the Daily Total value")]
	ReadOrEraseDailyTotalValue = 203,
	[PersianNameAttribute.PersianName("Set/Clear fetch-byte of an addressed Receipt to check whether it has been fetched or not at a later time in the future")]
	SetOrClearReceipt = 204,
	[PersianNameAttribute.PersianName("Read multiple receipts which haven't been fetched yet")]
	ReadReceiptNotReceive = 205,
	[PersianNameAttribute.PersianName("Read Display [Weight][Unit Price][Total Price]")]
	ReadDisplay = 100,
	[PersianNameAttribute.PersianName("Read RTC ( Real Time Clock) Time & Date")]
	ReadScaleTime = 101,
	[PersianNameAttribute.PersianName("Write/Read Access Password")]
	AccessPassword = 102,
	[PersianNameAttribute.PersianName("Read Scale Master Serial Number")]
	ReadScaleSerialNumber = 103,
	[PersianNameAttribute.PersianName("Read Receipt Texts")]
	ReadReceiptTexts = 104,
	[PersianNameAttribute.PersianName("Write Receipt Texts")]
	WriteReceiptTexts = 105,
	[PersianNameAttribute.PersianName("Read/Write Barcode ID")]
	ReadBarcode = 106,
	[PersianNameAttribute.PersianName("Set Memory Flush State (روزانھ فروش حذف حالت تعیین)")]
	SetMemoryFlushState = 107,
	[PersianNameAttribute.PersianName("Read Weight Continuous")]
	ReadWeightContinuous = 108,
	[PersianNameAttribute.PersianName("Write Memory Barcode(EAN13)")]
	WriteMemoryBarcode = 5,
	[PersianNameAttribute.PersianName("Read Memory Barcode(EAN13)")]
	ReadMemoryBarcode = 6,
	[PersianNameAttribute.PersianName("Read a PLU Memory [Price][Name][Entity][Product Code][Barcode]")]
	ReadPLUMemory = 8,
	[PersianNameAttribute.PersianName("Read N*PLU Memory Records [Price][Name][Entity][Product Code][Barcode]")]
	ReadSpeceficPLUMemory = 9,
	[PersianNameAttribute.PersianName("Clear One Addressed Memory Record")]
	ClearAddressOnMemory = 10,
	[PersianNameAttribute.PersianName("Find the first blank memory and send its address. Otherwise send 0 means that no blank is found.")]
	FindFirstBlankMemory = 10,
	[PersianNameAttribute.PersianName("Write Real Time Clock (RTC) Time & Date")]
	WriteTimeOnScale = 99
}
