CREATE DATABASE EcommerceDB;

USE EcommerceDB;

-------------------------------------------------------------------------------
-- 1) TABLA USUARIO (con EsAdmin para distinguir roles)
-------------------------------------------------------------------------------
CREATE TABLE Usuario (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    PasswordSalt NVARCHAR(255) NOT NULL,
    FechaRegistro DATETIME DEFAULT GETDATE(),
    EsAdmin BIT NOT NULL DEFAULT 0
);

-------------------------------------------------------------------------------
-- 2) INSERTAR USUARIOS DE EJEMPLO
-------------------------------------------------------------------------------
INSERT INTO Usuario (Nombre, Email, PasswordHash, PasswordSalt, FechaRegistro, EsAdmin)
VALUES
    ('Admin Local', 'admin@correo.com', 'INSERT-HASH-BASE64', 'INSERT-SALT-BASE64', GETDATE(), 1),
    ('Usuario Normal', 'user@correo.com', 'INSERT-HASH-BASE64', 'INSERT-SALT-BASE64', GETDATE(), 0);

-------------------------------------------------------------------------------
-- Consulta para verificar el login (selecciona todos los campos, incluido EsAdmin)
-------------------------------------------------------------------------------
SELECT Id, Nombre, Email, PasswordHash, PasswordSalt, FechaRegistro, EsAdmin
FROM Usuario
WHERE Email = 'admin@correo.com';

-------------------------------------------------------------------------------
-- 3) TABLA CATEGORIA
-------------------------------------------------------------------------------
CREATE TABLE Categoria (
    Id INT IDENTITY(1,1) PRIMARY KEY, 
    Nombre NVARCHAR(100) NOT NULL,  
    Descripcion NVARCHAR(500),   
    UrlImagen NVARCHAR(255) NOT NULL
);

-------------------------------------------------------------------------------
-- 4) TABLA PRODUCTO
-------------------------------------------------------------------------------
CREATE TABLE Producto (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Precio DECIMAL(18,2) NOT NULL,
    CategoriaId INT NOT NULL, 
    UrlImagen NVARCHAR(255),
    Descripcion NVARCHAR(500),
    FOREIGN KEY (CategoriaId) REFERENCES Categoria(Id)
);

-------------------------------------------------------------------------------
-- 5) INSERTAR CATEGORÍAS (Textil, Decoración vertical, Accesorio decorativo)
-------------------------------------------------------------------------------
INSERT INTO Categoria (Nombre, Descripcion, UrlImagen)
VALUES
    ('Textil', 'Productos relacionados con textiles, como sábanas, toallas, cortinas, alfombras, etc.', 'https://www.happers.es/server/Portal_0010674/img/blogposts/guia-de-textiles-para-el-hogar-como-combinar-y-cuidar_6979.jpg'),
    ('Decoración vertical', 'Productos relacionados con la decoración de paredes, como cuadros, espejos, vinilos, estanterías y otros accesorios verticales.', 'https://encrypted-tbn2.gstatic.com/shopping?q=tbn:ANd9GcSOEg6bKvWq-MeKNu9xtQrQ1paV9trxyCOA6wrljq8ltSXAe0R1g7579NMLEeopASW0OqnpLwdHk5lyaRdjHmd5dvDE8cK_v_BaglZLow4'),
    ('Accesorio decorativo', 'Productos que mejoran la estética de los espacios, como lámparas, jarrones, cuadros, estanterías, etc.', 'https://ixia.es/media/wysiwyg/2023-03/02-salon-straight-line-2023-05-11.jpg');

