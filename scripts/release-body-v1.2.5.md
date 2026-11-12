<div dir="rtl">

# 🔓 حضور+ (HozorPlus) v1.2.5 — راز دستگاه NP756+ حل شد: «ورود با رمز شبکه» (فرمان 0x52)

## 🎯 کشف اصلی این نسخه

خروجی np-probe شما دو چیز را قطعی کرد: دستگاه شما API HTTP ندارد و هیچ فریمِ رمزشده‌ای هم نمی‌پذیرد — ولی به فریم کلاسیکِ ساده پاسخِ رد می‌دهد. یعنی رمز شما درست بود، اما چیزی «قبل از» فرمان‌ها جا مانده بود.

دیگهٔ DLL نرم‌افزار اصلی را دوباره باز کردیم و این‌بار **تابع وصل شدن (StartTcpX) را خط‌به‌خط** اجرا کردیم — و جواب را پیدا کردیم:

```
بعد از اتصال TCP، DLL فروشنده اول یک «فرمان ورود» می‌فرستد:
  55 AA | شماره دستگاه | 79 19 | 52 00 | رمز شبکه (۴ بایت) | 00 00 | چک‌سام
دستگاه جواب می‌دهد: RESULT — status≠0 یعنی ورود موفق، status=0 یعنی رمز غلط
(همان پیام معروف «Password Mismatched» نرم‌افزار فروشنده!)
```

**تا وقتی وارد نشده‌اید، دستگاه NP فرمان‌های دیگر را با همان پاسخ ردِ ۱۰بایتی که شما دیدید (code 0x10 = «اول وارد شو») برمی‌گرداند و بقیه را ساکت رها می‌کند.** حال همه‌چیز توضیح داده می‌شود — نرم‌افزار ما تا امروز هرگز این فرمان ورود را نمی‌فرستاد!

## ✅ چه چیزی درست شد

- **«تست اتصال» حالا اول فرمان ورود (0x52) را با رمز شبکه می‌فرستد** — روی دستگاه NP756+ شما باید برای اولین بار موفق شود و سریال + ساعت دستگاه را نشان دهد.
- **«دانلود ترددها» هم روی همین مسیر کار می‌کند** — بعد از ورود، همان پروتکل کلاسیکِ امتحان‌شدهٔ قبلی جواب می‌دهد.
- رمز غلط = پیام روشن (نه سکوت مبهم قبلی).
- اگر رمز را خالی بگذارید، رمزهای رایج خودکار امتحان می‌شوند.
- ابزار `np-probe.js` هم حالا همین ورود را اجرا و نتیجه را قدم‌به‌قدم نشان می‌دهد.

## 🧪 چه کنید (۲ دقیقه)

1. زیپ جدید را روی محل قبلی استخراج کنید → `INSTALL.bat` → `START.bat`
2. **دستگاه‌ها ← تست اتصال** — رمز شبکه همان 12345 که وارد کرده‌اید.
3. اگر باز هم خطا داد، خروجی `node scripts\np-probe.js <IP> <پورت> 12345` را برایمان بفرستید — بخش [۱] همان ورود را با رمز درست/غلط/صفر مقایسه می‌کند و دقیقاً می‌گویند دستگاه چه گفت.

## 🔄 خلاصهٔ فنی

- `classicLogin/tryClassicLogin/ensureLogin` در `src/lib/sbxpc.ts` — فریم 0x52 با رمز در arg2، plaintext؛ RESULT status≠0 = موفق؛ نشست ورود روی سوکت زنده است و بعد از هر اتصال مجدد دوباره ورود انجام می‌شود
- نوبت‌بندیِ تشخیص: ردِ ۱۰بایتی → **ورود 0x52** → EXT رمزشده → API HTTP → کلاسیک رمزشده
- `DIALECT=classiclogin` در شبیه‌ساز — دقیقاً رفتار واقعی NP756+ (ردِ 0x10 قبل از ورود، RESULT بعد از ورود، فرمان‌های عادی بعدش)
- ۱۱ تست پشت‌سرهم سبز (ورود صریح/کشف خودکار/رمز غلط + رگرسیون ۸ گویش)
- مستندات پروتکل: توالی کامل Disassembly (vtable[2] → شاخهٔ کلاسیک → RESULT) + جدال معنایی code=0x10

</div>

---

**English TL;DR** — The NP756+ mystery is solved. The vendor's StartTcpX sends a LOGIN first — classic plaintext cmd **0x52** with the «رمز شبکه» in arg2 — and only then commands are accepted; until then the device refuses every classic frame with the 10-byte code-0x10 ack (which is exactly "login first", not a dialect error) and drops everything else. Our client never sent it. v1.2.5 sends it (configured password, else auto-probed candidates), reuses the same logged-in session for serial/clock/log-download, and reports a clear wrong-password error. The np-probe tool now performs the same login battery (correct/wrong/zero password + post-login command) — if the test still fails on your unit, send its output and we'll see the device's actual reply to the login frame.
