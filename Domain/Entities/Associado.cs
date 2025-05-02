using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Associado
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Telefone { get; set; }
        public int CarroId { get; set; }
        public Carro Carro { get; set; }
        public int EnderecoId { get; set; }
        public Endereco Endereco { get; set; }
    }
}