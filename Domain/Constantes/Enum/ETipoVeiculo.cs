using System.ComponentModel;

namespace Domain.Constantes.Enum
{
    public enum ETipoVeiculo : short
    {
        [Description("Não definido")]
        NaoDefinido = 0,

        [Description("Carro")]
        Carro = 1,

        [Description("Moto")]
        Moto = 2,

        [Description("Caminhão")]
        Caminhao = 3
    }
}