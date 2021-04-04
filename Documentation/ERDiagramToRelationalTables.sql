/***********************************************************/
/*------------------------- USERS -------------------------*/
/***********************************************************/
/* User Table */
CREATE TABLE Users
(
    UserID          TEXT NOT NULL,
    CreationDate    TIMESTAMP,
    UserName        TEXT,
    FansRating      INTEGER,
    FunnyRating     INTEGER,
    CoolRating      INTEGER,
    AvgStarRating   DECIMAL (2, 1),
    PRIMARY KEY (UserID)
);

/* Link table for creating friends */
CREATE TABLE Friends
(
    User01  TEXT NOT NULL,
    User02  TEXT NOT NULL,
    FOREIGN KEY (User01) REFERENCES Users(UserID),
    FOREIGN KEY (User02) REFERENCES Users(UserID),
    PRIMARY KEY (User01, User02)
);


/***********************************************************/
/*----------------------- BUSINESSES ----------------------*/
/***********************************************************/
/* Businesses Table */
CREATE TABLE Business
(
    BusinessID      TEXT NOT NULL,
    BusinessName    TEXT,
    State           TEXT,
    City            TEXT,
	Address         TEXT,
	Zip             TEXT,
	Longitude       FLOAT,
	Latitude        FLOAT,
    IsOpen          BOOLEAN,
    ReviewCount     INTEGER,
    StarRating      DECIMAL (2, 1),
    numTips         INTEGER,
	numCheckIns     INTEGER,
    PRIMARY KEY (BusinessID)
);

/* Business Address Table */

CREATE TABLE BusinessHours
(
    BusinessID      TEXT NOT NULL,
    timeOpen        TIME,
    timeClose       TIME,
	theDay          TEXT
    FOREIGN KEY (BusinessID) REFERENCES Business(BusinessID),
    PRIMARY KEY (theDay, BusinessID)
);

/* Basic Business Attributes Table */
CREATE TABLE BusinessAttributes
(
    BusinessID          TEXT NOT NULL,
    attrName            TEXT,
    attValue            TEXT,
    FOREIGN KEY (BusinessID) REFERENCES Business(BusinessID),
    PRIMARY KEY (attrName, BusinessID)
);

CREATE TABLE BusinessCategories
(
    BusinessID          TEXT NOT NULL,
    catName             TEXT,
    FOREIGN KEY (BusinessID) REFERENCES Business(BusinessID),
    PRIMARY KEY (catName, BusinessID)
);

/***********************************************************/
/*------------------------ REVIEWS ------------------------*/
/***********************************************************/
/* Reviews Table */
CREATE TABLE Tips
(
    UserID          TEXT NOT NULL,
    BusinessID      TEXT NOT NULL,
    ReviewText      TEXT,
    Likes           INTEGER,
    CreationDate    TIMESTAMP,
    FOREIGN KEY (BusinessID) REFERENCES Business(BusinessID),
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    PRIMARY KEY (UserID, CreationDate, BusinessID)
);


/***********************************************************/
/*-------------------------- MISC -------------------------*/
/***********************************************************/
/* Check Ins Table */
CREATE TABLE CheckIns
(
    BusinessID  TEXT,
    CheckInDate TIMESTAMP,
    FOREIGN KEY (BusinessID) REFERENCES Business(BusinessID),
    PRIMARY KEY (CheckInDate, BusinessID)
);

