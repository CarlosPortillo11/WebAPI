CREATE DATABASE AtlantidaDB;

USE AtlantidaDB;

CREATE TABLE [Cuenta] (
  NumeroTarjeta nvarchar(16) PRIMARY KEY NOT NULL ,
  NombreTitular nvarchar(255) NOT NULL,
  SaldoActual decimal(10, 2) NOT NULL,
  SaldoDisponible decimal(10, 2) NOT NULL,
  LimiteCredito decimal(10, 2) NOT NULL,
  InteresBonificable decimal(10, 2) NOT NULL,
  CuotaMinima decimal(10, 2) NOT NULL,
  MontoPago decimal(10, 2) NOT NULL,
  MontoPagoIntereses decimal(10, 2) NOT NULL
)

INSERT INTO Cuenta (NumeroTarjeta, NombreTitular, SaldoActual, SaldoDisponible, LimiteCredito, InteresBonificable, CuotaMinima, MontoPago, MontoPagoIntereses)
VALUES ('4390930039010961', 'Carlos Portillo', 604.5, 395.5, 1000, 30.23, 30.23, 604.5, 634.73);

CREATE TABLE Transaccion (
	ID int IDENTITY(1,1) NOT NULL,
	NumeroTarjeta nvarchar(16) NOT NULL,
	Tipo nvarchar(255) NOT NULL,
	Fecha date NOT NULL,
	Descripcion nvarchar(255),
	Monto decimal(10, 2)
)

ALTER TABLE [Transaccion] ADD FOREIGN KEY (NumeroTarjeta) REFERENCES [Cuenta] (NumeroTarjeta)

INSERT INTO Transaccion (NumeroTarjeta, Tipo, Fecha, Descripcion, Monto) VALUES ('4390930039010961', 'Compra', '2023-10-25', 'Pantalones Cargo', 100)
,('4390930039010961', 'Compra', '2023-10-12', 'Recarga movistar', 10)
,('4390930039010961', 'Pago', '2023-10-26', 'Pago', 5.50)
,('4390930039010961', 'Compra', '2023-11-3', 'Memoria ram 2x8gb', 65)
,('4390930039010961', 'Compra', '2023-11-3', 'Pedidos Ya',26.78)
,('4390930039010961', 'Compra', '2023-11-3', 'Super Selectos', 72.96)
,('4390930039010961', 'Compra', '2023-11-5', 'Tienda Adoc', 25.99)
,('4390930039010961', 'Compra', '2023-11-5', 'Zona Digital', 168.99)
,('4390930039010961', 'Compra', '2023-11-6', 'Tacos Hermanos', 62.67)
,('4390930039010961', 'Compra', '2023-11-9', 'CAEES', 52.31)
,('4390930039010961', 'Compra', '2023-11-11', 'Pedidos Ya', 25.3)

