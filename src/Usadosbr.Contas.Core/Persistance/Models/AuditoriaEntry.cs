using System;
using System.Collections.Generic;
using System.Text.Json;
using Usadosbr.Contas.Core.Entities;

namespace Usadosbr.Contas.Core.Persistance.Models
{
    public class AuditoriaEntry
    {
        public Guid Id { get; set; }
        public DateTimeOffset DataOperacao { get; set; }
        public string Operacao { get; set; } = null!;
        public string Entidade { get; set; } = null!;
        public string Tabela { get; set; } = null!;
        public Dictionary<string, object> Chaves { get; set; } = new();
        public Dictionary<string, object> ValoresOriginais { get; set; } = new();
        public Dictionary<string, object> NovosValores { get; set; } = new();


        public Auditoria ToAuditoria()
        {
            return new Auditoria
            {
                DataOperacao = DataOperacao,
                Operacao = Operacao,
                Entidade = Entidade,
                Tabela = Tabela,
                Chaves = JsonSerializer.Serialize(Chaves),
                ValoresOriginais = JsonSerializer.Serialize(ValoresOriginais),
                ValoresNovos = JsonSerializer.Serialize(NovosValores),
            };
        }
    }
}