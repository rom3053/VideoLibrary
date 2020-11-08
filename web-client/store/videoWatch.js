const initState = () => ({
  quality: "asdadaw2e2qdqwd"
})

export const state = initState

export const mutations = {
  setQuality(state, { quality }) {
    state.quality = quality;
  },
  reset(state) {
    Object.assign(state, initState())
  }
}

export const actions = {

}
