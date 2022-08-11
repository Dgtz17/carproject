if ('serviceWorker' in navigator) {
    window.addEventListener('load', function() {
      navigator.serviceWorker.register('/service-worker.js');
    });
  }

  const addResourcesToCache = async (resources) => {
    const cache = await caches.open('v1');
    await cache.addAll(resources);
  };
  
  const putInCache = async (request, response) => {
    const cache = await caches.open('v1');
    await cache.put(request, response);
  };
  
  const cacheFirst = async ({ request, preloadResponsePromise, fallbackUrl }) => {
    // First try to get the resource from the cache
    const responseFromCache = await caches.match(request);
    if (responseFromCache) {
      return responseFromCache;
    }
  
    const preloadResponse = await preloadResponsePromise;
    if (preloadResponse) {
      console.info('using preload response', preloadResponse);
      putInCache(request, preloadResponse.clone());
      return preloadResponse;
    }
  
    try {
      const responseFromNetwork = await fetch(request);

      putInCache(request, responseFromNetwork.clone());
      return responseFromNetwork;
    } catch (error) {
      const fallbackResponse = await caches.match(fallbackUrl);
      if (fallbackResponse) {
        return fallbackResponse;
      }
   
      return new Response('Network error happened', {
        status: 408,
        headers: { 'Content-Type': 'text/plain' },
      });
    }
  };
  
  const enableNavigationPreload = async () => {
    if (self.registration.navigationPreload) {
      await self.registration.navigationPreload.enable();
    }
  };
  
  self.addEventListener('activate', (event) => {
    event.waitUntil(enableNavigationPreload());
  });
  
  self.addEventListener('install', (event) => {
    event.waitUntil(
      addResourcesToCache([
        '..//sw-test/',
        '/sw-test/index.html',
        '/sw-test/style.css',
        '/sw-test/app.js',
        '/sw-test/image-list.js',
        '/sw-test/star-wars-logo.jpg',
        '/sw-test/gallery/bountyHunters.jpg',
        '/sw-test/gallery/myLittleVader.jpg',
        '/sw-test/gallery/snowTroopers.jpg',
      ])
    );
  });
  
  self.addEventListener('fetch', (event) => {
    event.respondWith(
      cacheFirst({
        request: event.request,
        preloadResponsePromise: event.preloadResponse,
        fallbackUrl: '/sw-test/gallery/myLittleVader.jpg',
      })
    );
  });
  