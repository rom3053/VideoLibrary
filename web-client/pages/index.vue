<template>
  <div>

    <div v-if="videos">
      <div v-for="v in videos">
        {{v.id}} - {{v.name}}
      </div>
    </div>
   
    <div v-if="submissions">
      <div v-for="s in submissions">
        {{s.id}} - {{s.description}} - {{s.videoId}}
        <div>
          <video width="400" controls :src="`http://localhost:5000/api/videoFiles/${s.videoFile}`"> </video>
        </div>
      </div>
    </div>

  </div>
</template>

<script>

  import { mapState, mapActions, mapMutations } from 'vuex';

  export default {
    computed: {
      ...mapState('videos', ['videos']),
      ...mapState('submissions', ['submissions'])
    }
    
}
</script>
