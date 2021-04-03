import json

businesses = open("./YelpBusinessSubset.json", "r")
checkins = open("./yelp_checkin.JSON", "r")
tips = open("./yelp_tip.JSON", "r")
users = open("./yelp_user.json", "r")

outcheckin = open("./YelpCheckinSubset.json", "w")
outtips = open("./YelpTipSubset.json", "w")
outusers = open("./YelpUserSubset.json", "w")
businessdb = set()
userdb = set()

for line in businesses.readlines():
    business = json.loads(line)
    businessdb.add(business["business_id"])

for line in checkins.readlines():
    checkin = json.loads(line)
    if checkin["business_id"] in businessdb:
        outcheckin.write(line)

for line in tips.readlines():
    tip = json.loads(line)
    if tip["business_id"] in businessdb:
        outtips.write(line)
        userdb.add(tip["user_id"])

# for line in users.readlines():
#     user = json.loads(line)
#     if user["user_id"] in userdb:
#         friendsindb = True
#         for friend in user["friends"]:
#             if friend not in userdb:
#                 friendsindb = False
#                 break
#         if friendsindb:
#             outusers.write(line)

businesses.close()
checkins.close()
tips.close()
users.close()
outcheckin.close()
outtips.close()
outusers.close()