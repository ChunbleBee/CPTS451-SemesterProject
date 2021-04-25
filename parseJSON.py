import json
import psycopg2
import pandas as pd


def cleanStr4SQL(s):
    return s.replace("'", "`").replace("\n", " ")


def int2BoolStr(value):
    if value == 0:
        return 'false'
    else:
        return 'true'


schema = open("./TAZE_schema_MS2.sql")
triggers = open("./TAZE_trigger.sql")
update = open("./TAZE_UPDATE.sql")
users = open("./Project/YelpData/yelp_user.JSON", "r")
businesses = open("./Project/YelpData/yelp_business.JSON", "r")
checkins = open('./Project/YelpData/yelp_checkin.JSON', "r")
tips = open('./Project/YelpData/yelp_tip.JSON', "r")
# checkins = open("./Project/YelpData/YelpCheckinSubset.json", "r")
# tips = open("./Project/YelpData/YelpTipSubset.json", "r")
# businesses = open("./Project/YelpData/YelpBusinessSubset.json", "r")

try:
    db = psycopg2.connect("dbname='milestone2test' user='postgres' host='localhost' password='th@darncat8'")
except Exception as ex:
    print("Connection to database failed with error: ", ex)
    exit(-1)


def DestroyPreviousDatabase():
    cursor = db.cursor()
    cursor.execute(
        "GRANT ALL ON SCHEMA public TO postgres;" +
        "GRANT ALL ON SCHEMA public TO public;" +
        "DROP SCHEMA public CASCADE;" +
        "CREATE SCHEMA public;"
    )
    db.commit()


def BuildDatabase(schema):
    cursor = db.cursor()
    commitval = ""

    for line in schema.readlines():
        commitval += line.replace("\n", " ")
        if ";" in line:
            # commitval = cleanStr4SQL(commitval)
            cursor.execute(commitval)
            db.commit()
            commitval = ""
    cursor.close()


def BusinessTableInsert(fin):
    cursor = db.cursor()

    for line in fin.readlines():
        # print("Attempting to push: ", line)
        business = json.loads(line)

        insertString = ("INSERT INTO Businesses (BusinessID," +
            " BusinessName, Street, State, City, ZipCode," +
            "Longitude, Latitude, IsOpen, ReviewCount, StarRating, NumCheckins, NumTips)"
        )

        valString = " VALUES ( "
        valString += "'" + business["business_id"] + "', "
        valString += "'" + cleanStr4SQL(business["name"]) + "', "
        valString += "'" + cleanStr4SQL(business["address"]) + "', "
        valString += "'" + business["state"] + "', "
        valString += "'" + business["city"] + "', "
        valString += "'" + business["postal_code"] + "', "
        valString += "'" + str(business["longitude"]) + "', "
        valString += "'" + str(business["latitude"]) + "', "
        valString += int2BoolStr(business["is_open"]) + ", "
        valString += str(business["review_count"]) + ", "
        valString += str(business["stars"]) + ", "
        valString += "0, "  #NumCheckIns
        valString += "0)"   #NumTips

        commitval = insertString + valString
        # print("\tCommit val: ", commitval)
        try:
            cursor.execute(commitval)
        except Exception as ex:
            print("Insert into Business table failed with error: ", ex)
            exit(-1)
        # print("\tSUCCESS\n")
        db.commit()
    cursor.close()


def BusinessCategoriesInsert(fin):
    cursor = db.cursor()

    for line in fin.readlines():
        business = json.loads(line)
        # print("Attempting to push: ", business["business_id"], business["categories"])

        insertString = ("INSERT INTO BusinessCategories (BusinessID, Category)")
        valString = " VALUES ('" + business["business_id"] + "', "
        
        for cat in business["categories"].split(","):
            cat = cleanStr4SQL(cat).strip()
            temp = valString + "'" + cat + "')"

            commitval = insertString + temp

            # print("\tCommit val: ", commitval)
            try:
                cursor.execute(commitval)
            except Exception as ex:
                print("Insert into BusinessCategories table failed with error: ", ex)
                exit(-1)
            # print("\tSUCCESS\n")
            db.commit()
    cursor.close()


