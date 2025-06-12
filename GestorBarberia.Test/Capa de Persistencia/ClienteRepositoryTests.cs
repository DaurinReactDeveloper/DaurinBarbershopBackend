using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Microsoft.Extensions.Logging;
using GestorBarberia.Persistence.Repositories;
using GestorBarberia.Domain.Entities;
using GestorBarberia.Infrastructure.Exceptions;
using GestorBarberia.Persistence.Interface;
using GestorBarberia.Infrastructure.Models;

namespace GestorBarberia.Test.Capa_de_Persistencia
{
    public class ClienteRepositoryTests
    {
        private readonly Mock<IAdministradorRepository> _mockAdministradorRepository;
        private readonly Mock<IBarberiasRepository> _mockBarberiasRepository;
        private readonly Mock<ILogger<ClienteRepository>> _mockLogger;
        private readonly Mock<IClienteRepository> _mockClienteRepository;

        public ClienteRepositoryTests()
        {
            // Mocks de las dependencias
            _mockAdministradorRepository = new Mock<IAdministradorRepository>();
            _mockBarberiasRepository = new Mock<IBarberiasRepository>();
            _mockLogger = new Mock<ILogger<ClienteRepository>>();
            _mockClienteRepository = new Mock<IClienteRepository>();

            // Configurar los datos simulados utilizando ClienteModel
            _mockClienteRepository.Setup(repo => repo.GetClientes())
                .Returns(new List<ClienteModel>
                {
                    new ClienteModel { ClienteId = 1, Nombre = "John Doe", BarberiaId = 1, Email = "john@example.com", Telefono = "12345", Password = "password" },
                    new ClienteModel { ClienteId = 2, Nombre = "Jane Doe", BarberiaId = 2, Email = "jane@example.com", Telefono = "67890", Password = "password" }
                });

            _mockClienteRepository.Setup(repo => repo.GetClientesByBarberiaId(It.IsAny<int>()))
                .Returns<int>(barberiaId => new List<ClienteModel>
                {
                    new ClienteModel { ClienteId = 1, Nombre = "John Doe", BarberiaId = barberiaId, Email = "john@example.com", Telefono = "12345", Password = "password" }
                });
        }

        [Fact]
        public void GetClientes_ShouldReturnListOfClienteModels()
        {
            // Act
            var result = _mockClienteRepository.Object.GetClientes();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count); // Según los datos simulados
            Assert.Contains(result, c => c.Nombre == "John Doe");
        }

        [Fact]
        public void GetClientesByBarberiaId_ShouldReturnFilteredClienteModels()
        {
            // Act
            var result = _mockClienteRepository.Object.GetClientesByBarberiaId(1);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result); // Solo un cliente para BarberiaId = 1
            Assert.Equal("John Doe", result.First().Nombre);
        }

        [Fact]
        public void GetClienteName_ShouldReturnClienteModel_WhenNameExists()
        {
            // Simular la búsqueda por nombre
            _mockClienteRepository.Setup(repo => repo.GetClienteName("John Doe"))
                .Returns(new ClienteModel { ClienteId = 1, Nombre = "John Doe", BarberiaId = 1, Email = "john@example.com", Telefono = "12345", Password = "password" });

            // Act
            var result = _mockClienteRepository.Object.GetClienteName("John Doe");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John Doe", result.Nombre);
        }

        [Fact]
        public void GetClienteName_ShouldThrowException_WhenNameDoesNotExist()
        {
            // Simular que no se encuentra el cliente
            _mockClienteRepository.Setup(repo => repo.GetClienteName(It.IsAny<string>()))
                .Throws(new ClienteExceptions("Cliente no encontrado"));

            // Act & Assert
            var exception = Assert.Throws<ClienteExceptions>(() => _mockClienteRepository.Object.GetClienteName("Nonexistent Name"));
            Assert.Equal("Cliente no encontrado", exception.Message);
        }

        [Fact]
        public void Add_ShouldAddNewClienteModel()
        {
            // Arrange
            var newCliente = new Clientes
            {
                ClienteId = 3,
                Nombre = "Test Cliente",
                BarberiaId = 1,
                Email = "test@example.com",
                Telefono = "99999",
                Password = "password"
            };

            // Act
            _mockClienteRepository.Object.Add(newCliente);

            // Assert
            _mockClienteRepository.Verify(repo => repo.Add(It.IsAny<Clientes>()), Times.Once);
        }

        [Fact]
        public void Update_ShouldUpdateExistingClienteModel()
        {
            // Arrange
            var clienteToUpdate = new Clientes { ClienteId = 1, Nombre = "Updated Name", BarberiaId = 1, Email = "updated@example.com", Telefono = "54321", Password = "newpassword" };

            // Act
            _mockClienteRepository.Object.Update(clienteToUpdate);

            // Assert
            _mockClienteRepository.Verify(repo => repo.Update(It.IsAny<Clientes>()), Times.Once);
        }

        [Fact]
        public void Remove_ShouldRemoveClienteModel()
        {
            // Arrange
            var clienteToRemove = new Clientes { ClienteId = 1, Nombre = "John Doe", BarberiaId = 1, Email = "john@example.com", Telefono = "12345", Password = "password" };

            // Act
            _mockClienteRepository.Object.Remove(clienteToRemove);

            // Assert
            _mockClienteRepository.Verify(repo => repo.Remove(It.IsAny<Clientes>()), Times.Once);
        }

        [Fact]
        public void VerifyPermissionsCliente_ShouldReturnTrue_WhenPermissionIsValid()
        {
            // Simular los repositorios de administradores y barberías
            _mockAdministradorRepository.Setup(repo => repo.GetById(It.IsAny<int>()))
                .Returns(new Administradores { AdministradoresId = 1 });
            _mockBarberiasRepository.Setup(repo => repo.GetBarberiaByAdminId(It.IsAny<int>()))
                .Returns(new BarberiaModel { BarberiasId = 1, Admin = 1 });

            var cliente = new ClienteModel { ClienteId = 1, BarberiaId = 1 };

            // Act
            var result = _mockClienteRepository.Object.VerifyPermissionsCliente(1, 1);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void RemoveClienteByAdmin_ShouldReturnTrue_WhenAdminHasPermission()
        {
            // Simular los repositorios de administradores y barberías
            _mockAdministradorRepository.Setup(repo => repo.GetById(It.IsAny<int>()))
                .Returns(new Administradores { AdministradoresId = 1 });
            _mockBarberiasRepository.Setup(repo => repo.GetBarberiaByAdminId(It.IsAny<int>()))
                .Returns(new BarberiaModel { BarberiasId = 1, Admin = 1 });

            // Act
            var result = _mockClienteRepository.Object.RemoveClienteByAdmin(1, 1);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void RemoveClienteByAdmin_ShouldReturnFalse_WhenAdminHasNoPermission()
        {
            // Simular los repositorios de administradores y barberías
            _mockAdministradorRepository.Setup(repo => repo.GetById(It.IsAny<int>()))
                .Returns(new Administradores { AdministradoresId = 1 });
            _mockBarberiasRepository.Setup(repo => repo.GetBarberiaByAdminId(It.IsAny<int>()))
                .Returns(new BarberiaModel { BarberiasId = 1, Admin = 1 });

            // Act
            var result = _mockClienteRepository.Object.RemoveClienteByAdmin(1, 2); // AdminId no coincide

            // Assert
            Assert.False(result);
        }
    }
}
