
const initState = () => ({
  videos: []
})

export const state = initState

export const mutations = {
  setVideos(state, {videos}) {
    state.videos = videos
  },
  reset(state) {
    Object.assign(state, initState())
  }
}

export const actions = {
  async fetchVideos({ commit }) {
    const videos = await this.$axios.$get("http://localhost:5000/api/videos");
    console.log("videos:", videos);
    commit("setVideos", { videos })
  },
}
