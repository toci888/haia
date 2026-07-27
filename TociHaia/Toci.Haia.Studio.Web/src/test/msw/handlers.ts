import { http, HttpResponse } from 'msw'

const defaultSystemInfo = {
	serviceName: 'HAIA Studio API',
	apiVersion: '1.0.0.0',
	status: 'ok',
}

export const handlers = [
	http.get('/api/v1/system/info', () => HttpResponse.json(defaultSystemInfo, { status: 200 })),
	http.get('/health/live', () => HttpResponse.json({ status: 'live' }, { status: 200 })),
	http.get('/health/ready', () => HttpResponse.json({ status: 'ready' }, { status: 200 })),
]
