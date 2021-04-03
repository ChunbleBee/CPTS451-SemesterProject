/***********************************************************/
/*------------------------- USERS -------------------------*/
/***********************************************************/
/* User Table */
CREATE TABLE Users
(
    UserID          TEXT NOT NULL,
    CreationDate    TIMESTAMP NOT NULL DEFAULT NOW(),
    UserName        TEXT,
    TotalLikes      INTEGER NOT NULL DEFAULT (0),
    TipCount        INTEGER NOT NULL DEFAULT (0),
    FansRating      INTEGER,
    FunnyRating     INTEGER,
    CoolRating      INTEGER,
    AvgStarRating   DECIMAL (3, 2),
    Latitude        DECIMAL (8, 6),
    Longitude       DECIMAL (9, 6),
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
CREATE TABLE Businesses
(
    BusinessID      TEXT NOT NULL,
    BusinessName    TEXT,
    Street          TEXT,
    City            TEXT,
    State           TEXT,
    ZipCode         INTEGER,
    Latitude        DECIMAL (8, 6),
    Longitude       DECIMAL (9, 6),
    IsOpen          BOOLEAN,
    ReviewCount     INTEGER NOT NULL DEFAULT 0,
    StarRating      DECIMAL (2, 1) NOT NULL DEFAULT 0.0,
    NumCheckIns     INTEGER NOT NULL DEFAULT (0),
    NumTips         INTEGER NOT NULL DEFAULT (0),
    PRIMARY KEY (BusinessID)
);

CREATE TABLE BusinessHours
(
    BusinessID      TEXT NOT NULL,
    /*Opening & closing time of 0:0 indicates closed on that day*/
    OpeningTimes    TIME [],
    ClosingTimes    TIME [],
    FOREIGN KEY (BusinessID) REFERENCES Businesses(BusinessID),
    PRIMARY KEY (BusinessID)
);

/* Basic Business Attributes Table */
CREATE TABLE BusinessAttributes
(
    BusinessID          TEXT NOT NULL,
    Attribute           TEXT NOT NULL,
    Value               TEXT,
    SubTypes            TEXT[],
    Values              TEXT[],
    FOREIGN KEY (BusinessID) REFERENCES Businesses(BusinessID),
    PRIMARY KEY (BusinessID, Attribute)
);

/* BusinessCategories Table */
CREATE TABLE BusinessCategories
(
    BusinessID  TEXT NOT NULL,
    Category    TEXT NOT NULL,
    FOREIGN KEY (BusinessID) REFERENCES Businesses(BusinessID),
    PRIMARY KEY (BusinessID, Category)
);


/***********************************************************/
/*------------------------ REVIEWS ------------------------*/
/***********************************************************/
/* Reviews Table */
CREATE TABLE Reviews
(
    UserID          TEXT NOT NULL,
    BusinessID      TEXT NOT NULL,
    ReviewTest      TEXT,
    Likes           INTEGER,
    CreationDate    TIMESTAMP,
    FOREIGN KEY (BusinessID) REFERENCES Businesses(BusinessID),
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    PRIMARY KEY (UserID, BusinessID)
);


/***********************************************************/
/*-------------------------- MISC -------------------------*/
/***********************************************************/
/* Check Ins Table */
CREATE TABLE CheckIns
(
    BusinessID  TEXT NOT NULL,
    CheckInDate TIMESTAMP NOT NULL DEFAULT NOW(),
    FOREIGN KEY (BusinessID) REFERENCES Businesses(BusinessID),
    PRIMARY KEY (BusinessID, CheckInDate)
);

/* Tips Table */
CREATE TABLE Tips
(
    BusinessID  TEXT NOT NULL,
	UserID      TEXT NOT NULL,
    Date        TIMESTAMP NOT NULL DEFAULT NOW(),
	Likes       INTEGER,
	Text        TEXT,
    FOREIGN KEY (BusinessID) REFERENCES Businesses(BusinessID),
	FOREIGN KEY (UserID) REFERENCES Users(UserID),
    PRIMARY KEY (BusinessID, UserID, Date)
);