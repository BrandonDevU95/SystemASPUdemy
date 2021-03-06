<template>
  <v-layout align-start>
    <v-flex>
      <v-data-table
        :headers="headers"
        :items="categorias"
        :search="search"
        class="elevation-1"
      >
        <template v-slot:top>
          <v-toolbar flat color="white">
            <v-toolbar-title>Categorias</v-toolbar-title>
            <v-divider class="mx-4" inset vertical></v-divider>
            <v-spacer></v-spacer>
            <v-text-field
              class="text-md-center"
              v-model="search"
              append-icon="search"
              label="Busqueda"
              single-line
              hide-details
            ></v-text-field>
            <v-spacer></v-spacer>

            <v-dialog v-model="dialog" max-width="500px">
              <template v-slot:activator="{ on }">
                <v-btn color="primary" dark class="mb-2" v-on="on">Nuevo</v-btn>
              </template>

              <v-card>
                <v-card-title>
                  <span class="headline text-center">{{ formTitle }}</span>
                </v-card-title>

                <v-card-text>
                  <v-container>
                    <v-layout wrap>
                      <v-flex xs12 sm12 md12>
                        <v-text-field v-model="name" label="Nombre"></v-text-field>
                      </v-flex>
                      <v-flex xs12 sm12 md12>
                        <v-text-field v-model="description" label="Descripcion"></v-text-field>
                      </v-flex>
                      <v-flex xs12 sm12 md12 v-show="validate">
                        <div class ="red--text" v-for="v in validateMessage" :key ="v" v-text="v"></div>
                      </v-flex>
                    </v-layout>
                    </v-row>
                  </v-container>
                </v-card-text>

                <v-card-actions>
                  <v-spacer></v-spacer>
                  <v-btn color="blue darken-1" text @click="close">Cancelar</v-btn>
                  <v-btn color="blue darken-1" text @click="save">Guardar</v-btn>
                </v-card-actions>
              </v-card>
            </v-dialog>
          </v-toolbar>
        </template>

        <template v-slot:item.status="{ item }">
          <v-card-text v-if="item.status" class="blue--text">Activo</v-card-text>
          <v-card-text v-if="!item.status" class="red--text">Inactivo</v-card-text>
        </template>

        <template v-slot:item.options="{ item }">
          <v-icon small class="mr-3" @click="editItem(item)">edit</v-icon>
          <v-icon small @click="deleteItem(item)">delete</v-icon>
        </template>

        <template v-slot:no-data>
          <v-btn color="primary">Resetear</v-btn>
        </template>
      </v-data-table>
    </v-flex>
  </v-layout>
</template>

<script>
import axios from "axios";
export default {
  data() {
    return {
      categorias: [],
      dialog: false,
      headers: [
        { text: "Opciones", value: "options", sortable: false },
        { text: "Nombre", value: "name" },
        { text: "Descripcion", value: "description", sortable: false },
        { text: "Estado", value: "status" }
      ],
      search: "",
      editedIndex: -1,
      editedItem: {
        name: "",
        description: ""
      },
      id: "",
      name: "",
      description: "",
      valid: 0,
      validateMessage:[]
    };
  },

  computed: {
    formTitle() {
      return this.editedIndex === -1
        ? "Nueva Categoria"
        : "Actualizar Categoria";
    }
  },

  watch: {
    dialog(val) {
      val || this.close();
    }
  },

  created() {
    this.list();
  },

  methods: {
    list() {
      let me = this;
      axios
        .get("api/Categories/List")
        .then(function(response) {
          me.categorias = response.data;
        })
        .catch(function(error) {
          console.log(error);
        });
    },

    editItem(item) {
      this.editedIndex = this.desserts.indexOf(item);
      this.editedItem = Object.assign({}, item);
      this.dialog = true;
    },

    deleteItem(item) {
      const index = this.desserts.indexOf(item);
      confirm("Are you sure you want to delete this item?") &&
        this.desserts.splice(index, 1);
    },

    close() {
      this.dialog = false;
    },

    clear() {
      this.id = "";
      this.name = "";
      this.description = "";
    },

    save() {
      if (this.validate()) {
        return;
      }  
      if (this.editedIndex > -1) {
        //Metodo para editar
      } else {        
        let me = this;
        axios
          .post("api/Categories/Create", {
            'name': me.name,
            'description': me.description
          })
          .then(function(response) {
            me.close();
            me.list();
            me.clear();
          })
          .catch(function(error) {
            console.log(error);
          });
      }
    },

    validate(){
      this.valid = 0;
      this.validateMessage = [];
      if (this.name.length < 3 || this.name.length >50) {
        this.validateMessage.push("El nombre debe tener mas de 3 caracteres y menos de 50")
      }
      if (this.validateMessage.length) {
        this.valid = 1;
      }
      return this.valid;
    }
  }
};
</script>
