'use client'

import { useEffect, useRef, useState } from 'react'
import { Button } from '@/components/ui/button'
import { Card, CardContent } from '@/components/ui/card'
import { api } from './types'
import { useTehranClock } from './jalali-ui'
import { Delete, LogIn, LogOut, QrCode, Keyboard, Loader2, CheckCircle2, XCircle, Fingerprint, RotateCcw } from 'lucide-react'

interface PunchResult { ok: boolean; direction?: string; person?: { name: string; code: string }; error?: string; timestamp?: string }

export function KioskView({ onBack }: { onBack: () => void }) {
  const [mode, setMode] = useState<'pin' | 'qr' | null>(null)
  const [code, setCode] = useState('')
  const [pin, setPin] = useState('')
  const [loading, setLoading] = useState(false)
  const [result, setResult] = useState<PunchResult | null>(null)
  const { time, dateFa } = useTehranClock()
  const resultTimer = useRef<ReturnType<typeof setTimeout> | null>(null)

  useEffect(() => () => { if (resultTimer.current) clearTimeout(resultTimer.current) }, [])

  const submitPin = async () => {
    if (!code || pin.length < 4) return
    setLoading(true)
    try {
      const res = await api<PunchResult>('/api/kiosk', { method: 'POST', body: JSON.stringify({ mode: 'pin', code, pin }) })
      setResult(res)
      setCode(''); setPin('')
      if (resultTimer.current) clearTimeout(resultTimer.current)
      resultTimer.current = setTimeout(() => setResult(null), 5000)
    } catch (err) {
      setResult({ ok: false, error: err instanceof Error ? err.message : 'خطا' })
      setPin('') // allow re-entry after failure
      if (resultTimer.current) clearTimeout(resultTimer.current)
      resultTimer.current = setTimeout(() => setResult(null), 5000)
    } finally { setLoading(false) }
  }

  const submitQr = async (token: string) => {
    setLoading(true)
    try {
      const res = await api<PunchResult>('/api/kiosk', { method: 'POST', body: JSON.stringify({ mode: 'qr', token }) })
      setResult(res)
      if (resultTimer.current) clearTimeout(resultTimer.current)
      resultTimer.current = setTimeout(() => setResult(null), 5000)
    } catch (err) {
      setResult({ ok: false, error: err instanceof Error ? err.message : 'خطا' })
      setTimeout(() => setResult(null), 5000)
    } finally { setLoading(false) }
  }

  const keypadPress = (key: string) => {
    if (mode !== 'pin') return
    if (key === 'del') { setPin(pin.slice(0, -1)); return }
    if (key === 'clr') { setPin(''); setCode(''); return }
    if (pin.length < 4) setPin(pin + key)
  }

  return (
    <div className="min-h-screen flex flex-col items-center justify-center bg-gradient-to-br from-emerald-50 to-teal-100 dark:from-gray-900 dark:to-gray-800 p-4" dir="rtl">
      <div className="text-center mb-6 space-y-1">
        <div className="inline-flex h-14 w-14 items-center justify-center rounded-2xl bg-primary text-primary-foreground shadow-lg mb-2">
          <Fingerprint className="h-7 w-7" />
        </div>
        <h1 className="text-2xl font-bold">ثبت حضور — حالت کیشی</h1>
        <div className="text-4xl font-bold tabular-nums text-primary">{time}</div>
        <div className="text-muted-foreground">{dateFa}</div>
      </div>

      {result && (
        <Card className={`w-full max-w-sm mb-4 border-2 ${result.ok ? 'border-emerald-400' : 'border-red-400'} animate-in zoom-in-95`}>
          <CardContent className="p-6 text-center space-y-2">
            {result.ok ? (
              <>
                <CheckCircle2 className="h-14 w-14 mx-auto text-emerald-500" />
                <div className="text-xl font-bold">{result.person?.name}</div>
                <div className="text-lg text-emerald-600 font-semibold flex items-center justify-center gap-2">
                  {result.direction === 'IN' ? <><LogIn className="h-5 w-5" /> ورود ثبت شد</> : <><LogOut className="h-5 w-5" /> خروج ثبت شد</>}
                </div>
              </>
            ) : (
              <>
                <XCircle className="h-14 w-14 mx-auto text-red-500" />
                <div className="text-lg font-semibold text-red-600">{result.error}</div>
              </>
            )}
          </CardContent>
        </Card>
      )}

      {!mode && (
        <div className="grid grid-cols-2 gap-3 w-full max-w-md">
          <Button size="lg" className="h-24 flex-col gap-2 text-base" onClick={() => setMode('pin')}>
            <Keyboard className="h-8 w-8" /> کد پرسنلی + پین
          </Button>
          <Button size="lg" variant="outline" className="h-24 flex-col gap-2 text-base" onClick={() => setMode('qr')}>
            <QrCode className="h-8 w-8" /> اسکن QR
          </Button>
        </div>
      )}

      {mode === 'pin' && (
        <Card className="w-full max-w-sm">
          <CardContent className="p-5 space-y-4">
            <div className="space-y-2">
              <input
                dir="ltr"
                inputMode="numeric"
                placeholder="کد پرسنلی (مثلاً 2011)"
                className="w-full text-center text-lg border rounded-xl px-3 py-3 bg-background"
                value={code}
                onChange={(e) => setCode(e.target.value.replace(/\D/g, ''))}
              />
              <div className="flex justify-center gap-2" dir="ltr">
                {[0, 1, 2, 3].map((i) => (
                  <div key={i} className={`h-3.5 w-3.5 rounded-full border-2 border-primary ${pin.length > i ? 'bg-primary' : ''}`} />
                ))}
              </div>
              <div className="text-center text-xs text-muted-foreground">PIN چهاررقمی</div>
            </div>

            <div className="grid grid-cols-3 gap-2" dir="ltr">
              {['1', '2', '3', '4', '5', '6', '7', '8', '9'].map((k) => (
                <Button key={k} variant="outline" className="h-14 text-xl" onClick={() => keypadPress(k)}>{k}</Button>
              ))}
              <Button variant="outline" className="h-14" onClick={() => keypadPress('del')}><Delete className="h-5 w-5" /></Button>
              <Button variant="outline" className="h-14 text-xl" onClick={() => keypadPress('0')}>0</Button>
              <Button variant="outline" className="h-14" onClick={() => keypadPress('clr')}><RotateCcw className="h-5 w-5" /></Button>
            </div>

            <div className="flex gap-2">
              <Button className="flex-1 h-12 text-base gap-2" onClick={submitPin} disabled={loading || !code || pin.length < 4}>
                {loading ? <Loader2 className="h-5 w-5 animate-spin" /> : <Fingerprint className="h-5 w-5" />}
                ثبت حضور
              </Button>
              <Button variant="ghost" onClick={() => { setMode(null); setCode(''); setPin('') }}>بازگشت</Button>
            </div>
          </CardContent>
        </Card>
      )}

      {mode === 'qr' && (
        <Card className="w-full max-w-sm">
          <CardContent className="p-5 space-y-4">
            <p className="text-sm text-muted-foreground text-center leading-6">
              کارت QR شخصی خود را با اسکنر (یا دوربین متصل به کیبورد) اسکن کنید.
              مقدار QR اینجا وارد می‌شود و حضور ثبت می‌گردد.
            </p>
            <input
              dir="ltr"
              autoFocus
              placeholder="hozorplus:xxxx-xxxx…"
              className="w-full text-center text-sm border rounded-xl px-3 py-3 bg-background"
              onKeyDown={(e) => {
                if (e.key === 'Enter') {
                  const v = (e.target as HTMLInputElement).value.trim()
                  if (v) submitQr(v.replace(/^hozorplus:/i, ''))
                  ;(e.target as HTMLInputElement).value = ''
                }
              }}
            />
            <Button variant="ghost" className="w-full" onClick={() => setMode(null)}>بازگشت</Button>
          </CardContent>
        </Card>
      )}

      <Button variant="link" className="mt-6 text-muted-foreground" onClick={onBack}>
        بازگشت به صفحه ورود مدیران
      </Button>
    </div>
  )
}
