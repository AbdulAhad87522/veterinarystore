-- MySQL dump 10.13  Distrib 8.0.42, for Win64 (x86_64)
--
-- Host: localhost    Database: veterninary
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
  `stock` int NOT NULL,
  `purchase_price` decimal(10,2) NOT NULL,
  `quantity_received` int NOT NULL,
  `expiry_date` datetime NOT NULL,
  PRIMARY KEY (`batch_item_id`),
  KEY `purchase_batch_id` (`purchase_batch_id`),
  KEY `idx_expiry` (`expiry_date`),
  KEY `product_id_idx` (`product_id`),
  CONSTRAINT `batch_items_ibfk_1` FOREIGN KEY (`purchase_batch_id`) REFERENCES `purchase_batches` (`purchase_batch_id`),
  CONSTRAINT `product_id` FOREIGN KEY (`product_id`) REFERENCES `medicines` (`product_id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `batch_items`
--

LOCK TABLES `batch_items` WRITE;
/*!40000 ALTER TABLE `batch_items` DISABLE KEYS */;
INSERT INTO `batch_items` VALUES (1,1,1,99,50.00,100,'2026-12-31 00:00:00'),(2,2,2,0,200.00,50,'2025-06-30 00:00:00'),(3,3,3,0,20.00,200,'2027-01-15 00:00:00'),(4,1,4,0,75.00,150,'2026-03-20 00:00:00'),(5,1,1,100,12.50,100,'2025-01-01 00:00:00'),(6,2,1,96,12.75,150,'2025-06-15 00:00:00'),(7,3,1,99,13.00,200,'2026-01-01 00:00:00');
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
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `categories`
--

LOCK TABLES `categories` WRITE;
/*!40000 ALTER TABLE `categories` DISABLE KEYS */;
INSERT INTO `categories` VALUES (1,'Antibiotic'),(2,'Vaccine'),(3,'Painkiller'),(4,'Vitamin');
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
  `company_name` varchar(45) NOT NULL,
  `contact` varchar(45) NOT NULL,
  `address` varchar(45) NOT NULL,
  PRIMARY KEY (`company_id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `company`
--

LOCK TABLES `company` WRITE;
/*!40000 ALTER TABLE `company` DISABLE KEYS */;
INSERT INTO `company` VALUES (1,'Pfizer Vet','0300-1234567','Lahore'),(2,'Searle Pharma','0321-9876543','Karachi'),(3,'Getz Veterinary','0312-5558888','Islamabad');
/*!40000 ALTER TABLE `company` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `companypricerecord`
--

DROP TABLE IF EXISTS `companypricerecord`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `companypricerecord` (
  `company_record_id` int NOT NULL AUTO_INCREMENT,
  `company_id` int NOT NULL,
  `purchase_batch_id` int NOT NULL,
  `date` date NOT NULL,
  `payment` decimal(10,2) NOT NULL,
  `remarks` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`company_record_id`),
  KEY `company_id` (`company_id`),
  KEY `purchase_batch_id` (`purchase_batch_id`),
  CONSTRAINT `supplierpricerecord_ibfk_1` FOREIGN KEY (`company_id`) REFERENCES `company` (`company_id`),
  CONSTRAINT `supplierpricerecord_ibfk_2` FOREIGN KEY (`purchase_batch_id`) REFERENCES `purchase_batches` (`purchase_batch_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=29 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `companypricerecord`
--

LOCK TABLES `companypricerecord` WRITE;
/*!40000 ALTER TABLE `companypricerecord` DISABLE KEYS */;
/*!40000 ALTER TABLE `companypricerecord` ENABLE KEYS */;
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
  `sale_id` int NOT NULL,
  `date` date NOT NULL,
  `payment` decimal(10,2) NOT NULL,
  `remarks` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`record_id`),
  KEY `customer_id` (`customer_id`),
  KEY `sale_id` (`sale_id`),
  CONSTRAINT `customerpricerecord_ibfk_1` FOREIGN KEY (`customer_id`) REFERENCES `customers` (`customer_id`),
  CONSTRAINT `customerpricerecord_ibfk_2` FOREIGN KEY (`sale_id`) REFERENCES `sales` (`sale_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=50 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `customerpricerecord`
--

LOCK TABLES `customerpricerecord` WRITE;
/*!40000 ALTER TABLE `customerpricerecord` DISABLE KEYS */;
INSERT INTO `customerpricerecord` VALUES (46,37,6,'2025-09-17',3000.00,NULL),(47,1,9,'2025-09-17',500.00,NULL),(48,1,10,'2025-09-17',100.00,NULL),(49,1,11,'2025-09-17',0.00,NULL);
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
  `first_name` varchar(50) DEFAULT NULL,
  `last_name` varchar(50) DEFAULT NULL,
  `phone` varchar(20) DEFAULT NULL,
  `address` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`customer_id`),
  UNIQUE KEY `first_name` (`first_name`,`last_name`)
) ENGINE=InnoDB AUTO_INCREMENT=42 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `customers`
--

LOCK TABLES `customers` WRITE;
/*!40000 ALTER TABLE `customers` DISABLE KEYS */;
INSERT INTO `customers` VALUES (1,'walk','_in','0','walk_in'),(36,'Ali','Khan','03001234567','Lahore, Punjab'),(37,'Sara','Ahmed','03017654321','Karachi, Sindh'),(38,'Usman','Sheikh','03121239876','Islamabad, Capital Territory'),(39,'Fatima','Malik','03334445555','Faisalabad, Punjab'),(40,'Bilal','Hussain','03451239876','Multan, Punjab');
/*!40000 ALTER TABLE `customers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `inventory`
--

DROP TABLE IF EXISTS `inventory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `inventory` (
  `inventory_id` int NOT NULL AUTO_INCREMENT,
  `medic_id` int NOT NULL,
  `stock` int NOT NULL,
  `expiry_date` date DEFAULT NULL,
  PRIMARY KEY (`inventory_id`),
  KEY `medic_id_idx` (`medic_id`),
  CONSTRAINT `medic_id` FOREIGN KEY (`medic_id`) REFERENCES `medicines` (`product_id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `inventory`
--

LOCK TABLES `inventory` WRITE;
/*!40000 ALTER TABLE `inventory` DISABLE KEYS */;
INSERT INTO `inventory` VALUES (1,1,100,'2026-12-31'),(2,2,50,'2025-06-30'),(3,3,200,'2027-01-15'),(4,4,150,'2026-03-20');
/*!40000 ALTER TABLE `inventory` ENABLE KEYS */;
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
  `Packing` varchar(45) DEFAULT NULL,
  `Category_id` int NOT NULL,
  `sale_price` int NOT NULL,
  PRIMARY KEY (`product_id`),
  UNIQUE KEY `uq_name_company` (`name`,`company_id`),
  KEY `idx_name` (`name`),
  KEY `idx_company` (`company_id`),
  KEY `Category_id_idx` (`Category_id`),
  CONSTRAINT `Category_id` FOREIGN KEY (`Category_id`) REFERENCES `categories` (`category_id`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `company_id` FOREIGN KEY (`company_id`) REFERENCES `company` (`company_id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `medicines`
--

LOCK TABLES `medicines` WRITE;
/*!40000 ALTER TABLE `medicines` DISABLE KEYS */;
INSERT INTO `medicines` VALUES (1,'Amoxicillin','Broad-spectrum antibiotic',1,'250mg Tablet',1,1000),(2,'Rabies Vaccine','Vaccine for rabies prevention',2,'5ml Vial',2,12000),(3,'Paracetamol Vet','Pain & fever reducer',1,'500mg Tablet',3,12340),(4,'Vitamin B12','Energy booster for livestock',3,'Injection 1ml',4,500);
/*!40000 ALTER TABLE `medicines` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `purchase_batches`
--

DROP TABLE IF EXISTS `purchase_batches`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `purchase_batches` (
  `purchase_batch_id` int NOT NULL AUTO_INCREMENT,
  `company_Id` int NOT NULL,
  `purchase_date` date NOT NULL DEFAULT (curdate()),
  PRIMARY KEY (`purchase_batch_id`),
  KEY `company_id_idx` (`company_Id`),
  CONSTRAINT `p_batches` FOREIGN KEY (`company_Id`) REFERENCES `company` (`company_id`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `purchase_batches`
--

LOCK TABLES `purchase_batches` WRITE;
/*!40000 ALTER TABLE `purchase_batches` DISABLE KEYS */;
INSERT INTO `purchase_batches` VALUES (1,1,'2025-09-01'),(2,2,'2025-09-05'),(3,3,'2025-09-10');
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
  `price` decimal(10,2) DEFAULT NULL,
  `Discount` int DEFAULT NULL,
  PRIMARY KEY (`sale_item_id`),
  KEY `idx_sale` (`sale_id`),
  KEY `sale_items_ibfk_2_idx` (`batch_item_id`),
  CONSTRAINT `sale_items_ibfk_1` FOREIGN KEY (`sale_id`) REFERENCES `sales` (`sale_id`),
  CONSTRAINT `sale_items_ibfk_2` FOREIGN KEY (`batch_item_id`) REFERENCES `batch_items` (`batch_item_id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sale_items`
--

LOCK TABLES `sale_items` WRITE;
/*!40000 ALTER TABLE `sale_items` DISABLE KEYS */;
INSERT INTO `sale_items` VALUES (1,10,6,2,1000.00,0),(2,11,6,1,1000.00,0),(3,11,1,1,1000.00,0),(4,11,7,1,1000.00,0);
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
  `customer_id` int DEFAULT NULL,
  `total_amount` decimal(10,2) NOT NULL,
  `paid_amount` int DEFAULT NULL,
  `sale_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`sale_id`),
  KEY `idx_sale_date` (`sale_date`),
  KEY `customer_id_idx` (`customer_id`),
  CONSTRAINT `customer_id` FOREIGN KEY (`customer_id`) REFERENCES `customers` (`customer_id`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sales`
--

LOCK TABLES `sales` WRITE;
/*!40000 ALTER TABLE `sales` DISABLE KEYS */;
INSERT INTO `sales` VALUES (1,NULL,5000.00,NULL,'2025-09-11 00:00:00'),(2,NULL,2500.00,NULL,'2025-09-12 00:00:00'),(6,37,3000.00,3000,'2025-09-17 22:03:05'),(9,1,0.00,500,'2025-09-17 22:12:48'),(10,1,2000.00,100,'2025-09-17 23:21:42'),(11,1,3000.00,0,'2025-09-17 23:23:22');
/*!40000 ALTER TABLE `sales` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `user_id` int NOT NULL AUTO_INCREMENT,
  `username` varchar(45) NOT NULL,
  `password` varchar(45) NOT NULL,
  PRIMARY KEY (`user_id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'admin','admin123'),(2,'vetuser','vet123');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;

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
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-09-18 23:53:57
