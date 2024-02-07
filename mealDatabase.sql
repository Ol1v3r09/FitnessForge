-- --------------------------------------------------------
-- Hoszt:                        127.0.0.1
-- Szerver verzió:               10.4.32-MariaDB - mariadb.org binary distribution
-- Szerver OS:                   Win64
-- HeidiSQL Verzió:              12.6.0.6765
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

-- Tábla adatainak mentése fitnessforge.activity_level: ~5 rows (hozzávetőleg)
INSERT INTO `activity_level` (`Id`, `Name`, `Description`, `BmrMultiplier`) VALUES
	(1, 'Mozgásszegény életmód', 'Olyan emberek, akik ülő munák végeznek, és nagyon kevés gyakorlatot vagy fizikai aktivitást végeznek.', 1.2),
	(2, 'Enyhén aktív', 'Olyan emberek, akik fizikai aktivitást végeznek és hosszú sétákat tesznek vagy egy héten legalább 1-3 napot edzenek.', 1.375),
	(3, 'Közepesen aktív', 'Olyan emberek, akik sokat mozognak napközben, és egy héten legalább 3-5 napot edzenek mérsékelt erőfeszítéssel.', 1.55),
	(4, 'Nagyon aktív', 'Olyan emberek, akik a legtöbb napon sportolnak vagy erőteljes testmozgást végeznek.', 1.725),
	(5, 'Extrán aktív', 'Olyan emberek, akik heti 6-7 napon intenzív edzést végeznek fizikai aktivitást igénylő munkával.', 1.9);

-- Tábla adatainak mentése fitnessforge.meal_type: ~5 rows (hozzávetőleg)
INSERT INTO `meal_type` (`Id`, `Name`) VALUES
	(1, 'Reggeli'),
	(2, 'Tízórai'),
	(3, 'Ebéd'),
	(4, 'Uzsonna'),
	(5, 'Vacsora');

-- Tábla adatainak mentése fitnessforge.nutrient_goal: ~4 rows (hozzávetőleg)
INSERT INTO `nutrient_goal` (`Id`, `Name`, `CarbohydratePercentage`, `ProteinPercentage`, `FatPercentage`) VALUES
	(1, 'Alapméretezett', 50, 20, 30),
	(2, 'Alacsony szénhidráttartalmú', 30, 30, 40),
	(3, 'Magas proteintartalmú', 35, 40, 25),
	(4, 'Alacsony zsírtartalmú', 45, 35, 20);

-- Tábla adatainak mentése fitnessforge.unit: ~7 rows (hozzávetőleg)
INSERT INTO `unit` (`Id`, `Name`, `Symbol`) VALUES
	(1, 'Liter', 'l'),
	(2, 'Centiliter', 'cl'),
	(3, 'Deciliter', 'dl'),
	(4, 'Milliliter', 'ml'),
	(5, 'Gramm', 'g'),
	(6, 'Dekagramm', 'dkg'),
	(7, 'Kilogramm', 'kg');

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
