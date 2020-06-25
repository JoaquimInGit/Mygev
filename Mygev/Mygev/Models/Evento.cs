using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mygev.Models {
    public class Evento {

        public Evento() {
            ListaConteudos = new HashSet<EventoConteudo>();
            ListaUtilizadores = new HashSet<EventoUtilizadores>();
        }

        /// <summary>
        /// ID de um evento 
        /// </summary>
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// Nome do evento
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// imagem  ilustrativa de um evento
        /// </summary>
        public string Logo { get; set; }

        /// <summary>
        /// data do evento
        /// </summary>
        public DateTime Data { get; set; }

        /// <summary>
        /// hora em que decorrerá o evento
        /// </summary>
        //[DataType(DataType.]
        public DateTime Hora { get; set; }

        /// <summary>
        /// descrição do evento
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Estado de um evento (presente, passado, futuro)
        /// </summary>
        public string Estado { get; set; }

        /// <summary>
        /// se o evento é publico ou privado
        /// </summary>
        public bool Publico { get; set; }

        /// <summary>
        /// lista dos conteudos do Evento 
        /// </summary>
        public ICollection<EventoConteudo> ListaConteudos { get; set; }
        public ICollection<EventoUtilizadores> ListaUtilizadores { get; set; }
    }
}

