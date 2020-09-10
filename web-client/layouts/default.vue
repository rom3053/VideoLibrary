<template>
  <v-app dark>
    <v-app-bar app dense>
      <v-toolbar-title>Videos Library</v-toolbar-title>

      <v-spacer></v-spacer>

      <v-btn depressed @click="toggleActivity">
        Upload
      </v-btn>

    </v-app-bar>
    <video-upload/>
    <v-main>

      <nuxt />

    </v-main>


  </v-app>
</template>

<script>
  import { mapMutations } from 'vuex'
  import VideoUpload from '../components/video-upload.vue'
  export default {
    methods: mapMutations('videoFiles', ['toggleActivity']),
  
  }
</script>
