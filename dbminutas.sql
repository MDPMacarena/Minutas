-- MySQL dump 10.13  Distrib 8.0.34, for Win64 (x86_64)
--
-- Host: localhost    Database: dbminutas
-- ------------------------------------------------------
-- Server version	8.1.0

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `departamento`
--

DROP TABLE IF EXISTS `departamento`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `departamento` (
  `id` int NOT NULL AUTO_INCREMENT,
  `nombre` varchar(100) NOT NULL,
  `idJefe` int DEFAULT NULL,
  `idDeptSuperior` int DEFAULT NULL,
  `activo` tinyint(1) NOT NULL DEFAULT '1',
  PRIMARY KEY (`id`),
  KEY `idDeptSuperior` (`idDeptSuperior`),
  KEY `fkDepartamento_Usuario_idx` (`idJefe`),
  CONSTRAINT `departamento_ibfk_1` FOREIGN KEY (`idDeptSuperior`) REFERENCES `departamento` (`id`) ON DELETE SET NULL,
  CONSTRAINT `departamento_usfk` FOREIGN KEY (`idJefe`) REFERENCES `usuarios` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=32 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `departamento`
--

LOCK TABLES `departamento` WRITE;
/*!40000 ALTER TABLE `departamento` DISABLE KEYS */;
INSERT INTO `departamento` VALUES (1,'Dirección General',1,1,1),(2,'Dirección Académica',2,1,1),(3,'Dirección de Planeación y Vinculación',3,1,1),(4,'Subdirección Académica',NULL,2,1),(5,'Subdirección de Posgrado e Investigación',5,2,1),(6,'Subdirección de Vinculación',6,3,1),(7,'Subdirección de Planeación',7,3,1),(8,'Subdirección de Servicios Administrativos',8,3,1),(9,'División de Ing.Mecatrónica',9,4,1),(10,'División de Ing.Electromecánica',10,4,1),(11,'División de Ing.Industrial',11,4,1),(12,'División de Ing. en Sistemas Computacionales',12,4,1),(13,'División de Ing.Petrolera/Quimica',13,4,1),(14,'División de Ing. en Administración',14,4,1),(15,'División de Educación a Distancia y Mixta / Animación Digital',15,4,1),(16,'División de Contraloría Académica  e Innovación',16,4,1),(17,'Depto. de Ciencias Básicas  y Tecnologías de la Informacion ',17,4,1),(18,'Depto. de Control Escolar y Servicios Estudiantiles',18,5,1),(19,'Depto. de Desarrollo Académico',19,5,1),(20,'Depto. de Posgrado ',20,5,1),(21,'Depto. de Formación Integral y Tutorías',21,5,1),(22,'Depto. de Vinculación y Actividades Extraescolares',22,6,1),(23,'Depto. de Epto. de Comunicación y Concertación',23,6,1),(24,'Depto. de Estadísitica,Evaluación y Control Interno',24,7,1),(25,'Depto. de Planeación  de la Calidad',25,7,1),(26,'Depto. de Personal',26,8,1),(27,'Depto de Recursos Financieros ',27,8,1),(28,'Depto. de Recursos Materiales y Servicios',28,8,1),(31,'VideoJuegosdfgdf',27,2,1);
/*!40000 ALTER TABLE `departamento` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `minuta_usuario`
--

DROP TABLE IF EXISTS `minuta_usuario`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `minuta_usuario` (
  `id` int NOT NULL AUTO_INCREMENT,
  `idUsuario` int NOT NULL,
  `firmado` tinyint(1) DEFAULT '0',
  `idMinuta` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `idUsuario` (`idUsuario`),
  KEY `idMinuta` (`idMinuta`),
  CONSTRAINT `minuta_usuario_ibfk_1` FOREIGN KEY (`idUsuario`) REFERENCES `usuarios` (`id`) ON DELETE CASCADE,
  CONSTRAINT `minuta_usuario_ibfk_2` FOREIGN KEY (`idMinuta`) REFERENCES `minutas` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `minuta_usuario`
--

LOCK TABLES `minuta_usuario` WRITE;
/*!40000 ALTER TABLE `minuta_usuario` DISABLE KEYS */;
/*!40000 ALTER TABLE `minuta_usuario` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `minutas`
--

DROP TABLE IF EXISTS `minutas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `minutas` (
  `id` int NOT NULL AUTO_INCREMENT,
  `idCreador` int NOT NULL,
  `fechaCreacion` date NOT NULL,
  `estado` enum('PorFirmar','Firmada','Borrador') NOT NULL,
  `idDepartamento` int NOT NULL,
  `objetivo` json NOT NULL,
  `ordenDia` json NOT NULL,
  `desarrollo` json NOT NULL,
  `compromisosYtareas` json NOT NULL,
  `privadas` tinyint DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `idCreador` (`idCreador`),
  KEY `idDepartamento` (`idDepartamento`),
  CONSTRAINT `minutas_ibfk_1` FOREIGN KEY (`idCreador`) REFERENCES `usuarios` (`id`) ON DELETE CASCADE,
  CONSTRAINT `minutas_ibfk_2` FOREIGN KEY (`idDepartamento`) REFERENCES `departamento` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `minutas`
--

LOCK TABLES `minutas` WRITE;
/*!40000 ALTER TABLE `minutas` DISABLE KEYS */;
/*!40000 ALTER TABLE `minutas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `roles`
--

DROP TABLE IF EXISTS `roles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `roles` (
  `id` int NOT NULL AUTO_INCREMENT,
  `nombre` varchar(45) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `roles`
--

LOCK TABLES `roles` WRITE;
/*!40000 ALTER TABLE `roles` DISABLE KEYS */;
INSERT INTO `roles` VALUES (1,'administrador'),(2,'jefe'),(3,'usuario');
/*!40000 ALTER TABLE `roles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `usuarios`
--

DROP TABLE IF EXISTS `usuarios`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `usuarios` (
  `id` int NOT NULL AUTO_INCREMENT,
  `idDepartamento` int NOT NULL,
  `activo` tinyint(1) NOT NULL DEFAULT '1',
  `numEmpleado` varchar(20) NOT NULL,
  `nombre` varchar(100) NOT NULL,
  `correo` varchar(100) NOT NULL,
  `contraseña_hash` varchar(255) NOT NULL,
  `fechaNacimiento` date NOT NULL,
  `idRol` int NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `numEmpleado_UNIQUE` (`numEmpleado`),
  UNIQUE KEY `correo_UNIQUE` (`correo`),
  KEY `idx_correo` (`correo`),
  KEY `idDepartamento` (`idDepartamento`),
  KEY `idRol` (`idRol`),
  CONSTRAINT `usuarios_ibfk_1` FOREIGN KEY (`idDepartamento`) REFERENCES `departamento` (`id`) ON DELETE CASCADE,
  CONSTRAINT `usuarios_ibfk_2` FOREIGN KEY (`idRol`) REFERENCES `roles` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=63 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usuarios`
--

LOCK TABLES `usuarios` WRITE;
/*!40000 ALTER TABLE `usuarios` DISABLE KEYS */;
INSERT INTO `usuarios` VALUES (1,1,1,'TEMP001','Luis Carlos Longares Vidal','temp001@example.com','default','2000-01-01',2),(2,2,1,'TEMP002','Etel Hernández Alemán','temp002@example.com','default','2000-01-01',2),(3,3,1,'TEMP003','Sarilu Cárdenas Rodríguez','temp003@example.com','default','2000-01-01',2),(5,5,1,'TEMP005','Elsa Georgina de León Trejo','temp005@example.com','default','2000-01-01',2),(6,6,1,'TEMP006','Eva Guadalupe Said Fernández','temp006@example.com','default','2000-01-01',2),(7,7,1,'TEMP007','Ing. Julio César Montenegro Hernández','temp007@example.com','default','2000-01-01',2),(8,8,1,'TEMP008','Elpidio Pérez Valdés','temp008@example.com','default','2000-01-01',2),(9,9,1,'TEMP009','Adrián Alberto Trujillo Becerra','temp009@example.com','default','2000-01-01',2),(10,10,1,'TEMP010','Juan Francisco Vélez Palacios','temp010@example.com','default','2000-01-01',2),(11,11,1,'TEMP011','Néstor Zamarippa Belmares','temp011@example.com','default','2000-01-01',2),(12,12,1,'TEMP012','Oscar Raúl Sánchez Flores','temp012@example.com','default','2000-01-01',2),(13,13,1,'TEMP013','Claudia Sánchez Ibarra','temp013@example.com','default','2000-01-01',2),(14,14,1,'TEMP014','Erick Gamaliel Martínez','temp014@example.com','default','2000-01-01',2),(15,15,1,'TEMP015','Ma. Enriqueta Tapia Herrera','temp015@example.com','default','2000-01-01',2),(16,16,1,'TEMP016','Evaristo Valdez Segovia','temp016@example.com','default','2000-01-01',2),(17,17,1,'TEMP017','Alejandro Gómez Hernández','temp017@example.com','default','2000-01-01',2),(18,18,1,'TEMP018','Noelia Castillo Villanueva','temp018@example.com','default','2000-01-01',2),(19,19,1,'TEMP019','Maricela Bernal García','temp019@example.com','default','2000-01-01',2),(20,20,1,'TEMP020','Abelauro Belmont Duque','temp020@example.com','default','2000-01-01',2),(21,21,1,'TEMP021','Rosa María Arizpe Flores','temp021@example.com','default','2000-01-01',2),(22,22,1,'TEMP022','Ubaldo Macías Rangel','temp022@example.com','default','2000-01-01',2),(23,23,1,'TEMP023','Mtro. Hugo Ríos Herrera','temp023@example.com','default','2000-01-01',2),(24,24,1,'TEMP024','María Isabel Cabrera Flores','temp024@example.com','default','2000-01-01',2),(25,25,1,'TEMP025','Ana Josefina Morales Cruz','temp025@example.com','default','2000-01-01',2),(26,26,1,'TEMP026','Marisa Escobedo Muñoz','temp026@example.com','default','2000-01-01',2),(27,27,1,'TEMP027','Gisela de León Salinas','temp027@example.com','default','2000-01-01',2),(28,28,1,'TEMP028','Roberto Tamez Zamora','temp028@example.com','default','2000-01-01',2),(61,16,1,'4040555','juanefewdfbfbfd','juan2@casa.com','REUNIONES','2025-05-05',3);
/*!40000 ALTER TABLE `usuarios` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'dbminutas'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-05-06 17:11:41
