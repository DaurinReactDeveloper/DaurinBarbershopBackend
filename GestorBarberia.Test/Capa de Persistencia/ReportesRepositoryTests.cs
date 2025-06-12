using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using GestorBarberia.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using GestorBarberia.Persistence.Context;
using GestorBarberia.Persistence.Repositories;
using GestorBarberia.Domain.Entities;

namespace GestorBarberia.Test.Capa_de_Persistencia
{
    public class ReportesRepositoryTests
    {
        private readonly Mock<DbContextBarberia> _mockDbContext;
        private readonly Mock<ILogger<ReportesRepository>> _mockLogger;
        private readonly ReportesRepository _repository;

        public ReportesRepositoryTests()
        {
            _mockDbContext = new Mock<DbContextBarberia>();
            _mockLogger = new Mock<ILogger<ReportesRepository>>();
            _repository = new ReportesRepository(_mockDbContext.Object, _mockLogger.Object);
        }

        [Fact]
        public void GenerarTablaIngresos_ShouldReturnCorrectReportData()
        {
            // Arrange
            int barberiaId = 1;
            DateTime fechaInicio = new DateTime(2024, 1, 1);
            DateTime fechaFin = new DateTime(2024, 1, 31);

            var citas = new List<Citas>
            {
                new Citas
                {
                    CitaId = 1,
                    Fecha = new DateTime(2024, 1, 5),
                    Estado = "Realizada",
                    BarberiaId = barberiaId,
                    Barbero = new Barberos { Nombre = "Barbero 1" },
                    Estilo = new Estilosdecortes { Precio = 50 }
                },
                new Citas
                {
                    CitaId = 2,
                    Fecha = new DateTime(2024, 1, 5),
                    Estado = "Realizada",
                    BarberiaId = barberiaId,
                    Barbero = new Barberos { Nombre = "Barbero 2" },
                    Estilo = new Estilosdecortes { Precio = 60 }
                },
                new Citas
                {
                    CitaId = 3,
                    Fecha = new DateTime(2024, 1, 10),
                    Estado = "Realizada",
                    BarberiaId = barberiaId,
                    Barbero = new Barberos { Nombre = "Barbero 1" },
                    Estilo = new Estilosdecortes { Precio = 50 }
                }
            };

            var mockDbSetCitas = new Mock<DbSet<Citas>>();
            mockDbSetCitas.As<IQueryable<Citas>>()
                .Setup(m => m.Provider).Returns(citas.AsQueryable().Provider);
            mockDbSetCitas.As<IQueryable<Citas>>()
                .Setup(m => m.Expression).Returns(citas.AsQueryable().Expression);
            mockDbSetCitas.As<IQueryable<Citas>>()
                .Setup(m => m.ElementType).Returns(citas.AsQueryable().ElementType);
            mockDbSetCitas.As<IQueryable<Citas>>()
                .Setup(m => m.GetEnumerator()).Returns(citas.GetEnumerator());

            _mockDbContext.Setup(x => x.Citas).Returns(mockDbSetCitas.Object);

            // Act
            var result = _repository.GenerarTablaIngresos(barberiaId, fechaInicio, fechaFin);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(2, result[0].CitasRealizadas);
            Assert.Equal(110, result[0].IngresoCitaRealizada);
            Assert.Equal("Barbero 1", result[0].Barbero);
        }

        [Fact]
        public void ObtenerTotalIngresos_ShouldReturnTotalIngresos()
        {
            // Arrange
            int barberiaId = 1;
            var citas = new List<Citas>
            {
                new Citas { Estilo = new Estilosdecortes { Precio = 50 }, BarberiaId = barberiaId, Estado = "Realizada" },
                new Citas { Estilo = new Estilosdecortes { Precio = 60 }, BarberiaId = barberiaId, Estado = "Realizada" }
            };

            var mockDbSetCitas = new Mock<DbSet<Citas>>();
            mockDbSetCitas.As<IQueryable<Citas>>()
                .Setup(m => m.Provider).Returns(citas.AsQueryable().Provider);
            mockDbSetCitas.As<IQueryable<Citas>>()
                .Setup(m => m.Expression).Returns(citas.AsQueryable().Expression);
            mockDbSetCitas.As<IQueryable<Citas>>()
                .Setup(m => m.ElementType).Returns(citas.AsQueryable().ElementType);
            mockDbSetCitas.As<IQueryable<Citas>>()
                .Setup(m => m.GetEnumerator()).Returns(citas.GetEnumerator());

            _mockDbContext.Setup(x => x.Citas).Returns(mockDbSetCitas.Object);

            // Act
            var result = _repository.ObtenerTotalIngresos(barberiaId);

            // Assert
            Assert.Equal(110, result);
        }

        [Fact]
        public void ObtenerTotalClientes_ShouldReturnTotalClientes()
        {
            // Arrange
            int barberiaId = 1;
            var clientes = new List<Clientes>
            {
                new Clientes { ClienteId = 1, BarberiaId = barberiaId },
                new Clientes { ClienteId = 2, BarberiaId = barberiaId }
            };

            var mockDbSetClientes = new Mock<DbSet<Clientes>>();
            mockDbSetClientes.As<IQueryable<Clientes>>()
                .Setup(m => m.Provider).Returns(clientes.AsQueryable().Provider);
            mockDbSetClientes.As<IQueryable<Clientes>>()
                .Setup(m => m.Expression).Returns(clientes.AsQueryable().Expression);
            mockDbSetClientes.As<IQueryable<Clientes>>()
                .Setup(m => m.ElementType).Returns(clientes.AsQueryable().ElementType);
            mockDbSetClientes.As<IQueryable<Clientes>>()
                .Setup(m => m.GetEnumerator()).Returns(clientes.GetEnumerator());

            _mockDbContext.Setup(x => x.Clientes).Returns(mockDbSetClientes.Object);

            // Act
            var result = _repository.ObtenerTotalClientes(barberiaId);

            // Assert
            Assert.Equal(2, result);
        }

        [Fact]
        public void ObtenerTotalBarberos_ShouldReturnTotalBarberos()
        {
            // Arrange
            int barberiaId = 1;
            var barberos = new List<Barberos>
            {
                new Barberos { BarberoId = 1, BarberiaId = barberiaId },
                new Barberos { BarberoId = 2, BarberiaId = barberiaId }
            };

            var mockDbSetBarberos = new Mock<DbSet<Barberos>>();
            mockDbSetBarberos.As<IQueryable<Barberos>>()
                .Setup(m => m.Provider).Returns(barberos.AsQueryable().Provider);
            mockDbSetBarberos.As<IQueryable<Barberos>>()
                .Setup(m => m.Expression).Returns(barberos.AsQueryable().Expression);
            mockDbSetBarberos.As<IQueryable<Barberos>>()
                .Setup(m => m.ElementType).Returns(barberos.AsQueryable().ElementType);
            mockDbSetBarberos.As<IQueryable<Barberos>>()
                .Setup(m => m.GetEnumerator()).Returns(barberos.GetEnumerator());

            _mockDbContext.Setup(x => x.Barberos).Returns(mockDbSetBarberos.Object);

            // Act
            var result = _repository.ObtenerTotalBarberos(barberiaId);

            // Assert
            Assert.Equal(2, result);
        }
    }
}
