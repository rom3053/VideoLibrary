<template>
  <div>

   
    <div v-if="videos">
      <div v-for="v in videos">
        {{v.name}}
        <div>
          <video width="400" controls :src="`http://localhost:5000/api/videoFiles/${v.videoFile}`"> </video>
        </div>
      </div>
    </div>

  </div>
</template>

<script>

  import { mapState, mapActions, mapMutations } from 'vuex';

  export default {
    computed: mapState('videos', ['videos']),
      
    
}
</script>
