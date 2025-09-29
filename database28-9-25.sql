-- MySQL dump 10.13  Distrib 8.0.42, for Win64 (x86_64)
--
-- Host: localhost    Database: medicine
-- ------------------------------------------------------
-- Server version	8.0.42

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
-- Table structure for table `batch_items`
--

DROP TABLE IF EXISTS `batch_items`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `batch_items` (
  `batch_item_id` int NOT NULL AUTO_INCREMENT,
  `purchase_batch_id` int NOT NULL,
  `product_id` int NOT NULL,
  `purchase_price` int NOT NULL,
  `quantity_received` int NOT NULL,
  `expiry_date` date NOT NULL,
  `quantity_remaining` int NOT NULL DEFAULT '0',
  `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`batch_item_id`),
  KEY `purchase_batch_id_idx` (`purchase_batch_id`),
  KEY `idx_expiry` (`expiry_date`),
  KEY `product_id_idx` (`product_id`),
  KEY `idx_product_remaining` (`product_id`,`quantity_remaining`),
  CONSTRAINT `fk_batch_product` FOREIGN KEY (`product_id`) REFERENCES `medicines` (`product_id`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `fk_batch_purchase` FOREIGN KEY (`purchase_batch_id`) REFERENCES `purchase_batches` (`purchase_batch_id`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `chk_purchase_price` CHECK ((`purchase_price` > 0)),
  CONSTRAINT `chk_quantity_received` CHECK ((`quantity_received` > 0)),
  CONSTRAINT `chk_quantity_remaining` CHECK ((`quantity_remaining` >= 0)),
  CONSTRAINT `chk_remaining_vs_received` CHECK ((`quantity_remaining` <= `quantity_received`))
) ENGINE=InnoDB AUTO_INCREMENT=71 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `batch_items`
--

LOCK TABLES `batch_items` WRITE;
/*!40000 ALTER TABLE `batch_items` DISABLE KEYS */;
INSERT INTO `batch_items` VALUES (55,31,71,100,500,'2026-12-31',495,'2025-09-22 17:35:11'),(56,31,71,150,300,'2026-11-30',298,'2025-09-22 17:35:11'),(57,32,72,800,100,'2026-10-31',99,'2025-09-22 17:35:11'),(58,32,72,600,120,'2026-09-30',79,'2025-09-22 17:35:11'),(59,33,73,30,1000,'2026-08-31',994,'2025-09-22 17:35:11'),(60,34,74,120,400,'2026-07-31',400,'2025-09-22 17:35:11'),(61,35,65,200,250,'2026-06-30',247,'2025-09-22 17:35:11'),(62,36,65,120,350,'2026-05-31',348,'2025-09-22 17:35:11'),(63,37,67,60,500,'2026-04-30',500,'2025-09-22 17:35:11'),(64,38,68,180,280,'2026-03-31',280,'2025-09-22 17:35:11'),(65,39,69,280,200,'2026-02-28',130,'2025-09-22 17:35:11'),(66,30,70,450,150,'2026-01-31',150,'2025-09-22 17:35:11'),(67,31,71,95,400,'2027-01-31',382,'2025-09-22 17:35:11'),(68,32,72,780,80,'2027-02-28',80,'2025-09-22 17:35:11'),(69,40,69,120,3,'2025-09-24',3,'2025-09-22 19:14:38'),(70,40,65,200,12,'2026-03-22',5,'2025-09-22 19:14:59');
/*!40000 ALTER TABLE `batch_items` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `categories`
--

DROP TABLE IF EXISTS `categories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `categories` (
  `category_id` int NOT NULL AUTO_INCREMENT,
  `category_name` varchar(100) NOT NULL,
  PRIMARY KEY (`category_id`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `categories`
--

LOCK TABLES `categories` WRITE;
/*!40000 ALTER TABLE `categories` DISABLE KEYS */;
INSERT INTO `categories` VALUES (5,'Antibiotic'),(6,'Vaccine'),(7,'Painkiller'),(8,'Vitamin'),(9,'Antifungal'),(10,'Antiparasitic'),(11,'Hormone'),(12,'Disinfectant'),(13,'Supplement'),(14,'Diagnostic'),(15,'antibiotic');
/*!40000 ALTER TABLE `categories` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `company`
--

DROP TABLE IF EXISTS `company`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `company` (
  `company_id` int NOT NULL AUTO_INCREMENT,
  `company_name` varchar(100) NOT NULL,
  `contact` varchar(20) NOT NULL,
  `address` varchar(200) NOT NULL,
  PRIMARY KEY (`company_id`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `company`
--

LOCK TABLES `company` WRITE;
/*!40000 ALTER TABLE `company` DISABLE KEYS */;
INSERT INTO `company` VALUES (4,'Pfizer Vet','0300-1234567','Lahore, Pakistan'),(5,'Searle Pharma','0321-9876543','Karachi, Pakistan'),(6,'Getz Veterinary','0312-5558888','Islamabad, Pakistan'),(7,'Novartis Animal Health','0301-4445555','Rawalpindi, Pakistan'),(8,'Merck Animal Health','0333-6667777','Faisalabad, Pakistan'),(9,'Bayer Veterinary','0345-8889999','Multan, Pakistan'),(10,'Zoetis Pakistan','0311-2223333','Peshawar, Pakistan'),(11,'Elanco','0302-7778888','Quetta, Pakistan'),(12,'Virbac Pakistan','0331-9990000','Gujranwala, Pakistan'),(13,'Ceva Animal Health','0344-1112222','Sialkot, Pakistan');
/*!40000 ALTER TABLE `company` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `customerpricerecord`
--

DROP TABLE IF EXISTS `customerpricerecord`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `customerpricerecord` (
  `record_id` int NOT NULL AUTO_INCREMENT,
  `customer_id` int NOT NULL,
  `date` date NOT NULL,
  `payment` decimal(10,2) NOT NULL,
  `remarks` varchar(255) DEFAULT NULL,
  `sale_id` int NOT NULL,
  PRIMARY KEY (`record_id`),
  KEY `customer_id` (`customer_id`),
  KEY `sale_id` (`sale_id`),
  CONSTRAINT `customerpricerecord_ibfk_1` FOREIGN KEY (`customer_id`) REFERENCES `customers` (`customer_id`),
  CONSTRAINT `customerpricerecord_ibfk_2` FOREIGN KEY (`sale_id`) REFERENCES `sales` (`sale_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=91 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `customerpricerecord`
--

LOCK TABLES `customerpricerecord` WRITE;
/*!40000 ALTER TABLE `customerpricerecord` DISABLE KEYS */;
INSERT INTO `customerpricerecord` VALUES (52,51,'2025-09-01',1500.00,'Full payment',1),(53,42,'2025-09-02',1200.00,'Partial payment',2),(54,42,'2025-09-03',1200.00,'Balance cleared',2),(55,43,'2025-09-03',1800.00,'Full payment',3),(56,44,'2025-09-04',2000.00,'Advance payment',4),(57,44,'2025-09-05',1200.00,'Final payment',4),(58,45,'2025-09-05',950.00,'Full payment',5),(59,46,'2025-09-06',2800.00,'Full payment',6),(60,47,'2025-09-07',1200.00,'Full payment',7),(61,48,'2025-09-08',4500.00,'Full payment',8),(62,49,'2025-09-09',1600.00,'Full payment',9),(63,50,'2025-09-10',1900.00,'Full payment',10),(68,1,'2025-09-24',90.00,NULL,17),(69,1,'2025-09-24',400.00,NULL,18),(70,1,'2025-09-24',300.00,NULL,19),(71,43,'2025-09-24',2000.00,NULL,20),(72,1,'2025-09-24',100.00,NULL,21),(73,46,'2025-09-24',200.00,NULL,22),(74,42,'2025-09-24',1500.00,'Payment applied to sale #1',1),(75,42,'2025-09-24',1500.00,'Payment applied to sale #2',2),(76,1,'2025-09-24',300.00,NULL,23),(77,1,'2025-09-24',810.00,NULL,24),(78,1,'2025-09-25',480.00,NULL,25),(79,1,'2025-09-26',50.00,'Payment applied to sale #21',21),(80,1,'2025-09-26',400.00,NULL,26),(81,1,'2025-09-26',450.00,NULL,27),(82,1,'2025-09-26',1020.00,NULL,28),(83,47,'2025-09-27',4000.00,'Payment applied to sale #8',8),(84,45,'2025-09-27',2000.00,'Payment applied to sale #6',6),(85,1,'2025-09-27',1270.00,NULL,29),(86,1,'2025-09-27',480.00,NULL,30),(87,1,'2025-09-27',190.00,NULL,31),(88,1,'2025-09-27',440.00,NULL,32),(90,44,'2025-09-27',20000.00,NULL,34);
/*!40000 ALTER TABLE `customerpricerecord` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `customers`
--

DROP TABLE IF EXISTS `customers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `customers` (
  `customer_id` int NOT NULL AUTO_INCREMENT,
  `full_name` varchar(100) NOT NULL,
  `phone` varchar(20) DEFAULT NULL,
  `address` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`customer_id`),
  UNIQUE KEY `full_name` (`full_name`)
) ENGINE=InnoDB AUTO_INCREMENT=56 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `customers`
--

LOCK TABLES `customers` WRITE;
/*!40000 ALTER TABLE `customers` DISABLE KEYS */;
INSERT INTO `customers` VALUES (1,'walkin','9090909090','walkin'),(42,'Ali Khan','0300-1111111','House 1, Street 1, Lahore'),(43,'Sara Ahmed','0300-2222222','House 2, Street 2, Karachi'),(44,'Usman Sheikh','0300-3333333','House 3, Street 3, Islamabad'),(45,'Fatima Malik','0300-4444444','House 4, Street 4, Faisalabad'),(46,'Bilal Hussain','0300-5555555','House 5, Street 5, Multan'),(47,'Ayesha Rehman','0300-6666666','House 6, Street 6, Rawalpindi'),(48,'Omar Farooq','0300-7777777','House 7, Street 7, Peshawar'),(49,'Zainab Akhtar','0300-8888888','House 8, Street 8, Quetta'),(50,'Hamza Iqbal','0300-9999999','House 9, Street 9, Gujranwala'),(51,'Nadia Shah','0300-0000000','House 10, Street 10, Sialkot'),(54,'zain lahoriya','03030303','lahore'),(55,'zahid latif','03030303','karachi');
/*!40000 ALTER TABLE `customers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `medicines`
--

DROP TABLE IF EXISTS `medicines`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `medicines` (
  `product_id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(100) NOT NULL,
  `description` varchar(200) DEFAULT NULL,
  `company_id` int NOT NULL,
  `packing_id` int NOT NULL,
  `category_id` int NOT NULL,
  `sale_price` int NOT NULL,
  PRIMARY KEY (`product_id`),
  UNIQUE KEY `uq_name_company` (`name`,`company_id`),
  KEY `idx_name` (`name`),
  KEY `idx_company` (`company_id`),
  KEY `category_id_idx` (`category_id`),
  KEY `packing_id_idx` (`packing_id`),
  CONSTRAINT `fk_category_id` FOREIGN KEY (`category_id`) REFERENCES `categories` (`category_id`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `fk_company_id` FOREIGN KEY (`company_id`) REFERENCES `company` (`company_id`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `fk_packing_id` FOREIGN KEY (`packing_id`) REFERENCES `packing` (`packing_id`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `chk_sale_price` CHECK ((`sale_price` > 0))
) ENGINE=InnoDB AUTO_INCREMENT=80 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `medicines`
--

LOCK TABLES `medicines` WRITE;
/*!40000 ALTER TABLE `medicines` DISABLE KEYS */;
INSERT INTO `medicines` VALUES (65,'Amoxicillin 250mg','Broad-spectrum antibiotic',5,5,7,200),(66,'Rabies Vaccine','Vaccine for rabies prevention',9,6,5,1200),(67,'Paracetamol 500mg','Pain and fever reducer',9,7,9,50),(68,'Vitamin B12 Injection','Energy booster for livestock',7,7,8,200),(69,'Fluconazole 150mg','Antifungal medication',5,9,5,300),(70,'Ivermectin Injection','Antiparasitic for animals',6,8,6,180),(71,'Dexamethasone','Anti-inflammatory hormone',7,5,7,90),(72,'Povidone Iodine','Surface disinfectant',8,9,8,250),(73,'Calcium Supplement','Bone health supplement',9,6,9,400),(74,'Pregnancy Test Kit','Animal pregnancy detection',10,10,10,600),(76,'Canine Distemper Vaccine','Dog vaccination',6,5,10,800),(77,'panadol','paracetamol',5,5,12,2),(78,'panadol extra','paracetamol extra',5,5,12,2);
/*!40000 ALTER TABLE `medicines` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `packing`
--

DROP TABLE IF EXISTS `packing`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `packing` (
  `packing_id` int NOT NULL AUTO_INCREMENT,
  `packing_name` varchar(70) NOT NULL,
  PRIMARY KEY (`packing_id`),
  UNIQUE KEY `packing_name_UNIQUE` (`packing_name`)
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `packing`
--

LOCK TABLES `packing` WRITE;
/*!40000 ALTER TABLE `packing` DISABLE KEYS */;
INSERT INTO `packing` VALUES (15,'bottle'),(6,'Capsule'),(13,'Cream'),(12,'Drop'),(7,'Injection'),(9,'Ointment'),(10,'Powder'),(16,'sach'),(17,'sache'),(18,'sachety'),(14,'Solution'),(11,'Spray'),(8,'Syrup'),(5,'Tablet');
/*!40000 ALTER TABLE `packing` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `payment_records`
--

DROP TABLE IF EXISTS `payment_records`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `payment_records` (
  `payment_id` int NOT NULL AUTO_INCREMENT,
  `company_id` int NOT NULL,
  `amount` decimal(10,2) NOT NULL,
  `payment_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `notes` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`payment_id`),
  KEY `fk_payment_company` (`company_id`),
  CONSTRAINT `fk_payment_company` FOREIGN KEY (`company_id`) REFERENCES `company` (`company_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `payment_records`
--

LOCK TABLES `payment_records` WRITE;
/*!40000 ALTER TABLE `payment_records` DISABLE KEYS */;
INSERT INTO `payment_records` VALUES (9,4,50000.00,'2025-01-15 10:00:00','Initial payment for BATCH-PFIZER-001'),(10,5,60000.00,'2025-01-20 11:30:00','Partial payment for BATCH-SEARLE-001'),(11,6,30000.00,'2025-02-01 14:15:00','Full payment for BATCH-GETZ-001'),(12,4,45000.00,'2025-02-15 09:45:00','Full payment for BATCH-NOVARTIS-001'),(13,5,40000.00,'2025-03-01 16:20:00','Partial payment for BATCH-MERCK-001'),(14,6,35000.00,'2025-03-15 13:10:00','Full payment for BATCH-BAYER-001'),(15,7,55000.00,'2025-04-01 15:30:00','Full payment for BATCH-ZOETIS-001'),(16,8,30000.00,'2025-04-15 10:45:00','Partial payment for BATCH-ELANCO-001'),(17,9,48000.00,'2025-05-01 12:00:00','Full payment for BATCH-VIRBAC-001'),(18,10,52000.00,'2025-05-15 14:50:00','Full payment for BATCH-CEVA-001'),(19,5,10000.00,'2025-09-24 16:31:06',NULL),(20,5,5000.00,'2025-09-27 00:27:05',NULL);
/*!40000 ALTER TABLE `payment_records` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `purchase_batches`
--

DROP TABLE IF EXISTS `purchase_batches`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `purchase_batches` (
  `purchase_batch_id` int NOT NULL AUTO_INCREMENT,
  `company_id` int NOT NULL,
  `purchase_date` date NOT NULL DEFAULT (curdate()),
  `total_price` decimal(10,2) NOT NULL,
  `paid` decimal(10,2) NOT NULL DEFAULT '0.00',
  `BatchName` varchar(45) NOT NULL,
  `status` enum('pending','completed','overpaid') NOT NULL DEFAULT 'pending',
  PRIMARY KEY (`purchase_batch_id`),
  UNIQUE KEY `BatchName_UNIQUE` (`BatchName`),
  KEY `company_id_idx` (`company_id`),
  CONSTRAINT `fk_purchase_batches_companies` FOREIGN KEY (`company_id`) REFERENCES `company` (`company_id`) ON DELETE RESTRICT,
  CONSTRAINT `fk_purchase_company` FOREIGN KEY (`company_id`) REFERENCES `company` (`company_id`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `chk_paid_amount` CHECK ((`paid` >= 0)),
  CONSTRAINT `chk_paid_vs_total` CHECK ((`paid` <= `total_price`)),
  CONSTRAINT `chk_total_price` CHECK ((`total_price` >= 0))
) ENGINE=InnoDB AUTO_INCREMENT=45 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `purchase_batches`
--

LOCK TABLES `purchase_batches` WRITE;
/*!40000 ALTER TABLE `purchase_batches` DISABLE KEYS */;
INSERT INTO `purchase_batches` VALUES (28,5,'2025-09-05',50000.00,50000.00,'BATCH-PFIZER-004','completed'),(29,7,'2025-01-20',75000.00,60000.00,'BATCH-SEARLE-001','pending'),(30,5,'2025-02-01',30000.00,30000.00,'BATCH-GETZ-001','completed'),(31,6,'2025-02-15',45000.00,45000.00,'BATCH-NOVARTIS-001','completed'),(32,5,'2025-03-01',60000.00,55000.00,'BATCH-MERCK-001','pending'),(33,6,'2025-03-15',35000.00,35000.00,'BATCH-BAYER-001','completed'),(34,7,'2025-04-01',55000.00,55000.00,'BATCH-ZOETIS-001','completed'),(35,8,'2025-04-15',40000.00,30000.00,'BATCH-ELANCO-001','pending'),(36,9,'2025-05-01',48000.00,48000.00,'BATCH-VIRBAC-001','completed'),(37,10,'2025-05-15',52000.00,52000.00,'BATCH-CEVA-001','completed'),(38,9,'2025-06-01',42000.00,42000.00,'BATCH-PFIZER-002','completed'),(39,9,'2025-06-15',38000.00,38000.00,'BATCH-SEARLE-002','completed'),(40,7,'2025-09-22',10000.00,5000.00,'september-25','pending'),(42,7,'2025-09-25',50000.00,0.00,'september-25-3','pending'),(44,5,'2025-09-27',20000.00,0.00,'september-25-02','pending');
/*!40000 ALTER TABLE `purchase_batches` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sale_items`
--

DROP TABLE IF EXISTS `sale_items`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sale_items` (
  `sale_item_id` int NOT NULL AUTO_INCREMENT,
  `sale_id` int NOT NULL,
  `batch_item_id` int NOT NULL,
  `quantity` int NOT NULL,
  `price` int NOT NULL,
  `discount` int DEFAULT '0',
  PRIMARY KEY (`sale_item_id`),
  KEY `idx_sale` (`sale_id`),
  KEY `fk_sale_items_product_idx` (`batch_item_id`),
  CONSTRAINT `fk_sale_items_product` FOREIGN KEY (`batch_item_id`) REFERENCES `batch_items` (`batch_item_id`) ON DELETE RESTRICT,
  CONSTRAINT `fk_sale_items_sale` FOREIGN KEY (`sale_id`) REFERENCES `sales` (`sale_id`) ON DELETE CASCADE,
  CONSTRAINT `chk_discount` CHECK (((`discount` >= 0) and (`discount` <= 100))),
  CONSTRAINT `chk_price` CHECK ((`price` > 0)),
  CONSTRAINT `chk_quantity` CHECK ((`quantity` > 0))
) ENGINE=InnoDB AUTO_INCREMENT=57 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sale_items`
--

LOCK TABLES `sale_items` WRITE;
/*!40000 ALTER TABLE `sale_items` DISABLE KEYS */;
INSERT INTO `sale_items` VALUES (15,1,61,10,150,0),(16,1,63,1,1200,5),(17,2,62,5,220,2),(18,2,64,2,800,0),(19,3,65,20,50,10),(20,4,66,5,200,0),(21,4,57,3,300,5),(22,5,58,5,180,0),(23,6,59,10,90,15),(24,7,60,4,250,0),(25,8,61,3,400,8),(26,9,62,2,600,0),(27,10,63,8,150,5),(28,11,64,1,1200,10),(29,17,55,1,90,0),(30,18,59,1,400,0),(31,19,70,2,150,0),(32,20,67,2,90,0),(33,20,59,2,400,0),(34,20,65,6,300,0),(35,21,61,1,150,0),(36,22,55,2,90,0),(37,22,70,2,150,0),(38,23,62,2,150,0),(39,24,67,9,90,0),(40,25,56,2,90,0),(41,25,65,1,300,0),(42,26,61,2,200,0),(43,27,67,5,90,0),(44,28,59,2,400,10),(45,28,55,1,90,0),(46,28,57,1,250,100),(47,29,67,2,90,0),(48,29,70,1,200,0),(49,29,65,3,300,0),(50,30,59,1,400,0),(51,30,55,1,90,0),(52,31,70,1,200,0),(53,32,70,1,200,0),(54,32,58,1,250,0),(55,34,58,40,250,0),(56,34,65,60,300,0);
/*!40000 ALTER TABLE `sale_items` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sales`
--

DROP TABLE IF EXISTS `sales`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sales` (
  `sale_id` int NOT NULL AUTO_INCREMENT,
  `customer_id` int NOT NULL,
  `sale_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `total_amount` decimal(10,2) NOT NULL,
  `paid_amount` decimal(10,2) DEFAULT '0.00',
  PRIMARY KEY (`sale_id`),
  KEY `idx_sale_date` (`sale_date`),
  KEY `customer_id_idx` (`customer_id`),
  CONSTRAINT `customer_foreign_key` FOREIGN KEY (`customer_id`) REFERENCES `customers` (`customer_id`),
  CONSTRAINT `chk_total_amount` CHECK ((`total_amount` > 0))
) ENGINE=InnoDB AUTO_INCREMENT=35 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sales`
--

LOCK TABLES `sales` WRITE;
/*!40000 ALTER TABLE `sales` DISABLE KEYS */;
INSERT INTO `sales` VALUES (1,42,'2025-09-01 10:00:00',1500.00,1500.00),(2,42,'2025-09-02 11:30:00',2400.00,1500.00),(3,43,'2025-09-03 14:15:00',1800.00,0.00),(4,44,'2025-09-04 09:45:00',3200.00,0.00),(5,44,'2025-09-05 16:20:00',950.00,0.00),(6,45,'2025-09-06 13:10:00',2800.00,2000.00),(7,46,'2025-09-07 15:30:00',1200.00,0.00),(8,47,'2025-09-08 10:45:00',4500.00,4000.00),(9,48,'2025-09-09 12:00:00',1600.00,0.00),(10,49,'2025-09-10 14:50:00',1900.00,0.00),(11,50,'2025-09-11 11:15:00',2200.00,0.00),(12,51,'2025-09-12 15:40:00',3100.00,0.00),(17,1,'2025-09-24 14:18:45',90.00,90.00),(18,1,'2025-09-24 16:01:52',400.00,400.00),(19,1,'2025-09-24 16:01:52',300.00,300.00),(20,43,'2025-09-24 16:05:46',2780.00,2000.00),(21,1,'2025-09-24 16:07:34',150.00,150.00),(22,46,'2025-09-24 16:10:24',480.00,200.00),(23,1,'2025-09-24 22:45:55',300.00,300.00),(24,1,'2025-09-24 23:13:00',810.00,810.00),(25,1,'2025-09-25 00:30:20',480.00,480.00),(26,1,'2025-09-26 20:26:47',400.00,400.00),(27,1,'2025-09-26 20:26:47',450.00,450.00),(28,1,'2025-09-26 20:26:47',1020.00,1020.00),(29,1,'2025-09-27 12:36:02',1270.00,1270.00),(30,1,'2025-09-27 12:49:04',480.00,480.00),(31,1,'2025-09-27 12:51:41',190.00,190.00),(32,1,'2025-09-27 13:09:53',440.00,440.00),(34,44,'2025-09-27 23:19:55',27000.00,20000.00);
/*!40000 ALTER TABLE `sales` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `stock_log`
--

DROP TABLE IF EXISTS `stock_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `stock_log` (
  `log_id` int NOT NULL AUTO_INCREMENT,
  `batch_id` int NOT NULL,
  `change_type` enum('PURCHASE','SALE','ADJUSTMENT','RETURN','EXPIRED') NOT NULL,
  `quantity_change` int NOT NULL,
  `log_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `remarks` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`log_id`),
  KEY `idx_log_date` (`log_date`),
  KEY `fk_stocklog_batch` (`batch_id`),
  CONSTRAINT `fk_stocklog_batch` FOREIGN KEY (`batch_id`) REFERENCES `batch_items` (`batch_item_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `stock_log`
--

LOCK TABLES `stock_log` WRITE;
/*!40000 ALTER TABLE `stock_log` DISABLE KEYS */;
/*!40000 ALTER TABLE `stock_log` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `user_id` int NOT NULL AUTO_INCREMENT,
  `username` varchar(50) NOT NULL,
  `password_hash` varchar(255) NOT NULL,
  `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`user_id`),
  UNIQUE KEY `username_UNIQUE` (`username`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'admin123','123456789','2025-09-04 09:45:00');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary view structure for view `v_current_stock`
--

DROP TABLE IF EXISTS `v_current_stock`;
/*!50001 DROP VIEW IF EXISTS `v_current_stock`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `v_current_stock` AS SELECT 
 1 AS `product_id`,
 1 AS `name`,
 1 AS `sale_price`,
 1 AS `company_name`,
 1 AS `category_name`,
 1 AS `packing_name`,
 1 AS `current_stock`,
 1 AS `active_batches`,
 1 AS `next_expiry`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `v_daily_sales`
--

DROP TABLE IF EXISTS `v_daily_sales`;
/*!50001 DROP VIEW IF EXISTS `v_daily_sales`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `v_daily_sales` AS SELECT 
 1 AS `sale_day`,
 1 AS `total_bills`,
 1 AS `total_sales`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `v_expiring_items`
--

DROP TABLE IF EXISTS `v_expiring_items`;
/*!50001 DROP VIEW IF EXISTS `v_expiring_items`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `v_expiring_items` AS SELECT 
 1 AS `name`,
 1 AS `expiry_date`,
 1 AS `quantity_remaining`,
 1 AS `purchase_price`,
 1 AS `sale_price`,
 1 AS `days_to_expiry`,
 1 AS `company_name`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `v_low_stock`
--

DROP TABLE IF EXISTS `v_low_stock`;
/*!50001 DROP VIEW IF EXISTS `v_low_stock`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `v_low_stock` AS SELECT 
 1 AS `product_id`,
 1 AS `name`,
 1 AS `current_stock`,
 1 AS `sale_price`,
 1 AS `company_name`,
 1 AS `stock_status`*/;
SET character_set_client = @saved_cs_client;

--
-- Dumping events for database 'medicine'
--

--
-- Final view structure for view `v_current_stock`
--

/*!50001 DROP VIEW IF EXISTS `v_current_stock`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb3 */;
/*!50001 SET character_set_results     = utf8mb3 */;
/*!50001 SET collation_connection      = utf8mb3_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `v_current_stock` AS select `m`.`product_id` AS `product_id`,`m`.`name` AS `name`,`m`.`sale_price` AS `sale_price`,`c`.`company_name` AS `company_name`,`cat`.`category_name` AS `category_name`,`p`.`packing_name` AS `packing_name`,coalesce(sum(`bi`.`quantity_remaining`),0) AS `current_stock`,count((case when (`bi`.`quantity_remaining` > 0) then `bi`.`batch_item_id` end)) AS `active_batches`,min((case when (`bi`.`quantity_remaining` > 0) then `bi`.`expiry_date` end)) AS `next_expiry` from ((((`medicines` `m` left join `batch_items` `bi` on((`m`.`product_id` = `bi`.`product_id`))) left join `company` `c` on((`m`.`company_id` = `c`.`company_id`))) left join `categories` `cat` on((`m`.`category_id` = `cat`.`category_id`))) left join `packing` `p` on((`m`.`packing_id` = `p`.`packing_id`))) group by `m`.`product_id`,`m`.`name`,`m`.`sale_price`,`c`.`company_name`,`cat`.`category_name`,`p`.`packing_name` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `v_daily_sales`
--

/*!50001 DROP VIEW IF EXISTS `v_daily_sales`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `v_daily_sales` AS select cast(`s`.`sale_date` as date) AS `sale_day`,count(distinct `s`.`sale_id`) AS `total_bills`,sum(`s`.`total_amount`) AS `total_sales` from `sales` `s` group by cast(`s`.`sale_date` as date) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `v_expiring_items`
--

/*!50001 DROP VIEW IF EXISTS `v_expiring_items`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb3 */;
/*!50001 SET character_set_results     = utf8mb3 */;
/*!50001 SET collation_connection      = utf8mb3_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `v_expiring_items` AS select `m`.`name` AS `name`,`bi`.`expiry_date` AS `expiry_date`,`bi`.`quantity_remaining` AS `quantity_remaining`,`bi`.`purchase_price` AS `purchase_price`,`m`.`sale_price` AS `sale_price`,(to_days(`bi`.`expiry_date`) - to_days(curdate())) AS `days_to_expiry`,`c`.`company_name` AS `company_name` from ((`batch_items` `bi` join `medicines` `m` on((`bi`.`product_id` = `m`.`product_id`))) join `company` `c` on((`m`.`company_id` = `c`.`company_id`))) where ((`bi`.`quantity_remaining` > 0) and (`bi`.`expiry_date` <= (curdate() + interval 60 day))) order by `bi`.`expiry_date` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `v_low_stock`
--

/*!50001 DROP VIEW IF EXISTS `v_low_stock`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb3 */;
/*!50001 SET character_set_results     = utf8mb3 */;
/*!50001 SET collation_connection      = utf8mb3_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `v_low_stock` AS select `v_current_stock`.`product_id` AS `product_id`,`v_current_stock`.`name` AS `name`,`v_current_stock`.`current_stock` AS `current_stock`,`v_current_stock`.`sale_price` AS `sale_price`,`v_current_stock`.`company_name` AS `company_name`,(case when (`v_current_stock`.`current_stock` = 0) then 'OUT_OF_STOCK' when (`v_current_stock`.`current_stock` <= 5) then 'CRITICAL' when (`v_current_stock`.`current_stock` <= 15) then 'LOW' end) AS `stock_status` from `v_current_stock` where (`v_current_stock`.`current_stock` <= 15) order by `v_current_stock`.`current_stock`,`v_current_stock`.`next_expiry` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-09-28 16:05:05
