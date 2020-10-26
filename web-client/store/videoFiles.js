
const initState = () => ({
  uploadPromise: null,
  active: false,
  component: null,
})

export const state = initState

export const mutations = {
  activate(state, {component}) {
    state.active = true;
    state.component = component
  },
  hide(state){
    state.active = false;
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
  startResearch({ commit, dispatch }) {
    this.$axios.$post("http://localhost:5000/api/videoFiles/research");
  },
  async createVideo({state, commit, dispatch }, { video, submission }) {
    
      const createdVideo = await this.$axios.$post("/api/videos", video)
      submission.videoId = createdVideo.id
   
    await this.$axios.$post("/api/submissions", submission)
    
  }

}
