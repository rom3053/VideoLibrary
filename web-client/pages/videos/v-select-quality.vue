<template>
  <div class="v-select-quality">
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
        <v-list-item v-for="(item, index) in qualityItems"
                     :key="index" @click="changeVideoQuality(index)">
          <v-list-item-title>
            {{index}} - {{ item.qualityName }}

          </v-list-item-title>
        </v-list-item>
      </v-list>
    </v-menu>
  </div>
</template>

<script>
  import { mapState, mapMutations } from 'vuex';
  export default {
    name: "v-select-quality",
    props: {
      qualityItems: {
        type: Object,
        default() {
          return {}
        }
      }
    },
    data: () => ({
      index: 0,
      offset: true,
    }),
    methods: {
      changeVideoQuality(index) {
        this.$emit('sendVideoQualityIndex', index)
        
      }
    },
    computed: {
      ...mapState('submissions', ['submissions']),
    }
  }
</script>

<style  scoped>

</style>
