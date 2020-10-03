<template>
  <div>
      <div>
            Video: {{$route.params.videoSubmission }}
      </div> 

    <div v-if="submissions">
      <div v-bind:key="s" v-for="s in submissions">
        {{s.id}} - {{s.description}} - {{s.videoId}}
        <div>
          <video width="400" controls :src="`http://localhost:5000/api/videoFiles/${s.videoFile}`"> </video>
        </div>
      </div>
    </div>
  </div>
    
</template>

<script>
import {mapState} from 'vuex';

export default {
    computed: mapState('submissions',['submissions']),
    async fetch(){
        const videoId = this.$route.params.videoSubmission;
        await this.$store.dispatch("submissions/fetchSubmissions", {videoId}, {root: true})
    }
}
</script>

<style scoped>

</style>