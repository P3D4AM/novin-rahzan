# -*- coding: utf-8 -*-
"""Stamp Persian page numbers on the PDF (skip cover) + set metadata."""

import io
from reportlab.pdfgen import canvas
from reportlab.pdfbase import pdfmetrics
from reportlab.pdfbase.ttfonts import TTFont
from pypdf import PdfReader, PdfWriter

SRC = '/home/z/my-project/download/گزارش_تحلیل_کتابخانه_ها.pdf'
PAGE_W, PAGE_H = 720, 1020
ACCENT = (0x24 / 255, 0x4a / 255, 0x5e / 255)
MUTED = (0x85 / 255, 0x8b / 255, 0x8f / 255)

pdfmetrics.registerFont(TTFont('Vazir', '/home/z/my-project/report_assets/fonts/Vazirmatn-Regular.ttf'))

FA = str.maketrans('0123456789', '۰۱۲۳۴۵۶۷۸۹')


def make_overlay(n_pages):
    buf = io.BytesIO()
    c = canvas.Canvas(buf, pagesize=(PAGE_W, PAGE_H))
    for i in range(n_pages):
        if i > 0:  # skip cover (pdf page 1)
            disp = str(i).translate(FA)  # displayed number = pdf index
            c.setFont('Vazir', 9)
            c.setFillColorRGB(*MUTED)
            c.drawCentredString(PAGE_W / 2, 16, disp)
        c.showPage()
    c.save()
    buf.seek(0)
    return buf


def main():
    reader = PdfReader(SRC)
    n = len(reader.pages)
    overlay = PdfReader(make_overlay(n))
    writer = PdfWriter()
    for i, page in enumerate(reader.pages):
        if i > 0:
            page.merge_page(overlay.pages[i])
        writer.add_page(page)

    writer.add_metadata({
        '/Title': 'گزارش آموزشی تحلیل کتابخانه‌های سیستم حضور و غیاب نوین پرداز',
        '/Author': 'Z.ai',
        '/Subject': 'تحلیل فنی ۱۳ کتابخانهٔ اختصاصی نرم‌افزار حضور و غیاب نوین پرداز (دکامپایل با ILSpy) و آموزش معماری چندلایه دات‌نت',
        '/Creator': 'Z.ai',
        '/Keywords': 'حضور و غیاب, .NET, C#, WinForms, SQL Server, decompile, ILSpy, Novin Pardaz',
    })
    with open(SRC, 'wb') as f:
        writer.write(f)
    print(f'stamped {n - 1} pages (cover skipped), metadata set')


if __name__ == '__main__':
    main()
