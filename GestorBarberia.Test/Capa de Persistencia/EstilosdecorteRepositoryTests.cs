using Moq;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using GestorBarberia.Domain.Entities;
using GestorBarberia.Persistence.Context;
using GestorBarberia.Persistence.Interface;
using GestorBarberia.Persistence.Repositories;
using Microsoft.Extensions.Logging;
using GestorBarberia.Infrastructure.Exceptions;
using GestorBarberia.Infrastructure.Models;

namespace GestorBarberia.Test.Capa_de_Persistencia
{
    public class EstilosdecorteRepositoryTests
    {
        private readonly Mock<DbContextBarberia> _mockDbContext;
        private readonly Mock<ILogger<EstilosdecorteRepository>> _mockLogger;
        private readonly Mock<IAdministradorRepository> _mockAdministradorRepository;
        private readonly Mock<IBarberiasRepository> _mockBarberiasRepository;
        private readonly EstilosdecorteRepository _repository;

        public EstilosdecorteRepositoryTests()
        {
            _mockDbContext = new Mock<DbContextBarberia>();
            _mockLogger = new Mock<ILogger<EstilosdecorteRepository>>();
            _mockAdministradorRepository = new Mock<IAdministradorRepository>();
            _mockBarberiasRepository = new Mock<IBarberiasRepository>();
            _repository = new EstilosdecorteRepository(
                _mockDbContext.Object,
                _mockLogger.Object,
                _mockAdministradorRepository.Object,
                _mockBarberiasRepository.Object);
        }

        [Fact]
        public void GetEstilosdecorte_ShouldReturnEstilosList()
        {
            // Arrange
            var estilos = new List<Estilosdecortes>
            {
                new Estilosdecortes { EstiloId = 1, Nombre = "Estilo 1", Descripcion = "Descripcion 1", Precio = 20 },
                new Estilosdecortes { EstiloId = 2, Nombre = "Estilo 2", Descripcion = "Descripcion 2", Precio = 25 }
            };

            var mockDbSet = new Mock<DbSet<Estilosdecortes>>();
            mockDbSet.As<IQueryable<Estilosdecortes>>()
                     .Setup(m => m.Provider).Returns(estilos.AsQueryable().Provider);
            mockDbSet.As<IQueryable<Estilosdecortes>>()
                     .Setup(m => m.Expression).Returns(estilos.AsQueryable().Expression);
            mockDbSet.As<IQueryable<Estilosdecortes>>()
                     .Setup(m => m.ElementType).Returns(estilos.AsQueryable().ElementType);
            mockDbSet.As<IQueryable<Estilosdecortes>>()
                     .Setup(m => m.GetEnumerator()).Returns(estilos.GetEnumerator());

            _mockDbContext.Setup(x => x.Estilosdecortes).Returns(mockDbSet.Object);

            // Act
            var result = _repository.GetEstilosdecorte();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Estilo 1", result[0].Nombre);
        }

        [Fact]
        public void GetEstilosdecorteByBarberiaId_ShouldReturnEstilosList()
        {
            // Arrange
            int barberiaId = 1;
            var estilos = new List<Estilosdecortes>
            {
                new Estilosdecortes { EstiloId = 1, Nombre = "Estilo 1", Descripcion = "Descripcion 1", Precio = 20, BarberiaId = barberiaId, Imgestilo = "img1.jpg", Deleted = false },
                new Estilosdecortes { EstiloId = 2, Nombre = "Estilo 2", Descripcion = "Descripcion 2", Precio = 25, BarberiaId = barberiaId, Imgestilo = "img2.jpg", Deleted = false }
            };

            var mockDbSet = new Mock<DbSet<Estilosdecortes>>();
            mockDbSet.As<IQueryable<Estilosdecortes>>()
                     .Setup(m => m.Provider).Returns(estilos.AsQueryable().Provider);
            mockDbSet.As<IQueryable<Estilosdecortes>>()
                     .Setup(m => m.Expression).Returns(estilos.AsQueryable().Expression);
            mockDbSet.As<IQueryable<Estilosdecortes>>()
                     .Setup(m => m.ElementType).Returns(estilos.AsQueryable().ElementType);
            mockDbSet.As<IQueryable<Estilosdecortes>>()
                     .Setup(m => m.GetEnumerator()).Returns(estilos.GetEnumerator());

            _mockDbContext.Setup(x => x.Estilosdecortes).Returns(mockDbSet.Object);

            // Act
            var result = _repository.GetEstilosdecorteByBarberiaId(barberiaId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Estilo 1", result[0].Nombre);
            Assert.Equal("img1.jpg", result[0].Imgestilo);
        }

        [Fact]
        public void GetEstilosdecorteByBarberiaId_ShouldHandleException()
        {
            // Arrange
            int barberiaId = 1;
            _mockDbContext.Setup(x => x.Estilosdecortes).Throws(new Exception("Test Exception"));

            // Act & Assert
            var exception = Assert.Throws<EstilosdecorteExceptions>(() => _repository.GetEstilosdecorteByBarberiaId(barberiaId));
            Assert.Equal("Ha ocurrido un error obteniendo los Estilos.", exception.Message);
        }


        [Fact]
        public void VerifyPermissionsEstilos_ShouldThrowExceptionWhenAdminNotFound()
        {
            // Arrange
            int estiloId = 1;
            int adminId = 1;

            _mockAdministradorRepository.Setup(r => r.GetById(adminId)).Returns((Administradores)null);

            // Act & Assert
            var exception = Assert.Throws<EstilosdecorteExceptions>(() => _repository.VerifyPermissionsEstilos(estiloId, adminId));
            Assert.Equal("Administrador no encontrado.", exception.Message);
        }

    }
}
