<template>
  <v-stepper v-model="step">
    <v-stepper-header>
      <v-stepper-step :complete="step > 1" step="1">
        Video Information
      </v-stepper-step>

      <v-divider></v-divider>

      <v-stepper-step step="2">Review</v-stepper-step>
    </v-stepper-header>

    <v-stepper-items>
      <v-stepper-content step="1">
        <div class="d-flex flex-column align-center">
          <v-text-field
            label="Video Name"
            v-model="formVideo.videoName"
          ></v-text-field>
          <v-file-input accept="video/*" @change="handleFile"> </v-file-input>
          <v-text-field
            label="Description"
            v-model="formVideo.submission"
          ></v-text-field>
          <v-btn @click="step++">Save Video</v-btn>
        </div>
      </v-stepper-content>

      <v-stepper-content step="2">
        <div>
          <v-btn @click="save">Save</v-btn>
        </div>
      </v-stepper-content>
    </v-stepper-items>
  </v-stepper>
</template>

<script>
import { mapState, mapActions, mapMutations } from "vuex";
const initState = () => ({
  step: 1,
  formVideo: {
    videoName: "",
    submission: "",
  },
});
export default {
  name: "video-upload-steps",
  data: initState,
  computed: {
    ...mapState("videoFiles", ["uploadPromise", "active"]),
  },
  watch: {
    active: function (newValue) {
      if (!newValue) {
        Object.assign(this.$data, initState());
      }
    },
  },
  methods: {
    ...mapMutations("videoFiles", ["reset", "hide"]),
    ...mapActions("videoFiles", ["startVideoUpload", "createVideo"]),
    async handleFile(file) {
      if (!file) return;

      const form = new FormData();
      form.append("video", file);
      this.startVideoUpload({ form });
    },
    async save() {
      if (!this.uploadPromise) {
        console.log("uploadPromise is null");
        return;
      }

      const videoFile = await this.uploadPromise;
      console.log("VideoFile",videoFile);
      await this.createVideo({
        video: { name: this.formVideo.videoName },
        submission: {
          description: this.formVideo.submission,
          videoFile,
          videoId: 1,
        },
      });

      this.reset();
      this.hide();
      Object.assign(this.$data, initState());
    },
  },
};
</script>

<style scoped>
</style>
