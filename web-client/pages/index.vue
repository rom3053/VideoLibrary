<template>
  <div class="d-flex justify-center">
    <v-container class="my-5" fluid v-if="videos">
      <v-row dense>
        <v-col v-for="vv in videos" v-bind:key="vv" cols="3" class="md-lg-auto">
          <v-card tile flat class="ma-3" :to="`/videos/${vv.id}`">
          <v-img class="elevation-6" 
          src="https://www.w3schools.com/images/w3schools_green.jpg" 
          alt="" contain
          max-height="300"
          max-width="300">
             </v-img>
            <v-card-text>
              <div>
                <p>VideoName - {{ vv.name }}</p>
                <p>UserName - {{ vv.id }}</p>
              </div>
            </v-card-text>
          </v-card>
        </v-col>
      </v-row>
    </v-container>

    <!-- <div v-if="videos">
      <div v-bind:key="v" v-for="v in videos">
        <v-btn :to="`/videos/${v.id}`"  depressed height="150" width="100" outlined>
            {{v.name}} 
        </v-btn>
      </div>
    </div> -->
  </div>
</template>

<script>
import { mapState, mapActions, mapMutations } from "vuex";

export default {
  computed: {
    ...mapState("videos", ["videos"]),
  },
  async fetch() {
    await this.$store.dispatch("videos/fetchVideos", null, { root: true });
  },
};
</script>