-------------------------------------------------------------------------------
-- 6) INSERTAR PRODUCTOS para la categoría 'Textil'
-------------------------------------------------------------------------------
INSERT INTO Producto (Nombre, Precio, CategoriaId, UrlImagen, Descripcion)
VALUES
    ('ALFOMBRA MULTICOLOR LANA-ALGODÓN 160 X 230 CM', 199.99, 1, 'https://ixia.es/media/catalog/product/6/1/614920.jpg?width=345&height=345&image-type=small_image&width=345&t=1739749309', 'Alfombra multicolor de lana y algodón, tamaño 160 x 230 cm. Ideal para espacios de estilo moderno.'),
    ('ALFOMBRA BLANCO NATURAL LANA-ALGODÓN 200 X 300 CM', 249.99, 1, 'https://ixia.es/media/catalog/product/6/1/614918.jpg?width=345&height=345&image-type=small_image&width=345&t=1739749309', 'Alfombra de color blanco natural, fabricada en lana y algodón, tamaño 200 x 300 cm. Perfecta para salas de estar elegantes.'),
    ('ALFOMBRA MARRÓN POLIPROPILENO 160 X 230 CM', 129.99, 1, 'https://ixia.es/media/catalog/product/6/1/612752.jpg?width=345&height=345&image-type=small_image&width=345&t=1739747541', 'Alfombra marrón de polipropileno, tamaño 160 x 230 cm. Resistente y fácil de mantener.'),
    ('ALFOMBRA GRIS POLIÉSTER-ALGODÓN 80 X 150 CM', 59.99, 1, 'https://ixia.es/media/catalog/product/6/0/609529.jpg?width=345&height=345&image-type=small_image&width=345&t=1739748853', 'Alfombra gris de poliéster y algodón, tamaño 80 x 150 cm. Ideal para habitaciones y oficinas.'),
    ('ALFOMBRA NATURAL YUTE DECORACIÓN 180 X 180 X 1 CM', 119.99, 1, 'https://ixia.es/media/catalog/product/1/0/106830.jpg?quality=80&bg-color=255,255,255&fit=bounds&height=700&width=700&canvas=700:700', 'Alfombra natural de yute para decoración, tamaño 180 x 180 x 1 cm. Perfecta para interiores rústicos.'),
    ('ALFOMBRA CONEJO GRIS ALGODÓN INFANTIL 100 CM', 39.99, 1, 'https://ixia.es/media/catalog/product/6/0/608553.jpg?quality=80&bg-color=255,255,255&fit=bounds&height=700&width=700&canvas=700:700', 'Alfombra infantil con diseño de conejo, hecha de algodón, diámetro de 100 cm. Ideal para niños.'),
    ('ALFOMBRA OSO MARRÓN ALGODÓN INFANTIL 100 CM', 39.99, 1, 'https://ixia.es/media/catalog/product/6/0/608552.jpg?quality=80&bg-color=255,255,255&fit=bounds&height=700&width=700&canvas=700:700', 'Alfombra infantil de algodón con diseño de oso marrón, diámetro de 100 cm. Comodidad y diversión para los pequeños.'),
    ('COJÍN VERDE ALGODÓN-POLIÉSTER 50 X 30 CM', 19.99, 1, 'https://ixia.es/media/catalog/product/6/1/614934.jpg?quality=80&bg-color=255,255,255&fit=bounds&height=700&width=700&canvas=700:700', 'Cojín verde de algodón y poliéster, tamaño 50 x 30 cm. Aporta frescura a tu hogar.'),
    ('COJÍN TIERRA ALGODÓN-POLIÉSTER 50 X 30 CM', 19.99, 1, 'https://ixia.es/media/catalog/product/6/1/614935.jpg?quality=80&bg-color=255,255,255&fit=bounds&height=700&width=700&canvas=700:700', 'Cojín en tonos tierra, hecho de algodón y poliéster, tamaño 50 x 30 cm. Perfecto para el sofá.'),
    ('COJÍN BLANCO ALGODÓN-POLIÉSTER 50 X 30 CM', 19.99, 1, 'https://ixia.es/media/catalog/product/6/1/614933.jpg?quality=80&bg-color=255,255,255&fit=bounds&height=700&width=700&canvas=700:700', 'Cojín blanco de algodón y poliéster, tamaño 50 x 30 cm. Simple y elegante para cualquier espacio.'),
    ('COJÍN CREMA JACQUARD DECORACIÓN 45 X 45 CM', 24.99, 1, 'https://ixia.es/media/catalog/product/6/1/615076.jpg?quality=80&bg-color=255,255,255&fit=bounds&height=700&width=700&canvas=700:700', 'Cojín jacquard crema para decoración, tamaño 45 x 45 cm. Aporta un toque elegante a tu hogar.'),
    ('COJÍN BLANCO/BEIGE JACQUARD DECORACIÓN 45 X 45 CM', 24.99, 1, 'https://ixia.es/media/catalog/product/6/1/615080.jpg?quality=80&bg-color=255,255,255&fit=bounds&height=700&width=700&canvas=700:700', 'Cojín decorativo blanco/beige jacquard, tamaño 45 x 45 cm. Combina con cualquier estilo de decoración.'),
    ('COJÍN BLANCO-VERDE JACQUARD DECORACIÓN 45 X 45 CM', 24.99, 1, 'https://ixia.es/media/catalog/product/6/1/615072.jpg?quality=80&bg-color=255,255,255&fit=bounds&height=700&width=700&canvas=700:700', 'Cojín decorativo blanco-verde jacquard, tamaño 45 x 45 cm. Ideal para resaltar tu espacio.');