def BusinessAttributesInsert(fin):
    cursor = db.cursor()

    for line in fin.readlines():
        business = json.loads(line)
        # print("Attempting to push: ", business["business_id"], business["attributes"])

        insertStringSimple = "INSERT INTO BusinessAttributes (BusinessID, Attribute, Value)"
        insertStringExtended = "INSERT INTO BusinessAttributes (BusinessID, Attribute, SubTypes, Values)"
        valString = " VALUES ('" + business["business_id"] + "', '"
        arrString = "ARRAY ["
        temp = ""
        commitval = ""

        for attr in business["attributes"]:
            #print(attr, "\t", business["attributes"][attr])
            if (isinstance(business["attributes"][attr], dict)):
                subtypestr = arrString + ""
                valuesstr = arrString + ""
                for subtype, value in business["attributes"][attr].items():
                    subtypestr += "'" + cleanStr4SQL(subtype) + "', "
                    valuesstr += "'" + cleanStr4SQL(value) + "', "
                subtypestr = subtypestr[:-2] + "]"
                valuesstr = valuesstr[:-2] + "]"
                temp = (
                    valString + 
                    cleanStr4SQL(attr).strip() + "', " +
                    subtypestr + ", " +
                    valuesstr + ");"
                )
                commitval = insertStringExtended + temp

            else:
                temp =  (
                    valString +
                    cleanStr4SQL(attr).strip() + "', '" +
                    str(business["attributes"][attr])  + "');"
                )

                commitval = insertStringSimple + temp

            # print("\tCommit val: ", commitval)
            try:
                cursor.execute(commitval)
            except Exception as ex:
                print("Insert into BusinessAttributes table failed with error: ", ex)
                exit(-1)
            # print("\tSUCCESS\n")
            db.commit()
    cursor.close()


def BusinessHoursInsert(fin):
    cursor = db.cursor()

    for line in fin.readlines():
        business = json.loads(line)
        # print("Attempting to push: ", business["business_id"], business["hours"])

        insertString = "INSERT INTO BusinessHours (BusinessID, Day, OpeningTime, ClosingTime)"
        # arrString = "ARRAY["
        day = ""
        openingTimes = ""
        closingTimes = ""

        start = " VALUES ( " + "'" + business["business_id"] + "', "

        for days, hours in business["hours"].items():
            valString = start
            openingTimes = ""
            closingTimes = ""
            day = ""
            day = "'" + days + "'"
            times = hours.split('-')
            opening = times[0]
            if (opening[-2] == ':'):
                opening += '0'
            closing = times[1]
            if (closing[-2] == ':'):
                closing += '0'
            openingTimes += "'" + opening + ":00'::TIME, "
            closingTimes += "'" + closing + ":00'::TIME, "
            openingTimes = openingTimes[:-2] #+ "]"
            closingTimes = closingTimes[:-2] #+ "]"
            if (openingTimes != "" and closingTimes != "" and day != ""):
                valString += day + "," + openingTimes + "," + closingTimes + ")"
                commitval = insertString + valString
                # commitval = cleanStr4SQL(commitval)
                # print("\tCommit val: ", commitval)
                try:
                    cursor.execute(commitval)
                except Exception as ex:
                    print("Insert into BusinessHours table failed with error: ", ex)
                    exit(-1)
                # print("\tSUCCESS\n")
                db.commit()

        # day = arrString + day[:-2] + "]"
        # openingTimes = arrString + openingTimes[:-2] + "]"
        # closingTimes = arrString + closingTimes[:-2] + "]"
        # # if (openingTimes != "ARRAY[]" and closingTimes != "ARRAY[]" and day != "ARRAY[]"):
        # if (openingTimes != "" and closingTimes != "" and day != ""):
        #     valString += day + "," + openingTimes + "," + closingTimes + ")"
        #     commitval = insertString + valString
        #     # print("\tCommit val: ", commitval)
        #     try:
        #         cursor.execute(commitval)
        #     except Exception as ex:
        #         print("Insert into BusinessHours table failed with error: ", ex)
        #         exit(-1)
        #     # print("\tSUCCESS\n")
        #     db.commit()
        # else:
            # print("\tNO KNOWN HOURS, SKIPPING")
    cursor.close()


def UserTableInsert(fin):
    cursor = db.cursor()

    for line in fin.readlines():
        user = json.loads(line)
        # print("Attempting to push: ", line)

        insertString = (
            "INSERT INTO Users" +
            " (UserID, CreationDate, UserName, " +
            "FansRating, FunnyRating, CoolRating, AvgStarRating)"
        )
        valString = " VALUES ( "
        valString += "'" + user["user_id"] + "', "
        valString += "'" + user["yelping_since"] + "'::TIMESTAMP, "
        valString += "'" + cleanStr4SQL(user["name"]) + "', "
        valString += str(user["fans"]) + ", "
        valString += str(user["funny"]) + ", "
        valString += str(user["cool"]) + ", "
        valString += str(user["average_stars"]) + ")"
        commitval = insertString + valString
        # print("\tCommit val: ", commitval)
        try:
            cursor.execute(commitval)
        except Exception as ex:
            print("Insert into Users table failed with error: ", ex)
            exit(-1)
        # print("\tSUCCESS\n")
        db.commit()
    cursor.close()


