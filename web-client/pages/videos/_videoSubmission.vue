<template>
  <div>
    <div class="d-flex justify-center">
      Video: {{$route.params.videoSubmission }}
    </div>
    <div class="d-flex justify-center">
      <v-btn @click="changeVideo">CHANGE VIDEO</v-btn>
      <div>
        <component is="videoChangeQuality"></component>
      </div>
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
        Selected - {{quality}}

      </div>
    </div>
    <div v-if="submissions">
      <div class="text-center" v-bind:key="n" v-for="n in submissions[0].videoQualities">
        {{n}}
      </div>
    </div>
    <div class="d-flex justify-center">
      <video width="400" :src="video" controls>
      </video>
      <!--"https://archive.org/download/BigBuckBunny_124/Content/big_buck_bunny_720p_surround.mp4
    `http://localhost:5000/api/videoFiles/${s.videoFile}`"-->
    </div>


  </div>
    
</template>

<script>
  import { mapState, mapMutations } from 'vuex';
  import videoChangeQuality from "./sselect-quality";

  export default {

    data: () => ({
      videolinkSecond: "https://archive.org/download/BigBuckBunny_124/Content/big_buck_bunny_720p_surround.mp4",
      videolinkFirst: "http://localhost:5000/api/videoFiles/",
      video: "",
      items: [
        { id: 1, title: "240", videolink: "" },
        { id: 2, title: "360", videolink: "" },
        { id: 3, title: "480", videolink: "" },
        { id: 4, title: "720", videolink: "" },
        { id: 5, title: "1080", videolink: "" },
      ],
      itemV: [],
      offset: true,
    }),
    components: { videoChangeQuality },
    methods: {
      changeVideo() {
        if (this.video == this.videolinkFirst) {
          this.video = this.videolinkSecond
        } else {
          this.video = this.videolinkFirst
        }
      },
      //changeVideoQuality(index) {
      //  this.quality = this.items[index].title;
      //}
    },
    computed: {
      ...mapState('submissions', ['submissions']),
      ...mapState('videoWatch', ['quality'])
    },
    async fetch(){
        const videoId = this.$route.params.videoSubmission;
      await this.$store.dispatch("submissions/fetchSubmissions", { videoId }, { root: true })
      this.itemV = this.submissions[0].videoQualities
      this.videolinkFirst += this.submissions[0].videoFile
      this.video = this.videolinkFirst
    }
}
</script>

<style scoped>

</style>
