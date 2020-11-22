<template>
  <v-menu top
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
      <v-list-item v-for="(item, index) in items"
                   :key="index" @click="changeVideoQuality(index)">
        <v-list-item-title>
          {{index}} - {{ item.title }}

        </v-list-item-title>
      </v-list-item>
    </v-list>
  </v-menu>

</template>

<script>
  import { mapState, mapMutations } from 'vuex';
  export default {
    name: "select-quality",
    data: () => ({
      index: 0,
      items: [
        { id: 1, title: "240", videolink: "" },
        { id: 2, title: "360", videolink: "" },
        { id: 3, title: "480", videolink: "" },
        { id: 4, title: "720", videolink: "" },
        { id: 5, title: "1080", videolink: "" },
      ],
      offset: true,
    }),
    methods: {
      ...mapMutations('videoWatch', ['setQuality']),
      changeVideoQuality(index) {
        const quality = this.items[index].title;
        this.setQuality({ quality });
      }
    },
    computed: {
      ...mapState('submissions', ['submissions']),
    }
  }
</script>

<style scoped>

</style>
