# Installer & npm Scripts Policy — v1.8.1

این سند تصمیم‌ها و ریشه‌های اصلاح v1.8.1 را ثبت می‌کند: رفع شکست نصب v1.8.0 روی
Windows و سازگارکردن INSTALL.bat با npm 10 و npm 11 (و آیندهٔ npm 12) بدون هیچ تغییری
در دیتابیس، `.env` یا دادهٔ کاربران.

## 1. علت ریشه‌ای شکست نصب v1.8.0 (دو علت مستقل)

1. **تشخیص غلط نام فایل موتور Prisma در `scripts/verify-native.js`.**
   اسکریپت قدیمی فقط نام‌های شامل `libquery_engine` را می‌پذیرفت. در نصب واقعی
   Windows با Prisma 6.x فایل موتور `query_engine-windows.dll.node` نام دارد
   (پیشوند `lib` فقط در نام‌های targetهای Unix ظاهر می‌شود). نتیجه: نصبِ کاملاً سالم
   «شکست‌خورده» اعلام می‌شد و پیام کلی «npx prisma generate را اجرا کنید / سیاست npm را
   اصلاح کنید» نمایش داده می‌شد — پیامی که با واقعیت (وجود کامل موتور و کلاینت) تناقض
   داشت.
2. **ترتیب غلط مراحل در `INSTALL.bat`.**
   `verify-native` **قبل از** `prisma generate` اجرا می‌شد؛ پس بررسیٔ «کلاینت تولیدشده»
   در همان لحظهٔ اجرا همیشه شکست می‌خورد (فایل هنوز تولید نشده بود).

اصلاح v1.8.1 در هر دو لایه: تشخیص موتور بر اساس OS/معماری واقعی + الگوهای نامِ هر
پلتفرم (بخش ۲) و ترتیب صحیح نصب: `npm install` ← `npx prisma generate` ←
`verify-native` ← `npx prisma db push --skip-generate` (بخش ۳).

## 2. ساختار واقعی فایل‌های Prisma که verifier حالا پوشش می‌دهد

محیط واقعی Windows کاربر (Prisma 6.19.3) این ساختار را داشت — که v1.8.0 آن را
«ناقص» می‌دانست:

```
node_modules/.prisma/client/index.js
node_modules/.prisma/client/query_engine-windows.dll.node
node_modules/@prisma/engines/query_engine-windows.dll.node
```

الگوهای معتبر موتور کوئری، بر اساس پلتفرم:

| پلتفرم | نام فایل موتور | نکته |
| --- | --- | --- |
| win32 | `query_engine-windows.dll.node` | نام قدیمی `libquery_engine-windows…dll.node` هم پذیرفته می‌شود |
| linux | `libquery_engine-<target>.so.node` | هر target توزیع: debian / rhel / musl / arch / … |
| darwin | `libquery_engine-darwin.dylib.node` | Apple Silicon: `libquery_engine-darwin-arm64.dylib.node` (و خودِ darwinِ ساده هم روی arm64 پذیرفته — Rosetta) |

دو مکان بررسی می‌شود: پوشهٔ کلاینت تولیدشده (کپی‌شده توسط `prisma generate` — لازم در
runtime) و بستهٔ `@prisma/engines` (رسیده از npm — لازم برای CLI). فایل باید
**غیرخالی و قابل‌خواندن** باشد؛ فایل صفر-بایت به‌عنوان دانلود ناقص گزارش می‌شود.
عدم تطابق معماری (موتور arm64 روی ماشین x64) خطای مشخص خودش را دارد. مسیر کلاینت
تولیدشده از `output = "…"` داخل `prisma/schema.prisma` خوانده می‌شود (پیش‌فرض:
`node_modules/.prisma/client`). سازگاری نسخهٔ prisma CLI / @prisma/client /
@prisma/engines / کلاینت تولیدشده بررسی و در صورت اختلاف major/minor هشدار داده
می‌شود. هر پیام خطا دقیقاً همان قلمِ مفقود را نام می‌برد — نه یک جملهٔ کلی.

