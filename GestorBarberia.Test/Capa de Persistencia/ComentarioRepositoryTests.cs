using GestorBarberia.Domain.Entities;
using GestorBarberia.Persistence.Repositories;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using GestorBarberia.Persistence.Context;

namespace GestorBarberia.Test.Capa_de_Persistencia
{
    public class ComentarioRepositoryTests
    {
        // Método para crear un Mock de DbSet<Comentarios>
        private Mock<DbSet<Comentarios>> CreateMockDbSet(List<Comentarios> comentarios)
        {
            var queryable = comentarios.AsQueryable();

            var mockDbSet = new Mock<DbSet<Comentarios>>();
            mockDbSet.As<IQueryable<Comentarios>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockDbSet.As<IQueryable<Comentarios>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockDbSet.As<IQueryable<Comentarios>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockDbSet.As<IQueryable<Comentarios>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            return mockDbSet;
        }

        // Método para crear un Mock de DbContextBarberia
        private Mock<DbContextBarberia> CreateMockDbContext(Mock<DbSet<Comentarios>> mockComentariosDbSet)
        {
            var mockContext = new Mock<DbContextBarberia>();
            mockContext.Setup(c => c.Comentarios).Returns(mockComentariosDbSet.Object);
            return mockContext;
        }

        // Método para crear un Mock de ILogger<ComentarioRepository>
        private Mock<ILogger<Comentarios>> CreateLoggerMock()
        {
            return new Mock<ILogger<Comentarios>>();
        }


        // Prueba para GetComentarios
        [Fact]
        public void GetComentarios_ReturnsListOfComentarioModel()
        {
            // Arrange
            var comentarios = new List<Comentarios>
            {
                new Comentarios
                {
                    IdComentarios = 1,
                    IdCita = 100,
                    IdCliente = 1,
                    IdCorte = 10,
                    IdBarbero = 1,
                    Calificacion = 5,
                    Comentario = "Excelente servicio",
                    Deleted = false
                }
            };

            var mockDbSet = CreateMockDbSet(comentarios);
            var mockContext = CreateMockDbContext(mockDbSet);
            var loggerMock = CreateLoggerMock();
            var repository = new ComentarioRepository(mockContext.Object, loggerMock.Object);

            // Act
            var result = repository.GetComentarios();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Excelente servicio", result.First().Comentario);
        }

        // Prueba para GetComentsByBarberoId
        [Fact]
        public void GetComentsByBarberoId_ReturnsCommentsForBarbero()
        {
            // Arrange
            var comentarios = new List<Comentarios>
            {
                new Comentarios
                {
                    IdComentarios = 1,
                    IdCita = 100,
                    IdCliente = 1,
                    IdCorte = 10,
                    IdBarbero = 1,
                    Calificacion = 5,
                    Comentario = "Buen corte",
                    Deleted = false
                },

                new Comentarios
                {
                    IdComentarios = 2,
                    IdCita = 101,
                    IdCliente = 2,
                    IdCorte = 11,
                    IdBarbero = 1,
                    Calificacion = 4,
                    Comentario = "Corte bueno",
                    Deleted = false
                }
            };

            var mockDbSet = CreateMockDbSet(comentarios);
            var mockContext = CreateMockDbContext(mockDbSet);
            var loggerMock = CreateLoggerMock();
            var repository = new ComentarioRepository(mockContext.Object, loggerMock.Object);

            // Act
            var result = repository.GetComentsByBarberoId(1);

            // Assert
            Assert.NotNull(result);
        }

    }
}
