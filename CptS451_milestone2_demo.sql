
--GLOSSARY
--table names
Business
Users
Tips
Friends
checkin
businesscategory
BusinessAttributes
BusinessHours

--some attribute names
zipcode
BusinessID
city  (business city)
name   (business name)
UserID
friend_id
numtips
numCheckins

UserID
tipcount  (user)
totallikes (user)

tipdate
Likes
Likes  (tip)

checkinyear
checkinmonth
checkinday
checkintime


--1.
SELECT COUNT(*) 
FROM  Business;
SELECT COUNT(*) 
FROM  Users;
SELECT COUNT(*) 
FROM  Tips;
SELECT COUNT(*) 
FROM  Friends;
SELECT COUNT(*) 
FROM  checkin;
SELECT COUNT(*) 
FROM  businesscategory;
SELECT COUNT(*) 
FROM  BusinessAttributes;
SELECT COUNT(*) 
FROM  BusinessHours;



--2. Run the following queries on your business table, checkin table and review table. Make sure to change the attribute names based on your schema. 

SELECT zipcode, COUNT(distinct C.category)
FROM Business as B, businesscategory as C
WHERE B.BusinessID = C.BusinessID
GROUP BY zipcode
HAVING count(distinct C.category)>300
ORDER BY zipcode;

SELECT zipcode, COUNT(distinct A.attribute)
FROM Business as B, BusinessAttributes as A
WHERE B.BusinessID = A.BusinessID
GROUP BY zipcode
HAVING count(distinct A.attribute) = 30;


SELECT Users.UserID, count(friend_id)
FROM Users, Friends
WHERE Users.UserID = Friends.UserID AND 
      Users.UserID = 'NxtYkOpXHSy7LWRKJf3z0w'
GROUP BY Users.UserID;


--3. Run the following queries on your business table, checkin table and tips table. Make sure to change the attribute names based on your schema. 


SELECT BusinessID, name, city, numtips, numCheckins
FROM Business 
WHERE BusinessID ='K8M3OeFCcAnxuxtTc0BQrQ';

SELECT UserID, name, tipcount, totallikes
FROM Users
WHERE UserID = 'NxtYkOpXHSy7LWRKJf3z0w';

-----------

SELECT COUNT(*) 
FROM checkin
WHERE BusinessID ='K8M3OeFCcAnxuxtTc0BQrQ';

SELECT count(*)
FROM Tips
WHERE  BusinessID = 'K8M3OeFCcAnxuxtTc0BQrQ';


--4. 
--Type the following statements. Make sure to change the attribute names based on your schema. 

SELECT BusinessID,name, city, numCheckins, numtips
FROM Business 
WHERE BusinessID ='hDD6-yk1yuuRIvfdtHsISg';

INSERT INTO checkin (BusinessID, checkinyear,checkinmonth, checkinday,checkintime)
VALUES ('hDD6-yk1yuuRIvfdtHsISg','2021','04','02','15:00');


--5.
--Type the following statements. Make sure to change the attribute names based on your schema.  

SELECT BusinessID,name, city, numCheckins, numtips
FROM Business 
WHERE BusinessID ='hDD6-yk1yuuRIvfdtHsISg';

SELECT UserID, name, tipcount, totallikes
FROM Users
WHERE UserID = '3z1EttCePzDn9OZbudD5VA';


INSERT INTO Tips (UserID, BusinessID, tipdate, Likes, Likes)  
VALUES ('3z1EttCePzDn9OZbudD5VA','hDD6-yk1yuuRIvfdtHsISg', '2021-04-02 13:00','EVERYTHING IS AWESOME',0);

UPDATE Tips 
SET Likes = Likes+1
WHERE UserID = '3z1EttCePzDn9OZbudD5VA' AND 
      BusinessID = 'hDD6-yk1yuuRIvfdtHsISg' AND 
      tipdate ='2021-04-02 13:00';

      