<template>
  <div>
    <div class="d-flex justify-center">
      Video: {{$route.params.videoSubmission }}
      <video-change-quality></video-change-quality>
    </div>
    <div class="d-flex justify-center">
      <v-btn @click="changeVideo">CHANGE VIDEO</v-btn>
      <component is="VideoChangeQuality"
                 :qualityItems="qualityItemsData"
                 @sendVideoQualityIndex="changeVideoQualityLink">
      </component>

      <!--<v-menu top
          :offset-y="offset">
    <template v-slot:activator="{ on, attrs }">
      <v-btn color="amber darken-2"
             dark
             v-bind="attrs"
             v-on="on">
        Change quality
      </v-btn>
    </template>

    <v-list>
      <v-list-item  v-for="(item, index) in items"
                   :key="index" @click="changeVideoQuality(index)">
        <v-list-item-title>


        </v-list-item-title>
      </v-list-item>
    </v-list>
  </v-menu>-->
    </div>
    <div v-if="submissions">
      <div class="text-center" v-bind:key="s" v-for="s in submissions">
        {{s.id}} - {{s.description}} - {{s.videoId}} -
        {{videolinkFirst}} --{{videolinkSecond}}
        Selected - {{videoTestLink}}
      </div>
    </div>
    <!--<div v-if="submissions">
      <div class="text-center" v-bind:key="n" v-for="n in submissions[0].videoQualities">
        {{n}}
      </div>
    </div>-->
    <div class="d-flex justify-center">
      <video width="400" :src="video" controls>
      </video>
      <!--"https://archive.org/download/BigBuckBunny_124/Content/big_buck_bunny_720p_surround.mp4
    `http://localhost:5000/api/videoFiles/${s.videoFile}`"-->
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
      videolinkSecond: "https://archive.org/download/BigBuckBunny_124/Content/big_buck_bunny_720p_surround.mp4",
      videolinkFirst: "http://localhost:5000/api/videoFiles/",
      video: "",
      videoTestLink: "",
      qualityItemsData: null,
      offset: true,
    }),
    
    methods: {
      changeVideo() {
        if (this.video == this.videolinkFirst) {
          this.video = this.videolinkSecond
        } else {
          this.video = this.videolinkFirst
        }
      },//DoTO доделать линк
      changeVideoQualityLink(index) {
        console.log(index)
        console.log(this.qualityItemsData)

        this.videoTestLink = this.qualityItemsData[index].qualityName;
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

</style>
