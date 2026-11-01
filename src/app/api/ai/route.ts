import { NextRequest, NextResponse } from 'next/server'
import { db } from '@/lib/db'
import { requireRole } from '@/lib/auth'
import { fail } from '@/lib/api'
import { addDays, tehranDayOf, toJalali, jFormat, durationText, faDigits } from '@/lib/jalali'

interface ToolResult {
  action: string
  summary: string
}

/** Execute structured action from assistant */
async function runAction(name: string, args: Record<string, unknown>, userName: string): Promise<ToolResult> {
  const today = tehranDayOf(new Date())
  const yesterday = addDays(today, -1)

  if (name === 'absent_list') {
    const day = args.date ? tehranDayOf(new Date(String(args.date))) : yesterday
    const records = await db.dailyAttendance.findMany({
      where: { date: day, status: 'ABSENT' },
      include: { person: { select: { firstName: true, lastName: true } } },
    })
    const names = records.map((r) => `${r.person.firstName} ${r.person.lastName}`).join('، ')
    return { action: 'absent_list', summary: `غایبان ${jFormat(day, { fa: true })}: ${names || 'هیچ‌کس'}` }
  }

  if (name === 'present_now') {
    const records = await db.dailyAttendance.findMany({
      where: { date: today, status: { in: ['PRESENT', 'INCOMPLETE'] } },
      include: { person: { select: { firstName: true, lastName: true } } },
    })
    const names = records.map((r) => `${r.person.firstName} ${r.person.lastName}`).join('، ')
    return { action: 'present_now', summary: `حاضرین امروز: ${names || 'هیچ‌کس'}` }
  }

  if (name === 'today_stats') {
    const [present, absent, late, leave, ot] = await Promise.all([
      db.dailyAttendance.count({ where: { date: today, status: 'PRESENT' } }),
      db.dailyAttendance.count({ where: { date: today, status: 'ABSENT' } }),
      db.dailyAttendance.count({ where: { date: today, lateMinutes: { gt: 0 } } }),
      db.dailyAttendance.count({ where: { date: today, status: 'LEAVE' } }),
      db.dailyAttendance.aggregate({ where: { date: today }, _sum: { overtimeMinutes: true } }),
    ])
    return {
      action: 'today_stats',
      summary: `امروز: ${faDigits(present)} حاضر، ${faDigits(absent)} غایب، ${faDigits(late)} تأخیر، ${faDigits(leave)} مرخصی، اضافه‌کاری جمع ${durationText(ot._sum.overtimeMinutes ?? 0)}`,
    }
  }

  if (name === 'person_report') {
    const q = String(args.name || '')
    const persons = await db.personnel.findMany({
      where: { OR: [{ firstName: { contains: q } }, { lastName: { contains: q } }, { personnelCode: { contains: q } }] },
      take: 1,
    })
    if (!persons.length) return { action: 'person_report', summary: `شخصی به نام «${q}» پیدا نشد` }
    const p = persons[0]
    const day = args.date ? tehranDayOf(new Date(String(args.date))) : yesterday
    const rec = await db.dailyAttendance.findUnique({ where: { personId_date: { personId: p.id, date: day } } })
    if (!rec) return { action: 'person_report', summary: `برای ${p.firstName} ${p.lastName} در ${jFormat(day, { fa: true })} رکوردی نیست` }
    const statusFa: Record<string, string> = { PRESENT: 'حاضر', ABSENT: 'غایب', LEAVE: 'مرخصی', HOLIDAY: 'تعطیل', OFF: 'تعطیلی', INCOMPLETE: 'ناقص', PENDING: 'در انتظار' }
    return {
      action: 'person_report',
      summary: `${p.firstName} ${p.lastName} در ${jFormat(day, { fa: true })}: ${statusFa[rec.status] ?? rec.status}${rec.lateMinutes ? `، تأخیر ${durationText(rec.lateMinutes)}` : ''}${rec.workedMinutes ? `، کارکرد ${durationText(rec.workedMinutes)}` : ''}${rec.overtimeMinutes ? `، اضافه‌کاری ${durationText(rec.overtimeMinutes)}` : ''}`,
    }
  }

  if (name === 'leave_register') {
    const q = String(args.name || '')
    const persons = await db.personnel.findMany({
      where: { OR: [{ firstName: { contains: q } }, { lastName: { contains: q } }, { personnelCode: { contains: q } }] },
      take: 1,
    })
    if (!persons.length) return { action: 'leave_register', summary: `شخصی به نام «${q}» پیدا نشد` }
    const p = persons[0]
    const lt = await db.leaveType.findFirst({ where: { name: { contains: 'استحقاقی' } } })
    if (!lt) return { action: 'leave_register', summary: 'نوع مرخصی استحقاقی تعریف نشده است' }
    const days = Number(args.days || 1)
    const startDay = args.date ? tehranDayOf(new Date(String(args.date))) : addDays(today, 1)
    const end = new Date(startDay.getTime() + days * 86400000 - 3600000)
    const req = await db.leaveRequest.create({
      data: {
        personId: p.id, leaveTypeId: lt.id, kind: 'DAILY',
        startDateTime: new Date(startDay.getTime() + 8 * 3600000),
        endDateTime: new Date(end.getTime() + 14 * 3600000),
        durationMinutes: days * 8 * 60, reason: `ثبت توسط دستیار (${userName})`, status: 'APPROVED',
        approverId: 'assistant', decidedAt: new Date(),
      },
    })
    return { action: 'leave_register', summary: `مرخصی ${days} روزه برای ${p.firstName} ${p.lastName} از ${jFormat(startDay, { fa: true })} ثبت شد.` }
  }

  if (name === 'overtime_report') {
    const day = args.date ? tehranDayOf(new Date(String(args.date))) : yesterday
    const records = await db.dailyAttendance.findMany({
      where: { date: day, overtimeMinutes: { gt: 0 } },
      include: { person: { select: { firstName: true, lastName: true } } },
      orderBy: { overtimeMinutes: 'desc' },
      take: 10,
    })
    const lines = records.map((r) => `${r.person.firstName} ${r.person.lastName}: ${durationText(r.overtimeMinutes)}`).join('؛ ')
    return { action: 'overtime_report', summary: `اضافه‌کاری ${jFormat(day, { fa: true })}: ${lines || 'ثبت نشده'}` }
  }

  if (name === 'late_report') {
    const day = args.date ? tehranDayOf(new Date(String(args.date))) : yesterday
    const records = await db.dailyAttendance.findMany({
      where: { date: day, lateMinutes: { gt: 0 } },
      include: { person: { select: { firstName: true, lastName: true } } },
      orderBy: { lateMinutes: 'desc' },
      take: 10,
    })
    const lines = records.map((r) => `${r.person.firstName} ${r.person.lastName}: ${durationText(r.lateMinutes)}`).join('؛ ')
    return { action: 'late_report', summary: `تأخیرهای ${jFormat(day, { fa: true })}: ${lines || 'هیچ تأخیری ثبت نشده'}` }
  }

  return { action: name, summary: 'عملیات شناخته نشد' }
}