def FriendsTableInsert(fin):
    cursor = db.cursor()

    for line in fin.readlines():
        user = json.loads(line)
        # print("Attempting to push: ", user["user_id"], user["friends"])

        insertString = "INSERT INTO Friends (User01, User02)"
        valString = " VALUES ( "
        valString += "'" + user["user_id"] + "', "
        for friend in user["friends"]:
            commitval = insertString + valString + "'" + friend + "');"
            # print("\tCommit val: ", commitval)
            try:
                cursor.execute(commitval)
            except Exception as ex:
                print("Insert into Business table failed with error: ", ex)
                exit(-1)
            # print("\tSUCCESS\n")
        db.commit()
    cursor.close()


def TipsTableInsert(fin):
    cursor = db.cursor()

    for line in fin.readlines():
        tip = json.loads(line)
        # print("Attempting to push: ", line)

        insertString = "INSERT INTO Tips (BusinessID, UserID, Date, Likes, Text)"
        valString = " VALUES ( "
        valString += "'" + tip["business_id"] + "', "
        valString += "'" + tip["user_id"] + "', "
        valString += "'" + tip["date"] + "'::TIMESTAMP, "
        valString += str(tip["likes"]) + ", "
        valString += "'" + cleanStr4SQL(tip["user_id"]) + "');"

        commitval = insertString + valString
        # print("\tCommit val: ", commitval)
        try:
            cursor.execute(commitval)
        except Exception as ex:
            print("Insert into Business table failed with error: ", ex)
            exit(-1)
        # print("\tSUCCESS\n")
        db.commit()
    cursor.close()


def CheckInsTableInsert(fin):
    cursor = db.cursor()

    for line in fin.readlines():
        checkin = json.loads(line)
        # print("Attempting to push: ", checkin["business_id"], checkin["date"])

        insertString = "INSERT INTO CheckIns (BusinessID, CheckInDate)"
        valString = " VALUES ( "
        valString += "'" + checkin["business_id"] + "', "

        for date in checkin["date"].split(','):
            commitval = insertString + valString + "'" + date + "'::TIMESTAMP);"
            # print("\tCommit val: ", commitval)
            try:
                cursor.execute(commitval)
                db.commit()
            except Exception as ex:
                print("Insert into CheckIns table failed with error: ", ex)
            # print("\tSUCCESS\n")
    cursor.close()


if __name__ == "__main__":
    print("------------------------------------------------")
    print("#\t\tDestroy and Rebuild DB\t\t#")
    print("------------------------------------------------")
    DestroyPreviousDatabase()
    BuildDatabase(schema)
    BuildDatabase(triggers)

    print("------------------------------------------------")
    print("#\t\tStarting Business Parse\t\t#")
    print("------------------------------------------------")
    BusinessTableInsert(businesses)
    businesses.seek(0)

    print("------------------------------------------------")
    print("#\t\tStarting Category Parse\t\t#")
    print("------------------------------------------------")
    BusinessCategoriesInsert(businesses)
    businesses.seek(0)

    print("------------------------------------------------")
    print("#\t\tStarting Attribute Parse\t\t#")
    print("------------------------------------------------")
    BusinessAttributesInsert(businesses)
    businesses.seek(0)

    print("------------------------------------------------")
    print("#\t\tStarting Hours Parse\t\t#")
    print("------------------------------------------------")
    BusinessHoursInsert(businesses)
    businesses.seek(0)

    print("------------------------------------------------")
    print("#\t\tStarting Users Parse\t\t#")
    print("------------------------------------------------")
    UserTableInsert(users)
    users.seek(0)

    print("------------------------------------------------")
    print("#\t\tStarting Friends Parse\t\t#")
    print("------------------------------------------------")
    FriendsTableInsert(users)
    users.seek(0)

    print("------------------------------------------------")
    print("#\t\tStarting Tips Parse\t\t#")
    print("------------------------------------------------")
    TipsTableInsert(tips)
    tips.seek(0)

    print("------------------------------------------------")
    print("#\t\tStarting CheckIns Parse\t\t#")
    print("------------------------------------------------")
    CheckInsTableInsert(checkins)
    checkins.seek(0)

    print("------------------------------------------------")
    print("#\t\tUpdate Database Derived Collumns\t\t#")
    print("------------------------------------------------")
    BuildDatabase(update)

    print("...\n\nCompleted without errors!")
    db.close()