-------------------------------------------------------------------------------
-- 7) INSERTAR PRODUCTOS para la categoría 'Decoración vertical'
-------------------------------------------------------------------------------
INSERT INTO Producto (Nombre, Precio, CategoriaId, UrlImagen, Descripcion)
VALUES
    ('CUADRO PINTURA ABSTRACTO LIENZO 60 X 150 CM', 159.99, 2, 'https://ixia.es/media/catalog/product/6/1/615166.jpg?width=345&height=345&image-type=small_image&width=345&t=1739749315', 'Cuadro de pintura abstracto en lienzo, tamaño 60 x 150 cm. Ideal para añadir un toque moderno a tu espacio.'),
    ('CUADRO PINTURA ABSTRACTO 44 X 50 CM', 89.99, 2, 'https://ixia.es/media/catalog/product/6/1/615155.jpg?width=345&height=345&image-type=small_image&width=345&t=1739749314', 'Cuadro de pintura abstracto, tamaño 44 x 50 cm. Una pieza elegante y simplista para tu hogar.'),
    ('CUADRO PINTURA ABSTRACTO 60 X 80 CM', 129.99, 2, 'https://ixia.es/media/catalog/product/6/1/615159.jpg?width=345&height=345&image-type=small_image&width=345&t=1739749315', 'Cuadro de pintura abstracto, tamaño 60 x 80 cm. Añade color y estilo a tu espacio.'),
    ('CUADRO PINTURA ABSTRACTO 70 X 70 CM', 149.99, 2, 'https://ixia.es/media/catalog/product/6/1/615162.jpg?width=345&height=345&image-type=small_image&width=345&t=1739749315', 'Cuadro de pintura abstracto, tamaño 70 x 70 cm. Una pieza impactante para cualquier habitación.'),
    ('PINTURA ABSTRACTO LIENZO DECORACIÓN 120 X 150 CM', 179.99, 2, 'https://ixia.es/media/catalog/product/6/1/615168.jpg?width=345&height=345&image-type=small_image&width=345&t=1739749315', 'Pintura abstracta en lienzo, tamaño 120 x 150 cm. Una pieza artística para decorar tu pared.'),
    ('CUADRO PINTURA ABSTRACTO 52 X 62 CM', 99.99, 2, 'https://ixia.es/media/catalog/product/6/1/615144.jpg?width=345&height=345&image-type=small_image&width=345&t=1739749314', 'Cuadro de pintura abstracto, tamaño 52 x 62 cm. Una pieza moderna para tu decoración.'),
    ('PINTURA ABSTRACTO LIENZO DECORACIÓN 120 X 3 X 60 CM', 189.99, 2, 'https://ixia.es/media/catalog/product/6/1/614404.jpg?width=345&height=345&image-type=small_image&width=345&t=1739749297', 'Pintura abstracta en lienzo, tamaño 120 x 3 x 60 cm. Ideal para dar una nueva perspectiva a tu hogar.'),
    ('PINTURA ABSTRACTO LIENZO DECORACIÓN 50 X 120 CM', 139.99, 2, 'https://ixia.es/media/catalog/product/6/1/614411.jpg?width=345&height=345&image-type=small_image&width=345&t=1739749297', 'Pintura abstracta en lienzo, tamaño 50 x 120 cm. Una pieza decorativa y estilosa para tu hogar.'),
    ('CUADRO PINTURA ABSTRACTO 50 X 4 X 50 CM', 109.99, 2, 'https://ixia.es/media/catalog/product/6/1/612473.jpg?width=345&height=345&image-type=small_image&width=345&t=1739747528', 'Cuadro de pintura abstracto, tamaño 50 x 4 x 50 cm. Una combinación perfecta de arte y diseño.'),
    ('PINTURA ABSTRACTO LIENZO DECORACIÓN 100 X 100 CM', 169.99, 2, 'https://ixia.es/media/catalog/product/6/1/614413.jpg?width=345&height=345&image-type=small_image&width=345&t=1739749297', 'Pintura abstracta en lienzo, tamaño 100 x 100 cm. Una pieza de impacto para tu hogar.'),
    ('PINTURA MUJER LIENZO DECORACIÓN 80 X 3,50 X 80 CM', 199.99, 2, 'https://ixia.es/media/catalog/product/6/1/610822.jpg?width=345&height=345&image-type=small_image&width=345&t=1739747225', 'Pintura de mujer en lienzo, tamaño 80 x 3,50 x 80 cm. Una pieza única y elegante.'),
    ('PINTURA AFRICANA LIENZO 90 X 3,50 X 120 CM', 219.99, 2, 'https://ixia.es/media/catalog/product/6/1/613361.jpg?width=345&height=345&image-type=small_image&width=345&t=1739749252', 'Pintura africana en lienzo, tamaño 90 x 3,50 x 120 cm. Una pieza artística que aporta personalidad a tu espacio.'),
    ('ESPEJO DM-CRISTAL DECORACIÓN 45 X 1 X 45 CM', 89.99, 2, 'https://ixia.es/media/catalog/product/6/0/608790.jpg?width=345&height=345&image-type=small_image&width=345&t=1739748561', 'Espejo decorativo en DM y cristal, tamaño 45 x 1 x 45 cm. Un toque elegante para cualquier habitación.'),
    ('ESPEJO NEGRO METAL-CRISTAL DECORACIÓN 82 X 7 X 113 CM', 349.99, 2, 'https://ixia.es/media/catalog/product/6/1/613600.jpg?width=345&height=345&image-type=small_image&width=345&t=1739749264', 'Espejo de metal negro y cristal, tamaño 82 x 7 x 113 cm. Ideal para un ambiente moderno y elegante.'),
    ('ESPEJO MARCO DORADO MADERA DE PINO 65 X 65 CM', 149.99, 2, 'https://ixia.es/media/catalog/product/6/1/610510.jpg?width=345&height=345&image-type=small_image&width=345&t=1739747213', 'Espejo con marco dorado de madera de pino, tamaño 65 x 65 cm. Aporta lujo y calidez a tu hogar.');

