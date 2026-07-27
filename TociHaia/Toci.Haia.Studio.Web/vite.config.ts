import { defineConfig as defineVitestConfig } from 'vitest/config'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineVitestConfig({
  plugins: [react()],
  server: {
	proxy: {
	  '/api': {
		target: process.env.VITE_STUDIO_API_PROXY_TARGET || 'http://localhost:5065',
		changeOrigin: true,
	  },
	  '/health': {
		target: process.env.VITE_STUDIO_API_PROXY_TARGET || 'http://localhost:5065',
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
