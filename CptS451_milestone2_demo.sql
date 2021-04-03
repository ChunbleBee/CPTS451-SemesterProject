
--GLOSSARY
--table names
Businesses
Users
Tips
Friends
CheckIn
BusinessCategory
BusinessAttributes
BusinessHours

--some attribute names
ZipCode
BusinessID
city  (Businesses city)
name   (Businesses name)
UserID
friend_id
numtips
numCheckins

UserID
tipcount  (user)
totallikes (user)

Date
Likes
Likes  (tip)

checkinyear
checkinmonth
checkinday
checkintime


--1.
SELECT COUNT(*) 
FROM  Businesses;
SELECT COUNT(*) 
FROM  Users;
SELECT COUNT(*) 
FROM  Tips;
SELECT COUNT(*) 
FROM  Friends;
SELECT COUNT(*) 
FROM  CheckIn;
SELECT COUNT(*) 
FROM  BusinessCategory;
SELECT COUNT(*) 
FROM  BusinessAttributes;
SELECT COUNT(*) 
FROM  BusinessHours;



--2. Run the following queries on your Businesses table, CheckIn table and review table. Make sure to change the attribute names based on your schema. 

SELECT ZipCode, COUNT(distinct C.Category)
FROM Businesses as B, BusinessCategory as C
WHERE B.BusinessID = C.BusinessID
GROUP BY ZipCode
HAVING count(distinct C.Category)>300
ORDER BY ZipCode;

SELECT ZipCode, COUNT(distinct A.attribute)
FROM Businesses as B, BusinessAttributes as A
WHERE B.BusinessID = A.BusinessID
GROUP BY ZipCode
HAVING count(distinct A.Attribute) = 30;


SELECT Users.UserID, count(User02)
FROM Users, Friends
WHERE Users.UserID = Friends.User01 AND 
      Users.UserID = 'NxtYkOpXHSy7LWRKJf3z0w'
GROUP BY Users.UserID;


--3. Run the following queries on your Businesses table, CheckIn table and tips table. Make sure to change the attribute names based on your schema. 


SELECT BusinessID, BusinessName, City, NumTips, NumCheckIns
FROM Businesses
WHERE BusinessID ='K8M3OeFCcAnxuxtTc0BQrQ';

SELECT UserID, name, TipCount, TotalLikes
FROM Users
WHERE UserID = 'NxtYkOpXHSy7LWRKJf3z0w';

-----------

SELECT COUNT(*)
FROM CheckIn
WHERE BusinessID ='K8M3OeFCcAnxuxtTc0BQrQ';

SELECT count(*)
FROM Tips
WHERE  BusinessID = 'K8M3OeFCcAnxuxtTc0BQrQ';


--4. 
--Type the following statements. Make sure to change the attribute names based on your schema. 

SELECT BusinessID, BusinessName, City, NumCheckIns, NumTips
FROM Businesses
WHERE BusinessID ='hDD6-yk1yuuRIvfdtHsISg';

INSERT INTO CheckIn (BusinessID, checkinyear,checkinmonth, checkinday,checkintime)
VALUES ('hDD6-yk1yuuRIvfdtHsISg','2021','04','02','15:00');


--5.
--Type the following statements. Make sure to change the attribute names based on your schema.  

SELECT BusinessID, BusinessName, City, NumCheckIns, NumTips
FROM Businesses
WHERE BusinessID ='hDD6-yk1yuuRIvfdtHsISg';

SELECT UserID, UserName, TipCount, TotalLikes
FROM Users
WHERE UserID = '3z1EttCePzDn9OZbudD5VA';


INSERT INTO Tips (UserID, BusinessID, Date, Text, Likes)  
VALUES ('3z1EttCePzDn9OZbudD5VA','hDD6-yk1yuuRIvfdtHsISg', '2021-04-02 13:00','EVERYTHING IS AWESOME', 0);

UPDATE Tips 
SET Likes = Likes+1
WHERE UserID = '3z1EttCePzDn9OZbudD5VA' AND 
      BusinessID = 'hDD6-yk1yuuRIvfdtHsISg' AND 
      Date ='2021-04-02 13:00';

      