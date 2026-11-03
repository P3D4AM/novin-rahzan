namespace PacketParser.Entities.WordPress;

public class WooSyncSettings
{
	[PersianNameAttribute.PersianName("کالاها")]
	public WooSyncProductSettings Product { get; set; } = new WooSyncProductSettings();


	[PersianNameAttribute.PersianName("گروه کالاها")]
	public WooSyncProductGroupSettings ProductGroup { get; set; } = new WooSyncProductGroupSettings();


	[PersianNameAttribute.PersianName("اشخاص")]
	public WooSyncCustomerSettings Customer { get; set; } = new WooSyncCustomerSettings();


	[PersianNameAttribute.PersianName("مشخصات و ویژگی های کالا")]
	public WooSyncProductPropertySettings ProductProperty { get; set; } = new WooSyncProductPropertySettings();


	[PersianNameAttribute.PersianName("تنوع کالاها")]
	public WooSyncProductVarietySettings ProductVariety { get; set; } = new WooSyncProductVarietySettings();


	[PersianNameAttribute.PersianName("فاکتور")]
	public WooSyncOrderSettings Order { get; set; } = new WooSyncOrderSettings();


	public WooSyncSettings Clone()
	{
		WooSyncSettings obj = (WooSyncSettings)MemberwiseClone();
		obj.Product = Product.Clone();
		obj.ProductGroup = ProductGroup.Clone();
		obj.Customer = Customer.Clone();
		obj.ProductProperty = ProductProperty.Clone();
		obj.ProductVariety = ProductVariety.Clone();
		obj.Order = Order.Clone();
		return obj;
	}
}