/** Rule-based Persian command parsing fallback */
async function ruleBasedReply(message: string, userName: string): Promise<{ reply: string; result?: ToolResult }> {
  const msg = message.replace(/\u200c/g, ' ').trim()
  const numMatch = msg.match(/(\d+)\s*(روز|ساعت)/)
  const num = numMatch ? Number(numMatch[1]) : 1

  if (/غایب|غیبت/.test(msg)) {
    const r = await runAction('absent_list', {}, userName)
    return { reply: r.summary, result: r }
  }
  if (/حاضر/.test(msg) && /الان|امروز|کنون/.test(msg)) {
    const r = await runAction('present_now', {}, userName)
    return { reply: r.summary, result: r }
  }
  if (/آمار|خلاصه|وضعیت امروز/.test(msg)) {
    const r = await runAction('today_stats', {}, userName)
    return { reply: r.summary, result: r }
  }
  if (/اضافه.?کاری/.test(msg)) {
    const r = await runAction('overtime_report', {}, userName)
    return { reply: r.summary, result: r }
  }
  if (/تاخیر|تأخیر/.test(msg)) {
    const r = await runAction('late_report', {}, userName)
    return { reply: r.summary, result: r }
  }
  if (/مرخصی/.test(msg)) {
    const nameMatch = msg.match(/برای\s+(.+?)(\s+از|\s*$)/)
    const name = nameMatch ? nameMatch[1] : ''
    if (name) {
      const r = await runAction('leave_register', { name, days: num }, userName)
      return { reply: r.summary, result: r }
    }
    return { reply: 'برای ثبت مرخصی بنویسید: «مرخصی ۲ روز برای [نام پرسنل]»' }
  }
  const personMatch = msg.match(/^(?:گزارش|وضعیت)\s+(.+?)(?:\s+(?:دیروز|امروز))?$/)
  if (personMatch) {
    const r = await runAction('person_report', { name: personMatch[1] }, userName)
    return { reply: r.summary, result: r }
  }
  return {
    reply:
      'می‌توانید این دستورها را امتحان کنید:\n• «آمار امروز»\n• «لیست غایبان»\n• «حاضرین الان»\n• «گزارش تأخیرها»\n• «اضافه‌کاری دیروز»\n• «مرخصی ۲ روز برای علی رضایی»\n• «گزارش [نام پرسنل]»',
  }
}

