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
  `purchase_price` decimal(10,2) NOT NULL,
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
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'NO_AUTO_VALUE_ON_ZERO' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `tr_batch_item_insert` BEFORE INSERT ON `batch_items` FOR EACH ROW BEGIN
    SET NEW.quantity_remaining = NEW.quantity_received;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

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
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
) ENGINE=InnoDB AUTO_INCREMENT=52 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
) ENGINE=InnoDB AUTO_INCREMENT=42 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
  `sale_price` decimal(10,2) NOT NULL,
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
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
  `status` enum('pending','completed','overpaid') NOT NULL DEFAULT 'pending',
  `BatchName` varchar(45) NOT NULL,
  PRIMARY KEY (`purchase_batch_id`),
  UNIQUE KEY `BatchName_UNIQUE` (`BatchName`),
  KEY `company_id_idx` (`company_id`),
  CONSTRAINT `fk_purchase_batches_companies` FOREIGN KEY (`company_id`) REFERENCES `company` (`company_id`) ON DELETE RESTRICT,
  CONSTRAINT `fk_purchase_company` FOREIGN KEY (`company_id`) REFERENCES `company` (`company_id`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `chk_paid_amount` CHECK ((`paid` >= 0)),
  CONSTRAINT `chk_paid_vs_total` CHECK ((`paid` <= `total_price`)),
  CONSTRAINT `chk_total_price` CHECK ((`total_price` >= 0))
) ENGINE=InnoDB AUTO_INCREMENT=28 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `trg_purchase_batches_insert` BEFORE INSERT ON `purchase_batches` FOR EACH ROW BEGIN
    IF NEW.paid = NEW.total_price THEN
        SET NEW.status = 'completed';
    ELSEIF NEW.paid < NEW.total_price THEN
        SET NEW.status = 'pending';
    ELSE
        SET NEW.status = 'overpaid'; -- optional safeguard
    END IF;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `trg_purchase_batches_update` BEFORE UPDATE ON `purchase_batches` FOR EACH ROW BEGIN
    IF NEW.paid = NEW.total_price THEN
        SET NEW.status = 'completed';
    ELSEIF NEW.paid < NEW.total_price THEN
        SET NEW.status = 'pending';
    ELSE
        SET NEW.status = 'overpaid'; -- optional safeguard
    END IF;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

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
  `price` decimal(10,2) NOT NULL,
  `discount` decimal(5,2) DEFAULT '0.00',
  PRIMARY KEY (`sale_item_id`),
  KEY `idx_sale` (`sale_id`),
  KEY `fk_sale_items_product_idx` (`batch_item_id`),
  CONSTRAINT `fk_sale_items_product` FOREIGN KEY (`batch_item_id`) REFERENCES `batch_items` (`batch_item_id`) ON DELETE RESTRICT,
  CONSTRAINT `fk_sale_items_sale` FOREIGN KEY (`sale_id`) REFERENCES `sales` (`sale_id`) ON DELETE CASCADE,
  CONSTRAINT `chk_discount` CHECK (((`discount` >= 0) and (`discount` <= 100))),
  CONSTRAINT `chk_price` CHECK ((`price` > 0)),
  CONSTRAINT `chk_quantity` CHECK ((`quantity` > 0))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `sales`
--

DROP TABLE IF EXISTS `sales`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sales` (
  `sale_id` int NOT NULL AUTO_INCREMENT,
  `sale_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `total_amount` decimal(10,2) NOT NULL,
  PRIMARY KEY (`sale_id`),
  KEY `idx_sale_date` (`sale_date`),
  CONSTRAINT `chk_total_amount` CHECK ((`total_amount` > 0))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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

-- Dump completed on 2025-09-22 15:52:04
