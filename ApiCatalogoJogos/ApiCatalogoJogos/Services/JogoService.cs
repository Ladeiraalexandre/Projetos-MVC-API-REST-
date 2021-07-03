using ApiCatalogoJogos.Entities;
using ApiCatalogoJogos.Exceptions;
using ApiCatalogoJogos.InputModel;
using ApiCatalogoJogos.Repositories;
using ApiCatalogoJogos.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.Services
{
    public class JogoService : IJogoService
    {
        private readonly IJogoRepository _jogoRepository;

        public JogoService(IJogoRepository jogoRepository)
        {
            _jogoRepository = jogoRepository;
        }

        public async Task<List<JogoViewModel>> Obter(int pagina, int quantidade)
        {
            var jogos = await _jogoRepository.Obter(pagina, quantidade);

            //Criando um jogo com link, onde esta dizendo que:
            //para cada jogo crie um jogo viewmodel e crie uma lista com o ToList. 
            return jogos.Select(jogo => new JogoViewModel  //é possivel fazer isso com AutoMaper
            {
                Id = jogo.Id,
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Preco = jogo.Preco
            }).ToList();
        }

        public async Task<JogoViewModel> ObterPorId(Guid id)
        {
            var jogo = await _jogoRepository.ObterPorId(id);

            if (jogo == null)
                return null;

            return new JogoViewModel
            {
                Id = jogo.Id,
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Preco = jogo.Preco
            };
        }

        public async Task<JogoViewModel> Inserir(JogoInputModel jogo)
        {
            //ao tentar inserir um jogo, primeiramente é preciso saber se esse jogo ja existe
            var entidadeJogo = await _jogoRepository.Obter(jogo.Nome, jogo.Produtora);

            //se existir o jogo retorna a exxception
            if (entidadeJogo.Count > 0)
                throw new JogoJaCadastradoException();

            //se não existir, insere o novo jogo
            var jogoInsert = new Jogo
            {
                //gera o Id para inserir o Jogo
                Id = Guid.NewGuid(),
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Preco = jogo.Preco
            };

            await _jogoRepository.Inserir(jogoInsert);

            //após inserir, retorna pela viewmodel o jogo inserido e com seu repsctivo Id
            return new JogoViewModel
            {
                Id = jogoInsert.Id,
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Preco = jogo.Preco
            };
        }

        public async Task Atualizar(Guid id, JogoInputModel jogo)
        {
            //verifica se o jogo existe para poder fazer a atualização
            var entidadeJogo = await _jogoRepository.ObterPorId(id);

            if (entidadeJogo == null)
                throw new JogoNaoCadastradoException();

            entidadeJogo.Nome = jogo.Nome;
            entidadeJogo.Produtora = jogo.Produtora;
            entidadeJogo.Preco = jogo.Preco;

            await _jogoRepository.Atualizar(entidadeJogo);
        }

        public async Task Atualizar(Guid id, double preco)
        {
            //verifica se o jogo existe para poder fazer a atualização
            var entidadeJogo = await _jogoRepository.ObterPorId(id);

            if (entidadeJogo == null)
                throw new JogoNaoCadastradoException();

            entidadeJogo.Preco = preco;

            await _jogoRepository.Atualizar(entidadeJogo);
        }

        public async Task Remover(Guid id)
        {
            var jogo = await _jogoRepository.ObterPorId(id);

            if (jogo == null)
                throw new JogoNaoCadastradoException();

            await _jogoRepository.Remover(id);
        }

        public void Dispose() //quando destruir fechar as conexões com o banco.
        {
            _jogoRepository?.Dispose();
        }


    }
}
