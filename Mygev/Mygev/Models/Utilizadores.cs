﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mygev.Models {
    public class Utilizadores {

        public Utilizadores() {
            ListaEventos = new HashSet<EventoUtilizadores>();
        }

        /// <summary>
        /// ID de um ultilizador 
        /// </summary>
        [Key]
        public int ID { get; set; }


        /// <summary>
        /// Nome que o utilizador usará na aplicaçao (username / Nick)
        /// </summary>
        [Required]
        public string NomeUser { get; set; }

        /// <summary>
        /// Email do utilizador
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// Password do Utilizador
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// Lista dos eventos em que o utilizador participa
        /// </summary>
        public ICollection<EventoUtilizadores> ListaEventos { get; set; }

    }

}

