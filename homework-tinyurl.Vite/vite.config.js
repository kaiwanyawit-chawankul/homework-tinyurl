import { defineConfig, loadEnv } from 'vite'
import vue from '@vitejs/plugin-vue'

// https://vite.dev/config/
export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd(), '');
    console.log('VITE_API_URL', env.VITE_API_URL);
  return {
      plugins: [vue()],
      server: {
          port: parseInt(env.VITE_PORT),
          proxy: {
            '/api': {
              target: env.VITE_API_URL,
              changeOrigin: true,
              rewrite: (path) => path.replace(/^\/api/, ''),
              configure: (proxy, options) => {
                proxy.on('proxyReq', (proxyReq, req, res) => {
                  console.log(`[proxy] ${req.method} ${req.url} -> ${proxyReq.protocol}//${proxyReq.host}${proxyReq.path}`);
                });
              }
            },
          },
      },
      build: {
          outDir: 'dist',
          rollupOptions: {
              input: './index.html'
          }
      }
  }
})
