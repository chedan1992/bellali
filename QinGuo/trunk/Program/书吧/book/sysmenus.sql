/*
 Navicat Premium Data Transfer

 Source Server         : 本地
 Source Server Type    : MySQL
 Source Server Version : 50643
 Source Host           : localhost:3306
 Source Schema         : book

 Target Server Type    : MySQL
 Target Server Version : 50643
 File Encoding         : 65001

 Date: 23/07/2019 23:35:43
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for sysmenus
-- ----------------------------
DROP TABLE IF EXISTS `sysmenus`;
CREATE TABLE `sysmenus`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Type` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `Name` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `PId` int(11) NULL DEFAULT NULL,
  `Content` varchar(500) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `CreateTime` datetime(0) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 8 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Compact;

-- ----------------------------
-- Records of sysmenus
-- ----------------------------
INSERT INTO `sysmenus` VALUES (1, '0', 'dd', 0, NULL, '2019-07-23 23:27:15');
INSERT INTO `sysmenus` VALUES (2, '0', '22', 0, NULL, '2019-07-23 23:27:34');
INSERT INTO `sysmenus` VALUES (5, '0', '234', 1, NULL, '2019-07-23 23:30:47');
INSERT INTO `sysmenus` VALUES (6, '0', 'ff', 1, NULL, '2019-07-23 23:31:26');
INSERT INTO `sysmenus` VALUES (7, '0', 'gg', 2, NULL, '2019-07-23 23:31:32');

SET FOREIGN_KEY_CHECKS = 1;
