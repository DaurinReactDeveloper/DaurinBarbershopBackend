using GestorBarberia.Application.Contract;
using GestorBarberia.Application.Core;
using GestorBarberia.Application.Dtos.BarberoDto;
using GestorBarberia.Application.Services;
using GestorBarberia.Application.Validations;
using GestorBarberia.Domain.Entities;
using GestorBarberia.Infrastructure.Exceptions;
using GestorBarberia.Infrastructure.Models;
using GestorBarberia.Persistence.Context;
using GestorBarberia.Persistence.Interface;
using GestorBarberia.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using Xunit;

namespace GestorBarberia.Test.Capa_de_Persistencia
{
    public class BarberoRepositoryTests
    {
        private readonly Mock<DbContextBarberia> _mockDbContext;
        private readonly Mock<IAdministradorRepository> _mockAdministradorRepository;
        private readonly Mock<IBarberiasRepository> _mockBarberiasRepository;
        private readonly BarberoRepository _repository;
        private readonly Mock<ILogger<BarberoRepository>> _mockLogger;

        public BarberoRepositoryTests()
        {
            _mockDbContext = new Mock<DbContextBarberia>();
            _mockLogger = new Mock<ILogger<BarberoRepository>>();
            _mockAdministradorRepository = new Mock<IAdministradorRepository>();
            _mockBarberiasRepository = new Mock<IBarberiasRepository>();

            _repository = new BarberoRepository(
                _mockDbContext.Object,
                _mockLogger.Object,
                _mockAdministradorRepository.Object,
                _mockBarberiasRepository.Object
            );
        }

        private Mock<DbSet<T>> CreateMockDbSet<T>(IQueryable<T> data) where T : class
        {
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mockSet;
        }

        [Fact]
        public void GetBarberoName_BarberoExistente_RetornaBarberoModel()
        {
            // Arrange
            var nombre = "barbero1";
            var barberos = new List<Barberos>
            {
                new Barberos { BarberoId = 1, Nombre = nombre, Password = "password", Deleted = false }
            }.AsQueryable();

            var mockSet = CreateMockDbSet(barberos);
            _mockDbContext.Setup(c => c.Barberos).Returns(mockSet.Object);

            // Act
            var result = _repository.GetBarberoName(nombre);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(nombre, result.Nombre);
            Assert.Equal("password", result.Password);
        }

        [Fact]
        public void VerifyNameBarbero_ShouldReturnTrue_WhenNameExists()
        {
            // Arrange
            var nombre = "John";
            var barberos = new List<Barberos>
            {
                new Barberos { Nombre = nombre, Deleted = false }
            }.AsQueryable();

            var mockSet = CreateMockDbSet(barberos);
            _mockDbContext.Setup(c => c.Barberos).Returns(mockSet.Object);

            // Act
            var result = _repository.VerifyNameBarbero(nombre);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void VerifyNameBarbero_ShouldReturnFalse_WhenNameDoesNotExist()
        {
            // Arrange
            var nombre = "John";
            var barberos = new List<Barberos>
            {
                new Barberos { Nombre = "Doe", Deleted = false }
            }.AsQueryable();

            var mockSet = CreateMockDbSet(barberos);
            _mockDbContext.Setup(c => c.Barberos).Returns(mockSet.Object);

            // Act
            var result = _repository.VerifyNameBarbero(nombre);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetBarberos_ShouldReturnListOfBarberos_WhenBarberosExist()
        {
            // Arrange
            var barberos = new List<Barberos>
            {
                new Barberos { BarberoId = 1, Nombre = "Barbero1", Email = "barbero1@example.com", Deleted = false },
                new Barberos { BarberoId = 2, Nombre = "Barbero2", Email = "barbero2@example.com", Deleted = false }
            }.AsQueryable();

            var mockSet = CreateMockDbSet(barberos);
            _mockDbContext.Setup(c => c.Barberos).Returns(mockSet.Object);

            // Act
            var result = _repository.GetBarberos();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Barbero1", result[0].Nombre);
            Assert.Equal("Barbero2", result[1].Nombre);
        }

        [Fact]
        public void GetBarberosByBarberiaId_ShouldReturnBarberos_WhenBarberiaIdExists()
        {
            // Arrange
            int barberiaId = 1;
            var barberos = new List<Barberos>
            {
                new Barberos { BarberoId = 1, Nombre = "Barbero1", BarberiaId = barberiaId, Deleted = false },
                new Barberos { BarberoId = 2, Nombre = "Barbero2", BarberiaId = barberiaId, Deleted = false }
            }.AsQueryable();

            var mockSet = CreateMockDbSet(barberos);
            _mockDbContext.Setup(c => c.Barberos).Returns(mockSet.Object);

            // Act
            var result = _repository.GetBarberosByBarberiaId(barberiaId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, r => Assert.Equal(barberiaId, r.BarberiaId));
        }

    }
}
