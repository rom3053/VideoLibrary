<template>
  <v-dialog :value="active" persistent>
    <v-stepper v-model="step">
      <v-stepper-header>
        <v-stepper-step :complete="step > 1" step="1">Select Type</v-stepper-step>

        <v-divider></v-divider>

        <v-stepper-step :complete="step > 2" step="2">Upload Video</v-stepper-step>

        <v-divider></v-divider>

        <v-stepper-step :complete="step > 3" step="3">Video Information</v-stepper-step>

        <v-divider></v-divider>

        <v-stepper-step step="4">Review</v-stepper-step>
      </v-stepper-header>

      <v-stepper-items>
        <v-stepper-content step="1">

          <div class="d-flex flex-column align-center">
            <v-btn class="my-2" @click="setType(uploadType.VIDEO)">VIDEO</v-btn>
            <v-btn class="my-2" @click="setType(uploadType.SUBMISSION)">SUBMISSION</v-btn>
          </div>

        </v-stepper-content>

        <v-stepper-content step="2">

          <div>
            <v-file-input accept="video/*" @change="handleFile"> </v-file-input>
          </div>

        </v-stepper-content>

        <v-stepper-content step="3">

          <div>
            <v-text-field label="Video Name" v-model="videoName"></v-text-field>
            <v-btn @click="saveVideo">Save Video</v-btn>
          </div>

        </v-stepper-content>

        <v-stepper-content step="4">
          <div>
            Success
          </div>
        </v-stepper-content>
      </v-stepper-items>
    </v-stepper>
    <div class="d-flex justify-center my-4">
      <v-btn @click="toggleActivity">
        Close
      </v-btn>
    </div>

  </v-dialog>
</template>

<script>
  import { UPLOAD_TYPE } from '../data/enum.js'
  import { mapState, mapActions, mapMutations } from 'vuex';
  export default {
    name: "video-upload",
    data: () => ({
      videoName: "",
    }),
    computed: {
      ...mapState('videoFiles', ['uploadPromise', 'active', 'step']),
      uploadType() {
        return {
          ...UPLOAD_TYPE
        }
      }
    },
    methods: {
      ...mapMutations('videoFiles', ['reset', 'toggleActivity', 'setType']),
      ...mapActions('videoFiles', ['startVideoUpload', 'createVideo']),
      async handleFile(file) {
        if (!file) return;

        const form = new FormData();
        form.append("video", file)
        this.startVideoUpload({ form });

      },
      async saveVideo() {
        if (!this.uploadPromise) {
          console.log("uploadPromise is null")
          return;
        }

        const videoFile = await this.uploadPromise;
        await this.createVideo({ video: { name: this.videoName, videoFile } });
        this.videoName = "";
        this.reset();
      },
    }
  }
</script>

<style scoped>

</style>