## 3. ترتیب جدید INSTALL.bat (با بررسی Exit Code در هر مرحله)

```
[1/8] بررسی Node.js (≥20) و npm        — winget فقط برای node؛ نبودِ npm = خطای مشخص PATH
[2/8] npm install --no-audit --no-fund --ignore-scripts=false
[3/8] npx prisma generate               — شکست → توقف با پیام «Prisma Client generation failed»
[4/8] node scripts\verify-native.js      — شکست → توقف با نمایش علت واقعی از خطوط [X]
[5/8] npx prisma db push --skip-generate — شکست → توقف با پیام مرحله‌ای مشخص
[6/8] نوشتن .env با scripts/env-ensure.js (متغیرهای موجود حفظ می‌شوند)
[7/8] مقداردهی دیتابیس با db-admin.ts (دیتای موجود به‌طور پیش‌فرض نگه داشته می‌شود)
[8/8] بیلد production با build-with-lock.js (قفل سریالی)
```

هیچ مرحله‌ای دیتابیس، `.env` یا دادهٔ کاربر را حذف/بازنویسی نمی‌کند؛ اجرای مجدد
INSTALL.bat روی نصب موجود (تست واقعی در `scripts/test-installer-real.sh`) حفظ
داده را اثبات می‌کند.

## 4. `allowScripts` در package.json — تصمیمِ به‌ازای-بسته

npm 11 (11.16+) هنگام نصب، فهرست بسته‌هایی را که اسکریپت نصب دارند و هنوز
«بررسی‌نشده»اند به‌صورت هشدار چاپ می‌کند؛ `--ignore-scripts=false` این هشدار را
حذف نمی‌کند. npm 12 همین سیاست را **اجباری** می‌کند: اسکریپتِ بستهٔ تأییدنشده اجرا
نخواهد شد. راه‌حل رسمی (مستندات `npm approve-scripts`) فیلد `allowScripts` در
`package.json` پروژه است — تنظیمی Project-level که با npm 10 نادیده گرفته می‌شود و
هیچ تنظیم Global/User-level کاربر را تغییر نمی‌دهد.

ورودی‌ها **بدون پین‌نسخه** (name-only) ثبت شده‌اند تا با تغییرهای patch تازه‌نشکن
باشند. هر بستهٔ زیر قبل از تصمیم، با اسکریپتِ نصبِ واقعیِ نصب‌شده بازبینی شده است:

| بسته | اسکریپت نصب (واقعی) | علت حضور در درخت | تصمیم | دلیل |
| --- | --- | --- | --- | --- |
| `prisma` | `preinstall: node scripts/preinstall-entry.js` (گیری نسخهٔ Node) | dependency مستقیم | ✅ allow | CLI مرحله‌های ۳ و ۵ INSTALL.bat است |
| `@prisma/client` | `postinstall: node scripts/postinstall.js` — `prisma generate` خودکار | dependency مستقیم | ✅ allow | توصیهٔ رسمی Prisma؛ تولید کلاینت شرط اجرای اپ است |
| `@prisma/engines` | `postinstall: node scripts/postinstall.js` (نازل/تأیید موتورها) | dependency مستقیم | ✅ allow | همان موتوری که verifier بررسی می‌کند |
| `esbuild` | `postinstall: node install.js` (اعتبارسنجی/لینک باینری پلتفرم) | tsx ← devDependency | ✅ allow | `npx tsx scripts/db-admin.ts` مرحلهٔ ۷ به باینری esbuild نیاز دارد |
| `sharp` | `install: node install/check.js ‖ build` | dependency مستقیم | ✅ allow | next/image در production؛ check فقط اعتبارسنجی @img/sharp است |
| `@swc/core` | `postinstall: node postinstall.js` (اعتبارسنجی بایندینگ پلتفرم) | via `next-intl` | ✅ allow | اعتبارسنجی بستهٔ optional پلتفرم؛ بدون آن کامپایل پیام‌ها کند می‌شود |
| `@parcel/watcher` | `install: node scripts/build-from-source.js` — **no-op** مگر `npm_config_build_from_source=true` | via `next-intl` | ✅ allow | اسکریپت به‌طور پیش‌فرض کاری نمی‌کند؛ بایندینگ از optionalDependencies می‌رسد |
| `es5-ext` | `postinstall: try{require('./_postinstall')}catch{} ‖ exit 0` | via `@mdxeditor/editor → sandpack-react → react-devtools-inline → es6-symbol → d → es5-ext` | ❌ deny | اسکریپت صرفاً گرم‌کردن cache است و خودش خطا را می‌بلعد؛ بسته بدون آن یکسان کار می‌کند. حضورش زنجیرهٔ عمیقِ ابزار preview ادیتور MDX است، نه نیاز runtime اپ |
| `unrs-resolver` | `postinstall: napi-postinstall unrs-resolver <ver> check` | via زنجیرهٔ eslint (devDep) | ❌ deny | checkِ تشخیصی برای ابزار eslint؛ بایندینگ native مستقل از این اسکریپت از optionalDependencies نصب می‌شود و نه INSTALL.bat و نه runtime اپ به آن نیاز ندارد |

