using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mygev.Models {
    public class EventoUtilizadores {

        public EventoUtilizadores()
        {}

        [Key]
        public int ID { get; set; }

        /// <summary>
        /// chave estrangeira para o Utilizador
        /// </summary>
        [ForeignKey(nameof(Utilizador))]
        public int IDUser { get; set; }
        public Utilizadores Utilizador { get; set; }

        /// <summary>
        /// chave estrangeira para o Evento
        /// </summary>
        [ForeignKey(nameof(Evento))]
        public int IDEvento { get; set; }
        public Evento Evento { get; set; }

        /// <summary>
        /// Diz se o utilizador tem permições para alterar o evento (Admistrador Colaborador Participante)
        /// </summary>
        public string Permissao { get; set; }
    }
}
