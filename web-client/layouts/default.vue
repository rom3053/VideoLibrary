<template>
  <v-app dark>
    <v-app-bar app dense>
      <v-toolbar-title>
      <router-link tag="v-toolbar-title" :to="`/`">Video Library</router-link>
      </v-toolbar-title>

      <v-spacer></v-spacer>


    <content-creation-dialog></content-creation-dialog>
    </v-app-bar>
    
    <v-main>
      <nuxt />
    </v-main>


  </v-app>
</template>

<script>
  import ContentCreationDialog from '../components/content-creation/content-creation-dialog'
  export default {
    components: {ContentCreationDialog}
  
  }
</script>
