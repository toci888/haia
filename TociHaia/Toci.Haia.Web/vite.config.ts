import react from '@vitejs/plugin-react'
import { defineConfig as defineVitestConfig } from 'vitest/config'

// https://vite.dev/config/
export default defineVitestConfig({
  plugins: [react()],
  server: {
	proxy: {
	  '/api': {
		target: process.env.VITE_API_PROXY_TARGET || 'http://localhost:5035',
		changeOrigin: true,
	  },
	  '/health': {
		target: process.env.VITE_API_PROXY_TARGET || 'http://localhost:5035',
		changeOrigin: true,
	  },
	  '/openapi': {
		target: process.env.VITE_API_PROXY_TARGET || 'http://localhost:5035',
		changeOrigin: true,
	  },
	},
  },
  test: {
	environment: 'jsdom',
	environmentOptions: {
	  jsdom: {
		url: 'http://localhost/',
	  },
	},
	setupFiles: './src/test/setupTests.ts',
	globals: true,
	css: true,
  },
})
