UPDATE business
SET numCheckins = 
    (SELECT COUNT(*) 
        FROM CheckIns
        WHERE business.businessID = CheckIns.businessID
        GROUP BY businessID);

UPDATE business
SET numTips = 
    (SELECT COUNT(*) 
        FROM tips
        WHERE business.businessID = tips.businessID
        GROUP BY businessID);

UPDATE users
SET totalLikes = 
    (SELECT SUM(*)
        FROM tips
        WHERE users.userID = tips.userID
        GROUP BY userID);

UPDATE users
SET tipCount = 
    (SELECT COUNT(*)
        FROM tips
        WHERE users.userID = tips.userID
        GROUP BY userID);