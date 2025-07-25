/*M!999999\- enable the sandbox mode */ 
-- MariaDB dump 10.19-11.7.2-MariaDB, for Win64 (AMD64)
--
-- Host: localhost    Database: sandbox
-- ------------------------------------------------------
-- Server version	8.0.30

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*M!100616 SET @OLD_NOTE_VERBOSITY=@@NOTE_VERBOSITY, NOTE_VERBOSITY=0 */;

--
-- Table structure for table `logs`
--

DROP TABLE IF EXISTS `logs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `logs` (
  `id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `merchant_id` varchar(36) NOT NULL,
  `process_name` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `data` json NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `logs`
--

LOCK TABLES `logs` WRITE;
/*!40000 ALTER TABLE `logs` DISABLE KEYS */;
/*!40000 ALTER TABLE `logs` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `merchant_users`
--

DROP TABLE IF EXISTS `merchant_users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `merchant_users` (
  `id` varchar(64) NOT NULL,
  `name` varchar(100) NOT NULL,
  `api_key` varchar(100) NOT NULL,
  `merchant_identifier` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `api_key` (`api_key`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `merchant_users`
--

LOCK TABLES `merchant_users` WRITE;
/*!40000 ALTER TABLE `merchant_users` DISABLE KEYS */;
INSERT INTO `merchant_users` VALUES
('09a74ba7-bb9b-4b98-8b3d-1552fd1a66d6','Toko A','test-key','merchant_1');
/*!40000 ALTER TABLE `merchant_users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `payins`
--

DROP TABLE IF EXISTS `payins`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `payins` (
  `id` varchar(64) NOT NULL,
  `amount` decimal(12,2) DEFAULT NULL,
  `currency` varchar(10) DEFAULT NULL,
  `status` varchar(20) DEFAULT NULL,
  `reference_id` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `payins`
--

LOCK TABLES `payins` WRITE;
/*!40000 ALTER TABLE `payins` DISABLE KEYS */;
INSERT INTO `payins` VALUES
('858e7b67-9247-4a37-9c97-51adadf50132',150000.00,'IDR','PENDING','ref-finmo-2d6b4e70-56b8-4de9-8b97-909bb85e76d7');
/*!40000 ALTER TABLE `payins` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `payouts`
--

DROP TABLE IF EXISTS `payouts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `payouts` (
  `id` varchar(64) NOT NULL,
  `amount` decimal(12,2) DEFAULT NULL,
  `currency` varchar(10) DEFAULT NULL,
  `status` varchar(20) DEFAULT NULL,
  `reference_id` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `payouts`
--

LOCK TABLES `payouts` WRITE;
/*!40000 ALTER TABLE `payouts` DISABLE KEYS */;
/*!40000 ALTER TABLE `payouts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `terminal_settings`
--

DROP TABLE IF EXISTS `terminal_settings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8mb4 */;
CREATE TABLE `terminal_settings` (
  `id` varchar(64) NOT NULL,
  `merchant_id` varchar(64) DEFAULT NULL,
  `gateway` varchar(20) DEFAULT NULL,
  `method` varchar(20) DEFAULT NULL,
  `currency` varchar(10) DEFAULT NULL,
  `is_active` tinyint(1) DEFAULT '1',
  PRIMARY KEY (`id`),
  KEY `merchant_id` (`merchant_id`),
  CONSTRAINT `terminal_settings_ibfk_1` FOREIGN KEY (`merchant_id`) REFERENCES `merchant_users` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `terminal_settings`
--

LOCK TABLES `terminal_settings` WRITE;
/*!40000 ALTER TABLE `terminal_settings` DISABLE KEYS */;
INSERT INTO `terminal_settings` VALUES
('cfg_1','09a74ba7-bb9b-4b98-8b3d-1552fd1a66d6','XENDIT','VA','IDR',1),
('cfg_2','09a74ba7-bb9b-4b98-8b3d-1552fd1a66d6','XENDIT','QRIS','IDR',1);
/*!40000 ALTER TABLE `terminal_settings` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'sandbox'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*M!100616 SET NOTE_VERBOSITY=@OLD_NOTE_VERBOSITY */;

-- Dump completed on 2025-07-25 13:09:37
