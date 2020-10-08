<template>
  <v-dialog :value="active" persistent>
    <template v-slot:activator="{on}">
      <v-menu offset-y>
          <template v-slot:activator="{on, attrs}">
          <v-btn depressed v-bind="attrs" v-on="on">
              Create
          </v-btn>
          </template>
          <v-list>
              <v-list-item v-for="(item, i) in menuItems" :key="`ccd-menu-${i}`" 
              @click="activate({component: item.component})">
                  <v-list-item-title>{{ item.title }}</v-list-item-title>
              </v-list-item>
          </v-list>
      </v-menu>
    </template>

   <div v-if="component">
       <component :is="component"> </component>
   </div>

    <div class="d-flex justify-center my-4">
      <v-btn @click="reset">
        Close
      </v-btn>
    </div>

  </v-dialog>
</template>

<script>
  import { mapState, mapActions, mapMutations } from 'vuex';
  import VideoUploadSteps from "./video-upload-steps"
  export default {
    name: "content-creation-dialog",
    components: {VideoUploadSteps},
    computed: {
      ...mapState('videoFiles', ['active','component']),
      menuItems(){
          return [
              {component: VideoUploadSteps, title: "Video"},
          ]
      }
    },
    methods: {
      ...mapMutations('videoFiles', ['reset', 'activate']),
    }
  }
</script>

<style scoped>

</style>