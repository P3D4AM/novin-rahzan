<div dir="rtl">

# حضور+ (HozorPlus)

سیستم جامع مدیریت حضور و غیاب تحت وب — با رابط کاربری کاملاً فارسی و راست‌به‌چپ.

جانشین مدرنِ نرم‌افزارهای دسکتاپی قدیمی: بدون نیاز به نصب روی تک‌تک سیستم‌ها، از هر مرورگری (دسکتاپ و موبایل) قابل استفاده است و همزمان دستگاه‌های اثرانگشت ZKTeco را نیز پشتیبانی می‌کند.

---

## ✨ امکانات

| ماژول | توضیح |
| --- | --- |
| 👥 **پرسنل** | پرونده کامل کارکنان، دپارتمان، سِمَت، نوع استخدام، کد پرسنلی، PIN و QR اختصاصی |
| 🕐 **شیفت‌ها** | شیفت ثابت، چندبخشی (اسپلیت)، شیفت شب و کراس‌میدنایت، الگوی گردشی و تقویم ماهانهٔ تخصیص شیفت |
| 📊 **محاسبهٔ حضور** | جفت‌کردن ورود/خروج، محاسبهٔ کارکرد، تأخیر، خروج قبل از موعد و اضافه‌کاری — با در نظر گرفتن جمعه‌ها و تعطیلات رسمی |
| 🖥️ **کاغذدیواری (Kiosk)** | ثبت تردد با PIN یا اسکن QR روی تبلت/کیوسک نصب‌شده در ورودی |
| 📱 **کارت وب پرسنل** | ثبت تردد از موبایل کارمند همراه با موقعیت جغرافیایی |
| 🔌 **دستگاه‌های ZKTeco** | اتصال مستقیم با پروتکل TCP/UDP (پورت 4370)، همگام‌سازی ترددها، تایم‌اوت امن و لاگ کامل |
| 📥 **ورود داده** | ایمپورت Excel/CSV عمومی + ثبت دستی مدیر با ثبت دقیق در Audit Log |
| 🌴 **مرخصی و اضافه‌کاری** | گردش کار تأیید چندمرحله‌ای، موجودی مرخصی سالانه، اعلان‌ها |
| 🔔 **اعلان هوشمند** | قواعد غیبت/تأخیر → زنگ اپلیکیشن + Toast لحظه‌ای |
| 🤖 **دستیار هوشمند** | چت‌بات فارسی با پروتکل اقدام ساختاریافته (به همراه موتور قاعده‌محور به‌عنوان جایگزین) |
| 📈 **داشبورد لحظه‌ای** | جریان ترددها به‌صورت Realtime (WebSocket)، نمودار ۱۴ روزه، توزیع دپارتمانی |
| 📑 **گزارش‌ها** | خلاصهٔ ماهانه + نمودار + خروجی Excel + نسخهٔ چاپی |
| 🔐 **سطوح دسترسی** | چهار نقش: مدیر کل، مدیر منابع انسانی، تأییدکننده، کارمند |

## 🖥️ نصب سریع روی ویندوز

> راهنمای تصویری نداره ولی واقعاً **دو دابل‌کلیک** بیشتر لازم نیست!

1. فایل `HozorPlus-v1.0.0-Windows-Setup.zip` را از بخش [Releases](../../releases) دانلود و استخراج کنید (پیشنهاد: مسیری کوتاه و بدون فاصله مثل `C:\HozorPlus`)
2. روی **`INSTALL.bat`** دابل‌کلیک کنید — این اسکریپت به‌ترتیب:
   - نبود Node.js را بررسی و در صورت نیاز با `winget` نصب می‌کند
   - پکیج‌ها را نصب می‌کند (`npm install`)
   - دیتابیس SQLite را می‌سازد و دادهٔ نمونهٔ فارسی را بارگذاری می‌کند
   - بیلد production را انجام می‌دهد
3. روی **`START.bat`** دابل‌کلیک کنید — مرورگر روی `http://localhost:3000` باز می‌شود ✅
4. برای توقف: پنجرهٔ سرور را ببندید یا `STOP.bat` را اجرا کنید

**پیش‌نیازها:** ویندوز ۱۰/۱۱ ×۶۴، اینترنت فقط برای مرحلهٔ نصب، و Node.js نسخهٔ ۲۰ به بالا (اسکریپت خودش نصب می‌کند).

**دسترسی از سایر سیستم‌های شبکه:** اگر ویندوز فایروال اجازهٔ دسترسی داد (Allow access)، دیگر کارکنان با آدرس `http://<IP-سیستم>:3000` می‌توانند وارد شوند.

