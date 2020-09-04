<template>
  <div>
    <v-file-input accept="video/*" @change="handleFile"> </v-file-input>
    <form action="">
      <input name="">
    </form>
    <div v-if="videos">
      <p v-for="v in videos">
        {{v.name}}
      </p>
    </div>

    <div>
      <v-text-field label="Videoing Name" v-model="videoName"></v-text-field>
      <v-btn @click="saveVideo">Save Video</v-btn>
    </div>

    {{message}}
    <v-btn @click="reset">Reset Message</v-btn>
    <v-btn @click="resetVideos">Reset Videos</v-btn>

  </div>
</template>

<script>

import Axios from 'axios'
  import { mapState, mapActions, mapMutations } from 'vuex';

  export default {
    data: () => ({
      videoName: ""
    }),
    computed: {
      ...mapState({
        message: state => state.message
      }),
      ...mapState('videos', {
        videos: state => state.videos
      }) 
    },
    methods: {
      ...mapMutations([
        'reset'
      ]),
      ...mapMutations('videos', {
        resetVideos: 'reset'
      }),
      ...mapActions('videos', ['createVideo']),
      async saveVideo() {
        await this.createVideo({ video: { name: this.videoName } });
        this.videoName = "";
      },
      async handleFile(files){
        if(!file) return;
        
        const form = new FormData();
        form.append("video", file)

        const result = await Axios.post("http://localhost:5000/api/videoFiles", form);
        console.log("Result",result)
      }
    }



    //async fetch (){
    //  await this.$store.dispatch('fetchMessage');
    //}
    //asyncData(payload){
    //  return Axios.get("http://localhost:5000/api/home")
    //    .then(({data}) => {
    //      return { message: data }
    //    })
    //}
}
</script>
