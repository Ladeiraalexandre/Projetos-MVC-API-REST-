using CursoAPI.Controllers;
using CursoMVC.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Threading.Tasks;
using Xunit;
using System;
using System.Threading;
using System.Collections.Generic;

namespace CursoTest
{
    public class CategoriasControllerTest
    {
        private readonly Mock<DbSet<Categoria>> _mockSet; //vai criar um objeto de Moq
        private readonly Mock<Context> _mockContext;
        private readonly Categoria _categoria;
        

        public CategoriasControllerTest() //construtor, inicializando as variáveis
        {
            
            _mockSet = new Mock<DbSet<Categoria>>();
            _mockContext = new Mock<Context>();
            _categoria = new Categoria { Id = 1, Descricao = "Teste Categoria" };

            //informar que o mockContext se refere ao mockSet
            _mockContext.Setup(m => m.Categorias).Returns(_mockSet.Object);


            //informar o que se espera no findAsync na categoria de teste Get_Categoria
            _mockContext.Setup(m => m.Categorias.FindAsync(1))
                .ReturnsAsync(_categoria);

            _mockContext.Setup(m => m.SetModified(_categoria));

            _mockContext.Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);
        }

        [Fact]
        public async Task Get_Categoria()
        {
            var service = new CategoriasController(_mockContext.Object); //Controller da API

            await service.GetCategoria(1); //para teste posso passar qualquer Id aqui

            //testa a quantidade de execução, neste caso deve ser uma. 
            //Para verificação, na controller da API, no metodo GetCategoria, inserir
            //mais uma categoria e um novo id. Neste caso o teste vai falhar, pois o metodo deve ser executado somente uma vez
            _mockSet.Verify(m => m.FindAsync(1),
                Times.Once());
        }

        [Fact]
        public async Task Put_Categoria()
        {
            var service = new CategoriasController(_mockContext.Object);

            await service.PutCategoria(1, _categoria);

            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once());
        }

        [Fact]
        public async Task Post_Categoria()
        {
            var service = new CategoriasController(_mockContext.Object);
            await service.PostCategoria(_categoria);

            _mockSet.Verify(x => x.Add(_categoria), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once());
        }

        [Fact]
        public async Task Delete_Categoria()
        {
            var service = new CategoriasController(_mockContext.Object);
            await service.DeleteCategoria(1);

            _mockSet.Verify(m => m.FindAsync(1),
                Times.Once());
            _mockSet.Verify(x => x.Remove(_categoria), Times.Once);
            _mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once());
        }





    }
}
