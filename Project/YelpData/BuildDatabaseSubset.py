import json

businesses = open("./YelpBusinessSubset.json", "r")
checkins = open("./yelp_checkin.JSON", "r")
tips = open("./yelp_tip.JSON", "r")

outcheckin = open("./YelpCheckinSubset.json", "w")
outtips = open("./YelpTipSubset.json", "w")
businessdb = set()

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

businesses.close()
checkins.close()
tips.close()
outcheckin.close()
outtips.close()