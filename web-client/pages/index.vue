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



    <v-stepper v-model="step">
      <v-stepper-header>
        <v-stepper-step :complete="step > 1" step="1">Upload Video</v-stepper-step>

        <v-divider></v-divider>

        <v-stepper-step :complete="step > 2" step="2">Video Information</v-stepper-step>

        <v-divider></v-divider>

        <v-stepper-step step="3">Confirmation</v-stepper-step>
      </v-stepper-header>

      <v-stepper-items>
        <v-stepper-content step="1">

          <div>
            <v-file-input accept="video/*" @change="handleFile"> </v-file-input>
          </div>

        </v-stepper-content>

        <v-stepper-content step="2">

          <div>
            <v-text-field label="Video Name" v-model="videoName"></v-text-field>
            <v-btn @click="saveVideo">Save Video</v-btn>
          </div>

        </v-stepper-content>

        <v-stepper-content step="3">
          <div>
            Success 
          </div>
        </v-stepper-content>
      </v-stepper-items>
    </v-stepper>

  </div>
</template>

<script>

import Axios from 'axios'
  import { mapState, mapActions, mapMutations } from 'vuex';

  export default {
    data: () => ({
      videoName: "",
      step: "1"
    }),
    computed: {
      ...mapState('videos', ['videos']),
      ...mapState('videoFiles', ['uploadPromise'])
    },
    methods: {
      ...mapMutations('videoFiles', {
        resetVideoFiles: 'reset'
      }),
      ...mapActions('videos', ['createVideo']),
      ...mapActions('videoFiles', ['startVideoUpload']),
       async handleFile(file){
        if(!file) return;
        
        const form = new FormData();
        form.append("video", file)
        this.startVideoUpload({form});
        this.step++;
        
      },
      async saveVideo() {
        if (!this.uploadPromise) {
          console.log("uploadPromise is null")
          return;
        }

        const videoFile = await this.uploadPromise;
        await this.createVideo({ video: { name: this.videoName, videoFile } });
        this.videoName = "";
        this.step++;
        this.resetVideoFiles();
      }
    }
}
</script>
