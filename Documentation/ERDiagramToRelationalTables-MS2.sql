/***********************************************************/
/*------------------------- USERS -------------------------*/
/***********************************************************/
/* User Table */
CREATE TABLE Users
(
    UserID          TEXT NOT NULL,
    CreationDate    TEXT,
    UserName        TEXT,
    TotalLikes      INTEGER NOT NULL DEFAULT (0),
    TipCount        INTEGER NOT NULL DEFAULT (0),
    FansRating      INTEGER,
    FunnyRating     INTEGER,
    CoolRating      INTEGER,
    AvgStarRating   DECIMAL (2, 1),
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
CREATE TABLE Business
(
    BusinessID      TEXT NOT NULL,
    BusinessName    TEXT,
    State           TEXT,
    City            TEXT,
    IsOpen          BOOLEAN,
    ReviewCount     INTEGER,
    StarRating      DECIMAL (2, 1),
    NumCheckIns        INTEGER NOT NULL DEFAULT (0),
    NumTips            INTEGER NOT NULL DEFAULT (0),
    Categories      TEXT [],
    PRIMARY KEY (BusinessID)
);

/* Business Address Table */
CREATE TABLE Addresses
(
    BusinessID  TEXT NOT NULL,
    Longitude   DECIMAL (9, 6),
    Latitude    DECIMAL (8, 6),
    Street      TEXT,
    City        TEXT,
    USState     TEXT,
    FOREIGN KEY (BusinessID) REFERENCES Business(BusinessID),
    PRIMARY KEY (BusinessID)
);

CREATE TABLE BusinessHours
(
    BusinessID      TEXT NOT NULL,
    Days            TEXT [],
    OpeningTimes    TEXT [],
    ClosingTimes    TEXT [],
    FOREIGN KEY (BusinessID) REFERENCES Business(BusinessID),
    PRIMARY KEY (BusinessID)
);

/* Basic Business Attributes Table */
CREATE TABLE BusinessAttributes
(
    BusinessID          TEXT NOT NULL,
    HasTV               BOOLEAN,
    HasWifi             BOOLEAN,
    WheelchairAccess    BOOLEAN,
    Parking             BOOLEAN [],
    PaymentMethods      BOOLEAN [],
    PriceRange          INTEGER,
    FOREIGN KEY (BusinessID) REFERENCES Business(BusinessID),
    PRIMARY KEY (BusinessID)
);

CREATE TABLE Restaurants
(
    BusinessID          TEXT NOT NULL,
    TakesReservations   BOOLEAN,
    TakeOut             BOOLEAN,
    TableService        BOOLEAN,
    KidFriendly         BOOLEAN,
    NoiseLevel          INTEGER,
    Ambiences           TEXT [],
    BestNights          BOOLEAN [7],
    MealTimes           BOOLEAN [],
    Alcohol             TEXT,
    FOREIGN KEY (BusinessID) REFERENCES Business(BusinessID),
    PRIMARY KEY (BusinessID)
);

CREATE TABLE MusicInfo
(
    BusinessID          TEXT NOT NULL,
    IsDancingFriendly   BOOLEAN,
    HasDJ               BOOLEAN,
    HasVJ               BOOLEAN,
    HasLivePerformances BOOLEAN,
    HasKaraoke          BOOLEAN,
    HasBackgroundMusic  BOOLEAN,
    FOREIGN KEY (BusinessID) REFERENCES Business(BusinessID),
    PRIMARY KEY (BusinessID)
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
    FOREIGN KEY (BusinessID) REFERENCES Business(BusinessID),
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    PRIMARY KEY (UserID, BusinessID)
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
    PRIMARY KEY (BusinessID, CheckInDate)
);

/* Tips Table */
CREATE TABLE Tips
(
    BusinessID  TEXT,
	UserID      TEXT,
    Date        TIMESTAMP,
	Likes       INTEGER,
	Text        TEXT,
    FOREIGN KEY (BusinessID) REFERENCES Business(BusinessID),
	FOREIGN KEY (UserID) REFERENCES Users(UserID),
    PRIMARY KEY (BusinessID, UserID, Date)
);