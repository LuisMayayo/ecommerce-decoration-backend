CREATE DATABASE EcommerceDB;

USE EcommerceDB;



-- 1. Crear la tabla de Categoria
CREATE TABLE Categoria (
    Id INT IDENTITY(1,1) PRIMARY KEY,  -- Auto-incrementable
    Nombre NVARCHAR(100) NOT NULL,  -- Nombre de la categoría
    Descripcion NVARCHAR(500)  -- Descripción de la categoría
);

-- 2. Insertar las categorías
INSERT INTO Categoria (Nombre, Descripcion)
VALUES
    ('Textil', 'Productos relacionados con textiles, como sábanas, toallas, etc.'),
    ('Confort', 'Productos que mejoran el confort, como camas, sofás, etc.'),
    ('Pinturas', 'Productos relacionados con la pintura y la decoración de interiores');

-- 3. Crear la tabla de Usuario
CREATE TABLE Usuario (
    Id INT IDENTITY(1,1) PRIMARY KEY,  -- Auto-incrementable
    Nombre NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    PasswordSalt NVARCHAR(255) NOT NULL,
    FechaRegistro DATETIME DEFAULT GETDATE()
);

-- 4. Crear la tabla de Producto
CREATE TABLE Producto (
    Id INT IDENTITY(1,1) PRIMARY KEY,  -- Auto-incrementable
    Nombre NVARCHAR(100) NOT NULL,
    Precio DECIMAL(18,2) NOT NULL,
    CategoriaId INT NOT NULL,  -- Relación con la categoría
    UrlImagen NVARCHAR(255),
    Descripcion NVARCHAR(500),
    FOREIGN KEY (CategoriaId) REFERENCES Categoria(Id)  -- Relación con la tabla Categoria
);

-- 5. Insertar datos de prueba en la tabla de Producto
INSERT INTO Producto (Nombre, Precio, CategoriaId, UrlImagen, Descripcion)
VALUES
    ('Cama King Size', 799.99, 2, 'https://example.com/cama.jpg', 'Cama de lujo para todos los gustos'),
    ('Sofá Cuero', 499.99, 2, 'https://example.com/sofa.jpg', 'Sofá de alta calidad y comodidad'),
    ('Pintura Acrílica', 15.99, 3, 'https://example.com/pintura.jpg', 'Pintura acrílica para interiores'),
    ('Toalla de baño', 12.99, 1, 'https://example.com/toalla.jpg', 'Toalla suave y absorbente');

-- 6. Crear la tabla de Pedido
CREATE TABLE Pedido (
    Id INT IDENTITY(1,1) PRIMARY KEY,  -- Auto-incrementable
    UsuarioId INT NOT NULL,  -- Relación con el Usuario
    FechaPedido DATETIME DEFAULT GETDATE(),
    Total DECIMAL(18,2) NOT NULL
);

-- 7. Crear la tabla de DetallePedido
CREATE TABLE DetallePedido (
    Id INT IDENTITY(1,1) PRIMARY KEY,  -- Auto-incrementable
    PedidoId INT NOT NULL,  -- Relación con Pedido
    ProductoId INT NOT NULL,  -- Relación con Producto
    Cantidad INT NOT NULL,
    PrecioUnitario DECIMAL(18,2) NOT NULL
);

-- 8. Crear la tabla de Reseña
CREATE TABLE Reseña (
    Id INT IDENTITY(1,1) PRIMARY KEY,  -- Auto-incrementable
    ProductoId INT NOT NULL,  -- Relación con Producto
    UsuarioId INT NOT NULL,  -- Relación con Usuario
    Comentario NVARCHAR(1000),
    Calificacion INT CHECK (Calificacion BETWEEN 1 AND 5)  -- Validación de calificación entre 1 y 5
);

-- 9. Insertar datos de prueba en la tabla de Usuario
INSERT INTO Usuario (Nombre, Email, PasswordHash, PasswordSalt, FechaRegistro)
VALUES
    ('Juan Perez', 'juan@correo.com', 'hashedpassword1', 'salt1', GETDATE()),
    ('Maria Lopez', 'maria@correo.com', 'hashedpassword2', 'salt2', GETDATE());

-- 10. Insertar datos de prueba en la tabla de Pedido
INSERT INTO Pedido (UsuarioId, Total)
VALUES
    (1, 799.99),  -- Pedido de Juan Perez
    (2, 499.99);  -- Pedido de Maria Lopez

-- 11. Insertar datos de prueba en la tabla de DetallePedido
INSERT INTO DetallePedido (PedidoId, ProductoId, Cantidad, PrecioUnitario)
VALUES
    (1, 1, 1, 799.99),  -- Detalle del pedido de Juan Perez
    (2, 2, 1, 499.99);  -- Detalle del pedido de Maria Lopez

-- 12. Insertar datos de prueba en la tabla de Reseña
INSERT INTO Reseña (ProductoId, UsuarioId, Comentario, Calificacion)
VALUES
    (1, 1, 'Excelente cama, muy cómoda.', 5),  -- Reseña de Juan sobre la Cama King Size
    (2, 2, 'Muy buen sofá, se ve elegante.', 4);  -- Reseña de Maria sobre el Sofá Cuero
