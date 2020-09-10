
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
    state.step++
  },
  setTask(state, { uploadPromise }) {
    state.uploadPromise = uploadPromise
    state.step++
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
  async createVideo({ commit, dispatch }, { video }) {
    await this.$axios.post("/api/videos", video)
    await dispatch('videos/fetchVideos')
  }

}
