'use client'

import { useRef, useState } from 'react'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Badge } from '@/components/ui/badge'
import { SessionUser, api } from './types'
import { Sparkles, Send, Loader2, User, Bot } from 'lucide-react'
import { useToast } from '@/hooks/use-toast'

interface Msg { role: 'user' | 'assistant'; text: string; action?: string }

const SUGGESTIONS = [
  'آمار امروز',
  'لیست غایبان',
  'حاضرین الان',
  'گزارش تأخیرها',
  'اضافه‌کاری دیروز',
  'مرخصی ۲ روز برای علی رضایی',
]

export function AssistantView({ user }: { user: SessionUser }) {
  const [messages, setMessages] = useState<Msg[]>([
    { role: 'assistant', text: 'سلام! من دستیار هوشمند حضور+ هستم 👋\nمی‌توانید وضعیت امروز، غایبان، تأخیرها، اضافه‌کاری یا ثبت مرخصی را از من بخواهید.' },
  ])
  const [input, setInput] = useState('')
  const [loading, setLoading] = useState(false)
  const [engine, setEngine] = useState<'ai' | 'rules' | null>(null)
  const bottomRef = useRef<HTMLDivElement>(null)
  const { toast } = useToast()

  const send = async (text?: string) => {
    const msg = (text ?? input).trim()
    if (!msg || loading) return
    setInput('')
    setMessages((prev) => [...prev, { role: 'user', text: msg }])
    setLoading(true)
    try {
      const res = await api<{ reply: string; result?: { action: string; summary: string }; engine: string }>('/api/ai', {
        method: 'POST',
        body: JSON.stringify({ message: msg }),
      })
      setEngine(res.engine === 'ai' ? 'ai' : 'rules')
      setMessages((prev) => [...prev, { role: 'assistant', text: res.reply, action: res.result?.action }])
    } catch (err) {
      toast({ title: 'خطا', description: err instanceof Error ? err.message : '', variant: 'destructive' })
      setMessages((prev) => [...prev, { role: 'assistant', text: 'متأسفانه خطایی رخ داد. دوباره تلاش کنید.' }])
    } finally {
      setLoading(false)
      setTimeout(() => bottomRef.current?.scrollIntoView({ behavior: 'smooth' }), 100)
    }
  }

  return (
    <div className="max-w-3xl mx-auto space-y-4">
      <Card>
        <CardHeader className="pb-3">
          <CardTitle className="text-base flex items-center gap-2">
            <span className="h-8 w-8 rounded-xl bg-primary/10 text-primary flex items-center justify-center"><Sparkles className="h-4 w-4" /></span>
            دستیار هوشمند حضور+
            {engine && <Badge variant="secondary" className="text-[10px]">{engine === 'ai' ? 'موتور AI' : 'موتور قاعده‌محور'}</Badge>}
          </CardTitle>
        </CardHeader>
        <CardContent>
          <div className="flex flex-wrap gap-1.5 mb-4">
            {SUGGESTIONS.map((s) => (
              <Button key={s} variant="secondary" size="sm" className="h-7 text-xs rounded-full" onClick={() => send(s)} disabled={loading}>{s}</Button>
            ))}
          </div>

          <div className="space-y-3 max-h-[55vh] overflow-y-auto p-1">
            {messages.map((m, i) => (
              <div key={i} className={`flex gap-2 ${m.role === 'user' ? 'flex-row-reverse' : ''}`}>
                <div className={`h-8 w-8 rounded-full shrink-0 flex items-center justify-center ${m.role === 'user' ? 'bg-primary text-primary-foreground' : 'bg-muted'}`}>
                  {m.role === 'user' ? <User className="h-4 w-4" /> : <Bot className="h-4 w-4" />}
                </div>
                <div className={`rounded-2xl px-4 py-2.5 text-sm max-w-[80%] whitespace-pre-wrap ${m.role === 'user' ? 'bg-primary text-primary-foreground rounded-tr-none' : 'bg-muted rounded-tl-none'}`}>
                  {m.text}
                </div>
              </div>
            ))}
            {loading && (
              <div className="flex gap-2">
                <div className="h-8 w-8 rounded-full bg-muted flex items-center justify-center"><Bot className="h-4 w-4" /></div>
                <div className="rounded-2xl px-4 py-2.5 bg-muted flex items-center gap-2 text-sm text-muted-foreground">
                  <Loader2 className="h-4 w-4 animate-spin" /> در حال بررسی…
                </div>
              </div>
            )}
            <div ref={bottomRef} />
          </div>

          <form className="flex gap-2 mt-4" onSubmit={(e) => { e.preventDefault(); send() }}>
            <Input value={input} onChange={(e) => setInput(e.target.value)} placeholder="دستور خود را بنویسید… مثلاً: لیست غایبان" disabled={loading} />
            <Button type="submit" disabled={loading || !input.trim()}><Send className="h-4 w-4" /></Button>
          </form>
        </CardContent>
      </Card>
    </div>
  )
}
