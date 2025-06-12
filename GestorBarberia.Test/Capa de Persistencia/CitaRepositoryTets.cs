using Moq;
using GestorBarberia.Domain.Entities;
using GestorBarberia.Infrastructure.Exceptions;
using GestorBarberia.Infrastructure.Models;
using GestorBarberia.Persistence.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using GestorBarberia.Persistence.Context;
using GestorBarberia.Persistence.Interface;
using Microsoft.EntityFrameworkCore;

namespace GestorBarberia.Test.Capa_de_Persistencia
{
    public class CitaRepositoryTest
    {
        private readonly Mock<DbContextBarberia> _mockDbContext;
        private readonly Mock<ILogger<CitaRepository>> _mockLogger;
        private readonly Mock<IBarberiasRepository> _mockBarberiasRepository;
        private readonly CitaRepository _citaRepository;

        public CitaRepositoryTest()
        {
            _mockDbContext = new Mock<DbContextBarberia>();
            _mockLogger = new Mock<ILogger<CitaRepository>>();
            _mockBarberiasRepository = new Mock<IBarberiasRepository>();
            _citaRepository = new CitaRepository(_mockDbContext.Object, _mockLogger.Object, _mockBarberiasRepository.Object);
        }

        [Fact]
        public void GetCitaById_Returns_CitaModel_When_CitaExists()
        {
            // Arrange
            var citaId = 1;
            var mockCitas = new List<Citas>
            {
                new Citas
                {
                    CitaId = citaId,
                    BarberoId = 1,
                    EstiloId = 1,
                    ClienteId = 1,
                    Deleted = false
                }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Citas>>();
            mockDbSet.As<IQueryable<Citas>>().Setup(m => m.Provider).Returns(mockCitas.Provider);
            mockDbSet.As<IQueryable<Citas>>().Setup(m => m.Expression).Returns(mockCitas.Expression);
            mockDbSet.As<IQueryable<Citas>>().Setup(m => m.ElementType).Returns(mockCitas.ElementType);
            mockDbSet.As<IQueryable<Citas>>().Setup(m => m.GetEnumerator()).Returns(mockCitas.GetEnumerator());

            _mockDbContext.Setup(db => db.Citas).Returns(mockDbSet.Object);

            // Act
            var result = _citaRepository.GetCitaById(citaId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(citaId, result.CitaId);
        }

        [Fact]
        public void GetCitaById_ThrowsException_When_CitaDoesNotExist()
        {
            // Arrange
            var citaId = 999; // Un ID que no existe en los datos de prueba
            var mockDbSet = new Mock<DbSet<Citas>>();
            _mockDbContext.Setup(db => db.Citas).Returns(mockDbSet.Object);

            // Act & Assert
            var exception = Assert.Throws<CitaExceptions>(() => _citaRepository.GetCitaById(citaId));
            Assert.Equal("Ha ocurrido un error obteniendo la Cita", exception.Message);
        }

        [Fact]
        public void VerifyCita_ReturnsTrue_When_CitaExistsInRange()
        {
            // Arrange
            var fecha = DateTime.Now.Date;
            var hora = TimeSpan.FromHours(10);

            var mockCitas = new List<Citas>
            {
                new Citas
                {
                    CitaId = 1,
                    Fecha = fecha,
                    Hora = hora,
                    Estado = "Realizada",
                    Deleted = false
                }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Citas>>();
            mockDbSet.As<IQueryable<Citas>>().Setup(m => m.Provider).Returns(mockCitas.Provider);
            mockDbSet.As<IQueryable<Citas>>().Setup(m => m.Expression).Returns(mockCitas.Expression);
            mockDbSet.As<IQueryable<Citas>>().Setup(m => m.ElementType).Returns(mockCitas.ElementType);
            mockDbSet.As<IQueryable<Citas>>().Setup(m => m.GetEnumerator()).Returns(mockCitas.GetEnumerator());

            _mockDbContext.Setup(db => db.Citas).Returns(mockDbSet.Object);

            // Act
            var result = _citaRepository.VerifyCita(fecha, hora);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void VerifyCita_ReturnsFalse_When_CitaDoesNotExistInRange()
        {
            // Arrange
            var fecha = DateTime.Now.Date;
            var hora = TimeSpan.FromHours(10);

            var mockCitas = new List<Citas>().AsQueryable();

            var mockDbSet = new Mock<DbSet<Citas>>();
            mockDbSet.As<IQueryable<Citas>>().Setup(m => m.Provider).Returns(mockCitas.Provider);
            mockDbSet.As<IQueryable<Citas>>().Setup(m => m.Expression).Returns(mockCitas.Expression);
            mockDbSet.As<IQueryable<Citas>>().Setup(m => m.ElementType).Returns(mockCitas.ElementType);
            mockDbSet.As<IQueryable<Citas>>().Setup(m => m.GetEnumerator()).Returns(mockCitas.GetEnumerator());

            _mockDbContext.Setup(db => db.Citas).Returns(mockDbSet.Object);

            // Act
            var result = _citaRepository.VerifyCita(fecha, hora);

            // Assert
            Assert.False(result);
        }
    }
}
