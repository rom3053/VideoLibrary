import { UPLOAD_TYPE } from "../data/enum"

const initState = () => ({
  uploadPromise: null,
  active: false,
  type: "",
  step: 1,
})

export const state = initState

export const mutations = {
  toggleActivity(state) {
    state.active = !state.active
    if (!state.active) {
      Object.assign(state, initState())
    }
  },
  setType(state, { type }) {
    state.type = type
    if (type === UPLOAD_TYPE.VIDEO) {
      state.step++
    }
    else if (type === UPLOAD_TYPE.SUBMISSION) {
      state.step += 2;
    }
    
  },
  setTask(state, { uploadPromise }) {
    state.uploadPromise = uploadPromise
    state.step++
  },
  incStep(state) {
    state.step++;
  },
  reset(state) {
    Object.assign(state, initState())
  }
}

export const actions = {
  startVideoUpload({ commit, dispatch }, { form }) {
    const uploadPromise = this.$axios.$post("http://localhost:5000/api/videoFiles", form);
    commit("setTask", {uploadPromise})
  },
  async createVideo({state, commit, dispatch }, { video, submission }) {
    if (state.type === UPLOAD_TYPE.VIDEO) {
      const createdVideo = await this.$axios.$post("/api/videos", video)
      submission.videoId = createdVideo.id
    }
   
    const createdSubmission = await this.$axios.$post("/api/submissions", submission)
    await dispatch('videos/fetchVideos', null, { root: true })
    await dispatch('submissions/fetchSubmissions', null, { root: true })
  }

}
