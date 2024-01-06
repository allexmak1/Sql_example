/* добавление новой строки */
/* INSERT INTO Table1 (Name, Phone, Data) VALUES (N'Саня', N'123', '10/2/2023') */

/* SELECT column_name1, column_name2 FORM table_name [WHERE expression] */
/* вывести все */
/* SELECT * FROM Products */

/* только нужные колонки*/
/* SELECT ProductName, UnitPrice FROM Products */

/* SELECT ProductName, UnitPrice FROM Products WHERE UnitPrice > 100 */

/* поиск с вхождением     N'%Кирилица%' */
/* SELECT ProductName, UnitPrice FROM Products WHERE ProductName LIKE '%Cha%' */

/* SELECT ProductName, UnitPrice FROM Products WHERE CategoryID = 3 AND UnitPrice > 20 */

/* умножаем и переименовываем колонку */
/* SELECT  ProductName, UnitPrice * UnitsInStock AS N'товаров на складе на сумму' FROM Products WHERE UnitsInStock > 0 */

/* вывод всех имен покупателей */
/* SELECT DISTINCT CustomerID FROM Orders */

/* кол-во*/
SELECT COUNT (DISTINCT CustomerID) FROM Orders
