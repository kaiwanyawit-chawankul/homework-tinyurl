import { defineConfig, loadEnv } from 'vite'
import vue from '@vitejs/plugin-vue'

// https://vite.dev/config/
export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd(), '');

  return {
      plugins: [vue()],
      server: {
          port: parseInt(env.VITE_PORT),
      },
      build: {
          outDir: 'dist',
          rollupOptions: {
              input: './index.html'
          }
      }
  }
})
