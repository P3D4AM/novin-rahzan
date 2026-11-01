import { createServer } from 'http'
import { Server } from 'socket.io'

const PORT = 3003
const EMIT_PORT = 3004

const io = new Server(
  createServer(),
  {
    path: '/',
    cors: { origin: '*', methods: ['GET', 'POST'] },
    pingTimeout: 60000,
    pingInterval: 25000,
  }
)

// internal HTTP endpoint used by Next.js API routes to broadcast events
const emitServer = createServer((req, res) => {
  if (req.method === 'POST' && req.url === '/emit') {
    let body = ''
    req.on('data', (chunk: Buffer) => (body += chunk))
    req.on('end', () => {
      try {
        const { event, payload } = JSON.parse(body)
        io.emit('rt', { event, payload, ts: Date.now() })
        res.writeHead(200, { 'Content-Type': 'application/json' })
        res.end(JSON.stringify({ ok: true }))
      } catch {
        res.writeHead(400)
        res.end('bad request')
      }
    })
    return
  }
  if (req.method === 'GET' && req.url === '/health') {
    res.writeHead(200, { 'Content-Type': 'application/json' })
    res.end(JSON.stringify({ ok: true, clients: io.engine.clientsCount }))
    return
  }
  res.writeHead(404)
  res.end('not found')
})

io.on('connection', (socket) => {
  socket.emit('rt', { event: 'hello', payload: { clients: io.engine.clientsCount } })
})

emitServer.listen(EMIT_PORT, () => {
  console.log(`[realtime] emit endpoint on :${EMIT_PORT}`)
})

io.listen(PORT)
console.log(`[realtime] socket.io listening on :${PORT}`)
