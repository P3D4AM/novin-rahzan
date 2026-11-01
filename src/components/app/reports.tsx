'use client'

import { useEffect, useState } from 'react'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select'
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs'
import { SessionUser, Bootstrap, api, full_name } from './types'
import { toGregorian, jalaaliMonthLength } from 'jalaali-js'
import { MonthPicker, monthLabel, faDigits, durationFa, minutesToHM } from './jalali-ui'
import { useToast } from '@/hooks/use-toast'
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/table'
import { Download, RefreshCw, Loader2, FileSpreadsheet } from 'lucide-react'
import { ResponsiveContainer, BarChart, Bar, XAxis, YAxis, Tooltip, CartesianGrid } from 'recharts'

interface Props { user: SessionUser; boot: Bootstrap; reloadBoot: () => void }

interface SummaryRow {
  personId: string; code: string; name: string; department: string; jobTitle: string
  present: number; absent: number; leave: number
  workedMinutes: number; lateMinutes: number; overtimeMinutes: number; leaveMinutes: number; otApproved: number
}

export function ReportsView({ user, boot, reloadBoot }: Props) {
  const today = boot.today
  const [jy, setJy] = useState(today.jy)
  const [jm, setJm] = useState(today.jm)
  const [dept, setDept] = useState('all')
  const [summary, setSummary] = useState<SummaryRow[]>([])
  const [loading, setLoading] = useState(false)
  const { toast } = useToast()

  // compute month range (jalali -> gregorian iso)
  const monthRangeIso = () => {
    const days = jalaaliMonthLength(jy, jm)
    const s = toGregorian(jy, jm, 1)
    const e = toGregorian(jy, jm, days)
    return {
      from: new Date(Date.UTC(s.gy, s.gm - 1, s.gd)).toISOString(),
      to: new Date(Date.UTC(e.gy, e.gm - 1, e.gd)).toISOString(),
    }
  }

  const load = async () => {
    setLoading(true)
    try {
      const { from, to } = monthRangeIso()
      const res = await api<{ summary: SummaryRow[] }>(`/api/reports?type=monthly&from=${from}&to=${to}&departmentId=${dept === 'all' ? '' : dept}`)
      setSummary(res.summary)
    } catch (err) {
      toast({ title: 'خطا', description: err instanceof Error ? err.message : '', variant: 'destructive' })
    } finally { setLoading(false) }
  }

  useEffect(() => { load()   }, [jy, jm, dept])

  const exportExcel = async () => {
    const { from, to } = monthRangeIso()
    try {
      const res = await fetch(`/api/reports?type=monthly&from=${from}&to=${to}&departmentId=${dept === 'all' ? '' : dept}&export=xlsx`)
      if (!res.ok) throw new Error('خطا در تولید فایل')
      const blob = await res.blob()
      const url = URL.createObjectURL(blob)
      const a = document.createElement('a')
      a.href = url
      a.download = `hozorplus-${jy}-${jm}.xlsx`
      a.click()
      URL.revokeObjectURL(url)
    } catch (err) {
      toast({ title: 'خطا', description: err instanceof Error ? err.message : '', variant: 'destructive' })
    }
  }

  const chartData = summary.slice(0, 15).map((s) => ({
    name: s.name.split(' ')[0] + ' ' + (s.name.split(' ')[1]?.[0] ?? '') + '.',
    کارکرد: Math.round(s.workedMinutes / 60),
    اضافه: Math.round(s.overtimeMinutes / 60),
    تأخیر: Math.round(s.lateMinutes / 60),
  }))

  const totals = {
    worked: summary.reduce((a, s) => a + s.workedMinutes, 0),
    ot: summary.reduce((a, s) => a + s.overtimeMinutes, 0),
    late: summary.reduce((a, s) => a + s.lateMinutes, 0),
    absent: summary.reduce((a, s) => a + s.absent, 0),
  }

  return (
    <div className="space-y-4">
      <div className="flex flex-wrap items-center gap-2 no-print">
        <MonthPicker jy={jy} jm={jm} onChange={(y, m) => { setJy(y); setJm(m) }} />
        <Select value={dept} onValueChange={setDept}>
          <SelectTrigger className="w-40"><SelectValue /></SelectTrigger>
          <SelectContent>
            <SelectItem value="all">همه دپارتمان‌ها</SelectItem>
            {boot.departments.map((d) => <SelectItem key={d.id} value={d.id}>{d.name}</SelectItem>)}
          </SelectContent>
        </Select>
        <Button variant="outline" onClick={load} disabled={loading}><RefreshCw className={`h-4 w-4 ${loading ? 'animate-spin' : ''}`} /> بروزرسانی</Button>
        <Button onClick={exportExcel} className="gap-1.5"><FileSpreadsheet className="h-4 w-4" /> خروجی Excel</Button>
        <Button variant="outline" onClick={() => window.print()} className="gap-1.5"><Download className="h-4 w-4" /> چاپ / PDF</Button>
      </div>

      <div className="text-center text-lg font-semibold print-area">{monthLabel(jy, jm)}</div>

      <div className="flex flex-wrap gap-2 text-xs no-print">
        <span className="px-2 py-1 rounded-full bg-muted">جمع کارکرد: {minutesToHM(totals.worked)} ساعت</span>
        <span className="px-2 py-1 rounded-full bg-muted">جمع اضافه‌کاری: {minutesFa(totals.ot)}</span>
        <span className="px-2 py-1 rounded-full bg-muted">جمع تأخیر: {durationFa(totals.late)}</span>
        <span className="px-2 py-1 rounded-full bg-muted">غیبت: {faDigits(totals.absent)} روز</span>
      </div>

      <Card className="no-print">
        <CardHeader className="pb-2"><CardTitle className="text-base">مقایسه کارکرد و اضافه‌کاری (ساعت)</CardTitle></CardHeader>
        <CardContent>
          <div className="h-72" dir="ltr">
            <ResponsiveContainer width="100%" height="100%">
              <BarChart data={chartData}>
                <CartesianGrid strokeDasharray="3 3" stroke="#e5e7eb" />
                <XAxis dataKey="name" tick={{ fontSize: 10 }} interval={0} angle={-30} textAnchor="end" height={60} />
                <YAxis tick={{ fontSize: 11 }} />
                <Tooltip contentStyle={{ direction: 'rtl', borderRadius: 8, fontSize: 12 }} />
                <Bar dataKey="کارکرد" fill="#10b981" radius={[4, 4, 0, 0]} />
                <Bar dataKey="اضافه" fill="#14b8a6" radius={[4, 4, 0, 0]} />
                <Bar dataKey="تأخیر" fill="#f97316" radius={[4, 4, 0, 0]} />
              </BarChart>
            </ResponsiveContainer>
          </div>
        </CardContent>
      </Card>

      <Card className="print-area">
        <CardContent className="p-0">
          <div className="overflow-auto">
            <Table>
              <TableHeader>
                <TableRow>
                  <TableHead>#</TableHead>
                  <TableHead>کد</TableHead>
                  <TableHead>نام و نام خانوادگی</TableHead>
                  <TableHead className="hidden md:table-cell">دپارتمان</TableHead>
                  <TableHead>حضور</TableHead>
                  <TableHead>غیبت</TableHead>
                  <TableHead>مرخصی</TableHead>
                  <TableHead>کارکرد</TableHead>
                  <TableHead>تأخیر</TableHead>
                  <TableHead>اضافه‌کاری</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {summary.map((s, i) => (
                  <TableRow key={s.personId}>
                    <TableCell className="text-xs text-muted-foreground">{faDigits(i + 1)}</TableCell>
                    <TableCell dir="ltr" className="font-mono text-xs">{s.code}</TableCell>
                    <TableCell className="font-medium text-sm">{s.name}</TableCell>
                    <TableCell className="hidden md:table-cell text-xs text-muted-foreground">{s.department}</TableCell>
                    <TableCell className="text-sm text-emerald-600 font-medium">{faDigits(s.present)}</TableCell>
                    <TableCell className="text-sm text-red-600 font-medium">{s.absent ? faDigits(s.absent) : '—'}</TableCell>
                    <TableCell className="text-sm text-amber-600 font-medium">{s.leave ? faDigits(s.leave) : '—'}</TableCell>
                    <TableCell className="text-sm font-medium">{minutesToHM(s.workedMinutes)}</TableCell>
                    <TableCell className="text-sm">{s.lateMinutes ? <span className="text-orange-600">{durationFa(s.lateMinutes)}</span> : '—'}</TableCell>
                    <TableCell className="text-sm font-medium text-teal-600">{s.overtimeMinutes ? durationFa(s.overtimeMinutes) : '—'}</TableCell>
                  </TableRow>
                ))}
                {summary.length === 0 && <TableRow><TableCell colSpan={10} className="text-center text-muted-foreground py-10">داده‌ای برای این ماه نیست</TableCell></TableRow>}
              </TableBody>
            </Table>
          </div>
        </CardContent>
      </Card>
    </div>
  )
}

function minutesFa(m: number) { return durationFa(m) }

