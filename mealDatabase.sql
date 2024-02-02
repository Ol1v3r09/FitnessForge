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

-- Tábla adatainak mentése fitnessforge.food: ~0 rows (hozzávetőleg)
INSERT INTO `food` (`foodId`, `name`, `unitId`) VALUES
	(1, 'Teszt', 1);

-- Tábla adatainak mentése fitnessforge.meal: ~0 rows (hozzávetőleg)
INSERT INTO `meal` (`mealId`, `name`, `date`) VALUES
	(1, 'teszt_meal', '2024-01-28 06:25:32.000000');

-- Tábla adatainak mentése fitnessforge.user: ~0 rows (hozzávetőleg)
INSERT INTO `user` (`userId`) VALUES
	(1);

-- Tábla adatainak mentése fitnessforge.user_meals: ~0 rows (hozzávetőleg)
INSERT INTO `user_meals` (`mealId`, `foodId`, `userId`, `amount`) VALUES
	(1, 1, 1, 1);

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
