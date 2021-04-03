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
users = open("./Project/YelpData/yelp_user.JSON", "r")
businesses = open("./Project/YelpData/yelp_business.JSON", "r")
checkins = open('./Project/YelpData/yelp_checkin.JSON', "r")
tips = open('./Project/YelpData/yelp_tip.JSON', "r")

def DestroyPreviousDatabase():
    try:
        db = psycopg2.connect("dbname='milestone2' user='postgres' host='localhost' password='SegaSaturn'")
    except Exception as ex:
        print("Connection to database failed with error: ", ex)
        return
    
    cursor = db.cursor()
    cursor.execute("GRANT ALL ON SCHEMA public TO postgres;")
    db.commit()
    cursor.execute("GRANT ALL ON SCHEMA public TO public;")
    db.commit()
    cursor.execute("DROP SCHEMA public CASCADE;")
    db.commit()
    cursor.execute("CREATE SCHEMA public;")
    db.commit()

def BuildDatabase(schema):
    try:
        db = psycopg2.connect("dbname='milestone2' user='postgres' host='localhost' password='SegaSaturn'")
    except Exception as ex:
        print("Connection to database failed with error: ", ex)
        return
    cursor = db.cursor()
    commitval = ""

    for line in schema.readlines():
        commitval += line.replace("\n", "")
        if ";" in line:
            cursor.execute(commitval)
            db.commit()
            commitval = ""
    cursor.close()
    db.close()

def BusinessTableInsert(fin):
    try:
        db = psycopg2.connect("dbname='milestone2' user='postgres' host='localhost' password='SegaSaturn'")
    except Exception as ex:
        print("Connection to database failed with error: ", ex)
        return

    cursor = db.cursor()

    for line in fin.readlines():
        print("Attempting to push: ", line)
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
        print("\tCommit val: ", commitval)
        try:
            cursor.execute(commitval)
        except Exception as ex:
            print("Insert into Business table failed with error: ", ex)
            return
        print("\tSUCCESS\n")
        db.commit()
    cursor.close()
    db.close()

def BusinessCategoriesInsert(fin):
    try:
        db = psycopg2.connect("dbname='milestone2' user='postgres' host='localhost' password='SegaSaturn'")
    except Exception as ex:
        print("Connection to database failed with error: ", ex)
        return

    cursor = db.cursor()

    for line in fin.readlines():
        business = json.loads(line)
        print("Attempting to push: ", business["business_id"], business["categories"])

        insertString = ("INSERT INTO BusinessCategories (BusinessID, Category)")
        valString = " VALUES ('" + business["business_id"] + "', "
        
        for cat in business["categories"].split(","):
            cat = cleanStr4SQL(cat).strip()
            print("\tcategory push: " + cat)
            temp = valString + "'" + cat + "')"

            commitval = insertString + temp

            print("\tCommit val: ", commitval)
            try:
                cursor.execute(commitval)
            except Exception as ex:
                print("Insert into Business table failed with error: ", ex)
                return
            print("\tSUCCESS\n")
            db.commit()
    cursor.close()
    db.close()

def BusinessAttributesInsert(fin):
    try:
        db = psycopg2.connect("dbname='milestone2' user='postgres' host='localhost' password='SegaSaturn'")
    except Exception as ex:
        print("Connection to database failed with error: ", ex)
        return

    cursor = db.cursor()

    for line in fin.readlines():
        business = json.loads(line)
        print("Attempting to push: ", business["business_id"], business["attributes"])

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

            print("\tCommit val: ", commitval)
            try:
                cursor.execute(commitval)
            except Exception as ex:
                print("Insert into Business table failed with error: ", ex)
                return
            print("\tSUCCESS\n")
            db.commit()
    cursor.close()
    db.close()

if __name__ == "__main__":
    print("------------------------------------------------")
    print("#\t\tDestroy and Rebuild DB\t\t#")
    print("------------------------------------------------")
    DestroyPreviousDatabase()
    BuildDatabase(schema)

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