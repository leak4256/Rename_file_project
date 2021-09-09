
import os
import pathlib
import Application


def renameFile(oldName, newName, path):
    """
    Rename the files with oldName to the newName
    :return: True if the action success
    """
    try:
        isChange = False  # Saves if the file name has been changed
        if pathlib.Path(path).exists():  # Validate the path
            for file in os.listdir(path):  # Goes through all the directories
                # Let to get input in the formats: filename / filename.type / .type
                if file == oldName or file.split('.')[0] == oldName or oldName.startswith('.') and oldName in file:
                    os.rename(path + '\\' + file , path + '\\' + file.replace(oldName, newName))
                    isChange = True
        if isChange:
            Application.app.ACKlbl.config(text=" The name change successfully")
            return True
        else:
            Application.app.ACKlbl.config(text="path/file is not exist")
            return False

    except Exception as e:
        Application.app.ACKlbl.config(text="ERROR")
        print(e)






