<template>
  <v-dialog :value="active" persistent>
    <template v-slot:activator="{on}">
      <v-btn depressed @click="toggleActivity">
        Upload
      </v-btn>
    </template>
    <v-stepper v-model="step">
      <v-stepper-header>
        <v-stepper-step :complete="step > 1" step="1">Select Type</v-stepper-step>

        <v-divider v-if="type === uploadType.VIDEO"></v-divider>

        <v-stepper-step v-if="type === uploadType.VIDEO" :complete="step > 2" step="2">Video Information</v-stepper-step>

        <v-divider></v-divider>

        <v-stepper-step :complete="step > 3" step="3">Upload Video</v-stepper-step>

        <v-divider></v-divider>

        <v-stepper-step :complete="step > 4" step="4">Submission Information</v-stepper-step>

        <v-divider></v-divider>

        <v-stepper-step step="5">Review</v-stepper-step>
      </v-stepper-header>

      <v-stepper-items>
        <v-stepper-content step="1">

          <div class="d-flex flex-column align-center">
            <v-btn class="my-2" @click="setType({type: uploadType.VIDEO})">VIDEO</v-btn>
            <v-btn class="my-2" @click="setType({type: uploadType.SUBMISSION})">SUBMISSION</v-btn>
          </div>

        </v-stepper-content>

        <v-stepper-content step="2">

          <div>
            <v-text-field label="Video Name" v-model="videoName"></v-text-field>
            <v-btn @click="incStep">Save Video</v-btn>
            
          </div>

        </v-stepper-content>

        <v-stepper-content step="3">
          <div>
            <v-file-input accept="video/*" @change="handleFile"> </v-file-input>
          </div>
        </v-stepper-content>


        <v-stepper-content step="4">
          <div>
            <v-text-field label="Description" v-model="submission"></v-text-field>
            <v-btn @click="incStep">Save Submisson</v-btn>
          </div>
        </v-stepper-content>

        <v-stepper-content step="5">
          <div>
            <v-btn @click="save">Save</v-btn>
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
      submission: ""
    }),
    computed: {
      ...mapState('videoFiles', ['uploadPromise', 'active', 'step', 'type']),
      uploadType() {
        return UPLOAD_TYPE;
      }
    },
    methods: {
      ...mapMutations('videoFiles', ['reset', 'incStep' ,'toggleActivity', 'setType']),
      ...mapActions('videoFiles', ['startVideoUpload', 'createVideo']),
      async handleFile(file) {
        if (!file) return;

        const form = new FormData();
        form.append("video", file)
        this.startVideoUpload({ form });

      },
      async save() {
        if (!this.uploadPromise) {
          console.log("uploadPromise is null")
          return;
        }

        const videoFile = await this.uploadPromise;
        await this.createVideo({ video: { name: this.videoName }, submission: { description: this.submission, videoFile, videoId: 1 } });
        this.videoName = ""
        this.submission = ""
        this.reset();
      },
    }
  }
</script>

<style scoped>

</style>