### حساب‌های پیش‌فرض (دادهٔ نمونه)

| نام کاربری | رمز عبور | نقش |
| --- | --- | --- |
| `admin` | `admin123` | مدیر کل (دسترسی کامل) |
| `hr` | `hr123` | مدیر منابع انسانی |
| `manager` | `manager123` | تأییدکننده (سرپرست) |
| `ali` | `ali123` | کارمند |

> ⚠️ بعد از ورود اول، رمزها را از بخش «تنظیمات» تغییر دهید.

## 🧑‍💻 راه‌اندازی برای توسعه‌دهندگان (هر سیستم‌عاملی)

```bash
git clone -b develop https://github.com/P3D4AM/novin-rahzan.git
cd novin-rahzan
npm install
cp .env.example .env            # مسیر دیتابیس را تنظیم کنید
npx prisma generate
npx prisma db push
npm run seed                    # دادهٔ نمونهٔ فارسی
npm run dev                     # http://localhost:3000
```

اجرای سرویس realtime به‌صورت جداگانه (اختیاری — برای داشبورد لحظه‌ای):

```bash
npm run start:realtime          # socket.io روی پورت 3003
```

بیلد production:

```bash
npm run build
npm start
```

## 🏗️ معماری

```
مرورگر ──HTTP──▶ Next.js (:3000)  ──▶ SQLite / PostgreSQL
   │                                   ▲
   └──WebSocket──▶ Realtime (:3003)    │
                      ▲                │
                      └─ emit (:3004) ─┘   (رویدادها از API Routes به همه پخش می‌شود)
```

| پورت | سرویس |
| --- | --- |
| `3000` | اپلیکیشن Next.js |
| `3003` | socket.io (فید لحظه‌ای داشبورد و اعلان‌ها) |
| `3004` | نقطهٔ emit داخلی (فقط localhost) |

## 🗄️ دیتابیس

به‌صورت پیش‌فرض **SQLite** است تا نصب بدون تنظیم باشد. برای استقرار سازمانی روی **PostgreSQL**:

1. در `prisma/schema.prisma` مقدار `provider` را به `"postgresql"` تغییر دهید
2. در `.env` رشتهٔ اتصال Postgres را قرار دهید
3. `npx prisma db push && npm run seed` را اجرا کنید

بقیهٔ کد به‌کلی قابل حمل است (ORM بدون ویژگی وابسته به دیتابیس خاص).

## 🚀 استقرار پشت پروکسی معکوس (HTTPS)

فایل [`Caddyfile`](./Caddyfile) نمونهٔ راه‌اندازی پشت Caddy است؛ الگوی `XTransformPort` اجازه می‌دهد سرویس realtime هم روی همان دامنه (بدون پورت اضافه) در دسترس باشد.

## 📁 ساختار پروژه

```
├── INSTALL.bat / START.bat / STOP.bat   ← نصب و اجرا روی ویندوز
├── scripts/
│   ├── postbuild.js      ← مونتاژ بستهٔ standalone (کراس‌پلتفرم)
│   ├── start-windows.js  ← لانچر production (وب + realtime)
│   ├── seed.ts           ← دادهٔ نمونهٔ فارسی
│   └── recalc.ts         ← محاسبهٔ مجدد حضور
├── mini-services/realtime/ ← سرویس socket.io
├── prisma/schema.prisma    ← مدل داده (۱۲ موجودیت)
├── src/app/api/            ← ۲۲ مسیر API
├── src/components/app/     ← ماژول‌های رابط کاربری
└── src/lib/                ← منطق حضور، احراز هویت، تقویم جلالی، اتصال ZK
```

</div>

---

## English summary

**HozorPlus (حضور+)** is a full-featured, RTL-first Persian web attendance system built with Next.js 16, TypeScript, Tailwind CSS 4 + shadcn/ui, Prisma (SQLite default, PostgreSQL-ready) and socket.io realtime.

- **Windows one-click install:** extract the release zip → `INSTALL.bat` → `START.bat` → <http://localhost:3000> (`admin` / `admin123`)
- Modules: personnel, shifts (fixed/split/overnight/rotations), attendance engine, web self-check-in with geolocation, PIN/QR kiosk, ZKTeco device sync (TCP/UDP 4370), Excel/CSV import, leave & overtime workflows, smart notifications, Persian AI assistant, live dashboard, monthly reports with Excel export, 4-level RBAC.
- Branch: **`develop`**
