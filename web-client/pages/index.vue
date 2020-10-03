<template>
  <div class="d-flex justify-center">
    
    <v-container
     fluid
     v-if="videos"
    >
      <v-row
      no-gutters
      >
      
      <v-col 
      v-for="vv in videos"
      v-bind:key="vv"
      cols="4"
      class="md-lg-auto"
      >
      <v-card
      class="pa-ma-8"
      tile
      flat
      >
      <div>
        <v-btn :to="`/videos/${vv.id}`" depressed min-height="100" min-width="100" outlined></v-btn>
        <p >VideoName - {{vv.name}}</p>
        <p >UserName - {{vv.id}}</p>
      </div>

      </v-card>

      </v-col>
      </v-row>
    </v-container>
    

   <!-- <div v-if="videos">
      <div v-bind:key="v" v-for="v in videos">
        <v-btn :to="`/videos/${v.id}`"  depressed height="150" width="100" outlined>
            {{v.name}} 
        </v-btn>
      </div>
    </div> -->
   

  </div>
</template>

<script>

  import { mapState, mapActions, mapMutations } from 'vuex';

  export default {
    computed: {
      ...mapState('videos', ['videos']),
      
    },
    async fetch(){
      await this.$store.dispatch("videos/fetchVideos", null, {root: true});
    }
    
}
</script>
