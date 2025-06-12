using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Xunit;
using GestorBarberia.Domain.Entities;
using GestorBarberia.Persistence.Context;
using GestorBarberia.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GestorBarberia.Test.Capa_de_Persistencia
{
    public class AdminRepositoryTests
    {
        private readonly Mock<DbContextBarberia> _mockDbContext;
        private readonly Mock<DbSet<Administradores>> _mockDbSet;
        private readonly Mock<ILogger<AdministradorRepository>> _mockLogger;
        private readonly AdministradorRepository _repository;

        public AdminRepositoryTests()
        {
            _mockDbContext = new Mock<DbContextBarberia>();
            _mockDbSet = new Mock<DbSet<Administradores>>();
            _mockLogger = new Mock<ILogger<AdministradorRepository>>();
            _repository = new AdministradorRepository(_mockDbContext.Object, _mockLogger.Object);
        }

        [Fact]
        public void GetAdministradores_ReturnsListOfAdministradores()
        {
            // Arrange
            var adminList = new List<Administradores>
        {
            new Administradores { AdministradoresId = 1, Nombre = "Admin1", Email = "admin1@test.com", Telefono = "123456789", Tipo = "SuperAdmin" },
            new Administradores { AdministradoresId = 2, Nombre = "Admin2", Email = "admin2@test.com", Telefono = "987654321", Tipo = "Admin" }
        }.AsQueryable();

            _mockDbSet.As<IQueryable<Administradores>>().Setup(m => m.Provider).Returns(adminList.Provider);
            _mockDbSet.As<IQueryable<Administradores>>().Setup(m => m.Expression).Returns(adminList.Expression);
            _mockDbSet.As<IQueryable<Administradores>>().Setup(m => m.ElementType).Returns(adminList.ElementType);
            _mockDbSet.As<IQueryable<Administradores>>().Setup(m => m.GetEnumerator()).Returns(adminList.GetEnumerator());

            _mockDbContext.Setup(c => c.Administradores).Returns(_mockDbSet.Object);

            // Act
            var result = _repository.GetAdministradores();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Admin1", result[0].Nombre);
        }

        [Fact]
        public void GetAdministrador_ReturnsAdministradorByName()
        {
            // Arrange
            var admin = new Administradores
            {
                AdministradoresId = 1,
                Nombre = "Admin1",
                Email = "admin1@test.com",
                Telefono = "123456789",
                Tipo = "SuperAdmin"
            };

            var adminList = new List<Administradores> { admin }.AsQueryable();

            // Configurar el mock del DbSet
            _mockDbSet.As<IQueryable<Administradores>>().Setup(m => m.Provider).Returns(adminList.Provider);
            _mockDbSet.As<IQueryable<Administradores>>().Setup(m => m.Expression).Returns(adminList.Expression);
            _mockDbSet.As<IQueryable<Administradores>>().Setup(m => m.ElementType).Returns(adminList.ElementType);
            _mockDbSet.As<IQueryable<Administradores>>().Setup(m => m.GetEnumerator()).Returns(adminList.GetEnumerator());

            // Configurar el DbContext mock
            _mockDbContext.Setup(c => c.Administradores).Returns(_mockDbSet.Object);

            // Act
            var result = _repository.GetAdministrador("Admin1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Admin1", result.Nombre);
        }

        [Fact]
        public void GetAdministradorById_ReturnsAdministradorById()
        {
            // Arrange
            var admin = new Administradores { AdministradoresId = 1, Nombre = "Admin1", Email = "admin1@test.com", Telefono = "123456789", Tipo = "SuperAdmin" };

            var adminList = new List<Administradores> { admin }.AsQueryable();

            _mockDbSet.As<IQueryable<Administradores>>().Setup(m => m.Provider).Returns(adminList.Provider);
            _mockDbSet.As<IQueryable<Administradores>>().Setup(m => m.Expression).Returns(adminList.Expression);
            _mockDbSet.As<IQueryable<Administradores>>().Setup(m => m.ElementType).Returns(adminList.ElementType);
            _mockDbSet.As<IQueryable<Administradores>>().Setup(m => m.GetEnumerator()).Returns(adminList.GetEnumerator());

            _mockDbContext.Setup(c => c.Administradores).Returns(_mockDbSet.Object);

            // Act
            var result = _repository.GetAdministradorById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.AdministradoresId);
            Assert.Equal("Admin1", result.Nombre);
        }

        [Fact]
        public void VerifyNameAdmin_ReturnsCorrectBoolean()
        {
            // Arrange
            var admin = new Administradores { AdministradoresId = 1, Nombre = "Admin1", Email = "admin1@test.com", Telefono = "123456789", Tipo = "SuperAdmin" };

            var adminList = new List<Administradores> { admin }.AsQueryable();

            _mockDbSet.As<IQueryable<Administradores>>().Setup(m => m.Provider).Returns(adminList.Provider);
            _mockDbSet.As<IQueryable<Administradores>>().Setup(m => m.Expression).Returns(adminList.Expression);
            _mockDbSet.As<IQueryable<Administradores>>().Setup(m => m.ElementType).Returns(adminList.ElementType);
            _mockDbSet.As<IQueryable<Administradores>>().Setup(m => m.GetEnumerator()).Returns(adminList.GetEnumerator());

            _mockDbContext.Setup(c => c.Administradores).Returns(_mockDbSet.Object);

            // Act
            var resultExist = _repository.VerifyNameAdmin("Admin1");
            var resultNotExist = _repository.VerifyNameAdmin("NonExistingAdmin");

            // Assert
            Assert.False(resultExist);  // El nombre "Admin1" ya existe
            Assert.True(resultNotExist);  // El nombre "NonExistingAdmin" no existe
        }


    }
}
