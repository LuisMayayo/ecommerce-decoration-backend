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
    ('Textil', 'Productos relacionados con textiles, como sábanas, toallas, cortinas, alfombras, etc.'),
    ('Accesorio decorativo', 'Productos que mejoran la estética de los espacios, como lámparas, jarrones, cuadros, estanterías, etc.'),
    ('Decoración vertical', 'Productos relacionados con la decoración de paredes, como cuadros, espejos, vinilos, estanterías y otros accesorios verticales.');


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
-- Insertar productos en la categoría 'Textil'
INSERT INTO Producto (Nombre, Precio, CategoriaId, UrlImagen, Descripcion)
VALUES
    ('ALFOMBRA MULTICOLOR LANA-ALGODÓN 160 X 230 CM', 199.99, 1, 'https://ixia.es/media/catalog/product/6/1/614920.jpg?width=345&height=345&image-type=small_image&width=345&t=1739749309', 'ALFOMBRA MULTICOLOR DE LANA Y ALGODÓN, TAMAÑO 160 X 230 CM. IDEAL PARA ESPACIOS DE ESTILO MODERNO.'),
    ('ALFOMBRA BLANCO NATURAL LANA-ALGODÓN 200 X 300 CM', 249.99, 1, 'https://ixia.es/media/catalog/product/6/1/614918.jpg?width=345&height=345&image-type=small_image&width=345&t=1739749309', 'ALFOMBRA DE COLOR BLANCO NATURAL, FABRICADA EN LANA Y ALGODÓN, TAMAÑO 200 X 300 CM. PERFECTA PARA SALAS DE ESTAR ELEGANTES.'),
    ('ALFOMBRA MARRÓN POLIPROPILENO 160 X 230 CM', 129.99, 1, 'https://ixia.es/media/catalog/product/6/1/612752.jpg?width=345&height=345&image-type=small_image&width=345&t=1739747541', 'ALFOMBRA MARRÓN DE POLIPROPILENO, TAMAÑO 160 X 230 CM. RESISTENTE Y FÁCIL DE MANTENER.'),
    ('ALFOMBRA GRIS POLIÉSTER-ALGODÓN 80 X 150 CM', 59.99, 1, 'https://ixia.es/media/catalog/product/6/0/609529.jpg?width=345&height=345&image-type=small_image&width=345&t=1739748853', 'ALFOMBRA GRIS DE POLIÉSTER Y ALGODÓN, TAMAÑO 80 X 150 CM. IDEAL PARA HABITACIONES Y OFICINAS.'),
    ('ALFOMBRA NATURAL YUTE DECORACIÓN 180 X 180 X 1 CM', 119.99, 1, 'https://ixia.es/media/catalog/product/1/0/106830.jpg?quality=80&bg-color=255,255,255&fit=bounds&height=700&width=700&canvas=700:700', 'ALFOMBRA NATURAL DE YUTE PARA DECORACIÓN, TAMAÑO 180 X 180 X 1 CM. PERFECTA PARA INTERIORES RÚSTICOS.'),
    ('ALFOMBRA CONEJO GRIS ALGODÓN INFANTIL 100 CM', 39.99, 1, 'https://ixia.es/media/catalog/product/6/0/608553.jpg?quality=80&bg-color=255,255,255&fit=bounds&height=700&width=700&canvas=700:700', 'ALFOMBRA INFANTIL CON DISEÑO DE CONEJO, HECHA DE ALGODÓN, DIÁMETRO DE 100 CM. IDEAL PARA NIÑOS.'),
    ('ALFOMBRA OSO MARRÓN ALGODÓN INFANTIL 100 CM', 39.99, 1, 'https://ixia.es/media/catalog/product/6/0/608552.jpg?quality=80&bg-color=255,255,255&fit=bounds&height=700&width=700&canvas=700:700', 'ALFOMBRA INFANTIL DE ALGODÓN CON DISEÑO DE OSO MARRÓN, DIÁMETRO DE 100 CM. COMODIDAD Y DIVERSIÓN PARA LOS PEQUEÑOS.'),
    ('COJÍN VERDE ALGODÓN-POLIÉSTER 50 X 30 CM', 19.99, 1, 'https://ixia.es/media/catalog/product/6/1/614934.jpg?quality=80&bg-color=255,255,255&fit=bounds&height=700&width=700&canvas=700:700', 'COJÍN VERDE DE ALGODÓN Y POLIÉSTER, TAMAÑO 50 X 30 CM. APORTA FRESCURA A TU HOGAR.'),
    ('COJÍN TIERRA ALGODÓN-POLIÉSTER 50 X 30 CM', 19.99, 1, 'https://ixia.es/media/catalog/product/6/1/614935.jpg?quality=80&bg-color=255,255,255&fit=bounds&height=700&width=700&canvas=700:700', 'COJÍN EN TONOS TIERRA, HECHO DE ALGODÓN Y POLIÉSTER, TAMAÑO 50 X 30 CM. PERFECTO PARA EL SOFÁ.'),
    ('COJÍN BLANCO ALGODÓN-POLIÉSTER 50 X 30 CM', 19.99, 1, 'https://ixia.es/media/catalog/product/6/1/614933.jpg?quality=80&bg-color=255,255,255&fit=bounds&height=700&width=700&canvas=700:700', 'COJÍN BLANCO DE ALGODÓN Y POLIÉSTER, TAMAÑO 50 X 30 CM. SIMPLE Y ELEGANTE PARA CUALQUIER ESPACIO.'),
    ('COJÍN CREMA JACQUARD DECORACIÓN 45 X 45 CM', 24.99, 1, 'https://ixia.es/media/catalog/product/6/1/615076.jpg?quality=80&bg-color=255,255,255&fit=bounds&height=700&width=700&canvas=700:700', 'COJÍN JACQUARD CREMA PARA DECORACIÓN, TAMAÑO 45 X 45 CM. APORTA UN TOQUE ELEGANTE A TU HOGAR.'),
    ('COJÍN BLANCO/BEIGE JACQUARD DECORACIÓN 45 X 45 CM', 24.99, 1, 'https://ixia.es/media/catalog/product/6/1/615080.jpg?quality=80&bg-color=255,255,255&fit=bounds&height=700&width=700&canvas=700:700', 'COJÍN DECORATIVO BLANCO/BEIGE JACQUARD, TAMAÑO 45 X 45 CM. COMBINA CON CUALQUIER ESTILO DE DECORACIÓN.'),
    ('COJÍN BLANCO-VERDE JACQUARD DECORACIÓN 45 X 45 CM', 24.99, 1, 'https://ixia.es/media/catalog/product/6/1/615072.jpg?quality=80&bg-color=255,255,255&fit=bounds&height=700&width=700&canvas=700:700', 'COJÍN DECORATIVO BLANCO-VERDE JACQUARD, TAMAÑO 45 X 45 CM. IDEAL PARA RESALTAR TU ESPACIO.');