const SYSTEM_PROMPT = `شما «دستیار حضور+» هستید؛ دستیار هوشمند سامانه حضور و غیاب. کاربر مدیر سیستم است و به فارسی دستور می‌دهد.
شما باید ONLY یک JSON معتبر برگردانید، بدون متن اضافه، با ساختار:
{"action": "<نام اکشن>", "args": {...}, "narrative": "<پاسخ کوتاه فارسی به کاربر>"}
اکشن‌های مجاز:
- today_stats: آمار امروز (بدون آرگومان)
- absent_list: لیست غایبان دیروز یا تاریخ مشخص {"date": "YYYY-MM-DD"}
- present_now: حاضرین امروز
- late_report: گزارش تأخیرها {"date"?}
- overtime_report: گزارش اضافه‌کاری {"date"?}
- person_report: وضعیت فرد مشخص {"name": "...", "date"?}
- leave_register: ثبت مرخصی {"name": "...", "days": عدد}
اگر دستور کاربر با هیچ اکشنی مطابقت نداشت، action را "none" بگذارید و در narrative راهنمایی کوتاه بدهید.`

export async function POST(req: NextRequest) {
  try {
    const user = await requireRole('APPROVER')
    const { message } = await req.json()
    if (!message?.trim()) return NextResponse.json({ error: 'پیام خالی است' }, { status: 400 })

    let reply = ''
    let result: ToolResult | undefined
    let engine = 'rules'

    try {
      const ZAI = (await import('z-ai-web-dev-sdk')).default
      const zai = await ZAI.create()
      const completion = await zai.chat.completions.create({
        messages: [
          { role: 'assistant', content: SYSTEM_PROMPT },
          { role: 'user', content: String(message).slice(0, 500) },
        ],
        thinking: { type: 'disabled' },
      })
      const content = completion.choices?.[0]?.message?.content ?? ''
      const jsonMatch = content.match(/\{[\s\S]*\}/)
      if (jsonMatch) {
        const parsed = JSON.parse(jsonMatch[0])
        if (parsed.action && parsed.action !== 'none') {
          result = await runAction(parsed.action, parsed.args ?? {}, user.fullName)
        }
        const narrative = String(parsed.narrative || '').trim()
        if (result?.summary && (!narrative || narrative.length < 8 || narrative.includes('در حال') || narrative.includes('...'))) {
          reply = result.summary
        } else if (result?.summary && narrative && !narrative.includes(result.summary.slice(0, 12))) {
          reply = `${narrative}\n${result.summary}`
        } else {
          reply = narrative || result?.summary || 'انجام شد'
        }
        engine = 'ai'
      } else {
        const rb = await ruleBasedReply(String(message), user.fullName)
        reply = rb.reply
        result = rb.result
      }
    } catch {
      const rb = await ruleBasedReply(String(message), user.fullName)
      reply = rb.reply
      result = rb.result
      engine = 'rules'
    }

    return NextResponse.json({ reply, result, engine })
  } catch (err) {
    return fail(err)
  }
}


