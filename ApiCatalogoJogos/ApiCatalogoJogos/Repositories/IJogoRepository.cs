using ApiCatalogoJogos.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.Repositories
{
    public interface IJogoRepository : IDisposable
    {
        Task<List<Jogo>> Obter(int pagina, int quantidade);
        Task<Jogo> ObterPorId(Guid id);
        Task<List<Jogo>> Obter(string nome, string produtora);

        //quandeo vc retorna somente Task, é como se fosse um void... sem retorno
        Task Inserir(Jogo jogo); 
        Task Atualizar(Jogo jogo);
        Task Remover(Guid id);
    }
}