-------------------------------------------------------------------------------
-- 8) INSERTAR PRODUCTOS para la categoría 'Accesorio decorativo'
-------------------------------------------------------------------------------
INSERT INTO Producto (Nombre, Precio, CategoriaId, UrlImagen, Descripcion)
VALUES
    ('FIGURA ABSTRACTA POLIRESINA DECORACIÓN 27,50 X 14,50 X 29 CM', 49.99, 3, 'https://ixia.es/media/catalog/product/6/1/613483.jpg?width=345&height=345&image-type=small_image&width=345&t=1739749255', 'Figura abstracta en poliresina, tamaño 27,50 X 14,50 X 29 cm. Ideal para decoración de interiores.'),
    ('FIGURA CÍRCULO BLANCO-ORO RESINA 45 X 10 X 57 CM', 79.99, 3, 'https://ixia.es/media/catalog/product/6/1/610484.jpg?width=345&height=345&image-type=small_image&width=345&t=1739747212', 'Figura en resina de círculo blanco y oro, tamaño 45 X 10 X 57 cm. Perfecta para decorar cualquier habitación.'),
    ('ESCULTURA ANILLO NATURAL-BLANCO 25,50 X 9,50 X 37 CM', 89.99, 3, 'https://ixia.es/media/catalog/product/6/0/609776.jpg?width=345&height=345&image-type=small_image&width=345&t=1739748865', 'Escultura de anillo natural con detalles en blanco, tamaño 25,50 X 9,50 X 37 cm.'),
    ('FIGURA NATURAL-NEGRO MADERA-HIERRO 17 X 17 X 31 CM', 59.99, 3, 'https://ixia.es/media/catalog/product/6/0/606256.jpg?width=345&height=345&image-type=small_image&width=345&t=1739750708', 'Figura natural en madera y hierro negro, tamaño 17 X 17 X 31 cm. Elegante y moderna.'),
    ('FIGURA ABSTRACTA NATURAL MADERA DE MANGO 15 X 9 X 68,50 CM', 129.99, 3, 'https://ixia.es/media/catalog/product/6/0/609768.jpg?width=345&height=345&image-type=small_image&width=345&t=1739748865', 'Figura abstracta en madera de mango natural, medidas 15 X 9 X 68,50 cm.'),
    ('ESCULTURA NATURAL MADERA DE MANGO 38 X 8 X 52 CM', 149.99, 3, 'https://ixia.es/media/catalog/product/6/0/609762.jpg?width=345&height=345&image-type=small_image&width=345&t=1739748864', 'Escultura elegante de madera de mango, tamaño 38 X 8 X 52 cm.'),
    ('FIGURA AFRICANO NATURAL MADERA DE MANGO 14 X 14 X 88,50 CM', 199.99, 3, 'https://ixia.es/media/catalog/product/6/0/609765.jpg?width=345&height=345&image-type=small_image&width=345&t=1739748865', 'Figura africana en madera de mango natural, medidas 14 X 14 X 88,50 cm.'),
    ('CAJA DORADO METAL DECORACIÓN 31 X 21,50 X 35,50 CM', 69.99, 3, 'https://ixia.es/media/catalog/product/6/1/613122.jpg?width=345&height=345&image-type=small_image&width=345&t=1739749242', 'Caja decorativa dorada en metal, tamaño 31 X 21,50 X 35,50 cm.'),
    ('FIGURA MUJER COBRE RESINA DECORACIÓN 18 X 16 X 63 CM', 79.99, 3, 'https://ixia.es/media/catalog/product/6/1/610476.jpg?width=345&height=345&image-type=small_image&width=345&t=1739747212', 'Figura de mujer en resina cobre para decoración, medidas 18 X 16 X 63 cm.'),
    ('ESCULTURA PERSONAS COBRE RESINA 40 X 10,50 X 34 CM', 99.99, 3, 'https://ixia.es/media/catalog/product/6/1/610487.jpg?width=345&height=345&image-type=small_image&width=345&t=1739747212', 'Escultura de personas en resina cobre, medidas 40 X 10,50 X 34 cm.'),
    ('FIGURA PAVO REAL ORO METAL DECORACIÓN 50 X 30 X 85 CM', 129.99, 3, 'https://ixia.es/media/catalog/product/6/1/610280.jpg?width=345&height=345&image-type=small_image&width=345&t=1739748881', 'Figura de pavo real en metal dorado para decoración, tamaño 50 X 30 X 85 cm.'),
    ('FIGURA FLAMENCO ORO METAL DECORACIÓN 25 X 21 X 85 CM', 109.99, 3, 'https://ixia.es/media/catalog/product/6/1/610281.jpg?width=345&height=345&image-type=small_image&width=345&t=1739748881', 'Figura de flamenco en metal dorado para decoración, tamaño 25 X 21 X 85 cm.'),
    ('FIGURA ÁRBOL ORO METAL DECORACIÓN 26 X 26 X 83 CM', 119.99, 3, 'https://ixia.es/media/catalog/product/6/1/613945.jpg?width=345&height=345&image-type=small_image&width=345&t=1739749278', 'Figura de árbol en metal dorado para decoración, tamaño 26 X 26 X 83 cm.'),
    ('CORONA BLANCO MADERA PAULONIA DECORACIÓN 52 X 15 X 52 CM', 89.99, 3, 'https://ixia.es/media/catalog/product/6/1/610781.jpg?width=345&height=345&image-type=small_image&width=345&t=1739747223', 'Corona decorativa en madera paulownia blanca, medidas 52 X 15 X 52 cm.'),
    ('FIGURA ÁNGEL NATURAL MADERA DE MANGO 22,50 X 9 X 84,50 CM', 159.99, 3, 'https://ixia.es/media/catalog/product/6/0/609758.jpg?width=345&height=345&image-type=small_image&width=345&t=1739748864', 'Figura de ángel en madera de mango natural para decoración, tamaño 22,50 X 9 X 84,50 cm.'),
    ('FIGURA GALLINA BLANCO ROZADO METAL 34 X 12 X 38 CM', 49.99, 3, 'https://ixia.es/media/catalog/product/6/1/613871.jpg?width=345&height=345&image-type=small_image&width=345&t=1739749275', 'Figura de gallina en metal blanco rozado, tamaño 34 X 12 X 38 cm.'),
    ('FIGURA PEZ POLIRESINA DECORACIÓN 25,70 X 14 X 35,70 CM', 69.99, 3, 'https://ixia.es/media/catalog/product/6/1/613467.jpg?width=345&height=345&image-type=small_image&width=345&t=1739749254', 'Figura de pez en poliresina para decoración, tamaño 25,70 X 14 X 35,70 cm.'),
    ('FIGURA PEZ MADERA-FIBRA NATURAL 70 X 12 X 53 CM', 89.99, 3, 'https://ixia.es/media/catalog/product/6/0/609795.jpg?width=345&height=345&image-type=small_image&width=345&t=1739748866', 'Figura de pez en madera-fibra natural, tamaño 70 X 12 X 53 cm.'),
    ('FIGURA PECES MADERA-HIERRO DECORACIÓN 56 X 7 X 31 CM', 99.99, 3, 'https://ixia.es/media/catalog/product/6/0/609802.jpg?width=345&height=345&image-type=small_image&width=345&t=1739748866', 'Figura de peces en madera-hierro, tamaño 56 X 7 X 31 cm, decoración ideal para interiores.');

