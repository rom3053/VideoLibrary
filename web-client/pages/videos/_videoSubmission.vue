<template>
  <div>
      <div>
            Video: {{$route.params.videoSubmission }}
      </div> 
    <v-btn @click="changeVideo">CHANGE VIDEO</v-btn>
    <div v-if="submissions">
      <div v-bind:key="s" v-for="s in submissions">
        {{s.id}} - {{s.description}} - {{s.videoId}} -
        {{videolinkFirst}} --{{videolinkSecond}}
        <div>
          <video width="400" :src="video" controls>
            

          </video>
          <!--"https://archive.org/download/BigBuckBunny_124/Content/big_buck_bunny_720p_surround.mp4
            `http://localhost:5000/api/videoFiles/${s.videoFile}`"-->
        </div>
      </div>
    </div>
  </div>
    
</template>

<script>
import {mapState} from 'vuex';

  export default {
    data: ()=> ({
      videolinkSecond: "https://archive.org/download/BigBuckBunny_124/Content/big_buck_bunny_720p_surround.mp4",
      videolinkFirst: "http://localhost:5000/api/videoFiles/",
      video:"",
    }),
    methods: {
      changeVideo() {
        if (this.video == this.videolinkFirst) {
          this.video = this.videolinkSecond
        } else {
          this.video = this.videolinkFirst
        }
      }
    },
    computed: mapState('submissions',['submissions']),
    async fetch(){
        const videoId = this.$route.params.videoSubmission;
      await this.$store.dispatch("submissions/fetchSubmissions", { videoId }, { root: true })
      this.videolinkFirst += this.submissions[0].videoFile
      this.video = this.videolinkFirst
    }
}
</script>

<style scoped>

</style>
