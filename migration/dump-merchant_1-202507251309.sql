/*M!999999\- enable the sandbox mode */ 
-- MariaDB dump 10.19-11.7.2-MariaDB, for Win64 (AMD64)
--
-- Host: localhost    Database: merchant_1
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
('1658bb9c-36f3-49ec-8f51-ba99e52a52b1',150000.00,'IDR','SUCCESS','ref-finmo-2d6b4e70-56b8-4de9-8b97-909bb85e76d7'),
('1a566c49-11dd-4010-9d03-ef0c2a1c79ea',5.00,'IDR','PENDING','ref-finmo-5b02eec2-dcbf-42d9-9af0-3c815bcc151c'),
('1c129fb8-70f6-48ca-bba8-fd6aa1de192d',150000.00,'IDR','PENDING','ref-finmo-2d6b4e70-56b8-4de9-8b97-909bb85e76d7'),
('25c71154-03b7-4334-8cbd-c0a03b9bdba0',2.00,'IDR','PENDING',NULL),
('2dbfb07d-18ff-4172-894f-5a46f5a3f5d2',2.00,'IDR','PENDING','ref-finmo-123'),
('31d22ac6-0926-45fc-8b4c-fdc31581b0c9',150000.00,'IDR','PENDING','ref-finmo-2d6b4e70-56b8-4de9-8b97-909bb85e76d7'),
('3658ce24-c70f-4489-aab1-9f16a2b41d1e',150000.00,'IDR','PENDING','ref-finmo-2d6b4e70-56b8-4de9-8b97-909bb85e76d7'),
('3b6802a0-8556-4121-a1f6-0ebdbf729a3f',2.00,'IDR','PENDING','ref-finmo-123'),
('3b7426b3-f6c7-4b4a-a101-bd6dd9ddf921',2.00,'IDR','PENDING','ref-finmo-123'),
('3bd686a4-37fd-4082-908c-93a1bec9d466',10000.00,'IDR','PENDING','ref-xyz'),
('3df0d9f0-70f9-4225-a331-ca201063d7a9',5.00,'IDR','PENDING','ref-finmo-23c0e21b-46b8-4339-beb7-c0434776ec95'),
('406d3b20-2dc9-4233-91b2-06bdc869e02f',10000.00,'IDR','PENDING','ref-xyz'),
('434c05aa-242a-4465-a90b-6c88b8ec5787',10000.00,'IDR','PENDING','ref-xyz'),
('4c3d39ef-e8ee-4aed-9167-1c7e9cfdbbec',150000.00,'IDR','PENDING','ref-finmo-2d6b4e70-56b8-4de9-8b97-909bb85e76d7'),
('4c54f4d8-5b5a-4924-b149-e5c98075c334',10000.00,'IDR','PENDING','ref-xyz'),
('4eb25e7a-487b-449a-85f5-5a52c8ee6939',2.00,'IDR','PENDING','ref-finmo-123'),
('4f185a61-b5f7-496e-a501-6a8f2467e39f',2.00,'IDR','PENDING','ref-finmo-3f6a58ba-a270-403e-9fa3-2ede5245708f'),
('59bbb315-d555-48fe-9241-289c2df76ae1',2.00,'IDR','PENDING','ref-finmo-123'),
('61a68a86-4c4d-45d7-b0e1-cb8e4afffe35',10000.00,'IDR','PENDING','ref-xyz'),
('6c3dab0d-d2fa-4076-934d-114eccdaf094',150000.00,'IDR','PENDING','ref-finmo-2d6b4e70-56b8-4de9-8b97-909bb85e76d7'),
('6cae5793-bcf6-4bfb-9eb0-e6fe18defbbc',1.00,'IDR','PENDING',NULL),
('6ecd9b6c-60fb-4404-9007-1ec940f50c77',150000.00,'IDR','SUCCESS','ref-finmo-2d6b4e70-56b8-4de9-8b97-909bb85e76d7'),
('766b651d-220c-4be4-a90c-3afc96553a63',2.00,'IDR','PENDING','ref-finmo-5eec8ed5-5a5b-44e6-805b-575c70626303'),
('858e7b67-9247-4a37-9c97-51adadf50132',150000.00,'IDR','PENDING','ref-finmo-2d6b4e70-56b8-4de9-8b97-909bb85e76d7'),
('87db8b77-26c8-4bb9-930f-c8e6d515d2fe',150000.00,'IDR','PENDING','ref-finmo-2d6b4e70-56b8-4de9-8b97-909bb85e76d7'),
('8d4385ae-014b-41b9-819c-3a3bdd18b4f9',150000.00,'IDR','SUCCESS','ref-finmo-2d6b4e70-56b8-4de9-8b97-909bb85e76d7'),
('9b401a7b-3f52-4686-a61d-80b8a1e91424',1.00,'IDR','PENDING',NULL),
('9f1ccdf0-883a-43fa-b6d1-f9c3e00f901e',150000.00,'IDR','SUCCESS','ref-finmo-2d6b4e70-56b8-4de9-8b97-909bb85e76d7'),
('a2216a7b-39df-46ee-9265-f50f531659e0',150000.00,'IDR','SUCCESS','ref-finmo-2d6b4e70-56b8-4de9-8b97-909bb85e76d7'),
('a2bdad97-6542-4865-97ac-70b710c62903',0.00,'IDR','PENDING','ref-xyz'),
('a34aa2e7-4963-4c54-87a1-8b7b5c73d539',2.00,'IDR','PENDING','ref-finmo-96aa23cc-97a3-4f6e-a570-a10ac07b2577'),
('a41f797d-313e-4da0-b9de-5977e01b93a0',10000.00,'IDR','PENDING','ref-xyz'),
('a4ca5854-c62c-411d-b0af-c07e439d81e4',150000.00,'IDR','PENDING','ref-finmo-2d6b4e70-56b8-4de9-8b97-909bb85e76d7'),
('a6835a6e-5e51-428b-b0a9-691fcb344a6e',1.00,'IDR','PENDING',NULL),
('a748fc43-682d-4efd-bc49-85d883cf3b4f',10000.00,'IDR','PENDING','ref-xyz'),
('a8544e64-390f-4dc3-b5f5-aa497534df01',150000.00,'IDR','SUCCESS','ref-finmo-2d6b4e70-56b8-4de9-8b97-909bb85e76d7'),
('ad2924df-a6de-4dd6-a960-befcb16d88b8',150000.00,'IDR','SUCCESS','ref-finmo-2d6b4e70-56b8-4de9-8b97-909bb85e76d7'),
('afc54908-596e-4298-a788-4089e25bcd8a',2.00,'IDR','PENDING','ref-finmo-c364cc10-1c6e-42c2-ad70-2add6195ae82'),
('b24362c7-90f1-4399-a998-d51351a15228',2.00,'IDR','PENDING','ref-finmo-123'),
('b701cbdc-143b-4ebe-9c17-7263145dc42b',150000.00,'IDR','PENDING','ref-finmo-2d6b4e70-56b8-4de9-8b97-909bb85e76d7'),
('b923fcdb-c924-44c8-88d7-f257d661b381',2.00,'IDR','PENDING','ref-finmo-123'),
('bb4b61f0-84aa-4482-a98e-69c9b80a1a60',2.00,'IDR','PENDING','ref-finmo-123'),
('bd9abed4-be09-4de3-a985-bc80e17ca036',1.00,'IDR','PENDING',NULL),
('be679eb1-2074-4c86-97ec-b167b56958a9',2.00,'IDR','PENDING','ref-finmo-123'),
('c4e452b4-cf8f-452f-bf9e-df55ede185e6',5.00,'IDR','PENDING','ref-finmo-cb386966-e0c8-4d84-a417-1b5f1cab1b67'),
('c977a5a0-0518-4e4b-86ca-18e0ab50a5e0',5.00,'IDR','PENDING','ref-finmo-6430085b-b8aa-4601-a568-988c4b94e754'),
('c97ab9d6-78fb-4641-a483-723246cfa3db',2.00,'IDR','PENDING','ref-finmo-123'),
('c986c10a-c74d-4f6e-b3a1-cea839f37953',2.00,'IDR','PENDING','ref-finmo-9626541a-c892-4305-8156-36756b21033a'),
('d13a9495-8786-41f4-a23d-57b6fe88a72a',150000.00,'IDR','PENDING','ref-finmo-2d6b4e70-56b8-4de9-8b97-909bb85e76d7'),
('d2eb87c9-0cfd-4ed9-954a-a5949d787faf',10000.00,'IDR','PENDING','ref-xyz'),
('d3c6166a-8d6c-4e64-98d8-ec25d8a56bdf',2.00,'IDR','PENDING','ref-finmo-776882fe-61d8-4501-ac69-dd7f65f4188d'),
('d5ec2fa6-783a-4436-bed7-04841ea86653',2.00,'IDR','PENDING','ref-finmo-123'),
('d62678ec-98bf-4c1b-a400-de2d63563ed4',10000.00,'IDR','PENDING','ref-xyz'),
('d6581757-d6df-427c-8294-34673dd8b60e',150000.00,'IDR','PENDING','ref-finmo-2d6b4e70-56b8-4de9-8b97-909bb85e76d7'),
('e2e7d617-a1dd-493d-976a-cf41c3daa69e',10000.00,'IDR','PENDING','ref-xyz'),
('e6637aea-e696-4c26-b9f4-027f6e99f23f',150000.00,'IDR','PENDING','ref-finmo-2d6b4e70-56b8-4de9-8b97-909bb85e76d7'),
('e680c98b-8f0f-4506-9492-fcda83704988',150000.00,'IDR','SUCCESS','ref-finmo-2d6b4e70-56b8-4de9-8b97-909bb85e76d7'),
('e76cf234-d283-49db-acf0-05f9304f318e',10000.00,'IDR','PENDING','ref-xyz'),
('e77fb176-50fa-4575-af73-2002229fcc05',2.00,'IDR','PENDING','ref-finmo-123'),
('e8eb1b6f-31c6-4c48-ab10-39c79e031486',10000.00,'IDR','PENDING','ref-xyz'),
('ea8fd8c5-6a9d-482e-a3fa-ec387cb51494',150000.00,'IDR','SUCCESS','ref-finmo-2d6b4e70-56b8-4de9-8b97-909bb85e76d7'),
('ebb714e7-b14b-4167-965a-b1a82ff99f0d',2.00,'IDR','PENDING','ref-finmo-123'),
('ef76dd79-b5ba-4c62-8206-87abf54b13f5',2.00,'IDR','PENDING','ref-finmo-123'),
('f40b190e-168d-4051-9b86-7cb44f6f20b9',150000.00,'IDR','PENDING','ref-finmo-2d6b4e70-56b8-4de9-8b97-909bb85e76d7'),
('f4a28080-d707-4d5f-ae65-a7543ffdae80',150000.00,'IDR','PENDING',NULL),
('f55969c1-7bc5-466f-808d-581ef5d1192f',10000.00,'IDR','PENDING','ref-xyz'),
('f62b4cc1-1659-47c6-92b5-1797792d31bb',1.00,'IDR','PENDING',NULL),
('f73b46f0-5bab-40c2-97f4-873d6b42c09f',150000.00,'IDR','PENDING','ref-finmo-2d6b4e70-56b8-4de9-8b97-909bb85e76d7'),
('f9933320-80d5-454b-b6e8-6969b328d435',10000.00,'IDR','PENDING','ref-xyz');
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
INSERT INTO `payouts` VALUES
('a426d506-44b0-4605-9134-b1d309dce51e',10000.00,'IDR','PENDING','ref-xyz');
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
-- Dumping routines for database 'merchant_1'
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
