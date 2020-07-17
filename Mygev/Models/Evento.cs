using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [StringLength(50, ErrorMessage = "O {0} do Evento não pode ter mais de {1} carateres.")]
        [Required(ErrorMessage = "O {0} do Evento é de preenchimento obrigatório.")]
        public string Nome { get; set; }

        /// <summary>
        /// imagem  ilustrativa de um evento
        /// </summary>
        public string Logo { get; set; }

        /// <summary>
        /// local do evento
        /// </summary>
        [Required(ErrorMessage = "Tem de ser definido um {0}.")]
        [StringLength(30, ErrorMessage = "O {0} não pode ter mais de {1} carateres.")]
        public string Local { get; set; }

        /// <summary>
        /// data inicial(dia e hora) do evento
        /// </summary>
        [Required(ErrorMessage = "A Data de Inicio do evento é de preenchimento obrigatório")]
        public DateTime DataInicio { get; set; }

        /// <summary>
        /// data final(dia e hora) do evento
        /// </summary>
        //[DataType(DataType.]
        public DateTime DataFim { get; set; }

        /// <summary>
        /// descrição do evento
        /// </summary>
        [Required(ErrorMessage = "A {0} do evento é de preenchimento obrigatório")]
        [StringLength(255, ErrorMessage = "A {0} não pode ter mais de {1} carateres.")]
        public string Descricao { get; set; }

        /// <summary>
        /// Estado de um evento (presente, passado, futuro)
        /// </summary>
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        public string Estado { get; set; }

        /// <summary>
        /// se o evento é publico ou privado
        /// </summary>
        public bool Publico { get; set; }

        ///summary
        ///Password para quando o evento é privado, deveria ser encriptado
        ///</summary>
        public string passEvento { get; set; }
        

        /// <summary>
        /// lista dos conteudos do Evento 
        /// </summary>
        public ICollection<EventoConteudo> ListaConteudos { get; set; }

        /// <summary>
        /// lista dos Utilizadores do Evento 
        /// </summary>
        public ICollection<EventoUtilizadores> ListaUtilizadores { get; set; }
    }
}