-- Insertar productos en la categoría 'Decoración vertical'
INSERT INTO Producto (Nombre, Precio, CategoriaId, UrlImagen, Descripcion)
VALUES
    ('CUADRO PINTURA ABSTRACTO LIENZO 60 X 150 CM', 159.99, 2, 'https://ixia.es/media/catalog/product/6/1/615166.jpg?width=345&height=345&image-type=small_image&width=345&t=1739749315', 'CUADRO DE PINTURA ABSTRACTO EN LIENZO, TAMAÑO 60 X 150 CM. IDEAL PARA AÑADIR UN TOQUE MODERNO A TU ESPACIO.'),
    ('CUADRO PINTURA ABSTRACTO 44 X 50 CM', 89.99, 2, 'https://ixia.es/media/catalog/product/6/1/615155.jpg?width=345&height=345&image-type=small_image&width=345&t=1739749314', 'CUADRO DE PINTURA ABSTRACTO, TAMAÑO 44 X 50 CM. UNA PIEZA ELEGANTE Y SIMPLISTA PARA TU HOGAR.'),
    ('CUADRO PINTURA ABSTRACTO 60 X 80 CM', 129.99, 2, 'https://ixia.es/media/catalog/product/6/1/615159.jpg?width=345&height=345&image-type=small_image&width=345&t=1739749315', 'CUADRO DE PINTURA ABSTRACTO, TAMAÑO 60 X 80 CM. AÑADE COLOR Y ESTILO A TU ESPACIO.'),
    ('CUADRO PINTURA ABSTRACTO 70 X 70 CM', 149.99, 2, 'https://ixia.es/media/catalog/product/6/1/615162.jpg?width=345&height=345&image-type=small_image&width=345&t=1739749315', 'CUADRO DE PINTURA ABSTRACTO, TAMAÑO 70 X 70 CM. UNA PIEZA IMPACTANTE PARA CUALQUIER HABITACIÓN.'),
    ('PINTURA ABSTRACTO LIENZO DECORACIÓN 120 X 150 CM', 179.99, 2, 'https://ixia.es/media/catalog/product/6/1/615168.jpg?width=345&height=345&image-type=small_image&width=345&t=1739749315', 'PINTURA ABSTRACTA EN LIENZO, TAMAÑO 120 X 150 CM. UNA PIEZA ARTÍSTICA PARA DECORAR TU PARED.'),
    ('CUADRO PINTURA ABSTRACTO 52 X 62 CM', 99.99, 2, 'https://ixia.es/media/catalog/product/6/1/615144.jpg?width=345&height=345&image-type=small_image&width=345&t=1739749314', 'CUADRO DE PINTURA ABSTRACTO, TAMAÑO 52 X 62 CM. UNA PIEZA MODERNA PARA TU DECORACIÓN.'),
    ('PINTURA ABSTRACTO LIENZO DECORACIÓN 120 X 3 X 60 CM', 189.99, 2, 'https://ixia.es/media/catalog/product/6/1/614404.jpg?width=345&height=345&image-type=small_image&width=345&t=1739749297', 'PINTURA ABSTRACTA EN LIENZO, TAMAÑO 120 X 3 X 60 CM. IDEAL PARA DAR UNA NUEVA PERSPECTIVA A TU HOGAR.'),
    ('PINTURA ABSTRACTO LIENZO DECORACIÓN 50 X 120 CM', 139.99, 2, 'https://ixia.es/media/catalog/product/6/1/614411.jpg?width=345&height=345&image-type=small_image&width=345&t=1739749297', 'PINTURA ABSTRACTA EN LIENZO, TAMAÑO 50 X 120 CM. UNA PIEZA DECORATIVA Y ESTILOSA PARA TU HOGAR.'),
    ('CUADRO PINTURA ABSTRACTO 50 X 4 X 50 CM', 109.99, 2, 'https://ixia.es/media/catalog/product/6/1/612473.jpg?width=345&height=345&image-type=small_image&width=345&t=1739747528', 'CUADRO DE PINTURA ABSTRACTO, TAMAÑO 50 X 4 X 50 CM. UNA COMBINACIÓN PERFECTA DE ARTE Y DISEÑO.'),
    ('PINTURA ABSTRACTO LIENZO DECORACIÓN 100 X 100 CM', 169.99, 2, 'https://ixia.es/media/catalog/product/6/1/614413.jpg?width=345&height=345&image-type=small_image&width=345&t=1739749297', 'PINTURA ABSTRACTA EN LIENZO, TAMAÑO 100 X 100 CM. UNA PIEZA DE IMPACTO PARA TU HOGAR.'),
    ('PINTURA MUJER LIENZO DECORACIÓN 80 X 3,50 X 80 CM', 199.99, 2, 'https://ixia.es/media/catalog/product/6/1/610822.jpg?width=345&height=345&image-type=small_image&width=345&t=1739747225', 'PINTURA DE MUJER EN LIENZO, TAMAÑO 80 X 3,50 X 80 CM. UNA PIEZA ÚNICA Y ELEGANTE.'),
    ('PINTURA AFRICANA LIENZO 90 X 3,50 X 120 CM', 219.99, 2, 'https://ixia.es/media/catalog/product/6/1/613361.jpg?width=345&height=345&image-type=small_image&width=345&t=1739749252', 'PINTURA AFRICANA EN LIENZO, TAMAÑO 90 X 3,50 X 120 CM. UNA PIEZA ARTÍSTICA QUE APORTA PERSONALIDAD A TU ESPACIO.'),
    ('ESPEJO DM-CRISTAL DECORACIÓN 45 X 1 X 45 CM', 89.99, 2, 'https://ixia.es/media/catalog/product/6/0/608790.jpg?width=345&height=345&image-type=small_image&width=345&t=1739748561', 'ESPEJO DECORATIVO EN DM Y CRISTAL, TAMAÑO 45 X 1 X 45 CM. UN TOQUE ELEGANTE PARA CUALQUIER HABITACIÓN.'),
    ('ESPEJO NEGRO METAL-CRISTAL DECORACIÓN 82 X 7 X 113 CM', 349.99, 2, 'https://ixia.es/media/catalog/product/6/1/613600.jpg?width=345&height=345&image-type=small_image&width=345&t=1739749264', 'ESPEJO DE METAL NEGRO Y CRISTAL, TAMAÑO 82 X 7 X 113 CM. IDEAL PARA UN AMBIENTE MODERNO Y ELEGANTE.'),
    ('ESPEJO MARCO DORADO MADERA DE PINO 65 X 65 CM', 149.99, 2, 'https://ixia.es/media/catalog/product/6/1/610510.jpg?width=345&height=345&image-type=small_image&width=345&t=1739747213', 'ESPEJO CON MARCO DORADO DE MADERA DE PINO, TAMAÑO 65 X 65 CM. APORTA LUJO Y CALIDEZ A TU HOGAR.');


    -- Insertar productos en la categoría 'Accesorio decorativo'
