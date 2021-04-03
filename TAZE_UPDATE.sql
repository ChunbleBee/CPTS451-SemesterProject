UPDATE businesses
SET numCheckins =
    (SELECT COUNT(*)
        FROM Checkins
        WHERE businesses.businessID = Checkins.businessID
        GROUP BY businessID);

UPDATE businesses
SET numCheckins = 0
WHERE businesses.numCheckins is NULL;

UPDATE businesses
SET numTips =
    (SELECT COUNT(*)
        FROM tips
        WHERE businesses.businessID = tips.businessID
        GROUP BY businessID);

UPDATE businesses
SET numTips = 0
WHERE businesses.numTips is NULL;

UPDATE users
SET totalLikes =
    (SELECT SUM(tips.likes)
        FROM tips
        WHERE users.userID = tips.userID
        GROUP BY userID);

UPDATE users
SET totalLikes = 0
WHERE users.totalLikes is NULL;

UPDATE users
SET tipCount =
    (SELECT COUNT(*)
        FROM tips
        WHERE users.userID = tips.userID
        GROUP BY userID);

UPDATE users
SET tipCount = 0
WHERE users.tipCount is NULL;