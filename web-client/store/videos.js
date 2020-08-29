import Axios from "axios";

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
    const videos = (await Axios.get("http://localhost:5000/api/videos")).data;
    console.log("videos:", videos);
    commit("setVideos", { videos })
  },

  async createVideo({ commit, dispatch }, {video}) {
    await Axios.post("http://localhost:5000/api/videos", video);
    await dispatch('fetchVideos')
  }

}