INSERT INTO Producto (Nombre, Precio, CategoriaId, UrlImagen, Descripcion)
VALUES
    ('FIGURA ABSTRACTA POLIRESINA DECORACIÓN 27,50 X 14,50 X 29 CM', 49.99, 2, 'https://ixia.es/media/catalog/product/6/1/613483.jpg?width=345&height=345&image-type=small_image&width=345&t=1739749255', 'Figura abstracta en poliresina, tamaño 27,50 X 14,50 X 29 cm. Ideal para decoración de interiores.'),
    ('FIGURA CÍRCULO BLANCO-ORO RESINA 45 X 10 X 57 CM', 79.99, 2, 'https://ixia.es/media/catalog/product/6/1/610484.jpg?width=345&height=345&image-type=small_image&width=345&t=1739747212', 'Figura en resina de círculo blanco y oro, tamaño 45 X 10 X 57 cm. Perfecta para decorar cualquier habitación.'),
    ('ESCULTURA ANILLO NATURAL-BLANCO 25,50 X 9,50 X 37 CM', 89.99, 2, 'https://ixia.es/media/catalog/product/6/0/609776.jpg?width=345&height=345&image-type=small_image&width=345&t=1739748865', 'Escultura de anillo natural con detalles en blanco, tamaño 25,50 X 9,50 X 37 cm.'),
    ('FIGURA NATURAL-NEGRO MADERA-HIERRO 17 X 17 X 31 CM', 59.99, 2, 'https://ixia.es/media/catalog/product/6/0/606256.jpg?width=345&height=345&image-type=small_image&width=345&t=1739750708', 'Figura natural en madera y hierro negro, tamaño 17 X 17 X 31 cm. Elegante y moderna.'),
    ('FIGURA ABSTRACTA NATURAL MADERA DE MANGO 15 X 9 X 68,50 CM', 129.99, 2, 'https://ixia.es/media/catalog/product/6/0/609768.jpg?width=345&height=345&image-type=small_image&width=345&t=1739748865', 'Figura abstracta en madera de mango natural, medidas 15 X 9 X 68,50 cm.'),
    ('ESCULTURA NATURAL MADERA DE MANGO 38 X 8 X 52 CM', 149.99, 2, 'https://ixia.es/media/catalog/product/6/0/609762.jpg?width=345&height=345&image-type=small_image&width=345&t=1739748864', 'Escultura elegante de madera de mango, tamaño 38 X 8 X 52 cm.'),
    ('FIGURA AFRICANO NATURAL MADERA DE MANGO 14 X 14 X 88,50 CM', 199.99, 2, 'https://ixia.es/media/catalog/product/6/0/609765.jpg?width=345&height=345&image-type=small_image&width=345&t=1739748865', 'Figura africana en madera de mango natural, medidas 14 X 14 X 88,50 cm.'),
    ('CAJA DORADO METAL DECORACIÓN 31 X 21,50 X 35,50 CM', 69.99, 2, 'https://ixia.es/media/catalog/product/6/1/613122.jpg?width=345&height=345&image-type=small_image&width=345&t=1739749242', 'Caja decorativa dorada en metal, tamaño 31 X 21,50 X 35,50 cm.'),
    ('FIGURA MUJER COBRE RESINA DECORACIÓN 18 X 16 X 63 CM', 79.99, 2, 'https://ixia.es/media/catalog/product/6/1/610476.jpg?width=345&height=345&image-type=small_image&width=345&t=1739747212', 'Figura de mujer en resina cobre para decoración, medidas 18 X 16 X 63 cm.'),
    ('ESCULTURA PERSONAS COBRE RESINA 40 X 10,50 X 34 CM', 99.99, 2, 'https://ixia.es/media/catalog/product/6/1/610487.jpg?width=345&height=345&image-type=small_image&width=345&t=1739747212', 'Escultura de personas en resina cobre, medidas 40 X 10,50 X 34 cm.'),
    ('FIGURA PAVO REAL ORO METAL DECORACIÓN 50 X 30 X 85 CM', 129.99, 2, 'https://ixia.es/media/catalog/product/6/1/610280.jpg?width=345&height=345&image-type=small_image&width=345&t=1739748881', 'Figura de pavo real en metal dorado para decoración, tamaño 50 X 30 X 85 cm.'),
    ('FIGURA FLAMENCO ORO METAL DECORACIÓN 25 X 21 X 85 CM', 109.99, 2, 'https://ixia.es/media/catalog/product/6/1/610281.jpg?width=345&height=345&image-type=small_image&width=345&t=1739748881', 'Figura de flamenco en metal dorado para decoración, tamaño 25 X 21 X 85 cm.'),
    ('FIGURA ÁRBOL ORO METAL DECORACIÓN 26 X 26 X 83 CM', 119.99, 2, 'https://ixia.es/media/catalog/product/6/1/613945.jpg?width=345&height=345&image-type=small_image&width=345&t=1739749278', 'Figura de árbol en metal dorado para decoración, tamaño 26 X 26 X 83 cm.'),
    ('CORONA BLANCO MADERA PAULONIA DECORACIÓN 52 X 15 X 52 CM', 89.99, 2, 'https://ixia.es/media/catalog/product/6/1/610781.jpg?width=345&height=345&image-type=small_image&width=345&t=1739747223', 'Corona decorativa en madera paulownia blanca, medidas 52 X 15 X 52 cm.'),
    ('FIGURA ÁNGEL NATURAL MADERA DE MANGO 22,50 X 9 X 84,50 CM', 159.99, 2, 'https://ixia.es/media/catalog/product/6/0/609758.jpg?width=345&height=345&image-type=small_image&width=345&t=1739748864', 'Figura de ángel en madera de mango natural para decoración, tamaño 22,50 X 9 X 84,50 cm.'),
    ('FIGURA GALLINA BLANCO ROZADO METAL 34 X 12 X 38 CM', 49.99, 2, 'https://ixia.es/media/catalog/product/6/1/613871.jpg?width=345&height=345&image-type=small_image&width=345&t=1739749275', 'Figura de gallina en metal blanco rozado, tamaño 34 X 12 X 38 cm.'),
    ('FIGURA PEZ POLIRESINA DECORACIÓN 25,70 X 14 X 35,70 CM', 69.99, 2, 'https://ixia.es/media/catalog/product/6/1/613467.jpg?width=345&height=345&image-type=small_image&width=345&t=1739749254', 'Figura de pez en poliresina para decoración, tamaño 25,70 X 14 X 35,70 cm.'),
    ('FIGURA PEZ MADERA-FIBRA NATURAL 70 X 12 X 53 CM', 89.99, 2, 'https://ixia.es/media/catalog/product/6/0/609795.jpg?width=345&height=345&image-type=small_image&width=345&t=1739748866', 'Figura de pez en madera-fibra natural, tamaño 70 X 12 X 53 cm.'),
    ('FIGURA PECES MADERA-HIERRO DECORACIÓN 56 X 7 X 31 CM', 99.99, 2, 'https://ixia.es/media/catalog/product/6/0/609802.jpg?width=345&height=345&image-type=small_image&width=345&t=1739748866', 'Figura de peces en madera-hierro, tamaño 56 X 7 X 31 cm, decoración ideal para interiores.');




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
