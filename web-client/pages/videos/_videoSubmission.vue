<template>
  <div>
    <div class="d-flex justify-center">
      Video: {{$route.params.videoSubmission }}
      <!--<video-change-quality></video-change-quality>-->
    </div>
    <div class="d-flex justify-center">
      <!--<v-btn @click="changeVideo">CHANGE VIDEO</v-btn>-->
      <component is="VideoChangeQuality"
                 :qualityItems="qualityItemsData"
                 @sendVideoQualityIndex="changeVideoQualityLink">
      </component>
    </div>
    <div v-if="submissions">
      <div class="text-center" v-bind:key="s" v-for="s in submissions">
        {{s.id}} - {{s.description}} - {{s.videoId}} -
        Selected - {{videoLinkAPI}}
      </div>
    </div>
    <div class="video-player__block">
      <video class="video-player__main"  :src="video" controls>
      </video>
    </div>


  </div>
    
</template>

<script>
  import { mapState, mapMutations } from 'vuex'
  import VideoChangeQuality from './v-select-quality.vue'

  export default {
    name: "videoSubmission",
    components: {
      VideoChangeQuality,
    },
    props: {},
    data: () => ({
      //videolinkSecond: "https://archive.org/download/BigBuckBunny_124/Content/big_buck_bunny_720p_surround.mp4",
      videolinkFirst: "http://localhost:5000/api/videoFiles/",
      videoLinkAPI: "http://localhost:5000/api/videoFiles/",
      video: "",
      videoTestLink: "",
      qualityItemsData: null,
      offset: true,
    }),
    
    methods: {
      //changeVideo() {
      //  if (this.video == this.videolinkFirst) {
      //    this.video = this.videolinkSecond
      //  } else {
      //    this.video = this.videolinkFirst
      //  }
      //},//DoTO доделать линк
      changeVideoQualityLink(index) {
        console.log(index)
        console.log(this.qualityItemsData)

        this.video = this.videoLinkAPI+this.qualityItemsData[index].qualityVideoLink;
      }
    },
    computed: {
      ...mapState('submissions', ['submissions']),
      //...mapState('videoWatch', ['quality'])
    },
    async fetch(){
        const videoId = this.$route.params.videoSubmission;
      await this.$store.dispatch("submissions/fetchSubmissions", { videoId }, { root: true });
      this.qualityItemsData = this.submissions[0].videoQualities
      this.videolinkFirst += this.submissions[0].videoFile
      this.video = this.videolinkFirst
    }
}
</script>

<style scoped>
  .video-player__main {
    min-height: 250px;
    min-width: 200px;
    max-height: 380px;
    max-width: 675px;
  }

  .video-player__block {
    display: flex;
    flex-direction: column;
    padding: 15px;
  }
</style>
