using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mygev.Models
{
    public class EventoConteudo
    {
        public EventoConteudo()
        {}

        /// <summary>
        /// ID do Conteudo de um Evento
        /// </summary>
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// Usado para adicionar conteudos adicionais ao evento
        /// </summary>
        public string Conteudo { get; set; }

        /// <summary>
        /// Comentarios extras ao conteudo adicionado
        /// </summary>
        public string Comentario { get; set; }

        /// <summary>
        /// chave estrangeira para o Evento
        /// </summary>
        [ForeignKey(nameof(Evento))]
        public int IDEvento { get; set; }
        public Evento Evento { get; set; }

        /// <summary>
        /// chave estrangeira para os Utilizadores
        /// </summary>
        [ForeignKey(nameof(Utilizador))]
        public int IDUser { get; set; }
        public Utilizadores Utilizador { get; set; }
    }
}
