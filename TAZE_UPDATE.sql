UPDATE businesses
SET numCheckins = 
    (SELECT COUNT(*) 
        FROM CheckIns
        WHERE businesses.businessID = CheckIns.businessID
        GROUP BY businessID);

UPDATE businesses
SET numTips = 
    (SELECT COUNT(*) 
        FROM tips
        WHERE businesses.businessID = tips.businessID
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