-------------------------------------------------------------------------------
-- 9) TABLA PEDIDO
-------------------------------------------------------------------------------
CREATE TABLE Pedido (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UsuarioId INT NOT NULL,
    FechaPedido DATETIME DEFAULT GETDATE(),
    Total DECIMAL(18,2) NOT NULL
);

-------------------------------------------------------------------------------
-- 10) TABLA DETALLEPEDIDO
-------------------------------------------------------------------------------
CREATE TABLE DetallePedido (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PedidoId INT NOT NULL,
    ProductoId INT NOT NULL,
    Cantidad INT NOT NULL,
    PrecioUnitario DECIMAL(18,2) NOT NULL
);

-------------------------------------------------------------------------------
-- 11) TABLA RESEÑA
-------------------------------------------------------------------------------
CREATE TABLE Reseña (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProductoId INT NOT NULL,
    UsuarioId INT NOT NULL,
    Comentario NVARCHAR(1000),
    Calificacion INT CHECK (Calificacion BETWEEN 1 AND 5)
);

-------------------------------------------------------------------------------
-- 12) INSERTS EN PEDIDO, DETALLEPEDIDO, RESEÑA
-------------------------------------------------------------------------------

-- INSERT INTO Pedido (UsuarioId, Total) VALUES (1, 799.99);
-- INSERT INTO DetallePedido (PedidoId, ProductoId, Cantidad, PrecioUnitario) VALUES (1, 1, 1, 799.99);
-- INSERT INTO Reseña (ProductoId, UsuarioId, Comentario, Calificacion) VALUES (1, 1, 'Muy bueno', 5);
