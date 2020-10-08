export { default as OldVideoUpload } from '../..\\components\\Old-video-upload.vue'
export { default as ContentCreationDialog } from '../..\\components\\content-creation\\content-creation-dialog.vue'
export { default as VideoUploadSteps } from '../..\\components\\content-creation\\video-upload-steps.vue'

export const LazyOldVideoUpload = import('../..\\components\\Old-video-upload.vue' /* webpackChunkName: "components_Old-video-upload" */).then(c => c.default || c)
export const LazyContentCreationDialog = import('../..\\components\\content-creation\\content-creation-dialog.vue' /* webpackChunkName: "components_content-creation/content-creation-dialog" */).then(c => c.default || c)
export const LazyVideoUploadSteps = import('../..\\components\\content-creation\\video-upload-steps.vue' /* webpackChunkName: "components_content-creation/video-upload-steps" */).then(c => c.default || c)
