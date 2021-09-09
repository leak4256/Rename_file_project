from pymongo import MongoClient
import certifi

ca = certifi.where()

link = "mongodb+srv://lea:211953211@clusterlea.tssqq.mongodb.net/ClusterLea?retryWrites=true&w=majority"
client = MongoClient(link, tlsCAFile=ca)  # Connecting to the mongodb account
db = client["Files"]  # Get "Files" db
collection = db["file"]  # Get "file" collection


def addToDB(oldName, newName, path):
    """
    Add document to the DB
    :return: 0 in success -a in failure
    """
    doc = {"id": collection.count(), "oldname": oldName, "newname": newName, "path": path}
    try:
        collection.insert_one(doc)
        return 0
    except:
        return -1


def getLastFromDB():
    """
    Returns the last document added to the DB
    :return: the last document
    """
    try:
        res = collection.find_one({"id": collection.count() - 1})
    except:
        return None
    if res:
        return res
    return None


def removeLastFromDB():
    """
    Remove the last document from the DB
    :return: -1  if an error occurs, returns -1
    """
    try:
        collection.delete_one({"id": collection.count() - 1})
    except:
        return -1


def DropCollection():
    collection.drop()
