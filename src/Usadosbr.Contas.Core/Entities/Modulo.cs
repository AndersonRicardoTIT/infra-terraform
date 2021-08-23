namespace Usadosbr.Contas.Core.Entities
{
    public class Modulo : EntityGuid
    {
        public string Descricao { get; private set; }
        
        public Modulo(string descricao)
        {
            Descricao = descricao;
        }

        public void Atualizar(string descricao)
        {
            Descricao = descricao;
        }
    }
}