`false` (deny) یعنی «بررسی شد و عمداً مجاز نشد» — در npm 11 فهرست pending را پاک
می‌کند و در npm 12 اسکریپت آن بسته را (بی‌خطر) نادیده می‌گیرد.

## 5. سازگاری npm 10 / 11 / 12 و Node 20 / 24

- **npm 10:** فیلد ناشناختهٔ `allowScripts` در `package.json` نادیده گرفته می‌شود؛
  نصب و اسکریپت‌ها بدون تغییر رفتار اجرا می‌شوند (تست واقعی در
  `scripts/test-npm-matrix.sh`).
- **npm 11:** هشدار `warn install-scripts` با پوشش کامل فیلد حذف می‌شود؛ اسکریپت‌های
  مجاز اجرا و ورودی‌های `false` نادیده گرفته می‌شوند (تست واقعی همان‌جا).
- **npm 12 (آینده):** اسکریپت‌های فقط بسته‌های allow بالا اجرا می‌شوند — همان چیزی
  که این پروژه به آن نیاز دارد.
- **Node 20 / 24:** هر دو ماتریس واقعی (scripts/test-npm-matrix.sh و GitHub Actions
  `.github/workflows/installer-matrix.yml`) پوشش داده می‌شوند.
- `--dangerously-allow-all-scripts` **استفاده نشده** و هیچ تنظیم global/user-level
  npm کاربر تغییر نمی‌کند.

## 6. ابزارهای تست این فاز

| اسکریپت | چه چیزی را واقعاً اجرا می‌کند |
| --- | --- |
| `scripts/test-verify-native.ts` | درخت‌های ساختگی (Windows/Linux/macOS، موتور ناموجود، کلاینت ناموجود، فایل خالی، غیرقابل‌خواندن، معماری اشتباه، مسیر output سفارشی) + فراخوانی CLI واقعی |
| `scripts/test-installer-bat.ts` | ترتیب/Exit-Code/شماره‌گذاری/پیام‌های اختصاصی INSTALL.bat + اثبات نبود دستور حذف دیتابیس/.env |
| `scripts/test-npm-matrix.sh` | نصب واقعی با Node 20/24 × npm 10/11؛ عدم وجود هشدار install-scripts؛ کارکرد esbuild/sharp |
| `scripts/test-installer-real.sh` | نصب واقعی npm + `prisma generate` + verify + `db push` + حفظ `.env` و ردیف دیتابیس در اجرای دوباره |
| `.github/workflows/installer-matrix.yml` | همان ماتریس روی ubuntu/windows runnerهای GitHub (تأیید واقعی نام فایل موتور Windows) |
