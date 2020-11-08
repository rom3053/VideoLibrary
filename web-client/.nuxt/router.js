import Vue from 'vue'
import Router from 'vue-router'
import { interopDefault } from './utils'
import scrollBehavior from './router.scrollBehavior.js'

const _401375a6 = () => interopDefault(import('..\\pages\\inspire.vue' /* webpackChunkName: "pages/inspire" */))
const _8e1ab056 = () => interopDefault(import('..\\pages\\videos\\sselect-quality.vue' /* webpackChunkName: "pages/videos/sselect-quality" */))
const _7dcd4eb8 = () => interopDefault(import('..\\pages\\videos\\_videoSubmission.vue' /* webpackChunkName: "pages/videos/_videoSubmission" */))
const _e17fa6c4 = () => interopDefault(import('..\\pages\\index.vue' /* webpackChunkName: "pages/index" */))

// TODO: remove in Nuxt 3
const emptyFn = () => {}
const originalPush = Router.prototype.push
Router.prototype.push = function push (location, onComplete = emptyFn, onAbort) {
  return originalPush.call(this, location, onComplete, onAbort)
}

Vue.use(Router)

export const routerOptions = {
  mode: 'history',
  base: decodeURI('/'),
  linkActiveClass: 'nuxt-link-active',
  linkExactActiveClass: 'nuxt-link-exact-active',
  scrollBehavior,

  routes: [{
    path: "/inspire",
    component: _401375a6,
    name: "inspire"
  }, {
    path: "/videos/sselect-quality",
    component: _8e1ab056,
    name: "videos-sselect-quality"
  }, {
    path: "/videos/:videoSubmission?",
    component: _7dcd4eb8,
    name: "videos-videoSubmission"
  }, {
    path: "/",
    component: _e17fa6c4,
    name: "index"
  }],

  fallback: false
}

export function createRouter () {
  return new Router(routerOptions)
}
