import type { Metadata } from "next";
import "vazirmatn/Vazirmatn-Variable-font-face.css";
import "./globals.css";
import { Toaster } from "@/components/ui/toaster";
import { ThemeProvider } from "@/components/theme-provider";

export const metadata: Metadata = {
  title: "حضور+ | سامانه هوشمند حضور و غیاب",
  description: "سامانه مدرن حضور و غیاب با داشبورد زنده، شیفت‌های متنوع، مرخصی، اضافه‌کاری، گزارش‌های پیشرفته و اتصال به دستگاه‌های اثرانگشت",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="fa" dir="rtl" suppressHydrationWarning>
      <body className="font-vazirmatn antialiased bg-background text-foreground min-h-screen">
        <ThemeProvider attribute="class" defaultTheme="light" enableSystem={false} disableTransitionOnChange>
          {children}
          <Toaster />
        </ThemeProvider>
      </body>
    </html>
  );
}
